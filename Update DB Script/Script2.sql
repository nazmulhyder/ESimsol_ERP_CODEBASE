IF NOT EXISTS (SELECT * FROM sys.columns where Name = N'IsHalfDayOff' and Object_ID = Object_ID(N'HRM_Shift'))
BEGIN
   ALTER TABLE HRM_Shift
   ADD IsHalfDayOff bit
END
GO
IF NOT EXISTS (SELECT * FROM sys.columns where Name = N'PStart' and Object_ID = Object_ID(N'HRM_Shift'))
BEGIN
   ALTER TABLE HRM_Shift
   ADD PStart datetime
END
GO
IF NOT EXISTS (SELECT * FROM sys.columns where Name = N'PEnd' and Object_ID = Object_ID(N'HRM_Shift'))
BEGIN
   ALTER TABLE HRM_Shift
   ADD PEnd datetime
END
GO
/****** Object:  StoredProcedure [dbo].[SP_Process_AttendanceDaily_V1]    Script Date: 10/23/2018 11:07:13 AM ******/
DROP PROCEDURE [dbo].[SP_Process_AttendanceDaily_V1]
GO
/****** Object:  StoredProcedure [dbo].[SP_Process_AttendanceDaily_EmployeeWise]    Script Date: 10/23/2018 11:07:13 AM ******/
DROP PROCEDURE [dbo].[SP_Process_AttendanceDaily_EmployeeWise]
GO
/****** Object:  StoredProcedure [dbo].[SP_IUD_AttendanceProcessManagement]    Script Date: 10/23/2018 11:07:13 AM ******/
DROP PROCEDURE [dbo].[SP_IUD_AttendanceProcessManagement]
GO
/****** Object:  StoredProcedure [dbo].[SP_IUD_AttendanceProcessManagement]    Script Date: 10/23/2018 11:07:13 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


-- =============================================
-- Author:		MD. Azharul Islam
-- Create date: 15 Jan 2014
-- Note:	Insert Update Delete AttendanceProcessManagement (Daily)
-- =============================================
CREATE PROCEDURE [dbo].[SP_IUD_AttendanceProcessManagement]
(
--DECLARE

	@Param_APMID as int,
	@Param_CompanyID as int,
	@Param_LocationID as int,
	@Param_DepartmentID as int, 
	@Param_ProcessType as smallint,
	@Param_ShiftID as int,
	@Param_Status as smallint,
	@Param_AttendanceDate as date,
	@Param_DBUserID as int,
	@Param_BusinessUnitID as int,
	@Param_DBOperation as smallint
	--%n,%n,%n,%n,%n,%n,%n,%d,%n,%n
--SET	@Param_APMID=0
--SET	@Param_CompanyID=1
--SET	@Param_LocationID=2
--SET	@Param_DepartmentID=2
--SET	@Param_ProcessType=1
--SET	@Param_ShiftID=1
--SET	@Param_Status=1
--SET	@Param_AttendanceDate='12 Feb 2014'
--SET	@Param_DBUserID=8
--SET	@Param_DBOperation=1
   
)

AS
BEGIN TRAN	
DECLARE
	@PV_DBServerDateTime as datetime
	,@PV_PreviousStatus as smallint
	,@PV_AttendanceDate as Date
	SET @PV_DBServerDateTime=Getdate()
	
	IF EXISTS (SELECT * FROM PayrollProcessManagement WHERE LocationID=@Param_LocationID AND @Param_AttendanceDate BETWEEN SalaryFrom AND SalaryTo)
	BEGIN
		ROLLBACK
			RAISERROR (N'Salary Already Processed By this date for this location.!!',16,1);	
		RETURN			
	END

	IF (@Param_APMID>0)
	BEGIN
		SET @PV_PreviousStatus=(SELECT [Status] FROM AttendanceProcessManagement WHERE APMID=@Param_APMID)	
		SET @Param_ProcessType=(SELECT ProcessType FROM AttendanceProcessManagement WHERE APMID=@Param_APMID)	
	END
--Process	
IF(@Param_Status=1/*Process*/ AND @Param_ProcessType=1/*Daily*/)
BEGIN
	
	IF  EXISTS (SELECT * FROM AttendanceProcessManagement WHERE CompanyID=@Param_CompanyID
	            AND LocationID=@Param_LocationID 
	            --AND DepartmentID=@Param_DepartmentID
	            AND ShiftID=@Param_ShiftID
	            AND ProcessType=@Param_ProcessType 
	            AND AttendanceDate=@Param_AttendanceDate)
	BEGIN
		--Get ApmID
		SET @Param_APMID=(SELECT top(1)APMID FROM AttendanceProcessManagement WHERE CompanyID=@Param_CompanyID
	            AND LocationID=@Param_LocationID --AND DepartmentID=@Param_DepartmentID
	            AND ShiftID=@Param_ShiftID AND ProcessType=@Param_ProcessType 
	            AND AttendanceDate=@Param_AttendanceDate)
		IF EXISTS (SELECT * FROM AttendanceDaily WHERE APMID=@Param_APMID)
		BEGIN
			ROLLBACK
				RAISERROR (N'Already Processed.!!',16,1);	
			RETURN
		END
		ELSE
		BEGIN
			DELETE FROM AttendanceProcessManagement WHERE APMID=@Param_APMID
			DELETE FROM AttendanceProcessManagementLog WHERE APMID=@Param_APMID
		END 	
	END
	IF (SELECT COUNT(*) FROM AttendanceProcessManagement WHERE BusinessUnitID=@Param_BusinessUnitID AND LocationID=@Param_LocationID AND DepartmentID=@Param_DepartmentID AND ShiftID=@Param_ShiftID AND ProcessType=@Param_ProcessType)>0
	BEGIN
		IF  NOT EXISTS (SELECT * FROM AttendanceProcessManagement WHERE
					CompanyID=@Param_CompanyID
					AND BusinessUnitID=@Param_BusinessUnitID 
					AND LocationID=@Param_LocationID 
					--AND DepartmentID=@Param_DepartmentID
					AND ShiftID=@Param_ShiftID
					AND ProcessType=@Param_ProcessType 
					AND AttendanceDate=DATEADD(DAY,-1,@Param_AttendanceDate))
		BEGIN
			ROLLBACK
				DECLARE @Msg as varchar(100)
				SET @Msg=Convert(varchar(100),DATEADD(DAY,-1,@Param_AttendanceDate))
				RAISERROR (N'Please Process for %s first and try for your desired date.!!',16,1,@Msg);	
			RETURN
		END
	END	
	
	SET @Param_APMID=(SELECT ISNULL(MAX(APMID),0)+1 FROM AttendanceProcessManagement)
	INSERT INTO AttendanceProcessManagement(APMID,       CompanyID,       LocationID,       DepartmentID,       ProcessType ,      ShiftID,       [Status] ,   AttendanceDate,        DBUserID,       DBServerDateTime,BusinessUnitID )
				                     VALUES(@Param_APMID,@Param_CompanyID,@Param_LocationID,@Param_DepartmentID,@Param_ProcessType,@Param_ShiftID,@Param_Status,@Param_AttendanceDate, @Param_DBUserID,@PV_DBServerDateTime,@Param_BusinessUnitID)    					    				  
	--Insert Log				                     
	INSERT INTO [AttendanceProcessManagementLog]
			   ([APMID],[PreviousStatus],[CurrenctStatus],[DSUserID],[DBServerDateTime])
		 VALUES(@Param_APMID,0,@Param_Status,@Param_DBUserID,GETDATE())					                     
	SELECT * FROM View_AttendanceProcessManagement WHERE APMID= @Param_APMID
END

--Rollback 
IF(@Param_Status=2/*Rollback*/ AND @Param_ProcessType=1/*Daily*/)
BEGIN
	IF(@Param_APMID<=0)
	BEGIN
		ROLLBACK
			RAISERROR (N'Your Selected AttendanceProcessManagement Is Invalid Please Refresh and try again!!',16,1);	
		RETURN
	END	
	
	IF EXISTS (SELECT * FROM AttendanceProcessManagement WHERE APMID=@Param_APMID AND [Status]=4/*Freeze*/)
	BEGIN
		ROLLBACK
			RAISERROR (N'This Process already locked(Freeze). You can not Rollback.!!',16,1);	
		RETURN
	END	
	IF EXISTS (SELECT * FROM AttendanceProcessManagement WHERE APMID=@Param_APMID AND [Status]=2/*Rollback*/)
	BEGIN
		ROLLBACK
			RAISERROR (N'This Process already in Rollback..!!',16,1);	
		RETURN
	END			
	--Check Attendance Locck
	IF EXISTS (SELECT * FROM AttendanceDaily WHERE APMID=@Param_APMID AND IsLock=1)
	BEGIN
		ROLLBACK
			RAISERROR (N'Attendance locked manually. You can not rollback!!',16,1);	
		RETURN
	END
	
	SET @PV_AttendanceDate=(SELECT AttendanceDate FROM AttendanceProcessManagement WHERE APMID=@Param_APMID)
	--Update Leave benifit
	--UPDATE EmployeeLeaveLedger SET TotalDay=TotalDay-(SELECT LeaveAmount FROM BenefitOnAttendance WHERE BOAID=@PV_BOAID AND LeaveHeadID>0 AND LeaveAmount>0)
	--WHERE ACSID IN (SELECT ACSID FROM AttendanceCalendarSession WHERE @PV_AttendanceDate BETWEEN StartDate AND EndDate)
	--AND LeaveID=(SELECT LeaveHeadID FROM BenefitOnAttendance WHERE BOAID IN (
	--			SELECT BOAID FROM BenefitOnAttendanceEmployee WHERE LeaveHeadID>0))
	--AND EmployeeID IN (SELECT EmployeeID FROM View_BenefitOnAttendanceEmployeeLedger WHERE AttendanceDate=@PV_AttendanceDate) 

	--DELETE Benefit On Attendance 
	--DELETE FROM BenefitOnAttendanceEmployeeLedger WHERE AttendanceDate=@PV_AttendanceDate
	--AND BOAEmployeeID IN (SELECT BOAEmployeeID FROM BenefitOnAttendanceEmployee WHERE EmployeeID IN(
	--SELECT EmployeeID FROM Employee WHERE IsActive=1))

	--Delete All Attendance Record	
	DELETE FROM AttendanceDaily WHERE APMID=@Param_APMID AND IsManual=0 AND ISELProcess=0 AND EmployeeID IN (
	SELECT EmployeeID FROM Employee WHERE IsActive=1);

	--Update APM
	UPDATE AttendanceProcessManagement SET [Status]=@Param_Status	WHERE APMID= @Param_APMID
	--Insert Log
	INSERT INTO [AttendanceProcessManagementLog]
			   ([APMID],[PreviousStatus],[CurrenctStatus],[DSUserID],[DBServerDateTime])
		 VALUES(@Param_APMID,@PV_PreviousStatus,@Param_Status,@Param_DBUserID,GETDATE())	
	SELECT * FROM View_AttendanceProcessManagement WHERE APMID= @Param_APMID
END

--Re-Process
IF(@Param_Status=3/*Re-Process*/ AND @Param_ProcessType=1/*Daily*/)
BEGIN
	IF(@Param_APMID<=0)
	BEGIN
		ROLLBACK
			RAISERROR (N'Your Selected Employee Is Invalid Please Refresh and try again!!',16,1);	
		RETURN
	END	
	
	IF (@PV_PreviousStatus!=2/*Rollback*/)
	BEGIN
		ROLLBACK
			RAISERROR (N'Status must be in "Rollback" to re-process.!!',16,1);	
		RETURN	
	END		
	--Update APM
	UPDATE AttendanceProcessManagement SET [Status]=@Param_Status	WHERE APMID= @Param_APMID
	--Insert Log
	INSERT INTO [AttendanceProcessManagementLog]
			   ([APMID],[PreviousStatus],[CurrenctStatus],[DSUserID],[DBServerDateTime])
		 VALUES(@Param_APMID,@PV_PreviousStatus,@Param_Status,@Param_DBUserID,GETDATE())	
		 
	SELECT * FROM View_AttendanceProcessManagement WHERE APMID= @Param_APMID	
END

--Freeze
IF(@Param_Status=4/*Freeze*/ AND @Param_ProcessType=1/*Daily*/)
BEGIN
	IF(@Param_APMID<=0)
	BEGIN
		ROLLBACK
			RAISERROR (N'Your Selected Employee Is Invalid Please Refresh and try again!!',16,1);	
		RETURN
	END	
	

	IF (@PV_PreviousStatus IN (2,4))--Rollback, Freeze
	BEGIN
		ROLLBACK
			RAISERROR (N'Status must be in "Processed" or "Re-Processed" to Freeze.!!',16,1);	
		RETURN	
	END
	--Update BOA
	UPDATE BenefitOnAttendanceEmployeeLedger SET IsBenefited=1 WHERE AttendanceDate  IN (
	SELECT AttendanceDate FROM AttendanceDaily WHERE APMID=@Param_APMID)

	--Lock Attendance Daily
	UPDATE AttendanceDaily SET IsLock=1 WHERE APMID=@Param_APMID		
	--Update APM
	UPDATE AttendanceProcessManagement SET [Status]=@Param_Status	WHERE APMID= @Param_APMID
	--Insert Log
	INSERT INTO [AttendanceProcessManagementLog]
			   ([APMID],[PreviousStatus],[CurrenctStatus],[DSUserID],[DBServerDateTime])
		 VALUES(@Param_APMID,@PV_PreviousStatus,@Param_Status,@Param_DBUserID,GETDATE())	
	----Insert Punchlog History
	--INSERT INTO PunchLogHIstory
	--SELECT MachineSLNo,EmployeeID,LocationID,DepartmentID,DesignationID,CardNo,PunchDateTime
	--FROM PunchLog WHERE CONVERT(DATE,PunchDateTime)=@Param_AttendanceDate
	--AND LocationID=@Param_LocationID --AND DepartmentID=@Param_DepartmentID
	----DElete Punch Log
	--DELETE FROM PunchLog WHERE CONVERT(DATE,PunchDateTime)=@Param_AttendanceDate		 
	--AND LocationID=@Param_LocationID --AND DepartmentID=@Param_DepartmentID
	--SELECT * FROM View_AttendanceProcessManagement WHERE APMID= @Param_APMID	
END

--Un Freeze
IF(@Param_Status=5/*UnFreeze*/ AND @Param_ProcessType=1/*Daily*/)
BEGIN
	IF(@Param_APMID<=0)
	BEGIN
		ROLLBACK
			RAISERROR (N'Your Selected Employee Is Invalid Please Refresh and try again!!',16,1);	
		RETURN
	END	

	IF (@PV_PreviousStatus !=4)--Rollback, Freeze
	BEGIN
		ROLLBACK
			RAISERROR (N'Status must be in Freezed.!!',16,1);	
		RETURN	
	END

	IF EXISTS (SELECT * FROM EmployeeSalary WHERE @Param_AttendanceDate BETWEEN EmployeeSalary.StartDate AND EmployeeSalary.EndDate)
	BEGIN
		ROLLBACK
			RAISERROR (N'Salary has Processed. Unfreeze is not possible!!',16,1);	
		RETURN	
	END

	--Update BOA
	UPDATE BenefitOnAttendanceEmployeeLedger SET IsBenefited=0 WHERE AttendanceDate  IN (
	SELECT AttendanceDate FROM AttendanceDaily WHERE APMID=@Param_APMID)

	--Lock Attendance Daily
	UPDATE AttendanceDaily SET IsLock=0 WHERE APMID=@Param_APMID
			
	DECLARE
	@PV_NewStatus INT

	--Update APM
	IF (@Param_APMID>0)
	BEGIN
		SET @PV_NewStatus=1
	END
	UPDATE AttendanceProcessManagement SET [Status]=@PV_NewStatus	WHERE APMID= @Param_APMID
	--Insert Log
	INSERT INTO [AttendanceProcessManagementLog]
			   ([APMID],[PreviousStatus],[CurrenctStatus],[DSUserID],[DBServerDateTime])
		 VALUES(@Param_APMID,@PV_PreviousStatus,@PV_NewStatus,@Param_DBUserID,GETDATE())	
END

COMMIT TRAN






