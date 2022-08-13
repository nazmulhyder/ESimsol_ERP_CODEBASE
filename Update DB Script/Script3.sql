USE [ESimSol_ERP]
GO

IF NOT EXISTS (SELECT * FROM sys.columns WHERE Name = N'IsDayOff' AND Object_ID = Object_ID(N'MaxOTConfigurationAttendance'))
BEGIN
	ALTER TABLE MaxOTConfigurationAttendance
	ADD IsDayOff bit
END
IF NOT EXISTS (SELECT * FROM sys.columns WHERE Name = N'IsHoliday' AND Object_ID = Object_ID(N'MaxOTConfigurationAttendance'))
BEGIN
	ALTER TABLE MaxOTConfigurationAttendance
	ADD IsHoliday bit
END
IF NOT EXISTS (SELECT * FROM sys.columns WHERE Name = N'IsLeave' AND Object_ID = Object_ID(N'MaxOTConfigurationAttendance'))
BEGIN
	ALTER TABLE MaxOTConfigurationAttendance
	ADD IsLeave bit
END
IF NOT EXISTS (SELECT * FROM sys.columns WHERE Name = N'LeaveHeadID' AND Object_ID = Object_ID(N'MaxOTConfigurationAttendance'))
BEGIN
	ALTER TABLE MaxOTConfigurationAttendance
	ADD LeaveHeadID int
END

