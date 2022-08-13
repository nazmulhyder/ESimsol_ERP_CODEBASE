using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using ESimSol.BusinessObjects;
using ESimSol.Services.DataAccess;
using ICS.Core;
using ICS.Core.DataAccess;
using ICS.Core.Utility;
using ICS.Core.Framework;
namespace ESimSol.Services.Services
{


    public class TradingSaleOrderService : MarshalByRefObject, ITradingSaleOrderService
    {
        #region Private functions and declaration
        private TradingSaleOrder MapObject(NullHandler oReader)
        {
            TradingSaleOrder oTradingSaleOrder = new TradingSaleOrder();
            oTradingSaleOrder.TradingSaleOrderID = oReader.GetInt32("TradingSaleOrderID");
            oTradingSaleOrder.BUID = oReader.GetInt32("BUID");
            oTradingSaleOrder.DemandRequsitionID = oReader.GetInt32("DemandRequsitionID");
            oTradingSaleOrder.TradingSaleOrderNo = oReader.GetString("TradingSaleOrderNo");
            oTradingSaleOrder.OrderType = (EnumTradingSaleOrderType)oReader.GetInt32("OrderType");
            oTradingSaleOrder.ContractorID = oReader.GetInt32("ContractorID");
            oTradingSaleOrder.ContractorPersonalID = oReader.GetInt32("ContractorPersonalID");
            oTradingSaleOrder.OrderCreateDate = oReader.GetDateTime("OrderCreateDate");
            oTradingSaleOrder.RequestedDeliveryDate = oReader.GetDateTime("RequestedDeliveryDate");
            oTradingSaleOrder.IsActive = oReader.GetBoolean("IsActive");
            oTradingSaleOrder.Note = oReader.GetString("Note");
            oTradingSaleOrder.SaleValidatyDate = oReader.GetDateTime("SaleValidatyDate");
            oTradingSaleOrder.ContractorName = oReader.GetString("ContractorName");
            oTradingSaleOrder.ContractPersonName = oReader.GetString("ContractPersonName");
            oTradingSaleOrder.Amount = oReader.GetDouble("Amount");
            return oTradingSaleOrder;
        }

        private TradingSaleOrder CreateObject(NullHandler oReader)
        {
            TradingSaleOrder oTradingSaleOrder = new TradingSaleOrder();
            oTradingSaleOrder = MapObject(oReader);
            return oTradingSaleOrder;
        }

        private List<TradingSaleOrder> CreateObjects(IDataReader oReader)
        {
            List<TradingSaleOrder> oTradingSaleOrder = new List<TradingSaleOrder>();
            NullHandler oHandler = new NullHandler(oReader);
            while (oReader.Read())
            {
                TradingSaleOrder oItem = CreateObject(oHandler);
                oTradingSaleOrder.Add(oItem);
            }
            return oTradingSaleOrder;
        }

        #endregion

        #region Interface implementation
        public TradingSaleOrderService() { }

        public TradingSaleOrder Save(TradingSaleOrder oTradingSaleOrder, Int64 nUserId)
        {
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);

                List<TradingSaleOrderDetail> oTradingSaleOrderDetails = new List<TradingSaleOrderDetail>();
                TradingSaleOrderDetail oTradingSaleOrderDetail = new TradingSaleOrderDetail();


                oTradingSaleOrderDetails = oTradingSaleOrder.TradingSaleOrderDetails;
                string sTradingSaleOrderDetailIDs = "";

                IDataReader reader;
                if (oTradingSaleOrder.TradingSaleOrderID <= 0)
                {
                    reader = TradingSaleOrderDA.InsertUpdate(tc, oTradingSaleOrder, EnumDBOperation.Insert, nUserId);
                }
                else
                {
                    reader = TradingSaleOrderDA.InsertUpdate(tc, oTradingSaleOrder, EnumDBOperation.Update, nUserId);
                }
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oTradingSaleOrder = new TradingSaleOrder();
                    oTradingSaleOrder = CreateObject(oReader);
                }
                reader.Close();


