USE [ESimSol_ERP]
GO
/****** Object:  StoredProcedure [dbo].[SP_RPT_TimeCard_MaxOTCon]    Script Date: 11/20/2018 12:14:40 PM ******/
DROP PROCEDURE [dbo].[SP_RPT_TimeCard_MaxOTCon]
GO
/****** Object:  StoredProcedure [dbo].[SP_Rpt_IncrementByPercent]    Script Date: 11/20/2018 12:14:40 PM ******/
DROP PROCEDURE [dbo].[SP_Rpt_IncrementByPercent]
GO
/****** Object:  StoredProcedure [dbo].[SP_RPT_AMG_SalarySheet_Comp_AsPerEdit]    Script Date: 11/20/2018 12:14:40 PM ******/
DROP PROCEDURE [dbo].[SP_RPT_AMG_SalarySheet_Comp_AsPerEdit]
GO
/****** Object:  StoredProcedure [dbo].[SP_Process_ComplianceAttendanceProcessAsPerEdit]    Script Date: 11/20/2018 12:14:40 PM ******/
DROP PROCEDURE [dbo].[SP_Process_ComplianceAttendanceProcessAsPerEdit]
GO
/****** Object:  StoredProcedure [dbo].[SP_Process_Comp_Payroll_Corporate_V1]    Script Date: 11/20/2018 12:14:40 PM ******/
DROP PROCEDURE [dbo].[SP_Process_Comp_Payroll_Corporate_V1]
GO
/****** Object:  StoredProcedure [dbo].[SP_Process_Comp_Payroll_Corporate_V1]    Script Date: 11/20/2018 12:14:40 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO




CREATE PROCEDURE [dbo].[SP_Process_Comp_Payroll_Corporate_V1] 
@Param_Index as int--for pagination top 300. 
,@Param_PayRollProcessID as int
,@Param_EmployeeIDs as varchar (max)
,@Param_DBUserID as int

--SET @Param_Index=0
--SET @Param_PayRollProcessID=1
--SET @Param_DBUserID=-9
AS
BEGIN 
--Common Validation
IF (@Param_PayRollProcessID<=0)
BEGIN
	ROLLBACK
		RAISERROR(N'Invalid Operation. Please Contact with vendor',16,1);
	RETURN
END

IF NOT EXISTS (SELECT * FROM CompliancePayrollProcessManagement WHERE PPMID=@Param_PayRollProcessID)
BEGIN
	ROLLBACK
		RAISERROR(N'Invalid Process.Please Contact with vendor!!!',16,1)
	RETURN
END
IF EXISTS (SELECT * FROM CompliancePayrollProcessManagement WHERE [Status]=4 AND PPMID=@Param_PayRollProcessID)
BEGIN
	ROLLBACK
		RAISERROR(N'Already freezed.!!!',16,1)
	RETURN
END
--Declare Local variable

--for Department, Salary Scheme & SalaryHead
DECLARE @tbl_Department AS TABLE (DepartmentID int)
DECLARE @tbl_SalaryScheme AS TABLE (SalarySchemeID int)
DECLARE @tbl_SalaryHead AS TABLE (SalaryHeadID int)

DECLARE @tbl_Employee AS TABLE (ESSID int,EmployeeID int,ActualGrossAmount decimal(30,17),CurrencyID int,SalarySchemeID int,CompGrossAmount decimal(18,2),IsCashFixed bit, CashAmount decimal(18,2))
--for update Production rate at EmployeeProduction
DECLARE @tbl_PRate AS TABLE (EPSRDID int, ProductionRate decimal(30,17))

DECLARE
--for payroll Process
@PV_PPM_LocationID as int
,@PV_PPM_StartDate as date
,@PV_PPM_EndDate as date
,@PV_PPM_Month as int

--for salary scheme
,@PV_SS_SalarySchemeID as int
,@PV_SS_IsAllowOverTime as bit
,@PV_SS_OverTimeOn as smallint
,@PV_SS_DevidedBy as decimal(18,2)
,@PV_SS_MultiplicationBy as decimal(18,2)
,@PV_SS_IsAttendanceDependant as bit
,@PV_SS_LateCount as int
,@PV_SS_EarlyLeavingCount as int
,@PV_SS_IsProductionBase as bit
,@PV_SS_StartDay as int
,@PV_SS_OverTimeOn_Com as smallint
,@PV_SS_DevidedBy_Com as decimal(18,2)
,@PV_SS_MultiplicationBy_Com as decimal(18,2)

--for production bonus
,@PV_PB_IsPercent bit
,@PV_PB_Value as decimal(30,17)

,@PV_EmployeeID as int
,@PV_ESSID as int
,@PV_ActualGrossAmount as decimal (30,17)
,@PV_CurrencyID as int

,@PV_TotalDaysOfAbsent as int
,@PV_DayForLate as int
,@PV_DayForEarly as int
,@PV_OneDayBasic as decimal(18,2)
,@PV_Addition as decimal(18,2)
,@PV_Deduction as decimal(18,2)
,@PV_DisciplinaryActionDeduction as decimal(18,2)
,@PV_DisciplinaryActionAddition as decimal(18,2)
,@PV_NetAmount as decimal(18,2)

,@PV_DepartmentID as int
,@PV_DesignationID as int
,@PV_EmployeeSalaryID as int

,@PV_ProductionAmount as decimal(30,17)
,@PB_ProductionBonus as decimal(30,17)

,@PV_TotalWorkingDay as int

,@PV_AbsentAmount as decimal(18,2)
,@PV_TotalLate as int
,@PV_TotalEarlyLeaving as int
,@PV_TotalDayOff as int
,@PV_TotalUPLeave as int
,@PV_TotalPLeave as int
,@PV_OTHour as decimal(18,2)
,@PV_OTRate as decimal(18,2)
,@PV_ReveniewStemp as decimal(30,17)
,@PV_Basic as decimal(30,17)
,@PV_OTValue as decimal(30,17)
,@PV_DayOffCountForOT as int
,@PV_OverTimeDayCount as int
,@PV_sDayOffDay as varchar(50)
,@PV_AttBonus as decimal(30,17)
,@PV_LeaveAllowance as decimal(30,17)
,@PV_TotalPresentDay as int
,@PV_DOJ as date
,@PV_DeductionForNewJoining as decimal(30,17)
,@PV_StartDate as Date
,@PV_EndDate as Date
,@PV_TotalNoWorkDay as int
,@PV_TotalNoWorkDayAllowance as decimal(18,2)
,@PV_AddShortFall as decimal(18,2)
,@PV_DayForAddShortFall as int
,@PV_AttStartDate as Date
,@PV_AttEndDate as Date
,@PV_TotalDayForAttBonus as int
,@PV_IsSalary as bit
,@PV_IsActive as bit
,@PV_RestOTDayCount as int
,@PV_BOA_TotalAmount as decimal(30,17)
,@PV_TotalHoliday as int
,@PV_WithOutBasicAmount as decimal(30,17)
,@PV_PFAmount as decimal(18,2)
,@PV_PFSalaryHeadID as int
,@PV_PFValueInPercent as decimal(18,2)
,@PV_PFCalculationON as smallint
,@PV_LWPCount as int				
,@PV_PIAmount as decimal(18,2)
,@PV_PISalaryHeadID as int
,@PV_LateAmount as decimal(18,2)
,@PV_EarlyLeaveAmount as decimal(18,2)
,@PV_SS_FixedLateAmount as decimal (18,2)
,@PV_SS_FixedEarlyAmount as decimal (18,2)
,@PV_OneDayGross as decimal(18,2)
,@PV_LWPAmount as decimal (18,2)
,@PV_TotalMonthDay as int
,@PV_DeductionForResign as decimal(18,2)
,@PV_ResignDay as int
,@PV_Loan as decimal(18,2)
,@PV_Loan_SalaryHeadID as int

,@PV_OTHour_Com as Decimal(18,2)
,@PV_OTValue_Com As decimal(18,2)
,@PV_OTRate_Com As decimal(18,2)
,@PV_NetAmount_Com AS Decimal(18,2)

,@PV_EarlyLeaveInMinute as int
,@PV_EducationFundAmt as decimal (18,2)
,@PV_PPM_MonthDays as int
,@PV_LateHourInMin AS int

,@PV_BankAmount as decimal(18,2)
,@PV_CashAmount AS decimal (18,2)
,@PV_CompBankAmount as decimal(18,2)
,@PV_CompCashAmount AS decimal (18,2)
,@PV_EmployeeBankACID as int
,@PV_IsCashFixed as bit

--Compliance
,@PV_CompGrossAmount as decimal(18,2)
,@PV_CompBasic As decimal(18,2)
,@PV_CompWithOutBasicAmount As decimal(18,2)
,@PV_CompOneDayBasic As decimal(18,2)
,@PV_CompOneDayGross As decimal(18,2)
,@PV_CompAddition As decimal(18,2)
,@PV_CompDeduction As decimal(18,2)
,@PV_CompDeductionForNewJoining As decimal(18,2)
,@PV_CompDeductionForResign As decimal(18,2)
,@PV_CompAbsentAmount As decimal(18,2)
,@PV_CompLateAmount As decimal(18,2)
,@PV_CompEarlyLeaveAmount As decimal(18,2)
,@PV_CompDisciplinaryActionAddition as decimal(18,2)
,@PV_CompDisciplinaryActionDeduction as decimal(18,2)

,@PV_CompTotalDayOff as int
,@PV_CompTotalHoliday as int
,@PV_CompTotalLeave as int
,@PV_CompTotalLate as int
,@PV_CompTotalEarlyLeave as int
,@PV_CompLateInMin as int
,@PV_CompTotalDayForAttBonus as int
,@PV_CompTotalWorkingDay as int
,@PV_CompTotalPresentDay as int
,@PV_CompTotalDaysOfAbsent as int
,@PV_CompEarlyLeaveInMinute as int
,@PV_CompAttBonus as Decimal(18,2)
,@PV_CompDayForLate as int
,@PV_CompLWPAmount as decimal(18,2)

,@PV_MOCID as int


,@PV_BaseAddress as varchar (100)



--Set reveniew stamp hard coded
SET @PV_ReveniewStemp=0--Hard coded
SET @PV_BaseAddress=(SELECT top(1)BaseAddress FROM Company)


--Get MOCID from payroll process management
SET @PV_MOCID = ISNULL((SELECT MOCID FROM CompliancePayrollProcessManagement WHERE PPMID=@Param_PayRollProcessID), 0)

--Get & Set basic information from Payroll process management
SELECT @PV_PPM_LocationID=LocationID,@PV_PPM_StartDate=SalaryFrom,@PV_PPM_EndDate=SalaryTo, @PV_PPM_Month=MonthID 
FROM CompliancePayrollProcessManagement WHERE PPMID=@Param_PayRollProcessID


--Insert Department those are attached with CompliancePayrollProcessManagement
INSERT INTO @tbl_Department
SELECT ObjectID FROM CompliancePayrollProcessManagementObject 
WHERE PPMID=@Param_PayRollProcessID AND PPMObject=1/*DepartmentID*/

--Insert SalaryScheme those are attached with CompliancePayrollProcessManagement
INSERT INTO @tbl_SalaryScheme
SELECT ObjectID FROM CompliancePayrollProcessManagementObject 
WHERE PPMID=@Param_PayRollProcessID AND PPMObject=2/*SalaryScheme*/

--Insert SalaryHead(Process Dependent) those are attached with CompliancePayrollProcessManagement
INSERT INTO @tbl_SalaryHead
SELECT ObjectID FROM CompliancePayrollProcessManagementObject 
WHERE PPMID=@Param_PayRollProcessID AND PPMObject=3/*SalaryHead*/

DECLARE @tbl_EmployeeSalaryStructureDetail AS TABLE (SalaryHeadID int,Amount decimal(30,17),SalaryHeadType smallint)

--Set MonthDAys
SET @PV_PPM_MonthDays=DATEDIFF(DAY,@PV_PPM_StartDate,@PV_PPM_EndDate)+1

--Insert active running empoyee by conditions 
IF ISNULL(@Param_EmployeeIDs,'')<>''
BEGIN
	INSERT INTO @tbl_Employee
	SELECT top(5)aa.ESSID,aa.EmployeeID,aa.ActualGrossAmount,aa.CurrencyID,aa.SalarySchemeID,aa.CompGrossAmount,aa.IsCashFixed,aa.CashAmount FROM (
	SELECT ROW_NUMBER() OVER(ORDER BY ESSID) AS Row,ESSID,EmployeeID,ActualGrossAmount,CurrencyID,SalarySchemeID,ISNULL(CompGrossAmount ,0) AS CompGrossAmount,ISNULL(IsCashFixed,0) AS IsCashFixed ,ISNULL(CashAmount,0) AS CashAmount
	FROM EmployeeSalaryStructure WITH(NOLOCK) WHERE IsActive=1 AND StartDay=DAY(@PV_PPM_StartDate) AND SalarySchemeID IN (
	SELECT SalarySchemeID FROM @tbl_SalaryScheme) AND EmployeeID IN (
	SELECT EmployeeID FROM View_EmployeeOfficial WHERE WorkingStatus IN (1,2,6)/*InWorkingPlace,OSD*/
	AND IsActive=1 AND DateOfJoin<=@PV_PPM_EndDate AND EmployeeID IN (SELECT items FROm dbo.SplitInToDataSet(@Param_EmployeeIDs,','))
	AND DRPID IN (
	SELECT DepartmentRequirementPolicyID FROM DepartmentRequirementPolicy WHERE LocationId=@PV_PPM_LocationID 
	AND DepartmentID IN (SELECT DepartmentID FROM @tbl_Department))))aa WHERE aa.Row>@Param_Index
END ELSE BEGIN
	INSERT INTO @tbl_Employee
	SELECT top(5)aa.ESSID,aa.EmployeeID,aa.ActualGrossAmount,aa.CurrencyID,aa.SalarySchemeID,aa.CompGrossAmount,aa.IsCashFixed,aa.CashAmount FROM (
	SELECT ROW_NUMBER() OVER(ORDER BY ESSID) AS Row,ESSID,EmployeeID,ActualGrossAmount,CurrencyID,SalarySchemeID,ISNULL(CompGrossAmount ,0) AS CompGrossAmount,ISNULL(IsCashFixed,0) AS IsCashFixed ,ISNULL(CashAmount,0) AS CashAmount
	FROM EmployeeSalaryStructure WITH(NOLOCK) WHERE IsActive=1 AND StartDay=DAY(@PV_PPM_StartDate) AND SalarySchemeID IN (
	SELECT SalarySchemeID FROM @tbl_SalaryScheme) AND EmployeeID IN (
	SELECT EmployeeID FROM View_EmployeeOfficial WHERE WorkingStatus IN (1,2,6)/*InWorkingPlace,OSD*/
	AND IsActive=1 AND DateOfJoin<=@PV_PPM_EndDate 
	AND DRPID IN (
	SELECT DepartmentRequirementPolicyID FROM DepartmentRequirementPolicy WHERE LocationId=@PV_PPM_LocationID 
	AND DepartmentID IN (SELECT DepartmentID FROM @tbl_Department))))aa WHERE aa.Row>@Param_Index
END

