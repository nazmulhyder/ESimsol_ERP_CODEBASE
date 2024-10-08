GO
/****** Object:  View [dbo].[View_ChequeBook]    Script Date: 12/4/2016 3:38:02 PM ******/
DROP VIEW [dbo].[View_ChequeBook]
GO
/****** Object:  View [dbo].[View_BankAccount]    Script Date: 12/4/2016 3:38:02 PM ******/
DROP VIEW [dbo].[View_BankAccount]
GO
/****** Object:  View [dbo].[View_Bank]    Script Date: 12/4/2016 3:38:02 PM ******/
DROP VIEW [dbo].[View_Bank]
GO
/****** Object:  StoredProcedure [dbo].[SP_IUD_ChartOfAccount]    Script Date: 12/4/2016 3:38:02 PM ******/
DROP PROCEDURE [dbo].[SP_IUD_ChartOfAccount]
GO
/****** Object:  StoredProcedure [dbo].[SP_IUD_BankAccount]    Script Date: 12/4/2016 3:38:02 PM ******/
DROP PROCEDURE [dbo].[SP_IUD_BankAccount]
GO
/****** Object:  StoredProcedure [dbo].[SP_IUD_BankAccount]    Script Date: 12/4/2016 3:38:02 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[SP_IUD_BankAccount]
(
	@BankAccountID as int,
	@AccountName as	varchar(64),
	@AccountNo as varchar(64),
	@BankID	as int,
	@BankBranchID as int,
	@AccountType as smallint,
	@LimitAmount as decimal(30,17), 
	@CurrentLimit as decimal(30,17),	
	@BusinessUnitID as int,
	@DBUserID as int,
	@DBOperation as smallint
	--%n, %s, %s, %n, %n, %n, %n, %n, %n, %n, %n
)		
AS
BEGIN TRAN
DECLARE 
@DBServerDateTime as datetime
SET @DBServerDateTime=Getdate()
IF(@DBOperation=1)
BEGIN			
		SET @BankAccountID=(SELECT ISNULL(MAX(BankAccountID),0)+1 FROM BankAccount)		
		SET @DBServerDateTime=getdate()		
		INSERT INTO BankAccount	(BankAccountID,		AccountName,	AccountNo,	BankID,		BankBranchID,	AccountType,	LimitAmount,	CurrentLimit,	BusinessUnitID,		DBServerDateTime,	DBUserID,	LastUpdateBy,	LastUpdateDateTime)
    					VALUES	(@BankAccountID,	@AccountName,	@AccountNo, @BankID,	@BankBranchID,	@AccountType,	@LimitAmount,	@CurrentLimit,	@BusinessUnitID,	@DBServerDateTime,	@DBUserID,	@DBUserID,		@DBServerDateTime)
    	UPDATE BankAccount SET CurrentLimit=@LimitAmount WHERE BankAccountID=@BankAccountID				    				  
    	SELECT * FROM View_BankAccount WHERE BankAccountID=@BankAccountID
END

IF(@DBOperation=2)

BEGIN
	IF(@BankAccountID<=0)
	BEGIN
		ROLLBACK
			RAISERROR (N'Your Selected Bank Account Is Invalid Please Refresh and try again!!~',16,1);	
		RETURN
	END		
	Update BankAccount  SET AccountName=@AccountName,	AccountNo=@AccountNo,	BankID=@BankID, BankBranchID = @BankBranchID,		AccountType=@AccountType,		LimitAmount=@LimitAmount,	CurrentLimit=@CurrentLimit, BusinessUnitID=@BusinessUnitID,  LastUpdateBy=@DBUserID,LastUpdateDateTime=@DBServerDateTime
	WHERE BankAccountID=@BankAccountID
	IF ((SELECT LimitAmount FROM BankAccount WHERE BankAccountID=@BankAccountID)!=(SELECT CurrentLimit FROM BankAccount WHERE BankAccountID=@BankAccountID))
	BEGIN
		UPDATE BankAccount SET CurrentLimit=@LimitAmount WHERE BankAccountID=@BankAccountID
	END
	SELECT * FROM View_BankAccount WHERE BankAccountID=@BankAccountID 
END

IF(@DBOperation=3)
BEGIN
	IF(@BankAccountID<=0)
	BEGIN
		ROLLBACK
			RAISERROR (N'Your Selected Bank Account Is Invalid Please Refresh and try again!!~',16,1);	
		RETURN
	END	
	IF EXISTS(SELECT * FROM ChequeBook WHERE BankAccountID=@BankAccountID)
	BEGIN
		ROLLBACK
			RAISERROR (N' Delation is not possible Cheque Book reference may loss!!!~',16,1);	
		RETURN
	END	
	DELETE FROM BankAccount WHERE BankAccountID=@BankAccountID
END
COMMIT TRAN

















GO
/****** Object:  StoredProcedure [dbo].[SP_IUD_ChartOfAccount]    Script Date: 12/4/2016 3:38:02 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[SP_IUD_ChartOfAccount]
(
	@AccountHeadID as int,
	@DAHCID	as int,
	@AccountCode as	varchar(50),
	@AccountHeadName as	varchar(512),
	@AccountType as smallint,
	@ReferenceObjectID as int,
	@Description as	varchar(512),
	@IsJVNode as bit,
	@IsDynamic	as bit,
	@ParentHeadID as int,
	@ReferenceType as int,
	@AccountOperationType AS smallint,	
	@DBUserID as int,
	@DBOperation as smallint
	--%n, %n, %s, %s, %n, %n, %s, %b, %b, %n, %n, %n, %n, %n
)
AS
BEGIN TRAN
DECLARE 
@DBServerDateTime as datetime,
@CompanyWiseAccountHeadID as int,
@TempAccountHeadID as int,
@COACodeType as int,
@ParentAccountOperationType as int
SET @DBServerDateTime=Getdate()
IF(@DBOperation=1)
BEGIN		
	IF(@ParentHeadID<=0)
	BEGIN
		ROLLBACK
			RAISERROR (N'Your Selected Account Head Is Root Head Please change Node!!~',16,1);	
		RETURN
	END	
	SET @AccountCode=(SELECT dbo.FN_ChartofAccountCode(@ParentHeadID))
	IF(ISNULL(@AccountCode,'') = '' OR @AccountCode='Cross Code Limit')
	BEGIN
		ROLLBACK
			RAISERROR (N'Invalid Account Code! Cross Code Limit!~',16,1);	
		RETURN
	END
	IF EXISTS(SELECT * FROM ChartsOfAccount WHERE AccountCode=@AccountCode)
	BEGIN
		ROLLBACK
			RAISERROR (N'Your entered Account Code Already exists Please try again!!~',16,1);	
		RETURN
	END		
	--IF EXISTS(SELECT * FROM ChartsOfAccount WHERE AccountHeadName=@AccountHeadName)
	--BEGIN
	--	ROLLBACK
	--		RAISERROR (N'Your entered Account Head Name Already exists Please try again!!~',16,1);
	--	RETURN
	--END		
		
	DECLARE 
	@ParentAccountType as int
	SET @ParentAccountType=(SELECT AccountType FROM ChartsOfAccount WHERE AccountHeadID=@ParentHeadID)
	IF(@ParentAccountType=@AccountType)
	BEGIN
		ROLLBACK
			RAISERROR (N'Invalid Operation! Parent Account Type & Child Account Type is Same Try Again!!~',16,1);	
		RETURN
	END		
	IF(@ParentAccountType=5)
	BEGIN
		ROLLBACK
			RAISERROR (N'Invalid Operation! Can not add any account under ledger!!~',16,1);	
		RETURN
	END		

	IF(@ReferenceObjectID>0 AND @ReferenceType>0)
	BEGIN
		IF EXISTS(SELECT * FROM ChartsOfAccount WHERE ReferenceObjectID=@ReferenceObjectID AND ReferenceType=@ReferenceType)
		BEGIN
			ROLLBACK
				RAISERROR (N'Selected reference already apply in another account head!!~',16,1);	
			RETURN
		END	
		SET @AccountType=4
	END	
		
	IF(@AccountType=3)--EnumAccountType { None = 0,Component = 1,Segment =2,Group = 3,SubGroup = 4,Ledger = 5}
	BEGIN
		SET @IsJVNode=1
	END
		
	IF(@AccountType=4)--EnumAccountType { None = 0,Component = 1,Segment =2,Group = 3,SubGroup = 4,Ledger = 5}
	BEGIN
		SET @IsJVNode=1
	END
	--edit by fahim0abir on date 12 nov 2015
	--start
	IF(@AccountType<4)--EnumAccountType { None = 0,Component = 1,Segment =2,Group = 3,SubGroup = 4,Ledger = 5}
	BEGIN
		SET @AccountOperationType=0
	END
	ELSE 
	BEGIN
		IF(@AccountType=5)
		BEGIN
			SET @ParentAccountOperationType=(SELECT COA.AccountOperationType FROM ChartsOfAccount AS COA WHERE COA.AccountHeadID=@ParentHeadID)
			IF(@ParentAccountOperationType=0)
			BEGIN
				SET @AccountOperationType=0
			END
		END
	END
	--end						
	SET @AccountHeadID=(SELECT ISNULL(MAX(AccountHeadID),0)+1 FROM ChartsOfAccount)		
	IF(@AccountHeadID<106)
	BEGIN
		SET @AccountHeadID=106
	END
	SET @DBServerDateTime=getdate()
	INSERT INTO ChartsOfAccount	(AccountHeadID,		DAHCID,		AccountCode,	AccountHeadName,	AccountType,	ReferenceObjectID,ReferenceType,	[Description],	IsJVNode,	IsDynamic,	ParentHeadID,	AccountOperationType,	DBUserID,	DBServerDate)
    					VALUES	(@AccountHeadID,	@DAHCID,	@AccountCode,	@AccountHeadName,	@AccountType,	@ReferenceObjectID,	@ReferenceType,@Description,	@IsJVNode,	@IsDynamic,	@ParentHeadID,	@AccountOperationType,	@DBUserID,	@DBServerDateTime)    				  
    UPDATE ChartsOfAccount SET IsJVNode=0 WHERE AccountHeadID=@ParentHeadID				
    SELECT * FROM View_ChartsOfAccount WHERE AccountHeadID=@AccountHeadID
END

IF(@DBOperation=2)
BEGIN
	IF(@AccountHeadID<=0)
	BEGIN
		ROLLBACK
			RAISERROR (N'Your Selected Account Head Is Invalid Please Refresh and try again!!~',16,1);	
		RETURN
	END
		
	IF(@AccountHeadID<106)
	BEGIN
		ROLLBACK
			RAISERROR (N'Your selected Account head is system generated so edition not possible!!~',16,1);	
		RETURN
	END
	
	IF EXISTS(SELECT * FROM ChartsOfAccount WHERE AccountCode=@AccountCode AND AccountHeadID!=@AccountHeadID)
	BEGIN
		ROLLBACK
			RAISERROR (N'Your entered Account Code Already exists Please try again!!~',16,1);	
		RETURN
	END	
	--IF EXISTS(SELECT * FROM ChartsOfAccount WHERE AccountHeadName=@AccountHeadName AND AccountHeadID!=@AccountHeadID)
	--BEGIN
	--	ROLLBACK
	--		RAISERROR (N'Your entered Account Head Name Already exists Please try again!!~',16,1);	
	--	RETURN
	--END	
	
	IF(@ReferenceObjectID>0 AND @ReferenceType>0)
	BEGIN
		IF EXISTS(SELECT * FROM ChartsOfAccount WHERE ReferenceObjectID=@ReferenceObjectID AND ReferenceType=@ReferenceType AND AccountHeadID!=@AccountHeadID)
		BEGIN
			ROLLBACK
				RAISERROR (N'Selected reference already apply in another account head!!~',16,1);	
			RETURN
		END	
	END	

	--edit by fahim0abir on date 12 nov 2015
	--start
	IF(@AccountType<4)--EnumAccountType { None = 0,Component = 1,Segment =2,Group = 3,SubGroup = 4,Ledger = 5}
	BEGIN
		SET @AccountOperationType=0
	END
	ELSE 
	BEGIN
		IF(@AccountType=4 AND @AccountOperationType=0)
		BEGIN
			Update ChartsOfAccount SET AccountOperationType=@AccountOperationType WHERE ParentHeadID=@AccountHeadID
		END	
		IF(@AccountType=5)
		BEGIN
			SET @ParentAccountOperationType=(SELECT COA.AccountOperationType FROM ChartsOfAccount AS COA WHERE COA.AccountHeadID=@ParentHeadID)
			IF(@ParentAccountOperationType=0)
			BEGIN
				SET @AccountOperationType=0
			END
		END
	END
	--end

	Update ChartsOfAccount  
	SET DAHCID=@DAHCID
	,	AccountCode=@AccountCode
	,	AccountHeadName=@AccountHeadName
	,	AccountType=@AccountType
	,	ReferenceObjectID=@ReferenceObjectID
	,	ReferenceType=@ReferenceType
	,	[Description]=@Description
	,	IsJVNode=@IsJVNode
	,	IsDynamic=@IsDynamic
	,	ParentHeadID=@ParentHeadID
	,	AccountOperationType=@AccountOperationType
	,	DBUserID=@DBUserID
	,	DBServerDate=@DBServerDateTime 
	WHERE AccountHeadID=@AccountHeadID
	SELECT * FROM View_ChartsOfAccount WHERE AccountHeadID=@AccountHeadID
END

IF(@DBOperation=3)
BEGIN
	IF(@AccountHeadID<=0)
	BEGIN
		ROLLBACK
			RAISERROR (N'Your Selected Test Is Invalid Please Refresh and try again!!~',16,1);	
		RETURN
	END
	IF(@AccountHeadID<106)
	BEGIN
		ROLLBACK
			RAISERROR (N'Your selected accout head is system generated so deletion not possible!!~',16,1);	
		RETURN
	END
		
	IF EXISTS(SELECT * FROM AccountOpenning AS TT WHERE TT.AccountHeadID=@AccountHeadID)
	BEGIN
		IF EXISTS(SELECT * FROM AccountOpenning AS TT WHERE TT.AccountHeadID=@AccountHeadID AND ISNULL(TT.OpenningBalance,0)=0)
		BEGIN
			DELETE FROM AccountOpenning WHERE AccountHeadID=@AccountHeadID
		END
		ELSE
		BEGIN
			ROLLBACK
				RAISERROR (N'Your Selected Node Deletion is not possible Account Openning Reference maybe loss Please Please check Node!!~',16,1);	
			RETURN
		END
	END

	IF EXISTS(SELECT * FROM AccountOpenningBreakdown AS TT WHERE TT.AccountHeadID=@AccountHeadID)
	BEGIN
		ROLLBACK
			RAISERROR (N'Your Selected Node Deletion is not possible  Account Openning Breakdown Reference maybe loss Please Please check Node!!~',16,1);	
		RETURN
	END
	IF EXISTS(SELECT * FROM View_VoucherDetail AS TT WHERE TT.AccountHeadID=@AccountHeadID)
	BEGIN
		ROLLBACK
			RAISERROR (N'Voucher Already created by this Account Head. Deletion not possible!!~',16,1);	
		RETURN
	END	
	DELETE FROM BusinessUnitWiseAccountHead WHERE AccountHeadID=@AccountHeadID
	IF NOT EXISTS(SELECT * FROM BusinessUnitWiseAccountHead WHERE AccountHeadID=@AccountHeadID)
	BEGIN
		IF EXISTS(SELECT * FROM ChartsOfAccount WHERE ParentHeadID=@AccountHeadID)
		BEGIN
			ROLLBACK
				RAISERROR (N'Your Selected Node Deletion is not possible BusinessUnit WiseAccountHead Reference maybe loss Please Please check Node!!~',16,1);	
			RETURN
		END
		DELETE FROM ChartsOfAccount WHERE AccountHeadID=@AccountHeadID
	END
END
COMMIT TRAN






GO
/****** Object:  View [dbo].[View_Bank]    Script Date: 12/4/2016 3:38:02 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE VIEW [dbo].[View_Bank]
AS
SELECT	Bank.BankID, 
		Bank.Code, 
		Bank.Name, 
		Bank.ShortName, 
		Bank.IsActive, 
		Bank.ChequeSetupID,
		Bank.FaxNo,
		Bank.DBServerDateTime,		 
		ChequeSetup.ChequeSetupName

FROM		Bank 
LEFT OUTER JOIN	ChequeSetup ON Bank.ChequeSetupID = ChequeSetup.ChequeSetupID














GO
/****** Object:  View [dbo].[View_BankAccount]    Script Date: 12/4/2016 3:38:02 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE VIEW [dbo].[View_BankAccount]
AS
SELECT	BA.BankAccountID,
		BA.AccountName,
		BA.AccountNo,
		BA.BankID,
		BA.BankBranchID,
		BA.AccountType,
		BA.LimitAmount,
		BA.CurrentLimit,
		BA.BusinessUnitID,
		BA.AccountName + ' [' + BA.AccountNo + ']' AS BankAccountName,
		B.Name + ' [' + BB.BranchName + ']' AS BankNameBranch,
		B.Name + ' [' + BA.AccountNo + ']' AS BankNameAccountNo,
		B.Name as BankName,
		B.ShortName as BankShortName,
		BB.BranchName as BranchName,
		BU.Name AS BusinessUnitName ,
		BU.Code AS BusinessUnitCode,
		BU.Name+ ' ['+BU.Code+']' AS BusinessUnitNameCode

FROM         dbo.BankAccount AS BA 
LEFT OUTER JOIN	dbo.BankBranch  AS BB ON BA.BankBranchID = BB.BankBranchID 
LEFT OUTER JOIN	dbo.Bank AS B ON BA.BankID = B.BankID
LEFT OUTER JOIN	dbo.BusinessUnit AS BU ON BA.BusinessUnitID = BU.BusinessUnitID











GO
/****** Object:  View [dbo].[View_ChequeBook]    Script Date: 12/4/2016 3:38:02 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE VIEW [dbo].[View_ChequeBook]
AS
--updated by fahim0abir on 19/04/15
--excluded other views and joins using sub-query
SELECT	CB.ChequeBookID, 
		CB.BankAccountID, 
		CB.BookCodePartOne, 
		CB.BookCodePartTwo, 
		CB.[PageCount], 
		CB.FirstChequeNo, 
		CB.IsActive, 
        CB.ActivteBy, 
		CB.ActivateTime, 
		CB.Note, 
		CB.DBServerDateTime,		 
		BA.BankBranchID, 
		BA.BankID, 
		BA.BusinessUnitID, 
		BA.AccountNo, 
		BA.AccountType, 
		BA.AccountName,
		(SELECT B.Name FROM Bank AS B Where B.BankID=(SELECT BA.BankID FROM BankAccount AS BA WHERE BA.BankAccountID=CB.BankAccountID)) AS BankName,
		(SELECT B.ShortName FROM Bank AS B Where B.BankID=(SELECT BA.BankID FROM BankAccount AS BA WHERE BA.BankAccountID=CB.BankAccountID)) AS BankShortName,
		(SELECT BB.BranchName FROM BankBranch AS BB Where BB.BankBranchID=(SELECT BA.BankBranchID FROM BankAccount AS BA WHERE BA.BankAccountID=CB.BankAccountID)) AS BankBranchName,
		(SELECT BU.Name FROM BusinessUnit AS BU Where BU.BusinessUnitID=(SELECT BA.BusinessUnitID FROM BankAccount AS BA WHERE BA.BankAccountID=CB.BankAccountID)) AS BusinessUnitName,
		(SELECT BU.Name+ ' ['+BU.Code+']' FROM BusinessUnit AS BU Where BU.BusinessUnitID=(SELECT BA.BusinessUnitID FROM BankAccount AS BA WHERE BA.BankAccountID=CB.BankAccountID)) AS BusinessUnitNameCode,
        (SELECT CS.ChequeSetupName FROM ChequeSetup AS CS WHERE CS.ChequeSetupID=(SELECT B.ChequeSetupID FROM Bank AS B Where B.BankID=(SELECT BA.BankID FROM BankAccount AS BA WHERE BA.BankAccountID=CB.BankAccountID))) AS ChequeSetupName

FROM		ChequeBook AS CB
LEFT OUTER JOIN	BankAccount AS BA ON CB.BankAccountID = BA.BankAccountID













GO
