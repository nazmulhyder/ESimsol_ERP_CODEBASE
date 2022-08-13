IF NOT EXISTS (SELECT * FROM sys.columns where Name = N'HasOrderReference' and Object_ID = Object_ID(N'DebitCreditSetup'))
BEGIN
   ALTER TABLE DebitCreditSetup
   ADD HasOrderReference bit
END
GO
IF NOT EXISTS (SELECT * FROM sys.columns where Name = N'OrderRefType' and Object_ID = Object_ID(N'DebitCreditSetup'))
BEGIN
   ALTER TABLE DebitCreditSetup
   ADD OrderRefType smallint
END
GO
IF NOT EXISTS (SELECT * FROM sys.columns where Name = N'OrderNoSetup' and Object_ID = Object_ID(N'DebitCreditSetup'))
BEGIN
   ALTER TABLE DebitCreditSetup
   ADD OrderNoSetup Varchar(1024)
END
GO
IF NOT EXISTS (SELECT * FROM sys.columns where Name = N'OrderDateColumn' and Object_ID = Object_ID(N'DebitCreditSetup'))
BEGIN
   ALTER TABLE DebitCreditSetup
   ADD OrderDateColumn Varchar(1024)
END
GO
IF NOT EXISTS (SELECT * FROM sys.columns where Name = N'OrderNoColumn' and Object_ID = Object_ID(N'DebitCreditSetup'))
BEGIN
   ALTER TABLE DebitCreditSetup
   ADD OrderNoColumn Varchar(1024)
END
GO
IF NOT EXISTS (SELECT * FROM sys.columns where Name = N'OrderRefColumn' and Object_ID = Object_ID(N'DebitCreditSetup'))
BEGIN
   ALTER TABLE DebitCreditSetup
   ADD OrderRefColumn Varchar(1024)
END
GO
/****** Object:  View [dbo].[View_Subledger]    Script Date: 2/7/2017 9:18:26 PM ******/
DROP VIEW [dbo].[View_Subledger]
GO
/****** Object:  View [dbo].[View_DeliveryChallan]    Script Date: 2/7/2017 9:18:26 PM ******/

GO
/****** Object:  View [dbo].[View_DebitCreditSetup]    Script Date: 2/7/2017 9:18:26 PM ******/
DROP VIEW [dbo].[View_DebitCreditSetup]
GO
/****** Object:  View [dbo].[View_VOReference]    Script Date: 2/7/2017 9:18:26 PM ******/
DROP VIEW [dbo].[View_VOReference]
GO
/****** Object:  View [dbo].[View_VOrder]    Script Date: 2/7/2017 9:18:26 PM ******/
DROP VIEW [dbo].[View_VOrder]
GO
/****** Object:  View [dbo].[View_DeliveryChallanDetail]    Script Date: 2/7/2017 9:18:26 PM ******/

GO
/****** Object:  View [dbo].[View_OrderSheetDetail]    Script Date: 2/7/2017 9:18:26 PM ******/
DROP VIEW [dbo].[View_OrderSheetDetail]
GO
/****** Object:  View [dbo].[View_ExportSCDetail]    Script Date: 2/7/2017 9:18:26 PM ******/

