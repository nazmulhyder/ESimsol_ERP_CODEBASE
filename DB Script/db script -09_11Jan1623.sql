GO
/****** Object:  View [dbo].[View_ChequeRequisition]    Script Date: 1/11/2017 9:45:01 AM ******/
DROP VIEW [dbo].[View_ChequeRequisition]
GO
/****** Object:  View [dbo].[View_ChequeRequisitionDetail]    Script Date: 1/11/2017 9:45:01 AM ******/
DROP VIEW [dbo].[View_ChequeRequisitionDetail]
GO
/****** Object:  View [dbo].[View_VoucherBill]    Script Date: 1/11/2017 9:45:01 AM ******/
DROP VIEW [dbo].[View_VoucherBill]
GO
/****** Object:  Table [dbo].[ChequeRequisitionDetail]    Script Date: 1/11/2017 9:45:01 AM ******/
DROP TABLE [dbo].[ChequeRequisitionDetail]
GO
/****** Object:  Table [dbo].[ChequeRequisition]    Script Date: 1/11/2017 9:45:01 AM ******/
DROP TABLE [dbo].[ChequeRequisition]
GO
/****** Object:  StoredProcedure [dbo].[SP_IUD_Voucher]    Script Date: 1/11/2017 9:45:01 AM ******/
DROP PROCEDURE [dbo].[SP_IUD_Voucher]
GO
/****** Object:  StoredProcedure [dbo].[SP_IUD_ChequeRequisitionDetail]    Script Date: 1/11/2017 9:45:01 AM ******/
DROP PROCEDURE [dbo].[SP_IUD_ChequeRequisitionDetail]
GO
/****** Object:  StoredProcedure [dbo].[SP_IUD_ChequeRequisition]    Script Date: 1/11/2017 9:45:01 AM ******/
DROP PROCEDURE [dbo].[SP_IUD_ChequeRequisition]
GO
/****** Object:  StoredProcedure [dbo].[SP_ApprovedChequeRequisition]    Script Date: 1/11/2017 9:45:01 AM ******/
DROP PROCEDURE [dbo].[SP_ApprovedChequeRequisition]
GO
/****** Object:  StoredProcedure [dbo].[SP_ApprovedChequeRequisition]    Script Date: 1/11/2017 9:45:01 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[SP_ApprovedChequeRequisition]
(
	@ChequeRequisitionID as int,
	@DBUserID as int
)
AS 
BEGIN TRAN
	--DECLARE
	--@ChequeRequisitionID as int,
	--@DBUserID as int

	--SET @ChequeRequisitionID = 1
	--SET @DBUserID=-9

	IF NOT EXISTS(SELECT * FROM ChequeRequisition AS HH WHERE HH.ChequeRequisitionID=@ChequeRequisitionID)
	BEGIN
		ROLLBACK
			RAISERROR (N'Invalid Cheque  Requisition Please try again!!~',16,1);	
		RETURN
	END

	IF(ISNULL((SELECT HH.ApprovedBy FROM ChequeRequisition AS HH WHERE HH.ChequeRequisitionID=@ChequeRequisitionID),0) != 0)
	BEGIN
		ROLLBACK
			RAISERROR (N'Your selected cheque requisition already approved!!~',16,1);	
		RETURN
	END


	DECLARE
	@ChequeID as int,
	@ChequeBookID as int,
	@ChequeStatus as smallint,
	@PaymentType as smallint,
	@ChequeNo  as varchar(128),	
	@ChequeDate as datetime,
	@PayTo as int,
	@IssueFigureID as int,
	@Amount as money,
	@VoucherReference as varchar(512),
	@Note as varchar(1024),
	@DBServerDateTime as datetime,
	@BillNo as Varchar(MAX),
	@ChequeHistoryID as int,
	@PreviousStatus as smallint,	
	@CurrentStatus as smallint,	
	@ChangeLog as Varchar(512)


	SET @ChequeID = (SELECT HH.ChequeID FROM ChequeRequisition AS HH WHERE HH.ChequeRequisitionID=@ChequeRequisitionID)
	SET @PaymentType = (SELECT HH.ChequeType FROM ChequeRequisition AS HH WHERE HH.ChequeRequisitionID=@ChequeRequisitionID)
	SET @ChequeDate  = (SELECT HH.ChequeDate FROM ChequeRequisition AS HH WHERE HH.ChequeRequisitionID=@ChequeRequisitionID)
	SET @PayTo =   ISNULL((SELECT TOP 1 MM.ReferenceObjectID FROM ACCostCenter AS MM WHERE MM.ReferenceType IN (1,2) AND MM.ACCostCenterID = (SELECT HH.SubledgerID FROM ChequeRequisition AS HH WHERE HH.ChequeRequisitionID=@ChequeRequisitionID)),0)
	SET @IssueFigureID = (SELECT HH.PayTo FROM ChequeRequisition AS HH WHERE HH.ChequeRequisitionID=@ChequeRequisitionID)
	SET @Amount = (SELECT HH.ChequeAmount FROM ChequeRequisition AS HH WHERE HH.ChequeRequisitionID=@ChequeRequisitionID)


	IF NOT EXISTS(SELECT * FROM Cheque AS HH WHERE HH.ChequeID=@ChequeID)
	BEGIN
		ROLLBACK
			RAISERROR (N'Invalid Cheque  Please try again!!~',16,1);	
		RETURN
	END
	IF NOT EXISTS(SELECT * FROM Contractor AS HH WHERE HH.ContractorID=@PayTo)
	BEGIN
		ROLLBACK
			RAISERROR (N'Invalid Party  Please try again!!~',16,1);	
		RETURN
	END
	IF NOT EXISTS(SELECT * FROM IssueFigure AS HH WHERE HH.ContractorID=@PayTo AND HH.IssueFigureID=@IssueFigureID)
	BEGIN
		ROLLBACK
			RAISERROR (N'Invalid Pay-To Please try again!!~',16,1);	
		RETURN
	END
	IF(ISNULL(@Amount,0)<=0)
	BEGIN
		ROLLBACK
			RAISERROR (N'Invalid Cheque Amount Please try again!!~',16,1);	
		RETURN
	END

	IF(ISNULL((SELECT HH.Amount FROM Cheque AS HH WHERE HH.ChequeID=@ChequeID),0)>0)
	BEGIN
		ROLLBACK
			RAISERROR (N'Your selected Cheque Already Issued Please try again!!~',16,1);	
		RETURN
	END


	SET @BillNo = ''
	SELECT @BillNo = @BillNo + HH.BillNo + ', ' FROM View_ChequeRequisitionDetail AS HH WHERE HH.ChequeRequisitionID = @ChequeRequisitionID
	SET @VoucherReference ='N/A'
	SET @ChequeStatus = 1
	SET @Note = 'Cheque issue with bill no :' + @BillNo
	SET @ChequeHistoryID = (SELECT ISNULL(MAX(ChequeHistoryID),0)+1 FROM ChequeHistory)
	SET @PreviousStatus = (SELECT HH.ChequeStatus FROM Cheque AS HH WHERE HH.ChequeID=@ChequeID)
	SET @CurrentStatus = 2 --EnumChequeStatus: Issued = 2,
	SET @ChequeStatus = 2 --EnumChequeStatus: Issued = 2,
	SET @ChangeLog = 'Cheque issue from cheque requisition by '+(SELECT MM.UserName FROM Users AS MM WHERE MM.UserID=@DBUserID)
	SET @DBServerDateTime = GETDATE()

	UPDATE	Cheque  SET ChequeStatus=@ChequeStatus,	PaymentType=@PaymentType,	ChequeDate=@ChequeDate,	PayTo=@PayTo, IssueFigureID=@IssueFigureID,	Amount=@Amount,	VoucherReference=@VoucherReference,	Note=@Note,	DBUserID=@DBUserID,	DBServerDateTime=@DBServerDateTime WHERE ChequeID= @ChequeID

	INSERT INTO ChequeHistory	(ChequeHistoryID,		ChequeID,		PreviousStatus,		CurrentStatus,	Note,	ChangeLog,	DBUserID,	DBServerDateTime)
					  VALUES	(@ChequeHistoryID,		@ChequeID,		@PreviousStatus,	@CurrentStatus,	@Note,	@ChangeLog,	@DBUserID,	@DBServerDateTime)

	--EnumChequeRequisitionStatus { None = 0, Initialized = 1, Approved = 2, Cancel = 3 }
	UPDATE ChequeRequisition SET RequisitionStatus= 2, ApprovedBy = @DBUserID WHERE ChequeRequisitionID=@ChequeRequisitionID
 
	SELECT * FROM  View_ChequeRequisition AS HH WHERE HH.ChequeRequisitionID=@ChequeRequisitionID
COMMIT TRAN


GO
/****** Object:  StoredProcedure [dbo].[SP_IUD_ChequeRequisition]    Script Date: 1/11/2017 9:45:01 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[SP_IUD_ChequeRequisition]
(
	@ChequeRequisitionID	as int,
	@RequisitionNo	as varchar(512),
	@BUID	as int,
	@RequisitionStatus	as smallint,
	@RequisitionDate	as date,
	@AccountHeadID	as int,
	@SubledgerID	as int,
	@PayTo	as int,
	@ChequeDate	as date,
	@ChequeType	as smallint,
	@BankAccountID	as int,
	@BankBookID	as int,
	@ChequeID	as int,
	@ChequeAmount	as decimal(30, 17),
	@ApprovedBy	as int,
	@Remarks	as varchar(1024),
	@DBUserID as int,
	@DBOperation as smallint
	--%n, %s, %n, %n, %d, %n, %n, %n, %d, %n, %n, %n, %n, %n, %n, %s, %n, %n
)
AS
BEGIN TRAN
DECLARE 
@DBServerDateTime as datetime,
@PV_YearPart as Varchar(100),
@PV_RequisitionNo as Varchar(100),
@PV_NumericPart as int,
@PV_NumericPartInString as varchar(100),
@PV_InitialText as Varchar(2)

SET @DBServerDateTime=Getdate()	
IF(@DBOperation=1)
BEGIN	
	SET @PV_YearPart=SUBSTRING((SELECT DATENAME(YEAR,@RequisitionDate)),3,2)	
	IF EXISTS(SELECT * FROM ChequeRequisition as HH WHERE  YEAR(HH.RequisitionDate) = YEAR(@RequisitionDate))
	BEGIN
		SET @PV_RequisitionNo = (SELECT NN.RequisitionNo FROM ChequeRequisition AS NN WHERE NN.ChequeRequisitionID=(SELECT MAX(HH.ChequeRequisitionID) from ChequeRequisition AS HH WHERE YEAR(HH.RequisitionDate) = YEAR(@RequisitionDate)))		
		SET @PV_NumericPart=CONVERT(int,SUBSTRING(@PV_RequisitionNo,1,5))+1
		SET @PV_NumericPartInString=RIGHT('00000' + CONVERT(VARCHAR(5), @PV_NumericPart), 5)	
		SET @RequisitionNo=@PV_NumericPartInString+'/'+@PV_YearPart	
	END
	ELSE
	BEGIN	
		--00001/17
		SET @RequisitionNo='00001/'+@PV_YearPart --for Reset Number 		
	END		

	SET @ChequeRequisitionID=(SELECT ISNULL(MAX(ChequeRequisitionID),0)+1 FROM ChequeRequisition)
	INSERT INTO ChequeRequisition	(ChequeRequisitionID,	RequisitionNo,	BUID,	RequisitionStatus,	RequisitionDate,	AccountHeadID,	SubledgerID,	PayTo,	ChequeDate,		ChequeType,		BankAccountID,		BankBookID,		ChequeID,	ChequeAmount,	ApprovedBy,		Remarks,	DBUserID,	DBServerDateTime)
							VALUES	(@ChequeRequisitionID,	@RequisitionNo,	@BUID,	@RequisitionStatus,	@RequisitionDate,	@AccountHeadID,	@SubledgerID,	@PayTo,	@ChequeDate,	@ChequeType,	@BankAccountID,		@BankBookID,	@ChequeID,	@ChequeAmount,	@ApprovedBy,	@Remarks,	@DBUserID,	@DBServerDateTime)
	SELECT * FROM View_ChequeRequisition WHERE ChequeRequisitionID=@ChequeRequisitionID
END

IF(@DBOperation=2)
BEGIN
	IF(@ChequeRequisitionID<=0) 
	BEGIN
		ROLLBACK
			RAISERROR (N'Selected ChequeRequisition is not valid. Please try again!!~',16,1);	
		RETURN
	END
	UPDATE ChequeRequisition SET  BUID=@BUID,	RequisitionStatus=@RequisitionStatus,	RequisitionDate=@RequisitionDate,	AccountHeadID=@AccountHeadID,	SubledgerID=@SubledgerID,	PayTo=@PayTo,	ChequeDate=@ChequeDate,		ChequeType=@ChequeType,		BankAccountID=@BankAccountID,		BankBookID=@BankBookID,		ChequeID=@ChequeID,	ChequeAmount=@ChequeAmount,	ApprovedBy=@ApprovedBy,		Remarks=@Remarks WHERE ChequeRequisitionID =@ChequeRequisitionID
	SELECT * FROM View_ChequeRequisition WHERE ChequeRequisitionID=@ChequeRequisitionID
END

IF(@DBOperation=3)
BEGIN
	IF(@ChequeRequisitionID<=0)
	BEGIN
		ROLLBACK
			RAISERROR (N'Selected ChequeRequisition is not valid. Please try again!!~',16,1);	
		RETURN
	END	
	IF((SELECT ApprovedBy FROM ChequeRequisition WHERE ChequeRequisitionID=@ChequeRequisitionID)!=0)
	BEGIN
		ROLLBACK
			RAISERROR (N'Deletion Not Possible. Your Selected ChequeRequisition Already Approved !!~',16,1);	
		RETURN
	END	
	DELETE FROM ChequeRequisitionDetail WHERE ChequeRequisitionID=@ChequeRequisitionID	
	DELETE FROM ChequeRequisition WHERE ChequeRequisitionID=@ChequeRequisitionID
END
COMMIT TRAN







GO
/****** Object:  StoredProcedure [dbo].[SP_IUD_ChequeRequisitionDetail]    Script Date: 1/11/2017 9:45:01 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[SP_IUD_ChequeRequisitionDetail]
(	
	@ChequeRequisitionDetailID	as int,
	@ChequeRequisitionID	as int,
	@VoucherBillID	as int,
	@Amount	as decimal(30, 17),
	@Remarks	as varchar(1012),
	@DBUserID  as int,
	@DBOperation as smallint,
	@ChequeRequisitionDetailIDs as varchar(512)
	--%n, %n, %n, %n, %s, %n, %n, %s
)
AS
BEGIN TRAN
DECLARE 
@DBServerDateTime as datetime,
@PV_OrderType as smallint
SET @DBServerDateTime=Getdate()	
	
IF(@DBOperation=1)
BEGIN	
	SET @ChequeRequisitionDetailID= (SELECT ISNULL(MAX(ChequeRequisitionDetailID),0)+1 FROM ChequeRequisitionDetail)	
	INSERT INTO ChequeRequisitionDetail	(ChequeRequisitionDetailID,		ChequeRequisitionID,	VoucherBillID,	Amount,		Remarks,	DBUserID,	DBServerDateTime)
								VALUES	(@ChequeRequisitionDetailID,	@ChequeRequisitionID,	@VoucherBillID,	@Amount,	@Remarks,	@DBUserID,	@DBServerDateTime)
	SELECT * FROM View_ChequeRequisitionDetail  WHERE ChequeRequisitionDetailID=@ChequeRequisitionDetailID
END
IF(@DBOperation=2)
BEGIN
	IF (@ChequeRequisitionDetailID<0) 
	BEGIN
		ROLLBACK
			RAISERROR (N' Selected ChequeRequisition Detail are Invalid Please try again!!~',16,1);	
		RETURN
	END
	UPDATE ChequeRequisitionDetail SET	ChequeRequisitionID=@ChequeRequisitionID,	VoucherBillID=@VoucherBillID,	Amount=@Amount,		Remarks=@Remarks WHERE ChequeRequisitionDetailID = @ChequeRequisitionDetailID
	SELECT * FROM View_ChequeRequisitionDetail  WHERE ChequeRequisitionDetailID=@ChequeRequisitionDetailID	
END

IF(@DBOperation=3)
BEGIN
	IF (@ChequeRequisitionID<0) 
	BEGIN
		ROLLBACK
			RAISERROR (N' Selected ChequeRequisition are Invalid Please try again!!~',16,1);	
		RETURN
	END
	DELETE FROM ChequeRequisitionDetail WHERE ChequeRequisitionID=@ChequeRequisitionID AND ChequeRequisitionDetailID NOT IN (SELECT * FROM dbo.SplitInToDataSet(@ChequeRequisitionDetailIDs,','))
END
COMMIT TRAN






GO
/****** Object:  StoredProcedure [dbo].[SP_IUD_Voucher]    Script Date: 1/11/2017 9:45:01 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[SP_IUD_Voucher]
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
	SET @TempVoucherTypeID = (SELECT HH.VoucherTypeID FROM Voucher AS HH WHERE HH.VoucherID=@VoucherID)
	IF(MONTH(@TempVoucherDate)!=MONTH(@VoucherDate) OR @TempVoucherTypeID != @VoucherTypeID)
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





GO
/****** Object:  Table [dbo].[ChequeRequisition]    Script Date: 1/11/2017 9:45:01 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[ChequeRequisition](
	[ChequeRequisitionID] [int] NOT NULL,
	[RequisitionNo] [varchar](512) NULL,
	[BUID] [int] NULL,
	[RequisitionStatus] [smallint] NULL,
	[RequisitionDate] [date] NULL,
	[AccountHeadID] [int] NULL,
	[SubledgerID] [int] NULL,
	[PayTo] [int] NULL,
	[ChequeDate] [date] NULL,
	[ChequeType] [smallint] NULL,
	[BankAccountID] [int] NULL,
	[BankBookID] [int] NULL,
	[ChequeID] [int] NULL,
	[ChequeAmount] [decimal](30, 17) NULL,
	[ApprovedBy] [int] NULL,
	[Remarks] [varchar](1024) NULL,
	[DBUserID] [int] NULL,
	[DBServerDateTime] [datetime] NULL,
 CONSTRAINT [PK_ChequeRequisition] PRIMARY KEY CLUSTERED 
(
	[ChequeRequisitionID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[ChequeRequisitionDetail]    Script Date: 1/11/2017 9:45:01 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[ChequeRequisitionDetail](
	[ChequeRequisitionDetailID] [int] NOT NULL,
	[ChequeRequisitionID] [int] NULL,
	[VoucherBillID] [int] NULL,
	[Amount] [decimal](30, 17) NULL,
	[Remarks] [varchar](1012) NULL,
	[DBUserID] [int] NULL,
	[DBServerDateTime] [datetime] NULL,
 CONSTRAINT [PK_ChequeRequisitionDetail] PRIMARY KEY CLUSTERED 
(
	[ChequeRequisitionDetailID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  View [dbo].[View_VoucherBill]    Script Date: 1/11/2017 9:45:01 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE VIEW [dbo].[View_VoucherBill]
AS
SELECT	VoucherBill.VoucherBillID,
		VoucherBill.AccountHeadID,
		VoucherBill.SubLedgerID,
		VoucherBill.BUID,
		VoucherBill.BillNo,
		VoucherBill.BillDate,
		VoucherBill.DueDate,
		VoucherBill.CreditDays,
		VoucherBill.Amount,
		VoucherBill.IsActive,
		VoucherBill.CurrencyID,
		VoucherBill.CurrencyRate,
		VoucherBill.CurrencyAmount,
		VoucherBill.ReferenceType,
		VoucherBill.ReferenceObjID,
		VoucherBill.OpeningBillAmount,
		VoucherBill.OpeningBillDate,
		VoucherBill.Remarks,		
		VoucherBill.IsHoldBill,
		dbo.FN_GetVoucherBillBalance(VoucherBill.VoucherBillID,VoucherBill.AccountHeadID, 1)  AS RemainningBalance,
		CONVERT(bit,dbo.FN_GetVoucherBillBalance(VoucherBill.VoucherBillID,VoucherBill.AccountHeadID, 0))  AS IsDebit,
		DATEDIFF(DAY,VoucherBill.DueDate,GETDATE()) AS OverDueDays,
		DATEDIFF(DAY,GETDATE(),VoucherBill.DueDate) AS DueDays,
		(SELECT C.BaseCurrencyID FROM Company AS C WHERE CompanyID=1) AS BaseCurrencyID,
		(SELECT Symbol FROM Currency  WHERE CurrencyID IN (SELECT BaseCurrencyID FROM Company WHERE CompanyID=1)) AS BaseCurrencySymbol,
		(SELECT Symbol FROM Currency WHERE CurrencyID=VoucherBill.CurrencyID) AS CurrencySymbol,		
		COA.AccountHeadName,
		COA.AccountCode,
		COA.AccountHeadCodeName,
		ACC.Name AS SubLedgerName,
		ACC.Code AS SubLedgerCode,
		ACC.NameCode AS SubLedgerNameCode,
		BU.Code AS BUCode,
		BU.Name AS BUName,
		BU.ShortName AS BUShortName,
		(VoucherBill.Amount - ISNULL((SELECT SUM(HH.Amount) FROM ChequeRequisitionDetail AS HH WHERE HH.VoucherBillID=VoucherBill.VoucherBillID),0))  AS DueCheque

FROM VoucherBill
LEFT OUTER JOIN View_ChartsOfAccount AS COA ON COA.AccountHeadID=VoucherBill.AccountHeadID
LEFT OUTER JOIN View_ACCostCenter AS ACC ON ACC.ACCostCenterID=VoucherBill.SubLedgerID
LEFT OUTER JOIN BusinessUnit AS BU ON BU.BusinessUnitID=VoucherBill.BUID










GO
/****** Object:  View [dbo].[View_ChequeRequisitionDetail]    Script Date: 1/11/2017 9:45:01 AM ******/
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
		VB.Amount AS BillAmount,
		VB.RemainningBalance

