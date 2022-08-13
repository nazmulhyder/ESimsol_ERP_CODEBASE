using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using ESimSol.BusinessObjects;
using ESimSol.Services.DataAccess;
using ICS.Core.DataAccess;
using ICS.Core.Framework;
using ICS.Core.Utility;

namespace ESimSol.Services.Services
{
    public class Export_LDBPDetailService : MarshalByRefObject, IExport_LDBPDetailService
    {
        #region Private functions and declaration
        private Export_LDBPDetail MapObject(NullHandler oReader)
        {
            Export_LDBPDetail oExport_LDBPDetail = new Export_LDBPDetail();
            oExport_LDBPDetail.Export_LDBPDetailID = oReader.GetInt32("Export_LDBPDetailID");
            oExport_LDBPDetail.Export_LDBPID = oReader.GetInt32("Export_LDBPID");
            oExport_LDBPDetail.ExportBillID = oReader.GetInt32("ExportBillID");
            oExport_LDBPDetail.ExportBillNo = oReader.GetString("ExportBillNo");
            oExport_LDBPDetail.ExportBillDate = oReader.GetDateTime("ExportBillDate");
            oExport_LDBPDetail.Amount = oReader.GetDouble("Amount");
            oExport_LDBPDetail.LDBPAmount = oReader.GetDouble("LDBPAmount");
            oExport_LDBPDetail.LDBCNo = oReader.GetString("LDBCNo");
            oExport_LDBPDetail.LDBPNo = oReader.GetString("LDBPNo");
            oExport_LDBPDetail.MaturityDate = oReader.GetDateTime("MaturityDate");
            oExport_LDBPDetail.BankName_Issue = oReader.GetString("BankName_Issue");
            oExport_LDBPDetail.ExportLCNo = oReader.GetString("ExportLCNo");
            oExport_LDBPDetail.Status = (EnumLCBillEvent)oReader.GetInt16("Status");
            oExport_LDBPDetail.LDBPDate = oReader.GetDateTime("LDBPDate");
            oExport_LDBPDetail.CCRate = oReader.GetDouble("CCRate");
            oExport_LDBPDetail.CurrencyID = oReader.GetInt32("CurrcncyID");
            oExport_LDBPDetail.Currency = oReader.GetString("Currency_Bill");
            oExport_LDBPDetail.AccountNo = oReader.GetString("AccountNo");
            oExport_LDBPDetail.ApplicantName = oReader.GetString("ApplicantName");
            oExport_LDBPDetail.BankBranchID = oReader.GetInt32("BankBranchID");
            oExport_LDBPDetail.BankAccountIDRecd = oReader.GetInt32("BankAccountIDRecd");
            return oExport_LDBPDetail;
        }

        private Export_LDBPDetail CreateObject(NullHandler oReader)
        {
            Export_LDBPDetail oInvoicePurchaseDetail = new Export_LDBPDetail();
            oInvoicePurchaseDetail = MapObject(oReader);
            return oInvoicePurchaseDetail;
        }

        private List<Export_LDBPDetail> CreateObjects(IDataReader oReader)
        {
            List<Export_LDBPDetail> oInvoicePurchaseDetail = new List<Export_LDBPDetail>();
            NullHandler oHandler = new NullHandler(oReader);
            while (oReader.Read())
            {
                Export_LDBPDetail oItem = CreateObject(oHandler);
                oInvoicePurchaseDetail.Add(oItem);
            }
            return oInvoicePurchaseDetail;
        }

        #endregion

        #region Interface implementation
        public Export_LDBPDetailService() { }
        public Export_LDBPDetail Save(Export_LDBPDetail oExport_LDBPDetail, Int64 nUserID)
        {
            Export_LDBP oExport_LDBP = new Export_LDBP();
            oExport_LDBP = oExport_LDBPDetail.Export_LDBP;
            int nCount = 0;
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);

                #region ExportBill Part
                if (oExport_LDBP != null)
                {
                    IDataReader reader;

                    reader = Export_LDBPDA.InsertUpdate(tc, oExport_LDBP, EnumDBOperation.Insert, nUserID);
                    NullHandler oReader = new NullHandler(reader);
                    if (reader.Read())
                    {
                        oExport_LDBP = new Export_LDBP();
                        oExport_LDBP = Export_LDBPService.CreateObject(oReader);
                        oExport_LDBPDetail.Export_LDBPID = oExport_LDBP.Export_LDBPID;
                    }
                    if (oExport_LDBP.Export_LDBPID <= 0)
                    {
                        oExport_LDBPDetail = new Export_LDBPDetail();
                        oExport_LDBPDetail.ErrorMessage = "Invalid ExportPI";
                        return oExport_LDBPDetail;
                    }
                    reader.Close();
                }
                #endregion