GO
/****** Object:  StoredProcedure [dbo].[SP_Process_AttendanceDaily_EmployeeWise]    Script Date: 10/23/2018 11:07:13 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[SP_Process_AttendanceDaily_EmployeeWise]-- not for posmi
(
--DECLARE
	    @Param_EmployeeID as int,
		@Param_StartDate as date,
		@Param_EndDate as date,
		@Param_DBUserID as int
)
AS
BEGIN TRAN
--SET @Param_EmployeeID =692
--SET @Param_StartDate ='07 MAR 2016'
--SET @Param_EndDate ='07 MAR 2016'
--set @Param_DBUserID=-9


-- =============================================
-- Author:		MD. Azharul Islam
-- Create date: 17 Jan 2015
-- Description:	employee wise Attendance Process
-- =============================================
--Description: The purpose of this SP is to process Attendance 
--			 from one Employee in a date range

-- at first we declared the required private  variables

--*Step 01. in this step mandatory fields are checked
--then values for mandatory private varibles are selected

--*Step 02. get Attendance Scheme info

--*Step 03. check attendance process is done in every day

--*Step 04. check holidy. get leave,dayoff, intime,outtime etc. then insert or upadate according to attendance exist or not					

--Salary Validation
IF EXISTS (SELECT * FROM EmployeeSalary WHERE EmployeeID=@Param_EmployeeID AND ((@Param_StartDate BETWEEN StartDate AND EndDate) OR (@Param_EndDate BETWEEN StartDate AND EndDate)) )
BEGIN
	ROLLBACK
		RAISERROR (N'Salary already Processed by this date!!',16,1);
	RETURN	
END

--Variable Declaration
DECLARE @PV_StartTime as DATETIME--time(0)
		,@PV_EndTime as DATETIME--time(0)
		,@PV_AttendanceSchemeID as int
		,@PV_AttendanceCalendarID as int
		,@PV_RosterPlanID as int
		,@PV_DesignationID as int
		,@PV_LateInMunit as int
		,@PV_EarlyInMunit as int		
		,@PV_TotalWorkingHourInMin as int
		,@PV_IsDayOff as bit
		,@PV_IsLeave as bit
		,@PV_IsCompLeave as bit
		,@PV_LeaveIsUnPaid as bit
		--,@PV_Index AS INT
	
		,@PV_AttendanceDate DATE
		,@PV_LocationID as int
		,@PV_DepartmentID as int
		,@PV_ShiftID as int
		,@PV_APMID as int
		,@PV_StartDate as date

		,@PV_EarlyStart as DATETIME
		,@PV_DelayStart as DATETIME
		,@PV_ActualStart as DATETIME
		,@PV_EarlyEnd as Datetime
		,@PV_DelayEnd as Datetime
		,@PV_ActualEnd as Datetime

		,@PV_TolaranceTime as Datetime--int
		,@PV_DayEndTime as DATETIME--time(0)
		,@PV_DayStartTime as DATETIME--time(0)
		,@PV_Shift_IsOT as bit
		,@PV_Shift_OTStart as datetime
		,@PV_Shift_OTEnd   as datetime

		,@PV_Shift_IsLeaveOnOFFHoliday as bit
		,@PV_Shift_OTCalclateAfterInMin as int

		,@PV_IsHoliday as bit
		,@PV_LeaveHeadID as int
		,@PV_CompLeaveHeadID as int
		,@PV_AttendanceID as int

		,@PV_DateOfJoin AS Date
		,@PV_Remark as varchar (50)
		
		--ASAD
		,@PV_IsHalfDayoff AS BIT
		,@PV_PStart AS DATETIME
		,@PV_PEnd AS DATETIME

		--For Compliance
		,@PV_CompInTime as datetime
		,@PV_CompOutTime as DateTime
		,@PV_CompIsDayOff as bit
		,@PV_CompIsHoliday as bit
		,@PV_CompOverTimeInMin as int
		,@PV_CompMaxCompOTInMin as int
		,@PV_CompTotalWorkingHourInMin as int
		,@PV_IsLock as bit
		,@PV_MaxOTInMinATRoster as int
		,@PV_TimeKeeperOutTime as datetime
		,@PV_LastWorkingDay as Date
		,@PV_BaseAddress AS varchar(100)
		,@PV_BusinessUnitID as int
		,@PV_IsGWD as bit ---General Working Day
		,@PV_IsCompGWD as bit
		,@PV_IsOSD as bit
		,@PV_IsRostered as bit
		,@PV_CompMaxEndTime DATETIME
		,@PV_CompSlabOTInMin as int
		,@PV_IsCompOTFromSlab as bit
		,@PV_IsOTFromSlab as bit
		,@PV_EmpOfficialShift as int 
		,@PV_ToleranceEarly as int
		--For Contractual
		,@PV_ContractEndDate AS DATE
		SET @PV_BaseAddress=(SELECT top(1)BaseAddress FROM Company)

		DECLARE @tbl_BreakTime AS TABLE (BStart DateTime, BEnd DAtetime, BDurationMin int)
	
	--STEP1 
	IF (@Param_EmployeeID<=0 OR @Param_StartDate='' OR @Param_StartDate=null OR @Param_EndDate='' OR @Param_EndDate=null)
	BEGIN
		ROLLBACK
			RAISERROR (N'Some of the mandatory field missing!!',16,1);
		RETURN	
	END
	SET @PV_LocationID=(SELECT LocationID FROM DepartmentRequirementPolicy WITH (NOLOCK) WHERE DepartmentRequirementPolicyID IN(SELECT DRPID FROM EmployeeOfficial WITH (NOLOCK) WHERE EmployeeID=@Param_EmployeeID))
	SET @PV_BusinessUnitID=(SELECT BusinessUnitID FROM DepartmentRequirementPolicy WITH (NOLOCK) WHERE DepartmentRequirementPolicyID IN(SELECT DRPID FROM EmployeeOfficial WITH (NOLOCK) WHERE EmployeeID=@Param_EmployeeID))
	SET @PV_DepartmentID=(SELECT DepartmentID FROM DepartmentRequirementPolicy WITH (NOLOCK) WHERE DepartmentRequirementPolicyID IN(SELECT DRPID FROM EmployeeOfficial WITH (NOLOCK) WHERE EmployeeID=@Param_EmployeeID))
	SET  @PV_DesignationID = (SELECT DesignationID  FROM EmployeeOfficial WITH (NOLOCK) WHERE EmployeeID=@Param_EmployeeID)
	SET @PV_ShiftID=(SELECT CurrentShiftID  FROM EmployeeOfficial WITH (NOLOCK) WHERE EmployeeID=@Param_EmployeeID)
	SET @PV_EmpOfficialShift=@PV_ShiftID
	SET @PV_AttendanceSchemeID=(SELECT AttendanceSchemeID FROM EmployeeOfficial WITH (NOLOCK) WHERE EmployeeID=@Param_EmployeeID)
	SELECT @PV_AttendanceCalendarID=AttendanceCalenderID,@PV_RosterPlanID=RosterPlanID
	FROM View_AttendanceScheme WITH (NOLOCK) WHERE AttendanceSchemeID=@PV_AttendanceSchemeID
	IF (@PV_AttendanceCalendarID is null)
	BEGIN
		ROLLBACK 
			RAISERROR(N'Invalid Attendance Scheme.!!',16,1)
		RETURN
	END

	--Get Date Of Join And set Start Date
	SET @PV_DateOfJoin=(SELECT DateOfJoin FROM EmployeeOfficial WHERE EmployeeID=@Param_EmployeeID)
	IF (@Param_StartDate<@PV_DateOfJoin)
	BEGIN
		SET @Param_StartDate=@PV_DateOfJoin
		DELETE FROM AttendanceDaily WHERE EmployeeID=@Param_EmployeeID AND AttendanceDate<@PV_DateOfJoin
	END

	/*This code is omited because of employee settlement module is ready*/
	--IF EXISTS (SELECT * FROM EmployeeOfficial WHERE EmployeeID=@Param_EmployeeID AND IsActive=0)
	--BEGIN
	--	SELECT top(1)@Param_EndDate=DBServerDateTime FROM EmployeeWorkingStatusHistory WHERE EmployeeID=@Param_EmployeeID AND CurrentStatus=6
	--	Order By EmployeeWSHID DESC
	--	DELETE FROM AttendanceDaily WHERE EmployeeID=@Param_EmployeeID AND AttendanceDate>=@Param_EndDate
	--END

	--Get End Date For Contractual Employee
	IF EXISTS (SELECT * FROm EmployeeConfirmation WITH (NOLOCK) WHERE EmployeeID=@Param_EmployeeID AND EmployeeCategory IN (3,4))--Contractual & Seasonal
	BEGIN
		SET @PV_ContractEndDate=(SELECT DateOfConfirmation FROM EmployeeOfficial WITH (NOLOCK) WHERE EmployeeID=@Param_EmployeeID)			
		IF @Param_EndDate>@PV_ContractEndDate
		BEGIN
			SET @Param_EndDate=@PV_ContractEndDate
		END
	END
	

	/*
		Employee Settlement Issue
		=========================
		If any employee has Approved but continued(IsResigned=0) settlement record by this or before date,
		Employee must be discontinued and must be deleted Attendance & BOA record after last working day.
	*/
	IF EXISTS (SELECT * FROM EmployeeSettlement WITH (NOLOCK) WHERE EmployeeID=@Param_EmployeeID AND ApproveBy>0 AND EffectDate>=GETDATE())
	BEGIN
		--Get LAst working day
		SET @PV_LastWorkingDay=(SELECT EffectDate FROM EmployeeSettlement WHERE EmployeeID=@Param_EmployeeID)
		--Delete Attendance record
		DELETE FROM AttendanceDaily WHERE EmployeeID=@Param_EmployeeID AND AttendanceDate>@PV_LastWorkingDay		
		DELETE FROM BenefitOnAttendanceEmployeeLedger WHERE BOAEmployeeID IN (
		SELECT BOAEmployeeID FROM BenefitOnAttendanceEmployee WITH (NOLOCK) WHERE EmployeeID=@Param_EmployeeID) 
		AND AttendanceDate>@PV_LastWorkingDay
		
		--Discontinue
		UPDATE Employee SET  IsActive = 0 WHERE EmployeeID= @Param_EmployeeID
		UPDATE EmployeeOfficial SET  IsActive = 0, WorkingStatus=6 WHERE EmployeeID= @Param_EmployeeID
		UPDATE EmployeeAuthentication SET  IsActive = 0 WHERE EmployeeID= @Param_EmployeeID
		UPDATE EmployeeSalaryStructure SET  IsActive = 0 WHERE EmployeeID= @Param_EmployeeID
		UPDATE PFmember SET  IsActive = 0 WHERE EmployeeID= @Param_EmployeeID
		
		UPDATE BenefitOnAttendanceEmployee SET  InactiveBy = @Param_DBUserID,InactiveDate=@PV_LastWorkingDay 
		WHERE EmployeeID= @Param_EmployeeID AND (InactiveBy<=0 OR InactiveBy IS NULL)

		UPDATE EmployeeSettlement SET  IsResigned = 1 WHERE EmployeeID= @Param_EmployeeID	
		
		SET @Param_EndDate=@PV_LastWorkingDay
	END

