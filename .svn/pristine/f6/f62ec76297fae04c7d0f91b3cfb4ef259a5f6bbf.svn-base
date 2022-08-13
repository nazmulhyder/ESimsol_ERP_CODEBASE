IF NOT EXISTS (SELECT * FROM sys.columns where Name = N'IsShowLedgerBalance' and Object_ID = Object_ID(N'Users'))
BEGIN
   ALTER TABLE Users
   ADD IsShowLedgerBalance bit
END
GO
UPDATE Users SET IsShowLedgerBalance=0
GO
ALTER FUNCTION [dbo].[GetLedgerBalance]
(
	@AccountHeadID as int,
	@BusinessUnitID as int,
	@UserID as int
)
RETURNS VARCHAR(512)
AS
BEGIN
--DECLARE
--@AccountHeadID as int,
--@BusinessUnitID as int,
--@UserID as int

--SET @AccountHeadID = 178
--SET @BusinessUnitID = 1
--SET @UserID =2

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

IF(@UserID!=-9)
BEGIN
	IF NOT EXISTS(SELECT * FROM Users AS HH WHERE HH.UserID = @UserID AND HH.IsShowLedgerBalance=1)
	BEGIN
		SET @Result='--'
	END
END
--SELECT @Result
RETURN @Result
END;





GO

ALTER PROCEDURE [dbo].[SP_IUD_User]
(
	@UserID as int,
	@LogInID as varchar(512),
	@UserName as varchar(512),
	@Password as varchar(512),
	@OwnerID as int,
	@LoggedOn as bit,
	@LoggedOnMachine as varchar(512),
	@CanLogin as bit,
	@DomainUserName as varchar(512),
	@EmployeeID as int,
	@LocationID as int,
	@AccountHolderType as smallint,
	@EmailAddress as varchar(512),
	@FinancialUserType as smallint,
	@IsLocationBindded as bit,
	@DBOperation as smallint
	--%n, %s, %s, %s, %n, %b, %s, %b, %s, %n, %n, %n, %s, %n, %b, %n
)		
AS
BEGIN TRAN
DECLARE 
@DBServerDateTime as datetime
SET @DBServerDateTime=Getdate()
IF(@DBOperation=1)
BEGIN		
		IF EXISTS(SELECT * FROM Users WHERE LogInID=@LogInID)
		BEGIN
			ROLLBACK
				RAISERROR (N'Your Engtered log-in id al ready exists!!',16,1);	
			RETURN
		END	
		SET @UserID=(SELECT ISNULL(MAX(UserID),0)+1 FROM Users)		
		SET @DBServerDateTime=getdate()		
		INSERT INTO Users	(UserID,	LogInID,	UserName,	[Password],	OwnerID,	LoggedOn,	LoggedOnMachine,	CanLogin,	DomainUserName,		EmployeeID,		LocationID,		AccountHolderType,	EmailAddress,	FinancialUserType, IsLocationBindded, IsShowLedgerBalance)
					VALUES	(@UserID,	@LogInID,	@UserName,	@Password,	@OwnerID,	@LoggedOn,	@LoggedOnMachine,	@CanLogin,	@DomainUserName,	@EmployeeID,	@LocationID,	@AccountHolderType,	@EmailAddress,	@FinancialUserType, @IsLocationBindded, 0)    				  
    	SELECT * FROM View_User WHERE UserID=@UserID
END

IF(@DBOperation=2)
BEGIN
	IF(@UserID<=0)
	BEGIN
		ROLLBACK
			RAISERROR (N'Your Selected User Is Invalid Please Refresh and try again!!',16,1);	
		RETURN
	END	
	IF EXISTS(SELECT * FROM Users WHERE LogInID=@LogInID AND UserID!=@UserID)
	BEGIN
		ROLLBACK
			RAISERROR (N'Your Entered log-in id al ready exists!!',16,1);	
		RETURN
	END	
	UPDATE Users SET  LogInID=@LogInID,	UserName=@UserName,	/*[Password]=@Password,*/	OwnerID=@OwnerID,	LoggedOn=@LoggedOn,	LoggedOnMachine=@LoggedOnMachine,	CanLogin=@CanLogin,	DomainUserName=@DomainUserName,		EmployeeID=@EmployeeID,		LocationID=@LocationID,		AccountHolderType=@AccountHolderType,	EmailAddress=@EmailAddress, FinancialUserType=@FinancialUserType   WHERE UserID=@UserID  
 	SELECT * FROM View_User WHERE UserID=@UserID
END

IF(@DBOperation=3)
BEGIN
	IF(@UserID<=0)
	BEGIN
		ROLLBACK
			RAISERROR (N'Your Selected User Is Invalid Please Refresh and try again!!',16,1);	
		RETURN
	END
	DELETE FROM UserPermissionFinance WHERE UserID=@UserID	
	DELETE FROM Users WHERE UserID=@UserID
END

IF(@DBOperation=5)
BEGIN
	IF NOT EXISTS(Select * from Users WHere UserID=@UserID)
	BEGIN
		ROLLBACK
			RAISERROR (N'Your Selected User Is Invalid Please Refresh and try again!!',16,1);	
		RETURN
	END
	UPDATE Users SET  IsLocationBindded=@IsLocationBindded  WHERE UserID=@UserID  
	
END

COMMIT TRAN


GO


ALTER VIEW [dbo].[View_User]
AS
SELECT		Users.UserID, 
			Users.LogInID, 
			Users.UserName, 
			Users.Password, 
			Users.OwnerID, 
			Users.LoggedOn, 
			Users.LoggedOnMachine, 
			Users.CanLogin, 
			Users.DomainUserName, 
			Users.EmployeeID, 
			Users.LocationID, 
			Users.AccountHolderType, 
			Users.EmailAddress,
			Users.FinancialUserType,
			Users.IsLocationBindded,
			Users.IsShowLedgerBalance,
            Employee.Name+'[ '+Employee.Code+' ]' AS EmployeeNameCode,                         
            Location.Name AS LocationName
            ,CASE WHEN Users.AccountHolderType = 2 THEN  Users.EmployeeID ELSE 0 END as ContractorID
            ,CASE WHEN Users.AccountHolderType = 1 THEN Employee.EmployeeDesignationType ELSE 0 END as EmployeeType
            ,CASE  
		    WHEN Users.AccountHolderType = 2 THEN (SELECT Top(1)ContractorType.ContractorType FROM ContractorType WHERE ContractorID=Users.EmployeeID)
		    else 0 END  AS ContractorType
		    , ISNULL(Employee.CompanyID,0) AS CompanyID
            
FROM			Users 
left JOIN	Employee ON Users.EmployeeID = Employee.EmployeeID 
LEFT JOIN		Location ON Users.LocationID = Location.LocationID





GO


ALTER FUNCTION [dbo].[FN_GetsRevarseHead] 
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
SELECT @ReturnData = @ReturnData +  HH.RevarseHeadName + ' @ '+ HH.Currency+ ' ' + CONVERT(VARCHAR, CAST(SUM(HH.Amount) AS MONEY), 1) + '; ' FROM @TempTable AS HH GROUP BY RevarseHeadName, Currency, IsLedger ORDER BY IsLedger ASC

SET @ReturnData= SUBSTRING(@ReturnData, 0, LEN(@ReturnData))
--SELECT @ReturnData
RETURN @ReturnData
END

