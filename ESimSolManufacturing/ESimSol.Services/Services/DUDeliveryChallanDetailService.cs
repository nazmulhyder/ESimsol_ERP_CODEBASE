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
    public class DUDeliveryChallanDetailService : MarshalByRefObject, IDUDeliveryChallanDetailService
    {
        #region Private functions and declaration
        private DUDeliveryChallanDetail MapObject(NullHandler oReader)
        {
            DUDeliveryChallanDetail oDUDeliveryChallanDetail = new DUDeliveryChallanDetail();

            oDUDeliveryChallanDetail.DUDeliveryChallanDetailID = oReader.GetInt32("DUDeliveryChallanDetailID");
            oDUDeliveryChallanDetail.DUDeliveryChallanID = oReader.GetInt32("DUDeliveryChallanID");
            oDUDeliveryChallanDetail.LotID = oReader.GetInt32("LotID");
            oDUDeliveryChallanDetail.DODetailID = oReader.GetInt32("DODetailID");
            oDUDeliveryChallanDetail.ProductID = oReader.GetInt32("ProductID");
            oDUDeliveryChallanDetail.Qty = oReader.GetDouble("Qty");
            oDUDeliveryChallanDetail.BagQty = oReader.GetDouble("BagQty");
            oDUDeliveryChallanDetail.PartyName = oReader.GetString("PartyName");
            oDUDeliveryChallanDetail.PTUID = oReader.GetInt32("PTUID");
            oDUDeliveryChallanDetail.OrderID = oReader.GetInt32("OrderID");
            oDUDeliveryChallanDetail.DyeingOrderID = oReader.GetInt32("DyeingOrderID");
            oDUDeliveryChallanDetail.UnitPrice = oReader.GetDouble("UnitPrice");
                        //derive
            oDUDeliveryChallanDetail.ColorName = oReader.GetString("ColorName");
            oDUDeliveryChallanDetail.ColorNo = oReader.GetString("ColorNo");
            oDUDeliveryChallanDetail.Shade = oReader.GetInt16("Shade");
            oDUDeliveryChallanDetail.ProductName = oReader.GetString("ProductName");
            oDUDeliveryChallanDetail.ProductCode = oReader.GetString("ProductCode");
            oDUDeliveryChallanDetail.ChallanNo = oReader.GetString("ChallanNo");
            oDUDeliveryChallanDetail.MUnit = oReader.GetString("MUnit");
            //oDUDeliveryChallanDetail.Status = oReader.GetInt16("Status");
            oDUDeliveryChallanDetail.LotNo = oReader.GetString("LotNo");
            oDUDeliveryChallanDetail.GYLotNo = oReader.GetString("GYLotNo");
            oDUDeliveryChallanDetail.DyeingOrderDetailID = oReader.GetInt32("DyeingOrderDetailID");
            oDUDeliveryChallanDetail.OrderNo = oReader.GetString("OrderNo");
            oDUDeliveryChallanDetail.PI_SampleNo = oReader.GetString("PI_SampleNo");
            oDUDeliveryChallanDetail.DONo = oReader.GetString("DONo");
            oDUDeliveryChallanDetail.RefNo = oReader.GetString("RefNo");
            oDUDeliveryChallanDetail.DeliveryPoint = oReader.GetString("DeliveryPoint");
            oDUDeliveryChallanDetail.HanksCone = oReader.GetInt32("HanksCone");
            oDUDeliveryChallanDetail.Note = oReader.GetString("Note");
            
            return oDUDeliveryChallanDetail;           
        }

      

        private DUDeliveryChallanDetail CreateObject(NullHandler oReader)
        {
            DUDeliveryChallanDetail oDUDeliveryChallanDetail = MapObject(oReader);
            return oDUDeliveryChallanDetail;
        }

        private List<DUDeliveryChallanDetail> CreateObjects(IDataReader oReader)
        {
            List<DUDeliveryChallanDetail> oDUDeliveryChallanDetail = new List<DUDeliveryChallanDetail>();
            NullHandler oHandler = new NullHandler(oReader);
            while (oReader.Read())
            {
                DUDeliveryChallanDetail oItem = CreateObject(oHandler);
                oDUDeliveryChallanDetail.Add(oItem);
            }
            return oDUDeliveryChallanDetail;
        }

        #region Get Lots
        private DUDeliveryChallanDetail MapObject_Lot(NullHandler oReader)
        {
            DUDeliveryChallanDetail oDUDeliveryChallanDetail = new DUDeliveryChallanDetail();

            oDUDeliveryChallanDetail.LotID = oReader.GetInt32("LotID");
            oDUDeliveryChallanDetail.ProductID = oReader.GetInt32("ProductID");
            oDUDeliveryChallanDetail.Qty = oReader.GetDouble("Qty");
            oDUDeliveryChallanDetail.LotNo = oReader.GetString("LotNo");
            oDUDeliveryChallanDetail.ProductName = oReader.GetString("ProductName");
            oDUDeliveryChallanDetail.ColorName = oReader.GetString("ColorName");
            oDUDeliveryChallanDetail.OrderID = oReader.GetInt32("OrderID");
            oDUDeliveryChallanDetail.DyeingOrderDetailID = oReader.GetInt32("DyeingOrderDetailID");
            oDUDeliveryChallanDetail.HanksCone = oReader.GetInt32("HanksCone");
            oDUDeliveryChallanDetail.BagQty = oReader.GetDouble("BagQty");
            return oDUDeliveryChallanDetail;
        }
        private DUDeliveryChallanDetail CreateObject_Lot(NullHandler oReader)
        {
            DUDeliveryChallanDetail oDUDeliveryChallanDetail = MapObject_Lot(oReader);
            return oDUDeliveryChallanDetail;
        }
        private List<DUDeliveryChallanDetail> CreateObjects_Lot(IDataReader oReader)
        {
            List<DUDeliveryChallanDetail> oDUDeliveryChallanDetail = new List<DUDeliveryChallanDetail>();
            NullHandler oHandler = new NullHandler(oReader);
            while (oReader.Read())
            {
                DUDeliveryChallanDetail oItem = CreateObject_Lot(oHandler);
                oDUDeliveryChallanDetail.Add(oItem);
            }
            return oDUDeliveryChallanDetail;
        }
        #endregion


        #endregion

        #region Interface implementation
        public DUDeliveryChallanDetailService() { }



        public DUDeliveryChallanDetail Save(DUDeliveryChallanDetail oDUDeliveryChallanDetail, Int64 nUserID)
        {
            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin(true);
                IDataReader reader;
                if (oDUDeliveryChallanDetail.DUDeliveryChallanDetailID <= 0)
                {
                    reader = DUDeliveryChallanDetailDA.InsertUpdate(tc, oDUDeliveryChallanDetail, EnumDBOperation.Insert, nUserID,"");
                }
                else
                {
                    reader = DUDeliveryChallanDetailDA.InsertUpdate(tc, oDUDeliveryChallanDetail, EnumDBOperation.Update, nUserID, "");
                }
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oDUDeliveryChallanDetail = new DUDeliveryChallanDetail();
                    oDUDeliveryChallanDetail = CreateObject(oReader);
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
                oDUDeliveryChallanDetail = new DUDeliveryChallanDetail();
                oDUDeliveryChallanDetail.ErrorMessage = e.Message.Split('~')[0];
                #endregion
            }
            return oDUDeliveryChallanDetail;
        }
        public DUDeliveryChallanDetail Get(int nDUDeliveryChallanDetailID, Int64 nUserId)
        {
            DUDeliveryChallanDetail oDUDeliveryChallanDetail = new DUDeliveryChallanDetail();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = DUDeliveryChallanDetailDA.Get(nDUDeliveryChallanDetailID, tc);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oDUDeliveryChallanDetail = CreateObject(oReader);
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
                oDUDeliveryChallanDetail.ErrorMessage = e.Message;
                #endregion
            }

            return oDUDeliveryChallanDetail;
        }
        public List<DUDeliveryChallanDetail> Gets(int nDUDeliveryChallanID, Int64 nUserID)
        {
            List<DUDeliveryChallanDetail> oDUDeliveryChallanDetail = null;

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = DUDeliveryChallanDetailDA.Gets(tc, nDUDeliveryChallanID);
                oDUDeliveryChallanDetail = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get DUDeliveryChallanDetail", e);
                #endregion
            }
            return oDUDeliveryChallanDetail;
        }
        public List<DUDeliveryChallanDetail> Gets_Lot(int nWorkingUnitID, int nDODetailID, int nPTUID,int nDyeingOrderDetailID, int nLotID, Int64 nUserID)
        {
            List<DUDeliveryChallanDetail> oDUDeliveryChallanDetail = null;

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = DUDeliveryChallanDetailDA.Gets_Lot( tc,nWorkingUnitID, nDODetailID, nPTUID,nDyeingOrderDetailID, nLotID);
                oDUDeliveryChallanDetail = CreateObjects_Lot(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get DUDeliveryChallanDetail", e);
                #endregion
            }
            return oDUDeliveryChallanDetail;
        }
      
        public List<DUDeliveryChallanDetail> Gets(string sSQL, Int64 nUserID)
        {
            List<DUDeliveryChallanDetail> oDUDeliveryChallanDetail = null;

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = DUDeliveryChallanDetailDA.Gets(sSQL, tc);
                oDUDeliveryChallanDetail = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get DUDeliveryChallanDetail", e);
                #endregion
            }
            return oDUDeliveryChallanDetail;
        }

      

        public string Delete(DUDeliveryChallanDetail oDUDeliveryChallanDetail, Int64 nUserId)
        {
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);

                DUDeliveryChallanDetailDA.Delete(tc, oDUDeliveryChallanDetail, EnumDBOperation.Delete, nUserId,"");
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