FROM			ChequeRequisitionDetail  AS CRD
LEFT OUTER JOIN	View_VoucherBill AS VB ON CRD.VoucherBillID = VB.VoucherBillID








GO
/****** Object:  View [dbo].[View_ChequeRequisition]    Script Date: 1/11/2017 9:45:01 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE VIEW [dbo].[View_ChequeRequisition]
AS
SELECT	CR.ChequeRequisitionID,
		CR.RequisitionNo,
		CR.BUID,
		CR.RequisitionStatus,
		CR.RequisitionDate,
		CR.AccountHeadID,
		CR.SubledgerID,
		CR.PayTo,
		CR.ChequeDate,
		CR.ChequeType,
		CR.BankAccountID,
		CR.BankBookID,
		CR.ChequeID,
		CR.ChequeAmount,
		CR.ApprovedBy,
		CR.Remarks,
		BusinessUnit.Name AS BUName,
		BusinessUnit.Code AS BUCode,
		COA.AccountCode, 
		COA.AccountHeadName,
		CC.Name AS SubledgerName,		
		CC.Code AS SubledgerCode,
		IssueFigure.ChequeIssueTo,
		IssueFigure.SecondLineIssueTo,
		BAC.AccountNo,
		BAC.BankName, 
		BAC.BranchName, 
		ChequeBook.BookCodePartOne+'-'+ChequeBook.BookCodePartTwo AS BookCode,
		Cheque.ChequeNo,
		Cheque.ChequeStatus,
		View_User.UserName AS ApprovedByName
		
FROM		ChequeRequisition AS CR
LEFT OUTER JOIN	BusinessUnit ON CR.BUID = BusinessUnit.BusinessUnitID
LEFT OUTER JOIN	ChartsOfAccount AS COA ON CR.AccountHeadID = COA.AccountHeadID 
LEFT OUTER JOIN	ACCostCenter AS CC ON CR.SubledgerID = CC.ACCostCenterID
LEFT OUTER JOIN	IssueFigure ON CR.PayTo = IssueFigure.IssueFigureID
LEFT OUTER JOIN	View_BankAccount AS BAC ON CR.BankAccountID = BAC.BankAccountID
LEFT OUTER JOIN	ChequeBook ON CR.BankBookID = ChequeBook.ChequeBookID
LEFT OUTER JOIN	Cheque ON CR.ChequeID = Cheque.ChequeID
LEFT OUTER JOIN	View_User ON CR.ApprovedBy = View_User.UserID

GO