/****** Object:  View [dbo].[View_MaxOTConfigurationAttendance]    Script Date: 11/5/2018 8:43:19 AM ******/
DROP VIEW [dbo].[View_MaxOTConfigurationAttendance]
GO
/****** Object:  View [dbo].[View_GrossSalaryCalculation]    Script Date: 11/5/2018 8:43:19 AM ******/
DROP VIEW [dbo].[View_GrossSalaryCalculation]
GO
/****** Object:  Table [dbo].[GrossSalaryCalculation]    Script Date: 11/5/2018 8:43:19 AM ******/
DROP TABLE [dbo].[GrossSalaryCalculation]
GO
/****** Object:  StoredProcedure [dbo].[SP_Upload_IncrementAsPerScheme]    Script Date: 11/5/2018 8:43:19 AM ******/
DROP PROCEDURE [dbo].[SP_Upload_IncrementAsPerScheme]
GO
/****** Object:  StoredProcedure [dbo].[SP_Upload_DisciplinaryAction]    Script Date: 11/5/2018 8:43:19 AM ******/
DROP PROCEDURE [dbo].[SP_Upload_DisciplinaryAction]
GO
/****** Object:  StoredProcedure [dbo].[SP_RPT_TimeCard_MaxOTCon]    Script Date: 11/5/2018 8:43:19 AM ******/
DROP PROCEDURE [dbo].[SP_RPT_TimeCard_MaxOTCon]
GO
/****** Object:  StoredProcedure [dbo].[SP_Rpt_SalarySummaryDetail_BUWise_Group]    Script Date: 11/5/2018 8:43:19 AM ******/
DROP PROCEDURE [dbo].[SP_Rpt_SalarySummaryDetail_BUWise_Group]
GO
/****** Object:  StoredProcedure [dbo].[SP_Rpt_IncrementByPercent]    Script Date: 11/5/2018 8:43:19 AM ******/
DROP PROCEDURE [dbo].[SP_Rpt_IncrementByPercent]
GO
/****** Object:  StoredProcedure [dbo].[SP_Process_UpdateComplianceAttendanceDaily]    Script Date: 11/5/2018 8:43:19 AM ******/
DROP PROCEDURE [dbo].[SP_Process_UpdateComplianceAttendanceDaily]
GO
/****** Object:  StoredProcedure [dbo].[SP_Process_TransferPromotionIncrement]    Script Date: 11/5/2018 8:43:19 AM ******/
DROP PROCEDURE [dbo].[SP_Process_TransferPromotionIncrement]
GO
/****** Object:  StoredProcedure [dbo].[SP_IUD_TimeCardMaxOTCon]    Script Date: 11/5/2018 8:43:19 AM ******/
DROP PROCEDURE [dbo].[SP_IUD_TimeCardMaxOTCon]
GO
/****** Object:  StoredProcedure [dbo].[SP_IUD_SalaryScheme]    Script Date: 11/5/2018 8:43:19 AM ******/
DROP PROCEDURE [dbo].[SP_IUD_SalaryScheme]
GO
/****** Object:  StoredProcedure [dbo].[SP_IUD_GrossSalaryCalculation]    Script Date: 11/5/2018 8:43:19 AM ******/
DROP PROCEDURE [dbo].[SP_IUD_GrossSalaryCalculation]
GO
/****** Object:  StoredProcedure [dbo].[SP_GetEmployeeBasicAmount]    Script Date: 11/5/2018 8:43:19 AM ******/
DROP PROCEDURE [dbo].[SP_GetEmployeeBasicAmount]
GO
/****** Object:  StoredProcedure [dbo].[SP_GetEmployeeBasicAmount]    Script Date: 11/5/2018 8:43:19 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[SP_GetEmployeeBasicAmount]
(
	@Param_EmployeeID INT,
	@Param_BasicOrGross BIT,
	@Param_BasicAmount DECIMAL(18, 2)	
)
AS
BEGIN
	--SET @Param_EmployeeID = 5
	--SET @Param_BasicOrGross = 1
	--SET @Param_BasicAmount  = 26000

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
	,@PV_sEquationBasic as nvarchar(500)
	
	,@PV_SQL as nvarchar(500)
	,@PV_SQLC as nvarchar(500)
	,@PV_FixedSalaryHeadIDs AS VARCHAR(MAX)
	,@PV_NonFixedSalaryHeadIDs AS VARCHAR(MAX)
	,@PV_CompGrossAmountStructure AS DECIMAL(18, 2)
	,@PV_SSDID INT
	,@PV_GrossAmount DECIMAL(18, 2)
	,@PV_BasicSalaryHeadID INT
	
	,@PV_IdDetail INT
	,@PV_IdCalc INT
		
	,@PV_IdDetailg INT
	,@PV_IdCalcg INT
		
	SET @PV_IdDetail = 1
	SET @PV_IdCalc = 1
	SET @PV_BasicSalaryHeadID = 0
	SET @PV_BasicSalaryHeadID = (SELECT SalaryHeadID FROM SalaryHead WHERE Name='Basic')
	  
	SET @PV_SalaryHeadID = (SELECT SalaryHeadID FROM SalaryHead WHERE Name='Basic')
	SET @PV_GrossAmount = (SELECT ActualGrossAmount FROM EmployeeSalaryStructure WHERE EmployeeID=@Param_EmployeeID)
	SET @PV_SalarySchemeID = (SELECT SalarySchemeID FROM EmployeeSalaryStructure WHERE EmployeeID=@Param_EmployeeID)	
	SET @PV_ESSID = (SELECT ESSID FROM EmployeeSalaryStructure WHERE EmployeeID=@Param_EmployeeID)
	
	
	DECLARE @tbl_SalarySchemeDetail AS TABLE(id INT IDENTITY(1, 1), SalarySchemeDetailID INT, SalaryHeadID INT)
	DECLARE @tbl_SalarySchemeDetailCalculation AS TABLE(idc INT IDENTITY(1, 1), ValueOperator INT ,CalculationOn INT,FixedValue DECIMAL(18, 2),SalaryHeadID INT,PercentVelue DECIMAL(18, 2),Operator INT)
	DECLARE @tbl_SalarySchemeGrossCalculation AS TABLE(idc INT IDENTITY(1, 1), ValueOperator INT ,CalculationOn INT,FixedValue DECIMAL(18, 2),SalaryHeadID INT,PercentVelue DECIMAL(18, 2),Operator INT)

	DECLARE @tbl_OutPut AS TABLE(SalaryHeadID INT, Amount DECIMAL(18, 2))
	
	IF @Param_BasicOrGross = 1
	BEGIN
		INSERT INTO @tbl_SalarySchemeGrossCalculation
		SELECT ValueOperator,CalculationOn,FixedValue,SalaryHeadID,PercentVelue,Operator FROM GrossSalaryCalculation WHERE SalarySchemeID =@PV_SalarySchemeID
	END
	IF @Param_BasicOrGross=0
	BEGIN
		INSERT INTO @tbl_SalarySchemeDetail
		SELECT SalarySchemeDetailID,SalaryHeadID FROM SalarySchemeDetail WHERE SalarySchemeID=@PV_SalarySchemeID AND SalaryHeadID =@PV_SalaryHeadID AND SalaryHeadID IN(SELECT SalaryHeadID FROM SalaryHead WHERE SalaryHeadType=1) Order BY SalaryHeadID ASC
	END ELSE BEGIN
		INSERT INTO @tbl_SalarySchemeDetail
		SELECT SalarySchemeDetailID,SalaryHeadID FROM SalarySchemeDetail WHERE SalarySchemeID=@PV_SalarySchemeID AND SalaryHeadID IN(SELECT SalaryHeadID FROM SalaryHead WHERE SalaryHeadType=1) Order BY SalaryHeadID ASC
	END
	IF @Param_BasicOrGross = 0
	BEGIN
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
				
			--SET @Param_BasicAmount = @PV_sValue
			INSERT INTO @tbl_OutPut VALUES(@PV_SalaryHeadID, @PV_sValue)

			SET @id = @id + 1
		END
	END 
	ELSE 
	BEGIN
		
		SET @idc = 0
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

		SET @PV_IdCalcg = (SELECT COUNT(*) FROM @tbl_SalarySchemeGrossCalculation)
		WHILE(@idc <= @PV_IdCalcg)
		BEGIN--	
			
			SELECT @PV_ValueOperator = ValueOperator,@PV_CalculationOn=CalculationOn,@PV_FixedValue=FixedValue,@PV_CalSalaryHeadID=SalaryHeadID,@PV_PercentVelue=PercentVelue,@PV_Operator=Operator FROM @tbl_SalarySchemeGrossCalculation WHERE idc=@idc
			
			IF (@PV_ValueOperator=1)--Value
			BEGIN
				IF (@PV_CalSalaryHeadID = @PV_BasicSalaryHeadID) 
				BEGIN
					SET @PV_sEquation=@PV_sEquation+CONVERT(varchar(100),@Param_BasicAmount)
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
				
		--SET @Param_BasicAmount = @PV_sValue
		INSERT INTO @tbl_OutPut VALUES(@PV_SalaryHeadID, @PV_sValue)
	END
	
	SELECT ISNULL(SUM(Amount), 0) FROM @tbl_OutPut
	
END
GO
/****** Object:  StoredProcedure [dbo].[SP_IUD_GrossSalaryCalculation]    Script Date: 11/5/2018 8:43:20 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[SP_IUD_GrossSalaryCalculation]
(



	    @Param_GSCID AS int,
		@Param_SalarySchemeID AS INT,
		@Param_ValueOperator AS SMALLINT,
		@Param_CalculationOn AS INT,
		@Param_FixedValue DECIMAL(30,17),
		@Param_Operator AS SMALLINT,
		@Param_SalaryHeadID AS INT,
		@Param_PercentVelue AS DECIMAL(18,2),
		@Param_DBUserID AS int,
		@Param_DBOperation AS smallint
		
	    --%n,%n,%n,%n,%n,%n,%n,%n,%n,%n
  

)

AS
BEGIN TRAN	
DECLARE
	@PV_DBServerDateTime as datetime
	SET @PV_DBServerDateTime=Getdate()

IF(@Param_DBOperation=1)
BEGIN
	
	IF(@Param_CalculationOn=3 AND @Param_Operator !=0)
	BEGIN
		ROLLBACK
			RAISERROR (N'Your are not allowed to select an operator for the fixed value !!',16,1);	
		RETURN
	END	
	
	SET @Param_GSCID=(SELECT ISNULL(MAX(GSCID),0)+1 FROM GrossSalaryCalculation)
	
	INSERT INTO  GrossSalaryCalculation(GSCID,        SalarySchemeID ,       ValueOperator ,       CalculationOn ,       FixedValue ,       Operator ,       SalaryHeadID ,       PercentVelue,  DBUserID ,       DBServerDateTime)
                                   VALUES       (@Param_GSCID ,@Param_SalarySchemeID ,@Param_ValueOperator ,@Param_CalculationOn ,@Param_FixedValue ,@Param_Operator ,@Param_SalaryHeadID , @Param_PercentVelue, @Param_DBUserID ,@PV_DBServerDateTime)

	SELECT * FROM View_GrossSalaryCalculation WHERE GSCID= @Param_GSCID
END

IF(@Param_DBOperation=2)
BEGIN
	IF(@Param_GSCID<=0)
	BEGIN
		ROLLBACK
			RAISERROR (N'Your Selected SalarySchemeDetailCalculation Is Invalid Please Refresh and try again!!',16,1);	
		RETURN
	END	
   
   IF(@Param_CalculationOn=3 AND @Param_Operator !=0)
	BEGIN
		ROLLBACK
			RAISERROR (N'Your are not allowed to select an operator for the fixed value !!',16,1);	
		RETURN
	END	
   
	UPDATE GrossSalaryCalculation SET ValueOperator = @Param_ValueOperator
                                           , CalculationOn = @Param_CalculationOn
										   , FixedValue = @Param_FixedValue
                                           , Operator = @Param_Operator
                                           , PercentVelue=@Param_PercentVelue
      
	WHERE GSCID= @Param_GSCID
	
	SELECT * FROM View_GrossSalaryCalculation WHERE GSCID= @Param_GSCID
END

IF(@Param_DBOperation=3)
BEGIN
	IF(@Param_GSCID<=0)
	BEGIN
		ROLLBACK
			RAISERROR (N'Your Selected Employee Is Invalid Please Refresh and try again!!',16,1);	
		RETURN
	END	
			
	DELETE FROM GrossSalaryCalculation WHERE GSCID= @Param_GSCID
END

COMMIT TRAN




GO
/****** Object:  StoredProcedure [dbo].[SP_IUD_SalaryScheme]    Script Date: 11/5/2018 8:43:20 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		MD. Azharul Islam
-- Create date: 27 Jan 2014
-- Note:	Insert Update Delete SalaryScheme
-- =============================================
-- =============================================
-- Modified By: Mohammad Shahjada Sagor
-- Modify date: 07 Dec 2015
-- Note:	Fixed Value & Gratuity Calulation
-- =============================================
CREATE PROCEDURE [dbo].[SP_IUD_SalaryScheme]
(
	@Param_SalarySchemeID AS int,
    @Param_Name AS VARCHAR(50),
	@Param_NatureOfEmployee AS smallint,
	@Param_PaymentCycle AS smallint,
	@Param_Description AS varchar(200),
	@Param_IsAllowBankAccount AS bit,
	@Param_IsAllowOverTime AS bit,
	@Param_OverTimeON AS varchar(20),
	@Param_DividedBy AS DECIMAL(18,2),
	@Param_MultiplicationBy AS DECIMAL(18,2),
	@Param_FixedOTRatePerHour as decimal(30,17),
	@Param_IsAttendanceDependant AS bit,
	@Param_LateCount AS int,
	@Param_EarlyLeavingCount AS int,
	@Param_FixedLatePenalty as decimal(30,17),
	@Param_FixedEarlyLeavePenalty as decimal(30,17),
	@Param_IsProductionBase AS bit,
	@Param_StartDay as int, 
	@Param_IsGratuity AS bit,	
	@Param_GraturityMaturedInYear as int, 
	@Param_NoOfMonthCountOneYear as int, 
	@Param_GratuityApplyOn as smallint, 
	@Param_DBUserID AS int,
	@Param_DBOperation AS smallint,
	
	@Param_CompOverTimeON AS smallint,
	@Param_CompDividedBy AS int,
	@Param_CompMultiplicationBy AS int,
	@Param_CompFixedOTRatePerHour AS DECIMAL(30,17)
			
	--%n,%s,%n,%n,%s,%b,%b,%s,%n,%n,%n,%b,%n,%n,%n,%n,%b,%n,%b,%n,%n,%n,%n,%n   ,%n,%n,%n,%n
)

AS
BEGIN TRAN	
DECLARE
	@PV_DBServerDateTime as datetime
	SET @PV_DBServerDateTime=Getdate()

IF(@Param_DBOperation=1)
BEGIN
	IF  EXISTS (SELECT Name FROM SalaryScheme WHERE Name = @Param_Name )
	BEGIN
		ROLLBACK
			RAISERROR (N'Duplicate SalaryScheme!!',16,1);	
		RETURN
	END
	-----
	--IF(@Param_IsAllowOverTime=1 & @Param_DividedBy=0 & @Param_MultiplicationBy=0)
	--BEGIN
	--	ROLLBACK
	--		RAISERROR (N'Enter Value In All Of The Fields Of Overtime!!',16,1);	
	--	RETURN
	--END
	-----
	IF(@Param_IsAllowOverTime=0)
	BEGIN
		SET @Param_OverTimeON =0
		SET @Param_DividedBy = 0
		SET @Param_MultiplicationBy = 0
		SET @Param_FixedOTRatePerHour = 0
		SET @Param_CompOverTimeON =0
		SET @Param_CompDividedBy =0
		SET @Param_CompMultiplicationBy =0
		SET @Param_CompFixedOTRatePerHour =0
	END
	ELSE
	BEGIN
		IF(@Param_CompOverTimeON<=0 AND @Param_CompFixedOTRatePerHour<=0)
		BEGIN
			SET @Param_CompOverTimeON =@Param_OverTimeON 
			SET @Param_CompDividedBy =@Param_DividedBy 
			SET @Param_CompMultiplicationBy =@Param_MultiplicationBy 
			SET @Param_CompFixedOTRatePerHour =@Param_FixedOTRatePerHour 
		END
	END


	SET @Param_SalarySchemeID=(SELECT ISNULL(MAX(SalarySchemeID),0)+1 FROM SalaryScheme)
	
	INSERT INTO SalaryScheme(SalarySchemeID,       Name,            NatureOfEmployee,        PaymentCycle,       [Description],      IsAllowBankAccount ,       IsAllowOverTime,         OverTimeON ,        DividedBy         ,MultiplicationBy,       FixedOTRatePerHour,        IsActive,        IsAttendanceDependant,         LateCount,         EarlyLeavingCount,		FixedLatePenalty,	    	FixedEarlyLeavePenalty,     IsProductionBase,			StartDay,	    IsGratuity,        GraturityMaturedInYear,       NoOfMonthCountOneYear,         GratuityApplyOn,  	    DBUserID ,       DBServerDateTime  ,CompOverTimeON       ,CompDividedBy        ,CompMultiplicationBy       ,CompFixedOTRatePerHour)
				      VALUES(@Param_SalarySchemeID,@Param_Name,  @Param_NatureOfEmployee,  @Param_PaymentCycle,@Param_Description, @Param_IsAllowBankAccount, @Param_IsAllowOverTime, @Param_OverTimeON,  @Param_DividedBy, @Param_MultiplicationBy,  @Param_FixedOTRatePerHour,     0,        @Param_IsAttendanceDependant, @Param_LateCount,  @Param_EarlyLeavingCount,   @Param_FixedLatePenalty,	@Param_FixedEarlyLeavePenalty, @Param_IsProductionBase,	@Param_StartDay, @Param_IsGratuity, @Param_GraturityMaturedInYear, @Param_NoOfMonthCountOneYear, @Param_GratuityApplyOn, @Param_DBUserID, @PV_DBServerDateTime ,@Param_CompOverTimeON,@Param_CompDividedBy ,@Param_CompMultiplicationBy,@Param_CompFixedOTRatePerHour)    					    				  
	SELECT * FROM SalaryScheme WHERE SalarySchemeID= @Param_SalarySchemeID
END

IF(@Param_DBOperation=2)
BEGIN
	IF(@Param_SalarySchemeID<=0)
	BEGIN
		ROLLBACK
			RAISERROR (N'Your Selected SalaryScheme Is Invalid Please Refresh and try again!!',16,1);	
		RETURN
	END	
	
    IF  EXISTS (SELECT Name FROM SalaryScheme WHERE Name = @Param_Name AND SalarySchemeID != @Param_SalarySchemeID)
	BEGIN
		ROLLBACK
			RAISERROR (N'SalarySchemeName can not be Updated. Already exists this Name!!',16,1);	
		RETURN
	END
	
	IF  EXISTS (SELECT * FROM SalaryScheme WHERE IsActive = 1 AND SalarySchemeID = @Param_SalarySchemeID)
	BEGIN
		ROLLBACK
			RAISERROR (N'Active SalarySchemeName can not be Updated.!!',16,1);	
		RETURN
	END
	IF  EXISTS (SELECT * FROM EmployeeSalaryStructure WHERE SalarySchemeID = @Param_SalarySchemeID AND IsActive=1)
	BEGIN
		ROLLBACK
			RAISERROR (N'This Scheme already assigned to employee. You can not edit.!!',16,1);	
		RETURN
	END	

	IF(@Param_IsAllowOverTime=0)
	BEGIN
		SET @Param_OverTimeON =0
		SET @Param_DividedBy = 0
		SET @Param_MultiplicationBy = 0
		SET @Param_FixedOTRatePerHour = 0
		SET @Param_CompOverTimeON =0
		SET @Param_CompDividedBy =0
		SET @Param_CompMultiplicationBy =0
		SET @Param_CompFixedOTRatePerHour =0
	END
	ELSE
	BEGIN
		IF(@Param_CompOverTimeON<=0 AND @Param_CompFixedOTRatePerHour<=0)
		BEGIN
			SET @Param_CompOverTimeON =@Param_OverTimeON 
			SET @Param_CompDividedBy =@Param_DividedBy 
			SET @Param_CompMultiplicationBy =@Param_MultiplicationBy 
			SET @Param_CompFixedOTRatePerHour =@Param_FixedOTRatePerHour 
		END
	END

	UPDATE SalaryScheme  SET	Name=@Param_Name
	                          , [Description]=@Param_Description
							  , PaymentCycle = @Param_PaymentCycle
							  , IsAllowBankAccount=@Param_IsAllowBankAccount
							  , IsAllowOverTime=@Param_IsAllowOverTime
							  , OverTimeON=@Param_OverTimeON
							  , DividedBy=@Param_DividedBy
							  , FixedOTRatePerHour=@Param_FixedOTRatePerHour
							  , IsAttendanceDependant=@Param_IsAttendanceDependant
							  , LateCount=@Param_LateCount
							  , EarlyLeavingCount=@Param_EarlyLeavingCount
							  , FixedLatePenalty=@Param_FixedLatePenalty
							  , FixedEarlyLeavePenalty=@Param_FixedEarlyLeavePenalty
							  , IsProductionBase=@Param_IsProductionBase
							  , MultiplicationBy=@Param_MultiplicationBy
							  , StartDay=@Param_StartDay
							  , IsGratuity=@Param_IsGratuity
							  , GraturityMaturedInYear=@Param_GraturityMaturedInYear
							  , NoOfMonthCountOneYear=@Param_NoOfMonthCountOneYear
							  , GratuityApplyOn=@Param_GratuityApplyOn	
							  ,	CompOverTimeON =@Param_CompOverTimeON 
							  , CompDividedBy =@Param_CompDividedBy 
							  , CompMultiplicationBy =@Param_CompMultiplicationBy 
							  , CompFixedOTRatePerHour =@Param_CompFixedOTRatePerHour 
							  					  							  
	WHERE SalarySchemeID= @Param_SalarySchemeID
	SELECT * FROM SalaryScheme WHERE SalarySchemeID= @Param_SalarySchemeID
END

IF(@Param_DBOperation=3)
BEGIN
	IF(@Param_SalarySchemeID<=0)
	BEGIN
		ROLLBACK
			RAISERROR (N'Your Selected Scheme Is Invalid Please Refresh and try again!!',16,1);	
		RETURN
	END
	IF  EXISTS (SELECT * FROM EmployeeSalaryStructure WHERE SalarySchemeID = @Param_SalarySchemeID)
	BEGIN
		ROLLBACK
			RAISERROR (N'This Scheme already assigned to employee. You can not delete.!!',16,1);	
		RETURN
	END			
	IF  EXISTS (SELECT * FROM SalaryScheme WHERE IsActive = 1 AND SalarySchemeID = @Param_SalarySchemeID)
	BEGIN
		ROLLBACK
			RAISERROR (N'Active SalarySchemeName can not be Deleted.!!',16,1);	
		RETURN
	END
	
    DELETE FROM SalarySchemeDetailCalculation WHERE SalarySchemeDetailID IN (SELECT SalarySchemeDetailID FROM  SalarySchemeDetail WHERE SalarySchemeID = @Param_SalarySchemeID) 
	DELETE FROM SalarySchemeDetail WHERE SalarySchemeID= @Param_SalarySchemeID		
	DELETE FROM ProductionBonus WHERE SalarySchemeID= @Param_SalarySchemeID
	DELETE FROM SalaryScheme WHERE SalarySchemeID= @Param_SalarySchemeID

	
END

COMMIT TRAN




GO
/****** Object:  StoredProcedure [dbo].[SP_IUD_TimeCardMaxOTCon]    Script Date: 11/5/2018 8:43:20 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[SP_IUD_TimeCardMaxOTCon] 
	@Param_EmployeeID INT
	,@Param_AttendanceDate DATE
	,@Param_MOCID INT
	,@Param_InTime DATETIME
	,@Param_OutTime DATETIME
	,@Param_IsAbsent as bit
	,@Param_IsDayOff BIT
	,@Param_LeaveHeadID INT
	,@Param_IsManualOT as bit
	,@Param_OverTimeInMinute as int	
	,@Param_UserID INT
AS
BEGIN

	DECLARE
	@PV_EmployeeID as int 
	,@PV_StartTime as DATETIME
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
	,@PV_AttendanceID INT
	,@PV_MOCAID INT

	IF @Param_IsDayOff=1 AND @Param_LeaveHeadID>0
	BEGIN
		ROLLBACk
			RAISERROR(N'Invalid Operation.!!',16,1);
		RETURN	
	END
	
	SET @PV_MOCAID = (SELECT MOCAID FROM MaxOTConfigurationAttendance WHERE AttendanceDate = @Param_AttendanceDate AND EmployeeID = @Param_EmployeeID AND MOCID = @Param_MOCID)
	SET @PV_AttendanceID = (SELECT AttendanceID FROM AttendanceDaily WHERE AttendanceDate=@Param_AttendanceDate AND EmployeeID = @Param_EmployeeID)

	SET @PV_ShiftID = (SELECT ShiftID FROM AttendanceDaily WHERE AttendanceID=@PV_AttendanceID)

	SELECT @PV_StartTime=(DATEADD(SECOND,0,DATEADD (MINUTE,DATEPART(MINUTE,StartTime), DATEADD(HOUR, DATEPART(HOUR,StartTime), DATEADD(dd, DATEDIFF(dd, -0, @Param_AttendanceDate), 0)))) )--CAST (StartTime AS TIME(0))
	  ,@PV_EndTime=(DATEADD(SECOND,0,DATEADD (MINUTE,DATEPART(MINUTE,EndTime), DATEADD(HOUR, DATEPART(HOUR,EndTime), DATEADD(dd, DATEDIFF(dd, -0, @Param_AttendanceDate), 0)))) )--CAST (EndTime AS TIME(0)) 	   
	  ,@PV_DayStartTime=(DATEADD(SECOND,0,DATEADD (MINUTE,DATEPART(MINUTE,DayStartTime), DATEADD(HOUR, DATEPART(HOUR,DayStartTime), DATEADD(dd, DATEDIFF(dd, -0, @Param_AttendanceDate), 0)))) )
	  ,@PV_DayEndTime=(DATEADD(SECOND,0,DATEADD (MINUTE,DATEPART(MINUTE,DayEndTime), DATEADD(HOUR, DATEPART(HOUR,DayEndTime), DATEADD(dd, DATEDIFF(dd, -0, @Param_AttendanceDate), 0)))) )
	  ,@PV_TolaranceTime=(DATEADD(SECOND,0,DATEADD (MINUTE,DATEPART(MINUTE,ToleranceTime), DATEADD(HOUR, DATEPART(HOUR,ToleranceTime), DATEADD(dd, DATEDIFF(dd, -0, @Param_AttendanceDate), 0)))) )
	  ,@PV_Shift_IsOT=IsOT
	  ,@PV_Shift_OTStart=(DATEADD(SECOND,0,DATEADD (MINUTE,DATEPART(MINUTE,OTStartTime), DATEADD(HOUR, DATEPART(HOUR,OTStartTime), DATEADD(dd, DATEDIFF(dd, -0, @Param_AttendanceDate), 0)))) )
	  ,@PV_Shift_OTEnd=(DATEADD(SECOND,0,DATEADD (MINUTE,DATEPART(MINUTE,OTEndTime), DATEADD(HOUR, DATEPART(HOUR,OTEndTime), DATEADD(dd, DATEDIFF(dd, -0, @Param_AttendanceDate), 0)))) )
	,@PV_Shift_ISOTOnActual=ISNULL(IsOTOnActual,0)
	,@PV_Shift_OTCalclateAfterInMin=ISNULL(OTCalculateAfterInMinute,0)
	,@PV_Shift_IsLeaveOnOFFHoliday=IsLeaveOnOFFHoliday
	FROM HRM_Shift WHERE ShiftID=@PV_ShiftID
	SET @Param_InTime=(DATEADD(SECOND,0,DATEADD (MINUTE,DATEPART(MINUTE,@Param_InTime), DATEADD(HOUR, DATEPART(HOUR,@Param_InTime), DATEADD(dd, DATEDIFF(dd, -0, @Param_AttendanceDate), 0)))) )
	SET @Param_OutTime=(DATEADD(SECOND,0,DATEADD (MINUTE,DATEPART(MINUTE,@Param_OutTime), DATEADD(HOUR, DATEPART(HOUR,@Param_OutTime), DATEADD(dd, DATEDIFF(dd, -0, @Param_AttendanceDate), 0)))) )


	--Find Shift Out time for night shift
	IF (DATEPART(HOUR,@PV_StartTime)>DATEPART(HOUR,@PV_EndTime))
	BEGIN--Night Shift or different date			
		SET @PV_EndTime=(DATEADD(SECOND,0,DATEADD (MINUTE,DATEPART(MINUTE,@PV_EndTime), DATEADD(HOUR, DATEPART(HOUR,@PV_EndTime), DATEADD(dd, DATEDIFF(dd, -1, @Param_AttendanceDate), 0)))) )
	END	ELSE BEGIN	
		SET @PV_EndTime=(DATEADD(SECOND,0,DATEADD (MINUTE,DATEPART(MINUTE,@PV_EndTime), DATEADD(HOUR, DATEPART(HOUR,@PV_EndTime), DATEADD(dd, DATEDIFF(dd, -0, @Param_AttendanceDate), 0)))) )
	END
	
	--Attendance Record for night shift
	IF (DATEPART(HOUR,@Param_InTime)>DATEPART(HOUR,@Param_OutTime) AND DATEPART(HOUR,@Param_OutTime)>0)
	BEGIN--Night Shift or different date			
		SET @Param_OutTime=(DATEADD(SECOND,0,DATEADD (MINUTE,DATEPART(MINUTE,@Param_OutTime), DATEADD(HOUR, DATEPART(HOUR,@Param_OutTime), DATEADD(dd, DATEDIFF(dd, -1, @Param_AttendanceDate), 0)))) )
	END	ELSE BEGIN	
		SET @Param_OutTime=(DATEADD(SECOND,0,DATEADD (MINUTE,DATEPART(MINUTE,@Param_OutTime), DATEADD(HOUR, DATEPART(HOUR,@Param_OutTime), DATEADD(dd, DATEDIFF(dd, -0, @Param_AttendanceDate), 0)))) )
	END


	IF @Param_IsAbsent=0
	BEGIN
		--in case of start & end time same
		IF  CONVERT (DATE,@Param_InTime)=CONVERT (DATE,@Param_OutTime)--Check Date
			AND DATEPART(HOUR,@Param_InTime)=DATEPART(HOUR,@Param_OutTime) --Hour check
			AND DATEPART(MINUTE,@Param_InTime)=DATEPART(MINUTE,@Param_OutTime)--Min Check
		BEGIN
			SET @Param_OutTime=(SELECT DATEADD(SECOND,0,DATEADD (MINUTE,0, DATEADD(HOUR, 0, DATEADD(dd, DATEDIFF(dd, -0, @PV_DayEndTime), 0)))))
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

	END ELSE BEGIN	
		UPDATE MaxOTConfigurationAttendance SET IsDayOff=0,IsHoliday=0 WHERE MOCAID= @PV_MOCAID
		SET @Param_OverTimeInMinute=0
	END

	SET @Param_OverTimeInMinute=CASE WHEN @Param_OverTimeInMinute<0 THEN @Param_OverTimeInMinute END

	IF @Param_LeaveHeadID > 0
	BEGIN
	UPDATE MaxOTConfigurationAttendance
	SET
		InTime=dateadd(DAY, DATEDIFF(DAY, 0,@Param_InTime), 0)
		,OutTime=dateadd(DAY, DATEDIFF(DAY, 0,@Param_OutTime), 0)
		,OverTimeInMin=0
		,IsDayOff = 0
		,IsLeave = 1
		,LeaveHeadID = @Param_LeaveHeadID
		WHERE MOCAID= @PV_MOCAID
	END ELSE 
	BEGIN
	UPDATE MaxOTConfigurationAttendance
	SET
		InTime=@Param_InTime
		,OutTime=@Param_OutTime
		,OverTimeInMin=@Param_OverTimeInMinute
		,IsDayOff = @Param_IsDayOff
		,IsLeave = 0
		,LeaveHeadID = 0
		WHERE MOCAID= @PV_MOCAID
	END

	SELECT * FROM View_MaxOTConfigurationAttendance WHERE MOCAID= @PV_MOCAID
END


GO
/****** Object:  StoredProcedure [dbo].[SP_Process_TransferPromotionIncrement]    Script Date: 11/5/2018 8:43:20 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		MD. Azharul Islam
-- Create date: 22 Apr 2014
-- Note:	   Effect TransferPromotionIncrement
-- =============================================
CREATE PROCEDURE [dbo].[SP_Process_TransferPromotionIncrement]
(

		@Param_TPIID AS int,
		@Param_ActualEffectedDate AS date,
		@Param_DBUserID AS int
	    --%n,%d,%n
)
AS
--DECLARE
--	@Param_TPIID AS int,
--	@Param_ActualEffectedDate AS date,
--	@Param_DBUserID AS int
	
--	SET @Param_TPIID =2
--	SET @Param_ActualEffectedDate=GETDATE()
--	SET @Param_DBUserID =5
	
BEGIN TRAN	
IF NOT EXISTS(SELECT*FROM TransferPromotionIncrement WHERE TPIID=@Param_TPIID AND ApproveBy>0)
BEGIN
	ROLLBACK
			RAISERROR (N'This Item Is Not Approved ! Effect Is Not Possible!!',16,1);	
	RETURN
END

DECLARE
	--SALARYSCHEME DETAIL
	@PV_IsFixedBasic AS BIT,
	@PV_BasicValue AS FLOAT, 
	@PV_AllowanceValue AS FLOAT,
	@PV_SalaryHeadID AS INT,
	@PV_SalaryHeadType AS SMALLINT,
	@PV_Amount AS INT,
	@PV_AllowanceCalculationOn AS SMALLINT,
	@PV_CalculationOnSalaryHeadID AS INT,
	-- EmployeeOfficialHistory VARIABLES
	@PV_EmployeeOfficialHistoryID as int,
	@PV_EmployeeOfficialID as int,
	@PV_CurrentShiftID as int,
	@PV_WorkingStatus as smallint,
	@PV_StartDate as date,
	@PV_EndDate as date,
	
	-- EmployeeSalaryStructureHistory VARIABLES
	@PV_ESSHistoryID as int,
	@PV_ESSID as int,
	@PV_IsIncludeFixedItem as bit,
	@PV_ActualGrossAmount as float,
	@PV_CurrencyID as int,
	@PV_IsActive as bit,

	--EmployeeSalaryStructureDetailHistory VARIABLES
	@PV_ESSSHistoryID as int,
	@PV_dESSHistoryID as int,
	@PV_ESSSID as int,

	
	--TPI VARIABLES AND GETS
	@PV_DBServerDateTime as datetime,
	@PV_EmployeeID as int,
	@PV_IsTransfer as bit,
	@PV_IsPromotion as bit,
	@PV_IsIncrement as bit,
	@PV_ASID as int,
	@PV_DRPID as int,
	@PV_ShiftID as int,
	@PV_DesignationID as int,
	@PV_TPIDesignationID as int,
	@PV_TPIAttendanceSchemeID as int,
	@PV_TPIDRPID as int,
	@PV_GrossAmount as float,
	@PV_CompGrossAmount as float,
	@PV_CompTPIGrossAmount as float,
	@PV_SalarySchemeID as int,
	@PV_TPISalarySchemeID as int,
	@PV_TPIGrossAmount as int,
	@PV_TPIIsFixedAmount bit,
	@PV_TPIEmployeeTypeID INT,
	@PV_IsCashFixed BIT,
	@PV_CashAmount DECIMAL(18, 2)
	
	
	SELECT @PV_EmployeeID=EmployeeID,
	       @PV_IsTransfer=IsTransfer,
	       @PV_IsPromotion=IsPromotion,
	       @PV_IsIncrement=IsIncrement,
	       @PV_ASID=ASID,
	       @PV_DRPID=DRPID,
	       @PV_DesignationID =DesignationID,
		   @PV_ShiftID = TPIShiftID,
	       @PV_TPIDesignationID=TPIDesignationID,
	       @PV_TPIDRPID=TPIDRPID,
	       @PV_TPIAttendanceSchemeID=TPIASID,
	       @PV_GrossAmount=GrossSalary,
	       @PV_CompGrossAmount=CompGrossSalary,
	       @PV_CompTPIGrossAmount=CompTPIGrossSalary,
	       @PV_SalarySchemeID=SalarySchemeID,
	       @PV_TPIGrossAmount=TPIGrossSalary,
	       @PV_TPIIsFixedAmount=TPIIsFixedAmount,
	       @PV_TPISalarySchemeID=TPISalarySchemeID,
		   @PV_TPIEmployeeTypeID = TPIEmployeeTypeID,
		   @PV_IsCashFixed = ISNULL(IsCashFixed, 0),
		   @PV_CashAmount = ISNULL(CashAmount, 0)
	FROM   TransferPromotionIncrement WHERE TPIID=@Param_TPIID
	
	SET    @PV_DBServerDateTime=Getdate()
	
--CHECK CONSTRAINTS

IF (@PV_IsTransfer=0 AND @PV_IsPromotion=0 AND @PV_IsIncrement=0) 
BEGIN
	ROLLBACK
			RAISERROR (N'Transfer,Promotion,Increment: No One Is Selected For This Employee. Effect Is Not Possible For This Employee!!',16,1);	
	RETURN
END

--2
IF EXISTS(SELECT  * FROM EmployeeOfficial WHERE EmployeeID =@PV_EmployeeID AND IsActive=0)
BEGIN
	ROLLBACK
			RAISERROR (N'This Employee Is Not In Workplace ! Effect Is Not Possible For This Employee!!',16,1);	
	RETURN
END
IF EXISTS(SELECT  * FROM TransferPromotionIncrement WHERE TPIID=@Param_TPIID AND ActualEffectedDate IS NOT NULL)
BEGIN
	ROLLBACK
			RAISERROR (N'Already effected.!!',16,1);	
	RETURN
END
--update effect date
UPDATE TransferPromotionIncrement SET ActualEffectedDate=@Param_ActualEffectedDate WHERE TPIID=@Param_TPIID
--3
--IF (@PV_IsTransfer=1)
--BEGIN
--	IF NOT EXISTS ( SELECT  * FROM AttendanceDaily WHERE EmployeeID =@PV_EmployeeID AND IsLock=1 AND AttendanceDate<@Param_ActualEffectedDate)
--	BEGIN
--		ROLLBACK
--			RAISERROR (N'Attendance Of This Employee Is Not  Locked ! Transfer Is Not Possible For This Employee!!',16,1);	
--		RETURN
--	END
--END

----4
--IF (@PV_IsIncrement=1)
--BEGIN
--	IF NOT EXISTS (SELECT  * FROM EmployeeSalary WHERE EmployeeID =@PV_EmployeeID AND IsLock=1 AND SalaryReceiveDate<@Param_ActualEffectedDate )
--	BEGIN
--		ROLLBACK
--				RAISERROR (N'Salary Of This Employee Is Not Processed ! Increment Is Not Possible For This Employee!!',16,1);	
--		RETURN
--	END
--END

--5
--IF NOT EXISTS (SELECT  * FROM AttendanceDaily WHERE EmployeeID =@PV_EmployeeID AND AttendanceDate>@Param_ActualEffectedDate)  
--BEGIN
--	ROLLBACK
--			RAISERROR (N' Attendance Locked Date Is Greater Than Actual Effected Date! Transfer And Promotion Is Not Possible For This Employee!!',16,1);	
--	RETURN
--END
  
--IF NOT EXISTS (SELECT  * FROM EmployeeSalary WHERE EmployeeID =@PV_EmployeeID AND SalaryReceiveDate>@Param_ActualEffectedDate)  
--BEGIN
--	ROLLBACK
--			RAISERROR (N'Salary Process Date Is Greater Than Actual Effected Date! Transfer And Promotion Is Not Possible For This Employee!!',16,1);	
--	RETURN
--END


--ISTRANSFER / ISPROMOTION
IF (@PV_IsTransfer=1 OR @PV_IsPromotion=1)
BEGIN
	IF EXISTS(SELECT  TOP (1)* FROM EmployeeOfficialHistory WHERE EmployeeOfficialID IN (SELECT EmployeeOfficialID FROM EmployeeOfficial WHERE EmployeeID=@PV_EmployeeID) ORDER BY EmployeeOfficialHistoryID DESC)
	BEGIN
		SET @PV_StartDate=(SELECT TOP (1)StartDate FROM EmployeeOfficialHistory WHERE EmployeeOfficialID IN (SELECT EmployeeOfficialID FROM EmployeeOfficial WHERE EmployeeID=@PV_EmployeeID) ORDER BY EmployeeOfficialHistoryID DESC)
	END
	
	ELSE
	BEGIN
		SET @PV_StartDate=(SELECT TOP (1)DateOfJoin FROM EmployeeOfficial WHERE EmployeeID = @PV_EmployeeID ORDER BY EmployeeOfficialID DESC)
	END
	
	SET @PV_EndDate= DATEADD(DAY,-1,@Param_ActualEffectedDate)
	SET @PV_EmployeeOfficialHistoryID=(SELECT ISNULL(MAX(EmployeeOfficialHistoryID),0)+1 FROM EmployeeOfficialHistory)
	SET @PV_EmployeeOfficialID=(SELECT EmployeeOfficialID FROM EmployeeOfficial WHERE EmployeeID=@PV_EmployeeID)
	SET @PV_CurrentShiftID=(SELECT CurrentShiftID FROM EmployeeOfficial WHERE EmployeeID=@PV_EmployeeID)
	SET @PV_WorkingStatus=(SELECT WorkingStatus FROM EmployeeOfficial WHERE EmployeeID=@PV_EmployeeID)
	
	INSERT INTO EmployeeOfficialHistory(EmployeeOfficialHistoryID,     EmployeeOfficialID,    AttendanceSchemeID,DRPID,     DesignationID,    CurrentShiftID,    WorkingStatus,    StartDate,    EndDate,    DBUserID,       DBServerDateTime)
								 VALUES(@PV_EmployeeOfficialHistoryID, @PV_EmployeeOfficialID,@PV_ASID,          @PV_DRPID, @PV_DesignationID,@PV_CurrentShiftID,@PV_WorkingStatus,@PV_StartDate,@PV_EndDate,@Param_DBUserID,@PV_DBServerDateTime )
	IF (@PV_IsTransfer=1)
	BEGIN
		UPDATE EmployeeOfficial SET DRPID= @PV_TPIDRPID ,AttendanceSchemeID=@PV_TPIAttendanceSchemeID, CurrentShiftID = @PV_ShiftID WHERE EmployeeID=@PV_EmployeeID  
    END    
    IF (@PV_IsPromotion=1)
	BEGIN
		UPDATE EmployeeOfficial SET DesignationID= @PV_TPIDesignationID, EmployeeTypeID=@PV_TPIEmployeeTypeID WHERE EmployeeID=@PV_EmployeeID   
    END    
END

--ISINCREMENT
IF (@PV_IsIncrement=1)
BEGIN
	--IF  EXISTS(SELECT  TOP (1)* FROM EmployeeSalaryStructureHistory WHERE ESSID IN (SELECT ESSID FROM EmployeeSalaryStructure WHERE EmployeeID=@PV_EmployeeID) ORDER BY ESSHistoryID DESC)
	--BEGIN
	--	SET @PV_StartDate=(SELECT TOP (1)StartDate FROM EmployeeOfficialHistory WHERE EmployeeOfficialID IN (SELECT EmployeeOfficialID FROM EmployeeOfficial WHERE EmployeeID =@PV_EmployeeID) ORDER BY EmployeeOfficialHistoryID DESC)
	--END	
	--ELSE
	--BEGIN
	--	SET @PV_StartDate=(SELECT TOP (1)DateOfJoin FROM EmployeeOfficial WHERE EmployeeID = @PV_EmployeeID ORDER BY EmployeeOfficialID DESC)
	--END
	
	--INSERT EmployeeSalaryStructureHistory
	SET @PV_EndDate= DATEADD(DAY,-1,@Param_ActualEffectedDate)	
	SET @PV_ESSHistoryID=(SELECT ISNULL(MAX(ESSHistoryID),0)+1 FROM EmployeeSalaryStructureHistory)
	SET @PV_ESSID=(SELECT ESSID FROM EmployeeSalaryStructure WHERE EmployeeID=@PV_EmployeeID)
	SET @PV_IsIncludeFixedItem=(SELECT IsIncludeFixedItem FROM  EmployeeSalaryStructure WHERE EmployeeID=@PV_EmployeeID)
	SET @PV_ActualGrossAmount=(SELECT ActualGrossAmount FROM  EmployeeSalaryStructure WHERE EmployeeID=@PV_EmployeeID)
	SET @PV_CurrencyID=	(SELECT CurrencyID FROM  EmployeeSalaryStructure WHERE EmployeeID=@PV_EmployeeID)
	SET @PV_IsActive=(SELECT IsActive FROM  EmployeeSalaryStructure WHERE EmployeeID=@PV_EmployeeID)

	INSERT INTO EmployeeSalaryStructureHistory(ESSHistoryID,    ESSID,    EmployeeID,    SalarySchemeID,     [Description],GrossAmount,    IsIncludeFixedItem,    ActualGrossAmount,    CurrencyID,    IsActive,    DBUSerID,       DBServerDateTime)
										VALUES(@PV_ESSHistoryID,@PV_ESSID,@PV_EmployeeID,@PV_SalarySchemeID, 'N/A',        @PV_GrossAmount,@PV_IsIncludeFixedItem,@PV_ActualGrossAmount,@PV_CurrencyID,@PV_IsActive,@Param_DBUserID,@PV_DBServerDateTime )
	  
	--INSERT  EmployeeSalaryStructureDetailHistory
    --DECLARE
	--@TempQuery AS VARCHAR(500)
	
	DECLARE Cur_EmployeeSalaryStructureHistory CURSOR FOR
	SELECT  ESSHistoryID FROM EmployeeSalaryStructureHistory WHERE ESSID IN(SELECT ESSID FROM  EmployeeSalaryStructure WHERE EmployeeID=@PV_EmployeeID)
	OPEN Cur_EmployeeSalaryStructureHistory
    FETCH NEXT FROM Cur_EmployeeSalaryStructureHistory INTO  @PV_ESSHistoryID
	WHILE @@Fetch_Status = 0 
		BEGIN
		
			DECLARE Cur_EmployeeSalaryStructureDetail CURSOR FOR
			SELECT  SalaryHeadID,Amount FROM EmployeeSalaryStructureDetail WHERE ESSID IN(SELECT ESSID FROM  EmployeeSalaryStructure WHERE EmployeeID=@PV_EmployeeID)
			OPEN Cur_EmployeeSalaryStructureDetail
			FETCH NEXT FROM Cur_EmployeeSalaryStructureDetail INTO  @PV_SalaryHeadID,@PV_Amount
			WHILE @@Fetch_Status = 0 
				BEGIN
				SET @PV_ESSSHistoryID=(SELECT ISNULL(MAX(ESSSHistoryID),0)+1 FROM EmployeeSalaryStructureDetailHistory)
							
				--SET @TempQuery=N'INSERT INTO EmployeeSalaryStructureDetailHistory (ESSSHistoryID, ESSHistoryID, SalaryHeadID, Amount)
	
				--			VALUES('+N','+CONVERT(VARCHAR,@PV_ESSSHistoryID)+N','+ CONVERT(VARCHAR,@PV_ESSHistoryID) +N','+ CONVERT(VARCHAR,@PV_SalaryHeadID) +N','+ CONVERT(VARCHAR,@PV_Amount) +N')'
				--EXEC (@TempQuery)
				    INSERT INTO EmployeeSalaryStructureDetailHistory (ESSSHistoryID, ESSHistoryID, SalaryHeadID, Amount)
	
				    VALUES(CAST(@PV_ESSSHistoryID AS VARCHAR(10)) ,CAST(@PV_ESSHistoryID AS VARCHAR(10)),CAST(@PV_SalaryHeadID AS VARCHAR(10)) ,CAST(@PV_Amount AS VARCHAR(10)))
				--EXEC (@TempQuery)
				FETCH NEXT FROM Cur_EmployeeSalaryStructureDetail INTO   @PV_SalaryHeadID,@PV_Amount
				END
			CLOSE Cur_EmployeeSalaryStructureDetail
			DEALLOCATE Cur_EmployeeSalaryStructureDetail
		
		FETCH NEXT FROM Cur_EmployeeSalaryStructureHistory INTO   @PV_ESSHistoryID
		END
	CLOSE Cur_EmployeeSalaryStructureHistory
	DEALLOCATE Cur_EmployeeSalaryStructureHistory
	
	--UPDATE EmployeeSalaryStructure
	UPDATE EmployeeSalaryStructure SET SalarySchemeID= @PV_TPISalarySchemeID ,GrossAmount=@PV_TPIGrossAmount,ActualGrossAmount=@PV_TPIGrossAmount,IsIncludeFixedItem=@PV_TPIIsFixedAmount, CompGrossAmount = @PV_CompTPIGrossAmount, IsCashFixed=@PV_IsCashFixed, CashAmount = @PV_CashAmount  WHERE  EmployeeID=@PV_EmployeeID  --@PV_CompGrossAmount
	
	--DELETE EmployeeSalaryStructureDetail
	DELETE FROM EmployeeSalaryStructureDetail  WHERE ESSID IN (SELECT ESSID FROM EmployeeSalaryStructure WHERE EmployeeID=@PV_EmployeeID)
--=======================
	--INSERT EmployeeSalaryStructureDetail
	--SET @PV_ESSID=(SELECT ESSID FROM EmployeeSalaryStructure WHERE EmployeeID=@PV_EmployeeID)
	--DECLARE Cur_SalarySchemeDetail CURSOR LOCAL FORWARD_ONLY KEYSET FOR
	--SELECT  IsFixedBasic,BasicValue,AllowanceValue,SalaryHeadType,AllowanceCalculationOn,CalculateOnSalaryHeadID FROM View_SalarySchemeDetail WHERE SalarySchemeID=@PV_SalarySchemeID
	--OPEN Cur_SalarySchemeDetail
	--	FETCH NEXT FROM Cur_SalarySchemeDetail INTO  @PV_IsFixedBasic,@PV_BasicValue,@PV_AllowanceValue,@PV_SalaryHeadType,@PV_AllowanceCalculationOn,@PV_CalculationOnSalaryHeadID
	--	WHILE @@Fetch_Status = 0 
	--	BEGIN--2
	--		SET @PV_ESSSID=(SELECT ISNULL(MAX(ESSSID),0)+1 FROM EmployeeSalaryStructureDetail)
	--			IF(@PV_SalaryHeadType=1)
	--				BEGIN
	--					IF(@PV_IsFixedBasic=1)
	--						BEGIN
	--							SET @PV_Amount=@PV_BasicValue
	--						END
	--					ELSE
	--						BEGIN
	--							SET @PV_Amount=@PV_TPIGrossAmount/100*@PV_BasicValue 
	--						END
	--				END
	--			ELSE
	--				BEGIN
	--					IF(@PV_AllowanceCalculationOn=2)
	--						BEGIN
	--							DECLARE Cur_CC4 CURSOR LOCAL FORWARD_ONLY KEYSET FOR
	--							SELECT  SalaryHeadID FROM View_SalarySchemeDetail WHERE SalarySchemeID=@PV_SalarySchemeID AND AllowanceCalculationOn != 2
	--								OPEN Cur_CC4
	--									FETCH NEXT FROM Cur_CC4 INTO  @PV_SalaryHeadID
	--									WHILE(@@Fetch_Status <> -1)
	--										BEGIN--3
	--											IF(@PV_SalaryHeadID=@PV_CalculationOnSalaryHeadID)
	--												BEGIN
	--													SET @PV_Amount=@PV_AllowanceValue/100*@PV_Amount
	--												END
	--												FETCH NEXT FROM Cur_CC4 INTO  @PV_SalaryHeadID
	--										END--3
	--								CLOSE Cur_CC4
	--								DEALLOCATE Cur_CC4
	--						END
	--					ELSE
	--						BEGIN
	--							SET @PV_Amount=@PV_AllowanceValue
	--						END    
	--				END
	--		SET @TempQuery=N'INSERT INTO EmployeeSalaryStructureDetail(ESSSID,                           ESSID,                         SalaryHeadID,                         Amount,                              DBUSerID,                            DBServerDateTime)
	--														   VALUES('+CONVERT(VARCHAR,@PV_ESSSID)+N','+CONVERT(VARCHAR,@PV_ESSID)+N','+CONVERT(VARCHAR,@PV_SalaryHeadID)+N','+CONVERT(VARCHAR,@PV_Amount)+N','+CONVERT(VARCHAR,@Param_DBUserID)+N','+CONVERT(VARCHAR,@PV_DBServerDateTime)+N')'			
	--		EXEC (@TempQuery)
				
	--	FETCH NEXT FROM Cur_SalarySchemeDetail INTO  @PV_IsFixedBasic,@PV_BasicValue,@PV_AllowanceValue,@PV_SalaryHeadType,@PV_AllowanceCalculationOn,@PV_CalculationOnSalaryHeadID
	--	END--2
	--CLOSE Cur_SalarySchemeDetail
	--DEALLOCATE Cur_SalarySchemeDetail

--=========================
END
SELECT * FROM View_TransferPromotionIncrement WHERE TPIID= @Param_TPIID
COMMIT TRAN


GO
/****** Object:  StoredProcedure [dbo].[SP_Process_UpdateComplianceAttendanceDaily]    Script Date: 11/5/2018 8:43:20 AM ******/
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

		--IF EXISTS(SELECT * FROM AttendanceDaily WHERE AttendanceID = @PV_AttendanceID AND (CAST(CompInTime AS TIME(0))) = '00:00:00' AND (CAST(CompOutTime AS TIME(0))) = '00:00:00'  AND IsCompLeave=0 AND IsCompDayOff=0)
		--BEGIN
		--	GOTO CONT;
		--END

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
		
		IF EXISTS(SELECT * FROM AttendanceDaily WHERE AttendanceID = @PV_AttendanceID AND (CAST(CompInTime AS TIME(0))) = '00:00:00' AND (CAST(CompOutTime AS TIME(0))) = '00:00:00'  AND IsCompLeave=0 AND IsCompDayOff=0)
		BEGIN
			SET @PV_InTime = DATEADD(MINUTE,RAND()*(-@Param_BufferTime),@PV_StartTime)
			SET @PV_OutTime = DATEADD(MINUTE,RAND()*(@Param_BufferTime),@PV_EndTime)
		END ELSE BEGIN
			SET @PV_InTime = CASE WHEN CAST(@PV_InTime AS TIME(0))< CAST(@PV_StartTime AS TIME(0)) THEN DATEADD(MINUTE,RAND()*(-@Param_BufferTime),@PV_StartTime) ELSE @PV_InTime END
			SET @PV_OutTime = CASE WHEN CAST(@PV_OutTime AS TIME(0))>CAST(@PV_EndTime AS TIME(0)) THEN DATEADD(MINUTE,RAND()*(@Param_BufferTime),@PV_EndTime) ELSE @PV_OutTime END
		END
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
/****** Object:  StoredProcedure [dbo].[SP_Rpt_IncrementByPercent]    Script Date: 11/5/2018 8:43:20 AM ******/
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
	CREATE TABLE #tbl_EmployeeSalaryStructure(EmployeeID INT, ESSID INT, SalarySchemeID INT, PreviousGrossAmount DECIMAL(18, 2), PreviousBasicAmount DECIMAL(18, 2), IncrementedGrossAmount DECIMAL(18, 2), IncrementedBasicAmount DECIMAL(18, 2), Code VARCHAR(512), Name VARCHAR(512), DesignationID INT, DRPID INT, ASID INT, DOJ DATE, SalarySchemeName VARCHAR(512), IndexNo INT)

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
		  ,EO.DateOfJoin
		  ,SS.Name
		  ,0
	FROM #tbl_EmployeeID EmpID
	LEFT JOIN EmployeeSalaryStructure ESS ON EmpID.EmployeeID = ESS.EmployeeID
	LEFT JOIN Employee EMP ON EmpID.EmployeeID = EMP.EmployeeID
	LEFT JOIN EmployeeOfficial EO ON EmpID.EmployeeID = EO.EmployeeID
	LEFT JOIN SalaryScheme SS ON ESS.SalarySchemeID = SS.SalarySchemeID
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
/****** Object:  StoredProcedure [dbo].[SP_Rpt_SalarySummaryDetail_BUWise_Group]    Script Date: 11/5/2018 8:43:20 AM ******/
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
		@Param_EmpGrouping INT

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
		IF EXISTS(SELECT * FROM Users WHERE userID = @Param_UserId AND FinancialUserType!=1)
		BEGIN
			 SET @sSQL = @sSQL + ' AND VES.DepartmentID IN(SELECT DepartmentID FROM DepartmentRequirementPolicy WHERE DepartmentRequirementPolicyID IN(SELECT DRPID FROM DepartmentRequirementPolicyPermission WHERE UserID ='  + CONVERT(VARCHAR(50),@Param_UserId)+'))'
		END
	END
	SET @sSQL += ' AND VES.EmployeeID IN(select EmployeeID from EmployeeGroup where EmployeeTypeID IN(select EmployeeTypeID from EmployeeType where EmployeeGrouping IN('+CONVERT(VARCHAR(20),@Param_EmpGrouping)+')))'
	
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
/****** Object:  StoredProcedure [dbo].[SP_RPT_TimeCard_MaxOTCon]    Script Date: 11/5/2018 8:43:20 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		MD. MASUD IQBAL
-- Create date: 19 Nov 2017
-- Description:	As Per MaxOTConfiguration Time will show attendance. The process of SP will be changed.
-- =============================================
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

	--SELECT * FROM MaxOTConfigurationAttendance WHERE MOCID =@Param_MOCID AND EmployeeID

	----Update time card
	--UPDATE #tbl_TimeCard SET OverTimeInMinute= CASE WHEN OverTimeInMinute>@PV_MaxOTInMin THEN @PV_MaxOTInMin ELSE OverTimeInMinute END
	UPDATE #tbl_TimeCard SET ShiftEndTime= (SELECT EndTime FROM HRM_Shift WHERE ShiftID=#tbl_TimeCard.ShiftID)
	--UPDATE #tbl_TimeCard SET ShiftEndTime= (DATEADD(SECOND,0,DATEADD (MINUTE,DATEPART(MINUTE,ShiftEndTime), DATEADD(HOUR, DATEPART(HOUR,ShiftEndTime), DATEADD(dd, DATEDIFF(dd, -0, OutTime), 0)))))
	IF @PV_MaxOutTime IS NULL
	BEGIN
		UPDATE #tbl_TimeCard SET ShiftEndTime= (DATEADD(SECOND,0,DATEADD (MINUTE,DATEPART(MINUTE,ShiftEndTime), DATEADD(HOUR, DATEPART(HOUR,ShiftEndTime), DATEADD(dd, DATEDIFF(dd, -0, InTime), 0)))))
		UPDATE #tbl_TimeCard SET ShiftEndTime= DATEADD(MINUTE,@PV_MaxOTInMin,ShiftEndTime)
	END ELSE BEGIN
		UPDATE #tbl_TimeCard SET ShiftEndTime= (DATEADD(SECOND,0,DATEADD (MINUTE,DATEPART(MINUTE,@PV_MaxOutTime), DATEADD(HOUR, DATEPART(HOUR,@PV_MaxOutTime), DATEADD(dd, DATEDIFF(dd, -0, InTime), 0)))))
		SET @PV_MaxOTInMin=0
	END


	--Cursor for insert data

	DECLARE Cur_CCC CURSOR LOCAL FORWARD_ONLY KEYSET FOR
		SELECT EmployeeID,AttendanceDate,OutTime,ShiftEndTime,OverTimeInMinute,InTime, IsDayOff, IsHoliday, IsLeave, LeaveHeadID FROM #tbl_TimeCard /*WHERE OutTime>DATEADD(MINUTE,@PV_MaxOTInMin,ShiftEndTime) AND EmployeeID NOT IN (
		SELECT EmployeeID FROM MaxOTConfigurationAttendance WITH (NOLOCK) WHERE MOCID =@Param_MOCID AND AttendanceDate=#tbl_TimeCard.AttendanceDate AND OutTime IS NOT NULL) 
		AND (CAST(InTime AS TIME(0))!='00:00:00' AND CAST(OutTime AS TIME(0))!='00:00:00')*/	
	OPEN Cur_CCC
	FETCH NEXT FROM Cur_CCC INTO  @PV_Loop_EmployeeID,@PV_Loop_AttendanceDate,@PV_Loop_OutTime,@PV_Loop_ShiftEndTime,@PV_Loop_OverTimeInMinute,@PV_Loop_InTime, @PV_Loop_IsDayOff, @PV_Loop_IsHoliday, @PV_Loop_IsLeave, @PV_Loop_LeaveHeadID
	WHILE(@@Fetch_Status <> -1)		
	BEGIN	
		IF @PV_MinInTime IS NOT NULL
		BEGIN
			SET @PV_MinInTime=(DATEADD(SECOND,0,DATEADD (MINUTE,DATEPART(MINUTE,@PV_MinInTime), DATEADD(HOUR, DATEPART(HOUR,@PV_MinInTime), DATEADD(dd, DATEDIFF(dd, -0, @PV_Loop_AttendanceDate), 0)))))
		END

		IF NOT EXISTS (SELECT * FROM MaxOTConfigurationAttendance WITH (NOLOCK) WHERE MOCID =@Param_MOCID AND EmployeeID=@PV_Loop_EmployeeID AND AttendanceDate=@PV_Loop_AttendanceDate)
		BEGIN
			--select @PV_Loop_AttendanceDate,@PV_MinInTime
			INSERT INTO MaxOTConfigurationAttendance 
			SELECT @PV_Loop_EmployeeID,@Param_MOCID,@PV_Loop_AttendanceDate,DATEADD(MINUTE,RAND()*(-10),@PV_Loop_ShiftEndTime)--DATEADD(MINUTE,RAND()*5,DATEADD(MINUTE,@PV_MaxOTInMin-5,@PV_Loop_ShiftEndTime))
			,CASE WHEN @PV_Loop_OverTimeInMinute>@PV_MaxOTInMin THEN DATEDIFF(MINUTE,@PV_Loop_ShiftEndTime,DATEADD(MINUTE,RAND()*10,DATEADD(MINUTE,@PV_MaxOTInMin-5,@PV_Loop_ShiftEndTime))) ELSE  @PV_Loop_OverTimeInMinute END
			,CASE WHEN @PV_MinInTime IS NOT NULL AND @PV_Loop_InTime<@PV_MinInTime THEN DATEADD(MINUTE,RAND()*(10),@PV_MinInTime) ELSE @PV_Loop_InTime END
			--Asad
			,ISNULL(@PV_Loop_IsDayOff, 0)
			,ISNULL(@PV_Loop_IsHoliday, 0)
			,ISNULL(@PV_Loop_IsLeave, 0)
			,ISNULL(@PV_Loop_LeaveHeadID, 0)
		END ELSE BEGIN
			UPDATE MaxOTConfigurationAttendance SET OutTime=DATEADD(MINUTE,RAND()*(-10),@PV_Loop_ShiftEndTime)
			,OverTimeInMin= CASE WHEN @PV_Loop_OverTimeInMinute>@PV_MaxOTInMin THEN DATEDIFF(MINUTE,@PV_Loop_ShiftEndTime,DATEADD(MINUTE,RAND()*10,DATEADD(MINUTE,@PV_MaxOTInMin-5,@PV_Loop_ShiftEndTime))) ELSE  @PV_Loop_OverTimeInMinute END
			--Asad
			,InTime = CASE WHEN @PV_Loop_InTime IS NULL THEN DATEADD(MINUTE,RAND()*(10),@PV_MinInTime) END
			,IsDayOff=ISNULL(@PV_Loop_IsDayOff, 0)                  
			,IsHoliday=ISNULL(@PV_Loop_IsHoliday, 0)
			,IsLeave=ISNULL(@PV_Loop_IsLeave, 0)
			,LeaveHeadID=ISNULL(@PV_Loop_LeaveHeadID, 0)
			WHERE MOCID =@Param_MOCID AND EmployeeID=@PV_Loop_EmployeeID AND AttendanceDate=@PV_Loop_AttendanceDate AND OutTime IS NULL
		END

	FETCH NEXT FROM Cur_CCC INTO @PV_Loop_EmployeeID,@PV_Loop_AttendanceDate,@PV_Loop_OutTime,@PV_Loop_ShiftEndTime,@PV_Loop_OverTimeInMinute,@PV_Loop_InTime, @PV_Loop_IsDayOff, @PV_Loop_IsHoliday, @PV_Loop_IsLeave, @PV_Loop_LeaveHeadID
	END								
	CLOSE Cur_CCC
	DEALLOCATE Cur_CCC

	--For InTime
	--IF @PV_MinInTime IS NOT NULL
	--BEGIN
	--	DECLARE Cur_C CURSOR LOCAL FORWARD_ONLY KEYSET FOR
	--		SELECT EmployeeID,AttendanceDate,InTime FROM #tbl_TimeCard WHERE EmployeeID NOT IN (
	--		SELECT EmployeeID FROM MaxOTConfigurationAttendance WITH (NOLOCK) WHERE MOCID =@Param_MOCID AND AttendanceDate=#tbl_TimeCard.AttendanceDate AND InTime IS NOT NULL) 
	--		AND (CAST(InTime AS TIME(0))!='00:00:00')	
	--	OPEN Cur_C
	--	FETCH NEXT FROM Cur_C INTO  @PV_Loop_EmployeeID,@PV_Loop_AttendanceDate,@PV_Loop_InTime
	--	WHILE(@@Fetch_Status <> -1)		
	--	BEGIN					
	--			SET @PV_MinInTime=(DATEADD(SECOND,0,DATEADD (MINUTE,DATEPART(MINUTE,@PV_MinInTime), DATEADD(HOUR, DATEPART(HOUR,@PV_MinInTime), DATEADD(dd, DATEDIFF(dd, -0, @PV_Loop_AttendanceDate), 0)))))
				
	--			IF @PV_Loop_InTime<@PV_MinInTime
	--			BEGIN
	--				IF NOT EXISTS (SELECT * FROM MaxOTConfigurationAttendance WITH (NOLOCK) WHERE MOCID =@Param_MOCID AND EmployeeID=@PV_Loop_EmployeeID AND AttendanceDate=@PV_Loop_AttendanceDate)
	--				BEGIN
	--					INSERT INTO MaxOTConfigurationAttendance 
	--					SELECT @PV_Loop_EmployeeID,@Param_MOCID,@PV_Loop_AttendanceDate,NULL,NULL
	--					,DATEADD(MINUTE,RAND()*(10),@PV_MinInTime)
	--				END ELSE BEGIN
	--					UPDATE MaxOTConfigurationAttendance SET InTime=DATEADD(MINUTE,RAND()*(10),@PV_MinInTime) WHERE MOCID =@Param_MOCID AND EmployeeID=@PV_Loop_EmployeeID AND AttendanceDate=@PV_Loop_AttendanceDate AND InTime IS NULL
	--				END
	--			END
	--	FETCH NEXT FROM Cur_C INTO @PV_Loop_EmployeeID,@PV_Loop_AttendanceDate,@PV_Loop_InTime
	--	END								
	--	CLOSE Cur_C
	--	DEALLOCATE Cur_C
	--END



	--UPDATE #tbl_TimeCard SET OutTime= DATEADD(MINUTE,@PV_MaxOTInMin,ShiftEndTime) WHERE OverTimeInMinute>@PV_MaxOTInMin AND CAST(OutTime AS TIME(0))!='00:00:00'	
	--UPDATE #tbl_TimeCard SET OutTime= CASE WHEN OutTime> DATEADD(MINUTE,@PV_MaxOTInMin,ShiftEndTime) THEN DATEADD(MINUTE,RAND()*5,DATEADD(MINUTE,@PV_MaxOTInMin,ShiftEndTime)) ELSE OutTime END WHERE OverTimeInMinute<=0 AND CAST(OutTime AS TIME(0))!='00:00:00'	
	
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

	--SELECT * FROM #tbl_EmpBasic
	--INSERT INTO #tbl_EmpBasicInfo
	--SELECT EB.*
	--,EMP.Code,EMP.Name
	--,EO.DateOfJoin 
	--,DRP.Department AS DeptName
	--,DRP.Location AS LocName
	--,DRP.BUName
	--FROM #tbl_EmpBasic EB
	--INNER JOIN Employee EMP ON Emp.EmployeeID=EB.EmployeeID
	--INNER JOIN EmployeeOfficial EO ON EO.EmployeeID=EB.EmployeeID
	--INNER JOIN View_DepartmentRequirementPolicy DRP ON DRP.DepartmentRequirementPolicyID=EO.DRPID

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

	--SELECT TC.*
	--,EMP.Code,EMP.Name
	--,EO.DateOfJoin 
	--,DRP.Department AS DeptName
	--,DRP.Location AS LocName
	--,DRP.BUName
	--FROM #tbl_TimeCard TC
	--INNER JOIN Employee EMP ON Emp.EmployeeID=TC.EmployeeID
	--INNER JOIN EmployeeOfficial EO ON EO.EmployeeID=TC.EmployeeID
	--INNER JOIN View_DepartmentRequirementPolicy DRP ON DRP.DepartmentRequirementPolicyID=EO.DRPID


	--SELECT * FROM #tbl_EmpBasic
	--SELECT * FROM #tbl_TimeCard
	DROP TABLE #tbl_TimeCard
	DROP TABLE #tbl_EmpBasic