BEGIN TRY
	DECLARE Cur_CC CURSOR LOCAL FORWARD_ONLY KEYSET FOR
	SELECT ESSID,EmployeeID,ActualGrossAmount,CurrencyID,SalarySchemeID,CompGrossAmount,IsCashFixed,CashAmount FROM @tbl_Employee	
	OPEN Cur_CC
	FETCH NEXT FROM Cur_CC INTO  @PV_ESSID,@PV_EmployeeID,@PV_ActualGrossAmount,@PV_CurrencyID,@PV_SS_SalarySchemeID,@PV_CompGrossAmount,@PV_IsCashFixed,@PV_CashAmount
	WHILE(@@Fetch_Status <> -1)
	BEGIN--	
			IF NOT EXISTS (SELECT EmployeeID FROM ComplianceEmployeeSalary WITH(NOLOCK) WHERE PayrollProcessID=@Param_PayrollProcessID AND EmployeeID=@PV_EmployeeID AND StartDate=@PV_PPM_StartDate AND EndDate=@PV_PPM_EndDate)
			BEGIN

				IF EXISTS (SELECT * FROM ComplianceEmployeeSalary WITH(NOLOCK) WHERE StartDate BETWEEN @PV_PPM_StartDate AND @PV_PPM_EndDate AND EmployeeID=@PV_EmployeeID)
				BEGIN
					GOTO CONT
				END

				IF NOT EXISTS (SELECT * FROM EmployeeSalaryStructureDetail WITH(NOLOCK) WHERE ESSID=@PV_ESSID)
				BEGIN
					GOTO CONT
				END

							
				SET @PV_StartDate=@PV_PPM_StartDate
				SET @PV_EndDate=@PV_PPM_EndDate
				
				--Get Salary Scheme record
				SELECT @PV_SS_IsAllowOverTime=IsAllowOverTime
				,@PV_SS_OverTimeOn=OverTimeON
				,@PV_SS_DevidedBy=DividedBy
				,@PV_SS_MultiplicationBy=MultiplicationBy
				,@PV_SS_OverTimeOn_Com=CompOverTimeON
				,@PV_SS_DevidedBy_Com=CompDividedBy
				,@PV_SS_MultiplicationBy_Com=CompMultiplicationBy

				,@PV_SS_IsAttendanceDependant=IsAttendanceDependant
				,@PV_SS_LateCount=LateCount
				,@PV_SS_EarlyLeavingCount=EarlyLeavingCount
				,@PV_SS_IsProductionBase=IsProductionBase
				,@PV_SS_FixedLateAmount=FixedLatePenalty
				,@PV_SS_FixedEarlyAmount=FixedEarlyLeavePenalty				
				FROM SalaryScheme WITH(NOLOCK) WHERE SalarySchemeID=@PV_SS_SalarySchemeID

				--Get Department, designation, DAte of join of an employee
				SELECT @PV_DepartmentID=DepartmentID,@PV_DesignationID=DesignationID,@PV_DOJ=DateOfJoin 
				FROM View_EmployeeOfficial WITH(NOLOCK) WHERE EmployeeID=@PV_EmployeeID
			
				--Initialize variable
				SET @PV_ProductionAmount=0
				SET @PB_ProductionBonus=0
				SET @PV_OTHour=0
				SET @PV_OTRate=0
				SET @PV_TotalDaysOfAbsent=0
				SET @PV_DayForLate=0
				SET @PV_DayForEarly=0
				SET @PV_AbsentAmount=0
				SET @PV_LeaveAllowance=0
				SET @PV_AttBonus=0
				SET @PV_TotalPLeave=0
				SET @PV_TotalUPLeave=0
				SET @PV_DeductionForNewJoining=0;
				SET @PV_DisciplinaryActionDeduction=0;
				SET @PV_DisciplinaryActionAddition=0
				SET @PV_Addition=0;
				SET @PV_Deduction=0;
				SET @PV_AddShortFall=0;
				SET @PV_TotalNoWorkDayAllowance=0;
				SET @PV_NetAmount=0;
				SET @PV_DayForAddShortFall=0;
				SET @PV_RestOTDayCount=0;
				SET @PV_PB_IsPercent=0
				SET @PV_PB_Value=0
				SET @PV_OTValue=0
				SET @PV_BOA_TotalAmount=0
				SET @PV_WithOutBasicAmount=0
				SET @PV_TotalHoliday=0
				SET @PV_TotalDayOff=0
				SET @PV_TotalNoWorkDay=0
				SET @PV_PFAmount=0
				SET @PV_LWPCount=0
				SET @PV_PFSalaryHeadID=0
				SET @PV_PFValueInPercent=0
				SET @PV_PFCalculationON=0
				SET @PV_PIAmount=0
				SET @PV_PISalaryHeadID=0
				SET @PV_LateAmount=0
				SET @PV_EarlyLeaveAmount=0
				SET @PV_OneDayGross=0
				SET @PV_LWPAmount=0
				SET @PV_DeductionForResign=0
				SET @PV_ResignDay=0
				SET @PV_Loan=0
				SET @PV_Loan_SalaryHeadID=0
				SET @PV_NetAmount_Com=0
				SET @PV_OTHour_Com=0
				SET @PV_OTRate_Com=0
				SET @PV_OTValue_Com=0
				SET @PV_EarlyLeaveInMinute=0
				SET @PV_EducationFundAmt=0
				SET @PV_LateHourInMin=0

				SET @PV_BankAmount =0

				--SET @PV_CashAmount=0
				SET @PV_EmployeeBankACID=0
			
				--Get Basic of an employee: Basic amount must be maximum within all heads
				SET @PV_Basic=(SELECT MAX(Amount) FROM VIEW_EmployeeSalaryStructureDetail WITH(NOLOCK) 
									WHERE ESSID=@PV_ESSID AND SalaryHeadType=1)	/*Basic Type*/

				SET @PV_CompBasic=(SELECT MAX(ISNULL(CompAmount,0)) FROM VIEW_EmployeeSalaryStructureDetail WITH(NOLOCK) 
									WHERE ESSID=@PV_ESSID AND SalaryHeadType=1)	/*Basic Type*/										
									
				SET @PV_WithOutBasicAmount=(SELECT ISNULL(SUM(Amount),0) FROM VIEW_EmployeeSalaryStructureDetail WITH(NOLOCK)
									WHERE ESSID=@PV_ESSID AND SalaryHeadType=1)-@PV_Basic	/*Basic Type*/	
									
				--SET @PV_CompWithOutBasicAmount=(SELECT ISNULL(SUM(CompAmount),0) FROM VIEW_EmployeeSalaryStructureDetail WITH(NOLOCK)
				--					WHERE ESSID=@PV_ESSID AND SalaryHeadType=1)-@PV_Basic	/*Basic Type*/																								
											
				IF (@PV_BaseAddress IN ('A007','akcl','golden','td'))
				BEGIN
					--As per Mr. Robiul requirement monthcycle day calculate 30 for each month
					SET @PV_OneDayBasic=@PV_Basic/30
					SET @PV_OneDayGross=@PV_ActualGrossAmount/30
				END ELSE BEGIN
					SET @PV_OneDayBasic=@PV_Basic/(SELECT(DATEDIFF(dd, @PV_StartDate, @PV_EndDate)+1))
					SET @PV_OneDayGross=@PV_ActualGrossAmount/(SELECT(DATEDIFF(dd, @PV_StartDate, @PV_EndDate)+1))
				END

				--IF (@PV_BaseAddress IN ('A007','akcl','golden','td','mithela'))
				--BEGIN
				--	SET @PV_CompOneDayBasic=@PV_CompBasic/30
				--	SET @PV_CompOneDayGross=@PV_CompGrossAmount/30
				--END ELSE BEGIN
				--	SET @PV_CompOneDayBasic=@PV_CompBasic/(SELECT(DATEDIFF(dd, @PV_StartDate, @PV_EndDate)+1))
				--	SET @PV_CompOneDayGross=@PV_CompGrossAmount/(SELECT(DATEDIFF(dd, @PV_StartDate, @PV_EndDate)+1))
				--END

				--DELETE FROM @tbl_EmployeeSalaryStructureDetail
				--IF (@PV_PPM_MonthDays NOT IN (30,31))
				--BEGIN
				--	SET @PV_Basic=@PV_OneDayBasic*@PV_PPM_MonthDays
				--	SET @PV_WithOutBasicAmount=
				--END

				IF @PV_SS_IsAttendanceDependant=1
				BEGIN
					IF EXISTS (SELECT * FROM MaxOTConfigurationAttendance WITH(NOLOCK) WHERE EmployeeID=@PV_EmployeeID AND AttendanceDate BETWEEN @PV_StartDate AND @PV_EndDate)
					BEGIN
						SET @PV_StartDate=(SELECT top(1)AttendanceDate FROM MaxOTConfigurationAttendance WITH(NOLOCK) WHERE EmployeeID=@PV_EmployeeID AND AttendanceDate BETWEEN @PV_StartDate AND @PV_EndDate
										Order By AttendanceDate ASC)		
										
						SET @PV_EndDate=(SELECT top(1)AttendanceDate FROM MaxOTConfigurationAttendance WITH(NOLOCK) WHERE EmployeeID=@PV_EmployeeID AND AttendanceDate BETWEEN @PV_StartDate AND @PV_EndDate
										Order By AttendanceDate DESC)													
					END ELSE
					BEGIN 
						--SET @PV_StartDate=DATEADD(d, 1, @PV_EndDate)
						GOTO CONT
					END
				END																																
			
				--get actual Attendance start & end date
				IF @PV_EndDate>=@PV_StartDate
				BEGIN
					SET @PV_AttStartDate=(SELECT top(1)AttendanceDate FROM MaxOTConfigurationAttendance WITH(NOLOCK) WHERE EmployeeID=@PV_EmployeeID AND AttendanceDate BETWEEN @PV_StartDate AND @PV_EndDate AND (CAST(InTime AS TIME(0))!='00:00:00' OR CAST(OutTime AS TIME(0))!='00:00:00')
										Order By AttendanceDate ASC)
					--SET @PV_AttEndDate=(SELECT top(1)AttendanceDate FROM AttendanceDaily WHERE EmployeeID=@PV_EmployeeID AND AttendanceDate BETWEEN @PV_StartDate AND @PV_EndDate AND (CAST(InTime AS TIME(0))!='00:00:00' OR CAST(OutTime AS TIME(0))!='00:00:00')
					--					Order By AttendanceDate DESC)

					SET @PV_AttEndDate=(SELECT top(1)AttendanceDate FROM MaxOTConfigurationAttendance WITH(NOLOCK) WHERE EmployeeID=@PV_EmployeeID AND AttendanceDate BETWEEN @PV_StartDate AND @PV_EndDate
										Order By AttendanceDate DESC)
				END

				IF @PV_AttEndDate<@PV_EndDate
				BEGIN
					SET @PV_ResignDay=DATEDIFF(DAY,@PV_AttEndDate,@PV_PPM_EndDate)
				END
	
			
				/*******Attendance related Information*******/
				--Actual										
				SET @PV_TotalDayOff=(SELECT COUNT(MOCAID) FROM MaxOTConfigurationAttendance WITH(NOLOCK) WHERE MOCID = @PV_MOCID AND EmployeeID=@PV_EmployeeID AND IsDayOff=1
				AND AttendanceDate BETWEEN 	@PV_StartDate AND @PV_EndDate)

				SET @PV_TotalHoliday=(SELECT COUNT(MOCAID) FROM MaxOTConfigurationAttendance WITH(NOLOCK) WHERE MOCID = @PV_MOCID AND EmployeeID=@PV_EmployeeID AND IsHoliday=1 AND IsDayOff=0
				AND AttendanceDate BETWEEN 	@PV_StartDate AND @PV_EndDate)
			
				SET @PV_TotalWorkingDay=(SELECT(DATEDIFF(dd, @PV_StartDate, @PV_EndDate)+1)-@PV_TotalDayOff-@PV_TotalHoliday)
				SET @PV_TotalDayForAttBonus=(SELECT(DATEDIFF(dd, @PV_PPM_StartDate, @PV_PPM_EndDate)+1)-@PV_TotalDayOff-@PV_TotalHoliday)
			
				SET @PV_TotalPLeave=(SELECT COUNT(MOCAID)FROM MaxOTConfigurationAttendance WITH(NOLOCK)
										WHERE MOCID = @PV_MOCID AND EmployeeID=@PV_EmployeeID AND IsLeave=1 AND /*IsUnPaid=0 --AND IsLock=1*/ EmployeeID IN(SELECT EmployeeID FROM AttendanceDaily WHERE EmployeeID=@PV_EmployeeID
										AND AttendanceDate BETWEEN @PV_StartDate AND @PV_EndDate AND IsUnPaid=0)
										AND AttendanceDate BETWEEN @PV_StartDate AND @PV_EndDate)	
										
				SET @PV_TotalUPLeave=(SELECT COUNT(MOCAID)FROM MaxOTConfigurationAttendance WITH(NOLOCK)
										WHERE MOCID = @PV_MOCID AND EmployeeID=@PV_EmployeeID AND IsLeave=1 AND /*IsUnPaid=1 --AND IsLock=1 */EmployeeID IN(SELECT EmployeeID FROM AttendanceDaily WHERE EmployeeID=@PV_EmployeeID
										AND AttendanceDate BETWEEN @PV_StartDate AND @PV_EndDate AND IsUnPaid=1)
										AND AttendanceDate BETWEEN @PV_StartDate AND @PV_EndDate)	
			
				SET @PV_TotalPresentDay=(SELECT COUNT(MOCAID) FROM MaxOTConfigurationAttendance WITH(NOLOCK) WHERE MOCID = @PV_MOCID AND EmployeeID=@PV_EmployeeID 
										AND IsDayOff=0 AND IsLeave=0 AND IsHoliday=0
										AND (CAST(InTime AS TIME(0))!='00:00:00'
										OR CAST(OutTime AS TIME(0))!='00:00:00' OR /*IsOSD =1*/
										EmployeeID IN(SELECT EmployeeID FROM AttendanceDaily WHERE EmployeeID=@PV_EmployeeID
										AND AttendanceDate BETWEEN @PV_StartDate AND @PV_EndDate AND IsUnPaid=0))
										AND AttendanceDate BETWEEN @PV_StartDate AND @PV_EndDate)	
												
				SET @PV_TotalDaysOfAbsent=@PV_TotalWorkingDay-@PV_TotalPresentDay-@PV_TotalPLeave-@PV_TotalUPLeave-@PV_ResignDay;
				
				IF (@PV_SS_IsProductionBase=0 AND @PV_TotalPresentDay+@PV_TotalPLeave+@PV_TotalUPLeave<=0)BEGIN GOTO CONT END
									
								
			
				--SET @PV_TotalLate=ISNULL((SELECT COUNT(LateArrivalMinute) FROM MaxOTConfigurationAttendance WITH(NOLOCK)
				--						WHERE MOCID = @PV_MOCID AND EmployeeID=@PV_EmployeeID AND IsDayOff=0 AND IsHoliday=0 
				--						AND AttendanceDate BETWEEN @PV_StartDate AND @PV_EndDate
				--						AND LateArrivalMinute>0),0)

			 --   SET @PV_LateHourInMin=ISNULL((SELECT SUM(LateArrivalMinute) FROM MaxOTConfigurationAttendance WITH(NOLOCK)
				--						WHERE MOCID = @PV_MOCID AND EmployeeID=@PV_EmployeeID AND IsDayOff=0 AND IsHoliday=0 
				--						AND AttendanceDate BETWEEN @PV_StartDate AND @PV_EndDate),0)
			
				--SET @PV_TotalEarlyLeaving=ISNULL((SELECT COUNT(EarlyDepartureMinute)FROM MaxOTConfigurationAttendance WITH(NOLOCK)
				--						WHERE MOCID = @PV_MOCID AND EmployeeID=@PV_EmployeeID AND IsDayOff=0 AND IsHoliday=0 
				--						AND AttendanceDate BETWEEN @PV_StartDate AND @PV_EndDate),0)
										
				--SET @PV_EarlyLeaveInMinute=ISNULL((SELECT SUM(EarlyDepartureMinute)FROM MaxOTConfigurationAttendance WITH(NOLOCK)
				--						WHERE MOCID = @PV_MOCID AND EmployeeID=@PV_EmployeeID AND IsDayOff=0 AND IsHoliday=0 
				--						AND AttendanceDate BETWEEN @PV_StartDate AND @PV_EndDate),0)	
										
				----Compliance
				--SET @PV_CompTotalDayOff=(SELECT COUNT(AttendanceID) FROM AttendanceDaily WITH(NOLOCK) WHERE  EmployeeID=@PV_EmployeeID AND IsCompDayOff=1
				--AND AttendanceDate BETWEEN 	@PV_StartDate AND @PV_EndDate)

				--SET @PV_CompTotalHoliday=(SELECT COUNT(AttendanceID) FROM AttendanceDaily WITH(NOLOCK) WHERE  EmployeeID=@PV_EmployeeID AND IsCompHoliday=1 AND IsCompDayOff=0
				--AND AttendanceDate BETWEEN 	@PV_StartDate AND @PV_EndDate)
			
				--SET @PV_CompTotalWorkingDay=(SELECT(DATEDIFF(dd, @PV_StartDate, @PV_EndDate)+1)-@PV_CompTotalDayOff-@PV_CompTotalHoliday)
				--SET @PV_CompTotalDayForAttBonus=(SELECT(DATEDIFF(dd, @PV_PPM_StartDate, @PV_PPM_EndDate)+1)-@PV_CompTotalDayOff-@PV_CompTotalHoliday)
			
				--SET @PV_CompTotalLeave=(SELECT COUNT(AttendanceID)FROM AttendanceDaily WITH(NOLOCK)
				--						WHERE EmployeeID=@PV_EmployeeID AND IsCompLeave=1
				--						AND AttendanceDate BETWEEN @PV_StartDate AND @PV_EndDate)	
			
				--SET @PV_CompTotalPresentDay=(SELECT COUNT(AttendanceID) FROM AttendanceDaily WITH(NOLOCK) WHERE EmployeeID=@PV_EmployeeID 
				--						AND IsCompDayOff=0 AND IsCompLeave=0 AND IsCompHoliday=0
				--						AND (CAST(CompInTime AS TIME(0))!='00:00:00'
				--						OR CAST(CompOutTime AS TIME(0))!='00:00:00' OR IsOSD =1)-- Modified by AZHAR
				--						AND AttendanceDate BETWEEN @PV_StartDate AND @PV_EndDate)	
												
				--SET @PV_CompTotalDaysOfAbsent=@PV_CompTotalWorkingDay-@PV_CompTotalPresentDay-@PV_CompTotalLeave--@PV_ResignDay;
								
			
				--SET @PV_CompTotalLate=ISNULL((SELECT COUNT(CompLateArrivalMinute) FROM AttendanceDaily WITH(NOLOCK) 
				--						WHERE EmployeeID=@PV_EmployeeID AND IsCompDayOff=0 AND IsCompHoliday=0 --AND IsLock=1 
				--						AND AttendanceDate BETWEEN @PV_StartDate AND @PV_EndDate
				--						AND CompLateArrivalMinute>0),0)

			 --  SET @PV_CompLateInMin=ISNULL((SELECT SUM(CompLateArrivalMinute) FROM AttendanceDaily WITH(NOLOCK)
				--						WHERE EmployeeID=@PV_EmployeeID AND IsCompDayOff=0 AND IsCompHoliday=0 --AND IsLock=1 
				--						AND AttendanceDate BETWEEN @PV_StartDate AND @PV_EndDate
				--						AND CompLateArrivalMinute>0),0)
			
				--SET @PV_CompTotalEarlyLeave=ISNULL((SELECT COUNT(CompEarlyDepartureMinute)FROM AttendanceDaily WITH(NOLOCK)
				--						WHERE EmployeeID=@PV_EmployeeID AND IsCompDayOff=0 AND IsCompHoliday=0 --AND IsLock=1 
				--						AND AttendanceDate BETWEEN @PV_StartDate AND @PV_EndDate
				--						AND CompEarlyDepartureMinute>0),0)
										
				--SET @PV_CompEarlyLeaveInMinute=ISNULL((SELECT SUM(CompEarlyDepartureMinute)FROM AttendanceDaily WITH(NOLOCK)
				--						WHERE EmployeeID=@PV_EmployeeID AND IsCompDayOff=0 AND IsCompHoliday=0 --AND IsLock=1 
				--						AND AttendanceDate BETWEEN @PV_StartDate AND @PV_EndDate
				--						AND CompEarlyDepartureMinute>0),0)	
																																																														
				
				/**********Without any Condition Addition Deduction Reimbursement & DisciplinaryAction summation */
				IF @PV_EndDate>=@PV_StartDate
				BEGIN
					SET @PV_Addition=(SELECT ISNULL(SUM(Amount),0) FROM VIEW_EmployeeSalaryStructureDetail WITH(NOLOCK)
									  WHERE ESSID=@PV_ESSID AND SalaryHeadType IN (2,4)/*Addition & Reimbursement*/
									  AND Condition=0 AND IsProcessDependent=0/*Without Any Condition Addition*/ 
									  AND (SalaryType IN (0,1,2) OR SalaryType IS NULL))

					SET @PV_Deduction=(SELECT ISNULL(SUM(Amount),0) FROM VIEW_EmployeeSalaryStructureDetail WITH(NOLOCK)
									  WHERE ESSID=@PV_ESSID AND SalaryHeadType=3 /*Deduction*/
									  AND Condition=0 AND IsProcessDependent=0 /*Without Any Condition Addition*/ 
									  AND (SalaryType IN (0,1,2) OR SalaryType IS NULL))
								  
					SET @PV_DisciplinaryActionAddition=(SELECT ISNULL(SUM(Amount),0) FROM View_DisciplinaryAction WITH(NOLOCK) WHERE ApproveBy>0 AND SalaryHeadType IN(2,4)/*Addition & Reimbursement*/ 
														AND EmployeeID=@PV_EmployeeID AND ExecutedOn BETWEEN @PV_PPM_StartDate AND @PV_PPM_EndDate )						  

					SET @PV_DisciplinaryActionDeduction=(SELECT ISNULL(SUM(Amount),0) FROM View_DisciplinaryAction WITH(NOLOCK) WHERE ApproveBy>0 AND SalaryHeadType=3/*Deduction*/ 
											AND EmployeeID=@PV_EmployeeID AND ExecutedOn BETWEEN @PV_PPM_StartDate AND @PV_PPM_EndDate)			
											
					---Compliance
					SET @PV_CompAddition=(SELECT ISNULL(SUM(CompAmount),0) FROM VIEW_EmployeeSalaryStructureDetail WITH(NOLOCK)
									  WHERE ESSID=@PV_ESSID AND SalaryHeadType IN (2,4)/*Addition & Reimbursement*/
									  AND Condition=0 AND IsProcessDependent=0/*Without Any Condition Addition*/ 
									  AND (SalaryType IN (0,1,3) OR SalaryType IS NULL))


					SET @PV_CompDeduction=(SELECT ISNULL(SUM(CompAmount),0) FROM VIEW_EmployeeSalaryStructureDetail WITH(NOLOCK)
									  WHERE ESSID=@PV_ESSID AND SalaryHeadType=3 /*Deduction*/
									  AND Condition=0 AND IsProcessDependent=0 /*Without Any Condition Addition*/ 
									  AND (SalaryType IN (0,1,3) OR SalaryType IS NULL))

					SET @PV_CompDisciplinaryActionAddition=(SELECT ISNULL(SUM(CompAmount),0) FROM View_DisciplinaryAction WITH(NOLOCK) WHERE ApproveBy>0 AND SalaryHeadType IN(2,4)/*Addition & Reimbursement*/ 
														AND EmployeeID=@PV_EmployeeID AND ExecutedOn BETWEEN @PV_PPM_StartDate AND @PV_PPM_EndDate )						  

					SET @PV_CompDisciplinaryActionDeduction=(SELECT ISNULL(SUM(CompAmount),0) FROM View_DisciplinaryAction WITH(NOLOCK) WHERE ApproveBy>0 AND SalaryHeadType=3/*Deduction*/ 
											AND EmployeeID=@PV_EmployeeID AND ExecutedOn BETWEEN @PV_PPM_StartDate AND @PV_PPM_EndDate)	

					--IF @PV_BaseAddress NOT IN ('amg')
					--BEGIN
					--	SET @PV_CompDisciplinaryActionAddition=@PV_DisciplinaryActionAddition
					--	SET @PV_CompDisciplinaryActionDeduction=@PV_DisciplinaryActionDeduction
					--END
								  
				END			
				/**********End Without any Condition Addition Deduction Reimbursement & DisciplinaryAction summation */
				
				--For New Joining
				IF @PV_DOJ>@PV_PPM_StartDate
				BEGIN
					IF @PV_BaseAddress IN ('rthr')
					BEGIN
						SET @PV_DeductionForNewJoining= ((@PV_Basic)/((DATEDIFF(DAY,@PV_PPM_StartDate,@PV_PPM_EndDate)+1)))
															* (DATEDIFF(DAY,@PV_PPM_StartDate,@PV_DOJ))

						--SET @PV_CompDeductionForNewJoining= ((@PV_CompBasic)/((DATEDIFF(DAY,@PV_PPM_StartDate,@PV_PPM_EndDate)+1)))
						--									* (DATEDIFF(DAY,@PV_PPM_StartDate,@PV_DOJ))
					END ELSE IF @PV_BaseAddress IN ('mithela')
					BEGIN
						SET @PV_DeductionForNewJoining= ((@PV_ActualGrossAmount+@PV_Addition)/((DATEDIFF(DAY,@PV_PPM_StartDate,@PV_PPM_EndDate)+1)))
															* (DATEDIFF(DAY,@PV_PPM_StartDate,@PV_DOJ))

						--SET @PV_CompDeductionForNewJoining= ((@PV_CompGrossAmount+@PV_CompAddition)/((DATEDIFF(DAY,@PV_PPM_StartDate,@PV_PPM_EndDate)+1)))
						--									* (DATEDIFF(DAY,@PV_PPM_StartDate,@PV_DOJ))
					END ELSE BEGIN
						SET @PV_DeductionForNewJoining= ((@PV_ActualGrossAmount+@PV_Addition)/((DATEDIFF(DAY,@PV_PPM_StartDate,@PV_PPM_EndDate)+1)))
															* (DATEDIFF(DAY,@PV_PPM_StartDate,@PV_DOJ))

						--SET @PV_CompDeductionForNewJoining= ((@PV_CompGrossAmount+@PV_CompAddition)/((DATEDIFF(DAY,@PV_PPM_StartDate,@PV_PPM_EndDate)+1)))
						--									* (DATEDIFF(DAY,@PV_PPM_StartDate,@PV_DOJ))
					END

					IF (@PV_DeductionForNewJoining<=0) BEGIN SET @PV_DeductionForNewJoining=0 END
					IF (@PV_CompDeductionForNewJoining<=0) BEGIN SET @PV_CompDeductionForNewJoining=0 END

					IF @PV_BaseAddress NOT IN ('A007','akcl')
					BEGIN
						IF NOT EXISTS (SELECT SalaryHeadID FROM SalarySchemeDetail WITH(NOLOCK) WHERE SalarySchemeID=@PV_SS_SalarySchemeID AND Condition=8)--New Joining
						BEGIN											
							SET @PV_DeductionForNewJoining=0
							--SET @PV_CompDeductionForNewJoining=0
						END
					END
				END 

				--For Resign Employee
				IF @PV_AttEndDate<@PV_PPM_EndDate
				BEGIN
					IF @PV_BaseAddress='mithela'
					BEGIN
						SET @PV_DeductionForResign= ((@PV_ActualGrossAmount+@PV_Addition)/((DATEDIFF(DAY,@PV_PPM_StartDate,@PV_PPM_EndDate)+1)))
															* (DATEDIFF(DAY,@PV_AttEndDate,@PV_PPM_EndDate))

						--SET @PV_CompDeductionForResign= ((@PV_CompGrossAmount+@PV_CompAddition)/((DATEDIFF(DAY,@PV_PPM_StartDate,@PV_PPM_EndDate)+1)))
						--								* (DATEDIFF(DAY,@PV_AttEndDate,@PV_PPM_EndDate))
					END ELSE BEGIN
						SET @PV_DeductionForResign= ((@PV_ActualGrossAmount+@PV_Addition)/((DATEDIFF(DAY,@PV_PPM_StartDate,@PV_PPM_EndDate)+1)))
															* (DATEDIFF(DAY,@PV_AttEndDate,@PV_PPM_EndDate))

						--SET @PV_CompDeductionForResign= ((@PV_CompGrossAmount+@PV_CompAddition)/((DATEDIFF(DAY,@PV_PPM_StartDate,@PV_PPM_EndDate)+1)))
						--								* (DATEDIFF(DAY,@PV_AttEndDate,@PV_PPM_EndDate))
					END

					IF (@PV_DeductionForResign<=0) BEGIN SET @PV_DeductionForResign=0 END
					--IF (@PV_CompDeductionForResign<=0) BEGIN SET @PV_CompDeductionForResign=0 END
					
					IF @PV_BaseAddress NOT IN ('A007','akcl')
					BEGIN
						IF NOT EXISTS (SELECT SalaryHeadID FROM SalarySchemeDetail WITH(NOLOCK) WHERE SalarySchemeID=@PV_SS_SalarySchemeID AND Condition=9)--Resign
						BEGIN
							SET @PV_DeductionForResign=0
							--SET @PV_CompDeductionForResign=0
						END
					END					
				END

				/***********Conditional Addition Deduction Reimbursement***************************/	
										
				--AttendanceBonus: EnumAllowanceCondition=1 (Condition)
				IF @PV_SS_IsAttendanceDependant=1
				BEGIN
					IF @PV_BaseAddress IN ('A007')
					BEGIN
						IF @PV_TotalDaysOfAbsent=0 AND @PV_TotalDayForAttBonus-@PV_TotalPLeave-@PV_TotalUPLeave=@PV_TotalPresentDay
						BEGIN										
							--SET @PV_AttBonus=(SELECT ISNULL (SUM(Amount),0) FROM VIEW_EmployeeSalaryStructureDetail WHERE ESSID=@PV_ESSID AND Condition=1 AND SalaryHeadType IN (2,4))--Addition & Reimbursement
							SET @PV_AttBonus=ISNULL((SELECT top(1)FixedValue FROM SalarySchemeDetailCalculation WITH(NOLOCK) WHERE SalarySchemeDetailID IN (
												SELECT SalarySchemeDetailID FROM SalarySchemeDetail WHERE SalarySchemeID=@PV_SS_SalarySchemeID AND Condition=1)),0)
						END 
						--Compliance
						--IF @PV_CompTotalDaysOfAbsent=0 AND @PV_CompTotalDayForAttBonus-@PV_CompTotalLeave=@PV_CompTotalPresentDay
						--BEGIN										
						--	--SET @PV_AttBonus=(SELECT ISNULL (SUM(Amount),0) FROM VIEW_EmployeeSalaryStructureDetail WHERE ESSID=@PV_ESSID AND Condition=1 AND SalaryHeadType IN (2,4))--Addition & Reimbursement
						--	SET @PV_CompAttBonus=ISNULL((SELECT top(1)FixedValue FROM SalarySchemeDetailCalculation WITH(NOLOCK) WHERE SalarySchemeDetailID IN (
						--						SELECT SalarySchemeDetailID FROM SalarySchemeDetail WHERE SalarySchemeID=@PV_SS_SalarySchemeID AND Condition=1)),0)
						--END 

					END ELSE --,'akcl and others
					BEGIN
						IF @PV_TotalDaysOfAbsent=0 AND @PV_TotalPLeave=0 AND @PV_TotalUPLeave=0 AND @PV_TotalDayForAttBonus=@PV_TotalPresentDay
						BEGIN												
							--SET @PV_AttBonus=(SELECT ISNULL (SUM(Amount),0) FROM VIEW_EmployeeSalaryStructureDetail WHERE ESSID=@PV_ESSID AND Condition=1 AND SalaryHeadType IN (2,4))--Addition & Reimbursement
							SET @PV_AttBonus=ISNULL((SELECT top(1)FixedValue FROM SalarySchemeDetailCalculation WITH(NOLOCK) WHERE SalarySchemeDetailID IN (
												SELECT SalarySchemeDetailID FROM SalarySchemeDetail WITH(NOLOCK) WHERE SalarySchemeID=@PV_SS_SalarySchemeID AND Condition=1 AND SalaryType IN (0,1,2))),0)
							IF @PV_BaseAddress='golden' AND @PV_LateHourInMin>5 --AND @PV_TotalLate>0	
							BEGIN
								--If any Employee late for 5 minutes more in a month, S/he will not get any Attendance Bonus.(Mr. Rahat: 25 Apr 2018)
								SET @PV_AttBonus=0
							END
						END 	

						--Compliance
						--IF @PV_CompTotalDaysOfAbsent=0 AND @PV_CompTotalLeave=0 AND @PV_CompTotalDayForAttBonus=@PV_CompTotalPresentDay
						--BEGIN												
						--	--SET @PV_AttBonus=(SELECT ISNULL (SUM(Amount),0) FROM VIEW_EmployeeSalaryStructureDetail WHERE ESSID=@PV_ESSID AND Condition=1 AND SalaryHeadType IN (2,4))--Addition & Reimbursement
						--	SET @PV_CompAttBonus=ISNULL((SELECT top(1)FixedValue FROM SalarySchemeDetailCalculation WITH(NOLOCK) WHERE SalarySchemeDetailID IN (
						--						SELECT SalarySchemeDetailID FROM SalarySchemeDetail WHERE SalarySchemeID=@PV_SS_SalarySchemeID AND Condition=1 AND SalaryType IN (0,1,3))),0)
						--	IF @PV_BaseAddress='golden' AND @PV_LateHourInMin>5 --AND @PV_TotalLate>0	
						--	BEGIN
						--		--If any Employee late for 5 minutes more in a month, S/he will not get any Attendance Bonus..(Mr. Rahat: 25 Apr 2018)
						--		SET @PV_CompAttBonus=0
						--	END
						--END 									
					END
				END--Att dependence

				----LWP: EnumAllowanceCondition=2 (Condition)						
				IF (@PV_TotalUPLeave>0 AND @PV_SS_IsAttendanceDependant=1)
				BEGIN--Check Attendance
					IF EXISTS (SELECT SalaryHeadID FROM SalarySchemeDetail WITH(NOLOCK) WHERE SalarySchemeID=@PV_SS_SalarySchemeID AND Condition=2)
					BEGIN						
						IF @PV_BaseAddress IN ('amg','mithela')
						BEGIN
							SET @PV_LWPAmount=@PV_TotalUPLeave*@PV_OneDayGross
						END ELSE BEGIN
							SET @PV_LWPAmount=@PV_TotalUPLeave*@PV_OneDayBasic
						END

						--Complaince
						--IF @PV_BaseAddress='mithela'
						--BEGIN
						--	SET @PV_CompLWPAmount=@PV_TotalUPLeave*@PV_OneDayBasic
						--END ELSE BEGIN
						--	SET @PV_CompLWPAmount=0
						--END
					END
				END	

				--Absent: EnumAllowanceCondition=5 (Condition)										
				IF (@PV_SS_IsAttendanceDependant=1)
				BEGIN--Check Attendance
					IF EXISTS (SELECT SalaryHeadID FROM SalarySchemeDetail WITH(NOLOCK) WHERE SalarySchemeID=@PV_SS_SalarySchemeID AND Condition=5)
					BEGIN						
						IF @PV_BaseAddress IN ('union','amg','b007','mithela')
						BEGIN
							SET @PV_AbsentAmount=@PV_TotalDaysOfAbsent*@PV_OneDayGross							
						END ELSE BEGIN
							SET @PV_AbsentAmount=@PV_TotalDaysOfAbsent*@PV_OneDayBasic
						END

						--Compliance
						--IF @PV_BaseAddress IN ('amg','b007','union','golden','td','mithela')
						--BEGIN
						--	SET @PV_CompAbsentAmount=@PV_CompTotalDaysOfAbsent*(@PV_CompBasic/30)
						--END ELSE
						--BEGIN
						--	SET @PV_CompAbsentAmount=@PV_AbsentAmount
						--END

						--IF @PV_DOJ>=@PV_PPM_StartDate AND @PV_AbsentAmount>0 AND @PV_BaseAddress='golden' 
						--BEGIN
						--	SET @PV_AbsentAmount=0
						--	SET @PV_AbsentAmount=@PV_TotalDaysOfAbsent*(@PV_Basic/(DATEDIFF(DAY,@PV_PPM_StartDate,@PV_PPM_EndDate)+1))
						--	SET @PV_CompAbsentAmount=@PV_TotalDaysOfAbsent*(@PV_CompBasic/(DATEDIFF(DAY,@PV_PPM_StartDate,@PV_PPM_EndDate)+1))
						--END
					END
				END	


				--Late: EnumAllowanceCondition=6 (Condition)										
				IF (@PV_SS_LateCount>0 AND @PV_SS_IsAttendanceDependant=1)
				BEGIN
					IF EXISTS (SELECT SalaryHeadID FROM SalarySchemeDetail WITH(NOLOCK) WHERE SalarySchemeID=@PV_SS_SalarySchemeID AND Condition=6)--Late					
					BEGIN
						IF @PV_BaseAddress IN ('amg')
						BEGIN
							IF @PV_TotalLate>=3
							BEGIN
								SET @PV_DayForLate=	@PV_TotalLate/@PV_SS_LateCount	
								SET @PV_LateAmount=@PV_AttBonus+ (@PV_DayForLate*@PV_OneDayGross)								
							END

							IF @PV_TotalLate=2
							BEGIN
								SET @PV_LateAmount=@PV_AttBonus
							END							
						END ELSE BEGIN
							SET @PV_DayForLate=	@PV_TotalLate/@PV_SS_LateCount	
							IF (@PV_SS_FixedLateAmount>0)
							BEGIN
								IF (@PV_AttBonus>0 AND @PV_DayForLate>0)
								BEGIN
									SET @PV_LateAmount=	@PV_SS_FixedLateAmount
								END								
							END ELSE BEGIN
								IF @PV_BaseAddress IN ('B007')
								BEGIN
									SET @PV_LateAmount=@PV_DayForLate*@PV_OneDayGross	
								END ELSE BEGIN
									SET @PV_LateAmount=@PV_DayForLate*@PV_OneDayBasic	
								END								
							END							
						END

						--Compliance
						--IF @PV_BaseAddress IN ('amg','B007')
						--BEGIN
						--	SET @PV_CompLateAmount=0 
						--END ELSE BEGIN
						--	SET @PV_CompLateAmount=@PV_LateAmount
						--	--[NB: Need clculation as per compiance. It will be done later.--2017 10 24]
						--END
					END
				END--Late Count			

				--Late: EnumAllowanceCondition=6 (Condition)										
				IF (@PV_SS_EarlyLeavingCount>0 AND @PV_SS_IsAttendanceDependant=1)
				BEGIN
					IF EXISTS (SELECT SalaryHeadID FROM SalarySchemeDetail WITH(NOLOCK) WHERE SalarySchemeID=@PV_SS_SalarySchemeID AND Condition=7)--Early					
					BEGIN
						IF @PV_BaseAddress='amg'
						BEGIN
							DECLARE @PV_TWHMin decimal(18,2)
							SET @PV_TWHMin=ISNULL((SELECT TotalWorkingTime FROM HRM_Shift WITH(NOLOCK) WHERE ShiftID IN (
											 SELECT CurrentShiftID FROM EmployeeOfficial WHERE EmployeeID=@PV_EmployeeID)),1)
							SET @PV_EarlyLeaveAmount=ROUND((@PV_ActualGrossAmount*@PV_EarlyLeaveInMinute)
													--/((DATEDIFF(DAY,@PV_PPM_StartDate,@PV_PPM_EndDate)+1)*8*60)
													--/(26*10*60),0)-- Updated on request of Mr. Rahat dated on 01 Apr 2017
													--/(26*@PV_TWHMin),0)--Updated on request of Mr. Rahat dated on 12 Apr 2017
													/((DATEDIFF(DAY,@PV_PPM_StartDate,@PV_PPM_EndDate)+1)*@PV_TWHMin),0)--Updated on request of Mr. Rahat dated on 13 Apr 2017 over the phone at 04: 33 PM
							--SET @PV_CompEarlyLeaveAmount=0
						END ELSE IF @PV_BaseAddress='golden' BEGIN
							IF @PV_EarlyLeaveInMinute>0
							BEGIN
								SET @PV_EarlyLeaveAmount=(@PV_Basic/14400)*@PV_EarlyLeaveInMinute
							END
							--IF @PV_CompEarlyLeaveInMinute>0
							--BEGIN
							--	 SET @PV_CompEarlyLeaveAmount=(@PV_CompBasic/14400)*@PV_CompEarlyLeaveInMinute
							--END	
						END ELSE BEGIN
							SET @PV_DayForEarly=@PV_TotalEarlyLeaving/@PV_SS_EarlyLeavingCount
							IF @PV_SS_FixedEarlyAmount>0
							BEGIN
								IF (@PV_AttBonus>0 AND @PV_DayForEarly>0)
								BEGIN
									SET @PV_EarlyLeaveAmount=@PV_SS_FixedEarlyAmount
								END
							END ELSE BEGIN
								SET @PV_EarlyLeaveAmount=@PV_DayForEarly*@PV_OneDayBasic
							END
							--SET @PV_CompEarlyLeaveAmount=@PV_EarlyLeaveAmount
							--[NB: Need clculation as per compiance. It will be done later.--2017 10 24]
						END

						--Compliance
						--IF @PV_BaseAddress='amg'
						--BEGIN
						--	SET @PV_CompEarlyLeaveAmount=0
						--END ELSE BEGIN
						--	SET @PV_CompEarlyLeaveAmount=@PV_EarlyLeaveAmount
						--END
					END
				END--Early Leaving Count
											
				--Education Fund: EnumAllowanceCondition=999 (Condition)
				IF EXISTS (SELECT SalaryHeadID FROM SalarySchemeDetail WITH(NOLOCK) WHERE SalarySchemeID=@PV_SS_SalarySchemeID AND Condition=999)--Education Fund
				BEGIN
					IF @PV_BaseAddress='amg'
					BEGIN
						IF @PV_ActualGrossAmount BETWEEN 1 AND 19999
						BEGIN
							SET @PV_EducationFundAmt=10
						END ELSE IF @PV_ActualGrossAmount BETWEEN 20000 AND 39999
						BEGIN
							SET @PV_EducationFundAmt=20
						END ELSE IF @PV_ActualGrossAmount>=40000
						BEGIN
							SET @PV_EducationFundAmt=30
						END
					END
				END

				/**********END Conditional Addition, Deduction & Reimbursement******************/

				/*****PF Deduction*******/	
				--[NB: Compliance PF deduction will be done later. 2017 10 24]			
				SET @PV_PFAmount=0
				IF EXISTS (SELECT * FROM PFmember WITH(NOLOCK) WHERE EmployeeID=@PV_EmployeeID AND IsActive=1 AND ApproveBy>0 AND PFSchemeID IN (SELECT PFSchemeID FROM PFScheme WHERE IsActive=1))
				BEGIN--PF
					SET @PV_PFSalaryHeadID=(SELECT RecommandedSalaryHeadID FROM PFScheme WITH(NOLOCK) WHERE PFSchemeID IN (
											SELECT PFSchemeID FROM PFmember WHERE EmployeeID=@PV_EmployeeID AND IsActive=1 AND ApproveBy>0))
					
					SELECT @PV_PFValueInPercent=Value,@PV_PFCalculationON=CalculationOn FROM PFMemberContribution WITH(NOLOCK) WHERE PFSchemeID IN (
					SELECT PFSchemeID FROM PFmember WHERE EmployeeID=@PV_EmployeeID AND IsActive=1 AND ApproveBy>0)
					AND @PV_ActualGrossAmount BETWEEN MinAmount AND MaxAmount
					
					IF @PV_PFValueInPercent>0 AND @PV_PFSalaryHeadID>0 AND @PV_PFCalculationON>0
					BEGIN
						IF @PV_PFCalculationON=1--Enum GROSS
						BEGIN
							SET @PV_PFAmount=@PV_PFValueInPercent*@PV_ActualGrossAmount/100
						END ELSE 
						BEGIN
							SET @PV_PFAmount=@PV_PFValueInPercent*@PV_Basic/100		
						END
					END					
				END--PF
				/*****END PF Deduction*******/

				/*****TAX Deduction**********/
				DECLARE @PV_Tax_SalaryHeadID as int, @PV_Tax_Amount as decimal (18,2)
				SET @PV_Tax_Amount=0
				SET @PV_Tax_SalaryHeadID=(SELECT top(1)SalaryHeadID FROM ITaxRateScheme WITH(NOLOCK) WHERE ITaxAssessmentYearID IN (
										  SELECT ITaxAssessmentYearID FROM ITaxAssessmentYear WHERE @PV_PPM_EndDate BETWEEN StartDate AND EndDate))

				SET @PV_Tax_Amount=ISNULL((SELECT top(1)InstallmentAmount FROM ITaxLedger WITH(NOLOCK) WHERE EmployeeID=@PV_EmployeeID AND ITaxRateSchemeID IN (
									SELECT ITaxRateSchemeID FROM ITaxRateScheme WHERE ITaxAssessmentYearID IN (
									SELECT ITaxAssessmentYearID FROM ITaxAssessmentYear WHERE @PV_PPM_EndDate BETWEEN StartDate AND EndDate))
									AND InactiveDate IS NULL),0)

				/*****End TAX Deduction**********/


				/*****Start PI Addition*******/
				IF EXISTS (SELECT * FROM PerformanceIncentiveEvaluation WITH(NOLOCK) WHERE PIMemberID IN (
							SELECT PIMemberID FROM PerformanceIncentiveMember WHERE EmployeeID=@PV_EmployeeID AND ApproveBy>0 AND InactiveBy=0) 
							AND MonthID=@PV_PPM_Month AND [Year]=DATEPART(YYYY,@PV_PPM_EndDate) AND ApproveBy>0)
				BEGIN--PI
					SET @PV_PISalaryHeadID=(SELECT SalaryHeadID FROM PerformanceIncentive WITH(NOLOCK) WHERE PIID IN (
											SELECT PIID FROM PerformanceIncentiveMember WHERE EmployeeID=@PV_EmployeeID 
											AND ApproveBy>0 AND InactiveBy=0))

					SET @PV_PIAmount=(SELECT Amount FROM PerformanceIncentiveEvaluation WITH(NOLOCK) WHERE PIMemberID IN (
									SELECT PIMemberID FROM PerformanceIncentiveMember WHERE EmployeeID=@PV_EmployeeID AND ApproveBy>0 AND InactiveBy=0) 
									AND MonthID=@PV_PPM_Month AND [Year]=DATEPART(YYYY,@PV_PPM_EndDate) AND ApproveBy>0)
				END--PI
				/*****END PI Addition*******/

				
				/*******************************************************************************************************/
			
				/*******Benefits On Attendance********/	
				DECLARE @tbl_BOA AS TABLE (SalaryHeadID int,Amount decimal(18,2))
				DECLARE @PV_BOAAmount as decimal (18,2)
				SET @PV_BOAAmount=0
				DELETE FROM @tbl_BOA

				IF EXISTS (SELECT * FROM View_BenefitOnAttendanceEmployeeLedger WITH(NOLOCK) WHERE EmployeeID =@PV_EmployeeID AND (IsExtraBenefit!=1 OR IsExtraBenefit IS NULL)
				AND SalaryHeadID IN (SELECT SalaryHeadID FROM Salaryhead)
				AND AttendanceDate BETWEEN @PV_StartDate AND @PV_EndDate)
				BEGIN

					INSERT INTO @tbl_BOA
					SELECT SalaryHeadID
					,CASE WHEN  AllowanceOn=0 THEN Value*COUNT(*)
						  WHEN AllowanceOn=1 THEN (@PV_OneDayGross *Value)/100*COUNT(*)
						  WHEN AllowanceOn=2 THEN (@PV_OneDayBasic*Value)/100*COUNT(*) END Amount
					FROM View_BenefitOnAttendanceEmployeeLedger WITH(NOLOCK) WHERE EmployeeID =@PV_EmployeeID AND (IsExtraBenefit!=1 OR IsExtraBenefit IS NULL)
					AND SalaryHeadID IN (SELECT SalaryHeadID FROM Salaryhead)
					AND AttendanceDate BETWEEN @PV_StartDate AND @PV_EndDate
					GROUP BY BOAID,SalaryHeadID,Value,AllowanceOn

					SET @PV_BOAAmount=ISNULL((SELECT SUM (Amount) FROM @tbl_BOA),0)
				END
				/***************End BOA**********************/		

				/*******Overtime Calculation***********/
				IF (@PV_SS_IsAllowOverTime=1)
				BEGIN
					--Actual
					SET @PV_OTHour=(SELECT ISNULL(SUM(OverTimeInMin),0) FROM MaxOTConfigurationAttendance WITH(NOLOCK)
									WHERE EmployeeID=@PV_EmployeeID AND AttendanceDate BETWEEN @PV_StartDate AND @PV_EndDate)

					IF (@PV_OTHour>0)
					BEGIN
						SET @PV_OTHour=@PV_OTHour/60
						IF @PV_SS_OverTimeOn=1--GROSS
						BEGIN
							SET @PV_OTValue=@PV_OTHour*ROUND((@PV_ActualGrossAmount/@PV_SS_DevidedBy)*@PV_SS_MultiplicationBy,2)
						END

						IF @PV_SS_OverTimeOn=2--Basic
						BEGIN
							SET @PV_OTValue=@PV_OTHour*ROUND((@PV_Basic/@PV_SS_DevidedBy)*@PV_SS_MultiplicationBy,2)
						END

						SET @PV_OTRate=@PV_OTValue/@PV_OTHour;
					END

					----Compliance
					--SET @PV_OTHour_Com=(SELECT ISNULL(SUM(CompOverTimeInMinute),0) FROM AttendanceDaily WITH(NOLOCK)
					--WHERE EmployeeID=@PV_EmployeeID AND AttendanceDate BETWEEN @PV_StartDate AND @PV_EndDate)

					--IF (@PV_OTHour_Com>0)
					--BEGIN
					--	SET @PV_OTHour_Com=@PV_OTHour_Com/60
					--	IF @PV_SS_OverTimeOn_Com=1
					--	BEGIN
					--		SET @PV_OTValue_Com=@PV_OTHour_Com*(@PV_CompGrossAmount/@PV_SS_DevidedBy_Com)*@PV_SS_MultiplicationBy_Com
					--	END
					--	IF @PV_SS_OverTimeOn_Com=2
					--	BEGIN
					--		SET @PV_OTValue_Com=@PV_OTHour_Com*(@PV_CompBasic/@PV_SS_DevidedBy_Com)*@PV_SS_MultiplicationBy_Com
					--	END

					--	SET @PV_OTRate_Com=@PV_OTValue_Com/@PV_OTHour_Com;
					--END
				END
				/*******END OT ************************/


				/*******Loan Deduction******************/
				SET @PV_Loan_SalaryHeadID=ISNULL((SELECT SalaryHeadID FROM EmployeeLoanSetup WITH(NOLOCK) WHERE InactiveBy=0),0)
				IF (@PV_Loan_SalaryHeadID>0)
				BEGIN
					SET @PV_Loan=ROUND(ISNULL((SELECT SUM(InstallmentAmount+InterestPerInstallment) FROM EmployeeLoanInstallment WITH(NOLOCK) WHERE EmployeeLoanID IN (
								SELECT EmployeeLoanID FROM EmployeeLoan WHERE EmployeeID=@PV_EmployeeID)
								AND InstallmentDate BETWEEN @PV_StartDate AND @PV_EndDate),0),0)
				END

				/***************************************/

				/*******AdvanceSalary Deduction***********/
				DECLARE @PV_EASP_SalaryHeadID as int,@PV_EASP_AdvanceAmount as decimal (18,2)
				SET @PV_EASP_SalaryHeadID=0; SET @PV_EASP_AdvanceAmount=0			
				IF EXISTS (SELECT * FROM View_EmployeeAdvanceSalary WITH(NOLOCK) WHERE EmployeeID=@PV_EmployeeID AND NYear=DATEPART(YEAR,@PV_PPM_EndDate) AND NMonth=@PV_PPM_Month AND ApproveBy>0 AND SararyHeadID>0)
				BEGIN
					SELECT @PV_EASP_SalaryHeadID=SararyHeadID,@PV_EASP_AdvanceAmount=NetAmount FROM View_EmployeeAdvanceSalary WHERE EmployeeID=@PV_EmployeeID AND NYear=DATEPART(YEAR,@PV_PPM_EndDate) 
					AND NMonth=@PV_PPM_Month AND ApproveBy>0 AND SararyHeadID>0	
				END
				/***************************************/

				

				--Net Amount Calculation
				IF @PV_SS_IsProductionBase=1
				BEGIN				
					SET @PV_NetAmount=@PV_ProductionAmount+@PB_ProductionBonus+@PV_Addition+@PV_BOAAmount
									  +@PV_TotalNoWorkDayAllowance+@PV_OTValue+@PV_AttBonus--+@PV_LeaveAllowance
									  +@PV_DisciplinaryActionAddition+@PV_PIAmount									  
									  -@PV_Deduction-@PV_DisciplinaryActionDeduction									  
									  -@PV_AbsentAmount-@PV_LateAmount-@PV_EarlyLeaveAmount
									  -@PV_ReveniewStemp-@PV_PFAmount-@PV_Tax_Amount
									  -@PV_DeductionForNewJoining-@PV_LWPAmount-@PV_DeductionForResign
									  -@PV_Loan-@PV_EducationFundAmt
									  -@PV_EASP_AdvanceAmount --Advance Payment
				END	
				ELSE BEGIN	
				
					SET @PV_NetAmount=@PV_ActualGrossAmount
										+@PV_Addition+@PV_BOAAmount+@PV_TotalNoWorkDayAllowance+@PV_OTValue+@PV_AttBonus--+@PV_LeaveAllowance
										+@PV_DisciplinaryActionAddition+@PV_PIAmount										
										-@PV_Deduction-@PV_DisciplinaryActionDeduction
										-@PV_AbsentAmount-@PV_LateAmount-@PV_EarlyLeaveAmount
										-@PV_ReveniewStemp-@PV_PFAmount-@PV_Tax_Amount
										-@PV_DeductionForNewJoining-@PV_LWPAmount-@PV_DeductionForResign
										-@PV_Loan-@PV_EducationFundAmt
										-@PV_EASP_AdvanceAmount --Advance Payment

					--Compliance
					--IF @PV_BaseAddress IN ('amg')
					--BEGIN
					--	SET @PV_NetAmount_Com=ISNULL(@PV_CompGrossAmount
					--						+@PV_CompAddition+0--@PV_BOAAmount
					--						+@PV_TotalNoWorkDayAllowance+@PV_OTValue_Com+@PV_CompAttBonus--+@PV_LeaveAllowance
					--						+@PV_CompDisciplinaryActionAddition
					--						+@PV_PIAmount										
					--						-@PV_CompDeduction
					--						-@PV_CompDisciplinaryActionDeduction
					--						-@PV_CompAbsentAmount-@PV_CompLateAmount-@PV_CompEarlyLeaveAmount
					--						-@PV_ReveniewStemp-@PV_PFAmount-@PV_Tax_Amount
					--						-@PV_CompDeductionForNewJoining-@PV_CompLWPAmount
					--						-@PV_DeductionForResign
					--						-@PV_Loan-0--@PV_EducationFundAmt
					--						-@PV_EASP_AdvanceAmount,0) --Advance Payment

					--						/*
					--							NO EducationFund
					--							NO WellFare
					--							NO Late
					--							NO Early
					--							NO BOA Amount
					--							No LWP
					--						*/
						
					--END ELSE BEGIN
					--	--SET @PV_NetAmount_Com=@PV_NetAmount-@PV_OTValue+@PV_OTValue_Com+@PV_CompAttBonus-@PV_AttBonus
					--	--					-@PV_DisciplinaryActionAddition+@PV_CompDisciplinaryActionAddition
					--	--					+@PV_DisciplinaryActionDeduction-@PV_CompDisciplinaryActionDeduction
					--	--					-@PV_BOAAmount
					--	SET @PV_NetAmount_Com=CASE WHEN @PV_CompGrossAmount IS NOT NULL AND  @PV_CompGrossAmount>0 THEN @PV_CompGrossAmount ELSE @PV_ActualGrossAmount END
					--						+@PV_CompAddition+@PV_TotalNoWorkDayAllowance+@PV_OTValue_Com+@PV_CompAttBonus--+@PV_LeaveAllowance
					--						+@PV_CompDisciplinaryActionAddition+@PV_PIAmount										
					--						-@PV_CompDeduction-@PV_CompDisciplinaryActionDeduction
					--						-@PV_CompAbsentAmount-@PV_CompLateAmount-@PV_CompEarlyLeaveAmount
					--						-@PV_ReveniewStemp-@PV_PFAmount-@PV_Tax_Amount
					--						-@PV_DeductionForNewJoining-@PV_CompLWPAmount-@PV_DeductionForResign
					--						-@PV_Loan-@PV_EducationFundAmt
					--						-@PV_EASP_AdvanceAmount
					--						---@PV_BOAAmount
					--END

				END						  
				
				--SET @PV_NetAmount_Com=@PV_NetAmount-@PV_OTValue+@PV_OTValue_Com

				IF (@PV_NetAmount IS NULL OR @PV_NetAmount<=0) AND @PV_BaseAddress!='digicon'
				BEGIN
					GOTO CONT
				END

				IF @PV_IsCashFixed=1
				BEGIN
					IF @PV_CashAmount>0 
					BEGIN
						SET @PV_BankAmount=CASE WHEN @PV_NetAmount>@PV_CashAmount THEN @PV_NetAmount-@PV_CashAmount ELSE 0 END
					END ELSE BEGIN
						SET @PV_CashAmount=@PV_NetAmount
						SET @PV_BankAmount=0
					END
				END ELSE BEGIN
					IF @PV_CashAmount>0 
					BEGIN
						SET @PV_BankAmount=@PV_CashAmount
						SET @PV_CashAmount=CASE WHEN @PV_NetAmount>@PV_CashAmount THEN @PV_NetAmount-@PV_BankAmount ELSE 0 END
					END ELSE BEGIN
						SET @PV_BankAmount=@PV_NetAmount
						SET @PV_CashAmount=0
					END
				END

				IF @PV_BankAmount>0
				BEGIN
					IF EXISTS (SELECT * FROM EmployeeBankAccount WHERE EmployeeID=@PV_EmployeeID AND IsActive=1)
					BEGIN
						SET @PV_EmployeeBankACID=(SELECT top(1)EmployeeBankACID FROM EmployeeBankAccount WHERE EmployeeID=@PV_EmployeeID AND IsActive=1)
					END ELSE BEGIN
						SET @PV_EmployeeBankACID=0
						SET @PV_BankAmount=0 
						SET @PV_CashAmount=@PV_NetAmount
					END
				END

				--Insert Employee Salary
				SET @PV_EmployeeSalaryID=(SELECT ISNULL(MAX(EmployeeSalaryID),0)+1 FROM ComplianceEmployeeSalary)
				INSERT INTO  [ComplianceEmployeeSalary]
							([EmployeeSalaryID],[EmployeeID],[LocationID],[DepartmentID],[DesignationID],[GrossAmount],[NetAmount],[CurrencyID],[SalaryDate],[SalaryReceiveDate],[PayrollProcessID],[IsManual],[StartDate],[EndDate],[IsLock]		,[ProductionAmount]  ,[ProductionBonus]  ,[OTHour]  ,[OTRatePerHour],[TotalWorkingDay]  ,[TotalAbsent]		  ,[TotalLate]	,[TotalEarlyLeaving]  ,[TotalDayOff]  ,[TotalUpLeave],[TotalPLeave]	   ,[RevenueStemp]	 ,[TotalNoWorkDay],[TotalNoWorkDayAllowance],[AddShortFall],[DBUSerID]	 , [DBServerDateTime],[TotalHoliday],LateInMin,CashAmount,BankAmount,BankAccountID)
				VALUES		(@PV_EmployeeSalaryID,@PV_EmployeeID,@PV_PPM_LocationID,@PV_DepartmentID,@PV_DesignationID,@PV_ActualGrossAmount,@PV_NetAmount,@PV_CurrencyID,GETDATE(),NULL,@Param_PayRollProcessID,0,@PV_PPM_StartDate,@PV_PPM_EndDate,0 ,@PV_ProductionAmount,@PB_ProductionBonus,@PV_OTHour,@PV_OTRate		,@PV_TotalWorkingDay,@PV_TotalDaysOfAbsent,@PV_TotalLate,@PV_TotalEarlyLeaving,@PV_TotalDayOff,@PV_TotalUPLeave,@PV_TotalPLeave,@PV_ReveniewStemp,@PV_TotalNoWorkDay,@PV_TotalNoWorkDayAllowance,@PV_AddShortFall,@Param_DBUserID,GETDATE(),@PV_TotalHoliday,@PV_LateHourInMin,@PV_CashAmount,@PV_BankAmount,@PV_EmployeeBankACID)
			
				--Insert Detail Unconditional AND Not ProcessDependent
				INSERT INTO [ComplianceEmployeeSalaryDetail]		
				SELECT @PV_EmployeeSalaryID,SalaryHeadID,Amount,@Param_DBUserID,GETDATE() From View_EmployeeSalaryStructureDetail 
				WHERE ESSID=@PV_ESSID AND SalaryHeadType IN (1,2,3,4) AND Condition=0 AND IsProcessDependent=0
				
				--Insert Detail Conditional
				IF (@PV_AttBonus>0)--Addition
				BEGIN
					--INSERT INTO [EmployeeSalaryDetail]		
					--SELECT @PV_EmployeeSalaryID,SalaryHeadID,Amount,@Param_DBUserID,GETDATE()
					--,CASE WHEN @PV_CompAttBonus>0 THEN CompAmount ELSE 0 END From View_EmployeeSalaryStructureDetail 
					--WHERE ESSID=@PV_ESSID AND SalaryHeadType IN (2,4) AND Condition=1--Monthly Full Attendance	
			
					INSERT INTO [ComplianceEmployeeSalaryDetail] 		
					SELECT @PV_EmployeeSalaryID
					,(SELECT SalaryHeadID FROM SalarySchemeDetail WHERE SalarySchemeID=@PV_SS_SalarySchemeID AND Condition=1)--Monthly Full Attendance	
					,@PV_AttBonus,@Param_DBUserID,GETDATE()
				END	

				IF (@PV_AbsentAmount>0)--Deduction
				BEGIN
					INSERT INTO [ComplianceEmployeeSalaryDetail] 		
					SELECT @PV_EmployeeSalaryID
					,(SELECT SalaryHeadID FROM SalarySchemeDetail WHERE SalarySchemeID=@PV_SS_SalarySchemeID AND Condition=5)--EnumAllowanceCondition=Absent
					,@PV_AbsentAmount,@Param_DBUserID,GETDATE()
				END

				--IF (@PV_DeductionForNewJoining>0)--Deduction
				--BEGIN
				--	IF EXISTS (SELECT SalaryHeadID FROM SalarySchemeDetail WITH(NOLOCK) WHERE SalarySchemeID=@PV_SS_SalarySchemeID AND Condition=8)--New Joining
				--	BEGIN
				--		INSERT INTO [ComplianceEmployeeSalaryDetail] 		
				--		SELECT @PV_EmployeeSalaryID
				--		,(SELECT SalaryHeadID FROM SalarySchemeDetail WITH(NOLOCK) WHERE SalarySchemeID=@PV_SS_SalarySchemeID AND Condition=8)--EnumAllowanceCondition=NewJoining
				--		,@PV_DeductionForNewJoining,@Param_DBUserID,GETDATE()
				--	END
				--END

				--IF (@PV_DeductionForResign>0)--Deduction
				--BEGIN
				--	IF EXISTS (SELECT SalaryHeadID FROM SalarySchemeDetail WITH(NOLOCK) WHERE SalarySchemeID=@PV_SS_SalarySchemeID AND Condition=9)--Resign
				--	BEGIN
				--		INSERT INTO [ComplianceEmployeeSalaryDetail] 		
				--		SELECT @PV_EmployeeSalaryID
				--		,(SELECT SalaryHeadID FROM SalarySchemeDetail WITH(NOLOCK) WHERE SalarySchemeID=@PV_SS_SalarySchemeID AND Condition=9)--EnumAllowanceCondition=Resign
				--		,@PV_DeductionForResign,@Param_DBUserID,GETDATE()
				--	END	
				--END

				--IF (@PV_LWPAmount>0)--Deduction
				--BEGIN
				--	INSERT INTO [ComplianceEmployeeSalaryDetail] 		
				--	SELECT @PV_EmployeeSalaryID
				--	,(SELECT SalaryHeadID FROM SalarySchemeDetail WITH(NOLOCK) WHERE SalarySchemeID=@PV_SS_SalarySchemeID AND Condition=2)--EnumAllowanceCondition=LWP
				--	,@PV_LWPAmount,@Param_DBUserID,GETDATE()
				--END

				--IF (@PV_LateAmount>0)--Deduction
				--BEGIN
				--	INSERT INTO [ComplianceEmployeeSalaryDetail] 		
				--	SELECT @PV_EmployeeSalaryID
				--	,(SELECT SalaryHeadID FROM SalarySchemeDetail WITH(NOLOCK) WHERE SalarySchemeID=@PV_SS_SalarySchemeID AND Condition=6)--EnumAllowanceCondition=Late
				--	,@PV_LateAmount,@Param_DBUserID,GETDATE()
				--END

				--IF (@PV_EarlyLeaveAmount>0)--Deduction
				--BEGIN
				--	INSERT INTO [ComplianceEmployeeSalaryDetail] 		
				--	SELECT @PV_EmployeeSalaryID
				--	,(SELECT SalaryHeadID FROM SalarySchemeDetail WITH(NOLOCK) WHERE SalarySchemeID=@PV_SS_SalarySchemeID AND Condition=7)--EnumAllowanceCondition=Early
				--	,@PV_EarlyLeaveAmount,@Param_DBUserID,GETDATE()
				--END

				--IF (@PV_EducationFundAmt>0)
				--BEGIN
				--	INSERT INTO [ComplianceEmployeeSalaryDetail] 		
				--	SELECT @PV_EmployeeSalaryID
				--	,(SELECT SalaryHeadID FROM SalarySchemeDetail WITH(NOLOCK) WHERE SalarySchemeID=@PV_SS_SalarySchemeID AND Condition=999)--EnumAllowanceCondition=EducationFund
				--	,@PV_EducationFundAmt,@Param_DBUserID,GETDATE()				
				--END
				--IF (@PV_LWPCount>0)--Deduction
				--BEGIN
				--	INSERT INTO [EmployeeSalaryDetail] 		
				--	SELECT @PV_EmployeeSalaryID,21/*Hard code -LWP*/,(@PV_LWPCount*@PV_OneDayBasic),@Param_DBUserID,GETDATE() 
								
				--END	

				--IF (@PV_PFAmount>0)--Deduction
				--BEGIN
				--	INSERT INTO [ComplianceEmployeeSalaryDetail] 		
				--	SELECT @PV_EmployeeSalaryID,@PV_PFSalaryHeadID,@PV_PFAmount,@Param_DBUserID,GETDATE()					
				--END	
				--IF (@PV_TAX_Amount>0)--Deduction
				--BEGIN
				--	INSERT INTO [ComplianceEmployeeSalaryDetail] 		
				--	SELECT @PV_EmployeeSalaryID,@PV_TAX_SalaryHeadID,@PV_TAX_Amount,@Param_DBUserID,GETDATE()
					
				--END	

				--IF (@PV_PIAmount>0)--Addition
				--BEGIN
				--	INSERT INTO [ComplianceEmployeeSalaryDetail] 		
				--	SELECT @PV_EmployeeSalaryID,@PV_PISalaryHeadID,@PV_PIAmount,@Param_DBUserID,GETDATE()
					
				--END	

				--IF (@PV_DisciplinaryActionAddition>0)
				--BEGIN	
				--	INSERT INTO [ComplianceEmployeeSalaryDetail] 	
				--	SELECT @PV_EmployeeSalaryID,aa.SalaryHeadID,aa.Amount,@Param_DBUserID,GETDATE()--CASE WHEN @PV_CompDisciplinaryActionAddition>0 THEN aa.Amount ELSE 0 END--@PV_CompDisciplinaryActionAddition--@PV_DisciplinaryActionAddition 
				--	FROM (SELECT SalaryHeadID,SUM(Amount) AS Amount FROM View_DisciplinaryAction 
				--	WHERE ApproveBy>0 AND SalaryHeadType IN(2,4)/*Addition & Reimbursement*/ 
				--	AND EmployeeID=@PV_EmployeeID AND ExecutedOn BETWEEN @PV_PPM_StartDate AND @PV_PPM_EndDate
				--	GROUP BY SalaryHeadID)aa

				--	--SELECT @PV_EmployeeSalaryID,SalaryHeadID,Amount,@Param_DBUserID,GETDATE() FROM View_DisciplinaryAction 
				--	--WHERE ApproveBy>0 AND SalaryHeadType IN(2,4)/*Addition & Reimbursement*/ 
				--	--AND EmployeeID=@PV_EmployeeID AND ExecutedOn BETWEEN @PV_PPM_StartDate AND @PV_PPM_EndDate						  
				--END

				--IF (@PV_DisciplinaryActionDeduction>0)
				--BEGIN	
				--	INSERT INTO [ComplianceEmployeeSalaryDetail] 	
				--	SELECT @PV_EmployeeSalaryID,aa.SalaryHeadID,aa.Amount,@Param_DBUserID,GETDATE()--CASE WHEN @PV_CompDisciplinaryActionDeduction>0 THEN aa.Amount ELSE 0 END--@PV_CompDisciplinaryActionDeduction--@PV_DisciplinaryActionDeduction 
				--	FROM (SELECT SalaryHeadID,SUM(Amount) AS Amount FROM View_DisciplinaryAction WITH(NOLOCK) 
				--	WHERE ApproveBy>0 AND SalaryHeadType IN(3)/*Deduction*/ 
				--	AND EmployeeID=@PV_EmployeeID AND ExecutedOn BETWEEN @PV_PPM_StartDate AND @PV_PPM_EndDate
				--	GROUP BY SalaryHeadID)aa

				--	--INSERT INTO [EmployeeSalaryDetail] 		
				--	--SELECT @PV_EmployeeSalaryID,SalaryHeadID,Amount,@Param_DBUserID,GETDATE() FROM View_DisciplinaryAction 
				--	--WHERE ApproveBy>0 AND SalaryHeadType IN(3)/*Deduction*/ 
				--	--AND EmployeeID=@PV_EmployeeID AND ExecutedOn BETWEEN @PV_PPM_StartDate AND @PV_PPM_EndDate						  
				--END

				----BOA Detail 
				--IF (@PV_BOAAmount>0)
				--BEGIN
				--	INSERT INTO [ComplianceEmployeeSalaryDetail]		
				--	SELECT @PV_EmployeeSalaryID,SalaryHeadID,SUM(Amount),@Param_DBUserID,GETDATE() 
				--	From @tbl_BOA GROUP BY SalaryHeadID
				--END		
				
				--IF (@PV_Loan>0)
				--BEGIN
				--	INSERT INTO [ComplianceEmployeeSalaryDetail]		
				--	SELECT @PV_EmployeeSalaryID,@PV_Loan_SalaryHeadID
				--	/*Loan*/,@PV_Loan,@Param_DBUserID,GETDATE()
				--	--Update employee loan installment

				--END

				----Advance Payment
				IF @PV_EASP_AdvanceAmount>0 AND @PV_EASP_SalaryHeadID>0--Deduction
				BEGIN
					INSERT INTO [ComplianceEmployeeSalaryDetail] 		
					SELECT @PV_EmployeeSalaryID,@PV_EASP_SalaryHeadID,@PV_EASP_AdvanceAmount,@Param_DBUserID,GETDATE()
				END
														
			END--salary exist checking		
		CONT:
	    SET @Param_Index=@Param_Index+1  
	FETCH NEXT FROM Cur_CC INTO   @PV_ESSID,@PV_EmployeeID,@PV_ActualGrossAmount,@PV_CurrencyID,@PV_SS_SalarySchemeID,@PV_CompGrossAmount,@PV_IsCashFixed,@PV_CashAmount
	END--
	CLOSE Cur_CC
	DEALLOCATE Cur_CC	
	
	IF NOT EXISTS (	SELECT * FROM (	SELECT ROW_NUMBER() OVER(ORDER BY ESSID) AS Row,ESSID,EmployeeID,ActualGrossAmount,CurrencyID,SalarySchemeID 
	FROM EmployeeSalaryStructure WITH(NOLOCK) WHERE IsActive=1 AND StartDay=DAY(@PV_PPM_StartDate) AND SalarySchemeID IN (
	SELECT SalarySchemeID FROM @tbl_SalaryScheme) AND EmployeeID IN (
	SELECT EmployeeID FROM View_EmployeeOfficial WITH(NOLOCK) WHERE WorkingStatus IN (1,2,6)/*InWorkingPlace,OSD*/
	AND IsActive=1 AND DateOfJoin<=@PV_PPM_EndDate AND DRPID IN (
	SELECT DepartmentRequirementPolicyID FROM DepartmentRequirementPolicy WITH(NOLOCK) WHERE LocationId=@PV_PPM_LocationID 
	AND DepartmentID IN (SELECT DepartmentID FROM @tbl_Department))))aa WHERE aa.Row>@Param_Index)
	BEGIN
		SET @Param_Index=0;
	END	

	SELECT @Param_Index
	
