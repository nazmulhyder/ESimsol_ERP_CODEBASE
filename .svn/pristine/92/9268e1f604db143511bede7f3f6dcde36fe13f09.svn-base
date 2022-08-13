IF NOT EXISTS (SELECT * FROM sys.columns where Name = N'BUID' and Object_ID = Object_ID(N'IntegrationSetup'))
BEGIN
   ALTER TABLE IntegrationSetup
   ADD BUID int
END
GO
/****** Object:  View [dbo].[View_IntegrationSetup]    Script Date: 2/8/2017 11:19:13 AM ******/
DROP VIEW [dbo].[View_IntegrationSetup]
GO
/****** Object:  StoredProcedure [dbo].[SP_IUD_IntegrationSetup]    Script Date: 2/8/2017 11:19:13 AM ******/
DROP PROCEDURE [dbo].[SP_IUD_IntegrationSetup]
GO
/****** Object:  StoredProcedure [dbo].[SP_IUD_IntegrationSetup]    Script Date: 2/8/2017 11:19:13 AM ******/
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
	@BUID as int,
	@DBUserID as int,
	@DBOperation as smallint
	--%n, %s, %n, %s, %s, %s, %n, %n, %n, %n, %n     
)
AS
BEGIN TRAN

DECLARE 
@PV_DBServerDateTime as datetime
SET @PV_DBServerDateTime=Getdate()	

		
IF(@DBOperation=1)
BEGIN	
	IF EXISTS(SELECT * FROM IntegrationSetup AS HH WHERE HH.VoucherSetup=@VoucherSetup AND HH.BUID=@BUID)
	BEGIN
		ROLLBACK
			RAISERROR (N'Your Entered Setup Name Already Exists!!',16,1);	
		RETURN
	END	
	SET @IntegrationSetupID=(SELECT ISNULL(MAX(IntegrationSetupID),0)+1 FROM IntegrationSetup)
	SET @SetupNo=RIGHT('0000' + CONVERT(VARCHAR(4), @IntegrationSetupID), 4)
	SET @Sequence = (SELECT ISNULL(IGS.Sequence,0)+1 FROM IntegrationSetup AS IGS WHERE IGS.IntegrationSetupID=(SELECT MAX(IntegrationSetupID) FROM IntegrationSetup))
	INSERT INTO IntegrationSetup	(IntegrationSetupID,	SetupNo,	VoucherSetup,	DataCollectionSQL,KeyColumn,	Note,	Sequence, SetupType,	BUID,	DBUserID,	DBServerDateTime)			
							VALUES	(@IntegrationSetupID,	@SetupNo,	@VoucherSetup,	@DataCollectionSQL,@KeyColumn,	@Note,	@Sequence, @SetupType,	@BUID,	@DBUserID,	@PV_DBServerDateTime)		
	SELECT * FROM View_IntegrationSetup WHERE IntegrationSetupID=@IntegrationSetupID
END