END

GO
/****** Object:  StoredProcedure [dbo].[SP_Upload_DisciplinaryAction]    Script Date: 11/5/2018 8:43:20 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		Md. Azharul Islam
-- Create date: 06 june 2016
-- Description:	Upload Financial Adjustment
-- =============================================
CREATE PROCEDURE [dbo].[SP_Upload_DisciplinaryAction] 
--declare
	 @Param_EmployeeCode AS VARCHAR(50)
	,@Param_EffectDate AS DATE
	,@Param_SalaryHeadName AS VARCHAR(50)
	,@Param_Amount AS INT
	,@Param_CompAmount AS INT
	,@Param_DBUserID AS INT

	--SET @Param_EmployeeCode ='A1035'
	--SET @Param_EffectDate ='21 Mar 2016'
	--SET  @Param_SalaryHeadName ='SHIFT AMT'
	--SET  @Param_Amount ='500'
	--SET @Param_DBUserID =1
AS
BEGIN TRAN
	--Variable declaration
	DECLARE
	  @PV_DisciplinaryActionID int
	 ,@PV_ActionType as int
	 ,@PV_EmployeeID int
	 ,@PV_SalaryHeadID as int
	
	SET @PV_SalaryHeadID=0
	SET @PV_ActionType=0

	SET @PV_SalaryHeadID= (SELECT SalaryHeadID FROM SalaryHead WHERE Name=@Param_SalaryHeadName)
	SET @PV_ActionType=(SELECT SalaryHeadType FROM SalaryHead WHERE Name=@Param_SalaryHeadName)
	SET @PV_EmployeeID =(SELECT  EmployeeID FROM Employee WHERE Code=@Param_EmployeeCode)

	IF (@PV_EmployeeID IS NULL OR @PV_EmployeeID =0)
	BEGIN
		ROLLBACK
			RAISERROR (N'Employee Not Active!!',16,1);	
		RETURN
	END	
	IF (@PV_SalaryHeadID IS NULL OR @PV_SalaryHeadID =0)
	BEGIN
		ROLLBACK
			RAISERROR (N'Must Give SalaryHead Name or incorrect head name!!',16,1);	
		RETURN
	END	
	IF (@Param_EffectDate IS NULL OR @Param_EffectDate='')
	BEGIN
		ROLLBACK
			RAISERROR (N'Must Give SalaryHead Name!!',16,1);	
		RETURN
	END	

	IF(@Param_EmployeeCode!='' AND @Param_SalaryHeadName !='' AND @Param_Amount>0 AND @Param_EffectDate IS NOT NULL AND @Param_EffectDate!='')
	BEGIN
		--Insert
		IF @PV_EmployeeID>0 AND @PV_SalaryHeadID>0 AND @PV_ActionType>0
		BEGIN
			SET @PV_DisciplinaryActionID=(SELECT ISNULL(MAX(DisciplinaryActionID),0)+1 FROM DisciplinaryAction)
			INSERT INTO [DisciplinaryAction]
						(DisciplinaryActionID    ,ActionType    ,EmployeeID    ,SalaryHeadID    ,PaymentCycle,[Description],Amount       , CompAmount,Remark ,ExecutedOn          ,[DBUserID]     ,[DBServerDateTime],ApproveBy,ApproveByDate)
					VALUES(@PV_DisciplinaryActionID,@PV_ActionType,@PV_EmployeeID,@PV_SalaryHeadID,3           ,''           ,@Param_Amount, @Param_CompAmount,''     , @Param_EffectDate  ,@Param_DBUserID,GETDATE(),@Param_DBUserID,GETDATE())
			
		END
		SELECT * FROM View_DisciplinaryAction  WHERE DisciplinaryActionID = @PV_DisciplinaryActionID
	END
COMMIT TRAN




GO
/****** Object:  StoredProcedure [dbo].[SP_Upload_IncrementAsPerScheme]    Script Date: 11/5/2018 8:43:20 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		Asadullah Sarker
-- Create date: 09 Apr 2018
-- Note:	   Increment Upload as per scheme
-- =============================================
CREATE PROCEDURE [dbo].[SP_Upload_IncrementAsPerScheme]
(

		@Param_EmployeeCode AS varchar(512),
		@Param_SalarySchemeName AS VARCHAR(512),
		@Param_ActualEffectedDate AS date,
		@Param_TPIGrossAmount AS DECIMAL(18, 2),
		@Param_CompTPIGrossAmount AS DECIMAL(18, 2),
		@Param_IsCashFixed AS BIT,
		@Param_CashAmount AS DECIMAL(18, 2),
		@Param_DBUserID AS int
	    --%n,%d,%n
)
AS
	
BEGIN	

	DECLARE
	--SALARYSCHEME DETAIL
	@PV_IsFixedBasic AS BIT,
	@PV_BasicValue AS FLOAT, 
	@PV_AllowanceValue AS FLOAT,
	@PV_SalaryHeadID AS INT,
	@PV_SalaryHeadType AS SMALLINT,
	@PV_Amount AS INT,
	@PV_AllowanceCalculationOn AS SMALLINT,
	@PV_CalculationOnSalaryHeadID AS INT,
	-- EmployeeOfficialHistory VARIABLES
	@PV_EmployeeOfficialHistoryID as int,
	@PV_EmployeeOfficialID as int,
	@PV_CurrentShiftID as int,
	@PV_WorkingStatus as smallint,
	@PV_StartDate as date,
	@PV_EndDate as date,
	@PV_ExistingGross as decimal(30,17),
	
	-- EmployeeSalaryStructureHistory VARIABLES
	@PV_ESSHistoryID as int,
	@PV_ESSID as int,
	@PV_IsIncludeFixedItem as bit,
	@PV_ActualGrossAmount as float,
	@PV_CurrencyID as int,
	@PV_IsActive as bit,

	--EmployeeSalaryStructureDetailHistory VARIABLES
	@PV_ESSSHistoryID as int,
	@PV_dESSHistoryID as int
	
	,@PV_SSDID as int
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
	
	--TPI VARIABLES AND GETS
	,@PV_DBServerDateTime as datetime,
	@PV_EmployeeID as int,
	@PV_IsTransfer as bit,
	@PV_IsPromotion as bit,
	@PV_IsIncrement as bit,
	@PV_ASID as int,
	@PV_DRPID as int,
	@PV_ShiftID as int,
	@PV_DesignationID as int,
	@PV_TPIDesignationID as int,
	@PV_TPIAttendanceSchemeID as int,
	@PV_TPIDRPID as int,
	@PV_GrossAmount as float,
	@PV_CompGrossAmount as float,
	@PV_CompTPIGrossAmount as float,
	@PV_SalarySchemeID as int,
	@PV_TPISalarySchemeID as int,
	@PV_TPIGrossAmount as int,
	@PV_TPIIsFixedAmount bit,
	@PV_TPIID INT,
	@PV_CompGross DECIMAL(18, 2),
	@PV_EmployeeTypeID INT,
	@PV_ESSSID INT


	
	IF NOT EXISTS(SELECT * FROM Employee WHERE Code = @Param_EmployeeCode)
	BEGIN
		ROLLBACK
				RAISERROR (N'Employee Not Exists by this code!!',16,1);	
		RETURN
	END
	IF NOT EXISTS(SELECT * FROM SalaryScheme WHERE Name = @Param_SalarySchemeName)
	BEGIN
		ROLLBACK
				RAISERROR (N'Salary Scheme Not Exists!!',16,1);	
		RETURN
	END

	SET @PV_EmployeeID = (SELECT EmployeeID FROM Employee WHERE Code=@Param_EmployeeCode)
	SET @PV_SalarySchemeID = (SELECT SalarySchemeID FROM SalaryScheme WHERE Name = @Param_SalarySchemeName)
	SET @PV_DesignationID = (SELECT DesignationID FROM EmployeeOfficial WHERE EmployeeID = @PV_EmployeeID)
	SET @PV_ASID = (SELECT AttendanceSchemeID FROM EmployeeOfficial WHERE EmployeeID = @PV_EmployeeID)
	SET @PV_DRPID = (SELECT DRPID FROM EmployeeOfficial WHERE EmployeeID = @PV_EmployeeID)
	SET @PV_ShiftID = (SELECT CurrentShiftID FROM EmployeeOfficial WHERE EmployeeID = @PV_EmployeeID)
	SET @PV_GrossAmount = (SELECT ActualGrossAmount FROM EmployeeSalaryStructure WHERE EmployeeID = @PV_EmployeeID)
	SET @PV_CompGross = (SELECT CompGrossAmount FROM EmployeeSalaryStructure WHERE EmployeeID = @PV_EmployeeID)
	SET @PV_EmployeeTypeID = (SELECT EmployeeTypeID FROM EmployeeOfficial WHERE EmployeeID = @PV_EmployeeID)

	SET @PV_TPISalarySchemeID = (SELECT SalarySchemeID FROM SalaryScheme WHERE Name = @Param_SalarySchemeName)
	
	SET @PV_TPIID=(SELECT ISNULL(MAX(TPIID),0)+1 FROM TransferPromotionIncrement)	
	INSERT INTO TransferPromotionIncrement
                (TPIID,       EmployeeID,       DesignationID,        DRPID,       ASID,       SalarySchemeID,       GrossSalary,       IsTransfer,       IsPromotion,       IsIncrement,       TPIDesignationID,       TPIDRPID,       TPIASID,       TPISalarySchemeID,       TPIShiftID,       TPIGrossSalary,      TPIIsFixedAmount,       Note,       EffectedDate, ActualEffectedDate,       RecommendedBy,ApproveBy, DBUserID,        DBServerDateTime,		CompGrossSalary, CompTPIGrossSalary,       EmployeeTypeID, TPIEmployeeTypeID,				IsNoHistory       ,IsCashFixed,    CashAmount)           
            
          VALUES(@PV_TPIID,@PV_EmployeeID, @PV_DesignationID,@PV_DRPID,@PV_ASID,@PV_SalarySchemeID,@PV_GrossAmount,0,0,1,@PV_DesignationID,@PV_DRPID,@PV_ASID,@PV_TPISalarySchemeID,@PV_ShiftID,@Param_TPIGrossAmount,0,'Multiple Increment',@Param_ActualEffectedDate,@Param_ActualEffectedDate,@Param_DBUserID,           @Param_DBUserID,      @Param_DBUserID,GETDATE(),	@PV_CompGross,	 @Param_CompTPIGrossAmount,@PV_EmployeeTypeID, @PV_EmployeeTypeID, 0,@Param_IsCashFixed, @Param_CashAmount) 
           
	SELECT @PV_EmployeeID=EmployeeID,
	       @PV_IsTransfer=IsTransfer,
	       @PV_IsPromotion=IsPromotion,
	       @PV_IsIncrement=IsIncrement,
	       @PV_ASID=ASID,
	       @PV_DRPID=DRPID,
	       @PV_DesignationID =DesignationID,
		   @PV_ShiftID = TPIShiftID,
	       @PV_TPIDesignationID=TPIDesignationID,
	       @PV_TPIDRPID=TPIDRPID,
	       @PV_TPIAttendanceSchemeID=TPIASID,
	       @PV_GrossAmount=GrossSalary,
	       @PV_CompGrossAmount=CompGrossSalary,
	       @PV_CompTPIGrossAmount=CompTPIGrossSalary,
	       @PV_SalarySchemeID=SalarySchemeID,
	       @PV_TPIGrossAmount=TPIGrossSalary,
	       @PV_TPIIsFixedAmount=TPIIsFixedAmount,
	       @PV_TPISalarySchemeID=TPISalarySchemeID
	FROM   TransferPromotionIncrement WHERE TPIID=@PV_TPIID
	
	SET    @PV_DBServerDateTime=Getdate()
	
--CHECK CONSTRAINTS



	--INSERT EmployeeSalaryStructureHistory
	SET @PV_EndDate= DATEADD(DAY,-1,@Param_ActualEffectedDate)	
	SET @PV_ESSHistoryID=(SELECT ISNULL(MAX(ESSHistoryID),0)+1 FROM EmployeeSalaryStructureHistory)
	SET @PV_ESSID=(SELECT ESSID FROM EmployeeSalaryStructure WHERE EmployeeID=@PV_EmployeeID)
	SET @PV_IsIncludeFixedItem=(SELECT IsIncludeFixedItem FROM  EmployeeSalaryStructure WHERE EmployeeID=@PV_EmployeeID)
	SET @PV_ActualGrossAmount=(SELECT ActualGrossAmount FROM  EmployeeSalaryStructure WHERE EmployeeID=@PV_EmployeeID)
	SET @PV_CurrencyID=	(SELECT CurrencyID FROM  EmployeeSalaryStructure WHERE EmployeeID=@PV_EmployeeID)
	SET @PV_IsActive=(SELECT IsActive FROM  EmployeeSalaryStructure WHERE EmployeeID=@PV_EmployeeID)

	INSERT INTO EmployeeSalaryStructureHistory(ESSHistoryID,    ESSID,    EmployeeID,    SalarySchemeID,     [Description],GrossAmount,    IsIncludeFixedItem,    ActualGrossAmount,    CurrencyID,    IsActive,    DBUSerID,       DBServerDateTime)
										VALUES(@PV_ESSHistoryID,@PV_ESSID,@PV_EmployeeID,@PV_SalarySchemeID, 'N/A',        @PV_GrossAmount,@PV_IsIncludeFixedItem,@PV_ActualGrossAmount,@PV_CurrencyID,@PV_IsActive,@Param_DBUserID,@PV_DBServerDateTime )
	  
	--INSERT  EmployeeSalaryStructureDetailHistory
    --DECLARE
	--@TempQuery AS VARCHAR(500)
	
	DECLARE Cur_EmployeeSalaryStructureHistory CURSOR FOR
	SELECT  ESSHistoryID FROM EmployeeSalaryStructureHistory WHERE ESSID IN(SELECT ESSID FROM  EmployeeSalaryStructure WHERE EmployeeID=@PV_EmployeeID)
	OPEN Cur_EmployeeSalaryStructureHistory
    FETCH NEXT FROM Cur_EmployeeSalaryStructureHistory INTO  @PV_ESSHistoryID
	WHILE @@Fetch_Status = 0 
		BEGIN
		
			DECLARE Cur_EmployeeSalaryStructureDetail CURSOR FOR
			SELECT  SalaryHeadID,Amount FROM EmployeeSalaryStructureDetail WHERE ESSID IN(SELECT ESSID FROM  EmployeeSalaryStructure WHERE EmployeeID=@PV_EmployeeID)
			OPEN Cur_EmployeeSalaryStructureDetail
			FETCH NEXT FROM Cur_EmployeeSalaryStructureDetail INTO  @PV_SalaryHeadID,@PV_Amount
			WHILE @@Fetch_Status = 0 
				BEGIN
				SET @PV_ESSSHistoryID=(SELECT ISNULL(MAX(ESSSHistoryID),0)+1 FROM EmployeeSalaryStructureDetailHistory)
							
				    INSERT INTO EmployeeSalaryStructureDetailHistory (ESSSHistoryID, ESSHistoryID, SalaryHeadID, Amount)
	
				    VALUES(CAST(@PV_ESSSHistoryID AS VARCHAR(10)) ,CAST(@PV_ESSHistoryID AS VARCHAR(10)),CAST(@PV_SalaryHeadID AS VARCHAR(10)) ,CAST(@PV_Amount AS VARCHAR(10)))
				--EXEC (@TempQuery)
				FETCH NEXT FROM Cur_EmployeeSalaryStructureDetail INTO   @PV_SalaryHeadID,@PV_Amount
				END
			CLOSE Cur_EmployeeSalaryStructureDetail
			DEALLOCATE Cur_EmployeeSalaryStructureDetail
		
		FETCH NEXT FROM Cur_EmployeeSalaryStructureHistory INTO   @PV_ESSHistoryID
		END
	CLOSE Cur_EmployeeSalaryStructureHistory
	DEALLOCATE Cur_EmployeeSalaryStructureHistory
	
	--UPDATE EmployeeSalaryStructure
	UPDATE EmployeeSalaryStructure SET SalarySchemeID= @PV_TPISalarySchemeID ,GrossAmount=@PV_TPIGrossAmount,ActualGrossAmount=@PV_TPIGrossAmount,IsIncludeFixedItem=@PV_TPIIsFixedAmount, CompGrossAmount = @PV_CompTPIGrossAmount, IsCashFixed=@Param_IsCashFixed, CashAmount=@Param_CashAmount  WHERE  EmployeeID=@PV_EmployeeID  --@PV_CompGrossAmount
	
	DELETE FROM EmployeeSalaryStructureDetail WHERE ESSID=@PV_ESSID

	DECLARE Cur_ESS CURSOR LOCAL FORWARD_ONLY KEYSET FOR
	SELECT ESSID,EmployeeID,ActualGrossAmount,SalarySchemeID, CompGrossAmount FROM EmployeeSalaryStructure WHERE ESSID=@PV_ESSID
	OPEN Cur_ESS
	FETCH NEXT FROM Cur_ESS INTO  @PV_ESSID,@PV_EmployeeID,@PV_ExistingGross,@PV_SalarySchemeID, @PV_CompGrossAmountStructure
	WHILE(@@Fetch_Status <> -1)
	BEGIN

	--loop for scheme & update detail

		SELECT @PV_FixedSalaryHeadIDs=COALESCE(@PV_FixedSalaryHeadIDs+',','')+CONVERT(varchar(50),SalaryHeadID) FROM SalarySchemeDetail WHERE SalarySchemeID=@PV_SalarySchemeID AND 1=(SELECT COUNT(*) FROM 
		SalarySchemeDetailCalculation WHERE SalarySchemeDetailCalculation.SalarySchemeDetailID=SalarySchemeDetail.SalarySchemeDetailID)



		DECLARE Cur_SSDetail CURSOR LOCAL FORWARD_ONLY KEYSET FOR
		SELECT SalarySchemeDetailID,SalaryHeadID FROM SalarySchemeDetail WHERE SalarySchemeID=@PV_TPISalarySchemeID AND SalaryHeadID IN (SELECT * FROM SplitInToDataSet(@PV_FixedSalaryHeadIDs,',')) Order BY SalaryHeadID DESC
		OPEN Cur_SSDetail
		FETCH NEXT FROM Cur_SSDetail INTO  @PV_SSDID ,@PV_SalaryHeadID
		WHILE(@@Fetch_Status <> -1)
		BEGIN--	
				--initialization				
				SET @PV_ValueOperator=0
				SET @PV_CalculationOn=0
				SET @PV_FixedValue=0
				SET @PV_CalSalaryHeadID=0
				SET @PV_PercentVelue=0
				SET @PV_Operator=0
				SET @PV_sEquation =''
				SET @PV_sEquationComp =''
				SET @PV_sValue=0.0
				SET @PV_sValueC=0.0
				SET @PV_SQL=''
				SET @PV_SQLC=''
				--get existing amount
				select @PV_ESSID, @PV_SalaryHeadID
				SET @PV_ExistingHeadAmount=ISNULL((SELECT TOP(1)Amount FROM EmployeeSalaryStructureDetail WHERE ESSID=@PV_ESSID AND SalaryHeadID=@PV_SalaryHeadID ORDER BY ESSSID DESC),0)


				--insert Employee Salary structure detail History
				SET @PV_ESSSHistoryID=(SELECT ISNULL(MAX(ESSSHistoryID),0)+1 FROM EmployeeSalaryStructureDetailHistory)
				INSERT INTO EmployeeSalaryStructureDetailHistory (ESSSHistoryID,	ESSHistoryID,	  SalaryHeadID,	   Amount)
														   VALUES(@PV_ESSSHistoryID ,@PV_ESSHistoryID,@PV_SalaryHeadID,@PV_ExistingHeadAmount)

				--update Employee SalaryStructure detail				
				/*
				ValueOperator (1=Value,2=Operator)
				EnumSalaryCalculationOn(Gross, SalaryItem, Fixed) IF VAlueOperator=2 THEN 0
				*/

				DECLARE Cur_CC CURSOR LOCAL FORWARD_ONLY KEYSET FOR
				SELECT ValueOperator,CalculationOn,FixedValue,SalaryHeadID,PercentVelue,Operator FROM SalarySchemeDetailCalculation WHERE SalarySchemeDetailID =@PV_SSDID  ORDER BY SSDCID ASC
				OPEN Cur_CC
				FETCH NEXT FROM Cur_CC INTO  @PV_ValueOperator,@PV_CalculationOn,@PV_FixedValue,@PV_CalSalaryHeadID,@PV_PercentVelue,@PV_Operator
				WHILE(@@Fetch_Status <> -1)
				BEGIN--	
					IF (@PV_ValueOperator=1)--Value
					BEGIN
						IF (@PV_CalculationOn=1)
						BEGIN
							SET @PV_sEquation=@PV_sEquation+CONVERT(varchar(100),@PV_TPIGrossAmount)
							SET @PV_sEquationComp=@PV_sEquationComp+CONVERT(varchar(100),@PV_CompTPIGrossAmount)
						END
						ELSE IF (@PV_CalculationOn=2) 
						BEGIN
							SET @PV_sEquation=@PV_sEquation+(SELECT CONVERT(varchar(100),Amount) FROM EmployeeSalaryStructureDetail WHERE ESSID=@PV_ESSID AND SalaryHeadID=@PV_CalSalaryHeadID)
							SET @PV_sEquationComp=@PV_sEquationComp+(SELECT CONVERT(varchar(100),CompAmount) FROM EmployeeSalaryStructureDetail WHERE ESSID=@PV_ESSID AND SalaryHeadID=@PV_CalSalaryHeadID)
						END
						ELSE IF (@PV_CalculationOn=3) 
						BEGIN
							SET @PV_sEquation=@PV_sEquation+CONVERT (varchar(100),@PV_FixedValue)		
							SET @PV_sEquationComp=@PV_sEquationComp+CONVERT (varchar(100),@PV_FixedValue)			
						END
					END
				
				FETCH NEXT FROM Cur_CC INTO  @PV_ValueOperator,@PV_CalculationOn,@PV_FixedValue,@PV_CalSalaryHeadID,@PV_PercentVelue,@PV_Operator
				END--
				CLOSE Cur_CC
				DEALLOCATE Cur_CC
				
				--value execution
				SET @PV_SQL=N'SET @PV_sValue=(SELECT '+@PV_sEquation+')'
				EXEC sp_executesql @PV_SQL, N'@PV_sValue decimal(30,17) out', @PV_sValue output

				--value execution
				SET @PV_SQLC=N'SET @PV_sValueC=(SELECT '+@PV_sEquationComp+')'
				EXEC sp_executesql @PV_SQLC, N'@PV_sValueC decimal(30,17) out', @PV_sValueC output

				IF NOT EXISTS(SELECT * FROM EmployeeSalaryStructureDetail WHERE ESSID=@PV_ESSID AND SalaryHeadID=@PV_SalaryHeadID)
				BEGIN
					SET @PV_ESSSID=(SELECT ISNULL(MAX(ESSSID),0)+1 FROM EmployeeSalaryStructureDetail)
					INSERT INTO EmployeeSalaryStructureDetail(ESSSID, ESSID, SalaryHeadID, Amount, DBUSerID, DBServerDateTime, CompAmount)
														VALUES(@PV_ESSSID, @PV_ESSID, @PV_SalaryHeadID, @PV_sValue, @Param_DBUserID, GETDATE(), @PV_sValueC)
				END ELSE 
				BEGIN
					--update Employee salary Structure
					UPDATE  EmployeeSalaryStructureDetail SET Amount=@PV_sValue, CompAmount=@PV_sValueC WHERE ESSID=@PV_ESSID AND SalaryHeadID=@PV_SalaryHeadID
				END

				FETCH NEXT FROM Cur_SSDetail INTO  @PV_SSDID ,@PV_SalaryHeadID
		END--
		CLOSE Cur_SSDetail
		DEALLOCATE Cur_SSDetail

		--SELECT @PV_NonFixedSalaryHeadIDs=COALESCE(@PV_NonFixedSalaryHeadIDs+',','')+CONVERT(varchar(50),SalaryHeadID) FROM SalarySchemeDetail WHERE 1<(SELECT COUNT(*) FROM 
		--SalarySchemeDetailCalculation WHERE SalarySchemeDetailCalculation.SalarySchemeDetailID=SalarySchemeDetail.SalarySchemeDetailID)


		DECLARE Cur_SSDetail_1 CURSOR LOCAL FORWARD_ONLY KEYSET FOR
		SELECT SalarySchemeDetailID,SalaryHeadID FROM SalarySchemeDetail WHERE SalarySchemeID=@PV_TPISalarySchemeID AND SalaryHeadID NOT IN (SELECT * FROM SplitInToDataSet(@PV_FixedSalaryHeadIDs,',')) Order BY SalaryHeadID ASC
		OPEN Cur_SSDetail_1
		FETCH NEXT FROM Cur_SSDetail_1 INTO  @PV_SSDID ,@PV_SalaryHeadID
		WHILE(@@Fetch_Status <> -1)
		BEGIN--	
				--initialization				
				SET @PV_ValueOperator=0
				SET @PV_CalculationOn=0
				SET @PV_FixedValue=0
				SET @PV_CalSalaryHeadID=0
				SET @PV_PercentVelue=0
				SET @PV_Operator=0
				SET @PV_sEquation =''
				SET @PV_sEquationComp =''
				SET @PV_sValue=0.0
				SET @PV_sValueC=0.0
				SET @PV_SQL=''
				SET @PV_SQLC=''
				--get existing amount
				SET @PV_ExistingHeadAmount=ISNULL((SELECT TOP(1)Amount FROM EmployeeSalaryStructureDetail WHERE ESSID=@PV_ESSID AND SalaryHeadID=@PV_SalaryHeadID ORDER BY ESSSID DESC), 0)


				--insert Employee Salary structure detail History
				SET @PV_ESSSHistoryID=(SELECT ISNULL(MAX(ESSSHistoryID),0)+1 FROM EmployeeSalaryStructureDetailHistory)
				INSERT INTO EmployeeSalaryStructureDetailHistory (ESSSHistoryID,	ESSHistoryID,	  SalaryHeadID,	   Amount)
														   VALUES(@PV_ESSSHistoryID ,@PV_ESSHistoryID,@PV_SalaryHeadID,@PV_ExistingHeadAmount)

				--update Employee SalaryStructure detail				
				/*
				ValueOperator (1=Value,2=Operator)
				EnumSalaryCalculationOn(Gross, SalaryItem, Fixed) IF VAlueOperator=2 THEN 0
				*/

				DECLARE Cur_CC CURSOR LOCAL FORWARD_ONLY KEYSET FOR
				SELECT ValueOperator,CalculationOn,FixedValue,SalaryHeadID,PercentVelue,Operator FROM SalarySchemeDetailCalculation WHERE SalarySchemeDetailID =@PV_SSDID  ORDER BY SSDCID ASC
				OPEN Cur_CC
				FETCH NEXT FROM Cur_CC INTO  @PV_ValueOperator,@PV_CalculationOn,@PV_FixedValue,@PV_CalSalaryHeadID,@PV_PercentVelue,@PV_Operator
				WHILE(@@Fetch_Status <> -1)
				BEGIN--	
					IF (@PV_ValueOperator=1)--Value
					BEGIN
						IF (@PV_CalculationOn=1)
						BEGIN
							SET @PV_sEquation=@PV_sEquation+CONVERT(varchar(100),@PV_TPIGrossAmount)
							SET @PV_sEquationComp=@PV_sEquationComp+CONVERT(varchar(100),@PV_CompTPIGrossAmount)
						END
						ELSE IF (@PV_CalculationOn=2) 
						BEGIN
							SET @PV_sEquation=@PV_sEquation+(SELECT CONVERT(varchar(100),Amount) FROM EmployeeSalaryStructureDetail WHERE ESSID=@PV_ESSID AND SalaryHeadID=@PV_CalSalaryHeadID)
							SET @PV_sEquationComp=@PV_sEquationComp+(SELECT CONVERT(varchar(100),CompAmount) FROM EmployeeSalaryStructureDetail WHERE ESSID=@PV_ESSID AND SalaryHeadID=@PV_CalSalaryHeadID)
						END
						ELSE IF (@PV_CalculationOn=3) 
						BEGIN
							SET @PV_sEquation=@PV_sEquation+CONVERT (varchar(100),@PV_FixedValue)			
							SET @PV_sEquationComp=@PV_sEquationComp+CONVERT (varchar(100),@PV_FixedValue)			
						END
					END
					ELSE IF (@PV_ValueOperator=2)--Operator
					BEGIN
						IF (@PV_Operator=1) 
						BEGIN 
							SET @PV_sEquation=@PV_sEquation+'(' 
							SET @PV_sEquationComp=@PV_sEquationComp+'(' 
						END
						IF (@PV_Operator=2) 
						BEGIN
							SET @PV_sEquation=@PV_sEquation+')'
							SET @PV_sEquationComp=@PV_sEquationComp+')'
						END
						IF (@PV_Operator=3) 
						BEGIN
							SET @PV_sEquation=@PV_sEquation+'+'
							SET @PV_sEquationComp=@PV_sEquationComp+'+'
						END
						IF (@PV_Operator=4) 
						BEGIN
							SET @PV_sEquation=@PV_sEquation+'-'
							SET @PV_sEquationComp=@PV_sEquationComp+'-'
						END
						IF (@PV_Operator=5) 
						BEGIN
							SET @PV_sEquation=@PV_sEquation+'*'
							SET @PV_sEquationComp=@PV_sEquationComp+'*'
						END
						IF (@PV_Operator=6) 
						BEGIN
							SET @PV_sEquation=@PV_sEquation+'/'
							SET @PV_sEquationComp=@PV_sEquationComp+'/'
						END
						IF (@PV_Operator=7)	
						BEGIN
							SET @PV_sEquation=@PV_sEquation+CONVERT(varchar(50),CONVERT(decimal(30,17),@PV_PercentVelue)/100)+'*'	
							SET @PV_sEquationComp=@PV_sEquationComp+CONVERT(varchar(50),CONVERT(decimal(30,17),@PV_PercentVelue)/100)+'*'				
						END
					END		
				
				FETCH NEXT FROM Cur_CC INTO  @PV_ValueOperator,@PV_CalculationOn,@PV_FixedValue,@PV_CalSalaryHeadID,@PV_PercentVelue,@PV_Operator
				END--
				CLOSE Cur_CC
				DEALLOCATE Cur_CC
				
				--value execution
				SET @PV_SQL=N'SET @PV_sValue=(SELECT '+@PV_sEquation+')'
				EXEC sp_executesql @PV_SQL, N'@PV_sValue decimal(30,17) out', @PV_sValue output
				--value execution
				SET @PV_SQLC=N'SET @PV_sValueC=(SELECT '+@PV_sEquationComp+')'
				EXEC sp_executesql @PV_SQLC, N'@PV_sValueC decimal(30,17) out', @PV_sValueC output


				IF NOT EXISTS(SELECT * FROM EmployeeSalaryStructureDetail WHERE ESSID=@PV_ESSID AND SalaryHeadID=@PV_SalaryHeadID)
				BEGIN
					SET @PV_ESSSID=(SELECT ISNULL(MAX(ESSSID),0)+1 FROM EmployeeSalaryStructureDetail)
					INSERT INTO EmployeeSalaryStructureDetail(ESSSID, ESSID, SalaryHeadID, Amount, DBUSerID, DBServerDateTime, CompAmount)
														VALUES(@PV_ESSSID, @PV_ESSID, @PV_SalaryHeadID, ROUND(@PV_sValue,2), @Param_DBUserID, GETDATE(), ROUND(@PV_sValueC,2))
				END ELSE 
				BEGIN
					--update Employee salary Structure
					UPDATE  EmployeeSalaryStructureDetail SET Amount=ROUND(@PV_sValue,2), CompAmount=ROUND(@PV_sValueC,2) WHERE ESSID=@PV_ESSID AND SalaryHeadID=@PV_SalaryHeadID
				END
				--update Employee salary Structure

				FETCH NEXT FROM Cur_SSDetail_1 INTO  @PV_SSDID ,@PV_SalaryHeadID
		END--
		CLOSE Cur_SSDetail_1
		DEALLOCATE Cur_SSDetail_1
		FETCH NEXT FROM Cur_ESS INTO  @PV_ESSID,@PV_EmployeeID,@PV_ExistingGross,@PV_SalarySchemeID, @PV_CompGrossAmountStructure
	END--
	CLOSE Cur_ESS
	DEALLOCATE Cur_ESS


