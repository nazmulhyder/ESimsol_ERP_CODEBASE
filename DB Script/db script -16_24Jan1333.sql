GO
/****** Object:  View [dbo].[View_HeadDisplayConfigure]    Script Date: 1/24/2017 1:20:53 PM ******/
DROP VIEW [dbo].[View_HeadDisplayConfigure]
GO
/****** Object:  Table [dbo].[HeadDisplayConfigure]    Script Date: 1/24/2017 1:20:53 PM ******/
DROP TABLE [dbo].[HeadDisplayConfigure]
GO
/****** Object:  UserDefinedFunction [dbo].[GetLedgerBalance]    Script Date: 1/24/2017 1:20:53 PM ******/
DROP FUNCTION [dbo].[GetLedgerBalance]
GO
/****** Object:  UserDefinedFunction [dbo].[FN_GetsRevarseHead]    Script Date: 1/24/2017 1:20:53 PM ******/
DROP FUNCTION [dbo].[FN_GetsRevarseHead]
GO
/****** Object:  StoredProcedure [dbo].[SP_IUD_HeadDisplayConfigure]    Script Date: 1/24/2017 1:20:53 PM ******/
DROP PROCEDURE [dbo].[SP_IUD_HeadDisplayConfigure]
GO
/****** Object:  StoredProcedure [dbo].[SP_GeneralLedger]    Script Date: 1/24/2017 1:20:53 PM ******/
DROP PROCEDURE [dbo].[SP_GeneralLedger]
GO
/****** Object:  StoredProcedure [dbo].[SP_CostCenterLedger]    Script Date: 1/24/2017 1:20:53 PM ******/
DROP PROCEDURE [dbo].[SP_CostCenterLedger]
GO
/****** Object:  StoredProcedure [dbo].[SP_BankReconciliationPrepare]    Script Date: 1/24/2017 1:20:53 PM ******/
DROP PROCEDURE [dbo].[SP_BankReconciliationPrepare]
GO
/****** Object:  StoredProcedure [dbo].[SP_BankReconciliationPrepare]    Script Date: 1/24/2017 1:20:53 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[SP_BankReconciliationPrepare]
(
	@BusinessUnitID as int,
	@CostCenterID as int,
	@CurrencyID as int,
	@StartDate date,
	@EndDate date,
	@IsApproved as bit
	--%n, %n, %n, %d, %d, %b
)
AS
BEGIN TRAN
--DECLARE
--@BusinessUnitID as int,
--@CostCenterID as int,
--@CurrencyID as int,
--@StartDate date,
--@EndDate date,
--@IsApproved as bit

--SET @BusinessUnitID =1
--SET @CostCenterID=6
--SET @CurrencyID =0
--SET @StartDate ='01 JAN 2017'
--SET @EndDate ='16 Jan 2017'
--SET @IsApproved=0

CREATE TABLE #TempTable(
							ID int IDENTITY(1,1) PRIMARY KEY,
							CCTID INT,
							CCID int,							
							DebitOpeiningValue decimal(30,17),
							CreditOpeiningValue decimal(30,17),
							OpeiningValue decimal(30,17),
							IsDebit bit,
							DebitAmount decimal(30,17),
							CreditAmount decimal(30,17),
							ClosingValue decimal(30,17),
							IsDrClosing bit,
							CurrencySymbol Varchar(512),
							VoucherID int,
							VoucherDate date,
							VoucherNo Varchar(512),
							AccountHeadID INT,
							ComponentID int,
							AccountHeadName Varchar(512),
							ReverseHead Varchar(max),
							VoucherDetailID int
						)

CREATE TABLE #ResultTable(
							ID int IDENTITY(1,1) PRIMARY KEY,
							CCTID INT,
							CCID int,							
							DebitOpeiningValue decimal(30,17),
							CreditOpeiningValue decimal(30,17),
							OpeiningValue decimal(30,17),
							IsDebit bit,
							DebitAmount decimal(30,17),
							CreditAmount decimal(30,17),
							ClosingValue decimal(30,17),
							IsDrClosing bit,
							CurrencySymbol Varchar(512),
							VoucherID int,
							VoucherDate date,
							VoucherNo Varchar(512),
							AccountHeadID INT,
							ComponentID int,
							AccountHeadName Varchar(512),
							ReverseHead Varchar(max),
							VoucherDetailID int
						)
DECLARE
@SessionID as int,
@SessionStartDate as date,
@BaseCurrencyID as int,
@ComponentID as int,
--@IsDebitOpen as bit,
@DebitOpeningValue as decimal(30,17),
@CreditOpeningValue as decimal(30,17),
@DebitAmount as decimal(30,17),
@CreditAmount as decimal(30,17),
@ClosingValue as decimal(30,17)

IF EXISTS(SELECT SessionID FROM AccountingSession  WHERE CONVERT(DATE,CONVERT(VARCHAR(12),StartDate,106))=CONVERT(DATE,CONVERT(VARCHAR(12),@StartDate,106)) AND SessionType=6)
BEGIN
	SET @SessionID=(SELECT SessionID FROM AccountingSession  WHERE CONVERT(DATE,CONVERT(VARCHAR(12),StartDate,106))=CONVERT(DATE,CONVERT(VARCHAR(12),@StartDate,106)) AND SessionType=6)
END
ELSE
BEGIN
	SET @SessionID=(SELECT TOP 1 SessionID FROM AccountingSession  WHERE CONVERT(DATE,CONVERT(VARCHAR(12),StartDate,106))>=CONVERT(DATE,CONVERT(VARCHAR(12),@StartDate,106)) AND SessionType=6)
	SET @StartDate=(SELECT MIN(StartDate) FROM AccountingSession  WHERE CONVERT(DATE,CONVERT(VARCHAR(12),StartDate,106))>=CONVERT(DATE,CONVERT(VARCHAR(12),@StartDate,106)) AND SessionType=6)
END
SET @SessionStartDate=(SELECT StartDate FROM AccountingSession WHERE AccountingSessionID=@SessionID)
SET @BaseCurrencyID = (SELECT C.BaseCurrencyID FROM Company AS C WHERE C.CompanyID=1)

IF(@CurrencyID<=0)
BEGIN
	SET @CurrencyID = (SELECT C.BaseCurrencyID FROM Company AS C WHERE C.CompanyID=1)
END
PRINT @CurrencyID
PRINT @IsApproved

IF(@IsApproved=1)
BEGIN
	IF(@CurrencyID=@BaseCurrencyID)
	BEGIN
		INSERT INTO #TempTable(	CCID,		AccountHeadID,		ComponentID)
			SELECT DISTINCT		AOB.BreakdownObjID,	AOB.AccountHeadID,	dbo.GetComponentID(AOB.AccountHeadID) FROM View_AccountOpenningBreakdown AS AOB WHERE AOB.BusinessUnitID=@BusinessUnitID AND AOB.AccountingSessionID=@SessionID AND AOB.BreakdownType=1 AND AOB.BreakdownObjID=@CostCenterID

		INSERT INTO #TempTable(	CCID,		AccountHeadID,		ComponentID)
			SELECT DISTINCT		CCT.CCID,	CCT.AccountHeadID,	dbo.GetComponentID(CCT.AccountHeadID) FROM View_CostCenterTransaction AS CCT WHERE ISNULL(CCT.ApprovedBy,0)!=0 AND CCT.BUID=@BusinessUnitID AND CCT.CCID=@CostCenterID AND CONVERT(DATE, CONVERT(VARCHAR(12),CCT.TransactionDate,106)) BETWEEN  CONVERT(DATE, CONVERT(VARCHAR(12),@SessionStartDate,106)) AND CONVERT(DATE, CONVERT(VARCHAR(12),@EndDate,106))
	END
	ELSE
	BEGIN
		INSERT INTO #TempTable(	CCID,		AccountHeadID,		ComponentID)
			SELECT DISTINCT		AOB.BreakdownObjID,	AOB.AccountHeadID,	dbo.GetComponentID(AOB.AccountHeadID) FROM View_AccountOpenningBreakdown AS AOB WHERE AOB.CurrencyID=@CurrencyID AND AOB.BusinessUnitID=@BusinessUnitID AND AOB.AccountingSessionID=@SessionID AND AOB.BreakdownType=1 AND AOB.BreakdownObjID=@CostCenterID

		INSERT INTO #TempTable(	CCID,		AccountHeadID,		ComponentID)
			SELECT DISTINCT		CCT.CCID,	CCT.AccountHeadID,	dbo.GetComponentID(CCT.AccountHeadID) FROM View_CostCenterTransaction AS CCT WHERE ISNULL(CCT.ApprovedBy,0)!=0 AND CCT.BUID=@BusinessUnitID AND CCT.CurrencyID=@CurrencyID AND CCT.CCID=@CostCenterID AND CONVERT(DATE, CONVERT(VARCHAR(12),CCT.TransactionDate,106)) BETWEEN  CONVERT(DATE, CONVERT(VARCHAR(12),@SessionStartDate,106)) AND CONVERT(DATE, CONVERT(VARCHAR(12),@EndDate,106))
	END
END
ELSE
BEGIN
	IF(@CurrencyID=@BaseCurrencyID)
	BEGIN
		INSERT INTO #TempTable(	CCID,		AccountHeadID,		ComponentID)
			SELECT DISTINCT		AOB.BreakdownObjID,	AOB.AccountHeadID,	dbo.GetComponentID(AOB.AccountHeadID) FROM View_AccountOpenningBreakdown AS AOB WHERE AOB.BusinessUnitID=@BusinessUnitID AND AOB.AccountingSessionID=@SessionID AND AOB.BreakdownType=1 AND AOB.BreakdownObjID=@CostCenterID

		INSERT INTO #TempTable(	CCID,		AccountHeadID,		ComponentID)
			SELECT DISTINCT		CCT.CCID,	CCT.AccountHeadID,	dbo.GetComponentID(CCT.AccountHeadID) FROM View_CostCenterTransaction AS CCT WHERE CCT.BUID=@BusinessUnitID AND CCT.CCID=@CostCenterID AND CONVERT(DATE, CONVERT(VARCHAR(12),CCT.TransactionDate,106)) BETWEEN  CONVERT(DATE, CONVERT(VARCHAR(12),@SessionStartDate,106)) AND CONVERT(DATE, CONVERT(VARCHAR(12),@EndDate,106))
	END
	ELSE
	BEGIN
		INSERT INTO #TempTable(	CCID,		AccountHeadID,		ComponentID)
			SELECT DISTINCT		AOB.BreakdownObjID,	AOB.AccountHeadID,	dbo.GetComponentID(AOB.AccountHeadID) FROM View_AccountOpenningBreakdown AS AOB WHERE AOB.CurrencyID=@CurrencyID AND AOB.BusinessUnitID=@BusinessUnitID AND AOB.AccountingSessionID=@SessionID AND AOB.BreakdownType=1 AND AOB.BreakdownObjID=@CostCenterID

		INSERT INTO #TempTable(	CCID,		AccountHeadID,		ComponentID)
			SELECT DISTINCT		CCT.CCID,	CCT.AccountHeadID,	dbo.GetComponentID(CCT.AccountHeadID) FROM View_CostCenterTransaction AS CCT WHERE CCT.BUID=@BusinessUnitID AND CCT.CurrencyID=@CurrencyID AND CCT.CCID=@CostCenterID AND CONVERT(DATE, CONVERT(VARCHAR(12),CCT.TransactionDate,106)) BETWEEN  CONVERT(DATE, CONVERT(VARCHAR(12),@SessionStartDate,106)) AND CONVERT(DATE, CONVERT(VARCHAR(12),@EndDate,106))
	END
END

DELETE FROM #TempTable WHERE ID NOT IN (SELECT MIN(ID) FROM #TempTable GROUP BY CCID,AccountHeadID)

--SELECT * FROM #TempTable
IF(@IsApproved=1)
BEGIN
	IF(@CurrencyID=@BaseCurrencyID)
	BEGIN
		UPDATE #TempTable
		SET DebitOpeiningValue=ISNULL((SELECT SUM(AOB.OpenningBalance) FROM AccountOpenningBreakdown AS AOB WHERE AOB.BusinessUnitID=@BusinessUnitID AND AOB.IsDr=1 AND AOB.AccountHeadID=TT.AccountHeadID AND AOB.AccountingSessionID=@SessionID AND AOB.BreakdownType=1 AND AOB.BreakdownObjID=TT.CCID),0),
			CreditOpeiningValue=ISNULL((SELECT SUM(AOB.OpenningBalance) FROM AccountOpenningBreakdown AS AOB WHERE AOB.BusinessUnitID=@BusinessUnitID AND AOB.IsDr=0 AND AOB.AccountHeadID=TT.AccountHeadID AND AOB.AccountingSessionID=@SessionID AND AOB.BreakdownType=1 AND AOB.BreakdownObjID=TT.CCID),0),
			DebitAmount=ISNULL((SELECT SUM(CCT.Amount*CCT.CurrencyConversionRate) FROM View_CostCenterTransaction AS CCT WHERE CCT.BUID=@BusinessUnitID AND ISNULL(CCT.ApprovedBy,0)!=0 AND  CCT.IsDr=1 AND CCT.CCID=TT.CCID AND CCT.AccountHeadID=TT.AccountHeadID AND CONVERT(DATE, CONVERT(VARCHAR(12),CCT.TransactionDate,106)) BETWEEN  CONVERT(DATE, CONVERT(VARCHAR(12),@SessionStartDate,106)) AND CONVERT(DATE, CONVERT(VARCHAR(12),DATEADD(DAY,-1,@StartDate),106))),0),
			CreditAmount=ISNULL((SELECT SUM(CCT.Amount*CCT.CurrencyConversionRate) FROM View_CostCenterTransaction AS CCT WHERE CCT.BUID=@BusinessUnitID AND ISNULL(CCT.ApprovedBy,0)!=0 AND CCT.IsDr=0 AND CCT.CCID=TT.CCID AND CCT.AccountHeadID=TT.AccountHeadID AND CONVERT(DATE, CONVERT(VARCHAR(12),CCT.TransactionDate,106)) BETWEEN  CONVERT(DATE, CONVERT(VARCHAR(12),@SessionStartDate,106)) AND CONVERT(DATE, CONVERT(VARCHAR(12),DATEADD(DAY,-1,@StartDate),106))),0)
		FROM #TempTable AS TT
	END
	ELSE
	BEGIN
		UPDATE #TempTable
		SET DebitOpeiningValue=ISNULL((SELECT SUM(AOB.AmountInCurrency) FROM AccountOpenningBreakdown AS AOB WHERE AOB.BusinessUnitID=@BusinessUnitID AND AOB.IsDr=1 AND AOB.CurrencyID=@CurrencyID AND AOB.AccountHeadID=TT.AccountHeadID AND AOB.AccountingSessionID=@SessionID AND AOB.BreakdownType=1 AND AOB.BreakdownObjID=TT.CCID),0),
			CreditOpeiningValue=ISNULL((SELECT SUM(AOB.AmountInCurrency) FROM AccountOpenningBreakdown AS AOB WHERE AOB.BusinessUnitID=@BusinessUnitID AND AOB.IsDr=0 AND AOB.CurrencyID=@CurrencyID AND AOB.AccountHeadID=TT.AccountHeadID AND AOB.AccountingSessionID=@SessionID AND AOB.BreakdownType=1 AND AOB.BreakdownObjID=TT.CCID),0),
			DebitAmount=ISNULL((SELECT SUM(CCT.Amount*CCT.CurrencyConversionRate) FROM View_CostCenterTransaction AS CCT WHERE  CCT.BUID=@BusinessUnitID AND ISNULL(CCT.ApprovedBy,0)!=0 AND CCT.CurrencyID=@CurrencyID AND CCT.IsDr=1 AND CCT.CCID=TT.CCID AND CCT.AccountHeadID=TT.AccountHeadID AND CONVERT(DATE, CONVERT(VARCHAR(12),CCT.TransactionDate,106)) BETWEEN  CONVERT(DATE, CONVERT(VARCHAR(12),@SessionStartDate,106)) AND CONVERT(DATE, CONVERT(VARCHAR(12),DATEADD(DAY,-1,@StartDate),106))),0),
			CreditAmount=ISNULL((SELECT SUM(CCT.Amount*CCT.CurrencyConversionRate) FROM View_CostCenterTransaction AS CCT WHERE  CCT.BUID=@BusinessUnitID AND ISNULL(CCT.ApprovedBy,0)!=0 AND CCT.CurrencyID=@CurrencyID AND CCT.IsDr=0 AND CCT.CCID=TT.CCID AND CCT.AccountHeadID=TT.AccountHeadID AND CONVERT(DATE, CONVERT(VARCHAR(12),CCT.TransactionDate,106)) BETWEEN  CONVERT(DATE, CONVERT(VARCHAR(12),@SessionStartDate,106)) AND CONVERT(DATE, CONVERT(VARCHAR(12),DATEADD(DAY,-1,@StartDate),106))),0)
		FROM #TempTable AS TT
	END
END
ELSE
BEGIN
	IF(@CurrencyID=@BaseCurrencyID)
	BEGIN
		UPDATE #TempTable
		SET DebitOpeiningValue=ISNULL((SELECT SUM(AOB.OpenningBalance) FROM AccountOpenningBreakdown AS AOB WHERE AOB.BusinessUnitID=@BusinessUnitID AND AOB.IsDr=1 AND AOB.AccountHeadID=TT.AccountHeadID AND AOB.AccountingSessionID=@SessionID AND AOB.BreakdownType=1 AND AOB.BreakdownObjID=TT.CCID),0),
		CreditOpeiningValue=ISNULL((SELECT SUM(AOB.OpenningBalance) FROM AccountOpenningBreakdown AS AOB WHERE AOB.BusinessUnitID=@BusinessUnitID AND AOB.IsDr=0 AND AOB.AccountHeadID=TT.AccountHeadID AND AOB.AccountingSessionID=@SessionID AND AOB.BreakdownType=1 AND AOB.BreakdownObjID=TT.CCID),0),
		DebitAmount=ISNULL((SELECT SUM(CCT.Amount*CCT.CurrencyConversionRate) FROM View_CostCenterTransaction AS CCT WHERE CCT.BUID=@BusinessUnitID AND CCT.IsDr=1 AND CCT.CCID=TT.CCID AND CCT.AccountHeadID=TT.AccountHeadID AND CONVERT(DATE, CONVERT(VARCHAR(12),CCT.TransactionDate,106)) BETWEEN  CONVERT(DATE, CONVERT(VARCHAR(12),@SessionStartDate,106)) AND CONVERT(DATE, CONVERT(VARCHAR(12),DATEADD(DAY,-1,@StartDate),106))),0),
		CreditAmount=ISNULL((SELECT SUM(CCT.Amount*CCT.CurrencyConversionRate) FROM View_CostCenterTransaction AS CCT WHERE CCT.BUID=@BusinessUnitID AND CCT.IsDr=0 AND CCT.CCID=TT.CCID AND CCT.AccountHeadID=TT.AccountHeadID AND CONVERT(DATE, CONVERT(VARCHAR(12),CCT.TransactionDate,106)) BETWEEN  CONVERT(DATE, CONVERT(VARCHAR(12),@SessionStartDate,106)) AND CONVERT(DATE, CONVERT(VARCHAR(12),DATEADD(DAY,-1,@StartDate),106))),0)
		FROM #TempTable AS TT
	END
	ELSE
	BEGIN
		UPDATE #TempTable
		SET DebitOpeiningValue=ISNULL((SELECT SUM(AOB.AmountInCurrency) FROM AccountOpenningBreakdown AS AOB WHERE AOB.BusinessUnitID=@BusinessUnitID AND AOB.IsDr=1 AND AOB.CurrencyID=@CurrencyID AND AOB.AccountHeadID=TT.AccountHeadID AND AOB.AccountingSessionID=@SessionID AND AOB.BreakdownType=1 AND AOB.BreakdownObjID=TT.CCID),0),
		CreditOpeiningValue=ISNULL((SELECT SUM(AOB.AmountInCurrency) FROM AccountOpenningBreakdown AS AOB WHERE AOB.BusinessUnitID=@BusinessUnitID AND AOB.IsDr=0 AND AOB.CurrencyID=@CurrencyID AND AOB.AccountHeadID=TT.AccountHeadID AND AOB.AccountingSessionID=@SessionID AND AOB.BreakdownType=1 AND AOB.BreakdownObjID=TT.CCID),0),
		DebitAmount=ISNULL((SELECT SUM(CCT.Amount*CCT.CurrencyConversionRate) FROM View_CostCenterTransaction AS CCT WHERE CCT.BUID=@BusinessUnitID AND CCT.CurrencyID=@CurrencyID AND CCT.IsDr=1 AND CCT.CCID=TT.CCID AND CCT.AccountHeadID=TT.AccountHeadID AND CONVERT(DATE, CONVERT(VARCHAR(12),CCT.TransactionDate,106)) BETWEEN  CONVERT(DATE, CONVERT(VARCHAR(12),@SessionStartDate,106)) AND CONVERT(DATE, CONVERT(VARCHAR(12),DATEADD(DAY,-1,@StartDate),106))),0),
		CreditAmount=ISNULL((SELECT SUM(CCT.Amount*CCT.CurrencyConversionRate) FROM View_CostCenterTransaction AS CCT WHERE CCT.BUID=@BusinessUnitID AND CCT.CurrencyID=@CurrencyID AND CCT.IsDr=0 AND CCT.CCID=TT.CCID AND CCT.AccountHeadID=TT.AccountHeadID AND CONVERT(DATE, CONVERT(VARCHAR(12),CCT.TransactionDate,106)) BETWEEN  CONVERT(DATE, CONVERT(VARCHAR(12),@SessionStartDate,106)) AND CONVERT(DATE, CONVERT(VARCHAR(12),DATEADD(DAY,-1,@StartDate),106))),0)
		FROM #TempTable AS TT
	END
END


IF NOT EXISTS(SELECT * FROM #TempTable)
BEGIN
	INSERT INTO #TempTable (CCTID,	CCID,	OpeiningValue,	IsDebit,	DebitAmount,	CreditAmount,	ClosingValue,	IsDrClosing,	CurrencySymbol, VoucherID,	VoucherDate,	VoucherNo,	AccountHeadName)
					VALUES (0,		0,		0.00,			0,			0.00,			0.00,			0.00,			0,				'',				0,			@StartDate,		'',			'Opening Balance')
END
ELSE
BEGIN
	UPDATE #TempTable
	SET VoucherID=0,
		VoucherNo='',
		VoucherDate=@StartDate,
		AccountHeadName='Opening Balance'		
	FROM #TempTable AS TT

	UPDATE #TempTable 
	SET ClosingValue=TT.DebitOpeiningValue-TT.CreditOpeiningValue+TT.DebitAmount-TT.CreditAmount 
	FROM #TempTable AS TT WHERE ISNULL(TT.VoucherDetailID,0)=0 AND TT.ComponentID IN (2,6)

	UPDATE #TempTable 
	SET ClosingValue=TT.CreditOpeiningValue-TT.DebitOpeiningValue-TT.DebitAmount+TT.CreditAmount 
	FROM #TempTable AS TT WHERE ISNULL(TT.VoucherDetailID,0)=0 AND TT.ComponentID NOT IN (2,6)
END

IF(@IsApproved=1)
BEGIN
	IF(@CurrencyID=@BaseCurrencyID)
	BEGIN
		INSERT INTO #TempTable(CCTID,		CCID,		AccountHeadID,		ComponentID,							VoucherDetailID)
						SELECT CCT.CCTID,	CCT.CCID,	CCT.AccountHeadID,	dbo.GetComponentID(CCT.AccountHeadID),	CCT.VoucherDetailID FROM View_CostCenterTransaction AS CCT WHERE ISNULL(CCT.ApprovedBy,0)!=0 AND CCT.BUID=@BusinessUnitID AND CCT.CCID=@CostCenterID AND CONVERT(DATE, CONVERT(VARCHAR(12),CCT.TransactionDate,106)) BETWEEN  CONVERT(DATE, CONVERT(VARCHAR(12),@StartDate,106)) AND CONVERT(DATE, CONVERT(VARCHAR(12),@EndDate,106)) ORDER BY CCT.AccountHeadID
	END
	ELSE
	BEGIN
		INSERT INTO #TempTable(CCTID,		CCID,		AccountHeadID,		ComponentID,							VoucherDetailID)
						SELECT CCT.CCTID,	CCT.CCID,	CCT.AccountHeadID,	dbo.GetComponentID(CCT.AccountHeadID),	CCT.VoucherDetailID FROM View_CostCenterTransaction AS CCT WHERE ISNULL(CCT.ApprovedBy,0)!=0 AND CCT.BUID=@BusinessUnitID AND CCT.CurrencyID=@CurrencyID AND CCT.CCID=@CostCenterID AND CONVERT(DATE, CONVERT(VARCHAR(12),CCT.TransactionDate,106)) BETWEEN  CONVERT(DATE, CONVERT(VARCHAR(12),@StartDate,106)) AND CONVERT(DATE, CONVERT(VARCHAR(12),@EndDate,106)) ORDER BY CCT.AccountHeadID
	END
END
ELSE
BEGIN
	IF(@CurrencyID=@BaseCurrencyID)
	BEGIN
		INSERT INTO #TempTable(CCTID,		CCID,		AccountHeadID,		ComponentID,							VoucherDetailID)
						SELECT CCT.CCTID,	CCT.CCID,	CCT.AccountHeadID,	dbo.GetComponentID(CCT.AccountHeadID),	CCT.VoucherDetailID FROM View_CostCenterTransaction AS CCT WHERE CCT.BUID=@BusinessUnitID AND CCT.CCID=@CostCenterID AND CONVERT(DATE, CONVERT(VARCHAR(12),CCT.TransactionDate,106)) BETWEEN  CONVERT(DATE, CONVERT(VARCHAR(12),@StartDate,106)) AND CONVERT(DATE, CONVERT(VARCHAR(12),@EndDate,106)) ORDER BY CCT.AccountHeadID
	END
	ELSE
	BEGIN
		INSERT INTO #TempTable(CCTID,		CCID,		AccountHeadID,		ComponentID,							VoucherDetailID)
						SELECT CCT.CCTID,	CCT.CCID,	CCT.AccountHeadID,	dbo.GetComponentID(CCT.AccountHeadID),	CCT.VoucherDetailID FROM View_CostCenterTransaction AS CCT WHERE CCT.BUID=@BusinessUnitID AND CCT.CurrencyID=@CurrencyID AND CCT.CCID=@CostCenterID AND CONVERT(DATE, CONVERT(VARCHAR(12),CCT.TransactionDate,106)) BETWEEN  CONVERT(DATE, CONVERT(VARCHAR(12),@StartDate,106)) AND CONVERT(DATE, CONVERT(VARCHAR(12),@EndDate,106)) ORDER BY CCT.AccountHeadID
	END
END


--SELECT * FROM #TempTable
UPDATE #TempTable
SET VoucherID=(SELECT VD.VoucherID FROM View_VoucherDetail AS VD WHERE VD.VoucherDetailID=TT.VoucherDetailID),
	IsDebit=(SELECT CCT.IsDr FROM View_CostCenterTransaction AS CCT WHERE CCT.CCTID=TT.CCTID)	
FROM #TempTable AS TT WHERE ISNULL(TT.VoucherDetailID,0)!=0

IF(@CurrencyID=@BaseCurrencyID)
BEGIN
	UPDATE #TempTable
	SET DebitAmount=ISNULL((SELECT (CCT.Amount*CCT.CurrencyConversionRate) FROM CostCenterTransaction AS CCT WHERE CCT.CCTID=ISNULL(TT.CCTID,0)),0),
		CreditAmount=0.00
	FROM #TempTable AS TT WHERE TT.IsDebit=1 AND ISNULL(TT.VoucherDetailID,0)!=0

	UPDATE #TempTable
	SET DebitAmount=0.00,
		CreditAmount=(SELECT (CCT.Amount*CCT.CurrencyConversionRate) FROM CostCenterTransaction AS CCT WHERE CCT.CCTID=ISNULL(TT.CCTID,0))
	FROM #TempTable AS TT WHERE TT.IsDebit=0 AND ISNULL(TT.VoucherDetailID,0)!=0
END
ELSE
BEGIN
	UPDATE #TempTable
	SET DebitAmount=ISNULL((SELECT CCT.Amount FROM CostCenterTransaction AS CCT WHERE CCT.CCTID=TT.CCTID),0),
		CreditAmount=0.00
	FROM #TempTable AS TT WHERE TT.IsDebit=1 AND ISNULL(TT.VoucherDetailID,0)!=0

	UPDATE #TempTable
	SET DebitAmount=0.00,
		CreditAmount=(SELECT CCT.Amount FROM CostCenterTransaction AS CCT WHERE CCT.CCTID=TT.CCTID)
	FROM #TempTable AS TT WHERE TT.IsDebit=0 AND ISNULL(TT.VoucherDetailID,0)!=0
END

--SELECT * FROM #TempTable

UPDATE #TempTable
SET ReverseHead= dbo.FN_GetsRevarseHead(TT.VoucherID, TT.IsDebit), --ISNULL((SELECT TOP(1)AccountHeadName FROM View_VoucherDetail WHERE  IsDebit!=TT.IsDebit AND  VoucherID=TT.VoucherID),''),
	AccountHeadName=ISNULL((SELECT COA.AccountHeadName FROM View_ChartsOfAccount AS COA WHERE  COA.AccountHeadID=TT.AccountHeadID),''),	
	VoucherDate=(SELECT V.VoucherDate FROM Voucher AS V WHERE V.VoucherID=TT.VoucherID),
	VoucherNo=(SELECT V.VoucherNo FROM Voucher AS V WHERE V.VoucherID=TT.VoucherID)
FROM #TempTable AS TT WHERE ISNULL(TT.VoucherDetailID,0)!=0


INSERT INTO #ResultTable(CCTID,		CCID,		DebitOpeiningValue,		CreditOpeiningValue,	OpeiningValue,		IsDebit,		DebitAmount,	CreditAmount,		ClosingValue,		IsDrClosing,		CurrencySymbol,		VoucherID,		VoucherDate,		VoucherNo,		AccountHeadName,	AccountHeadID,	ComponentID,		ReverseHead,		VoucherDetailID)
				  SELECT TT.CCTID,	TT.CCID,	TT.DebitOpeiningValue,	TT.CreditOpeiningValue,	TT.OpeiningValue,	TT.IsDebit,		TT.DebitAmount,	TT.CreditAmount,	TT.ClosingValue,	TT.IsDrClosing,		TT.CurrencySymbol,	TT.VoucherID,	TT.VoucherDate,		TT.VoucherNo,	TT.AccountHeadName,	TT.AccountHeadID,TT.ComponentID,	TT.ReverseHead,		TT.VoucherDetailID FROM #TempTable  AS TT ORDER BY TT.VoucherDate, TT.VoucherID ASC

DECLARE
@ID as int,
@Count as int,
@IsDebit as bit,
@Amount as decimal(30,17),
@CurrentBalance as decimal(30,17)

SET @ID=1--2
SET @Count=(SELECT COUNT(*) FROM #ResultTable)
--SET @CurrentBalance =@ClosingValue

WHILE(@Count>=@ID)
BEGIN
	IF(ISNULL((SELECT RT.VoucherDetailID FROM #ResultTable AS RT WHERE RT.ID=@ID),0)>0)
	BEGIN			
		IF(ISNULL((SELECT RT.ComponentID FROM #ResultTable AS RT WHERE RT.ID=@ID),0) IN (2,6))
		BEGIN
			SET @Amount=ISNULL((SELECT TT.DebitAmount-TT.CreditAmount FROM #ResultTable AS TT WHERE TT.ID=@ID),0)
		END
		ELSE
		BEGIN
			SET @Amount=ISNULL((SELECT TT.CreditAmount-TT.DebitAmount FROM #ResultTable AS TT WHERE TT.ID=@ID),0)
		END
		SET @CurrentBalance=@CurrentBalance+@Amount			
		UPDATE #ResultTable SET ClosingValue=@CurrentBalance WHERE ID=@ID
	END
	ELSE
	BEGIN
		SET @CurrentBalance=ISNULL((SELECT RT.ClosingValue FROM #ResultTable AS RT WHERE RT.ID=@ID),0)
	END
	SET @ID=@ID+1
END


CREATE TABLE #ReconcileTable(
								BankReconciliationID int,
								SubledgerID int,
								VoucherDetailID int,
								AccountHeadID int, 
								ReconcilHeadID int, 
								ReconcilDate date,
								ReconcilRemarks Varchar(1024),
								IsDebit bit,
								Amount decimal(30,17),
								ReconcilStatus smallint,
								ReconcilBy  int,
								SubledgerCode varchar(512), 
								SubledgerName varchar(512),
								AccountCode varchar(512),
								AccountHeadName varchar(512),
								RCAHCode varchar(512), 
								RCAHName varchar(512), 
								ReverseHead Varchar(max),
								ReconcilByName varchar(512),
								VoucherID int,
								VoucherDate date,
								VoucherNo varchar(512),
								DebitAmount decimal(30,17),
								CreditAmount decimal(30,17),
								CurrentBalance decimal(30,17)
							)
INSERT INTO #ReconcileTable (	BankReconciliationID,	SubledgerID,	VoucherDetailID,		AccountHeadID,		ReconcilHeadID,		ReconcilDate,		ReconcilRemarks,	IsDebit,	Amount,		ReconcilStatus,	ReconcilBy,	AccountCode,	AccountHeadName,		RCAHCode,	RCAHName,	ReverseHead,		ReconcilByName,	VoucherID,		VoucherDate,		VoucherNo,		DebitAmount,	CreditAmount,		CurrentBalance)
					SELECT		0,						@CostCenterID,	RT.VoucherDetailID,		RT.AccountHeadID,	0,					RT.VoucherDate,		'N/A',				RT.IsDebit,	0,			0,				0,			'',				RT.AccountHeadName,		'',			'',			RT.ReverseHead,		'',				RT.VoucherID,	RT.VoucherDate,		RT.VoucherNo,	RT.DebitAmount,	RT.CreditAmount,	RT.ClosingValue FROM #ResultTable AS RT ORDER BY RT.VoucherDate, RT.VoucherID ASC

UPDATE #ReconcileTable
SET BankReconciliationID = ISNULL((SELECT MM.BankReconciliationID FROM BankReconciliation AS MM WHERE MM.VoucherDetailID=HH.VoucherDetailID),0)	
FROM #ReconcileTable AS HH

UPDATE #ReconcileTable SET Amount = HH.DebitAmount FROM #ReconcileTable AS HH WHERE HH.IsDebit=1
UPDATE #ReconcileTable SET Amount = HH.CreditAmount	 FROM #ReconcileTable AS HH WHERE HH.IsDebit=0


UPDATE #ReconcileTable
SET SubledgerCode = ISNULL((SELECT MM.Code FROM ACCostCenter AS MM WHERE MM.ACCostCenterID=@CostCenterID),0),
	SubledgerName = ISNULL((SELECT MM.Name FROM ACCostCenter AS MM WHERE MM.ACCostCenterID=@CostCenterID),0),
	ReconcilHeadID = ISNULL((SELECT MM.ReconcilHeadID FROM BankReconciliation AS MM WHERE MM.BankReconciliationID=HH.BankReconciliationID),0),
	RCAHCode = ISNULL((SELECT MM.RCAHCode FROM View_BankReconciliation AS MM WHERE MM.BankReconciliationID=HH.BankReconciliationID),''),
	RCAHName = ISNULL((SELECT MM.RCAHName FROM View_BankReconciliation AS MM WHERE MM.BankReconciliationID=HH.BankReconciliationID),''),
	ReconcilDate = ISNULL((SELECT MM.ReconcilDate FROM BankReconciliation AS MM WHERE MM.BankReconciliationID=HH.BankReconciliationID),HH.VoucherDate),
	ReconcilRemarks = ISNULL((SELECT MM.ReconcilRemarks FROM BankReconciliation AS MM WHERE MM.BankReconciliationID=HH.BankReconciliationID),'N/A'),
	ReconcilStatus = ISNULL((SELECT MM.ReconcilStatus FROM BankReconciliation AS MM WHERE MM.BankReconciliationID=HH.BankReconciliationID),0),
	ReconcilBy = ISNULL((SELECT MM.ReconcilBy FROM BankReconciliation AS MM WHERE MM.BankReconciliationID=HH.BankReconciliationID),0),
	ReconcilByName = ISNULL((SELECT MM.ReconcilByName FROM View_BankReconciliation AS MM WHERE MM.BankReconciliationID=HH.BankReconciliationID),'')
FROM #ReconcileTable AS HH WHERE HH.BankReconciliationID>0


SELECT * FROM #ReconcileTable AS HH ORDER BY HH.VoucherDate, HH.VoucherID
DROP TABLE #TempTable
DROP TABLE #ResultTable
DROP TABLE #ReconcileTable
COMMIT TRAN



GO
/****** Object:  StoredProcedure [dbo].[SP_CostCenterLedger]    Script Date: 1/24/2017 1:20:53 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[SP_CostCenterLedger]
(
	@BusinessUnitID as int,
	@AccountHeadID as int,
	@CostCenterID as int,
	@CurrencyID as int,
	@StartDate date,
	@EndDate date,
	@IsApproved as bit
)
AS
BEGIN TRAN
--DECLARE
--@BusinessUnitID as int,
--@AccountHeadID as int,
--@CostCenterID as int,
--@CurrencyID as int,
--@StartDate date,
--@EndDate date,
--@IsApproved as bit

--SET @BusinessUnitID =0
--SET @AccountHeadID =0
--SET @CostCenterID=300
--SET @CurrencyID =0
--SET @StartDate ='01 JAN 2016'
--SET @EndDate ='08 FEB 2016'
--SET @IsApproved=0

CREATE TABLE #TempTable(
							ID int IDENTITY(1,1) PRIMARY KEY,
							CCTID INT,
							CCID int,
							CCCode Varchar(512),
							CCName Varchar(512),
							DebitOpeiningValue decimal(30,17),
							CreditOpeiningValue decimal(30,17),
							OpeiningValue decimal(30,17),
							IsDebit bit,
							DebitAmount decimal(30,17),
							CreditAmount decimal(30,17),
							ClosingValue decimal(30,17),
							IsDrClosing bit,
							CurrencySymbol Varchar(512),
							VoucherID int,
							VoucherDate date,
							VoucherNo Varchar(512),
							ParentHeadID INT,
							ComponentID int,
							AccountHeadName Varchar(512),
							ParentHeadName varchar(512),
							ParentHeadCode varchar(512),
							VoucherDetailID int,
							Narration VARCHAR(512),
							VoucherNarration VARCHAR(512),
							Description VARCHAR(512)
						)

CREATE TABLE #ResultTable(
							ID int IDENTITY(1,1) PRIMARY KEY,
							CCTID INT,
							CCID int,
							CCCode Varchar(512),
							CCName Varchar(512),
							DebitOpeiningValue decimal(30,17),
							CreditOpeiningValue decimal(30,17),
							OpeiningValue decimal(30,17),
							IsDebit bit,
							DebitAmount decimal(30,17),
							CreditAmount decimal(30,17),
							ClosingValue decimal(30,17),
							IsDrClosing bit,
							CurrencySymbol Varchar(512),
							VoucherID int,
							VoucherDate date,
							VoucherNo Varchar(512),
							ParentHeadID INT,
							ComponentID int,
							AccountHeadName Varchar(512),
							ParentHeadName varchar(512),
							ParentHeadCode varchar(512),
							VoucherDetailID int,
							Narration VARCHAR(512),
							VoucherNarration VARCHAR(512),
							Description VARCHAR(512)
						)
DECLARE
@SessionID as int,
@SessionStartDate as date,
@BaseCurrencyID as int,
@ComponentID as int,
--@IsDebitOpen as bit,
@DebitOpeningValue as decimal(30,17),
@CreditOpeningValue as decimal(30,17),
@DebitAmount as decimal(30,17),
@CreditAmount as decimal(30,17),
@ClosingValue as decimal(30,17)

IF EXISTS(SELECT SessionID FROM AccountingSession  WHERE CONVERT(DATE,CONVERT(VARCHAR(12),StartDate,106))=CONVERT(DATE,CONVERT(VARCHAR(12),@StartDate,106)) AND SessionType=6)
BEGIN
	SET @SessionID=(SELECT SessionID FROM AccountingSession  WHERE CONVERT(DATE,CONVERT(VARCHAR(12),StartDate,106))=CONVERT(DATE,CONVERT(VARCHAR(12),@StartDate,106)) AND SessionType=6)
END
ELSE
BEGIN
	SET @SessionID=(SELECT TOP 1 SessionID FROM AccountingSession  WHERE CONVERT(DATE,CONVERT(VARCHAR(12),StartDate,106))>=CONVERT(DATE,CONVERT(VARCHAR(12),@StartDate,106)) AND SessionType=6)
	SET @StartDate=(SELECT MIN(StartDate) FROM AccountingSession  WHERE CONVERT(DATE,CONVERT(VARCHAR(12),StartDate,106))>=CONVERT(DATE,CONVERT(VARCHAR(12),@StartDate,106)) AND SessionType=6)
END
SET @SessionStartDate=(SELECT StartDate FROM AccountingSession WHERE AccountingSessionID=@SessionID)
SET @BaseCurrencyID = (SELECT C.BaseCurrencyID FROM Company AS C WHERE C.CompanyID=1)
SET @ComponentID =dbo.GetComponentID(@AccountHeadID)

IF(@CurrencyID<=0)
BEGIN
	SET @CurrencyID = (SELECT C.BaseCurrencyID FROM Company AS C WHERE C.CompanyID=1)
END
PRINT @CurrencyID
PRINT @IsApproved

IF(@CostCenterID>0)
BEGIN
	IF(@BusinessUnitID>0)
	BEGIN
		IF(@AccountHeadID>0)
		BEGIN
			IF(@IsApproved=1)
			BEGIN
				IF(@CurrencyID=@BaseCurrencyID)
				BEGIN
					INSERT INTO #TempTable(	CCID,		ParentHeadID,		ComponentID)
						SELECT DISTINCT		AOB.BreakdownObjID,	AOB.AccountHeadID,	dbo.GetComponentID(AOB.AccountHeadID) FROM View_AccountOpenningBreakdown AS AOB WHERE AOB.BusinessUnitID=@BusinessUnitID AND AOB.AccountingSessionID=@SessionID AND AOB.BreakdownType=1 AND AOB.AccountHeadID=@AccountHeadID AND AOB.BreakdownObjID=@CostCenterID

					INSERT INTO #TempTable(	CCID,		ParentHeadID,		ComponentID)
						SELECT DISTINCT		CCT.CCID,	CCT.AccountHeadID,	dbo.GetComponentID(CCT.AccountHeadID) FROM View_CostCenterTransaction AS CCT WHERE ISNULL(CCT.ApprovedBy,0)!=0 AND CCT.BUID=@BusinessUnitID AND CCT.CCID=@CostCenterID AND CCT.AccountHeadID=@AccountHeadID AND CONVERT(DATE, CONVERT(VARCHAR(12),CCT.TransactionDate,106)) BETWEEN  CONVERT(DATE, CONVERT(VARCHAR(12),@SessionStartDate,106)) AND CONVERT(DATE, CONVERT(VARCHAR(12),@EndDate,106))
				END
				ELSE
				BEGIN
					INSERT INTO #TempTable(	CCID,		ParentHeadID,		ComponentID)
						SELECT DISTINCT		AOB.BreakdownObjID,	AOB.AccountHeadID,	dbo.GetComponentID(AOB.AccountHeadID) FROM View_AccountOpenningBreakdown AS AOB WHERE AOB.CurrencyID=@CurrencyID AND AOB.BusinessUnitID=@BusinessUnitID AND AOB.AccountingSessionID=@SessionID AND AOB.BreakdownType=1 AND AOB.AccountHeadID=@AccountHeadID AND AOB.BreakdownObjID=@CostCenterID

					INSERT INTO #TempTable(	CCID,		ParentHeadID,		ComponentID)
						SELECT DISTINCT		CCT.CCID,	CCT.AccountHeadID,	dbo.GetComponentID(CCT.AccountHeadID) FROM View_CostCenterTransaction AS CCT WHERE ISNULL(CCT.ApprovedBy,0)!=0 AND CCT.BUID=@BusinessUnitID AND CCT.CurrencyID=@CurrencyID AND CCT.CCID=@CostCenterID AND CCT.AccountHeadID=@AccountHeadID AND CONVERT(DATE, CONVERT(VARCHAR(12),CCT.TransactionDate,106)) BETWEEN  CONVERT(DATE, CONVERT(VARCHAR(12),@SessionStartDate,106)) AND CONVERT(DATE, CONVERT(VARCHAR(12),@EndDate,106))
				END
			END
			ELSE
			BEGIN
				IF(@CurrencyID=@BaseCurrencyID)
				BEGIN
					INSERT INTO #TempTable(	CCID,		ParentHeadID,		ComponentID)
						SELECT DISTINCT		AOB.BreakdownObjID,	AOB.AccountHeadID,	dbo.GetComponentID(AOB.AccountHeadID) FROM View_AccountOpenningBreakdown AS AOB WHERE AOB.BusinessUnitID=@BusinessUnitID AND AOB.AccountingSessionID=@SessionID AND AOB.BreakdownType=1 AND AOB.AccountHeadID=@AccountHeadID AND AOB.BreakdownObjID=@CostCenterID

					INSERT INTO #TempTable(	CCID,		ParentHeadID,		ComponentID)
						SELECT DISTINCT		CCT.CCID,	CCT.AccountHeadID,	dbo.GetComponentID(CCT.AccountHeadID) FROM View_CostCenterTransaction AS CCT WHERE CCT.BUID=@BusinessUnitID AND CCT.CCID=@CostCenterID AND CCT.AccountHeadID=@AccountHeadID AND CONVERT(DATE, CONVERT(VARCHAR(12),CCT.TransactionDate,106)) BETWEEN  CONVERT(DATE, CONVERT(VARCHAR(12),@SessionStartDate,106)) AND CONVERT(DATE, CONVERT(VARCHAR(12),@EndDate,106))
				END
				ELSE
				BEGIN
					INSERT INTO #TempTable(	CCID,		ParentHeadID,		ComponentID)
						SELECT DISTINCT		AOB.BreakdownObjID,	AOB.AccountHeadID,	dbo.GetComponentID(AOB.AccountHeadID) FROM View_AccountOpenningBreakdown AS AOB WHERE AOB.CurrencyID=@CurrencyID AND AOB.BusinessUnitID=@BusinessUnitID AND AOB.AccountingSessionID=@SessionID AND AOB.BreakdownType=1 AND AOB.AccountHeadID=@AccountHeadID AND AOB.BreakdownObjID=@CostCenterID

					INSERT INTO #TempTable(	CCID,		ParentHeadID,		ComponentID)
						SELECT DISTINCT		CCT.CCID,	CCT.AccountHeadID,	dbo.GetComponentID(CCT.AccountHeadID) FROM View_CostCenterTransaction AS CCT WHERE CCT.BUID=@BusinessUnitID AND CCT.CurrencyID=@CurrencyID AND CCT.CCID=@CostCenterID AND CCT.AccountHeadID=@AccountHeadID AND CONVERT(DATE, CONVERT(VARCHAR(12),CCT.TransactionDate,106)) BETWEEN  CONVERT(DATE, CONVERT(VARCHAR(12),@SessionStartDate,106)) AND CONVERT(DATE, CONVERT(VARCHAR(12),@EndDate,106))
				END
			END
		END
		ELSE
		BEGIN
			IF(@IsApproved=1)
			BEGIN
				IF(@CurrencyID=@BaseCurrencyID)
				BEGIN
					INSERT INTO #TempTable(	CCID,		ParentHeadID,		ComponentID)
						SELECT DISTINCT		AOB.BreakdownObjID,	AOB.AccountHeadID,	dbo.GetComponentID(AOB.AccountHeadID) FROM View_AccountOpenningBreakdown AS AOB WHERE AOB.BusinessUnitID=@BusinessUnitID AND AOB.AccountingSessionID=@SessionID AND AOB.BreakdownType=1 AND AOB.BreakdownObjID=@CostCenterID

					INSERT INTO #TempTable(	CCID,		ParentHeadID,		ComponentID)
						SELECT DISTINCT		CCT.CCID,	CCT.AccountHeadID,	dbo.GetComponentID(CCT.AccountHeadID) FROM View_CostCenterTransaction AS CCT WHERE ISNULL(CCT.ApprovedBy,0)!=0 AND CCT.BUID=@BusinessUnitID AND CCT.CCID=@CostCenterID AND CONVERT(DATE, CONVERT(VARCHAR(12),CCT.TransactionDate,106)) BETWEEN  CONVERT(DATE, CONVERT(VARCHAR(12),@SessionStartDate,106)) AND CONVERT(DATE, CONVERT(VARCHAR(12),@EndDate,106))
				END
				ELSE
				BEGIN
					INSERT INTO #TempTable(	CCID,		ParentHeadID,		ComponentID)
						SELECT DISTINCT		AOB.BreakdownObjID,	AOB.AccountHeadID,	dbo.GetComponentID(AOB.AccountHeadID) FROM View_AccountOpenningBreakdown AS AOB WHERE AOB.CurrencyID=@CurrencyID AND AOB.BusinessUnitID=@BusinessUnitID AND AOB.AccountingSessionID=@SessionID AND AOB.BreakdownType=1 AND AOB.BreakdownObjID=@CostCenterID

					INSERT INTO #TempTable(	CCID,		ParentHeadID,		ComponentID)
						SELECT DISTINCT		CCT.CCID,	CCT.AccountHeadID,	dbo.GetComponentID(CCT.AccountHeadID) FROM View_CostCenterTransaction AS CCT WHERE ISNULL(CCT.ApprovedBy,0)!=0 AND CCT.BUID=@BusinessUnitID AND CCT.CurrencyID=@CurrencyID AND CCT.CCID=@CostCenterID AND CONVERT(DATE, CONVERT(VARCHAR(12),CCT.TransactionDate,106)) BETWEEN  CONVERT(DATE, CONVERT(VARCHAR(12),@SessionStartDate,106)) AND CONVERT(DATE, CONVERT(VARCHAR(12),@EndDate,106))
				END
			END
			ELSE
			BEGIN
				IF(@CurrencyID=@BaseCurrencyID)
				BEGIN
					INSERT INTO #TempTable(	CCID,		ParentHeadID,		ComponentID)
						SELECT DISTINCT		AOB.BreakdownObjID,	AOB.AccountHeadID,	dbo.GetComponentID(AOB.AccountHeadID) FROM View_AccountOpenningBreakdown AS AOB WHERE AOB.BusinessUnitID=@BusinessUnitID AND AOB.AccountingSessionID=@SessionID AND AOB.BreakdownType=1 AND AOB.BreakdownObjID=@CostCenterID

					INSERT INTO #TempTable(	CCID,		ParentHeadID,		ComponentID)
						SELECT DISTINCT		CCT.CCID,	CCT.AccountHeadID,	dbo.GetComponentID(CCT.AccountHeadID) FROM View_CostCenterTransaction AS CCT WHERE CCT.BUID=@BusinessUnitID AND CCT.CCID=@CostCenterID AND CONVERT(DATE, CONVERT(VARCHAR(12),CCT.TransactionDate,106)) BETWEEN  CONVERT(DATE, CONVERT(VARCHAR(12),@SessionStartDate,106)) AND CONVERT(DATE, CONVERT(VARCHAR(12),@EndDate,106))
				END
				ELSE
				BEGIN
					INSERT INTO #TempTable(	CCID,		ParentHeadID,		ComponentID)
						SELECT DISTINCT		AOB.BreakdownObjID,	AOB.AccountHeadID,	dbo.GetComponentID(AOB.AccountHeadID) FROM View_AccountOpenningBreakdown AS AOB WHERE AOB.CurrencyID=@CurrencyID AND AOB.BusinessUnitID=@BusinessUnitID AND AOB.AccountingSessionID=@SessionID AND AOB.BreakdownType=1 AND AOB.BreakdownObjID=@CostCenterID

					INSERT INTO #TempTable(	CCID,		ParentHeadID,		ComponentID)
						SELECT DISTINCT		CCT.CCID,	CCT.AccountHeadID,	dbo.GetComponentID(CCT.AccountHeadID) FROM View_CostCenterTransaction AS CCT WHERE CCT.BUID=@BusinessUnitID AND CCT.CurrencyID=@CurrencyID AND CCT.CCID=@CostCenterID AND CONVERT(DATE, CONVERT(VARCHAR(12),CCT.TransactionDate,106)) BETWEEN  CONVERT(DATE, CONVERT(VARCHAR(12),@SessionStartDate,106)) AND CONVERT(DATE, CONVERT(VARCHAR(12),@EndDate,106))
				END
			END
		END
	END
	ELSE
	BEGIN
		IF(@AccountHeadID>0)
		BEGIN
			IF(@IsApproved=1)
			BEGIN
				IF(@CurrencyID=@BaseCurrencyID)
				BEGIN
					INSERT INTO #TempTable(	CCID,		ParentHeadID,		ComponentID)
						SELECT DISTINCT		AOB.BreakdownObjID,	AOB.AccountHeadID,	dbo.GetComponentID(AOB.AccountHeadID) FROM View_AccountOpenningBreakdown AS AOB WHERE AOB.AccountingSessionID=@SessionID AND AOB.BreakdownType=1 AND AOB.AccountHeadID=@AccountHeadID AND AOB.BreakdownObjID=@CostCenterID

					INSERT INTO #TempTable(	CCID,		ParentHeadID,		ComponentID)
						SELECT DISTINCT		CCT.CCID,	CCT.AccountHeadID,	dbo.GetComponentID(CCT.AccountHeadID) FROM View_CostCenterTransaction AS CCT WHERE ISNULL(CCT.ApprovedBy,0)!=0 AND CCT.CCID=@CostCenterID AND CCT.AccountHeadID=@AccountHeadID AND CONVERT(DATE, CONVERT(VARCHAR(12),CCT.TransactionDate,106)) BETWEEN  CONVERT(DATE, CONVERT(VARCHAR(12),@SessionStartDate,106)) AND CONVERT(DATE, CONVERT(VARCHAR(12),@EndDate,106))
				END
				ELSE
				BEGIN
					INSERT INTO #TempTable(	CCID,		ParentHeadID,		ComponentID)
						SELECT DISTINCT		AOB.BreakdownObjID,	AOB.AccountHeadID,	dbo.GetComponentID(AOB.AccountHeadID) FROM View_AccountOpenningBreakdown AS AOB WHERE AOB.CurrencyID=@CurrencyID AND AOB.AccountingSessionID=@SessionID AND AOB.BreakdownType=1 AND AOB.AccountHeadID=@AccountHeadID AND AOB.BreakdownObjID=@CostCenterID

					INSERT INTO #TempTable(	CCID,		ParentHeadID,		ComponentID)
						SELECT DISTINCT		CCT.CCID,	CCT.AccountHeadID,	dbo.GetComponentID(CCT.AccountHeadID) FROM View_CostCenterTransaction AS CCT WHERE ISNULL(CCT.ApprovedBy,0)!=0 AND CCT.CurrencyID=@CurrencyID AND CCT.CCID=@CostCenterID AND CCT.AccountHeadID=@AccountHeadID AND CONVERT(DATE, CONVERT(VARCHAR(12),CCT.TransactionDate,106)) BETWEEN  CONVERT(DATE, CONVERT(VARCHAR(12),@SessionStartDate,106)) AND CONVERT(DATE, CONVERT(VARCHAR(12),@EndDate,106))
				END
			END
			ELSE
			BEGIN
				IF(@CurrencyID=@BaseCurrencyID)
				BEGIN
					INSERT INTO #TempTable(	CCID,		ParentHeadID,		ComponentID)
						SELECT DISTINCT		AOB.BreakdownObjID,	AOB.AccountHeadID,	dbo.GetComponentID(AOB.AccountHeadID) FROM View_AccountOpenningBreakdown AS AOB WHERE AOB.AccountingSessionID=@SessionID AND AOB.BreakdownType=1 AND AOB.AccountHeadID=@AccountHeadID AND AOB.BreakdownObjID=@CostCenterID

					INSERT INTO #TempTable(	CCID,		ParentHeadID,		ComponentID)
						SELECT DISTINCT		CCT.CCID,	CCT.AccountHeadID,	dbo.GetComponentID(CCT.AccountHeadID) FROM View_CostCenterTransaction AS CCT WHERE CCT.CCID=@CostCenterID AND CCT.AccountHeadID=@AccountHeadID AND CONVERT(DATE, CONVERT(VARCHAR(12),CCT.TransactionDate,106)) BETWEEN  CONVERT(DATE, CONVERT(VARCHAR(12),@SessionStartDate,106)) AND CONVERT(DATE, CONVERT(VARCHAR(12),@EndDate,106))
				END
				ELSE
				BEGIN
					INSERT INTO #TempTable(	CCID,		ParentHeadID,		ComponentID)
						SELECT DISTINCT		AOB.BreakdownObjID,	AOB.AccountHeadID,	dbo.GetComponentID(AOB.AccountHeadID) FROM View_AccountOpenningBreakdown AS AOB WHERE AOB.CurrencyID=@CurrencyID AND AOB.AccountingSessionID=@SessionID AND AOB.BreakdownType=1 AND AOB.AccountHeadID=@AccountHeadID AND AOB.BreakdownObjID=@CostCenterID

					INSERT INTO #TempTable(	CCID,		ParentHeadID,		ComponentID)
						SELECT DISTINCT		CCT.CCID,	CCT.AccountHeadID,	dbo.GetComponentID(CCT.AccountHeadID) FROM View_CostCenterTransaction AS CCT WHERE CCT.CurrencyID=@CurrencyID AND CCT.CCID=@CostCenterID AND CCT.AccountHeadID=@AccountHeadID AND CONVERT(DATE, CONVERT(VARCHAR(12),CCT.TransactionDate,106)) BETWEEN  CONVERT(DATE, CONVERT(VARCHAR(12),@SessionStartDate,106)) AND CONVERT(DATE, CONVERT(VARCHAR(12),@EndDate,106))
				END
			END
		END
		ELSE
		BEGIN
			IF(@IsApproved=1)
			BEGIN
				IF(@CurrencyID=@BaseCurrencyID)
				BEGIN
					INSERT INTO #TempTable(	CCID,		ParentHeadID,		ComponentID)
						SELECT DISTINCT		AOB.BreakdownObjID,	AOB.AccountHeadID,	dbo.GetComponentID(AOB.AccountHeadID) FROM View_AccountOpenningBreakdown AS AOB WHERE AOB.AccountingSessionID=@SessionID AND AOB.BreakdownType=1 AND AOB.BreakdownObjID=@CostCenterID

					INSERT INTO #TempTable(	CCID,		ParentHeadID,		ComponentID)
						SELECT DISTINCT		CCT.CCID,	CCT.AccountHeadID,	dbo.GetComponentID(CCT.AccountHeadID) FROM View_CostCenterTransaction AS CCT WHERE ISNULL(CCT.ApprovedBy,0)!=0 AND CCT.CCID=@CostCenterID AND CONVERT(DATE, CONVERT(VARCHAR(12),CCT.TransactionDate,106)) BETWEEN  CONVERT(DATE, CONVERT(VARCHAR(12),@SessionStartDate,106)) AND CONVERT(DATE, CONVERT(VARCHAR(12),@EndDate,106))
				END
				ELSE
				BEGIN
					INSERT INTO #TempTable(	CCID,		ParentHeadID,		ComponentID)
						SELECT DISTINCT		AOB.BreakdownObjID,	AOB.AccountHeadID,	dbo.GetComponentID(AOB.AccountHeadID) FROM View_AccountOpenningBreakdown AS AOB WHERE AOB.CurrencyID=@CurrencyID AND AOB.AccountingSessionID=@SessionID AND AOB.BreakdownType=1 AND AOB.BreakdownObjID=@CostCenterID

					INSERT INTO #TempTable(	CCID,		ParentHeadID,		ComponentID)
						SELECT DISTINCT		CCT.CCID,	CCT.AccountHeadID,	dbo.GetComponentID(CCT.AccountHeadID) FROM View_CostCenterTransaction AS CCT WHERE ISNULL(CCT.ApprovedBy,0)!=0 AND CCT.CurrencyID=@CurrencyID AND CCT.CCID=@CostCenterID AND CONVERT(DATE, CONVERT(VARCHAR(12),CCT.TransactionDate,106)) BETWEEN  CONVERT(DATE, CONVERT(VARCHAR(12),@SessionStartDate,106)) AND CONVERT(DATE, CONVERT(VARCHAR(12),@EndDate,106))
				END
			END
			ELSE
			BEGIN
				IF(@CurrencyID=@BaseCurrencyID)
				BEGIN
					INSERT INTO #TempTable(	CCID,		ParentHeadID,		ComponentID)
						SELECT DISTINCT		AOB.BreakdownObjID,	AOB.AccountHeadID,	dbo.GetComponentID(AOB.AccountHeadID) FROM View_AccountOpenningBreakdown AS AOB WHERE AOB.AccountingSessionID=@SessionID AND AOB.BreakdownType=1 AND AOB.BreakdownObjID=@CostCenterID

					INSERT INTO #TempTable(	CCID,		ParentHeadID,		ComponentID)
						SELECT DISTINCT		CCT.CCID,	CCT.AccountHeadID,	dbo.GetComponentID(CCT.AccountHeadID) FROM View_CostCenterTransaction AS CCT WHERE CCT.CCID=@CostCenterID AND CONVERT(DATE, CONVERT(VARCHAR(12),CCT.TransactionDate,106)) BETWEEN  CONVERT(DATE, CONVERT(VARCHAR(12),@SessionStartDate,106)) AND CONVERT(DATE, CONVERT(VARCHAR(12),@EndDate,106))
				END
				ELSE
				BEGIN
					INSERT INTO #TempTable(	CCID,		ParentHeadID,		ComponentID)
						SELECT DISTINCT		AOB.BreakdownObjID,	AOB.AccountHeadID,	dbo.GetComponentID(AOB.AccountHeadID) FROM View_AccountOpenningBreakdown AS AOB WHERE AOB.CurrencyID=@CurrencyID AND AOB.AccountingSessionID=@SessionID AND AOB.BreakdownType=1 AND AOB.BreakdownObjID=@CostCenterID

					INSERT INTO #TempTable(	CCID,		ParentHeadID,		ComponentID)
						SELECT DISTINCT		CCT.CCID,	CCT.AccountHeadID,	dbo.GetComponentID(CCT.AccountHeadID) FROM View_CostCenterTransaction AS CCT WHERE CCT.CurrencyID=@CurrencyID AND CCT.CCID=@CostCenterID AND CONVERT(DATE, CONVERT(VARCHAR(12),CCT.TransactionDate,106)) BETWEEN  CONVERT(DATE, CONVERT(VARCHAR(12),@SessionStartDate,106)) AND CONVERT(DATE, CONVERT(VARCHAR(12),@EndDate,106))
				END
			END
		END
	END
END
ELSE
BEGIN
	IF(@BusinessUnitID>0)
	BEGIN
		IF(@AccountHeadID>0)
		BEGIN
			IF(@IsApproved=1)
			BEGIN
				IF(@CurrencyID=@BaseCurrencyID)
				BEGIN
					INSERT INTO #TempTable(	CCID,		ParentHeadID,		ComponentID)
						SELECT DISTINCT		AOB.BreakdownObjID,	AOB.AccountHeadID,	dbo.GetComponentID(AOB.AccountHeadID) FROM View_AccountOpenningBreakdown AS AOB WHERE AOB.BusinessUnitID=@BusinessUnitID AND AOB.AccountingSessionID=@SessionID AND AOB.BreakdownType=1 AND AOB.AccountHeadID=@AccountHeadID 

					INSERT INTO #TempTable(	CCID,		ParentHeadID,		ComponentID)
						SELECT DISTINCT		CCT.CCID,	CCT.AccountHeadID,	dbo.GetComponentID(CCT.AccountHeadID) FROM View_CostCenterTransaction AS CCT WHERE ISNULL(CCT.ApprovedBy,0)!=0 AND CCT.BUID=@BusinessUnitID AND CCT.AccountHeadID=@AccountHeadID AND CONVERT(DATE, CONVERT(VARCHAR(12),CCT.TransactionDate,106)) BETWEEN  CONVERT(DATE, CONVERT(VARCHAR(12),@SessionStartDate,106)) AND CONVERT(DATE, CONVERT(VARCHAR(12),@EndDate,106))
				END
				ELSE
				BEGIN
					INSERT INTO #TempTable(	CCID,		ParentHeadID,		ComponentID)
						SELECT DISTINCT		AOB.BreakdownObjID,	AOB.AccountHeadID,	dbo.GetComponentID(AOB.AccountHeadID) FROM View_AccountOpenningBreakdown AS AOB WHERE AOB.CurrencyID=@CurrencyID AND AOB.BusinessUnitID=@BusinessUnitID AND AOB.AccountingSessionID=@SessionID AND AOB.BreakdownType=1 AND AOB.AccountHeadID=@AccountHeadID 

					INSERT INTO #TempTable(	CCID,		ParentHeadID,		ComponentID)
						SELECT DISTINCT		CCT.CCID,	CCT.AccountHeadID,	dbo.GetComponentID(CCT.AccountHeadID) FROM View_CostCenterTransaction AS CCT WHERE ISNULL(CCT.ApprovedBy,0)!=0 AND CCT.BUID=@BusinessUnitID AND CCT.CurrencyID=@CurrencyID AND CONVERT(DATE, CONVERT(VARCHAR(12),CCT.TransactionDate,106)) BETWEEN  CONVERT(DATE, CONVERT(VARCHAR(12),@SessionStartDate,106)) AND CONVERT(DATE, CONVERT(VARCHAR(12),@EndDate,106))
				END
			END
			ELSE
			BEGIN
				IF(@CurrencyID=@BaseCurrencyID)
				BEGIN
					INSERT INTO #TempTable(	CCID,		ParentHeadID,		ComponentID)
						SELECT DISTINCT		AOB.BreakdownObjID,	AOB.AccountHeadID,	dbo.GetComponentID(AOB.AccountHeadID) FROM View_AccountOpenningBreakdown AS AOB WHERE AOB.BusinessUnitID=@BusinessUnitID AND AOB.AccountingSessionID=@SessionID AND AOB.BreakdownType=1 AND AOB.AccountHeadID=@AccountHeadID 

					INSERT INTO #TempTable(	CCID,		ParentHeadID,		ComponentID)
						SELECT DISTINCT		CCT.CCID,	CCT.AccountHeadID,	dbo.GetComponentID(CCT.AccountHeadID) FROM View_CostCenterTransaction AS CCT WHERE CCT.BUID=@BusinessUnitID AND CCT.AccountHeadID=@AccountHeadID AND CONVERT(DATE, CONVERT(VARCHAR(12),CCT.TransactionDate,106)) BETWEEN  CONVERT(DATE, CONVERT(VARCHAR(12),@SessionStartDate,106)) AND CONVERT(DATE, CONVERT(VARCHAR(12),@EndDate,106))
				END
				ELSE
				BEGIN
					INSERT INTO #TempTable(	CCID,		ParentHeadID,		ComponentID)
						SELECT DISTINCT		AOB.BreakdownObjID,	AOB.AccountHeadID,	dbo.GetComponentID(AOB.AccountHeadID) FROM View_AccountOpenningBreakdown AS AOB WHERE AOB.CurrencyID=@CurrencyID AND AOB.BusinessUnitID=@BusinessUnitID AND AOB.AccountingSessionID=@SessionID AND AOB.BreakdownType=1 AND AOB.AccountHeadID=@AccountHeadID 

					INSERT INTO #TempTable(	CCID,		ParentHeadID,		ComponentID)
						SELECT DISTINCT		CCT.CCID,	CCT.AccountHeadID,	dbo.GetComponentID(CCT.AccountHeadID) FROM View_CostCenterTransaction AS CCT WHERE CCT.BUID=@BusinessUnitID AND CCT.CurrencyID=@CurrencyID AND CCT.AccountHeadID=@AccountHeadID AND CONVERT(DATE, CONVERT(VARCHAR(12),CCT.TransactionDate,106)) BETWEEN  CONVERT(DATE, CONVERT(VARCHAR(12),@SessionStartDate,106)) AND CONVERT(DATE, CONVERT(VARCHAR(12),@EndDate,106))
				END
			END
		END
		ELSE
		BEGIN
			IF(@IsApproved=1)
			BEGIN
				IF(@CurrencyID=@BaseCurrencyID)
				BEGIN
					INSERT INTO #TempTable(	CCID,		ParentHeadID,		ComponentID)
						SELECT DISTINCT		AOB.BreakdownObjID,	AOB.AccountHeadID,	dbo.GetComponentID(AOB.AccountHeadID) FROM View_AccountOpenningBreakdown AS AOB WHERE AOB.BusinessUnitID=@BusinessUnitID AND AOB.AccountingSessionID=@SessionID AND AOB.BreakdownType=1 

					INSERT INTO #TempTable(	CCID,		ParentHeadID,		ComponentID)
						SELECT DISTINCT		CCT.CCID,	CCT.AccountHeadID,	dbo.GetComponentID(CCT.AccountHeadID) FROM View_CostCenterTransaction AS CCT WHERE ISNULL(CCT.ApprovedBy,0)!=0 AND CCT.BUID=@BusinessUnitID AND CONVERT(DATE, CONVERT(VARCHAR(12),CCT.TransactionDate,106)) BETWEEN  CONVERT(DATE, CONVERT(VARCHAR(12),@SessionStartDate,106)) AND CONVERT(DATE, CONVERT(VARCHAR(12),@EndDate,106))
				END
				ELSE
				BEGIN
					INSERT INTO #TempTable(	CCID,		ParentHeadID,		ComponentID)
						SELECT DISTINCT		AOB.BreakdownObjID,	AOB.AccountHeadID,	dbo.GetComponentID(AOB.AccountHeadID) FROM View_AccountOpenningBreakdown AS AOB WHERE AOB.CurrencyID=@CurrencyID AND AOB.BusinessUnitID=@BusinessUnitID AND AOB.AccountingSessionID=@SessionID AND AOB.BreakdownType=1

					INSERT INTO #TempTable(	CCID,		ParentHeadID,		ComponentID)
						SELECT DISTINCT		CCT.CCID,	CCT.AccountHeadID,	dbo.GetComponentID(CCT.AccountHeadID) FROM View_CostCenterTransaction AS CCT WHERE ISNULL(CCT.ApprovedBy,0)!=0 AND CCT.BUID=@BusinessUnitID AND CCT.CurrencyID=@CurrencyID AND CONVERT(DATE, CONVERT(VARCHAR(12),CCT.TransactionDate,106)) BETWEEN  CONVERT(DATE, CONVERT(VARCHAR(12),@SessionStartDate,106)) AND CONVERT(DATE, CONVERT(VARCHAR(12),@EndDate,106))
				END
			END
			ELSE
			BEGIN
				IF(@CurrencyID=@BaseCurrencyID)
				BEGIN
					INSERT INTO #TempTable(	CCID,		ParentHeadID,		ComponentID)
						SELECT DISTINCT		AOB.BreakdownObjID,	AOB.AccountHeadID,	dbo.GetComponentID(AOB.AccountHeadID) FROM View_AccountOpenningBreakdown AS AOB WHERE AOB.BusinessUnitID=@BusinessUnitID AND AOB.AccountingSessionID=@SessionID AND AOB.BreakdownType=1 

					INSERT INTO #TempTable(	CCID,		ParentHeadID,		ComponentID)
						SELECT DISTINCT		CCT.CCID,	CCT.AccountHeadID,	dbo.GetComponentID(CCT.AccountHeadID) FROM View_CostCenterTransaction AS CCT WHERE CCT.BUID=@BusinessUnitID AND CONVERT(DATE, CONVERT(VARCHAR(12),CCT.TransactionDate,106)) BETWEEN  CONVERT(DATE, CONVERT(VARCHAR(12),@SessionStartDate,106)) AND CONVERT(DATE, CONVERT(VARCHAR(12),@EndDate,106))
				END
				ELSE
				BEGIN
					INSERT INTO #TempTable(	CCID,		ParentHeadID,		ComponentID)
						SELECT DISTINCT		AOB.BreakdownObjID,	AOB.AccountHeadID,	dbo.GetComponentID(AOB.AccountHeadID) FROM View_AccountOpenningBreakdown AS AOB WHERE AOB.CurrencyID=@CurrencyID AND AOB.BusinessUnitID=@BusinessUnitID AND AOB.AccountingSessionID=@SessionID AND AOB.BreakdownType=1 

					INSERT INTO #TempTable(	CCID,		ParentHeadID,		ComponentID)
						SELECT DISTINCT		CCT.CCID,	CCT.AccountHeadID,	dbo.GetComponentID(CCT.AccountHeadID) FROM View_CostCenterTransaction AS CCT WHERE CCT.BUID=@BusinessUnitID AND CCT.CurrencyID=@CurrencyID AND CONVERT(DATE, CONVERT(VARCHAR(12),CCT.TransactionDate,106)) BETWEEN  CONVERT(DATE, CONVERT(VARCHAR(12),@SessionStartDate,106)) AND CONVERT(DATE, CONVERT(VARCHAR(12),@EndDate,106))
				END
			END
		END
	END
	ELSE
	BEGIN
		IF(@AccountHeadID>0)
		BEGIN
			IF(@IsApproved=1)
			BEGIN
				IF(@CurrencyID=@BaseCurrencyID)
				BEGIN
					INSERT INTO #TempTable(	CCID,		ParentHeadID,		ComponentID)
						SELECT DISTINCT		AOB.BreakdownObjID,	AOB.AccountHeadID,	dbo.GetComponentID(AOB.AccountHeadID) FROM View_AccountOpenningBreakdown AS AOB WHERE AOB.AccountingSessionID=@SessionID AND AOB.BreakdownType=1 AND AOB.AccountHeadID=@AccountHeadID 

					INSERT INTO #TempTable(	CCID,		ParentHeadID,		ComponentID)
						SELECT DISTINCT		CCT.CCID,	CCT.AccountHeadID,	dbo.GetComponentID(CCT.AccountHeadID) FROM View_CostCenterTransaction AS CCT WHERE ISNULL(CCT.ApprovedBy,0)!=0 AND CCT.AccountHeadID=@AccountHeadID AND CONVERT(DATE, CONVERT(VARCHAR(12),CCT.TransactionDate,106)) BETWEEN  CONVERT(DATE, CONVERT(VARCHAR(12),@SessionStartDate,106)) AND CONVERT(DATE, CONVERT(VARCHAR(12),@EndDate,106))
				END
				ELSE
				BEGIN
					INSERT INTO #TempTable(	CCID,		ParentHeadID,		ComponentID)
						SELECT DISTINCT		AOB.BreakdownObjID,	AOB.AccountHeadID,	dbo.GetComponentID(AOB.AccountHeadID) FROM View_AccountOpenningBreakdown AS AOB WHERE AOB.CurrencyID=@CurrencyID AND AOB.AccountingSessionID=@SessionID AND AOB.BreakdownType=1 AND AOB.AccountHeadID=@AccountHeadID 

					INSERT INTO #TempTable(	CCID,		ParentHeadID,		ComponentID)
						SELECT DISTINCT		CCT.CCID,	CCT.AccountHeadID,	dbo.GetComponentID(CCT.AccountHeadID) FROM View_CostCenterTransaction AS CCT WHERE ISNULL(CCT.ApprovedBy,0)!=0 AND CCT.CurrencyID=@CurrencyID AND CCT.AccountHeadID=@AccountHeadID AND CONVERT(DATE, CONVERT(VARCHAR(12),CCT.TransactionDate,106)) BETWEEN  CONVERT(DATE, CONVERT(VARCHAR(12),@SessionStartDate,106)) AND CONVERT(DATE, CONVERT(VARCHAR(12),@EndDate,106))
				END
			END
			ELSE
			BEGIN
				IF(@CurrencyID=@BaseCurrencyID)
				BEGIN
					INSERT INTO #TempTable(	CCID,		ParentHeadID,		ComponentID)
						SELECT DISTINCT		AOB.BreakdownObjID,	AOB.AccountHeadID,	dbo.GetComponentID(AOB.AccountHeadID) FROM View_AccountOpenningBreakdown AS AOB WHERE AOB.AccountingSessionID=@SessionID AND AOB.BreakdownType=1 AND AOB.AccountHeadID=@AccountHeadID 

					INSERT INTO #TempTable(	CCID,		ParentHeadID,		ComponentID)
						SELECT DISTINCT		CCT.CCID,	CCT.AccountHeadID,	dbo.GetComponentID(CCT.AccountHeadID) FROM View_CostCenterTransaction AS CCT WHERE CCT.AccountHeadID=@AccountHeadID AND CONVERT(DATE, CONVERT(VARCHAR(12),CCT.TransactionDate,106)) BETWEEN  CONVERT(DATE, CONVERT(VARCHAR(12),@SessionStartDate,106)) AND CONVERT(DATE, CONVERT(VARCHAR(12),@EndDate,106))
				END
				ELSE
				BEGIN
					INSERT INTO #TempTable(	CCID,		ParentHeadID,		ComponentID)
						SELECT DISTINCT		AOB.BreakdownObjID,	AOB.AccountHeadID,	dbo.GetComponentID(AOB.AccountHeadID) FROM View_AccountOpenningBreakdown AS AOB WHERE AOB.CurrencyID=@CurrencyID AND AOB.AccountingSessionID=@SessionID AND AOB.BreakdownType=1 AND AOB.AccountHeadID=@AccountHeadID 

					INSERT INTO #TempTable(	CCID,		ParentHeadID,		ComponentID)
						SELECT DISTINCT		CCT.CCID,	CCT.AccountHeadID,	dbo.GetComponentID(CCT.AccountHeadID) FROM View_CostCenterTransaction AS CCT WHERE CCT.CurrencyID=@CurrencyID AND CCT.AccountHeadID=@AccountHeadID AND CONVERT(DATE, CONVERT(VARCHAR(12),CCT.TransactionDate,106)) BETWEEN  CONVERT(DATE, CONVERT(VARCHAR(12),@SessionStartDate,106)) AND CONVERT(DATE, CONVERT(VARCHAR(12),@EndDate,106))
				END
			END
		END
		ELSE
		BEGIN
			IF(@IsApproved=1)
			BEGIN
				IF(@CurrencyID=@BaseCurrencyID)
				BEGIN
					INSERT INTO #TempTable(	CCID,		ParentHeadID,		ComponentID)
						SELECT DISTINCT		AOB.BreakdownObjID,	AOB.AccountHeadID,	dbo.GetComponentID(AOB.AccountHeadID) FROM View_AccountOpenningBreakdown AS AOB WHERE AOB.AccountingSessionID=@SessionID AND AOB.BreakdownType=1 

					INSERT INTO #TempTable(	CCID,		ParentHeadID,		ComponentID)
						SELECT DISTINCT		CCT.CCID,	CCT.AccountHeadID,	dbo.GetComponentID(CCT.AccountHeadID) FROM View_CostCenterTransaction AS CCT WHERE ISNULL(CCT.ApprovedBy,0)!=0 AND CONVERT(DATE, CONVERT(VARCHAR(12),CCT.TransactionDate,106)) BETWEEN  CONVERT(DATE, CONVERT(VARCHAR(12),@SessionStartDate,106)) AND CONVERT(DATE, CONVERT(VARCHAR(12),@EndDate,106))
				END
				ELSE
				BEGIN
					INSERT INTO #TempTable(	CCID,		ParentHeadID,		ComponentID)
						SELECT DISTINCT		AOB.BreakdownObjID,	AOB.AccountHeadID,	dbo.GetComponentID(AOB.AccountHeadID) FROM View_AccountOpenningBreakdown AS AOB WHERE AOB.CurrencyID=@CurrencyID AND AOB.AccountingSessionID=@SessionID AND AOB.BreakdownType=1 

					INSERT INTO #TempTable(	CCID,		ParentHeadID,		ComponentID)
						SELECT DISTINCT		CCT.CCID,	CCT.AccountHeadID,	dbo.GetComponentID(CCT.AccountHeadID) FROM View_CostCenterTransaction AS CCT WHERE ISNULL(CCT.ApprovedBy,0)!=0 AND CCT.CurrencyID=@CurrencyID AND CONVERT(DATE, CONVERT(VARCHAR(12),CCT.TransactionDate,106)) BETWEEN  CONVERT(DATE, CONVERT(VARCHAR(12),@SessionStartDate,106)) AND CONVERT(DATE, CONVERT(VARCHAR(12),@EndDate,106))
				END
			END
			ELSE
			BEGIN
				IF(@CurrencyID=@BaseCurrencyID)
				BEGIN
					INSERT INTO #TempTable(	CCID,		ParentHeadID,		ComponentID)
						SELECT DISTINCT		AOB.BreakdownObjID,	AOB.AccountHeadID,	dbo.GetComponentID(AOB.AccountHeadID) FROM View_AccountOpenningBreakdown AS AOB WHERE AOB.AccountingSessionID=@SessionID AND AOB.BreakdownType=1 

					INSERT INTO #TempTable(	CCID,		ParentHeadID,		ComponentID)
						SELECT DISTINCT		CCT.CCID,	CCT.AccountHeadID,	dbo.GetComponentID(CCT.AccountHeadID) FROM View_CostCenterTransaction AS CCT WHERE CONVERT(DATE, CONVERT(VARCHAR(12),CCT.TransactionDate,106)) BETWEEN  CONVERT(DATE, CONVERT(VARCHAR(12),@SessionStartDate,106)) AND CONVERT(DATE, CONVERT(VARCHAR(12),@EndDate,106))
				END
				ELSE
				BEGIN
					INSERT INTO #TempTable(	CCID,		ParentHeadID,		ComponentID)
						SELECT DISTINCT		AOB.BreakdownObjID,	AOB.AccountHeadID,	dbo.GetComponentID(AOB.AccountHeadID) FROM View_AccountOpenningBreakdown AS AOB WHERE AOB.CurrencyID=@CurrencyID AND AOB.AccountingSessionID=@SessionID AND AOB.BreakdownType=1 

					INSERT INTO #TempTable(	CCID,		ParentHeadID,		ComponentID)
						SELECT DISTINCT		CCT.CCID,	CCT.AccountHeadID,	dbo.GetComponentID(CCT.AccountHeadID) FROM View_CostCenterTransaction AS CCT WHERE CCT.CurrencyID=@CurrencyID AND CONVERT(DATE, CONVERT(VARCHAR(12),CCT.TransactionDate,106)) BETWEEN  CONVERT(DATE, CONVERT(VARCHAR(12),@SessionStartDate,106)) AND CONVERT(DATE, CONVERT(VARCHAR(12),@EndDate,106))
				END
			END
		END
	END
END

DELETE FROM #TempTable WHERE ID NOT IN (SELECT MIN(ID) FROM #TempTable GROUP BY CCID,ParentHeadID)

--SELECT * FROM #TempTable
IF(@BusinessUnitID>0)
BEGIN
	IF(@IsApproved=1)
	BEGIN
		IF(@CurrencyID=@BaseCurrencyID)
		BEGIN
			UPDATE #TempTable
			SET DebitOpeiningValue=ISNULL((SELECT SUM(AOB.OpenningBalance) FROM AccountOpenningBreakdown AS AOB WHERE AOB.BusinessUnitID=@BusinessUnitID AND AOB.IsDr=1 AND AOB.AccountHeadID=TT.ParentHeadID AND AOB.AccountingSessionID=@SessionID AND AOB.BreakdownType=1 AND AOB.BreakdownObjID=TT.CCID),0),
				CreditOpeiningValue=ISNULL((SELECT SUM(AOB.OpenningBalance) FROM AccountOpenningBreakdown AS AOB WHERE AOB.BusinessUnitID=@BusinessUnitID AND AOB.IsDr=0 AND AOB.AccountHeadID=TT.ParentHeadID AND AOB.AccountingSessionID=@SessionID AND AOB.BreakdownType=1 AND AOB.BreakdownObjID=TT.CCID),0),
				DebitAmount=ISNULL((SELECT SUM(CCT.Amount*CCT.CurrencyConversionRate) FROM View_CostCenterTransaction AS CCT WHERE CCT.BUID=@BusinessUnitID AND ISNULL(CCT.ApprovedBy,0)!=0 AND  CCT.IsDr=1 AND CCT.CCID=TT.CCID AND CCT.AccountHeadID=TT.ParentHeadID AND CONVERT(DATE, CONVERT(VARCHAR(12),CCT.TransactionDate,106)) BETWEEN  CONVERT(DATE, CONVERT(VARCHAR(12),@SessionStartDate,106)) AND CONVERT(DATE, CONVERT(VARCHAR(12),DATEADD(DAY,-1,@StartDate),106))),0),
				CreditAmount=ISNULL((SELECT SUM(CCT.Amount*CCT.CurrencyConversionRate) FROM View_CostCenterTransaction AS CCT WHERE CCT.BUID=@BusinessUnitID AND ISNULL(CCT.ApprovedBy,0)!=0 AND CCT.IsDr=0 AND CCT.CCID=TT.CCID AND CCT.AccountHeadID=TT.ParentHeadID AND CONVERT(DATE, CONVERT(VARCHAR(12),CCT.TransactionDate,106)) BETWEEN  CONVERT(DATE, CONVERT(VARCHAR(12),@SessionStartDate,106)) AND CONVERT(DATE, CONVERT(VARCHAR(12),DATEADD(DAY,-1,@StartDate),106))),0)
			FROM #TempTable AS TT
		END
		ELSE
		BEGIN
			UPDATE #TempTable
			SET DebitOpeiningValue=ISNULL((SELECT SUM(AOB.AmountInCurrency) FROM AccountOpenningBreakdown AS AOB WHERE AOB.BusinessUnitID=@BusinessUnitID AND AOB.IsDr=1 AND AOB.CurrencyID=@CurrencyID AND AOB.AccountHeadID=TT.ParentHeadID AND AOB.AccountingSessionID=@SessionID AND AOB.BreakdownType=1 AND AOB.BreakdownObjID=TT.CCID),0),
				CreditOpeiningValue=ISNULL((SELECT SUM(AOB.AmountInCurrency) FROM AccountOpenningBreakdown AS AOB WHERE AOB.BusinessUnitID=@BusinessUnitID AND AOB.IsDr=0 AND AOB.CurrencyID=@CurrencyID AND AOB.AccountHeadID=TT.ParentHeadID AND AOB.AccountingSessionID=@SessionID AND AOB.BreakdownType=1 AND AOB.BreakdownObjID=TT.CCID),0),
				DebitAmount=ISNULL((SELECT SUM(CCT.Amount*CCT.CurrencyConversionRate) FROM View_CostCenterTransaction AS CCT WHERE  CCT.BUID=@BusinessUnitID AND ISNULL(CCT.ApprovedBy,0)!=0 AND CCT.CurrencyID=@CurrencyID AND CCT.IsDr=1 AND CCT.CCID=TT.CCID AND CCT.AccountHeadID=TT.ParentHeadID AND CONVERT(DATE, CONVERT(VARCHAR(12),CCT.TransactionDate,106)) BETWEEN  CONVERT(DATE, CONVERT(VARCHAR(12),@SessionStartDate,106)) AND CONVERT(DATE, CONVERT(VARCHAR(12),DATEADD(DAY,-1,@StartDate),106))),0),
				CreditAmount=ISNULL((SELECT SUM(CCT.Amount*CCT.CurrencyConversionRate) FROM View_CostCenterTransaction AS CCT WHERE  CCT.BUID=@BusinessUnitID AND ISNULL(CCT.ApprovedBy,0)!=0 AND CCT.CurrencyID=@CurrencyID AND CCT.IsDr=0 AND CCT.CCID=TT.CCID AND CCT.AccountHeadID=TT.ParentHeadID AND CONVERT(DATE, CONVERT(VARCHAR(12),CCT.TransactionDate,106)) BETWEEN  CONVERT(DATE, CONVERT(VARCHAR(12),@SessionStartDate,106)) AND CONVERT(DATE, CONVERT(VARCHAR(12),DATEADD(DAY,-1,@StartDate),106))),0)
			FROM #TempTable AS TT
		END
	END
	ELSE
	BEGIN
		IF(@CurrencyID=@BaseCurrencyID)
		BEGIN
			UPDATE #TempTable
			SET DebitOpeiningValue=ISNULL((SELECT SUM(AOB.OpenningBalance) FROM AccountOpenningBreakdown AS AOB WHERE AOB.BusinessUnitID=@BusinessUnitID AND AOB.IsDr=1 AND AOB.AccountHeadID=TT.ParentHeadID AND AOB.AccountingSessionID=@SessionID AND AOB.BreakdownType=1 AND AOB.BreakdownObjID=TT.CCID),0),
			CreditOpeiningValue=ISNULL((SELECT SUM(AOB.OpenningBalance) FROM AccountOpenningBreakdown AS AOB WHERE AOB.BusinessUnitID=@BusinessUnitID AND AOB.IsDr=0 AND AOB.AccountHeadID=TT.ParentHeadID AND AOB.AccountingSessionID=@SessionID AND AOB.BreakdownType=1 AND AOB.BreakdownObjID=TT.CCID),0),
			DebitAmount=ISNULL((SELECT SUM(CCT.Amount*CCT.CurrencyConversionRate) FROM View_CostCenterTransaction AS CCT WHERE CCT.BUID=@BusinessUnitID AND CCT.IsDr=1 AND CCT.CCID=TT.CCID AND CCT.AccountHeadID=TT.ParentHeadID AND CONVERT(DATE, CONVERT(VARCHAR(12),CCT.TransactionDate,106)) BETWEEN  CONVERT(DATE, CONVERT(VARCHAR(12),@SessionStartDate,106)) AND CONVERT(DATE, CONVERT(VARCHAR(12),DATEADD(DAY,-1,@StartDate),106))),0),
			CreditAmount=ISNULL((SELECT SUM(CCT.Amount*CCT.CurrencyConversionRate) FROM View_CostCenterTransaction AS CCT WHERE CCT.BUID=@BusinessUnitID AND CCT.IsDr=0 AND CCT.CCID=TT.CCID AND CCT.AccountHeadID=TT.ParentHeadID AND CONVERT(DATE, CONVERT(VARCHAR(12),CCT.TransactionDate,106)) BETWEEN  CONVERT(DATE, CONVERT(VARCHAR(12),@SessionStartDate,106)) AND CONVERT(DATE, CONVERT(VARCHAR(12),DATEADD(DAY,-1,@StartDate),106))),0)
			FROM #TempTable AS TT
		END
		ELSE
		BEGIN
			UPDATE #TempTable
			SET DebitOpeiningValue=ISNULL((SELECT SUM(AOB.AmountInCurrency) FROM AccountOpenningBreakdown AS AOB WHERE AOB.BusinessUnitID=@BusinessUnitID AND AOB.IsDr=1 AND AOB.CurrencyID=@CurrencyID AND AOB.AccountHeadID=TT.ParentHeadID AND AOB.AccountingSessionID=@SessionID AND AOB.BreakdownType=1 AND AOB.BreakdownObjID=TT.CCID),0),
			CreditOpeiningValue=ISNULL((SELECT SUM(AOB.AmountInCurrency) FROM AccountOpenningBreakdown AS AOB WHERE AOB.BusinessUnitID=@BusinessUnitID AND AOB.IsDr=0 AND AOB.CurrencyID=@CurrencyID AND AOB.AccountHeadID=TT.ParentHeadID AND AOB.AccountingSessionID=@SessionID AND AOB.BreakdownType=1 AND AOB.BreakdownObjID=TT.CCID),0),
			DebitAmount=ISNULL((SELECT SUM(CCT.Amount*CCT.CurrencyConversionRate) FROM View_CostCenterTransaction AS CCT WHERE CCT.BUID=@BusinessUnitID AND CCT.CurrencyID=@CurrencyID AND CCT.IsDr=1 AND CCT.CCID=TT.CCID AND CCT.AccountHeadID=TT.ParentHeadID AND CONVERT(DATE, CONVERT(VARCHAR(12),CCT.TransactionDate,106)) BETWEEN  CONVERT(DATE, CONVERT(VARCHAR(12),@SessionStartDate,106)) AND CONVERT(DATE, CONVERT(VARCHAR(12),DATEADD(DAY,-1,@StartDate),106))),0),
			CreditAmount=ISNULL((SELECT SUM(CCT.Amount*CCT.CurrencyConversionRate) FROM View_CostCenterTransaction AS CCT WHERE CCT.BUID=@BusinessUnitID AND CCT.CurrencyID=@CurrencyID AND CCT.IsDr=0 AND CCT.CCID=TT.CCID AND CCT.AccountHeadID=TT.ParentHeadID AND CONVERT(DATE, CONVERT(VARCHAR(12),CCT.TransactionDate,106)) BETWEEN  CONVERT(DATE, CONVERT(VARCHAR(12),@SessionStartDate,106)) AND CONVERT(DATE, CONVERT(VARCHAR(12),DATEADD(DAY,-1,@StartDate),106))),0)
			FROM #TempTable AS TT
		END
	END
END
ELSE
BEGIN
	IF(@IsApproved=1)
	BEGIN
		IF(@CurrencyID=@BaseCurrencyID)
		BEGIN
			UPDATE #TempTable
			SET DebitOpeiningValue=ISNULL((SELECT SUM(AOB.OpenningBalance) FROM AccountOpenningBreakdown AS AOB WHERE AOB.IsDr=1 AND AOB.AccountHeadID=TT.ParentHeadID AND AOB.AccountingSessionID=@SessionID AND AOB.BreakdownType=1 AND AOB.BreakdownObjID=TT.CCID),0),
			CreditOpeiningValue=ISNULL((SELECT SUM(AOB.OpenningBalance) FROM AccountOpenningBreakdown AS AOB WHERE AOB.IsDr=0 AND AOB.AccountHeadID=TT.ParentHeadID AND AOB.AccountingSessionID=@SessionID AND AOB.BreakdownType=1 AND AOB.BreakdownObjID=TT.CCID),0),
			DebitAmount=ISNULL((SELECT SUM(CCT.Amount*CCT.CurrencyConversionRate) FROM View_CostCenterTransaction AS CCT WHERE ISNULL(CCT.ApprovedBy,0)!=0 AND  CCT.IsDr=1 AND CCT.CCID=TT.CCID AND CCT.AccountHeadID=TT.ParentHeadID AND CONVERT(DATE, CONVERT(VARCHAR(12),CCT.TransactionDate,106)) BETWEEN  CONVERT(DATE, CONVERT(VARCHAR(12),@SessionStartDate,106)) AND CONVERT(DATE, CONVERT(VARCHAR(12),DATEADD(DAY,-1,@StartDate),106))),0),
			CreditAmount=ISNULL((SELECT SUM(CCT.Amount*CCT.CurrencyConversionRate) FROM View_CostCenterTransaction AS CCT WHERE ISNULL(CCT.ApprovedBy,0)!=0 AND CCT.IsDr=0 AND CCT.CCID=TT.CCID AND CCT.AccountHeadID=TT.ParentHeadID AND CONVERT(DATE, CONVERT(VARCHAR(12),CCT.TransactionDate,106)) BETWEEN  CONVERT(DATE, CONVERT(VARCHAR(12),@SessionStartDate,106)) AND CONVERT(DATE, CONVERT(VARCHAR(12),DATEADD(DAY,-1,@StartDate),106))),0)
			FROM #TempTable AS TT
		END
		ELSE
		BEGIN
			UPDATE #TempTable
			SET DebitOpeiningValue=ISNULL((SELECT SUM(AOB.AmountInCurrency) FROM AccountOpenningBreakdown AS AOB WHERE AOB.IsDr=1 AND AOB.CurrencyID=@CurrencyID AND AOB.AccountHeadID=TT.ParentHeadID AND AOB.AccountingSessionID=@SessionID AND AOB.BreakdownType=1 AND AOB.BreakdownObjID=TT.CCID),0),
			CreditOpeiningValue=ISNULL((SELECT SUM(AOB.AmountInCurrency) FROM AccountOpenningBreakdown AS AOB WHERE AOB.IsDr=0 AND AOB.CurrencyID=@CurrencyID AND AOB.AccountHeadID=TT.ParentHeadID AND AOB.AccountingSessionID=@SessionID AND AOB.BreakdownType=1 AND AOB.BreakdownObjID=TT.CCID),0),
			DebitAmount=ISNULL((SELECT SUM(CCT.Amount*CCT.CurrencyConversionRate) FROM View_CostCenterTransaction AS CCT WHERE  ISNULL(CCT.ApprovedBy,0)!=0 AND CCT.CurrencyID=@CurrencyID AND CCT.IsDr=1 AND CCT.CCID=TT.CCID AND CCT.AccountHeadID=TT.ParentHeadID AND CONVERT(DATE, CONVERT(VARCHAR(12),CCT.TransactionDate,106)) BETWEEN  CONVERT(DATE, CONVERT(VARCHAR(12),@SessionStartDate,106)) AND CONVERT(DATE, CONVERT(VARCHAR(12),DATEADD(DAY,-1,@StartDate),106))),0),
			CreditAmount=ISNULL((SELECT SUM(CCT.Amount*CCT.CurrencyConversionRate) FROM View_CostCenterTransaction AS CCT WHERE  ISNULL(CCT.ApprovedBy,0)!=0 AND CCT.CurrencyID=@CurrencyID AND CCT.IsDr=0 AND CCT.CCID=TT.CCID AND CCT.AccountHeadID=TT.ParentHeadID AND CONVERT(DATE, CONVERT(VARCHAR(12),CCT.TransactionDate,106)) BETWEEN  CONVERT(DATE, CONVERT(VARCHAR(12),@SessionStartDate,106)) AND CONVERT(DATE, CONVERT(VARCHAR(12),DATEADD(DAY,-1,@StartDate),106))),0)
			FROM #TempTable AS TT
		END
	END
	ELSE
	BEGIN
		IF(@CurrencyID=@BaseCurrencyID)
		BEGIN
			UPDATE #TempTable
			SET DebitOpeiningValue=ISNULL((SELECT SUM(AOB.OpenningBalance) FROM AccountOpenningBreakdown AS AOB WHERE AOB.IsDr=1 AND AOB.AccountHeadID=TT.ParentHeadID AND AOB.AccountingSessionID=@SessionID AND AOB.BreakdownType=1 AND AOB.BreakdownObjID=TT.CCID),0),
			CreditOpeiningValue=ISNULL((SELECT SUM(AOB.OpenningBalance) FROM AccountOpenningBreakdown AS AOB WHERE AOB.IsDr=0 AND AOB.AccountHeadID=TT.ParentHeadID AND AOB.AccountingSessionID=@SessionID AND AOB.BreakdownType=1 AND AOB.BreakdownObjID=TT.CCID),0),
			DebitAmount=ISNULL((SELECT SUM(CCT.Amount*CCT.CurrencyConversionRate) FROM View_CostCenterTransaction AS CCT WHERE CCT.IsDr=1 AND CCT.CCID=TT.CCID AND CCT.AccountHeadID=TT.ParentHeadID AND CONVERT(DATE, CONVERT(VARCHAR(12),CCT.TransactionDate,106)) BETWEEN  CONVERT(DATE, CONVERT(VARCHAR(12),@SessionStartDate,106)) AND CONVERT(DATE, CONVERT(VARCHAR(12),DATEADD(DAY,-1,@StartDate),106))),0),
			CreditAmount=ISNULL((SELECT SUM(CCT.Amount*CCT.CurrencyConversionRate) FROM View_CostCenterTransaction AS CCT WHERE CCT.IsDr=0 AND CCT.CCID=TT.CCID AND CCT.AccountHeadID=TT.ParentHeadID AND CONVERT(DATE, CONVERT(VARCHAR(12),CCT.TransactionDate,106)) BETWEEN  CONVERT(DATE, CONVERT(VARCHAR(12),@SessionStartDate,106)) AND CONVERT(DATE, CONVERT(VARCHAR(12),DATEADD(DAY,-1,@StartDate),106))),0)
			FROM #TempTable AS TT
		END
		ELSE
		BEGIN
			UPDATE #TempTable
			SET DebitOpeiningValue=ISNULL((SELECT SUM(AOB.AmountInCurrency) FROM AccountOpenningBreakdown AS AOB WHERE AOB.IsDr=1 AND AOB.CurrencyID=@CurrencyID AND AOB.AccountHeadID=TT.ParentHeadID AND AOB.AccountingSessionID=@SessionID AND AOB.BreakdownType=1 AND AOB.BreakdownObjID=TT.CCID),0),
			CreditOpeiningValue=ISNULL((SELECT SUM(AOB.AmountInCurrency) FROM AccountOpenningBreakdown AS AOB WHERE AOB.IsDr=0 AND AOB.CurrencyID=@CurrencyID AND AOB.AccountHeadID=TT.ParentHeadID AND AOB.AccountingSessionID=@SessionID AND AOB.BreakdownType=1 AND AOB.BreakdownObjID=TT.CCID),0),
			DebitAmount=ISNULL((SELECT SUM(CCT.Amount*CCT.CurrencyConversionRate) FROM View_CostCenterTransaction AS CCT WHERE CCT.CurrencyID=@CurrencyID AND CCT.IsDr=1 AND CCT.CCID=TT.CCID AND CCT.AccountHeadID=TT.ParentHeadID AND CONVERT(DATE, CONVERT(VARCHAR(12),CCT.TransactionDate,106)) BETWEEN  CONVERT(DATE, CONVERT(VARCHAR(12),@SessionStartDate,106)) AND CONVERT(DATE, CONVERT(VARCHAR(12),DATEADD(DAY,-1,@StartDate),106))),0),
			CreditAmount=ISNULL((SELECT SUM(CCT.Amount*CCT.CurrencyConversionRate) FROM View_CostCenterTransaction AS CCT WHERE CCT.CurrencyID=@CurrencyID AND CCT.IsDr=0 AND CCT.CCID=TT.CCID AND CCT.AccountHeadID=TT.ParentHeadID AND CONVERT(DATE, CONVERT(VARCHAR(12),CCT.TransactionDate,106)) BETWEEN  CONVERT(DATE, CONVERT(VARCHAR(12),@SessionStartDate,106)) AND CONVERT(DATE, CONVERT(VARCHAR(12),DATEADD(DAY,-1,@StartDate),106))),0)
			FROM #TempTable AS TT
		END
	END
END

IF NOT EXISTS(SELECT * FROM #TempTable)
BEGIN
	INSERT INTO #TempTable (CCTID,	CCID,	CCCode, CCName,	OpeiningValue,	IsDebit,	DebitAmount,	CreditAmount,	ClosingValue,	IsDrClosing,	CurrencySymbol, VoucherID,	VoucherDate,	VoucherNo,	AccountHeadName)
					VALUES (0,		0,		'',		'',		0.00,			0,			0.00,			0.00,			0.00,			0,				'',				0,			@StartDate,		'',			'Opening Balance')
END
ELSE
BEGIN
	UPDATE #TempTable
	SET VoucherID=0,
		VoucherNo='',
		VoucherDate=@StartDate,
		AccountHeadName='Opening Balance',
		ParentHeadName=ISNULL((SELECT COA.AccountHeadName FROM View_ChartsOfAccount AS COA WHERE  COA.AccountHeadID=TT.ParentHeadID),''),
		ParentHeadCode=ISNULL((SELECT COA.AccountCode FROM View_ChartsOfAccount AS COA WHERE  COA.AccountHeadID=TT.ParentHeadID),''),
		CCName=(SELECT ACC.Name FROM ACCostCenter AS ACC WHERE ACC.ACCostCenterID=TT.CCID),
		CCCode=(SELECT ACC.Code FROM ACCostCenter AS ACC WHERE ACC.ACCostCenterID=TT.CCID)
	FROM #TempTable AS TT

	UPDATE #TempTable 
	SET ClosingValue=TT.DebitOpeiningValue-TT.CreditOpeiningValue+TT.DebitAmount-TT.CreditAmount 
	FROM #TempTable AS TT WHERE ISNULL(TT.VoucherDetailID,0)=0 AND TT.ComponentID IN (2,6)

	UPDATE #TempTable 
	SET ClosingValue=TT.CreditOpeiningValue-TT.DebitOpeiningValue-TT.DebitAmount+TT.CreditAmount 
	FROM #TempTable AS TT WHERE ISNULL(TT.VoucherDetailID,0)=0 AND TT.ComponentID NOT IN (2,6)
END



--IF(@ComponentID IN(2,6))
--BEGIN
--	SET @ClosingValue=@DebitOpeningValue-@CreditOpeningValue+@DebitAmount-@CreditAmount
--END
--ELSE
--BEGIN
--	SET @ClosingValue=@CreditOpeningValue-@DebitOpeningValue-@DebitAmount+@CreditAmount
--END

--INSERT INTO #TempTable (	CCTID,	CCID,	CCCode, CCName,	OpeiningValue,	IsDebit,	DebitAmount,	CreditAmount,	ClosingValue,	IsDrClosing,	CurrencySymbol, VoucherID,	VoucherDate,	VoucherNo,	AccountHeadName)
--					VALUES (0,		0,		'',		'',		0.00,			0,			0.00,			0.00,			@ClosingValue,	0,				'',				0,			@StartDate,		'',			'Opening Balance')

IF(@CostCenterID>0)
BEGIN
	IF(@BusinessUnitID>0)
	BEGIN
		IF(@AccountHeadID>0)
		BEGIN
			IF(@IsApproved=1)
			BEGIN
				IF(@CurrencyID=@BaseCurrencyID)
				BEGIN
					INSERT INTO #TempTable(CCTID,		CCID,		ParentHeadID,		ComponentID,							VoucherDetailID,	Description)
									SELECT CCT.CCTID,	CCT.CCID,	CCT.AccountHeadID,	dbo.GetComponentID(CCT.AccountHeadID),	CCT.VoucherDetailID,CCT.Description FROM View_CostCenterTransaction AS CCT WHERE ISNULL(CCT.ApprovedBy,0)!=0 AND CCT.BUID=@BusinessUnitID AND CCT.CCID=@CostCenterID AND CCT.AccountHeadID=@AccountHeadID AND CONVERT(DATE, CONVERT(VARCHAR(12),CCT.TransactionDate,106)) BETWEEN  CONVERT(DATE, CONVERT(VARCHAR(12),@StartDate,106)) AND CONVERT(DATE, CONVERT(VARCHAR(12),@EndDate,106)) ORDER BY CCT.AccountHeadID
				END
				ELSE
				BEGIN
					INSERT INTO #TempTable(CCTID,		CCID,		ParentHeadID,		ComponentID,							VoucherDetailID,	Description)
									SELECT CCT.CCTID,	CCT.CCID,	CCT.AccountHeadID,	dbo.GetComponentID(CCT.AccountHeadID),	CCT.VoucherDetailID,CCT.Description FROM View_CostCenterTransaction AS CCT WHERE ISNULL(CCT.ApprovedBy,0)!=0 AND CCT.BUID=@BusinessUnitID AND CCT.CurrencyID=@CurrencyID AND CCT.CCID=@CostCenterID AND CCT.AccountHeadID=@AccountHeadID AND CONVERT(DATE, CONVERT(VARCHAR(12),CCT.TransactionDate,106)) BETWEEN  CONVERT(DATE, CONVERT(VARCHAR(12),@StartDate,106)) AND CONVERT(DATE, CONVERT(VARCHAR(12),@EndDate,106)) ORDER BY CCT.AccountHeadID
				END
			END
			ELSE
			BEGIN
				IF(@CurrencyID=@BaseCurrencyID)
				BEGIN
					INSERT INTO #TempTable(CCTID,		CCID,		ParentHeadID,		ComponentID,							VoucherDetailID,	Description)
									SELECT CCT.CCTID,	CCT.CCID,	CCT.AccountHeadID,	dbo.GetComponentID(CCT.AccountHeadID),	CCT.VoucherDetailID,CCT.Description FROM View_CostCenterTransaction AS CCT WHERE CCT.BUID=@BusinessUnitID AND CCT.CCID=@CostCenterID AND CCT.AccountHeadID=@AccountHeadID AND CONVERT(DATE, CONVERT(VARCHAR(12),CCT.TransactionDate,106)) BETWEEN  CONVERT(DATE, CONVERT(VARCHAR(12),@StartDate,106)) AND CONVERT(DATE, CONVERT(VARCHAR(12),@EndDate,106)) ORDER BY CCT.AccountHeadID
				END
				ELSE
				BEGIN
					INSERT INTO #TempTable(CCTID,		CCID,		ParentHeadID,		ComponentID,							VoucherDetailID,	Description)
									SELECT CCT.CCTID,	CCT.CCID,	CCT.AccountHeadID,	dbo.GetComponentID(CCT.AccountHeadID),	CCT.VoucherDetailID,CCT.Description FROM View_CostCenterTransaction AS CCT WHERE CCT.BUID=@BusinessUnitID AND CCT.CurrencyID=@CurrencyID AND CCT.CCID=@CostCenterID AND CCT.AccountHeadID=@AccountHeadID AND CONVERT(DATE, CONVERT(VARCHAR(12),CCT.TransactionDate,106)) BETWEEN  CONVERT(DATE, CONVERT(VARCHAR(12),@StartDate,106)) AND CONVERT(DATE, CONVERT(VARCHAR(12),@EndDate,106)) ORDER BY CCT.AccountHeadID
				END
			END
		END
		ELSE
		BEGIN
			IF(@IsApproved=1)
			BEGIN
				IF(@CurrencyID=@BaseCurrencyID)
				BEGIN
					INSERT INTO #TempTable(CCTID,		CCID,		ParentHeadID,		ComponentID,							VoucherDetailID,	Description)
									SELECT CCT.CCTID,	CCT.CCID,	CCT.AccountHeadID,	dbo.GetComponentID(CCT.AccountHeadID),	CCT.VoucherDetailID,CCT.Description FROM View_CostCenterTransaction AS CCT WHERE ISNULL(CCT.ApprovedBy,0)!=0 AND CCT.BUID=@BusinessUnitID AND CCT.CCID=@CostCenterID AND CONVERT(DATE, CONVERT(VARCHAR(12),CCT.TransactionDate,106)) BETWEEN  CONVERT(DATE, CONVERT(VARCHAR(12),@StartDate,106)) AND CONVERT(DATE, CONVERT(VARCHAR(12),@EndDate,106)) ORDER BY CCT.AccountHeadID
				END
				ELSE
				BEGIN
					INSERT INTO #TempTable(CCTID,		CCID,		ParentHeadID,		ComponentID,							VoucherDetailID,	Description)
									SELECT CCT.CCTID,	CCT.CCID,	CCT.AccountHeadID,	dbo.GetComponentID(CCT.AccountHeadID),	CCT.VoucherDetailID,CCT.Description FROM View_CostCenterTransaction AS CCT WHERE ISNULL(CCT.ApprovedBy,0)!=0 AND CCT.BUID=@BusinessUnitID AND CCT.CurrencyID=@CurrencyID AND CCT.CCID=@CostCenterID AND CONVERT(DATE, CONVERT(VARCHAR(12),CCT.TransactionDate,106)) BETWEEN  CONVERT(DATE, CONVERT(VARCHAR(12),@StartDate,106)) AND CONVERT(DATE, CONVERT(VARCHAR(12),@EndDate,106)) ORDER BY CCT.AccountHeadID
				END
			END
			ELSE
			BEGIN
				IF(@CurrencyID=@BaseCurrencyID)
				BEGIN
					INSERT INTO #TempTable(CCTID,		CCID,		ParentHeadID,		ComponentID,							VoucherDetailID,	Description)
									SELECT CCT.CCTID,	CCT.CCID,	CCT.AccountHeadID,	dbo.GetComponentID(CCT.AccountHeadID),	CCT.VoucherDetailID,CCT.Description FROM View_CostCenterTransaction AS CCT WHERE CCT.BUID=@BusinessUnitID AND CCT.CCID=@CostCenterID AND CONVERT(DATE, CONVERT(VARCHAR(12),CCT.TransactionDate,106)) BETWEEN  CONVERT(DATE, CONVERT(VARCHAR(12),@StartDate,106)) AND CONVERT(DATE, CONVERT(VARCHAR(12),@EndDate,106)) ORDER BY CCT.AccountHeadID
				END
				ELSE
				BEGIN
					INSERT INTO #TempTable(CCTID,		CCID,		ParentHeadID,		ComponentID,							VoucherDetailID,	Description)
									SELECT CCT.CCTID,	CCT.CCID,	CCT.AccountHeadID,	dbo.GetComponentID(CCT.AccountHeadID),	CCT.VoucherDetailID,CCT.Description FROM View_CostCenterTransaction AS CCT WHERE CCT.BUID=@BusinessUnitID AND CCT.CurrencyID=@CurrencyID AND CCT.CCID=@CostCenterID AND CONVERT(DATE, CONVERT(VARCHAR(12),CCT.TransactionDate,106)) BETWEEN  CONVERT(DATE, CONVERT(VARCHAR(12),@StartDate,106)) AND CONVERT(DATE, CONVERT(VARCHAR(12),@EndDate,106)) ORDER BY CCT.AccountHeadID
				END
			END
		END
	END
	ELSE
	BEGIN
		IF(@AccountHeadID>0)
		BEGIN
			IF(@IsApproved=1)
			BEGIN
				IF(@CurrencyID=@BaseCurrencyID)
				BEGIN
					INSERT INTO #TempTable(CCTID,		CCID,		ParentHeadID,		ComponentID,							VoucherDetailID,	Description)
									SELECT CCT.CCTID,	CCT.CCID,	CCT.AccountHeadID,	dbo.GetComponentID(CCT.AccountHeadID),	CCT.VoucherDetailID,CCT.Description FROM View_CostCenterTransaction AS CCT WHERE ISNULL(CCT.ApprovedBy,0)!=0 AND CCT.CCID=@CostCenterID AND CCT.AccountHeadID=@AccountHeadID AND CONVERT(DATE, CONVERT(VARCHAR(12),CCT.TransactionDate,106)) BETWEEN  CONVERT(DATE, CONVERT(VARCHAR(12),@StartDate,106)) AND CONVERT(DATE, CONVERT(VARCHAR(12),@EndDate,106)) ORDER BY CCT.AccountHeadID
				END
				ELSE
				BEGIN
					INSERT INTO #TempTable(CCTID,		CCID,		ParentHeadID,		ComponentID,							VoucherDetailID,	Description)
									SELECT CCT.CCTID,	CCT.CCID,	CCT.AccountHeadID,	dbo.GetComponentID(CCT.AccountHeadID),	CCT.VoucherDetailID,CCT.Description FROM View_CostCenterTransaction AS CCT WHERE ISNULL(CCT.ApprovedBy,0)!=0 AND CCT.CurrencyID=@CurrencyID AND CCT.CCID=@CostCenterID AND CCT.AccountHeadID=@AccountHeadID AND CONVERT(DATE, CONVERT(VARCHAR(12),CCT.TransactionDate,106)) BETWEEN  CONVERT(DATE, CONVERT(VARCHAR(12),@StartDate,106)) AND CONVERT(DATE, CONVERT(VARCHAR(12),@EndDate,106)) ORDER BY CCT.AccountHeadID
				END
			END
			ELSE
			BEGIN
				IF(@CurrencyID=@BaseCurrencyID)
				BEGIN
					INSERT INTO #TempTable(CCTID,		CCID,		ParentHeadID,		ComponentID,							VoucherDetailID,	Description)
									SELECT CCT.CCTID,	CCT.CCID,	CCT.AccountHeadID,	dbo.GetComponentID(CCT.AccountHeadID),	CCT.VoucherDetailID,CCT.Description FROM View_CostCenterTransaction AS CCT WHERE CCT.CCID=@CostCenterID AND CCT.AccountHeadID=@AccountHeadID AND CONVERT(DATE, CONVERT(VARCHAR(12),CCT.TransactionDate,106)) BETWEEN  CONVERT(DATE, CONVERT(VARCHAR(12),@StartDate,106)) AND CONVERT(DATE, CONVERT(VARCHAR(12),@EndDate,106)) ORDER BY CCT.AccountHeadID
				END
				ELSE
				BEGIN
					INSERT INTO #TempTable(CCTID,		CCID,		ParentHeadID,		ComponentID,							VoucherDetailID,	Description)
									SELECT CCT.CCTID,	CCT.CCID,	CCT.AccountHeadID,	dbo.GetComponentID(CCT.AccountHeadID),	CCT.VoucherDetailID,CCT.Description FROM View_CostCenterTransaction AS CCT WHERE CCT.CurrencyID=@CurrencyID AND CCT.CCID=@CostCenterID AND CCT.AccountHeadID=@AccountHeadID AND CONVERT(DATE, CONVERT(VARCHAR(12),CCT.TransactionDate,106)) BETWEEN  CONVERT(DATE, CONVERT(VARCHAR(12),@StartDate,106)) AND CONVERT(DATE, CONVERT(VARCHAR(12),@EndDate,106)) ORDER BY CCT.AccountHeadID
				END
			END
		END
		ELSE
		BEGIN
			IF(@IsApproved=1)
			BEGIN
				IF(@CurrencyID=@BaseCurrencyID)
				BEGIN
					INSERT INTO #TempTable(CCTID,		CCID,		ParentHeadID,		ComponentID,							VoucherDetailID,	Description)
									SELECT CCT.CCTID,	CCT.CCID,	CCT.AccountHeadID,	dbo.GetComponentID(CCT.AccountHeadID),	CCT.VoucherDetailID,CCT.Description FROM View_CostCenterTransaction AS CCT WHERE ISNULL(CCT.ApprovedBy,0)!=0 AND CCT.CCID=@CostCenterID AND CONVERT(DATE, CONVERT(VARCHAR(12),CCT.TransactionDate,106)) BETWEEN  CONVERT(DATE, CONVERT(VARCHAR(12),@StartDate,106)) AND CONVERT(DATE, CONVERT(VARCHAR(12),@EndDate,106)) ORDER BY CCT.AccountHeadID
				END
				ELSE
				BEGIN
					INSERT INTO #TempTable(CCTID,		CCID,		ParentHeadID,		ComponentID,							VoucherDetailID,	Description)
									SELECT CCT.CCTID,	CCT.CCID,	CCT.AccountHeadID,	dbo.GetComponentID(CCT.AccountHeadID),	CCT.VoucherDetailID,CCT.Description FROM View_CostCenterTransaction AS CCT WHERE ISNULL(CCT.ApprovedBy,0)!=0 AND CCT.CurrencyID=@CurrencyID AND CCT.CCID=@CostCenterID AND CONVERT(DATE, CONVERT(VARCHAR(12),CCT.TransactionDate,106)) BETWEEN  CONVERT(DATE, CONVERT(VARCHAR(12),@StartDate,106)) AND CONVERT(DATE, CONVERT(VARCHAR(12),@EndDate,106)) ORDER BY CCT.AccountHeadID
				END
			END
			ELSE
			BEGIN
				IF(@CurrencyID=@BaseCurrencyID)
				BEGIN
					INSERT INTO #TempTable(CCTID,		CCID,		ParentHeadID,		ComponentID,							VoucherDetailID,	Description)
									SELECT CCT.CCTID,	CCT.CCID,	CCT.AccountHeadID,	dbo.GetComponentID(CCT.AccountHeadID),	CCT.VoucherDetailID,CCT.Description FROM View_CostCenterTransaction AS CCT WHERE CCT.CCID=@CostCenterID AND CONVERT(DATE, CONVERT(VARCHAR(12),CCT.TransactionDate,106)) BETWEEN  CONVERT(DATE, CONVERT(VARCHAR(12),@StartDate,106)) AND CONVERT(DATE, CONVERT(VARCHAR(12),@EndDate,106)) ORDER BY CCT.AccountHeadID
				END
				ELSE
				BEGIN
					INSERT INTO #TempTable(CCTID,		CCID,		ParentHeadID,		ComponentID,							VoucherDetailID,	Description)
									SELECT CCT.CCTID,	CCT.CCID,	CCT.AccountHeadID,	dbo.GetComponentID(CCT.AccountHeadID),	CCT.VoucherDetailID,CCT.Description FROM View_CostCenterTransaction AS CCT WHERE CCT.CurrencyID=@CurrencyID AND CCT.CCID=@CostCenterID AND CONVERT(DATE, CONVERT(VARCHAR(12),CCT.TransactionDate,106)) BETWEEN  CONVERT(DATE, CONVERT(VARCHAR(12),@StartDate,106)) AND CONVERT(DATE, CONVERT(VARCHAR(12),@EndDate,106)) ORDER BY CCT.AccountHeadID
				END
			END
		END
	END
END
ELSE
BEGIN
	IF(@BusinessUnitID>0)
	BEGIN
		IF(@AccountHeadID>0)
		BEGIN
			IF(@IsApproved=1)
			BEGIN
				IF(@CurrencyID=@BaseCurrencyID)
				BEGIN
					INSERT INTO #TempTable(CCTID,		CCID,		ParentHeadID,		ComponentID,							VoucherDetailID,	Description)
									SELECT CCT.CCTID,	CCT.CCID,	CCT.AccountHeadID,	dbo.GetComponentID(CCT.AccountHeadID),	CCT.VoucherDetailID,CCT.Description FROM View_CostCenterTransaction AS CCT WHERE ISNULL(CCT.ApprovedBy,0)!=0 AND CCT.BUID=@BusinessUnitID AND CCT.AccountHeadID=@AccountHeadID AND CONVERT(DATE, CONVERT(VARCHAR(12),CCT.TransactionDate,106)) BETWEEN  CONVERT(DATE, CONVERT(VARCHAR(12),@StartDate,106)) AND CONVERT(DATE, CONVERT(VARCHAR(12),@EndDate,106)) ORDER BY CCT.AccountHeadID
				END
				ELSE
				BEGIN
					INSERT INTO #TempTable(CCTID,		CCID,		ParentHeadID,		ComponentID,							VoucherDetailID,	Description)
									SELECT CCT.CCTID,	CCT.CCID,	CCT.AccountHeadID,	dbo.GetComponentID(CCT.AccountHeadID),	CCT.VoucherDetailID,CCT.Description FROM View_CostCenterTransaction AS CCT WHERE ISNULL(CCT.ApprovedBy,0)!=0 AND CCT.BUID=@BusinessUnitID AND CCT.CurrencyID=@CurrencyID AND CCT.AccountHeadID=@AccountHeadID AND CONVERT(DATE, CONVERT(VARCHAR(12),CCT.TransactionDate,106)) BETWEEN  CONVERT(DATE, CONVERT(VARCHAR(12),@StartDate,106)) AND CONVERT(DATE, CONVERT(VARCHAR(12),@EndDate,106)) ORDER BY CCT.AccountHeadID
				END
			END
			ELSE
			BEGIN
				IF(@CurrencyID=@BaseCurrencyID)
				BEGIN
					INSERT INTO #TempTable(CCTID,		CCID,		ParentHeadID,		ComponentID,							VoucherDetailID,	Description)
									SELECT CCT.CCTID,	CCT.CCID,	CCT.AccountHeadID,	dbo.GetComponentID(CCT.AccountHeadID),	CCT.VoucherDetailID,CCT.Description FROM View_CostCenterTransaction AS CCT WHERE CCT.BUID=@BusinessUnitID AND CCT.AccountHeadID=@AccountHeadID AND CONVERT(DATE, CONVERT(VARCHAR(12),CCT.TransactionDate,106)) BETWEEN  CONVERT(DATE, CONVERT(VARCHAR(12),@StartDate,106)) AND CONVERT(DATE, CONVERT(VARCHAR(12),@EndDate,106)) ORDER BY CCT.AccountHeadID
				END
				ELSE
				BEGIN
					INSERT INTO #TempTable(CCTID,		CCID,		ParentHeadID,		ComponentID,							VoucherDetailID,	Description)
									SELECT CCT.CCTID,	CCT.CCID,	CCT.AccountHeadID,	dbo.GetComponentID(CCT.AccountHeadID),	CCT.VoucherDetailID,CCT.Description FROM View_CostCenterTransaction AS CCT WHERE CCT.BUID=@BusinessUnitID AND CCT.CurrencyID=@CurrencyID AND CCT.AccountHeadID=@AccountHeadID AND CONVERT(DATE, CONVERT(VARCHAR(12),CCT.TransactionDate,106)) BETWEEN  CONVERT(DATE, CONVERT(VARCHAR(12),@StartDate,106)) AND CONVERT(DATE, CONVERT(VARCHAR(12),@EndDate,106)) ORDER BY CCT.AccountHeadID
				END
			END
		END
		ELSE
		BEGIN
			IF(@IsApproved=1)
			BEGIN
				IF(@CurrencyID=@BaseCurrencyID)
				BEGIN
					INSERT INTO #TempTable(CCTID,		CCID,		ParentHeadID,		ComponentID,							VoucherDetailID,	Description)
									SELECT CCT.CCTID,	CCT.CCID,	CCT.AccountHeadID,	dbo.GetComponentID(CCT.AccountHeadID),	CCT.VoucherDetailID,CCT.Description FROM View_CostCenterTransaction AS CCT WHERE ISNULL(CCT.ApprovedBy,0)!=0 AND CCT.BUID=@BusinessUnitID AND CONVERT(DATE, CONVERT(VARCHAR(12),CCT.TransactionDate,106)) BETWEEN  CONVERT(DATE, CONVERT(VARCHAR(12),@StartDate,106)) AND CONVERT(DATE, CONVERT(VARCHAR(12),@EndDate,106)) ORDER BY CCT.AccountHeadID
				END
				ELSE
				BEGIN
					INSERT INTO #TempTable(CCTID,		CCID,		ParentHeadID,		ComponentID,							VoucherDetailID,	Description)
									SELECT CCT.CCTID,	CCT.CCID,	CCT.AccountHeadID,	dbo.GetComponentID(CCT.AccountHeadID),	CCT.VoucherDetailID,CCT.Description FROM View_CostCenterTransaction AS CCT WHERE ISNULL(CCT.ApprovedBy,0)!=0 AND CCT.BUID=@BusinessUnitID AND CCT.CurrencyID=@CurrencyID AND CONVERT(DATE, CONVERT(VARCHAR(12),CCT.TransactionDate,106)) BETWEEN  CONVERT(DATE, CONVERT(VARCHAR(12),@StartDate,106)) AND CONVERT(DATE, CONVERT(VARCHAR(12),@EndDate,106)) ORDER BY CCT.AccountHeadID
				END
			END
			ELSE
			BEGIN
				IF(@CurrencyID=@BaseCurrencyID)
				BEGIN
					INSERT INTO #TempTable(CCTID,		CCID,		ParentHeadID,		ComponentID,							VoucherDetailID,	Description)
									SELECT CCT.CCTID,	CCT.CCID,	CCT.AccountHeadID,	dbo.GetComponentID(CCT.AccountHeadID),	CCT.VoucherDetailID,CCT.Description FROM View_CostCenterTransaction AS CCT WHERE CCT.BUID=@BusinessUnitID AND CONVERT(DATE, CONVERT(VARCHAR(12),CCT.TransactionDate,106)) BETWEEN  CONVERT(DATE, CONVERT(VARCHAR(12),@StartDate,106)) AND CONVERT(DATE, CONVERT(VARCHAR(12),@EndDate,106)) ORDER BY CCT.AccountHeadID
				END
				ELSE
				BEGIN
					INSERT INTO #TempTable(CCTID,		CCID,		ParentHeadID,		ComponentID,							VoucherDetailID,	Description)
									SELECT CCT.CCTID,	CCT.CCID,	CCT.AccountHeadID,	dbo.GetComponentID(CCT.AccountHeadID),	CCT.VoucherDetailID,CCT.Description FROM View_CostCenterTransaction AS CCT WHERE CCT.BUID=@BusinessUnitID AND CCT.CurrencyID=@CurrencyID AND CONVERT(DATE, CONVERT(VARCHAR(12),CCT.TransactionDate,106)) BETWEEN  CONVERT(DATE, CONVERT(VARCHAR(12),@StartDate,106)) AND CONVERT(DATE, CONVERT(VARCHAR(12),@EndDate,106)) ORDER BY CCT.AccountHeadID
				END
			END
		END
	END
	ELSE
	BEGIN
		IF(@AccountHeadID>0)
		BEGIN
			IF(@IsApproved=1)
			BEGIN
				IF(@CurrencyID=@BaseCurrencyID)
				BEGIN
					INSERT INTO #TempTable(CCTID,		CCID,		ParentHeadID,		ComponentID,							VoucherDetailID,	Description)
									SELECT CCT.CCTID,	CCT.CCID,	CCT.AccountHeadID,	dbo.GetComponentID(CCT.AccountHeadID),	CCT.VoucherDetailID,CCT.Description FROM View_CostCenterTransaction AS CCT WHERE ISNULL(CCT.ApprovedBy,0)!=0 AND CCT.AccountHeadID=@AccountHeadID AND CONVERT(DATE, CONVERT(VARCHAR(12),CCT.TransactionDate,106)) BETWEEN  CONVERT(DATE, CONVERT(VARCHAR(12),@StartDate,106)) AND CONVERT(DATE, CONVERT(VARCHAR(12),@EndDate,106)) ORDER BY CCT.AccountHeadID
				END
				ELSE
				BEGIN
					INSERT INTO #TempTable(CCTID,		CCID,		ParentHeadID,		ComponentID,							VoucherDetailID,	Description)
									SELECT CCT.CCTID,	CCT.CCID,	CCT.AccountHeadID,	dbo.GetComponentID(CCT.AccountHeadID),	CCT.VoucherDetailID,CCT.Description FROM View_CostCenterTransaction AS CCT WHERE ISNULL(CCT.ApprovedBy,0)!=0 AND CCT.CurrencyID=@CurrencyID AND CCT.AccountHeadID=@AccountHeadID AND CONVERT(DATE, CONVERT(VARCHAR(12),CCT.TransactionDate,106)) BETWEEN  CONVERT(DATE, CONVERT(VARCHAR(12),@StartDate,106)) AND CONVERT(DATE, CONVERT(VARCHAR(12),@EndDate,106)) ORDER BY CCT.AccountHeadID
				END
			END
			ELSE
			BEGIN
				IF(@CurrencyID=@BaseCurrencyID)
				BEGIN
					INSERT INTO #TempTable(CCTID,		CCID,		ParentHeadID,		ComponentID,							VoucherDetailID,	Description)
									SELECT CCT.CCTID,	CCT.CCID,	CCT.AccountHeadID,	dbo.GetComponentID(CCT.AccountHeadID),	CCT.VoucherDetailID,CCT.Description FROM View_CostCenterTransaction AS CCT WHERE CCT.AccountHeadID=@AccountHeadID AND CONVERT(DATE, CONVERT(VARCHAR(12),CCT.TransactionDate,106)) BETWEEN  CONVERT(DATE, CONVERT(VARCHAR(12),@StartDate,106)) AND CONVERT(DATE, CONVERT(VARCHAR(12),@EndDate,106)) ORDER BY CCT.AccountHeadID
				END
				ELSE
				BEGIN
					INSERT INTO #TempTable(CCTID,		CCID,		ParentHeadID,		ComponentID,							VoucherDetailID,	Description)
									SELECT CCT.CCTID,	CCT.CCID,	CCT.AccountHeadID,	dbo.GetComponentID(CCT.AccountHeadID),	CCT.VoucherDetailID,CCT.Description FROM View_CostCenterTransaction AS CCT WHERE CCT.CurrencyID=@CurrencyID AND CCT.AccountHeadID=@AccountHeadID AND CONVERT(DATE, CONVERT(VARCHAR(12),CCT.TransactionDate,106)) BETWEEN  CONVERT(DATE, CONVERT(VARCHAR(12),@StartDate,106)) AND CONVERT(DATE, CONVERT(VARCHAR(12),@EndDate,106)) ORDER BY CCT.AccountHeadID
				END
			END
		END
		ELSE
		BEGIN
			IF(@IsApproved=1)
			BEGIN
				IF(@CurrencyID=@BaseCurrencyID)
				BEGIN
					INSERT INTO #TempTable(CCTID,		CCID,		ParentHeadID,		ComponentID,							VoucherDetailID,	Description)
									SELECT CCT.CCTID,	CCT.CCID,	CCT.AccountHeadID,	dbo.GetComponentID(CCT.AccountHeadID),	CCT.VoucherDetailID,CCT.Description FROM View_CostCenterTransaction AS CCT WHERE ISNULL(CCT.ApprovedBy,0)!=0 AND CONVERT(DATE, CONVERT(VARCHAR(12),CCT.TransactionDate,106)) BETWEEN  CONVERT(DATE, CONVERT(VARCHAR(12),@StartDate,106)) AND CONVERT(DATE, CONVERT(VARCHAR(12),@EndDate,106)) ORDER BY CCT.AccountHeadID
				END
				ELSE
				BEGIN
					INSERT INTO #TempTable(CCTID,		CCID,		ParentHeadID,		ComponentID,							VoucherDetailID,	Description)
									SELECT CCT.CCTID,	CCT.CCID,	CCT.AccountHeadID,	dbo.GetComponentID(CCT.AccountHeadID),	CCT.VoucherDetailID,CCT.Description FROM View_CostCenterTransaction AS CCT WHERE ISNULL(ISNULL(CCT.ApprovedBy,0),0)!=0 AND CCT.CurrencyID=@CurrencyID AND CONVERT(DATE, CONVERT(VARCHAR(12),CCT.TransactionDate,106)) BETWEEN  CONVERT(DATE, CONVERT(VARCHAR(12),@StartDate,106)) AND CONVERT(DATE, CONVERT(VARCHAR(12),@EndDate,106)) ORDER BY CCT.AccountHeadID
				END
			END
			ELSE
			BEGIN
				IF(@CurrencyID=@BaseCurrencyID)
				BEGIN
					INSERT INTO #TempTable(CCTID,		CCID,		ParentHeadID,		ComponentID,							VoucherDetailID,	Description)
									SELECT CCT.CCTID,	CCT.CCID,	CCT.AccountHeadID,	dbo.GetComponentID(CCT.AccountHeadID),	CCT.VoucherDetailID,CCT.Description FROM View_CostCenterTransaction AS CCT WHERE CONVERT(DATE, CONVERT(VARCHAR(12),CCT.TransactionDate,106)) BETWEEN  CONVERT(DATE, CONVERT(VARCHAR(12),@StartDate,106)) AND CONVERT(DATE, CONVERT(VARCHAR(12),@EndDate,106)) ORDER BY CCT.AccountHeadID
				END
				ELSE
				BEGIN
					INSERT INTO #TempTable(CCTID,		CCID,		ParentHeadID,		ComponentID,							VoucherDetailID,	Description)
									SELECT CCT.CCTID,	CCT.CCID,	CCT.AccountHeadID,	dbo.GetComponentID(CCT.AccountHeadID),	CCT.VoucherDetailID,CCT.Description FROM View_CostCenterTransaction AS CCT WHERE CCT.CurrencyID=@CurrencyID AND CONVERT(DATE, CONVERT(VARCHAR(12),CCT.TransactionDate,106)) BETWEEN  CONVERT(DATE, CONVERT(VARCHAR(12),@StartDate,106)) AND CONVERT(DATE, CONVERT(VARCHAR(12),@EndDate,106)) ORDER BY CCT.AccountHeadID
				END
			END
		END
	END
END

--SELECT * FROM #TempTable

UPDATE #TempTable
SET VoucherID=(SELECT VD.VoucherID FROM View_VoucherDetail AS VD WHERE VD.VoucherDetailID=TT.VoucherDetailID),
	IsDebit=(SELECT CCT.IsDr FROM View_CostCenterTransaction AS CCT WHERE CCT.CCTID=TT.CCTID)	
FROM #TempTable AS TT WHERE ISNULL(TT.VoucherDetailID,0)!=0

IF(@CurrencyID=@BaseCurrencyID)
BEGIN
	UPDATE #TempTable
	SET DebitAmount=ISNULL((SELECT (CCT.Amount*CCT.CurrencyConversionRate) FROM CostCenterTransaction AS CCT WHERE CCT.CCTID=ISNULL(TT.CCTID,0)),0),
		CreditAmount=0.00
	FROM #TempTable AS TT WHERE TT.IsDebit=1 AND ISNULL(TT.VoucherDetailID,0)!=0

	UPDATE #TempTable
	SET DebitAmount=0.00,
		CreditAmount=(SELECT (CCT.Amount*CCT.CurrencyConversionRate) FROM CostCenterTransaction AS CCT WHERE CCT.CCTID=ISNULL(TT.CCTID,0))
	FROM #TempTable AS TT WHERE TT.IsDebit=0 AND ISNULL(TT.VoucherDetailID,0)!=0
END
ELSE
BEGIN
	UPDATE #TempTable
	SET DebitAmount=ISNULL((SELECT CCT.Amount FROM CostCenterTransaction AS CCT WHERE CCT.CCTID=TT.CCTID),0),
		CreditAmount=0.00
	FROM #TempTable AS TT WHERE TT.IsDebit=1 AND ISNULL(TT.VoucherDetailID,0)!=0

	UPDATE #TempTable
	SET DebitAmount=0.00,
		CreditAmount=(SELECT CCT.Amount FROM CostCenterTransaction AS CCT WHERE CCT.CCTID=TT.CCTID)
	FROM #TempTable AS TT WHERE TT.IsDebit=0 AND ISNULL(TT.VoucherDetailID,0)!=0
END

--SELECT * FROM #TempTable

UPDATE #TempTable
SET AccountHeadName= dbo.FN_GetsRevarseHead(TT.VoucherID, TT.IsDebit), --ISNULL((SELECT TOP(1)AccountHeadName FROM View_VoucherDetail WHERE  IsDebit!=TT.IsDebit AND  AccountHeadID<>@AccountHeadID AND  VoucherID=TT.VoucherID),''),
	ParentHeadName=ISNULL((SELECT COA.AccountHeadName FROM View_ChartsOfAccount AS COA WHERE  COA.AccountHeadID=TT.ParentHeadID),''),
	ParentHeadCode=ISNULL((SELECT COA.AccountCode FROM View_ChartsOfAccount AS COA WHERE  COA.AccountHeadID=TT.ParentHeadID),''),
	CCName=(SELECT ACC.Name FROM ACCostCenter AS ACC WHERE ACC.ACCostCenterID=TT.CCID),
	CCCode=(SELECT ACC.Code FROM ACCostCenter AS ACC WHERE ACC.ACCostCenterID=TT.CCID),
	VoucherDate=(SELECT V.VoucherDate FROM Voucher AS V WHERE V.VoucherID=TT.VoucherID),
	VoucherNo=(SELECT V.VoucherNo FROM Voucher AS V WHERE V.VoucherID=TT.VoucherID),
	Narration=(SELECT V.Narration FROM Voucher AS V WHERE V.VoucherID=TT.VoucherID),
	VoucherNarration=(SELECT VD.Narration FROM VoucherDetail AS VD WHERE VD.VoucherDetailID=TT.VoucherDetailID)
FROM #TempTable AS TT WHERE ISNULL(TT.VoucherDetailID,0)!=0


INSERT INTO #ResultTable(CCTID,		CCID,		CCCode,		CCName,		DebitOpeiningValue,		CreditOpeiningValue,	OpeiningValue,		IsDebit,		DebitAmount,	CreditAmount,		ClosingValue,		IsDrClosing,		CurrencySymbol,		VoucherID,		VoucherDate,		VoucherNo,		AccountHeadName,	ParentHeadID,	ComponentID,	ParentHeadName,		ParentHeadCode,		VoucherDetailID,	Narration,		VoucherNarration,	Description)
				  SELECT TT.CCTID,	TT.CCID,	TT.CCCode,	TT.CCName,	TT.DebitOpeiningValue,	TT.CreditOpeiningValue,	TT.OpeiningValue,	TT.IsDebit,		TT.DebitAmount,	TT.CreditAmount,	TT.ClosingValue,	TT.IsDrClosing,		TT.CurrencySymbol,	TT.VoucherID,	TT.VoucherDate,		TT.VoucherNo,	TT.AccountHeadName,	TT.ParentHeadID,TT.ComponentID,	TT.ParentHeadName,	TT.ParentHeadCode,	TT.VoucherDetailID,	TT.Narration,	TT.VoucherNarration,TT.Description FROM #TempTable  AS TT ORDER BY TT.CCID, TT.ParentHeadID, TT.VoucherDate ASC

--SELECT * FROM #TempTable  AS TT ORDER BY TT.CCID, TT.ParentHeadID, TT.VoucherDate ASC

DECLARE
@ID as int,
@Count as int,
@IsDebit as bit,
@Amount as decimal(30,17),
@CurrentBalance as decimal(30,17)

SET @ID=1--2
SET @Count=(SELECT COUNT(*) FROM #ResultTable)
--SET @CurrentBalance =@ClosingValue

--IF(@ComponentID IN(2,6))--ComponentID{Asset = 2,Laibility = 3,OwnerEquity=4,Income = 5,Expeness = 6}
WHILE(@Count>=@ID)
BEGIN
	IF(ISNULL((SELECT RT.VoucherDetailID FROM #ResultTable AS RT WHERE RT.ID=@ID),0)>0)
	BEGIN			
		IF(ISNULL((SELECT RT.ComponentID FROM #ResultTable AS RT WHERE RT.ID=@ID),0) IN (2,6))
		BEGIN
			SET @Amount=ISNULL((SELECT TT.DebitAmount-TT.CreditAmount FROM #ResultTable AS TT WHERE TT.ID=@ID),0)
		END
		ELSE
		BEGIN
			SET @Amount=ISNULL((SELECT TT.CreditAmount-TT.DebitAmount FROM #ResultTable AS TT WHERE TT.ID=@ID),0)
		END
			SET @CurrentBalance=@CurrentBalance+@Amount			
		UPDATE #ResultTable SET ClosingValue=@CurrentBalance WHERE ID=@ID
	END
	ELSE
	BEGIN
		SET @CurrentBalance=ISNULL((SELECT RT.ClosingValue FROM #ResultTable AS RT WHERE RT.ID=@ID),0)
	END
	SET @ID=@ID+1
END


SELECT * FROM #ResultTable AS RT ORDER BY RT.CCID, RT.ParentHeadID, RT.VoucherDate ASC
DROP TABLE #TempTable
DROP TABLE #ResultTable
COMMIT TRAN


GO
/****** Object:  StoredProcedure [dbo].[SP_GeneralLedger]    Script Date: 1/24/2017 1:20:53 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[SP_GeneralLedger]
(
	@AccountHeadID as int,
	@StartDate as date,
	@EndDate as date,
	@CurrencyID as int,
	@IsApproved as bit,
	@BusinessUnitID as int
	--%n, %d, %d, %n, %b, %n
)
AS
BEGIN TRAN
--DECLARE
--@AccountHeadID as int,
--@StartDate as date,
--@EndDate as date,
--@CurrencyID as int,
--@IsApproved as bit,
--@BusinessUnitID as int

--SET @AccountHeadID=476
--SET @StartDate='01 JAN 2017'
--SET @EndDate='15 JAN 2017'
--SET @CurrencyID=0
--SET @IsApproved=0
--SET @BusinessUnitID= 1

CREATE TABLE #Temp_Table(	
							ID int IDENTITY(1,1) PRIMARY KEY,
							VoucherDetailID int,
							VoucherID int,
							AccountHeadID int,
							DebitAmount decimal(30,17),
							CreditAmount decimal(30,17),							
							CurrentBalance decimal(30,17),
							Narration varchar(512),														
							VoucherNo Varchar(128),
							VoucherDate datetime,														
							VoucherNarration varchar(1024),
							RevarseHead varchar(512),							
							IsDebit bit
						)
						
CREATE NONCLUSTERED INDEX #IX_Temp2_2 ON #Temp_Table(VoucherID)
CREATE NONCLUSTERED INDEX #IX_Temp2_3 ON #Temp_Table(AccountHeadID)

CREATE TABLE #FinalTable(	
							ID int IDENTITY(1,1) PRIMARY KEY,
							VoucherDetailID int,
							VoucherID int,
							AccountHeadID int,
							DebitAmount decimal(30,17),
							CreditAmount decimal(30,17),							
							CurrentBalance decimal(30,17),
							Narration varchar(512),														
							VoucherNo Varchar(128),
							VoucherDate datetime,							
							OpenningBalance decimal(30,17),
							VoucherNarration varchar(1024),
							RevarseHead varchar(512),							
							IsDebit bit
							)
						
CREATE NONCLUSTERED INDEX #IX_Temp2_2 ON #FinalTable(VoucherID)
CREATE NONCLUSTERED INDEX #IX_Temp2_3 ON #FinalTable(AccountHeadID)

DECLARE 
@BaseCurrencyID as bit,
@ComponentType as smallint

SET @BaseCurrencyID=(SELECT BaseCurrencyID FROM Company WHERE CompanyID=1)
IF(@CurrencyID<=0)
BEGIN
	SET @CurrencyID=(SELECT BaseCurrencyID FROM Company WHERE CompanyID=1)
END

SET @ComponentType=  dbo.GetComponentID(@AccountHeadID) --Set CompanyID

INSERT INTO #Temp_Table	(VoucherID,	VoucherDate,	RevarseHead,		DebitAmount,	CreditAmount,		CurrentBalance, IsDebit)	
				SELECT   0,			@StartDate,		'Opening Balance',  TT.DebitAmount, TT.CreditAmount,	0.00,			TT.IsDebit FROM [dbo].[GetLedgerOpeningBalance](@AccountHeadID, @StartDate, @CurrencyID, @IsApproved, 1, @BusinessUnitID) AS TT



-- Get Between two date 
IF(@BusinessUnitID>0)
BEGIN
	IF(@CurrencyID=@BaseCurrencyID)
	BEGIN
		IF(@IsApproved=1)
		BEGIN
			INSERT INTO #Temp_Table	(VoucherID,		AccountHeadID,		VoucherDetailID)
							  SELECT VD.VoucherID,	VD.AccountHeadID,	VD.VoucherDetailID FROM View_VoucherDetail AS VD  WHERE VD.BUID=@BusinessUnitID AND VD.AccountHeadID =@AccountHeadID AND  ISNULL(VD.AuthorizedBy,0) !=0  AND  CONVERT(DATE,CONVERT(VARCHAR(12),VD.VoucherDate,106))  BETWEEN CONVERT(DATE,CONVERT(VARCHAR(12),@StartDate,106))  AND CONVERT(DATE,CONVERT(VARCHAR(12),@EndDate,106))
		END
		ELSE
		BEGIN
			INSERT INTO #Temp_Table	(VoucherID,		AccountHeadID,		VoucherDetailID)
							  SELECT VD.VoucherID,	VD.AccountHeadID,	VD.VoucherDetailID FROM View_VoucherDetail AS VD  WHERE  VD.BUID=@BusinessUnitID AND VD.AccountHeadID =@AccountHeadID AND  CONVERT(DATE,CONVERT(VARCHAR(12),VD.VoucherDate,106))  BETWEEN CONVERT(DATE,CONVERT(VARCHAR(12),@StartDate,106))  AND CONVERT(DATE,CONVERT(VARCHAR(12),@EndDate,106))
		END
	END
	ELSE
	BEGIN
		IF(@IsApproved=1)
		BEGIN
			INSERT INTO #Temp_Table	(VoucherID,		AccountHeadID,		VoucherDetailID)
							  SELECT VD.VoucherID,	VD.AccountHeadID,	VD.VoucherDetailID FROM View_VoucherDetail AS VD  WHERE  VD.BUID=@BusinessUnitID AND VD.AccountHeadID =@AccountHeadID AND  ISNULL(VD.AuthorizedBy,0) !=0  AND  VD.CurrencyID = @CurrencyID AND  CONVERT(DATE,CONVERT(VARCHAR(12),VD.VoucherDate,106))  BETWEEN CONVERT(DATE,CONVERT(VARCHAR(12),@StartDate,106))  AND CONVERT(DATE,CONVERT(VARCHAR(12),@EndDate,106))
		END
		ELSE
		BEGIN
			INSERT INTO #Temp_Table	(VoucherID,		AccountHeadID,		VoucherDetailID)
							  SELECT VD.VoucherID,	VD.AccountHeadID,	VD.VoucherDetailID FROM View_VoucherDetail AS VD  WHERE VD.BUID=@BusinessUnitID AND  VD.AccountHeadID =@AccountHeadID AND  VD.CurrencyID = @CurrencyID AND  CONVERT(DATE,CONVERT(VARCHAR(12),VD.VoucherDate,106))  BETWEEN CONVERT(DATE,CONVERT(VARCHAR(12),@StartDate,106))  AND CONVERT(DATE,CONVERT(VARCHAR(12),@EndDate,106))
		END
	END
END
ELSE
BEGIN
	IF(@CurrencyID=@BaseCurrencyID)
	BEGIN
		IF(@IsApproved=1)
		BEGIN
			INSERT INTO #Temp_Table	(VoucherID,		AccountHeadID,		VoucherDetailID)
							  SELECT VD.VoucherID,	VD.AccountHeadID,	VD.VoucherDetailID FROM View_VoucherDetail AS VD  WHERE  VD.AccountHeadID =@AccountHeadID AND  ISNULL(VD.AuthorizedBy,0) !=0  AND  CONVERT(DATE,CONVERT(VARCHAR(12),VD.VoucherDate,106))  BETWEEN CONVERT(DATE,CONVERT(VARCHAR(12),@StartDate,106))  AND CONVERT(DATE,CONVERT(VARCHAR(12),@EndDate,106))
		END
		ELSE
		BEGIN
			INSERT INTO #Temp_Table	(VoucherID,		AccountHeadID,		VoucherDetailID)
							  SELECT VD.VoucherID,	VD.AccountHeadID,	VD.VoucherDetailID FROM View_VoucherDetail AS VD  WHERE  VD.AccountHeadID =@AccountHeadID AND  CONVERT(DATE,CONVERT(VARCHAR(12),VD.VoucherDate,106))  BETWEEN CONVERT(DATE,CONVERT(VARCHAR(12),@StartDate,106))  AND CONVERT(DATE,CONVERT(VARCHAR(12),@EndDate,106))
		END
	END
	ELSE
	BEGIN
		IF(@IsApproved=1)
		BEGIN
			INSERT INTO #Temp_Table	(VoucherID,		AccountHeadID,		VoucherDetailID)
							  SELECT VD.VoucherID,	VD.AccountHeadID,	VD.VoucherDetailID FROM View_VoucherDetail AS VD  WHERE  VD.AccountHeadID =@AccountHeadID AND  ISNULL(VD.AuthorizedBy,0) !=0  AND  VD.CurrencyID = @CurrencyID AND  CONVERT(DATE,CONVERT(VARCHAR(12),VD.VoucherDate,106))  BETWEEN CONVERT(DATE,CONVERT(VARCHAR(12),@StartDate,106))  AND CONVERT(DATE,CONVERT(VARCHAR(12),@EndDate,106))
		END
		ELSE
		BEGIN
			INSERT INTO #Temp_Table	(VoucherID,		AccountHeadID,		VoucherDetailID)
							  SELECT VD.VoucherID,	VD.AccountHeadID,	VD.VoucherDetailID FROM View_VoucherDetail AS VD  WHERE  VD.AccountHeadID =@AccountHeadID AND  VD.CurrencyID = @CurrencyID AND  CONVERT(DATE,CONVERT(VARCHAR(12),VD.VoucherDate,106))  BETWEEN CONVERT(DATE,CONVERT(VARCHAR(12),@StartDate,106))  AND CONVERT(DATE,CONVERT(VARCHAR(12),@EndDate,106))
		END
	END
END
				  --EXEC (@SQL)	


--- Update voucher info
UPDATE #Temp_Table
SET		Narration=(SELECT TOP 1 VD.Narration FROM VoucherDetail AS VD WHERE  VD.VoucherDetailID=TT.VoucherDetailID),				
		VoucherNo=(SELECT VoucherNo FROM Voucher AS V WHERE V.VoucherID=TT.VoucherID),
		VoucherDate=(SELECT VoucherDate FROM Voucher AS V WHERE  V.VoucherID=TT.VoucherID),
		VoucherNarration=(SELECT Narration FROM Voucher AS V WHERE  V.VoucherID=TT.VoucherID)		
FROM #Temp_Table AS TT WHERE TT.VoucherID>0

IF(@CurrencyID=@BaseCurrencyID)
BEGIN
	UPDATE #Temp_Table
	SET		DebitAmount=ISNULL((SELECT SUM(VD.Amount) FROM VoucherDetail  AS VD WHERE  VD.IsDebit=1 AND VD.VoucherDetailID=TT.VoucherDetailID),0),
			CreditAmount=ISNULL((SELECT SUM(VD.Amount) FROM VoucherDetail  AS VD WHERE VD.IsDebit=0 AND VD.VoucherDetailID=TT.VoucherDetailID),0)			
	FROM #Temp_Table AS TT  WHERE TT.VoucherID>0
END
ELSE
BEGIN
	UPDATE #Temp_Table
	SET		DebitAmount=ISNULL((SELECT SUM(VD.AmountInCurrency) FROM VoucherDetail  AS VD WHERE VD.CurrencyID=@CurrencyID AND  VD.IsDebit=1 AND VD.VoucherDetailID=TT.VoucherDetailID),0),
			CreditAmount=ISNULL((SELECT SUM(VD.AmountInCurrency) FROM VoucherDetail  AS VD WHERE VD.CurrencyID=@CurrencyID AND  VD.IsDebit=0 AND VD.VoucherDetailID=TT.VoucherDetailID),0)
	FROM #Temp_Table AS TT  WHERE TT.VoucherID>0
END


---- Get Revarse Account Head --
-----If have any bank related account head muse show this. For that there is a fixed code (588) pls change it 
UPDATE #Temp_Table 
SET IsDebit=	1
FROM #Temp_Table AS TT WHERE ISNULL(TT.DebitAmount,0)>0


UPDATE #Temp_Table 
SET IsDebit=	0
FROM #Temp_Table AS TT WHERE ISNULL(TT.CreditAmount,0)>0

UPDATE #Temp_Table 
SET RevarseHead= dbo.FN_GetsRevarseHead(TT.VoucherID, TT.IsDebit) --(SELECT TOP(1) MM.CostCenterName FROM View_CostCenterTransaction AS MM WHERE MM.VoucherDetailID = (SELECT TOP(1) HH.VoucherDetailID FROM View_VoucherDetail AS HH WHERE  IsDebit!=TT.IsDebit AND  VoucherID=TT.VoucherID))
FROM #Temp_Table AS TT WHERE TT.VoucherID>0


INSERT INTO #FinalTable(VoucherDetailID,	VoucherID,		AccountHeadID,		DebitAmount,		CreditAmount,		CurrentBalance,		Narration,		VoucherNo,		VoucherDate,	OpenningBalance,	VoucherNarration,		RevarseHead,	IsDebit)
				SELECT	TT.VoucherDetailID,	TT.VoucherID,	TT.AccountHeadID,	TT.DebitAmount,		TT.CreditAmount,	TT.CurrentBalance,	TT.Narration,	TT.VoucherNo,	TT.VoucherDate,	0,					TT.VoucherNarration,	TT.RevarseHead,	TT.IsDebit FROM #Temp_Table AS TT ORDER BY TT.VoucherDate, TT.AccountHeadID ASC


DECLARE
@ID as int,
@Count as int,
@IsDebit as bit,
@DebitAmount as decimal(30,17),
@CreditAmount as decimal(30,17),
@CurrentBalance as decimal(30,17)

SET @ID=1
SET @Count=(SELECT COUNT(*) FROM #FinalTable)
SET @CurrentBalance =0.00

IF(@ComponentType IN(2,6))--ComponentID{Asset = 2,Laibility = 3,OwnerEquity=4,Income = 5,Expeness = 6}
BEGIN
	WHILE(@Count>=@ID)
	BEGIN
		SET @DebitAmount=ISNULL((SELECT TT.DebitAmount FROM #FinalTable AS TT WHERE TT.ID=@ID),0)		
		SET @CreditAmount=ISNULL((SELECT TT.CreditAmount FROM #FinalTable AS TT WHERE TT.ID=@ID),0)		
		SET @CurrentBalance=@CurrentBalance+@DebitAmount-@CreditAmount				
		UPDATE #FinalTable SET CurrentBalance=@CurrentBalance WHERE ID=@ID
		SET @ID=@ID+1
	END
END		
ELSE
BEGIN
	WHILE(@Count>=@ID)
	BEGIN
		SET @DebitAmount=ISNULL((SELECT TT.DebitAmount FROM #FinalTable AS TT WHERE TT.ID=@ID),0)	
		SET @CreditAmount=ISNULL((SELECT TT.CreditAmount FROM #FinalTable AS TT WHERE TT.ID=@ID),0)
		SET @CurrentBalance=@CurrentBalance+@CreditAmount-@DebitAmount
		UPDATE #FinalTable SET CurrentBalance=@CurrentBalance WHERE ID=@ID
		SET @ID=@ID+1
	END
END

SELECT * FROM #FinalTable ORDER BY VoucherDate ASC
DROP TABLE #Temp_Table
DROP TABLE #FinalTable
COMMIT TRAN




GO
/****** Object:  StoredProcedure [dbo].[SP_IUD_HeadDisplayConfigure]    Script Date: 1/24/2017 1:20:53 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[SP_IUD_HeadDisplayConfigure]
(
	@HeadDisplayConfigureID	as int,
	@VoucherTypeID	as int,
	@SubGroupID	as int,
	@IsDebit as bit,
	@DBUserID as int,
	@DBOperation as smallint
	--%n, %n, %n,%b, %n, %n
)		
AS
BEGIN TRAN
DECLARE 
@DBServerDateTime as datetime
SET @DBServerDateTime=Getdate()
IF(@DBOperation=1)
BEGIN		
		IF NOT EXISTS(SELECT * FROM HeadDisplayConfigure WHERE VoucherTypeID = @VoucherTypeID AND SubGroupID = @SubGroupID AND IsDebit = @IsDebit)
		BEGIN
				SET @HeadDisplayConfigureID=(SELECT ISNULL(MAX(HeadDisplayConfigureID),0)+1 FROM HeadDisplayConfigure)		
				SET @DBServerDateTime=getdate()
				INSERT INTO HeadDisplayConfigure(HeadDisplayConfigureID,	VoucherTypeID,	SubGroupID,		IsDebit,	DBUserID,	DBServerDateTime)
    									VALUES	(@HeadDisplayConfigureID,	@VoucherTypeID,	@SubGroupID,	@IsDebit,	@DBUserID,	@DBServerDateTime)
    					    				  
    			SELECT * FROM View_HeadDisplayConfigure WHERE HeadDisplayConfigureID=@HeadDisplayConfigureID
		END	
		
END

IF(@DBOperation=3)
BEGIN
	IF(@HeadDisplayConfigureID<=0)
	BEGIN
		ROLLBACK
			RAISERROR (N'Your Selected Head Display Configure Is Invalid Please Refresh and try again!!',16,1);	
		RETURN
	END		
	DELETE FROM HeadDisplayConfigure WHERE HeadDisplayConfigureID=@HeadDisplayConfigureID
END
COMMIT TRAN





GO
/****** Object:  UserDefinedFunction [dbo].[FN_GetsRevarseHead]    Script Date: 1/24/2017 1:20:53 PM ******/
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
SELECT @ReturnData = @ReturnData +  HH.RevarseHeadName + ' @ '+ HH.Currency+ ' ' + CONVERT(VARCHAR, CAST(SUM(HH.Amount) AS MONEY), 1) + ', ' FROM @TempTable AS HH GROUP BY RevarseHeadName, Currency, IsLedger ORDER BY IsLedger ASC

