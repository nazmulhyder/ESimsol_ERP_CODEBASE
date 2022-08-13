IF NOT EXISTS (SELECT * FROM sys.columns where Name = N'RegisterPrint' and Object_ID = Object_ID(N'Cheque'))
BEGIN
   ALTER TABLE Cheque
   ADD RegisterPrint bit
END

GO
UPDATE Cheque SET RegisterPrint=0
GO
ALTER VIEW [dbo].[View_Cheque]
AS
--updated by fahim0abir on 19/04/15
--excluded other views and joins using sub-query
SELECT		Cheque.ChequeID, 
			Cheque.ChequeBookID, 
			Cheque.ChequeStatus, 
			Cheque.PaymentType, 
			Cheque.ChequeNo, 
			Cheque.ChequeDate, 
			Cheque.PayTo, 
			Cheque.IssueFigureID, 
			Cheque.Amount, 
			Cheque.VoucherReference, 
			Cheque.Note, 
			Cheque.RegisterPrint, 
			(SELECT CB.BankAccountID From ChequeBook AS CB WHERE CB.ChequeBookID=Cheque.ChequeBookID) AS BankAccountID, 
			(SELECT BA.BankBranchID FROM BankAccount as BA WHERE BA.BankAccountID=(SELECT CB.BankAccountID FROM ChequeBook as CB WHERE CB.ChequeBookID=Cheque.ChequeBookID)) AS BankBranchID,
			(SELECT BA.BankID FROM BankAccount as BA WHERE BA.BankAccountID=(SELECT CB.BankAccountID FROM ChequeBook as CB WHERE CB.ChequeBookID=Cheque.ChequeBookID)) AS BankID,
			(SELECT BA.BusinessUnitID FROM BankAccount as BA WHERE BA.BankAccountID=(SELECT CB.BankAccountID FROM ChequeBook as CB WHERE CB.ChequeBookID=Cheque.ChequeBookID)) AS BusinessUnitID,
			(SELECT CB.BookCodePartOne FROM ChequeBook as CB WHERE CB.ChequeBookID=Cheque.ChequeBookID) AS BookCodePartOne,
			(SELECT CB.BookCodePartTwo FROM ChequeBook as CB WHERE CB.ChequeBookID=Cheque.ChequeBookID) AS BookCodePartTwo,
			(SELECT BA.AccountNo FROM BankAccount as BA WHERE BA.BankAccountID=(SELECT CB.BankAccountID FROM ChequeBook as CB WHERE CB.ChequeBookID=Cheque.ChequeBookID)) AS AccountNo,
			(SELECT B.Name From Bank AS B WHERE B.BankID=(SELECT BA.BankID FROM BankAccount as BA WHERE BA.BankAccountID=(SELECT CB.BankAccountID FROM ChequeBook as CB WHERE CB.ChequeBookID=Cheque.ChequeBookID))) AS BankName,
			(SELECT B.ShortName From Bank AS B WHERE B.BankID=(SELECT BA.BankID FROM BankAccount as BA WHERE BA.BankAccountID=(SELECT CB.BankAccountID FROM ChequeBook as CB WHERE CB.ChequeBookID=Cheque.ChequeBookID))) AS BankShortName,
			(SELECT BB.BranchName From BankBranch AS BB WHERE BB.BankBranchID=(SELECT BA.BankBranchID FROM BankAccount as BA WHERE BA.BankAccountID=(SELECT CB.BankAccountID FROM ChequeBook as CB WHERE CB.ChequeBookID=Cheque.ChequeBookID))) AS BankBranchName,
			(SELECT BU.Name From BusinessUnit AS BU WHERE BU.BusinessUnitID=(SELECT BA.BusinessUnitID FROM BankAccount as BA WHERE BA.BankAccountID=(SELECT CB.BankAccountID FROM ChequeBook as CB WHERE CB.ChequeBookID=Cheque.ChequeBookID))) AS BusinessUnitName,
			(SELECT B.ChequeSetupID From Bank AS B WHERE B.BankID=(SELECT BA.BankID FROM BankAccount as BA WHERE BA.BankAccountID=(SELECT CB.BankAccountID FROM ChequeBook as CB WHERE CB.ChequeBookID=Cheque.ChequeBookID))) AS ChequeSetupID,
			(SELECT C.Name FROM Contractor as C WHERE C.ContractorID=Cheque.PayTo) AS ContractorName,
			(SELECT C.Phone FROM Contractor as C WHERE C.ContractorID=Cheque.PayTo) AS ContractorPhone,
			(SELECT C.[Address] FROM Contractor as C WHERE C.ContractorID=Cheque.PayTo) AS ContractorAddress,
			(SELECT [IF].ChequeIssueTo FROM IssueFigure as [IF] WHERE [IF].IssueFigureID=Cheque.IssueFigureID) AS ChequeIssueTo,
			(SELECT [IF].SecondLineIssueTo FROM IssueFigure as [IF] WHERE [IF].IssueFigureID=Cheque.IssueFigureID) AS SecondLineIssueTo,
			(SELECT CH.DBUserID FROM ChequeHistory AS CH WHERE CH.ChequeHistoryID=(SELECT MAX(CH.ChequeHistoryID) FROM ChequeHistory AS CH WHERE CH.ChequeID=Cheque.ChequeID AND CH.CurrentStatus=Cheque.ChequeStatus)) AS OperationBy,
			(SELECT CH.DBServerDateTime FROM ChequeHistory AS CH WHERE CH.ChequeHistoryID=(SELECT MAX(CH.ChequeHistoryID) FROM ChequeHistory AS CH WHERE CH.ChequeID=Cheque.ChequeID AND CH.CurrentStatus=Cheque.ChequeStatus)) AS OperationDateTime,
			(SELECT U.UserName FROM Users AS U WHERE U.UserID=(SELECT CH.DBUserID FROM ChequeHistory AS CH WHERE CH.ChequeHistoryID=(SELECT MAX(CH.ChequeHistoryID) FROM ChequeHistory AS CH WHERE CH.ChequeID=Cheque.ChequeID AND CH.CurrentStatus=Cheque.ChequeStatus))) AS OperationByName

FROM		Cheque 
--LEFT OUTER JOIN		View_ChequeBook AS CB ON Cheque.ChequeBookID = CB.ChequeBookID
--LEFT OUTER JOIN	Contractor ON Cheque.PayTo = Contractor.ContractorID
--LEFT OUTER JOIN	IssueFigure ON Cheque.IssueFigureID = IssueFigure.IssueFigureID