SET @PV_StartDate=@Param_StartDate
WHILE(@Param_StartDate<=@Param_EndDate)
BEGIN
	SET @PV_AttendanceDate=@Param_StartDate
	IF EXISTS (SELECT * FROM AttendanceDaily WITH (NOLOCK) WHERE EmployeeID=@Param_EmployeeID AND AttendanceDate=@PV_AttendanceDate AND (IsManual=1 OR ISELProcess=1))
	BEGIN
		GOTO CONT;
	END

	--Check General Working Day
	SET @PV_IsGWD=0
	IF EXISTS (SELECT * FROM GeneralWorkingDay WITH (NOLOCK) WHERE AttendanceDate=@PV_AttendanceDate AND GWDID IN (
				SELECT GWDID FROM GeneralWorkingDayDetail WHERE DRPID IN (
				SELECT DepartmentRequirementPolicyID FROM DepartmentRequirementPolicy WITH (NOLOCK) WHERE BusinessUnitID=@PV_BusinessUnitID 
				AND LocationID=@PV_LocationID AND DepartmentID=@PV_DepartmentID)))
	BEGIN
		SET @PV_IsGWD=1
		SET @PV_IsCompGWD=ISNULL((SELECT IsCompApplicable FROM GeneralWorkingDay WITH (NOLOCK) WHERE AttendanceDate=@PV_AttendanceDate),1)--if Null then comp & actual applicable
	END

	IF @PV_IsGWD=0
	BEGIN
		IF EXISTS (SELECT * FROM GeneralWorkingDay WITH (NOLOCK) WHERE AttendanceDate=@PV_AttendanceDate)
		BEGIN
			SET @PV_IsGWD=1
			SET @PV_IsCompGWD=ISNULL((SELECT IsCompApplicable FROM GeneralWorkingDay WITH (NOLOCK) WHERE AttendanceDate=@PV_AttendanceDate),1)--if Null then comp & actual applicable
		END
	END

	--Initilize
	SET @PV_APMID=0
	SET @PV_IsDayOff=0
	SET @PV_LeaveIsUnPaid=0
	SET @PV_IsLeave=0
	SET @PV_IsCompLeave=0;
	SET @PV_LateInMunit =0
	SET @PV_EarlyInMunit =0
	SET @PV_TotalWorkingHourInMin =0
	SET @PV_IsDayOff =0
	SET @PV_IsHoliday =0
	SET @PV_IsLeave =0
	SET @PV_LeaveIsUnPaid =0
	SET @PV_LeaveHeadID=0
	SET @PV_CompLeaveHeadID=0	
	SET @PV_CompIsDayOff=0
	SET @PV_CompIsHoliday=0
	SET @PV_CompOverTimeInMin=0
	SET @PV_CompTotalWorkingHourInMin=0
	SET @PV_MaxOTInMinATRoster=0
	SET @PV_IsOSD=0
	SET @PV_Remark=NULL
	SET @PV_IsRostered=0
	SET @PV_CompSlabOTInMin=0
	SET @PV_CompMaxEndTime=NULL
	SET @PV_IsCompOTFromSlab=0
	SET @PV_IsOTFromSlab=0
	SET @PV_ToleranceEarly=0

	
	DECLARE @tbl_Date TABLE (attDate DATE)

	--Check Attendance & and its existance
	IF EXISTS (SELECT * FROM AttendanceDaily WITH (NOLOCK) WHERE EmployeeID=@Param_EmployeeID AND AttendanceDate=@PV_AttendanceDate)
	BEGIN--chk att existance
		SELECT @PV_AttendanceID=AttendanceID,@PV_APMID=APMID,@PV_IsLock=IsLock
		FROM AttendanceDaily WITH (NOLOCK) WHERE EmployeeID=@Param_EmployeeID AND AttendanceDate=@PV_AttendanceDate		
		DELETE FROM AttendanceDaily WHERE AttendanceID=@PV_AttendanceID
	END----chk att existance
	ELSE
	BEGIN
		SELECT @PV_APMID=APMID FROM AttendanceProcessManagement WITH (NOLOCK) WHERE LocationID=@PV_LocationID AND ShiftID=@PV_EmpOfficialShift AND AttendanceDate=@PV_AttendanceDate AND BusinessUnitID=@PV_BusinessUnitID

		IF EXISTS (SELECT * FROM AttendanceProcessManagement WITH (NOLOCK) WHERE APMID=@PV_APMID AND [Status]=4)
		BEGIN
			SET @PV_IsLock=1
		END ELSE BEGIN
			SET @PV_IsLock=0
		END
	END

	IF(@PV_APMID<=0)
	BEGIN
		ROLLBACK
			DECLARE
			@AttendanceDate AS VARCHAR(100)
			SET @AttendanceDate =CONVERT(VARCHAR,@PV_AttendanceDate,105)
			RAISERROR (N'Attendance is not processed in %s!!',16,1,@AttendanceDate);
		RETURN
	END

	--check Shift rostering
	IF EXISTS (SELECT * FROM RosterPlanEmployee WITH (NOLOCK) WHERE EmployeeID=@Param_EmployeeID AND AttendanceDate=@PV_AttendanceDate)
	BEGIN
		SET @PV_IsRostered=1
		SET @PV_ShiftID=0

		SELECT top(1)@PV_ShiftID=ShiftID
		,@PV_IsDayOff=ISNULL(IsDayOff,0)
		,@PV_IsHoliday=ISNULL(IsHoliday,0)
		,@PV_MaxOTInMinATRoster=ISNULL(MaxOTInMin,0)
		,@PV_Remark=Remarks 
		,@PV_StartTime=(DATEADD(SECOND,0,DATEADD (MINUTE,DATEPART(MINUTE,InTime), DATEADD(HOUR, DATEPART(HOUR,InTime), DATEADD(dd, DATEDIFF(dd, -0, @PV_AttendanceDate), 0)))) )--CAST (StartTime AS TIME(0))
		,@PV_EndTime=(DATEADD(SECOND,0,DATEADD (MINUTE,DATEPART(MINUTE,OutTime), DATEADD(HOUR, DATEPART(HOUR,OutTime), DATEADD(dd, DATEDIFF(dd, -0, @PV_AttendanceDate), 0)))) )--CAST (EndTime AS TIME(0)) 	   
		FROM RosterPlanEmployee WITH (NOLOCK) WHERE EmployeeID=@Param_EmployeeID AND AttendanceDate=@PV_AttendanceDate		

		SELECT 		
		@PV_DayStartTime=(DATEADD(SECOND,0,DATEADD (MINUTE,DATEPART(MINUTE,DayStartTime), DATEADD(HOUR, DATEPART(HOUR,DayStartTime), DATEADD(dd, DATEDIFF(dd, -0, @PV_AttendanceDate), 0)))) )
		,@PV_DayEndTime=(DATEADD(SECOND,0,DATEADD (MINUTE,DATEPART(MINUTE,DayEndTime), DATEADD(HOUR, DATEPART(HOUR,DayEndTime), DATEADD(dd, DATEDIFF(dd, -0, @PV_AttendanceDate), 0)))) )
		,@PV_TolaranceTime=(DATEADD(SECOND,0,DATEADD (MINUTE,DATEPART(MINUTE,ToleranceTime), DATEADD(HOUR, DATEPART(HOUR,ToleranceTime), DATEADD(dd, DATEDIFF(dd, -0, @PV_AttendanceDate), 0)))) )
		,@PV_Shift_IsOT=IsOT
		,@PV_Shift_OTStart=(DATEADD(SECOND,0,DATEADD (MINUTE,DATEPART(MINUTE,OTStartTime), DATEADD(HOUR, DATEPART(HOUR,OTStartTime), DATEADD(dd, DATEDIFF(dd, -0, @PV_AttendanceDate), 0)))) )
		,@PV_Shift_OTEnd=(DATEADD(SECOND,0,DATEADD (MINUTE,DATEPART(MINUTE,OTEndTime), DATEADD(HOUR, DATEPART(HOUR,OTEndTime), DATEADD(dd, DATEDIFF(dd, -0, @PV_AttendanceDate), 0)))) )

		,@PV_CompMaxCompOTInMin=MaxOTComplianceInMin
		,@PV_Shift_OTCalclateAfterInMin=ISNULL(OTCalculateAfterInMinute,0)
		,@PV_Shift_IsLeaveOnOFFHoliday=IsLeaveOnOFFHoliday
		,@PV_CompMaxEndTime=(DATEADD(SECOND,0,DATEADD (MINUTE,DATEPART(MINUTE,CompMaxEndTime), DATEADD(HOUR, DATEPART(HOUR,CompMaxEndTime), DATEADD(dd, DATEDIFF(dd, -0, @PV_AttendanceDate), 0)))) )--CAST (EndTime AS TIME(0)) 	  
		,@PV_ToleranceEarly=ISNULL(ToleranceForEarlyInMin,0) 

		--ASAD
		,@PV_IsHalfDayoff = ISNULL(IsHalfDayOff, 0)
		,@PV_PStart = PStart
		,@PV_PEnd = PEnd
		FROM HRM_Shift WITH (NOLOCK) WHERE ShiftID=@PV_ShiftID

		IF @PV_StartTime IS NULL
		BEGIN
			SELECT @PV_StartTime=(DATEADD(SECOND,0,DATEADD (MINUTE,DATEPART(MINUTE,StartTime), DATEADD(HOUR, DATEPART(HOUR,StartTime), DATEADD(dd, DATEDIFF(dd, -0, @PV_AttendanceDate), 0)))) )--CAST (StartTime AS TIME(0))					
			FROM HRM_Shift WHERE ShiftID=@PV_ShiftID			
		END

		IF @PV_EndTime IS NULL
		BEGIN
			SELECT @PV_EndTime=(DATEADD(SECOND,0,DATEADD (MINUTE,DATEPART(MINUTE,EndTime), DATEADD(HOUR, DATEPART(HOUR,EndTime), DATEADD(dd, DATEDIFF(dd, -0, @PV_AttendanceDate), 0)))) )--CAST (EndTime AS TIME(0)) 	   
			FROM HRM_Shift WHERE ShiftID=@PV_ShiftID			
		END

		--For compliance (The following code has been blocked because dayoff and holiday will be occur as per scheme not current roster. roster for compliance will be made later,)
		--SET @PV_CompIsDayOff=@PV_IsDayOff
		--SET @PV_CompIsHoliday=@PV_IsHoliday

	END--check Shift rostering
	ELSE BEGIN 
		SET @PV_ShiftID=(SELECT CurrentShiftID FROM  EmployeeOfficial WHERE EmployeeID=@Param_EmployeeID)

		SELECT @PV_StartTime=(DATEADD(SECOND,0,DATEADD (MINUTE,DATEPART(MINUTE,StartTime), DATEADD(HOUR, DATEPART(HOUR,StartTime), DATEADD(dd, DATEDIFF(dd, -0, @PV_AttendanceDate), 0)))) )--CAST (StartTime AS TIME(0))
				,@PV_EndTime=(DATEADD(SECOND,0,DATEADD (MINUTE,DATEPART(MINUTE,EndTime), DATEADD(HOUR, DATEPART(HOUR,EndTime), DATEADD(dd, DATEDIFF(dd, -0, @PV_AttendanceDate), 0)))) )--CAST (EndTime AS TIME(0)) 	
				,@PV_CompMaxEndTime=(DATEADD(SECOND,0,DATEADD (MINUTE,DATEPART(MINUTE,CompMaxEndTime), DATEADD(HOUR, DATEPART(HOUR,CompMaxEndTime), DATEADD(dd, DATEDIFF(dd, -0, @PV_AttendanceDate), 0)))) )--CAST (EndTime AS TIME(0)) 	   		   
				,@PV_DayStartTime=(DATEADD(SECOND,0,DATEADD (MINUTE,DATEPART(MINUTE,DayStartTime), DATEADD(HOUR, DATEPART(HOUR,DayStartTime), DATEADD(dd, DATEDIFF(dd, -0, @PV_AttendanceDate), 0)))) )
				,@PV_DayEndTime=(DATEADD(SECOND,0,DATEADD (MINUTE,DATEPART(MINUTE,DayEndTime), DATEADD(HOUR, DATEPART(HOUR,DayEndTime), DATEADD(dd, DATEDIFF(dd, -0, @PV_AttendanceDate), 0)))) )
				,@PV_TolaranceTime=(DATEADD(SECOND,0,DATEADD (MINUTE,DATEPART(MINUTE,ToleranceTime), DATEADD(HOUR, DATEPART(HOUR,ToleranceTime), DATEADD(dd, DATEDIFF(dd, -0, @PV_AttendanceDate), 0)))) )
				,@PV_Shift_IsOT=IsOT
				,@PV_Shift_OTStart=(DATEADD(SECOND,0,DATEADD (MINUTE,DATEPART(MINUTE,OTStartTime), DATEADD(HOUR, DATEPART(HOUR,OTStartTime), DATEADD(dd, DATEDIFF(dd, -0, @PV_AttendanceDate), 0)))) )
				,@PV_Shift_OTEnd=(DATEADD(SECOND,0,DATEADD (MINUTE,DATEPART(MINUTE,OTEndTime), DATEADD(HOUR, DATEPART(HOUR,OTEndTime), DATEADD(dd, DATEDIFF(dd, -0, @PV_AttendanceDate), 0)))) )

				,@PV_CompMaxCompOTInMin=MaxOTComplianceInMin
				,@PV_Shift_OTCalclateAfterInMin=ISNULL(OTCalculateAfterInMinute,0)
				,@PV_Shift_IsLeaveOnOFFHoliday=IsLeaveOnOFFHoliday
				
				--ASAD
				,@PV_IsHalfDayoff = ISNULL(IsHalfDayOff, 0)
				,@PV_PStart = PStart
				,@PV_PEnd = PEnd

		FROM HRM_Shift WHERE ShiftID=@PV_ShiftID
	END	
	

	--Break Times
	DELETE FROM @tbl_BreakTime

	INSERT @tbl_BreakTime
	SELECT BStart,BEnd,DATEDIFF(MINUTE,BStart,BEnd) FROM 
	(SELECT (DATEADD(SECOND,0,DATEADD (MINUTE,DATEPART(MINUTE,StartTime), DATEADD(HOUR, DATEPART(HOUR,StartTime), DATEADD(dd, DATEDIFF(dd, -0, CASE WHEN DATEPART(HOUR,@PV_DayStartTime)<DATEPART(HOUR,StartTime) THEN @PV_AttendanceDate ELSE DATEADD(DAY,1,@PV_AttendanceDate) END), 0)))) ) AS BStart 
	,(DATEADD(SECOND,0,DATEADD (MINUTE,DATEPART(MINUTE,EndTime), DATEADD(HOUR, DATEPART(HOUR,EndTime), DATEADD(dd, DATEDIFF(dd, -0, CASE WHEN DATEPART(HOUR,@PV_DayStartTime)<DATEPART(HOUR,EndTime) THEN @PV_AttendanceDate ELSE DATEADD(DAY,1,@PV_AttendanceDate) END), 0)))) ) AS BEnd
	FROM ShiftBreakSchedule WHERE ShiftID=@PV_ShiftID) ss

	IF (DATEPART(HOUR,@PV_DayStartTime)>DATEPART(HOUR,@PV_DayEndTime))
	BEGIN--Night Shift or different date
		SET @PV_DayEndTime=(DATEADD(SECOND,0,DATEADD (MINUTE,DATEPART(MINUTE,@PV_DayEndTime), DATEADD(HOUR, DATEPART(HOUR,@PV_DayEndTime), DATEADD(dd, DATEDIFF(dd, -1, @PV_AttendanceDate), 0)))) )
	END
	IF (DATEPART(HOUR,@PV_DayStartTime)=DATEPART(HOUR,@PV_DayEndTime) AND DATEPART(MINUTE,@PV_DayStartTime)>DATEPART(MINUTE,@PV_DayEndTime))
	BEGIN--Night Shift or different date
		SET @PV_DayEndTime=(DATEADD(SECOND,0,DATEADD (MINUTE,DATEPART(MINUTE,@PV_DayEndTime), DATEADD(HOUR, DATEPART(HOUR,@PV_DayEndTime), DATEADD(dd, DATEDIFF(dd, -1, @PV_AttendanceDate), 0)))) )
	END

	IF (DATEPART(HOUR,@PV_StartTime)>DATEPART(HOUR,@PV_EndTime))
	BEGIN--Night Shift or different date
		SET @PV_EndTime=(DATEADD(SECOND,0,DATEADD (MINUTE,DATEPART(MINUTE,@PV_EndTime), DATEADD(HOUR, DATEPART(HOUR,@PV_EndTime), DATEADD(dd, DATEDIFF(dd, -1, @PV_AttendanceDate), 0)))) )
		SET @PV_CompMaxEndTime=(DATEADD(SECOND,0,DATEADD (MINUTE,DATEPART(MINUTE,@PV_CompMaxEndTime), DATEADD(HOUR, DATEPART(HOUR,@PV_CompMaxEndTime), DATEADD(dd, DATEDIFF(dd, -1, @PV_AttendanceDate), 0)))) )
		--OT Start time : If OT start time > Shift End Time Then Shift End Date will Be OT StartDate
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

					
	--get Leave Information
	IF (EXISTS (SELECT * FROM LeaveApplication WHERE IsApprove=1/*ApproveBy>0*/ AND (CancelledBy=0 OR CancelledBy IS NULL) AND EmployeeID =@Param_EmployeeID	AND @PV_AttendanceDate BETWEEN CONVERT (DATE,StartDateTime) AND CONVERT(DATE,EndDateTime)))
	BEGIN 
		SET @PV_IsLeave=1;
		SET @PV_IsCompLeave=ISNULL((SELECT top(1)IsComp FROM EmployeeLeaveLedger WHERE EmpLeaveLedgerID IN (
							SELECT EmpLeaveLedgerID FROM LeaveApplication WHERE IsApprove=1/*ApproveBy>0*/ AND (CancelledBy=0 OR CancelledBy IS NULL) AND EmployeeID =@Param_EmployeeID	
							AND @PV_AttendanceDate BETWEEN CONVERT (DATE,StartDateTime) AND CONVERT(DATE,EndDateTime))),1)


		SET @PV_LeaveHeadID=(SELECT top(1)LeaveID FROM EmployeeLeaveLedger WHERE EmpLeaveLedgerID IN (
							SELECT EmpLeaveLedgerID FROM LeaveApplication WHERE IsApprove=1/*ApproveBy>0*/ AND (CancelledBy=0 OR CancelledBy IS NULL) AND EmployeeID =@Param_EmployeeID	
							AND @PV_AttendanceDate BETWEEN CONVERT (DATE,StartDateTime) AND CONVERT(DATE,EndDateTime)))

		IF(@PV_IsCompLeave=1)
		BEGIN
			SET @PV_CompLeaveHeadID = @PV_LeaveHeadID
		END ELSE BEGIN
			SET @PV_CompLeaveHeadID = 0
		END

		--SET @PV_LeaveIsUnPaid=(SELECT top(1)IsUnPaid FROM LeaveApplication WHERE ApproveBy>0 AND EmployeeID =@Param_EmployeeID	AND @PV_AttendanceDate BETWEEN CONVERT (DATE,StartDateTime,105) AND CONVERT(DATE,EndDateTime))
		IF EXISTS (SELECT * FROM LeaveHead WHERE IsLWP=1 AND LeaveHeadID IN (@PV_LeaveHeadID))
		BEGIN
			SET @PV_LeaveIsUnPaid=1
		END
	END
		
		
	-- check Holiday
	IF @PV_IsRostered=0--Not General working day & Non Rostered  @PV_IsGWD=0 AND 
	BEGIN
		IF EXISTS (SELECT * FROM AttendanceCalendarSessionHoliday WITH (NOLOCK) WHERE ACSID IN (
			SELECT ACSID FROM AttendanceCalendarSession WITH (NOLOCK) WHERE AttendanceCalendarID=@PV_AttendanceCalendarID)
			--AND HolidayID IN (SELECT HolidayID FROM AttendanceSchemeHoliday WHERE AttendanceSchemeID=@PV_AttendanceSchemeID) 
			AND @PV_AttendanceDate BETWEEN StartDate AND EndDate)
		BEGIN--Holiday
			SET @PV_IsHoliday=1	
			SET @PV_CompIsHoliday=1				
		END
	END ELSE BEGIN
		--For Compliance: No holiday effect from roster
		IF EXISTS (SELECT * FROM AttendanceCalendarSessionHoliday WITH (NOLOCK) WHERE ACSID IN (
			SELECT ACSID FROM AttendanceCalendarSession WITH (NOLOCK) WHERE AttendanceCalendarID=@PV_AttendanceCalendarID)
			--AND HolidayID IN (SELECT HolidayID FROM AttendanceSchemeHoliday WHERE AttendanceSchemeID=@PV_AttendanceSchemeID) 
			AND @PV_AttendanceDate BETWEEN StartDate AND EndDate)
		BEGIN--Holiday				
			SET @PV_CompIsHoliday=1				
		END
	END

	
	
	IF EXISTS(SELECT * FROM AttendanceSchemeDayOff WHERE AttendanceSchemeID=@PV_AttendanceSchemeID AND DayOffType=3 AND [WeekDay]=DATENAME(DW,@PV_AttendanceDate))
	BEGIN
		IF @PV_IsHalfDayoff=1
		BEGIN
			SET @PV_IsDayOff=0;
			SET @PV_CompIsDayOff=0;
			SET @PV_StartTime=(DATEADD(SECOND,0,DATEADD (MINUTE,DATEPART(MINUTE,@PV_PStart), DATEADD(HOUR, DATEPART(HOUR,@PV_PStart), DATEADD(dd, DATEDIFF(dd, -0, @PV_AttendanceDate), 0)))) )--CAST (StartTime AS TIME(0))
			SET @PV_EndTime=(DATEADD(SECOND,0,DATEADD (MINUTE,DATEPART(MINUTE,@PV_PEnd), DATEADD(HOUR, DATEPART(HOUR,@PV_PEnd), DATEADD(dd, DATEDIFF(dd, -0, @PV_AttendanceDate), 0)))) )--CAST (EndTime AS TIME(0)) 	   
		END
	END		
								
	--get Day Off information
	IF  @PV_IsRostered=0--Not General working day & Non Rostered @PV_IsGWD=0 AND
	BEGIN
		IF EXISTS (SELECT * FROM AttendanceSchemeDayOff WHERE AttendanceSchemeID=@PV_AttendanceSchemeID AND [WeekDay]=DATENAME(DW,@PV_AttendanceDate))
		BEGIN
			SET @PV_IsDayOff=1;
			SET @PV_CompIsDayOff=1;
		END
	END ELSE BEGIN
		IF EXISTS (SELECT * FROM AttendanceSchemeDayOff WHERE AttendanceSchemeID=@PV_AttendanceSchemeID AND [WeekDay]=DATENAME(DW,@PV_AttendanceDate))
		BEGIN
			SET @PV_CompIsDayOff=1;
		END
	END

	
	IF @PV_IsHoliday=1	BEGIN SET @PV_IsDayOff=0; END
	IF @PV_CompIsHoliday=1 BEGIN SET @PV_CompIsDayOff=0; END

	IF @PV_IsGWD=1
	BEGIN
		IF @PV_IsCompGWD=1
		BEGIN
			SET @PV_CompIsDayOff=0;
			SET @PV_CompIsHoliday=0
		END
		SET @PV_IsDayOff=0;				
		SET @PV_IsHoliday=0		
	END

	IF (@PV_IsLeave=1)
	BEGIN
		SET @PV_IsDayOff=0
		SET @PV_IsHoliday=0							
	END
	IF (@PV_IsCompLeave=1)
	BEGIN
		SET @PV_CompIsDayOff=0;
		SET @PV_CompIsHoliday=0;						
	END


	--Suffix & Prefix for leave 
	--IF @PV_Shift_IsLeaveOnOFFHoliday=1
	--BEGIN--Leave Between DayOff or holiday
	--	IF (@PV_IsLeave=1)
	--	BEGIN
	--		SET @PV_IsDayOff=0
	--		SET @PV_IsHoliday=0
	--		SET @PV_CompIsDayOff=0;
	--		SET @PV_CompIsHoliday=0;							
	--	END
	--END--Leave Between DayOff or holiday

	--
	SELECT @PV_ActualStart=MIN(PunchDateTime)--Attendance Start
			,@PV_ActualEnd=MAX(PunchDateTime) --Attendance End
	FROM PunchLog WHERE EmployeeID=@Param_EmployeeID					
	AND PunchDateTime BETWEEN @PV_DayStartTime AND @PV_DayEndTime 

	IF @PV_ActualStart IS NULL
	BEGIN
		SET @PV_ActualStart=(DATEADD(SECOND,0,DATEADD (MINUTE,0, DATEADD(HOUR, 0, DATEADD(dd, DATEDIFF(dd, -0, @PV_AttendanceDate), 0)))) )		
	END

	IF @PV_ActualEnd IS NULL
	BEGIN
		SET @PV_ActualEnd=(DATEADD(SECOND,0,DATEADD (MINUTE,0, DATEADD(HOUR, 0, DATEADD(dd, DATEDIFF(dd, -0, @PV_AttendanceDate), 0)))) )		
	END

	--in case of start & end time same
	IF  CONVERT (DATE,@PV_ActualStart)=CONVERT (DATE,@PV_ActualEnd)--Check Date
		AND DATEPART(HOUR,@PV_ActualStart)=DATEPART(HOUR,@PV_ActualEnd) --Hour check
		AND DATEPART(MINUTE,@PV_ActualStart)=DATEPART(MINUTE,@PV_ActualEnd)--Min Check
	BEGIN
		SET @PV_ActualEnd=(DATEADD(SECOND,0,DATEADD (MINUTE,0, DATEADD(HOUR, 0, DATEADD(dd, DATEDIFF(dd, -0, @PV_AttendanceDate), 0)))) )
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
	IF @PV_ActualStart>@PV_TolaranceTime AND @PV_IsLeave=0
	BEGIN
		SET @PV_LateInMunit=DATEDIFF(MINUTE,@PV_StartTime,@PV_ActualStart)
		SET @PV_LateInMunit=CASE WHEN @PV_LateInMunit<0 THEN 0 ELSE @PV_LateInMunit END
	END

	--find early
	IF CAST(@PV_ActualEnd AS TIME(0))!='00:00' AND @PV_ActualEnd<@PV_EndTime AND @PV_IsLeave=0
	BEGIN
		SET @PV_EarlyInMunit=DATEDIFF(MINUTE,@PV_ActualEnd,@PV_EndTime)		
	END
	ELSE
	BEGIN
		SET @PV_EarlyInMunit=0
	END				

	--find early
	IF CAST(@PV_ActualEnd AS TIME(0))='00:00' AND CAST(@PV_ActualStart AS TIME(0))!='00:00' AND @PV_IsLeave=0
	BEGIN
		SET @PV_EarlyInMunit=DATEDIFF(MINUTE,@PV_ActualStart,@PV_EndTime)
	END	

	SET @PV_EarlyInMunit= CASE WHEN @PV_EarlyInMunit<0 THEN 0 ELSE @PV_EarlyInMunit END
	SET @PV_EarlyInMunit= CASE WHEN @PV_EarlyInMunit>@PV_ToleranceEarly THEN @PV_EarlyInMunit ELSE 0 END

	IF @PV_EarlyInMunit>0
	BEGIN
		IF EXISTS (SELECT * FROM @tbl_BreakTime)
		BEGIN
			DECLARE @PV_BrkEnd AS DATETIME
			SET @PV_BrkEnd = (SELECT MAX(BEnd) FROM @tbl_BreakTime)
			IF @PV_EarlyInMunit>0 AND @PV_BrkEnd IS NOT NULL AND @PV_EndTime<@PV_BrkEnd
			BEGIN
				SET @PV_EarlyInMunit=@PV_EarlyInMunit-DATEDIFF(MINUTE,@PV_EndTime,@PV_BrkEnd)
				SET @PV_EarlyInMunit= CASE WHEN @PV_EarlyInMunit<0 THEN 0 ELSE @PV_EarlyInMunit END
			END
		END
	END

	IF (@PV_BaseAddress IN ('amg','A007','akcl','golden','mithela','union','b007','wangs')) AND (@PV_IsDayOff=1 OR @PV_IsHoliday=1 OR @PV_IsOSD=1 OR @PV_IsLeave=1)
	BEGIN
		SET @PV_LateInMunit=0
		SET @PV_EarlyInMunit=0
	END							 
	--find total working hour
	IF CAST(@PV_ActualStart AS TIME(0))!='00:00' AND CAST(@PV_ActualEnd AS TIME(0))!='00:00'
	BEGIN
		SET @PV_TotalWorkingHourInMin=DATEDIFF(MINUTE,@PV_ActualStart,@PV_ActualEnd) 										 
	END	
	ELSE
	BEGIN
		SET @PV_TotalWorkingHourInMin=0
	END								
	SET @PV_TotalWorkingHourInMin= CASE WHEN @PV_TotalWorkingHourInMin<0 THEN 0 ELSE @PV_TotalWorkingHourInMin END

	/****************Over time in minute ******************************/
		DECLARE @PV_OverTimeCalculateInMinAfter as int, @PV_OTMin as int, @PV_TotalWHourForOT as int
				, @PV_IsOTFromDayStart as bit, @PV_BrkTimeInMin as int , @PV_BrkTime as int 

		SET @PV_OTMin=0
		SET @PV_BrkTime=0

		--Check OT From Slab or Not
		IF EXISTS (SELECT * FROM ShiftOTSlab WITH (NOLOCK) WHERE ShiftID=@PV_ShiftID AND ISNULL(AchieveOTInMin,0)>0 AND IsActive=1)BEGIN SET @PV_IsOTFromSlab=1 END
		--IF EXISTS (SELECT * FROM ShiftOTSlab WITH (NOLOCK) WHERE ShiftID=@PV_ShiftID AND ISNULL(CompAchieveOTInMin,0)>0 AND IsCompActive=1)BEGIN SET @PV_IsCompOTFromSlab=1 END

		IF (@PV_TotalWorkingHourInMin>0)
		BEGIN--Workig hour
			IF @PV_IsRostered=1 AND ISNULL(@PV_MaxOTInMinATRoster,0)<=0
			BEGIN
				GOTO CONT_ENDOT;
			END
			IF (@PV_Shift_IsOT=0)
			BEGIN
				--get overtime policy from attendance scheme
				SELECT @PV_OverTimeCalculateInMinAfter=OvertimeCalculateInMinuteAfter, @PV_IsOTFromDayStart=IsOTCalTimeStartFromShiftStart,@PV_BrkTimeInMin=BreakageTimeInMinute 
				FROM AttendanceScheme WITH (NOLOCK) WHERE AttendanceSchemeID=@PV_AttendanceSchemeID 

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
						SET @PV_CompSlabOTInMin=ISNULL((SELECT top(1)CompAchieveOTInMin FROM ShiftOTSlab WHERE IsCompActive=1 AND ShiftID=@PV_ShiftID 
										AND @PV_OTMin BETWEEN CompMinOTInMin AND CompMaxOTInMin),0)
					END

					IF (@PV_IsOTFromSlab=1)
					BEGIN
						SET @PV_OTMin=ISNULL((SELECT top(1) AchieveOTInMin FROM ShiftOTSlab WHERE IsActive=1 AND ShiftID=@PV_ShiftID 
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
			CONT_ENDOT:
		END--Working		
		
			--Compliance Intime, EndTime, Working Hour,Overtime
			SET @PV_CompInTime=@PV_ActualStart
			SET @PV_CompOutTime=@PV_ActualEnd
			SET @PV_CompTotalWorkingHourInMin=@PV_TotalWorkingHourInMin
			SET @PV_CompOverTimeInMin=@PV_OTMin

			IF EXISTS (SELECT * FROM HRM_Shift WITH (NOLOCK) WHERE ShiftID=@PV_ShiftID AND CompMaxEndTime IS NOT NULL)
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
			
			--TimeKeeper Out Time
			SET @PV_TimeKeeperOutTime=@PV_ActualEnd
			IF @PV_OTMin>0
			BEGIN
				SET @PV_TimeKeeperOutTime=DATEADD(MINUTE,RAND()*10,DATEADD(MINUTE,@PV_OTMin, @PV_EndTime))
			END							

	--Insert or update Attendance daily
		IF EXISTS(SELECT * FROM AttendanceDaily WHERE EmployeeID=@Param_EmployeeID AND AttendanceDate=@PV_AttendanceDate)
		BEGIN	
			UPDATE [dbo].[AttendanceDaily]
					   SET [RosterPlanID] = @PV_RosterPlanID
						  ,[ShiftID] = @PV_ShiftID
						  ,[AttendanceDate] = @PV_AttendanceDate
						  ,[InTime] = @PV_ActualStart
						  ,[OutTime] = @PV_ActualEnd
						  ,[CompInTime] = @PV_CompInTime
						  ,[CompOutTime] = @PV_CompOutTime
						  ,[LateArrivalMinute] = @PV_LateInMunit
						  ,[EarlyDepartureMinute] = @PV_EarlyInMunit
						  ,[TotalWorkingHourInMinute] = @PV_TotalWorkingHourInMin
						  ,[OverTimeInMinute] = @PV_OTMin
						  ,[CompLateArrivalMinute] = @PV_LateInMunit
						  ,[CompEarlyDepartureMinute] = @PV_EarlyInMunit
						  ,[CompTotalWorkingHourInMinute] = @PV_CompTotalWorkingHourInMin
						  ,[CompOverTimeInMinute] = @PV_CompOverTimeInMin
						  ,[IsDayOff] = @PV_IsDayOff
						  ,[IsLeave] = @PV_IsCompLeave
						  ,[IsUnPaid] = @PV_LeaveIsUnPaid
						  ,[IsHoliday] = @PV_IsHoliday
						  ,[IsCompDayOff] = @PV_CompIsDayOff
						  ,[IsCompLeave] = @PV_IsLeave
						  ,[IsCompHoliday] = @PV_CompIsHoliday					  					  					  
						  --,[IsLock] = 0					  					  
						  ,[LastUpdatedBY] = @Param_DBUserID
						  ,[LastUpdatedDate] = GETDATE()
						  ,[LeaveHeadID] = @PV_LeaveHeadID
						  ,[CompLeaveHeadID]=@PV_CompLeaveHeadID
						  ,TimeKeeperOutTime=@PV_TimeKeeperOutTime
						WHERE AttendanceID= (SELECT TOP(1)AttendanceID FROM AttendanceDaily WHERE EmployeeID=@Param_EmployeeID AND AttendanceDate=@PV_AttendanceDate)
		END
		ELSE
		BEGIN
		INSERT INTO [AttendanceDaily]
					([EmployeeID],[AttendanceSchemeID],[LocationID],[DepartmentID],[DesignationID],[RosterPlanID],[ShiftID],[AttendanceDate],[InTime],[OutTime],[CompInTime],[CompOutTime],[LateArrivalMinute],[EarlyDepartureMinute],[TotalWorkingHourInMinute],[OverTimeInMinute],[CompLateArrivalMinute],[CompEarlyDepartureMinute],[CompTotalWorkingHourInMinute],[CompOverTimeInMinute],[IsDayOff],[IsLeave],[IsUnPaid],[IsHoliday],[IsCompDayOff],[IsCompLeave],[IsCompHoliday],[WorkingStatus],[Note],[APMID],[IsLock],[IsNoWork],[IsManual],[LastUpdatedBY],[LastUpdatedDate],[DBUSerID],[DBServerDateTime],[LeaveHeadID],[IsOSD],[TimeKeeperOutTime],[ISELProcess],BusinessUnitID,Remark,[CompLeaveHeadID])
					VALUES
					(@Param_EmployeeID,@PV_AttendanceSchemeID,@PV_LocationID,@PV_DepartmentID,@PV_DesignationID,@PV_RosterPlanID
					,@PV_ShiftID,@PV_AttendanceDate,@PV_ActualStart,@PV_ActualEnd,@PV_CompInTime,@PV_CompOutTime,@PV_LateInMunit,@PV_EarlyInMunit
					,@PV_TotalWorkingHourInMin,@PV_OTMin,@PV_LateInMunit,@PV_EarlyInMunit,@PV_CompTotalWorkingHourInMin,@PV_CompOverTimeInMin,@PV_IsDayOff,@PV_IsLeave
					,@PV_LeaveIsUnPaid,@PV_IsHoliday,@PV_CompIsDayOff,@PV_IsCompLeave,@PV_CompIsHoliday,1/*In Work Place*/,'System Generated',@PV_APMID
					,@PV_IsLock/*IsLocl*/,0/*ByDefault Nowork 0*/,0/*Not manual*/,@Param_DBUserID,GETDATE(),@Param_DBUserID,GETDATE(),@PV_LeaveHeadID,0/*OSD*/,@PV_TimeKeeperOutTime,0,@PV_BusinessUnitID,@PV_Remark,@PV_CompLeaveHeadID)
		END		
  	
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
	,@PV_IsComp as bit	
	,@PV_BOA_IsOTSlab as bit

	--Delete BOA 
	DELETE FROM BenefitOnAttendanceEmployeeLedger WHERE  BOAEmployeeID IN (
	SELECT BOAEmployeeID FROM BenefitOnAttendanceEmployee WHERE EmployeeID=@Param_EmployeeID AND BOAID IN (
	SELECT BOAID FROM BenefitOnAttendance WHERE LeaveHeadID=0 OR LeaveHeadID IS NULL))
	ANd AttendanceDate=@PV_AttendanceDate

	DECLARE Cur_BOA CURSOR LOCAL FORWARD_ONLY KEYSET FOR
	SELECT BOAID,BOAEmployeeID,IsTemporaryAssign FROM BenefitOnAttendanceEmployee WHERE EmployeeID=@Param_EmployeeID AND (InactiveDate IS NULL OR InactiveDate>@PV_AttendanceDate)
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
								  SELECT AttendanceSchemeID FROM EmployeeOfficial WHERE EmployeeID=@Param_EmployeeID))),0)
			SET @PV_EmpLeaveLedgerID=0
			SET @PV_BOA_IsExtraBenefit=0
			SET @PV_BOA_IsPercent=0
			SET @PV_BOA_value=0
			SET @PV_BenefitValue=0
			SET @PV_BOA_AllowanceOn=0
			SET @PV_BOATS_ValueInPercent=0
			SET @PV_BenefitDurationInMinute=0
			SET @PV_IsComp = 1
			SET @PV_BOA_IsOTSlab=0

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
				SET @PV_Gross=ISNULL((SELECT ActualGrossAmount FROM EmployeeSalaryStructure WHERE EmployeeID=@Param_EmployeeID),0)
				SET @PV_BOA_value =ISNULL((SELECT top(1)Value FROM BenefitOnAttendanceValueSlab WHERE BOAID=@PV_BOAID AND @PV_Gross BETWEEN MinGross AND MaxGross),0)
			END

			--Check Leave benefit
			IF @PV_BOA_LeaveHeadID>0 AND @PV_BOA_LeaveAmount>0
			BEGIN						
				IF EXISTS (SELECT * FROM BenefitOnAttendanceEmployeeLedger WHERE BOAEmployeeID=@PV_BOAEmployeeID AND AttendanceDate=@PV_AttendanceDate)
				BEGIN
					SET @PV_EmpLeaveLedgerID=ISNULL((SELECT EmpLeaveLedgerID FROM EmployeeLeaveLedger	
											WHERE ACSID =@PV_ACSID AND LeaveID=@PV_BOA_LeaveHeadID	
											AND EmployeeID=@Param_EmployeeID),0)

					UPDATE EmployeeLeaveLedger SET TotalDay=CASE WHEN TotalDay>0 THEN TotalDay-@PV_BOA_LeaveAmount ELSE 0 END
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
				IF (@PV_BOA_OTInMinute>0 AND @PV_BOA_IsFullOT=0)
				BEGIN--Start OT Benefit
					IF (@PV_BOA_OTDistributePerPresence>0)
					BEGIN--OTDistributePerPresence
						SET @PV_OverTimeDayCount=@PV_BOA_OTInMinute/@PV_BOA_OTDistributePerPresence
						--get last salaryDate
						SELECT @PV_LastSalaryDate=MAX(EndDate) FROM EmployeeSalary WHERE EmployeeID=@Param_EmployeeID
						--Get previous present day
						IF (@PV_LastSalaryDate IS NOT NULL)
						BEGIN
							UPDATE 	top(@PV_OverTimeDayCount)AttendanceDaily 
							SET OverTimeInMinute=@PV_BOA_OTDistributePerPresence
							WHERE EmployeeID=@Param_EmployeeID AND AttendanceDate>	@PV_LastSalaryDate
							AND OverTimeInMinute<180							
						END ELSE BEGIN
							UPDATE 	top(@PV_OverTimeDayCount)AttendanceDaily 
							SET OverTimeInMinute=@PV_BOA_OTDistributePerPresence
							WHERE EmployeeID=@Param_EmployeeID --AND AttendanceDate>	@PV_LastSalaryDate
							AND OverTimeInMinute<180
						END
					END--OTDistributePerPresence
					ELSE BEGIN
						UPDATE AttendanceDaily SET OverTimeInMinute= @PV_BOA_OTInMinute 
						WHERE EmployeeID=@Param_EmployeeID AND AttendanceDate=@PV_AttendanceDate
					END
				END--END OT Benefit
				ELSE IF (@PV_BOA_IsFullOT=1)
				BEGIN
					SET @PV_BOA_OTInMinute=0
					SET @PV_BrkTime=0
					SET @PV_BOA_IsOTSlab=0


					IF EXISTS (SELECT * FROM BenefitOnAttendanceOTSlab WITH (NOLOCK) WHERE BOAID=@PV_BOAID AND OTInMin>0)
					BEGIN
						SET @PV_BOA_IsOTSlab=1
					END
					 
					 IF @PV_BOA_IsOTSlab=0
					 BEGIN--OT Slab
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

							IF (DATEPART(MINUTE,@PV_StartTime)=DATEPART(MINUTE,@PV_ActualEnd)  OR @PV_BaseAddress='mamiya')
							BEGIN
								SET @PV_BOA_OTInMinute=DATEDIFF(MINUTE,CASE WHEN @PV_StartTime>@PV_ActualStart THEN @PV_StartTime ELSE @PV_ActualStart END,@PV_ActualEnd)-ISNULL(@PV_BrkTime,0)+@PV_OTMin--@PV_EarlyInMunit-@PV_LateInMunit
							END ELSE BEGIN
								SET @PV_BOA_OTInMinute=DATEDIFF(MINUTE,CASE WHEN @PV_StartTime>@PV_ActualStart THEN @PV_StartTime ELSE @PV_ActualStart END,@PV_ActualEnd)+1-ISNULL(@PV_BrkTime,0)+@PV_OTMin--@PV_EarlyInMunit-@PV_LateInMunit
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
					WHERE EmployeeID=@Param_EmployeeID AND AttendanceDate=@PV_AttendanceDate
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
											AND EmployeeID=@Param_EmployeeID),0)

					IF @PV_EmpLeaveLedgerID>0
					BEGIN
						UPDATE EmployeeLeaveLedger SET TotalDay=TotalDay+@PV_BOA_LeaveAmount
						WHERE EmpLeaveLedgerID=@PV_EmpLeaveLedgerID
					END 
					ELSE BEGIN
					    --Asad
						SET  @PV_IsComp = (SELECT IsComp FROM AttendanceSchemeLeave WHERE AttendanceSchemeID=@PV_AttendanceSchemeID AND LeaveID= @PV_BOA_LeaveHeadID)
						INSERT INTO [dbo].[EmployeeLeaveLedger]
						([EmpLeaveLedgerID],[EmployeeID],[ACSID],[ASLID],[LeaveID],[DeferredDay],[ActivationAfter],[TotalDay],[IsLeaveOnPresence],[PresencePerLeave],[IsCarryForward],[MaxCarryDays],[DBUSerID],[DBServerDateTime],[IsComp])
						VALUES	((SELECT ISNULL(MAX(EmpLeaveLedgerID),0)+1 FROM EmployeeLeaveLedger)
						,@Param_EmployeeID,@PV_ACSID,0,@PV_BOA_LeaveHeadID,1,1,@PV_BOA_LeaveAmount,0,0,0,0,@Param_DBUserID,GETDATE(),@PV_IsComp)
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
						SET @PV_BenefitValue=ISNULL((SELECT ActualGrossAmount FROM EmployeeSalaryStructure WHERE EmployeeID=@Param_EmployeeID),0)/@PV_DaysOfMonth
					END ELSE IF @PV_BOA_AllowanceOn=2
					BEGIN
						SET @PV_BenefitValue=ISNULL((SELECT Max(Amount) FROM EmployeeSalaryStructureDetail WHERE ESSID IN (
											SELECT ESSID FROM EmployeeSalaryStructure WHERE EmployeeID=@Param_EmployeeID)),0) /@PV_DaysOfMonth
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

	CONT:
	SET @Param_StartDate=DATEADD(day,1,@Param_StartDate) 
