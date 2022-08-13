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
    public class TradingSaleInvoiceService : MarshalByRefObject, ITradingSaleInvoiceService
    {
        #region Private functions and declaration
        private TradingSaleInvoice MapObject(NullHandler oReader)
        {
            TradingSaleInvoice oTradingSaleInvoice = new TradingSaleInvoice();
            oTradingSaleInvoice.TradingSaleInvoiceID = oReader.GetInt32("TradingSaleInvoiceID");
            oTradingSaleInvoice.TradingSaleInvoiceLogID = oReader.GetInt32("TradingSaleInvoiceLogID");
            oTradingSaleInvoice.BUID = oReader.GetInt32("BUID");
            oTradingSaleInvoice.TradingSaleInvoiceStatus = (EnumTradingSaleInvoiceStatus)oReader.GetInt16("TradingSaleInvoiceStatus");
            oTradingSaleInvoice.SalesType = (EnumSalesType)oReader.GetInt32("SalesType");
            oTradingSaleInvoice.SalesTypeInt = oReader.GetInt32("SalesType");
            oTradingSaleInvoice.InvoiceNo = oReader.GetString("InvoiceNo");
            oTradingSaleInvoice.InvoiceDate = oReader.GetDateTime("InvoiceDate");
            oTradingSaleInvoice.BuyerID = oReader.GetInt32("BuyerID");
            oTradingSaleInvoice.ContactPersonID = oReader.GetInt32("ContactPersonID");
            oTradingSaleInvoice.CurrencyID = oReader.GetInt32("CurrencyID");
            oTradingSaleInvoice.Note = oReader.GetString("Note");
            oTradingSaleInvoice.ApprovedBy = oReader.GetInt32("ApprovedBy");
            oTradingSaleInvoice.GrossAmount = oReader.GetDouble("GrossAmount");
            oTradingSaleInvoice.DiscountAmount = oReader.GetDouble("DiscountAmount");
            oTradingSaleInvoice.VatAmount = oReader.GetDouble("VatAmount");
            oTradingSaleInvoice.ServiceCharge = oReader.GetDouble("ServiceCharge");
            oTradingSaleInvoice.CommissionAmount = oReader.GetDouble("CommissionAmount");
            oTradingSaleInvoice.NetAmount = oReader.GetDouble("NetAmount");
            oTradingSaleInvoice.CardNo = oReader.GetString("CardNo");
            oTradingSaleInvoice.CardPaid = oReader.GetDouble("CardPaid");
            oTradingSaleInvoice.CashPaid = oReader.GetDouble("CashPaid");
            oTradingSaleInvoice.ATMCardID = oReader.GetInt32("ATMCardID");
            oTradingSaleInvoice.IsChallanExist = oReader.GetBoolean("IsChallanExist");
            oTradingSaleInvoice.BUName = oReader.GetString("BUName");
            oTradingSaleInvoice.BuyerName = oReader.GetString("BuyerName");
            oTradingSaleInvoice.BuyerAddress = oReader.GetString("BuyerAddress");
            oTradingSaleInvoice.BuyerPhone = oReader.GetString("BuyerPhone");
            oTradingSaleInvoice.ContactPersonName = oReader.GetString("ContactPersonName");
            oTradingSaleInvoice.ContactPersonPhone = oReader.GetString("ContactPersonPhone");
            oTradingSaleInvoice.CurrencyName = oReader.GetString("CurrencyName");
            oTradingSaleInvoice.CurrencySymbol = oReader.GetString("CurrencySymbol");
            oTradingSaleInvoice.ApprovedByName = oReader.GetString("ApprovedByName");
            oTradingSaleInvoice.YetToChallanQty = oReader.GetDouble("YetToChallanQty");
            oTradingSaleInvoice.DueAmount = oReader.GetDouble("DueAmount");
            return oTradingSaleInvoice;
        }

        private TradingSaleInvoice CreateObject(NullHandler oReader)
        {
            TradingSaleInvoice oTradingSaleInvoice = new TradingSaleInvoice();
            oTradingSaleInvoice = MapObject(oReader);
            return oTradingSaleInvoice;
        }

        private List<TradingSaleInvoice> CreateObjects(IDataReader oReader)
        {
            List<TradingSaleInvoice> oTradingSaleInvoices = new List<TradingSaleInvoice>();
            NullHandler oHandler = new NullHandler(oReader);
            while (oReader.Read())
            {
                TradingSaleInvoice oItem = CreateObject(oHandler);
                oTradingSaleInvoices.Add(oItem);
            }
            return oTradingSaleInvoices;
        }
        #endregion

        #region Interface implementation
        public TradingSaleInvoiceService() { }

        public TradingSaleInvoice Save(TradingSaleInvoice oTradingSaleInvoice, int nUserID)
        {
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
                List<TradingSaleInvoiceDetail> oTradingSaleInvoiceDetails = new List<TradingSaleInvoiceDetail>();
                TradingSaleInvoiceDetail oTradingSaleInvoiceDetail = new TradingSaleInvoiceDetail();

                oTradingSaleInvoiceDetails = oTradingSaleInvoice.TradingSaleInvoiceDetails;
                string sTradingSaleInvoiceDetailIDs = "";

                #region TradingSaleInvoice part
                IDataReader reader;
                if (oTradingSaleInvoice.TradingSaleInvoiceID <= 0)
                {
                    AuthorizationRoleDA.CheckUserPermission(tc, nUserID, EnumModuleName.TradingSaleInvoice, EnumRoleOperationType.Add);
                    reader = TradingSaleInvoiceDA.InsertUpdate(tc, oTradingSaleInvoice, EnumDBOperation.Insert, nUserID);
                }
                else
                {
                    AuthorizationRoleDA.CheckUserPermission(tc, nUserID, EnumModuleName.TradingSaleInvoice, EnumRoleOperationType.Edit);
                    reader = TradingSaleInvoiceDA.InsertUpdate(tc, oTradingSaleInvoice, EnumDBOperation.Update, nUserID);
                }

                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oTradingSaleInvoice = new TradingSaleInvoice();
                    oTradingSaleInvoice = CreateObject(oReader);
                }
                reader.Close();
                #endregion

                #region Valid TradingSaleInvoice
                if (oTradingSaleInvoice.TradingSaleInvoiceID <= 0)
                {
                    throw new Exception("Invalid TradingSaleInvoice!");
                }
                #endregion

                #region Purchase Invoice Detail Part
                if (oTradingSaleInvoiceDetails != null)
                {
                    foreach (TradingSaleInvoiceDetail oItem in oTradingSaleInvoiceDetails)
                    {
                        IDataReader readerdetail;
                        oItem.TradingSaleInvoiceID = oTradingSaleInvoice.TradingSaleInvoiceID;
                        if (oItem.TradingSaleInvoiceDetailID <= 0)
                        {
                            readerdetail = TradingSaleInvoiceDetailDA.InsertUpdate(tc, oItem, EnumDBOperation.Insert, nUserID);
                        }
                        else
                        {
                            readerdetail = TradingSaleInvoiceDetailDA.InsertUpdate(tc, oItem, EnumDBOperation.Update, nUserID);
                        }
                        NullHandler oReaderDetail = new NullHandler(readerdetail);
                        if (readerdetail.Read())
                        {
                            sTradingSaleInvoiceDetailIDs = sTradingSaleInvoiceDetailIDs + oReaderDetail.GetString("TradingSaleInvoiceDetailID") + ",";
                        }
                        readerdetail.Close();
                    }
                    if (sTradingSaleInvoiceDetailIDs.Length > 0)
                    {
                        sTradingSaleInvoiceDetailIDs = sTradingSaleInvoiceDetailIDs.Remove(sTradingSaleInvoiceDetailIDs.Length - 1, 1);
                    }
                    oTradingSaleInvoiceDetail = new TradingSaleInvoiceDetail();
                    oTradingSaleInvoiceDetail.TradingSaleInvoiceID = oTradingSaleInvoice.TradingSaleInvoiceID;
                    TradingSaleInvoiceDetailDA.Delete(tc, oTradingSaleInvoiceDetail, EnumDBOperation.Delete, nUserID, sTradingSaleInvoiceDetailIDs);
                }

                #endregion

                if (oTradingSaleInvoice.SalesType == EnumSalesType.CashSale)
                {
                    oTradingSaleInvoice.ApprovedBy = nUserID;
                    reader = TradingSaleInvoiceDA.ApprovedCashSale(tc, oTradingSaleInvoice.TradingSaleInvoiceID, nUserID);
                    oReader = new NullHandler(reader);
                    if (reader.Read())
                    {
                        oTradingSaleInvoice = CreateObject(oReader);
                    }
                    reader.Close();
                }
                else
                {
                    reader = TradingSaleInvoiceDA.Get(tc, oTradingSaleInvoice.TradingSaleInvoiceID);
                    oReader = new NullHandler(reader);
                    if (reader.Read())
                    {
                        oTradingSaleInvoice = CreateObject(oReader);
                    }
                    reader.Close();
                }

                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                oTradingSaleInvoice = new TradingSaleInvoice();
                oTradingSaleInvoice.ErrorMessage = e.Message.Split('~')[0];
                #endregion
            }
            return oTradingSaleInvoice;
        }

        public TradingSaleInvoice Approved(TradingSaleInvoice oTradingSaleInvoice, int nUserID)
        {
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);

                #region Valid TradingSaleInvoice
                if (oTradingSaleInvoice.TradingSaleInvoiceID <= 0)
                {
                    throw new Exception("Invalid TradingSaleInvoice!");
                }
                if (oTradingSaleInvoice.ApprovedBy != 0)
                {
                    throw new Exception("Your Selected Purchase Invoice Already Approved!");
                }
                #endregion

                if (oTradingSaleInvoice.SalesType == EnumSalesType.CashSale)
                {
                    #region Approved PurchaseInvocie
                    AuthorizationRoleDA.CheckUserPermission(tc, nUserID, EnumModuleName.TradingSaleInvoice, EnumRoleOperationType.Approved);
                    IDataReader reader;
                    reader = TradingSaleInvoiceDA.ApprovedCashSale(tc, oTradingSaleInvoice.TradingSaleInvoiceID, nUserID);
                    NullHandler oReader = new NullHandler(reader);
                    if (reader.Read())
                    {
                        oTradingSaleInvoice = CreateObject(oReader);
                    }
                    reader.Close();
                    #endregion
                }
                else
                {

                    #region Approved PurchaseInvocie
                    AuthorizationRoleDA.CheckUserPermission(tc, nUserID, EnumModuleName.TradingSaleInvoice, EnumRoleOperationType.Approved);
                    TradingSaleInvoiceDA.Approved(tc, oTradingSaleInvoice, nUserID);
                    #endregion

                    #region Get TradingSaleInvoice
                    IDataReader reader;
                    reader = TradingSaleInvoiceDA.Get(tc, oTradingSaleInvoice.TradingSaleInvoiceID);
                    NullHandler oReader = new NullHandler(reader);
                    if (reader.Read())
                    {
                        oTradingSaleInvoice = CreateObject(oReader);
                    }
                    reader.Close();
                    #endregion
                }
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                oTradingSaleInvoice = new TradingSaleInvoice();
                string Message = "";
                Message = e.Message;
                Message = Message.Split('!')[0];
                oTradingSaleInvoice.ErrorMessage = Message;

                #endregion
            }
            return oTradingSaleInvoice;
        }

        public TradingSaleInvoice UndoApproved(TradingSaleInvoice oTradingSaleInvoice, int nUserID)
        {
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);

                    #region Approved PurchaseInvocie
                    AuthorizationRoleDA.CheckUserPermission(tc, nUserID, EnumModuleName.TradingSaleInvoice, EnumRoleOperationType.Approved);
                    TradingSaleInvoiceDA.UndoApproved(tc, oTradingSaleInvoice, nUserID);
                    #endregion

                    #region Get TradingSaleInvoice
                    IDataReader reader;
                    reader = TradingSaleInvoiceDA.Get(tc, oTradingSaleInvoice.TradingSaleInvoiceID);
                    NullHandler oReader = new NullHandler(reader);
                    if (reader.Read())
                    {
                        oTradingSaleInvoice = CreateObject(oReader);
                    }
                    reader.Close();
                    #endregion
                
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                oTradingSaleInvoice = new TradingSaleInvoice();
                string Message = "";
                Message = e.Message;
                Message = Message.Split('!')[0];
                oTradingSaleInvoice.ErrorMessage = Message;

                #endregion
            }
            return oTradingSaleInvoice;
        }

        public string Delete(TradingSaleInvoice oTradingSaleInvoice, int nUserID)
        {
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
                AuthorizationRoleDA.CheckUserPermission(tc, nUserID, EnumModuleName.TradingSaleInvoice, EnumRoleOperationType.Delete);
                TradingSaleInvoiceDA.Delete(tc, oTradingSaleInvoice, EnumDBOperation.Delete, nUserID);
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                return e.Message;
                //throw new Exception(e.Message, e);
                #endregion
            }
            return "Data delete successfully";
        }

        public TradingSaleInvoice Get(int nTradingSaleInvoiceID, int nUserID)
        {
            TradingSaleInvoice oTradingSaleInvoice = new TradingSaleInvoice();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = TradingSaleInvoiceDA.Get(tc, nTradingSaleInvoiceID);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oTradingSaleInvoice = CreateObject(oReader);
                }
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                throw new Exception("Failed to Get TradingSaleInvoice \n" + e.Message, e);
                #endregion
            }
            return oTradingSaleInvoice;
        }

        public TradingSaleInvoice GetLog(int nTradingSaleInvoiceLogID, int nUserID)
        {
            TradingSaleInvoice oTradingSaleInvoice = new TradingSaleInvoice();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = TradingSaleInvoiceDA.GetLog(tc, nTradingSaleInvoiceLogID);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oTradingSaleInvoice = CreateObject(oReader);
                }
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                throw new Exception("Failed to Get TradingSaleInvoice \n" + e.Message, e);
                #endregion
            }
            return oTradingSaleInvoice;
        }
        public List<TradingSaleInvoice> Gets(int nUserID)
        {
            List<TradingSaleInvoice> oTradingSaleInvoices = null;
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = TradingSaleInvoiceDA.Gets(tc);
                oTradingSaleInvoices = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                throw new Exception("Failed to Get TradingSaleInvoice \n" + e.Message, e);
                #endregion
            }

            return oTradingSaleInvoices;
        }

        public List<TradingSaleInvoice> Gets(string sSQL, int nUserID)
        {
            List<TradingSaleInvoice> oTradingSaleInvoices = null;

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = TradingSaleInvoiceDA.Gets(tc, sSQL);
                oTradingSaleInvoices = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                throw new Exception("Failed to Get TradingSaleInvoices \n" + e.Message, e);
                #endregion
            }
            return oTradingSaleInvoices;
        }
        public List<TradingSaleInvoice> GetsInitialInvoices(int nBUID, int nUserID)
        {
            List<TradingSaleInvoice> oTradingSaleInvoices = new List<TradingSaleInvoice>();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = TradingSaleInvoiceDA.GetsInitialInvoices(tc, nBUID);
                oTradingSaleInvoices = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                throw new Exception("Failed to Get TradingSaleInvoices \n" + e.Message, e);
                #endregion
            }
            return oTradingSaleInvoices;
        }


        public TradingSaleInvoice RequestRevise(TradingSaleInvoice oTradingSaleInvoice, Int64 nUserID)
        {
            ReviseRequest oReviseRequest = new ReviseRequest();
            oReviseRequest = oTradingSaleInvoice.ReviseRequest;
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
                IDataReader reader;
                reader = TradingSaleInvoiceDA.RequestRevise(tc, oTradingSaleInvoice, EnumDBOperation.Insert, nUserID);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oTradingSaleInvoice = new TradingSaleInvoice();
                    oTradingSaleInvoice = CreateObject(oReader);
                }
                reader.Close();


                if (oReviseRequest != null)
                {
                    if (oTradingSaleInvoice.TradingSaleInvoiceStatus == EnumTradingSaleInvoiceStatus.Request_For_Revise && (oReviseRequest.RequestBy != 0 && oReviseRequest.RequestTo != 0))
                    {
                        IDataReader ReviseRequestreader;
                        ReviseRequestreader = ReviseRequestDA.InsertUpdate(tc, oReviseRequest, EnumDBOperation.Insert, nUserID);
                        if (ReviseRequestreader.Read())
                        {

                        }
                        ReviseRequestreader.Close();
                    }
                }
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                string Message = "";
                Message = e.Message;
                Message = Message.Split('!')[0];
                oTradingSaleInvoice.ErrorMessage = Message;
                //ExceptionLog.Write(e);
                //throw new ServiceException("Failed to Save OrderSheetDetail. Because of " + e.Message, e);
                #endregion
            }
            return oTradingSaleInvoice;
        }

        public TradingSaleInvoice AcceptRevise(TradingSaleInvoice oTradingSaleInvoice, int nUserID)
        {
            TransactionContext tc = null;
            string sTradingSaleInvoiceDetailIDs = "";
            List<TradingSaleInvoiceDetail> oTradingSaleInvoiceDetails = new List<TradingSaleInvoiceDetail>();
            TradingSaleInvoiceDetail oTradingSaleInvoiceDetail = new TradingSaleInvoiceDetail();
            oTradingSaleInvoiceDetails = oTradingSaleInvoice.TradingSaleInvoiceDetails;
            try
            {
                tc = TransactionContext.Begin(true);
                IDataReader reader;
                reader = TradingSaleInvoiceDA.AcceptRevise(tc, oTradingSaleInvoice, EnumDBOperation.Insert, nUserID);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oTradingSaleInvoice = new TradingSaleInvoice();
                    oTradingSaleInvoice = CreateObject(oReader);
                }
                reader.Close();
                #region Order Sheet Detail Part
                foreach (TradingSaleInvoiceDetail oItem in oTradingSaleInvoiceDetails)
                {
                    IDataReader readerdetail;
                    oItem.TradingSaleInvoiceID = oTradingSaleInvoice.TradingSaleInvoiceID;
                    if (oItem.TradingSaleInvoiceDetailID <= 0)
                    {
                        readerdetail = TradingSaleInvoiceDetailDA.InsertUpdate(tc, oItem, EnumDBOperation.Insert, nUserID);
                    }
                    else
                    {
                        readerdetail = TradingSaleInvoiceDetailDA.InsertUpdate(tc, oItem, EnumDBOperation.Update, nUserID);
                    }
                    NullHandler oReaderDetail = new NullHandler(readerdetail);
                    if (readerdetail.Read())
                    {
                        sTradingSaleInvoiceDetailIDs = sTradingSaleInvoiceDetailIDs + oReaderDetail.GetString("TradingSaleInvoiceDetailID") + ",";
                    }
                    readerdetail.Close();
                }
                if (sTradingSaleInvoiceDetailIDs.Length > 0)
                {
                    sTradingSaleInvoiceDetailIDs = sTradingSaleInvoiceDetailIDs.Remove(sTradingSaleInvoiceDetailIDs.Length - 1, 1);
                }
                oTradingSaleInvoiceDetail = new TradingSaleInvoiceDetail();
                oTradingSaleInvoiceDetail.TradingSaleInvoiceID = oTradingSaleInvoice.TradingSaleInvoiceID;
                TradingSaleInvoiceDetailDA.Delete(tc, oTradingSaleInvoiceDetail, EnumDBOperation.Delete, nUserID, sTradingSaleInvoiceDetailIDs);
                #endregion

                #region Get Order Sheet for Actual Order Qty
                reader = TradingSaleInvoiceDA.Get(tc, oTradingSaleInvoice.TradingSaleInvoiceID);
                oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oTradingSaleInvoice = new TradingSaleInvoice();
                    oTradingSaleInvoice = CreateObject(oReader);
                }
                reader.Close();
                #endregion

                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                {
                    tc.HandleError();
                    oTradingSaleInvoice = new TradingSaleInvoice();
                    oTradingSaleInvoice.ErrorMessage = e.Message.Split('!')[0];
                }
                #endregion
            }
            return oTradingSaleInvoice;
        }

        #endregion
    }
}
