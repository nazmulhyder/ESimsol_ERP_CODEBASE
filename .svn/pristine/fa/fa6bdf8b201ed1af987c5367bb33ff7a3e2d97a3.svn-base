using System;
using System.Collections.Generic;
using System.Data;
using ESimSol.BusinessObjects;
using ESimSol.Services.DataAccess;
using ICS.Core.DataAccess;
using ICS.Core.Framework;
using ICS.Core.Utility;

namespace ESimSol.Services.Services
{
    public class ImportPaymentSettlementService : MarshalByRefObject, IImportPaymentSettlementService
    {
        #region Private functions and declaration
        private ImportPaymentSettlement MapObject(NullHandler oReader)
        {
            ImportPaymentSettlement oImportPaymentSettlement = new ImportPaymentSettlement();
            oImportPaymentSettlement.PIPRDetailID = oReader.GetInt32("PIPRDetailID");
            oImportPaymentSettlement.ImportPaymentRequestID = oReader.GetInt32("ImportPaymentRequestID");
            oImportPaymentSettlement.ImportInvoiceID = oReader.GetInt32("ImportInvoiceID");
            oImportPaymentSettlement.ImportInvoiceNo = oReader.GetString("ImportLCNo");
            oImportPaymentSettlement.ImportInvoiceNo = oReader.GetString("ImportInvoiceNo");
            oImportPaymentSettlement.DateofInvoice = oReader.GetDateTime("DateofInvoice");
            oImportPaymentSettlement.Amount = oReader.GetDouble("Amount");
            oImportPaymentSettlement.RefNo = oReader.GetString("RefNo");
            oImportPaymentSettlement.DateofMaturity = oReader.GetDateTime("DateofMaturity");
            oImportPaymentSettlement.BankName = oReader.GetString("BankName");
            oImportPaymentSettlement.BranchName = oReader.GetString("BranchName");
            oImportPaymentSettlement.ImportLCNo = oReader.GetString("ImportLCNo");
            oImportPaymentSettlement.Status = oReader.GetInt16("Status");
            oImportPaymentSettlement.Currency_Inv = oReader.GetString("Currency_Inv");
            oImportPaymentSettlement.LetterIssueDate = oReader.GetDateTime("LetterIssueDate");
            oImportPaymentSettlement.AccountNo = oReader.GetString("AccountNo");
            oImportPaymentSettlement.LiabilityType = (EnumLiabilityType)oReader.GetInt32("LiabilityType");
            oImportPaymentSettlement.LiabilityTypeInt = oReader.GetInt32("LiabilityType");
            oImportPaymentSettlement.ImportPaymentID = oReader.GetInt32("ImportPaymentID");
            oImportPaymentSettlement.PaymentDate = oReader.GetDateTime("PaymentDate");
            oImportPaymentSettlement.PMTLiabilityNo = oReader.GetString("PMTLiabilityNo");
            oImportPaymentSettlement.PMTLoanOpenDate = oReader.GetDateTime("PMTLoanOpenDate");
            oImportPaymentSettlement.PMTRemarks = oReader.GetString("PMTRemarks");
            oImportPaymentSettlement.PMTAmount = oReader.GetDouble("PMTAmount");
            oImportPaymentSettlement.PMTCSymbol = oReader.GetString("PMTCSymbol");
            oImportPaymentSettlement.PMTCCRate = oReader.GetDouble("PMTCCRate");
            oImportPaymentSettlement.PMTAmountBC = oReader.GetDouble("PMTAmountBC");
            oImportPaymentSettlement.PMTApprovedBy = oReader.GetInt32("PMTApprovedBy");
            oImportPaymentSettlement.PMTApprovedByName = oReader.GetString("PMTApprovedByName");
            oImportPaymentSettlement.ForExGainLoss = (EnumForExGainLoss)oReader.GetInt32("ForExGainLoss");
            oImportPaymentSettlement.BankShortName = oReader.GetString("BankShortName");
            return oImportPaymentSettlement;
        }

        private ImportPaymentSettlement CreateObject(NullHandler oReader)
        {
            ImportPaymentSettlement oInvoicePurchaseDetail = new ImportPaymentSettlement();
            oInvoicePurchaseDetail = MapObject(oReader);
            return oInvoicePurchaseDetail;
        }

        private List<ImportPaymentSettlement> CreateObjects(IDataReader oReader)
        {
            List<ImportPaymentSettlement> oInvoicePurchaseDetail = new List<ImportPaymentSettlement>();
            NullHandler oHandler = new NullHandler(oReader);
            while (oReader.Read())
            {
                ImportPaymentSettlement oItem = CreateObject(oHandler);
                oInvoicePurchaseDetail.Add(oItem);
            }
            return oInvoicePurchaseDetail;
        }

        #endregion

        #region Interface implementation
        public ImportPaymentSettlementService() { }

