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
    public class DUReturnChallanDetailService : MarshalByRefObject, IDUReturnChallanDetailService
    {
        #region Private functions and declaration
        private DUReturnChallanDetail MapObject(NullHandler oReader)
        {
            DUReturnChallanDetail oDUReturnChallanDetail = new DUReturnChallanDetail();
            oDUReturnChallanDetail.DUReturnChallanDetailID = oReader.GetInt32("DUReturnChallanDetailID");
            oDUReturnChallanDetail.DUReturnChallanID = oReader.GetInt32("DUReturnChallanID");
            oDUReturnChallanDetail.DUDeliveryChallanDetailID = oReader.GetInt32("DUDeliveryChallanDetailID");
            oDUReturnChallanDetail.LotID = oReader.GetInt32("LotID");
            oDUReturnChallanDetail.ProductID = oReader.GetInt32("ProductID");
            oDUReturnChallanDetail.DyeingOrderDetailID = oReader.GetInt32("DyeingOrderDetailID");
            oDUReturnChallanDetail.DyeingOrderID = oReader.GetInt32("DyeingOrderID");
            oDUReturnChallanDetail.MUnitID = oReader.GetInt32("MUnitID");
            oDUReturnChallanDetail.Qty = oReader.GetDouble("Qty");
            oDUReturnChallanDetail.Note = oReader.GetString("Note");
            oDUReturnChallanDetail.ProductName = oReader.GetString("ProductName");
            oDUReturnChallanDetail.ProductCode = oReader.GetString("ProductCode");
            oDUReturnChallanDetail.MUnit = oReader.GetString("MUnit");
            oDUReturnChallanDetail.LotNo = oReader.GetString("LotNo");
            oDUReturnChallanDetail.PINo = oReader.GetString("PINo");
            oDUReturnChallanDetail.OrderNo = oReader.GetString("OrderNo");
            oDUReturnChallanDetail.ColorName = oReader.GetString("ColorName");
            //oDUDeliveryChallanDetail.OrderID = oReader.GetInt32("OrderID");
            //oDUDeliveryChallanDetail.DyeingOrderDetailID = oReader.GetInt32("DyeingOrderDetailID");
            oDUReturnChallanDetail.HanksCone = oReader.GetInt32("HanksCone");
            oDUReturnChallanDetail.BagQty = oReader.GetDouble("BagQty");
            oDUReturnChallanDetail.UnitPrice = oReader.GetDouble("UnitPrice");

            return oDUReturnChallanDetail;
        }

        private DUReturnChallanDetail CreateObject(NullHandler oReader)
        {
            DUReturnChallanDetail oDUReturnChallanDetail = new DUReturnChallanDetail();
            oDUReturnChallanDetail = MapObject(oReader);
            return oDUReturnChallanDetail;
        }

        private List<DUReturnChallanDetail> CreateObjects(IDataReader oReader)
        {
            List<DUReturnChallanDetail> oDUReturnChallanDetail = new List<DUReturnChallanDetail>();
            NullHandler oHandler = new NullHandler(oReader);
            while (oReader.Read())
            {
                DUReturnChallanDetail oItem = CreateObject(oHandler);
                oDUReturnChallanDetail.Add(oItem);
            }
            return oDUReturnChallanDetail;
        }

        #endregion

        #region Interface implementation

        public DUReturnChallanDetail Get(int id, Int64 nUserId)
        {
            DUReturnChallanDetail oDUReturnChallanDetail = new DUReturnChallanDetail();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();
                IDataReader reader = DUReturnChallanDetailDA.Get(tc, id);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oDUReturnChallanDetail = CreateObject(oReader);
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
                throw new ServiceException("Failed to Get DUReturnChallanDetail", e);
                #endregion
            }
            return oDUReturnChallanDetail;
        }

        public List<DUReturnChallanDetail> Gets(int nID, Int64 nUserID)
        {
            List<DUReturnChallanDetail> oDUReturnChallanDetails = new List<DUReturnChallanDetail>();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();
                IDataReader reader = null;
                reader = DUReturnChallanDetailDA.Gets(nID, tc);
                oDUReturnChallanDetails = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                DUReturnChallanDetail oDUReturnChallanDetail = new DUReturnChallanDetail();
                oDUReturnChallanDetail.ErrorMessage = e.Message.Split('!')[0];
                #endregion
            }
            return oDUReturnChallanDetails;
        }

        public List<DUReturnChallanDetail> Gets(string sSQL, Int64 nUserID)
        {
            List<DUReturnChallanDetail> oDUReturnChallanDetails = new List<DUReturnChallanDetail>();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();
                IDataReader reader = null;
                reader = DUReturnChallanDetailDA.Gets(tc, sSQL);
                oDUReturnChallanDetails = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null) ;
                tc.HandleError();
                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get DUReturnChallanDetail", e);
                #endregion
            }
            return oDUReturnChallanDetails;
        }

        #endregion
    }

}
