USE [ESimSol_ERP]
GO
/****** Object:  View [dbo].[View_LeaveApplication]    Script Date: 11/12/2018 10:10:41 AM ******/
DROP VIEW [dbo].[View_LeaveApplication]
GO
/****** Object:  View [dbo].[View_Employee_WithImage]    Script Date: 11/12/2018 10:10:41 AM ******/
DROP VIEW [dbo].[View_Employee_WithImage]
GO
/****** Object:  StoredProcedure [dbo].[SP_Rpt_SalarySummaryDetail_F2]    Script Date: 11/12/2018 10:10:41 AM ******/
DROP PROCEDURE [dbo].[SP_Rpt_SalarySummaryDetail_F2]
GO
/****** Object:  StoredProcedure [dbo].[SP_Rpt_SalarySummaryDetail_BUWise_Group]    Script Date: 11/12/2018 10:10:41 AM ******/
DROP PROCEDURE [dbo].[SP_Rpt_SalarySummaryDetail_BUWise_Group]
GO
/****** Object:  StoredProcedure [dbo].[SP_Rpt_SalarySummary_F2]    Script Date: 11/12/2018 10:10:41 AM ******/
DROP PROCEDURE [dbo].[SP_Rpt_SalarySummary_F2]
GO
/****** Object:  StoredProcedure [dbo].[SP_Rpt_SalarySummary_BUWise_Group]    Script Date: 11/12/2018 10:10:41 AM ******/
DROP PROCEDURE [dbo].[SP_Rpt_SalarySummary_BUWise_Group]
GO
/****** Object:  StoredProcedure [dbo].[SP_RPT_SalarySheetDetail]    Script Date: 11/12/2018 10:10:41 AM ******/
DROP PROCEDURE [dbo].[SP_RPT_SalarySheetDetail]
GO
/****** Object:  StoredProcedure [dbo].[SP_RPT_SalarySheet]    Script Date: 11/12/2018 10:10:41 AM ******/
DROP PROCEDURE [dbo].[SP_RPT_SalarySheet]
GO
/****** Object:  StoredProcedure [dbo].[SP_FindErrorOnProcessedSalary]    Script Date: 11/12/2018 10:10:41 AM ******/
DROP PROCEDURE [dbo].[SP_FindErrorOnProcessedSalary]
GO
/****** Object:  StoredProcedure [dbo].[SP_FindErrorOnProcessedSalary]    Script Date: 11/12/2018 10:10:41 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

---- ========================================================
---- Author:		Asadullah Sarker
---- Create date: 17 July 2018
---- Description:	Find the reason of not processing salary
---- ========================================================
CREATE PROCEDURE [dbo].[SP_FindErrorOnProcessedSalary]
--DECLARE
	@Param_BUIDs VARCHAR(MAX)
	,@Param_LocationIDs VARCHAR(MAX)
	,@Param_MonthID INT
	,@Param_Year INT
	,@Param_Index INT

	--SET @Param_BUIDs ='2'
	--SET @Param_LocationIDs ='2'
	--SET @Param_MonthID =4
	--SET @Param_Year =2018

AS
BEGIN
	DECLARE
	@PV_PayRollProcessID INT
	,@PV_PPM_LocationID as int
	,@PV_PPM_StartDate as date
	,@PV_PPM_EndDate as date
	,@PV_PPM_Month as int
	,@PV_PPMID as int
	,@PV_BUID as int
	,@PV_LocID as int
	,@PV_MonthID as int
	,@PV_Year as int
	,@PV_PPM_MonthDays int
	,@PV_EmployeeID int
	,@PV_SS_IsAttendanceDependant bit
	,@PV_ESSID int
	,@PV_SalarySchemeID int
	,@PV_Flag bit

	,@PV_SQL as varchar(max)
	
	SET @PV_SQL=''


	DECLARE @tbl_Department AS TABLE (PPMID int, DepartmentID int)
	DECLARE @tbl_SalaryScheme AS TABLE (PPMID int, SalarySchemeID int)
	DECLARE @tbl_SalaryHead AS TABLE (PPMID int, SalaryHeadID int)
	
	DECLARE @tbl_Employee AS TABLE (ESSID int,EmployeeID int,ActualGrossAmount decimal(30,17),CurrencyID int,SalarySchemeID int,CompGrossAmount decimal(18,2))
	
	DECLARE @tbl_ErrorIDs AS TABLE(EmployeeID int)
	DECLARE @tbl_Salary AS TABLE(EmployeeID int)

	DECLARE @tbl_PPM AS TABLE (PPMID int, BUID int, LocID int, MonthID int, [Year] int, SalaryFrom DATETIME, SalaryTo DATETIME)

	DECLARE @tbl_ErrorTable AS TABLE (EmployeeID int, Reason VARCHAR(MAX))
	

	IF NOT EXISTS(SELECT * FROM PayrollProcessManagement WHERE MonthID=@Param_MonthID AND DATEPART(Year, SalaryTo) = @Param_Year)
	BEGIN
		ROLLBACK
			RAISERROR(N'Salary not processed.!',16,1);
		RETURN
	END

	--get them PPMID according to searching criteria
	SET @PV_SQL = 'SELECT PPMID, BusinessUnitID, LocationID, MonthID, DATEPART(YEAR, SalaryTo), SalaryFrom, SalaryTo FROM PayrollProcessManagement WITH (NOLOCK) WHERE MonthID='+CONVERT(VARCHAR(512),@Param_MonthID)+' AND DATEPART(Year, SalaryTo) = '+CONVERT(VARCHAR(512),@Param_Year) 
	IF (@Param_LocationIDs <> '' AND @Param_LocationIDs IS NOT NULL)
	BEGIN
		SET @PV_SQL = @PV_SQL + ' AND LocationID IN('+@Param_LocationIDs+')'
	END
	IF (@Param_BUIDs <> '' AND @Param_BUIDs IS NOT NULL)
	BEGIN
		SET @PV_SQL = @PV_SQL + ' AND BusinessUnitID IN('+@Param_BUIDs+')'
	END

	INSERT INTO @tbl_PPM 
	EXEC (@PV_SQL)
	--select * from @tbl_PPM
	
	--select * from @tbl_PPM

	--SELECT @PV_PPM_LocationID=LocationID,@PV_PPM_StartDate=SalaryFrom,@PV_PPM_EndDate=SalaryTo, @PV_PPM_Month=MonthID 
	--FROM PayrollProcessManagement WHERE PPMID=@PV_PPMID
		
	--get the objects of process management
	INSERT INTO @tbl_Department
	SELECT PPMID, ObjectID FROM PayrollProcessManagementObject 
	WHERE PPMID IN(SELECT PPMID FROM @tbl_PPM) AND PPMObject=1/*DepartmentID*/

	INSERT INTO @tbl_SalaryScheme
	SELECT PPMID, ObjectID FROM PayrollProcessManagementObject 
	WHERE PPMID IN(SELECT PPMID FROM @tbl_PPM) AND PPMObject=2/*SalaryScheme*/

	INSERT INTO @tbl_SalaryHead
	SELECT PPMID, ObjectID FROM PayrollProcessManagementObject 
	WHERE PPMID IN(SELECT PPMID FROM @tbl_PPM) AND PPMObject=3/*SalaryHead*/
		
	SET @PV_PPM_MonthDays=DATEDIFF(DAY,@PV_PPM_StartDate,@PV_PPM_EndDate)+1


	--Get the employees who are eligible for salary
	INSERT INTO @tbl_Employee
	SELECT top(100)aa.ESSID,aa.EmployeeID,aa.ActualGrossAmount,aa.CurrencyID,aa.SalarySchemeID,aa.CompGrossAmount FROM (
	SELECT ROW_NUMBER() OVER(ORDER BY ESSID) AS Row,ESSID,EmployeeID,ActualGrossAmount,CurrencyID,SalarySchemeID,ISNULL(CompGrossAmount ,0) AS CompGrossAmount
	FROM EmployeeSalaryStructure WITH(NOLOCK) WHERE IsActive=1 AND StartDay=(SELECT TOP(1)DAY(SalaryFrom) FROM @tbl_PPM) AND SalarySchemeID IN (
	SELECT SalarySchemeID FROM @tbl_SalaryScheme) AND EmployeeID IN (
	SELECT EmployeeID FROM EmployeeOfficial WHERE WorkingStatus IN (1,2,6)/*InWorkingPlace,OSD*/
	AND IsActive=1 AND DateOfJoin<=(SELECT TOP(1)SalaryTo FROM @tbl_PPM) AND DRPID IN (
	SELECT DepartmentRequirementPolicyID FROM DepartmentRequirementPolicy WHERE LocationId IN(SELECT LocID FROM @tbl_PPM)
	AND DepartmentID IN (SELECT DepartmentID FROM @tbl_Department))))aa WHERE aa.Row>@Param_Index

	--select * from @tbl_Employee

	--get the error ids
	INSERT INTO @tbl_Salary 
	SELECT ES.EmployeeID
	FROM @tbl_Employee Emp
	LEFT JOIN EmployeeSalary ES ON Emp.EmployeeID= ES.EmployeeID
	LEFT JOIN EmployeeOfficial EO ON EO.EmployeeID = ES.EmployeeID
	LEFT JOIN DepartmentRequirementPolicy DRP ON DRP.DepartmentRequirementPolicyID = EO.DRPID
	LEFT JOIN BusinessUnit BU ON BU.BusinessUnitID = DRP.BusinessUnitID
	WHERE PayrollProcessID IN(SELECT PPMID FROM @tbl_PPM) AND EO.IsActive=1-- AND ES.EmployeeID NOT IN(SELECT EmployeeID FROM @tbl_Employee)

	INSERT INTO @tbl_ErrorIDs 
	SELECT EmployeeID FROM @tbl_Employee WHERE EmployeeID NOT IN(SELECT EmployeeID FROM @tbl_Salary)


	----select @PV_PPM_StartDate, @PV_PPM_EndDate
	--select * from @tbl_Employee
	

	--get the errors why not processed 
	DECLARE Cur_CC1 CURSOR LOCAL FORWARD_ONLY KEYSET FOR
	SELECT EmployeeID FROM @tbl_ErrorIDs	
	OPEN Cur_CC1
	FETCH NEXT FROM Cur_CC1 INTO  @PV_EmployeeID
	WHILE(@@Fetch_Status <> -1)
	BEGIN
		
		SET @PV_Flag = 0
		--select @PV_EmployeeID
		SET @PV_SalarySchemeID = (SELECT SalarySchemeID FROM EmployeeSalaryStructure WHERE EmployeeID = @PV_EmployeeID)
		SET @PV_SS_IsAttendanceDependant = (SELECT IsAttendanceDependant FROM SalaryScheme WHERE SalarySchemeID = @PV_SalarySchemeID)
		SET @PV_ESSID = (SELECT ESSID FROM EmployeeSalaryStructure WHERE EmployeeID= @PV_EmployeeID)

		--IF EXISTS (SELECT * FROM EmployeeSalary WITH(NOLOCK) WHERE StartDate BETWEEN @PV_PPM_StartDate AND @PV_PPM_EndDate AND EmployeeID=@PV_EmployeeID)
		--BEGIN
		--	INSERT INTO @tbl_ErrorTable (EmployeeID, BUID, LocationID, MonthID, [Year], ErrorMessage) 
		--	                     VALUES (@PV_EmployeeID, @PV_BUID, @PV_LocID, @PV_MonthID, @PV_Year, 'Salary Already Exists.')
		--END

		IF NOT EXISTS (SELECT * FROM EmployeeSalaryStructureDetail WITH(NOLOCK) WHERE ESSID=@PV_ESSID)
		BEGIN
			SET @PV_Flag = 1
			INSERT INTO @tbl_ErrorTable (EmployeeID, Reason) 
				                    VALUES (@PV_EmployeeID, 'No salary structure detail for this employee')
			
		END
		SET @PV_PPM_StartDate = (SELECT TOP(1)SalaryFrom FROM @tbl_PPM)
		SET @PV_PPM_EndDate = (SELECT TOP(1)SalaryTo FROM @tbl_PPM)

		--select @PV_PPM_StartDate, @PV_PPM_EndDate
		IF @PV_SS_IsAttendanceDependant=1
		BEGIN
			IF NOT EXISTS (SELECT * FROM AttendanceDaily WITH(NOLOCK) WHERE EmployeeID=@PV_EmployeeID AND AttendanceDate BETWEEN @PV_PPM_StartDate AND @PV_PPM_EndDate)
			BEGIN
				SET @PV_Flag = 1
				INSERT INTO @tbl_ErrorTable (EmployeeID, Reason) 
				                    VALUES (@PV_EmployeeID, 'No attendance in this month for this employee')											
			END
			
		END		

		--No salary structure 
		IF NOT EXISTS(SELECT * FROM EmployeeSalaryStructure WHERE EmployeeID=@PV_EmployeeID)
		BEGIN
			SET @PV_Flag = 1
			INSERT INTO @tbl_ErrorTable (EmployeeID, Reason) 
				                VALUES (@PV_EmployeeID, 'No salary structure from this employee')		
											
		END
		--active inactive
		IF EXISTS(SELECT * FROM EmployeeActiveInactiveHistory WHERE EmployeeID=@PV_EmployeeID AND InactiveDate BETWEEN @PV_PPM_StartDate AND @PV_PPM_EndDate)
		BEGIN
			SET @PV_Flag = 1
			INSERT INTO @tbl_ErrorTable (EmployeeID,  Reason) 
				                VALUES (@PV_EmployeeID, 'Employee inactivated in this month cycle')		
											
		END

		IF @PV_Flag=0
		BEGIN
			INSERT INTO @tbl_ErrorTable (EmployeeID,  Reason) 
				                VALUES (@PV_EmployeeID, 'Now you can process salary for this employee')	
		END

		FETCH NEXT FROM Cur_CC1 INTO  @PV_EmployeeID
	END--
	CLOSE Cur_CC1
	DEALLOCATE Cur_CC1


	--select * from @tbl_ErrorTable
	SET @Param_Index = @Param_Index + 100

	SELECT ET.* 
		,Emp.Name
		,Emp.Code
		,BU.BusinessUnitID AS BUID
		,BU.Name AS BUName
		,Loc.LocationID AS LocationID
		,Loc.Name AS LocationName
		,Dept.DepartmentID AS DepartmentID
		,Dept.Name AS DepartmentName
		,Desg.DesignationID AS DesignationID
		,Desg.Name AS DesignationName
		,@Param_Index AS IndexNo
	FROM @tbl_ErrorTable ET
	LEFT JOIN Employee Emp ON Emp.EmployeeID = ET.EmployeeID
	LEFT JOIN EmployeeOfficial EO ON EO.EmployeeID = ET.EmployeeID
	LEFT JOIN DepartmentRequirementPolicy DRP ON DRP.DepartmentRequirementPolicyID = EO.DRPID
	LEFT JOIN BusinessUnit BU ON BU.BusinessUnitID = DRP.BusinessUnitID
	LEFT JOIN Location Loc ON Loc.LocationID = DRP.LocationID
	LEFT JOIN Department Dept ON Dept.DepartmentID = DRP.DepartmentID
	LEFT JOIN Designation Desg ON Desg.DesignationID = EO.DesignationID