END

SELECT * FROM VIEW_AttendanceDaily WHERE EmployeeID=@Param_EmployeeID AND AttendanceDate BETWEEN @PV_StartDate AND @Param_EndDate  
COMMIT TRAN














GO
/****** Object:  StoredProcedure [dbo].[SP_Process_AttendanceDaily_V1]    Script Date: 10/23/2018 11:07:13 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[SP_Process_AttendanceDaily_V1]
(
--DECLARE
		@Param_BusinessUnitID as int,
	    @Param_CompanyID as int,
		@Param_LocationID as int, 
		@Param_DepartmentID as int, 
		@Param_ShiftID as int,
		@Param_AttendanceDate as DateTime,
		@Param_APMID as int, 
		@Param_DBUserID as int,		
		@Param_Index as int --for pagination top 300. 
)
AS
BEGIN
--SET @Param_CompanyID =1
--SET @Param_LocationID =2
--SET @Param_DepartmentID =12
--SET @PV_ShiftID =1
--SET @Param_AttendanceDate ='22 Feb 2014'
--SET @Param_APMID =88
--SET @Param_DBUserID =8
-- =============================================
-- Author:		MD. Masud Iqbal
-- Create date: 23 Jan 2014
-- Description:	Process
-- =============================================

--Daily Attendance Process
/*
SELECT GETDATE()
SELECT CAST (GETDATE() AS TIME(0))
SELECT DATEPART(HOUR,GETDATE())
SELECT DATEPART(MINUTE,GETDATE())
SELECT DATEPART(SECOND,GETDATE())
SELECT DATEADD(SECOND,10,DATEADD (MINUTE,20, DATEADD(HOUR, 18, DATEADD(dd, DATEDIFF(dd, -0, GETDATE()), 0)))) 

Description: The purpose of this SP is to process Attendance 
			 from Employee Punch log for each employee
 
Step 01. Parameter : CompanyID,LocationID, DepartmentID, ShiftID, DBUserID,
		             AttendanceDate(Who have the process Permission) 
		             
*Setp 02. Check User Permission from Process Management Permission. 
		 If no Permission rollback.
		 
*Step 03. Check Attendance Already Processed for given date. 
		 if yes rollback.
		
*Step 04. Insert AttendanceProcessManagement		 
		 
Step 05. GET Schedule By Param ShiftID
		 
*Step 06. Check previous day's attendance process. If not found get the 
		 last processed Attendance date and roll back with message (last date).
		 
Step 07. Get Distinct Attendance Scheme information BY Employee official with param.		 

Step 08. Check this Day is working Day for that employee by Attendance scheme Holiday, 
		 If no skip process for that employee.
		 
Step 09. Check Day Off. IF yes, Insert Attendance Daily as schedule with isDayOff true. 

Step 10. If Day Off true, Check Employee's Leave and update Attendance Information.

Step 11. the following process will be eligible if Step 08 false.

Step 12. Get Employees By Attendance SchemeID, CurrentShiftID, LocationID, DepartmentID.

Step 13. Get Punch log of Employee by employee id and Attendance Date

Step 14. Check Schedule Start and end time with punch time and find Start and end time 

Step 15. Insert Attendance Daily with informaton, If there is no punch for this 
		 employee check leave for this empoyee and insert all 0 with leave.
		 
Step 16. If there is no leave information, he is absent and insert all 0.   		        		 

Step 17. End process.
		 
		 
 
*/


