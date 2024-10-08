USE [ESimSol_ERP]
GO
/****** Object:  StoredProcedure [dbo].[SP_IUD_AttendanceDaily_Manual_Single]    Script Date: 11/20/2018 2:51:26 PM ******/
DROP PROCEDURE [dbo].[SP_IUD_AttendanceDaily_Manual_Single]
GO
/****** Object:  StoredProcedure [dbo].[SP_IUD_AttendanceDaily_Manual_Single]    Script Date: 11/20/2018 2:51:26 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO




CREATE PROCEDURE [dbo].[SP_IUD_AttendanceDaily_Manual_Single]
(
--DECLARE
	    @Param_AttendanceID AS INT,
		@Param_InTime AS DATETIME,
		@Param_OutTime AS DATETIME,
		@Param_ShiftID as int,
		@Param_IsOSD as bit,
		@param_IsDayoff as bit,
		@Param_IsAbsent as bit,
		@Param_LateArrivalMinute as int,--IsLate
		@Param_EarlyDepartureMinute as int,--IsEarly
		@Param_Remark as varchar(500),
		@Param_IsManualOT as bit,
		@Param_OverTimeInMinute as int,				
		@Param_DBUserID AS INT
)
AS
BEGIN TRAN
--SET @PV_EmployeeID =139
--SET @Param_StartDate ='2 nov 2015'
--SET @Param_EndDate ='3 nov 2015'
--set @Param_DBUserID=-9


-- =============================================
-- Author:		MD. Azharul Islam
-- Create date: 17 Jan 2015
-- Description:	Manual Attendance update
-- =============================================

-- =============================================
-- Author:		Asadullah Sarker
-- Edited date: 19 June 2017
-- Description:	DayOff And Holiday Check
-- =============================================

--Variable Declaration
	DECLARE
		 @PV_EmployeeID as int 
		,@PV_StartTime as DATETIME--time(0)
		,@PV_EndTime as DATETIME--time(0)
		,@PV_AttendanceSchemeID as int
		,@PV_AttendanceCalendarID as int
		,@PV_RosterPlanID as int
		,@PV_DesignationID as int
		,@PV_LateInMunit as int
		,@PV_EarlyInMunit as int		
		,@PV_TotalWorkingHourInMin as int
		,@PV_IsDayOff as bit
		,@PV_IsHoliday AS BIT
		,@PV_IsLeave as bit
		,@PV_LeaveIsUnPaid as bit
		,@PV_Index AS INT
	
		,@PV_TolaranceTime as Datetime--int
		,@PV_DayEndTime as DATETIME--time(0)
		,@PV_DayStartTime as DATETIME--time(0)
		,@PV_AttendanceDate DATE
		,@PV_LocationID as int
		,@PV_DepartmentID as int
		
		
		,@PV_ActualStart as DATETIME
		,@PV_ActualEnd as DATETIME
		,@PV_Shift_IsOT as bit
		,@PV_Shift_OTStart as datetime
		,@PV_Shift_OTEnd   as datetime	
		,@PV_Shift_ISOTOnActual as bit
		,@PV_Shift_OTCalclateAfterInMin as int
		,@PV_Shift_IsLeaveOnOFFHoliday as bit
		,@PV_LeaveHeadID as int

		--For Compliance
		,@PV_CompInTime as datetime
		,@PV_CompOutTime as DateTime
		,@PV_CompIsDayOff as bit
		,@PV_CompIsHoliday as bit
		,@PV_CompOverTimeInMin as int
		,@PV_CompMaxOTInMin as int
		,@PV_CompTotalWorkingHourInMin as int
		,@PV_HistoryDescription As NVarchar(500)
		,@PV_Msg as varchar(100)
		,@PV_MaxOTInMinATRoster as int
		,@PV_TimeKeeperOutTime as datetime
		,@PV_BaseAddress AS varchar(100)
		,@PV_BUID as int 
		,@PV_IsGWD as bit ---General Working Day
		,@PV_CompSlabOTInMin as int
		,@PV_IsCompOTFromSlab as bit
		,@PV_IsOTFromSlab as bit
		,@PV_CompMaxCompOTInMin as int
		,@PV_CompMaxEndTime AS DATE
		,@PV_BOA_IsOTSlab as bit

		SET @PV_BaseAddress=(SELECT top(1)BaseAddress FROM Company)
		IF @Param_Remark IS NULL BEGIN SET @Param_Remark='' END

		--For Break time
		DECLARE @tbl_BreakTime AS TABLE (BStart DateTime, BEnd DAtetime, BDurationMin int)

		DECLARE @tbl_Date TABLE (attDate DATE)

		IF (@PV_EmployeeID<=0 OR @Param_InTime='' OR @Param_InTime=null OR @Param_OutTime='' OR @Param_OutTime=null)
		BEGIN
			ROLLBACK
				RAISERROR (N'Some of the mandatory field missing!!',16,1);
			RETURN	
		END
		
		IF EXISTS (SELECT * FROM AttendanceDaily WHERE AttendanceID=@Param_AttendanceID AND ISELProcess=1)
		BEGIN
			IF NOT EXISTS (SELECT * FROM ELSetup WHERE IsConsiderDayOff=1 AND IsConsiderAbsent=1 AND IsConsiderDayOff=1 AND IsConsiderHoliday=1)
			BEGIN
				ROLLBACK 
					RAISERROR(N'EL Processed by This date. You cant edit.!!',16,1);
				RETURN				
			END
		END

		--Salary Check
		IF EXISTS (SELECT * FROM EmployeeSalary WHERE EmployeeID=@PV_EmployeeID AND @PV_AttendanceDate BETWEEN StartDate AND EndDate)
		BEGIN
			ROLLBACK 
				RAISERROR(N'Salary Already Processed by this date.!!',16,1);
			RETURN		
		END

		IF EXISTS (SELECT * FROM AttendanceDaily WHERE AttendanceID=@Param_AttendanceID AND IsHoliday=1 AND @Param_InTime='00:00:00' AND @Param_OutTime='00:00:00')
		BEGIN
			ROLLBACK 
				RAISERROR(N'This day is holiday, so you can not make anyone absent in this day.!!',16,1);
			RETURN
		END

	SELECT @PV_EmployeeID=EmployeeID
		   ,@PV_LocationID=LocationID
		   ,@PV_DepartmentID=DepartmentID
		   ,@PV_DesignationID = DesignationID 		   
		   ,@PV_AttendanceDate=AttendanceDate
		   ,@PV_AttendanceSchemeID=AttendanceSchemeID
		   ,@PV_RosterPlanID=RosterPlanID
		   ,@PV_BUID=BusinessUnitID
	FROM AttendanceDaily WHERE AttendanceID=@Param_AttendanceID


	----Check General Working Day
	--SET @PV_IsGWD=0
	--IF EXISTS (SELECT * FROM GeneralWorkingDay WHERE AttendanceDate=@PV_AttendanceDate AND GWDID IN (
	--			SELECT GWDID FROM GeneralWorkingDayDetail WHERE DRPID IN (
	--			SELECT DepartmentRequirementPolicyID FROM DepartmentRequirementPolicy WHERE BusinessUnitID=@PV_BUID AND LocationID=@PV_LocationID AND DepartmentID=@PV_DepartmentID)))
	--BEGIN
	--	SET @PV_IsGWD=1
	--END
	
	SET @PV_IsDayOff=0
	SET @PV_LeaveIsUnPaid=0
	SET @PV_IsLeave=0
	SET @PV_TotalWorkingHourInMin=0
	SET @PV_IsHoliday=0
	SET @PV_LateInMunit=0
	SET @PV_EarlyInMunit=0
	SET @PV_LeaveHeadID=0
	SET @PV_CompIsDayOff=0
	SET @PV_CompIsHoliday=0
	SET @PV_CompOverTimeInMin=0
	SET @PV_CompTotalWorkingHourInMin=0
	SET @PV_CompSlabOTInMin=0
	SET @PV_IsCompOTFromSlab=0
	SET @PV_IsOTFromSlab=0
	SET @PV_CompMaxCompOTInMin=0
	SET @PV_CompMaxEndTime=NULL
	SET @PV_MaxOTInMinATRoster=0

	--check Shift rostering
	IF EXISTS (SELECT * FROM RosterPlanEmployee WHERE EmployeeID=@PV_EmployeeID AND AttendanceDate=@PV_AttendanceDate)
	BEGIN		
		SELECT top(1)@PV_MaxOTInMinATRoster=ISNULL(MaxOTInMin,0)  
		FROM RosterPlanEmployee WHERE EmployeeID=@PV_EmployeeID AND AttendanceDate=@PV_AttendanceDate
	END--check Shift rostering


	SELECT @PV_AttendanceCalendarID=AttendanceCalenderID
	FROM AttendanceScheme WHERE AttendanceSchemeID=@PV_AttendanceSchemeID

	IF (@PV_AttendanceCalendarID is null)
	BEGIN
		ROLLBACK 
			RAISERROR(N'Invalid Attendance Scheme.!!',16,1);
		RETURN
	END

	--Get Shift Information
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
	,@PV_CompMaxCompOTInMin=ISNULL(MaxOTComplianceInMIn,0)
	FROM HRM_Shift WHERE ShiftID=@Param_ShiftID

	--Break Times
	INSERT @tbl_BreakTime
	SELECT BStart,BEnd,DATEDIFF(MINUTE,BStart,BEnd) FROM 
	(SELECT (DATEADD(SECOND,0,DATEADD (MINUTE,DATEPART(MINUTE,StartTime), DATEADD(HOUR, DATEPART(HOUR,StartTime), DATEADD(dd, DATEDIFF(dd, -0, CASE WHEN DATEPART(HOUR,@PV_DayStartTime)<DATEPART(HOUR,StartTime) THEN @PV_AttendanceDate ELSE DATEADD(DAY,1,@PV_AttendanceDate) END), 0)))) ) AS BStart 
	,(DATEADD(SECOND,0,DATEADD (MINUTE,DATEPART(MINUTE,EndTime), DATEADD(HOUR, DATEPART(HOUR,EndTime), DATEADD(dd, DATEDIFF(dd, -0, CASE WHEN DATEPART(HOUR,@PV_DayStartTime)<DATEPART(HOUR,EndTime) THEN @PV_AttendanceDate ELSE DATEADD(DAY,1,@PV_AttendanceDate) END), 0)))) ) AS BEnd
	FROM ShiftBreakSchedule WHERE ShiftID=@Param_ShiftID) ss


	SET @PV_ActualStart=(DATEADD(SECOND,0,DATEADD (MINUTE,DATEPART(MINUTE,@Param_InTime), DATEADD(HOUR, DATEPART(HOUR,@Param_InTime), DATEADD(dd, DATEDIFF(dd, -0, @PV_AttendanceDate), 0)))) )
	SET @PV_ActualEnd=(DATEADD(SECOND,0,DATEADD (MINUTE,DATEPART(MINUTE,@Param_OutTime), DATEADD(HOUR, DATEPART(HOUR,@Param_OutTime), DATEADD(dd, DATEDIFF(dd, -0, @PV_AttendanceDate), 0)))) )

	--Attendance Record for night shift
	IF (DATEPART(HOUR,@PV_ActualStart)>DATEPART(HOUR,@PV_ActualEnd))
	BEGIN--Night Shift or different date			
		SET @PV_ActualEnd=(DATEADD(SECOND,0,DATEADD (MINUTE,DATEPART(MINUTE,@PV_ActualEnd), DATEADD(HOUR, DATEPART(HOUR,@PV_ActualEnd), DATEADD(dd, DATEDIFF(dd, -1, @PV_AttendanceDate), 0)))) )
	END	ELSE BEGIN	
		SET @PV_ActualEnd=(DATEADD(SECOND,0,DATEADD (MINUTE,DATEPART(MINUTE,@Param_OutTime), DATEADD(HOUR, DATEPART(HOUR,@Param_OutTime), DATEADD(dd, DATEDIFF(dd, -0, @PV_AttendanceDate), 0)))) )
	END


	IF (@Param_IsOSD=1 AND (CAST(@PV_ActualStart AS TIME(0))='00:00' AND CAST(@PV_ActualEnd AS TIME(0))='00:00'))
	BEGIN		
		SET @PV_ActualStart=@PV_StartTime
		SET @PV_ActualEnd=@PV_EndTime
	END
	

	IF (DATEPART(HOUR,@PV_DayStartTime)>DATEPART(HOUR,@PV_DayEndTime))
	BEGIN--Night Shift or different date
		SET @PV_DayEndTime=(DATEADD(SECOND,0,DATEADD (MINUTE,DATEPART(MINUTE,@PV_DayEndTime), DATEADD(HOUR, DATEPART(HOUR,@PV_DayEndTime), DATEADD(dd, DATEDIFF(dd, -1, @PV_AttendanceDate), 0)))) )			
	END

	IF (DATEPART(HOUR,@PV_StartTime)>DATEPART(HOUR,@PV_EndTime))
	BEGIN--Night Shift or different date
		SET @PV_EndTime=(DATEADD(SECOND,0,DATEADD (MINUTE,DATEPART(MINUTE,@PV_EndTime), DATEADD(HOUR, DATEPART(HOUR,@PV_EndTime), DATEADD(dd, DATEDIFF(dd, -1, @PV_AttendanceDate), 0)))) )
		IF @PV_BaseAddress IN ('B007','mithela','akcl')
		BEGIN
			--OT Start time : If OT start time > Shift End Time Then Shift End Date will Be OT StartDate
			IF (DATEPART(HOUR,@PV_Shift_OTStart)>=DATEPART(HOUR,@PV_EndTime))-- AND DATEPART(HOUR,@PV_Shift_OTStart)>DATEPART(HOUR,@PV_StartTime))
			BEGIN
				SET @PV_Shift_OTStart=(DATEADD(SECOND,0,DATEADD (MINUTE,DATEPART(MINUTE,@PV_Shift_OTStart), DATEADD(HOUR, DATEPART(HOUR,@PV_Shift_OTStart), DATEADD(dd, DATEDIFF(dd, -0, @PV_EndTime), 0)))) )
				SET @PV_Shift_OTEnd=(DATEADD(SECOND,0,DATEADD (MINUTE,DATEPART(MINUTE,@PV_Shift_OTEnd), DATEADD(HOUR, DATEPART(HOUR,@PV_Shift_OTEnd), DATEADD(dd, DATEDIFF(dd, -0, @PV_EndTime), 0)))))
			END	
		END
	END
	
	IF (DATEPART(HOUR,@PV_Shift_OTStart)>DATEPART(HOUR,@PV_Shift_OTEnd))
	BEGIN--Night Shift or different date
		SET @PV_Shift_OTEnd=(DATEADD(SECOND,0,DATEADD (MINUTE,DATEPART(MINUTE,@PV_Shift_OTEnd), DATEADD(HOUR, DATEPART(HOUR,@PV_Shift_OTEnd), DATEADD(dd, DATEDIFF(dd, -1, @PV_AttendanceDate), 0)))) )
	END

------
	
	
	--get Leave Information if param day off false
	IF @param_IsDayoff=0
	BEGIN
		IF (EXISTS (SELECT * FROM LeaveApplication WHERE IsApprove=1/*ApproveBy>0*/ AND (CancelledBy=0 OR CancelledBy IS NULL) AND EmployeeID =@PV_EmployeeID	 AND @PV_AttendanceDate BETWEEN CONVERT (DATE,StartDateTime) AND CONVERT(DATE,EndDateTime)))
		BEGIN 
			SET @PV_IsLeave=1
			--SET @PV_LeaveIsUnPaid=(SELECT top(1)IsUnPaid FROM LeaveApplication WHERE ApproveBy>0 AND EmployeeID =@PV_EmployeeID	AND @PV_AttendanceDate BETWEEN CONVERT (DATE,StartDateTime) AND CONVERT(DATE,EndDateTime))
			IF EXISTS (SELECT * FROM LeaveHead WHERE IsLWP=1 AND LeaveHeadID IN (
			SELECT LeaveID FROM EmployeeLeaveLedger WHERE EmpLeaveLedgerID IN (
			SELECT EmpLeaveLedgerID FROM LeaveApplication WHERE IsApprove=1/*ApproveBy>0*/ AND (CancelledBy=0 OR CancelledBy IS NULL) AND EmployeeID =@PV_EmployeeID	AND @PV_AttendanceDate BETWEEN CONVERT (DATE,StartDateTime) AND CONVERT(DATE,EndDateTime))))
			BEGIN
				SET @PV_LeaveIsUnPaid=1
			END

			SET @PV_LeaveHeadID=(SELECT top(1)LeaveID FROM EmployeeLeaveLedger WHERE EmpLeaveLedgerID IN (
								SELECT EmpLeaveLedgerID FROM LeaveApplication WHERE IsApprove=1/*ApproveBy>0*/ AND (CancelledBy=0 OR CancelledBy IS NULL) AND EmployeeID =@PV_EmployeeID	
								AND @PV_AttendanceDate BETWEEN CONVERT (DATE,StartDateTime) AND CONVERT(DATE,EndDateTime)))

			IF EXISTS (SELECT * FROM LeaveApplication WHERE IsApprove=1/*ApproveBy>0*/ AND EmployeeID =@PV_EmployeeID	AND @PV_AttendanceDate BETWEEN CONVERT (DATE,StartDateTime) AND CONVERT(DATE,EndDateTime) AND LeaveType=1 AND CancelledBy<=0)
			BEGIN
				ROLLBACK
					RAISERROR(N'This employee is in full day Leave by this date.!!',16,1)
				RETURN
			END
		END	
	END

	-- check Holiday
	IF EXISTS (SELECT * FROM AttendanceCalendarSessionHoliday WHERE ACSID IN (
		SELECT ACSID FROM AttendanceCalendarSession WHERE AttendanceCalendarID=@PV_AttendanceCalendarID)
		--AND HolidayID IN (SELECT HolidayID FROM AttendanceSchemeHoliday WHERE AttendanceSchemeID=@PV_AttendanceSchemeID) 
		AND @PV_AttendanceDate BETWEEN StartDate AND EndDate)
	BEGIN--Holiday
		SET @PV_IsHoliday=1	
		SET @PV_CompIsHoliday=1				
	END
	
				
			
	----get Day Off information
	--IF @PV_IsGWD=0--Not General day
	--BEGIN
	--	IF EXISTS (SELECT * FROM AttendanceSchemeDayOff WHERE AttendanceSchemeID=@PV_AttendanceSchemeID AND [WeekDay]=DATENAME(DW,@PV_AttendanceDate))
	--	BEGIN
	--		SET @PV_IsDayOff=1;
	--		SET @PV_CompIsDayOff=1;
	--	END
	--END

	----Suffix & Prefix for leave 
	--IF @PV_Shift_IsLeaveOnOFFHoliday=1
	--BEGIN--Leave Between DayOff or holiday
	--	IF (@PV_IsLeave=1)
	--	BEGIN
	--		SET @PV_IsDayOff=0
	--		SET @PV_IsHoliday=0
	--		SET @PV_CompIsDayOff=0;
	--		SET @PV_CompIsHoliday=0;	
	--		SET @Param_IsOSD=0						
	--	END			
	--END--Leave Between DayOff or holiday

	IF @param_IsDayoff=1
	BEGIN
			SET @PV_IsDayOff=1;
			SET @PV_CompIsDayOff=1;		
			SET @PV_IsLeave=0		
			SET @Param_IsOSD=0
	END

	IF @PV_IsHoliday=1
	BEGIN
		SET @PV_IsDayOff=0;
		SET @PV_CompIsDayOff=0;		
		SET @Param_IsOSD=0
		SET @PV_IsLeave=0
	END

	--in case of start & end time same
	IF  CONVERT (DATE,@PV_ActualStart)=CONVERT (DATE,@PV_ActualEnd)--Check Date
		AND DATEPART(HOUR,@PV_ActualStart)=DATEPART(HOUR,@PV_ActualEnd) --Hour check
		AND DATEPART(MINUTE,@PV_ActualStart)=DATEPART(MINUTE,@PV_ActualEnd)--Min Check
	BEGIN
		SET @PV_ActualEnd=(SELECT DATEADD(SECOND,0,DATEADD (MINUTE,0, DATEADD(HOUR, 0, DATEADD(dd, DATEDIFF(dd, -0, @PV_DayEndTime), 0)))))
		--find early
		SET @PV_EarlyInMunit=DATEDIFF(MINUTE,@PV_ActualStart,@PV_EndTime)
	END	


	--In case of 00:00 hour (12 AM)
	IF DATEPART(HOUR,@PV_ActualStart)=0 AND DATEPART(MINUTE,@PV_ActualStart)=0 AND DATEPART(SECOND,@PV_ActualStart)!=0 
	BEGIN
		SET @PV_ActualStart=(SELECT DATEADD(SECOND,0,DATEADD (MINUTE,1, DATEADD(HOUR, 0, DATEADD(dd, DATEDIFF(dd, -0, @PV_ActualStart), 0)))))
	END
	--In case of 00:00 hour (12 AM)
	IF DATEPART(HOUR,@PV_ActualEnd)=0 AND DATEPART(MINUTE,@PV_ActualEnd)=0 AND DATEPART(SECOND,@PV_ActualEnd)!=0 
	BEGIN
		SET @PV_ActualEnd=(SELECT DATEADD(SECOND,0,DATEADD (MINUTE,1, DATEADD(HOUR, 0, DATEADD(dd, DATEDIFF(dd, -0, @PV_ActualEnd), 0)))))
	END

	--Late	
	IF @PV_ActualStart>@PV_TolaranceTime
	BEGIN
		SET @PV_LateInMunit=DATEDIFF(MINUTE,@PV_StartTime,@PV_ActualStart)
	END
	IF @Param_LateArrivalMinute=0
	BEGIN
		SET @PV_LateInMunit=@Param_LateArrivalMinute
	END

	--find early	
	IF CAST(@PV_ActualEnd AS TIME(0))!='00:00' AND @PV_ActualEnd<@PV_EndTime
	BEGIN
		SET @PV_EarlyInMunit=DATEDIFF(MINUTE,@PV_ActualEnd,@PV_EndTime)
	END
	--find early
	IF CAST(@PV_ActualEnd AS TIME(0))='00:00' AND CAST(@PV_ActualStart AS TIME(0))='00:00'
	BEGIN
		SET @PV_EarlyInMunit=0
	END
	IF @Param_EarlyDepartureMinute=0
	BEGIN
		SET @PV_EarlyInMunit=@Param_EarlyDepartureMinute
	END		
	
	IF (@PV_BaseAddress='amg') AND (@PV_IsDayOff=1 OR @PV_IsHoliday=1 OR @Param_IsOSD=1 OR @PV_IsLeave=1)
	BEGIN
		SET @PV_LateInMunit=0
		SET @PV_EarlyInMunit=0
	END				 
	--find total working hour
	IF CAST(@PV_ActualStart AS TIME(0))!='00:00' AND CAST(@PV_ActualEnd AS TIME(0))!='00:00'
	BEGIN
		SET @PV_TotalWorkingHourInMin=DATEDIFF(MINUTE,@PV_ActualStart,@PV_ActualEnd) 										 
	END	
	
	SET @PV_LateInMunit=CASE WHEN @PV_LateInMunit<0 THEN 0 ELSE @PV_LateInMunit END		
	SET @PV_EarlyInMunit=CASE WHEN @PV_EarlyInMunit<0 THEN 0 ELSE @PV_EarlyInMunit END	
	SET @PV_TotalWorkingHourInMin= CASE WHEN @PV_TotalWorkingHourInMin<0 THEN 0 ELSE @PV_TotalWorkingHourInMin END								

	/****************Over time in minute ******************************/
		
		DECLARE @PV_OverTimeCalculateInMinAfter as int, @PV_OTMin as int, @PV_TotalWHourForOT as int
				, @PV_IsOTFromDayStart as bit, @PV_BrkTimeInMin as int , @PV_BrkTime as int 

		SET @PV_OTMin=0
		SET @PV_BrkTime=0

		--Check OT From Slab or Not
		IF EXISTS (SELECT * FROM ShiftOTSlab WITH (NOLOCK) WHERE ShiftID=@Param_ShiftID AND ISNULL(AchieveOTInMin,0)>0 AND IsActive=1)BEGIN SET @PV_IsOTFromSlab=1 END
		IF EXISTS (SELECT * FROM ShiftOTSlab WITH (NOLOCK) WHERE ShiftID=@Param_ShiftID AND ISNULL(CompAchieveOTInMin,0)>0 AND IsCompActive=1)BEGIN SET @PV_IsCompOTFromSlab=1 END


		IF(@Param_IsMAnualOT=1) BEGIN SET @PV_OTMin=@Param_OverTimeInMinute GOTO CONT;  END-- edited by azhar
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
						SET @PV_TotalWHourForOT=DATEDIFF(MINUTE,@PV_StartTime,@PV_ActualEnd)
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
					IF @PV_ActualStart>@PV_Shift_OTStart
					BEGIN
						SET @PV_OTMin=DATEDIFF(MINUTE,@PV_ActualStart,CASE WHEN @PV_Shift_OTEnd>@PV_ActualEnd THEN @PV_ActualEnd ELSE @PV_Shift_OTEnd END)+1												
					END
					ELSE BEGIN
						SET @PV_OTMin=DATEDIFF(MINUTE,@PV_Shift_OTStart,CASE WHEN @PV_Shift_OTEnd>@PV_ActualEnd THEN @PV_ActualEnd ELSE @PV_Shift_OTEnd END)+1
					END
				END--Before
				--IF OT Begain After Shift End
				IF @PV_Shift_OTStart>=@PV_EndTime 
				BEGIN--After
					IF (@PV_ActualEnd<@PV_Shift_OTEnd)
					BEGIN
						SET @PV_OTMin=DATEDIFF(MINUTE,@PV_Shift_OTStart,@PV_ActualEnd)+1	
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

					SET @PV_BrkTime=ISNULL((SELECT SUM(DATEDIFF(MINUTE,CASE WHEN BStart>@PV_ActualStart THEN BStart ELSE  @PV_ActualStart END,
									CASE WHEN BEnd>@PV_ActualEnd THEN @PV_ActualEnd ELSE BEnd END) ) FROM @tbl_BreakTime WHERE BStart BETWEEN 
									CASE WHEN @PV_ActualStart>@PV_Shift_OTStart THEN @PV_ActualStart ELSE  @PV_Shift_OTStart END
									AND  CASE WHEN @PV_Shift_OTEnd>@PV_ActualEnd THEN @PV_ActualEnd ELSE @PV_Shift_OTEnd END),0)
					IF @PV_BrkTime<0 BEGIN SET @PV_BrkTime=0 END
				END 

				SET @PV_OTMin=@PV_OTMin-@PV_BrkTime
				IF (@PV_OTMin>@PV_Shift_OTCalclateAfterInMin)--Check minimum working hour after Out time
				BEGIN

					IF (@PV_IsCompOTFromSlab=1)
					BEGIN
						SET @PV_CompSlabOTInMin=ISNULL((SELECT top(1)CompAchieveOTInMin FROM ShiftOTSlab WHERE IsCompActive=1 AND ShiftID=@Param_ShiftID 
										AND @PV_OTMin BETWEEN CompMinOTInMin AND CompMaxOTInMin),0)
					END

					IF (@PV_IsOTFromSlab=1)
					BEGIN
						SET @PV_OTMin=ISNULL((SELECT top(1) AchieveOTInMin FROM ShiftOTSlab WHERE IsActive=1 AND ShiftID=@Param_ShiftID 
										AND @PV_OTMin BETWEEN MinOTInMin AND MaxOTInMin),0)
					END
				END
				ELSE BEGIN
					SET @PV_OTMin=0
				END--Check minimum working hour after Out time
			END--From Shift

			---Final OT after checking Max OT from roster
			IF (@PV_OTMin>0 AND @PV_MaxOTInMinATRoster>0 AND @PV_OTMin>@PV_MaxOTInMinATRoster)
			BEGIN
				SET @PV_OTMin=@PV_MaxOTInMinATRoster
			END
		END--WorkingHour
		SET @PV_OTMin=CASE WHEN @PV_OTMin<0 THEN 0 ELSE @PV_OTMin END
		CONT:
	/****************End Over time in minute**************************/

		--Compliance Intime, EndTime, Working Hour,Overtime
		SET @PV_CompInTime=@PV_ActualStart
		SET @PV_CompOutTime=@PV_ActualEnd
		SET @PV_CompTotalWorkingHourInMin=@PV_TotalWorkingHourInMin
		SET @PV_CompOverTimeInMin=@PV_OTMin

		IF EXISTS (SELECT * FROM HRM_Shift WITH (NOLOCK) WHERE ShiftID=@Param_ShiftID AND CompMaxEndTime IS NOT NULL)
		BEGIN
			SET @PV_CompMaxCompOTInMin=0
			SET @PV_CompMaxCompOTInMin=DATEDIFF(MINUTE,@PV_EndTime,@PV_CompMaxEndTime)+1
			SET @PV_CompOutTime=CASE WHEN @PV_ActualEnd>@PV_CompMaxEndTime AND @PV_OTMin<=0 THEN DATEADD(MINUTE,RAND()*(-10),@PV_CompMaxEndTime) ELSE @PV_ActualEnd END
		END
			

		IF (@PV_CompIsDayOff=1 OR @PV_CompIsHoliday=1)
		BEGIN
			SET @PV_CompInTime=(DATEADD(SECOND,0,DATEADD (MINUTE,0, DATEADD(HOUR, 0, DATEADD(dd, DATEDIFF(dd, -0, @PV_AttendanceDate), 0)))) )
			SET @PV_CompOutTime=(DATEADD(SECOND,0,DATEADD (MINUTE,0, DATEADD(HOUR, 0, DATEADD(dd, DATEDIFF(dd, -0, @PV_AttendanceDate), 0)))) )
			SET @PV_CompTotalWorkingHourInMin=0
			SET @PV_CompOverTimeInMin=0					
		END ELSE BEGIN
			IF (@PV_TotalWorkingHourInMin>0 )--AND @PV_CompMaxCompOTInMin>0 AND @PV_CompMaxCompOTInMin<@PV_OTMin
			BEGIN
				IF @PV_CompMaxCompOTInMin>0 AND @PV_CompMaxCompOTInMin<@PV_OTMin AND @PV_IsCompOTFromSlab=0
				BEGIN
					SET @PV_CompOverTimeInMin=@PV_CompMaxCompOTInMin
					SET @PV_CompOutTime=DATEADD(MINUTE,RAND()*10,(DATEADD(MINUTE,@PV_CompOverTimeInMin, @PV_EndTime)))
					SET @PV_CompTotalWorkingHourInMin=DATEDIFF(MINUTE,@PV_ActualStart,@PV_CompOutTime)
				END

				IF (@PV_IsCompOTFromSlab=1)
				BEGIN
					SET @PV_CompOverTimeInMin=@PV_CompSlabOTInMin
					SET @PV_CompOutTime=DATEADD(MINUTE,RAND()*10,(DATEADD(MINUTE,@PV_CompSlabOTInMin, @PV_EndTime)))
					SET @PV_CompTotalWorkingHourInMin=DATEDIFF(MINUTE,@PV_ActualStart,@PV_CompOutTime)
				END
			END
		END
		SET @PV_CompOverTimeInMin=CASE WHEN @PV_CompOverTimeInMin<0 THEN 0 ELSE @PV_CompOverTimeInMin END

		--TimeKeeper Out Time
		SET @PV_TimeKeeperOutTime=@PV_ActualEnd
		IF @PV_OTMin>0
		BEGIN
			SET @PV_TimeKeeperOutTime=DATEADD(MINUTE,RAND()*10,DATEADD(MINUTE,@PV_OTMin, @PV_EndTime))
		END



	/***************Set Description************************************/
	SET @PV_HistoryDescription=''
	SET @PV_Msg=''
	IF NOT EXISTS (SELECT * FROM AttendanceDaily WHERE AttendanceID=@Param_AttendanceID AND ShiftID=@Param_ShiftID)
	BEGIN
		SET @PV_Msg=''
		SET @PV_Msg=(SELECT HRM_Shift FROM View_AttendanceDaily WHERE AttendanceID=@Param_AttendanceID)
		SET @PV_HistoryDescription +='Shift-'+@PV_Msg+','
	END
	IF NOT EXISTS (SELECT * FROM AttendanceDaily WHERE AttendanceID=@Param_AttendanceID AND IsOSD=@Param_IsOSD)
	BEGIN
		SET @PV_Msg=''
		IF @Param_IsOSD=1 BEGIN SET @PV_Msg='No OSD' END ELSE BEGIN SET @PV_Msg='OSD' END
		SET @PV_HistoryDescription +=@PV_Msg+','
	END

	IF NOT EXISTS (SELECT * FROM AttendanceDaily WHERE AttendanceID=@Param_AttendanceID AND LateArrivalMinute=@PV_LateInMunit)
	BEGIN
		SET @PV_Msg=''
		SET @PV_Msg=(SELECT CONVERT(VARCHAR(50),LateArrivalMinute) FROM AttendanceDaily WHERE AttendanceID=@Param_AttendanceID)
		SET @PV_HistoryDescription +='Late-'+@PV_Msg+','
	END
	IF NOT EXISTS (SELECT * FROM AttendanceDaily WHERE AttendanceID=@Param_AttendanceID AND EarlyDepartureMinute=@PV_EarlyInMunit)
	BEGIN
		SET @PV_Msg=''
		SET @PV_Msg=(SELECT CONVERT(VARCHAR(50),EarlyDepartureMinute) FROM AttendanceDaily WHERE AttendanceID=@Param_AttendanceID)
		SET @PV_HistoryDescription +='Early-'+@PV_Msg+','
	END
	
	IF NOT EXISTS (SELECT * FROM AttendanceDaily WHERE AttendanceID=@Param_AttendanceID AND ISNULL(Remark,'')=@Param_Remark)
	BEGIN		
		SET @PV_Msg=''
		SET @PV_Msg=ISNULL((SELECT Remark FROM AttendanceDaily WHERE AttendanceID=@Param_AttendanceID),'')
		SET @PV_HistoryDescription +='Remark-'+@PV_Msg+','
	END

	IF NOT EXISTS (SELECT * FROM AttendanceDaily WHERE AttendanceID=@Param_AttendanceID AND IsDayOff=@param_IsDayoff)
	BEGIN
		SET @PV_Msg=''
		SET @PV_HistoryDescription +='Dayoff-'+@PV_Msg+','
	END

	IF NOT EXISTS (SELECT * FROM AttendanceDaily WHERE AttendanceID=@Param_AttendanceID AND InTime=@PV_ActualStart)
	BEGIN
		SET @PV_Msg='IN'
	END

	IF NOT EXISTS (SELECT * FROM AttendanceDaily WHERE AttendanceID=@Param_AttendanceID AND OutTime=@PV_ActualEnd)
	BEGIN
		SET @PV_Msg='OUT'		
	END



	IF @PV_Msg IN ('IN','OUT')
	BEGIN
		SET @PV_Msg=(SELECT CONVERT(Varchar(5), CAST(InTime AS TIME (0)))+'-'+CONVERT(Varchar(5), CAST(OutTime as TIME (0))) 
					FROM AttendanceDaily WHERE AttendanceID=@Param_AttendanceID)
		SET @PV_HistoryDescription +='Att-'+@PV_Msg+','		
	END

	IF @PV_HistoryDescription=''
	BEGIN
		SET @PV_HistoryDescription='No Update'
	END ELSE BEGIN
		--SET @PV_HistoryDescription='Update On: '+@PV_HistoryDescription
		SET @PV_HistoryDescription=SUBSTRING( @PV_HistoryDescription ,0 ,LEN(@PV_HistoryDescription))  
	END

	IF @Param_IsAbsent=1
	BEGIN
		SET @PV_IsDayOff=0
		SET @PV_IsHoliday=0
		SET @Param_IsOSD=0
	END
 

	/***************END Set Description************************************/

	--update Attendance daily
	IF @PV_BaseAddress IN ('amg','golden')
	BEGIN
		UPDATE [AttendanceDaily]
		SET 
		--[EmployeeID]=@PV_EmployeeID,[AttendanceSchemeID]=@PV_AttendanceSchemeID,[LocationID]=@PV_LocationID,
		--[DepartmentID]=@PV_DepartmentID,[DesignationID]=@PV_DesignationID,[RosterPlanID]=@PV_RosterPlanID
		[ShiftID]=@Param_ShiftID--,[AttendanceDate]=@PV_AttendanceDate
		,[InTime]=@PV_ActualStart,[OutTime]=@PV_ActualEnd
		--,[CompInTime]=@PV_CompInTime,[CompOutTime]=@PV_CompOutTime
		,[LateArrivalMinute]=@PV_LateInMunit,[EarlyDepartureMinute]=@PV_EarlyInMunit
		,[TotalWorkingHourInMinute]=@PV_TotalWorkingHourInMin,[OverTimeInMinute]=@PV_OTMin
		--,[CompLateArrivalMinute]=@PV_LateInMunit,[CompEarlyDepartureMinute]=@PV_EarlyInMunit
		--,[CompTotalWorkingHourInMinute]=@PV_CompTotalWorkingHourInMin,[CompOverTimeInMinute]=@PV_CompOverTimeInMin
		,[IsDayOff]=@PV_IsDayOff
		--,[IsCompDayOff]=@PV_CompIsDayOff
		,[IsLeave]=@PV_IsLeave,[IsUnPaid]=@PV_LeaveIsUnPaid
		,[IsHoliday]=@PV_IsHoliday
		--,[IsCompHoliday]=@PV_CompIsHoliday

		,[WorkingStatus]=1,[Note]='Manual',
		--[APMID]=@PV_APMID,[IsLock]=0,[IsNoWork]=0
		IsManual=1
		,LeaveHeadID=@PV_LeaveHeadID
		,IsOSD=@Param_IsOSD
		,TimeKeeperOutTime=@PV_TimeKeeperOutTime
		,Remark=@Param_Remark
		,IsManualOT=@Param_IsManualOT
		WHERE AttendanceID= @Param_AttendanceID
	END ELSE BEGIN
		UPDATE [AttendanceDaily]
		SET 
		--[EmployeeID]=@PV_EmployeeID,[AttendanceSchemeID]=@PV_AttendanceSchemeID,[LocationID]=@PV_LocationID,
		--[DepartmentID]=@PV_DepartmentID,[DesignationID]=@PV_DesignationID,[RosterPlanID]=@PV_RosterPlanID
		[ShiftID]=@Param_ShiftID--,[AttendanceDate]=@PV_AttendanceDate
		,[InTime]=@PV_ActualStart,[OutTime]=@PV_ActualEnd
		,[CompInTime]=@PV_CompInTime,[CompOutTime]=@PV_CompOutTime
		,[LateArrivalMinute]=@PV_LateInMunit,[EarlyDepartureMinute]=@PV_EarlyInMunit
		,[TotalWorkingHourInMinute]=@PV_TotalWorkingHourInMin,[OverTimeInMinute]=@PV_OTMin
		,[CompLateArrivalMinute]=@PV_LateInMunit,[CompEarlyDepartureMinute]=@PV_EarlyInMunit
		,[CompTotalWorkingHourInMinute]=@PV_CompTotalWorkingHourInMin,[CompOverTimeInMinute]=@PV_CompOverTimeInMin
		,[IsDayOff]=@PV_IsDayOff
		,[IsCompDayOff]=@PV_CompIsDayOff
		,[IsLeave]=@PV_IsLeave,[IsUnPaid]=@PV_LeaveIsUnPaid
		,[IsHoliday]=@PV_IsHoliday
		,[IsCompHoliday]=@PV_CompIsHoliday

		,[WorkingStatus]=1,[Note]='Manual',
		--[APMID]=@PV_APMID,[IsLock]=0,[IsNoWork]=0
		IsManual=1
		,LeaveHeadID=@PV_LeaveHeadID
		,IsOSD=@Param_IsOSD
		,TimeKeeperOutTime=@PV_TimeKeeperOutTime
		,Remark=@Param_Remark
		,IsManualOT=@Param_IsManualOT
		WHERE AttendanceID= @Param_AttendanceID
	END
	--Insert Manual history
	 INSERT INTO [AttendanceDailyManualHistory]   ([AttendanceID],[Description],[DBUSerID],[DBServerDateTime])
     VALUES    (@Param_AttendanceID,@PV_HistoryDescription,@Param_DBUserID,GETDATE())	

	--Benefits On Attendance
	DECLARE 
	@PV_BOAID as int
	,@PV_BOAELID as int
	,@PV_BOAEmployeeID as int
	,@PV_BOA_BenefitOn as smallint
	,@PV_BOA_StartTime as datetime
	,@PV_BOA_EndTime as datetime				
	,@PV_BOA_OTInMinute as int
	,@PV_BOA_OTDistributePerPresence as int				
	,@PV_BOA_IsContinous as Bit
	,@PV_BOA_HolidayID as int
	,@PV_BOA_BenefitStartDate as Date
	,@PV_BOA_BenefitEndDate as Date
	,@PV_BOA_IsFullOT as bit
	,@PV_BOA_LeaveHeadID as int
	,@PV_BOA_LeaveAmount as decimal (18,2)
	,@PV_BOA_IsExtraBenefit as bit
	,@PV_BOA_IsPercent as bit
	,@PV_BOA_value as decimal(18,2)
	,@PV_BOA_AllowanceOn as smallint
	,@PV_OverTimeDayCount as int
	,@PV_LastSalaryDate as Date
	,@PV_IsBenifited as bit
	,@PV_IsTemporaryAssign as bit
	,@PV_ACSID as int
	,@PV_EmpLeaveLedgerID as int
	,@PV_BenefitValue as decimal(18,2)
	,@PV_BOATS_ValueInPercent as decimal(18,2)
	,@PV_BenefitDurationInMinute as int
	,@PV_BOA_ToleranceInMin as int	

	--Delete BOA 
	DELETE FROM BenefitOnAttendanceEmployeeLedger WHERE  BOAEmployeeID IN (
	SELECT BOAEmployeeID FROM BenefitOnAttendanceEmployee WHERE EmployeeID=@PV_EmployeeID AND BOAID IN (
	SELECT BOAID FROM BenefitOnAttendance WHERE LeaveHeadID=0 OR LeaveHeadID IS NULL))
	ANd AttendanceDate=@PV_AttendanceDate

	DECLARE Cur_BOA CURSOR LOCAL FORWARD_ONLY KEYSET FOR
	SELECT BOAID,BOAEmployeeID,IsTemporaryAssign FROM BenefitOnAttendanceEmployee WHERE EmployeeID=@PV_EmployeeID AND (InactiveDate IS NULL OR InactiveDate>@PV_AttendanceDate)
	--AND BOAEmployeeID NOT IN (SELECT BOAEmployeeID FROM BenefitOnAttendanceEmployeeStopped WHERE @PV_AttendanceDate
	--BETWEEN StartDate AND CASE WHEN InactiveDate IS NULL THEN EndDate ELSE InactiveDate END)
	OPEN Cur_BOA
	FETCH NEXT FROM Cur_BOA INTO  @PV_BOAID,@PV_BOAEmployeeID,@PV_IsTemporaryAssign
	WHILE(@@Fetch_Status <> -1)		
	BEGIN
			--Check Temporary Stop
			IF EXISTS (SELECT * FROM BenefitOnAttendanceEmployeeStopped WHERE BOAEmployeeID=@PV_BOAEmployeeID AND @PV_AttendanceDate BETWEEN StartDate AND EndDate)
			BEGIN
				GOTO CONT_BOA;
			END

			--Check Temporary Assign
			IF @PV_IsTemporaryAssign=1
			BEGIN
				IF NOT EXISTS (SELECT * FROM BenefitOnAttendanceEmployeeAssign WHERE BOAEmployeeID=@PV_BOAEmployeeID AND @PV_AttendanceDate BETWEEN StartDate AND EndDate)
				BEGIN
					GOTO CONT_BOA;
				END
			END
			--Initialize value
			SET @PV_BOA_BenefitOn=0
			SET @PV_BOA_StartTime=NULL 
			SET @PV_BOA_EndTime=NULL 
			SET @PV_BOA_OTInMinute=0
			SET @PV_BOA_OTDistributePerPresence=0
			SET @PV_BOA_HolidayID=0
			SET @PV_BOA_IsContinous=0
			SET @PV_BOA_BenefitStartDate=NULL 
			SET @PV_BOA_BenefitEndDate=NULL
			SET @PV_OverTimeDayCount=0
			SET @PV_IsBenifited=0
			SET @PV_BOAELID=0
			SET @PV_BOA_IsFullOT=0
			SET @PV_BOA_LeaveHeadID=0
			SET @PV_BOA_LeaveAmount=0
			SET @PV_ACSID=ISNULL((SELECT ACSID FROM AttendanceCalendarSession WHERE @PV_AttendanceDate BETWEEN StartDate AND EndDate AND AttendanceCalendarID IN (SELECT AttendanceCalenderID FROm AttendanceScheme WHERE AttendanceSchemeID IN (
								  SELECT AttendanceSchemeID FROM EmployeeOfficial WHERE EmployeeID=@PV_EmployeeID))),0)
			SET @PV_EmpLeaveLedgerID=0
			SET @PV_BOA_IsExtraBenefit=0
			SET @PV_BOA_IsPercent=0
			SET @PV_BOA_value=0
			SET @PV_BenefitValue=0
			SET @PV_BOA_AllowanceOn=0
			SET @PV_BOATS_ValueInPercent=0
			SET @PV_BenefitDurationInMinute=0


			SELECT
			@PV_BOA_BenefitOn=[BenefitOn]
			,@PV_BOA_StartTime=[StartTime]
			,@PV_BOA_EndTime=DATEADD(MINUTE,-TolarenceInMinute,[EndTime])
			,@PV_BOA_OTInMinute=[OTInMinute]
			,@PV_BOA_OTDistributePerPresence=[OTDistributePerPresence]
			,@PV_BOA_IsFullOT=IsFullWorkingHourOT
			,@PV_BOA_HolidayID=[HolidayID]
			,@PV_BOA_IsContinous=[IsContinous]
			,@PV_BOA_BenefitStartDate=[BenefitStartDate]
			,@PV_BOA_BenefitEndDate=[BenefitEndDate]
			,@PV_BOA_LeaveHeadID=LeaveHeadID
			,@PV_BOA_LeaveAmount=LeaveAmount
			,@PV_BOA_IsExtraBenefit=ISNULL(IsExtraBenefit,0)
			,@PV_BOA_IsPercent=IsPercent
			,@PV_BOA_value=Value
			,@PV_BOA_AllowanceOn=AllowanceOn
			,@PV_BOA_ToleranceInMin=ISNULL(TolarenceInMinute,0)
			FROM [BenefitOnAttendance] WHERE BOAID=@PV_BOAID

			--If any BOA has BOA Valu slab than value slab is more prioritize
			IF EXISTS (SELECT * FROM BenefitOnAttendanceValueSlab WHERE BOAID=@PV_BOAID)
			BEGIN
				--Get Gross Salary of employee
				DECLARE @PV_Gross as decimal (18,2)=0
				SET @PV_Gross=ISNULL((SELECT ActualGrossAmount FROM EmployeeSalaryStructure WHERE EmployeeID=@PV_EmployeeID),0)
				SET @PV_BOA_value =ISNULL((SELECT top(1)Value FROM BenefitOnAttendanceValueSlab WHERE BOAID=@PV_BOAID AND @PV_Gross BETWEEN MinGross AND MaxGross),0)
			END

			--Check Leave benefit
			IF @PV_BOA_LeaveHeadID>0 AND @PV_BOA_LeaveAmount>0
			BEGIN						
				IF EXISTS (SELECT * FROM BenefitOnAttendanceEmployeeLedger WHERE BOAEmployeeID=@PV_BOAEmployeeID AND AttendanceDate=@PV_AttendanceDate)
				BEGIN
					SET @PV_EmpLeaveLedgerID=ISNULL((SELECT EmpLeaveLedgerID FROM EmployeeLeaveLedger	
											WHERE ACSID =@PV_ACSID AND LeaveID=@PV_BOA_LeaveHeadID	
											AND EmployeeID=@PV_EmployeeID),0)

					UPDATE EmployeeLeaveLedger SET TotalDay=TotalDay-@PV_BOA_LeaveAmount
					WHERE EmpLeaveLedgerID=@PV_EmpLeaveLedgerID

					DELETE BenefitOnAttendanceEmployeeLedger WHERE BOAEmployeeID=@PV_BOAEmployeeID AND AttendanceDate=@PV_AttendanceDate
				END
			END

			IF @PV_BOA_IsContinous=0 AND @PV_AttendanceDate NOT BETWEEN @PV_BOA_BenefitStartDate AND @PV_BOA_BenefitEndDate
			BEGIN
				GOTO CONT_BOA;
			END

			IF (@PV_ActualStart>DATEADD(MINUTE,10,@PV_StartTime) OR @PV_ActualEnd<@PV_EndTime) AND (@PV_LateInMunit>0 OR @PV_EarlyInMunit>0) AND @PV_BOA_BenefitOn!=2 AND @PV_BOA_OTInMinute=0 AND @PV_BOA_IsFullOT=0 AND @PV_BaseAddress='mamiya'
			BEGIN				
				GOTO CONT_BOA;
			END

			IF (@PV_BOA_BenefitOn=1 AND (@PV_IsHoliday=1 OR @PV_IsDayOff=1) AND CAST(@PV_ActualStart AS TIME(0))!='00:00:00')
			BEGIN----DayOff_Holiday_Presence
				IF @PV_BOA_HolidayID>0--Check Special Holiday
				BEGIN
					IF EXISTS (SELECT * FROM AttendanceCalendarSessionHoliday WHERE HolidayID=@PV_BOA_HolidayID AND @PV_AttendanceDate BETWEEN StartDate AND EndDate)
					BEGIN
						SET @PV_IsBenifited=1
					END
				END ELSE BEGIN
					SET @PV_IsBenifited=1
				END
			END--End DayOff_Holiday_Presence Scope

			IF (@PV_BOA_BenefitOn=2 AND CAST(@PV_ActualStart AS TIME(0))!='00:00:00' AND CAST(@PV_ActualEnd AS TIME(0))!='00:00:00')
			BEGIN--Time Slot						

				IF (DATEPART(HOUR,@PV_BOA_StartTime))>(DATEPART(HOUR,@PV_DayStartTime))
				BEGIN--Same date

					SET @PV_BOA_StartTime=(DATEADD(SECOND,0,DATEADD (MINUTE,DATEPART(MINUTE,@PV_BOA_StartTime), DATEADD(HOUR, DATEPART(HOUR,@PV_BOA_StartTime), 
											DATEADD(dd, DATEDIFF(dd, -0, @PV_AttendanceDate), 0)))))
				END
				ELSE BEGIN--Different date
					SET @PV_BOA_StartTime=(DATEADD(SECOND,0,DATEADD (MINUTE,DATEPART(MINUTE,@PV_BOA_StartTime), DATEADD(HOUR, DATEPART(HOUR,@PV_BOA_StartTime), 
											DATEADD(dd, DATEDIFF(dd, -1, @PV_AttendanceDate), 0)))))
				END

				IF (DATEPART(HOUR,@PV_BOA_EndTime))>(DATEPART(HOUR,@PV_DayStartTime))
				BEGIN--Same date

					SET @PV_BOA_EndTime=(DATEADD(SECOND,0,DATEADD (MINUTE,DATEPART(MINUTE,@PV_BOA_EndTime)
										, DATEADD(HOUR, DATEPART(HOUR,@PV_BOA_EndTime), 
											DATEADD(dd, DATEDIFF(dd, -0, @PV_AttendanceDate), 0)))))
				END
				ELSE BEGIN----Different date
					SET @PV_BOA_EndTime=(DATEADD(SECOND,0,DATEADD (MINUTE,DATEPART(MINUTE,@PV_BOA_EndTime)
										, DATEADD(HOUR, DATEPART(HOUR,@PV_BOA_EndTime), 
											DATEADD(dd, DATEDIFF(dd, -1, @PV_AttendanceDate), 0)))))
				END
											  											  	
				IF @PV_BOA_StartTime BETWEEN @PV_ActualStart AND @PV_ActualEnd
				BEGIN
					IF @PV_ActualEnd>=@PV_BOA_EndTime	BEGIN	SET @PV_BenefitDurationInMinute=DATEDIFF(MINUTE,@PV_BOA_StartTime,@PV_BOA_EndTime)+1
														END ELSE BEGIN SET @PV_BenefitDurationInMinute=DATEDIFF(MINUTE,@PV_BOA_StartTime,@PV_ActualEnd)+1 END

					--IF @PV_ActualEnd >= @PV_BOA_EndTime--Check Out time
					IF (@PV_BenefitDurationInMinute-@PV_BOA_ToleranceInMin)>0 AND @PV_ActualStart<=@PV_BOA_StartTime AND @PV_ActualEnd >=@PV_BOA_EndTime
					BEGIN								
						IF @PV_BOA_HolidayID>0 --check time slot in special holiday 
						BEGIN
							IF EXISTS (SELECT * FROM AttendanceCalendarSessionHoliday WHERE HolidayID=@PV_BOA_HolidayID AND @PV_AttendanceDate BETWEEN StartDate AND EndDate)
							BEGIN
								SET @PV_IsBenifited=1
							END
						END ELSE BEGIN
							SET @PV_IsBenifited=1
						END
					END
				END
			END--End Time Slot

			IF (@PV_BOA_BenefitOn=3 AND @PV_IsDayOff=1 AND CAST(@PV_ActualStart AS TIME(0))!='00:00:00')
			BEGIN----OnlyDayOff_Presence
				IF @PV_BOA_HolidayID>0--Check Special Holiday
				BEGIN
					IF EXISTS (SELECT * FROM AttendanceCalendarSessionHoliday WHERE HolidayID=@PV_BOA_HolidayID AND @PV_AttendanceDate BETWEEN StartDate AND EndDate)
					BEGIN
						SET @PV_IsBenifited=1
					END
				END ELSE BEGIN
					SET @PV_IsBenifited=1
				END
			END--End OnlyDayOff_Presence Scope

			IF (@PV_BOA_BenefitOn=4 AND @PV_IsHoliday=1 AND CAST(@PV_ActualStart AS TIME(0))!='00:00:00')
			BEGIN----OnlyDayOff_Presence
				IF @PV_BOA_HolidayID>0--Check Special Holiday
				BEGIN
					IF EXISTS (SELECT * FROM AttendanceCalendarSessionHoliday WHERE HolidayID=@PV_BOA_HolidayID AND @PV_AttendanceDate BETWEEN StartDate AND EndDate)
					BEGIN
						SET @PV_IsBenifited=1
					END
				END ELSE BEGIN
					SET @PV_IsBenifited=1
				END
			END--End OnlyDayOff_Presence Scope

			IF (@PV_IsBenifited=1)
			BEGIN
				/*In case of Over time benefit it will be benefited in this process*/
				IF (@PV_BOA_OTInMinute>0 AND @PV_BOA_IsFullOT=0 AND @Param_IsManualOT=0)
				BEGIN--Start OT Benefit
					IF (@PV_BOA_OTDistributePerPresence>0)
					BEGIN--OTDistributePerPresence
						SET @PV_OverTimeDayCount=@PV_BOA_OTInMinute/@PV_BOA_OTDistributePerPresence
						--get last salaryDate
						SELECT @PV_LastSalaryDate=MAX(EndDate) FROM EmployeeSalary WHERE EmployeeID=@PV_EmployeeID
						--Get previous present day
						IF (@PV_LastSalaryDate IS NOT NULL)
						BEGIN
							UPDATE 	top(@PV_OverTimeDayCount)AttendanceDaily 
							SET OverTimeInMinute=@PV_BOA_OTDistributePerPresence
							WHERE EmployeeID=@PV_EmployeeID AND AttendanceDate>	@PV_LastSalaryDate
							AND OverTimeInMinute<180							
						END ELSE BEGIN
							UPDATE 	top(@PV_OverTimeDayCount)AttendanceDaily 
							SET OverTimeInMinute=@PV_BOA_OTDistributePerPresence
							WHERE EmployeeID=@PV_EmployeeID --AND AttendanceDate>	@PV_LastSalaryDate
							AND OverTimeInMinute<180
						END
					END--OTDistributePerPresence
					ELSE BEGIN
						UPDATE AttendanceDaily SET OverTimeInMinute= @PV_BOA_OTInMinute 
						WHERE EmployeeID=@PV_EmployeeID AND AttendanceDate=@PV_AttendanceDate
					END
				END--END OT Benefit
				ELSE IF (@PV_BOA_IsFullOT=1 AND @Param_IsManualOT=0)
				BEGIN
					SET @PV_BOA_OTInMinute=0
					SET @PV_BrkTime=0
					SET @PV_BOA_IsOTSlab=0

					IF EXISTS (SELECT * FROM BenefitOnAttendanceOTSlab WITH (NOLOCK) WHERE BOAID=@PV_BOAID AND OTInMin>0)
					BEGIN
						SET @PV_BOA_IsOTSlab=1
					END

					IF @PV_BOA_IsOTSlab=0
					BEGIN--OTSlab
						--Find Working as Shift Start to End
						IF @PV_ActualEnd>@PV_EndTime
						BEGIN
							--Find brk time between Shift						
							IF EXISTS (SELECT * FROM @tbl_BreakTime WHERE BStart BETWEEN @PV_StartTime AND  @PV_EndTime)
							BEGIN
								SET @PV_BrkTime=(SELECT SUM(DATEDIFF(MINUTE,aa.BStart,aa.BEnd)) FROM
												(SELECT CASE WHEN BStart< @PV_StartTime THEN @PV_StartTime ELSE BStart END AS BStart
												,CASE WHEN BEnd> @PV_EndTime THEN @PV_EndTime ELSE BEnd END AS BEnd		
												FROM @tbl_BreakTime WHERE (BStart BETWEEN @PV_StartTime AND  @PV_EndTime) 
												OR (BEnd BETWEEN @PV_StartTime AND  @PV_EndTime))aa)
							END
						
							--IF DATEPART(MINUTE,@PV_StartTime)=DATEPART(MINUTE,@PV_EndTime)
							--BEGIN
							--	SET @PV_BOA_OTInMinute=DATEDIFF(MINUTE,@PV_StartTime,@PV_EndTime)-@PV_LateInMunit-ISNULL(@PV_BrkTime,0)+@PV_OTMin-@PV_EarlyInMunit
							--END ELSE BEGIN
							--	SET @PV_BOA_OTInMinute=DATEDIFF(MINUTE,@PV_StartTime,@PV_EndTime)+1-@PV_LateInMunit-ISNULL(@PV_BrkTime,0)+@PV_OTMin-@PV_EarlyInMunit
							--END
							IF (DATEPART(MINUTE,@PV_StartTime)=DATEPART(MINUTE,@PV_EndTime) OR @PV_BaseAddress='mamiya')
							BEGIN
								SET @PV_BOA_OTInMinute=DATEDIFF(MINUTE,CASE WHEN @PV_StartTime>@PV_ActualStart THEN @PV_StartTime ELSE @PV_ActualStart END ,CASE WHEN @PV_EndTime>@PV_ActualEnd THEN @PV_ActualEnd ELSE @PV_EndTime END)-ISNULL(@PV_BrkTime,0)+@PV_OTMin---@PV_LateInMunit-@PV_EarlyInMunit
							END ELSE BEGIN
								SET @PV_BOA_OTInMinute=DATEDIFF(MINUTE,CASE WHEN @PV_StartTime>@PV_ActualStart THEN @PV_StartTime ELSE @PV_ActualStart END ,CASE WHEN @PV_EndTime>@PV_ActualEnd THEN @PV_ActualEnd ELSE @PV_EndTime END)+1-ISNULL(@PV_BrkTime,0)+@PV_OTMin---@PV_EarlyInMunit-@PV_LateInMunit
							END
						
						END
						ELSE BEGIN 
							--Find brk time between ShiftStart & actual End						
							IF EXISTS (SELECT * FROM @tbl_BreakTime WHERE BStart BETWEEN @PV_StartTime AND  @PV_ActualEnd)
							BEGIN
								SET @PV_BrkTime=(SELECT SUM(DATEDIFF(MINUTE,aa.BStart,aa.BEnd)) FROM
												(SELECT CASE WHEN BStart< @PV_StartTime THEN @PV_StartTime ELSE BStart END AS BStart
												,CASE WHEN BEnd> @PV_ActualEnd THEN @PV_ActualEnd ELSE BEnd END AS BEnd		
												FROM @tbl_BreakTime WHERE (BStart BETWEEN @PV_StartTime AND  @PV_ActualEnd) 
												OR (BEnd BETWEEN @PV_StartTime AND  @PV_ActualEnd))aa)
							END

							IF (@PV_TotalWorkingHourInMin>DATEDIFF(MINUTE,@PV_StartTime,@PV_ActualEnd))
							BEGIN
								--IF DATEPART(MINUTE,@PV_StartTime)=DATEPART(MINUTE,@PV_ActualEnd) 
								--BEGIN
								--	SET @PV_BOA_OTInMinute=DATEDIFF(MINUTE,@PV_StartTime,@PV_ActualEnd)-@PV_LateInMunit-ISNULL(@PV_BrkTime,0)+@PV_OTMin--@PV_EarlyInMunit
								--END ELSE BEGIN
								--	SET @PV_BOA_OTInMinute=DATEDIFF(MINUTE,@PV_StartTime,@PV_ActualEnd)+1-@PV_LateInMunit-ISNULL(@PV_BrkTime,0)+@PV_OTMin--@PV_EarlyInMunit
								--END
								IF (DATEPART(MINUTE,@PV_StartTime)=DATEPART(MINUTE,@PV_ActualEnd)  OR @PV_BaseAddress='mamiya')
								BEGIN
									SET @PV_BOA_OTInMinute=DATEDIFF(MINUTE,CASE WHEN @PV_StartTime>@PV_ActualStart THEN @PV_StartTime ELSE @PV_ActualStart END,@PV_ActualEnd)-ISNULL(@PV_BrkTime,0)+@PV_OTMin--@PV_EarlyInMunit-@PV_LateInMunit
								END ELSE BEGIN
									SET @PV_BOA_OTInMinute=DATEDIFF(MINUTE,CASE WHEN @PV_StartTime>@PV_ActualStart THEN @PV_StartTime ELSE @PV_ActualStart END,@PV_ActualEnd)+1-ISNULL(@PV_BrkTime,0)+@PV_OTMin--@PV_EarlyInMunit-@PV_LateInMunit
								END
							END ELSE BEGIN
								SET @PV_BOA_OTInMinute=@PV_TotalWorkingHourInMin-@PV_LateInMunit-ISNULL(@PV_BrkTime,0)+@PV_OTMin
							END						
						END
					END--OT Slab
					ELSE BEGIN
						IF @PV_TotalWorkingHourInMin>0
						BEGIN
							SET @PV_BOA_OTInMinute=ISNULL((SELECT top(1)OTInMin FROM BenefitOnAttendanceOTSlab WITH (NOLOCK) 
													WHERE BOAID=@PV_BOAID AND @PV_TotalWorkingHourInMin BETWEEN StartMinute AND EndMinute),0)
						END
					END

					UPDATE AttendanceDaily SET OverTimeInMinute= @PV_BOA_OTInMinute 
					WHERE EmployeeID=@PV_EmployeeID AND AttendanceDate=@PV_AttendanceDate
				END

				--Find Benefit Hour for DAyOff,Holiday
				IF @PV_BOA_BenefitOn IN (1,3,4)
				BEGIN
					IF CAST(@PV_ActualEnd AS TIME(0))!='00:00:00'
					BEGIN
						SET @PV_BenefitDurationInMinute=@PV_TotalWorkingHourInMin
					END
				END

				/*Incase Of Leave benefit*/
				IF @PV_BOA_LeaveHeadID>0 AND @PV_BOA_LeaveAmount>0
				BEGIN
					SET @PV_EmpLeaveLedgerID=ISNULL((SELECT EmpLeaveLedgerID FROM EmployeeLeaveLedger	
											WHERE ACSID =@PV_ACSID AND LeaveID=@PV_BOA_LeaveHeadID	
											AND EmployeeID=@PV_EmployeeID),0)

					IF @PV_EmpLeaveLedgerID>0
					BEGIN
						UPDATE EmployeeLeaveLedger SET TotalDay=TotalDay+@PV_BOA_LeaveAmount
						WHERE EmpLeaveLedgerID=@PV_EmpLeaveLedgerID
					END 
					ELSE BEGIN
						INSERT INTO [dbo].[EmployeeLeaveLedger]
						([EmpLeaveLedgerID],[EmployeeID],[ACSID],[ASLID],[LeaveID],[DeferredDay],[ActivationAfter],[TotalDay],[IsLeaveOnPresence],[PresencePerLeave],[IsCarryForward],[MaxCarryDays],[DBUSerID],[DBServerDateTime])
						VALUES	((SELECT ISNULL(MAX(EmpLeaveLedgerID),0)+1 FROM EmployeeLeaveLedger)
						,@PV_EmployeeID,@PV_ACSID,0,@PV_BOA_LeaveHeadID,1,1,@PV_BOA_LeaveAmount,0,0,0,0,@Param_DBUserID,GETDATE())
					END
				END

				/*In case of Extra Benifit*/
				IF @PV_BOA_IsExtraBenefit=1 AND @PV_BOA_value>0
				BEGIN		
					DECLARE @PV_DaysOfMonth as int
					SET @PV_DaysOfMonth=ISNULL(DAY(EOMONTH(@PV_AttendanceDate)),1)				
					
					IF EXISTS (SELECT * FROM BenefitOnAttendanceTimeSlab WHERE BOAID=@PV_BOAID)
					BEGIN
						SET @PV_BOATS_ValueInPercent=ISNULL((SELECT top(1)Value FROM BenefitOnAttendanceTimeSlab WHERE BOAID=@PV_BOAID AND @PV_BenefitDurationInMinute BETWEEN StartMinute AND EndMinute),0)
					END ELSE
					BEGIN
						SET @PV_BOATS_ValueInPercent=NULL 
					END

					IF @PV_BOA_AllowanceOn=0 
					BEGIN									
						SET @PV_BenefitValue=@PV_BOA_value
					END ELSE IF @PV_BOA_AllowanceOn=1
					BEGIN
						SET @PV_BenefitValue=ISNULL((SELECT ActualGrossAmount FROM EmployeeSalaryStructure WHERE EmployeeID=@PV_EmployeeID),0)/@PV_DaysOfMonth
					END ELSE IF @PV_BOA_AllowanceOn=2
					BEGIN
						SET @PV_BenefitValue=ISNULL((SELECT Max(Amount) FROM EmployeeSalaryStructureDetail WHERE ESSID IN (
											SELECT ESSID FROM EmployeeSalaryStructure WHERE EmployeeID=@PV_EmployeeID)),0) /@PV_DaysOfMonth
					END ELSE BEGIN
						SET @PV_BenefitValue=0
					END

					IF @PV_BOATS_ValueInPercent IS NOT NULL 
					BEGIN
						SET @PV_BenefitValue=@PV_BenefitValue*@PV_BOATS_ValueInPercent/100
					END 

				END--Extra benefit
				--Incase of Salary Benefit
				IF @PV_BOA_value>0 AND @PV_BOA_IsExtraBenefit<>1
				BEGIN
					SET @PV_BenefitValue=@PV_BOA_value;
				END														

				--Insert Employee BOA Ledger
				SET @PV_BOAELID=(SELECT ISNULL(MAX(BOAELID),0)+1 FROM [BenefitOnAttendanceEmployeeLedger])
				INSERT INTO [BenefitOnAttendanceEmployeeLedger]
							([BOAELID],[BOAEmployeeID],[AttendanceDate],[Note],[IsBenefited],[BenefitedDate],[DBUserID],[DBServerDateTime],Value)
						VALUES(@PV_BOAELID,@PV_BOAEmployeeID,@PV_AttendanceDate,'N/A',0,NULL,@Param_DBUserID,GETDATE(),@PV_BenefitValue)													

			END--end benefit
		CONT_BOA:
	FETCH NEXT FROM Cur_BOA INTO @PV_BOAID,@PV_BOAEmployeeID,@PV_IsTemporaryAssign
	END								
	CLOSE Cur_BOA
	DEALLOCATE Cur_BOA
	

	SELECT * FROM VIEW_AttendanceDaily WHERE AttendanceID=@Param_AttendanceID

COMMIT TRAN






GO
