GO
/****** Object:  View [dbo].[View_VPTransaction]    Script Date: 12/1/2016 2:17:46 PM ******/
DROP VIEW [dbo].[View_VPTransaction]
GO
/****** Object:  View [dbo].[View_VProduct]    Script Date: 12/1/2016 2:17:46 PM ******/
DROP VIEW [dbo].[View_VProduct]
GO
/****** Object:  View [dbo].[View_VoucherReference]    Script Date: 12/1/2016 2:17:46 PM ******/
DROP VIEW [dbo].[View_VoucherReference]
GO
/****** Object:  View [dbo].[View_VoucherHistory]    Script Date: 12/1/2016 2:17:46 PM ******/
DROP VIEW [dbo].[View_VoucherHistory]
GO
/****** Object:  View [dbo].[View_VoucherCheque]    Script Date: 12/1/2016 2:17:46 PM ******/
DROP VIEW [dbo].[View_VoucherCheque]
GO
/****** Object:  View [dbo].[View_VoucherBatchHistory]    Script Date: 12/1/2016 2:17:46 PM ******/
DROP VIEW [dbo].[View_VoucherBatchHistory]
GO
/****** Object:  View [dbo].[View_VOReference]    Script Date: 12/1/2016 2:17:46 PM ******/
DROP VIEW [dbo].[View_VOReference]
GO
/****** Object:  View [dbo].[View_VoucherBill]    Script Date: 12/1/2016 2:17:46 PM ******/
DROP VIEW [dbo].[View_VoucherBill]
GO
/****** Object:  View [dbo].[View_VoucherBillTransaction]    Script Date: 12/1/2016 2:17:46 PM ******/
DROP VIEW [dbo].[View_VoucherBillTransaction]
GO
/****** Object:  UserDefinedFunction [dbo].[GetLedgerOpeningBalanceByBU]    Script Date: 12/1/2016 2:17:46 PM ******/
DROP FUNCTION [dbo].[GetLedgerOpeningBalanceByBU]
GO
/****** Object:  UserDefinedFunction [dbo].[GetLedgerOpeningBalance]    Script Date: 12/1/2016 2:17:46 PM ******/
DROP FUNCTION [dbo].[GetLedgerOpeningBalance]
GO
/****** Object:  UserDefinedFunction [dbo].[GetInventoryEffectedVoucher]    Script Date: 12/1/2016 2:17:46 PM ******/
DROP FUNCTION [dbo].[GetInventoryEffectedVoucher]
GO
/****** Object:  UserDefinedFunction [dbo].[GetAccountHeadByComponents]    Script Date: 12/1/2016 2:17:46 PM ******/
DROP FUNCTION [dbo].[GetAccountHeadByComponents]
GO
/****** Object:  UserDefinedFunction [dbo].[GetAccountHead]    Script Date: 12/1/2016 2:17:46 PM ******/
DROP FUNCTION [dbo].[GetAccountHead]
GO
/****** Object:  UserDefinedFunction [dbo].[FN_IsBalanceIncrease]    Script Date: 12/1/2016 2:17:46 PM ******/
DROP FUNCTION [dbo].[FN_IsBalanceIncrease]
GO
/****** Object:  UserDefinedFunction [dbo].[FN_GetVoucherNoNumericPart]    Script Date: 12/1/2016 2:17:46 PM ******/
DROP FUNCTION [dbo].[FN_GetVoucherNoNumericPart]
GO
/****** Object:  UserDefinedFunction [dbo].[FN_GetVoucherBillBalance]    Script Date: 12/1/2016 2:17:46 PM ******/
DROP FUNCTION [dbo].[FN_GetVoucherBillBalance]
GO
/****** Object:  UserDefinedFunction [dbo].[FN_GetIncomeStatementBalanceBU]    Script Date: 12/1/2016 2:17:46 PM ******/
DROP FUNCTION [dbo].[FN_GetIncomeStatementBalanceBU]
GO
/****** Object:  UserDefinedFunction [dbo].[FN_GetIncomeStatementBalance]    Script Date: 12/1/2016 2:17:46 PM ******/
DROP FUNCTION [dbo].[FN_GetIncomeStatementBalance]
GO
/****** Object:  StoredProcedure [dbo].[SP_YetToAccountHeadConfigure]    Script Date: 12/1/2016 2:17:46 PM ******/
DROP PROCEDURE [dbo].[SP_YetToAccountHeadConfigure]
GO
/****** Object:  StoredProcedure [dbo].[SP_VPTransactionSummary]    Script Date: 12/1/2016 2:17:46 PM ******/
DROP PROCEDURE [dbo].[SP_VPTransactionSummary]
GO
/****** Object:  StoredProcedure [dbo].[SP_VPTransactionLedger]    Script Date: 12/1/2016 2:17:46 PM ******/
DROP PROCEDURE [dbo].[SP_VPTransactionLedger]
GO
/****** Object:  StoredProcedure [dbo].[SP_VoucherBillLedger]    Script Date: 12/1/2016 2:17:46 PM ******/
DROP PROCEDURE [dbo].[SP_VoucherBillLedger]
GO
/****** Object:  StoredProcedure [dbo].[SP_VoucherBillBreakDown]    Script Date: 12/1/2016 2:17:46 PM ******/
DROP PROCEDURE [dbo].[SP_VoucherBillBreakDown]
GO
/****** Object:  StoredProcedure [dbo].[SP_VoucherBatchStatusUpdate]    Script Date: 12/1/2016 2:17:46 PM ******/
DROP PROCEDURE [dbo].[SP_VoucherBatchStatusUpdate]
GO
/****** Object:  StoredProcedure [dbo].[SP_Un_Approved_Voucher]    Script Date: 12/1/2016 2:17:46 PM ******/
DROP PROCEDURE [dbo].[SP_Un_Approved_Voucher]
GO
/****** Object:  StoredProcedure [dbo].[SP_TrialBalance_Categorized]    Script Date: 12/1/2016 2:17:46 PM ******/
DROP PROCEDURE [dbo].[SP_TrialBalance_Categorized]
GO
/****** Object:  StoredProcedure [dbo].[SP_TrailBalance]    Script Date: 12/1/2016 2:17:46 PM ******/
DROP PROCEDURE [dbo].[SP_TrailBalance]
GO
/****** Object:  StoredProcedure [dbo].[SP_IncomeStatement]    Script Date: 12/1/2016 2:17:46 PM ******/
DROP PROCEDURE [dbo].[SP_IncomeStatement]
GO
/****** Object:  StoredProcedure [dbo].[sp_GetCurrentAccountBalance]    Script Date: 12/1/2016 2:17:46 PM ******/
DROP PROCEDURE [dbo].[sp_GetCurrentAccountBalance]
GO
/****** Object:  StoredProcedure [dbo].[SP_GetBankAccountBalance]    Script Date: 12/1/2016 2:17:46 PM ******/
DROP PROCEDURE [dbo].[SP_GetBankAccountBalance]
GO
/****** Object:  StoredProcedure [dbo].[SP_GetAHOpeningBalance]    Script Date: 12/1/2016 2:17:46 PM ******/
DROP PROCEDURE [dbo].[SP_GetAHOpeningBalance]
GO
/****** Object:  StoredProcedure [dbo].[SP_CounterVoucherDelete]    Script Date: 12/1/2016 2:17:46 PM ******/
DROP PROCEDURE [dbo].[SP_CounterVoucherDelete]
GO
/****** Object:  StoredProcedure [dbo].[SP_CounterVoucher]    Script Date: 12/1/2016 2:17:46 PM ******/
GO
/****** Object:  StoredProcedure [dbo].[SP_CommitProfitLossAccount]    Script Date: 12/1/2016 2:17:46 PM ******/
GO
/****** Object:  StoredProcedure [dbo].[SP_CloseAccountingYear]    Script Date: 12/1/2016 2:17:46 PM ******/
DROP PROCEDURE [dbo].[SP_CloseAccountingYear]
GO
/****** Object:  StoredProcedure [dbo].[SP_ChangesInEquity]    Script Date: 12/1/2016 2:17:46 PM ******/
DROP PROCEDURE [dbo].[SP_ChangesInEquity]
GO
/****** Object:  StoredProcedure [dbo].[SP_CCOpeningBreakdown]    Script Date: 12/1/2016 2:17:46 PM ******/
DROP PROCEDURE [dbo].[SP_CCOpeningBreakdown]
GO
/****** Object:  StoredProcedure [dbo].[SP_CCAccountWiseBreakdown]    Script Date: 12/1/2016 2:17:46 PM ******/
DROP PROCEDURE [dbo].[SP_CCAccountWiseBreakdown]
GO
/****** Object:  StoredProcedure [dbo].[SP_CashFlowStatement]    Script Date: 12/1/2016 2:17:46 PM ******/
DROP PROCEDURE [dbo].[SP_CashFlowStatement]
GO
/****** Object:  StoredProcedure [dbo].[SP_BalanceSheet]    Script Date: 12/1/2016 2:17:46 PM ******/
DROP PROCEDURE [dbo].[SP_BalanceSheet]
GO
/****** Object:  StoredProcedure [dbo].[SP_AHOpeningBreakdown]    Script Date: 12/1/2016 2:17:46 PM ******/
DROP PROCEDURE [dbo].[SP_AHOpeningBreakdown]
GO
/****** Object:  StoredProcedure [dbo].[SP_AccountsBook]    Script Date: 12/1/2016 2:17:46 PM ******/
DROP PROCEDURE [dbo].[SP_AccountsBook]
GO
/****** Object:  StoredProcedure [dbo].[SP_AccountingStatementLinkUp]    Script Date: 12/1/2016 2:17:46 PM ******/
DROP PROCEDURE [dbo].[SP_AccountingStatementLinkUp]
GO
/****** Object:  StoredProcedure [dbo].[SP_AccountingStatement]    Script Date: 12/1/2016 2:17:46 PM ******/
DROP PROCEDURE [dbo].[SP_AccountingStatement]
GO
/****** Object:  StoredProcedure [dbo].[SP_AccountingSessionLockUnLock]    Script Date: 12/1/2016 2:17:46 PM ******/
DROP PROCEDURE [dbo].[SP_AccountingSessionLockUnLock]
GO
/****** Object:  StoredProcedure [dbo].[SP_AccountingActivity]    Script Date: 12/1/2016 2:17:46 PM ******/
DROP PROCEDURE [dbo].[SP_AccountingActivity]
GO
/****** Object:  StoredProcedure [dbo].[SP_AccountingActivity]    Script Date: 12/1/2016 2:17:46 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[SP_AccountingActivity]
(
	@UserID as int,
	@StartDate date,
	@EndDate date
)
AS
BEGIN TRAN
--DECLARE
--@UserID as int,
--@StartDate date,
--@EndDate date

--SET @UserID=0
--SET @StartDate ='25 FEB 2016'
--SET @EndDate ='25 FEB 2016'

CREATE TABLE #TempTable(
							ID int IDENTITY(1,1) PRIMARY KEY,
							UserID INT,
							UserName varchar(512),
							VoucherTypeID int,
							VoucherName varchar(512),
							Added int,
							Edited int,
							Approved int,
							Total INT
						)



IF(@UserID<>0)
BEGIN
	INSERT INTO #TempTable (UserID,		VoucherTypeID)
	SELECT DISTINCT			VH.UserID,	VH.VoucherTypeID FROM View_VoucherHistory AS VH WHERE VH.UserID=@UserID AND CONVERT(DATE, CONVERT(VARCHAR(12),VH.TransactionDate,106)) BETWEEN  CONVERT(DATE, CONVERT(VARCHAR(12),@StartDate,106)) AND CONVERT(DATE, CONVERT(VARCHAR(12),@EndDate,106))
END
ELSE
BEGIN
	INSERT INTO #TempTable (UserID)
	SELECT DISTINCT			VH.UserID FROM View_VoucherHistory AS VH WHERE CONVERT(DATE, CONVERT(VARCHAR(12),VH.TransactionDate,106)) BETWEEN  CONVERT(DATE, CONVERT(VARCHAR(12),@StartDate,106)) AND CONVERT(DATE, CONVERT(VARCHAR(12),@EndDate,106))
END

IF(@UserID<>0)
BEGIN
	UPDATE #TempTable
	SET Added=ISNULL((SELECT  COUNT(DISTINCT VH.VoucherID) FROM View_VoucherHistory AS VH WHERE VH.ActionType=1 AND VH.VoucherTypeID=TT.VoucherTypeID AND VH.UserID=TT.UserID AND CONVERT(DATE, CONVERT(VARCHAR(12),VH.TransactionDate,106)) BETWEEN  CONVERT(DATE, CONVERT(VARCHAR(12),@StartDate,106)) AND CONVERT(DATE, CONVERT(VARCHAR(12),@EndDate,106))),0),
		Edited=ISNULL((SELECT  COUNT(DISTINCT VH.VoucherID) FROM View_VoucherHistory AS VH WHERE VH.ActionType=2 AND VH.VoucherTypeID=TT.VoucherTypeID AND VH.UserID=TT.UserID AND CONVERT(DATE, CONVERT(VARCHAR(12),VH.TransactionDate,106)) BETWEEN  CONVERT(DATE, CONVERT(VARCHAR(12),@StartDate,106)) AND CONVERT(DATE, CONVERT(VARCHAR(12),@EndDate,106))),0),
		Approved=ISNULL((SELECT  COUNT(DISTINCT VH.VoucherID) FROM View_VoucherHistory AS VH WHERE VH.ActionType=8 AND VH.VoucherTypeID=TT.VoucherTypeID AND VH.UserID=TT.UserID AND CONVERT(DATE, CONVERT(VARCHAR(12),VH.TransactionDate,106)) BETWEEN  CONVERT(DATE, CONVERT(VARCHAR(12),@StartDate,106)) AND CONVERT(DATE, CONVERT(VARCHAR(12),@EndDate,106))),0)
	FROM #TempTable AS TT
END
ELSE
BEGIN
	UPDATE #TempTable
	SET Added=ISNULL((SELECT  COUNT(DISTINCT VH.VoucherID) FROM View_VoucherHistory AS VH WHERE VH.ActionType=1 AND VH.UserID=TT.UserID AND CONVERT(DATE, CONVERT(VARCHAR(12),VH.TransactionDate,106)) BETWEEN  CONVERT(DATE, CONVERT(VARCHAR(12),@StartDate,106)) AND CONVERT(DATE, CONVERT(VARCHAR(12),@EndDate,106))),0),
		Edited=ISNULL((SELECT  COUNT(DISTINCT VH.VoucherID) FROM View_VoucherHistory AS VH WHERE VH.ActionType=2 AND VH.UserID=TT.UserID AND CONVERT(DATE, CONVERT(VARCHAR(12),VH.TransactionDate,106)) BETWEEN  CONVERT(DATE, CONVERT(VARCHAR(12),@StartDate,106)) AND CONVERT(DATE, CONVERT(VARCHAR(12),@EndDate,106))),0),
		Approved=ISNULL((SELECT  COUNT(DISTINCT VH.VoucherID) FROM View_VoucherHistory AS VH WHERE VH.ActionType=8 AND VH.UserID=TT.UserID AND CONVERT(DATE, CONVERT(VARCHAR(12),VH.TransactionDate,106)) BETWEEN  CONVERT(DATE, CONVERT(VARCHAR(12),@StartDate,106)) AND CONVERT(DATE, CONVERT(VARCHAR(12),@EndDate,106))),0)
	FROM #TempTable AS TT
END


UPDATE #TempTable
SET UserName=(SELECT U.UserName+' ['+U.LoginID+']' FROM Users AS U WHERE U.UserID=TT.UserID),
	VoucherName=(SELECT VT.VoucherName FROM VoucherType AS VT WHERE VT.VoucherTypeID=TT.VoucherTypeID),
	Total=TT.Added+TT.Edited+TT.Approved
FROM #TempTable AS TT

SELECT * FROM #TempTable AS TT 

DROP TABLE #TempTable
COMMIT TRAN






GO
/****** Object:  StoredProcedure [dbo].[SP_AccountingSessionLockUnLock]    Script Date: 12/1/2016 2:17:46 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[SP_AccountingSessionLockUnLock]
(
	@AccountingSessionID as int,
	@LockDate as date,	
	@LockTime as datetime,
	@IsLock as bit
)
AS
BEGIN TRAN
--DECLARE 
--@AccountingSessionID as int,
--@LockTime as datetime


--SET @AccountingSessionID =10
--SET @LockTime ='02 Sep 2012 15:58:00'


DECLARE 
@SessionType as int,
@FinalLockDateTime as datetime

SET @FinalLockDateTime  = (SELECT CONVERT(char(10), @LockDate,126) + ' ' + CONVERT(VARCHAR(12), @LockTime, 114))

--YearEnd=1,HalfYearEnd=2,QuarterEnd=3,MonthEnd=4,WeekEnd=5,DayEnd=6
SET @SessionType =(SELECT SessionType FROM AccountingSession WHERE AccountingSessionID=@AccountingSessionID)
--Validation Check
IF(@SessionType=1)
BEGIN
	ROLLBACK
		RAISERROR (N'Year end session can not un lock, Selection chage and try again!!',16,1);	
	RETURN
END
--IF(@IsLock=0)
--BEGIN
--	IF(@LockTime < GETDATE())
--	BEGIN 
--		ROLLBACK
--			RAISERROR (N'You must be select post date session try again!!',16,1);	
--		RETURN
--	END
--END
UPDATE AccountingSession SET LockDateTime=@FinalLockDateTime WHERE AccountingSessionID IN (SELECT TT.AccountingSessionID FROM dbo.GetSessionByRoot(@AccountingSessionID) AS TT)
SELECT * FROM View_AccountingSession WHERE AccountingSessionID=@AccountingSessionID
COMMIT TRAN




GO
/****** Object:  StoredProcedure [dbo].[SP_AccountingStatement]    Script Date: 12/1/2016 2:17:46 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[SP_AccountingStatement]
(
	@StatementSetupID as int,
	@StartDate as date,
	@EndDate as date,
	@BUID as int	
	--%n, %d, %d, %n
)
AS
BEGIN TRAN
--DECLARE
--@StatementSetupID as int,
--@StartDate as date,
--@EndDate as date,
--@BUID as int
----EXECUTE [SP_AccountingStatement] 1, '21 Oct 2015', '10 Nov 2015'
--SET @StatementSetupID =1
--SET @StartDate='01 Jan 2015'
--SET @EndDate='20 Mar 2016'
--SET @BUID=0

DECLARE
@SessionStartDate as date,
@SessionEndDate as date,
@SessionID as int,
@SessionOpenningBalance as decimal(30,17),
@DebitTransactionAmount as decimal(30,17),
@CreditTransactionAmount as decimal(30,17)

IF EXISTS(SELECT SessionID FROM AccountingSession WHERE CONVERT(DATE,CONVERT(VARCHAR(12),StartDate,106))=CONVERT(DATE,CONVERT(VARCHAR(12),@StartDate,106)) AND SessionType=6)
BEGIN
	SET @SessionID=(SELECT SessionID FROM AccountingSession  WHERE CONVERT(DATE,CONVERT(VARCHAR(12),StartDate,106))=CONVERT(DATE,CONVERT(VARCHAR(12),@StartDate,106)) AND SessionType=6)
END
ELSE
BEGIN
	SET @SessionID=(SELECT TOP 1 SessionID FROM AccountingSession  WHERE CONVERT(DATE,CONVERT(VARCHAR(12),StartDate,106))>=CONVERT(DATE,CONVERT(VARCHAR(12),@StartDate,106)) AND SessionType=6)
	SET @StartDate=(SELECT TOP 1 StartDate FROM AccountingSession  WHERE CONVERT(DATE,CONVERT(VARCHAR(12),StartDate,106))>=CONVERT(DATE,CONVERT(VARCHAR(12),@StartDate,106)) AND SessionType=6)
END

SET @SessionStartDate=(SELECT StartDate FROM AccountingSession WHERE AccountingSessionID=@SessionID)

CREATE TABLE #Temp_OpeningTable(	
									AccountHeadID int,
									ComponentID int,
									OpenningBalance decimal(30,17),
									DebitAmount decimal(30,17),
									CreditAmount decimal(30,17),							
									ClosingBalance decimal(30,17)									
								)


INSERT INTO #Temp_OpeningTable (AccountHeadID,		OpenningBalance,	DebitAmount,	CreditAmount,	ClosingBalance)
						SELECT	LBD.AccountHeadID,	0.00,				0.00,			0.00,			0.00 FROM LedgerBreakDown AS LBD WHERE LBD.IsEffectedAccounts=1 AND LBD.ReferenceID=@StatementSetupID

IF(@BUID>0)
BEGIN
	UPDATE #Temp_OpeningTable
	SET ComponentID=dbo.GetComponentID(TT.AccountHeadID),
		OpenningBalance=ISNULL((SELECT ISNULL(OpenningBalance,0) FROM AccountOpenning AS AO WHERE AO.BusinessUnitID=@BUID AND AO.AccountingSessionID=@SessionID AND AO.AccountHeadID=TT.AccountHeadID),0),
		DebitAmount=(SELECT ISNULL(SUM(VD.Amount),0) FROM View_VoucherDetail AS VD  WHERE VD.BUID=@BUID AND VD.IsDebit=1 AND VD.AccountHeadID=TT.AccountHeadID AND VD.VoucherID IN (SELECT VoucherID FROM Voucher AS V WHERE ISNULL(V.AuthorizedBy,0)!=0 AND  CONVERT(DATE,CONVERT(VARCHAR(12),V.VoucherDate,106)) BETWEEN CONVERT(DATE,CONVERT(VARCHAR(12),@SessionStartDate,106)) AND CONVERT(DATE,CONVERT(VARCHAR(12),DATEADD(DAY,-1,@StartDate),106)))),
		CreditAmount =(SELECT ISNULL(SUM(VD.Amount),0) FROM View_VoucherDetail AS VD  WHERE VD.BUID=@BUID AND VD.IsDebit=0 AND VD.AccountHeadID=TT.AccountHeadID AND VD.VoucherID IN (SELECT VoucherID FROM Voucher AS V WHERE ISNULL(V.AuthorizedBy,0)!=0 AND  CONVERT(DATE,CONVERT(VARCHAR(12),V.VoucherDate,106)) BETWEEN CONVERT(DATE,CONVERT(VARCHAR(12),@SessionStartDate,106)) AND CONVERT(DATE,CONVERT(VARCHAR(12),DATEADD(DAY,-1,@StartDate),106))))	
	FROM #Temp_OpeningTable AS TT
END
ELSE
BEGIN
	UPDATE #Temp_OpeningTable
	SET ComponentID=dbo.GetComponentID(TT.AccountHeadID),
		OpenningBalance=ISNULL((SELECT ISNULL(OpenningBalance,0) FROM AccountOpenning AS AO WHERE AO.AccountingSessionID=@SessionID AND AO.AccountHeadID=TT.AccountHeadID),0),
		DebitAmount=(SELECT ISNULL(SUM(VD.Amount),0) FROM VoucherDetail AS VD  WHERE VD.IsDebit=1 AND VD.AccountHeadID=TT.AccountHeadID AND VD.VoucherID IN (SELECT VoucherID FROM Voucher AS V WHERE ISNULL(V.AuthorizedBy,0)!=0 AND  CONVERT(DATE,CONVERT(VARCHAR(12),V.VoucherDate,106)) BETWEEN CONVERT(DATE,CONVERT(VARCHAR(12),@SessionStartDate,106)) AND CONVERT(DATE,CONVERT(VARCHAR(12),DATEADD(DAY,-1,@StartDate),106)))),
		CreditAmount =(SELECT ISNULL(SUM(VD.Amount),0) FROM VoucherDetail AS VD  WHERE VD.IsDebit=0 AND VD.AccountHeadID=TT.AccountHeadID AND VD.VoucherID IN (SELECT VoucherID FROM Voucher AS V WHERE ISNULL(V.AuthorizedBy,0)!=0 AND  CONVERT(DATE,CONVERT(VARCHAR(12),V.VoucherDate,106)) BETWEEN CONVERT(DATE,CONVERT(VARCHAR(12),@SessionStartDate,106)) AND CONVERT(DATE,CONVERT(VARCHAR(12),DATEADD(DAY,-1,@StartDate),106))))	
	FROM #Temp_OpeningTable AS TT
END

UPDATE #Temp_OpeningTable
SET ClosingBalance=ISNULL((TT.OpenningBalance+TT.DebitAmount-TT.CreditAmount),0)
FROM #Temp_OpeningTable AS TT WHERE TT.ComponentID IN (2,6)

UPDATE #Temp_OpeningTable
SET ClosingBalance=ISNULL((TT.OpenningBalance+TT.CreditAmount-TT.DebitAmount),0)
FROM #Temp_OpeningTable AS TT WHERE TT.ComponentID NOT IN (2,6)

CREATE TABLE #Temp_Table(	
							LedgerGroupSetupID int, 
							OCSID int,  
							LedgerGroupSetupName  Varchar(512),
							IsDr bit,
							BalanceAmount decimal(30,17),
							OpeningBalance decimal(30,17)
						)
						
INSERT #Temp_Table (LedgerGroupSetupID, OCSID,  LedgerGroupSetupName, IsDr) 
			 SELECT LedgerGroupSetupID, OCSID,  LedgerGroupSetupName, IsDr  FROM LedgerGroupSetup AS TT WHERE TT.OCSID IN (SELECT BB.OperationCategorySetupID FROM OperationCategorySetup AS BB WHERE BB.StatementSetupID=@StatementSetupID)

CREATE TABLE #TR_Table (
							ID int IDENTITY(1,1) PRIMARY KEY,
							VoucherID int,
							VoucherDetailID int,
							AccountHeadID int,
							IsDr bit,
							Amount decimal(30,17)
							
						)

CREATE TABLE #FTR_Table (							
							VoucherID int,
							VoucherDetailID int,
							AccountHeadID int,
							IsDr bit,
							Amount decimal(30,17)							
						)

IF(@BUID>0)
BEGIN
	INSERT INTO #TR_Table	(VoucherID,		VoucherDetailID,		AccountHeadID,		IsDr,		Amount)
					 SELECT VD.VoucherID,	VD.VoucherDetailID,		VD.AccountHeadID,	VD.IsDebit,	VD.Amount FROM View_VoucherDetail AS VD WHERE VD.BUID=@BUID AND ISNULL(VD.AuthorizedBy,0)!=0 AND CONVERT(DATE,CONVERT(VARCHAR(12),VD.VoucherDate,106)) BETWEEN CONVERT(DATE,CONVERT(VARCHAR(12),@StartDate,106)) AND CONVERT(DATE,CONVERT(VARCHAR(12),@EndDate,106)) AND VD.AccountHeadID IN (SELECT LBD.AccountHeadID FROM LedgerBreakDown AS LBD WHERE LBD.IsEffectedAccounts=1 AND LBD.ReferenceID=@StatementSetupID) ORDER BY VD.VoucherID
END
ELSE
BEGIN
	INSERT INTO #TR_Table	(VoucherID,		VoucherDetailID,		AccountHeadID,		IsDr,		Amount)
					 SELECT VD.VoucherID,	VD.VoucherDetailID,		VD.AccountHeadID,	VD.IsDebit,	VD.Amount FROM View_VoucherDetail AS VD WHERE  ISNULL(VD.AuthorizedBy,0)!=0 AND CONVERT(DATE,CONVERT(VARCHAR(12),VD.VoucherDate,106)) BETWEEN CONVERT(DATE,CONVERT(VARCHAR(12),@StartDate,106)) AND CONVERT(DATE,CONVERT(VARCHAR(12),@EndDate,106)) AND VD.AccountHeadID IN (SELECT LBD.AccountHeadID FROM LedgerBreakDown AS LBD WHERE LBD.IsEffectedAccounts=1 AND LBD.ReferenceID=@StatementSetupID) ORDER BY VD.VoucherID
END

DECLARE
@Count as int,
@Index as int,
@VoucherID as int,
@EffectedVoucherDetailID as int,
@VoucherDetailID as int,
@AccountHeadID as int,
@IsDr as bit,
@TempIsDr as bit,
@EffectedAmount as decimal(30,17),
@Amount as decimal(30,17),
@BalanceAmount as decimal(30,17),
@TempAmount as decimal(30,17)

SET @Index=1
SET @Count=(SELECT COUNT(*) FROM #TR_Table)

WHILE (@Index<=@Count)
BEGIN
	SET @EffectedVoucherDetailID=(SELECT TT.VoucherDetailID FROM #TR_Table AS TT WHERE TT.ID=@Index)
	SET @VoucherID=(SELECT TT.VoucherID FROM #TR_Table AS TT WHERE TT.ID=@Index)
	SET @IsDr=(SELECT TT.IsDr FROM #TR_Table AS TT WHERE TT.ID=@Index)
	IF NOT EXISTS(SELECT * FROM #FTR_Table AS TT WHERE TT.VoucherID=@VoucherID AND TT.IsDr!=@IsDr)
	BEGIN
		SET @BalanceAmount=0		
		SET @EffectedAmount=(SELECT TT.Amount FROM #TR_Table AS TT WHERE TT.VoucherDetailID=@EffectedVoucherDetailID)	
		SET @BalanceAmount=@EffectedAmount

		WHILE(@BalanceAmount>0)
		BEGIN		
			IF(@BUID>0)
			BEGIN
				SET @VoucherDetailID=(SELECT TOP 1 VD.VoucherDetailID FROM View_VoucherDetail AS VD WHERE VD.BUID=@BUID AND VD.IsDebit!=@IsDr AND VD.VoucherID=@VoucherID AND VD.VoucherDetailID NOT IN (SELECT TT.VoucherDetailID FROM #FTR_Table AS TT WHERE TT.VoucherID=@VoucherID AND TT.IsDr!=@IsDr))
			END
			ELSE
			BEGIN
				SET @VoucherDetailID=(SELECT TOP 1 VD.VoucherDetailID FROM View_VoucherDetail AS VD WHERE VD.IsDebit!=@IsDr AND VD.VoucherID=@VoucherID AND VD.VoucherDetailID NOT IN (SELECT TT.VoucherDetailID FROM #FTR_Table AS TT WHERE TT.VoucherID=@VoucherID AND TT.IsDr!=@IsDr))
			END
			SET @AccountHeadID=(SELECT VD.AccountHeadID FROM View_VoucherDetail AS VD WHERE VD.VoucherDetailID=@VoucherDetailID)
			SET @TempIsDr=(SELECT VD.IsDebit FROM View_VoucherDetail AS VD WHERE VD.VoucherDetailID=@VoucherDetailID)	
			SET @Amount =(SELECT VD.Amount FROM View_VoucherDetail AS VD WHERE VD.VoucherDetailID=@VoucherDetailID)
			IF(ROUND(@BalanceAmount,2)<=ROUND(@Amount,2))
			BEGIN
				SET @TempAmount=@BalanceAmount
				SET @BalanceAmount=0
			END
			ELSE
			BEGIN
				SET @TempAmount=@Amount
				SET @BalanceAmount=@BalanceAmount-@Amount
			END

			INSERT INTO #FTR_Table (VoucherID,	VoucherDetailID,	AccountHeadID,		IsDr,		Amount)
							 VALUES(@VoucherID, @VoucherDetailID,	@AccountHeadID,		@TempIsDr,	@TempAmount)
		END
	END
	SET @Index=@Index+1
END

--SELECT *, (SELECT COA.AccountHeadName FROM ChartsOfAccount AS COA WHERE COA.AccountHeadID=TT.AccountHeadID) AS AccountHeadName, (SELECT V.VoucherNo FROM Voucher AS V WHERE V.VoucherID=TT.VoucherID) AS VoucherNo   FROM #FTR_Table AS TT
UPDATE #Temp_Table
SET BalanceAmount=ISNULL((SELECT SUM(BB.Amount) FROM #FTR_Table AS BB WHERE BB.IsDr!=TT.IsDr AND BB.AccountHeadID IN (SELECT LBD.AccountHeadID FROM LedgerBreakDown AS LBD WHERE LBD.IsEffectedAccounts=0 AND LBD.ReferenceID=TT.LedgerGroupSetupID)),0)
FROM #Temp_Table AS TT

--SELECT * FROM #Temp_OpeningTable
UPDATE #Temp_Table SET OpeningBalance=ISNULL((SELECT SUM(TT.ClosingBalance) FROM #Temp_OpeningTable AS TT),0)
SELECT * FROM #Temp_Table ORDER BY  OCSID

DROP TABLE #FTR_Table
DROP TABLE #TR_Table
DROP TABLE #Temp_Table
DROP TABLE #Temp_OpeningTable
COMMIT TRAN


GO
/****** Object:  StoredProcedure [dbo].[SP_AccountingStatementLinkUp]    Script Date: 12/1/2016 2:17:46 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[SP_AccountingStatementLinkUp]
(
	@StatementSetupID as int,
	@StartDate as date,
	@EndDate as date,
	@BUID as int,
	@LedgerGroupSetupID as int
	--%n, %d, %d, %n, %n
)
AS
BEGIN TRAN
--DECLARE
--@StatementSetupID as int,
--@StartDate as date,
--@EndDate as date,
--@BUID as int,
--@LedgerGroupSetupID as int
----EXECUTE [SP_AccountingStatement] 1, '21 Oct 2015', '10 Nov 2015'
--SET @StatementSetupID =1
--SET @StartDate='01 Jan 2015'
--SET @EndDate='19 Mar 2016'
--SET @BUID=0
--SET @LedgerGroupSetupID =2

CREATE TABLE #TR_Table (
							ID int IDENTITY(1,1) PRIMARY KEY,
							VoucherID int,
							VoucherDetailID int,
							AccountHeadID int,
							IsDr bit,
							Amount decimal(30,17)
							
						)

CREATE TABLE #FTR_Table (							
							VoucherID int,
							CashVoucherDetailID int,
							VoucherDetailID int,
							CashAccountHeadID int,
							AccountHeadID int,
							IsDr bit,
							Amount decimal(30,17)							
						)

IF(@BUID>0)
BEGIN
	INSERT INTO #TR_Table	(VoucherID,		VoucherDetailID,		AccountHeadID,		IsDr,		Amount)
					 SELECT VD.VoucherID,	VD.VoucherDetailID,		VD.AccountHeadID,	VD.IsDebit,	VD.Amount FROM View_VoucherDetail AS VD WHERE VD.BUID=@BUID AND ISNULL(VD.AuthorizedBy,0)!=0 AND CONVERT(DATE,CONVERT(VARCHAR(12),VD.VoucherDate,106)) BETWEEN CONVERT(DATE,CONVERT(VARCHAR(12),@StartDate,106)) AND CONVERT(DATE,CONVERT(VARCHAR(12),@EndDate,106)) AND VD.AccountHeadID IN (SELECT LBD.AccountHeadID FROM LedgerBreakDown AS LBD WHERE LBD.IsEffectedAccounts=1 AND LBD.ReferenceID=@StatementSetupID) ORDER BY VD.VoucherID
END
ELSE
BEGIN
	INSERT INTO #TR_Table	(VoucherID,		VoucherDetailID,		AccountHeadID,		IsDr,		Amount)
					 SELECT VD.VoucherID,	VD.VoucherDetailID,		VD.AccountHeadID,	VD.IsDebit,	VD.Amount FROM View_VoucherDetail AS VD WHERE ISNULL(VD.AuthorizedBy,0)!=0 AND CONVERT(DATE,CONVERT(VARCHAR(12),VD.VoucherDate,106)) BETWEEN CONVERT(DATE,CONVERT(VARCHAR(12),@StartDate,106)) AND CONVERT(DATE,CONVERT(VARCHAR(12),@EndDate,106)) AND VD.AccountHeadID IN (SELECT LBD.AccountHeadID FROM LedgerBreakDown AS LBD WHERE LBD.IsEffectedAccounts=1 AND LBD.ReferenceID=@StatementSetupID) ORDER BY VD.VoucherID
END






DECLARE
@Count as int,
@Index as int,
@VoucherID as int,
@VoucherDetailID as int,
@AccountHeadID as int,
@IsDr as bit,
@TempIsDr as bit,
@EffectedAmount as decimal(30,17),
@Amount as decimal(30,17),
@BalanceAmount as decimal(30,17),
@TempAmount as decimal(30,17),
@CashAccountHeadID as int,
@CashVoucherDetailID as int

SET @Index=1
SET @Count=(SELECT COUNT(*) FROM #TR_Table)

WHILE (@Index<=@Count)
BEGIN
	SET @VoucherID=(SELECT TT.VoucherID FROM #TR_Table AS TT WHERE TT.ID=@Index)
	SET @CashVoucherDetailID=(SELECT TT.VoucherDetailID FROM #TR_Table AS TT WHERE TT.ID=@Index)
	SET @CashAccountHeadID = (SELECT TT.AccountHeadID FROM #TR_Table AS TT WHERE TT.ID=@Index)	
	SET @IsDr=(SELECT TT.IsDr FROM #TR_Table AS TT WHERE TT.ID=@Index)
	IF NOT EXISTS(SELECT * FROM #FTR_Table AS TT WHERE TT.VoucherID=@VoucherID AND TT.IsDr!=@IsDr)
	BEGIN
		SET @BalanceAmount=0		
		SET @EffectedAmount=(SELECT TT.Amount FROM #TR_Table AS TT WHERE TT.VoucherDetailID=@CashVoucherDetailID)	
		SET @BalanceAmount=@EffectedAmount

		WHILE(@BalanceAmount>0)
		BEGIN		
			IF(@BUID>0)
			BEGIN
				SET @VoucherDetailID=(SELECT TOP 1 VD.VoucherDetailID FROM View_VoucherDetail AS VD WHERE VD.BUID=@BUID AND VD.IsDebit!=@IsDr AND VD.VoucherID=@VoucherID AND VD.VoucherDetailID NOT IN (SELECT TT.VoucherDetailID FROM #FTR_Table AS TT WHERE TT.VoucherID=@VoucherID AND TT.IsDr!=@IsDr))
			END
			ELSE
			BEGIN
				SET @VoucherDetailID=(SELECT TOP 1 VD.VoucherDetailID FROM View_VoucherDetail AS VD WHERE VD.IsDebit!=@IsDr AND VD.VoucherID=@VoucherID AND VD.VoucherDetailID NOT IN (SELECT TT.VoucherDetailID FROM #FTR_Table AS TT WHERE TT.VoucherID=@VoucherID AND TT.IsDr!=@IsDr))
			END
			SET @AccountHeadID=(SELECT VD.AccountHeadID FROM View_VoucherDetail AS VD WHERE VD.VoucherDetailID=@VoucherDetailID)
			SET @TempIsDr=(SELECT VD.IsDebit FROM View_VoucherDetail AS VD WHERE VD.VoucherDetailID=@VoucherDetailID)	
			SET @Amount =(SELECT VD.Amount FROM View_VoucherDetail AS VD WHERE VD.VoucherDetailID=@VoucherDetailID)
			IF(ROUND(@BalanceAmount,2)<=ROUND(@Amount,2))
			BEGIN
				SET @TempAmount=@BalanceAmount
				SET @BalanceAmount=0
			END
			ELSE
			BEGIN
				SET @TempAmount=@Amount
				SET @BalanceAmount=@BalanceAmount-@Amount
			END

			INSERT INTO #FTR_Table (VoucherID,	CashVoucherDetailID,	VoucherDetailID,	CashAccountHeadID,	AccountHeadID,		IsDr,		Amount)
							 VALUES(@VoucherID, @CashVoucherDetailID,	@VoucherDetailID,	@CashAccountHeadID,	@AccountHeadID,		@TempIsDr,	@TempAmount)
		END
	END
	SET @Index=@Index+1
END

CREATE TABLE #TempTable (
							VoucherID int,
							VoucherNo varchar(512),
							VoucherDate date,
							ApprovedBy int,
							ApprovedByName Varchar(512),
							PrepareBy int,
							PrepareByName Varchar(512),
							VoucherNarration Varchar(1024),		
							CashVoucherDetailID int,					
							VoucherDetailID int,
							CashAccountHeadID int,
							CashAccountCode Varchar(1024),
							CashAccountName Varchar(1024),
							CashSubLedgerID int,
							CashSubLedgerName Varchar(512),
							CashIsDebit  bit,
							AccountHeadID int,
							ParticularAccountCode varchar(1024),
							ParticularAccountName varchar(1024),
							ParticularSubLedgerID int,
							ParticularSubLedgerName Varchar(512),
							IsDebit bit,
							Amount decimal(30,17),
							CurrencyID int,
							CurrencySymbol varchar(512)	
						)


SET @IsDr = (SELECT TT.IsDr FROM LedgerGroupSetup AS TT WHERE TT.LedgerGroupSetupID=@LedgerGroupSetupID)
INSERT INTO #TempTable(VoucherID,		CashVoucherDetailID,	VoucherDetailID,	CashAccountHeadID,		AccountHeadID,		IsDebit,		Amount)
				SELECT HH.VoucherID,	HH.CashVoucherDetailID,	HH.VoucherDetailID, HH.CashAccountHeadID,	HH.AccountHeadID,	HH.IsDr,	HH.Amount FROM #FTR_Table AS HH WHERE HH.IsDr!=@IsDr AND HH.AccountHeadID IN (SELECT LBD.AccountHeadID FROM LedgerBreakDown AS LBD WHERE LBD.IsEffectedAccounts=0 AND LBD.ReferenceID=@LedgerGroupSetupID)

UPDATE #TempTable
SET VoucherNo=(SELECT V.VoucherNo FROM Voucher AS V WHERE V.VoucherID=TT.VoucherID),
	VoucherDate=(SELECT V.VoucherDate FROM Voucher AS V WHERE V.VoucherID=TT.VoucherID),
	ApprovedBy=(SELECT V.AuthorizedBy FROM Voucher AS V WHERE V.VoucherID=TT.VoucherID),
	PrepareBy=(SELECT V.DBUserID FROM Voucher AS V WHERE V.VoucherID=TT.VoucherID),
	VoucherNarration=(SELECT V.Narration FROM Voucher AS V WHERE V.VoucherID=TT.VoucherID),
	CashAccountCode=(SELECT COA.AccountCode FROM ChartsOfAccount AS COA WHERE COA.AccountHeadID =TT.CashAccountHeadID),
	CashAccountName=(SELECT COA.AccountHeadName FROM ChartsOfAccount AS COA WHERE COA.AccountHeadID =TT.CashAccountHeadID),
	CashSubLedgerID=ISNULL((SELECT TOP 1 CCT.CCID FROM CostCenterTransaction AS CCT WHERE CCT.VoucherDetailID =TT.CashVoucherDetailID),0),
	ParticularAccountCode=(SELECT COA.AccountCode FROM ChartsOfAccount AS COA WHERE COA.AccountHeadID =TT.AccountHeadID),
	ParticularAccountName=(SELECT COA.AccountHeadName FROM ChartsOfAccount AS COA WHERE COA.AccountHeadID =TT.AccountHeadID),
	ParticularSubLedgerID=ISNULL((SELECT TOP 1 CCT.CCID FROM CostCenterTransaction AS CCT WHERE CCT.VoucherDetailID =TT.VoucherDetailID),0),
	CurrencyID=(SELECT HH.BaseCurrencyID FROM Company AS HH WHERE HH.CompanyID=1),
	CashIsDebit = (CASE WHEN  TT.IsDebit=1  THEN 0 ELSE 1 END)
FROM #TempTable AS TT 

UPDATE #TempTable
SET ApprovedByName=(SELECT U.UserName FROM Users AS U WHERE U.UserID=TT.ApprovedBy),
	PrepareByName=(SELECT U.UserName FROM Users AS U WHERE U.UserID=TT.PrepareBy),
	CurrencySymbol=(SELECT CU.Symbol FROM Currency AS CU WHERE CU.CurrencyID=TT.CurrencyID),
	CashSubLedgerName=ISNULL((SELECT CC.Name FROM ACCostCenter AS CC WHERE CC.ACCostCenterID=TT.CashSubLedgerID),''),
	ParticularSubLedgerName=ISNULL((SELECT CC.Name FROM ACCostCenter AS CC WHERE CC.ACCostCenterID=TT.ParticularSubLedgerID),'')
FROM #TempTable AS TT 


SELECT * FROM #TempTable AS TT ORDER BY TT.VoucherDate
DROP TABLE #TempTable
DROP TABLE #FTR_Table
DROP TABLE #TR_Table
COMMIT TRAN


GO
/****** Object:  StoredProcedure [dbo].[SP_AccountsBook]    Script Date: 12/1/2016 2:17:46 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[SP_AccountsBook]
(
	@AccountsBookSetUpID as int,
	@StartDate as date,
	@EndDate as date
)
AS
BEGIN TRAN
--DECLARE
--@AccountsBookSetUpID as int,
--@StartDate as date,
--@EndDate as date
----EXEC [SP_AccountsBook]1, '21 Oct 2015', '10 Nov 2015'
--SET @AccountsBookSetUpID=1
--SET @StartDate='01 May 2016'
--SET @EndDate='28 Aug 2016'

CREATE TABLE #TempTable (
							AccountHeadID int,
							AccountCode Varchar(512),
							AccountHeadName Varchar(512),						
							ComponentType int,
							AccountType smallint,									
							OpenningBalance decimal(30,17),
							DebitAmount decimal(30,17),
							CreditAmount decimal(30,17),
							ClosingBalance decimal(30,17)
						)

CREATE TABLE #TempTable2(
							AccountHeadID int,	
							ComponentType int,						
							OpeiningValue decimal(30,17),							
							DebitAmount decimal(30,17),
							CreditAmount decimal(30,17),
							ClosingValue decimal(30,17)							
						)

INSERT INTO #TempTable	   (AccountHeadID,	AccountCode,	AccountHeadName,	AccountType)
				SELECT		AccountHeadID,	AccountCode,	AccountHeadName,	AccountType FROM dbo.ChartsOfAccount AS TT WHERE TT.AccountHeadID IN (SELECT ABD.AccountHeadID FROM AccountsBookSetupDetail AS ABD WHERE ABD.AccountsBookSetupID=@AccountsBookSetUpID)


--EnumAccountType{None = 0,Component = 1,Segment =2, Group = 3,SubGroup = 4,Ledger = 5}
INSERT INTO #TempTable2 (AccountHeadID)
			   SELECT TT.AccountHeadID FROM #TempTable AS TT



DECLARE
@SessionID as int,
@SessionStartDate as date

IF EXISTS(SELECT SessionID FROM AccountingSession  WHERE CONVERT(DATE,CONVERT(VARCHAR(12),StartDate,106))=CONVERT(DATE,CONVERT(VARCHAR(12),@StartDate,106)) AND SessionType=6)
BEGIN
	SET @SessionID=(SELECT SessionID FROM AccountingSession  WHERE CONVERT(DATE,CONVERT(VARCHAR(12),StartDate,106))=CONVERT(DATE,CONVERT(VARCHAR(12),@StartDate,106)) AND SessionType=6)
END
ELSE
BEGIN
	SET @SessionID=(SELECT TOP 1 SessionID FROM AccountingSession  WHERE CONVERT(DATE,CONVERT(VARCHAR(12),StartDate,106))>=CONVERT(DATE,CONVERT(VARCHAR(12),@StartDate,106)) AND SessionType=6)
	SET @StartDate=(SELECT TOP 1 StartDate FROM AccountingSession  WHERE CONVERT(DATE,CONVERT(VARCHAR(12),StartDate,106))>=CONVERT(DATE,CONVERT(VARCHAR(12),@StartDate,106)) AND SessionType=6)
END
SET @SessionStartDate=(SELECT StartDate FROM AccountingSession WHERE AccountingSessionID=@SessionID)

UPDATE #TempTable2
SET	ComponentType=dbo.GetComponentID(TT.AccountHeadID),
	OpeiningValue=ISNULL((SELECT TOP 1 OpenningBalance FROM AccountOpenning WHERE AccountHeadID=TT.AccountHeadID AND AccountingSessionID=@SessionID),0),
	DebitAmount=ISNULL((SELECT SUM(VD.Amount) FROM View_VoucherDetail AS VD WHERE VD.IsDebit=1 AND VD.AccountHeadID=TT.AccountHeadID AND VD.VoucherID IN(SELECT V.VoucherID FROM Voucher AS V WHERE ISNULL(V.AuthorizedBy,0)!=0 AND CONVERT(DATE,CONVERT(VARCHAR(12),V.VoucherDate,106)) BETWEEN CONVERT(DATE,CONVERT(VARCHAR(12),@SessionStartDate,106)) AND CONVERT(DATE,CONVERT(VARCHAR(12), DATEADD(DAY,-1,@StartDate),106)))),0),
	CreditAmount=ISNULL((SELECT SUM(VD.Amount) FROM View_VoucherDetail AS VD WHERE VD.IsDebit=0 AND VD.AccountHeadID=TT.AccountHeadID AND VD.VoucherID IN(SELECT V.VoucherID FROM Voucher AS V WHERE ISNULL(V.AuthorizedBy,0)!=0 AND CONVERT(DATE,CONVERT(VARCHAR(12),V.VoucherDate,106)) BETWEEN CONVERT(DATE,CONVERT(VARCHAR(12),@SessionStartDate,106)) AND CONVERT(DATE,CONVERT(VARCHAR(12), DATEADD(DAY,-1,@StartDate),106)))),0)
FROM #TempTable2 AS TT 

UPDATE #TempTable2 SET ClosingValue=TT.OpeiningValue+TT.DebitAmount-TT.CreditAmount FROM #TempTable2 AS TT WHERE TT.ComponentType IN (2,6)
UPDATE #TempTable2 SET ClosingValue=TT.OpeiningValue-TT.DebitAmount+TT.CreditAmount FROM #TempTable2 AS TT WHERE TT.ComponentType NOT IN (2,6)


UPDATE #TempTable
SET ComponentType=dbo.GetComponentID(TT.AccountHeadID),	
	AccountCode=(SELECT COA.AccountCode FROM ChartsOfAccount AS COA WHERE COA.AccountHeadID=TT.AccountHeadID),
	AccountHeadName=(SELECT COA.AccountHeadName FROM ChartsOfAccount AS COA WHERE COA.AccountHeadID=TT.AccountHeadID),
	OpenningBalance=ISNULL((SELECT BB.ClosingValue FROM #TempTable2 AS BB WHERE BB.AccountHeadID=TT.AccountHeadID),0),
	DebitAmount=ISNULL((SELECT SUM(VD.Amount) FROM View_VoucherDetail AS VD WHERE VD.IsDebit=1 AND VD.AccountHeadID=TT.AccountHeadID AND VD.VoucherID IN(SELECT V.VoucherID FROM Voucher AS V WHERE ISNULL(V.AuthorizedBy,0)!=0 AND CONVERT(DATE,CONVERT(VARCHAR(12),V.VoucherDate,106)) BETWEEN CONVERT(DATE,CONVERT(VARCHAR(12),@StartDate,106)) AND CONVERT(DATE,CONVERT(VARCHAR(12), @EndDate,106)))),0),
	CreditAmount=ISNULL((SELECT SUM(VD.Amount) FROM View_VoucherDetail AS VD WHERE VD.IsDebit=0 AND VD.AccountHeadID=TT.AccountHeadID AND VD.VoucherID IN(SELECT V.VoucherID FROM Voucher AS V WHERE ISNULL(V.AuthorizedBy,0)!=0 AND CONVERT(DATE,CONVERT(VARCHAR(12),V.VoucherDate,106)) BETWEEN CONVERT(DATE,CONVERT(VARCHAR(12),@StartDate,106)) AND CONVERT(DATE,CONVERT(VARCHAR(12), @EndDate,106)))),0)
FROM #TempTable AS TT WHERE TT.AccountType=5

UPDATE #TempTable SET ClosingBalance=TT.OpenningBalance+TT.DebitAmount-TT.CreditAmount FROM #TempTable AS TT WHERE TT.ComponentType IN (2,6)
UPDATE #TempTable SET ClosingBalance=TT.OpenningBalance-TT.DebitAmount+TT.CreditAmount FROM #TempTable AS TT WHERE TT.ComponentType NOT IN (2,6)

SELECT * FROM #TempTable
DROP TABLE #TempTable
DROP TABLE #TempTable2
COMMIT TRAN



GO
/****** Object:  StoredProcedure [dbo].[SP_AHOpeningBreakdown]    Script Date: 12/1/2016 2:17:46 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[SP_AHOpeningBreakdown]
(
	@AccountHeadID as int,
	@StartDate as date,
	@CurrencyID as int,
	@IsApproved as bit,
	@BusinessUnitID as int
	--%n, %d, %n, %b, %n
)
AS
BEGIN TRAN
--DECLARE
--@AccountHeadID as int,
--@StartDate as date,
--@CurrencyID as int,
--@IsApproved as bit,
--@BusinessUnitID as int

--SET @AccountHeadID=266
--SET @StartDate='01 FEB 2016'
--SET @CurrencyID=1
--SET @IsApproved=1
--SET @BusinessUnitID= 0

CREATE TABLE #Temp_Table(	
							AccountHeadID int,
							IsDebit bit,
							IsDrClosing bit,
							ComponentID int,
							CCID INT,
							VoucherBillID INT,
							ProductID INT,
							OrderID INT,
							BreakdownType int,
							BreakodwnID int,
							BreakdownName varchar(512),
							AccountHeadName varchar(512),														
							AccountHeadCode varchar(512),														
							CCName Varchar(128),
							CCCode Varchar(128),
							BillNo Varchar(128),
							BillDate datetime,														
							ProductName varchar(1024),
							ProductCode varchar(1024),
							OrderNo varchar(512),							
							OrderDate datetime,
							OpeningAmount decimal(30,17),
							ClosingAmount decimal(30,17),
							DebitOpeningAmount decimal(30,17),
							CreditOpeningAmount decimal(30,17),
							DebitTransactionAmount decimal(30,17),
							CreditTransactionAmount decimal(30,17),
							DebitAmount decimal(30,17),
							CreditAmount decimal(30,17),
							Debit_CCSessionAmount decimal(30,17),
							Credit_CCSessionAmount decimal(30,17),							
							Debit_CCAmount decimal(30,17),
							Credit_CCAmount decimal(30,17),							
							Debit_VBSessionAmount decimal(30,17),
							Credit_VBSessionAmount decimal(30,17),							
							Debit_VBAmount decimal(30,17),
							Credit_VBAmount decimal(30,17),							
							Debit_VPSessionAmount decimal(30,17),
							Credit_VPSessionAmount decimal(30,17),							
							Debit_VPAmount decimal(30,17),
							Credit_VPAmount decimal(30,17),							
							Debit_VOSessionAmount decimal(30,17),
							Credit_VOSessionAmount decimal(30,17),
							Debit_VOAmount decimal(30,17),
							Credit_VOAmount decimal(30,17),
							CurrentBalance decimal(30,17)
							)
			

--EnumBreakdownType
--{
--VoucherDetail = 0,
--CostCenter = 1,// BreakdownObjID will be CostCenterID, ExplanationName will be CostCenter Name,ExplanationCode will be CostCenterCode & ExplanationAmount will be User Entered Amount
--BillReference = 2, // BreakdownObjID will be BillID, ExplanationName will be BillNo & ExplanationAmount will be User Entered Amount
--ChequeReference = 3, // ExplanationName will be user entered text & ExplanationAmount will be User Entered Amount
--InventoryReference = 4, //BreakdownObjID will be ProductID, ExplanationName will be Product Name, ExplanationCode will be ProductCode, ExplanationAmount will be User Entered Amount & WorkingUnitID, Qty, UnitPrice  must be confirm   
--SubledgerBill = 5, //Only use in interface not database 
--SubledgerCheque = 6, //Only use in interface not database 
--OrderReference = 7 // BreakdownObjID will be SaleOrderID, ExplanationName will be SalesNo & ExplanationAmount will be User Entered Amount
--}
--EnumVoucherExplanationType
--{
--    VoucherDetail = 0, //Voucher detail object here Explanation data not need
--    CostCenter = 1,// ExplanationID will be CostCenterID, ExplanationName will be CostCenter Name,ExplanationCode will be CostCenterCode & ExplanationAmount will be User Entered Amount
--    BillReference = 2, // ExplanationID will be BillID, ExplanationName will be BillNo & ExplanationAmount will be User Entered Amount
--    VoucherReference = 3, // ExplanationName will be user entered text & ExplanationAmount will be User Entered Amount
--    InventoryReference = 4 //ExplanationID will be ProductID, ExplanationName will be Product Name, ExplanationCode will be ProductCode, ExplanationAmount will be User Entered Amount & WorkingUnitID, Qty, UnitPrice  must be confirm
--}

DECLARE
@SessionID as int,
@SessionStartDate as date,
@BaseCurrencyID as int,
@ComponentID as int

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

SET @BaseCurrencyID=(SELECT BaseCurrencyID FROM Company WHERE CompanyID=1)
IF(@CurrencyID<=0)
BEGIN
	SET @CurrencyID=(SELECT BaseCurrencyID FROM Company WHERE CompanyID=1)
END

print CONVERT(VARCHAR(12),@SessionStartDate,106)
print CONVERT(VARCHAR(12),@StartDate,106)
IF(@BusinessUnitID>0)
BEGIN
	IF(@IsApproved=1)
	BEGIN
		IF(@CurrencyID=@BaseCurrencyID)
		BEGIN
			INSERT INTO #Temp_Table (CCID,				AccountHeadID,		CCName,				CCCode,				BreakdownType)
			SELECT DISTINCT			AOB.BreakdownObjID,	AOB.AccountHeadID,	AOB.BreakdownName,	AOB.BreakdownCode,	1 FROM View_AccountOpenningBreakdown AS AOB WHERE AOB.AccountHeadID=@AccountHeadID AND AOB.BusinessUnitID=@BusinessUnitID AND AOB.AccountingSessionID=@SessionID AND AOB.BreakdownType=1 AND ISNULL(AOB.BreakdownObjID,0)!=0

			INSERT INTO #Temp_Table (CCID,		AccountHeadID,		CCName,				CCCode,				BreakdownType)
			SELECT DISTINCT			CCT.CCID,	CCT.AccountHeadID,	CCT.CostCenterName,	CCT.CostCenterCode,	1 FROM View_CostCenterTransaction AS CCT WHERE CCT.AccountHeadID=@AccountHeadID AND CCT.BUID=@BusinessUnitID AND ISNULL(CCT.ApprovedBy,0)!=0 AND CONVERT(DATE, CONVERT(VARCHAR(12),CCT.TransactionDate,106)) BETWEEN  CONVERT(DATE, CONVERT(VARCHAR(12),@SessionStartDate,106)) AND DATEADD(DAY,-1,CONVERT(DATE, CONVERT(VARCHAR(12),@StartDate,106))) AND CCT.CCID NOT IN (SELECT ISNULL(TT.CCID,0) FROM #Temp_Table AS TT)


			INSERT INTO #Temp_Table(VoucherBillID,		AccountHeadID,		BillNo,		BillDate,		BreakdownType)
				SELECT DISTINCT		VB.VoucherBillID,	VB.AccountHeadID,	VB.BillNo,	VB.BillDate,	2 FROM   View_VoucherBill as VB WHERE VB.AccountHeadID=@AccountHeadID AND VB.SubLedgerID=0 AND VB.RemainningBalance>0


			INSERT INTO #Temp_Table (ProductID,			AccountHeadID,		ProductName,		ProductCode,		BreakdownType)
			SELECT DISTINCT			AOB.BreakdownObjID,	AOB.AccountHeadID,	AOB.BreakdownName,	AOB.BreakdownCode,	4 FROM View_AccountOpenningBreakdown AS AOB WHERE AOB.BusinessUnitID=@BusinessUnitID AND AOB.AccountHeadID=@AccountHeadID AND AOB.AccountingSessionID=@SessionID AND AOB.BreakdownType=4 AND ISNULL(AOB.BreakdownObjID,0)!=0

			INSERT INTO #Temp_Table (ProductID,		AccountHeadID,		ProductName,		ProductCode,		BreakdownType)
			SELECT DISTINCT			VPT.ProductID,	VPT.AccountHeadID,	VPT.ProductName,	VPT.ProductCode,	4 FROM View_VPTransaction AS VPT WHERE VPT.BUID=@BusinessUnitID AND ISNULL(VPT.ApprovedBy,0)!=0  AND VPT.AccountHeadID=@AccountHeadID AND CONVERT(DATE, CONVERT(VARCHAR(12),VPT.TransactionDate,106)) BETWEEN  CONVERT(DATE, CONVERT(VARCHAR(12),@SessionStartDate,106)) AND DATEADD(DAY,-1,CONVERT(DATE, CONVERT(VARCHAR(12),@StartDate,106))) AND VPT.ProductID NOT IN (SELECT ISNULL(TT.ProductID,0) FROM #Temp_Table AS TT) AND VPT.ProductID IN (SELECT P.ProductID FROM Product AS P WHERE P.ProductType=1)


			INSERT INTO #Temp_Table (OrderID,		AccountHeadID,		OrderNo,		OrderDate,		BreakdownType)
			SELECT DISTINCT			VOR.OrderID,	VOR.AccountHeadID,	VOR.SalesNo,	VOR.SalesDate,	7 FROM View_VOReference AS VOR WHERE VOR.BUID=@BusinessUnitID AND ISNULL(VOR.ApprovedBy,0)!=0  AND VOR.AccountHeadID=@AccountHeadID AND CONVERT(DATE, CONVERT(VARCHAR(12),VOR.TransactionDate,106)) BETWEEN  CONVERT(DATE, CONVERT(VARCHAR(12),@SessionStartDate,106)) AND DATEADD(DAY,-1,CONVERT(DATE, CONVERT(VARCHAR(12),@StartDate,106))) AND VOR.OrderID IN (SELECT SO.SaleOrderID FROM SaleOrder AS SO WHERE ISNULL(SO.ApprovedBy,0)!=0)
		END
		ELSE
		BEGIN
			INSERT INTO #Temp_Table (CCID,				AccountHeadID,		CCName,				CCCode,				BreakdownType)
			SELECT DISTINCT			AOB.BreakdownObjID,	AOB.AccountHeadID,	AOB.BreakdownName,	AOB.BreakdownCode,	1 FROM View_AccountOpenningBreakdown AS AOB WHERE AOB.AccountHeadID=@AccountHeadID AND AOB.BusinessUnitID=@BusinessUnitID AND AOB.CurrencyID=@CurrencyID AND AOB.AccountingSessionID=@SessionID AND AOB.BreakdownType=1 AND ISNULL(AOB.BreakdownObjID,0)!=0

			INSERT INTO #Temp_Table (CCID,		AccountHeadID,		CCName,				CCCode,				BreakdownType)
			SELECT DISTINCT			CCT.CCID,	CCT.AccountHeadID,	CCT.CostCenterName,	CCT.CostCenterCode,	1 FROM View_CostCenterTransaction AS CCT WHERE CCT.AccountHeadID=@AccountHeadID AND CCT.BUID=@BusinessUnitID AND ISNULL(CCT.ApprovedBy,0)!=0  AND CCT.CurrencyID=@CurrencyID AND CONVERT(DATE, CONVERT(VARCHAR(12),CCT.TransactionDate,106)) BETWEEN  CONVERT(DATE, CONVERT(VARCHAR(12),@SessionStartDate,106)) AND DATEADD(DAY,-1,CONVERT(DATE, CONVERT(VARCHAR(12),@StartDate,106))) AND CCT.CCID NOT IN (SELECT ISNULL(TT.CCID,0) FROM #Temp_Table AS TT)


			INSERT INTO #Temp_Table(VoucherBillID,		AccountHeadID,		BillNo,		BillDate,		BreakdownType)
				SELECT DISTINCT		VB.VoucherBillID,	VB.AccountHeadID,	VB.BillNo,	VB.BillDate,	2 FROM   View_VoucherBill as VB WHERE VB.AccountHeadID=@AccountHeadID AND VB.SubLedgerID=0 AND VB.RemainningBalance>0


			INSERT INTO #Temp_Table (ProductID,			AccountHeadID,		ProductName,		ProductCode,		BreakdownType)
			SELECT DISTINCT			AOB.BreakdownObjID,	AOB.AccountHeadID,	AOB.BreakdownName,	AOB.BreakdownCode,	4 FROM View_AccountOpenningBreakdown AS AOB WHERE AOB.BusinessUnitID=@BusinessUnitID AND AOB.CurrencyID=@CurrencyID AND AOB.AccountHeadID=@AccountHeadID AND AOB.AccountingSessionID=@SessionID AND AOB.BreakdownType=4 AND ISNULL(AOB.BreakdownObjID,0)!=0

			INSERT INTO #Temp_Table (ProductID,		AccountHeadID,		ProductName,		ProductCode,		BreakdownType)
			SELECT DISTINCT			VPT.ProductID,	VPT.AccountHeadID,	VPT.ProductName,	VPT.ProductCode,	4 FROM View_VPTransaction AS VPT WHERE VPT.BUID=@BusinessUnitID AND ISNULL(VPT.ApprovedBy,0)!=0  AND VPT.CurrencyID=@CurrencyID AND VPT.AccountHeadID=@AccountHeadID AND CONVERT(DATE, CONVERT(VARCHAR(12),VPT.TransactionDate,106)) BETWEEN  CONVERT(DATE, CONVERT(VARCHAR(12),@SessionStartDate,106)) AND DATEADD(DAY,-1,CONVERT(DATE, CONVERT(VARCHAR(12),@StartDate,106))) AND VPT.ProductID NOT IN (SELECT ISNULL(TT.ProductID,0) FROM #Temp_Table AS TT) AND VPT.ProductID IN (SELECT P.ProductID FROM Product AS P WHERE P.ProductType=1)


			INSERT INTO #Temp_Table (OrderID,		AccountHeadID,		OrderNo,		OrderDate,		BreakdownType)
			SELECT DISTINCT			VOR.OrderID,	VOR.AccountHeadID,	VOR.SalesNo,	VOR.SalesDate,	7 FROM View_VOReference AS VOR WHERE VOR.BUID=@BusinessUnitID AND ISNULL(VOR.ApprovedBy,0)!=0  AND VOR.CurrencyID=@CurrencyID AND VOR.AccountHeadID=@AccountHeadID AND CONVERT(DATE, CONVERT(VARCHAR(12),VOR.TransactionDate,106)) BETWEEN  CONVERT(DATE, CONVERT(VARCHAR(12),@SessionStartDate,106)) AND DATEADD(DAY,-1,CONVERT(DATE, CONVERT(VARCHAR(12),@StartDate,106))) AND VOR.OrderID IN (SELECT SO.SaleOrderID FROM SaleOrder AS SO WHERE ISNULL(SO.ApprovedBy,0)!=0)
		END
	END
	ELSE
	BEGIN
		IF(@CurrencyID=@BaseCurrencyID)
		BEGIN
			INSERT INTO #Temp_Table (CCID,				AccountHeadID,		CCName,				CCCode,				BreakdownType)
			SELECT DISTINCT			AOB.BreakdownObjID,	AOB.AccountHeadID,	AOB.BreakdownName,	AOB.BreakdownCode,	1 FROM View_AccountOpenningBreakdown AS AOB WHERE AOB.AccountHeadID=@AccountHeadID AND AOB.BusinessUnitID=@BusinessUnitID AND AOB.AccountingSessionID=@SessionID AND AOB.BreakdownType=1 AND ISNULL(AOB.BreakdownObjID,0)!=0

			INSERT INTO #Temp_Table (CCID,		AccountHeadID,		CCName,				CCCode,				BreakdownType)
			SELECT DISTINCT			CCT.CCID,	CCT.AccountHeadID,	CCT.CostCenterName,	CCT.CostCenterCode,	1 FROM View_CostCenterTransaction AS CCT WHERE CCT.AccountHeadID=@AccountHeadID AND CCT.BUID=@BusinessUnitID AND CONVERT(DATE, CONVERT(VARCHAR(12),CCT.TransactionDate,106)) BETWEEN  CONVERT(DATE, CONVERT(VARCHAR(12),@SessionStartDate,106)) AND DATEADD(DAY,-1,CONVERT(DATE, CONVERT(VARCHAR(12),@StartDate,106))) AND CCT.CCID NOT IN (SELECT ISNULL(TT.CCID,0) FROM #Temp_Table AS TT)


			INSERT INTO #Temp_Table(VoucherBillID,		AccountHeadID,		BillNo,		BillDate,		BreakdownType)
				SELECT DISTINCT		VB.VoucherBillID,	VB.AccountHeadID,	VB.BillNo,	VB.BillDate,	2 FROM   View_VoucherBill as VB WHERE VB.AccountHeadID=@AccountHeadID AND VB.SubLedgerID=0 AND VB.RemainningBalance>0


			INSERT INTO #Temp_Table (ProductID,			AccountHeadID,		ProductName,		ProductCode,		BreakdownType)
			SELECT DISTINCT			AOB.BreakdownObjID,	AOB.AccountHeadID,	AOB.BreakdownName,	AOB.BreakdownCode,	4 FROM View_AccountOpenningBreakdown AS AOB WHERE AOB.BusinessUnitID=@BusinessUnitID AND AOB.AccountHeadID=@AccountHeadID AND AOB.AccountingSessionID=@SessionID AND AOB.BreakdownType=4 AND ISNULL(AOB.BreakdownObjID,0)!=0

			INSERT INTO #Temp_Table (ProductID,		AccountHeadID,		ProductName,		ProductCode,		BreakdownType)
			SELECT DISTINCT			VPT.ProductID,	VPT.AccountHeadID,	VPT.ProductName,	VPT.ProductCode,	4 FROM View_VPTransaction AS VPT WHERE VPT.BUID=@BusinessUnitID AND VPT.AccountHeadID=@AccountHeadID AND CONVERT(DATE, CONVERT(VARCHAR(12),VPT.TransactionDate,106)) BETWEEN  CONVERT(DATE, CONVERT(VARCHAR(12),@SessionStartDate,106)) AND DATEADD(DAY,-1,CONVERT(DATE, CONVERT(VARCHAR(12),@StartDate,106))) AND VPT.ProductID NOT IN (SELECT ISNULL(TT.ProductID,0) FROM #Temp_Table AS TT) AND VPT.ProductID IN (SELECT P.ProductID FROM Product AS P WHERE P.ProductType=1)


			INSERT INTO #Temp_Table (OrderID,		AccountHeadID,		OrderNo,		OrderDate,		BreakdownType)
			SELECT DISTINCT			VOR.OrderID,	VOR.AccountHeadID,	VOR.SalesNo,	VOR.SalesDate,	7 FROM View_VOReference AS VOR WHERE VOR.BUID=@BusinessUnitID AND VOR.AccountHeadID=@AccountHeadID AND CONVERT(DATE, CONVERT(VARCHAR(12),VOR.TransactionDate,106)) BETWEEN  CONVERT(DATE, CONVERT(VARCHAR(12),@SessionStartDate,106)) AND DATEADD(DAY,-1,CONVERT(DATE, CONVERT(VARCHAR(12),@StartDate,106))) AND VOR.OrderID IN (SELECT SO.SaleOrderID FROM SaleOrder AS SO WHERE ISNULL(SO.ApprovedBy,0)!=0)
		END
		ELSE
		BEGIN
			INSERT INTO #Temp_Table (CCID,				AccountHeadID,		CCName,				CCCode,				BreakdownType)
			SELECT DISTINCT			AOB.BreakdownObjID,	AOB.AccountHeadID,	AOB.BreakdownName,	AOB.BreakdownCode,	1 FROM View_AccountOpenningBreakdown AS AOB WHERE AOB.AccountHeadID=@AccountHeadID AND AOB.BusinessUnitID=@BusinessUnitID AND AOB.CurrencyID=@CurrencyID AND AOB.AccountingSessionID=@SessionID AND AOB.BreakdownType=1 AND ISNULL(AOB.BreakdownObjID,0)!=0

			INSERT INTO #Temp_Table (CCID,		AccountHeadID,		CCName,				CCCode,				BreakdownType)
			SELECT DISTINCT			CCT.CCID,	CCT.AccountHeadID,	CCT.CostCenterName,	CCT.CostCenterCode,	1 FROM View_CostCenterTransaction AS CCT WHERE CCT.AccountHeadID=@AccountHeadID AND CCT.BUID=@BusinessUnitID AND CCT.CurrencyID=@CurrencyID AND CONVERT(DATE, CONVERT(VARCHAR(12),CCT.TransactionDate,106)) BETWEEN  CONVERT(DATE, CONVERT(VARCHAR(12),@SessionStartDate,106)) AND DATEADD(DAY,-1,CONVERT(DATE, CONVERT(VARCHAR(12),@StartDate,106))) AND CCT.CCID NOT IN (SELECT ISNULL(TT.CCID,0) FROM #Temp_Table AS TT)


			INSERT INTO #Temp_Table(VoucherBillID,		AccountHeadID,		BillNo,		BillDate,		BreakdownType)
				SELECT DISTINCT		VB.VoucherBillID,	VB.AccountHeadID,	VB.BillNo,	VB.BillDate,	2 FROM   View_VoucherBill as VB WHERE VB.AccountHeadID=@AccountHeadID AND VB.SubLedgerID=0 AND VB.RemainningBalance>0


			INSERT INTO #Temp_Table (ProductID,			AccountHeadID,		ProductName,		ProductCode,		BreakdownType)
			SELECT DISTINCT			AOB.BreakdownObjID,	AOB.AccountHeadID,	AOB.BreakdownName,	AOB.BreakdownCode,	4 FROM View_AccountOpenningBreakdown AS AOB WHERE AOB.BusinessUnitID=@BusinessUnitID AND AOB.CurrencyID=@CurrencyID AND AOB.AccountHeadID=@AccountHeadID AND AOB.AccountingSessionID=@SessionID AND AOB.BreakdownType=4 AND ISNULL(AOB.BreakdownObjID,0)!=0

			INSERT INTO #Temp_Table (ProductID,		AccountHeadID,		ProductName,		ProductCode,		BreakdownType)
			SELECT DISTINCT			VPT.ProductID,	VPT.AccountHeadID,	VPT.ProductName,	VPT.ProductCode,	4 FROM View_VPTransaction AS VPT WHERE VPT.BUID=@BusinessUnitID AND VPT.CurrencyID=@CurrencyID AND VPT.AccountHeadID=@AccountHeadID AND CONVERT(DATE, CONVERT(VARCHAR(12),VPT.TransactionDate,106)) BETWEEN  CONVERT(DATE, CONVERT(VARCHAR(12),@SessionStartDate,106)) AND DATEADD(DAY,-1,CONVERT(DATE, CONVERT(VARCHAR(12),@StartDate,106))) AND VPT.ProductID NOT IN (SELECT ISNULL(TT.ProductID,0) FROM #Temp_Table AS TT) AND VPT.ProductID IN (SELECT P.ProductID FROM Product AS P WHERE P.ProductType=1)


			INSERT INTO #Temp_Table (OrderID,		AccountHeadID,		OrderNo,		OrderDate,		BreakdownType)
			SELECT DISTINCT			VOR.OrderID,	VOR.AccountHeadID,	VOR.SalesNo,	VOR.SalesDate,	7 FROM View_VOReference AS VOR WHERE VOR.BUID=@BusinessUnitID AND VOR.CurrencyID=@CurrencyID AND VOR.AccountHeadID=@AccountHeadID AND CONVERT(DATE, CONVERT(VARCHAR(12),VOR.TransactionDate,106)) BETWEEN  CONVERT(DATE, CONVERT(VARCHAR(12),@SessionStartDate,106)) AND DATEADD(DAY,-1,CONVERT(DATE, CONVERT(VARCHAR(12),@StartDate,106))) AND VOR.OrderID IN (SELECT SO.SaleOrderID FROM SaleOrder AS SO WHERE ISNULL(SO.ApprovedBy,0)!=0)
		END
	END
END
ELSE
BEGIN
	IF(@IsApproved=1)
	BEGIN
		IF(@CurrencyID=@BaseCurrencyID)
		BEGIN
			INSERT INTO #Temp_Table (CCID,				AccountHeadID,		CCName,				CCCode,				BreakdownType)
			SELECT DISTINCT			AOB.BreakdownObjID,	AOB.AccountHeadID,	AOB.BreakdownName,	AOB.BreakdownCode,	1 FROM View_AccountOpenningBreakdown AS AOB WHERE AOB.AccountHeadID=@AccountHeadID AND AOB.AccountingSessionID=@SessionID AND AOB.BreakdownType=1 AND ISNULL(AOB.BreakdownObjID,0)!=0

			INSERT INTO #Temp_Table (CCID,		AccountHeadID,		CCName,				CCCode,				BreakdownType)
			SELECT DISTINCT			CCT.CCID,	CCT.AccountHeadID,	CCT.CostCenterName,	CCT.CostCenterCode,	1 FROM View_CostCenterTransaction AS CCT WHERE CCT.AccountHeadID=@AccountHeadID AND ISNULL(CCT.ApprovedBy,0)!=0 AND CONVERT(DATE, CONVERT(VARCHAR(12),CCT.TransactionDate,106)) BETWEEN  CONVERT(DATE, CONVERT(VARCHAR(12),@SessionStartDate,106)) AND DATEADD(DAY,-1,CONVERT(DATE, CONVERT(VARCHAR(12),@StartDate,106))) AND CCT.CCID NOT IN (SELECT ISNULL(TT.CCID,0) FROM #Temp_Table AS TT)


			INSERT INTO #Temp_Table(VoucherBillID,		AccountHeadID,		BillNo,		BillDate,		BreakdownType)
				SELECT DISTINCT		VB.VoucherBillID,	VB.AccountHeadID,	VB.BillNo,	VB.BillDate,	2 FROM   View_VoucherBill as VB WHERE VB.AccountHeadID=@AccountHeadID AND VB.SubLedgerID=0 AND VB.RemainningBalance>0


			INSERT INTO #Temp_Table (ProductID,			AccountHeadID,		ProductName,		ProductCode,		BreakdownType)
			SELECT DISTINCT			AOB.BreakdownObjID,	AOB.AccountHeadID,	AOB.BreakdownName,	AOB.BreakdownCode,	4 FROM View_AccountOpenningBreakdown AS AOB WHERE AOB.AccountHeadID=@AccountHeadID AND AOB.AccountingSessionID=@SessionID AND AOB.BreakdownType=4 AND ISNULL(AOB.BreakdownObjID,0)!=0

			INSERT INTO #Temp_Table (ProductID,		AccountHeadID,		ProductName,		ProductCode,		BreakdownType)
			SELECT DISTINCT			VPT.ProductID,	VPT.AccountHeadID,	VPT.ProductName,	VPT.ProductCode,	4 FROM View_VPTransaction AS VPT WHERE ISNULL(VPT.ApprovedBy,0)!=0  AND VPT.AccountHeadID=@AccountHeadID AND CONVERT(DATE, CONVERT(VARCHAR(12),VPT.TransactionDate,106)) BETWEEN  CONVERT(DATE, CONVERT(VARCHAR(12),@SessionStartDate,106)) AND DATEADD(DAY,-1,CONVERT(DATE, CONVERT(VARCHAR(12),@StartDate,106))) AND VPT.ProductID NOT IN (SELECT ISNULL(TT.ProductID,0) FROM #Temp_Table AS TT) AND VPT.ProductID IN (SELECT P.ProductID FROM Product AS P WHERE P.ProductType=1)


			INSERT INTO #Temp_Table (OrderID,		AccountHeadID,		OrderNo,		OrderDate,		BreakdownType)
			SELECT DISTINCT			VOR.OrderID,	VOR.AccountHeadID,	VOR.SalesNo,	VOR.SalesDate,	7 FROM View_VOReference AS VOR WHERE ISNULL(VOR.ApprovedBy,0)!=0  AND VOR.AccountHeadID=@AccountHeadID AND CONVERT(DATE, CONVERT(VARCHAR(12),VOR.TransactionDate,106)) BETWEEN  CONVERT(DATE, CONVERT(VARCHAR(12),@SessionStartDate,106)) AND DATEADD(DAY,-1,CONVERT(DATE, CONVERT(VARCHAR(12),@StartDate,106))) AND VOR.OrderID IN (SELECT SO.SaleOrderID FROM SaleOrder AS SO WHERE ISNULL(SO.ApprovedBy,0)!=0)
		END
		ELSE
		BEGIN
			INSERT INTO #Temp_Table (CCID,				AccountHeadID,		CCName,				CCCode,				BreakdownType)
			SELECT DISTINCT			AOB.BreakdownObjID,	AOB.AccountHeadID,	AOB.BreakdownName,	AOB.BreakdownCode,	1 FROM View_AccountOpenningBreakdown AS AOB WHERE AOB.AccountHeadID=@AccountHeadID AND AOB.CurrencyID=@CurrencyID AND AOB.AccountingSessionID=@SessionID AND AOB.BreakdownType=1 AND ISNULL(AOB.BreakdownObjID,0)!=0

			INSERT INTO #Temp_Table (CCID,		AccountHeadID,		CCName,				CCCode,				BreakdownType)
			SELECT DISTINCT			CCT.CCID,	CCT.AccountHeadID,	CCT.CostCenterName,	CCT.CostCenterCode,	1 FROM View_CostCenterTransaction AS CCT WHERE CCT.AccountHeadID=@AccountHeadID AND ISNULL(CCT.ApprovedBy,0)!=0  AND CCT.CurrencyID=@CurrencyID AND CONVERT(DATE, CONVERT(VARCHAR(12),CCT.TransactionDate,106)) BETWEEN  CONVERT(DATE, CONVERT(VARCHAR(12),@SessionStartDate,106)) AND DATEADD(DAY,-1,CONVERT(DATE, CONVERT(VARCHAR(12),@StartDate,106))) AND CCT.CCID NOT IN (SELECT ISNULL(TT.CCID,0) FROM #Temp_Table AS TT)


			INSERT INTO #Temp_Table(VoucherBillID,		AccountHeadID,		BillNo,		BillDate,		BreakdownType)
				SELECT DISTINCT		VB.VoucherBillID,	VB.AccountHeadID,	VB.BillNo,	VB.BillDate,	2 FROM   View_VoucherBill as VB WHERE VB.AccountHeadID=@AccountHeadID AND VB.SubLedgerID=0 AND VB.RemainningBalance>0


			INSERT INTO #Temp_Table (ProductID,			AccountHeadID,		ProductName,		ProductCode,		BreakdownType)
			SELECT DISTINCT			AOB.BreakdownObjID,	AOB.AccountHeadID,	AOB.BreakdownName,	AOB.BreakdownCode,	4 FROM View_AccountOpenningBreakdown AS AOB WHERE AOB.CurrencyID=@CurrencyID AND AOB.AccountHeadID=@AccountHeadID AND AOB.AccountingSessionID=@SessionID AND AOB.BreakdownType=4 AND ISNULL(AOB.BreakdownObjID,0)!=0

			INSERT INTO #Temp_Table (ProductID,		AccountHeadID,		ProductName,		ProductCode,		BreakdownType)
			SELECT DISTINCT			VPT.ProductID,	VPT.AccountHeadID,	VPT.ProductName,	VPT.ProductCode,	4 FROM View_VPTransaction AS VPT WHERE ISNULL(VPT.ApprovedBy,0)!=0  AND VPT.CurrencyID=@CurrencyID AND VPT.AccountHeadID=@AccountHeadID AND CONVERT(DATE, CONVERT(VARCHAR(12),VPT.TransactionDate,106)) BETWEEN  CONVERT(DATE, CONVERT(VARCHAR(12),@SessionStartDate,106)) AND DATEADD(DAY,-1,CONVERT(DATE, CONVERT(VARCHAR(12),@StartDate,106))) AND VPT.ProductID NOT IN (SELECT ISNULL(TT.ProductID,0) FROM #Temp_Table AS TT) AND VPT.ProductID IN (SELECT P.ProductID FROM Product AS P WHERE P.ProductType=1)


			INSERT INTO #Temp_Table (OrderID,		AccountHeadID,		OrderNo,		OrderDate,		BreakdownType)
			SELECT DISTINCT			VOR.OrderID,	VOR.AccountHeadID,	VOR.SalesNo,	VOR.SalesDate,	7 FROM View_VOReference AS VOR WHERE ISNULL(VOR.ApprovedBy,0)!=0  AND VOR.CurrencyID=@CurrencyID AND VOR.AccountHeadID=@AccountHeadID AND CONVERT(DATE, CONVERT(VARCHAR(12),VOR.TransactionDate,106)) BETWEEN  CONVERT(DATE, CONVERT(VARCHAR(12),@SessionStartDate,106)) AND DATEADD(DAY,-1,CONVERT(DATE, CONVERT(VARCHAR(12),@StartDate,106))) AND VOR.OrderID IN (SELECT SO.SaleOrderID FROM SaleOrder AS SO WHERE ISNULL(SO.ApprovedBy,0)!=0)
		END
	END
	ELSE
	BEGIN
		IF(@CurrencyID=@BaseCurrencyID)
		BEGIN
			INSERT INTO #Temp_Table (CCID,				AccountHeadID,		CCName,				CCCode,				BreakdownType)
			SELECT DISTINCT			AOB.BreakdownObjID,	AOB.AccountHeadID,	AOB.BreakdownName,	AOB.BreakdownCode,	1 FROM View_AccountOpenningBreakdown AS AOB WHERE AOB.AccountHeadID=@AccountHeadID AND AOB.AccountingSessionID=@SessionID AND AOB.BreakdownType=1 AND ISNULL(AOB.BreakdownObjID,0)!=0

			INSERT INTO #Temp_Table (CCID,		AccountHeadID,		CCName,				CCCode,				BreakdownType)
			SELECT DISTINCT			CCT.CCID,	CCT.AccountHeadID,	CCT.CostCenterName,	CCT.CostCenterCode,	1 FROM View_CostCenterTransaction AS CCT WHERE CCT.AccountHeadID=@AccountHeadID AND CONVERT(DATE, CONVERT(VARCHAR(12),CCT.TransactionDate,106)) BETWEEN  CONVERT(DATE, CONVERT(VARCHAR(12),@SessionStartDate,106)) AND DATEADD(DAY,-1,CONVERT(DATE, CONVERT(VARCHAR(12),@StartDate,106))) AND CCT.CCID NOT IN (SELECT ISNULL(TT.CCID,0) FROM #Temp_Table AS TT)


			INSERT INTO #Temp_Table(VoucherBillID,		AccountHeadID,		BillNo,		BillDate,		BreakdownType)
				SELECT DISTINCT		VB.VoucherBillID,	VB.AccountHeadID,	VB.BillNo,	VB.BillDate,	2 FROM   View_VoucherBill as VB WHERE VB.AccountHeadID=@AccountHeadID AND VB.SubLedgerID=0 AND VB.RemainningBalance>0


			INSERT INTO #Temp_Table (ProductID,			AccountHeadID,		ProductName,		ProductCode,		BreakdownType)
			SELECT DISTINCT			AOB.BreakdownObjID,	AOB.AccountHeadID,	AOB.BreakdownName,	AOB.BreakdownCode,	4 FROM View_AccountOpenningBreakdown AS AOB WHERE AOB.AccountHeadID=@AccountHeadID AND AOB.AccountingSessionID=@SessionID AND AOB.BreakdownType=4 AND ISNULL(AOB.BreakdownObjID,0)!=0

			INSERT INTO #Temp_Table (ProductID,		AccountHeadID,		ProductName,		ProductCode,		BreakdownType)
			SELECT DISTINCT			VPT.ProductID,	VPT.AccountHeadID,	VPT.ProductName,	VPT.ProductCode,	4 FROM View_VPTransaction AS VPT WHERE VPT.AccountHeadID=@AccountHeadID AND CONVERT(DATE, CONVERT(VARCHAR(12),VPT.TransactionDate,106)) BETWEEN  CONVERT(DATE, CONVERT(VARCHAR(12),@SessionStartDate,106)) AND DATEADD(DAY,-1,CONVERT(DATE, CONVERT(VARCHAR(12),@StartDate,106))) AND VPT.ProductID NOT IN (SELECT ISNULL(TT.ProductID,0) FROM #Temp_Table AS TT) AND VPT.ProductID IN (SELECT P.ProductID FROM Product AS P WHERE P.ProductType=1)


			INSERT INTO #Temp_Table (OrderID,		AccountHeadID,		OrderNo,		OrderDate,		BreakdownType)
			SELECT DISTINCT			VOR.OrderID,	VOR.AccountHeadID,	VOR.SalesNo,	VOR.SalesDate,	7 FROM View_VOReference AS VOR WHERE VOR.AccountHeadID=@AccountHeadID AND CONVERT(DATE, CONVERT(VARCHAR(12),VOR.TransactionDate,106)) BETWEEN  CONVERT(DATE, CONVERT(VARCHAR(12),@SessionStartDate,106)) AND DATEADD(DAY,-1,CONVERT(DATE, CONVERT(VARCHAR(12),@StartDate,106))) AND VOR.OrderID IN (SELECT SO.SaleOrderID FROM SaleOrder AS SO WHERE ISNULL(SO.ApprovedBy,0)!=0)
		END
		ELSE
		BEGIN
			INSERT INTO #Temp_Table (CCID,				AccountHeadID,		CCName,				CCCode,				BreakdownType)
			SELECT DISTINCT			AOB.BreakdownObjID,	AOB.AccountHeadID,	AOB.BreakdownName,	AOB.BreakdownCode,	1 FROM View_AccountOpenningBreakdown AS AOB WHERE AOB.AccountHeadID=@AccountHeadID AND AOB.CurrencyID=@CurrencyID AND AOB.AccountingSessionID=@SessionID AND AOB.BreakdownType=1 AND ISNULL(AOB.BreakdownObjID,0)!=0

			INSERT INTO #Temp_Table (CCID,		AccountHeadID,		CCName,				CCCode,				BreakdownType)
			SELECT DISTINCT			CCT.CCID,	CCT.AccountHeadID,	CCT.CostCenterName,	CCT.CostCenterCode,	1 FROM View_CostCenterTransaction AS CCT WHERE CCT.AccountHeadID=@AccountHeadID AND CCT.CurrencyID=@CurrencyID AND CONVERT(DATE, CONVERT(VARCHAR(12),CCT.TransactionDate,106)) BETWEEN  CONVERT(DATE, CONVERT(VARCHAR(12),@SessionStartDate,106)) AND DATEADD(DAY,-1,CONVERT(DATE, CONVERT(VARCHAR(12),@StartDate,106))) AND CCT.CCID NOT IN (SELECT ISNULL(TT.CCID,0) FROM #Temp_Table AS TT)


			INSERT INTO #Temp_Table(VoucherBillID,		AccountHeadID,		BillNo,		BillDate,		BreakdownType)
				SELECT DISTINCT		VB.VoucherBillID,	VB.AccountHeadID,	VB.BillNo,	VB.BillDate,	2 FROM   View_VoucherBill as VB WHERE VB.AccountHeadID=@AccountHeadID AND VB.SubLedgerID=0 AND VB.RemainningBalance>0


			INSERT INTO #Temp_Table (ProductID,			AccountHeadID,		ProductName,		ProductCode,		BreakdownType)
			SELECT DISTINCT			AOB.BreakdownObjID,	AOB.AccountHeadID,	AOB.BreakdownName,	AOB.BreakdownCode,	4 FROM View_AccountOpenningBreakdown AS AOB WHERE AOB.CurrencyID=@CurrencyID AND AOB.AccountHeadID=@AccountHeadID AND AOB.AccountingSessionID=@SessionID AND AOB.BreakdownType=4 AND ISNULL(AOB.BreakdownObjID,0)!=0

			INSERT INTO #Temp_Table (ProductID,		AccountHeadID,		ProductName,		ProductCode,		BreakdownType)
			SELECT DISTINCT			VPT.ProductID,	VPT.AccountHeadID,	VPT.ProductName,	VPT.ProductCode,	4 FROM View_VPTransaction AS VPT WHERE VPT.CurrencyID=@CurrencyID AND VPT.AccountHeadID=@AccountHeadID AND CONVERT(DATE, CONVERT(VARCHAR(12),VPT.TransactionDate,106)) BETWEEN  CONVERT(DATE, CONVERT(VARCHAR(12),@SessionStartDate,106)) AND DATEADD(DAY,-1,CONVERT(DATE, CONVERT(VARCHAR(12),@StartDate,106))) AND VPT.ProductID NOT IN (SELECT ISNULL(TT.ProductID,0) FROM #Temp_Table AS TT) AND VPT.ProductID IN (SELECT P.ProductID FROM Product AS P WHERE P.ProductType=1)


			INSERT INTO #Temp_Table (OrderID,		AccountHeadID,		OrderNo,		OrderDate,		BreakdownType)
			SELECT DISTINCT			VOR.OrderID,	VOR.AccountHeadID,	VOR.SalesNo,	VOR.SalesDate,	7 FROM View_VOReference AS VOR WHERE VOR.CurrencyID=@CurrencyID AND VOR.AccountHeadID=@AccountHeadID AND CONVERT(DATE, CONVERT(VARCHAR(12),VOR.TransactionDate,106)) BETWEEN  CONVERT(DATE, CONVERT(VARCHAR(12),@SessionStartDate,106)) AND DATEADD(DAY,-1,CONVERT(DATE, CONVERT(VARCHAR(12),@StartDate,106))) AND VOR.OrderID IN (SELECT SO.SaleOrderID FROM SaleOrder AS SO WHERE ISNULL(SO.ApprovedBy,0)!=0)
		END
	END
END

print 'hello'
--select * from #Temp_Table

UPDATE #Temp_Table SET ComponentID=dbo.GetComponentID(TT.AccountHeadID) FROM #Temp_Table AS TT

IF(@BusinessUnitID>0)
BEGIN
	IF(@IsApproved=1)
	BEGIN
		IF(@CurrencyID=@BaseCurrencyID)
		BEGIN
			UPDATE #Temp_Table
			SET Debit_CCSessionAmount=ISNULL((SELECT SUM(AOB.OpenningBalance) FROM AccountOpenningBreakdown AS AOB WHERE AOB.IsDr=1 AND AOB.BusinessUnitID=@BusinessUnitID AND AOB.BreakdownObjID=TT.CCID AND AOB.AccountHeadID=TT.AccountHeadID AND AOB.AccountingSessionID=@SessionID AND AOB.BreakdownType=1),0),
				Credit_CCSessionAmount=ISNULL((SELECT SUM(AOB.OpenningBalance) FROM AccountOpenningBreakdown AS AOB WHERE AOB.IsDr=0 AND AOB.BusinessUnitID=@BusinessUnitID AND AOB.BreakdownObjID=TT.CCID AND AOB.AccountHeadID=TT.AccountHeadID AND AOB.AccountingSessionID=@SessionID AND AOB.BreakdownType=1),0),
				Debit_CCAmount=ISNULL((SELECT SUM(CCT.Amount*CCT.CurrencyConversionRate) FROM View_CostCenterTransaction AS CCT WHERE CCT.BUID=@BusinessUnitID AND ISNULL(CCT.ApprovedBy,0)!=0 AND  CCT.IsDr=1 AND CCT.CCID=TT.CCID AND CCT.AccountHeadID=TT.AccountHeadID AND CONVERT(DATE, CONVERT(VARCHAR(12),CCT.TransactionDate,106)) BETWEEN  CONVERT(DATE, CONVERT(VARCHAR(12),@SessionStartDate,106)) AND CONVERT(DATE, CONVERT(VARCHAR(12),DATEADD(DAY,-1,@StartDate),106))),0),
				Credit_CCAmount=ISNULL((SELECT SUM(CCT.Amount*CCT.CurrencyConversionRate) FROM View_CostCenterTransaction AS CCT WHERE CCT.BUID=@BusinessUnitID AND ISNULL(CCT.ApprovedBy,0)!=0 AND CCT.IsDr=0 AND CCT.CCID=TT.CCID AND CCT.AccountHeadID=TT.AccountHeadID AND CONVERT(DATE, CONVERT(VARCHAR(12),CCT.TransactionDate,106)) BETWEEN  CONVERT(DATE, CONVERT(VARCHAR(12),@SessionStartDate,106)) AND CONVERT(DATE, CONVERT(VARCHAR(12),DATEADD(DAY,-1,@StartDate),106))),0)
			FROM #Temp_Table AS TT WHERE TT.BreakdownType=1--CC

			UPDATE #Temp_Table
			SET Debit_VBSessionAmount=ISNULL((SELECT SUM(AOB.OpenningBalance) FROM AccountOpenningBreakdown AS AOB WHERE AOB.IsDr=1 AND AOB.BusinessUnitID=@BusinessUnitID AND AOB.CCID=0 AND AOB.AccountHeadID=@AccountHeadID AND AOB.BreakdownObjID=TT.VoucherBillID AND AOB.BreakdownType=2 AND AOB.AccountingSessionID=@SessionID),0),
				Credit_VBSessionAmount=ISNULL((SELECT SUM(AOB.OpenningBalance) FROM AccountOpenningBreakdown AS AOB WHERE AOB.IsDr=0 AND AOB.BusinessUnitID=@BusinessUnitID AND AOB.CCID=0 AND AOB.AccountHeadID=@AccountHeadID AND AOB.BreakdownObjID=TT.VoucherBillID AND AOB.BreakdownType=2 AND AOB.AccountingSessionID=@SessionID),0),
				Debit_VBAmount=ISNULL((SELECT SUM(VBT.Amount*VBT.ConversionRate) FROM View_VoucherBillTransaction AS VBT WHERE VBT.VoucherBillID=TT.VoucherBillID AND  VBT.IsDr=1 AND  CONVERT(DATE, CONVERT(VARCHAR(12),VBT.TransactionDate,106)) BETWEEN CONVERT(DATE, CONVERT(VARCHAR(12),@SessionStartDate,106)) AND DATEADD(DAY,-1,CONVERT(DATE, CONVERT(VARCHAR(12),@StartDate,106))) AND VBT.BUID=@BusinessUnitID AND ISNULL(VBT.CCID,0)=0 AND VBT.AccountHeadID=@AccountHeadID AND ISNULL(VBT.ApprovedBy,0)!=0),0),
				Credit_VBAmount=ISNULL((SELECT SUM(VBT.Amount*VBT.ConversionRate) FROM View_VoucherBillTransaction AS VBT WHERE VBT.VoucherBillID=TT.VoucherBillID AND  VBT.IsDr=0 AND  CONVERT(DATE, CONVERT(VARCHAR(12),VBT.TransactionDate,106)) BETWEEN CONVERT(DATE, CONVERT(VARCHAR(12),@SessionStartDate,106)) AND DATEADD(DAY,-1,CONVERT(DATE, CONVERT(VARCHAR(12),@StartDate,106))) AND VBT.BUID=@BusinessUnitID AND ISNULL(VBT.CCID,0)=0 AND VBT.AccountHeadID=@AccountHeadID AND ISNULL(VBT.ApprovedBy,0)!=0),0)
			FROM #Temp_Table AS TT WHERE TT.BreakdownType=2--BILL

			UPDATE #Temp_Table
			SET Debit_VPSessionAmount=ISNULL((SELECT SUM(AOB.OpenningBalance) FROM AccountOpenningBreakdown AS AOB WHERE AOB.BusinessUnitID=@BusinessUnitID AND AOB.IsDr=1 AND AOB.BreakdownObjID=TT.ProductID AND AOB.AccountHeadID=@AccountHeadID AND AOB.AccountingSessionID=@SessionID AND AOB.BreakdownType=4),0),
				Credit_VPSessionAmount=ISNULL((SELECT SUM(AOB.OpenningBalance) FROM AccountOpenningBreakdown AS AOB WHERE AOB.BusinessUnitID=@BusinessUnitID AND AOB.IsDr=0 AND AOB.BreakdownObjID=TT.ProductID AND AOB.AccountHeadID=@AccountHeadID AND AOB.AccountingSessionID=@SessionID AND AOB.BreakdownType=4),0),
				Debit_VPAmount=ISNULL((SELECT SUM(VPT.Amount*VPT.ConversionRate) FROM View_VPTransaction AS VPT WHERE VPT.BUID=@BusinessUnitID AND ISNULL(VPT.ApprovedBy,0)!=0 AND  VPT.IsDr=1 AND VPT.ProductID=TT.ProductID AND VPT.AccountHeadID=@AccountHeadID AND CONVERT(DATE, CONVERT(VARCHAR(12),VPT.TransactionDate,106)) BETWEEN  CONVERT(DATE, CONVERT(VARCHAR(12),@SessionStartDate,106)) AND CONVERT(DATE, CONVERT(VARCHAR(12),DATEADD(DAY,-1,@StartDate),106))),0),
				Credit_VPAmount=ISNULL((SELECT SUM(VPT.Amount*VPT.ConversionRate) FROM View_VPTransaction AS VPT WHERE VPT.BUID=@BusinessUnitID AND ISNULL(VPT.ApprovedBy,0)!=0 AND VPT.IsDr=0 AND VPT.ProductID=TT.ProductID AND VPT.AccountHeadID=@AccountHeadID AND CONVERT(DATE, CONVERT(VARCHAR(12),VPT.TransactionDate,106)) BETWEEN  CONVERT(DATE, CONVERT(VARCHAR(12),@SessionStartDate,106)) AND CONVERT(DATE, CONVERT(VARCHAR(12),DATEADD(DAY,-1,@StartDate),106))),0)
			FROM #Temp_Table AS TT WHERE TT.BreakdownType=4--ITEM

			UPDATE #Temp_Table
			SET Debit_VOSessionAmount=0,--ISNULL((SELECT SUM(AOB.AmountInCurrency) FROM AccountOpenningBreakdown AS AOB WHERE AOB.BusinessUnitID=@BusinessUnitID AND AOB.IsDr=1 AND AOB.CurrencyID=@CurrencyID AND AOB.BreakdownObjID=TT.ProductID AND AOB.AccountHeadID=@AccountHeadID AND AOB.AccountingSessionID=@SessionID AND AOB.BreakdownType=4),0),
				Credit_VOSessionAmount=0,--ISNULL((SELECT SUM(AOB.AmountInCurrency) FROM AccountOpenningBreakdown AS AOB WHERE AOB.BusinessUnitID=@BusinessUnitID AND AOB.IsDr=0 AND AOB.CurrencyID=@CurrencyID AND AOB.BreakdownObjID=TT.ProductID AND AOB.AccountHeadID=@AccountHeadID AND AOB.AccountingSessionID=@SessionID AND AOB.BreakdownType=4),0),
				Debit_VOAmount=ISNULL((SELECT SUM(VOR.Amount*VOR.ConversionRate) FROM View_VOReference AS VOR WHERE VOR.BUID=@BusinessUnitID AND ISNULL(VOR.ApprovedBy,0)!=0 AND  VOR.IsDebit=1 AND VOR.OrderID=TT.OrderID AND VOR.AccountHeadID=@AccountHeadID AND CONVERT(DATE, CONVERT(VARCHAR(12),VOR.TransactionDate,106)) BETWEEN  CONVERT(DATE, CONVERT(VARCHAR(12),@SessionStartDate,106)) AND CONVERT(DATE, CONVERT(VARCHAR(12),DATEADD(DAY,-1,@StartDate),106))),0),
				Credit_VOAmount=ISNULL((SELECT SUM(VOR.Amount*VOR.ConversionRate) FROM View_VOReference AS VOR WHERE VOR.BUID=@BusinessUnitID AND ISNULL(VOR.ApprovedBy,0)!=0 AND VOR.IsDebit=0 AND VOR.OrderID=TT.OrderID AND VOR.AccountHeadID=@AccountHeadID AND CONVERT(DATE, CONVERT(VARCHAR(12),VOR.TransactionDate,106)) BETWEEN  CONVERT(DATE, CONVERT(VARCHAR(12),@SessionStartDate,106)) AND CONVERT(DATE, CONVERT(VARCHAR(12),DATEADD(DAY,-1,@StartDate),106))),0)
			FROM #Temp_Table AS TT WHERE TT.BreakdownType=7--ORDER
		END
		ELSE
		BEGIN
			UPDATE #Temp_Table
			SET Debit_CCSessionAmount=ISNULL((SELECT SUM(AOB.AmountInCurrency) FROM AccountOpenningBreakdown AS AOB WHERE AOB.IsDr=1 AND AOB.BusinessUnitID=@BusinessUnitID AND AOB.CurrencyID=@CurrencyID AND AOB.BreakdownObjID=TT.CCID AND AOB.AccountHeadID=TT.AccountHeadID AND AOB.AccountingSessionID=@SessionID AND AOB.BreakdownType=1),0),
				Credit_CCSessionAmount=ISNULL((SELECT SUM(AOB.AmountInCurrency) FROM AccountOpenningBreakdown AS AOB WHERE AOB.IsDr=0 AND AOB.BusinessUnitID=@BusinessUnitID AND AOB.CurrencyID=@CurrencyID AND AOB.BreakdownObjID=TT.CCID AND AOB.AccountHeadID=TT.AccountHeadID AND AOB.AccountingSessionID=@SessionID AND AOB.BreakdownType=1),0),
				Debit_CCAmount=ISNULL((SELECT SUM(CCT.Amount) FROM View_CostCenterTransaction AS CCT WHERE CCT.BUID=@BusinessUnitID AND ISNULL(CCT.ApprovedBy,0)!=0 AND CCT.CurrencyID=@CurrencyID AND  CCT.IsDr=1 AND CCT.CCID=TT.CCID AND CCT.AccountHeadID=TT.AccountHeadID AND CONVERT(DATE, CONVERT(VARCHAR(12),CCT.TransactionDate,106)) BETWEEN  CONVERT(DATE, CONVERT(VARCHAR(12),@SessionStartDate,106)) AND CONVERT(DATE, CONVERT(VARCHAR(12),DATEADD(DAY,-1,@StartDate),106))),0),
				Credit_CCAmount=ISNULL((SELECT SUM(CCT.Amount) FROM View_CostCenterTransaction AS CCT WHERE CCT.BUID=@BusinessUnitID AND ISNULL(CCT.ApprovedBy,0)!=0 AND CCT.CurrencyID=@CurrencyID AND CCT.IsDr=0 AND CCT.CCID=TT.CCID AND CCT.AccountHeadID=TT.AccountHeadID AND CONVERT(DATE, CONVERT(VARCHAR(12),CCT.TransactionDate,106)) BETWEEN  CONVERT(DATE, CONVERT(VARCHAR(12),@SessionStartDate,106)) AND CONVERT(DATE, CONVERT(VARCHAR(12),DATEADD(DAY,-1,@StartDate),106))),0)
			FROM #Temp_Table AS TT WHERE TT.BreakdownType=1--CC

			UPDATE #Temp_Table
			SET Debit_VBSessionAmount=ISNULL((SELECT SUM(AOB.AmountInCurrency) FROM AccountOpenningBreakdown AS AOB WHERE AOB.IsDr=1 AND AOB.BusinessUnitID=@BusinessUnitID AND AOB.CurrencyID=@CurrencyID AND AOB.CCID=0 AND AOB.AccountHeadID=@AccountHeadID AND AOB.BreakdownObjID=TT.VoucherBillID AND AOB.BreakdownType=2 AND AOB.AccountingSessionID=@SessionID),0),
				Credit_VBSessionAmount=ISNULL((SELECT SUM(AOB.AmountInCurrency) FROM AccountOpenningBreakdown AS AOB WHERE AOB.IsDr=0 AND AOB.BusinessUnitID=@BusinessUnitID AND AOB.CurrencyID=@CurrencyID AND AOB.CCID=0 AND AOB.AccountHeadID=@AccountHeadID AND AOB.BreakdownObjID=TT.VoucherBillID AND AOB.BreakdownType=2 AND AOB.AccountingSessionID=@SessionID),0),
				Debit_VBAmount=ISNULL((SELECT SUM(VBT.Amount) FROM View_VoucherBillTransaction AS VBT WHERE VBT.CurrencyID=@CurrencyID AND VBT.VoucherBillID=TT.VoucherBillID AND  VBT.IsDr=1 AND  CONVERT(DATE, CONVERT(VARCHAR(12),VBT.TransactionDate,106)) BETWEEN CONVERT(DATE, CONVERT(VARCHAR(12),@SessionStartDate,106)) AND DATEADD(DAY,-1,CONVERT(DATE, CONVERT(VARCHAR(12),@StartDate,106))) AND VBT.BUID=@BusinessUnitID AND ISNULL(VBT.CCID,0)=0 AND VBT.AccountHeadID=@AccountHeadID AND ISNULL(VBT.ApprovedBy,0)!=0),0),
				Credit_VBAmount=ISNULL((SELECT SUM(VBT.Amount) FROM View_VoucherBillTransaction AS VBT WHERE VBT.CurrencyID=@CurrencyID AND VBT.VoucherBillID=TT.VoucherBillID AND  VBT.IsDr=0 AND  CONVERT(DATE, CONVERT(VARCHAR(12),VBT.TransactionDate,106)) BETWEEN CONVERT(DATE, CONVERT(VARCHAR(12),@SessionStartDate,106)) AND DATEADD(DAY,-1,CONVERT(DATE, CONVERT(VARCHAR(12),@StartDate,106))) AND VBT.BUID=@BusinessUnitID AND ISNULL(VBT.CCID,0)=0 AND VBT.AccountHeadID=@AccountHeadID AND ISNULL(VBT.ApprovedBy,0)!=0),0)
			FROM #Temp_Table AS TT WHERE TT.BreakdownType=2--BILL

			UPDATE #Temp_Table
			SET Debit_VPSessionAmount=ISNULL((SELECT SUM(AOB.AmountInCurrency) FROM AccountOpenningBreakdown AS AOB WHERE AOB.BusinessUnitID=@BusinessUnitID AND AOB.IsDr=1 AND AOB.CurrencyID=@CurrencyID AND AOB.BreakdownObjID=TT.ProductID AND AOB.AccountHeadID=@AccountHeadID AND AOB.AccountingSessionID=@SessionID AND AOB.BreakdownType=4),0),
				Credit_VPSessionAmount=ISNULL((SELECT SUM(AOB.AmountInCurrency) FROM AccountOpenningBreakdown AS AOB WHERE AOB.BusinessUnitID=@BusinessUnitID AND AOB.IsDr=0 AND AOB.CurrencyID=@CurrencyID AND AOB.BreakdownObjID=TT.ProductID AND AOB.AccountHeadID=@AccountHeadID AND AOB.AccountingSessionID=@SessionID AND AOB.BreakdownType=4),0),
				Debit_VPAmount=ISNULL((SELECT SUM(VPT.Amount) FROM View_VPTransaction AS VPT WHERE VPT.BUID=@BusinessUnitID AND ISNULL(VPT.ApprovedBy,0)!=0 AND VPT.CurrencyID=@CurrencyID AND  VPT.IsDr=1 AND VPT.ProductID=TT.ProductID AND VPT.AccountHeadID=@AccountHeadID AND CONVERT(DATE, CONVERT(VARCHAR(12),VPT.TransactionDate,106)) BETWEEN  CONVERT(DATE, CONVERT(VARCHAR(12),@SessionStartDate,106)) AND CONVERT(DATE, CONVERT(VARCHAR(12),DATEADD(DAY,-1,@StartDate),106))),0),
				Credit_VPAmount=ISNULL((SELECT SUM(VPT.Amount) FROM View_VPTransaction AS VPT WHERE VPT.BUID=@BusinessUnitID AND ISNULL(VPT.ApprovedBy,0)!=0 AND VPT.CurrencyID=@CurrencyID AND VPT.IsDr=0 AND VPT.ProductID=TT.ProductID AND VPT.AccountHeadID=@AccountHeadID AND CONVERT(DATE, CONVERT(VARCHAR(12),VPT.TransactionDate,106)) BETWEEN  CONVERT(DATE, CONVERT(VARCHAR(12),@SessionStartDate,106)) AND CONVERT(DATE, CONVERT(VARCHAR(12),DATEADD(DAY,-1,@StartDate),106))),0)
			FROM #Temp_Table AS TT WHERE TT.BreakdownType=4--ITEM

			UPDATE #Temp_Table
			SET Debit_VOSessionAmount=0,--ISNULL((SELECT SUM(AOB.AmountInCurrency) FROM AccountOpenningBreakdown AS AOB WHERE AOB.BusinessUnitID=@BusinessUnitID AND AOB.IsDr=1 AND AOB.CurrencyID=@CurrencyID AND AOB.BreakdownObjID=TT.ProductID AND AOB.AccountHeadID=@AccountHeadID AND AOB.AccountingSessionID=@SessionID AND AOB.BreakdownType=4),0),
				Credit_VOSessionAmount=0,--ISNULL((SELECT SUM(AOB.AmountInCurrency) FROM AccountOpenningBreakdown AS AOB WHERE AOB.BusinessUnitID=@BusinessUnitID AND AOB.IsDr=0 AND AOB.CurrencyID=@CurrencyID AND AOB.BreakdownObjID=TT.ProductID AND AOB.AccountHeadID=@AccountHeadID AND AOB.AccountingSessionID=@SessionID AND AOB.BreakdownType=4),0),
				Debit_VOAmount=ISNULL((SELECT SUM(VOR.Amount) FROM View_VOReference AS VOR WHERE VOR.BUID=@BusinessUnitID AND ISNULL(VOR.ApprovedBy,0)!=0 AND VOR.CurrencyID=@CurrencyID AND  VOR.IsDebit=1 AND VOR.OrderID=TT.OrderID AND VOR.AccountHeadID=@AccountHeadID AND CONVERT(DATE, CONVERT(VARCHAR(12),VOR.TransactionDate,106)) BETWEEN  CONVERT(DATE, CONVERT(VARCHAR(12),@SessionStartDate,106)) AND CONVERT(DATE, CONVERT(VARCHAR(12),DATEADD(DAY,-1,@StartDate),106))),0),
				Credit_VOAmount=ISNULL((SELECT SUM(VOR.Amount) FROM View_VOReference AS VOR WHERE VOR.BUID=@BusinessUnitID AND ISNULL(VOR.ApprovedBy,0)!=0 AND VOR.CurrencyID=@CurrencyID AND VOR.IsDebit=0 AND VOR.OrderID=TT.OrderID AND VOR.AccountHeadID=@AccountHeadID AND CONVERT(DATE, CONVERT(VARCHAR(12),VOR.TransactionDate,106)) BETWEEN  CONVERT(DATE, CONVERT(VARCHAR(12),@SessionStartDate,106)) AND CONVERT(DATE, CONVERT(VARCHAR(12),DATEADD(DAY,-1,@StartDate),106))),0)
			FROM #Temp_Table AS TT WHERE TT.BreakdownType=7--ORDER
		END
	END
	ELSE
	BEGIN
		IF(@CurrencyID=@BaseCurrencyID)
		BEGIN
			UPDATE #Temp_Table
			SET Debit_CCSessionAmount=ISNULL((SELECT SUM(AOB.OpenningBalance) FROM AccountOpenningBreakdown AS AOB WHERE AOB.IsDr=1 AND AOB.BusinessUnitID=@BusinessUnitID AND AOB.BreakdownObjID=TT.CCID AND AOB.AccountHeadID=TT.AccountHeadID AND AOB.AccountingSessionID=@SessionID AND AOB.BreakdownType=1),0),
				Credit_CCSessionAmount=ISNULL((SELECT SUM(AOB.OpenningBalance) FROM AccountOpenningBreakdown AS AOB WHERE AOB.IsDr=0 AND AOB.BusinessUnitID=@BusinessUnitID AND AOB.BreakdownObjID=TT.CCID AND AOB.AccountHeadID=TT.AccountHeadID AND AOB.AccountingSessionID=@SessionID AND AOB.BreakdownType=1),0),
				Debit_CCAmount=ISNULL((SELECT SUM(CCT.Amount*CCT.CurrencyConversionRate) FROM View_CostCenterTransaction AS CCT WHERE CCT.BUID=@BusinessUnitID AND  CCT.IsDr=1 AND CCT.CCID=TT.CCID AND CCT.AccountHeadID=TT.AccountHeadID AND CONVERT(DATE, CONVERT(VARCHAR(12),CCT.TransactionDate,106)) BETWEEN  CONVERT(DATE, CONVERT(VARCHAR(12),@SessionStartDate,106)) AND CONVERT(DATE, CONVERT(VARCHAR(12),DATEADD(DAY,-1,@StartDate),106))),0),
				Credit_CCAmount=ISNULL((SELECT SUM(CCT.Amount*CCT.CurrencyConversionRate) FROM View_CostCenterTransaction AS CCT WHERE CCT.BUID=@BusinessUnitID AND CCT.IsDr=0 AND CCT.CCID=TT.CCID AND CCT.AccountHeadID=TT.AccountHeadID AND CONVERT(DATE, CONVERT(VARCHAR(12),CCT.TransactionDate,106)) BETWEEN  CONVERT(DATE, CONVERT(VARCHAR(12),@SessionStartDate,106)) AND CONVERT(DATE, CONVERT(VARCHAR(12),DATEADD(DAY,-1,@StartDate),106))),0)
			FROM #Temp_Table AS TT WHERE TT.BreakdownType=1--CC

			UPDATE #Temp_Table
			SET Debit_VBSessionAmount=ISNULL((SELECT SUM(AOB.OpenningBalance) FROM AccountOpenningBreakdown AS AOB WHERE AOB.IsDr=1 AND AOB.BusinessUnitID=@BusinessUnitID AND AOB.CCID=0 AND AOB.AccountHeadID=@AccountHeadID AND AOB.BreakdownObjID=TT.VoucherBillID AND AOB.BreakdownType=2 AND AOB.AccountingSessionID=@SessionID),0),
				Credit_VBSessionAmount=ISNULL((SELECT SUM(AOB.OpenningBalance) FROM AccountOpenningBreakdown AS AOB WHERE AOB.IsDr=0 AND AOB.BusinessUnitID=@BusinessUnitID AND AOB.CCID=0 AND AOB.AccountHeadID=@AccountHeadID AND AOB.BreakdownObjID=TT.VoucherBillID AND AOB.BreakdownType=2 AND AOB.AccountingSessionID=@SessionID),0),
				Debit_VBAmount=ISNULL((SELECT SUM(VBT.Amount*VBT.ConversionRate) FROM View_VoucherBillTransaction AS VBT WHERE VBT.VoucherBillID=TT.VoucherBillID AND  VBT.IsDr=1 AND  CONVERT(DATE, CONVERT(VARCHAR(12),VBT.TransactionDate,106)) BETWEEN CONVERT(DATE, CONVERT(VARCHAR(12),@SessionStartDate,106)) AND DATEADD(DAY,-1,CONVERT(DATE, CONVERT(VARCHAR(12),@StartDate,106))) AND VBT.BUID=@BusinessUnitID AND ISNULL(VBT.CCID,0)=0 AND VBT.AccountHeadID=@AccountHeadID),0),
				Credit_VBAmount=ISNULL((SELECT SUM(VBT.Amount*VBT.ConversionRate) FROM View_VoucherBillTransaction AS VBT WHERE VBT.VoucherBillID=TT.VoucherBillID AND  VBT.IsDr=0 AND  CONVERT(DATE, CONVERT(VARCHAR(12),VBT.TransactionDate,106)) BETWEEN CONVERT(DATE, CONVERT(VARCHAR(12),@SessionStartDate,106)) AND DATEADD(DAY,-1,CONVERT(DATE, CONVERT(VARCHAR(12),@StartDate,106))) AND VBT.BUID=@BusinessUnitID AND ISNULL(VBT.CCID,0)=0 AND VBT.AccountHeadID=@AccountHeadID),0)
			FROM #Temp_Table AS TT WHERE TT.BreakdownType=2--BILL

			UPDATE #Temp_Table
			SET Debit_VPSessionAmount=ISNULL((SELECT SUM(AOB.OpenningBalance) FROM AccountOpenningBreakdown AS AOB WHERE AOB.BusinessUnitID=@BusinessUnitID AND AOB.IsDr=1 AND AOB.BreakdownObjID=TT.ProductID AND AOB.AccountHeadID=@AccountHeadID AND AOB.AccountingSessionID=@SessionID AND AOB.BreakdownType=4),0),
				Credit_VPSessionAmount=ISNULL((SELECT SUM(AOB.OpenningBalance) FROM AccountOpenningBreakdown AS AOB WHERE AOB.BusinessUnitID=@BusinessUnitID AND AOB.IsDr=0 AND AOB.BreakdownObjID=TT.ProductID AND AOB.AccountHeadID=@AccountHeadID AND AOB.AccountingSessionID=@SessionID AND AOB.BreakdownType=4),0),
				Debit_VPAmount=ISNULL((SELECT SUM(VPT.Amount*VPT.ConversionRate) FROM View_VPTransaction AS VPT WHERE VPT.BUID=@BusinessUnitID AND  VPT.IsDr=1 AND VPT.ProductID=TT.ProductID AND VPT.AccountHeadID=@AccountHeadID AND CONVERT(DATE, CONVERT(VARCHAR(12),VPT.TransactionDate,106)) BETWEEN  CONVERT(DATE, CONVERT(VARCHAR(12),@SessionStartDate,106)) AND CONVERT(DATE, CONVERT(VARCHAR(12),DATEADD(DAY,-1,@StartDate),106))),0),
				Credit_VPAmount=ISNULL((SELECT SUM(VPT.Amount*VPT.ConversionRate) FROM View_VPTransaction AS VPT WHERE VPT.BUID=@BusinessUnitID AND VPT.IsDr=0 AND VPT.ProductID=TT.ProductID AND VPT.AccountHeadID=@AccountHeadID AND CONVERT(DATE, CONVERT(VARCHAR(12),VPT.TransactionDate,106)) BETWEEN  CONVERT(DATE, CONVERT(VARCHAR(12),@SessionStartDate,106)) AND CONVERT(DATE, CONVERT(VARCHAR(12),DATEADD(DAY,-1,@StartDate),106))),0)
			FROM #Temp_Table AS TT WHERE TT.BreakdownType=4--ITEM

			UPDATE #Temp_Table
			SET Debit_VOSessionAmount=0,--ISNULL((SELECT SUM(AOB.AmountInCurrency) FROM AccountOpenningBreakdown AS AOB WHERE AOB.BusinessUnitID=@BusinessUnitID AND AOB.IsDr=1 AND AOB.CurrencyID=@CurrencyID AND AOB.BreakdownObjID=TT.ProductID AND AOB.AccountHeadID=@AccountHeadID AND AOB.AccountingSessionID=@SessionID AND AOB.BreakdownType=4),0),
				Credit_VOSessionAmount=0,--ISNULL((SELECT SUM(AOB.AmountInCurrency) FROM AccountOpenningBreakdown AS AOB WHERE AOB.BusinessUnitID=@BusinessUnitID AND AOB.IsDr=0 AND AOB.CurrencyID=@CurrencyID AND AOB.BreakdownObjID=TT.ProductID AND AOB.AccountHeadID=@AccountHeadID AND AOB.AccountingSessionID=@SessionID AND AOB.BreakdownType=4),0),
				Debit_VOAmount=ISNULL((SELECT SUM(VOR.Amount*VOR.ConversionRate) FROM View_VOReference AS VOR WHERE VOR.BUID=@BusinessUnitID AND  VOR.IsDebit=1 AND VOR.OrderID=TT.OrderID AND VOR.AccountHeadID=@AccountHeadID AND CONVERT(DATE, CONVERT(VARCHAR(12),VOR.TransactionDate,106)) BETWEEN  CONVERT(DATE, CONVERT(VARCHAR(12),@SessionStartDate,106)) AND CONVERT(DATE, CONVERT(VARCHAR(12),DATEADD(DAY,-1,@StartDate),106))),0),
				Credit_VOAmount=ISNULL((SELECT SUM(VOR.Amount*VOR.ConversionRate) FROM View_VOReference AS VOR WHERE VOR.BUID=@BusinessUnitID AND VOR.IsDebit=0 AND VOR.OrderID=TT.OrderID AND VOR.AccountHeadID=@AccountHeadID AND CONVERT(DATE, CONVERT(VARCHAR(12),VOR.TransactionDate,106)) BETWEEN  CONVERT(DATE, CONVERT(VARCHAR(12),@SessionStartDate,106)) AND CONVERT(DATE, CONVERT(VARCHAR(12),DATEADD(DAY,-1,@StartDate),106))),0)
			FROM #Temp_Table AS TT WHERE TT.BreakdownType=7--ORDER
		END
		ELSE
		BEGIN
			UPDATE #Temp_Table
			SET Debit_CCSessionAmount=ISNULL((SELECT SUM(AOB.AmountInCurrency) FROM AccountOpenningBreakdown AS AOB WHERE AOB.IsDr=1 AND AOB.BusinessUnitID=@BusinessUnitID AND AOB.CurrencyID=@CurrencyID AND AOB.BreakdownObjID=TT.CCID AND AOB.AccountHeadID=TT.AccountHeadID AND AOB.AccountingSessionID=@SessionID AND AOB.BreakdownType=1),0),
				Credit_CCSessionAmount=ISNULL((SELECT SUM(AOB.AmountInCurrency) FROM AccountOpenningBreakdown AS AOB WHERE AOB.IsDr=0 AND AOB.BusinessUnitID=@BusinessUnitID AND AOB.CurrencyID=@CurrencyID AND AOB.BreakdownObjID=TT.CCID AND AOB.AccountHeadID=TT.AccountHeadID AND AOB.AccountingSessionID=@SessionID AND AOB.BreakdownType=1),0),
				Debit_CCAmount=ISNULL((SELECT SUM(CCT.Amount) FROM View_CostCenterTransaction AS CCT WHERE CCT.BUID=@BusinessUnitID AND CCT.CurrencyID=@CurrencyID AND  CCT.IsDr=1 AND CCT.CCID=TT.CCID AND CCT.AccountHeadID=TT.AccountHeadID AND CONVERT(DATE, CONVERT(VARCHAR(12),CCT.TransactionDate,106)) BETWEEN  CONVERT(DATE, CONVERT(VARCHAR(12),@SessionStartDate,106)) AND CONVERT(DATE, CONVERT(VARCHAR(12),DATEADD(DAY,-1,@StartDate),106))),0),
				Credit_CCAmount=ISNULL((SELECT SUM(CCT.Amount) FROM View_CostCenterTransaction AS CCT WHERE CCT.BUID=@BusinessUnitID AND CCT.CurrencyID=@CurrencyID AND CCT.IsDr=0 AND CCT.CCID=TT.CCID AND CCT.AccountHeadID=TT.AccountHeadID AND CONVERT(DATE, CONVERT(VARCHAR(12),CCT.TransactionDate,106)) BETWEEN  CONVERT(DATE, CONVERT(VARCHAR(12),@SessionStartDate,106)) AND CONVERT(DATE, CONVERT(VARCHAR(12),DATEADD(DAY,-1,@StartDate),106))),0)
			FROM #Temp_Table AS TT WHERE TT.BreakdownType=1--CC

			UPDATE #Temp_Table
			SET Debit_VBSessionAmount=ISNULL((SELECT SUM(AOB.AmountInCurrency) FROM AccountOpenningBreakdown AS AOB WHERE AOB.IsDr=1 AND AOB.BusinessUnitID=@BusinessUnitID AND AOB.CurrencyID=@CurrencyID AND AOB.CCID=0 AND AOB.AccountHeadID=@AccountHeadID AND AOB.BreakdownObjID=TT.VoucherBillID AND AOB.BreakdownType=2 AND AOB.AccountingSessionID=@SessionID),0),
				Credit_VBSessionAmount=ISNULL((SELECT SUM(AOB.AmountInCurrency) FROM AccountOpenningBreakdown AS AOB WHERE AOB.IsDr=0 AND AOB.BusinessUnitID=@BusinessUnitID AND AOB.CurrencyID=@CurrencyID AND AOB.CCID=0 AND AOB.AccountHeadID=@AccountHeadID AND AOB.BreakdownObjID=TT.VoucherBillID AND AOB.BreakdownType=2 AND AOB.AccountingSessionID=@SessionID),0),
				Debit_VBAmount=ISNULL((SELECT SUM(VBT.Amount) FROM View_VoucherBillTransaction AS VBT WHERE VBT.CurrencyID=@CurrencyID AND VBT.VoucherBillID=TT.VoucherBillID AND  VBT.IsDr=1 AND  CONVERT(DATE, CONVERT(VARCHAR(12),VBT.TransactionDate,106)) BETWEEN CONVERT(DATE, CONVERT(VARCHAR(12),@SessionStartDate,106)) AND DATEADD(DAY,-1,CONVERT(DATE, CONVERT(VARCHAR(12),@StartDate,106))) AND VBT.BUID=@BusinessUnitID AND ISNULL(VBT.CCID,0)=0 AND VBT.AccountHeadID=@AccountHeadID),0),
				Credit_VBAmount=ISNULL((SELECT SUM(VBT.Amount) FROM View_VoucherBillTransaction AS VBT WHERE VBT.CurrencyID=@CurrencyID AND VBT.VoucherBillID=TT.VoucherBillID AND  VBT.IsDr=0 AND  CONVERT(DATE, CONVERT(VARCHAR(12),VBT.TransactionDate,106)) BETWEEN CONVERT(DATE, CONVERT(VARCHAR(12),@SessionStartDate,106)) AND DATEADD(DAY,-1,CONVERT(DATE, CONVERT(VARCHAR(12),@StartDate,106))) AND VBT.BUID=@BusinessUnitID AND ISNULL(VBT.CCID,0)=0 AND VBT.AccountHeadID=@AccountHeadID),0)
			FROM #Temp_Table AS TT WHERE TT.BreakdownType=2--BILL

			UPDATE #Temp_Table
			SET Debit_VPSessionAmount=ISNULL((SELECT SUM(AOB.AmountInCurrency) FROM AccountOpenningBreakdown AS AOB WHERE AOB.BusinessUnitID=@BusinessUnitID AND AOB.IsDr=1 AND AOB.CurrencyID=@CurrencyID AND AOB.BreakdownObjID=TT.ProductID AND AOB.AccountHeadID=@AccountHeadID AND AOB.AccountingSessionID=@SessionID AND AOB.BreakdownType=4),0),
				Credit_VPSessionAmount=ISNULL((SELECT SUM(AOB.AmountInCurrency) FROM AccountOpenningBreakdown AS AOB WHERE AOB.BusinessUnitID=@BusinessUnitID AND AOB.IsDr=0 AND AOB.CurrencyID=@CurrencyID AND AOB.BreakdownObjID=TT.ProductID AND AOB.AccountHeadID=@AccountHeadID AND AOB.AccountingSessionID=@SessionID AND AOB.BreakdownType=4),0),
				Debit_VPAmount=ISNULL((SELECT SUM(VPT.Amount) FROM View_VPTransaction AS VPT WHERE VPT.BUID=@BusinessUnitID AND VPT.CurrencyID=@CurrencyID AND  VPT.IsDr=1 AND VPT.ProductID=TT.ProductID AND VPT.AccountHeadID=@AccountHeadID AND CONVERT(DATE, CONVERT(VARCHAR(12),VPT.TransactionDate,106)) BETWEEN  CONVERT(DATE, CONVERT(VARCHAR(12),@SessionStartDate,106)) AND CONVERT(DATE, CONVERT(VARCHAR(12),DATEADD(DAY,-1,@StartDate),106))),0),
				Credit_VPAmount=ISNULL((SELECT SUM(VPT.Amount) FROM View_VPTransaction AS VPT WHERE VPT.BUID=@BusinessUnitID AND VPT.CurrencyID=@CurrencyID AND VPT.IsDr=0 AND VPT.ProductID=TT.ProductID AND VPT.AccountHeadID=@AccountHeadID AND CONVERT(DATE, CONVERT(VARCHAR(12),VPT.TransactionDate,106)) BETWEEN  CONVERT(DATE, CONVERT(VARCHAR(12),@SessionStartDate,106)) AND CONVERT(DATE, CONVERT(VARCHAR(12),DATEADD(DAY,-1,@StartDate),106))),0)
			FROM #Temp_Table AS TT WHERE TT.BreakdownType=4--ITEM

			UPDATE #Temp_Table
			SET Debit_VOSessionAmount=0,--ISNULL((SELECT SUM(AOB.AmountInCurrency) FROM AccountOpenningBreakdown AS AOB WHERE AOB.BusinessUnitID=@BusinessUnitID AND AOB.IsDr=1 AND AOB.CurrencyID=@CurrencyID AND AOB.BreakdownObjID=TT.ProductID AND AOB.AccountHeadID=@AccountHeadID AND AOB.AccountingSessionID=@SessionID AND AOB.BreakdownType=4),0),
				Credit_VOSessionAmount=0,--ISNULL((SELECT SUM(AOB.AmountInCurrency) FROM AccountOpenningBreakdown AS AOB WHERE AOB.BusinessUnitID=@BusinessUnitID AND AOB.IsDr=0 AND AOB.CurrencyID=@CurrencyID AND AOB.BreakdownObjID=TT.ProductID AND AOB.AccountHeadID=@AccountHeadID AND AOB.AccountingSessionID=@SessionID AND AOB.BreakdownType=4),0),
				Debit_VOAmount=ISNULL((SELECT SUM(VOR.Amount) FROM View_VOReference AS VOR WHERE VOR.BUID=@BusinessUnitID AND VOR.CurrencyID=@CurrencyID AND  VOR.IsDebit=1 AND VOR.OrderID=TT.OrderID AND VOR.AccountHeadID=@AccountHeadID AND CONVERT(DATE, CONVERT(VARCHAR(12),VOR.TransactionDate,106)) BETWEEN  CONVERT(DATE, CONVERT(VARCHAR(12),@SessionStartDate,106)) AND CONVERT(DATE, CONVERT(VARCHAR(12),DATEADD(DAY,-1,@StartDate),106))),0),
				Credit_VOAmount=ISNULL((SELECT SUM(VOR.Amount) FROM View_VOReference AS VOR WHERE VOR.BUID=@BusinessUnitID AND VOR.CurrencyID=@CurrencyID AND VOR.IsDebit=0 AND VOR.OrderID=TT.OrderID AND VOR.AccountHeadID=@AccountHeadID AND CONVERT(DATE, CONVERT(VARCHAR(12),VOR.TransactionDate,106)) BETWEEN  CONVERT(DATE, CONVERT(VARCHAR(12),@SessionStartDate,106)) AND CONVERT(DATE, CONVERT(VARCHAR(12),DATEADD(DAY,-1,@StartDate),106))),0)
			FROM #Temp_Table AS TT WHERE TT.BreakdownType=7--ORDER
		END
	END
END
ELSE
BEGIN
	IF(@IsApproved=1)
	BEGIN
		IF(@CurrencyID=@BaseCurrencyID)
		BEGIN
			UPDATE #Temp_Table
			SET Debit_CCSessionAmount=ISNULL((SELECT SUM(AOB.OpenningBalance) FROM AccountOpenningBreakdown AS AOB WHERE AOB.IsDr=1 AND AOB.BreakdownObjID=TT.CCID AND AOB.AccountHeadID=TT.AccountHeadID AND AOB.AccountingSessionID=@SessionID AND AOB.BreakdownType=1),0),
				Credit_CCSessionAmount=ISNULL((SELECT SUM(AOB.OpenningBalance) FROM AccountOpenningBreakdown AS AOB WHERE AOB.IsDr=0 AND AOB.BreakdownObjID=TT.CCID AND AOB.AccountHeadID=TT.AccountHeadID AND AOB.AccountingSessionID=@SessionID AND AOB.BreakdownType=1),0),
				Debit_CCAmount=ISNULL((SELECT SUM(CCT.Amount*CCT.CurrencyConversionRate) FROM View_CostCenterTransaction AS CCT WHERE ISNULL(CCT.ApprovedBy,0)!=0 AND  CCT.IsDr=1 AND CCT.CCID=TT.CCID AND CCT.AccountHeadID=TT.AccountHeadID AND CONVERT(DATE, CONVERT(VARCHAR(12),CCT.TransactionDate,106)) BETWEEN  CONVERT(DATE, CONVERT(VARCHAR(12),@SessionStartDate,106)) AND CONVERT(DATE, CONVERT(VARCHAR(12),DATEADD(DAY,-1,@StartDate),106))),0),
				Credit_CCAmount=ISNULL((SELECT SUM(CCT.Amount*CCT.CurrencyConversionRate) FROM View_CostCenterTransaction AS CCT WHERE ISNULL(CCT.ApprovedBy,0)!=0 AND CCT.IsDr=0 AND CCT.CCID=TT.CCID AND CCT.AccountHeadID=TT.AccountHeadID AND CONVERT(DATE, CONVERT(VARCHAR(12),CCT.TransactionDate,106)) BETWEEN  CONVERT(DATE, CONVERT(VARCHAR(12),@SessionStartDate,106)) AND CONVERT(DATE, CONVERT(VARCHAR(12),DATEADD(DAY,-1,@StartDate),106))),0)
			FROM #Temp_Table AS TT WHERE TT.BreakdownType=1--CC

			UPDATE #Temp_Table
			SET Debit_VBSessionAmount=ISNULL((SELECT SUM(AOB.OpenningBalance) FROM AccountOpenningBreakdown AS AOB WHERE AOB.IsDr=1 AND AOB.CCID=0 AND AOB.AccountHeadID=@AccountHeadID AND AOB.BreakdownObjID=TT.VoucherBillID AND AOB.BreakdownType=2 AND AOB.AccountingSessionID=@SessionID),0),
				Credit_VBSessionAmount=ISNULL((SELECT SUM(AOB.OpenningBalance) FROM AccountOpenningBreakdown AS AOB WHERE AOB.IsDr=0 AND AOB.CCID=0 AND AOB.AccountHeadID=@AccountHeadID AND AOB.BreakdownObjID=TT.VoucherBillID AND AOB.BreakdownType=2 AND AOB.AccountingSessionID=@SessionID),0),
				Debit_VBAmount=ISNULL((SELECT SUM(VBT.Amount*VBT.ConversionRate) FROM View_VoucherBillTransaction AS VBT WHERE VBT.VoucherBillID=TT.VoucherBillID AND  VBT.IsDr=1 AND  CONVERT(DATE, CONVERT(VARCHAR(12),VBT.TransactionDate,106)) BETWEEN CONVERT(DATE, CONVERT(VARCHAR(12),@SessionStartDate,106)) AND DATEADD(DAY,-1,CONVERT(DATE, CONVERT(VARCHAR(12),@StartDate,106))) AND ISNULL(VBT.CCID,0)=0 AND VBT.AccountHeadID=@AccountHeadID AND ISNULL(VBT.ApprovedBy,0)!=0),0),
				Credit_VBAmount=ISNULL((SELECT SUM(VBT.Amount*VBT.ConversionRate) FROM View_VoucherBillTransaction AS VBT WHERE VBT.VoucherBillID=TT.VoucherBillID AND  VBT.IsDr=0 AND  CONVERT(DATE, CONVERT(VARCHAR(12),VBT.TransactionDate,106)) BETWEEN CONVERT(DATE, CONVERT(VARCHAR(12),@SessionStartDate,106)) AND DATEADD(DAY,-1,CONVERT(DATE, CONVERT(VARCHAR(12),@StartDate,106))) AND ISNULL(VBT.CCID,0)=0 AND VBT.AccountHeadID=@AccountHeadID AND ISNULL(VBT.ApprovedBy,0)!=0),0)
			FROM #Temp_Table AS TT WHERE TT.BreakdownType=2--BILL

			UPDATE #Temp_Table
			SET Debit_VPSessionAmount=ISNULL((SELECT SUM(AOB.OpenningBalance) FROM AccountOpenningBreakdown AS AOB WHERE AOB.IsDr=1 AND AOB.BreakdownObjID=TT.ProductID AND AOB.AccountHeadID=@AccountHeadID AND AOB.AccountingSessionID=@SessionID AND AOB.BreakdownType=4),0),
				Credit_VPSessionAmount=ISNULL((SELECT SUM(AOB.OpenningBalance) FROM AccountOpenningBreakdown AS AOB WHERE AOB.IsDr=0 AND AOB.BreakdownObjID=TT.ProductID AND AOB.AccountHeadID=@AccountHeadID AND AOB.AccountingSessionID=@SessionID AND AOB.BreakdownType=4),0),
				Debit_VPAmount=ISNULL((SELECT SUM(VPT.Amount*VPT.ConversionRate) FROM View_VPTransaction AS VPT WHERE ISNULL(VPT.ApprovedBy,0)!=0 AND  VPT.IsDr=1 AND VPT.ProductID=TT.ProductID AND VPT.AccountHeadID=@AccountHeadID AND CONVERT(DATE, CONVERT(VARCHAR(12),VPT.TransactionDate,106)) BETWEEN  CONVERT(DATE, CONVERT(VARCHAR(12),@SessionStartDate,106)) AND CONVERT(DATE, CONVERT(VARCHAR(12),DATEADD(DAY,-1,@StartDate),106))),0),
				Credit_VPAmount=ISNULL((SELECT SUM(VPT.Amount*VPT.ConversionRate) FROM View_VPTransaction AS VPT WHERE ISNULL(VPT.ApprovedBy,0)!=0 AND VPT.IsDr=0 AND VPT.ProductID=TT.ProductID AND VPT.AccountHeadID=@AccountHeadID AND CONVERT(DATE, CONVERT(VARCHAR(12),VPT.TransactionDate,106)) BETWEEN  CONVERT(DATE, CONVERT(VARCHAR(12),@SessionStartDate,106)) AND CONVERT(DATE, CONVERT(VARCHAR(12),DATEADD(DAY,-1,@StartDate),106))),0)
			FROM #Temp_Table AS TT WHERE TT.BreakdownType=4--ITEM

			UPDATE #Temp_Table
			SET Debit_VOSessionAmount=0,--ISNULL((SELECT SUM(AOB.AmountInCurrency) FROM AccountOpenningBreakdown AS AOB WHERE AOB.BusinessUnitID=@BusinessUnitID AND AOB.IsDr=1 AND AOB.CurrencyID=@CurrencyID AND AOB.BreakdownObjID=TT.ProductID AND AOB.AccountHeadID=@AccountHeadID AND AOB.AccountingSessionID=@SessionID AND AOB.BreakdownType=4),0),
				Credit_VOSessionAmount=0,--ISNULL((SELECT SUM(AOB.AmountInCurrency) FROM AccountOpenningBreakdown AS AOB WHERE AOB.BusinessUnitID=@BusinessUnitID AND AOB.IsDr=0 AND AOB.CurrencyID=@CurrencyID AND AOB.BreakdownObjID=TT.ProductID AND AOB.AccountHeadID=@AccountHeadID AND AOB.AccountingSessionID=@SessionID AND AOB.BreakdownType=4),0),
				Debit_VOAmount=ISNULL((SELECT SUM(VOR.Amount*VOR.ConversionRate) FROM View_VOReference AS VOR WHERE ISNULL(VOR.ApprovedBy,0)!=0 AND  VOR.IsDebit=1 AND VOR.OrderID=TT.OrderID AND VOR.AccountHeadID=@AccountHeadID AND CONVERT(DATE, CONVERT(VARCHAR(12),VOR.TransactionDate,106)) BETWEEN  CONVERT(DATE, CONVERT(VARCHAR(12),@SessionStartDate,106)) AND CONVERT(DATE, CONVERT(VARCHAR(12),DATEADD(DAY,-1,@StartDate),106))),0),
				Credit_VOAmount=ISNULL((SELECT SUM(VOR.Amount*VOR.ConversionRate) FROM View_VOReference AS VOR WHERE ISNULL(VOR.ApprovedBy,0)!=0 AND VOR.IsDebit=0 AND VOR.OrderID=TT.OrderID AND VOR.AccountHeadID=@AccountHeadID AND CONVERT(DATE, CONVERT(VARCHAR(12),VOR.TransactionDate,106)) BETWEEN  CONVERT(DATE, CONVERT(VARCHAR(12),@SessionStartDate,106)) AND CONVERT(DATE, CONVERT(VARCHAR(12),DATEADD(DAY,-1,@StartDate),106))),0)
			FROM #Temp_Table AS TT WHERE TT.BreakdownType=7--ORDER
		END
		ELSE
		BEGIN
			UPDATE #Temp_Table
			SET Debit_CCSessionAmount=ISNULL((SELECT SUM(AOB.AmountInCurrency) FROM AccountOpenningBreakdown AS AOB WHERE AOB.IsDr=1 AND AOB.CurrencyID=@CurrencyID AND AOB.BreakdownObjID=TT.CCID AND AOB.AccountHeadID=TT.AccountHeadID AND AOB.AccountingSessionID=@SessionID AND AOB.BreakdownType=1),0),
				Credit_CCSessionAmount=ISNULL((SELECT SUM(AOB.AmountInCurrency) FROM AccountOpenningBreakdown AS AOB WHERE AOB.IsDr=0 AND AOB.CurrencyID=@CurrencyID AND AOB.BreakdownObjID=TT.CCID AND AOB.AccountHeadID=TT.AccountHeadID AND AOB.AccountingSessionID=@SessionID AND AOB.BreakdownType=1),0),
				Debit_CCAmount=ISNULL((SELECT SUM(CCT.Amount) FROM View_CostCenterTransaction AS CCT WHERE ISNULL(CCT.ApprovedBy,0)!=0 AND CCT.CurrencyID=@CurrencyID AND  CCT.IsDr=1 AND CCT.CCID=TT.CCID AND CCT.AccountHeadID=TT.AccountHeadID AND CONVERT(DATE, CONVERT(VARCHAR(12),CCT.TransactionDate,106)) BETWEEN  CONVERT(DATE, CONVERT(VARCHAR(12),@SessionStartDate,106)) AND CONVERT(DATE, CONVERT(VARCHAR(12),DATEADD(DAY,-1,@StartDate),106))),0),
				Credit_CCAmount=ISNULL((SELECT SUM(CCT.Amount) FROM View_CostCenterTransaction AS CCT WHERE ISNULL(CCT.ApprovedBy,0)!=0 AND CCT.CurrencyID=@CurrencyID AND CCT.IsDr=0 AND CCT.CCID=TT.CCID AND CCT.AccountHeadID=TT.AccountHeadID AND CONVERT(DATE, CONVERT(VARCHAR(12),CCT.TransactionDate,106)) BETWEEN  CONVERT(DATE, CONVERT(VARCHAR(12),@SessionStartDate,106)) AND CONVERT(DATE, CONVERT(VARCHAR(12),DATEADD(DAY,-1,@StartDate),106))),0)
			FROM #Temp_Table AS TT WHERE TT.BreakdownType=1--CC

			UPDATE #Temp_Table
			SET Debit_VBSessionAmount=ISNULL((SELECT SUM(AOB.AmountInCurrency) FROM AccountOpenningBreakdown AS AOB WHERE AOB.IsDr=1 AND AOB.CurrencyID=@CurrencyID AND AOB.CCID=0 AND AOB.AccountHeadID=@AccountHeadID AND AOB.BreakdownObjID=TT.VoucherBillID AND AOB.BreakdownType=2 AND AOB.AccountingSessionID=@SessionID),0),
				Credit_VBSessionAmount=ISNULL((SELECT SUM(AOB.AmountInCurrency) FROM AccountOpenningBreakdown AS AOB WHERE AOB.IsDr=0 AND AOB.CurrencyID=@CurrencyID AND AOB.CCID=0 AND AOB.AccountHeadID=@AccountHeadID AND AOB.BreakdownObjID=TT.VoucherBillID AND AOB.BreakdownType=2 AND AOB.AccountingSessionID=@SessionID),0),
				Debit_VBAmount=ISNULL((SELECT SUM(VBT.Amount) FROM View_VoucherBillTransaction AS VBT WHERE VBT.CurrencyID=@CurrencyID AND VBT.VoucherBillID=TT.VoucherBillID AND  VBT.IsDr=1 AND  CONVERT(DATE, CONVERT(VARCHAR(12),VBT.TransactionDate,106)) BETWEEN CONVERT(DATE, CONVERT(VARCHAR(12),@SessionStartDate,106)) AND DATEADD(DAY,-1,CONVERT(DATE, CONVERT(VARCHAR(12),@StartDate,106))) AND ISNULL(VBT.CCID,0)=0 AND VBT.AccountHeadID=@AccountHeadID AND ISNULL(VBT.ApprovedBy,0)!=0),0),
				Credit_VBAmount=ISNULL((SELECT SUM(VBT.Amount) FROM View_VoucherBillTransaction AS VBT WHERE VBT.CurrencyID=@CurrencyID AND VBT.VoucherBillID=TT.VoucherBillID AND  VBT.IsDr=0 AND  CONVERT(DATE, CONVERT(VARCHAR(12),VBT.TransactionDate,106)) BETWEEN CONVERT(DATE, CONVERT(VARCHAR(12),@SessionStartDate,106)) AND DATEADD(DAY,-1,CONVERT(DATE, CONVERT(VARCHAR(12),@StartDate,106))) AND ISNULL(VBT.CCID,0)=0 AND VBT.AccountHeadID=@AccountHeadID AND ISNULL(VBT.ApprovedBy,0)!=0),0)
			FROM #Temp_Table AS TT WHERE TT.BreakdownType=2--BILL

			UPDATE #Temp_Table
			SET Debit_VPSessionAmount=ISNULL((SELECT SUM(AOB.AmountInCurrency) FROM AccountOpenningBreakdown AS AOB WHERE AOB.IsDr=1 AND AOB.CurrencyID=@CurrencyID AND AOB.BreakdownObjID=TT.ProductID AND AOB.AccountHeadID=@AccountHeadID AND AOB.AccountingSessionID=@SessionID AND AOB.BreakdownType=4),0),
				Credit_VPSessionAmount=ISNULL((SELECT SUM(AOB.AmountInCurrency) FROM AccountOpenningBreakdown AS AOB WHERE AOB.IsDr=0 AND AOB.CurrencyID=@CurrencyID AND AOB.BreakdownObjID=TT.ProductID AND AOB.AccountHeadID=@AccountHeadID AND AOB.AccountingSessionID=@SessionID AND AOB.BreakdownType=4),0),
				Debit_VPAmount=ISNULL((SELECT SUM(VPT.Amount) FROM View_VPTransaction AS VPT WHERE ISNULL(VPT.ApprovedBy,0)!=0 AND VPT.CurrencyID=@CurrencyID AND  VPT.IsDr=1 AND VPT.ProductID=TT.ProductID AND VPT.AccountHeadID=@AccountHeadID AND CONVERT(DATE, CONVERT(VARCHAR(12),VPT.TransactionDate,106)) BETWEEN  CONVERT(DATE, CONVERT(VARCHAR(12),@SessionStartDate,106)) AND CONVERT(DATE, CONVERT(VARCHAR(12),DATEADD(DAY,-1,@StartDate),106))),0),
				Credit_VPAmount=ISNULL((SELECT SUM(VPT.Amount) FROM View_VPTransaction AS VPT WHERE ISNULL(VPT.ApprovedBy,0)!=0 AND VPT.CurrencyID=@CurrencyID AND VPT.IsDr=0 AND VPT.ProductID=TT.ProductID AND VPT.AccountHeadID=@AccountHeadID AND CONVERT(DATE, CONVERT(VARCHAR(12),VPT.TransactionDate,106)) BETWEEN  CONVERT(DATE, CONVERT(VARCHAR(12),@SessionStartDate,106)) AND CONVERT(DATE, CONVERT(VARCHAR(12),DATEADD(DAY,-1,@StartDate),106))),0)
			FROM #Temp_Table AS TT WHERE TT.BreakdownType=4--ITEM

			UPDATE #Temp_Table
			SET Debit_VOSessionAmount=0,--ISNULL((SELECT SUM(AOB.AmountInCurrency) FROM AccountOpenningBreakdown AS AOB WHERE AOB.BusinessUnitID=@BusinessUnitID AND AOB.IsDr=1 AND AOB.CurrencyID=@CurrencyID AND AOB.BreakdownObjID=TT.ProductID AND AOB.AccountHeadID=@AccountHeadID AND AOB.AccountingSessionID=@SessionID AND AOB.BreakdownType=4),0),
				Credit_VOSessionAmount=0,--ISNULL((SELECT SUM(AOB.AmountInCurrency) FROM AccountOpenningBreakdown AS AOB WHERE AOB.BusinessUnitID=@BusinessUnitID AND AOB.IsDr=0 AND AOB.CurrencyID=@CurrencyID AND AOB.BreakdownObjID=TT.ProductID AND AOB.AccountHeadID=@AccountHeadID AND AOB.AccountingSessionID=@SessionID AND AOB.BreakdownType=4),0),
				Debit_VOAmount=ISNULL((SELECT SUM(VOR.Amount) FROM View_VOReference AS VOR WHERE ISNULL(VOR.ApprovedBy,0)!=0 AND VOR.CurrencyID=@CurrencyID AND  VOR.IsDebit=1 AND VOR.OrderID=TT.OrderID AND VOR.AccountHeadID=@AccountHeadID AND CONVERT(DATE, CONVERT(VARCHAR(12),VOR.TransactionDate,106)) BETWEEN  CONVERT(DATE, CONVERT(VARCHAR(12),@SessionStartDate,106)) AND CONVERT(DATE, CONVERT(VARCHAR(12),DATEADD(DAY,-1,@StartDate),106))),0),
				Credit_VOAmount=ISNULL((SELECT SUM(VOR.Amount) FROM View_VOReference AS VOR WHERE ISNULL(VOR.ApprovedBy,0)!=0 AND VOR.CurrencyID=@CurrencyID AND VOR.IsDebit=0 AND VOR.OrderID=TT.OrderID AND VOR.AccountHeadID=@AccountHeadID AND CONVERT(DATE, CONVERT(VARCHAR(12),VOR.TransactionDate,106)) BETWEEN  CONVERT(DATE, CONVERT(VARCHAR(12),@SessionStartDate,106)) AND CONVERT(DATE, CONVERT(VARCHAR(12),DATEADD(DAY,-1,@StartDate),106))),0)
			FROM #Temp_Table AS TT WHERE TT.BreakdownType=7--ORDER
		END
	END
	ELSE
	BEGIN
		IF(@CurrencyID=@BaseCurrencyID)
		BEGIN
			UPDATE #Temp_Table
			SET Debit_CCSessionAmount=ISNULL((SELECT SUM(AOB.OpenningBalance) FROM AccountOpenningBreakdown AS AOB WHERE AOB.IsDr=1 AND AOB.BreakdownObjID=TT.CCID AND AOB.AccountHeadID=TT.AccountHeadID AND AOB.AccountingSessionID=@SessionID AND AOB.BreakdownType=1),0),
				Credit_CCSessionAmount=ISNULL((SELECT SUM(AOB.OpenningBalance) FROM AccountOpenningBreakdown AS AOB WHERE AOB.IsDr=0 AND AOB.BreakdownObjID=TT.CCID AND AOB.AccountHeadID=TT.AccountHeadID AND AOB.AccountingSessionID=@SessionID AND AOB.BreakdownType=1),0),
				Debit_CCAmount=ISNULL((SELECT SUM(CCT.Amount*CCT.CurrencyConversionRate) FROM View_CostCenterTransaction AS CCT WHERE CCT.IsDr=1 AND CCT.CCID=TT.CCID AND CCT.AccountHeadID=TT.AccountHeadID AND CONVERT(DATE, CONVERT(VARCHAR(12),CCT.TransactionDate,106)) BETWEEN  CONVERT(DATE, CONVERT(VARCHAR(12),@SessionStartDate,106)) AND CONVERT(DATE, CONVERT(VARCHAR(12),DATEADD(DAY,-1,@StartDate),106))),0),
				Credit_CCAmount=ISNULL((SELECT SUM(CCT.Amount*CCT.CurrencyConversionRate) FROM View_CostCenterTransaction AS CCT WHERE CCT.IsDr=0 AND CCT.CCID=TT.CCID AND CCT.AccountHeadID=TT.AccountHeadID AND CONVERT(DATE, CONVERT(VARCHAR(12),CCT.TransactionDate,106)) BETWEEN  CONVERT(DATE, CONVERT(VARCHAR(12),@SessionStartDate,106)) AND CONVERT(DATE, CONVERT(VARCHAR(12),DATEADD(DAY,-1,@StartDate),106))),0)
			FROM #Temp_Table AS TT WHERE TT.BreakdownType=1--CC

			UPDATE #Temp_Table
			SET Debit_VBSessionAmount=ISNULL((SELECT SUM(AOB.OpenningBalance) FROM AccountOpenningBreakdown AS AOB WHERE AOB.IsDr=1 AND AOB.CCID=0 AND AOB.AccountHeadID=@AccountHeadID AND AOB.BreakdownObjID=TT.VoucherBillID AND AOB.BreakdownType=2 AND AOB.AccountingSessionID=@SessionID),0),
				Credit_VBSessionAmount=ISNULL((SELECT SUM(AOB.OpenningBalance) FROM AccountOpenningBreakdown AS AOB WHERE AOB.IsDr=0 AND AOB.CCID=0 AND AOB.AccountHeadID=@AccountHeadID AND AOB.BreakdownObjID=TT.VoucherBillID AND AOB.BreakdownType=2 AND AOB.AccountingSessionID=@SessionID),0),
				Debit_VBAmount=ISNULL((SELECT SUM(VBT.Amount*VBT.ConversionRate) FROM View_VoucherBillTransaction AS VBT WHERE VBT.VoucherBillID=TT.VoucherBillID AND  VBT.IsDr=1 AND  CONVERT(DATE, CONVERT(VARCHAR(12),VBT.TransactionDate,106)) BETWEEN CONVERT(DATE, CONVERT(VARCHAR(12),@SessionStartDate,106)) AND DATEADD(DAY,-1,CONVERT(DATE, CONVERT(VARCHAR(12),@StartDate,106))) AND ISNULL(VBT.CCID,0)=0 AND VBT.AccountHeadID=@AccountHeadID),0),
				Credit_VBAmount=ISNULL((SELECT SUM(VBT.Amount*VBT.ConversionRate) FROM View_VoucherBillTransaction AS VBT WHERE VBT.VoucherBillID=TT.VoucherBillID AND  VBT.IsDr=0 AND  CONVERT(DATE, CONVERT(VARCHAR(12),VBT.TransactionDate,106)) BETWEEN CONVERT(DATE, CONVERT(VARCHAR(12),@SessionStartDate,106)) AND DATEADD(DAY,-1,CONVERT(DATE, CONVERT(VARCHAR(12),@StartDate,106))) AND ISNULL(VBT.CCID,0)=0 AND VBT.AccountHeadID=@AccountHeadID),0)
			FROM #Temp_Table AS TT WHERE TT.BreakdownType=2--BILL

			UPDATE #Temp_Table
			SET Debit_VPSessionAmount=ISNULL((SELECT SUM(AOB.OpenningBalance) FROM AccountOpenningBreakdown AS AOB WHERE AOB.IsDr=1 AND AOB.BreakdownObjID=TT.ProductID AND AOB.AccountHeadID=@AccountHeadID AND AOB.AccountingSessionID=@SessionID AND AOB.BreakdownType=4),0),
				Credit_VPSessionAmount=ISNULL((SELECT SUM(AOB.OpenningBalance) FROM AccountOpenningBreakdown AS AOB WHERE AOB.IsDr=0 AND AOB.BreakdownObjID=TT.ProductID AND AOB.AccountHeadID=@AccountHeadID AND AOB.AccountingSessionID=@SessionID AND AOB.BreakdownType=4),0),
				Debit_VPAmount=ISNULL((SELECT SUM(VPT.Amount*VPT.ConversionRate) FROM View_VPTransaction AS VPT WHERE VPT.IsDr=1 AND VPT.ProductID=TT.ProductID AND VPT.AccountHeadID=@AccountHeadID AND CONVERT(DATE, CONVERT(VARCHAR(12),VPT.TransactionDate,106)) BETWEEN  CONVERT(DATE, CONVERT(VARCHAR(12),@SessionStartDate,106)) AND CONVERT(DATE, CONVERT(VARCHAR(12),DATEADD(DAY,-1,@StartDate),106))),0),
				Credit_VPAmount=ISNULL((SELECT SUM(VPT.Amount*VPT.ConversionRate) FROM View_VPTransaction AS VPT WHERE VPT.IsDr=0 AND VPT.ProductID=TT.ProductID AND VPT.AccountHeadID=@AccountHeadID AND CONVERT(DATE, CONVERT(VARCHAR(12),VPT.TransactionDate,106)) BETWEEN  CONVERT(DATE, CONVERT(VARCHAR(12),@SessionStartDate,106)) AND CONVERT(DATE, CONVERT(VARCHAR(12),DATEADD(DAY,-1,@StartDate),106))),0)
			FROM #Temp_Table AS TT WHERE TT.BreakdownType=4--ITEM

			UPDATE #Temp_Table
			SET Debit_VOSessionAmount=0,--ISNULL((SELECT SUM(AOB.AmountInCurrency) FROM AccountOpenningBreakdown AS AOB WHERE AOB.BusinessUnitID=@BusinessUnitID AND AOB.IsDr=1 AND AOB.CurrencyID=@CurrencyID AND AOB.BreakdownObjID=TT.ProductID AND AOB.AccountHeadID=@AccountHeadID AND AOB.AccountingSessionID=@SessionID AND AOB.BreakdownType=4),0),
				Credit_VOSessionAmount=0,--ISNULL((SELECT SUM(AOB.AmountInCurrency) FROM AccountOpenningBreakdown AS AOB WHERE AOB.BusinessUnitID=@BusinessUnitID AND AOB.IsDr=0 AND AOB.CurrencyID=@CurrencyID AND AOB.BreakdownObjID=TT.ProductID AND AOB.AccountHeadID=@AccountHeadID AND AOB.AccountingSessionID=@SessionID AND AOB.BreakdownType=4),0),
				Debit_VOAmount=ISNULL((SELECT SUM(VOR.Amount*VOR.ConversionRate) FROM View_VOReference AS VOR WHERE VOR.IsDebit=1 AND VOR.OrderID=TT.OrderID AND VOR.AccountHeadID=@AccountHeadID AND CONVERT(DATE, CONVERT(VARCHAR(12),VOR.TransactionDate,106)) BETWEEN  CONVERT(DATE, CONVERT(VARCHAR(12),@SessionStartDate,106)) AND CONVERT(DATE, CONVERT(VARCHAR(12),DATEADD(DAY,-1,@StartDate),106))),0),
				Credit_VOAmount=ISNULL((SELECT SUM(VOR.Amount*VOR.ConversionRate) FROM View_VOReference AS VOR WHERE VOR.IsDebit=0 AND VOR.OrderID=TT.OrderID AND VOR.AccountHeadID=@AccountHeadID AND CONVERT(DATE, CONVERT(VARCHAR(12),VOR.TransactionDate,106)) BETWEEN  CONVERT(DATE, CONVERT(VARCHAR(12),@SessionStartDate,106)) AND CONVERT(DATE, CONVERT(VARCHAR(12),DATEADD(DAY,-1,@StartDate),106))),0)
			FROM #Temp_Table AS TT WHERE TT.BreakdownType=7--ORDER
		END
		ELSE
		BEGIN
			UPDATE #Temp_Table
			SET Debit_CCSessionAmount=ISNULL((SELECT SUM(AOB.AmountInCurrency) FROM AccountOpenningBreakdown AS AOB WHERE AOB.IsDr=1 AND AOB.CurrencyID=@CurrencyID AND AOB.BreakdownObjID=TT.CCID AND AOB.AccountHeadID=TT.AccountHeadID AND AOB.AccountingSessionID=@SessionID AND AOB.BreakdownType=1),0),
				Credit_CCSessionAmount=ISNULL((SELECT SUM(AOB.AmountInCurrency) FROM AccountOpenningBreakdown AS AOB WHERE AOB.IsDr=0 AND AOB.CurrencyID=@CurrencyID AND AOB.BreakdownObjID=TT.CCID AND AOB.AccountHeadID=TT.AccountHeadID AND AOB.AccountingSessionID=@SessionID AND AOB.BreakdownType=1),0),
				Debit_CCAmount=ISNULL((SELECT SUM(CCT.Amount) FROM View_CostCenterTransaction AS CCT WHERE CCT.CurrencyID=@CurrencyID AND  CCT.IsDr=1 AND CCT.CCID=TT.CCID AND CCT.AccountHeadID=TT.AccountHeadID AND CONVERT(DATE, CONVERT(VARCHAR(12),CCT.TransactionDate,106)) BETWEEN  CONVERT(DATE, CONVERT(VARCHAR(12),@SessionStartDate,106)) AND CONVERT(DATE, CONVERT(VARCHAR(12),DATEADD(DAY,-1,@StartDate),106))),0),
				Credit_CCAmount=ISNULL((SELECT SUM(CCT.Amount) FROM View_CostCenterTransaction AS CCT WHERE CCT.CurrencyID=@CurrencyID AND CCT.IsDr=0 AND CCT.CCID=TT.CCID AND CCT.AccountHeadID=TT.AccountHeadID AND CONVERT(DATE, CONVERT(VARCHAR(12),CCT.TransactionDate,106)) BETWEEN  CONVERT(DATE, CONVERT(VARCHAR(12),@SessionStartDate,106)) AND CONVERT(DATE, CONVERT(VARCHAR(12),DATEADD(DAY,-1,@StartDate),106))),0)
			FROM #Temp_Table AS TT WHERE TT.BreakdownType=1--CC

			UPDATE #Temp_Table
			SET Debit_VBSessionAmount=ISNULL((SELECT SUM(AOB.AmountInCurrency) FROM AccountOpenningBreakdown AS AOB WHERE AOB.IsDr=1 AND AOB.CurrencyID=@CurrencyID AND AOB.CCID=0 AND AOB.AccountHeadID=@AccountHeadID AND AOB.BreakdownObjID=TT.VoucherBillID AND AOB.BreakdownType=2 AND AOB.AccountingSessionID=@SessionID),0),
				Credit_VBSessionAmount=ISNULL((SELECT SUM(AOB.AmountInCurrency) FROM AccountOpenningBreakdown AS AOB WHERE AOB.IsDr=0 AND AOB.CurrencyID=@CurrencyID AND AOB.CCID=0 AND AOB.AccountHeadID=@AccountHeadID AND AOB.BreakdownObjID=TT.VoucherBillID AND AOB.BreakdownType=2 AND AOB.AccountingSessionID=@SessionID),0),
				Debit_VBAmount=ISNULL((SELECT SUM(VBT.Amount) FROM View_VoucherBillTransaction AS VBT WHERE VBT.CurrencyID=@CurrencyID AND VBT.VoucherBillID=TT.VoucherBillID AND  VBT.IsDr=1 AND  CONVERT(DATE, CONVERT(VARCHAR(12),VBT.TransactionDate,106)) BETWEEN CONVERT(DATE, CONVERT(VARCHAR(12),@SessionStartDate,106)) AND DATEADD(DAY,-1,CONVERT(DATE, CONVERT(VARCHAR(12),@StartDate,106))) AND ISNULL(VBT.CCID,0)=0 AND VBT.AccountHeadID=@AccountHeadID),0),
				Credit_VBAmount=ISNULL((SELECT SUM(VBT.Amount) FROM View_VoucherBillTransaction AS VBT WHERE VBT.CurrencyID=@CurrencyID AND VBT.VoucherBillID=TT.VoucherBillID AND  VBT.IsDr=0 AND  CONVERT(DATE, CONVERT(VARCHAR(12),VBT.TransactionDate,106)) BETWEEN CONVERT(DATE, CONVERT(VARCHAR(12),@SessionStartDate,106)) AND DATEADD(DAY,-1,CONVERT(DATE, CONVERT(VARCHAR(12),@StartDate,106))) AND ISNULL(VBT.CCID,0)=0 AND VBT.AccountHeadID=@AccountHeadID),0)
			FROM #Temp_Table AS TT WHERE TT.BreakdownType=2--BILL

			UPDATE #Temp_Table
			SET Debit_VPSessionAmount=ISNULL((SELECT SUM(AOB.AmountInCurrency) FROM AccountOpenningBreakdown AS AOB WHERE AOB.IsDr=1 AND AOB.CurrencyID=@CurrencyID AND AOB.BreakdownObjID=TT.ProductID AND AOB.AccountHeadID=@AccountHeadID AND AOB.AccountingSessionID=@SessionID AND AOB.BreakdownType=4),0),
				Credit_VPSessionAmount=ISNULL((SELECT SUM(AOB.AmountInCurrency) FROM AccountOpenningBreakdown AS AOB WHERE AOB.IsDr=0 AND AOB.CurrencyID=@CurrencyID AND AOB.BreakdownObjID=TT.ProductID AND AOB.AccountHeadID=@AccountHeadID AND AOB.AccountingSessionID=@SessionID AND AOB.BreakdownType=4),0),
				Debit_VPAmount=ISNULL((SELECT SUM(VPT.Amount) FROM View_VPTransaction AS VPT WHERE VPT.CurrencyID=@CurrencyID AND  VPT.IsDr=1 AND VPT.ProductID=TT.ProductID AND VPT.AccountHeadID=@AccountHeadID AND CONVERT(DATE, CONVERT(VARCHAR(12),VPT.TransactionDate,106)) BETWEEN  CONVERT(DATE, CONVERT(VARCHAR(12),@SessionStartDate,106)) AND CONVERT(DATE, CONVERT(VARCHAR(12),DATEADD(DAY,-1,@StartDate),106))),0),
				Credit_VPAmount=ISNULL((SELECT SUM(VPT.Amount) FROM View_VPTransaction AS VPT WHERE VPT.CurrencyID=@CurrencyID AND VPT.IsDr=0 AND VPT.ProductID=TT.ProductID AND VPT.AccountHeadID=@AccountHeadID AND CONVERT(DATE, CONVERT(VARCHAR(12),VPT.TransactionDate,106)) BETWEEN  CONVERT(DATE, CONVERT(VARCHAR(12),@SessionStartDate,106)) AND CONVERT(DATE, CONVERT(VARCHAR(12),DATEADD(DAY,-1,@StartDate),106))),0)
			FROM #Temp_Table AS TT WHERE TT.BreakdownType=4--ITEM

			UPDATE #Temp_Table
			SET Debit_VOSessionAmount=0,--ISNULL((SELECT SUM(AOB.AmountInCurrency) FROM AccountOpenningBreakdown AS AOB WHERE AOB.BusinessUnitID=@BusinessUnitID AND AOB.IsDr=1 AND AOB.CurrencyID=@CurrencyID AND AOB.BreakdownObjID=TT.ProductID AND AOB.AccountHeadID=@AccountHeadID AND AOB.AccountingSessionID=@SessionID AND AOB.BreakdownType=4),0),
				Credit_VOSessionAmount=0,--ISNULL((SELECT SUM(AOB.AmountInCurrency) FROM AccountOpenningBreakdown AS AOB WHERE AOB.BusinessUnitID=@BusinessUnitID AND AOB.IsDr=0 AND AOB.CurrencyID=@CurrencyID AND AOB.BreakdownObjID=TT.ProductID AND AOB.AccountHeadID=@AccountHeadID AND AOB.AccountingSessionID=@SessionID AND AOB.BreakdownType=4),0),
				Debit_VOAmount=ISNULL((SELECT SUM(VOR.Amount) FROM View_VOReference AS VOR WHERE VOR.CurrencyID=@CurrencyID AND  VOR.IsDebit=1 AND VOR.OrderID=TT.OrderID AND VOR.AccountHeadID=@AccountHeadID AND CONVERT(DATE, CONVERT(VARCHAR(12),VOR.TransactionDate,106)) BETWEEN  CONVERT(DATE, CONVERT(VARCHAR(12),@SessionStartDate,106)) AND CONVERT(DATE, CONVERT(VARCHAR(12),DATEADD(DAY,-1,@StartDate),106))),0),
				Credit_VOAmount=ISNULL((SELECT SUM(VOR.Amount) FROM View_VOReference AS VOR WHERE VOR.CurrencyID=@CurrencyID AND VOR.IsDebit=0 AND VOR.OrderID=TT.OrderID AND VOR.AccountHeadID=@AccountHeadID AND CONVERT(DATE, CONVERT(VARCHAR(12),VOR.TransactionDate,106)) BETWEEN  CONVERT(DATE, CONVERT(VARCHAR(12),@SessionStartDate,106)) AND CONVERT(DATE, CONVERT(VARCHAR(12),DATEADD(DAY,-1,@StartDate),106))),0)
			FROM #Temp_Table AS TT WHERE TT.BreakdownType=7--ORDER
		END
	END
END





UPDATE #Temp_Table
SET BreakodwnID=TT.CCID,
	BreakdownName=TT.CCName,
	DebitOpeningAmount=TT.Debit_CCSessionAmount,
	CreditOpeningAmount=TT.Credit_CCSessionAmount,
	DebitTransactionAmount=TT.Debit_CCAmount,
	CreditTransactionAmount=TT.Credit_CCAmount
	--DebitAmount=TT.Debit_CCSessionAmount+TT.Debit_CCAmount,
	--CreditAmount=TT.Credit_CCSessionAmount+TT.Credit_CCAmount
FROM #Temp_Table AS TT WHERE TT.BreakdownType=1--CC


UPDATE #Temp_Table
SET BreakodwnID=TT.VoucherBillID,
	BreakdownName=TT.BillNo,
	DebitOpeningAmount=TT.Debit_VBSessionAmount,
	CreditOpeningAmount=TT.Credit_VBSessionAmount,
	DebitTransactionAmount=TT.Debit_VBAmount,
	CreditTransactionAmount=TT.Credit_VBAmount
	--DebitAmount=TT.Debit_VBSessionAmount+TT.Debit_VBAmount,
	--CreditAmount=TT.Credit_VBSessionAmount+TT.Credit_VBAmount
FROM #Temp_Table AS TT WHERE TT.BreakdownType=2--BILL


UPDATE #Temp_Table
SET	BreakodwnID=TT.ProductID,
	BreakdownName=TT.ProductName,
	DebitOpeningAmount=TT.Debit_VPSessionAmount,
	CreditOpeningAmount=TT.Credit_VPSessionAmount,
	DebitTransactionAmount=TT.Debit_VPAmount,
	CreditTransactionAmount=TT.Credit_VPAmount
	--DebitAmount=TT.Debit_VPSessionAmount+TT.Debit_VPAmount,
	--CreditAmount=TT.Credit_VPSessionAmount+TT.Credit_VPAmount
FROM #Temp_Table AS TT WHERE TT.BreakdownType=4--ITEM


UPDATE #Temp_Table
SET BreakodwnID=TT.OrderID,
	BreakdownName=TT.OrderNo,
	DebitOpeningAmount=TT.Debit_VOSessionAmount,
	CreditOpeningAmount=TT.Credit_VOSessionAmount,
	DebitTransactionAmount=TT.Debit_VOAmount,
	CreditTransactionAmount=TT.Credit_VOAmount
	--DebitAmount=TT.Debit_VOSessionAmount+TT.Debit_VOAmount,
	--CreditAmount=TT.Credit_VOSessionAmount+TT.Credit_VOAmount
FROM #Temp_Table AS TT WHERE TT.BreakdownType=7--ORDER

UPDATE #Temp_Table
SET DebitAmount=TT.DebitTransactionAmount,
	CreditAmount=TT.CreditTransactionAmount
FROM #Temp_Table AS TT

UPDATE #Temp_Table SET IsDebit=1 FROM #Temp_Table AS TT 
UPDATE #Temp_Table SET IsDebit=0 FROM #Temp_Table AS TT WHERE TT.DebitOpeningAmount<TT.CreditOpeningAmount

UPDATE #Temp_Table SET OpeningAmount=TT.DebitOpeningAmount-TT.CreditOpeningAmount, ClosingAmount=(TT.DebitOpeningAmount-TT.CreditOpeningAmount+TT.DebitAmount-TT.CreditAmount), IsDrClosing=1 FROM #Temp_Table AS TT WHERE TT.ComponentID IN (2,6)
UPDATE #Temp_Table SET IsDrClosing=0 FROM #Temp_Table AS TT WHERE TT.ClosingAmount<0 AND TT.ComponentID IN (2,6)--DEBIT

UPDATE #Temp_Table SET OpeningAmount=TT.CreditOpeningAmount-TT.DebitOpeningAmount, ClosingAmount=(TT.CreditOpeningAmount-TT.DebitOpeningAmount-TT.DebitAmount+TT.CreditAmount), IsDrClosing=0 FROM #Temp_Table AS TT WHERE TT.ComponentID NOT IN (2,6)
UPDATE #Temp_Table SET IsDrClosing=1 FROM #Temp_Table AS TT WHERE TT.ClosingAmount<0	AND TT.ComponentID NOT IN (2,6)--CREDIT




UPDATE #Temp_Table
SET AccountHeadCode=(SELECT COA.AccountCode FROM ChartsOfAccount AS COA WHERE COA.AccountHeadID=TT.AccountHeadID),
	AccountHeadName=(SELECT COA.AccountHeadName FROM ChartsOfAccount AS COA WHERE COA.AccountHeadID=TT.AccountHeadID)
FROM #Temp_Table AS TT

SELECT * FROM #Temp_Table
DROP TABLE #Temp_Table
COMMIT TRAN





GO
/****** Object:  StoredProcedure [dbo].[SP_ApprovedVoucher]    Script Date: 12/1/2016 2:17:46 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO



GO
/****** Object:  StoredProcedure [dbo].[SP_BalanceSheet]    Script Date: 12/1/2016 2:17:46 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[SP_BalanceSheet]
(	
	@BUID as int,	
	@AccountType as int,	
	@BalanceSheetUptoDate as Date,
	@ParentAccountHeadID as int
)
AS
BEGIN TRAN
--DECLARE
--@BUID as int,	
--@AccountType as int,	
----@DescriptiveReport as bit,
--@BalanceSheetUptoDate as Date
----EXEC [SP_BalanceSheet]1,'13 Mar 2014'
----SET @DescriptiveReport=1
--SET @BalanceSheetUptoDate='30 JAN 2016'
--set @AccountType=5
--SET @BUID=2

DECLARE
@BUCode as Varchar(512),
@ComponentIDs as Varchar(512),
@SessionID as int,
@SessionStartDate as date,
@SessionEndDate as date,
@CurrencyID as int

SET @BUCode=ISNULL((SELECT BU.Code FROM BusinessUnit AS BU WHERE BU.BusinessUnitID=@BUID),'00')
SET @ComponentIDs='2,3,4'
SET @CurrencyID = (SELECT TT.BaseCurrencyID FROM Company AS TT WHERE TT.CompanyID=1)
--ComponentID{Asset = 2,Laibility = 3,OwnerEquity=4,Income = 5,Expeness = 6}

IF EXISTS(SELECT ASE.SessionID FROM AccountingSession AS ASE WHERE  CONVERT(DATE,CONVERT(VARCHAR(12),ASE.StartDate,106))=CONVERT(DATE,CONVERT(VARCHAR(12),@BalanceSheetUptoDate,106)) AND ASE.SessionType=6)
BEGIN
	SET @SessionID=(SELECT top(1)ASE.SessionID FROM AccountingSession AS ASE WHERE  CONVERT(DATE,CONVERT(VARCHAR(12),ASE.StartDate,106))=CONVERT(DATE,CONVERT(VARCHAR(12),@BalanceSheetUptoDate,106)) AND ASE.SessionType=6)
END
ELSE
BEGIN
	SET @SessionID=(SELECT TOP 1 ASE.SessionID FROM AccountingSession AS ASE WHERE  CONVERT(DATE,CONVERT(VARCHAR(12),ASE.StartDate,106))>CONVERT(DATE,CONVERT(VARCHAR(12),@BalanceSheetUptoDate,106)) AND ASE.SessionType=6)	
END
SET @SessionStartDate=(SELECT ASE.StartDate FROM AccountingSession AS ASE WHERE  ASE.AccountingSessionID=@SessionID)
SET @SessionEndDate=@BalanceSheetUptoDate

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
							Sequence int							
						)

CREATE NONCLUSTERED INDEX #IX_Temp2_1 ON #Temp_Table(AccountHeadID)

INSERT INTO #Temp_Table		(	AccountHeadID,		AccountCode,			AccountHeadName,		ParentAccountHeadID,	AccountType)
					SELECT		AccountHeadID,		AccountCode,			AccountHeadName,		ParentHeadID,			AccountType FROM dbo.GetAccountHeadByComponents(@ComponentIDs)


--EnumAccountType{None = 0,Component = 1,Segment =2,Group = 3,SubGroup = 4,Ledger = 5}
--Update ledger transaction, ComponentID, Opening Balance
IF(@BUID>0)
BEGIN
	UPDATE #Temp_Table
	SET	ComponentType=dbo.GetComponentID(TT.AccountHeadID),
		OpenningBalance= ISNULL((SELECT LOB.OpenningBalance FROM dbo.[GetLedgerOpeningBalanceByBU](TT.AccountHeadID, @BalanceSheetUptoDate, @CurrencyID, 1, 0, @BUCode, '00' ) AS LOB),0),	
		DebitTransaction=(SELECT ISNULL(SUM(VD.Amount),0) FROM View_VoucherDetail AS VD  WHERE  VD.IsDebit=1 AND VD.BUID=@BUID AND VD.AccountHeadID=TT.AccountHeadID AND VD.VoucherID IN(SELECT V.VoucherID FROM Voucher AS V WHERE V.AuthorizedBy!=0 AND V.VoucherDate BETWEEN @SessionStartDate AND @SessionEndDate)),
		CreditTransaction=(SELECT ISNULL(SUM(VD.Amount),0) FROM View_VoucherDetail AS VD  WHERE  VD.IsDebit=0 AND VD.BUID=@BUID AND VD.AccountHeadID=TT.AccountHeadID AND VD.VoucherID IN(SELECT V.VoucherID FROM Voucher AS V WHERE V.AuthorizedBy!=0 AND V.VoucherDate BETWEEN @SessionStartDate AND @SessionEndDate)),
		ClosingBalance=0
	FROM #Temp_Table AS TT WHERE TT.AccountType=5
END
ELSE
BEGIN
	UPDATE #Temp_Table
	SET	ComponentType=dbo.GetComponentID(TT.AccountHeadID),
		OpenningBalance= ISNULL((SELECT LOB.OpenningBalance FROM dbo.[GetLedgerOpeningBalance](TT.AccountHeadID, @BalanceSheetUptoDate, @CurrencyID, 1, 0, 0) AS LOB),0),	
		DebitTransaction=(SELECT ISNULL(SUM(VD.Amount),0) FROM VIEW_VoucherDetail AS VD  WHERE  VD.IsDebit=1 AND VD.AccountHeadID=TT.AccountHeadID AND VD.VoucherID IN(SELECT V.VoucherID FROM Voucher AS V WHERE V.AuthorizedBy!=0 AND V.VoucherDate BETWEEN @SessionStartDate AND @SessionEndDate)),
		CreditTransaction=(SELECT ISNULL(SUM(VD.Amount),0) FROM VIEW_VoucherDetail AS VD  WHERE  VD.IsDebit=0 AND VD.AccountHeadID=TT.AccountHeadID AND VD.VoucherID IN(SELECT V.VoucherID FROM Voucher AS V WHERE V.AuthorizedBy!=0 AND V.VoucherDate BETWEEN @SessionStartDate AND @SessionEndDate)),
		ClosingBalance=0
	FROM #Temp_Table AS TT WHERE TT.AccountType=5
END

--Start Reset Net Income & Net Loss 
--14 =	P/L Appropriation A/C 
DECLARE
@NetIncome as decimal(30,17),
@TransferAmountFromNetIncome as decimal(30,17),
@TempDistributedAmount as decimal(30,17)

--PRINT CONVERT(VARCHAR,@SessionStartDate)+' ' + CONVERT(VARCHAR,@SessionEndDate)
SET @NetIncome=dbo.FN_GetIncomeStatementBalanceBU(@SessionStartDate, @SessionEndDate, @BUCode,'00')
--EnumVoucherOperationType { None = 0, FreshVoucher = 1, ProfitLossAccountVoucher = 2, DividendVoucher = 3, RetainedEarningVoucher= 4 }
IF(@BUID>0)
BEGIN
	SET @TransferAmountFromNetIncome= ISNULL((SELECT SUM(VD.Amount) FROM View_VoucherDetail AS VD WHERE  VD.IsDebit=0 AND VD.BUID=@BUID AND VD.VoucherID IN (SELECT V.VoucherID FROM Voucher AS V WHERE V.OperationType IN (3,4) AND V.VoucherDate BETWEEN  @SessionStartDate AND @SessionEndDate)),0)
END
ELSE
BEGIN
	SET @TransferAmountFromNetIncome= ISNULL((SELECT SUM(VD.Amount) FROM View_VoucherDetail AS VD WHERE  VD.IsDebit=0 AND VD.VoucherID IN (SELECT V.VoucherID FROM Voucher AS V WHERE V.OperationType IN (3,4) AND V.VoucherDate BETWEEN  @SessionStartDate AND @SessionEndDate)),0)
END

IF(@NetIncome<0)
BEGIN
	--14 =	P/L Appropriation A/C 
	SET @TempDistributedAmount = ((-1)*@NetIncome)-@TransferAmountFromNetIncome
	IF(@TempDistributedAmount<0)
	BEGIN
		SET @TempDistributedAmount=((-1)*@TempDistributedAmount)
	END
	UPDATE #Temp_Table SET DebitTransaction=@TempDistributedAmount WHERE AccountHeadID=14
	UPDATE #Temp_Table SET CreditTransaction=0 WHERE AccountHeadID=14
END
ELSE
BEGIN
	--14 =	P/L Appropriation A/C 
	SET @TempDistributedAmount = @NetIncome-@TransferAmountFromNetIncome
	IF(@TempDistributedAmount<0)
	BEGIN
		SET @TempDistributedAmount=((-1)*@TempDistributedAmount)
	END
	UPDATE #Temp_Table SET CreditTransaction=@TempDistributedAmount WHERE AccountHeadID=14
	UPDATE #Temp_Table SET DebitTransaction=0 WHERE AccountHeadID=14
END
--End Reset Net Income & Net Loss 

--SELECT * FROM #Temp_Table WHERE AccountType <= @AccountType ORDER BY AccountHeadID

--Update Ledger debit/credit Transaction
UPDATE #Temp_Table
SET ComponentType=dbo.GetComponentID(TT.AccountHeadID),
	OpenningBalance=(SELECT ISNULL(SUM(ABC.OpenningBalance),0) FROM #Temp_Table AS ABC WHERE ABC.ParentAccountHeadID=TT.AccountHeadID),
	DebitTransaction=(SELECT ISNULL(SUM(ABC.DebitTransaction),0) FROM #Temp_Table AS ABC WHERE ABC.ParentAccountHeadID=TT.AccountHeadID),
	CreditTransaction=(SELECT ISNULL(SUM(ABC.CreditTransaction),0) FROM #Temp_Table AS ABC WHERE ABC.ParentAccountHeadID=TT.AccountHeadID)
FROM #Temp_Table AS TT WHERE TT.AccountType=4 --EnumAccountType{None = 0,Component = 1,Segment =2,Group = 3,SubGroup = 4,Ledger = 5}


--Update SubGroup debit/credit Transaction
UPDATE #Temp_Table
SET ComponentType=dbo.GetComponentID(TT.AccountHeadID),
	OpenningBalance=(SELECT ISNULL(SUM(ABC.OpenningBalance),0) FROM #Temp_Table AS ABC WHERE ABC.ParentAccountHeadID=TT.AccountHeadID),
	DebitTransaction=(SELECT ISNULL(SUM(ABC.DebitTransaction),0) FROM #Temp_Table AS ABC WHERE ABC.ParentAccountHeadID=TT.AccountHeadID),
	CreditTransaction=(SELECT ISNULL(SUM(ABC.CreditTransaction),0) FROM #Temp_Table AS ABC WHERE ABC.ParentAccountHeadID=TT.AccountHeadID)
FROM #Temp_Table AS TT WHERE TT.AccountType=3 --EnumAccountType{None = 0,Component = 1,Segment =2,Group = 3,SubGroup = 4,Ledger = 5}

--Update Group debit/credit Transaction
UPDATE #Temp_Table
SET ComponentType=dbo.GetComponentID(TT.AccountHeadID),
	OpenningBalance=(SELECT ISNULL(SUM(ABC.OpenningBalance),0) FROM #Temp_Table AS ABC WHERE ABC.ParentAccountHeadID=TT.AccountHeadID),
	DebitTransaction=(SELECT ISNULL(SUM(ABC.DebitTransaction),0) FROM #Temp_Table AS ABC WHERE ABC.ParentAccountHeadID=TT.AccountHeadID),
	CreditTransaction=(SELECT ISNULL(SUM(ABC.CreditTransaction),0) FROM #Temp_Table AS ABC WHERE ABC.ParentAccountHeadID=TT.AccountHeadID)
FROM #Temp_Table AS TT WHERE TT.AccountType=2 --EnumAccountType{None = 0,Component = 1,Segment =2,Group = 3,SubGroup = 4,Ledger = 5}

--Update Component debit/credit Transaction
UPDATE #Temp_Table
SET ComponentType=dbo.GetComponentID(TT.AccountHeadID),
	OpenningBalance=(SELECT ISNULL(SUM(ABC.OpenningBalance),0) FROM #Temp_Table AS ABC WHERE ABC.ParentAccountHeadID=TT.AccountHeadID),
	DebitTransaction=(SELECT ISNULL(SUM(ABC.DebitTransaction),0) FROM #Temp_Table AS ABC WHERE ABC.ParentAccountHeadID=TT.AccountHeadID),
	CreditTransaction=(SELECT ISNULL(SUM(ABC.CreditTransaction),0) FROM #Temp_Table AS ABC WHERE ABC.ParentAccountHeadID=TT.AccountHeadID)
FROM #Temp_Table AS TT WHERE TT.AccountType=1 --EnumAccountType{None = 0,Component = 1,Segment =2,Group = 3,SubGroup = 4,Ledger = 5}

--Update Closing Balance
UPDATE #Temp_Table
set	ClosingBalance=TT.OpenningBalance+TT.DebitTransaction-TT.CreditTransaction
FROM #Temp_Table AS TT where TT.ComponentType IN(2,6)

UPDATE #Temp_Table
SET	ClosingBalance=TT.OpenningBalance-TT.DebitTransaction+TT.CreditTransaction	
FROM #Temp_Table AS TT where TT.ComponentType NOT IN(2,6)

UPDATE #Temp_Table
SET	Sequence = ISNULL((SELECT FPS.Sequence FROM FinancialPositionSetup AS FPS WHERE FPS.AccountHeadID=TT.AccountHeadID),0)
FROM #Temp_Table AS TT 
		

--EnumAccountType{None = 0,Component = 1,Segment =2,Group = 3,SubGroup = 4,Ledger = 5}
IF(ISNULL(@ParentAccountHeadID,0)=0)
BEGIN
	SELECT * FROM #Temp_Table WHERE AccountType <= @AccountType ORDER BY AccountHeadID
END
ELSE
BEGIN
	SELECT * FROM #Temp_Table WHERE AccountHeadID =@ParentAccountHeadID OR ParentAccountHeadID=@ParentAccountHeadID ORDER BY AccountHeadID
END



DROP TABLE #Temp_Table
COMMIT TRAN



GO
/****** Object:  StoredProcedure [dbo].[SP_CashFlowStatement]    Script Date: 12/1/2016 2:17:46 PM ******/
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

--EnumCFTransactionGroup : Interest_Paid = 3, EnumCFDataType : Financila_Cost_of_SCI = 3, EnumCISSetup : Income_Tax = 7
SET @TransactionAmount = (-1)*ISNULL((SELECT SUM(TT.ClosingBalance) FROM #Temp_Table AS TT WHERE TT.AccountHeadID IN (SELECT HH.AccountHeadID FROM CIStatementSetup AS HH WHERE HH.CIHeadType=10)),0) 
UPDATE #TempCFTable SET Amount = @TransactionAmount WHERE CFTransactionGroup=3


--EnumCFTransactionGroup : Income_Tax_paid = 4, EnumCFDataType : Income_Tax_of_SCI = 4, EnumCISSetup : Financial_Cost = 10 
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
UPDATE #TempCFTable SET Amount = @AdministrativeCost  WHERE CFTransactionGroup=13

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
/****** Object:  StoredProcedure [dbo].[SP_CCAccountWiseBreakdown]    Script Date: 12/1/2016 2:17:46 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


CREATE PROCEDURE [dbo].[SP_CCAccountWiseBreakdown]
(
	@AccountHeadID as int,
	@BusinessUnitID as int,
	@SubLedgerID as int,
	@CurrencyID as int,
	@StartDate date,
	@EndDate date,
	@IsApproved as bit
)
AS
BEGIN TRAN
--DECLARE
--@AccountHeadID as int,
--@BusinessUnitID as int,
--@SubLedgerID as int,
--@CurrencyID as int,
--@StartDate date,
--@EndDate date,
--@IsApproved as bit

--SET @AccountHeadID=0
--SET @BusinessUnitID =0
--SET @SubLedgerID =300
--SET @CurrencyID =0
--SET @StartDate ='01 Jan 2015'
--SET @EndDate ='03 Mar 2016'
--SET @IsApproved =0

CREATE TABLE #TempTable(
							ID int IDENTITY(1,1) PRIMARY KEY,
							CCTID INT,
							CCID int,
							CCCode Varchar(512),
							CCName Varchar(512),
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
							AccountHeadName Varchar(512),
							ParentHeadName varchar(512),
							ParentHeadCode varchar(512),
							VoucherDetailID int,
							Narration VARCHAR(512),
							VoucherNarration VARCHAR(512),
							Description VARCHAR(512),
							ComponentID int
						)

CREATE TABLE #TempTable2(
							CCID int,	
							ParentHeadID int,
							DebitOpeiningValue decimal(30,17),
							CreditOpeiningValue decimal(30,17),
							IsDebit bit,
							DebitAmount decimal(30,17),
							CreditAmount decimal(30,17),
							ClosingValue decimal(30,17),
							IsDrClosing bit,
							ComponentID int
						)

DECLARE
@SessionID as int,
@SessionStartDate as date,
@BaseCurrencyID as int

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


IF(@SubLedgerID>0)
BEGIN
	IF(@AccountHeadID>0)
	BEGIN
		IF(@BusinessUnitID>0)
		BEGIN
			IF(@IsApproved=1)
			BEGIN
				IF(@CurrencyID=@BaseCurrencyID)
				BEGIN
					INSERT INTO #TempTable (CCID,ParentHeadID,ComponentID)
					SELECT DISTINCT AOB.BreakdownObjID,AOB.AccountHeadID,dbo.GetComponentID(AOB.AccountHeadID) FROM AccountOpenningBreakdown AS AOB WHERE AOB.AccountHeadID=@AccountHeadID AND AOB.BusinessUnitID=@BusinessUnitID AND AOB.BreakdownObjID=@SubLedgerID AND AOB.AccountingSessionID=@SessionID AND AOB.BreakdownType=1 AND ISNULL(AOB.BreakdownObjID,0)!=0

					INSERT INTO #TempTable (CCID,ParentHeadID,ComponentID)
					SELECT DISTINCT CCT.CCID,CCT.AccountHeadID,dbo.GetComponentID(CCT.AccountHeadID) FROM View_CostCenterTransaction AS CCT WHERE CCT.AccountHeadID=@AccountHeadID AND CCT.BUID=@BusinessUnitID AND ISNULL(CCT.ApprovedBy,0)!=0 AND CCT.CCID=@SubLedgerID AND CONVERT(DATE, CONVERT(VARCHAR(12),CCT.TransactionDate,106)) BETWEEN  CONVERT(DATE, CONVERT(VARCHAR(12),@SessionStartDate,106)) AND CONVERT(DATE, CONVERT(VARCHAR(12),@EndDate,106)) 
				END
				ELSE
				BEGIN
					INSERT INTO #TempTable (CCID,ParentHeadID,ComponentID)
					SELECT DISTINCT AOB.BreakdownObjID,AOB.AccountHeadID,dbo.GetComponentID(AOB.AccountHeadID) FROM AccountOpenningBreakdown AS AOB WHERE AOB.AccountHeadID=@AccountHeadID AND AOB.BusinessUnitID=@BusinessUnitID AND AOB.CurrencyID=@CurrencyID AND AOB.BreakdownObjID=@SubLedgerID AND AOB.AccountingSessionID=@SessionID AND AOB.BreakdownType=1 AND ISNULL(AOB.BreakdownObjID,0)!=0

					INSERT INTO #TempTable (CCID,ParentHeadID,ComponentID)
					SELECT DISTINCT CCT.CCID,CCT.AccountHeadID,dbo.GetComponentID(CCT.AccountHeadID) FROM View_CostCenterTransaction AS CCT WHERE CCT.AccountHeadID=@AccountHeadID AND CCT.BUID=@BusinessUnitID AND ISNULL(CCT.ApprovedBy,0)!=0  AND CCT.CurrencyID=@CurrencyID AND CCT.CCID=@SubLedgerID AND CONVERT(DATE, CONVERT(VARCHAR(12),CCT.TransactionDate,106)) BETWEEN  CONVERT(DATE, CONVERT(VARCHAR(12),@SessionStartDate,106)) AND CONVERT(DATE, CONVERT(VARCHAR(12),@EndDate,106)) 
				END
			END
			ELSE
			BEGIN
				IF(@CurrencyID=@BaseCurrencyID)
				BEGIN
					INSERT INTO #TempTable (CCID,ParentHeadID,ComponentID)
					SELECT DISTINCT AOB.BreakdownObjID,AOB.AccountHeadID,dbo.GetComponentID(AOB.AccountHeadID) FROM AccountOpenningBreakdown AS AOB WHERE AOB.AccountHeadID=@AccountHeadID AND AOB.BusinessUnitID=@BusinessUnitID AND AOB.BreakdownObjID=@SubLedgerID AND AOB.AccountingSessionID=@SessionID AND AOB.BreakdownType=1 AND ISNULL(AOB.BreakdownObjID,0)!=0

					INSERT INTO #TempTable (CCID,ParentHeadID,ComponentID)
					SELECT DISTINCT CCT.CCID,CCT.AccountHeadID,dbo.GetComponentID(CCT.AccountHeadID) FROM View_CostCenterTransaction AS CCT WHERE CCT.AccountHeadID=@AccountHeadID AND CCT.BUID=@BusinessUnitID AND CCT.CCID=@SubLedgerID AND CONVERT(DATE, CONVERT(VARCHAR(12),CCT.TransactionDate,106)) BETWEEN  CONVERT(DATE, CONVERT(VARCHAR(12),@SessionStartDate,106)) AND CONVERT(DATE, CONVERT(VARCHAR(12),@EndDate,106)) 
				END
				ELSE
				BEGIN
					INSERT INTO #TempTable (CCID,ParentHeadID,ComponentID)
					SELECT DISTINCT AOB.BreakdownObjID,AOB.AccountHeadID,dbo.GetComponentID(AOB.AccountHeadID) FROM AccountOpenningBreakdown AS AOB WHERE AOB.AccountHeadID=@AccountHeadID AND AOB.BusinessUnitID=@BusinessUnitID AND AOB.CurrencyID=@CurrencyID AND AOB.BreakdownObjID=@SubLedgerID AND AOB.AccountingSessionID=@SessionID AND AOB.BreakdownType=1 AND ISNULL(AOB.BreakdownObjID,0)!=0

					INSERT INTO #TempTable (CCID,ParentHeadID,ComponentID)
					SELECT DISTINCT CCT.CCID,CCT.AccountHeadID,dbo.GetComponentID(CCT.AccountHeadID) FROM View_CostCenterTransaction AS CCT WHERE CCT.AccountHeadID=@AccountHeadID AND CCT.BUID=@BusinessUnitID AND CCT.CurrencyID=@CurrencyID AND CCT.CCID=@SubLedgerID AND CONVERT(DATE, CONVERT(VARCHAR(12),CCT.TransactionDate,106)) BETWEEN  CONVERT(DATE, CONVERT(VARCHAR(12),@SessionStartDate,106)) AND CONVERT(DATE, CONVERT(VARCHAR(12),@EndDate,106)) 
				END
			END
		END
		ELSE
		BEGIN
			IF(@IsApproved=1)
			BEGIN
				IF(@CurrencyID=@BaseCurrencyID)
				BEGIN
					INSERT INTO #TempTable (CCID,ParentHeadID,ComponentID)
					SELECT DISTINCT AOB.BreakdownObjID,AOB.AccountHeadID,dbo.GetComponentID(AOB.AccountHeadID) FROM AccountOpenningBreakdown AS AOB WHERE AOB.AccountHeadID=@AccountHeadID AND AOB.BreakdownObjID=@SubLedgerID AND AOB.AccountingSessionID=@SessionID AND AOB.BreakdownType=1 AND ISNULL(AOB.BreakdownObjID,0)!=0

					INSERT INTO #TempTable (CCID,ParentHeadID,ComponentID)
					SELECT DISTINCT CCT.CCID,CCT.AccountHeadID,dbo.GetComponentID(CCT.AccountHeadID) FROM View_CostCenterTransaction AS CCT WHERE CCT.AccountHeadID=@AccountHeadID AND ISNULL(CCT.ApprovedBy,0)!=0 AND CCT.CCID=@SubLedgerID AND CONVERT(DATE, CONVERT(VARCHAR(12),CCT.TransactionDate,106)) BETWEEN  CONVERT(DATE, CONVERT(VARCHAR(12),@SessionStartDate,106)) AND CONVERT(DATE, CONVERT(VARCHAR(12),@EndDate,106)) 
				END
				ELSE
				BEGIN
					INSERT INTO #TempTable (CCID,ParentHeadID,ComponentID)
					SELECT DISTINCT AOB.BreakdownObjID,AOB.AccountHeadID,dbo.GetComponentID(AOB.AccountHeadID) FROM AccountOpenningBreakdown AS AOB WHERE AOB.AccountHeadID=@AccountHeadID AND AOB.CurrencyID=@CurrencyID AND AOB.BreakdownObjID=@SubLedgerID AND AOB.AccountingSessionID=@SessionID AND AOB.BreakdownType=1 AND ISNULL(AOB.BreakdownObjID,0)!=0

					INSERT INTO #TempTable (CCID,ParentHeadID,ComponentID)
					SELECT DISTINCT CCT.CCID,CCT.AccountHeadID,dbo.GetComponentID(CCT.AccountHeadID) FROM View_CostCenterTransaction AS CCT WHERE CCT.AccountHeadID=@AccountHeadID AND ISNULL(CCT.ApprovedBy,0)!=0  AND CCT.CurrencyID=@CurrencyID AND CCT.CCID=@SubLedgerID AND CONVERT(DATE, CONVERT(VARCHAR(12),CCT.TransactionDate,106)) BETWEEN  CONVERT(DATE, CONVERT(VARCHAR(12),@SessionStartDate,106)) AND CONVERT(DATE, CONVERT(VARCHAR(12),@EndDate,106)) 
				END
			END
			ELSE
			BEGIN
				IF(@CurrencyID=@BaseCurrencyID)
				BEGIN
					INSERT INTO #TempTable (CCID,ParentHeadID,ComponentID)
					SELECT DISTINCT AOB.BreakdownObjID,AOB.AccountHeadID,dbo.GetComponentID(AOB.AccountHeadID) FROM AccountOpenningBreakdown AS AOB WHERE AOB.AccountHeadID=@AccountHeadID AND AOB.BreakdownObjID=@SubLedgerID AND AOB.AccountingSessionID=@SessionID AND AOB.BreakdownType=1 AND ISNULL(AOB.BreakdownObjID,0)!=0

					INSERT INTO #TempTable (CCID,ParentHeadID,ComponentID)
					SELECT DISTINCT CCT.CCID,CCT.AccountHeadID,dbo.GetComponentID(CCT.AccountHeadID) FROM View_CostCenterTransaction AS CCT WHERE CCT.AccountHeadID=@AccountHeadID AND CCT.CCID=@SubLedgerID AND CONVERT(DATE, CONVERT(VARCHAR(12),CCT.TransactionDate,106)) BETWEEN  CONVERT(DATE, CONVERT(VARCHAR(12),@SessionStartDate,106)) AND CONVERT(DATE, CONVERT(VARCHAR(12),@EndDate,106)) 
				END
				ELSE
				BEGIN
					INSERT INTO #TempTable (CCID,ParentHeadID,ComponentID)
					SELECT DISTINCT AOB.BreakdownObjID,AOB.AccountHeadID,dbo.GetComponentID(AOB.AccountHeadID) FROM AccountOpenningBreakdown AS AOB WHERE AOB.AccountHeadID=@AccountHeadID AND AOB.CurrencyID=@CurrencyID AND AOB.BreakdownObjID=@SubLedgerID AND AOB.AccountingSessionID=@SessionID AND AOB.BreakdownType=1 AND ISNULL(AOB.BreakdownObjID,0)!=0

					INSERT INTO #TempTable (CCID,ParentHeadID,ComponentID)
					SELECT DISTINCT CCT.CCID,CCT.AccountHeadID,dbo.GetComponentID(CCT.AccountHeadID) FROM View_CostCenterTransaction AS CCT WHERE CCT.AccountHeadID=@AccountHeadID AND CCT.CurrencyID=@CurrencyID AND CCT.CCID=@SubLedgerID AND CONVERT(DATE, CONVERT(VARCHAR(12),CCT.TransactionDate,106)) BETWEEN  CONVERT(DATE, CONVERT(VARCHAR(12),@SessionStartDate,106)) AND CONVERT(DATE, CONVERT(VARCHAR(12),@EndDate,106)) 
				END
			END
		END
	END
	ELSE
	BEGIN
		IF(@BusinessUnitID>0)
		BEGIN
			IF(@IsApproved=1)
			BEGIN
				IF(@CurrencyID=@BaseCurrencyID)
				BEGIN
					INSERT INTO #TempTable (CCID,ParentHeadID,ComponentID)
					SELECT DISTINCT AOB.BreakdownObjID,AOB.AccountHeadID,dbo.GetComponentID(AOB.AccountHeadID) FROM AccountOpenningBreakdown AS AOB WHERE AOB.BusinessUnitID=@BusinessUnitID AND AOB.BreakdownObjID=@SubLedgerID AND AOB.AccountingSessionID=@SessionID AND AOB.BreakdownType=1 AND ISNULL(AOB.BreakdownObjID,0)!=0

					INSERT INTO #TempTable (CCID,ParentHeadID,ComponentID)
					SELECT DISTINCT CCT.CCID,CCT.AccountHeadID,dbo.GetComponentID(CCT.AccountHeadID) FROM View_CostCenterTransaction AS CCT WHERE CCT.BUID=@BusinessUnitID AND ISNULL(CCT.ApprovedBy,0)!=0 AND CCT.CCID=@SubLedgerID AND CONVERT(DATE, CONVERT(VARCHAR(12),CCT.TransactionDate,106)) BETWEEN  CONVERT(DATE, CONVERT(VARCHAR(12),@SessionStartDate,106)) AND CONVERT(DATE, CONVERT(VARCHAR(12),@EndDate,106)) 
				END
				ELSE
				BEGIN
					INSERT INTO #TempTable (CCID,ParentHeadID,ComponentID)
					SELECT DISTINCT AOB.BreakdownObjID,AOB.AccountHeadID,dbo.GetComponentID(AOB.AccountHeadID) FROM AccountOpenningBreakdown AS AOB WHERE AOB.BusinessUnitID=@BusinessUnitID AND AOB.CurrencyID=@CurrencyID AND AOB.BreakdownObjID=@SubLedgerID AND AOB.AccountingSessionID=@SessionID AND AOB.BreakdownType=1 AND ISNULL(AOB.BreakdownObjID,0)!=0

					INSERT INTO #TempTable (CCID,ParentHeadID,ComponentID)
					SELECT DISTINCT CCT.CCID,CCT.AccountHeadID,dbo.GetComponentID(CCT.AccountHeadID) FROM View_CostCenterTransaction AS CCT WHERE CCT.BUID=@BusinessUnitID AND ISNULL(CCT.ApprovedBy,0)!=0  AND CCT.CurrencyID=@CurrencyID AND CCT.CCID=@SubLedgerID AND CONVERT(DATE, CONVERT(VARCHAR(12),CCT.TransactionDate,106)) BETWEEN  CONVERT(DATE, CONVERT(VARCHAR(12),@SessionStartDate,106)) AND CONVERT(DATE, CONVERT(VARCHAR(12),@EndDate,106)) 
				END
			END
			ELSE
			BEGIN
				IF(@CurrencyID=@BaseCurrencyID)
				BEGIN
					INSERT INTO #TempTable (CCID,ParentHeadID,ComponentID)
					SELECT DISTINCT AOB.BreakdownObjID,AOB.AccountHeadID,dbo.GetComponentID(AOB.AccountHeadID) FROM AccountOpenningBreakdown AS AOB WHERE AOB.BusinessUnitID=@BusinessUnitID AND AOB.BreakdownObjID=@SubLedgerID AND AOB.AccountingSessionID=@SessionID AND AOB.BreakdownType=1 AND ISNULL(AOB.BreakdownObjID,0)!=0

					INSERT INTO #TempTable (CCID,ParentHeadID,ComponentID)
					SELECT DISTINCT CCT.CCID,CCT.AccountHeadID,dbo.GetComponentID(CCT.AccountHeadID) FROM View_CostCenterTransaction AS CCT WHERE CCT.BUID=@BusinessUnitID AND CCT.CCID=@SubLedgerID AND CONVERT(DATE, CONVERT(VARCHAR(12),CCT.TransactionDate,106)) BETWEEN  CONVERT(DATE, CONVERT(VARCHAR(12),@SessionStartDate,106)) AND CONVERT(DATE, CONVERT(VARCHAR(12),@EndDate,106)) 
				END
				ELSE
				BEGIN
					INSERT INTO #TempTable (CCID,ParentHeadID,ComponentID)
					SELECT DISTINCT AOB.BreakdownObjID,AOB.AccountHeadID,dbo.GetComponentID(AOB.AccountHeadID) FROM AccountOpenningBreakdown AS AOB WHERE AOB.BusinessUnitID=@BusinessUnitID AND AOB.CurrencyID=@CurrencyID AND AOB.BreakdownObjID=@SubLedgerID AND AOB.AccountingSessionID=@SessionID AND AOB.BreakdownType=1 AND ISNULL(AOB.BreakdownObjID,0)!=0

					INSERT INTO #TempTable (CCID,ParentHeadID,ComponentID)
					SELECT DISTINCT CCT.CCID,CCT.AccountHeadID,dbo.GetComponentID(CCT.AccountHeadID) FROM View_CostCenterTransaction AS CCT WHERE CCT.BUID=@BusinessUnitID AND CCT.CurrencyID=@CurrencyID AND CCT.CCID=@SubLedgerID AND CONVERT(DATE, CONVERT(VARCHAR(12),CCT.TransactionDate,106)) BETWEEN  CONVERT(DATE, CONVERT(VARCHAR(12),@SessionStartDate,106)) AND CONVERT(DATE, CONVERT(VARCHAR(12),@EndDate,106)) 
				END
			END
		END
		ELSE
		BEGIN
			IF(@IsApproved=1)
			BEGIN
				IF(@CurrencyID=@BaseCurrencyID)
				BEGIN
					INSERT INTO #TempTable (CCID,ParentHeadID,ComponentID)
					SELECT DISTINCT AOB.BreakdownObjID,AOB.AccountHeadID,dbo.GetComponentID(AOB.AccountHeadID) FROM AccountOpenningBreakdown AS AOB WHERE AOB.BreakdownObjID=@SubLedgerID AND AOB.AccountingSessionID=@SessionID AND AOB.BreakdownType=1 AND ISNULL(AOB.BreakdownObjID,0)!=0

					INSERT INTO #TempTable (CCID,ParentHeadID,ComponentID)
					SELECT DISTINCT CCT.CCID,CCT.AccountHeadID,dbo.GetComponentID(CCT.AccountHeadID) FROM View_CostCenterTransaction AS CCT WHERE ISNULL(CCT.ApprovedBy,0)!=0 AND CCT.CCID=@SubLedgerID AND CONVERT(DATE, CONVERT(VARCHAR(12),CCT.TransactionDate,106)) BETWEEN  CONVERT(DATE, CONVERT(VARCHAR(12),@SessionStartDate,106)) AND CONVERT(DATE, CONVERT(VARCHAR(12),@EndDate,106)) 
				END
				ELSE
				BEGIN
					INSERT INTO #TempTable (CCID,ParentHeadID,ComponentID)
					SELECT DISTINCT AOB.BreakdownObjID,AOB.AccountHeadID,dbo.GetComponentID(AOB.AccountHeadID) FROM AccountOpenningBreakdown AS AOB WHERE AOB.CurrencyID=@CurrencyID AND AOB.BreakdownObjID=@SubLedgerID AND AOB.AccountingSessionID=@SessionID AND AOB.BreakdownType=1 AND ISNULL(AOB.BreakdownObjID,0)!=0

					INSERT INTO #TempTable (CCID,ParentHeadID,ComponentID)
					SELECT DISTINCT CCT.CCID,CCT.AccountHeadID,dbo.GetComponentID(CCT.AccountHeadID) FROM View_CostCenterTransaction AS CCT WHERE ISNULL(CCT.ApprovedBy,0)!=0  AND CCT.CurrencyID=@CurrencyID AND CCT.CCID=@SubLedgerID AND CONVERT(DATE, CONVERT(VARCHAR(12),CCT.TransactionDate,106)) BETWEEN  CONVERT(DATE, CONVERT(VARCHAR(12),@SessionStartDate,106)) AND CONVERT(DATE, CONVERT(VARCHAR(12),@EndDate,106)) 
				END
			END
			ELSE
			BEGIN
				IF(@CurrencyID=@BaseCurrencyID)
				BEGIN
					INSERT INTO #TempTable (CCID,ParentHeadID,ComponentID)
					SELECT DISTINCT AOB.BreakdownObjID,AOB.AccountHeadID,dbo.GetComponentID(AOB.AccountHeadID) FROM AccountOpenningBreakdown AS AOB WHERE AOB.BreakdownObjID=@SubLedgerID AND AOB.AccountingSessionID=@SessionID AND AOB.BreakdownType=1 AND ISNULL(AOB.BreakdownObjID,0)!=0

					INSERT INTO #TempTable (CCID,ParentHeadID,ComponentID)
					SELECT DISTINCT CCT.CCID,CCT.AccountHeadID,dbo.GetComponentID(CCT.AccountHeadID) FROM View_CostCenterTransaction AS CCT WHERE CCT.CCID=@SubLedgerID AND CONVERT(DATE, CONVERT(VARCHAR(12),CCT.TransactionDate,106)) BETWEEN  CONVERT(DATE, CONVERT(VARCHAR(12),@SessionStartDate,106)) AND CONVERT(DATE, CONVERT(VARCHAR(12),@EndDate,106)) 
				END
				ELSE
				BEGIN
					INSERT INTO #TempTable (CCID,ParentHeadID,ComponentID)
					SELECT DISTINCT AOB.BreakdownObjID,AOB.AccountHeadID,dbo.GetComponentID(AOB.AccountHeadID) FROM AccountOpenningBreakdown AS AOB WHERE AOB.CurrencyID=@CurrencyID AND AOB.BreakdownObjID=@SubLedgerID AND AOB.AccountingSessionID=@SessionID AND AOB.BreakdownType=1 AND ISNULL(AOB.BreakdownObjID,0)!=0

					INSERT INTO #TempTable (CCID,ParentHeadID,ComponentID)
					SELECT DISTINCT CCT.CCID,CCT.AccountHeadID,dbo.GetComponentID(CCT.AccountHeadID) FROM View_CostCenterTransaction AS CCT WHERE CCT.CurrencyID=@CurrencyID AND CCT.CCID=@SubLedgerID AND CONVERT(DATE, CONVERT(VARCHAR(12),CCT.TransactionDate,106)) BETWEEN  CONVERT(DATE, CONVERT(VARCHAR(12),@SessionStartDate,106)) AND CONVERT(DATE, CONVERT(VARCHAR(12),@EndDate,106)) 
				END
			END
		END
	END
END
ELSE
BEGIN
	IF(@AccountHeadID>0)
	BEGIN
		IF(@BusinessUnitID>0)
		BEGIN
			IF(@IsApproved=1)
			BEGIN
				IF(@CurrencyID=@BaseCurrencyID)
				BEGIN
					INSERT INTO #TempTable (CCID,ParentHeadID,ComponentID)
					SELECT DISTINCT AOB.BreakdownObjID,AOB.AccountHeadID,dbo.GetComponentID(AOB.AccountHeadID) FROM AccountOpenningBreakdown AS AOB WHERE AOB.AccountHeadID=@AccountHeadID AND AOB.BusinessUnitID=@BusinessUnitID AND AOB.AccountingSessionID=@SessionID AND AOB.BreakdownType=1 AND ISNULL(AOB.BreakdownObjID,0)!=0

					INSERT INTO #TempTable (CCID,ParentHeadID,ComponentID)
					SELECT DISTINCT CCT.CCID,CCT.AccountHeadID,dbo.GetComponentID(CCT.AccountHeadID) FROM View_CostCenterTransaction AS CCT WHERE CCT.AccountHeadID=@AccountHeadID AND CCT.BUID=@BusinessUnitID AND ISNULL(CCT.ApprovedBy,0)!=0 AND CONVERT(DATE, CONVERT(VARCHAR(12),CCT.TransactionDate,106)) BETWEEN  CONVERT(DATE, CONVERT(VARCHAR(12),@SessionStartDate,106)) AND CONVERT(DATE, CONVERT(VARCHAR(12),@EndDate,106)) 
				END
				ELSE
				BEGIN
					INSERT INTO #TempTable (CCID,ParentHeadID,ComponentID)
					SELECT DISTINCT AOB.BreakdownObjID,AOB.AccountHeadID,dbo.GetComponentID(AOB.AccountHeadID) FROM AccountOpenningBreakdown AS AOB WHERE AOB.AccountHeadID=@AccountHeadID AND AOB.BusinessUnitID=@BusinessUnitID AND AOB.CurrencyID=@CurrencyID AND AOB.AccountingSessionID=@SessionID AND AOB.BreakdownType=1 AND ISNULL(AOB.BreakdownObjID,0)!=0

					INSERT INTO #TempTable (CCID,ParentHeadID,ComponentID)
					SELECT DISTINCT CCT.CCID,CCT.AccountHeadID,dbo.GetComponentID(CCT.AccountHeadID) FROM View_CostCenterTransaction AS CCT WHERE CCT.AccountHeadID=@AccountHeadID AND CCT.BUID=@BusinessUnitID AND ISNULL(CCT.ApprovedBy,0)!=0  AND CCT.CurrencyID=@CurrencyID AND CONVERT(DATE, CONVERT(VARCHAR(12),CCT.TransactionDate,106)) BETWEEN  CONVERT(DATE, CONVERT(VARCHAR(12),@SessionStartDate,106)) AND CONVERT(DATE, CONVERT(VARCHAR(12),@EndDate,106)) 
				END
			END
			ELSE
			BEGIN
				IF(@CurrencyID=@BaseCurrencyID)
				BEGIN
					INSERT INTO #TempTable (CCID,ParentHeadID,ComponentID)
					SELECT DISTINCT AOB.BreakdownObjID,AOB.AccountHeadID,dbo.GetComponentID(AOB.AccountHeadID) FROM AccountOpenningBreakdown AS AOB WHERE AOB.AccountHeadID=@AccountHeadID AND AOB.BusinessUnitID=@BusinessUnitID AND AOB.AccountingSessionID=@SessionID AND AOB.BreakdownType=1 AND ISNULL(AOB.BreakdownObjID,0)!=0

					INSERT INTO #TempTable (CCID,ParentHeadID,ComponentID)
					SELECT DISTINCT CCT.CCID,CCT.AccountHeadID,dbo.GetComponentID(CCT.AccountHeadID) FROM View_CostCenterTransaction AS CCT WHERE CCT.AccountHeadID=@AccountHeadID AND CCT.BUID=@BusinessUnitID AND CONVERT(DATE, CONVERT(VARCHAR(12),CCT.TransactionDate,106)) BETWEEN  CONVERT(DATE, CONVERT(VARCHAR(12),@SessionStartDate,106)) AND CONVERT(DATE, CONVERT(VARCHAR(12),@EndDate,106)) 
				END
				ELSE
				BEGIN
					INSERT INTO #TempTable (CCID,ParentHeadID,ComponentID)
					SELECT DISTINCT AOB.BreakdownObjID,AOB.AccountHeadID,dbo.GetComponentID(AOB.AccountHeadID) FROM AccountOpenningBreakdown AS AOB WHERE AOB.AccountHeadID=@AccountHeadID AND AOB.BusinessUnitID=@BusinessUnitID AND AOB.CurrencyID=@CurrencyID AND AOB.AccountingSessionID=@SessionID AND AOB.BreakdownType=1 AND ISNULL(AOB.BreakdownObjID,0)!=0

					INSERT INTO #TempTable (CCID,ParentHeadID,ComponentID)
					SELECT DISTINCT CCT.CCID,CCT.AccountHeadID,dbo.GetComponentID(CCT.AccountHeadID) FROM View_CostCenterTransaction AS CCT WHERE CCT.AccountHeadID=@AccountHeadID AND CCT.BUID=@BusinessUnitID AND CCT.CurrencyID=@CurrencyID AND CONVERT(DATE, CONVERT(VARCHAR(12),CCT.TransactionDate,106)) BETWEEN  CONVERT(DATE, CONVERT(VARCHAR(12),@SessionStartDate,106)) AND CONVERT(DATE, CONVERT(VARCHAR(12),@EndDate,106)) 
				END
			END
		END
		ELSE
		BEGIN
			IF(@IsApproved=1)
			BEGIN
				IF(@CurrencyID=@BaseCurrencyID)
				BEGIN
					INSERT INTO #TempTable (CCID,ParentHeadID,ComponentID)
					SELECT DISTINCT AOB.BreakdownObjID,AOB.AccountHeadID,dbo.GetComponentID(AOB.AccountHeadID) FROM AccountOpenningBreakdown AS AOB WHERE AOB.AccountHeadID=@AccountHeadID AND AOB.AccountingSessionID=@SessionID AND AOB.BreakdownType=1 AND ISNULL(AOB.BreakdownObjID,0)!=0

					INSERT INTO #TempTable (CCID,ParentHeadID,ComponentID)
					SELECT DISTINCT CCT.CCID,CCT.AccountHeadID,dbo.GetComponentID(CCT.AccountHeadID) FROM View_CostCenterTransaction AS CCT WHERE CCT.AccountHeadID=@AccountHeadID AND ISNULL(CCT.ApprovedBy,0)!=0 AND CONVERT(DATE, CONVERT(VARCHAR(12),CCT.TransactionDate,106)) BETWEEN  CONVERT(DATE, CONVERT(VARCHAR(12),@SessionStartDate,106)) AND CONVERT(DATE, CONVERT(VARCHAR(12),@EndDate,106)) 
				END
				ELSE
				BEGIN
					INSERT INTO #TempTable (CCID,ParentHeadID,ComponentID)
					SELECT DISTINCT AOB.BreakdownObjID,AOB.AccountHeadID,dbo.GetComponentID(AOB.AccountHeadID) FROM AccountOpenningBreakdown AS AOB WHERE AOB.AccountHeadID=@AccountHeadID AND AOB.CurrencyID=@CurrencyID AND AOB.AccountingSessionID=@SessionID AND AOB.BreakdownType=1 AND ISNULL(AOB.BreakdownObjID,0)!=0

					INSERT INTO #TempTable (CCID,ParentHeadID,ComponentID)
					SELECT DISTINCT CCT.CCID,CCT.AccountHeadID,dbo.GetComponentID(CCT.AccountHeadID) FROM View_CostCenterTransaction AS CCT WHERE CCT.AccountHeadID=@AccountHeadID AND ISNULL(CCT.ApprovedBy,0)!=0  AND CCT.CurrencyID=@CurrencyID AND CONVERT(DATE, CONVERT(VARCHAR(12),CCT.TransactionDate,106)) BETWEEN  CONVERT(DATE, CONVERT(VARCHAR(12),@SessionStartDate,106)) AND CONVERT(DATE, CONVERT(VARCHAR(12),@EndDate,106)) 
				END
			END
			ELSE
			BEGIN
				IF(@CurrencyID=@BaseCurrencyID)
				BEGIN
					INSERT INTO #TempTable (CCID,ParentHeadID,ComponentID)
					SELECT DISTINCT AOB.BreakdownObjID,AOB.AccountHeadID,dbo.GetComponentID(AOB.AccountHeadID) FROM AccountOpenningBreakdown AS AOB WHERE AOB.AccountHeadID=@AccountHeadID AND AOB.AccountingSessionID=@SessionID AND AOB.BreakdownType=1 AND ISNULL(AOB.BreakdownObjID,0)!=0

					INSERT INTO #TempTable (CCID,ParentHeadID,ComponentID)
					SELECT DISTINCT CCT.CCID,CCT.AccountHeadID,dbo.GetComponentID(CCT.AccountHeadID) FROM View_CostCenterTransaction AS CCT WHERE CCT.AccountHeadID=@AccountHeadID AND CONVERT(DATE, CONVERT(VARCHAR(12),CCT.TransactionDate,106)) BETWEEN  CONVERT(DATE, CONVERT(VARCHAR(12),@SessionStartDate,106)) AND CONVERT(DATE, CONVERT(VARCHAR(12),@EndDate,106)) 
				END
				ELSE
				BEGIN
					INSERT INTO #TempTable (CCID,ParentHeadID,ComponentID)
					SELECT DISTINCT AOB.BreakdownObjID,AOB.AccountHeadID,dbo.GetComponentID(AOB.AccountHeadID) FROM AccountOpenningBreakdown AS AOB WHERE AOB.AccountHeadID=@AccountHeadID AND AOB.CurrencyID=@CurrencyID AND AOB.AccountingSessionID=@SessionID AND AOB.BreakdownType=1 AND ISNULL(AOB.BreakdownObjID,0)!=0

					INSERT INTO #TempTable (CCID,ParentHeadID,ComponentID)
					SELECT DISTINCT CCT.CCID,CCT.AccountHeadID,dbo.GetComponentID(CCT.AccountHeadID) FROM View_CostCenterTransaction AS CCT WHERE CCT.AccountHeadID=@AccountHeadID AND CCT.CurrencyID=@CurrencyID AND CONVERT(DATE, CONVERT(VARCHAR(12),CCT.TransactionDate,106)) BETWEEN  CONVERT(DATE, CONVERT(VARCHAR(12),@SessionStartDate,106)) AND CONVERT(DATE, CONVERT(VARCHAR(12),@EndDate,106)) 
				END
			END
		END
	END
	ELSE
	BEGIN
		IF(@BusinessUnitID>0)
		BEGIN
			IF(@IsApproved=1)
			BEGIN
				IF(@CurrencyID=@BaseCurrencyID)
				BEGIN
					INSERT INTO #TempTable (CCID,ParentHeadID,ComponentID)
					SELECT DISTINCT AOB.BreakdownObjID,AOB.AccountHeadID,dbo.GetComponentID(AOB.AccountHeadID) FROM AccountOpenningBreakdown AS AOB WHERE AOB.BusinessUnitID=@BusinessUnitID AND AOB.AccountingSessionID=@SessionID AND AOB.BreakdownType=1 AND ISNULL(AOB.BreakdownObjID,0)!=0

					INSERT INTO #TempTable (CCID,ParentHeadID,ComponentID)
					SELECT DISTINCT CCT.CCID,CCT.AccountHeadID,dbo.GetComponentID(CCT.AccountHeadID) FROM View_CostCenterTransaction AS CCT WHERE CCT.BUID=@BusinessUnitID AND ISNULL(CCT.ApprovedBy,0)!=0 AND CONVERT(DATE, CONVERT(VARCHAR(12),CCT.TransactionDate,106)) BETWEEN  CONVERT(DATE, CONVERT(VARCHAR(12),@SessionStartDate,106)) AND CONVERT(DATE, CONVERT(VARCHAR(12),@EndDate,106)) 
				END
				ELSE
				BEGIN
					INSERT INTO #TempTable (CCID,ParentHeadID,ComponentID)
					SELECT DISTINCT AOB.BreakdownObjID,AOB.AccountHeadID,dbo.GetComponentID(AOB.AccountHeadID) FROM AccountOpenningBreakdown AS AOB WHERE AOB.BusinessUnitID=@BusinessUnitID AND AOB.CurrencyID=@CurrencyID AND AOB.AccountingSessionID=@SessionID AND AOB.BreakdownType=1 AND ISNULL(AOB.BreakdownObjID,0)!=0

					INSERT INTO #TempTable (CCID,ParentHeadID,ComponentID)
					SELECT DISTINCT CCT.CCID,CCT.AccountHeadID,dbo.GetComponentID(CCT.AccountHeadID) FROM View_CostCenterTransaction AS CCT WHERE CCT.BUID=@BusinessUnitID AND ISNULL(CCT.ApprovedBy,0)!=0  AND CCT.CurrencyID=@CurrencyID AND CONVERT(DATE, CONVERT(VARCHAR(12),CCT.TransactionDate,106)) BETWEEN  CONVERT(DATE, CONVERT(VARCHAR(12),@SessionStartDate,106)) AND CONVERT(DATE, CONVERT(VARCHAR(12),@EndDate,106)) 
				END
			END
			ELSE
			BEGIN
				IF(@CurrencyID=@BaseCurrencyID)
				BEGIN
					INSERT INTO #TempTable (CCID,ParentHeadID,ComponentID)
					SELECT DISTINCT AOB.BreakdownObjID,AOB.AccountHeadID,dbo.GetComponentID(AOB.AccountHeadID) FROM AccountOpenningBreakdown AS AOB WHERE AOB.BusinessUnitID=@BusinessUnitID AND AOB.AccountingSessionID=@SessionID AND AOB.BreakdownType=1 AND ISNULL(AOB.BreakdownObjID,0)!=0

					INSERT INTO #TempTable (CCID,ParentHeadID,ComponentID)
					SELECT DISTINCT CCT.CCID,CCT.AccountHeadID,dbo.GetComponentID(CCT.AccountHeadID) FROM View_CostCenterTransaction AS CCT WHERE CCT.BUID=@BusinessUnitID AND CONVERT(DATE, CONVERT(VARCHAR(12),CCT.TransactionDate,106)) BETWEEN  CONVERT(DATE, CONVERT(VARCHAR(12),@SessionStartDate,106)) AND CONVERT(DATE, CONVERT(VARCHAR(12),@EndDate,106)) 
				END
				ELSE
				BEGIN
					INSERT INTO #TempTable (CCID,ParentHeadID,ComponentID)
					SELECT DISTINCT AOB.BreakdownObjID,AOB.AccountHeadID,dbo.GetComponentID(AOB.AccountHeadID) FROM AccountOpenningBreakdown AS AOB WHERE AOB.BusinessUnitID=@BusinessUnitID AND AOB.CurrencyID=@CurrencyID AND AOB.AccountingSessionID=@SessionID AND AOB.BreakdownType=1 AND ISNULL(AOB.BreakdownObjID,0)!=0

					INSERT INTO #TempTable (CCID,ParentHeadID,ComponentID)
					SELECT DISTINCT CCT.CCID,CCT.AccountHeadID,dbo.GetComponentID(CCT.AccountHeadID) FROM View_CostCenterTransaction AS CCT WHERE CCT.BUID=@BusinessUnitID AND CCT.CurrencyID=@CurrencyID AND CONVERT(DATE, CONVERT(VARCHAR(12),CCT.TransactionDate,106)) BETWEEN  CONVERT(DATE, CONVERT(VARCHAR(12),@SessionStartDate,106)) AND CONVERT(DATE, CONVERT(VARCHAR(12),@EndDate,106)) 
				END
			END
		END
		ELSE
		BEGIN
			IF(@IsApproved=1)
			BEGIN
				IF(@CurrencyID=@BaseCurrencyID)
				BEGIN
					INSERT INTO #TempTable (CCID,ParentHeadID,ComponentID)
					SELECT DISTINCT AOB.BreakdownObjID,AOB.AccountHeadID,dbo.GetComponentID(AOB.AccountHeadID) FROM AccountOpenningBreakdown AS AOB WHERE AOB.AccountingSessionID=@SessionID AND AOB.BreakdownType=1 AND ISNULL(AOB.BreakdownObjID,0)!=0

					INSERT INTO #TempTable (CCID,ParentHeadID,ComponentID)
					SELECT DISTINCT CCT.CCID,CCT.AccountHeadID,dbo.GetComponentID(CCT.AccountHeadID) FROM View_CostCenterTransaction AS CCT WHERE ISNULL(CCT.ApprovedBy,0)!=0 AND CONVERT(DATE, CONVERT(VARCHAR(12),CCT.TransactionDate,106)) BETWEEN  CONVERT(DATE, CONVERT(VARCHAR(12),@SessionStartDate,106)) AND CONVERT(DATE, CONVERT(VARCHAR(12),@EndDate,106)) 
				END
				ELSE
				BEGIN
					INSERT INTO #TempTable (CCID,ParentHeadID,ComponentID)
					SELECT DISTINCT AOB.BreakdownObjID,AOB.AccountHeadID,dbo.GetComponentID(AOB.AccountHeadID) FROM AccountOpenningBreakdown AS AOB WHERE AOB.CurrencyID=@CurrencyID AND AOB.AccountingSessionID=@SessionID AND AOB.BreakdownType=1 AND ISNULL(AOB.BreakdownObjID,0)!=0

					INSERT INTO #TempTable (CCID,ParentHeadID,ComponentID)
					SELECT DISTINCT CCT.CCID,CCT.AccountHeadID,dbo.GetComponentID(CCT.AccountHeadID) FROM View_CostCenterTransaction AS CCT WHERE ISNULL(CCT.ApprovedBy,0)!=0  AND CCT.CurrencyID=@CurrencyID AND CONVERT(DATE, CONVERT(VARCHAR(12),CCT.TransactionDate,106)) BETWEEN  CONVERT(DATE, CONVERT(VARCHAR(12),@SessionStartDate,106)) AND CONVERT(DATE, CONVERT(VARCHAR(12),@EndDate,106)) 
				END
			END
			ELSE
			BEGIN
				IF(@CurrencyID=@BaseCurrencyID)
				BEGIN
					INSERT INTO #TempTable (CCID,ParentHeadID,ComponentID)
					SELECT DISTINCT AOB.BreakdownObjID,AOB.AccountHeadID,dbo.GetComponentID(AOB.AccountHeadID) FROM AccountOpenningBreakdown AS AOB WHERE AOB.AccountingSessionID=@SessionID AND AOB.BreakdownType=1 AND ISNULL(AOB.BreakdownObjID,0)!=0

					INSERT INTO #TempTable (CCID,ParentHeadID,ComponentID)
					SELECT DISTINCT CCT.CCID,CCT.AccountHeadID,dbo.GetComponentID(CCT.AccountHeadID) FROM View_CostCenterTransaction AS CCT WHERE CONVERT(DATE, CONVERT(VARCHAR(12),CCT.TransactionDate,106)) BETWEEN  CONVERT(DATE, CONVERT(VARCHAR(12),@SessionStartDate,106)) AND CONVERT(DATE, CONVERT(VARCHAR(12),@EndDate,106)) 
				END
				ELSE
				BEGIN
					INSERT INTO #TempTable (CCID,ParentHeadID,ComponentID)
					SELECT DISTINCT AOB.BreakdownObjID,AOB.AccountHeadID,dbo.GetComponentID(AOB.AccountHeadID) FROM AccountOpenningBreakdown AS AOB WHERE AOB.CurrencyID=@CurrencyID AND AOB.AccountingSessionID=@SessionID AND AOB.BreakdownType=1 AND ISNULL(AOB.BreakdownObjID,0)!=0

					INSERT INTO #TempTable (CCID,ParentHeadID,ComponentID)
					SELECT DISTINCT CCT.CCID,CCT.AccountHeadID,dbo.GetComponentID(CCT.AccountHeadID) FROM View_CostCenterTransaction AS CCT WHERE CCT.CurrencyID=@CurrencyID AND CONVERT(DATE, CONVERT(VARCHAR(12),CCT.TransactionDate,106)) BETWEEN  CONVERT(DATE, CONVERT(VARCHAR(12),@SessionStartDate,106)) AND CONVERT(DATE, CONVERT(VARCHAR(12),@EndDate,106)) 
				END
			END
		END
	END
END

DELETE FROM #TempTable WHERE ID NOT IN (SELECT MIN(ID) FROM #TempTable GROUP BY CCID,ParentHeadID)

INSERT INTO #TempTable2 (CCID,ParentHeadID,ComponentID) SELECT CCID,ParentHeadID,ComponentID FROM #TempTable

IF(@BusinessUnitID>0)
BEGIN
	IF(@IsApproved=1)
	BEGIN
		IF(@CurrencyID=@BaseCurrencyID)
		BEGIN
			UPDATE #TempTable2
			SET DebitOpeiningValue=ISNULL((SELECT SUM(AOB.OpenningBalance) FROM AccountOpenningBreakdown AS AOB WHERE AOB.IsDr=1 AND AOB.BusinessUnitID=@BusinessUnitID AND AOB.BreakdownObjID=TT.CCID AND AOB.AccountHeadID=TT.ParentHeadID AND AOB.AccountingSessionID=@SessionID AND AOB.BreakdownType=1),0),
				CreditOpeiningValue=ISNULL((SELECT SUM(AOB.OpenningBalance) FROM AccountOpenningBreakdown AS AOB WHERE AOB.IsDr=0 AND AOB.BusinessUnitID=@BusinessUnitID AND AOB.BreakdownObjID=TT.CCID AND AOB.AccountHeadID=TT.ParentHeadID AND AOB.AccountingSessionID=@SessionID AND AOB.BreakdownType=1),0),
				DebitAmount=ISNULL((SELECT SUM(CCT.Amount*CCT.CurrencyConversionRate) FROM View_CostCenterTransaction AS CCT WHERE CCT.BUID=@BusinessUnitID AND  ISNULL(CCT.ApprovedBy,0)!=0 AND  CCT.IsDr=1 AND CCT.CCID=TT.CCID AND CCT.AccountHeadID=TT.ParentHeadID AND CONVERT(DATE, CONVERT(VARCHAR(12),CCT.TransactionDate,106)) BETWEEN  CONVERT(DATE, CONVERT(VARCHAR(12),@SessionStartDate,106)) AND CONVERT(DATE, CONVERT(VARCHAR(12),DATEADD(DAY,-1,@StartDate),106))),0),
				CreditAmount=ISNULL((SELECT SUM(CCT.Amount*CCT.CurrencyConversionRate) FROM View_CostCenterTransaction AS CCT WHERE  CCT.BUID=@BusinessUnitID AND ISNULL(CCT.ApprovedBy,0)!=0 AND  CCT.IsDr=0 AND CCT.CCID=TT.CCID AND CCT.AccountHeadID=TT.ParentHeadID AND CONVERT(DATE, CONVERT(VARCHAR(12),CCT.TransactionDate,106)) BETWEEN  CONVERT(DATE, CONVERT(VARCHAR(12),@SessionStartDate,106)) AND CONVERT(DATE, CONVERT(VARCHAR(12),DATEADD(DAY,-1,@StartDate),106))),0)
			FROM #TempTable2 AS TT
	
			UPDATE #TempTable
			SET DebitAmount=ISNULL((SELECT SUM(CCT.Amount*CCT.CurrencyConversionRate) FROM View_CostCenterTransaction AS CCT WHERE  CCT.BUID=@BusinessUnitID AND ISNULL(CCT.ApprovedBy,0)!=0 AND  CCT.IsDr=1 AND CCT.CCID=TT.CCID AND CCT.AccountHeadID=TT.ParentHeadID AND CONVERT(DATE, CONVERT(VARCHAR(12),CCT.TransactionDate,106)) BETWEEN  CONVERT(DATE, CONVERT(VARCHAR(12),@StartDate,106)) AND CONVERT(DATE, CONVERT(VARCHAR(12),@EndDate,106))),0),
				CreditAmount=ISNULL((SELECT SUM(CCT.Amount*CCT.CurrencyConversionRate) FROM View_CostCenterTransaction AS CCT WHERE CCT.BUID=@BusinessUnitID AND  ISNULL(CCT.ApprovedBy,0)!=0 AND  CCT.IsDr=0 AND CCT.CCID=TT.CCID AND CCT.AccountHeadID=TT.ParentHeadID AND CONVERT(DATE, CONVERT(VARCHAR(12),CCT.TransactionDate,106)) BETWEEN  CONVERT(DATE, CONVERT(VARCHAR(12),@StartDate,106)) AND CONVERT(DATE, CONVERT(VARCHAR(12),@EndDate,106))),0)
			FROM #TempTable AS TT

		END
		ELSE
		BEGIN
			UPDATE #TempTable2
			SET DebitOpeiningValue=ISNULL((SELECT SUM(AOB.AmountInCurrency) FROM AccountOpenningBreakdown AS AOB WHERE AOB.IsDr=1 AND AOB.BusinessUnitID=@BusinessUnitID AND AOB.CurrencyID=@CurrencyID AND AOB.BreakdownObjID=TT.CCID AND AOB.AccountHeadID=TT.ParentHeadID AND AOB.AccountingSessionID=@SessionID AND AOB.BreakdownType=1),0),
				CreditOpeiningValue=ISNULL((SELECT SUM(AOB.AmountInCurrency) FROM AccountOpenningBreakdown AS AOB WHERE AOB.IsDr=0 AND AOB.BusinessUnitID=@BusinessUnitID AND AOB.CurrencyID=@CurrencyID AND AOB.BreakdownObjID=TT.CCID AND AOB.AccountHeadID=TT.ParentHeadID AND AOB.AccountingSessionID=@SessionID AND AOB.BreakdownType=1),0),
				DebitAmount=ISNULL((SELECT SUM(CCT.Amount) FROM View_CostCenterTransaction AS CCT WHERE CCT.BUID=@BusinessUnitID AND CCT.ApprovedBy!=0 AND CCT.CurrencyID=@CurrencyID AND  CCT.IsDr=1 AND CCT.CCID=TT.CCID AND CCT.AccountHeadID=TT.ParentHeadID AND CONVERT(DATE, CONVERT(VARCHAR(12),CCT.TransactionDate,106)) BETWEEN  CONVERT(DATE, CONVERT(VARCHAR(12),@SessionStartDate,106)) AND CONVERT(DATE, CONVERT(VARCHAR(12),DATEADD(DAY,-1,@StartDate),106))),0),
				CreditAmount=ISNULL((SELECT SUM(CCT.Amount) FROM View_CostCenterTransaction AS CCT WHERE CCT.BUID=@BusinessUnitID AND CCT.ApprovedBy!=0 AND CCT.CurrencyID=@CurrencyID AND CCT.IsDr=0 AND CCT.CCID=TT.CCID AND CCT.AccountHeadID=TT.ParentHeadID AND CONVERT(DATE, CONVERT(VARCHAR(12),CCT.TransactionDate,106)) BETWEEN  CONVERT(DATE, CONVERT(VARCHAR(12),@SessionStartDate,106)) AND CONVERT(DATE, CONVERT(VARCHAR(12),DATEADD(DAY,-1,@StartDate),106))),0)
			FROM #TempTable2 AS TT

			UPDATE #TempTable
			SET DebitAmount=ISNULL((SELECT SUM(CCT.Amount) FROM View_CostCenterTransaction AS CCT WHERE CCT.BUID=@BusinessUnitID AND CCT.ApprovedBy!=0 AND CCT.CurrencyID=@CurrencyID AND  CCT.IsDr=1 AND CCT.CCID=TT.CCID AND CCT.AccountHeadID=TT.ParentHeadID AND CONVERT(DATE, CONVERT(VARCHAR(12),CCT.TransactionDate,106)) BETWEEN  CONVERT(DATE, CONVERT(VARCHAR(12),@StartDate,106)) AND CONVERT(DATE, CONVERT(VARCHAR(12),@EndDate,106))),0),
				CreditAmount=ISNULL((SELECT SUM(CCT.Amount) FROM View_CostCenterTransaction AS CCT WHERE CCT.BUID=@BusinessUnitID AND CCT.ApprovedBy!=0 AND CCT.CurrencyID=@CurrencyID AND  CCT.IsDr=0 AND CCT.CCID=TT.CCID AND CCT.AccountHeadID=TT.ParentHeadID AND CONVERT(DATE, CONVERT(VARCHAR(12),CCT.TransactionDate,106)) BETWEEN  CONVERT(DATE, CONVERT(VARCHAR(12),@StartDate,106)) AND CONVERT(DATE, CONVERT(VARCHAR(12),@EndDate,106))),0)
			FROM #TempTable AS TT
		END
	END
	ELSE
	BEGIN
		IF(@CurrencyID=@BaseCurrencyID)
		BEGIN
			UPDATE #TempTable2
			SET DebitOpeiningValue=ISNULL((SELECT SUM(AOB.OpenningBalance) FROM AccountOpenningBreakdown AS AOB WHERE AOB.IsDr=1 AND AOB.BusinessUnitID=@BusinessUnitID AND AOB.BreakdownObjID=TT.CCID AND AOB.AccountHeadID=TT.ParentHeadID AND AOB.AccountingSessionID=@SessionID AND AOB.BreakdownType=1),0),
				CreditOpeiningValue=ISNULL((SELECT SUM(AOB.OpenningBalance) FROM AccountOpenningBreakdown AS AOB WHERE AOB.IsDr=0 AND AOB.BusinessUnitID=@BusinessUnitID AND AOB.BreakdownObjID=TT.CCID AND AOB.AccountHeadID=TT.ParentHeadID AND AOB.AccountingSessionID=@SessionID AND AOB.BreakdownType=1),0),
				DebitAmount=ISNULL((SELECT SUM(CCT.Amount*CCT.CurrencyConversionRate) FROM View_CostCenterTransaction AS CCT WHERE CCT.BUID=@BusinessUnitID AND CCT.IsDr=1 AND CCT.CCID=TT.CCID AND CCT.AccountHeadID=TT.ParentHeadID AND CONVERT(DATE, CONVERT(VARCHAR(12),CCT.TransactionDate,106)) BETWEEN  CONVERT(DATE, CONVERT(VARCHAR(12),@SessionStartDate,106)) AND CONVERT(DATE, CONVERT(VARCHAR(12),DATEADD(DAY,-1,@StartDate),106))),0),
				CreditAmount=ISNULL((SELECT SUM(CCT.Amount*CCT.CurrencyConversionRate) FROM View_CostCenterTransaction AS CCT WHERE CCT.BUID=@BusinessUnitID AND CCT.IsDr=0 AND CCT.CCID=TT.CCID AND CCT.AccountHeadID=TT.ParentHeadID AND CONVERT(DATE, CONVERT(VARCHAR(12),CCT.TransactionDate,106)) BETWEEN  CONVERT(DATE, CONVERT(VARCHAR(12),@SessionStartDate,106)) AND CONVERT(DATE, CONVERT(VARCHAR(12),DATEADD(DAY,-1,@StartDate),106))),0)
			FROM #TempTable2 AS TT
	
			UPDATE #TempTable
			SET DebitAmount=ISNULL((SELECT SUM(CCT.Amount*CCT.CurrencyConversionRate) FROM View_CostCenterTransaction AS CCT WHERE CCT.BUID=@BusinessUnitID AND CCT.IsDr=1 AND CCT.CCID=TT.CCID AND CCT.AccountHeadID=TT.ParentHeadID AND CONVERT(DATE, CONVERT(VARCHAR(12),CCT.TransactionDate,106)) BETWEEN  CONVERT(DATE, CONVERT(VARCHAR(12),@StartDate,106)) AND CONVERT(DATE, CONVERT(VARCHAR(12),@EndDate,106))),0),
				CreditAmount=ISNULL((SELECT SUM(CCT.Amount*CCT.CurrencyConversionRate) FROM View_CostCenterTransaction AS CCT WHERE CCT.BUID=@BusinessUnitID AND CCT.IsDr=0 AND CCT.CCID=TT.CCID AND CCT.AccountHeadID=TT.ParentHeadID AND CONVERT(DATE, CONVERT(VARCHAR(12),CCT.TransactionDate,106)) BETWEEN  CONVERT(DATE, CONVERT(VARCHAR(12),@StartDate,106)) AND CONVERT(DATE, CONVERT(VARCHAR(12),@EndDate,106))),0)
			FROM #TempTable AS TT

		END
		ELSE
		BEGIN
			UPDATE #TempTable2
			SET DebitOpeiningValue=ISNULL((SELECT SUM(AOB.AmountInCurrency) FROM AccountOpenningBreakdown AS AOB WHERE AOB.IsDr=1 AND AOB.BusinessUnitID=@BusinessUnitID AND AOB.CurrencyID=@CurrencyID AND AOB.BreakdownObjID=TT.CCID AND AOB.AccountHeadID=TT.ParentHeadID AND AOB.AccountingSessionID=@SessionID AND AOB.BreakdownType=1),0),
				CreditOpeiningValue=ISNULL((SELECT SUM(AOB.AmountInCurrency) FROM AccountOpenningBreakdown AS AOB WHERE AOB.IsDr=0 AND AOB.BusinessUnitID=@BusinessUnitID AND AOB.CurrencyID=@CurrencyID AND AOB.BreakdownObjID=TT.CCID AND AOB.AccountHeadID=TT.ParentHeadID AND AOB.AccountingSessionID=@SessionID AND AOB.BreakdownType=1),0),
				DebitAmount=ISNULL((SELECT SUM(CCT.Amount) FROM View_CostCenterTransaction AS CCT WHERE CCT.BUID=@BusinessUnitID AND CCT.CurrencyID=@CurrencyID AND  CCT.IsDr=1 AND CCT.CCID=TT.CCID AND CCT.AccountHeadID=TT.ParentHeadID AND CONVERT(DATE, CONVERT(VARCHAR(12),CCT.TransactionDate,106)) BETWEEN  CONVERT(DATE, CONVERT(VARCHAR(12),@SessionStartDate,106)) AND CONVERT(DATE, CONVERT(VARCHAR(12),DATEADD(DAY,-1,@StartDate),106))),0),
				CreditAmount=ISNULL((SELECT SUM(CCT.Amount) FROM View_CostCenterTransaction AS CCT WHERE CCT.BUID=@BusinessUnitID AND CCT.CurrencyID=@CurrencyID AND CCT.IsDr=0 AND CCT.CCID=TT.CCID AND CCT.AccountHeadID=TT.ParentHeadID AND CONVERT(DATE, CONVERT(VARCHAR(12),CCT.TransactionDate,106)) BETWEEN  CONVERT(DATE, CONVERT(VARCHAR(12),@SessionStartDate,106)) AND CONVERT(DATE, CONVERT(VARCHAR(12),DATEADD(DAY,-1,@StartDate),106))),0)
			FROM #TempTable2 AS TT

			UPDATE #TempTable
			SET DebitAmount=ISNULL((SELECT SUM(CCT.Amount) FROM View_CostCenterTransaction AS CCT WHERE CCT.BUID=@BusinessUnitID AND CCT.CurrencyID=@CurrencyID AND  CCT.IsDr=1 AND CCT.CCID=TT.CCID AND CCT.AccountHeadID=TT.ParentHeadID AND CONVERT(DATE, CONVERT(VARCHAR(12),CCT.TransactionDate,106)) BETWEEN  CONVERT(DATE, CONVERT(VARCHAR(12),@StartDate,106)) AND CONVERT(DATE, CONVERT(VARCHAR(12),@EndDate,106))),0),
				CreditAmount=ISNULL((SELECT SUM(CCT.Amount) FROM View_CostCenterTransaction AS CCT WHERE CCT.BUID=@BusinessUnitID AND CCT.CurrencyID=@CurrencyID AND  CCT.IsDr=0 AND CCT.CCID=TT.CCID AND CCT.AccountHeadID=TT.ParentHeadID AND CONVERT(DATE, CONVERT(VARCHAR(12),CCT.TransactionDate,106)) BETWEEN  CONVERT(DATE, CONVERT(VARCHAR(12),@StartDate,106)) AND CONVERT(DATE, CONVERT(VARCHAR(12),@EndDate,106))),0)
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
			UPDATE #TempTable2
			SET DebitOpeiningValue=ISNULL((SELECT SUM(AOB.OpenningBalance) FROM AccountOpenningBreakdown AS AOB WHERE AOB.IsDr=1 AND AOB.BreakdownObjID=TT.CCID AND AOB.AccountHeadID=TT.ParentHeadID AND AOB.AccountingSessionID=@SessionID AND AOB.BreakdownType=1),0),
				CreditOpeiningValue=ISNULL((SELECT SUM(AOB.OpenningBalance) FROM AccountOpenningBreakdown AS AOB WHERE AOB.IsDr=0 AND AOB.BreakdownObjID=TT.CCID AND AOB.AccountHeadID=TT.ParentHeadID AND AOB.AccountingSessionID=@SessionID AND AOB.BreakdownType=1),0),
				DebitAmount=ISNULL((SELECT SUM(CCT.Amount*CCT.CurrencyConversionRate) FROM View_CostCenterTransaction AS CCT WHERE  ISNULL(CCT.ApprovedBy,0)!=0 AND  CCT.IsDr=1 AND CCT.CCID=TT.CCID AND CCT.AccountHeadID=TT.ParentHeadID AND CONVERT(DATE, CONVERT(VARCHAR(12),CCT.TransactionDate,106)) BETWEEN  CONVERT(DATE, CONVERT(VARCHAR(12),@SessionStartDate,106)) AND CONVERT(DATE, CONVERT(VARCHAR(12),DATEADD(DAY,-1,@StartDate),106))),0),
				CreditAmount=ISNULL((SELECT SUM(CCT.Amount*CCT.CurrencyConversionRate) FROM View_CostCenterTransaction AS CCT WHERE  ISNULL(CCT.ApprovedBy,0)!=0 AND  CCT.IsDr=0 AND CCT.CCID=TT.CCID AND CCT.AccountHeadID=TT.ParentHeadID AND CONVERT(DATE, CONVERT(VARCHAR(12),CCT.TransactionDate,106)) BETWEEN  CONVERT(DATE, CONVERT(VARCHAR(12),@SessionStartDate,106)) AND CONVERT(DATE, CONVERT(VARCHAR(12),DATEADD(DAY,-1,@StartDate),106))),0)
			FROM #TempTable2 AS TT
	
			UPDATE #TempTable
			SET DebitAmount=ISNULL((SELECT SUM(CCT.Amount*CCT.CurrencyConversionRate) FROM View_CostCenterTransaction AS CCT WHERE  ISNULL(CCT.ApprovedBy,0)!=0 AND  CCT.IsDr=1 AND CCT.CCID=TT.CCID AND CCT.AccountHeadID=TT.ParentHeadID AND CONVERT(DATE, CONVERT(VARCHAR(12),CCT.TransactionDate,106)) BETWEEN  CONVERT(DATE, CONVERT(VARCHAR(12),@StartDate,106)) AND CONVERT(DATE, CONVERT(VARCHAR(12),@EndDate,106))),0),
				CreditAmount=ISNULL((SELECT SUM(CCT.Amount*CCT.CurrencyConversionRate) FROM View_CostCenterTransaction AS CCT WHERE  ISNULL(CCT.ApprovedBy,0)!=0 AND  CCT.IsDr=0 AND CCT.CCID=TT.CCID AND CCT.AccountHeadID=TT.ParentHeadID AND CONVERT(DATE, CONVERT(VARCHAR(12),CCT.TransactionDate,106)) BETWEEN  CONVERT(DATE, CONVERT(VARCHAR(12),@StartDate,106)) AND CONVERT(DATE, CONVERT(VARCHAR(12),@EndDate,106))),0)
			FROM #TempTable AS TT

		END
		ELSE
		BEGIN
			UPDATE #TempTable2
			SET DebitOpeiningValue=ISNULL((SELECT SUM(AOB.AmountInCurrency) FROM AccountOpenningBreakdown AS AOB WHERE AOB.IsDr=1 AND AOB.CurrencyID=@CurrencyID AND AOB.BreakdownObjID=TT.CCID AND AOB.AccountHeadID=TT.ParentHeadID AND AOB.AccountingSessionID=@SessionID AND AOB.BreakdownType=1),0),
				CreditOpeiningValue=ISNULL((SELECT SUM(AOB.AmountInCurrency) FROM AccountOpenningBreakdown AS AOB WHERE AOB.IsDr=0 AND AOB.CurrencyID=@CurrencyID AND AOB.BreakdownObjID=TT.CCID AND AOB.AccountHeadID=TT.ParentHeadID AND AOB.AccountingSessionID=@SessionID AND AOB.BreakdownType=1),0),
				DebitAmount=ISNULL((SELECT SUM(CCT.Amount) FROM View_CostCenterTransaction AS CCT WHERE CCT.ApprovedBy!=0 AND CCT.CurrencyID=@CurrencyID AND  CCT.IsDr=1 AND CCT.CCID=TT.CCID AND CCT.AccountHeadID=TT.ParentHeadID AND CONVERT(DATE, CONVERT(VARCHAR(12),CCT.TransactionDate,106)) BETWEEN  CONVERT(DATE, CONVERT(VARCHAR(12),@SessionStartDate,106)) AND CONVERT(DATE, CONVERT(VARCHAR(12),DATEADD(DAY,-1,@StartDate),106))),0),
				CreditAmount=ISNULL((SELECT SUM(CCT.Amount) FROM View_CostCenterTransaction AS CCT WHERE CCT.ApprovedBy!=0 AND CCT.CurrencyID=@CurrencyID AND CCT.IsDr=0 AND CCT.CCID=TT.CCID AND CCT.AccountHeadID=TT.ParentHeadID AND CONVERT(DATE, CONVERT(VARCHAR(12),CCT.TransactionDate,106)) BETWEEN  CONVERT(DATE, CONVERT(VARCHAR(12),@SessionStartDate,106)) AND CONVERT(DATE, CONVERT(VARCHAR(12),DATEADD(DAY,-1,@StartDate),106))),0)
			FROM #TempTable2 AS TT

			UPDATE #TempTable
			SET DebitAmount=ISNULL((SELECT SUM(CCT.Amount) FROM View_CostCenterTransaction AS CCT WHERE CCT.ApprovedBy!=0 AND CCT.CurrencyID=@CurrencyID AND  CCT.IsDr=1 AND CCT.CCID=TT.CCID AND CCT.AccountHeadID=TT.ParentHeadID AND CONVERT(DATE, CONVERT(VARCHAR(12),CCT.TransactionDate,106)) BETWEEN  CONVERT(DATE, CONVERT(VARCHAR(12),@StartDate,106)) AND CONVERT(DATE, CONVERT(VARCHAR(12),@EndDate,106))),0),
				CreditAmount=ISNULL((SELECT SUM(CCT.Amount) FROM View_CostCenterTransaction AS CCT WHERE CCT.ApprovedBy!=0 AND CCT.CurrencyID=@CurrencyID AND  CCT.IsDr=0 AND CCT.CCID=TT.CCID AND CCT.AccountHeadID=TT.ParentHeadID AND CONVERT(DATE, CONVERT(VARCHAR(12),CCT.TransactionDate,106)) BETWEEN  CONVERT(DATE, CONVERT(VARCHAR(12),@StartDate,106)) AND CONVERT(DATE, CONVERT(VARCHAR(12),@EndDate,106))),0)
			FROM #TempTable AS TT
		END
	END
	ELSE
	BEGIN
		IF(@CurrencyID=@BaseCurrencyID)
		BEGIN
			UPDATE #TempTable2
			SET DebitOpeiningValue=ISNULL((SELECT SUM(AOB.OpenningBalance) FROM AccountOpenningBreakdown AS AOB WHERE AOB.IsDr=1 AND AOB.BreakdownObjID=TT.CCID AND AOB.AccountHeadID=TT.ParentHeadID AND AOB.AccountingSessionID=@SessionID AND AOB.BreakdownType=1),0),
				CreditOpeiningValue=ISNULL((SELECT SUM(AOB.OpenningBalance) FROM AccountOpenningBreakdown AS AOB WHERE AOB.IsDr=0 AND AOB.BreakdownObjID=TT.CCID AND AOB.AccountHeadID=TT.ParentHeadID AND AOB.AccountingSessionID=@SessionID AND AOB.BreakdownType=1),0),
				DebitAmount=ISNULL((SELECT SUM(CCT.Amount*CCT.CurrencyConversionRate) FROM View_CostCenterTransaction AS CCT WHERE CCT.IsDr=1 AND CCT.CCID=TT.CCID AND CCT.AccountHeadID=TT.ParentHeadID AND CONVERT(DATE, CONVERT(VARCHAR(12),CCT.TransactionDate,106)) BETWEEN  CONVERT(DATE, CONVERT(VARCHAR(12),@SessionStartDate,106)) AND CONVERT(DATE, CONVERT(VARCHAR(12),DATEADD(DAY,-1,@StartDate),106))),0),
				CreditAmount=ISNULL((SELECT SUM(CCT.Amount*CCT.CurrencyConversionRate) FROM View_CostCenterTransaction AS CCT WHERE CCT.IsDr=0 AND CCT.CCID=TT.CCID AND CCT.AccountHeadID=TT.ParentHeadID AND CONVERT(DATE, CONVERT(VARCHAR(12),CCT.TransactionDate,106)) BETWEEN  CONVERT(DATE, CONVERT(VARCHAR(12),@SessionStartDate,106)) AND CONVERT(DATE, CONVERT(VARCHAR(12),DATEADD(DAY,-1,@StartDate),106))),0)
			FROM #TempTable2 AS TT
	
			UPDATE #TempTable
			SET DebitAmount=ISNULL((SELECT SUM(CCT.Amount*CCT.CurrencyConversionRate) FROM View_CostCenterTransaction AS CCT WHERE CCT.IsDr=1 AND CCT.CCID=TT.CCID AND CCT.AccountHeadID=TT.ParentHeadID AND CONVERT(DATE, CONVERT(VARCHAR(12),CCT.TransactionDate,106)) BETWEEN  CONVERT(DATE, CONVERT(VARCHAR(12),@StartDate,106)) AND CONVERT(DATE, CONVERT(VARCHAR(12),@EndDate,106))),0),
				CreditAmount=ISNULL((SELECT SUM(CCT.Amount*CCT.CurrencyConversionRate) FROM View_CostCenterTransaction AS CCT WHERE CCT.IsDr=0 AND CCT.CCID=TT.CCID AND CCT.AccountHeadID=TT.ParentHeadID AND CONVERT(DATE, CONVERT(VARCHAR(12),CCT.TransactionDate,106)) BETWEEN  CONVERT(DATE, CONVERT(VARCHAR(12),@StartDate,106)) AND CONVERT(DATE, CONVERT(VARCHAR(12),@EndDate,106))),0)
			FROM #TempTable AS TT

		END
		ELSE
		BEGIN
			UPDATE #TempTable2
			SET DebitOpeiningValue=ISNULL((SELECT SUM(AOB.AmountInCurrency) FROM AccountOpenningBreakdown AS AOB WHERE AOB.IsDr=1 AND AOB.CurrencyID=@CurrencyID AND AOB.BreakdownObjID=TT.CCID AND AOB.AccountHeadID=TT.ParentHeadID AND AOB.AccountingSessionID=@SessionID AND AOB.BreakdownType=1),0),
				CreditOpeiningValue=ISNULL((SELECT SUM(AOB.AmountInCurrency) FROM AccountOpenningBreakdown AS AOB WHERE AOB.IsDr=0 AND AOB.CurrencyID=@CurrencyID AND AOB.BreakdownObjID=TT.CCID AND AOB.AccountHeadID=TT.ParentHeadID AND AOB.AccountingSessionID=@SessionID AND AOB.BreakdownType=1),0),
				DebitAmount=ISNULL((SELECT SUM(CCT.Amount) FROM View_CostCenterTransaction AS CCT WHERE CCT.CurrencyID=@CurrencyID AND  CCT.IsDr=1 AND CCT.CCID=TT.CCID AND CCT.AccountHeadID=TT.ParentHeadID AND CONVERT(DATE, CONVERT(VARCHAR(12),CCT.TransactionDate,106)) BETWEEN  CONVERT(DATE, CONVERT(VARCHAR(12),@SessionStartDate,106)) AND CONVERT(DATE, CONVERT(VARCHAR(12),DATEADD(DAY,-1,@StartDate),106))),0),
				CreditAmount=ISNULL((SELECT SUM(CCT.Amount) FROM View_CostCenterTransaction AS CCT WHERE CCT.CurrencyID=@CurrencyID AND CCT.IsDr=0 AND CCT.CCID=TT.CCID AND CCT.AccountHeadID=TT.ParentHeadID AND CONVERT(DATE, CONVERT(VARCHAR(12),CCT.TransactionDate,106)) BETWEEN  CONVERT(DATE, CONVERT(VARCHAR(12),@SessionStartDate,106)) AND CONVERT(DATE, CONVERT(VARCHAR(12),DATEADD(DAY,-1,@StartDate),106))),0)
			FROM #TempTable2 AS TT

			UPDATE #TempTable
			SET DebitAmount=ISNULL((SELECT SUM(CCT.Amount) FROM View_CostCenterTransaction AS CCT WHERE CCT.CurrencyID=@CurrencyID AND  CCT.IsDr=1 AND CCT.CCID=TT.CCID AND CCT.AccountHeadID=TT.ParentHeadID AND CONVERT(DATE, CONVERT(VARCHAR(12),CCT.TransactionDate,106)) BETWEEN  CONVERT(DATE, CONVERT(VARCHAR(12),@StartDate,106)) AND CONVERT(DATE, CONVERT(VARCHAR(12),@EndDate,106))),0),
				CreditAmount=ISNULL((SELECT SUM(CCT.Amount) FROM View_CostCenterTransaction AS CCT WHERE CCT.CurrencyID=@CurrencyID AND  CCT.IsDr=0 AND CCT.CCID=TT.CCID AND CCT.AccountHeadID=TT.ParentHeadID AND CONVERT(DATE, CONVERT(VARCHAR(12),CCT.TransactionDate,106)) BETWEEN  CONVERT(DATE, CONVERT(VARCHAR(12),@StartDate,106)) AND CONVERT(DATE, CONVERT(VARCHAR(12),@EndDate,106))),0)
			FROM #TempTable AS TT
		END
	END
END



	UPDATE #TempTable2 SET IsDebit=1 FROM #TempTable2 AS TT 
	UPDATE #TempTable2 SET IsDebit=0 FROM #TempTable2 AS TT WHERE TT.DebitOpeiningValue<TT.CreditOpeiningValue

	UPDATE #TempTable2 SET ClosingValue=(TT.DebitOpeiningValue-TT.CreditOpeiningValue+TT.DebitAmount-TT.CreditAmount), IsDrClosing=1 FROM #TempTable2 AS TT WHERE TT.ComponentID IN (2,6)
	UPDATE #TempTable2 SET IsDrClosing=0 FROM #TempTable2 AS TT WHERE TT.ClosingValue<0 AND TT.ComponentID IN (2,6)

	UPDATE #TempTable2 SET ClosingValue=(TT.CreditOpeiningValue-TT.DebitOpeiningValue-TT.DebitAmount+TT.CreditAmount), IsDrClosing=0 FROM #TempTable2 AS TT WHERE TT.ComponentID NOT IN (2,6)
	UPDATE #TempTable2 SET IsDrClosing=1 FROM #TempTable2 AS TT WHERE TT.ClosingValue<0	AND TT.ComponentID NOT IN (2,6)

UPDATE #TempTable
SET CCCode=(SELECT CC.Code FROM ACCostCenter AS CC WHERE CC.ACCostCenterID=TT.CCID),
	CCName=(SELECT CC.Name FROM ACCostCenter AS CC WHERE CC.ACCostCenterID=TT.CCID),
	ParentHeadName=(SELECT COA.AccountHeadName FROM ChartsOfAccount AS COA WHERE COA.AccountHeadID=TT.ParentHeadID),
	ParentHeadCode=(SELECT COA.AccountCode FROM ChartsOfAccount AS COA WHERE COA.AccountHeadID=TT.ParentHeadID),
	CurrencySymbol = (SELECT CU.Symbol FROM Currency AS CU WHERE CU.CurrencyID=@CurrencyID),
	OpeiningValue=(SELECT ABC.ClosingValue FROM #TempTable2 AS ABC WHERE ABC.CCID=TT.CCID AND ABC.ParentHeadID=TT.ParentHeadID),
	IsDebit=(SELECT ABC.IsDrClosing FROM #TempTable2 AS ABC WHERE ABC.CCID=TT.CCID AND ABC.ParentHeadID=TT.ParentHeadID)	
FROM #TempTable AS TT

	UPDATE #TempTable SET ClosingValue=(TT.OpeiningValue+TT.DebitAmount-TT.CreditAmount), IsDrClosing=1 FROM #TempTable AS TT WHERE TT.ComponentID IN (2,6)
	UPDATE #TempTable SET IsDrClosing=0 FROM #TempTable AS TT WHERE TT.ClosingValue<0 AND TT.ComponentID IN (2,6)

	UPDATE #TempTable SET ClosingValue=(TT.OpeiningValue-TT.DebitAmount+TT.CreditAmount), IsDrClosing=0 FROM #TempTable AS TT WHERE TT.ComponentID NOT IN (2,6)
	UPDATE #TempTable SET IsDrClosing=1 FROM #TempTable AS TT WHERE TT.ClosingValue<0 AND TT.ComponentID NOT IN (2,6)

--INSERT INTO #TempTable (CCID, CCName)
--				 VALUES(0, 'Grand Total')
--DECLARE
--@IsDrOV as bit,
--@IsDrCV as bit,
--@OpeiningValue as decimal(30,17),
--@ClosingValue as decimal(30,17)

--SET @OpeiningValue = (SELECT SUM(AA.OpeiningValue) FROM #TempTable AS AA WHERE AA.CCID!=0)
--SET @ClosingValue = (SELECT SUM(AA.ClosingValue) FROM #TempTable AS AA WHERE AA.CCID!=0)

--IF(ISNULL((SELECT SUM(AA.OpeiningValue) FROM #TempTable AS AA WHERE AA.CCID!=0 AND AA.ComponentID IN (2,6)),0) < 0)
--BEGIN
	    
--	SET @IsDrOV = 0
--END
--ELSE
--BEGIN
--	SET @IsDrOV = 1
--END

--IF(ISNULL((SELECT SUM(AA.ClosingValue) FROM #TempTable AS AA WHERE AA.CCID!=0 AND AA.ComponentID IN (2,6)),0) < 0)
--BEGIN
--	SET @IsDrCV = 0
--END
--ELSE
--BEGIN
--	SET @IsDrCV = 1
--END

--IF(ISNULL((SELECT SUM(AA.OpeiningValue) FROM #TempTable AS AA WHERE AA.CCID!=0 AND AA.ComponentID NOT IN (2,6)),0) < 0)
--BEGIN
	    
--	SET @IsDrOV = 1
--END
--ELSE
--BEGIN
--	SET @IsDrOV = 0
--END

--IF(ISNULL((SELECT SUM(AA.ClosingValue) FROM #TempTable AS AA WHERE AA.CCID!=0 AND AA.ComponentID NOT IN (2,6)),0) < 0)
--BEGIN
--	SET @IsDrCV = 1
--END
--ELSE
--BEGIN
--	SET @IsDrCV = 0
--END

--UPDATE #TempTable
--SET OpeiningValue=ISNULL((SELECT SUM(AA.OpeiningValue) FROM #TempTable AS AA WHERE AA.CCID!=0),0),
--	DebitAmount=ISNULL((SELECT SUM(AA.DebitAmount) FROM #TempTable AS AA WHERE AA.CCID!=0),0),
--	CreditAmount=ISNULL((SELECT SUM(AA.CreditAmount) FROM #TempTable AS AA WHERE AA.CCID!=0),0),
--	ClosingValue=ISNULL((SELECT SUM(AA.ClosingValue) FROM #TempTable AS AA WHERE AA.CCID!=0),0),
--	IsDebit=@IsDrOV,
--	IsDrClosing=@IsDrCV
--FROM #TempTable AS TT WHERE TT.CCID=0

--UPDATE #TempTable SET OpeiningValue=TT.OpeiningValue*(-1) FROM #TempTable AS TT WHERE TT.OpeiningValue<0
--UPDATE #TempTable SET ClosingValue=TT.ClosingValue*(-1) FROM #TempTable AS TT WHERE TT.ClosingValue<0

SELECT * FROM #TempTable AS TT ORDER BY TT.CCID,TT.ParentHeadID

DROP TABLE #TempTable
DROP TABLE #TempTable2
COMMIT TRAN





GO
/****** Object:  StoredProcedure [dbo].[SP_CCOpeningBreakdown]    Script Date: 12/1/2016 2:17:46 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[SP_CCOpeningBreakdown]
(
	@AccountHeadID as int,
	@SubLedgerID as int,
	@StartDate as date,
	@CurrencyID as int,
	@IsApproved as bit,
	@BusinessUnitID as int
	--%n, %n, %d, %n, %b, %n
)
AS
BEGIN TRAN
--DECLARE
--@AccountHeadID as int,
--@SubLedgerID as int,
--@StartDate as date,
--@CurrencyID as int,
--@IsApproved as bit,
--@BusinessUnitID as int

--SET @AccountHeadID=0
--SET @SubLedgerID=279
--SET @StartDate='01 JAN 2016'
--SET @CurrencyID=1
--SET @IsApproved=0
--SET @BusinessUnitID= 0

CREATE TABLE #Temp_Table(	
							AccountHeadID int,
							IsDebit bit,
							IsDrClosing bit,
							ComponentID int,
							CCID INT,
							VoucherBillID INT,
							ProductID INT,
							OrderID INT,
							BreakdownType int,
							BreakodwnID int,
							BreakdownName varchar(512),
							AccountHeadName varchar(512),														
							AccountHeadCode varchar(512),														
							CCName Varchar(128),
							CCCode Varchar(128),
							BillNo Varchar(128),
							BillDate datetime,														
							ProductName varchar(1024),
							ProductCode varchar(1024),
							OrderNo varchar(512),							
							OrderDate datetime,
							OpeningAmount decimal(30,17),
							ClosingAmount decimal(30,17),
							DebitOpeningAmount decimal(30,17),
							CreditOpeningAmount decimal(30,17),
							DebitTransactionAmount decimal(30,17),
							CreditTransactionAmount decimal(30,17),
							DebitAmount decimal(30,17),
							CreditAmount decimal(30,17),
							Debit_CCSessionAmount decimal(30,17),
							Credit_CCSessionAmount decimal(30,17),							
							Debit_CCAmount decimal(30,17),
							Credit_CCAmount decimal(30,17),							
							Debit_VBSessionAmount decimal(30,17),
							Credit_VBSessionAmount decimal(30,17),							
							Debit_VBAmount decimal(30,17),
							Credit_VBAmount decimal(30,17),							
							Debit_VPSessionAmount decimal(30,17),
							Credit_VPSessionAmount decimal(30,17),							
							Debit_VPAmount decimal(30,17),
							Credit_VPAmount decimal(30,17),							
							Debit_VOSessionAmount decimal(30,17),
							Credit_VOSessionAmount decimal(30,17),
							Debit_VOAmount decimal(30,17),
							Credit_VOAmount decimal(30,17),
							CurrentBalance decimal(30,17)
							)
			

--EnumBreakdownType
--{
--VoucherDetail = 0,
--CostCenter = 1,// BreakdownObjID will be CostCenterID, ExplanationName will be CostCenter Name,ExplanationCode will be CostCenterCode & ExplanationAmount will be User Entered Amount
--BillReference = 2, // BreakdownObjID will be BillID, ExplanationName will be BillNo & ExplanationAmount will be User Entered Amount
--ChequeReference = 3, // ExplanationName will be user entered text & ExplanationAmount will be User Entered Amount
--InventoryReference = 4, //BreakdownObjID will be ProductID, ExplanationName will be Product Name, ExplanationCode will be ProductCode, ExplanationAmount will be User Entered Amount & WorkingUnitID, Qty, UnitPrice  must be confirm   
--SubledgerBill = 5, //Only use in interface not database 
--SubledgerCheque = 6, //Only use in interface not database 
--OrderReference = 7 // BreakdownObjID will be SaleOrderID, ExplanationName will be SalesNo & ExplanationAmount will be User Entered Amount
--}
--EnumVoucherExplanationType
--{
--    VoucherDetail = 0, //Voucher detail object here Explanation data not need
--    CostCenter = 1,// ExplanationID will be CostCenterID, ExplanationName will be CostCenter Name,ExplanationCode will be CostCenterCode & ExplanationAmount will be User Entered Amount
--    BillReference = 2, // ExplanationID will be BillID, ExplanationName will be BillNo & ExplanationAmount will be User Entered Amount
--    VoucherReference = 3, // ExplanationName will be user entered text & ExplanationAmount will be User Entered Amount
--    InventoryReference = 4 //ExplanationID will be ProductID, ExplanationName will be Product Name, ExplanationCode will be ProductCode, ExplanationAmount will be User Entered Amount & WorkingUnitID, Qty, UnitPrice  must be confirm
--}

DECLARE
@SessionID as int,
@SessionStartDate as date,
@BaseCurrencyID as int,
@ComponentID as int

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

SET @BaseCurrencyID=(SELECT BaseCurrencyID FROM Company WHERE CompanyID=1)
IF(@CurrencyID<=0)
BEGIN
	SET @CurrencyID=(SELECT BaseCurrencyID FROM Company WHERE CompanyID=1)
END


IF(@AccountHeadID>0)
BEGIN
	IF(@BusinessUnitID>0)
	BEGIN
		IF(@IsApproved=1)
		BEGIN
			IF(@CurrencyID=@BaseCurrencyID)
			BEGIN
				INSERT INTO #Temp_Table(VoucherBillID,		AccountHeadID,		CCID,			BillNo,		BillDate,		BreakdownType)
					SELECT DISTINCT		VB.VoucherBillID,	VB.AccountHeadID,	VB.SubLedgerID,	VB.BillNo,	VB.BillDate,	2 FROM   View_VoucherBill as VB WHERE VB.AccountHeadID=@AccountHeadID AND VB.SubLedgerID=@SubLedgerID AND VB.RemainningBalance>0
			END
			ELSE
			BEGIN
				INSERT INTO #Temp_Table(VoucherBillID,		AccountHeadID,		CCID,			BillNo,		BillDate,		BreakdownType)
					SELECT DISTINCT		VB.VoucherBillID,	VB.AccountHeadID,	VB.SubLedgerID,	VB.BillNo,	VB.BillDate,	2 FROM   View_VoucherBill as VB WHERE VB.AccountHeadID=@AccountHeadID AND VB.SubLedgerID=@SubLedgerID AND VB.RemainningBalance>0
			END
		END
		ELSE
		BEGIN
			IF(@CurrencyID=@BaseCurrencyID)
			BEGIN
				INSERT INTO #Temp_Table(VoucherBillID,		AccountHeadID,		CCID,			BillNo,		BillDate,		BreakdownType)
					SELECT DISTINCT		VB.VoucherBillID,	VB.AccountHeadID,	VB.SubLedgerID,	VB.BillNo,	VB.BillDate,	2 FROM   View_VoucherBill as VB WHERE VB.AccountHeadID=@AccountHeadID AND VB.SubLedgerID=@SubLedgerID AND VB.RemainningBalance>0
			END
			ELSE
			BEGIN
				INSERT INTO #Temp_Table(VoucherBillID,		AccountHeadID,		CCID,			BillNo,		BillDate,		BreakdownType)
					SELECT DISTINCT		VB.VoucherBillID,	VB.AccountHeadID,	VB.SubLedgerID,	VB.BillNo,	VB.BillDate,	2 FROM   View_VoucherBill as VB WHERE VB.AccountHeadID=@AccountHeadID AND VB.SubLedgerID=@SubLedgerID AND VB.RemainningBalance>0
			END
		END
	END
	ELSE
	BEGIN
		IF(@IsApproved=1)
		BEGIN
			IF(@CurrencyID=@BaseCurrencyID)
			BEGIN
				INSERT INTO #Temp_Table(VoucherBillID,		AccountHeadID,		CCID,			BillNo,		BillDate,		BreakdownType)
					SELECT DISTINCT		VB.VoucherBillID,	VB.AccountHeadID,	VB.SubLedgerID,	VB.BillNo,	VB.BillDate,	2 FROM   View_VoucherBill as VB WHERE VB.AccountHeadID=@AccountHeadID AND VB.SubLedgerID=@SubLedgerID AND VB.RemainningBalance>0
			END
			ELSE
			BEGIN
				INSERT INTO #Temp_Table(VoucherBillID,		AccountHeadID,		CCID,			BillNo,		BillDate,		BreakdownType)
					SELECT DISTINCT		VB.VoucherBillID,	VB.AccountHeadID,	VB.SubLedgerID,	VB.BillNo,	VB.BillDate,	2 FROM   View_VoucherBill as VB WHERE VB.AccountHeadID=@AccountHeadID AND VB.SubLedgerID=@SubLedgerID AND VB.RemainningBalance>0
			END
		END
		ELSE
		BEGIN
			IF(@CurrencyID=@BaseCurrencyID)
			BEGIN
				INSERT INTO #Temp_Table(VoucherBillID,		AccountHeadID,		CCID,			BillNo,		BillDate,		BreakdownType)
					SELECT DISTINCT		VB.VoucherBillID,	VB.AccountHeadID,	VB.SubLedgerID,	VB.BillNo,	VB.BillDate,	2 FROM   View_VoucherBill as VB WHERE VB.AccountHeadID=@AccountHeadID AND VB.SubLedgerID=@SubLedgerID AND VB.RemainningBalance>0
			END
			ELSE
			BEGIN
				INSERT INTO #Temp_Table(VoucherBillID,		AccountHeadID,		CCID,			BillNo,		BillDate,		BreakdownType)
					SELECT DISTINCT		VB.VoucherBillID,	VB.AccountHeadID,	VB.SubLedgerID,	VB.BillNo,	VB.BillDate,	2 FROM   View_VoucherBill as VB WHERE VB.AccountHeadID=@AccountHeadID AND VB.SubLedgerID=@SubLedgerID AND VB.RemainningBalance>0
			END
		END
	END
END
ELSE
BEGIN
	IF(@BusinessUnitID>0)
	BEGIN
		IF(@IsApproved=1)
		BEGIN
			IF(@CurrencyID=@BaseCurrencyID)
			BEGIN
				INSERT INTO #Temp_Table(VoucherBillID,		AccountHeadID,		CCID,			BillNo,		BillDate,		BreakdownType)
					SELECT DISTINCT		VB.VoucherBillID,	VB.AccountHeadID,	VB.SubLedgerID,	VB.BillNo,	VB.BillDate,	2 FROM   View_VoucherBill as VB WHERE VB.SubLedgerID=@SubLedgerID AND VB.RemainningBalance>0
			END
			ELSE
			BEGIN
				INSERT INTO #Temp_Table(VoucherBillID,		AccountHeadID,		CCID,			BillNo,		BillDate,		BreakdownType)
					SELECT DISTINCT		VB.VoucherBillID,	VB.AccountHeadID,	VB.SubLedgerID,	VB.BillNo,	VB.BillDate,	2 FROM   View_VoucherBill as VB WHERE VB.SubLedgerID=@SubLedgerID AND VB.RemainningBalance>0
			END
		END
		ELSE
		BEGIN
			IF(@CurrencyID=@BaseCurrencyID)
			BEGIN
				INSERT INTO #Temp_Table(VoucherBillID,		AccountHeadID,		CCID,			BillNo,		BillDate,		BreakdownType)
					SELECT DISTINCT		VB.VoucherBillID,	VB.AccountHeadID,	VB.SubLedgerID,	VB.BillNo,	VB.BillDate,	2 FROM   View_VoucherBill as VB WHERE VB.SubLedgerID=@SubLedgerID AND VB.RemainningBalance>0
			END
			ELSE
			BEGIN
				INSERT INTO #Temp_Table(VoucherBillID,		AccountHeadID,		CCID,			BillNo,		BillDate,		BreakdownType)
					SELECT DISTINCT		VB.VoucherBillID,	VB.AccountHeadID,	VB.SubLedgerID,	VB.BillNo,	VB.BillDate,	2 FROM   View_VoucherBill as VB WHERE VB.SubLedgerID=@SubLedgerID AND VB.RemainningBalance>0
			END
		END
	END
	ELSE
	BEGIN
		IF(@IsApproved=1)
		BEGIN
			IF(@CurrencyID=@BaseCurrencyID)
			BEGIN
				INSERT INTO #Temp_Table(VoucherBillID,		AccountHeadID,		CCID,			BillNo,		BillDate,		BreakdownType)
					SELECT DISTINCT		VB.VoucherBillID,	VB.AccountHeadID,	VB.SubLedgerID,	VB.BillNo,	VB.BillDate,	2 FROM   View_VoucherBill as VB WHERE VB.SubLedgerID=@SubLedgerID AND VB.RemainningBalance>0
			END
			ELSE
			BEGIN
				INSERT INTO #Temp_Table(VoucherBillID,		AccountHeadID,		CCID,			BillNo,		BillDate,		BreakdownType)
					SELECT DISTINCT		VB.VoucherBillID,	VB.AccountHeadID,	VB.SubLedgerID,	VB.BillNo,	VB.BillDate,	2 FROM   View_VoucherBill as VB WHERE VB.SubLedgerID=@SubLedgerID AND VB.RemainningBalance>0
			END
		END
		ELSE
		BEGIN
			IF(@CurrencyID=@BaseCurrencyID)
			BEGIN
				INSERT INTO #Temp_Table(VoucherBillID,		AccountHeadID,		CCID,			BillNo,		BillDate,		BreakdownType)
					SELECT DISTINCT		VB.VoucherBillID,	VB.AccountHeadID,	VB.SubLedgerID,	VB.BillNo,	VB.BillDate,	2 FROM   View_VoucherBill as VB WHERE VB.SubLedgerID=@SubLedgerID AND VB.RemainningBalance>0
			END
			ELSE
			BEGIN
				INSERT INTO #Temp_Table(VoucherBillID,		AccountHeadID,		CCID,			BillNo,		BillDate,		BreakdownType)
					SELECT DISTINCT		VB.VoucherBillID,	VB.AccountHeadID,	VB.SubLedgerID,	VB.BillNo,	VB.BillDate,	2 FROM   View_VoucherBill as VB WHERE VB.SubLedgerID=@SubLedgerID AND VB.RemainningBalance>0
			END
		END
	END
END



UPDATE #Temp_Table SET ComponentID=dbo.GetComponentID(TT.AccountHeadID) FROM #Temp_Table AS TT

IF(@AccountHeadID>0)
BEGIN
	IF(@BusinessUnitID>0)
	BEGIN
		IF(@IsApproved=1)
		BEGIN
			IF(@CurrencyID=@BaseCurrencyID)
			BEGIN
				UPDATE #Temp_Table
				SET Debit_VBSessionAmount=ISNULL((SELECT SUM(AOB.OpenningBalance) FROM AccountOpenningBreakdown AS AOB WHERE AOB.IsDr=1 AND AOB.BusinessUnitID=@BusinessUnitID AND AOB.CCID=@SubLedgerID AND AOB.AccountHeadID=@AccountHeadID AND AOB.BreakdownObjID=TT.VoucherBillID AND AOB.BreakdownType=2 AND AOB.AccountingSessionID=@SessionID),0),
					Credit_VBSessionAmount=ISNULL((SELECT SUM(AOB.OpenningBalance) FROM AccountOpenningBreakdown AS AOB WHERE AOB.IsDr=0 AND AOB.BusinessUnitID=@BusinessUnitID AND AOB.CCID=@SubLedgerID AND AOB.AccountHeadID=@AccountHeadID AND AOB.BreakdownObjID=TT.VoucherBillID AND AOB.BreakdownType=2 AND AOB.AccountingSessionID=@SessionID),0),
					Debit_VBAmount=ISNULL((SELECT SUM(VBT.Amount*VBT.ConversionRate) FROM View_VoucherBillTransaction AS VBT WHERE VBT.VoucherBillID=TT.VoucherBillID AND  VBT.IsDr=1 AND  CONVERT(DATE, CONVERT(VARCHAR(12),VBT.TransactionDate,106)) BETWEEN CONVERT(DATE, CONVERT(VARCHAR(12),@SessionStartDate,106)) AND DATEADD(DAY,-1,CONVERT(DATE, CONVERT(VARCHAR(12),@StartDate,106))) AND VBT.BUID=@BusinessUnitID AND ISNULL(VBT.CCID,0)=@SubLedgerID AND VBT.AccountHeadID=@AccountHeadID AND ISNULL(VBT.ApprovedBy,0)!=0),0),
					Credit_VBAmount=ISNULL((SELECT SUM(VBT.Amount*VBT.ConversionRate) FROM View_VoucherBillTransaction AS VBT WHERE VBT.VoucherBillID=TT.VoucherBillID AND  VBT.IsDr=0 AND  CONVERT(DATE, CONVERT(VARCHAR(12),VBT.TransactionDate,106)) BETWEEN CONVERT(DATE, CONVERT(VARCHAR(12),@SessionStartDate,106)) AND DATEADD(DAY,-1,CONVERT(DATE, CONVERT(VARCHAR(12),@StartDate,106))) AND VBT.BUID=@BusinessUnitID AND ISNULL(VBT.CCID,0)=@SubLedgerID AND VBT.AccountHeadID=@AccountHeadID AND ISNULL(VBT.ApprovedBy,0)!=0),0)
				FROM #Temp_Table AS TT WHERE TT.BreakdownType=2--BILL
			END
			ELSE
			BEGIN
				UPDATE #Temp_Table
				SET Debit_VBSessionAmount=ISNULL((SELECT SUM(AOB.AmountInCurrency) FROM AccountOpenningBreakdown AS AOB WHERE AOB.IsDr=1 AND AOB.BusinessUnitID=@BusinessUnitID AND AOB.CurrencyID=@CurrencyID AND AOB.CCID=@SubLedgerID AND AOB.AccountHeadID=@AccountHeadID AND AOB.BreakdownObjID=TT.VoucherBillID AND AOB.BreakdownType=2 AND AOB.AccountingSessionID=@SessionID),0),
					Credit_VBSessionAmount=ISNULL((SELECT SUM(AOB.AmountInCurrency) FROM AccountOpenningBreakdown AS AOB WHERE AOB.IsDr=0 AND AOB.BusinessUnitID=@BusinessUnitID AND AOB.CurrencyID=@CurrencyID AND AOB.CCID=@SubLedgerID AND AOB.AccountHeadID=@AccountHeadID AND AOB.BreakdownObjID=TT.VoucherBillID AND AOB.BreakdownType=2 AND AOB.AccountingSessionID=@SessionID),0),
					Debit_VBAmount=ISNULL((SELECT SUM(VBT.Amount) FROM View_VoucherBillTransaction AS VBT WHERE VBT.CurrencyID=@CurrencyID AND VBT.VoucherBillID=TT.VoucherBillID AND  VBT.IsDr=1 AND  CONVERT(DATE, CONVERT(VARCHAR(12),VBT.TransactionDate,106)) BETWEEN CONVERT(DATE, CONVERT(VARCHAR(12),@SessionStartDate,106)) AND DATEADD(DAY,-1,CONVERT(DATE, CONVERT(VARCHAR(12),@StartDate,106))) AND VBT.BUID=@BusinessUnitID AND ISNULL(VBT.CCID,0)=@SubLedgerID AND VBT.AccountHeadID=@AccountHeadID AND ISNULL(VBT.ApprovedBy,0)!=0),0),
					Credit_VBAmount=ISNULL((SELECT SUM(VBT.Amount) FROM View_VoucherBillTransaction AS VBT WHERE VBT.CurrencyID=@CurrencyID AND VBT.VoucherBillID=TT.VoucherBillID AND  VBT.IsDr=0 AND  CONVERT(DATE, CONVERT(VARCHAR(12),VBT.TransactionDate,106)) BETWEEN CONVERT(DATE, CONVERT(VARCHAR(12),@SessionStartDate,106)) AND DATEADD(DAY,-1,CONVERT(DATE, CONVERT(VARCHAR(12),@StartDate,106))) AND VBT.BUID=@BusinessUnitID AND ISNULL(VBT.CCID,0)=@SubLedgerID AND VBT.AccountHeadID=@AccountHeadID AND ISNULL(VBT.ApprovedBy,0)!=0),0)
				FROM #Temp_Table AS TT WHERE TT.BreakdownType=2--BILL
			END
		END
		ELSE
		BEGIN
			IF(@CurrencyID=@BaseCurrencyID)
			BEGIN
				UPDATE #Temp_Table
				SET Debit_VBSessionAmount=ISNULL((SELECT SUM(AOB.OpenningBalance) FROM AccountOpenningBreakdown AS AOB WHERE AOB.IsDr=1 AND AOB.BusinessUnitID=@BusinessUnitID AND AOB.CCID=@SubLedgerID AND AOB.AccountHeadID=@AccountHeadID AND AOB.BreakdownObjID=TT.VoucherBillID AND AOB.BreakdownType=2 AND AOB.AccountingSessionID=@SessionID),0),
					Credit_VBSessionAmount=ISNULL((SELECT SUM(AOB.OpenningBalance) FROM AccountOpenningBreakdown AS AOB WHERE AOB.IsDr=0 AND AOB.BusinessUnitID=@BusinessUnitID AND AOB.CCID=@SubLedgerID AND AOB.AccountHeadID=@AccountHeadID AND AOB.BreakdownObjID=TT.VoucherBillID AND AOB.BreakdownType=2 AND AOB.AccountingSessionID=@SessionID),0),
					Debit_VBAmount=ISNULL((SELECT SUM(VBT.Amount*VBT.ConversionRate) FROM View_VoucherBillTransaction AS VBT WHERE VBT.VoucherBillID=TT.VoucherBillID AND  VBT.IsDr=1 AND  CONVERT(DATE, CONVERT(VARCHAR(12),VBT.TransactionDate,106)) BETWEEN CONVERT(DATE, CONVERT(VARCHAR(12),@SessionStartDate,106)) AND DATEADD(DAY,-1,CONVERT(DATE, CONVERT(VARCHAR(12),@StartDate,106))) AND VBT.BUID=@BusinessUnitID AND ISNULL(VBT.CCID,0)=@SubLedgerID AND VBT.AccountHeadID=@AccountHeadID),0),
					Credit_VBAmount=ISNULL((SELECT SUM(VBT.Amount*VBT.ConversionRate) FROM View_VoucherBillTransaction AS VBT WHERE VBT.VoucherBillID=TT.VoucherBillID AND  VBT.IsDr=0 AND  CONVERT(DATE, CONVERT(VARCHAR(12),VBT.TransactionDate,106)) BETWEEN CONVERT(DATE, CONVERT(VARCHAR(12),@SessionStartDate,106)) AND DATEADD(DAY,-1,CONVERT(DATE, CONVERT(VARCHAR(12),@StartDate,106))) AND VBT.BUID=@BusinessUnitID AND ISNULL(VBT.CCID,0)=@SubLedgerID AND VBT.AccountHeadID=@AccountHeadID),0)
				FROM #Temp_Table AS TT WHERE TT.BreakdownType=2--BILL
			END
			ELSE
			BEGIN
				UPDATE #Temp_Table
				SET Debit_VBSessionAmount=ISNULL((SELECT SUM(AOB.AmountInCurrency) FROM AccountOpenningBreakdown AS AOB WHERE AOB.IsDr=1 AND AOB.BusinessUnitID=@BusinessUnitID AND AOB.CurrencyID=@CurrencyID AND AOB.CCID=@SubLedgerID AND AOB.AccountHeadID=@AccountHeadID AND AOB.BreakdownObjID=TT.VoucherBillID AND AOB.BreakdownType=2 AND AOB.AccountingSessionID=@SessionID),0),
					Credit_VBSessionAmount=ISNULL((SELECT SUM(AOB.AmountInCurrency) FROM AccountOpenningBreakdown AS AOB WHERE AOB.IsDr=0 AND AOB.BusinessUnitID=@BusinessUnitID AND AOB.CurrencyID=@CurrencyID AND AOB.CCID=@SubLedgerID AND AOB.AccountHeadID=@AccountHeadID AND AOB.BreakdownObjID=TT.VoucherBillID AND AOB.BreakdownType=2 AND AOB.AccountingSessionID=@SessionID),0),
					Debit_VBAmount=ISNULL((SELECT SUM(VBT.Amount) FROM View_VoucherBillTransaction AS VBT WHERE VBT.CurrencyID=@CurrencyID AND VBT.VoucherBillID=TT.VoucherBillID AND  VBT.IsDr=1 AND  CONVERT(DATE, CONVERT(VARCHAR(12),VBT.TransactionDate,106)) BETWEEN CONVERT(DATE, CONVERT(VARCHAR(12),@SessionStartDate,106)) AND DATEADD(DAY,-1,CONVERT(DATE, CONVERT(VARCHAR(12),@StartDate,106))) AND VBT.BUID=@BusinessUnitID AND ISNULL(VBT.CCID,0)=@SubLedgerID AND VBT.AccountHeadID=@AccountHeadID),0),
					Credit_VBAmount=ISNULL((SELECT SUM(VBT.Amount) FROM View_VoucherBillTransaction AS VBT WHERE VBT.CurrencyID=@CurrencyID AND VBT.VoucherBillID=TT.VoucherBillID AND  VBT.IsDr=0 AND  CONVERT(DATE, CONVERT(VARCHAR(12),VBT.TransactionDate,106)) BETWEEN CONVERT(DATE, CONVERT(VARCHAR(12),@SessionStartDate,106)) AND DATEADD(DAY,-1,CONVERT(DATE, CONVERT(VARCHAR(12),@StartDate,106))) AND VBT.BUID=@BusinessUnitID AND ISNULL(VBT.CCID,0)=@SubLedgerID AND VBT.AccountHeadID=@AccountHeadID),0)
				FROM #Temp_Table AS TT WHERE TT.BreakdownType=2--BILL
			END
		END
	END
	ELSE
	BEGIN
		IF(@IsApproved=1)
		BEGIN
			IF(@CurrencyID=@BaseCurrencyID)
			BEGIN
				UPDATE #Temp_Table
				SET Debit_VBSessionAmount=ISNULL((SELECT SUM(AOB.OpenningBalance) FROM AccountOpenningBreakdown AS AOB WHERE AOB.IsDr=1 AND AOB.CCID=@SubLedgerID AND AOB.AccountHeadID=@AccountHeadID AND AOB.BreakdownObjID=TT.VoucherBillID AND AOB.BreakdownType=2 AND AOB.AccountingSessionID=@SessionID),0),
					Credit_VBSessionAmount=ISNULL((SELECT SUM(AOB.OpenningBalance) FROM AccountOpenningBreakdown AS AOB WHERE AOB.IsDr=0 AND AOB.CCID=@SubLedgerID AND AOB.AccountHeadID=@AccountHeadID AND AOB.BreakdownObjID=TT.VoucherBillID AND AOB.BreakdownType=2 AND AOB.AccountingSessionID=@SessionID),0),
					Debit_VBAmount=ISNULL((SELECT SUM(VBT.Amount*VBT.ConversionRate) FROM View_VoucherBillTransaction AS VBT WHERE VBT.VoucherBillID=TT.VoucherBillID AND  VBT.IsDr=1 AND  CONVERT(DATE, CONVERT(VARCHAR(12),VBT.TransactionDate,106)) BETWEEN CONVERT(DATE, CONVERT(VARCHAR(12),@SessionStartDate,106)) AND DATEADD(DAY,-1,CONVERT(DATE, CONVERT(VARCHAR(12),@StartDate,106))) AND ISNULL(VBT.CCID,0)=@SubLedgerID AND VBT.AccountHeadID=@AccountHeadID AND ISNULL(VBT.ApprovedBy,0)!=0),0),
					Credit_VBAmount=ISNULL((SELECT SUM(VBT.Amount*VBT.ConversionRate) FROM View_VoucherBillTransaction AS VBT WHERE VBT.VoucherBillID=TT.VoucherBillID AND  VBT.IsDr=0 AND  CONVERT(DATE, CONVERT(VARCHAR(12),VBT.TransactionDate,106)) BETWEEN CONVERT(DATE, CONVERT(VARCHAR(12),@SessionStartDate,106)) AND DATEADD(DAY,-1,CONVERT(DATE, CONVERT(VARCHAR(12),@StartDate,106))) AND ISNULL(VBT.CCID,0)=@SubLedgerID AND VBT.AccountHeadID=@AccountHeadID AND ISNULL(VBT.ApprovedBy,0)!=0),0)
				FROM #Temp_Table AS TT WHERE TT.BreakdownType=2--BILL
			END
			ELSE
			BEGIN
				UPDATE #Temp_Table
				SET Debit_VBSessionAmount=ISNULL((SELECT SUM(AOB.AmountInCurrency) FROM AccountOpenningBreakdown AS AOB WHERE AOB.IsDr=1 AND AOB.CurrencyID=@CurrencyID AND AOB.CCID=@SubLedgerID AND AOB.AccountHeadID=@AccountHeadID AND AOB.BreakdownObjID=TT.VoucherBillID AND AOB.BreakdownType=2 AND AOB.AccountingSessionID=@SessionID),0),
					Credit_VBSessionAmount=ISNULL((SELECT SUM(AOB.AmountInCurrency) FROM AccountOpenningBreakdown AS AOB WHERE AOB.IsDr=0 AND AOB.CurrencyID=@CurrencyID AND AOB.CCID=@SubLedgerID AND AOB.AccountHeadID=@AccountHeadID AND AOB.BreakdownObjID=TT.VoucherBillID AND AOB.BreakdownType=2 AND AOB.AccountingSessionID=@SessionID),0),
					Debit_VBAmount=ISNULL((SELECT SUM(VBT.Amount) FROM View_VoucherBillTransaction AS VBT WHERE VBT.CurrencyID=@CurrencyID AND VBT.VoucherBillID=TT.VoucherBillID AND  VBT.IsDr=1 AND  CONVERT(DATE, CONVERT(VARCHAR(12),VBT.TransactionDate,106)) BETWEEN CONVERT(DATE, CONVERT(VARCHAR(12),@SessionStartDate,106)) AND DATEADD(DAY,-1,CONVERT(DATE, CONVERT(VARCHAR(12),@StartDate,106))) AND ISNULL(VBT.CCID,0)=@SubLedgerID AND VBT.AccountHeadID=@AccountHeadID AND ISNULL(VBT.ApprovedBy,0)!=0),0),
					Credit_VBAmount=ISNULL((SELECT SUM(VBT.Amount) FROM View_VoucherBillTransaction AS VBT WHERE VBT.CurrencyID=@CurrencyID AND VBT.VoucherBillID=TT.VoucherBillID AND  VBT.IsDr=0 AND  CONVERT(DATE, CONVERT(VARCHAR(12),VBT.TransactionDate,106)) BETWEEN CONVERT(DATE, CONVERT(VARCHAR(12),@SessionStartDate,106)) AND DATEADD(DAY,-1,CONVERT(DATE, CONVERT(VARCHAR(12),@StartDate,106))) AND ISNULL(VBT.CCID,0)=@SubLedgerID AND VBT.AccountHeadID=@AccountHeadID AND ISNULL(VBT.ApprovedBy,0)!=0),0)
				FROM #Temp_Table AS TT WHERE TT.BreakdownType=2--BILL
			END
		END
		ELSE
		BEGIN
			IF(@CurrencyID=@BaseCurrencyID)
			BEGIN
				UPDATE #Temp_Table
				SET Debit_VBSessionAmount=ISNULL((SELECT SUM(AOB.OpenningBalance) FROM AccountOpenningBreakdown AS AOB WHERE AOB.IsDr=1 AND AOB.CCID=@SubLedgerID AND AOB.AccountHeadID=@AccountHeadID AND AOB.BreakdownObjID=TT.VoucherBillID AND AOB.BreakdownType=2 AND AOB.AccountingSessionID=@SessionID),0),
					Credit_VBSessionAmount=ISNULL((SELECT SUM(AOB.OpenningBalance) FROM AccountOpenningBreakdown AS AOB WHERE AOB.IsDr=0 AND AOB.CCID=@SubLedgerID AND AOB.AccountHeadID=@AccountHeadID AND AOB.BreakdownObjID=TT.VoucherBillID AND AOB.BreakdownType=2 AND AOB.AccountingSessionID=@SessionID),0),
					Debit_VBAmount=ISNULL((SELECT SUM(VBT.Amount*VBT.ConversionRate) FROM View_VoucherBillTransaction AS VBT WHERE VBT.VoucherBillID=TT.VoucherBillID AND  VBT.IsDr=1 AND  CONVERT(DATE, CONVERT(VARCHAR(12),VBT.TransactionDate,106)) BETWEEN CONVERT(DATE, CONVERT(VARCHAR(12),@SessionStartDate,106)) AND DATEADD(DAY,-1,CONVERT(DATE, CONVERT(VARCHAR(12),@StartDate,106))) AND ISNULL(VBT.CCID,0)=@SubLedgerID AND VBT.AccountHeadID=@AccountHeadID),0),
					Credit_VBAmount=ISNULL((SELECT SUM(VBT.Amount*VBT.ConversionRate) FROM View_VoucherBillTransaction AS VBT WHERE VBT.VoucherBillID=TT.VoucherBillID AND  VBT.IsDr=0 AND  CONVERT(DATE, CONVERT(VARCHAR(12),VBT.TransactionDate,106)) BETWEEN CONVERT(DATE, CONVERT(VARCHAR(12),@SessionStartDate,106)) AND DATEADD(DAY,-1,CONVERT(DATE, CONVERT(VARCHAR(12),@StartDate,106))) AND ISNULL(VBT.CCID,0)=@SubLedgerID AND VBT.AccountHeadID=@AccountHeadID),0)
				FROM #Temp_Table AS TT WHERE TT.BreakdownType=2--BILL
			END
			ELSE
			BEGIN
				UPDATE #Temp_Table
				SET Debit_VBSessionAmount=ISNULL((SELECT SUM(AOB.AmountInCurrency) FROM AccountOpenningBreakdown AS AOB WHERE AOB.IsDr=1 AND AOB.CurrencyID=@CurrencyID AND AOB.CCID=@SubLedgerID AND AOB.AccountHeadID=@AccountHeadID AND AOB.BreakdownObjID=TT.VoucherBillID AND AOB.BreakdownType=2 AND AOB.AccountingSessionID=@SessionID),0),
					Credit_VBSessionAmount=ISNULL((SELECT SUM(AOB.AmountInCurrency) FROM AccountOpenningBreakdown AS AOB WHERE AOB.IsDr=0 AND AOB.CurrencyID=@CurrencyID AND AOB.CCID=@SubLedgerID AND AOB.AccountHeadID=@AccountHeadID AND AOB.BreakdownObjID=TT.VoucherBillID AND AOB.BreakdownType=2 AND AOB.AccountingSessionID=@SessionID),0),
					Debit_VBAmount=ISNULL((SELECT SUM(VBT.Amount) FROM View_VoucherBillTransaction AS VBT WHERE VBT.CurrencyID=@CurrencyID AND VBT.VoucherBillID=TT.VoucherBillID AND  VBT.IsDr=1 AND  CONVERT(DATE, CONVERT(VARCHAR(12),VBT.TransactionDate,106)) BETWEEN CONVERT(DATE, CONVERT(VARCHAR(12),@SessionStartDate,106)) AND DATEADD(DAY,-1,CONVERT(DATE, CONVERT(VARCHAR(12),@StartDate,106))) AND ISNULL(VBT.CCID,0)=@SubLedgerID AND VBT.AccountHeadID=@AccountHeadID),0),
					Credit_VBAmount=ISNULL((SELECT SUM(VBT.Amount) FROM View_VoucherBillTransaction AS VBT WHERE VBT.CurrencyID=@CurrencyID AND VBT.VoucherBillID=TT.VoucherBillID AND  VBT.IsDr=0 AND  CONVERT(DATE, CONVERT(VARCHAR(12),VBT.TransactionDate,106)) BETWEEN CONVERT(DATE, CONVERT(VARCHAR(12),@SessionStartDate,106)) AND DATEADD(DAY,-1,CONVERT(DATE, CONVERT(VARCHAR(12),@StartDate,106))) AND ISNULL(VBT.CCID,0)=@SubLedgerID AND VBT.AccountHeadID=@AccountHeadID),0)
				FROM #Temp_Table AS TT WHERE TT.BreakdownType=2--BILL
			END
		END
	END
END
ELSE
BEGIN
	IF(@BusinessUnitID>0)
	BEGIN
		IF(@IsApproved=1)
		BEGIN
			IF(@CurrencyID=@BaseCurrencyID)
			BEGIN
				UPDATE #Temp_Table
				SET Debit_VBSessionAmount=ISNULL((SELECT SUM(AOB.OpenningBalance) FROM AccountOpenningBreakdown AS AOB WHERE AOB.IsDr=1 AND AOB.BusinessUnitID=@BusinessUnitID AND AOB.CCID=@SubLedgerID AND AOB.BreakdownObjID=TT.VoucherBillID AND AOB.BreakdownType=2 AND AOB.AccountingSessionID=@SessionID),0),
					Credit_VBSessionAmount=ISNULL((SELECT SUM(AOB.OpenningBalance) FROM AccountOpenningBreakdown AS AOB WHERE AOB.IsDr=0 AND AOB.BusinessUnitID=@BusinessUnitID AND AOB.CCID=@SubLedgerID AND AOB.BreakdownObjID=TT.VoucherBillID AND AOB.BreakdownType=2 AND AOB.AccountingSessionID=@SessionID),0),
					Debit_VBAmount=ISNULL((SELECT SUM(VBT.Amount*VBT.ConversionRate) FROM View_VoucherBillTransaction AS VBT WHERE VBT.VoucherBillID=TT.VoucherBillID AND  VBT.IsDr=1 AND  CONVERT(DATE, CONVERT(VARCHAR(12),VBT.TransactionDate,106)) BETWEEN CONVERT(DATE, CONVERT(VARCHAR(12),@SessionStartDate,106)) AND DATEADD(DAY,-1,CONVERT(DATE, CONVERT(VARCHAR(12),@StartDate,106))) AND VBT.BUID=@BusinessUnitID AND ISNULL(VBT.CCID,0)=@SubLedgerID AND ISNULL(VBT.ApprovedBy,0)!=0),0),
					Credit_VBAmount=ISNULL((SELECT SUM(VBT.Amount*VBT.ConversionRate) FROM View_VoucherBillTransaction AS VBT WHERE VBT.VoucherBillID=TT.VoucherBillID AND  VBT.IsDr=0 AND  CONVERT(DATE, CONVERT(VARCHAR(12),VBT.TransactionDate,106)) BETWEEN CONVERT(DATE, CONVERT(VARCHAR(12),@SessionStartDate,106)) AND DATEADD(DAY,-1,CONVERT(DATE, CONVERT(VARCHAR(12),@StartDate,106))) AND VBT.BUID=@BusinessUnitID AND ISNULL(VBT.CCID,0)=@SubLedgerID AND ISNULL(VBT.ApprovedBy,0)!=0),0)
				FROM #Temp_Table AS TT WHERE TT.BreakdownType=2--BILL
			END
			ELSE
			BEGIN
				UPDATE #Temp_Table
				SET Debit_VBSessionAmount=ISNULL((SELECT SUM(AOB.AmountInCurrency) FROM AccountOpenningBreakdown AS AOB WHERE AOB.IsDr=1 AND AOB.BusinessUnitID=@BusinessUnitID AND AOB.CurrencyID=@CurrencyID AND AOB.CCID=@SubLedgerID AND AOB.AccountHeadID=@AccountHeadID AND AOB.BreakdownObjID=TT.VoucherBillID AND AOB.BreakdownType=2 AND AOB.AccountingSessionID=@SessionID),0),
					Credit_VBSessionAmount=ISNULL((SELECT SUM(AOB.AmountInCurrency) FROM AccountOpenningBreakdown AS AOB WHERE AOB.IsDr=0 AND AOB.BusinessUnitID=@BusinessUnitID AND AOB.CurrencyID=@CurrencyID AND AOB.CCID=@SubLedgerID AND AOB.AccountHeadID=@AccountHeadID AND AOB.BreakdownObjID=TT.VoucherBillID AND AOB.BreakdownType=2 AND AOB.AccountingSessionID=@SessionID),0),
					Debit_VBAmount=ISNULL((SELECT SUM(VBT.Amount) FROM View_VoucherBillTransaction AS VBT WHERE VBT.CurrencyID=@CurrencyID AND VBT.VoucherBillID=TT.VoucherBillID AND  VBT.IsDr=1 AND  CONVERT(DATE, CONVERT(VARCHAR(12),VBT.TransactionDate,106)) BETWEEN CONVERT(DATE, CONVERT(VARCHAR(12),@SessionStartDate,106)) AND DATEADD(DAY,-1,CONVERT(DATE, CONVERT(VARCHAR(12),@StartDate,106))) AND VBT.BUID=@BusinessUnitID AND ISNULL(VBT.CCID,0)=@SubLedgerID AND ISNULL(VBT.ApprovedBy,0)!=0),0),
					Credit_VBAmount=ISNULL((SELECT SUM(VBT.Amount) FROM View_VoucherBillTransaction AS VBT WHERE VBT.CurrencyID=@CurrencyID AND VBT.VoucherBillID=TT.VoucherBillID AND  VBT.IsDr=0 AND  CONVERT(DATE, CONVERT(VARCHAR(12),VBT.TransactionDate,106)) BETWEEN CONVERT(DATE, CONVERT(VARCHAR(12),@SessionStartDate,106)) AND DATEADD(DAY,-1,CONVERT(DATE, CONVERT(VARCHAR(12),@StartDate,106))) AND VBT.BUID=@BusinessUnitID AND ISNULL(VBT.CCID,0)=@SubLedgerID AND ISNULL(VBT.ApprovedBy,0)!=0),0)
				FROM #Temp_Table AS TT WHERE TT.BreakdownType=2--BILL
			END
		END
		ELSE
		BEGIN
			IF(@CurrencyID=@BaseCurrencyID)
			BEGIN
				UPDATE #Temp_Table
				SET Debit_VBSessionAmount=ISNULL((SELECT SUM(AOB.OpenningBalance) FROM AccountOpenningBreakdown AS AOB WHERE AOB.IsDr=1 AND AOB.BusinessUnitID=@BusinessUnitID AND AOB.CCID=@SubLedgerID AND AOB.AccountHeadID=@AccountHeadID AND AOB.BreakdownObjID=TT.VoucherBillID AND AOB.BreakdownType=2 AND AOB.AccountingSessionID=@SessionID),0),
					Credit_VBSessionAmount=ISNULL((SELECT SUM(AOB.OpenningBalance) FROM AccountOpenningBreakdown AS AOB WHERE AOB.IsDr=0 AND AOB.BusinessUnitID=@BusinessUnitID AND AOB.CCID=@SubLedgerID AND AOB.AccountHeadID=@AccountHeadID AND AOB.BreakdownObjID=TT.VoucherBillID AND AOB.BreakdownType=2 AND AOB.AccountingSessionID=@SessionID),0),
					Debit_VBAmount=ISNULL((SELECT SUM(VBT.Amount*VBT.ConversionRate) FROM View_VoucherBillTransaction AS VBT WHERE VBT.VoucherBillID=TT.VoucherBillID AND  VBT.IsDr=1 AND  CONVERT(DATE, CONVERT(VARCHAR(12),VBT.TransactionDate,106)) BETWEEN CONVERT(DATE, CONVERT(VARCHAR(12),@SessionStartDate,106)) AND DATEADD(DAY,-1,CONVERT(DATE, CONVERT(VARCHAR(12),@StartDate,106))) AND VBT.BUID=@BusinessUnitID AND ISNULL(VBT.CCID,0)=@SubLedgerID),0),
					Credit_VBAmount=ISNULL((SELECT SUM(VBT.Amount*VBT.ConversionRate) FROM View_VoucherBillTransaction AS VBT WHERE VBT.VoucherBillID=TT.VoucherBillID AND  VBT.IsDr=0 AND  CONVERT(DATE, CONVERT(VARCHAR(12),VBT.TransactionDate,106)) BETWEEN CONVERT(DATE, CONVERT(VARCHAR(12),@SessionStartDate,106)) AND DATEADD(DAY,-1,CONVERT(DATE, CONVERT(VARCHAR(12),@StartDate,106))) AND VBT.BUID=@BusinessUnitID AND ISNULL(VBT.CCID,0)=@SubLedgerID),0)
				FROM #Temp_Table AS TT WHERE TT.BreakdownType=2--BILL
			END
			ELSE
			BEGIN
				UPDATE #Temp_Table
				SET Debit_VBSessionAmount=ISNULL((SELECT SUM(AOB.AmountInCurrency) FROM AccountOpenningBreakdown AS AOB WHERE AOB.IsDr=1 AND AOB.BusinessUnitID=@BusinessUnitID AND AOB.CurrencyID=@CurrencyID AND AOB.CCID=@SubLedgerID AND AOB.AccountHeadID=@AccountHeadID AND AOB.BreakdownObjID=TT.VoucherBillID AND AOB.BreakdownType=2 AND AOB.AccountingSessionID=@SessionID),0),
					Credit_VBSessionAmount=ISNULL((SELECT SUM(AOB.AmountInCurrency) FROM AccountOpenningBreakdown AS AOB WHERE AOB.IsDr=0 AND AOB.BusinessUnitID=@BusinessUnitID AND AOB.CurrencyID=@CurrencyID AND AOB.CCID=@SubLedgerID AND AOB.AccountHeadID=@AccountHeadID AND AOB.BreakdownObjID=TT.VoucherBillID AND AOB.BreakdownType=2 AND AOB.AccountingSessionID=@SessionID),0),
					Debit_VBAmount=ISNULL((SELECT SUM(VBT.Amount) FROM View_VoucherBillTransaction AS VBT WHERE VBT.CurrencyID=@CurrencyID AND VBT.VoucherBillID=TT.VoucherBillID AND  VBT.IsDr=1 AND  CONVERT(DATE, CONVERT(VARCHAR(12),VBT.TransactionDate,106)) BETWEEN CONVERT(DATE, CONVERT(VARCHAR(12),@SessionStartDate,106)) AND DATEADD(DAY,-1,CONVERT(DATE, CONVERT(VARCHAR(12),@StartDate,106))) AND VBT.BUID=@BusinessUnitID AND ISNULL(VBT.CCID,0)=@SubLedgerID),0),
					Credit_VBAmount=ISNULL((SELECT SUM(VBT.Amount) FROM View_VoucherBillTransaction AS VBT WHERE VBT.CurrencyID=@CurrencyID AND VBT.VoucherBillID=TT.VoucherBillID AND  VBT.IsDr=0 AND  CONVERT(DATE, CONVERT(VARCHAR(12),VBT.TransactionDate,106)) BETWEEN CONVERT(DATE, CONVERT(VARCHAR(12),@SessionStartDate,106)) AND DATEADD(DAY,-1,CONVERT(DATE, CONVERT(VARCHAR(12),@StartDate,106))) AND VBT.BUID=@BusinessUnitID AND ISNULL(VBT.CCID,0)=@SubLedgerID),0)
				FROM #Temp_Table AS TT WHERE TT.BreakdownType=2--BILL
			END
		END
	END
	ELSE
	BEGIN
		IF(@IsApproved=1)
		BEGIN
			IF(@CurrencyID=@BaseCurrencyID)
			BEGIN
				UPDATE #Temp_Table
				SET Debit_VBSessionAmount=ISNULL((SELECT SUM(AOB.OpenningBalance) FROM AccountOpenningBreakdown AS AOB WHERE AOB.IsDr=1 AND AOB.CCID=@SubLedgerID AND AOB.AccountHeadID=@AccountHeadID AND AOB.BreakdownObjID=TT.VoucherBillID AND AOB.BreakdownType=2 AND AOB.AccountingSessionID=@SessionID),0),
					Credit_VBSessionAmount=ISNULL((SELECT SUM(AOB.OpenningBalance) FROM AccountOpenningBreakdown AS AOB WHERE AOB.IsDr=0 AND AOB.CCID=@SubLedgerID AND AOB.AccountHeadID=@AccountHeadID AND AOB.BreakdownObjID=TT.VoucherBillID AND AOB.BreakdownType=2 AND AOB.AccountingSessionID=@SessionID),0),
					Debit_VBAmount=ISNULL((SELECT SUM(VBT.Amount*VBT.ConversionRate) FROM View_VoucherBillTransaction AS VBT WHERE VBT.VoucherBillID=TT.VoucherBillID AND  VBT.IsDr=1 AND  CONVERT(DATE, CONVERT(VARCHAR(12),VBT.TransactionDate,106)) BETWEEN CONVERT(DATE, CONVERT(VARCHAR(12),@SessionStartDate,106)) AND DATEADD(DAY,-1,CONVERT(DATE, CONVERT(VARCHAR(12),@StartDate,106))) AND ISNULL(VBT.CCID,0)=@SubLedgerID AND ISNULL(VBT.ApprovedBy,0)!=0),0),
					Credit_VBAmount=ISNULL((SELECT SUM(VBT.Amount*VBT.ConversionRate) FROM View_VoucherBillTransaction AS VBT WHERE VBT.VoucherBillID=TT.VoucherBillID AND  VBT.IsDr=0 AND  CONVERT(DATE, CONVERT(VARCHAR(12),VBT.TransactionDate,106)) BETWEEN CONVERT(DATE, CONVERT(VARCHAR(12),@SessionStartDate,106)) AND DATEADD(DAY,-1,CONVERT(DATE, CONVERT(VARCHAR(12),@StartDate,106))) AND ISNULL(VBT.CCID,0)=@SubLedgerID AND ISNULL(VBT.ApprovedBy,0)!=0),0)
				FROM #Temp_Table AS TT WHERE TT.BreakdownType=2--BILL
			END
			ELSE
			BEGIN
				UPDATE #Temp_Table
				SET Debit_VBSessionAmount=ISNULL((SELECT SUM(AOB.AmountInCurrency) FROM AccountOpenningBreakdown AS AOB WHERE AOB.IsDr=1 AND AOB.CurrencyID=@CurrencyID AND AOB.CCID=@SubLedgerID AND AOB.AccountHeadID=@AccountHeadID AND AOB.BreakdownObjID=TT.VoucherBillID AND AOB.BreakdownType=2 AND AOB.AccountingSessionID=@SessionID),0),
					Credit_VBSessionAmount=ISNULL((SELECT SUM(AOB.AmountInCurrency) FROM AccountOpenningBreakdown AS AOB WHERE AOB.IsDr=0 AND AOB.CurrencyID=@CurrencyID AND AOB.CCID=@SubLedgerID AND AOB.AccountHeadID=@AccountHeadID AND AOB.BreakdownObjID=TT.VoucherBillID AND AOB.BreakdownType=2 AND AOB.AccountingSessionID=@SessionID),0),
					Debit_VBAmount=ISNULL((SELECT SUM(VBT.Amount) FROM View_VoucherBillTransaction AS VBT WHERE VBT.CurrencyID=@CurrencyID AND VBT.VoucherBillID=TT.VoucherBillID AND  VBT.IsDr=1 AND  CONVERT(DATE, CONVERT(VARCHAR(12),VBT.TransactionDate,106)) BETWEEN CONVERT(DATE, CONVERT(VARCHAR(12),@SessionStartDate,106)) AND DATEADD(DAY,-1,CONVERT(DATE, CONVERT(VARCHAR(12),@StartDate,106))) AND ISNULL(VBT.CCID,0)=@SubLedgerID AND ISNULL(VBT.ApprovedBy,0)!=0),0),
					Credit_VBAmount=ISNULL((SELECT SUM(VBT.Amount) FROM View_VoucherBillTransaction AS VBT WHERE VBT.CurrencyID=@CurrencyID AND VBT.VoucherBillID=TT.VoucherBillID AND  VBT.IsDr=0 AND  CONVERT(DATE, CONVERT(VARCHAR(12),VBT.TransactionDate,106)) BETWEEN CONVERT(DATE, CONVERT(VARCHAR(12),@SessionStartDate,106)) AND DATEADD(DAY,-1,CONVERT(DATE, CONVERT(VARCHAR(12),@StartDate,106))) AND ISNULL(VBT.CCID,0)=@SubLedgerID AND ISNULL(VBT.ApprovedBy,0)!=0),0)
				FROM #Temp_Table AS TT WHERE TT.BreakdownType=2--BILL
			END
		END
		ELSE
		BEGIN
			IF(@CurrencyID=@BaseCurrencyID)
			BEGIN
				UPDATE #Temp_Table
				SET Debit_VBSessionAmount=ISNULL((SELECT SUM(AOB.OpenningBalance) FROM AccountOpenningBreakdown AS AOB WHERE AOB.IsDr=1 AND AOB.CCID=@SubLedgerID AND AOB.AccountHeadID=@AccountHeadID AND AOB.BreakdownObjID=TT.VoucherBillID AND AOB.BreakdownType=2 AND AOB.AccountingSessionID=@SessionID),0),
					Credit_VBSessionAmount=ISNULL((SELECT SUM(AOB.OpenningBalance) FROM AccountOpenningBreakdown AS AOB WHERE AOB.IsDr=0 AND AOB.CCID=@SubLedgerID AND AOB.AccountHeadID=@AccountHeadID AND AOB.BreakdownObjID=TT.VoucherBillID AND AOB.BreakdownType=2 AND AOB.AccountingSessionID=@SessionID),0),
					Debit_VBAmount=ISNULL((SELECT SUM(VBT.Amount*VBT.ConversionRate) FROM View_VoucherBillTransaction AS VBT WHERE VBT.VoucherBillID=TT.VoucherBillID AND  VBT.IsDr=1 AND  CONVERT(DATE, CONVERT(VARCHAR(12),VBT.TransactionDate,106)) BETWEEN CONVERT(DATE, CONVERT(VARCHAR(12),@SessionStartDate,106)) AND DATEADD(DAY,-1,CONVERT(DATE, CONVERT(VARCHAR(12),@StartDate,106))) AND ISNULL(VBT.CCID,0)=@SubLedgerID),0),
					Credit_VBAmount=ISNULL((SELECT SUM(VBT.Amount*VBT.ConversionRate) FROM View_VoucherBillTransaction AS VBT WHERE VBT.VoucherBillID=TT.VoucherBillID AND  VBT.IsDr=0 AND  CONVERT(DATE, CONVERT(VARCHAR(12),VBT.TransactionDate,106)) BETWEEN CONVERT(DATE, CONVERT(VARCHAR(12),@SessionStartDate,106)) AND DATEADD(DAY,-1,CONVERT(DATE, CONVERT(VARCHAR(12),@StartDate,106))) AND ISNULL(VBT.CCID,0)=@SubLedgerID),0)
				FROM #Temp_Table AS TT WHERE TT.BreakdownType=2--BILL
			END
			ELSE
			BEGIN
				UPDATE #Temp_Table
				SET Debit_VBSessionAmount=ISNULL((SELECT SUM(AOB.AmountInCurrency) FROM AccountOpenningBreakdown AS AOB WHERE AOB.IsDr=1 AND AOB.CurrencyID=@CurrencyID AND AOB.CCID=@SubLedgerID AND AOB.AccountHeadID=@AccountHeadID AND AOB.BreakdownObjID=TT.VoucherBillID AND AOB.BreakdownType=2 AND AOB.AccountingSessionID=@SessionID),0),
					Credit_VBSessionAmount=ISNULL((SELECT SUM(AOB.AmountInCurrency) FROM AccountOpenningBreakdown AS AOB WHERE AOB.IsDr=0 AND AOB.CurrencyID=@CurrencyID AND AOB.CCID=@SubLedgerID AND AOB.AccountHeadID=@AccountHeadID AND AOB.BreakdownObjID=TT.VoucherBillID AND AOB.BreakdownType=2 AND AOB.AccountingSessionID=@SessionID),0),
					Debit_VBAmount=ISNULL((SELECT SUM(VBT.Amount) FROM View_VoucherBillTransaction AS VBT WHERE VBT.CurrencyID=@CurrencyID AND VBT.VoucherBillID=TT.VoucherBillID AND  VBT.IsDr=1 AND  CONVERT(DATE, CONVERT(VARCHAR(12),VBT.TransactionDate,106)) BETWEEN CONVERT(DATE, CONVERT(VARCHAR(12),@SessionStartDate,106)) AND DATEADD(DAY,-1,CONVERT(DATE, CONVERT(VARCHAR(12),@StartDate,106))) AND ISNULL(VBT.CCID,0)=@SubLedgerID),0),
					Credit_VBAmount=ISNULL((SELECT SUM(VBT.Amount) FROM View_VoucherBillTransaction AS VBT WHERE VBT.CurrencyID=@CurrencyID AND VBT.VoucherBillID=TT.VoucherBillID AND  VBT.IsDr=0 AND  CONVERT(DATE, CONVERT(VARCHAR(12),VBT.TransactionDate,106)) BETWEEN CONVERT(DATE, CONVERT(VARCHAR(12),@SessionStartDate,106)) AND DATEADD(DAY,-1,CONVERT(DATE, CONVERT(VARCHAR(12),@StartDate,106))) AND ISNULL(VBT.CCID,0)=@SubLedgerID),0)
				FROM #Temp_Table AS TT WHERE TT.BreakdownType=2--BILL
			END
		END
	END
END




UPDATE #Temp_Table
SET BreakodwnID=TT.CCID,
	BreakdownName=TT.CCName,
	DebitOpeningAmount=TT.Debit_CCSessionAmount,
	CreditOpeningAmount=TT.Credit_CCSessionAmount,
	DebitTransactionAmount=TT.Debit_CCAmount,
	CreditTransactionAmount=TT.Credit_CCAmount
	--DebitAmount=TT.Debit_CCSessionAmount+TT.Debit_CCAmount,
	--CreditAmount=TT.Credit_CCSessionAmount+TT.Credit_CCAmount
FROM #Temp_Table AS TT WHERE TT.BreakdownType=1--CC


UPDATE #Temp_Table
SET BreakodwnID=TT.VoucherBillID,
	BreakdownName=TT.BillNo,
	DebitOpeningAmount=TT.Debit_VBSessionAmount,
	CreditOpeningAmount=TT.Credit_VBSessionAmount,
	DebitTransactionAmount=TT.Debit_VBAmount,
	CreditTransactionAmount=TT.Credit_VBAmount
	--DebitAmount=TT.Debit_VBSessionAmount+TT.Debit_VBAmount,
	--CreditAmount=TT.Credit_VBSessionAmount+TT.Credit_VBAmount
FROM #Temp_Table AS TT WHERE TT.BreakdownType=2--BILL


UPDATE #Temp_Table
SET	BreakodwnID=TT.ProductID,
	BreakdownName=TT.ProductName,
	DebitOpeningAmount=TT.Debit_VPSessionAmount,
	CreditOpeningAmount=TT.Credit_VPSessionAmount,
	DebitTransactionAmount=TT.Debit_VPAmount,
	CreditTransactionAmount=TT.Credit_VPAmount
	--DebitAmount=TT.Debit_VPSessionAmount+TT.Debit_VPAmount,
	--CreditAmount=TT.Credit_VPSessionAmount+TT.Credit_VPAmount
FROM #Temp_Table AS TT WHERE TT.BreakdownType=4--ITEM


UPDATE #Temp_Table
SET BreakodwnID=TT.OrderID,
	BreakdownName=TT.OrderNo,
	DebitOpeningAmount=TT.Debit_VOSessionAmount,
	CreditOpeningAmount=TT.Credit_VOSessionAmount,
	DebitTransactionAmount=TT.Debit_VOAmount,
	CreditTransactionAmount=TT.Credit_VOAmount
	--DebitAmount=TT.Debit_VOSessionAmount+TT.Debit_VOAmount,
	--CreditAmount=TT.Credit_VOSessionAmount+TT.Credit_VOAmount
FROM #Temp_Table AS TT WHERE TT.BreakdownType=7--ORDER

UPDATE #Temp_Table
SET DebitAmount=TT.DebitTransactionAmount,
	CreditAmount=TT.CreditTransactionAmount
FROM #Temp_Table AS TT

UPDATE #Temp_Table SET IsDebit=1 FROM #Temp_Table AS TT 
UPDATE #Temp_Table SET IsDebit=0 FROM #Temp_Table AS TT WHERE TT.DebitOpeningAmount<TT.CreditOpeningAmount

UPDATE #Temp_Table SET OpeningAmount=TT.DebitOpeningAmount-TT.CreditOpeningAmount, ClosingAmount=(TT.DebitOpeningAmount-TT.CreditOpeningAmount+TT.DebitAmount-TT.CreditAmount), IsDrClosing=1 FROM #Temp_Table AS TT WHERE TT.ComponentID IN (2,6)
UPDATE #Temp_Table SET IsDrClosing=0 FROM #Temp_Table AS TT WHERE TT.ClosingAmount<0 AND TT.ComponentID IN (2,6)--DEBIT

UPDATE #Temp_Table SET OpeningAmount=TT.CreditOpeningAmount-TT.DebitOpeningAmount, ClosingAmount=(TT.CreditOpeningAmount-TT.DebitOpeningAmount-TT.DebitAmount+TT.CreditAmount), IsDrClosing=0 FROM #Temp_Table AS TT WHERE TT.ComponentID NOT IN (2,6)
UPDATE #Temp_Table SET IsDrClosing=1 FROM #Temp_Table AS TT WHERE TT.ClosingAmount<0	AND TT.ComponentID NOT IN (2,6)--CREDIT




UPDATE #Temp_Table
SET AccountHeadCode=(SELECT COA.AccountCode FROM ChartsOfAccount AS COA WHERE COA.AccountHeadID=TT.AccountHeadID),
	AccountHeadName=(SELECT COA.AccountHeadName FROM ChartsOfAccount AS COA WHERE COA.AccountHeadID=TT.AccountHeadID),
	CCCode=(SELECT ACC.Code FROM ACCostCenter AS ACC WHERE ACC.ACCostCenterID=TT.CCID),
	CCName=(SELECT ACC.Name FROM ACCostCenter AS ACC WHERE ACC.ACCostCenterID=TT.CCID)
FROM #Temp_Table AS TT

SELECT * FROM #Temp_Table
DROP TABLE #Temp_Table
COMMIT TRAN


GO
/****** Object:  StoredProcedure [dbo].[SP_ChangesInEquity]    Script Date: 12/1/2016 2:17:46 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[SP_ChangesInEquity]
(	
	@AccountingSessionID as int,
	@BusinessUnitID as int
)
AS
BEGIN TRAN
--DECLARE
--@AccountingSessionID as int,
--@BusinessUnitID as int

--SET @BusinessUnitID=1
--SET @AccountingSessionID=2

DECLARE
@StartDate as date,
@EndDate as date

SET @StartDate=(SELECT TT.StartDate FROM AccountingSession AS TT WHERE TT.AccountingSessionID=@AccountingSessionID)
SET @EndDate = (SELECT TT.EndDate FROM AccountingSession AS TT WHERE TT.AccountingSessionID=@AccountingSessionID)

CREATE TABLE #TempTable (
							ID int IDENTITY(1,1) PRIMARY KEY,
							TransactionType smallint,
							ShareCapital decimal(30,17),
							SharePremium decimal(30,17),
							ExcessOfIssuePrice decimal(30,17),
							CapitalReserve decimal(30,17),
							RevaluationSurplus decimal(30,17),
							FairValueGainOnInvestment decimal(30,17),
							RetainedEarnings decimal(30,17),
							TotalAmount decimal(30,17)
						)

--OpeningBalance = 1,
--OperationalProfit = 2,
--OtherIncome = 3,
--TransactionWithShareholder = 4,
--AdjustmentForDepreciation = 5,
--AdjustmentForDeferredTax = 6,
--ClosingBalance = 7
INSERT INTO #TempTable(TransactionType)VALUES(1)--OpeningBalance = 1
INSERT INTO #TempTable(TransactionType)VALUES(2)--OperationalProfit = 2
INSERT INTO #TempTable(TransactionType)VALUES(3)--OtherIncome = 3
INSERT INTO #TempTable(TransactionType)VALUES(4)--TransactionWithShareholder = 4
INSERT INTO #TempTable(TransactionType)VALUES(5)--AdjustmentForDepreciation = 5
INSERT INTO #TempTable(TransactionType)VALUES(6)--AdjustmentForDeferredTax = 6
INSERT INTO #TempTable(TransactionType)VALUES(7)--ClosingBalance = 7

--Share_Capital = 1,
--Share_Premium = 2,
--Excess_of_Issue_Price_Over_Face_Value_of_GDRs = 3,
--Capital_Reserve_on_Merger = 4,
--Revaluation_Surplus = 5,
--Fair_Value_Gain_on_Investment = 6,
--Retained_Earnings = 7

IF(@BusinessUnitID>0)
BEGIN
	UPDATE #TempTable
	SET ShareCapital=ISNULL((SELECT SUM(AO.OpenningBalance) FROM AccountOpenning AS AO WHERE AO.BusinessUnitID=@BusinessUnitID AND AO.AccountHeadID IN (SELECT CESD.EffectedAccountID FROM ChangesEquitySetupDetail AS CESD WHERE CESD.ChangesEquitySetupID IN (SELECT CES.ChangesEquitySetupID FROM ChangesEquitySetup AS CES WHERE CES.EquityCategory=1))),0),--Share_Capital = 1,
		SharePremium=ISNULL((SELECT SUM(AO.OpenningBalance) FROM AccountOpenning AS AO WHERE AO.BusinessUnitID=@BusinessUnitID AND AO.AccountHeadID IN (SELECT CESD.EffectedAccountID FROM ChangesEquitySetupDetail AS CESD WHERE CESD.ChangesEquitySetupID IN (SELECT CES.ChangesEquitySetupID FROM ChangesEquitySetup AS CES WHERE CES.EquityCategory=2))),0),--Share_Premium = 2,
		ExcessOfIssuePrice=ISNULL((SELECT SUM(AO.OpenningBalance) FROM AccountOpenning AS AO WHERE AO.BusinessUnitID=@BusinessUnitID AND AO.AccountHeadID IN (SELECT CESD.EffectedAccountID FROM ChangesEquitySetupDetail AS CESD WHERE CESD.ChangesEquitySetupID IN (SELECT CES.ChangesEquitySetupID FROM ChangesEquitySetup AS CES WHERE CES.EquityCategory=3))),0),--Excess_of_Issue_Price_Over_Face_Value_of_GDRs = 3
		CapitalReserve=ISNULL((SELECT SUM(AO.OpenningBalance) FROM AccountOpenning AS AO WHERE AO.BusinessUnitID=@BusinessUnitID AND AO.AccountHeadID IN (SELECT CESD.EffectedAccountID FROM ChangesEquitySetupDetail AS CESD WHERE CESD.ChangesEquitySetupID IN (SELECT CES.ChangesEquitySetupID FROM ChangesEquitySetup AS CES WHERE CES.EquityCategory=4))),0),--Capital_Reserve_on_Merger = 4
		RevaluationSurplus=ISNULL((SELECT SUM(AO.OpenningBalance) FROM AccountOpenning AS AO WHERE AO.BusinessUnitID=@BusinessUnitID AND AO.AccountHeadID IN (SELECT CESD.EffectedAccountID FROM ChangesEquitySetupDetail AS CESD WHERE CESD.ChangesEquitySetupID IN (SELECT CES.ChangesEquitySetupID FROM ChangesEquitySetup AS CES WHERE CES.EquityCategory=5))),0),--Revaluation_Surplus = 5
		FairValueGainOnInvestment=ISNULL((SELECT SUM(AO.OpenningBalance) FROM AccountOpenning AS AO WHERE AO.BusinessUnitID=@BusinessUnitID AND AO.AccountHeadID IN (SELECT CESD.EffectedAccountID FROM ChangesEquitySetupDetail AS CESD WHERE CESD.ChangesEquitySetupID IN (SELECT CES.ChangesEquitySetupID FROM ChangesEquitySetup AS CES WHERE CES.EquityCategory=6))),0),--Fair_Value_Gain_on_Investment = 6,
		RetainedEarnings=ISNULL((SELECT SUM(AO.OpenningBalance) FROM AccountOpenning AS AO WHERE AO.BusinessUnitID=@BusinessUnitID AND AO.AccountHeadID IN (SELECT CESD.EffectedAccountID FROM ChangesEquitySetupDetail AS CESD WHERE CESD.ChangesEquitySetupID IN (SELECT CES.ChangesEquitySetupID FROM ChangesEquitySetup AS CES WHERE CES.EquityCategory=7))),0)--Retained_Earnings = 7	
	FROM #TempTable AS TT WHERE TT.TransactionType=1
END
ELSE
BEGIN
	UPDATE #TempTable
	SET ShareCapital=ISNULL((SELECT SUM(AO.OpenningBalance) FROM AccountOpenning AS AO WHERE AO.AccountHeadID IN (SELECT CESD.EffectedAccountID FROM ChangesEquitySetupDetail AS CESD WHERE CESD.ChangesEquitySetupID IN (SELECT CES.ChangesEquitySetupID FROM ChangesEquitySetup AS CES WHERE CES.EquityCategory=1))),0),--Share_Capital = 1,
		SharePremium=ISNULL((SELECT SUM(AO.OpenningBalance) FROM AccountOpenning AS AO WHERE AO.AccountHeadID IN (SELECT CESD.EffectedAccountID FROM ChangesEquitySetupDetail AS CESD WHERE CESD.ChangesEquitySetupID IN (SELECT CES.ChangesEquitySetupID FROM ChangesEquitySetup AS CES WHERE CES.EquityCategory=2))),0),--Share_Premium = 2,
		ExcessOfIssuePrice=ISNULL((SELECT SUM(AO.OpenningBalance) FROM AccountOpenning AS AO WHERE AO.AccountHeadID IN (SELECT CESD.EffectedAccountID FROM ChangesEquitySetupDetail AS CESD WHERE CESD.ChangesEquitySetupID IN (SELECT CES.ChangesEquitySetupID FROM ChangesEquitySetup AS CES WHERE CES.EquityCategory=3))),0),--Excess_of_Issue_Price_Over_Face_Value_of_GDRs = 3
		CapitalReserve=ISNULL((SELECT SUM(AO.OpenningBalance) FROM AccountOpenning AS AO WHERE AO.AccountHeadID IN (SELECT CESD.EffectedAccountID FROM ChangesEquitySetupDetail AS CESD WHERE CESD.ChangesEquitySetupID IN (SELECT CES.ChangesEquitySetupID FROM ChangesEquitySetup AS CES WHERE CES.EquityCategory=4))),0),--Capital_Reserve_on_Merger = 4
		RevaluationSurplus=ISNULL((SELECT SUM(AO.OpenningBalance) FROM AccountOpenning AS AO WHERE AO.AccountHeadID IN (SELECT CESD.EffectedAccountID FROM ChangesEquitySetupDetail AS CESD WHERE CESD.ChangesEquitySetupID IN (SELECT CES.ChangesEquitySetupID FROM ChangesEquitySetup AS CES WHERE CES.EquityCategory=5))),0),--Revaluation_Surplus = 5
		FairValueGainOnInvestment=ISNULL((SELECT SUM(AO.OpenningBalance) FROM AccountOpenning AS AO WHERE AO.AccountHeadID IN (SELECT CESD.EffectedAccountID FROM ChangesEquitySetupDetail AS CESD WHERE CESD.ChangesEquitySetupID IN (SELECT CES.ChangesEquitySetupID FROM ChangesEquitySetup AS CES WHERE CES.EquityCategory=6))),0),--Fair_Value_Gain_on_Investment = 6,
		RetainedEarnings=ISNULL((SELECT SUM(AO.OpenningBalance) FROM AccountOpenning AS AO WHERE AO.AccountHeadID IN (SELECT CESD.EffectedAccountID FROM ChangesEquitySetupDetail AS CESD WHERE CESD.ChangesEquitySetupID IN (SELECT CES.ChangesEquitySetupID FROM ChangesEquitySetup AS CES WHERE CES.EquityCategory=7))),0)--Retained_Earnings = 7	
	FROM #TempTable AS TT WHERE TT.TransactionType=1
END


--Start Profit Loss Find Out
CREATE TABLE #PL_Table	(
								AccountHeadID int,							
								ParentAccountHeadID int,
								ComponentType smallint,
								AccountType smallint,							
								OpenningBalance decimal(30,17),
								DebitTransaction decimal(30,17),
								CreditTransaction decimal(30,17),							
								ClosingBalance decimal(30,17)
							)


INSERT INTO #PL_Table	(AccountHeadID,		ParentAccountHeadID,	AccountType)
				SELECT	 AccountHeadID,		ParentHeadID,			AccountType FROM dbo.GetAccountHeadByComponents('5,6') AS TT WHERE TT.AccountType=5 


IF(@BusinessUnitID>0)
BEGIN
	UPDATE #PL_Table
	SET	ComponentType=dbo.GetComponentID(TT.AccountHeadID),
		OpenningBalance=0,
		DebitTransaction=(SELECT ISNULL(SUM(VD.Amount),0) FROM View_VoucherDetail AS VD WHERE VD.BUID=@BusinessUnitID AND VD.OperationType=1 AND VD.IsDebit=1 AND VD.AccountHeadID=TT.AccountHeadID AND VD.VoucherID IN(SELECT V.VoucherID FROM Voucher AS V WHERE V.AuthorizedBy!=0 AND CONVERT(DATE,CONVERT(VARCHAR(12),VoucherDate,106)) BETWEEN CONVERT(DATE,CONVERT(VARCHAR(12),@StartDate,106)) AND CONVERT(DATE,CONVERT(VARCHAR(12),@EndDate,106)))),
		CreditTransaction=(SELECT ISNULL(SUM(VD.Amount),0) FROM View_VoucherDetail AS VD WHERE VD.BUID=@BusinessUnitID AND VD.OperationType=1 AND VD.IsDebit=0 AND VD.AccountHeadID=TT.AccountHeadID AND VD.VoucherID IN(SELECT V.VoucherID FROM Voucher AS V WHERE V.AuthorizedBy!=0 AND CONVERT(DATE,CONVERT(VARCHAR(12),VoucherDate,106)) BETWEEN CONVERT(DATE,CONVERT(VARCHAR(12),@StartDate,106)) AND CONVERT(DATE,CONVERT(VARCHAR(12),@EndDate,106)))),
		ClosingBalance=0
	FROM #PL_Table AS TT
END
ELSE
BEGIN
	UPDATE #PL_Table
	SET	ComponentType=dbo.GetComponentID(TT.AccountHeadID),
		OpenningBalance=0,
		DebitTransaction=(SELECT ISNULL(SUM(VD.Amount),0) FROM View_VoucherDetail AS VD WHERE VD.OperationType=1 AND VD.IsDebit=1 AND VD.AccountHeadID=TT.AccountHeadID AND VD.VoucherID IN(SELECT V.VoucherID FROM Voucher AS V WHERE V.AuthorizedBy!=0 AND CONVERT(DATE,CONVERT(VARCHAR(12),VoucherDate,106)) BETWEEN CONVERT(DATE,CONVERT(VARCHAR(12),@StartDate,106)) AND CONVERT(DATE,CONVERT(VARCHAR(12),@EndDate,106)))),
		CreditTransaction=(SELECT ISNULL(SUM(VD.Amount),0) FROM View_VoucherDetail AS VD WHERE VD.OperationType=1 AND VD.IsDebit=0 AND VD.AccountHeadID=TT.AccountHeadID AND VD.VoucherID IN(SELECT V.VoucherID FROM Voucher AS V WHERE V.AuthorizedBy!=0 AND CONVERT(DATE,CONVERT(VARCHAR(12),VoucherDate,106)) BETWEEN CONVERT(DATE,CONVERT(VARCHAR(12),@StartDate,106)) AND CONVERT(DATE,CONVERT(VARCHAR(12),@EndDate,106)))),
		ClosingBalance=0
	FROM #PL_Table AS TT
END

--Update Closing Balance
UPDATE #PL_Table
set	ClosingBalance=TT.OpenningBalance+TT.DebitTransaction-TT.CreditTransaction
FROM #PL_Table AS TT where ComponentType  IN(2,6)

UPDATE #PL_Table
set	ClosingBalance=TT.OpenningBalance-TT.DebitTransaction+TT.CreditTransaction
FROM #PL_Table AS TT where ComponentType NOT IN(2,6)



--Gross_Turnover = 1,//Income
--Value_Added_Tax = 2,//libilit
--Cost_Of_Goods_Sold = 3,//Expenditure
--Operating_Expenses = 4,//Expenditure
--Other_Income = 5,//income
--WPPF_Allocation = 6,//Expenditure
--Income_Tax = 7,//expenditure
--Profit_From_Associate_Undertaking = 8,//income
--Comprehensive_Income = 9,//incoem
--Financial_Cost = 10


UPDATE #TempTable
SET ShareCapital= 0, SharePremium= 0, ExcessOfIssuePrice= 0, CapitalReserve= 0, RevaluationSurplus= 0, FairValueGainOnInvestment= 0,
	RetainedEarnings= (ISNULL((SELECT SUM(PLTBL.ClosingBalance) FROM #PL_Table AS PLTBL WHERE PLTBL.ParentAccountHeadID IN (SELECT CIS.AccountHeadID FROM CIStatementSetup AS CIS WHERE CIS.CIHeadType IN(1,5,8))),0) - ISNULL((SELECT SUM(PLTBL.ClosingBalance) FROM #PL_Table AS PLTBL WHERE PLTBL.ParentAccountHeadID IN (SELECT CIS.AccountHeadID FROM CIStatementSetup AS CIS WHERE CIS.CIHeadType IN(2,3,4,6,7,10))),0))
FROM #TempTable AS TT  WHERE TT.TransactionType=2--OperationalProfit = 2

UPDATE #TempTable
SET ShareCapital= 0, SharePremium= 0, ExcessOfIssuePrice= 0, CapitalReserve= 0, RevaluationSurplus= 0, RetainedEarnings= 0,
	FairValueGainOnInvestment = ISNULL((SELECT SUM(PLTBL.ClosingBalance) FROM #PL_Table AS PLTBL WHERE PLTBL.ParentAccountHeadID IN (SELECT CIS.AccountHeadID FROM CIStatementSetup AS CIS WHERE CIS.CIHeadType IN(9))),0)
FROM #TempTable AS TT  WHERE TT.TransactionType=3--OtherIncome = 3
--End Profit Loss 


UPDATE #TempTable
SET TotalAmount= ISNULL(TT.ShareCapital,0)+ISNULL(TT.SharePremium,0)+ISNULL(TT.ExcessOfIssuePrice,0)+ISNULL(TT.CapitalReserve,0)+ISNULL(TT.RevaluationSurplus,0)+ISNULL(TT.FairValueGainOnInvestment,0)+ISNULL(TT.RetainedEarnings,0)
FROM #TempTable AS TT 

UPDATE #TempTable
SET ShareCapital= ISNULL((SELECT SUM(HH.ShareCapital) FROM #TempTable AS HH WHERE HH.TransactionType!=7),0),
	SharePremium= ISNULL((SELECT SUM(HH.SharePremium) FROM #TempTable AS HH WHERE HH.TransactionType!=7),0),
	ExcessOfIssuePrice= ISNULL((SELECT SUM(HH.ExcessOfIssuePrice) FROM #TempTable AS HH WHERE HH.TransactionType!=7),0),
	CapitalReserve= ISNULL((SELECT SUM(HH.CapitalReserve) FROM #TempTable AS HH WHERE HH.TransactionType!=7),0),
	RevaluationSurplus= ISNULL((SELECT SUM(HH.RevaluationSurplus) FROM #TempTable AS HH WHERE HH.TransactionType!=7),0),
	FairValueGainOnInvestment= ISNULL((SELECT SUM(HH.FairValueGainOnInvestment) FROM #TempTable AS HH WHERE HH.TransactionType!=7),0),
	RetainedEarnings= ISNULL((SELECT SUM(HH.RetainedEarnings) FROM #TempTable AS HH WHERE HH.TransactionType!=7),0),
	TotalAmount= ISNULL((SELECT SUM(HH.TotalAmount) FROM #TempTable AS HH WHERE HH.TransactionType!=7),0)
FROM #TempTable AS TT  WHERE TT.TransactionType=7

SELECT * FROM #TempTable ORDER BY ID
DROP TABLE #TempTable
DROP TABLE #PL_Table
COMMIT TRAN


GO
/****** Object:  StoredProcedure [dbo].[SP_CloseAccountingYear]    Script Date: 12/1/2016 2:17:46 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[SP_CloseAccountingYear]
(
	@ClosingSessionID as int,
	@DBUserID as int
)
AS
BEGIN TRAN
--DECLARE
--@ClosingSessionID as int,
--@DBUserID as int
--SET @ClosingSessionID=445
--SET @DBUserID=-9

DECLARE
@PV_TempMessage as varchar(512)

IF(@ClosingSessionID<=0)	
BEGIN	
	ROLLBACK
		RAISERROR (N'Invalid Accounting session!!~',16,1);	
	RETURN
END

IF((SELECT YearStatus FROM AccountingSession WHERE AccountingSessionID=@ClosingSessionID)=3)	
BEGIN	
	ROLLBACK
		RAISERROR (N'Your Selected Session Already Closed!!~',16,1);	
	RETURN
END

IF((SELECT YearStatus FROM AccountingSession WHERE AccountingSessionID=@ClosingSessionID)!=2)	
BEGIN	
	ROLLBACK
		RAISERROR (N'Please Selected an Freez Accounting Session!!~',16,1);	
	RETURN
END

DECLARE 
@PreAccountingYear as int 
SET @PreAccountingYear=ISNULL((SELECT TOP(1) AccountingSessionID  FROM AccountingSession WHERE ParentSessionID=1 AND YearStatus !=3 AND AccountingSessionID<@ClosingSessionID),0)
IF (@PreAccountingYear >0 )	
BEGIN
	SET @PV_TempMessage=(SELECT SessionName FROM AccountingSession WHERE AccountingSessionID=@PreAccountingYear)
	ROLLBACK
		RAISERROR (N'Please close %s accounting session!!~',16,1, @PV_TempMessage);	
	RETURN
END

UPDATE AccountingSession SET YearStatus=3, DBUserID=@DBUserID WHERE AccountingSessionID=@ClosingSessionID OR SessionID=@ClosingSessionID
SELECT * FROM View_AccountingSession WHERE AccountingSessionID=@ClosingSessionID
COMMIT TRAN



GO
/****** Object:  StoredProcedure [dbo].[SP_CommitNewAccountingYear]    Script Date: 12/1/2016 2:17:46 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

GO
/****** Object:  StoredProcedure [dbo].[SP_CommitProfitLossAccount]    Script Date: 12/1/2016 2:17:46 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
/****** Object:  StoredProcedure [dbo].[SP_CounterVoucher]    Script Date: 12/1/2016 2:17:46 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

GO
/****** Object:  StoredProcedure [dbo].[SP_CounterVoucherDelete]    Script Date: 12/1/2016 2:17:46 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[SP_CounterVoucherDelete]
(	
	@VoucherID as int,
	@CounterVoucherID as int
)
AS 
BEGIN TRAN
	DELETE FROM VoucherHistory WHERE VoucherID=@CounterVoucherID
	DELETE FROM VoucherCheque WHERE VoucherDetailID IN (SELECT HH.VoucherDetailID FROM VoucherDetail AS HH WHERE HH.VoucherID =@CounterVoucherID)
	DELETE FROM VoucherReference WHERE VoucherDetailID IN (SELECT HH.VoucherDetailID FROM VoucherDetail AS HH WHERE HH.VoucherID =@CounterVoucherID)
	DELETE FROM VOReference WHERE VoucherDetailID IN (SELECT HH.VoucherDetailID FROM VoucherDetail AS HH WHERE HH.VoucherID =@CounterVoucherID)
	DELETE FROM VPTransaction WHERE VoucherDetailID IN (SELECT HH.VoucherDetailID FROM VoucherDetail AS HH WHERE HH.VoucherID =@CounterVoucherID)
	DELETE FROM VoucherBillTransaction WHERE VoucherDetailID IN (SELECT HH.VoucherDetailID FROM VoucherDetail AS HH WHERE HH.VoucherID =@CounterVoucherID)
	DELETE FROM CostCenterTransaction WHERE VoucherDetailID IN (SELECT HH.VoucherDetailID FROM VoucherDetail AS HH WHERE HH.VoucherID =@CounterVoucherID)
	DELETE FROM VoucherDetail WHERE VoucherID =@CounterVoucherID
	DELETE FROM Voucher WHERE VoucherID=@CounterVoucherID
	UPDATE Voucher SET CounterVoucherID=0 WHERE VoucherID=@VoucherID
COMMIT TRAN

GO
/****** Object:  StoredProcedure [dbo].[SP_GetAHOpeningBalance]    Script Date: 12/1/2016 2:17:46 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[SP_GetAHOpeningBalance]
(
@AccountHeadID as int,
@StartDate as date,
@CurrencyID as int,
@IsApproved as bit
--%n, %d, %n, %b
)
AS

BEGIN TRAN
--DECLARE
--@AccountHeadID as int,
--@StartDate as date,
--@CurrencyID as int,
--@IsApproved as bit

--SET @AccountHeadID=234
--SET @StartDate='1 JUL 2015'
--SET @CurrencyID=1
--SET @IsApproved=1


CREATE TABLE #Temp_Table(	
							ID int IDENTITY(1,1) PRIMARY KEY,
							CurrentBalance decimal(30,17),
							VoucherDate datetime,							
							OpenningBalance decimal(30,17),
							IsDebit bit
						)


DECLARE 
@IsBaseCurrency as bit,
@OpenningBalance as decimal(30,17),
@ComponentType as smallint,
@DebitOpenningBalance as decimal(30,17),
@CreditOpenningBalance as decimal(30,17),
@DebitTransactionAmount as decimal(30,17),
@CreditTransactionAmount as decimal(30,17),
@SessionStartDate as date,
@SessionID as int

SET @IsBaseCurrency=1
IF((SELECT C.BaseCurrencyID FROM Company AS C WHERE C.CompanyID=1)!=@CurrencyID)
BEGIN
	SET @IsBaseCurrency=0
END

IF EXISTS(SELECT SessionID FROM AccountingSession WHERE CONVERT(DATE,CONVERT(VARCHAR(12),StartDate,106))=CONVERT(DATE,CONVERT(VARCHAR(12),@StartDate,106)) AND SessionType=6)
BEGIN
	SET @SessionID=(SELECT Top(1)SessionID FROM AccountingSession  WHERE  CONVERT(DATE,CONVERT(VARCHAR(12),StartDate,106))=CONVERT(DATE,CONVERT(VARCHAR(12),@StartDate,106)) AND SessionType=6)
END
ELSE
BEGIN
	SET @SessionID=(SELECT TOP 1 SessionID FROM AccountingSession  WHERE  CONVERT(DATE,CONVERT(VARCHAR(12),StartDate,106))>=CONVERT(DATE,CONVERT(VARCHAR(12),@StartDate,106)) AND SessionType=6)
	SET @StartDate=(SELECT TOP 1 StartDate FROM AccountingSession  WHERE  CONVERT(DATE,CONVERT(VARCHAR(12),StartDate,106))>=CONVERT(DATE,CONVERT(VARCHAR(12),@StartDate,106)) AND SessionType=6)
END

SET @SessionStartDate=(SELECT StartDate FROM AccountingSession WHERE  AccountingSessionID=@SessionID)

SET @ComponentType=  dbo.GetComponentID(@AccountHeadID) --Set CompanyID
IF(@IsApproved=1)
BEGIN
	IF(@IsBaseCurrency=1)
	BEGIN
		SET @DebitOpenningBalance=ISNULL((SELECT ISNULL(SUM(OpenningBalance),0) FROM AccountOpenning AS AO WHERE AO.IsDebit=1 AND  AO.AccountingSessionID=@SessionID AND AO.AccountHeadID=@AccountHeadID),0)
		SET @CreditOpenningBalance=ISNULL((SELECT ISNULL(SUM(OpenningBalance),0) FROM AccountOpenning AS AO WHERE AO.IsDebit=0 AND  AO.AccountingSessionID=@SessionID AND AO.AccountHeadID=@AccountHeadID),0)
		SET @DebitTransactionAmount=(SELECT ISNULL(SUM(VD.Amount),0) FROM VoucherDetail AS VD WHERE  VD.IsDebit=1 AND VD.AccountHeadID=@AccountHeadID AND VD.VoucherID IN (SELECT VoucherID FROM Voucher AS V WHERE V.AuthorizedBy!=0 AND  CONVERT(DATE,CONVERT(VARCHAR(12),V.VoucherDate,106)) BETWEEN CONVERT(DATE,CONVERT(VARCHAR(12),@SessionStartDate,106)) AND CONVERT(DATE,CONVERT(VARCHAR(12),DATEADD(DAY,-1,@StartDate),106))))
		SET @CreditTransactionAmount=(SELECT ISNULL(SUM(VD.Amount),0) FROM VoucherDetail AS VD WHERE  VD.IsDebit=0 AND VD.AccountHeadID=@AccountHeadID AND VD.VoucherID IN(SELECT VoucherID FROM Voucher AS V WHERE V.AuthorizedBy!=0 AND  CONVERT(DATE,CONVERT(VARCHAR(12),V.VoucherDate,106)) BETWEEN CONVERT(DATE,CONVERT(VARCHAR(12),@SessionStartDate,106)) AND CONVERT(DATE,CONVERT(VARCHAR(12),DATEADD(DAY,-1,@StartDate),106))))
	END
	ELSE
	BEGIN
		SET @DebitOpenningBalance=ISNULL((SELECT ISNULL(SUM(AmountInCurrency),0) FROM AccountOpenning AS AO WHERE AO.IsDebit=1 AND AO.AccountingSessionID=@SessionID AND AO.AccountHeadID=@AccountHeadID AND AO.CurrencyID=@CurrencyID),0)
		SET @CreditOpenningBalance=ISNULL((SELECT ISNULL(SUM(AmountInCurrency),0) FROM AccountOpenning AS AO WHERE AO.IsDebit=0 AND AO.AccountingSessionID=@SessionID AND AO.AccountHeadID=@AccountHeadID AND AO.CurrencyID=@CurrencyID),0)
		SET @DebitTransactionAmount=(SELECT ISNULL(SUM(VD.AmountInCurrency),0) FROM VoucherDetail AS VD WHERE  VD.CurrencyID=@CurrencyID AND VD.IsDebit=1 AND VD.AccountHeadID=@AccountHeadID AND VD.VoucherID IN (SELECT VoucherID FROM Voucher AS V WHERE V.AuthorizedBy!=0 AND  CONVERT(DATE,CONVERT(VARCHAR(12),V.VoucherDate,106)) BETWEEN CONVERT(DATE,CONVERT(VARCHAR(12),@SessionStartDate,106)) AND CONVERT(DATE,CONVERT(VARCHAR(12),DATEADD(DAY,-1,@StartDate),106))))
		SET @CreditTransactionAmount=(SELECT ISNULL(SUM(VD.AmountInCurrency),0) FROM VoucherDetail AS VD WHERE  VD.CurrencyID=@CurrencyID AND VD.IsDebit=0 AND VD.AccountHeadID=@AccountHeadID AND VD.VoucherID IN(SELECT VoucherID FROM Voucher AS V WHERE V.AuthorizedBy!=0 AND  CONVERT(DATE,CONVERT(VARCHAR(12),V.VoucherDate,106)) BETWEEN CONVERT(DATE,CONVERT(VARCHAR(12),@SessionStartDate,106)) AND CONVERT(DATE,CONVERT(VARCHAR(12),DATEADD(DAY,-1,@StartDate),106))))
	END
END
ELSE
BEGIN
	IF(@IsBaseCurrency=1)
	BEGIN
		SET @DebitOpenningBalance=ISNULL((SELECT ISNULL(Sum(OpenningBalance),0) FROM AccountOpenning AS AO WHERE AO.IsDebit=1 AND  AO.AccountingSessionID=@SessionID AND AO.AccountHeadID=@AccountHeadID),0)
		SET @CreditOpenningBalance=ISNULL((SELECT ISNULL(Sum(OpenningBalance),0) FROM AccountOpenning AS AO WHERE AO.IsDebit=0 AND  AO.AccountingSessionID=@SessionID AND AO.AccountHeadID=@AccountHeadID),0)
		SET @DebitTransactionAmount=(SELECT ISNULL(SUM(VD.Amount),0) FROM VoucherDetail AS VD WHERE  VD.IsDebit=1 AND VD.AccountHeadID=@AccountHeadID AND VD.VoucherID IN (SELECT VoucherID FROM Voucher AS V WHERE CONVERT(DATE,CONVERT(VARCHAR(12),V.VoucherDate,106)) BETWEEN CONVERT(DATE,CONVERT(VARCHAR(12),@SessionStartDate,106)) AND CONVERT(DATE,CONVERT(VARCHAR(12),DATEADD(DAY,-1,@StartDate),106))))
		SET @CreditTransactionAmount=(SELECT ISNULL(SUM(VD.Amount),0) FROM VoucherDetail AS VD WHERE  VD.IsDebit=0 AND VD.AccountHeadID=@AccountHeadID AND VD.VoucherID IN(SELECT VoucherID FROM Voucher AS V WHERE CONVERT(DATE,CONVERT(VARCHAR(12),V.VoucherDate,106)) BETWEEN CONVERT(DATE,CONVERT(VARCHAR(12),@SessionStartDate,106)) AND CONVERT(DATE,CONVERT(VARCHAR(12),DATEADD(DAY,-1,@StartDate),106))))
	END
	ELSE
	BEGIN
		SET @DebitOpenningBalance=ISNULL((SELECT ISNULL(SUM(AmountInCurrency),0) FROM AccountOpenning AS AO WHERE AO.IsDebit=1 AND AO.AccountingSessionID=@SessionID AND AO.AccountHeadID=@AccountHeadID AND AO.CurrencyID=@CurrencyID),0)
		SET @CreditOpenningBalance=ISNULL((SELECT ISNULL(SUM(AmountInCurrency),0) FROM AccountOpenning AS AO WHERE AO.IsDebit=0 AND AO.AccountingSessionID=@SessionID AND AO.AccountHeadID=@AccountHeadID AND AO.CurrencyID=@CurrencyID),0)
		SET @DebitTransactionAmount=(SELECT ISNULL(SUM(VD.AmountInCurrency),0) FROM VoucherDetail AS VD WHERE  VD.CurrencyID=@CurrencyID AND VD.IsDebit=1 AND VD.AccountHeadID=@AccountHeadID AND VD.VoucherID IN (SELECT VoucherID FROM Voucher AS V WHERE CONVERT(DATE,CONVERT(VARCHAR(12),V.VoucherDate,106)) BETWEEN CONVERT(DATE,CONVERT(VARCHAR(12),@SessionStartDate,106)) AND CONVERT(DATE,CONVERT(VARCHAR(12),DATEADD(DAY,-1,@StartDate),106))))
		SET @CreditTransactionAmount=(SELECT ISNULL(SUM(VD.AmountInCurrency),0) FROM VoucherDetail AS VD WHERE  VD.CurrencyID=@CurrencyID AND VD.IsDebit=0 AND VD.AccountHeadID=@AccountHeadID AND VD.VoucherID IN(SELECT VoucherID FROM Voucher AS V WHERE CONVERT(DATE,CONVERT(VARCHAR(12),V.VoucherDate,106)) BETWEEN CONVERT(DATE,CONVERT(VARCHAR(12),@SessionStartDate,106)) AND CONVERT(DATE,CONVERT(VARCHAR(12),DATEADD(DAY,-1,@StartDate),106))))
	END
END

IF(@ComponentType IN(2,6))--ComponentID{Asset = 2,Laibility = 3,OwnerEquity=4,Income = 5,Expeness = 6}
BEGIN
	SET @OpenningBalance =@DebitOpenningBalance-@CreditOpenningBalance+@DebitTransactionAmount-@CreditTransactionAmount
	if(@OpenningBalance>0)
	BEGIN	
		INSERT INTO #Temp_Table	(VoucherDate,	OpenningBalance,	CurrentBalance,IsDebit)	
						VALUES	(@StartDate,	@OpenningBalance,	@OpenningBalance,1)
	END
	ELSE
	BEGIN	
		INSERT INTO #Temp_Table	(VoucherDate,	OpenningBalance,		CurrentBalance,IsDebit)	
						VALUES	(@StartDate,	(-1)*@OpenningBalance,	@OpenningBalance,1)
	END
END
ELSE
BEGIN
	SET @OpenningBalance =@CreditOpenningBalance-@DebitOpenningBalance-@DebitTransactionAmount+@CreditTransactionAmount	
	IF(@OpenningBalance>0)
	BEGIN		
		INSERT INTO #Temp_Table	(VoucherDate,	OpenningBalance,	CurrentBalance,IsDebit)	
						VALUES	(@StartDate,	@OpenningBalance,	@OpenningBalance,0)	
	END
	ELSE
	BEGIN	
		INSERT INTO #Temp_Table	(VoucherDate,	OpenningBalance,		CurrentBalance,IsDebit)	
						VALUES	(@StartDate,	(-1)*@OpenningBalance,	@OpenningBalance,0)  
	END
END	


SELECT * FROM #Temp_Table
DROP TABLE #Temp_Table
COMMIT TRAN


GO
/****** Object:  StoredProcedure [dbo].[SP_GetBankAccountBalance]    Script Date: 12/1/2016 2:17:46 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO



CREATE  PROCEDURE [dbo].[SP_GetBankAccountBalance]
(
	@Param_AccountHeadIDs varchar(500),
	@StartDate as date,
	@EndDate as date

)
AS
BEGIN TRAN
--DECLARE
--@Param_AccountHeadIDs varchar(500),
--@Param_AccountTypeInInt as int,
--@StartDate as date,
--@EndDate as date



SET @StartDate=CONVERT(DATE,GetDate()) 
SET @EndDate=CONVERT(DATE,GetDate()) 
SET @Param_AccountHeadIDs='1,2,3,4,5,6'

--Select * from ChartsOfAccount where AccountType=5

CREATE TABLE #TempTable (
							AccountHeadID int,
							AccountCode Varchar(512),
							AccountHeadName Varchar(512),						
							ComponentType int,
							AccountType smallint,		
							ParentHeadID int,
							OpenningBalance decimal(30,17),
							DebitAmount decimal(30,17),
							CreditAmount decimal(30,17),
							ClosingBalance decimal(30,17),
							AccountIssue decimal(30,17),
							Currency Varchar(512),
							BankAccountID int,
							BankName varchar(500),
							AccountNo varchar(500)

						)

CREATE TABLE #TempTable2(
							AccountHeadID int,	
							ComponentType int,						
							DebitOpeiningValue decimal(30,17),		
							CreditOpeiningValue decimal(30,17),												
							DebitAmount decimal(30,17),
							CreditAmount decimal(30,17),
							ClosingValue decimal(30,17)							
						)

INSERT INTO #TempTable	   (AccountHeadID,	AccountCode,	AccountHeadName,	ParentHeadID,	AccountType,BankAccountID)
				SELECT		AccountHeadID,	AccountCode,	AccountHeadName,	ParentHeadID,	AccountType,ReferenceObjectID FROM ChartsOfAccount where ReferenceType=4 --and ReferenceObjectID in (Select * from [dbo].[SplitInToDataSet](@Param_AccountHeadIDs,','))
				--SELECT		AccountHeadID,	AccountCode,	AccountHeadName,	ParentHeadID,	AccountType FROM ChartsOfAccount where AccountHeadID in (Select * from [dbo].[SplitInToDataSet](@Param_AccountHeadIDs,','))

--EnumAccountType{None = 0,Component = 1,Segment =2,Group = 3,SubGroup = 4,Ledger = 5}
INSERT INTO #TempTable2 (AccountHeadID)
			   SELECT TT.AccountHeadID FROM #TempTable AS TT WHERE TT.AccountType=5

DECLARE
@SessionID as int,
@SessionStartDate as date

IF EXISTS(SELECT SessionID FROM AccountingSession  WHERE CONVERT(DATE,CONVERT(VARCHAR(12),StartDate,106))=CONVERT(DATE,CONVERT(VARCHAR(12),@StartDate,106)) AND SessionType=6)
BEGIN
	SET @SessionID=(SELECT top(1)SessionID FROM AccountingSession  WHERE CONVERT(DATE,CONVERT(VARCHAR(12),StartDate,106))=CONVERT(DATE,CONVERT(VARCHAR(12),@StartDate,106)) AND SessionType=6)
END
ELSE
BEGIN
	SET @SessionID=(SELECT TOP 1 SessionID FROM AccountingSession  WHERE CONVERT(DATE,CONVERT(VARCHAR(12),StartDate,106))>=CONVERT(DATE,CONVERT(VARCHAR(12),@StartDate,106)) AND SessionType=6)
	SET @StartDate=(SELECT TOP 1 StartDate FROM AccountingSession  WHERE  CONVERT(DATE,CONVERT(VARCHAR(12),StartDate,106))>=CONVERT(DATE,CONVERT(VARCHAR(12),@StartDate,106)) AND SessionType=6)
END
SET @SessionStartDate=(SELECT StartDate FROM AccountingSession WHERE  AccountingSessionID=@SessionID)



UPDATE #TempTable2
SET	ComponentType=dbo.GetComponentID(TT.AccountHeadID),
	DebitOpeiningValue=ISNULL((SELECT SUM(AO.OpenningBalance) FROM AccountOpenning AS AO WHERE AO.IsDebit=1 AND AO.AccountHeadID=TT.AccountHeadID AND AO.AccountingSessionID=@SessionID),0),
	CreditOpeiningValue=ISNULL((SELECT SUM(AO.OpenningBalance) FROM AccountOpenning AS AO WHERE AO.IsDebit=0 AND AO.AccountHeadID=TT.AccountHeadID AND AO.AccountingSessionID=@SessionID),0),
	DebitAmount=ISNULL((SELECT SUM(VD.Amount) FROM View_VoucherDetail AS VD WHERE VD.IsDebit=1 AND VD.AccountHeadID=TT.AccountHeadID AND VD.VoucherID IN(SELECT V.VoucherID FROM Voucher AS V WHERE ISNULL(V.AuthorizedBy,0)!=0 AND CONVERT(DATE,CONVERT(VARCHAR(12),V.VoucherDate,106)) BETWEEN CONVERT(DATE,CONVERT(VARCHAR(12),@SessionStartDate,106)) AND CONVERT(DATE,CONVERT(VARCHAR(12), DATEADD(DAY,-1,@StartDate),106)))),0),
	CreditAmount=ISNULL((SELECT SUM(VD.Amount) FROM View_VoucherDetail AS VD WHERE VD.IsDebit=0 AND VD.AccountHeadID=TT.AccountHeadID AND VD.VoucherID IN(SELECT V.VoucherID FROM Voucher AS V WHERE ISNULL(V.AuthorizedBy,0)!=0 AND CONVERT(DATE,CONVERT(VARCHAR(12),V.VoucherDate,106)) BETWEEN CONVERT(DATE,CONVERT(VARCHAR(12),@SessionStartDate,106)) AND CONVERT(DATE,CONVERT(VARCHAR(12), DATEADD(DAY,-1,@StartDate),106)))),0)
FROM #TempTable2 AS TT 

UPDATE #TempTable2 SET ClosingValue=TT.DebitOpeiningValue-TT.CreditOpeiningValue+TT.DebitAmount-TT.CreditAmount FROM #TempTable2 AS TT WHERE TT.ComponentType IN (2,6)
UPDATE #TempTable2 SET ClosingValue=TT.CreditOpeiningValue-TT.DebitOpeiningValue+TT.CreditAmount-TT.DebitAmount FROM #TempTable2 AS TT WHERE TT.ComponentType NOT IN (2,6)

UPDATE #TempTable
SET ComponentType=dbo.GetComponentID(TT.AccountHeadID),
	AccountCode=(SELECT COA.AccountCode FROM ChartsOfAccount AS COA WHERE COA.AccountHeadID=TT.AccountHeadID), -- AND COA.AccountHeadID IN (SELECT CWAH.AccountHeadID FROM CompanyWiseAccountHead AS CWAH WHERE CWAH.CompanyID = @CompanyID)),
	AccountHeadName=(SELECT COA.AccountHeadName FROM ChartsOfAccount AS COA WHERE COA.AccountHeadID=TT.AccountHeadID) -- AND COA.AccountHeadID IN (SELECT CWAH.AccountHeadID FROM CompanyWiseAccountHead AS CWAH WHERE CWAH.CompanyID = @CompanyID))
FROM #TempTable AS TT WHERE AccountHeadID !=1
------------------------------

UPDATE #TempTable
SET OpenningBalance=ISNULL((SELECT BB.ClosingValue FROM #TempTable2 AS BB WHERE BB.AccountHeadID=TT.AccountHeadID),0),
	DebitAmount=ISNULL((SELECT SUM(VD.Amount) FROM View_VoucherDetail AS VD WHERE VD.IsDebit=1  AND VD.AccountHeadID=TT.AccountHeadID AND VD.VoucherID IN(SELECT V.VoucherID FROM Voucher AS V WHERE ISNULL(V.AuthorizedBy,0)!=0 AND CONVERT(DATE,CONVERT(VARCHAR(12),V.VoucherDate,106)) BETWEEN CONVERT(DATE,CONVERT(VARCHAR(12),@StartDate,106)) AND CONVERT(DATE,CONVERT(VARCHAR(12), @EndDate,106)))),0),
	CreditAmount=ISNULL((SELECT SUM(VD.Amount) FROM View_VoucherDetail AS VD WHERE VD.IsDebit=0 AND VD.AccountHeadID=TT.AccountHeadID AND VD.VoucherID IN(SELECT V.VoucherID FROM Voucher AS V WHERE ISNULL(V.AuthorizedBy,0)!=0 AND CONVERT(DATE,CONVERT(VARCHAR(12),V.VoucherDate,106)) BETWEEN CONVERT(DATE,CONVERT(VARCHAR(12),@StartDate,106)) AND CONVERT(DATE,CONVERT(VARCHAR(12), @EndDate,106)))),0)
FROM #TempTable AS TT WHERE TT.AccountType=5

UPDATE #TempTable SET ClosingBalance=TT.OpenningBalance+TT.DebitAmount-TT.CreditAmount FROM #TempTable AS TT WHERE TT.ComponentType IN (2,6)
UPDATE #TempTable SET ClosingBalance=TT.OpenningBalance-TT.DebitAmount+TT.CreditAmount FROM #TempTable AS TT WHERE TT.ComponentType NOT IN (2,6)


UPDATE #TempTable SET AccountIssue=(Select SUM(ApproveAmount) from PaymentRequisitionDetail)
UPDATE #TempTable SET ClosingBalance=TT.OpenningBalance-TT.DebitAmount+TT.CreditAmount FROM #TempTable AS TT WHERE TT.ComponentType NOT IN (2,6)

--Update Ledger
--EnumAccountType{None = 0,Component = 1,Segment =2,Group = 3,SubGroup = 4,Ledger = 5}


Update #TempTable set BankName=(Select BA.BankNameBranch from View_BankAccount as BA where BA.BankAccountID=TT.BankAccountID)
,AccountNo=(Select BA.AccountNo from View_BankAccount as BA where BA.BankAccountID=TT.BankAccountID)
from #TempTable as TT

Update #TempTable set Currency=(Select BA.Symbol from Currency as BA where BA.CurrencyID=(Select Top(1)BaseCurrencyID from Company))

from #TempTable as TT


delete from #TempTable

SELECT * FROM #TempTable AS TT   
DROP TABLE #TempTable2
DROP TABLE #TempTable
COMMIT TRAN






GO
/****** Object:  StoredProcedure [dbo].[sp_GetCurrentAccountBalance]    Script Date: 12/1/2016 2:17:46 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[sp_GetCurrentAccountBalance]
	/*
	(
	@parameter1 int = 5,
	@parameter2 datatype OUTPUT
	)
	*/
AS
begin tran
begin
SET NOCOUNT ON;
	DECLARE
	@Bank_ID as int,
	@BankID as varchar(100),
	@sSQL as Varchar(4096)

	CREATE TABLE #Temp_Table2 (Account_Balance_ID INT 
								,Bank_ID INT
								,ERQAC decimal(30,17) 
								,FCAC decimal(30,17)
								,LTR decimal(30,17)
								,PAD decimal(30,17)
								,Saleable_Amount decimal(30,17)
								,Date DATETIME)

	DECLARE Cur_AB CURSOR LOCAL FORWARD_ONLY KEYSET FOR
	SELECT Distinct(Bank_ID) FROM Account_Balance order by Bank_ID 

	OPEN Cur_AB
	FETCH NEXT FROM Cur_AB INTO  @Bank_ID
	WHILE(@@Fetch_Status <> -1)
	BEGIN 
		SET @BankID=Convert(varchar(100),@Bank_ID)
		SET @sSQL= 'SELECT * FROM Account_Balance WHERE Bank_ID='+ @BankID +' AND	Date=(SELECT MAX(Date) FROM Account_Balance WHERE Bank_ID='+ @BankID +')'
		
		INSERT INTO #Temp_Table2 EXEC(@sSQL)
	FETCH NEXT FROM Cur_AB INTO  @Bank_ID	
	END
	CLOSE Cur_AB
	DEALLOCATE Cur_AB

	SELECT * FROM #Temp_Table2
	DROP TABLE #Temp_Table2
END




commit tran



GO
/****** Object:  StoredProcedure [dbo].[SP_IncomeStatement]    Script Date: 12/1/2016 2:17:46 PM ******/
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
--SET @StartDate='01 Jan 2015'
--SET @EndDate ='31 Dec 2016'
--SET @ParenHeadID=0
--SET @AccountType = 5


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
SELECT * FROM #Temp_Table WHERE AccountType <= @AccountType ORDER BY AccountHeadID


DROP TABLE #Temp_Table
COMMIT TRAN



GO
/****** Object:  StoredProcedure [dbo].[SP_TrailBalance]    Script Date: 12/1/2016 2:17:46 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[SP_TrailBalance]
(
	@Param_AccountHeadID as int,
	@Param_AccountTypeInInt as int,
	@StartDate as date,
	@EndDate as date,
	@BusinessUnitID as int
)
AS
BEGIN TRAN
--DECLARE
--@Param_AccountHeadID int,
--@Param_AccountTypeInInt as int,
--@StartDate as date,
--@EndDate as date

--SET @Param_AccountTypeInInt=5
--SET @StartDate=CONVERT(DATE,'16 SEP 2015') 
--SET @EndDate=CONVERT(DATE,'30 Sep 2015') 
--SET @Param_AccountHeadID=0

DECLARE
 @AccountHeadID as int 
IF(@Param_AccountHeadID <=0 or @Param_AccountHeadID is null)
BEGIN
	SET @AccountHeadID =1
END
Else
BEGIN
	SET @AccountHeadID = @Param_AccountHeadID
END

CREATE TABLE #TempTable (
							AccountHeadID int,
							AccountCode Varchar(512),
							AccountHeadName Varchar(512),						
							ComponentType int,
							AccountType smallint,		
							ParentHeadID int,
							OpenningBalance decimal(30,17),
							DebitAmount decimal(30,17),
							CreditAmount decimal(30,17),
							ClosingBalance decimal(30,17)
						)

CREATE TABLE #TempTable2(
							AccountHeadID int,	
							ComponentType int,						
							DebitOpeiningValue decimal(30,17),		
							CreditOpeiningValue decimal(30,17),												
							DebitAmount decimal(30,17),
							CreditAmount decimal(30,17),
							ClosingValue decimal(30,17)							
						)

INSERT INTO #TempTable	   (AccountHeadID,	AccountCode,	AccountHeadName,	ParentHeadID,	AccountType)
				SELECT		AccountHeadID,	AccountCode,	AccountHeadName,	ParentHeadID,	AccountType FROM dbo.GetAccountHead(@AccountHeadID)

INSERT INTO #TempTable	   (AccountHeadID,	AccountCode,	AccountHeadName,	ParentHeadID,	AccountType)
				SELECT		AccountHeadID,	AccountCode,	'Grand Total',		ParentHeadID,	1  FROM ChartsOfAccount where ChartsOfAccount.AccountHeadID=1

--EnumAccountType{None = 0,Component = 1,Segment =2,Group = 3,SubGroup = 4,Ledger = 5}
INSERT INTO #TempTable2 (AccountHeadID)
			   SELECT TT.AccountHeadID FROM #TempTable AS TT WHERE TT.AccountType=5

DECLARE
@SessionID as int,
@SessionStartDate as date

IF EXISTS(SELECT SessionID FROM AccountingSession  WHERE CONVERT(DATE,CONVERT(VARCHAR(12),StartDate,106))=CONVERT(DATE,CONVERT(VARCHAR(12),@StartDate,106)) AND SessionType=6)
BEGIN
	SET @SessionID=(SELECT top(1)SessionID FROM AccountingSession  WHERE CONVERT(DATE,CONVERT(VARCHAR(12),StartDate,106))=CONVERT(DATE,CONVERT(VARCHAR(12),@StartDate,106)) AND SessionType=6)
END
ELSE
BEGIN
	SET @SessionID=(SELECT TOP 1 SessionID FROM AccountingSession  WHERE CONVERT(DATE,CONVERT(VARCHAR(12),StartDate,106))>=CONVERT(DATE,CONVERT(VARCHAR(12),@StartDate,106)) AND SessionType=6)
	SET @StartDate=(SELECT MIN(StartDate) FROM AccountingSession  WHERE  CONVERT(DATE,CONVERT(VARCHAR(12),StartDate,106))>=CONVERT(DATE,CONVERT(VARCHAR(12),@StartDate,106)) AND SessionType=6)
END
SET @SessionStartDate=(SELECT StartDate FROM AccountingSession WHERE  AccountingSessionID=@SessionID)


IF(@BusinessUnitID>0)
BEGIN
	UPDATE #TempTable2
	SET	ComponentType=dbo.GetComponentID(TT.AccountHeadID),
		DebitOpeiningValue=ISNULL((SELECT SUM(AO.OpenningBalance) FROM AccountOpenning AS AO WHERE  AO.BusinessUnitID=@BusinessUnitID AND AO.IsDebit=1 AND AO.AccountHeadID=TT.AccountHeadID AND AO.AccountingSessionID=@SessionID),0),
		CreditOpeiningValue=ISNULL((SELECT SUM(AO.OpenningBalance) FROM AccountOpenning AS AO WHERE  AO.BusinessUnitID=@BusinessUnitID AND AO.IsDebit=0 AND AO.AccountHeadID=TT.AccountHeadID AND AO.AccountingSessionID=@SessionID),0),
		DebitAmount=ISNULL((SELECT SUM(VD.Amount) FROM View_VoucherDetail AS VD WHERE VD.BUID=@BusinessUnitID AND VD.IsDebit=1 AND VD.AccountHeadID=TT.AccountHeadID AND VD.VoucherID IN(SELECT V.VoucherID FROM Voucher AS V WHERE ISNULL(V.AuthorizedBy,0)!=0 AND CONVERT(DATE,CONVERT(VARCHAR(12),V.VoucherDate,106)) BETWEEN CONVERT(DATE,CONVERT(VARCHAR(12),@SessionStartDate,106)) AND CONVERT(DATE,CONVERT(VARCHAR(12), DATEADD(DAY,-1,@StartDate),106)))),0),
		CreditAmount=ISNULL((SELECT SUM(VD.Amount) FROM View_VoucherDetail AS VD WHERE VD.BUID=@BusinessUnitID AND VD.IsDebit=0 AND VD.AccountHeadID=TT.AccountHeadID AND VD.VoucherID IN(SELECT V.VoucherID FROM Voucher AS V WHERE ISNULL(V.AuthorizedBy,0)!=0 AND CONVERT(DATE,CONVERT(VARCHAR(12),V.VoucherDate,106)) BETWEEN CONVERT(DATE,CONVERT(VARCHAR(12),@SessionStartDate,106)) AND CONVERT(DATE,CONVERT(VARCHAR(12), DATEADD(DAY,-1,@StartDate),106)))),0)
	FROM #TempTable2 AS TT 
END
ELSE
BEGIN
	UPDATE #TempTable2
	SET	ComponentType=dbo.GetComponentID(TT.AccountHeadID),
		DebitOpeiningValue=ISNULL((SELECT SUM(AO.OpenningBalance) FROM AccountOpenning AS AO WHERE AO.IsDebit=1 AND AO.AccountHeadID=TT.AccountHeadID AND AO.AccountingSessionID=@SessionID),0),
		CreditOpeiningValue=ISNULL((SELECT SUM(AO.OpenningBalance) FROM AccountOpenning AS AO WHERE AO.IsDebit=0 AND AO.AccountHeadID=TT.AccountHeadID AND AO.AccountingSessionID=@SessionID),0),
		DebitAmount=ISNULL((SELECT SUM(VD.Amount) FROM View_VoucherDetail AS VD WHERE VD.IsDebit=1 AND VD.AccountHeadID=TT.AccountHeadID AND VD.VoucherID IN(SELECT V.VoucherID FROM Voucher AS V WHERE ISNULL(V.AuthorizedBy,0)!=0 AND CONVERT(DATE,CONVERT(VARCHAR(12),V.VoucherDate,106)) BETWEEN CONVERT(DATE,CONVERT(VARCHAR(12),@SessionStartDate,106)) AND CONVERT(DATE,CONVERT(VARCHAR(12), DATEADD(DAY,-1,@StartDate),106)))),0),
		CreditAmount=ISNULL((SELECT SUM(VD.Amount) FROM View_VoucherDetail AS VD WHERE VD.IsDebit=0 AND VD.AccountHeadID=TT.AccountHeadID AND VD.VoucherID IN(SELECT V.VoucherID FROM Voucher AS V WHERE ISNULL(V.AuthorizedBy,0)!=0 AND CONVERT(DATE,CONVERT(VARCHAR(12),V.VoucherDate,106)) BETWEEN CONVERT(DATE,CONVERT(VARCHAR(12),@SessionStartDate,106)) AND CONVERT(DATE,CONVERT(VARCHAR(12), DATEADD(DAY,-1,@StartDate),106)))),0)
	FROM #TempTable2 AS TT 
END


UPDATE #TempTable2 SET ClosingValue=TT.DebitOpeiningValue-TT.CreditOpeiningValue+TT.DebitAmount-TT.CreditAmount FROM #TempTable2 AS TT WHERE TT.ComponentType IN (2,6)
UPDATE #TempTable2 SET ClosingValue=TT.CreditOpeiningValue-TT.DebitOpeiningValue+TT.CreditAmount-TT.DebitAmount FROM #TempTable2 AS TT WHERE TT.ComponentType NOT IN (2,6)

UPDATE #TempTable
SET ComponentType=dbo.GetComponentID(TT.AccountHeadID),
	AccountCode=(SELECT COA.AccountCode FROM ChartsOfAccount AS COA WHERE COA.AccountHeadID=TT.AccountHeadID),
	AccountHeadName=(SELECT COA.AccountHeadName FROM ChartsOfAccount AS COA WHERE COA.AccountHeadID=TT.AccountHeadID)
FROM #TempTable AS TT WHERE AccountHeadID !=1
------------------------------

IF(@BusinessUnitID>0)
BEGIN
	UPDATE #TempTable
	SET OpenningBalance=ISNULL((SELECT BB.ClosingValue FROM #TempTable2 AS BB WHERE BB.AccountHeadID=TT.AccountHeadID),0),
		DebitAmount=ISNULL((SELECT SUM(VD.Amount) FROM View_VoucherDetail AS VD WHERE VD.BUID=@BusinessUnitID AND VD.IsDebit=1  AND VD.AccountHeadID=TT.AccountHeadID AND VD.VoucherID IN(SELECT V.VoucherID FROM Voucher AS V WHERE ISNULL(V.AuthorizedBy,0)!=0 AND CONVERT(DATE,CONVERT(VARCHAR(12),V.VoucherDate,106)) BETWEEN CONVERT(DATE,CONVERT(VARCHAR(12),@StartDate,106)) AND CONVERT(DATE,CONVERT(VARCHAR(12), @EndDate,106)))),0),
		CreditAmount=ISNULL((SELECT SUM(VD.Amount) FROM View_VoucherDetail AS VD WHERE VD.BUID=@BusinessUnitID AND VD.IsDebit=0 AND VD.AccountHeadID=TT.AccountHeadID AND VD.VoucherID IN(SELECT V.VoucherID FROM Voucher AS V WHERE ISNULL(V.AuthorizedBy,0)!=0 AND CONVERT(DATE,CONVERT(VARCHAR(12),V.VoucherDate,106)) BETWEEN CONVERT(DATE,CONVERT(VARCHAR(12),@StartDate,106)) AND CONVERT(DATE,CONVERT(VARCHAR(12), @EndDate,106)))),0)
	FROM #TempTable AS TT WHERE TT.AccountType=5
END
ELSE
BEGIN
	UPDATE #TempTable
	SET OpenningBalance=ISNULL((SELECT BB.ClosingValue FROM #TempTable2 AS BB WHERE BB.AccountHeadID=TT.AccountHeadID),0),
		DebitAmount=ISNULL((SELECT SUM(VD.Amount) FROM View_VoucherDetail AS VD WHERE VD.IsDebit=1  AND VD.AccountHeadID=TT.AccountHeadID AND VD.VoucherID IN(SELECT V.VoucherID FROM Voucher AS V WHERE ISNULL(V.AuthorizedBy,0)!=0 AND CONVERT(DATE,CONVERT(VARCHAR(12),V.VoucherDate,106)) BETWEEN CONVERT(DATE,CONVERT(VARCHAR(12),@StartDate,106)) AND CONVERT(DATE,CONVERT(VARCHAR(12), @EndDate,106)))),0),
		CreditAmount=ISNULL((SELECT SUM(VD.Amount) FROM View_VoucherDetail AS VD WHERE VD.IsDebit=0 AND VD.AccountHeadID=TT.AccountHeadID AND VD.VoucherID IN(SELECT V.VoucherID FROM Voucher AS V WHERE ISNULL(V.AuthorizedBy,0)!=0 AND CONVERT(DATE,CONVERT(VARCHAR(12),V.VoucherDate,106)) BETWEEN CONVERT(DATE,CONVERT(VARCHAR(12),@StartDate,106)) AND CONVERT(DATE,CONVERT(VARCHAR(12), @EndDate,106)))),0)
	FROM #TempTable AS TT WHERE TT.AccountType=5
END

UPDATE #TempTable SET ClosingBalance=TT.OpenningBalance+TT.DebitAmount-TT.CreditAmount FROM #TempTable AS TT WHERE TT.ComponentType IN (2,6)
UPDATE #TempTable SET ClosingBalance=TT.OpenningBalance-TT.DebitAmount+TT.CreditAmount FROM #TempTable AS TT WHERE TT.ComponentType NOT IN (2,6)

--Update Ledger
--EnumAccountType{None = 0,Component = 1,Segment =2,Group = 3,SubGroup = 4,Ledger = 5}

UPDATE #TempTable
SET OpenningBalance=ISNULL((SELECT SUM(BB.OpenningBalance) FROM #TempTable AS BB WHERE BB.ParentHeadID=TT.AccountHeadID),0),
	DebitAmount=ISNULL((SELECT SUM(BB.DebitAmount) FROM #TempTable AS BB WHERE BB.ParentHeadID=TT.AccountHeadID),0),
	CreditAmount=ISNULL((SELECT SUM(BB.CreditAmount) FROM #TempTable AS BB WHERE BB.ParentHeadID=TT.AccountHeadID),0),
	ClosingBalance=ISNULL((SELECT SUM(BB.ClosingBalance) FROM #TempTable AS BB WHERE BB.ParentHeadID=TT.AccountHeadID),0)
FROM #TempTable AS TT WHERE TT.AccountType=4

--Update Sub Group
--EnumAccountType{None = 0,Component = 1,Segment =2,Group = 3,SubGroup = 4,Ledger = 5}

UPDATE #TempTable
SET OpenningBalance=ISNULL((SELECT SUM(BB.OpenningBalance) FROM #TempTable AS BB WHERE BB.ParentHeadID=TT.AccountHeadID),0),
	DebitAmount=ISNULL((SELECT SUM(BB.DebitAmount) FROM #TempTable AS BB WHERE BB.ParentHeadID=TT.AccountHeadID),0),
	CreditAmount=ISNULL((SELECT SUM(BB.CreditAmount) FROM #TempTable AS BB WHERE BB.ParentHeadID=TT.AccountHeadID),0),
	ClosingBalance=ISNULL((SELECT SUM(BB.ClosingBalance) FROM #TempTable AS BB WHERE BB.ParentHeadID=TT.AccountHeadID),0)
FROM #TempTable AS TT WHERE TT.AccountType=3

--Update Group
--EnumAccountType{None = 0,Component = 1,Segment =2,Group = 3,SubGroup = 4,Ledger = 5}
UPDATE #TempTable
SET OpenningBalance=ISNULL((SELECT SUM(BB.OpenningBalance) FROM #TempTable AS BB WHERE BB.ParentHeadID=TT.AccountHeadID),0),
	DebitAmount=ISNULL((SELECT SUM(BB.DebitAmount) FROM #TempTable AS BB WHERE BB.ParentHeadID=TT.AccountHeadID),0),
	CreditAmount=ISNULL((SELECT SUM(BB.CreditAmount) FROM #TempTable AS BB WHERE BB.ParentHeadID=TT.AccountHeadID),0),
	ClosingBalance=ISNULL((SELECT SUM(BB.ClosingBalance) FROM #TempTable AS BB WHERE BB.ParentHeadID=TT.AccountHeadID),0)
FROM #TempTable AS TT WHERE TT.AccountType=2

--Update Component
--EnumAccountType{None = 0,Component = 1,Segment =2,Group = 3,SubGroup = 4,Ledger = 5}
UPDATE #TempTable
SET OpenningBalance=ISNULL((SELECT SUM(BB.OpenningBalance) FROM #TempTable AS BB WHERE BB.ParentHeadID=TT.AccountHeadID),0),
	DebitAmount=ISNULL((SELECT SUM(BB.DebitAmount) FROM #TempTable AS BB WHERE BB.ParentHeadID=TT.AccountHeadID),0),
	CreditAmount=ISNULL((SELECT SUM(BB.CreditAmount) FROM #TempTable AS BB WHERE BB.ParentHeadID=TT.AccountHeadID),0),
	ClosingBalance=ISNULL((SELECT SUM(BB.ClosingBalance) FROM #TempTable AS BB WHERE BB.ParentHeadID=TT.AccountHeadID),0)
FROM #TempTable AS TT WHERE TT.AccountType=1



SELECT * FROM #TempTable AS TT  -- WHERE TT.ClosingBalance<>0 and  TT.AccountType <= @Param_AccountTypeInInt
DROP TABLE #TempTable2
DROP TABLE #TempTable
COMMIT TRAN








GO
/****** Object:  StoredProcedure [dbo].[SP_TrialBalance_Categorized]    Script Date: 12/1/2016 2:17:46 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[SP_TrialBalance_Categorized]
(
	@Param_AccountHeadID as int,
	@StartDate as date,
	@EndDate as date,
	@IsApproved as bit,
	@CurrencyID as int,
	@BusinessUnitID as int
)
AS
BEGIN TRAN
--DECLARE
--@Param_AccountHeadID int,
--@StartDate as date,
--@EndDate as date,
--@IsApproved as bit,
--@CurrencyID as int,
--@BusinessUnitID as int

--SET @StartDate=CONVERT(DATE,'17 NOV 2015') 
--SET @EndDate=CONVERT(DATE,'18 NOV 2015') 
--SET @Param_AccountHeadID=0
--SET @IsApproved=1
--SET @CurrencyID=0
--SET @BusinessUnitID=0

DECLARE
 @AccountHeadID as int 
IF(@Param_AccountHeadID <=0 or @Param_AccountHeadID is null)
BEGIN
	SET @AccountHeadID =1
END
Else
BEGIN
	SET @AccountHeadID = @Param_AccountHeadID
END

CREATE TABLE #TempTable (
							AccountHeadID int,
							AccountCode Varchar(512),
							AccountHeadName Varchar(512),						
							ComponentType int,
							AccountType smallint,		
							ParentHeadID int,
							OpenningBalance decimal(30,17),
							DebitAmount decimal(30,17),
							CreditAmount decimal(30,17),
							ClosingBalance decimal(30,17)
						)

CREATE TABLE #TempTable2(
							AccountHeadID int,	
							ComponentType int,						
							DebitOpeiningValue decimal(30,17),		
							CreditOpeiningValue decimal(30,17),												
							DebitAmount decimal(30,17),
							CreditAmount decimal(30,17),
							ClosingValue decimal(30,17)							
						)

INSERT INTO #TempTable	   (AccountHeadID,	AccountCode,	AccountHeadName,	ParentHeadID,	AccountType)
				SELECT		AccountHeadID,	AccountCode,	AccountHeadName,	ParentHeadID,	AccountType FROM dbo.GetAccountHeads(@AccountHeadID)

				--SELECT		AccountHeadID,	AccountCode,	AccountHeadName,	ParentHeadID,	AccountType FROM dbo.GetAccountHeads(@AccountHeadID)

INSERT INTO #TempTable	   (AccountHeadID,	AccountCode,	AccountHeadName,	ParentHeadID,	AccountType)
				SELECT		AccountHeadID,	AccountCode,	'Grand Total',		ParentHeadID,	1  FROM ChartsOfAccount where ChartsOfAccount.AccountHeadID=1

--EnumAccountType{None = 0,Component = 1,Segment =2,Group = 3,SubGroup = 4,Ledger = 5}
INSERT INTO #TempTable2 (AccountHeadID)
			   SELECT TT.AccountHeadID FROM #TempTable AS TT WHERE TT.AccountType=5

DECLARE
@BaseCurrencyID as int,
@SessionID as int,
@SessionStartDate as date

IF EXISTS(SELECT SessionID FROM AccountingSession  WHERE CONVERT(DATE,CONVERT(VARCHAR(12),StartDate,106))=CONVERT(DATE,CONVERT(VARCHAR(12),@StartDate,106)) AND SessionType=6)
BEGIN
	SET @SessionID=(SELECT top(1)SessionID FROM AccountingSession  WHERE CONVERT(DATE,CONVERT(VARCHAR(12),StartDate,106))=CONVERT(DATE,CONVERT(VARCHAR(12),@StartDate,106)) AND SessionType=6)
END
ELSE
BEGIN
	SET @SessionID=(SELECT TOP 1 SessionID FROM AccountingSession  WHERE CONVERT(DATE,CONVERT(VARCHAR(12),StartDate,106))>=CONVERT(DATE,CONVERT(VARCHAR(12),@StartDate,106)) AND SessionType=6)
	SET @StartDate=(SELECT MIN(StartDate) FROM AccountingSession  WHERE  CONVERT(DATE,CONVERT(VARCHAR(12),StartDate,106))>=CONVERT(DATE,CONVERT(VARCHAR(12),@StartDate,106)) AND SessionType=6)
END
SET @SessionStartDate=(SELECT StartDate FROM AccountingSession WHERE  AccountingSessionID=@SessionID)

SET @BaseCurrencyID=(SELECT BaseCurrencyID FROM Company WHERE CompanyID=1)
IF(@CurrencyID<=0)
BEGIN
	SET @CurrencyID=(SELECT BaseCurrencyID FROM Company WHERE CompanyID=1)
END

IF(@CurrencyID=@BaseCurrencyID)
BEGIN
	IF(@IsApproved=1)
	BEGIN
		IF(@BusinessUnitID>0)
		BEGIN
			UPDATE #TempTable2
			SET	ComponentType=dbo.GetComponentID(TT.AccountHeadID),
				DebitOpeiningValue=ISNULL((SELECT SUM(AO.OpenningBalance) FROM AccountOpenning AS AO WHERE  AO.BusinessUnitID=@BusinessUnitID AND AO.IsDebit=1 AND AO.AccountHeadID=TT.AccountHeadID AND AO.AccountingSessionID=@SessionID),0),
				CreditOpeiningValue=ISNULL((SELECT SUM(AO.OpenningBalance) FROM AccountOpenning AS AO WHERE  AO.BusinessUnitID=@BusinessUnitID AND AO.IsDebit=0 AND AO.AccountHeadID=TT.AccountHeadID AND AO.AccountingSessionID=@SessionID),0),
				DebitAmount=ISNULL((SELECT SUM(VD.Amount) FROM View_VoucherDetail AS VD WHERE VD.BUID=@BusinessUnitID AND VD.IsDebit=1 AND VD.AccountHeadID=TT.AccountHeadID AND ISNULL(VD.AuthorizedBy,0)!=0 AND CONVERT(DATE,CONVERT(VARCHAR(12),VD.VoucherDate,106)) BETWEEN CONVERT(DATE,CONVERT(VARCHAR(12),@SessionStartDate,106)) AND CONVERT(DATE,CONVERT(VARCHAR(12), DATEADD(DAY,-1,@StartDate),106))),0),
				CreditAmount=ISNULL((SELECT SUM(VD.Amount) FROM View_VoucherDetail AS VD WHERE VD.BUID=@BusinessUnitID AND VD.IsDebit=0 AND VD.AccountHeadID=TT.AccountHeadID AND ISNULL(VD.AuthorizedBy,0)!=0 AND CONVERT(DATE,CONVERT(VARCHAR(12),VD.VoucherDate,106)) BETWEEN CONVERT(DATE,CONVERT(VARCHAR(12),@SessionStartDate,106)) AND CONVERT(DATE,CONVERT(VARCHAR(12), DATEADD(DAY,-1,@StartDate),106))),0)
			FROM #TempTable2 AS TT 
		END
		ELSE
		BEGIN
			UPDATE #TempTable2
			SET	ComponentType=dbo.GetComponentID(TT.AccountHeadID),
				DebitOpeiningValue=ISNULL((SELECT SUM(AO.OpenningBalance) FROM AccountOpenning AS AO WHERE AO.IsDebit=1 AND AO.AccountHeadID=TT.AccountHeadID AND AO.AccountingSessionID=@SessionID),0),
				CreditOpeiningValue=ISNULL((SELECT SUM(AO.OpenningBalance) FROM AccountOpenning AS AO WHERE AO.IsDebit=0 AND AO.AccountHeadID=TT.AccountHeadID AND AO.AccountingSessionID=@SessionID),0),
				DebitAmount=ISNULL((SELECT SUM(VD.Amount) FROM View_VoucherDetail AS VD WHERE VD.IsDebit=1 AND VD.AccountHeadID=TT.AccountHeadID AND ISNULL(VD.AuthorizedBy,0)!=0 AND CONVERT(DATE,CONVERT(VARCHAR(12),VD.VoucherDate,106)) BETWEEN CONVERT(DATE,CONVERT(VARCHAR(12),@SessionStartDate,106)) AND CONVERT(DATE,CONVERT(VARCHAR(12), DATEADD(DAY,-1,@StartDate),106))),0),
				CreditAmount=ISNULL((SELECT SUM(VD.Amount) FROM View_VoucherDetail AS VD WHERE VD.IsDebit=0 AND VD.AccountHeadID=TT.AccountHeadID AND ISNULL(VD.AuthorizedBy,0)!=0 AND CONVERT(DATE,CONVERT(VARCHAR(12),VD.VoucherDate,106)) BETWEEN CONVERT(DATE,CONVERT(VARCHAR(12),@SessionStartDate,106)) AND CONVERT(DATE,CONVERT(VARCHAR(12), DATEADD(DAY,-1,@StartDate),106))),0)
			FROM #TempTable2 AS TT 
		END
	END
	ELSE
	BEGIN
		IF(@BusinessUnitID>0)
		BEGIN
			UPDATE #TempTable2
			SET	ComponentType=dbo.GetComponentID(TT.AccountHeadID),
				DebitOpeiningValue=ISNULL((SELECT SUM(AO.OpenningBalance) FROM AccountOpenning AS AO WHERE  AO.BusinessUnitID=@BusinessUnitID AND AO.IsDebit=1 AND AO.AccountHeadID=TT.AccountHeadID AND AO.AccountingSessionID=@SessionID),0),
				CreditOpeiningValue=ISNULL((SELECT SUM(AO.OpenningBalance) FROM AccountOpenning AS AO WHERE  AO.BusinessUnitID=@BusinessUnitID AND AO.IsDebit=0 AND AO.AccountHeadID=TT.AccountHeadID AND AO.AccountingSessionID=@SessionID),0),
				DebitAmount=ISNULL((SELECT SUM(VD.Amount) FROM View_VoucherDetail AS VD WHERE VD.BUID=@BusinessUnitID AND VD.IsDebit=1 AND VD.AccountHeadID=TT.AccountHeadID AND CONVERT(DATE,CONVERT(VARCHAR(12),VD.VoucherDate,106)) BETWEEN CONVERT(DATE,CONVERT(VARCHAR(12),@SessionStartDate,106)) AND CONVERT(DATE,CONVERT(VARCHAR(12), DATEADD(DAY,-1,@StartDate),106))),0),
				CreditAmount=ISNULL((SELECT SUM(VD.Amount) FROM View_VoucherDetail AS VD WHERE VD.BUID=@BusinessUnitID AND VD.IsDebit=0 AND VD.AccountHeadID=TT.AccountHeadID AND CONVERT(DATE,CONVERT(VARCHAR(12),VD.VoucherDate,106)) BETWEEN CONVERT(DATE,CONVERT(VARCHAR(12),@SessionStartDate,106)) AND CONVERT(DATE,CONVERT(VARCHAR(12), DATEADD(DAY,-1,@StartDate),106))),0)
			FROM #TempTable2 AS TT 
		END
		ELSE
		BEGIN
			UPDATE #TempTable2
			SET	ComponentType=dbo.GetComponentID(TT.AccountHeadID),
				DebitOpeiningValue=ISNULL((SELECT SUM(AO.OpenningBalance) FROM AccountOpenning AS AO WHERE AO.IsDebit=1 AND AO.AccountHeadID=TT.AccountHeadID AND AO.AccountingSessionID=@SessionID),0),
				CreditOpeiningValue=ISNULL((SELECT SUM(AO.OpenningBalance) FROM AccountOpenning AS AO WHERE AO.IsDebit=0 AND AO.AccountHeadID=TT.AccountHeadID AND AO.AccountingSessionID=@SessionID),0),
				DebitAmount=ISNULL((SELECT SUM(VD.Amount) FROM View_VoucherDetail AS VD WHERE VD.IsDebit=1 AND VD.AccountHeadID=TT.AccountHeadID AND CONVERT(DATE,CONVERT(VARCHAR(12),VD.VoucherDate,106)) BETWEEN CONVERT(DATE,CONVERT(VARCHAR(12),@SessionStartDate,106)) AND CONVERT(DATE,CONVERT(VARCHAR(12), DATEADD(DAY,-1,@StartDate),106))),0),
				CreditAmount=ISNULL((SELECT SUM(VD.Amount) FROM View_VoucherDetail AS VD WHERE VD.IsDebit=0 AND VD.AccountHeadID=TT.AccountHeadID AND CONVERT(DATE,CONVERT(VARCHAR(12),VD.VoucherDate,106)) BETWEEN CONVERT(DATE,CONVERT(VARCHAR(12),@SessionStartDate,106)) AND CONVERT(DATE,CONVERT(VARCHAR(12), DATEADD(DAY,-1,@StartDate),106))),0)
			FROM #TempTable2 AS TT 
		END
	END
END
ELSE
BEGIN
	IF(@IsApproved=1)
	BEGIN
		IF(@BusinessUnitID>0)
		BEGIN
			UPDATE #TempTable2
			SET	ComponentType=dbo.GetComponentID(TT.AccountHeadID),
				DebitOpeiningValue=ISNULL((SELECT SUM(AO.AmountInCurrency) FROM AccountOpenning AS AO WHERE  AO.BusinessUnitID=@BusinessUnitID AND AO.CurrencyID=@CurrencyID AND AO.IsDebit=1 AND AO.AccountHeadID=TT.AccountHeadID AND AO.AccountingSessionID=@SessionID),0),
				CreditOpeiningValue=ISNULL((SELECT SUM(AO.AmountInCurrency) FROM AccountOpenning AS AO WHERE  AO.BusinessUnitID=@BusinessUnitID AND AO.CurrencyID=@CurrencyID AND AO.IsDebit=0 AND AO.AccountHeadID=TT.AccountHeadID AND AO.AccountingSessionID=@SessionID),0),
				DebitAmount=ISNULL((SELECT SUM(VD.AmountInCurrency) FROM View_VoucherDetail AS VD WHERE VD.BUID=@BusinessUnitID AND VD.IsDebit=1 AND VD.CurrencyID=@CurrencyID AND VD.AccountHeadID=TT.AccountHeadID AND ISNULL(VD.AuthorizedBy,0)!=0 AND CONVERT(DATE,CONVERT(VARCHAR(12),VD.VoucherDate,106)) BETWEEN CONVERT(DATE,CONVERT(VARCHAR(12),@SessionStartDate,106)) AND CONVERT(DATE,CONVERT(VARCHAR(12), DATEADD(DAY,-1,@StartDate),106))),0),
				CreditAmount=ISNULL((SELECT SUM(VD.AmountInCurrency) FROM View_VoucherDetail AS VD WHERE VD.BUID=@BusinessUnitID AND VD.IsDebit=0 AND VD.CurrencyID=@CurrencyID AND VD.AccountHeadID=TT.AccountHeadID AND ISNULL(VD.AuthorizedBy,0)!=0 AND CONVERT(DATE,CONVERT(VARCHAR(12),VD.VoucherDate,106)) BETWEEN CONVERT(DATE,CONVERT(VARCHAR(12),@SessionStartDate,106)) AND CONVERT(DATE,CONVERT(VARCHAR(12), DATEADD(DAY,-1,@StartDate),106))),0)
			FROM #TempTable2 AS TT 
		END
		ELSE
		BEGIN
			UPDATE #TempTable2
			SET	ComponentType=dbo.GetComponentID(TT.AccountHeadID),
				DebitOpeiningValue=ISNULL((SELECT SUM(AO.AmountInCurrency) FROM AccountOpenning AS AO WHERE AO.IsDebit=1 AND AO.CurrencyID=@CurrencyID AND AO.AccountHeadID=TT.AccountHeadID AND AO.AccountingSessionID=@SessionID),0),
				CreditOpeiningValue=ISNULL((SELECT SUM(AO.AmountInCurrency) FROM AccountOpenning AS AO WHERE AO.IsDebit=0 AND AO.CurrencyID=@CurrencyID AND AO.AccountHeadID=TT.AccountHeadID AND AO.AccountingSessionID=@SessionID),0),
				DebitAmount=ISNULL((SELECT SUM(VD.AmountInCurrency) FROM View_VoucherDetail AS VD WHERE VD.IsDebit=1 AND VD.CurrencyID=@CurrencyID AND VD.AccountHeadID=TT.AccountHeadID AND ISNULL(VD.AuthorizedBy,0)!=0 AND CONVERT(DATE,CONVERT(VARCHAR(12),VD.VoucherDate,106)) BETWEEN CONVERT(DATE,CONVERT(VARCHAR(12),@SessionStartDate,106)) AND CONVERT(DATE,CONVERT(VARCHAR(12), DATEADD(DAY,-1,@StartDate),106))),0),
				CreditAmount=ISNULL((SELECT SUM(VD.AmountInCurrency) FROM View_VoucherDetail AS VD WHERE VD.IsDebit=0 AND VD.CurrencyID=@CurrencyID AND VD.AccountHeadID=TT.AccountHeadID AND ISNULL(VD.AuthorizedBy,0)!=0 AND CONVERT(DATE,CONVERT(VARCHAR(12),VD.VoucherDate,106)) BETWEEN CONVERT(DATE,CONVERT(VARCHAR(12),@SessionStartDate,106)) AND CONVERT(DATE,CONVERT(VARCHAR(12), DATEADD(DAY,-1,@StartDate),106))),0)
			FROM #TempTable2 AS TT 
		END
	END
	ELSE
	BEGIN
		IF(@BusinessUnitID>0)
		BEGIN
			UPDATE #TempTable2
			SET	ComponentType=dbo.GetComponentID(TT.AccountHeadID),
				DebitOpeiningValue=ISNULL((SELECT SUM(AO.AmountInCurrency) FROM AccountOpenning AS AO WHERE  AO.BusinessUnitID=@BusinessUnitID AND AO.CurrencyID=@CurrencyID AND AO.IsDebit=1 AND AO.AccountHeadID=TT.AccountHeadID AND AO.AccountingSessionID=@SessionID),0),
				CreditOpeiningValue=ISNULL((SELECT SUM(AO.AmountInCurrency) FROM AccountOpenning AS AO WHERE  AO.BusinessUnitID=@BusinessUnitID AND AO.CurrencyID=@CurrencyID AND AO.IsDebit=0 AND AO.AccountHeadID=TT.AccountHeadID AND AO.AccountingSessionID=@SessionID),0),
				DebitAmount=ISNULL((SELECT SUM(VD.AmountInCurrency) FROM View_VoucherDetail AS VD WHERE VD.BUID=@BusinessUnitID AND VD.IsDebit=1 AND VD.CurrencyID=@CurrencyID AND VD.AccountHeadID=TT.AccountHeadID AND CONVERT(DATE,CONVERT(VARCHAR(12),VD.VoucherDate,106)) BETWEEN CONVERT(DATE,CONVERT(VARCHAR(12),@SessionStartDate,106)) AND CONVERT(DATE,CONVERT(VARCHAR(12), DATEADD(DAY,-1,@StartDate),106))),0),
				CreditAmount=ISNULL((SELECT SUM(VD.AmountInCurrency) FROM View_VoucherDetail AS VD WHERE VD.BUID=@BusinessUnitID AND VD.IsDebit=0 AND VD.CurrencyID=@CurrencyID AND VD.AccountHeadID=TT.AccountHeadID AND CONVERT(DATE,CONVERT(VARCHAR(12),VD.VoucherDate,106)) BETWEEN CONVERT(DATE,CONVERT(VARCHAR(12),@SessionStartDate,106)) AND CONVERT(DATE,CONVERT(VARCHAR(12), DATEADD(DAY,-1,@StartDate),106))),0)
			FROM #TempTable2 AS TT 
		END
		ELSE
		BEGIN
			UPDATE #TempTable2
			SET	ComponentType=dbo.GetComponentID(TT.AccountHeadID),
				DebitOpeiningValue=ISNULL((SELECT SUM(AO.AmountInCurrency) FROM AccountOpenning AS AO WHERE AO.IsDebit=1 AND AO.CurrencyID=@CurrencyID AND AO.AccountHeadID=TT.AccountHeadID AND AO.AccountingSessionID=@SessionID),0),
				CreditOpeiningValue=ISNULL((SELECT SUM(AO.AmountInCurrency) FROM AccountOpenning AS AO WHERE AO.IsDebit=0 AND AO.CurrencyID=@CurrencyID AND AO.AccountHeadID=TT.AccountHeadID AND AO.AccountingSessionID=@SessionID),0),
				DebitAmount=ISNULL((SELECT SUM(VD.AmountInCurrency) FROM View_VoucherDetail AS VD WHERE VD.IsDebit=1 AND VD.CurrencyID=@CurrencyID AND VD.AccountHeadID=TT.AccountHeadID AND CONVERT(DATE,CONVERT(VARCHAR(12),VD.VoucherDate,106)) BETWEEN CONVERT(DATE,CONVERT(VARCHAR(12),@SessionStartDate,106)) AND CONVERT(DATE,CONVERT(VARCHAR(12), DATEADD(DAY,-1,@StartDate),106))),0),
				CreditAmount=ISNULL((SELECT SUM(VD.AmountInCurrency) FROM View_VoucherDetail AS VD WHERE VD.IsDebit=0 AND VD.CurrencyID=@CurrencyID AND VD.AccountHeadID=TT.AccountHeadID AND CONVERT(DATE,CONVERT(VARCHAR(12),VD.VoucherDate,106)) BETWEEN CONVERT(DATE,CONVERT(VARCHAR(12),@SessionStartDate,106)) AND CONVERT(DATE,CONVERT(VARCHAR(12), DATEADD(DAY,-1,@StartDate),106))),0)
			FROM #TempTable2 AS TT 
		END
	END
END


UPDATE #TempTable2 SET ClosingValue=TT.DebitOpeiningValue-TT.CreditOpeiningValue+TT.DebitAmount-TT.CreditAmount FROM #TempTable2 AS TT WHERE TT.ComponentType IN (2,6)
UPDATE #TempTable2 SET ClosingValue=TT.CreditOpeiningValue-TT.DebitOpeiningValue+TT.CreditAmount-TT.DebitAmount FROM #TempTable2 AS TT WHERE TT.ComponentType NOT IN (2,6)

UPDATE #TempTable
SET ComponentType=dbo.GetComponentID(TT.AccountHeadID),
	AccountCode=(SELECT COA.AccountCode FROM ChartsOfAccount AS COA WHERE COA.AccountHeadID=TT.AccountHeadID),
	AccountHeadName=(SELECT COA.AccountHeadName FROM ChartsOfAccount AS COA WHERE COA.AccountHeadID=TT.AccountHeadID)
FROM #TempTable AS TT WHERE AccountHeadID !=1
------------------------------

IF(@CurrencyID=@BaseCurrencyID)
BEGIN
	IF(@IsApproved=1)
	BEGIN
		IF(@BusinessUnitID>0)
		BEGIN
			UPDATE #TempTable
			SET OpenningBalance=ISNULL((SELECT BB.ClosingValue FROM #TempTable2 AS BB WHERE BB.AccountHeadID=TT.AccountHeadID),0),
				DebitAmount=ISNULL((SELECT SUM(VD.Amount) FROM View_VoucherDetail AS VD WHERE VD.BUID=@BusinessUnitID AND VD.IsDebit=1  AND VD.AccountHeadID=TT.AccountHeadID AND ISNULL(VD.AuthorizedBy,0)!=0 AND CONVERT(DATE,CONVERT(VARCHAR(12),VD.VoucherDate,106)) BETWEEN CONVERT(DATE,CONVERT(VARCHAR(12),@StartDate,106)) AND CONVERT(DATE,CONVERT(VARCHAR(12), @EndDate,106))),0),
				CreditAmount=ISNULL((SELECT SUM(VD.Amount) FROM View_VoucherDetail AS VD WHERE VD.BUID=@BusinessUnitID AND VD.IsDebit=0 AND VD.AccountHeadID=TT.AccountHeadID AND ISNULL(VD.AuthorizedBy,0)!=0 AND CONVERT(DATE,CONVERT(VARCHAR(12),VD.VoucherDate,106)) BETWEEN CONVERT(DATE,CONVERT(VARCHAR(12),@StartDate,106)) AND CONVERT(DATE,CONVERT(VARCHAR(12), @EndDate,106))),0)
			FROM #TempTable AS TT WHERE TT.AccountType=5
		END
		ELSE
		BEGIN
			UPDATE #TempTable
			SET OpenningBalance=ISNULL((SELECT BB.ClosingValue FROM #TempTable2 AS BB WHERE BB.AccountHeadID=TT.AccountHeadID),0),
				DebitAmount=ISNULL((SELECT SUM(VD.Amount) FROM View_VoucherDetail AS VD WHERE VD.IsDebit=1  AND VD.AccountHeadID=TT.AccountHeadID AND ISNULL(VD.AuthorizedBy,0)!=0 AND CONVERT(DATE,CONVERT(VARCHAR(12),VD.VoucherDate,106)) BETWEEN CONVERT(DATE,CONVERT(VARCHAR(12),@StartDate,106)) AND CONVERT(DATE,CONVERT(VARCHAR(12), @EndDate,106))),0),
				CreditAmount=ISNULL((SELECT SUM(VD.Amount) FROM View_VoucherDetail AS VD WHERE VD.IsDebit=0 AND VD.AccountHeadID=TT.AccountHeadID AND ISNULL(VD.AuthorizedBy,0)!=0 AND CONVERT(DATE,CONVERT(VARCHAR(12),VD.VoucherDate,106)) BETWEEN CONVERT(DATE,CONVERT(VARCHAR(12),@StartDate,106)) AND CONVERT(DATE,CONVERT(VARCHAR(12), @EndDate,106))),0)
			FROM #TempTable AS TT WHERE TT.AccountType=5
		END
	END
	ELSE
	BEGIN
		IF(@BusinessUnitID>0)
		BEGIN
			UPDATE #TempTable
			SET OpenningBalance=ISNULL((SELECT BB.ClosingValue FROM #TempTable2 AS BB WHERE BB.AccountHeadID=TT.AccountHeadID),0),
				DebitAmount=ISNULL((SELECT SUM(VD.Amount) FROM View_VoucherDetail AS VD WHERE VD.BUID=@BusinessUnitID AND VD.IsDebit=1  AND VD.AccountHeadID=TT.AccountHeadID AND CONVERT(DATE,CONVERT(VARCHAR(12),VD.VoucherDate,106)) BETWEEN CONVERT(DATE,CONVERT(VARCHAR(12),@StartDate,106)) AND CONVERT(DATE,CONVERT(VARCHAR(12), @EndDate,106))),0),
				CreditAmount=ISNULL((SELECT SUM(VD.Amount) FROM View_VoucherDetail AS VD WHERE VD.BUID=@BusinessUnitID AND VD.IsDebit=0 AND VD.AccountHeadID=TT.AccountHeadID AND CONVERT(DATE,CONVERT(VARCHAR(12),VD.VoucherDate,106)) BETWEEN CONVERT(DATE,CONVERT(VARCHAR(12),@StartDate,106)) AND CONVERT(DATE,CONVERT(VARCHAR(12), @EndDate,106))),0)
			FROM #TempTable AS TT WHERE TT.AccountType=5
		END
		ELSE
		BEGIN
			UPDATE #TempTable
			SET OpenningBalance=ISNULL((SELECT BB.ClosingValue FROM #TempTable2 AS BB WHERE BB.AccountHeadID=TT.AccountHeadID),0),
				DebitAmount=ISNULL((SELECT SUM(VD.Amount) FROM View_VoucherDetail AS VD WHERE VD.IsDebit=1  AND VD.AccountHeadID=TT.AccountHeadID AND CONVERT(DATE,CONVERT(VARCHAR(12),VD.VoucherDate,106)) BETWEEN CONVERT(DATE,CONVERT(VARCHAR(12),@StartDate,106)) AND CONVERT(DATE,CONVERT(VARCHAR(12), @EndDate,106))),0),
				CreditAmount=ISNULL((SELECT SUM(VD.Amount) FROM View_VoucherDetail AS VD WHERE VD.IsDebit=0 AND VD.AccountHeadID=TT.AccountHeadID AND CONVERT(DATE,CONVERT(VARCHAR(12),VD.VoucherDate,106)) BETWEEN CONVERT(DATE,CONVERT(VARCHAR(12),@StartDate,106)) AND CONVERT(DATE,CONVERT(VARCHAR(12), @EndDate,106))),0)
			FROM #TempTable AS TT WHERE TT.AccountType=5
		END
	END
END
ELSE
BEGIN

	IF(@IsApproved=1)
	BEGIN
		IF(@BusinessUnitID>0)
		BEGIN
			UPDATE #TempTable
			SET OpenningBalance=ISNULL((SELECT BB.ClosingValue FROM #TempTable2 AS BB WHERE BB.AccountHeadID=TT.AccountHeadID),0),
				DebitAmount=ISNULL((SELECT SUM(VD.AmountInCurrency) FROM View_VoucherDetail AS VD WHERE VD.BUID=@BusinessUnitID AND VD.IsDebit=1 AND VD.CurrencyID=@CurrencyID  AND VD.AccountHeadID=TT.AccountHeadID AND ISNULL(VD.AuthorizedBy,0)!=0 AND CONVERT(DATE,CONVERT(VARCHAR(12),VD.VoucherDate,106)) BETWEEN CONVERT(DATE,CONVERT(VARCHAR(12),@StartDate,106)) AND CONVERT(DATE,CONVERT(VARCHAR(12), @EndDate,106))),0),
				CreditAmount=ISNULL((SELECT SUM(VD.AmountInCurrency) FROM View_VoucherDetail AS VD WHERE VD.BUID=@BusinessUnitID AND VD.IsDebit=0 AND VD.CurrencyID=@CurrencyID AND VD.AccountHeadID=TT.AccountHeadID AND ISNULL(VD.AuthorizedBy,0)!=0 AND CONVERT(DATE,CONVERT(VARCHAR(12),VD.VoucherDate,106)) BETWEEN CONVERT(DATE,CONVERT(VARCHAR(12),@StartDate,106)) AND CONVERT(DATE,CONVERT(VARCHAR(12), @EndDate,106))),0)
			FROM #TempTable AS TT WHERE TT.AccountType=5
		END
		ELSE
		BEGIN
			UPDATE #TempTable
			SET OpenningBalance=ISNULL((SELECT BB.ClosingValue FROM #TempTable2 AS BB WHERE BB.AccountHeadID=TT.AccountHeadID),0),
				DebitAmount=ISNULL((SELECT SUM(VD.AmountInCurrency) FROM View_VoucherDetail AS VD WHERE VD.IsDebit=1 AND VD.CurrencyID=@CurrencyID  AND VD.AccountHeadID=TT.AccountHeadID AND ISNULL(VD.AuthorizedBy,0)!=0 AND CONVERT(DATE,CONVERT(VARCHAR(12),VD.VoucherDate,106)) BETWEEN CONVERT(DATE,CONVERT(VARCHAR(12),@StartDate,106)) AND CONVERT(DATE,CONVERT(VARCHAR(12), @EndDate,106))),0),
				CreditAmount=ISNULL((SELECT SUM(VD.AmountInCurrency) FROM View_VoucherDetail AS VD WHERE VD.IsDebit=0 AND VD.CurrencyID=@CurrencyID AND VD.AccountHeadID=TT.AccountHeadID AND ISNULL(VD.AuthorizedBy,0)!=0 AND CONVERT(DATE,CONVERT(VARCHAR(12),VD.VoucherDate,106)) BETWEEN CONVERT(DATE,CONVERT(VARCHAR(12),@StartDate,106)) AND CONVERT(DATE,CONVERT(VARCHAR(12), @EndDate,106))),0)
			FROM #TempTable AS TT WHERE TT.AccountType=5
		END
	END
	ELSE
	BEGIN
		IF(@BusinessUnitID>0)
		BEGIN
			UPDATE #TempTable
			SET OpenningBalance=ISNULL((SELECT BB.ClosingValue FROM #TempTable2 AS BB WHERE BB.AccountHeadID=TT.AccountHeadID),0),
				DebitAmount=ISNULL((SELECT SUM(VD.AmountInCurrency) FROM View_VoucherDetail AS VD WHERE VD.BUID=@BusinessUnitID AND VD.IsDebit=1 AND VD.CurrencyID=@CurrencyID  AND VD.AccountHeadID=TT.AccountHeadID AND CONVERT(DATE,CONVERT(VARCHAR(12),VD.VoucherDate,106)) BETWEEN CONVERT(DATE,CONVERT(VARCHAR(12),@StartDate,106)) AND CONVERT(DATE,CONVERT(VARCHAR(12), @EndDate,106))),0),
				CreditAmount=ISNULL((SELECT SUM(VD.AmountInCurrency) FROM View_VoucherDetail AS VD WHERE VD.BUID=@BusinessUnitID AND VD.IsDebit=0 AND VD.CurrencyID=@CurrencyID AND VD.AccountHeadID=TT.AccountHeadID AND CONVERT(DATE,CONVERT(VARCHAR(12),VD.VoucherDate,106)) BETWEEN CONVERT(DATE,CONVERT(VARCHAR(12),@StartDate,106)) AND CONVERT(DATE,CONVERT(VARCHAR(12), @EndDate,106))),0)
			FROM #TempTable AS TT WHERE TT.AccountType=5
		END
		ELSE
		BEGIN
			UPDATE #TempTable
			SET OpenningBalance=ISNULL((SELECT BB.ClosingValue FROM #TempTable2 AS BB WHERE BB.AccountHeadID=TT.AccountHeadID),0),
				DebitAmount=ISNULL((SELECT SUM(VD.AmountInCurrency) FROM View_VoucherDetail AS VD WHERE VD.IsDebit=1  AND VD.CurrencyID=@CurrencyID AND VD.AccountHeadID=TT.AccountHeadID AND CONVERT(DATE,CONVERT(VARCHAR(12),VD.VoucherDate,106)) BETWEEN CONVERT(DATE,CONVERT(VARCHAR(12),@StartDate,106)) AND CONVERT(DATE,CONVERT(VARCHAR(12), @EndDate,106))),0),
				CreditAmount=ISNULL((SELECT SUM(VD.AmountInCurrency) FROM View_VoucherDetail AS VD WHERE VD.IsDebit=0 AND VD.CurrencyID=@CurrencyID AND VD.AccountHeadID=TT.AccountHeadID AND CONVERT(DATE,CONVERT(VARCHAR(12),VD.VoucherDate,106)) BETWEEN CONVERT(DATE,CONVERT(VARCHAR(12),@StartDate,106)) AND CONVERT(DATE,CONVERT(VARCHAR(12), @EndDate,106))),0)
			FROM #TempTable AS TT WHERE TT.AccountType=5
		END
	END
END

UPDATE #TempTable SET ClosingBalance=TT.OpenningBalance+TT.DebitAmount-TT.CreditAmount FROM #TempTable AS TT WHERE TT.ComponentType IN (2,6)
UPDATE #TempTable SET ClosingBalance=TT.OpenningBalance-TT.DebitAmount+TT.CreditAmount FROM #TempTable AS TT WHERE TT.ComponentType NOT IN (2,6)

--Update Ledger
--EnumAccountType{None = 0,Component = 1,Segment =2,Group = 3,SubGroup = 4,Ledger = 5}

UPDATE #TempTable
SET OpenningBalance=ISNULL((SELECT SUM(BB.OpenningBalance) FROM #TempTable AS BB WHERE BB.ParentHeadID=TT.AccountHeadID),0),
	DebitAmount=ISNULL((SELECT SUM(BB.DebitAmount) FROM #TempTable AS BB WHERE BB.ParentHeadID=TT.AccountHeadID),0),
	CreditAmount=ISNULL((SELECT SUM(BB.CreditAmount) FROM #TempTable AS BB WHERE BB.ParentHeadID=TT.AccountHeadID),0),
	ClosingBalance=ISNULL((SELECT SUM(BB.ClosingBalance) FROM #TempTable AS BB WHERE BB.ParentHeadID=TT.AccountHeadID),0)
FROM #TempTable AS TT WHERE TT.AccountType=4

--Update Sub Group
--EnumAccountType{None = 0,Component = 1,Segment =2,Group = 3,SubGroup = 4,Ledger = 5}

UPDATE #TempTable
SET OpenningBalance=ISNULL((SELECT SUM(BB.OpenningBalance) FROM #TempTable AS BB WHERE BB.ParentHeadID=TT.AccountHeadID),0),
	DebitAmount=ISNULL((SELECT SUM(BB.DebitAmount) FROM #TempTable AS BB WHERE BB.ParentHeadID=TT.AccountHeadID),0),
	CreditAmount=ISNULL((SELECT SUM(BB.CreditAmount) FROM #TempTable AS BB WHERE BB.ParentHeadID=TT.AccountHeadID),0),
	ClosingBalance=ISNULL((SELECT SUM(BB.ClosingBalance) FROM #TempTable AS BB WHERE BB.ParentHeadID=TT.AccountHeadID),0)
FROM #TempTable AS TT WHERE TT.AccountType=3

--Update Group
--EnumAccountType{None = 0,Component = 1,Segment =2,Group = 3,SubGroup = 4,Ledger = 5}
UPDATE #TempTable
SET OpenningBalance=ISNULL((SELECT SUM(BB.OpenningBalance) FROM #TempTable AS BB WHERE BB.ParentHeadID=TT.AccountHeadID),0),
	DebitAmount=ISNULL((SELECT SUM(BB.DebitAmount) FROM #TempTable AS BB WHERE BB.ParentHeadID=TT.AccountHeadID),0),
	CreditAmount=ISNULL((SELECT SUM(BB.CreditAmount) FROM #TempTable AS BB WHERE BB.ParentHeadID=TT.AccountHeadID),0),
	ClosingBalance=ISNULL((SELECT SUM(BB.ClosingBalance) FROM #TempTable AS BB WHERE BB.ParentHeadID=TT.AccountHeadID),0)
FROM #TempTable AS TT WHERE TT.AccountType=2

--Update Component
--EnumAccountType{None = 0,Component = 1,Segment =2,Group = 3,SubGroup = 4,Ledger = 5}
UPDATE #TempTable
SET OpenningBalance=ISNULL((SELECT SUM(BB.OpenningBalance) FROM #TempTable AS BB WHERE BB.ParentHeadID=TT.AccountHeadID),0),
	DebitAmount=ISNULL((SELECT SUM(BB.DebitAmount) FROM #TempTable AS BB WHERE BB.ParentHeadID=TT.AccountHeadID),0),
	CreditAmount=ISNULL((SELECT SUM(BB.CreditAmount) FROM #TempTable AS BB WHERE BB.ParentHeadID=TT.AccountHeadID),0),
	ClosingBalance=ISNULL((SELECT SUM(BB.ClosingBalance) FROM #TempTable AS BB WHERE BB.ParentHeadID=TT.AccountHeadID),0)
FROM #TempTable AS TT WHERE TT.AccountType=1



SELECT * FROM #TempTable AS TT   WHERE TT.ParentHeadID=@AccountHeadID
DROP TABLE #TempTable2
DROP TABLE #TempTable
COMMIT TRAN












GO
/****** Object:  StoredProcedure [dbo].[SP_Un_Approved_Voucher]    Script Date: 12/1/2016 2:17:46 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[SP_Un_Approved_Voucher]
(
	@VoucherID as int
)
AS
BEGIN TRAN
--DECLARE
--@VoucherID as int
--SET @VoucherID=952

DECLARE
@TempMessage as Varchar(512)

IF EXISTS(SELECT * FROM Voucher AS HH WHERE HH.VoucherID=@VoucherID AND ISNULL(HH.AuthorizedBy,0)=0)
BEGIN
	ROLLBACK
		RAISERROR (N'Please Select an approved Voucher!!~',16,1);	
	RETURN
END

--IF EXISTS(SELECT * FROM View_VoucherBillTransaction AS TT WHERE TT.VoucherID != @VoucherID AND TT.TrType IN (2,4) AND TT.VoucherBillID IN(SELECT VBT.VoucherBillID FROM VoucherBillTransaction AS VBT WHERE VBT.VoucherDetailID IN(SELECT HH.VoucherDetailID FROM VoucherDetail AS HH WHERE HH.VoucherID=@VoucherID)))
--BEGIN
--	SET @TempMessage =(SELECT TOP 1 TT.VoucherNo FROM View_VoucherBillTransaction AS TT WHERE TT.VoucherID != @VoucherID AND TT.TrType IN (2,4) AND TT.VoucherBillID IN(SELECT VBT.VoucherBillID FROM VoucherBillTransaction AS VBT WHERE VBT.VoucherDetailID IN(SELECT HH.VoucherDetailID FROM VoucherDetail AS HH WHERE HH.VoucherID=@VoucherID)))
--	ROLLBACK
--		RAISERROR (N'Sorry Delete Not Possible Your Selected Voucher Bill(s) already userd as a AgstRef on Voucher No : %s!!~',16,1, @TempMessage);	
--	RETURN
--END

DECLARE 
@VoucherBatchID as int

SET @VoucherBatchID = (SELECT TT.BatchID FROM Voucher AS TT WHERE TT.VoucherID=@VoucherID)
UPDATE VoucherBatch SET BatchStatus=4 WHERE VoucherBatchID=@VoucherBatchID
UPDATE Voucher SET AuthorizedBy =0 WHERE VoucherID=@VoucherID
UPDATE Voucher SET AuthorizedBy =0 WHERE VoucherID=(SELECT HH.CounterVoucherID FROM Voucher AS HH WHERE HH.VoucherID=@VoucherID)
COMMIT TRAN


GO
/****** Object:  StoredProcedure [dbo].[SP_VoucherBatchStatusUpdate]    Script Date: 12/1/2016 2:17:46 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[SP_VoucherBatchStatusUpdate]
(		
	@VoucherBatchID as int,		
	@ActionType as smallint,
	@RequestTo as int,
	@DBUserID as int	
	--%n, %n, %n, %n
)		
AS
BEGIN TRAN		
DECLARE 
@VoucherBatchHistoryID as int,
@BatchStatus as int,
@Note as Varchar(512),
@PV_DBServerDateTime AS datetime,
@PV_CurrentState as int,
@PV_PriviousState as int,
@PV_BatchNO as varchar(100),
@PV_CreateBy as int,
@PV_CreateDate as datetime
SET @PV_DBServerDateTime=Getdate()
	
IF NOT EXISTS(SELECT * FROM VoucherBatch WHERE VoucherBatchID=@VoucherBatchID)
BEGIN
	ROLLBACK
		RAISERROR (N'Invalid Voucher Batch!!~',16,1);	
	RETURN
END
--IF EXISTS(SELECT * FROM Voucher AS V WHERE V.BatchID=@VoucherBatchID AND V.AuthorizedBy!=0)
--BEGIN
--	ROLLBACK
--		RAISERROR (N'Your Selected Voucher Batch has one or more Authorized Vouchers. Status Can not be changed!!~',16,1);	
--	RETURN
--END
SET @BatchStatus=(SELECT TT.BatchStatus FROM VoucherBatch AS TT WHERE VoucherBatchID=@VoucherBatchID)
SET @PV_BatchNO=(SELECT TT.BatchNO FROM VoucherBatch AS TT WHERE VoucherBatchID=@VoucherBatchID)
SET @PV_CreateBy=(SELECT TT.CreateBy FROM VoucherBatch AS TT WHERE VoucherBatchID=@VoucherBatchID)
SET @PV_CreateDate=(SELECT TT.CreateDate FROM VoucherBatch AS TT WHERE VoucherBatchID=@VoucherBatchID)

--EnumVoucherBatchStatus    {        None = 0, BatchOpen = 1, BatchClose = 2, RequestForApprove = 3, ApprovalInProgress = 4, Approved = 5    }

SET @VoucherBatchHistoryID=(SELECT ISNULL(MAX(VoucherBatchHistoryID),0)+1 FROM VoucherBatchHistory)	
IF (@ActionType>=1 AND @ActionType<2)
BEGIN
	IF NOT EXISTS(SELECT BatchStatus FROM VoucherBatch WHERE VoucherBatchID=@VoucherBatchID AND BatchStatus IN (0,2))
	BEGIN
		ROLLBACK
			RAISERROR (N'Your Selected Voucher Batch Status is not valid for Open!!~',16,1);	
		RETURN
	END

	SET @Note ='Batch Open'
	SET @PV_CurrentState=1
	SET @PV_PriviousState =@BatchStatus
	UPDATE VoucherBatch SET BatchStatus =1,LastUpdateBy=@DBUserID,LastUpdateDateTime=@PV_DBServerDateTime WHERE VoucherBatchID = @VoucherBatchID
END

IF (@ActionType>=2 AND @ActionType<3)
BEGIN
	IF NOT EXISTS(SELECT BatchStatus FROM VoucherBatch WHERE VoucherBatchID=@VoucherBatchID AND BatchStatus IN (1,3,4))
	BEGIN
		ROLLBACK
			RAISERROR (N'Your Selected Voucher Batch Status is not valid for Close!!~',16,1);	
		RETURN
	END
	
	SET @Note ='Batch Close'
	SET @PV_CurrentState=2
	SET @PV_PriviousState=@BatchStatus
	UPDATE VoucherBatch SET BatchStatus = 2, RequestTo =0,LastUpdateBy=@DBUserID,LastUpdateDateTime=@PV_DBServerDateTime WHERE VoucherBatchID = @VoucherBatchID
END

			 
IF (@ActionType>=3 AND @ActionType<4)
BEGIN
	IF NOT EXISTS(SELECT BatchStatus FROM VoucherBatch WHERE VoucherBatchID=@VoucherBatchID AND BatchStatus=2)
	BEGIN
		ROLLBACK
			RAISERROR (N'Your Selected Voucher Batch Status need to be Closed first!!~',16,1);	
		RETURN
	END
	
	SET @Note ='Request For Approval'
	SET @PV_CurrentState=3
	SET @PV_PriviousState=@BatchStatus
	
	UPDATE VoucherBatch SET BatchStatus=3, RequestTo =@RequestTo,RequestDate=@PV_DBServerDateTime,LastUpdateBy=@DBUserID,LastUpdateDateTime=@PV_DBServerDateTime  WHERE VoucherBatchID = @VoucherBatchID
END


INSERT INTO VoucherBatchHistory	(VoucherBatchHistoryID,	VoucherBatchID,		BatchNO,		CreateBy,		CreateDate,		PreviousBatchStatus,	CurrentBatchStatus,	RequestTo,	RequestDate,			Note,	DBUserID,	DBServerDateTime,		LastUpdateBy,	LastUpdateDateTime)
    			VALUES			(@VoucherBatchHistoryID,@VoucherBatchID,	@PV_BatchNO,	@PV_CreateBy,	@PV_CreateDate,	@PV_PriviousState,		@PV_CurrentState,	@RequestTo,	@PV_DBServerDateTime,	@Note,	@DBUserID,	@PV_DBServerDateTime,	@DBUserID,		@PV_DBServerDateTime)
    		
SELECT * FROM View_VoucherBatch WHERE VoucherBatchID=@VoucherBatchID
COMMIT TRAN






GO
/****** Object:  StoredProcedure [dbo].[SP_VoucherBillBreakDown]    Script Date: 12/1/2016 2:17:46 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[SP_VoucherBillBreakDown]
(
	@AccountHeadID as int,
	@SubLedgerID as int,
	@OpeningDate as date,
	@EndDate as date,
	@IsApproved as bit,
	@IsAllBill as bit,
	@CurrencyID as int,
	@BusinessUnitID as int
)
AS
BEGIN TRAN
--DECLARE
--@AccountHeadID as int,
--@SubLedgerID as int,
--@OpeningDate as date,
--@EndDate as date,
--@IsApproved as bit,
--@IsAllBill as bit,
--@CurrencyID as int,
--@BusinessUnitID as int

----EXEC [SP_VoucherBillBreakDown] 235, 0, '01 Jan 2016', '31 Oct 2016', 0, 0, 0, 2
--SET @AccountHeadID=235
--SET @SubLedgerID=0--439--462--459
--SET @OpeningDate='01 Jan 2016'
--set @EndDate='01 Jan 2016'
--set @IsApproved=0
--set @IsAllBill=0
--set @CurrencyID=0
--SET @BusinessUnitID=2

DECLARE
@SessionID as int,
@SessionStartDate as date,
@ComponentType as smallint,
@SessionOpenningBalance as decimal(30,17),
@Debit_OpeningAmount as decimal(30,17),
@Credit_OpeningAmount as decimal(30,17),
@OpenningBalance as decimal(30,17),
@IsDr_Opening as bit,
@BaseCurrencyID as int


IF EXISTS(SELECT SessionID FROM AccountingSession  WHERE CONVERT(DATE,CONVERT(VARCHAR(12),StartDate,106))=CONVERT(DATE,CONVERT(VARCHAR(12),@OpeningDate,106)) AND SessionType=6)
BEGIN
	SET @SessionID=(SELECT SessionID FROM AccountingSession  WHERE CONVERT(DATE,CONVERT(VARCHAR(12),StartDate,106))=CONVERT(DATE,CONVERT(VARCHAR(12),@OpeningDate,106)) AND SessionType=6)
END
ELSE
BEGIN
	SET @SessionID=(SELECT TOP 1 SessionID FROM AccountingSession  WHERE CONVERT(DATE,CONVERT(VARCHAR(12),StartDate,106))>=CONVERT(DATE,CONVERT(VARCHAR(12),@OpeningDate,106)) AND SessionType=6)
	SET @OpeningDate=(SELECT MIN(StartDate) FROM AccountingSession  WHERE CONVERT(DATE,CONVERT(VARCHAR(12),StartDate,106))>=CONVERT(DATE,CONVERT(VARCHAR(12),@OpeningDate,106)) AND SessionType=6)
END
SET @SessionStartDate=(SELECT StartDate FROM AccountingSession WHERE AccountingSessionID=@SessionID)
--PRINT @OpeningDate
--PRINT @SessionStartDate
SET @BaseCurrencyID=(SELECT BaseCurrencyID FROM Company WHERE CompanyID=1)
IF(@CurrencyID<=0)
BEGIN
	SET @CurrencyID=(SELECT BaseCurrencyID FROM Company WHERE CompanyID=1)
END

IF(@SubLedgerID<0)
BEGIN
	SET @SubLedgerID=0
END

CREATE TABLE #TempTableVBill (
							ID int IDENTITY(1,1) PRIMARY KEY,
							VoucherBillID int,	
							BillNo Varchar(512),
							Amount decimal(30,17),						
							RemainingBalance decimal(30,17),
							CreditDays int, 
							ComponentType smallint,
							DebitSessionOpening decimal(30,17),
							CreditSessionOpening decimal(30,17),
							Debit_Opening decimal(30,17),
							Credit_Opening decimal(30,17),
							OpeningBalance decimal(30,17),
							IsDebit bit,
							BillDate DateTime,
							DueDate DateTime,
							OverdueByDays int,
							DebitAmount decimal(30,17),
							CreditAmount decimal(30,17),
							ClosingBalance decimal(30,17),
							AccountHeadID int,
							CCID int,
							VoucherID int,
							VoucherNo varchar(512),
							VoucherDate datetime,
							AccountHeadName varchar(512),
							AccountHeadCode varchar(512),
							SubledgerCode varchar(512),
							SubledgerName varchar(512)			
						)

IF(@BusinessUnitID>0)
BEGIN
	IF(@IsAllBill=1)
	BEGIN
		IF(@AccountHeadID>0)
		BEGIN
			INSERT INTO #TempTableVBill(VoucherBillID,		BillDate,	DueDate,	BillNo,		Amount,		RemainingBalance,		CreditDays,		AccountHeadID,	CCID,			OverdueByDays,						ComponentType)
			SELECT DISTINCT				VB.VoucherBillID,	VB.BillDate,VB.DueDate,	VB.BillNo,	VB.Amount,	VB.RemainningBalance,	VB.CreditDays,	@AccountHeadID,	@SubLedgerID,	DATEDIFF(DAY,VB.DueDate,GETDATE()),	dbo.GetComponentID(@AccountHeadID) FROM   View_VoucherBill as VB WHERE VB.AccountHeadID=@AccountHeadID AND VB.SubLedgerID=@SubLedgerID AND VB.BUID=@BusinessUnitID
		END
		ELSE
		BEGIN
			INSERT INTO #TempTableVBill(VoucherBillID,		BillDate,	DueDate,	BillNo,		Amount,		RemainingBalance,		CreditDays,		AccountHeadID,	CCID,			OverdueByDays,						ComponentType)
			SELECT DISTINCT				VB.VoucherBillID,	VB.BillDate,VB.DueDate,	VB.BillNo,	VB.Amount,	VB.RemainningBalance,	VB.CreditDays,	@AccountHeadID,	@SubLedgerID,	DATEDIFF(DAY,VB.DueDate,GETDATE()),	dbo.GetComponentID(@AccountHeadID) FROM   View_VoucherBill as VB WHERE VB.SubLedgerID=@SubLedgerID AND VB.BUID=@BusinessUnitID
		END
	END
	ELSE
	BEGIN
		IF(@AccountHeadID>0)
		BEGIN
			INSERT INTO #TempTableVBill(VoucherBillID,		BillDate,	DueDate,	BillNo,		Amount,		RemainingBalance,		CreditDays,		AccountHeadID,	CCID,			OverdueByDays,						ComponentType)
			SELECT DISTINCT				VB.VoucherBillID,	VB.BillDate,VB.DueDate,	VB.BillNo,	VB.Amount,	VB.RemainningBalance,	VB.CreditDays,	@AccountHeadID,	@SubLedgerID,	DATEDIFF(DAY,VB.DueDate,GETDATE()),	dbo.GetComponentID(@AccountHeadID) FROM   View_VoucherBill as VB WHERE VB.AccountHeadID=@AccountHeadID AND VB.SubLedgerID=@SubLedgerID  AND VB.RemainningBalance>0  AND VB.BUID=@BusinessUnitID
		END
		ELSE
		BEGIN
			INSERT INTO #TempTableVBill(VoucherBillID,		BillDate,	DueDate,	BillNo,		Amount,		RemainingBalance,		CreditDays,		AccountHeadID,	CCID,			OverdueByDays,						ComponentType)
			SELECT DISTINCT				VB.VoucherBillID,	VB.BillDate,VB.DueDate,	VB.BillNo,	VB.Amount,	VB.RemainningBalance,	VB.CreditDays,	@AccountHeadID,	@SubLedgerID,	DATEDIFF(DAY,VB.DueDate,GETDATE()),	dbo.GetComponentID(@AccountHeadID) FROM   View_VoucherBill as VB WHERE VB.SubLedgerID=@SubLedgerID AND VB.RemainningBalance>0  AND VB.BUID=@BusinessUnitID
		END
	END
END
ELSE 
BEGIN
	IF(@IsAllBill=1)
	BEGIN
		IF(@AccountHeadID>0)
		BEGIN
			INSERT INTO #TempTableVBill(VoucherBillID,		BillDate,	DueDate,	BillNo,		Amount,		RemainingBalance,		CreditDays,		AccountHeadID,	CCID,			OverdueByDays,						ComponentType)
			SELECT DISTINCT				VB.VoucherBillID,	VB.BillDate,VB.DueDate,	VB.BillNo,	VB.Amount,	VB.RemainningBalance,	VB.CreditDays,	@AccountHeadID,	@SubLedgerID,	DATEDIFF(DAY,VB.DueDate,GETDATE()),	dbo.GetComponentID(@AccountHeadID) FROM   View_VoucherBill as VB WHERE VB.AccountHeadID=@AccountHeadID AND VB.SubLedgerID=@SubLedgerID
		END
		ELSE
		BEGIN
			INSERT INTO #TempTableVBill(VoucherBillID,		BillDate,	DueDate,	BillNo,		Amount,		RemainingBalance,		CreditDays,		AccountHeadID,	CCID,			OverdueByDays,						ComponentType)
			SELECT DISTINCT				VB.VoucherBillID,	VB.BillDate,VB.DueDate,	VB.BillNo,	VB.Amount,	VB.RemainningBalance,	VB.CreditDays,	@AccountHeadID,	@SubLedgerID,	DATEDIFF(DAY,VB.DueDate,GETDATE()),	dbo.GetComponentID(@AccountHeadID) FROM   View_VoucherBill as VB WHERE VB.SubLedgerID=@SubLedgerID
		END
	END
	ELSE
	BEGIN
		IF(@AccountHeadID>0)
		BEGIN
			INSERT INTO #TempTableVBill(VoucherBillID,		BillDate,	DueDate,	BillNo,		Amount,		RemainingBalance,		CreditDays,		AccountHeadID,	CCID,			OverdueByDays,						ComponentType)
			SELECT DISTINCT				VB.VoucherBillID,	VB.BillDate,VB.DueDate,	VB.BillNo,	VB.Amount,	VB.RemainningBalance,	VB.CreditDays,	@AccountHeadID,	@SubLedgerID,	DATEDIFF(DAY,VB.DueDate,GETDATE()),	dbo.GetComponentID(@AccountHeadID) FROM   View_VoucherBill as VB WHERE VB.AccountHeadID=@AccountHeadID AND VB.SubLedgerID=@SubLedgerID  AND VB.RemainningBalance>0
		END
		ELSE
		BEGIN
			INSERT INTO #TempTableVBill(VoucherBillID,		BillDate,	DueDate,	BillNo,		Amount,		RemainingBalance,		CreditDays,		AccountHeadID,	CCID,			OverdueByDays,						ComponentType)
			SELECT DISTINCT				VB.VoucherBillID,	VB.BillDate,VB.DueDate,	VB.BillNo,	VB.Amount,	VB.RemainningBalance,	VB.CreditDays,	@AccountHeadID,	@SubLedgerID,	DATEDIFF(DAY,VB.DueDate,GETDATE()),	dbo.GetComponentID(@AccountHeadID) FROM   View_VoucherBill as VB WHERE VB.SubLedgerID=@SubLedgerID AND VB.RemainningBalance>0
		END
	END
END


--INSERT INTO #TempTableVBill(VoucherBillID)
--SELECT DISTINCT AOB.CostCenterID FROM AccountOpenningBreakdown AS AOB WHERE AOB.CostCenterID>0 AND AOB.AccountHeadID=@AccountHeadID
--UPDATE #TempTableVBill SET ComponentType=dbo.GetComponentID(@AccountHeadID)

IF(@CurrencyID<>@BaseCurrencyID)
BEGIN
	IF(@AccountHeadID>0)
	BEGIN
		IF(@BusinessUnitID<=0)
		BEGIN
			IF(@IsApproved=1)
			BEGIN
				UPDATE #TempTableVBill
				SET DebitSessionOpening=ISNULL((SELECT SUM(AOB.AmountInCurrency) FROM AccountOpenningBreakdown AS AOB WHERE AOB.IsDr=1 AND AOB.CurrencyID=@CurrencyID AND AOB.CCID=@SubLedgerID AND AOB.AccountHeadID=@AccountHeadID AND AOB.BreakdownObjID=TT.VoucherBillID AND AOB.BreakdownType=2 AND AOB.AccountingSessionID=@SessionID),0),
					CreditSessionOpening=ISNULL((SELECT SUM(AOB.AmountInCurrency) FROM AccountOpenningBreakdown AS AOB WHERE AOB.IsDr=0 AND AOB.CurrencyID=@CurrencyID AND AOB.CCID=@SubLedgerID AND AOB.AccountHeadID=@AccountHeadID AND AOB.BreakdownObjID=TT.VoucherBillID AND AOB.BreakdownType=2 AND AOB.AccountingSessionID=@SessionID),0),
					Debit_Opening=ISNULL((SELECT SUM(VBT.Amount) FROM View_VoucherBillTransaction AS VBT WHERE VBT.CurrencyID=@CurrencyID AND VBT.VoucherBillID=TT.VoucherBillID AND  VBT.IsDr=1 AND  CONVERT(DATE, CONVERT(VARCHAR(12),VBT.TransactionDate,106)) BETWEEN CONVERT(DATE, CONVERT(VARCHAR(12),@SessionStartDate,106)) AND DATEADD(DAY,-1,CONVERT(DATE, CONVERT(VARCHAR(12),@OpeningDate,106))) AND ISNULL(VBT.CCID,0)=@SubLedgerID AND VBT.AccountHeadID=@AccountHeadID AND VBT.ApprovedBy!=0),0),
					Credit_Opening=ISNULL((SELECT SUM(VBT.Amount) FROM View_VoucherBillTransaction AS VBT WHERE VBT.CurrencyID=@CurrencyID AND VBT.VoucherBillID=TT.VoucherBillID AND  VBT.IsDr=0 AND  CONVERT(DATE, CONVERT(VARCHAR(12),VBT.TransactionDate,106)) BETWEEN CONVERT(DATE, CONVERT(VARCHAR(12),@SessionStartDate,106)) AND DATEADD(DAY,-1,CONVERT(DATE, CONVERT(VARCHAR(12),@OpeningDate,106))) AND ISNULL(VBT.CCID,0)=@SubLedgerID AND VBT.AccountHeadID=@AccountHeadID AND VBT.ApprovedBy!=0),0),
					DebitAmount=ISNULL((SELECT SUM(abs(VBT.Amount)*VBT.ConversionRate) FROM View_VoucherBillTransaction AS VBT WHERE VBT.CurrencyID=@CurrencyID AND VBT.VoucherBillID=TT.VoucherBillID AND  VBT.IsDr=1 AND  CONVERT(DATE, CONVERT(VARCHAR(12),VBT.TransactionDate,106)) BETWEEN CONVERT(DATE, CONVERT(VARCHAR(12),@OpeningDate,106)) AND CONVERT(DATE, CONVERT(VARCHAR(12),@EndDate,106)) AND ISNULL(VBT.CCID,0)=@SubLedgerID AND VBT.AccountHeadID=@AccountHeadID AND VBT.ApprovedBy!=0),0),
					CreditAmount=ISNULL((SELECT SUM(abs(VBT.Amount)*VBT.ConversionRate) FROM View_VoucherBillTransaction AS VBT WHERE VBT.CurrencyID=@CurrencyID AND VBT.VoucherBillID=TT.VoucherBillID AND  VBT.IsDr=0 AND  CONVERT(DATE, CONVERT(VARCHAR(12),VBT.TransactionDate,106)) BETWEEN CONVERT(DATE, CONVERT(VARCHAR(12),@OpeningDate,106)) AND CONVERT(DATE, CONVERT(VARCHAR(12),@EndDate,106)) AND ISNULL(VBT.CCID,0)=@SubLedgerID AND VBT.AccountHeadID=@AccountHeadID AND VBT.ApprovedBy!=0),0)
				FROM #TempTableVBill AS TT
			END
			ELSE
			BEGIN
				UPDATE #TempTableVBill
				SET DebitSessionOpening=ISNULL((SELECT SUM(AOB.AmountInCurrency) FROM AccountOpenningBreakdown AS AOB WHERE AOB.IsDr=1 AND AOB.CurrencyID=@CurrencyID AND AOB.CCID=@SubLedgerID AND AOB.AccountHeadID=@AccountHeadID AND AOB.BreakdownObjID=TT.VoucherBillID AND AOB.BreakdownType=2 AND AOB.AccountingSessionID=@SessionID),0),
					CreditSessionOpening=ISNULL((SELECT SUM(AOB.AmountInCurrency) FROM AccountOpenningBreakdown AS AOB WHERE AOB.IsDr=0 AND AOB.CurrencyID=@CurrencyID AND AOB.CCID=@SubLedgerID AND AOB.AccountHeadID=@AccountHeadID AND AOB.BreakdownObjID=TT.VoucherBillID AND AOB.BreakdownType=2 AND AOB.AccountingSessionID=@SessionID),0),
					Debit_Opening=ISNULL((SELECT SUM(VBT.Amount) FROM View_VoucherBillTransaction AS VBT WHERE VBT.CurrencyID=@CurrencyID AND VBT.VoucherBillID=TT.VoucherBillID AND  VBT.IsDr=1 AND  CONVERT(DATE, CONVERT(VARCHAR(12),VBT.TransactionDate,106)) BETWEEN CONVERT(DATE, CONVERT(VARCHAR(12),@SessionStartDate,106)) AND DATEADD(DAY,-1,CONVERT(DATE, CONVERT(VARCHAR(12),@OpeningDate,106))) AND ISNULL(VBT.CCID,0)=@SubLedgerID AND VBT.AccountHeadID=@AccountHeadID),0),
					Credit_Opening=ISNULL((SELECT SUM(VBT.Amount) FROM View_VoucherBillTransaction AS VBT WHERE VBT.CurrencyID=@CurrencyID AND VBT.VoucherBillID=TT.VoucherBillID AND  VBT.IsDr=0 AND  CONVERT(DATE, CONVERT(VARCHAR(12),VBT.TransactionDate,106)) BETWEEN CONVERT(DATE, CONVERT(VARCHAR(12),@SessionStartDate,106)) AND DATEADD(DAY,-1,CONVERT(DATE, CONVERT(VARCHAR(12),@OpeningDate,106))) AND ISNULL(VBT.CCID,0)=@SubLedgerID AND VBT.AccountHeadID=@AccountHeadID),0),
					DebitAmount=ISNULL((SELECT SUM(abs(VBT.Amount)*VBT.ConversionRate) FROM View_VoucherBillTransaction AS VBT WHERE VBT.CurrencyID=@CurrencyID AND VBT.VoucherBillID=TT.VoucherBillID AND  VBT.IsDr=1 AND  CONVERT(DATE, CONVERT(VARCHAR(12),VBT.TransactionDate,106)) BETWEEN CONVERT(DATE, CONVERT(VARCHAR(12),@OpeningDate,106)) AND CONVERT(DATE, CONVERT(VARCHAR(12),@EndDate,106)) AND ISNULL(VBT.CCID,0)=@SubLedgerID AND VBT.AccountHeadID=@AccountHeadID),0),
					CreditAmount=ISNULL((SELECT SUM(abs(VBT.Amount)*VBT.ConversionRate) FROM View_VoucherBillTransaction AS VBT WHERE VBT.CurrencyID=@CurrencyID AND VBT.VoucherBillID=TT.VoucherBillID AND  VBT.IsDr=0 AND  CONVERT(DATE, CONVERT(VARCHAR(12),VBT.TransactionDate,106)) BETWEEN CONVERT(DATE, CONVERT(VARCHAR(12),@OpeningDate,106)) AND CONVERT(DATE, CONVERT(VARCHAR(12),@EndDate,106)) AND ISNULL(VBT.CCID,0)=@SubLedgerID AND VBT.AccountHeadID=@AccountHeadID),0)
				FROM #TempTableVBill AS TT
			END
		END
		ELSE
		BEGIN
			IF(@IsApproved=1)
			BEGIN
				UPDATE #TempTableVBill
				SET DebitSessionOpening=ISNULL((SELECT SUM(AOB.AmountInCurrency) FROM AccountOpenningBreakdown AS AOB WHERE AOB.IsDr=1 AND AOB.BusinessUnitID=@BusinessUnitID AND AOB.CurrencyID=@CurrencyID AND AOB.CCID=@SubLedgerID AND AOB.AccountHeadID=@AccountHeadID AND AOB.BreakdownObjID=TT.VoucherBillID AND AOB.BreakdownType=2 AND AOB.AccountingSessionID=@SessionID),0),
					CreditSessionOpening=ISNULL((SELECT SUM(AOB.AmountInCurrency) FROM AccountOpenningBreakdown AS AOB WHERE AOB.IsDr=0 AND AOB.BusinessUnitID=@BusinessUnitID AND AOB.CurrencyID=@CurrencyID AND AOB.CCID=@SubLedgerID AND AOB.AccountHeadID=@AccountHeadID AND AOB.BreakdownObjID=TT.VoucherBillID AND AOB.BreakdownType=2 AND AOB.AccountingSessionID=@SessionID),0),
					Debit_Opening=ISNULL((SELECT SUM(VBT.Amount) FROM View_VoucherBillTransaction AS VBT WHERE VBT.CurrencyID=@CurrencyID AND VBT.VoucherBillID=TT.VoucherBillID AND  VBT.IsDr=1 AND  CONVERT(DATE, CONVERT(VARCHAR(12),VBT.TransactionDate,106)) BETWEEN CONVERT(DATE, CONVERT(VARCHAR(12),@SessionStartDate,106)) AND DATEADD(DAY,-1,CONVERT(DATE, CONVERT(VARCHAR(12),@OpeningDate,106))) AND ISNULL(VBT.CCID,0)=@SubLedgerID AND VBT.BUID=@BusinessUnitID AND VBT.AccountHeadID=@AccountHeadID AND VBT.ApprovedBy!=0),0),
					Credit_Opening=ISNULL((SELECT SUM(VBT.Amount) FROM View_VoucherBillTransaction AS VBT WHERE VBT.CurrencyID=@CurrencyID AND VBT.VoucherBillID=TT.VoucherBillID AND  VBT.IsDr=0 AND  CONVERT(DATE, CONVERT(VARCHAR(12),VBT.TransactionDate,106)) BETWEEN CONVERT(DATE, CONVERT(VARCHAR(12),@SessionStartDate,106)) AND DATEADD(DAY,-1,CONVERT(DATE, CONVERT(VARCHAR(12),@OpeningDate,106))) AND ISNULL(VBT.CCID,0)=@SubLedgerID AND VBT.BUID=@BusinessUnitID AND VBT.AccountHeadID=@AccountHeadID AND VBT.ApprovedBy!=0),0),
					DebitAmount=ISNULL((SELECT SUM(abs(VBT.Amount)*VBT.ConversionRate) FROM View_VoucherBillTransaction AS VBT WHERE VBT.CurrencyID=@CurrencyID AND VBT.VoucherBillID=TT.VoucherBillID AND  VBT.IsDr=1 AND  CONVERT(DATE, CONVERT(VARCHAR(12),VBT.TransactionDate,106)) BETWEEN CONVERT(DATE, CONVERT(VARCHAR(12),@OpeningDate,106)) AND CONVERT(DATE, CONVERT(VARCHAR(12),@EndDate,106)) AND ISNULL(VBT.CCID,0)=@SubLedgerID AND VBT.BUID=@BusinessUnitID AND VBT.AccountHeadID=@AccountHeadID AND VBT.ApprovedBy!=0),0),
					CreditAmount=ISNULL((SELECT SUM(abs(VBT.Amount)*VBT.ConversionRate) FROM View_VoucherBillTransaction AS VBT WHERE VBT.CurrencyID=@CurrencyID AND VBT.VoucherBillID=TT.VoucherBillID AND  VBT.IsDr=0 AND  CONVERT(DATE, CONVERT(VARCHAR(12),VBT.TransactionDate,106)) BETWEEN CONVERT(DATE, CONVERT(VARCHAR(12),@OpeningDate,106)) AND CONVERT(DATE, CONVERT(VARCHAR(12),@EndDate,106)) AND ISNULL(VBT.CCID,0)=@SubLedgerID AND VBT.BUID=@BusinessUnitID AND VBT.AccountHeadID=@AccountHeadID AND VBT.ApprovedBy!=0),0)
				FROM #TempTableVBill AS TT
			END
			ELSE
			BEGIN
				UPDATE #TempTableVBill
				SET DebitSessionOpening=ISNULL((SELECT SUM(AOB.AmountInCurrency) FROM AccountOpenningBreakdown AS AOB WHERE AOB.IsDr=1 AND AOB.BusinessUnitID=@BusinessUnitID AND AOB.CurrencyID=@CurrencyID AND AOB.CCID=@SubLedgerID AND AOB.AccountHeadID=@AccountHeadID AND AOB.BreakdownObjID=TT.VoucherBillID AND AOB.BreakdownType=2 AND AOB.AccountingSessionID=@SessionID),0),
					CreditSessionOpening=ISNULL((SELECT SUM(AOB.AmountInCurrency) FROM AccountOpenningBreakdown AS AOB WHERE AOB.IsDr=0 AND AOB.BusinessUnitID=@BusinessUnitID AND AOB.CurrencyID=@CurrencyID AND AOB.CCID=@SubLedgerID AND AOB.AccountHeadID=@AccountHeadID AND AOB.BreakdownObjID=TT.VoucherBillID AND AOB.BreakdownType=2 AND AOB.AccountingSessionID=@SessionID),0),
					Debit_Opening=ISNULL((SELECT SUM(VBT.Amount) FROM View_VoucherBillTransaction AS VBT WHERE VBT.CurrencyID=@CurrencyID AND VBT.VoucherBillID=TT.VoucherBillID AND  VBT.IsDr=1 AND  CONVERT(DATE, CONVERT(VARCHAR(12),VBT.TransactionDate,106)) BETWEEN CONVERT(DATE, CONVERT(VARCHAR(12),@SessionStartDate,106)) AND DATEADD(DAY,-1,CONVERT(DATE, CONVERT(VARCHAR(12),@OpeningDate,106))) AND ISNULL(VBT.CCID,0)=@SubLedgerID AND VBT.BUID=@BusinessUnitID AND VBT.AccountHeadID=@AccountHeadID),0),
					Credit_Opening=ISNULL((SELECT SUM(VBT.Amount) FROM View_VoucherBillTransaction AS VBT WHERE VBT.CurrencyID=@CurrencyID AND VBT.VoucherBillID=TT.VoucherBillID AND  VBT.IsDr=0 AND  CONVERT(DATE, CONVERT(VARCHAR(12),VBT.TransactionDate,106)) BETWEEN CONVERT(DATE, CONVERT(VARCHAR(12),@SessionStartDate,106)) AND DATEADD(DAY,-1,CONVERT(DATE, CONVERT(VARCHAR(12),@OpeningDate,106))) AND ISNULL(VBT.CCID,0)=@SubLedgerID AND VBT.BUID=@BusinessUnitID AND VBT.AccountHeadID=@AccountHeadID),0),
					DebitAmount=ISNULL((SELECT SUM(abs(VBT.Amount)*VBT.ConversionRate) FROM View_VoucherBillTransaction AS VBT WHERE VBT.CurrencyID=@CurrencyID AND VBT.VoucherBillID=TT.VoucherBillID AND  VBT.IsDr=1 AND  CONVERT(DATE, CONVERT(VARCHAR(12),VBT.TransactionDate,106)) BETWEEN CONVERT(DATE, CONVERT(VARCHAR(12),@OpeningDate,106)) AND CONVERT(DATE, CONVERT(VARCHAR(12),@EndDate,106)) AND ISNULL(VBT.CCID,0)=@SubLedgerID AND VBT.BUID=@BusinessUnitID AND VBT.AccountHeadID=@AccountHeadID),0),
					CreditAmount=ISNULL((SELECT SUM(abs(VBT.Amount)*VBT.ConversionRate) FROM View_VoucherBillTransaction AS VBT WHERE VBT.CurrencyID=@CurrencyID AND VBT.VoucherBillID=TT.VoucherBillID AND  VBT.IsDr=0 AND  CONVERT(DATE, CONVERT(VARCHAR(12),VBT.TransactionDate,106)) BETWEEN CONVERT(DATE, CONVERT(VARCHAR(12),@OpeningDate,106)) AND CONVERT(DATE, CONVERT(VARCHAR(12),@EndDate,106)) AND ISNULL(VBT.CCID,0)=@SubLedgerID AND VBT.BUID=@BusinessUnitID AND VBT.AccountHeadID=@AccountHeadID),0)
				FROM #TempTableVBill AS TT
			END
		END
	END
	ELSE
	BEGIN
		IF(@BusinessUnitID<=0)
		BEGIN
			IF(@IsApproved=1)
			BEGIN
				UPDATE #TempTableVBill
				SET DebitSessionOpening=ISNULL((SELECT SUM(AOB.AmountInCurrency) FROM AccountOpenningBreakdown AS AOB WHERE AOB.IsDr=1 AND AOB.CurrencyID=@CurrencyID AND AOB.CCID=@SubLedgerID AND AOB.BreakdownObjID=TT.VoucherBillID AND AOB.BreakdownType=2 AND AOB.AccountingSessionID=@SessionID),0),
					CreditSessionOpening=ISNULL((SELECT SUM(AOB.AmountInCurrency) FROM AccountOpenningBreakdown AS AOB WHERE AOB.IsDr=0 AND AOB.CurrencyID=@CurrencyID AND AOB.CCID=@SubLedgerID AND AOB.BreakdownObjID=TT.VoucherBillID AND AOB.BreakdownType=2 AND AOB.AccountingSessionID=@SessionID),0),
					Debit_Opening=ISNULL((SELECT SUM(VBT.Amount) FROM View_VoucherBillTransaction AS VBT WHERE VBT.CurrencyID=@CurrencyID AND VBT.VoucherBillID=TT.VoucherBillID AND  VBT.IsDr=1 AND  CONVERT(DATE, CONVERT(VARCHAR(12),VBT.TransactionDate,106)) BETWEEN CONVERT(DATE, CONVERT(VARCHAR(12),@SessionStartDate,106)) AND DATEADD(DAY,-1,CONVERT(DATE, CONVERT(VARCHAR(12),@OpeningDate,106))) AND ISNULL(VBT.CCID,0)=@SubLedgerID AND VBT.ApprovedBy!=0),0),
					Credit_Opening=ISNULL((SELECT SUM(VBT.Amount) FROM View_VoucherBillTransaction AS VBT WHERE VBT.CurrencyID=@CurrencyID AND VBT.VoucherBillID=TT.VoucherBillID AND  VBT.IsDr=0 AND  CONVERT(DATE, CONVERT(VARCHAR(12),VBT.TransactionDate,106)) BETWEEN CONVERT(DATE, CONVERT(VARCHAR(12),@SessionStartDate,106)) AND DATEADD(DAY,-1,CONVERT(DATE, CONVERT(VARCHAR(12),@OpeningDate,106))) AND ISNULL(VBT.CCID,0)=@SubLedgerID AND VBT.ApprovedBy!=0),0),
					DebitAmount=ISNULL((SELECT SUM(abs(VBT.Amount)*VBT.ConversionRate) FROM View_VoucherBillTransaction AS VBT WHERE VBT.CurrencyID=@CurrencyID AND VBT.VoucherBillID=TT.VoucherBillID AND  VBT.IsDr=1 AND  CONVERT(DATE, CONVERT(VARCHAR(12),VBT.TransactionDate,106)) BETWEEN CONVERT(DATE, CONVERT(VARCHAR(12),@OpeningDate,106)) AND CONVERT(DATE, CONVERT(VARCHAR(12),@EndDate,106)) AND ISNULL(VBT.CCID,0)=@SubLedgerID AND VBT.ApprovedBy!=0),0),
					CreditAmount=ISNULL((SELECT SUM(abs(VBT.Amount)*VBT.ConversionRate) FROM View_VoucherBillTransaction AS VBT WHERE VBT.CurrencyID=@CurrencyID AND VBT.VoucherBillID=TT.VoucherBillID AND  VBT.IsDr=0 AND  CONVERT(DATE, CONVERT(VARCHAR(12),VBT.TransactionDate,106)) BETWEEN CONVERT(DATE, CONVERT(VARCHAR(12),@OpeningDate,106)) AND CONVERT(DATE, CONVERT(VARCHAR(12),@EndDate,106)) AND ISNULL(VBT.CCID,0)=@SubLedgerID AND VBT.ApprovedBy!=0),0)
				FROM #TempTableVBill AS TT
			END
			ELSE
			BEGIN
				UPDATE #TempTableVBill
				SET DebitSessionOpening=ISNULL((SELECT SUM(AOB.AmountInCurrency) FROM AccountOpenningBreakdown AS AOB WHERE AOB.IsDr=1 AND AOB.CurrencyID=@CurrencyID AND AOB.CCID=@SubLedgerID AND AOB.BreakdownObjID=TT.VoucherBillID AND AOB.BreakdownType=2 AND AOB.AccountingSessionID=@SessionID),0),
					CreditSessionOpening=ISNULL((SELECT SUM(AOB.AmountInCurrency) FROM AccountOpenningBreakdown AS AOB WHERE AOB.IsDr=0 AND AOB.CurrencyID=@CurrencyID AND AOB.CCID=@SubLedgerID AND AOB.BreakdownObjID=TT.VoucherBillID AND AOB.BreakdownType=2 AND AOB.AccountingSessionID=@SessionID),0),
					Debit_Opening=ISNULL((SELECT SUM(VBT.Amount) FROM View_VoucherBillTransaction AS VBT WHERE VBT.CurrencyID=@CurrencyID AND VBT.VoucherBillID=TT.VoucherBillID AND  VBT.IsDr=1 AND  CONVERT(DATE, CONVERT(VARCHAR(12),VBT.TransactionDate,106)) BETWEEN CONVERT(DATE, CONVERT(VARCHAR(12),@SessionStartDate,106)) AND DATEADD(DAY,-1,CONVERT(DATE, CONVERT(VARCHAR(12),@OpeningDate,106))) AND ISNULL(VBT.CCID,0)=@SubLedgerID),0),
					Credit_Opening=ISNULL((SELECT SUM(VBT.Amount) FROM View_VoucherBillTransaction AS VBT WHERE VBT.CurrencyID=@CurrencyID AND VBT.VoucherBillID=TT.VoucherBillID AND  VBT.IsDr=0 AND  CONVERT(DATE, CONVERT(VARCHAR(12),VBT.TransactionDate,106)) BETWEEN CONVERT(DATE, CONVERT(VARCHAR(12),@SessionStartDate,106)) AND DATEADD(DAY,-1,CONVERT(DATE, CONVERT(VARCHAR(12),@OpeningDate,106))) AND ISNULL(VBT.CCID,0)=@SubLedgerID),0),
					DebitAmount=ISNULL((SELECT SUM(abs(VBT.Amount)*VBT.ConversionRate) FROM View_VoucherBillTransaction AS VBT WHERE VBT.CurrencyID=@CurrencyID AND VBT.VoucherBillID=TT.VoucherBillID AND  VBT.IsDr=1 AND  CONVERT(DATE, CONVERT(VARCHAR(12),VBT.TransactionDate,106)) BETWEEN CONVERT(DATE, CONVERT(VARCHAR(12),@OpeningDate,106)) AND CONVERT(DATE, CONVERT(VARCHAR(12),@EndDate,106)) AND ISNULL(VBT.CCID,0)=@SubLedgerID),0),
					CreditAmount=ISNULL((SELECT SUM(abs(VBT.Amount)*VBT.ConversionRate) FROM View_VoucherBillTransaction AS VBT WHERE VBT.CurrencyID=@CurrencyID AND VBT.VoucherBillID=TT.VoucherBillID AND  VBT.IsDr=0 AND  CONVERT(DATE, CONVERT(VARCHAR(12),VBT.TransactionDate,106)) BETWEEN CONVERT(DATE, CONVERT(VARCHAR(12),@OpeningDate,106)) AND CONVERT(DATE, CONVERT(VARCHAR(12),@EndDate,106)) AND ISNULL(VBT.CCID,0)=@SubLedgerID),0)
				FROM #TempTableVBill AS TT
			END
		END
		ELSE
		BEGIN
			IF(@IsApproved=1)
			BEGIN
				UPDATE #TempTableVBill
				SET DebitSessionOpening=ISNULL((SELECT SUM(AOB.AmountInCurrency) FROM AccountOpenningBreakdown AS AOB WHERE AOB.IsDr=1 AND AOB.BusinessUnitID=@BusinessUnitID AND AOB.CurrencyID=@CurrencyID AND AOB.CCID=@SubLedgerID AND AOB.BreakdownObjID=TT.VoucherBillID AND AOB.BreakdownType=2 AND AOB.AccountingSessionID=@SessionID),0),
					CreditSessionOpening=ISNULL((SELECT SUM(AOB.AmountInCurrency) FROM AccountOpenningBreakdown AS AOB WHERE AOB.IsDr=0 AND AOB.BusinessUnitID=@BusinessUnitID AND AOB.CurrencyID=@CurrencyID AND AOB.CCID=@SubLedgerID AND AOB.BreakdownObjID=TT.VoucherBillID AND AOB.BreakdownType=2 AND AOB.AccountingSessionID=@SessionID),0),
					Debit_Opening=ISNULL((SELECT SUM(VBT.Amount) FROM View_VoucherBillTransaction AS VBT WHERE VBT.CurrencyID=@CurrencyID AND VBT.VoucherBillID=TT.VoucherBillID AND  VBT.IsDr=1 AND  CONVERT(DATE, CONVERT(VARCHAR(12),VBT.TransactionDate,106)) BETWEEN CONVERT(DATE, CONVERT(VARCHAR(12),@SessionStartDate,106)) AND DATEADD(DAY,-1,CONVERT(DATE, CONVERT(VARCHAR(12),@OpeningDate,106))) AND ISNULL(VBT.CCID,0)=@SubLedgerID AND VBT.BUID=@BusinessUnitID AND VBT.ApprovedBy!=0),0),
					Credit_Opening=ISNULL((SELECT SUM(VBT.Amount) FROM View_VoucherBillTransaction AS VBT WHERE VBT.CurrencyID=@CurrencyID AND VBT.VoucherBillID=TT.VoucherBillID AND  VBT.IsDr=0 AND  CONVERT(DATE, CONVERT(VARCHAR(12),VBT.TransactionDate,106)) BETWEEN CONVERT(DATE, CONVERT(VARCHAR(12),@SessionStartDate,106)) AND DATEADD(DAY,-1,CONVERT(DATE, CONVERT(VARCHAR(12),@OpeningDate,106))) AND ISNULL(VBT.CCID,0)=@SubLedgerID AND VBT.BUID=@BusinessUnitID AND VBT.ApprovedBy!=0),0),
					DebitAmount=ISNULL((SELECT SUM(abs(VBT.Amount)*VBT.ConversionRate) FROM View_VoucherBillTransaction AS VBT WHERE VBT.CurrencyID=@CurrencyID AND VBT.VoucherBillID=TT.VoucherBillID AND  VBT.IsDr=1 AND  CONVERT(DATE, CONVERT(VARCHAR(12),VBT.TransactionDate,106)) BETWEEN CONVERT(DATE, CONVERT(VARCHAR(12),@OpeningDate,106)) AND CONVERT(DATE, CONVERT(VARCHAR(12),@EndDate,106)) AND ISNULL(VBT.CCID,0)=@SubLedgerID AND VBT.BUID=@BusinessUnitID AND VBT.ApprovedBy!=0),0),
					CreditAmount=ISNULL((SELECT SUM(abs(VBT.Amount)*VBT.ConversionRate) FROM View_VoucherBillTransaction AS VBT WHERE VBT.CurrencyID=@CurrencyID AND VBT.VoucherBillID=TT.VoucherBillID AND  VBT.IsDr=0 AND  CONVERT(DATE, CONVERT(VARCHAR(12),VBT.TransactionDate,106)) BETWEEN CONVERT(DATE, CONVERT(VARCHAR(12),@OpeningDate,106)) AND CONVERT(DATE, CONVERT(VARCHAR(12),@EndDate,106)) AND ISNULL(VBT.CCID,0)=@SubLedgerID AND VBT.BUID=@BusinessUnitID AND VBT.ApprovedBy!=0),0)
				FROM #TempTableVBill AS TT
			END
			ELSE
			BEGIN
				UPDATE #TempTableVBill
				SET DebitSessionOpening=ISNULL((SELECT SUM(AOB.AmountInCurrency) FROM AccountOpenningBreakdown AS AOB WHERE AOB.IsDr=1 AND AOB.BusinessUnitID=@BusinessUnitID AND AOB.CurrencyID=@CurrencyID AND AOB.CCID=@SubLedgerID AND AOB.BreakdownObjID=TT.VoucherBillID AND AOB.BreakdownType=2 AND AOB.AccountingSessionID=@SessionID),0),
					CreditSessionOpening=ISNULL((SELECT SUM(AOB.AmountInCurrency) FROM AccountOpenningBreakdown AS AOB WHERE AOB.IsDr=0 AND AOB.BusinessUnitID=@BusinessUnitID AND AOB.CurrencyID=@CurrencyID AND AOB.CCID=@SubLedgerID AND AOB.BreakdownObjID=TT.VoucherBillID AND AOB.BreakdownType=2 AND AOB.AccountingSessionID=@SessionID),0),
					Debit_Opening=ISNULL((SELECT SUM(VBT.Amount) FROM View_VoucherBillTransaction AS VBT WHERE VBT.CurrencyID=@CurrencyID AND VBT.VoucherBillID=TT.VoucherBillID AND  VBT.IsDr=1 AND  CONVERT(DATE, CONVERT(VARCHAR(12),VBT.TransactionDate,106)) BETWEEN CONVERT(DATE, CONVERT(VARCHAR(12),@SessionStartDate,106)) AND DATEADD(DAY,-1,CONVERT(DATE, CONVERT(VARCHAR(12),@OpeningDate,106))) AND ISNULL(VBT.CCID,0)=@SubLedgerID AND VBT.BUID=@BusinessUnitID),0),
					Credit_Opening=ISNULL((SELECT SUM(VBT.Amount) FROM View_VoucherBillTransaction AS VBT WHERE VBT.CurrencyID=@CurrencyID AND VBT.VoucherBillID=TT.VoucherBillID AND  VBT.IsDr=0 AND  CONVERT(DATE, CONVERT(VARCHAR(12),VBT.TransactionDate,106)) BETWEEN CONVERT(DATE, CONVERT(VARCHAR(12),@SessionStartDate,106)) AND DATEADD(DAY,-1,CONVERT(DATE, CONVERT(VARCHAR(12),@OpeningDate,106))) AND ISNULL(VBT.CCID,0)=@SubLedgerID AND VBT.BUID=@BusinessUnitID),0),
					DebitAmount=ISNULL((SELECT SUM(abs(VBT.Amount)*VBT.ConversionRate) FROM View_VoucherBillTransaction AS VBT WHERE VBT.CurrencyID=@CurrencyID AND VBT.VoucherBillID=TT.VoucherBillID AND  VBT.IsDr=1 AND  CONVERT(DATE, CONVERT(VARCHAR(12),VBT.TransactionDate,106)) BETWEEN CONVERT(DATE, CONVERT(VARCHAR(12),@OpeningDate,106)) AND CONVERT(DATE, CONVERT(VARCHAR(12),@EndDate,106)) AND ISNULL(VBT.CCID,0)=@SubLedgerID AND VBT.BUID=@BusinessUnitID),0),
					CreditAmount=ISNULL((SELECT SUM(abs(VBT.Amount)*VBT.ConversionRate) FROM View_VoucherBillTransaction AS VBT WHERE VBT.CurrencyID=@CurrencyID AND VBT.VoucherBillID=TT.VoucherBillID AND  VBT.IsDr=0 AND  CONVERT(DATE, CONVERT(VARCHAR(12),VBT.TransactionDate,106)) BETWEEN CONVERT(DATE, CONVERT(VARCHAR(12),@OpeningDate,106)) AND CONVERT(DATE, CONVERT(VARCHAR(12),@EndDate,106)) AND ISNULL(VBT.CCID,0)=@SubLedgerID AND VBT.BUID=@BusinessUnitID),0)
				FROM #TempTableVBill AS TT
			END
		END
	END
END
ELSE
BEGIN
	IF(@AccountHeadID>0)
	BEGIN
		IF(@BusinessUnitID<=0)
		BEGIN
			IF(@IsApproved=1)
			BEGIN
				UPDATE #TempTableVBill
				SET DebitSessionOpening=ISNULL((SELECT SUM(AOB.OpenningBalance) FROM AccountOpenningBreakdown AS AOB WHERE AOB.IsDr=1 AND  AOB.CCID=@SubLedgerID AND AOB.AccountHeadID=@AccountHeadID AND AOB.BreakdownObjID=TT.VoucherBillID AND AOB.BreakdownType=2 AND AOB.AccountingSessionID=@SessionID),0),
					CreditSessionOpening=ISNULL((SELECT SUM(AOB.OpenningBalance) FROM AccountOpenningBreakdown AS AOB WHERE AOB.IsDr=0 AND  AOB.CCID=@SubLedgerID AND AOB.AccountHeadID=@AccountHeadID AND AOB.BreakdownObjID=TT.VoucherBillID AND AOB.BreakdownType=2 AND AOB.AccountingSessionID=@SessionID),0),
					Debit_Opening=ISNULL((SELECT SUM(VBT.Amount*VBT.ConversionRate) FROM View_VoucherBillTransaction AS VBT WHERE  VBT.VoucherBillID=TT.VoucherBillID AND  VBT.IsDr=1 AND  CONVERT(DATE, CONVERT(VARCHAR(12),VBT.TransactionDate,106)) BETWEEN CONVERT(DATE, CONVERT(VARCHAR(12),@SessionStartDate,106)) AND DATEADD(DAY,-1,CONVERT(DATE, CONVERT(VARCHAR(12),@OpeningDate,106))) AND ISNULL(VBT.CCID,0)=@SubLedgerID AND VBT.AccountHeadID=@AccountHeadID AND VBT.ApprovedBy!=0),0),
					Credit_Opening=ISNULL((SELECT SUM(VBT.Amount*VBT.ConversionRate) FROM View_VoucherBillTransaction AS VBT WHERE  VBT.VoucherBillID=TT.VoucherBillID AND  VBT.IsDr=0 AND  CONVERT(DATE, CONVERT(VARCHAR(12),VBT.TransactionDate,106)) BETWEEN CONVERT(DATE, CONVERT(VARCHAR(12),@SessionStartDate,106)) AND DATEADD(DAY,-1,CONVERT(DATE, CONVERT(VARCHAR(12),@OpeningDate,106))) AND ISNULL(VBT.CCID,0)=@SubLedgerID AND VBT.AccountHeadID=@AccountHeadID AND VBT.ApprovedBy!=0),0),
					DebitAmount=ISNULL((SELECT SUM(abs(VBT.Amount)*VBT.ConversionRate) FROM View_VoucherBillTransaction AS VBT WHERE  VBT.VoucherBillID=TT.VoucherBillID AND  VBT.IsDr=1 AND  CONVERT(DATE, CONVERT(VARCHAR(12),VBT.TransactionDate,106)) BETWEEN CONVERT(DATE, CONVERT(VARCHAR(12),@OpeningDate,106)) AND CONVERT(DATE, CONVERT(VARCHAR(12),@EndDate,106)) AND ISNULL(VBT.CCID,0)=@SubLedgerID AND VBT.AccountHeadID=@AccountHeadID AND VBT.ApprovedBy!=0),0),
					CreditAmount=ISNULL((SELECT SUM(abs(VBT.Amount)*VBT.ConversionRate) FROM View_VoucherBillTransaction AS VBT WHERE  VBT.VoucherBillID=TT.VoucherBillID AND  VBT.IsDr=0 AND  CONVERT(DATE, CONVERT(VARCHAR(12),VBT.TransactionDate,106)) BETWEEN CONVERT(DATE, CONVERT(VARCHAR(12),@OpeningDate,106)) AND CONVERT(DATE, CONVERT(VARCHAR(12),@EndDate,106)) AND ISNULL(VBT.CCID,0)=@SubLedgerID AND VBT.AccountHeadID=@AccountHeadID AND VBT.ApprovedBy!=0),0)
				FROM #TempTableVBill AS TT
			END
			ELSE
			BEGIN
				UPDATE #TempTableVBill
				SET DebitSessionOpening=ISNULL((SELECT SUM(AOB.OpenningBalance) FROM AccountOpenningBreakdown AS AOB WHERE AOB.IsDr=1 AND  AOB.CCID=@SubLedgerID AND AOB.AccountHeadID=@AccountHeadID AND AOB.BreakdownObjID=TT.VoucherBillID AND AOB.BreakdownType=2 AND AOB.AccountingSessionID=@SessionID),0),
					CreditSessionOpening=ISNULL((SELECT SUM(AOB.OpenningBalance) FROM AccountOpenningBreakdown AS AOB WHERE AOB.IsDr=0 AND  AOB.CCID=@SubLedgerID AND AOB.AccountHeadID=@AccountHeadID AND AOB.BreakdownObjID=TT.VoucherBillID AND AOB.BreakdownType=2 AND AOB.AccountingSessionID=@SessionID),0),
					Debit_Opening=ISNULL((SELECT SUM(VBT.Amount*VBT.ConversionRate) FROM View_VoucherBillTransaction AS VBT WHERE  VBT.VoucherBillID=TT.VoucherBillID AND  VBT.IsDr=1 AND  CONVERT(DATE, CONVERT(VARCHAR(12),VBT.TransactionDate,106)) BETWEEN CONVERT(DATE, CONVERT(VARCHAR(12),@SessionStartDate,106)) AND DATEADD(DAY,-1,CONVERT(DATE, CONVERT(VARCHAR(12),@OpeningDate,106))) AND ISNULL(VBT.CCID,0)=@SubLedgerID AND VBT.AccountHeadID=@AccountHeadID),0),
					Credit_Opening=ISNULL((SELECT SUM(VBT.Amount*VBT.ConversionRate) FROM View_VoucherBillTransaction AS VBT WHERE  VBT.VoucherBillID=TT.VoucherBillID AND  VBT.IsDr=0 AND  CONVERT(DATE, CONVERT(VARCHAR(12),VBT.TransactionDate,106)) BETWEEN CONVERT(DATE, CONVERT(VARCHAR(12),@SessionStartDate,106)) AND DATEADD(DAY,-1,CONVERT(DATE, CONVERT(VARCHAR(12),@OpeningDate,106))) AND ISNULL(VBT.CCID,0)=@SubLedgerID AND VBT.AccountHeadID=@AccountHeadID),0),
					DebitAmount=ISNULL((SELECT SUM(abs(VBT.Amount)*VBT.ConversionRate) FROM View_VoucherBillTransaction AS VBT WHERE  VBT.VoucherBillID=TT.VoucherBillID AND  VBT.IsDr=1 AND  CONVERT(DATE, CONVERT(VARCHAR(12),VBT.TransactionDate,106)) BETWEEN CONVERT(DATE, CONVERT(VARCHAR(12),@OpeningDate,106)) AND CONVERT(DATE, CONVERT(VARCHAR(12),@EndDate,106)) AND ISNULL(VBT.CCID,0)=@SubLedgerID AND VBT.AccountHeadID=@AccountHeadID),0),
					CreditAmount=ISNULL((SELECT SUM(abs(VBT.Amount)*VBT.ConversionRate) FROM View_VoucherBillTransaction AS VBT WHERE  VBT.VoucherBillID=TT.VoucherBillID AND  VBT.IsDr=0 AND  CONVERT(DATE, CONVERT(VARCHAR(12),VBT.TransactionDate,106)) BETWEEN CONVERT(DATE, CONVERT(VARCHAR(12),@OpeningDate,106)) AND CONVERT(DATE, CONVERT(VARCHAR(12),@EndDate,106)) AND ISNULL(VBT.CCID,0)=@SubLedgerID AND VBT.AccountHeadID=@AccountHeadID),0)
				FROM #TempTableVBill AS TT
			END
		END
		ELSE
		BEGIN
			IF(@IsApproved=1)
			BEGIN
				UPDATE #TempTableVBill
				SET DebitSessionOpening=ISNULL((SELECT SUM(AOB.OpenningBalance) FROM AccountOpenningBreakdown AS AOB WHERE AOB.IsDr=1 AND AOB.BusinessUnitID=@BusinessUnitID AND  AOB.CCID=@SubLedgerID AND AOB.AccountHeadID=@AccountHeadID AND AOB.BreakdownObjID=TT.VoucherBillID AND AOB.BreakdownType=2 AND AOB.AccountingSessionID=@SessionID),0),
					CreditSessionOpening=ISNULL((SELECT SUM(AOB.OpenningBalance) FROM AccountOpenningBreakdown AS AOB WHERE AOB.IsDr=0 AND AOB.BusinessUnitID=@BusinessUnitID AND  AOB.CCID=@SubLedgerID AND AOB.AccountHeadID=@AccountHeadID AND AOB.BreakdownObjID=TT.VoucherBillID AND AOB.BreakdownType=2 AND AOB.AccountingSessionID=@SessionID),0),
					Debit_Opening=ISNULL((SELECT SUM(VBT.Amount*VBT.ConversionRate) FROM View_VoucherBillTransaction AS VBT WHERE  VBT.VoucherBillID=TT.VoucherBillID AND  VBT.IsDr=1 AND  CONVERT(DATE, CONVERT(VARCHAR(12),VBT.TransactionDate,106)) BETWEEN CONVERT(DATE, CONVERT(VARCHAR(12),@SessionStartDate,106)) AND DATEADD(DAY,-1,CONVERT(DATE, CONVERT(VARCHAR(12),@OpeningDate,106))) AND ISNULL(VBT.CCID,0)=@SubLedgerID AND VBT.BUID=@BusinessUnitID AND VBT.AccountHeadID=@AccountHeadID AND VBT.ApprovedBy!=0),0),
					Credit_Opening=ISNULL((SELECT SUM(VBT.Amount*VBT.ConversionRate) FROM View_VoucherBillTransaction AS VBT WHERE  VBT.VoucherBillID=TT.VoucherBillID AND  VBT.IsDr=0 AND  CONVERT(DATE, CONVERT(VARCHAR(12),VBT.TransactionDate,106)) BETWEEN CONVERT(DATE, CONVERT(VARCHAR(12),@SessionStartDate,106)) AND DATEADD(DAY,-1,CONVERT(DATE, CONVERT(VARCHAR(12),@OpeningDate,106))) AND ISNULL(VBT.CCID,0)=@SubLedgerID AND VBT.BUID=@BusinessUnitID AND VBT.AccountHeadID=@AccountHeadID AND VBT.ApprovedBy!=0),0),
					DebitAmount=ISNULL((SELECT SUM(abs(VBT.Amount)*VBT.ConversionRate) FROM View_VoucherBillTransaction AS VBT WHERE  VBT.VoucherBillID=TT.VoucherBillID AND  VBT.IsDr=1 AND  CONVERT(DATE, CONVERT(VARCHAR(12),VBT.TransactionDate,106)) BETWEEN CONVERT(DATE, CONVERT(VARCHAR(12),@OpeningDate,106)) AND CONVERT(DATE, CONVERT(VARCHAR(12),@EndDate,106)) AND ISNULL(VBT.CCID,0)=@SubLedgerID AND VBT.BUID=@BusinessUnitID AND VBT.AccountHeadID=@AccountHeadID AND VBT.ApprovedBy!=0),0),
					CreditAmount=ISNULL((SELECT SUM(abs(VBT.Amount)*VBT.ConversionRate) FROM View_VoucherBillTransaction AS VBT WHERE  VBT.VoucherBillID=TT.VoucherBillID AND  VBT.IsDr=0 AND  CONVERT(DATE, CONVERT(VARCHAR(12),VBT.TransactionDate,106)) BETWEEN CONVERT(DATE, CONVERT(VARCHAR(12),@OpeningDate,106)) AND CONVERT(DATE, CONVERT(VARCHAR(12),@EndDate,106)) AND ISNULL(VBT.CCID,0)=@SubLedgerID AND VBT.BUID=@BusinessUnitID AND VBT.AccountHeadID=@AccountHeadID AND VBT.ApprovedBy!=0),0)
				FROM #TempTableVBill AS TT
			END
			ELSE
			BEGIN
				UPDATE #TempTableVBill
				SET DebitSessionOpening=ISNULL((SELECT SUM(AOB.OpenningBalance) FROM AccountOpenningBreakdown AS AOB WHERE AOB.IsDr=1 AND AOB.BusinessUnitID=@BusinessUnitID AND  AOB.CCID=@SubLedgerID AND AOB.AccountHeadID=@AccountHeadID AND AOB.BreakdownObjID=TT.VoucherBillID AND AOB.BreakdownType=2 AND AOB.AccountingSessionID=@SessionID),0),
					CreditSessionOpening=ISNULL((SELECT SUM(AOB.OpenningBalance) FROM AccountOpenningBreakdown AS AOB WHERE AOB.IsDr=0 AND AOB.BusinessUnitID=@BusinessUnitID AND  AOB.CCID=@SubLedgerID AND AOB.AccountHeadID=@AccountHeadID AND AOB.BreakdownObjID=TT.VoucherBillID AND AOB.BreakdownType=2 AND AOB.AccountingSessionID=@SessionID),0),
					Debit_Opening=ISNULL((SELECT SUM(VBT.Amount*VBT.ConversionRate) FROM View_VoucherBillTransaction AS VBT WHERE  VBT.VoucherBillID=TT.VoucherBillID AND  VBT.IsDr=1 AND  CONVERT(DATE, CONVERT(VARCHAR(12),VBT.TransactionDate,106)) BETWEEN CONVERT(DATE, CONVERT(VARCHAR(12),@SessionStartDate,106)) AND DATEADD(DAY,-1,CONVERT(DATE, CONVERT(VARCHAR(12),@OpeningDate,106))) AND ISNULL(VBT.CCID,0)=@SubLedgerID AND VBT.BUID=@BusinessUnitID AND VBT.AccountHeadID=@AccountHeadID),0),
					Credit_Opening=ISNULL((SELECT SUM(VBT.Amount*VBT.ConversionRate) FROM View_VoucherBillTransaction AS VBT WHERE  VBT.VoucherBillID=TT.VoucherBillID AND  VBT.IsDr=0 AND  CONVERT(DATE, CONVERT(VARCHAR(12),VBT.TransactionDate,106)) BETWEEN CONVERT(DATE, CONVERT(VARCHAR(12),@SessionStartDate,106)) AND DATEADD(DAY,-1,CONVERT(DATE, CONVERT(VARCHAR(12),@OpeningDate,106))) AND ISNULL(VBT.CCID,0)=@SubLedgerID AND VBT.BUID=@BusinessUnitID AND VBT.AccountHeadID=@AccountHeadID),0),
					DebitAmount=ISNULL((SELECT SUM(abs(VBT.Amount)*VBT.ConversionRate) FROM View_VoucherBillTransaction AS VBT WHERE  VBT.VoucherBillID=TT.VoucherBillID AND  VBT.IsDr=1 AND  CONVERT(DATE, CONVERT(VARCHAR(12),VBT.TransactionDate,106)) BETWEEN CONVERT(DATE, CONVERT(VARCHAR(12),@OpeningDate,106)) AND CONVERT(DATE, CONVERT(VARCHAR(12),@EndDate,106)) AND ISNULL(VBT.CCID,0)=@SubLedgerID AND VBT.BUID=@BusinessUnitID AND VBT.AccountHeadID=@AccountHeadID),0),
					CreditAmount=ISNULL((SELECT SUM(abs(VBT.Amount)*VBT.ConversionRate) FROM View_VoucherBillTransaction AS VBT WHERE  VBT.VoucherBillID=TT.VoucherBillID AND  VBT.IsDr=0 AND  CONVERT(DATE, CONVERT(VARCHAR(12),VBT.TransactionDate,106)) BETWEEN CONVERT(DATE, CONVERT(VARCHAR(12),@OpeningDate,106)) AND CONVERT(DATE, CONVERT(VARCHAR(12),@EndDate,106)) AND ISNULL(VBT.CCID,0)=@SubLedgerID AND VBT.BUID=@BusinessUnitID AND VBT.AccountHeadID=@AccountHeadID),0)
				FROM #TempTableVBill AS TT
			END
		END
	END
	ELSE
	BEGIN
		IF(@BusinessUnitID<=0)
		BEGIN
			IF(@IsApproved=1)
			BEGIN
				UPDATE #TempTableVBill
				SET DebitSessionOpening=ISNULL((SELECT SUM(AOB.OpenningBalance) FROM AccountOpenningBreakdown AS AOB WHERE AOB.IsDr=1 AND  AOB.CCID=@SubLedgerID AND AOB.BreakdownObjID=TT.VoucherBillID AND AOB.BreakdownType=2 AND AOB.AccountingSessionID=@SessionID),0),
					CreditSessionOpening=ISNULL((SELECT SUM(AOB.OpenningBalance) FROM AccountOpenningBreakdown AS AOB WHERE AOB.IsDr=0 AND  AOB.CCID=@SubLedgerID AND AOB.BreakdownObjID=TT.VoucherBillID AND AOB.BreakdownType=2 AND AOB.AccountingSessionID=@SessionID),0),
					Debit_Opening=ISNULL((SELECT SUM(VBT.Amount*VBT.ConversionRate) FROM View_VoucherBillTransaction AS VBT WHERE  VBT.VoucherBillID=TT.VoucherBillID AND  VBT.IsDr=1 AND  CONVERT(DATE, CONVERT(VARCHAR(12),VBT.TransactionDate,106)) BETWEEN CONVERT(DATE, CONVERT(VARCHAR(12),@SessionStartDate,106)) AND DATEADD(DAY,-1,CONVERT(DATE, CONVERT(VARCHAR(12),@OpeningDate,106))) AND ISNULL(VBT.CCID,0)=@SubLedgerID AND VBT.ApprovedBy!=0),0),
					Credit_Opening=ISNULL((SELECT SUM(VBT.Amount*VBT.ConversionRate) FROM View_VoucherBillTransaction AS VBT WHERE  VBT.VoucherBillID=TT.VoucherBillID AND  VBT.IsDr=0 AND  CONVERT(DATE, CONVERT(VARCHAR(12),VBT.TransactionDate,106)) BETWEEN CONVERT(DATE, CONVERT(VARCHAR(12),@SessionStartDate,106)) AND DATEADD(DAY,-1,CONVERT(DATE, CONVERT(VARCHAR(12),@OpeningDate,106))) AND ISNULL(VBT.CCID,0)=@SubLedgerID AND VBT.ApprovedBy!=0),0),
					DebitAmount=ISNULL((SELECT SUM(abs(VBT.Amount)*VBT.ConversionRate) FROM View_VoucherBillTransaction AS VBT WHERE  VBT.VoucherBillID=TT.VoucherBillID AND  VBT.IsDr=1 AND  CONVERT(DATE, CONVERT(VARCHAR(12),VBT.TransactionDate,106)) BETWEEN CONVERT(DATE, CONVERT(VARCHAR(12),@OpeningDate,106)) AND CONVERT(DATE, CONVERT(VARCHAR(12),@EndDate,106)) AND ISNULL(VBT.CCID,0)=@SubLedgerID AND VBT.ApprovedBy!=0),0),
					CreditAmount=ISNULL((SELECT SUM(abs(VBT.Amount)*VBT.ConversionRate) FROM View_VoucherBillTransaction AS VBT WHERE  VBT.VoucherBillID=TT.VoucherBillID AND  VBT.IsDr=0 AND  CONVERT(DATE, CONVERT(VARCHAR(12),VBT.TransactionDate,106)) BETWEEN CONVERT(DATE, CONVERT(VARCHAR(12),@OpeningDate,106)) AND CONVERT(DATE, CONVERT(VARCHAR(12),@EndDate,106)) AND ISNULL(VBT.CCID,0)=@SubLedgerID AND VBT.ApprovedBy!=0),0)
				FROM #TempTableVBill AS TT
			END
			ELSE
			BEGIN
				UPDATE #TempTableVBill
				SET DebitSessionOpening=ISNULL((SELECT SUM(AOB.OpenningBalance) FROM AccountOpenningBreakdown AS AOB WHERE AOB.IsDr=1 AND  AOB.CCID=@SubLedgerID AND AOB.BreakdownObjID=TT.VoucherBillID AND AOB.BreakdownType=2 AND AOB.AccountingSessionID=@SessionID),0),
					CreditSessionOpening=ISNULL((SELECT SUM(AOB.OpenningBalance) FROM AccountOpenningBreakdown AS AOB WHERE AOB.IsDr=0 AND  AOB.CCID=@SubLedgerID AND AOB.BreakdownObjID=TT.VoucherBillID AND AOB.BreakdownType=2 AND AOB.AccountingSessionID=@SessionID),0),
					Debit_Opening=ISNULL((SELECT SUM(VBT.Amount*VBT.ConversionRate) FROM View_VoucherBillTransaction AS VBT WHERE  VBT.VoucherBillID=TT.VoucherBillID AND  VBT.IsDr=1 AND  CONVERT(DATE, CONVERT(VARCHAR(12),VBT.TransactionDate,106)) BETWEEN CONVERT(DATE, CONVERT(VARCHAR(12),@SessionStartDate,106)) AND DATEADD(DAY,-1,CONVERT(DATE, CONVERT(VARCHAR(12),@OpeningDate,106))) AND ISNULL(VBT.CCID,0)=@SubLedgerID),0),
					Credit_Opening=ISNULL((SELECT SUM(VBT.Amount*VBT.ConversionRate) FROM View_VoucherBillTransaction AS VBT WHERE  VBT.VoucherBillID=TT.VoucherBillID AND  VBT.IsDr=0 AND  CONVERT(DATE, CONVERT(VARCHAR(12),VBT.TransactionDate,106)) BETWEEN CONVERT(DATE, CONVERT(VARCHAR(12),@SessionStartDate,106)) AND DATEADD(DAY,-1,CONVERT(DATE, CONVERT(VARCHAR(12),@OpeningDate,106))) AND ISNULL(VBT.CCID,0)=@SubLedgerID),0),
					DebitAmount=ISNULL((SELECT SUM(abs(VBT.Amount)*VBT.ConversionRate) FROM View_VoucherBillTransaction AS VBT WHERE  VBT.VoucherBillID=TT.VoucherBillID AND  VBT.IsDr=1 AND  CONVERT(DATE, CONVERT(VARCHAR(12),VBT.TransactionDate,106)) BETWEEN CONVERT(DATE, CONVERT(VARCHAR(12),@OpeningDate,106)) AND CONVERT(DATE, CONVERT(VARCHAR(12),@EndDate,106)) AND ISNULL(VBT.CCID,0)=@SubLedgerID),0),
					CreditAmount=ISNULL((SELECT SUM(abs(VBT.Amount)*VBT.ConversionRate) FROM View_VoucherBillTransaction AS VBT WHERE  VBT.VoucherBillID=TT.VoucherBillID AND  VBT.IsDr=0 AND  CONVERT(DATE, CONVERT(VARCHAR(12),VBT.TransactionDate,106)) BETWEEN CONVERT(DATE, CONVERT(VARCHAR(12),@OpeningDate,106)) AND CONVERT(DATE, CONVERT(VARCHAR(12),@EndDate,106)) AND ISNULL(VBT.CCID,0)=@SubLedgerID),0)
				FROM #TempTableVBill AS TT
			END
		END
		ELSE
		BEGIN
			IF(@IsApproved=1)
			BEGIN
				UPDATE #TempTableVBill
				SET DebitSessionOpening=ISNULL((SELECT SUM(AOB.OpenningBalance) FROM AccountOpenningBreakdown AS AOB WHERE AOB.IsDr=1 AND AOB.BusinessUnitID=@BusinessUnitID AND  AOB.CCID=@SubLedgerID AND AOB.BreakdownObjID=TT.VoucherBillID AND AOB.BreakdownType=2 AND AOB.AccountingSessionID=@SessionID),0),
					CreditSessionOpening=ISNULL((SELECT SUM(AOB.OpenningBalance) FROM AccountOpenningBreakdown AS AOB WHERE AOB.IsDr=0 AND AOB.BusinessUnitID=@BusinessUnitID AND  AOB.CCID=@SubLedgerID AND AOB.BreakdownObjID=TT.VoucherBillID AND AOB.BreakdownType=2 AND AOB.AccountingSessionID=@SessionID),0),
					Debit_Opening=ISNULL((SELECT SUM(VBT.Amount*VBT.ConversionRate) FROM View_VoucherBillTransaction AS VBT WHERE  VBT.VoucherBillID=TT.VoucherBillID AND  VBT.IsDr=1 AND  CONVERT(DATE, CONVERT(VARCHAR(12),VBT.TransactionDate,106)) BETWEEN CONVERT(DATE, CONVERT(VARCHAR(12),@SessionStartDate,106)) AND DATEADD(DAY,-1,CONVERT(DATE, CONVERT(VARCHAR(12),@OpeningDate,106))) AND ISNULL(VBT.CCID,0)=@SubLedgerID AND VBT.BUID=@BusinessUnitID AND VBT.ApprovedBy!=0),0),
					Credit_Opening=ISNULL((SELECT SUM(VBT.Amount*VBT.ConversionRate) FROM View_VoucherBillTransaction AS VBT WHERE  VBT.VoucherBillID=TT.VoucherBillID AND  VBT.IsDr=0 AND  CONVERT(DATE, CONVERT(VARCHAR(12),VBT.TransactionDate,106)) BETWEEN CONVERT(DATE, CONVERT(VARCHAR(12),@SessionStartDate,106)) AND DATEADD(DAY,-1,CONVERT(DATE, CONVERT(VARCHAR(12),@OpeningDate,106))) AND ISNULL(VBT.CCID,0)=@SubLedgerID AND VBT.BUID=@BusinessUnitID AND VBT.ApprovedBy!=0),0),
					DebitAmount=ISNULL((SELECT SUM(abs(VBT.Amount)*VBT.ConversionRate) FROM View_VoucherBillTransaction AS VBT WHERE  VBT.VoucherBillID=TT.VoucherBillID AND  VBT.IsDr=1 AND  CONVERT(DATE, CONVERT(VARCHAR(12),VBT.TransactionDate,106)) BETWEEN CONVERT(DATE, CONVERT(VARCHAR(12),@OpeningDate,106)) AND CONVERT(DATE, CONVERT(VARCHAR(12),@EndDate,106)) AND ISNULL(VBT.CCID,0)=@SubLedgerID AND VBT.BUID=@BusinessUnitID AND VBT.ApprovedBy!=0),0),
					CreditAmount=ISNULL((SELECT SUM(abs(VBT.Amount)*VBT.ConversionRate) FROM View_VoucherBillTransaction AS VBT WHERE  VBT.VoucherBillID=TT.VoucherBillID AND  VBT.IsDr=0 AND  CONVERT(DATE, CONVERT(VARCHAR(12),VBT.TransactionDate,106)) BETWEEN CONVERT(DATE, CONVERT(VARCHAR(12),@OpeningDate,106)) AND CONVERT(DATE, CONVERT(VARCHAR(12),@EndDate,106)) AND ISNULL(VBT.CCID,0)=@SubLedgerID AND VBT.BUID=@BusinessUnitID AND VBT.ApprovedBy!=0),0)
				FROM #TempTableVBill AS TT
			END
			ELSE
			BEGIN
				UPDATE #TempTableVBill
				SET DebitSessionOpening=ISNULL((SELECT SUM(AOB.OpenningBalance) FROM AccountOpenningBreakdown AS AOB WHERE AOB.IsDr=1 AND AOB.BusinessUnitID=@BusinessUnitID AND  AOB.CCID=@SubLedgerID AND AOB.BreakdownObjID=TT.VoucherBillID AND AOB.BreakdownType=2 AND AOB.AccountingSessionID=@SessionID),0),
					CreditSessionOpening=ISNULL((SELECT SUM(AOB.OpenningBalance) FROM AccountOpenningBreakdown AS AOB WHERE AOB.IsDr=0 AND AOB.BusinessUnitID=@BusinessUnitID AND  AOB.CCID=@SubLedgerID AND AOB.BreakdownObjID=TT.VoucherBillID AND AOB.BreakdownType=2 AND AOB.AccountingSessionID=@SessionID),0),
					Debit_Opening=ISNULL((SELECT SUM(VBT.Amount*VBT.ConversionRate) FROM View_VoucherBillTransaction AS VBT WHERE  VBT.VoucherBillID=TT.VoucherBillID AND  VBT.IsDr=1 AND  CONVERT(DATE, CONVERT(VARCHAR(12),VBT.TransactionDate,106)) BETWEEN CONVERT(DATE, CONVERT(VARCHAR(12),@SessionStartDate,106)) AND DATEADD(DAY,-1,CONVERT(DATE, CONVERT(VARCHAR(12),@OpeningDate,106))) AND ISNULL(VBT.CCID,0)=@SubLedgerID AND VBT.BUID=@BusinessUnitID),0),
					Credit_Opening=ISNULL((SELECT SUM(VBT.Amount*VBT.ConversionRate) FROM View_VoucherBillTransaction AS VBT WHERE  VBT.VoucherBillID=TT.VoucherBillID AND  VBT.IsDr=0 AND  CONVERT(DATE, CONVERT(VARCHAR(12),VBT.TransactionDate,106)) BETWEEN CONVERT(DATE, CONVERT(VARCHAR(12),@SessionStartDate,106)) AND DATEADD(DAY,-1,CONVERT(DATE, CONVERT(VARCHAR(12),@OpeningDate,106))) AND ISNULL(VBT.CCID,0)=@SubLedgerID AND VBT.BUID=@BusinessUnitID),0),
					DebitAmount=ISNULL((SELECT SUM(abs(VBT.Amount)*VBT.ConversionRate) FROM View_VoucherBillTransaction AS VBT WHERE  VBT.VoucherBillID=TT.VoucherBillID AND  VBT.IsDr=1 AND  CONVERT(DATE, CONVERT(VARCHAR(12),VBT.TransactionDate,106)) BETWEEN CONVERT(DATE, CONVERT(VARCHAR(12),@OpeningDate,106)) AND CONVERT(DATE, CONVERT(VARCHAR(12),@EndDate,106)) AND ISNULL(VBT.CCID,0)=@SubLedgerID AND VBT.BUID=@BusinessUnitID),0),
					CreditAmount=ISNULL((SELECT SUM(abs(VBT.Amount)*VBT.ConversionRate) FROM View_VoucherBillTransaction AS VBT WHERE  VBT.VoucherBillID=TT.VoucherBillID AND  VBT.IsDr=0 AND  CONVERT(DATE, CONVERT(VARCHAR(12),VBT.TransactionDate,106)) BETWEEN CONVERT(DATE, CONVERT(VARCHAR(12),@OpeningDate,106)) AND CONVERT(DATE, CONVERT(VARCHAR(12),@EndDate,106)) AND ISNULL(VBT.CCID,0)=@SubLedgerID AND VBT.BUID=@BusinessUnitID),0)
				FROM #TempTableVBill AS TT
			END
		END
	END
END
UPDATE #TempTableVBill
SET OpeningBalance=TT.DebitSessionOpening-TT.CreditSessionOpening+TT.Debit_Opening-TT.Credit_Opening
FROM #TempTableVBill AS TT WHERE TT.ComponentType IN (2,6)

UPDATE #TempTableVBill
SET OpeningBalance=TT.CreditSessionOpening-TT.DebitSessionOpening-TT.Debit_Opening+TT.Credit_Opening
FROM #TempTableVBill AS TT WHERE TT.ComponentType NOT IN (2,6)

UPDATE #TempTableVBill
SET IsDebit=CASE WHEN TT.OpeningBalance >=0 THEN 1 ELSE 0 END
FROM #TempTableVBill AS TT WHERE TT.ComponentType IN (2,6)


UPDATE #TempTableVBill
SET IsDebit=CASE WHEN TT.OpeningBalance >=0 THEN 0 ELSE 1 END
FROM #TempTableVBill AS TT WHERE TT.ComponentType NOT IN (2,6)


---------------------------------
-----------Calculate Closing Balance
UPDATE #TempTableVBill
SET ClosingBalance=TT.OpeningBalance+TT.DebitAmount-TT.CreditAmount
FROM #TempTableVBill AS TT WHERE TT.ComponentType IN (2,6)

UPDATE #TempTableVBill
SET ClosingBalance=TT.OpeningBalance-TT.DebitAmount+TT.CreditAmount
FROM #TempTableVBill AS TT WHERE TT.ComponentType not IN (2,6)


SELECT * FROM #TempTableVBill
DROP TABLE #TempTableVBill 
COMMIT TRAN


GO
/****** Object:  StoredProcedure [dbo].[SP_VoucherBillLedger]    Script Date: 12/1/2016 2:17:46 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[SP_VoucherBillLedger]
(
	@VoucherBillID as int,
	@OpeningDate as date,
	@EndDate as date,
	@IsApproved as bit,
	@CurrencyID as int,
	@BusinessUnitID as int
)
AS
BEGIN TRAN
--DECLARE
--@VoucherBillID as int,
--@OpeningDate as date,
--@EndDate as date,
--@IsApproved as bit,
--@CurrencyID as int,
--@BusinessUnitID as int

----EXEC [SP_VoucherBillLedger] 2191, '01 Jan 2016', '02 Oct 2016', 1, 0, 1

--SET @VoucherBillID=2191
--SET @OpeningDate='01 Jan 2016'
--set @EndDate='02 Oct 2016'
--set @IsApproved=1
--set @CurrencyID=0
--set @BusinessUnitID=1

DECLARE
@SessionID as int,
@SessionStartDate as date,
@Debit_OpeningAmount as decimal(30,17),
@Credit_OpeningAmount as decimal(30,17),
@DebitOpenningBalance as decimal(30,17),
@CreditOpenningBalance as decimal(30,17),
@ClosingBalance as decimal(30,17),
@BaseCurrencyID as int,
@ParentAccountID as int,
@ParentSLID as int,
@ComponentID as int

IF EXISTS(SELECT SessionID FROM AccountingSession  WHERE CONVERT(DATE,CONVERT(VARCHAR(12),StartDate,106))=CONVERT(DATE,CONVERT(VARCHAR(12),@OpeningDate,106)) AND SessionType=6)
BEGIN
	SET @SessionID=(SELECT SessionID FROM AccountingSession  WHERE CONVERT(DATE,CONVERT(VARCHAR(12),StartDate,106))=CONVERT(DATE,CONVERT(VARCHAR(12),@OpeningDate,106)) AND SessionType=6)
END
ELSE
BEGIN
	SET @SessionID=(SELECT TOP 1 SessionID FROM AccountingSession  WHERE CONVERT(DATE,CONVERT(VARCHAR(12),StartDate,106))>=CONVERT(DATE,CONVERT(VARCHAR(12),@OpeningDate,106)) AND SessionType=6)
	SET @OpeningDate=(SELECT MIN(StartDate) FROM AccountingSession  WHERE CONVERT(DATE,CONVERT(VARCHAR(12),StartDate,106))>=CONVERT(DATE,CONVERT(VARCHAR(12),@OpeningDate,106)) AND SessionType=6)
END
SET @SessionStartDate=(SELECT StartDate FROM AccountingSession WHERE AccountingSessionID=@SessionID)
SET @BaseCurrencyID = (SELECT C.BaseCurrencyID FROM Company AS C WHERE C.CompanyID=1)

IF(@CurrencyID<=0)
BEGIN
	SET @CurrencyID = (SELECT C.BaseCurrencyID FROM Company AS C WHERE C.CompanyID=1)
END

SET @ParentAccountID=(SELECT VB.AccountHeadID FROM VoucherBill AS VB WHERE VB.VoucherBillID=@VoucherBillID)
SET @ParentSLID=(SELECT VB.SubLedgerID FROM VoucherBill AS VB WHERE VB.VoucherBillID=@VoucherBillID)
SET @ComponentID=[dbo].GetComponentID(@ParentAccountID)

CREATE TABLE #TempTableVBill (
							ID int IDENTITY(1,1) PRIMARY KEY,
							VoucherBillTransactionID int,
							VoucherBillID int,	
							BillNo Varchar(512),
							Amount decimal(30,17),						
							RemainingBalance decimal(30,17),
							CreditDays int, 
							IsDebit bit,
							ComponentType smallint,
							SessionOpening decimal(30,17),
							Debit_Opening decimal(30,17),
							Credit_Opening decimal(30,17),
							OpeningBalance decimal(30,17),
							BillDate DateTime,
							DueDate DateTime,
							OverdueByDays int,
							DebitAmount decimal(30,17),
							CreditAmount decimal(30,17),
							ClosingBalance decimal(30,17),
							IsDr_Closing bit,
							AccountHeadID int,
							CCID int,
							VoucherID int,
							VoucherNo varchar(512),
							VoucherDate datetime,
							AccountHeadName varchar(512),
							AccountHeadCode varchar(512),
							SubledgerCode varchar(512),
							SubledgerName varchar(512)
						)

CREATE TABLE #ResultTableVBill (
							ID int IDENTITY(1,1) PRIMARY KEY,
							VoucherBillTransactionID int,
							VoucherBillID int,	
							BillNo Varchar(512),
							Amount decimal(30,17),						
							RemainingBalance decimal(30,17),
							CreditDays int, 
							IsDebit bit,
							ComponentType smallint,
							SessionOpening decimal(30,17),
							Debit_Opening decimal(30,17),
							Credit_Opening decimal(30,17),
							OpeningBalance decimal(30,17),
							BillDate DateTime,
							DueDate DateTime,
							OverdueByDays int,
							DebitAmount decimal(30,17),
							CreditAmount decimal(30,17),
							ClosingBalance decimal(30,17),
							IsDr_Closing bit,
							AccountHeadID int,
							CCID int,
							VoucherID int,
							VoucherNo varchar(512),
							VoucherDate datetime,
							AccountHeadName varchar(512),
							AccountHeadCode varchar(512),
							SubledgerCode varchar(512),
							SubledgerName varchar(512)
						)

IF(@BusinessUnitID>0)
BEGIN
	IF(@IsApproved=1)
	BEGIN
		IF(@CurrencyID=@BaseCurrencyID)
		BEGIN
			SET @DebitOpenningBalance=ISNULL((SELECT SUM(AOB.OpenningBalance) FROM AccountOpenningBreakdown AS AOB WHERE AOB.IsDr=1 AND AOB.BusinessUnitID=@BusinessUnitID AND AOB.AccountingSessionID=@SessionID AND AOB.BreakdownType=2 AND AOB.BreakdownObjID=@VoucherBillID),0)
			SET @CreditOpenningBalance=ISNULL((SELECT SUM(AOB.OpenningBalance) FROM AccountOpenningBreakdown AS AOB WHERE AOB.IsDr=0 AND AOB.BusinessUnitID=@BusinessUnitID AND  AOB.AccountingSessionID=@SessionID AND AOB.BreakdownType=2 AND AOB.BreakdownObjID=@VoucherBillID),0)
			SET @Debit_OpeningAmount=ISNULL((SELECT SUM(VBT.Amount*VBT.ConversionRate) FROM View_VoucherBillTransaction AS VBT WHERE VBT.BUID=@BusinessUnitID AND VBT.ApprovedBy!=0 AND  VBT.IsDr=1 AND VBT.VoucherBillID=@VoucherBillID AND CONVERT(DATE, CONVERT(VARCHAR(12),VBT.TransactionDate,106)) BETWEEN  CONVERT(DATE, CONVERT(VARCHAR(12),@SessionStartDate,106)) AND CONVERT(DATE, CONVERT(VARCHAR(12),DATEADD(DAY,-1,@OpeningDate),106))),0)
			SET @Credit_OpeningAmount=ISNULL((SELECT SUM(VBT.Amount*VBT.ConversionRate) FROM View_VoucherBillTransaction AS VBT WHERE VBT.BUID=@BusinessUnitID AND VBT.ApprovedBy!=0 AND VBT.IsDr=0 AND VBT.VoucherBillID=@VoucherBillID AND CONVERT(DATE, CONVERT(VARCHAR(12),VBT.TransactionDate,106)) BETWEEN  CONVERT(DATE, CONVERT(VARCHAR(12),@SessionStartDate,106)) AND CONVERT(DATE, CONVERT(VARCHAR(12),DATEADD(DAY,-1,@OpeningDate),106))),0)
		END
		ELSE
		BEGIN
			SET @DebitOpenningBalance=ISNULL((SELECT SUM(AOB.AmountInCurrency) FROM AccountOpenningBreakdown AS AOB WHERE AOB.IsDr=1 AND AOB.BusinessUnitID=@BusinessUnitID AND AOB.CurrencyID=@CurrencyID AND  AOB.AccountingSessionID=@SessionID AND AOB.BreakdownType=2 AND AOB.BreakdownObjID=@VoucherBillID),0)
			SET @CreditOpenningBalance=ISNULL((SELECT SUM(AOB.AmountInCurrency) FROM AccountOpenningBreakdown AS AOB WHERE AOB.IsDr=0 AND AOB.BusinessUnitID=@BusinessUnitID AND AOB.CurrencyID=@CurrencyID AND  AOB.AccountingSessionID=@SessionID AND AOB.BreakdownType=2 AND AOB.BreakdownObjID=@VoucherBillID),0)
			SET @Debit_OpeningAmount=ISNULL((SELECT SUM(VBT.Amount) FROM View_VoucherBillTransaction AS VBT WHERE  VBT.BUID=@BusinessUnitID AND VBT.ApprovedBy!=0 AND VBT.CurrencyID=@CurrencyID AND VBT.IsDr=1 AND VBT.VoucherBillID=@VoucherBillID AND CONVERT(DATE, CONVERT(VARCHAR(12),VBT.TransactionDate,106)) BETWEEN  CONVERT(DATE, CONVERT(VARCHAR(12),@SessionStartDate,106)) AND CONVERT(DATE, CONVERT(VARCHAR(12),DATEADD(DAY,-1,@OpeningDate),106))),0)
			SET @Credit_OpeningAmount=ISNULL((SELECT SUM(VBT.Amount) FROM View_VoucherBillTransaction AS VBT WHERE  VBT.BUID=@BusinessUnitID AND VBT.ApprovedBy!=0 AND VBT.CurrencyID=@CurrencyID AND VBT.IsDr=0 AND VBT.VoucherBillID=@VoucherBillID AND CONVERT(DATE, CONVERT(VARCHAR(12),VBT.TransactionDate,106)) BETWEEN  CONVERT(DATE, CONVERT(VARCHAR(12),@SessionStartDate,106)) AND CONVERT(DATE, CONVERT(VARCHAR(12),DATEADD(DAY,-1,@OpeningDate),106))),0)
		END
	END
	ELSE
	BEGIN
		IF(@CurrencyID=@BaseCurrencyID)
		BEGIN
			SET @DebitOpenningBalance=ISNULL((SELECT SUM(AOB.OpenningBalance) FROM AccountOpenningBreakdown AS AOB WHERE AOB.IsDr=1 AND AOB.BusinessUnitID=@BusinessUnitID AND  AOB.AccountingSessionID=@SessionID AND AOB.BreakdownType=2 AND AOB.BreakdownObjID=@VoucherBillID),0)
			SET @CreditOpenningBalance=ISNULL((SELECT SUM(AOB.OpenningBalance) FROM AccountOpenningBreakdown AS AOB WHERE AOB.IsDr=0 AND AOB.BusinessUnitID=@BusinessUnitID AND  AOB.AccountingSessionID=@SessionID AND AOB.BreakdownType=2 AND AOB.BreakdownObjID=@VoucherBillID),0)
			SET @Debit_OpeningAmount=ISNULL((SELECT SUM(VBT.Amount*VBT.ConversionRate) FROM View_VoucherBillTransaction AS VBT WHERE VBT.BUID=@BusinessUnitID AND VBT.IsDr=1 AND VBT.VoucherBillID=@VoucherBillID AND CONVERT(DATE, CONVERT(VARCHAR(12),VBT.TransactionDate,106)) BETWEEN  CONVERT(DATE, CONVERT(VARCHAR(12),@SessionStartDate,106)) AND CONVERT(DATE, CONVERT(VARCHAR(12),DATEADD(DAY,-1,@OpeningDate),106))),0)
			SET @Credit_OpeningAmount=ISNULL((SELECT SUM(VBT.Amount*VBT.ConversionRate) FROM View_VoucherBillTransaction AS VBT WHERE VBT.BUID=@BusinessUnitID AND VBT.IsDr=0 AND VBT.VoucherBillID=@VoucherBillID AND CONVERT(DATE, CONVERT(VARCHAR(12),VBT.TransactionDate,106)) BETWEEN  CONVERT(DATE, CONVERT(VARCHAR(12),@SessionStartDate,106)) AND CONVERT(DATE, CONVERT(VARCHAR(12),DATEADD(DAY,-1,@OpeningDate),106))),0)
		END
		ELSE
		BEGIN
			SET @DebitOpenningBalance=ISNULL((SELECT SUM(AOB.AmountInCurrency) FROM AccountOpenningBreakdown AS AOB WHERE AOB.IsDr=1 AND AOB.BusinessUnitID=@BusinessUnitID AND AOB.CurrencyID=@CurrencyID AND  AOB.AccountingSessionID=@SessionID AND AOB.BreakdownType=2 AND AOB.BreakdownObjID=@VoucherBillID),0)
			SET @CreditOpenningBalance=ISNULL((SELECT SUM(AOB.AmountInCurrency) FROM AccountOpenningBreakdown AS AOB WHERE AOB.IsDr=0 AND AOB.BusinessUnitID=@BusinessUnitID AND AOB.CurrencyID=@CurrencyID AND  AOB.AccountingSessionID=@SessionID AND AOB.BreakdownType=2 AND AOB.BreakdownObjID=@VoucherBillID),0)
			SET @Debit_OpeningAmount=ISNULL((SELECT SUM(VBT.Amount) FROM View_VoucherBillTransaction AS VBT WHERE VBT.BUID=@BusinessUnitID AND VBT.CurrencyID=@CurrencyID AND VBT.IsDr=1 AND VBT.VoucherBillID=@VoucherBillID AND CONVERT(DATE, CONVERT(VARCHAR(12),VBT.TransactionDate,106)) BETWEEN  CONVERT(DATE, CONVERT(VARCHAR(12),@SessionStartDate,106)) AND CONVERT(DATE, CONVERT(VARCHAR(12),DATEADD(DAY,-1,@OpeningDate),106))),0)
			SET @Credit_OpeningAmount=ISNULL((SELECT SUM(VBT.Amount) FROM View_VoucherBillTransaction AS VBT WHERE VBT.BUID=@BusinessUnitID AND VBT.CurrencyID=@CurrencyID AND VBT.IsDr=0 AND VBT.VoucherBillID=@VoucherBillID AND CONVERT(DATE, CONVERT(VARCHAR(12),VBT.TransactionDate,106)) BETWEEN  CONVERT(DATE, CONVERT(VARCHAR(12),@SessionStartDate,106)) AND CONVERT(DATE, CONVERT(VARCHAR(12),DATEADD(DAY,-1,@OpeningDate),106))),0)
		END
	END
END
ELSE
BEGIN
	IF(@IsApproved=1)
	BEGIN
		IF(@CurrencyID=@BaseCurrencyID)
		BEGIN
			SET @DebitOpenningBalance=ISNULL((SELECT SUM(AOB.OpenningBalance) FROM AccountOpenningBreakdown AS AOB WHERE AOB.IsDr=1 AND  AOB.AccountingSessionID=@SessionID AND AOB.BreakdownType=2 AND AOB.BreakdownObjID=@VoucherBillID),0)
			SET @CreditOpenningBalance=ISNULL((SELECT SUM(AOB.OpenningBalance) FROM AccountOpenningBreakdown AS AOB WHERE AOB.IsDr=0 AND  AOB.AccountingSessionID=@SessionID AND AOB.BreakdownType=2 AND AOB.BreakdownObjID=@VoucherBillID),0)
			SET @Debit_OpeningAmount=ISNULL((SELECT SUM(VBT.Amount*VBT.ConversionRate) FROM View_VoucherBillTransaction AS VBT WHERE VBT.ApprovedBy!=0 AND  VBT.IsDr=1 AND VBT.VoucherBillID=@VoucherBillID AND CONVERT(DATE, CONVERT(VARCHAR(12),VBT.TransactionDate,106)) BETWEEN  CONVERT(DATE, CONVERT(VARCHAR(12),@SessionStartDate,106)) AND CONVERT(DATE, CONVERT(VARCHAR(12),DATEADD(DAY,-1,@OpeningDate),106))),0)
			SET @Credit_OpeningAmount=ISNULL((SELECT SUM(VBT.Amount*VBT.ConversionRate) FROM View_VoucherBillTransaction AS VBT WHERE VBT.ApprovedBy!=0 AND VBT.IsDr=0 AND VBT.VoucherBillID=@VoucherBillID AND CONVERT(DATE, CONVERT(VARCHAR(12),VBT.TransactionDate,106)) BETWEEN  CONVERT(DATE, CONVERT(VARCHAR(12),@SessionStartDate,106)) AND CONVERT(DATE, CONVERT(VARCHAR(12),DATEADD(DAY,-1,@OpeningDate),106))),0)
		END
		ELSE
		BEGIN
			SET @DebitOpenningBalance=ISNULL((SELECT SUM(AOB.AmountInCurrency) FROM AccountOpenningBreakdown AS AOB WHERE AOB.IsDr=1 AND AOB.CurrencyID=@CurrencyID AND  AOB.AccountingSessionID=@SessionID AND AOB.BreakdownType=2 AND AOB.BreakdownObjID=@VoucherBillID),0)
			SET @CreditOpenningBalance=ISNULL((SELECT SUM(AOB.AmountInCurrency) FROM AccountOpenningBreakdown AS AOB WHERE AOB.IsDr=0 AND AOB.CurrencyID=@CurrencyID AND  AOB.AccountingSessionID=@SessionID AND AOB.BreakdownType=2 AND AOB.BreakdownObjID=@VoucherBillID),0)
			SET @Debit_OpeningAmount=ISNULL((SELECT SUM(VBT.Amount) FROM View_VoucherBillTransaction AS VBT WHERE  VBT.ApprovedBy!=0 AND VBT.CurrencyID=@CurrencyID AND VBT.IsDr=1 AND VBT.VoucherBillID=@VoucherBillID AND CONVERT(DATE, CONVERT(VARCHAR(12),VBT.TransactionDate,106)) BETWEEN  CONVERT(DATE, CONVERT(VARCHAR(12),@SessionStartDate,106)) AND CONVERT(DATE, CONVERT(VARCHAR(12),DATEADD(DAY,-1,@OpeningDate),106))),0)
			SET @Credit_OpeningAmount=ISNULL((SELECT SUM(VBT.Amount) FROM View_VoucherBillTransaction AS VBT WHERE  VBT.ApprovedBy!=0 AND VBT.CurrencyID=@CurrencyID AND VBT.IsDr=0 AND VBT.VoucherBillID=@VoucherBillID AND CONVERT(DATE, CONVERT(VARCHAR(12),VBT.TransactionDate,106)) BETWEEN  CONVERT(DATE, CONVERT(VARCHAR(12),@SessionStartDate,106)) AND CONVERT(DATE, CONVERT(VARCHAR(12),DATEADD(DAY,-1,@OpeningDate),106))),0)
		END
	END
	ELSE
	BEGIN
		IF(@CurrencyID=@BaseCurrencyID)
		BEGIN
			SET @DebitOpenningBalance=ISNULL((SELECT SUM(AOB.OpenningBalance) FROM AccountOpenningBreakdown AS AOB WHERE AOB.IsDr=1 AND  AOB.AccountingSessionID=@SessionID AND AOB.BreakdownType=2 AND AOB.BreakdownObjID=@VoucherBillID),0)
			SET @CreditOpenningBalance=ISNULL((SELECT SUM(AOB.OpenningBalance) FROM AccountOpenningBreakdown AS AOB WHERE AOB.IsDr=0 AND  AOB.AccountingSessionID=@SessionID AND AOB.BreakdownType=2 AND AOB.BreakdownObjID=@VoucherBillID),0)
			SET @Debit_OpeningAmount=ISNULL((SELECT SUM(VBT.Amount*VBT.ConversionRate) FROM View_VoucherBillTransaction AS VBT WHERE VBT.IsDr=1 AND VBT.VoucherBillID=@VoucherBillID AND CONVERT(DATE, CONVERT(VARCHAR(12),VBT.TransactionDate,106)) BETWEEN  CONVERT(DATE, CONVERT(VARCHAR(12),@SessionStartDate,106)) AND CONVERT(DATE, CONVERT(VARCHAR(12),DATEADD(DAY,-1,@OpeningDate),106))),0)
			SET @Credit_OpeningAmount=ISNULL((SELECT SUM(VBT.Amount*VBT.ConversionRate) FROM View_VoucherBillTransaction AS VBT WHERE VBT.IsDr=0 AND VBT.VoucherBillID=@VoucherBillID AND CONVERT(DATE, CONVERT(VARCHAR(12),VBT.TransactionDate,106)) BETWEEN  CONVERT(DATE, CONVERT(VARCHAR(12),@SessionStartDate,106)) AND CONVERT(DATE, CONVERT(VARCHAR(12),DATEADD(DAY,-1,@OpeningDate),106))),0)
		END
		ELSE
		BEGIN
			SET @DebitOpenningBalance=ISNULL((SELECT SUM(AOB.AmountInCurrency) FROM AccountOpenningBreakdown AS AOB WHERE AOB.IsDr=1 AND AOB.CurrencyID=@CurrencyID AND  AOB.AccountingSessionID=@SessionID AND AOB.BreakdownType=2 AND AOB.BreakdownObjID=@VoucherBillID),0)
			SET @CreditOpenningBalance=ISNULL((SELECT SUM(AOB.AmountInCurrency) FROM AccountOpenningBreakdown AS AOB WHERE AOB.IsDr=0 AND AOB.CurrencyID=@CurrencyID AND  AOB.AccountingSessionID=@SessionID AND AOB.BreakdownType=2 AND AOB.BreakdownObjID=@VoucherBillID),0)
			SET @Debit_OpeningAmount=ISNULL((SELECT SUM(VBT.Amount) FROM View_VoucherBillTransaction AS VBT WHERE VBT.CurrencyID=@CurrencyID AND VBT.IsDr=1 AND VBT.VoucherBillID=@VoucherBillID AND CONVERT(DATE, CONVERT(VARCHAR(12),VBT.TransactionDate,106)) BETWEEN  CONVERT(DATE, CONVERT(VARCHAR(12),@SessionStartDate,106)) AND CONVERT(DATE, CONVERT(VARCHAR(12),DATEADD(DAY,-1,@OpeningDate),106))),0)
			SET @Credit_OpeningAmount=ISNULL((SELECT SUM(VBT.Amount) FROM View_VoucherBillTransaction AS VBT WHERE VBT.CurrencyID=@CurrencyID AND VBT.IsDr=0 AND VBT.VoucherBillID=@VoucherBillID AND CONVERT(DATE, CONVERT(VARCHAR(12),VBT.TransactionDate,106)) BETWEEN  CONVERT(DATE, CONVERT(VARCHAR(12),@SessionStartDate,106)) AND CONVERT(DATE, CONVERT(VARCHAR(12),DATEADD(DAY,-1,@OpeningDate),106))),0)
		END
	END
END

SET @ClosingBalance=0

IF(@ComponentID IN(2,6))
BEGIN
	SET @ClosingBalance=ISNULL((@DebitOpenningBalance-@CreditOpenningBalance+@Debit_OpeningAmount-@Credit_OpeningAmount),0)
END
ELSE
BEGIN
	SET @ClosingBalance=ISNULL((@CreditOpenningBalance-@DebitOpenningBalance-@Debit_OpeningAmount+@Credit_OpeningAmount),0)
END

INSERT INTO #TempTableVBill (VoucherBillTransactionID ,	VoucherBillID ,	BillNo ,Amount ,RemainingBalance ,	CreditDays,	IsDebit ,	ComponentType ,	SessionOpening ,Debit_Opening ,	Credit_Opening ,OpeningBalance,	BillDate ,		DueDate ,		OverdueByDays ,	DebitAmount ,	CreditAmount ,	ClosingBalance ,	IsDr_Closing ,	AccountHeadID ,	CCID,	VoucherID ,	VoucherNo ,	VoucherDate ,	AccountHeadName ,	AccountHeadCode ,	SubledgerCode ,	SubledgerName )
					VALUES	(0,							0,				'',		0,		0.00,				0,			0,			0,				0.00,			0,				0,				0,				@OpeningDate,	@OpeningDate,	0,				0,				0,				@ClosingBalance,	0,				0,				0,		0,			'',			GETDATE(),		'Opening Balance',	'',					'',				'')
IF(@BusinessUnitID>0)
BEGIN
	IF(@IsApproved=1)
	BEGIN
		IF(@CurrencyID=@BaseCurrencyID)
		BEGIN
			INSERT INTO #TempTableVBill(VoucherBillTransactionID)
							SELECT		VBT.VoucherBillTransactionID FROM View_VoucherBillTransaction AS VBT WHERE VBT.BUID=@BusinessUnitID AND VBT.VoucherBillID=@VoucherBillID AND VBT.ApprovedBy!=0  AND CONVERT(DATE, CONVERT(VARCHAR(12),VBT.TransactionDate,106)) BETWEEN  CONVERT(DATE, CONVERT(VARCHAR(12),@OpeningDate,106)) AND CONVERT(DATE, CONVERT(VARCHAR(12),@EndDate,106)) AND VBT.BUID=@BusinessUnitID
		END
		ELSE
		BEGIN
			INSERT INTO #TempTableVBill(VoucherBillTransactionID)
							SELECT		VBT.VoucherBillTransactionID FROM View_VoucherBillTransaction AS VBT WHERE VBT.BUID=@BusinessUnitID AND VBT.VoucherBillID=@VoucherBillID AND VBT.ApprovedBy!=0 AND VBT.CurrencyID=@CurrencyID  AND CONVERT(DATE, CONVERT(VARCHAR(12),VBT.TransactionDate,106)) BETWEEN  CONVERT(DATE, CONVERT(VARCHAR(12),@OpeningDate,106)) AND CONVERT(DATE, CONVERT(VARCHAR(12),@EndDate,106)) AND VBT.BUID=@BusinessUnitID
		END
	END
	ELSE
	BEGIN
		IF(@CurrencyID=@BaseCurrencyID)
		BEGIN
			INSERT INTO #TempTableVBill(VoucherBillTransactionID)
							SELECT		VBT.VoucherBillTransactionID FROM View_VoucherBillTransaction AS VBT WHERE VBT.BUID=@BusinessUnitID AND VBT.VoucherBillID=@VoucherBillID  AND CONVERT(DATE, CONVERT(VARCHAR(12),VBT.TransactionDate,106)) BETWEEN  CONVERT(DATE, CONVERT(VARCHAR(12),@OpeningDate,106)) AND CONVERT(DATE, CONVERT(VARCHAR(12),@EndDate,106))
		END
		ELSE
		BEGIN
			INSERT INTO #TempTableVBill(VoucherBillTransactionID)
							SELECT		VBT.VoucherBillTransactionID FROM View_VoucherBillTransaction AS VBT WHERE VBT.BUID=@BusinessUnitID AND VBT.VoucherBillID=@VoucherBillID AND VBT.CurrencyID=@CurrencyID  AND CONVERT(DATE, CONVERT(VARCHAR(12),VBT.TransactionDate,106)) BETWEEN  CONVERT(DATE, CONVERT(VARCHAR(12),@OpeningDate,106)) AND CONVERT(DATE, CONVERT(VARCHAR(12),@EndDate,106))
		END
	END
END
ELSE
BEGIN
	IF(@IsApproved=1)
	BEGIN
		IF(@CurrencyID=@BaseCurrencyID)
		BEGIN
			INSERT INTO #TempTableVBill(VoucherBillTransactionID)
							SELECT		VBT.VoucherBillTransactionID FROM View_VoucherBillTransaction AS VBT WHERE VBT.VoucherBillID=@VoucherBillID AND VBT.ApprovedBy!=0  AND CONVERT(DATE, CONVERT(VARCHAR(12),VBT.TransactionDate,106)) BETWEEN  CONVERT(DATE, CONVERT(VARCHAR(12),@OpeningDate,106)) AND CONVERT(DATE, CONVERT(VARCHAR(12),@EndDate,106))
		END
		ELSE
		BEGIN
			INSERT INTO #TempTableVBill(VoucherBillTransactionID)
							SELECT		VBT.VoucherBillTransactionID FROM View_VoucherBillTransaction AS VBT WHERE VBT.VoucherBillID=@VoucherBillID AND VBT.ApprovedBy!=0 AND VBT.CurrencyID=@CurrencyID  AND CONVERT(DATE, CONVERT(VARCHAR(12),VBT.TransactionDate,106)) BETWEEN  CONVERT(DATE, CONVERT(VARCHAR(12),@OpeningDate,106)) AND CONVERT(DATE, CONVERT(VARCHAR(12),@EndDate,106))
		END
	END
	ELSE
	BEGIN
		IF(@CurrencyID=@BaseCurrencyID)
		BEGIN
			INSERT INTO #TempTableVBill(VoucherBillTransactionID)
							SELECT		VBT.VoucherBillTransactionID FROM View_VoucherBillTransaction AS VBT WHERE VBT.VoucherBillID=@VoucherBillID  AND CONVERT(DATE, CONVERT(VARCHAR(12),VBT.TransactionDate,106)) BETWEEN  CONVERT(DATE, CONVERT(VARCHAR(12),@OpeningDate,106)) AND CONVERT(DATE, CONVERT(VARCHAR(12),@EndDate,106))
		END
		ELSE
		BEGIN
			INSERT INTO #TempTableVBill(VoucherBillTransactionID)
							SELECT		VBT.VoucherBillTransactionID FROM View_VoucherBillTransaction AS VBT WHERE VBT.VoucherBillID=@VoucherBillID AND VBT.CurrencyID=@CurrencyID  AND CONVERT(DATE, CONVERT(VARCHAR(12),VBT.TransactionDate,106)) BETWEEN  CONVERT(DATE, CONVERT(VARCHAR(12),@OpeningDate,106)) AND CONVERT(DATE, CONVERT(VARCHAR(12),@EndDate,106))
		END
	END
END

UPDATE #TempTableVBill
SET VoucherID=(SELECT VD.VoucherID FROM View_VoucherDetail AS VD WHERE VD.VoucherDetailID=(SELECT VBT.VoucherDetailID FROM View_VoucherBillTransaction AS VBT WHERE VBT.VoucherBillTransactionID=TT.VoucherBillTransactionID)),
	IsDebit=(SELECT VBT.IsDr FROM View_VoucherBillTransaction AS VBT WHERE VBT.VoucherBillTransactionID=TT.VoucherBillTransactionID)	
FROM #TempTableVBill AS TT

IF(@CurrencyID=@BaseCurrencyID)
BEGIN
	UPDATE #TempTableVBill
	SET DebitAmount=ISNULL((SELECT (VBT.Amount*VBT.ConversionRate) FROM VoucherBillTransaction AS VBT WHERE VBT.VoucherBillTransactionID=TT.VoucherBillTransactionID),0),
		CreditAmount=0.00
	FROM #TempTableVBill AS TT WHERE TT.IsDebit=1

	UPDATE #TempTableVBill
	SET DebitAmount=0.00,
		CreditAmount=(SELECT (VBT.Amount*VBT.ConversionRate) FROM VoucherBillTransaction AS VBT WHERE VBT.VoucherBillTransactionID=TT.VoucherBillTransactionID)
	FROM #TempTableVBill AS TT WHERE TT.IsDebit=0
END
ELSE
BEGIN
	UPDATE #TempTableVBill
	SET DebitAmount=ISNULL((SELECT VBT.Amount FROM VoucherBillTransaction AS VBT WHERE VBT.VoucherBillTransactionID=TT.VoucherBillTransactionID),0),
		CreditAmount=0.00
	FROM #TempTableVBill AS TT WHERE TT.IsDebit=1

	UPDATE #TempTableVBill
	SET DebitAmount=0.00,
		CreditAmount=(SELECT VBT.Amount FROM VoucherBillTransaction AS VBT WHERE VBT.VoucherBillTransactionID=TT.VoucherBillTransactionID)
	FROM #TempTableVBill AS TT WHERE TT.IsDebit=0
END

UPDATE #TempTableVBill
SET AccountHeadName=ISNULL((SELECT TOP(1)AccountHeadName FROM View_VoucherDetail WHERE  IsDebit!=TT.IsDebit AND  AccountHeadID<>@ParentAccountID AND  VoucherID=TT.VoucherID),''),
	AccountHeadCode=ISNULL((SELECT VD.AccountHeadCode FROM View_VoucherDetail AS VD WHERE VD.VoucherDetailID=(SELECT VBT.VoucherDetailID FROM VoucherBillTransaction AS VBT WHERE VBT.VoucherBillTransactionID=TT.VoucherBillTransactionID)),''),
	VoucherDate=(SELECT V.VoucherDate FROM Voucher AS V WHERE V.VoucherID=TT.VoucherID),
	VoucherNo=(SELECT V.VoucherNo FROM Voucher AS V WHERE V.VoucherID=TT.VoucherID),
	SubledgerCode=(SELECT VBT.CostCenterCode FROM View_VoucherBillTransaction AS VBT WHERE VBT.VoucherBillTransactionID=(SELECT VBT.VoucherBillTransactionID FROM VoucherBillTransaction AS VBT WHERE VBT.VoucherBillTransactionID=TT.VoucherBillTransactionID)),
	SubledgerName=(SELECT VBT.CostCenterName FROM View_VoucherBillTransaction AS VBT WHERE VBT.VoucherBillTransactionID=(SELECT VBT.VoucherBillTransactionID FROM VoucherBillTransaction AS VBT WHERE VBT.VoucherBillTransactionID=TT.VoucherBillTransactionID)),
	VoucherBillID=(SELECT VBT.VoucherBillID FROM VoucherBillTransaction AS VBT WHERE VBT.VoucherBillTransactionID=TT.VoucherBillTransactionID),
	BillNo=(SELECT VB.BillNo FROM View_VoucherBill AS VB WHERE VB.VoucherBillID=(SELECT VBT.VoucherBillID FROM VoucherBillTransaction AS VBT WHERE VBT.VoucherBillTransactionID=TT.VoucherBillTransactionID)),
	Amount=(SELECT VB.Amount FROM View_VoucherBill AS VB WHERE VB.VoucherBillID=(SELECT VBT.VoucherBillID FROM VoucherBillTransaction AS VBT WHERE VBT.VoucherBillTransactionID=TT.VoucherBillTransactionID)),
	RemainingBalance=(SELECT VB.RemainningBalance FROM View_VoucherBill AS VB WHERE VB.VoucherBillID=(SELECT VBT.VoucherBillID FROM VoucherBillTransaction AS VBT WHERE VBT.VoucherBillTransactionID=TT.VoucherBillTransactionID)),
	CreditDays=(SELECT VB.CreditDays FROM View_VoucherBill AS VB WHERE VB.VoucherBillID=(SELECT VBT.VoucherBillID FROM VoucherBillTransaction AS VBT WHERE VBT.VoucherBillTransactionID=TT.VoucherBillTransactionID)),
	BillDate=(SELECT VB.BillDate FROM View_VoucherBill AS VB WHERE VB.VoucherBillID=(SELECT VBT.VoucherBillID FROM VoucherBillTransaction AS VBT WHERE VBT.VoucherBillTransactionID=TT.VoucherBillTransactionID)),
	DueDate=(SELECT VB.DueDate FROM View_VoucherBill AS VB WHERE VB.VoucherBillID=(SELECT VBT.VoucherBillID FROM VoucherBillTransaction AS VBT WHERE VBT.VoucherBillTransactionID=TT.VoucherBillTransactionID)),
	AccountHeadID=@ParentAccountID,
	CCID=@ParentSLID,
	ComponentType=@ComponentID
FROM #TempTableVBill AS TT WHERE TT.ID!=1


INSERT INTO #ResultTableVBill(	VoucherBillTransactionID ,		VoucherBillID ,		BillNo ,	Amount ,	RemainingBalance ,		CreditDays,		IsDebit ,	ComponentType ,		SessionOpening ,	Debit_Opening ,		Credit_Opening ,	OpeningBalance,		BillDate ,		DueDate ,		OverdueByDays ,		DebitAmount ,		CreditAmount ,		ClosingBalance ,	IsDr_Closing ,		AccountHeadID ,		CCID,	VoucherID ,		VoucherNo ,		VoucherDate ,		AccountHeadName ,		AccountHeadCode ,		SubledgerCode ,		SubledgerName )
				  SELECT		TT.VoucherBillTransactionID ,	TT.VoucherBillID ,	TT.BillNo ,	TT.Amount ,	TT.RemainingBalance ,	TT.CreditDays,	TT.IsDebit ,TT.ComponentType ,	TT.SessionOpening ,	TT.Debit_Opening ,	TT.Credit_Opening ,	TT.OpeningBalance,	TT.BillDate ,	TT.DueDate ,	TT.OverdueByDays ,	TT.DebitAmount ,	TT.CreditAmount ,	TT.ClosingBalance ,	TT.IsDr_Closing ,	TT.AccountHeadID ,	TT.CCID,TT.VoucherID ,	TT.VoucherNo ,	TT.VoucherDate ,	TT.AccountHeadName ,	TT.AccountHeadCode ,	TT.SubledgerCode ,	TT.SubledgerName  
				  FROM #TempTableVBill  AS TT ORDER BY TT.AccountHeadID, TT.VoucherDate ASC

DECLARE
@ID as int,
@Count as int,
@IsDebit as bit,
@Amount as decimal(30,17),
@CurrentBalance as decimal(30,17)

SET @ID=1
SET @Count=(SELECT COUNT(*) FROM #ResultTableVBill)
SET @CurrentBalance =@ClosingBalance

IF(@ComponentID IN(2,6))--ComponentID{Asset = 2,Laibility = 3,OwnerEquity=4,Income = 5,Expeness = 6}
BEGIN
	WHILE(@Count>=@ID)
	BEGIN
		SET @IsDebit=0
		SET @IsDebit=(SELECT TT.IsDebit FROM #ResultTableVBill AS TT WHERE TT.ID=@ID)
		IF(@IsDebit=1)
		BEGIN
			SET @Amount=(SELECT TT.DebitAmount FROM #ResultTableVBill AS TT WHERE TT.ID=@ID)		
			SET @CurrentBalance=@CurrentBalance+@Amount
		END
		ELSE
		BEGIN
			SET @Amount=(SELECT TT.CreditAmount FROM #ResultTableVBill AS TT WHERE TT.ID=@ID)
			SET @CurrentBalance=@CurrentBalance-@Amount
		END
		UPDATE #ResultTableVBill SET ClosingBalance=@CurrentBalance WHERE ID=@ID
		SET @ID=@ID+1
	END
END		
ELSE
BEGIN
	WHILE(@Count>=@ID)
	BEGIN
		SET @IsDebit=0
		SET @IsDebit=(SELECT TT.IsDebit FROM #ResultTableVBill AS TT WHERE TT.ID=@ID)
		IF(@IsDebit=1)
		BEGIN
			SET @Amount=(SELECT TT.DebitAmount FROM #ResultTableVBill AS TT WHERE TT.ID=@ID)		
			SET @CurrentBalance=@CurrentBalance-@Amount
		END
		ELSE
		BEGIN
			SET @Amount=(SELECT TT.CreditAmount FROM #ResultTableVBill AS TT WHERE TT.ID=@ID)
			SET @CurrentBalance=@CurrentBalance+@Amount
		END
		UPDATE #ResultTableVBill SET ClosingBalance=@CurrentBalance WHERE ID=@ID
		SET @ID=@ID+1
	END
END

SELECT * FROM #ResultTableVBill AS VB ORDER BY VB.AccountHeadID, VB.VoucherDate ASC
DROP TABLE #TempTableVBill
DROP TABLE #ResultTableVBill
COMMIT TRAN

GO
/****** Object:  StoredProcedure [dbo].[SP_VPTransactionLedger]    Script Date: 12/1/2016 2:17:46 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[SP_VPTransactionLedger]
(
	@BusinessUnitID as int,
	@AccountHeadID as int,
	@ProductID as int,
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
--@ProductID as int,
--@CurrencyID as int,
--@StartDate date,
--@EndDate date,
--@IsApproved as bit

--SET @BusinessUnitID =0
--SET @AccountHeadID =0
--SET @ProductID=24--23--24
--SET @CurrencyID =0
--SET @StartDate ='01 JAN 2015'
--SET @EndDate ='31 dec 2016'
--SET @IsApproved=0

CREATE TABLE #TempTable(
							ID int IDENTITY(1,1) PRIMARY KEY,
							ProductID int,
							ProductCode Varchar(512),
							ProductName Varchar(512),
							OpeiningValue decimal(30,17),
							OpeiningQty decimal(30,17),
							IsDebit bit,
							DebitAmount decimal(30,17),
							DebitQty decimal(30,17),
							CreditAmount decimal(30,17),
							CreditQty decimal(30,17),
							ClosingValue decimal(30,17),
							ClosingQty decimal(30,17),
							IsDrClosing bit,
							CurrencySymbol Varchar(512),
							VoucherID int,
							VoucherDate date,
							VoucherNo Varchar(512),
							ParentHeadID INT,
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
							ProductID int,
							ProductCode Varchar(512),
							ProductName Varchar(512),
							OpeiningValue decimal(30,17),
							OpeiningQty decimal(30,17),
							IsDebit bit,
							DebitAmount decimal(30,17),
							DebitQty decimal(30,17),
							CreditAmount decimal(30,17),
							CreditQty decimal(30,17),
							ClosingValue decimal(30,17),
							ClosingQty decimal(30,17),
							IsDrClosing bit,
							CurrencySymbol Varchar(512),
							VoucherID int,
							VoucherDate date,
							VoucherNo Varchar(512),
							ParentHeadID INT,
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
@DebitOpeningQty as decimal(30,17),
@CreditOpeningValue as decimal(30,17),
@CreditOpeningQty as decimal(30,17),
@DebitAmount as decimal(30,17),
@DebitQty as decimal(30,17),
@CreditAmount as decimal(30,17),
@CreditQty as decimal(30,17),
@ClosingValue as decimal(30,17),
@ClosingQty as decimal(30,17)

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
SET @ComponentID =2--iNVENTORY aLWAYS dEBIT--dbo.GetComponentID(@AccountHeadID)

IF(@CurrencyID<=0)
BEGIN
SET @CurrencyID = (SELECT C.BaseCurrencyID FROM Company AS C WHERE C.CompanyID=1)
END

IF(@BusinessUnitID>0)
BEGIN
	IF(@AccountHeadID>0)
	BEGIN
		IF(@IsApproved=1)
		BEGIN
			IF(@CurrencyID=@BaseCurrencyID)
			BEGIN
				SET @DebitOpeningQty=ISNULL((SELECT SUM(AOB.Qty) FROM AccountOpenningBreakdown AS AOB WHERE dbo.GetComponentID(AOB.AccountHeadID)=2 AND dbo.GetComponentID(AOB.AccountHeadID)=2 AND AOB.BusinessUnitID=@BusinessUnitID AND AOB.IsDr=1 AND AOB.AccountHeadID=@AccountHeadID AND AOB.AccountingSessionID=@SessionID AND AOB.BreakdownType=4 AND AOB.BreakdownObjID=@ProductID),0)
				SET @DebitOpeningValue=ISNULL((SELECT SUM(AOB.OpenningBalance) FROM AccountOpenningBreakdown AS AOB WHERE dbo.GetComponentID(AOB.AccountHeadID)=2 AND AOB.BusinessUnitID=@BusinessUnitID AND AOB.IsDr=1 AND AOB.AccountHeadID=@AccountHeadID AND AOB.AccountingSessionID=@SessionID AND AOB.BreakdownType=4 AND AOB.BreakdownObjID=@ProductID),0)
				SET @CreditOpeningQty=ISNULL((SELECT SUM(AOB.Qty) FROM AccountOpenningBreakdown AS AOB WHERE dbo.GetComponentID(AOB.AccountHeadID)=2 AND AOB.BusinessUnitID=@BusinessUnitID AND AOB.IsDr=0 AND AOB.AccountHeadID=@AccountHeadID AND AOB.AccountingSessionID=@SessionID AND AOB.BreakdownType=4 AND AOB.BreakdownObjID=@ProductID),0)
				SET @CreditOpeningValue=ISNULL((SELECT SUM(AOB.OpenningBalance) FROM AccountOpenningBreakdown AS AOB WHERE dbo.GetComponentID(AOB.AccountHeadID)=2 AND AOB.BusinessUnitID=@BusinessUnitID AND AOB.IsDr=0 AND AOB.AccountHeadID=@AccountHeadID AND AOB.AccountingSessionID=@SessionID AND AOB.BreakdownType=4 AND AOB.BreakdownObjID=@ProductID),0)
				SET @DebitQty=ISNULL((SELECT SUM(VPT.Qty) FROM View_VPTransaction AS VPT WHERE dbo.GetComponentID(VPT.AccountHeadID)=2 AND VPT.BUID=@BusinessUnitID AND VPT.ApprovedBy!=0 AND  VPT.IsDr=1 AND VPT.ProductID=@ProductID AND VPT.AccountHeadID=@AccountHeadID AND CONVERT(DATE, CONVERT(VARCHAR(12),VPT.TransactionDate,106)) BETWEEN  CONVERT(DATE, CONVERT(VARCHAR(12),@SessionStartDate,106)) AND CONVERT(DATE, CONVERT(VARCHAR(12),DATEADD(DAY,-1,@StartDate),106))),0)
				SET @DebitAmount=ISNULL((SELECT SUM(VPT.Amount*VPT.ConversionRate) FROM View_VPTransaction AS VPT WHERE dbo.GetComponentID(VPT.AccountHeadID)=2 AND VPT.BUID=@BusinessUnitID AND VPT.ApprovedBy!=0 AND  VPT.IsDr=1 AND VPT.ProductID=@ProductID AND VPT.AccountHeadID=@AccountHeadID AND CONVERT(DATE, CONVERT(VARCHAR(12),VPT.TransactionDate,106)) BETWEEN  CONVERT(DATE, CONVERT(VARCHAR(12),@SessionStartDate,106)) AND CONVERT(DATE, CONVERT(VARCHAR(12),DATEADD(DAY,-1,@StartDate),106))),0)
				SET @CreditQty=ISNULL((SELECT SUM(VPT.Qty) FROM View_VPTransaction AS VPT WHERE dbo.GetComponentID(VPT.AccountHeadID)=2 AND VPT.BUID=@BusinessUnitID AND VPT.ApprovedBy!=0 AND VPT.IsDr=0 AND VPT.ProductID=@ProductID AND VPT.AccountHeadID=@AccountHeadID AND CONVERT(DATE, CONVERT(VARCHAR(12),VPT.TransactionDate,106)) BETWEEN  CONVERT(DATE, CONVERT(VARCHAR(12),@SessionStartDate,106)) AND CONVERT(DATE, CONVERT(VARCHAR(12),DATEADD(DAY,-1,@StartDate),106))),0)
				SET @CreditAmount=ISNULL((SELECT SUM(VPT.Amount*VPT.ConversionRate) FROM View_VPTransaction AS VPT WHERE dbo.GetComponentID(VPT.AccountHeadID)=2 AND VPT.BUID=@BusinessUnitID AND VPT.ApprovedBy!=0 AND VPT.IsDr=0 AND VPT.ProductID=@ProductID AND VPT.AccountHeadID=@AccountHeadID AND CONVERT(DATE, CONVERT(VARCHAR(12),VPT.TransactionDate,106)) BETWEEN  CONVERT(DATE, CONVERT(VARCHAR(12),@SessionStartDate,106)) AND CONVERT(DATE, CONVERT(VARCHAR(12),DATEADD(DAY,-1,@StartDate),106))),0)
			END
			ELSE
			BEGIN
				SET @DebitOpeningQty=ISNULL((SELECT SUM(AOB.Qty) FROM AccountOpenningBreakdown AS AOB WHERE dbo.GetComponentID(AOB.AccountHeadID)=2 AND AOB.BusinessUnitID=@BusinessUnitID AND AOB.IsDr=1 AND AOB.CurrencyID=@CurrencyID AND AOB.AccountHeadID=@AccountHeadID AND AOB.AccountingSessionID=@SessionID AND AOB.BreakdownType=4 AND AOB.BreakdownObjID=@ProductID),0)
				SET @DebitOpeningValue=ISNULL((SELECT SUM(AOB.AmountInCurrency) FROM AccountOpenningBreakdown AS AOB WHERE dbo.GetComponentID(AOB.AccountHeadID)=2 AND AOB.BusinessUnitID=@BusinessUnitID AND AOB.IsDr=1 AND AOB.CurrencyID=@CurrencyID AND AOB.AccountHeadID=@AccountHeadID AND AOB.AccountingSessionID=@SessionID AND AOB.BreakdownType=4 AND AOB.BreakdownObjID=@ProductID),0)
				SET @CreditOpeningQty=ISNULL((SELECT SUM(AOB.Qty) FROM AccountOpenningBreakdown AS AOB WHERE dbo.GetComponentID(AOB.AccountHeadID)=2 AND AOB.BusinessUnitID=@BusinessUnitID AND AOB.IsDr=0 AND AOB.CurrencyID=@CurrencyID AND AOB.AccountHeadID=@AccountHeadID AND AOB.AccountingSessionID=@SessionID AND AOB.BreakdownType=4 AND AOB.BreakdownObjID=@ProductID),0)
				SET @CreditOpeningValue=ISNULL((SELECT SUM(AOB.AmountInCurrency) FROM AccountOpenningBreakdown AS AOB WHERE dbo.GetComponentID(AOB.AccountHeadID)=2 AND AOB.BusinessUnitID=@BusinessUnitID AND AOB.IsDr=0 AND AOB.CurrencyID=@CurrencyID AND AOB.AccountHeadID=@AccountHeadID AND AOB.AccountingSessionID=@SessionID AND AOB.BreakdownType=4 AND AOB.BreakdownObjID=@ProductID),0)
				SET @DebitQty=ISNULL((SELECT SUM(VPT.Qty) FROM View_VPTransaction AS VPT WHERE dbo.GetComponentID(VPT.AccountHeadID)=2 AND  VPT.BUID=@BusinessUnitID AND VPT.ApprovedBy!=0 AND VPT.CurrencyID=@CurrencyID AND VPT.IsDr=1 AND VPT.ProductID=@ProductID AND VPT.AccountHeadID=@AccountHeadID AND CONVERT(DATE, CONVERT(VARCHAR(12),VPT.TransactionDate,106)) BETWEEN  CONVERT(DATE, CONVERT(VARCHAR(12),@SessionStartDate,106)) AND CONVERT(DATE, CONVERT(VARCHAR(12),DATEADD(DAY,-1,@StartDate),106))),0)
				SET @DebitAmount=ISNULL((SELECT SUM(VPT.Amount*VPT.ConversionRate) FROM View_VPTransaction AS VPT WHERE dbo.GetComponentID(VPT.AccountHeadID)=2 AND  VPT.BUID=@BusinessUnitID AND VPT.ApprovedBy!=0 AND VPT.CurrencyID=@CurrencyID AND VPT.IsDr=1 AND VPT.ProductID=@ProductID AND VPT.AccountHeadID=@AccountHeadID AND CONVERT(DATE, CONVERT(VARCHAR(12),VPT.TransactionDate,106)) BETWEEN  CONVERT(DATE, CONVERT(VARCHAR(12),@SessionStartDate,106)) AND CONVERT(DATE, CONVERT(VARCHAR(12),DATEADD(DAY,-1,@StartDate),106))),0)
				SET @CreditQty=ISNULL((SELECT SUM(VPT.Amount*VPT.ConversionRate) FROM View_VPTransaction AS VPT WHERE dbo.GetComponentID(VPT.AccountHeadID)=2 AND  VPT.BUID=@BusinessUnitID AND VPT.ApprovedBy!=0 AND VPT.CurrencyID=@CurrencyID AND VPT.IsDr=0 AND VPT.ProductID=@ProductID AND VPT.AccountHeadID=@AccountHeadID AND CONVERT(DATE, CONVERT(VARCHAR(12),VPT.TransactionDate,106)) BETWEEN  CONVERT(DATE, CONVERT(VARCHAR(12),@SessionStartDate,106)) AND CONVERT(DATE, CONVERT(VARCHAR(12),DATEADD(DAY,-1,@StartDate),106))),0)
				SET @CreditAmount=ISNULL((SELECT SUM(VPT.Amount*VPT.ConversionRate) FROM View_VPTransaction AS VPT WHERE dbo.GetComponentID(VPT.AccountHeadID)=2 AND  VPT.BUID=@BusinessUnitID AND VPT.ApprovedBy!=0 AND VPT.CurrencyID=@CurrencyID AND VPT.IsDr=0 AND VPT.ProductID=@ProductID AND VPT.AccountHeadID=@AccountHeadID AND CONVERT(DATE, CONVERT(VARCHAR(12),VPT.TransactionDate,106)) BETWEEN  CONVERT(DATE, CONVERT(VARCHAR(12),@SessionStartDate,106)) AND CONVERT(DATE, CONVERT(VARCHAR(12),DATEADD(DAY,-1,@StartDate),106))),0)
			END
		END
		ELSE
		BEGIN
			IF(@CurrencyID=@BaseCurrencyID)
			BEGIN
				SET @DebitOpeningQty=ISNULL((SELECT SUM(AOB.Qty) FROM AccountOpenningBreakdown AS AOB WHERE dbo.GetComponentID(AOB.AccountHeadID)=2 AND AOB.BusinessUnitID=@BusinessUnitID AND AOB.IsDr=1 AND AOB.AccountHeadID=@AccountHeadID AND AOB.AccountingSessionID=@SessionID AND AOB.BreakdownType=4 AND AOB.BreakdownObjID=@ProductID),0)
				SET @DebitOpeningValue=ISNULL((SELECT SUM(AOB.OpenningBalance) FROM AccountOpenningBreakdown AS AOB WHERE dbo.GetComponentID(AOB.AccountHeadID)=2 AND AOB.BusinessUnitID=@BusinessUnitID AND AOB.IsDr=1 AND AOB.AccountHeadID=@AccountHeadID AND AOB.AccountingSessionID=@SessionID AND AOB.BreakdownType=4 AND AOB.BreakdownObjID=@ProductID),0)
				SET @CreditOpeningQty=ISNULL((SELECT SUM(AOB.Qty) FROM AccountOpenningBreakdown AS AOB WHERE dbo.GetComponentID(AOB.AccountHeadID)=2 AND AOB.BusinessUnitID=@BusinessUnitID AND AOB.IsDr=0 AND AOB.AccountHeadID=@AccountHeadID AND AOB.AccountingSessionID=@SessionID AND AOB.BreakdownType=4 AND AOB.BreakdownObjID=@ProductID),0)
				SET @CreditOpeningValue=ISNULL((SELECT SUM(AOB.OpenningBalance) FROM AccountOpenningBreakdown AS AOB WHERE dbo.GetComponentID(AOB.AccountHeadID)=2 AND AOB.BusinessUnitID=@BusinessUnitID AND AOB.IsDr=0 AND AOB.AccountHeadID=@AccountHeadID AND AOB.AccountingSessionID=@SessionID AND AOB.BreakdownType=4 AND AOB.BreakdownObjID=@ProductID),0)
				SET @DebitQty=ISNULL((SELECT SUM(VPT.Qty) FROM View_VPTransaction AS VPT WHERE dbo.GetComponentID(VPT.AccountHeadID)=2 AND VPT.BUID=@BusinessUnitID AND VPT.IsDr=1 AND VPT.ProductID=@ProductID AND VPT.AccountHeadID=@AccountHeadID AND CONVERT(DATE, CONVERT(VARCHAR(12),VPT.TransactionDate,106)) BETWEEN  CONVERT(DATE, CONVERT(VARCHAR(12),@SessionStartDate,106)) AND CONVERT(DATE, CONVERT(VARCHAR(12),DATEADD(DAY,-1,@StartDate),106))),0)
				SET @DebitAmount=ISNULL((SELECT SUM(VPT.Amount*VPT.ConversionRate) FROM View_VPTransaction AS VPT WHERE dbo.GetComponentID(VPT.AccountHeadID)=2 AND VPT.BUID=@BusinessUnitID AND VPT.IsDr=1 AND VPT.ProductID=@ProductID AND VPT.AccountHeadID=@AccountHeadID AND CONVERT(DATE, CONVERT(VARCHAR(12),VPT.TransactionDate,106)) BETWEEN  CONVERT(DATE, CONVERT(VARCHAR(12),@SessionStartDate,106)) AND CONVERT(DATE, CONVERT(VARCHAR(12),DATEADD(DAY,-1,@StartDate),106))),0)
				SET @CreditQty=ISNULL((SELECT SUM(VPT.Qty) FROM View_VPTransaction AS VPT WHERE dbo.GetComponentID(VPT.AccountHeadID)=2 AND VPT.BUID=@BusinessUnitID AND VPT.IsDr=0 AND VPT.ProductID=@ProductID AND VPT.AccountHeadID=@AccountHeadID AND CONVERT(DATE, CONVERT(VARCHAR(12),VPT.TransactionDate,106)) BETWEEN  CONVERT(DATE, CONVERT(VARCHAR(12),@SessionStartDate,106)) AND CONVERT(DATE, CONVERT(VARCHAR(12),DATEADD(DAY,-1,@StartDate),106))),0)
				SET @CreditAmount=ISNULL((SELECT SUM(VPT.Amount*VPT.ConversionRate) FROM View_VPTransaction AS VPT WHERE dbo.GetComponentID(VPT.AccountHeadID)=2 AND VPT.BUID=@BusinessUnitID AND VPT.IsDr=0 AND VPT.ProductID=@ProductID AND VPT.AccountHeadID=@AccountHeadID AND CONVERT(DATE, CONVERT(VARCHAR(12),VPT.TransactionDate,106)) BETWEEN  CONVERT(DATE, CONVERT(VARCHAR(12),@SessionStartDate,106)) AND CONVERT(DATE, CONVERT(VARCHAR(12),DATEADD(DAY,-1,@StartDate),106))),0)
			END
			ELSE
			BEGIN
				SET @DebitOpeningQty=ISNULL((SELECT SUM(AOB.Qty) FROM AccountOpenningBreakdown AS AOB WHERE dbo.GetComponentID(AOB.AccountHeadID)=2 AND AOB.BusinessUnitID=@BusinessUnitID AND AOB.IsDr=1 AND AOB.CurrencyID=@CurrencyID AND AOB.AccountHeadID=@AccountHeadID AND AOB.AccountingSessionID=@SessionID AND AOB.BreakdownType=4 AND AOB.BreakdownObjID=@ProductID),0)
				SET @DebitOpeningValue=ISNULL((SELECT SUM(AOB.AmountInCurrency) FROM AccountOpenningBreakdown AS AOB WHERE dbo.GetComponentID(AOB.AccountHeadID)=2 AND AOB.BusinessUnitID=@BusinessUnitID AND AOB.IsDr=1 AND AOB.CurrencyID=@CurrencyID AND AOB.AccountHeadID=@AccountHeadID AND AOB.AccountingSessionID=@SessionID AND AOB.BreakdownType=4 AND AOB.BreakdownObjID=@ProductID),0)
				SET @CreditOpeningQty=ISNULL((SELECT SUM(AOB.Qty) FROM AccountOpenningBreakdown AS AOB WHERE dbo.GetComponentID(AOB.AccountHeadID)=2 AND AOB.BusinessUnitID=@BusinessUnitID AND AOB.IsDr=0 AND AOB.CurrencyID=@CurrencyID AND AOB.AccountHeadID=@AccountHeadID AND AOB.AccountingSessionID=@SessionID AND AOB.BreakdownType=4 AND AOB.BreakdownObjID=@ProductID),0)
				SET @CreditOpeningValue=ISNULL((SELECT SUM(AOB.AmountInCurrency) FROM AccountOpenningBreakdown AS AOB WHERE dbo.GetComponentID(AOB.AccountHeadID)=2 AND AOB.BusinessUnitID=@BusinessUnitID AND AOB.IsDr=0 AND AOB.CurrencyID=@CurrencyID AND AOB.AccountHeadID=@AccountHeadID AND AOB.AccountingSessionID=@SessionID AND AOB.BreakdownType=4 AND AOB.BreakdownObjID=@ProductID),0)
				SET @DebitQty=ISNULL((SELECT SUM(VPT.Qty) FROM View_VPTransaction AS VPT WHERE dbo.GetComponentID(VPT.AccountHeadID)=2 AND VPT.BUID=@BusinessUnitID AND VPT.CurrencyID=@CurrencyID AND VPT.IsDr=1 AND VPT.ProductID=@ProductID AND VPT.AccountHeadID=@AccountHeadID AND CONVERT(DATE, CONVERT(VARCHAR(12),VPT.TransactionDate,106)) BETWEEN  CONVERT(DATE, CONVERT(VARCHAR(12),@SessionStartDate,106)) AND CONVERT(DATE, CONVERT(VARCHAR(12),DATEADD(DAY,-1,@StartDate),106))),0)
				SET @DebitAmount=ISNULL((SELECT SUM(VPT.Amount*VPT.ConversionRate) FROM View_VPTransaction AS VPT WHERE dbo.GetComponentID(VPT.AccountHeadID)=2 AND VPT.BUID=@BusinessUnitID AND VPT.CurrencyID=@CurrencyID AND VPT.IsDr=1 AND VPT.ProductID=@ProductID AND VPT.AccountHeadID=@AccountHeadID AND CONVERT(DATE, CONVERT(VARCHAR(12),VPT.TransactionDate,106)) BETWEEN  CONVERT(DATE, CONVERT(VARCHAR(12),@SessionStartDate,106)) AND CONVERT(DATE, CONVERT(VARCHAR(12),DATEADD(DAY,-1,@StartDate),106))),0)
				SET @CreditQty=ISNULL((SELECT SUM(VPT.Qty) FROM View_VPTransaction AS VPT WHERE dbo.GetComponentID(VPT.AccountHeadID)=2 AND VPT.BUID=@BusinessUnitID AND VPT.CurrencyID=@CurrencyID AND VPT.IsDr=0 AND VPT.ProductID=@ProductID AND VPT.AccountHeadID=@AccountHeadID AND CONVERT(DATE, CONVERT(VARCHAR(12),VPT.TransactionDate,106)) BETWEEN  CONVERT(DATE, CONVERT(VARCHAR(12),@SessionStartDate,106)) AND CONVERT(DATE, CONVERT(VARCHAR(12),DATEADD(DAY,-1,@StartDate),106))),0)
				SET @CreditAmount=ISNULL((SELECT SUM(VPT.Amount*VPT.ConversionRate) FROM View_VPTransaction AS VPT WHERE dbo.GetComponentID(VPT.AccountHeadID)=2 AND VPT.BUID=@BusinessUnitID AND VPT.CurrencyID=@CurrencyID AND VPT.IsDr=0 AND VPT.ProductID=@ProductID AND VPT.AccountHeadID=@AccountHeadID AND CONVERT(DATE, CONVERT(VARCHAR(12),VPT.TransactionDate,106)) BETWEEN  CONVERT(DATE, CONVERT(VARCHAR(12),@SessionStartDate,106)) AND CONVERT(DATE, CONVERT(VARCHAR(12),DATEADD(DAY,-1,@StartDate),106))),0)
			END
		END
	END
	ELSE
	BEGIN
		IF(@IsApproved=1)
		BEGIN
			IF(@CurrencyID=@BaseCurrencyID)
			BEGIN
				SET @DebitOpeningQty=ISNULL((SELECT SUM(AOB.Qty) FROM AccountOpenningBreakdown AS AOB WHERE dbo.GetComponentID(AOB.AccountHeadID)=2 AND AOB.BusinessUnitID=@BusinessUnitID AND AOB.IsDr=1 AND AOB.AccountingSessionID=@SessionID AND AOB.BreakdownType=4 AND AOB.BreakdownObjID=@ProductID),0)
				SET @DebitOpeningValue=ISNULL((SELECT SUM(AOB.OpenningBalance) FROM AccountOpenningBreakdown AS AOB WHERE dbo.GetComponentID(AOB.AccountHeadID)=2 AND AOB.BusinessUnitID=@BusinessUnitID AND AOB.IsDr=1 AND AOB.AccountingSessionID=@SessionID AND AOB.BreakdownType=4 AND AOB.BreakdownObjID=@ProductID),0)
				SET @CreditOpeningQty=ISNULL((SELECT SUM(AOB.Qty) FROM AccountOpenningBreakdown AS AOB WHERE dbo.GetComponentID(AOB.AccountHeadID)=2 AND AOB.BusinessUnitID=@BusinessUnitID AND AOB.IsDr=0 AND AOB.AccountingSessionID=@SessionID AND AOB.BreakdownType=4 AND AOB.BreakdownObjID=@ProductID),0)
				SET @CreditOpeningValue=ISNULL((SELECT SUM(AOB.OpenningBalance) FROM AccountOpenningBreakdown AS AOB WHERE dbo.GetComponentID(AOB.AccountHeadID)=2 AND AOB.BusinessUnitID=@BusinessUnitID AND AOB.IsDr=0 AND AOB.AccountingSessionID=@SessionID AND AOB.BreakdownType=4 AND AOB.BreakdownObjID=@ProductID),0)
				SET @DebitQty=ISNULL((SELECT SUM(VPT.Qty) FROM View_VPTransaction AS VPT WHERE dbo.GetComponentID(VPT.AccountHeadID)=2 AND VPT.BUID=@BusinessUnitID AND VPT.ApprovedBy!=0 AND  VPT.IsDr=1 AND VPT.ProductID=@ProductID AND CONVERT(DATE, CONVERT(VARCHAR(12),VPT.TransactionDate,106)) BETWEEN  CONVERT(DATE, CONVERT(VARCHAR(12),@SessionStartDate,106)) AND CONVERT(DATE, CONVERT(VARCHAR(12),DATEADD(DAY,-1,@StartDate),106))),0)
				SET @DebitAmount=ISNULL((SELECT SUM(VPT.Amount*VPT.ConversionRate) FROM View_VPTransaction AS VPT WHERE dbo.GetComponentID(VPT.AccountHeadID)=2 AND VPT.BUID=@BusinessUnitID AND VPT.ApprovedBy!=0 AND  VPT.IsDr=1 AND VPT.ProductID=@ProductID AND CONVERT(DATE, CONVERT(VARCHAR(12),VPT.TransactionDate,106)) BETWEEN  CONVERT(DATE, CONVERT(VARCHAR(12),@SessionStartDate,106)) AND CONVERT(DATE, CONVERT(VARCHAR(12),DATEADD(DAY,-1,@StartDate),106))),0)
				SET @CreditQty=ISNULL((SELECT SUM(VPT.Qty) FROM View_VPTransaction AS VPT WHERE dbo.GetComponentID(VPT.AccountHeadID)=2 AND VPT.BUID=@BusinessUnitID AND VPT.ApprovedBy!=0 AND VPT.IsDr=0 AND VPT.ProductID=@ProductID AND CONVERT(DATE, CONVERT(VARCHAR(12),VPT.TransactionDate,106)) BETWEEN  CONVERT(DATE, CONVERT(VARCHAR(12),@SessionStartDate,106)) AND CONVERT(DATE, CONVERT(VARCHAR(12),DATEADD(DAY,-1,@StartDate),106))),0)
				SET @CreditAmount=ISNULL((SELECT SUM(VPT.Amount*VPT.ConversionRate) FROM View_VPTransaction AS VPT WHERE dbo.GetComponentID(VPT.AccountHeadID)=2 AND VPT.BUID=@BusinessUnitID AND VPT.ApprovedBy!=0 AND VPT.IsDr=0 AND VPT.ProductID=@ProductID AND CONVERT(DATE, CONVERT(VARCHAR(12),VPT.TransactionDate,106)) BETWEEN  CONVERT(DATE, CONVERT(VARCHAR(12),@SessionStartDate,106)) AND CONVERT(DATE, CONVERT(VARCHAR(12),DATEADD(DAY,-1,@StartDate),106))),0)
			END
			ELSE
			BEGIN
				SET @DebitOpeningQty=ISNULL((SELECT SUM(AOB.Qty) FROM AccountOpenningBreakdown AS AOB WHERE dbo.GetComponentID(AOB.AccountHeadID)=2 AND AOB.BusinessUnitID=@BusinessUnitID AND AOB.IsDr=1 AND AOB.CurrencyID=@CurrencyID AND AOB.AccountingSessionID=@SessionID AND AOB.BreakdownType=4 AND AOB.BreakdownObjID=@ProductID),0)
				SET @DebitOpeningValue=ISNULL((SELECT SUM(AOB.AmountInCurrency) FROM AccountOpenningBreakdown AS AOB WHERE dbo.GetComponentID(AOB.AccountHeadID)=2 AND AOB.BusinessUnitID=@BusinessUnitID AND AOB.IsDr=1 AND AOB.CurrencyID=@CurrencyID AND AOB.AccountingSessionID=@SessionID AND AOB.BreakdownType=4 AND AOB.BreakdownObjID=@ProductID),0)
				SET @CreditOpeningQty=ISNULL((SELECT SUM(AOB.Qty) FROM AccountOpenningBreakdown AS AOB WHERE dbo.GetComponentID(AOB.AccountHeadID)=2 AND AOB.BusinessUnitID=@BusinessUnitID AND AOB.IsDr=0 AND AOB.CurrencyID=@CurrencyID AND AOB.AccountingSessionID=@SessionID AND AOB.BreakdownType=4 AND AOB.BreakdownObjID=@ProductID),0)
				SET @CreditOpeningValue=ISNULL((SELECT SUM(AOB.AmountInCurrency) FROM AccountOpenningBreakdown AS AOB WHERE dbo.GetComponentID(AOB.AccountHeadID)=2 AND AOB.BusinessUnitID=@BusinessUnitID AND AOB.IsDr=0 AND AOB.CurrencyID=@CurrencyID AND AOB.AccountingSessionID=@SessionID AND AOB.BreakdownType=4 AND AOB.BreakdownObjID=@ProductID),0)
				SET @DebitQty=ISNULL((SELECT SUM(VPT.Qty) FROM View_VPTransaction AS VPT WHERE dbo.GetComponentID(VPT.AccountHeadID)=2 AND  VPT.BUID=@BusinessUnitID AND VPT.ApprovedBy!=0 AND VPT.CurrencyID=@CurrencyID AND VPT.IsDr=1 AND VPT.ProductID=@ProductID AND CONVERT(DATE, CONVERT(VARCHAR(12),VPT.TransactionDate,106)) BETWEEN  CONVERT(DATE, CONVERT(VARCHAR(12),@SessionStartDate,106)) AND CONVERT(DATE, CONVERT(VARCHAR(12),DATEADD(DAY,-1,@StartDate),106))),0)
				SET @DebitAmount=ISNULL((SELECT SUM(VPT.Amount*VPT.ConversionRate) FROM View_VPTransaction AS VPT WHERE dbo.GetComponentID(VPT.AccountHeadID)=2 AND  VPT.BUID=@BusinessUnitID AND VPT.ApprovedBy!=0 AND VPT.CurrencyID=@CurrencyID AND VPT.IsDr=1 AND VPT.ProductID=@ProductID AND CONVERT(DATE, CONVERT(VARCHAR(12),VPT.TransactionDate,106)) BETWEEN  CONVERT(DATE, CONVERT(VARCHAR(12),@SessionStartDate,106)) AND CONVERT(DATE, CONVERT(VARCHAR(12),DATEADD(DAY,-1,@StartDate),106))),0)
				SET @CreditQty=ISNULL((SELECT SUM(VPT.Qty) FROM View_VPTransaction AS VPT WHERE dbo.GetComponentID(VPT.AccountHeadID)=2 AND  VPT.BUID=@BusinessUnitID AND VPT.ApprovedBy!=0 AND VPT.CurrencyID=@CurrencyID AND VPT.IsDr=0 AND VPT.ProductID=@ProductID AND CONVERT(DATE, CONVERT(VARCHAR(12),VPT.TransactionDate,106)) BETWEEN  CONVERT(DATE, CONVERT(VARCHAR(12),@SessionStartDate,106)) AND CONVERT(DATE, CONVERT(VARCHAR(12),DATEADD(DAY,-1,@StartDate),106))),0)
				SET @CreditAmount=ISNULL((SELECT SUM(VPT.Amount*VPT.ConversionRate) FROM View_VPTransaction AS VPT WHERE dbo.GetComponentID(VPT.AccountHeadID)=2 AND  VPT.BUID=@BusinessUnitID AND VPT.ApprovedBy!=0 AND VPT.CurrencyID=@CurrencyID AND VPT.IsDr=0 AND VPT.ProductID=@ProductID AND CONVERT(DATE, CONVERT(VARCHAR(12),VPT.TransactionDate,106)) BETWEEN  CONVERT(DATE, CONVERT(VARCHAR(12),@SessionStartDate,106)) AND CONVERT(DATE, CONVERT(VARCHAR(12),DATEADD(DAY,-1,@StartDate),106))),0)
			END
		END
		ELSE
		BEGIN
			IF(@CurrencyID=@BaseCurrencyID)
			BEGIN
				SET @DebitOpeningQty=ISNULL((SELECT SUM(AOB.Qty) FROM AccountOpenningBreakdown AS AOB WHERE dbo.GetComponentID(AOB.AccountHeadID)=2 AND AOB.BusinessUnitID=@BusinessUnitID AND AOB.IsDr=1 AND AOB.AccountHeadID=@AccountHeadID AND AOB.AccountingSessionID=@SessionID AND AOB.BreakdownType=4 AND AOB.BreakdownObjID=@ProductID),0)
				SET @DebitOpeningValue=ISNULL((SELECT SUM(AOB.OpenningBalance) FROM AccountOpenningBreakdown AS AOB WHERE dbo.GetComponentID(AOB.AccountHeadID)=2 AND AOB.BusinessUnitID=@BusinessUnitID AND AOB.IsDr=1 AND AOB.AccountHeadID=@AccountHeadID AND AOB.AccountingSessionID=@SessionID AND AOB.BreakdownType=4 AND AOB.BreakdownObjID=@ProductID),0)
				SET @CreditOpeningQty=ISNULL((SELECT SUM(AOB.Qty) FROM AccountOpenningBreakdown AS AOB WHERE dbo.GetComponentID(AOB.AccountHeadID)=2 AND AOB.BusinessUnitID=@BusinessUnitID AND AOB.IsDr=0 AND AOB.AccountHeadID=@AccountHeadID AND AOB.AccountingSessionID=@SessionID AND AOB.BreakdownType=4 AND AOB.BreakdownObjID=@ProductID),0)
				SET @CreditOpeningValue=ISNULL((SELECT SUM(AOB.OpenningBalance) FROM AccountOpenningBreakdown AS AOB WHERE dbo.GetComponentID(AOB.AccountHeadID)=2 AND AOB.BusinessUnitID=@BusinessUnitID AND AOB.IsDr=0 AND AOB.AccountHeadID=@AccountHeadID AND AOB.AccountingSessionID=@SessionID AND AOB.BreakdownType=4 AND AOB.BreakdownObjID=@ProductID),0)
				SET @DebitQty=ISNULL((SELECT SUM(VPT.Qty) FROM View_VPTransaction AS VPT WHERE dbo.GetComponentID(VPT.AccountHeadID)=2 AND VPT.BUID=@BusinessUnitID AND VPT.IsDr=1 AND VPT.ProductID=@ProductID AND VPT.AccountHeadID=@AccountHeadID AND CONVERT(DATE, CONVERT(VARCHAR(12),VPT.TransactionDate,106)) BETWEEN  CONVERT(DATE, CONVERT(VARCHAR(12),@SessionStartDate,106)) AND CONVERT(DATE, CONVERT(VARCHAR(12),DATEADD(DAY,-1,@StartDate),106))),0)
				SET @DebitAmount=ISNULL((SELECT SUM(VPT.Amount*VPT.ConversionRate) FROM View_VPTransaction AS VPT WHERE dbo.GetComponentID(VPT.AccountHeadID)=2 AND VPT.BUID=@BusinessUnitID AND VPT.IsDr=1 AND VPT.ProductID=@ProductID AND VPT.AccountHeadID=@AccountHeadID AND CONVERT(DATE, CONVERT(VARCHAR(12),VPT.TransactionDate,106)) BETWEEN  CONVERT(DATE, CONVERT(VARCHAR(12),@SessionStartDate,106)) AND CONVERT(DATE, CONVERT(VARCHAR(12),DATEADD(DAY,-1,@StartDate),106))),0)
				SET @CreditQty=ISNULL((SELECT SUM(VPT.Qty) FROM View_VPTransaction AS VPT WHERE dbo.GetComponentID(VPT.AccountHeadID)=2 AND VPT.BUID=@BusinessUnitID AND VPT.IsDr=0 AND VPT.ProductID=@ProductID AND VPT.AccountHeadID=@AccountHeadID AND CONVERT(DATE, CONVERT(VARCHAR(12),VPT.TransactionDate,106)) BETWEEN  CONVERT(DATE, CONVERT(VARCHAR(12),@SessionStartDate,106)) AND CONVERT(DATE, CONVERT(VARCHAR(12),DATEADD(DAY,-1,@StartDate),106))),0)
				SET @CreditAmount=ISNULL((SELECT SUM(VPT.Amount*VPT.ConversionRate) FROM View_VPTransaction AS VPT WHERE dbo.GetComponentID(VPT.AccountHeadID)=2 AND VPT.BUID=@BusinessUnitID AND VPT.IsDr=0 AND VPT.ProductID=@ProductID AND VPT.AccountHeadID=@AccountHeadID AND CONVERT(DATE, CONVERT(VARCHAR(12),VPT.TransactionDate,106)) BETWEEN  CONVERT(DATE, CONVERT(VARCHAR(12),@SessionStartDate,106)) AND CONVERT(DATE, CONVERT(VARCHAR(12),DATEADD(DAY,-1,@StartDate),106))),0)
			END
			ELSE
			BEGIN
				SET @DebitOpeningQty=ISNULL((SELECT SUM(AOB.Qty) FROM AccountOpenningBreakdown AS AOB WHERE dbo.GetComponentID(AOB.AccountHeadID)=2 AND AOB.BusinessUnitID=@BusinessUnitID AND AOB.IsDr=1 AND AOB.CurrencyID=@CurrencyID AND AOB.AccountHeadID=@AccountHeadID AND AOB.AccountingSessionID=@SessionID AND AOB.BreakdownType=4 AND AOB.BreakdownObjID=@ProductID),0)
				SET @DebitOpeningValue=ISNULL((SELECT SUM(AOB.AmountInCurrency) FROM AccountOpenningBreakdown AS AOB WHERE dbo.GetComponentID(AOB.AccountHeadID)=2 AND AOB.BusinessUnitID=@BusinessUnitID AND AOB.IsDr=1 AND AOB.CurrencyID=@CurrencyID AND AOB.AccountHeadID=@AccountHeadID AND AOB.AccountingSessionID=@SessionID AND AOB.BreakdownType=4 AND AOB.BreakdownObjID=@ProductID),0)
				SET @CreditOpeningQty=ISNULL((SELECT SUM(AOB.Qty) FROM AccountOpenningBreakdown AS AOB WHERE dbo.GetComponentID(AOB.AccountHeadID)=2 AND AOB.BusinessUnitID=@BusinessUnitID AND AOB.IsDr=0 AND AOB.CurrencyID=@CurrencyID AND AOB.AccountHeadID=@AccountHeadID AND AOB.AccountingSessionID=@SessionID AND AOB.BreakdownType=4 AND AOB.BreakdownObjID=@ProductID),0)
				SET @CreditOpeningValue=ISNULL((SELECT SUM(AOB.AmountInCurrency) FROM AccountOpenningBreakdown AS AOB WHERE dbo.GetComponentID(AOB.AccountHeadID)=2 AND AOB.BusinessUnitID=@BusinessUnitID AND AOB.IsDr=0 AND AOB.CurrencyID=@CurrencyID AND AOB.AccountHeadID=@AccountHeadID AND AOB.AccountingSessionID=@SessionID AND AOB.BreakdownType=4 AND AOB.BreakdownObjID=@ProductID),0)
				SET @DebitQty=ISNULL((SELECT SUM(VPT.Qty) FROM View_VPTransaction AS VPT WHERE dbo.GetComponentID(VPT.AccountHeadID)=2 AND VPT.BUID=@BusinessUnitID AND VPT.CurrencyID=@CurrencyID AND VPT.IsDr=1 AND VPT.ProductID=@ProductID AND VPT.AccountHeadID=@AccountHeadID AND CONVERT(DATE, CONVERT(VARCHAR(12),VPT.TransactionDate,106)) BETWEEN  CONVERT(DATE, CONVERT(VARCHAR(12),@SessionStartDate,106)) AND CONVERT(DATE, CONVERT(VARCHAR(12),DATEADD(DAY,-1,@StartDate),106))),0)
				SET @DebitAmount=ISNULL((SELECT SUM(VPT.Amount*VPT.ConversionRate) FROM View_VPTransaction AS VPT WHERE dbo.GetComponentID(VPT.AccountHeadID)=2 AND VPT.BUID=@BusinessUnitID AND VPT.CurrencyID=@CurrencyID AND VPT.IsDr=1 AND VPT.ProductID=@ProductID AND VPT.AccountHeadID=@AccountHeadID AND CONVERT(DATE, CONVERT(VARCHAR(12),VPT.TransactionDate,106)) BETWEEN  CONVERT(DATE, CONVERT(VARCHAR(12),@SessionStartDate,106)) AND CONVERT(DATE, CONVERT(VARCHAR(12),DATEADD(DAY,-1,@StartDate),106))),0)
				SET @CreditQty=ISNULL((SELECT SUM(VPT.Amount*VPT.ConversionRate) FROM View_VPTransaction AS VPT WHERE dbo.GetComponentID(VPT.AccountHeadID)=2 AND VPT.BUID=@BusinessUnitID AND VPT.CurrencyID=@CurrencyID AND VPT.IsDr=0 AND VPT.ProductID=@ProductID AND VPT.AccountHeadID=@AccountHeadID AND CONVERT(DATE, CONVERT(VARCHAR(12),VPT.TransactionDate,106)) BETWEEN  CONVERT(DATE, CONVERT(VARCHAR(12),@SessionStartDate,106)) AND CONVERT(DATE, CONVERT(VARCHAR(12),DATEADD(DAY,-1,@StartDate),106))),0)
				SET @CreditAmount=ISNULL((SELECT SUM(VPT.Amount*VPT.ConversionRate) FROM View_VPTransaction AS VPT WHERE dbo.GetComponentID(VPT.AccountHeadID)=2 AND VPT.BUID=@BusinessUnitID AND VPT.CurrencyID=@CurrencyID AND VPT.IsDr=0 AND VPT.ProductID=@ProductID AND VPT.AccountHeadID=@AccountHeadID AND CONVERT(DATE, CONVERT(VARCHAR(12),VPT.TransactionDate,106)) BETWEEN  CONVERT(DATE, CONVERT(VARCHAR(12),@SessionStartDate,106)) AND CONVERT(DATE, CONVERT(VARCHAR(12),DATEADD(DAY,-1,@StartDate),106))),0)
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
				SET @DebitOpeningQty=ISNULL((SELECT SUM(AOB.Qty) FROM AccountOpenningBreakdown AS AOB WHERE dbo.GetComponentID(AOB.AccountHeadID)=2 AND AOB.IsDr=1 AND AOB.AccountHeadID=@AccountHeadID AND AOB.AccountingSessionID=@SessionID AND AOB.BreakdownType=4 AND AOB.BreakdownObjID=@ProductID),0)
				SET @DebitOpeningValue=ISNULL((SELECT SUM(AOB.OpenningBalance) FROM AccountOpenningBreakdown AS AOB WHERE dbo.GetComponentID(AOB.AccountHeadID)=2 AND AOB.IsDr=1 AND AOB.AccountHeadID=@AccountHeadID AND AOB.AccountingSessionID=@SessionID AND AOB.BreakdownType=4 AND AOB.BreakdownObjID=@ProductID),0)
				SET @CreditOpeningQty=ISNULL((SELECT SUM(AOB.Qty) FROM AccountOpenningBreakdown AS AOB WHERE dbo.GetComponentID(AOB.AccountHeadID)=2 AND AOB.IsDr=0 AND AOB.AccountHeadID=@AccountHeadID AND AOB.AccountingSessionID=@SessionID AND AOB.BreakdownType=4 AND AOB.BreakdownObjID=@ProductID),0)
				SET @CreditOpeningValue=ISNULL((SELECT SUM(AOB.OpenningBalance) FROM AccountOpenningBreakdown AS AOB WHERE dbo.GetComponentID(AOB.AccountHeadID)=2 AND AOB.IsDr=0 AND AOB.AccountHeadID=@AccountHeadID AND AOB.AccountingSessionID=@SessionID AND AOB.BreakdownType=4 AND AOB.BreakdownObjID=@ProductID),0)
				SET @DebitQty=ISNULL((SELECT SUM(VPT.Qty) FROM View_VPTransaction AS VPT WHERE dbo.GetComponentID(VPT.AccountHeadID)=2 AND VPT.ApprovedBy!=0 AND  VPT.IsDr=1 AND VPT.ProductID=@ProductID AND VPT.AccountHeadID=@AccountHeadID AND CONVERT(DATE, CONVERT(VARCHAR(12),VPT.TransactionDate,106)) BETWEEN  CONVERT(DATE, CONVERT(VARCHAR(12),@SessionStartDate,106)) AND CONVERT(DATE, CONVERT(VARCHAR(12),DATEADD(DAY,-1,@StartDate),106))),0)
				SET @DebitAmount=ISNULL((SELECT SUM(VPT.Amount*VPT.ConversionRate) FROM View_VPTransaction AS VPT WHERE dbo.GetComponentID(VPT.AccountHeadID)=2 AND VPT.ApprovedBy!=0 AND  VPT.IsDr=1 AND VPT.ProductID=@ProductID AND VPT.AccountHeadID=@AccountHeadID AND CONVERT(DATE, CONVERT(VARCHAR(12),VPT.TransactionDate,106)) BETWEEN  CONVERT(DATE, CONVERT(VARCHAR(12),@SessionStartDate,106)) AND CONVERT(DATE, CONVERT(VARCHAR(12),DATEADD(DAY,-1,@StartDate),106))),0)
				SET @CreditQty=ISNULL((SELECT SUM(VPT.Qty) FROM View_VPTransaction AS VPT WHERE dbo.GetComponentID(VPT.AccountHeadID)=2 AND VPT.ApprovedBy!=0 AND VPT.IsDr=0 AND VPT.ProductID=@ProductID AND VPT.AccountHeadID=@AccountHeadID AND CONVERT(DATE, CONVERT(VARCHAR(12),VPT.TransactionDate,106)) BETWEEN  CONVERT(DATE, CONVERT(VARCHAR(12),@SessionStartDate,106)) AND CONVERT(DATE, CONVERT(VARCHAR(12),DATEADD(DAY,-1,@StartDate),106))),0)
				SET @CreditAmount=ISNULL((SELECT SUM(VPT.Amount*VPT.ConversionRate) FROM View_VPTransaction AS VPT WHERE dbo.GetComponentID(VPT.AccountHeadID)=2 AND VPT.ApprovedBy!=0 AND VPT.IsDr=0 AND VPT.ProductID=@ProductID AND VPT.AccountHeadID=@AccountHeadID AND CONVERT(DATE, CONVERT(VARCHAR(12),VPT.TransactionDate,106)) BETWEEN  CONVERT(DATE, CONVERT(VARCHAR(12),@SessionStartDate,106)) AND CONVERT(DATE, CONVERT(VARCHAR(12),DATEADD(DAY,-1,@StartDate),106))),0)
			END
			ELSE
			BEGIN
				SET @DebitOpeningQty=ISNULL((SELECT SUM(AOB.Qty) FROM AccountOpenningBreakdown AS AOB WHERE dbo.GetComponentID(AOB.AccountHeadID)=2 AND AOB.IsDr=1 AND AOB.CurrencyID=@CurrencyID AND AOB.AccountHeadID=@AccountHeadID AND AOB.AccountingSessionID=@SessionID AND AOB.BreakdownType=4 AND AOB.BreakdownObjID=@ProductID),0)
				SET @DebitOpeningValue=ISNULL((SELECT SUM(AOB.AmountInCurrency) FROM AccountOpenningBreakdown AS AOB WHERE dbo.GetComponentID(AOB.AccountHeadID)=2 AND AOB.IsDr=1 AND AOB.CurrencyID=@CurrencyID AND AOB.AccountHeadID=@AccountHeadID AND AOB.AccountingSessionID=@SessionID AND AOB.BreakdownType=4 AND AOB.BreakdownObjID=@ProductID),0)
				SET @CreditOpeningQty=ISNULL((SELECT SUM(AOB.Qty) FROM AccountOpenningBreakdown AS AOB WHERE dbo.GetComponentID(AOB.AccountHeadID)=2 AND AOB.IsDr=0 AND AOB.CurrencyID=@CurrencyID AND AOB.AccountHeadID=@AccountHeadID AND AOB.AccountingSessionID=@SessionID AND AOB.BreakdownType=4 AND AOB.BreakdownObjID=@ProductID),0)
				SET @CreditOpeningValue=ISNULL((SELECT SUM(AOB.AmountInCurrency) FROM AccountOpenningBreakdown AS AOB WHERE dbo.GetComponentID(AOB.AccountHeadID)=2 AND AOB.IsDr=0 AND AOB.CurrencyID=@CurrencyID AND AOB.AccountHeadID=@AccountHeadID AND AOB.AccountingSessionID=@SessionID AND AOB.BreakdownType=4 AND AOB.BreakdownObjID=@ProductID),0)
				SET @DebitQty=ISNULL((SELECT SUM(VPT.Qty) FROM View_VPTransaction AS VPT WHERE dbo.GetComponentID(VPT.AccountHeadID)=2 AND  VPT.ApprovedBy!=0 AND VPT.CurrencyID=@CurrencyID AND VPT.IsDr=1 AND VPT.ProductID=@ProductID AND VPT.AccountHeadID=@AccountHeadID AND CONVERT(DATE, CONVERT(VARCHAR(12),VPT.TransactionDate,106)) BETWEEN  CONVERT(DATE, CONVERT(VARCHAR(12),@SessionStartDate,106)) AND CONVERT(DATE, CONVERT(VARCHAR(12),DATEADD(DAY,-1,@StartDate),106))),0)
				SET @DebitAmount=ISNULL((SELECT SUM(VPT.Amount*VPT.ConversionRate) FROM View_VPTransaction AS VPT WHERE dbo.GetComponentID(VPT.AccountHeadID)=2 AND  VPT.ApprovedBy!=0 AND VPT.CurrencyID=@CurrencyID AND VPT.IsDr=1 AND VPT.ProductID=@ProductID AND VPT.AccountHeadID=@AccountHeadID AND CONVERT(DATE, CONVERT(VARCHAR(12),VPT.TransactionDate,106)) BETWEEN  CONVERT(DATE, CONVERT(VARCHAR(12),@SessionStartDate,106)) AND CONVERT(DATE, CONVERT(VARCHAR(12),DATEADD(DAY,-1,@StartDate),106))),0)
				SET @CreditQty=ISNULL((SELECT SUM(VPT.Amount*VPT.ConversionRate) FROM View_VPTransaction AS VPT WHERE dbo.GetComponentID(VPT.AccountHeadID)=2 AND  VPT.ApprovedBy!=0 AND VPT.CurrencyID=@CurrencyID AND VPT.IsDr=0 AND VPT.ProductID=@ProductID AND VPT.AccountHeadID=@AccountHeadID AND CONVERT(DATE, CONVERT(VARCHAR(12),VPT.TransactionDate,106)) BETWEEN  CONVERT(DATE, CONVERT(VARCHAR(12),@SessionStartDate,106)) AND CONVERT(DATE, CONVERT(VARCHAR(12),DATEADD(DAY,-1,@StartDate),106))),0)
				SET @CreditAmount=ISNULL((SELECT SUM(VPT.Amount*VPT.ConversionRate) FROM View_VPTransaction AS VPT WHERE dbo.GetComponentID(VPT.AccountHeadID)=2 AND  VPT.ApprovedBy!=0 AND VPT.CurrencyID=@CurrencyID AND VPT.IsDr=0 AND VPT.ProductID=@ProductID AND VPT.AccountHeadID=@AccountHeadID AND CONVERT(DATE, CONVERT(VARCHAR(12),VPT.TransactionDate,106)) BETWEEN  CONVERT(DATE, CONVERT(VARCHAR(12),@SessionStartDate,106)) AND CONVERT(DATE, CONVERT(VARCHAR(12),DATEADD(DAY,-1,@StartDate),106))),0)
			END
		END
		ELSE
		BEGIN
			IF(@CurrencyID=@BaseCurrencyID)
			BEGIN
				SET @DebitOpeningQty=ISNULL((SELECT SUM(AOB.Qty) FROM AccountOpenningBreakdown AS AOB WHERE dbo.GetComponentID(AOB.AccountHeadID)=2 AND AOB.IsDr=1 AND AOB.AccountHeadID=@AccountHeadID AND AOB.AccountingSessionID=@SessionID AND AOB.BreakdownType=4 AND AOB.BreakdownObjID=@ProductID),0)
				SET @DebitOpeningValue=ISNULL((SELECT SUM(AOB.OpenningBalance) FROM AccountOpenningBreakdown AS AOB WHERE dbo.GetComponentID(AOB.AccountHeadID)=2 AND AOB.IsDr=1 AND AOB.AccountHeadID=@AccountHeadID AND AOB.AccountingSessionID=@SessionID AND AOB.BreakdownType=4 AND AOB.BreakdownObjID=@ProductID),0)
				SET @CreditOpeningQty=ISNULL((SELECT SUM(AOB.Qty) FROM AccountOpenningBreakdown AS AOB WHERE dbo.GetComponentID(AOB.AccountHeadID)=2 AND AOB.IsDr=0 AND AOB.AccountHeadID=@AccountHeadID AND AOB.AccountingSessionID=@SessionID AND AOB.BreakdownType=4 AND AOB.BreakdownObjID=@ProductID),0)
				SET @CreditOpeningValue=ISNULL((SELECT SUM(AOB.OpenningBalance) FROM AccountOpenningBreakdown AS AOB WHERE dbo.GetComponentID(AOB.AccountHeadID)=2 AND AOB.IsDr=0 AND AOB.AccountHeadID=@AccountHeadID AND AOB.AccountingSessionID=@SessionID AND AOB.BreakdownType=4 AND AOB.BreakdownObjID=@ProductID),0)
				SET @DebitQty=ISNULL((SELECT SUM(VPT.Qty) FROM View_VPTransaction AS VPT WHERE dbo.GetComponentID(VPT.AccountHeadID)=2 AND VPT.IsDr=1 AND VPT.ProductID=@ProductID AND VPT.AccountHeadID=@AccountHeadID AND CONVERT(DATE, CONVERT(VARCHAR(12),VPT.TransactionDate,106)) BETWEEN  CONVERT(DATE, CONVERT(VARCHAR(12),@SessionStartDate,106)) AND CONVERT(DATE, CONVERT(VARCHAR(12),DATEADD(DAY,-1,@StartDate),106))),0)
				SET @DebitAmount=ISNULL((SELECT SUM(VPT.Amount*VPT.ConversionRate) FROM View_VPTransaction AS VPT WHERE dbo.GetComponentID(VPT.AccountHeadID)=2 AND VPT.IsDr=1 AND VPT.ProductID=@ProductID AND VPT.AccountHeadID=@AccountHeadID AND CONVERT(DATE, CONVERT(VARCHAR(12),VPT.TransactionDate,106)) BETWEEN  CONVERT(DATE, CONVERT(VARCHAR(12),@SessionStartDate,106)) AND CONVERT(DATE, CONVERT(VARCHAR(12),DATEADD(DAY,-1,@StartDate),106))),0)
				SET @CreditQty=ISNULL((SELECT SUM(VPT.Qty) FROM View_VPTransaction AS VPT WHERE dbo.GetComponentID(VPT.AccountHeadID)=2 AND VPT.IsDr=0 AND VPT.ProductID=@ProductID AND VPT.AccountHeadID=@AccountHeadID AND CONVERT(DATE, CONVERT(VARCHAR(12),VPT.TransactionDate,106)) BETWEEN  CONVERT(DATE, CONVERT(VARCHAR(12),@SessionStartDate,106)) AND CONVERT(DATE, CONVERT(VARCHAR(12),DATEADD(DAY,-1,@StartDate),106))),0)
				SET @CreditAmount=ISNULL((SELECT SUM(VPT.Amount*VPT.ConversionRate) FROM View_VPTransaction AS VPT WHERE dbo.GetComponentID(VPT.AccountHeadID)=2 AND VPT.IsDr=0 AND VPT.ProductID=@ProductID AND VPT.AccountHeadID=@AccountHeadID AND CONVERT(DATE, CONVERT(VARCHAR(12),VPT.TransactionDate,106)) BETWEEN  CONVERT(DATE, CONVERT(VARCHAR(12),@SessionStartDate,106)) AND CONVERT(DATE, CONVERT(VARCHAR(12),DATEADD(DAY,-1,@StartDate),106))),0)
			END
			ELSE
			BEGIN
				SET @DebitOpeningQty=ISNULL((SELECT SUM(AOB.Qty) FROM AccountOpenningBreakdown AS AOB WHERE dbo.GetComponentID(AOB.AccountHeadID)=2 AND AOB.IsDr=1 AND AOB.CurrencyID=@CurrencyID AND AOB.AccountHeadID=@AccountHeadID AND AOB.AccountingSessionID=@SessionID AND AOB.BreakdownType=4 AND AOB.BreakdownObjID=@ProductID),0)
				SET @DebitOpeningValue=ISNULL((SELECT SUM(AOB.AmountInCurrency) FROM AccountOpenningBreakdown AS AOB WHERE dbo.GetComponentID(AOB.AccountHeadID)=2 AND AOB.IsDr=1 AND AOB.CurrencyID=@CurrencyID AND AOB.AccountHeadID=@AccountHeadID AND AOB.AccountingSessionID=@SessionID AND AOB.BreakdownType=4 AND AOB.BreakdownObjID=@ProductID),0)
				SET @CreditOpeningQty=ISNULL((SELECT SUM(AOB.Qty) FROM AccountOpenningBreakdown AS AOB WHERE dbo.GetComponentID(AOB.AccountHeadID)=2 AND AOB.IsDr=0 AND AOB.CurrencyID=@CurrencyID AND AOB.AccountHeadID=@AccountHeadID AND AOB.AccountingSessionID=@SessionID AND AOB.BreakdownType=4 AND AOB.BreakdownObjID=@ProductID),0)
				SET @CreditOpeningValue=ISNULL((SELECT SUM(AOB.AmountInCurrency) FROM AccountOpenningBreakdown AS AOB WHERE dbo.GetComponentID(AOB.AccountHeadID)=2 AND AOB.IsDr=0 AND AOB.CurrencyID=@CurrencyID AND AOB.AccountHeadID=@AccountHeadID AND AOB.AccountingSessionID=@SessionID AND AOB.BreakdownType=4 AND AOB.BreakdownObjID=@ProductID),0)
				SET @DebitQty=ISNULL((SELECT SUM(VPT.Qty) FROM View_VPTransaction AS VPT WHERE dbo.GetComponentID(VPT.AccountHeadID)=2 AND VPT.CurrencyID=@CurrencyID AND VPT.IsDr=1 AND VPT.ProductID=@ProductID AND VPT.AccountHeadID=@AccountHeadID AND CONVERT(DATE, CONVERT(VARCHAR(12),VPT.TransactionDate,106)) BETWEEN  CONVERT(DATE, CONVERT(VARCHAR(12),@SessionStartDate,106)) AND CONVERT(DATE, CONVERT(VARCHAR(12),DATEADD(DAY,-1,@StartDate),106))),0)
				SET @DebitAmount=ISNULL((SELECT SUM(VPT.Amount*VPT.ConversionRate) FROM View_VPTransaction AS VPT WHERE dbo.GetComponentID(VPT.AccountHeadID)=2 AND VPT.CurrencyID=@CurrencyID AND VPT.IsDr=1 AND VPT.ProductID=@ProductID AND VPT.AccountHeadID=@AccountHeadID AND CONVERT(DATE, CONVERT(VARCHAR(12),VPT.TransactionDate,106)) BETWEEN  CONVERT(DATE, CONVERT(VARCHAR(12),@SessionStartDate,106)) AND CONVERT(DATE, CONVERT(VARCHAR(12),DATEADD(DAY,-1,@StartDate),106))),0)
				SET @CreditQty=ISNULL((SELECT SUM(VPT.Qty) FROM View_VPTransaction AS VPT WHERE dbo.GetComponentID(VPT.AccountHeadID)=2 AND VPT.CurrencyID=@CurrencyID AND VPT.IsDr=0 AND VPT.ProductID=@ProductID AND VPT.AccountHeadID=@AccountHeadID AND CONVERT(DATE, CONVERT(VARCHAR(12),VPT.TransactionDate,106)) BETWEEN  CONVERT(DATE, CONVERT(VARCHAR(12),@SessionStartDate,106)) AND CONVERT(DATE, CONVERT(VARCHAR(12),DATEADD(DAY,-1,@StartDate),106))),0)
				SET @CreditAmount=ISNULL((SELECT SUM(VPT.Amount*VPT.ConversionRate) FROM View_VPTransaction AS VPT WHERE dbo.GetComponentID(VPT.AccountHeadID)=2 AND VPT.CurrencyID=@CurrencyID AND VPT.IsDr=0 AND VPT.ProductID=@ProductID AND VPT.AccountHeadID=@AccountHeadID AND CONVERT(DATE, CONVERT(VARCHAR(12),VPT.TransactionDate,106)) BETWEEN  CONVERT(DATE, CONVERT(VARCHAR(12),@SessionStartDate,106)) AND CONVERT(DATE, CONVERT(VARCHAR(12),DATEADD(DAY,-1,@StartDate),106))),0)
			END
		END
	END
	ELSE
	BEGIN
		IF(@IsApproved=1)
		BEGIN
			IF(@CurrencyID=@BaseCurrencyID)
			BEGIN
				SET @DebitOpeningQty=ISNULL((SELECT SUM(AOB.Qty) FROM AccountOpenningBreakdown AS AOB WHERE dbo.GetComponentID(AOB.AccountHeadID)=2 AND AOB.IsDr=1 AND AOB.AccountingSessionID=@SessionID AND AOB.BreakdownType=4 AND AOB.BreakdownObjID=@ProductID),0)
				SET @DebitOpeningValue=ISNULL((SELECT SUM(AOB.OpenningBalance) FROM AccountOpenningBreakdown AS AOB WHERE dbo.GetComponentID(AOB.AccountHeadID)=2 AND AOB.IsDr=1 AND AOB.AccountingSessionID=@SessionID AND AOB.BreakdownType=4 AND AOB.BreakdownObjID=@ProductID),0)
				SET @CreditOpeningQty=ISNULL((SELECT SUM(AOB.Qty) FROM AccountOpenningBreakdown AS AOB WHERE dbo.GetComponentID(AOB.AccountHeadID)=2 AND AOB.IsDr=0 AND AOB.AccountingSessionID=@SessionID AND AOB.BreakdownType=4 AND AOB.BreakdownObjID=@ProductID),0)
				SET @CreditOpeningValue=ISNULL((SELECT SUM(AOB.OpenningBalance) FROM AccountOpenningBreakdown AS AOB WHERE dbo.GetComponentID(AOB.AccountHeadID)=2 AND AOB.IsDr=0 AND AOB.AccountingSessionID=@SessionID AND AOB.BreakdownType=4 AND AOB.BreakdownObjID=@ProductID),0)
				SET @DebitQty=ISNULL((SELECT SUM(VPT.Qty) FROM View_VPTransaction AS VPT WHERE dbo.GetComponentID(VPT.AccountHeadID)=2 AND VPT.ApprovedBy!=0 AND  VPT.IsDr=1 AND VPT.ProductID=@ProductID AND CONVERT(DATE, CONVERT(VARCHAR(12),VPT.TransactionDate,106)) BETWEEN  CONVERT(DATE, CONVERT(VARCHAR(12),@SessionStartDate,106)) AND CONVERT(DATE, CONVERT(VARCHAR(12),DATEADD(DAY,-1,@StartDate),106))),0)
				SET @DebitAmount=ISNULL((SELECT SUM(VPT.Amount*VPT.ConversionRate) FROM View_VPTransaction AS VPT WHERE dbo.GetComponentID(VPT.AccountHeadID)=2 AND VPT.ApprovedBy!=0 AND  VPT.IsDr=1 AND VPT.ProductID=@ProductID AND CONVERT(DATE, CONVERT(VARCHAR(12),VPT.TransactionDate,106)) BETWEEN  CONVERT(DATE, CONVERT(VARCHAR(12),@SessionStartDate,106)) AND CONVERT(DATE, CONVERT(VARCHAR(12),DATEADD(DAY,-1,@StartDate),106))),0)
				SET @CreditQty=ISNULL((SELECT SUM(VPT.Qty) FROM View_VPTransaction AS VPT WHERE dbo.GetComponentID(VPT.AccountHeadID)=2 AND VPT.ApprovedBy!=0 AND VPT.IsDr=0 AND VPT.ProductID=@ProductID AND CONVERT(DATE, CONVERT(VARCHAR(12),VPT.TransactionDate,106)) BETWEEN  CONVERT(DATE, CONVERT(VARCHAR(12),@SessionStartDate,106)) AND CONVERT(DATE, CONVERT(VARCHAR(12),DATEADD(DAY,-1,@StartDate),106))),0)
				SET @CreditAmount=ISNULL((SELECT SUM(VPT.Amount*VPT.ConversionRate) FROM View_VPTransaction AS VPT WHERE dbo.GetComponentID(VPT.AccountHeadID)=2 AND VPT.ApprovedBy!=0 AND VPT.IsDr=0 AND VPT.ProductID=@ProductID AND CONVERT(DATE, CONVERT(VARCHAR(12),VPT.TransactionDate,106)) BETWEEN  CONVERT(DATE, CONVERT(VARCHAR(12),@SessionStartDate,106)) AND CONVERT(DATE, CONVERT(VARCHAR(12),DATEADD(DAY,-1,@StartDate),106))),0)
			END
			ELSE
			BEGIN
				SET @DebitOpeningQty=ISNULL((SELECT SUM(AOB.Qty) FROM AccountOpenningBreakdown AS AOB WHERE dbo.GetComponentID(AOB.AccountHeadID)=2 AND AOB.IsDr=1 AND AOB.CurrencyID=@CurrencyID AND AOB.AccountingSessionID=@SessionID AND AOB.BreakdownType=4 AND AOB.BreakdownObjID=@ProductID),0)
				SET @DebitOpeningValue=ISNULL((SELECT SUM(AOB.AmountInCurrency) FROM AccountOpenningBreakdown AS AOB WHERE dbo.GetComponentID(AOB.AccountHeadID)=2 AND AOB.IsDr=1 AND AOB.CurrencyID=@CurrencyID AND AOB.AccountingSessionID=@SessionID AND AOB.BreakdownType=4 AND AOB.BreakdownObjID=@ProductID),0)
				SET @CreditOpeningQty=ISNULL((SELECT SUM(AOB.Qty) FROM AccountOpenningBreakdown AS AOB WHERE dbo.GetComponentID(AOB.AccountHeadID)=2 AND AOB.IsDr=0 AND AOB.CurrencyID=@CurrencyID AND AOB.AccountingSessionID=@SessionID AND AOB.BreakdownType=4 AND AOB.BreakdownObjID=@ProductID),0)
				SET @CreditOpeningValue=ISNULL((SELECT SUM(AOB.AmountInCurrency) FROM AccountOpenningBreakdown AS AOB WHERE dbo.GetComponentID(AOB.AccountHeadID)=2 AND AOB.IsDr=0 AND AOB.CurrencyID=@CurrencyID AND AOB.AccountingSessionID=@SessionID AND AOB.BreakdownType=4 AND AOB.BreakdownObjID=@ProductID),0)
				SET @DebitQty=ISNULL((SELECT SUM(VPT.Qty) FROM View_VPTransaction AS VPT WHERE dbo.GetComponentID(VPT.AccountHeadID)=2 AND  VPT.ApprovedBy!=0 AND VPT.CurrencyID=@CurrencyID AND VPT.IsDr=1 AND VPT.ProductID=@ProductID AND CONVERT(DATE, CONVERT(VARCHAR(12),VPT.TransactionDate,106)) BETWEEN  CONVERT(DATE, CONVERT(VARCHAR(12),@SessionStartDate,106)) AND CONVERT(DATE, CONVERT(VARCHAR(12),DATEADD(DAY,-1,@StartDate),106))),0)
				SET @DebitAmount=ISNULL((SELECT SUM(VPT.Amount*VPT.ConversionRate) FROM View_VPTransaction AS VPT WHERE dbo.GetComponentID(VPT.AccountHeadID)=2 AND  VPT.ApprovedBy!=0 AND VPT.CurrencyID=@CurrencyID AND VPT.IsDr=1 AND VPT.ProductID=@ProductID AND CONVERT(DATE, CONVERT(VARCHAR(12),VPT.TransactionDate,106)) BETWEEN  CONVERT(DATE, CONVERT(VARCHAR(12),@SessionStartDate,106)) AND CONVERT(DATE, CONVERT(VARCHAR(12),DATEADD(DAY,-1,@StartDate),106))),0)
				SET @CreditQty=ISNULL((SELECT SUM(VPT.Qty) FROM View_VPTransaction AS VPT WHERE dbo.GetComponentID(VPT.AccountHeadID)=2 AND  VPT.ApprovedBy!=0 AND VPT.CurrencyID=@CurrencyID AND VPT.IsDr=0 AND VPT.ProductID=@ProductID AND CONVERT(DATE, CONVERT(VARCHAR(12),VPT.TransactionDate,106)) BETWEEN  CONVERT(DATE, CONVERT(VARCHAR(12),@SessionStartDate,106)) AND CONVERT(DATE, CONVERT(VARCHAR(12),DATEADD(DAY,-1,@StartDate),106))),0)
				SET @CreditAmount=ISNULL((SELECT SUM(VPT.Amount*VPT.ConversionRate) FROM View_VPTransaction AS VPT WHERE dbo.GetComponentID(VPT.AccountHeadID)=2 AND  VPT.ApprovedBy!=0 AND VPT.CurrencyID=@CurrencyID AND VPT.IsDr=0 AND VPT.ProductID=@ProductID AND CONVERT(DATE, CONVERT(VARCHAR(12),VPT.TransactionDate,106)) BETWEEN  CONVERT(DATE, CONVERT(VARCHAR(12),@SessionStartDate,106)) AND CONVERT(DATE, CONVERT(VARCHAR(12),DATEADD(DAY,-1,@StartDate),106))),0)
			END
		END
		ELSE
		BEGIN
			IF(@CurrencyID=@BaseCurrencyID)
			BEGIN
				SET @DebitOpeningQty=ISNULL((SELECT SUM(AOB.Qty) FROM AccountOpenningBreakdown AS AOB WHERE dbo.GetComponentID(AOB.AccountHeadID)=2 AND AOB.IsDr=1 AND AOB.AccountHeadID=@AccountHeadID AND AOB.AccountingSessionID=@SessionID AND AOB.BreakdownType=4 AND AOB.BreakdownObjID=@ProductID),0)
				SET @DebitOpeningValue=ISNULL((SELECT SUM(AOB.OpenningBalance) FROM AccountOpenningBreakdown AS AOB WHERE dbo.GetComponentID(AOB.AccountHeadID)=2 AND AOB.IsDr=1 AND AOB.AccountHeadID=@AccountHeadID AND AOB.AccountingSessionID=@SessionID AND AOB.BreakdownType=4 AND AOB.BreakdownObjID=@ProductID),0)
				SET @CreditOpeningQty=ISNULL((SELECT SUM(AOB.Qty) FROM AccountOpenningBreakdown AS AOB WHERE dbo.GetComponentID(AOB.AccountHeadID)=2 AND AOB.IsDr=0 AND AOB.AccountHeadID=@AccountHeadID AND AOB.AccountingSessionID=@SessionID AND AOB.BreakdownType=4 AND AOB.BreakdownObjID=@ProductID),0)
				SET @CreditOpeningValue=ISNULL((SELECT SUM(AOB.OpenningBalance) FROM AccountOpenningBreakdown AS AOB WHERE dbo.GetComponentID(AOB.AccountHeadID)=2 AND AOB.IsDr=0 AND AOB.AccountHeadID=@AccountHeadID AND AOB.AccountingSessionID=@SessionID AND AOB.BreakdownType=4 AND AOB.BreakdownObjID=@ProductID),0)
				SET @DebitQty=ISNULL((SELECT SUM(VPT.Qty) FROM View_VPTransaction AS VPT WHERE dbo.GetComponentID(VPT.AccountHeadID)=2 AND VPT.IsDr=1 AND VPT.ProductID=@ProductID AND VPT.AccountHeadID=@AccountHeadID AND CONVERT(DATE, CONVERT(VARCHAR(12),VPT.TransactionDate,106)) BETWEEN  CONVERT(DATE, CONVERT(VARCHAR(12),@SessionStartDate,106)) AND CONVERT(DATE, CONVERT(VARCHAR(12),DATEADD(DAY,-1,@StartDate),106))),0)
				SET @DebitAmount=ISNULL((SELECT SUM(VPT.Amount*VPT.ConversionRate) FROM View_VPTransaction AS VPT WHERE dbo.GetComponentID(VPT.AccountHeadID)=2 AND VPT.IsDr=1 AND VPT.ProductID=@ProductID AND VPT.AccountHeadID=@AccountHeadID AND CONVERT(DATE, CONVERT(VARCHAR(12),VPT.TransactionDate,106)) BETWEEN  CONVERT(DATE, CONVERT(VARCHAR(12),@SessionStartDate,106)) AND CONVERT(DATE, CONVERT(VARCHAR(12),DATEADD(DAY,-1,@StartDate),106))),0)
				SET @CreditQty=ISNULL((SELECT SUM(VPT.Qty) FROM View_VPTransaction AS VPT WHERE dbo.GetComponentID(VPT.AccountHeadID)=2 AND VPT.IsDr=0 AND VPT.ProductID=@ProductID AND VPT.AccountHeadID=@AccountHeadID AND CONVERT(DATE, CONVERT(VARCHAR(12),VPT.TransactionDate,106)) BETWEEN  CONVERT(DATE, CONVERT(VARCHAR(12),@SessionStartDate,106)) AND CONVERT(DATE, CONVERT(VARCHAR(12),DATEADD(DAY,-1,@StartDate),106))),0)
				SET @CreditAmount=ISNULL((SELECT SUM(VPT.Amount*VPT.ConversionRate) FROM View_VPTransaction AS VPT WHERE dbo.GetComponentID(VPT.AccountHeadID)=2 AND VPT.IsDr=0 AND VPT.ProductID=@ProductID AND VPT.AccountHeadID=@AccountHeadID AND CONVERT(DATE, CONVERT(VARCHAR(12),VPT.TransactionDate,106)) BETWEEN  CONVERT(DATE, CONVERT(VARCHAR(12),@SessionStartDate,106)) AND CONVERT(DATE, CONVERT(VARCHAR(12),DATEADD(DAY,-1,@StartDate),106))),0)
			END
			ELSE
			BEGIN
				SET @DebitOpeningQty=ISNULL((SELECT SUM(AOB.Qty) FROM AccountOpenningBreakdown AS AOB WHERE dbo.GetComponentID(AOB.AccountHeadID)=2 AND AOB.IsDr=1 AND AOB.CurrencyID=@CurrencyID AND AOB.AccountHeadID=@AccountHeadID AND AOB.AccountingSessionID=@SessionID AND AOB.BreakdownType=4 AND AOB.BreakdownObjID=@ProductID),0)
				SET @DebitOpeningValue=ISNULL((SELECT SUM(AOB.AmountInCurrency) FROM AccountOpenningBreakdown AS AOB WHERE dbo.GetComponentID(AOB.AccountHeadID)=2 AND AOB.IsDr=1 AND AOB.CurrencyID=@CurrencyID AND AOB.AccountHeadID=@AccountHeadID AND AOB.AccountingSessionID=@SessionID AND AOB.BreakdownType=4 AND AOB.BreakdownObjID=@ProductID),0)
				SET @CreditOpeningQty=ISNULL((SELECT SUM(AOB.Qty) FROM AccountOpenningBreakdown AS AOB WHERE dbo.GetComponentID(AOB.AccountHeadID)=2 AND AOB.IsDr=0 AND AOB.CurrencyID=@CurrencyID AND AOB.AccountHeadID=@AccountHeadID AND AOB.AccountingSessionID=@SessionID AND AOB.BreakdownType=4 AND AOB.BreakdownObjID=@ProductID),0)
				SET @CreditOpeningValue=ISNULL((SELECT SUM(AOB.AmountInCurrency) FROM AccountOpenningBreakdown AS AOB WHERE dbo.GetComponentID(AOB.AccountHeadID)=2 AND AOB.IsDr=0 AND AOB.CurrencyID=@CurrencyID AND AOB.AccountHeadID=@AccountHeadID AND AOB.AccountingSessionID=@SessionID AND AOB.BreakdownType=4 AND AOB.BreakdownObjID=@ProductID),0)
				SET @DebitQty=ISNULL((SELECT SUM(VPT.Qty) FROM View_VPTransaction AS VPT WHERE dbo.GetComponentID(VPT.AccountHeadID)=2 AND VPT.CurrencyID=@CurrencyID AND VPT.IsDr=1 AND VPT.ProductID=@ProductID AND VPT.AccountHeadID=@AccountHeadID AND CONVERT(DATE, CONVERT(VARCHAR(12),VPT.TransactionDate,106)) BETWEEN  CONVERT(DATE, CONVERT(VARCHAR(12),@SessionStartDate,106)) AND CONVERT(DATE, CONVERT(VARCHAR(12),DATEADD(DAY,-1,@StartDate),106))),0)
				SET @DebitAmount=ISNULL((SELECT SUM(VPT.Amount*VPT.ConversionRate) FROM View_VPTransaction AS VPT WHERE dbo.GetComponentID(VPT.AccountHeadID)=2 AND VPT.CurrencyID=@CurrencyID AND VPT.IsDr=1 AND VPT.ProductID=@ProductID AND VPT.AccountHeadID=@AccountHeadID AND CONVERT(DATE, CONVERT(VARCHAR(12),VPT.TransactionDate,106)) BETWEEN  CONVERT(DATE, CONVERT(VARCHAR(12),@SessionStartDate,106)) AND CONVERT(DATE, CONVERT(VARCHAR(12),DATEADD(DAY,-1,@StartDate),106))),0)
				SET @CreditQty=ISNULL((SELECT SUM(VPT.Amount*VPT.ConversionRate) FROM View_VPTransaction AS VPT WHERE dbo.GetComponentID(VPT.AccountHeadID)=2 AND VPT.CurrencyID=@CurrencyID AND VPT.IsDr=0 AND VPT.ProductID=@ProductID AND VPT.AccountHeadID=@AccountHeadID AND CONVERT(DATE, CONVERT(VARCHAR(12),VPT.TransactionDate,106)) BETWEEN  CONVERT(DATE, CONVERT(VARCHAR(12),@SessionStartDate,106)) AND CONVERT(DATE, CONVERT(VARCHAR(12),DATEADD(DAY,-1,@StartDate),106))),0)
				SET @CreditAmount=ISNULL((SELECT SUM(VPT.Amount*VPT.ConversionRate) FROM View_VPTransaction AS VPT WHERE dbo.GetComponentID(VPT.AccountHeadID)=2 AND VPT.CurrencyID=@CurrencyID AND VPT.IsDr=0 AND VPT.ProductID=@ProductID AND VPT.AccountHeadID=@AccountHeadID AND CONVERT(DATE, CONVERT(VARCHAR(12),VPT.TransactionDate,106)) BETWEEN  CONVERT(DATE, CONVERT(VARCHAR(12),@SessionStartDate,106)) AND CONVERT(DATE, CONVERT(VARCHAR(12),DATEADD(DAY,-1,@StartDate),106))),0)
			END
		END
	END
END

IF(@ComponentID IN(2,6))
BEGIN
	SET @ClosingValue=@DebitOpeningValue-@CreditOpeningValue+@DebitAmount-@CreditAmount
	SET @ClosingQty=@DebitOpeningQty-@CreditOpeningQty+@DebitQty-@CreditQty
END
ELSE
BEGIN
	SET @ClosingValue=@CreditOpeningValue-@DebitOpeningValue-@DebitAmount+@CreditAmount
	SET @ClosingQty=@CreditOpeningQty-@DebitOpeningQty-@DebitQty+@CreditQty
END

INSERT INTO #TempTable (ProductID,	ProductCode,ProductName,	OpeiningValue,	OpeiningQty,	IsDebit,	DebitAmount,	DebitQty,	CreditAmount,	CreditQty,	ClosingValue,	ClosingQty,	IsDrClosing,	CurrencySymbol, VoucherID,	VoucherDate,	VoucherNo,	AccountHeadName)
				VALUES (0,			'',			'',				0.00,			0.00,				0,			0.00,		0.00,		0.00,			0.00,		@ClosingValue,	@ClosingQty,0,				'',				0,			@StartDate,		'',			'Opening Balance')

IF(@BusinessUnitID>0)
BEGIN
	IF(@AccountHeadID>0)
	BEGIN
		IF(@IsApproved=1)
		BEGIN
			IF(@CurrencyID=@BaseCurrencyID)
			BEGIN
				INSERT INTO #TempTable(ProductID,			ParentHeadID,		VoucherDetailID,	Description)--Note ProductID Contain VPTID That means VPTransaction PK
								SELECT VPT.VPTransactionID,	VPT.AccountHeadID,	VPT.VoucherDetailID,VPT.Description FROM View_VPTransaction AS VPT WHERE dbo.GetComponentID(VPT.AccountHeadID)=2 AND VPT.BUID=@BusinessUnitID AND VPT.ProductID=@ProductID AND VPT.AccountHeadID=@AccountHeadID AND ISNULL(VPT.ApprovedBy,0)!=0 AND CONVERT(DATE, CONVERT(VARCHAR(12),VPT.TransactionDate,106)) BETWEEN  CONVERT(DATE, CONVERT(VARCHAR(12),@StartDate,106)) AND CONVERT(DATE, CONVERT(VARCHAR(12),@EndDate,106))
			END
			ELSE
			BEGIN
				INSERT INTO #TempTable(ProductID,			ParentHeadID,		VoucherDetailID,	Description)--Note ProductID Contain VPTID That means VPTransaction PK
								SELECT VPT.VPTransactionID,	VPT.AccountHeadID,	VPT.VoucherDetailID,VPT.Description FROM View_VPTransaction AS VPT WHERE dbo.GetComponentID(VPT.AccountHeadID)=2 AND VPT.BUID=@BusinessUnitID AND VPT.CurrencyID=@CurrencyID AND VPT.ProductID=@ProductID AND VPT.AccountHeadID=@AccountHeadID AND ISNULL(VPT.ApprovedBy,0)!=0 AND CONVERT(DATE, CONVERT(VARCHAR(12),VPT.TransactionDate,106)) BETWEEN  CONVERT(DATE, CONVERT(VARCHAR(12),@StartDate,106)) AND CONVERT(DATE, CONVERT(VARCHAR(12),@EndDate,106))
			END
		END
		ELSE
		BEGIN
			IF(@CurrencyID=@BaseCurrencyID)
			BEGIN
				INSERT INTO #TempTable(ProductID,			ParentHeadID,		VoucherDetailID,	Description)--Note ProductID Contain VPTID That means VPTransaction PK
								SELECT VPT.VPTransactionID,	VPT.AccountHeadID,	VPT.VoucherDetailID,VPT.Description FROM View_VPTransaction AS VPT WHERE dbo.GetComponentID(VPT.AccountHeadID)=2 AND VPT.BUID=@BusinessUnitID AND VPT.ProductID=@ProductID AND VPT.AccountHeadID=@AccountHeadID AND CONVERT(DATE, CONVERT(VARCHAR(12),VPT.TransactionDate,106)) BETWEEN  CONVERT(DATE, CONVERT(VARCHAR(12),@StartDate,106)) AND CONVERT(DATE, CONVERT(VARCHAR(12),@EndDate,106))
			END
			ELSE
			BEGIN
				INSERT INTO #TempTable(ProductID,			ParentHeadID,		VoucherDetailID,	Description)--Note ProductID Contain VPTID That means VPTransaction PK
								SELECT VPT.VPTransactionID,	VPT.AccountHeadID,	VPT.VoucherDetailID,VPT.Description FROM View_VPTransaction AS VPT WHERE dbo.GetComponentID(VPT.AccountHeadID)=2 AND VPT.BUID=@BusinessUnitID AND VPT.CurrencyID=@CurrencyID AND VPT.ProductID=@ProductID AND VPT.AccountHeadID=@AccountHeadID AND CONVERT(DATE, CONVERT(VARCHAR(12),VPT.TransactionDate,106)) BETWEEN  CONVERT(DATE, CONVERT(VARCHAR(12),@StartDate,106)) AND CONVERT(DATE, CONVERT(VARCHAR(12),@EndDate,106))
			END
		END
	END
	ELSE
	BEGIN
		IF(@IsApproved=1)
		BEGIN
			IF(@CurrencyID=@BaseCurrencyID)
			BEGIN
				INSERT INTO #TempTable(ProductID,			ParentHeadID,		VoucherDetailID,	Description)--Note ProductID Contain VPTID That means VPTransaction PK
								SELECT VPT.VPTransactionID,	VPT.AccountHeadID,	VPT.VoucherDetailID,VPT.Description FROM View_VPTransaction AS VPT WHERE dbo.GetComponentID(VPT.AccountHeadID)=2 AND VPT.BUID=@BusinessUnitID AND VPT.ProductID=@ProductID AND ISNULL(VPT.ApprovedBy,0)!=0 AND CONVERT(DATE, CONVERT(VARCHAR(12),VPT.TransactionDate,106)) BETWEEN  CONVERT(DATE, CONVERT(VARCHAR(12),@StartDate,106)) AND CONVERT(DATE, CONVERT(VARCHAR(12),@EndDate,106))
			END
			ELSE
			BEGIN
				INSERT INTO #TempTable(ProductID,			ParentHeadID,		VoucherDetailID,	Description)--Note ProductID Contain VPTID That means VPTransaction PK
								SELECT VPT.VPTransactionID,	VPT.AccountHeadID,	VPT.VoucherDetailID,VPT.Description FROM View_VPTransaction AS VPT WHERE dbo.GetComponentID(VPT.AccountHeadID)=2 AND VPT.BUID=@BusinessUnitID AND VPT.CurrencyID=@CurrencyID AND VPT.ProductID=@ProductID AND ISNULL(VPT.ApprovedBy,0)!=0 AND CONVERT(DATE, CONVERT(VARCHAR(12),VPT.TransactionDate,106)) BETWEEN  CONVERT(DATE, CONVERT(VARCHAR(12),@StartDate,106)) AND CONVERT(DATE, CONVERT(VARCHAR(12),@EndDate,106))
			END
		END
		ELSE
		BEGIN
			IF(@CurrencyID=@BaseCurrencyID)
			BEGIN
				INSERT INTO #TempTable(ProductID,			ParentHeadID,		VoucherDetailID,	Description)--Note ProductID Contain VPTID That means VPTransaction PK
								SELECT VPT.VPTransactionID,	VPT.AccountHeadID,	VPT.VoucherDetailID,VPT.Description FROM View_VPTransaction AS VPT WHERE dbo.GetComponentID(VPT.AccountHeadID)=2 AND VPT.BUID=@BusinessUnitID AND VPT.ProductID=@ProductID AND CONVERT(DATE, CONVERT(VARCHAR(12),VPT.TransactionDate,106)) BETWEEN  CONVERT(DATE, CONVERT(VARCHAR(12),@StartDate,106)) AND CONVERT(DATE, CONVERT(VARCHAR(12),@EndDate,106))
			END
			ELSE
			BEGIN
				INSERT INTO #TempTable(ProductID,			ParentHeadID,		VoucherDetailID,	Description)--Note ProductID Contain VPTID That means VPTransaction PK
								SELECT VPT.VPTransactionID,	VPT.AccountHeadID,	VPT.VoucherDetailID,VPT.Description FROM View_VPTransaction AS VPT WHERE dbo.GetComponentID(VPT.AccountHeadID)=2 AND VPT.BUID=@BusinessUnitID AND VPT.CurrencyID=@CurrencyID AND VPT.ProductID=@ProductID AND CONVERT(DATE, CONVERT(VARCHAR(12),VPT.TransactionDate,106)) BETWEEN  CONVERT(DATE, CONVERT(VARCHAR(12),@StartDate,106)) AND CONVERT(DATE, CONVERT(VARCHAR(12),@EndDate,106))
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
				INSERT INTO #TempTable(ProductID,			ParentHeadID,		VoucherDetailID,	Description)--Note ProductID Contain VPTID That means VPTransaction PK
								SELECT VPT.VPTransactionID,	VPT.AccountHeadID,	VPT.VoucherDetailID,VPT.Description FROM View_VPTransaction AS VPT WHERE dbo.GetComponentID(VPT.AccountHeadID)=2 AND VPT.ProductID=@ProductID AND VPT.AccountHeadID=@AccountHeadID AND ISNULL(VPT.ApprovedBy,0)!=0 AND CONVERT(DATE, CONVERT(VARCHAR(12),VPT.TransactionDate,106)) BETWEEN  CONVERT(DATE, CONVERT(VARCHAR(12),@StartDate,106)) AND CONVERT(DATE, CONVERT(VARCHAR(12),@EndDate,106))
			END
			ELSE
			BEGIN
				INSERT INTO #TempTable(ProductID,			ParentHeadID,		VoucherDetailID,	Description)--Note ProductID Contain VPTID That means VPTransaction PK
								SELECT VPT.VPTransactionID,	VPT.AccountHeadID,	VPT.VoucherDetailID,VPT.Description FROM View_VPTransaction AS VPT WHERE dbo.GetComponentID(VPT.AccountHeadID)=2 AND VPT.CurrencyID=@CurrencyID AND VPT.ProductID=@ProductID AND VPT.AccountHeadID=@AccountHeadID AND ISNULL(VPT.ApprovedBy,0)!=0 AND CONVERT(DATE, CONVERT(VARCHAR(12),VPT.TransactionDate,106)) BETWEEN  CONVERT(DATE, CONVERT(VARCHAR(12),@StartDate,106)) AND CONVERT(DATE, CONVERT(VARCHAR(12),@EndDate,106))
			END
		END
		ELSE
		BEGIN
			IF(@CurrencyID=@BaseCurrencyID)
			BEGIN
				INSERT INTO #TempTable(ProductID,			ParentHeadID,		VoucherDetailID,	Description)--Note ProductID Contain VPTID That means VPTransaction PK
								SELECT VPT.VPTransactionID,	VPT.AccountHeadID,	VPT.VoucherDetailID,VPT.Description FROM View_VPTransaction AS VPT WHERE dbo.GetComponentID(VPT.AccountHeadID)=2 AND VPT.ProductID=@ProductID AND VPT.AccountHeadID=@AccountHeadID AND CONVERT(DATE, CONVERT(VARCHAR(12),VPT.TransactionDate,106)) BETWEEN  CONVERT(DATE, CONVERT(VARCHAR(12),@StartDate,106)) AND CONVERT(DATE, CONVERT(VARCHAR(12),@EndDate,106))
			END
			ELSE
			BEGIN
				INSERT INTO #TempTable(ProductID,			ParentHeadID,		VoucherDetailID,	Description)--Note ProductID Contain VPTID That means VPTransaction PK
								SELECT VPT.VPTransactionID,	VPT.AccountHeadID,	VPT.VoucherDetailID,VPT.Description FROM View_VPTransaction AS VPT WHERE dbo.GetComponentID(VPT.AccountHeadID)=2 AND VPT.CurrencyID=@CurrencyID AND VPT.ProductID=@ProductID AND VPT.AccountHeadID=@AccountHeadID AND CONVERT(DATE, CONVERT(VARCHAR(12),VPT.TransactionDate,106)) BETWEEN  CONVERT(DATE, CONVERT(VARCHAR(12),@StartDate,106)) AND CONVERT(DATE, CONVERT(VARCHAR(12),@EndDate,106))
			END
		END
	END
	ELSE
	BEGIN
		IF(@IsApproved=1)
		BEGIN
			IF(@CurrencyID=@BaseCurrencyID)
			BEGIN
				INSERT INTO #TempTable(ProductID,			ParentHeadID,		VoucherDetailID,	Description)--Note ProductID Contain VPTID That means VPTransaction PK
								SELECT VPT.VPTransactionID,	VPT.AccountHeadID,	VPT.VoucherDetailID,VPT.Description FROM View_VPTransaction AS VPT WHERE dbo.GetComponentID(VPT.AccountHeadID)=2 AND VPT.ProductID=@ProductID AND ISNULL(VPT.ApprovedBy,0)!=0 AND CONVERT(DATE, CONVERT(VARCHAR(12),VPT.TransactionDate,106)) BETWEEN  CONVERT(DATE, CONVERT(VARCHAR(12),@StartDate,106)) AND CONVERT(DATE, CONVERT(VARCHAR(12),@EndDate,106))
			END
			ELSE
			BEGIN
				INSERT INTO #TempTable(ProductID,			ParentHeadID,		VoucherDetailID,	Description)--Note ProductID Contain VPTID That means VPTransaction PK
								SELECT VPT.VPTransactionID,	VPT.AccountHeadID,	VPT.VoucherDetailID,VPT.Description FROM View_VPTransaction AS VPT WHERE dbo.GetComponentID(VPT.AccountHeadID)=2 AND VPT.CurrencyID=@CurrencyID AND VPT.ProductID=@ProductID AND ISNULL(VPT.ApprovedBy,0)!=0 AND CONVERT(DATE, CONVERT(VARCHAR(12),VPT.TransactionDate,106)) BETWEEN  CONVERT(DATE, CONVERT(VARCHAR(12),@StartDate,106)) AND CONVERT(DATE, CONVERT(VARCHAR(12),@EndDate,106))
			END
		END
		ELSE
		BEGIN
			IF(@CurrencyID=@BaseCurrencyID)
			BEGIN
				INSERT INTO #TempTable(ProductID,			ParentHeadID,		VoucherDetailID,	Description)--Note ProductID Contain VPTID That means VPTransaction PK
								SELECT VPT.VPTransactionID,	VPT.AccountHeadID,	VPT.VoucherDetailID,VPT.Description FROM View_VPTransaction AS VPT WHERE dbo.GetComponentID(VPT.AccountHeadID)=2 AND VPT.ProductID=@ProductID AND CONVERT(DATE, CONVERT(VARCHAR(12),VPT.TransactionDate,106)) BETWEEN  CONVERT(DATE, CONVERT(VARCHAR(12),@StartDate,106)) AND CONVERT(DATE, CONVERT(VARCHAR(12),@EndDate,106))
			END
			ELSE
			BEGIN
				INSERT INTO #TempTable(ProductID,			ParentHeadID,		VoucherDetailID,	Description)--Note ProductID Contain VPTID That means VPTransaction PK
								SELECT VPT.VPTransactionID,	VPT.AccountHeadID,	VPT.VoucherDetailID,VPT.Description FROM View_VPTransaction AS VPT WHERE dbo.GetComponentID(VPT.AccountHeadID)=2 AND VPT.CurrencyID=@CurrencyID AND VPT.ProductID=@ProductID AND CONVERT(DATE, CONVERT(VARCHAR(12),VPT.TransactionDate,106)) BETWEEN  CONVERT(DATE, CONVERT(VARCHAR(12),@StartDate,106)) AND CONVERT(DATE, CONVERT(VARCHAR(12),@EndDate,106))
			END
		END
	END
END

UPDATE #TempTable
SET VoucherID=(SELECT VD.VoucherID FROM View_VoucherDetail AS VD WHERE VD.VoucherDetailID=TT.VoucherDetailID),
	IsDebit=(SELECT VPT.IsDr FROM View_VPTransaction AS VPT WHERE VPT.VPTransactionID=TT.ProductID)	
FROM #TempTable AS TT

IF(@CurrencyID=@BaseCurrencyID)
BEGIN
	UPDATE #TempTable
	SET DebitQty=ISNULL((SELECT (VPT.Qty) FROM VPTransaction AS VPT WHERE VPT.VPTransactionID=TT.ProductID),0),
		DebitAmount=ISNULL((SELECT (VPT.Amount*VPT.ConversionRate) FROM VPTransaction AS VPT WHERE VPT.VPTransactionID=TT.ProductID),0),
		CreditQty=0.00,
		CreditAmount=0.00
	FROM #TempTable AS TT WHERE TT.IsDebit=1

	UPDATE #TempTable
	SET DebitQty=0.00,
		DebitAmount=0.00,
		CreditQty=(SELECT (VPT.Qty) FROM VPTransaction AS VPT WHERE VPT.VPTransactionID=TT.ProductID),
		CreditAmount=(SELECT (VPT.Amount*VPT.ConversionRate) FROM VPTransaction AS VPT WHERE VPT.VPTransactionID=TT.ProductID)
	FROM #TempTable AS TT WHERE TT.IsDebit=0
END
ELSE
BEGIN
	UPDATE #TempTable
	SET DebitQty=ISNULL((SELECT VPT.Qty FROM VPTransaction AS VPT WHERE VPT.VPTransactionID=TT.ProductID),0),
		DebitAmount=ISNULL((SELECT VPT.Amount FROM VPTransaction AS VPT WHERE VPT.VPTransactionID=TT.ProductID),0),
		CreditQty=0.00,
		CreditAmount=0.00
	FROM #TempTable AS TT WHERE TT.IsDebit=1

	UPDATE #TempTable
	SET DebitQty=0.00,
		DebitAmount=0.00,
		CreditQty=ISNULL((SELECT VPT.Qty FROM VPTransaction AS VPT WHERE VPT.VPTransactionID=TT.ProductID),0),
		CreditAmount=ISNULL((SELECT VPT.Amount FROM VPTransaction AS VPT WHERE VPT.VPTransactionID=TT.ProductID),0)
	FROM #TempTable AS TT WHERE TT.IsDebit=0
END

UPDATE #TempTable
SET AccountHeadName=ISNULL((SELECT TOP(1)AccountHeadName FROM View_VoucherDetail WHERE  IsDebit!=TT.IsDebit AND  AccountHeadID<>@AccountHeadID AND  VoucherID=TT.VoucherID),''),
	ParentHeadName=ISNULL((SELECT COA.AccountHeadName FROM View_ChartsOfAccount AS COA WHERE  COA.AccountHeadID=TT.ParentHeadID),''),
	ParentHeadCode=ISNULL((SELECT COA.AccountCode FROM View_ChartsOfAccount AS COA WHERE  COA.AccountHeadID=TT.ParentHeadID),''),
	VoucherDate=(SELECT V.VoucherDate FROM Voucher AS V WHERE V.VoucherID=TT.VoucherID),
	VoucherNo=(SELECT V.VoucherNo FROM Voucher AS V WHERE V.VoucherID=TT.VoucherID),
	Narration=(SELECT V.Narration FROM Voucher AS V WHERE V.VoucherID=TT.VoucherID),
	VoucherNarration=(SELECT VD.Narration FROM VoucherDetail AS VD WHERE VD.VoucherDetailID=TT.VoucherDetailID)
FROM #TempTable AS TT WHERE TT.ID!=1


INSERT INTO #ResultTable(ProductID,		ProductCode,		ProductName,		OpeiningValue,		OpeiningQty,		IsDebit,		DebitAmount,	DebitQty,	CreditAmount,	CreditQty,		ClosingValue,	ClosingQty,		IsDrClosing,		CurrencySymbol,		VoucherID,		VoucherDate,		VoucherNo,		AccountHeadName,	ParentHeadID,	ParentHeadName,		ParentHeadCode,		VoucherDetailID,	Narration,		VoucherNarration,	Description)
				  SELECT TT.ProductID,	TT.ProductCode,		TT.ProductName,		TT.OpeiningValue,	TT.OpeiningQty,		TT.IsDebit,		TT.DebitAmount,	TT.DebitQty,TT.CreditAmount,TT.CreditQty,	TT.ClosingValue,TT.ClosingQty,	TT.IsDrClosing,		TT.CurrencySymbol,	TT.VoucherID,	TT.VoucherDate,		TT.VoucherNo,	TT.AccountHeadName,	TT.ParentHeadID,TT.ParentHeadName,	TT.ParentHeadCode,	TT.VoucherDetailID,	TT.Narration,	TT.VoucherNarration,TT.Description FROM #TempTable  AS TT ORDER BY TT.VoucherDate ASC

DECLARE
@ID as int,
@Count as int,
@IsDebit as bit,
@Amount as decimal(30,17),
@Qty as decimal(30,17),
@CurrentBalance as decimal(30,17),
@CurrentQty as decimal(30,17)

SET @ID=2
SET @Count=(SELECT COUNT(*) FROM #ResultTable)
SET @CurrentBalance =@ClosingValue
SET @CurrentQty =@ClosingQty

IF(@ComponentID IN(2,6))--ComponentID{Asset = 2,Laibility = 3,OwnerEquity=4,Income = 5,Expeness = 6}
BEGIN
	WHILE(@Count>=@ID)
	BEGIN
		SET @IsDebit=0
		SET @IsDebit=(SELECT TT.IsDebit FROM #ResultTable AS TT WHERE TT.ID=@ID)
		IF(@IsDebit=1)
		BEGIN
			SET @Amount=(SELECT TT.DebitAmount FROM #ResultTable AS TT WHERE TT.ID=@ID)		
			SET @Qty=(SELECT TT.DebitQty FROM #ResultTable AS TT WHERE TT.ID=@ID)		
			SET @CurrentBalance=@CurrentBalance+@Amount
			SET @CurrentQty=@CurrentQty+@Qty
		END
		ELSE
		BEGIN
			SET @Amount=(SELECT TT.CreditAmount FROM #ResultTable AS TT WHERE TT.ID=@ID)
			SET @Qty=(SELECT TT.CreditQty FROM #ResultTable AS TT WHERE TT.ID=@ID)
			SET @CurrentBalance=@CurrentBalance-@Amount
			SET @CurrentQty=@CurrentQty-@Qty
		END
		UPDATE #ResultTable SET ClosingValue=@CurrentBalance, ClosingQty=@CurrentQty WHERE ID=@ID
		SET @ID=@ID+1
	END
END		
ELSE
BEGIN
	WHILE(@Count>=@ID)
	BEGIN
		SET @IsDebit=0
		SET @IsDebit=(SELECT TT.IsDebit FROM #ResultTable AS TT WHERE TT.ID=@ID)
		IF(@IsDebit=1)
		BEGIN
			SET @Amount=(SELECT TT.DebitAmount FROM #ResultTable AS TT WHERE TT.ID=@ID)		
			SET @Qty=(SELECT TT.DebitQty FROM #ResultTable AS TT WHERE TT.ID=@ID)		
			SET @CurrentBalance=@CurrentBalance-@Amount
			SET @CurrentQty=@CurrentQty-@Qty
		END
		ELSE
		BEGIN
			SET @Amount=(SELECT TT.CreditAmount FROM #ResultTable AS TT WHERE TT.ID=@ID)
			SET @Qty=(SELECT TT.CreditQty FROM #ResultTable AS TT WHERE TT.ID=@ID)
			SET @CurrentBalance=@CurrentBalance+@Amount
			SET @CurrentQty=@CurrentQty+@Qty
		END
		UPDATE #ResultTable SET ClosingValue=@CurrentBalance, ClosingQty=@CurrentQty WHERE ID=@ID
		SET @ID=@ID+1
	END
END

SELECT * FROM #ResultTable ORDER BY VoucherDate ASC
DROP TABLE #TempTable
DROP TABLE #ResultTable
COMMIT TRAN






GO
/****** Object:  StoredProcedure [dbo].[SP_VPTransactionSummary]    Script Date: 12/1/2016 2:17:46 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[SP_VPTransactionSummary]
(
	@BusinessUnitID as int,
	@AccountHeadID as int,
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
--@CurrencyID as int,
--@StartDate date,
--@EndDate date,
--@IsApproved as bit

--SET @BusinessUnitID =0
--SET @AccountHeadID =0
--SET @CurrencyID =0
--SET @StartDate ='01 jan 2015'
--SET @EndDate ='31 dec 2016'
--SET @IsApproved =0

CREATE TABLE #TempTable(
							ID int IDENTITY(1,1) PRIMARY KEY,
							ProductID int,
							ProductCode Varchar(512),
							ProductName Varchar(512),
							OpeiningValue decimal(30,17),
							OpeiningQty decimal(30,17),
							IsDebit bit,
							DebitAmount decimal(30,17),
							DebitQty decimal(30,17),
							CreditAmount decimal(30,17),
							CreditQty decimal(30,17),
							ClosingValue decimal(30,17),
							ClosingQty decimal(30,17),
							IsDrClosing bit,
							CurrencySymbol Varchar(512),
							VoucherID int,
							VoucherDate date,
							VoucherNo Varchar(512),
							ParentHeadID INT,
							AccountHeadName Varchar(512),
							ParentHeadName varchar(512),
							ParentHeadCode varchar(512),
							VoucherDetailID int,
							Narration VARCHAR(512),
							VoucherNarration VARCHAR(512),
							Description VARCHAR(512)
						)

CREATE TABLE #TempTable2(
							ProductID int,							
							DebitOpeiningValue decimal(30,17),
							CreditOpeiningValue decimal(30,17),
							DebitOpeiningQty decimal(30,17),
							CreditOpeiningQty decimal(30,17),
							IsDebit bit,
							DebitAmount decimal(30,17),
							DebitQty decimal(30,17),
							CreditAmount decimal(30,17),
							CreditQty decimal(30,17),
							ClosingValue decimal(30,17),
							ClosingQty decimal(30,17),
							IsDrClosing bit
						)

DECLARE
@SessionID as int,
@SessionStartDate as date,
@BaseCurrencyID as int,
@ComponentID as int

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

IF(@BusinessUnitID>0)
BEGIN
	IF(@AccountHeadID>0)
	BEGIN
		IF(@IsApproved=1)
		BEGIN
			IF(@CurrencyID=@BaseCurrencyID)
			BEGIN
			INSERT INTO #TempTable (ProductID,	ParentHeadID)
			SELECT DISTINCT AOB.BreakdownObjID,	@AccountHeadID FROM AccountOpenningBreakdown AS AOB WHERE dbo.GetComponentID(AOB.AccountHeadID)=2 AND AOB.BusinessUnitID=@BusinessUnitID AND AOB.AccountHeadID=@AccountHeadID AND AOB.AccountingSessionID=@SessionID AND AOB.BreakdownType=4 AND ISNULL(AOB.BreakdownObjID,0)!=0

			INSERT INTO #TempTable (ProductID,	ParentHeadID)
			SELECT DISTINCT VPT.ProductID,	@AccountHeadID FROM View_VPTransaction AS VPT WHERE dbo.GetComponentID(VPT.AccountHeadID)=2 AND  VPT.BUID=@BusinessUnitID AND ISNULL(VPT.ApprovedBy,0)!=0 AND VPT.AccountHeadID=@AccountHeadID AND CONVERT(DATE, CONVERT(VARCHAR(12),VPT.TransactionDate,106)) BETWEEN  CONVERT(DATE, CONVERT(VARCHAR(12),@SessionStartDate,106)) AND CONVERT(DATE, CONVERT(VARCHAR(12),@EndDate,106))  AND VPT.ProductID IN (SELECT P.ProductID FROM Product AS P WHERE P.ProductType=1)
		END
			ELSE
			BEGIN
			INSERT INTO #TempTable (ProductID,	ParentHeadID)
			SELECT DISTINCT AOB.BreakdownObjID,	@AccountHeadID FROM AccountOpenningBreakdown AS AOB WHERE dbo.GetComponentID(AOB.AccountHeadID)=2 AND AOB.BusinessUnitID=@BusinessUnitID AND AOB.CurrencyID=@CurrencyID AND AOB.AccountHeadID=@AccountHeadID AND AOB.AccountingSessionID=@SessionID AND AOB.BreakdownType=4 AND ISNULL(AOB.BreakdownObjID,0)!=0

			INSERT INTO #TempTable (ProductID,	ParentHeadID)
			SELECT DISTINCT VPT.ProductID,	@AccountHeadID FROM View_VPTransaction AS VPT WHERE dbo.GetComponentID(VPT.AccountHeadID)=2 AND  VPT.BUID=@BusinessUnitID AND ISNULL(VPT.ApprovedBy,0)!=0  AND VPT.CurrencyID=@CurrencyID AND VPT.AccountHeadID=@AccountHeadID AND CONVERT(DATE, CONVERT(VARCHAR(12),VPT.TransactionDate,106)) BETWEEN  CONVERT(DATE, CONVERT(VARCHAR(12),@SessionStartDate,106)) AND CONVERT(DATE, CONVERT(VARCHAR(12),@EndDate,106))  AND VPT.ProductID IN (SELECT P.ProductID FROM Product AS P WHERE P.ProductType=1)
		END
		END
		ELSE
		BEGIN
			IF(@CurrencyID=@BaseCurrencyID)
			BEGIN
			INSERT INTO #TempTable (ProductID,	ParentHeadID)
			SELECT DISTINCT AOB.BreakdownObjID,	@AccountHeadID FROM AccountOpenningBreakdown AS AOB WHERE dbo.GetComponentID(AOB.AccountHeadID)=2 AND AOB.BusinessUnitID=@BusinessUnitID AND AOB.AccountHeadID=@AccountHeadID AND AOB.AccountingSessionID=@SessionID AND AOB.BreakdownType=4 AND ISNULL(AOB.BreakdownObjID,0)!=0

			INSERT INTO #TempTable (ProductID,	ParentHeadID)
			SELECT DISTINCT VPT.ProductID,	@AccountHeadID FROM View_VPTransaction AS VPT WHERE dbo.GetComponentID(VPT.AccountHeadID)=2 AND  VPT.BUID=@BusinessUnitID AND VPT.AccountHeadID=@AccountHeadID AND CONVERT(DATE, CONVERT(VARCHAR(12),VPT.TransactionDate,106)) BETWEEN  CONVERT(DATE, CONVERT(VARCHAR(12),@SessionStartDate,106)) AND CONVERT(DATE, CONVERT(VARCHAR(12),@EndDate,106))  AND VPT.ProductID IN (SELECT P.ProductID FROM Product AS P WHERE P.ProductType=1)
		END
			ELSE
			BEGIN
			INSERT INTO #TempTable (ProductID,	ParentHeadID)
			SELECT DISTINCT AOB.BreakdownObjID,	@AccountHeadID FROM AccountOpenningBreakdown AS AOB WHERE dbo.GetComponentID(AOB.AccountHeadID)=2 AND AOB.BusinessUnitID=@BusinessUnitID AND AOB.CurrencyID=@CurrencyID AND AOB.AccountHeadID=@AccountHeadID AND AOB.AccountingSessionID=@SessionID AND AOB.BreakdownType=4 AND ISNULL(AOB.BreakdownObjID,0)!=0

			INSERT INTO #TempTable (ProductID,	ParentHeadID)
			SELECT DISTINCT VPT.ProductID,	@AccountHeadID FROM View_VPTransaction AS VPT WHERE dbo.GetComponentID(VPT.AccountHeadID)=2 AND  VPT.BUID=@BusinessUnitID AND VPT.CurrencyID=@CurrencyID AND VPT.AccountHeadID=@AccountHeadID AND CONVERT(DATE, CONVERT(VARCHAR(12),VPT.TransactionDate,106)) BETWEEN  CONVERT(DATE, CONVERT(VARCHAR(12),@SessionStartDate,106)) AND CONVERT(DATE, CONVERT(VARCHAR(12),@EndDate,106))  AND VPT.ProductID IN (SELECT P.ProductID FROM Product AS P WHERE P.ProductType=1)
		END
		END
	END
	ELSE
	BEGIN
	IF(@IsApproved=1)
	BEGIN
		IF(@CurrencyID=@BaseCurrencyID)
		BEGIN
			INSERT INTO #TempTable (ProductID,	ParentHeadID)
			SELECT DISTINCT AOB.BreakdownObjID,	@AccountHeadID FROM AccountOpenningBreakdown AS AOB WHERE dbo.GetComponentID(AOB.AccountHeadID)=2 AND AOB.BusinessUnitID=@BusinessUnitID AND AOB.AccountingSessionID=@SessionID AND AOB.BreakdownType=4 AND ISNULL(AOB.BreakdownObjID,0)!=0

			INSERT INTO #TempTable (ProductID,	ParentHeadID)
			SELECT DISTINCT VPT.ProductID,	@AccountHeadID FROM View_VPTransaction AS VPT WHERE dbo.GetComponentID(VPT.AccountHeadID)=2 AND  VPT.BUID=@BusinessUnitID AND ISNULL(VPT.ApprovedBy,0)!=0 AND CONVERT(DATE, CONVERT(VARCHAR(12),VPT.TransactionDate,106)) BETWEEN  CONVERT(DATE, CONVERT(VARCHAR(12),@SessionStartDate,106)) AND CONVERT(DATE, CONVERT(VARCHAR(12),@EndDate,106))  AND VPT.ProductID IN (SELECT P.ProductID FROM Product AS P WHERE P.ProductType=1)
		END
		ELSE
		BEGIN
			INSERT INTO #TempTable (ProductID,	ParentHeadID)
			SELECT DISTINCT AOB.BreakdownObjID,	@AccountHeadID FROM AccountOpenningBreakdown AS AOB WHERE dbo.GetComponentID(AOB.AccountHeadID)=2 AND AOB.BusinessUnitID=@BusinessUnitID AND AOB.CurrencyID=@CurrencyID AND AOB.AccountingSessionID=@SessionID AND AOB.BreakdownType=4 AND ISNULL(AOB.BreakdownObjID,0)!=0

			INSERT INTO #TempTable (ProductID,	ParentHeadID)
			SELECT DISTINCT VPT.ProductID,	@AccountHeadID FROM View_VPTransaction AS VPT WHERE dbo.GetComponentID(VPT.AccountHeadID)=2 AND  VPT.BUID=@BusinessUnitID AND ISNULL(VPT.ApprovedBy,0)!=0  AND VPT.CurrencyID=@CurrencyID AND CONVERT(DATE, CONVERT(VARCHAR(12),VPT.TransactionDate,106)) BETWEEN  CONVERT(DATE, CONVERT(VARCHAR(12),@SessionStartDate,106)) AND CONVERT(DATE, CONVERT(VARCHAR(12),@EndDate,106))  AND VPT.ProductID IN (SELECT P.ProductID FROM Product AS P WHERE P.ProductType=1)
		END
	END
	ELSE
	BEGIN
		IF(@CurrencyID=@BaseCurrencyID)
		BEGIN
			INSERT INTO #TempTable (ProductID,	ParentHeadID)
			SELECT DISTINCT AOB.BreakdownObjID,	@AccountHeadID FROM AccountOpenningBreakdown AS AOB WHERE dbo.GetComponentID(AOB.AccountHeadID)=2 AND AOB.BusinessUnitID=@BusinessUnitID AND AOB.AccountingSessionID=@SessionID AND AOB.BreakdownType=4 AND ISNULL(AOB.BreakdownObjID,0)!=0

			INSERT INTO #TempTable (ProductID,	ParentHeadID)
			SELECT DISTINCT VPT.ProductID,	@AccountHeadID FROM View_VPTransaction AS VPT WHERE dbo.GetComponentID(VPT.AccountHeadID)=2 AND  VPT.BUID=@BusinessUnitID AND CONVERT(DATE, CONVERT(VARCHAR(12),VPT.TransactionDate,106)) BETWEEN  CONVERT(DATE, CONVERT(VARCHAR(12),@SessionStartDate,106)) AND CONVERT(DATE, CONVERT(VARCHAR(12),@EndDate,106)) 
		END
		ELSE
		BEGIN
			INSERT INTO #TempTable (ProductID,	ParentHeadID)
			SELECT DISTINCT AOB.BreakdownObjID,	@AccountHeadID FROM AccountOpenningBreakdown AS AOB WHERE dbo.GetComponentID(AOB.AccountHeadID)=2 AND AOB.BusinessUnitID=@BusinessUnitID AND AOB.CurrencyID=@CurrencyID AND AOB.AccountingSessionID=@SessionID AND AOB.BreakdownType=4 AND ISNULL(AOB.BreakdownObjID,0)!=0

			INSERT INTO #TempTable (ProductID,	ParentHeadID)
			SELECT DISTINCT VPT.ProductID,	@AccountHeadID FROM View_VPTransaction AS VPT WHERE dbo.GetComponentID(VPT.AccountHeadID)=2 AND  VPT.BUID=@BusinessUnitID AND VPT.CurrencyID=@CurrencyID AND CONVERT(DATE, CONVERT(VARCHAR(12),VPT.TransactionDate,106)) BETWEEN  CONVERT(DATE, CONVERT(VARCHAR(12),@SessionStartDate,106)) AND CONVERT(DATE, CONVERT(VARCHAR(12),@EndDate,106))  AND VPT.ProductID IN (SELECT P.ProductID FROM Product AS P WHERE P.ProductType=1)
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
				INSERT INTO #TempTable (ProductID,	ParentHeadID)
				SELECT DISTINCT AOB.BreakdownObjID,	@AccountHeadID FROM AccountOpenningBreakdown AS AOB WHERE dbo.GetComponentID(AOB.AccountHeadID)=2 AND AOB.AccountHeadID=@AccountHeadID AND AOB.AccountingSessionID=@SessionID AND AOB.BreakdownType=4 AND ISNULL(AOB.BreakdownObjID,0)!=0

				INSERT INTO #TempTable (ProductID,	ParentHeadID)
				SELECT DISTINCT VPT.ProductID,	@AccountHeadID FROM View_VPTransaction AS VPT WHERE dbo.GetComponentID(VPT.AccountHeadID)=2 AND  ISNULL(VPT.ApprovedBy,0)!=0 AND VPT.AccountHeadID=@AccountHeadID AND CONVERT(DATE, CONVERT(VARCHAR(12),VPT.TransactionDate,106)) BETWEEN  CONVERT(DATE, CONVERT(VARCHAR(12),@SessionStartDate,106)) AND CONVERT(DATE, CONVERT(VARCHAR(12),@EndDate,106))  AND VPT.ProductID IN (SELECT P.ProductID FROM Product AS P WHERE P.ProductType=1)
			END
			ELSE
			BEGIN
				INSERT INTO #TempTable (ProductID,	ParentHeadID)
				SELECT DISTINCT AOB.BreakdownObjID,	@AccountHeadID FROM AccountOpenningBreakdown AS AOB WHERE dbo.GetComponentID(AOB.AccountHeadID)=2 AND AOB.CurrencyID=@CurrencyID AND AOB.AccountHeadID=@AccountHeadID AND AOB.AccountingSessionID=@SessionID AND AOB.BreakdownType=4 AND ISNULL(AOB.BreakdownObjID,0)!=0

				INSERT INTO #TempTable (ProductID,	ParentHeadID)
				SELECT DISTINCT VPT.ProductID,	@AccountHeadID FROM View_VPTransaction AS VPT WHERE dbo.GetComponentID(VPT.AccountHeadID)=2 AND  ISNULL(VPT.ApprovedBy,0)!=0  AND VPT.CurrencyID=@CurrencyID AND VPT.AccountHeadID=@AccountHeadID AND CONVERT(DATE, CONVERT(VARCHAR(12),VPT.TransactionDate,106)) BETWEEN  CONVERT(DATE, CONVERT(VARCHAR(12),@SessionStartDate,106)) AND CONVERT(DATE, CONVERT(VARCHAR(12),@EndDate,106))  AND VPT.ProductID IN (SELECT P.ProductID FROM Product AS P WHERE P.ProductType=1)
			END
		END
		ELSE
		BEGIN
			IF(@CurrencyID=@BaseCurrencyID)
			BEGIN
				INSERT INTO #TempTable (ProductID,	ParentHeadID)
				SELECT DISTINCT AOB.BreakdownObjID,	@AccountHeadID FROM AccountOpenningBreakdown AS AOB WHERE dbo.GetComponentID(AOB.AccountHeadID)=2 AND AOB.AccountHeadID=@AccountHeadID AND AOB.AccountingSessionID=@SessionID AND AOB.BreakdownType=4 AND ISNULL(AOB.BreakdownObjID,0)!=0

				INSERT INTO #TempTable (ProductID,	ParentHeadID)
				SELECT DISTINCT VPT.ProductID,	@AccountHeadID FROM View_VPTransaction AS VPT WHERE dbo.GetComponentID(VPT.AccountHeadID)=2 AND  VPT.AccountHeadID=@AccountHeadID AND CONVERT(DATE, CONVERT(VARCHAR(12),VPT.TransactionDate,106)) BETWEEN  CONVERT(DATE, CONVERT(VARCHAR(12),@SessionStartDate,106)) AND CONVERT(DATE, CONVERT(VARCHAR(12),@EndDate,106))  AND VPT.ProductID IN (SELECT P.ProductID FROM Product AS P WHERE P.ProductType=1)
			END
			ELSE
			BEGIN
				INSERT INTO #TempTable (ProductID,	ParentHeadID)
				SELECT DISTINCT AOB.BreakdownObjID,	@AccountHeadID FROM AccountOpenningBreakdown AS AOB WHERE dbo.GetComponentID(AOB.AccountHeadID)=2 AND AOB.CurrencyID=@CurrencyID AND AOB.AccountHeadID=@AccountHeadID AND AOB.AccountingSessionID=@SessionID AND AOB.BreakdownType=4 AND ISNULL(AOB.BreakdownObjID,0)!=0

				INSERT INTO #TempTable (ProductID,	ParentHeadID)
				SELECT DISTINCT VPT.ProductID,	@AccountHeadID FROM View_VPTransaction AS VPT WHERE dbo.GetComponentID(VPT.AccountHeadID)=2 AND  VPT.CurrencyID=@CurrencyID AND VPT.AccountHeadID=@AccountHeadID AND CONVERT(DATE, CONVERT(VARCHAR(12),VPT.TransactionDate,106)) BETWEEN  CONVERT(DATE, CONVERT(VARCHAR(12),@SessionStartDate,106)) AND CONVERT(DATE, CONVERT(VARCHAR(12),@EndDate,106))  AND VPT.ProductID IN (SELECT P.ProductID FROM Product AS P WHERE P.ProductType=1)
			END
		END
	END
	ELSE
	BEGIN
		IF(@IsApproved=1)
		BEGIN
			IF(@CurrencyID=@BaseCurrencyID)
			BEGIN
				INSERT INTO #TempTable (ProductID,	ParentHeadID)
				SELECT DISTINCT AOB.BreakdownObjID,	@AccountHeadID FROM AccountOpenningBreakdown AS AOB WHERE dbo.GetComponentID(AOB.AccountHeadID)=2 AND AOB.AccountingSessionID=@SessionID AND AOB.BreakdownType=4 AND ISNULL(AOB.BreakdownObjID,0)!=0

				INSERT INTO #TempTable (ProductID,	ParentHeadID)
				SELECT DISTINCT VPT.ProductID,	@AccountHeadID FROM View_VPTransaction AS VPT WHERE dbo.GetComponentID(VPT.AccountHeadID)=2 AND  ISNULL(VPT.ApprovedBy,0)!=0 AND CONVERT(DATE, CONVERT(VARCHAR(12),VPT.TransactionDate,106)) BETWEEN  CONVERT(DATE, CONVERT(VARCHAR(12),@SessionStartDate,106)) AND CONVERT(DATE, CONVERT(VARCHAR(12),@EndDate,106))  AND VPT.ProductID IN (SELECT P.ProductID FROM Product AS P WHERE P.ProductType=1)
			END
			ELSE
			BEGIN
				INSERT INTO #TempTable (ProductID,	ParentHeadID)
				SELECT DISTINCT AOB.BreakdownObjID,	@AccountHeadID FROM AccountOpenningBreakdown AS AOB WHERE dbo.GetComponentID(AOB.AccountHeadID)=2 AND AOB.CurrencyID=@CurrencyID AND AOB.AccountingSessionID=@SessionID AND AOB.BreakdownType=4 AND ISNULL(AOB.BreakdownObjID,0)!=0

				INSERT INTO #TempTable (ProductID,	ParentHeadID)
				SELECT DISTINCT VPT.ProductID,	@AccountHeadID FROM View_VPTransaction AS VPT WHERE dbo.GetComponentID(VPT.AccountHeadID)=2 AND  ISNULL(VPT.ApprovedBy,0)!=0  AND VPT.CurrencyID=@CurrencyID AND CONVERT(DATE, CONVERT(VARCHAR(12),VPT.TransactionDate,106)) BETWEEN  CONVERT(DATE, CONVERT(VARCHAR(12),@SessionStartDate,106)) AND CONVERT(DATE, CONVERT(VARCHAR(12),@EndDate,106))  AND VPT.ProductID IN (SELECT P.ProductID FROM Product AS P WHERE P.ProductType=1)
			END
		END
		ELSE
		BEGIN
			IF(@CurrencyID=@BaseCurrencyID)
			BEGIN
				INSERT INTO #TempTable (ProductID,	ParentHeadID)
				SELECT DISTINCT AOB.BreakdownObjID,	@AccountHeadID FROM AccountOpenningBreakdown AS AOB WHERE dbo.GetComponentID(AOB.AccountHeadID)=2 AND AOB.AccountingSessionID=@SessionID AND AOB.BreakdownType=4 AND ISNULL(AOB.BreakdownObjID,0)!=0

				INSERT INTO #TempTable (ProductID,	ParentHeadID)
				SELECT DISTINCT VPT.ProductID,	@AccountHeadID FROM View_VPTransaction AS VPT WHERE dbo.GetComponentID(VPT.AccountHeadID)=2 AND  CONVERT(DATE, CONVERT(VARCHAR(12),VPT.TransactionDate,106)) BETWEEN  CONVERT(DATE, CONVERT(VARCHAR(12),@SessionStartDate,106)) AND CONVERT(DATE, CONVERT(VARCHAR(12),@EndDate,106))  AND VPT.ProductID IN (SELECT P.ProductID FROM Product AS P WHERE P.ProductType=1)
			END
			ELSE
			BEGIN
				INSERT INTO #TempTable (ProductID,	ParentHeadID)
				SELECT DISTINCT AOB.BreakdownObjID,	@AccountHeadID FROM AccountOpenningBreakdown AS AOB WHERE dbo.GetComponentID(AOB.AccountHeadID)=2 AND AOB.CurrencyID=@CurrencyID AND AOB.AccountingSessionID=@SessionID AND AOB.BreakdownType=4 AND ISNULL(AOB.BreakdownObjID,0)!=0

				INSERT INTO #TempTable (ProductID,	ParentHeadID)
				SELECT DISTINCT VPT.ProductID,	@AccountHeadID FROM View_VPTransaction AS VPT WHERE dbo.GetComponentID(VPT.AccountHeadID)=2 AND  VPT.CurrencyID=@CurrencyID AND CONVERT(DATE, CONVERT(VARCHAR(12),VPT.TransactionDate,106)) BETWEEN  CONVERT(DATE, CONVERT(VARCHAR(12),@SessionStartDate,106)) AND CONVERT(DATE, CONVERT(VARCHAR(12),@EndDate,106))  AND VPT.ProductID IN (SELECT P.ProductID FROM Product AS P WHERE P.ProductType=1)
			END
		END
	END
END

DELETE FROM #TempTable WHERE ID NOT IN (SELECT MIN(ID) FROM #TempTable GROUP BY ProductID,ParentHeadID)

INSERT INTO #TempTable2 (ProductID) SELECT ProductID FROM #TempTable

IF(@BusinessUnitID>0)
BEGIN
	IF(@AccountHeadID>0)
	BEGIN
		IF(@IsApproved=1)
		BEGIN
			IF(@CurrencyID=@BaseCurrencyID)
			BEGIN
				UPDATE #TempTable2
				SET DebitOpeiningQty=ISNULL((SELECT SUM(AOB.Qty) FROM AccountOpenningBreakdown AS AOB WHERE dbo.GetComponentID(AOB.AccountHeadID)=2 AND AOB.BusinessUnitID=@BusinessUnitID AND AOB.IsDr=1 AND AOB.BreakdownObjID=TT.ProductID AND AOB.AccountHeadID=@AccountHeadID AND AOB.AccountingSessionID=@SessionID AND AOB.BreakdownType=4),0),
					CreditOpeiningQty=ISNULL((SELECT SUM(AOB.Qty) FROM AccountOpenningBreakdown AS AOB WHERE dbo.GetComponentID(AOB.AccountHeadID)=2 AND AOB.BusinessUnitID=@BusinessUnitID AND AOB.IsDr=0 AND AOB.BreakdownObjID=TT.ProductID AND AOB.AccountHeadID=@AccountHeadID AND AOB.AccountingSessionID=@SessionID AND AOB.BreakdownType=4),0),
					DebitOpeiningValue=ISNULL((SELECT SUM(AOB.OpenningBalance) FROM AccountOpenningBreakdown AS AOB WHERE dbo.GetComponentID(AOB.AccountHeadID)=2 AND AOB.BusinessUnitID=@BusinessUnitID AND AOB.IsDr=1 AND AOB.BreakdownObjID=TT.ProductID AND AOB.AccountHeadID=@AccountHeadID AND AOB.AccountingSessionID=@SessionID AND AOB.BreakdownType=4),0),
					CreditOpeiningValue=ISNULL((SELECT SUM(AOB.OpenningBalance) FROM AccountOpenningBreakdown AS AOB WHERE dbo.GetComponentID(AOB.AccountHeadID)=2 AND AOB.BusinessUnitID=@BusinessUnitID AND AOB.IsDr=0 AND AOB.BreakdownObjID=TT.ProductID AND AOB.AccountHeadID=@AccountHeadID AND AOB.AccountingSessionID=@SessionID AND AOB.BreakdownType=4),0),
					DebitQty=ISNULL((SELECT SUM(VPT.Qty) FROM View_VPTransaction AS VPT WHERE dbo.GetComponentID(VPT.AccountHeadID)=2 AND   VPT.BUID=@BusinessUnitID AND ISNULL(VPT.ApprovedBy,0)!=0 AND  VPT.IsDr=1 AND VPT.ProductID=TT.ProductID AND VPT.AccountHeadID=@AccountHeadID AND CONVERT(DATE, CONVERT(VARCHAR(12),VPT.TransactionDate,106)) BETWEEN  CONVERT(DATE, CONVERT(VARCHAR(12),@SessionStartDate,106)) AND CONVERT(DATE, CONVERT(VARCHAR(12),DATEADD(DAY,-1,@StartDate),106))),0),
					DebitAmount=ISNULL((SELECT SUM(VPT.Amount*VPT.ConversionRate) FROM View_VPTransaction AS VPT WHERE dbo.GetComponentID(VPT.AccountHeadID)=2 AND   VPT.BUID=@BusinessUnitID AND ISNULL(VPT.ApprovedBy,0)!=0 AND  VPT.IsDr=1 AND VPT.ProductID=TT.ProductID AND VPT.AccountHeadID=@AccountHeadID AND CONVERT(DATE, CONVERT(VARCHAR(12),VPT.TransactionDate,106)) BETWEEN  CONVERT(DATE, CONVERT(VARCHAR(12),@SessionStartDate,106)) AND CONVERT(DATE, CONVERT(VARCHAR(12),DATEADD(DAY,-1,@StartDate),106))),0),
					CreditQty=ISNULL((SELECT SUM(VPT.Qty) FROM View_VPTransaction AS VPT WHERE dbo.GetComponentID(VPT.AccountHeadID)=2 AND   VPT.BUID=@BusinessUnitID AND ISNULL(VPT.ApprovedBy,0)!=0 AND  VPT.IsDr=0 AND VPT.ProductID=TT.ProductID AND VPT.AccountHeadID=@AccountHeadID AND CONVERT(DATE, CONVERT(VARCHAR(12),VPT.TransactionDate,106)) BETWEEN  CONVERT(DATE, CONVERT(VARCHAR(12),@SessionStartDate,106)) AND CONVERT(DATE, CONVERT(VARCHAR(12),DATEADD(DAY,-1,@StartDate),106))),0),
					CreditAmount=ISNULL((SELECT SUM(VPT.Amount*VPT.ConversionRate) FROM View_VPTransaction AS VPT WHERE dbo.GetComponentID(VPT.AccountHeadID)=2 AND   VPT.BUID=@BusinessUnitID AND ISNULL(VPT.ApprovedBy,0)!=0 AND  VPT.IsDr=0 AND VPT.ProductID=TT.ProductID AND VPT.AccountHeadID=@AccountHeadID AND CONVERT(DATE, CONVERT(VARCHAR(12),VPT.TransactionDate,106)) BETWEEN  CONVERT(DATE, CONVERT(VARCHAR(12),@SessionStartDate,106)) AND CONVERT(DATE, CONVERT(VARCHAR(12),DATEADD(DAY,-1,@StartDate),106))),0)
				FROM #TempTable2 AS TT
	
				UPDATE #TempTable
				SET DebitQty=ISNULL((SELECT SUM(VPT.Qty) FROM View_VPTransaction AS VPT WHERE dbo.GetComponentID(VPT.AccountHeadID)=2 AND   VPT.BUID=@BusinessUnitID AND ISNULL(VPT.ApprovedBy,0)!=0 AND  VPT.IsDr=1 AND VPT.ProductID=TT.ProductID AND VPT.AccountHeadID=@AccountHeadID AND CONVERT(DATE, CONVERT(VARCHAR(12),VPT.TransactionDate,106)) BETWEEN  CONVERT(DATE, CONVERT(VARCHAR(12),@StartDate,106)) AND CONVERT(DATE, CONVERT(VARCHAR(12),@EndDate,106))),0),
					DebitAmount=ISNULL((SELECT SUM(VPT.Amount*VPT.ConversionRate) FROM View_VPTransaction AS VPT WHERE dbo.GetComponentID(VPT.AccountHeadID)=2 AND   VPT.BUID=@BusinessUnitID AND ISNULL(VPT.ApprovedBy,0)!=0 AND  VPT.IsDr=1 AND VPT.ProductID=TT.ProductID AND VPT.AccountHeadID=@AccountHeadID AND CONVERT(DATE, CONVERT(VARCHAR(12),VPT.TransactionDate,106)) BETWEEN  CONVERT(DATE, CONVERT(VARCHAR(12),@StartDate,106)) AND CONVERT(DATE, CONVERT(VARCHAR(12),@EndDate,106))),0),
					CreditQty=ISNULL((SELECT SUM(VPT.Qty) FROM View_VPTransaction AS VPT WHERE dbo.GetComponentID(VPT.AccountHeadID)=2 AND   VPT.BUID=@BusinessUnitID AND ISNULL(VPT.ApprovedBy,0)!=0 AND  VPT.IsDr=0 AND VPT.ProductID=TT.ProductID AND VPT.AccountHeadID=@AccountHeadID AND CONVERT(DATE, CONVERT(VARCHAR(12),VPT.TransactionDate,106)) BETWEEN  CONVERT(DATE, CONVERT(VARCHAR(12),@StartDate,106)) AND CONVERT(DATE, CONVERT(VARCHAR(12),@EndDate,106))),0),
					CreditAmount=ISNULL((SELECT SUM(VPT.Amount*VPT.ConversionRate) FROM View_VPTransaction AS VPT WHERE dbo.GetComponentID(VPT.AccountHeadID)=2 AND  VPT.BUID=@BusinessUnitID AND  ISNULL(VPT.ApprovedBy,0)!=0 AND  VPT.IsDr=0 AND VPT.ProductID=TT.ProductID AND VPT.AccountHeadID=@AccountHeadID AND CONVERT(DATE, CONVERT(VARCHAR(12),VPT.TransactionDate,106)) BETWEEN  CONVERT(DATE, CONVERT(VARCHAR(12),@StartDate,106)) AND CONVERT(DATE, CONVERT(VARCHAR(12),@EndDate,106))),0)
				FROM #TempTable AS TT

			END
			ELSE
			BEGIN
				UPDATE #TempTable2
				SET DebitOpeiningQty=ISNULL((SELECT SUM(AOB.Qty) FROM AccountOpenningBreakdown AS AOB WHERE dbo.GetComponentID(AOB.AccountHeadID)=2 AND AOB.BusinessUnitID=@BusinessUnitID AND AOB.IsDr=1 AND AOB.CurrencyID=@CurrencyID AND AOB.BreakdownObjID=TT.ProductID AND AOB.AccountHeadID=@AccountHeadID AND AOB.AccountingSessionID=@SessionID AND AOB.BreakdownType=4),0),
					CreditOpeiningQty=ISNULL((SELECT SUM(AOB.Qty) FROM AccountOpenningBreakdown AS AOB WHERE dbo.GetComponentID(AOB.AccountHeadID)=2 AND AOB.BusinessUnitID=@BusinessUnitID AND AOB.IsDr=0 AND AOB.CurrencyID=@CurrencyID AND AOB.BreakdownObjID=TT.ProductID AND AOB.AccountHeadID=@AccountHeadID AND AOB.AccountingSessionID=@SessionID AND AOB.BreakdownType=4),0),
					DebitOpeiningValue=ISNULL((SELECT SUM(AOB.AmountInCurrency) FROM AccountOpenningBreakdown AS AOB WHERE dbo.GetComponentID(AOB.AccountHeadID)=2 AND AOB.BusinessUnitID=@BusinessUnitID AND AOB.IsDr=1 AND AOB.CurrencyID=@CurrencyID AND AOB.BreakdownObjID=TT.ProductID AND AOB.AccountHeadID=@AccountHeadID AND AOB.AccountingSessionID=@SessionID AND AOB.BreakdownType=4),0),
					CreditOpeiningValue=ISNULL((SELECT SUM(AOB.AmountInCurrency) FROM AccountOpenningBreakdown AS AOB WHERE dbo.GetComponentID(AOB.AccountHeadID)=2 AND AOB.BusinessUnitID=@BusinessUnitID AND AOB.IsDr=0 AND AOB.CurrencyID=@CurrencyID AND AOB.BreakdownObjID=TT.ProductID AND AOB.AccountHeadID=@AccountHeadID AND AOB.AccountingSessionID=@SessionID AND AOB.BreakdownType=4),0),
					DebitQty=ISNULL((SELECT SUM(VPT.Qty) FROM View_VPTransaction AS VPT WHERE dbo.GetComponentID(VPT.AccountHeadID)=2 AND  VPT.BUID=@BusinessUnitID AND VPT.ApprovedBy!=0 AND VPT.CurrencyID=@CurrencyID AND  VPT.IsDr=1 AND VPT.ProductID=TT.ProductID AND VPT.AccountHeadID=@AccountHeadID AND CONVERT(DATE, CONVERT(VARCHAR(12),VPT.TransactionDate,106)) BETWEEN  CONVERT(DATE, CONVERT(VARCHAR(12),@SessionStartDate,106)) AND CONVERT(DATE, CONVERT(VARCHAR(12),DATEADD(DAY,-1,@StartDate),106))),0),
					DebitAmount=ISNULL((SELECT SUM(VPT.Amount) FROM View_VPTransaction AS VPT WHERE dbo.GetComponentID(VPT.AccountHeadID)=2 AND  VPT.BUID=@BusinessUnitID AND VPT.ApprovedBy!=0 AND VPT.CurrencyID=@CurrencyID AND  VPT.IsDr=1 AND VPT.ProductID=TT.ProductID AND VPT.AccountHeadID=@AccountHeadID AND CONVERT(DATE, CONVERT(VARCHAR(12),VPT.TransactionDate,106)) BETWEEN  CONVERT(DATE, CONVERT(VARCHAR(12),@SessionStartDate,106)) AND CONVERT(DATE, CONVERT(VARCHAR(12),DATEADD(DAY,-1,@StartDate),106))),0),
					CreditQty=ISNULL((SELECT SUM(VPT.Qty) FROM View_VPTransaction AS VPT WHERE dbo.GetComponentID(VPT.AccountHeadID)=2 AND  VPT.BUID=@BusinessUnitID AND VPT.ApprovedBy!=0 AND VPT.CurrencyID=@CurrencyID AND VPT.IsDr=0 AND VPT.ProductID=TT.ProductID AND VPT.AccountHeadID=@AccountHeadID AND CONVERT(DATE, CONVERT(VARCHAR(12),VPT.TransactionDate,106)) BETWEEN  CONVERT(DATE, CONVERT(VARCHAR(12),@SessionStartDate,106)) AND CONVERT(DATE, CONVERT(VARCHAR(12),DATEADD(DAY,-1,@StartDate),106))),0),
					CreditAmount=ISNULL((SELECT SUM(VPT.Amount) FROM View_VPTransaction AS VPT WHERE dbo.GetComponentID(VPT.AccountHeadID)=2 AND  VPT.BUID=@BusinessUnitID AND VPT.ApprovedBy!=0 AND VPT.CurrencyID=@CurrencyID AND VPT.IsDr=0 AND VPT.ProductID=TT.ProductID AND VPT.AccountHeadID=@AccountHeadID AND CONVERT(DATE, CONVERT(VARCHAR(12),VPT.TransactionDate,106)) BETWEEN  CONVERT(DATE, CONVERT(VARCHAR(12),@SessionStartDate,106)) AND CONVERT(DATE, CONVERT(VARCHAR(12),DATEADD(DAY,-1,@StartDate),106))),0)
				FROM #TempTable2 AS TT

				UPDATE #TempTable
				SET DebitQty=ISNULL((SELECT SUM(VPT.Qty) FROM View_VPTransaction AS VPT WHERE dbo.GetComponentID(VPT.AccountHeadID)=2 AND  VPT.BUID=@BusinessUnitID AND VPT.ApprovedBy!=0 AND VPT.CurrencyID=@CurrencyID AND  VPT.IsDr=1 AND VPT.ProductID=TT.ProductID AND VPT.AccountHeadID=@AccountHeadID AND CONVERT(DATE, CONVERT(VARCHAR(12),VPT.TransactionDate,106)) BETWEEN  CONVERT(DATE, CONVERT(VARCHAR(12),@StartDate,106)) AND CONVERT(DATE, CONVERT(VARCHAR(12),@EndDate,106))),0),
					DebitAmount=ISNULL((SELECT SUM(VPT.Amount) FROM View_VPTransaction AS VPT WHERE dbo.GetComponentID(VPT.AccountHeadID)=2 AND  VPT.BUID=@BusinessUnitID AND VPT.ApprovedBy!=0 AND VPT.CurrencyID=@CurrencyID AND  VPT.IsDr=1 AND VPT.ProductID=TT.ProductID AND VPT.AccountHeadID=@AccountHeadID AND CONVERT(DATE, CONVERT(VARCHAR(12),VPT.TransactionDate,106)) BETWEEN  CONVERT(DATE, CONVERT(VARCHAR(12),@StartDate,106)) AND CONVERT(DATE, CONVERT(VARCHAR(12),@EndDate,106))),0),
					CreditQty=ISNULL((SELECT SUM(VPT.Qty) FROM View_VPTransaction AS VPT WHERE dbo.GetComponentID(VPT.AccountHeadID)=2 AND  VPT.BUID=@BusinessUnitID AND VPT.ApprovedBy!=0 AND VPT.CurrencyID=@CurrencyID AND  VPT.IsDr=0 AND VPT.ProductID=TT.ProductID AND VPT.AccountHeadID=@AccountHeadID AND CONVERT(DATE, CONVERT(VARCHAR(12),VPT.TransactionDate,106)) BETWEEN  CONVERT(DATE, CONVERT(VARCHAR(12),@StartDate,106)) AND CONVERT(DATE, CONVERT(VARCHAR(12),@EndDate,106))),0),
					CreditAmount=ISNULL((SELECT SUM(VPT.Amount) FROM View_VPTransaction AS VPT WHERE dbo.GetComponentID(VPT.AccountHeadID)=2 AND  VPT.BUID=@BusinessUnitID AND VPT.ApprovedBy!=0 AND VPT.CurrencyID=@CurrencyID AND  VPT.IsDr=0 AND VPT.ProductID=TT.ProductID AND VPT.AccountHeadID=@AccountHeadID AND CONVERT(DATE, CONVERT(VARCHAR(12),VPT.TransactionDate,106)) BETWEEN  CONVERT(DATE, CONVERT(VARCHAR(12),@StartDate,106)) AND CONVERT(DATE, CONVERT(VARCHAR(12),@EndDate,106))),0)
				FROM #TempTable AS TT
			END
		END
		ELSE
		BEGIN
			IF(@CurrencyID=@BaseCurrencyID)
			BEGIN
				UPDATE #TempTable2
				SET DebitOpeiningQty=ISNULL((SELECT SUM(AOB.Qty) FROM AccountOpenningBreakdown AS AOB WHERE dbo.GetComponentID(AOB.AccountHeadID)=2 AND AOB.BusinessUnitID=@BusinessUnitID AND AOB.IsDr=1 AND AOB.BreakdownObjID=TT.ProductID AND AOB.AccountHeadID=@AccountHeadID AND AOB.AccountingSessionID=@SessionID AND AOB.BreakdownType=4),0),
					CreditOpeiningQty=ISNULL((SELECT SUM(AOB.Qty) FROM AccountOpenningBreakdown AS AOB WHERE dbo.GetComponentID(AOB.AccountHeadID)=2 AND AOB.BusinessUnitID=@BusinessUnitID AND AOB.IsDr=0 AND AOB.BreakdownObjID=TT.ProductID AND AOB.AccountHeadID=@AccountHeadID AND AOB.AccountingSessionID=@SessionID AND AOB.BreakdownType=4),0),
					DebitOpeiningValue=ISNULL((SELECT SUM(AOB.OpenningBalance) FROM AccountOpenningBreakdown AS AOB WHERE dbo.GetComponentID(AOB.AccountHeadID)=2 AND AOB.BusinessUnitID=@BusinessUnitID AND AOB.IsDr=1 AND AOB.BreakdownObjID=TT.ProductID AND AOB.AccountHeadID=@AccountHeadID AND AOB.AccountingSessionID=@SessionID AND AOB.BreakdownType=4),0),
					CreditOpeiningValue=ISNULL((SELECT SUM(AOB.OpenningBalance) FROM AccountOpenningBreakdown AS AOB WHERE dbo.GetComponentID(AOB.AccountHeadID)=2 AND AOB.BusinessUnitID=@BusinessUnitID AND AOB.IsDr=0 AND AOB.BreakdownObjID=TT.ProductID AND AOB.AccountHeadID=@AccountHeadID AND AOB.AccountingSessionID=@SessionID AND AOB.BreakdownType=4),0),
					DebitQty=ISNULL((SELECT SUM(VPT.Qty) FROM View_VPTransaction AS VPT WHERE dbo.GetComponentID(VPT.AccountHeadID)=2 AND  VPT.BUID=@BusinessUnitID AND VPT.IsDr=1 AND VPT.ProductID=TT.ProductID AND VPT.AccountHeadID=@AccountHeadID AND CONVERT(DATE, CONVERT(VARCHAR(12),VPT.TransactionDate,106)) BETWEEN  CONVERT(DATE, CONVERT(VARCHAR(12),@SessionStartDate,106)) AND CONVERT(DATE, CONVERT(VARCHAR(12),DATEADD(DAY,-1,@StartDate),106))),0),
					DebitAmount=ISNULL((SELECT SUM(VPT.Amount*VPT.ConversionRate) FROM View_VPTransaction AS VPT WHERE dbo.GetComponentID(VPT.AccountHeadID)=2 AND  VPT.BUID=@BusinessUnitID AND VPT.IsDr=1 AND VPT.ProductID=TT.ProductID AND VPT.AccountHeadID=@AccountHeadID AND CONVERT(DATE, CONVERT(VARCHAR(12),VPT.TransactionDate,106)) BETWEEN  CONVERT(DATE, CONVERT(VARCHAR(12),@SessionStartDate,106)) AND CONVERT(DATE, CONVERT(VARCHAR(12),DATEADD(DAY,-1,@StartDate),106))),0),
					CreditQty=ISNULL((SELECT SUM(VPT.Qty) FROM View_VPTransaction AS VPT WHERE dbo.GetComponentID(VPT.AccountHeadID)=2 AND  VPT.BUID=@BusinessUnitID AND VPT.IsDr=0 AND VPT.ProductID=TT.ProductID AND VPT.AccountHeadID=@AccountHeadID AND CONVERT(DATE, CONVERT(VARCHAR(12),VPT.TransactionDate,106)) BETWEEN  CONVERT(DATE, CONVERT(VARCHAR(12),@SessionStartDate,106)) AND CONVERT(DATE, CONVERT(VARCHAR(12),DATEADD(DAY,-1,@StartDate),106))),0),
					CreditAmount=ISNULL((SELECT SUM(VPT.Amount*VPT.ConversionRate) FROM View_VPTransaction AS VPT WHERE dbo.GetComponentID(VPT.AccountHeadID)=2 AND  VPT.BUID=@BusinessUnitID AND VPT.IsDr=0 AND VPT.ProductID=TT.ProductID AND VPT.AccountHeadID=@AccountHeadID AND CONVERT(DATE, CONVERT(VARCHAR(12),VPT.TransactionDate,106)) BETWEEN  CONVERT(DATE, CONVERT(VARCHAR(12),@SessionStartDate,106)) AND CONVERT(DATE, CONVERT(VARCHAR(12),DATEADD(DAY,-1,@StartDate),106))),0)
				FROM #TempTable2 AS TT
	
				UPDATE #TempTable
				SET DebitQty=ISNULL((SELECT SUM(VPT.Qty) FROM View_VPTransaction AS VPT WHERE dbo.GetComponentID(VPT.AccountHeadID)=2 AND  VPT.BUID=@BusinessUnitID AND VPT.IsDr=1 AND VPT.ProductID=TT.ProductID AND VPT.AccountHeadID=@AccountHeadID AND CONVERT(DATE, CONVERT(VARCHAR(12),VPT.TransactionDate,106)) BETWEEN  CONVERT(DATE, CONVERT(VARCHAR(12),@StartDate,106)) AND CONVERT(DATE, CONVERT(VARCHAR(12),@EndDate,106))),0),
					DebitAmount=ISNULL((SELECT SUM(VPT.Amount*VPT.ConversionRate) FROM View_VPTransaction AS VPT WHERE dbo.GetComponentID(VPT.AccountHeadID)=2 AND  VPT.BUID=@BusinessUnitID AND VPT.IsDr=1 AND VPT.ProductID=TT.ProductID AND VPT.AccountHeadID=@AccountHeadID AND CONVERT(DATE, CONVERT(VARCHAR(12),VPT.TransactionDate,106)) BETWEEN  CONVERT(DATE, CONVERT(VARCHAR(12),@StartDate,106)) AND CONVERT(DATE, CONVERT(VARCHAR(12),@EndDate,106))),0),
					CreditQty=ISNULL((SELECT SUM(VPT.Qty) FROM View_VPTransaction AS VPT WHERE dbo.GetComponentID(VPT.AccountHeadID)=2 AND  VPT.BUID=@BusinessUnitID AND VPT.IsDr=0 AND VPT.ProductID=TT.ProductID AND VPT.AccountHeadID=@AccountHeadID AND CONVERT(DATE, CONVERT(VARCHAR(12),VPT.TransactionDate,106)) BETWEEN  CONVERT(DATE, CONVERT(VARCHAR(12),@StartDate,106)) AND CONVERT(DATE, CONVERT(VARCHAR(12),@EndDate,106))),0),
					CreditAmount=ISNULL((SELECT SUM(VPT.Amount*VPT.ConversionRate) FROM View_VPTransaction AS VPT WHERE dbo.GetComponentID(VPT.AccountHeadID)=2 AND  VPT.BUID=@BusinessUnitID AND VPT.IsDr=0 AND VPT.ProductID=TT.ProductID AND VPT.AccountHeadID=@AccountHeadID AND CONVERT(DATE, CONVERT(VARCHAR(12),VPT.TransactionDate,106)) BETWEEN  CONVERT(DATE, CONVERT(VARCHAR(12),@StartDate,106)) AND CONVERT(DATE, CONVERT(VARCHAR(12),@EndDate,106))),0)
				FROM #TempTable AS TT

			END
			ELSE
			BEGIN
				UPDATE #TempTable2
				SET DebitOpeiningQty=ISNULL((SELECT SUM(AOB.Qty) FROM AccountOpenningBreakdown AS AOB WHERE dbo.GetComponentID(AOB.AccountHeadID)=2 AND AOB.BusinessUnitID=@BusinessUnitID AND AOB.IsDr=1 AND AOB.CurrencyID=@CurrencyID AND AOB.BreakdownObjID=TT.ProductID AND AOB.AccountHeadID=@AccountHeadID AND AOB.AccountingSessionID=@SessionID AND AOB.BreakdownType=4),0),
					CreditOpeiningQty=ISNULL((SELECT SUM(AOB.Qty) FROM AccountOpenningBreakdown AS AOB WHERE dbo.GetComponentID(AOB.AccountHeadID)=2 AND AOB.BusinessUnitID=@BusinessUnitID AND AOB.IsDr=0 AND AOB.CurrencyID=@CurrencyID AND AOB.BreakdownObjID=TT.ProductID AND AOB.AccountHeadID=@AccountHeadID AND AOB.AccountingSessionID=@SessionID AND AOB.BreakdownType=4),0),
					DebitOpeiningValue=ISNULL((SELECT SUM(AOB.AmountInCurrency) FROM AccountOpenningBreakdown AS AOB WHERE dbo.GetComponentID(AOB.AccountHeadID)=2 AND AOB.BusinessUnitID=@BusinessUnitID AND AOB.IsDr=1 AND AOB.CurrencyID=@CurrencyID AND AOB.BreakdownObjID=TT.ProductID AND AOB.AccountHeadID=@AccountHeadID AND AOB.AccountingSessionID=@SessionID AND AOB.BreakdownType=4),0),
					CreditOpeiningValue=ISNULL((SELECT SUM(AOB.AmountInCurrency) FROM AccountOpenningBreakdown AS AOB WHERE dbo.GetComponentID(AOB.AccountHeadID)=2 AND AOB.BusinessUnitID=@BusinessUnitID AND AOB.IsDr=0 AND AOB.CurrencyID=@CurrencyID AND AOB.BreakdownObjID=TT.ProductID AND AOB.AccountHeadID=@AccountHeadID AND AOB.AccountingSessionID=@SessionID AND AOB.BreakdownType=4),0),
					DebitQty=ISNULL((SELECT SUM(VPT.Qty) FROM View_VPTransaction AS VPT WHERE dbo.GetComponentID(VPT.AccountHeadID)=2 AND  VPT.BUID=@BusinessUnitID AND VPT.CurrencyID=@CurrencyID AND  VPT.IsDr=1 AND VPT.ProductID=TT.ProductID AND VPT.AccountHeadID=@AccountHeadID AND CONVERT(DATE, CONVERT(VARCHAR(12),VPT.TransactionDate,106)) BETWEEN  CONVERT(DATE, CONVERT(VARCHAR(12),@SessionStartDate,106)) AND CONVERT(DATE, CONVERT(VARCHAR(12),DATEADD(DAY,-1,@StartDate),106))),0),
					DebitAmount=ISNULL((SELECT SUM(VPT.Amount) FROM View_VPTransaction AS VPT WHERE dbo.GetComponentID(VPT.AccountHeadID)=2 AND  VPT.BUID=@BusinessUnitID AND VPT.CurrencyID=@CurrencyID AND  VPT.IsDr=1 AND VPT.ProductID=TT.ProductID AND VPT.AccountHeadID=@AccountHeadID AND CONVERT(DATE, CONVERT(VARCHAR(12),VPT.TransactionDate,106)) BETWEEN  CONVERT(DATE, CONVERT(VARCHAR(12),@SessionStartDate,106)) AND CONVERT(DATE, CONVERT(VARCHAR(12),DATEADD(DAY,-1,@StartDate),106))),0),
					CreditQty=ISNULL((SELECT SUM(VPT.Qty) FROM View_VPTransaction AS VPT WHERE dbo.GetComponentID(VPT.AccountHeadID)=2 AND  VPT.BUID=@BusinessUnitID AND VPT.CurrencyID=@CurrencyID AND VPT.IsDr=0 AND VPT.ProductID=TT.ProductID AND VPT.AccountHeadID=@AccountHeadID AND CONVERT(DATE, CONVERT(VARCHAR(12),VPT.TransactionDate,106)) BETWEEN  CONVERT(DATE, CONVERT(VARCHAR(12),@SessionStartDate,106)) AND CONVERT(DATE, CONVERT(VARCHAR(12),DATEADD(DAY,-1,@StartDate),106))),0),
					CreditAmount=ISNULL((SELECT SUM(VPT.Amount) FROM View_VPTransaction AS VPT WHERE dbo.GetComponentID(VPT.AccountHeadID)=2 AND  VPT.BUID=@BusinessUnitID AND VPT.CurrencyID=@CurrencyID AND VPT.IsDr=0 AND VPT.ProductID=TT.ProductID AND VPT.AccountHeadID=@AccountHeadID AND CONVERT(DATE, CONVERT(VARCHAR(12),VPT.TransactionDate,106)) BETWEEN  CONVERT(DATE, CONVERT(VARCHAR(12),@SessionStartDate,106)) AND CONVERT(DATE, CONVERT(VARCHAR(12),DATEADD(DAY,-1,@StartDate),106))),0)
				FROM #TempTable2 AS TT

				UPDATE #TempTable
				SET DebitQty=ISNULL((SELECT SUM(VPT.Qty) FROM View_VPTransaction AS VPT WHERE dbo.GetComponentID(VPT.AccountHeadID)=2 AND  VPT.BUID=@BusinessUnitID AND VPT.CurrencyID=@CurrencyID AND  VPT.IsDr=1 AND VPT.ProductID=TT.ProductID AND VPT.AccountHeadID=@AccountHeadID AND CONVERT(DATE, CONVERT(VARCHAR(12),VPT.TransactionDate,106)) BETWEEN  CONVERT(DATE, CONVERT(VARCHAR(12),@StartDate,106)) AND CONVERT(DATE, CONVERT(VARCHAR(12),@EndDate,106))),0),
					DebitAmount=ISNULL((SELECT SUM(VPT.Amount) FROM View_VPTransaction AS VPT WHERE dbo.GetComponentID(VPT.AccountHeadID)=2 AND  VPT.BUID=@BusinessUnitID AND VPT.CurrencyID=@CurrencyID AND  VPT.IsDr=1 AND VPT.ProductID=TT.ProductID AND VPT.AccountHeadID=@AccountHeadID AND CONVERT(DATE, CONVERT(VARCHAR(12),VPT.TransactionDate,106)) BETWEEN  CONVERT(DATE, CONVERT(VARCHAR(12),@StartDate,106)) AND CONVERT(DATE, CONVERT(VARCHAR(12),@EndDate,106))),0),
					CreditQty=ISNULL((SELECT SUM(VPT.Qty) FROM View_VPTransaction AS VPT WHERE dbo.GetComponentID(VPT.AccountHeadID)=2 AND  VPT.BUID=@BusinessUnitID AND VPT.CurrencyID=@CurrencyID AND  VPT.IsDr=0 AND VPT.ProductID=TT.ProductID AND VPT.AccountHeadID=@AccountHeadID AND CONVERT(DATE, CONVERT(VARCHAR(12),VPT.TransactionDate,106)) BETWEEN  CONVERT(DATE, CONVERT(VARCHAR(12),@StartDate,106)) AND CONVERT(DATE, CONVERT(VARCHAR(12),@EndDate,106))),0),
					CreditAmount=ISNULL((SELECT SUM(VPT.Amount) FROM View_VPTransaction AS VPT WHERE dbo.GetComponentID(VPT.AccountHeadID)=2 AND  VPT.BUID=@BusinessUnitID AND VPT.CurrencyID=@CurrencyID AND  VPT.IsDr=0 AND VPT.ProductID=TT.ProductID AND VPT.AccountHeadID=@AccountHeadID AND CONVERT(DATE, CONVERT(VARCHAR(12),VPT.TransactionDate,106)) BETWEEN  CONVERT(DATE, CONVERT(VARCHAR(12),@StartDate,106)) AND CONVERT(DATE, CONVERT(VARCHAR(12),@EndDate,106))),0)
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
				UPDATE #TempTable2
				SET DebitOpeiningQty=ISNULL((SELECT SUM(AOB.Qty) FROM AccountOpenningBreakdown AS AOB WHERE dbo.GetComponentID(AOB.AccountHeadID)=2 AND AOB.BusinessUnitID=@BusinessUnitID AND AOB.IsDr=1 AND AOB.BreakdownObjID=TT.ProductID AND AOB.AccountingSessionID=@SessionID AND AOB.BreakdownType=4),0),
					CreditOpeiningQty=ISNULL((SELECT SUM(AOB.Qty) FROM AccountOpenningBreakdown AS AOB WHERE dbo.GetComponentID(AOB.AccountHeadID)=2 AND AOB.BusinessUnitID=@BusinessUnitID AND AOB.IsDr=0 AND AOB.BreakdownObjID=TT.ProductID AND AOB.AccountingSessionID=@SessionID AND AOB.BreakdownType=4),0),
					DebitOpeiningValue=ISNULL((SELECT SUM(AOB.OpenningBalance) FROM AccountOpenningBreakdown AS AOB WHERE dbo.GetComponentID(AOB.AccountHeadID)=2 AND AOB.BusinessUnitID=@BusinessUnitID AND AOB.IsDr=1 AND AOB.BreakdownObjID=TT.ProductID AND AOB.AccountingSessionID=@SessionID AND AOB.BreakdownType=4),0),
					CreditOpeiningValue=ISNULL((SELECT SUM(AOB.OpenningBalance) FROM AccountOpenningBreakdown AS AOB WHERE dbo.GetComponentID(AOB.AccountHeadID)=2 AND AOB.BusinessUnitID=@BusinessUnitID AND AOB.IsDr=0 AND AOB.BreakdownObjID=TT.ProductID AND AOB.AccountingSessionID=@SessionID AND AOB.BreakdownType=4),0),
					DebitQty=ISNULL((SELECT SUM(VPT.Qty) FROM View_VPTransaction AS VPT WHERE dbo.GetComponentID(VPT.AccountHeadID)=2 AND   VPT.BUID=@BusinessUnitID AND ISNULL(VPT.ApprovedBy,0)!=0 AND  VPT.IsDr=1 AND VPT.ProductID=TT.ProductID AND CONVERT(DATE, CONVERT(VARCHAR(12),VPT.TransactionDate,106)) BETWEEN  CONVERT(DATE, CONVERT(VARCHAR(12),@SessionStartDate,106)) AND CONVERT(DATE, CONVERT(VARCHAR(12),DATEADD(DAY,-1,@StartDate),106))),0),
					DebitAmount=ISNULL((SELECT SUM(VPT.Amount*VPT.ConversionRate) FROM View_VPTransaction AS VPT WHERE dbo.GetComponentID(VPT.AccountHeadID)=2 AND  VPT.BUID=@BusinessUnitID AND  ISNULL(VPT.ApprovedBy,0)!=0 AND  VPT.IsDr=1 AND VPT.ProductID=TT.ProductID AND CONVERT(DATE, CONVERT(VARCHAR(12),VPT.TransactionDate,106)) BETWEEN  CONVERT(DATE, CONVERT(VARCHAR(12),@SessionStartDate,106)) AND CONVERT(DATE, CONVERT(VARCHAR(12),DATEADD(DAY,-1,@StartDate),106))),0),
					CreditQty=ISNULL((SELECT SUM(VPT.Qty) FROM View_VPTransaction AS VPT WHERE dbo.GetComponentID(VPT.AccountHeadID)=2 AND  VPT.BUID=@BusinessUnitID AND  ISNULL(VPT.ApprovedBy,0)!=0 AND  VPT.IsDr=0 AND VPT.ProductID=TT.ProductID AND CONVERT(DATE, CONVERT(VARCHAR(12),VPT.TransactionDate,106)) BETWEEN  CONVERT(DATE, CONVERT(VARCHAR(12),@SessionStartDate,106)) AND CONVERT(DATE, CONVERT(VARCHAR(12),DATEADD(DAY,-1,@StartDate),106))),0),
					CreditAmount=ISNULL((SELECT SUM(VPT.Amount*VPT.ConversionRate) FROM View_VPTransaction AS VPT WHERE dbo.GetComponentID(VPT.AccountHeadID)=2 AND  VPT.BUID=@BusinessUnitID AND  ISNULL(VPT.ApprovedBy,0)!=0 AND  VPT.IsDr=0 AND VPT.ProductID=TT.ProductID AND CONVERT(DATE, CONVERT(VARCHAR(12),VPT.TransactionDate,106)) BETWEEN  CONVERT(DATE, CONVERT(VARCHAR(12),@SessionStartDate,106)) AND CONVERT(DATE, CONVERT(VARCHAR(12),DATEADD(DAY,-1,@StartDate),106))),0)
				FROM #TempTable2 AS TT
	
				UPDATE #TempTable
				SET DebitQty=ISNULL((SELECT SUM(VPT.Qty) FROM View_VPTransaction AS VPT WHERE dbo.GetComponentID(VPT.AccountHeadID)=2 AND   VPT.BUID=@BusinessUnitID AND ISNULL(VPT.ApprovedBy,0)!=0 AND  VPT.IsDr=1 AND VPT.ProductID=TT.ProductID AND CONVERT(DATE, CONVERT(VARCHAR(12),VPT.TransactionDate,106)) BETWEEN  CONVERT(DATE, CONVERT(VARCHAR(12),@StartDate,106)) AND CONVERT(DATE, CONVERT(VARCHAR(12),@EndDate,106))),0),
					DebitAmount=ISNULL((SELECT SUM(VPT.Amount*VPT.ConversionRate) FROM View_VPTransaction AS VPT WHERE dbo.GetComponentID(VPT.AccountHeadID)=2 AND  VPT.BUID=@BusinessUnitID AND  ISNULL(VPT.ApprovedBy,0)!=0 AND  VPT.IsDr=1 AND VPT.ProductID=TT.ProductID AND CONVERT(DATE, CONVERT(VARCHAR(12),VPT.TransactionDate,106)) BETWEEN  CONVERT(DATE, CONVERT(VARCHAR(12),@StartDate,106)) AND CONVERT(DATE, CONVERT(VARCHAR(12),@EndDate,106))),0),
					CreditQty=ISNULL((SELECT SUM(VPT.Qty) FROM View_VPTransaction AS VPT WHERE dbo.GetComponentID(VPT.AccountHeadID)=2 AND  VPT.BUID=@BusinessUnitID AND  ISNULL(VPT.ApprovedBy,0)!=0 AND  VPT.IsDr=0 AND VPT.ProductID=TT.ProductID AND CONVERT(DATE, CONVERT(VARCHAR(12),VPT.TransactionDate,106)) BETWEEN  CONVERT(DATE, CONVERT(VARCHAR(12),@StartDate,106)) AND CONVERT(DATE, CONVERT(VARCHAR(12),@EndDate,106))),0),
					CreditAmount=ISNULL((SELECT SUM(VPT.Amount*VPT.ConversionRate) FROM View_VPTransaction AS VPT WHERE dbo.GetComponentID(VPT.AccountHeadID)=2 AND  VPT.BUID=@BusinessUnitID AND  ISNULL(VPT.ApprovedBy,0)!=0 AND  VPT.IsDr=0 AND VPT.ProductID=TT.ProductID AND CONVERT(DATE, CONVERT(VARCHAR(12),VPT.TransactionDate,106)) BETWEEN  CONVERT(DATE, CONVERT(VARCHAR(12),@StartDate,106)) AND CONVERT(DATE, CONVERT(VARCHAR(12),@EndDate,106))),0)
				FROM #TempTable AS TT

			END
			ELSE
			BEGIN
				UPDATE #TempTable2
				SET DebitOpeiningQty=ISNULL((SELECT SUM(AOB.Qty) FROM AccountOpenningBreakdown AS AOB WHERE dbo.GetComponentID(AOB.AccountHeadID)=2 AND AOB.BusinessUnitID=@BusinessUnitID AND AOB.IsDr=1 AND AOB.CurrencyID=@CurrencyID AND AOB.BreakdownObjID=TT.ProductID AND AOB.AccountingSessionID=@SessionID AND AOB.BreakdownType=4),0),
					CreditOpeiningQty=ISNULL((SELECT SUM(AOB.Qty) FROM AccountOpenningBreakdown AS AOB WHERE dbo.GetComponentID(AOB.AccountHeadID)=2 AND AOB.BusinessUnitID=@BusinessUnitID AND AOB.IsDr=0 AND AOB.CurrencyID=@CurrencyID AND AOB.BreakdownObjID=TT.ProductID AND AOB.AccountingSessionID=@SessionID AND AOB.BreakdownType=4),0),
					DebitOpeiningValue=ISNULL((SELECT SUM(AOB.AmountInCurrency) FROM AccountOpenningBreakdown AS AOB WHERE dbo.GetComponentID(AOB.AccountHeadID)=2 AND AOB.BusinessUnitID=@BusinessUnitID AND AOB.IsDr=1 AND AOB.CurrencyID=@CurrencyID AND AOB.BreakdownObjID=TT.ProductID AND AOB.AccountingSessionID=@SessionID AND AOB.BreakdownType=4),0),
					CreditOpeiningValue=ISNULL((SELECT SUM(AOB.AmountInCurrency) FROM AccountOpenningBreakdown AS AOB WHERE dbo.GetComponentID(AOB.AccountHeadID)=2 AND AOB.BusinessUnitID=@BusinessUnitID AND AOB.IsDr=0 AND AOB.CurrencyID=@CurrencyID AND AOB.BreakdownObjID=TT.ProductID AND AOB.AccountingSessionID=@SessionID AND AOB.BreakdownType=4),0),
					DebitQty=ISNULL((SELECT SUM(VPT.Qty) FROM View_VPTransaction AS VPT WHERE dbo.GetComponentID(VPT.AccountHeadID)=2 AND  VPT.BUID=@BusinessUnitID AND VPT.ApprovedBy!=0 AND VPT.CurrencyID=@CurrencyID AND  VPT.IsDr=1 AND VPT.ProductID=TT.ProductID AND CONVERT(DATE, CONVERT(VARCHAR(12),VPT.TransactionDate,106)) BETWEEN  CONVERT(DATE, CONVERT(VARCHAR(12),@SessionStartDate,106)) AND CONVERT(DATE, CONVERT(VARCHAR(12),DATEADD(DAY,-1,@StartDate),106))),0),
					DebitAmount=ISNULL((SELECT SUM(VPT.Amount) FROM View_VPTransaction AS VPT WHERE dbo.GetComponentID(VPT.AccountHeadID)=2 AND  VPT.BUID=@BusinessUnitID AND VPT.ApprovedBy!=0 AND VPT.CurrencyID=@CurrencyID AND  VPT.IsDr=1 AND VPT.ProductID=TT.ProductID AND CONVERT(DATE, CONVERT(VARCHAR(12),VPT.TransactionDate,106)) BETWEEN  CONVERT(DATE, CONVERT(VARCHAR(12),@SessionStartDate,106)) AND CONVERT(DATE, CONVERT(VARCHAR(12),DATEADD(DAY,-1,@StartDate),106))),0),
					CreditQty=ISNULL((SELECT SUM(VPT.Qty) FROM View_VPTransaction AS VPT WHERE dbo.GetComponentID(VPT.AccountHeadID)=2 AND  VPT.BUID=@BusinessUnitID AND VPT.ApprovedBy!=0 AND VPT.CurrencyID=@CurrencyID AND VPT.IsDr=0 AND VPT.ProductID=TT.ProductID AND CONVERT(DATE, CONVERT(VARCHAR(12),VPT.TransactionDate,106)) BETWEEN  CONVERT(DATE, CONVERT(VARCHAR(12),@SessionStartDate,106)) AND CONVERT(DATE, CONVERT(VARCHAR(12),DATEADD(DAY,-1,@StartDate),106))),0),
					CreditAmount=ISNULL((SELECT SUM(VPT.Amount) FROM View_VPTransaction AS VPT WHERE dbo.GetComponentID(VPT.AccountHeadID)=2 AND  VPT.BUID=@BusinessUnitID AND VPT.ApprovedBy!=0 AND VPT.CurrencyID=@CurrencyID AND VPT.IsDr=0 AND VPT.ProductID=TT.ProductID AND CONVERT(DATE, CONVERT(VARCHAR(12),VPT.TransactionDate,106)) BETWEEN  CONVERT(DATE, CONVERT(VARCHAR(12),@SessionStartDate,106)) AND CONVERT(DATE, CONVERT(VARCHAR(12),DATEADD(DAY,-1,@StartDate),106))),0)
				FROM #TempTable2 AS TT

				UPDATE #TempTable
				SET DebitQty=ISNULL((SELECT SUM(VPT.Qty) FROM View_VPTransaction AS VPT WHERE dbo.GetComponentID(VPT.AccountHeadID)=2 AND  VPT.BUID=@BusinessUnitID AND VPT.ApprovedBy!=0 AND VPT.CurrencyID=@CurrencyID AND  VPT.IsDr=1 AND VPT.ProductID=TT.ProductID AND CONVERT(DATE, CONVERT(VARCHAR(12),VPT.TransactionDate,106)) BETWEEN  CONVERT(DATE, CONVERT(VARCHAR(12),@StartDate,106)) AND CONVERT(DATE, CONVERT(VARCHAR(12),@EndDate,106))),0),
					DebitAmount=ISNULL((SELECT SUM(VPT.Amount) FROM View_VPTransaction AS VPT WHERE dbo.GetComponentID(VPT.AccountHeadID)=2 AND  VPT.BUID=@BusinessUnitID AND VPT.ApprovedBy!=0 AND VPT.CurrencyID=@CurrencyID AND  VPT.IsDr=1 AND VPT.ProductID=TT.ProductID AND CONVERT(DATE, CONVERT(VARCHAR(12),VPT.TransactionDate,106)) BETWEEN  CONVERT(DATE, CONVERT(VARCHAR(12),@StartDate,106)) AND CONVERT(DATE, CONVERT(VARCHAR(12),@EndDate,106))),0),
					CreditQty=ISNULL((SELECT SUM(VPT.Qty) FROM View_VPTransaction AS VPT WHERE dbo.GetComponentID(VPT.AccountHeadID)=2 AND  VPT.BUID=@BusinessUnitID AND VPT.ApprovedBy!=0 AND VPT.CurrencyID=@CurrencyID AND  VPT.IsDr=0 AND VPT.ProductID=TT.ProductID AND CONVERT(DATE, CONVERT(VARCHAR(12),VPT.TransactionDate,106)) BETWEEN  CONVERT(DATE, CONVERT(VARCHAR(12),@StartDate,106)) AND CONVERT(DATE, CONVERT(VARCHAR(12),@EndDate,106))),0),
					CreditAmount=ISNULL((SELECT SUM(VPT.Amount) FROM View_VPTransaction AS VPT WHERE dbo.GetComponentID(VPT.AccountHeadID)=2 AND  VPT.BUID=@BusinessUnitID AND VPT.ApprovedBy!=0 AND VPT.CurrencyID=@CurrencyID AND  VPT.IsDr=0 AND VPT.ProductID=TT.ProductID AND CONVERT(DATE, CONVERT(VARCHAR(12),VPT.TransactionDate,106)) BETWEEN  CONVERT(DATE, CONVERT(VARCHAR(12),@StartDate,106)) AND CONVERT(DATE, CONVERT(VARCHAR(12),@EndDate,106))),0)
				FROM #TempTable AS TT
			END
		END
		ELSE
		BEGIN
			IF(@CurrencyID=@BaseCurrencyID)
			BEGIN
				UPDATE #TempTable2
				SET DebitOpeiningQty=ISNULL((SELECT SUM(AOB.Qty) FROM AccountOpenningBreakdown AS AOB WHERE dbo.GetComponentID(AOB.AccountHeadID)=2 AND AOB.BusinessUnitID=@BusinessUnitID AND AOB.IsDr=1 AND AOB.BreakdownObjID=TT.ProductID AND AOB.AccountingSessionID=@SessionID AND AOB.BreakdownType=4),0),
					CreditOpeiningQty=ISNULL((SELECT SUM(AOB.Qty) FROM AccountOpenningBreakdown AS AOB WHERE dbo.GetComponentID(AOB.AccountHeadID)=2 AND AOB.BusinessUnitID=@BusinessUnitID AND AOB.IsDr=0 AND AOB.BreakdownObjID=TT.ProductID AND AOB.AccountingSessionID=@SessionID AND AOB.BreakdownType=4),0),
					DebitOpeiningValue=ISNULL((SELECT SUM(AOB.OpenningBalance) FROM AccountOpenningBreakdown AS AOB WHERE dbo.GetComponentID(AOB.AccountHeadID)=2 AND AOB.BusinessUnitID=@BusinessUnitID AND AOB.IsDr=1 AND AOB.BreakdownObjID=TT.ProductID AND AOB.AccountingSessionID=@SessionID AND AOB.BreakdownType=4),0),
					CreditOpeiningValue=ISNULL((SELECT SUM(AOB.OpenningBalance) FROM AccountOpenningBreakdown AS AOB WHERE dbo.GetComponentID(AOB.AccountHeadID)=2 AND AOB.BusinessUnitID=@BusinessUnitID AND AOB.IsDr=0 AND AOB.BreakdownObjID=TT.ProductID AND AOB.AccountingSessionID=@SessionID AND AOB.BreakdownType=4),0),
					DebitQty=ISNULL((SELECT SUM(VPT.Qty) FROM View_VPTransaction AS VPT WHERE dbo.GetComponentID(VPT.AccountHeadID)=2 AND  VPT.BUID=@BusinessUnitID AND VPT.IsDr=1 AND VPT.ProductID=TT.ProductID AND CONVERT(DATE, CONVERT(VARCHAR(12),VPT.TransactionDate,106)) BETWEEN  CONVERT(DATE, CONVERT(VARCHAR(12),@SessionStartDate,106)) AND CONVERT(DATE, CONVERT(VARCHAR(12),DATEADD(DAY,-1,@StartDate),106))),0),
					DebitAmount=ISNULL((SELECT SUM(VPT.Amount*VPT.ConversionRate) FROM View_VPTransaction AS VPT WHERE dbo.GetComponentID(VPT.AccountHeadID)=2 AND  VPT.BUID=@BusinessUnitID AND VPT.IsDr=1 AND VPT.ProductID=TT.ProductID AND CONVERT(DATE, CONVERT(VARCHAR(12),VPT.TransactionDate,106)) BETWEEN  CONVERT(DATE, CONVERT(VARCHAR(12),@SessionStartDate,106)) AND CONVERT(DATE, CONVERT(VARCHAR(12),DATEADD(DAY,-1,@StartDate),106))),0),
					CreditQty=ISNULL((SELECT SUM(VPT.Qty) FROM View_VPTransaction AS VPT WHERE dbo.GetComponentID(VPT.AccountHeadID)=2 AND  VPT.BUID=@BusinessUnitID AND VPT.IsDr=0 AND VPT.ProductID=TT.ProductID AND CONVERT(DATE, CONVERT(VARCHAR(12),VPT.TransactionDate,106)) BETWEEN  CONVERT(DATE, CONVERT(VARCHAR(12),@SessionStartDate,106)) AND CONVERT(DATE, CONVERT(VARCHAR(12),DATEADD(DAY,-1,@StartDate),106))),0),
					CreditAmount=ISNULL((SELECT SUM(VPT.Amount*VPT.ConversionRate) FROM View_VPTransaction AS VPT WHERE dbo.GetComponentID(VPT.AccountHeadID)=2 AND  VPT.BUID=@BusinessUnitID AND VPT.IsDr=0 AND VPT.ProductID=TT.ProductID AND CONVERT(DATE, CONVERT(VARCHAR(12),VPT.TransactionDate,106)) BETWEEN  CONVERT(DATE, CONVERT(VARCHAR(12),@SessionStartDate,106)) AND CONVERT(DATE, CONVERT(VARCHAR(12),DATEADD(DAY,-1,@StartDate),106))),0)
				FROM #TempTable2 AS TT
	
				UPDATE #TempTable
				SET DebitQty=ISNULL((SELECT SUM(VPT.Qty) FROM View_VPTransaction AS VPT WHERE dbo.GetComponentID(VPT.AccountHeadID)=2 AND  VPT.BUID=@BusinessUnitID AND VPT.IsDr=1 AND VPT.ProductID=TT.ProductID AND CONVERT(DATE, CONVERT(VARCHAR(12),VPT.TransactionDate,106)) BETWEEN  CONVERT(DATE, CONVERT(VARCHAR(12),@StartDate,106)) AND CONVERT(DATE, CONVERT(VARCHAR(12),@EndDate,106))),0),
					DebitAmount=ISNULL((SELECT SUM(VPT.Amount*VPT.ConversionRate) FROM View_VPTransaction AS VPT WHERE dbo.GetComponentID(VPT.AccountHeadID)=2 AND  VPT.BUID=@BusinessUnitID AND VPT.IsDr=1 AND VPT.ProductID=TT.ProductID AND CONVERT(DATE, CONVERT(VARCHAR(12),VPT.TransactionDate,106)) BETWEEN  CONVERT(DATE, CONVERT(VARCHAR(12),@StartDate,106)) AND CONVERT(DATE, CONVERT(VARCHAR(12),@EndDate,106))),0),
					CreditQty=ISNULL((SELECT SUM(VPT.Qty) FROM View_VPTransaction AS VPT WHERE dbo.GetComponentID(VPT.AccountHeadID)=2 AND  VPT.BUID=@BusinessUnitID AND VPT.IsDr=0 AND VPT.ProductID=TT.ProductID AND CONVERT(DATE, CONVERT(VARCHAR(12),VPT.TransactionDate,106)) BETWEEN  CONVERT(DATE, CONVERT(VARCHAR(12),@StartDate,106)) AND CONVERT(DATE, CONVERT(VARCHAR(12),@EndDate,106))),0),
					CreditAmount=ISNULL((SELECT SUM(VPT.Amount*VPT.ConversionRate) FROM View_VPTransaction AS VPT WHERE dbo.GetComponentID(VPT.AccountHeadID)=2 AND  VPT.BUID=@BusinessUnitID AND VPT.IsDr=0 AND VPT.ProductID=TT.ProductID AND CONVERT(DATE, CONVERT(VARCHAR(12),VPT.TransactionDate,106)) BETWEEN  CONVERT(DATE, CONVERT(VARCHAR(12),@StartDate,106)) AND CONVERT(DATE, CONVERT(VARCHAR(12),@EndDate,106))),0)
				FROM #TempTable AS TT

			END
			ELSE
			BEGIN
				UPDATE #TempTable2
				SET DebitOpeiningQty=ISNULL((SELECT SUM(AOB.Qty) FROM AccountOpenningBreakdown AS AOB WHERE dbo.GetComponentID(AOB.AccountHeadID)=2 AND AOB.BusinessUnitID=@BusinessUnitID AND AOB.IsDr=1 AND AOB.CurrencyID=@CurrencyID AND AOB.BreakdownObjID=TT.ProductID AND AOB.AccountingSessionID=@SessionID AND AOB.BreakdownType=4),0),
					CreditOpeiningQty=ISNULL((SELECT SUM(AOB.Qty) FROM AccountOpenningBreakdown AS AOB WHERE dbo.GetComponentID(AOB.AccountHeadID)=2 AND AOB.BusinessUnitID=@BusinessUnitID AND AOB.IsDr=0 AND AOB.CurrencyID=@CurrencyID AND AOB.BreakdownObjID=TT.ProductID AND AOB.AccountingSessionID=@SessionID AND AOB.BreakdownType=4),0),
					DebitOpeiningValue=ISNULL((SELECT SUM(AOB.AmountInCurrency) FROM AccountOpenningBreakdown AS AOB WHERE dbo.GetComponentID(AOB.AccountHeadID)=2 AND AOB.BusinessUnitID=@BusinessUnitID AND AOB.IsDr=1 AND AOB.CurrencyID=@CurrencyID AND AOB.BreakdownObjID=TT.ProductID AND AOB.AccountingSessionID=@SessionID AND AOB.BreakdownType=4),0),
					CreditOpeiningValue=ISNULL((SELECT SUM(AOB.AmountInCurrency) FROM AccountOpenningBreakdown AS AOB WHERE dbo.GetComponentID(AOB.AccountHeadID)=2 AND AOB.BusinessUnitID=@BusinessUnitID AND AOB.IsDr=0 AND AOB.CurrencyID=@CurrencyID AND AOB.BreakdownObjID=TT.ProductID AND AOB.AccountingSessionID=@SessionID AND AOB.BreakdownType=4),0),
					DebitQty=ISNULL((SELECT SUM(VPT.Qty) FROM View_VPTransaction AS VPT WHERE dbo.GetComponentID(VPT.AccountHeadID)=2 AND  VPT.BUID=@BusinessUnitID AND VPT.CurrencyID=@CurrencyID AND  VPT.IsDr=1 AND VPT.ProductID=TT.ProductID AND CONVERT(DATE, CONVERT(VARCHAR(12),VPT.TransactionDate,106)) BETWEEN  CONVERT(DATE, CONVERT(VARCHAR(12),@SessionStartDate,106)) AND CONVERT(DATE, CONVERT(VARCHAR(12),DATEADD(DAY,-1,@StartDate),106))),0),
					DebitAmount=ISNULL((SELECT SUM(VPT.Amount) FROM View_VPTransaction AS VPT WHERE dbo.GetComponentID(VPT.AccountHeadID)=2 AND  VPT.BUID=@BusinessUnitID AND VPT.CurrencyID=@CurrencyID AND  VPT.IsDr=1 AND VPT.ProductID=TT.ProductID AND CONVERT(DATE, CONVERT(VARCHAR(12),VPT.TransactionDate,106)) BETWEEN  CONVERT(DATE, CONVERT(VARCHAR(12),@SessionStartDate,106)) AND CONVERT(DATE, CONVERT(VARCHAR(12),DATEADD(DAY,-1,@StartDate),106))),0),
					CreditQty=ISNULL((SELECT SUM(VPT.Qty) FROM View_VPTransaction AS VPT WHERE dbo.GetComponentID(VPT.AccountHeadID)=2 AND  VPT.BUID=@BusinessUnitID AND VPT.CurrencyID=@CurrencyID AND VPT.IsDr=0 AND VPT.ProductID=TT.ProductID AND CONVERT(DATE, CONVERT(VARCHAR(12),VPT.TransactionDate,106)) BETWEEN  CONVERT(DATE, CONVERT(VARCHAR(12),@SessionStartDate,106)) AND CONVERT(DATE, CONVERT(VARCHAR(12),DATEADD(DAY,-1,@StartDate),106))),0),
					CreditAmount=ISNULL((SELECT SUM(VPT.Amount) FROM View_VPTransaction AS VPT WHERE dbo.GetComponentID(VPT.AccountHeadID)=2 AND  VPT.BUID=@BusinessUnitID AND VPT.CurrencyID=@CurrencyID AND VPT.IsDr=0 AND VPT.ProductID=TT.ProductID AND CONVERT(DATE, CONVERT(VARCHAR(12),VPT.TransactionDate,106)) BETWEEN  CONVERT(DATE, CONVERT(VARCHAR(12),@SessionStartDate,106)) AND CONVERT(DATE, CONVERT(VARCHAR(12),DATEADD(DAY,-1,@StartDate),106))),0)
				FROM #TempTable2 AS TT

				UPDATE #TempTable
				SET DebitQty=ISNULL((SELECT SUM(VPT.Qty) FROM View_VPTransaction AS VPT WHERE dbo.GetComponentID(VPT.AccountHeadID)=2 AND  VPT.BUID=@BusinessUnitID AND VPT.CurrencyID=@CurrencyID AND  VPT.IsDr=1 AND VPT.ProductID=TT.ProductID AND CONVERT(DATE, CONVERT(VARCHAR(12),VPT.TransactionDate,106)) BETWEEN  CONVERT(DATE, CONVERT(VARCHAR(12),@StartDate,106)) AND CONVERT(DATE, CONVERT(VARCHAR(12),@EndDate,106))),0),
					DebitAmount=ISNULL((SELECT SUM(VPT.Amount) FROM View_VPTransaction AS VPT WHERE dbo.GetComponentID(VPT.AccountHeadID)=2 AND  VPT.BUID=@BusinessUnitID AND VPT.CurrencyID=@CurrencyID AND  VPT.IsDr=1 AND VPT.ProductID=TT.ProductID AND CONVERT(DATE, CONVERT(VARCHAR(12),VPT.TransactionDate,106)) BETWEEN  CONVERT(DATE, CONVERT(VARCHAR(12),@StartDate,106)) AND CONVERT(DATE, CONVERT(VARCHAR(12),@EndDate,106))),0),
					CreditQty=ISNULL((SELECT SUM(VPT.Qty) FROM View_VPTransaction AS VPT WHERE dbo.GetComponentID(VPT.AccountHeadID)=2 AND  VPT.BUID=@BusinessUnitID AND VPT.CurrencyID=@CurrencyID AND  VPT.IsDr=0 AND VPT.ProductID=TT.ProductID AND CONVERT(DATE, CONVERT(VARCHAR(12),VPT.TransactionDate,106)) BETWEEN  CONVERT(DATE, CONVERT(VARCHAR(12),@StartDate,106)) AND CONVERT(DATE, CONVERT(VARCHAR(12),@EndDate,106))),0),
					CreditAmount=ISNULL((SELECT SUM(VPT.Amount) FROM View_VPTransaction AS VPT WHERE dbo.GetComponentID(VPT.AccountHeadID)=2 AND  VPT.BUID=@BusinessUnitID AND VPT.CurrencyID=@CurrencyID AND  VPT.IsDr=0 AND VPT.ProductID=TT.ProductID AND CONVERT(DATE, CONVERT(VARCHAR(12),VPT.TransactionDate,106)) BETWEEN  CONVERT(DATE, CONVERT(VARCHAR(12),@StartDate,106)) AND CONVERT(DATE, CONVERT(VARCHAR(12),@EndDate,106))),0)
				FROM #TempTable AS TT
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
				UPDATE #TempTable2
				SET DebitOpeiningQty=ISNULL((SELECT SUM(AOB.Qty) FROM AccountOpenningBreakdown AS AOB WHERE dbo.GetComponentID(AOB.AccountHeadID)=2 AND AOB.IsDr=1 AND AOB.BreakdownObjID=TT.ProductID AND AOB.AccountHeadID=@AccountHeadID AND AOB.AccountingSessionID=@SessionID AND AOB.BreakdownType=4),0),
					CreditOpeiningQty=ISNULL((SELECT SUM(AOB.Qty) FROM AccountOpenningBreakdown AS AOB WHERE dbo.GetComponentID(AOB.AccountHeadID)=2 AND AOB.IsDr=0 AND AOB.BreakdownObjID=TT.ProductID AND AOB.AccountHeadID=@AccountHeadID AND AOB.AccountingSessionID=@SessionID AND AOB.BreakdownType=4),0),
					DebitOpeiningValue=ISNULL((SELECT SUM(AOB.OpenningBalance) FROM AccountOpenningBreakdown AS AOB WHERE dbo.GetComponentID(AOB.AccountHeadID)=2 AND AOB.IsDr=1 AND AOB.BreakdownObjID=TT.ProductID AND AOB.AccountHeadID=@AccountHeadID AND AOB.AccountingSessionID=@SessionID AND AOB.BreakdownType=4),0),
					CreditOpeiningValue=ISNULL((SELECT SUM(AOB.OpenningBalance) FROM AccountOpenningBreakdown AS AOB WHERE dbo.GetComponentID(AOB.AccountHeadID)=2 AND AOB.IsDr=0 AND AOB.BreakdownObjID=TT.ProductID AND AOB.AccountHeadID=@AccountHeadID AND AOB.AccountingSessionID=@SessionID AND AOB.BreakdownType=4),0),
					DebitQty=ISNULL((SELECT SUM(VPT.Qty) FROM View_VPTransaction AS VPT WHERE dbo.GetComponentID(VPT.AccountHeadID)=2 AND   ISNULL(VPT.ApprovedBy,0)!=0 AND  VPT.IsDr=1 AND VPT.ProductID=TT.ProductID AND VPT.AccountHeadID=@AccountHeadID AND CONVERT(DATE, CONVERT(VARCHAR(12),VPT.TransactionDate,106)) BETWEEN  CONVERT(DATE, CONVERT(VARCHAR(12),@SessionStartDate,106)) AND CONVERT(DATE, CONVERT(VARCHAR(12),DATEADD(DAY,-1,@StartDate),106))),0),
					DebitAmount=ISNULL((SELECT SUM(VPT.Amount*VPT.ConversionRate) FROM View_VPTransaction AS VPT WHERE dbo.GetComponentID(VPT.AccountHeadID)=2 AND   ISNULL(VPT.ApprovedBy,0)!=0 AND  VPT.IsDr=1 AND VPT.ProductID=TT.ProductID AND VPT.AccountHeadID=@AccountHeadID AND CONVERT(DATE, CONVERT(VARCHAR(12),VPT.TransactionDate,106)) BETWEEN  CONVERT(DATE, CONVERT(VARCHAR(12),@SessionStartDate,106)) AND CONVERT(DATE, CONVERT(VARCHAR(12),DATEADD(DAY,-1,@StartDate),106))),0),
					CreditQty=ISNULL((SELECT SUM(VPT.Qty) FROM View_VPTransaction AS VPT WHERE dbo.GetComponentID(VPT.AccountHeadID)=2 AND   ISNULL(VPT.ApprovedBy,0)!=0 AND  VPT.IsDr=0 AND VPT.ProductID=TT.ProductID AND VPT.AccountHeadID=@AccountHeadID AND CONVERT(DATE, CONVERT(VARCHAR(12),VPT.TransactionDate,106)) BETWEEN  CONVERT(DATE, CONVERT(VARCHAR(12),@SessionStartDate,106)) AND CONVERT(DATE, CONVERT(VARCHAR(12),DATEADD(DAY,-1,@StartDate),106))),0),
					CreditAmount=ISNULL((SELECT SUM(VPT.Amount*VPT.ConversionRate) FROM View_VPTransaction AS VPT WHERE dbo.GetComponentID(VPT.AccountHeadID)=2 AND   ISNULL(VPT.ApprovedBy,0)!=0 AND  VPT.IsDr=0 AND VPT.ProductID=TT.ProductID AND VPT.AccountHeadID=@AccountHeadID AND CONVERT(DATE, CONVERT(VARCHAR(12),VPT.TransactionDate,106)) BETWEEN  CONVERT(DATE, CONVERT(VARCHAR(12),@SessionStartDate,106)) AND CONVERT(DATE, CONVERT(VARCHAR(12),DATEADD(DAY,-1,@StartDate),106))),0)
				FROM #TempTable2 AS TT
	
				UPDATE #TempTable
				SET DebitQty=ISNULL((SELECT SUM(VPT.Qty) FROM View_VPTransaction AS VPT WHERE dbo.GetComponentID(VPT.AccountHeadID)=2 AND   ISNULL(VPT.ApprovedBy,0)!=0 AND  VPT.IsDr=1 AND VPT.ProductID=TT.ProductID AND VPT.AccountHeadID=@AccountHeadID AND CONVERT(DATE, CONVERT(VARCHAR(12),VPT.TransactionDate,106)) BETWEEN  CONVERT(DATE, CONVERT(VARCHAR(12),@StartDate,106)) AND CONVERT(DATE, CONVERT(VARCHAR(12),@EndDate,106))),0),
					DebitAmount=ISNULL((SELECT SUM(VPT.Amount*VPT.ConversionRate) FROM View_VPTransaction AS VPT WHERE dbo.GetComponentID(VPT.AccountHeadID)=2 AND   ISNULL(VPT.ApprovedBy,0)!=0 AND  VPT.IsDr=1 AND VPT.ProductID=TT.ProductID AND VPT.AccountHeadID=@AccountHeadID AND CONVERT(DATE, CONVERT(VARCHAR(12),VPT.TransactionDate,106)) BETWEEN  CONVERT(DATE, CONVERT(VARCHAR(12),@StartDate,106)) AND CONVERT(DATE, CONVERT(VARCHAR(12),@EndDate,106))),0),
					CreditQty=ISNULL((SELECT SUM(VPT.Qty) FROM View_VPTransaction AS VPT WHERE dbo.GetComponentID(VPT.AccountHeadID)=2 AND   ISNULL(VPT.ApprovedBy,0)!=0 AND  VPT.IsDr=0 AND VPT.ProductID=TT.ProductID AND VPT.AccountHeadID=@AccountHeadID AND CONVERT(DATE, CONVERT(VARCHAR(12),VPT.TransactionDate,106)) BETWEEN  CONVERT(DATE, CONVERT(VARCHAR(12),@StartDate,106)) AND CONVERT(DATE, CONVERT(VARCHAR(12),@EndDate,106))),0),
					CreditAmount=ISNULL((SELECT SUM(VPT.Amount*VPT.ConversionRate) FROM View_VPTransaction AS VPT WHERE dbo.GetComponentID(VPT.AccountHeadID)=2 AND   ISNULL(VPT.ApprovedBy,0)!=0 AND  VPT.IsDr=0 AND VPT.ProductID=TT.ProductID AND VPT.AccountHeadID=@AccountHeadID AND CONVERT(DATE, CONVERT(VARCHAR(12),VPT.TransactionDate,106)) BETWEEN  CONVERT(DATE, CONVERT(VARCHAR(12),@StartDate,106)) AND CONVERT(DATE, CONVERT(VARCHAR(12),@EndDate,106))),0)
				FROM #TempTable AS TT

			END
			ELSE
			BEGIN
				UPDATE #TempTable2
				SET DebitOpeiningQty=ISNULL((SELECT SUM(AOB.Qty) FROM AccountOpenningBreakdown AS AOB WHERE dbo.GetComponentID(AOB.AccountHeadID)=2 AND AOB.IsDr=1 AND AOB.CurrencyID=@CurrencyID AND AOB.BreakdownObjID=TT.ProductID AND AOB.AccountHeadID=@AccountHeadID AND AOB.AccountingSessionID=@SessionID AND AOB.BreakdownType=4),0),
					CreditOpeiningQty=ISNULL((SELECT SUM(AOB.Qty) FROM AccountOpenningBreakdown AS AOB WHERE dbo.GetComponentID(AOB.AccountHeadID)=2 AND AOB.IsDr=0 AND AOB.CurrencyID=@CurrencyID AND AOB.BreakdownObjID=TT.ProductID AND AOB.AccountHeadID=@AccountHeadID AND AOB.AccountingSessionID=@SessionID AND AOB.BreakdownType=4),0),
					DebitOpeiningValue=ISNULL((SELECT SUM(AOB.AmountInCurrency) FROM AccountOpenningBreakdown AS AOB WHERE dbo.GetComponentID(AOB.AccountHeadID)=2 AND AOB.IsDr=1 AND AOB.CurrencyID=@CurrencyID AND AOB.BreakdownObjID=TT.ProductID AND AOB.AccountHeadID=@AccountHeadID AND AOB.AccountingSessionID=@SessionID AND AOB.BreakdownType=4),0),
					CreditOpeiningValue=ISNULL((SELECT SUM(AOB.AmountInCurrency) FROM AccountOpenningBreakdown AS AOB WHERE dbo.GetComponentID(AOB.AccountHeadID)=2 AND AOB.IsDr=0 AND AOB.CurrencyID=@CurrencyID AND AOB.BreakdownObjID=TT.ProductID AND AOB.AccountHeadID=@AccountHeadID AND AOB.AccountingSessionID=@SessionID AND AOB.BreakdownType=4),0),
					DebitQty=ISNULL((SELECT SUM(VPT.Qty) FROM View_VPTransaction AS VPT WHERE dbo.GetComponentID(VPT.AccountHeadID)=2 AND  VPT.ApprovedBy!=0 AND VPT.CurrencyID=@CurrencyID AND  VPT.IsDr=1 AND VPT.ProductID=TT.ProductID AND VPT.AccountHeadID=@AccountHeadID AND CONVERT(DATE, CONVERT(VARCHAR(12),VPT.TransactionDate,106)) BETWEEN  CONVERT(DATE, CONVERT(VARCHAR(12),@SessionStartDate,106)) AND CONVERT(DATE, CONVERT(VARCHAR(12),DATEADD(DAY,-1,@StartDate),106))),0),
					DebitAmount=ISNULL((SELECT SUM(VPT.Amount) FROM View_VPTransaction AS VPT WHERE dbo.GetComponentID(VPT.AccountHeadID)=2 AND  VPT.ApprovedBy!=0 AND VPT.CurrencyID=@CurrencyID AND  VPT.IsDr=1 AND VPT.ProductID=TT.ProductID AND VPT.AccountHeadID=@AccountHeadID AND CONVERT(DATE, CONVERT(VARCHAR(12),VPT.TransactionDate,106)) BETWEEN  CONVERT(DATE, CONVERT(VARCHAR(12),@SessionStartDate,106)) AND CONVERT(DATE, CONVERT(VARCHAR(12),DATEADD(DAY,-1,@StartDate),106))),0),
					CreditQty=ISNULL((SELECT SUM(VPT.Qty) FROM View_VPTransaction AS VPT WHERE dbo.GetComponentID(VPT.AccountHeadID)=2 AND  VPT.ApprovedBy!=0 AND VPT.CurrencyID=@CurrencyID AND VPT.IsDr=0 AND VPT.ProductID=TT.ProductID AND VPT.AccountHeadID=@AccountHeadID AND CONVERT(DATE, CONVERT(VARCHAR(12),VPT.TransactionDate,106)) BETWEEN  CONVERT(DATE, CONVERT(VARCHAR(12),@SessionStartDate,106)) AND CONVERT(DATE, CONVERT(VARCHAR(12),DATEADD(DAY,-1,@StartDate),106))),0),
					CreditAmount=ISNULL((SELECT SUM(VPT.Amount) FROM View_VPTransaction AS VPT WHERE dbo.GetComponentID(VPT.AccountHeadID)=2 AND  VPT.ApprovedBy!=0 AND VPT.CurrencyID=@CurrencyID AND VPT.IsDr=0 AND VPT.ProductID=TT.ProductID AND VPT.AccountHeadID=@AccountHeadID AND CONVERT(DATE, CONVERT(VARCHAR(12),VPT.TransactionDate,106)) BETWEEN  CONVERT(DATE, CONVERT(VARCHAR(12),@SessionStartDate,106)) AND CONVERT(DATE, CONVERT(VARCHAR(12),DATEADD(DAY,-1,@StartDate),106))),0)
				FROM #TempTable2 AS TT

				UPDATE #TempTable
				SET DebitQty=ISNULL((SELECT SUM(VPT.Qty) FROM View_VPTransaction AS VPT WHERE dbo.GetComponentID(VPT.AccountHeadID)=2 AND  VPT.ApprovedBy!=0 AND VPT.CurrencyID=@CurrencyID AND  VPT.IsDr=1 AND VPT.ProductID=TT.ProductID AND VPT.AccountHeadID=@AccountHeadID AND CONVERT(DATE, CONVERT(VARCHAR(12),VPT.TransactionDate,106)) BETWEEN  CONVERT(DATE, CONVERT(VARCHAR(12),@StartDate,106)) AND CONVERT(DATE, CONVERT(VARCHAR(12),@EndDate,106))),0),
					DebitAmount=ISNULL((SELECT SUM(VPT.Amount) FROM View_VPTransaction AS VPT WHERE dbo.GetComponentID(VPT.AccountHeadID)=2 AND  VPT.ApprovedBy!=0 AND VPT.CurrencyID=@CurrencyID AND  VPT.IsDr=1 AND VPT.ProductID=TT.ProductID AND VPT.AccountHeadID=@AccountHeadID AND CONVERT(DATE, CONVERT(VARCHAR(12),VPT.TransactionDate,106)) BETWEEN  CONVERT(DATE, CONVERT(VARCHAR(12),@StartDate,106)) AND CONVERT(DATE, CONVERT(VARCHAR(12),@EndDate,106))),0),
					CreditQty=ISNULL((SELECT SUM(VPT.Qty) FROM View_VPTransaction AS VPT WHERE dbo.GetComponentID(VPT.AccountHeadID)=2 AND  VPT.ApprovedBy!=0 AND VPT.CurrencyID=@CurrencyID AND  VPT.IsDr=0 AND VPT.ProductID=TT.ProductID AND VPT.AccountHeadID=@AccountHeadID AND CONVERT(DATE, CONVERT(VARCHAR(12),VPT.TransactionDate,106)) BETWEEN  CONVERT(DATE, CONVERT(VARCHAR(12),@StartDate,106)) AND CONVERT(DATE, CONVERT(VARCHAR(12),@EndDate,106))),0),
					CreditAmount=ISNULL((SELECT SUM(VPT.Amount) FROM View_VPTransaction AS VPT WHERE dbo.GetComponentID(VPT.AccountHeadID)=2 AND  VPT.ApprovedBy!=0 AND VPT.CurrencyID=@CurrencyID AND  VPT.IsDr=0 AND VPT.ProductID=TT.ProductID AND VPT.AccountHeadID=@AccountHeadID AND CONVERT(DATE, CONVERT(VARCHAR(12),VPT.TransactionDate,106)) BETWEEN  CONVERT(DATE, CONVERT(VARCHAR(12),@StartDate,106)) AND CONVERT(DATE, CONVERT(VARCHAR(12),@EndDate,106))),0)
				FROM #TempTable AS TT
			END
		END
		ELSE
		BEGIN
			IF(@CurrencyID=@BaseCurrencyID)
			BEGIN
				UPDATE #TempTable2
				SET DebitOpeiningQty=ISNULL((SELECT SUM(AOB.Qty) FROM AccountOpenningBreakdown AS AOB WHERE dbo.GetComponentID(AOB.AccountHeadID)=2 AND AOB.IsDr=1 AND AOB.BreakdownObjID=TT.ProductID AND AOB.AccountHeadID=@AccountHeadID AND AOB.AccountingSessionID=@SessionID AND AOB.BreakdownType=4),0),
					CreditOpeiningQty=ISNULL((SELECT SUM(AOB.Qty) FROM AccountOpenningBreakdown AS AOB WHERE dbo.GetComponentID(AOB.AccountHeadID)=2 AND AOB.IsDr=0 AND AOB.BreakdownObjID=TT.ProductID AND AOB.AccountHeadID=@AccountHeadID AND AOB.AccountingSessionID=@SessionID AND AOB.BreakdownType=4),0),
					DebitOpeiningValue=ISNULL((SELECT SUM(AOB.OpenningBalance) FROM AccountOpenningBreakdown AS AOB WHERE dbo.GetComponentID(AOB.AccountHeadID)=2 AND AOB.IsDr=1 AND AOB.BreakdownObjID=TT.ProductID AND AOB.AccountHeadID=@AccountHeadID AND AOB.AccountingSessionID=@SessionID AND AOB.BreakdownType=4),0),
					CreditOpeiningValue=ISNULL((SELECT SUM(AOB.OpenningBalance) FROM AccountOpenningBreakdown AS AOB WHERE dbo.GetComponentID(AOB.AccountHeadID)=2 AND AOB.IsDr=0 AND AOB.BreakdownObjID=TT.ProductID AND AOB.AccountHeadID=@AccountHeadID AND AOB.AccountingSessionID=@SessionID AND AOB.BreakdownType=4),0),
					DebitQty=ISNULL((SELECT SUM(VPT.Qty) FROM View_VPTransaction AS VPT WHERE dbo.GetComponentID(VPT.AccountHeadID)=2 AND  VPT.IsDr=1 AND VPT.ProductID=TT.ProductID AND VPT.AccountHeadID=@AccountHeadID AND CONVERT(DATE, CONVERT(VARCHAR(12),VPT.TransactionDate,106)) BETWEEN  CONVERT(DATE, CONVERT(VARCHAR(12),@SessionStartDate,106)) AND CONVERT(DATE, CONVERT(VARCHAR(12),DATEADD(DAY,-1,@StartDate),106))),0),
					DebitAmount=ISNULL((SELECT SUM(VPT.Amount*VPT.ConversionRate) FROM View_VPTransaction AS VPT WHERE dbo.GetComponentID(VPT.AccountHeadID)=2 AND  VPT.IsDr=1 AND VPT.ProductID=TT.ProductID AND VPT.AccountHeadID=@AccountHeadID AND CONVERT(DATE, CONVERT(VARCHAR(12),VPT.TransactionDate,106)) BETWEEN  CONVERT(DATE, CONVERT(VARCHAR(12),@SessionStartDate,106)) AND CONVERT(DATE, CONVERT(VARCHAR(12),DATEADD(DAY,-1,@StartDate),106))),0),
					CreditQty=ISNULL((SELECT SUM(VPT.Qty) FROM View_VPTransaction AS VPT WHERE dbo.GetComponentID(VPT.AccountHeadID)=2 AND  VPT.IsDr=0 AND VPT.ProductID=TT.ProductID AND VPT.AccountHeadID=@AccountHeadID AND CONVERT(DATE, CONVERT(VARCHAR(12),VPT.TransactionDate,106)) BETWEEN  CONVERT(DATE, CONVERT(VARCHAR(12),@SessionStartDate,106)) AND CONVERT(DATE, CONVERT(VARCHAR(12),DATEADD(DAY,-1,@StartDate),106))),0),
					CreditAmount=ISNULL((SELECT SUM(VPT.Amount*VPT.ConversionRate) FROM View_VPTransaction AS VPT WHERE dbo.GetComponentID(VPT.AccountHeadID)=2 AND  VPT.IsDr=0 AND VPT.ProductID=TT.ProductID AND VPT.AccountHeadID=@AccountHeadID AND CONVERT(DATE, CONVERT(VARCHAR(12),VPT.TransactionDate,106)) BETWEEN  CONVERT(DATE, CONVERT(VARCHAR(12),@SessionStartDate,106)) AND CONVERT(DATE, CONVERT(VARCHAR(12),DATEADD(DAY,-1,@StartDate),106))),0)
				FROM #TempTable2 AS TT
	
				UPDATE #TempTable
				SET DebitQty=ISNULL((SELECT SUM(VPT.Qty) FROM View_VPTransaction AS VPT WHERE dbo.GetComponentID(VPT.AccountHeadID)=2 AND  VPT.IsDr=1 AND VPT.ProductID=TT.ProductID AND VPT.AccountHeadID=@AccountHeadID AND CONVERT(DATE, CONVERT(VARCHAR(12),VPT.TransactionDate,106)) BETWEEN  CONVERT(DATE, CONVERT(VARCHAR(12),@StartDate,106)) AND CONVERT(DATE, CONVERT(VARCHAR(12),@EndDate,106))),0),
					DebitAmount=ISNULL((SELECT SUM(VPT.Amount*VPT.ConversionRate) FROM View_VPTransaction AS VPT WHERE dbo.GetComponentID(VPT.AccountHeadID)=2 AND  VPT.IsDr=1 AND VPT.ProductID=TT.ProductID AND VPT.AccountHeadID=@AccountHeadID AND CONVERT(DATE, CONVERT(VARCHAR(12),VPT.TransactionDate,106)) BETWEEN  CONVERT(DATE, CONVERT(VARCHAR(12),@StartDate,106)) AND CONVERT(DATE, CONVERT(VARCHAR(12),@EndDate,106))),0),
					CreditQty=ISNULL((SELECT SUM(VPT.Qty) FROM View_VPTransaction AS VPT WHERE dbo.GetComponentID(VPT.AccountHeadID)=2 AND  VPT.IsDr=0 AND VPT.ProductID=TT.ProductID AND VPT.AccountHeadID=@AccountHeadID AND CONVERT(DATE, CONVERT(VARCHAR(12),VPT.TransactionDate,106)) BETWEEN  CONVERT(DATE, CONVERT(VARCHAR(12),@StartDate,106)) AND CONVERT(DATE, CONVERT(VARCHAR(12),@EndDate,106))),0),
					CreditAmount=ISNULL((SELECT SUM(VPT.Amount*VPT.ConversionRate) FROM View_VPTransaction AS VPT WHERE dbo.GetComponentID(VPT.AccountHeadID)=2 AND  VPT.IsDr=0 AND VPT.ProductID=TT.ProductID AND VPT.AccountHeadID=@AccountHeadID AND CONVERT(DATE, CONVERT(VARCHAR(12),VPT.TransactionDate,106)) BETWEEN  CONVERT(DATE, CONVERT(VARCHAR(12),@StartDate,106)) AND CONVERT(DATE, CONVERT(VARCHAR(12),@EndDate,106))),0)
				FROM #TempTable AS TT

			END
			ELSE
			BEGIN
				UPDATE #TempTable2
				SET DebitOpeiningQty=ISNULL((SELECT SUM(AOB.Qty) FROM AccountOpenningBreakdown AS AOB WHERE dbo.GetComponentID(AOB.AccountHeadID)=2 AND AOB.IsDr=1 AND AOB.CurrencyID=@CurrencyID AND AOB.BreakdownObjID=TT.ProductID AND AOB.AccountHeadID=@AccountHeadID AND AOB.AccountingSessionID=@SessionID AND AOB.BreakdownType=4),0),
					CreditOpeiningQty=ISNULL((SELECT SUM(AOB.Qty) FROM AccountOpenningBreakdown AS AOB WHERE dbo.GetComponentID(AOB.AccountHeadID)=2 AND AOB.IsDr=0 AND AOB.CurrencyID=@CurrencyID AND AOB.BreakdownObjID=TT.ProductID AND AOB.AccountHeadID=@AccountHeadID AND AOB.AccountingSessionID=@SessionID AND AOB.BreakdownType=4),0),
					DebitOpeiningValue=ISNULL((SELECT SUM(AOB.AmountInCurrency) FROM AccountOpenningBreakdown AS AOB WHERE dbo.GetComponentID(AOB.AccountHeadID)=2 AND AOB.IsDr=1 AND AOB.CurrencyID=@CurrencyID AND AOB.BreakdownObjID=TT.ProductID AND AOB.AccountHeadID=@AccountHeadID AND AOB.AccountingSessionID=@SessionID AND AOB.BreakdownType=4),0),
					CreditOpeiningValue=ISNULL((SELECT SUM(AOB.AmountInCurrency) FROM AccountOpenningBreakdown AS AOB WHERE dbo.GetComponentID(AOB.AccountHeadID)=2 AND AOB.IsDr=0 AND AOB.CurrencyID=@CurrencyID AND AOB.BreakdownObjID=TT.ProductID AND AOB.AccountHeadID=@AccountHeadID AND AOB.AccountingSessionID=@SessionID AND AOB.BreakdownType=4),0),
					DebitQty=ISNULL((SELECT SUM(VPT.Qty) FROM View_VPTransaction AS VPT WHERE dbo.GetComponentID(VPT.AccountHeadID)=2 AND  VPT.CurrencyID=@CurrencyID AND  VPT.IsDr=1 AND VPT.ProductID=TT.ProductID AND VPT.AccountHeadID=@AccountHeadID AND CONVERT(DATE, CONVERT(VARCHAR(12),VPT.TransactionDate,106)) BETWEEN  CONVERT(DATE, CONVERT(VARCHAR(12),@SessionStartDate,106)) AND CONVERT(DATE, CONVERT(VARCHAR(12),DATEADD(DAY,-1,@StartDate),106))),0),
					DebitAmount=ISNULL((SELECT SUM(VPT.Amount) FROM View_VPTransaction AS VPT WHERE dbo.GetComponentID(VPT.AccountHeadID)=2 AND  VPT.CurrencyID=@CurrencyID AND  VPT.IsDr=1 AND VPT.ProductID=TT.ProductID AND VPT.AccountHeadID=@AccountHeadID AND CONVERT(DATE, CONVERT(VARCHAR(12),VPT.TransactionDate,106)) BETWEEN  CONVERT(DATE, CONVERT(VARCHAR(12),@SessionStartDate,106)) AND CONVERT(DATE, CONVERT(VARCHAR(12),DATEADD(DAY,-1,@StartDate),106))),0),
					CreditQty=ISNULL((SELECT SUM(VPT.Qty) FROM View_VPTransaction AS VPT WHERE dbo.GetComponentID(VPT.AccountHeadID)=2 AND  VPT.CurrencyID=@CurrencyID AND VPT.IsDr=0 AND VPT.ProductID=TT.ProductID AND VPT.AccountHeadID=@AccountHeadID AND CONVERT(DATE, CONVERT(VARCHAR(12),VPT.TransactionDate,106)) BETWEEN  CONVERT(DATE, CONVERT(VARCHAR(12),@SessionStartDate,106)) AND CONVERT(DATE, CONVERT(VARCHAR(12),DATEADD(DAY,-1,@StartDate),106))),0),
					CreditAmount=ISNULL((SELECT SUM(VPT.Amount) FROM View_VPTransaction AS VPT WHERE dbo.GetComponentID(VPT.AccountHeadID)=2 AND  VPT.CurrencyID=@CurrencyID AND VPT.IsDr=0 AND VPT.ProductID=TT.ProductID AND VPT.AccountHeadID=@AccountHeadID AND CONVERT(DATE, CONVERT(VARCHAR(12),VPT.TransactionDate,106)) BETWEEN  CONVERT(DATE, CONVERT(VARCHAR(12),@SessionStartDate,106)) AND CONVERT(DATE, CONVERT(VARCHAR(12),DATEADD(DAY,-1,@StartDate),106))),0)
				FROM #TempTable2 AS TT

				UPDATE #TempTable
				SET DebitQty=ISNULL((SELECT SUM(VPT.Qty) FROM View_VPTransaction AS VPT WHERE dbo.GetComponentID(VPT.AccountHeadID)=2 AND  VPT.CurrencyID=@CurrencyID AND  VPT.IsDr=1 AND VPT.ProductID=TT.ProductID AND VPT.AccountHeadID=@AccountHeadID AND CONVERT(DATE, CONVERT(VARCHAR(12),VPT.TransactionDate,106)) BETWEEN  CONVERT(DATE, CONVERT(VARCHAR(12),@StartDate,106)) AND CONVERT(DATE, CONVERT(VARCHAR(12),@EndDate,106))),0),
					DebitAmount=ISNULL((SELECT SUM(VPT.Amount) FROM View_VPTransaction AS VPT WHERE dbo.GetComponentID(VPT.AccountHeadID)=2 AND  VPT.CurrencyID=@CurrencyID AND  VPT.IsDr=1 AND VPT.ProductID=TT.ProductID AND VPT.AccountHeadID=@AccountHeadID AND CONVERT(DATE, CONVERT(VARCHAR(12),VPT.TransactionDate,106)) BETWEEN  CONVERT(DATE, CONVERT(VARCHAR(12),@StartDate,106)) AND CONVERT(DATE, CONVERT(VARCHAR(12),@EndDate,106))),0),
					CreditQty=ISNULL((SELECT SUM(VPT.Qty) FROM View_VPTransaction AS VPT WHERE dbo.GetComponentID(VPT.AccountHeadID)=2 AND  VPT.CurrencyID=@CurrencyID AND  VPT.IsDr=0 AND VPT.ProductID=TT.ProductID AND VPT.AccountHeadID=@AccountHeadID AND CONVERT(DATE, CONVERT(VARCHAR(12),VPT.TransactionDate,106)) BETWEEN  CONVERT(DATE, CONVERT(VARCHAR(12),@StartDate,106)) AND CONVERT(DATE, CONVERT(VARCHAR(12),@EndDate,106))),0),
					CreditAmount=ISNULL((SELECT SUM(VPT.Amount) FROM View_VPTransaction AS VPT WHERE dbo.GetComponentID(VPT.AccountHeadID)=2 AND  VPT.CurrencyID=@CurrencyID AND  VPT.IsDr=0 AND VPT.ProductID=TT.ProductID AND VPT.AccountHeadID=@AccountHeadID AND CONVERT(DATE, CONVERT(VARCHAR(12),VPT.TransactionDate,106)) BETWEEN  CONVERT(DATE, CONVERT(VARCHAR(12),@StartDate,106)) AND CONVERT(DATE, CONVERT(VARCHAR(12),@EndDate,106))),0)
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
				UPDATE #TempTable2
				SET DebitOpeiningQty=ISNULL((SELECT SUM(AOB.Qty) FROM AccountOpenningBreakdown AS AOB WHERE dbo.GetComponentID(AOB.AccountHeadID)=2 AND AOB.IsDr=1 AND AOB.BreakdownObjID=TT.ProductID AND AOB.AccountingSessionID=@SessionID AND AOB.BreakdownType=4),0),
					CreditOpeiningQty=ISNULL((SELECT SUM(AOB.Qty) FROM AccountOpenningBreakdown AS AOB WHERE dbo.GetComponentID(AOB.AccountHeadID)=2 AND AOB.IsDr=0 AND AOB.BreakdownObjID=TT.ProductID AND AOB.AccountingSessionID=@SessionID AND AOB.BreakdownType=4),0),
					DebitOpeiningValue=ISNULL((SELECT SUM(AOB.OpenningBalance) FROM AccountOpenningBreakdown AS AOB WHERE dbo.GetComponentID(AOB.AccountHeadID)=2 AND AOB.IsDr=1 AND AOB.BreakdownObjID=TT.ProductID AND AOB.AccountingSessionID=@SessionID AND AOB.BreakdownType=4),0),
					CreditOpeiningValue=ISNULL((SELECT SUM(AOB.OpenningBalance) FROM AccountOpenningBreakdown AS AOB WHERE dbo.GetComponentID(AOB.AccountHeadID)=2 AND AOB.IsDr=0 AND AOB.BreakdownObjID=TT.ProductID AND AOB.AccountingSessionID=@SessionID AND AOB.BreakdownType=4),0),
					DebitQty=ISNULL((SELECT SUM(VPT.Qty) FROM View_VPTransaction AS VPT WHERE dbo.GetComponentID(VPT.AccountHeadID)=2 AND   ISNULL(VPT.ApprovedBy,0)!=0 AND  VPT.IsDr=1 AND VPT.ProductID=TT.ProductID AND CONVERT(DATE, CONVERT(VARCHAR(12),VPT.TransactionDate,106)) BETWEEN  CONVERT(DATE, CONVERT(VARCHAR(12),@SessionStartDate,106)) AND CONVERT(DATE, CONVERT(VARCHAR(12),DATEADD(DAY,-1,@StartDate),106))),0),
					DebitAmount=ISNULL((SELECT SUM(VPT.Amount*VPT.ConversionRate) FROM View_VPTransaction AS VPT WHERE dbo.GetComponentID(VPT.AccountHeadID)=2 AND   ISNULL(VPT.ApprovedBy,0)!=0 AND  VPT.IsDr=1 AND VPT.ProductID=TT.ProductID AND CONVERT(DATE, CONVERT(VARCHAR(12),VPT.TransactionDate,106)) BETWEEN  CONVERT(DATE, CONVERT(VARCHAR(12),@SessionStartDate,106)) AND CONVERT(DATE, CONVERT(VARCHAR(12),DATEADD(DAY,-1,@StartDate),106))),0),
					CreditQty=ISNULL((SELECT SUM(VPT.Qty) FROM View_VPTransaction AS VPT WHERE dbo.GetComponentID(VPT.AccountHeadID)=2 AND   ISNULL(VPT.ApprovedBy,0)!=0 AND  VPT.IsDr=0 AND VPT.ProductID=TT.ProductID AND CONVERT(DATE, CONVERT(VARCHAR(12),VPT.TransactionDate,106)) BETWEEN  CONVERT(DATE, CONVERT(VARCHAR(12),@SessionStartDate,106)) AND CONVERT(DATE, CONVERT(VARCHAR(12),DATEADD(DAY,-1,@StartDate),106))),0),
					CreditAmount=ISNULL((SELECT SUM(VPT.Amount*VPT.ConversionRate) FROM View_VPTransaction AS VPT WHERE dbo.GetComponentID(VPT.AccountHeadID)=2 AND   ISNULL(VPT.ApprovedBy,0)!=0 AND  VPT.IsDr=0 AND VPT.ProductID=TT.ProductID AND CONVERT(DATE, CONVERT(VARCHAR(12),VPT.TransactionDate,106)) BETWEEN  CONVERT(DATE, CONVERT(VARCHAR(12),@SessionStartDate,106)) AND CONVERT(DATE, CONVERT(VARCHAR(12),DATEADD(DAY,-1,@StartDate),106))),0)
				FROM #TempTable2 AS TT
	
				UPDATE #TempTable
				SET DebitQty=ISNULL((SELECT SUM(VPT.Qty) FROM View_VPTransaction AS VPT WHERE dbo.GetComponentID(VPT.AccountHeadID)=2 AND   ISNULL(VPT.ApprovedBy,0)!=0 AND  VPT.IsDr=1 AND VPT.ProductID=TT.ProductID AND CONVERT(DATE, CONVERT(VARCHAR(12),VPT.TransactionDate,106)) BETWEEN  CONVERT(DATE, CONVERT(VARCHAR(12),@StartDate,106)) AND CONVERT(DATE, CONVERT(VARCHAR(12),@EndDate,106))),0),
					DebitAmount=ISNULL((SELECT SUM(VPT.Amount*VPT.ConversionRate) FROM View_VPTransaction AS VPT WHERE dbo.GetComponentID(VPT.AccountHeadID)=2 AND   ISNULL(VPT.ApprovedBy,0)!=0 AND  VPT.IsDr=1 AND VPT.ProductID=TT.ProductID AND CONVERT(DATE, CONVERT(VARCHAR(12),VPT.TransactionDate,106)) BETWEEN  CONVERT(DATE, CONVERT(VARCHAR(12),@StartDate,106)) AND CONVERT(DATE, CONVERT(VARCHAR(12),@EndDate,106))),0),
					CreditQty=ISNULL((SELECT SUM(VPT.Qty) FROM View_VPTransaction AS VPT WHERE dbo.GetComponentID(VPT.AccountHeadID)=2 AND   ISNULL(VPT.ApprovedBy,0)!=0 AND  VPT.IsDr=0 AND VPT.ProductID=TT.ProductID AND CONVERT(DATE, CONVERT(VARCHAR(12),VPT.TransactionDate,106)) BETWEEN  CONVERT(DATE, CONVERT(VARCHAR(12),@StartDate,106)) AND CONVERT(DATE, CONVERT(VARCHAR(12),@EndDate,106))),0),
					CreditAmount=ISNULL((SELECT SUM(VPT.Amount*VPT.ConversionRate) FROM View_VPTransaction AS VPT WHERE dbo.GetComponentID(VPT.AccountHeadID)=2 AND   ISNULL(VPT.ApprovedBy,0)!=0 AND  VPT.IsDr=0 AND VPT.ProductID=TT.ProductID AND CONVERT(DATE, CONVERT(VARCHAR(12),VPT.TransactionDate,106)) BETWEEN  CONVERT(DATE, CONVERT(VARCHAR(12),@StartDate,106)) AND CONVERT(DATE, CONVERT(VARCHAR(12),@EndDate,106))),0)
				FROM #TempTable AS TT

			END
			ELSE
			BEGIN
				UPDATE #TempTable2
				SET DebitOpeiningQty=ISNULL((SELECT SUM(AOB.Qty) FROM AccountOpenningBreakdown AS AOB WHERE dbo.GetComponentID(AOB.AccountHeadID)=2 AND AOB.IsDr=1 AND AOB.CurrencyID=@CurrencyID AND AOB.BreakdownObjID=TT.ProductID AND AOB.AccountingSessionID=@SessionID AND AOB.BreakdownType=4),0),
					CreditOpeiningQty=ISNULL((SELECT SUM(AOB.Qty) FROM AccountOpenningBreakdown AS AOB WHERE dbo.GetComponentID(AOB.AccountHeadID)=2 AND AOB.IsDr=0 AND AOB.CurrencyID=@CurrencyID AND AOB.BreakdownObjID=TT.ProductID AND AOB.AccountingSessionID=@SessionID AND AOB.BreakdownType=4),0),
					DebitOpeiningValue=ISNULL((SELECT SUM(AOB.AmountInCurrency) FROM AccountOpenningBreakdown AS AOB WHERE dbo.GetComponentID(AOB.AccountHeadID)=2 AND AOB.IsDr=1 AND AOB.CurrencyID=@CurrencyID AND AOB.BreakdownObjID=TT.ProductID AND AOB.AccountingSessionID=@SessionID AND AOB.BreakdownType=4),0),
					CreditOpeiningValue=ISNULL((SELECT SUM(AOB.AmountInCurrency) FROM AccountOpenningBreakdown AS AOB WHERE dbo.GetComponentID(AOB.AccountHeadID)=2 AND AOB.IsDr=0 AND AOB.CurrencyID=@CurrencyID AND AOB.BreakdownObjID=TT.ProductID AND AOB.AccountingSessionID=@SessionID AND AOB.BreakdownType=4),0),
					DebitQty=ISNULL((SELECT SUM(VPT.Qty) FROM View_VPTransaction AS VPT WHERE dbo.GetComponentID(VPT.AccountHeadID)=2 AND  VPT.ApprovedBy!=0 AND VPT.CurrencyID=@CurrencyID AND  VPT.IsDr=1 AND VPT.ProductID=TT.ProductID AND CONVERT(DATE, CONVERT(VARCHAR(12),VPT.TransactionDate,106)) BETWEEN  CONVERT(DATE, CONVERT(VARCHAR(12),@SessionStartDate,106)) AND CONVERT(DATE, CONVERT(VARCHAR(12),DATEADD(DAY,-1,@StartDate),106))),0),
					DebitAmount=ISNULL((SELECT SUM(VPT.Amount) FROM View_VPTransaction AS VPT WHERE dbo.GetComponentID(VPT.AccountHeadID)=2 AND  VPT.ApprovedBy!=0 AND VPT.CurrencyID=@CurrencyID AND  VPT.IsDr=1 AND VPT.ProductID=TT.ProductID AND CONVERT(DATE, CONVERT(VARCHAR(12),VPT.TransactionDate,106)) BETWEEN  CONVERT(DATE, CONVERT(VARCHAR(12),@SessionStartDate,106)) AND CONVERT(DATE, CONVERT(VARCHAR(12),DATEADD(DAY,-1,@StartDate),106))),0),
					CreditQty=ISNULL((SELECT SUM(VPT.Qty) FROM View_VPTransaction AS VPT WHERE dbo.GetComponentID(VPT.AccountHeadID)=2 AND  VPT.ApprovedBy!=0 AND VPT.CurrencyID=@CurrencyID AND VPT.IsDr=0 AND VPT.ProductID=TT.ProductID AND CONVERT(DATE, CONVERT(VARCHAR(12),VPT.TransactionDate,106)) BETWEEN  CONVERT(DATE, CONVERT(VARCHAR(12),@SessionStartDate,106)) AND CONVERT(DATE, CONVERT(VARCHAR(12),DATEADD(DAY,-1,@StartDate),106))),0),
					CreditAmount=ISNULL((SELECT SUM(VPT.Amount) FROM View_VPTransaction AS VPT WHERE dbo.GetComponentID(VPT.AccountHeadID)=2 AND  VPT.ApprovedBy!=0 AND VPT.CurrencyID=@CurrencyID AND VPT.IsDr=0 AND VPT.ProductID=TT.ProductID AND CONVERT(DATE, CONVERT(VARCHAR(12),VPT.TransactionDate,106)) BETWEEN  CONVERT(DATE, CONVERT(VARCHAR(12),@SessionStartDate,106)) AND CONVERT(DATE, CONVERT(VARCHAR(12),DATEADD(DAY,-1,@StartDate),106))),0)
				FROM #TempTable2 AS TT

				UPDATE #TempTable
				SET DebitQty=ISNULL((SELECT SUM(VPT.Qty) FROM View_VPTransaction AS VPT WHERE dbo.GetComponentID(VPT.AccountHeadID)=2 AND  VPT.ApprovedBy!=0 AND VPT.CurrencyID=@CurrencyID AND  VPT.IsDr=1 AND VPT.ProductID=TT.ProductID AND CONVERT(DATE, CONVERT(VARCHAR(12),VPT.TransactionDate,106)) BETWEEN  CONVERT(DATE, CONVERT(VARCHAR(12),@StartDate,106)) AND CONVERT(DATE, CONVERT(VARCHAR(12),@EndDate,106))),0),
					DebitAmount=ISNULL((SELECT SUM(VPT.Amount) FROM View_VPTransaction AS VPT WHERE dbo.GetComponentID(VPT.AccountHeadID)=2 AND  VPT.ApprovedBy!=0 AND VPT.CurrencyID=@CurrencyID AND  VPT.IsDr=1 AND VPT.ProductID=TT.ProductID AND CONVERT(DATE, CONVERT(VARCHAR(12),VPT.TransactionDate,106)) BETWEEN  CONVERT(DATE, CONVERT(VARCHAR(12),@StartDate,106)) AND CONVERT(DATE, CONVERT(VARCHAR(12),@EndDate,106))),0),
					CreditQty=ISNULL((SELECT SUM(VPT.Qty) FROM View_VPTransaction AS VPT WHERE dbo.GetComponentID(VPT.AccountHeadID)=2 AND  VPT.ApprovedBy!=0 AND VPT.CurrencyID=@CurrencyID AND  VPT.IsDr=0 AND VPT.ProductID=TT.ProductID AND CONVERT(DATE, CONVERT(VARCHAR(12),VPT.TransactionDate,106)) BETWEEN  CONVERT(DATE, CONVERT(VARCHAR(12),@StartDate,106)) AND CONVERT(DATE, CONVERT(VARCHAR(12),@EndDate,106))),0),
					CreditAmount=ISNULL((SELECT SUM(VPT.Amount) FROM View_VPTransaction AS VPT WHERE dbo.GetComponentID(VPT.AccountHeadID)=2 AND  VPT.ApprovedBy!=0 AND VPT.CurrencyID=@CurrencyID AND  VPT.IsDr=0 AND VPT.ProductID=TT.ProductID AND CONVERT(DATE, CONVERT(VARCHAR(12),VPT.TransactionDate,106)) BETWEEN  CONVERT(DATE, CONVERT(VARCHAR(12),@StartDate,106)) AND CONVERT(DATE, CONVERT(VARCHAR(12),@EndDate,106))),0)
				FROM #TempTable AS TT
			END
		END
		ELSE
		BEGIN
			IF(@CurrencyID=@BaseCurrencyID)
			BEGIN
				UPDATE #TempTable2
				SET DebitOpeiningQty=ISNULL((SELECT SUM(AOB.Qty) FROM AccountOpenningBreakdown AS AOB WHERE dbo.GetComponentID(AOB.AccountHeadID)=2 AND AOB.IsDr=1 AND AOB.BreakdownObjID=TT.ProductID AND AOB.AccountingSessionID=@SessionID AND AOB.BreakdownType=4),0),
					CreditOpeiningQty=ISNULL((SELECT SUM(AOB.Qty) FROM AccountOpenningBreakdown AS AOB WHERE dbo.GetComponentID(AOB.AccountHeadID)=2 AND AOB.IsDr=0 AND AOB.BreakdownObjID=TT.ProductID AND AOB.AccountingSessionID=@SessionID AND AOB.BreakdownType=4),0),
					DebitOpeiningValue=ISNULL((SELECT SUM(AOB.OpenningBalance) FROM AccountOpenningBreakdown AS AOB WHERE dbo.GetComponentID(AOB.AccountHeadID)=2 AND AOB.IsDr=1 AND AOB.BreakdownObjID=TT.ProductID AND AOB.AccountingSessionID=@SessionID AND AOB.BreakdownType=4),0),
					CreditOpeiningValue=ISNULL((SELECT SUM(AOB.OpenningBalance) FROM AccountOpenningBreakdown AS AOB WHERE dbo.GetComponentID(AOB.AccountHeadID)=2 AND AOB.IsDr=0 AND AOB.BreakdownObjID=TT.ProductID AND AOB.AccountingSessionID=@SessionID AND AOB.BreakdownType=4),0),
					DebitQty=ISNULL((SELECT SUM(VPT.Qty) FROM View_VPTransaction AS VPT WHERE dbo.GetComponentID(VPT.AccountHeadID)=2 AND  VPT.IsDr=1 AND VPT.ProductID=TT.ProductID AND CONVERT(DATE, CONVERT(VARCHAR(12),VPT.TransactionDate,106)) BETWEEN  CONVERT(DATE, CONVERT(VARCHAR(12),@SessionStartDate,106)) AND CONVERT(DATE, CONVERT(VARCHAR(12),DATEADD(DAY,-1,@StartDate),106))),0),
					DebitAmount=ISNULL((SELECT SUM(VPT.Amount*VPT.ConversionRate) FROM View_VPTransaction AS VPT WHERE dbo.GetComponentID(VPT.AccountHeadID)=2 AND  VPT.IsDr=1 AND VPT.ProductID=TT.ProductID AND CONVERT(DATE, CONVERT(VARCHAR(12),VPT.TransactionDate,106)) BETWEEN  CONVERT(DATE, CONVERT(VARCHAR(12),@SessionStartDate,106)) AND CONVERT(DATE, CONVERT(VARCHAR(12),DATEADD(DAY,-1,@StartDate),106))),0),
					CreditQty=ISNULL((SELECT SUM(VPT.Qty) FROM View_VPTransaction AS VPT WHERE dbo.GetComponentID(VPT.AccountHeadID)=2 AND  VPT.IsDr=0 AND VPT.ProductID=TT.ProductID AND CONVERT(DATE, CONVERT(VARCHAR(12),VPT.TransactionDate,106)) BETWEEN  CONVERT(DATE, CONVERT(VARCHAR(12),@SessionStartDate,106)) AND CONVERT(DATE, CONVERT(VARCHAR(12),DATEADD(DAY,-1,@StartDate),106))),0),
					CreditAmount=ISNULL((SELECT SUM(VPT.Amount*VPT.ConversionRate) FROM View_VPTransaction AS VPT WHERE dbo.GetComponentID(VPT.AccountHeadID)=2 AND  VPT.IsDr=0 AND VPT.ProductID=TT.ProductID AND CONVERT(DATE, CONVERT(VARCHAR(12),VPT.TransactionDate,106)) BETWEEN  CONVERT(DATE, CONVERT(VARCHAR(12),@SessionStartDate,106)) AND CONVERT(DATE, CONVERT(VARCHAR(12),DATEADD(DAY,-1,@StartDate),106))),0)
				FROM #TempTable2 AS TT
	
				UPDATE #TempTable
				SET DebitQty=ISNULL((SELECT SUM(VPT.Qty) FROM View_VPTransaction AS VPT WHERE dbo.GetComponentID(VPT.AccountHeadID)=2 AND  VPT.IsDr=1 AND VPT.ProductID=TT.ProductID AND CONVERT(DATE, CONVERT(VARCHAR(12),VPT.TransactionDate,106)) BETWEEN  CONVERT(DATE, CONVERT(VARCHAR(12),@StartDate,106)) AND CONVERT(DATE, CONVERT(VARCHAR(12),@EndDate,106))),0),
					DebitAmount=ISNULL((SELECT SUM(VPT.Amount*VPT.ConversionRate) FROM View_VPTransaction AS VPT WHERE dbo.GetComponentID(VPT.AccountHeadID)=2 AND  VPT.IsDr=1 AND VPT.ProductID=TT.ProductID AND CONVERT(DATE, CONVERT(VARCHAR(12),VPT.TransactionDate,106)) BETWEEN  CONVERT(DATE, CONVERT(VARCHAR(12),@StartDate,106)) AND CONVERT(DATE, CONVERT(VARCHAR(12),@EndDate,106))),0),
					CreditQty=ISNULL((SELECT SUM(VPT.Qty) FROM View_VPTransaction AS VPT WHERE dbo.GetComponentID(VPT.AccountHeadID)=2 AND  VPT.IsDr=0 AND VPT.ProductID=TT.ProductID AND CONVERT(DATE, CONVERT(VARCHAR(12),VPT.TransactionDate,106)) BETWEEN  CONVERT(DATE, CONVERT(VARCHAR(12),@StartDate,106)) AND CONVERT(DATE, CONVERT(VARCHAR(12),@EndDate,106))),0),
					CreditAmount=ISNULL((SELECT SUM(VPT.Amount*VPT.ConversionRate) FROM View_VPTransaction AS VPT WHERE dbo.GetComponentID(VPT.AccountHeadID)=2 AND  VPT.IsDr=0 AND VPT.ProductID=TT.ProductID AND CONVERT(DATE, CONVERT(VARCHAR(12),VPT.TransactionDate,106)) BETWEEN  CONVERT(DATE, CONVERT(VARCHAR(12),@StartDate,106)) AND CONVERT(DATE, CONVERT(VARCHAR(12),@EndDate,106))),0)
				FROM #TempTable AS TT

			END
			ELSE
			BEGIN
				UPDATE #TempTable2
				SET DebitOpeiningQty=ISNULL((SELECT SUM(AOB.Qty) FROM AccountOpenningBreakdown AS AOB WHERE dbo.GetComponentID(AOB.AccountHeadID)=2 AND AOB.IsDr=1 AND AOB.CurrencyID=@CurrencyID AND AOB.BreakdownObjID=TT.ProductID AND AOB.AccountingSessionID=@SessionID AND AOB.BreakdownType=4),0),
					CreditOpeiningQty=ISNULL((SELECT SUM(AOB.Qty) FROM AccountOpenningBreakdown AS AOB WHERE dbo.GetComponentID(AOB.AccountHeadID)=2 AND AOB.IsDr=0 AND AOB.CurrencyID=@CurrencyID AND AOB.BreakdownObjID=TT.ProductID AND AOB.AccountingSessionID=@SessionID AND AOB.BreakdownType=4),0),
					DebitOpeiningValue=ISNULL((SELECT SUM(AOB.AmountInCurrency) FROM AccountOpenningBreakdown AS AOB WHERE dbo.GetComponentID(AOB.AccountHeadID)=2 AND AOB.IsDr=1 AND AOB.CurrencyID=@CurrencyID AND AOB.BreakdownObjID=TT.ProductID AND AOB.AccountingSessionID=@SessionID AND AOB.BreakdownType=4),0),
					CreditOpeiningValue=ISNULL((SELECT SUM(AOB.AmountInCurrency) FROM AccountOpenningBreakdown AS AOB WHERE dbo.GetComponentID(AOB.AccountHeadID)=2 AND AOB.IsDr=0 AND AOB.CurrencyID=@CurrencyID AND AOB.BreakdownObjID=TT.ProductID AND AOB.AccountingSessionID=@SessionID AND AOB.BreakdownType=4),0),
					DebitQty=ISNULL((SELECT SUM(VPT.Qty) FROM View_VPTransaction AS VPT WHERE dbo.GetComponentID(VPT.AccountHeadID)=2 AND  VPT.CurrencyID=@CurrencyID AND  VPT.IsDr=1 AND VPT.ProductID=TT.ProductID AND CONVERT(DATE, CONVERT(VARCHAR(12),VPT.TransactionDate,106)) BETWEEN  CONVERT(DATE, CONVERT(VARCHAR(12),@SessionStartDate,106)) AND CONVERT(DATE, CONVERT(VARCHAR(12),DATEADD(DAY,-1,@StartDate),106))),0),
					DebitAmount=ISNULL((SELECT SUM(VPT.Amount) FROM View_VPTransaction AS VPT WHERE dbo.GetComponentID(VPT.AccountHeadID)=2 AND  VPT.CurrencyID=@CurrencyID AND  VPT.IsDr=1 AND VPT.ProductID=TT.ProductID AND CONVERT(DATE, CONVERT(VARCHAR(12),VPT.TransactionDate,106)) BETWEEN  CONVERT(DATE, CONVERT(VARCHAR(12),@SessionStartDate,106)) AND CONVERT(DATE, CONVERT(VARCHAR(12),DATEADD(DAY,-1,@StartDate),106))),0),
					CreditQty=ISNULL((SELECT SUM(VPT.Qty) FROM View_VPTransaction AS VPT WHERE dbo.GetComponentID(VPT.AccountHeadID)=2 AND  VPT.CurrencyID=@CurrencyID AND VPT.IsDr=0 AND VPT.ProductID=TT.ProductID AND CONVERT(DATE, CONVERT(VARCHAR(12),VPT.TransactionDate,106)) BETWEEN  CONVERT(DATE, CONVERT(VARCHAR(12),@SessionStartDate,106)) AND CONVERT(DATE, CONVERT(VARCHAR(12),DATEADD(DAY,-1,@StartDate),106))),0),
					CreditAmount=ISNULL((SELECT SUM(VPT.Amount) FROM View_VPTransaction AS VPT WHERE dbo.GetComponentID(VPT.AccountHeadID)=2 AND  VPT.CurrencyID=@CurrencyID AND VPT.IsDr=0 AND VPT.ProductID=TT.ProductID AND CONVERT(DATE, CONVERT(VARCHAR(12),VPT.TransactionDate,106)) BETWEEN  CONVERT(DATE, CONVERT(VARCHAR(12),@SessionStartDate,106)) AND CONVERT(DATE, CONVERT(VARCHAR(12),DATEADD(DAY,-1,@StartDate),106))),0)
				FROM #TempTable2 AS TT

				UPDATE #TempTable
				SET DebitQty=ISNULL((SELECT SUM(VPT.Qty) FROM View_VPTransaction AS VPT WHERE dbo.GetComponentID(VPT.AccountHeadID)=2 AND  VPT.CurrencyID=@CurrencyID AND  VPT.IsDr=1 AND VPT.ProductID=TT.ProductID AND CONVERT(DATE, CONVERT(VARCHAR(12),VPT.TransactionDate,106)) BETWEEN  CONVERT(DATE, CONVERT(VARCHAR(12),@StartDate,106)) AND CONVERT(DATE, CONVERT(VARCHAR(12),@EndDate,106))),0),
					DebitAmount=ISNULL((SELECT SUM(VPT.Amount) FROM View_VPTransaction AS VPT WHERE dbo.GetComponentID(VPT.AccountHeadID)=2 AND  VPT.CurrencyID=@CurrencyID AND  VPT.IsDr=1 AND VPT.ProductID=TT.ProductID AND CONVERT(DATE, CONVERT(VARCHAR(12),VPT.TransactionDate,106)) BETWEEN  CONVERT(DATE, CONVERT(VARCHAR(12),@StartDate,106)) AND CONVERT(DATE, CONVERT(VARCHAR(12),@EndDate,106))),0),
					CreditQty=ISNULL((SELECT SUM(VPT.Qty) FROM View_VPTransaction AS VPT WHERE dbo.GetComponentID(VPT.AccountHeadID)=2 AND  VPT.CurrencyID=@CurrencyID AND  VPT.IsDr=0 AND VPT.ProductID=TT.ProductID AND CONVERT(DATE, CONVERT(VARCHAR(12),VPT.TransactionDate,106)) BETWEEN  CONVERT(DATE, CONVERT(VARCHAR(12),@StartDate,106)) AND CONVERT(DATE, CONVERT(VARCHAR(12),@EndDate,106))),0),
					CreditAmount=ISNULL((SELECT SUM(VPT.Amount) FROM View_VPTransaction AS VPT WHERE dbo.GetComponentID(VPT.AccountHeadID)=2 AND  dbo.GetComponentID(VPT.AccountHeadID)=2 AND VPT.CurrencyID=@CurrencyID AND  VPT.IsDr=0 AND VPT.ProductID=TT.ProductID AND CONVERT(DATE, CONVERT(VARCHAR(12),VPT.TransactionDate,106)) BETWEEN  CONVERT(DATE, CONVERT(VARCHAR(12),@StartDate,106)) AND CONVERT(DATE, CONVERT(VARCHAR(12),@EndDate,106))),0)
				FROM #TempTable AS TT
			END
		END
	END
END


--IF(@ComponentID IN(2,6))
--BEGIN
UPDATE #TempTable2 SET IsDebit=1 FROM #TempTable2 AS TT
UPDATE #TempTable2 SET IsDebit=0 FROM #TempTable2 AS TT WHERE TT.DebitOpeiningValue<TT.CreditOpeiningValue
UPDATE #TempTable2 SET ClosingValue=(TT.DebitOpeiningValue-TT.CreditOpeiningValue+TT.DebitAmount-TT.CreditAmount), ClosingQty=(TT.DebitOpeiningQty-TT.CreditOpeiningQty+TT.DebitQty-TT.CreditQty), IsDrClosing=1 FROM #TempTable2 AS TT
UPDATE #TempTable2 SET IsDrClosing=0 FROM #TempTable2 AS TT WHERE TT.ClosingValue<0
--END
--ELSE
--BEGIN
--	UPDATE #TempTable2 SET OpeiningValue=(TT.OpeiningValue*(-1)) FROM #TempTable2 AS TT WHERE TT.IsDebit=1
--	UPDATE #TempTable2 SET ClosingValue=(TT.OpeiningValue-TT.DebitAmount+TT.CreditAmount), IsDrClosing=0 FROM #TempTable2 AS TT
--	UPDATE #TempTable2 SET IsDrClosing=1 FROM #TempTable2 AS TT WHERE TT.ClosingValue<0	
--END

IF(@AccountHeadID>0)
BEGIN
UPDATE #TempTable
SET	ParentHeadCode=ISNULL((SELECT COA.AccountCode FROM ChartsOfAccount AS COA WHERE COA.AccountHeadID=TT.ParentHeadID),''),
	ParentHeadName=ISNULL((SELECT COA.AccountHeadName FROM ChartsOfAccount AS COA WHERE COA.AccountHeadID=TT.ParentHeadID),'')
FROM #TempTable AS TT
END
UPDATE #TempTable
SET	ProductCode=(SELECT P.ProductCode FROM Product AS P WHERE P.ProductID=TT.ProductID),
	ProductName=(SELECT P.ProductName FROM Product AS P WHERE P.ProductID=TT.ProductID),
	CurrencySymbol = (SELECT CU.Symbol FROM Currency AS CU WHERE CU.CurrencyID=@CurrencyID),
	OpeiningValue=(SELECT ABC.ClosingValue FROM #TempTable2 AS ABC WHERE ABC.ProductID=TT.ProductID),
	OpeiningQty=(SELECT ABC.ClosingQty FROM #TempTable2 AS ABC WHERE ABC.ProductID=TT.ProductID),
	IsDebit=(SELECT ABC.IsDrClosing FROM #TempTable2 AS ABC WHERE ABC.ProductID=TT.ProductID)	
FROM #TempTable AS TT

--IF(@ComponentID IN(2,6))
--BEGIN
UPDATE #TempTable SET ClosingValue=(TT.OpeiningValue+TT.DebitAmount-TT.CreditAmount), ClosingQty=(TT.OpeiningQty+TT.DebitQty-TT.CreditQty), IsDrClosing=1 FROM #TempTable AS TT
UPDATE #TempTable SET IsDrClosing=0 FROM #TempTable AS TT WHERE TT.ClosingValue<0
--END
--ELSE
--BEGIN
--	UPDATE #TempTable SET ClosingValue=(TT.OpeiningValue-TT.DebitAmount+TT.CreditAmount), IsDrClosing=0 FROM #TempTable AS TT
--	UPDATE #TempTable SET IsDrClosing=1 FROM #TempTable AS TT WHERE TT.ClosingValue<0
--END



UPDATE #TempTable SET OpeiningValue=TT.OpeiningValue*(-1) FROM #TempTable AS TT WHERE TT.OpeiningValue<0
UPDATE #TempTable SET OpeiningQty=TT.OpeiningQty*(-1) FROM #TempTable AS TT WHERE TT.OpeiningQty<0
UPDATE #TempTable SET ClosingValue=TT.ClosingValue*(-1) FROM #TempTable AS TT WHERE TT.ClosingValue<0
UPDATE #TempTable SET ClosingQty=TT.ClosingQty*(-1) FROM #TempTable AS TT WHERE TT.ClosingQty<0

SELECT * FROM #TempTable

DROP TABLE #TempTable
DROP TABLE #TempTable2
COMMIT TRAN








GO
/****** Object:  StoredProcedure [dbo].[SP_YetToAccountHeadConfigure]    Script Date: 12/1/2016 2:17:46 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[SP_YetToAccountHeadConfigure]
(
	@StatementSetupID as int,
	@StartDate as date,
	@EndDate as date,
	@BUID as int
	--%n, %d, %d, %n
)
AS
BEGIN TRAN
--DECLARE
--@StatementSetupID as int,
--@StartDate as date,
--@EndDate as date,
--@BUID as int

--SET @StatementSetupID =1
--SET @StartDate='01 Jan 2015'
--SET @EndDate='19 Mar 2016'
--SET @BUID=0

CREATE TABLE #TR_Table (
							ID int IDENTITY(1,1) PRIMARY KEY,
							VoucherID int,
							VoucherDetailID int,
							AccountHeadID int,
							IsDr bit,
							Amount decimal(30,17)
							
						)

CREATE TABLE #FTR_Table (							
							VoucherID int,
							CashVoucherDetailID int,
							VoucherDetailID int,
							CashAccountHeadID int,
							AccountHeadID int,
							IsDr bit,
							Amount decimal(30,17)							
						)

IF(@BUID>0)
BEGIN
	INSERT INTO #TR_Table	(VoucherID,		VoucherDetailID,		AccountHeadID,		IsDr,		Amount)
					 SELECT VD.VoucherID,	VD.VoucherDetailID,		VD.AccountHeadID,	VD.IsDebit,	VD.Amount FROM View_VoucherDetail AS VD WHERE VD.BUID=@BUID AND ISNULL(VD.AuthorizedBy,0)!=0 AND CONVERT(DATE,CONVERT(VARCHAR(12),VD.VoucherDate,106)) BETWEEN CONVERT(DATE,CONVERT(VARCHAR(12),@StartDate,106)) AND CONVERT(DATE,CONVERT(VARCHAR(12),@EndDate,106)) AND VD.AccountHeadID IN (SELECT LBD.AccountHeadID FROM LedgerBreakDown AS LBD WHERE LBD.IsEffectedAccounts=1 AND LBD.ReferenceID=@StatementSetupID) ORDER BY VD.VoucherID
END
ELSE
BEGIN
	INSERT INTO #TR_Table	(VoucherID,		VoucherDetailID,		AccountHeadID,		IsDr,		Amount)
					 SELECT VD.VoucherID,	VD.VoucherDetailID,		VD.AccountHeadID,	VD.IsDebit,	VD.Amount FROM View_VoucherDetail AS VD WHERE ISNULL(VD.AuthorizedBy,0)!=0 AND CONVERT(DATE,CONVERT(VARCHAR(12),VD.VoucherDate,106)) BETWEEN CONVERT(DATE,CONVERT(VARCHAR(12),@StartDate,106)) AND CONVERT(DATE,CONVERT(VARCHAR(12),@EndDate,106)) AND VD.AccountHeadID IN (SELECT LBD.AccountHeadID FROM LedgerBreakDown AS LBD WHERE LBD.IsEffectedAccounts=1 AND LBD.ReferenceID=@StatementSetupID) ORDER BY VD.VoucherID
END

DECLARE
@Count as int,
@Index as int,
@VoucherID as int,
@VoucherDetailID as int,
@AccountHeadID as int,
@IsDr as bit,
@TempIsDr as bit,
@EffectedAmount as decimal(30,17),
@Amount as decimal(30,17),
@BalanceAmount as decimal(30,17),
@TempAmount as decimal(30,17),
@CashAccountHeadID as int,
@CashVoucherDetailID as int

SET @Index=1
SET @Count=(SELECT COUNT(*) FROM #TR_Table)

WHILE (@Index<=@Count)
BEGIN
	SET @VoucherID=(SELECT TT.VoucherID FROM #TR_Table AS TT WHERE TT.ID=@Index)
	SET @CashVoucherDetailID=(SELECT TT.VoucherDetailID FROM #TR_Table AS TT WHERE TT.ID=@Index)
	SET @CashAccountHeadID = (SELECT TT.AccountHeadID FROM #TR_Table AS TT WHERE TT.ID=@Index)	
	SET @IsDr=(SELECT TT.IsDr FROM #TR_Table AS TT WHERE TT.ID=@Index)
	IF NOT EXISTS(SELECT * FROM #FTR_Table AS TT WHERE TT.VoucherDetailID=@CashVoucherDetailID AND TT.IsDr!=@IsDr)
	BEGIN
		SET @BalanceAmount=0		
		SET @EffectedAmount=(SELECT TT.Amount FROM #TR_Table AS TT WHERE TT.VoucherDetailID=@CashVoucherDetailID)	
		SET @BalanceAmount=@EffectedAmount

		WHILE(@BalanceAmount>0)
		BEGIN		
			IF(@BUID>0)
			BEGIN
				SET @VoucherDetailID=(SELECT TOP 1 VD.VoucherDetailID FROM View_VoucherDetail AS VD WHERE VD.BUID=@BUID AND VD.IsDebit!=@IsDr AND VD.VoucherID=@VoucherID AND VD.VoucherDetailID NOT IN (SELECT TT.VoucherDetailID FROM #FTR_Table AS TT WHERE TT.VoucherID=@VoucherID AND TT.IsDr!=@IsDr))
			END
			ELSE
			BEGIN
				SET @VoucherDetailID=(SELECT TOP 1 VD.VoucherDetailID FROM View_VoucherDetail AS VD WHERE VD.IsDebit!=@IsDr AND VD.VoucherID=@VoucherID AND VD.VoucherDetailID NOT IN (SELECT TT.VoucherDetailID FROM #FTR_Table AS TT WHERE TT.VoucherID=@VoucherID AND TT.IsDr!=@IsDr))
			END
			SET @AccountHeadID=(SELECT VD.AccountHeadID FROM View_VoucherDetail AS VD WHERE VD.VoucherDetailID=@VoucherDetailID)
			SET @TempIsDr=(SELECT VD.IsDebit FROM View_VoucherDetail AS VD WHERE VD.VoucherDetailID=@VoucherDetailID)	
			SET @Amount =(SELECT VD.Amount FROM View_VoucherDetail AS VD WHERE VD.VoucherDetailID=@VoucherDetailID)
			IF(ROUND(@BalanceAmount,2)<=ROUND(@Amount,2))
			BEGIN
				SET @TempAmount=@BalanceAmount
				SET @BalanceAmount=0
			END
			ELSE
			BEGIN
				SET @TempAmount=@Amount
				SET @BalanceAmount=@BalanceAmount-@Amount
			END

			INSERT INTO #FTR_Table (VoucherID,	CashVoucherDetailID,	VoucherDetailID,	CashAccountHeadID,	AccountHeadID,		IsDr,		Amount)
							 VALUES(@VoucherID, @CashVoucherDetailID,	@VoucherDetailID,	@CashAccountHeadID,	@AccountHeadID,		@TempIsDr,	@TempAmount)
		END
	END
	SET @Index=@Index+1
END

CREATE TABLE #TempTable (
							AccountHeadID int,
							AccountHeadCode Varchar(512),
							AccountHeadName	Varchar(512),
							SubGroupName Varchar(512),
							IsDebit bit,
							OCSID int,
							LedgerGroupSetUpID int,
							LedgerGroupName Varchar(512)
						)


DECLARE
@LedgerGroupSetupID as int,
@TempDebit as bit

DECLARE Cur_AB1 CURSOR LOCAL FORWARD_ONLY KEYSET FOR
SELECT TT.LedgerGroupSetupID FROM LedgerGroupSetup AS TT WHERE TT.OCSID IN (SELECT MM.OperationCategorySetupID FROM OperationCategorySetup AS MM WHERE MM.StatementSetupID=@StatementSetupID)
OPEN Cur_AB1
FETCH NEXT FROM Cur_AB1 INTO  @LedgerGroupSetupID
WHILE(@@Fetch_Status <> -1)
BEGIN
	SET @IsDr = (SELECT TT.IsDr FROM LedgerGroupSetup AS TT WHERE TT.LedgerGroupSetupID=@LedgerGroupSetupID)
	IF(@IsDr=1)
	BEGIN
		SET @TempDebit= 0
	END
	ELSE
	BEGIN
		SET @TempDebit= 1
	END
	INSERT INTO #TempTable(	AccountHeadID,		LedgerGroupSetUpID,		IsDebit)
		SELECT DISTINCT		HH.AccountHeadID,	@LedgerGroupSetupID,	@TempDebit FROM #FTR_Table AS HH WHERE HH.IsDr!=@IsDr AND HH.AccountHeadID NOT IN (SELECT LBD.AccountHeadID FROM LedgerBreakDown AS LBD WHERE LBD.IsEffectedAccounts=0 AND LBD.ReferenceID=@LedgerGroupSetupID)
	
	FETCH NEXT FROM Cur_AB1 INTO  @LedgerGroupSetupID
END
CLOSE Cur_AB1
DEALLOCATE Cur_AB1

UPDATE #TempTable
SET AccountHeadCode = (SELECT COA.AccountCode FROM ChartsOfAccount AS COA WHERE COA.AccountHeadID=TT.AccountHeadID),
	AccountHeadName = (SELECT COA.AccountHeadName FROM ChartsOfAccount AS COA WHERE COA.AccountHeadID=TT.AccountHeadID),
	SubGroupName = (SELECT COA.AccountHeadName FROM ChartsOfAccount AS COA WHERE COA.AccountHeadID=(SELECT COA.ParentHeadID FROM ChartsOfAccount AS COA WHERE COA.AccountHeadID=TT.AccountHeadID)),
	OCSID = (SELECT HH.OCSID FROM LedgerGroupSetup AS HH WHERE HH.LedgerGroupSetupID=TT.LedgerGroupSetUpID),
	LedgerGroupName = (SELECT HH.LedgerGroupSetupName FROM LedgerGroupSetup AS HH WHERE HH.LedgerGroupSetupID=TT.LedgerGroupSetUpID)
FROM #TempTable AS TT

SELECT * FROM #TempTable AS TT ORDER BY TT.OCSID, TT.LedgerGroupSetUpID

DROP TABLE #TempTable
DROP TABLE #FTR_Table
DROP TABLE #TR_Table
COMMIT TRAN


GO
/****** Object:  UserDefinedFunction [dbo].[FN_GetIncomeStatementBalance]    Script Date: 12/1/2016 2:17:46 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE FUNCTION [dbo].[FN_GetIncomeStatementBalance]
(
	@StartDate as date,
	@EndDate as date

)
RETURNS Decimal(30,17)
AS
BEGIN
	--DECLARE 
	--@AccountingSessionID Int 
	--SET @AccountingSessionID=2
		
	DECLARE
	@IncomeStatementBalance as decimal(30,17),	
	@TempAccountHeadID as int,
	@ComponentType as smallint,
	@OpenningBalance as decimal(30,17),
	@DebitTransaction  as decimal(30,17),
	@CreditTransaction as decimal(30,17),
	@TotalIncome as decimal(30,17),
	@TotalExpenditure as decimal(30,17)	
	
	
	DECLARE @TempTable AS TABLE(	AccountHeadID int, 
									ComponentType smallint, 
									OpenningBalance decimal(30,17), 
									DebitTransaction decimal(30,17), 
									CreditTransaction decimal(30,17), 
									ClosingBalance decimal(30,17)
								)
	INSERT INTO @TempTable (AccountHeadID)
					SELECT	AccountHeadID FROM dbo.GetAccountHeadByComponents('5,6')--5=Income, 6=Expenditure
	
	UPDATE @TempTable
	SET	ComponentType=dbo.GetComponentID(TT.AccountHeadID),
		OpenningBalance=0,
		DebitTransaction = (SELECT ISNULL(SUM(VD.Amount),0) FROM VIEW_VoucherDetail  AS VD WHERE  VD.OperationType=1 AND VD.IsDebit=1 AND VD.AccountHeadID=TT.AccountHeadID AND VD.VoucherID IN(SELECT V.VoucherID FROM Voucher AS V WHERE V.AuthorizedBy!=0 AND CONVERT(DATE,CONVERT(VARCHAR(12),V.VoucherDate,106)) BETWEEN CONVERT(DATE,CONVERT(VARCHAR(12),@StartDate,106)) AND CONVERT(DATE,CONVERT(VARCHAR(12),@EndDate,106)))),
		CreditTransaction = (SELECT ISNULL(SUM(VD.Amount),0) FROM VIEW_VoucherDetail  AS VD WHERE VD.OperationType=1 AND VD.IsDebit=0 AND VD.AccountHeadID=TT.AccountHeadID AND VD.VoucherID IN(SELECT V.VoucherID FROM Voucher AS V WHERE V.AuthorizedBy!=0 AND CONVERT(DATE,CONVERT(VARCHAR(12),V.VoucherDate,106)) BETWEEN CONVERT(DATE,CONVERT(VARCHAR(12),@StartDate,106)) AND CONVERT(DATE,CONVERT(VARCHAR(12),@EndDate,106)))),
		ClosingBalance=0
	FROM @TempTable AS TT
	
	
	--Update Closing Balance
	UPDATE @TempTable
	set	ClosingBalance=TT.OpenningBalance+TT.DebitTransaction-TT.CreditTransaction
	FROM @TempTable AS TT where ComponentType IN(2,6)

	UPDATE @TempTable
	set	ClosingBalance=TT.OpenningBalance-TT.DebitTransaction+TT.CreditTransaction
	FROM @TempTable AS TT where ComponentType NOT IN(2,6)
		
	--Find Total Income & Expenditure
	SET @TotalIncome=(SELECT SUM(ClosingBalance) FROM @TempTable WHERE ComponentType=5)--ComponentID{Asset = 2,Laibility = 3,OwnerEquity=4,Income = 5,Expeness = 6}
	SET @TotalExpenditure=(SELECT SUM(ClosingBalance) FROM @TempTable WHERE ComponentType=6)--ComponentID{Asset = 2,Laibility = 3,OwnerEquity=4,Income = 5,Expeness = 6}
	
	SET @IncomeStatementBalance=@TotalIncome-@TotalExpenditure
	--SELECT @IncomeStatementBalance
  	RETURN(@IncomeStatementBalance)
END




GO
/****** Object:  UserDefinedFunction [dbo].[FN_GetIncomeStatementBalanceBU]    Script Date: 12/1/2016 2:17:46 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE FUNCTION [dbo].[FN_GetIncomeStatementBalanceBU]
(
	@StartDate as date,
	@EndDate as date,
	@StartBusinessUnitCode Varchar(512),
	@EndBusinessUnitCode Varchar(512)
)
RETURNS Decimal(30,17)
AS
BEGIN
	--DECLARE 
	--@AccountingSessionID Int 
	--SET @AccountingSessionID=2
		
	DECLARE
	@IncomeStatementBalance as decimal(30,17),	
	@TempAccountHeadID as int,
	@ComponentType as smallint,
	@OpenningBalance as decimal(30,17),
	@DebitTransaction  as decimal(30,17),
	@CreditTransaction as decimal(30,17),
	@TotalIncome as decimal(30,17),
	@TotalExpenditure as decimal(30,17)	


	--Reset BusinessUnit Start & End Code
	IF(@StartBusinessUnitCode!='00')
	BEGIN
		IF(@EndBusinessUnitCode='00')
		BEGIN
			SET @EndBusinessUnitCode=@StartBusinessUnitCode
		END
	END
	ELSE
	BEGIN
		SET @StartBusinessUnitCode=ISNULL((SELECT TT.Code FROM BusinessUnit AS TT WHERE TT.BusinessUnitID=(SELECT MIN(BU.BusinessUnitID) FROM BusinessUnit AS BU)),'00')
		SET @EndBusinessUnitCode=ISNULL((SELECT TT.Code FROM BusinessUnit AS TT WHERE TT.BusinessUnitID=(SELECT MAX(BU.BusinessUnitID) FROM BusinessUnit AS BU)),'00')	
	END
	
	
	DECLARE @TempTable AS TABLE(	AccountHeadID int, 
									ComponentType smallint, 
									OpenningBalance decimal(30,17), 
									DebitTransaction decimal(30,17), 
									CreditTransaction decimal(30,17), 
									ClosingBalance decimal(30,17)
								)
	INSERT INTO @TempTable (AccountHeadID)
					SELECT	AccountHeadID FROM dbo.GetAccountHeadByComponents('5,6')--5=Income, 6=Expenditure
	
	UPDATE @TempTable
	SET	ComponentType=dbo.GetComponentID(TT.AccountHeadID),
		OpenningBalance=0,
		DebitTransaction = (SELECT ISNULL(SUM(VD.Amount),0) FROM View_VoucherDetail  AS VD WHERE  VD.OperationType=1 AND VD.IsDebit=1 AND VD.AccountHeadID=TT.AccountHeadID AND VD.BUID IN (SELECT BU.BusinessUnitID FROM BusinessUnit AS BU WHERE CONVERT(INT,BU.Code) BETWEEN CONVERT(INT,@StartBusinessUnitCode) AND CONVERT(INT,@EndBusinessUnitCode)) AND VD.VoucherID IN(SELECT V.VoucherID FROM Voucher AS V WHERE V.AuthorizedBy!=0 AND CONVERT(DATE,CONVERT(VARCHAR(12),V.VoucherDate,106)) BETWEEN CONVERT(DATE,CONVERT(VARCHAR(12),@StartDate,106)) AND CONVERT(DATE,CONVERT(VARCHAR(12),@EndDate,106)))),
		CreditTransaction = (SELECT ISNULL(SUM(VD.Amount),0) FROM View_VoucherDetail  AS VD WHERE VD.OperationType=1 AND VD.IsDebit=0 AND VD.AccountHeadID=TT.AccountHeadID AND VD.BUID IN (SELECT BU.BusinessUnitID FROM BusinessUnit AS BU WHERE CONVERT(INT,BU.Code) BETWEEN CONVERT(INT,@StartBusinessUnitCode) AND CONVERT(INT,@EndBusinessUnitCode)) AND VD.VoucherID IN(SELECT V.VoucherID FROM Voucher AS V WHERE V.AuthorizedBy!=0 AND CONVERT(DATE,CONVERT(VARCHAR(12),V.VoucherDate,106)) BETWEEN CONVERT(DATE,CONVERT(VARCHAR(12),@StartDate,106)) AND CONVERT(DATE,CONVERT(VARCHAR(12),@EndDate,106)))),
		ClosingBalance=0
	FROM @TempTable AS TT
	
	
	--Update Closing Balance
	UPDATE @TempTable
	set	ClosingBalance=TT.OpenningBalance+TT.DebitTransaction-TT.CreditTransaction
	FROM @TempTable AS TT where ComponentType IN(2,6)

	UPDATE @TempTable
	set	ClosingBalance=TT.OpenningBalance-TT.DebitTransaction+TT.CreditTransaction
	FROM @TempTable AS TT where ComponentType NOT IN(2,6)
		
	--Find Total Income & Expenditure
	SET @TotalIncome=(SELECT SUM(ClosingBalance) FROM @TempTable WHERE ComponentType=5)--ComponentID{Asset = 2,Laibility = 3,OwnerEquity=4,Income = 5,Expeness = 6}
	SET @TotalExpenditure=(SELECT SUM(ClosingBalance) FROM @TempTable WHERE ComponentType=6)--ComponentID{Asset = 2,Laibility = 3,OwnerEquity=4,Income = 5,Expeness = 6}
	
	SET @IncomeStatementBalance=@TotalIncome-@TotalExpenditure
	--SELECT @IncomeStatementBalance
  	RETURN(@IncomeStatementBalance)
END




GO
/****** Object:  UserDefinedFunction [dbo].[FN_GetVoucherBillBalance]    Script Date: 12/1/2016 2:17:46 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE FUNCTION [dbo].[FN_GetVoucherBillBalance] 
(
	@VoucherBillID as int,
	@AccountHeadID as int,
	@IsBalance as bit	
)
RETURNS decimal(30,17)
AS
BEGIN
--DECLARE
--@VoucherBillID as int,
--@AccountHeadID as int,
--@IsBalance as bit

--SET @VoucherBillID=1113
--SET @AccountHeadID =383
--SET @IsBalance=1

DECLARE
@BillAmount as decimal(30,17),
@BillConsumptionAmount as decimal(30,17),
@RemainningBalance as decimal(30,17),
@RunningSessionID as int,
@ComponentID as int,
@IsBillDr as bit,
@IsDebit as bit,
@Result as decimal(30,17),
@SubLedgerID as int

SET @ComponentID=dbo.GetComponentID(@AccountHeadID)
SET @RunningSessionID =(SELECT TOP 1 ACS.AccountingSessionID FROM AccountingSession AS ACS WHERE  ACS.YearStatus=1 AND ACS.SessionType=1) 
IF(@ComponentID IN (2,6))
BEGIN
	SET @IsBillDr=1
END
ELSE
BEGIN
	SET @IsBillDr=0
END

SET @SubLedgerID = ISNULL((SELECT HH.SubLedgerID FROM VoucherBill AS HH WHERE HH.VoucherBillID=@VoucherBillID),0)
IF EXISTS(SELECT * FROM AccountOpenningBreakdown AS AOB WHERE  AOB.AccountingSessionID=@RunningSessionID AND AOB.AccountHeadID=@AccountHeadID AND AOB.CCID=@SubLedgerID AND AOB.BreakdownObjID=@VoucherBillID AND AOB.BreakdownType=2)
BEGIN		
	SET @BillAmount=(SELECT AOB.AmountInCurrency FROM AccountOpenningBreakdown AS AOB WHERE  AOB.AccountingSessionID=@RunningSessionID AND AOB.AccountHeadID=@AccountHeadID AND AOB.CCID=@SubLedgerID AND AOB.BreakdownObjID=@VoucherBillID AND AOB.BreakdownType=2)
	SET @IsDebit=(SELECT AOB.IsDr FROM AccountOpenningBreakdown AS AOB WHERE AOB.AccountingSessionID=@RunningSessionID AND AOB.AccountHeadID=@AccountHeadID AND AOB.CCID=@SubLedgerID AND AOB.BreakdownObjID=@VoucherBillID AND AOB.BreakdownType=2)
END
ELSE
BEGIN
	SET @BillAmount=(SELECT VB.CurrencyAmount FROM VoucherBill AS VB WHERE VB.AccountHeadID=@AccountHeadID AND VB.VoucherBillID=@VoucherBillID)
	IF EXISTS(SELECT * FROM VoucherBillTransaction AS VBT WHERE VBT.VoucherBillID=@VoucherBillID AND  VBT.TrType IN(1,3))
	BEGIN
		--SET @BillAmount=(SELECT TOP 1 (VBT.Amount) FROM VoucherBillTransaction AS VBT WHERE VBT.VoucherBillID=@VoucherBillID AND VBT.TrType IN(1,3))
		SET @IsDebit=(SELECT TOP 1 VBT.IsDr FROM VoucherBillTransaction AS VBT WHERE VBT.VoucherBillID=@VoucherBillID AND VBT.TrType IN(1,3))
	END
	ELSE
	BEGIN
		--SET @BillAmount=(SELECT VB.CurrencyAmount FROM VoucherBill AS VB WHERE VB.AccountHeadID=@AccountHeadID AND VB.VoucherBillID=@VoucherBillID)
		SET @IsDebit=@IsBillDr
	END
END



SET @BillConsumptionAmount=ISNULL((SELECT SUM(VBT.Amount) FROM VoucherBillTransaction AS VBT WHERE VBT.VoucherBillID=@VoucherBillID AND VBT.TrType IN(2,4)),0)
SET @RemainningBalance=ISNULL(@BillAmount-@BillConsumptionAmount,0)

--IF(@IsBalance=1)
--BEGIN
--	SELECT @RemainningBalance
--END
--ELSE
--BEGIN
--	SELECT CONVERT(Decimal(30,17), @IsDebit)
--END


IF(@IsBalance=1)
BEGIN
	SET @Result=@RemainningBalance
	
END
ELSE
BEGIN
	SET @Result= CONVERT(Decimal(30,17), @IsDebit)
END
RETURN @Result
--SELECT @Result
END





GO
/****** Object:  UserDefinedFunction [dbo].[FN_GetVoucherNoNumericPart]    Script Date: 12/1/2016 2:17:46 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE FUNCTION [dbo].[FN_GetVoucherNoNumericPart] (
    @VoucherID INT
)
RETURNS int
AS
BEGIN
	--DECLARE
	--@VoucherID as int
	--SET @VoucherID=140

	DECLARE
	@Length as int,
	@VoucherTypeID as int,
	@NumericStartIndex as int,
	@TempVoucherCodeType as smallint,
	@TempLength as int, 
	@TempSequence as int,
	@VoucherNo as Varchar(512),
	@NumberMethod as smallint,
	@VoucherNoNumericPart as int

	SET @VoucherNoNumericPart=0
	SET @NumericStartIndex=0
	SET @VoucherTypeID=(SELECT V.VoucherTypeID FROM Voucher AS V WHERE V.VoucherID=@VoucherID)
	SET @NumberMethod = (SELECT VT.NumberMethod FROM VoucherType AS VT WHERE VT.VoucherTypeID=@VoucherTypeID)
	SET @Length = (SELECT VC.[Length] FROM VoucherCode AS VC WHERE VC.VoucherTypeID=@VoucherTypeID AND VC.VoucherCodeType=3)
	IF(@NumberMethod=2)
	BEGIN
		DECLARE Cur_AB2 CURSOR LOCAL FORWARD_ONLY KEYSET FOR
		SELECT VoucherCodeType,[Length],Sequence FROM VoucherCode WHERE VoucherTypeID=@VoucherTypeID ORDER BY Sequence
		OPEN Cur_AB2
			FETCH NEXT FROM Cur_AB2 INTO  @TempVoucherCodeType,@TempLength, @TempSequence
			WHILE(@@Fetch_Status <> -1)
			BEGIN 
				IF(@TempVoucherCodeType=3)
				BEGIN
					BREAK
				END
				SET @NumericStartIndex=@NumericStartIndex+@TempLength
				FETCH NEXT FROM Cur_AB2 INTO  @TempVoucherCodeType,@TempLength, @TempSequence
			END
		CLOSE Cur_AB2
		DEALLOCATE Cur_AB2	
		SET @VoucherNo= (SELECT V.VoucherNo FROM Voucher AS V WHERE V.VoucherID=@VoucherID)
		SET @VoucherNoNumericPart=CONVERT(int,SUBSTRING(@VoucherNo, @NumericStartIndex+1,@Length))
	END
	--SELECT @VoucherNoNumericPart
	RETURN @VoucherNoNumericPart  
END


GO
/****** Object:  UserDefinedFunction [dbo].[FN_IsBalanceIncrease]    Script Date: 12/1/2016 2:17:46 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE FUNCTION [dbo].[FN_IsBalanceIncrease]
(
	@AccountHeadID as int,
	@IsDebit as bit
)
RETURNS bit
AS
--DECLARE 
--@ChartsofAccountType as smallint,
--@IsDebit as bit

--SET @ChartsofAccountType=4
--SET @IsDebit=1

BEGIN 
DECLARE 
@TempFlag as bit,
@ComponentID as int
 
SET @TempFlag =0

--Constants:
--===========
--Asset=2
--Liability=3
--Owners Equity=4
--Income=5
--Expenditure=6

SET @ComponentID=dbo.GetComponentID(@AccountHeadID)
IF (@ComponentID=2)--Asset=2,
BEGIN
	IF(@IsDebit=1)
	BEGIN
		SET @TempFlag=1
	END
	ELSE
	BEGIN
		SET @TempFlag=0
	END				
END-- end of Asset
ELSE IF (@ComponentID=3)--Laibility=3
BEGIN
	IF(@IsDebit=1)
	BEGIN
		SET @TempFlag=0
	END
	ELSE
	BEGIN
		SET @TempFlag=1
	END				
END--end Laibility
ELSE IF (@ComponentID=4)--OwnerEquity=4
BEGIN
	IF(@IsDebit=1)
	BEGIN
		SET @TempFlag=0
	END
	ELSE
	BEGIN
		SET @TempFlag=1
	END				
END--end OwnerEquity
ELSE IF (@ComponentID=5)--Income=5
BEGIN
	IF(@IsDebit=1)
	BEGIN
		SET @TempFlag=0
	END
	ELSE
	BEGIN
		SET @TempFlag=1
	END				
END--end Income
ELSE IF (@ComponentID=6)--Expeness=6
BEGIN
	IF(@IsDebit=1)
	BEGIN
		SET @TempFlag=1
	END
	ELSE
	BEGIN
		SET @TempFlag=0
	END				
END--end Expeness
RETURN @TempFlag 
--SELECT @TempFlag 
END



GO
/****** Object:  UserDefinedFunction [dbo].[GetAccountHead]    Script Date: 12/1/2016 2:17:46 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
--Updated By fahim0abir
--date: 09 nov 2015
CREATE FUNCTION [dbo].[GetAccountHead](@AccountHeadID int)       
RETURNS @TableAccountHead TABLE(	AccountHeadID int,
									AccountCode Varchar(512),
									AccountHeadName Varchar(512),
									AccountType SMALLINT,
									ParentHeadID int,
									OpeningBalance decimal(30,17)																
								)
AS
BEGIN
	WITH temptable (AccountHeadID,AccountCode,AccountHeadName,AccountType,ParentHeadID, OpeningBalance, MinChildCode) AS
	(
	  SELECT AccountHeadID,AccountCode,AccountHeadName,AccountType,ParentHeadID, OpeningBalance, MinChildCode
	  FROM View_ChartsOfAccount TempTable1
	  WHERE TempTable1.AccountHeadID = 1
	  UNION ALL
	  SELECT TempTable2.AccountHeadID, TempTable2.AccountCode, TempTable2.AccountHeadName, TempTable2.AccountType, TempTable2.ParentHeadID, TempTable2.OpeningBalance, TempTable2.MinChildCode 
	  FROM View_ChartsOfAccount TempTable2, temptable TempTable3
	  WHERE TempTable2.ParentHeadID = TempTable3.AccountHeadID
	)
	INSERT @TableAccountHead
	SELECT AccountHeadID,AccountCode,AccountHeadName,AccountType,ParentHeadID, OpeningBalance FROM temptable WHERE ParentHeadID <> 0 ORDER BY MinChildCode,AccountHeadID --AccountCode
    RETURN
END;





GO
/****** Object:  UserDefinedFunction [dbo].[GetAccountHeadByComponents]    Script Date: 12/1/2016 2:17:46 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE FUNCTION [dbo].[GetAccountHeadByComponents](@AccountHeadIDs Varchar(256))       
RETURNS @TableAccountHead TABLE(	AccountHeadID int,
									AccountCode Varchar(512),
									AccountHeadName Varchar(512),
									AccountType SMALLINT,
									ComponentID int,
									ParentHeadID int,
									OpeningBalance decimal(30,17)																
								)
AS
BEGIN
	WITH temptable (AccountHeadID,AccountCode,AccountHeadName,AccountType,ComponentID,ParentHeadID, OpeningBalance) AS
	(
	  SELECT AccountHeadID,AccountCode,AccountHeadName,AccountType,dbo.GetComponentID(AccountHeadID),ParentHeadID, OpeningBalance
	  FROM ChartsOfAccount TempTable1 
	  WHERE TempTable1.AccountHeadID IN(SELECT * FROM dbo.SplitInToDataSet(@AccountHeadIDs,','))
	  UNION ALL
	  SELECT TempTable2.AccountHeadID, TempTable2.AccountCode, TempTable2.AccountHeadName, TempTable2.AccountType,dbo.GetComponentID(TempTable2.AccountHeadID), TempTable2.ParentHeadID, TempTable2.OpeningBalance 
	  FROM ChartsOfAccount TempTable2, temptable TempTable3
	  WHERE TempTable2.ParentHeadID = TempTable3.AccountHeadID
	)
	INSERT @TableAccountHead
	SELECT AccountHeadID,AccountCode,AccountHeadName,AccountType,ComponentID,ParentHeadID, OpeningBalance FROM temptable WHERE ParentHeadID <> 0 ORDER BY AccountCode
    RETURN
END;



GO
/****** Object:  UserDefinedFunction [dbo].[GetInventoryEffectedVoucher]    Script Date: 12/1/2016 2:17:46 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE FUNCTION [dbo].[GetInventoryEffectedVoucher]()
RETURNS @TableVoucher TABLE (VoucherID int)
AS
BEGIN	
	INSERT @TableVoucher
	SELECT DISTINCT TT.VoucherID FROM View_VPTransaction AS TT WHERE TT.VoucherDetailID IN (SELECT HH.VoucherDetailID FROM VoucherDetail AS HH WHERE HH.IsDebit=0 AND HH.VoucherID IN (SELECT HH.VoucherID FROM View_Voucher AS HH WHERE HH.VoucherCategory=14))
    RETURN
END;





GO
/****** Object:  UserDefinedFunction [dbo].[GetLedgerOpeningBalance]    Script Date: 12/1/2016 2:17:46 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE FUNCTION [dbo].[GetLedgerOpeningBalance]
(
	@AccountHeadID int,
	@StartDate date,
	@CurrencyID int,
	@IsApproved bit,
	@IsTransactioncount bit,
	@BusinessUnitID as int
)
RETURNS @Temp_Table TABLE(								
							IsDebit bit,
							DebitAmount decimal(30,17),
							CreditAmount decimal(30,17),						
							OpenningBalance decimal(30,17)
						)

AS
BEGIN
DECLARE 
@IsBaseCurrency as bit,
@OpenningBalance as decimal(30,17),
@ComponentType as smallint,
@DebitOpenningBalance as decimal(30,17),
@CreditOpenningBalance as decimal(30,17),
@DebitTransactionAmount as decimal(30,17),
@CreditTransactionAmount as decimal(30,17),
@SessionStartDate as date,
@SessionID as int

IF(@CurrencyID<=0)
BEGIN
	SET @CurrencyID=(SELECT C.BaseCurrencyID FROM Company AS C WHERE C.CompanyID=1)
END
SET @IsBaseCurrency=1
IF((SELECT C.BaseCurrencyID FROM Company AS C WHERE C.CompanyID=1)!=@CurrencyID)
BEGIN
	SET @IsBaseCurrency=0
END

IF EXISTS(SELECT SessionID FROM AccountingSession WHERE CONVERT(DATE,CONVERT(VARCHAR(12),StartDate,106))=CONVERT(DATE,CONVERT(VARCHAR(12),@StartDate,106)) AND SessionType=6)
BEGIN
	SET @SessionID=(SELECT Top(1)SessionID FROM AccountingSession  WHERE  CONVERT(DATE,CONVERT(VARCHAR(12),StartDate,106))=CONVERT(DATE,CONVERT(VARCHAR(12),@StartDate,106)) AND SessionType=6)
END
ELSE
BEGIN
	SET @SessionID=(SELECT TOP 1 SessionID FROM AccountingSession  WHERE  CONVERT(DATE,CONVERT(VARCHAR(12),StartDate,106))>=CONVERT(DATE,CONVERT(VARCHAR(12),@StartDate,106)) AND SessionType=6)
	SET @StartDate=(SELECT MIN(StartDate) FROM AccountingSession  WHERE  CONVERT(DATE,CONVERT(VARCHAR(12),StartDate,106))>=CONVERT(DATE,CONVERT(VARCHAR(12),@StartDate,106)) AND SessionType=6)
END

SET @SessionStartDate=(SELECT StartDate FROM AccountingSession WHERE  AccountingSessionID=@SessionID)
SET @ComponentType=  dbo.GetComponentID(@AccountHeadID) --Set CompanyID

SET @DebitOpenningBalance=0
SET @CreditOpenningBalance=0
SET @DebitTransactionAmount=0
SET @CreditTransactionAmount=0

IF(@BusinessUnitID>0)
BEGIN
	IF(@IsApproved=1)
	BEGIN
		IF(@IsBaseCurrency=1)
		BEGIN
			SET @DebitOpenningBalance=ISNULL((SELECT ISNULL(SUM(AO.OpenningBalance),0) FROM AccountOpenning AS AO WHERE AO.IsDebit=1 AND AO.BusinessUnitID=@BusinessUnitID AND  AO.AccountingSessionID=@SessionID AND AO.AccountHeadID=@AccountHeadID),0)
			SET @CreditOpenningBalance=ISNULL((SELECT ISNULL(SUM(AO.OpenningBalance),0) FROM AccountOpenning AS AO WHERE AO.IsDebit=0 AND AO.BusinessUnitID=@BusinessUnitID AND  AO.AccountingSessionID=@SessionID AND AO.AccountHeadID=@AccountHeadID),0)
			IF(@IsTransactioncount=1)
			BEGIN
				SET @DebitTransactionAmount=(SELECT ISNULL(SUM(VD.Amount),0) FROM View_VoucherDetail AS VD WHERE  VD.BUID=@BusinessUnitID AND VD.IsDebit=1 AND VD.AccountHeadID=@AccountHeadID AND ISNULL(VD.AuthorizedBy,0)!=0 AND  CONVERT(DATE,CONVERT(VARCHAR(12),VD.VoucherDate,106)) BETWEEN CONVERT(DATE,CONVERT(VARCHAR(12),@SessionStartDate,106)) AND CONVERT(DATE,CONVERT(VARCHAR(12),DATEADD(DAY,-1,@StartDate),106)))
				SET @CreditTransactionAmount=(SELECT ISNULL(SUM(VD.Amount),0) FROM View_VoucherDetail AS VD WHERE  VD.BUID=@BusinessUnitID AND VD.IsDebit=0 AND VD.AccountHeadID=@AccountHeadID AND ISNULL(VD.AuthorizedBy,0)!=0 AND  CONVERT(DATE,CONVERT(VARCHAR(12),VD.VoucherDate,106)) BETWEEN CONVERT(DATE,CONVERT(VARCHAR(12),@SessionStartDate,106)) AND CONVERT(DATE,CONVERT(VARCHAR(12),DATEADD(DAY,-1,@StartDate),106)))
			END
		END
		ELSE
		BEGIN
			SET @DebitOpenningBalance=ISNULL((SELECT ISNULL(SUM(AmountInCurrency),0) FROM AccountOpenning AS AO WHERE AO.IsDebit=1 AND AO.BusinessUnitID=@BusinessUnitID AND AO.AccountingSessionID=@SessionID AND AO.AccountHeadID=@AccountHeadID AND AO.CurrencyID=@CurrencyID),0)
			SET @CreditOpenningBalance=ISNULL((SELECT ISNULL(SUM(AmountInCurrency),0) FROM AccountOpenning AS AO WHERE AO.IsDebit=0 AND AO.BusinessUnitID=@BusinessUnitID AND AO.AccountingSessionID=@SessionID AND AO.AccountHeadID=@AccountHeadID AND AO.CurrencyID=@CurrencyID),0)
			IF(@IsTransactioncount=1)
			BEGIN
				SET @DebitTransactionAmount=(SELECT ISNULL(SUM(VD.AmountInCurrency),0) FROM View_VoucherDetail AS VD WHERE  VD.BUID=@BusinessUnitID AND VD.CurrencyID=@CurrencyID AND VD.IsDebit=1 AND VD.AccountHeadID=@AccountHeadID AND ISNULL(VD.AuthorizedBy,0)!=0 AND  CONVERT(DATE,CONVERT(VARCHAR(12),VD.VoucherDate,106)) BETWEEN CONVERT(DATE,CONVERT(VARCHAR(12),@SessionStartDate,106)) AND CONVERT(DATE,CONVERT(VARCHAR(12),DATEADD(DAY,-1,@StartDate),106)))
				SET @CreditTransactionAmount=(SELECT ISNULL(SUM(VD.AmountInCurrency),0) FROM View_VoucherDetail AS VD WHERE  VD.BUID=@BusinessUnitID AND VD.CurrencyID=@CurrencyID AND VD.IsDebit=0 AND VD.AccountHeadID=@AccountHeadID AND ISNULL(VD.AuthorizedBy,0)!=0 AND  CONVERT(DATE,CONVERT(VARCHAR(12),VD.VoucherDate,106)) BETWEEN CONVERT(DATE,CONVERT(VARCHAR(12),@SessionStartDate,106)) AND CONVERT(DATE,CONVERT(VARCHAR(12),DATEADD(DAY,-1,@StartDate),106)))
			END
		END
	END
	ELSE
	BEGIN
		IF(@IsBaseCurrency=1)
		BEGIN
			SET @DebitOpenningBalance=ISNULL((SELECT ISNULL(Sum(OpenningBalance),0) FROM AccountOpenning AS AO WHERE AO.IsDebit=1 AND AO.BusinessUnitID=@BusinessUnitID AND  AO.AccountingSessionID=@SessionID AND AO.AccountHeadID=@AccountHeadID),0)
			SET @CreditOpenningBalance=ISNULL((SELECT ISNULL(Sum(OpenningBalance),0) FROM AccountOpenning AS AO WHERE AO.IsDebit=0 AND AO.BusinessUnitID=@BusinessUnitID AND  AO.AccountingSessionID=@SessionID AND AO.AccountHeadID=@AccountHeadID),0)
			IF(@IsTransactioncount=1)
			BEGIN
				SET @DebitTransactionAmount=(SELECT ISNULL(SUM(VD.Amount),0) FROM View_VoucherDetail AS VD WHERE  VD.BUID=@BusinessUnitID AND VD.IsDebit=1 AND VD.AccountHeadID=@AccountHeadID AND CONVERT(DATE,CONVERT(VARCHAR(12),VD.VoucherDate,106)) BETWEEN CONVERT(DATE,CONVERT(VARCHAR(12),@SessionStartDate,106)) AND CONVERT(DATE,CONVERT(VARCHAR(12),DATEADD(DAY,-1,@StartDate),106)))
				SET @CreditTransactionAmount=(SELECT ISNULL(SUM(VD.Amount),0) FROM View_VoucherDetail AS VD WHERE  VD.BUID=@BusinessUnitID AND VD.IsDebit=0 AND VD.AccountHeadID=@AccountHeadID AND CONVERT(DATE,CONVERT(VARCHAR(12),VD.VoucherDate,106)) BETWEEN CONVERT(DATE,CONVERT(VARCHAR(12),@SessionStartDate,106)) AND CONVERT(DATE,CONVERT(VARCHAR(12),DATEADD(DAY,-1,@StartDate),106)))
			END
		END
		ELSE
		BEGIN
			SET @DebitOpenningBalance=ISNULL((SELECT ISNULL(SUM(AmountInCurrency),0) FROM AccountOpenning AS AO WHERE AO.IsDebit=1 AND AO.BusinessUnitID=@BusinessUnitID AND AO.AccountingSessionID=@SessionID AND AO.AccountHeadID=@AccountHeadID AND AO.CurrencyID=@CurrencyID),0)
			SET @CreditOpenningBalance=ISNULL((SELECT ISNULL(SUM(AmountInCurrency),0) FROM AccountOpenning AS AO WHERE AO.IsDebit=0 AND AO.BusinessUnitID=@BusinessUnitID AND AO.AccountingSessionID=@SessionID AND AO.AccountHeadID=@AccountHeadID AND AO.CurrencyID=@CurrencyID),0)
			IF(@IsTransactioncount=1)
			BEGIN
				SET @DebitTransactionAmount=(SELECT ISNULL(SUM(VD.AmountInCurrency),0) FROM View_VoucherDetail AS VD WHERE VD.BUID=@BusinessUnitID AND VD.CurrencyID=@CurrencyID AND VD.IsDebit=1 AND VD.AccountHeadID=@AccountHeadID AND CONVERT(DATE,CONVERT(VARCHAR(12),VD.VoucherDate,106)) BETWEEN CONVERT(DATE,CONVERT(VARCHAR(12),@SessionStartDate,106)) AND CONVERT(DATE,CONVERT(VARCHAR(12),DATEADD(DAY,-1,@StartDate),106)))
				SET @CreditTransactionAmount=(SELECT ISNULL(SUM(VD.AmountInCurrency),0) FROM View_VoucherDetail AS VD WHERE VD.BUID=@BusinessUnitID AND  VD.CurrencyID=@CurrencyID AND VD.IsDebit=0 AND VD.AccountHeadID=@AccountHeadID AND CONVERT(DATE,CONVERT(VARCHAR(12),VD.VoucherDate,106)) BETWEEN CONVERT(DATE,CONVERT(VARCHAR(12),@SessionStartDate,106)) AND CONVERT(DATE,CONVERT(VARCHAR(12),DATEADD(DAY,-1,@StartDate),106)))
			END
		END
	END
END
ELSE
BEGIN
	IF(@IsApproved=1)
	BEGIN
		IF(@IsBaseCurrency=1)
		BEGIN
			SET @DebitOpenningBalance=ISNULL((SELECT ISNULL(SUM(AO.OpenningBalance),0) FROM AccountOpenning AS AO WHERE AO.IsDebit=1 AND  AO.AccountingSessionID=@SessionID AND AO.AccountHeadID=@AccountHeadID),0)
			SET @CreditOpenningBalance=ISNULL((SELECT ISNULL(SUM(AO.OpenningBalance),0) FROM AccountOpenning AS AO WHERE AO.IsDebit=0 AND  AO.AccountingSessionID=@SessionID AND AO.AccountHeadID=@AccountHeadID),0)
			IF(@IsTransactioncount=1)
			BEGIN
				SET @DebitTransactionAmount=(SELECT ISNULL(SUM(VD.Amount),0) FROM View_VoucherDetail AS VD WHERE  VD.IsDebit=1 AND VD.AccountHeadID=@AccountHeadID AND ISNULL(VD.AuthorizedBy,0)!=0 AND CONVERT(DATE,CONVERT(VARCHAR(12),VD.VoucherDate,106)) BETWEEN CONVERT(DATE,CONVERT(VARCHAR(12),@SessionStartDate,106)) AND CONVERT(DATE,CONVERT(VARCHAR(12),DATEADD(DAY,-1,@StartDate),106)))
				SET @CreditTransactionAmount=(SELECT ISNULL(SUM(VD.Amount),0) FROM View_VoucherDetail AS VD WHERE  VD.IsDebit=0 AND VD.AccountHeadID=@AccountHeadID AND ISNULL(VD.AuthorizedBy,0)!=0 AND CONVERT(DATE,CONVERT(VARCHAR(12),VD.VoucherDate,106)) BETWEEN CONVERT(DATE,CONVERT(VARCHAR(12),@SessionStartDate,106)) AND CONVERT(DATE,CONVERT(VARCHAR(12),DATEADD(DAY,-1,@StartDate),106)))
			END
		END
		ELSE
		BEGIN
			SET @DebitOpenningBalance=ISNULL((SELECT ISNULL(SUM(AmountInCurrency),0) FROM AccountOpenning AS AO WHERE AO.IsDebit=1 AND AO.AccountingSessionID=@SessionID AND AO.AccountHeadID=@AccountHeadID AND AO.CurrencyID=@CurrencyID),0)
			SET @CreditOpenningBalance=ISNULL((SELECT ISNULL(SUM(AmountInCurrency),0) FROM AccountOpenning AS AO WHERE AO.IsDebit=0 AND AO.AccountingSessionID=@SessionID AND AO.AccountHeadID=@AccountHeadID AND AO.CurrencyID=@CurrencyID),0)
			IF(@IsTransactioncount=1)
			BEGIN
				SET @DebitTransactionAmount=(SELECT ISNULL(SUM(VD.AmountInCurrency),0) FROM View_VoucherDetail AS VD WHERE  VD.CurrencyID=@CurrencyID AND VD.IsDebit=1 AND VD.AccountHeadID=@AccountHeadID AND ISNULL(VD.AuthorizedBy,0)!=0 AND CONVERT(DATE,CONVERT(VARCHAR(12),VD.VoucherDate,106)) BETWEEN CONVERT(DATE,CONVERT(VARCHAR(12),@SessionStartDate,106)) AND CONVERT(DATE,CONVERT(VARCHAR(12),DATEADD(DAY,-1,@StartDate),106)))
				SET @CreditTransactionAmount=(SELECT ISNULL(SUM(VD.AmountInCurrency),0) FROM View_VoucherDetail AS VD WHERE  VD.CurrencyID=@CurrencyID AND VD.IsDebit=0 AND VD.AccountHeadID=@AccountHeadID AND ISNULL(VD.AuthorizedBy,0)!=0 AND CONVERT(DATE,CONVERT(VARCHAR(12),VD.VoucherDate,106)) BETWEEN CONVERT(DATE,CONVERT(VARCHAR(12),@SessionStartDate,106)) AND CONVERT(DATE,CONVERT(VARCHAR(12),DATEADD(DAY,-1,@StartDate),106)))
			END
		END
	END
	ELSE
	BEGIN
		IF(@IsBaseCurrency=1)
		BEGIN
			SET @DebitOpenningBalance=ISNULL((SELECT ISNULL(Sum(OpenningBalance),0) FROM AccountOpenning AS AO WHERE AO.IsDebit=1 AND  AO.AccountingSessionID=@SessionID AND AO.AccountHeadID=@AccountHeadID),0)
			SET @CreditOpenningBalance=ISNULL((SELECT ISNULL(Sum(OpenningBalance),0) FROM AccountOpenning AS AO WHERE AO.IsDebit=0 AND  AO.AccountingSessionID=@SessionID AND AO.AccountHeadID=@AccountHeadID),0)
			IF(@IsTransactioncount=1)
			BEGIN
				SET @DebitTransactionAmount=(SELECT ISNULL(SUM(VD.Amount),0) FROM View_VoucherDetail AS VD WHERE  VD.IsDebit=1 AND VD.AccountHeadID=@AccountHeadID AND CONVERT(DATE,CONVERT(VARCHAR(12),VD.VoucherDate,106)) BETWEEN CONVERT(DATE,CONVERT(VARCHAR(12),@SessionStartDate,106)) AND CONVERT(DATE,CONVERT(VARCHAR(12),DATEADD(DAY,-1,@StartDate),106)))
				SET @CreditTransactionAmount=(SELECT ISNULL(SUM(VD.Amount),0) FROM View_VoucherDetail AS VD WHERE  VD.IsDebit=0 AND VD.AccountHeadID=@AccountHeadID AND CONVERT(DATE,CONVERT(VARCHAR(12),VD.VoucherDate,106)) BETWEEN CONVERT(DATE,CONVERT(VARCHAR(12),@SessionStartDate,106)) AND CONVERT(DATE,CONVERT(VARCHAR(12),DATEADD(DAY,-1,@StartDate),106)))
			END
		END
		ELSE
		BEGIN
			SET @DebitOpenningBalance=ISNULL((SELECT ISNULL(SUM(AmountInCurrency),0) FROM AccountOpenning AS AO WHERE AO.IsDebit=1 AND AO.AccountingSessionID=@SessionID AND AO.AccountHeadID=@AccountHeadID AND AO.CurrencyID=@CurrencyID),0)
			SET @CreditOpenningBalance=ISNULL((SELECT ISNULL(SUM(AmountInCurrency),0) FROM AccountOpenning AS AO WHERE AO.IsDebit=0 AND AO.AccountingSessionID=@SessionID AND AO.AccountHeadID=@AccountHeadID AND AO.CurrencyID=@CurrencyID),0)
			IF(@IsTransactioncount=1)
			BEGIN
				SET @DebitTransactionAmount=(SELECT ISNULL(SUM(VD.AmountInCurrency),0) FROM View_VoucherDetail AS VD WHERE  VD.CurrencyID=@CurrencyID AND VD.IsDebit=1 AND VD.AccountHeadID=@AccountHeadID AND CONVERT(DATE,CONVERT(VARCHAR(12),VD.VoucherDate,106)) BETWEEN CONVERT(DATE,CONVERT(VARCHAR(12),@SessionStartDate,106)) AND CONVERT(DATE,CONVERT(VARCHAR(12),DATEADD(DAY,-1,@StartDate),106)))
				SET @CreditTransactionAmount=(SELECT ISNULL(SUM(VD.AmountInCurrency),0) FROM View_VoucherDetail AS VD WHERE  VD.CurrencyID=@CurrencyID AND VD.IsDebit=0 AND VD.AccountHeadID=@AccountHeadID AND CONVERT(DATE,CONVERT(VARCHAR(12),VD.VoucherDate,106)) BETWEEN CONVERT(DATE,CONVERT(VARCHAR(12),@SessionStartDate,106)) AND CONVERT(DATE,CONVERT(VARCHAR(12),DATEADD(DAY,-1,@StartDate),106)))
			END
		END
	END
END

IF(@ComponentType IN(2,6))--ComponentID{Asset = 2,Laibility = 3,OwnerEquity=4,Income = 5,Expeness = 6}
BEGIN
	SET @OpenningBalance =@DebitOpenningBalance-@CreditOpenningBalance+@DebitTransactionAmount-@CreditTransactionAmount
	IF(@OpenningBalance>=0)
	BEGIN	
		INSERT INTO @Temp_Table	(IsDebit,	DebitAmount,		CreditAmount,	OpenningBalance)	
						VALUES	(1,			@OpenningBalance,	0.00,			@OpenningBalance)
	END
	ELSE
	BEGIN	
		INSERT INTO @Temp_Table	(IsDebit,	DebitAmount,	CreditAmount,				OpenningBalance)	
						VALUES	(0,			0.00,			((-1)*@OpenningBalance),	@OpenningBalance)
	END
END
ELSE
BEGIN
	SET @OpenningBalance =@CreditOpenningBalance-@DebitOpenningBalance-@DebitTransactionAmount+@CreditTransactionAmount	
	IF(@OpenningBalance>=0)
	BEGIN	
		INSERT INTO @Temp_Table	(IsDebit,	DebitAmount,		CreditAmount,		OpenningBalance)	
						VALUES	(0,			0.00,				@OpenningBalance,	@OpenningBalance)
	END
	ELSE
	BEGIN	
		INSERT INTO @Temp_Table	(IsDebit,	DebitAmount,				CreditAmount,	OpenningBalance)	
						VALUES	(1,			((-1)*@OpenningBalance),	0.00,			@OpenningBalance)
	END
END	
RETURN
END;






GO
/****** Object:  UserDefinedFunction [dbo].[GetLedgerOpeningBalanceByBU]    Script Date: 12/1/2016 2:17:46 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE FUNCTION [dbo].[GetLedgerOpeningBalanceByBU]
(
	@AccountHeadID int,
	@StartDate date,
	@CurrencyID int,
	@IsApproved bit,
	@IsTransactioncount bit,
	@StartBusinessUnitCode Varchar(512),
	@EndBusinessUnitCode Varchar(512)
)
RETURNS @Temp_Table TABLE(								
							IsDebit bit,
							DebitAmount decimal(30,17),
							CreditAmount decimal(30,17),						
							OpenningBalance decimal(30,17)
						)

AS
BEGIN
DECLARE 
@IsBaseCurrency as bit,
@OpenningBalance as decimal(30,17),
@ComponentType as smallint,
@DebitOpenningBalance as decimal(30,17),
@CreditOpenningBalance as decimal(30,17),
@DebitTransactionAmount as decimal(30,17),
@CreditTransactionAmount as decimal(30,17),
@SessionStartDate as date,
@SessionID as int

IF(@CurrencyID<=0)
BEGIN
	SET @CurrencyID=(SELECT C.BaseCurrencyID FROM Company AS C WHERE C.CompanyID=1)
END
SET @IsBaseCurrency=1
IF((SELECT C.BaseCurrencyID FROM Company AS C WHERE C.CompanyID=1)!=@CurrencyID)
BEGIN
	SET @IsBaseCurrency=0
END

IF EXISTS(SELECT SessionID FROM AccountingSession WHERE CONVERT(DATE,CONVERT(VARCHAR(12),StartDate,106))=CONVERT(DATE,CONVERT(VARCHAR(12),@StartDate,106)) AND SessionType=6)
BEGIN
	SET @SessionID=(SELECT Top(1)SessionID FROM AccountingSession  WHERE  CONVERT(DATE,CONVERT(VARCHAR(12),StartDate,106))=CONVERT(DATE,CONVERT(VARCHAR(12),@StartDate,106)) AND SessionType=6)
END
ELSE
BEGIN
	SET @SessionID=(SELECT TOP 1 SessionID FROM AccountingSession  WHERE  CONVERT(DATE,CONVERT(VARCHAR(12),StartDate,106))>=CONVERT(DATE,CONVERT(VARCHAR(12),@StartDate,106)) AND SessionType=6)
	SET @StartDate=(SELECT MIN(StartDate) FROM AccountingSession  WHERE  CONVERT(DATE,CONVERT(VARCHAR(12),StartDate,106))>=CONVERT(DATE,CONVERT(VARCHAR(12),@StartDate,106)) AND SessionType=6)
END

SET @SessionStartDate=(SELECT StartDate FROM AccountingSession WHERE  AccountingSessionID=@SessionID)
SET @ComponentType=  dbo.GetComponentID(@AccountHeadID) --Set CompanyID

--Reset BusinessUnit Start & End Code
IF(@StartBusinessUnitCode!='00')
BEGIN
	IF(@EndBusinessUnitCode='00')
	BEGIN
		SET @EndBusinessUnitCode=@StartBusinessUnitCode
	END
END
ELSE
BEGIN
	SET @StartBusinessUnitCode=ISNULL((SELECT TT.Code FROM BusinessUnit AS TT WHERE TT.BusinessUnitID=(SELECT MIN(BU.BusinessUnitID) FROM BusinessUnit AS BU)),'00')
	SET @EndBusinessUnitCode=ISNULL((SELECT TT.Code FROM BusinessUnit AS TT WHERE TT.BusinessUnitID=(SELECT MAX(BU.BusinessUnitID) FROM BusinessUnit AS BU)),'00')	
END

SET @DebitOpenningBalance=0
SET @CreditOpenningBalance=0
SET @DebitTransactionAmount=0
SET @CreditTransactionAmount=0

IF(@IsApproved=1)
BEGIN
	IF(@IsBaseCurrency=1)
	BEGIN
		SET @DebitOpenningBalance=ISNULL((SELECT ISNULL(SUM(AO.OpenningBalance),0) FROM AccountOpenning AS AO WHERE AO.IsDebit=1 AND  AO.AccountingSessionID=@SessionID AND AO.AccountHeadID=@AccountHeadID AND AO.BusinessUnitID IN (SELECT BU.BusinessUnitID FROM BusinessUnit AS BU WHERE CONVERT(INT,BU.Code) BETWEEN CONVERT(INT,@StartBusinessUnitCode) AND CONVERT(INT,@EndBusinessUnitCode))),0)
		SET @CreditOpenningBalance=ISNULL((SELECT ISNULL(SUM(AO.OpenningBalance),0) FROM AccountOpenning AS AO WHERE AO.IsDebit=0 AND  AO.AccountingSessionID=@SessionID AND AO.AccountHeadID=@AccountHeadID AND AO.BusinessUnitID IN (SELECT BU.BusinessUnitID FROM BusinessUnit AS BU WHERE CONVERT(INT,BU.Code) BETWEEN CONVERT(INT,@StartBusinessUnitCode) AND CONVERT(INT,@EndBusinessUnitCode))),0)
		IF(@IsTransactioncount=1)
		BEGIN
			SET @DebitTransactionAmount=(SELECT ISNULL(SUM(VD.Amount),0) FROM View_VoucherDetail AS VD WHERE  VD.IsDebit=1 AND VD.AccountHeadID=@AccountHeadID AND VD.BUID IN (SELECT BU.BusinessUnitID FROM BusinessUnit AS BU WHERE CONVERT(INT,BU.Code) BETWEEN CONVERT(INT,@StartBusinessUnitCode) AND CONVERT(INT,@EndBusinessUnitCode)) AND VD.VoucherID IN (SELECT VoucherID FROM Voucher AS V WHERE ISNULL(V.AuthorizedBy,0)!=0 AND  CONVERT(DATE,CONVERT(VARCHAR(12),V.VoucherDate,106)) BETWEEN CONVERT(DATE,CONVERT(VARCHAR(12),@SessionStartDate,106)) AND CONVERT(DATE,CONVERT(VARCHAR(12),DATEADD(DAY,-1,@StartDate),106))))
			SET @CreditTransactionAmount=(SELECT ISNULL(SUM(VD.Amount),0) FROM View_VoucherDetail AS VD WHERE  VD.IsDebit=0 AND VD.AccountHeadID=@AccountHeadID AND VD.BUID IN (SELECT BU.BusinessUnitID FROM BusinessUnit AS BU WHERE CONVERT(INT,BU.Code) BETWEEN CONVERT(INT,@StartBusinessUnitCode) AND CONVERT(INT,@EndBusinessUnitCode)) AND VD.VoucherID IN(SELECT VoucherID FROM Voucher AS V WHERE ISNULL(V.AuthorizedBy,0)!=0 AND  CONVERT(DATE,CONVERT(VARCHAR(12),V.VoucherDate,106)) BETWEEN CONVERT(DATE,CONVERT(VARCHAR(12),@SessionStartDate,106)) AND CONVERT(DATE,CONVERT(VARCHAR(12),DATEADD(DAY,-1,@StartDate),106))))
		END
	END
	ELSE
	BEGIN
		SET @DebitOpenningBalance=ISNULL((SELECT ISNULL(SUM(AmountInCurrency),0) FROM AccountOpenning AS AO WHERE AO.IsDebit=1 AND AO.AccountingSessionID=@SessionID AND AO.AccountHeadID=@AccountHeadID AND AO.CurrencyID=@CurrencyID AND AO.BusinessUnitID IN (SELECT BU.BusinessUnitID FROM BusinessUnit AS BU WHERE CONVERT(INT,BU.Code) BETWEEN CONVERT(INT,@StartBusinessUnitCode) AND CONVERT(INT,@EndBusinessUnitCode))),0)
		SET @CreditOpenningBalance=ISNULL((SELECT ISNULL(SUM(AmountInCurrency),0) FROM AccountOpenning AS AO WHERE AO.IsDebit=0 AND AO.AccountingSessionID=@SessionID AND AO.AccountHeadID=@AccountHeadID AND AO.CurrencyID=@CurrencyID AND AO.BusinessUnitID IN (SELECT BU.BusinessUnitID FROM BusinessUnit AS BU WHERE CONVERT(INT,BU.Code) BETWEEN CONVERT(INT,@StartBusinessUnitCode) AND CONVERT(INT,@EndBusinessUnitCode))),0)
		IF(@IsTransactioncount=1)
		BEGIN
			SET @DebitTransactionAmount=(SELECT ISNULL(SUM(VD.AmountInCurrency),0) FROM View_VoucherDetail AS VD WHERE  VD.CurrencyID=@CurrencyID AND VD.IsDebit=1 AND VD.AccountHeadID=@AccountHeadID AND VD.BUID IN (SELECT BU.BusinessUnitID FROM BusinessUnit AS BU WHERE CONVERT(INT,BU.Code) BETWEEN CONVERT(INT,@StartBusinessUnitCode) AND CONVERT(INT,@EndBusinessUnitCode)) AND VD.VoucherID IN (SELECT VoucherID FROM Voucher AS V WHERE ISNULL(V.AuthorizedBy,0)!=0 AND  CONVERT(DATE,CONVERT(VARCHAR(12),V.VoucherDate,106)) BETWEEN CONVERT(DATE,CONVERT(VARCHAR(12),@SessionStartDate,106)) AND CONVERT(DATE,CONVERT(VARCHAR(12),DATEADD(DAY,-1,@StartDate),106))))
			SET @CreditTransactionAmount=(SELECT ISNULL(SUM(VD.AmountInCurrency),0) FROM View_VoucherDetail AS VD WHERE  VD.CurrencyID=@CurrencyID AND VD.IsDebit=0 AND VD.AccountHeadID=@AccountHeadID AND VD.BUID IN (SELECT BU.BusinessUnitID FROM BusinessUnit AS BU WHERE CONVERT(INT,BU.Code) BETWEEN CONVERT(INT,@StartBusinessUnitCode) AND CONVERT(INT,@EndBusinessUnitCode)) AND VD.VoucherID IN(SELECT VoucherID FROM Voucher AS V WHERE ISNULL(V.AuthorizedBy,0)!=0 AND  CONVERT(DATE,CONVERT(VARCHAR(12),V.VoucherDate,106)) BETWEEN CONVERT(DATE,CONVERT(VARCHAR(12),@SessionStartDate,106)) AND CONVERT(DATE,CONVERT(VARCHAR(12),DATEADD(DAY,-1,@StartDate),106))))
		END
	END
END
ELSE
BEGIN
	IF(@IsBaseCurrency=1)
	BEGIN
		SET @DebitOpenningBalance=ISNULL((SELECT ISNULL(Sum(OpenningBalance),0) FROM AccountOpenning AS AO WHERE AO.IsDebit=1 AND  AO.AccountingSessionID=@SessionID AND AO.AccountHeadID=@AccountHeadID AND AO.BusinessUnitID IN (SELECT BU.BusinessUnitID FROM BusinessUnit AS BU WHERE CONVERT(INT,BU.Code) BETWEEN CONVERT(INT,@StartBusinessUnitCode) AND CONVERT(INT,@EndBusinessUnitCode))),0)
		SET @CreditOpenningBalance=ISNULL((SELECT ISNULL(Sum(OpenningBalance),0) FROM AccountOpenning AS AO WHERE AO.IsDebit=0 AND  AO.AccountingSessionID=@SessionID AND AO.AccountHeadID=@AccountHeadID AND AO.BusinessUnitID IN (SELECT BU.BusinessUnitID FROM BusinessUnit AS BU WHERE CONVERT(INT,BU.Code) BETWEEN CONVERT(INT,@StartBusinessUnitCode) AND CONVERT(INT,@EndBusinessUnitCode))),0)
		IF(@IsTransactioncount=1)
		BEGIN
			SET @DebitTransactionAmount=(SELECT ISNULL(SUM(VD.Amount),0) FROM View_VoucherDetail AS VD WHERE  VD.IsDebit=1 AND VD.AccountHeadID=@AccountHeadID AND VD.BUID IN (SELECT BU.BusinessUnitID FROM BusinessUnit AS BU WHERE CONVERT(INT,BU.Code) BETWEEN CONVERT(INT,@StartBusinessUnitCode) AND CONVERT(INT,@EndBusinessUnitCode)) AND VD.VoucherID IN (SELECT VoucherID FROM Voucher AS V WHERE CONVERT(DATE,CONVERT(VARCHAR(12),V.VoucherDate,106)) BETWEEN CONVERT(DATE,CONVERT(VARCHAR(12),@SessionStartDate,106)) AND CONVERT(DATE,CONVERT(VARCHAR(12),DATEADD(DAY,-1,@StartDate),106))))
			SET @CreditTransactionAmount=(SELECT ISNULL(SUM(VD.Amount),0) FROM View_VoucherDetail AS VD WHERE  VD.IsDebit=0 AND VD.AccountHeadID=@AccountHeadID AND VD.BUID IN (SELECT BU.BusinessUnitID FROM BusinessUnit AS BU WHERE CONVERT(INT,BU.Code) BETWEEN CONVERT(INT,@StartBusinessUnitCode) AND CONVERT(INT,@EndBusinessUnitCode)) AND VD.VoucherID IN(SELECT VoucherID FROM Voucher AS V WHERE CONVERT(DATE,CONVERT(VARCHAR(12),V.VoucherDate,106)) BETWEEN CONVERT(DATE,CONVERT(VARCHAR(12),@SessionStartDate,106)) AND CONVERT(DATE,CONVERT(VARCHAR(12),DATEADD(DAY,-1,@StartDate),106))))
		END
	END
	ELSE
	BEGIN
		SET @DebitOpenningBalance=ISNULL((SELECT ISNULL(SUM(AmountInCurrency),0) FROM AccountOpenning AS AO WHERE AO.IsDebit=1 AND AO.AccountingSessionID=@SessionID AND AO.AccountHeadID=@AccountHeadID AND AO.CurrencyID=@CurrencyID AND AO.BusinessUnitID IN (SELECT BU.BusinessUnitID FROM BusinessUnit AS BU WHERE CONVERT(INT,BU.Code) BETWEEN CONVERT(INT,@StartBusinessUnitCode) AND CONVERT(INT,@EndBusinessUnitCode))),0)
		SET @CreditOpenningBalance=ISNULL((SELECT ISNULL(SUM(AmountInCurrency),0) FROM AccountOpenning AS AO WHERE AO.IsDebit=0 AND AO.AccountingSessionID=@SessionID AND AO.AccountHeadID=@AccountHeadID AND AO.CurrencyID=@CurrencyID AND AO.BusinessUnitID IN (SELECT BU.BusinessUnitID FROM BusinessUnit AS BU WHERE CONVERT(INT,BU.Code) BETWEEN CONVERT(INT,@StartBusinessUnitCode) AND CONVERT(INT,@EndBusinessUnitCode))),0)
		IF(@IsTransactioncount=1)
		BEGIN
			SET @DebitTransactionAmount=(SELECT ISNULL(SUM(VD.AmountInCurrency),0) FROM View_VoucherDetail AS VD WHERE  VD.CurrencyID=@CurrencyID AND VD.IsDebit=1 AND VD.AccountHeadID=@AccountHeadID AND VD.BUID IN (SELECT BU.BusinessUnitID FROM BusinessUnit AS BU WHERE CONVERT(INT,BU.Code) BETWEEN CONVERT(INT,@StartBusinessUnitCode) AND CONVERT(INT,@EndBusinessUnitCode)) AND VD.VoucherID IN (SELECT VoucherID FROM Voucher AS V WHERE CONVERT(DATE,CONVERT(VARCHAR(12),V.VoucherDate,106)) BETWEEN CONVERT(DATE,CONVERT(VARCHAR(12),@SessionStartDate,106)) AND CONVERT(DATE,CONVERT(VARCHAR(12),DATEADD(DAY,-1,@StartDate),106))))
			SET @CreditTransactionAmount=(SELECT ISNULL(SUM(VD.AmountInCurrency),0) FROM View_VoucherDetail AS VD WHERE  VD.CurrencyID=@CurrencyID AND VD.IsDebit=0 AND VD.AccountHeadID=@AccountHeadID AND VD.BUID IN (SELECT BU.BusinessUnitID FROM BusinessUnit AS BU WHERE CONVERT(INT,BU.Code) BETWEEN CONVERT(INT,@StartBusinessUnitCode) AND CONVERT(INT,@EndBusinessUnitCode)) AND VD.VoucherID IN(SELECT VoucherID FROM Voucher AS V WHERE CONVERT(DATE,CONVERT(VARCHAR(12),V.VoucherDate,106)) BETWEEN CONVERT(DATE,CONVERT(VARCHAR(12),@SessionStartDate,106)) AND CONVERT(DATE,CONVERT(VARCHAR(12),DATEADD(DAY,-1,@StartDate),106))))
		END
	END
END

IF(@ComponentType IN(2,6))--ComponentID{Asset = 2,Laibility = 3,OwnerEquity=4,Income = 5,Expeness = 6}
BEGIN
	SET @OpenningBalance =@DebitOpenningBalance-@CreditOpenningBalance+@DebitTransactionAmount-@CreditTransactionAmount
	IF(@OpenningBalance>=0)
	BEGIN	
		INSERT INTO @Temp_Table	(IsDebit,	DebitAmount,		CreditAmount,	OpenningBalance)	
						VALUES	(1,			@OpenningBalance,	0.00,			@OpenningBalance)
	END
	ELSE
	BEGIN	
		INSERT INTO @Temp_Table	(IsDebit,	DebitAmount,	CreditAmount,				OpenningBalance)	
						VALUES	(0,			0.00,			((-1)*@OpenningBalance),	@OpenningBalance)
	END
END
ELSE
BEGIN
	SET @OpenningBalance =@CreditOpenningBalance-@DebitOpenningBalance-@DebitTransactionAmount+@CreditTransactionAmount	
	IF(@OpenningBalance>=0)
	BEGIN	
		INSERT INTO @Temp_Table	(IsDebit,	DebitAmount,		CreditAmount,		OpenningBalance)	
						VALUES	(0,			0.00,				@OpenningBalance,	@OpenningBalance)
	END
	ELSE
	BEGIN	
		INSERT INTO @Temp_Table	(IsDebit,	DebitAmount,				CreditAmount,	OpenningBalance)	
						VALUES	(1,			((-1)*@OpenningBalance),	0.00,			@OpenningBalance)
	END
END	
RETURN
END;





GO
/****** Object:  View [dbo].[View_VoucherBillTransaction]    Script Date: 12/1/2016 2:17:46 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE VIEW [dbo].[View_VoucherBillTransaction]
AS
SELECT	VoucherBillTransaction.*,       
		VoucherBill.BillNo,
		VoucherBill.Amount AS BillAmount,
		VoucherBill.BillDate,
		CU.Symbol AS CurrencySymbol,
		VD.AccountHeadCode,		
		VD.AccountHeadName,
		VD.AccountHeadID,
		(SELECT BaseCurrencyID FROM Company WHERE CompanyID=1) AS BaseCurrencyID,
		(SELECT Symbol FROM Currency  WHERE CurrencyID IN (SELECT BaseCurrencyID FROM Company WHERE CompanyID=1)) AS BaseCurrencySymbol,
		(SELECT VoucherNo FROM Voucher WHERE VoucherID =VD.VoucherID) AS VoucherNo,
		VD.VoucherID,
		VD.AuthorizedBy ApprovedBy,
		VD.BUID,
		CCT.CCID,
		CCT.CostCenterCode,
		CCT.CostCenterName


FROM         VoucherBillTransaction 
LEFT OUTER JOIN   VoucherBill ON VoucherBill.VoucherBillID = VoucherBillTransaction.VoucherBillID 
LEFT  OUTER JOIN   Currency as CU ON CU.CurrencyID = VoucherBillTransaction.CurrencyID
LEFT  OUTER JOIN   View_VoucherDetail as VD ON VD.VoucherDetailID = VoucherBillTransaction.VoucherDetailID
LEFT  OUTER JOIN   View_CostCenterTransaction as CCT ON CCT.CCTID= VoucherBillTransaction.CCTID








GO
/****** Object:  View [dbo].[View_VoucherBill]    Script Date: 12/1/2016 2:17:46 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE VIEW [dbo].[View_VoucherBill]
AS
SELECT	VoucherBill.VoucherBillID,
		VoucherBill.AccountHeadID,
		VoucherBill.SubLedgerID,
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
		CASE	WHEN ISNULL((SELECT TOP 1 HH.BUID FROM View_VoucherBillTransaction AS HH WHERE HH.VoucherBillID=VoucherBill.VoucherBillID),0)!=0 
				THEN ISNULL((SELECT TOP 1 HH.BUID FROM View_VoucherBillTransaction AS HH WHERE HH.VoucherBillID=VoucherBill.VoucherBillID),0)
				ELSE ISNULL((SELECT TOP 1 MM.BusinessUnitID FROM AccountOpenningBreakdown AS MM WHERE MM.BreakdownObjID=VoucherBill.VoucherBillID AND MM.BreakdownType=2),0)--BillReference = 2
		END AS BUID,
		COA.AccountHeadName,
		COA.AccountCode,
		COA.AccountHeadCodeName,
		ACC.Name AS SubLedgerName,
		ACC.Code AS SubLedgerCode,
		ACC.NameCode AS SubLedgerNameCode
FROM VoucherBill
LEFT OUTER JOIN View_ChartsOfAccount AS COA ON COA.AccountHeadID=VoucherBill.AccountHeadID
LEFT OUTER JOIN View_ACCostCenter AS ACC ON ACC.ACCostCenterID=VoucherBill.SubLedgerID







GO
/****** Object:  View [dbo].[View_VOReference]    Script Date: 12/1/2016 2:17:46 PM ******/
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
		SO.SalesNo,
		SO.ProjectName,
		SO.SalesDate,
		SO.Amount as SalesAmount,
		VD.BUID,
		VD.AuthorizedBy as ApprovedBy,
		VD.AccountHeadID,
		VD.VoucherID

FROM		VOReference 
LEFT OUTER JOIN View_SaleOrder AS SO ON VOReference.OrderID = SO.SaleOrderID
LEFT OUTER JOIN View_VoucherDetail AS VD ON VOReference.VoucherDetailID=VD.VoucherDetailID








GO
/****** Object:  View [dbo].[View_VoucherBatchHistory]    Script Date: 12/1/2016 2:17:46 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


CREATE VIEW [dbo].[View_VoucherBatchHistory]
AS
SELECT			VB.VoucherBatchHistoryID
				, VB.VoucherBatchID
				, VB.BatchNO
				, VB.CreateBy
				, VB.CreateDate
				, VB.PreviousBatchStatus
				, VB.CurrentBatchStatus
				, VB.RequestTo
				, VB.RequestDate
				, VB.Note
				, VB.DBUserID
				, VB.DBServerDateTime
				, VB.LastUpdateBy
				, VB.LastUpdateDateTime
				, CU.UserName AS CreateByName
				, REQU.UserName AS RequestToName
				, (SELECT COUNT(*) FROM Voucher AS V WHERE V.BatchID=VB.VoucherBatchID) AS VoucherCount

FROM            dbo.VoucherBatchHistory AS VB 
				LEFT OUTER JOIN dbo.Users AS REQU ON VB.RequestTo = REQU.UserID  
				LEFT OUTER JOIN dbo.Users AS CU ON VB.CreateBy = CU.UserID


GO
/****** Object:  View [dbo].[View_VoucherCheque]    Script Date: 12/1/2016 2:17:46 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE VIEW [dbo].[View_VoucherCheque]
AS
SELECT	VoucherCheque.VoucherChequeID, 
		VoucherCheque.VoucherDetailID, 
		VoucherCheque.CCTID,
		VoucherCheque.ChequeType, 
		VoucherCheque.ChequeID, 
		VoucherCheque.TransactionDate, 
		VoucherCheque.Amount, 
        VD.AccountHeadID,
		VD.VoucherID,
		CCT.CCID,
		CCT.CostCenterCode,
		CCT.CostCenterName,
		(SELECT TT.AccountHeadName FROM ChartsOfAccount AS TT WHERE TT.AccountHeadID=VD.AccountHeadID) AS AccountHeadName,
		(SELECT TT.AccountCode FROM ChartsOfAccount AS TT WHERE TT.AccountHeadID=VD.AccountHeadID) AS AccountCode,
				
		CASE WHEN VoucherCheque.ChequeType=2 THEN (SELECT TT.ChequeNo FROM Cheque AS TT WHERE TT.ChequeID=VoucherCheque.ChequeID)
			 WHEN VoucherCheque.ChequeType=1 THEN (SELECT TT.ChequeNo FROM ReceivedCheque AS TT WHERE TT.ReceivedChequeID=VoucherCheque.ChequeID)
		END AS ChequeNo,

		CASE WHEN VoucherCheque.ChequeType=2 THEN (SELECT TT.ChequeDate FROM Cheque AS TT WHERE TT.ChequeID=VoucherCheque.ChequeID)
			 WHEN VoucherCheque.ChequeType=1 THEN (SELECT TT.ChequeDate FROM ReceivedCheque AS TT WHERE TT.ReceivedChequeID=VoucherCheque.ChequeID)
		END AS ChequeDate,

		CASE WHEN VoucherCheque.ChequeType=2 THEN (SELECT TT.BankName FROM View_Cheque AS TT WHERE TT.ChequeID=VoucherCheque.ChequeID)
			 WHEN VoucherCheque.ChequeType=1 THEN (SELECT TT.BankName FROM ReceivedCheque AS TT WHERE TT.ReceivedChequeID=VoucherCheque.ChequeID)
		END AS BankName,

		CASE WHEN VoucherCheque.ChequeType=2 THEN (SELECT TT.BankBranchName FROM View_Cheque AS TT WHERE TT.ChequeID=VoucherCheque.ChequeID)
			 WHEN VoucherCheque.ChequeType=1 THEN (SELECT TT.BranchName FROM ReceivedCheque AS TT WHERE TT.ReceivedChequeID=VoucherCheque.ChequeID)
		END AS BranchName,

		CASE WHEN VoucherCheque.ChequeType=2 THEN (SELECT TT.AccountNo FROM View_Cheque AS TT WHERE TT.ChequeID=VoucherCheque.ChequeID)
			 WHEN VoucherCheque.ChequeType=1 THEN (SELECT TT.AccountNo FROM ReceivedCheque AS TT WHERE TT.ReceivedChequeID=VoucherCheque.ChequeID)
		END AS AccountNo,
		VD.AuthorizedBy AS ApprovedBy

FROM    VoucherCheque LEFT OUTER JOIN 
		View_VoucherDetail AS VD ON VoucherCheque.VoucherDetailID = VD.VoucherDetailID LEFT OUTER JOIN
		View_CostCenterTransaction AS CCT ON VoucherCheque.CCTID=CCT.CCTID








GO
/****** Object:  View [dbo].[View_VoucherHistory]    Script Date: 12/1/2016 2:17:46 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE VIEW [dbo].[View_VoucherHistory]
AS
SELECT	VH.VoucherHistoryID
		, VH.VoucherID
		, VH.UserID
		, VH.TransactionDate
		, VH.ActionType
		, VH.Remarks
		, VH.DBUserID
		, VH.DBServerDateTime
		, VH.LastUpdateBy
		, VH.LastUpdateDateTime
		, U.UserName+' ['+U.LoginID+']' AS UserName
		, U.EmployeeNameCode
		, V.VoucherTypeID
		, V.VoucherNo
		, V.Narration
		, V.VoucherName
		, V.VoucherDate
		, V.AuthorizedByName
		, V.PreparedByName
		, V.VoucherAmount
		, V.DBServerDate AS PostingDate
		

FROM    dbo.VoucherHistory AS VH
LEFT OUTER JOIN dbo.View_User AS U ON VH.UserID = U.UserID 
LEFT OUTER JOIN dbo.View_Voucher AS V ON VH.VoucherID = V.VoucherID




GO
/****** Object:  View [dbo].[View_VoucherReference]    Script Date: 12/1/2016 2:17:46 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO




CREATE VIEW [dbo].[View_VoucherReference]
AS
SELECT		VoucherReference.*,
			(SELECT Symbol FROM Currency WHERE CurrencyID = VoucherReference.CurrencyID) AS CurrencySymbol


FROM	     VoucherReference




GO
/****** Object:  View [dbo].[View_VProduct]    Script Date: 12/1/2016 2:17:46 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE VIEW [dbo].[View_VProduct]
AS
SELECT	TT.VProductID, 
		TT.ProductCode, 
		TT.ProductName, 
		TT.ShortName, 
		TT.BrandName, 
		TT.Remarks,
		TT.ProductName+ ' ['+TT.ProductCode+']' AS NameCode

FROM    dbo.VProduct AS TT


GO
/****** Object:  View [dbo].[View_VPTransaction]    Script Date: 12/1/2016 2:17:46 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE VIEW [dbo].[View_VPTransaction]
AS
SELECT	VPTransaction.*
        ,Product.ProductName
		,Product.ProductCode
        ,MU.Symbol as MUnitName
		,CU.Symbol AS CurrencySymbol,
		(SELECT WU.OperationUnitName FROM View_WorkingUnit AS WU WHERE WU.WorkingUnitID=VPTransaction.WorkingUnitID) AS OperationUnitName,
		(SELECT WU.LocationName FROM View_WorkingUnit AS WU WHERE WU.WorkingUnitID=VPTransaction.WorkingUnitID) AS LocationName,
		(SELECT WU.LocationName+'['+WU.OperationUnitName+']' FROM View_WorkingUnit AS WU WHERE WU.WorkingUnitID=VPTransaction.WorkingUnitID) AS WorkingUnitName,
		VD.AccountHeadID, 
		VD.VoucherID,
		VD.AuthorizedBy AS ApprovedBy,
		VD.BUID
FROM         VPTransaction 
LEFT OUTER JOIN Product ON Product.ProductID = VPTransaction.ProductID 
LEFT OUTER JOIN MeasurementUnit as MU ON MU.MeasurementUnitID = VPTransaction.MUnitID
LEFT OUTER JOIN Currency as CU ON CU.CurrencyID = VPTransaction.CurrencyID
LEFT OUTER JOIN View_VoucherDetail AS VD ON VD.VoucherDetailID=VPTransaction.VoucherDetailID

















GO