                IDataReader readerdetail;
                if (oExport_LDBPDetail.Export_LDBPDetailID <= 0)
                {
                    readerdetail = Export_LDBPDetailDA.InsertUpdate(tc, oExport_LDBPDetail, EnumDBOperation.Insert, nUserID,"");
                }
                else
                {
                    readerdetail = Export_LDBPDetailDA.InsertUpdate(tc, oExport_LDBPDetail, EnumDBOperation.Update, nUserID,"");
                }
                NullHandler oReaderDetail = new NullHandler(readerdetail);
                if (readerdetail.Read())
                {
                    oExport_LDBPDetail = new Export_LDBPDetail();
                    oExport_LDBPDetail = CreateObject(oReaderDetail);
                    oExport_LDBPDetail.Export_LDBP = oExport_LDBP;
                }
                readerdetail.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                oExport_LDBPDetail = new Export_LDBPDetail();
                oExport_LDBPDetail.ErrorMessage = e.Message;

                //ExceptionLog.Write(e);
                //throw new ServiceException("Failed to . Because of " + e.Message, e);
                #endregion
            }
            return oExport_LDBPDetail;
        }       
      
        public Export_LDBPDetail Save_LDBP(Export_LDBPDetail oExport_LDBPDetail, Int64 nUserID)
        {
            TransactionContext tc = null;

            List<Export_LDBPDetail> _oInvoicePurchaseDetails = new List<Export_LDBPDetail>();
            oExport_LDBPDetail.ErrorMessage = "";
            try
            {

                tc = TransactionContext.Begin(true);
                IDataReader reader;
                reader = Export_LDBPDetailDA.InsertUpdate_LDBP(tc, oExport_LDBPDetail, EnumDBOperation.Insert, nUserID);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oExport_LDBPDetail = new Export_LDBPDetail();
                    oExport_LDBPDetail = CreateObject(oReader);
                }
                reader.Close();

                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                oExport_LDBPDetail.ErrorMessage =  e.Message.Split('~')[0];
                #endregion
            }
            return oExport_LDBPDetail;
        }
        public Export_LDBPDetail Cancel_Request(Export_LDBPDetail oExport_LDBPDetail, Int64 nUserID)
        {
            TransactionContext tc = null;

            List<Export_LDBPDetail> _oExport_LDBPDetails = new List<Export_LDBPDetail>();
            oExport_LDBPDetail.ErrorMessage = "";
            try
            {

                tc = TransactionContext.Begin(true);
                IDataReader reader;
                reader = Export_LDBPDetailDA.InsertUpdate(tc, oExport_LDBPDetail, EnumDBOperation.Cancel, nUserID, "");
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oExport_LDBPDetail = new Export_LDBPDetail();
                    oExport_LDBPDetail = CreateObject(oReader);
                }
                reader.Close();

                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                oExport_LDBPDetail.ErrorMessage   = e.Message.Split('~')[0];
                #endregion
            }
            return oExport_LDBPDetail;
        }
        public Export_LDBPDetail Get(int nInvoicePurchaseDetailID, Int64 nUserId)
        {
            Export_LDBPDetail oAccountHead = new Export_LDBPDetail();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = Export_LDBPDetailDA.Get(tc, nInvoicePurchaseDetailID);
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
        public Export_LDBPDetail GetBy(int nExportBillID, Int64 nUserId)
        {
            Export_LDBPDetail oAccountHead = new Export_LDBPDetail();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = Export_LDBPDetailDA.GetBy(tc, nExportBillID);
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

        public List<Export_LDBPDetail> Gets(int nExport_LDBPID, Int64 nUserID)
        {
            List<Export_LDBPDetail> oInvoicePurchaseDetail = null;

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = Export_LDBPDetailDA.Gets(nExport_LDBPID, tc);
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

        public List<Export_LDBPDetail> Gets(string sSQL, Int64 nUserID)
        {
            List<Export_LDBPDetail> oInvoicePurchaseDetail = null;

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = Export_LDBPDetailDA.Gets(tc, sSQL);
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
