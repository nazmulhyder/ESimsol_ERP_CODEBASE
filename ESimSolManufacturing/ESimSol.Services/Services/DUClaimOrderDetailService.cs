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

    public class DUClaimOrderDetailService : MarshalByRefObject, IDUClaimOrderDetailService
    {
        #region Private functions and declaration
        private DUClaimOrderDetail MapObject(NullHandler oReader)
        {
            DUClaimOrderDetail oDUClaimOrderDetail = new DUClaimOrderDetail();
            oDUClaimOrderDetail.DUClaimOrderDetailID = oReader.GetInt32("DUClaimOrderDetailID");
            oDUClaimOrderDetail.DUClaimOrderID = oReader.GetInt32("DUClaimOrderID");
            oDUClaimOrderDetail.DyeingOrderDetailID = oReader.GetInt32("DyeingOrderDetailID");
            oDUClaimOrderDetail.ParentDODetailID = oReader.GetInt32("ParentDODetailID");
            oDUClaimOrderDetail.Qty = oReader.GetDouble("Qty");
            oDUClaimOrderDetail.OrderQty = oReader.GetDouble("OrderQty");
            oDUClaimOrderDetail.LotID = oReader.GetInt32("LotID");
            oDUClaimOrderDetail.ProductName = oReader.GetString("ProductName");
            oDUClaimOrderDetail.ClaimOrderNo = oReader.GetString("ClaimOrderNo");

            oDUClaimOrderDetail.ClaimReasonID = oReader.GetInt32("ClaimReasonID");
            oDUClaimOrderDetail.ClaimRegion = oReader.GetString("ClaimRegion");
            oDUClaimOrderDetail.OrderNo = oReader.GetString("OrderNo");
            oDUClaimOrderDetail.ColorName = oReader.GetString("ColorName");
            oDUClaimOrderDetail.Shade = oReader.GetInt16("Shade");
            oDUClaimOrderDetail.ColorNo = oReader.GetString("ColorNo");
            oDUClaimOrderDetail.Note = oReader.GetString("Note");
          
            
            oDUClaimOrderDetail.DOQty = oReader.GetDouble("DOQty");
            oDUClaimOrderDetail.Qty_RS = oReader.GetDouble("Qty_RS");
            oDUClaimOrderDetail.ExportSCDetailID = oReader.GetInt32("ExportSCDetailID");
            oDUClaimOrderDetail.ProductID = oReader.GetInt32("ProductID");
            oDUClaimOrderDetail.PTUID = oReader.GetInt32("PTUID");
            oDUClaimOrderDetail.UnitPrice = oReader.GetDouble("UnitPrice");
            oDUClaimOrderDetail.NoOfCone = oReader.GetString("NoOfCone");
            oDUClaimOrderDetail.LengthOfCone = oReader.GetString("LengthOfCone");
            oDUClaimOrderDetail.NoOfCone_Weft = oReader.GetString("NoOfCone_Weft");
            oDUClaimOrderDetail.LengthOfCone_Weft = oReader.GetString("LengthOfCone_Weft");
            oDUClaimOrderDetail.DeliveryDate = oReader.GetDateTime("DeliveryDate");
            oDUClaimOrderDetail.LabDipType = oReader.GetInt16("LabDipType");
            oDUClaimOrderDetail.ApproveLotNo = oReader.GetString("ApproveLotNo");
            

            //derive
            oDUClaimOrderDetail.ProductName = oReader.GetString("ProductName");
            oDUClaimOrderDetail.LDNo = oReader.GetString("LDNo");
            oDUClaimOrderDetail.PantonNo = oReader.GetString("PantonNo");
            oDUClaimOrderDetail.BuyerCombo = oReader.GetInt16("BuyerCombo");
            //oDUClaimOrderDetail.Qty_Schedule = oReader.GetDouble("Qty_Schedule");
            oDUClaimOrderDetail.HankorCone = oReader.GetInt16("HankorCone");
            oDUClaimOrderDetail.ColorDevelopProduct = oReader.GetString("ColorDevelopProduct");
            oDUClaimOrderDetail.LabdipNo = oReader.GetString("LabdipNo");
            oDUClaimOrderDetail.MUnit = oReader.GetString("MUnit");
            oDUClaimOrderDetail.Status = oReader.GetInt16("Status");
            oDUClaimOrderDetail.BuyerRef = oReader.GetString("BuyerRef");
            oDUClaimOrderDetail.ChallanNo = oReader.GetString("ChallanNo");
            oDUClaimOrderDetail.BatchNo = oReader.GetString("BatchNo");
            oDUClaimOrderDetail.OrderDate = oReader.GetDateTime("OrderDate");
            oDUClaimOrderDetail.ClaimType = oReader.GetInt32("ClaimType");
            oDUClaimOrderDetail.BuyerID = oReader.GetInt32("BuyerID");
            oDUClaimOrderDetail.BuyerName = oReader.GetString("BuyerName");

            return oDUClaimOrderDetail;

        }

        private DUClaimOrderDetail CreateObject(NullHandler oReader)
        {
            DUClaimOrderDetail oDUClaimOrderDetail = new DUClaimOrderDetail();
            oDUClaimOrderDetail = MapObject(oReader);
            return oDUClaimOrderDetail;
        }

        private List<DUClaimOrderDetail> CreateObjects(IDataReader oReader)
        {
            List<DUClaimOrderDetail> oDUClaimOrderDetails = new List<DUClaimOrderDetail>();
            NullHandler oHandler = new NullHandler(oReader);
            while (oReader.Read())
            {
                DUClaimOrderDetail oItem = CreateObject(oHandler);
                oDUClaimOrderDetails.Add(oItem);
            }
            return oDUClaimOrderDetails;
        }
        #endregion

        #region Interface implementation
        public DUClaimOrderDetailService() { }

        public DUClaimOrderDetail Save(DUClaimOrderDetail oDUClaimOrderDetail, Int64 nUserID)
        {
            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin(true);
                IDataReader reader;
                if (oDUClaimOrderDetail.DUClaimOrderDetailID <= 0)
                {
                    reader = DUClaimOrderDetailDA.InsertUpdate(tc, oDUClaimOrderDetail, EnumDBOperation.Insert, nUserID, "");
                }
                else
                {
                    reader = DUClaimOrderDetailDA.InsertUpdate(tc, oDUClaimOrderDetail, EnumDBOperation.Update, nUserID, "");
                }
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oDUClaimOrderDetail = new DUClaimOrderDetail();
                    oDUClaimOrderDetail = CreateObject(oReader);
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
                oDUClaimOrderDetail = new DUClaimOrderDetail();
                oDUClaimOrderDetail.ErrorMessage = e.Message.Split('~')[0];
                #endregion
            }
            return oDUClaimOrderDetail;
        }
        public string Delete(DUClaimOrderDetail oDUClaimOrderDetail, Int64 nUserId)
        {
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);

                DUClaimOrderDetailDA.Delete(tc, oDUClaimOrderDetail, EnumDBOperation.Delete, nUserId, "");
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
        public DUClaimOrderDetail Get(int id, Int64 nUserId)
        {
            DUClaimOrderDetail oDUClaimOrderDetail = new DUClaimOrderDetail();

            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = DUClaimOrderDetailDA.Get(tc, id);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oDUClaimOrderDetail = CreateObject(oReader);
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
                throw new ServiceException("Failed to Get DUClaimOrderDetail", e);
                #endregion
            }

            return oDUClaimOrderDetail;
        }

        public List<DUClaimOrderDetail> Gets(int nDUDeliveryChallanID, Int64 nUserID)
        {
            List<DUClaimOrderDetail> oDUClaimOrderDetail = null;

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = DUClaimOrderDetailDA.Gets(tc, nDUDeliveryChallanID);
                oDUClaimOrderDetail = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get DUClaimOrderDetail", e);
                #endregion
            }
            return oDUClaimOrderDetail;
        }
        public List<DUClaimOrderDetail> Gets(string sSQL, Int64 nUserID)
        {
            List<DUClaimOrderDetail> oDUClaimOrderDetail = null;

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = DUClaimOrderDetailDA.Gets(tc, sSQL);
                oDUClaimOrderDetail = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get DUClaimOrderDetail", e);
                #endregion
            }
            return oDUClaimOrderDetail;
        }

        #endregion
    }
}
