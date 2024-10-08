GO
/****** Object:  View [dbo].[View_IntegrationSetupDetail]    Script Date: 1/29/2017 2:27:06 PM ******/
DROP VIEW [dbo].[View_IntegrationSetupDetail]
GO
/****** Object:  View [dbo].[View_DebitCreditSetup]    Script Date: 1/29/2017 2:27:06 PM ******/
DROP VIEW [dbo].[View_DebitCreditSetup]
GO
/****** Object:  View [dbo].[View_ChequeRequisitionDetail]    Script Date: 1/29/2017 2:27:06 PM ******/
DROP VIEW [dbo].[View_ChequeRequisitionDetail]
GO
/****** Object:  Table [dbo].[VoucherMapping]    Script Date: 1/29/2017 2:27:06 PM ******/
DROP TABLE [dbo].[VoucherMapping]
GO
/****** Object:  Table [dbo].[IntegrationSetupDetail]    Script Date: 1/29/2017 2:27:06 PM ******/
DROP TABLE [dbo].[IntegrationSetupDetail]
GO
/****** Object:  Table [dbo].[IntegrationSetup]    Script Date: 1/29/2017 2:27:06 PM ******/
DROP TABLE [dbo].[IntegrationSetup]
GO
/****** Object:  Table [dbo].[DebitCreditSetup]    Script Date: 1/29/2017 2:27:06 PM ******/
DROP TABLE [dbo].[DebitCreditSetup]
GO
/****** Object:  Table [dbo].[DataCollectionSetup]    Script Date: 1/29/2017 2:27:06 PM ******/
DROP TABLE [dbo].[DataCollectionSetup]
GO
/****** Object:  StoredProcedure [dbo].[SP_IUD_VoucherMapping]    Script Date: 1/29/2017 2:27:06 PM ******/
DROP PROCEDURE [dbo].[SP_IUD_VoucherMapping]
GO
/****** Object:  StoredProcedure [dbo].[SP_IUD_IntegrationSetupDetail]    Script Date: 1/29/2017 2:27:06 PM ******/
DROP PROCEDURE [dbo].[SP_IUD_IntegrationSetupDetail]
GO
/****** Object:  StoredProcedure [dbo].[SP_IUD_IntegrationSetup]    Script Date: 1/29/2017 2:27:06 PM ******/
DROP PROCEDURE [dbo].[SP_IUD_IntegrationSetup]
GO
/****** Object:  StoredProcedure [dbo].[SP_IUD_DebitCreditSetup]    Script Date: 1/29/2017 2:27:06 PM ******/
DROP PROCEDURE [dbo].[SP_IUD_DebitCreditSetup]
GO
/****** Object:  StoredProcedure [dbo].[SP_IUD_DataCollectionSetup]    Script Date: 1/29/2017 2:27:06 PM ******/
DROP PROCEDURE [dbo].[SP_IUD_DataCollectionSetup]
GO
/****** Object:  StoredProcedure [dbo].[SP_ApprovedVoucher]    Script Date: 1/29/2017 2:27:06 PM ******/
DROP PROCEDURE [dbo].[SP_ApprovedVoucher]
GO
/****** Object:  StoredProcedure [dbo].[SP_ApprovedVoucher]    Script Date: 1/29/2017 2:27:06 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[SP_ApprovedVoucher]
(
	@VoucherIDs as Varchar(MAX),
	@AuthorizedBy as int,
	@IsAutoProcess as bit
)
AS
BEGIN TRAN
DECLARE
@Index as int,
@Count as int,
@VoucherID as int,
@VoucherBatchID as int,
@VoucherNo as Varchar(512),
@DebitAmount as decimal(30,17),
@CreditAmount as decimal(30,17)

SET @Index=0
SET @Count=0
SET @Count= (SELECT COUNT(*) FROM dbo.SplitInToDataSet(@VoucherIDs,','))

IF(@Count<=0)
BEGIN
	ROLLBACK
		RAISERROR (N'Please select at least one voucher!!~',16,1);	
	RETURN
END
IF(ISNULL(@AuthorizedBy,0)=0)
BEGIN
	ROLLBACK
		RAISERROR (N'Invalid Authorized User!!~',16,1);	
	RETURN
END

WHILE (@Index<@Count)
BEGIN
	SET @VoucherID=dbo.SplitedStringGet(@VoucherIDs,',',@Index)
	SET @VoucherNo=(SELECT V.VoucherNo FROM  Voucher AS V WHERE V.VoucherID=@VoucherID)
	SET @DebitAmount=ISNULL((SELECT SUM(VD.Amount) FROM  VoucherDetail AS VD WHERE VD.VoucherID=@VoucherID AND VD.IsDebit=1),0)
	SET @CreditAmount=ISNULL((SELECT SUM(VD.Amount) FROM  VoucherDetail AS VD WHERE VD.VoucherID=@VoucherID AND VD.IsDebit=0),0)
	SET @VoucherBatchID=(SELECT V.BatchID FROM Voucher AS V WHERE V.VoucherID=@VoucherID)

	--EnumVoucherBatchStatus{None = 0, BatchOpen = 1, BatchClose = 2, RequestForApprove = 3, ApprovalInProgress = 4, Approved = 5}
	IF(@IsAutoProcess=0)
	BEGIN
		IF EXISTS (SELECT * FROM VoucherBatch AS VB WHERE VB.VoucherBatchID=@VoucherBatchID AND VB.BatchStatus<3)
		BEGIN
			ROLLBACK
				RAISERROR (N'Request for Approval of the Voucher Batch not submitted for Voucher No :[ %s ]!!~',16,1, @VoucherNo);	
			RETURN
		END
	END
	IF(ISNULL((SELECT V.AuthorizedBy FROM Voucher AS V WHERE V.VoucherID=@VoucherID),0)!=0)
	BEGIN
		ROLLBACK
			RAISERROR (N'Voucher No :[ %s ] already approved!!~',16,1, @VoucherNo);	
		RETURN
	END
	IF(@DebitAmount <=0)
	BEGIN
		ROLLBACK
			RAISERROR (N'Please Check Debit Amount For Voucher No : [ %s ] !!~',16,1, @VoucherNo);	
		RETURN
	END
	IF(@CreditAmount <=0)
	BEGIN
		ROLLBACK
			RAISERROR (N'Please Check Credit Amount For Voucher No : [ %s ] !!~',16,1, @VoucherNo);	
		RETURN
	END

	IF( ROUND(@DebitAmount,0)!=  ROUND(@CreditAmount,0))
	BEGIN
		ROLLBACK
			RAISERROR (N'Debit & Credit Amount For Voucher No : [ %s ] !!~',16,1, @VoucherNo);	
		RETURN
	END	
	UPDATE Voucher  SET AuthorizedBy=@AuthorizedBy WHERE VoucherID=@VoucherID	
	SET @VoucherBatchID=(SELECT V.BatchID FROM Voucher AS V WHERE V.VoucherID=@VoucherID)
	
	DECLARE
	@CounterVoucherID as int
	SET @CounterVoucherID = ISNULL((SELECT TT.CounterVoucherID FROM Voucher AS TT WHERE TT.VoucherID=@VoucherID),0)
	IF EXISTS(SELECT * FROM Voucher AS TT WHERE TT.VoucherID=@CounterVoucherID AND @CounterVoucherID!=0)
	BEGIN
		UPDATE Voucher  SET AuthorizedBy=@AuthorizedBy WHERE VoucherID=@CounterVoucherID	
	END

	IF(@IsAutoProcess=0)
	BEGIN
		IF NOT EXISTS (SELECT * FROM Voucher AS V WHERE V.BatchID=@VoucherBatchID AND V.AuthorizedBy=0)
		BEGIN
			UPDATE VoucherBatch SET BatchStatus=5 WHERE VoucherBatchID=@VoucherBatchID
		END
		ELSE
		BEGIN
			UPDATE VoucherBatch SET BatchStatus=4 WHERE VoucherBatchID=@VoucherBatchID
		END
	END

	SET @Index=@Index+1
END
SELECT * FROM View_Voucher WHERE VoucherID IN (SELECT items FROM dbo.SplitInToDataSet(@VoucherIDs,','))
COMMIT TRAN


GO
/****** Object:  StoredProcedure [dbo].[SP_IUD_DataCollectionSetup]    Script Date: 1/29/2017 2:27:06 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[SP_IUD_DataCollectionSetup]
(	
	@DataCollectionSetupID	as int,
	@DataReferenceType	as smallint,
	@DataReferenceID	as int,
	@DataSetupType	as smallint,
	@DataGenerateType	as smallint,
	@QueryForValue	as varchar(MAX),
	@ReferenceValueFields	as varchar(512),
	@FixedText as Varchar(512),
	@Note	as varchar(512),
	@DBUserID  as int,
	@DBOperation as smallint,
	@DataCollectionSetupIDs as varchar(512)
	--%n, %n, %n, %n, %n, %s, %s, %s, %s, %n, %n, %s
)
AS
BEGIN TRAN
DECLARE 
@PV_DBServerDateTime as datetime,
@PV_OrderType as smallint
SET @PV_DBServerDateTime=Getdate()	
	
IF(@DBOperation=1)
BEGIN	
	SET @DataCollectionSetupID= (SELECT ISNULL(MAX(DataCollectionSetupID),0)+1 FROM DataCollectionSetup)			
	INSERT INTO DataCollectionSetup	(DataCollectionSetupID,		DataReferenceType,	DataReferenceID,	DataSetupType,	DataGenerateType,	QueryForValue,	ReferenceValueFields,	FixedText,		Note,	DBUserID,	DBServerDateTime)		
						VALUES		(@DataCollectionSetupID,	@DataReferenceType,	@DataReferenceID,	@DataSetupType,	@DataGenerateType,	@QueryForValue,	@ReferenceValueFields,	@FixedText, 	 @Note,	@DBUserID,	@PV_DBServerDateTime)
	SELECT * FROM DataCollectionSetup  WHERE DataCollectionSetupID=@DataCollectionSetupID
END

IF(@DBOperation=2)
BEGIN

IF (@DataCollectionSetupID<0) 
	BEGIN
		ROLLBACK
			RAISERROR (N' Selected Data Collection Setup are Invalid Please try again!!',16,1);	
		RETURN
	END
	UPDATE DataCollectionSetup SET	DataReferenceType=@DataReferenceType,	DataReferenceID=@DataReferenceID,	DataSetupType=@DataSetupType,	DataGenerateType=@DataGenerateType,	QueryForValue=@QueryForValue,	ReferenceValueFields=@ReferenceValueFields,	FixedText =@FixedText,	Note=@Note,		DBUserID =@DBUserID,	DBServerDateTime =@PV_DBServerDateTime WHERE DataCollectionSetupID = @DataCollectionSetupID
	SELECT * FROM DataCollectionSetup  WHERE DataCollectionSetupID=@DataCollectionSetupID
END

IF(@DBOperation=3)
BEGIN	
	DELETE FROM DataCollectionSetup WHERE DataReferenceType=@DataReferenceType AND DataReferenceID = @DataReferenceID AND DataCollectionSetupID NOT IN (SELECT * FROM dbo.SplitInToDataSet(@DataCollectionSetupIDs,','))	
END
COMMIT TRAN





GO
/****** Object:  StoredProcedure [dbo].[SP_IUD_DebitCreditSetup]    Script Date: 1/29/2017 2:27:06 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[SP_IUD_DebitCreditSetup]
(	
	@DebitCreditSetupID	as int,
	@IntegrationSetupDetailID	as int,
	@DataCollectionQuery as Varchar(1024), 
	@CompareColumn as Varchar(1024), 
	@AccountHeadType	as smallint,
	@FixedAccountHeadID	as int,
	@AccountHeadSetup as varchar(512),
	@AccountNameSetup as Varchar(512),
	@ReferenceType	as smallint,
	@CurrencySetup as varchar(512),
	@ConversionRateSetup as varchar(512),
	@AmountSetup	as varchar(512),
	@NarrationSetup	as varchar(512),	
	---Reference Field
	
	@IsChequeReferenceCreate	as bit,
	@ChequeReferenceDataSQL	as varchar(1024),
	@ChequeReferenceCompareColumns	as varchar(512),
	@ChequeType as smallint,
	@ChequeSetup as Varchar(512),
	@ChequeReferenceAmountSetup	as varchar(512),
	@ChequeReferenceDescriptionSetup	as varchar(1024),
	@ChequeReferenceDateSetup	as varchar(512),
	---CostCenter Field
	@IsCostCenterCrate	as bit,
	@HasBillReference as bit,
	@HasChequeReference as bit,
	@CostcenterDataSQL	as varchar(1024),
	@CostCenterCompareColumns	as varchar(512),
	@CostcenterSetup	as varchar(512),
	@CostCenterAmountSetup	as varchar(512),
	@CostCenterDescriptionSetup	as varchar(1024),
	@CostCenterDateSetup	as varchar(512),
	---Voucher Bill Field
	@IsVoucherBill as bit,
	@VoucherBillDataSQL 	as varchar(1024),
	@VoucherBillCompareColumns 	as varchar(512),
	@VoucherBillTrType as smallint,
	@VoucherBillSetup 	as varchar(512),
	@VoucherBillAmountSetup 	as varchar(512),
	@VoucherBillDescriptionSetup 	as varchar(1024),
	@VoucherBillDateSetup 	as varchar(512),
	---Inventory Field
	@IsInventoryEffect as bit,
	@InventoryDataSQL 	as varchar(1024),
	@InventoryCompareColumns 	as varchar(512),
	@InventoryProductSetup 	as varchar(512),
	@InventoryWorkingUnitSetup 	as varchar(512),
	@InventoryQtySetup	as varchar(512),
	@InventoryUnitSetup 	as varchar(512),
	@InventoryUnitPriceSetup 	as varchar(512),
	@InventoryDescriptionSetup	as varchar(1024),
	@InventoryDateSetup	as varchar(512),
	@IsDebit as bit,
	@Note	as varchar(512),
	@CostCenterCategorySetUp as Varchar(512),
	@CostCenterNoColumn as Varchar(512),
	@CostCenterRefObjType as smallint,
	@CostCenterRefObjColumn as Varchar(512),
	@VoucherBillNoColumn as Varchar(512),
	@VoucherBillRefObjType as smallint,
	@VoucherBillRefObjColumn as Varchar(512),
	@BillDateSetup as Varchar(512),
	@BillDueDateSetup as Varchar(512),
	@IsOrderReferenceApply as bit,
	@OrderReferenceDataSQL as Varchar(1024),
	@OrderReferenceCompareColumns as Varchar(1024),
	@OrderReferenceSetup as Varchar(1024),
	@OrderAmountSetup as Varchar(1024),
	@OrderRemarkSetup as Varchar(1024),
	@OrderDateSetup as Varchar(1024),
	@DBUserID  as int,
	@DBOperation as smallint,
	@DebitCreditSetupIDs as varchar(512)
	--%n, %n, %s, %s, %n, %n, %s, %s, %n, %s, %s, %s, %s, %b, %s, %s, %n, %s, %s, %s, %s, %b, %b, %b, %s, %s, %s, %s, %s, %s, %b, %s, %s, %s, %s, %s, %s, %s, %b, %s, %s, %s, %s, %s, %s, %s, %s, %s, %b, %s, %s, %s, %n, %s, %s, %n, %s, %s, %s, %b, %s, %s, %s, %s, %s, %s, %n, %n, %s
)
AS
BEGIN TRAN
DECLARE 
@PV_DBServerDateTime as datetime,
@PV_OrderType as smallint
SET @PV_DBServerDateTime=Getdate()	
	
IF(@DBOperation=1)
BEGIN	
	SET @DebitCreditSetupID= (SELECT ISNULL(MAX(DebitCreditSetupID),0)+1 FROM DebitCreditSetup)			
	INSERT INTO DebitCreditSetup	(DebitCreditSetupID,	IntegrationSetupDetailID,	DataCollectionQuery,	CompareColumn, 		AccountHeadType,	FixedAccountHeadID,		AccountHeadSetup,	AccountNameSetup,	ReferenceType,	CurrencySetup,	ConversionRateSetup,	AmountSetup,	NarrationSetup,		IsChequeReferenceCreate,	ChequeReferenceDataSQL,		ChequeReferenceCompareColumns,	ChequeType,		ChequeSetup,	ChequeReferenceAmountSetup,		ChequeReferenceDescriptionSetup,	ChequeReferenceDateSetup,	 IsDebit,	IsCostCenterCreate,	HasBillReference,	HasChequeReference,		CostcenterDataSQL,	CostCenterCompareColumns,	CostcenterSetup,	CostCenterAmountSetup,	CostCenterDescriptionSetup,		CostCenterDateSetup,	IsVoucherBill,		VoucherBillDataSQL,			VoucherBillCompareColumns,			VoucherBillTrType,	VoucherBillSetup,		VoucherBillAmountSetup,			VoucherBillDescriptionSetup,		VoucherBillDateSetup,	IsOrderReferenceApply,		OrderReferenceDataSQL,		OrderReferenceCompareColumns,	OrderReferenceSetup,	OrderAmountSetup,	OrderRemarkSetup,	OrderDateSetup,		IsInventoryEffect,		InventoryDataSQL,	InventoryCompareColumns,	InventoryWorkingUnitSetup,		InventoryProductSetup,		InventoryQtySetup,		InventoryUnitSetup,		InventoryUnitPriceSetup,	InventoryDescriptionSetup,			InventoryDateSetup,			Note,	CostCenterCategorySetup,		CostCenterNoColumn,		CostCenterRefObjType,		CostCenterRefObjColumn,		VoucherBillNoColumn,		VoucherBillRefObjType,		VoucherBillRefObjColumn,	BillDateSetup,		BillDueDateSetup,	DBUserID,	DBServerDateTime)		
						VALUES		(@DebitCreditSetupID,	@IntegrationSetupDetailID,	@DataCollectionQuery,	@CompareColumn,		@AccountHeadType,	@FixedAccountHeadID,	@AccountHeadSetup,	@AccountNameSetup,	@ReferenceType,	@CurrencySetup,	@ConversionRateSetup,	@AmountSetup,	@NarrationSetup,	@IsChequeReferenceCreate,	@ChequeReferenceDataSQL,	@ChequeReferenceCompareColumns,	@ChequeType,	@ChequeSetup,	@ChequeReferenceAmountSetup,	@ChequeReferenceDescriptionSetup,	@ChequeReferenceDateSetup, @IsDebit,	@IsCostCenterCrate,	@HasBillReference,	@HasChequeReference,	@CostcenterDataSQL,	@CostCenterCompareColumns,	@CostcenterSetup,	@CostCenterAmountSetup,	@CostCenterDescriptionSetup,	@CostCenterDateSetup,	@IsVoucherBill,		@VoucherBillDataSQL,		@VoucherBillCompareColumns,			@VoucherBillTrType, @VoucherBillSetup,		@VoucherBillAmountSetup,		@VoucherBillDescriptionSetup,		@VoucherBillDateSetup,	@IsOrderReferenceApply,		@OrderReferenceDataSQL,		@OrderReferenceCompareColumns,	@OrderReferenceSetup,	@OrderAmountSetup,	@OrderRemarkSetup,	@OrderDateSetup,	@IsInventoryEffect,		@InventoryDataSQL,	@InventoryCompareColumns,	@InventoryWorkingUnitSetup,		@InventoryProductSetup,		@InventoryQtySetup,		@InventoryUnitSetup,	@InventoryUnitPriceSetup,	@InventoryDescriptionSetup ,		@InventoryDateSetup ,		@Note,	@CostCenterCategorySetUp,		@CostCenterNoColumn,	@CostCenterRefObjType,		@CostCenterRefObjColumn,	@VoucherBillNoColumn,		@VoucherBillRefObjType,		@VoucherBillRefObjColumn,	@BillDateSetup,		@BillDueDateSetup,	@DBUserID,	@PV_DBServerDateTime)
		SELECT * FROM View_DebitCreditSetup  WHERE DebitCreditSetupID=@DebitCreditSetupID
END

IF(@DBOperation=2)
BEGIN

IF (@DebitCreditSetupID<0) 
	BEGIN
		ROLLBACK
			RAISERROR (N' Selected DebitCredit Setup are Invalid Please try again!!',16,1);	
		RETURN
	END
	UPDATE DebitCreditSetup SET	IntegrationSetupDetailID=@IntegrationSetupDetailID,	DataCollectionQuery=@DataCollectionQuery, CompareColumn=@CompareColumn,  AccountHeadType=@AccountHeadType,	FixedAccountHeadID=@FixedAccountHeadID,		AccountHeadSetup=@AccountHeadSetup,	AccountNameSetup=@AccountNameSetup,	ReferenceType=@ReferenceType,	CurrencySetup=@CurrencySetup, ConversionRateSetup=@ConversionRateSetup, AmountSetup=@AmountSetup,	NarrationSetup=@NarrationSetup, IsChequeReferenceCreate=@IsChequeReferenceCreate,	ChequeReferenceDataSQL=@ChequeReferenceDataSQL,	ChequeReferenceCompareColumns=@ChequeReferenceCompareColumns, ChequeType=@ChequeType, ChequeSetup=@ChequeSetup,	ChequeReferenceAmountSetup=@ChequeReferenceAmountSetup,	ChequeReferenceDescriptionSetup=@ChequeReferenceDescriptionSetup,	ChequeReferenceDateSetup=@ChequeReferenceDateSetup,	IsDebit=@IsDebit,	IsCostCenterCreate=@IsCostCenterCrate, HasBillReference=@HasBillReference, HasChequeReference=@HasChequeReference,	CostcenterDataSQL=@CostcenterDataSQL,	CostCenterCompareColumns=@CostCenterCompareColumns,	CostcenterSetup=@CostcenterSetup,	CostCenterAmountSetup=@CostCenterAmountSetup,	CostCenterDescriptionSetup=@CostCenterDescriptionSetup,		CostCenterDateSetup=@CostCenterDateSetup,	IsVoucherBill = @IsVoucherBill,		VoucherBillDataSQL = @VoucherBillDataSQL,			VoucherBillCompareColumns = @VoucherBillCompareColumns,	 VoucherBillTrType=@VoucherBillTrType,			VoucherBillSetup = @VoucherBillSetup,			VoucherBillAmountSetup =@VoucherBillAmountSetup,			VoucherBillDescriptionSetup = @VoucherBillDescriptionSetup,			VoucherBillDateSetup =@VoucherBillDateSetup ,	 IsOrderReferenceApply=@IsOrderReferenceApply,		OrderReferenceDataSQL=@OrderReferenceDataSQL,		OrderReferenceCompareColumns=@OrderReferenceCompareColumns,	OrderReferenceSetup=@OrderReferenceSetup,	OrderAmountSetup=@OrderAmountSetup,	OrderRemarkSetup=@OrderRemarkSetup,	OrderDateSetup=@OrderDateSetup,	IsInventoryEffect = @IsInventoryEffect,			InventoryDataSQL = @InventoryDataSQL,		InventoryCompareColumns = @InventoryCompareColumns,	 InventoryWorkingUnitSetup=@InventoryWorkingUnitSetup, InventoryProductSetup = @InventoryProductSetup,			InventoryQtySetup = @InventoryQtySetup, InventoryUnitSetup=@InventoryUnitSetup,  InventoryUnitPriceSetup=@InventoryUnitPriceSetup,			InventoryDescriptionSetup = @InventoryDescriptionSetup,			InventoryDateSetup =@InventoryDateSetup ,	Note=@Note,	 CostCenterCategorySetup=@CostCenterCategorySetUp,		CostCenterNoColumn=@CostCenterNoColumn,		CostCenterRefObjType=@CostCenterRefObjType,		CostCenterRefObjColumn=@CostCenterRefObjColumn,		VoucherBillNoColumn=@VoucherBillNoColumn,		VoucherBillRefObjType=@VoucherBillRefObjType,		VoucherBillRefObjColumn=@VoucherBillRefObjColumn, BillDateSetup=@BillDateSetup, BillDueDateSetup=@BillDueDateSetup,	DBUserID =@DBUserID,	DBServerDateTime =@PV_DBServerDateTime WHERE DebitCreditSetupID = @DebitCreditSetupID
	SELECT * FROM View_DebitCreditSetup  WHERE DebitCreditSetupID=@DebitCreditSetupID
END

IF(@DBOperation=3)
BEGIN
	DELETE FROM DataCollectionSetup WHERE DataReferenceType=2 AND DataReferenceID IN (SELECT  DrCr.DebitCreditSetupID FROM DebitCreditSetup AS DrCr WHERE DrCr.IntegrationSetupDetailID=@IntegrationSetupDetailID AND DrCr.DebitCreditSetupID NOT IN (SELECT * FROM dbo.SplitInToDataSet(@DebitCreditSetupIDs,',')))
	DELETE FROM DebitCreditSetup WHERE IntegrationSetupDetailID=@IntegrationSetupDetailID AND DebitCreditSetupID NOT IN (SELECT * FROM dbo.SplitInToDataSet(@DebitCreditSetupIDs,','))
END
COMMIT TRAN










GO
/****** Object:  StoredProcedure [dbo].[SP_IUD_IntegrationSetup]    Script Date: 1/29/2017 2:27:06 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[SP_IUD_IntegrationSetup]
(
	@IntegrationSetupID	as int,
	@SetupNo	as varchar(128),
	@VoucherSetup	as smallint,
	@DataCollectionSQL	as varchar(1024),	
	@KeyColumn	as varchar(1024),	
	@Note	as varchar(512),	
	@Sequence as int,
	@SetupType as smallint,
	@DBUserID as int,
	@DBOperation as smallint
	--%n, %s, %n, %s, %s, %s, %n, %n, %n, %n     
)
AS
BEGIN TRAN

DECLARE 
@PV_DBServerDateTime as datetime
SET @PV_DBServerDateTime=Getdate()	

		
IF(@DBOperation=1)
BEGIN	
	IF EXISTS(SELECT * FROM IntegrationSetup WHERE VoucherSetup=@VoucherSetup)
	BEGIN
		ROLLBACK
			RAISERROR (N'Your Entered Setup Name Already Exists!!',16,1);	
		RETURN
	END	
	SET @IntegrationSetupID=(SELECT ISNULL(MAX(IntegrationSetupID),0)+1 FROM IntegrationSetup)
	SET @SetupNo=RIGHT('0000' + CONVERT(VARCHAR(4), @IntegrationSetupID), 4)
	SET @Sequence = (SELECT ISNULL(IGS.Sequence,0)+1 FROM IntegrationSetup AS IGS WHERE IGS.IntegrationSetupID=(SELECT MAX(IntegrationSetupID) FROM IntegrationSetup))
	INSERT INTO IntegrationSetup	(IntegrationSetupID,	SetupNo,	VoucherSetup,	DataCollectionSQL,KeyColumn,	Note,	Sequence, SetupType,	DBUserID,	DBServerDateTime)			
							VALUES	(@IntegrationSetupID,	@SetupNo,	@VoucherSetup,	@DataCollectionSQL,@KeyColumn,	@Note,	@Sequence, @SetupType,	@DBUserID,	@PV_DBServerDateTime)		
	SELECT * FROM IntegrationSetup WHERE IntegrationSetupID=@IntegrationSetupID
END

IF(@DBOperation=2)
BEGIN
	IF (@IntegrationSetupID<=0) 
	BEGIN
		ROLLBACK
			RAISERROR (N' Selected Integration Setup are Invalid Please try again!!',16,1);	
		RETURN
	END
	IF EXISTS(SELECT * FROM IntegrationSetup WHERE VoucherSetup=@VoucherSetup AND IntegrationSetupID != @IntegrationSetupID)
	BEGIN
		ROLLBACK
			RAISERROR (N'Your Entered Setup Name Already Exists!!',16,1);	
		RETURN
	END	
	UPDATE IntegrationSetup SET	SetupNo=SetupNo,	VoucherSetup=@VoucherSetup,	DataCollectionSQL=@DataCollectionSQL,KeyColumn=@KeyColumn,	Note =@Note, SetupType=@SetupType, DBUserID =@DBUserID,	DBServerDateTime =@PV_DBServerDateTime Where IntegrationSetupID =@IntegrationSetupID
	SELECT * FROM IntegrationSetup WHERE IntegrationSetupID=@IntegrationSetupID
END

IF(@DBOperation=3)
BEGIN
	IF (@IntegrationSetupID<=0) 
	BEGIN
		ROLLBACK
			RAISERROR (N' Selected Integration Setup are Invalid Please try again!!',16,1);	
		RETURN
	END	
	DELETE FROM DataCollectionSetup WHERE DataReferenceType=1 AND DataReferenceID IN (SELECT ISD.IntegrationSetupDetailID FROM IntegrationSetupDetail AS ISD WHERE ISD.IntegrationSetupID =@IntegrationSetupID)
	DELETE FROM DataCollectionSetup WHERE DataReferenceType=2 AND DataReferenceID IN (SELECT DCS.DebitCreditSetupID FROM  DebitCreditSetup AS DCS WHERE DCS.IntegrationSetupDetailID IN (SELECT ISD.IntegrationSetupDetailID FROM IntegrationSetupDetail AS ISD WHERE ISD.IntegrationSetupID =@IntegrationSetupID))
	DELETE FROM DebitCreditSetup WHERE IntegrationSetupDetailID IN (SELECT ISD.IntegrationSetupDetailID FROM IntegrationSetupDetail AS ISD WHERE ISD.IntegrationSetupID =@IntegrationSetupID)
	DELETE FROM IntegrationSetupDetail WHERE IntegrationSetupID =@IntegrationSetupID
	DELETE FROM IntegrationSetup WHERE IntegrationSetupID=@IntegrationSetupID
END
COMMIT TRAN




GO
/****** Object:  StoredProcedure [dbo].[SP_IUD_IntegrationSetupDetail]    Script Date: 1/29/2017 2:27:06 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[SP_IUD_IntegrationSetupDetail]
(	
	@IntegrationSetupDetailID	as int,
	@IntegrationSetupID	as int,
	@VoucherTypeID	as int,
	@BusinessUnitSetup as Varchar(512),
	@VoucherDateSetup	as varchar(512),	
	@NarrationSetup	as varchar(1024),
	@ReferenceNoteSetup	as varchar(1024),
	@UpdateTable as Varchar(512),
	@KeyColumn as Varchar(512),
	@Note	as varchar(512),
	@DBUserID  as int,
	@DBOperation as smallint,
	@IntegrationSetupDetailIDs as varchar(512)
	--%n, %n, %n, %s, %s, %s, %s, %s, %s, %s, %n, %n, %s
)
AS
BEGIN TRAN
DECLARE 
@PV_DBServerDateTime as datetime,
@PV_OrderType as smallint
SET @PV_DBServerDateTime=Getdate()	
	
IF(@DBOperation=1)
BEGIN	
	SET @IntegrationSetupDetailID= (SELECT ISNULL(MAX(IntegrationSetupDetailID),0)+1 FROM IntegrationSetupDetail)	
	SET @KeyColumn=(SELECT column_name FROM INFORMATION_SCHEMA.KEY_COLUMN_USAGE WHERE OBJECTPROPERTY(OBJECT_ID(constraint_name), 'IsPrimaryKey') = 1 AND table_name = @UpdateTable)
	INSERT INTO IntegrationSetupDetail	(IntegrationSetupDetailID,	IntegrationSetupID,		VoucherTypeID,	BusinessUnitSetup,	VoucherDateSetup,	NarrationSetup,		ReferenceNoteSetup,		UpdateTable,	KeyColumn,	Note,	DBUserID,	DBServerDateTime)		
							VALUES		(@IntegrationSetupDetailID,	@IntegrationSetupID,	@VoucherTypeID,	@BusinessUnitSetup, @VoucherDateSetup,	@NarrationSetup,	@ReferenceNoteSetup,	@UpdateTable,	@KeyColumn,	@Note,	@DBUserID,	@PV_DBServerDateTime)
		SELECT * FROM View_IntegrationSetupDetail  WHERE IntegrationSetupDetailID=@IntegrationSetupDetailID
END

IF(@DBOperation=2)
BEGIN

IF (@IntegrationSetupDetailID<0) 
	BEGIN
		ROLLBACK
			RAISERROR (N' Selected Integration Setup Detail are Invalid Please try again!!',16,1);	
		RETURN
	END
	SET @KeyColumn=(SELECT column_name FROM INFORMATION_SCHEMA.KEY_COLUMN_USAGE WHERE OBJECTPROPERTY(OBJECT_ID(constraint_name), 'IsPrimaryKey') = 1 AND table_name = @UpdateTable)
	UPDATE IntegrationSetupDetail SET	IntegrationSetupID=@IntegrationSetupID,		VoucherTypeID=@VoucherTypeID,	BusinessUnitSetup=@BusinessUnitSetup,	VoucherDateSetup=@VoucherDateSetup,		NarrationSetup=@NarrationSetup,		ReferenceNoteSetup=@ReferenceNoteSetup,		UpdateTable=@UpdateTable, KeyColumn=@KeyColumn,  Note=@Note,		DBUserID =@DBUserID,	DBServerDateTime =@PV_DBServerDateTime WHERE IntegrationSetupDetailID = @IntegrationSetupDetailID
	SELECT * FROM View_IntegrationSetupDetail  WHERE IntegrationSetupDetailID=@IntegrationSetupDetailID
END

IF(@DBOperation=3)
BEGIN
	DELETE FROM DataCollectionSetup WHERE DataReferenceType=2 AND DataReferenceID IN (SELECT DrCr.DebitCreditSetupID FROM DebitCreditSetup AS DrCr WHERE DrCr.IntegrationSetupDetailID IN (SELECT INTD.IntegrationSetupDetailID FROM IntegrationSetupDetail AS INTD WHERE INTD.IntegrationSetupID=@IntegrationSetupID AND INTD.IntegrationSetupDetailID NOT IN (SELECT * FROM dbo.SplitInToDataSet(@IntegrationSetupDetailIDs,','))))
	DELETE FROM DebitCreditSetup WHERE IntegrationSetupDetailID IN (SELECT INTD.IntegrationSetupDetailID FROM IntegrationSetupDetail AS INTD WHERE INTD.IntegrationSetupID=@IntegrationSetupID AND INTD.IntegrationSetupDetailID NOT IN (SELECT * FROM dbo.SplitInToDataSet(@IntegrationSetupDetailIDs,',')))
	DELETE FROM DataCollectionSetup WHERE DataReferenceType=1 AND DataReferenceID IN (SELECT INTD.IntegrationSetupDetailID FROM IntegrationSetupDetail AS INTD WHERE INTD.IntegrationSetupID=@IntegrationSetupID AND INTD.IntegrationSetupDetailID NOT IN (SELECT * FROM dbo.SplitInToDataSet(@IntegrationSetupDetailIDs,',')))
	DELETE FROM IntegrationSetupDetail WHERE IntegrationSetupID=@IntegrationSetupID AND IntegrationSetupDetailID NOT IN (SELECT * FROM dbo.SplitInToDataSet(@IntegrationSetupDetailIDs,','))	
END
COMMIT TRAN






GO
/****** Object:  StoredProcedure [dbo].[SP_IUD_VoucherMapping]    Script Date: 1/29/2017 2:27:06 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[SP_IUD_VoucherMapping]
(
	@VoucherMappingID	as int,
	@TableName	as varchar(512),
	@PKColumnName	as varchar(512),
	@PKValue	as int,
 	@VoucherSetup as int,
	@VoucherID	as int,
	@DBUserID as int,
	@DBOperation as smallint
	--%n, %s, %s, %n, %n, %n, %n, %n
)
AS
BEGIN TRAN
DECLARE 
@PV_DBServerDateTime as datetime
SET @PV_DBServerDateTime=Getdate()	
		
IF(@DBOperation=1)
BEGIN	
	IF EXISTS(SELECT * FROM VoucherMapping WHERE TableName=@TableName AND PKColumnName=@PKColumnName AND PKValue=@PKValue AND VoucherSetup=@VoucherSetup and VoucherID=@VoucherID)
	BEGIN
		ROLLBACK
			RAISERROR (N'Your Entered Voucher Already Exists!!~',16,1);	
		RETURN
	END	
	IF(ISNULL(@PKValue,0)<=0)
	BEGIN
		ROLLBACK
			RAISERROR (N'Invalid PKValue. Please try again!!~',16,1);	
		RETURN
	END
	IF(ISNULL(@VoucherID,0)<=0)
	BEGIN
		ROLLBACK
			RAISERROR (N'Invalid Voucher For Voucher Mapping. Please try again!!~',16,1);	
		RETURN
	END
	SET @VoucherMappingID=(SELECT ISNULL(MAX(VoucherMappingID),0)+1 FROM VoucherMapping)	
	INSERT INTO VoucherMapping	(VoucherMappingID,	TableName,	PKColumnName,	PKValue,	VoucherSetup,	VoucherID,	DBUserID,	DBServerDateTime)			
						VALUES	(@VoucherMappingID,	@TableName,	@PKColumnName,	@PKValue,	@VoucherSetup	,@VoucherID,	@DBUserID,	@PV_DBServerDateTime)		
	SELECT * FROM VoucherMapping WHERE VoucherMappingID=@VoucherMappingID
END

IF(@DBOperation=2)
BEGIN
	IF (@VoucherMappingID<=0) 
	BEGIN
		ROLLBACK
			RAISERROR (N' Selected Integration Setup are Invalid Please try again!!~',16,1);	
		RETURN
	END	
	IF(ISNULL(@PKValue,0)<=0)
	BEGIN
		ROLLBACK
			RAISERROR (N'Invalid PKValue. Please try again!!~',16,1);	
		RETURN
	END
	IF(ISNULL(@VoucherID,0)<=0)
	BEGIN
		ROLLBACK
			RAISERROR (N'Invalid Voucher For Voucher Mapping. Please try again!!~',16,1);	
		RETURN
	END
	UPDATE VoucherMapping SET	TableName=@TableName,	PKColumnName=@PKColumnName,	PKValue=@PKValue,VoucherSetup=@VoucherSetup,VoucherID=@VoucherID,	 DBUserID =@DBUserID,	DBServerDateTime =@PV_DBServerDateTime Where VoucherMappingID =@VoucherMappingID
	SELECT * FROM VoucherMapping WHERE VoucherMappingID=@VoucherMappingID
END

IF(@DBOperation=3)
BEGIN
	IF (@VoucherMappingID<=0) 
	BEGIN
		ROLLBACK
			RAISERROR (N' Selected Integration Setup are Invalid Please try again!!~',16,1);	
		RETURN
	END		
	DELETE FROM VoucherMapping WHERE VoucherMappingID=@VoucherMappingID
END
COMMIT TRAN








GO
/****** Object:  Table [dbo].[DataCollectionSetup]    Script Date: 1/29/2017 2:27:06 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[DataCollectionSetup](
	[DataCollectionSetupID] [int] NOT NULL,
	[DataReferenceType] [smallint] NULL,
	[DataReferenceID] [int] NULL,
	[DataSetupType] [smallint] NULL,
	[DataGenerateType] [smallint] NULL,
	[QueryForValue] [varchar](max) NULL,
	[ReferenceValueFields] [varchar](512) NULL,
	[FixedText] [varchar](512) NULL,
	[Note] [varchar](512) NULL,
	[DBUserID] [int] NULL,
	[DBServerDateTime] [datetime] NULL,
 CONSTRAINT [PK_DataCollectionSetup] PRIMARY KEY CLUSTERED 
(
	[DataCollectionSetupID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[DebitCreditSetup]    Script Date: 1/29/2017 2:27:06 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[DebitCreditSetup](
	[DebitCreditSetupID] [int] NOT NULL,
	[IntegrationSetupDetailID] [int] NULL,
	[DataCollectionQuery] [varchar](1024) NULL,
	[CompareColumn] [varchar](1024) NULL,
	[AccountHeadType] [smallint] NULL,
	[FixedAccountHeadID] [int] NULL,
	[AccountHeadSetup] [varchar](512) NULL,
	[ReferenceType] [smallint] NULL,
	[AccountNameSetup] [varchar](512) NULL,
	[CurrencySetup] [varchar](512) NULL,
	[ConversionRateSetup] [varchar](512) NULL,
	[AmountSetup] [varchar](512) NULL,
	[NarrationSetup] [varchar](512) NULL,
	[IsChequeReferenceCreate] [bit] NULL,
	[ChequeReferenceDataSQL] [varchar](1024) NULL,
	[ChequeReferenceCompareColumns] [varchar](512) NULL,
	[ChequeType] [smallint] NULL,
	[ChequeSetup] [varchar](512) NULL,
	[ChequeReferenceAmountSetup] [varchar](512) NULL,
	[ChequeReferenceDescriptionSetup] [varchar](1024) NULL,
	[ChequeReferenceDateSetup] [varchar](512) NULL,
	[IsDebit] [bit] NULL,
	[IsCostCenterCreate] [bit] NULL,
	[HasBillReference] [bit] NULL,
	[CostcenterDataSQL] [varchar](1024) NULL,
	[CostCenterCompareColumns] [varchar](512) NULL,
	[CostcenterSetup] [varchar](512) NULL,
	[CostCenterAmountSetup] [varchar](512) NULL,
	[CostCenterDescriptionSetup] [varchar](1024) NULL,
	[CostCenterDateSetup] [varchar](512) NULL,
	[IsVoucherBill] [bit] NULL,
	[VoucherBillDataSQL] [varchar](1024) NULL,
	[VoucherBillCompareColumns] [varchar](512) NULL,
	[VoucherBillTrType] [smallint] NULL,
	[VoucherBillSetup] [varchar](512) NULL,
	[VoucherBillAmountSetup] [varchar](512) NULL,
	[VoucherBillDescriptionSetup] [varchar](512) NULL,
	[VoucherBillDateSetup] [varchar](512) NULL,
	[IsOrderReferenceApply] [bit] NULL,
	[OrderReferenceDataSQL] [varchar](1024) NULL,
	[OrderReferenceCompareColumns] [varchar](1024) NULL,
	[OrderReferenceSetup] [varchar](1024) NULL,
	[OrderAmountSetup] [varchar](1024) NULL,
	[OrderRemarkSetup] [varchar](1024) NULL,
	[OrderDateSetup] [varchar](1024) NULL,
	[IsInventoryEffect] [bit] NULL,
	[InventoryDataSQL] [varchar](1024) NULL,
	[InventoryCompareColumns] [varchar](512) NULL,
	[InventoryWorkingUnitSetup] [varchar](512) NULL,
	[InventoryProductSetup] [varchar](512) NULL,
	[InventoryQtySetup] [varchar](512) NULL,
	[InventoryUnitSetup] [varchar](512) NULL,
	[InventoryUnitPriceSetup] [varchar](512) NULL,
	[InventoryDescriptionSetup] [varchar](512) NULL,
	[InventoryDateSetup] [varchar](512) NULL,
	[Note] [varchar](512) NULL,
	[CostCenterCategorySetUp] [varchar](512) NULL,
	[CostCenterNoColumn] [varchar](512) NULL,
	[CostCenterRefObjType] [smallint] NULL,
	[CostCenterRefObjColumn] [varchar](512) NULL,
	[VoucherBillNoColumn] [varchar](512) NULL,
	[VoucherBillRefObjType] [smallint] NULL,
	[VoucherBillRefObjColumn] [varchar](512) NULL,
	[BillDateSetup] [varchar](512) NULL,
	[BillDueDateSetup] [varchar](512) NULL,
	[DBUserID] [int] NULL,
	[DBServerDateTime] [datetime] NULL,
	[HasChequeReference] [bit] NULL,
 CONSTRAINT [PK_DebitCreditSetup] PRIMARY KEY CLUSTERED 
(
	[DebitCreditSetupID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[IntegrationSetup]    Script Date: 1/29/2017 2:27:06 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[IntegrationSetup](
	[IntegrationSetupID] [int] NOT NULL,
	[SetupNo] [varchar](128) NULL,
	[VoucherSetup] [smallint] NULL,
	[DataCollectionSQL] [varchar](1024) NULL,
	[KeyColumn] [varchar](1024) NULL,
	[Note] [varchar](512) NULL,
	[Sequence] [int] NULL,
	[SetupType] [smallint] NULL,
	[DBUserID] [int] NULL,
	[DBServerDateTime] [datetime] NULL,
 CONSTRAINT [PK_IntegrationSetup] PRIMARY KEY CLUSTERED 
(
	[IntegrationSetupID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[IntegrationSetupDetail]    Script Date: 1/29/2017 2:27:06 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[IntegrationSetupDetail](
	[IntegrationSetupDetailID] [int] NOT NULL,
	[IntegrationSetupID] [int] NULL,
	[VoucherTypeID] [int] NULL,
	[BusinessUnitSetup] [varchar](512) NULL,
	[VoucherDateSetup] [varchar](512) NULL,
	[NarrationSetup] [varchar](1024) NULL,
	[ReferenceNoteSetup] [varchar](1024) NULL,
	[UpdateTable] [varchar](512) NULL,
	[KeyColumn] [varchar](512) NULL,
	[Note] [varchar](512) NULL,
	[DBUserID] [int] NULL,
	[DBServerDateTime] [datetime] NULL,
 CONSTRAINT [PK_IntegrationSetupDetail] PRIMARY KEY CLUSTERED 
(
	[IntegrationSetupDetailID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[VoucherMapping]    Script Date: 1/29/2017 2:27:06 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[VoucherMapping](
	[VoucherMappingID] [int] NOT NULL,
	[TableName] [varchar](512) NOT NULL,
	[PKColumnName] [varchar](512) NOT NULL,
	[PKValue] [int] NOT NULL,
	[VoucherSetup] [smallint] NOT NULL CONSTRAINT [DF_VoucherMapping_OperationName]  DEFAULT (''),
	[VoucherID] [int] NOT NULL,
	[DBUserID] [int] NOT NULL,
	[DBServerDateTime] [datetime] NOT NULL,
 CONSTRAINT [PK_VoucherMapping] PRIMARY KEY CLUSTERED 
(
	[VoucherMappingID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  View [dbo].[View_ChequeRequisitionDetail]    Script Date: 1/29/2017 2:27:06 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE VIEW [dbo].[View_ChequeRequisitionDetail]
AS
SELECT	CRD.ChequeRequisitionDetailID, 
		CRD.ChequeRequisitionID, 
		CRD.VoucherBillID, 		
		CRD.Amount, 
		CRD.Remarks,		
		VB.BillNo,
		VB.BillDate,
		VB.AccountHeadID,
		VB.SubLedgerID,
		VB.AccountHeadName,
		VB.Amount AS BillAmount,
		VB.RemainningBalance

FROM	ChequeRequisitionDetail  AS CRD
LEFT OUTER JOIN	View_VoucherBill AS VB ON CRD.VoucherBillID = VB.VoucherBillID
GO
/****** Object:  View [dbo].[View_DebitCreditSetup]    Script Date: 1/29/2017 2:27:06 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


CREATE VIEW [dbo].[View_DebitCreditSetup]
AS
SELECT		DebitCreditSetup.DebitCreditSetupID, 
			DebitCreditSetup.IntegrationSetupDetailID, 
			DebitCreditSetup.DataCollectionQuery, 
			DebitCreditSetup.CompareColumn, 
			DebitCreditSetup.AccountHeadType, 
            DebitCreditSetup.FixedAccountHeadID, 
            DebitCreditSetup.AccountHeadSetup, 
			DebitCreditSetup.AccountNameSetup,
            DebitCreditSetup.ReferenceType, 
			DebitCreditSetup.CurrencySetup, 
			DebitCreditSetup.ConversionRateSetup, 
            DebitCreditSetup.AmountSetup, 
            DebitCreditSetup.NarrationSetup, 			
            DebitCreditSetup.IsChequeReferenceCreate, 
            DebitCreditSetup.ChequeReferenceDataSQL, 
			DebitCreditSetup.ChequeReferenceCompareColumns, 
			DebitCreditSetup.ChequeReferenceAmountSetup, 
			DebitCreditSetup.ChequeType, 
			DebitCreditSetup.ChequeSetup, 
			DebitCreditSetup.ChequeReferenceDescriptionSetup, 
			DebitCreditSetup.ChequeReferenceDateSetup, 
			DebitCreditSetup.IsCostCenterCreate, 
			DebitCreditSetup.HasBillReference,
			DebitCreditSetup.HasChequeReference,
			DebitCreditSetup.CostcenterDataSQL, 
            DebitCreditSetup.CostCenterCompareColumns, 
            DebitCreditSetup.CostcenterSetup, 
            DebitCreditSetup.CostCenterAmountSetup, 
            DebitCreditSetup.CostCenterDescriptionSetup, 
            DebitCreditSetup.CostCenterDateSetup, 
			DebitCreditSetup.IsVoucherBill,
			DebitCreditSetup.VoucherBillDataSQL,
			DebitCreditSetup.VoucherBillCompareColumns,
			DebitCreditSetup.VoucherBillTrType,
			DebitCreditSetup.VoucherBillSetup,
			DebitCreditSetup.VoucherBillAmountSetup,
			DebitCreditSetup.VoucherBillDescriptionSetup,
			DebitCreditSetup.VoucherBillDateSetup,
			DebitCreditSetup.IsOrderReferenceApply,
			DebitCreditSetup.OrderReferenceDataSQL,
			DebitCreditSetup.OrderReferenceCompareColumns,
			DebitCreditSetup.OrderReferenceSetup,
			DebitCreditSetup.OrderAmountSetup,
			DebitCreditSetup.OrderRemarkSetup,
			DebitCreditSetup.OrderDateSetup,
			DebitCreditSetup.IsInventoryEffect,
			DebitCreditSetup.InventoryDataSQL,
			DebitCreditSetup.InventoryCompareColumns,
			DebitCreditSetup.InventoryWorkingUnitSetup,
			DebitCreditSetup.InventoryProductSetup,
			DebitCreditSetup.InventoryQtySetup,
			DebitCreditSetup.InventoryUnitSetup,
			DebitCreditSetup.InventoryUnitPriceSetup,
			DebitCreditSetup.InventoryDescriptionSetup,
			DebitCreditSetup.InventoryDateSetup,
            DebitCreditSetup.Note, 
            DebitCreditSetup.IsDebit,
			DebitCreditSetup.CostCenterCategorySetup,
			DebitCreditSetup.CostCenterNoColumn,
			DebitCreditSetup.CostCenterRefObjType,
			DebitCreditSetup.CostCenterRefObjColumn,
			DebitCreditSetup.VoucherBillNoColumn,
			DebitCreditSetup.VoucherBillRefObjType,
			DebitCreditSetup.BillDateSetup,
			DebitCreditSetup.BillDueDateSetup,
			DebitCreditSetup.VoucherBillRefObjColumn,
            ChartsOfAccount.AccountCode, 
            ChartsOfAccount.AccountHeadName
            
FROM			DebitCreditSetup 
LEFT OUTER JOIN	ChartsOfAccount ON DebitCreditSetup.FixedAccountHeadID = ChartsOfAccount.AccountHeadID





















GO
/****** Object:  View [dbo].[View_IntegrationSetupDetail]    Script Date: 1/29/2017 2:27:06 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


CREATE VIEW [dbo].[View_IntegrationSetupDetail]
AS
SELECT		IntegrationSetupDetail.IntegrationSetupDetailID, 
			IntegrationSetupDetail.IntegrationSetupID, 
			IntegrationSetupDetail.VoucherTypeID, 
			IntegrationSetupDetail.BusinessUnitSetup,
            IntegrationSetupDetail.VoucherDateSetup,             
            IntegrationSetupDetail.NarrationSetup, 
            IntegrationSetupDetail.ReferenceNoteSetup, 
            IntegrationSetupDetail.UpdateTable, 
            IntegrationSetupDetail.KeyColumn, 
            IntegrationSetupDetail.Note, 
            VoucherType.VoucherName
            
FROM        IntegrationSetupDetail 
INNER JOIN	VoucherType ON IntegrationSetupDetail.VoucherTypeID = VoucherType.VoucherTypeID








GO
INSERT [dbo].[DataCollectionSetup] ([DataCollectionSetupID], [DataReferenceType], [DataReferenceID], [DataSetupType], [DataGenerateType], [QueryForValue], [ReferenceValueFields], [FixedText], [Note], [DBUserID], [DBServerDateTime]) VALUES (1, 2, 1, 30, 2, N'SELECT ''Payable To Supplier Local'' AS DebitAccountHead FROM View_ChequeRequisition WHERE ChequeRequisitionID=%n', N'ChequeRequisitionID', NULL, NULL, -9, CAST(N'2017-01-29 14:04:08.713' AS DateTime))
GO
INSERT [dbo].[DataCollectionSetup] ([DataCollectionSetupID], [DataReferenceType], [DataReferenceID], [DataSetupType], [DataGenerateType], [QueryForValue], [ReferenceValueFields], [FixedText], [Note], [DBUserID], [DBServerDateTime]) VALUES (2, 2, 1, 2, 2, N'SELECT BaseCurrencyID FROM Company WHERE CompanyID=1', N'ChequeRequisitionID', NULL, NULL, -9, CAST(N'2017-01-29 14:04:08.723' AS DateTime))
GO
INSERT [dbo].[DataCollectionSetup] ([DataCollectionSetupID], [DataReferenceType], [DataReferenceID], [DataSetupType], [DataGenerateType], [QueryForValue], [ReferenceValueFields], [FixedText], [Note], [DBUserID], [DBServerDateTime]) VALUES (3, 2, 1, 3, 2, N'SELECT 1 AS CRate FROM ChequeRequisition WHERE ChequeRequisitionID=%n', N'ChequeRequisitionID', NULL, NULL, -9, CAST(N'2017-01-29 14:04:08.737' AS DateTime))
GO
INSERT [dbo].[DataCollectionSetup] ([DataCollectionSetupID], [DataReferenceType], [DataReferenceID], [DataSetupType], [DataGenerateType], [QueryForValue], [ReferenceValueFields], [FixedText], [Note], [DBUserID], [DBServerDateTime]) VALUES (4, 2, 1, 7, 2, N'SELECT ISNULL(SUM(HH.Amount),0) FROM View_ChequeRequisitionDetail AS HH WHERE HH.ChequeRequisitionID =%n AND HH.AccountHeadID=%n', N'ChequeRequisitionID,AccountHeadID', NULL, NULL, -9, CAST(N'2017-01-29 14:04:08.747' AS DateTime))
GO
INSERT [dbo].[DataCollectionSetup] ([DataCollectionSetupID], [DataReferenceType], [DataReferenceID], [DataSetupType], [DataGenerateType], [QueryForValue], [ReferenceValueFields], [FixedText], [Note], [DBUserID], [DBServerDateTime]) VALUES (6, 2, 2, 30, 2, N'SELECT ''Bank Cleare Account'' AS BankCleareAccount FROM View_ChequeRequisition WHERE ChequeRequisitionID =%n', N'ChequeRequisitionID', NULL, NULL, -9, CAST(N'2017-01-29 14:04:09.157' AS DateTime))
GO
INSERT [dbo].[DataCollectionSetup] ([DataCollectionSetupID], [DataReferenceType], [DataReferenceID], [DataSetupType], [DataGenerateType], [QueryForValue], [ReferenceValueFields], [FixedText], [Note], [DBUserID], [DBServerDateTime]) VALUES (7, 2, 2, 2, 2, N'SELECT BaseCurrencyID FROM Company WHERE CompanyID=1', N'ChequeRequisitionID', NULL, NULL, -9, CAST(N'2017-01-29 14:04:09.177' AS DateTime))
GO
INSERT [dbo].[DataCollectionSetup] ([DataCollectionSetupID], [DataReferenceType], [DataReferenceID], [DataSetupType], [DataGenerateType], [QueryForValue], [ReferenceValueFields], [FixedText], [Note], [DBUserID], [DBServerDateTime]) VALUES (8, 2, 2, 3, 2, N'SELECT 1 AS CRate FROM ChequeRequisition WHERE ChequeRequisitionID =%n', N'ChequeRequisitionID', NULL, NULL, -9, CAST(N'2017-01-29 14:04:09.197' AS DateTime))
GO
INSERT [dbo].[DataCollectionSetup] ([DataCollectionSetupID], [DataReferenceType], [DataReferenceID], [DataSetupType], [DataGenerateType], [QueryForValue], [ReferenceValueFields], [FixedText], [Note], [DBUserID], [DBServerDateTime]) VALUES (9, 2, 2, 7, 2, N'SELECT ISNULL(HH.ChequeAmount,0) FROM ChequeRequisition AS HH WHERE HH.ChequeRequisitionID=%n', N'ChequeRequisitionID', NULL, NULL, -9, CAST(N'2017-01-29 14:04:09.207' AS DateTime))
GO
INSERT [dbo].[DataCollectionSetup] ([DataCollectionSetupID], [DataReferenceType], [DataReferenceID], [DataSetupType], [DataGenerateType], [QueryForValue], [ReferenceValueFields], [FixedText], [Note], [DBUserID], [DBServerDateTime]) VALUES (10, 2, 2, 8, 2, N'SELECT HH.Remarks FROM ChequeRequisition AS HH WHERE HH.ChequeRequisitionID=%n', N'ChequeRequisitionID', NULL, NULL, -9, CAST(N'2017-01-29 14:04:09.217' AS DateTime))
GO
INSERT [dbo].[DataCollectionSetup] ([DataCollectionSetupID], [DataReferenceType], [DataReferenceID], [DataSetupType], [DataGenerateType], [QueryForValue], [ReferenceValueFields], [FixedText], [Note], [DBUserID], [DBServerDateTime]) VALUES (11, 1, 1, 36, 2, N'SELECT HH.BUID FROM ChequeRequisition AS HH WHERE HH.ChequeRequisitionID=%n', N'ChequeRequisitionID', NULL, NULL, -9, CAST(N'2017-01-29 14:04:09.430' AS DateTime))
GO
INSERT [dbo].[DataCollectionSetup] ([DataCollectionSetupID], [DataReferenceType], [DataReferenceID], [DataSetupType], [DataGenerateType], [QueryForValue], [ReferenceValueFields], [FixedText], [Note], [DBUserID], [DBServerDateTime]) VALUES (12, 1, 1, 1, 2, N'SELECT HH.ChequeDate FROM ChequeRequisition AS HH WHERE HH.ChequeRequisitionID=%n', N'ChequeRequisitionID', NULL, NULL, -9, CAST(N'2017-01-29 14:04:09.443' AS DateTime))
GO
INSERT [dbo].[DataCollectionSetup] ([DataCollectionSetupID], [DataReferenceType], [DataReferenceID], [DataSetupType], [DataGenerateType], [QueryForValue], [ReferenceValueFields], [FixedText], [Note], [DBUserID], [DBServerDateTime]) VALUES (14, 1, 1, 5, 3, NULL, NULL, N'N/A', NULL, -9, CAST(N'2017-01-29 14:04:09.480' AS DateTime))
GO
INSERT [dbo].[DataCollectionSetup] ([DataCollectionSetupID], [DataReferenceType], [DataReferenceID], [DataSetupType], [DataGenerateType], [QueryForValue], [ReferenceValueFields], [FixedText], [Note], [DBUserID], [DBServerDateTime]) VALUES (15, 1, 1, 4, 2, N'SELECT HH.Remarks FROM ChequeRequisition AS HH WHERE HH.ChequeRequisitionID=%n', N'ChequeRequisitionID', NULL, NULL, -9, CAST(N'2017-01-29 14:04:09.470' AS DateTime))
GO
INSERT [dbo].[DataCollectionSetup] ([DataCollectionSetupID], [DataReferenceType], [DataReferenceID], [DataSetupType], [DataGenerateType], [QueryForValue], [ReferenceValueFields], [FixedText], [Note], [DBUserID], [DBServerDateTime]) VALUES (16, 2, 1, 12, 2, N'SELECT HH.ACCostCenterID FROM ACCostCenter AS HH WHERE HH.ACCostCenterID=%n', N'SubLedgerID', NULL, NULL, -9, CAST(N'2017-01-29 14:04:08.757' AS DateTime))
GO
INSERT [dbo].[DataCollectionSetup] ([DataCollectionSetupID], [DataReferenceType], [DataReferenceID], [DataSetupType], [DataGenerateType], [QueryForValue], [ReferenceValueFields], [FixedText], [Note], [DBUserID], [DBServerDateTime]) VALUES (17, 2, 1, 13, 2, N'SELECT ISNULL(SUM(HH.Amount),0) FROM View_ChequeRequisitionDetail AS HH WHERE HH.ChequeRequisitionID=%n AND HH.AccountHeadID=%n AND HH.SubLedgerID=%n', N'ChequeRequisitionID,AccountHeadID,SubLedgerID', NULL, NULL, -9, CAST(N'2017-01-29 14:04:08.780' AS DateTime))
GO
INSERT [dbo].[DataCollectionSetup] ([DataCollectionSetupID], [DataReferenceType], [DataReferenceID], [DataSetupType], [DataGenerateType], [QueryForValue], [ReferenceValueFields], [FixedText], [Note], [DBUserID], [DBServerDateTime]) VALUES (19, 2, 1, 15, 2, N'SELECT HH.ChequeDate FROM ChequeRequisition AS HH WHERE HH.ChequeRequisitionID=%n', N'ChequeRequisitionID', NULL, NULL, -9, CAST(N'2017-01-29 14:04:08.793' AS DateTime))
GO
INSERT [dbo].[DataCollectionSetup] ([DataCollectionSetupID], [DataReferenceType], [DataReferenceID], [DataSetupType], [DataGenerateType], [QueryForValue], [ReferenceValueFields], [FixedText], [Note], [DBUserID], [DBServerDateTime]) VALUES (20, 2, 1, 27, 2, N'SELECT 15 AS CCCAtegory FROM ACCostCenter WHERE ACCostCenterID=1', N'ChequeRequisitionID', NULL, NULL, -9, CAST(N'2017-01-29 14:04:08.807' AS DateTime))
GO
INSERT [dbo].[DataCollectionSetup] ([DataCollectionSetupID], [DataReferenceType], [DataReferenceID], [DataSetupType], [DataGenerateType], [QueryForValue], [ReferenceValueFields], [FixedText], [Note], [DBUserID], [DBServerDateTime]) VALUES (21, 2, 1, 14, 2, N'SELECT HH.Remarks FROM ChequeRequisition AS HH WHERE HH.ChequeRequisitionID=%n', N'ChequeRequisitionID', NULL, NULL, -9, CAST(N'2017-01-29 14:04:08.817' AS DateTime))
GO
INSERT [dbo].[DataCollectionSetup] ([DataCollectionSetupID], [DataReferenceType], [DataReferenceID], [DataSetupType], [DataGenerateType], [QueryForValue], [ReferenceValueFields], [FixedText], [Note], [DBUserID], [DBServerDateTime]) VALUES (22, 2, 1, 16, 2, N'SELECT HH.VoucherBillID FROM ChequeRequisitionDetail AS HH WHERE HH.ChequeRequisitionDetailID=%n', N'ChequeRequisitionDetailID', NULL, NULL, -9, CAST(N'2017-01-29 14:04:08.843' AS DateTime))
GO
INSERT [dbo].[DataCollectionSetup] ([DataCollectionSetupID], [DataReferenceType], [DataReferenceID], [DataSetupType], [DataGenerateType], [QueryForValue], [ReferenceValueFields], [FixedText], [Note], [DBUserID], [DBServerDateTime]) VALUES (23, 2, 1, 17, 2, N'SELECT HH.Amount FROM ChequeRequisitionDetail AS HH WHERE HH.ChequeRequisitionDetailID=%n', N'ChequeRequisitionDetailID', NULL, NULL, -9, CAST(N'2017-01-29 14:04:08.853' AS DateTime))
GO
INSERT [dbo].[DataCollectionSetup] ([DataCollectionSetupID], [DataReferenceType], [DataReferenceID], [DataSetupType], [DataGenerateType], [QueryForValue], [ReferenceValueFields], [FixedText], [Note], [DBUserID], [DBServerDateTime]) VALUES (24, 2, 1, 18, 2, N'SELECT HH.Remarks FROM ChequeRequisitionDetail AS HH WHERE HH.ChequeRequisitionDetailID=%n', N'ChequeRequisitionDetailID', NULL, NULL, -9, CAST(N'2017-01-29 14:04:08.867' AS DateTime))
GO
INSERT [dbo].[DataCollectionSetup] ([DataCollectionSetupID], [DataReferenceType], [DataReferenceID], [DataSetupType], [DataGenerateType], [QueryForValue], [ReferenceValueFields], [FixedText], [Note], [DBUserID], [DBServerDateTime]) VALUES (25, 2, 1, 19, 2, N'SELECT HH.ChequeDate FROM ChequeRequisition AS HH WHERE HH.ChequeRequisitionID=%n', N'ChequeRequisitionID', NULL, NULL, -9, CAST(N'2017-01-29 14:04:08.880' AS DateTime))
GO
INSERT [dbo].[DataCollectionSetup] ([DataCollectionSetupID], [DataReferenceType], [DataReferenceID], [DataSetupType], [DataGenerateType], [QueryForValue], [ReferenceValueFields], [FixedText], [Note], [DBUserID], [DBServerDateTime]) VALUES (26, 2, 1, 28, 2, N'SELECT HH.BillDate FROM View_ChequeRequisitionDetail AS HH WHERE HH.ChequeRequisitionDetailID=%n', N'ChequeRequisitionDetailID', NULL, NULL, -9, CAST(N'2017-01-29 14:04:08.890' AS DateTime))
GO
INSERT [dbo].[DataCollectionSetup] ([DataCollectionSetupID], [DataReferenceType], [DataReferenceID], [DataSetupType], [DataGenerateType], [QueryForValue], [ReferenceValueFields], [FixedText], [Note], [DBUserID], [DBServerDateTime]) VALUES (27, 2, 1, 29, 2, N'SELECT HH.BillDate FROM View_ChequeRequisitionDetail AS HH WHERE HH.ChequeRequisitionDetailID=%n', N'ChequeRequisitionDetailID', NULL, NULL, -9, CAST(N'2017-01-29 14:04:08.923' AS DateTime))
GO
INSERT [dbo].[DataCollectionSetup] ([DataCollectionSetupID], [DataReferenceType], [DataReferenceID], [DataSetupType], [DataGenerateType], [QueryForValue], [ReferenceValueFields], [FixedText], [Note], [DBUserID], [DBServerDateTime]) VALUES (28, 2, 1, 6, 2, N'SELECT HH.AccountHeadID FROM ChartsOfAccount AS HH WHERE HH.AccountHeadID=%n', N'AccountHeadID', NULL, NULL, -9, CAST(N'2017-01-29 14:04:08.940' AS DateTime))
GO
INSERT [dbo].[DataCollectionSetup] ([DataCollectionSetupID], [DataReferenceType], [DataReferenceID], [DataSetupType], [DataGenerateType], [QueryForValue], [ReferenceValueFields], [FixedText], [Note], [DBUserID], [DBServerDateTime]) VALUES (29, 2, 1, 8, 2, N'SELECT HH.Remarks FROM ChequeRequisition AS HH WHERE HH.ChequeRequisitionID=%n', N'ChequeRequisitionID', NULL, NULL, -9, CAST(N'2017-01-29 14:04:08.973' AS DateTime))
GO
INSERT [dbo].[DataCollectionSetup] ([DataCollectionSetupID], [DataReferenceType], [DataReferenceID], [DataSetupType], [DataGenerateType], [QueryForValue], [ReferenceValueFields], [FixedText], [Note], [DBUserID], [DBServerDateTime]) VALUES (30, 2, 2, 12, 2, N'SELECT MM.ACCostCenterID FROM ACCostCenter AS MM WHERE MM.ReferenceType=4 AND MM.ReferenceObjectID =(SELECT HH.BankAccountID FROM ChequeRequisition AS HH WHERE HH.ChequeRequisitionID=%n)', N'ChequeRequisitionID', NULL, NULL, -9, CAST(N'2017-01-29 14:04:09.237' AS DateTime))
GO
INSERT [dbo].[DataCollectionSetup] ([DataCollectionSetupID], [DataReferenceType], [DataReferenceID], [DataSetupType], [DataGenerateType], [QueryForValue], [ReferenceValueFields], [FixedText], [Note], [DBUserID], [DBServerDateTime]) VALUES (31, 2, 2, 13, 2, N'SELECT ISNULL(HH.ChequeAmount,0) FROM ChequeRequisition AS HH WHERE HH.ChequeRequisitionID =%n', N'ChequeRequisitionID', NULL, NULL, -9, CAST(N'2017-01-29 14:04:09.247' AS DateTime))
GO
INSERT [dbo].[DataCollectionSetup] ([DataCollectionSetupID], [DataReferenceType], [DataReferenceID], [DataSetupType], [DataGenerateType], [QueryForValue], [ReferenceValueFields], [FixedText], [Note], [DBUserID], [DBServerDateTime]) VALUES (32, 2, 2, 14, 2, N'SELECT HH.Remarks FROM ChequeRequisition AS HH WHERE HH.ChequeRequisitionID =%n', N'ChequeRequisitionID', NULL, NULL, -9, CAST(N'2017-01-29 14:04:09.293' AS DateTime))
GO
INSERT [dbo].[DataCollectionSetup] ([DataCollectionSetupID], [DataReferenceType], [DataReferenceID], [DataSetupType], [DataGenerateType], [QueryForValue], [ReferenceValueFields], [FixedText], [Note], [DBUserID], [DBServerDateTime]) VALUES (33, 2, 2, 15, 2, N'SELECT HH.ChequeDate FROM ChequeRequisition AS HH WHERE HH.ChequeRequisitionID=%n', N'ChequeRequisitionID', NULL, NULL, -9, CAST(N'2017-01-29 14:04:09.313' AS DateTime))
GO
INSERT [dbo].[DataCollectionSetup] ([DataCollectionSetupID], [DataReferenceType], [DataReferenceID], [DataSetupType], [DataGenerateType], [QueryForValue], [ReferenceValueFields], [FixedText], [Note], [DBUserID], [DBServerDateTime]) VALUES (34, 2, 2, 27, 2, N'SELECT 1878 AS CCCategory FROM ACCostCenter AS HH WHERE ACCostCenterID=1', N'ChequeRequisitionID', NULL, NULL, -9, CAST(N'2017-01-29 14:04:09.330' AS DateTime))
GO
INSERT [dbo].[DebitCreditSetup] ([DebitCreditSetupID], [IntegrationSetupDetailID], [DataCollectionQuery], [CompareColumn], [AccountHeadType], [FixedAccountHeadID], [AccountHeadSetup], [ReferenceType], [AccountNameSetup], [CurrencySetup], [ConversionRateSetup], [AmountSetup], [NarrationSetup], [IsChequeReferenceCreate], [ChequeReferenceDataSQL], [ChequeReferenceCompareColumns], [ChequeType], [ChequeSetup], [ChequeReferenceAmountSetup], [ChequeReferenceDescriptionSetup], [ChequeReferenceDateSetup], [IsDebit], [IsCostCenterCreate], [HasBillReference], [CostcenterDataSQL], [CostCenterCompareColumns], [CostcenterSetup], [CostCenterAmountSetup], [CostCenterDescriptionSetup], [CostCenterDateSetup], [IsVoucherBill], [VoucherBillDataSQL], [VoucherBillCompareColumns], [VoucherBillTrType], [VoucherBillSetup], [VoucherBillAmountSetup], [VoucherBillDescriptionSetup], [VoucherBillDateSetup], [IsOrderReferenceApply], [OrderReferenceDataSQL], [OrderReferenceCompareColumns], [OrderReferenceSetup], [OrderAmountSetup], [OrderRemarkSetup], [OrderDateSetup], [IsInventoryEffect], [InventoryDataSQL], [InventoryCompareColumns], [InventoryWorkingUnitSetup], [InventoryProductSetup], [InventoryQtySetup], [InventoryUnitSetup], [InventoryUnitPriceSetup], [InventoryDescriptionSetup], [InventoryDateSetup], [Note], [CostCenterCategorySetUp], [CostCenterNoColumn], [CostCenterRefObjType], [CostCenterRefObjColumn], [VoucherBillNoColumn], [VoucherBillRefObjType], [VoucherBillRefObjColumn], [BillDateSetup], [BillDueDateSetup], [DBUserID], [DBServerDateTime], [HasChequeReference]) VALUES (1, 1, N'SELECT DISTINCT HH.AccountHeadID, HH.ChequeRequisitionID  FROM View_ChequeRequisitionDetail AS HH WHERE ChequeRequisitionID=%n', N'ChequeRequisitionID', 2, 0, N'HH.AccountHeadID of ChartsOfAccount', 0, N'''Payable of Supplier', N'BaseCurrencyID of Company', N'1 of CRate', N'ISNULL(SUM(HH.Amount),0) of View_ChequeRequisitionDetail', N'HH.Remarks of ChequeRequisition', 0, NULL, NULL, 0, NULL, NULL, NULL, NULL, 1, 1, 1, N'SELECT DISTINCT HH.SubLedgerID,HH.AccountHeadID,HH.ChequeRequisitionID FROM View_ChequeRequisitionDetail AS HH WHERE HH.ChequeRequisitionID =%n AND HH.AccountHeadID=%n', N'ChequeRequisitionID,AccountHeadID', N'HH.ACCostCenterID of ACCostCenter', N'ISNULL(SUM(HH.Amount),0) of View_ChequeRequisitionDetail', N'HH.Remarks of ChequeRequisition', N'HH.ChequeDate of ChequeRequisition', 1, N'SELECT * FROM View_ChequeRequisitionDetail AS HH WHERE HH.ChequeRequisitionID=%n AND HH.AccountHeadID=%n AND HH.SubLedgerID=%n', N'ChequeRequisitionID,AccountHeadID,SubLedgerID', 2, N'HH.VoucherBillID of ChequeRequisitionDetail', N'HH.Amount of ChequeRequisitionDetail', N'HH.Remarks of ChequeRequisitionDetail', N'HH.ChequeDate of ChequeRequisition', 0, NULL, NULL, NULL, NULL, NULL, NULL, 0, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, N'N/A', N'15 of CCCAtegory', N'SubledgerName', 2, N'SubledgerID', N'BillNo', 1, N'VoucherBillID', N'HH.BillDate of View_ChequeRequisitionDetail', N'HH.BillDate of View_ChequeRequisitionDetail', -9, CAST(N'2017-01-29 14:04:08.703' AS DateTime), 0)
GO
INSERT [dbo].[DebitCreditSetup] ([DebitCreditSetupID], [IntegrationSetupDetailID], [DataCollectionQuery], [CompareColumn], [AccountHeadType], [FixedAccountHeadID], [AccountHeadSetup], [ReferenceType], [AccountNameSetup], [CurrencySetup], [ConversionRateSetup], [AmountSetup], [NarrationSetup], [IsChequeReferenceCreate], [ChequeReferenceDataSQL], [ChequeReferenceCompareColumns], [ChequeType], [ChequeSetup], [ChequeReferenceAmountSetup], [ChequeReferenceDescriptionSetup], [ChequeReferenceDateSetup], [IsDebit], [IsCostCenterCreate], [HasBillReference], [CostcenterDataSQL], [CostCenterCompareColumns], [CostcenterSetup], [CostCenterAmountSetup], [CostCenterDescriptionSetup], [CostCenterDateSetup], [IsVoucherBill], [VoucherBillDataSQL], [VoucherBillCompareColumns], [VoucherBillTrType], [VoucherBillSetup], [VoucherBillAmountSetup], [VoucherBillDescriptionSetup], [VoucherBillDateSetup], [IsOrderReferenceApply], [OrderReferenceDataSQL], [OrderReferenceCompareColumns], [OrderReferenceSetup], [OrderAmountSetup], [OrderRemarkSetup], [OrderDateSetup], [IsInventoryEffect], [InventoryDataSQL], [InventoryCompareColumns], [InventoryWorkingUnitSetup], [InventoryProductSetup], [InventoryQtySetup], [InventoryUnitSetup], [InventoryUnitPriceSetup], [InventoryDescriptionSetup], [InventoryDateSetup], [Note], [CostCenterCategorySetUp], [CostCenterNoColumn], [CostCenterRefObjType], [CostCenterRefObjColumn], [VoucherBillNoColumn], [VoucherBillRefObjType], [VoucherBillRefObjColumn], [BillDateSetup], [BillDueDateSetup], [DBUserID], [DBServerDateTime], [HasChequeReference]) VALUES (2, 1, N'SELECT * FROM View_ChequeRequisition WHERE ChequeRequisitionID =%n', N'ChequeRequisitionID', 1, 178, N'Bank Account - General', 0, N'''Bank of Account''', N'BaseCurrencyID of Company', N'1 of CRate', N'ISNULL(HH.ChequeAmount,0) of ChequeRequisition', N'HH.Remarks of ChequeRequisition', 0, NULL, NULL, 0, NULL, NULL, NULL, NULL, 0, 1, 0, N'SELECT * FROM View_ChequeRequisition WHERE ChequeRequisitionID =%n', N'ChequeRequisitionID', N'MM.ACCostCenterID of ACCostCenter', N'ISNULL(HH.ChequeAmount,0) of ChequeRequisition', N'HH.Remarks of ChequeRequisition', N'HH.ChequeDate of ChequeRequisition', 0, NULL, NULL, 0, NULL, NULL, NULL, NULL, 0, NULL, NULL, NULL, NULL, NULL, NULL, 0, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, N'N/A', N'1878 of CCCategory', N'AccountNo', 4, N'BankAccountID', NULL, 0, NULL, NULL, NULL, -9, CAST(N'2017-01-29 14:04:09.070' AS DateTime), 0)
GO
INSERT [dbo].[IntegrationSetup] ([IntegrationSetupID], [SetupNo], [VoucherSetup], [DataCollectionSQL], [KeyColumn], [Note], [Sequence], [SetupType], [DBUserID], [DBServerDateTime]) VALUES (1, N'0001', 1, N'SELECT * FROM View_ChequeRequisition WHERE ChequeRequisitionID NOT IN (SELECT HH.PKValue FROM VoucherMapping AS HH WHERE HH.TableName=''ChequeRequisition'' AND HH.PKColumnName=''ChequeRequisitionID'' AND HH.VoucherSetup=1) ORDER BY RequisitionDate ASC', N'ChequeRequisitionID', N'N/A', NULL, 1, -9, CAST(N'2017-01-29 14:04:08.667' AS DateTime))
GO
INSERT [dbo].[IntegrationSetupDetail] ([IntegrationSetupDetailID], [IntegrationSetupID], [VoucherTypeID], [BusinessUnitSetup], [VoucherDateSetup], [NarrationSetup], [ReferenceNoteSetup], [UpdateTable], [KeyColumn], [Note], [DBUserID], [DBServerDateTime]) VALUES (1, 1, 3, N'HH.BUID of ChequeRequisition', N'HH.ChequeDate of ChequeRequisition', N'HH.Remarks of ChequeRequisition', N'N/A(Fixed Text)', N'ChequeRequisition', N'ChequeRequisitionID', N'N/A', -9, CAST(N'2017-01-29 14:04:08.680' AS DateTime))
GO

ALTER VIEW [dbo].[View_BankReconciliation]
AS
SELECT	BankReconciliation.BankReconciliationID, 
		BankReconciliation.SubledgerID, 
		BankReconciliation.VoucherDetailID, 
		BankReconciliation.AccountHeadID, 
		BankReconciliation.ReconcilHeadID, 
		BankReconciliation.ReconcilDate, 
		BankReconciliation.ReconcilRemarks, 
		BankReconciliation.IsDebit, 
		BankReconciliation.Amount, 
		BankReconciliation.ReconcilStatus, 
		BankReconciliation.ReconcilBy,
		CC.Code  AS SubledgerCode, 
		CC.Name AS SubledgerName,
		COA.AccountCode, 
		COA.AccountHeadName, 
		RCAH.AccountCode AS RCAHCode, 
		RCAH.AccountHeadName AS RCAHName, 
		'' AS ReverseHead,
		ReconcilUser.UserName AS ReconcilByName,
		VD.BUID,
		VD.VoucherDate,
		VD.VoucherID,
		(SELECT HH.VoucherNo FROM View_Voucher AS HH WHERE HH.VoucherID=VD.VoucherID) AS VoucherNo,
		0 AS DebitAmount,
		0 AS CreditAmount,
		0 AS CurrentBalance

FROM    BankReconciliation 
LEFT OUTER JOIN ChartsOfAccount AS COA ON BankReconciliation.AccountHeadID = COA.AccountHeadID 
LEFT OUTER JOIN	ChartsOfAccount AS RCAH ON BankReconciliation.AccountHeadID = RCAH.AccountHeadID 
LEFT OUTER JOIN Users AS ReconcilUser ON BankReconciliation.ReconcilBy = ReconcilUser.UserID
LEFT OUTER JOIN View_VoucherDetail AS VD ON BankReconciliation.VoucherDetailID = VD.VoucherDetailID
LEFT OUTER JOIN ACCostCenter AS CC ON BankReconciliation.SubledgerID = CC.ACCostCenterID







GO