GO
/****** Object:  Table [dbo].[VOReference]    Script Date: 2/7/2017 9:18:26 PM ******/
DROP TABLE [dbo].[VOReference]
GO
/****** Object:  Table [dbo].[VOrder]    Script Date: 2/7/2017 9:18:26 PM ******/
DROP TABLE [dbo].[VOrder]
GO
/****** Object:  StoredProcedure [dbo].[SP_IUD_VOReference]    Script Date: 2/7/2017 9:18:26 PM ******/
DROP PROCEDURE [dbo].[SP_IUD_VOReference]
GO
/****** Object:  StoredProcedure [dbo].[SP_IUD_VOrder]    Script Date: 2/7/2017 9:18:26 PM ******/
DROP PROCEDURE [dbo].[SP_IUD_VOrder]
GO
/****** Object:  StoredProcedure [dbo].[SP_IUD_DebitCreditSetup]    Script Date: 2/7/2017 9:18:26 PM ******/
DROP PROCEDURE [dbo].[SP_IUD_DebitCreditSetup]
GO
/****** Object:  StoredProcedure [dbo].[SP_IUD_CashFlowSetup]    Script Date: 2/7/2017 9:18:26 PM ******/
DROP PROCEDURE [dbo].[SP_IUD_CashFlowSetup]
GO
/****** Object:  StoredProcedure [dbo].[SP_CashFlowStatement]    Script Date: 2/7/2017 9:18:26 PM ******/
DROP PROCEDURE [dbo].[SP_CashFlowStatement]
GO
/****** Object:  StoredProcedure [dbo].[SP_CashFlowStatement]    Script Date: 2/7/2017 9:18:26 PM ******/
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
/****** Object:  StoredProcedure [dbo].[SP_IUD_CashFlowSetup]    Script Date: 2/7/2017 9:18:26 PM ******/
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
/****** Object:  StoredProcedure [dbo].[SP_IUD_DebitCreditSetup]    Script Date: 2/7/2017 9:18:26 PM ******/
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
	@OrderRefType as smallint,
	@OrderNoSetup as varchar(1024),
	@OrderRefColumn as Varchar(1024),
	@OrderNoColumn as varchar(1024),
	@OrderDateColumn as varchar(1024),
	@HasOrderReference as bit, 
	@DBUserID  as int,
	@DBOperation as smallint,
	@DebitCreditSetupIDs as varchar(512)
	--%n, %n, %s, %s, %n, %n, %s, %s, %n, %s, %s, %s, %s, %b, %s, %s, %n, %s, %s, %s, %s, %b, %b, %b, %s, %s, %s, %s, %s, %s, %b, %s, %s, %s, %s, %s, %s, %s, %b, %s, %s, %s, %s, %s, %s, %s, %s, %s, %b, %s, %s, %s, %n, %s, %s, %n, %s, %s, %s, %b, %s, %s, %s, %s, %s, %s, %n, %s, %s, %s, %s, %b, %n, %n, %s
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
	INSERT INTO DebitCreditSetup	(DebitCreditSetupID,	IntegrationSetupDetailID,	DataCollectionQuery,	CompareColumn, 		AccountHeadType,	FixedAccountHeadID,		AccountHeadSetup,	AccountNameSetup,	ReferenceType,	CurrencySetup,	ConversionRateSetup,	AmountSetup,	NarrationSetup,		IsChequeReferenceCreate,	ChequeReferenceDataSQL,		ChequeReferenceCompareColumns,	ChequeType,		ChequeSetup,	ChequeReferenceAmountSetup,		ChequeReferenceDescriptionSetup,	ChequeReferenceDateSetup,	 IsDebit,	IsCostCenterCreate,	HasBillReference,	HasChequeReference,		HasOrderReference,	CostcenterDataSQL,	CostCenterCompareColumns,	CostcenterSetup,	CostCenterAmountSetup,	CostCenterDescriptionSetup,		CostCenterDateSetup,	IsVoucherBill,		VoucherBillDataSQL,			VoucherBillCompareColumns,			VoucherBillTrType,	VoucherBillSetup,		VoucherBillAmountSetup,			VoucherBillDescriptionSetup,		VoucherBillDateSetup,	IsOrderReferenceApply,		OrderReferenceDataSQL,		OrderReferenceCompareColumns,	OrderReferenceSetup,	OrderAmountSetup,	OrderRemarkSetup,	OrderDateSetup,		OrderRefType,	OrderNoSetup,	OrderRefColumn,		OrderNoColumn,		OrderDateColumn,		IsInventoryEffect,		InventoryDataSQL,	InventoryCompareColumns,	InventoryWorkingUnitSetup,		InventoryProductSetup,		InventoryQtySetup,		InventoryUnitSetup,		InventoryUnitPriceSetup,	InventoryDescriptionSetup,			InventoryDateSetup,			Note,	CostCenterCategorySetup,		CostCenterNoColumn,		CostCenterRefObjType,		CostCenterRefObjColumn,		VoucherBillNoColumn,		VoucherBillRefObjType,		VoucherBillRefObjColumn,	BillDateSetup,		BillDueDateSetup,	DBUserID,	DBServerDateTime)		
						VALUES		(@DebitCreditSetupID,	@IntegrationSetupDetailID,	@DataCollectionQuery,	@CompareColumn,		@AccountHeadType,	@FixedAccountHeadID,	@AccountHeadSetup,	@AccountNameSetup,	@ReferenceType,	@CurrencySetup,	@ConversionRateSetup,	@AmountSetup,	@NarrationSetup,	@IsChequeReferenceCreate,	@ChequeReferenceDataSQL,	@ChequeReferenceCompareColumns,	@ChequeType,	@ChequeSetup,	@ChequeReferenceAmountSetup,	@ChequeReferenceDescriptionSetup,	@ChequeReferenceDateSetup, @IsDebit,	@IsCostCenterCrate,	@HasBillReference,	@HasChequeReference,	@HasOrderReference,	@CostcenterDataSQL,	@CostCenterCompareColumns,	@CostcenterSetup,	@CostCenterAmountSetup,	@CostCenterDescriptionSetup,	@CostCenterDateSetup,	@IsVoucherBill,		@VoucherBillDataSQL,		@VoucherBillCompareColumns,			@VoucherBillTrType, @VoucherBillSetup,		@VoucherBillAmountSetup,		@VoucherBillDescriptionSetup,		@VoucherBillDateSetup,	@IsOrderReferenceApply,		@OrderReferenceDataSQL,		@OrderReferenceCompareColumns,	@OrderReferenceSetup,	@OrderAmountSetup,	@OrderRemarkSetup,	@OrderDateSetup,	@OrderRefType,	@OrderNoSetup,	@OrderRefColumn,	@OrderNoColumn,		@OrderDateColumn,		@IsInventoryEffect,		@InventoryDataSQL,	@InventoryCompareColumns,	@InventoryWorkingUnitSetup,		@InventoryProductSetup,		@InventoryQtySetup,		@InventoryUnitSetup,	@InventoryUnitPriceSetup,	@InventoryDescriptionSetup ,		@InventoryDateSetup ,		@Note,	@CostCenterCategorySetUp,		@CostCenterNoColumn,	@CostCenterRefObjType,		@CostCenterRefObjColumn,	@VoucherBillNoColumn,		@VoucherBillRefObjType,		@VoucherBillRefObjColumn,	@BillDateSetup,		@BillDueDateSetup,	@DBUserID,	@PV_DBServerDateTime)
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
	UPDATE DebitCreditSetup SET	IntegrationSetupDetailID=@IntegrationSetupDetailID,	DataCollectionQuery=@DataCollectionQuery, CompareColumn=@CompareColumn,  AccountHeadType=@AccountHeadType,	FixedAccountHeadID=@FixedAccountHeadID,		AccountHeadSetup=@AccountHeadSetup,	AccountNameSetup=@AccountNameSetup,	ReferenceType=@ReferenceType,	CurrencySetup=@CurrencySetup, ConversionRateSetup=@ConversionRateSetup, AmountSetup=@AmountSetup,	NarrationSetup=@NarrationSetup, IsChequeReferenceCreate=@IsChequeReferenceCreate,	ChequeReferenceDataSQL=@ChequeReferenceDataSQL,	ChequeReferenceCompareColumns=@ChequeReferenceCompareColumns, ChequeType=@ChequeType, ChequeSetup=@ChequeSetup,	ChequeReferenceAmountSetup=@ChequeReferenceAmountSetup,	ChequeReferenceDescriptionSetup=@ChequeReferenceDescriptionSetup,	ChequeReferenceDateSetup=@ChequeReferenceDateSetup,	IsDebit=@IsDebit,	IsCostCenterCreate=@IsCostCenterCrate, HasBillReference=@HasBillReference, HasChequeReference=@HasChequeReference, HasOrderReference=@HasOrderReference,	CostcenterDataSQL=@CostcenterDataSQL,	CostCenterCompareColumns=@CostCenterCompareColumns,	CostcenterSetup=@CostcenterSetup,	CostCenterAmountSetup=@CostCenterAmountSetup,	CostCenterDescriptionSetup=@CostCenterDescriptionSetup,		CostCenterDateSetup=@CostCenterDateSetup,	IsVoucherBill = @IsVoucherBill,		VoucherBillDataSQL = @VoucherBillDataSQL,			VoucherBillCompareColumns = @VoucherBillCompareColumns,	 VoucherBillTrType=@VoucherBillTrType,			VoucherBillSetup = @VoucherBillSetup,			VoucherBillAmountSetup =@VoucherBillAmountSetup,			VoucherBillDescriptionSetup = @VoucherBillDescriptionSetup,			VoucherBillDateSetup =@VoucherBillDateSetup ,	 IsOrderReferenceApply=@IsOrderReferenceApply,		OrderReferenceDataSQL=@OrderReferenceDataSQL,		OrderReferenceCompareColumns=@OrderReferenceCompareColumns,	OrderReferenceSetup=@OrderReferenceSetup,  OrderNoSetup=@OrderNoSetup,	OrderAmountSetup=@OrderAmountSetup,	OrderRemarkSetup=@OrderRemarkSetup,	OrderDateSetup=@OrderDateSetup,	OrderRefType=@OrderRefType,		OrderRefColumn=@OrderRefColumn,		OrderNoColumn=@OrderNoColumn,		OrderDateColumn=@OrderDateColumn,	IsInventoryEffect = @IsInventoryEffect,			InventoryDataSQL = @InventoryDataSQL,		InventoryCompareColumns = @InventoryCompareColumns,	 InventoryWorkingUnitSetup=@InventoryWorkingUnitSetup, InventoryProductSetup = @InventoryProductSetup,			InventoryQtySetup = @InventoryQtySetup, InventoryUnitSetup=@InventoryUnitSetup,  InventoryUnitPriceSetup=@InventoryUnitPriceSetup,			InventoryDescriptionSetup = @InventoryDescriptionSetup,			InventoryDateSetup =@InventoryDateSetup ,	Note=@Note,	 CostCenterCategorySetup=@CostCenterCategorySetUp,		CostCenterNoColumn=@CostCenterNoColumn,		CostCenterRefObjType=@CostCenterRefObjType,		CostCenterRefObjColumn=@CostCenterRefObjColumn,		VoucherBillNoColumn=@VoucherBillNoColumn,		VoucherBillRefObjType=@VoucherBillRefObjType,		VoucherBillRefObjColumn=@VoucherBillRefObjColumn, BillDateSetup=@BillDateSetup, BillDueDateSetup=@BillDueDateSetup,	DBUserID =@DBUserID,	DBServerDateTime =@PV_DBServerDateTime WHERE DebitCreditSetupID = @DebitCreditSetupID
	SELECT * FROM View_DebitCreditSetup  WHERE DebitCreditSetupID=@DebitCreditSetupID
