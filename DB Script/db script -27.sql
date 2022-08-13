GO
/****** Object:  View [dbo].[View_VoucherDetailReverse]    Script Date: 3/7/2017 1:05:54 PM ******/
DROP VIEW [dbo].[View_VoucherDetailReverse]
GO
/****** Object:  UserDefinedFunction [dbo].[FN_GetsReverseAccounts]    Script Date: 3/7/2017 1:05:54 PM ******/
DROP FUNCTION [dbo].[FN_GetsReverseAccounts]
GO
/****** Object:  UserDefinedFunction [dbo].[FN_GetsRevarseHead]    Script Date: 3/7/2017 1:05:54 PM ******/
DROP FUNCTION [dbo].[FN_GetsRevarseHead]
GO
/****** Object:  StoredProcedure [dbo].[SP_GeneralLedger]    Script Date: 3/7/2017 1:05:54 PM ******/
DROP PROCEDURE [dbo].[SP_GeneralLedger]
GO
/****** Object:  StoredProcedure [dbo].[SP_CostCenterLedger]    Script Date: 3/7/2017 1:05:54 PM ******/
DROP PROCEDURE [dbo].[SP_CostCenterLedger]
GO
/****** Object:  StoredProcedure [dbo].[SP_BankReconciliationPrepare]    Script Date: 3/7/2017 1:05:54 PM ******/
DROP PROCEDURE [dbo].[SP_BankReconciliationPrepare]
GO
/****** Object:  StoredProcedure [dbo].[SP_BankReconciliationPrepare]    Script Date: 3/7/2017 1:05:54 PM ******/
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
SET ReverseHead= dbo.FN_GetsRevarseHead(TT.VoucherDetailID), --dbo.FN_GetsRevarseHead(TT.VoucherID, TT.IsDebit)
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
/****** Object:  StoredProcedure [dbo].[SP_CostCenterLedger]    Script Date: 3/7/2017 1:05:54 PM ******/
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
							AccountHeadName Varchar(MAX),
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
							AccountHeadName Varchar(MAX),
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
SET AccountHeadName= dbo.FN_GetsRevarseHead(TT.VoucherDetailID), --dbo.FN_GetsRevarseHead(TT.VoucherID, TT.IsDebit)
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
/****** Object:  StoredProcedure [dbo].[SP_GeneralLedger]    Script Date: 3/7/2017 1:05:54 PM ******/
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
							RevarseHead varchar(MAX),							
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
							RevarseHead varchar(MAX),							
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
SET RevarseHead= dbo.FN_GetsRevarseHead(TT.VoucherDetailID) --dbo.FN_GetsRevarseHead(TT.VoucherID, TT.IsDebit)
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
/****** Object:  UserDefinedFunction [dbo].[FN_GetsRevarseHead]    Script Date: 3/7/2017 1:05:54 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE FUNCTION [dbo].[FN_GetsRevarseHead] 
(
	@VoucherDetailID as int
)
RETURNS VARCHAR(MAX)
AS
BEGIN
--DECLARE
--@VoucherDetailID as int
--SET @VoucherDetailID = 320

DECLARE
@ReturnData as VARCHAR(MAX)
SET @ReturnData = ''

DECLARE @TempTable AS TABLE (RevarseHeadName Varchar(512), Amount decimal(30,17), Currency Varchar(512), VoucherDetailID int, IsLedger bit)
INSERT INTO @TempTable (RevarseHeadName,			Amount,				Currency,			VoucherDetailID,			IsLedger)
				SELECT	MM.ReverseSubledgerName,	MM.ReverseAmount,	MM.CSymbol,			MM.ReverseVoucherDetailID,	0 FROM View_VoucherDetailReverse AS MM WHERE MM.VoucherDetailID = @VoucherDetailID AND ISNULL(MM.ReverseSubledgerID,0) > 0  

INSERT INTO @TempTable (RevarseHeadName,	Amount,					Currency,			VoucherDetailID,	IsLedger)
				SELECT HH.ReverseHeadName,	HH.ReverseAmount,		HH.CSymbol,			HH.VoucherDetailID, 1 FROM View_VoucherDetailReverse AS HH WHERE  HH.VoucherDetailID=@VoucherDetailID AND HH.ReverseVoucherDetailID NOT IN (SELECT MM.VoucherDetailID FROM @TempTable AS MM)

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
/****** Object:  UserDefinedFunction [dbo].[FN_GetsReverseAccounts]    Script Date: 3/7/2017 1:05:54 PM ******/
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
								ReverseAmount decimal(30,17),
								ReverseVoucherDetailID int,
								ReverseSubledgerID int,
								ReverseSubledgerName Varchar(512),
								CSymbol Varchar(512)
							)
