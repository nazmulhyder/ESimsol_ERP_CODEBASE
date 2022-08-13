ALTER PROCEDURE [dbo].[SP_IUD_Voucher]
(
	@VoucherID	as int,
	@BUID as int,
	@VoucherTypeID	as int,
	@VoucherNo	as varchar(50),
	@Narration	as varchar(max),
	@ReferenceNote	as varchar(500),
	@VoucherDate	as date,	
	@AuthorizedBy	as int,
	@LastUpdatedDate	as date,
	@OperationType as smallint,
	@BatchID as int,
	@TaxEffect as smallint,
	@DBUserID	as int,
	@DBOperation as smallint
	--%n, %n, %n, %s, %s, %s, %d, %n, %d,  %n, %n, %n, %n, %n
)	
AS
BEGIN TRAN
DECLARE 
@DBServerDateTime as datetime,
@TempAuthorizedBy as int,
@IsNarationEntryMust as bit,
@ResetType as smallint,
@TempVoucherNumberType as smallint,
@TempVoucherDate as date,
@TempVoucherTypeID as int,
@LockDateTime as datetime,
@TempMessage as Varchar(512)

SET @TempVoucherNumberType =(SELECT ISNULL(NumberMethod,0) FROM VoucherType WHERE VoucherTypeID=@VoucherTypeID)

SET @OperationType=1 -- EnumVoucherOperationType:FreshVoucher = 1
SET @DBServerDateTime=Getdate()
IF(@DBOperation=1)
BEGIN
	SET @VoucherID=(SELECT ISNULL(MAX(VoucherID),0)+1 FROM Voucher)	
	IF(@TempVoucherNumberType=2)
	BEGIN		
		SET @VoucherNo=[dbo].[FN_VoucherNo] (@BUID, @VoucherTypeID, @VoucherDate)			
	END		
	IF(@VoucherNo=Null OR @VoucherNo='')
	BEGIN
		ROLLBACK
			RAISERROR (N'Invalid Voucher No Please Try Again!!~',16,1);	
		RETURN
	END
	IF EXISTS(SELECT * FROM Voucher AS HH WHERE HH.BUID=@BUID AND HH.VoucherNo=@VoucherNo)	
	BEGIN
		ROLLBACK
			RAISERROR (N'Your Entered Voucher No Already Exists!!~',16,1);	
		RETURN
	END		
	IF(ISNULL(@BUID,0)<=0)
	BEGIN
		ROLLBACK
			RAISERROR (N'Invalid Business Unit. Please try again!!~',16,1);	
		RETURN
	END
	IF(ISNULL(@VoucherTypeID,0)<=0)	
	BEGIN
		ROLLBACK
			RAISERROR (N'Invalid Voucher Type!!~',16,1);	
		RETURN
	END	

	IF NOT EXISTS(SELECT * FROM AccountingSession AS TT WHERE TT.SessionType=6 AND CONVERT(DATE,CONVERT(VARCHAR(12),TT.StartDate,106)) = CONVERT(DATE,CONVERT(VARCHAR(12),@VoucherDate,106)))	
	BEGIN
		ROLLBACK
			RAISERROR (N'Invalid Voucher Date! Please Configure accounting session for SELECTed date!~',16,1);	
		RETURN
	END

	IF NOT EXISTS(SELECT * FROM VoucherBatch AS VB WHERE VB.VoucherBatchID=@BatchID AND VB.BatchStatus=1)
	BEGIN
		ROLLBACK
			RAISERROR (N'Voucher Batch is not Open. Please Refresh and Try Again!!~',16,1);	
		RETURN
	END

	--commented by fahim0abir on date: 02 Nov 2015 as per instructions from faruk
	SET @LockDateTime = (SELECT TT.LockDateTime FROM AccountingSession AS TT WHERE TT.SessionType=6 AND CONVERT(DATE,CONVERT(VARCHAR(12),TT.StartDate,106)) = CONVERT(DATE,CONVERT(VARCHAR(12),@VoucherDate,106)))		
	IF(@LockDateTime<@VoucherDate)	
	BEGIN
		ROLLBACK
			RAISERROR (N'Your selected voucher date already lock!~',16,1);	
		RETURN
	END

	IF NOT EXISTS(SELECT * FROM AccountingSession WHERE SessionType=6 AND StartDate=@VoucherDate AND YearStatus IN (1,2))	
	BEGIN
		ROLLBACK
			RAISERROR (N'Your entered voucher date not in running/freeze session!!~',16,1);	
		RETURN
	END	
		
	SET @IsNarationEntryMust=(SELECT MustNarrationEntry FROM VoucherType WHERE VoucherTypeID=@VoucherTypeID)		
	IF(@IsNarationEntryMust=1)
	BEGIN
		IF(@Narration='')
		BEGIN
			ROLLBACK
				RAISERROR (N'Please enter Voucher Naration!!~',16,1);	
			RETURN
		END
	END
		
	INSERT INTO	Voucher	(VoucherID,		BUID,	VoucherTypeID,	VoucherNo,	Narration,	ReferenceNote,	VoucherDate,	AuthorizedBy,	LastUpdatedDate,	OperationType,	BatchID,	TaxEffect,  DBUserID,	DBServerDate)
    			VALUES  (@VoucherID,	@BUID,	@VoucherTypeID,	@VoucherNo,	@Narration,	@ReferenceNote,	@VoucherDate,	@AuthorizedBy,	@LastUpdatedDate,	@OperationType, @BatchID,	@TaxEffect, @DBUserID,	@DBServerDateTime)    				
    SELECT  * FROM View_Voucher WHERE VoucherID=@VoucherID
