GO
/****** Object:  View [dbo].[View_SubledgerRefConfig]    Script Date: 2/5/2017 3:24:44 PM ******/
DROP VIEW [dbo].[View_SubledgerRefConfig]
GO
/****** Object:  View [dbo].[View_CostCenterTransaction]    Script Date: 2/5/2017 3:24:44 PM ******/
DROP VIEW [dbo].[View_CostCenterTransaction]
GO
/****** Object:  View [dbo].[View_AccountOpenningBreakdown]    Script Date: 2/5/2017 3:24:44 PM ******/
DROP VIEW [dbo].[View_AccountOpenningBreakdown]
GO
/****** Object:  View [dbo].[View_AccountHeadConfigure]    Script Date: 2/5/2017 3:24:44 PM ******/
DROP VIEW [dbo].[View_AccountHeadConfigure]
GO
/****** Object:  View [dbo].[View_ACCostCenter]    Script Date: 2/5/2017 3:24:44 PM ******/
DROP VIEW [dbo].[View_ACCostCenter]
GO
/****** Object:  Table [dbo].[SubledgerRefConfig]    Script Date: 2/5/2017 3:24:44 PM ******/
DROP TABLE [dbo].[SubledgerRefConfig]
GO
/****** Object:  StoredProcedure [dbo].[SP_Process_DynamicHead]    Script Date: 2/5/2017 3:24:44 PM ******/
DROP PROCEDURE [dbo].[SP_Process_DynamicHead]
GO
/****** Object:  StoredProcedure [dbo].[SP_IUD_SubledgerWiseBusinessUnit]    Script Date: 2/5/2017 3:24:44 PM ******/
DROP PROCEDURE [dbo].[SP_IUD_SubledgerWiseBusinessUnit]
GO
/****** Object:  StoredProcedure [dbo].[SP_IUD_SubledgerRefConfig]    Script Date: 2/5/2017 3:24:44 PM ******/
DROP PROCEDURE [dbo].[SP_IUD_SubledgerRefConfig]
GO
/****** Object:  StoredProcedure [dbo].[SP_IUD_ACCostCenter]    Script Date: 2/5/2017 3:24:44 PM ******/
DROP PROCEDURE [dbo].[SP_IUD_ACCostCenter]
GO
/****** Object:  StoredProcedure [dbo].[SP_IUD_ACCostCenter]    Script Date: 2/5/2017 3:24:44 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[SP_IUD_ACCostCenter] 
(
    @ACCostCenterID as	int,	 
	@Code as	varchar(512),	 
	@Name as	varchar(512),	 
	@Description as	varchar(512),	 
	@ParentID as	int,	 
	@ReferenceType as	smallint,	 
	@ReferenceObjectID as	int,	 
	@ActivationDate as	date,	 
	@ExpireDate as	date,	 
	@IsActive as	bit,	
	@DBUserID as	int,	 
	@DBOperation as smallint 
)
AS
BEGIN TRAN
DECLARE 
@DBServerDateTime as datetime,
@PerantIsLastLayer as bit,
@PreviousCode as Varchar(512),
@PreviousCodeInNumeric as int,
@Lenth as int,
@Flag as bit
SET @DBServerDateTime=Getdate()
SET @Flag=1

IF (@DBOperation=1)
BEGIN	
		SET @ACCostCenterID=(SELECT ISNULL(MAX(ACCostCenterID),0)+1 FROM ACCostCenter)						
		IF(@ReferenceType>0)
		BEGIN
			IF EXISTS(SELECT * FROM ACCostCenter AS CC WHERE CC.Name=@Name AND CC.ReferenceObjectID=0)
			BEGIN
				SET @ACCostCenterID=(SELECT TOP 1 CC.ACCostCenterID FROM ACCostCenter AS CC WHERE CC.Name=@Name  AND ReferenceObjectID=0)
				UPDATE ACCostCenter SET  Name=@Name,	[Description]=@Description,	ParentID=@ParentID,	ReferenceType=@ReferenceType,	ReferenceObjectID=@ReferenceObjectID,	ActivationDate=@ActivationDate,	[ExpireDate]=@ExpireDate,	IsActive=@IsActive, DBUserID=@DBUserID,	DBServerDateTime=@DBServerDateTime WHERE ACCostCenterID=@ACCostCenterID 	
				SELECT * FROM View_ACCostCenter WHERE ACCostCenterID=@ACCostCenterID
				SET @Flag=0
			END
		END
		
		IF(@Flag=1)
		BEGIN
			IF EXISTS(SELECT * FROM ACCostCenter AS CC WHERE CC.Name=@Name AND CC.ParentID=@ParentID)
			BEGIN
				ROLLBACK
					RAISERROR (N'Your Entered Name Already Exists!!~',16,1);	
				RETURN
			END
			IF(@ParentID=1)
			BEGIN
				IF EXISTS(SELECT * FROM ACCostCenter AS CC WHERE CC.ParentID=@ParentID)
				BEGIN
					SET @PreviousCode=(SELECT ISNULL(MAX(CONVERT(int,CC.Code)),0)  FROM ACCostCenter AS CC WHERE CC.ParentID=@ParentID)
					SET @PreviousCodeInNumeric =CONVERT(INT, @PreviousCode)+1
					SET @Code=RIGHT('00' + CONVERT(VARCHAR(3), @PreviousCodeInNumeric), 2)
				END
				ELSE
				BEGIN
					SET @Code='01'
				END
			END
			ELSE
			BEGIN
				IF EXISTS(SELECT * FROM ACCostCenter AS CC WHERE CC.ParentID =@ParentID)
				BEGIN
					SET @PreviousCode=(SELECT ISNULL(CC.Code,'') FROM ACCostCenter AS CC WHERE CC.ACCostCenterID=(SELECT ISNULL(MAX(CC.ACCostCenterID),0)  FROM ACCostCenter AS CC WHERE CC.ParentID=@ParentID))

					if(@Lenth>=3)
					BEGIN
					set @PreviousCode=  Left(@PreviousCode, LEN(@PreviousCode)-2)
					END
					SET @PreviousCodeInNumeric =CONVERT(INT, @PreviousCode)+1
					SET @Code=RIGHT('000' + CONVERT(VARCHAR(4), @PreviousCodeInNumeric), 3)
				END
				ELSE
				BEGIN
					SET @Code='001'
				END
			END
			INSERT INTO ACCostCenter  (ACCostCenterID,		Code,			Name,		[Description],		ParentID,		ReferenceType,		ReferenceObjectID,		ActivationDate,			[ExpireDate],		IsActive,	DBUserID,		DBServerDateTime)
								VALUES(@ACCostCenterID,		@Code,			@Name,		@Description,		@ParentID,		@ReferenceType,		@ReferenceObjectID,		@ActivationDate,		@ExpireDate,		@IsActive,	@DBUserID,		@DBServerDateTime)    				  
    		SELECT * FROM View_ACCostCenter WHERE ACCostCenterID=@ACCostCenterID
		END
END
IF (@DBOperation=2)
BEGIN	
	IF(@ACCostCenterID<=0)
	BEGIN
		ROLLBACK
			RAISERROR (N'Your Selected ACCostCenter Is Invalid Please Refresh and try again!!~',16,1);	
		RETURN
	END	
	IF EXISTS(SELECT * FROM ACCostCenter AS CC WHERE  CC.Name=@Name AND CC.ParentID=@ParentID  AND CC.ACCostCenterID!=@ACCostCenterID)
	BEGIN
		ROLLBACK
			RAISERROR (N'Your Entered Name Already Exists!!~',16,1);	
		RETURN
	END		
	IF((SELECT CC.ParentID FROM ACCostCenter AS CC WHERE CC.ACCostCenterID=@ACCostCenterID)!=@ParentID)
	BEGIN
		IF(@ParentID=1)
		BEGIN
			IF EXISTS(SELECT * FROM ACCostCenter AS CC WHERE CC.ParentID=@ACCostCenterID)
			BEGIN
				SET @PreviousCode=(SELECT ISNULL(CC.Code,'') FROM ACCostCenter AS CC WHERE CC.ACCostCenterID=(SELECT MAX(ACCC.ACCostCenterID) FROM ACCostCenter ACCC WHERE ACCC.ParentID=@ParentID))
				SET @PreviousCodeInNumeric =CONVERT(INT, @PreviousCode)+1
				SET @Code=RIGHT('000' + CONVERT(VARCHAR(3), @PreviousCodeInNumeric), 3)
			END
			ELSE
			BEGIN
				SET @Code='001'
			END
		END
		ELSE
		BEGIN
			IF EXISTS(SELECT * FROM ACCostCenter AS CC WHERE CC.ParentID=@ACCostCenterID)
			BEGIN
				SET @PreviousCode=(SELECT ISNULL(CC.Code,'') FROM ACCostCenter AS CC WHERE CC.ACCostCenterID=(SELECT MAX(ACCC.ACCostCenterID) FROM ACCostCenter ACCC WHERE ACCC.ParentID=@ParentID))
				SET @PreviousCodeInNumeric =CONVERT(INT, @PreviousCode)+1
				SET @Code=RIGHT('0000' + CONVERT(VARCHAR(4), @PreviousCodeInNumeric), 4)
			END
			ELSE
			BEGIN
				SET @Code='0001'
			END
		END
	END	
	UPDATE ACCostCenter SET  Code=@Code,	Name=@Name,	[Description]=@Description,	ParentID=@ParentID,	ReferenceType=@ReferenceType,	ReferenceObjectID=@ReferenceObjectID,	ActivationDate=@ActivationDate,	[ExpireDate]=@ExpireDate,	IsActive=@IsActive, DBUserID=@DBUserID,	DBServerDateTime=@DBServerDateTime WHERE ACCostCenterID=@ACCostCenterID
	SELECT * FROM View_ACCostCenter WHERE ACCostCenterID=@ACCostCenterID


END
IF (@DBOperation=3)
BEGIN	
	IF(@ACCostCenterID<=0)
	BEGIN
		ROLLBACK
			RAISERROR (N'Your Selected ACCostCenter Is Invalid Please Refresh and try again!!~',16,1);	
		RETURN
	END	
	IF EXISTS(SELECT * FROM ACCostCenter WHERE ParentID=@ACCostCenterID)
	BEGIN
		ROLLBACK
			RAISERROR (N'Deletion Not Possible! Cost Center Exists!!~',16,1);	
		RETURN
	END	
	IF EXISTS(SELECT * FROM AccountOpenningBreakdown WHERE BreakdownObjID=@ACCostCenterID AND BreakdownType=1)
	BEGIN
		ROLLBACK
			RAISERROR (N'Deletion Not Possible! Has Openning Reference!!~',16,1);	
		RETURN
	END	
	IF EXISTS(SELECT * FROM CostCenterTransaction WHERE CCID=@ACCostCenterID AND CCID IN (SELECT ACCostCenterID FROM ACCostCenter))
	BEGIN
		ROLLBACK
			RAISERROR (N'Deletion Not Possible! Has Voucher Reference!!~',16,1);	
		RETURN
	END	
	DELETE FROM BUWiseSubLedger WHERE SubLedgerID=@ACCostCenterID
	DELETE FROM SubledgerRefConfig WHERE SubledgerID=@ACCostCenterID
	DELETE FROM ACCostCenter WHERE ACCostCenterID=@ACCostCenterID
END

COMMIT TRAN













GO
/****** Object:  StoredProcedure [dbo].[SP_IUD_SubledgerRefConfig]    Script Date: 2/5/2017 3:24:44 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[SP_IUD_SubledgerRefConfig]
(
	@SubledgerRefConfigID	as int,
	@AccountHeadID	as int,
	@SubledgerID	as int,
	@IsBillRefApply	as bit,
	@IsOrderRefApply	as bit,
	@DBUserID as  int,	 
	@DBOperation as smallint
	--%n, %n, %n, %b, %b, %n, %n
)	
AS
BEGIN TRAN
DECLARE 
@DBServerDateTime as datetime,
@SubledgerCategoryID as int,
@TempSubLedgerID as int

SET @DBServerDateTime=Getdate()
SET @SubledgerCategoryID = ISNULL((SELECT HH.ParentID FROM ACCostCenter AS HH WHERE HH.ACCostCenterID=@SubLedgerID),0)
IF(@SubledgerCategoryID=1)
BEGIN
	DECLARE Cur_AB2 CURSOR LOCAL FORWARD_ONLY KEYSET FOR
	SELECT HH.ACCostCenterID FROM ACCostCenter AS HH WHERE HH.ParentID=@SubLedgerID
	OPEN Cur_AB2
	FETCH NEXT FROM Cur_AB2 INTO  @TempSubLedgerID
	WHILE(@@Fetch_Status <> -1)
	BEGIN	
		IF NOT EXISTS(SELECT * FROM SubledgerRefConfig AS HH WHERE HH.AccountHeadID=@AccountHeadID AND HH.SubledgerID=@TempSubLedgerID)
		BEGIN
			SET @SubledgerRefConfigID=(SELECT ISNULL(MAX(SubledgerRefConfigID),0)+1 FROM SubledgerRefConfig)				
				INSERT INTO SubledgerRefConfig	(SubledgerRefConfigID,		AccountHeadID,		SubledgerID,		IsBillRefApply,		IsOrderRefApply,	DBUserID,		DBServerDateTime)
    									 VALUES	(@SubledgerRefConfigID,		@AccountHeadID,		@TempSubLedgerID,		@IsBillRefApply,	@IsOrderRefApply,	@DBUserID,		@DBServerDateTime)
		END
		ELSE
		BEGIN
			SET @SubledgerRefConfigID = ISNULL((SELECT HH.SubledgerRefConfigID FROM SubledgerRefConfig AS HH WHERE HH.AccountHeadID=@AccountHeadID AND HH.SubledgerID=@TempSubLedgerID),0)
			UPDATE SubledgerRefConfig  SET AccountHeadID=@AccountHeadID, SubledgerID=@TempSubLedgerID,	IsBillRefApply=@IsBillRefApply, IsOrderRefApply=@IsOrderRefApply, DBUserID=@DBUserID, DBServerDateTime=@DBServerDateTime  WHERE SubledgerRefConfigID= @SubledgerRefConfigID
		END
	FETCH NEXT FROM Cur_AB2 INTO  @TempSubLedgerID
	END
	CLOSE Cur_AB2
	DEALLOCATE Cur_AB2	
END
ELSE
BEGIN
	IF NOT EXISTS(SELECT * FROM SubledgerRefConfig AS HH WHERE HH.AccountHeadID=@AccountHeadID AND HH.SubledgerID=@SubledgerID)
	BEGIN
		SET @SubledgerRefConfigID=(SELECT ISNULL(MAX(SubledgerRefConfigID),0)+1 FROM SubledgerRefConfig)				
			INSERT INTO SubledgerRefConfig	(SubledgerRefConfigID,		AccountHeadID,		SubledgerID,		IsBillRefApply,		IsOrderRefApply,	DBUserID,		DBServerDateTime)
    								 VALUES	(@SubledgerRefConfigID,		@AccountHeadID,		@SubledgerID,		@IsBillRefApply,	@IsOrderRefApply,	@DBUserID,		@DBServerDateTime)
	END
	ELSE
	BEGIN
		SET @SubledgerRefConfigID = ISNULL((SELECT HH.SubledgerRefConfigID FROM SubledgerRefConfig AS HH WHERE HH.AccountHeadID=@AccountHeadID AND HH.SubledgerID=@SubledgerID),0)
		UPDATE SubledgerRefConfig  SET AccountHeadID=@AccountHeadID, SubledgerID=@SubledgerID,	IsBillRefApply=@IsBillRefApply, IsOrderRefApply=@IsOrderRefApply, DBUserID=@DBUserID, DBServerDateTime=@DBServerDateTime  WHERE SubledgerRefConfigID= @SubledgerRefConfigID
	END
END
COMMIT TRAN






GO
/****** Object:  StoredProcedure [dbo].[SP_IUD_SubledgerWiseBusinessUnit]    Script Date: 2/5/2017 3:24:44 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[SP_IUD_SubledgerWiseBusinessUnit]
(
	@SubLedgerID as int,
	@BusinessUnitIDs as varchar(512),
	@DBUserID as int
	-- %n, %s, %n
)	
AS
BEGIN TRAN
DECLARE
@BUWiseSubLedgerID as int,
@DBServerDateTime as datetime,
@BusinessUnitID as int,
@SubledgerCategoryID as int,
@TempSubLedgerID as int

SET @DBServerDateTime=getdate()

IF(@SubLedgerID<=0)
BEGIN			
		
	ROLLBACK
		RAISERROR (N'Invalid Subledger.!!~',16,1);	
	RETURN
END

IF NOT EXISTS(SELECT * FROM dbo.ACCostCenter where ACCostCenterID=@SubLedgerID)
BEGIN			
		
	ROLLBACK
		RAISERROR (N'Invalid Subledger.!!~',16,1);	
	RETURN
END

IF NOT EXISTS(SELECT * FROM dbo.SplitInToDataSet(@BusinessUnitIDs,','))
BEGIN			
		
	ROLLBACK
		RAISERROR (N'No Business Units selected. Please Try Again.!!~',16,1);	
	RETURN
END

SET @SubledgerCategoryID = ISNULL((SELECT HH.ParentID FROM ACCostCenter AS HH WHERE HH.ACCostCenterID=@SubLedgerID),0)
IF(@SubledgerCategoryID=1)
BEGIN
	DECLARE Cur_AB2 CURSOR LOCAL FORWARD_ONLY KEYSET FOR
	SELECT HH.ACCostCenterID FROM ACCostCenter AS HH WHERE HH.ParentID=@SubLedgerID
	OPEN Cur_AB2
	FETCH NEXT FROM Cur_AB2 INTO  @TempSubLedgerID
	WHILE(@@Fetch_Status <> -1)
	BEGIN	
		DECLARE Cur_AB1 CURSOR LOCAL FORWARD_ONLY KEYSET FOR
		SELECT * FROM dbo.SplitInToDataSet(@BusinessUnitIDs,',')
		OPEN Cur_AB1
		FETCH NEXT FROM Cur_AB1 INTO  @BusinessUnitID
		WHILE(@@Fetch_Status <> -1)
		BEGIN	
			IF NOT EXISTS (SELECT * FROM BUWiseSubLedger AS BUAH WHERE BUAH.SubLedgerID=@TempSubLedgerID AND BUAH.BusinessUnitID=@BusinessUnitID)
			BEGIN
				SET @BUWiseSubLedgerID=(SELECT ISNULL(MAX(BUWiseSubLedgerID),0)+1 FROM BUWiseSubLedger)				
				INSERT INTO BUWiseSubLedger	(BUWiseSubLedgerID,		BusinessUnitID,		SubLedgerID,		DBUserID,	DBServerDateTime)
								VALUES		(@BUWiseSubLedgerID,	@BusinessUnitID,	@TempSubLedgerID,	@DBUserID,	@DBServerDateTime)
			END	
		FETCH NEXT FROM Cur_AB1 INTO  @BusinessUnitID
		END
		CLOSE Cur_AB1
		DEALLOCATE Cur_AB1
		DELETE FROM BUWiseSubLedger WHERE BusinessUnitID NOT IN (SELECT * FROM  dbo.SplitInToDataSet(@BusinessUnitIDs,',')) AND SubLedgerID = @TempSubLedgerID
	FETCH NEXT FROM Cur_AB2 INTO  @TempSubLedgerID
	END
	CLOSE Cur_AB2
	DEALLOCATE Cur_AB2	
END
ELSE
BEGIN
	DECLARE Cur_AB1 CURSOR LOCAL FORWARD_ONLY KEYSET FOR
	SELECT * FROM dbo.SplitInToDataSet(@BusinessUnitIDs,',')
	OPEN Cur_AB1
	FETCH NEXT FROM Cur_AB1 INTO  @BusinessUnitID
	WHILE(@@Fetch_Status <> -1)
	BEGIN	
		IF NOT EXISTS (SELECT * FROM BUWiseSubLedger AS BUAH WHERE BUAH.SubLedgerID=@SubLedgerID AND BUAH.BusinessUnitID=@BusinessUnitID)
		BEGIN
			SET @BUWiseSubLedgerID=(SELECT ISNULL(MAX(BUWiseSubLedgerID),0)+1 FROM BUWiseSubLedger)				
			INSERT INTO BUWiseSubLedger	(BUWiseSubLedgerID,		BusinessUnitID,		SubLedgerID,	DBUserID,	DBServerDateTime)
							VALUES		(@BUWiseSubLedgerID,	@BusinessUnitID,	@SubLedgerID,	@DBUserID,	@DBServerDateTime)
		END	
	FETCH NEXT FROM Cur_AB1 INTO  @BusinessUnitID
	END
	CLOSE Cur_AB1
	DEALLOCATE Cur_AB1
	DELETE FROM BUWiseSubLedger WHERE BusinessUnitID NOT IN (SELECT * FROM  dbo.SplitInToDataSet(@BusinessUnitIDs,',')) AND SubLedgerID = @SubLedgerID
END
COMMIT TRAN






GO
/****** Object:  StoredProcedure [dbo].[SP_Process_DynamicHead]    Script Date: 2/5/2017 3:24:44 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[SP_Process_DynamicHead] 
(
	@Param_DBUserID as int
)
AS
BEGIN TRAN
DECLARE 
--For COA
@AccountHeadID as int,
@AccountHeadCode as varchar(50),
@AccountHeadName as varchar (500),
@DBServerDate as datetime,
@ReferenceType as int,
@MappingID as int,
@ReferenceObjectID int,

--- For CC
@ACCostCenterID int,
@Code varchar (500),
@Name as varchar (500),
@Description as varchar (500),
@PreviousCode as varchar (500),
@PreviousCodeInNumeric int,
@IsBillRefApply bit

SET @Description='Auto Create'
SET @DBServerDate=GETDATE()

CREATE TABLE #TempTABLEOne (
								AccountHeadName Varchar(512),						
								ReferenceObjectID int,
								ReferenceType44 int,
								MappingType int
							)

--EnumReferenceType{ None = 0, Customer = 1, Vendor = 2, BankBranch = 3, BankAccount = 4, Vendor_Foreign = 5}
--EnumContractorType{ None = 0, Supplier = 1, Buyer = 2, Factory = 3, Bank = 4, Agent = 5, MotherBuyer = 6 }
INSERT INTO #TempTABLEOne( ReferenceObjectID,	AccountHeadName,	ReferenceType44)
					SELECT HH.ContractorID,		HH.Name,			2 AS VendorRef FROM Contractor AS HH WHERE HH.ContractorID IN (SELECT MM.ContractorID FROM ContractorType AS MM WHERE MM.ContractorType=1)

INSERT INTO #TempTABLEOne( ReferenceObjectID,	AccountHeadName,	ReferenceType44)
					SELECT HH.ContractorID,		HH.Name,			1 AS CustomerRef FROM Contractor AS HH WHERE HH.ContractorID IN (SELECT MM.ContractorID FROM ContractorType AS MM WHERE MM.ContractorType=2)

INSERT INTO #TempTABLEOne(	ReferenceObjectID,	AccountHeadName,						ReferenceType44)
					SELECT	TT.BankAccountID,	TT.BankShortName+'-['+TT.AccountNo+']',	4 FROM View_BankAccount  AS TT

CREATE TABLE #TempTABLE (
							MappingID int,
							AccountCode Varchar(512),
							AccountHeadName Varchar(512),						
							ReferenceObjectID int,
							ReferenceType int,
							MappingType int
						)

INSERT INTO #TempTABLE(ReferenceObjectID,	AccountHeadName,	ReferenceType,		MappingID,					MappingType)
				SELECT ReferenceObjectID,	AccountHeadName,	ReferenceType44,	Isnull(DymanicHeadSETup.MappingID,0),	Isnull(DymanicHeadSETup.MappingType,0) FROM #TempTABLEOne 
				LEFT JOIN DymanicHeadSETup on DymanicHeadSETup.ReferenceType=#TempTABLEOne.ReferenceType44
	

--INSERT COA_ChartsOfAccount according to the samme parent head by cursor		
DECLARE Cur_AB1 CURSOR GLOBAL FORWARD_ONLY KEYSET FOR           
SELECT ReferenceObjectID,AccountHeadName,MappingID,ReferenceType FROM #TempTABLE WHERE MappingType=1 AND Isnull(MappingID,0)>0
OPEN Cur_AB1
	FETCH NEXT FROM Cur_AB1 INTO @ReferenceObjectID,@AccountHeadName,@MappingID,@ReferenceType
	WHILE(@@Fetch_Status <> -1)
	BEGIN			
		SET @AccountHeadID=0
		SET @AccountHeadID=(SELECT ISNULL(MAX(AccountHeadID),0)+1 FROM ChartsOfAccount)		
		SET @AccountHeadCode=[dbo].[FN_ChartofAccountCode] (@MappingID)
				
		IF NOT EXISTS (SELECT * FROM ChartsOfAccount WHERE ReferenceObjectID= @ReferenceObjectID AND ParentHeadID=@MappingID AND ReferenceType=@ReferenceType)
		BEGIN
			INSERT INTO [dbo].[ChartsOfAccount] (AccountHeadID,		DAHCID,	AccountCode,		AccountHeadName,	AccountType,	ReferenceObjectID,		[Description],	IsJVNode,	IsDynamic,	ParentHeadID,	ReferenceType,	OpeningBalance,	DBUserID,	DBServerDate)
										VALUES	(@AccountHeadID,	5,		@AccountHeadCode,	@AccountHeadName,	5,				@ReferenceObjectID,		@Description,	0,			1,			@MappingID,		@ReferenceType,	0,				-9,			@DBServerDate)
		END
	FETCH NEXT FROM Cur_AB1 INTO @ReferenceObjectID,@AccountHeadName,@MappingID,@ReferenceType
	END 
CLOSE Cur_AB1
DEALLOCATE Cur_AB1


--Insert Cost Center according to the samme parent head by cursor		
DECLARE Cur_AB1 CURSOR GLOBAL FORWARD_ONLY KEYSET FOR           
SELECT ReferenceObjectID,AccountHeadName,MappingID,ReferenceType FROM #TempTABLE WHERE  MappingType=2 AND Isnull(MappingID,0)>0
OPEN Cur_AB1
	FETCH NEXT FROM Cur_AB1 INTO @ReferenceObjectID,@AccountHeadName,@MappingID,@ReferenceType
	WHILE(@@Fetch_Status <> -1)
	BEGIN
		SET @ACCostCenterID=(SELECT ISNULL(MAX(ACCostCenterID),0)+1 FROM ACCostCenter)
		IF EXISTS(SELECT * FROM ACCostCenter AS CC WHERE CC.ParentID=@MappingID)
		BEGIN
			SET @PreviousCode=(SELECT CC.Code FROM ACCostCenter AS CC WHERE CC.ACCostCenterID=(SELECT MAX(ACCC.ACCostCenterID) FROM ACCostCenter ACCC WHERE ACCC.ParentID=@MappingID))
			SET @PreviousCodeInNumeric =CONVERT(INT, @PreviousCode)+1
			SET @Code=RIGHT('0000' + CONVERT(VARCHAR(4), @PreviousCodeInNumeric), 4)
		END
		ELSE
		BEGIN
			SET @Code='0001'
		END
		
		IF NOT EXISTS(SELECT * FROM ACCostCenter WHERE ReferenceObjectID= @ReferenceObjectID AND ParentID=@MappingID AND ReferenceType=@ReferenceType)
		BEGIN
			INSERT INTO ACCostCenter  (ACCostCenterID,		Code,			Name,					[Description],		ParentID,		ReferenceType,		ReferenceObjectID,		ActivationDate,			[ExpireDate],		IsActive,   DBUserID,		DBServerDateTime)
								VALUES(@ACCostCenterID,		@Code,			@AccountHeadName,		@Description,		@MappingID,		@ReferenceType,		@ReferenceObjectID,		GetDate(),				GetDate(),			1,			-9,				@DBServerDate)
		END
		ELSE 
		BEGIN
			SET @ACCostCenterID = (SELECT TT.ACCostCenterID FROM ACCostCenter AS TT WHERE TT.ReferenceObjectID= @ReferenceObjectID AND TT.ParentID=@MappingID AND TT.ReferenceType=@ReferenceType)
			UPDATE ACCostCenter SET Name=@AccountHeadName,	[Description]=@Description WHERE ACCostCenterID=@ACCostCenterID
		END
	FETCH NEXT FROM Cur_AB1 INTO @ReferenceObjectID,@AccountHeadName,@MappingID,@ReferenceType
	END 
CLOSE Cur_AB1
DEALLOCATE Cur_AB1


DROP TABLE #TempTABLE
DROP TABLE #TempTABLEOne
COMMIT TRAN



GO
/****** Object:  Table [dbo].[SubledgerRefConfig]    Script Date: 2/5/2017 3:24:44 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[SubledgerRefConfig](
	[SubledgerRefConfigID] [int] NOT NULL,
	[AccountHeadID] [int] NULL,
	[SubledgerID] [int] NULL,
	[IsBillRefApply] [bit] NULL,
	[IsOrderRefApply] [bit] NULL,
	[DBUserID] [int] NULL,
	[DBServerDateTime] [datetime] NULL,
 CONSTRAINT [PK_SubledgerRefConfig] PRIMARY KEY CLUSTERED 
(
	[SubledgerRefConfigID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  View [dbo].[View_ACCostCenter]    Script Date: 2/5/2017 3:24:44 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO




CREATE View [dbo].[View_ACCostCenter]
AS
SELECT	ACCostCenter.ACCostCenterID,
		ACCostCenter.Code,
		ACCostCenter.Name,
		ACCostCenter.Description,
		ACCostCenter.ParentID,
		ACCostCenter.ReferenceType,
		ACCostCenter.ReferenceObjectID,
		ACCostCenter.ActivationDate,
		ACCostCenter.ExpireDate,
		ACCostCenter.IsActive,		
		dbo.FN_GetSLWBUName(ACCostCenter.ACCostCenterID, 0) AS BUName,		
		(SELECT CC.Name FROM ACCostCenter AS CC WHERE CC.ACCostCenterID=ACCostCenter.ParentID) AS CategoryName,
		(SELECT CC.Code FROM ACCostCenter AS CC WHERE CC.ACCostCenterID=ACCostCenter.ParentID)+''+ACCostCenter.Code AS CategoryCode,		
		0 AS DueAmount,
		0 AS OverDueDays,		
		ACCostCenter.Name+' ['+ACCostCenter.Code+']' AS NameCode
		                   
FROM	ACCostCenter






















GO
/****** Object:  View [dbo].[View_AccountHeadConfigure]    Script Date: 2/5/2017 3:24:44 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE VIEW [dbo].[View_AccountHeadConfigure]
AS
SELECT      AccountHeadConfigure.*,
			(SELECT HH.AccountHeadName FROM View_ChartsOfAccount AS HH WHERE HH.AccountHeadID=AccountHeadConfigure.AccountHeadID) AS AccountHeadName,
			(SELECT HH.[PathName] FROM View_ChartsOfAccount AS HH WHERE HH.AccountHeadID=AccountHeadConfigure.AccountHeadID) AS AccountPathName,
            (SELECT Name FROM ACCostCenter WHERE ACCostCenterID = AccountHeadConfigure.ReferenceObjectID AND ReferenceObjectType=1) AS Name,
			(SELECT [Description] FROM ACCostCenter WHERE ACCostCenterID = AccountHeadConfigure.ReferenceObjectID AND ReferenceObjectType=1) AS CostCenterDescription,
			(SELECT ProductCategoryName FROM ProductCategory WHERE ProductCategoryID = AccountHeadConfigure.ReferenceObjectID AND ReferenceObjectType=4) AS ProductCategoryName
		
FROM        AccountHeadConfigure
GO
/****** Object:  View [dbo].[View_AccountOpenningBreakdown]    Script Date: 2/5/2017 3:24:44 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE VIEW [dbo].[View_AccountOpenningBreakdown]
AS
SELECT	AOB.AccountOpenningBreakdownID,
		AOB.AccountingSessionID,	
		AOB.BusinessUnitID,	
		AOB.AccountHeadID,
		AOB.BreakdownObjID,
		AOB.IsDr,
		AOB.BreakdownType,
		AOB.MUnitID,
		AOB.WUnitID,
		AOB.UnitPrice,
		AOB.Qty,
		AOB.CurrencyID,
		AOB.ConversionRate,
		AOB.AmountInCurrency,
		AOB.OpenningBalance,
		AOB.CCID,	
		ACC.Code AS CCCode,
		ACC.Name AS CCName,	
		COA.AccountCode,
		COA.AccountHeadName,
		MU.UnitName,
		MU.Symbol,
		BusinessUnit.Code AS BUCode,
		BusinessUnit.Name AS BUName,		
		Currency.CurrencyName,
		Currency.Symbol AS CurrencySymbol,
		AccountingSession.SessionName,
		(SELECT TT.LOUName FROM View_WorkingUnit AS TT WHERE WorkingUnitID=AOB.WUnitID) AS WUName,		
		(SELECT CASE  
		WHEN AOB.BreakdownType=1 THEN (SELECT CC.Name FROM ACCostCenter AS CC WHERE CC.ACCostCenterID=AOB.BreakdownObjID)
		WHEN AOB.BreakdownType=2 THEN (SELECT VB.BillNo FROM VoucherBill AS VB WHERE VB.VoucherBillID=AOB.BreakdownObjID)
		WHEN AOB.BreakdownType=4 THEN (SELECT PD.ProductName FROM Product AS PD WHERE PD.ProductID=AOB.BreakdownObjID)
		END) AS BreakdownName,
		ISNULL((SELECT CASE  
		WHEN AOB.BreakdownType=1 THEN (SELECT CC.Code FROM ACCostCenter AS CC WHERE CC.ACCostCenterID=AOB.BreakdownObjID)
		WHEN AOB.BreakdownType=2 THEN (SELECT VB.BillNo FROM VoucherBill AS VB WHERE VB.VoucherBillID=AOB.BreakdownObjID)
		WHEN AOB.BreakdownType=4 THEN (SELECT PD.ProductCode FROM Product AS PD WHERE PD.ProductID=AOB.BreakdownObjID)
		END),'0000') AS BreakdownCode,
		(SELECT CASE  WHEN AOB.BreakdownType=1 THEN (SELECT SLRC.IsBillRefApply FROM SubledgerRefConfig AS SLRC WHERE SLRC.AccountHeadID=AOB.AccountHeadID AND SLRC.SubledgerID=AOB.BreakdownObjID) ELSE 0 END) AS IsBTAply

FROM AccountOpenningBreakdown AS AOB
INNER JOIN AccountingSession ON AOB.AccountingSessionID = AccountingSession.AccountingSessionID 
INNER JOIN BusinessUnit ON AOB.BusinessUnitID = BusinessUnit.BusinessUnitID
LEFT OUTER JOIN ChartsOfAccount AS COA ON AOB.AccountHeadID = COA.AccountHeadID
LEFT OUTER JOIN ACCostCenter AS ACC ON ACC.ACCostCenterID=AOB.CCID
LEFT OUTER JOIN MeasurementUnit AS MU ON AOB.MUnitID = MU.MeasurementUnitID
LEFT OUTER JOIN Currency ON AOB.CurrencyID = Currency.CurrencyID


















GO
/****** Object:  View [dbo].[View_CostCenterTransaction]    Script Date: 2/5/2017 3:24:44 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO






CREATE VIEW [dbo].[View_CostCenterTransaction]
AS
SELECT	CCT.CCTID, 
		CCT.CCID,
		VD.AccountHeadID, 
		VD.VoucherID,
		CCT.VoucherDetailID, 
        CCT.Amount, 
        CCT.CurrencyID, 
        CCT.CurrencyConversionRate, 
        CCT.[Description], 
        CCT.TransactionDate, 
        CCT.LastUpdateBY, 
        CCT.IsDr, 
        CCT.LastUpdateDate, 	                   
		(Select CU.Symbol from Currency as CU where CU.CurrencyID=CCT.CurrencyID) as CurrencySymbol,
        (Select CC.Name from ACCostCenter as CC where CC.ACCostCenterID=ACCostCenter.ParentID ) as CategoryName,		
        ACCostCenter.Code as CostCenterCode,
		ACCostCenter.Name as CostCenterName,
		ACCostCenter.ParentID AS CCCategoryID,
		(SELECT HH.IsBillRefApply FROM SubledgerRefConfig AS HH WHERE HH.AccountHeadID=VD.AccountHeadID AND HH.SubledgerID=CCT.CCID) AS IsBillRefApply,
		(SELECT HH.IsOrderRefApply FROM SubledgerRefConfig AS HH WHERE HH.AccountHeadID=VD.AccountHeadID AND HH.SubledgerID=CCT.CCID) AS IsOrderRefApply,
		VD.BUID,
		VD.AuthorizedBy AS ApprovedBy

FROM	CostCenterTransaction AS CCT
LEFT OUTER JOIN ACCostCenter ON CCT.CCID = ACCostCenter.ACCostCenterID
LEFT OUTER JOIN View_VoucherDetail AS VD ON CCT.VoucherDetailID=VD.VoucherDetailID
























GO
/****** Object:  View [dbo].[View_SubledgerRefConfig]    Script Date: 2/5/2017 3:24:44 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE VIEW [dbo].[View_SubledgerRefConfig]
AS
SELECT  SRC.SubledgerRefConfigID,
		SRC.AccountHeadID,
		SRC.SubledgerID,
		SRC.IsBillRefApply,
		SRC.IsOrderRefApply,		
		(SELECT COA.AccountHeadName FROM ChartsOfAccount AS COA WHERE COA.AccountHeadID=SRC.AccountHeadID) AS AccountHeadName,
		(SELECT COA.AccountCode FROM ChartsOfAccount AS COA WHERE COA.AccountHeadID=SRC.AccountHeadID) AS AccountCode,
		(SELECT ACC.Name FROM ACCostCenter AS ACC WHERE ACC.ACCostCenterID=SRC.SubledgerID) AS SubledgerName,
		(SELECT ACC.Code FROM ACCostCenter AS ACC WHERE ACC.ACCostCenterID=SRC.SubledgerID) AS SubledgerCode

FROM	SubledgerRefConfig AS SRC



GO