END

GO
/****** Object:  StoredProcedure [dbo].[SP_RPT_SalarySheet]    Script Date: 11/12/2018 10:10:41 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[SP_RPT_SalarySheet]
	@Param_BUIDs AS varchar(max)
	,@Param_LocationIDs AS VARCHAR (max)
	,@Param_DepartmentIDs AS Varchar(max)
	,@Param_DesignationIDs AS Varchar(max)
	,@Param_SalarySchemeIDs AS Varchar(max)
	,@Param_BlockIds AS Varchar(max)
	,@Param_GroupIds AS Varchar(max)
	,@Param_EmployeeIDs as varchar(max)
	,@Param_MonthID AS int--Mandatory
	,@Param_Year as int--Mandatory
	,@Param_IsNewJoin as bit
	,@Param_IsOutSheet as int
	,@Param_MinSalary as decimal (18,2)
	,@Param_MaxSalary as decimal (18,2)
	,@Param_IsComp as bit
	,@Param_PaymentType int--0 All, 1 Bank, 2 Cash
	,@Param_IsMatchExact BIT
AS
BEGIN
	--SET @Param_MonthID=12
	--SET @Param_Year=2017
	--SET @Param_BUIDs=''
	--SET @Param_LocationIDs=''
	--SET @Param_DepartmentIDs=''
	--SET @Param_DesignationIDs=''
	--SET @Param_EmployeeIDs=''
	--SET @Param_IsNewJoin=0
	--SET @Param_MinSalary=0
	--SET @Param_MaxSalary=0

	IF OBJECT_ID('tempdb..#tbl_EmpID') IS NOT NULL
	BEGIN
		DROP TABLE #tbl_EmpID
	END
	IF OBJECT_ID('tempdb..#tbl_EmpBasic') IS NOT NULL
	BEGIN
		DROP TABLE #tbl_EmpBasic
	END
	IF OBJECT_ID('tempdb..#tbl_EmpOff') IS NOT NULL
	BEGIN
		DROP TABLE #tbl_EmpOff
	END
	IF OBJECT_ID('tempdb..#tbl_AttDaily') IS NOT NULL
	BEGIN
		DROP TABLE #tbl_AttDaily
	END
	IF OBJECT_ID('tempdb..#tbl_SalarySheet') IS NOT NULL
	BEGIN
		DROP TABLE #tbl_SalarySheet
	END

	IF OBJECT_ID('tempdb..#tbl_TPI') IS NOT NULL
	BEGIN
		DROP TABLE #tbl_TPI
	END

	IF OBJECT_ID('tempdb..#tbl_EmpCashAmount') IS NOT NULL
	BEGIN
		DROP TABLE #tbl_EmpCashAmount
	END

	IF OBJECT_ID('tempdb..#tbl_EmpBlock') IS NOT NULL
	BEGIN
		DROP TABLE #tbl_EmpBlock
	END
	IF OBJECT_ID('tempdb..#tbl_SalaryScheme') IS NOT NULL
	BEGIN
		DROP TABLE #tbl_SalaryScheme
	END

	SET @Param_PaymentType=ISNULL(@Param_PaymentType,0)


	CREATE TABLE #tbl_EmpID (EmployeeID int,ESID int) 
	CREATE CLUSTERED INDEX IDX_tbl_EmpID ON #tbl_EmpID (EmployeeID);

	CREATE TABLE #tbl_EmpBasic(EmployeeID int,Code varchar(100),Name varchar(100),AccountNo varchar(100),BankName varchar (100),EmpNameInBangla nvarchar(100),GENDER varchar(50))
	CREATE CLUSTERED INDEX IDX_tbl_EmpBasic ON #tbl_EmpBasic (EmployeeID);

	CREATE TABLE #tbl_EmpOff(EmployeeID int,DOJ DATE,Grade Varchar(50),DOC DATE)
	CREATE CLUSTERED INDEX IDX_tbl_EmpOff ON #tbl_EmpOff (EmployeeID);

	CREATE TABLE #tbl_AttDaily(EmployeeID int,TotalDays int/*att record*/, LateInMin int, EarlyInMin int)
	CREATE CLUSTERED INDEX IDX_tbl_AttDaily ON #tbl_AttDaily (EmployeeID);
	--CREATE TABLE #tbl_ESD(ESID int,SalaryHeadID int,SType smallint,Amount Decimal(18,2))	
	CREATE TABLE #tbl_TPI (EmployeeID int, LastGross Decimal(18,2),EffectedDate DATE )
	CREATE CLUSTERED INDEX IDX_tbl_TPI ON #tbl_TPI (EmployeeID);

	CREATE TABLE #tbl_EmpCashAmount (EmployeeID int, CashAmount decimal(18,2))
	CREATE CLUSTERED INDEX IDX_tbl_EmpCashAmount ON #tbl_EmpCashAmount (EmployeeID);

	CREATE TABLE #tbl_EmpBlock (EmployeeID int,BlockName varchar(100)) 
	CREATE CLUSTERED INDEX IDX_tbl_EmpBlock ON #tbl_EmpBlock (EmployeeID);
	
	CREATE TABLE #tbl_SalaryScheme (EmployeeID int,SalarySchemeID int, OTVal DECIMAL(18,2), EmployeeSalaryID INT) 
	CREATE CLUSTERED INDEX IDX_tbl_SalaryScheme ON #tbl_SalaryScheme (EmployeeID);

	CREATE TABLE #tbl_SalarySheet (	EmployeeSalaryID int,EmployeeID int,BUID int,LocationID int,DepartmentID int,DesignationID int
	,DayOffHoliday int
	--,LWP int/*Leave without Pay*/,CL int/*Casual Leave*/,EL int/*Earn Leave*/,SL int/*Sick Liave*/,ML int/*Maternity Leave*/
	,A int/*Absent*/,P int/*Present*/,TotalLeave int,TotalLate int,TotalEarly int
	,Gross decimal(18,2)
	--,Basics decimal (18,2),HR decimal(18,2)/*House Rent*/,Med decimal(18,2)/* Medical*/,Food decimal(18,2)/*Food*/,Conv decimal(18,2)/*Conveyance*/
	--,Earning decimal(18,2),AttBonus decimal(18,2)
	,OT_HR decimal(18,2)/*OverTime Hour*/,OT_Rate decimal(18,2),OT_Amount decimal(18,2)
	,NetAmount decimal (18,2), StartDate DAte, EndDate Date, TotalPLeave int, TotalUpLeave int, TotalHoliday int, TotalDOff int, BankAmount DECIMAL(18, 2), CashAmount DECIMAL(18, 2), BankAccountID INT)
	CREATE CLUSTERED INDEX IDX_tbl_SalarySheet ON #tbl_SalarySheet (EmployeeID);

	DECLARE
	@PV_SQL as varchar(max)
	,@PV_StartDate As DATE
	,@PV_EndDate AS DATE
	SET @PV_SQL=''

	IF NOT EXISTS (SELECT * FROM PayrollProcessManagement WHERE MonthID=@Param_MonthID AND DATEPART(YEAR,SalaryTo)=@Param_Year)
	BEGIN
		ROLLBACK
			RAISERROR (N'Invalid Year & Month',16,1)
		RETURN
	END

	--Get Start Date & End Date
	SELECT top(1)@PV_StartDate=SalaryFrom,@PV_EndDate=SalaryTo 
	FROM PayrollProcessManagement WHERE MonthID=@Param_MonthID AND DATEPART(YEAR,SalaryTo)=@Param_Year
	
	SET @Param_IsComp= ISNULL(@Param_IsComp,0)

	SET @PV_SQL='SELECT EmployeeID,EmployeeSalaryID FROM EmployeeSalary WHERE StartDate='''+CONVERT(VARCHAR(50),@PV_StartDate)+''' AND EndDate='''+CONVERT(VARCHAR(50),@PV_EndDate)+''''
	
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

	IF @Param_IsOutSheet IS NOT NULL AND @Param_IsOutSheet=1
	BEGIN
		SET @PV_SQL=@PV_SQL+ ' AND IsOutSheet = 1'
	END
	
	IF @Param_MinSalary>0 AND @Param_MaxSalary>0
	BEGIN
		IF @Param_IsComp=1
		BEGIN
			SET @PV_SQL=@PV_SQL+ ' AND CompGrossAmount BETWEEN '+CONVERT(VARCHAR(50),@Param_MinSalary)+' AND '+CONVERT(VARCHAR(50),@Param_MaxSalary)+''
		END ELSE BEGIN
			SET @PV_SQL=@PV_SQL+ ' AND GrossAmount BETWEEN '+CONVERT(VARCHAR(50),@Param_MinSalary)+' AND '+CONVERT(VARCHAR(50),@Param_MaxSalary)+''
		END
	END

	IF (@Param_BUIDs IS NOT NULL AND @Param_BUIDs<>'')
	BEGIN
		SET @PV_SQL=@PV_SQL+ ' AND PayrollProcessID IN (SELECT PPMID FROM PayrollProcessManagement WHERE BusinessUnitID IN ('+@Param_BUIDs+'))'
	END

	IF (@Param_SalarySchemeIDs IS NOT NULL AND @Param_SalarySchemeIDs <>'')
	BEGIN
		SET @PV_SQL=@PV_SQL+ ' AND EmployeeID IN (SELECT EmployeeID FROM EmployeeSalaryStructure WHERE SalarySchemeID IN ('+@Param_SalarySchemeIDs+'))'
	END

	IF (@Param_IsNewJoin IS NOT NULL AND @Param_IsNewJoin <>0)
	BEGIN
		SET @PV_SQL=@PV_SQL+ ' AND EmployeeID IN (SELECT EmployeeID FROM EmployeeOfficial WHERE DateOfJoin BETWEEN '''+CONVERT(VARCHAR(50),@PV_StartDate)+''' AND '''+CONVERT(VARCHAR(50),@PV_EndDate)+''')'
	END

	IF @Param_GroupIds IS NOT NULL AND @Param_GroupIds <>''
	BEGIN
		IF (@Param_BlockIds IS NOT NULL AND @Param_BlockIds <>'')
		BEGIN
			SET @Param_BlockIds=@Param_BlockIds+','+@Param_GroupIds
		END ELSE BEGIN
			SET @Param_BlockIds=@Param_GroupIds
		END
	END

	IF @Param_IsMatchExact=0
	BEGIN
		IF (@Param_BlockIds IS NOT NULL AND @Param_BlockIds <>'')
		BEGIN
			--SET @PV_SQL=@PV_SQL+ ' AND EmployeeID IN (SELECT EmployeeID FROM EmployeeGroup WHERE EmployeeTypeID IN('+@Param_BlockIds+'))'
			IF @Param_GroupIds<>'' AND @Param_GroupIds IS NOT NULL
			BEGIN
				SET @PV_SQL=@PV_SQL+' AND EmployeeID IN(SELECT EmployeeID From EmployeeGroup WHERE EmployeeTypeID IN(' + @Param_GroupIds + ') AND EmployeeTypeID IN(SELECT EmployeeTypeID FROM EmployeeType WHERE EmployeeGrouping=2))'
			END
			IF @Param_BlockIds<>'' AND @Param_BlockIds IS NOT NULL
			BEGIN
				SET @PV_SQL=@PV_SQL+' AND EmployeeID IN(SELECT EmployeeID From EmployeeGroup WHERE EmployeeTypeID IN(' + @Param_BlockIds + ') AND EmployeeTypeID IN(SELECT EmployeeTypeID FROM EmployeeType WHERE EmployeeGrouping=3))'
			END
		END
	END ELSE
	BEGIN
		IF (@Param_BlockIds IS NOT NULL AND @Param_BlockIds <>'')
		BEGIN
			SET @PV_SQL=@PV_SQL+ ' AND EmployeeID IN(SELECT HH.EmployeeID FROM EmployeeGroup AS HH 
			WHERE HH.EmployeeTypeID IN (SELECT MM.items FROM dbo.SplitInToDataSet('''+@Param_BlockIds+''','+''','') AS MM)
			GROUP BY HH.EmployeeID
			HAVING  (COUNT(DISTINCT HH.EmployeeTypeID) >= (SELECT COUNT(MM.items) FROM dbo.SplitInToDataSet('''+@Param_BlockIds+''','+''','') AS MM)))'
		END
	END
	IF @Param_PaymentType=1
	BEGIN
		SET @PV_SQL=@PV_SQL+ ' AND EmployeeID IN (SELECT EmployeeID FROM EmployeeBankAccount WHERE IsActive=1)'
	END
	

	
	--SELECT @PV_SQL
	
	--Get EmployeeID
	INSERT INTO #tbl_EmpID
	EXEC(@PV_SQL)
	
	--Get EmpBAsic Info
	INSERT INTO #tbl_EmpBasic
	SELECT EmployeeID,Code,Name 
	,(SELECT top(1)AccountNo FROM EmployeeBankAccount WHERE EmployeeID=Employee.EmployeeID AND IsActive=1)
	,(SELECT top(1)BankBranchName FROM View_EmployeeBankAccount WHERE EmployeeID=Employee.EmployeeID AND IsActive=1)
	,NameInBangla,Gender
	FROM Employee WHERE EmployeeID IN (
	SELECT EmployeeID FROM #tbl_EmpID)
	--SELECT * FROM #tbl_EmpBasic


	--Get Official Info
	INSERT INTO #tbl_EmpOff
	SELECT EmployeeID,DateOfJoin
	,(SELECT Name FROM EmployeeType WHERE EmployeeTypeID=EmployeeOfficial.EmployeeTypeID)	
	,DateOfConfirmation	
	FROM EmployeeOfficial WHERE EmployeeID IN (
	SELECT EmployeeID FROM #tbl_EmpID)
	--SELECT * FROM #tbl_EmpOff

	--Get Employee Block
	INSERT INTO #tbl_EmpBlock
	SELECT Distinct EmployeeID,Name FROM View_EmployeeGroup WHERE EmployeeTypeID IN (SELECT EmployeeTypeID FROM EmployeeType WHERE EmployeeGrouping=3) AND EmployeeID IN (
	SELECT EmployeeID FROM #tbl_EmpID)

	--Get Attendance
	IF @Param_IsComp=1 
	BEGIN
		INSERT INTO #tbl_AttDaily
		SELECT EmployeeID,COUNT(AttendanceID),SUM(CompLateArrivalMinute),SUM(CompEarlyDepartureMinute)
		FROM AttendanceDaily WHERE AttendanceDate BETWEEN @PV_StartDate AND @PV_EndDate AND EmployeeID IN (
		SELECT EmployeeID FROM #tbl_EmpID)
		GROUP BY EmployeeID ORDER BY EmployeeID
	END ELSE BEGIN
		INSERT INTO #tbl_AttDaily
		SELECT EmployeeID,COUNT(AttendanceID),SUM(LateArrivalMinute),SUM(EarlyDepartureMinute)
		FROM AttendanceDaily WHERE AttendanceDate BETWEEN @PV_StartDate AND @PV_EndDate AND EmployeeID IN (
		SELECT EmployeeID FROM #tbl_EmpID)
		GROUP BY EmployeeID ORDER BY EmployeeID
	END

	--Get Transfer Promotion Increament
	IF @Param_IsComp=1
	BEGIN
		INSERT INTO #tbl_TPI 
		SELECT EmployeeID,MAX(CompGrossSalary),MAX(ActualEffectedDate) FROM TransferPromotionIncrement WHERE IsIncrement=1 AND ActualEffectedDate <=@PV_EndDate AND EmployeeID IN (
		SELECT EmployeeID FROM #tbl_EmpID)
		GROUP BY EmployeeID Order BY EmployeeID
	END ELSE BEGIN
		INSERT INTO #tbl_TPI 
		SELECT EmployeeID,MAX(GrossSalary),MAX(ActualEffectedDate) FROM TransferPromotionIncrement WHERE IsIncrement=1 AND ActualEffectedDate <=@PV_EndDate AND EmployeeID IN (
		SELECT EmployeeID FROM #tbl_EmpID)
		GROUP BY EmployeeID Order BY EmployeeID
	END

	--Get CashAmount
	--Edited on 28 Mar 2018 for compliance cash amount will be 0
	IF @Param_IsComp=1
	BEGIN 
		INSERT INTO #tbl_EmpCashAmount
		SELECT EmployeeID,0 FROM EmployeeSalaryStructure WHERE EmployeeID IN (
		SELECT EmployeeID FROM #tbl_EmpID)
	END ELSE BEGIN
		INSERT INTO #tbl_EmpCashAmount
		SELECT EmployeeID,CashAmount FROM EmployeeSalaryStructure WHERE CashAmount>0 AND EmployeeID IN (
		SELECT EmployeeID FROM #tbl_EmpID)
	END
	
	INSERT INTO #tbl_SalaryScheme
	SELECT ESS.EmployeeID
		 , ESS.SalarySchemeID 
		 , CASE WHEN SS.CompOverTimeON=1 THEN 1*ROUND((ESS.CompGrossAmount/SS.CompDividedBy)*SS.CompMultiplicationBy,2) WHEN SS.CompOverTimeON=2 THEN 1*ROUND(((SELECT MAX(CompAmount) FROM EmployeeSalaryStructureDetail WHERE ESSID=ESS.ESSID)/SS.CompDividedBy)*SS.CompMultiplicationBy,2) ELSE 0 END
		 , Emp.ESID
	FROM EmployeeSalaryStructure ESS WITH (NOLOCk)
	LEFT JOIN SalaryScheme SS ON SS.SalarySchemeID = ESS.SalarySchemeID
	LEFT JOIN #tbl_EmpID Emp ON Emp.EmployeeID = ESS.EmployeeID
	WHERE ESS.EmployeeID IN (
	SELECT EmployeeID FROM #tbl_EmpID) 
	
	--Get Employee Salary
	IF @Param_IsComp=1 
	BEGIN
		INSERT INTO #tbl_SalarySheet 
		SELECT ES.EmployeeSalaryID,ES.EmployeeID
			,(SELECT BusinessUnitID FROM PAyrollProcessManagement WHERE PPMID=ES.PayrollProcessID)--BUID
			,LocationID,DepartmentID,DesignationID
			,ISNULL(CompTotalDayOff,0)+ISNULL(CompTotalHoliday,0)/*DayOffHoliday*/
			,ISNULL(CompTotalAbsent,0)
			,ISNULL(CompTotalWorkingDay,0)-ISNULL(CompTotalAbsent,0)-ISNULL(CompTotalLeave,0)
			,ISNULL(CompTotalLeave,0)
			,CompTotalLate,CompTotalEarlyLeave
			--SET @PV_TotalDaysOfAbsent=@PV_TotalWorkingDay-@PV_TotalPresentDay-@PV_TotalPLeave-@PV_TotalUPLeave-@PV_ResignDay;
			,CompGrossAmount
			,CompOTHour
			,CASE WHEN ES.CompOTRatePerHour=0 THEN SS.OTVal ELSE ES.CompOTRatePerHour END--CompOTRatePerHour
			--,CompOTRatePerHour
			,ROUND(CompOTHour*CompOTRatePerHour,2)
			,CompNetAmount,StartDate,EndDate, 0, 0, ISNULL(CompTotalHoliday,0), ISNULL(CompTotalDayOff,0)
			,CompBankAmount--ASAD
			,CompCashAmount--ASAD
			,BankAccountID--ASAD
		FROM EmployeeSalary ES
		LEFT JOIN #tbl_SalaryScheme SS ON SS.EmployeeSalaryID = ES.EmployeeSalaryID
		WHERE ES.EmployeeSalaryID IN (SELECT ESID FROM #tbl_EmpID)
		--StartDate=@PV_StartDate AND EndDate=@PV_EndDate
	END ELSE BEGIN		
		INSERT INTO #tbl_SalarySheet 
		SELECT EmployeeSalaryID,EmployeeID
			,(SELECT BusinessUnitID FROM PAyrollProcessManagement WHERE PPMID=EmployeeSalary.PayrollProcessID)--BUID
			,LocationID,DepartmentID,DesignationID
			,ISNULL(TotalDayOff,0)+ISNULL(TotalHoliday,0)/*DayOffHoliday*/
			,ISNULL(TotalAbsent,0)
			,ISNULL(TotalWorkingDay,0)-ISNULL(TotalAbsent,0)-ISNULL(TotalPLeave,0)-ISNULL(TotalUpLeave,0)
			,ISNULL(TotalPLeave,0)+ISNULL(TotalUpLeave,0)
			,TotalLate,TotalEarlyLeaving
			--SET @PV_TotalDaysOfAbsent=@PV_TotalWorkingDay-@PV_TotalPresentDay-@PV_TotalPLeave-@PV_TotalUPLeave-@PV_ResignDay;
			,GrossAmount
			,OTHour,OTRatePerHour,ROUND(OTHour*OTRatePerHour,2)
			,NetAmount,StartDate,EndDate, ISNULL(TotalPLeave,0), ISNULL(TotalUpLeave,0), ISNULL(TotalHoliday,0), ISNULL(TotalDayOff,0)
			,BankAmount--ASAD
			,CashAmount--ASAD
			,BankAccountID--ASAD
		FROM EmployeeSalary WHERE EmployeeSalaryID IN (SELECT ESID FROM #tbl_EmpID)
		--StartDate=@PV_StartDate AND EndDate=@PV_EndDate
	END

	----Get Employee SalaryDetail	
	--INSERT INTO #tbl_ESD 
	--SELECT EmployeeSalaryID,SalaryHeadID
	--,(SELECT SalaryHeadType FROM SalaryHead WHERE SalaryHeadID=EmployeeSalaryDetail.SalaryHeadID)
	--,ISNULL(Amount,0)
	--FROM EmployeeSalaryDetail WHERE EmployeeSalaryID IN (
	--SELECT EmployeeSalaryID FROM #tbl_SalarySheet)


	IF @Param_PaymentType<>2-- This condition will be optimized later
	BEGIN
		SELECT ASS.*
		,EmpB.Code,EmpB.Name,EmpB.GENDER 
		,EmpO.DOJ,EmpO.Grade,EmpO.DOC
		,BU.Name AS BUName
		,Loc.Name AS LocName
		,ParentDept.Name AS ParentDeptName
		,Dpt.Name AS DptName
		,Dsg.Name AS DsgName
		,Block.BlockName
		,EmpB.AccountNo
		,EmpB.BankName
		,EmpB.EmpNameInBangla
		,Dsg.NameInBangla AS DsgNameInBangla
		,BU.[Address] AS BUAddress	
		,AttDaily.TotalDays
		,AttDaily.LateInMin
		,AttDaily.EarlyInMin
		,ASS.DayOffHoliday+Ass.TotalPLeave+ASS.P AS EWD
		,ISNULL(TPI.LastGross,0) AS LastGross
		,CASE WHEN TPI.LastGross IS NOT NULL THEN ASS.Gross- TPI.LastGross ELSE 0 END AS IncrementAmount
		,CASE WHEN TPI.EffectedDate IS NOT NULL THEN TPI.EffectedDate ELSE NULL END AS EffectedDate
		--,CASE WHEN EmpB.AccountNo IS NOT NULL THEN ISNULL(ECA.CashAmount,0) ELSE ASS.NetAmount END AS  CashAmount
		--,CASE WHEN EmpB.AccountNo IS NOT NULL THEN ASS.NetAmount-ISNULL(ECA.CashAmount,0) ELSE 0 END BankAmount
		FROM #tbl_SalarySheet ASS
		INNER JOIN #tbl_EmpBasic EmpB ON ASS.EmployeeID=EmpB.EmployeeID
		INNER JOIN #tbl_EmpOff EmpO ON ASS.EmployeeID=EmpO.EmployeeID
		INNER JOIN #tbl_AttDaily AttDaily ON ASS.EmployeeID=AttDaily.EmployeeID
		INNER JOIN BUsinessUnit BU ON ASS.BUID=BU.BusinessUnitID
		INNER JOIN Location Loc ON ASS.LocationID=Loc.LocationID
		INNER JOIN Department Dpt ON ASS.DepartmentID=Dpt.DepartmentID
		INNER JOIN Designation Dsg ON ASS.DesignationID=Dsg.DesignationID
		INNER JOIN Department ParentDept ON Dpt.ParentID=ParentDept.DepartmentID
		LEFT JOIN #tbl_TPI TPI ON ASS.EmployeeID=TPI.EmployeeID
		--LEFT JOIN #tbl_EmpCashAmount ECA ON ASS.EmployeeID=ECA.EmployeeID
		LEFT JOIN #tbl_EmpBlock Block ON ASS.EmployeeID=Block.EmployeeID
		ORDER BY EmpB.Code
	END ELSE BEGIN

		SELECT * FROM (SELECT ASS.*
		,EmpB.Code,EmpB.Name,EmpB.GENDER 
		,EmpO.DOJ,EmpO.Grade,EmpO.DOC
		,BU.Name AS BUName
		,Loc.Name AS LocName
		,ParentDept.Name AS ParentDeptName
		,Dpt.Name AS DptName
		,Dsg.Name AS DsgName
		,Block.BlockName
		,EmpB.AccountNo
		,EmpB.BankName
		,EmpB.EmpNameInBangla
		,Dsg.NameInBangla AS DsgNameInBangla
		,BU.[Address] AS BUAddress	
		,AttDaily.TotalDays
		,AttDaily.LateInMin
		,AttDaily.EarlyInMin
		,ASS.DayOffHoliday+Ass.TotalPLeave+ASS.P AS EWD
		,ISNULL(TPI.LastGross,0) AS LastGross
		,CASE WHEN TPI.LastGross IS NOT NULL THEN ASS.Gross- TPI.LastGross ELSE 0 END AS IncrementAmount
		,CASE WHEN TPI.EffectedDate IS NOT NULL THEN TPI.EffectedDate ELSE NULL END AS EffectedDate
		--,CASE WHEN EmpB.AccountNo IS NOT NULL THEN ISNULL(ECA.CashAmount,0) ELSE ASS.NetAmount END AS  CashAmount
		--,CASE WHEN EmpB.AccountNo IS NOT NULL THEN ASS.NetAmount-ISNULL(ECA.CashAmount,0) ELSE 0 END BankAmount
		FROM #tbl_SalarySheet ASS
		INNER JOIN #tbl_EmpBasic EmpB ON ASS.EmployeeID=EmpB.EmployeeID
		INNER JOIN #tbl_EmpOff EmpO ON ASS.EmployeeID=EmpO.EmployeeID
		INNER JOIN #tbl_AttDaily AttDaily ON ASS.EmployeeID=AttDaily.EmployeeID
		INNER JOIN BUsinessUnit BU ON ASS.BUID=BU.BusinessUnitID
		INNER JOIN Location Loc ON ASS.LocationID=Loc.LocationID
		INNER JOIN Department Dpt ON ASS.DepartmentID=Dpt.DepartmentID
		INNER JOIN Designation Dsg ON ASS.DesignationID=Dsg.DesignationID
		INNER JOIN Department ParentDept ON Dpt.ParentID=ParentDept.DepartmentID
		LEFT JOIN #tbl_TPI TPI ON ASS.EmployeeID=TPI.EmployeeID
		--LEFT JOIN #tbl_EmpCashAmount ECA ON ASS.EmployeeID=ECA.EmployeeID
		LEFT JOIN #tbl_EmpBlock Block ON ASS.EmployeeID=Block.EmployeeID
		)tab WHERE CashAmount>0 ORDER BY tab.Code
	END
	


	--SELECT * FROM Designation
	DROP TABLE #tbl_SalarySheet
	DROP TABLE #tbl_EmpBasic
	DROP TABLE #tbl_EmpOff
	DROP TABLE #tbl_AttDaily
	DROP TABLE #tbl_EmpID
	DROP TABLE #tbl_EmpCashAmount
	DROP TABLE #tbl_TPI
	DROP TABLE #tbl_EmpBlock
	DROP TABLE #tbl_SalaryScheme
END





GO
/****** Object:  StoredProcedure [dbo].[SP_RPT_SalarySheetDetail]    Script Date: 11/12/2018 10:10:41 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[SP_RPT_SalarySheetDetail]
	@Param_BUIDs AS varchar(max)
	,@Param_LocationIDs AS VARCHAR (max)
	,@Param_DepartmentIDs AS Varchar(max)
	,@Param_DesignationIDs AS Varchar(max)
	,@Param_SalarySchemeIDs AS Varchar(max)
	,@Param_BlockIds AS Varchar(max)
	,@Param_GroupIds AS Varchar(max)
	,@Param_EmployeeIDs as varchar(max)
	,@Param_MonthID AS int--Mandatory
	,@Param_Year as int--Mandatory
	,@Param_IsNewJoin as bit
	,@Param_IsOutSheet as int
	,@Param_MinSalary as decimal (18,2)
	,@Param_MaxSalary as decimal (18,2)
	,@Param_IsComp as bit
AS
BEGIN
	--SET @Param_MonthID=12
	--SET @Param_Year=2017
	--SET @Param_BUIDs=''
	--SET @Param_LocationIDs=''
	--SET @Param_DepartmentIDs=''
	--SET @Param_DesignationIDs=''
	--SET @Param_EmployeeIDs=''
	--SET @Param_IsNewJoin=0
	--SET @Param_MinSalary=0
	--SET @Param_MaxSalary=0

	IF OBJECT_ID('tempdb..#tbl_Emp') IS NOT NULL
	BEGIN
		DROP TABLE #tbl_Emp
	END
	IF OBJECT_ID('tempdb..#tbl_EmpID') IS NOT NULL
	BEGIN
		DROP TABLE #tbl_EmpID
	END
	IF OBJECT_ID('tempdb..#tbl_EmpBasic') IS NOT NULL
	BEGIN
		DROP TABLE #tbl_EmpBasic
	END
	IF OBJECT_ID('tempdb..#tbl_EmpOff') IS NOT NULL
	BEGIN
		DROP TABLE #tbl_EmpOff
	END
	IF OBJECT_ID('tempdb..#tbl_AttDaily') IS NOT NULL
	BEGIN
		DROP TABLE #tbl_AttDaily
	END
		IF OBJECT_ID('tempdb..#tbl_ESD') IS NOT NULL
	BEGIN
		DROP TABLE #tbl_ESD
	END
	IF OBJECT_ID('tempdb..#tbl_SalarySheet') IS NOT NULL
	BEGIN
		DROP TABLE #tbl_SalarySheet
	END

	CREATE TABLE #tbl_Emp (ESID int) 
	CREATE CLUSTERED INDEX IDX_tbl_EmpID ON #tbl_Emp (ESID);

	CREATE TABLE #tbl_ESD(EmployeeSalaryID int,SalaryHeadID int,SType smallint,Amount Decimal(18,2), SalaryHeadName varchar(100))	
	

	DECLARE
	@PV_SQL as varchar(max)
	,@PV_StartDate As DATE
	,@PV_EndDate AS DATE
	SET @PV_SQL=''

	IF NOT EXISTS (SELECT * FROM PayrollProcessManagement WHERE MonthID=@Param_MonthID AND DATEPART(YEAR,SalaryTo)=@Param_Year)
	BEGIN
		ROLLBACK
			RAISERROR (N'Invalid Year & Month',16,1)
		RETURN
	END

	--Get Start Date & End Date
	SELECT top(1)@PV_StartDate=SalaryFrom,@PV_EndDate=SalaryTo 
	FROM PayrollProcessManagement WHERE MonthID=@Param_MonthID AND DATEPART(YEAR,SalaryTo)=@Param_Year
	
	SET @Param_IsComp= ISNULL(@Param_IsComp,0)

	SET @PV_SQL='SELECT EmployeeSalaryID FROM EmployeeSalary WHERE StartDate='''+CONVERT(VARCHAR(50),@PV_StartDate)+''' AND EndDate='''+CONVERT(VARCHAR(50),@PV_EndDate)+''''
	
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

	IF @Param_IsOutSheet IS NOT NULL AND @Param_IsOutSheet=1
	BEGIN
		SET @PV_SQL=@PV_SQL+ ' AND IsOutSheet = 1'
	END

	IF @Param_MinSalary>0 AND @Param_MaxSalary>0
	BEGIN
		IF @Param_IsComp=1
		BEGIN
			SET @PV_SQL=@PV_SQL+ ' AND CompGrossAmount BETWEEN '+CONVERT(VARCHAR(50),@Param_MinSalary)+' AND '+CONVERT(VARCHAR(50),@Param_MaxSalary)+''
		END ELSE BEGIN
			SET @PV_SQL=@PV_SQL+ ' AND GrossAmount BETWEEN '+CONVERT(VARCHAR(50),@Param_MinSalary)+' AND '+CONVERT(VARCHAR(50),@Param_MaxSalary)+''
		END
	END

	IF (@Param_BUIDs IS NOT NULL AND @Param_BUIDs<>'')
	BEGIN
		SET @PV_SQL=@PV_SQL+ ' AND PayrollProcessID IN (SELECT PPMID FROM PayrollProcessManagement WHERE BusinessUnitID IN ('+@Param_BUIDs+'))'
	END

	IF (@Param_SalarySchemeIDs IS NOT NULL AND @Param_SalarySchemeIDs <>'')
	BEGIN
		SET @PV_SQL=@PV_SQL+ ' AND EmployeeID IN (SELECT EmployeeID FROM EmployeeSalaryStructure WHERE SalarySchemeID IN ('+@Param_SalarySchemeIDs+'))'
	END

	IF (@Param_IsNewJoin IS NOT NULL AND @Param_IsNewJoin <>0)
	BEGIN
		SET @PV_SQL=@PV_SQL+ ' AND EmployeeID IN (SELECT EmployeeID FROM EmployeeOfficial WHERE DateOfJoin BETWEEN '''+CONVERT(VARCHAR(50),@PV_StartDate)+''' AND '''+CONVERT(VARCHAR(50),@PV_EndDate)+''')'
	END

	--IF @Param_GroupIds IS NOT NULL AND @Param_GroupIds <>''
	--BEGIN
	--	IF (@Param_BlockIds IS NOT NULL AND @Param_BlockIds <>'')
	--	BEGIN
	--		SET @Param_BlockIds=@Param_BlockIds+','+@Param_GroupIds
	--	END ELSE BEGIN
	--		SET @Param_BlockIds=@Param_GroupIds
	--	END
	--END

	--IF (@Param_BlockIds IS NOT NULL AND @Param_BlockIds <>'')
	--BEGIN
	--	SET @PV_SQL=@PV_SQL+ ' AND EmployeeID IN (SELECT EmployeeID FROM EmployeeGroup WHERE EmployeeTypeID IN('+@Param_BlockIds+'))'
	--END
	IF @Param_GroupIds<>'' AND @Param_GroupIds IS NOT NULL
	BEGIN
		SET @PV_SQL=@PV_SQL+' AND EmployeeID IN( SELECT EmployeeID From EmployeeGroup WHERE EmployeeTypeID IN(' + @Param_GroupIds + ') AND EmployeeTypeID IN(SELECT EmployeeTypeID FROM EmployeeType WHERE EmployeeGrouping=2))'
	END
	IF @Param_BlockIds<>'' AND @Param_BlockIds IS NOT NULL
	BEGIN
		SET @PV_SQL=@PV_SQL+' AND EmployeeID IN( SELECT EmployeeID From EmployeeGroup WHERE EmployeeTypeID IN(' + @Param_BlockIds + ') AND EmployeeTypeID IN(SELECT EmployeeTypeID FROM EmployeeType WHERE EmployeeGrouping=3))'
	END
	
	--SELECT @PV_SQL
	
	--Get EmployeeID
	INSERT INTO #tbl_Emp
	EXEC(@PV_SQL)

	--Get Employee SalaryDetail	
	INSERT INTO #tbl_ESD 
	SELECT EmployeeSalaryID,SalaryHeadID
	,(SELECT SalaryHeadType FROM SalaryHead WHERE SalaryHeadID=EmployeeSalaryDetail.SalaryHeadID) 
	,CASE WHEN @Param_IsComp=1 THEN ISNULL(CompAmount,0) ELSE ISNULL(Amount,0) END Amount
	,(SELECT Name FROM SalaryHead WHERE SalaryHeadID=EmployeeSalaryDetail.SalaryHeadID)
	FROM EmployeeSalaryDetail WHERE EmployeeSalaryID IN (
	SELECT ESID FROm #tbl_Emp)
	
	SELECT * FROM #tbl_ESD
	DROP TABLE #tbl_ESD
	DROP TABLE #tbl_Emp

END



GO
/****** Object:  StoredProcedure [dbo].[SP_Rpt_SalarySummary_BUWise_Group]    Script Date: 11/12/2018 10:10:41 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[SP_Rpt_SalarySummary_BUWise_Group]
(
		--DECLARE

		@Param_BusinessUnitIDs VARCHAR(MAX),
		@Param_LocationIDs VARCHAR(MAX),
		@Param_DepartmentIDs VARCHAR(MAX),
		@Param_DesignationIDs VARCHAR(MAX),
		@Param_SalarySchemeIDs VARCHAR(MAX),
		@Param_EmployeeIDs VARCHAR(MAX),
		@Param_PayType INT,
		@Param_MonthID INT,
		@Param_Year INT,
		@Param_NewJoin BIT,
		@Param_UserId INT,
		@Param_EmpGrouping INT,
		@Param_MinSalary DECIMAL(18, 2),
		@Param_MaxSalary DECIMAL(18, 2)

)

AS
BEGIN 
	DECLARE
	@PV_StartDate AS DATE
	,@PV_EndDate AS DATE

	IF OBJECT_ID('tempdb..#tbl_SalarySummery') IS NOT NULL
	BEGIN
		DROP TABLE #tbl_SalarySummery
	END
	IF OBJECT_ID('tempdb..#tbl_EmployeeSalary') IS NOT NULL
	BEGIN
		DROP TABLE #tbl_EmployeeSalary
	END
	IF OBJECT_ID('tempdb..#tbl_Cash') IS NOT NULL
	BEGIN
		DROP TABLE #tbl_Cash
	END
	IF OBJECT_ID('tempdb..#EmpGroup') IS NOT NULL
	BEGIN
		DROP TABLE #EmpGroup
	END

	CREATE TABLE #tbl_EmployeeSalary(StartDate DATE,EndDate DATE, EmployeeID INT,BusinessUnitID INT ,BusinessUnitName VARCHAR(250) , LocationID INT,LocationName VARCHAR(250)
	,DepartmentID INT,DepartmentName VARCHAR(100),OTHr DECIMAL(30,17) ,GrossSalary DECIMAL(30,17),OTAmount DECIMAL(30,17)
	,TotalPayable DECIMAL(30,17), Stamp DECIMAL(30,17), NetPay DECIMAL(30,17), AccountNo VARCHAR(100))

	

	CREATE TABLE #tbl_SalarySummery(StartDate DATE,EndDate DATE,NoOfEmp INT,OTHr DECIMAL(30,17) ,GrossSalary DECIMAL(30,17),OTAmount DECIMAL(30,17)
	,TotalPayable DECIMAL(30,17), Stamp DECIMAL(30,17), NetPay DECIMAL(30,17), BankPay DECIMAL(30,17), CashPay DECIMAL(30,17), EmployeeTypeID INT, GroupName VARCHAR(512))

	CREATE TABLE #tbl_EmpGroup(EmployeeID INT, EmployeeTypeID INT, Name VARCHAR(512))

	CREATE TABLE #tbl_Cash(EmpID INT, CashAmount DECIMAL(30,17))
	--Check OT Allowance in SalarySheetProperty
	DECLARE @PV_IsOTAllowance as bit =1
	IF EXISTS(SELECT * FROM SalarySheetProperty WHERE SalarySheetFormatProperty=410 AND IsActive=0)
	BEGIN
		SET @PV_IsOTAllowance=0
	END

	DECLARE @sSQL as NVARCHAR (MAX)
	--'SELECT EmployeeID,EmployeeSalaryID FROM EmployeeSalary WHERE StartDate='''+CONVERT(VARCHAR(50),@PV_StartDate)+''' AND EndDate='''+CONVERT(VARCHAR(50),@PV_EndDate)+''''
	SET @sSQL='SELECT VES.StartDate,VES.EndDate,VES.EmployeeID,PPM.BusinessUnitID,BU.Name AS BUName,VES.LocationID,Loc.Name AS LocationName,VES.DepartmentID,Dept.Name As DepartmentName
	,VES.OTHour,VES.GrossAmount,VES.OTHour*VES.OTRatePerHour,VES.GrossAmount+VES.OTHour*VES.OTRatePerHour,VES.RevenueStemp
	,VES.NetAmount
	,(SELECT top(1)AccountNo FROM EmployeeBankAccount WHERE EmployeeID=VES.EmployeeID AND IsActive=1)
	FROM EmployeeSalary VES
	INNER JOIN PayrollProcessManagement PPM ON VES.PayrollProcessID=PPM.PPMID
	INNER JOIN BusinessUnit BU ON PPM.BusinessUnitID=BU.BusinessUnitID
	INNER JOIN Location Loc ON VES.LocationID=Loc.LocationID
	INNER JOIN Department Dept ON VES.DepartmentID=Dept.DepartmentID
	INNER JOIN EmployeeOfficial EO ON VES.EmployeeID=EO.EmployeeID
	INNER JOIN DepartmentRequirementPolicy DRP ON EO.DRPID=DRP.DepartmentRequirementPolicyID
	WHERE  MonthID='+CONVERT(VARCHAR(100),@Param_MonthID) + ' AND DATEPART(YYYY,EndDate)='+CONVERT(VARCHAR(100), @Param_Year)
	
	IF(@Param_EmployeeIDs!='')
	BEGIN
		SET @sSQL+=' AND VES.EmployeeID IN('+@Param_EmployeeIDs+')'
	END
	ELSE
	BEGIN
		IF (@Param_BusinessUnitIDs !='' AND @Param_BusinessUnitIDs IS NOT NULL)
		BEGIN
			--SET  @sSQL=@sSQL+' AND EmployeeID IN(SELECT EmployeeID FROM EmployeeOfficial WHERE DRPID IN(SELECT DepartmentRequirementPolicyID FROM DepartmentRequirementPolicy WHERE BusinessUnitID IN('+CONVERT(varchar(50),@Param_BusinessUnitIDs)+')))'
			SET  @sSQL=@sSQL+' AND DRP.BusinessUnitID IN('+CONVERT(varchar(50),@Param_BusinessUnitIDs)+')'
		END
		IF (@Param_LocationIDs !='' AND @Param_LocationIDs IS NOT NULL)
		BEGIN
			SET  @sSQL=@sSQL+' AND DRP.LocationID IN('+CONVERT(varchar(50),@Param_LocationIDs)+')'
		END
		IF(@Param_DepartmentIDs!='' AND @Param_DepartmentIDs IS NOT NULL)
		BEGIN
			SET @sSQL+=' AND DRO.DepartmentID IN('+@Param_DepartmentIDs+')'
		END
		IF(@Param_DesignationIDs!='' AND @Param_DesignationIDs IS NOT NULL)
		BEGIN
			SET @sSQL+=' AND EO.DesignationID IN('+@Param_DesignationIDs+')'
		END
		IF(@Param_SalarySchemeIDs!='' AND @Param_SalarySchemeIDs IS NOT NULL)
		BEGIN
			SET @sSQL+=' AND VES.EmployeeID IN(SELECT EmployeeID FROM EmployeeSalaryStructure WHERE SalarySchemeID IN('+@Param_SalarySchemeIDs+'))'
		END
		IF (@Param_PayType =1 AND @Param_PayType IS NOT NULL)
		BEGIN
			SET  @sSQL=@sSQL+' AND  IsAllowBankAccount=1'
		END
		IF (@Param_PayType =2 AND @Param_PayType IS NOT NULL)
		BEGIN
			SET  @sSQL=@sSQL+' AND  IsAllowBankAccount=0'
		END
		IF (@Param_NewJoin !=0 AND @Param_NewJoin IS NOT NULL)
		BEGIN
			SET  @sSQL=@sSQL+' AND JoiningDate BETWEEN StartDate AND EndDate'
		END
		
		IF @Param_MinSalary>0 AND @Param_MaxSalary>0
		BEGIN
			SET @sSQL=@sSQL+ ' AND GrossAmount BETWEEN '+CONVERT(VARCHAR(50),@Param_MinSalary)+' AND '+CONVERT(VARCHAR(50),@Param_MaxSalary)+''
		END

		IF EXISTS(SELECT * FROM Users WHERE userID = @Param_UserId AND FinancialUserType!=1)
		BEGIN
			 SET @sSQL = @sSQL + ' AND EO.DRPID IN(SELECT DRPID FROM DepartmentRequirementPolicyPermission WHERE UserID ='  + CONVERT(VARCHAR(50),@Param_UserId)+')'
		END
	END
	SET @sSQL += ' AND VES.EmployeeID IN(select EmployeeID from EmployeeGroup where EmployeeTypeID IN(select EmployeeTypeID from EmployeeType where EmployeeGrouping IN('+CONVERT(VARCHAR(20),@Param_EmpGrouping)+')))'
	
	INSERT INTO #tbl_EmployeeSalary 
	EXEC(@sSQL)
	

	INSERT INTO #tbl_Cash
	SELECT EmployeeID,CashAmount FROM EmployeeSalaryStructure WHERE CashAmount>0 AND EmployeeID IN (
	SELECT EmployeeID FROM #tbl_EmployeeSalary)

	INSERT INTO #tbl_EmpGroup 
	SELECT EmployeeID
		, EmployeeTypeID
		, (SELECT Name FROM EmployeeType WHERE EmployeeTypeID = EG.EmployeeTypeID)
	FROM EmployeeGroup EG WHERE EmployeeID IN(
	SELECT EmployeeID FROM #tbl_EmployeeSalary)

	SET @PV_StartDate =(SELECT TOP(1)StartDate FROM #tbl_EmployeeSalary)
	SET @PV_EndDate =(SELECT TOP(1)EndDate FROM #tbl_EmployeeSalary)


	INSERT INTO #tbl_SalarySummery 
	SELECT  @PV_StartDate
			,@PV_EndDate
			,COUNT(ES.EmployeeID)
			,SUM(OTHr)
			,SUM(GrossSalary)
			,SUM(OTAmount) AS OTAmount
			,SUM(GrossSalary+OTAmount) AS Gross
			,SUM(Stamp)
			,SUM(NetPay)			
			,SUM(CASE WHEN AccountNo IS NOT NULL THEN ES.NetPay-ISNULL(EC.CashAmount,0) ELSE 0 END) AS  BankAmount
			,SUM(CASE WHEN AccountNo IS NOT NULL THEN ISNULL(EC.CashAmount,0) ELSE ES.NetPay END) AS  CashAmount
			,EG.EmployeeTypeID
			,EG.Name
	FROM #tbl_EmployeeSalary ES
	LEFT JOIN #tbl_Cash EC ON ES.EmployeeID=EC.EmpID
	LEFT JOIN #tbl_EmpGroup EG ON ES.EmployeeID = EG.EmployeeID
	GROUP BY EG.EmployeeTypeID,EG.Name


	SELECT StartDate,EndDate
	,NoOfEmp,OTHr, OTAmount,TotalPayable,NetPay, EmployeeTypeID, GroupName
	,GrossSalary--CASE WHEN @PV_IsOTAllowance=0 THEN GrossSalary-OTAmount ELSE GrossSalary END AS GrossSalary
	,BankPay--CASE WHEN @PV_IsOTAllowance=0 AND BankPay>0 THEN BankPay-OTAmount ELSE BankPay END AS BankPay
	,CashPay--CASE WHEN @PV_IsOTAllowance=0 AND BankPay<=0 AND CashPay>0 THEN CashPay-OTAmount ELSE CashPay END AS CashPay
	FROM #tbl_SalarySummery SS


	--SELECT *
	--FROM #tbl_SalarySummery 

	DROP TABLE #tbl_SalarySummery
	DROP TABLE #tbl_EmployeeSalary
	DROP TABLE #tbl_Cash
	DROP TABLE #tbl_EmpGroup
	
END 
	















GO
/****** Object:  StoredProcedure [dbo].[SP_Rpt_SalarySummary_F2]    Script Date: 11/12/2018 10:10:41 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[SP_Rpt_SalarySummary_F2]
(
		--DECLARE

		@Param_BusinessUnitIDs VARCHAR(MAX),
		@Param_LocationIDs VARCHAR(MAX),
		@Param_DepartmentIDs VARCHAR(MAX),
		@Param_DesignationIDs VARCHAR(MAX),
		@Param_SalarySchemeIDs VARCHAR(MAX),
		@Param_EmployeeIDs VARCHAR(MAX),
		@Param_PayType INT,
		@Param_MonthID INT,
		@Param_Year INT,
		@Param_NewJoin BIT,
		@Param_UserId INT,
		@Param_sGroupIDs VARCHAR(MAX),
		@Param_sBlockIDs VARCHAR(MAX),
		@Param_MinSalary DECIMAL(18, 2),
		@Param_MaxSalary DECIMAL(18, 2)

)

AS
BEGIN 
	DECLARE
	@PV_StartDate AS DATE
	,@PV_EndDate AS DATE
	,@PV_MonthCycleDay INT
	,@PV_IsOTAllowance as bit
	SET @PV_IsOTAllowance=1
	IF OBJECT_ID('tempdb..#tbl_SalarySummery') IS NOT NULL
	BEGIN
		DROP TABLE #tbl_SalarySummery
	END
	IF OBJECT_ID('tempdb..#tbl_EmployeeSalary') IS NOT NULL
	BEGIN
		DROP TABLE #tbl_EmployeeSalary
	END
	
	IF OBJECT_ID('tempdb..#tbl_EarningOnAtt') IS NOT NULL
	BEGIN
		DROP TABLE #tbl_EarningOnAtt
	END
	IF OBJECT_ID('tempdb..#tbl_ESD') IS NOT NULL
	BEGIN
		DROP TABLE #tbl_ESD
	END
	IF OBJECT_ID('tempdb..#tbl_EarmingsOnAttVal') IS NOT NULL
	BEGIN
		DROP TABLE #tbl_EarmingsOnAttVal
	END

	CREATE TABLE #tbl_EmployeeSalary(EmployeeSalaryID INT,StartDate DATE,EndDate DATE, EmployeeID INT,BusinessUnitID INT ,BusinessUnitName VARCHAR(250) , LocationID INT,LocationName VARCHAR(250)
	,DepartmentID INT,DepartmentName VARCHAR(100),OTHr DECIMAL(30,17) ,GrossSalary DECIMAL(30,17),OTAmount DECIMAL(30,17)
	,TotalPayable DECIMAL(30,17), Stamp DECIMAL(30,17), NetPay DECIMAL(30,17), AccountNo VARCHAR(100), BankAmount DECIMAL(18, 2), CashAmount DECIMAL(18, 2), BankAccountID INT)

	

	CREATE TABLE #tbl_SalarySummery(StartDate DATE,EndDate DATE,BusinessUnitID INT ,BusinessUnitName VARCHAR(250),BusinessUnitAddress VARCHAR(250) , LocationID INT,LocationName VARCHAR(250)
	,DepartmentID INT,DepartmentName VARCHAR(100),NoOfEmp INT,OTHr DECIMAL(30,17) ,GrossSalary DECIMAL(30,17),OTAmount DECIMAL(30,17)
	,TotalPayable DECIMAL(30,17), Stamp DECIMAL(30,17), NetPay DECIMAL(30,17), BankPay DECIMAL(30,17), CashPay DECIMAL(30,17), EOA DECIMAL(30,17))
	
	CREATE TABLE #tbl_EmpAtt(EmployeeID INT, P INT,TotalDOff int, TotalPLeave INT)
	CREATE TABLE #tbl_EarningOnAtt(EmployeeID INT, Gross DECIMAL(30,17), PerDayGross DECIMAL(30, 17),OTAmount DECIMAL(30,17))
	CREATE TABLE #tbl_ESD(EmployeeID INT, ESID int,Amount Decimal(18,2))
	CREATE TABLE #tbl_EarmingsOnAttVal(EmployeeID INT, EOA DECIMAL(30, 17))	

	CREATE TABLE #tbl_Cash(EmpID INT, CashAmount DECIMAL(30,17))
	--Check OT Allowance in SalarySheetProperty
	
	IF EXISTS(SELECT * FROM SalarySheetProperty WHERE SalarySheetFormatProperty=410 AND IsActive=0)
	BEGIN SET @PV_IsOTAllowance=0 END ELSE BEGIN SET @PV_IsOTAllowance=1 END

	DECLARE @sSQL as NVARCHAR (MAX)
	--'SELECT EmployeeID,EmployeeSalaryID FROM EmployeeSalary WHERE StartDate='''+CONVERT(VARCHAR(50),@PV_StartDate)+''' AND EndDate='''+CONVERT(VARCHAR(50),@PV_EndDate)+''''
	SET @sSQL='SELECT VES.EmployeeSalaryID,VES.StartDate,VES.EndDate,VES.EmployeeID,PPM.BusinessUnitID,BU.Name AS BUName,VES.LocationID,Loc.Name AS LocationName,VES.DepartmentID,Dept.Name As DepartmentName
	,VES.OTHour,VES.GrossAmount,VES.OTHour*VES.OTRatePerHour,VES.GrossAmount+VES.OTHour*VES.OTRatePerHour,VES.RevenueStemp
	,VES.NetAmount
	,(SELECT top(1)AccountNo FROM EmployeeBankAccount WHERE EmployeeID=VES.EmployeeID AND IsActive=1)
	,BankAmount
	,CashAmount
	,BankAccountID
	FROM EmployeeSalary VES
	INNER JOIN PayrollProcessManagement PPM ON VES.PayrollProcessID=PPM.PPMID
	INNER JOIN BusinessUnit BU ON PPM.BusinessUnitID=BU.BusinessUnitID
	INNER JOIN Location Loc ON VES.LocationID=Loc.LocationID
	INNER JOIN Department Dept ON VES.DepartmentID=Dept.DepartmentID
	WHERE  MonthID='+CONVERT(VARCHAR(100),@Param_MonthID) + ' AND DATEPART(YYYY,EndDate)='+CONVERT(VARCHAR(100), @Param_Year)
	
	IF(@Param_EmployeeIDs!='')
	BEGIN
		SET @sSQL+=' AND VES.EmployeeID IN('+@Param_EmployeeIDs+')'
	END
	ELSE
	BEGIN
		IF (@Param_BusinessUnitIDs !='' AND @Param_BusinessUnitIDs IS NOT NULL)
		BEGIN
			--SET  @sSQL=@sSQL+' AND VES.EmployeeID IN(SELECT EmployeeID FROM EmployeeOfficial WHERE DRPID IN(SELECT DepartmentRequirementPolicyID FROM DepartmentRequirementPolicy WHERE BusinessUnitID IN('+CONVERT(varchar(50),@Param_BusinessUnitIDs)+')))'
			SET  @sSQL=@sSQL+' AND PPM.BusinessUnitID IN ('+CONVERT(varchar(50),@Param_BusinessUnitIDs)+')'
		END
		IF (@Param_LocationIDs !='' AND @Param_LocationIDs IS NOT NULL)
		BEGIN
			SET  @sSQL=@sSQL+' AND VES.LocationID IN('+CONVERT(varchar(50),@Param_LocationIDs)+')'
		END
		IF(@Param_DepartmentIDs!='' AND @Param_DepartmentIDs IS NOT NULL)
		BEGIN
			SET @sSQL+=' AND VES.DepartmentID IN('+@Param_DepartmentIDs+')'
		END
		IF(@Param_DesignationIDs!='' AND @Param_DesignationIDs IS NOT NULL)
		BEGIN
			SET @sSQL+=' AND VES.DesignationID IN('+@Param_DesignationIDs+')'
		END
		IF(@Param_SalarySchemeIDs!='' AND @Param_SalarySchemeIDs IS NOT NULL)
		BEGIN
			SET @sSQL+=' AND VES.EmployeeID IN(SELECT EmployeeID FROM EmployeeSalaryStructure WHERE SalarySchemeID IN('+@Param_SalarySchemeIDs+'))'
		END
		IF (@Param_PayType =1 AND @Param_PayType IS NOT NULL)
		BEGIN
			SET  @sSQL=@sSQL+' AND  VES.EmployeeID IN (SELECT EmployeeID FROM EmployeeSalaryStructure WHERE SalarySchemeID IN (SELECT SalarySchemeID FROM SalaryScheme WHERE IsAllowBankAccount=1))'
		END
		IF (@Param_PayType =2 AND @Param_PayType IS NOT NULL)
		BEGIN
			SET  @sSQL=@sSQL+' AND  VES.EmployeeID IN (SELECT EmployeeID FROM EmployeeSalaryStructure WHERE SalarySchemeID IN (SELECT SalarySchemeID FROM SalaryScheme WHERE IsAllowBankAccount=0))'
		END
		IF @Param_sGroupIDs<>'' AND @Param_sGroupIDs IS NOT NULL
		BEGIN
			SET @sSQL=@sSQL+' AND VES.EmployeeID IN( SELECT EmployeeID From EmployeeGroup WHERE EmployeeTypeID IN(' + @Param_sGroupIDs + '))'
		END
		IF @Param_sBlockIDs<>'' AND @Param_sBlockIDs IS NOT NULL
		BEGIN
			SET @sSQL=@sSQL+' AND VES.EmployeeID IN( SELECT EmployeeID From EmployeeGroup WHERE EmployeeTypeID IN(' + @Param_sBlockIDs + '))'
		END
		IF (@Param_NewJoin !=0 AND @Param_NewJoin IS NOT NULL)
		BEGIN
			SET  @sSQL=@sSQL+' AND VES.EmployeeID IN(SELECT EmployeeID FROM EmployeeOfficial WHERE DateOfJoin BETWEEN StartDate AND EndDate)'
		END
		IF @Param_MinSalary>0 AND @Param_MaxSalary>0
		BEGIN
			SET @sSQL=@sSQL+ ' AND GrossAmount BETWEEN '+CONVERT(VARCHAR(50),@Param_MinSalary)+' AND '+CONVERT(VARCHAR(50),@Param_MaxSalary)+''
		END
		IF EXISTS(SELECT * FROM Users WHERE userID = @Param_UserId AND FinancialUserType!=1)
		BEGIN
			 SET @sSQL = @sSQL + ' AND VES.DepartmentID IN(SELECT DepartmentID FROM DepartmentRequirementPolicy WHERE DepartmentRequirementPolicyID IN(SELECT DRPID FROM DepartmentRequirementPolicyPermission WHERE UserID ='  + CONVERT(VARCHAR(50),@Param_UserId)+'))'
		END
	END
	

	INSERT INTO #tbl_EmployeeSalary 
	EXEC(@sSQL)

	INSERT INTO #tbl_Cash
	SELECT EmployeeID,CashAmount FROM EmployeeSalaryStructure WHERE CashAmount>0 AND EmployeeID IN (
	SELECT EmployeeID FROM #tbl_EmployeeSalary)

	SET @PV_StartDate =(SELECT TOP(1)StartDate FROM #tbl_EmployeeSalary)
	SET @PV_EndDate =(SELECT TOP(1)EndDate FROM #tbl_EmployeeSalary)

	SET @PV_MonthCycleDay = (SELECT DATEDIFF(DAY, @PV_StartDate,@PV_EndDate) + 1)
	INSERT INTO #tbl_EmpAtt
	SELECT EmployeeID
		,ISNULL(TotalWorkingDay,0)-ISNULL(TotalAbsent,0)-ISNULL(TotalPLeave,0)-ISNULL(TotalUpLeave,0)
		,ISNULL(TotalDayOff,0)
		,ISNULL(TotalPLeave,0)
	FROM EmployeeSalary WHERE EmployeeSalaryID IN(
	SELECT EmployeeSalaryID FROM #tbl_EmployeeSalary)
	
	--select * from EmployeeSalary where EmployeeSalaryID=16713
	--select * from #tbl_EmployeeSalary

	INSERT INTO #tbl_EarningOnAtt
	SELECT EmployeeID
		,ISNULL(GrossAmount,0)
		,ISNULL(ROUND((GrossAmount/@PV_MonthCycleDay),2), 0)
		,ISNULL(ROUND((OTHour * OTRatePerHour),2),0)
	FROM EmployeeSalary WHERE EmployeeSalaryID IN(
	SELECT EmployeeSalaryID FROM #tbl_EmployeeSalary)
	
	--select * from #tbl_EarningOnAtt
	INSERT INTO #tbl_ESD 
	SELECT 
	ES.EmployeeID
	,EmployeeSalaryDetail.EmployeeSalaryID
	,SUM(CASE WHEN (SalaryHead.SalaryHeadType=2 AND Amount>0) THEN EmployeeSalaryDetail.Amount ELSE 0 END)
	--,ISNULL(CASE WHEN (SalaryHead.SalaryHeadType=2 AND Amount>0) THEN EmployeeSalaryDetail.Amount ELSE 0 END,0)
	FROM EmployeeSalaryDetail
	LEFT JOIN SalaryHead ON EmployeeSalaryDetail.SalaryHeadID=SalaryHead.SalaryHeadID
	INNER JOIN #tbl_EmployeeSalary ES ON ES.EmployeeSalaryID=EmployeeSalaryDetail.EmployeeSalaryID
	GROUP BY ES.EmployeeID, EmployeeSalaryDetail.EmployeeSalaryID
	
	--select * from #tbl_EmpAtt
	--select * from #tbl_EarningOnAtt
	--select * from #tbl_ESD

	INSERT INTO #tbl_EarmingsOnAttVal
	SELECT EOA.EmployeeID
		,((EA.P + EA.TotalDOff + EA.TotalPLeave) * EOA.PerDayGross) + EOA.OTAmount + ES.Amount
	FROM #tbl_ESD ES
	INNER JOIN #tbl_EmpAtt EA ON EA.EmployeeID = ES.EmployeeID
	INNER JOIN #tbl_EarningOnAtt EOA ON EOA.EmployeeID=ES.EmployeeID


	INSERT INTO #tbl_SalarySummery 
		SELECT @PV_StartDate AS StartDate
				,@PV_EndDate AS EndDate	 			
				,tab.BusinessUnitID
				,tab.BusinessUnitName
				,(SELECT [Address] FROM BusinessUnit WHERE BusinessUnitID=tab.BusinessUnitID) AS [Address]
				,tab.LocationID
				,tab.LocationName
				,tab.DepartmentID
				,tab.DepartmentName
				,COUNT(tab.EmployeeID) AS EmployeeID
				,SUM(tab.OTHr) AS OTHr
				,SUM(tab.GrossSalary) AS GrossSalary
				,SUM(tab.OTAmount) AS OTAmount
				,SUM(CASE WHEN @PV_IsOTAllowance=0 THEN tab.Gross-tab.OTAmount ELSE tab.Gross END) AS Gross
				,SUM(tab.Stamp) AS Stamp
				,SUM(CASE WHEN @PV_IsOTAllowance=0 THEN tab.NetPay-tab.OTAmount ELSE tab.NetPay END) AS NetPay
				--,SUM(CASE WHEN @PV_IsOTAllowance=0 AND ISNULL(tab.BankAmount,0)>0 THEN  tab.BankAmount-tab.OTAmount ELSE tab.BankAmount END) AS BankAmount
				--,SUM(CASE WHEN @PV_IsOTAllowance=0 AND ISNULL(tab.BankAmount,0)<=0 THEN  tab.CashAmount-tab.OTAmount ELSE tab.CashAmount END) AS CashAmount
				,SUM(tab.BankAmount)
				,SUM(tab.CashAmount)
				,SUM(tab.EOA)
			FROM (
			SELECT  
					ES.BusinessUnitID
					,ES.BusinessUnitName
					,ES.LocationID
					,ES.LocationName
					,ES.DepartmentID
					,ES.DepartmentName--
					,ES.EmployeeID As EmployeeID
					,ES.OTHr AS OTHr
					,ES.GrossSalary AS GrossSalary
					,ES.OTAmount AS OTAmount
					,ES.GrossSalary+ES.OTAmount AS Gross
					,ES.Stamp AS Stamp
					,ES.NetPay AS NetPay			
					--,CASE WHEN ES.AccountNo IS NOT NULL THEN ES.NetPay-ISNULL(EC.CashAmount,0) ELSE 0 END AS  BankAmount
					--,CASE WHEN ES.AccountNo IS NOT NULL THEN ISNULL(EC.CashAmount,0) ELSE ES.NetPay END AS  CashAmount
					,ES.BankAmount
					,ES.CashAmount
					,EOA.EOA AS EOA
			FROM #tbl_EmployeeSalary ES
			LEFT JOIN #tbl_Cash EC ON ES.EmployeeID=EC.EmpID
			LEFT JOIN #tbl_EarmingsOnAttVal EOA ON ES.EmployeeID = EOA.EmployeeID)tab 
			GROUP BY tab.DepartmentID,tab.DepartmentName,tab.LocationID,tab.LocationName,tab.BusinessUnitID,tab.BusinessUnitName


	SELECT StartDate,EndDate,BusinessUnitID,BusinessUnitName,BusinessUnitAddress,LocationID,LocationName,DepartmentID,DepartmentName
	,NoOfEmp,OTHr, OTAmount,TotalPayable
	,NetPay
	,GrossSalary
	,BankPay
	,CashPay
	,EOA
	FROM #tbl_SalarySummery





	--SELECT *
	--FROM #tbl_SalarySummery 

	DROP TABLE #tbl_SalarySummery
	DROP TABLE #tbl_EmployeeSalary
	DROP TABLE #tbl_Cash
	DROP TABLE #tbl_EmpAtt
	DROP TABLE #tbl_EarningOnAtt
	DROP TABLE #tbl_ESD
	DROP TABLE #tbl_EarmingsOnAttVal
	
END 
	
















GO
/****** Object:  StoredProcedure [dbo].[SP_Rpt_SalarySummaryDetail_BUWise_Group]    Script Date: 11/12/2018 10:10:41 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


/****** Object:  StoredProcedure [dbo].[SP_Rpt_MonthlyAttendance]    Script Date: 5/14/2015 9:52:45 AM ******/
--SET ANSI_NULLS ON
--GO
--SET QUOTED_IDENTIFIER ON
--GO


---- =============================================
---- Author:		MD. Azharul Islam
---- Create date: 05 apr 2017
---- Note:	Get for report salary summary F2
---- =============================================

CREATE PROCEDURE [dbo].[SP_Rpt_SalarySummaryDetail_BUWise_Group]
(
		--DECLARE

		@Param_BusinessUnitIDs VARCHAR(MAX),
		@Param_LocationIDs VARCHAR(MAX),
		@Param_DepartmentIDs VARCHAR(MAX),
		@Param_DesignationIDs VARCHAR(MAX),
		@Param_SalarySchemeIDs VARCHAR(MAX),
		@Param_EmployeeIDs VARCHAR(MAX),
		@Param_PayType INT,
		@Param_MonthID INT,
		@Param_Year INT,
		@Param_NewJoin BIT,
		@Param_UserId INT,
		@Param_EmpGrouping INT,
		@Param_MinSalary DECIMAL(18, 2),
		@Param_MaxSalary DECIMAL(18, 2)

)

AS
BEGIN 
	IF OBJECT_ID('tempdb..#tbl_EmployeeSalaryDetail') IS NOT NULL
	BEGIN
		DROP TABLE #tbl_EmployeeSalaryDetail
	END
	IF OBJECT_ID('tempdb..#tbl_EmployeeSalary') IS NOT NULL
	BEGIN
		DROP TABLE #tbl_EmployeeSalary
	END
	
	
	
	CREATE TABLE #tbl_EmployeeSalary(LocationID INT,BusinessUnitID INT,DepartmentID INT,EmployeeSalaryID INT)
	CREATE TABLE #tbl_EmployeeSalaryDetail(BusinessUnitID INT,LocationID INT,DepartmentID INT, Amount DECIMAL(18,2),SalaryHeadID INT,SalaryHeadName VARCHAR(100),SalaryHeadIDType INT)

	

	DECLARE @sSQL as NVARCHAR (MAX)
	SET @sSQL='SELECT VES.LocationID,BU.BusinessUnitID,VES.DepartmentID,VES.EmployeeSalaryID
	FROM EmployeeSalary VES
	LEFT JOIN EmployeeOfficial EO ON VES.EmployeeID=EO.EmployeeID
	LEFT JOIN DepartmentRequirementPolicy DRP ON EO.DRPID=DRP.DepartmentRequirementPolicyID
	LEFT JOIN BusinessUnit BU ON DRP.BusinessUnitID=BU.BusinessUnitID
	LEFT JOIN PayrollProcessManagement PPM ON VES.PayrollProcessID=PPM.PPMID
	WHERE PPM.MonthID='+CONVERT(VARCHAR(100),@Param_MonthID) + ' AND DATEPART(YYYY,VES.EndDate)='+CONVERT(VARCHAR(100), @Param_Year)


	IF(@Param_EmployeeIDs!='')
	BEGIN
		SET @sSQL+=' AND VES.EmployeeID IN('+@Param_EmployeeIDs+')'
	END
	ELSE
	BEGIN
		IF (@Param_BusinessUnitIDs !='' AND @Param_BusinessUnitIDs IS NOT NULL)
		BEGIN
			--SET  @sSQL=@sSQL+' AND VES.EmployeeID IN(SELECT EmployeeID FROM EmployeeOfficial WHERE DRPID IN(SELECT DepartmentRequirementPolicyID FROM DepartmentRequirementPolicy WHERE BusinessUnitID IN('+CONVERT(varchar(50),@Param_BusinessUnitIDs)+')))'
			SET  @sSQL=@sSQL+' AND BU.BusinessUnitID IN('+CONVERT(varchar(50),@Param_BusinessUnitIDs)+')'
		END
		IF (@Param_LocationIDs !='' AND @Param_LocationIDs IS NOT NULL)
		BEGIN
			SET  @sSQL=@sSQL+' AND VES.LocationID IN('+CONVERT(varchar(50),@Param_LocationIDs)+')'
		END
		IF(@Param_DepartmentIDs!='' AND @Param_DepartmentIDs IS NOT NULL)
		BEGIN
			SET @sSQL+=' AND VES.DepartmentID IN('+@Param_DepartmentIDs+')'
		END
		IF(@Param_DesignationIDs!='' AND @Param_DesignationIDs IS NOT NULL)
		BEGIN
			SET @sSQL+=' AND VES.DesignationID IN('+@Param_DesignationIDs+')'
		END
		IF(@Param_SalarySchemeIDs!='' AND @Param_SalarySchemeIDs IS NOT NULL)
		BEGIN
			SET @sSQL+=' AND VES.EmployeeID IN(SELECT EmployeeID FROM EmployeeSalaryStructure WHERE SalarySchemeID IN('+@Param_SalarySchemeIDs+'))'
		END
		IF (@Param_PayType =1 AND @Param_PayType IS NOT NULL)
		BEGIN
			SET  @sSQL=@sSQL+' AND  VES.EmployeeID IN (SELECT EmployeeID FROM EmployeeSalaryStructure WHERE SalarySchemeID IN (SELECT SalarySchemeID FROM SalaryScheme WHERE IsAllowBankAccount=1))'
		END
		IF (@Param_PayType =2 AND @Param_PayType IS NOT NULL)
		BEGIN
			SET  @sSQL=@sSQL+' AND  VES.EmployeeID IN (SELECT EmployeeID FROM EmployeeSalaryStructure WHERE SalarySchemeID IN (SELECT SalarySchemeID FROM SalaryScheme WHERE IsAllowBankAccount=0))'
		END
		IF (@Param_NewJoin !=0 AND @Param_NewJoin IS NOT NULL)
		BEGIN
			SET  @sSQL=@sSQL+' AND VES.EmployeeID IN(SELECT EmployeeID FROM EmployeeOfficial WHERE DateOfJoin BETWEEN StartDate AND EndDate)'
		END
		IF @Param_MinSalary>0 AND @Param_MaxSalary>0
		BEGIN
			SET @sSQL=@sSQL+ ' AND GrossAmount BETWEEN '+CONVERT(VARCHAR(50),@Param_MinSalary)+' AND '+CONVERT(VARCHAR(50),@Param_MaxSalary)+''
		END
		IF EXISTS(SELECT * FROM Users WHERE userID = @Param_UserId AND FinancialUserType!=1)
		BEGIN
			 SET @sSQL = @sSQL + ' AND VES.DepartmentID IN(SELECT DepartmentID FROM DepartmentRequirementPolicy WHERE DepartmentRequirementPolicyID IN(SELECT DRPID FROM DepartmentRequirementPolicyPermission WHERE UserID ='  + CONVERT(VARCHAR(50),@Param_UserId)+'))'
		END
	END
	--Following code Omited by MMI dated on 08 Nov 2018. Next Search By Employee Group should be work. Param is comming wrong. 
	--SET @sSQL += ' AND VES.EmployeeID IN(select EmployeeID from EmployeeGroup where EmployeeTypeID IN(select EmployeeTypeID from EmployeeType where EmployeeGrouping IN('+CONVERT(VARCHAR(20),@Param_EmpGrouping)+')))'
	
	INSERT INTO #tbl_EmployeeSalary 
	EXEC(@sSQL)
	
	INSERT INTO #tbl_EmployeeSalaryDetail
	
	SELECT aa.BusinessUnitID,aa.LocationID,aa.DepartmentID, SUM(aa.Amount),aa.SalaryHeadID ,aa.SalaryHeadName ,aa.SalaryHeadType FROM (
	SELECT   EmployeeSalaryDetail.SalaryHeadID AS SalaryHeadID
			,EmployeeSalaryDetail.Amount AS Amount
            ,SalaryHead.Name AS SalaryHeadName
            ,SalaryHead.SalaryHeadType AS SalaryHeadType
		    ,#tbl_EmployeeSalary.DepartmentID AS DepartmentID
			,#tbl_EmployeeSalary.LocationID AS  LocationID
			,#tbl_EmployeeSalary.BusinessUnitID AS BusinessUnitID
		    
         
	FROM       EmployeeSalaryDetail
	INNER JOIN SalaryHead ON EmployeeSalaryDetail.SalaryHeadID = SalaryHead.SalaryHeadID
	INNER JOIN #tbl_EmployeeSalary ON EmployeeSalaryDetail.EmployeeSalaryID = #tbl_EmployeeSalary.EmployeeSalaryID) aa
	GROUP BY  BusinessUnitID,LocationID,DepartmentID,SalaryHeadID,SalaryHeadName ,SalaryHeadType 

	SELECT * FROM #tbl_EmployeeSalaryDetail

	DROP TABLE #tbl_EmployeeSalary
	DROP TABLE #tbl_EmployeeSalaryDetail
	
END 













GO
/****** Object:  StoredProcedure [dbo].[SP_Rpt_SalarySummaryDetail_F2]    Script Date: 11/12/2018 10:10:41 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


/****** Object:  StoredProcedure [dbo].[SP_Rpt_MonthlyAttendance]    Script Date: 5/14/2015 9:52:45 AM ******/
--SET ANSI_NULLS ON
--GO
--SET QUOTED_IDENTIFIER ON
--GO


---- =============================================
---- Author:		MD. Azharul Islam
---- Create date: 05 apr 2017
---- Note:	Get for report salary summary F2
---- =============================================

CREATE PROCEDURE [dbo].[SP_Rpt_SalarySummaryDetail_F2]
(
		--DECLARE

		@Param_BusinessUnitIDs VARCHAR(MAX),
		@Param_LocationIDs VARCHAR(MAX),
		@Param_DepartmentIDs VARCHAR(MAX),
		@Param_DesignationIDs VARCHAR(MAX),
		@Param_SalarySchemeIDs VARCHAR(MAX),
		@Param_EmployeeIDs VARCHAR(MAX),
		@Param_PayType INT,
		@Param_MonthID INT,
		@Param_Year INT,
		@Param_NewJoin BIT,
		@Param_UserId INT,
		@Param_sGroupIDs VARCHAR(MAX),
		@Param_sBlockIDs VARCHAR(MAX),
		@Param_MinSalary DECIMAL(18, 2),
		@Param_MaxSalary DECIMAL(18, 2)

)

AS
BEGIN 
	IF OBJECT_ID('tempdb..#tbl_EmployeeSalary') IS NOT NULL
	BEGIN
		DROP TABLE #tbl_EmployeeSalary
	END

	IF OBJECT_ID('tempdb..#tbl_EmployeeSalaryDetail') IS NOT NULL
	BEGIN
		DROP TABLE #tbl_EmployeeSalaryDetail
	END

	CREATE TABLE #tbl_EmployeeSalary(LocationID INT,BusinessUnitID INT,DepartmentID INT,EmployeeSalaryID INT)
	CREATE TABLE #tbl_EmployeeSalaryDetail(BusinessUnitID INT,LocationID INT,DepartmentID INT, Amount DECIMAL(18,2),SalaryHeadID INT,SalaryHeadName VARCHAR(100),SalaryHeadIDType INT)

	DECLARE @sSQL as NVARCHAR (MAX)

	SET @sSQL='SELECT LocationID,BusinessUnitID,DepartmentID,EmployeeSalaryID
	FROM View_EmployeeSalary 
	WHERE MonthID='+CONVERT(VARCHAR(100),@Param_MonthID) + ' AND DATEPART(YYYY,EndDate)='+CONVERT(VARCHAR(100), @Param_Year)

	IF(@Param_EmployeeIDs!='')
	BEGIN
		SET @sSQL+=' AND EmployeeID IN('+@Param_EmployeeIDs+')'
	END
	ELSE
	BEGIN
		IF (@Param_BusinessUnitIDs !='' AND @Param_BusinessUnitIDs IS NOT NULL)
		BEGIN
			--SET  @sSQL=@sSQL+' AND EmployeeID IN(SELECT EmployeeID FROM EmployeeOfficial WHERE DRPID IN(SELECT DepartmentRequirementPolicyID FROM DepartmentRequirementPolicy WHERE BusinessUnitID IN('+CONVERT(varchar(50),@Param_BusinessUnitIDs)+')))'
			SET  @sSQL=@sSQL+' AND BusinessUnitID IN('+CONVERT(varchar(50),@Param_BusinessUnitIDs)+')'
		END
		IF (@Param_LocationIDs !='' AND @Param_LocationIDs IS NOT NULL)
		BEGIN
			SET  @sSQL=@sSQL+' AND LocationID IN('+CONVERT(varchar(50),@Param_LocationIDs)+')'
		END
		IF(@Param_DepartmentIDs!='' AND @Param_DepartmentIDs IS NOT NULL)
		BEGIN
			SET @sSQL+=' AND DepartmentID IN('+@Param_DepartmentIDs+')'
		END
		IF(@Param_DesignationIDs!='' AND @Param_DesignationIDs IS NOT NULL)
		BEGIN
			SET @sSQL+=' AND DesignationID IN('+@Param_DesignationIDs+')'
		END
		IF(@Param_SalarySchemeIDs!='' AND @Param_SalarySchemeIDs IS NOT NULL)
		BEGIN
			SET @sSQL+=' AND EmployeeID IN(SELECT EmployeeID FROM EmployeeSalaryStructure WHERE SalarySchemeID IN('+@Param_SalarySchemeIDs+'))'
		END
		IF (@Param_PayType =1 AND @Param_PayType IS NOT NULL)
		BEGIN
			SET  @sSQL=@sSQL+' AND  IsAllowBankAccount=1'
		END
		IF (@Param_PayType =2 AND @Param_PayType IS NOT NULL)
		BEGIN
			SET  @sSQL=@sSQL+' AND  IsAllowBankAccount=0'
		END
		IF @Param_sGroupIDs<>'' AND @Param_sGroupIDs IS NOT NULL
		BEGIN
			SET @sSQL=@sSQL+' AND EmployeeID IN( SELECT EmployeeID From View_EmployeeGroup WHERE EmployeeTypeID IN(' + @Param_sGroupIDs + '))'
		END
		IF @Param_sBlockIDs<>'' AND @Param_sBlockIDs IS NOT NULL
		BEGIN
			SET @sSQL=@sSQL+' AND EmployeeID IN( SELECT EmployeeID From View_EmployeeGroup WHERE EmployeeTypeID IN(' + @Param_sBlockIDs + '))'
		END
		IF (@Param_NewJoin !=0 AND @Param_NewJoin IS NOT NULL)
		BEGIN
			SET  @sSQL=@sSQL+' AND JoiningDate BETWEEN StartDate AND EndDate'
		END
		IF @Param_MinSalary>0 AND @Param_MaxSalary>0
		BEGIN
			SET @sSQL=@sSQL+ ' AND GrossAmount BETWEEN '+CONVERT(VARCHAR(50),@Param_MinSalary)+' AND '+CONVERT(VARCHAR(50),@Param_MaxSalary)+''
		END
		IF EXISTS(SELECT * FROM Users WHERE userID = @Param_UserId AND FinancialUserType!=1)
		BEGIN
			 SET @sSQL = @sSQL + ' AND DepartmentID IN(SELECT DepartmentID FROM DepartmentRequirementPolicy WHERE DepartmentRequirementPolicyID IN(SELECT DRPID FROM DepartmentRequirementPolicyPermission WHERE UserID ='  + CONVERT(VARCHAR(50),@Param_UserId)+'))'
		END
	END

	INSERT INTO #tbl_EmployeeSalary
	EXEC(@sSQL)


	INSERT INTO #tbl_EmployeeSalaryDetail
	
	SELECT aa.BusinessUnitID,aa.LocationID,aa.DepartmentID, SUM(aa.Amount),aa.SalaryHeadID ,aa.SalaryHeadName ,aa.SalaryHeadType FROM (
	SELECT   EmployeeSalaryDetail.SalaryHeadID AS SalaryHeadID
			,EmployeeSalaryDetail.Amount AS Amount
            ,SalaryHead.Name AS SalaryHeadName
            ,SalaryHead.SalaryHeadType AS SalaryHeadType
		    ,#tbl_EmployeeSalary.DepartmentID AS DepartmentID
			,#tbl_EmployeeSalary.LocationID AS  LocationID
			,#tbl_EmployeeSalary.BusinessUnitID AS BusinessUnitID
		    
         
	FROM       EmployeeSalaryDetail
	INNER JOIN SalaryHead ON EmployeeSalaryDetail.SalaryHeadID = SalaryHead.SalaryHeadID
	INNER JOIN #tbl_EmployeeSalary ON EmployeeSalaryDetail.EmployeeSalaryID = #tbl_EmployeeSalary.EmployeeSalaryID) aa
	GROUP BY  BusinessUnitID,LocationID,DepartmentID,SalaryHeadID,SalaryHeadName ,SalaryHeadType 

	SELECT * FROM #tbl_EmployeeSalaryDetail

	DROP TABLE #tbl_EmployeeSalary
	DROP TABLE #tbl_EmployeeSalaryDetail
	
END 













GO
/****** Object:  View [dbo].[View_Employee_WithImage]    Script Date: 11/12/2018 10:10:41 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO












CREATE View [dbo].[View_Employee_WithImage]
AS
SELECT      Employee.EmployeeID,
			Employee.CompanyID,
			Employee.Code,
			Employee.Name,
			Employee.NickName,
			Employee.Gender,
			Employee.MaritalStatus,
			Employee.FatherName,
			Employee.MotherName,
			Employee.ParmanentAddress,
			Employee.PresentAddress,
			Employee.ContactNo,
			Employee.Email,
			Employee.DateOfBirth,
			Employee.BloodGroup,
			Employee.Height,
			Employee.Weight,
			Employee.IdentificationMart,
			Employee.Photo,
			Employee.Signature,
			Employee.Note,
			Employee.Attachment,
			Employee.IsActive,
			Employee.EmployeeDesignationType,
			Employee.IsFather,
			Employee.BirthID,
			Employee.NationalID,
			Employee.Religious,
			Employee.Nationalism, 
			Employee.ChildrenInfo,
			Employee.Village,
			Employee.PostOffice,
			Employee.Thana,
			Employee.District,
			Employee.PostCode,
			Employee.NameInBangla,
			Employee.OtherPhoneNo,
			Employee.PermVillage,
			Employee.PermPostOffice,
			Employee.PermThana,
			Employee.PermDistrict,
			EO.LocationID,			
			EO.LocationName, 
			EO.DepartmentID,
			EO.DepartmentName, 
			EO.DRPID,

			Dept.NameInBangla as DepartmentNameInBangla,
			L.NameInBangla as LocationNameInBangla,
			BU.NameInBangla as BusinessUnitNameInBangla,
			BU.Name as BusinessUnitName,

			BU.AddressInBangla AS BUAddressInBangla,
			BU.Address AS BUAddress,
			BU.Phone AS BUPhone,
			BU.FaxNo AS BUFaxNo,

			D.Name AS DesignationName,
			D.NameInBangla AS DesignationNameInBangla,
			D.DesignationID,
			EO.WorkingStatus,
			EO.RosterPlanID,
		    EO.RosterPlanDescription,
		    (SELECT HS.Name+'('+CONVERT(varchar(5),HS.StartTime,108)+'-'+CONVERT(varchar(5),HS.EndTime,108)+')' FROM HRM_Shift AS HS WHERE HS.ShiftID=EO.CurrentShiftID) as CurrentShift,			
			EO.EmployeeTypeID,
			EO.EmployeeTypeName,
			EO.DateOfJoin AS JoiningDate,
			EO.DateOfConfirmation AS  ConfirmationDate,
			EmployeeCard.EmployeeCardStatus
			,ISNULL((SELECT TOP(1)EmployeeCategory FROM EmployeeConfirmation WHERE EmployeeConfirmation.EmployeeID=Employee.EmployeeID ORDER BY ECID DESC),0) AS EmployeeCategory
			,(SELECT TOP(1)CardNo FROM  Employeeauthentication WHERE EmployeeID = Employee.EmployeeID AND IsActive=1) CardNo
			
FROM         Employee 
LEFT JOIN   VIEW_EmployeeOfficialALL EO ON Employee.EmployeeID = EO.EmployeeID 
--LEFT JOIN   View_AttendanceScheme VAS ON EO.AttendanceSchemeID = VAS.AttendanceSchemeID 
LEFT JOIN   Designation D ON EO.DesignationID = D.DesignationID
LEFT JOIN   Department Dept ON EO.departmentID = Dept.departmentID
LEFT JOIN   Location L ON EO.LocationID = L.LocationID
LEFT JOIN   BusinessUnit BU ON EO.BusinessUnitID = BU.BusinessUnitID
--LEFT JOIN  View_AttendanceScheme ON EO.AttendanceSchemeID = View_AttendanceScheme.AttendanceSchemeID
--LEFT JOIN  View_DepartmentRequirementPolicy VDR ON View_AttendanceScheme.DepartmentRequirementPolicyID = VDR.DepartmentRequirementPolicyID
LEFT  JOIN EmployeeCard ON  Employee.EmployeeID=EmployeeCard.EmployeeID





















GO
/****** Object:  View [dbo].[View_LeaveApplication]    Script Date: 11/12/2018 10:10:41 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO





CREATE VIEW [dbo].[View_LeaveApplication]
AS
SELECT     
		LeaveApplication.*,
        view_Employee.Name AS EmployeeName,
        view_Employee.Code AS EmployeeCode,
		VEmpLL.LeaveID AS LeaveHeadID,
        VEmpLL.LeaveName AS LeaveHeadName,
		view_Employee.BusinessUnitID,
		view_Employee.LocationID,
		view_Employee.LocationName,
		view_Employee.DepartmentID,
		view_Employee.DepartmentName AS DepartmentName,
		view_Employee.DesignationID,
		view_Employee.DesignationName AS DesignationName,
		view_Employee.JoiningDate,
		VEmpLL.IsActive,
        --View_User.EmployeeNameCode AS RequestedForRecommendation,
		(Select Name from Employee Where EmployeeID=LeaveApplication.RequestForRecommendation) AS RequestedForRecommendation,
        View_User2.EmployeeNameCode AS RecommendedByName,
        View_User3.EmployeeNameCode AS ApprovedByName,
		
        (SELECT DepartmentName FROM View_EmployeeOfficial WHERE View_EmployeeOfficial.EmployeeID = View_User3.EmployeeID) AS ApprovedByDepartment,
        (SELECT DesignationName FROM View_EmployeeOfficial WHERE View_EmployeeOfficial.EmployeeID = View_User3.EmployeeID) AS ApprovedByDesignation,

		(Select Name from Employee Where EmployeeID=LeaveApplication.RequestForAproval) AS RequestForAprovalName,
		(Select Name from Employee Where EmployeeID=LeaveApplication.ResponsiblePersonID) AS ResponsiblePersonName
		,HRApproval.EmployeeNameCode AS HRApproveBYName
		,ISNULL((SELECT top(1) UserID FROM Users WHERE EmployeeID=LeaveApplication.EmployeeID),0) AS Sender
		,ISNULL((SELECT top(1) UserID FROM Users WHERE EmployeeID=LeaveApplication.RequestForRecommendation),0) AS RecommandRcvr
		,ISNULL((SELECT top(1) UserID FROM Users WHERE EmployeeID=LeaveApplication.RequestForAproval),0) AS ApprovalRcvr
        ,DBUser.EmployeeNameCode AS DBUserName
		,(SELECT Name FROM BusinessUnit WHERE BusinessUnitID IN(SELECT BusinessUnitID FROM DepartmentRequirementPolicy
		 WHERE DepartmentRequirementPolicyID= view_Employee.DRPID)) AS BusinessUnitName
FROM    LeaveApplication 

LEFT JOIN view_Employee ON LeaveApplication.EmployeeID = view_Employee.EmployeeID
LEFT JOIN View_EmployeeLeaveLedger VEmpLL ON LeaveApplication.EmpLeaveLedgerID = VEmpLL.EmpLeaveLedgerID
LEFT JOIN View_User ON LeaveApplication.RequestForRecommendation = View_User.UserID
LEFT JOIN View_User AS View_User2 ON LeaveApplication.RecommendedBy = View_User2.UserID
LEFT JOIN View_User AS View_User3 ON LeaveApplication.ApproveBy = View_User3.UserID
LEFT JOIN View_User AS HRApproval ON LeaveApplication.HRApproveBy = HRApproval.UserID
LEFT JOIN View_User AS DBUser ON LeaveApplication.DBUserID = DBUser.UserID

















GO
