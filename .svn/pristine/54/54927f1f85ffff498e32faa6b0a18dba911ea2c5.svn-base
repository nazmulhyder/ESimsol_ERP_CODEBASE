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
    [Serializable]
    public class ImportInvoiceService : MarshalByRefObject, IImportInvoiceService
    {
        #region Private functions and declaration
        private ImportInvoice MapObject(NullHandler oReader)
        {
            ImportInvoice oImportInvoice = new ImportInvoice();
            oImportInvoice.ImportInvoiceID = oReader.GetInt32("ImportInvoiceID");
            oImportInvoice.ImportInvoiceNo = oReader.GetString("ImportInvoiceNo");
            oImportInvoice.FileNo = oReader.GetString("FileNo");
            oImportInvoice.DateofInvoice = oReader.GetDateTime("DateofInvoice");
            oImportInvoice.InvoiceType = (EnumImportPIType)oReader.GetInt32("InvoiceType");
            oImportInvoice.InvoiceTypeInt = oReader.GetInt32("InvoiceType");
            oImportInvoice.ImportLCID = oReader.GetInt32("ImportLCID");
            oImportInvoice.ContractorID = oReader.GetInt32("ContractorID");
            oImportInvoice.DateofReceive = oReader.GetDateTime("DateofReceive");
            oImportInvoice.Amount = oReader.GetDouble("Amount");
            oImportInvoice.Amount_LC = oReader.GetDouble("Amount_LC");
            oImportInvoice.CurrencyID = oReader.GetInt32("CurrencyID");
            oImportInvoice.DateofBankInfo = oReader.GetDateTime("DateofBankInfo");
            oImportInvoice.DateOfTakeOutDoc = oReader.GetDateTime("DateOfTakeOutDoc");
            oImportInvoice.DateofApplication = oReader.GetDateTime("DateofApplication");
            oImportInvoice.DateofNegotiation = oReader.GetDateTime("DateofNegotiation");
            oImportInvoice.DateofAcceptance = oReader.GetDateTime("DateofAcceptance");
            oImportInvoice.DateofMaturity = oReader.GetDateTime("DateofMaturity");
            oImportInvoice.Remark_Pament = oReader.GetString("Remark_Pament");
            oImportInvoice.AcceptedBy = oReader.GetInt32("AcceptedBy");
            oImportInvoice.DateofPayment = oReader.GetDateTime("DateofPayment");
            oImportInvoice.ExceedDays = oReader.GetInt32("ExceedDays");
            oImportInvoice.BankStatus = (EnumInvoiceBankStatus)oReader.GetInt32("BankStatus");
            oImportInvoice.BankStatusInt = oReader.GetInt32("BankStatus");
            oImportInvoice.InvoiceStatus = (EnumInvoiceEvent)oReader.GetInt32("InvoiceStatus");
            oImportInvoice.InvoiceStatusInt = oReader.GetInt32("InvoiceStatus");
            oImportInvoice.LCCurrentStatus = (EnumLCCurrentStatus)oReader.GetInt32("LCCurrentStatus");
            oImportInvoice.sBankStatus = oReader.GetInt32("BankStatus").ToString();
            oImportInvoice.Qty = oReader.GetDouble("Qty");
            oImportInvoice.ContractorName = oReader.GetString("ContractorName");
            oImportInvoice.Currency = oReader.GetString("Currency");
            oImportInvoice.BLNo = oReader.GetString("BLNo");
            oImportInvoice.BLDate = oReader.GetDateTime("BLDate");
            oImportInvoice.DeliveryNoticeDate = oReader.GetDateTime("DeliveryNoticeDate");
            oImportInvoice.ETADate = oReader.GetDateTime("ETADate");
            oImportInvoice.CommissionInPercent = oReader.GetDouble("CommissionInPercent");
            oImportInvoice.Commission = oReader.GetDouble("Commission");
            oImportInvoice.CommissionRemarks = oReader.GetString("CommissionRemarks");
            oImportInvoice.PaymentExist = oReader.GetBoolean("PaymentExist");
            
            //For Import Invoice Management
            oImportInvoice.Origin = oReader.GetString("Origin");
            oImportInvoice.ImportLCNo = oReader.GetString("ImportLCNo");
            oImportInvoice.ImportLCDate = oReader.GetDateTime("ImportLCDate");
            oImportInvoice.BankName_Nego = oReader.GetString("BankName_Nego");
            oImportInvoice.BankBranchID_Nego = oReader.GetInt32("BankBranchID_Nego");
            oImportInvoice.LCPaymentType = ((EnumLCPaymentType)oReader.GetInt32("LCPaymentType")).ToString();
            oImportInvoice.LCPaymentTypeInt = oReader.GetInt32("LCPaymentType");
            oImportInvoice.BUName = oReader.GetString("BUName");
            oImportInvoice.BUCode = oReader.GetString("BUCode");            
            oImportInvoice.BUID = oReader.GetInt32("BUID");            
            oImportInvoice.CCRate = oReader.GetDouble("CCRate");
            oImportInvoice.CRate_Acceptance = oReader.GetDouble("CRate_Acceptance");
            oImportInvoice.ABPNo = oReader.GetString("ABPNo");
            oImportInvoice.Tenor = oReader.GetInt32("Tenor");
            oImportInvoice.LCTermsName = oReader.GetString("LCTermsName");
            oImportInvoice.AcceptedByName = oReader.GetString("AcceptedByName");
            oImportInvoice.MUnit = oReader.GetString("MUnit");
            oImportInvoice.PaymentInstructionType = oReader.GetInt32("PaymentInstructionType");
            oImportInvoice.BaseCurrencyID = oReader.GetInt32("BaseCurrencyID");
            oImportInvoice.ShipmentDate = oReader.GetDateTime("ShipmentDate");
            oImportInvoice.CurrencyID_LC = oReader.GetInt32("CurrencyID_LC");
            //oImportInvoice.CnFName = oReader.GetString("CnFName");
            //oImportInvoice.CnFRemarks = oReader.GetString("CnFRemarks");
            oImportInvoice.PositionOuterDate = oReader.GetDateTime("PositionOuterDate");
            oImportInvoice.PositionJTDate = oReader.GetDateTime("PositionJTDate");
            oImportInvoice.AssesmentDate = oReader.GetDateTime("AssesmentDate");
            oImportInvoice.AssesmentRemarks = oReader.GetString("AssesmentRemarks");
            oImportInvoice.NotingDate = oReader.GetDateTime("NotingDate");
            oImportInvoice.NotingRemarks = oReader.GetString("NotingRemarks");
            oImportInvoice.ExamineDate = oReader.GetDateTime("ExamineDate");
            oImportInvoice.ExamineRemarks = oReader.GetString("ExamineRemarks");
            oImportInvoice.DOReceiveFromDate = oReader.GetDateTime("DOReceiveFromDate");
            oImportInvoice.DOReceiveFromRemarks = oReader.GetString("DOReceiveFromRemarks");
            oImportInvoice.BillofEntryDate = oReader.GetDateTime("BillofEntryDate");
            oImportInvoice.BillofEntryNo = oReader.GetString("BillofEntryNo");
            oImportInvoice.Remarks = oReader.GetString("Remarks");
           

            return oImportInvoice;
        }

        private ImportInvoice CreateObject(NullHandler oReader)
        {
            ImportInvoice oImportInvoice = new ImportInvoice();
            oImportInvoice = MapObject(oReader);
            return oImportInvoice;
        }

        private List<ImportInvoice> CreateObjects(IDataReader oReader)
        {
            List<ImportInvoice> oImportInvoices = new List<ImportInvoice>();
            NullHandler oHandler = new NullHandler(oReader);
            while (oReader.Read())
            {
                ImportInvoice oItem = CreateObject(oHandler);
                oImportInvoices.Add(oItem);
            }
            return oImportInvoices;
        }
        #endregion

        #region Interface implementation
        public ImportInvoiceService() { }

        #region Save Import Invoice & Import Invoice Product
        public ImportInvoice Save(ImportInvoice oImportInvoice, Int64 nUserID)
        {
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
                List<ImportInvoiceDetail> oImportInvoiceDetails = new List<ImportInvoiceDetail>();
                oImportInvoiceDetails = oImportInvoice.ImportInvoiceDetails;
                string sImportInvoiceDetailIDS = "";

                IDataReader reader;
                if (oImportInvoice.ImportInvoiceID <= 0)
                {
                    reader = ImportInvoiceDA.InsertUpdate(tc, oImportInvoice, EnumDBOperation.Insert, nUserID);
                }
                else
                {
                    VoucherDA.CheckVoucherReference(tc, "ImportInvoice", "ImportInvoiceID", oImportInvoice.ImportInvoiceID);
                    reader = ImportInvoiceDA.InsertUpdate(tc, oImportInvoice, EnumDBOperation.Update, nUserID);
                }
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oImportInvoice = CreateObject(oReader);
                }
                reader.Close();

                #region ImportInvoice Part

                foreach (ImportInvoiceDetail oItem in oImportInvoiceDetails)
                {
                    IDataReader readerdetail;
                    oItem.ImportInvoiceID = oImportInvoice.ImportInvoiceID;
                    oItem.Amount = (oItem.Qty * oItem.UnitPrice);
                    if (oItem.ImportInvoiceDetailID <= 0)
                    {
                        readerdetail = ImportInvoiceDetailDA.InsertUpdate(tc, oItem, EnumDBOperation.Insert, nUserID);
                    }
                    else
                    {
                        readerdetail = ImportInvoiceDetailDA.InsertUpdate(tc, oItem, EnumDBOperation.Update, nUserID);
                    }
                    NullHandler oReaderDetail = new NullHandler(readerdetail);
                    if (readerdetail.Read())
                    {
                        sImportInvoiceDetailIDS = sImportInvoiceDetailIDS + oReaderDetail.GetString("ImportInvoiceDetailID") + ",";
                    }
                    readerdetail.Close();
                }
                ImportInvoiceDetail oImportInvoiceDetail = new ImportInvoiceDetail();
                oImportInvoiceDetail.ImportInvoiceID = oImportInvoice.ImportInvoiceID;
                if (sImportInvoiceDetailIDS.Length > 0)
                {
                    sImportInvoiceDetailIDS = sImportInvoiceDetailIDS.Remove(sImportInvoiceDetailIDS.Length - 1, 1);
                }
                ImportInvoiceDetailDA.Delete(tc, oImportInvoiceDetail, EnumDBOperation.Delete, nUserID, sImportInvoiceDetailIDS);
                #endregion
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                oImportInvoice.ErrorMessage = e.Message.Split('~')[0];
                #endregion
            }

            return oImportInvoice;

        }
        public bool SaveDeliveryNotice(ImportInvoice oImportInvoice, Int64 nUserID)
        {
            TransactionContext tc = null;
            bool result;
            try
            {
                tc = TransactionContext.Begin(true);
                
                if (oImportInvoice.ImportInvoiceID <= 0)
                {
                    result = false;
                }
                else
                {
                    result = ImportInvoiceDA.SaveDeliveryNotice(tc, oImportInvoice, nUserID);
                }
                tc.End();
            }
            catch (Exception)
            {
                result = false;
            }
            return result;
        }
        public ImportInvoice SavePIHistory(ImportInvoice oImportInvoice, Int64 nUserID)
        {
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
                IDataReader reader;
                if (oImportInvoice.ImportInvoiceID <= 0)
                {
                    reader = ImportInvoiceDA.InsertUpdatePILCHistory(tc, oImportInvoice, EnumDBOperation.Insert, nUserID);
                }
                else
                {
                    reader = ImportInvoiceDA.InsertUpdatePILCHistory(tc, oImportInvoice, EnumDBOperation.Update, nUserID);
                }
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oImportInvoice = new ImportInvoice();
                    oImportInvoice = CreateObject(oReader);
                }
                reader.Close();
             
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                oImportInvoice = new ImportInvoice();
                oImportInvoice.ErrorMessage = e.Message.Split('~')[0];
                #endregion
            }

            return oImportInvoice;

        }
        public ImportInvoice Save_UpdateStatus(ImportInvoice oImportInvoice, Int64 nUserID)
        {

            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
                IDataReader reader;
                if (oImportInvoice.ImportInvoiceID <= 0)
                {
                    reader = ImportInvoiceDA.InsertUpdateInvoiceStatus(tc, oImportInvoice, EnumDBOperation.Insert, nUserID);
                }
                else
                {
                    reader = ImportInvoiceDA.InsertUpdateInvoiceStatus(tc, oImportInvoice, EnumDBOperation.Update, nUserID);
                }
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oImportInvoice = new ImportInvoice();
                    oImportInvoice = CreateObject(oReader);
                }
                reader.Close();

                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                oImportInvoice = new ImportInvoice();
                oImportInvoice.ErrorMessage = e.Message.Split('~')[0];
                #endregion
            }

            return oImportInvoice;

        }

        public ImportInvoice UpdateCommission(ImportInvoice oImportInvoice, Int64 nUserID)
        {

            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
                ImportInvoiceDA.UpdateCommission(tc, oImportInvoice,  nUserID);
                IDataReader reader = ImportInvoiceDA.Get(oImportInvoice.ImportInvoiceID, tc);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oImportInvoice = new ImportInvoice();
                    oImportInvoice = CreateObject(oReader);
                }
                reader.Close();

                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                oImportInvoice = new ImportInvoice();
                oImportInvoice.ErrorMessage = e.Message.Split('~')[0];
                #endregion
            }

            return oImportInvoice;

        }
        public ImportInvoice SaveAccptanceVoucher(ImportInvoice oImportInvoice, Int64 nUserID)
        {

            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
                //List<Voucher> oVouchers = new List<Voucher>();
                //List<Voucher> oTempVouchers = new List<Voucher>();
                //oVouchers = oImportInvoice.Vouchers;

                IDataReader reader;
                if (oImportInvoice.ImportInvoiceID <= 0)
                {
                    reader = ImportInvoiceDA.InsertUpdatePILCHistory(tc, oImportInvoice, EnumDBOperation.Insert, nUserID);
                }
                else
                {
                    reader = ImportInvoiceDA.InsertUpdatePILCHistory(tc, oImportInvoice, EnumDBOperation.Update, nUserID);
                }
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oImportInvoice = new ImportInvoice();
                    oImportInvoice = CreateObject(oReader);
                }
                reader.Close();
                //oTempVouchers = new VoucherService().CommitAutoVoucher(tc, oVouchers, nUserID);
                //if (oTempVouchers[0].ErrorMessage != "")
                //{
                //    throw new Exception(oTempVouchers[0].ErrorMessage);
                //}
                //oImportInvoice.Vouchers = oTempVouchers;
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                oImportInvoice = new ImportInvoice();
                oImportInvoice.ErrorMessage = e.Message.Split('~')[0];
                #endregion
            }

            return oImportInvoice;

        }

        public ImportInvoice UpdateAmount(ImportInvoice oImportInvoice, Int64 nUserId)
        {
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = ImportInvoiceDA.UpdateAmount(tc, oImportInvoice);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oImportInvoice = CreateObject(oReader);
                }
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                oImportInvoice = new ImportInvoice();
                oImportInvoice.ErrorMessage = e.Message.Split('~')[0];
                #endregion
            }
            return oImportInvoice;
        }
        public List<ImportInvoice> Gets(string sSQL, Int64 nUserID)
        {
            List<ImportInvoice> oImportInvoice = null;

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = ImportInvoiceDA.Gets(tc, sSQL);
                oImportInvoice = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get ImportInvoice", e);
                #endregion
            }

            return oImportInvoice;
        }

     

        #endregion

        #region Delete
        public String Delete(ImportInvoice oImportInvoice, Int64 nUserID)
        {
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
                VoucherDA.CheckVoucherReference(tc, "ImportInvoice", "ImportInvoiceID", oImportInvoice.ImportInvoiceID);
                ImportInvoiceDA.Delete(tc, oImportInvoice, EnumDBOperation.Delete, nUserID);
                tc.End();

            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                return e.Message;
                #endregion
            }
            return Global.DeleteMessage;
        }

        #endregion

        #region Retrive Information

        public ImportInvoice Get(int nImportInvoiceID, Int64 nUserID)
        {
            ImportInvoice oImportInvoice = new ImportInvoice();

            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = ImportInvoiceDA.Get(nImportInvoiceID, tc);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oImportInvoice = CreateObject(oReader);
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
                throw new ServiceException("Failed to Get ImportInvoice", e);
                #endregion
            }

            return oImportInvoice;
        }
        public ImportInvoice Get(int nInvoiceType, int nImportPIID, Int64 nUserID)
        {
            ImportInvoice oImportInvoice = new ImportInvoice();

            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = ImportInvoiceDA.Get( nInvoiceType,  nImportPIID, tc);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oImportInvoice = CreateObject(oReader);
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
                throw new ServiceException("Failed to Get ImportInvoice", e);
                #endregion
            }

            return oImportInvoice;
        }
        public List<ImportInvoice> Gets(int nPurchaseLCID, Int64 nUserID)
        {
            List<ImportInvoice> oImportInvoices = null;

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = ImportInvoiceDA.Gets(nPurchaseLCID, tc);
                oImportInvoices = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get ImportInvoices", e);
                #endregion
            }

            return oImportInvoices;
        }

        public List<ImportInvoice> Gets(Int64 nUserID)
        {
            List<ImportInvoice> oImportInvoices = null;

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = ImportInvoiceDA.Gets(tc);
                oImportInvoices = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get ImportInvoices", e);
                #endregion
            }

            return oImportInvoices;
        }

        #endregion



        #endregion


    }

}
