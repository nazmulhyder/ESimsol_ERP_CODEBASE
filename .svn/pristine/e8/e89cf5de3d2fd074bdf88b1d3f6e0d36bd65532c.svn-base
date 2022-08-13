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
    public class TradingDeliveryChallanDetailService : MarshalByRefObject, ITradingDeliveryChallanDetailService
    {

        #region Private functions and declaration
        private TradingDeliveryChallanDetail MapObject(NullHandler oReader)
        {
            TradingDeliveryChallanDetail oTradingDeliveryChallanDetail = new TradingDeliveryChallanDetail();
            oTradingDeliveryChallanDetail.TradingDeliveryChallanDetailID = oReader.GetInt32("TradingDeliveryChallanDetailID");
            oTradingDeliveryChallanDetail.TradingDeliveryChallanID = oReader.GetInt32("TradingDeliveryChallanID");
            oTradingDeliveryChallanDetail.TradingSaleInvoiceDetailID = oReader.GetInt32("TradingSaleInvoiceDetailID");
            oTradingDeliveryChallanDetail.ProductID = oReader.GetInt32("ProductID");
            oTradingDeliveryChallanDetail.ItemDescription = oReader.GetString("ItemDescription");
            oTradingDeliveryChallanDetail.UnitID = oReader.GetInt32("UnitID");
            oTradingDeliveryChallanDetail.LotID = oReader.GetInt32("LotID");
            oTradingDeliveryChallanDetail.LotNo = oReader.GetString("LotNo");
            oTradingDeliveryChallanDetail.LotBalance = oReader.GetDouble("Balance");
            oTradingDeliveryChallanDetail.ChallanQty = oReader.GetDouble("ChallanQty");
            oTradingDeliveryChallanDetail.UnitPrice = oReader.GetDouble("UnitPrice");
            oTradingDeliveryChallanDetail.Amount = oReader.GetDouble("Amount");
            oTradingDeliveryChallanDetail.ProductCode = oReader.GetString("ProductCode");
            oTradingDeliveryChallanDetail.ProductName = oReader.GetString("ProductName");
            oTradingDeliveryChallanDetail.UnitName = oReader.GetString("UnitName");
            oTradingDeliveryChallanDetail.Symbol = oReader.GetString("Symbol");
            oTradingDeliveryChallanDetail.YetToChallanQty = oReader.GetDouble("YetToChallanQty");
            return oTradingDeliveryChallanDetail;
        }

        private TradingDeliveryChallanDetail CreateObject(NullHandler oReader)
        {
            TradingDeliveryChallanDetail oTradingDeliveryChallanDetail = new TradingDeliveryChallanDetail();
            oTradingDeliveryChallanDetail = MapObject(oReader);
            return oTradingDeliveryChallanDetail;
        }

        private List<TradingDeliveryChallanDetail> CreateObjects(IDataReader oReader)
        {
            List<TradingDeliveryChallanDetail> oTradingDeliveryChallanDetails = new List<TradingDeliveryChallanDetail>();
            NullHandler oHandler = new NullHandler(oReader);
            while (oReader.Read())
            {
                TradingDeliveryChallanDetail oItem = CreateObject(oHandler);
                oTradingDeliveryChallanDetails.Add(oItem);
            }
            return oTradingDeliveryChallanDetails;
        }
        #endregion

        #region Interface implementation
        public TradingDeliveryChallanDetailService() { }

        public TradingDeliveryChallanDetail Save(TradingDeliveryChallanDetail oTradingDeliveryChallanDetail, int nUserID)
        {
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
                IDataReader reader;
                if (oTradingDeliveryChallanDetail.TradingDeliveryChallanDetailID <= 0)
                {
                    reader = TradingDeliveryChallanDetailDA.InsertUpdate(tc, oTradingDeliveryChallanDetail, EnumDBOperation.Insert, nUserID);
                }
                else
                {
                    reader = TradingDeliveryChallanDetailDA.InsertUpdate(tc, oTradingDeliveryChallanDetail, EnumDBOperation.Update, nUserID);
                }
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oTradingDeliveryChallanDetail = new TradingDeliveryChallanDetail();
                    oTradingDeliveryChallanDetail = CreateObject(oReader);
                }
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                throw new Exception("Failed to Save TradingDeliveryChallanDetail \n" + e.Message, e);
                #endregion
            }
            return oTradingDeliveryChallanDetail;
        }
        public string Delete(TradingDeliveryChallanDetail oTradingDeliveryChallanDetail, int nUserID)
        {
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
                TradingDeliveryChallanDetailDA.Delete(tc, oTradingDeliveryChallanDetail, EnumDBOperation.Delete, nUserID, "");
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

        public TradingDeliveryChallanDetail Get(int nTradingDeliveryChallanDetailID, int nUserID)
        {
            TradingDeliveryChallanDetail oTradingDeliveryChallanDetail = new TradingDeliveryChallanDetail();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = TradingDeliveryChallanDetailDA.Get(tc, nTradingDeliveryChallanDetailID);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oTradingDeliveryChallanDetail = CreateObject(oReader);
                }
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                throw new Exception("Failed to Get TradingDeliveryChallanDetail \n" + e.Message, e);
                #endregion
            }
            return oTradingDeliveryChallanDetail;
        }

        public List<TradingDeliveryChallanDetail> Gets(int nUserID)
        {
            List<TradingDeliveryChallanDetail> oTradingDeliveryChallanDetails = null;
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = TradingDeliveryChallanDetailDA.Gets(tc);
                oTradingDeliveryChallanDetails = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                throw new Exception("Failed to Get TradingDeliveryChallanDetail \n" + e.Message, e);
                #endregion
            }

            return oTradingDeliveryChallanDetails;
        }
        public List<TradingDeliveryChallanDetail> Gets(int nDeliveryChallanID, int nUserID)
        {
            List<TradingDeliveryChallanDetail> oTradingDeliveryChallanDetails = null;
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = TradingDeliveryChallanDetailDA.Gets(tc, nDeliveryChallanID);
                oTradingDeliveryChallanDetails = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                throw new Exception("Failed to Get TradingDeliveryChallanDetail \n" + e.Message, e);
                #endregion
            }

            return oTradingDeliveryChallanDetails;
        }
        public List<TradingDeliveryChallanDetail> Gets(string sSQL, int nUserID)
        {
            List<TradingDeliveryChallanDetail> oTradingDeliveryChallanDetails = null;

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = TradingDeliveryChallanDetailDA.Gets(tc, sSQL);
                oTradingDeliveryChallanDetails = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                throw new Exception("Failed to Get TradingDeliveryChallanDetails \n" + e.Message, e);
                #endregion
            }
            return oTradingDeliveryChallanDetails;
        }
        #endregion
    }
}
