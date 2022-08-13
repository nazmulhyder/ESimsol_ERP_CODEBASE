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
    public class TradingSaleReturnDetailService : MarshalByRefObject, ITradingSaleReturnDetailService
    {        

        #region Private functions and declaration
        private TradingSaleReturnDetail MapObject(NullHandler oReader)
        {
            TradingSaleReturnDetail oTradingSaleReturnDetail = new TradingSaleReturnDetail();
            oTradingSaleReturnDetail.TradingSaleReturnDetailID = oReader.GetInt32("TradingSaleReturnDetailID");
            oTradingSaleReturnDetail.TradingSaleReturnID = oReader.GetInt32("TradingSaleReturnID");
            oTradingSaleReturnDetail.ProductID = oReader.GetInt32("ProductID");
            oTradingSaleReturnDetail.ReturnQty = oReader.GetDouble("ReturnQty");
            oTradingSaleReturnDetail.MeasurementUnitID = oReader.GetInt32("MeasurementUnitID");
            oTradingSaleReturnDetail.UnitPrice = oReader.GetDouble("UnitPrice");
            oTradingSaleReturnDetail.Amount = oReader.GetDouble("Amount");
            oTradingSaleReturnDetail.ItemDescription = oReader.GetString("ItemDescription");
            oTradingSaleReturnDetail.ProductCode = oReader.GetString("ProductCode");
            oTradingSaleReturnDetail.ProductName = oReader.GetString("ProductName");
            oTradingSaleReturnDetail.UnitName = oReader.GetString("UnitName");
            oTradingSaleReturnDetail.Symbol = oReader.GetString("Symbol");
            return oTradingSaleReturnDetail;
        }

        private TradingSaleReturnDetail CreateObject(NullHandler oReader)
        {
            TradingSaleReturnDetail oTradingSaleReturnDetail = new TradingSaleReturnDetail();
            oTradingSaleReturnDetail = MapObject(oReader);
            return oTradingSaleReturnDetail;
        }

        private List<TradingSaleReturnDetail> CreateObjects(IDataReader oReader)
        {
            List<TradingSaleReturnDetail> oTradingSaleReturnDetails = new List<TradingSaleReturnDetail>();
            NullHandler oHandler = new NullHandler(oReader);
            while (oReader.Read())
            {
                TradingSaleReturnDetail oItem = CreateObject(oHandler);
                oTradingSaleReturnDetails.Add(oItem);
            }
            return oTradingSaleReturnDetails;
        }
        #endregion

        #region Interface implementation
        public TradingSaleReturnDetailService() { }

        public TradingSaleReturnDetail Save(TradingSaleReturnDetail oTradingSaleReturnDetail, int nUserID)
        {
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
                IDataReader reader;
                if (oTradingSaleReturnDetail.TradingSaleReturnDetailID <= 0)
                {
                    reader = TradingSaleReturnDetailDA.InsertUpdate(tc, oTradingSaleReturnDetail, EnumDBOperation.Insert, nUserID);
                }
                else
                {
                    reader = TradingSaleReturnDetailDA.InsertUpdate(tc, oTradingSaleReturnDetail, EnumDBOperation.Update, nUserID);
                }
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oTradingSaleReturnDetail = new TradingSaleReturnDetail();
                    oTradingSaleReturnDetail = CreateObject(oReader);
                }
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                throw new Exception("Failed to Save TradingSaleReturnDetail \n" + e.Message, e);
                #endregion
            }
            return oTradingSaleReturnDetail;
        }
        public string Delete(TradingSaleReturnDetail oTradingSaleReturnDetail, int nUserID)
        {
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
                TradingSaleReturnDetailDA.Delete(tc, oTradingSaleReturnDetail, EnumDBOperation.Delete, nUserID, "");
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

        public TradingSaleReturnDetail Get(int nTradingSaleReturnDetailID, int nUserID)
        {
            TradingSaleReturnDetail oTradingSaleReturnDetail = new TradingSaleReturnDetail();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = TradingSaleReturnDetailDA.Get(tc, nTradingSaleReturnDetailID);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oTradingSaleReturnDetail = CreateObject(oReader);
                }
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                throw new Exception("Failed to Get TradingSaleReturnDetail \n" + e.Message, e);
                #endregion
            }
            return oTradingSaleReturnDetail;
        }

        public List<TradingSaleReturnDetail> Gets(int nUserID)
        {
            List<TradingSaleReturnDetail> oTradingSaleReturnDetails = null;
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = TradingSaleReturnDetailDA.Gets(tc);
                oTradingSaleReturnDetails = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                throw new Exception("Failed to Get TradingSaleReturnDetail \n" + e.Message, e);
                #endregion
            }

            return oTradingSaleReturnDetails;
        }
        public List<TradingSaleReturnDetail> Gets(int nTradingSaleReturnID, int nUserID)
        {
            List<TradingSaleReturnDetail> oTradingSaleReturnDetails = null;
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = TradingSaleReturnDetailDA.Gets(tc, nTradingSaleReturnID);
                oTradingSaleReturnDetails = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                throw new Exception("Failed to Get TradingSaleReturnDetail \n" + e.Message, e);
                #endregion
            }

            return oTradingSaleReturnDetails;
        }
        public List<TradingSaleReturnDetail> Gets(string sSQL, int nUserID)
        {
            List<TradingSaleReturnDetail> oTradingSaleReturnDetails = null;

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = TradingSaleReturnDetailDA.Gets(tc, sSQL);
                oTradingSaleReturnDetails = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                throw new Exception("Failed to Get TradingSaleReturnDetails \n" + e.Message, e);
                #endregion
            }
            return oTradingSaleReturnDetails;
        }
        #endregion
    }
}

