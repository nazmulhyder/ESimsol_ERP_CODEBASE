IF NOT EXISTS (SELECT * FROM sys.columns where Name = N'ComponentType' and Object_ID = Object_ID(N'AccountsBookSetupDetail'))
BEGIN
   ALTER TABLE AccountsBookSetupDetail
   ADD ComponentType smallint
END
GO
/****** Object:  View [dbo].[View_AccountsBookSetupDetail]    Script Date: 1/12/2017 6:11:50 PM ******/
DROP VIEW [dbo].[View_AccountsBookSetupDetail]
GO
/****** Object:  StoredProcedure [dbo].[SP_IUD_AccountsBookSetupDetail]    Script Date: 1/12/2017 6:11:50 PM ******/
DROP PROCEDURE [dbo].[SP_IUD_AccountsBookSetupDetail]
GO
/****** Object:  StoredProcedure [dbo].[SP_AccountsBook]    Script Date: 1/12/2017 6:11:50 PM ******/
DROP PROCEDURE [dbo].[SP_AccountsBook]
GO
/****** Object:  StoredProcedure [dbo].[SP_AccountsBook]    Script Date: 1/12/2017 6:11:50 PM ******/
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
		SET	ComponentType=(SELECT TOP 1 HH.ComponentType FROM AccountsBookSetupDetail AS HH WHERE HH.AccountsBookSetupID=@AccountsBookSetUpID AND HH.AccountHeadID=TT.AccountHeadID),
			OpeiningValue=ISNULL((SELECT TOP 1 HH.OpenningBalance FROM AccountOpenningBreakdown AS HH WHERE HH.BreakdownType=1 AND HH.BreakdownObjID=TT.AccountHeadID AND HH.AccountingSessionID=@SessionID),0),
			DebitAmount=ISNULL((SELECT SUM(CCT.Amount * CCT.CurrencyConversionRate) FROM View_CostCenterTransaction AS CCT WHERE CCT.IsDr=1 AND CCT.CCID=TT.AccountHeadID AND CCT.VoucherID IN(SELECT V.VoucherID FROM Voucher AS V WHERE ISNULL(V.AuthorizedBy,0)!=0 AND CONVERT(DATE,CONVERT(VARCHAR(12),V.VoucherDate,106)) BETWEEN CONVERT(DATE,CONVERT(VARCHAR(12),@SessionStartDate,106)) AND CONVERT(DATE,CONVERT(VARCHAR(12), DATEADD(DAY,-1,@StartDate),106)))),0),
			CreditAmount=ISNULL((SELECT SUM(CCT.Amount * CCT.CurrencyConversionRate) FROM View_CostCenterTransaction AS CCT WHERE CCT.IsDr=0 AND CCT.CCID=TT.AccountHeadID AND CCT.VoucherID IN(SELECT V.VoucherID FROM Voucher AS V WHERE ISNULL(V.AuthorizedBy,0)!=0 AND CONVERT(DATE,CONVERT(VARCHAR(12),V.VoucherDate,106)) BETWEEN CONVERT(DATE,CONVERT(VARCHAR(12),@SessionStartDate,106)) AND CONVERT(DATE,CONVERT(VARCHAR(12), DATEADD(DAY,-1,@StartDate),106)))),0)
		FROM #TempTable2 AS TT 

		UPDATE #TempTable2 SET ClosingValue=TT.OpeiningValue+TT.DebitAmount-TT.CreditAmount FROM #TempTable2 AS TT WHERE TT.ComponentType IN (2,6)
		UPDATE #TempTable2 SET ClosingValue=TT.OpeiningValue-TT.DebitAmount+TT.CreditAmount FROM #TempTable2 AS TT WHERE TT.ComponentType NOT IN (2,6)

		IF(@ApprivedOnly=1)
		BEGIN
			UPDATE #TempTable
			SET ComponentType=(SELECT TOP 1 HH.ComponentType FROM AccountsBookSetupDetail AS HH WHERE HH.AccountsBookSetupID=@AccountsBookSetUpID AND HH.AccountHeadID=TT.AccountHeadID),
				OpenningBalance=ISNULL((SELECT BB.ClosingValue FROM #TempTable2 AS BB WHERE BB.AccountHeadID=TT.AccountHeadID),0),
				DebitAmount=ISNULL((SELECT SUM(CCT.Amount * CCT.CurrencyConversionRate) FROM View_CostCenterTransaction AS CCT WHERE CCT.IsDr=1 AND CCT.CCID=TT.AccountHeadID AND CCT.VoucherID IN(SELECT V.VoucherID FROM Voucher AS V WHERE ISNULL(V.AuthorizedBy,0)!=0 AND CONVERT(DATE,CONVERT(VARCHAR(12),V.VoucherDate,106)) BETWEEN CONVERT(DATE,CONVERT(VARCHAR(12),@StartDate,106)) AND CONVERT(DATE,CONVERT(VARCHAR(12), @EndDate,106)))),0),
				CreditAmount=ISNULL((SELECT SUM(CCT.Amount * CCT.CurrencyConversionRate) FROM View_CostCenterTransaction AS CCT WHERE CCT.IsDr=0 AND CCT.CCID=TT.AccountHeadID AND CCT.VoucherID IN(SELECT V.VoucherID FROM Voucher AS V WHERE ISNULL(V.AuthorizedBy,0)!=0 AND CONVERT(DATE,CONVERT(VARCHAR(12),V.VoucherDate,106)) BETWEEN CONVERT(DATE,CONVERT(VARCHAR(12),@StartDate,106)) AND CONVERT(DATE,CONVERT(VARCHAR(12), @EndDate,106)))),0)
			FROM #TempTable AS TT
		END
		ELSE
		BEGIN
			UPDATE #TempTable
			SET ComponentType=(SELECT TOP 1 HH.ComponentType FROM AccountsBookSetupDetail AS HH WHERE HH.AccountsBookSetupID=@AccountsBookSetUpID AND HH.AccountHeadID=TT.AccountHeadID),
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
		SET	ComponentType=(SELECT TOP 1 HH.ComponentType FROM AccountsBookSetupDetail AS HH WHERE HH.AccountsBookSetupID=@AccountsBookSetUpID AND HH.AccountHeadID=TT.AccountHeadID),
			OpeiningValue=ISNULL((SELECT TOP 1 HH.OpenningBalance FROM AccountOpenningBreakdown AS HH WHERE HH.BusinessUnitID=@BUID AND HH.BreakdownType=1 AND HH.BreakdownObjID=TT.AccountHeadID AND HH.AccountingSessionID=@SessionID),0),
			DebitAmount=ISNULL((SELECT SUM(CCT.Amount * CCT.CurrencyConversionRate) FROM View_CostCenterTransaction AS CCT WHERE CCT.BUID=@BUID AND CCT.IsDr=1 AND CCT.CCID=TT.AccountHeadID AND CCT.VoucherID IN(SELECT V.VoucherID FROM Voucher AS V WHERE ISNULL(V.AuthorizedBy,0)!=0 AND CONVERT(DATE,CONVERT(VARCHAR(12),V.VoucherDate,106)) BETWEEN CONVERT(DATE,CONVERT(VARCHAR(12),@SessionStartDate,106)) AND CONVERT(DATE,CONVERT(VARCHAR(12), DATEADD(DAY,-1,@StartDate),106)))),0),
			CreditAmount=ISNULL((SELECT SUM(CCT.Amount * CCT.CurrencyConversionRate) FROM View_CostCenterTransaction AS CCT WHERE CCT.BUID=@BUID AND CCT.IsDr=0 AND CCT.CCID=TT.AccountHeadID AND CCT.VoucherID IN(SELECT V.VoucherID FROM Voucher AS V WHERE ISNULL(V.AuthorizedBy,0)!=0 AND CONVERT(DATE,CONVERT(VARCHAR(12),V.VoucherDate,106)) BETWEEN CONVERT(DATE,CONVERT(VARCHAR(12),@SessionStartDate,106)) AND CONVERT(DATE,CONVERT(VARCHAR(12), DATEADD(DAY,-1,@StartDate),106)))),0)
		FROM #TempTable2 AS TT 

		UPDATE #TempTable2 SET ClosingValue=TT.OpeiningValue+TT.DebitAmount-TT.CreditAmount FROM #TempTable2 AS TT WHERE TT.ComponentType IN (2,6)
		UPDATE #TempTable2 SET ClosingValue=TT.OpeiningValue-TT.DebitAmount+TT.CreditAmount FROM #TempTable2 AS TT WHERE TT.ComponentType NOT IN (2,6)

		IF(@ApprivedOnly=1)
		BEGIN
			UPDATE #TempTable
			SET ComponentType=(SELECT TOP 1 HH.ComponentType FROM AccountsBookSetupDetail AS HH WHERE HH.AccountsBookSetupID=@AccountsBookSetUpID AND HH.AccountHeadID=TT.AccountHeadID),
				OpenningBalance=ISNULL((SELECT BB.ClosingValue FROM #TempTable2 AS BB WHERE BB.AccountHeadID=TT.AccountHeadID),0),
				DebitAmount=ISNULL((SELECT SUM(CCT.Amount * CCT.CurrencyConversionRate) FROM View_CostCenterTransaction AS CCT WHERE CCT.BUID=@BUID AND CCT.IsDr=1 AND CCT.CCID=TT.AccountHeadID AND CCT.VoucherID IN(SELECT V.VoucherID FROM Voucher AS V WHERE ISNULL(V.AuthorizedBy,0)!=0 AND CONVERT(DATE,CONVERT(VARCHAR(12),V.VoucherDate,106)) BETWEEN CONVERT(DATE,CONVERT(VARCHAR(12),@StartDate,106)) AND CONVERT(DATE,CONVERT(VARCHAR(12), @EndDate,106)))),0),
				CreditAmount=ISNULL((SELECT SUM(CCT.Amount * CCT.CurrencyConversionRate) FROM View_CostCenterTransaction AS CCT WHERE CCT.BUID=@BUID AND CCT.IsDr=0 AND CCT.CCID=TT.AccountHeadID AND CCT.VoucherID IN(SELECT V.VoucherID FROM Voucher AS V WHERE ISNULL(V.AuthorizedBy,0)!=0 AND CONVERT(DATE,CONVERT(VARCHAR(12),V.VoucherDate,106)) BETWEEN CONVERT(DATE,CONVERT(VARCHAR(12),@StartDate,106)) AND CONVERT(DATE,CONVERT(VARCHAR(12), @EndDate,106)))),0)
			FROM #TempTable AS TT
		END
		ELSE
		BEGIN
			UPDATE #TempTable
			SET ComponentType=(SELECT TOP 1 HH.ComponentType FROM AccountsBookSetupDetail AS HH WHERE HH.AccountsBookSetupID=@AccountsBookSetUpID AND HH.AccountHeadID=TT.AccountHeadID),
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
/****** Object:  StoredProcedure [dbo].[SP_IUD_AccountsBookSetupDetail]    Script Date: 1/12/2017 6:11:50 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[SP_IUD_AccountsBookSetupDetail]
(
	@AccountsBookSetupDetailID as	int, 
	@AccountsBookSetupID as	int, 
	@AccountHeadID as	varchar(512),
	@ComponentType as smallint,
	@DBUserID as	int, 
	@DBOperation as smallint	 
	-- %n, %n, %n, %n, %n, %n

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
	INSERT INTO AccountsBookSetupDetail	(AccountsBookSetupDetailID,				AccountsBookSetupID,		AccountHeadID,		ComponentType,		DBUserID,				DBServerDateTime)	
    							VALUES	(@AccountsBookSetupDetailID,			@AccountsBookSetupID,		@AccountHeadID,		@ComponentType,		@DBUserID,				@DBServerDateTime)
    SELECT * FROM View_AccountsBookSetupDetail WHERE AccountsBookSetupDetailID = @AccountsBookSetupDetailID
END

IF(@DBOperation=2)
BEGIN
	IF(ISNULL(@AccountsBookSetupDetailID,0)<=0)
	BEGIN
		ROLLBACK
			RAISERROR (N'Invalid Accounts Book Setup.!!~',16,1);	
		RETURN
	END
	Update AccountsBookSetupDetail  SET AccountsBookSetupID=@AccountsBookSetupID, AccountHeadID=@AccountHeadID, ComponentType=@ComponentType, DBUserID=@DBUserID, DBServerDateTime=@DBServerDateTime  WHERE AccountsBookSetupDetailID= @AccountsBookSetupDetailID
	SELECT * FROM View_AccountsBookSetupDetail WHERE AccountsBookSetupDetailID= @AccountsBookSetupDetailID
END

IF(@DBOperation=3)
BEGIN	
	DELETE FROM AccountsBookSetupDetail WHERE AccountsBookSetupID= @AccountsBookSetupID
END
COMMIT TRAN



GO
/****** Object:  View [dbo].[View_AccountsBookSetupDetail]    Script Date: 1/12/2017 6:11:50 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE VIEW [dbo].[View_AccountsBookSetupDetail]
AS
SELECT	ABSD.AccountsBookSetupDetailID,
		ABSD.AccountsBookSetupID,
		ABSD.AccountHeadID,
		ABSD.ComponentType,
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
