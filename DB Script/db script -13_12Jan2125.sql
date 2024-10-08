IF EXISTS (SELECT * FROM sys.columns where Name = N'AccountHeadID' and Object_ID = Object_ID(N'ChequeRequisition'))
BEGIN
   ALTER TABLE ChequeRequisition
   DROP COLUMN  AccountHeadID
END
GO
/****** Object:  View [dbo].[View_Subledger]    Script Date: 1/12/2017 2:07:44 PM ******/
DROP VIEW [dbo].[View_Subledger]
GO
/****** Object:  View [dbo].[View_ChequeRequisitionDetail]    Script Date: 1/12/2017 2:07:44 PM ******/
DROP VIEW [dbo].[View_ChequeRequisitionDetail]
GO
/****** Object:  View [dbo].[View_ChequeRequisition]    Script Date: 1/12/2017 2:07:44 PM ******/
DROP VIEW [dbo].[View_ChequeRequisition]
GO
/****** Object:  View [dbo].[View_ACCostCenter]    Script Date: 1/12/2017 2:07:44 PM ******/
DROP VIEW [dbo].[View_ACCostCenter]
GO
/****** Object:  StoredProcedure [dbo].[SP_IUD_ChequeRequisitionDetail]    Script Date: 1/12/2017 2:07:44 PM ******/
DROP PROCEDURE [dbo].[SP_IUD_ChequeRequisitionDetail]
GO
/****** Object:  StoredProcedure [dbo].[SP_IUD_ChequeRequisition]    Script Date: 1/12/2017 2:07:44 PM ******/
DROP PROCEDURE [dbo].[SP_IUD_ChequeRequisition]
GO
/****** Object:  StoredProcedure [dbo].[SP_IUD_ChequeRequisition]    Script Date: 1/12/2017 2:07:44 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[SP_IUD_ChequeRequisition]
(
	@ChequeRequisitionID	as int,
	@RequisitionNo	as varchar(512),
	@BUID	as int,
	@RequisitionStatus	as smallint,
	@RequisitionDate	as date,	
	@SubledgerID	as int,
	@PayTo	as int,
	@ChequeDate	as date,
	@ChequeType	as smallint,
	@BankAccountID	as int,
	@BankBookID	as int,
	@ChequeID	as int,
	@ChequeAmount	as decimal(30, 17),
	@ApprovedBy	as int,
	@Remarks	as varchar(1024),
	@DBUserID as int,
	@DBOperation as smallint
	--%n, %s, %n, %n, %d, %n, %n, %d, %n, %n, %n, %n, %n, %n, %s, %n, %n
)
AS
BEGIN TRAN
DECLARE 
@DBServerDateTime as datetime,
@PV_YearPart as Varchar(100),
@PV_RequisitionNo as Varchar(100),
@PV_NumericPart as int,
@PV_NumericPartInString as varchar(100),
@PV_InitialText as Varchar(2)

SET @DBServerDateTime=Getdate()	
IF(@DBOperation=1)
BEGIN	
	SET @PV_YearPart=SUBSTRING((SELECT DATENAME(YEAR,@RequisitionDate)),3,2)	
	IF EXISTS(SELECT * FROM ChequeRequisition as HH WHERE  YEAR(HH.RequisitionDate) = YEAR(@RequisitionDate))
	BEGIN
		SET @PV_RequisitionNo = (SELECT NN.RequisitionNo FROM ChequeRequisition AS NN WHERE NN.ChequeRequisitionID=(SELECT MAX(HH.ChequeRequisitionID) from ChequeRequisition AS HH WHERE YEAR(HH.RequisitionDate) = YEAR(@RequisitionDate)))		
		SET @PV_NumericPart=CONVERT(int,SUBSTRING(@PV_RequisitionNo,1,5))+1
		SET @PV_NumericPartInString=RIGHT('00000' + CONVERT(VARCHAR(5), @PV_NumericPart), 5)	
		SET @RequisitionNo=@PV_NumericPartInString+'/'+@PV_YearPart	
	END
	ELSE
	BEGIN	
		--00001/17
		SET @RequisitionNo='00001/'+@PV_YearPart --for Reset Number 		
	END		

	SET @ChequeRequisitionID=(SELECT ISNULL(MAX(ChequeRequisitionID),0)+1 FROM ChequeRequisition)
	INSERT INTO ChequeRequisition	(ChequeRequisitionID,	RequisitionNo,	BUID,	RequisitionStatus,	RequisitionDate,	SubledgerID,	PayTo,	ChequeDate,		ChequeType,		BankAccountID,		BankBookID,		ChequeID,	ChequeAmount,	ApprovedBy,		Remarks,	DBUserID,	DBServerDateTime)
							VALUES	(@ChequeRequisitionID,	@RequisitionNo,	@BUID,	@RequisitionStatus,	@RequisitionDate,	@SubledgerID,	@PayTo,	@ChequeDate,	@ChequeType,	@BankAccountID,		@BankBookID,	@ChequeID,	@ChequeAmount,	@ApprovedBy,	@Remarks,	@DBUserID,	@DBServerDateTime)
	SELECT * FROM View_ChequeRequisition WHERE ChequeRequisitionID=@ChequeRequisitionID
END

IF(@DBOperation=2)
BEGIN
	IF(@ChequeRequisitionID<=0) 
	BEGIN
		ROLLBACK
			RAISERROR (N'Selected ChequeRequisition is not valid. Please try again!!~',16,1);	
		RETURN
	END
	UPDATE ChequeRequisition SET  BUID=@BUID,	RequisitionStatus=@RequisitionStatus,	RequisitionDate=@RequisitionDate,	SubledgerID=@SubledgerID,	PayTo=@PayTo,	ChequeDate=@ChequeDate,		ChequeType=@ChequeType,		BankAccountID=@BankAccountID,		BankBookID=@BankBookID,		ChequeID=@ChequeID,	ChequeAmount=@ChequeAmount,	ApprovedBy=@ApprovedBy,		Remarks=@Remarks WHERE ChequeRequisitionID =@ChequeRequisitionID
	SELECT * FROM View_ChequeRequisition WHERE ChequeRequisitionID=@ChequeRequisitionID
END

IF(@DBOperation=3)
BEGIN
	IF(@ChequeRequisitionID<=0)
	BEGIN
		ROLLBACK
			RAISERROR (N'Selected ChequeRequisition is not valid. Please try again!!~',16,1);	
		RETURN
	END	
	IF((SELECT ApprovedBy FROM ChequeRequisition WHERE ChequeRequisitionID=@ChequeRequisitionID)!=0)
	BEGIN
		ROLLBACK
			RAISERROR (N'Deletion Not Possible. Your Selected ChequeRequisition Already Approved !!~',16,1);	
		RETURN
	END	
	DELETE FROM ChequeRequisitionDetail WHERE ChequeRequisitionID=@ChequeRequisitionID	
	DELETE FROM ChequeRequisition WHERE ChequeRequisitionID=@ChequeRequisitionID
END
COMMIT TRAN







GO
/****** Object:  StoredProcedure [dbo].[SP_IUD_ChequeRequisitionDetail]    Script Date: 1/12/2017 2:07:44 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[SP_IUD_ChequeRequisitionDetail]
(	
	@ChequeRequisitionDetailID	as int,
	@ChequeRequisitionID	as int,
	@VoucherBillID	as int,
	@Amount	as decimal(30, 17),
	@Remarks	as varchar(1012),
	@DBUserID  as int,
	@DBOperation as smallint,
	@ChequeRequisitionDetailIDs as varchar(512)
	--%n, %n, %n, %n, %s, %n, %n, %s
)
AS
BEGIN TRAN
DECLARE 
@DBServerDateTime as datetime,
@PV_OrderType as smallint
SET @DBServerDateTime=Getdate()	
	
IF(@DBOperation=1)
BEGIN	
	SET @ChequeRequisitionDetailID= (SELECT ISNULL(MAX(ChequeRequisitionDetailID),0)+1 FROM ChequeRequisitionDetail)	
	INSERT INTO ChequeRequisitionDetail	(ChequeRequisitionDetailID,		ChequeRequisitionID,	VoucherBillID,	Amount,		Remarks,	DBUserID,	DBServerDateTime)
								VALUES	(@ChequeRequisitionDetailID,	@ChequeRequisitionID,	@VoucherBillID,	@Amount,	@Remarks,	@DBUserID,	@DBServerDateTime)
	SELECT * FROM View_ChequeRequisitionDetail  WHERE ChequeRequisitionDetailID=@ChequeRequisitionDetailID
END
IF(@DBOperation=2)
BEGIN
	IF (@ChequeRequisitionDetailID<0) 
	BEGIN
		ROLLBACK
			RAISERROR (N' Selected ChequeRequisition Detail are Invalid Please try again!!~',16,1);	
		RETURN
	END
	UPDATE ChequeRequisitionDetail SET	ChequeRequisitionID=@ChequeRequisitionID,	VoucherBillID=@VoucherBillID,	Amount=@Amount,		Remarks=@Remarks WHERE ChequeRequisitionDetailID = @ChequeRequisitionDetailID
	SELECT * FROM View_ChequeRequisitionDetail  WHERE ChequeRequisitionDetailID=@ChequeRequisitionDetailID	
END

IF(@DBOperation=3)
BEGIN
	IF (@ChequeRequisitionID<0) 
	BEGIN
		ROLLBACK
			RAISERROR (N' Selected ChequeRequisition are Invalid Please try again!!~',16,1);	
		RETURN
	END
	DELETE FROM ChequeRequisitionDetail WHERE ChequeRequisitionID=@ChequeRequisitionID AND ChequeRequisitionDetailID NOT IN (SELECT * FROM dbo.SplitInToDataSet(@ChequeRequisitionDetailIDs,','))
END
COMMIT TRAN






GO
/****** Object:  View [dbo].[View_ACCostCenter]    Script Date: 1/12/2017 2:07:44 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE View [dbo].[View_ACCostCenter]
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
		ACCostCenter.IsBillRefApply,
		ACCostCenter.IsChequeApply,
		dbo.FN_GetSLWBUName(ACCostCenter.ACCostCenterID, 0) AS BUName,
		(SELECT CC.Name FROM ACCostCenter AS CC WHERE CC.ACCostCenterID=ACCostCenter.ParentID) AS CategoryName,
		(SELECT CC.Code FROM ACCostCenter AS CC WHERE CC.ACCostCenterID=ACCostCenter.ParentID)+''+ACCostCenter.Code AS CategoryCode,
		(SELECT CC.IsBillRefApply FROM ACCostCenter AS CC WHERE CC.ACCostCenterID=ACCostCenter.ParentID) AS IsCategoryBillRefApply,
		(SELECT CC.IsChequeApply FROM ACCostCenter AS CC WHERE CC.ACCostCenterID=ACCostCenter.ParentID) AS IsCategoryChequeApply,		
		0 AS DueAmount,
		0 AS OverDueDays,		
		ACCostCenter.Name+' ['+ACCostCenter.Code+']' AS NameCode
		                   
FROM	ACCostCenter



















GO
/****** Object:  View [dbo].[View_ChequeRequisition]    Script Date: 1/12/2017 2:07:44 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE VIEW [dbo].[View_ChequeRequisition]
AS
SELECT	CR.ChequeRequisitionID,
		CR.RequisitionNo,
		CR.BUID,
		CR.RequisitionStatus,
		CR.RequisitionDate,		
		CR.SubledgerID,
		CR.PayTo,
		CR.ChequeDate,
		CR.ChequeType,
		CR.BankAccountID,
		CR.BankBookID,
		CR.ChequeID,
		CR.ChequeAmount,
		CR.ApprovedBy,
		CR.Remarks,
		BusinessUnit.Name AS BUName,
		BusinessUnit.Code AS BUCode,		
		CC.Name AS SubledgerName,		
		CC.Code AS SubledgerCode,
		IssueFigure.ChequeIssueTo,
		IssueFigure.SecondLineIssueTo,
		BAC.AccountNo,
		BAC.BankName, 
		BAC.BranchName, 
		ChequeBook.BookCodePartOne+'-'+ChequeBook.BookCodePartTwo AS BookCode,
		Cheque.ChequeNo,
		Cheque.ChequeStatus,
		View_User.UserName AS ApprovedByName
		
FROM		ChequeRequisition AS CR
LEFT OUTER JOIN	BusinessUnit ON CR.BUID = BusinessUnit.BusinessUnitID
LEFT OUTER JOIN	ACCostCenter AS CC ON CR.SubledgerID = CC.ACCostCenterID
LEFT OUTER JOIN	IssueFigure ON CR.PayTo = IssueFigure.IssueFigureID
LEFT OUTER JOIN	View_BankAccount AS BAC ON CR.BankAccountID = BAC.BankAccountID
LEFT OUTER JOIN	ChequeBook ON CR.BankBookID = ChequeBook.ChequeBookID
LEFT OUTER JOIN	Cheque ON CR.ChequeID = Cheque.ChequeID
LEFT OUTER JOIN	View_User ON CR.ApprovedBy = View_User.UserID


GO
/****** Object:  View [dbo].[View_ChequeRequisitionDetail]    Script Date: 1/12/2017 2:07:44 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE VIEW [dbo].[View_ChequeRequisitionDetail]
AS
SELECT	CRD.ChequeRequisitionDetailID, 
		CRD.ChequeRequisitionID, 
		CRD.VoucherBillID, 		
		CRD.Amount, 
		CRD.Remarks,		
		VB.BillNo,
		VB.BillDate,
		VB.AccountHeadName,
		VB.Amount AS BillAmount,
		VB.RemainningBalance

FROM			ChequeRequisitionDetail  AS CRD
LEFT OUTER JOIN	View_VoucherBill AS VB ON CRD.VoucherBillID = VB.VoucherBillID









GO
/****** Object:  View [dbo].[View_Subledger]    Script Date: 1/12/2017 2:07:44 PM ******/
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
		ACCostCenter.IsBillRefApply,
		ACCostCenter.IsChequeApply,
		dbo.FN_GetSLWBUName(ACCostCenter.ACCostCenterID, 0) AS BUName,
		(SELECT CC.Name FROM ACCostCenter AS CC WHERE CC.ACCostCenterID=ACCostCenter.ParentID) AS CategoryName,
		(SELECT CC.Code FROM ACCostCenter AS CC WHERE CC.ACCostCenterID=ACCostCenter.ParentID)+''+ACCostCenter.Code AS CategoryCode,
		(SELECT CC.IsBillRefApply FROM ACCostCenter AS CC WHERE CC.ACCostCenterID=ACCostCenter.ParentID) AS IsCategoryBillRefApply,
		(SELECT CC.IsChequeApply FROM ACCostCenter AS CC WHERE CC.ACCostCenterID=ACCostCenter.ParentID) AS IsCategoryChequeApply,
		ISNULL((SELECT SUM(HH.DueCheque) FROM View_VoucherBill AS HH WHERE HH.SubLedgerID=ACCostCenter.ACCostCenterID),0) AS DueAmount,
		ISNULL((SELECT MAX(HH.OverDueDays) FROM View_VoucherBill AS HH WHERE HH.SubLedgerID=ACCostCenter.ACCostCenterID),0) AS OverDueDays,		
		ACCostCenter.Name+' ['+ACCostCenter.Code+']' AS NameCode
		                   
FROM	ACCostCenter


















GO
