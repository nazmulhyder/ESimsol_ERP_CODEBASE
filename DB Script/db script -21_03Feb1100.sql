GO
/****** Object:  View [dbo].[View_BankReconciliation]    Script Date: 1/30/2017 4:17:56 PM ******/
DROP VIEW [dbo].[View_BankReconciliation]
GO
/****** Object:  UserDefinedFunction [dbo].[FN_GetsRevarseHead]    Script Date: 1/30/2017 4:17:56 PM ******/
DROP FUNCTION [dbo].[FN_GetsRevarseHead]
GO
/****** Object:  StoredProcedure [dbo].[SP_IUD_Contractor]    Script Date: 1/30/2017 4:17:56 PM ******/
DROP PROCEDURE [dbo].[SP_IUD_Contractor]
GO
/****** Object:  StoredProcedure [dbo].[SP_IUD_Contractor]    Script Date: 1/30/2017 4:17:56 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[SP_IUD_Contractor]
(
	@ContractorID	as int,
	@Name	as varchar(255),
	@Origin	as varchar(50),
	@Address	as varchar(500),
	@Address2 as varchar(512),
	@Address3 as varchar(512),
	@Phone	as varchar(200),
	@Phone2 as varchar(512),
	@Email	as varchar(200),
	@ShortName	as varchar(127),
	@Fax	as varchar(128),
	@ActualBalance	as decimal(30, 17),
	@Note	as varchar(512),
	@Activity as bit,
	@TIN	as varchar(512),
	@VAT 	as varchar(512),
	@GroupName	as varchar(512),
	@Zone as varchar(512),
	@DBUserID	as int,
	@DBOperation as smallint
	--%n, %n, %s, %s, %s,TS, %s, %s,%s, %s, %s, %n, %s,%b,%s,%s,%s, %n, %n
)		
AS
BEGIN TRAN
DECLARE 
@DBServerDateTime as datetime,
@IssueFigureID	as int,
@ChequeIssueTo	as nvarchar(150),
@SecondLineIssueTo	as nvarchar(150),
@DetailNote	as nvarchar(250),
@IsActive	as bit

SET @DBServerDateTime=Getdate()
IF(@DBOperation=1)
BEGIN
		IF EXISTS(SELECT * FROM Contractor WHERE Name=@Name )
		BEGIN
			ROLLBACK
				RAISERROR (N'Sorry, Contractor with this name already exists,Try another one.~!!',16,1);	
			RETURN
		END				
		SET @ContractorID=(SELECT ISNULL(MAX(ContractorID),0)+1 FROM Contractor)
		INSERT INTO Contractor	(ContractorID,			Name,	Origin,		[Address],	Address2 ,Address3,	Phone,	Phone2, Email,		ShortName,	Fax,	ActualBalance,	TIN, VAT,	GroupName,	Note,	Zone,Activity,	DBUserID,	DBServerDateTime)
    					VALUES	(@ContractorID,		@Name,	@Origin,	@Address,	@Address2, @Address3,	@Phone,	@Phone2, @Email,	@ShortName,	@Fax,	@ActualBalance,	@TIN, @VAT,	@GroupName,	@Note,	@Zone,	@Activity, @DBUserID,	@DBServerDateTime)    					    				  

		--Insert Issue Figure
		SET @IssueFigureID=(SELECT ISNULL(MAX(IssueFigureID),0)+1 FROM IssueFigure)
		SET @ChequeIssueTo = @Name
		SET @SecondLineIssueTo	=''
		SET @DetailNote	='N/A'
		SET @IsActive	=1
		INSERT INTO IssueFigure	(IssueFigureID,		ContractorID,	ChequeIssueTo,	SecondLineIssueTo,	DetailNote,		IsActive,	DBUserID,	DBDateTime,				LastUpdateBy,	LastUpdateDateTime)
    				VALUES		(@IssueFigureID,	@ContractorID,	@ChequeIssueTo,	@SecondLineIssueTo,	@DetailNote,	@IsActive,	@DBUserID,	@DBServerDateTime,		@DBUserID,		@DBServerDateTime)

    	SELECT * FROM Contractor  WHERE ContractorID= @ContractorID
END

IF(@DBOperation=2)
BEGIN
	IF(@ContractorID<=0)
	BEGIN
		ROLLBACK
			RAISERROR (N'Your Selected Contractor Is Invalid Please Refresh and try again!!',16,1);	
		RETURN
	END
	IF EXISTS(SELECT * FROM Contractor WHERE Name=@Name  AND ContractorID!=@ContractorID)
		BEGIN
			ROLLBACK
				RAISERROR (N'Sorry, Contractor with this name already exists,Try another one.~!!',16,1);	
			RETURN
		END	
	Update Contractor   SET 	Name=@Name,	Origin=@Origin,		[Address]=@Address, Address2 =@Address2,Address3=@Address3,	Phone=@Phone, Phone2 = @Phone2,	Email=@Email,	ShortName=@ShortName,	Fax=@Fax,		TIN = @TIN, VAT=@VAT,	GroupName=@GroupName,	Note=@Note, Activity =Activity,Zone=@Zone,	DBUserID=@DBUserID,	DBServerDateTime=@DBServerDateTime WHERE ContractorID= @ContractorID
	SELECT * FROM Contractor  WHERE ContractorID= @ContractorID
END

IF(@DBOperation=3)
BEGIN
	IF(@ContractorID<=0)
	BEGIN
		ROLLBACK
			RAISERROR (N'Your Selected Contractor Is Invalid Please Refresh and try again!!',16,1);	
		RETURN
	END		
	
	IF EXISTS(SELECT * FROM ContactPersonnel WHERE ContractorID=@ContractorID)
	BEGIN
		ROLLBACK
			RAISERROR (N'Deletion Not Possible, Contact Personnel Reference May be Loss!!',16,1);	
		RETURN
	END	
	--IF EXISTS(SELECT top(1)* FROM PurchaseInvoice WHERE ContractorID=@ContractorID)
	--BEGIN
	--	ROLLBACK
	--		RAISERROR (N'Deletion Not Possible, Invoice Reference May be Loss!!',16,1);	
	--	RETURN
	--END	
	DELETE FROM BUWiseParty  WHERE ContractorID= @ContractorID
	DELETE FROM ContractorType  WHERE ContractorID= @ContractorID
	DELETE FROM Contractor  WHERE ContractorID= @ContractorID
	
END
COMMIT TRAN













GO
/****** Object:  UserDefinedFunction [dbo].[FN_GetsRevarseHead]    Script Date: 1/30/2017 4:17:56 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE FUNCTION [dbo].[FN_GetsRevarseHead] 
(
	@VoucherID as int,
	@IsDebit as bit
)
RETURNS VARCHAR(MAX)
AS
BEGIN
--DECLARE
--@VoucherID as int,
--@IsDebit as bit

--SET @VoucherID = 99
--SET @IsDebit =0

DECLARE
@ReturnData as VARCHAR(MAX)
SET @ReturnData = ''

DECLARE @TempTable AS TABLE (RevarseHeadName Varchar(512), Amount decimal(30,17), Currency Varchar(512), VoucherDetailID int, IsLedger bit)
INSERT INTO @TempTable (RevarseHeadName,	Amount,		Currency,			VoucherDetailID,	IsLedger)
				SELECT	MM.CostCenterName,	MM.Amount,	MM.CurrencySymbol,	MM.VoucherDetailID,	0 FROM View_CostCenterTransaction AS MM WHERE MM.VoucherDetailID IN (SELECT HH.VoucherDetailID FROM View_VoucherDetail AS HH WHERE  IsDebit!=@IsDebit AND  VoucherID=@VoucherID)

INSERT INTO @TempTable (RevarseHeadName,	Amount,					Currency,			VoucherDetailID,	IsLedger)
				SELECT HH.AccountHeadName,	HH.AmountInCurrency,	HH.CUSymbol,		HH.VoucherDetailID, 1 FROM View_VoucherDetail AS HH WHERE  HH.IsDebit!=@IsDebit AND  HH.VoucherID=@VoucherID AND HH.VoucherDetailID NOT IN (SELECT MM.VoucherDetailID FROM @TempTable AS MM)

--SELECT * FROM @TempTable
SELECT @ReturnData = @ReturnData +  ISNULL(HH.RevarseHeadName,'') + ' @ '+ ISNULL(HH.Currency,'')+ ' ' + CONVERT(VARCHAR, CAST(SUM(ISNULL(HH.Amount,0.00)) AS MONEY), 1) + '; ' FROM @TempTable AS HH GROUP BY RevarseHeadName, Currency, IsLedger ORDER BY IsLedger ASC
SET @ReturnData = ISNULL(@ReturnData,'')

IF(LEN(@ReturnData)>0)
BEGIN
	SET @ReturnData= SUBSTRING(@ReturnData, 0, LEN(@ReturnData))
END
--SELECT @ReturnData
RETURN @ReturnData
END



GO
/****** Object:  View [dbo].[View_BankReconciliation]    Script Date: 1/30/2017 4:17:56 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE VIEW [dbo].[View_BankReconciliation]
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
