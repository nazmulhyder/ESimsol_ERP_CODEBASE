using System;
using System.Collections.Generic;
using System.Data;
using ESimSol.BusinessObjects;
using ESimSol.Services.DataAccess;
using ICS.Core.DataAccess;
using ICS.Core.Utility;
using ICS.Core.Framework;

namespace ESimSol.Services.Services
{
    public class DUDeliveryOrderDetailService : MarshalByRefObject, IDUDeliveryOrderDetailService
    {
        #region Private functions and declaration
        private DUDeliveryOrderDetail MapObject(NullHandler oReader)
        {
            DUDeliveryOrderDetail oDUDeliveryOrderDetail = new DUDeliveryOrderDetail();

            oDUDeliveryOrderDetail.DUDeliveryOrderDetailID = oReader.GetInt32("DUDeliveryOrderDetailID");
            oDUDeliveryOrderDetail.DUDeliveryOrderID = oReader.GetInt32("DUDeliveryOrderID");
            oDUDeliveryOrderDetail.ExportSCDetailID = oReader.GetInt32("ExportSCDetailID");
            oDUDeliveryOrderDetail.DyeingOrderDetailID = oReader.GetInt32("DyeingOrderDetailID");
            oDUDeliveryOrderDetail.OrderType = oReader.GetInt32("OrderType");
            oDUDeliveryOrderDetail.ProductID = oReader.GetInt32("ProductID");
            oDUDeliveryOrderDetail.Qty = oReader.GetDouble("Qty");
            oDUDeliveryOrderDetail.Qty_DC = oReader.GetDouble("Qty_DC");
            oDUDeliveryOrderDetail.Qty_RC = oReader.GetDouble("Qty_RC");
            oDUDeliveryOrderDetail.UnitPrice = oReader.GetDouble("UnitPrice");
            //oDUDeliveryOrderDetail.DeliveryDate = oReader.GetDateTime("DeliveryDate");
            oDUDeliveryOrderDetail.Note = oReader.GetString("Note");
            oDUDeliveryOrderDetail.DONo = oReader.GetString("DONo");
            oDUDeliveryOrderDetail.ColorName = oReader.GetString("ColorName");
            //derive
            oDUDeliveryOrderDetail.ProductNameCode = oReader.GetString("ProductName");
            oDUDeliveryOrderDetail.ProductName = oReader.GetString("ProductName");
            oDUDeliveryOrderDetail.OrderNo = oReader.GetString("OrderNo");

            oDUDeliveryOrderDetail.MUName = oReader.GetString("MUName");
            //oDUDeliveryOrderDetail.Status = oReader.GetInt16("Status");
            
            return oDUDeliveryOrderDetail;           
        }

        private DUDeliveryOrderDetail CreateObject(NullHandler oReader)
        {
            DUDeliveryOrderDetail oDUDeliveryOrderDetail = MapObject(oReader);
            return oDUDeliveryOrderDetail;
        }

        private List<DUDeliveryOrderDetail> CreateObjects(IDataReader oReader)
        {
            List<DUDeliveryOrderDetail> oDUDeliveryOrderDetail = new List<DUDeliveryOrderDetail>();
            NullHandler oHandler = new NullHandler(oReader);
            while (oReader.Read())
            {
                DUDeliveryOrderDetail oItem = CreateObject(oHandler);
                oDUDeliveryOrderDetail.Add(oItem);
            }
            return oDUDeliveryOrderDetail;
        }

        #endregion

        #region Interface implementation
        public DUDeliveryOrderDetailService() { }



        public DUDeliveryOrderDetail Save(DUDeliveryOrderDetail oDUDeliveryOrderDetail, Int64 nUserID)
        {
            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin(true);
                IDataReader reader;
                if (oDUDeliveryOrderDetail.DUDeliveryOrderDetailID <= 0)
                {
                    reader = DUDeliveryOrderDetailDA.InsertUpdate(tc, oDUDeliveryOrderDetail, EnumDBOperation.Insert, nUserID,"");
                }
                else
                {
                    reader = DUDeliveryOrderDetailDA.InsertUpdate(tc, oDUDeliveryOrderDetail, EnumDBOperation.Update, nUserID, "");
                }
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oDUDeliveryOrderDetail = new DUDeliveryOrderDetail();
                    oDUDeliveryOrderDetail = CreateObject(oReader);
                }
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                //ExceptionLog.Write(e);
                //throw new ServiceException("Failed to . Because of " + e.Message, e);
                oDUDeliveryOrderDetail = new DUDeliveryOrderDetail();
                oDUDeliveryOrderDetail.ErrorMessage = e.Message.Split('~')[0];
                #endregion
            }
            return oDUDeliveryOrderDetail;
        }
        public DUDeliveryOrderDetail Get(int nDUDeliveryOrderDetailID, Int64 nUserId)
        {
            DUDeliveryOrderDetail oDUDeliveryOrderDetail = new DUDeliveryOrderDetail();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = DUDeliveryOrderDetailDA.Get(nDUDeliveryOrderDetailID, tc);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oDUDeliveryOrderDetail = CreateObject(oReader);
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
                oDUDeliveryOrderDetail.ErrorMessage = e.Message;
                #endregion
            }

            return oDUDeliveryOrderDetail;
        }
        public List<DUDeliveryOrderDetail> Gets(int nDUDeliveryOrderID, Int64 nUserID)
        {
            List<DUDeliveryOrderDetail> oDUDeliveryOrderDetail = null;

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = DUDeliveryOrderDetailDA.Gets(tc, nDUDeliveryOrderID);
                oDUDeliveryOrderDetail = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get DUDeliveryOrderDetail", e);
                #endregion
            }
            return oDUDeliveryOrderDetail;
        }

  

        public List<DUDeliveryOrderDetail> Gets(string sSQL, Int64 nUserID)
        {
            List<DUDeliveryOrderDetail> oDUDeliveryOrderDetail = null;

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = DUDeliveryOrderDetailDA.Gets(sSQL, tc);
                oDUDeliveryOrderDetail = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get DUDeliveryOrderDetail", e);
                #endregion
            }
            return oDUDeliveryOrderDetail;
        }

      

        public string Delete(DUDeliveryOrderDetail oDUDeliveryOrderDetail, Int64 nUserId)
        {
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);

                DUDeliveryOrderDetailDA.Delete(tc, oDUDeliveryOrderDetail, EnumDBOperation.Delete, nUserId,"");
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                return e.Message.Split('~')[0];
                #endregion
            }
            return Global.DeleteMessage;
        }

        #endregion
    }
}
