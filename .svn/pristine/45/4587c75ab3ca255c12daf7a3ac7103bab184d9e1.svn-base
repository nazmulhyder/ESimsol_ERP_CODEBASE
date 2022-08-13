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
    public class TradingSaleReturnService : MarshalByRefObject, ITradingSaleReturnService
    {
        #region Private functions and declaration
        private TradingSaleReturn MapObject(NullHandler oReader)
        {
            TradingSaleReturn oTradingSaleReturn = new TradingSaleReturn();
            oTradingSaleReturn.TradingSaleReturnID = oReader.GetInt32("TradingSaleReturnID");
            oTradingSaleReturn.BUID = oReader.GetInt32("BUID");
            oTradingSaleReturn.ReturnNo = oReader.GetString("ReturnNo");
            oTradingSaleReturn.ReturnDate = oReader.GetDateTime("ReturnDate");
            oTradingSaleReturn.BuyerID = oReader.GetInt32("BuyerID");
            oTradingSaleReturn.ContactPersonID = oReader.GetInt32("ContactPersonID");
            oTradingSaleReturn.CurrencyID = oReader.GetInt32("CurrencyID");            
            oTradingSaleReturn.Note = oReader.GetString("Note");
            oTradingSaleReturn.ApprovedBy = oReader.GetInt32("ApprovedBy");
            oTradingSaleReturn.GrossAmount = oReader.GetDouble("GrossAmount");
            oTradingSaleReturn.PaymentAmount = oReader.GetDouble("PaymentAmount");
            oTradingSaleReturn.StoreID = oReader.GetInt32("StoreID");
            oTradingSaleReturn.BUName = oReader.GetString("BUName");
            oTradingSaleReturn.BuyerName = oReader.GetString("BuyerName");
            oTradingSaleReturn.BuyerAddress = oReader.GetString("BuyerAddress");
            oTradingSaleReturn.BuyerPhone = oReader.GetString("BuyerPhone");
            oTradingSaleReturn.ContactPersonName = oReader.GetString("ContactPersonName");
            oTradingSaleReturn.ContactPersonPhone = oReader.GetString("ContactPersonPhone");
            oTradingSaleReturn.CurrencyName = oReader.GetString("CurrencyName");
            oTradingSaleReturn.CurrencySymbol = oReader.GetString("CurrencySymbol");
            oTradingSaleReturn.ApprovedByName = oReader.GetString("ApprovedByName");
            oTradingSaleReturn.StoreName = oReader.GetString("StoreName");
            oTradingSaleReturn.DueAmount = oReader.GetDouble("DueAmount");
            return oTradingSaleReturn;
        }

        private TradingSaleReturn CreateObject(NullHandler oReader)
        {
            TradingSaleReturn oTradingSaleReturn = new TradingSaleReturn();
            oTradingSaleReturn = MapObject(oReader);
            return oTradingSaleReturn;
        }

        private List<TradingSaleReturn> CreateObjects(IDataReader oReader)
        {
            List<TradingSaleReturn> oTradingSaleReturns = new List<TradingSaleReturn>();
            NullHandler oHandler = new NullHandler(oReader);
            while (oReader.Read())
            {
                TradingSaleReturn oItem = CreateObject(oHandler);
                oTradingSaleReturns.Add(oItem);
            }
            return oTradingSaleReturns;
        }
        #endregion

        #region Interface implementation
        public TradingSaleReturnService() { }

        public TradingSaleReturn Save(TradingSaleReturn oTradingSaleReturn, int nUserID)
        {
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
                List<TradingSaleReturnDetail> oTradingSaleReturnDetails = new List<TradingSaleReturnDetail>();
                TradingSaleReturnDetail oTradingSaleReturnDetail = new TradingSaleReturnDetail();

                oTradingSaleReturnDetails = oTradingSaleReturn.TradingSaleReturnDetails;
                string sTradingSaleReturnDetailIDs = "";

                #region TradingSaleReturn part
                IDataReader reader;
                if (oTradingSaleReturn.TradingSaleReturnID <= 0)
                {
                    AuthorizationRoleDA.CheckUserPermission(tc, nUserID, EnumModuleName.TradingSaleReturn, EnumRoleOperationType.Add);
                    reader = TradingSaleReturnDA.InsertUpdate(tc, oTradingSaleReturn, EnumDBOperation.Insert, nUserID);
                }
                else
                {
                    AuthorizationRoleDA.CheckUserPermission(tc, nUserID, EnumModuleName.TradingSaleReturn, EnumRoleOperationType.Edit);
                    reader = TradingSaleReturnDA.InsertUpdate(tc, oTradingSaleReturn, EnumDBOperation.Update, nUserID);
                }

                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oTradingSaleReturn = new TradingSaleReturn();
                    oTradingSaleReturn = CreateObject(oReader);
                }
                reader.Close();
                #endregion

                #region Valid TradingSaleReturn
                if (oTradingSaleReturn.TradingSaleReturnID <= 0)
                {
                    throw new Exception("Invalid TradingSaleReturn!");
                }
                #endregion

                #region Purchase Return Detail Part
                if (oTradingSaleReturnDetails != null)
                {
                    foreach (TradingSaleReturnDetail oItem in oTradingSaleReturnDetails)
                    {
                        IDataReader readerdetail;
                        oItem.TradingSaleReturnID = oTradingSaleReturn.TradingSaleReturnID;
                        if (oItem.TradingSaleReturnDetailID <= 0)
                        {
                            readerdetail = TradingSaleReturnDetailDA.InsertUpdate(tc, oItem, EnumDBOperation.Insert, nUserID);
                        }
                        else
                        {
                            readerdetail = TradingSaleReturnDetailDA.InsertUpdate(tc, oItem, EnumDBOperation.Update, nUserID);
                        }
                        NullHandler oReaderDetail = new NullHandler(readerdetail);
                        if (readerdetail.Read())
                        {
                            sTradingSaleReturnDetailIDs = sTradingSaleReturnDetailIDs + oReaderDetail.GetString("TradingSaleReturnDetailID") + ",";
                        }
                        readerdetail.Close();
                    }
                    if (sTradingSaleReturnDetailIDs.Length > 0)
                    {
                        sTradingSaleReturnDetailIDs = sTradingSaleReturnDetailIDs.Remove(sTradingSaleReturnDetailIDs.Length - 1, 1);
                    }
                    oTradingSaleReturnDetail = new TradingSaleReturnDetail();
                    oTradingSaleReturnDetail.TradingSaleReturnID = oTradingSaleReturn.TradingSaleReturnID;
                    TradingSaleReturnDetailDA.Delete(tc, oTradingSaleReturnDetail, EnumDBOperation.Delete, nUserID, sTradingSaleReturnDetailIDs);
                }

                #endregion

                reader = TradingSaleReturnDA.Get(tc, oTradingSaleReturn.TradingSaleReturnID);
                oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oTradingSaleReturn = CreateObject(oReader);
                }
                reader.Close();
               
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                oTradingSaleReturn = new TradingSaleReturn();
                oTradingSaleReturn.ErrorMessage = e.Message.Split('~')[0];
                #endregion
            }
            return oTradingSaleReturn;
        }

        public TradingSaleReturn Approved(TradingSaleReturn oTradingSaleReturn, int nUserID)
        {
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);                               
              
                AuthorizationRoleDA.CheckUserPermission(tc, nUserID, EnumModuleName.TradingSaleReturn, EnumRoleOperationType.Approved);
                #region Get TradingSaleReturn
                IDataReader reader;
                reader = TradingSaleReturnDA.Approved(tc, oTradingSaleReturn.TradingSaleReturnID, nUserID);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oTradingSaleReturn = CreateObject(oReader);
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

                oTradingSaleReturn = new TradingSaleReturn();
                string Message = "";
                Message = e.Message;
                Message = Message.Split('!')[0];
                oTradingSaleReturn.ErrorMessage = Message;

                #endregion
            }
            return oTradingSaleReturn;
        }

        public string Delete(TradingSaleReturn oTradingSaleReturn, int nUserID)
        {
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
                AuthorizationRoleDA.CheckUserPermission(tc, nUserID, EnumModuleName.TradingSaleReturn, EnumRoleOperationType.Delete);
                TradingSaleReturnDA.Delete(tc, oTradingSaleReturn, EnumDBOperation.Delete, nUserID);
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

        public TradingSaleReturn Get(int nTradingSaleReturnID, int nUserID)
        {
            TradingSaleReturn oTradingSaleReturn = new TradingSaleReturn();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = TradingSaleReturnDA.Get(tc, nTradingSaleReturnID);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oTradingSaleReturn = CreateObject(oReader);
                }
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                throw new Exception("Failed to Get TradingSaleReturn \n" + e.Message, e);
                #endregion
            }
            return oTradingSaleReturn;
        }

        public List<TradingSaleReturn> Gets(int nUserID)
        {
            List<TradingSaleReturn> oTradingSaleReturns = null;
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = TradingSaleReturnDA.Gets(tc);
                oTradingSaleReturns = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                throw new Exception("Failed to Get TradingSaleReturn \n" + e.Message, e);
                #endregion
            }

            return oTradingSaleReturns;
        }

        public List<TradingSaleReturn> Gets(string sSQL, int nUserID)
        {
            List<TradingSaleReturn> oTradingSaleReturns = null;

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = TradingSaleReturnDA.Gets(tc, sSQL);
                oTradingSaleReturns = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                throw new Exception("Failed to Get TradingSaleReturns \n" + e.Message, e);
                #endregion
            }
            return oTradingSaleReturns;
        }
        public List<TradingSaleReturn> GetsInitialReturns(int nBUID, int nUserID)
        {
            List<TradingSaleReturn> oTradingSaleReturns = new List<TradingSaleReturn>();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = TradingSaleReturnDA.GetsInitialReturns(tc, nBUID);
                oTradingSaleReturns = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                throw new Exception("Failed to Get TradingSaleReturns \n" + e.Message, e);
                #endregion
            }
            return oTradingSaleReturns;
        }
        
        #endregion
    }
}