--Step 01 : 


--Variable Declaration
DECLARE @PV_StartTime as DATETIME--time(0)
	,@PV_EndTime as DATETIME--time(0)
	,@PV_AttendanceSchemeID as int
	,@PV_AttendanceCalendarID as int
	,@PV_RosterPlanID as int
	,@PV_EarlyStart as DATETIME
	,@PV_DelayStart as DATETIME
	,@PV_ActualStart as DATETIME
	,@PV_EarlyEnd as Datetime
	,@PV_DelayEnd as Datetime
	,@PV_ActualEnd as Datetime		
	,@PV_EmployeeID as int
	,@PV_EmployeeTypeID as int
	,@PV_DesignationID as int
	,@PV_LateInMunit as int
	,@PV_EarlyInMunit as int		
	,@PV_TotalWorkingHourInMin as int
	,@PV_IsDayOff as bit
	,@PV_IsLeave as bit
	,@PV_IsCompLeave as bit
	,@PV_LeaveIsUnPaid as bit
	,@PV_Index as int
	,@PV_AttCount as int 
	,@PV_TolaranceTime as Datetime--int
	,@PV_DayEndTime as DATETIME--time(0)
	,@PV_DayStartTime as DATETIME--time(0)
	,@PV_Remark as varchar(50)

	--ASAD
	,@PV_IsHalfDayoff AS BIT
	,@PV_PStart AS DATETIME
	,@PV_PEnd AS DATETIME

	,@PV_IsAllowOverTime as bit		
	--,@PV_FirstDayOffDate as Date		
	--,@PV_Top AS int

	,@PV_IsHoliday as bit--21 Jan 2016
	--,@PV_StartAttendanceDateTime as datetime
	--,@PV_EndAttendanceDateTime as datetime
	--,@PV_IsNightShift

	,@PV_Shift_IsOT as bit
	,@PV_Shift_OTStart as datetime
	,@PV_Shift_OTEnd   as datetime
	,@PV_Shift_ISOTOnActual as bit
	,@PV_Shift_OTCalclateAfterInMin as int
	,@PV_Shift_IsLeaveOnOFFHoliday as bit
			
	,@PV_AttendanceID as int
	,@PV_IsManual as int
	,@PV_LeaveHeadID as int
	,@PV_CompLeaveHeadID as int
	,@PV_ShiftID as int
	,@PV_ToleranceEarly as int

	--For Compliance
	,@PV_CompInTime as datetime
	,@PV_CompOutTime as DateTime
	,@PV_CompIsDayOff as bit
	,@PV_CompIsHoliday as bit
	,@PV_CompOverTimeInMin as int
	,@PV_CompMaxCompOTInMin as int
	,@PV_CompTotalWorkingHourInMin as int
	,@PV_MaxOTInMinATRoster as int
	,@PV_TimeKeeperOutTime as datetime
	,@PV_LastWorkingDay as date
	,@PV_BaseAddress AS varchar(100)
	,@PV_IsGWD as bit ---General Working Day
	,@PV_IsCompGWD as bit
	,@PV_IsRostered as bit
	,@PV_CompMaxEndTime DATETIME
	,@PV_CompSlabOTInMin as int
	,@PV_IsCompOTFromSlab as bit
	,@PV_IsOTFromSlab as bit

	--For Contractual & Seasonal
	,@PV_ContractEndDate AS DATE

	SET @PV_BaseAddress=(SELECT top(1)BaseAddress FROM Company)

	SET @PV_ShiftID=@Param_ShiftID
		
	--For Break time
	DECLARE @tbl_BreakTime AS TABLE (BStart DateTime, BEnd DAtetime, BDurationMin int)


	--Common Validation
	IF @Param_BusinessUnitID<=0 AND @Param_CompanyID<=0 OR @Param_LocationID<=0 OR @Param_DepartmentID<=0 OR @PV_ShiftID<=0 OR @Param_APMID<=0
	BEGIN
		ROLLBACK
			RAISERROR (N'Some of the mandatory field missing!!',16,1);
		RETURN	
	END

	IF (@Param_Index is null)
	BEGIN
		ROLLBACK
			RAISERROR (N'Please Set Index first.!!',16,1);
		RETURN	
	END

	IF NOT EXISTS (SELECT * FROM HRM_Shift WHERE ShiftID=@PV_ShiftID)
	BEGIN
		ROLLBACK
			RAISERROR (N'Invalid Shift!!',16,1);
		RETURN		
	END

--Check General Working Day
SET @PV_IsGWD=0
SET @PV_IsCompGWD=0

IF EXISTS (SELECT * FROM GeneralWorkingDay WITH (NOLOCK) WHERE AttendanceDate=@Param_AttendanceDate AND GWDID IN (
			SELECT GWDID FROM GeneralWorkingDayDetail WHERE DRPID IN (
			SELECT DepartmentRequirementPolicyID FROM DepartmentRequirementPolicy WHERE BusinessUnitID=@Param_BusinessUnitID 
			AND LocationID=@Param_LocationID AND DepartmentID=@Param_DepartmentID)))
BEGIN
	SET @PV_IsGWD=1
	SET @PV_IsCompGWD=ISNULL((SELECT IsCompApplicable FROM GeneralWorkingDay WITH (NOLOCK) WHERE AttendanceDate=@Param_AttendanceDate),1)--if Null then comp & actual applicable
END

IF @PV_IsGWD=0
BEGIN
	IF EXISTS (SELECT * FROM GeneralWorkingDay WITH (NOLOCK) WHERE AttendanceDate=@Param_AttendanceDate)
	BEGIN
		SET @PV_IsGWD=1
		SET @PV_IsCompGWD=ISNULL((SELECT IsCompApplicable FROM GeneralWorkingDay WITH (NOLOCK) WHERE AttendanceDate=@Param_AttendanceDate),1)--if Null then comp & actual applicable
	END
END

DECLARE @tbl_EmployeeOfficial AS TABLE (RowID int, EmployeeID int,DesignationID int,EmployeeTypeID int,AttendanceSchemeID int)
INSERT INTO @tbl_EmployeeOfficial
SELECT aa.Row,aa.EmployeeID,aa.DesignationID,aa.EmployeeTypeID,aa.AttendanceSchemeID FROM (SELECT ROW_NUMBER() OVER(ORDER BY EmployeeOfficialID) AS Row, 
* FROM VIEW_EmployeeOfficial WITH (NOLOCK) WHERE CurrentShiftID=@PV_ShiftID AND DepartmentID=@Param_DepartmentID AND LocationID=@Param_LocationID AND BusinessUnitID=@Param_BusinessUnitID AND IsActive=1 AND DateOfJoin<=@Param_AttendanceDate)aa	
				
