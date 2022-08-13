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
    public class ImportPaymentRequestDetailService : MarshalByRefObject, IImportPaymentRequestDetailService
    {
        #region Private functions and declaration
        private ImportPaymentRequestDetail MapObject(NullHandler oReader)
        {
            ImportPaymentRequestDetail oImportPaymentRequestDetail = new ImportPaymentRequestDetail();
            oImportPaymentRequestDetail.PIPRDetailID = oReader.GetInt32("PIPRDetailID");
            oImportPaymentRequestDetail.PIPRID = oReader.GetInt32("ImportPaymentRequestID");
            oImportPaymentRequestDetail.ImportInvoiceID = oReader.GetInt32("ImportInvoiceID");
            oImportPaymentRequestDetail.ImportInvoiceNo = oReader.GetString("ImportInvoiceNo");
            oImportPaymentRequestDetail.DateofInvoice = oReader.GetDateTime("DateofInvoice");
            oImportPaymentRequestDetail.Amount = oReader.GetDouble("Amount");
            //oImportPaymentRequestDetail.LDBPAmount = oReader.GetDouble("LDBPAmount");
            //oImportPaymentRequestDetail.LDBCNo = oReader.GetString("LDBCNo");
            oImportPaymentRequestDetail.ContractorName = oReader.GetString("ContractorName");
            oImportPaymentRequestDetail.DateofMaturity = oReader.GetDateTime("DateofMaturity");
            oImportPaymentRequestDetail.IssueBankName = oReader.GetString("IssueBankName");
            oImportPaymentRequestDetail.ImportLCNo = oReader.GetString("ImportLCNo");
            oImportPaymentRequestDetail.Status = oReader.GetInt16("Status");
            oImportPaymentRequestDetail.Currency = oReader.GetString("Currency");
            oImportPaymentRequestDetail.CCRate = oReader.GetDouble("CCRate");
            oImportPaymentRequestDetail.CurrencyID = oReader.GetInt32("CurrcncyID");
            oImportPaymentRequestDetail.AccountNo = oReader.GetString("AccountNo");
            oImportPaymentRequestDetail.BankBranchID = oReader.GetInt32("BankBranchID");
            //oImportPaymentRequestDetail.BankAccountIDRecd = oReader.GetInt32("BankAccountIDRecd");
            return oImportPaymentRequestDetail;
        }

        private ImportPaymentRequestDetail CreateObject(NullHandler oReader)
        {
            ImportPaymentRequestDetail oInvoicePurchaseDetail = new ImportPaymentRequestDetail();
            oInvoicePurchaseDetail = MapObject(oReader);
            return oInvoicePurchaseDetail;
        }

        private List<ImportPaymentRequestDetail> CreateObjects(IDataReader oReader)
        {
            List<ImportPaymentRequestDetail> oInvoicePurchaseDetail = new List<ImportPaymentRequestDetail>();
            NullHandler oHandler = new NullHandler(oReader);
            while (oReader.Read())
            {
                ImportPaymentRequestDetail oItem = CreateObject(oHandler);
                oInvoicePurchaseDetail.Add(oItem);
            }
            return oInvoicePurchaseDetail;
        }

        #endregion

        #region Interface implementation
        public ImportPaymentRequestDetailService() { }

        public ImportPaymentRequestDetail Save(ImportPaymentRequestDetail oInvoicePurchaseDetail, Int64 nUserID)
        {
            TransactionContext tc = null;

            List<ImportPaymentRequestDetail> _oInvoicePurchaseDetails = new List<ImportPaymentRequestDetail>();
            oInvoicePurchaseDetail.ErrorMessage = "";
            try
            {

                tc = TransactionContext.Begin(true);
                IDataReader reader;
                reader = ImportPaymentRequestDetailDA.InsertUpdate(tc, oInvoicePurchaseDetail, EnumDBOperation.Update, nUserID, "");
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oInvoicePurchaseDetail = new ImportPaymentRequestDetail();
                    oInvoicePurchaseDetail = CreateObject(oReader);
                }
                reader.Close();

                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                oInvoicePurchaseDetail.ErrorMessage = e.Message;
                #endregion
            }
            return oInvoicePurchaseDetail;
        }
     

        public ImportPaymentRequestDetail Get(int nInvoicePurchaseDetailID, Int64 nUserId)
        {
            ImportPaymentRequestDetail oAccountHead = new ImportPaymentRequestDetail();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = ImportPaymentRequestDetailDA.Get(tc, nInvoicePurchaseDetailID);
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
        public ImportPaymentRequestDetail GetBy(int nPurchaseInvoiceLCID, Int64 nUserId)
        {
            ImportPaymentRequestDetail oAccountHead = new ImportPaymentRequestDetail();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = ImportPaymentRequestDetailDA.GetBy(tc, nPurchaseInvoiceLCID);
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

        public List<ImportPaymentRequestDetail> Gets(int nnImportPaymentRequestID, Int64 nUserID)
        {
            List<ImportPaymentRequestDetail> oInvoicePurchaseDetail = null;

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = ImportPaymentRequestDetailDA.Gets(nnImportPaymentRequestID, tc);
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

        public List<ImportPaymentRequestDetail> Gets(string sSQL, Int64 nUserID)
        {
            List<ImportPaymentRequestDetail> oInvoicePurchaseDetail = null;

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = ImportPaymentRequestDetailDA.Gets(tc, sSQL);
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
        #endregion
    }
}
