USE [ESimSol_ERP]
GO
/****** Object:  StoredProcedure [dbo].[SP_Rpt_MaternityFollowUp]    Script Date: 10/22/2018 4:56:59 PM ******/
DROP PROCEDURE [dbo].[SP_Rpt_MaternityFollowUp]
GO
/****** Object:  StoredProcedure [dbo].[SP_Rpt_LateEarlyAttendanceSummary]    Script Date: 10/22/2018 4:56:59 PM ******/
DROP PROCEDURE [dbo].[SP_Rpt_LateEarlyAttendanceSummary]
GO
/****** Object:  StoredProcedure [dbo].[SP_Rpt_IncrementByPercent]    Script Date: 10/22/2018 4:56:59 PM ******/
DROP PROCEDURE [dbo].[SP_Rpt_IncrementByPercent]
GO
/****** Object:  StoredProcedure [dbo].[SP_Rpt_HourlyMonthlyAttendance]    Script Date: 10/22/2018 4:56:59 PM ******/
DROP PROCEDURE [dbo].[SP_Rpt_HourlyMonthlyAttendance]
GO
/****** Object:  StoredProcedure [dbo].[SP_Process_UpdateComplianceAttendanceDaily]    Script Date: 10/22/2018 4:56:59 PM ******/
DROP PROCEDURE [dbo].[SP_Process_UpdateComplianceAttendanceDaily]
GO
/****** Object:  StoredProcedure [dbo].[SP_IUD_MaxOTConfigurationUser]    Script Date: 10/22/2018 4:56:59 PM ******/
DROP PROCEDURE [dbo].[SP_IUD_MaxOTConfigurationUser]
GO
/****** Object:  StoredProcedure [dbo].[SP_IUD_MaxOTConfiguration]    Script Date: 10/22/2018 4:56:59 PM ******/
DROP PROCEDURE [dbo].[SP_IUD_MaxOTConfiguration]
GO
/****** Object:  StoredProcedure [dbo].[SP_IUD_AttendanceDaily_Manual_Single_Comp]    Script Date: 10/22/2018 4:56:59 PM ******/
DROP PROCEDURE [dbo].[SP_IUD_AttendanceDaily_Manual_Single_Comp]
GO
/****** Object:  StoredProcedure [dbo].[SP_IUD_AttendanceDaily_Manual_Single_Comp]    Script Date: 10/22/2018 4:56:59 PM ******/
DROP PROCEDURE [dbo].[SP_GetEmployeeBasicAmount]
GO
/****** Object:  StoredProcedure [dbo].[SP_GetEmployeeBasicAmount]    Script Date: 10/22/2018 4:56:59 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[SP_IUD_AttendanceDaily_Manual_Single_Comp]
(
	@Param_AttendanceID AS INT
	,@Param_InTime AS DATETIME
	,@Param_OutTime AS DATETIME
	,@Param_LateArrivalMinute as int
	,@Param_EarlyDepartureMinute as int
	,@Param_IsAbsent as bit
	,@Param_IsManualOT as bit
	,@Param_OverTimeInMinute as int	
	,@Param_LeaveHeadID as int
	,@Param_IsDayOff as bit				
	,@Param_DBUserID AS INT

)

AS
BEGIN 	
	DECLARE
	@PV_EmployeeID as int 
	,@PV_StartTime as DATETIME
	,@PV_EndTime as DATETIME
	,@PV_AttendanceDate DATE
	,@PV_DayEndTime as DATETIME
	,@PV_DayStartTime as DATETIME
	,@PV_TolaranceTime as Datetime
	,@PV_ShiftID as int
	,@PV_Shift_IsOT as bit
	,@PV_Shift_OTStart as datetime
	,@PV_Shift_OTEnd   as datetime	
	,@PV_Shift_ISOTOnActual as bit
	,@PV_Shift_OTCalclateAfterInMin as int
	,@PV_Shift_IsLeaveOnOFFHoliday as bit
	,@PV_CompMaxOTInMin as int
	,@PV_LateInMin as int
	,@PV_EarlyInMin as int	
	,@PV_CompTotalWorkingHourInMin as int
	,@PV_HistoryDescription As NVarchar(500)
	,@PV_Msg as varchar(100)

	IF NOT EXISTS (SELECT * FROM AttendanceDaily WHERE AttendanceID=@Param_AttendanceID)
	BEGIN
		ROLLBACK 
			RAISERROR(N'Attendance is not processed yet for this employee.!!',16,1);
		RETURN		
	END

	--IF EXISTS (SELECT * FROM AttendanceDaily WHERE AttendanceID=@Param_AttendanceID AND IsCompLeave=1)
	--BEGIN
	--	ROLLBACK 
	--		RAISERROR(N'Already In leave.!!',16,1);
	--	RETURN		
	--END

	IF @Param_IsDayOff=1 AND @Param_LeaveHeadID>0
	BEGIN
		ROLLBACK 
			RAISERROR(N'Invalid Operation.!!',16,1);
		RETURN	
	END

	--IF @Param_InTime > @Param_OutTime 
	--BEGIN
	--	ROLLBACK 
	--		RAISERROR(N'In Time must be less than Out Time.!!',16,1);
	--	RETURN		
	--END


	SELECT @PV_EmployeeID=EmployeeID	   
		   ,@PV_AttendanceDate=AttendanceDate
	FROM AttendanceDaily WHERE AttendanceID=@Param_AttendanceID

	
	--IF EXISTS (SELECT * FROM EmployeeSalary WHERE EmployeeID=@PV_EmployeeID AND @PV_AttendanceDate BETWEEN StartDate AND EndDate)
	--BEGIN
	--	ROLLBACK 
	--		RAISERROR(N'Salary Already Processed by this date.!!',16,1);
	--	RETURN		
	--END

	IF EXISTS (SELECT * FROM AttendanceDaily WHERE AttendanceID=@Param_AttendanceID AND IsCompHoliday=1 AND @Param_InTime='00:00:00' AND @Param_OutTime='00:00:00')
	BEGIN
		ROLLBACK 
			RAISERROR(N'This day is holiday, so you can not make anyone absent in this day.!!',16,1);
		RETURN
	END

	
	SET @PV_LateInMin=0
	SET @PV_EarlyInMin=0
	SET @PV_CompTotalWorkingHourInMin=0

	--need to get actual shift
	SET @PV_ShiftID = (SELECT ShiftID FROM AttendanceDaily WHERE AttendanceID=@Param_AttendanceID)



	SELECT @PV_StartTime=(DATEADD(SECOND,0,DATEADD (MINUTE,DATEPART(MINUTE,StartTime), DATEADD(HOUR, DATEPART(HOUR,StartTime), DATEADD(dd, DATEDIFF(dd, -0, @PV_AttendanceDate), 0)))) )--CAST (StartTime AS TIME(0))
	  ,@PV_EndTime=(DATEADD(SECOND,0,DATEADD (MINUTE,DATEPART(MINUTE,EndTime), DATEADD(HOUR, DATEPART(HOUR,EndTime), DATEADD(dd, DATEDIFF(dd, -0, @PV_AttendanceDate), 0)))) )--CAST (EndTime AS TIME(0)) 	   
	  ,@PV_DayStartTime=(DATEADD(SECOND,0,DATEADD (MINUTE,DATEPART(MINUTE,DayStartTime), DATEADD(HOUR, DATEPART(HOUR,DayStartTime), DATEADD(dd, DATEDIFF(dd, -0, @PV_AttendanceDate), 0)))) )
	  ,@PV_DayEndTime=(DATEADD(SECOND,0,DATEADD (MINUTE,DATEPART(MINUTE,DayEndTime), DATEADD(HOUR, DATEPART(HOUR,DayEndTime), DATEADD(dd, DATEDIFF(dd, -0, @PV_AttendanceDate), 0)))) )
	  ,@PV_TolaranceTime=(DATEADD(SECOND,0,DATEADD (MINUTE,DATEPART(MINUTE,ToleranceTime), DATEADD(HOUR, DATEPART(HOUR,ToleranceTime), DATEADD(dd, DATEDIFF(dd, -0, @PV_AttendanceDate), 0)))) )
	  ,@PV_Shift_IsOT=IsOT
	  ,@PV_Shift_OTStart=(DATEADD(SECOND,0,DATEADD (MINUTE,DATEPART(MINUTE,OTStartTime), DATEADD(HOUR, DATEPART(HOUR,OTStartTime), DATEADD(dd, DATEDIFF(dd, -0, @PV_AttendanceDate), 0)))) )
	  ,@PV_Shift_OTEnd=(DATEADD(SECOND,0,DATEADD (MINUTE,DATEPART(MINUTE,OTEndTime), DATEADD(HOUR, DATEPART(HOUR,OTEndTime), DATEADD(dd, DATEDIFF(dd, -0, @PV_AttendanceDate), 0)))) )
	,@PV_Shift_ISOTOnActual=ISNULL(IsOTOnActual,0)
	,@PV_CompMaxOTInMin=MaxOTComplianceInMin
	,@PV_Shift_OTCalclateAfterInMin=ISNULL(OTCalculateAfterInMinute,0)
	,@PV_Shift_IsLeaveOnOFFHoliday=IsLeaveOnOFFHoliday
	FROM HRM_Shift WHERE ShiftID=@PV_ShiftID
	SET @Param_InTime=(DATEADD(SECOND,0,DATEADD (MINUTE,DATEPART(MINUTE,@Param_InTime), DATEADD(HOUR, DATEPART(HOUR,@Param_InTime), DATEADD(dd, DATEDIFF(dd, -0, @PV_AttendanceDate), 0)))) )
	SET @Param_OutTime=(DATEADD(SECOND,0,DATEADD (MINUTE,DATEPART(MINUTE,@Param_OutTime), DATEADD(HOUR, DATEPART(HOUR,@Param_OutTime), DATEADD(dd, DATEDIFF(dd, -0, @PV_AttendanceDate), 0)))) )


	--Find Shift Out time for night shift
	IF (DATEPART(HOUR,@PV_StartTime)>DATEPART(HOUR,@PV_EndTime))
	BEGIN--Night Shift or different date			
		SET @PV_EndTime=(DATEADD(SECOND,0,DATEADD (MINUTE,DATEPART(MINUTE,@PV_EndTime), DATEADD(HOUR, DATEPART(HOUR,@PV_EndTime), DATEADD(dd, DATEDIFF(dd, -1, @PV_AttendanceDate), 0)))) )
	END	ELSE BEGIN	
		SET @PV_EndTime=(DATEADD(SECOND,0,DATEADD (MINUTE,DATEPART(MINUTE,@PV_EndTime), DATEADD(HOUR, DATEPART(HOUR,@PV_EndTime), DATEADD(dd, DATEDIFF(dd, -0, @PV_AttendanceDate), 0)))) )
	END
	
	--Attendance Record for night shift
	IF (DATEPART(HOUR,@Param_InTime)>DATEPART(HOUR,@Param_OutTime) AND DATEPART(HOUR,@Param_OutTime)>0)
	BEGIN--Night Shift or different date			
		SET @Param_OutTime=(DATEADD(SECOND,0,DATEADD (MINUTE,DATEPART(MINUTE,@Param_OutTime), DATEADD(HOUR, DATEPART(HOUR,@Param_OutTime), DATEADD(dd, DATEDIFF(dd, -1, @PV_AttendanceDate), 0)))) )
	END	ELSE BEGIN	
		SET @Param_OutTime=(DATEADD(SECOND,0,DATEADD (MINUTE,DATEPART(MINUTE,@Param_OutTime), DATEADD(HOUR, DATEPART(HOUR,@Param_OutTime), DATEADD(dd, DATEDIFF(dd, -0, @PV_AttendanceDate), 0)))) )
	END

	IF @Param_IsAbsent=0
	BEGIN
		--in case of start & end time same
		IF  CONVERT (DATE,@Param_InTime)=CONVERT (DATE,@Param_OutTime)--Check Date
			AND DATEPART(HOUR,@Param_InTime)=DATEPART(HOUR,@Param_OutTime) --Hour check
			AND DATEPART(MINUTE,@Param_InTime)=DATEPART(MINUTE,@Param_OutTime)--Min Check
		BEGIN
			SET @Param_OutTime=(SELECT DATEADD(SECOND,0,DATEADD (MINUTE,0, DATEADD(HOUR, 0, DATEADD(dd, DATEDIFF(dd, -0, @PV_DayEndTime), 0)))))
			--find early
			SET @PV_EarlyInMin=DATEDIFF(MINUTE,@Param_InTime,@PV_EndTime)
		END	

		--In case of 00:00 hour (12 AM)
		IF DATEPART(HOUR,@Param_InTime)=0 AND DATEPART(MINUTE,@Param_InTime)=0 AND DATEPART(SECOND,@Param_InTime)!=0 
		BEGIN
			SET @Param_InTime=(SELECT DATEADD(SECOND,0,DATEADD (MINUTE,1, DATEADD(HOUR, 0, DATEADD(dd, DATEDIFF(dd, -0, @Param_InTime), 0)))))
		END
		--In case of 00:00 hour (12 AM)
		IF DATEPART(HOUR,@Param_OutTime)=0 AND DATEPART(MINUTE,@Param_OutTime)=0 AND DATEPART(SECOND,@Param_OutTime)!=0 
		BEGIN
			SET @Param_OutTime=(SELECT DATEADD(SECOND,0,DATEADD (MINUTE,1, DATEADD(HOUR, 0, DATEADD(dd, DATEDIFF(dd, -0, @Param_OutTime), 0)))))
		END

		--Late	
		IF @Param_InTime>@PV_TolaranceTime
		BEGIN
			SET @PV_LateInMin=DATEDIFF(MINUTE,@PV_StartTime,@Param_InTime)
		END
		IF @Param_LateArrivalMinute=0
		BEGIN
			SET @PV_LateInMin=@Param_LateArrivalMinute
		END

		IF CAST(@Param_OutTime AS TIME(0))!='00:00' AND @Param_OutTime<@PV_EndTime
		BEGIN
			SET @PV_EarlyInMin=DATEDIFF(MINUTE,@Param_OutTime,@PV_EndTime)
		END
		--find early
		IF CAST(@Param_OutTime AS TIME(0))='00:00' AND CAST(@Param_InTime AS TIME(0))='00:00'
		BEGIN
			SET @PV_EarlyInMin=0
		END
		IF @Param_EarlyDepartureMinute=0
		BEGIN
			SET @PV_EarlyInMin=@Param_EarlyDepartureMinute
		END		
	END ELSE BEGIN	
		UPDATE [AttendanceDaily] SET IsCompDayOff=0,IsCompHoliday=0 WHERE AttendanceID= @Param_AttendanceID
		SET @PV_LateInMin=0
		SET @PV_EarlyInMin=0
		SET @Param_OverTimeInMinute=0
	END

	SET @PV_LateInMin= CASE WHEN @PV_LateInMin<0 THEN 0 ELSE @PV_LateInMin END
	SET @PV_EarlyInMin=CASE WHEN @PV_EarlyInMin<0 THEN 0 ELSE @PV_EarlyInMin END
	SET @Param_OverTimeInMinute=CASE WHEN @Param_OverTimeInMinute<0 THEN @Param_OverTimeInMinute END


	/***************Set Description************************************/
	SET @PV_HistoryDescription='Comp:'
	SET @PV_Msg=''
	IF NOT EXISTS (SELECT * FROM AttendanceDaily WHERE AttendanceID=@Param_AttendanceID AND ShiftID=@PV_ShiftID)
	BEGIN
		SET @PV_Msg=''
		SET @PV_Msg=(SELECT HRM_Shift FROM View_AttendanceDaily WHERE AttendanceID=@Param_AttendanceID)
		SET @PV_HistoryDescription +='Shift-'+@PV_Msg+','
	END

	IF NOT EXISTS (SELECT * FROM AttendanceDaily WHERE AttendanceID=@Param_AttendanceID AND CompLateArrivalMinute=@PV_LateInMin)
	BEGIN
		SET @PV_Msg=''
		SET @PV_Msg=(SELECT CONVERT(VARCHAR(50),CompLateArrivalMinute) FROM AttendanceDaily WHERE AttendanceID=@Param_AttendanceID)
		SET @PV_HistoryDescription +='Late-'+@PV_Msg+','
	END
	IF NOT EXISTS (SELECT * FROM AttendanceDaily WHERE AttendanceID=@Param_AttendanceID AND CompEarlyDepartureMinute=@PV_EarlyInMin)
	BEGIN
		SET @PV_Msg=''
		SET @PV_Msg=(SELECT CONVERT(VARCHAR(50),CompEarlyDepartureMinute) FROM AttendanceDaily WHERE AttendanceID=@Param_AttendanceID)
		SET @PV_HistoryDescription +='Early-'+@PV_Msg+','
	END
	

	IF NOT EXISTS (SELECT * FROM AttendanceDaily WHERE AttendanceID=@Param_AttendanceID AND IsCompDayOff=@param_IsDayoff)
	BEGIN
		SET @PV_Msg=''
		SET @PV_HistoryDescription +='Dayoff-'+@PV_Msg+','
	END

	IF NOT EXISTS (SELECT * FROM AttendanceDaily WHERE AttendanceID=@Param_AttendanceID AND CompInTime=@Param_InTime)
	BEGIN
		SET @PV_Msg='IN'
	END

	IF NOT EXISTS (SELECT * FROM AttendanceDaily WHERE AttendanceID=@Param_AttendanceID AND CompOutTime=@Param_OutTime)
	BEGIN
		SET @PV_Msg='OUT'		
	END



	IF @PV_Msg IN ('IN','OUT')
	BEGIN
		SET @PV_Msg=(SELECT CONVERT(Varchar(5), CAST(CompInTime AS TIME (0)))+'-'+CONVERT(Varchar(5), CAST(CompOutTime as TIME (0))) 
					FROM AttendanceDaily WHERE AttendanceID=@Param_AttendanceID)
		SET @PV_HistoryDescription +='Att-'+@PV_Msg+','		
	END

	IF @PV_HistoryDescription='Comp:'
	BEGIN
		SET @PV_HistoryDescription='No Update'
	END ELSE BEGIN
		--SET @PV_HistoryDescription='Update On: '+@PV_HistoryDescription
		SET @PV_HistoryDescription=SUBSTRING( @PV_HistoryDescription ,0 ,LEN(@PV_HistoryDescription))  
	END
	INSERT INTO [AttendanceDailyManualHistory]   ([AttendanceID],[Description],[DBUSerID],[DBServerDateTime])
     VALUES    (@Param_AttendanceID,@PV_HistoryDescription,@Param_DBUserID,GETDATE())	
	IF @Param_LeaveHeadID > 0
	BEGIN
	UPDATE [AttendanceDaily]
	SET
		[CompInTime]=dateadd(DAY, DATEDIFF(DAY, 0,@Param_InTime), 0)
		,[CompOutTime]=dateadd(DAY, DATEDIFF(DAY, 0,@Param_OutTime), 0)
		,[CompLateArrivalMinute]=0
		,[CompEarlyDepartureMinute]=0
		,[CompTotalWorkingHourInMinute]=0
		,[CompOverTimeInMinute]=0
		,IsManual=1
		,IsManualOT=0
		,[IsCompDayOff] = 0
		,[IsCompLeave] = 1
		,[CompLeaveHeadID] = @Param_LeaveHeadID
		WHERE AttendanceID= @Param_AttendanceID
	END ELSE 
	BEGIN
	UPDATE [AttendanceDaily]
	SET 
		[CompInTime]=@Param_InTime
		,[CompOutTime]=@Param_OutTime
		,[CompLateArrivalMinute]=@PV_LateInMin
		,[CompEarlyDepartureMinute]=@PV_EarlyInMin
		,[CompOverTimeInMinute]=@Param_OverTimeInMinute
		,IsManual=1
		,IsManualOT=@Param_IsManualOT
		,[IsCompDayOff] = @Param_IsDayOff
		,[CompTotalWorkingHourInMinute]=CASE WHEN (DATEDIFF(MINUTE,@Param_InTime,@Param_OutTime))<0 THEN 0 ELSE DATEDIFF(MINUTE,@Param_InTime,@Param_OutTime) END
		,[CompLeaveHeadID] = 0--@Param_LeaveHeadID
		,[IsCompLeave] = 0
		WHERE AttendanceID= @Param_AttendanceID
	END

	

	SELECT * FROM View_AttendanceDaily WHERE AttendanceID= @Param_AttendanceID