END

IF(@DBOperation=2)
BEGIN
	IF(@VoucherID<=0)
	BEGIN
		ROLLBACK
			RAISERROR (N'Your SELECTed voucher Is Invalid Please Refresh and try again!!~',16,1);	
		RETURN
	END	
	--IF NOT EXISTS(SELECT * FROM VoucherBatch AS VB WHERE VB.VoucherBatchID=@BatchID AND VB.BatchStatus=1)
	--BEGIN
	--	ROLLBACK
	--		RAISERROR (N'Voucher Batch is not Open. Please Refresh and Try Again!!~',16,1);	
	--	RETURN
	--END
	SET @VoucherNo = (SELECT HH.VoucherNo FROM Voucher AS HH WHERE HH.VoucherID=@VoucherID)
	SET @TempVoucherDate=(SELECT HH.VoucherDate FROM Voucher AS HH WHERE HH.VoucherID=@VoucherID)
	IF(MONTH(@TempVoucherDate)!=MONTH(@VoucherDate))
	BEGIN
		IF(@TempVoucherNumberType=2)
		BEGIN		
			SET @VoucherNo=[dbo].[FN_VoucherNo] (@BUID, @VoucherTypeID, @VoucherDate)			
		END		
	END
	IF EXISTS(SELECT * FROM Voucher AS HH WHERE HH.BUID=@BUID AND HH.VoucherNo=@VoucherNo AND HH.VoucherID!=@VoucherID)	
	BEGIN
		ROLLBACK
			RAISERROR (N'Your Entered Voucher No Already Exists!!~',16,1);	
		RETURN
	END	
	IF NOT EXISTS(SELECT * FROM AccountingSession AS TT WHERE TT.SessionType=6 AND CONVERT(DATE,CONVERT(VARCHAR(12),TT.StartDate,106)) = CONVERT(DATE,CONVERT(VARCHAR(12),@VoucherDate,106)))	
	BEGIN
		ROLLBACK
			RAISERROR (N'Invalid Voucher Date! Please Configure accounting session for SELECTed date!~',16,1);	
		RETURN
	END
	--commented by fahim0abir on date: 02 Nov 2015 as per instructions from faruk
	SET @LockDateTime = (SELECT TT.LockDateTime FROM AccountingSession AS TT WHERE TT.SessionType=6 AND CONVERT(DATE,CONVERT(VARCHAR(12),TT.StartDate,106)) = CONVERT(DATE,CONVERT(VARCHAR(12),@VoucherDate,106)))		
	IF(@LockDateTime<@VoucherDate)	
	BEGIN
		ROLLBACK
			RAISERROR (N'Your SELECTed voucher date already lock!~',16,1);	
		RETURN
	END
	IF NOT EXISTS(SELECT * FROM AccountingSession WHERE SessionType=6 AND StartDate=@VoucherDate AND YearStatus IN (1,2))	
	BEGIN
		ROLLBACK
			RAISERROR (N'Your entered voucher date not in running/freeze session!!~',16,1);	
		RETURN
	END	
	SET @IsNarationEntryMust=(SELECT MustNarrationEntry FROM VoucherType WHERE VoucherTypeID=@VoucherTypeID)		
	IF(@IsNarationEntryMust=1)
	BEGIN
		IF(@Narration='')
		BEGIN
			ROLLBACK
				RAISERROR (N'Please enter Voucher Naration!!~',16,1);	
			RETURN
		END
	END

	IF(@VoucherNo=Null OR @VoucherNo='')
	BEGIN
		ROLLBACK
			RAISERROR (N'Invalid Voucher No Please Try Again!!~',16,1);	
		RETURN
	END
	IF EXISTS(SELECT * FROM Voucher WHERE VoucherNo=@VoucherNo AND VoucherID!=@VoucherID)	
	BEGIN
		ROLLBACK
			RAISERROR (N'Your Entered Voucher No Already Exists!!~',16,1);	
		RETURN
	END	
	IF(ISNULL(@BUID,0)<=0)
	BEGIN
		ROLLBACK
			RAISERROR (N'Invalid Business Unit. Please try again!!~',16,1);	
		RETURN
	END	
	IF(ISNULL(@VoucherTypeID,0)<=0)	
	BEGIN
		ROLLBACK
			RAISERROR (N'Invalid Voucher Type!!~',16,1);	
		RETURN
	END	
	IF EXISTS(SELECT * FROM Voucher AS TT WHERE TT.CounterVoucherID=@VoucherID AND ISNULL(TT.CounterVoucherID,0)!=0)	
	BEGIN
		ROLLBACK
			RAISERROR (N'Edition not possible. read only voucher!!~',16,1);	
		RETURN
	END	

	SET @TempAuthorizedBy =(SELECT ISNULL(AuthorizedBy,0) FROM Voucher WHERE VoucherID=@VoucherID)
	Update Voucher  SET BUID=@BUID, VoucherTypeID=@VoucherTypeID,	VoucherNo=@VoucherNo,	Narration=@Narration,	ReferenceNote=@ReferenceNote,	VoucherDate=@VoucherDate,	AuthorizedBy=@AuthorizedBy,	LastUpdatedDate=@LastUpdatedDate,	OperationType=@OperationType, BatchID=@BatchID, TaxEffect=@TaxEffect WHERE VoucherID=@VoucherID
	SELECT  * FROM View_Voucher where VoucherID=@VoucherID