SELECT * FROM View_TransferPromotionIncrement WHERE TPIID= @PV_TPIID
END



GO
/****** Object:  Table [dbo].[GrossSalaryCalculation]    Script Date: 11/5/2018 8:43:20 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[GrossSalaryCalculation](
	[GSCID] [int] NOT NULL,
	[SalarySchemeID] [int] NOT NULL,
	[ValueOperator] [smallint] NOT NULL,
	[CalculationOn] [int] NOT NULL,
	[FixedValue] [decimal](30, 17) NOT NULL,
	[Operator] [smallint] NOT NULL,
	[SalaryHeadID] [int] NOT NULL,
	[PercentVelue] [decimal](18, 2) NULL,
	[DBUserID] [int] NOT NULL,
	[DBServerDateTime] [datetime] NOT NULL,
 CONSTRAINT [PK_GrossSalaryCalculation] PRIMARY KEY CLUSTERED 
(
	[GSCID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  View [dbo].[View_GrossSalaryCalculation]    Script Date: 11/5/2018 8:43:20 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO




CREATE VIEW [dbo].[View_GrossSalaryCalculation]
AS
SELECT     GrossSalaryCalculation.*
         , SalaryHead.Name AS SalaryHeadName
         
FROM       GrossSalaryCalculation 

LEFT JOIN SalaryHead ON GrossSalaryCalculation.SalaryHeadID = SalaryHead.SalaryHeadID






GO
/****** Object:  View [dbo].[View_MaxOTConfigurationAttendance]    Script Date: 11/5/2018 8:43:20 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


CREATE VIEW [dbo].[View_MaxOTConfigurationAttendance]
AS
SELECT      MCA.*
			,Emp.Code AS EmployeeCode
			,Emp.Name AS EmployeeName
			,EO.CurrentShiftID AS ShiftID
			,HS.Name AS ShiftName
FROM        MaxOTConfigurationAttendance MCA
LEFT JOIN Employee Emp ON MCA.EmployeeID = Emp.EmployeeID
LEFT JOIN EmployeeOfficial EO ON MCA.EmployeeID = EO.EmployeeID
LEFT JOIN HRM_Shift HS ON EO.CurrentShiftID = HS.ShiftID





GO
