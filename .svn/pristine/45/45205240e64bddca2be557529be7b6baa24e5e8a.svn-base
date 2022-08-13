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
	public class ServiceOrderService : MarshalByRefObject, IServiceOrderService
	{
		#region Private functions and declaration

		private ServiceOrder MapObject(NullHandler oReader)
		{
			ServiceOrder oServiceOrder = new ServiceOrder();
			oServiceOrder.ServiceOrderID = oReader.GetInt32("ServiceOrderID");
            oServiceOrder.ServiceOrderNo = oReader.GetString("ServiceOrderNo");
			oServiceOrder.ServiceOrderType = (EnumServiceOrderType) oReader.GetInt32("ServiceOrderType");
            oServiceOrder.ServiceOrderTypeInt = oReader.GetInt32("ServiceOrderType");
			oServiceOrder.VehicleRegistrationID = oReader.GetInt32("VehicleRegistrationID");
			oServiceOrder.AdvisorID = oReader.GetInt32("AdvisorID");
			oServiceOrder.CustomerID = oReader.GetInt32("CustomerID");
            oServiceOrder.CurrencyID = oReader.GetInt32("CurrencyID");
			oServiceOrder.ContactPersonID = oReader.GetInt32("ContactPersonID");
			oServiceOrder.KilometerReading = oReader.GetString("KilometerReading");
			oServiceOrder.ServiceOrderDate = oReader.GetDateTime("ServiceOrderDate");
			oServiceOrder.IssueDate = oReader.GetDateTime("IssueDate");
			oServiceOrder.RcvDateTime = oReader.GetDateTime("RcvDateTime");
			oServiceOrder.DelDateTime = oReader.GetDateTime("DelDateTime");
			oServiceOrder.Remarks = oReader.GetString("Remarks");
            oServiceOrder.CurrencySymbol = oReader.GetString("CurrencySymbol");
            oServiceOrder.CurrencyName = oReader.GetString("CurrencyName");
			oServiceOrder.OrderStatus =  (EnumServiceOrderStatus) oReader.GetInt32("OrderStatus");
            oServiceOrder.OrderStatusInt = oReader.GetInt32("OrderStatus");
			oServiceOrder.ApproveByID = oReader.GetInt32("ApproveByID");
			oServiceOrder.ActualRcvDateTime = oReader.GetDateTime("ActualRcvDateTime");
			oServiceOrder.ActualDelDateTime = oReader.GetDateTime("ActualDelDateTime");
			oServiceOrder.LastUpdateBy = oReader.GetInt32("LastUpdateBy");
			oServiceOrder.LastUpdateDateTime = oReader.GetDateTime("LastUpdateDateTime");
			oServiceOrder.MobilityService = oReader.GetString("MobilityService");
			oServiceOrder.IPNo = oReader.GetString("IPNo");
			oServiceOrder.IPExpDate = oReader.GetString("IPExpDate");
			oServiceOrder.SoldByDealer = oReader.GetString("SoldByDealer");
			oServiceOrder.NoShowStatus = oReader.GetString("NoShowStatus");
			oServiceOrder.ReasonOfVisit = oReader.GetString("ReasonOfVisit");
			oServiceOrder.ExtendedWarranty = oReader.GetString("ExtendedWarranty");
            oServiceOrder.ServicePlan = oReader.GetString("ServicePlan");
			oServiceOrder.RSAPolicyNo = oReader.GetString("RSAPolicyNo");
			oServiceOrder.FuelStatus = (EnumFuelStatus) oReader.GetInt32("FuelStatus");
            oServiceOrder.FuelStatusInt = oReader.GetInt32("FuelStatus");

			oServiceOrder.NoOfKeys = oReader.GetInt32("NoOfKeys");
			oServiceOrder.ENetAmount = oReader.GetDouble("ENetAmount");
			oServiceOrder.ELCAmount = oReader.GetDouble("ELCAmount");
			oServiceOrder.EPartsAmount = oReader.GetDouble("EPartsAmount");
			oServiceOrder.ModeOfPaymentInt = oReader.GetInt32("ModeOfPayment");
            oServiceOrder.ModeOfPayment = (EnumPaymentMethod) oReader.GetInt32("ModeOfPayment");
			oServiceOrder.IsTaxesApplicable = oReader.GetBoolean("IsTaxesApplicable");
			oServiceOrder.IsWindows = oReader.GetBoolean("IsWindows");
			oServiceOrder.IsWiperBlades = oReader.GetBoolean("IsWiperBlades");
			oServiceOrder.IsLIghts = oReader.GetBoolean("IsLIghts");
			oServiceOrder.IsExhaustSys = oReader.GetBoolean("IsExhaustSys");
			oServiceOrder.IsUnderbody = oReader.GetBoolean("IsUnderbody");
			oServiceOrder.IsEngineComp = oReader.GetBoolean("IsEngineComp");
			oServiceOrder.IsWashing = oReader.GetBoolean("IsWashing");
			oServiceOrder.IsOilLevel = oReader.GetBoolean("IsOilLevel");
			oServiceOrder.IsCoolant = oReader.GetBoolean("IsCoolant");
			oServiceOrder.IsWindWasher = oReader.GetBoolean("IsWindWasher");
			oServiceOrder.IsBreakes = oReader.GetBoolean("IsBreakes");
			oServiceOrder.IsAxle = oReader.GetBoolean("IsAxle");
			oServiceOrder.IsMonograms = oReader.GetBoolean("IsMonograms");
			oServiceOrder.IsPolishing = oReader.GetBoolean("IsPolishing");
			oServiceOrder.IsOwnersManual = oReader.GetBoolean("IsOwnersManual");
			oServiceOrder.IsScheManual = oReader.GetBoolean("IsScheManual");
			oServiceOrder.IsNavManual = oReader.GetBoolean("IsNavManual");
			oServiceOrder.IsWBook = oReader.GetBoolean("IsWBook");
			oServiceOrder.IsRefGuide = oReader.GetBoolean("IsRefGuide");
			oServiceOrder.IsSpareWheel = oReader.GetBoolean("IsSpareWheel");
			oServiceOrder.IsToolKits = oReader.GetBoolean("IsToolKits");
			oServiceOrder.IsFloorMats = oReader.GetBoolean("IsFloorMats");
			oServiceOrder.IsMudFlaps = oReader.GetBoolean("IsMudFlaps");
			oServiceOrder.IsWarningT = oReader.GetBoolean("IsWarningT");
			oServiceOrder.IsFirstAidKit = oReader.GetBoolean("IsFirstAidKit");
            oServiceOrder.NoOfCDs = oReader.GetInt32("NoOfCDs");
			oServiceOrder.IsOtherLoose = oReader.GetBoolean("IsOtherLoose");
			oServiceOrder.ServiceOrderNoFull = oReader.GetString("ServiceOrderNoFull");
			oServiceOrder.VehicleRegNo = oReader.GetString("VehicleRegNo");
			oServiceOrder.ChassisNo = oReader.GetString("ChassisNo");
			oServiceOrder.EngineNo = oReader.GetString("EngineNo");
			oServiceOrder.VehicleModelNo = oReader.GetString("VehicleModelNo");
			oServiceOrder.VehicleTypeName = oReader.GetString("VehicleTypeName");
			oServiceOrder.ApproveByName = oReader.GetString("ApproveByName");
            oServiceOrder.CustomerName = oReader.GetString("CustomerName");
            oServiceOrder.CustomerAddress = oReader.GetString("CustomerAddress");
            oServiceOrder.CustomerPhone = oReader.GetString("CustomerPhone");
            oServiceOrder.CustomerEmail = oReader.GetString("CustomerEmail");
			oServiceOrder.ContactPerson = oReader.GetString("ContactPerson");
			oServiceOrder.ContactPersonPhone = oReader.GetString("ContactPersonPhone");
			oServiceOrder.AdvisorName = oReader.GetString("AdvisorName");
			oServiceOrder.AttachDocumentID = oReader.GetInt32("AttachDocumentID");
            oServiceOrder.CustomerVoice = oReader.GetString("CustomerVoice");
            oServiceOrder.TechincalVoice = oReader.GetString("TechincalVoice");
            oServiceOrder.RemainingFreeService = oReader.GetString("RemainingFreeService");

            //oServiceOrder.CustomerVoice = oServiceOrder.CustomerVoice.Replace("~", System.Environment.NewLine);
            //oServiceOrder.TechincalVoice = oServiceOrder.CustomerVoice.Replace("~", System.Environment.NewLine);

			return oServiceOrder;
		}

		private ServiceOrder CreateObject(NullHandler oReader)
		{
			ServiceOrder oServiceOrder = new ServiceOrder();
			oServiceOrder = MapObject(oReader);
			return oServiceOrder;
		}

		private List<ServiceOrder> CreateObjects(IDataReader oReader)
		{
			List<ServiceOrder> oServiceOrder = new List<ServiceOrder>();
			NullHandler oHandler = new NullHandler(oReader);
			while (oReader.Read())
			{
				ServiceOrder oItem = CreateObject(oHandler);
				oServiceOrder.Add(oItem);
			}
			return oServiceOrder;
		}

		#endregion

		#region Interface implementation
			public ServiceOrder Save(ServiceOrder oServiceOrder, Int64 nUserID)
			{
				TransactionContext tc = null;
                List<ServiceOrderDetail> oServiceOrderDetails = new List<ServiceOrderDetail>();
                oServiceOrderDetails = oServiceOrder.ServiceOrderDetails;
                string sServiceOrderDetailIDs = "";
				try
				{
					tc = TransactionContext.Begin(true);
					IDataReader reader;
					if (oServiceOrder.ServiceOrderID <= 0)
					{
						AuthorizationRoleDA.CheckUserPermission(tc, nUserID, EnumModuleName.ServiceOrder, EnumRoleOperationType.Add);
						reader = ServiceOrderDA.InsertUpdate(tc, oServiceOrder, EnumDBOperation.Insert, nUserID);
					}
					else{
                        AuthorizationRoleDA.CheckUserPermission(tc, nUserID, EnumModuleName.ServiceOrder, EnumRoleOperationType.Edit);
						reader = ServiceOrderDA.InsertUpdate(tc, oServiceOrder, EnumDBOperation.Update, nUserID);
					}
					NullHandler oReader = new NullHandler(reader);
					if (reader.Read())
					{
						oServiceOrder = new ServiceOrder();
						oServiceOrder = CreateObject(oReader);
					}
					reader.Close();

                    if (oServiceOrderDetails != null) //throw (new Exception("No Details Found!!")); 
                    {   
                        #region Service Order Detail Part
                        foreach (ServiceOrderDetail oItem in oServiceOrderDetails)
                        {
                            IDataReader readerdetail;
                            oItem.ServiceOrderID = oServiceOrder.ServiceOrderID;
                            if (oItem.ServiceOrderDetailID <= 0)
                            {
                                readerdetail = ServiceOrderDetailDA.InsertUpdate(tc, oItem, EnumDBOperation.Insert, nUserID, "");
                            }
                            else
                            {
                                readerdetail = ServiceOrderDetailDA.InsertUpdate(tc, oItem, EnumDBOperation.Update, nUserID, "");
                            }
                            NullHandler oReaderDetail = new NullHandler(readerdetail);
                            if (readerdetail.Read())
                            {
                                sServiceOrderDetailIDs = sServiceOrderDetailIDs + oReaderDetail.GetString("ServiceOrderDetailID") + ",";
                            }
                            readerdetail.Close();
                        }
                        if (sServiceOrderDetailIDs.Length > 0)
                        {
                            sServiceOrderDetailIDs = sServiceOrderDetailIDs.Remove(sServiceOrderDetailIDs.Length - 1, 1);
                        }
                        ServiceOrderDetail oServiceOrderDetail = new ServiceOrderDetail();
                        oServiceOrderDetail.ServiceOrderID = oServiceOrder.ServiceOrderID;
                        ServiceOrderDetailDA.Delete(tc, oServiceOrderDetail, EnumDBOperation.Delete, nUserID, sServiceOrderDetailIDs);
                        #endregion
                    }
                    #region Get Service Order 
                    reader = ServiceOrderDA.Get(tc, oServiceOrder.ServiceOrderID);
                    oReader = new NullHandler(reader);
                    if (reader.Read())
                    {
                        oServiceOrder = new ServiceOrder();
                        oServiceOrder = CreateObject(oReader);
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
						oServiceOrder = new ServiceOrder();
						oServiceOrder.ErrorMessage = e.Message.Split('!')[0];
					}
					#endregion
				}
				return oServiceOrder;
			}

            public string UpdateStatus(ServiceOrder oServiceOrder, Int64 nUserID)
            {
                TransactionContext tc = null;
                string sMessage = "";
                try
                {
                    tc = TransactionContext.Begin(true);

                    if (oServiceOrder.nRequest == 1)//Approve
                    {
                        sMessage = "Approved";
                        AuthorizationRoleDA.CheckUserPermission(tc, nUserID, EnumModuleName.ServiceOrder, EnumRoleOperationType.Approved);
                        DBTableReferenceDA.HasReference(tc, "ServiceOrder", oServiceOrder.ServiceOrderID);

                        oServiceOrder.OrderStatus = EnumServiceOrderStatus.Approved;
                        ServiceOrderDA.UpdateStatus(tc, oServiceOrder, EnumDBOperation.Approval, nUserID);
                    }
                    else if (oServiceOrder.nRequest == 2)//Received
                    {
                        sMessage = "Received";
                        AuthorizationRoleDA.CheckUserPermission(tc, nUserID, EnumModuleName.ServiceOrder, EnumRoleOperationType.Received);
                        DBTableReferenceDA.HasReference(tc, "ServiceOrder", oServiceOrder.ServiceOrderID);

                        oServiceOrder.OrderStatus = EnumServiceOrderStatus.Received;
                        ServiceOrderDA.UpdateStatus(tc, oServiceOrder, EnumDBOperation.Receive, nUserID);
                    }
                    else if (oServiceOrder.nRequest == 3)//Deliverd
                    {
                        sMessage = "Delivered";
                        AuthorizationRoleDA.CheckUserPermission(tc, nUserID, EnumModuleName.ServiceOrder, EnumRoleOperationType.DeliverToParty);
                        DBTableReferenceDA.HasReference(tc, "ServiceOrder", oServiceOrder.ServiceOrderID);

                        oServiceOrder.OrderStatus = EnumServiceOrderStatus.Done;
                        ServiceOrderDA.UpdateStatus(tc, oServiceOrder, EnumDBOperation.Delivered, nUserID);
                    }
                    else if (oServiceOrder.nRequest == 4)//Cancel
                    {
                        sMessage = "Cancel";
                        AuthorizationRoleDA.CheckUserPermission(tc, nUserID, EnumModuleName.ServiceOrder, EnumRoleOperationType.Cancel);
                        DBTableReferenceDA.HasReference(tc, "ServiceOrder", oServiceOrder.ServiceOrderID);

                        oServiceOrder.OrderStatus = EnumServiceOrderStatus.Canceled;
                        ServiceOrderDA.UpdateStatus(tc, oServiceOrder, EnumDBOperation.Cancel, nUserID);
                    }
                    tc.End();
                }
                catch (Exception e)
                {
                    #region Handle Exception
                    if (tc != null)
                    {
                        tc.HandleError();
                        return e.Message.Split('!')[0];
                    }
                    #endregion
                }
                return sMessage;
            }
			public string Delete(int id, Int64 nUserId)
			{
				TransactionContext tc = null;
				try
				{
					tc = TransactionContext.Begin(true);
					ServiceOrder oServiceOrder = new ServiceOrder();
					oServiceOrder.ServiceOrderID = id;
                    AuthorizationRoleDA.CheckUserPermission(tc, nUserId, EnumModuleName.ServiceOrder, EnumRoleOperationType.Delete);
					DBTableReferenceDA.HasReference(tc, "ServiceOrder", id);
					ServiceOrderDA.Delete(tc, oServiceOrder, EnumDBOperation.Delete, nUserId);
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

			public ServiceOrder Get(int id, Int64 nUserId)
			{
				ServiceOrder oServiceOrder = new ServiceOrder();
				TransactionContext tc = null;
				try
				{
					tc = TransactionContext.Begin();
					IDataReader reader = ServiceOrderDA.Get(tc, id);
					NullHandler oReader = new NullHandler(reader);
					if (reader.Read())
					{
					oServiceOrder = CreateObject(oReader);
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
					throw new ServiceException("Failed to Get ServiceOrder", e);
					#endregion
				}
				return oServiceOrder;
			}

			public List<ServiceOrder> Gets(Int64 nUserID)
			{
				List<ServiceOrder> oServiceOrders = new List<ServiceOrder>();
				TransactionContext tc = null;
				try
				{
					tc = TransactionContext.Begin();
					IDataReader reader = null;
					reader = ServiceOrderDA.Gets(tc);
					oServiceOrders = CreateObjects(reader);
					reader.Close();
					tc.End();
				}
				catch (Exception e)
				{
					#region Handle Exception
					if (tc != null)
					tc.HandleError();
					ServiceOrder oServiceOrder = new ServiceOrder();
					oServiceOrder.ErrorMessage =  e.Message.Split('!')[0];
					#endregion
				}
				return oServiceOrders;
			}

			public List<ServiceOrder> Gets (string sSQL, Int64 nUserID)
			{
				List<ServiceOrder> oServiceOrders = new List<ServiceOrder>();
				TransactionContext tc = null;
				try
				{
					tc = TransactionContext.Begin();
					IDataReader reader = null;
					reader = ServiceOrderDA.Gets(tc, sSQL);
					oServiceOrders = CreateObjects(reader);
					reader.Close();
					tc.End();
				}
				catch (Exception e)
				{
					#region Handle Exception
					if (tc != null);
					tc.HandleError();
					ExceptionLog.Write(e);
					throw new ServiceException("Failed to Get ServiceOrder", e);
					#endregion
				}
				return oServiceOrders;
			}

		#endregion
	}
}