SET @ReturnData= SUBSTRING(@ReturnData, 0, LEN(@ReturnData))
--SELECT @ReturnData
RETURN @ReturnData
END
GO
/****** Object:  UserDefinedFunction [dbo].[GetLedgerBalance]    Script Date: 1/24/2017 1:20:53 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE FUNCTION [dbo].[GetLedgerBalance]
(
	@AccountHeadID as int,
	@BusinessUnitID as int
)
RETURNS VARCHAR(512)
AS
BEGIN
--DECLARE
--@AccountHeadID as int,
--@BusinessUnitID as int

--SET @AccountHeadID = 178
--SET @BusinessUnitID = 1

DECLARE
@StartDate as date,
@EndDate as date,
@CurrencyID as int,
@ComponentType as smallint,
@RunningSessionID as int,
@OpenningBalance as decimal(30,17),
@DebitOpenningBalance as decimal(30,17),
@CreditOpenningBalance as decimal(30,17),
@DebitTransactionAmount as decimal(30,17),
@CreditTransactionAmount as decimal(30,17),
@Result as Varchar(512)

SET @RunningSessionID = ISNULL((SELECT TOP 1 HH.AccountingSessionID FROM AccountingSession AS HH WHERE HH.YearStatus=1 AND HH.SessionType=1),0)
SET @StartDate = (SELECT HH.StartDate FROM AccountingSession AS HH WHERE HH.AccountingSessionID=@RunningSessionID)
SET @EndDate = (SELECT HH.EndDate FROM AccountingSession AS HH WHERE HH.AccountingSessionID=@RunningSessionID)
SET @CurrencyID = (SELECT HH.BaseCurrencyID FROM Company AS HH WHERE HH.CompanyID=1)
SET @ComponentType = dbo.GetComponentID(@AccountHeadID) --Set CompanyID


SET @DebitOpenningBalance=0
SET @CreditOpenningBalance=0
SET @DebitTransactionAmount=0
SET @CreditTransactionAmount=0
SET @Result =''

SET @DebitOpenningBalance=ISNULL((SELECT ISNULL(Sum(OpenningBalance),0) FROM AccountOpenning AS AO WHERE AO.IsDebit=1 AND  AO.AccountingSessionID=@RunningSessionID AND AO.AccountHeadID=@AccountHeadID AND AO.BusinessUnitID =@BusinessUnitID),0)
SET @CreditOpenningBalance=ISNULL((SELECT ISNULL(Sum(OpenningBalance),0) FROM AccountOpenning AS AO WHERE AO.IsDebit=0 AND  AO.AccountingSessionID=@RunningSessionID AND AO.AccountHeadID=@AccountHeadID AND AO.BusinessUnitID =@BusinessUnitID),0)
SET @DebitTransactionAmount=ISNULL((SELECT ISNULL(SUM(VD.Amount),0) FROM View_VoucherDetail AS VD WHERE  VD.IsDebit=1 AND VD.AccountHeadID=@AccountHeadID AND VD.BUID = @BusinessUnitID AND CONVERT(DATE,CONVERT(VARCHAR(12),VD.VoucherDate,106)) BETWEEN CONVERT(DATE,CONVERT(VARCHAR(12),@StartDate,106)) AND CONVERT(DATE,CONVERT(VARCHAR(12),@EndDate,106))),0)
SET @CreditTransactionAmount=ISNULL((SELECT ISNULL(SUM(VD.Amount),0) FROM View_VoucherDetail AS VD WHERE  VD.IsDebit=0 AND VD.AccountHeadID=@AccountHeadID AND VD.BUID = @BusinessUnitID AND CONVERT(DATE,CONVERT(VARCHAR(12),VD.VoucherDate,106)) BETWEEN CONVERT(DATE,CONVERT(VARCHAR(12),@StartDate,106)) AND CONVERT(DATE,CONVERT(VARCHAR(12),@EndDate,106))),0)
		



--ComponentID{Asset = 2,Laibility = 3,OwnerEquity=4,Income = 5,Expeness = 6}
IF(@ComponentType IN(2,6))
BEGIN
	SET @OpenningBalance = @DebitOpenningBalance - @CreditOpenningBalance + @DebitTransactionAmount - @CreditTransactionAmount
	IF(@OpenningBalance>=0)
	BEGIN	
		SET @Result =  'Dr '+CONVERT(VARCHAR, CAST(@OpenningBalance AS MONEY), 1)
	END
	ELSE
	BEGIN	
		SET @Result =  'Cr '+CONVERT(VARCHAR, CAST(((-1)*@OpenningBalance) AS MONEY), 1)
	END
END
ELSE
BEGIN
	SET @OpenningBalance =@CreditOpenningBalance-@DebitOpenningBalance-@DebitTransactionAmount+@CreditTransactionAmount	
	IF(@OpenningBalance>=0)
	BEGIN
		SET @Result =  'Cr '+CONVERT(VARCHAR, CAST(@OpenningBalance AS MONEY), 1)		
	END
	ELSE
	BEGIN
		SET @Result =  'Dr '+CONVERT(VARCHAR, CAST(((-1)*@OpenningBalance) AS MONEY), 1)		
	END
END	
--SELECT @Result
RETURN @Result
END;






GO
/****** Object:  Table [dbo].[HeadDisplayConfigure]    Script Date: 1/24/2017 1:20:53 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[HeadDisplayConfigure](
	[HeadDisplayConfigureID] [int] NOT NULL,
	[VoucherTypeID] [int] NULL,
	[DBUserID] [int] NULL,
	[DBServerDateTime] [datetime] NULL,
	[SubGroupID] [int] NULL,
	[IsDebit] [bit] NULL,
 CONSTRAINT [PK_HeadDisplayConfigure] PRIMARY KEY CLUSTERED 
(
	[HeadDisplayConfigureID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  View [dbo].[View_HeadDisplayConfigure]    Script Date: 1/24/2017 1:20:53 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


CREATE VIEW [dbo].[View_HeadDisplayConfigure]
AS
SELECT         HeadDisplayConfigure.*,  
				VoucherType.VoucherName,
			   ChartsOfAccount.AccountHeadCodeName


FROM             HeadDisplayConfigure INNER JOIN View_ChartsOfAccount AS ChartsOfAccount  ON  HeadDisplayConfigure.SubGroupID =  ChartsOfAccount.AccountHeadID
				 LEft JOIN VoucherType ON HeadDisplayConfigure.VoucherTypeID = VoucherType.VoucherTypeID


GO
