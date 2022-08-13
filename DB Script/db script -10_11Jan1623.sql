ALTER PROCEDURE [dbo].[SP_IUD_VoucherBill] 
(
	@VoucherBillID as	int,	 
	@AccountHeadID as	int,
	@SubLedgerID AS INT,	 
	@BUID as int,
	@BillNo as	varchar(500),	 
	@BillDate as	datetime,	 
	@DueDate as	datetime,	 
	@CreditDays as	int, 
	@Amount as	decimal(30, 17),	 
	@IsActive as	bit,	 
	@CurrencyID as	int,	 
	@CurrencyRate as	decimal(30, 18),	 
	@CurrencyAmount as	decimal(30, 18),	
	@ReferenceType  as smallint,
	@ReferenceObjID as int,
	@OpeningBillAmount as decimal(30,17),
	@OpeningBillDate as date,
	@Remarks as Varchar(1024),		
	@DBUserID as	int,	 
	@DBOperation as smallint
	--%n, %n, %n, %n, %s, %d, %d, %n, %n, %n, %n, %n, %n, %n, %n, %n, %d, %s, %n, %n
)
AS
BEGIN TRAN
DECLARE 
@DBServerDateTime as datetime,
@IsHoldBill as bit,
@Flag as bit
 SET @IsHoldBill = 0
SET @DBServerDateTime=Getdate()
SET @Flag=0
SET NOCOUNT ON;
IF (@DBOperation=1)
BEGIN
	
	IF (@BillNo is null OR @BillNo='')
	BEGIN
		ROLLBACK
			RAISERROR (N'Please, Entry Bill BillNo/No.!!~',16,1);	
		RETURN
	END
	IF (@AccountHeadID<=0 or @AccountHeadID is null)
	BEGIN
		ROLLBACK
			RAISERROR (N'Please, Account Head  not found .!!~',16,1);	
		RETURN
	END
	IF(@ReferenceType>0)
	BEGIN
		IF EXISTS (SELECT BillNo FROM VoucherBill WHERE BillNo=@BillNo and AccountHeadID=@AccountHeadID AND ReferenceObjID=0)
		BEGIN
			SET @VoucherBillID=(SELECT TOP 1 VoucherBillID FROM VoucherBill WHERE BillNo=@BillNo and AccountHeadID=@AccountHeadID AND ReferenceObjID=0)
			UPDATE [VoucherBill] SET AccountHeadID=@AccountHeadID, SubLedgerID=@SubLedgerID,  BUID=@BUID, BillNo=@BillNo, BillDate=@BillDate, DueDate=@DueDate, CreditDays=@CreditDays, Amount=@Amount, IsActive=@IsActive, CurrencyID=@CurrencyID, CurrencyRate=@CurrencyRate, CurrencyAmount=@CurrencyAmount, ReferenceType=@ReferenceType,	ReferenceObjID=@ReferenceObjID, DBUserID=@DBUserID, IsHoldBill=@IsHoldBill WHERE VoucherBillID = @VoucherBillID			
			SET @Flag=1
		END
	END
	IF(@Flag=0)
	BEGIN
		IF EXISTS (SELECT * FROM VoucherBill AS HH WHERE HH.BillNo=@BillNo AND  HH.AccountHeadID=@AccountHeadID AND HH.SubLedgerID=@SubLedgerID)
		BEGIN
			ROLLBACK
				RAISERROR (N'This Bill No alreay exists for this .!!~',16,1);	
			RETURN
		END		
		SET @VoucherBillID=(SELECT ISNULL(MAX(VoucherBillID),0)+1 FROM VoucherBill)
		
		INSERT INTO VoucherBill (VoucherBillID,			AccountHeadID,		SubLedgerID,	BUID,		BillNo,			BillDate,			DueDate,			CreditDays,			Amount,			IsActive,			CurrencyID,			CurrencyRate,			CurrencyAmount,		ReferenceType,	ReferenceObjID,		OpeningBillAmount,		OpeningBillDate,	Remarks,	IsHoldBill,		DBUserID,			DBServerDateTime)
		VALUES					(@VoucherBillID,		@AccountHeadID,		@SubLedgerID,	@BUID,		@BillNo,		@BillDate,			@DueDate,			@CreditDays,		@Amount,		@IsActive,			@CurrencyID,		@CurrencyRate,			@CurrencyAmount,	@ReferenceType,	@ReferenceObjID,	@OpeningBillAmount,		@OpeningBillDate,	@Remarks,	@IsHoldBill,	@DBUserID,			@DBServerDateTime)		
	END	
	Select * from View_VoucherBill where View_VoucherBill.VoucherBillId= @VoucherBillID	
