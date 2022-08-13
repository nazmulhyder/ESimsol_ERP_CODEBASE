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
	public class OrderSheetService : MarshalByRefObject, IOrderSheetService
	{
		#region Private functions and declaration

		private OrderSheet MapObject(NullHandler oReader)
		{
			OrderSheet oOrderSheet = new OrderSheet();
			oOrderSheet.OrderSheetID = oReader.GetInt32("OrderSheetID");
            oOrderSheet.OrderSheetLogID = oReader.GetInt32("OrderSheetLogID");
			oOrderSheet.PONo = oReader.GetString("PONo");
            oOrderSheet.ReviseNo = oReader.GetInt32("ReviseNo");
            oOrderSheet.FullPONo = oReader.GetString("FullPONo");
			oOrderSheet.OrderDate = oReader.GetDateTime("OrderDate");
			oOrderSheet.BUID = oReader.GetInt32("BUID");
            oOrderSheet.OrderSheetStatus = (EnumOrderSheetStatus)oReader.GetInt32("OrderSheetStatus");
            oOrderSheet.OrderSheetStatusInInt = oReader.GetInt32("OrderSheetStatus");
            oOrderSheet.OrderSheetType = (EnumOrderSheetType)oReader.GetInt32("OrderSheetType");
            oOrderSheet.ContractorID = oReader.GetInt32("ContractorID");
			oOrderSheet.ContactPersonnelID = oReader.GetInt32("ContactPersonnelID");
			oOrderSheet.Priority = (EnumPriorityLevel) oReader.GetInt32("Priority");
			oOrderSheet.Note = oReader.GetString("Note");
			oOrderSheet.MKTEmpID = oReader.GetInt32("MKTEmpID");
			oOrderSheet.ApproveDate = oReader.GetDateTime("ApproveDate");
			oOrderSheet.ApproveBy = oReader.GetInt32("ApproveBy");
			oOrderSheet.PartyPONo = oReader.GetString("PartyPONo");
            oOrderSheet.PaymentType = (EnumPaymentType)oReader.GetInt32("PaymentType");
			oOrderSheet.DeliveryTo = oReader.GetInt32("DeliveryTo");
			oOrderSheet.DeliveryContactPerson = oReader.GetInt32("DeliveryContactPerson");
			oOrderSheet.ContractorName = oReader.GetString("ContractorName");
			oOrderSheet.ContractorAddress = oReader.GetString("ContractorAddress");
			oOrderSheet.ContactPersonnelName = oReader.GetString("ContactPersonnelName");
			oOrderSheet.DeliveryContactPersonName = oReader.GetString("DeliveryContactPersonName");
			oOrderSheet.MKTPName = oReader.GetString("MKTPName");
			oOrderSheet.MKTPNickName = oReader.GetString("MKTPNickName");
			oOrderSheet.ApproveByName = oReader.GetString("ApproveByName");
			oOrderSheet.DeliveryToName = oReader.GetString("DeliveryToName");
            oOrderSheet.CurrencyName = oReader.GetString("CurrencyName");
            oOrderSheet.CurrencySymbol = oReader.GetString("CurrencySymbol");
            oOrderSheet.CurrencyID = oReader.GetInt32("CurrencyID");
            oOrderSheet.ProductNature = (EnumProductNature)oReader.GetInt32("ProductNature");
			oOrderSheet.Qty = oReader.GetDouble("Qty");
			oOrderSheet.Amount = oReader.GetDouble("Amount");
            oOrderSheet.BuyerID = oReader.GetInt32("BuyerID");
            oOrderSheet.BuyerName = oReader.GetString("BuyerName");
            oOrderSheet.ExpectedDeliveryDate = oReader.GetDateTime("ExpectedDeliveryDate");
            oOrderSheet.RateUnit = oReader.GetInt32("RateUnit");            
			return oOrderSheet;
		}

		private OrderSheet CreateObject(NullHandler oReader)
		{
			OrderSheet oOrderSheet = new OrderSheet();
			oOrderSheet = MapObject(oReader);
			return oOrderSheet;
		}

		private List<OrderSheet> CreateObjects(IDataReader oReader)
		{
			List<OrderSheet> oOrderSheet = new List<OrderSheet>();
			NullHandler oHandler = new NullHandler(oReader);
			while (oReader.Read())
			{
				OrderSheet oItem = CreateObject(oHandler);
				oOrderSheet.Add(oItem);
			}
			return oOrderSheet;
		}

		#endregion

		#region Interface implementation
			public OrderSheet Save(OrderSheet oOrderSheet, Int64 nUserID)
			{
				TransactionContext tc = null;
                string sOrderSheetDetailIDs = "";
                List<OrderSheetDetail> oOrderSheetDetails = new List<OrderSheetDetail>();
                OrderSheetDetail oOrderSheetDetail = new OrderSheetDetail();
                oOrderSheetDetails = oOrderSheet.OrderSheetDetails;
				try
				{
					tc = TransactionContext.Begin(true);
					IDataReader reader;
					if (oOrderSheet.OrderSheetID <= 0)
					{
                        AuthorizationRoleDA.CheckUserPermission(tc, nUserID, EnumModuleName.OrderSheet, EnumRoleOperationType.Add);
						reader = OrderSheetDA.InsertUpdate(tc, oOrderSheet, EnumDBOperation.Insert, nUserID);
					}
					else{
                        AuthorizationRoleDA.CheckUserPermission(tc, nUserID, EnumModuleName.OrderSheet, EnumRoleOperationType.Edit);
						reader = OrderSheetDA.InsertUpdate(tc, oOrderSheet, EnumDBOperation.Update, nUserID);
					}
					NullHandler oReader = new NullHandler(reader);
					if (reader.Read())
					{
						oOrderSheet = new OrderSheet();
						oOrderSheet = CreateObject(oReader);
					}
					reader.Close();
                    #region Order Sheet Detail Part
                    foreach (OrderSheetDetail oItem in oOrderSheetDetails)
                    {
                        IDataReader readerdetail;
                        oItem.OrderSheetID = oOrderSheet.OrderSheetID;
                        if (oItem.OrderSheetDetailID <= 0)
                        {
                            readerdetail = OrderSheetDetailDA.InsertUpdate(tc, oItem, EnumDBOperation.Insert, nUserID, "");
                        }
                        else
                        {
                            readerdetail = OrderSheetDetailDA.InsertUpdate(tc, oItem, EnumDBOperation.Update, nUserID, "");
                        }
                        NullHandler oReaderDetail = new NullHandler(readerdetail);
                        if (readerdetail.Read())
                        {
                            sOrderSheetDetailIDs = sOrderSheetDetailIDs + oReaderDetail.GetString("OrderSheetDetailID") + ",";
                        }
                        readerdetail.Close();
                    }
                    if (sOrderSheetDetailIDs.Length > 0)
                    {
                        sOrderSheetDetailIDs = sOrderSheetDetailIDs.Remove(sOrderSheetDetailIDs.Length - 1, 1);
                    }
                    oOrderSheetDetail = new OrderSheetDetail();
                    oOrderSheetDetail.OrderSheetID = oOrderSheet.OrderSheetID;
                    OrderSheetDetailDA.Delete(tc, oOrderSheetDetail, EnumDBOperation.Delete, nUserID, sOrderSheetDetailIDs);
                    #endregion

                    #region Get Order Sheet for Actual Order Qty
                    reader = OrderSheetDA.Get(tc, oOrderSheet.OrderSheetID);
                    oReader = new NullHandler(reader);
                    if (reader.Read())
                    {
                        oOrderSheet = new OrderSheet();
                        oOrderSheet = CreateObject(oReader);
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
						oOrderSheet = new OrderSheet();
						oOrderSheet.ErrorMessage = e.Message.Split('!')[0];
					}
					#endregion
				}
				return oOrderSheet;
			}

            public OrderSheet AcceptRevise(OrderSheet oOrderSheet, Int64 nUserID)
            {
                TransactionContext tc = null;
                string sOrderSheetDetailIDs = "";
                List<OrderSheetDetail> oOrderSheetDetails = new List<OrderSheetDetail>();
                OrderSheetDetail oOrderSheetDetail = new OrderSheetDetail();
                oOrderSheetDetails = oOrderSheet.OrderSheetDetails;
                try
                {
                    tc = TransactionContext.Begin(true);
                    IDataReader reader;
                    reader = OrderSheetDA.AcceptRevise(tc, oOrderSheet, EnumDBOperation.Insert, nUserID);
                    NullHandler oReader = new NullHandler(reader);
                    if (reader.Read())
                    {
                        oOrderSheet = new OrderSheet();
                        oOrderSheet = CreateObject(oReader);
                    }
                    reader.Close();
                    #region Order Sheet Detail Part
                    foreach (OrderSheetDetail oItem in oOrderSheetDetails)
                    {
                        IDataReader readerdetail;
                        oItem.OrderSheetID = oOrderSheet.OrderSheetID;
                        if (oItem.OrderSheetDetailID <= 0)
                        {
                            readerdetail = OrderSheetDetailDA.InsertUpdate(tc, oItem, EnumDBOperation.Insert, nUserID, "");
                        }
                        else
                        {
                            readerdetail = OrderSheetDetailDA.InsertUpdate(tc, oItem, EnumDBOperation.Update, nUserID, "");
                        }
                        NullHandler oReaderDetail = new NullHandler(readerdetail);
                        if (readerdetail.Read())
                        {
                            sOrderSheetDetailIDs = sOrderSheetDetailIDs + oReaderDetail.GetString("OrderSheetDetailID") + ",";
                        }
                        readerdetail.Close();
                    }
                    if (sOrderSheetDetailIDs.Length > 0)
                    {
                        sOrderSheetDetailIDs = sOrderSheetDetailIDs.Remove(sOrderSheetDetailIDs.Length - 1, 1);
                    }
                    oOrderSheetDetail = new OrderSheetDetail();
                    oOrderSheetDetail.OrderSheetID = oOrderSheet.OrderSheetID;
                    OrderSheetDetailDA.Delete(tc, oOrderSheetDetail, EnumDBOperation.Delete, nUserID, sOrderSheetDetailIDs);
                    #endregion

                    #region Get Order Sheet for Actual Order Qty
                    reader = OrderSheetDA.Get(tc, oOrderSheet.OrderSheetID);
                    oReader = new NullHandler(reader);
                    if (reader.Read())
                    {
                        oOrderSheet = new OrderSheet();
                        oOrderSheet = CreateObject(oReader);
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
                        oOrderSheet = new OrderSheet();
                        oOrderSheet.ErrorMessage = e.Message.Split('!')[0];
                    }
                    #endregion
                }
                return oOrderSheet;
            }
            public OrderSheet ChangeStatus(OrderSheet oOrderSheet, Int64 nUserID)
            {
                ApprovalRequest oApprovalRequest = new ApprovalRequest();
                ReviseRequest oReviseRequest = new ReviseRequest();
                oApprovalRequest = oOrderSheet.ApprovalRequest;
                oReviseRequest = oOrderSheet.ReviseRequest;
                TransactionContext tc = null;
                try
                {
                    tc = TransactionContext.Begin(true);
                    IDataReader reader;
                    if (oOrderSheet.OrderSheetActionType == EnumOrderSheetActionType.Approve)
                    {
                        AuthorizationRoleDA.CheckUserPermission(tc, nUserID, EnumModuleName.OrderSheet, EnumRoleOperationType.Approved);
                    }
                    if (oOrderSheet.OrderSheetActionType == EnumOrderSheetActionType.Cancel)
                    {
                        AuthorizationRoleDA.CheckUserPermission(tc, nUserID, EnumModuleName.OrderSheet, EnumRoleOperationType.Cancel);
                    }
                    reader = OrderSheetDA.ChangeStatus(tc, oOrderSheet, EnumDBOperation.Insert, nUserID);
                    NullHandler oReader = new NullHandler(reader);
                    if (reader.Read())
                    {
                        oOrderSheet = new OrderSheet();
                        oOrderSheet = CreateObject(oReader);
                    }
                    reader.Close();
                    if (oApprovalRequest!=null)
                    {
                        if (oOrderSheet.OrderSheetStatus == EnumOrderSheetStatus.Request_For_Approval && (oApprovalRequest.RequestBy != 0 && oApprovalRequest.RequestTo != 0))
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
                        if (oOrderSheet.OrderSheetStatus == EnumOrderSheetStatus.Request_For_Revise && (oReviseRequest.RequestBy != 0 && oReviseRequest.RequestTo != 0))
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
                    oOrderSheet.ErrorMessage = Message;
                    //ExceptionLog.Write(e);
                    //throw new ServiceException("Failed to Save OrderSheetDetail. Because of " + e.Message, e);
                    #endregion
                }
                return oOrderSheet;
            }
			public string Delete(int id, Int64 nUserId)
			{
				TransactionContext tc = null;
				try
				{
					tc = TransactionContext.Begin(true);
					OrderSheet oOrderSheet = new OrderSheet();
					oOrderSheet.OrderSheetID = id;
					AuthorizationRoleDA.CheckUserPermission(tc, nUserId, EnumModuleName.OrderSheet, EnumRoleOperationType.Delete);
					DBTableReferenceDA.HasReference(tc, "OrderSheet", id);
					OrderSheetDA.Delete(tc, oOrderSheet, EnumDBOperation.Delete, nUserId);
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
			public OrderSheet Get(int id, Int64 nUserId)
			{
				OrderSheet oOrderSheet = new OrderSheet();
				TransactionContext tc = null;
				try
				{
					tc = TransactionContext.Begin();
					IDataReader reader = OrderSheetDA.Get(tc, id);
					NullHandler oReader = new NullHandler(reader);
					if (reader.Read())
					{
					oOrderSheet = CreateObject(oReader);
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
					throw new ServiceException("Failed to Get OrderSheet", e);
					#endregion
				}
				return oOrderSheet;
			}

            public OrderSheet GetByLog(int Logid, Int64 nUserId)
			{
				OrderSheet oOrderSheet = new OrderSheet();
				TransactionContext tc = null;
				try
				{
					tc = TransactionContext.Begin();
                    IDataReader reader = OrderSheetDA.GetByLog(tc, Logid);
					NullHandler oReader = new NullHandler(reader);
					if (reader.Read())
					{
					oOrderSheet = CreateObject(oReader);
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
					throw new ServiceException("Failed to Get OrderSheet", e);
					#endregion
				}
				return oOrderSheet;
			}
        
			public List<OrderSheet> Gets(Int64 nUserID)
			{
				List<OrderSheet> oOrderSheets = new List<OrderSheet>();
				TransactionContext tc = null;
				try
				{
					tc = TransactionContext.Begin();
					IDataReader reader = null;
					reader = OrderSheetDA.Gets(tc);
					oOrderSheets = CreateObjects(reader);
					reader.Close();
					tc.End();
				}
				catch (Exception e)
				{
					#region Handle Exception
					if (tc != null)
					tc.HandleError();
					OrderSheet oOrderSheet = new OrderSheet();
					oOrderSheet.ErrorMessage =  e.Message.Split('!')[0];
					#endregion
				}
				return oOrderSheets;
			}
            //
            public List<OrderSheet> BUWiseWithProductNatureGets(int nBUID, int nProductNature, Int64 nUserID)
            {
                List<OrderSheet> oOrderSheets = new List<OrderSheet>();
                TransactionContext tc = null;
                try
                {
                    tc = TransactionContext.Begin();
                    IDataReader reader = null;
                    reader = OrderSheetDA.BUWiseWithProductNatureGets(nBUID, nProductNature, tc);
                    oOrderSheets = CreateObjects(reader);
                    reader.Close();
                    tc.End();
                }
                catch (Exception e)
                {
                    #region Handle Exception
                    if (tc != null)
                        tc.HandleError();
                    OrderSheet oOrderSheet = new OrderSheet();
                    oOrderSheet.ErrorMessage = e.Message.Split('!')[0];
                    #endregion
                }
                return oOrderSheets;
            }

			public List<OrderSheet> Gets (string sSQL, Int64 nUserID)
			{
				List<OrderSheet> oOrderSheets = new List<OrderSheet>();
				TransactionContext tc = null;
				try
				{
					tc = TransactionContext.Begin();
					IDataReader reader = null;
					reader = OrderSheetDA.Gets(tc, sSQL);
					oOrderSheets = CreateObjects(reader);
					reader.Close();
					tc.End();
				}
				catch (Exception e)
				{
					#region Handle Exception
					if (tc != null);
					tc.HandleError();
					ExceptionLog.Write(e);
					throw new ServiceException("Failed to Get OrderSheet", e);
					#endregion
				}
				return oOrderSheets;
			}

		#endregion
	}

}
