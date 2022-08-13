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
	public class ProductionOrderService : MarshalByRefObject, IProductionOrderService
	{
		#region Private functions and declaration
		private ProductionOrder MapObject(NullHandler oReader)
		{
			ProductionOrder oProductionOrder = new ProductionOrder();
			oProductionOrder.ProductionOrderID = oReader.GetInt32("ProductionOrderID");
            oProductionOrder.ProductionOrderLogID = oReader.GetInt32("ProductionOrderLogID");
			oProductionOrder.PONo = oReader.GetString("PONo");
            oProductionOrder.ReviseNo = oReader.GetInt32("ReviseNo");
            oProductionOrder.FullPONo = oReader.GetString("FullPONo");
			oProductionOrder.OrderDate = oReader.GetDateTime("OrderDate");
			oProductionOrder.BUID = oReader.GetInt32("BUID");
            oProductionOrder.ProductionOrderStatus = (EnumProductionOrderStatus)oReader.GetInt32("ProductionOrderStatus");
            oProductionOrder.ProductionOrderStatusInInt = oReader.GetInt32("ProductionOrderStatus");
            oProductionOrder.ContractorID = oReader.GetInt32("ContractorID");
			oProductionOrder.ContactPersonnelID = oReader.GetInt32("ContactPersonnelID");
			oProductionOrder.Note = oReader.GetString("Note");
            oProductionOrder.ExportSCID = oReader.GetInt32("ExportSCID");
			oProductionOrder.ApproveDate = oReader.GetDateTime("ApproveDate");
			oProductionOrder.ApproveBy = oReader.GetInt32("ApproveBy");
			oProductionOrder.DeliveryTo = oReader.GetInt32("DeliveryTo");
			oProductionOrder.DeliveryContactPerson = oReader.GetInt32("DeliveryContactPerson");
			oProductionOrder.ContractorName = oReader.GetString("ContractorName");
			oProductionOrder.ContractorAddress = oReader.GetString("ContractorAddress");
			oProductionOrder.ContactPersonnelName = oReader.GetString("ContactPersonnelName");
			oProductionOrder.DeliveryContactPersonName = oReader.GetString("DeliveryContactPersonName");
            oProductionOrder.ExportPINo = oReader.GetString("ExportPINo");
			oProductionOrder.ApproveByName = oReader.GetString("ApproveByName");
			oProductionOrder.DeliveryToName = oReader.GetString("DeliveryToName");
			oProductionOrder.Qty = oReader.GetDouble("Qty");
            oProductionOrder.BuyerID = oReader.GetInt32("BuyerID");
            oProductionOrder.BuyerName = oReader.GetString("BuyerName");
            oProductionOrder.ExpectedDeliveryDate = oReader.GetDateTime("ExpectedDeliveryDate");
            oProductionOrder.DBServerDateTime = oReader.GetDateTime("DBServerDateTime");
            oProductionOrder.MKTEmpID = oReader.GetInt32("MKTEmpID");
            oProductionOrder.ProductNature = (EnumProductNature)oReader.GetInt32("ProductNature");
            oProductionOrder.MKTPName = oReader.GetString("MKTPName");
            oProductionOrder.MKTPNickName = oReader.GetString("MKTPNickName");
            oProductionOrder.PrepareByName = oReader.GetString("PrepareByName");
            oProductionOrder.DirApprovedByName = oReader.GetString("DirApprovedByName");
            oProductionOrder.LCNo = oReader.GetString("LCNo");
            oProductionOrder.YetToProductionScheduleQty = oReader.GetDouble("YetToProductionScheduleQty");
            oProductionOrder.PrepareBy = oReader.GetInt32("DBUserID");
            oProductionOrder.DirApprovedBy = oReader.GetInt32("DirApprovedBy");
            oProductionOrder.LastReviseDate = oReader.GetDateTime("LastReviseDate");
			return oProductionOrder;
		}

		private ProductionOrder CreateObject(NullHandler oReader)
		{
			ProductionOrder oProductionOrder = new ProductionOrder();
			oProductionOrder = MapObject(oReader);
			return oProductionOrder;
		}

		private List<ProductionOrder> CreateObjects(IDataReader oReader)
		{
			List<ProductionOrder> oProductionOrder = new List<ProductionOrder>();
			NullHandler oHandler = new NullHandler(oReader);
			while (oReader.Read())
			{
				ProductionOrder oItem = CreateObject(oHandler);
				oProductionOrder.Add(oItem);
			}
			return oProductionOrder;
		}

		#endregion

		#region Interface implementation
			public ProductionOrder Save(ProductionOrder oProductionOrder, Int64 nUserID)
			{
				TransactionContext tc = null;
                string sProductionOrderDetailIDs = "";
                List<ProductionOrderDetail> oProductionOrderDetails = new List<ProductionOrderDetail>();
                List<POSizerBreakDown> oPOSizerBreakDowns = new List<POSizerBreakDown>();
                ProductionOrderDetail oProductionOrderDetail = new ProductionOrderDetail();
                oProductionOrderDetails = oProductionOrder.ProductionOrderDetails;
                oPOSizerBreakDowns = oProductionOrder.POSizerBreakDowns;
                string sBreakDownIDs = "";
				try
				{
					tc = TransactionContext.Begin(true);
					IDataReader reader;
					if (oProductionOrder.ProductionOrderID <= 0)
					{
                        AuthorizationRoleDA.CheckUserPermission(tc, nUserID, EnumModuleName.ProductionOrder, EnumRoleOperationType.Add);
						reader = ProductionOrderDA.InsertUpdate(tc, oProductionOrder, EnumDBOperation.Insert, nUserID);
					}
					else{
                        AuthorizationRoleDA.CheckUserPermission(tc, nUserID, EnumModuleName.ProductionOrder, EnumRoleOperationType.Edit);
						reader = ProductionOrderDA.InsertUpdate(tc, oProductionOrder, EnumDBOperation.Update, nUserID);
					}
					NullHandler oReader = new NullHandler(reader);
					if (reader.Read())
					{
						oProductionOrder = new ProductionOrder();
						oProductionOrder = CreateObject(oReader);
					}
					reader.Close();
                    #region Production Order Detail Part
                    foreach (ProductionOrderDetail oItem in oProductionOrderDetails)
                    {
                        IDataReader readerdetail;
                        oItem.ProductionOrderID = oProductionOrder.ProductionOrderID;
                        if (oItem.ProductionOrderDetailID <= 0)
                        {
                            readerdetail = ProductionOrderDetailDA.InsertUpdate(tc, oItem, EnumDBOperation.Insert, nUserID, "");
                        }
                        else
                        {
                            readerdetail = ProductionOrderDetailDA.InsertUpdate(tc, oItem, EnumDBOperation.Update, nUserID, "");
                        }
                        NullHandler oReaderDetail = new NullHandler(readerdetail);
                        if (readerdetail.Read())
                        {
                            sProductionOrderDetailIDs = sProductionOrderDetailIDs + oReaderDetail.GetString("ProductionOrderDetailID") + ",";
                        }
                        readerdetail.Close();
                    }
                    if (sProductionOrderDetailIDs.Length > 0)
                    {
                        sProductionOrderDetailIDs = sProductionOrderDetailIDs.Remove(sProductionOrderDetailIDs.Length - 1, 1);
                    }
                    oProductionOrderDetail = new ProductionOrderDetail();
                    oProductionOrderDetail.ProductionOrderID = oProductionOrder.ProductionOrderID;
                    ProductionOrderDetailDA.Delete(tc, oProductionOrderDetail, EnumDBOperation.Delete, nUserID, sProductionOrderDetailIDs);
                    #endregion

                    #region Export Pi Sizer BreakDown Part
                    foreach (POSizerBreakDown oItem in oPOSizerBreakDowns)
                    {
                        IDataReader readerBreakDown;
                        oItem.ProductionOrderID = oProductionOrder.ProductionOrderID;
                        if (oItem.POSizerBreakDownID <= 0)
                        {
                            readerBreakDown = POSizerBreakDownDA.InsertUpdate(tc, oItem, EnumDBOperation.Insert, nUserID);
                        }
                        else
                        {
                            readerBreakDown = POSizerBreakDownDA.InsertUpdate(tc, oItem, EnumDBOperation.Update, nUserID);
                        }
                        NullHandler oReaderBreakDown = new NullHandler(readerBreakDown);
                        if (readerBreakDown.Read())
                        {
                            sBreakDownIDs = sBreakDownIDs + oReaderBreakDown.GetString("POSizerBreakDownID") + ",";
                        }
                        readerBreakDown.Close();
                    }
                    if (sBreakDownIDs.Length > 0)
                    {
                        sBreakDownIDs = sBreakDownIDs.Remove(sBreakDownIDs.Length - 1, 1);
                    }
                    POSizerBreakDown oPOSizerBreakDown = new POSizerBreakDown();
                    oPOSizerBreakDown.ProductionOrderID = oProductionOrder.ProductionOrderID;
                    POSizerBreakDownDA.Delete(tc, oPOSizerBreakDown, EnumDBOperation.Delete, nUserID, sBreakDownIDs);
                    #endregion

                    #region Get Production Order
                    reader = ProductionOrderDA.Get(tc, oProductionOrder.ProductionOrderID);
                    oReader = new NullHandler(reader);
                    if (reader.Read())
                    {
                        oProductionOrder = new ProductionOrder();
                        oProductionOrder = CreateObject(oReader);
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
						oProductionOrder = new ProductionOrder();
						oProductionOrder.ErrorMessage = e.Message.Split('!')[0];
					}
					#endregion
				}
				return oProductionOrder;
			}

            public ProductionOrder AcceptRevise(ProductionOrder oProductionOrder, Int64 nUserID)
            {
                TransactionContext tc = null;
                string sProductionOrderDetailIDs = "";
                List<ProductionOrderDetail> oProductionOrderDetails = new List<ProductionOrderDetail>();
                List<POSizerBreakDown> oPOSizerBreakDowns = new List<POSizerBreakDown>();
                ProductionOrderDetail oProductionOrderDetail = new ProductionOrderDetail();
                oProductionOrderDetails = oProductionOrder.ProductionOrderDetails;
                oPOSizerBreakDowns = oProductionOrder.POSizerBreakDowns;
                string sBreakDownIDs = "";
                try
                {
                    tc = TransactionContext.Begin(true);
                    IDataReader reader;
                    reader = ProductionOrderDA.AcceptRevise(tc, oProductionOrder,  nUserID);
                    NullHandler oReader = new NullHandler(reader);
                    if (reader.Read())
                    {
                        oProductionOrder = new ProductionOrder();
                        oProductionOrder = CreateObject(oReader);
                    }
                    reader.Close();
                    #region Production Order Detail Part
                    foreach (ProductionOrderDetail oItem in oProductionOrderDetails)
                    {
                        IDataReader readerdetail;
                        oItem.ProductionOrderID = oProductionOrder.ProductionOrderID;
                        if (oItem.ProductionOrderDetailID <= 0)
                        {
                            readerdetail = ProductionOrderDetailDA.InsertUpdate(tc, oItem, EnumDBOperation.Insert, nUserID, "");
                        }
                        else
                        {
                            readerdetail = ProductionOrderDetailDA.InsertUpdate(tc, oItem, EnumDBOperation.Update, nUserID, "");
                        }
                        NullHandler oReaderDetail = new NullHandler(readerdetail);
                        if (readerdetail.Read())
                        {
                            sProductionOrderDetailIDs = sProductionOrderDetailIDs + oReaderDetail.GetString("ProductionOrderDetailID") + ",";
                        }
                        readerdetail.Close();
                    }
                    if (sProductionOrderDetailIDs.Length > 0)
                    {
                        sProductionOrderDetailIDs = sProductionOrderDetailIDs.Remove(sProductionOrderDetailIDs.Length - 1, 1);
                    }
                    oProductionOrderDetail = new ProductionOrderDetail();
                    oProductionOrderDetail.ProductionOrderID = oProductionOrder.ProductionOrderID;
                    ProductionOrderDetailDA.Delete(tc, oProductionOrderDetail, EnumDBOperation.Delete, nUserID, sProductionOrderDetailIDs);
                    #endregion

                    #region Export Pi Sizer BreakDown Part
                    foreach (POSizerBreakDown oItem in oPOSizerBreakDowns)
                    {
                        IDataReader readerBreakDown;
                        oItem.ProductionOrderID = oProductionOrder.ProductionOrderID;
                        if (oItem.POSizerBreakDownID <= 0)
                        {
                            readerBreakDown = POSizerBreakDownDA.InsertUpdate(tc, oItem, EnumDBOperation.Insert, nUserID);
                        }
                        else
                        {
                            readerBreakDown = POSizerBreakDownDA.InsertUpdate(tc, oItem, EnumDBOperation.Update, nUserID);
                        }
                        NullHandler oReaderBreakDown = new NullHandler(readerBreakDown);
                        if (readerBreakDown.Read())
                        {
                            sBreakDownIDs = sBreakDownIDs + oReaderBreakDown.GetString("POSizerBreakDownID") + ",";
                        }
                        readerBreakDown.Close();
                    }
                    if (sBreakDownIDs.Length > 0)
                    {
                        sBreakDownIDs = sBreakDownIDs.Remove(sBreakDownIDs.Length - 1, 1);
                    }
                    POSizerBreakDown oPOSizerBreakDown = new POSizerBreakDown();
                    oPOSizerBreakDown.ProductionOrderID = oProductionOrder.ProductionOrderID;
                    POSizerBreakDownDA.Delete(tc, oPOSizerBreakDown, EnumDBOperation.Delete, nUserID, sBreakDownIDs);
                    #endregion

                    #region Get Production Order
                    reader = ProductionOrderDA.Get(tc, oProductionOrder.ProductionOrderID);
                    oReader = new NullHandler(reader);
                    if (reader.Read())
                    {
                        oProductionOrder = new ProductionOrder();
                        oProductionOrder = CreateObject(oReader);
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
                        oProductionOrder = new ProductionOrder();
                        oProductionOrder.ErrorMessage = e.Message.Split('!')[0];
                    }
                    #endregion
                }
                return oProductionOrder;
            }
            public ProductionOrder ChangeStatus(ProductionOrder oProductionOrder, Int64 nUserID)
            {
                ApprovalRequest oApprovalRequest = new ApprovalRequest();
                ReviseRequest oReviseRequest = new ReviseRequest();
                oApprovalRequest = oProductionOrder.ApprovalRequest;
                oReviseRequest = oProductionOrder.ReviseRequest;
                TransactionContext tc = null;
                try
                {
                    tc = TransactionContext.Begin(true);
                    IDataReader reader;
                    if (oProductionOrder.ProductionOrderActionType == EnumProductionOrderActionType.Approve)
                    {
                        AuthorizationRoleDA.CheckUserPermission(tc, nUserID, EnumModuleName.ProductionOrder, EnumRoleOperationType.Approved);
                    }
                    if (oProductionOrder.ProductionOrderActionType == EnumProductionOrderActionType.Cancel)
                    {
                        AuthorizationRoleDA.CheckUserPermission(tc, nUserID, EnumModuleName.ProductionOrder, EnumRoleOperationType.Cancel);
                    }
                    if (oProductionOrder.ProductionOrderActionType == EnumProductionOrderActionType.InProduction)
                    {
                        ProductionOrderDA.SendToProduction(tc, oProductionOrder, nUserID);
                    }

                    reader = ProductionOrderDA.ChangeStatus(tc, oProductionOrder, EnumDBOperation.Insert, nUserID);
                    NullHandler oReader = new NullHandler(reader);
                    if (reader.Read())
                    {
                        oProductionOrder = new ProductionOrder();
                        oProductionOrder = CreateObject(oReader);
                    }
                    reader.Close();
                    if (oApprovalRequest!=null)
                    {
                        if (oProductionOrder.ProductionOrderStatus == EnumProductionOrderStatus.Request_For_Approval && (oApprovalRequest.RequestBy != 0 && oApprovalRequest.RequestTo != 0))
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
                    oProductionOrder.ErrorMessage = Message;
                    //ExceptionLog.Write(e);
                    //throw new ServiceException("Failed to Save ProductionOrderDetail. Because of " + e.Message, e);
                    #endregion
                }
                return oProductionOrder;
            }
			public string Delete(int id, Int64 nUserId)
			{
				TransactionContext tc = null;
				try
				{
					tc = TransactionContext.Begin(true);
					ProductionOrder oProductionOrder = new ProductionOrder();
					oProductionOrder.ProductionOrderID = id;
					AuthorizationRoleDA.CheckUserPermission(tc, nUserId, EnumModuleName.ProductionOrder, EnumRoleOperationType.Delete);
					DBTableReferenceDA.HasReference(tc, "ProductionOrder", id);
					ProductionOrderDA.Delete(tc, oProductionOrder, EnumDBOperation.Delete, nUserId);
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
			public ProductionOrder Get(int id, Int64 nUserId)
			{
				ProductionOrder oProductionOrder = new ProductionOrder();
				TransactionContext tc = null;
				try
				{
					tc = TransactionContext.Begin();
					IDataReader reader = ProductionOrderDA.Get(tc, id);
					NullHandler oReader = new NullHandler(reader);
					if (reader.Read())
					{
					oProductionOrder = CreateObject(oReader);
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
					throw new ServiceException("Failed to Get ProductionOrder", e);
					#endregion
				}
				return oProductionOrder;
			}

            public ProductionOrder GetByLog(int nLogid, Int64 nUserId)
            {
                ProductionOrder oProductionOrder = new ProductionOrder();
                TransactionContext tc = null;
                try
                {
                    tc = TransactionContext.Begin();
                    IDataReader reader = ProductionOrderDA.GetByLog(tc, nLogid);
                    NullHandler oReader = new NullHandler(reader);
                    if (reader.Read())
                    {
                        oProductionOrder = CreateObject(oReader);
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
                    throw new ServiceException("Failed to Get ProductionOrder", e);
                    #endregion
                }
                return oProductionOrder;
            }
			public List<ProductionOrder> Gets(Int64 nUserID)
			{
				List<ProductionOrder> oProductionOrders = new List<ProductionOrder>();
				TransactionContext tc = null;
				try
				{
					tc = TransactionContext.Begin();
					IDataReader reader = null;
					reader = ProductionOrderDA.Gets(tc);
					oProductionOrders = CreateObjects(reader);
					reader.Close();
					tc.End();
				}
				catch (Exception e)
				{
					#region Handle Exception
					if (tc != null)
					tc.HandleError();
					ProductionOrder oProductionOrder = new ProductionOrder();
					oProductionOrder.ErrorMessage =  e.Message.Split('!')[0];
					#endregion
				}
				return oProductionOrders;
			}
            //
            public List<ProductionOrder> BUWithProductNatureWiseGets(int nBUID, int ProductNature, Int64 nUserID)
            {
                List<ProductionOrder> oProductionOrders = new List<ProductionOrder>();
                TransactionContext tc = null;
                try
                {
                    tc = TransactionContext.Begin();
                    IDataReader reader = null;
                    reader = ProductionOrderDA.BUWithProductNatureWiseGets(nBUID,ProductNature, tc);
                    oProductionOrders = CreateObjects(reader);
                    reader.Close();
                    tc.End();
                }
                catch (Exception e)
                {
                    #region Handle Exception
                    if (tc != null)
                        tc.HandleError();
                    ProductionOrder oProductionOrder = new ProductionOrder();
                    oProductionOrder.ErrorMessage = e.Message.Split('!')[0];
                    #endregion
                }
                return oProductionOrders;
            }

			public List<ProductionOrder> Gets (string sSQL, Int64 nUserID)
			{
				List<ProductionOrder> oProductionOrders = new List<ProductionOrder>();
				TransactionContext tc = null;
				try
				{
					tc = TransactionContext.Begin();
					IDataReader reader = null;
					reader = ProductionOrderDA.Gets(tc, sSQL);
					oProductionOrders = CreateObjects(reader);
					reader.Close();
					tc.End();
				}
				catch (Exception e)
				{
					#region Handle Exception
					if (tc != null);
					tc.HandleError();
					ExceptionLog.Write(e);
					throw new ServiceException("Failed to Get ProductionOrder", e);
					#endregion
				}
				return oProductionOrders;
			}

		#endregion
	}

}
