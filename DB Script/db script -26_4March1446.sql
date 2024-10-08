GO
/****** Object:  View [dbo].[View_VoucherDetailReverse]    Script Date: 3/2/2017 1:53:03 PM ******/
DROP VIEW [dbo].[View_VoucherDetailReverse]
GO
/****** Object:  View [dbo].[View_CashFlowSetup]    Script Date: 3/2/2017 1:53:03 PM ******/
DROP VIEW [dbo].[View_CashFlowSetup]
GO
/****** Object:  View [dbo].[View_CashFlowDmSetup]    Script Date: 3/2/2017 1:53:03 PM ******/
DROP VIEW [dbo].[View_CashFlowDmSetup]
GO
/****** Object:  Table [dbo].[CashFlowDmSetup]    Script Date: 3/2/2017 1:53:03 PM ******/
DROP TABLE [dbo].[CashFlowDmSetup]
GO
/****** Object:  UserDefinedFunction [dbo].[FN_GetsReverseAccounts]    Script Date: 3/2/2017 1:53:03 PM ******/
DROP FUNCTION [dbo].[FN_GetsReverseAccounts]
GO
/****** Object:  StoredProcedure [dbo].[SP_IUD_CashFlowSetup]    Script Date: 3/2/2017 1:53:03 PM ******/
DROP PROCEDURE [dbo].[SP_IUD_CashFlowSetup]
GO
/****** Object:  StoredProcedure [dbo].[SP_IUD_CashFlowDmSetup]    Script Date: 3/2/2017 1:53:03 PM ******/
DROP PROCEDURE [dbo].[SP_IUD_CashFlowDmSetup]
GO
/****** Object:  StoredProcedure [dbo].[SP_IncomeStatement]    Script Date: 3/2/2017 1:53:03 PM ******/
DROP PROCEDURE [dbo].[SP_IncomeStatement]
GO
/****** Object:  StoredProcedure [dbo].[SP_CashFlowStatement]    Script Date: 3/2/2017 1:53:03 PM ******/
DROP PROCEDURE [dbo].[SP_CashFlowStatement]
GO
/****** Object:  StoredProcedure [dbo].[SP_CashFlowDmStatement]    Script Date: 3/2/2017 1:53:03 PM ******/
DROP PROCEDURE [dbo].[SP_CashFlowDmStatement]
GO
/****** Object:  StoredProcedure [dbo].[SP_CashFlowDmStatement]    Script Date: 3/2/2017 1:53:03 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[SP_CashFlowDmStatement]
(
	@BUID as int,
	@StartDate as date,
	@EndDate as date,
	@IsDetailsView as bit
)
AS
BEGIN TRAN
--DECLARE
--@BUID as int,
--@StartDate as date,
--@EndDate as date

--SET @BUID=0
--SET @StartDate ='01 Jul 2016'
--SET @EndDate = '30 Jun 2017'

DECLARE
@BUCode as Varchar(512),
@ComponentIDs as Varchar(512),
@SessionID as int,
@SessionStartDate as date,
@SessionEndDate as date,
@CurrencyID as int


SET @BUCode=ISNULL((SELECT BU.Code FROM BusinessUnit AS BU WHERE BU.BusinessUnitID=@BUID),'00')
SET @ComponentIDs='2,3,4,5,6'
SET @CurrencyID = (SELECT TT.BaseCurrencyID FROM Company AS TT WHERE TT.CompanyID=1)
--ComponentID{AsSET = 2,Laibility = 3,OwnerEquity=4,Income = 5,Expeness = 6}

IF EXISTS(SELECT ASE.SessionID FROM AccountingSession AS ASE WHERE  CONVERT(DATE,CONVERT(VARCHAR(12),ASE.StartDate,106))=CONVERT(DATE,CONVERT(VARCHAR(12),@StartDate,106)) AND ASE.SessionType=6)
BEGIN
	SET @SessionID=(SELECT top(1)ASE.SessionID FROM AccountingSession AS ASE WHERE  CONVERT(DATE,CONVERT(VARCHAR(12),ASE.StartDate,106))=CONVERT(DATE,CONVERT(VARCHAR(12),@StartDate,106)) AND ASE.SessionType=6)
END
ELSE
BEGIN
	SET @SessionID=(SELECT TOP 1 ASE.SessionID FROM AccountingSession AS ASE WHERE  CONVERT(DATE,CONVERT(VARCHAR(12),ASE.StartDate,106))>CONVERT(DATE,CONVERT(VARCHAR(12),@StartDate,106)) AND ASE.SessionType=6)	
END
SET @SessionStartDate=(SELECT ASE.StartDate FROM AccountingSession AS ASE WHERE  ASE.AccountingSessionID=@SessionID)
SET @SessionEndDate=DATEADD(DAY, -1, @StartDate)

CREATE TABLE #Temp_Table(		
							AccountHeadID int,
							AccountCode varchar(128),
							AccountHeadName varchar(512),
							ParentAccountHeadID int,
							ComponentType smallint,
							AccountType smallint,							
							OpenningBalance decimal(30,17),
							DebitTransaction decimal(30,17),
							CreditTransaction decimal(30,17),							
							ClosingBalance decimal(30,17),
							EDDebitTransaction decimal(30,17),
							EDCreditTransaction decimal(30,17),							
							EDClosingBalance decimal(30,17),
							ChangesAmount decimal(30,17)
						)

CREATE NONCLUSTERED INDEX #IX_Temp2_1 ON #Temp_Table(AccountHeadID)

INSERT INTO #Temp_Table		(	AccountHeadID,		AccountCode,			AccountHeadName,		ParentAccountHeadID,	AccountType)
					SELECT		AccountHeadID,		AccountCode,			AccountHeadName,		ParentHeadID,			AccountType FROM dbo.GetAccountHeadByComponents(@ComponentIDs)

UPDATE #Temp_Table SET	ComponentType=dbo.GetComponentID(TT.AccountHeadID) FROM #Temp_Table AS TT 

--For Balance Sheet
--EnumAccountType{None = 0,Component = 1,Segment =2,Group = 3,SubGroup = 4,Ledger = 5}
--Update ledger transaction, ComponentID, Opening Balance
IF(@BUID>0)
BEGIN
	UPDATE #Temp_Table
	SET	ComponentType=dbo.GetComponentID(TT.AccountHeadID),
		OpenningBalance= ISNULL((SELECT LOB.OpenningBalance FROM dbo.[GetLedgerOpeningBalanceByBU](TT.AccountHeadID, @StartDate, @CurrencyID, 1, 0, @BUCode, '00' ) AS LOB),0),	
		DebitTransaction=(SELECT ISNULL(SUM(VD.Amount),0) FROM View_VoucherDetail AS VD  WHERE  VD.IsDebit=1 AND VD.BUID=@BUID AND VD.AccountHeadID=TT.AccountHeadID AND VD.VoucherID IN(SELECT V.VoucherID FROM Voucher AS V WHERE ISNULL(V.AuthorizedBy,0)!=0 AND CONVERT(DATE,CONVERT(VARCHAR(12),V.VoucherDate)) BETWEEN CONVERT(DATE,CONVERT(VARCHAR(12),@SessionStartDate))  AND CONVERT(DATE,CONVERT(VARCHAR(12),@SessionEndDate)))),
		CreditTransaction=(SELECT ISNULL(SUM(VD.Amount),0) FROM View_VoucherDetail AS VD  WHERE  VD.IsDebit=0 AND VD.BUID=@BUID AND VD.AccountHeadID=TT.AccountHeadID AND VD.VoucherID IN(SELECT V.VoucherID FROM Voucher AS V WHERE ISNULL(V.AuthorizedBy,0)!=0 AND CONVERT(DATE,CONVERT(VARCHAR(12),V.VoucherDate)) BETWEEN CONVERT(DATE,CONVERT(VARCHAR(12),@SessionStartDate))  AND CONVERT(DATE,CONVERT(VARCHAR(12),@SessionEndDate)))),
		ClosingBalance=0,
		EDDebitTransaction=(SELECT ISNULL(SUM(VD.Amount),0) FROM View_VoucherDetail AS VD  WHERE  VD.IsDebit=1 AND VD.BUID=@BUID AND VD.AccountHeadID=TT.AccountHeadID AND VD.VoucherID IN(SELECT V.VoucherID FROM Voucher AS V WHERE ISNULL(V.AuthorizedBy,0)!=0 AND CONVERT(DATE,CONVERT(VARCHAR(12),V.VoucherDate)) BETWEEN CONVERT(DATE,CONVERT(VARCHAR(12),@StartDate))  AND CONVERT(DATE,CONVERT(VARCHAR(12),@EndDate)))),
		EDCreditTransaction=(SELECT ISNULL(SUM(VD.Amount),0) FROM View_VoucherDetail AS VD  WHERE  VD.IsDebit=0 AND VD.BUID=@BUID AND VD.AccountHeadID=TT.AccountHeadID AND VD.VoucherID IN(SELECT V.VoucherID FROM Voucher AS V WHERE ISNULL(V.AuthorizedBy,0)!=0 AND CONVERT(DATE,CONVERT(VARCHAR(12),V.VoucherDate)) BETWEEN CONVERT(DATE,CONVERT(VARCHAR(12),@StartDate))  AND CONVERT(DATE,CONVERT(VARCHAR(12),@EndDate)))),
		EDClosingBalance=0
	FROM #Temp_Table AS TT WHERE TT.AccountType=5 AND TT.ComponentType IN (2,3,4)
END
ELSE
BEGIN
	UPDATE #Temp_Table
	SET	ComponentType=dbo.GetComponentID(TT.AccountHeadID),
		OpenningBalance= ISNULL((SELECT LOB.OpenningBalance FROM dbo.[GetLedgerOpeningBalance](TT.AccountHeadID, @StartDate, @CurrencyID, 1, 0, 0) AS LOB),0),	
		DebitTransaction=(SELECT ISNULL(SUM(VD.Amount),0) FROM View_VoucherDetail AS VD  WHERE  VD.IsDebit=1 AND VD.AccountHeadID=TT.AccountHeadID AND VD.VoucherID IN(SELECT V.VoucherID FROM Voucher AS V WHERE ISNULL(V.AuthorizedBy,0)!=0 AND CONVERT(DATE,CONVERT(VARCHAR(12),V.VoucherDate)) BETWEEN CONVERT(DATE,CONVERT(VARCHAR(12),@SessionStartDate))  AND CONVERT(DATE,CONVERT(VARCHAR(12),@SessionEndDate)))),
		CreditTransaction=(SELECT ISNULL(SUM(VD.Amount),0) FROM View_VoucherDetail AS VD  WHERE  VD.IsDebit=0 AND VD.AccountHeadID=TT.AccountHeadID AND VD.VoucherID IN(SELECT V.VoucherID FROM Voucher AS V WHERE ISNULL(V.AuthorizedBy,0)!=0 AND CONVERT(DATE,CONVERT(VARCHAR(12),V.VoucherDate)) BETWEEN CONVERT(DATE,CONVERT(VARCHAR(12),@SessionStartDate))  AND CONVERT(DATE,CONVERT(VARCHAR(12),@SessionEndDate)))),
		ClosingBalance=0,
		EDDebitTransaction=(SELECT ISNULL(SUM(VD.Amount),0) FROM View_VoucherDetail AS VD  WHERE  VD.IsDebit=1 AND VD.AccountHeadID=TT.AccountHeadID AND VD.VoucherID IN(SELECT V.VoucherID FROM Voucher AS V WHERE ISNULL(V.AuthorizedBy,0)!=0 AND CONVERT(DATE,CONVERT(VARCHAR(12),V.VoucherDate)) BETWEEN CONVERT(DATE,CONVERT(VARCHAR(12),@StartDate))  AND CONVERT(DATE,CONVERT(VARCHAR(12),@EndDate)))),
		EDCreditTransaction=(SELECT ISNULL(SUM(VD.Amount),0) FROM View_VoucherDetail AS VD  WHERE  VD.IsDebit=0 AND VD.AccountHeadID=TT.AccountHeadID AND VD.VoucherID IN(SELECT V.VoucherID FROM Voucher AS V WHERE ISNULL(V.AuthorizedBy,0)!=0 AND CONVERT(DATE,CONVERT(VARCHAR(12),V.VoucherDate)) BETWEEN CONVERT(DATE,CONVERT(VARCHAR(12),@StartDate))  AND CONVERT(DATE,CONVERT(VARCHAR(12),@EndDate)))),
		EDClosingBalance=0
	FROM #Temp_Table AS TT WHERE TT.AccountType=5 AND TT.ComponentType IN (2,3,4)
END

--
--EnumAccountType{None = 0,Component = 1,Segment =2,Group = 3,SubGroup = 4,Ledger = 5}
--Update ledger transaction, ComponentID, Openning Balance
--OperationType 1 means FreshVoucher
IF(@BUID>0)
BEGIN
	UPDATE #Temp_Table
	SET	OpenningBalance=0,
		DebitTransaction=(SELECT ISNULL(SUM(VD.Amount),0) FROM View_VoucherDetail AS VD WHERE VD.BUID=@BUID AND VD.OperationType=1 AND VD.IsDebit=1 AND VD.AccountHeadID=TT.AccountHeadID AND VD.VoucherID IN(SELECT V.VoucherID FROM Voucher AS V WHERE V.AuthorizedBy!=0 AND CONVERT(DATE,CONVERT(VARCHAR(12),VoucherDate,106)) BETWEEN CONVERT(DATE,CONVERT(VARCHAR(12),@StartDate,106)) AND CONVERT(DATE,CONVERT(VARCHAR(12),@EndDate,106)))),
		CreditTransaction=(SELECT ISNULL(SUM(VD.Amount),0) FROM View_VoucherDetail AS VD WHERE VD.BUID=@BUID AND VD.OperationType=1 AND VD.IsDebit=0 AND VD.AccountHeadID=TT.AccountHeadID AND VD.VoucherID IN(SELECT V.VoucherID FROM Voucher AS V WHERE V.AuthorizedBy!=0 AND CONVERT(DATE,CONVERT(VARCHAR(12),VoucherDate,106)) BETWEEN CONVERT(DATE,CONVERT(VARCHAR(12),@StartDate,106)) AND CONVERT(DATE,CONVERT(VARCHAR(12),@EndDate,106)))),
		ClosingBalance=0
	FROM #Temp_Table AS TT WHERE TT.AccountType=5 AND TT.ComponentType IN (5,6)
END
ELSE
BEGIN
	UPDATE #Temp_Table
	SET	OpenningBalance=0,
		DebitTransaction=(SELECT ISNULL(SUM(VD.Amount),0) FROM View_VoucherDetail AS VD WHERE VD.OperationType=1 AND VD.IsDebit=1 AND VD.AccountHeadID=TT.AccountHeadID AND VD.VoucherID IN(SELECT V.VoucherID FROM Voucher AS V WHERE V.AuthorizedBy!=0 AND CONVERT(DATE,CONVERT(VARCHAR(12),VoucherDate,106)) BETWEEN CONVERT(DATE,CONVERT(VARCHAR(12),@StartDate,106)) AND CONVERT(DATE,CONVERT(VARCHAR(12),@EndDate,106)))),
		CreditTransaction=(SELECT ISNULL(SUM(VD.Amount),0) FROM View_VoucherDetail AS VD WHERE VD.OperationType=1 AND VD.IsDebit=0 AND VD.AccountHeadID=TT.AccountHeadID AND VD.VoucherID IN(SELECT V.VoucherID FROM Voucher AS V WHERE V.AuthorizedBy!=0 AND CONVERT(DATE,CONVERT(VARCHAR(12),VoucherDate,106)) BETWEEN CONVERT(DATE,CONVERT(VARCHAR(12),@StartDate,106)) AND CONVERT(DATE,CONVERT(VARCHAR(12),@EndDate,106)))),
		ClosingBalance=0
	FROM #Temp_Table AS TT WHERE TT.AccountType=5 AND TT.ComponentType IN (5,6)
END

--Update SubGroup debit/credit Transaction
UPDATE #Temp_Table
SET OpenningBalance=(SELECT ISNULL(SUM(ABC.OpenningBalance),0) FROM #Temp_Table AS ABC WHERE ABC.ParentAccountHeadID=TT.AccountHeadID),
	DebitTransaction=(SELECT ISNULL(SUM(ABC.DebitTransaction),0) FROM #Temp_Table AS ABC WHERE ABC.ParentAccountHeadID=TT.AccountHeadID),
	CreditTransaction=(SELECT ISNULL(SUM(ABC.CreditTransaction),0) FROM #Temp_Table AS ABC WHERE ABC.ParentAccountHeadID=TT.AccountHeadID),
	EDDebitTransaction=(SELECT ISNULL(SUM(ABC.EDDebitTransaction),0) FROM #Temp_Table AS ABC WHERE ABC.ParentAccountHeadID=TT.AccountHeadID),
	EDCreditTransaction=(SELECT ISNULL(SUM(ABC.EDCreditTransaction),0) FROM #Temp_Table AS ABC WHERE ABC.ParentAccountHeadID=TT.AccountHeadID)
FROM #Temp_Table AS TT WHERE TT.AccountType=4 --EnumAccountType{None = 0,Component = 1,Segment =2,Group = 3,SubGroup = 4,Ledger = 5}

--Update Group debit/credit Transaction
UPDATE #Temp_Table
SET OpenningBalance=(SELECT ISNULL(SUM(ABC.OpenningBalance),0) FROM #Temp_Table AS ABC WHERE ABC.ParentAccountHeadID=TT.AccountHeadID),
	DebitTransaction=(SELECT ISNULL(SUM(ABC.DebitTransaction),0) FROM #Temp_Table AS ABC WHERE ABC.ParentAccountHeadID=TT.AccountHeadID),
	CreditTransaction=(SELECT ISNULL(SUM(ABC.CreditTransaction),0) FROM #Temp_Table AS ABC WHERE ABC.ParentAccountHeadID=TT.AccountHeadID),
	EDDebitTransaction=(SELECT ISNULL(SUM(ABC.EDDebitTransaction),0) FROM #Temp_Table AS ABC WHERE ABC.ParentAccountHeadID=TT.AccountHeadID),
	EDCreditTransaction=(SELECT ISNULL(SUM(ABC.EDCreditTransaction),0) FROM #Temp_Table AS ABC WHERE ABC.ParentAccountHeadID=TT.AccountHeadID)
FROM #Temp_Table AS TT WHERE TT.AccountType=3 --EnumAccountType{None = 0,Component = 1,Segment =2,Group = 3,SubGroup = 4,Ledger = 5}

--Update Segment debit/credit Transaction
UPDATE #Temp_Table
SET OpenningBalance=(SELECT ISNULL(SUM(ABC.OpenningBalance),0) FROM #Temp_Table AS ABC WHERE ABC.ParentAccountHeadID=TT.AccountHeadID),
	DebitTransaction=(SELECT ISNULL(SUM(ABC.DebitTransaction),0) FROM #Temp_Table AS ABC WHERE ABC.ParentAccountHeadID=TT.AccountHeadID),
	CreditTransaction=(SELECT ISNULL(SUM(ABC.CreditTransaction),0) FROM #Temp_Table AS ABC WHERE ABC.ParentAccountHeadID=TT.AccountHeadID),
	EDDebitTransaction=(SELECT ISNULL(SUM(ABC.EDDebitTransaction),0) FROM #Temp_Table AS ABC WHERE ABC.ParentAccountHeadID=TT.AccountHeadID),
	EDCreditTransaction=(SELECT ISNULL(SUM(ABC.EDCreditTransaction),0) FROM #Temp_Table AS ABC WHERE ABC.ParentAccountHeadID=TT.AccountHeadID)
FROM #Temp_Table AS TT WHERE TT.AccountType=2 --EnumAccountType{None = 0,Component = 1,Segment =2,Group = 3,SubGroup = 4,Ledger = 5}

--Update Component debit/credit Transaction
UPDATE #Temp_Table
SET OpenningBalance=(SELECT ISNULL(SUM(ABC.OpenningBalance),0) FROM #Temp_Table AS ABC WHERE ABC.ParentAccountHeadID=TT.AccountHeadID),
	DebitTransaction=(SELECT ISNULL(SUM(ABC.DebitTransaction),0) FROM #Temp_Table AS ABC WHERE ABC.ParentAccountHeadID=TT.AccountHeadID),
	CreditTransaction=(SELECT ISNULL(SUM(ABC.CreditTransaction),0) FROM #Temp_Table AS ABC WHERE ABC.ParentAccountHeadID=TT.AccountHeadID),
	EDDebitTransaction=(SELECT ISNULL(SUM(ABC.EDDebitTransaction),0) FROM #Temp_Table AS ABC WHERE ABC.ParentAccountHeadID=TT.AccountHeadID),
	EDCreditTransaction=(SELECT ISNULL(SUM(ABC.EDCreditTransaction),0) FROM #Temp_Table AS ABC WHERE ABC.ParentAccountHeadID=TT.AccountHeadID)
FROM #Temp_Table AS TT WHERE TT.AccountType=1 --EnumAccountType{None = 0,Component = 1,Segment =2,Group = 3,SubGroup = 4,Ledger = 5}

--Update Closing Balance
UPDATE #Temp_Table
SET	ClosingBalance=TT.OpenningBalance+TT.DebitTransaction-TT.CreditTransaction
FROM #Temp_Table AS TT WHERE TT.ComponentType  IN(2,6)

UPDATE #Temp_Table
SET	ClosingBalance=TT.OpenningBalance-TT.DebitTransaction+TT.CreditTransaction
FROM #Temp_Table AS TT WHERE TT.ComponentType NOT IN(2,6)
--select * from #Temp_Table


--Update EDClosing Balance
UPDATE #Temp_Table
SET	EDClosingBalance=TT.ClosingBalance+TT.EDDebitTransaction-TT.EDCreditTransaction
FROM #Temp_Table AS TT WHERE TT.ComponentType  IN(2,6) AND TT.ComponentType IN(2,3,4)

UPDATE #Temp_Table
SET	EDClosingBalance=TT.ClosingBalance-TT.EDDebitTransaction+TT.EDCreditTransaction
FROM #Temp_Table AS TT WHERE TT.ComponentType NOT IN(2,6) AND TT.ComponentType IN(2,3,4)
--select * from #Temp_Table

UPDATE #Temp_Table
SET ChangesAmount = ISNULL((TT.EDClosingBalance - TT.ClosingBalance),0)
FROM #Temp_Table AS TT WHERE TT.ComponentType IN(2,3,4)


CREATE TABLE #TempCFTable(
								CashFlowDmSetupID	int,
								CFTransactionCategory	smallint,
								CFTransactionGroup	smallint,
								CFDataType	int,
								SubGroupID	int,
								DisplayCaption	varchar(1024),
								Amount decimal(30,17),
								SubGroupCode Varchar(512),
								SubGroupName Varchar(512),
								Remarks Varchar(512)
						 )

INSERT INTO #TempCFTable ( CashFlowDmSetupID,		CFTransactionCategory,		CFTransactionGroup,		CFDataType,		SubGroupID,		DisplayCaption,		Amount,		SubGroupCode,		SubGroupName,		Remarks)
					SELECT TT.CashFlowDmSetupID,	TT.CFTransactionCategory,	TT.CFTransactionGroup,	TT.CFDataType,	TT.SubGroupID,	TT.DisplayCaption,	0.00,		TT.SubGroupCode,	TT.SubGroupName,	'' FROM View_CashFlowDmSetup AS TT

DECLARE
@Amount as decimal(30,17),
@DisplayCaption as Varchar(1024),
@TransactionAmount as decimal(30,17),
@DepreciationAmount as decimal(30,17),
@COGSAmount as decimal(30,17),
@OverHeadCost as decimal(30,17),
@AdministrativeCost as decimal(30,17),
@SellingCost as decimal(30,17),
@OtherIncome as decimal(30,17),
@ExpensesAmount as decimal(30,17),
@CACLChangesAmount as decimal(30,17)

--EnumCFDmTransactionGroup : Acquisition_of_Fixed_Asset = 5, EnumCFDmDataType : Fixed_Asset_SFP_Changes_Less_Depreciation = 5
SET @TransactionAmount = (-1)*ISNULL((SELECT SUM(TT.ChangesAmount) FROM #Temp_Table AS TT WHERE TT.AccountHeadID IN (SELECT HH.SubGroupID FROM #TempCFTable AS HH WHERE HH.CFDataType=5)),0) 
UPDATE #TempCFTable SET Amount = (@TransactionAmount) WHERE CFTransactionGroup=5

--EnumCFDmTransactionGroup : Fixed_Doposit = 6, EnumCFDmDataType : Fixed_Deposit_SFP_Changes = 6
SET @TransactionAmount = (-1)*ISNULL((SELECT SUM(TT.ChangesAmount) FROM #Temp_Table AS TT WHERE TT.AccountHeadID IN (SELECT HH.SubGroupID FROM #TempCFTable AS HH WHERE HH.CFDataType=6)),0) 
UPDATE #TempCFTable SET Amount = @TransactionAmount WHERE CFTransactionGroup=6

--EnumCFDmTransactionGroup : Acquisition_of_Intengible_Asstes = 7, EnumCFDmDataType : Intengible_Asstes_SFP_Changes_Less_Depreciation = 7
SET @TransactionAmount = (-1)*ISNULL((SELECT SUM(TT.ChangesAmount) FROM #Temp_Table AS TT WHERE TT.AccountHeadID IN (SELECT HH.SubGroupID FROM #TempCFTable AS HH WHERE HH.CFDataType=7)),0) 
UPDATE #TempCFTable SET Amount = (@TransactionAmount) WHERE CFTransactionGroup=7

--EnumCFDmTransactionGroup : Capital_WIP = 8, EnumCFDmDataType : Investment_SFP_Changes = 8
SET @TransactionAmount = (-1)*ISNULL((SELECT SUM(TT.ChangesAmount) FROM #Temp_Table AS TT WHERE TT.AccountHeadID IN (SELECT HH.SubGroupID FROM #TempCFTable AS HH WHERE HH.CFDataType=8)),0) 
UPDATE #TempCFTable SET Amount = @TransactionAmount WHERE CFTransactionGroup=8

--EnumCFDmTransactionGroup : Payment_of_Lease_Loan = 9, EnumCFDmDataType : None_Current_Loan_SFP_Changes = 9
SET @TransactionAmount = ISNULL((SELECT SUM(TT.ChangesAmount) FROM #Temp_Table AS TT WHERE TT.AccountHeadID IN (SELECT HH.SubGroupID FROM #TempCFTable AS HH WHERE HH.CFDataType=9)),0) 
UPDATE #TempCFTable SET Amount = @TransactionAmount WHERE CFTransactionGroup=9

--EnumCFDmTransactionGroup : Payment_of_Term_Loan = 10, EnumCFDmDataType : Current_Loan_SFP_Changes = 10
SET @TransactionAmount = ISNULL((SELECT SUM(TT.ChangesAmount) FROM #Temp_Table AS TT WHERE TT.AccountHeadID IN (SELECT HH.SubGroupID FROM #TempCFTable AS HH WHERE HH.CFDataType=10)),0) 
UPDATE #TempCFTable SET Amount = @TransactionAmount WHERE CFTransactionGroup=10

--EnumCFDmTransactionGroup : Payment_of_Dividend = 11, EnumCFDmDataType : Dividend_SFP_Changes = 11
SET @TransactionAmount = ISNULL((SELECT SUM(TT.ChangesAmount) FROM #Temp_Table AS TT WHERE TT.AccountHeadID IN (SELECT HH.SubGroupID FROM #TempCFTable AS HH WHERE HH.CFDataType=11)),0) 
UPDATE #TempCFTable SET Amount = @TransactionAmount WHERE CFTransactionGroup=11

--EnumCFDmTransactionGroup : Owners_Equity = 12, EnumCFDmDataType : Owners_Equity_SFP_Changes = 12
SET @TransactionAmount = ISNULL((SELECT SUM(TT.ChangesAmount) FROM #Temp_Table AS TT WHERE TT.AccountHeadID IN (SELECT HH.SubGroupID FROM #TempCFTable AS HH WHERE HH.CFDataType=12)),0) 
UPDATE #TempCFTable SET Amount = @TransactionAmount WHERE CFTransactionGroup=12

--EnumCFTransactionGroup : Opening_Balance = 19, EnumCFDataType : Opening_Balance_of_SFP = 19
UPDATE #TempCFTable 
SET Amount = ISNULL((SELECT SUM(HH.ClosingBalance) FROM #Temp_Table AS HH WHERE HH.AccountHeadID=TT.SubGroupID),0) 
FROM #TempCFTable AS TT WHERE TT.CFTransactionGroup=19


CREATE TABLE #FinalCFTable(
								ID int IDENTITY(1,1) PRIMARY KEY,
								CashFlowDmSetupID	int,
								CFTransactionCategory	smallint,
								CFTransactionGroup	smallint,
								CFDataType	int,
								SubGroupID	int,
								DisplayCaption	varchar(1024),
								Amount decimal(30,17),
								SubGroupCode Varchar(512),
								SubGroupName Varchar(512),
								Remarks Varchar(512)
						 )

--//Fixed for Operating_Activities        
--Cash_Receipts = 1, // Fixed for Operating_Activities //Choice Multiple Payment Collection Related Subgroup Head with Same Caption        
--Other_Income = 2, // Fixed for Operating_Activities //Choice Multiple Other Income Related Subgroup Head with Same Caption        
--Cash_Paid = 3,  // Fixed for Operating_Activities //Choice Multiple Payment Paid Related Subgroup Head with Same Caption

--//Fixed for Investing_Activities
--Acquisition_of_Fixed_Asset = 5, //Fixed_Asset_SFP_Changes_Less_Depreciation. //Choice a Asset Subgroup Head
--Fixed_Doposit = 6,//Fixed_Deposit_SFP_Changes  // Choice a Asset Subgroup Head
--Acquisition_of_Intengible_Asstes = 7,//Fixed for Intengible_Asstes_SFP_Changes_Less_Depreciation // Choice a Asset Subgroup Head
--Capital_WIP = 8, //Fixed for Investment_SFP_Changes // Choice a Asset Subgroup Head

--//Fixed for Financing_Activities
--Payment_of_Lease_Loan = 9, //Fixed for None_Current_Loan_SFP_Changes // Choice a Libility Subgroup Head
--Payment_of_Term_Loan = 10, //Fixed for Current_Loan_SFP_Changes // Choice a Libility Subgroup Head
--Payment_of_Dividend = 11, //Fixed for Dividend_SFP_Changes // Choice a Libility Subgroup Head

--OPERATING ACTIVITIES
INSERT INTO #FinalCFTable (CashFlowDmSetupID,		CFTransactionCategory,		CFTransactionGroup,		CFDataType,		SubGroupID,		DisplayCaption,							Amount,		SubGroupCode,		SubGroupName,		Remarks)
					VALUES(0,						1,							0,						0,				0,				'CASH FLOWS FROM OPERATING ACTIVITIES',	0.00,		'',					'A',				'')


--EnumCFDmTransactionCategory :Operating_Activities = 1,  EnumCFDmTransactionGroup : Cash_Receipts = 1, Opening_Balance = 19,  EnumCFDmDataType : Collection_Received_From_Sales = 1
SET @DisplayCaption = (SELECT TOP 1 HH.DisplayCaption FROM #TempCFTable AS HH WHERE HH.CFTransactionGroup=1)
SET @Amount = ISNULL((	SELECT SUM(HH.ReverseAmount) FROM View_VoucherDetailReverse AS HH 
						WHERE HH.IsDebit=1 
						AND HH.AccountHeadID IN (SELECT COA.AccountHeadID FROM ChartsOfAccount AS COA WHERE COA.ParentHeadID IN (SELECT MM.SubGroupID FROM #TempCFTable AS MM WHERE MM.CFTransactionGroup=19)) -- Opening_Balance = 19
						AND HH.ReverseHeadID IN (SELECT COA.AccountHeadID FROM ChartsOfAccount AS COA WHERE COA.ParentHeadID IN (SELECT MM.SubGroupID FROM #TempCFTable AS MM WHERE MM.CFTransactionGroup=1))  -- Cash_Receipts = 1
					  ),0)
INSERT INTO #FinalCFTable (CashFlowDmSetupID,		CFTransactionCategory,		CFTransactionGroup,		CFDataType,		SubGroupID,		DisplayCaption,		Amount,		SubGroupCode,		SubGroupName,		Remarks)
					VALUES(0,						1,							1,						1,				0,				@DisplayCaption,	@Amount,	'',					'',					'')

--EnumCFDmTransactionCategory :Operating_Activities = 1,  EnumCFDmTransactionGroup : Other_Income = 2, Opening_Balance = 19,  EnumCFDmDataType : Collection_From_Other_Income = 2
SET @DisplayCaption = (SELECT TOP 1 HH.DisplayCaption FROM #TempCFTable AS HH WHERE HH.CFTransactionGroup=2)
SET @Amount = ISNULL((	SELECT SUM(HH.ReverseAmount) FROM View_VoucherDetailReverse AS HH 
						WHERE HH.IsDebit=1 
						AND HH.AccountHeadID IN (SELECT COA.AccountHeadID FROM ChartsOfAccount AS COA WHERE COA.ParentHeadID IN (SELECT MM.SubGroupID FROM #TempCFTable AS MM WHERE MM.CFTransactionGroup=19)) -- Opening_Balance = 19
						AND HH.ReverseHeadID IN (SELECT COA.AccountHeadID FROM ChartsOfAccount AS COA WHERE COA.ParentHeadID IN (SELECT MM.SubGroupID FROM #TempCFTable AS MM WHERE MM.CFTransactionGroup=2))  -- Other_Income = 2
					  ),0)
INSERT INTO #FinalCFTable (CashFlowDmSetupID,		CFTransactionCategory,		CFTransactionGroup,		CFDataType,		SubGroupID,		DisplayCaption,		Amount,		SubGroupCode,		SubGroupName,		Remarks)
					VALUES(0,						1,							2,						2,				0,				@DisplayCaption,	@Amount,	'',					'',					'')


--EnumCFDmTransactionCategory :Operating_Activities = 1,  EnumCFDmTransactionGroup : Cash_Paid = 3, Opening_Balance = 19,  EnumCFDmDataType : Paid_To_Expenses_And_Purchase = 3
SET @DisplayCaption = (SELECT TOP 1 HH.DisplayCaption FROM #TempCFTable AS HH WHERE HH.CFTransactionGroup=3)
SET @Amount = ISNULL((	SELECT SUM(HH.ReverseAmount) FROM View_VoucherDetailReverse AS HH 
						WHERE HH.IsDebit=0 
						AND HH.AccountHeadID IN (SELECT COA.AccountHeadID FROM ChartsOfAccount AS COA WHERE COA.ParentHeadID IN (SELECT MM.SubGroupID FROM #TempCFTable AS MM WHERE MM.CFTransactionGroup=19)) -- Opening_Balance = 19
						AND HH.ReverseHeadID IN (SELECT COA.AccountHeadID FROM ChartsOfAccount AS COA WHERE COA.ParentHeadID IN (SELECT MM.SubGroupID FROM #TempCFTable AS MM WHERE MM.CFTransactionGroup=3))  -- Cash_Paid = 3
					  ),0)
INSERT INTO #FinalCFTable (CashFlowDmSetupID,		CFTransactionCategory,		CFTransactionGroup,		CFDataType,		SubGroupID,		DisplayCaption,		Amount,				SubGroupCode,		SubGroupName,		Remarks)
					VALUES(0,						1,							3,						3,				0,				@DisplayCaption,	((-1) *@Amount),	'',					'',					'')

SET @TransactionAmount = ISNULL((SELECT SUM(TT.Amount) FROM #FinalCFTable AS TT WHERE TT.CFTransactionGroup IN (1,2,3)),0)
INSERT INTO #FinalCFTable (CashFlowDmSetupID,		CFTransactionCategory,		CFTransactionGroup,		CFDataType,		SubGroupID,		DisplayCaption,								Amount,					SubGroupCode,		SubGroupName,		Remarks)
					VALUES(1001,					1,							0,						0,				0,				'Net Cash Flows from Operating Activities',	@TransactionAmount,		'',					'',				'')




--INVESTING ACTIVITIES
INSERT INTO #FinalCFTable (CashFlowDmSetupID,		CFTransactionCategory,		CFTransactionGroup,		CFDataType,		SubGroupID,		DisplayCaption,							Amount,		SubGroupCode,		SubGroupName,		Remarks)
					VALUES(0,						2,							0,						0,				0,				'CASH FLOWS FROM INVESTING ACTIVITIES',	0.00,		'',					'B',				'')

INSERT INTO #FinalCFTable
SELECT * FROM #TempCFTable AS TT WHERE TT.CFTransactionGroup IN (5,6,7,8) ORDER BY TT.CFTransactionGroup

SET @TransactionAmount = ISNULL((SELECT SUM(TT.Amount) FROM #TempCFTable AS TT WHERE TT.CFTransactionGroup IN (5,6,7,8)),0)
INSERT INTO #FinalCFTable (CashFlowDmSetupID,		CFTransactionCategory,		CFTransactionGroup,		CFDataType,		SubGroupID,		DisplayCaption,								Amount,					SubGroupCode,		SubGroupName,		Remarks)
					VALUES(1002,				2,							0,						0,				0,				'Net Cash Flows from Investing Activities',	@TransactionAmount,		'',					'',				'')

--FINANCING ACTIVITIES
INSERT INTO #FinalCFTable (CashFlowDmSetupID,		CFTransactionCategory,		CFTransactionGroup,		CFDataType,		SubGroupID,		DisplayCaption,			Amount,		SubGroupCode,		SubGroupName,		Remarks)
					VALUES(0,					3,							0,						0,				0,				'CASH FLOWS FROM FINANCING ACTIVITIES',	0.00,		'',					'C',				'')

INSERT INTO #FinalCFTable
SELECT * FROM #TempCFTable AS TT WHERE TT.CFTransactionGroup IN (9,10,11,12) ORDER BY TT.CFTransactionGroup

SET @TransactionAmount = ISNULL((SELECT SUM(TT.Amount) FROM #TempCFTable AS TT WHERE TT.CFTransactionGroup IN (9,10,11,12)),0)
INSERT INTO #FinalCFTable (CashFlowDmSetupID,		CFTransactionCategory,		CFTransactionGroup,		CFDataType,		SubGroupID,		DisplayCaption,								Amount,					SubGroupCode,		SubGroupName,		Remarks)
					VALUES(1003,					3,							0,						0,				0,				'Net Cash Flows from Financing Activities',	@TransactionAmount,		'',					'',				'')

--Net_Increase =8
SET @TransactionAmount = ISNULL((SELECT SUM(TT.Amount) FROM #FinalCFTable AS TT WHERE TT.CashFlowDmSetupID IN (1001,1002,1003)),0)
INSERT INTO #FinalCFTable (CashFlowDmSetupID,		CFTransactionCategory,		CFTransactionGroup,		CFDataType,		SubGroupID,		DisplayCaption,											Amount,					SubGroupCode,		SubGroupName,		Remarks)
					VALUES(1004,					8,							0,						0,				0,				'Net changes in Cash and Cash Equivalents (A+B+C)',		@TransactionAmount,		'',					'',				'')


--Opening_Balance = 7
SET @TransactionAmount = ISNULL((SELECT SUM(TT.Amount) FROM #TempCFTable AS TT WHERE TT.CFTransactionCategory IN (7)),0)
INSERT INTO #FinalCFTable (CashFlowDmSetupID,		CFTransactionCategory,		CFTransactionGroup,		CFDataType,		SubGroupID,		DisplayCaption,												Amount,					SubGroupCode,		SubGroupName,		Remarks)
					VALUES(1005,					7,							0,						0,				0,				'Cash and Cash Equivalents at the Beginning of the period',	@TransactionAmount,		'',					'',				'')

--Closing_Balance=9
SET @TransactionAmount = ISNULL((SELECT SUM(TT.Amount) FROM #FinalCFTable AS TT WHERE TT.CashFlowDmSetupID IN (1004,1005)),0)
INSERT INTO #FinalCFTable (CashFlowDmSetupID,		CFTransactionCategory,		CFTransactionGroup,		CFDataType,		SubGroupID,		DisplayCaption,												Amount,					SubGroupCode,		SubGroupName,		Remarks)
					VALUES(1006,				9,							0,						0,				0,				'Cash and Cash Equivalents at the Closing of the period',		@TransactionAmount,		'',					'',				'')

SELECT * FROM #FinalCFTable  ORDER BY ID
DROP TABLE #FinalCFTable
DROP TABLE #TempCFTable
DROP TABLE #Temp_Table
COMMIT TRAN




GO
/****** Object:  StoredProcedure [dbo].[SP_CashFlowStatement]    Script Date: 3/2/2017 1:53:03 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[SP_CashFlowStatement]
(
	@BUID as int,
	@StartDate as date,
	@EndDate as date,
	@IsPaymentDetails as bit
)
AS
BEGIN TRAN
--DECLARE
--@BUID as int,
--@StartDate as date,
--@EndDate as date,
--@IsPaymentDetails as bit

--SET @BUID=0
--SET @StartDate ='01 Jan 2016'
--SET @EndDate = '21 Mar 2016'
--SET @IsPaymentDetails =0

DECLARE
@BUCode as Varchar(512),
@ComponentIDs as Varchar(512),
@SessionID as int,
@SessionStartDate as date,
@SessionEndDate as date,
@CurrencyID as int


SET @BUCode=ISNULL((SELECT BU.Code FROM BusinessUnit AS BU WHERE BU.BusinessUnitID=@BUID),'00')
SET @ComponentIDs='2,3,4,5,6'
SET @CurrencyID = (SELECT TT.BaseCurrencyID FROM Company AS TT WHERE TT.CompanyID=1)
--ComponentID{AsSET = 2,Laibility = 3,OwnerEquity=4,Income = 5,Expeness = 6}

IF EXISTS(SELECT ASE.SessionID FROM AccountingSession AS ASE WHERE  CONVERT(DATE,CONVERT(VARCHAR(12),ASE.StartDate,106))=CONVERT(DATE,CONVERT(VARCHAR(12),@StartDate,106)) AND ASE.SessionType=6)
BEGIN
	SET @SessionID=(SELECT top(1)ASE.SessionID FROM AccountingSession AS ASE WHERE  CONVERT(DATE,CONVERT(VARCHAR(12),ASE.StartDate,106))=CONVERT(DATE,CONVERT(VARCHAR(12),@StartDate,106)) AND ASE.SessionType=6)
END
ELSE
BEGIN
	SET @SessionID=(SELECT TOP 1 ASE.SessionID FROM AccountingSession AS ASE WHERE  CONVERT(DATE,CONVERT(VARCHAR(12),ASE.StartDate,106))>CONVERT(DATE,CONVERT(VARCHAR(12),@StartDate,106)) AND ASE.SessionType=6)	
END
SET @SessionStartDate=(SELECT ASE.StartDate FROM AccountingSession AS ASE WHERE  ASE.AccountingSessionID=@SessionID)
SET @SessionEndDate=DATEADD(DAY, -1, @StartDate)

CREATE TABLE #Temp_Table(		
							AccountHeadID int,
							AccountCode varchar(128),
							AccountHeadName varchar(512),
							ParentAccountHeadID int,
							ComponentType smallint,
							AccountType smallint,							
							OpenningBalance decimal(30,17),
							DebitTransaction decimal(30,17),
							CreditTransaction decimal(30,17),							
							ClosingBalance decimal(30,17),
							EDDebitTransaction decimal(30,17),
							EDCreditTransaction decimal(30,17),							
							EDClosingBalance decimal(30,17),
							ChangesAmount decimal(30,17)
						)

CREATE NONCLUSTERED INDEX #IX_Temp2_1 ON #Temp_Table(AccountHeadID)

INSERT INTO #Temp_Table		(	AccountHeadID,		AccountCode,			AccountHeadName,		ParentAccountHeadID,	AccountType)
					SELECT		AccountHeadID,		AccountCode,			AccountHeadName,		ParentHeadID,			AccountType FROM dbo.GetAccountHeadByComponents(@ComponentIDs)

UPDATE #Temp_Table SET	ComponentType=dbo.GetComponentID(TT.AccountHeadID) FROM #Temp_Table AS TT 

--For Balance Sheet
--EnumAccountType{None = 0,Component = 1,Segment =2,Group = 3,SubGroup = 4,Ledger = 5}
--Update ledger transaction, ComponentID, Opening Balance
IF(@BUID>0)
BEGIN
	UPDATE #Temp_Table
	SET	ComponentType=dbo.GetComponentID(TT.AccountHeadID),
		OpenningBalance= ISNULL((SELECT LOB.OpenningBalance FROM dbo.[GetLedgerOpeningBalanceByBU](TT.AccountHeadID, @StartDate, @CurrencyID, 1, 0, @BUCode, '00' ) AS LOB),0),	
		DebitTransaction=(SELECT ISNULL(SUM(VD.Amount),0) FROM View_VoucherDetail AS VD  WHERE  VD.IsDebit=1 AND VD.BUID=@BUID AND VD.AccountHeadID=TT.AccountHeadID AND VD.VoucherID IN(SELECT V.VoucherID FROM Voucher AS V WHERE ISNULL(V.AuthorizedBy,0)!=0 AND CONVERT(DATE,CONVERT(VARCHAR(12),V.VoucherDate)) BETWEEN CONVERT(DATE,CONVERT(VARCHAR(12),@SessionStartDate))  AND CONVERT(DATE,CONVERT(VARCHAR(12),@SessionEndDate)))),
		CreditTransaction=(SELECT ISNULL(SUM(VD.Amount),0) FROM View_VoucherDetail AS VD  WHERE  VD.IsDebit=0 AND VD.BUID=@BUID AND VD.AccountHeadID=TT.AccountHeadID AND VD.VoucherID IN(SELECT V.VoucherID FROM Voucher AS V WHERE ISNULL(V.AuthorizedBy,0)!=0 AND CONVERT(DATE,CONVERT(VARCHAR(12),V.VoucherDate)) BETWEEN CONVERT(DATE,CONVERT(VARCHAR(12),@SessionStartDate))  AND CONVERT(DATE,CONVERT(VARCHAR(12),@SessionEndDate)))),
		ClosingBalance=0,
		EDDebitTransaction=(SELECT ISNULL(SUM(VD.Amount),0) FROM View_VoucherDetail AS VD  WHERE  VD.IsDebit=1 AND VD.BUID=@BUID AND VD.AccountHeadID=TT.AccountHeadID AND VD.VoucherID IN(SELECT V.VoucherID FROM Voucher AS V WHERE ISNULL(V.AuthorizedBy,0)!=0 AND CONVERT(DATE,CONVERT(VARCHAR(12),V.VoucherDate)) BETWEEN CONVERT(DATE,CONVERT(VARCHAR(12),@StartDate))  AND CONVERT(DATE,CONVERT(VARCHAR(12),@EndDate)))),
		EDCreditTransaction=(SELECT ISNULL(SUM(VD.Amount),0) FROM View_VoucherDetail AS VD  WHERE  VD.IsDebit=0 AND VD.BUID=@BUID AND VD.AccountHeadID=TT.AccountHeadID AND VD.VoucherID IN(SELECT V.VoucherID FROM Voucher AS V WHERE ISNULL(V.AuthorizedBy,0)!=0 AND CONVERT(DATE,CONVERT(VARCHAR(12),V.VoucherDate)) BETWEEN CONVERT(DATE,CONVERT(VARCHAR(12),@StartDate))  AND CONVERT(DATE,CONVERT(VARCHAR(12),@EndDate)))),
		EDClosingBalance=0
	FROM #Temp_Table AS TT WHERE TT.AccountType=5 AND TT.ComponentType IN (2,3,4)
END
ELSE
BEGIN
	UPDATE #Temp_Table
	SET	ComponentType=dbo.GetComponentID(TT.AccountHeadID),
		OpenningBalance= ISNULL((SELECT LOB.OpenningBalance FROM dbo.[GetLedgerOpeningBalance](TT.AccountHeadID, @StartDate, @CurrencyID, 1, 0, 0) AS LOB),0),	
		DebitTransaction=(SELECT ISNULL(SUM(VD.Amount),0) FROM View_VoucherDetail AS VD  WHERE  VD.IsDebit=1 AND VD.AccountHeadID=TT.AccountHeadID AND VD.VoucherID IN(SELECT V.VoucherID FROM Voucher AS V WHERE ISNULL(V.AuthorizedBy,0)!=0 AND CONVERT(DATE,CONVERT(VARCHAR(12),V.VoucherDate)) BETWEEN CONVERT(DATE,CONVERT(VARCHAR(12),@SessionStartDate))  AND CONVERT(DATE,CONVERT(VARCHAR(12),@SessionEndDate)))),
		CreditTransaction=(SELECT ISNULL(SUM(VD.Amount),0) FROM View_VoucherDetail AS VD  WHERE  VD.IsDebit=0 AND VD.AccountHeadID=TT.AccountHeadID AND VD.VoucherID IN(SELECT V.VoucherID FROM Voucher AS V WHERE ISNULL(V.AuthorizedBy,0)!=0 AND CONVERT(DATE,CONVERT(VARCHAR(12),V.VoucherDate)) BETWEEN CONVERT(DATE,CONVERT(VARCHAR(12),@SessionStartDate))  AND CONVERT(DATE,CONVERT(VARCHAR(12),@SessionEndDate)))),
		ClosingBalance=0,
		EDDebitTransaction=(SELECT ISNULL(SUM(VD.Amount),0) FROM View_VoucherDetail AS VD  WHERE  VD.IsDebit=1 AND VD.AccountHeadID=TT.AccountHeadID AND VD.VoucherID IN(SELECT V.VoucherID FROM Voucher AS V WHERE ISNULL(V.AuthorizedBy,0)!=0 AND CONVERT(DATE,CONVERT(VARCHAR(12),V.VoucherDate)) BETWEEN CONVERT(DATE,CONVERT(VARCHAR(12),@StartDate))  AND CONVERT(DATE,CONVERT(VARCHAR(12),@EndDate)))),
		EDCreditTransaction=(SELECT ISNULL(SUM(VD.Amount),0) FROM View_VoucherDetail AS VD  WHERE  VD.IsDebit=0 AND VD.AccountHeadID=TT.AccountHeadID AND VD.VoucherID IN(SELECT V.VoucherID FROM Voucher AS V WHERE ISNULL(V.AuthorizedBy,0)!=0 AND CONVERT(DATE,CONVERT(VARCHAR(12),V.VoucherDate)) BETWEEN CONVERT(DATE,CONVERT(VARCHAR(12),@StartDate))  AND CONVERT(DATE,CONVERT(VARCHAR(12),@EndDate)))),
		EDClosingBalance=0
	FROM #Temp_Table AS TT WHERE TT.AccountType=5 AND TT.ComponentType IN (2,3,4)
END

--
--EnumAccountType{None = 0,Component = 1,Segment =2,Group = 3,SubGroup = 4,Ledger = 5}
--Update ledger transaction, ComponentID, Openning Balance
--OperationType 1 means FreshVoucher
IF(@BUID>0)
BEGIN
	UPDATE #Temp_Table
	SET	OpenningBalance=0,
		DebitTransaction=(SELECT ISNULL(SUM(VD.Amount),0) FROM View_VoucherDetail AS VD WHERE VD.BUID=@BUID AND VD.OperationType=1 AND VD.IsDebit=1 AND VD.AccountHeadID=TT.AccountHeadID AND VD.VoucherID IN(SELECT V.VoucherID FROM Voucher AS V WHERE V.AuthorizedBy!=0 AND CONVERT(DATE,CONVERT(VARCHAR(12),VoucherDate,106)) BETWEEN CONVERT(DATE,CONVERT(VARCHAR(12),@StartDate,106)) AND CONVERT(DATE,CONVERT(VARCHAR(12),@EndDate,106)))),
		CreditTransaction=(SELECT ISNULL(SUM(VD.Amount),0) FROM View_VoucherDetail AS VD WHERE VD.BUID=@BUID AND VD.OperationType=1 AND VD.IsDebit=0 AND VD.AccountHeadID=TT.AccountHeadID AND VD.VoucherID IN(SELECT V.VoucherID FROM Voucher AS V WHERE V.AuthorizedBy!=0 AND CONVERT(DATE,CONVERT(VARCHAR(12),VoucherDate,106)) BETWEEN CONVERT(DATE,CONVERT(VARCHAR(12),@StartDate,106)) AND CONVERT(DATE,CONVERT(VARCHAR(12),@EndDate,106)))),
		ClosingBalance=0
	FROM #Temp_Table AS TT WHERE TT.AccountType=5 AND TT.ComponentType IN (5,6)
END
ELSE
BEGIN
	UPDATE #Temp_Table
	SET	OpenningBalance=0,
		DebitTransaction=(SELECT ISNULL(SUM(VD.Amount),0) FROM View_VoucherDetail AS VD WHERE VD.OperationType=1 AND VD.IsDebit=1 AND VD.AccountHeadID=TT.AccountHeadID AND VD.VoucherID IN(SELECT V.VoucherID FROM Voucher AS V WHERE V.AuthorizedBy!=0 AND CONVERT(DATE,CONVERT(VARCHAR(12),VoucherDate,106)) BETWEEN CONVERT(DATE,CONVERT(VARCHAR(12),@StartDate,106)) AND CONVERT(DATE,CONVERT(VARCHAR(12),@EndDate,106)))),
		CreditTransaction=(SELECT ISNULL(SUM(VD.Amount),0) FROM View_VoucherDetail AS VD WHERE VD.OperationType=1 AND VD.IsDebit=0 AND VD.AccountHeadID=TT.AccountHeadID AND VD.VoucherID IN(SELECT V.VoucherID FROM Voucher AS V WHERE V.AuthorizedBy!=0 AND CONVERT(DATE,CONVERT(VARCHAR(12),VoucherDate,106)) BETWEEN CONVERT(DATE,CONVERT(VARCHAR(12),@StartDate,106)) AND CONVERT(DATE,CONVERT(VARCHAR(12),@EndDate,106)))),
		ClosingBalance=0
	FROM #Temp_Table AS TT WHERE TT.AccountType=5 AND TT.ComponentType IN (5,6)
END

--Update SubGroup debit/credit Transaction
UPDATE #Temp_Table
SET OpenningBalance=(SELECT ISNULL(SUM(ABC.OpenningBalance),0) FROM #Temp_Table AS ABC WHERE ABC.ParentAccountHeadID=TT.AccountHeadID),
	DebitTransaction=(SELECT ISNULL(SUM(ABC.DebitTransaction),0) FROM #Temp_Table AS ABC WHERE ABC.ParentAccountHeadID=TT.AccountHeadID),
	CreditTransaction=(SELECT ISNULL(SUM(ABC.CreditTransaction),0) FROM #Temp_Table AS ABC WHERE ABC.ParentAccountHeadID=TT.AccountHeadID),
	EDDebitTransaction=(SELECT ISNULL(SUM(ABC.EDDebitTransaction),0) FROM #Temp_Table AS ABC WHERE ABC.ParentAccountHeadID=TT.AccountHeadID),
	EDCreditTransaction=(SELECT ISNULL(SUM(ABC.EDCreditTransaction),0) FROM #Temp_Table AS ABC WHERE ABC.ParentAccountHeadID=TT.AccountHeadID)
FROM #Temp_Table AS TT WHERE TT.AccountType=4 --EnumAccountType{None = 0,Component = 1,Segment =2,Group = 3,SubGroup = 4,Ledger = 5}

--Update Group debit/credit Transaction
UPDATE #Temp_Table
SET OpenningBalance=(SELECT ISNULL(SUM(ABC.OpenningBalance),0) FROM #Temp_Table AS ABC WHERE ABC.ParentAccountHeadID=TT.AccountHeadID),
	DebitTransaction=(SELECT ISNULL(SUM(ABC.DebitTransaction),0) FROM #Temp_Table AS ABC WHERE ABC.ParentAccountHeadID=TT.AccountHeadID),
	CreditTransaction=(SELECT ISNULL(SUM(ABC.CreditTransaction),0) FROM #Temp_Table AS ABC WHERE ABC.ParentAccountHeadID=TT.AccountHeadID),
	EDDebitTransaction=(SELECT ISNULL(SUM(ABC.EDDebitTransaction),0) FROM #Temp_Table AS ABC WHERE ABC.ParentAccountHeadID=TT.AccountHeadID),
	EDCreditTransaction=(SELECT ISNULL(SUM(ABC.EDCreditTransaction),0) FROM #Temp_Table AS ABC WHERE ABC.ParentAccountHeadID=TT.AccountHeadID)
FROM #Temp_Table AS TT WHERE TT.AccountType=3 --EnumAccountType{None = 0,Component = 1,Segment =2,Group = 3,SubGroup = 4,Ledger = 5}

--Update Segment debit/credit Transaction
UPDATE #Temp_Table
SET OpenningBalance=(SELECT ISNULL(SUM(ABC.OpenningBalance),0) FROM #Temp_Table AS ABC WHERE ABC.ParentAccountHeadID=TT.AccountHeadID),
	DebitTransaction=(SELECT ISNULL(SUM(ABC.DebitTransaction),0) FROM #Temp_Table AS ABC WHERE ABC.ParentAccountHeadID=TT.AccountHeadID),
	CreditTransaction=(SELECT ISNULL(SUM(ABC.CreditTransaction),0) FROM #Temp_Table AS ABC WHERE ABC.ParentAccountHeadID=TT.AccountHeadID),
	EDDebitTransaction=(SELECT ISNULL(SUM(ABC.EDDebitTransaction),0) FROM #Temp_Table AS ABC WHERE ABC.ParentAccountHeadID=TT.AccountHeadID),
	EDCreditTransaction=(SELECT ISNULL(SUM(ABC.EDCreditTransaction),0) FROM #Temp_Table AS ABC WHERE ABC.ParentAccountHeadID=TT.AccountHeadID)
FROM #Temp_Table AS TT WHERE TT.AccountType=2 --EnumAccountType{None = 0,Component = 1,Segment =2,Group = 3,SubGroup = 4,Ledger = 5}

--Update Component debit/credit Transaction
UPDATE #Temp_Table
SET OpenningBalance=(SELECT ISNULL(SUM(ABC.OpenningBalance),0) FROM #Temp_Table AS ABC WHERE ABC.ParentAccountHeadID=TT.AccountHeadID),
	DebitTransaction=(SELECT ISNULL(SUM(ABC.DebitTransaction),0) FROM #Temp_Table AS ABC WHERE ABC.ParentAccountHeadID=TT.AccountHeadID),
	CreditTransaction=(SELECT ISNULL(SUM(ABC.CreditTransaction),0) FROM #Temp_Table AS ABC WHERE ABC.ParentAccountHeadID=TT.AccountHeadID),
	EDDebitTransaction=(SELECT ISNULL(SUM(ABC.EDDebitTransaction),0) FROM #Temp_Table AS ABC WHERE ABC.ParentAccountHeadID=TT.AccountHeadID),
	EDCreditTransaction=(SELECT ISNULL(SUM(ABC.EDCreditTransaction),0) FROM #Temp_Table AS ABC WHERE ABC.ParentAccountHeadID=TT.AccountHeadID)
FROM #Temp_Table AS TT WHERE TT.AccountType=1 --EnumAccountType{None = 0,Component = 1,Segment =2,Group = 3,SubGroup = 4,Ledger = 5}

--Update Closing Balance
UPDATE #Temp_Table
SET	ClosingBalance=TT.OpenningBalance+TT.DebitTransaction-TT.CreditTransaction
FROM #Temp_Table AS TT WHERE TT.ComponentType  IN(2,6)

UPDATE #Temp_Table
SET	ClosingBalance=TT.OpenningBalance-TT.DebitTransaction+TT.CreditTransaction
FROM #Temp_Table AS TT WHERE TT.ComponentType NOT IN(2,6)
--select * from #Temp_Table


--Update EDClosing Balance
UPDATE #Temp_Table
SET	EDClosingBalance=TT.ClosingBalance+TT.EDDebitTransaction-TT.EDCreditTransaction
FROM #Temp_Table AS TT WHERE TT.ComponentType  IN(2,6) AND TT.ComponentType IN(2,3,4)

UPDATE #Temp_Table
SET	EDClosingBalance=TT.ClosingBalance-TT.EDDebitTransaction+TT.EDCreditTransaction
FROM #Temp_Table AS TT WHERE TT.ComponentType NOT IN(2,6) AND TT.ComponentType IN(2,3,4)
--select * from #Temp_Table

UPDATE #Temp_Table
SET ChangesAmount = ISNULL((TT.EDClosingBalance - TT.ClosingBalance),0)
FROM #Temp_Table AS TT WHERE TT.ComponentType IN(2,3,4)


CREATE TABLE #TempCFTable(
								CashFlowSetupID	int,
								CFTransactionCategory	smallint,
								CFTransactionGroup	smallint,
								CFDataType	int,
								SubGroupID	int,
								DisplayCaption	varchar(1024),
								Amount decimal(30,17),
								SubGroupCode Varchar(512),
								SubGroupName Varchar(512),
								Remarks Varchar(512)
						 )

INSERT INTO #TempCFTable ( CashFlowSetupID,		CFTransactionCategory,		CFTransactionGroup,		CFDataType,		SubGroupID,		DisplayCaption,		Amount,		SubGroupCode,		SubGroupName,		Remarks)
					SELECT TT.CashFlowSetupID,	TT.CFTransactionCategory,	TT.CFTransactionGroup,	TT.CFDataType,	TT.SubGroupID,	TT.DisplayCaption,	0.00,		TT.SubGroupCode,	TT.SubGroupName,	'' FROM View_CashFlowSetup AS TT

--INSERT INTO #TempCFTable VALUES(1,1,1,1,0,'Net Trunover of Comprehensive Income',0.00, '', '', '')
--INSERT INTO #TempCFTable VALUES(2,1,2,2,0,'COGS of Comprehensive Income Current Asset & Current Libility Chnages',0.00, '', '', '')
--INSERT INTO #TempCFTable VALUES(3,1,3,3,0,'Financila Cost of Comprehensive Income',0.00, '', '', '')
--INSERT INTO #TempCFTable VALUES(4,1,4,4,0,'Income Tax of Comprehensive Income',0.00, '', '', '')
--INSERT INTO #TempCFTable VALUES(5,2,5,5,236,'Fixed Asset SFP Changes Less Depreciation',0.00, '', '', '')
--INSERT INTO #TempCFTable VALUES(6,2,6,6,176,'Fixed Deposit SFP Changes',0.00, '', '', '')
--INSERT INTO #TempCFTable VALUES(7,2,7,7,219,'Intengible Asstes SFP Changes Less Depreciation',0.00, '', '', '')
--INSERT INTO #TempCFTable VALUES(8,2,8,8,227,'Investment SFP Changes',0.00, '', '', '')
--INSERT INTO #TempCFTable VALUES(9,3,9,9,163,'None Current Loan SFP Changes',0.00, '', '', '')
--INSERT INTO #TempCFTable VALUES(10,3,10,10,154,'Current Loan SFP Changes',0.00, '', '', '')
--INSERT INTO #TempCFTable VALUES(11,3,11,11,0,'Dividend SFP Changes',0.00, '', '', '')
--INSERT INTO #TempCFTable VALUES(12,4,12,12,0,'COGS of Comprehensive Income',0.00, '', '', '')
--INSERT INTO #TempCFTable VALUES(13,4,13,13,283,'Administrative Cost Of Comprehensive Income',0.00, '', '', '')
--INSERT INTO #TempCFTable VALUES(14,4,14,14,284,'Selling Cost of Comprehensive Income',0.00, '', '', '')
--INSERT INTO #TempCFTable VALUES(15,5,15,15,187,'Increase/ (Decrease) in Inventory',0.00, '', '', '')
--INSERT INTO #TempCFTable VALUES(16,5,16,16,125,'Increase/ (Decrease) in Payable to Supplier-Local',0.00, '', '', '')
--INSERT INTO #TempCFTable VALUES(17,6,17,17,286,'Fixed Asset Depreciation Cost Of Comprehensive Income',0.00, '', '', '')
--INSERT INTO #TempCFTable VALUES(18,6,18,18,0,'Intengible Asset Depreciation Cost Of Comprehensive Income',0.00, '', '', '')
--INSERT INTO #TempCFTable VALUES(19,7,19,19,175,'Opening Balance of Bank Balance',0.00, '', '', '')
--INSERT INTO #TempCFTable VALUES(20,7,20,20,0,'Other Income of Comprehensive Income',0.00, '', '', '')
--INSERT INTO #TempCFTable VALUES(21,4,13,13,286,'Depreciation and Amortization Of Comprehensive Income',0.00, '', '', '')
--INSERT INTO #TempCFTable VALUES(22,5,15,15,212,'Increase/ (Decrease) in Accounts Receivable',0.00, '', '', '')
--INSERT INTO #TempCFTable VALUES(23,5,15,15,228,'Increase/ (Decrease) in Security Deposit',0.00, '', '', '')
--INSERT INTO #TempCFTable VALUES(24,5,15,15,195,'Increase/ (Decrease) in Loan and Advance',0.00, '', '', '')
--INSERT INTO #TempCFTable VALUES(25,5,15,15,196,'Increase/ (Decrease) in Advance Tax and Vat',0.00, '', '', '')
--INSERT INTO #TempCFTable VALUES(26,5,15,15,197,'Increase/ (Decrease) in Prepayments',0.00, '', '', '')
--INSERT INTO #TempCFTable VALUES(27,5,15,15,198,'Increase/ (Decrease) in Others',0.00, '', '', '')
--INSERT INTO #TempCFTable VALUES(28,5,15,15,177,'Increase/ (Decrease) in Bank Guarantee',0.00, '', '', '')
--INSERT INTO #TempCFTable VALUES(29,5,16,16,127,'Increase/ (Decrease) in Payable to Supplier-Foreign',0.00, '', '', '')
--INSERT INTO #TempCFTable VALUES(30,5,16,16,132,'Increase/ (Decrease) in Liability for Expenses',0.00, '', '', '')
--INSERT INTO #TempCFTable VALUES(31,5,16,16,133,'Increase/ (Decrease) in Statutory Provision',0.00, '', '', '')
--INSERT INTO #TempCFTable VALUES(32,5,16,16,340,'Increase/ (Decrease) in Unearned Revenue',0.00, '', '', '')
--INSERT INTO #TempCFTable VALUES(33,7,19,19,396,'Opening Balance of Cash in Hand',0.00, '', '', '')

--EnumCISSetup {  Gross_Turnover = 1, value_Added_Tax = 2,  Cost_Of_Goods_Sold = 3, Operating_Expenses = 4, Other_Income = 5, WPPF_Allocation = 6,  
--Income_Tax = 7, Profit_From_Associate_Undertaking = 8,  Comprehensive_Income = 9,  Financial_Cost = 10 }

DECLARE
@TransactionAmount as decimal(30,17),
@DepreciationAmount as decimal(30,17),
@COGSAmount as decimal(30,17),
@OverHeadCost as decimal(30,17),
@AdministrativeCost as decimal(30,17),
@SellingCost as decimal(30,17),
@OtherIncome as decimal(30,17),
@ExpensesAmount as decimal(30,17),
@CACLChangesAmount as decimal(30,17)

--EnumCFTransactionGroup : Cash_Receipts = 1, EnumCFDataType : Net_Trunover_of_SCI = 1, EnumCISSetup : Gross_Turnover = 1, value_Added_Tax = 2
SET @TransactionAmount = (ISNULL((SELECT SUM(TT.ClosingBalance) FROM #Temp_Table AS TT WHERE TT.AccountHeadID IN (SELECT HH.AccountHeadID FROM CIStatementSetup AS HH WHERE HH.CIHeadType=1)),0) - ISNULL((SELECT SUM(TT.ClosingBalance) FROM #Temp_Table AS TT WHERE TT.AccountHeadID IN (SELECT HH.AccountHeadID FROM CIStatementSetup AS HH WHERE HH.CIHeadType=2)),0))
UPDATE #TempCFTable SET Amount = @TransactionAmount WHERE CFTransactionGroup=1

--EnumCFTransactionGroup : Interest_Paid = 3, EnumCFDataType : Financila_Cost_of_SCI = 3, EnumCISSetup : Financial_Cost = 10
SET @TransactionAmount = (-1)*ISNULL((SELECT SUM(TT.ClosingBalance) FROM #Temp_Table AS TT WHERE TT.AccountHeadID IN (SELECT HH.AccountHeadID FROM CIStatementSetup AS HH WHERE HH.CIHeadType=10)),0) 
UPDATE #TempCFTable SET Amount = @TransactionAmount WHERE CFTransactionGroup=3


--EnumCFTransactionGroup : Income_Tax_paid = 4, EnumCFDataType : Income_Tax_of_SCI = 4, EnumCISSetup :  Income_Tax = 7
SET @TransactionAmount = (-1)*ISNULL((SELECT SUM(TT.ClosingBalance) FROM #Temp_Table AS TT WHERE TT.AccountHeadID IN (SELECT HH.AccountHeadID FROM CIStatementSetup AS HH WHERE HH.CIHeadType=7)),0) 
UPDATE #TempCFTable SET Amount = @TransactionAmount WHERE CFTransactionGroup=4

--EnumCFTransactionGroup : Fixed_Asset_Depreciation_Cost = 17, EnumCFDataType : Fixed_Asset_Depreciation_Cost_Of_SCI = 17
SET @DepreciationAmount = ISNULL((SELECT SUM(TT.ClosingBalance) FROM #Temp_Table AS TT WHERE TT.AccountHeadID IN (SELECT HH.SubGroupID FROM #TempCFTable AS HH WHERE HH.CFDataType=17)),0) 
UPDATE #TempCFTable SET Amount = @DepreciationAmount WHERE CFTransactionGroup=17

--EnumCFTransactionGroup : Acquisition_of_Fixed_Asset = 5, EnumCFDataType : Fixed_Asset_SFP_Changes_Less_Depreciation = 5
SET @TransactionAmount = (-1)*ISNULL((SELECT SUM(TT.ChangesAmount) FROM #Temp_Table AS TT WHERE TT.AccountHeadID IN (SELECT HH.SubGroupID FROM #TempCFTable AS HH WHERE HH.CFDataType=5)),0) 
UPDATE #TempCFTable SET Amount = (@TransactionAmount-@DepreciationAmount) WHERE CFTransactionGroup=5


--EnumCFTransactionGroup : Fixed_Doposit = 6, EnumCFDataType : Fixed_Deposit_SFP_Changes = 6
SET @TransactionAmount = (-1)*ISNULL((SELECT SUM(TT.ChangesAmount) FROM #Temp_Table AS TT WHERE TT.AccountHeadID IN (SELECT HH.SubGroupID FROM #TempCFTable AS HH WHERE HH.CFDataType=6)),0) 
UPDATE #TempCFTable SET Amount = @TransactionAmount WHERE CFTransactionGroup=6


--EnumCFTransactionGroup : Intengible_Assets_Depreciation_Cost = 18, EnumCFDataType : Intengible_Assets_Depreciation_Cost_Of_SCI = 18
SET @DepreciationAmount = ISNULL((SELECT SUM(TT.ClosingBalance) FROM #Temp_Table AS TT WHERE TT.AccountHeadID IN (SELECT HH.SubGroupID FROM #TempCFTable AS HH WHERE HH.CFDataType=18)),0) 
UPDATE #TempCFTable SET Amount = @DepreciationAmount WHERE CFTransactionGroup=18

--EnumCFTransactionGroup : Acquisition_of_Intengible_Asstes = 7, EnumCFDataType : Intengible_Asstes_SFP_Changes_Less_Depreciation = 7
SET @TransactionAmount = (-1)*ISNULL((SELECT SUM(TT.ChangesAmount) FROM #Temp_Table AS TT WHERE TT.AccountHeadID IN (SELECT HH.SubGroupID FROM #TempCFTable AS HH WHERE HH.CFDataType=7)),0) 
UPDATE #TempCFTable SET Amount = (@TransactionAmount-@DepreciationAmount) WHERE CFTransactionGroup=7

--EnumCFTransactionGroup : Capital_WIP = 8, EnumCFDataType : Investment_SFP_Changes = 8
SET @TransactionAmount = (-1)*ISNULL((SELECT SUM(TT.ChangesAmount) FROM #Temp_Table AS TT WHERE TT.AccountHeadID IN (SELECT HH.SubGroupID FROM #TempCFTable AS HH WHERE HH.CFDataType=8)),0) 
UPDATE #TempCFTable SET Amount = @TransactionAmount WHERE CFTransactionGroup=8


--EnumCFTransactionGroup : Payment_of_Lease_Loan = 9, EnumCFDataType : None_Current_Loan_SFP_Changes = 9
SET @TransactionAmount = ISNULL((SELECT SUM(TT.ChangesAmount) FROM #Temp_Table AS TT WHERE TT.AccountHeadID IN (SELECT HH.SubGroupID FROM #TempCFTable AS HH WHERE HH.CFDataType=9)),0) 
UPDATE #TempCFTable SET Amount = @TransactionAmount WHERE CFTransactionGroup=9

--EnumCFTransactionGroup : Payment_of_Term_Loan = 10, EnumCFDataType : Current_Loan_SFP_Changes = 10
SET @TransactionAmount = ISNULL((SELECT SUM(TT.ChangesAmount) FROM #Temp_Table AS TT WHERE TT.AccountHeadID IN (SELECT HH.SubGroupID FROM #TempCFTable AS HH WHERE HH.CFDataType=10)),0) 
UPDATE #TempCFTable SET Amount = @TransactionAmount WHERE CFTransactionGroup=10

--EnumCFTransactionGroup : Payment_of_Dividend = 11, EnumCFDataType : Dividend_SFP_Changes = 11
SET @TransactionAmount = ISNULL((SELECT SUM(TT.ChangesAmount) FROM #Temp_Table AS TT WHERE TT.AccountHeadID IN (SELECT HH.SubGroupID FROM #TempCFTable AS HH WHERE HH.CFDataType=11)),0) 
UPDATE #TempCFTable SET Amount = @TransactionAmount WHERE CFTransactionGroup=11


--EnumCFTransactionGroup : Cost_of_Sales = 12, EnumCFDataType : COGS_of_SCI = 12, EnumCISSetup : Cost_Of_Goods_Sold = 3
SET @TransactionAmount = ISNULL((SELECT SUM(TT.ClosingBalance) FROM #Temp_Table AS TT WHERE TT.AccountHeadID IN (SELECT HH.AccountHeadID FROM CIStatementSetup AS HH WHERE HH.CIHeadType=3)),0)
UPDATE #TempCFTable SET Amount = @TransactionAmount WHERE CFTransactionGroup=12

--EnumCFTransactionGroup : Administrative_Cost = 13, EnumCFDataType : Administrative_Cost_Of_SCI = 13
UPDATE #TempCFTable 
SET Amount = ISNULL((SELECT HH.ClosingBalance FROM #Temp_Table AS HH WHERE HH.AccountHeadID=TT.SubGroupID),0) 
FROM #TempCFTable AS TT WHERE TT.CFTransactionGroup=13

--EnumCFTransactionGroup : Other_Income = 20, EnumCFDataType : Other_Income_of_SCI = 20 , EnumCISSetup : Other_Income = 5
SET @TransactionAmount = ISNULL((SELECT SUM(TT.ClosingBalance) FROM #Temp_Table AS TT WHERE TT.AccountHeadID IN (SELECT HH.AccountHeadID FROM CIStatementSetup AS HH WHERE HH.CIHeadType=5)),0)
UPDATE #TempCFTable SET Amount = @TransactionAmount WHERE CFTransactionGroup=20

--EnumCFTransactionGroup : Selling_Cost = 14, EnumCFDataType : Selling_Cost_of_SCI = 14
UPDATE #TempCFTable 
SET Amount = ISNULL((SELECT HH.ClosingBalance FROM #Temp_Table AS HH WHERE HH.AccountHeadID=TT.SubGroupID),0) 
FROM #TempCFTable AS TT WHERE TT.CFTransactionGroup=14


--EnumCFTransactionGroup : Current_Asset_Chnages = 15, EnumCFDataType : Current_Asset_Chnages_Of_SFP = 15
UPDATE #TempCFTable 
SET Amount = ISNULL((SELECT SUM(HH.ChangesAmount) FROM #Temp_Table AS HH WHERE HH.AccountHeadID=TT.SubGroupID),0) 
FROM #TempCFTable AS TT WHERE TT.CFTransactionGroup=15

--EnumCFTransactionGroup : Current_Libility_Chnages = 16, EnumCFDataType : Current_Libility_Chnages_Of_SFP = 16
UPDATE #TempCFTable 
SET Amount = (-1)*ISNULL((SELECT SUM(HH.ChangesAmount) FROM #Temp_Table AS HH WHERE HH.AccountHeadID=TT.SubGroupID),0) 
FROM #TempCFTable AS TT WHERE TT.CFTransactionGroup=16


--EnumCFTransactionGroup : Current_Libility_Chnages = 16, EnumCFDataType : Current_Libility_Chnages_Of_SFP = 16
UPDATE #TempCFTable 
SET Amount = ISNULL((SELECT SUM(HH.ClosingBalance) FROM #Temp_Table AS HH WHERE HH.AccountHeadID=TT.SubGroupID),0) 
FROM #TempCFTable AS TT WHERE TT.CFTransactionGroup=19

--Fixed for Expenses
 --Cost_of_Sales = 12, // Fixed for COGS_of_SCI
 --Administrative_Cost = 13, // Fixed for Administrative_Cost_Of_SCI //Choice Multiple Expenditure Subgroup Head with Same Caption
 --Selling_Cost = 14, // Fixed for Selling_Cost_of_SCI //Choice Multiple Expenditure Subgroup Head with Same Caption
 --Other_Income = 20 // Fixed for Other_Income_of_SCI

--//Fixed for Changes_In_CA_AND_CL
--Current_Asset_Chnages = 15, // Fixed for Current_Asset_Chnages_Of_SFP //Choice Multiple Asset Subgroup Head with Different Caption
--Current_Libility_Chnages = 16, // Fixed for Current_Libility_Chnages_Of_SFP //Choice Multiple Libility Subgroup Head with Different Caption

--//Fixed for Depreciation      
--Fixed_Asset_Depreciation_Cost = 17, // Fixed for Fixed_Asset_Depreciation_Cost_Of_SCI //Choice Multiple Expenditure Subgroup Head with Same Caption
--Intengible_Assets_Depreciation_Cost = 18, // Fixed for Intengible_Assets_Depreciation_Cost_Of_SCI //Choice Multiple Expenditure Subgroup Head with Same Caption
--EnumCFTransactionGroup : Cash_Paid = 2, EnumCFDataType : COGS_of_SCI_CA_And_CL_Chnages = 2
SET @COGSAmount =ISNULL((SELECT SUM(TT.Amount) AS Amount FROM #TempCFTable AS TT WHERE TT.CFTransactionGroup IN (12)),0)
SET @AdministrativeCost =ISNULL((SELECT SUM(TT.Amount) AS Amount FROM #TempCFTable AS TT WHERE TT.CFTransactionGroup IN (13)),0)
SET @SellingCost =ISNULL((SELECT SUM(TT.Amount) AS Amount FROM #TempCFTable AS TT WHERE TT.CFTransactionGroup IN (14)),0)
SET @DepreciationAmount =ISNULL((SELECT SUM(TT.Amount) AS Amount FROM #TempCFTable AS TT WHERE TT.CFTransactionGroup IN (17,18)),0)
SET @CACLChangesAmount =ISNULL((SELECT SUM(TT.Amount) AS Amount FROM #TempCFTable AS TT WHERE TT.CFTransactionGroup IN (15,16)),0)

--Administrative Cost = Administrative Expenses + Depreciation Amount
SET @AdministrativeCost = (@AdministrativeCost + @DepreciationAmount)
--UPDATE #TempCFTable SET Amount = @AdministrativeCost  WHERE CFTransactionGroup=13

--OverHead Amount = Administrative Cost + Selling Cost
SET @OverHeadCost = (@AdministrativeCost+ @SellingCost)

--Expenses Amount = COGS Amount + OverHead Amount
SET @ExpensesAmount =(@COGSAmount+@OverHeadCost)

--Payment Amount = Expenses Amount + Changes In CA&CL - Depreciation Amount
SET @TransactionAmount =(-1)*(@ExpensesAmount+@CACLChangesAmount-@DepreciationAmount)
UPDATE #TempCFTable SET Amount = @TransactionAmount, CashFlowSetupID=999 WHERE CFTransactionGroup=2

--SELECT * FROM #TempCFTable AS TT WHERE TT.CFTransactionGroup IN (12,13,14,20)
--SELECT SUM(TT.Amount) AS Amount FROM #TempCFTable AS TT WHERE TT.CFTransactionGroup IN (12,13,14)
--SELECT * FROM #TempCFTable AS TT ORDER BY TT.CFTransactionCategory, TT.CFTransactionGroup, TT.CFDataType, TT.CashFlowSetupID ASC


CREATE TABLE #FinalCFTable(
								ID int IDENTITY(1,1) PRIMARY KEY,
								CashFlowSetupID	int,
								CFTransactionCategory	smallint,
								CFTransactionGroup	smallint,
								CFDataType	int,
								SubGroupID	int,
								DisplayCaption	varchar(1024),
								Amount decimal(30,17),
								SubGroupCode Varchar(512),
								SubGroupName Varchar(512),
								Remarks Varchar(512)
						 )


IF(@IsPaymentDetails=1)
BEGIN
	--//Fixed for Expenses
	--Cost_of_Sales = 12, // Fixed for COGS_of_SCI
	--Administrative_Cost = 13, // Fixed for Administrative_Cost_Of_SCI //Choice Multiple Expenditure Subgroup Head with Same Caption
	--Selling_Cost = 14, // Fixed for Selling_Cost_of_SCI //Choice Multiple Expenditure Subgroup Head with Same Caption

	--//Fixed for Changes_In_CA_AND_CL
	--Current_Asset_Chnages = 15, // Fixed for Current_Asset_Chnages_Of_SFP //Choice Multiple Asset Subgroup Head with Different Caption
	--Current_Libility_Chnages = 16, // Fixed for Current_Libility_Chnages_Of_SFP //Choice Multiple Libility Subgroup Head with Different Caption

	--//Fixed for Depreciation      
	--Fixed_Asset_Depreciation_Cost = 17, // Fixed for Fixed_Asset_Depreciation_Cost_Of_SCI //Choice Multiple Expenditure Subgroup Head with Same Caption
	--Intengible_Assets_Depreciation_Cost = 18, // Fixed for Intengible_Assets_Depreciation_Cost_Of_SCI //Choice Multiple Expenditure Subgroup Head with Same Caption

	--//Opening_Balance
	--Opening_Balance = 19, // Fixed for Opening_Balance_of_FPS // Choice Multiple Asset Subgroup Head with no Caption

	--//Fixed for Expenses
	--Other_Income = 20 // Fixed for Other_Income_of_SCI

	--Expenses :
	INSERT INTO #FinalCFTable (CashFlowSetupID,		CFTransactionCategory,		CFTransactionGroup,		CFDataType,		SubGroupID,		DisplayCaption,			Amount,		SubGroupCode,		SubGroupName,		Remarks)
						VALUES(0,					4,							0,						0,				0,				'Expenses :',			0.00,		'',					'',					'')

	INSERT INTO #FinalCFTable
	SELECT * FROM #TempCFTable AS TT WHERE TT.CFTransactionGroup IN (12) ORDER BY TT.CFTransactionGroup

	--INSERT INTO #FinalCFTable (CashFlowSetupID,		CFTransactionCategory,		CFTransactionGroup,		CFDataType,		SubGroupID,		DisplayCaption,			Amount,		SubGroupCode,		SubGroupName,		Remarks)
	--					VALUES(0,					4,							0,						0,				0,				'Overhead :',			0.00,		'',					'',					'')

	INSERT INTO #FinalCFTable
	SELECT * FROM #TempCFTable AS TT WHERE TT.CFTransactionGroup IN (13,14) ORDER BY TT.CFTransactionGroup
		
	SET @TransactionAmount = ISNULL((SELECT SUM(TT.Amount) FROM #TempCFTable AS TT WHERE TT.CFTransactionGroup IN (12,13,14)),0)
	INSERT INTO #FinalCFTable (CashFlowSetupID,		CFTransactionCategory,		CFTransactionGroup,		CFDataType,		SubGroupID,		DisplayCaption,		Amount,					SubGroupCode,		SubGroupName,		Remarks)
						VALUES(1001,				4,							0,						0,				0,				'Sub. Total:   A',	@TransactionAmount,		'',					'',				'')

	
	--Changes in CA/ CL:
	INSERT INTO #FinalCFTable (CashFlowSetupID,		CFTransactionCategory,		CFTransactionGroup,		CFDataType,		SubGroupID,		DisplayCaption,			Amount,		SubGroupCode,		SubGroupName,		Remarks)
						VALUES(0,					5,							0,						0,				0,				'Changes in CA/ CL:',			0.00,		'',					'',					'')

	INSERT INTO #FinalCFTable
	SELECT * FROM #TempCFTable AS TT WHERE TT.CFTransactionGroup IN (15,16) ORDER BY TT.CFTransactionGroup
		
	SET @TransactionAmount = ISNULL((SELECT SUM(TT.Amount) FROM #TempCFTable AS TT WHERE TT.CFTransactionGroup IN (15,16)),0)
	INSERT INTO #FinalCFTable (CashFlowSetupID,		CFTransactionCategory,		CFTransactionGroup,		CFDataType,		SubGroupID,		DisplayCaption,		Amount,					SubGroupCode,		SubGroupName,		Remarks)
						VALUES(1002,				5,							0,						0,				0,				'Sub. Total:   B',	@TransactionAmount,		'',					'',				'')


	SET @TransactionAmount = ISNULL((SELECT SUM(TT.Amount) FROM #FinalCFTable AS TT WHERE TT.CashFlowSetupID IN (1001,1002)),0)
	INSERT INTO #FinalCFTable (CashFlowSetupID,		CFTransactionCategory,		CFTransactionGroup,		CFDataType,		SubGroupID,		DisplayCaption,			Amount,					SubGroupCode,		SubGroupName,		Remarks)
						VALUES(1003,				5,							0,						0,				0,				'Grand Total ( A+B)',	@TransactionAmount,		'',					'',				'')
						

	--Depreciation:
	INSERT INTO #FinalCFTable
	SELECT * FROM #TempCFTable AS TT WHERE TT.CFTransactionGroup IN (17,18) ORDER BY TT.CFTransactionGroup
	UPDATE #FinalCFTable SET CFTransactionCategory=5  WHERE CFTransactionGroup IN (17,18) 

	--Net Payment
	SET @TransactionAmount = (ISNULL((SELECT SUM(TT.Amount) FROM #FinalCFTable AS TT WHERE TT.CashFlowSetupID IN (1001,1002)),0) - ISNULL((SELECT SUM(TT.Amount) FROM #TempCFTable AS TT WHERE TT.CFTransactionGroup IN (17,18)),0))
	INSERT INTO #FinalCFTable (CashFlowSetupID,		CFTransactionCategory,		CFTransactionGroup,		CFDataType,		SubGroupID,		DisplayCaption,			Amount,					SubGroupCode,		SubGroupName,		Remarks)
						VALUES(1004,				5,							0,						0,				0,				'TOTAL',				@TransactionAmount,		'',					'',				'')
END
ELSE
BEGIN
	--//Fixed for Operating_Activities
	--Cash_Receipts = 1, // Fixed for Net_TrunOver_of_SCI
	--Other_Income = 20 // Fixed for Other_Income_of_SCI
	--Cash_Paid = 2,  // Fixed for COGS_of_SCI_CA_And_CL_Chnages
	--Interest_Paid = 3, // Fixed for Financila_Cost_of_SCI
	--Income_Tax_paid = 4, // Fixed for Income_Tax_of_SCI

	--//Fixed for Investing_Activities
	--Acquisition_of_Fixed_Asset = 5, //Fixed_Asset_SFP_Changes_Less_Depreciation. //Choice a Asset Subgroup Head
	--Fixed_Doposit = 6,//Fixed_Deposit_SFP_Changes  // Choice a Asset Subgroup Head
	--Acquisition_of_Intengible_Asstes = 7,//Fixed for Intengible_Asstes_SFP_Changes_Less_Depreciation // Choice a Asset Subgroup Head
	--Capital_WIP = 8, //Fixed for Investment_SFP_Changes // Choice a Asset Subgroup Head

	--//Fixed for Financing_Activities
	--Payment_of_Lease_Loan = 9, //Fixed for None_Current_Loan_SFP_Changes // Choice a Libility Subgroup Head
	--Payment_of_Term_Loan = 10, //Fixed for Current_Loan_SFP_Changes // Choice a Libility Subgroup Head
	--Payment_of_Dividend = 11, //Fixed for Dividend_SFP_Changes // Choice a Libility Subgroup Head

	--OPERATING ACTIVITIES
	INSERT INTO #FinalCFTable (CashFlowSetupID,		CFTransactionCategory,		CFTransactionGroup,		CFDataType,		SubGroupID,		DisplayCaption,			Amount,		SubGroupCode,		SubGroupName,		Remarks)
						VALUES(0,					1,							0,						0,				0,				'CASH FLOWS FROM OPERATING ACTIVITIES',	0.00,		'',					'A',				'')

	INSERT INTO #FinalCFTable
	SELECT * FROM #TempCFTable AS TT WHERE TT.CFTransactionGroup IN (1) ORDER BY TT.CFTransactionGroup
	INSERT INTO #FinalCFTable
	SELECT * FROM #TempCFTable AS TT WHERE TT.CFTransactionGroup IN (20) ORDER BY TT.CFTransactionGroup -- Other Income 
	INSERT INTO #FinalCFTable
	SELECT * FROM #TempCFTable AS TT WHERE TT.CFTransactionGroup IN (2) ORDER BY TT.CFTransactionGroup

	INSERT INTO #FinalCFTable
	SELECT * FROM #TempCFTable AS TT WHERE TT.CFTransactionGroup IN (3,4) ORDER BY TT.CFTransactionGroup

	SET @TransactionAmount = ISNULL((SELECT SUM(TT.Amount) FROM #TempCFTable AS TT WHERE TT.CFTransactionGroup IN (1,20,2,3,4)),0)
	INSERT INTO #FinalCFTable (CashFlowSetupID,		CFTransactionCategory,		CFTransactionGroup,		CFDataType,		SubGroupID,		DisplayCaption,								Amount,					SubGroupCode,		SubGroupName,		Remarks)
						VALUES(1001,				1,							0,						0,				0,				'Net Cash Flows from Operating Activities',	@TransactionAmount,		'',					'',				'')


	--INVESTING ACTIVITIES
	INSERT INTO #FinalCFTable (CashFlowSetupID,		CFTransactionCategory,		CFTransactionGroup,		CFDataType,		SubGroupID,		DisplayCaption,			Amount,		SubGroupCode,		SubGroupName,		Remarks)
						VALUES(0,					2,							0,						0,				0,				'CASH FLOWS FROM INVESTING ACTIVITIES',	0.00,		'',					'B',				'')

	INSERT INTO #FinalCFTable
	SELECT * FROM #TempCFTable AS TT WHERE TT.CFTransactionGroup IN (5,6,7,8) ORDER BY TT.CFTransactionGroup

	SET @TransactionAmount = ISNULL((SELECT SUM(TT.Amount) FROM #TempCFTable AS TT WHERE TT.CFTransactionGroup IN (5,6,7,8)),0)
	INSERT INTO #FinalCFTable (CashFlowSetupID,		CFTransactionCategory,		CFTransactionGroup,		CFDataType,		SubGroupID,		DisplayCaption,								Amount,					SubGroupCode,		SubGroupName,		Remarks)
						VALUES(1002,				2,							0,						0,				0,				'Net Cash Flows from Investing Activities',	@TransactionAmount,		'',					'',				'')

	--FINANCING ACTIVITIES
	INSERT INTO #FinalCFTable (CashFlowSetupID,		CFTransactionCategory,		CFTransactionGroup,		CFDataType,		SubGroupID,		DisplayCaption,			Amount,		SubGroupCode,		SubGroupName,		Remarks)
						VALUES(0,					3,							0,						0,				0,				'CASH FLOWS FROM FINANCING ACTIVITIES',	0.00,		'',					'C',				'')

	INSERT INTO #FinalCFTable
	SELECT * FROM #TempCFTable AS TT WHERE TT.CFTransactionGroup IN (9,10,11) ORDER BY TT.CFTransactionGroup

	SET @TransactionAmount = ISNULL((SELECT SUM(TT.Amount) FROM #TempCFTable AS TT WHERE TT.CFTransactionGroup IN (9,10,11)),0)
	INSERT INTO #FinalCFTable (CashFlowSetupID,		CFTransactionCategory,		CFTransactionGroup,		CFDataType,		SubGroupID,		DisplayCaption,								Amount,					SubGroupCode,		SubGroupName,		Remarks)
						VALUES(1003,				3,							0,						0,				0,				'Net Cash Flows from Financing Activities',	@TransactionAmount,		'',					'',				'')

	--Net_Increase =8
	SET @TransactionAmount = ISNULL((SELECT SUM(TT.Amount) FROM #FinalCFTable AS TT WHERE TT.CashFlowSetupID IN (1001,1002,1003)),0)
	INSERT INTO #FinalCFTable (CashFlowSetupID,		CFTransactionCategory,		CFTransactionGroup,		CFDataType,		SubGroupID,		DisplayCaption,											Amount,					SubGroupCode,		SubGroupName,		Remarks)
						VALUES(1004,				8,							0,						0,				0,				'Net changes in Cash and Cash Equivalents (A+B+C)',	@TransactionAmount,		'',					'',				'')


	--Opening_Balance = 7
	SET @TransactionAmount = ISNULL((SELECT SUM(TT.Amount) FROM #TempCFTable AS TT WHERE TT.CFTransactionCategory IN (7)),0)
	INSERT INTO #FinalCFTable (CashFlowSetupID,		CFTransactionCategory,		CFTransactionGroup,		CFDataType,		SubGroupID,		DisplayCaption,											Amount,					SubGroupCode,		SubGroupName,		Remarks)
						VALUES(1005,				7,							0,						0,				0,				'Cash and Cash Equivalents at the Beginning of the period',	@TransactionAmount,		'',					'',				'')

	--Closing_Balance=9
	SET @TransactionAmount = ISNULL((SELECT SUM(TT.Amount) FROM #FinalCFTable AS TT WHERE TT.CashFlowSetupID IN (1004,1005)),0)
	INSERT INTO #FinalCFTable (CashFlowSetupID,		CFTransactionCategory,		CFTransactionGroup,		CFDataType,		SubGroupID,		DisplayCaption,												Amount,					SubGroupCode,		SubGroupName,		Remarks)
						VALUES(1006,				9,							0,						0,				0,				'Cash and Cash Equivalents at the Closing of the period',	@TransactionAmount,		'',					'',				'')
END

SELECT * FROM #FinalCFTable  ORDER BY ID
DROP TABLE #FinalCFTable
DROP TABLE #TempCFTable
DROP TABLE #Temp_Table
COMMIT TRAN




GO
/****** Object:  StoredProcedure [dbo].[SP_IncomeStatement]    Script Date: 3/2/2017 1:53:03 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[SP_IncomeStatement]
(		
	@BUID as int,
	@StartDate as date,
	@EndDate as date,
	@ParenHeadID as int,
	@AccountType as int
)
AS
BEGIN TRAN
--DECLARE
--@BUID as int,
--@StartDate as date,
--@EndDate as date,
--@ParenHeadID as int,
--@AccountType as int

--SET @BUID=0
--SET @StartDate='01 Jul 2016'
--SET @EndDate ='27 Feb 2017'
--SET @ParenHeadID=0
--SET @AccountType = 4

--ComponentID{Asset = 2,Laibility = 3,OwnerEquity=4,Income = 5,Expeness = 6}
DECLARE
@ComponentIDs as Varchar(3)
IF(@ParenHeadID>0)
BEGIN
	SET @ComponentIDs=@ParenHeadID
END
ELSE
BEGIN
	SET @ComponentIDs='5,6'
END

PRINT @ComponentIDs
CREATE TABLE #Temp_Table(		
							AccountHeadID int,
							AccountCode varchar(128),
							AccountHeadName varchar(512),
							ParentAccountHeadID int,
							ComponentType smallint,
							AccountType smallint,							
							OpenningBalance decimal(30,17),
							DebitTransaction decimal(30,17),
							CreditTransaction decimal(30,17),							
							ClosingBalance decimal(30,17)
						)

CREATE TABLE #Result_Table(		
							AccountHeadID int,
							AccountCode varchar(128),
							AccountHeadName varchar(512),
							ParentAccountHeadID int,
							ComponentType smallint,
							AccountType smallint,							
							OpenningBalance decimal(30,17),
							DebitTransaction decimal(30,17),
							CreditTransaction decimal(30,17),							
							ClosingBalance decimal(30,17)
						)

CREATE NONCLUSTERED INDEX #IX_Temp2_1 ON #Temp_Table(AccountHeadID)

INSERT INTO #Temp_Table		(	AccountHeadID,		AccountCode,			AccountHeadName,		ParentAccountHeadID,	AccountType)
					SELECT		AccountHeadID,		AccountCode,			AccountHeadName,		ParentHeadID,			AccountType FROM dbo.GetAccountHeadByComponents(@ComponentIDs)


--EnumAccountType{None = 0,Component = 1,Segment =2,Group = 3,SubGroup = 4,Ledger = 5}
--Update ledger transaction, ComponentID, Openning Balance
--OperationType 1 means FreshVoucher
IF(@BUID>0)
BEGIN
	UPDATE #Temp_Table
	SET	ComponentType=dbo.GetComponentID(TT.AccountHeadID),
		OpenningBalance=0,
		DebitTransaction=(SELECT ISNULL(SUM(VD.Amount),0) FROM View_VoucherDetail AS VD WHERE VD.BUID=@BUID AND VD.OperationType=1 AND VD.IsDebit=1 AND VD.AccountHeadID=TT.AccountHeadID AND VD.VoucherID IN(SELECT V.VoucherID FROM Voucher AS V WHERE V.AuthorizedBy!=0 AND CONVERT(DATE,CONVERT(VARCHAR(12),VoucherDate,106)) BETWEEN CONVERT(DATE,CONVERT(VARCHAR(12),@StartDate,106)) AND CONVERT(DATE,CONVERT(VARCHAR(12),@EndDate,106)))),
		CreditTransaction=(SELECT ISNULL(SUM(VD.Amount),0) FROM View_VoucherDetail AS VD WHERE VD.BUID=@BUID AND VD.OperationType=1 AND VD.IsDebit=0 AND VD.AccountHeadID=TT.AccountHeadID AND VD.VoucherID IN(SELECT V.VoucherID FROM Voucher AS V WHERE V.AuthorizedBy!=0 AND CONVERT(DATE,CONVERT(VARCHAR(12),VoucherDate,106)) BETWEEN CONVERT(DATE,CONVERT(VARCHAR(12),@StartDate,106)) AND CONVERT(DATE,CONVERT(VARCHAR(12),@EndDate,106)))),
		ClosingBalance=0
	FROM #Temp_Table AS TT
END
ELSE
BEGIN
	UPDATE #Temp_Table
	SET	ComponentType=dbo.GetComponentID(TT.AccountHeadID),
		OpenningBalance=0,
		DebitTransaction=(SELECT ISNULL(SUM(VD.Amount),0) FROM View_VoucherDetail AS VD WHERE VD.OperationType=1 AND VD.IsDebit=1 AND VD.AccountHeadID=TT.AccountHeadID AND VD.VoucherID IN(SELECT V.VoucherID FROM Voucher AS V WHERE V.AuthorizedBy!=0 AND CONVERT(DATE,CONVERT(VARCHAR(12),VoucherDate,106)) BETWEEN CONVERT(DATE,CONVERT(VARCHAR(12),@StartDate,106)) AND CONVERT(DATE,CONVERT(VARCHAR(12),@EndDate,106)))),
		CreditTransaction=(SELECT ISNULL(SUM(VD.Amount),0) FROM View_VoucherDetail AS VD WHERE VD.OperationType=1 AND VD.IsDebit=0 AND VD.AccountHeadID=TT.AccountHeadID AND VD.VoucherID IN(SELECT V.VoucherID FROM Voucher AS V WHERE V.AuthorizedBy!=0 AND CONVERT(DATE,CONVERT(VARCHAR(12),VoucherDate,106)) BETWEEN CONVERT(DATE,CONVERT(VARCHAR(12),@StartDate,106)) AND CONVERT(DATE,CONVERT(VARCHAR(12),@EndDate,106)))),
		ClosingBalance=0
	FROM #Temp_Table AS TT
END

--Update Ledger debit/credit Transaction
UPDATE #Temp_Table
SET OpenningBalance=(SELECT ISNULL(SUM(ABC.OpenningBalance),0) FROM #Temp_Table AS ABC WHERE ABC.ParentAccountHeadID=TT.AccountHeadID),
	DebitTransaction=(SELECT ISNULL(SUM(ABC.DebitTransaction),0) FROM #Temp_Table AS ABC WHERE ABC.ParentAccountHeadID=TT.AccountHeadID),
	CreditTransaction=(SELECT ISNULL(SUM(ABC.CreditTransaction),0) FROM #Temp_Table AS ABC WHERE ABC.ParentAccountHeadID=TT.AccountHeadID)
FROM #Temp_Table AS TT WHERE TT.AccountType=4 --EnumAccountType{None = 0,Component = 1,Segment =2,Group = 3,SubGroup = 4,Ledger = 5}

--Update SubGroup debit/credit Transaction
UPDATE #Temp_Table
SET OpenningBalance=(SELECT ISNULL(SUM(ABC.OpenningBalance),0) FROM #Temp_Table AS ABC WHERE ABC.ParentAccountHeadID=TT.AccountHeadID),
	DebitTransaction=(SELECT ISNULL(SUM(ABC.DebitTransaction),0) FROM #Temp_Table AS ABC WHERE ABC.ParentAccountHeadID=TT.AccountHeadID),
	CreditTransaction=(SELECT ISNULL(SUM(ABC.CreditTransaction),0) FROM #Temp_Table AS ABC WHERE ABC.ParentAccountHeadID=TT.AccountHeadID)
FROM #Temp_Table AS TT WHERE TT.AccountType=3 --EnumAccountType{None = 0,Component = 1,Segment =2,Group = 3,SubGroup = 4,Ledger = 5}

--Update Group debit/credit Transaction
UPDATE #Temp_Table
SET OpenningBalance=(SELECT ISNULL(SUM(ABC.OpenningBalance),0) FROM #Temp_Table AS ABC WHERE ABC.ParentAccountHeadID=TT.AccountHeadID),
	DebitTransaction=(SELECT ISNULL(SUM(ABC.DebitTransaction),0) FROM #Temp_Table AS ABC WHERE ABC.ParentAccountHeadID=TT.AccountHeadID),
	CreditTransaction=(SELECT ISNULL(SUM(ABC.CreditTransaction),0) FROM #Temp_Table AS ABC WHERE ABC.ParentAccountHeadID=TT.AccountHeadID)
FROM #Temp_Table AS TT WHERE TT.AccountType=2 --EnumAccountType{None = 0,Component = 1,Segment =2,Group = 3,SubGroup = 4,Ledger = 5}

--Update Component debit/credit Transaction
UPDATE #Temp_Table
SET OpenningBalance=(SELECT ISNULL(SUM(ABC.OpenningBalance),0) FROM #Temp_Table AS ABC WHERE ABC.ParentAccountHeadID=TT.AccountHeadID),
	DebitTransaction=(SELECT ISNULL(SUM(ABC.DebitTransaction),0) FROM #Temp_Table AS ABC WHERE ABC.ParentAccountHeadID=TT.AccountHeadID),
	CreditTransaction=(SELECT ISNULL(SUM(ABC.CreditTransaction),0) FROM #Temp_Table AS ABC WHERE ABC.ParentAccountHeadID=TT.AccountHeadID)
FROM #Temp_Table AS TT WHERE TT.AccountType=1 --EnumAccountType{None = 0,Component = 1,Segment =2,Group = 3,SubGroup = 4,Ledger = 5}


--Update Closing Balance
UPDATE #Temp_Table
set	ClosingBalance=TT.OpenningBalance+TT.DebitTransaction-TT.CreditTransaction
FROM #Temp_Table AS TT where ComponentType  IN(2,6)

UPDATE #Temp_Table
set	ClosingBalance=TT.OpenningBalance-TT.DebitTransaction+TT.CreditTransaction
FROM #Temp_Table AS TT where ComponentType NOT IN(2,6)
--select * from #Temp_Table


--EnumAccountType{None = 0,Component = 1,Segment =2,Group = 3,SubGroup = 4,Ledger = 5}
INSERT INTO #Result_Table
SELECT * FROM #Temp_Table WHERE AccountType <= @AccountType ORDER BY AccountHeadID


CREATE TABLE #Temp_InvntoryTable(		
									AccountHeadID int,	
									AccountCode varchar(128),
									AccountHeadName varchar(512),						
									IsDebit bit,													
									OpenningBalance decimal(30,17),
									DebitTransaction decimal(30,17),
									CreditTransaction decimal(30,17),							
									ClosingBalance decimal(30,17)
								)

INSERT INTO #Temp_InvntoryTable(AccountHeadID,		AccountCode,	AccountHeadName)
					    SELECT HH.AccountHeadID,	HH.AccountCode, HH.AccountHeadName FROM ChartsOfAccount AS HH WHERE HH.ParentHeadID IN (SELECT MM.AccountHeadID FROM CIStatementSetup AS MM WHERE MM.CIHeadType=11)

DECLARE
@CurrencyID as int
SET @CurrencyID = (SELECT HH.BaseCurrencyID FROM Company AS HH WHERE HH.CompanyID=1)

UPDATE #Temp_InvntoryTable											--@AccountHeadID,	@StartDate,	@CurrencyID,	@IsApproved,	@IsTransactioncount,	@BusinessUnitID
SET IsDebit = (SELECT   TT.IsDebit FROM [dbo].[GetLedgerOpeningBalance](HH.AccountHeadID, @StartDate, @CurrencyID,	1,				1,						@BUID) AS TT),
	DebitTransaction =(SELECT   TT.DebitAmount FROM [dbo].[GetLedgerOpeningBalance](HH.AccountHeadID, @StartDate, @CurrencyID,	1,				1,						@BUID) AS TT),
	CreditTransaction =(SELECT   TT.CreditAmount FROM [dbo].[GetLedgerOpeningBalance](HH.AccountHeadID, @StartDate, @CurrencyID,	1,				1,						@BUID) AS TT)
FROM #Temp_InvntoryTable AS HH 

UPDATE #Temp_InvntoryTable SET OpenningBalance = HH.DebitTransaction FROM #Temp_InvntoryTable AS HH WHERE HH.IsDebit=1
UPDATE #Temp_InvntoryTable SET OpenningBalance = ((-1) * HH.CreditTransaction) FROM #Temp_InvntoryTable AS HH WHERE HH.IsDebit=0
UPDATE #Temp_InvntoryTable SET DebitTransaction = 0, CreditTransaction=0


IF(@BUID>0)
BEGIN
	UPDATE #Temp_InvntoryTable
	SET	DebitTransaction=(SELECT ISNULL(SUM(VD.Amount),0) FROM View_VoucherDetail AS VD WHERE VD.BUID=@BUID AND VD.OperationType=1 AND VD.IsDebit=1 AND VD.AccountHeadID=TT.AccountHeadID AND VD.VoucherID IN(SELECT V.VoucherID FROM Voucher AS V WHERE ISNULL(V.AuthorizedBy,0)!=0 AND CONVERT(DATE,CONVERT(VARCHAR(12),VoucherDate,106)) BETWEEN CONVERT(DATE,CONVERT(VARCHAR(12),@StartDate,106)) AND CONVERT(DATE,CONVERT(VARCHAR(12),@EndDate,106)))),
		CreditTransaction=(SELECT ISNULL(SUM(VD.Amount),0) FROM View_VoucherDetail AS VD WHERE VD.BUID=@BUID AND VD.OperationType=1 AND VD.IsDebit=0 AND VD.AccountHeadID=TT.AccountHeadID AND VD.VoucherID IN(SELECT V.VoucherID FROM Voucher AS V WHERE ISNULL(V.AuthorizedBy,0)!=0 AND CONVERT(DATE,CONVERT(VARCHAR(12),VoucherDate,106)) BETWEEN CONVERT(DATE,CONVERT(VARCHAR(12),@StartDate,106)) AND CONVERT(DATE,CONVERT(VARCHAR(12),@EndDate,106)))),
		ClosingBalance=0
	FROM #Temp_InvntoryTable AS TT
END
ELSE
BEGIN
	UPDATE #Temp_InvntoryTable
	SET	DebitTransaction=(SELECT ISNULL(SUM(VD.Amount),0) FROM View_VoucherDetail AS VD WHERE VD.OperationType=1 AND VD.IsDebit=1 AND VD.AccountHeadID=TT.AccountHeadID AND VD.VoucherID IN(SELECT V.VoucherID FROM Voucher AS V WHERE ISNULL(V.AuthorizedBy,0)!=0 AND CONVERT(DATE,CONVERT(VARCHAR(12),VoucherDate,106)) BETWEEN CONVERT(DATE,CONVERT(VARCHAR(12),@StartDate,106)) AND CONVERT(DATE,CONVERT(VARCHAR(12),@EndDate,106)))),
		CreditTransaction=(SELECT ISNULL(SUM(VD.Amount),0) FROM View_VoucherDetail AS VD WHERE VD.OperationType=1 AND VD.IsDebit=0 AND VD.AccountHeadID=TT.AccountHeadID AND VD.VoucherID IN(SELECT V.VoucherID FROM Voucher AS V WHERE ISNULL(V.AuthorizedBy,0)!=0 AND CONVERT(DATE,CONVERT(VARCHAR(12),VoucherDate,106)) BETWEEN CONVERT(DATE,CONVERT(VARCHAR(12),@StartDate,106)) AND CONVERT(DATE,CONVERT(VARCHAR(12),@EndDate,106)))),
		ClosingBalance=0
	FROM #Temp_InvntoryTable AS TT
END

UPDATE #Temp_InvntoryTable SET	ClosingBalance=TT.OpenningBalance+TT.DebitTransaction-TT.CreditTransaction FROM #Temp_InvntoryTable AS TT

INSERT INTO #Result_Table(AccountHeadID)VALUES(0)
UPDATE #Result_Table
SET AccountCode = '',
	AccountHeadName = 'Opening & Closing Stock of Materials',
	ParentAccountHeadID = 0,
	ComponentType = 2,
	AccountType = 5,
	OpenningBalance = ISNULL((SELECT SUM(TT.OpenningBalance) FROM #Temp_InvntoryTable AS TT),0),
	DebitTransaction = ISNULL((SELECT SUM(TT.DebitTransaction) FROM #Temp_InvntoryTable AS TT),0),
	CreditTransaction = ISNULL((SELECT SUM(TT.CreditTransaction) FROM #Temp_InvntoryTable AS TT),0),
	ClosingBalance = ISNULL((SELECT SUM(TT.ClosingBalance) FROM #Temp_InvntoryTable AS TT),0)
FROM #Result_Table AS HH WHERE HH.AccountHeadID=0


CREATE TABLE #Purchase_Table(		
								AccountHeadID int,		
								ReverseHeadID int,
								ParentAccountHeadID int,
								VoucherDetailID smallint,								
								Amount decimal(30,17)								
							)


IF(@BUID>0)
BEGIN	
	INSERT INTO #Purchase_Table (AccountHeadID,		VoucherDetailID,	Amount)
						  SELECT VD.AccountHeadID,	VD.VoucherDetailID, VD.Amount FROM View_VoucherDetail AS VD WHERE VD.BUID=@BUID AND VD.OperationType=1 AND VD.IsDebit=1 AND  ISNULL(VD.AuthorizedBy,0)!=0 AND CONVERT(DATE,CONVERT(VARCHAR(12),VD.VoucherDate,106)) BETWEEN CONVERT(DATE,CONVERT(VARCHAR(12),@StartDate,106)) AND CONVERT(DATE,CONVERT(VARCHAR(12),@EndDate,106)) AND VD.AccountHeadID IN (SELECT HH.AccountHeadID FROM ChartsOfAccount AS HH WHERE HH.ParentHeadID IN (SELECT MM.AccountHeadID FROM CIStatementSetup AS MM WHERE MM.CIHeadType=11))
END
ELSE
BEGIN
	INSERT INTO #Purchase_Table (AccountHeadID,		VoucherDetailID,	Amount)
						  SELECT VD.AccountHeadID,	VD.VoucherDetailID, VD.Amount FROM View_VoucherDetail AS VD WHERE VD.OperationType=1 AND VD.IsDebit=1 AND  ISNULL(VD.AuthorizedBy,0)!=0 AND CONVERT(DATE,CONVERT(VARCHAR(12),VD.VoucherDate,106)) BETWEEN CONVERT(DATE,CONVERT(VARCHAR(12),@StartDate,106)) AND CONVERT(DATE,CONVERT(VARCHAR(12),@EndDate,106)) AND VD.AccountHeadID IN (SELECT HH.AccountHeadID FROM ChartsOfAccount AS HH WHERE HH.ParentHeadID IN (SELECT MM.AccountHeadID FROM CIStatementSetup AS MM WHERE MM.CIHeadType=11))
END


UPDATE #Purchase_Table
SET ReverseHeadID=(SELECT TOP 1 MM.AccountHeadID FROM VoucherDetail AS MM WHERE MM.IsDebit=0 AND MM.VoucherID=(SELECT VD.VoucherID FROM VoucherDetail AS VD WHERE VD.VoucherDetailID=HH.VoucherDetailID))
FROM #Purchase_Table AS HH

UPDATE #Purchase_Table
SET ParentAccountHeadID=(SELECT MM.ParentHeadID FROM ChartsOfAccount AS MM WHERE MM.AccountHeadID=HH.ReverseHeadID)
FROM #Purchase_Table AS HH


INSERT INTO #Result_Table (AccountHeadID,			DebitTransaction,	AccountType,	ComponentType, OpenningBalance, CreditTransaction,	ClosingBalance)
					SELECT HH.ParentAccountHeadID,	SUM(HH.Amount),		4,				2,				0,				0,					0  FROM #Purchase_Table AS HH GROUP BY HH.ParentAccountHeadID

UPDATE #Result_Table
SET AccountCode=(SELECT MM.AccountCode FROM ChartsOfAccount AS MM WHERE MM.AccountHeadID=HH.AccountHeadID),
	AccountHeadName=(SELECT MM.AccountHeadName FROM ChartsOfAccount AS MM WHERE MM.AccountHeadID=HH.AccountHeadID),
	ParentAccountHeadID=0
FROM #Result_Table AS HH WHERE HH.ComponentType=2 AND HH.AccountHeadID>0

--SELECT * FROM #Purchase_Table ORDER BY AccountHeadID
SELECT * FROM #Result_Table ORDER BY ComponentType, AccountHeadID ASC

DROP TABLE #Purchase_Table
DROP TABLE #Result_Table
DROP TABLE #Temp_InvntoryTable
DROP TABLE #Temp_Table
COMMIT TRAN




GO
/****** Object:  StoredProcedure [dbo].[SP_IUD_CashFlowDmSetup]    Script Date: 3/2/2017 1:53:03 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[SP_IUD_CashFlowDmSetup]
(
	@CashFlowDmSetupID int,
	@CFTransactionCategory smallint,
	@CFTransactionGroup smallint,
	@CFDataType smallint,
	@SubGroupID smallint,
	@DisplayCaption Varchar(512),
	@Remarks Varchar(512),
	@DBUserID int,
	@DBOperation smallint 
)
 As 
 BEGIN TRAN
DECLARE
@PV_DBServerDateTime as DateTime
SET @PV_DBServerDateTime = GETDATE()
IF(@DBOperation=1)--insert
BEGIN
	IF(@CFDataType IN (5,6,7,8,9,10,11,12,20))
	BEGIN
		IF EXISTS(SELECT * FROM CashFlowDmSetup WHERE CFDataType = @CFDataType) --duplicate not allow
		BEGIN
			ROLLBACK

					RAISERROR (N'Your Selected Data type already Exist!!',16,1);
			RETURN
		END
	END
	SET @CashFlowDmSetupID=(SELECT ISNULL(MAX(CashFlowDmSetupID),0)+1 FROM CashFlowDmSetup)   
	INSERT INTO CashFlowDmSetup (CashFlowDmSetupID,		CFTransactionCategory,	CFTransactionGroup,		CFDataType,		SubGroupID,		DisplayCaption,		Remarks,	DBUserID,	DBServerDateTime)
						Values  (@CashFlowDmSetupID,	@CFTransactionCategory,	@CFTransactionGroup,	@CFDataType,	@SubGroupID,	@DisplayCaption,	@Remarks,	@DBUserID,	@PV_DBServerDateTime)
	SELECT * FROM View_CashFlowDmSetup WHERE CashFlowDmSetupID = @CashFlowDmSetupID  
END
IF(@DBOperation = 2)--Update
BEGIN
	
	IF(@CashFlowDmSetupID<=0)
	BEGIN
		ROLLBACK
			RAISERROR (N'Your Selected CashFlowDmSetup Is Invalid Please Refresh and try again!!',16,1);
		RETURN
	END
	Update CashFlowDmSetup SET CFTransactionCategory = @CFTransactionCategory,CFTransactionGroup = @CFTransactionGroup,CFDataType = @CFDataType,SubGroupID = @SubGroupID,DisplayCaption = @DisplayCaption,Remarks = @Remarks,DBUserID = @DBUserID,DBServerDateTime = @PV_DBServerDateTime WHERE CashFlowDmSetupID = @CashFlowDmSetupID
	SELECT * FROM View_CashFlowDmSetup WHERE CashFlowDmSetupID = @CashFlowDmSetupID 
END
IF(@DBOperation = 3)--Delete
BEGIN
	
	IF(@CashFlowDmSetupID<=0)
	BEGIN
		ROLLBACK
			RAISERROR (N'Your Selected CashFlowDmSetup Is Invalid Please Refresh and try again!!',16,1);
		RETURN
	END  
	DELETE FROM CashFlowDmSetup WHERE CashFlowDmSetupID = @CashFlowDmSetupID
END
COMMIT TRAN

GO
/****** Object:  StoredProcedure [dbo].[SP_IUD_CashFlowSetup]    Script Date: 3/2/2017 1:53:03 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[SP_IUD_CashFlowSetup]
(
	@Param_CashFlowSetupID int,
	@Param_CFTransactionCategory smallint,
	@Param_CFTransactionGroup smallint,
	@Param_CFDataType smallint,
	@Param_SubGroupID smallint,
	@Param_DisplayCaption Varchar(512),
	@Param_Remarks Varchar(512),
	@Param_DBUserID int,
	@Param_DBOperation smallint 
)
 As BEGIN TRAN
	DECLARE
	@PV_DBServerDateTime as DateTime
	SET @PV_DBServerDateTime = GETDATE()
	IF(@Param_DBOperation=1)--insert
	BEGIN
	IF(@Param_CFDataType IN (1,2,3,4,5,6,7,8,9,10,11,12,20))
	BEGIN
		IF EXISTS(SELECT * FROM CashFlowSetup WHERE CFDataType = @Param_CFDataType) --duplicate not allow
		BEGIN
			ROLLBACK

					RAISERROR (N'Your Selected Data type already Exist!!',16,1);
			RETURN
		END
	END

		SET @Param_CashFlowSetupID=(SELECT ISNULL(MAX(CashFlowSetupID),0)+1 FROM CashFlowSetup)   
		INSERT INTO CashFlowSetup (	CashFlowSetupID,		CFTransactionCategory,			CFTransactionGroup,			CFDataType,			SubGroupID,			DisplayCaption,			Remarks,		DBUserID,			DBServerDateTime)
							Values (@Param_CashFlowSetupID,	@Param_CFTransactionCategory,	@Param_CFTransactionGroup,	@Param_CFDataType,	@Param_SubGroupID,	@Param_DisplayCaption,	@Param_Remarks,	@Param_DBUserID,	@PV_DBServerDateTime)
			SELECT * FROM View_CashFlowSetup WHERE CashFlowSetupID = @Param_CashFlowSetupID  
	END
	IF(@Param_DBOperation = 2)--Update
	BEGIN
	
		IF(@Param_CashFlowSetupID<=0)
		BEGIN
			ROLLBACK
				RAISERROR (N'Your Selected CashFlowSetup Is Invalid Please Refresh and try again!!',16,1);
			RETURN
		END
		Update CashFlowSetup SET CFTransactionCategory = @Param_CFTransactionCategory,CFTransactionGroup = @Param_CFTransactionGroup,CFDataType = @Param_CFDataType,SubGroupID = @Param_SubGroupID,DisplayCaption = @Param_DisplayCaption,Remarks = @Param_Remarks,DBUserID = @Param_DBUserID,DBServerDateTime = @PV_DBServerDateTime WHERE CashFlowSetupID = @Param_CashFlowSetupID
		SELECT * FROM View_CashFlowSetup WHERE CashFlowSetupID = @Param_CashFlowSetupID 
	END
	IF(@Param_DBOperation = 3)--Delete
	BEGIN
	
		IF(@Param_CashFlowSetupID<=0)
		BEGIN
			ROLLBACK
				RAISERROR (N'Your Selected CashFlowSetup Is Invalid Please Refresh and try again!!',16,1);
			RETURN
		END  
		DELETE FROM CashFlowSetup WHERE CashFlowSetupID = @Param_CashFlowSetupID
	END
 COMMIT TRAN




GO
/****** Object:  UserDefinedFunction [dbo].[FN_GetsReverseAccounts]    Script Date: 3/2/2017 1:53:03 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE FUNCTION [dbo].[FN_GetsReverseAccounts]
(
	@VoucherDetailID as int
)
RETURNS @Result_Table TABLE (
								VoucherDetailID int,
								ReverseHeadID int,
								ReverseHeadCode Varchar(512),
								ReverseHeadName Varchar(1024),
								ReverseAmount decimal(30,17)
							)
AS
BEGIN
--DECLARE
--@VoucherDetailID as int
--SET @VoucherDetailID= 320

DECLARE @Temp_Table	TABLE (								
								ID int IDENTITY(1,1),
								VoucherDetailID int,
								AccountHeadID int,
								Amount decimal(30,17)
							)


DECLARE
@ID as int,
@VoucherID as int,
@IsDebit as bit,
@Amount as decimal(30,17),
@TempAmount as decimal(30,17),
@ReverseAmount as decimal(30,17),
@ReverseHeadID as int,
@UsedAmount as decimal(30,17)

SET @VoucherID = (SELECT HH.VoucherID FROM VoucherDetail AS HH WHERE HH.VoucherDetailID=@VoucherDetailID)
SET @IsDebit = (SELECT HH.IsDebit FROM VoucherDetail AS HH WHERE HH.VoucherDetailID=@VoucherDetailID)
SET @Amount = (SELECT HH.Amount FROM VoucherDetail AS HH WHERE HH.VoucherDetailID=@VoucherDetailID)
SET @TempAmount = (SELECT HH.Amount FROM VoucherDetail AS HH WHERE HH.VoucherDetailID=@VoucherDetailID)
SET @UsedAmount = ISNULL((SELECT SUM(HH.Amount) FROM VoucherDetail AS HH WHERE HH.VoucherID=@VoucherID AND HH.IsDebit=@IsDebit AND HH.VoucherDetailID<@VoucherDetailID),0)

INSERT INTO @Temp_Table(VoucherDetailID,	AccountHeadID,		Amount)
				SELECT  HH.VoucherDetailID,	HH.AccountHeadID,	HH.Amount FROM VoucherDetail AS HH WHERE HH.VoucherID=@VoucherID AND HH.IsDebit!=@IsDebit ORDER BY HH.VoucherDetailID ASC

SET @ID = 1
WHILE(@TempAmount>0)
BEGIN
	SET @ReverseAmount = ISNULL((SELECT HH.Amount FROM @Temp_Table AS HH WHERE HH.ID=@ID),0)
	SET @ReverseHeadID = ISNULL((SELECT HH.AccountHeadID FROM @Temp_Table AS HH WHERE HH.ID=@ID),0)
	IF(@ReverseAmount>0)
	BEGIN
		IF(@UsedAmount>0)
		BEGIN
			IF(@ReverseAmount>=@UsedAmount)
			BEGIN
				SET @ReverseAmount = @ReverseAmount-@UsedAmount
				SET @UsedAmount = 0
			END
			ELSE 
			BEGIN
				SET @UsedAmount = @UsedAmount -  @ReverseAmount
				SET @ReverseAmount  = 0
			END
		END
		IF(@ReverseAmount>0)
		BEGIN
			IF(@ReverseAmount>=@TempAmount)
			BEGIN
				INSERT INTO @Result_Table (VoucherDetailID,  ReverseHeadID,  ReverseAmount)
									VALUES(@VoucherDetailID, @ReverseHeadID, @TempAmount)
				SET @TempAmount = 0
			END
			ELSE
			BEGIN			
				INSERT INTO @Result_Table (VoucherDetailID,  ReverseHeadID,  ReverseAmount)
									VALUES(@VoucherDetailID, @ReverseHeadID, @ReverseAmount)
				SET @TempAmount = (@TempAmount - @ReverseAmount)
			END
		END
	END
	ELSE
	BEGIN
		SET @TempAmount = 0
	END
	SET @ID = (@ID + 1)
END


UPDATE @Result_Table
SET ReverseHeadCode = (SELECT MM.AccountCode FROM ChartsOfAccount AS MM WHERE MM.AccountHeadID=HH.ReverseHeadID),
	ReverseHeadName = (SELECT MM.AccountHeadName FROM ChartsOfAccount AS MM WHERE MM.AccountHeadID=HH.ReverseHeadID)
FROM @Result_Table AS HH

--SELECT * FROM @Temp_Table 
--SELECT * FROM @Result_Table
RETURN
END;
GO
/****** Object:  Table [dbo].[CashFlowDmSetup]    Script Date: 3/2/2017 1:53:03 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[CashFlowDmSetup](
	[CashFlowDmSetupID] [int] NOT NULL,
	[CFTransactionCategory] [smallint] NULL,
	[CFTransactionGroup] [smallint] NULL,
	[CFDataType] [int] NULL,
	[SubGroupID] [int] NULL,
	[DisplayCaption] [varchar](1024) NULL,
	[Remarks] [varchar](1024) NULL,
	[DBUserID] [int] NULL,
	[DBServerDateTime] [datetime] NULL,
 CONSTRAINT [PK_CashFlowDmSetup] PRIMARY KEY CLUSTERED 
(
	[CashFlowDmSetupID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  View [dbo].[View_CashFlowDmSetup]    Script Date: 3/2/2017 1:53:03 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE VIEW [dbo].[View_CashFlowDmSetup]
AS
SELECT	CashFlowDmSetup.CashFlowDmSetupID,  
		CashFlowDmSetup.CFTransactionCategory,  
		CashFlowDmSetup.CFTransactionGroup,  
		CashFlowDmSetup.CFDataType,  
		CashFlowDmSetup.SubGroupID, 
        ChartsOfAccount.AccountCode AS SubGroupCode,  
		ChartsOfAccount.AccountHeadName AS SubGroupName,  					
		CashFlowDmSetup.DisplayCaption,  
		CashFlowDmSetup.Remarks,
		0.00 AS Amount

FROM    CashFlowDmSetup 
LEFT OUTER JOIN ChartsOfAccount ON  CashFlowDmSetup.SubGroupID =  ChartsOfAccount.AccountHeadID




GO
/****** Object:  View [dbo].[View_CashFlowSetup]    Script Date: 3/2/2017 1:53:03 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


CREATE VIEW [dbo].[View_CashFlowSetup]
AS
SELECT			    CashFlowSetup.CashFlowSetupID,  
					CashFlowSetup.CFTransactionCategory,  
					CashFlowSetup.CFTransactionGroup,  
					CashFlowSetup.CFDataType,  
					CashFlowSetup.SubGroupID, 
                    ChartsOfAccount.AccountCode AS SubGroupCode,  
					ChartsOfAccount.AccountHeadName AS SubGroupName,  					
					CashFlowSetup.DisplayCaption,  
					CashFlowSetup.Remarks,
					0.00 AS Amount

FROM             CashFlowSetup 
LEFT OUTER JOIN ChartsOfAccount ON  CashFlowSetup.SubGroupID =  ChartsOfAccount.AccountHeadID



GO
/****** Object:  View [dbo].[View_VoucherDetailReverse]    Script Date: 3/2/2017 1:53:03 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE VIEW [dbo].[View_VoucherDetailReverse]
AS
SELECT	VD.VoucherDetailID,
		VD.AccountHeadID,
		VD.IsDebit,
		VD.Amount,
		RH.ReverseHeadID,
		RH.ReverseHeadCode,
		RH.ReverseHeadName,
		RH.ReverseAmount,		
		Voucher.VoucherDate,
		Voucher.AuthorizedBy

FROM			VoucherDetail AS VD
CROSS APPLY		[dbo].[FN_GetsReverseAccounts] (VD.VoucherDetailID) AS RH
LEFT OUTER JOIN Voucher ON VD.VoucherID =Voucher.VoucherID


GO
