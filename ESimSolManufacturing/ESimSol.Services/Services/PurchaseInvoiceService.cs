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
    public class PurchaseInvoiceService : MarshalByRefObject, IPurchaseInvoiceService
    {
        #region Private functions and declaration
        private PurchaseInvoice MapObject(NullHandler oReader)
        {
            PurchaseInvoice oPurchaseInvoice = new PurchaseInvoice();
            oPurchaseInvoice.PurchaseInvoiceID = oReader.GetInt32("PurchaseInvoiceID");
            oPurchaseInvoice.PurchaseInvoiceNo = oReader.GetString("PurchaseInvoiceNo");
            oPurchaseInvoice.BUID = oReader.GetInt32("BUID");
            oPurchaseInvoice.BillNo = oReader.GetString("BillNo");
            oPurchaseInvoice.DateofBill = oReader.GetDateTime("DateofBill");
            oPurchaseInvoice.DateofInvoice = oReader.GetDateTime("DateofInvoice");
            oPurchaseInvoice.InvoiceType = (EnumPInvoiceType)oReader.GetInt32("InvoiceType");
            oPurchaseInvoice.InvoiceTypeInt = oReader.GetInt32("InvoiceType");
            oPurchaseInvoice.InvoicePaymentMode = (EnumInvoicePaymentMode)oReader.GetInt32("InvoicePaymentMode");
            oPurchaseInvoice.InvoicePaymentModeInt = oReader.GetInt32("InvoicePaymentMode");
            oPurchaseInvoice.InvoiceStatus = (EnumPInvoiceStatus)oReader.GetInt32("InvoiceStatus");
            oPurchaseInvoice.InvoiceStatusInt = oReader.GetInt32("InvoiceStatus");
            oPurchaseInvoice.RefType = (EnumInvoiceReferenceType)oReader.GetInt32("RefType");
            oPurchaseInvoice.RefTypeInt = oReader.GetInt32("RefType");
            //oPurchaseInvoice.RefID = oReader.GetInt32("RefID");
            oPurchaseInvoice.ContractorID = oReader.GetInt32("ContractorID");
            oPurchaseInvoice.ContractorPersonalID = oReader.GetInt32("ContractorPersonalID");
            oPurchaseInvoice.Amount = oReader.GetDouble("Amount");
            oPurchaseInvoice.CurrencyID = oReader.GetInt32("CurrencyID");
            oPurchaseInvoice.ConvertionRate = oReader.GetDouble("ConvertionRate");
            oPurchaseInvoice.DateofMaturity = oReader.GetDateTime("DateofMaturity");
            oPurchaseInvoice.Note = oReader.GetString("Note");
            oPurchaseInvoice.ApprovedBy = oReader.GetInt32("ApprovedBy");
            oPurchaseInvoice.ApprovedDate = oReader.GetDateTime("ApprovedDate");
            oPurchaseInvoice.PaymentTermID = oReader.GetInt32("PaymentTermID");
            oPurchaseInvoice.ContractorName = oReader.GetString("ContractorName");
            oPurchaseInvoice.CurrencySymbol = oReader.GetString("CurrencySymbol");
            oPurchaseInvoice.CurrencyName = oReader.GetString("CurrencyName");
            oPurchaseInvoice.CurrencyBFDP = oReader.GetString("CurrencyBFDP");
            oPurchaseInvoice.CurrencyBADP = oReader.GetString("CurrencyBADP");
            oPurchaseInvoice.RefNo = oReader.GetString("RefNo");
            oPurchaseInvoice.RefDate = oReader.GetDateTime("RefDate");
            oPurchaseInvoice.RefAmount = oReader.GetDouble("RefAmount");
            oPurchaseInvoice.BUName = oReader.GetString("BUName");
            oPurchaseInvoice.BUCode = oReader.GetString("BUCode");
            oPurchaseInvoice.ContrctorPersonalName = oReader.GetString("ContrctorPersonalName");
            oPurchaseInvoice.PrepareUserName = oReader.GetString("PrepareUserName");
            oPurchaseInvoice.ApprovedUserName = oReader.GetString("ApprovedUserName");
            oPurchaseInvoice.PaymentTermText = oReader.GetString("PaymentTermText");
            oPurchaseInvoice.AdvanceSettle = oReader.GetDouble("AdvanceSettle");
            oPurchaseInvoice.YetToGRNQty = oReader.GetDouble("YetToGRNQty");
            oPurchaseInvoice.ShipBy = oReader.GetString("ShipBy");
            oPurchaseInvoice.TradeTerm = oReader.GetString("TradeTerm");
            oPurchaseInvoice.DeliveryTo = oReader.GetInt32("DeliveryTo");
            oPurchaseInvoice.DeliveryToName = oReader.GetString("DeliveryToName");
            oPurchaseInvoice.DeliveryToContactPerson = oReader.GetInt32("DeliveryToContactPerson");
            oPurchaseInvoice.DeliveryToContactPersonName = oReader.GetString("DeliveryToContactPersonName");
            oPurchaseInvoice.BillTo = oReader.GetInt32("BillTo");
            oPurchaseInvoice.BillToName = oReader.GetString("BillToName");
            oPurchaseInvoice.BIllToContactPerson = oReader.GetInt32("BIllToContactPerson");
            oPurchaseInvoice.BIllToContactPersonName = oReader.GetString("BIllToContactPersonName");
            oPurchaseInvoice.YeToBillAmount = oReader.GetDouble("YeToBillAmount");
            oPurchaseInvoice.Discount = oReader.GetDouble("Discount");
            oPurchaseInvoice.ServiceCharge = oReader.GetDouble("ServiceCharge");
            oPurchaseInvoice.NetAmount = oReader.GetDouble("NetAmount");
            oPurchaseInvoice.RateOn = oReader.GetInt32("RateOn");
            oPurchaseInvoice.LastApprovalSequence = oReader.GetInt32("LastApprovalSequence");
            oPurchaseInvoice.ApprovalSequence = oReader.GetInt32("ApprovalSequence");
            oPurchaseInvoice.ApprovalStatus = oReader.GetString("ApprovalStatus");
            oPurchaseInvoice.IsWillVoucherEffect = oReader.GetBoolean("IsWillVoucherEffect");

            oPurchaseInvoice.PaymentMethod = (EnumPaymentMethod)oReader.GetInt32("PaymentMethod");
            oPurchaseInvoice.PaymentMethodInt = oReader.GetInt32("PaymentMethod");
            oPurchaseInvoice.BankAccountID = oReader.GetInt32("BankAccountID");
            oPurchaseInvoice.AccountNo = oReader.GetString("AccountNo");
            oPurchaseInvoice.BankShortName = oReader.GetString("BankShortName");
            oPurchaseInvoice.ServiceChargeName = oReader.GetString("ServiceChargeName");
            oPurchaseInvoice.ServiceChargeID = oReader.GetInt32("ServiceChargeID");

            
            return oPurchaseInvoice;
        }

        private PurchaseInvoice CreateObject(NullHandler oReader)
        {
            PurchaseInvoice oPurchaseInvoice = new PurchaseInvoice();
            oPurchaseInvoice = MapObject(oReader);
            return oPurchaseInvoice;
        }

        private List<PurchaseInvoice> CreateObjects(IDataReader oReader)
        {
            List<PurchaseInvoice> oPurchaseInvoices = new List<PurchaseInvoice>();
            NullHandler oHandler = new NullHandler(oReader);
            while (oReader.Read())
            {
                PurchaseInvoice oItem = CreateObject(oHandler);
                oPurchaseInvoices.Add(oItem);
            }
            return oPurchaseInvoices;
        }
        #endregion

        #region Interface implementation
        public PurchaseInvoiceService() { }

        #region Save Purchase Invoice 

        public PurchaseInvoice Save(PurchaseInvoice oPurchaseInvoice, Int64 nUserID)
        {

            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
                List<PurchaseInvoiceDetail> oPurchaseInvoiceDetails = new List<PurchaseInvoiceDetail>();                
                oPurchaseInvoiceDetails = oPurchaseInvoice.PurchaseInvoiceDetails;                
                string sPurchaseInvoiceDetailIDS = "";
               
                IDataReader reader;
                if (oPurchaseInvoice.PurchaseInvoiceID <= 0)
                {
                    reader = PurchaseInvoiceDA.InsertUpdate(tc, oPurchaseInvoice, EnumDBOperation.Insert, nUserID);
                }
                else
                {
                    VoucherDA.CheckVoucherReference(tc, "PurchaseInvoice", "PurchaseInvoiceID", oPurchaseInvoice.PurchaseInvoiceID);
                    reader = PurchaseInvoiceDA.InsertUpdate(tc, oPurchaseInvoice, EnumDBOperation.Update, nUserID);
                }
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oPurchaseInvoice = new PurchaseInvoice();
                    oPurchaseInvoice = CreateObject(oReader);
                }
                reader.Close();

                #region Purchase invoice Detail Part
                if (oPurchaseInvoiceDetails!=null)
                {
                    foreach (PurchaseInvoiceDetail oItem in oPurchaseInvoiceDetails)
                    {
                        IDataReader readerdetail;
                        oItem.PurchaseInvoiceID = oPurchaseInvoice.PurchaseInvoiceID;                       
                        if (oItem.PurchaseInvoiceDetailID <= 0)
                        {
                            readerdetail = PurchaseInvoiceDetailDA.InsertUpdate(tc, oItem, EnumDBOperation.Insert, nUserID, "");
                        }
                        else
                        {
                            readerdetail = PurchaseInvoiceDetailDA.InsertUpdate(tc, oItem, EnumDBOperation.Update, nUserID, "");
                        }
                        NullHandler oReaderDetail = new NullHandler(readerdetail);
                        if (readerdetail.Read())
                        {
                            sPurchaseInvoiceDetailIDS = sPurchaseInvoiceDetailIDS + oReaderDetail.GetString("PurchaseInvoiceDetailID") + ",";
                        }
                        readerdetail.Close();
                    }
                }
                PurchaseInvoiceDetail oPurchaseInvoiceDetail = new PurchaseInvoiceDetail();
                oPurchaseInvoiceDetail.PurchaseInvoiceID = oPurchaseInvoice.PurchaseInvoiceID;
                PurchaseInvoiceDetailDA.Delete(tc, oPurchaseInvoiceDetail, EnumDBOperation.Delete, nUserID, sPurchaseInvoiceDetailIDS);
                #endregion

                IDataReader readerdetails = null;
                readerdetails = PurchaseInvoiceDetailDA.Gets(oPurchaseInvoice.PurchaseInvoiceID, tc);
                PurchaseInvoiceDetailService obj = new PurchaseInvoiceDetailService();
                oPurchaseInvoice.PurchaseInvoiceDetails = obj.CreateObjects(readerdetails);
                readerdetails.Close();

               
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                oPurchaseInvoice = new PurchaseInvoice();
                oPurchaseInvoice.ErrorMessage = e.Message.Split('~')[0];
             
                #endregion
            }

            return oPurchaseInvoice;

        }
                
        public PurchaseInvoice Approved(PurchaseInvoice oPurchaseInvoice, Int64 nUserID)
        {

            TransactionContext tc = null;            
            try
            {
                tc = TransactionContext.Begin(true);
                IDataReader reader;
                reader = PurchaseInvoiceDA.Approve(tc, oPurchaseInvoice, nUserID);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oPurchaseInvoice = new PurchaseInvoice();
                    oPurchaseInvoice = CreateObject(oReader);
                }
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                oPurchaseInvoice = new PurchaseInvoice();
                oPurchaseInvoice.ErrorMessage = e.Message.Split('~')[0];
                
                #endregion
            }

            return oPurchaseInvoice;

        }

        public PurchaseInvoice UndoApproved(PurchaseInvoice oPurchaseInvoice, Int64 nUserID)
        {

            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
                IDataReader reader;
                reader = PurchaseInvoiceDA.UndoApproved(tc, oPurchaseInvoice, nUserID);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oPurchaseInvoice = new PurchaseInvoice();
                    oPurchaseInvoice = CreateObject(oReader);
                }
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                oPurchaseInvoice = new PurchaseInvoice();
                oPurchaseInvoice.ErrorMessage = e.Message.Split('~')[0];

                #endregion
            }

            return oPurchaseInvoice;

        }
        public PurchaseInvoice UpdatePaymentMode(PurchaseInvoice oPurchaseInvoice, Int64 nUserID)
        {

            TransactionContext tc = null;            
            try
            {
                tc = TransactionContext.Begin(true);
                PurchaseInvoiceDA.UpdatePaymentMode(tc, oPurchaseInvoice);
                IDataReader reader;
                reader = PurchaseInvoiceDA.Get( oPurchaseInvoice.PurchaseInvoiceID, tc);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oPurchaseInvoice = new PurchaseInvoice();
                    oPurchaseInvoice = CreateObject(oReader);
                }
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                oPurchaseInvoice = new PurchaseInvoice();
                oPurchaseInvoice.ErrorMessage = e.Message.Split('~')[0];
                
                #endregion
            }

            return oPurchaseInvoice;

        }

        public PurchaseInvoice UpdateVoucherEffect(PurchaseInvoice oPurchaseInvoice, Int64 nUserID)
        {

            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
                PurchaseInvoiceDA.UpdateVoucherEffect(tc, oPurchaseInvoice);
                IDataReader reader;
                reader = PurchaseInvoiceDA.Get(oPurchaseInvoice.PurchaseInvoiceID, tc);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oPurchaseInvoice = new PurchaseInvoice();
                    oPurchaseInvoice = CreateObject(oReader);
                }
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                oPurchaseInvoice = new PurchaseInvoice();
                oPurchaseInvoice.ErrorMessage = e.Message.Split('~')[0];

                #endregion
            }

            return oPurchaseInvoice;

        }
        public List<PurchaseInvoice> Gets(string sSQL, Int64 nUserID)
        {
            List<PurchaseInvoice> oPurchaseInvoice = null;

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = PurchaseInvoiceDA.Gets(tc, sSQL);
                oPurchaseInvoice = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get PurchaseInvoice", e);
                #endregion
            }

            return oPurchaseInvoice;
        }

        #endregion

        #region Delete
        public String Delete(PurchaseInvoice oPurchaseInvoice, Int64 nUserID)
        {
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);

                VoucherDA.CheckVoucherReference(tc, "PurchaseInvoice", "PurchaseInvoiceID", oPurchaseInvoice.PurchaseInvoiceID);
                PurchaseInvoiceDA.Delete(tc, oPurchaseInvoice, EnumDBOperation.Delete, nUserID);
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

        public PurchaseInvoice Get(int nPurchaseInvoiceID, Int64 nUserID)
        {
            PurchaseInvoice oPurchaseInvoice = new PurchaseInvoice();

            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = PurchaseInvoiceDA.Get(nPurchaseInvoiceID, tc);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oPurchaseInvoice = CreateObject(oReader);
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
                throw new ServiceException("Failed to Get PurchaseInvoice", e);
                #endregion
            }

            return oPurchaseInvoice;
        }
        public PurchaseInvoice Get(int nRefID, int RefType, Int64 nUserID)
        {
            PurchaseInvoice oPurchaseInvoice = new PurchaseInvoice();

            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = PurchaseInvoiceDA.Get( nRefID,  RefType, tc);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oPurchaseInvoice = CreateObject(oReader);
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
                throw new ServiceException("Failed to Get PurchaseInvoice", e);
                #endregion
            }

            return oPurchaseInvoice;
        }

        public List<PurchaseInvoice> Gets(int nPurchaseLCID, Int64 nUserID)
        {
            List<PurchaseInvoice> oPurchaseInvoices = null;

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = PurchaseInvoiceDA.Gets(nPurchaseLCID, tc);
                oPurchaseInvoices = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get PurchaseInvoices", e);
                #endregion
            }

            return oPurchaseInvoices;
        }      

        public List<PurchaseInvoice> Gets(Int64 nUserID)
        {
            List<PurchaseInvoice> oPurchaseInvoices = null;

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = PurchaseInvoiceDA.Gets(tc);
                oPurchaseInvoices = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get PurchaseInvoices", e);
                #endregion
            }

            return oPurchaseInvoices;
        }       
        #endregion
        #endregion
    }

}
