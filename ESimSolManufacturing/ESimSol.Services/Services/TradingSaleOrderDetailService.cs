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

    public class TradingSaleOrderDetailService : MarshalByRefObject, ITradingSaleOrderDetailService
    {
        #region Private functions and declaration
        private TradingSaleOrderDetail MapObject(NullHandler oReader)
        {
            TradingSaleOrderDetail oTradingSaleOrderDetail = new TradingSaleOrderDetail();
            oTradingSaleOrderDetail.TradingSaleOrderDetailID = oReader.GetInt32("TradingSaleOrderDetailID");
            oTradingSaleOrderDetail.TradingSaleOrderID = oReader.GetInt32("TradingSaleOrderID");
            oTradingSaleOrderDetail.ProductID = oReader.GetInt32("ProductID");
            oTradingSaleOrderDetail.OrderQty = oReader.GetDouble("OrderQty");
            oTradingSaleOrderDetail.UnitPrice = oReader.GetDouble("UnitPrice");
            oTradingSaleOrderDetail.MeasurementUnitID = oReader.GetInt32("MeasurementUnitID");
            oTradingSaleOrderDetail.VatInPercent = oReader.GetDouble("VatInPercent");
            oTradingSaleOrderDetail.InvoiceQty = oReader.GetDouble("InvoiceQty");
            oTradingSaleOrderDetail.SaleOrderNo = oReader.GetString("SaleOrderNo");
            oTradingSaleOrderDetail.ContractorName = oReader.GetString("ContractorName");
            oTradingSaleOrderDetail.OrderCreateDate = oReader.GetDateTime("OrderCreateDate");
            oTradingSaleOrderDetail.OrderType = (EnumTradingSaleOrderType)oReader.GetInt16("OrderType");
            oTradingSaleOrderDetail.ProductName = oReader.GetString("ProductName");
            oTradingSaleOrderDetail.ProductCode = oReader.GetString("ProductCode");
            oTradingSaleOrderDetail.MeasurementUnitName = oReader.GetString("MeasurementUnitName");
            oTradingSaleOrderDetail.YetToInvoice = oReader.GetDouble("YetToInvoice");
            oTradingSaleOrderDetail.ContractorID = oReader.GetInt32("ContractorID");
            oTradingSaleOrderDetail.GradeAPrice = oReader.GetDouble("GradeAPrice");
            oTradingSaleOrderDetail.GradeBPrice = oReader.GetDouble("GradeBPrice");
            oTradingSaleOrderDetail.GradeCPrice = oReader.GetDouble("GradeCPrice");
            oTradingSaleOrderDetail.ProductGrade = (EnumProductGrade)oReader.GetInt16("ProductGrade");
            return oTradingSaleOrderDetail;
        }

        private TradingSaleOrderDetail CreateObject(NullHandler oReader)
        {
            TradingSaleOrderDetail oTradingSaleOrderDetail = new TradingSaleOrderDetail();
            oTradingSaleOrderDetail = MapObject(oReader);
            return oTradingSaleOrderDetail;
        }

        private List<TradingSaleOrderDetail> CreateObjects(IDataReader oReader)
        {
            List<TradingSaleOrderDetail> oTradingSaleOrderDetail = new List<TradingSaleOrderDetail>();
            NullHandler oHandler = new NullHandler(oReader);
            while (oReader.Read())
            {
                TradingSaleOrderDetail oItem = CreateObject(oHandler);
                oTradingSaleOrderDetail.Add(oItem);
            }
            return oTradingSaleOrderDetail;
        }

        #endregion

        #region Interface implementation
        public TradingSaleOrderDetailService() { }

        public TradingSaleOrderDetail Save(TradingSaleOrderDetail oTradingSaleOrderDetail, Int64 nUserId)
        {
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
                IDataReader reader;
                if (oTradingSaleOrderDetail.TradingSaleOrderDetailID <= 0)
                {
                    reader = TradingSaleOrderDetailDA.InsertUpdate(tc, oTradingSaleOrderDetail, EnumDBOperation.Insert, nUserId);
                }
                else
                {
                    reader = TradingSaleOrderDetailDA.InsertUpdate(tc, oTradingSaleOrderDetail, EnumDBOperation.Update, nUserId);
                }
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oTradingSaleOrderDetail = new TradingSaleOrderDetail();
                    oTradingSaleOrderDetail = CreateObject(oReader);
                }
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                oTradingSaleOrderDetail = new TradingSaleOrderDetail();
                oTradingSaleOrderDetail.ErrorMessage = e.Message;
                //ExceptionLog.Write(e);
                //throw new ServiceException("Failed to Save TradingSaleOrderDetail. Because of " + e.Message, e);
                #endregion
            }
            return oTradingSaleOrderDetail;
        }
        public string Delete(int id, Int64 nUserId)
        {
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
                TradingSaleOrderDetail oTradingSaleOrderDetail = new TradingSaleOrderDetail();
                oTradingSaleOrderDetail.TradingSaleOrderDetailID = id;
                TradingSaleOrderDetailDA.Delete(tc, oTradingSaleOrderDetail, EnumDBOperation.Delete, nUserId, "");
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                return e.Message;
                //ExceptionLog.Write(e);
                //throw new ServiceException("Failed to Delete TradingSaleOrderDetail. Because of " + e.Message, e);
                #endregion
            }
            return "Data delete successfully";
        }

        public TradingSaleOrderDetail Get(int id, Int64 nUserId)
        {
            TradingSaleOrderDetail oAccountHead = new TradingSaleOrderDetail();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = TradingSaleOrderDetailDA.Get(tc, id);
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
                throw new ServiceException("Failed to Get TradingSaleOrderDetail", e);
                #endregion
            }

            return oAccountHead;
        }

        public List<TradingSaleOrderDetail> Gets(Int64 nUserId)
        {
            List<TradingSaleOrderDetail> oTradingSaleOrderDetail = null;

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = TradingSaleOrderDetailDA.Gets(tc);
                oTradingSaleOrderDetail = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get TradingSaleOrderDetail", e);
                #endregion
            }

            return oTradingSaleOrderDetail;
        }

        public List<TradingSaleOrderDetail> GetsForInvoice(int id, Int64 nUserId)
        {
            List<TradingSaleOrderDetail> oTradingSaleOrderDetail = null;

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = TradingSaleOrderDetailDA.GetsForInvoice(tc, id);
                oTradingSaleOrderDetail = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get TradingSaleOrderDetail", e);
                #endregion
            }

            return oTradingSaleOrderDetail;
        }


        public List<TradingSaleOrderDetail> Gets(int id, Int64 nUserId)
        {
            List<TradingSaleOrderDetail> oTradingSaleOrderDetail = null;

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = TradingSaleOrderDetailDA.Gets(tc, id);
                oTradingSaleOrderDetail = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get TradingSaleOrderDetail", e);
                #endregion
            }

            return oTradingSaleOrderDetail;
        }
        #endregion
    }



}
