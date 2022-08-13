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
    public class TradingSaleInvoiceDetailService : MarshalByRefObject, ITradingSaleInvoiceDetailService
    {

        #region Private functions and declaration
        private TradingSaleInvoiceDetail MapObject(NullHandler oReader)
        {
            TradingSaleInvoiceDetail oTradingSaleInvoiceDetail = new TradingSaleInvoiceDetail();
            oTradingSaleInvoiceDetail.TradingSaleInvoiceDetailID = oReader.GetInt32("TradingSaleInvoiceDetailID");
            oTradingSaleInvoiceDetail.TradingSaleInvoiceID = oReader.GetInt32("TradingSaleInvoiceID");
            oTradingSaleInvoiceDetail.ProductID = oReader.GetInt32("ProductID");
            oTradingSaleInvoiceDetail.InvoiceQty = oReader.GetDouble("InvoiceQty");
            oTradingSaleInvoiceDetail.MeasurementUnitID = oReader.GetInt32("MeasurementUnitID");
            oTradingSaleInvoiceDetail.UnitPrice = oReader.GetDouble("UnitPrice");
            oTradingSaleInvoiceDetail.Amount = oReader.GetDouble("Amount");
            oTradingSaleInvoiceDetail.Discount = oReader.GetDouble("Discount");
            oTradingSaleInvoiceDetail.NetAmount = oReader.GetDouble("NetAmount");
            oTradingSaleInvoiceDetail.ItemDescription = oReader.GetString("ItemDescription");
            oTradingSaleInvoiceDetail.ProductCode = oReader.GetString("ProductCode");
            oTradingSaleInvoiceDetail.ProductName = oReader.GetString("ProductName");
            oTradingSaleInvoiceDetail.UnitName = oReader.GetString("UnitName");
            oTradingSaleInvoiceDetail.Symbol = oReader.GetString("Symbol");
            oTradingSaleInvoiceDetail.ProductCategoryName = oReader.GetString("ProductCategoryName");
            oTradingSaleInvoiceDetail.YetToChallanQty = oReader.GetDouble("YetToChallanQty");
            oTradingSaleInvoiceDetail.PurchaseAmount = oReader.GetDouble("PurchaseAmount");
            oTradingSaleInvoiceDetail.CurrentStock = oReader.GetDouble("CurrentStock");
            oTradingSaleInvoiceDetail.ChallanQty = oReader.GetDouble("ChallanQty");
            oTradingSaleInvoiceDetail.TradingSaleInvoiceDetailLogID = oReader.GetInt32("TradingSaleInvoiceDetailLogID");
            oTradingSaleInvoiceDetail.TradingSaleInvoiceLogID = oReader.GetInt32("TradingSaleInvoiceLogID");
            return oTradingSaleInvoiceDetail;
        }

        private TradingSaleInvoiceDetail CreateObject(NullHandler oReader)
        {
            TradingSaleInvoiceDetail oTradingSaleInvoiceDetail = new TradingSaleInvoiceDetail();
            oTradingSaleInvoiceDetail = MapObject(oReader);
            return oTradingSaleInvoiceDetail;
        }

        private List<TradingSaleInvoiceDetail> CreateObjects(IDataReader oReader)
        {
            List<TradingSaleInvoiceDetail> oTradingSaleInvoiceDetails = new List<TradingSaleInvoiceDetail>();
            NullHandler oHandler = new NullHandler(oReader);
            while (oReader.Read())
            {
                TradingSaleInvoiceDetail oItem = CreateObject(oHandler);
                oTradingSaleInvoiceDetails.Add(oItem);
            }
            return oTradingSaleInvoiceDetails;
        }
        #endregion

        #region Interface implementation
        public TradingSaleInvoiceDetailService() { }

        public TradingSaleInvoiceDetail Save(TradingSaleInvoiceDetail oTradingSaleInvoiceDetail, int nUserID)
        {
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
                IDataReader reader;
                if (oTradingSaleInvoiceDetail.TradingSaleInvoiceDetailID <= 0)
                {
                    reader = TradingSaleInvoiceDetailDA.InsertUpdate(tc, oTradingSaleInvoiceDetail, EnumDBOperation.Insert, nUserID);
                }
                else
                {
                    reader = TradingSaleInvoiceDetailDA.InsertUpdate(tc, oTradingSaleInvoiceDetail, EnumDBOperation.Update, nUserID);
                }
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oTradingSaleInvoiceDetail = new TradingSaleInvoiceDetail();
                    oTradingSaleInvoiceDetail = CreateObject(oReader);
                }
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                throw new Exception("Failed to Save TradingSaleInvoiceDetail \n" + e.Message, e);
                #endregion
            }
            return oTradingSaleInvoiceDetail;
        }
        public string Delete(TradingSaleInvoiceDetail oTradingSaleInvoiceDetail, int nUserID)
        {
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
                TradingSaleInvoiceDetailDA.Delete(tc, oTradingSaleInvoiceDetail, EnumDBOperation.Delete, nUserID, "");
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

        public TradingSaleInvoiceDetail Get(int nTradingSaleInvoiceDetailID, int nUserID)
        {
            TradingSaleInvoiceDetail oTradingSaleInvoiceDetail = new TradingSaleInvoiceDetail();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = TradingSaleInvoiceDetailDA.Get(tc, nTradingSaleInvoiceDetailID);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oTradingSaleInvoiceDetail = CreateObject(oReader);
                }
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                throw new Exception("Failed to Get TradingSaleInvoiceDetail \n" + e.Message, e);
                #endregion
            }
            return oTradingSaleInvoiceDetail;
        }

        public List<TradingSaleInvoiceDetail> Gets(int nUserID)
        {
            List<TradingSaleInvoiceDetail> oTradingSaleInvoiceDetails = null;
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = TradingSaleInvoiceDetailDA.Gets(tc);
                oTradingSaleInvoiceDetails = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                throw new Exception("Failed to Get TradingSaleInvoiceDetail \n" + e.Message, e);
                #endregion
            }

            return oTradingSaleInvoiceDetails;
        }
        public List<TradingSaleInvoiceDetail> Gets(int nSaleInvoiceID, int nUserID)
        {
            List<TradingSaleInvoiceDetail> oTradingSaleInvoiceDetails = null;
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = TradingSaleInvoiceDetailDA.Gets(tc, nSaleInvoiceID);
                oTradingSaleInvoiceDetails = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                throw new Exception("Failed to Get TradingSaleInvoiceDetail \n" + e.Message, e);
                #endregion
            }

            return oTradingSaleInvoiceDetails;
        }
        public List<TradingSaleInvoiceDetail> GetsLog(int nSaleInvoiceLogID, int nUserID)
        {
            List<TradingSaleInvoiceDetail> oTradingSaleInvoiceDetails = null;
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = TradingSaleInvoiceDetailDA.GetsLog(tc, nSaleInvoiceLogID);
                oTradingSaleInvoiceDetails = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                throw new Exception("Failed to Get TradingSaleInvoiceDetail \n" + e.Message, e);
                #endregion
            }

            return oTradingSaleInvoiceDetails;
        }
        public List<TradingSaleInvoiceDetail> Gets(string sSQL, int nUserID)
        {
            List<TradingSaleInvoiceDetail> oTradingSaleInvoiceDetails = null;

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = TradingSaleInvoiceDetailDA.Gets(tc, sSQL);
                oTradingSaleInvoiceDetails = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                throw new Exception("Failed to Get TradingSaleInvoiceDetails \n" + e.Message, e);
                #endregion
            }
            return oTradingSaleInvoiceDetails;
        }
        #endregion
    }
}
