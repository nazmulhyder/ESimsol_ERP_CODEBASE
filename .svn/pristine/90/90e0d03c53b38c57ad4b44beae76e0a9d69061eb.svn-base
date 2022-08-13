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
    public class DeliveryOrderService : MarshalByRefObject, IDeliveryOrderService
    {
        #region Private functions and declaration
        private DeliveryOrder MapObject(NullHandler oReader)
        {
            DeliveryOrder oDeliveryOrder = new DeliveryOrder();
            oDeliveryOrder.DeliveryOrderID = oReader.GetInt32("DeliveryOrderID");
            oDeliveryOrder.DeliveryOrderLogID = oReader.GetInt32("DeliveryOrderLogID");
            oDeliveryOrder.BUID = oReader.GetInt32("BUID");
            oDeliveryOrder.DONo = oReader.GetString("DONo");
            oDeliveryOrder.DODate = oReader.GetDateTime("DODate");
            oDeliveryOrder.DOStatus = (EnumDOStatus)oReader.GetInt32("DOStatus");
            oDeliveryOrder.DOStatusInInt = oReader.GetInt32("DOStatus");
            oDeliveryOrder.RefType = (EnumRefType)oReader.GetInt32("RefType");
            oDeliveryOrder.RefID = oReader.GetInt32("RefID");
            oDeliveryOrder.ContractorID = oReader.GetInt32("ContractorID");
            oDeliveryOrder.DeliveryDate = oReader.GetDateTime("DeliveryDate");
            oDeliveryOrder.DeliveryPoint = oReader.GetString("DeliveryPoint");
            oDeliveryOrder.Note = oReader.GetString("Note");
            oDeliveryOrder.ApproveBy = oReader.GetInt32("ApproveBy");
            oDeliveryOrder.ReviseNo = oReader.GetInt32("ReviseNo");
            oDeliveryOrder.RefNo = oReader.GetString("RefNo");
            oDeliveryOrder.ContractorName = oReader.GetString("ContractorName");
            oDeliveryOrder.ApprovedByName = oReader.GetString("ApprovedByName");
            oDeliveryOrder.YetToDeliveryChallanQty = oReader.GetDouble("YetToDeliveryChallanQty");
            oDeliveryOrder.PrepareBy = oReader.GetInt32("PrepareBy");
            oDeliveryOrder.MDApproveBy = oReader.GetInt32("MDApproveBy");
            oDeliveryOrder.MDApproveByName = oReader.GetString("MDApproveByName");
            oDeliveryOrder.ReceiveBy = oReader.GetInt32("ReceiveBy");
            oDeliveryOrder.ReceiveByName = oReader.GetString("ReceiveByName");
            oDeliveryOrder.DeliveryToName = oReader.GetString("DeliveryToName");
            oDeliveryOrder.DeliveryToAddress = oReader.GetString("DeliveryToAddress");
            oDeliveryOrder.PrepareByName = oReader.GetString("PrepareByName");
            oDeliveryOrder.BuyerID = oReader.GetInt32("BuyerID");
            oDeliveryOrder.BuyerName = oReader.GetString("BuyerName");
            oDeliveryOrder.ExportLCNo = oReader.GetString("ExportLCNo");
            oDeliveryOrder.LCTermsName = oReader.GetString("LCTermsName");
            oDeliveryOrder.ApprovedDate = oReader.GetDateTime("ApprovedDate");
            oDeliveryOrder.ProductNature =  (EnumProductNature) oReader.GetInt32("ProductNature");
            oDeliveryOrder.ProductNatureInInt = oReader.GetInt32("ProductNature");
            oDeliveryOrder.ContractorContactPerson = oReader.GetInt32("ContractorContactPerson");
            return oDeliveryOrder;
        }

        private DeliveryOrder CreateObject(NullHandler oReader)
        {
            DeliveryOrder oDeliveryOrder = new DeliveryOrder();
            oDeliveryOrder = MapObject(oReader);
            return oDeliveryOrder;
        }

        private List<DeliveryOrder> CreateObjects(IDataReader oReader)
        {
            List<DeliveryOrder> oDeliveryOrder = new List<DeliveryOrder>();
            NullHandler oHandler = new NullHandler(oReader);
            while (oReader.Read())
            {
                DeliveryOrder oItem = CreateObject(oHandler);
                oDeliveryOrder.Add(oItem);
            }
            return oDeliveryOrder;
        }

        #endregion

        #region Interface implementation
        public DeliveryOrder IUD(DeliveryOrder oDeliveryOrder, short nDBOperation, Int64 nUserID)
        {
            TransactionContext tc = null;
            string sDeliveryOrderDetailIDs = "";
            List<DeliveryOrderDetail> oDeliveryOrderDetails = new List<DeliveryOrderDetail>();
            DeliveryOrderDetail oDeliveryOrderDetail = new DeliveryOrderDetail();
            oDeliveryOrderDetails = oDeliveryOrder.DeliveryOrderDetails;

            try
            {
                tc = TransactionContext.Begin(true);

                if (nDBOperation == (short)EnumDBOperation.Insert || nDBOperation == (short)EnumDBOperation.Update)
                {
                    AuthorizationRoleDA.CheckUserPermission(tc, nUserID, EnumModuleName.DeliveryOrder, ((nDBOperation == (short)EnumDBOperation.Insert) ? EnumRoleOperationType.Add : EnumRoleOperationType.Edit));
                    IDataReader reader;
                    reader = DeliveryOrderDA.InsertUpdate(tc, oDeliveryOrder, nDBOperation, nUserID);
                    NullHandler oReader = new NullHandler(reader);
                    if (reader.Read())
                    {
                        oDeliveryOrder = new DeliveryOrder();
                        oDeliveryOrder = CreateObject(oReader);
                    }
                    reader.Close();

                    #region Delivery Order Detail Part
                    foreach (DeliveryOrderDetail oItem in oDeliveryOrderDetails)
                    {
                        IDataReader readerdetail;
                        oItem.DeliveryOrderID = oDeliveryOrder.DeliveryOrderID;
                        if (oItem.DeliveryOrderDetailID <= 0)
                        {
                            readerdetail = DeliveryOrderDetailDA.InsertUpdate(tc, oItem, EnumDBOperation.Insert, nUserID, "");
                        }
                        else
                        {
                            readerdetail = DeliveryOrderDetailDA.InsertUpdate(tc, oItem, EnumDBOperation.Update, nUserID, "");
                        }
                        NullHandler oReaderDetail = new NullHandler(readerdetail);
                        if (readerdetail.Read())
                        {
                            sDeliveryOrderDetailIDs = sDeliveryOrderDetailIDs + oReaderDetail.GetString("DeliveryOrderDetailID") + ",";
                        }
                        readerdetail.Close();
                    }
                    if (sDeliveryOrderDetailIDs.Length > 0)
                    {
                        sDeliveryOrderDetailIDs = sDeliveryOrderDetailIDs.Remove(sDeliveryOrderDetailIDs.Length - 1, 1);
                    }
                    oDeliveryOrderDetail = new DeliveryOrderDetail();
                    oDeliveryOrderDetail.DeliveryOrderID = oDeliveryOrder.DeliveryOrderID;
                    DeliveryOrderDetailDA.Delete(tc, oDeliveryOrderDetail, EnumDBOperation.Delete, nUserID, sDeliveryOrderDetailIDs);
                    #endregion

                    #region Get Production Order
                    reader = DeliveryOrderDA.Get(tc, oDeliveryOrder.DeliveryOrderID);
                    oReader = new NullHandler(reader);
                    if (reader.Read())
                    {
                        oDeliveryOrder = new DeliveryOrder();
                        oDeliveryOrder = CreateObject(oReader);
                    }
                    reader.Close();
                    #endregion
                }
                else
                {
                    AuthorizationRoleDA.CheckUserPermission(tc, nUserID, EnumModuleName.DeliveryOrder, EnumRoleOperationType.Delete);
                    DeliveryOrderDA.Delete(tc, oDeliveryOrder, nDBOperation, nUserID);
                    oDeliveryOrder = new DeliveryOrder();
                    oDeliveryOrder.ErrorMessage = Global.DeleteMessage;
                }
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                {
                    tc.HandleError();
                    oDeliveryOrder = new DeliveryOrder();
                    oDeliveryOrder.ErrorMessage = e.Message.Split('!')[0];
                }
                #endregion
            }
            return oDeliveryOrder;
        }

        public DeliveryOrder AcceptRevise(DeliveryOrder oDeliveryOrder, short nDBOperation, Int64 nUserID)
        {
            TransactionContext tc = null;
            string sDeliveryOrderDetailIDs = "";
            List<DeliveryOrderDetail> oDeliveryOrderDetails = new List<DeliveryOrderDetail>();
            DeliveryOrderDetail oDeliveryOrderDetail = new DeliveryOrderDetail();
            oDeliveryOrderDetails = oDeliveryOrder.DeliveryOrderDetails;

            try
            {
                tc = TransactionContext.Begin(true);
                    IDataReader reader;
                    reader = DeliveryOrderDA.AcceptRevise(tc, oDeliveryOrder, nDBOperation, nUserID);
                    NullHandler oReader = new NullHandler(reader);
                    if (reader.Read())
                    {
                        oDeliveryOrder = new DeliveryOrder();
                        oDeliveryOrder = CreateObject(oReader);
                    }
                    reader.Close();

                    #region Delivery Order Detail Part
                    foreach (DeliveryOrderDetail oItem in oDeliveryOrderDetails)
                    {
                        IDataReader readerdetail;
                        oItem.DeliveryOrderID = oDeliveryOrder.DeliveryOrderID;
                        if (oItem.DeliveryOrderDetailID <= 0)
                        {
                            readerdetail = DeliveryOrderDetailDA.InsertUpdate(tc, oItem, EnumDBOperation.Insert, nUserID, "");
                        }
                        else
                        {
                            readerdetail = DeliveryOrderDetailDA.InsertUpdate(tc, oItem, EnumDBOperation.Update, nUserID, "");
                        }
                        NullHandler oReaderDetail = new NullHandler(readerdetail);
                        if (readerdetail.Read())
                        {
                            sDeliveryOrderDetailIDs = sDeliveryOrderDetailIDs + oReaderDetail.GetString("DeliveryOrderDetailID") + ",";
                        }
                        readerdetail.Close();
                    }
                    if (sDeliveryOrderDetailIDs.Length > 0)
                    {
                        sDeliveryOrderDetailIDs = sDeliveryOrderDetailIDs.Remove(sDeliveryOrderDetailIDs.Length - 1, 1);
                    }
                    oDeliveryOrderDetail = new DeliveryOrderDetail();
                    oDeliveryOrderDetail.DeliveryOrderID = oDeliveryOrder.DeliveryOrderID;
                    DeliveryOrderDetailDA.Delete(tc, oDeliveryOrderDetail, EnumDBOperation.Delete, nUserID, sDeliveryOrderDetailIDs);
                    #endregion

                    #region Get Production Order
                    reader = DeliveryOrderDA.Get(tc, oDeliveryOrder.DeliveryOrderID);
                    oReader = new NullHandler(reader);
                    if (reader.Read())
                    {
                        oDeliveryOrder = new DeliveryOrder();
                        oDeliveryOrder = CreateObject(oReader);
                    }
                    reader.Close();
                    #endregion
             
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                {
                    tc.HandleError();
                    oDeliveryOrder = new DeliveryOrder();
                    oDeliveryOrder.ErrorMessage = e.Message.Split('!')[0];
                }
                #endregion
            }
            return oDeliveryOrder;
        }

        public DeliveryOrder Get(int id, Int64 nUserId)
        {
            DeliveryOrder oDeliveryOrder = new DeliveryOrder();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();
                IDataReader reader = DeliveryOrderDA.Get(tc, id);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oDeliveryOrder = CreateObject(oReader);
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
                throw new ServiceException("Failed to Get DeliveryOrder", e);
                #endregion
            }
            return oDeliveryOrder;
        }

        public List<DeliveryOrder> Gets(string sSQL, Int64 nUserID)
        {
            List<DeliveryOrder> oDeliveryOrders = new List<DeliveryOrder>();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();
                IDataReader reader = null;
                reader = DeliveryOrderDA.Gets(tc, sSQL);
                oDeliveryOrders = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                tc.HandleError();
                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get Delivery Order", e);
                #endregion
            }
            return oDeliveryOrders;
        }

        public DeliveryOrder Approve(DeliveryOrder oDeliveryOrder, Int64 nUserID)
        {
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
         
                #region Valid Delivery Order
                if (oDeliveryOrder.DeliveryOrderID <= 0)
                {
                    throw new Exception("Invalid delivery order!");
                }
                if (oDeliveryOrder.ApproveBy != 0)
                {
                    throw new Exception("Your selected delivery order already approved!");
                }
                #endregion

                #region Delivery Order Approve
               
                AuthorizationRoleDA.CheckUserPermission(tc, nUserID, EnumModuleName.DeliveryOrder, EnumRoleOperationType.Approved);
                DeliveryOrderDA.Approve(tc, oDeliveryOrder.DeliveryOrderID, nUserID);
                #endregion

                #region Get Delivery Order
                IDataReader reader;
                reader = DeliveryOrderDA.Get(tc, oDeliveryOrder.DeliveryOrderID);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oDeliveryOrder = new DeliveryOrder();
                    oDeliveryOrder = CreateObject(oReader);
                }
                reader.Close();
                #endregion

                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                {
                    tc.HandleError();
                    oDeliveryOrder = new DeliveryOrder();
                    oDeliveryOrder.ErrorMessage = e.Message.Split('!')[0];
                }
                #endregion
            }
            return oDeliveryOrder;
        }



        public DeliveryOrder ChangeStatus(DeliveryOrder oDeliveryOrder, Int64 nUserID)
        {
            ApprovalRequest oApprovalRequest = new ApprovalRequest();
            ReviseRequest oReviseRequest = new ReviseRequest();
            oApprovalRequest = oDeliveryOrder.ApprovalRequest;
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
                IDataReader reader;
                if (oDeliveryOrder.DOActionType == EnumDOActionType.Approved)
                {
                    AuthorizationRoleDA.CheckUserPermission(tc, nUserID, EnumModuleName.DeliveryOrder, EnumRoleOperationType.Approved);
                }
                if (oDeliveryOrder.DOActionType == EnumDOActionType.Cancel)
                {
                    AuthorizationRoleDA.CheckUserPermission(tc, nUserID, EnumModuleName.DeliveryOrder, EnumRoleOperationType.Cancel);
                }
                reader = DeliveryOrderDA.ChangeStatus(tc, oDeliveryOrder, EnumDBOperation.Insert, nUserID);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oDeliveryOrder = new DeliveryOrder();
                    oDeliveryOrder = CreateObject(oReader);
                }
                reader.Close();
                if (oApprovalRequest != null)
                {
                    if (oDeliveryOrder.DOStatus == EnumDOStatus.RequestForApproved && (oApprovalRequest.RequestBy != 0 && oApprovalRequest.RequestTo != 0))
                    {
                        IDataReader ApprovalRequestreader;
                        ApprovalRequestreader = ApprovalRequestDA.InsertUpdate(tc, oApprovalRequest, EnumDBOperation.Insert, nUserID);
                        if (ApprovalRequestreader.Read())
                        {

                        }
                        ApprovalRequestreader.Close();
                    }
                }

               
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                string Message = "";
                Message = e.Message;
                Message = Message.Split('!')[0];
                oDeliveryOrder.ErrorMessage = Message;
                //ExceptionLog.Write(e);
                //throw new ServiceException("Failed to Save DeliveryOrderDetail. Because of " + e.Message, e);
                #endregion
            }
            return oDeliveryOrder;
        }
        
        #endregion
    }

}