END

IF(@DBOperation=3)
BEGIN
	IF(@VoucherID<=0)
	BEGIN
		ROLLBACK
			RAISERROR (N'Your SELECTed Voucher Is Invalid Please Refresh and try again!!~',16,1);	
		RETURN
	END			
	
	SET @AuthorizedBy=isnull((SELECT ISNULL(AuthorizedBy,0) FROM Voucher WHERE VoucherID=@VoucherID),0)
	IF(@AuthorizedBy=-9 OR @AuthorizedBy>0)
	BEGIN
		ROLLBACK
			RAISERROR (N'Your SELECTed voucher Already approved!!~',16,1);	
		RETURN
	END		
	IF EXISTS(SELECT * FROM Voucher AS TT WHERE TT.CounterVoucherID=@VoucherID AND ISNULL(TT.CounterVoucherID,0)!=0)	
	BEGIN
		ROLLBACK
			RAISERROR (N'Deletion not possible. read only voucher!!~',16,1);	
		RETURN
	END	

	IF EXISTS(SELECT * FROM View_VoucherBillTransaction AS TT WHERE TT.VoucherID != @VoucherID AND TT.TrType IN (2,4) AND TT.VoucherBillID IN(SELECT VBT.VoucherBillID FROM VoucherBillTransaction AS VBT WHERE VBT.VoucherDetailID IN(SELECT HH.VoucherDetailID FROM VoucherDetail AS HH WHERE HH.VoucherID=@VoucherID)))
	BEGIN
		SET @TempMessage =(SELECT TOP 1 TT.VoucherNo FROM View_VoucherBillTransaction AS TT WHERE TT.VoucherID != @VoucherID AND TT.TrType IN (2,4) AND TT.VoucherBillID IN(SELECT VBT.VoucherBillID FROM VoucherBillTransaction AS VBT WHERE VBT.VoucherDetailID IN(SELECT HH.VoucherDetailID FROM VoucherDetail AS HH WHERE HH.VoucherID=@VoucherID)))
		ROLLBACK
			RAISERROR (N'Sorry Delete Not Possible Your Selected Voucher Bill(s) already userd as a AgstRef on Voucher No : %s!!~',16,1, @TempMessage);	
		RETURN
	END

	DECLARE
	@CounterVoucherID as int
	SET @CounterVoucherID = ISNULL((SELECT TT.CounterVoucherID FROM Voucher AS TT WHERE TT.VoucherID=@VoucherID),0)
	IF(@CounterVoucherID>0)
	BEGIN
		DELETE FROM VoucherMapping WHERE VoucherID=@CounterVoucherID
		DELETE FROM VOReference WHERE VOReference.VoucherDetailID in (SELECT VoucherDetailID from VoucherDetail where VoucherID=@CounterVoucherID)
		DELETE FROM VoucherBillTransaction WHERE VoucherBillTransaction.VoucherDetailID in (SELECT VoucherDetailID from VoucherDetail where VoucherID=@CounterVoucherID) 	
		DELETE FROM VoucherCheque WHERE VoucherCheque.VoucherDetailID in (SELECT VoucherDetailID from VoucherDetail where VoucherID=@CounterVoucherID) 
		DELETE FROM CostCenterTransaction WHERE CostCenterTransaction.VoucherDetailID in (SELECT VoucherDetailID from VoucherDetail where VoucherID=@CounterVoucherID) 
		DELETE FROM VPTransaction WHERE VPTransaction.VoucherDetailID in (SELECT VoucherDetailID from VoucherDetail where VoucherID=@CounterVoucherID) 
		DELETE FROM VoucherDetail WHERE VoucherID=@CounterVoucherID
		DELETE FROM Voucher WHERE VoucherID=@CounterVoucherID
	END

	DELETE FROM VoucherMapping WHERE VoucherID=@VoucherID
	DELETE FROM VOReference WHERE VOReference.VoucherDetailID in (SELECT VoucherDetailID from VoucherDetail where VoucherID=@VoucherID)
	DELETE FROM VoucherBillTransaction WHERE VoucherBillTransaction.VoucherDetailID in (SELECT VoucherDetailID from VoucherDetail where VoucherID=@VoucherID) 	
	DELETE FROM VoucherCheque WHERE VoucherCheque.VoucherDetailID in (SELECT VoucherDetailID from VoucherDetail where VoucherID=@VoucherID) 
	DELETE FROM CostCenterTransaction WHERE CostCenterTransaction.VoucherDetailID in (SELECT VoucherDetailID from VoucherDetail where VoucherID=@VoucherID) 
	DELETE FROM VPTransaction WHERE VPTransaction.VoucherDetailID in (SELECT VoucherDetailID from VoucherDetail where VoucherID=@VoucherID) 
	DELETE FROM VoucherDetail WHERE VoucherID=@VoucherID
	DELETE FROM Voucher WHERE VoucherID=@VoucherID
END
COMMIT TRAN