        public ImportPaymentSettlement Get(int nInvoicePurchaseDetailID, Int64 nUserId)
        {
            ImportPaymentSettlement oAccountHead = new ImportPaymentSettlement();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = ImportPaymentSettlementDA.Get(tc, nInvoicePurchaseDetailID);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oAccountHead = CreateObject(oReader);
                }
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get InvoicePurchaseDetail", e);
                #endregion
            }

            return oAccountHead;
        }

        public List<ImportPaymentSettlement> Gets(int buid, int nBankStatus, Int64 nUserID)
        {
            List<ImportPaymentSettlement> oInvoicePurchaseDetail = null;

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = ImportPaymentSettlementDA.Gets(buid,nBankStatus, tc);
                oInvoicePurchaseDetail = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get InvoicePurchaseDetail", e);
                #endregion
            }

            return oInvoicePurchaseDetail;
        }

        public List<ImportPaymentSettlement> Gets(string sSQL, Int64 nUserID)
        {
            List<ImportPaymentSettlement> oImportPaymentSettlements = new List<ImportPaymentSettlement>();

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();
                IDataReader reader = null;
                reader = ImportPaymentSettlementDA.Gets(tc, sSQL);
                oImportPaymentSettlements = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get InvoicePurchaseDetail", e);
                #endregion
            }

            return oImportPaymentSettlements;
        }
        public ImportPaymentSettlement Save(ImportPayment oImportPayment, Int64 nUserID)
        {
            ImportPaymentSettlement oImportPaymentSettlement = new ImportPaymentSettlement();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
                List<EHTransaction> oEHTransactions = new List<EHTransaction>();
                oEHTransactions = oImportPayment.EHTransactions;
               
                IDataReader reader;
                if (oImportPayment.ImportPaymentID <= 0)
                {
                    reader = ImportPaymentDA.InsertUpdate(tc, oImportPayment, EnumDBOperation.Insert, nUserID);
                }
                else
                {
                    VoucherDA.CheckVoucherReference(tc, "ImportPayment", "ImportPaymentID", oImportPayment.ImportPaymentID);
                    reader = ImportPaymentDA.InsertUpdate(tc, oImportPayment, EnumDBOperation.Update, nUserID);
                }
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oImportPaymentSettlement = new ImportPaymentSettlement();
                    oImportPaymentSettlement = CreateObject(oReader);
                }
                reader.Close();

                #region EHTransaction Part
                if (oEHTransactions != null)
                {
                    string sEHTransactionIDs = "";
                    foreach (EHTransaction oItem in oEHTransactions)
                    {
                        IDataReader readerdetail;
                        oItem.RefObjectID = oImportPaymentSettlement.ImportInvoiceID;
                        if (oItem.EHTransactionID <= 0)
                        {
                            readerdetail = EHTransactionDA.InsertUpdate(tc, oItem, (int)EnumDBOperation.Insert, nUserID, "");
                        }
                        else
                        {
                            readerdetail = EHTransactionDA.InsertUpdate(tc, oItem, (int)EnumDBOperation.Update, nUserID, "");
                        }
                        NullHandler oReaderDetail = new NullHandler(readerdetail);
                        if (readerdetail.Read())
                        {
                            sEHTransactionIDs = sEHTransactionIDs + oReaderDetail.GetString("EHTransactionID") + ",";
                        }
                        readerdetail.Close();
                    }
                    if (sEHTransactionIDs.Length > 0)
                    {
                        sEHTransactionIDs = sEHTransactionIDs.Remove(sEHTransactionIDs.Length - 1, 1);
                    }

                    EHTransaction oEHTransaction = new EHTransaction();
                    oEHTransaction.ExpenditureType = EnumExpenditureType.ImportInvoice;
                    oEHTransaction.ExpenditureTypeInt = (int)EnumExpenditureType.ImportInvoice;
                    oEHTransaction.RefObjectID = oImportPaymentSettlement.ImportInvoiceID;
                    EHTransactionDA.Delete(tc, oEHTransaction, EnumDBOperation.Delete, nUserID, sEHTransactionIDs);
                }
                #endregion

                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                oImportPaymentSettlement = new ImportPaymentSettlement();
                oImportPaymentSettlement.ErrorMessage = e.Message;
                #endregion
            }

            return oImportPaymentSettlement;

        }

        public ImportPaymentSettlement Approved(ImportPayment oImportPayment, Int64 nUserID)
        {
            ImportPaymentSettlement oImportPaymentSettlement = new ImportPaymentSettlement();            
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
                IDataReader reader;
                reader = ImportPaymentDA.InsertUpdate(tc, oImportPayment, EnumDBOperation.Approval, nUserID);                
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oImportPaymentSettlement = new ImportPaymentSettlement();
                    oImportPaymentSettlement = CreateObject(oReader);
                }
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                oImportPaymentSettlement = new ImportPaymentSettlement();
                oImportPaymentSettlement.ErrorMessage = e.Message;
                #endregion
            }

            return oImportPaymentSettlement;

        }
    
        #endregion
    }
}
