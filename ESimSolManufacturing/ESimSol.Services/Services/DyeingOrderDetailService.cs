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
    public class DyeingOrderDetailService : MarshalByRefObject, IDyeingOrderDetailService
    {
        #region Private functions and declaration
        private DyeingOrderDetail MapObject(NullHandler oReader)
        {
            DyeingOrderDetail oDyeingOrderDetail = new DyeingOrderDetail();
            oDyeingOrderDetail.DyeingOrderDetailID = oReader.GetInt32("DyeingOrderDetailID");
            oDyeingOrderDetail.DyeingOrderID = oReader.GetInt32("DyeingOrderID");
            oDyeingOrderDetail.LabDipDetailID = oReader.GetInt32("LabDipDetailID");
            oDyeingOrderDetail.Shade = oReader.GetInt16("Shade");
            oDyeingOrderDetail.ExportSCDetailID = oReader.GetInt32("ExportSCDetailID");
            oDyeingOrderDetail.ProductID = oReader.GetInt32("ProductID");
            oDyeingOrderDetail.PTUID = oReader.GetInt32("PTUID");
            oDyeingOrderDetail.Qty = oReader.GetDouble("Qty");
            oDyeingOrderDetail.UnitPrice = oReader.GetDouble("UnitPrice");
            oDyeingOrderDetail.NoOfCone = oReader.GetString("NoOfCone");
            oDyeingOrderDetail.LengthOfCone = oReader.GetString("LengthOfCone");
            oDyeingOrderDetail.NoOfCone_Weft = oReader.GetString("NoOfCone_Weft");
            oDyeingOrderDetail.LengthOfCone_Weft = oReader.GetString("LengthOfCone_Weft");
            oDyeingOrderDetail.DeliveryDate = oReader.GetDateTime("DeliveryDate");
            oDyeingOrderDetail.Note = oReader.GetString("Note");
            oDyeingOrderDetail.LabDipType = oReader.GetInt16("LabDipType");
            oDyeingOrderDetail.ApproveLotNo = oReader.GetString("ApproveLotNo");
            //derive
            oDyeingOrderDetail.ProductNameCode = oReader.GetString("ProductName");
            oDyeingOrderDetail.ProductCodeName = oReader.GetString("ProductNameCode");
            oDyeingOrderDetail.ProductName = oReader.GetString("ProductName");
            oDyeingOrderDetail.OrderNo = oReader.GetString("OrderNo");
            oDyeingOrderDetail.ColorNo = oReader.GetString("ColorNo");
            oDyeingOrderDetail.LDNo = oReader.GetString("LDNo");
            oDyeingOrderDetail.ColorName = oReader.GetString("ColorName");
            oDyeingOrderDetail.PantonNo = oReader.GetString("PantonNo");
            oDyeingOrderDetail.BuyerCombo = oReader.GetInt16("BuyerCombo");
            //oDyeingOrderDetail.Qty_Schedule = oReader.GetDouble("Qty_Schedule");
            oDyeingOrderDetail.HankorCone = oReader.GetInt16("HankorCone");
            oDyeingOrderDetail.ColorDevelopProduct = oReader.GetString("ColorDevelopProduct");
            oDyeingOrderDetail.LabdipNo = oReader.GetString("LabdipNo");
            oDyeingOrderDetail.MUnit = oReader.GetString("MUnit");
            oDyeingOrderDetail.MUnitID = oReader.GetInt32("MUnitID");
            oDyeingOrderDetail.Status = oReader.GetInt16("Status");
            oDyeingOrderDetail.BuyerRef = oReader.GetString("BuyerRef");
            oDyeingOrderDetail.Qty_Pro = oReader.GetDouble("Qty_Pro");
            oDyeingOrderDetail.DOQty = oReader.GetDouble("DOQty");
            oDyeingOrderDetail.BuyerName = oReader.GetString("BuyerName");
            oDyeingOrderDetail.JobNo = oReader.GetString("JobNo");
            oDyeingOrderDetail.SL = oReader.GetInt32("SL");
            oDyeingOrderDetail.OrderDateNew = oReader.GetDateTime("OrderDate");

            //History
            oDyeingOrderDetail.DyeingOrderHistoryID = oReader.GetInt32("DyeingOrderHistoryID");
            oDyeingOrderDetail.OprationDate = oReader.GetDateTime("OprationDate");
            oDyeingOrderDetail.CurrentStatus = oReader.GetInt32("CurrentStatus");
            oDyeingOrderDetail.PreviousStatus = oReader.GetInt32("PreviousStatus");
            oDyeingOrderDetail.Note_System = oReader.GetString("Note_System");
            oDyeingOrderDetail.HistoryNote = oReader.GetString("HistoryNote");
            oDyeingOrderDetail.UserName = oReader.GetString("UserName");
            oDyeingOrderDetail.YetQty = oReader.GetDouble("YetQty");
            
            return oDyeingOrderDetail;           
        }

        private DyeingOrderDetail CreateObject(NullHandler oReader)
        {
            DyeingOrderDetail oDyeingOrderDetail = MapObject(oReader);
            return oDyeingOrderDetail;
        }

        private List<DyeingOrderDetail> CreateObjects(IDataReader oReader)
        {
            List<DyeingOrderDetail> oDyeingOrderDetail = new List<DyeingOrderDetail>();
            NullHandler oHandler = new NullHandler(oReader);
            while (oReader.Read())
            {
                DyeingOrderDetail oItem = CreateObject(oHandler);
                oDyeingOrderDetail.Add(oItem);
            }
            return oDyeingOrderDetail;
        }

        #endregion

        #region Interface implementation
        public DyeingOrderDetailService() { }

        public DyeingOrderDetail Save(DyeingOrderDetail oDyeingOrderDetail, Int64 nUserID)
        {
            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin(true);
                IDataReader reader;
                if (oDyeingOrderDetail.DyeingOrderDetailID <= 0)
                {
                    reader = DyeingOrderDetailDA.InsertUpdate(tc, oDyeingOrderDetail, EnumDBOperation.Insert, nUserID,"");
                }
                else
                {
                    reader = DyeingOrderDetailDA.InsertUpdate_PTU_DO(tc, oDyeingOrderDetail, EnumDBOperation.Update, nUserID);
                }
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oDyeingOrderDetail = new DyeingOrderDetail();
                    oDyeingOrderDetail = CreateObject(oReader);
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
                oDyeingOrderDetail = new DyeingOrderDetail();
                oDyeingOrderDetail.ErrorMessage = e.Message.Split('~')[0];
                #endregion
            }
            return oDyeingOrderDetail;
        }
        public DyeingOrderDetail OrderHold(DyeingOrderDetail oDyeingOrderDetail, Int64 nUserId)
        {
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = DyeingOrderDetailDA.OrderHold(tc, oDyeingOrderDetail);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oDyeingOrderDetail = CreateObject(oReader);
                }
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                oDyeingOrderDetail = new DyeingOrderDetail();
                oDyeingOrderDetail.ErrorMessage = e.Message.Split('~')[0];
                #endregion
            }
            return oDyeingOrderDetail;
        }
        public List<DyeingOrderDetail> MakeTwistedGroup(string sDyeingOrderDetailID, int nDyeingOrderID, int nTwistedGroup, int nDBOperation, int nUserID)
        {
            List<DyeingOrderDetail> oDyeingOrderDetails = new List<DyeingOrderDetail>();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = DyeingOrderDetailDA.MakeTwistedGroup(tc, sDyeingOrderDetailID, nDyeingOrderID, nTwistedGroup, nDBOperation, nUserID);
                oDyeingOrderDetails = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                DyeingOrderDetail oDyeingOrderDetail = new DyeingOrderDetail();
                oDyeingOrderDetail.ErrorMessage = e.Message;
                oDyeingOrderDetails.Add(oDyeingOrderDetail);
                #endregion
            }

            return oDyeingOrderDetails;
        }
        public string Delete(DyeingOrderDetail oDyeingOrderDetail, Int64 nUserId)
        {
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);

                //  DyeingOrderDetailDA.Delete(tc, oDyeingOrderDetail, EnumDBOperation.Delete, nUserId,"");
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
        public DyeingOrderDetail Get(int nDyeingOrderDetailID, Int64 nUserId)
        {
            DyeingOrderDetail oDyeingOrderDetail = new DyeingOrderDetail();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = DyeingOrderDetailDA.Get(nDyeingOrderDetailID, tc);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oDyeingOrderDetail = CreateObject(oReader);
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
                oDyeingOrderDetail.ErrorMessage = e.Message;
                #endregion
            }

            return oDyeingOrderDetail;
        }
        public List<DyeingOrderDetail> Gets(int nDyeingOrderID, Int64 nUserID)
        {
            List<DyeingOrderDetail> oDyeingOrderDetail = null;

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = DyeingOrderDetailDA.Gets(tc, nDyeingOrderID);
                oDyeingOrderDetail = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get DyeingOrderDetail", e);
                #endregion
            }
            return oDyeingOrderDetail;
        }

        public List<DyeingOrderDetail> GetsBy(int nSampleInvoiceID, Int64 nUserID)
        {
            List<DyeingOrderDetail> oDyeingOrderDetail = null;

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = DyeingOrderDetailDA.GetsBy(tc, nSampleInvoiceID);
                oDyeingOrderDetail = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get DyeingOrderDetail", e);
                #endregion
            }
            return oDyeingOrderDetail;
        }
        public List<DyeingOrderDetail> GetsBy(int nLabDipDetailID, int nOrderID, int nOrderType, Int64 nUserID)
        {
            List<DyeingOrderDetail> oDyeingOrderDetail = null;

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = DyeingOrderDetailDA.GetsBy(tc,  nLabDipDetailID,  nOrderID,  nOrderType);
                oDyeingOrderDetail = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get DyeingOrderDetail", e);
                #endregion
            }
            return oDyeingOrderDetail;
        }

        public List<DyeingOrderDetail> Gets(string sSQL, Int64 nUserID)
        {
            List<DyeingOrderDetail> oDyeingOrderDetail = null;

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = DyeingOrderDetailDA.Gets(sSQL, tc);
                oDyeingOrderDetail = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get DyeingOrderDetail", e);
                #endregion
            }
            return oDyeingOrderDetail;
        }

      

       

        #endregion


        #region Loan Order
        private DyeingOrderDetail MapObjectLoanOrder(NullHandler oReader)
        {
            DyeingOrderDetail oDyeingOrderDetail = new DyeingOrderDetail();
            oDyeingOrderDetail.DyeingOrderID = oReader.GetInt32("DyeingOrderID");
            oDyeingOrderDetail.DyeingOrderDetailID = oReader.GetInt32("DyeingOrderDetailID");
            oDyeingOrderDetail.ProductID = oReader.GetInt32("ProductID");
            oDyeingOrderDetail.Status = oReader.GetInt32("Status");
            oDyeingOrderDetail.ContractorID = oReader.GetInt32("ContractorID");
            oDyeingOrderDetail.DUProGuideLineID = oReader.GetInt32("DUProGuideLineID");
            oDyeingOrderDetail.ReceiveProductID = oReader.GetInt32("ReceiveProductID");
            oDyeingOrderDetail.ReceiveProductMUnitID = oReader.GetInt32("ReceiveProductMUnitID");
            oDyeingOrderDetail.MUnitID = oReader.GetInt32("MUnitID");
            oDyeingOrderDetail.DeliveryChallanProductID = oReader.GetInt32("DeliveryChallanProductID");
            oDyeingOrderDetail.DeliveryChallanDetailID = oReader.GetInt32("DeliveryChallanDetailID");
            oDyeingOrderDetail.DeliveryChallanMUnitID = oReader.GetInt32("DeliveryChallanMUnitID");
            oDyeingOrderDetail.OrderNo = oReader.GetString("OrderNo");
            oDyeingOrderDetail.ContractorName = oReader.GetString("ContractorName");
            oDyeingOrderDetail.MUnitName = oReader.GetString("MUnit");
            oDyeingOrderDetail.ReceiveProductName = oReader.GetString("ReceiveProductName");
            oDyeingOrderDetail.ReceiveProductMUnitName = oReader.GetString("ReceiveProductMUnitName");
            oDyeingOrderDetail.DeliveryChallanMUnitName = oReader.GetString("DeliveryChallanMUnitName");
            oDyeingOrderDetail.DeliveryChallanProductName = oReader.GetString("DeliveryChallanProductName");
            oDyeingOrderDetail.ReceiveSLNo = oReader.GetString("ReceiveSLNo");
            oDyeingOrderDetail.ProductName = oReader.GetString("ProductName");
            oDyeingOrderDetail.DeliveryChallanNo = oReader.GetString("DeliveryChallanNo");
            oDyeingOrderDetail.OrderDate = oReader.GetDateTime("OrderDate").ToString("dd MMM yyyy");
            oDyeingOrderDetail.ReceiveDate = oReader.GetDateTime("ReceiveDate");
            oDyeingOrderDetail.ReceiveDate = oReader.GetDateTime("ReceiveDate");
            oDyeingOrderDetail.Qty = oReader.GetDouble("Qty");
            oDyeingOrderDetail.DeliveryChallanQty = oReader.GetDouble("DeliveryChallanQty");
            oDyeingOrderDetail.ReceiveProductQty = oReader.GetDouble("ReceiveProductQty");
            oDyeingOrderDetail.YetQty = oReader.GetDouble("YetQty");

            return oDyeingOrderDetail;
        }

      

      
        //public List<DyeingOrderDetail> GetsLoanOrder(string sSQL, Int64 nUserID)
        //{
        //    List<DyeingOrderDetail> oDyeingOrderDetail = null;

        //    TransactionContext tc = null;

        //    try
        //    {
        //        tc = TransactionContext.Begin();

        //        IDataReader reader = null;
        //        reader = DyeingOrderDetailDA.GetsLoanOrder(sSQL, tc);
        //        oDyeingOrderDetail = CreateObjectsLoanOrder(reader);
        //        reader.Close();
        //        tc.End();
        //    }
        //    catch (Exception e)
        //    {
        //        #region Handle Exception
        //        if (tc != null)
        //            tc.HandleError();
        //        ExceptionLog.Write(e);
        //        throw new ServiceException("Failed to Get DyeingOrderDetail", e);
        //        #endregion
        //    }
        //    return oDyeingOrderDetail;
        //}
        #endregion
    }
}