                if (oTradingSaleOrderDetails != null)
                {
                    foreach (TradingSaleOrderDetail oItem in oTradingSaleOrderDetails)
                    {
                        IDataReader readerdetail;
                        oItem.TradingSaleOrderID = oTradingSaleOrder.TradingSaleOrderID;
                        if (oItem.TradingSaleOrderDetailID <= 0)
                        {
                            readerdetail = TradingSaleOrderDetailDA.InsertUpdate(tc, oItem, EnumDBOperation.Insert, nUserId);
                        }
                        else
                        {
                            readerdetail = TradingSaleOrderDetailDA.InsertUpdate(tc, oItem, EnumDBOperation.Update, nUserId);
                        }
                        NullHandler oReaderDetail = new NullHandler(readerdetail);
                        if (readerdetail.Read())
                        {
                            sTradingSaleOrderDetailIDs = sTradingSaleOrderDetailIDs + oReaderDetail.GetString("TradingSaleOrderDetailID") + ",";
                        }
                        readerdetail.Close();
                    }
                    if (sTradingSaleOrderDetailIDs.Length > 0)
                    {
                        sTradingSaleOrderDetailIDs = sTradingSaleOrderDetailIDs.Remove(sTradingSaleOrderDetailIDs.Length - 1, 1);
                    }
                    oTradingSaleOrderDetail = new TradingSaleOrderDetail();
                    oTradingSaleOrderDetail.TradingSaleOrderID = oTradingSaleOrder.TradingSaleOrderID;
                    TradingSaleOrderDetailDA.Delete(tc, oTradingSaleOrderDetail, EnumDBOperation.Delete, nUserId, sTradingSaleOrderDetailIDs);
                }

                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                oTradingSaleOrder = new TradingSaleOrder();
                oTradingSaleOrder.ErrorMessage = e.Message.Split('!')[0];
                //ExceptionLog.Write(e);
                //throw new ServiceException("Failed to Save TradingSaleOrder. Because of " + e.Message, e);
                #endregion
            }
            return oTradingSaleOrder;
        }
        public TradingSaleOrder SaveInvoice(SaleInvoice oSaleInvoice, Int64 nUserId)
        {
            TradingSaleOrder oTradingSaleOrder = new TradingSaleOrder();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
                IDataReader reader;
                if (oSaleInvoice.SaleInvoiceID <= 0)
                {
                    reader = TradingSaleOrderDA.SaveInvoice(tc, oSaleInvoice, EnumDBOperation.Insert, nUserId);
                }
                else
                {
                    reader = TradingSaleOrderDA.SaveInvoice(tc, oSaleInvoice, EnumDBOperation.Update, nUserId);
                }
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oTradingSaleOrder = new TradingSaleOrder();
                    oTradingSaleOrder = CreateObject(oReader);
                }
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                oTradingSaleOrder = new TradingSaleOrder();
                oTradingSaleOrder.ErrorMessage = e.Message;
                //ExceptionLog.Write(e);
                //throw new ServiceException("Failed to Save TradingSaleOrder. Because of " + e.Message, e);
                #endregion
            }
            return oTradingSaleOrder;
        }
        public string Delete(int id, Int64 nUserId)
        {
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
                TradingSaleOrder oTradingSaleOrder = new TradingSaleOrder();
                oTradingSaleOrder.TradingSaleOrderID = id;
                TradingSaleOrderDA.Delete(tc, oTradingSaleOrder, EnumDBOperation.Delete, nUserId);
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                return e.Message;
                //ExceptionLog.Write(e);
                //throw new ServiceException("Failed to Delete TradingSaleOrder. Because of " + e.Message, e);
                #endregion
            }
            return "Data delete successfully";
        }

        public string GetTradingSaleOrderNo(Int64 nUserId)
        {
            string sTradingSaleOrderNo = "";
            TradingSaleOrder oTradingSaleOrder = new TradingSaleOrder();

            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = TradingSaleOrderDA.GetMaxOrderNo(tc);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oTradingSaleOrder = CreateObject(oReader);
                    sTradingSaleOrderNo = (oTradingSaleOrder.TradingSaleOrderID + 1001).ToString();
                }
                else
                {
                    sTradingSaleOrderNo = "1001";
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
                throw new ServiceException("Failed to Get DemandRequisition", e);
                #endregion
            }

            return sTradingSaleOrderNo;
        }

        public TradingSaleOrder Get(int id, Int64 nUserId)
        {
            TradingSaleOrder oAccountHead = new TradingSaleOrder();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = TradingSaleOrderDA.Get(tc, id);
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
                throw new ServiceException("Failed to Get TradingSaleOrder", e);
                #endregion
            }

            return oAccountHead;
        }
        public TradingSaleOrder GetByTradingSaleOrderNo(string sTradingSaleOrderNo, Int64 nUserId)
        {
            TradingSaleOrder oAccountHead = new TradingSaleOrder();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = TradingSaleOrderDA.GetByTradingSaleOrderNo(tc, sTradingSaleOrderNo);
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
                throw new ServiceException("Failed to Get TradingSaleOrder", e);
                #endregion
            }

            return oAccountHead;
        }


        public List<TradingSaleOrder> Gets(Int64 nUserId)
        {
            List<TradingSaleOrder> oTradingSaleOrder = null;

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = TradingSaleOrderDA.Gets(tc);
                oTradingSaleOrder = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get TradingSaleOrder", e);
                #endregion
            }

            return oTradingSaleOrder;
        }

        public List<TradingSaleOrder> Gets(string sSQL, Int64 nUserId)
        {
            List<TradingSaleOrder> oTradingSaleOrder = null;

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = TradingSaleOrderDA.Gets(tc, sSQL);
                oTradingSaleOrder = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get TradingSaleOrder", e);
                #endregion
            }

            return oTradingSaleOrder;
        }
        #endregion
    } 

    
}