END





GO
/****** Object:  StoredProcedure [dbo].[SP_IUD_MaxOTConfiguration]    Script Date: 10/22/2018 4:56:59 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[SP_IUD_MaxOTConfiguration]
	@Param_MOCID AS INT
	,@param_MaxOTInMin AS INT

	,@Param_TimeCardName AS VARCHAR(512)
	,@Param_ExtraOT AS VARCHAR(512)
	,@Param_PaySlip AS VARCHAR(512)
	,@Param_IsPresentOnDayOff AS BIT
	,@Param_IsPresentOnHoliday AS BIT
	,@Param_MaxOutTime AS DATETIME
	,@Param_MinInTime AS DATETIME

	,@Param_DBOperation AS INT
	,@Param_DBUserID AS INT
AS
BEGIN

IF @Param_DBOperation = 1
BEGIN
	DECLARE @PV_Sequence AS INT
	IF EXISTS(SELECT * FROM MaxOTConfiguration WHERE MOCID = @Param_MOCID)
	BEGIN
		ROLLBACK
			RAISERROR (N'Already a configuration exists',16,1);	
		RETURN
	END
	SET @Param_MOCID = (SELECT ISNULL(MAX(MOCID),0)+1 FROM MaxOTConfiguration)
	SET @PV_Sequence = (SELECT ISNULL(MAX(Sequence),0)+1 FROM MaxOTConfiguration)
	INSERT INTO MaxOTConfiguration(MOCID,		   MaxOTInMin,		   DBUserID,		DBServerDateTime ,Sequence,     TimeCardName,        ExtraOT,        PaySlip,        IsPresentOnDayOff, IsPresentOnHoliday, MaxOutTime, MinInTime)
							VALUES(@Param_MOCID,   @param_MaxOTInMin,  @Param_DBUserID, GETDATE(),        @PV_Sequence, @Param_TimeCardName, @Param_ExtraOT, @Param_PaySlip, @Param_IsPresentOnDayOff, @Param_IsPresentOnHoliday, @Param_MaxOutTime, @Param_MinInTime  )

	SELECT * FROM MaxOTConfiguration WHERE MOCID = @Param_MOCID
END

IF @Param_DBOperation = 3
BEGIN
	DELETE FROM MaxOTConfiguration WHERE MOCID = @Param_MOCID
	SELECT * FROM MaxOTConfiguration WHERE MOCID = @Param_MOCID
END

END
GO
/****** Object:  StoredProcedure [dbo].[SP_IUD_MaxOTConfigurationUser]    Script Date: 10/22/2018 4:56:59 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[SP_IUD_MaxOTConfigurationUser]
(
	@Param_MOCUID as int,
	@Param_MOCID as int,
	@Param_UserID as int,
	@Param_DBUserID as int,
	@Param_DBOperation as smallint,
	@Param_IsUserBased as bit,
	@Oaram_ids as varchar(MAX)
)
	
AS
BEGIN TRAN
DECLARE 
@PV_DBServerDateTime as datetime
SET @PV_DBServerDateTime=Getdate()
IF(@Param_DBOperation=1)
BEGIN			
		IF EXISTS(SELECT * FROM MaxOTConfigurationUser WHERE MOCID = @Param_MOCUID AND UserID = @Param_UserID)
		BEGIN
			ROLLBACK
				RAISERROR (N'Selected Time Card Already Assign for Selected User!!',16,1);	
			RETURN
		END	
		
		SET @Param_MOCUID=(SELECT ISNULL(MAX(MOCUID),0)+1 FROM MaxOTConfigurationUser)		
	
		INSERT INTO MaxOTConfigurationUser	(MOCUID,			MOCID,				UserID)
    									VALUES	(@Param_MOCUID,		@Param_MOCID,			@Param_UserID)    					    				  
    	SELECT * FROM MaxOTConfigurationUser WHERE MOCUID= @Param_MOCUID
END

IF(@Param_DBOperation=2)
BEGIN
	IF(@Param_MOCUID<=0)
	BEGIN
		ROLLBACK
			RAISERROR (N'Your Selected Timecard Is Invalid Please Refresh and try again!!',16,1);	
		RETURN
	END	
	Update MaxOTConfigurationUser  SET MOCUID = @Param_MOCUID ,			MOCID =@Param_MOCID,				UserID =@Param_UserID  WHERE MOCUID= @Param_MOCUID
	SELECT * FROM MaxOTConfigurationUser WHERE MOCUID= @Param_MOCUID
END

IF(@Param_DBOperation=3)
BEGIN
	
	IF(@Param_IsUserBased=1)
	BEGIN
		DELETE FROM MaxOTConfigurationUser WHERE UserID = @Param_UserID AND MOCID NOT IN (SELECT * FROM dbo.SplitInToDataSet(@Oaram_ids,','))
	END
	ELSE
	BEGIN
		DELETE FROM MaxOTConfigurationUser WHERE MOCID = @Param_MOCID AND UserID NOT IN (SELECT * FROM dbo.SplitInToDataSet(@Oaram_ids,','))
	END
	
	
END
COMMIT TRAN







GO
/****** Object:  StoredProcedure [dbo].[SP_Process_UpdateComplianceAttendanceDaily]    Script Date: 10/22/2018 4:56:59 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[SP_Process_UpdateComplianceAttendanceDaily]
(
	--DECLARE
	@Param_StartDate DATE
	,@Param_EndDate DATE
	,@Param_EmployeeID INT
	,@Param_BufferTime INT
	,@Param_IsOverTime BIT
	,@Param_UserID INT
)
AS
BEGIN 
	DECLARE
	@PV_StartTime as DATETIME
	,@PV_EndTime as DATETIME
	,@PV_DayEndTime as DATETIME
	,@PV_DayStartTime as DATETIME
	,@PV_TolaranceTime as Datetime
	,@PV_ShiftID as int
	,@PV_Shift_IsOT as bit
	,@PV_Shift_OTStart as datetime
	,@PV_Shift_OTEnd   as datetime	
	,@PV_Shift_ISOTOnActual as bit
	,@PV_Shift_OTCalclateAfterInMin as int
	,@PV_Shift_IsLeaveOnOFFHoliday as bit
	,@PV_CompMaxOTInMin as int
	,@PV_LateInMin as int
	,@PV_EarlyInMin as int	
	,@PV_TotalWorkingHourInMin as int
	,@PV_HistoryDescription As NVarchar(500)
	,@PV_IsHoliday as bit
	,@PV_IsDayoff as bit
	,@PV_MaxOTInMinATRoster as int
	,@PV_AttendanceSchemeID as int
	,@PV_EmployeeID as int
	,@PV_OverTimeInMin as int
	,@PV_IsAbsent as bit
	,@PV_IsLeave as bit
	,@PV_AttendanceID as int
	,@PV_AttendanceDate as date
	,@PV_InTime as datetime
	,@PV_OutTime as datetime
	,@PV_IsCompLeave BIT
	,@PV_IsCompHoliday BIT
	,@PV_IsCompDayOff BIT
	
	DECLARE @tbl_BreakTime AS TABLE (BStart DateTime, BEnd DAtetime, BDurationMin int)

	--IF NOT EXISTS(SELECT * FROM AttendanceDaily WHERE AttendanceID = @PV_AttendanceID)
	--BEGIN
	--	ROLLBACK
	--		RAISERROR (N'No Attendance Found!!~',16,1);	
	--	RETURN
	--END
	
	--SELECT @PV_EmployeeID=EmployeeID	   
	--	   ,@PV_AttendanceDate=AttendanceDate
	--	   ,@PV_AttendanceSchemeID=AttendanceSchemeID
	--FROM AttendanceDaily WHERE AttendanceID=@PV_AttendanceID
	
	--SET @PV_IsAbsent = 0
	--IF EXISTS(SELECT * FROM AttendanceDaily WHERE AttendanceID = @PV_AttendanceID AND (CAST(CompInTime AS TIME(0))) = '00:00:00' AND (CAST(CompOutTime AS TIME(0))) = '00:00:00'  AND IsCompLeave=0 AND IsCompDayOff=0)
	--BEGIN
	--	SET @PV_IsAbsent = 1
	--END

	--IF EXISTS (SELECT * FROM AttendanceDaily WHERE AttendanceID=@PV_AttendanceID AND ISELProcess=1)
	--BEGIN
	--	IF NOT EXISTS (SELECT * FROM ELSetup WHERE IsConsiderDayOff=1 AND IsConsiderAbsent=1 AND IsConsiderDayOff=1 AND IsConsiderHoliday=1)
	--	BEGIN
	--		ROLLBACK 
	--			RAISERROR(N'EL Processed by This date. You cant edit.!!',16,1);
	--		RETURN				
	--	END
	--END

	----Salary Check
	--IF EXISTS (SELECT * FROM EmployeeSalary WHERE EmployeeID=@PV_EmployeeID AND @PV_AttendanceDate BETWEEN StartDate AND EndDate)
	--BEGIN
	--	ROLLBACK 
	--		RAISERROR(N'Salary Already Processed by this date.!!',16,1);
	--	RETURN		
	--END

	--IF EXISTS (SELECT * FROM AttendanceDaily WHERE AttendanceID=@PV_AttendanceID AND IsHoliday=1 AND @PV_InTime='00:00' AND @PV_OutTime='00:00')
	--BEGIN
	--	ROLLBACK 
	--		RAISERROR(N'This day is holiday, so you can not make anyone absent in this day.!!',16,1);
	--	RETURN
	--END
	IF OBJECT_ID('tempdb..#tbl_Att') IS NOT NULL
	BEGIN
		DROP TABLE #tbl_Att
	END


	CREATE TABLE #tbl_Att(AttendanceID INT, AttendanceDate DATE, EmployeeID INT, ShiftID INT, CompInTime DATETIME, CompOutTime DATETIME, IsCompLeave BIT, IsCompHoliday BIT, IsCompDayOff BIT)
	INSERT INTO #tbl_Att
	SELECT AD.AttendanceID
		  ,AD.AttendanceDate
		  ,AD.EmployeeID
		  ,AD.ShiftID
		  ,AD.CompInTime
		  ,AD.CompOutTime
		  ,AD.IsCompLeave
		  ,AD.IsCompHoliday
		  ,AD.IsCompDayOff
	FROM AttendanceDaily AD
	WHERE AD.EmployeeID = @Param_EmployeeID AND AD.AttendanceDate BETWEEN @Param_StartDate AND @Param_EndDate

	--select * From #tbl_Att ORDER BY AttendanceDate

	DECLARE Cur_CC CURSOR LOCAL FORWARD_ONLY KEYSET FOR
	SELECT AttendanceID,AttendanceDate,EmployeeID,ShiftID,CompInTime,CompOutTime, IsCompLeave, IsCompHoliday, IsCompDayOff FROM #tbl_Att	
	OPEN Cur_CC
	FETCH NEXT FROM Cur_CC INTO @PV_AttendanceID,@PV_AttendanceDate,@PV_EmployeeID,@PV_ShiftID,@PV_InTime,@PV_OutTime, @PV_IsCompLeave, @PV_IsCompHoliday, @PV_IsCompDayOff
	WHILE(@@Fetch_Status <> -1)
	BEGIN

		IF EXISTS(SELECT * FROM AttendanceDaily WHERE AttendanceID = @PV_AttendanceID AND (CAST(CompInTime AS TIME(0))) = '00:00:00' AND (CAST(CompOutTime AS TIME(0))) = '00:00:00'  AND IsCompLeave=0 AND IsCompDayOff=0)
		BEGIN
			GOTO CONT;
		END

		IF (@PV_IsCompLeave = 1 OR @PV_IsCompHoliday = 1 OR @PV_IsCompDayOff = 1)
		BEGIN
			GOTO CONT;
		END

		SET @PV_LateInMin=0
		SET @PV_EarlyInMin=0
		SET @PV_TotalWorkingHourInMin=0
		SET @PV_OverTimeInMin = 0

		SELECT @PV_StartTime=(DATEADD(SECOND,0,DATEADD (MINUTE,DATEPART(MINUTE,StartTime), DATEADD(HOUR, DATEPART(HOUR,StartTime), DATEADD(dd, DATEDIFF(dd, -0, @PV_AttendanceDate), 0)))) )--CAST (StartTime AS TIME(0))
		  ,@PV_EndTime=(DATEADD(SECOND,0,DATEADD (MINUTE,DATEPART(MINUTE,EndTime), DATEADD(HOUR, DATEPART(HOUR,EndTime), DATEADD(dd, DATEDIFF(dd, -0, @PV_AttendanceDate), 0)))) )--CAST (EndTime AS TIME(0)) 	   
		  ,@PV_DayStartTime=(DATEADD(SECOND,0,DATEADD (MINUTE,DATEPART(MINUTE,DayStartTime), DATEADD(HOUR, DATEPART(HOUR,DayStartTime), DATEADD(dd, DATEDIFF(dd, -0, @PV_AttendanceDate), 0)))) )
		  ,@PV_DayEndTime=(DATEADD(SECOND,0,DATEADD (MINUTE,DATEPART(MINUTE,DayEndTime), DATEADD(HOUR, DATEPART(HOUR,DayEndTime), DATEADD(dd, DATEDIFF(dd, -0, @PV_AttendanceDate), 0)))) )
		  ,@PV_TolaranceTime=(DATEADD(SECOND,0,DATEADD (MINUTE,DATEPART(MINUTE,ToleranceTime), DATEADD(HOUR, DATEPART(HOUR,ToleranceTime), DATEADD(dd, DATEDIFF(dd, -0, @PV_AttendanceDate), 0)))) )
		  ,@PV_Shift_IsOT=IsOT
		  ,@PV_Shift_OTStart=(DATEADD(SECOND,0,DATEADD (MINUTE,DATEPART(MINUTE,OTStartTime), DATEADD(HOUR, DATEPART(HOUR,OTStartTime), DATEADD(dd, DATEDIFF(dd, -0, @PV_AttendanceDate), 0)))) )
		  ,@PV_Shift_OTEnd=(DATEADD(SECOND,0,DATEADD (MINUTE,DATEPART(MINUTE,OTEndTime), DATEADD(HOUR, DATEPART(HOUR,OTEndTime), DATEADD(dd, DATEDIFF(dd, -0, @PV_AttendanceDate), 0)))) )
		,@PV_Shift_ISOTOnActual=ISNULL(IsOTOnActual,0)
		,@PV_CompMaxOTInMin=MaxOTComplianceInMin
		,@PV_Shift_OTCalclateAfterInMin=ISNULL(OTCalculateAfterInMinute,0)
		,@PV_Shift_IsLeaveOnOFFHoliday=IsLeaveOnOFFHoliday
		FROM HRM_Shift WHERE ShiftID=@PV_ShiftID
		
		IF (DATEPART(HOUR,@PV_StartTime)>DATEPART(HOUR,@PV_EndTime))
		BEGIN--Night Shift or different date			
			SET @PV_EndTime=(DATEADD(SECOND,0,DATEADD (MINUTE,DATEPART(MINUTE,@PV_EndTime), DATEADD(HOUR, DATEPART(HOUR,@PV_EndTime), DATEADD(dd, DATEDIFF(dd, -1, @PV_AttendanceDate), 0)))) )
		END	ELSE BEGIN	
			SET @PV_EndTime=(DATEADD(SECOND,0,DATEADD (MINUTE,DATEPART(MINUTE,@PV_EndTime), DATEADD(HOUR, DATEPART(HOUR,@PV_EndTime), DATEADD(dd, DATEDIFF(dd, -0, @PV_AttendanceDate), 0)))) )
		END
	
		--Attendance Record for night shift
		IF (DATEPART(HOUR,@PV_InTime)>DATEPART(HOUR,@PV_OutTime) AND DATEPART(HOUR,@PV_OutTime)>0)
		BEGIN--Night Shift or different date			
			SET @PV_OutTime=(DATEADD(SECOND,0,DATEADD (MINUTE,DATEPART(MINUTE,@PV_OutTime), DATEADD(HOUR, DATEPART(HOUR,@PV_OutTime), DATEADD(dd, DATEDIFF(dd, -1, @PV_AttendanceDate), 0)))) )
		END	ELSE BEGIN	
			SET @PV_OutTime=(DATEADD(SECOND,0,DATEADD (MINUTE,DATEPART(MINUTE,@PV_OutTime), DATEADD(HOUR, DATEPART(HOUR,@PV_OutTime), DATEADD(dd, DATEDIFF(dd, -0, @PV_AttendanceDate), 0)))) )
		END
		

		SET @PV_InTime = CASE WHEN CAST(@PV_InTime AS TIME(0))< CAST(@PV_StartTime AS TIME(0)) THEN DATEADD(MINUTE,RAND()*(-@Param_BufferTime),@PV_StartTime) ELSE @PV_InTime END
		SET @PV_OutTime = CASE WHEN CAST(@PV_OutTime AS TIME(0))>CAST(@PV_EndTime AS TIME(0)) THEN DATEADD(MINUTE,RAND()*(@Param_BufferTime),@PV_EndTime) ELSE @PV_OutTime END

		--select @PV_AttendanceID, @PV_InTime, @PV_OutTime, @PV_StartTime, @PV_EndTime
		
		SET @PV_TotalWorkingHourInMin = CASE WHEN (DATEDIFF(MINUTE,@PV_InTime,@PV_OutTime))<0 THEN 0 ELSE DATEDIFF(MINUTE,@PV_InTime,@PV_OutTime) END

		IF @Param_IsOverTime=1
		BEGIN
			--Break Times
			INSERT @tbl_BreakTime
			SELECT BStart,BEnd,DATEDIFF(MINUTE,BStart,BEnd) FROM 
			(SELECT (DATEADD(SECOND,0,DATEADD (MINUTE,DATEPART(MINUTE,StartTime), DATEADD(HOUR, DATEPART(HOUR,StartTime), DATEADD(dd, DATEDIFF(dd, -0, CASE WHEN DATEPART(HOUR,@PV_DayStartTime)<DATEPART(HOUR,StartTime) THEN @PV_AttendanceDate ELSE DATEADD(DAY,1,@PV_AttendanceDate) END), 0)))) ) AS BStart 
			,(DATEADD(SECOND,0,DATEADD (MINUTE,DATEPART(MINUTE,EndTime), DATEADD(HOUR, DATEPART(HOUR,EndTime), DATEADD(dd, DATEDIFF(dd, -0, CASE WHEN DATEPART(HOUR,@PV_DayStartTime)<DATEPART(HOUR,EndTime) THEN @PV_AttendanceDate ELSE DATEADD(DAY,1,@PV_AttendanceDate) END), 0)))) ) AS BEnd
			FROM ShiftBreakSchedule WHERE ShiftID=@PV_ShiftID) ss


			DECLARE @PV_OverTimeCalculateInMinAfter as int, @PV_OTMin as int, @PV_TotalWHourForOT as int
					, @PV_IsOTFromDayStart as bit, @PV_BrkTimeInMin as int , @PV_BrkTime as int

			SET @PV_TotalWorkingHourInMin = CASE WHEN (DATEDIFF(MINUTE,@PV_InTime,@PV_OutTime))<0 THEN 0 ELSE DATEDIFF(MINUTE,@PV_InTime,@PV_OutTime) END

			SET @PV_OTMin=0
			SET @PV_BrkTime=0

			IF (@PV_TotalWorkingHourInMin>0)
			BEGIN--Workig hour
				IF (@PV_Shift_IsOT=0)
				BEGIN--From scheme
					--get overtime policy from attendance scheme
					SELECT @PV_OverTimeCalculateInMinAfter=OvertimeCalculateInMinuteAfter, @PV_IsOTFromDayStart=IsOTCalTimeStartFromShiftStart,@PV_BrkTimeInMin=BreakageTimeInMinute 
					FROM AttendanceScheme WHERE AttendanceSchemeID=@PV_AttendanceSchemeID 

					IF (@PV_OverTimeCalculateInMinAfter>0)
					BEGIN--Att Scheme			
						SET @PV_TotalWHourForOT =@PV_TotalWorkingHourInMin							
						IF (@PV_IsOTFromDayStart=1)
						BEGIN
							SET @PV_TotalWHourForOT=DATEDIFF(MINUTE,@PV_StartTime,@PV_OutTime)
						END				
						SELECT @PV_OTMin=@PV_TotalWHourForOT-@PV_OverTimeCalculateInMinAfter
						IF (@PV_OTMin<@PV_BrkTimeInMin)
						BEGIN
							SET @PV_OTMin=0
						END
					END
				END--From Scheme
				ELSE BEGIN--From Shift	
					--IF OT Begain before Shift Start
					IF @PV_Shift_OTStart<@PV_StartTime
					BEGIN--Before
						IF @PV_InTime>@PV_Shift_OTStart
						BEGIN
							SET @PV_OTMin=DATEDIFF(MINUTE,@PV_InTime,CASE WHEN @PV_Shift_OTEnd>@PV_OutTime THEN @PV_OutTime ELSE @PV_Shift_OTEnd END)+1												
						END
						ELSE BEGIN
							SET @PV_OTMin=DATEDIFF(MINUTE,@PV_Shift_OTStart,CASE WHEN @PV_Shift_OTEnd>@PV_OutTime THEN @PV_OutTime ELSE @PV_Shift_OTEnd END)+1
						END
					END--Before
					--IF OT Begain After Shift End
					IF @PV_Shift_OTStart>=@PV_EndTime 
					BEGIN--After
						IF (@PV_OutTime<@PV_Shift_OTEnd)
						BEGIN
							SET @PV_OTMin=DATEDIFF(MINUTE,@PV_Shift_OTStart,@PV_OutTime)+1	
						END
						ELSE BEGIN
							SET @PV_OTMin=DATEDIFF(MINUTE,@PV_Shift_OTStart,@PV_Shift_OTEnd)+1	
						END						
					END--After
					--Check break time if has
					IF EXISTS (SELECT * FROM @tbl_BreakTime WHERE BStart BETWEEN @PV_Shift_OTStart AND  @PV_Shift_OTEnd)
					BEGIN
						--SET @PV_BrkTime=ISNULL((SELECT SUM(BDurationMin) FROM @tbl_BreakTime WHERE BStart BETWEEN 
						--CASE WHEN @PV_ActualStart>@PV_Shift_OTStart THEN @PV_ActualStart ELSE  @PV_Shift_OTStart END
						--AND  CASE WHEN @PV_Shift_OTEnd>@PV_ActualEnd THEN @PV_ActualEnd ELSE @PV_Shift_OTEnd END),0)

						SET @PV_BrkTime=ISNULL((SELECT SUM(DATEDIFF(MINUTE,CASE WHEN BStart>@PV_InTime THEN BStart ELSE  @PV_InTime END,
										CASE WHEN BEnd>@PV_OutTime THEN @PV_OutTime ELSE BEnd END) ) FROM @tbl_BreakTime WHERE BStart BETWEEN 
										CASE WHEN @PV_InTime>@PV_Shift_OTStart THEN @PV_InTime ELSE  @PV_Shift_OTStart END
										AND  CASE WHEN @PV_Shift_OTEnd>@PV_OutTime THEN @PV_OutTime ELSE @PV_Shift_OTEnd END),0)
						IF @PV_BrkTime<0 BEGIN SET @PV_BrkTime=0 END
					END 

					SET @PV_OTMin=@PV_OTMin-@PV_BrkTime
					IF (@PV_OTMin>@PV_Shift_OTCalclateAfterInMin)
					BEGIN
						IF (@PV_Shift_ISOTOnActual=0)
						BEGIN
							IF NOT EXISTS (SELECT * FROM ShiftOTSlab WHERE ShiftID=@PV_ShiftID)
							BEGIN
								SET @PV_OTMin=DATEDIFF(MINUTE,@PV_Shift_OTStart,@PV_Shift_OTEnd)-@PV_BrkTime
							END
							ELSE BEGIN
								SET @PV_OTMin=ISNULL((SELECT AchieveOTInMin FROM ShiftOTSlab WHERE IsActive=1 AND ShiftID=@PV_ShiftID 
												AND @PV_OTMin BETWEEN MinOTInMin AND MaxOTInMin),0)
							END
						END
					END
					ELSE BEGIN
						SET @PV_OTMin=0
					END
				END--From Shift

				---Final OT after checking Max OT from roster
				IF (@PV_OTMin>0 AND @PV_MaxOTInMinATRoster>0 AND @PV_OTMin>@PV_MaxOTInMinATRoster)
				BEGIN
					SET @PV_OTMin=@PV_MaxOTInMinATRoster
				END
			END--WorkingHour

			SET @PV_OverTimeInMin=@PV_OTMin
		END

		UPDATE AttendanceDaily
		SET [CompInTime]=@PV_InTime
			,[CompOutTime]=@PV_OutTime
			,[CompLateArrivalMinute]=@PV_LateInMin
			,[CompEarlyDepartureMinute]=@PV_EarlyInMin
			,[CompOverTimeInMinute]=@PV_OverTimeInMin
			,IsManual=1
			,[CompTotalWorkingHourInMinute]=@PV_TotalWorkingHourInMin
			WHERE AttendanceID= @PV_AttendanceID

		CONT:
	FETCH NEXT FROM Cur_CC INTO @PV_AttendanceID,@PV_AttendanceDate,@PV_EmployeeID,@PV_ShiftID,@PV_InTime,@PV_OutTime, @PV_IsCompLeave, @PV_IsCompHoliday, @PV_IsCompDayOff
	END--
	CLOSE Cur_CC
	DEALLOCATE Cur_CC
	
	DROP TABLE #tbl_Att

	SELECT TOP(1) * FROM View_AttendanceDaily WHERE EmployeeID = @Param_EmployeeID
END 
GO
/****** Object:  StoredProcedure [dbo].[SP_Rpt_HourlyMonthlyAttendance]    Script Date: 10/22/2018 4:56:59 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[SP_Rpt_HourlyMonthlyAttendance]
(
	--DECLARE
	@Param_EmployeeIDs VARCHAR(max),
	@Param_BusinessUnitIds VARCHAR(max),
	@Param_LocationIDs VARCHAR(max),
	@Param_DepartmentIds VARCHAR(max),
	@Param_sDesignationIds VARCHAR(max),
	@Param_SalarySchemeIDs VARCHAR(max),		
	@Param_DateFrom AS DATE,
	@Param_DateTo AS DATE,
	@Param_WorkingStatus VARCHAR(50),
	@Param_UserId INT,
	@Param_sGroupIDs VARCHAR(MAX),
	@Param_sBlockIDs VARCHAR(MAX),
	@Param_StartSalaryRange DECIMAL(30, 17),
	@Param_EndSalaryRange DECIMAL(30, 17),
	@Param_ShiftIDs VARCHAR(MAX),
	@Param_Remarks VARCHAR(MAX)
)
AS
BEGIN 
	DECLARE 
	@sSQL as nvarchar (max)
	
	IF OBJECT_ID('tempdb..#tbl_EmployeeID') IS NOT NULL
	BEGIN
		DROP TABLE #tbl_EmployeeID
	END
	IF OBJECT_ID('tempdb..#tbl_EmployeeBasic') IS NOT NULL
	BEGIN
		DROP TABLE #tbl_EmployeeBasic
	END
	
	IF OBJECT_ID('tempdb..#tbl_AttReport') IS NOT NULL
	BEGIN
		DROP TABLE #tbl_AttReport
	END
	
	CREATE TABLE #tbl_EmployeeID(EmployeeID INT, ShiftID INT, AttendanceDate DATE, InTime DATETIME, OutTime DATETIME, TotalWorkingHourInMinute DECIMAL(18, 2), IsLeave BIT, IsDayOff BIT, IsHoliday BIT)

	CREATE TABLE #tbl_EmployeeBasic(EmployeeID int,EmployeeName varchar(100),Code varchar(100)
									,BusinessUnitID int,LocationID int,DepartmentID int,DesignationID int
									,BUName varchar(512), LocationName varchar(512), Department varchar(512), Designation varchar(512)
									,Gender varchar(512), ShiftID INT)
	CREATE TABLE #tbl_AttReport(EmployeeID INT, Fullfiled INT, LessThan11Hrs INT, LessThan9Hrs INT, LessThan6Hrs INT, TotalAbsent INT, TotalLeave INT, TotalDayOff INT)

	SET @sSQL='SELECT 
	EmployeeID, ShiftID, AttendanceDate, InTime, OutTime, TotalWorkingHourInMinute, IsLeave, IsDayOff, IsHoliday
	FROM AttendanceDaily WHERE AttendanceDate BETWEEN '''+ CONVERT(varchar(50),@Param_DateFrom) +''' AND '''+  CONVERT(varchar(50),@Param_DateTo)+''''

	IF @Param_EmployeeIDs<>'' AND @Param_EmployeeIDs IS NOT NULL
	BEGIN
		SET @sSQL=@sSQL+' AND EmployeeID IN ('+@Param_EmployeeIDs+')'
	END
	IF(@Param_BusinessUnitIDs !='' AND @Param_BusinessUnitIDs IS NOT NULL)
	BEGIN
		SET @sSQL=@sSQL+' AND BusinessUnitID IN('+ @Param_BusinessUnitIDs+')'
	END
	IF(@Param_LocationIDs !='' AND @Param_LocationIDs IS NOT NULL)
	BEGIN
		SET @sSQL=@sSQL+' AND LocationID IN('+ @Param_LocationIDs+')'
	END
	IF @Param_DepartmentIds<>'' AND @Param_DepartmentIds IS NOT NULL
	BEGIN
		SET @sSQL=@sSQL+' AND DepartmentID IN ('+@Param_DepartmentIds+')'
	END
	IF @Param_sDesignationIds<>'' AND @Param_sDesignationIds IS NOT NULL
	BEGIN
		SET @sSQL=@sSQL+' AND DesignationID IN ('+@Param_sDesignationIds+')'
	END
	IF @Param_SalarySchemeIDs<>'' AND @Param_SalarySchemeIDs IS NOT NULL
	BEGIN
		SET @sSQL=@sSQL+' AND EmployeeID IN(SELECT EmployeeID FROM EmployeeSalaryStructure WHERE SalarySchemeID IN ('+@Param_SalarySchemeIDs+'))'
	END
	IF @Param_sGroupIDs<>'' AND @Param_sGroupIDs IS NOT NULL
	BEGIN
		SET @sSQL=@sSQL+' AND EmployeeID IN( SELECT EmployeeID From EmployeeGroup WHERE EmployeeTypeID IN(' + @Param_sGroupIDs + '))'
	END
	IF @Param_sBlockIDs<>'' AND @Param_sBlockIDs IS NOT NULL
	BEGIN
		SET @sSQL=@sSQL+' AND EmployeeID IN( SELECT EmployeeID From EmployeeGroup WHERE EmployeeTypeID IN(' + @Param_sBlockIDs + '))'
	END
	IF(@Param_StartSalaryRange > 0 AND @Param_EndSalaryRange >0)
	BEGIN
		SET @sSQL = @sSQL + ' AND EmployeeID IN(SELECT EmployeeID From EmployeeSalary WHERE GrossAmount BETWEEN ' + CONVERT(varchar(512), @Param_StartSalaryRange) + ' AND ' + CONVERT(varchar(512), @Param_EndSalaryRange) + ')';
	END
	IF @Param_ShiftIDs <>'' AND @Param_ShiftIDs IS NOT NULL
	BEGIN
		SET @sSQL = @sSQL + ' AND ShiftID IN('+@Param_ShiftIDs+')'
	END

	IF EXISTS(SELECT * FROM Users WHERE userID = @Param_UserId AND FinancialUserType!=1)
	BEGIN
		 SET @sSQL = @sSQL + ' AND DepartmentID IN( SELECT DepartmentID FROM DepartmentRequirementPolicy WHERE DepartmentRequirementPolicyID IN(SELECT DRPID FROM DepartmentRequirementPolicyPermission WHERE UserID ='  + CONVERT(VARCHAR(50),@Param_UserId)+'))'
    END

	INSERT INTO #tbl_EmployeeID
	EXEC(@sSQL)

	INSERT INTO #tbl_EmployeeBasic
	SELECT DISTINCT EmpID.EmployeeID 
		  ,Emp.Name
		  ,Emp.Code
		  ,BU.BusinessUnitID
		  ,Loc.LocationID
		  ,Dpt.DepartmentID
		  ,Dsg.DesignationID
		  ,BU.Name
		  ,Loc.Name
		  ,Dpt.Name
		  ,Dsg.Name
		  ,Emp.Gender
		  ,EO.CurrentShiftID
	FROM #tbl_EmployeeID EmpID
	LEFT JOIN Employee Emp ON EmpID.EmployeeID = Emp.EmployeeID
	LEFT JOIN EmployeeOfficial AS EO ON Emp.EmployeeID = EO.EmployeeID
	LEFT JOIN DepartmentRequirementPolicy AS DRP ON EO.DRPID = DRP.DepartmentRequirementPolicyID
	LEFT JOIN BusinessUnit AS BU ON DRP.BusinessUnitID = BU.BusinessUnitID
	LEFT JOIN Location AS Loc ON DRP.LocationID = Loc.LocationID
	LEFT JOIN Department AS Dpt ON DRP.DepartmentID = Dpt.DepartmentID
	LEFT JOIN Designation AS Dsg ON EO.DesignationID = Dsg.DesignationID


	INSERT INTO #tbl_AttReport
	SELECT EmpID.EmployeeID
		, SUM(CASE WHEN (EmpID.TotalWorkingHourInMinute >= HS.TotalWorkingTime) THEN 1 ELSE 0 END) 
		, SUM(CASE WHEN (CONVERT(DECIMAL(18, 2), EmpID.TotalWorkingHourInMinute) / 60 < 11.0 AND CONVERT(DECIMAL(18, 2), EmpID.TotalWorkingHourInMinute) / 60 >= 9.0) THEN 1 ELSE 0 END) 
		, SUM(CASE WHEN (CONVERT(DECIMAL(18, 2), EmpID.TotalWorkingHourInMinute) / 60 < 9.0 AND CONVERT(DECIMAL(18, 2), EmpID.TotalWorkingHourInMinute) / 60 >= 6.0) THEN 1 ELSE 0 END) 
		, SUM(CASE WHEN (CONVERT(DECIMAL(18, 2), EmpID.TotalWorkingHourInMinute) / 60 < 6.0 AND CONVERT(DECIMAL(18, 2), EmpID.TotalWorkingHourInMinute) / 60 > 0) THEN 1 ELSE 0 END) 
		, SUM(CASE WHEN (CAST(EmpID.InTime AS TIME(0)) = '00:00:00' AND CAST(EmpID.OutTime AS TIME(0)) = '00:00:00' AND EmpID.IsDayOff=0 AND EmpID.IsHoliday=0 AND EmpID.IsLeave=0) THEN 1 ELSE 0 END) 
		, SUM(CASE WHEN (EmpID.IsLeave=1) THEN 1 ELSE 0 END) 
		, SUM(CASE WHEN (EmpID.IsDayOff=1) THEN 1 ELSE 0 END)
	FROM #tbl_EmployeeID EmpID 
	LEFT JOIN HRM_Shift HS ON EmpID.ShiftID=HS.ShiftID
	GROUP BY EmpID.EmployeeID



	SELECT EB.*
		 , AR.Fullfiled
		 , AR.LessThan11Hrs
		 , AR.LessThan9Hrs
		 , AR.LessThan6Hrs
		 , AR.TotalAbsent
		 , AR.TotalLeave
		 , HS.TotalWorkingTime / 60 AS DutyHour
	FROM #tbl_EmployeeBasic EB 
	INNER JOIN #tbl_AttReport AR ON EB.EmployeeID = AR.EmployeeID
	LEFT JOIN HRM_Shift HS ON EB.ShiftID=HS.ShiftID

	
	DROP TABLE #tbl_EmployeeID
	DROP TABLE #tbl_EmployeeBasic
	DROP TABLE #tbl_AttReport
END 
GO
/****** Object:  StoredProcedure [dbo].[SP_Rpt_IncrementByPercent]    Script Date: 10/22/2018 4:56:59 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[SP_Rpt_IncrementByPercent]
--DECLARE
	@Param_EmployeeIDs AS VARCHAR(MAX),
    @Param_SalaryHeadID AS INT,
    @Param_Percent AS INT,
    @Param_MonthIDs AS VARCHAR(512),
    @Param_YearIDs AS VARCHAR(512),
    @Param_UserID AS INT
AS
BEGIN
	--SET @Param_EmployeeIDs='65325'
 --   SET @Param_SalaryHeadID=1
 --   SET @Param_Percent=5
 --   SET @Param_MonthIDs='2'
 --   SET @Param_YearIDs='2010'
 --   SET @Param_UserID=128

	DECLARE
	@PV_SQL AS NVARCHAR(MAX)
	
	IF OBJECT_ID('tempdb..#tbl_EmployeeID') IS NOT NULL
	BEGIN
		DROP TABLE #tbl_EmployeeID
	END
	IF OBJECT_ID('tempdb..#tbl_EmployeeSalaryStructure') IS NOT NULL
	BEGIN
		DROP TABLE #tbl_EmployeeSalaryStructure
	END
	IF OBJECT_ID('tempdb..#tbl_EmployeeSalaryStructureDetail') IS NOT NULL
	BEGIN
		DROP TABLE #tbl_EmployeeSalaryStructureDetail
	END

	CREATE TABLE #tbl_EmployeeID(EmployeeID INT)
	CREATE TABLE #tbl_EmployeeSalaryStructure(EmployeeID INT, ESSID INT, SalarySchemeID INT, PreviousGrossAmount DECIMAL(18, 2), PreviousBasicAmount DECIMAL(18, 2), IncrementedGrossAmonut DECIMAL(18, 2), IncrementedBasicAmonut DECIMAL(18, 2), Code VARCHAR(512), Name VARCHAR(512), DesignationID INT, DRPID INT, ASID INT, DOJ DATE, SalarySchemeName VARCHAR(512))

	SET @PV_SQL=''
	SET @PV_SQL='SELECT 
		EMP.EmployeeID 
		FROM Employee EMP
		LEFT JOIN EmployeeOfficial EO ON EO.EmployeeID = EMP.EmployeeID
		WHERE EMP.EmployeeID <> 0 AND EMP.IsActive=1'

	
	IF @Param_EmployeeIDs!='' AND @Param_EmployeeIDs IS NOT NULL
	BEGIN
		SET @PV_SQL=@PV_SQL+' AND EMP.EmployeeID IN ('+@Param_EmployeeIDs+')'
	END		
	SET @PV_SQL=@PV_SQL+ ' AND CONVERT(INT,DATEPART(MONTH,DateOfJoin)) IN ('+@Param_MonthIDs+') AND CONVERT(INT,DATEPART(YEAR,DateOfJoin)) IN( '+@Param_YearIDs+')'
	
	INSERT INTO #tbl_EmployeeID
	EXEC(@PV_SQL)

	INSERT INTO #tbl_EmployeeSalaryStructure
	SELECT EmpID.EmployeeID
		  ,ESS.ESSID
		  ,ESS.SalarySchemeID
		  ,ESS.ActualGrossAmount 
		  ,(SELECT MAX(ESSD.Amount) FROM EmployeeSalaryStructureDetail ESSD WHERE ESS.ESSID = ESSD.ESSID)
		  ,ESS.ActualGrossAmount 
		  ,(SELECT MAX(ESSD.Amount) FROM EmployeeSalaryStructureDetail ESSD WHERE ESS.ESSID = ESSD.ESSID)
		  ,EMP.Code
		  ,EMP.Name
		  ,EO.DesignationID
		  ,EO.DRPID
		  ,EO.AttendanceSchemeID
		  ,EO.DateOfJoin
		  ,SS.Name
	FROM #tbl_EmployeeID EmpID
	LEFT JOIN EmployeeSalaryStructure ESS ON EmpID.EmployeeID = ESS.EmployeeID
	LEFT JOIN Employee EMP ON EmpID.EmployeeID = EMP.EmployeeID
	LEFT JOIN EmployeeOfficial EO ON EmpID.EmployeeID = EO.EmployeeID
	LEFT JOIN SalaryScheme SS ON ESS.SalarySchemeID = SS.SalarySchemeID

	
	IF @Param_Percent>0
	BEGIN
		UPDATE #tbl_EmployeeSalaryStructure SET IncrementedGrossAmonut = PreviousGrossAmount + ((@Param_Percent * PreviousGrossAmount) / 100)
											  , IncrementedBasicAmonut = PreviousBasicAmount + ((@Param_Percent * PreviousBasicAmount) / 100)
	END
	


	SELECT * FROM #tbl_EmployeeSalaryStructure
	
	DROP TABLE #tbl_EmployeeID
	DROP TABLE #tbl_EmployeeSalaryStructure
END

GO
/****** Object:  StoredProcedure [dbo].[SP_Rpt_LateEarlyAttendanceSummary]    Script Date: 10/22/2018 4:56:59 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[SP_Rpt_LateEarlyAttendanceSummary]
(
	--DECLARE
	@Param_EmployeeIDs VARCHAR(max),
	@Param_BusinessUnitIds VARCHAR(max),
	@Param_LocationIDs VARCHAR(max),
	@Param_DepartmentIds VARCHAR(max),
	@Param_sDesignationIds VARCHAR(max),
	@Param_SalarySchemeIDs VARCHAR(max),		
	@Param_DateFrom AS DATE,
	@Param_DateTo AS DATE,
	@Param_WorkingStatus VARCHAR(50),
	@Param_UserId INT,
	@Param_sGroupIDs VARCHAR(MAX),
	@Param_sBlockIDs VARCHAR(MAX),
	@Param_StartSalaryRange DECIMAL(30, 17),
	@Param_EndSalaryRange DECIMAL(30, 17),
	@Param_ShiftIDs VARCHAR(MAX),
	@Param_Remarks VARCHAR(MAX),
	@Param_IsMultipleMonth BIT,
	@Param_MonthIDs VARCHAR(512),
	@Param_YearIDs VARCHAR(512)
)
AS
BEGIN 
	DECLARE 
	@sSQL as nvarchar (max)
	
	IF OBJECT_ID('tempdb..#tbl_EmployeeID') IS NOT NULL
	BEGIN
		DROP TABLE #tbl_EmployeeID
	END
	IF OBJECT_ID('tempdb..#tbl_EmployeeBasic') IS NOT NULL
	BEGIN
		DROP TABLE #tbl_EmployeeBasic
	END
	
	IF OBJECT_ID('tempdb..#tbl_AttReport') IS NOT NULL
	BEGIN
		DROP TABLE #tbl_AttReport
	END
	IF OBJECT_ID('tempdb..#tbl_EmployeeIDDist') IS NOT NULL
	BEGIN
		DROP TABLE #tbl_EmployeeIDDist
	END
	
	CREATE TABLE #tbl_EmployeeID(EmployeeID INT, AttendanceID INT, ShiftID INT, AttendanceDate DATE, InTime DATETIME, OutTime DATETIME, EarlyDepartureMinute INT, LateArrivalMinute INT, IsLeave BIT, IsDayOff BIT, IsHoliday BIT, MonthID INT, YearID INT)

	CREATE TABLE #tbl_EmployeeIDDist(EmployeeID INT)

	CREATE TABLE #tbl_EmployeeBasic(EmployeeID int,EmployeeName varchar(100),Code varchar(100)
									,BusinessUnitID int,LocationID int,DepartmentID int,DesignationID int
									,BUName varchar(512), LocationName varchar(512), Department varchar(512), Designation varchar(512)
									,Gender varchar(512), ShiftID INT)
	CREATE TABLE #tbl_AttReport(EmployeeID INT, MonthID INT, YearID INT, LateAttendanceCount INT, EarlyLeaveCount INT, TotalWorkingDays INT)
	IF @Param_IsMultipleMonth = 0
	BEGIN
		SET @sSQL='SELECT 
		EmployeeID, AttendanceID, ShiftID, AttendanceDate, InTime, OutTime, EarlyDepartureMinute, LateArrivalMinute, IsLeave, IsDayOff, IsHoliday, CONVERT(INT,DATEPART(MONTH,AttendanceDate)), CONVERT(INT,DATEPART(YEAR,AttendanceDate))
		FROM AttendanceDaily WHERE AttendanceDate BETWEEN '''+ CONVERT(varchar(50),@Param_DateFrom) +''' AND '''+  CONVERT(varchar(50),@Param_DateTo)+''''
	END ELSE BEGIN
		SET @sSQL='SELECT 
		EmployeeID, AttendanceID, ShiftID, AttendanceDate, InTime, OutTime, EarlyDepartureMinute, LateArrivalMinute, IsLeave, IsDayOff, IsHoliday, CONVERT(INT,DATEPART(MONTH,AttendanceDate)), CONVERT(INT,DATEPART(YEAR,AttendanceDate))
		FROM AttendanceDaily WHERE CONVERT(INT,DATEPART(MONTH,AttendanceDate)) IN ('+@Param_MonthIDs+') AND CONVERT(INT,DATEPART(YEAR,AttendanceDate)) IN( '+@Param_YearIDs+')'
	END
	IF @Param_EmployeeIDs<>'' AND @Param_EmployeeIDs IS NOT NULL
	BEGIN
		SET @sSQL=@sSQL+' AND EmployeeID IN ('+@Param_EmployeeIDs+')'
	END
	IF(@Param_BusinessUnitIDs !='' AND @Param_BusinessUnitIDs IS NOT NULL)
	BEGIN
		SET @sSQL=@sSQL+' AND BusinessUnitID IN('+ @Param_BusinessUnitIDs+')'
	END
	IF(@Param_LocationIDs !='' AND @Param_LocationIDs IS NOT NULL)
	BEGIN
		SET @sSQL=@sSQL+' AND LocationID IN('+ @Param_LocationIDs+')'
	END
	IF @Param_DepartmentIds<>'' AND @Param_DepartmentIds IS NOT NULL
	BEGIN
		SET @sSQL=@sSQL+' AND DepartmentID IN ('+@Param_DepartmentIds+')'
	END
	IF @Param_sDesignationIds<>'' AND @Param_sDesignationIds IS NOT NULL
	BEGIN
		SET @sSQL=@sSQL+' AND DesignationID IN ('+@Param_sDesignationIds+')'
	END
	IF @Param_SalarySchemeIDs<>'' AND @Param_SalarySchemeIDs IS NOT NULL
	BEGIN
		SET @sSQL=@sSQL+' AND EmployeeID IN(SELECT EmployeeID FROM EmployeeSalaryStructure WHERE SalarySchemeID IN ('+@Param_SalarySchemeIDs+'))'
	END
	IF @Param_sGroupIDs<>'' AND @Param_sGroupIDs IS NOT NULL
	BEGIN
		SET @sSQL=@sSQL+' AND EmployeeID IN( SELECT EmployeeID From EmployeeGroup WHERE EmployeeTypeID IN(' + @Param_sGroupIDs + '))'
	END
	IF @Param_sBlockIDs<>'' AND @Param_sBlockIDs IS NOT NULL
	BEGIN
		SET @sSQL=@sSQL+' AND EmployeeID IN( SELECT EmployeeID From EmployeeGroup WHERE EmployeeTypeID IN(' + @Param_sBlockIDs + '))'
	END
	IF(@Param_StartSalaryRange > 0 AND @Param_EndSalaryRange >0)
	BEGIN
		SET @sSQL = @sSQL + ' AND EmployeeID IN(SELECT EmployeeID From EmployeeSalary WHERE GrossAmount BETWEEN ' + CONVERT(varchar(512), @Param_StartSalaryRange) + ' AND ' + CONVERT(varchar(512), @Param_EndSalaryRange) + ')';
	END
	IF @Param_ShiftIDs <>'' AND @Param_ShiftIDs IS NOT NULL
	BEGIN
		SET @sSQL = @sSQL + ' AND ShiftID IN('+@Param_ShiftIDs+')'
	END

	IF EXISTS(SELECT * FROM Users WHERE userID = @Param_UserId AND FinancialUserType!=1)
	BEGIN
		 SET @sSQL = @sSQL + ' AND DepartmentID IN( SELECT DepartmentID FROM DepartmentRequirementPolicy WHERE DepartmentRequirementPolicyID IN(SELECT DRPID FROM DepartmentRequirementPolicyPermission WHERE UserID ='  + CONVERT(VARCHAR(50),@Param_UserId)+'))'
    END

	INSERT INTO #tbl_EmployeeID
	EXEC(@sSQL)

	INSERT INTO #tbl_EmployeeIDDist
	SELECT DISTINCT ID.EmployeeID from #tbl_EmployeeID ID

	INSERT INTO #tbl_EmployeeBasic
	SELECT DISTINCT EmpID.EmployeeID 
		  ,Emp.Name
		  ,Emp.Code
		  ,BU.BusinessUnitID
		  ,Loc.LocationID
		  ,Dpt.DepartmentID
		  ,Dsg.DesignationID
		  ,BU.Name
		  ,Loc.Name
		  ,Dpt.Name
		  ,Dsg.Name
		  ,Emp.Gender
		  ,EO.CurrentShiftID
	FROM #tbl_EmployeeID EmpID
	LEFT JOIN Employee Emp ON EmpID.EmployeeID = Emp.EmployeeID
	LEFT JOIN EmployeeOfficial AS EO ON Emp.EmployeeID = EO.EmployeeID
	LEFT JOIN DepartmentRequirementPolicy AS DRP ON EO.DRPID = DRP.DepartmentRequirementPolicyID
	LEFT JOIN BusinessUnit AS BU ON DRP.BusinessUnitID = BU.BusinessUnitID
	LEFT JOIN Location AS Loc ON DRP.LocationID = Loc.LocationID
	LEFT JOIN Department AS Dpt ON DRP.DepartmentID = Dpt.DepartmentID
	LEFT JOIN Designation AS Dsg ON EO.DesignationID = Dsg.DesignationID


	INSERT INTO #tbl_AttReport
	SELECT EmpID.EmployeeID
		  ,EmpID.MonthID
		  ,EmpID.YearID
		  ,SUM(CASE WHEN ISNULL(EmpID.EarlyDepartureMinute,0)>0 AND EmpID.IsLeave=0 AND  EmpID.IsDayOff=0 AND EmpID.IsHoliday=0 THEN 1 ELSE 0 END)
		  ,SUM(CASE WHEN ISNULL(EmpID.LateArrivalMinute,0)>0 AND EmpID.IsLeave=0 AND EmpID.IsDayOff=0 AND EmpID.IsHoliday=0 THEN 1 ELSE 0 END) 
		  ,COUNT(EmpID.AttendanceID)
	FROM #tbl_EmployeeID EmpID 
	GROUP BY EmpID.EmployeeID, EmpID.MonthID,EmpID.YearID

	
	--select COUNT(*) from #tbl_AttReport
	--select COUNT(*) from #tbl_EmployeeIDDist

	SELECT AR.* 
		  ,EB.EmployeeName
		  ,EB.Code
		  ,EB.BusinessUnitID
		  ,EB.LocationID
		  ,EB.DepartmentID
		  ,EB.DesignationID
		  ,EB.BUName
		  ,EB.LocationName
		  ,EB.Department
		  ,EB.Designation
		  ,EB.Gender
		  ,EB.ShiftID
	FROM #tbl_AttReport AR
	LEFT JOIN #tbl_EmployeeIDDist EmpID ON AR.EmployeeID = EmpID.EmployeeID
	LEFT JOIN #tbl_EmployeeBasic EB ON AR.EmployeeID = EB.EmployeeID

	--SELECT EB.*
	--	  ,AR.LateAttendanceCount
	--	  ,AR.EarlyLeaveCount
	--	  ,EID.MonthID
	--FROM #tbl_EmployeeBasic EB 
	--INNER JOIN #tbl_AttReport AR ON EB.EmployeeID = AR.EmployeeID
	--INNER JOIN #tbl_EmployeeID EID ON EB.EmployeeID = EID.EmployeeID

	
	DROP TABLE #tbl_EmployeeID
	DROP TABLE #tbl_EmployeeBasic
	DROP TABLE #tbl_AttReport
	DROP TABLE #tbl_EmployeeIDDist
END 
GO
/****** Object:  StoredProcedure [dbo].[SP_Rpt_MaternityFollowUp]    Script Date: 10/22/2018 4:56:59 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[SP_Rpt_MaternityFollowUp]
(
		--DECLARE
		@Param_EmployeeIDs VARCHAR(max),
		@Param_BusinessUnitIds VARCHAR(max),
		@Param_LocationIDs VARCHAR(max),
		@Param_DepartmentIds VARCHAR(max),
		@Param_sDesignationIds VARCHAR(max),
		@Param_SalarySchemeIDs VARCHAR(max),		
		@Param_DateFrom AS DATE,
		@Param_DateTo AS DATE,
		@Param_WorkingStatus VARCHAR(50),
		@Param_UserId INT,
		@Param_sGroupIDs VARCHAR(MAX),
		@Param_sBlockIDs VARCHAR(MAX),
		@Param_StartSalaryRange DECIMAL(30, 17),
		@Param_EndSalaryRange DECIMAL(30, 17),
		@Param_ShiftIDs VARCHAR(MAX),
		@Param_Remarks VARCHAR(MAX)
)
AS
BEGIN 
	DECLARE 
	@sSQL as nvarchar (max)
	
	IF OBJECT_ID('tempdb..#tbl_EmployeeID') IS NOT NULL
	BEGIN
		DROP TABLE #tbl_EmployeeID
	END

	IF OBJECT_ID('tempdb..#tbl_EmployeeBasic') IS NOT NULL
	BEGIN
		DROP TABLE #tbl_EmployeeBasic
	END

	
	CREATE TABLE #tbl_EmployeeID(EmployeeID INT)

	CREATE TABLE #tbl_EmployeeBasic(EmployeeID int,EmployeeName varchar(100),Code varchar(100)
									,BusinessUnitID int,LocationID int,DepartmentID int,DesignationID int
									,BUName varchar(512), LocationName varchar(512), Department varchar(512), Designation varchar(512)
									,Gender varchar(512))
	
	SET @sSQL='SELECT 
	 DISTINCT EmployeeID 
	FROM AttendanceDaily WHERE AttendanceDate BETWEEN '''+ CONVERT(varchar(50),@Param_DateFrom) +''' AND '''+  CONVERT(varchar(50),@Param_DateTo)+''''

	IF @Param_EmployeeIDs<>'' AND @Param_EmployeeIDs IS NOT NULL
	BEGIN
		SET @sSQL=@sSQL+' AND EmployeeID IN ('+@Param_EmployeeIDs+')'
	END
	IF(@Param_BusinessUnitIDs !='' AND @Param_BusinessUnitIDs IS NOT NULL)
	BEGIN
		SET @sSQL=@sSQL+' AND BusinessUnitID IN('+ @Param_BusinessUnitIDs+')'
	END
	IF(@Param_LocationIDs !='' AND @Param_LocationIDs IS NOT NULL)
	BEGIN
		SET @sSQL=@sSQL+' AND LocationID IN('+ @Param_LocationIDs+')'
	END
	IF @Param_DepartmentIds<>'' AND @Param_DepartmentIds IS NOT NULL
	BEGIN
		SET @sSQL=@sSQL+' AND DepartmentID IN ('+@Param_DepartmentIds+')'
	END
	IF @Param_sDesignationIds<>'' AND @Param_sDesignationIds IS NOT NULL
	BEGIN
		SET @sSQL=@sSQL+' AND DesignationID IN ('+@Param_sDesignationIds+')'
	END
	IF @Param_SalarySchemeIDs<>'' AND @Param_SalarySchemeIDs IS NOT NULL
	BEGIN
		SET @sSQL=@sSQL+' AND EmployeeID IN(SELECT EmployeeID FROM EmployeeSalaryStructure WHERE SalarySchemeID IN ('+@Param_SalarySchemeIDs+'))'
	END
	IF @Param_sGroupIDs<>'' AND @Param_sGroupIDs IS NOT NULL
	BEGIN
		SET @sSQL=@sSQL+' AND EmployeeID IN( SELECT EmployeeID From EmployeeGroup WHERE EmployeeTypeID IN(' + @Param_sGroupIDs + '))'
	END
	IF @Param_sBlockIDs<>'' AND @Param_sBlockIDs IS NOT NULL
	BEGIN
		SET @sSQL=@sSQL+' AND EmployeeID IN( SELECT EmployeeID From EmployeeGroup WHERE EmployeeTypeID IN(' + @Param_sBlockIDs + '))'
	END
	IF(@Param_StartSalaryRange > 0 AND @Param_EndSalaryRange >0)
	BEGIN
		SET @sSQL = @sSQL + ' AND EmployeeID IN(SELECT EmployeeID From EmployeeSalary WHERE GrossAmount BETWEEN ' + CONVERT(varchar(512), @Param_StartSalaryRange) + ' AND ' + CONVERT(varchar(512), @Param_EndSalaryRange) + ')';
	END
	IF @Param_ShiftIDs <>'' AND @Param_ShiftIDs IS NOT NULL
	BEGIN
		SET @sSQL = @sSQL + ' AND ShiftID IN('+@Param_ShiftIDs+')'
	END

	IF EXISTS(SELECT * FROM Users WHERE userID = @Param_UserId AND FinancialUserType!=1)
	BEGIN
		 SET @sSQL = @sSQL + ' AND DepartmentID IN( SELECT DepartmentID FROM DepartmentRequirementPolicy WHERE DepartmentRequirementPolicyID IN(SELECT DRPID FROM DepartmentRequirementPolicyPermission WHERE UserID ='  + CONVERT(VARCHAR(50),@Param_UserId)+'))'
    END

	INSERT INTO #tbl_EmployeeID
	EXEC(@sSQL)

	INSERT INTO #tbl_EmployeeBasic
	SELECT EmpID.EmployeeID 
		  ,Emp.Name
		  ,Emp.Code
		  ,BU.BusinessUnitID
		  ,Loc.LocationID
		  ,Dpt.DepartmentID
		  ,Dsg.DesignationID
		  ,BU.Name
		  ,Loc.Name
		  ,Dpt.Name
		  ,Dsg.Name
		  ,Emp.Gender
	FROM #tbl_EmployeeID EmpID
	LEFT JOIN Employee Emp ON EmpID.EmployeeID = Emp.EmployeeID
	LEFT JOIN EmployeeOfficial AS EO ON Emp.EmployeeID = EO.EmployeeID
	LEFT JOIN DepartmentRequirementPolicy AS DRP ON EO.DRPID = DRP.DepartmentRequirementPolicyID
	LEFT JOIN BusinessUnit AS BU ON DRP.BusinessUnitID = BU.BusinessUnitID
	LEFT JOIN Location AS Loc ON DRP.LocationID = Loc.LocationID
	LEFT JOIN Department AS Dpt ON DRP.DepartmentID = Dpt.DepartmentID
	LEFT JOIN Designation AS Dsg ON EO.DesignationID = Dsg.DesignationID
	
	SELECT * FROM #tbl_EmployeeBasic
	
	DROP TABLE #tbl_EmployeeID
	DROP TABLE #tbl_EmployeeBasic

END 
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[SP_GetEmployeeBasicAmount]
(
	@Param_EmployeeID INT,
	@Param_BasicOrGross BIT
)
AS
BEGIN
	DECLARE	
	@Param_BasicAmount DECIMAL(18, 2)		
	SET @Param_BasicAmount  = 0

	DECLARE 
	@PV_ESSID INT
	,@PV_SalaryHeadID INT
	,@PV_SalarySchemeID INT
	  
	,@PV_ValueOperator as smallint
	,@PV_CalculationOn as smallint
	,@PV_FixedValue as decimal(30,17)
	,@PV_PercentVelue as int
	,@PV_Operator as smallint
	,@PV_sEquation as nvarchar(500)
	,@PV_sEquationComp as nvarchar(500)
	,@PV_ExistingHeadAmount as decimal(30,17)
	,@PV_CalSalaryHeadID as int
	,@PV_sValue as decimal(30,17)
	,@PV_sValueC as decimal(30,17)
	
	,@PV_SQL as nvarchar(500)
	,@PV_SQLC as nvarchar(500)
	,@PV_FixedSalaryHeadIDs AS VARCHAR(MAX)
	,@PV_NonFixedSalaryHeadIDs AS VARCHAR(MAX)
	,@PV_CompGrossAmountStructure AS DECIMAL(18, 2)
	,@PV_SSDID INT
	,@PV_GrossAmount DECIMAL(18, 2)
	
	,@PV_IdDetail INT
	,@PV_IdCalc INT
		
	SET @PV_IdDetail = 1
	SET @PV_IdCalc = 1
	  
	SET @PV_SalaryHeadID = (SELECT SalaryHeadID FROM SalaryHead WHERE Name='Basic')
	SET @PV_GrossAmount = (SELECT ActualGrossAmount FROM EmployeeSalaryStructure WHERE EmployeeID=@Param_EmployeeID)
	SET @PV_SalarySchemeID = (SELECT SalarySchemeID FROM EmployeeSalaryStructure WHERE EmployeeID=@Param_EmployeeID)	
	SET @PV_ESSID = (SELECT ESSID FROM EmployeeSalaryStructure WHERE EmployeeID=@Param_EmployeeID)
	
	SET @Param_BasicAmount = 0
	
	DECLARE @tbl_SalarySchemeDetail AS TABLE(id INT IDENTITY(1, 1), SalarySchemeDetailID INT, SalaryHeadID INT)
	DECLARE @tbl_SalarySchemeDetailCalculation AS TABLE(idc INT IDENTITY(1, 1), ValueOperator INT ,CalculationOn INT,FixedValue DECIMAL(18, 2),SalaryHeadID INT,PercentVelue DECIMAL(18, 2),Operator INT)

	
	INSERT INTO @tbl_SalarySchemeDetail
	SELECT SalarySchemeDetailID,SalaryHeadID FROM SalarySchemeDetail WHERE SalarySchemeID=@PV_SalarySchemeID AND SalaryHeadID =@PV_SalaryHeadID Order BY SalaryHeadID ASC
	
	SET @PV_IdDetail = (SELECT COUNT(*) FROM @tbl_SalarySchemeDetail)
	DECLARE @id INT = 1
	DECLARE @idc INT = 1
	WHILE(@id <= @PV_IdDetail)
	BEGIN--	
			
		SELECT @PV_SSDID = SalarySchemeDetailID, @PV_SalaryHeadID = SalaryHeadID FROM @tbl_SalarySchemeDetail WHERE id=@id
		
		--initialization				
		SET @PV_ValueOperator=0
		SET @PV_CalculationOn=0
		SET @PV_FixedValue=0
		SET @PV_CalSalaryHeadID=0
		SET @PV_PercentVelue=0
		SET @PV_Operator=0
		SET @PV_sEquation =''
		SET @PV_sValue=0.0
		SET @PV_SQL=''

		INSERT INTO @tbl_SalarySchemeDetailCalculation
		SELECT ValueOperator,CalculationOn,FixedValue,SalaryHeadID,PercentVelue,Operator FROM SalarySchemeDetailCalculation WHERE SalarySchemeDetailID =@PV_SSDID  ORDER BY SSDCID ASC
		
		SET @PV_IdCalc = (SELECT COUNT(*) FROM @tbl_SalarySchemeDetailCalculation)
		WHILE(@idc <= @PV_IdCalc)
		BEGIN--	
			
			SELECT @PV_ValueOperator = ValueOperator,@PV_CalculationOn=CalculationOn,@PV_FixedValue=FixedValue,@PV_CalSalaryHeadID=SalaryHeadID,@PV_PercentVelue=PercentVelue,@PV_Operator=Operator FROM @tbl_SalarySchemeDetailCalculation WHERE idc=@idc
			
			IF (@PV_ValueOperator=1)--Value
			BEGIN
				IF (@PV_CalculationOn=1)
				BEGIN
					SET @PV_sEquation=@PV_sEquation+CONVERT(varchar(100),@PV_GrossAmount)
				END
				ELSE IF (@PV_CalculationOn=2) 
				BEGIN
					SET @PV_sEquation=@PV_sEquation+(SELECT CONVERT(varchar(100),Amount) FROM EmployeeSalaryStructureDetail WHERE ESSID=@PV_ESSID AND SalaryHeadID=@PV_CalSalaryHeadID)
				END
				ELSE IF (@PV_CalculationOn=3) 
				BEGIN
					SET @PV_sEquation=@PV_sEquation+CONVERT (varchar(100),@PV_FixedValue)				
				END
			END
			ELSE IF (@PV_ValueOperator=2)--Operator
			BEGIN
			
				IF (@PV_Operator=1) 
				BEGIN 
					SET @PV_sEquation=@PV_sEquation+'(' 
				END
				IF (@PV_Operator=2) 
				BEGIN
					SET @PV_sEquation=@PV_sEquation+')'
				END
				IF (@PV_Operator=3) 
				BEGIN
					SET @PV_sEquation=@PV_sEquation+'+'
				END
				IF (@PV_Operator=4) 
				BEGIN
					SET @PV_sEquation=@PV_sEquation+'-'
				END
				IF (@PV_Operator=5) 
				BEGIN
					SET @PV_sEquation=@PV_sEquation+'*'
				END
				IF (@PV_Operator=6) 
				BEGIN
					SET @PV_sEquation=@PV_sEquation+'/'
				END
				IF (@PV_Operator=7)	
				BEGIN
					SET @PV_sEquation=@PV_sEquation+CONVERT(varchar(50),CONVERT(decimal(30,17),@PV_PercentVelue)/100)+'*'			
				END
			END	
			SET @idc = @idc + 1
		END
		----value execution
		SET @PV_SQL=N'SET @PV_sValue=(SELECT '+@PV_sEquation+')'
		EXEC sp_executesql @PV_SQL, N'@PV_sValue decimal(18,2) out', @PV_sValue output
				
		SET @Param_BasicAmount = @PV_sValue

		SET @id = @id + 1
	END



	-- Return the result of the function	
	--SELECT @Param_BasicAmount
	RETURN @Param_BasicAmount
END