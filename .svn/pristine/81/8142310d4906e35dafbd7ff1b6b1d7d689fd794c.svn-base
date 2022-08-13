using System;
using ICS.Core.Framework;
using ICS.Core.Utility;
using ICS.Core;
using ESimSol.BusinessObjects;
using System.Collections.Generic;
using System.Text;
using System.Data;
using ESimSol.Services.DataAccess;
using ICS.Core.DataAccess;

namespace ESimSol.Services.Services
{
    public class DeliveryOrderDetailService : MarshalByRefObject, IDeliveryOrderDetailService
    {
        #region Private functions and declaration

        private DeliveryOrderDetail MapObject(NullHandler oReader)
        {
            DeliveryOrderDetail oDeliveryOrderDetail = new DeliveryOrderDetail();
            oDeliveryOrderDetail.DeliveryOrderDetailID = oReader.GetInt32("DeliveryOrderDetailID");
            oDeliveryOrderDetail.DeliveryOrderID = oReader.GetInt32("DeliveryOrderID");
            oDeliveryOrderDetail.DeliveryOrderLogID = oReader.GetInt32("DeliveryOrderLogID");
            oDeliveryOrderDetail.DeliveryOrderDetailLogID = oReader.GetInt32("DeliveryOrderDetailLogID");
            oDeliveryOrderDetail.ProductID = oReader.GetInt32("ProductID");
            oDeliveryOrderDetail.RefDetailID = oReader.GetInt32("RefDetailID");
            oDeliveryOrderDetail.Qty = oReader.GetDouble("Qty");
            oDeliveryOrderDetail.Note = oReader.GetString("Note");
            oDeliveryOrderDetail.ProductName = oReader.GetString("ProductName");
            oDeliveryOrderDetail.ProductCode = oReader.GetString("ProductCode");
            oDeliveryOrderDetail.MUnitID = oReader.GetInt32("MUnitID");
            oDeliveryOrderDetail.MUnit = oReader.GetString("MUnit");
            oDeliveryOrderDetail.YetToDeliveryOrderQty = oReader.GetDouble("YetToDeliveryOrderQty");
            oDeliveryOrderDetail.YetToDeliveryChallanQty = oReader.GetDouble("YetToDeliveryChallanQty");
            oDeliveryOrderDetail.ColorID = oReader.GetInt32("ColorID");
            oDeliveryOrderDetail.ColorName = oReader.GetString("ColorName");
            oDeliveryOrderDetail.DONo = oReader.GetString("DONo");
            oDeliveryOrderDetail.StyleNo = oReader.GetString("StyleNo");
            oDeliveryOrderDetail.Measurement = oReader.GetString("Measurement");
            oDeliveryOrderDetail.SizeName = oReader.GetString("SizeName");
            oDeliveryOrderDetail.PTUUnit2ID = oReader.GetInt32("PTUUnit2ID");
            oDeliveryOrderDetail.DeliveryDate = oReader.GetDateTime("DeliveryDate");            
            
            return oDeliveryOrderDetail;
        }

        private DeliveryOrderDetail CreateObject(NullHandler oReader)
        {
            DeliveryOrderDetail oDeliveryOrderDetail = new DeliveryOrderDetail();
            oDeliveryOrderDetail = MapObject(oReader);
            return oDeliveryOrderDetail;
        }

        private List<DeliveryOrderDetail> CreateObjects(IDataReader oReader)
        {
            List<DeliveryOrderDetail> oDeliveryOrderDetail = new List<DeliveryOrderDetail>();
            NullHandler oHandler = new NullHandler(oReader);
            while (oReader.Read())
            {
                DeliveryOrderDetail oItem = CreateObject(oHandler);
                oDeliveryOrderDetail.Add(oItem);
            }
            return oDeliveryOrderDetail;
        }

        #endregion

        #region Interface implementation

        public DeliveryOrderDetail Get(int id, Int64 nUserId)
        {
            DeliveryOrderDetail oDeliveryOrderDetail = new DeliveryOrderDetail();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();
                IDataReader reader = DeliveryOrderDetailDA.Get(tc, id);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oDeliveryOrderDetail = CreateObject(oReader);
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
                throw new ServiceException("Failed to Get DeliveryOrderDetail", e);
                #endregion
            }
            return oDeliveryOrderDetail;
        }

        public List<DeliveryOrderDetail> Gets(int nDOID, Int64 nUserID)
        {
            List<DeliveryOrderDetail> oDeliveryOrderDetails = new List<DeliveryOrderDetail>();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();
                IDataReader reader = null;
                reader = DeliveryOrderDetailDA.Gets(nDOID, tc);
                oDeliveryOrderDetails = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                DeliveryOrderDetail oDeliveryOrderDetail = new DeliveryOrderDetail();
                oDeliveryOrderDetail.ErrorMessage = e.Message.Split('!')[0];
                #endregion
            }
            return oDeliveryOrderDetails;
        }
        
        public List<DeliveryOrderDetail> Gets(string sSQL, Int64 nUserID)
        {
            List<DeliveryOrderDetail> oDeliveryOrderDetails = new List<DeliveryOrderDetail>();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();
                IDataReader reader = null;
                reader = DeliveryOrderDetailDA.Gets(tc, sSQL);
                oDeliveryOrderDetails = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null) ;
                tc.HandleError();
                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get DeliveryOrderDetail", e);
                #endregion
            }
            return oDeliveryOrderDetails;
        }

        #endregion
    }

}
