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
	public class WorkOrderService : MarshalByRefObject, IWorkOrderService
	{
		#region Private functions and declaration

		private WorkOrder MapObject(NullHandler oReader)
		{
			WorkOrder oWorkOrder = new WorkOrder();
            oWorkOrder.WorkOrderID = oReader.GetInt32("WorkOrderID");
            oWorkOrder.BUID = oReader.GetInt32("BUID");
            oWorkOrder.FileNo = oReader.GetString("FileNo");
            oWorkOrder.WorkOrderNo = oReader.GetString("WorkOrderNo");
            oWorkOrder.PreparedByName = oReader.GetString("PreparedByName");
            oWorkOrder.PreparedBy = oReader.GetInt32("DBUserID");
            
            oWorkOrder.WorkOrderDate = oReader.GetDateTime("WorkOrderDate");
            oWorkOrder.ExpectedDeliveryDate = oReader.GetDateTime("ExpectedDeliveryDate");
            oWorkOrder.WorkOrderStatus = (EnumWorkOrderStatus)oReader.GetInt32("WorkOrderStatus");
            oWorkOrder.WorkOrderStatusInt = oReader.GetInt32("WorkOrderStatus");
            oWorkOrder.SupplierID = oReader.GetInt32("SupplierID");
            oWorkOrder.ContactPersonID = oReader.GetInt32("ContactPersonID");
            oWorkOrder.Note = oReader.GetString("Note");
            oWorkOrder.MerchandiserID = oReader.GetInt32("MerchandiserID");
            oWorkOrder.ApproveDate = oReader.GetDateTime("ApproveDate");
            oWorkOrder.ApproveBy = oReader.GetInt32("ApproveBy");
            oWorkOrder.CurrencyID = oReader.GetInt32("CurrencyID");
            oWorkOrder.CRate = oReader.GetDouble("CRate");
            oWorkOrder.RateUnit = oReader.GetInt32("RateUnit");
            oWorkOrder.ReviseNo = oReader.GetInt32("ReviseNo");
            oWorkOrder.FullFileNo = oReader.GetString("FullFileNo");
            oWorkOrder.SupplierName = oReader.GetString("SupplierName");
            oWorkOrder.SupplierAddress = oReader.GetString("SupplierAddress");
            oWorkOrder.ContactPersonName = oReader.GetString("ContactPersonName");
            oWorkOrder.MerchandiserName = oReader.GetString("MerchandiserName");
            oWorkOrder.ApproveByName = oReader.GetString("ApproveByName");
            oWorkOrder.CurrencyName = oReader.GetString("CurrencyName");
            oWorkOrder.CurrencySymbol = oReader.GetString("CurrencySymbol");
            oWorkOrder.Qty = oReader.GetDouble("Qty");
            oWorkOrder.Amount = oReader.GetDouble("Amount");
            oWorkOrder.YetToGRNQty = oReader.GetDouble("YetToGRNQty");
            oWorkOrder.WorkOrderLogID = oReader.GetInt32("WorkOrderLogID");
			return oWorkOrder;
		}

		private WorkOrder CreateObject(NullHandler oReader)
		{
			WorkOrder oWorkOrder = new WorkOrder();
			oWorkOrder = MapObject(oReader);
			return oWorkOrder;
		}

		private List<WorkOrder> CreateObjects(IDataReader oReader)
		{
			List<WorkOrder> oWorkOrder = new List<WorkOrder>();
			NullHandler oHandler = new NullHandler(oReader);
			while (oReader.Read())
			{
				WorkOrder oItem = CreateObject(oHandler);
				oWorkOrder.Add(oItem);
			}
			return oWorkOrder;
		}

		#endregion

		#region Interface implementation
			public WorkOrder Save(WorkOrder oWorkOrder, Int64 nUserID)
			{
				TransactionContext tc = null;
                string sWorkOrderDetailIDs = "";
                List<WorkOrderDetail> oWorkOrderDetails = new List<WorkOrderDetail>();
                WorkOrderDetail oWorkOrderDetail = new WorkOrderDetail();
                List<WOTermsAndCondition> oWOTermsAndConditions = new List<WOTermsAndCondition>();
                oWorkOrderDetails = oWorkOrder.WorkOrderDetails;
                oWOTermsAndConditions = oWorkOrder.WOTermsAndConditions;
                string sWOTandCIDs = "";

				try
				{
					tc = TransactionContext.Begin(true);
					IDataReader reader;
					if (oWorkOrder.WorkOrderID <= 0)
					{
                        AuthorizationRoleDA.CheckUserPermission(tc, nUserID, EnumModuleName.WorkOrder, EnumRoleOperationType.Add);
						reader = WorkOrderDA.InsertUpdate(tc, oWorkOrder, EnumDBOperation.Insert, nUserID);
					}
					else{
                        AuthorizationRoleDA.CheckUserPermission(tc, nUserID, EnumModuleName.WorkOrder, EnumRoleOperationType.Edit);
						reader = WorkOrderDA.InsertUpdate(tc, oWorkOrder, EnumDBOperation.Update, nUserID);
					}
					NullHandler oReader = new NullHandler(reader);
					if (reader.Read())
					{
						oWorkOrder = new WorkOrder();
						oWorkOrder = CreateObject(oReader);
					}
					reader.Close();
                    #region Work Order Detail Part
                    foreach (WorkOrderDetail oItem in oWorkOrderDetails)
                    {
                        IDataReader readerdetail;
                        oItem.WorkOrderID = oWorkOrder.WorkOrderID;
                        if (oItem.WorkOrderDetailID <= 0)
                        {
                            readerdetail = WorkOrderDetailDA.InsertUpdate(tc, oItem, EnumDBOperation.Insert, nUserID, "");
                        }
                        else
                        {
                            readerdetail = WorkOrderDetailDA.InsertUpdate(tc, oItem, EnumDBOperation.Update, nUserID, "");
                        }
                        NullHandler oReaderDetail = new NullHandler(readerdetail);
                        if (readerdetail.Read())
                        {
                            sWorkOrderDetailIDs = sWorkOrderDetailIDs + oReaderDetail.GetString("WorkOrderDetailID") + ",";
                        }
                        readerdetail.Close();
                    }
                    if (sWorkOrderDetailIDs.Length > 0)
                    {
                        sWorkOrderDetailIDs = sWorkOrderDetailIDs.Remove(sWorkOrderDetailIDs.Length - 1, 1);
                    }
                    oWorkOrderDetail = new WorkOrderDetail();
                    oWorkOrderDetail.WorkOrderID = oWorkOrder.WorkOrderID;
                    WorkOrderDetailDA.Delete(tc, oWorkOrderDetail, EnumDBOperation.Delete, nUserID, sWorkOrderDetailIDs);
                    #endregion

                    #region WO And C
                    if (oWOTermsAndConditions != null)
                    {
                        foreach (WOTermsAndCondition oItem in oWOTermsAndConditions)
                        {
                            IDataReader readerdetail;
                            oItem.WOID = oWorkOrder.WorkOrderID;
                            if (oItem.WOTermsAndConditionID <= 0)
                            {
                                readerdetail = WOTermsAndConditionDA.InsertUpdate(tc, oItem, EnumDBOperation.Insert, nUserID, "");
                            }
                            else
                            {
                                readerdetail = WOTermsAndConditionDA.InsertUpdate(tc, oItem, EnumDBOperation.Update, nUserID, "");
                            }
                            NullHandler oReaderDetail = new NullHandler(readerdetail);
                            if (readerdetail.Read())
                            {
                                sWOTandCIDs = sWOTandCIDs + oReaderDetail.GetString("WOTermsAndConditionID") + ",";
                            }
                            readerdetail.Close();
                        }
                        if (sWOTandCIDs.Length > 0)
                        {
                            sWOTandCIDs = sWOTandCIDs.Remove(sWOTandCIDs.Length - 1, 1);
                        }
                        WOTermsAndCondition oWOTermsAndCondition = new WOTermsAndCondition();
                        oWOTermsAndCondition.WOID = oWorkOrder.WorkOrderID;
                        WOTermsAndConditionDA.Delete(tc, oWOTermsAndCondition, EnumDBOperation.Delete, nUserID, sWOTandCIDs);
                    }

                    #endregion

                    #region Get Work Order for Actual Order Qty
                    reader = WorkOrderDA.Get(tc, oWorkOrder.WorkOrderID);
                    oReader = new NullHandler(reader);
                    if (reader.Read())
                    {
                        oWorkOrder = new WorkOrder();
                        oWorkOrder = CreateObject(oReader);
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
						oWorkOrder = new WorkOrder();
						oWorkOrder.ErrorMessage = e.Message.Split('!')[0];
					}
					#endregion
				}
				return oWorkOrder;
			}

            public WorkOrder AcceptRevise(WorkOrder oWorkOrder, Int64 nUserID)
            {
                TransactionContext tc = null;
                string sWorkOrderDetailIDs = "";
                List<WorkOrderDetail> oWorkOrderDetails = new List<WorkOrderDetail>();
                WorkOrderDetail oWorkOrderDetail = new WorkOrderDetail();
                List<WOTermsAndCondition> oWOTermsAndConditions = new List<WOTermsAndCondition>();
                oWorkOrderDetails = oWorkOrder.WorkOrderDetails;
                oWOTermsAndConditions = oWorkOrder.WOTermsAndConditions;
                string sWOTandCIDs = "";
                try
                {
                    tc = TransactionContext.Begin(true);
                    IDataReader reader;
                    reader = WorkOrderDA.AcceptRevise(tc, oWorkOrder, EnumDBOperation.Insert, nUserID);
                    NullHandler oReader = new NullHandler(reader);
                    if (reader.Read())
                    {
                        oWorkOrder = new WorkOrder();
                        oWorkOrder = CreateObject(oReader);
                    }
                    reader.Close();
                    #region Work Order Detail Part
                    foreach (WorkOrderDetail oItem in oWorkOrderDetails)
                    {
                        IDataReader readerdetail;
                        oItem.WorkOrderID = oWorkOrder.WorkOrderID;
                        if (oItem.WorkOrderDetailID <= 0)
                        {
                            readerdetail = WorkOrderDetailDA.InsertUpdate(tc, oItem, EnumDBOperation.Insert, nUserID, "");
                        }
                        else
                        {
                            readerdetail = WorkOrderDetailDA.InsertUpdate(tc, oItem, EnumDBOperation.Update, nUserID, "");
                        }
                        NullHandler oReaderDetail = new NullHandler(readerdetail);
                        if (readerdetail.Read())
                        {
                            sWorkOrderDetailIDs = sWorkOrderDetailIDs + oReaderDetail.GetString("WorkOrderDetailID") + ",";
                        }
                        readerdetail.Close();
                    }
                    if (sWorkOrderDetailIDs.Length > 0)
                    {
                        sWorkOrderDetailIDs = sWorkOrderDetailIDs.Remove(sWorkOrderDetailIDs.Length - 1, 1);
                    }
                    oWorkOrderDetail = new WorkOrderDetail();
                    oWorkOrderDetail.WorkOrderID = oWorkOrder.WorkOrderID;
                    WorkOrderDetailDA.Delete(tc, oWorkOrderDetail, EnumDBOperation.Delete, nUserID, sWorkOrderDetailIDs);
                    #endregion

                    #region WO And C
                    if (oWOTermsAndConditions != null)
                    {
                        foreach (WOTermsAndCondition oItem in oWOTermsAndConditions)
                        {
                            IDataReader readerdetail;
                            oItem.WOID = oWorkOrder.WorkOrderID;
                            if (oItem.WOTermsAndConditionID <= 0)
                            {
                                readerdetail = WOTermsAndConditionDA.InsertUpdate(tc, oItem, EnumDBOperation.Insert, nUserID, "");
                            }
                            else
                            {
                                readerdetail = WOTermsAndConditionDA.InsertUpdate(tc, oItem, EnumDBOperation.Update, nUserID, "");
                            }
                            NullHandler oReaderDetail = new NullHandler(readerdetail);
                            if (readerdetail.Read())
                            {
                                sWOTandCIDs = sWOTandCIDs + oReaderDetail.GetString("WOTermsAndConditionID") + ",";
                            }
                            readerdetail.Close();
                        }
                        if (sWOTandCIDs.Length > 0)
                        {
                            sWOTandCIDs = sWOTandCIDs.Remove(sWOTandCIDs.Length - 1, 1);
                        }
                        WOTermsAndCondition oWOTermsAndCondition = new WOTermsAndCondition();
                        oWOTermsAndCondition.WOID = oWorkOrder.WorkOrderID;
                        WOTermsAndConditionDA.Delete(tc, oWOTermsAndCondition, EnumDBOperation.Delete, nUserID, sWOTandCIDs);
                    }
                    #endregion

                    #region Get Work Order for Actual Order Qty
                    reader = WorkOrderDA.Get(tc, oWorkOrder.WorkOrderID);
                    oReader = new NullHandler(reader);
                    if (reader.Read())
                    {
                        oWorkOrder = new WorkOrder();
                        oWorkOrder = CreateObject(oReader);
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
                        oWorkOrder = new WorkOrder();
                        oWorkOrder.ErrorMessage = e.Message.Split('!')[0];
                    }
                    #endregion
                }
                return oWorkOrder;
            }
            public WorkOrder ChangeStatus(WorkOrder oWorkOrder, Int64 nUserID)
            {
                ApprovalRequest oApprovalRequest = new ApprovalRequest();
                ReviseRequest oReviseRequest = new ReviseRequest();
                oApprovalRequest = oWorkOrder.ApprovalRequest;
                oReviseRequest = oWorkOrder.ReviseRequest;
                TransactionContext tc = null;
                try
                {
                    tc = TransactionContext.Begin(true);
                    IDataReader reader;
                    if (oWorkOrder.WorkOrderActionType == EnumWorkOrderActionType.Approve)
                    {
                        AuthorizationRoleDA.CheckUserPermission(tc, nUserID, EnumModuleName.WorkOrder, EnumRoleOperationType.Approved);
                    }
                    if (oWorkOrder.WorkOrderActionType == EnumWorkOrderActionType.Cancel)
                    {
                        AuthorizationRoleDA.CheckUserPermission(tc, nUserID, EnumModuleName.WorkOrder, EnumRoleOperationType.Cancel);
                    }
                    reader = WorkOrderDA.ChangeStatus(tc, oWorkOrder, EnumDBOperation.Insert, nUserID);
                    NullHandler oReader = new NullHandler(reader);
                    if (reader.Read())
                    {
                        oWorkOrder = new WorkOrder();
                        oWorkOrder = CreateObject(oReader);
                    }
                    reader.Close();
                    if (oApprovalRequest!=null)
                    {
                        if (oWorkOrder.WorkOrderStatus == EnumWorkOrderStatus.Request_For_Approval && (oApprovalRequest.RequestBy != 0 && oApprovalRequest.RequestTo != 0))
                        {
                            IDataReader ApprovalRequestreader;
                            ApprovalRequestreader = ApprovalRequestDA.InsertUpdate(tc, oApprovalRequest, EnumDBOperation.Insert, nUserID);
                            if (ApprovalRequestreader.Read())
                            {

                            }
                            ApprovalRequestreader.Close();
                        }
                    }

                    else if (oReviseRequest!=null)
                    {
                        if (oWorkOrder.WorkOrderStatus == EnumWorkOrderStatus.Request_For_Revise && (oReviseRequest.RequestBy != 0 && oReviseRequest.RequestTo != 0))
                        {
                            IDataReader ReviseRequestreader;
                            ReviseRequestreader = ReviseRequestDA.InsertUpdate(tc, oReviseRequest, EnumDBOperation.Insert, nUserID);
                            if (ReviseRequestreader.Read())
                            {

                            }
                            ReviseRequestreader.Close();
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
                    oWorkOrder.ErrorMessage = Message;
                    //ExceptionLog.Write(e);
                    //throw new ServiceException("Failed to Save WorkOrderDetail. Because of " + e.Message, e);
                    #endregion
                }
                return oWorkOrder;
            }
			public string Delete(int id, Int64 nUserId)
			{
				TransactionContext tc = null;
				try
				{
					tc = TransactionContext.Begin(true);
					WorkOrder oWorkOrder = new WorkOrder();
					oWorkOrder.WorkOrderID = id;
					AuthorizationRoleDA.CheckUserPermission(tc, nUserId, EnumModuleName.WorkOrder, EnumRoleOperationType.Delete);
					DBTableReferenceDA.HasReference(tc, "WorkOrder", id);
					WorkOrderDA.Delete(tc, oWorkOrder, EnumDBOperation.Delete, nUserId);
					tc.End();
				}
				catch (Exception e)
				{
					#region Handle Exceptionif (tc != null)
					tc.HandleError();
					return e.Message.Split('!')[0];
					#endregion
				}
				return "Data delete successfully";
			}

            //
            public WorkOrder BillDone(WorkOrder oWorkOrder, Int64 nUserId)
            {
                TransactionContext tc = null;
                try
                {
                    tc = TransactionContext.Begin();
                    WorkOrderDA.BillDone(tc, oWorkOrder);
                    IDataReader reader = WorkOrderDA.Get(tc, oWorkOrder.WorkOrderID);
                    NullHandler oReader = new NullHandler(reader);
                    if (reader.Read())
                    {
                        oWorkOrder = CreateObject(oReader);
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
                    throw new ServiceException("Failed to Get WorkOrder", e);
                    #endregion
                }
                return oWorkOrder;
            }
        public WorkOrder Get(int id, Int64 nUserId)
			{
				WorkOrder oWorkOrder = new WorkOrder();
				TransactionContext tc = null;
				try
				{
					tc = TransactionContext.Begin();
					IDataReader reader = WorkOrderDA.Get(tc, id);
					NullHandler oReader = new NullHandler(reader);
					if (reader.Read())
					{
					oWorkOrder = CreateObject(oReader);
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
					throw new ServiceException("Failed to Get WorkOrder", e);
					#endregion
				}
				return oWorkOrder;
			}

            public WorkOrder GetByLog(int Logid, Int64 nUserId)
			{
				WorkOrder oWorkOrder = new WorkOrder();
				TransactionContext tc = null;
				try
				{
					tc = TransactionContext.Begin();
                    IDataReader reader = WorkOrderDA.GetByLog(tc, Logid);
					NullHandler oReader = new NullHandler(reader);
					if (reader.Read())
					{
					oWorkOrder = CreateObject(oReader);
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
					throw new ServiceException("Failed to Get WorkOrder", e);
					#endregion
				}
				return oWorkOrder;
			}
        
			public List<WorkOrder> Gets(Int64 nUserID)
			{
				List<WorkOrder> oWorkOrders = new List<WorkOrder>();
				TransactionContext tc = null;
				try
				{
					tc = TransactionContext.Begin();
					IDataReader reader = null;
					reader = WorkOrderDA.Gets(tc);
					oWorkOrders = CreateObjects(reader);
					reader.Close();
					tc.End();
				}
				catch (Exception e)
				{
					#region Handle Exception
					if (tc != null)
					tc.HandleError();
					WorkOrder oWorkOrder = new WorkOrder();
					oWorkOrder.ErrorMessage =  e.Message.Split('!')[0];
					#endregion
				}
				return oWorkOrders;
			}
            
			public List<WorkOrder> Gets (string sSQL, Int64 nUserID)
			{
				List<WorkOrder> oWorkOrders = new List<WorkOrder>();
				TransactionContext tc = null;
				try
				{
					tc = TransactionContext.Begin();
					IDataReader reader = null;
					reader = WorkOrderDA.Gets(tc, sSQL);
					oWorkOrders = CreateObjects(reader);
					reader.Close();
					tc.End();
				}
				catch (Exception e)
				{
					#region Handle Exception
					if (tc != null);
					tc.HandleError();
					ExceptionLog.Write(e);
					throw new ServiceException("Failed to Get WorkOrder", e);
					#endregion
				}
				return oWorkOrders;
			}

		#endregion
	}

}
