GO
/****** Object:  View [dbo].[View_AccountsBookSetupDetail]    Script Date: 1/12/2017 10:23:08 AM ******/
DROP VIEW [dbo].[View_AccountsBookSetupDetail]
GO
/****** Object:  Table [dbo].[AccountsBookSetupDetail]    Script Date: 1/12/2017 10:23:08 AM ******/
DROP TABLE [dbo].[AccountsBookSetupDetail]
GO
/****** Object:  Table [dbo].[AccountsBookSetup]    Script Date: 1/12/2017 10:23:08 AM ******/
DROP TABLE [dbo].[AccountsBookSetup]
GO
/****** Object:  StoredProcedure [dbo].[SP_IUD_VoucherBill]    Script Date: 1/12/2017 10:23:08 AM ******/
DROP PROCEDURE [dbo].[SP_IUD_VoucherBill]
GO
/****** Object:  StoredProcedure [dbo].[SP_IUD_AccountsBookSetupDetail]    Script Date: 1/12/2017 10:23:08 AM ******/
DROP PROCEDURE [dbo].[SP_IUD_AccountsBookSetupDetail]
GO
/****** Object:  StoredProcedure [dbo].[SP_IUD_AccountsBookSetup]    Script Date: 1/12/2017 10:23:08 AM ******/
DROP PROCEDURE [dbo].[SP_IUD_AccountsBookSetup]
GO
/****** Object:  StoredProcedure [dbo].[SP_AccountsBook]    Script Date: 1/12/2017 10:23:08 AM ******/
DROP PROCEDURE [dbo].[SP_AccountsBook]
GO
/****** Object:  StoredProcedure [dbo].[SP_AccountsBook]    Script Date: 1/12/2017 10:23:08 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[SP_AccountsBook]
(
	@AccountsBookSetUpID as int,
	@StartDate as date,
	@EndDate as date,
	@BUID as int,
	@ApprivedOnly as bit
	--%n, %d, %d, %n, %b
)
AS
BEGIN TRAN
--DECLARE
--@AccountsBookSetUpID as int,
--@StartDate as date,
--@EndDate as date,
--@BUID as int,
--@ApprivedOnly as bit

--SET @AccountsBookSetUpID=2
--SET @StartDate='01 Jan 2017'
--SET @EndDate='20 Jan 2017'
--SET @BUID = 1
--SET @ApprivedOnly=0

CREATE TABLE #TempTable (
							AccountHeadID int,
							AccountCode Varchar(512),
							AccountHeadName Varchar(512),
							MappingType smallint,							
							ComponentType int,
							AccountType smallint,									
							CategoryName Varchar(512),
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


DECLARE
@MappingType as smallint
SET @MappingType = (SELECT HH.MappingType FROM AccountsBookSetup AS HH WHERE HH.AccountsBookSetupID=@AccountsBookSetUpID)


--EnumACMappingType{ None = 0, Ledger = 1, SubLedger = 2 }
IF(@MappingType = 1)
BEGIN
	INSERT INTO #TempTable	(AccountHeadID,		AccountCode,	AccountHeadName,	AccountType,	CategoryName,			MappingType)
					SELECT	 TT.AccountHeadID,	TT.AccountCode,	TT.AccountHeadName,	TT.AccountType,	TT.ParentHeadName,     @MappingType FROM dbo.View_ChartsOfAccount AS TT WHERE TT.AccountHeadID IN (SELECT ABD.AccountHeadID FROM AccountsBookSetupDetail AS ABD WHERE ABD.AccountsBookSetupID=@AccountsBookSetUpID)
END
ELSE
BEGIN
	INSERT INTO #TempTable	(AccountHeadID,			AccountCode,	AccountHeadName,	AccountType,	CategoryName,		MappingType)
					SELECT	 TT.ACCostCenterID,		TT.Code,		TT.Name,			0,				TT.CategoryName,	@MappingType FROM dbo.View_ACCostCenter AS TT WHERE TT.ACCostCenterID IN (SELECT ABD.AccountHeadID FROM AccountsBookSetupDetail AS ABD WHERE ABD.AccountsBookSetupID=@AccountsBookSetUpID)
END

INSERT INTO #TempTable2 (AccountHeadID)
			   SELECT TT.AccountHeadID FROM #TempTable AS TT


--EnumAccountType{None = 0,Component = 1,Segment =2, Group = 3,SubGroup = 4,Ledger = 5}
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

IF(@MappingType=1)
BEGIN
	IF(@BUID=0)
	BEGIN
		UPDATE #TempTable2
		SET	ComponentType=dbo.GetComponentID(TT.AccountHeadID),
			OpeiningValue=ISNULL((SELECT TOP 1 HH.OpenningBalance FROM AccountOpenning AS HH WHERE HH.AccountHeadID=TT.AccountHeadID AND HH.AccountingSessionID=@SessionID),0),
			DebitAmount=ISNULL((SELECT SUM(VD.Amount) FROM View_VoucherDetail AS VD WHERE VD.IsDebit=1 AND VD.AccountHeadID=TT.AccountHeadID AND VD.VoucherID IN(SELECT V.VoucherID FROM Voucher AS V WHERE ISNULL(V.AuthorizedBy,0)!=0 AND CONVERT(DATE,CONVERT(VARCHAR(12),V.VoucherDate,106)) BETWEEN CONVERT(DATE,CONVERT(VARCHAR(12),@SessionStartDate,106)) AND CONVERT(DATE,CONVERT(VARCHAR(12), DATEADD(DAY,-1,@StartDate),106)))),0),
			CreditAmount=ISNULL((SELECT SUM(VD.Amount) FROM View_VoucherDetail AS VD WHERE VD.IsDebit=0 AND VD.AccountHeadID=TT.AccountHeadID AND VD.VoucherID IN(SELECT V.VoucherID FROM Voucher AS V WHERE ISNULL(V.AuthorizedBy,0)!=0 AND CONVERT(DATE,CONVERT(VARCHAR(12),V.VoucherDate,106)) BETWEEN CONVERT(DATE,CONVERT(VARCHAR(12),@SessionStartDate,106)) AND CONVERT(DATE,CONVERT(VARCHAR(12), DATEADD(DAY,-1,@StartDate),106)))),0)
		FROM #TempTable2 AS TT 

		UPDATE #TempTable2 SET ClosingValue=TT.OpeiningValue+TT.DebitAmount-TT.CreditAmount FROM #TempTable2 AS TT WHERE TT.ComponentType IN (2,6)
		UPDATE #TempTable2 SET ClosingValue=TT.OpeiningValue-TT.DebitAmount+TT.CreditAmount FROM #TempTable2 AS TT WHERE TT.ComponentType NOT IN (2,6)

		IF(@ApprivedOnly=1)
		BEGIN
			UPDATE #TempTable
			SET ComponentType=dbo.GetComponentID(TT.AccountHeadID),	
				OpenningBalance=ISNULL((SELECT BB.ClosingValue FROM #TempTable2 AS BB WHERE BB.AccountHeadID=TT.AccountHeadID),0),
				DebitAmount=ISNULL((SELECT SUM(VD.Amount) FROM View_VoucherDetail AS VD WHERE VD.IsDebit=1 AND VD.AccountHeadID=TT.AccountHeadID AND VD.VoucherID IN(SELECT V.VoucherID FROM Voucher AS V WHERE ISNULL(V.AuthorizedBy,0)!=0 AND CONVERT(DATE,CONVERT(VARCHAR(12),V.VoucherDate,106)) BETWEEN CONVERT(DATE,CONVERT(VARCHAR(12),@StartDate,106)) AND CONVERT(DATE,CONVERT(VARCHAR(12), @EndDate,106)))),0),
				CreditAmount=ISNULL((SELECT SUM(VD.Amount) FROM View_VoucherDetail AS VD WHERE VD.IsDebit=0 AND VD.AccountHeadID=TT.AccountHeadID AND VD.VoucherID IN(SELECT V.VoucherID FROM Voucher AS V WHERE ISNULL(V.AuthorizedBy,0)!=0 AND CONVERT(DATE,CONVERT(VARCHAR(12),V.VoucherDate,106)) BETWEEN CONVERT(DATE,CONVERT(VARCHAR(12),@StartDate,106)) AND CONVERT(DATE,CONVERT(VARCHAR(12), @EndDate,106)))),0)
			FROM #TempTable AS TT WHERE TT.AccountType=5
		END
		ELSE
		BEGIN
			UPDATE #TempTable
			SET ComponentType=dbo.GetComponentID(TT.AccountHeadID),					
				OpenningBalance=ISNULL((SELECT BB.ClosingValue FROM #TempTable2 AS BB WHERE BB.AccountHeadID=TT.AccountHeadID),0),
				DebitAmount=ISNULL((SELECT SUM(VD.Amount) FROM View_VoucherDetail AS VD WHERE VD.IsDebit=1 AND VD.AccountHeadID=TT.AccountHeadID AND VD.VoucherID IN(SELECT V.VoucherID FROM Voucher AS V WHERE CONVERT(DATE,CONVERT(VARCHAR(12),V.VoucherDate,106)) BETWEEN CONVERT(DATE,CONVERT(VARCHAR(12),@StartDate,106)) AND CONVERT(DATE,CONVERT(VARCHAR(12), @EndDate,106)))),0),
				CreditAmount=ISNULL((SELECT SUM(VD.Amount) FROM View_VoucherDetail AS VD WHERE VD.IsDebit=0 AND VD.AccountHeadID=TT.AccountHeadID AND VD.VoucherID IN(SELECT V.VoucherID FROM Voucher AS V WHERE CONVERT(DATE,CONVERT(VARCHAR(12),V.VoucherDate,106)) BETWEEN CONVERT(DATE,CONVERT(VARCHAR(12),@StartDate,106)) AND CONVERT(DATE,CONVERT(VARCHAR(12), @EndDate,106)))),0)
			FROM #TempTable AS TT WHERE TT.AccountType=5
		END
	
		UPDATE #TempTable SET ClosingBalance=TT.OpenningBalance+TT.DebitAmount-TT.CreditAmount FROM #TempTable AS TT WHERE TT.ComponentType IN (2,6)
		UPDATE #TempTable SET ClosingBalance=TT.OpenningBalance-TT.DebitAmount+TT.CreditAmount FROM #TempTable AS TT WHERE TT.ComponentType NOT IN (2,6)
	END
	ELSE
	BEGIN
		UPDATE #TempTable2
		SET	ComponentType=dbo.GetComponentID(TT.AccountHeadID),
			OpeiningValue=ISNULL((SELECT TOP 1 HH.OpenningBalance FROM AccountOpenning AS HH WHERE HH.BusinessUnitID=@BUID AND HH.AccountHeadID=TT.AccountHeadID AND HH.AccountingSessionID=@SessionID),0),
			DebitAmount=ISNULL((SELECT SUM(VD.Amount) FROM View_VoucherDetail AS VD WHERE VD.BUID = @BUID AND VD.IsDebit=1 AND VD.AccountHeadID=TT.AccountHeadID AND VD.VoucherID IN(SELECT V.VoucherID FROM Voucher AS V WHERE ISNULL(V.AuthorizedBy,0)!=0 AND CONVERT(DATE,CONVERT(VARCHAR(12),V.VoucherDate,106)) BETWEEN CONVERT(DATE,CONVERT(VARCHAR(12),@SessionStartDate,106)) AND CONVERT(DATE,CONVERT(VARCHAR(12), DATEADD(DAY,-1,@StartDate),106)))),0),
			CreditAmount=ISNULL((SELECT SUM(VD.Amount) FROM View_VoucherDetail AS VD WHERE VD.BUID = @BUID AND VD.IsDebit=0 AND VD.AccountHeadID=TT.AccountHeadID AND VD.VoucherID IN(SELECT V.VoucherID FROM Voucher AS V WHERE ISNULL(V.AuthorizedBy,0)!=0 AND CONVERT(DATE,CONVERT(VARCHAR(12),V.VoucherDate,106)) BETWEEN CONVERT(DATE,CONVERT(VARCHAR(12),@SessionStartDate,106)) AND CONVERT(DATE,CONVERT(VARCHAR(12), DATEADD(DAY,-1,@StartDate),106)))),0)
		FROM #TempTable2 AS TT 

		UPDATE #TempTable2 SET ClosingValue=TT.OpeiningValue+TT.DebitAmount-TT.CreditAmount FROM #TempTable2 AS TT WHERE TT.ComponentType IN (2,6)
		UPDATE #TempTable2 SET ClosingValue=TT.OpeiningValue-TT.DebitAmount+TT.CreditAmount FROM #TempTable2 AS TT WHERE TT.ComponentType NOT IN (2,6)

		IF(@ApprivedOnly=1)
		BEGIN
			UPDATE #TempTable
			SET ComponentType=dbo.GetComponentID(TT.AccountHeadID),					
				OpenningBalance=ISNULL((SELECT BB.ClosingValue FROM #TempTable2 AS BB WHERE BB.AccountHeadID=TT.AccountHeadID),0),
				DebitAmount=ISNULL((SELECT SUM(VD.Amount) FROM View_VoucherDetail AS VD WHERE VD.BUID = @BUID AND VD.IsDebit=1 AND VD.AccountHeadID=TT.AccountHeadID AND VD.VoucherID IN(SELECT V.VoucherID FROM Voucher AS V WHERE ISNULL(V.AuthorizedBy,0)!=0 AND CONVERT(DATE,CONVERT(VARCHAR(12),V.VoucherDate,106)) BETWEEN CONVERT(DATE,CONVERT(VARCHAR(12),@StartDate,106)) AND CONVERT(DATE,CONVERT(VARCHAR(12), @EndDate,106)))),0),
				CreditAmount=ISNULL((SELECT SUM(VD.Amount) FROM View_VoucherDetail AS VD WHERE VD.BUID = @BUID AND VD.IsDebit=0 AND VD.AccountHeadID=TT.AccountHeadID AND VD.VoucherID IN(SELECT V.VoucherID FROM Voucher AS V WHERE ISNULL(V.AuthorizedBy,0)!=0 AND CONVERT(DATE,CONVERT(VARCHAR(12),V.VoucherDate,106)) BETWEEN CONVERT(DATE,CONVERT(VARCHAR(12),@StartDate,106)) AND CONVERT(DATE,CONVERT(VARCHAR(12), @EndDate,106)))),0)
			FROM #TempTable AS TT WHERE TT.AccountType=5
		END
		ELSE
		BEGIN
			UPDATE #TempTable
			SET ComponentType=dbo.GetComponentID(TT.AccountHeadID),					
				OpenningBalance=ISNULL((SELECT BB.ClosingValue FROM #TempTable2 AS BB WHERE BB.AccountHeadID=TT.AccountHeadID),0),
				DebitAmount=ISNULL((SELECT SUM(VD.Amount) FROM View_VoucherDetail AS VD WHERE VD.BUID = @BUID AND VD.IsDebit=1 AND VD.AccountHeadID=TT.AccountHeadID AND VD.VoucherID IN(SELECT V.VoucherID FROM Voucher AS V WHERE CONVERT(DATE,CONVERT(VARCHAR(12),V.VoucherDate,106)) BETWEEN CONVERT(DATE,CONVERT(VARCHAR(12),@StartDate,106)) AND CONVERT(DATE,CONVERT(VARCHAR(12), @EndDate,106)))),0),
				CreditAmount=ISNULL((SELECT SUM(VD.Amount) FROM View_VoucherDetail AS VD WHERE VD.BUID = @BUID AND VD.IsDebit=0 AND VD.AccountHeadID=TT.AccountHeadID AND VD.VoucherID IN(SELECT V.VoucherID FROM Voucher AS V WHERE CONVERT(DATE,CONVERT(VARCHAR(12),V.VoucherDate,106)) BETWEEN CONVERT(DATE,CONVERT(VARCHAR(12),@StartDate,106)) AND CONVERT(DATE,CONVERT(VARCHAR(12), @EndDate,106)))),0)
			FROM #TempTable AS TT WHERE TT.AccountType=5
		END
	
		UPDATE #TempTable SET ClosingBalance=TT.OpenningBalance+TT.DebitAmount-TT.CreditAmount FROM #TempTable AS TT WHERE TT.ComponentType IN (2,6)
		UPDATE #TempTable SET ClosingBalance=TT.OpenningBalance-TT.DebitAmount+TT.CreditAmount FROM #TempTable AS TT WHERE TT.ComponentType NOT IN (2,6)
	END
END
ELSE
BEGIN
	IF(@BUID=0)
	BEGIN
		UPDATE #TempTable2
		SET	ComponentType=0,
			OpeiningValue=ISNULL((SELECT TOP 1 HH.OpenningBalance FROM AccountOpenningBreakdown AS HH WHERE HH.BreakdownType=1 AND HH.BreakdownObjID=TT.AccountHeadID AND HH.AccountingSessionID=@SessionID),0),
			DebitAmount=ISNULL((SELECT SUM(CCT.Amount * CCT.CurrencyConversionRate) FROM View_CostCenterTransaction AS CCT WHERE CCT.IsDr=1 AND CCT.CCID=TT.AccountHeadID AND CCT.VoucherID IN(SELECT V.VoucherID FROM Voucher AS V WHERE ISNULL(V.AuthorizedBy,0)!=0 AND CONVERT(DATE,CONVERT(VARCHAR(12),V.VoucherDate,106)) BETWEEN CONVERT(DATE,CONVERT(VARCHAR(12),@SessionStartDate,106)) AND CONVERT(DATE,CONVERT(VARCHAR(12), DATEADD(DAY,-1,@StartDate),106)))),0),
			CreditAmount=ISNULL((SELECT SUM(CCT.Amount * CCT.CurrencyConversionRate) FROM View_CostCenterTransaction AS CCT WHERE CCT.IsDr=0 AND CCT.CCID=TT.AccountHeadID AND CCT.VoucherID IN(SELECT V.VoucherID FROM Voucher AS V WHERE ISNULL(V.AuthorizedBy,0)!=0 AND CONVERT(DATE,CONVERT(VARCHAR(12),V.VoucherDate,106)) BETWEEN CONVERT(DATE,CONVERT(VARCHAR(12),@SessionStartDate,106)) AND CONVERT(DATE,CONVERT(VARCHAR(12), DATEADD(DAY,-1,@StartDate),106)))),0)
		FROM #TempTable2 AS TT 

		UPDATE #TempTable2 SET ClosingValue=TT.OpeiningValue+TT.DebitAmount-TT.CreditAmount FROM #TempTable2 AS TT WHERE TT.ComponentType IN (2,6)
		UPDATE #TempTable2 SET ClosingValue=TT.OpeiningValue-TT.DebitAmount+TT.CreditAmount FROM #TempTable2 AS TT WHERE TT.ComponentType NOT IN (2,6)

		IF(@ApprivedOnly=1)
		BEGIN
			UPDATE #TempTable
			SET ComponentType=0,					
				OpenningBalance=ISNULL((SELECT BB.ClosingValue FROM #TempTable2 AS BB WHERE BB.AccountHeadID=TT.AccountHeadID),0),
				DebitAmount=ISNULL((SELECT SUM(CCT.Amount * CCT.CurrencyConversionRate) FROM View_CostCenterTransaction AS CCT WHERE CCT.IsDr=1 AND CCT.CCID=TT.AccountHeadID AND CCT.VoucherID IN(SELECT V.VoucherID FROM Voucher AS V WHERE ISNULL(V.AuthorizedBy,0)!=0 AND CONVERT(DATE,CONVERT(VARCHAR(12),V.VoucherDate,106)) BETWEEN CONVERT(DATE,CONVERT(VARCHAR(12),@StartDate,106)) AND CONVERT(DATE,CONVERT(VARCHAR(12), @EndDate,106)))),0),
				CreditAmount=ISNULL((SELECT SUM(CCT.Amount * CCT.CurrencyConversionRate) FROM View_CostCenterTransaction AS CCT WHERE CCT.IsDr=0 AND CCT.CCID=TT.AccountHeadID AND CCT.VoucherID IN(SELECT V.VoucherID FROM Voucher AS V WHERE ISNULL(V.AuthorizedBy,0)!=0 AND CONVERT(DATE,CONVERT(VARCHAR(12),V.VoucherDate,106)) BETWEEN CONVERT(DATE,CONVERT(VARCHAR(12),@StartDate,106)) AND CONVERT(DATE,CONVERT(VARCHAR(12), @EndDate,106)))),0)
			FROM #TempTable AS TT
		END
		ELSE
		BEGIN
			UPDATE #TempTable
			SET ComponentType=0,					
				OpenningBalance=ISNULL((SELECT BB.ClosingValue FROM #TempTable2 AS BB WHERE BB.AccountHeadID=TT.AccountHeadID),0),
				DebitAmount=ISNULL((SELECT SUM(CCT.Amount * CCT.CurrencyConversionRate) FROM View_CostCenterTransaction AS CCT WHERE CCT.IsDr=1 AND CCT.CCID=TT.AccountHeadID AND CCT.VoucherID IN(SELECT V.VoucherID FROM Voucher AS V WHERE CONVERT(DATE,CONVERT(VARCHAR(12),V.VoucherDate,106)) BETWEEN CONVERT(DATE,CONVERT(VARCHAR(12),@StartDate,106)) AND CONVERT(DATE,CONVERT(VARCHAR(12), @EndDate,106)))),0),
				CreditAmount=ISNULL((SELECT SUM(CCT.Amount * CCT.CurrencyConversionRate) FROM View_CostCenterTransaction AS CCT WHERE CCT.IsDr=0 AND CCT.CCID=TT.AccountHeadID AND CCT.VoucherID IN(SELECT V.VoucherID FROM Voucher AS V WHERE CONVERT(DATE,CONVERT(VARCHAR(12),V.VoucherDate,106)) BETWEEN CONVERT(DATE,CONVERT(VARCHAR(12),@StartDate,106)) AND CONVERT(DATE,CONVERT(VARCHAR(12), @EndDate,106)))),0)
			FROM #TempTable AS TT
		END
	
		UPDATE #TempTable SET ClosingBalance=TT.OpenningBalance+TT.DebitAmount-TT.CreditAmount FROM #TempTable AS TT WHERE TT.ComponentType IN (2,6)
		UPDATE #TempTable SET ClosingBalance=TT.OpenningBalance-TT.DebitAmount+TT.CreditAmount FROM #TempTable AS TT WHERE TT.ComponentType NOT IN (2,6)
	END
	ELSE
	BEGIN
		UPDATE #TempTable2
		SET	ComponentType=0,
			OpeiningValue=ISNULL((SELECT TOP 1 HH.OpenningBalance FROM AccountOpenningBreakdown AS HH WHERE HH.BusinessUnitID=@BUID AND HH.BreakdownType=1 AND HH.BreakdownObjID=TT.AccountHeadID AND HH.AccountingSessionID=@SessionID),0),
			DebitAmount=ISNULL((SELECT SUM(CCT.Amount * CCT.CurrencyConversionRate) FROM View_CostCenterTransaction AS CCT WHERE CCT.BUID=@BUID AND CCT.IsDr=1 AND CCT.CCID=TT.AccountHeadID AND CCT.VoucherID IN(SELECT V.VoucherID FROM Voucher AS V WHERE ISNULL(V.AuthorizedBy,0)!=0 AND CONVERT(DATE,CONVERT(VARCHAR(12),V.VoucherDate,106)) BETWEEN CONVERT(DATE,CONVERT(VARCHAR(12),@SessionStartDate,106)) AND CONVERT(DATE,CONVERT(VARCHAR(12), DATEADD(DAY,-1,@StartDate),106)))),0),
			CreditAmount=ISNULL((SELECT SUM(CCT.Amount * CCT.CurrencyConversionRate) FROM View_CostCenterTransaction AS CCT WHERE CCT.BUID=@BUID AND CCT.IsDr=0 AND CCT.CCID=TT.AccountHeadID AND CCT.VoucherID IN(SELECT V.VoucherID FROM Voucher AS V WHERE ISNULL(V.AuthorizedBy,0)!=0 AND CONVERT(DATE,CONVERT(VARCHAR(12),V.VoucherDate,106)) BETWEEN CONVERT(DATE,CONVERT(VARCHAR(12),@SessionStartDate,106)) AND CONVERT(DATE,CONVERT(VARCHAR(12), DATEADD(DAY,-1,@StartDate),106)))),0)
		FROM #TempTable2 AS TT 

		UPDATE #TempTable2 SET ClosingValue=TT.OpeiningValue+TT.DebitAmount-TT.CreditAmount FROM #TempTable2 AS TT WHERE TT.ComponentType IN (2,6)
		UPDATE #TempTable2 SET ClosingValue=TT.OpeiningValue-TT.DebitAmount+TT.CreditAmount FROM #TempTable2 AS TT WHERE TT.ComponentType NOT IN (2,6)

		IF(@ApprivedOnly=1)
		BEGIN
			UPDATE #TempTable
			SET ComponentType=0,					
				OpenningBalance=ISNULL((SELECT BB.ClosingValue FROM #TempTable2 AS BB WHERE BB.AccountHeadID=TT.AccountHeadID),0),
				DebitAmount=ISNULL((SELECT SUM(CCT.Amount * CCT.CurrencyConversionRate) FROM View_CostCenterTransaction AS CCT WHERE CCT.BUID=@BUID AND CCT.IsDr=1 AND CCT.CCID=TT.AccountHeadID AND CCT.VoucherID IN(SELECT V.VoucherID FROM Voucher AS V WHERE ISNULL(V.AuthorizedBy,0)!=0 AND CONVERT(DATE,CONVERT(VARCHAR(12),V.VoucherDate,106)) BETWEEN CONVERT(DATE,CONVERT(VARCHAR(12),@StartDate,106)) AND CONVERT(DATE,CONVERT(VARCHAR(12), @EndDate,106)))),0),
				CreditAmount=ISNULL((SELECT SUM(CCT.Amount * CCT.CurrencyConversionRate) FROM View_CostCenterTransaction AS CCT WHERE CCT.BUID=@BUID AND CCT.IsDr=0 AND CCT.CCID=TT.AccountHeadID AND CCT.VoucherID IN(SELECT V.VoucherID FROM Voucher AS V WHERE ISNULL(V.AuthorizedBy,0)!=0 AND CONVERT(DATE,CONVERT(VARCHAR(12),V.VoucherDate,106)) BETWEEN CONVERT(DATE,CONVERT(VARCHAR(12),@StartDate,106)) AND CONVERT(DATE,CONVERT(VARCHAR(12), @EndDate,106)))),0)
			FROM #TempTable AS TT
		END
		ELSE
		BEGIN
			UPDATE #TempTable
			SET ComponentType=0,					
				OpenningBalance=ISNULL((SELECT BB.ClosingValue FROM #TempTable2 AS BB WHERE BB.AccountHeadID=TT.AccountHeadID),0),
				DebitAmount=ISNULL((SELECT SUM(CCT.Amount * CCT.CurrencyConversionRate) FROM View_CostCenterTransaction AS CCT WHERE CCT.BUID=@BUID AND CCT.IsDr=1 AND CCT.CCID=TT.AccountHeadID AND CCT.VoucherID IN(SELECT V.VoucherID FROM Voucher AS V WHERE CONVERT(DATE,CONVERT(VARCHAR(12),V.VoucherDate,106)) BETWEEN CONVERT(DATE,CONVERT(VARCHAR(12),@StartDate,106)) AND CONVERT(DATE,CONVERT(VARCHAR(12), @EndDate,106)))),0),
				CreditAmount=ISNULL((SELECT SUM(CCT.Amount * CCT.CurrencyConversionRate) FROM View_CostCenterTransaction AS CCT WHERE CCT.BUID=@BUID AND CCT.IsDr=0 AND CCT.CCID=TT.AccountHeadID AND CCT.VoucherID IN(SELECT V.VoucherID FROM Voucher AS V WHERE CONVERT(DATE,CONVERT(VARCHAR(12),V.VoucherDate,106)) BETWEEN CONVERT(DATE,CONVERT(VARCHAR(12),@StartDate,106)) AND CONVERT(DATE,CONVERT(VARCHAR(12), @EndDate,106)))),0)
			FROM #TempTable AS TT
		END
	
		UPDATE #TempTable SET ClosingBalance=TT.OpenningBalance+TT.DebitAmount-TT.CreditAmount FROM #TempTable AS TT WHERE TT.ComponentType IN (2,6)
		UPDATE #TempTable SET ClosingBalance=TT.OpenningBalance-TT.DebitAmount+TT.CreditAmount FROM #TempTable AS TT WHERE TT.ComponentType NOT IN (2,6)
	END
END


SELECT * FROM #TempTable
DROP TABLE #TempTable
DROP TABLE #TempTable2
COMMIT TRAN




GO
/****** Object:  StoredProcedure [dbo].[SP_IUD_AccountsBookSetup]    Script Date: 1/12/2017 10:23:08 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[SP_IUD_AccountsBookSetup]
(
	@AccountsBookSetupID as	int, 
	@AccountsBookSetupName as	varchar(512),	
	@MappingType as smallint, 
	@Note as	varchar(512),	
	@DBUserID as	int, 
	@DBOperation as smallint	 
	-- %n, %s, %n, %s, %n, %n
)
	
AS
BEGIN TRAN
DECLARE 
@DBServerDateTime as datetime
SET @DBServerDateTime=Getdate()
IF(@DBOperation=1)
BEGIN				
	IF EXISTS (SELECT * FROM AccountsBookSetup WHERE AccountsBookSetupName=@AccountsBookSetupName)
	BEGIN
		ROLLBACK
			RAISERROR (N'Account Book Setup with this name already exist.!!~',16,1);	
		RETURN
	END	
	SET @AccountsBookSetupID=(SELECT ISNULL(MAX(AccountsBookSetupID),0)+1 FROM AccountsBookSetup)		
	SET @DBServerDateTime=getdate()
	INSERT INTO AccountsBookSetup	(AccountsBookSetupID,		AccountsBookSetupName,		MappingType,	Note,	DBUserID,		DBServerDateTime)
    					    VALUES	(@AccountsBookSetupID,		@AccountsBookSetupName,		@MappingType,	@Note,	@DBUserID,		@DBServerDateTime)
    SELECT * FROM AccountsBookSetup WHERE AccountsBookSetupID= @AccountsBookSetupID
END

IF(@DBOperation=2)
BEGIN	
	IF EXISTS (SELECT * FROM AccountsBookSetup WHERE AccountsBookSetupName=@AccountsBookSetupName AND AccountsBookSetupID!=@AccountsBookSetupID)
	BEGIN
		ROLLBACK
			RAISERROR (N'Account Book Setup with this name already exist.!!~',16,1);	
		RETURN
	END	
	UPDATE AccountsBookSetup  SET AccountsBookSetupName=@AccountsBookSetupName,	MappingType=@MappingType,	Note = @Note, DBUserID=@DBUserID,DBServerDateTime=@DBServerDateTime  WHERE AccountsBookSetupID= @AccountsBookSetupID
	SELECT * FROM AccountsBookSetup WHERE AccountsBookSetupID= @AccountsBookSetupID
END

IF(@DBOperation=3)
BEGIN	
	IF(ISNULL(@AccountsBookSetupID,0)<=0)
	BEGIN
		ROLLBACK
			RAISERROR (N'Invalid Accounts Book Setup.!!~',16,1);	
		RETURN
	END
	DELETE FROM AccountsBookSetupDetail WHERE AccountsBookSetupID = @AccountsBookSetupID
	DELETE FROM AccountsBookSetup WHERE AccountsBookSetupID = @AccountsBookSetupID	
END
COMMIT TRAN




GO
/****** Object:  StoredProcedure [dbo].[SP_IUD_AccountsBookSetupDetail]    Script Date: 1/12/2017 10:23:08 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[SP_IUD_AccountsBookSetupDetail]
(
	@AccountsBookSetupDetailID as	int, 
	@AccountsBookSetupID as	int, 
	@AccountHeadID as	varchar(512),	 
	@DBUserID as	int, 
	@DBOperation as smallint	 
	-- %n, %n, %n, %n, %n

)
	
AS
BEGIN TRAN
DECLARE 
@DBServerDateTime as datetime
SET @DBServerDateTime=Getdate()
IF(@DBOperation=1)
BEGIN			
		
		SET @AccountsBookSetupDetailID=(SELECT ISNULL(MAX(AccountsBookSetupDetailID),0)+1 FROM AccountsBookSetupDetail)		
		SET @DBServerDateTime=getdate()
		INSERT INTO AccountsBookSetupDetail	(AccountsBookSetupDetailID,				AccountsBookSetupID,		AccountHeadID,			DBUserID,				DBServerDateTime)	
    								VALUES	(@AccountsBookSetupDetailID,			@AccountsBookSetupID,		@AccountHeadID,			@DBUserID,				@DBServerDateTime)
    	SELECT * FROM View_AccountsBookSetupDetail WHERE AccountsBookSetupDetailID = @AccountsBookSetupDetailID
END

IF(@DBOperation=2)
BEGIN
	
	Update AccountsBookSetupDetail  SET AccountsBookSetupID=@AccountsBookSetupID,
									 AccountHeadID=@AccountHeadID,
									 DBUserID=@DBUserID,
									 DBServerDateTime=@DBServerDateTime  WHERE AccountsBookSetupDetailID= @AccountsBookSetupDetailID
	 	SELECT * FROM View_AccountsBookSetupDetail WHERE AccountsBookSetupDetailID= @AccountsBookSetupDetailID
END

IF(@DBOperation=3)
BEGIN
	DELETE FROM AccountsBookSetupDetail WHERE AccountsBookSetupID= @AccountsBookSetupID
END
COMMIT TRAN



GO
/****** Object:  StoredProcedure [dbo].[SP_IUD_VoucherBill]    Script Date: 1/12/2017 10:23:08 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[SP_IUD_VoucherBill] 
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
		--IF EXISTS (SELECT * FROM VoucherBill AS HH WHERE HH.BillNo=@BillNo AND  HH.AccountHeadID=@AccountHeadID AND HH.SubLedgerID=@SubLedgerID)
		--BEGIN
		--	ROLLBACK
		--		RAISERROR (N'This Bill No alreay exists for this .!!~',16,1);	
		--	RETURN
		--END		
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
	IF (@BillNo is null OR @BillNo='')
	BEGIN
		ROLLBACK
			RAISERROR (N'Please, Entry Bill BillNo/No.!!~',16,1);	
		RETURN
	END
	--IF EXISTS (SELECT * FROM VoucherBill AS HH WHERE HH.BillNo=@BillNo AND  HH.AccountHeadID=@AccountHeadID AND HH.SubLedgerID=@SubLedgerID AND HH.VoucherBillID<>@VoucherBillID)
	--BEGIN
	--	ROLLBACK
	--		RAISERROR (N'This Bill No alreay exists for this .!!~',16,1);	
	--	RETURN
	--END

	--IF EXISTS (SELECT * FROM VoucherBillTransaction AS TT WHERE  TT.VoucherBillID=@VoucherBillID)
	--BEGIN
	--	ROLLBACK
	--		RAISERROR (N'Edition Not Possible VoucherBillTransaction Reference Exists.!!~',16,1);	
	--	RETURN
	--END	
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




GO
/****** Object:  Table [dbo].[AccountsBookSetup]    Script Date: 1/12/2017 10:23:08 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[AccountsBookSetup](
	[AccountsBookSetupID] [int] NOT NULL,
	[AccountsBookSetupName] [varchar](512) NULL,
	[MappingType] [smallint] NULL,
	[Note] [varchar](512) NULL,
	[DBUserID] [int] NULL,
	[DBServerDateTime] [datetime] NULL,
 CONSTRAINT [PK_AccountsBookSetup] PRIMARY KEY CLUSTERED 
(
	[AccountsBookSetupID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[AccountsBookSetupDetail]    Script Date: 1/12/2017 10:23:08 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[AccountsBookSetupDetail](
	[AccountsBookSetupDetailID] [int] NOT NULL,
	[AccountsBookSetupID] [int] NULL,
	[AccountHeadID] [int] NULL,
	[DBUserID] [int] NULL,
	[DBServerDateTime] [datetime] NULL,
 CONSTRAINT [PK_AccountsBookSetupDetail] PRIMARY KEY CLUSTERED 
(
	[AccountsBookSetupDetailID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  View [dbo].[View_AccountsBookSetupDetail]    Script Date: 1/12/2017 10:23:08 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE VIEW [dbo].[View_AccountsBookSetupDetail]
AS
SELECT	ABSD.AccountsBookSetupDetailID,
		ABSD.AccountsBookSetupID,
		ABSD.AccountHeadID,
		CASE WHEN ACCBS.MappingType=1 THEN (SELECT HH.AccountHeadName FROM ChartsOfAccount AS HH WHERE HH.AccountHeadID = ABSD.AccountHeadID) 
			 WHEN ACCBS.MappingType=2 THEN (SELECT HH.Name FROM ACCostCenter AS HH WHERE HH.ACCostCenterID = ABSD.AccountHeadID)	
		END AS AccountHeadName,
		CASE WHEN ACCBS.MappingType=1 THEN (SELECT HH.AccountCode FROM ChartsOfAccount AS HH WHERE HH.AccountHeadID = ABSD.AccountHeadID) 
			 WHEN ACCBS.MappingType=2 THEN (SELECT HH.Code FROM ACCostCenter AS HH WHERE HH.ACCostCenterID = ABSD.AccountHeadID)	
		END AS AccountHeadCode,
		CASE WHEN ACCBS.MappingType=1 THEN (SELECT HH.ParentHeadName FROM View_ChartsOfAccount AS HH WHERE HH.AccountHeadID = ABSD.AccountHeadID) 
			 WHEN ACCBS.MappingType=2 THEN (SELECT HH.CategoryName FROM View_ACCostCenter AS HH WHERE HH.ACCostCenterID = ABSD.AccountHeadID)	
		END AS CategoryName

FROM AccountsBookSetupDetail AS ABSD
INNER JOIN AccountsBookSetup AS ACCBS ON ABSD.AccountsBookSetupID = ACCBS.AccountsBookSetupID






GO
