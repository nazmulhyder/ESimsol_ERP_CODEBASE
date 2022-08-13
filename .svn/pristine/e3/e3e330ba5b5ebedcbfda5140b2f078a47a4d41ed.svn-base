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
    public class DyeingOrderService : MarshalByRefObject, IDyeingOrderService
    {
        #region Private functions and declaration
        private DyeingOrder MapObject(NullHandler oReader)
        {
            DyeingOrder oDyeingOrder = new DyeingOrder();
            oDyeingOrder.DyeingOrderID = oReader.GetInt32("DyeingOrderID");
            oDyeingOrder.OrderNo = oReader.GetString("OrderNo");
            oDyeingOrder.OrderDate = oReader.GetDateTime("OrderDate");
            //oDyeingOrder.OrderNoFull = oReader.GetString("OrderNoFull");
            oDyeingOrder.ReviseDate = oReader.GetDateTime("ReviseDate");
            oDyeingOrder.ReviseNote = oReader.GetString("ReviseNote");
            oDyeingOrder.ContractorID = oReader.GetInt32("ContractorID");
            oDyeingOrder.MBuyerID = oReader.GetInt32("MBuyerID");
            oDyeingOrder.ContactPersonnelID = oReader.GetInt32("ContactPersonnelID");
            oDyeingOrder.DeliveryToID = oReader.GetInt32("DeliveryToID");
            oDyeingOrder.ContactPersonnelID_DelTo = oReader.GetInt32("DeliveryToContactPersonnelID");
            oDyeingOrder.ContactPersonnelID = oReader.GetInt32("ContactPersonnelID");
            oDyeingOrder.PaymentType = oReader.GetInt32("PaymentType");
            oDyeingOrder.DyeingOrderType = oReader.GetInt32("DyeingOrderType");
            oDyeingOrder.LightSourchID = oReader.GetInt32("LightSourchID");
            oDyeingOrder.LightSourchIDTwo = oReader.GetInt32("LightSourchIDTwo");
            oDyeingOrder.Priority = (EnumPriorityLevel)oReader.GetInt16("Priority");
            oDyeingOrder.PriorityInt = oReader.GetInt16("Priority");
            oDyeingOrder.ExportPIID = oReader.GetInt32("ExportPIID");
            oDyeingOrder.SampleInvoiceID = oReader.GetInt32("SampleInvoiceID");
            oDyeingOrder.MKTEmpID = oReader.GetInt32("MKTEmpID");
            oDyeingOrder.RefNo = oReader.GetString("RefNo");
            oDyeingOrder.StyleNo = oReader.GetString("StyleNo");

            oDyeingOrder.StripeOrder = oReader.GetString("StripeOrder");
            oDyeingOrder.KnittingStyle = oReader.GetString("KnittingStyle");
            oDyeingOrder.Gauge = oReader.GetString("Gauge");
        
            oDyeingOrder.ApproveBy = oReader.GetInt32("ApproveBy");
            oDyeingOrder.Note = oReader.GetString("Note");
            oDyeingOrder.ApproveDate = oReader.GetDateTime("ApproveDate");
            oDyeingOrder.IsClose = oReader.GetBoolean("IsClose");
            ////derive
            oDyeingOrder.ContractorName = oReader.GetString("ContractorName");
            oDyeingOrder.DeliveryToName = oReader.GetString("DeliveryToName");
            oDyeingOrder.MBuyer = oReader.GetString("MBuyer");
            oDyeingOrder.PreaperByName = oReader.GetString("PreaperByName");
            oDyeingOrder.MKTPName = oReader.GetString("MKTPName");
            oDyeingOrder.CPName = oReader.GetString("CPName");
            oDyeingOrder.ApproveByName = oReader.GetString("ApproveByName");
            oDyeingOrder.SampleInvocieNo = oReader.GetString("SampleInvocieNo");
            oDyeingOrder.ExportPINo = oReader.GetString("ExportPINo");
            oDyeingOrder.Amount = oReader.GetDouble("Amount");
            oDyeingOrder.Qty = oReader.GetDouble("Qty");
            oDyeingOrder.Status = oReader.GetInt32("Status");
            oDyeingOrder.ReviseNo = oReader.GetInt32("ReviseNo");
            oDyeingOrder.IsInHouse = oReader.GetBoolean("IsInHouse");
            oDyeingOrder.PONo = oReader.GetString("PONo"); // Without Revise No
            oDyeingOrder.ContactPersonnelName = oReader.GetString("ContactPersonnelName");
            oDyeingOrder.NoCode = oReader.GetString("NoCode");
            oDyeingOrder.LabdipNo = oReader.GetString("LabdipNo");
            oDyeingOrder.OrderType = oReader.GetString("OrderType");
            oDyeingOrder.DyeingStepTypeInt = oReader.GetInt32("DyeingStepType");
            //oDyeingOrder.DeliveryZoneID = oReader.GetInt32("DeliveryZoneID");
            //oDyeingOrder.DeliveryZone = oReader.GetString("DeliveryZoneName");
            oDyeingOrder.OrderTypeSt = oReader.GetString("OrderTypeSt");
            oDyeingOrder.FSCDetailID = oReader.GetInt32("FSCDetailID");
            oDyeingOrder.CurrencyID = oReader.GetInt32("CurrencyID");
            oDyeingOrder.DeliveryNote = oReader.GetString("DeliveryNote");
            
            return oDyeingOrder;

        }


        private DyeingOrder CreateObject(NullHandler oReader)
        {
            DyeingOrder oDyeingOrder = MapObject(oReader);
            return oDyeingOrder;
        }

        private List<DyeingOrder> CreateObjects(IDataReader oReader)
        {
            List<DyeingOrder> oDyeingOrders = new List<DyeingOrder>();
            NullHandler oHandler = new NullHandler(oReader);
            while (oReader.Read())
            {
                DyeingOrder oItem = CreateObject(oHandler);
                oDyeingOrders.Add(oItem);
            }
            return oDyeingOrders;
        }

        #endregion

        #region Interface implementation
        public DyeingOrderService() { }

        public DyeingOrder Save(DyeingOrder oDyeingOrder, Int64 nUserID)
        {

            TransactionContext tc = null;
            List<DyeingOrderDetail> oDyeingOrderDetails = new List<DyeingOrderDetail>();
            String sDyeingOrderDetaillIDs = "";
            try
            {
                oDyeingOrderDetails = oDyeingOrder.DyeingOrderDetails;

                tc = TransactionContext.Begin(true);
                IDataReader reader;

                bool bIsLabdipApply = DyeingOrderDA.GetIsLabdipApply(tc, oDyeingOrder.DyeingOrderType);

                if (oDyeingOrder.DyeingOrderID <= 0)
                {
                    reader = DyeingOrderDA.InsertUpdate(tc, oDyeingOrder, EnumDBOperation.Insert, nUserID);
                }
                else
                {
                    reader = DyeingOrderDA.InsertUpdate(tc, oDyeingOrder, EnumDBOperation.Update, nUserID);
                }
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oDyeingOrder = new DyeingOrder();
                    oDyeingOrder = CreateObject(oReader);
                    oDyeingOrder.Qty = 0;
                    oDyeingOrder.Amount = 0;
                }
                reader.Close();

                #region DyeingOrderDetails Part
                if (oDyeingOrderDetails != null)
                {
                    foreach (DyeingOrderDetail oItem in oDyeingOrderDetails)
                    {
                        IDataReader readertnc;

                        oItem.DyeingOrderID = oDyeingOrder.DyeingOrderID;
                        oDyeingOrder.Qty = oDyeingOrder.Qty + oItem.Qty;
                        oDyeingOrder.Amount = oDyeingOrder.Amount + oItem.Qty * oItem.UnitPrice;
                        if (oItem.DyeingOrderDetailID <= 0)
                        {
                            readertnc = DyeingOrderDetailDA.InsertUpdate(tc, oItem, EnumDBOperation.Insert, nUserID,"");
                        }
                        else
                        {
                            readertnc = DyeingOrderDetailDA.InsertUpdate(tc, oItem, EnumDBOperation.Update, nUserID,"");
                        }
                        NullHandler oReaderTNC = new NullHandler(readertnc);

                        if (readertnc.Read())
                        {
                            sDyeingOrderDetaillIDs = sDyeingOrderDetaillIDs + oReaderTNC.GetString("DyeingOrderDetailID") + ",";
                        }
                        readertnc.Close();
                    }
                    if (sDyeingOrderDetaillIDs.Length > 0)
                    {
                        sDyeingOrderDetaillIDs = sDyeingOrderDetaillIDs.Remove(sDyeingOrderDetaillIDs.Length - 1, 1);
                    }
                    DyeingOrderDetail oDyeingOrderDetail = new DyeingOrderDetail();
                    oDyeingOrderDetail.DyeingOrderID = oDyeingOrder.DyeingOrderID;
                    DyeingOrderDetailDA.Delete(tc, oDyeingOrderDetail, EnumDBOperation.Delete, nUserID, sDyeingOrderDetaillIDs);
                    sDyeingOrderDetaillIDs = "";

                }
                #endregion
                if (bIsLabdipApply==true)
                {
                    DyeingOrderDA.CreateLabdipByDO(tc, oDyeingOrder.DyeingOrderID, nUserID);
                }
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                //ExceptionLog.Write(e);
                //throw new ServiceException("Failed to . Because of " + e.Message, e);
                oDyeingOrder = new DyeingOrder();
                oDyeingOrder.ErrorMessage = e.Message.Split('~')[0];
                #endregion
            }
            return oDyeingOrder;
        }
        public DyeingOrder Save_Log(DyeingOrder oDyeingOrder, Int64 nUserID)
        {
            TransactionContext tc = null;
            List<DyeingOrderDetail> oDyeingOrderDetails = new List<DyeingOrderDetail>();

            double nAmount = 0;
            String sDyeingOrderDetaillIDs = "";
            try
            {
                oDyeingOrderDetails = oDyeingOrder.DyeingOrderDetails;
               

                foreach (DyeingOrderDetail oItem in oDyeingOrderDetails)
                {
                    nAmount = nAmount + oItem.Qty * oItem.UnitPrice;
                }

                tc = TransactionContext.Begin(true);
                IDataReader reader;
                bool bIsLabdipApply = DyeingOrderDA.GetIsLabdipApply(tc, oDyeingOrder.DyeingOrderType);
                oDyeingOrder.Amount = nAmount;
                reader = DyeingOrderDA.InsertUpdate_Log(tc, oDyeingOrder, EnumDBOperation.Revise, nUserID);
                
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oDyeingOrder = new DyeingOrder();
                    oDyeingOrder = CreateObject(oReader);
                }
                reader.Close();

                #region DyeingOrderDetails Part
                if (oDyeingOrderDetails != null)
                {
                    foreach (DyeingOrderDetail oItem in oDyeingOrderDetails)
                    {
                        IDataReader readertnc;
                        oItem.DyeingOrderID = oDyeingOrder.DyeingOrderID;
                        if (oItem.DyeingOrderDetailID <= 0)
                        {
                            readertnc = DyeingOrderDetailDA.InsertUpdate(tc, oItem, EnumDBOperation.Insert, nUserID,"");
                        }
                        else
                        {
                            readertnc = DyeingOrderDetailDA.InsertUpdate(tc, oItem, EnumDBOperation.Update, nUserID,"");
                        }
                        NullHandler oReaderTNC = new NullHandler(readertnc);

                        if (readertnc.Read())
                        {
                            sDyeingOrderDetaillIDs = sDyeingOrderDetaillIDs + oReaderTNC.GetString("DyeingOrderDetailID") + ",";
                        }
                        readertnc.Close();
                    }
                    if (sDyeingOrderDetaillIDs.Length > 0)
                    {
                        sDyeingOrderDetaillIDs = sDyeingOrderDetaillIDs.Remove(sDyeingOrderDetaillIDs.Length - 1, 1);
                    }
                    DyeingOrderDetail oDyeingOrderDetail = new DyeingOrderDetail();
                    oDyeingOrderDetail.DyeingOrderID = oDyeingOrder.DyeingOrderID;
                    DyeingOrderDetailDA.Delete(tc, oDyeingOrderDetail, EnumDBOperation.Delete, nUserID, sDyeingOrderDetaillIDs);
                    sDyeingOrderDetaillIDs = "";

                }
                #endregion
                if (bIsLabdipApply == true)
                {
                    DyeingOrderDA.CreateLabdipByDO(tc, oDyeingOrder.DyeingOrderID, nUserID);
                }

                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                //ExceptionLog.Write(e);
                //throw new ServiceException("Failed to . Because of " + e.Message, e);
                oDyeingOrder = new DyeingOrder();
                oDyeingOrder.ErrorMessage = e.Message.Split('~')[0];
                #endregion
            }
            return oDyeingOrder;
        }
        public DyeingOrder Approve(DyeingOrder oDyeingOrder, Int64 nUserID)
        {
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader;
                reader = DyeingOrderDA.InsertUpdate(tc, oDyeingOrder, EnumDBOperation.Approval, nUserID);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oDyeingOrder = CreateObject(oReader);
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
                //throw new ServiceException("Failed to Get DyeingOrder", e);
                oDyeingOrder.ErrorMessage = e.Message.Split('~')[0];
                #endregion
            }

            return oDyeingOrder;
        }
        public DyeingOrder DOSave_Auto(DyeingOrder oDyeingOrder, Int64 nUserID)
        {
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader;
                reader = DyeingOrderDA.InsertUpdate_DeliveryOrder(tc, oDyeingOrder, EnumDBOperation.Insert, nUserID);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oDyeingOrder = CreateObject(oReader);
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
                //throw new ServiceException("Failed to Get DyeingOrder", e);
                oDyeingOrder.ErrorMessage = e.Message.Split('~')[0];
                #endregion
            }

            return oDyeingOrder;
        }
        public DyeingOrder DOCancel(DyeingOrder oDyeingOrder, Int64 nUserID)
        {
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader;
                reader = DyeingOrderDA.DOCancel(tc, oDyeingOrder, EnumDBOperation.Cancel, nUserID);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oDyeingOrder = CreateObject(oReader);
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
                oDyeingOrder = new DyeingOrder();
                //throw new ServiceException("Failed to Get DyeingOrder", e);
                oDyeingOrder.ErrorMessage = e.Message.Split('~')[0];
                #endregion
            }

            return oDyeingOrder;
        }
        public DyeingOrder DyeingOrderSendToProduction(DyeingOrder oDyeingOrder, Int64 nUserID)
        {
           
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader;
                reader = DyeingOrderDA.DyeingOrderSendToProduction(tc, oDyeingOrder,  nUserID);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oDyeingOrder = CreateObject(oReader);
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
                //throw new ServiceException("Failed to Get DyeingOrder", e);
                oDyeingOrder.ErrorMessage = e.Message;
                #endregion
            }

            return oDyeingOrder;
        }
        public DyeingOrder DyeingOrderHistory(DyeingOrder oDyeingOrder, Int64 nUserID)
        {

            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader;
                reader = DyeingOrderDA.DyeingOrderHistory(tc, oDyeingOrder, nUserID);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oDyeingOrder = CreateObject(oReader);
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
                //throw new ServiceException("Failed to Get DyeingOrder", e);
                oDyeingOrder.ErrorMessage = e.Message;
                #endregion
            }

            return oDyeingOrder;
        }
        public DyeingOrder Get(int nDOID, Int64 nUserId)
        {
            DyeingOrder oDyeingOrder = new DyeingOrder();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = DyeingOrderDA.Get(nDOID, tc);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oDyeingOrder = CreateObject(oReader);
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
                //throw new ServiceException("Failed to Get DyeingOrder", e);
                oDyeingOrder.ErrorMessage = e.Message;
                #endregion
            }

            return oDyeingOrder;
        }
        public DyeingOrder GetFSCD(int nFSEDetailID, Int64 nUserId)
        {
            DyeingOrder oDyeingOrder = new DyeingOrder();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = DyeingOrderDA.GetFSCD(nFSEDetailID, tc);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oDyeingOrder = CreateObject(oReader);
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
                //throw new ServiceException("Failed to Get DyeingOrder", e);
                oDyeingOrder.ErrorMessage = e.Message;
                #endregion
            }

            return oDyeingOrder;
        }
        public List<DyeingOrder> GetsByPaymentType(string sPaymentTypes, Int64 nUserID)
        {
            List<DyeingOrder> oDyeingOrder = null;

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = DyeingOrderDA.GetsByPaymentType(tc, sPaymentTypes);
                oDyeingOrder = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get DyeingOrder", e);
                #endregion
            }
            return oDyeingOrder;
        }
        public List<DyeingOrder> GetsBy(string sContractorID, Int64 nUserID)
        {
            List<DyeingOrder> oDyeingOrder = null;

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = DyeingOrderDA.GetsBy(tc, sContractorID);
                oDyeingOrder = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get DyeingOrder", e);
                #endregion
            }
            return oDyeingOrder;
        }
        public List<DyeingOrder> GetsByPI(int nExportPIID, Int64 nUserID)
        {
            List<DyeingOrder> oDyeingOrder = null;

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = DyeingOrderDA.GetsByPI(tc, nExportPIID);
                oDyeingOrder = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get DyeingOrder", e);
                #endregion
            }
            return oDyeingOrder;
        }
        public List<DyeingOrder> GetsByInvoice(int nSampleInvoiceID, Int64 nUserID)
        {
            List<DyeingOrder> oDyeingOrder = null;

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = DyeingOrderDA.GetsByInvoice(tc, nSampleInvoiceID);
                oDyeingOrder = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get DyeingOrder", e);
                #endregion
            }
            return oDyeingOrder;
        }
        public List<DyeingOrder> GetsByNo(string sOrderNo,Int64 nUserID)
        {
            List<DyeingOrder> oDyeingOrder = null;

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = DyeingOrderDA.GetsByNo(tc, sOrderNo);
                oDyeingOrder = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get DyeingOrder", e);
                #endregion
            }
            return oDyeingOrder;
        }
        public string Delete(DyeingOrder oDyeingOrder, Int64 nUserId)
        {
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);

                DyeingOrderDA.Delete(tc, oDyeingOrder, EnumDBOperation.Delete, nUserId);
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
        public List<DyeingOrder> Gets(string sSQL, Int64 nUserID)
        {
            List<DyeingOrder> oDyeingOrder = null;

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = DyeingOrderDA.Gets(sSQL, tc);
                oDyeingOrder = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get DyeingOrder", e);
                #endregion
            }
            return oDyeingOrder;
        }
        public List<DyeingOrder> DyeingOrderAdjustmentForExportPI(string sDyeingOrderIDs, int nExportPIID, int nDBOperation ,Int64 nUserId)
        {
            List<DyeingOrder> oDyeingOrders = new List<DyeingOrder>();
             DyeingOrder oDyeingOrder = new DyeingOrder();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();
                IDataReader reader = DyeingOrderDA.DyeingOrderAdjustmentForExportPI(tc, sDyeingOrderIDs, nExportPIID, nDBOperation, nUserId);
                oDyeingOrders = CreateObjects(reader);
                reader.Close();
                tc.End();

                if (nDBOperation == (int)EnumDBOperation.Delete)
                {
                    oDyeingOrder.ErrorMessage = Global.DeleteMessage;
                    oDyeingOrders.Add(oDyeingOrder);
                }

            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null) tc.HandleError();
                oDyeingOrder.ErrorMessage = (e.Message.Contains("~"))? e.Message.Split('~')[0]: e.Message;
                oDyeingOrders.Add(oDyeingOrder);
                #endregion
            }

            return oDyeingOrders;
        }
        public DyeingOrder CreateServisePI(DyeingOrder oDyeingOrder, Int64 nUserID)
        {
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader;
                reader = DyeingOrderDA.CreateServisePI(tc, oDyeingOrder, EnumDBOperation.Cancel, nUserID);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oDyeingOrder = CreateObject(oReader);
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
                //throw new ServiceException("Failed to Get DyeingOrder", e);
                oDyeingOrder.ErrorMessage = e.Message.Split('~')[0];
                #endregion
            }

            return oDyeingOrder;
        }
        public DyeingOrder OrderClose(DyeingOrder oDyeingOrder, Int64 nUserId)
        {
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = DyeingOrderDA.OrderClose(tc, oDyeingOrder);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oDyeingOrder = CreateObject(oReader);
                }
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                oDyeingOrder = new DyeingOrder();
                oDyeingOrder.ErrorMessage = e.Message.Split('~')[0];
                #endregion
            }
            return oDyeingOrder;
        }
        public DyeingOrder UpdateMasterBuyer(DyeingOrder oDyeingOrder, Int64 nUserId)
        {
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = DyeingOrderDA.UpdateMasterBuyer(tc, oDyeingOrder);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oDyeingOrder = CreateObject(oReader);
                }
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                oDyeingOrder = new DyeingOrder();
                oDyeingOrder.ErrorMessage = e.Message.Split('~')[0];
                #endregion
            }
            return oDyeingOrder;
        }
        #endregion
       
    }
}
