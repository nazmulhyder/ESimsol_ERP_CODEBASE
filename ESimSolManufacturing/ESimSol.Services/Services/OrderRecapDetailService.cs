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
    public class OrderRecapDetailService : MarshalByRefObject, IOrderRecapDetailService
    {
        #region Private functions and declaration
        private OrderRecapDetail MapObject(NullHandler oReader)
        {
            OrderRecapDetail oOrderRecapDetail = new OrderRecapDetail();
            oOrderRecapDetail.OrderRecapDetailID = oReader.GetInt32("OrderRecapDetailID");
            oOrderRecapDetail.OrderRecapID = oReader.GetInt32("OrderRecapID");
            oOrderRecapDetail.OrderRecapDetailLogID = oReader.GetInt32("OrderRecapDetailLogID");
            oOrderRecapDetail.OrderRecapLogID = oReader.GetInt32("OrderRecapLogID");
            oOrderRecapDetail.MeasurementUnitID = oReader.GetInt32("MeasurementUnitID");
            oOrderRecapDetail.SizeID = oReader.GetInt32("SizeID");
            oOrderRecapDetail.UnitPrice = oReader.GetDouble("UnitPrice");
            oOrderRecapDetail.Quantity = oReader.GetDouble("Quantity");
            oOrderRecapDetail.Amount = oReader.GetDouble("Amount");
            oOrderRecapDetail.ColorID = oReader.GetInt32("ColorID");
            oOrderRecapDetail.ColorName = oReader.GetString("ColorName");
            oOrderRecapDetail.SizeName = oReader.GetString("SizeCategoryName");
            oOrderRecapDetail.UnitName = oReader.GetString("UnitName");
            oOrderRecapDetail.UnitSymbol = oReader.GetString("UnitSymbol");
            oOrderRecapDetail.CurrencySymbol = oReader.GetString("CurrencySymbol");
            oOrderRecapDetail.ShipmentDate = oReader.GetDateTime("ShipmentDate");
            oOrderRecapDetail.OrderRecapNo = oReader.GetString("OrderRecapNo");
            
            oOrderRecapDetail.ColorSequence = oReader.GetInt32("ColorSequence");
            oOrderRecapDetail.SizeSequence = oReader.GetInt32("SizeSequence");
            oOrderRecapDetail.PoductionQty = oReader.GetDouble("PoductionQty");
            oOrderRecapDetail.YetToPoductionQty = oReader.GetDouble("YetToPoductionQty");
            oOrderRecapDetail.YetToScheduleQty = oReader.GetDouble("YetToScheduleQty");     
            
            return oOrderRecapDetail;
        }

        private OrderRecapDetail CreateObject(NullHandler oReader)
        {
            OrderRecapDetail oOrderRecapDetail = new OrderRecapDetail();
            oOrderRecapDetail = MapObject(oReader);
            return oOrderRecapDetail;
        }

        private List<OrderRecapDetail> CreateObjects(IDataReader oReader)
        {
            List<OrderRecapDetail> oOrderRecapDetails = new List<OrderRecapDetail>();
            NullHandler oHandler = new NullHandler(oReader);
            while (oReader.Read())
            {
                OrderRecapDetail oItem = CreateObject(oHandler);
                oOrderRecapDetails.Add(oItem);
            }
            return oOrderRecapDetails;
        }

        #endregion

        #region Interface implementation
        public OrderRecapDetailService() { }

        public OrderRecapDetail Save(OrderRecapDetail oOrderRecapDetail, Int64 nUserId)
        {
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
                IDataReader reader;
                if (oOrderRecapDetail.OrderRecapDetailID <= 0)
                {
                    reader = OrderRecapDetailDA.InsertUpdate(tc, oOrderRecapDetail, EnumDBOperation.Insert, nUserId);
                }
                else
                {
                    reader = OrderRecapDetailDA.InsertUpdate(tc, oOrderRecapDetail, EnumDBOperation.Update, nUserId);
                }
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oOrderRecapDetail = new OrderRecapDetail();
                    oOrderRecapDetail = CreateObject(oReader);
                }
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                oOrderRecapDetail = new OrderRecapDetail();
                oOrderRecapDetail.ErrorMessage = e.Message;
                #endregion
            }
            return oOrderRecapDetail;
        }


        public string Delete(int id, Int64 nUserId, string sOrderRecapDetailIDs)
        {
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
                OrderRecapDetail oOrderRecapDetail = new OrderRecapDetail();
                oOrderRecapDetail.OrderRecapDetailID = id;
                OrderRecapDetailDA.Delete(tc, oOrderRecapDetail, EnumDBOperation.Delete, nUserId, sOrderRecapDetailIDs);
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                #endregion
                return e.Message;
            }
            return "Data delete successfully";
        }

        public List<OrderRecapDetail> Gets(int id, Int64 nUserId)
        {
            List<OrderRecapDetail> oOrderRecapDetails = null;

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = OrderRecapDetailDA.Gets(tc, id);
                oOrderRecapDetails = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                throw new ServiceException(e.Message);
                #endregion
            }

            return oOrderRecapDetails;
        }

        public List<OrderRecapDetail> GetsByLog(int id, Int64 nUserId)
        {
            List<OrderRecapDetail> oOrderRecapDetails = null;

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = OrderRecapDetailDA.GetsByLog(tc, id);
                oOrderRecapDetails = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                throw new ServiceException(e.Message);
                #endregion
            }

            return oOrderRecapDetails;
        }
        public List<OrderRecapDetail> Gets(string sSql, Int64 nUserId)
        {
            List<OrderRecapDetail> oOrderRecapDetails = null;

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = OrderRecapDetailDA.Gets(tc, sSql);
                oOrderRecapDetails = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                throw new ServiceException(e.Message);
                #endregion
            }

            return oOrderRecapDetails;
        }

        #endregion
    }
}