END
IF (@DBOperation=2)--Start Update
BEGIN	 
	IF (@VoucherBillID is null OR @VoucherBillID<=0)
	BEGIN
		ROLLBACK
			RAISERROR (N'Please Select a Cost center.!!~',16,1);	
		RETURN
	END
	IF (@AccountHeadID<=0 or @AccountHeadID is null)
	BEGIN
		ROLLBACK
			RAISERROR (N'Please, Account Head  not found .!!~',16,1);	
		RETURN
	END
	IF EXISTS (SELECT * FROM VoucherBill AS HH WHERE HH.BillNo=@BillNo AND  HH.AccountHeadID=@AccountHeadID AND HH.SubLedgerID=@SubLedgerID AND HH.VoucherBillID<>@VoucherBillID)
	BEGIN
		ROLLBACK
			RAISERROR (N'This Bill No alreay exists for this .!!~',16,1);	
		RETURN
	END

	IF EXISTS (SELECT * FROM VoucherBillTransaction AS TT WHERE  TT.VoucherBillID=@VoucherBillID)
	BEGIN
		ROLLBACK
			RAISERROR (N'Edition Not Possible VoucherBillTransaction Reference Exists.!!~',16,1);	
		RETURN
	END	
	IF EXISTS (SELECT * FROM AccountOpenningBreakdown WHERE  BreakdownType=2 AND BreakdownObjID=@VoucherBillID)
	BEGIN
		IF(ISNULL((SELECT SUM(TT.AmountInCurrency) FROM AccountOpenningBreakdown AS TT WHERE  TT.BreakdownType=2 AND TT.BreakdownObjID=@VoucherBillID),0)>@CurrencyAmount)
		BEGIN
			ROLLBACK
				RAISERROR (N'Please Check Bill Amount! Bill amount must be greater than or equal Openning Breakdown Amount!!~',16,1);	
			RETURN
		END
	END	
	UPDATE [VoucherBill] SET AccountHeadID=@AccountHeadID,SubLedgerID=@SubLedgerID, BUID=@BUID, BillNo=@BillNo, BillDate=@BillDate, DueDate=@DueDate, CreditDays=@CreditDays, Amount=@Amount, IsActive=@IsActive, CurrencyID=@CurrencyID, CurrencyRate=@CurrencyRate, CurrencyAmount=@CurrencyAmount, ReferenceType=@ReferenceType,	ReferenceObjID=@ReferenceObjID, OpeningBillAmount=@OpeningBillAmount,	OpeningBillDate=@OpeningBillDate,	Remarks=@Remarks,	IsHoldBill=@IsHoldBill,  DBUserID=@DBUserID WHERE VoucherBillID = @VoucherBillID
	Select * from View_VoucherBill where View_VoucherBill.VoucherBillId= @VoucherBillID
END
IF (@DBOperation=3)
BEGIN
		
	IF EXISTS (SELECT * FROM VoucherBillTransaction WHERE  VoucherBillID=@VoucherBillID)
	BEGIN
		ROLLBACK
			RAISERROR (N'Edition Not Possible VoucherBillTransaction Reference Exists.!!~',16,1);	
		RETURN
	END	
	IF EXISTS (SELECT * FROM AccountOpenningBreakdown WHERE  BreakdownType=2 AND BreakdownObjID=@VoucherBillID)
	BEGIN
		ROLLBACK
			RAISERROR (N'Edition Not Possible Openning Reference Exists.!!~',16,1);	
		RETURN
	END	
	DELETE FROM VoucherBill WHERE VoucherBillID=@VoucherBillID	
END
COMMIT TRAN