END TRY
BEGIN CATCH	
	ROLLBACK
		DECLARE @Msg as varchar (500)
		SET @Msg=(SELECT ERROR_MESSAGE() AS ErrorNumber)
		SET @Param_Index=0;
		RAISERROR('%s',16,1,@Msg)
	RETURN
END CATCH	
END








GO
/****** Object:  StoredProcedure [dbo].[SP_Process_ComplianceAttendanceProcessAsPerEdit]    Script Date: 11/20/2018 12:14:40 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[SP_Process_ComplianceAttendanceProcessAsPerEdit]
	@Param_StartDate DATE
	,@Param_EndDate DATE
	,@Param_EmployeeIDs VARCHAR(MAX)
	,@Param_MOCID INT
	,@Param_UserId INT
	,@Param_Index INT
AS
BEGIN
	
	IF OBJECT_ID('tempdb..#tbl_TimeCard') IS NOT NULL
	BEGIN
		DROP TABLE #tbl_TimeCard
	END
	IF OBJECT_ID('tempdb..#tbl_EmpID') IS NOT NULL
	BEGIN
		DROP TABLE #tbl_EmpID
	END

	DECLARE 
	@PV_MaxOTInMin as int
	,@PV_MaxOutTime AS DATETIME
	,@PV_MinInTime AS DateTime
	,@PV_IsPresentOnDayOff bit
	,@PV_IsPresentOnHoliday bit
	,@PV_SQL as varchar (max)
	,@PV_Loop_EmployeeID as int
	,@PV_Loop_OutTime as dateTime
	,@PV_Loop_InTime as dateTime
	,@PV_Loop_AttendanceDate Date
	,@PV_Loop_ShiftEndTime AS DATETIME
	,@PV_Loop_OverTimeInMinute AS int
	,@PV_Loop_IsDayOff as bit
	,@PV_Loop_IsHoliday as bit
	,@PV_Loop_IsLeave as bit
	,@PV_Loop_LeaveHeadID INT
	,@PV_Loop_ShiftID INT

	SELECT @PV_MaxOTInMin=ISNULL(MaxOTInMin,0)
	,@PV_MaxOutTime=MaxOutTime
	,@PV_MinInTime=MinInTime
	,@PV_IsPresentOnDayOff=ISNULL(IsPresentOnDayOff,0)
	,@PV_IsPresentOnHoliday=ISNULL(ISPresentOnHoliday,0) 
	FROM MaxOTConfiguration WHERE MOCID=@Param_MOCID

	SET @PV_MaxOTInMin=ISNULL((SELECT MaxOTInMin FROM MaxOTConfiguration WHERE MOCID=@Param_MOCID),0)
	SET @PV_SQL=''

	CREATE TABLE #tbl_TimeCard(AttendanceID int,EmployeeID int,AttendanceDate DATE,InTime DATETIME
							 ,OutTime DATETIME,TotalWorkingHourInMinute int,OverTimeInMinute int,ShiftID int, ShiftName VARCHAR(512),ShiftEndTime DateTime
							 ,LeaveHeadID int,LeaveName VARCHAR(100),IsOSD BIT,IsDayOff BIT, IsHoliday BIT, IsLeave BIT)
	CREATE CLUSTERED INDEX tbl_TimeCard_AttendanceID ON #tbl_TimeCard(AttendanceID)

	CREATE TABLE #tbl_EmpID(RowID int, EmployeeID INT)

	IF @Param_EmployeeIDs<>'' AND @Param_EmployeeIDs IS NOT NULL
	BEGIN
		
		INSERT INTO #tbl_EmpID
		SELECT top(200)aa.Row,aa.EmployeeID FROM(SELECT ROW_NUMBER() OVER(ORDER BY EmployeeOfficialID) AS Row, * FROM View_EmployeeOfficial WHERE IsActive =1 AND EmployeeID IN (@Param_EmployeeIDs))aa WHERE aa.Row>@Param_Index
		--SET @PV_SQL=@PV_SQL+'SELECT top(200)aa.Row,aa.EmployeeID FROM(SELECT ROW_NUMBER() OVER(ORDER BY EmployeeOfficialID) AS Row, * FROM View_EmployeeOfficial WHERE IsActive =1 AND EmployeeID IN ('+@Param_EmployeeIDs+'))aa WHERE aa.Row>' + CONVERT(VARCHAR(512), @Param_Index)
	END ELSE BEGIN
		INSERT INTO #tbl_EmpID
		SELECT top(200)aa.Row,aa.EmployeeID FROM(SELECT ROW_NUMBER() OVER(ORDER BY EmployeeOfficialID) AS Row, * FROM View_EmployeeOfficial WHERE IsActive =1 AND EmployeeID IN (SELECT EmployeeID FROM AttendanceDaily WHERE AttendanceDate BETWEEN 
			@Param_StartDate AND @Param_EndDate))aa WHERE aa.Row>@Param_Index
		
		--SET @PV_SQL=@PV_SQL+'SELECT top(200)aa.Row,aa.EmployeeID FROM(SELECT ROW_NUMBER() OVER(ORDER BY EmployeeOfficialID) AS Row, * FROM View_EmployeeOfficial WHERE IsActive =1 AND EmployeeID IN (SELECT EmployeeID FROM AttendanceDaily WHERE AttendanceDate BETWEEN 
		--	'''+CONVERT(VARCHAR(50),@Param_StartDate)+''' AND '''+CONVERT(VARCHAR(50),@Param_EndDate)+'''))aa WHERE aa.Row>' + CONVERT(VARCHAR(512), @Param_Index)
		
		--SET @PV_SQL=@PV_SQL+'SELECT EmployeeID EmployeeOfficial WHERE EmployeeID IN (SELECT EmployeeID FROM AttendanceDaily WHERE AttendanceDate BETWEEN 
		--	'''+CONVERT(VARCHAR(50),@Param_StartDate)+''' AND '''+CONVERT(VARCHAR(50),@Param_EndDate)+''') '
	END

	--IF EXISTS(SELECT * FROM Users WHERE userID = @Param_UserId AND FinancialUserType!=1)
	--BEGIN
	--	SET @PV_SQL = @PV_SQL + ' AND  DepartmentID IN(SELECT DepartmentID FROM DepartmentRequirementPolicy WHERE DepartmentRequirementPolicyID IN(SELECT DRPID FROM DepartmentRequirementPolicyPermission WHERE UserID ='  + CONVERT(VARCHAR(50),@Param_UserId)+'))'
	--END

	--SELECT @PV_SQL
	--Distinct Empolyee	
	--EXEC(@PV_SQL)
	

	--select * from #tbl_EmpID
	--Get AttendanceRecord
	INSERT INTO #tbl_TimeCard
	SELECT AD.AttendanceID,Emp.EmployeeID,AD.AttendanceDate,AD.InTime,AD.OutTime,0/*TotalWorkingHourInMinute*/,AD.OverTimeInMinute,AD.ShiftID, HS.Name
	,NULL/*Shift End Time*/,AD.LeaveHeadID,''/*Leave*/,AD.IsOSD,AD.IsDayOff,AD.IsHoliday, AD.IsLeave
	FROM #tbl_EmpID Emp
	LEFT JOIN AttendanceDaily AD ON Emp.EmployeeID = AD.EmployeeID
	LEFT JOIN HRM_Shift HS ON HS.ShiftID = AD.ShiftID
	WHERE AD.AttendanceDate BETWEEN @Param_StartDate AND @Param_EndDate
	
	DECLARE 
	@PV_AttendanceID INT
	,@PV_ActualStart DATETIME
	,@PV_ActualEnd DATETIME
	,@PV_LateInMunit INT
	,@PV_EarlyInMunit INT
	,@PV_StartTime DATETIME
	,@PV_EndTime DATETIME
	,@PV_TolaranceTime as Datetime--int
	,@PV_DayEndTime as DATETIME--time(0)
	,@PV_DayStartTime as DATETIME--time(0)
	,@PV_Remark as varchar(50)
	
	,@PV_Shift_IsOT as bit
	,@PV_Shift_OTStart as datetime
	,@PV_Shift_OTEnd   as datetime
	,@PV_Shift_ISOTOnActual as bit
	,@PV_Shift_OTCalclateAfterInMin as int
	,@PV_Shift_IsLeaveOnOFFHoliday as bit
	,@PV_ShiftID as int
	,@PV_ToleranceEarly as int
	--ASAD
	,@PV_IsHalfDayoff AS BIT
	,@PV_PStart AS DATETIME
	,@PV_PEnd AS DATETIME
	,@PV_IsLeave BIT

	,@PV_OverTime INT

	,@PV_ShiftEndTime DATETIME


	--Cursor for insert data
	DECLARE Cur_CCC CURSOR LOCAL FORWARD_ONLY KEYSET FOR
	SELECT EmployeeID,AttendanceDate,OutTime,/*ShiftEndTime,*/OverTimeInMinute,InTime, IsDayOff, IsHoliday, IsLeave, LeaveHeadID,ShiftID FROM #tbl_TimeCard
	OPEN Cur_CCC
	FETCH NEXT FROM Cur_CCC INTO  @PV_Loop_EmployeeID,@PV_Loop_AttendanceDate,@PV_Loop_OutTime,/*@PV_Loop_ShiftEndTime,*/@PV_Loop_OverTimeInMinute,@PV_Loop_InTime, @PV_Loop_IsDayOff, @PV_Loop_IsHoliday, @PV_Loop_IsLeave, @PV_Loop_LeaveHeadID, @PV_Loop_ShiftID
	WHILE(@@Fetch_Status <> -1)		
	BEGIN	

		--Delete if previous data exists
		IF EXISTS(SELECT * FROM MaxOTConfigurationAttendance WHERE MOCID =@Param_MOCID AND EmployeeID=@PV_Loop_EmployeeID AND AttendanceDate=@PV_Loop_AttendanceDate)
		BEGIN
			DELETE FROM MaxOTConfigurationAttendance WHERE MOCID =@Param_MOCID AND EmployeeID=@PV_Loop_EmployeeID AND AttendanceDate=@PV_Loop_AttendanceDate
		END

		--Process if no data found on that day
		IF NOT EXISTS (SELECT * FROM MaxOTConfigurationAttendance WHERE MOCID =@Param_MOCID AND EmployeeID=@PV_Loop_EmployeeID AND AttendanceDate=@PV_Loop_AttendanceDate)
		BEGIN
			SET @PV_ShiftEndTime= (SELECT EndTime FROM HRM_Shift WHERE ShiftID=@PV_Loop_ShiftID)
			IF @PV_MinInTime IS NOT NULL
			BEGIN
				SET @PV_Loop_InTime=(DATEADD(SECOND,0,DATEADD (MINUTE,DATEPART(MINUTE,@PV_MinInTime), DATEADD(HOUR, DATEPART(HOUR,@PV_MinInTime), DATEADD(dd, DATEDIFF(dd, -0, @PV_Loop_AttendanceDate), 0)))))
			END ELSE
			BEGIN
				SET @PV_Loop_InTime=(DATEADD(SECOND,0,DATEADD (MINUTE,DATEPART(MINUTE,@PV_Loop_InTime), DATEADD(HOUR, DATEPART(HOUR,@PV_Loop_InTime), DATEADD(dd, DATEDIFF(dd, -0, @PV_Loop_AttendanceDate), 0)))))
				
			END


			IF @PV_MaxOutTime IS NULL
			BEGIN
				SET @PV_Loop_ShiftEndTime = (DATEADD(SECOND,0,DATEADD (MINUTE,DATEPART(MINUTE,@PV_ShiftEndTime), DATEADD(HOUR, DATEPART(HOUR,@PV_ShiftEndTime), DATEADD(dd, DATEDIFF(dd, -0, @PV_Loop_InTime), 0)))))
				IF EXISTS(SELECT * FROM AttendanceDaily WHERE EmployeeID=@PV_Loop_EmployeeID AND AttendanceDate=@PV_Loop_AttendanceDate AND OverTimeInMinute>0)
				BEGIN
					SET @PV_Loop_ShiftEndTime = DATEADD(MINUTE,@PV_MaxOTInMin,@PV_ShiftEndTime)
				END
			END ELSE BEGIN
				UPDATE #tbl_TimeCard SET ShiftEndTime= (DATEADD(SECOND,0,DATEADD (MINUTE,DATEPART(MINUTE,@PV_MaxOutTime), DATEADD(HOUR, DATEPART(HOUR,@PV_MaxOutTime), DATEADD(dd, DATEDIFF(dd, -0, InTime), 0)))))
				SET @PV_MaxOTInMin=0
			END
			
			SET @PV_Loop_ShiftEndTime = DATEADD(MINUTE,RAND()*(10),@PV_Loop_ShiftEndTime)
			SET @PV_OverTime = CASE WHEN @PV_Loop_OverTimeInMinute>@PV_MaxOTInMin THEN DATEDIFF(MINUTE,@PV_Loop_ShiftEndTime,DATEADD(MINUTE,RAND()*10,DATEADD(MINUTE,@PV_MaxOTInMin,@PV_Loop_ShiftEndTime))) ELSE  @PV_Loop_OverTimeInMinute END
			SET @PV_Loop_InTime = CASE WHEN @PV_MinInTime IS NOT NULL AND @PV_Loop_InTime<@PV_MinInTime THEN DATEADD(MINUTE,RAND()*(10),@PV_MinInTime) ELSE @PV_Loop_InTime END

			--select @PV_Loop_AttendanceDate,@PV_MinInTime
			INSERT INTO MaxOTConfigurationAttendance 
			SELECT @PV_Loop_EmployeeID,@Param_MOCID,@PV_Loop_AttendanceDate,@PV_Loop_ShiftEndTime/*DATEADD(MINUTE,RAND()*(-10),@PV_Loop_ShiftEndTime)*/--DATEADD(MINUTE,RAND()*5,DATEADD(MINUTE,@PV_MaxOTInMin-5,@PV_Loop_ShiftEndTime))
			
			,@PV_OverTime
			,@PV_Loop_InTime
			
			--Asad
			,ISNULL(@PV_Loop_IsDayOff, 0)
			,ISNULL(@PV_Loop_IsHoliday, 0)
			,ISNULL(@PV_Loop_IsLeave, 0)
			,ISNULL(@PV_Loop_LeaveHeadID, 0)
		END 
	FETCH NEXT FROM Cur_CCC INTO @PV_Loop_EmployeeID,@PV_Loop_AttendanceDate,@PV_Loop_OutTime,/*@PV_Loop_ShiftEndTime,*/@PV_Loop_OverTimeInMinute,@PV_Loop_InTime, @PV_Loop_IsDayOff, @PV_Loop_IsHoliday, @PV_Loop_IsLeave, @PV_Loop_LeaveHeadID, @PV_Loop_ShiftID
	END								
	CLOSE Cur_CCC
	DEALLOCATE Cur_CCC
	
	DECLARE @Count INT = (SELECT COUNT(*) FROM #tbl_EmpID)
	SET @Param_Index=@Param_Index+@Count  

	UPDATE #tbl_TimeCard SET OutTime= ISNULL((SELECT OutTime FROM MaxOTConfigurationAttendance WHERE MOCID =@Param_MOCID AND AttendanceDate=#tbl_TimeCard.AttendanceDate AND EmployeeID=#tbl_TimeCard.EmployeeID),OutTime)
	UPDATE #tbl_TimeCard SET OverTimeInMinute=ISNULL((SELECT OverTimeInMin FROM MaxOTConfigurationAttendance WHERE MOCID =@Param_MOCID AND AttendanceDate=#tbl_TimeCard.AttendanceDate AND EmployeeID=#tbl_TimeCard.EmployeeID),OverTimeInMinute)
	UPDATE #tbl_TimeCard SET InTime= ISNULL((SELECT InTime FROM MaxOTConfigurationAttendance WHERE MOCID =@Param_MOCID AND AttendanceDate=#tbl_TimeCard.AttendanceDate AND EmployeeID=#tbl_TimeCard.EmployeeID),InTime)
	
	UPDATE #tbl_TimeCard SET LeaveName= (SELECT ShortName FROM LeaveHead WHERE LeaveHeadID=#tbl_TimeCard.LeaveHeadID) WHERE LeaveHeadID>0
	UPDATE #tbl_TimeCard SET TotalWorkingHourInMinute= DATEDIFF(MINUTE,InTime,OutTime)+1 WHERE CAST(InTime AS TIME(0))!='00:00:00' AND CAST(OutTime AS TIME(0))!='00:00:00'

	IF @PV_IsPresentOnDayOff=0
	BEGIN
		UPDATE #tbl_TimeCard SET InTime=CONVERT(DATE,InTime),OutTime=CONVERT(DATE,OutTime),TotalWorkingHourInMinute=0,OverTimeInMinute=0
		WHERE IsDayOff=1
	END ELSE BEGIN
		UPDATE #tbl_TimeCard SET IsDayOff=0
		WHERE IsDayOff=1
	END

	IF @PV_IsPresentOnHoliday=0
	BEGIN
		UPDATE #tbl_TimeCard SET InTime=CONVERT(DATE,InTime),OutTime=CONVERT(DATE,OutTime),TotalWorkingHourInMinute=0,OverTimeInMinute=0
		WHERE IsHoliday=1
	END ELSE BEGIN
		UPDATE #tbl_TimeCard SET IsDayOff=0
		WHERE IsHoliday=1
	END
	--IF NOT EXISTS (SELECT * FROM #tbl_EmpID WHERE RowID>@Param_Index)
	--BEGIN
	--	SET @Param_Index=0
	--END	


	IF NOT EXISTS(SELECT top(200)aa.Row,aa.EmployeeID FROM(SELECT ROW_NUMBER() OVER(ORDER BY EmployeeOfficialID) AS Row, * FROM View_EmployeeOfficial WHERE IsActive =1 AND EmployeeID IN (SELECT EmployeeID FROM AttendanceDaily WHERE AttendanceDate BETWEEN 
			@Param_StartDate AND @Param_EndDate))aa WHERE aa.Row>@Param_Index)
	BEGIN
		SET @Param_Index=0
	END


	SELECT @Param_Index

	
	DROP TABLE #tbl_TimeCard
	DROP TABLE #tbl_EmpID

END

GO
/****** Object:  StoredProcedure [dbo].[SP_RPT_AMG_SalarySheet_Comp_AsPerEdit]    Script Date: 11/20/2018 12:14:40 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[SP_RPT_AMG_SalarySheet_Comp_AsPerEdit]
	@Param_BUIDs AS varchar(50)
	,@Param_LocationIDs AS VARCHAR (100)
	,@Param_DepartmentIDs AS Varchar(1000)
	,@Param_DesignationIDs AS Varchar(1000)
	,@Param_SalarySchemeIDs AS Varchar(50)
	,@Param_EmployeeIDs as varchar(100)
	,@Param_MonthID AS int--Mandatory
	,@Param_Year as int--Mandatory
	,@Param_IsNewJoin as bit
	,@Param_IsOutSheet as int
	,@Param_MinSalary as decimal (18,2)
	,@Param_MaxSalary as decimal (18,2)
	,@Param_sGroupIDs VARCHAR(MAX)
	,@Param_sBlockIDs VARCHAR(MAX)
	,@Param_UserID INT
	,@Param_TimeCardID INT
AS
BEGIN
	--SET @Param_MonthID=8
	--SET @Param_Year=2017
	--SET @Param_BUIDs=''
	--SET @Param_LocationIDs=''
	--SET @Param_DepartmentIDs=''
	--SET @Param_DesignationIDs=''
	--SET @Param_EmployeeIDs=''
	--SET @Param_IsNewJoin=0
	--SET @Param_MinSalary=10000
	--SET @Param_MaxSalary=15000

	IF OBJECT_ID('tempdb..#tbl_AMG_EmpID') IS NOT NULL
	BEGIN
		DROP TABLE #tbl_AMG_EmpID
	END
	IF OBJECT_ID('tempdb..#tbl_AMG_EmpBasic') IS NOT NULL
	BEGIN
		DROP TABLE #tbl_AMG_EmpBasic
	END
	IF OBJECT_ID('tempdb..#tbl_AMG_EmpOff') IS NOT NULL
	BEGIN
		DROP TABLE #tbl_AMG_EmpOff
	END
	IF OBJECT_ID('tempdb..#tbl_AMG_AttDaily') IS NOT NULL
	BEGIN
		DROP TABLE #tbl_AMG_AttDaily
	END
		IF OBJECT_ID('tempdb..#tbl_AMG_ESD') IS NOT NULL
	BEGIN
		DROP TABLE #tbl_AMG_ESD
	END
	IF OBJECT_ID('tempdb..#tbl_AMG_SalarySheet') IS NOT NULL
	BEGIN
		DROP TABLE #tbl_AMG_SalarySheet
	END

	CREATE TABLE #tbl_AMG_EmpID (EmployeeID int,ESID int)
	CREATE TABLE #tbl_AMG_EmpBasic(EmployeeID int,Code varchar(100),Name varchar(100),AccountNo varchar(100),BankName varchar (100),EmpNameInBangla nvarchar(100))
	CREATE TABLE #tbl_AMG_EmpOff(EmployeeID int,DOJ DATE,Grade Varchar(50))
	CREATE TABLE #tbl_AMG_AttDaily(EmployeeID int,LeaveID int)
	CREATE TABLE #tbl_AMG_ESD(ESID int,SalaryHeadID int,SType smallint,Amount Decimal(18,2))	

	CREATE TABLE #tbl_AMG_SalarySheet (	EmployeeSalaryID int,EmployeeID int,BUID int,LocationID int,DepartmentID int,DesignationID int
	,TotalDays int /*Att record*/,DayOffHoliday int,EWD int
	,LWP int/*Leave without Pay*/,CL int/*Casual Leave*/,EL int/*Earn Leave*/,SL int/*Sick Liave*/,ML int/*Maternity Leave*/,A int/*Absent*/,P int/*Present*/
	,Gross decimal(18,2),Basics decimal (18,2),HR decimal(18,2)/*House Rent*/,Med decimal(18,2)/* Medical*/,Food decimal(18,2)/*Food*/,Conv decimal(18,2)/*Conveyance*/
	,Earning decimal(18,2),AttBonus decimal(18,2),OT_HR decimal(18,2)/*OverTime Hour*/,OT_Rate decimal(18,2),OT_Amount decimal(18,2),GrossEarning decimal (18,2)
	,AbsentAmount decimal (18,2),Advance decimal(18,2),Stemp decimal(18,2),TotalDeduction decimal(18,2),NetAmount decimal (18,2), GroupByID int)

	DECLARE
	@PV_SQL as varchar(max)
	,@PV_StartDate As DATE
	,@PV_EndDate AS DATE
	SET @PV_SQL=''

	IF NOT EXISTS (SELECT * FROM CompliancePayrollProcessManagement WHERE MonthID=@Param_MonthID AND DATEPART(YEAR,SalaryTo)=@Param_Year AND MOCID=@Param_TimeCardID)
	BEGIN
		ROLLBACK
			RAISERROR (N'Invalid Year & Month',16,1)
		RETURN
	END

	--Get Start Date & End Date
	SELECT top(1)@PV_StartDate=SalaryFrom,@PV_EndDate=SalaryTo 
	FROM CompliancePayrollProcessManagement WHERE MonthID=@Param_MonthID AND DATEPART(YEAR,SalaryTo)=@Param_Year AND MOCID=@Param_TimeCardID

	SET @PV_SQL='SELECT EmployeeID,EmployeeSalaryID FROM ComplianceEmployeeSalary WHERE PayrollProcessID IN(SELECT PPMID FROM CompliancePayrollProcessManagement WHERE MonthID='+CONVERT(VARCHAR(512),@Param_MonthID)+' AND DATEPART(YEAR,SalaryTo)='+CONVERT(VARCHAR(512),@Param_Year)+' AND MOCID='+CONVERT(VARCHAR(512),@Param_TimeCardID)+') AND StartDate='''+CONVERT(VARCHAR(50),@PV_StartDate)+''' AND EndDate='''+CONVERT(VARCHAR(50),@PV_EndDate)+''''
	
	IF (@Param_EmployeeIDs IS NOT NULL AND @Param_EmployeeIDs <>'')
	BEGIN
		SET @PV_SQL=@PV_SQL+ ' AND EmployeeID IN ('+@Param_EmployeeIDs+')'
	END

	IF (@Param_LocationIDs IS NOT NULL AND @Param_LocationIDs <>'')
	BEGIN
		SET @PV_SQL=@PV_SQL+ ' AND LocationID IN ('+@Param_LocationIDs+')'
	END

	IF (@Param_DepartmentIDs IS NOT NULL AND @Param_DepartmentIDs <>'')
	BEGIN
		SET @PV_SQL=@PV_SQL+ ' AND DepartmentID IN ('+@Param_DepartmentIDs+')'
	END

	IF (@Param_DesignationIDs IS NOT NULL AND @Param_DesignationIDs <>'')
	BEGIN
		SET @PV_SQL=@PV_SQL+ ' AND DesignationID IN ('+@Param_DesignationIDs+')'
	END

	IF @Param_MinSalary>0 AND @Param_MaxSalary>0
	BEGIN
		SET @PV_SQL=@PV_SQL+ ' AND GrossAmount BETWEEN '+CONVERT(VARCHAR(50),@Param_MinSalary)+' AND '+CONVERT(VARCHAR(50),@Param_MaxSalary)+''
	END

	IF (@Param_BUIDs IS NOT NULL AND @Param_BUIDs<>'')
	BEGIN
		SET @PV_SQL=@PV_SQL+ ' AND PayrollProcessID IN (SELECT PPMID FROM CompliancePayrollProcessManagement WHERE MOCID='+CONVERT(VARCHAR(512),@Param_TimeCardID)+' AND BusinessUnitID IN ('+@Param_BUIDs+'))'
	END

	IF (@Param_SalarySchemeIDs IS NOT NULL AND @Param_SalarySchemeIDs <>'')
	BEGIN
		SET @PV_SQL=@PV_SQL+ ' AND EmployeeID IN (SELECT EmployeeID FROM EmployeeSalaryStructure WHERE SalarySchemeID IN ('+@Param_SalarySchemeIDs+'))'
	END

	IF (@Param_IsNewJoin IS NOT NULL AND @Param_IsNewJoin <>0)
	BEGIN
		SET @PV_SQL=@PV_SQL+ ' AND EmployeeID IN (SELECT EmployeeID FROM EmployeeOfficial WHERE DateOfJoin BETWEEN '''+CONVERT(VARCHAR(50),@PV_StartDate)+''' AND '''+CONVERT(VARCHAR(50),@PV_EndDate)+''')'
	END
	IF @Param_sGroupIDs<>'' AND @Param_sGroupIDs IS NOT NULL
	BEGIN
		SET @PV_SQL=@PV_SQL+' AND EmployeeID IN( SELECT EmployeeID From View_EmployeeGroup WHERE EmployeeTypeID IN(' + @Param_sGroupIDs + '))'
	END
	IF @Param_sBlockIDs<>'' AND @Param_sBlockIDs IS NOT NULL
	BEGIN
		SET @PV_SQL=@PV_SQL+' AND EmployeeID IN( SELECT EmployeeID From View_EmployeeGroup WHERE EmployeeTypeID IN(' + @Param_sBlockIDs + '))'
	END
	IF EXISTS(SELECT * FROM Users WHERE userID = @Param_UserId AND FinancialUserType!=1)
	BEGIN
			SET @PV_SQL = @PV_SQL + ' AND DepartmentID IN(SELECT DepartmentID FROM DepartmentRequirementPolicy WHERE DepartmentRequirementPolicyID IN(SELECT DRPID FROM DepartmentRequirementPolicyPermission WHERE UserID ='  + CONVERT(VARCHAR(50),@Param_UserId)+'))'
	END

	--SELECT @PV_SQL

	--Get EmployeeID
	INSERT INTO #tbl_AMG_EmpID
	EXEC(@PV_SQL)

	--Get EmpBAsic Info
	INSERT INTO #tbl_AMG_EmpBasic
	SELECT EmployeeID,Code,Name 
	,(SELECT top(1)AccountNo FROM EmployeeBankAccount WHERE EmployeeID=Employee.EmployeeID AND IsActive=1)
	,(SELECT top(1)BankBranchName FROM View_EmployeeBankAccount WHERE EmployeeID=Employee.EmployeeID AND IsActive=1)
	,NameInBangla
	FROM Employee WHERE EmployeeID IN (
	SELECT EmployeeID FROM #tbl_AMG_EmpID)
	--SELECT * FROM #tbl_EmpBasic


	--Get Official Info
	INSERT INTO #tbl_AMG_EmpOff
	SELECT EmployeeID,DateOfJoin
	,(SELECT Name FROM EmployeeType WHERE EmployeeTypeID=EmployeeOfficial.EmployeeTypeID)		
	FROM EmployeeOfficial WHERE EmployeeID IN (
	SELECT EmployeeID FROM #tbl_AMG_EmpID)
	--SELECT * FROM #tbl_AMG_EmpOff

	--Get Attendance
	
	INSERT INTO #tbl_AMG_AttDaily
	SELECT EmployeeID,LeaveHeadID FROM MaxOTConfigurationAttendance WHERE AttendanceDate BETWEEN @PV_StartDate AND @PV_EndDate AND MOCID=@Param_TimeCardID AND EmployeeID IN (
	SELECT EmployeeID FROM #tbl_AMG_EmpID)
	

	--SELECT * FROM #tbl_AMG_AttDaily

	--Get Employee Salary
	
		INSERT INTO #tbl_AMG_SalarySheet 
		SELECT EmployeeSalaryID,EmployeeID
			,(SELECT BusinessUnitID FROM CompliancePayrollProcessManagement WHERE PPMID=ComplianceEmployeeSalary.PayrollProcessID AND MOCID=@Param_TimeCardID)--BUID
			,LocationID,DepartmentID,DesignationID		
			,0/*TotalDays*/
			,ISNULL(TotalDayOff,0)+ISNULL(TotalHoliday,0)/*DayOffHoliday*/
			,0/*EWD*/,0,0,0,0,0
			,ISNULL(TotalAbsent,0)
			,ISNULL(TotalWorkingDay,0)-ISNULL(TotalAbsent,0)-ISNULL(TotalPLeave,0)-ISNULL(TotalUpLeave, 0)
			--SET @PV_TotalDaysOfAbsent=@PV_TotalWorkingDay-@PV_TotalPresentDay-@PV_TotalPLeave-@PV_TotalUPLeave-@PV_ResignDay;
			,GrossAmount,0,0,0,0,0,0,0/*Earning*/
			,OTHour,OTRatePerHour,ROUND(OTHour*OTRatePerHour,2)
			,0/*GrossEarning*/,0,0,0,0,NetAmount
			,0 /*GroupByID*/
		FROM ComplianceEmployeeSalary WHERE EmployeeSalaryID IN (SELECT ESID FROM #tbl_AMG_EmpID)
	
	--StartDate=@PV_StartDate AND EndDate=@PV_EndDate


	--Get Employee SalaryDetail	
	
	
		INSERT INTO #tbl_AMG_ESD 
		SELECT EmployeeSalaryID,SalaryHeadID
		,(SELECT SalaryHeadType FROM SalaryHead WHERE SalaryHeadID=ComplianceEmployeeSalaryDetail.SalaryHeadID)
		,ISNULL(Amount,0)
		FROM ComplianceEmployeeSalaryDetail WHERE EmployeeSalaryID IN (
		SELECT EmployeeSalaryID FROM #tbl_AMG_SalarySheet)
	

	--Update SAlary Sheet

	UPDATE #tbl_AMG_SalarySheet 
	SET TotalDays=(SELECT COUNT(*) FROM #tbl_AMG_AttDaily WHERE EmployeeID=#tbl_AMG_SalarySheet.EmployeeID)
	
	,LWP=(SELECT COUNT(*) FROM #tbl_AMG_AttDaily WHERE EmployeeID=#tbl_AMG_SalarySheet.EmployeeID AND LeaveID=4)
	,CL=(SELECT COUNT(*) FROM #tbl_AMG_AttDaily WHERE EmployeeID=#tbl_AMG_SalarySheet.EmployeeID AND LeaveID=1)
	,EL=(SELECT COUNT(*) FROM #tbl_AMG_AttDaily WHERE EmployeeID=#tbl_AMG_SalarySheet.EmployeeID AND LeaveID=3)
	,SL=(SELECT COUNT(*) FROM #tbl_AMG_AttDaily WHERE EmployeeID=#tbl_AMG_SalarySheet.EmployeeID AND LeaveID=2)--Medical Leave
	,Basics=ISNULL((SELECT Amount FROM #tbl_AMG_ESD WHERE ESID=#tbl_AMG_SalarySheet.EmployeeSalaryID AND SalaryHeadID=1),0)
	,HR=ISNULL((SELECT Amount FROM #tbl_AMG_ESD WHERE ESID=#tbl_AMG_SalarySheet.EmployeeSalaryID AND SalaryHeadID=2),0)
	,Med=ISNULL((SELECT Amount FROM #tbl_AMG_ESD WHERE ESID=#tbl_AMG_SalarySheet.EmployeeSalaryID AND SalaryHeadID=5),0)
	,Food=ISNULL((SELECT Amount FROM #tbl_AMG_ESD WHERE ESID=#tbl_AMG_SalarySheet.EmployeeSalaryID AND SalaryHeadID=4),0)
	,Conv=ISNULL((SELECT Amount FROM #tbl_AMG_ESD WHERE ESID=#tbl_AMG_SalarySheet.EmployeeSalaryID AND SalaryHeadID=3),0)
	,AttBonus=ISNULL((SELECT Amount FROM #tbl_AMG_ESD WHERE ESID=#tbl_AMG_SalarySheet.EmployeeSalaryID AND SalaryHeadID=7),0)
	,AbsentAmount=ISNULL((SELECT Amount FROM #tbl_AMG_ESD WHERE ESID=#tbl_AMG_SalarySheet.EmployeeSalaryID AND SalaryHeadID=8),0)
	,Advance=ISNULL((SELECT Amount FROM #tbl_AMG_ESD WHERE ESID=#tbl_AMG_SalarySheet.EmployeeSalaryID AND SalaryHeadID=11),0)
	,Stemp=ISNULL((SELECT  Amount FROM #tbl_AMG_ESD WHERE ESID=#tbl_AMG_SalarySheet.EmployeeSalaryID AND SalaryHeadID=10),0)
	,Earning=ISNULL((SELECT SUM(ISNULL(Amount,0)) FROM #tbl_AMG_ESD WHERE ESID=#tbl_AMG_SalarySheet.EmployeeSalaryID AND SType=1),0)
	,GrossEarning=ISNULL((SELECT SUM(ISNULL(Amount,0)) FROM #tbl_AMG_ESD WHERE ESID=#tbl_AMG_SalarySheet.EmployeeSalaryID AND SType IN(1,2)),0)
				  +ISNULL(OT_Amount,0)
	,TotalDeduction=ISNULL((SELECT SUM(ISNULL(Amount,0)) FROM #tbl_AMG_ESD WHERE ESID=#tbl_AMG_SalarySheet.EmployeeSalaryID AND SType IN(3)),0)
	
	UPDATE #tbl_AMG_SalarySheet SET EWD=DayOffHoliday+CL+EL+SL+ML+P+LWP 


	CREATE TABLE #TempTableGroupBy (
										GroupByID int identity(1,1),
										BUID int,
										LocID int,
										DeptID int	
								   )

	INSERT INTO #TempTableGroupBy(BUID, LocID, DeptID)
	SELECT HH.BUID, HH.LocationID, HH.DepartmentID FROM #tbl_AMG_SalarySheet AS HH GROUP BY HH.BUID, HH.LocationID, HH.DepartmentID  ORDER BY HH.BUID, HH.LocationID, HH.DepartmentID ASC

	UPDATE #tbl_AMG_SalarySheet 
	SET GroupByID = (SELECT MM.GroupByID FROM #TempTableGroupBy AS MM WHERE MM.BUID=HH.BUID AND MM.LocID=HH.LocationID AND MM.DeptID=HH.DepartmentID)
	FROM #tbl_AMG_SalarySheet AS HH

	SELECT ASS.*
	,EmpB.Code,EmpB.Name 
	,EmpO.DOJ,EmpO.Grade
	,BU.Name AS BUName
	,Loc.Name AS LocName
	,Dpt.Name AS DptName
	,Dsg.Name AS DsgName
	,EmpB.AccountNo
	,EmpB.BankName
	,EmpB.EmpNameInBangla
	,Dsg.NameInBangla AS DsgNameInBangla
	,BU.[Address] AS BUAddress
	FROM #tbl_AMG_SalarySheet ASS
	INNER JOIN #tbl_AMG_EmpBasic EmpB ON EmpB.EmployeeID=ASS.EmployeeID
	INNER JOIN #tbl_AMG_EmpOff EmpO ON EmpO.EmployeeID=ASS.EmployeeID
	INNER JOIN BUsinessUnit BU ON BU.BusinessUnitID=ASS.BUID
	INNER JOIN Location Loc ON Loc.LocationID=ASS.LocationID
	INNER JOIN Department Dpt ON Dpt.DepartmentID=ASS.DepartmentID
	INNER JOIN Designation Dsg ON Dsg.DesignationID=ASS.DesignationID
	


	--SELECT * FROM Designation
	DROP TABLE #tbl_AMG_SalarySheet
	DROP TABLE #tbl_AMG_EmpBasic
	DROP TABLE #tbl_AMG_EmpOff
	DROP TABLE #tbl_AMG_AttDaily
	DROP TABLE #tbl_AMG_EmpID
	DROP TABLE #tbl_AMG_ESD
	DROP TABLE #TempTableGroupBy
END		





GO
/****** Object:  StoredProcedure [dbo].[SP_Rpt_IncrementByPercent]    Script Date: 11/20/2018 12:14:40 PM ******/
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
    @Param_UserID AS INT,
	@Param_Index AS INT
AS
BEGIN
	--SET @Param_EmployeeIDs=''
 --   SET @Param_SalaryHeadID=1
 --   SET @Param_Percent=5
 --   SET @Param_MonthIDs='1,2,3,4,5,6,7,8,9,10,11,12'
 --   SET @Param_YearIDs='2010,2014,2016'
 --   SET @Param_UserID=128

		DECLARE
	@PV_SQL AS NVARCHAR(MAX)
	,@Row_Count INT
	
	IF OBJECT_ID('tempdb..#tbl_EmployeeID') IS NOT NULL
	BEGIN
		DROP TABLE #tbl_EmployeeID
	END
	IF OBJECT_ID('tempdb..#tbl_EmployeeSalaryStructure') IS NOT NULL
	BEGIN
		DROP TABLE #tbl_EmployeeSalaryStructure
	END

	IF OBJECT_ID('tempdb..#tbl_RetVal') IS NOT NULL
	BEGIN
		DROP TABLE #tbl_RetVal
	END

	CREATE TABLE #tbl_RetVal(Amount DECIMAL(18, 2))
	CREATE TABLE #tbl_EmployeeID(EmployeeID INT)
	CREATE TABLE #tbl_EmployeeSalaryStructure(EmployeeID INT, ESSID INT, SalarySchemeID INT, PreviousGrossAmount DECIMAL(18, 2), PreviousBasicAmount DECIMAL(18, 2), IncrementedGrossAmount DECIMAL(18, 2), IncrementedBasicAmount DECIMAL(18, 2), Code VARCHAR(512), Name VARCHAR(512), DesignationID INT, DRPID INT, ASID INT, DOJ DATE, SalarySchemeName VARCHAR(512), IndexNo INT, LocationName VARCHAR(MAX), BUName VARCHAR(MAX))

	SET @PV_SQL=''
	SET @PV_SQL='SELECT TOP(100) tab.EmployeeID FROM (
		SELECT 
			ROW_NUMBER() OVER(ORDER BY EO.EmployeeID) AS Row
			,EO.EmployeeID
		FROM EmployeeOfficial EO
		LEFT JOIN Employee EMP ON EO.EmployeeID = EMP.EmployeeID
		WHERE EO.IsActive = 1 '

	
	IF @Param_EmployeeIDs!='' AND @Param_EmployeeIDs IS NOT NULL
	BEGIN
		SET @PV_SQL=@PV_SQL+' AND EMP.EmployeeID IN ('+@Param_EmployeeIDs+')'
	END	
	IF @Param_MonthIDs!='' AND @Param_MonthIDs IS NOT NULL
	BEGIN
		SET @PV_SQL=@PV_SQL+' AND CONVERT(INT,DATEPART(MONTH,DateOfJoin)) IN ('+@Param_MonthIDs+')'
	END	
	IF @Param_YearIDs!='' AND @Param_YearIDs IS NOT NULL
	BEGIN
		SET @PV_SQL=@PV_SQL+' AND CONVERT(INT,DATEPART(YEAR,DateOfJoin)) IN( '+@Param_YearIDs+')'
	END	
	SET @PV_SQL = @PV_SQL + ')tab WHERE tab.Row > '+CONVERT(VARCHAR(512), @Param_Index)
	--SET @PV_SQL=@PV_SQL+ ' AND CONVERT(INT,DATEPART(MONTH,DateOfJoin)) IN ('+@Param_MonthIDs+') AND CONVERT(INT,DATEPART(YEAR,DateOfJoin)) IN( '+@Param_YearIDs+')'
	
	INSERT INTO #tbl_EmployeeID
	EXEC(@PV_SQL)

	INSERT INTO #tbl_EmployeeSalaryStructure
	SELECT EmpID.EmployeeID
		  ,ESS.ESSID
		  ,ESS.SalarySchemeID
		  ,ESS.ActualGrossAmount 
		  ,(SELECT MAX(ESSD.Amount) FROM EmployeeSalaryStructureDetail ESSD WHERE ESS.ESSID = ESSD.ESSID)
		  ,0 
		  ,0
		  ,EMP.Code
		  ,EMP.Name
		  ,EO.DesignationID
		  ,EO.DRPID
		  ,EO.AttendanceSchemeID
		  ,(SELECT datefromparts(datepart(year, EO.DateOfJoin)+1, datepart(month, EO.DateOfJoin), datepart(day, (SELECT DATEADD(DAY,-DAY(EO.DateOfJoin)+1, CAST(EO.DateOfJoin AS DATETIME))))))--EO.DateOfJoin
		  ,SS.Name
		  ,0
		  ,Loc.Name
		  ,BU.Name
	FROM #tbl_EmployeeID EmpID
	LEFT JOIN EmployeeSalaryStructure ESS ON EmpID.EmployeeID = ESS.EmployeeID
	LEFT JOIN Employee EMP ON EmpID.EmployeeID = EMP.EmployeeID
	LEFT JOIN EmployeeOfficial EO ON EmpID.EmployeeID = EO.EmployeeID
	LEFT JOIN SalaryScheme SS ON ESS.SalarySchemeID = SS.SalarySchemeID
	LEFT JOIN DepartmentRequirementPolicy DRP ON EO.DRPID = DRP.DepartmentRequirementPolicyID
	LEFT JOIN Location Loc ON DRP.LocationID = Loc.LocationID
	LEFT JOIN BusinessUnit BU ON DRP.BusinessUnitID = BU.BusinessUnitID
	WHERE ISNULL(ESS.ESSID, 0)>0

	IF @Param_Percent>0
	BEGIN
		UPDATE #tbl_EmployeeSalaryStructure SET IncrementedBasicAmount = PreviousBasicAmount + ((@Param_Percent * PreviousBasicAmount) / 100)
	END
	
	IF @Param_Percent>0
	BEGIN
		DECLARE
		@EmployeeID as int,
		@IncrementedGrossAmount decimal(18,2)
		,@IncrementedBasicAmount decimal(18, 2)

		DECLARE Cur_AB1 CURSOR LOCAL FORWARD_ONLY KEYSET FOR
		SELECT HH.EmployeeID, IncrementedBasicAmount FROM #tbl_EmployeeSalaryStructure AS HH ORDER BY HH.EmployeeID
		OPEN Cur_AB1
		FETCH NEXT FROM Cur_AB1 INTO  @EmployeeID, @IncrementedBasicAmount
		WHILE(@@Fetch_Status <> -1)
		BEGIN
			SET @IncrementedGrossAmount =0
			INSERT INTO #tbl_RetVal(Amount)
			EXEC [dbo].[SP_GetEmployeeBasicAmount] @EmployeeID, 1, @IncrementedBasicAmount--0=Basic, 1=Gross
			UPDATE #tbl_EmployeeSalaryStructure SET IncrementedGrossAmount = (SELECT Amount FROM #tbl_RetVal)  WHERE EmployeeID = @EmployeeID
			DELETE FROM #tbl_RetVal
			FETCH NEXT FROM Cur_AB1 INTO  @EmployeeID, @IncrementedBasicAmount
		END
		CLOSE Cur_AB1
		DEALLOCATE Cur_AB1
	END

	
	SET @Row_Count = (SELECT COUNT(*) FROM #tbl_EmployeeSalaryStructure)

	SET @Param_Index = @Param_Index + @Row_Count

	UPDATE #tbl_EmployeeSalaryStructure SET IndexNo = @Param_Index

	SELECT * FROM #tbl_EmployeeSalaryStructure
	
	DROP TABLE #tbl_EmployeeID
	DROP TABLE #tbl_EmployeeSalaryStructure
	DROP TABLE #tbl_RetVal
END
GO
/****** Object:  StoredProcedure [dbo].[SP_RPT_TimeCard_MaxOTCon]    Script Date: 11/20/2018 12:14:40 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[SP_RPT_TimeCard_MaxOTCon]
--DECLARE
	@Param_EmployeeIDs as varchar(50)
	,@Param_StartDate AS DATE--Mandatory
	,@Param_EndDate As DATE--Mandatory
	,@Param_LocationIDs varchar(50)
	,@Param_DepartmentIDs varchar(500)
	,@Param_BUIDs varchar(50)
	--,@Param_DesignationIDs varchar(500)
	,@Param_MinSalary AS decimal(18,2)
	,@Param_MaxSalary AS decimal(18,2)
	,@Param_BlockIDs as varchar (500)
	,@Param_GroupIDs as varchar (500)
	,@Param_MOCID int--Mandatory
	,@Param_UserID INT

	--SET @Param_EmployeeIDs='2384'
	--SET @Param_StartDate='01 June 2018'
	--SET @Param_EndDate='30 June 2018'
	--SET @Param_BUIDs=''
	--SET @Param_LocationIDs=''
	--SET @Param_DepartmentIDs=''
	----SET @Param_DesignationIDs=''
	--SET @Param_BlockIDs=''
	--SET @Param_MinSalary=0
	--SET @Param_MaxSalary=0
	--SET @Param_MOCID=3
AS
BEGIN
	IF OBJECT_ID('tempdb..#tbl_TimeCard') IS NOT NULL
	BEGIN
		DROP TABLE #tbl_TimeCard
	END
	IF OBJECT_ID('tempdb..#tbl_EmpBasic') IS NOT NULL
	BEGIN
		DROP TABLE #tbl_EmpBasic
	END

	DECLARE 
	@PV_MaxOTInMin as int
	,@PV_MaxOutTime AS DATETIME
	,@PV_MinInTime AS DateTime
	,@PV_IsPresentOnDayOff bit
	,@PV_IsPresentOnHoliday bit
	,@PV_SQL as varchar (max)
	,@PV_Loop_EmployeeID as int
	,@PV_Loop_OutTime as dateTime
	,@PV_Loop_InTime as dateTime
	,@PV_Loop_AttendanceDate Date
	,@PV_Loop_ShiftEndTime AS DATETIME
	,@PV_Loop_OverTimeInMinute AS int
	,@PV_Loop_IsDayOff as bit
	,@PV_Loop_IsHoliday as bit
	,@PV_Loop_IsLeave as bit
	,@PV_Loop_LeaveHeadID INT
	,@PV_Loop_ShiftID INT

	SELECT @PV_MaxOTInMin=ISNULL(MaxOTInMin,0)
	,@PV_MaxOutTime=MaxOutTime
	,@PV_MinInTime=MinInTime
	,@PV_IsPresentOnDayOff=ISNULL(IsPresentOnDayOff,0)
	,@PV_IsPresentOnHoliday=ISNULL(ISPresentOnHoliday,0) 
	FROM MaxOTConfiguration WHERE MOCID=@Param_MOCID

	SET @PV_MaxOTInMin=ISNULL((SELECT MaxOTInMin FROM MaxOTConfiguration WHERE MOCID=@Param_MOCID),0)
	SET @PV_SQL=''

	CREATE TABLE #tbl_TimeCard(AttendanceID int,EmployeeID int,AttendanceDate DATE,InTime DATETIME
							 ,OutTime DATETIME,TotalWorkingHourInMinute int,OverTimeInMinute int,ShiftID int, ShiftName VARCHAR(512),ShiftEndTime DateTime
							 ,LeaveHeadID int,LeaveName VARCHAR(100),IsOSD BIT,IsDayOff BIT, IsHoliday BIT, IsLeave BIT)
	CREATE CLUSTERED INDEX tbl_TimeCard_AttendanceID ON #tbl_TimeCard(AttendanceID)

	CREATE TABLE #tbl_EmpBasic(EmployeeID int,EmployeeCode VARCHAR(100),EmployeeName VARCHAR(100),LocationName VARCHAR(100),DepartmentName VARCHAR(100)
							  ,DesignationName VARCHAR(100),JoiningDate DATE, BUName VARCHAR(200),BUAddress VARCHAR(200))
	CREATE CLUSTERED INDEX tbl_EmpBasic_EmployeeID ON #tbl_EmpBasic(EmployeeID)


	IF @Param_EmployeeIDs<>'' AND @Param_EmployeeIDs IS NOT NULL
	BEGIN
		SET @PV_SQL=@PV_SQL+'  SELECT  EmployeeID, Code, Name, LocationName,DepartmentName,DesignationName,DateOfJoin, BUName
		,(SELECT Address FROM BusinessUnit WHERE BusinessUnitID = View_EmployeeOfficialALL.BusinessUnitID)
			FROM View_EmployeeOfficialALL WHERE EmployeeID IN ('+@Param_EmployeeIDs+')'
	END ELSE BEGIN
		SET @PV_SQL=@PV_SQL+'  SELECT  EmployeeID, Code, Name, LocationName,DepartmentName,DesignationName, DateOfJoin, BUName
		,(SELECT Address FROM BusinessUnit WHERE BusinessUnitID = View_EmployeeOfficial.BusinessUnitID)
			FROM View_EmployeeOfficial WHERE EmployeeID IN (SELECT EmployeeID FROM AttendanceDaily WHERE AttendanceDate BETWEEN 
			'''+CONVERT(VARCHAR(50),@Param_StartDate)+''' AND '''+CONVERT(VARCHAR(50),@Param_EndDate)+''') '
	END

	IF @Param_LocationIDs<>'' AND @Param_LocationIDs IS NOT NULL 
	BEGIN
		SET @PV_SQL=@PV_SQL+' AND  LocationID IN('+ @Param_LocationIDs+')'
	END
	IF @Param_DepartmentIds<>'' AND @Param_DepartmentIds IS NOT NULL
	BEGIN
		SET @PV_SQL=@PV_SQL+' AND  DepartmentID IN('+ @Param_DepartmentIds+')'
	END
	IF @Param_BUIds<>'' AND @Param_BUIds IS NOT NULL
	BEGIN
		SET @PV_SQL=@PV_SQL+' AND  BusinessUnitID IN('+ @Param_BUIds+')'
	END
	IF @Param_MinSalary > 0 AND @Param_MaxSalary > 0
	BEGIN
		SET @PV_SQL=@PV_SQL+' AND EmployeeID IN (SELECT EmployeeID FROM EmployeeSalaryStructure WHERE GrossAmount BETWEEN ' + CONVERT(varchar(50),@Param_MinSalary) + ' AND ' + CONVERT(varchar(50),@Param_MaxSalary)+')'
	END

	
	IF @Param_GroupIDs IS NOT NULL AND @Param_GroupIDs <>''
	BEGIN
		IF (@Param_BlockIDs IS NOT NULL AND @Param_BlockIDs <>'')
		BEGIN
			SET @Param_BlockIDs=@Param_BlockIDs+','+@Param_GroupIDs
		END ELSE BEGIN
			SET @Param_BlockIDs=@Param_GroupIDs
		END
	END

	IF (@Param_BlockIDs IS NOT NULL AND @Param_BlockIDs <>'')
	BEGIN
		SET @PV_SQL=@PV_SQL+ ' AND EmployeeID IN (SELECT EmployeeID FROM EmployeeGroup WHERE EmployeeTypeID IN('+@Param_BlockIDs+'))'
	END

	IF EXISTS(SELECT * FROM Users WHERE userID = @Param_UserId AND FinancialUserType!=1)
	BEGIN
			SET @PV_SQL = @PV_SQL + ' AND  DepartmentID IN(SELECT DepartmentID FROM DepartmentRequirementPolicy WHERE DepartmentRequirementPolicyID IN(SELECT DRPID FROM DepartmentRequirementPolicyPermission WHERE UserID ='  + CONVERT(VARCHAR(50),@Param_UserId)+'))'
	END

	--SELECT @PV_SQL
	--Distinct Empolyee	
	INSERT INTO #tbl_EmpBasic
	EXEC(@PV_SQL)

	--Get AttendanceRecord
	INSERT INTO #tbl_TimeCard
	SELECT AttendanceID,EmployeeID,AttendanceDate,InTime,OutTime,0/*TotalWorkingHourInMinute*/,OverTimeInMinute,AD.ShiftID, HS.Name
	,NULL/*Shift End Time*/,LeaveHeadID,''/*Leave*/,IsOSD,IsDayOff,IsHoliday, IsLeave
	FROM AttendanceDaily AD
	LEFT JOIN HRM_Shift HS ON HS.ShiftID = AD.ShiftID
	WHERE AttendanceDate BETWEEN @Param_StartDate AND @Param_EndDate AND EmployeeID IN (
	SELECT EmployeeID FROM #tbl_EmpBasic)		



	DECLARE 
	@PV_AttendanceID INT
	,@PV_ActualStart DATETIME
	,@PV_ActualEnd DATETIME
	,@PV_LateInMunit INT
	,@PV_EarlyInMunit INT
	,@PV_StartTime DATETIME
	,@PV_EndTime DATETIME
	,@PV_TolaranceTime as Datetime--int
	,@PV_DayEndTime as DATETIME--time(0)
	,@PV_DayStartTime as DATETIME--time(0)
	,@PV_Remark as varchar(50)
	
	,@PV_Shift_IsOT as bit
	,@PV_Shift_OTStart as datetime
	,@PV_Shift_OTEnd   as datetime
	,@PV_Shift_ISOTOnActual as bit
	,@PV_Shift_OTCalclateAfterInMin as int
	,@PV_Shift_IsLeaveOnOFFHoliday as bit
	,@PV_ShiftID as int
	,@PV_ToleranceEarly as int
	--ASAD
	,@PV_IsHalfDayoff AS BIT
	,@PV_PStart AS DATETIME
	,@PV_PEnd AS DATETIME
	,@PV_IsLeave BIT

	,@PV_OverTime INT

	,@PV_ShiftEndTime DATETIME


	--Cursor for insert data

	DECLARE Cur_CCC CURSOR LOCAL FORWARD_ONLY KEYSET FOR
		SELECT EmployeeID,AttendanceDate,OutTime,/*ShiftEndTime,*/OverTimeInMinute,InTime, IsDayOff, IsHoliday, IsLeave, LeaveHeadID,ShiftID FROM #tbl_TimeCard /*WHERE OutTime>DATEADD(MINUTE,@PV_MaxOTInMin,ShiftEndTime) AND EmployeeID NOT IN (
		SELECT EmployeeID FROM MaxOTConfigurationAttendance WITH (NOLOCK) WHERE MOCID =@Param_MOCID AND AttendanceDate=#tbl_TimeCard.AttendanceDate AND OutTime IS NOT NULL) 
		AND (CAST(InTime AS TIME(0))!='00:00:00' AND CAST(OutTime AS TIME(0))!='00:00:00')*/	
	OPEN Cur_CCC
	FETCH NEXT FROM Cur_CCC INTO  @PV_Loop_EmployeeID,@PV_Loop_AttendanceDate,@PV_Loop_OutTime,/*@PV_Loop_ShiftEndTime,*/@PV_Loop_OverTimeInMinute,@PV_Loop_InTime, @PV_Loop_IsDayOff, @PV_Loop_IsHoliday, @PV_Loop_IsLeave, @PV_Loop_LeaveHeadID, @PV_Loop_ShiftID
	WHILE(@@Fetch_Status <> -1)		
	BEGIN	

		
		

		IF NOT EXISTS (SELECT * FROM MaxOTConfigurationAttendance WITH (NOLOCK) WHERE MOCID =@Param_MOCID AND EmployeeID=@PV_Loop_EmployeeID AND AttendanceDate=@PV_Loop_AttendanceDate)
		BEGIN
			SET @PV_ShiftEndTime= (SELECT EndTime FROM HRM_Shift WHERE ShiftID=@PV_Loop_ShiftID)
			IF @PV_MinInTime IS NOT NULL
			BEGIN
				SET @PV_Loop_InTime=(DATEADD(SECOND,0,DATEADD (MINUTE,DATEPART(MINUTE,@PV_MinInTime), DATEADD(HOUR, DATEPART(HOUR,@PV_MinInTime), DATEADD(dd, DATEDIFF(dd, -0, @PV_Loop_AttendanceDate), 0)))))
			END ELSE
			BEGIN
				SET @PV_Loop_InTime=(DATEADD(SECOND,0,DATEADD (MINUTE,DATEPART(MINUTE,@PV_Loop_InTime), DATEADD(HOUR, DATEPART(HOUR,@PV_Loop_InTime), DATEADD(dd, DATEDIFF(dd, -0, @PV_Loop_AttendanceDate), 0)))))
				
			END


			IF @PV_MaxOutTime IS NULL
			BEGIN
				SET @PV_Loop_ShiftEndTime = (DATEADD(SECOND,0,DATEADD (MINUTE,DATEPART(MINUTE,@PV_ShiftEndTime), DATEADD(HOUR, DATEPART(HOUR,@PV_ShiftEndTime), DATEADD(dd, DATEDIFF(dd, -0, @PV_Loop_InTime), 0)))))
				IF EXISTS(SELECT * FROM AttendanceDaily WHERE EmployeeID=@PV_Loop_EmployeeID AND AttendanceDate=@PV_Loop_AttendanceDate AND OverTimeInMinute>0)
				BEGIN
					SET @PV_Loop_ShiftEndTime = DATEADD(MINUTE,@PV_MaxOTInMin,@PV_ShiftEndTime)
				END
			END ELSE BEGIN
				UPDATE #tbl_TimeCard SET ShiftEndTime= (DATEADD(SECOND,0,DATEADD (MINUTE,DATEPART(MINUTE,@PV_MaxOutTime), DATEADD(HOUR, DATEPART(HOUR,@PV_MaxOutTime), DATEADD(dd, DATEDIFF(dd, -0, InTime), 0)))))
				SET @PV_MaxOTInMin=0
			END
			
			SET @PV_Loop_ShiftEndTime = DATEADD(MINUTE,RAND()*(10),@PV_Loop_ShiftEndTime)
			SET @PV_OverTime = CASE WHEN @PV_Loop_OverTimeInMinute>@PV_MaxOTInMin THEN DATEDIFF(MINUTE,@PV_Loop_ShiftEndTime,DATEADD(MINUTE,RAND()*10,DATEADD(MINUTE,@PV_MaxOTInMin,@PV_Loop_ShiftEndTime))) ELSE  @PV_Loop_OverTimeInMinute END
			SET @PV_Loop_InTime = CASE WHEN @PV_MinInTime IS NOT NULL AND @PV_Loop_InTime<@PV_MinInTime THEN DATEADD(MINUTE,RAND()*(10),@PV_MinInTime) ELSE @PV_Loop_InTime END

			--select @PV_Loop_AttendanceDate,@PV_MinInTime
			INSERT INTO MaxOTConfigurationAttendance 
			SELECT @PV_Loop_EmployeeID,@Param_MOCID,@PV_Loop_AttendanceDate,@PV_Loop_ShiftEndTime/*DATEADD(MINUTE,RAND()*(-10),@PV_Loop_ShiftEndTime)*/--DATEADD(MINUTE,RAND()*5,DATEADD(MINUTE,@PV_MaxOTInMin-5,@PV_Loop_ShiftEndTime))
			
			,@PV_OverTime
			,@PV_Loop_InTime
			
			--Asad
			,ISNULL(@PV_Loop_IsDayOff, 0)
			,ISNULL(@PV_Loop_IsHoliday, 0)
			,ISNULL(@PV_Loop_IsLeave, 0)
			,ISNULL(@PV_Loop_LeaveHeadID, 0)
		END 
	FETCH NEXT FROM Cur_CCC INTO @PV_Loop_EmployeeID,@PV_Loop_AttendanceDate,@PV_Loop_OutTime,/*@PV_Loop_ShiftEndTime,*/@PV_Loop_OverTimeInMinute,@PV_Loop_InTime, @PV_Loop_IsDayOff, @PV_Loop_IsHoliday, @PV_Loop_IsLeave, @PV_Loop_LeaveHeadID, @PV_Loop_ShiftID
	END								
	CLOSE Cur_CCC
	DEALLOCATE Cur_CCC

	UPDATE #tbl_TimeCard SET OutTime= ISNULL((SELECT OutTime FROM MaxOTConfigurationAttendance WHERE MOCID =@Param_MOCID AND AttendanceDate=#tbl_TimeCard.AttendanceDate AND EmployeeID=#tbl_TimeCard.EmployeeID),OutTime)
	UPDATE #tbl_TimeCard SET OverTimeInMinute=ISNULL((SELECT OverTimeInMin FROM MaxOTConfigurationAttendance WHERE MOCID =@Param_MOCID AND AttendanceDate=#tbl_TimeCard.AttendanceDate AND EmployeeID=#tbl_TimeCard.EmployeeID),OverTimeInMinute)
	UPDATE #tbl_TimeCard SET InTime= ISNULL((SELECT InTime FROM MaxOTConfigurationAttendance WHERE MOCID =@Param_MOCID AND AttendanceDate=#tbl_TimeCard.AttendanceDate AND EmployeeID=#tbl_TimeCard.EmployeeID),InTime)
	
	UPDATE #tbl_TimeCard SET LeaveName= (SELECT ShortName FROM LeaveHead WHERE LeaveHeadID=#tbl_TimeCard.LeaveHeadID) WHERE LeaveHeadID>0
	UPDATE #tbl_TimeCard SET TotalWorkingHourInMinute= DATEDIFF(MINUTE,InTime,OutTime)+1 WHERE CAST(InTime AS TIME(0))!='00:00:00' AND CAST(OutTime AS TIME(0))!='00:00:00'

	IF @PV_IsPresentOnDayOff=0
	BEGIN
		UPDATE #tbl_TimeCard SET InTime=CONVERT(DATE,InTime),OutTime=CONVERT(DATE,OutTime),TotalWorkingHourInMinute=0,OverTimeInMinute=0
		WHERE IsDayOff=1
	END ELSE BEGIN
		UPDATE #tbl_TimeCard SET IsDayOff=0
		WHERE IsDayOff=1
	END

	IF @PV_IsPresentOnHoliday=0
	BEGIN
		UPDATE #tbl_TimeCard SET InTime=CONVERT(DATE,InTime),OutTime=CONVERT(DATE,OutTime),TotalWorkingHourInMinute=0,OverTimeInMinute=0
		WHERE IsHoliday=1
	END ELSE BEGIN
		UPDATE #tbl_TimeCard SET IsDayOff=0
		WHERE IsHoliday=1
	END

	SELECT TC.*
	,EMP.EmployeeCode
	,EMP.EmployeeName
	,EMP.JoiningDate 
	,EMP.DepartmentName
	,EMP.DesignationName
	,EMP.LocationName
	,EMP.BUName
	,EMP.DesignationName
	FROM #tbl_TimeCard TC
	INNER JOIN #tbl_EmpBasic EMP ON Emp.EmployeeID=TC.EmployeeID
	ORDER BY TC.AttendanceDate

	--SELECT * FROM #tbl_EmpBasic
	--SELECT * FROM #tbl_TimeCard
	DROP TABLE #tbl_TimeCard
	DROP TABLE #tbl_EmpBasic

END


GO