END

IF(@DBOperation=3)
BEGIN
	DELETE FROM DataCollectionSetup WHERE DataReferenceType=2 AND DataReferenceID IN (SELECT  DrCr.DebitCreditSetupID FROM DebitCreditSetup AS DrCr WHERE DrCr.IntegrationSetupDetailID=@IntegrationSetupDetailID AND DrCr.DebitCreditSetupID NOT IN (SELECT * FROM dbo.SplitInToDataSet(@DebitCreditSetupIDs,',')))
	DELETE FROM DebitCreditSetup WHERE IntegrationSetupDetailID=@IntegrationSetupDetailID AND DebitCreditSetupID NOT IN (SELECT * FROM dbo.SplitInToDataSet(@DebitCreditSetupIDs,','))
END
COMMIT TRAN










GO
/****** Object:  StoredProcedure [dbo].[SP_IUD_VOrder]    Script Date: 2/7/2017 9:18:26 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[SP_IUD_VOrder]
(
	@VOrderID	as int,
	@BUID	as int,
	@RefNo	as varchar(512),
	@VOrderRefType	as smallint,
	@VOrderRefID	as int,
	@OrderNo	as varchar(512),
	@OrderDate	as datetime,
	@SubledgerID	as int,
	@Remarks	as varchar(512),
	@DBOperation as smallint,
	@DBUserID as int
	--%n, %n, %s, %n, %n, %s, %d, %n, %s, %n, %n
)		
AS
BEGIN TRAN	
DECLARE 
@TempNo as int,
@YearPart as Varchar(512),
@DBServerDateTime as datetime
SET @DBServerDateTime=Getdate()

IF(@DBOperation=1)
BEGIN	
	SET @YearPart=SUBSTRING((SELECT DATENAME(YEAR,@OrderDate)),3,2)
	IF EXISTS (SELECT * FROM VOrder AS HH WHERE YEAR(HH.OrderDate)=YEAR(@OrderDate))
	BEGIN
		SET @TempNo = ISNULL((SELECT MAX(CONVERT(int,dbo.SplitedStringGet(HH.RefNo, '/',0))) FROM VOrder AS HH WHERE YEAR(HH.OrderDate)=YEAR(@OrderDate)),0)+1
		SET @RefNo = RIGHT('00000' + CONVERT(VARCHAR(5),@TempNo), 5)+'/'+@YearPart
	END
	ELSE 
	BEGIN
		SET @RefNo = '00001/'+@YearPart
	END

	IF(@VOrderRefType!=1)
	BEGIN
		IF EXISTS((SELECT * FROM VOrder AS HH WHERE HH.VOrderRefType=@VOrderRefType AND HH.VOrderRefID=@VOrderRefID))
		BEGIN
			ROLLBACK
				RAISERROR (N' Sorry, Your Entered VOrder Ref already exists!!',16,1);
			RETURN
		END
	END
	
	SET @VOrderID=(SELECT ISNULL(MAX(VOrderID),0)+1 FROM VOrder)		
	INSERT INTO VOrder(VOrderID,	BUID,	RefNo,	VOrderRefType,	VOrderRefID,	OrderNo,	OrderDate,	SubledgerID,	Remarks,	DBUserID,	DBServerDateTime,	LastUpdateBy,	LastUpdateDateTime)
				VALUES(@VOrderID,	@BUID,	@RefNo,	@VOrderRefType,	@VOrderRefID,	@OrderNo,	@OrderDate,	@SubledgerID,	@Remarks,	@DBUserID,	@DBServerDateTime,	@DBUserID,		@DBServerDateTime)    				  
    SELECT * FROM View_VOrder WHERE VOrderID=@VOrderID
END

IF(@DBOperation=2)
BEGIN
	IF(@VOrderID<=0)
	BEGIN
		ROLLBACK
			RAISERROR (N'Your Selected Order Is Invalid Please Refresh and try again!!',16,1);	
		RETURN
	END
	IF(@VOrderRefType!=1)
	BEGIN
		IF EXISTS((SELECT * FROM VOrder AS HH WHERE HH.VOrderRefType=@VOrderRefType AND HH.VOrderRefID=@VOrderRefID AND HH.VOrderID!=@VOrderID))
		BEGIN
			ROLLBACK
				RAISERROR (N' Sorry, Your Entered VOrder Ref already exists!!',16,1);
			RETURN
		END
	END
	UPDATE VOrder SET BUID=BUID,	RefNo=RefNo,	VOrderRefType=VOrderRefType,	VOrderRefID=VOrderRefID,	OrderNo=OrderNo,	OrderDate=OrderDate,	SubledgerID=SubledgerID,	Remarks=Remarks,	LastUpdateBy = @DBUserID,		 LastUpdateDateTime = @DBServerDateTime   WHERE VOrderID=@VOrderID
 	SELECT * FROM View_VOrder WHERE VOrderID=@VOrderID
END

IF(@DBOperation=3)
BEGIN
	IF(@VOrderID<=0)
	BEGIN
		ROLLBACK
			RAISERROR (N'Your Selected Order  Is Invalid Please Refresh and try again!!',16,1);	
		RETURN
	END	
	IF EXISTS (SELECT * FROM VOReference AS HH WHERE HH.OrderID= @VOrderID)
	BEGIN
		ROLLBACK
			RAISERROR (N'Deletion Not possible voucher reference may loss!!',16,1);	
		RETURN
	END		
	DELETE FROM VOrder WHERE VOrderID=@VOrderID
END
COMMIT TRAN



GO
/****** Object:  StoredProcedure [dbo].[SP_IUD_VOReference]    Script Date: 2/7/2017 9:18:26 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE  PROCEDURE   [dbo].[SP_IUD_VOReference]
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








GO
/****** Object:  Table [dbo].[VOrder]    Script Date: 2/7/2017 9:18:26 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[VOrder](
	[VOrderID] [int] NOT NULL,
	[BUID] [int] NOT NULL,
	[RefNo] [varchar](512) NULL,
	[VOrderRefType] [smallint] NULL,
	[VOrderRefID] [int] NULL,
	[OrderNo] [varchar](512) NULL,
	[OrderDate] [datetime] NULL,
	[SubledgerID] [int] NULL,
	[Remarks] [varchar](512) NULL,
	[DBUserID] [int] NULL,
	[DBServerDateTime] [datetime] NULL,
	[LastUpdateBy] [int] NULL,
	[LastUpdateDateTime] [datetime] NULL,
 CONSTRAINT [PK_VOrder] PRIMARY KEY CLUSTERED 
(
	[VOrderID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[VOReference]    Script Date: 2/7/2017 9:18:26 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[VOReference](
	[VOReferenceID] [int] NOT NULL,
	[VoucherDetailID] [int] NULL,
	[OrderID] [int] NULL,
	[TransactionDate] [datetime] NULL,
	[Remarks] [varchar](1024) NULL,
	[IsDebit] [bit] NULL,
	[CurrencyID] [int] NULL,
	[ConversionRate] [decimal](30, 17) NULL,
	[AmountInCurrency] [decimal](30, 17) NULL,
	[Amount] [decimal](30, 17) NULL,
	[DBUserID] [int] NULL,
	[DBServerDateTime] [datetime] NULL,
	[CCTID] [int] NULL,
 CONSTRAINT [PK_VOReference] PRIMARY KEY CLUSTERED 
(
	[VOReferenceID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  View [dbo].[View_ExportSCDetail]    Script Date: 2/7/2017 9:18:26 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

GO
/****** Object:  View [dbo].[View_OrderSheetDetail]    Script Date: 2/7/2017 9:18:26 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE VIEW [dbo].[View_OrderSheetDetail]
AS
SELECT		[OrderSheetDetail].*
			,Product.ProductCode
			,Product.ProductName
			,Product.IsApplySizer
			,CapitalResource.Name AS ModelReferenceName
			,(SELECT ColorName FROM ColorCategory WHERE ColorCategoryID = OrderSheetDetail.ColorID) As ColorName
			,Product.AddTwo As SizeName
			,MeasurementUnit.UnitName
			,MeasurementUnit.Symbol AS UnitSymbol,
			(SELECT HH.RateUnit FROM OrderSheet AS HH WHERE HH.OrderSheetID=OrderSheetDetail.OrderSheetID) AS RateUnit,
			(OrderSheetDetail.Qty / (SELECT ISNULL(RateUnit,1) FROM OrderSheet WHERE OrderSheetID = OrderSheetDetail.OrderSheetID) * OrderSheetDetail.UnitPrice) As Amount,
			(OrderSheetDetail.Qty - ISNULL((SELECT SUM(ISNULL(Qty,0)) FROM ExportPIDetail WHERE OrderSheetDetailID = OrderSheetDetail.OrderSheetDetailID),0))AS YetToPIQty,
			0 AS OrderSheetDetailLogID,
			0 AS OrderSheetLogID


FROM		[OrderSheetDetail] 
 LEFT   JOIN  View_Product AS Product ON  [OrderSheetDetail].ProductID =  Product.ProductID 
 LEFT	JOIN  CapitalResource ON  [OrderSheetDetail].ModelReferenceID = CapitalResource.CRID 
 LEFT	JOIN  MeasurementUnit ON [OrderSheetDetail].UnitID =  MeasurementUnit.MeasurementUnitID

















GO
/****** Object:  View [dbo].[View_DeliveryChallanDetail]    Script Date: 2/7/2017 9:18:26 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

GO
/****** Object:  View [dbo].[View_VOrder]    Script Date: 2/7/2017 9:18:26 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE VIEW [dbo].[View_VOrder]
AS
SELECT	VOrder.VOrderID,
		VOrder.BUID,
		VOrder.RefNo,
		VOrder.VOrderRefType,
		VOrder.VOrderRefID,
		VOrder.OrderNo,
		VOrder.OrderDate,
		VOrder.SubledgerID,
		VOrder.Remarks,
		ACCostCenter.Name AS SubledgerName,  		
		BU.Name AS BUName,
		BU.Code AS BUCode

FROM    VOrder 
LEFT OUTER JOIN ACCostCenter ON  VOrder.SubledgerID =  ACCostCenter.ACCostCenterID
LEFT OUTER JOIN	BusinessUnit AS BU ON VOrder.BUID = BU.BusinessUnitID








GO
/****** Object:  View [dbo].[View_VOReference]    Script Date: 2/7/2017 9:18:26 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE VIEW [dbo].[View_VOReference]
AS
SELECT	VOReference.VOReferenceID, 
		VOReference.VoucherDetailID, 
		VOReference.OrderID, 
		VOReference.TransactionDate,
		VOReference.Remarks, 
		VOReference.IsDebit,
		VOReference.CurrencyID, 
		VOReference.ConversionRate, 
		VOReference.AmountInCurrency, 
		VOReference.Amount, 
		VOReference.CCTID, 
		VO.RefNo,
		VO.OrderNo,
		VO.SubledgerID,
		VO.SubledgerName,
		VD.BUID,
		VD.AuthorizedBy as ApprovedBy,
		VD.AccountHeadID,
		VD.VoucherID

FROM		VOReference 
LEFT OUTER JOIN View_VOrder AS VO ON VOReference.OrderID = VO.VOrderID
LEFT OUTER JOIN View_VoucherDetail AS VD ON VOReference.VoucherDetailID=VD.VoucherDetailID










GO
/****** Object:  View [dbo].[View_DebitCreditSetup]    Script Date: 2/7/2017 9:18:26 PM ******/
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
			DebitCreditSetup.OrderRefType,
			DebitCreditSetup.OrderNoSetup,
			DebitCreditSetup.OrderRefColumn,
			DebitCreditSetup.OrderNoColumn,
			DebitCreditSetup.OrderDateColumn,
			DebitCreditSetup.HasOrderReference,
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
/****** Object:  View [dbo].[View_DeliveryChallan]    Script Date: 2/7/2017 9:18:26 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON


GO
/****** Object:  View [dbo].[View_Subledger]    Script Date: 2/7/2017 9:18:26 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE View [dbo].[View_Subledger]
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
		ISNULL((SELECT SUM(HH.DueCheque) FROM View_VoucherBill AS HH WHERE HH.SubLedgerID=ACCostCenter.ACCostCenterID),0) AS DueAmount,
		ISNULL((SELECT MAX(HH.OverDueDays) FROM View_VoucherBill AS HH WHERE HH.SubLedgerID=ACCostCenter.ACCostCenterID),0) AS OverDueDays,		
		ACCostCenter.Name+' ['+ACCostCenter.Code+']' AS NameCode
		                   
FROM	ACCostCenter



















GO
