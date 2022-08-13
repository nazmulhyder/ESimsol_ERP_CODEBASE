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

    public class DUClaimOrderService : MarshalByRefObject, IDUClaimOrderService
    {
        #region Private functions and declaration
        private DUClaimOrder MapObject(NullHandler oReader)
        {
            DUClaimOrder oDUClaimOrder = new DUClaimOrder();
            oDUClaimOrder.DUClaimOrderID = oReader.GetInt32("DUClaimOrderID");
            oDUClaimOrder.DyeingOrderID = oReader.GetInt32("DyeingOrderID");
            oDUClaimOrder.ClaimOrderNo = oReader.GetString("OrderNo");
            oDUClaimOrder.OrderDate = oReader.GetDateTime("OrderDate");
            oDUClaimOrder.Note = oReader.GetString("Note");
            oDUClaimOrder.Note_Checked = oReader.GetString("Note_Checked");
            oDUClaimOrder.Note_Approve = oReader.GetString("Note_Approve");
            oDUClaimOrder.ExportPIID = oReader.GetInt32("ExportPIID");
            oDUClaimOrder.OrderType = oReader.GetInt32("OrderType");
            oDUClaimOrder.ApproveBy = oReader.GetInt32("ApproveBy");
            oDUClaimOrder.ClaimType = oReader.GetInt32("ClaimType");
            oDUClaimOrder.Qty = oReader.GetDouble("Qty");
            oDUClaimOrder.ApproveDate = oReader.GetDateTime("ApproveDate");
            oDUClaimOrder.CheckedBy = oReader.GetInt32("CheckedBy");
            oDUClaimOrder.PreaperByName = oReader.GetString("PreaperByName");
            oDUClaimOrder.CheckedByName = oReader.GetString("CheckedByName");
            oDUClaimOrder.ApproveByName = oReader.GetString("ApproveByName");
            oDUClaimOrder.ContractorName = oReader.GetString("ContractorName");
            oDUClaimOrder.PINo = oReader.GetString("PINo");
            oDUClaimOrder.DeliveryZone = oReader.GetString("DeliveryZoneName");
            oDUClaimOrder.StyleNo = oReader.GetString("StyleNo");
            oDUClaimOrder.RefNo = oReader.GetString("RefNo");
            oDUClaimOrder.ContactPersonnelName = oReader.GetString("ContactPersonnelName");
            //oDUClaimOrder.LCNo = oReader.GetString("LCNo");
            oDUClaimOrder.ContractorID = oReader.GetInt32("ContractorID");
            oDUClaimOrder.DUReturnChallanID = oReader.GetInt32("DUReturnChallanID");
            oDUClaimOrder.ParentDONo = oReader.GetString("ParentDONo");
            oDUClaimOrder.DUReturnChallanNo = oReader.GetString("DUReturnChallanNo");
            oDUClaimOrder.DUReturnChallanID = oReader.GetInt32("DUReturnChallanID");
            oDUClaimOrder.PaymentType = oReader.GetInt32("PaymentType");
            oDUClaimOrder.ParentDOID = oReader.GetInt32("ParentDOID");
            oDUClaimOrder.StatusDo = (EnumDyeingOrderState)oReader.GetInt32("Status");
            oDUClaimOrder.IsClose = oReader.GetBoolean("IsClose");
            return oDUClaimOrder;
        }

        private DUClaimOrder CreateObject(NullHandler oReader)
        {
            DUClaimOrder oDUClaimOrder = new DUClaimOrder();
            oDUClaimOrder = MapObject(oReader);
            return oDUClaimOrder;
        }

        private List<DUClaimOrder> CreateObjects(IDataReader oReader)
        {
            List<DUClaimOrder> oDUClaimOrders = new List<DUClaimOrder>();
            NullHandler oHandler = new NullHandler(oReader);
            while (oReader.Read())
            {
                DUClaimOrder oItem = CreateObject(oHandler);
                oDUClaimOrders.Add(oItem);
            }
            return oDUClaimOrders;
        }
        #endregion

        #region Interface implementation
        public DUClaimOrderService() { }

        public DUClaimOrder Save(DUClaimOrder oDUClaimOrder, Int64 nUserID)
        {
            TransactionContext tc = null;
            List<DUClaimOrderDetail> oDUClaimOrderDetails = new List<DUClaimOrderDetail>();
            String sDUClaimOrderDetaillIDs = "";
            try
            {
                oDUClaimOrderDetails = oDUClaimOrder.DUClaimOrderDetails;

                tc = TransactionContext.Begin(true);
                IDataReader reader;
                if (oDUClaimOrder.DUClaimOrderID <= 0)
                {
                    reader = DUClaimOrderDA.InsertUpdate(tc, oDUClaimOrder, EnumDBOperation.Insert, nUserID);
                }
                else
                {
                    reader = DUClaimOrderDA.InsertUpdate(tc, oDUClaimOrder, EnumDBOperation.Update, nUserID);
                }
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oDUClaimOrder = new DUClaimOrder();
                    oDUClaimOrder = CreateObject(oReader);
                }
                reader.Close();

                #region DUClaimOrderDetails Part
                if (oDUClaimOrderDetails != null)
                {
                    foreach (DUClaimOrderDetail oItem in oDUClaimOrderDetails)
                    {
                        if (oItem.Qty > 0)
                        {
                            IDataReader readertnc;
                            oItem.DUClaimOrderID = oDUClaimOrder.DUClaimOrderID;
                            if (oItem.DUClaimOrderDetailID <= 0)
                            {
                                readertnc = DUClaimOrderDetailDA.InsertUpdate(tc, oItem, EnumDBOperation.Insert, nUserID, "");
                            }
                            else
                            {
                                readertnc = DUClaimOrderDetailDA.InsertUpdate(tc, oItem, EnumDBOperation.Update, nUserID, "");
                            }
                            NullHandler oReaderTNC = new NullHandler(readertnc);

                            if (readertnc.Read())
                            {
                                sDUClaimOrderDetaillIDs = sDUClaimOrderDetaillIDs + oReaderTNC.GetString("DUClaimOrderDetailID") + ",";
                            }
                            readertnc.Close();
                        }
                    }
                    if (sDUClaimOrderDetaillIDs.Length > 0)
                    {
                        sDUClaimOrderDetaillIDs = sDUClaimOrderDetaillIDs.Remove(sDUClaimOrderDetaillIDs.Length - 1, 1);
                    }
                    DUClaimOrderDetail oDUClaimOrderDetail = new DUClaimOrderDetail();
                    oDUClaimOrderDetail.DUClaimOrderID = oDUClaimOrder.DUClaimOrderID;
                    DUClaimOrderDetailDA.Delete(tc, oDUClaimOrderDetail, EnumDBOperation.Delete, nUserID, sDUClaimOrderDetaillIDs);
                    sDUClaimOrderDetaillIDs = "";

                }
                #endregion

                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                //ExceptionLog.Write(e);
                //throw new ServiceException("Failed to . Because of " + e.Message, e);
                oDUClaimOrder = new DUClaimOrder();
                oDUClaimOrder.ErrorMessage = e.Message.Split('~')[0];
                #endregion
            }
            return oDUClaimOrder;
        }
        public DUClaimOrder Save_Log(DUClaimOrder oDUClaimOrder, Int64 nUserID)
        {
            TransactionContext tc = null;
            List<DUClaimOrderDetail> oDUClaimOrderDetails = new List<DUClaimOrderDetail>();
            String sDUClaimOrderDetaillIDs = "";
            try
            {
                oDUClaimOrderDetails = oDUClaimOrder.DUClaimOrderDetails;

                tc = TransactionContext.Begin(true);
                IDataReader reader;
              
                reader = DUClaimOrderDA.InsertUpdate_Log(tc, oDUClaimOrder, EnumDBOperation.Update, nUserID);
        
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oDUClaimOrder = new DUClaimOrder();
                    oDUClaimOrder = CreateObject(oReader);
                }
                reader.Close();

                #region DUClaimOrderDetails Part
                if (oDUClaimOrderDetails != null)
                {
                    foreach (DUClaimOrderDetail oItem in oDUClaimOrderDetails)
                    {
                        IDataReader readertnc;
                        oItem.DUClaimOrderID = oDUClaimOrder.DUClaimOrderID;
                        if (oItem.DUClaimOrderDetailID <= 0)
                        {
                            readertnc = DUClaimOrderDetailDA.InsertUpdate(tc, oItem, EnumDBOperation.Insert, nUserID, "");
                        }
                        else
                        {
                            readertnc = DUClaimOrderDetailDA.InsertUpdate(tc, oItem, EnumDBOperation.Update, nUserID, "");
                        }
                        NullHandler oReaderTNC = new NullHandler(readertnc);

                        if (readertnc.Read())
                        {
                            sDUClaimOrderDetaillIDs = sDUClaimOrderDetaillIDs + oReaderTNC.GetString("DUClaimOrderDetailID") + ",";
                        }
                        readertnc.Close();
                    }
                    if (sDUClaimOrderDetaillIDs.Length > 0)
                    {
                        sDUClaimOrderDetaillIDs = sDUClaimOrderDetaillIDs.Remove(sDUClaimOrderDetaillIDs.Length - 1, 1);
                    }
                    DUClaimOrderDetail oDUClaimOrderDetail = new DUClaimOrderDetail();
                    oDUClaimOrderDetail.DUClaimOrderID = oDUClaimOrder.DUClaimOrderID;
                    DUClaimOrderDetailDA.Delete(tc, oDUClaimOrderDetail, EnumDBOperation.Delete, nUserID, sDUClaimOrderDetaillIDs);
                    sDUClaimOrderDetaillIDs = "";

                }
                #endregion

                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                //ExceptionLog.Write(e);
                //throw new ServiceException("Failed to . Because of " + e.Message, e);
                oDUClaimOrder = new DUClaimOrder();
                oDUClaimOrder.ErrorMessage = e.Message.Split('~')[0];
                #endregion
            }
            return oDUClaimOrder;
        }
        public DUClaimOrder Approve(DUClaimOrder oDUClaimOrder, Int64 nUserID)
        {
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader;
                reader = DUClaimOrderDA.InsertUpdate(tc, oDUClaimOrder, EnumDBOperation.Approval, nUserID);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oDUClaimOrder = CreateObject(oReader);
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
                //throw new ServiceException("Failed to Get DUClaimOrder", e);
                oDUClaimOrder.ErrorMessage = e.Message.Split('~')[0];
                #endregion
            }

            return oDUClaimOrder;
        }
        public DUClaimOrder Checked(DUClaimOrder oDUClaimOrder, Int64 nUserID)
        {
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader;
                reader = DUClaimOrderDA.InsertUpdate(tc, oDUClaimOrder, EnumDBOperation.Request, nUserID);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oDUClaimOrder = CreateObject(oReader);
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
                //throw new ServiceException("Failed to Get DUClaimOrder", e);
                oDUClaimOrder.ErrorMessage = e.Message.Split('~')[0];
                #endregion
            }

            return oDUClaimOrder;
        }

        public DUClaimOrder Get(int nDOID, Int64 nUserId)
        {
            DUClaimOrder oDUClaimOrder = new DUClaimOrder();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = DUClaimOrderDA.Get(nDOID, tc);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oDUClaimOrder = CreateObject(oReader);
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
                //throw new ServiceException("Failed to Get DUClaimOrder", e);
                oDUClaimOrder.ErrorMessage = e.Message;
                #endregion
            }

            return oDUClaimOrder;
        }

        public List<DUClaimOrder> GetsBy(string sContractorID, Int64 nUserID)
        {
            List<DUClaimOrder> oDUClaimOrder = null;

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = DUClaimOrderDA.GetsBy(tc, sContractorID);
                oDUClaimOrder = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get DUClaimOrder", e);
                #endregion
            }
            return oDUClaimOrder;
        }
        public List<DUClaimOrder> GetsByPI(int nExportPIID, Int64 nUserID)
        {
            List<DUClaimOrder> oDUClaimOrder = null;

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = DUClaimOrderDA.GetsByPI(tc, nExportPIID);
                oDUClaimOrder = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get DUClaimOrder", e);
                #endregion
            }
            return oDUClaimOrder;
        }
        public DUClaimOrder DOSave_Auto(DUClaimOrder oDUClaimOrder, Int64 nUserID)
        {
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader;
                reader = DUClaimOrderDA.InsertUpdate_DeliveryOrder(tc, oDUClaimOrder, EnumDBOperation.Insert, nUserID);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oDUClaimOrder = CreateObject(oReader);
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
                //throw new ServiceException("Failed to Get DUClaimOrder", e);
                oDUClaimOrder.ErrorMessage = e.Message.Split('~')[0];
                #endregion
            }

            return oDUClaimOrder;
        }
        public string Delete(DUClaimOrder oDUClaimOrder, Int64 nUserId)
        {
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);

                DUClaimOrderDA.Delete(tc, oDUClaimOrder, EnumDBOperation.Delete, nUserId);
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
        public List<DUClaimOrder> Gets(string sSQL, Int64 nUserID)
        {
            List<DUClaimOrder> oDUClaimOrder = null;

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = DUClaimOrderDA.Gets(sSQL, tc);
                oDUClaimOrder = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get DUClaimOrder", e);
                #endregion
            }
            return oDUClaimOrder;
        }

        #endregion
    }
}
