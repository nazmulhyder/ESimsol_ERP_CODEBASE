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
    public class TradingDeliveryChallanService : MarshalByRefObject, ITradingDeliveryChallanService
    {
        #region Private functions and declaration
        private TradingDeliveryChallan MapObject(NullHandler oReader)
        {
            TradingDeliveryChallan oTradingDeliveryChallan = new TradingDeliveryChallan();
            oTradingDeliveryChallan.TradingDeliveryChallanID = oReader.GetInt32("TradingDeliveryChallanID");
            oTradingDeliveryChallan.BUID = oReader.GetInt32("BUID");
            oTradingDeliveryChallan.TradingSaleInvoiceID = oReader.GetInt32("TradingSaleInvoiceID");
            oTradingDeliveryChallan.ChallanNo = oReader.GetString("ChallanNo");
            oTradingDeliveryChallan.ChallanDate = oReader.GetDateTime("ChallanDate");
            oTradingDeliveryChallan.BuyerID = oReader.GetInt32("BuyerID");
            oTradingDeliveryChallan.StoreID = oReader.GetInt32("StoreID");
            oTradingDeliveryChallan.CurrencyID = oReader.GetInt32("CurrencyID");
            oTradingDeliveryChallan.NetAmount = oReader.GetDouble("NetAmount");
            oTradingDeliveryChallan.TotalQty = oReader.GetDouble("TotalQty");
            oTradingDeliveryChallan.Note = oReader.GetString("Note");
            oTradingDeliveryChallan.DeliveryBy = oReader.GetInt32("DeliveryBy");
            oTradingDeliveryChallan.DeliveryByName = oReader.GetString("DeliveryByName");
            oTradingDeliveryChallan.BUName = oReader.GetString("BUName");
            oTradingDeliveryChallan.BuyerName = oReader.GetString("BuyerName");
            oTradingDeliveryChallan.StoreName = oReader.GetString("StoreName");
            oTradingDeliveryChallan.CurrencyName = oReader.GetString("CurrencyName");
            oTradingDeliveryChallan.CurrencySymbol = oReader.GetString("CurrencySymbol");
   
            oTradingDeliveryChallan.InvoiceNo = oReader.GetString("InvoiceNo");
            return oTradingDeliveryChallan;
        }

        private TradingDeliveryChallan CreateObject(NullHandler oReader)
        {
            TradingDeliveryChallan oTradingDeliveryChallan = new TradingDeliveryChallan();
            oTradingDeliveryChallan = MapObject(oReader);
            return oTradingDeliveryChallan;
        }

        private List<TradingDeliveryChallan> CreateObjects(IDataReader oReader)
        {
            List<TradingDeliveryChallan> oTradingDeliveryChallans = new List<TradingDeliveryChallan>();
            NullHandler oHandler = new NullHandler(oReader);
            while (oReader.Read())
            {
                TradingDeliveryChallan oItem = CreateObject(oHandler);
                oTradingDeliveryChallans.Add(oItem);
            }
            return oTradingDeliveryChallans;
        }
        #endregion

        #region Interface implementation
        public TradingDeliveryChallanService() { }
        public TradingDeliveryChallan Save(TradingDeliveryChallan oTradingDeliveryChallan, int nUserID)
        {
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
                List<TradingDeliveryChallanDetail> oTradingDeliveryChallanDetails = new List<TradingDeliveryChallanDetail>();
                TradingDeliveryChallanDetail oTradingDeliveryChallanDetail = new TradingDeliveryChallanDetail();

                oTradingDeliveryChallanDetails = oTradingDeliveryChallan.TradingDeliveryChallanDetails;
                string sTradingDeliveryChallanDetailIDs = "";

                #region TradingDeliveryChallan part
                IDataReader reader;
                if (oTradingDeliveryChallan.TradingDeliveryChallanID <= 0)
                {
                    AuthorizationRoleDA.CheckUserPermission(tc, nUserID, EnumModuleName.TradingDeliveryChallan, EnumRoleOperationType.Add);
                    reader = TradingDeliveryChallanDA.InsertUpdate(tc, oTradingDeliveryChallan, EnumDBOperation.Insert, nUserID);
                }
                else
                {
                    AuthorizationRoleDA.CheckUserPermission(tc, nUserID, EnumModuleName.TradingDeliveryChallan, EnumRoleOperationType.Edit);
                    reader = TradingDeliveryChallanDA.InsertUpdate(tc, oTradingDeliveryChallan, EnumDBOperation.Update, nUserID);
                }

                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oTradingDeliveryChallan = new TradingDeliveryChallan();
                    oTradingDeliveryChallan = CreateObject(oReader);
                }
                reader.Close();
                #endregion

                #region Valid TradingDeliveryChallan
                if (oTradingDeliveryChallan.TradingDeliveryChallanID <= 0)
                {
                    throw new Exception("Invalid TradingDeliveryChallan!");
                }
                #endregion

                #region Purchase Invoice Detail Part
                if (oTradingDeliveryChallanDetails != null)
                {
                    foreach (TradingDeliveryChallanDetail oItem in oTradingDeliveryChallanDetails)
                    {
                        IDataReader readerdetail;
                        oItem.TradingDeliveryChallanID = oTradingDeliveryChallan.TradingDeliveryChallanID;
                        if (oItem.TradingDeliveryChallanDetailID <= 0)
                        {
                            readerdetail = TradingDeliveryChallanDetailDA.InsertUpdate(tc, oItem, EnumDBOperation.Insert, nUserID);
                        }
                        else
                        {
                            readerdetail = TradingDeliveryChallanDetailDA.InsertUpdate(tc, oItem, EnumDBOperation.Update, nUserID);
                        }
                        NullHandler oReaderDetail = new NullHandler(readerdetail);
                        if (readerdetail.Read())
                        {
                            sTradingDeliveryChallanDetailIDs = sTradingDeliveryChallanDetailIDs + oReaderDetail.GetString("TradingDeliveryChallanDetailID") + ",";
                        }
                        readerdetail.Close();
                    }
                    if (sTradingDeliveryChallanDetailIDs.Length > 0)
                    {
                        sTradingDeliveryChallanDetailIDs = sTradingDeliveryChallanDetailIDs.Remove(sTradingDeliveryChallanDetailIDs.Length - 1, 1);
                    }
                    oTradingDeliveryChallanDetail = new TradingDeliveryChallanDetail();
                    oTradingDeliveryChallanDetail.TradingDeliveryChallanID = oTradingDeliveryChallan.TradingDeliveryChallanID;
                    TradingDeliveryChallanDetailDA.Delete(tc, oTradingDeliveryChallanDetail, EnumDBOperation.Delete, nUserID, sTradingDeliveryChallanDetailIDs);
                }

                #endregion

                #region Get TradingDeliveryChallan
                reader = TradingDeliveryChallanDA.Get(tc, oTradingDeliveryChallan.TradingDeliveryChallanID);
                oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oTradingDeliveryChallan = CreateObject(oReader);
                }
                reader.Close();
                #endregion
                //#region Get TradingDeliveryChallan
                //reader = TradingSaleInvoiceDA.Get(tc, oTradingDeliveryChallan.TradingSaleInvoiceID);
                //oReader = new NullHandler(reader);
                //if (reader.Read())
                //{
                //    oTradingDeliveryChallan = CreateObject(oReader);
                //}
                //reader.Close();
                //#endregion

                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                oTradingDeliveryChallan = new TradingDeliveryChallan();
                oTradingDeliveryChallan.ErrorMessage = e.Message.Split('~')[0];
                #endregion
            }
            return oTradingDeliveryChallan;
        }
        public TradingDeliveryChallan Disburse(TradingDeliveryChallan oTradingDeliveryChallan, int nUserID)
        {
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
                IDataReader reader;
                AuthorizationRoleDA.CheckUserPermission(tc, nUserID, EnumModuleName.TradingDeliveryChallan, EnumRoleOperationType.Disburse);
                reader = TradingDeliveryChallanDA.Disburse(tc, oTradingDeliveryChallan, nUserID);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oTradingDeliveryChallan = CreateObject(oReader);
                }
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                oTradingDeliveryChallan = new TradingDeliveryChallan();
                oTradingDeliveryChallan.ErrorMessage = e.Message.Split('~')[0];
                #endregion
            }
            return oTradingDeliveryChallan;
        }
        public string Delete(TradingDeliveryChallan oTradingDeliveryChallan, int nUserID)
        {
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
                TradingDeliveryChallanDA.Delete(tc, oTradingDeliveryChallan, EnumDBOperation.Delete, nUserID);
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

        public TradingDeliveryChallan Get(int nTradingDeliveryChallanID, int nUserID)
        {
            TradingDeliveryChallan oTradingDeliveryChallan = new TradingDeliveryChallan();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = TradingDeliveryChallanDA.Get(tc, nTradingDeliveryChallanID);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oTradingDeliveryChallan = CreateObject(oReader);
                }
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                throw new Exception("Failed to Get TradingDeliveryChallan \n" + e.Message, e);
                #endregion
            }
            return oTradingDeliveryChallan;
        }

        public List<TradingDeliveryChallan> Gets(int nUserID)
        {
            List<TradingDeliveryChallan> oTradingDeliveryChallans = null;
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = TradingDeliveryChallanDA.Gets(tc);
                oTradingDeliveryChallans = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                throw new Exception("Failed to Get TradingDeliveryChallan \n" + e.Message, e);
                #endregion
            }

            return oTradingDeliveryChallans;
        }

        public List<TradingDeliveryChallan> GetsInitialTradingDeliveryChallans(int nBUID, int nUserID)
        {
            List<TradingDeliveryChallan> oTradingDeliveryChallans = new List<TradingDeliveryChallan>();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();
                IDataReader reader = null;
                reader = TradingDeliveryChallanDA.GetsInitialTradingDeliveryChallans(tc, nBUID);
                oTradingDeliveryChallans = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                throw new Exception("Failed to Get TradingDeliveryChallan \n" + e.Message, e);
                #endregion
            }

            return oTradingDeliveryChallans;
        }

        public List<TradingDeliveryChallan> Gets(string sSQL, int nUserID)
        {
            List<TradingDeliveryChallan> oTradingDeliveryChallans = null;

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = TradingDeliveryChallanDA.Gets(tc, sSQL);
                oTradingDeliveryChallans = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                throw new Exception("Failed to Get TradingDeliveryChallans \n" + e.Message, e);
                #endregion
            }
            return oTradingDeliveryChallans;
        }
        #endregion
    }
}
