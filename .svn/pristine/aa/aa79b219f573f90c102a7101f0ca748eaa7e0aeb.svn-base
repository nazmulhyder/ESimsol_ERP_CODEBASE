SELECT TOP 10 * FROM View_ImportPayment WHERE BUID=1 AND LiabilityType!=1 AND ISNULL(ApprovedBy,0)!=0 AND ImportPaymentID NOT IN (SELECT HH.PKValue FROM VoucherMapping AS HH WHERE HH.TableName='ImportPayment' AND HH.PKColumnName='ImportPaymentID' AND HH.VoucherSetup=8) ORDER BY ImportPaymentID ASC
SELECT HH.BUID FROM View_ImportPayment AS HH WHERE HH.ImportPaymentID=1
SELECT HH.PaymentDate FROM View_ImportPayment AS HH WHERE HH.ImportPaymentID=1
SELECT HH.ImportInvoiceNo FROM View_ImportPayment AS HH WHERE HH.ImportPaymentID=1
SELECT HH.LiabilityNo FROM View_ImportPayment AS HH WHERE HH.ImportPaymentID=1
SELECT * FROM View_ImportPayment AS HH WHERE HH.ImportPaymentID=1
SELECT 'Payable to Supplier -Foreign' FROM View_ImportPayment AS HH WHERE HH.ImportPaymentID=1
SELECT HH.InvoiceCurrencyID FROM View_ImportPayment AS HH WHERE HH.ImportPaymentID=1
SELECT HH.CRate_Acceptance FROM View_ImportPayment AS HH WHERE HH.ImportPaymentID=1
SELECT HH.Amount_Invoice FROM View_ImportPayment AS HH WHERE HH.ImportPaymentID=1
SELECT HH.CurrencyID FROM View_ImportPayment AS HH WHERE HH.ImportPaymentID=1
SELECT HH.CCRate FROM View_ImportPayment AS HH WHERE HH.ImportPaymentID=1
SELECT HH.Amount FROM View_ImportPayment AS HH WHERE HH.ImportPaymentID=1
SELECT CC.ACCostCenterID FROM ACCostCenter AS CC WHERE CC.ReferenceType=2 AND CC.ReferenceObjectID=(SELECT HH.ContractorID FROM View_ImportPayment AS HH WHERE HH.ImportPaymentID=1)
SELECT 14 AS CCCategory
SELECT * FROM ACCostCenter WHERE ParentID=1
SELECT VB.VoucherBillID FROM VoucherBill AS VB WHERE VB.ReferenceType=3 AND VB.ReferenceObjID=(SELECT HH.ImportInvoiceID FROM View_ImportPayment AS HH WHERE HH.ImportPaymentID=1)
SELECT HH.DateOfMaturity FROM View_ImportPayment AS HH WHERE HH.ImportPaymentID=1
SELECT MM.VOrderRefID FROM VOrder AS MM WHERE MM.VOrderRefType=5 AND MM.VOrderRefID=(SELECT HH.ImportLCID FROM View_ImportPayment AS HH WHERE HH.ImportPaymentID=1)
SELECT HH.ImportLCNo FROM View_ImportPayment AS HH WHERE HH.ImportPaymentID=1
SELECT * FROM View_EHTransaction AS HH WHERE HH.ExpenditureType=21 AND HH.RefObjectID=112
SELECT HH.AccountHeadID FROM View_EHTransaction AS HH WHERE HH.EHTransactionID=1
SELECT 'Cost Head' FROM View_EHTransaction AS HH WHERE HH.EHTransactionID=1
SELECT HH.CurrencyID FROM View_EHTransaction AS HH WHERE HH.EHTransactionID=1
SELECT HH.CCRate FROM View_EHTransaction AS HH WHERE HH.EHTransactionID=1
SELECT HH.Amount FROM View_EHTransaction AS HH WHERE HH.EHTransactionID=1
SELECT * FROM View_ImportPayment AS HH WHERE HH.ImportInvoiceID=1
--SELECT *,%n AS EHTransactionID FROM View_ImportPayment AS HH WHERE HH.ImportInvoiceID=%n
SELECT * FROM View_ImportPayment AS HH WHERE HH.ForExGainLoss=2 AND HH.ImportPaymentID=1
SELECT 'Foreign Exchange Gain/(Loss)' FROM View_ImportPayment AS HH WHERE HH.ImportPaymentID=1
SELECT HH.ForExCurrencyID FROM View_ImportPayment AS HH WHERE HH.ImportPaymentID=1
SELECT HH.ForExCCRate FROM View_ImportPayment AS HH WHERE HH.ImportPaymentID=1
SELECT HH.ForExAmount FROM View_ImportPayment AS HH WHERE HH.ImportPaymentID=1
SELECT 'Term Loan' FROM View_ImportPayment AS HH WHERE HH.ImportPaymentID=1
SELECT HH.CurrencyID FROM View_ImportPayment AS HH WHERE HH.ImportPaymentID=1
SELECT HH.CCRate FROM View_ImportPayment AS HH WHERE HH.ImportPaymentID=1
SELECT HH.Amount FROM View_ImportPayment AS HH WHERE HH.ImportPaymentID=1

SELECT CC.ACCostCenterID FROM ACCostCenter AS CC WHERE CC.ReferenceType=7 AND CC.ReferenceObjectID=(SELECT HH.ImportPaymentID FROM View_ImportPayment AS HH WHERE HH.ImportPaymentID=1)
SELECT * FROM ACCostCenter WHERE ParentID=1
SELECT 13 AS CCCategory
SELECT * FROM View_ImportPayment AS HH WHERE HH.ImportPaymentID=1
SELECT VB.VoucherBillID FROM VoucherBill AS VB WHERE VB.ReferenceType=5 AND VB.ReferenceObjID=(SELECT HH.ImportPaymentID FROM View_ImportPayment AS HH WHERE HH.ImportPaymentID=1)
SELECT HH.DateOfOpening FROM View_ImportPayment AS HH WHERE HH.ImportPaymentID=1
SELECT HH.DateOfMaturity FROM View_ImportPayment AS HH WHERE HH.ImportPaymentID=1
SELECT HH.ImportInvoiceNo FROM View_ImportPayment AS HH WHERE HH.ImportPaymentID=1
SELECT HH.PaymentDate FROM View_ImportPayment AS HH WHERE HH.ImportPaymentID=1
SELECT * FROM View_ImportPayment AS HH WHERE ISNULL(HH.LCMarginAmount,0)>0 AND HH.ImportPaymentID=1
SELECT 'LCMargin' FROM View_ImportPayment AS HH WHERE HH.ImportPaymentID=1
SELECT HH.MarginCurrencyID FROM View_ImportPayment AS HH WHERE HH.ImportPaymentID=1
SELECT HH.MarginCCRate FROM View_ImportPayment AS HH WHERE HH.ImportPaymentID=1
SELECT HH.LCMarginAmount FROM View_ImportPayment AS HH WHERE HH.ImportPaymentID=1
SELECT CC.ACCostCenterID FROM ACCostCenter AS CC WHERE CC.ReferenceType=4 AND CC.ReferenceObjectID=(SELECT HH.MarginAccountID FROM View_ImportPayment AS HH WHERE HH.ImportPaymentID=1)