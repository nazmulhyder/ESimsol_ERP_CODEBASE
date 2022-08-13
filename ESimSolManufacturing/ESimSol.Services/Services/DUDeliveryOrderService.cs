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
    public class DUDeliveryOrderService : MarshalByRefObject, IDUDeliveryOrderService
    {
        #region Private functions and declaration
        private DUDeliveryOrder MapObject(NullHandler oReader)
        {
            DUDeliveryOrder oDUDeliveryOrder = new DUDeliveryOrder();
            oDUDeliveryOrder.DUDeliveryOrderID = oReader.GetInt32("DUDeliveryOrderID");
            oDUDeliveryOrder.DONo = oReader.GetString("DONo");
            oDUDeliveryOrder.DODate = oReader.GetDateTime("DODate");
            oDUDeliveryOrder.ContractorID = oReader.GetInt32("ContractorID");
            //oDUDeliveryOrder.ContactPersonnelID = oReader.GetInt32("ContactPersonnelID");
            oDUDeliveryOrder.DeliveryDate = oReader.GetDateTime("DeliveryDate");
            oDUDeliveryOrder.DeliveryPoint = oReader.GetString("DeliveryPoint");
            oDUDeliveryOrder.OrderType = oReader.GetInt32("OrderType");
            oDUDeliveryOrder.OrderID = oReader.GetInt32("OrderID");
            oDUDeliveryOrder.ExportPIID = oReader.GetInt32("ExportPIID");
            oDUDeliveryOrder.ApproveBy = oReader.GetInt32("ApproveBy");
            oDUDeliveryOrder.Note = oReader.GetString("Note");
            //oDUDeliveryOrder.ApproveDate = oReader.GetDateTime("ApproveDate");
            ////derive
            //oDUDeliveryOrder.ContractorName = oReader.GetString("ContractorName");
            oDUDeliveryOrder.DeliveryToName = oReader.GetString("DeliveryToName");
            oDUDeliveryOrder.PreaperByName = oReader.GetString("PreaperByName");
            oDUDeliveryOrder.ApproveByName = oReader.GetString("ApproveByName");
            oDUDeliveryOrder.OrderNo = oReader.GetString("OrderNo");
            oDUDeliveryOrder.Qty = oReader.GetDouble("Qty");
            oDUDeliveryOrder.DOStatus = oReader.GetInt32("DOStatus");
            oDUDeliveryOrder.OrderTypeSt = oReader.GetString("OrderTypeSt");
            oDUDeliveryOrder.DONoCode = oReader.GetString("DONoCode");
            oDUDeliveryOrder.ExportPINo = oReader.GetString("ExportPINo");
            
            return oDUDeliveryOrder;

        }


        private DUDeliveryOrder CreateObject(NullHandler oReader)
        {
            DUDeliveryOrder oDUDeliveryOrder = MapObject(oReader);
            return oDUDeliveryOrder;
        }

        private List<DUDeliveryOrder> CreateObjects(IDataReader oReader)
        {
            List<DUDeliveryOrder> oDUDeliveryOrders = new List<DUDeliveryOrder>();
            NullHandler oHandler = new NullHandler(oReader);
            while (oReader.Read())
            {
                DUDeliveryOrder oItem = CreateObject(oHandler);
                oDUDeliveryOrders.Add(oItem);
            }
            return oDUDeliveryOrders;
        }

        #endregion

        #region Interface implementation
        public DUDeliveryOrderService() { }

        public DUDeliveryOrder Save(DUDeliveryOrder oDUDeliveryOrder, Int64 nUserID)
        {
            TransactionContext tc = null;
            List<DUDeliveryOrderDetail> oDUDeliveryOrderDetails = new List<DUDeliveryOrderDetail>();
            String sDUDeliveryOrderDetaillIDs = "";
            try
            {
                oDUDeliveryOrderDetails = oDUDeliveryOrder.DUDeliveryOrderDetails;

                tc = TransactionContext.Begin(true);
                IDataReader reader;
                if (oDUDeliveryOrder.DUDeliveryOrderID <= 0)
                {
                    reader = DUDeliveryOrderDA.InsertUpdate(tc, oDUDeliveryOrder, EnumDBOperation.Insert, nUserID);
                }
                else
                {
                    reader = DUDeliveryOrderDA.InsertUpdate(tc, oDUDeliveryOrder, EnumDBOperation.Update, nUserID);
                }
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oDUDeliveryOrder = new DUDeliveryOrder();
                    oDUDeliveryOrder = CreateObject(oReader);
                }
                reader.Close();

                #region DUDeliveryOrderDetails Part
                if (oDUDeliveryOrderDetails != null)
                {
                    foreach (DUDeliveryOrderDetail oItem in oDUDeliveryOrderDetails)
                    {
                        IDataReader readertnc;
                        oItem.DUDeliveryOrderID = oDUDeliveryOrder.DUDeliveryOrderID;
                        if (oItem.DUDeliveryOrderDetailID <= 0)
                        {
                            readertnc = DUDeliveryOrderDetailDA.InsertUpdate(tc, oItem, EnumDBOperation.Insert, nUserID,"");
                        }
                        else
                        {
                            readertnc = DUDeliveryOrderDetailDA.InsertUpdate(tc, oItem, EnumDBOperation.Update, nUserID,"");
                        }
                        NullHandler oReaderTNC = new NullHandler(readertnc);

                        if (readertnc.Read())
                        {
                            sDUDeliveryOrderDetaillIDs = sDUDeliveryOrderDetaillIDs + oReaderTNC.GetString("DUDeliveryOrderDetailID") + ",";
                        }
                        readertnc.Close();
                    }
                    if (sDUDeliveryOrderDetaillIDs.Length > 0)
                    {
                        sDUDeliveryOrderDetaillIDs = sDUDeliveryOrderDetaillIDs.Remove(sDUDeliveryOrderDetaillIDs.Length - 1, 1);
                    }
                    DUDeliveryOrderDetail oDUDeliveryOrderDetail = new DUDeliveryOrderDetail();
                    oDUDeliveryOrderDetail.DUDeliveryOrderID = oDUDeliveryOrder.DUDeliveryOrderID;
                    DUDeliveryOrderDetailDA.Delete(tc, oDUDeliveryOrderDetail, EnumDBOperation.Delete, nUserID, sDUDeliveryOrderDetaillIDs);
                    sDUDeliveryOrderDetaillIDs = "";

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
                oDUDeliveryOrder = new DUDeliveryOrder();
                oDUDeliveryOrder.ErrorMessage = e.Message.Split('~')[0];
                #endregion
            }
            return oDUDeliveryOrder;
        }
        public DUDeliveryOrder Save_Log(DUDeliveryOrder oDUDeliveryOrder, Int64 nUserID)
        {
            TransactionContext tc = null;
            List<DUDeliveryOrderDetail> oDUDeliveryOrderDetails = new List<DUDeliveryOrderDetail>();
            String sDUDeliveryOrderDetaillIDs = "";
            try
            {
                oDUDeliveryOrderDetails = oDUDeliveryOrder.DUDeliveryOrderDetails;

                tc = TransactionContext.Begin(true);
                IDataReader reader;
                
                reader = DUDeliveryOrderDA.InsertUpdate_Log(tc, oDUDeliveryOrder, EnumDBOperation.Revise, nUserID);
                
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oDUDeliveryOrder = new DUDeliveryOrder();
                    oDUDeliveryOrder = CreateObject(oReader);
                }
                reader.Close();

                #region DUDeliveryOrderDetails Part
                if (oDUDeliveryOrderDetails != null)
                {
                    foreach (DUDeliveryOrderDetail oItem in oDUDeliveryOrderDetails)
                    {
                        IDataReader readertnc;
                        oItem.DUDeliveryOrderID = oDUDeliveryOrder.DUDeliveryOrderID;
                        if (oItem.DUDeliveryOrderDetailID <= 0)
                        {
                            readertnc = DUDeliveryOrderDetailDA.InsertUpdate(tc, oItem, EnumDBOperation.Insert, nUserID,"");
                        }
                        else
                        {
                            readertnc = DUDeliveryOrderDetailDA.InsertUpdate(tc, oItem, EnumDBOperation.Update, nUserID,"");
                        }
                        NullHandler oReaderTNC = new NullHandler(readertnc);

                        if (readertnc.Read())
                        {
                            sDUDeliveryOrderDetaillIDs = sDUDeliveryOrderDetaillIDs + oReaderTNC.GetString("DUDeliveryOrderDetailID") + ",";
                        }
                        readertnc.Close();
                    }

                    if (sDUDeliveryOrderDetaillIDs.Length > 0)
                    {
                        sDUDeliveryOrderDetaillIDs = sDUDeliveryOrderDetaillIDs.Remove(sDUDeliveryOrderDetaillIDs.Length - 1, 1);
                    }
                    DUDeliveryOrderDetail oDUDeliveryOrderDetail = new DUDeliveryOrderDetail();
                    oDUDeliveryOrderDetail.DUDeliveryOrderID = oDUDeliveryOrder.DUDeliveryOrderID;
                    DUDeliveryOrderDetailDA.Delete(tc, oDUDeliveryOrderDetail, EnumDBOperation.Delete, nUserID, sDUDeliveryOrderDetaillIDs);
                    sDUDeliveryOrderDetaillIDs = "";
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
                oDUDeliveryOrder = new DUDeliveryOrder();
                oDUDeliveryOrder.ErrorMessage = e.Message.Split('~')[0];
                #endregion
            }
            return oDUDeliveryOrder;
        }
        public DUDeliveryOrder Approve(DUDeliveryOrder oDUDeliveryOrder, Int64 nUserID)
        {
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader;
                reader = DUDeliveryOrderDA.InsertUpdate(tc, oDUDeliveryOrder, EnumDBOperation.Approval, nUserID);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oDUDeliveryOrder = CreateObject(oReader);
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
                //throw new ServiceException("Failed to Get DUDeliveryOrder", e);
                oDUDeliveryOrder.ErrorMessage = e.Message.Split('~')[0];
                #endregion
            }

            return oDUDeliveryOrder;
        }
        public DUDeliveryOrder DOCancel(DUDeliveryOrder oDUDeliveryOrder, Int64 nUserID)
        {
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader;
                reader = DUDeliveryOrderDA.DOCancel(tc, oDUDeliveryOrder, EnumDBOperation.Cancel, nUserID);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oDUDeliveryOrder = CreateObject(oReader);
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
                //throw new ServiceException("Failed to Get DUDeliveryOrder", e);
                oDUDeliveryOrder.ErrorMessage = e.Message.Split('~')[0];
                #endregion
            }

            return oDUDeliveryOrder;
        }
        public DUDeliveryOrder DUDeliveryOrderSendToProduction(DUDeliveryOrder oDUDeliveryOrder, Int64 nUserID)
        {
           
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader;
                reader = DUDeliveryOrderDA.DUDeliveryOrderSendToProduction(tc, oDUDeliveryOrder,  nUserID);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oDUDeliveryOrder = CreateObject(oReader);
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
                //throw new ServiceException("Failed to Get DUDeliveryOrder", e);
                oDUDeliveryOrder.ErrorMessage = e.Message;
                #endregion
            }

            return oDUDeliveryOrder;
        }
        public DUDeliveryOrder Get(int nDOID, Int64 nUserId)
        {
            DUDeliveryOrder oDUDeliveryOrder = new DUDeliveryOrder();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = DUDeliveryOrderDA.Get(nDOID, tc);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oDUDeliveryOrder = CreateObject(oReader);
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
                //throw new ServiceException("Failed to Get DUDeliveryOrder", e);
                oDUDeliveryOrder.ErrorMessage = e.Message;
                #endregion
            }

            return oDUDeliveryOrder;
        }
        public List<DUDeliveryOrder> GetsByPaymentType(string sPaymentTypes, Int64 nUserID)
        {
            List<DUDeliveryOrder> oDUDeliveryOrder = null;

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = DUDeliveryOrderDA.GetsByPaymentType(tc, sPaymentTypes);
                oDUDeliveryOrder = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get DUDeliveryOrder", e);
                #endregion
            }
            return oDUDeliveryOrder;
        }
        public List<DUDeliveryOrder> GetsBy(string sContractorID, Int64 nUserID)
        {
            List<DUDeliveryOrder> oDUDeliveryOrder = null;

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = DUDeliveryOrderDA.GetsBy(tc, sContractorID);
                oDUDeliveryOrder = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get DUDeliveryOrder", e);
                #endregion
            }
            return oDUDeliveryOrder;
        }
        public List<DUDeliveryOrder> GetsByPI(int nExportPIID, Int64 nUserID)
        {
            List<DUDeliveryOrder> oDUDeliveryOrder = null;

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = DUDeliveryOrderDA.GetsByPI(tc, nExportPIID);
                oDUDeliveryOrder = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get DUDeliveryOrder", e);
                #endregion
            }
            return oDUDeliveryOrder;
        }
        public List<DUDeliveryOrder> GetsByNo(string sOrderNo,Int64 nUserID)
        {
            List<DUDeliveryOrder> oDUDeliveryOrder = null;

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = DUDeliveryOrderDA.GetsByNo(tc, sOrderNo);
                oDUDeliveryOrder = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get DUDeliveryOrder", e);
                #endregion
            }
            return oDUDeliveryOrder;
        }
        public string Delete(DUDeliveryOrder oDUDeliveryOrder, Int64 nUserId)
        {
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);

                DUDeliveryOrderDA.Delete(tc, oDUDeliveryOrder, EnumDBOperation.Delete, nUserId);
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
        public List<DUDeliveryOrder> Gets(string sSQL, Int64 nUserID)
        {
            List<DUDeliveryOrder> oDUDeliveryOrder = null;

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = DUDeliveryOrderDA.Gets(sSQL, tc);
                oDUDeliveryOrder = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get DUDeliveryOrder", e);
                #endregion
            }
            return oDUDeliveryOrder;
        }

        public List<DUDeliveryOrder> DUDeliveryOrderAdjustmentForExportPI(string sDUDeliveryOrderIDs, int nExportPIID, int nDBOperation ,Int64 nUserId)
        {
            List<DUDeliveryOrder> oDUDeliveryOrders = new List<DUDeliveryOrder>();
             DUDeliveryOrder oDUDeliveryOrder = new DUDeliveryOrder();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();
                IDataReader reader = DUDeliveryOrderDA.DUDeliveryOrderAdjustmentForExportPI(tc, sDUDeliveryOrderIDs, nExportPIID, nDBOperation, nUserId);
                oDUDeliveryOrders = CreateObjects(reader);
                reader.Close();
                tc.End();

                if (nDBOperation == (int)EnumDBOperation.Delete)
                {
                    oDUDeliveryOrder.ErrorMessage = Global.DeleteMessage;
                    oDUDeliveryOrders.Add(oDUDeliveryOrder);
                }

            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null) tc.HandleError();
                oDUDeliveryOrder.ErrorMessage = (e.Message.Contains("!"))? e.Message.Split('!')[0]: e.Message;
                oDUDeliveryOrders.Add(oDUDeliveryOrder);
                #endregion
            }

            return oDUDeliveryOrders;
        }

        public DUDeliveryOrder UpdateDONo(DUDeliveryOrder oDUDeliveryOrder, Int64 nUserID)
        {
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
                IDataReader reader;
                reader = DUDeliveryOrderDA.UpdateDONo(tc, oDUDeliveryOrder, nUserID, EnumDBOperation.Insert);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oDUDeliveryOrder = new DUDeliveryOrder();
                    oDUDeliveryOrder = CreateObject(oReader);
                }
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                oDUDeliveryOrder.ErrorMessage = e.Message;
                oDUDeliveryOrder.ErrorMessage = e.Message.Split('~')[0];
                #endregion
            }
            return oDUDeliveryOrder;
        }

        #endregion
       
    }
}