--Start loop for getting actual start and end time from machine data				
DECLARE Cur_CCC CURSOR LOCAL FORWARD_ONLY KEYSET FOR
SELECT top(20) EmployeeID,DesignationID,EmployeeTypeID,AttendanceSchemeID FROM @tbl_EmployeeOfficial WHERE RowID>@Param_Index
OPEN Cur_CCC
FETCH NEXT FROM Cur_CCC INTO  @PV_EmployeeID,@PV_DesignationID,@PV_EmployeeTypeID,@PV_AttendanceSchemeID
WHILE(@@Fetch_Status <> -1)		
BEGIN	
	/* Chek Manual Attendance & El process*/
	IF EXISTS (SELECT * FROM AttendanceDaily WITH (NOLOCK) WHERE EmployeeID=@PV_EmployeeID AND AttendanceDate=@Param_AttendanceDate AND (IsManual=1 OR ISELProcess=1))
	BEGIN--chk att existance
		GOTO CONT;
	END
	/* Chek Attendance & Delete*/
	IF EXISTS (SELECT * FROM AttendanceDaily WITH (NOLOCK) WHERE EmployeeID=@PV_EmployeeID AND AttendanceDate=@Param_AttendanceDate)
	BEGIN--chk att existance
		DELETE FROM AttendanceDaily WHERE EmployeeID=@PV_EmployeeID AND AttendanceDate=@Param_AttendanceDate
	END----chk att existance


	--value initialize
	SET @PV_IsDayOff=0;
	SET @PV_LeaveIsUnPaid=0;
	SET @PV_IsLeave=0;
	SET @PV_IsCompLeave=0;
	SET @PV_Index=0
	SET @PV_AttCount=0;
	SET @PV_IsAllowOverTime=0
	SET @PV_IsHoliday=0
	SET @PV_LateInMunit=0
	SET @PV_EarlyInMunit=0
	SET @PV_TotalWorkingHourInMin=0
	SET @PV_AttendanceID=0
	SET @PV_IsManual=0
	SET @PV_LeaveHeadID=0
	SET @PV_CompLeaveHeadID=0	
	SET @PV_CompIsDayOff=0
	SET @PV_CompIsHoliday=0
	SET @PV_CompOverTimeInMin=0
	SET @PV_CompTotalWorkingHourInMin=0
	SET @PV_MaxOTInMinATRoster=0
	SET @PV_Remark=NULL
	SET @PV_IsRostered=0
	SET @PV_CompSlabOTInMin=0
	SET @PV_CompMaxEndTime=NULL
	SET @PV_IsCompOTFromSlab=0
	SET @PV_IsOTFromSlab=0
	SET @PV_ToleranceEarly=0



	/*
		Employee Settlement Issue
		=========================
		If any employee has Approved but continued(IsResigned=0) settlement record by this or before date,
		Employee must be discontinued and must be deleted Attendance & BOA record after last working day.
	*/
	IF EXISTS (SELECT * FROM EmployeeSettlement WITH (NOLOCK) WHERE EmployeeID=@PV_EmployeeID AND IsResigned=0 AND ApproveBy>0 AND EffectDate<=@Param_AttendanceDate)
	BEGIN
		--Get LAst working day
		SET @PV_LastWorkingDay=(SELECT EffectDate FROM EmployeeSettlement WHERE EmployeeID=@PV_EmployeeID)
		--Delete Attendance record
		DELETE FROM AttendanceDaily WHERE EmployeeID=@PV_EmployeeID AND AttendanceDate>@PV_LastWorkingDay		
		DELETE FROM BenefitOnAttendanceEmployeeLedger WHERE BOAEmployeeID IN (
		SELECT EmployeeID FROM BenefitOnAttendanceEmployee WITH (NOLOCK) WHERE EmployeeID=@PV_EmployeeID) 
		AND AttendanceDate>@PV_LastWorkingDay
		
		--Discontinue
		UPDATE Employee SET  IsActive = 0 WHERE EmployeeID= @PV_EmployeeID
		UPDATE EmployeeOfficial SET  IsActive = 0, WorkingStatus=6 WHERE EmployeeID= @PV_EmployeeID
		UPDATE EmployeeAuthentication SET  IsActive = 0 WHERE EmployeeID= @PV_EmployeeID
		UPDATE EmployeeSalaryStructure SET  IsActive = 0 WHERE EmployeeID= @PV_EmployeeID
		UPDATE PFmember SET  IsActive = 0 WHERE EmployeeID= @PV_EmployeeID
		
		UPDATE BenefitOnAttendanceEmployee SET  InactiveBy = @Param_DBUserID,InactiveDate=@PV_LastWorkingDay 
		WHERE EmployeeID= @PV_EmployeeID AND (InactiveBy<=0 OR InactiveBy IS NULL)

		UPDATE EmployeeSettlement SET  IsResigned = 1 WHERE EmployeeID= @PV_EmployeeID	
		
		IF (@PV_LastWorkingDay<@Param_AttendanceDate)
		BEGIN
			GOTO CONT
		END	
	END

	/*
		For Contractual & Seasonal Employee
		===================================
		This kind of employee must has Specific duration. No attendance Record can be occuer except duration.
		In this section If this is the after the last date and no record in Employee Settlement, Insert Employee
		Settlement record.
		
		If exists record in  employee settlement return.
	*/
	IF EXISTS (SELECT * FROm EmployeeConfirmation WITH (NOLOCK) WHERE EmployeeID=@PV_EmployeeID AND EmployeeCategory IN (3,4))--Contractual & Seasonal
	BEGIN
		SET @PV_ContractEndDate=(SELECT DateOfConfirmation FROM EmployeeOfficial WITH (NOLOCK) WHERE EmployeeID=@PV_EmployeeID)
		IF (@Param_AttendanceDate>@PV_ContractEndDate)
		BEGIN
			IF NOT EXISTS (SELECT * FROM EmployeeSettlement WITH (NOLOCK) WHERE EmployeeID=@PV_EmployeeID)
			BEGIN
				INSERT INTO [dbo].[EmployeeSettlement]
						   ([EmployeeSettlementID],[EmployeeID],[Reason],[SubmissionDate],[EffectDate],[SettlementType],[IsNoticePayDeduction],[ApproveBy],[ApproveByDate],[IsResigned],[DBUserID],[DBServerDateTime],[PaymentDate],[IsSalaryHold])
					 VALUES((SELECT ISNULL(MAX(EmployeeSettlementID),0)+1 FROM EmployeeSettlement)
							,@PV_EmployeeID,'Date Over',@PV_ContractEndDate,@PV_ContractEndDate,1,0,0
							,GETDATE(),0,@Param_DBUserID,GETDATE(),NULL,1)				
			END 
			
			GOTO CONT
		END
	END

	
	SET @PV_ShiftID=@Param_ShiftID

	--check Shift rostering
	IF EXISTS (SELECT * FROM RosterPlanEmployee WITH (NOLOCK) WHERE EmployeeID=@PV_EmployeeID AND AttendanceDate=@Param_AttendanceDate)
	BEGIN
		SET @PV_IsRostered=1
		SET @PV_ShiftID=0

		SELECT top(1)@PV_ShiftID=ShiftID
		,@PV_IsDayOff=ISNULL(IsDayOff,0)
		,@PV_IsHoliday=ISNULL(IsHoliday,0)
		,@PV_MaxOTInMinATRoster=ISNULL(MaxOTInMin,0)
		,@PV_Remark=Remarks 
		,@PV_StartTime=(DATEADD(SECOND,0,DATEADD (MINUTE,DATEPART(MINUTE,InTime), DATEADD(HOUR, DATEPART(HOUR,InTime), DATEADD(dd, DATEDIFF(dd, -0, @Param_AttendanceDate), 0)))) )--CAST (StartTime AS TIME(0))
		,@PV_EndTime=(DATEADD(SECOND,0,DATEADD (MINUTE,DATEPART(MINUTE,OutTime), DATEADD(HOUR, DATEPART(HOUR,OutTime), DATEADD(dd, DATEDIFF(dd, -0, @Param_AttendanceDate), 0)))) )--CAST (EndTime AS TIME(0)) 	   
		
		FROM RosterPlanEmployee WITH (NOLOCK) WHERE EmployeeID=@PV_EmployeeID AND AttendanceDate=@Param_AttendanceDate		

		SELECT 			
		@PV_DayStartTime=(DATEADD(SECOND,0,DATEADD (MINUTE,DATEPART(MINUTE,DayStartTime), DATEADD(HOUR, DATEPART(HOUR,DayStartTime), DATEADD(dd, DATEDIFF(dd, -0, @Param_AttendanceDate), 0)))) )
		,@PV_DayEndTime=(DATEADD(SECOND,0,DATEADD (MINUTE,DATEPART(MINUTE,DayEndTime), DATEADD(HOUR, DATEPART(HOUR,DayEndTime), DATEADD(dd, DATEDIFF(dd, -0, @Param_AttendanceDate), 0)))) )
		,@PV_TolaranceTime=(DATEADD(SECOND,0,DATEADD (MINUTE,DATEPART(MINUTE,ToleranceTime), DATEADD(HOUR, DATEPART(HOUR,ToleranceTime), DATEADD(dd, DATEDIFF(dd, -0, @Param_AttendanceDate), 0)))) )
		,@PV_Shift_IsOT=IsOT
		,@PV_Shift_OTStart=(DATEADD(SECOND,0,DATEADD (MINUTE,DATEPART(MINUTE,OTStartTime), DATEADD(HOUR, DATEPART(HOUR,OTStartTime), DATEADD(dd, DATEDIFF(dd, -0, @Param_AttendanceDate), 0)))) )
		,@PV_Shift_OTEnd=(DATEADD(SECOND,0,DATEADD (MINUTE,DATEPART(MINUTE,OTEndTime), DATEADD(HOUR, DATEPART(HOUR,OTEndTime), DATEADD(dd, DATEDIFF(dd, -0, @Param_AttendanceDate), 0)))) )
		,@PV_CompMaxEndTime=(DATEADD(SECOND,0,DATEADD (MINUTE,DATEPART(MINUTE,CompMaxEndTime), DATEADD(HOUR, DATEPART(HOUR,CompMaxEndTime), DATEADD(dd, DATEDIFF(dd, -0, @Param_AttendanceDate), 0)))) )--CAST (EndTime AS TIME(0)) 	   
		,@PV_Shift_ISOTOnActual=ISNULL(IsOTOnActual,0)
		,@PV_CompMaxCompOTInMin=MaxOTComplianceInMin
		,@PV_Shift_OTCalclateAfterInMin=ISNULL(OTCalculateAfterInMinute,0)
		,@PV_Shift_IsLeaveOnOFFHoliday=IsLeaveOnOFFHoliday
		,@PV_ToleranceEarly=ISNULL(ToleranceForEarlyInMin,0)

		--ASAD
		,@PV_IsHalfDayoff = ISNULL(IsHalfDayOff, 0)
		,@PV_PStart = PStart
		,@PV_PEnd = PEnd
		FROM HRM_Shift WITH (NOLOCK) WHERE ShiftID=@PV_ShiftID

		IF @PV_StartTime IS NULL
		BEGIN
			SELECT @PV_StartTime=(DATEADD(SECOND,0,DATEADD (MINUTE,DATEPART(MINUTE,StartTime), DATEADD(HOUR, DATEPART(HOUR,StartTime), DATEADD(dd, DATEDIFF(dd, -0, @Param_AttendanceDate), 0)))) )--CAST (StartTime AS TIME(0))					
			FROM HRM_Shift WITH (NOLOCK) WHERE ShiftID=@PV_ShiftID			
		END

		IF @PV_EndTime IS NULL
		BEGIN
			SELECT @PV_EndTime=(DATEADD(SECOND,0,DATEADD (MINUTE,DATEPART(MINUTE,EndTime), DATEADD(HOUR, DATEPART(HOUR,EndTime), DATEADD(dd, DATEDIFF(dd, -0, @Param_AttendanceDate), 0)))) )--CAST (EndTime AS TIME(0)) 	   
			FROM HRM_Shift WITH (NOLOCK) WHERE ShiftID=@PV_ShiftID			
		END

		--For compliance(The following code has been blocked because dayoff and holiday will be occur as per scheme not current roster. roster for compliance will be made later,)
		--SET @PV_CompIsDayOff=@PV_IsDayOff
		--SET @PV_CompIsHoliday=@PV_IsHoliday

	END--check Shift rostering
	ELSE BEGIN 
		SELECT @PV_StartTime=(DATEADD(SECOND,0,DATEADD (MINUTE,DATEPART(MINUTE,StartTime), DATEADD(HOUR, DATEPART(HOUR,StartTime), DATEADD(dd, DATEDIFF(dd, -0, @Param_AttendanceDate), 0)))) )--CAST (StartTime AS TIME(0))
				,@PV_EndTime=(DATEADD(SECOND,0,DATEADD (MINUTE,DATEPART(MINUTE,EndTime), DATEADD(HOUR, DATEPART(HOUR,EndTime), DATEADD(dd, DATEDIFF(dd, -0, @Param_AttendanceDate), 0)))) )--CAST (EndTime AS TIME(0)) 	   
				,@PV_DayStartTime=(DATEADD(SECOND,0,DATEADD (MINUTE,DATEPART(MINUTE,DayStartTime), DATEADD(HOUR, DATEPART(HOUR,DayStartTime), DATEADD(dd, DATEDIFF(dd, -0, @Param_AttendanceDate), 0)))) )
				,@PV_DayEndTime=(DATEADD(SECOND,0,DATEADD (MINUTE,DATEPART(MINUTE,DayEndTime), DATEADD(HOUR, DATEPART(HOUR,DayEndTime), DATEADD(dd, DATEDIFF(dd, -0, @Param_AttendanceDate), 0)))) )
				,@PV_CompMaxEndTime=(DATEADD(SECOND,0,DATEADD (MINUTE,DATEPART(MINUTE,CompMaxEndTime), DATEADD(HOUR, DATEPART(HOUR,CompMaxEndTime), DATEADD(dd, DATEDIFF(dd, -0, @Param_AttendanceDate), 0)))) )--CAST (EndTime AS TIME(0)) 	   
				,@PV_TolaranceTime=(DATEADD(SECOND,0,DATEADD (MINUTE,DATEPART(MINUTE,ToleranceTime), DATEADD(HOUR, DATEPART(HOUR,ToleranceTime), DATEADD(dd, DATEDIFF(dd, -0, @Param_AttendanceDate), 0)))) )
				,@PV_Shift_IsOT=IsOT
				,@PV_Shift_OTStart=(DATEADD(SECOND,0,DATEADD (MINUTE,DATEPART(MINUTE,OTStartTime), DATEADD(HOUR, DATEPART(HOUR,OTStartTime), DATEADD(dd, DATEDIFF(dd, -0, @Param_AttendanceDate), 0)))) )
				,@PV_Shift_OTEnd=(DATEADD(SECOND,0,DATEADD (MINUTE,DATEPART(MINUTE,OTEndTime), DATEADD(HOUR, DATEPART(HOUR,OTEndTime), DATEADD(dd, DATEDIFF(dd, -0, @Param_AttendanceDate), 0)))) )
				,@PV_Shift_ISOTOnActual=ISNULL(IsOTOnActual,0)
				,@PV_CompMaxCompOTInMin=MaxOTComplianceInMin
				,@PV_Shift_OTCalclateAfterInMin=ISNULL(OTCalculateAfterInMinute,0)
				,@PV_Shift_IsLeaveOnOFFHoliday=IsLeaveOnOFFHoliday
				,@PV_ToleranceEarly=ISNULL(ToleranceForEarlyInMin,0)

				--ASAD
				,@PV_IsHalfDayoff = ISNULL(IsHalfDayOff, 0)
				,@PV_PStart = PStart
				,@PV_PEnd = PEnd
		FROM HRM_Shift WITH (NOLOCK) WHERE ShiftID=@PV_ShiftID

	END

	--Break Times
	DELETE FROM @tbl_BreakTime	
	INSERT @tbl_BreakTime
	SELECT BStart,BEnd,DATEDIFF(MINUTE,BStart,BEnd) FROM 
	(SELECT (DATEADD(SECOND,0,DATEADD (MINUTE,DATEPART(MINUTE,StartTime), DATEADD(HOUR, DATEPART(HOUR,StartTime), DATEADD(dd, DATEDIFF(dd, -0, CASE WHEN DATEPART(HOUR,@PV_DayStartTime)<DATEPART(HOUR,StartTime) THEN @Param_AttendanceDate ELSE DATEADD(DAY,1,@Param_AttendanceDate) END), 0)))) ) AS BStart 
	,(DATEADD(SECOND,0,DATEADD (MINUTE,DATEPART(MINUTE,EndTime), DATEADD(HOUR, DATEPART(HOUR,EndTime), DATEADD(dd, DATEDIFF(dd, -0, CASE WHEN DATEPART(HOUR,@PV_DayStartTime)<DATEPART(HOUR,EndTime) THEN @Param_AttendanceDate ELSE DATEADD(DAY,1,@Param_AttendanceDate) END), 0)))) ) AS BEnd
	FROM ShiftBreakSchedule WITH (NOLOCK) WHERE ShiftID=@PV_ShiftID) ss


	IF (DATEPART(HOUR,@PV_DayStartTime)>DATEPART(HOUR,@PV_DayEndTime))
	BEGIN--Night Shift or different date
		SET @PV_DayEndTime=(DATEADD(SECOND,0,DATEADD (MINUTE,DATEPART(MINUTE,@PV_DayEndTime), DATEADD(HOUR, DATEPART(HOUR,@PV_DayEndTime), DATEADD(dd, DATEDIFF(dd, -1, @Param_AttendanceDate), 0)))) )
	END

	IF (DATEPART(HOUR,@PV_DayStartTime)=DATEPART(HOUR,@PV_DayEndTime) AND DATEPART(MINUTE,@PV_DayStartTime)>DATEPART(MINUTE,@PV_DayEndTime))
	BEGIN--Night Shift or different date
		SET @PV_DayEndTime=(DATEADD(SECOND,0,DATEADD (MINUTE,DATEPART(MINUTE,@PV_DayEndTime), DATEADD(HOUR, DATEPART(HOUR,@PV_DayEndTime), DATEADD(dd, DATEDIFF(dd, -1, @Param_AttendanceDate), 0)))) )
	END

	IF (DATEPART(HOUR,@PV_StartTime)>DATEPART(HOUR,@PV_EndTime))
	BEGIN--Night Shift or different date
		SET @PV_EndTime=(DATEADD(SECOND,0,DATEADD (MINUTE,DATEPART(MINUTE,@PV_EndTime), DATEADD(HOUR, DATEPART(HOUR,@PV_EndTime), DATEADD(dd, DATEDIFF(dd, -1, @Param_AttendanceDate), 0)))) )
		SET @PV_CompMaxEndTime=(DATEADD(SECOND,0,DATEADD (MINUTE,DATEPART(MINUTE,@PV_CompMaxEndTime), DATEADD(HOUR, DATEPART(HOUR,@PV_CompMaxEndTime), DATEADD(dd, DATEDIFF(dd, -1, @Param_AttendanceDate), 0)))) )
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
		SET @PV_Shift_OTEnd=(DATEADD(SECOND,0,DATEADD (MINUTE,DATEPART(MINUTE,@PV_Shift_OTEnd), DATEADD(HOUR, DATEPART(HOUR,@PV_Shift_OTEnd), DATEADD(dd, DATEDIFF(dd, -1, @Param_AttendanceDate), 0)))) )
	END

	--get Attendance Scheme info		
	SELECT @PV_AttendanceCalendarID=AttendanceCalenderID,@PV_RosterPlanID=RosterPlanID 
	FROM View_AttendanceScheme WHERE AttendanceSchemeID=@PV_AttendanceSchemeID
	IF (@PV_AttendanceCalendarID is null)
	BEGIN
		ROLLBACK 
			RAISERROR(N'Invalid Attendance Scheme.!!',16,1);
		RETURN;
	END	

	--get Leave Information
	IF (EXISTS (SELECT * FROM LeaveApplication WITH (NOLOCK) WHERE IsApprove=1/*ApproveBy>0*/ AND (CancelledBy=0 OR CancelledBy IS NULL) AND EmployeeID =@PV_EmployeeID	AND @Param_AttendanceDate BETWEEN CONVERT (DATE,StartDateTime) AND CONVERT(DATE,EndDateTime)))
	BEGIN 
		SET @PV_IsLeave=1;
		SET @PV_IsCompLeave=ISNULL((SELECT top(1)IsComp FROM EmployeeLeaveLedger WHERE EmpLeaveLedgerID IN (
							SELECT EmpLeaveLedgerID FROM LeaveApplication WHERE IsApprove=1/*ApproveBy>0*/ AND (CancelledBy=0 OR CancelledBy IS NULL) AND EmployeeID =@PV_EmployeeID	
							AND @Param_AttendanceDate BETWEEN CONVERT (DATE,StartDateTime) AND CONVERT(DATE,EndDateTime))),1)

		SET @PV_LeaveHeadID=(SELECT top(1)LeaveID FROM EmployeeLeaveLedger WHERE EmpLeaveLedgerID IN (
							SELECT EmpLeaveLedgerID FROM LeaveApplication WHERE IsApprove=1/*ApproveBy>0*/ AND (CancelledBy=0 OR CancelledBy IS NULL) AND EmployeeID =@PV_EmployeeID	
							AND @Param_AttendanceDate BETWEEN CONVERT (DATE,StartDateTime) AND CONVERT(DATE,EndDateTime)))
		
		IF(@PV_IsCompLeave=1)
		BEGIN
			SET @PV_CompLeaveHeadID = @PV_LeaveHeadID
		END ELSE BEGIN
			SET @PV_CompLeaveHeadID = 0
		END
		IF EXISTS (SELECT * FROM LeaveHead WITH (NOLOCK) WHERE IsLWP=1 AND LeaveHeadID IN (@PV_LeaveHeadID))
		BEGIN
			SET @PV_LeaveIsUnPaid=1
		END
	END

	-- check Holiday
	--IF @PV_IsGWD=0 AND @PV_IsHoliday=0--Not General working day and not rostered
	--BEGIN		
		IF EXISTS (SELECT * FROM AttendanceCalendarSessionHoliday WITH (NOLOCK) WHERE ACSID IN (
			SELECT ACSID FROM AttendanceCalendarSession WHERE AttendanceCalendarID=@PV_AttendanceCalendarID)
			--AND HolidayID IN (SELECT HolidayID FROM AttendanceSchemeHoliday WHERE AttendanceSchemeID=@PV_AttendanceSchemeID) 
			AND @Param_AttendanceDate BETWEEN StartDate AND EndDate)
		BEGIN--Holiday
			SET @PV_IsHoliday=1	
			SET @PV_CompIsHoliday=1	

			IF @PV_IsRostered=1			
			BEGIN
				UPDATE RosterPlanEmployee SET IsHoliday=1 WHERE AttendanceDate=@Param_AttendanceDate AND EmployeeID=@PV_EmployeeID
			END
		END
	--END

	IF EXISTS(SELECT * FROM AttendanceSchemeDayOff WHERE AttendanceSchemeID=@PV_AttendanceSchemeID AND DayOffType=3 AND [WeekDay]=DATENAME(DW,@Param_AttendanceDate))
	BEGIN
		IF @PV_IsHalfDayoff=1
		BEGIN
			SET @PV_IsDayOff=0;
			SET @PV_CompIsDayOff=0;
			SET @PV_StartTime=(DATEADD(SECOND,0,DATEADD (MINUTE,DATEPART(MINUTE,@PV_PStart), DATEADD(HOUR, DATEPART(HOUR,@PV_PStart), DATEADD(dd, DATEDIFF(dd, -0, @Param_AttendanceDate), 0)))) )--CAST (StartTime AS TIME(0))
			SET @PV_EndTime=(DATEADD(SECOND,0,DATEADD (MINUTE,DATEPART(MINUTE,@PV_PEnd), DATEADD(HOUR, DATEPART(HOUR,@PV_PEnd), DATEADD(dd, DATEDIFF(dd, -0, @Param_AttendanceDate), 0)))) )--CAST (EndTime AS TIME(0)) 	   
		END
	END					
	--get Day Off information
	IF  @PV_IsRostered=0--Not General working day & Non Rostered @PV_IsGWD=0 AND
	BEGIN
		IF EXISTS (SELECT * FROM AttendanceSchemeDayOff WHERE AttendanceSchemeID=@PV_AttendanceSchemeID AND [WeekDay]=DATENAME(DW,@Param_AttendanceDate))
		BEGIN
			SET @PV_IsDayOff=1;
			SET @PV_CompIsDayOff=1;
		END
	END ELSE BEGIN
		IF EXISTS (SELECT * FROM AttendanceSchemeDayOff WHERE AttendanceSchemeID=@PV_AttendanceSchemeID AND [WeekDay]=DATENAME(DW,@Param_AttendanceDate))
		BEGIN
			SET @PV_CompIsDayOff=1;
		END
	END

	IF @PV_IsHoliday=1	BEGIN SET @PV_IsDayOff=0; END
	IF @PV_CompIsHoliday=1 BEGIN SET @PV_CompIsDayOff=0; END


	IF @PV_IsGWD=1
	BEGIN
		IF @PV_IsCompGWD=1
		BEGIN
			SET @PV_CompIsDayOff=0;
			SET @PV_CompIsHoliday=0
		END
		SET @PV_IsDayOff=0;				
		SET @PV_IsHoliday=0		
	END

	IF (@PV_IsLeave=1)
	BEGIN
		SET @PV_IsDayOff=0
		SET @PV_IsHoliday=0							
	END

	IF (@PV_IsCompLeave=1)
	BEGIN
		SET @PV_CompIsDayOff=0;
		SET @PV_CompIsHoliday=0;							
	END
	----Suffix & Prefix for leave 
	--IF @PV_Shift_IsLeaveOnOFFHoliday=1
	--BEGIN--Leave Between DayOff or holiday
	--	IF (@PV_IsLeave=1)
	--	BEGIN
	--		SET @PV_IsDayOff=0
	--		SET @PV_IsHoliday=0
	--		SET @PV_CompIsDayOff=0;
	--		SET @PV_CompIsHoliday=0;							
	--	END
	--END--Leave Between DayOff or holiday				

	/*
		Date 01 Dec 2015/9.52 am, As Per Mr. Samad requirement
		01. If there is more than 1 punch found, first punch will be
			In time and last punch will be Out time.  

		02. IF There is one punch found. this must be In time.
		03. IF Same time found Only In time will be set // Date: 22 Dec 2015
	*/

	IF (@PV_AttendanceID<=0)
	BEGIN
		SELECT @PV_ActualStart=MIN(PunchDateTime)--Attendance Start
				,@PV_ActualEnd=MAX(PunchDateTime) --Attendance End
		FROM PunchLog WITH (NOLOCK) WHERE EmployeeID=@PV_EmployeeID					
		AND PunchDateTime BETWEEN @PV_DayStartTime AND @PV_DayEndTime 
	END	ELSE	
	BEGIN
		SELECT @PV_ActualStart=(DATEADD(SECOND,0,DATEADD (MINUTE,DATEPART(MINUTE,InTime), DATEADD(HOUR, DATEPART(HOUR,InTime), DATEADD(dd, DATEDIFF(dd, -0, @Param_AttendanceDate), 0)))) )
		,@PV_ActualEnd=(DATEADD(SECOND,0,DATEADD (MINUTE,DATEPART(MINUTE,OutTime), DATEADD(HOUR, DATEPART(HOUR,OutTime), DATEADD(dd, DATEDIFF(dd, -0, @Param_AttendanceDate), 0)))) ) 
		FROM AttendanceDaily WITH (NOLOCK) WHERE AttendanceID=@PV_AttendanceID
					
		IF (DATEPART(HOUR,@PV_ActualStart)>DATEPART(HOUR,@PV_ActualEnd))
		BEGIN--Night Shift or different date			
			SET @PV_ActualEnd=(DATEADD(SECOND,0,DATEADD (MINUTE,DATEPART(MINUTE,@PV_ActualEnd), DATEADD(HOUR, DATEPART(HOUR,@PV_ActualEnd), DATEADD(dd, DATEDIFF(dd, -1, @Param_AttendanceDate), 0)))) )
		END	
	END

	IF @PV_ActualStart IS NULL
	BEGIN
		SET @PV_ActualStart=(DATEADD(SECOND,0,DATEADD (MINUTE,0, DATEADD(HOUR, 0, DATEADD(dd, DATEDIFF(dd, -0, @Param_AttendanceDate), 0)))) )		
	END

	IF @PV_ActualEnd IS NULL
	BEGIN
		SET @PV_ActualEnd=(DATEADD(SECOND,0,DATEADD (MINUTE,0, DATEADD(HOUR, 0, DATEADD(dd, DATEDIFF(dd, -0, @Param_AttendanceDate), 0)))) )		
	END

	--in case of start & end time same
	IF  CONVERT (DATE,@PV_ActualStart)=CONVERT (DATE,@PV_ActualEnd)--Check Date
		AND DATEPART(HOUR,@PV_ActualStart)=DATEPART(HOUR,@PV_ActualEnd) --Hour check
		AND DATEPART(MINUTE,@PV_ActualStart)=DATEPART(MINUTE,@PV_ActualEnd)--Min Check
	BEGIN
		SET @PV_ActualEnd=(SELECT DATEADD(SECOND,0,DATEADD (MINUTE,0, DATEADD(HOUR, 0, DATEADD(dd, DATEDIFF(dd, -0, @PV_DayEndTime), 0)))))
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
	IF @PV_ActualStart>@PV_TolaranceTime AND @PV_IsLeave=0
	BEGIN
		SET @PV_LateInMunit=DATEDIFF(MINUTE,@PV_StartTime,@PV_ActualStart)
		SET @PV_LateInMunit= CASE WHEN @PV_LateInMunit<0 THEN 0 ELSE @PV_LateInMunit END
	END

	--find early
	IF CAST(@PV_ActualEnd AS TIME(0))!='00:00' AND @PV_ActualEnd<@PV_EndTime AND @PV_IsLeave=0
	BEGIN
		SET @PV_EarlyInMunit=DATEDIFF(MINUTE,@PV_ActualEnd,@PV_EndTime)
	END	
	--find early
	IF CAST(@PV_ActualEnd AS TIME(0))='00:00' AND CAST(@PV_ActualStart AS TIME(0))='00:00' AND @PV_IsLeave=0
	BEGIN
		SET @PV_EarlyInMunit=0
	END	
	--find early
	IF CAST(@PV_ActualEnd AS TIME(0))='00:00' AND CAST(@PV_ActualStart AS TIME(0))!='00:00' AND @PV_IsLeave=0
	BEGIN
		SET @PV_EarlyInMunit=DATEDIFF(MINUTE,@PV_ActualStart,@PV_EndTime)
	END												
	SET @PV_EarlyInMunit= CASE WHEN @PV_EarlyInMunit<0 THEN 0 ELSE @PV_EarlyInMunit END	
	SET @PV_EarlyInMunit= CASE WHEN @PV_EarlyInMunit>@PV_ToleranceEarly THEN @PV_EarlyInMunit ELSE 0 END	

	IF @PV_EarlyInMunit>0
	BEGIN
		IF EXISTS (SELECT * FROM @tbl_BreakTime)
		BEGIN
			DECLARE @PV_BrkEnd AS DATETIME
			SET @PV_BrkEnd = (SELECT MAX(BEnd) FROM @tbl_BreakTime)
			IF @PV_EarlyInMunit>0 AND @PV_BrkEnd IS NOT NULL AND @PV_EndTime<@PV_BrkEnd
			BEGIN
				SET @PV_EarlyInMunit=@PV_EarlyInMunit-DATEDIFF(MINUTE,@PV_EndTime,@PV_BrkEnd)
				SET @PV_EarlyInMunit= CASE WHEN @PV_EarlyInMunit<0 THEN 0 ELSE @PV_EarlyInMunit END
			END
		END
	END
		
	IF @PV_BaseAddress IN ('amg','A007','akcl','golden','mithela','union','b007','wangs') AND (@PV_IsDayOff=1 OR @PV_IsHoliday=1 OR @PV_IsLeave=1)
	BEGIN
		--As per Mr. Rahat request No Late & No Early in Dayoff Dated On 02 Aug 2018.
		SET @PV_LateInMunit=0
		SET @PV_EarlyInMunit=0
	END			
											 
	--find total working hour
	IF CAST(@PV_ActualStart AS TIME(0))!='00:00'  AND CAST(@PV_ActualEnd AS TIME(0))!='00:00'
	BEGIN
		SET @PV_TotalWorkingHourInMin=DATEDIFF(MINUTE,@PV_ActualStart,@PV_ActualEnd) 										 
	END									
	SET @PV_TotalWorkingHourInMin= CASE WHEN @PV_TotalWorkingHourInMin<0 THEN 0 ELSE @PV_TotalWorkingHourInMin END

	/****************Over time in minute ******************************/
		DECLARE @PV_OverTimeCalculateInMinAfter as int, @PV_OTMin as int, @PV_TotalWHourForOT as int
				, @PV_IsOTFromDayStart as bit, @PV_BrkTimeInMin as int , @PV_BrkTime as int 

		--Check OT From Slab or Not
		IF EXISTS (SELECT * FROM ShiftOTSlab WITH (NOLOCK) WHERE ShiftID=@PV_ShiftID AND ISNULL(AchieveOTInMin,0)>0 AND IsActive=1)BEGIN SET @PV_IsOTFromSlab=1 END
		--IF EXISTS (SELECT * FROM ShiftOTSlab WITH (NOLOCK) WHERE ShiftID=@PV_ShiftID AND ISNULL(CompAchieveOTInMin,0)>0 AND IsCompActive=1)BEGIN SET @PV_IsCompOTFromSlab=1 END
		
		SET @PV_OTMin=0
		SET @PV_BrkTime=0

		IF (@PV_TotalWorkingHourInMin>0)
		BEGIN--Workig hour
			IF @PV_IsRostered=1 AND ISNULL(@PV_MaxOTInMinATRoster,0)<=0
			BEGIN
				GOTO CONT_OTEND;
			END

			IF (@PV_Shift_IsOT=0)
			BEGIN--From scheme
				--get overtime policy from attendance scheme
				SELECT @PV_OverTimeCalculateInMinAfter=OvertimeCalculateInMinuteAfter, @PV_IsOTFromDayStart=IsOTCalTimeStartFromShiftStart,@PV_BrkTimeInMin=BreakageTimeInMinute 
				FROM AttendanceScheme WITH (NOLOCK) WHERE AttendanceSchemeID=@PV_AttendanceSchemeID 

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
						SET @PV_OTMin=DATEDIFF(MINUTE,@PV_ActualStart,CASE WHEN @PV_Shift_OTEnd>@PV_ActualEnd THEN @PV_ActualEnd ELSE @PV_Shift_OTEnd END)+1	--=====											
					END
					ELSE BEGIN
						SET @PV_OTMin=DATEDIFF(MINUTE,@PV_Shift_OTStart,CASE WHEN @PV_Shift_OTEnd>@PV_ActualEnd THEN @PV_ActualEnd ELSE @PV_Shift_OTEnd END)+1--=====
					END	
				END--Before
				--IF OT Begain After Shift End
				IF @PV_Shift_OTStart>=@PV_EndTime --=====
				BEGIN--After
					IF (@PV_ActualEnd<@PV_Shift_OTEnd)
					BEGIN
						SET @PV_OTMin=DATEDIFF(MINUTE,@PV_Shift_OTStart,@PV_ActualEnd)+1	--=====
					END
					ELSE BEGIN
						SET @PV_OTMin=DATEDIFF(MINUTE,@PV_Shift_OTStart,@PV_Shift_OTEnd)+1	--=====
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
				IF (@PV_OTMin>@PV_Shift_OTCalclateAfterInMin)
				BEGIN
					IF (@PV_IsCompOTFromSlab=1)
					BEGIN
						SET @PV_CompSlabOTInMin=ISNULL((SELECT top(1)CompAchieveOTInMin FROM ShiftOTSlab WHERE IsCompActive=1 AND ShiftID=@PV_ShiftID 
										AND @PV_OTMin BETWEEN CompMinOTInMin AND CompMaxOTInMin),0)
					END

					IF (@PV_IsOTFromSlab=1)
					BEGIN
						SET @PV_OTMin=ISNULL((SELECT top(1)AchieveOTInMin FROM ShiftOTSlab WHERE IsActive=1 AND ShiftID=@PV_ShiftID 
										AND @PV_OTMin BETWEEN MinOTInMin AND MaxOTInMin),0)
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
			CONT_OTEND:		
		END--WorkingHour
	/****************End Over time in minute**************************/

			--Compliance Intime, EndTime, Working Hour,Overtime
			SET @PV_CompInTime=@PV_ActualStart
			SET @PV_CompOutTime=@PV_ActualEnd
			SET @PV_CompTotalWorkingHourInMin=@PV_TotalWorkingHourInMin
			SET @PV_CompOverTimeInMin=@PV_OTMin

			IF EXISTS (SELECT * FROM HRM_Shift WITH (NOLOCK) WHERE ShiftID=@PV_ShiftID AND CompMaxEndTime IS NOT NULL)
			BEGIN
				SET @PV_CompMaxCompOTInMin=0
				SET @PV_CompMaxCompOTInMin=DATEDIFF(MINUTE,@PV_EndTime,@PV_CompMaxEndTime)+1
				SET @PV_CompOutTime=CASE WHEN @PV_ActualEnd>@PV_CompMaxEndTime AND @PV_OTMin<=0 THEN DATEADD(MINUTE,RAND()*(-10),@PV_CompMaxEndTime) ELSE @PV_ActualEnd END
			END

			IF (@PV_CompIsDayOff=1 OR @PV_CompIsHoliday=1)
			BEGIN
				SET @PV_CompInTime=(DATEADD(SECOND,0,DATEADD (MINUTE,0, DATEADD(HOUR, 0, DATEADD(dd, DATEDIFF(dd, -0, @Param_AttendanceDate), 0)))) )
				SET @PV_CompOutTime=(DATEADD(SECOND,0,DATEADD (MINUTE,0, DATEADD(HOUR, 0, DATEADD(dd, DATEDIFF(dd, -0, @Param_AttendanceDate), 0)))) )
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
			--TimeKeeper Out Time
			SET @PV_TimeKeeperOutTime=@PV_ActualEnd
			IF @PV_OTMin>0
			BEGIN
				SET @PV_TimeKeeperOutTime=DATEADD(MINUTE,RAND()*10,DATEADD(MINUTE,@PV_OTMin, @PV_EndTime))
			END
			
			--Insert Attendance daily
			INSERT INTO [AttendanceDaily]
			([EmployeeID],[AttendanceSchemeID],[LocationID],[DepartmentID],[DesignationID],[RosterPlanID],[ShiftID],[AttendanceDate],[InTime],[OutTime],[CompInTime],[CompOutTime],[LateArrivalMinute],[EarlyDepartureMinute],[TotalWorkingHourInMinute],[OverTimeInMinute],[CompLateArrivalMinute],[CompEarlyDepartureMinute],[CompTotalWorkingHourInMinute],[CompOverTimeInMinute],[IsDayOff],[IsLeave],[IsUnPaid],[IsHoliday],[IsCompDayOff],[IsCompLeave],[IsCompHoliday],[WorkingStatus],[Note],[APMID],[IsLock],[IsNoWork],[IsManual],[LastUpdatedBY],[LastUpdatedDate],[DBUSerID],[DBServerDateTime],[LeaveHeadID],[IsOsD],TimeKeeperOutTime,ISELProcess,BusinessUnitID,Remark,[CompLeaveHeadID])
			VALUES
			(@PV_EmployeeID,@PV_AttendanceSchemeID,@Param_LocationID,@Param_DepartmentID,@PV_DesignationID,@PV_RosterPlanID
			,@PV_ShiftID,@Param_AttendanceDate,@PV_ActualStart,@PV_ActualEnd,@PV_CompInTime,@PV_CompOutTime,@PV_LateInMunit,@PV_EarlyInMunit
			,@PV_TotalWorkingHourInMin,@PV_OTMin,@PV_LateInMunit,@PV_EarlyInMunit,@PV_CompTotalWorkingHourInMin,@PV_CompOverTimeInMin,@PV_IsDayOff,@PV_IsLeave
			,@PV_LeaveIsUnPaid,@PV_IsHoliday,@PV_CompIsDayOff,@PV_IsCompLeave,@PV_CompIsHoliday,1/*In Work Place*/,'System Generated',@Param_APMID
			,0/*IsLocl*/,0/*ByDefault Nowork 0*/,0/*Not manual*/,@Param_DBUserID,GETDATE(),@Param_DBUserID,GETDATE(),@PV_LeaveHeadID,0/*OSD*/,@PV_TimeKeeperOutTime,0,@Param_BusinessUnitID,@PV_Remark,@PV_CompLeaveHeadID)
				

				
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
				,@PV_IsComp as bit
				,@PV_BOA_IsOTSlab as bit			

				--Delete BOA except Leave benefit
				DELETE FROM BenefitOnAttendanceEmployeeLedger WHERE  BOAEmployeeID IN (
				SELECT BOAEmployeeID FROM BenefitOnAttendanceEmployee WHERE EmployeeID=@PV_EmployeeID AND BOAID IN (
				SELECT BOAID FROM BenefitOnAttendance WITH (NOLOCK) WHERE LeaveHeadID=0 OR LeaveHeadID IS NULL))
				ANd AttendanceDate=@Param_AttendanceDate

				DECLARE Cur_BOA CURSOR LOCAL FORWARD_ONLY KEYSET FOR
				SELECT BOAID,BOAEmployeeID,IsTemporaryAssign FROM BenefitOnAttendanceEmployee WITH (NOLOCK) WHERE EmployeeID=@PV_EmployeeID AND (InactiveDate IS NULL OR InactiveDate>@Param_AttendanceDate)
				--AND BOAEmployeeID NOT IN (SELECT BOAEmployeeID FROM BenefitOnAttendanceEmployeeStopped WHERE @Param_AttendanceDate
				--BETWEEN StartDate AND CASE WHEN InactiveDate IS NULL THEN EndDate ELSE InactiveDate END)
				OPEN Cur_BOA
				FETCH NEXT FROM Cur_BOA INTO  @PV_BOAID,@PV_BOAEmployeeID,@PV_IsTemporaryAssign
				WHILE(@@Fetch_Status <> -1)		
				BEGIN
					--Check Temporary Stop
					IF EXISTS (SELECT * FROM BenefitOnAttendanceEmployeeStopped WITH (NOLOCK) WHERE BOAEmployeeID=@PV_BOAEmployeeID AND @Param_AttendanceDate BETWEEN StartDate AND EndDate)
					BEGIN
						GOTO CONT_BOA;
					END

					--Check Temporary Assign
					IF @PV_IsTemporaryAssign=1
					BEGIN
						IF NOT EXISTS (SELECT * FROM BenefitOnAttendanceEmployeeAssign WITH (NOLOCK) WHERE BOAEmployeeID=@PV_BOAEmployeeID AND @Param_AttendanceDate BETWEEN StartDate AND EndDate)
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
					 SET @PV_ACSID=ISNULL((SELECT ACSID FROM AttendanceCalendarSession WHERE @Param_AttendanceDate BETWEEN StartDate AND EndDate AND AttendanceCalendarID IN (SELECT AttendanceCalenderID FROm AttendanceScheme WHERE AttendanceSchemeID IN (
										  SELECT AttendanceSchemeID FROM EmployeeOfficial WHERE EmployeeID=@PV_EmployeeID))),0)
					 SET @PV_EmpLeaveLedgerID=0
					 SET @PV_BOA_IsExtraBenefit=0
					 SET @PV_BOA_IsPercent=0
					 SET @PV_BOA_value=0
					 SET @PV_BenefitValue=0
					 SET @PV_BOA_AllowanceOn=0
					 SET @PV_BenefitDurationInMinute=0
					 SET @PV_IsComp = 1
					 SET @PV_BOA_IsOTSlab=0

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
					 FROM [BenefitOnAttendance] WITH (NOLOCK) WHERE BOAID=@PV_BOAID

					 --If any BOA has BOA Valu slab than value slab is more prioritize
					 IF EXISTS (SELECT * FROM BenefitOnAttendanceValueSlab WITH (NOLOCK) WHERE BOAID=@PV_BOAID)
					 BEGIN
						--Get Gross Salary of employee
						DECLARE @PV_Gross as decimal (18,2)=0
						SET @PV_Gross=ISNULL((SELECT ActualGrossAmount FROM EmployeeSalaryStructure WHERE EmployeeID=@PV_EmployeeID),0)
						SET @PV_BOA_value =ISNULL((SELECT top(1)Value FROM BenefitOnAttendanceValueSlab WHERE BOAID=@PV_BOAID AND @PV_Gross BETWEEN MinGross AND MaxGross),0)
					 END

					--Check Leave benefit
					IF @PV_BOA_LeaveHeadID>0 AND @PV_BOA_LeaveAmount>0
					BEGIN						
						IF EXISTS (SELECT * FROM BenefitOnAttendanceEmployeeLedger WITH (NOLOCK) WHERE BOAEmployeeID=@PV_BOAEmployeeID AND AttendanceDate=@Param_AttendanceDate)
						BEGIN
							SET @PV_EmpLeaveLedgerID=ISNULL((SELECT EmpLeaveLedgerID FROM EmployeeLeaveLedger	
													WHERE ACSID =@PV_ACSID AND LeaveID=@PV_BOA_LeaveHeadID	
													AND EmployeeID=@PV_EmployeeID),0)

						UPDATE EmployeeLeaveLedger SET TotalDay=CASE WHEN TotalDay>0 THEN TotalDay-@PV_BOA_LeaveAmount ELSE 0 END
						WHERE EmpLeaveLedgerID=@PV_EmpLeaveLedgerID

							DELETE BenefitOnAttendanceEmployeeLedger WHERE BOAEmployeeID=@PV_BOAEmployeeID AND AttendanceDate=@Param_AttendanceDate
						END
					END


					 IF @PV_BOA_IsContinous=0 AND @Param_AttendanceDate NOT BETWEEN @PV_BOA_BenefitStartDate AND @PV_BOA_BenefitEndDate
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
							IF EXISTS (SELECT * FROM AttendanceCalendarSessionHoliday WITH (NOLOCK) WHERE HolidayID=@PV_BOA_HolidayID AND @Param_AttendanceDate BETWEEN StartDate AND EndDate)
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
												  DATEADD(dd, DATEDIFF(dd, -0, @Param_AttendanceDate), 0)))))
						END
						ELSE BEGIN--Different date
							SET @PV_BOA_StartTime=(DATEADD(SECOND,0,DATEADD (MINUTE,DATEPART(MINUTE,@PV_BOA_StartTime), DATEADD(HOUR, DATEPART(HOUR,@PV_BOA_StartTime), 
												  DATEADD(dd, DATEDIFF(dd, -1, @Param_AttendanceDate), 0)))))
						END

						IF (DATEPART(HOUR,@PV_BOA_EndTime))>(DATEPART(HOUR,@PV_DayStartTime))
						BEGIN--Same date

							SET @PV_BOA_EndTime=(DATEADD(SECOND,0,DATEADD (MINUTE,DATEPART(MINUTE,@PV_BOA_EndTime)
												, DATEADD(HOUR, DATEPART(HOUR,@PV_BOA_EndTime), 
												 DATEADD(dd, DATEDIFF(dd, -0, @Param_AttendanceDate), 0)))))
						END
						ELSE BEGIN----Different date
							SET @PV_BOA_EndTime=(DATEADD(SECOND,0,DATEADD (MINUTE,DATEPART(MINUTE,@PV_BOA_EndTime)
												, DATEADD(HOUR, DATEPART(HOUR,@PV_BOA_EndTime), 
												 DATEADD(dd, DATEDIFF(dd, -1, @Param_AttendanceDate), 0)))))
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
									IF EXISTS (SELECT * FROM AttendanceCalendarSessionHoliday WITH (NOLOCK) WHERE HolidayID=@PV_BOA_HolidayID AND @Param_AttendanceDate BETWEEN StartDate AND EndDate)
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
							IF EXISTS (SELECT * FROM AttendanceCalendarSessionHoliday WITH (NOLOCK) WHERE HolidayID=@PV_BOA_HolidayID AND @Param_AttendanceDate BETWEEN StartDate AND EndDate)
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
							IF EXISTS (SELECT * FROM AttendanceCalendarSessionHoliday WITH (NOLOCK) WHERE HolidayID=@PV_BOA_HolidayID AND @Param_AttendanceDate BETWEEN StartDate AND EndDate)
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
							IF (@PV_BOA_OTInMinute>0 AND @PV_BOA_IsFullOT=0)
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
									WHERE EmployeeID=@PV_EmployeeID AND AttendanceDate=@Param_AttendanceDate
								END
							END--END OT Benefit
							ELSE IF (@PV_BOA_IsFullOT=1)
							BEGIN
								SET @PV_BOA_OTInMinute=0
								SET @PV_BrkTime=0
								SET @PV_BOA_IsOTSlab=0

								IF EXISTS (SELECT * FROM BenefitOnAttendanceOTSlab WITH (NOLOCK) WHERE BOAID=@PV_BOAID AND OTInMin>0)
								BEGIN
									SET @PV_BOA_IsOTSlab=1
								END

								IF @PV_BOA_IsOTSlab=0
								BEGIN--OT Slab
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

										IF (DATEPART(MINUTE,@PV_StartTime)=DATEPART(MINUTE,@PV_ActualEnd)  OR @PV_BaseAddress='mamiya')
										BEGIN
											SET @PV_BOA_OTInMinute=DATEDIFF(MINUTE,CASE WHEN @PV_StartTime>@PV_ActualStart THEN @PV_StartTime ELSE @PV_ActualStart END,@PV_ActualEnd)-ISNULL(@PV_BrkTime,0)+@PV_OTMin--@PV_EarlyInMunit-@PV_LateInMunit
										END ELSE BEGIN
											SET @PV_BOA_OTInMinute=DATEDIFF(MINUTE,CASE WHEN @PV_StartTime>@PV_ActualStart THEN @PV_StartTime ELSE @PV_ActualStart END,@PV_ActualEnd)+1-ISNULL(@PV_BrkTime,0)+@PV_OTMin--@PV_EarlyInMunit-@PV_LateInMunit
										END
									END
								END--OT Slab
								ELSE BEGIN
									IF @PV_TotalWorkingHourInMin>0
									BEGIN
										SET @PV_BOA_OTInMinute=ISNULL((SELECT top(1)OTInMin FROM BenefitOnAttendanceOTSlab WITH (NOLOCK) 
																WHERE BOAID=@PV_BOAID AND @PV_TotalWorkingHourInMin BETWEEN StartMinute AND EndMinute),0)
									END								
								END--OT Slab

								UPDATE AttendanceDaily SET OverTimeInMinute= @PV_BOA_OTInMinute 
								WHERE EmployeeID=@PV_EmployeeID AND AttendanceDate=@Param_AttendanceDate
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
								SET @PV_EmpLeaveLedgerID=ISNULL((SELECT EmpLeaveLedgerID FROM EmployeeLeaveLedger WITH (NOLOCK)
														WHERE ACSID =@PV_ACSID AND LeaveID=@PV_BOA_LeaveHeadID	
														AND EmployeeID=@PV_EmployeeID),0)
													
								IF @PV_EmpLeaveLedgerID>0
								BEGIN
									UPDATE EmployeeLeaveLedger SET TotalDay=TotalDay+@PV_BOA_LeaveAmount
									WHERE EmpLeaveLedgerID=@PV_EmpLeaveLedgerID
								END 
								ELSE BEGIN
									--Asad
									--SET  @PV_IsComp = ISNULL((SELECT IsComp FROM AttendanceSchemeLeave WHERE AttendanceSchemeID=@PV_AttendanceSchemeID AND LeaveID= @PV_BOA_LeaveHeadID),1)
									INSERT INTO [dbo].[EmployeeLeaveLedger]
									([EmpLeaveLedgerID],[EmployeeID],[ACSID],[ASLID],[LeaveID],[DeferredDay],[ActivationAfter],[TotalDay],[IsLeaveOnPresence],[PresencePerLeave],[IsCarryForward],[MaxCarryDays],[DBUSerID],[DBServerDateTime],[IsComp])
									VALUES	((SELECT ISNULL(MAX(EmpLeaveLedgerID),0)+1 FROM EmployeeLeaveLedger)
									,@PV_EmployeeID,@PV_ACSID,0,@PV_BOA_LeaveHeadID,1,1,@PV_BOA_LeaveAmount,0,0,0,0,@Param_DBUserID,GETDATE(), @PV_IsComp)
								END
							END
							
							/*In case of Extra Benifit*/
							IF @PV_BOA_IsExtraBenefit=1 AND @PV_BOA_value>0
							BEGIN
								DECLARE @PV_DaysOfMonth as int
								SET @PV_DaysOfMonth=ISNULL(DAY(EOMONTH(@Param_AttendanceDate)),1)
								IF EXISTS (SELECT * FROM BenefitOnAttendanceTimeSlab WITH (NOLOCK) WHERE BOAID=@PV_BOAID)
								BEGIN
									SET @PV_BOATS_ValueInPercent=ISNULL((SELECT top(1)Value FROM BenefitOnAttendanceTimeSlab WITH (NOLOCK) WHERE BOAID=@PV_BOAID AND @PV_BenefitDurationInMinute BETWEEN StartMinute AND EndMinute),0)
								END ELSE
								BEGIN
									SET @PV_BOATS_ValueInPercent=NULL 
								END

								IF @PV_BOA_AllowanceOn=0 
								BEGIN									
									SET @PV_BenefitValue=@PV_BOA_value
								END ELSE IF @PV_BOA_AllowanceOn=1
								BEGIN
									SET @PV_BenefitValue=ISNULL((SELECT ActualGrossAmount FROM EmployeeSalaryStructure WITH (NOLOCK) WHERE EmployeeID=@PV_EmployeeID),0)/@PV_DaysOfMonth
								END ELSE IF @PV_BOA_AllowanceOn=2
								BEGIN
									SET @PV_BenefitValue=ISNULL((SELECT Max(Amount) FROM EmployeeSalaryStructureDetail WITH (NOLOCK) WHERE ESSID IN (
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
								 VALUES(@PV_BOAELID,@PV_BOAEmployeeID,@Param_AttendanceDate,'N/A',0,NULL,@Param_DBUserID,GETDATE(),@PV_BenefitValue)													

					 END--end benefit
					CONT_BOA:
				FETCH NEXT FROM Cur_BOA INTO @PV_BOAID,@PV_BOAEmployeeID,@PV_IsTemporaryAssign
				END								
				CLOSE Cur_BOA
				DEALLOCATE Cur_BOA
				
		--END--Holiday		
		CONT:
		SET @Param_Index=@Param_Index+1																			      
FETCH NEXT FROM Cur_CCC INTO @PV_EmployeeID,@PV_DesignationID,@PV_EmployeeTypeID,@PV_AttendanceSchemeID
END								
CLOSE Cur_CCC
DEALLOCATE Cur_CCC

IF NOT EXISTS (SELECT * FROM @tbl_EmployeeOfficial WHERE RowID>@Param_Index)
BEGIN
	SET @Param_Index=0;
END	
SELECT @Param_Index;
				      	
END



GO