AS
BEGIN
--DECLARE
--@VoucherDetailID as int
--SET @VoucherDetailID= 320

--DECLARE @Result_Table TABLE (
--								VoucherDetailID int,
--								ReverseHeadID int,
--								ReverseHeadCode Varchar(512),
--								ReverseHeadName Varchar(1024),
--								ReverseAmount decimal(30,17),
--								ReverseVoucherDetailID int,
--								ReverseSubledgerID int,
--								ReverseSubledgerName Varchar(512)
--							)

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
@UsedAmount as decimal(30,17),
@ReverseVoucherDetailID int,
@ReverseSubledgerID int,
@ReverseSubledgerName Varchar(512)

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
	SET @ReverseVoucherDetailID = ISNULL((SELECT HH.VoucherDetailID FROM @Temp_Table AS HH WHERE HH.ID=@ID),0)
	SET @ReverseSubledgerID = ISNULL((SELECT TOP 1 HH.CCID FROM CostCenterTransaction AS HH WHERE HH.VoucherDetailID=@ReverseVoucherDetailID),0)
	SET @ReverseSubledgerName = ISNULL((SELECT HH.Name FROM ACCostCenter AS HH WHERE HH.ACCostCenterID=@ReverseSubledgerID),'')

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
				INSERT INTO @Result_Table (VoucherDetailID,  ReverseHeadID,  ReverseAmount, ReverseVoucherDetailID,		ReverseSubledgerID,		ReverseSubledgerName)
									VALUES(@VoucherDetailID, @ReverseHeadID, @TempAmount,	@ReverseVoucherDetailID,	@ReverseSubledgerID,	@ReverseSubledgerName)
				SET @TempAmount = 0
			END
			ELSE
			BEGIN			
				INSERT INTO @Result_Table (VoucherDetailID,  ReverseHeadID,  ReverseAmount,		ReverseVoucherDetailID,		ReverseSubledgerID,		ReverseSubledgerName)
									VALUES(@VoucherDetailID, @ReverseHeadID, @ReverseAmount,	@ReverseVoucherDetailID,	@ReverseSubledgerID,	@ReverseSubledgerName)
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
	ReverseHeadName = (SELECT MM.AccountHeadName FROM ChartsOfAccount AS MM WHERE MM.AccountHeadID=HH.ReverseHeadID),
	CSymbol = (SELECT MM.CUSymbol FROM View_VoucherDetail AS MM WHERE MM.VoucherDetailID=HH.VoucherDetailID)
FROM @Result_Table AS HH

--SELECT * FROM @Temp_Table 
--SELECT * FROM @Result_Table
RETURN
END;

GO
/****** Object:  View [dbo].[View_VoucherDetailReverse]    Script Date: 3/7/2017 1:05:54 PM ******/
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
		RH.ReverseVoucherDetailID,
		RH.ReverseSubledgerID,
		RH.ReverseSubledgerName,
		RH.CSymbol,									
		Voucher.VoucherDate,
		Voucher.AuthorizedBy

FROM			VoucherDetail AS VD
CROSS APPLY		[dbo].[FN_GetsReverseAccounts] (VD.VoucherDetailID) AS RH
LEFT OUTER JOIN Voucher ON VD.VoucherID =Voucher.VoucherID




GO