IF(@DBOperation=2)
BEGIN
	IF (@IntegrationSetupID<=0) 
	BEGIN
		ROLLBACK
			RAISERROR (N' Selected Integration Setup are Invalid Please try again!!',16,1);	
		RETURN
	END
	IF EXISTS(SELECT * FROM IntegrationSetup AS HH WHERE HH.VoucherSetup=@VoucherSetup AND HH.BUID=@BUID AND HH.IntegrationSetupID!=@IntegrationSetupID)
	BEGIN
		ROLLBACK
			RAISERROR (N'Your Entered Setup Name Already Exists!!',16,1);	
		RETURN
	END	
	UPDATE IntegrationSetup SET	SetupNo=SetupNo,	VoucherSetup=@VoucherSetup,	DataCollectionSQL=@DataCollectionSQL,KeyColumn=@KeyColumn,	Note =@Note, SetupType=@SetupType, BUID=@BUID, DBUserID =@DBUserID,	DBServerDateTime =@PV_DBServerDateTime Where IntegrationSetupID =@IntegrationSetupID
	SELECT * FROM View_IntegrationSetup WHERE IntegrationSetupID=@IntegrationSetupID
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
/****** Object:  View [dbo].[View_IntegrationSetup]    Script Date: 2/8/2017 11:19:13 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE VIEW [dbo].[View_IntegrationSetup]
AS 
SELECT	IntegrationSetup.IntegrationSetupID, 
		IntegrationSetup.SetupNo, 
		IntegrationSetup.VoucherSetup, 
		IntegrationSetup.DataCollectionSQL, 
		IntegrationSetup.KeyColumn, 
		IntegrationSetup.Note, 
        IntegrationSetup.Sequence, 
		IntegrationSetup.SetupType, 
		IntegrationSetup.BUID, 
		BU.Name AS BUName, 
		BU.ShortName AS BUSName

FROM			IntegrationSetup 
LEFT OUTER JOIN BusinessUnit AS BU ON IntegrationSetup.BUID=BU.BusinessUnitID
GO
ALTER  PROCEDURE   [dbo].[SP_IUD_VOReference]
(	
	@VOReferenceID	as int,
	@VoucherDetailID	as int,
	@OrderID	as int,
	@TransactionDate	as datetime,
	@Remarks	as varchar(1024),
	@IsDebit as bit,
	@CurrencyID	as int,
	@ConversionRate	as decimal(30, 17),
	@AmountInCurrency	as decimal(30, 17),
	@Amount	as decimal(30, 17),
	@CCTID as int,
    @DBUserID as int,
    @Operation as smallint
	--%n, %n, %n, %d, %s, %n, %n, %n, %n, %n, %n, %n
)	
AS
BEGIN TRAN
DECLARE 
@DBServerDateTime as datetime
SET @DBServerDateTime=Getdate()
IF(@Operation=1)
BEGIN			
	IF(ISNULL(@VoucherDetailID,0)<=0)
	BEGIN
		ROLLBACK
			RAISERROR (N'Invalid voucher. Please try again!!~',16,1);	
		RETURN
	END
	IF(ISNULL(@OrderID,0)<=0)
	BEGIN
		ROLLBACK
			RAISERROR (N'Invalid Voucher Order. Please try again!!~',16,1);	
		RETURN
	END
	IF(ISNULL(@Amount,0)<=0)
	BEGIN
		ROLLBACK
			RAISERROR (N'Invalid Voucher Reference Amount. Please try again!!~',16,1);	
		RETURN
	END
	IF(ISNULL(@CurrencyID,0)<=0)
	BEGIN
		ROLLBACK
			RAISERROR (N'Invalid Voucher Reference Currency. Please try again!!~',16,1);	
		RETURN
	END
	IF(ISNULL(@ConversionRate,0)<=0)
	BEGIN
		ROLLBACK
			RAISERROR (N'Invalid Voucher Reference Conversion Rate. Please try again!!~',16,1);	
		RETURN
	END
	SET @TransactionDate=(SELECT VD.VoucherDate FROM View_VoucherDetail AS VD WHERE VD.VoucherDetailID=@VoucherDetailID)
	SET @VOReferenceID=(SELECT ISNULL(MAX(VOReferenceID),0)+1 FROM VOReference)		
	SET @AmountInCurrency = @Amount
	SET @Amount = (@Amount * @ConversionRate) 
	INSERT INTO VOReference  (VOReferenceID,	VoucherDetailID,	OrderID,	TransactionDate,	Remarks,	IsDebit,	CurrencyID,		ConversionRate,		AmountInCurrency,	Amount,		CCTID,	DBUserID,	DBServerDateTime)	
					   Values(@VOReferenceID,	@VoucherDetailID,	@OrderID,	@TransactionDate,	@Remarks,	@IsDebit,	@CurrencyID,	@ConversionRate,	@AmountInCurrency,	@Amount,	@CCTID,	@DBUserID,	@DBServerDateTime)			
	SELECT * FROM VOReference WHERE VOReferenceID=@VOReferenceID
END

IF(@Operation=3)
BEGIN		
	IF(@VoucherDetailID<=0)
	BEGIN
		ROLLBACK
			RAISERROR (N'Invalid voucher. Please try again!!~',16,1);	
		RETURN
	END
	DELETE FROM VOReference WHERE VoucherDetailID=@VoucherDetailID
END
COMMIT TRAN







