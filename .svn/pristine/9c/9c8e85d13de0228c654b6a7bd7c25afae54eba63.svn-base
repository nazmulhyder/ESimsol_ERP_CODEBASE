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
	public class ServiceInvoiceService : MarshalByRefObject, IServiceInvoiceService
	{
		#region Private functions and declaration

		private ServiceInvoice MapObject(NullHandler oReader)
		{
			ServiceInvoice oServiceInvoice = new ServiceInvoice();
            oServiceInvoice.ServiceInvoiceID = oReader.GetInt32("ServiceInvoiceID");
            oServiceInvoice.ServiceInvoiceLogID = oReader.GetInt32("ServiceInvoiceLogID");
            oServiceInvoice.ServiceOrderID = oReader.GetInt32("ServiceOrderID");
            oServiceInvoice.ServiceInvoiceNo = oReader.GetString("ServiceInvoiceNo");
            oServiceInvoice.ServiceOrderNo = oReader.GetString("ServiceOrderNo");
            oServiceInvoice.ServiceInvoiceDate = oReader.GetDateTime("ServiceInvoiceDate");
            oServiceInvoice.ServiceInvoiceType =(EnumServiceInvoiceType) oReader.GetInt32("ServiceInvoiceType");
            oServiceInvoice.InvoiceType = (EnumInvoiceType)oReader.GetInt32("InvoiceType");
            oServiceInvoice.InvoiceTypeInt =oReader.GetInt32("InvoiceType");
            oServiceInvoice.VehicleModelNo = oReader.GetString("VehicleModelNo");
            oServiceInvoice.VehicleRegNo = oReader.GetString("VehicleRegNo");
            oServiceInvoice.VehicleTypeName = oReader.GetString("VehicleTypeName");
            oServiceInvoice.VehicleRegistrationID = oReader.GetInt32("VehicleRegistrationID");
            oServiceInvoice.VehicleTypeName = oReader.GetString("VehicleTypeName");
            oServiceInvoice.ChassisNo = oReader.GetString("ChassisNo");
            oServiceInvoice.EngineNo = oReader.GetString("EngineNo");
            oServiceInvoice.ApproveByID = oReader.GetInt32("ApproveByID");
            oServiceInvoice.ApproveByName = oReader.GetString("ApproveByName");
            oServiceInvoice.ContactPerson = oReader.GetString("ContactPerson");
            oServiceInvoice.ContactPersonID = oReader.GetInt32("ContactPersonID");
            oServiceInvoice.CustomerID = oReader.GetInt32("CustomerID");
            oServiceInvoice.WorkOrderByID = oReader.GetInt32("WorkOrderByID");
            oServiceInvoice.WorkOrderByName = oReader.GetString("WorkInvoiceByName");
            oServiceInvoice.CustomerName = oReader.GetString("CustomerName");
            oServiceInvoice.CurrencyID = oReader.GetInt32("CurrencyID");
            oServiceInvoice.CurrencyName = oReader.GetString("CurrencyName");
            oServiceInvoice.KilometerReading = oReader.GetString("KilometerReading");
            oServiceInvoice.InvoiceStatus = (EnumServiceInvoiceStatus)oReader.GetInt32("InvoiceStatus");
            oServiceInvoice.Remarks = oReader.GetString("Remarks");
            oServiceInvoice.CustomerRemarks = oReader.GetString("CustomerRemarks");
            oServiceInvoice.OfficeRemarks = oReader.GetString("OfficeRemarks");
            oServiceInvoice.CustomerPhone = oReader.GetString("CustomerPhone");
            oServiceInvoice.ContactPersonPhone = oReader.GetString("ContactPersonPhone");
            oServiceInvoice.DiscountAmount_Parts = oReader.GetDouble("DiscountAmount_Parts");
            oServiceInvoice.NetAmount_Parts = oReader.GetDouble("NetAmount_Parts");
            oServiceInvoice.NetAmount_Payble = oReader.GetDouble("NetAmount_Payble");
            oServiceInvoice.LaborCharge_Discount = oReader.GetDouble("LaborCharge_Discount");
            oServiceInvoice.LaborCharge_Net = oReader.GetDouble("LaborCharge_Net");
            oServiceInvoice.LaborCharge_Vat = oReader.GetDouble("LaborCharge_Vat");
            oServiceInvoice.LaborCharge_Total = oReader.GetDouble("LaborCharge_Total");
            oServiceInvoice.PartsCharge_Vat = oReader.GetDouble("PartsCharge_Vat");
            oServiceInvoice.PartsCharge_Net = oReader.GetDouble("PartsCharge_Net");
           
            oServiceInvoice.RequisitionNo = oReader.GetString("RequisitionNo");
            oServiceInvoice.PrepaireBy = oReader.GetInt32("PrepaireBy");
            oServiceInvoice.PrepaireByName = oReader.GetString("PrepaireByName");

            oServiceInvoice.ServiceScheduleID = oReader.GetInt32("ServiceScheduleID");
            oServiceInvoice.ScheduleDate = oReader.GetDateTime("ScheduleDate");
            oServiceInvoice.ServiceDoneDate = oReader.GetDateTime("ServiceDoneDate"); 
			return oServiceInvoice;
		}

		private ServiceInvoice CreateObject(NullHandler oReader)
		{
			ServiceInvoice oServiceInvoice = new ServiceInvoice();
			oServiceInvoice = MapObject(oReader);
			return oServiceInvoice;
		}

		private List<ServiceInvoice> CreateObjects(IDataReader oReader)
		{
			List<ServiceInvoice> oServiceInvoice = new List<ServiceInvoice>();
			NullHandler oHandler = new NullHandler(oReader);
			while (oReader.Read())
			{
				ServiceInvoice oItem = CreateObject(oHandler);
				oServiceInvoice.Add(oItem);
			}
			return oServiceInvoice;
		}

		#endregion

		#region Interface implementation
        public ServiceInvoice Save(ServiceInvoice oServiceInvoice, Int64 nUserID)
        {
            TransactionContext tc = null;
            string sServiceInvoiceDetailIDs = "";
            List<ServiceInvoiceDetail> oServiceInvoiceDetails = new List<ServiceInvoiceDetail>();
            List<ServiceILaborChargeDetail> oServiceILaborChargeDetails= new List<ServiceILaborChargeDetail>();
            List<ServiceInvoiceTerms> oServiceInvoiceTerms = new List<ServiceInvoiceTerms>();
            ServiceInvoiceDetail oServiceInvoiceDetail = new ServiceInvoiceDetail();
            ServiceILaborChargeDetail oServiceILaborChargeDetail = new ServiceILaborChargeDetail();
            ServiceInvoiceTerms oServiceInvoiceTerm = new ServiceInvoiceTerms();
            oServiceInvoiceDetails = oServiceInvoice.ServiceInvoiceDetails;
            oServiceILaborChargeDetails = oServiceInvoice.ServiceILaborChargeDetails;
            oServiceInvoiceTerms = oServiceInvoice.ServiceInvoiceTermsList;
            try
            {
                tc = TransactionContext.Begin(true);
                IDataReader reader;
                if (oServiceInvoice.ServiceInvoiceID <= 0)
                {
                    oServiceInvoice.InvoiceStatus= EnumServiceInvoiceStatus.Initialize;
                    AuthorizationRoleDA.CheckUserPermission(tc, nUserID, EnumModuleName.ServiceInvoice, EnumRoleOperationType.Add);
                    reader = ServiceInvoiceDA.InsertUpdate(tc, oServiceInvoice, EnumDBOperation.Insert, nUserID);
                }
                else
                {
                    AuthorizationRoleDA.CheckUserPermission(tc, nUserID, EnumModuleName.ServiceInvoice, EnumRoleOperationType.Edit);
                    reader = ServiceInvoiceDA.InsertUpdate(tc, oServiceInvoice, EnumDBOperation.Update, nUserID);
                }
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oServiceInvoice = new ServiceInvoice();
                    oServiceInvoice = CreateObject(oReader);
                }
                reader.Close();
                #region Service Invoice Detail Part
                foreach (ServiceInvoiceDetail oItem in oServiceInvoiceDetails)
                {
                    IDataReader readerdetail;
                    oItem.ServiceInvoiceID = oServiceInvoice.ServiceInvoiceID;
                    if (oItem.ServiceInvoiceDetailID <= 0)
                    {
                        readerdetail = ServiceInvoiceDetailDA.InsertUpdate(tc, oItem, EnumDBOperation.Insert, nUserID, "");
                    }
                    else
                    {
                        readerdetail = ServiceInvoiceDetailDA.InsertUpdate(tc, oItem, EnumDBOperation.Update, nUserID, "");
                    }
                    NullHandler oReaderDetail = new NullHandler(readerdetail);
                    if (readerdetail.Read())
                    {
                        sServiceInvoiceDetailIDs = sServiceInvoiceDetailIDs + oReaderDetail.GetString("ServiceInvoiceDetailID") + ",";
                    }
                    readerdetail.Close();
                }
                if (sServiceInvoiceDetailIDs.Length > 0)
                {
                    sServiceInvoiceDetailIDs = sServiceInvoiceDetailIDs.Remove(sServiceInvoiceDetailIDs.Length - 1, 1);
                }
                oServiceInvoiceDetail = new ServiceInvoiceDetail();
                oServiceInvoiceDetail.ServiceInvoiceID = oServiceInvoice.ServiceInvoiceID;
                if (oServiceInvoice.ServiceInvoiceID>0)
                    ServiceInvoiceDetailDA.Delete(tc, oServiceInvoiceDetail, EnumDBOperation.Delete, nUserID, sServiceInvoiceDetailIDs);
                #endregion

                string sServiceILCDetailIDs = "";
                #region Service Invoice Labor Charge Detail Part
                foreach (ServiceILaborChargeDetail oItem in oServiceILaborChargeDetails)
                {
                    IDataReader readerdetail;
                    oItem.ServiceInvoiceID = oServiceInvoice.ServiceInvoiceID;
                    if (oItem.ServiceILaborChargeDetailID <= 0)
                    {
                        readerdetail = ServiceILaborChargeDetailDA.InsertUpdate(tc, oItem, EnumDBOperation.Insert, nUserID, "");
                    }
                    else
                    {
                        readerdetail = ServiceILaborChargeDetailDA.InsertUpdate(tc, oItem, EnumDBOperation.Update, nUserID, "");
                    }
                    NullHandler oReaderDetail = new NullHandler(readerdetail);
                    if (readerdetail.Read())
                    {
                        sServiceILCDetailIDs = sServiceILCDetailIDs + oReaderDetail.GetString("ServiceILaborChargeDetailID") + ",";
                    }
                    readerdetail.Close();
                }
                if (sServiceILCDetailIDs.Length > 0)
                {
                    sServiceILCDetailIDs = sServiceILCDetailIDs.Remove(sServiceILCDetailIDs.Length - 1, 1);
                }
                oServiceILaborChargeDetail = new ServiceILaborChargeDetail();
                oServiceILaborChargeDetail.ServiceInvoiceID = oServiceInvoice.ServiceInvoiceID;
                if (oServiceInvoice.ServiceInvoiceID > 0)
                    ServiceILaborChargeDetailDA.Delete(tc, oServiceILaborChargeDetail, EnumDBOperation.Delete, nUserID, sServiceILCDetailIDs);
                #endregion

                string sServiceTermsIDs = "";
                #region Service Invoice Terms
                foreach (ServiceInvoiceTerms oItem in oServiceInvoiceTerms)
                {
                    IDataReader readerdetail;
                    oItem.ServiceInvoiceID = oServiceInvoice.ServiceInvoiceID;
                    if (oItem.ServiceInvoiceTermsID <= 0)
                    {
                        readerdetail = ServiceInvoiceTermsDA.InsertUpdate(tc, oItem, EnumDBOperation.Insert, nUserID, "");
                    }
                    else
                    {
                        readerdetail = ServiceInvoiceTermsDA.InsertUpdate(tc, oItem, EnumDBOperation.Update, nUserID, "");
                    }
                    NullHandler oReaderDetail = new NullHandler(readerdetail);
                    if (readerdetail.Read())
                    {
                        sServiceTermsIDs = sServiceTermsIDs + oReaderDetail.GetString("ServiceInvoiceTermsID") + ",";
                    }
                    readerdetail.Close();
                }
                if (sServiceTermsIDs.Length > 0)
                {
                    sServiceTermsIDs = sServiceTermsIDs.Remove(sServiceTermsIDs.Length - 1, 1);
                }
                oServiceInvoiceTerm = new ServiceInvoiceTerms();
                oServiceInvoiceTerm.ServiceInvoiceID = oServiceInvoice.ServiceInvoiceID;
                if (oServiceInvoice.ServiceInvoiceID > 0)
                    ServiceInvoiceTermsDA.Delete(tc, oServiceInvoiceTerm, EnumDBOperation.Delete, nUserID, sServiceTermsIDs);
                #endregion

                #region Get Service Invoice
                reader = ServiceInvoiceDA.Get(tc, oServiceInvoice.ServiceInvoiceID);
                oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oServiceInvoice = new ServiceInvoice();
                    oServiceInvoice = CreateObject(oReader);
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
                    oServiceInvoice = new ServiceInvoice();
                    oServiceInvoice.ErrorMessage = e.Message.Split('!')[0];
                }
                #endregion
            }
            return oServiceInvoice;
        }

        public ServiceInvoice Revise(ServiceInvoice oServiceInvoice, Int64 nUserID)
        {
            TransactionContext tc = null;
            string sServiceInvoiceDetailIDs = "";
            List<ServiceInvoiceDetail> oServiceInvoiceDetails = new List<ServiceInvoiceDetail>();
            List<ServiceILaborChargeDetail> oServiceILaborChargeDetails = new List<ServiceILaborChargeDetail>();
            List<ServiceInvoiceTerms> oServiceInvoiceTerms = new List<ServiceInvoiceTerms>();
            ServiceInvoiceDetail oServiceInvoiceDetail = new ServiceInvoiceDetail();
            ServiceILaborChargeDetail oServiceILaborChargeDetail = new ServiceILaborChargeDetail();
            ServiceInvoiceTerms oServiceInvoiceTerm = new ServiceInvoiceTerms();
            oServiceInvoiceDetails = oServiceInvoice.ServiceInvoiceDetails;
            oServiceILaborChargeDetails = oServiceInvoice.ServiceILaborChargeDetails;
            oServiceInvoiceTerms = oServiceInvoice.ServiceInvoiceTermsList;
            try
            {
                tc = TransactionContext.Begin(true);
                IDataReader reader;
                oServiceInvoice.InvoiceStatus = EnumServiceInvoiceStatus.Initialize;
                AuthorizationRoleDA.CheckUserPermission(tc, nUserID, EnumModuleName.ServiceInvoice, EnumRoleOperationType.Add);
                reader = ServiceInvoiceDA.Revise(tc, oServiceInvoice, EnumDBOperation.Revise, nUserID);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oServiceInvoice = new ServiceInvoice();
                    oServiceInvoice = CreateObject(oReader);
                }
                reader.Close();
                #region Service Invoice Detail Part
                foreach (ServiceInvoiceDetail oItem in oServiceInvoiceDetails)
                {
                    IDataReader readerdetail;
                    oItem.ServiceInvoiceID = oServiceInvoice.ServiceInvoiceID;
                    if (oItem.ServiceInvoiceDetailID <= 0)
                    {
                        readerdetail = ServiceInvoiceDetailDA.InsertUpdate(tc, oItem, EnumDBOperation.Insert, nUserID, "");
                    }
                    else
                    {
                        readerdetail = ServiceInvoiceDetailDA.InsertUpdate(tc, oItem, EnumDBOperation.Update, nUserID, "");
                    }
                    NullHandler oReaderDetail = new NullHandler(readerdetail);
                    if (readerdetail.Read())
                    {
                        sServiceInvoiceDetailIDs = sServiceInvoiceDetailIDs + oReaderDetail.GetString("ServiceInvoiceDetailID") + ",";
                    }
                    readerdetail.Close();
                }
                if (sServiceInvoiceDetailIDs.Length > 0)
                {
                    sServiceInvoiceDetailIDs = sServiceInvoiceDetailIDs.Remove(sServiceInvoiceDetailIDs.Length - 1, 1);
                }
                oServiceInvoiceDetail = new ServiceInvoiceDetail();
                oServiceInvoiceDetail.ServiceInvoiceID = oServiceInvoice.ServiceInvoiceID;
                if (oServiceInvoice.ServiceInvoiceID > 0)
                    ServiceInvoiceDetailDA.Delete(tc, oServiceInvoiceDetail, EnumDBOperation.Delete, nUserID, sServiceInvoiceDetailIDs);
                #endregion

                string sServiceILCDetailIDs = "";
                #region Service Invoice Labor Charge Detail Part
                foreach (ServiceILaborChargeDetail oItem in oServiceILaborChargeDetails)
                {
                    IDataReader readerdetail;
                    oItem.ServiceInvoiceID = oServiceInvoice.ServiceInvoiceID;
                    if (oItem.ServiceILaborChargeDetailID <= 0)
                    {
                        readerdetail = ServiceILaborChargeDetailDA.InsertUpdate(tc, oItem, EnumDBOperation.Insert, nUserID, "");
                    }
                    else
                    {
                        readerdetail = ServiceILaborChargeDetailDA.InsertUpdate(tc, oItem, EnumDBOperation.Update, nUserID, "");
                    }
                    NullHandler oReaderDetail = new NullHandler(readerdetail);
                    if (readerdetail.Read())
                    {
                        sServiceILCDetailIDs = sServiceILCDetailIDs + oReaderDetail.GetString("ServiceILaborChargeDetailID") + ",";
                    }
                    readerdetail.Close();
                }
                if (sServiceILCDetailIDs.Length > 0)
                {
                    sServiceILCDetailIDs = sServiceILCDetailIDs.Remove(sServiceILCDetailIDs.Length - 1, 1);
                }
                oServiceILaborChargeDetail = new ServiceILaborChargeDetail();
                oServiceILaborChargeDetail.ServiceInvoiceID = oServiceInvoice.ServiceInvoiceID;
                if (oServiceInvoice.ServiceInvoiceID > 0)
                    ServiceILaborChargeDetailDA.Delete(tc, oServiceILaborChargeDetail, EnumDBOperation.Delete, nUserID, sServiceILCDetailIDs);
                #endregion

                string sServiceTermsIDs = "";
                #region Service Invoice Terms
                foreach (ServiceInvoiceTerms oItem in oServiceInvoiceTerms)
                {
                    IDataReader readerdetail;
                    oItem.ServiceInvoiceID = oServiceInvoice.ServiceInvoiceID;
                    if (oItem.ServiceInvoiceTermsID <= 0)
                    {
                        readerdetail = ServiceInvoiceTermsDA.InsertUpdate(tc, oItem, EnumDBOperation.Insert, nUserID, "");
                    }
                    else
                    {
                        readerdetail = ServiceInvoiceTermsDA.InsertUpdate(tc, oItem, EnumDBOperation.Update, nUserID, "");
                    }
                    NullHandler oReaderDetail = new NullHandler(readerdetail);
                    if (readerdetail.Read())
                    {
                        sServiceTermsIDs = sServiceTermsIDs + oReaderDetail.GetString("ServiceInvoiceTermsID") + ",";
                    }
                    readerdetail.Close();
                }
                if (sServiceTermsIDs.Length > 0)
                {
                    sServiceTermsIDs = sServiceTermsIDs.Remove(sServiceTermsIDs.Length - 1, 1);
                }
                oServiceInvoiceTerm = new ServiceInvoiceTerms();
                oServiceInvoiceTerm.ServiceInvoiceID = oServiceInvoice.ServiceInvoiceID;
                if (oServiceInvoice.ServiceInvoiceID > 0)
                    ServiceInvoiceTermsDA.Delete(tc, oServiceInvoiceTerm, EnumDBOperation.Delete, nUserID, sServiceTermsIDs);
                #endregion

                #region Get Service Invoice
                reader = ServiceInvoiceDA.Get(tc, oServiceInvoice.ServiceInvoiceID);
                oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oServiceInvoice = new ServiceInvoice();
                    oServiceInvoice = CreateObject(oReader);
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
                    oServiceInvoice = new ServiceInvoice();
                    oServiceInvoice.ErrorMessage = e.Message.Split('!')[0];
                }
                #endregion
            }
            return oServiceInvoice;
        }

        public List<ServiceInvoice> GetsServiceInvoiceLog(int id, Int64 nUserID)
        {
            List<ServiceInvoice> oServiceInvoices = null;
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();
                IDataReader reader = null;
                reader = ServiceInvoiceDA.GetsServiceInvoiceLog(id, tc);
                oServiceInvoices = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get ServiceInvoice History", e);
                #endregion
            }

            return oServiceInvoices;
        }
        public string Approve(ServiceInvoice oServiceInvoice, Int64 nUserID)
        {
            TransactionContext tc = null;
            string sMessage = "";
            try
            {
                tc = TransactionContext.Begin(true);
              
                if (oServiceInvoice.nRequest == 1)//Approve
                {
                    sMessage = "Approved";
                    AuthorizationRoleDA.CheckUserPermission(tc, nUserID, EnumModuleName.ServiceInvoice, EnumRoleOperationType.Approved);
                    DBTableReferenceDA.HasReference(tc, "ServiceInvoice", oServiceInvoice.ServiceInvoiceID);
                   
                    oServiceInvoice.InvoiceStatus = EnumServiceInvoiceStatus.Approved;
                    ServiceInvoiceDA.Approve(tc, oServiceInvoice, EnumDBOperation.Approval, nUserID);
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
				ServiceInvoice oServiceInvoice = new ServiceInvoice();
				oServiceInvoice.ServiceInvoiceID = id;
                AuthorizationRoleDA.CheckUserPermission(tc, nUserId, EnumModuleName.ServiceInvoice, EnumRoleOperationType.Delete);
				DBTableReferenceDA.HasReference(tc, "ServiceInvoice", id);
				ServiceInvoiceDA.Delete(tc, oServiceInvoice, EnumDBOperation.Delete, nUserId);
				tc.End();
			}
			catch (Exception e)
			{
				#region Handle Exceptionif (tc != null)
				tc.HandleError();
				return e.Message.Split('!')[0];
				#endregion
			}
			return Global.DeleteMessage;
		}

		public ServiceInvoice Get(int id, Int64 nUserId)
		{
			ServiceInvoice oServiceInvoice = new ServiceInvoice();
			TransactionContext tc = null;
			try
			{
				tc = TransactionContext.Begin();
				IDataReader reader = ServiceInvoiceDA.Get(tc, id);
				NullHandler oReader = new NullHandler(reader);
				if (reader.Read())
				{
				oServiceInvoice = CreateObject(oReader);
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
				throw new ServiceException("Failed to Get ServiceInvoice", e);
				#endregion
			}
			return oServiceInvoice;
		}
        public ServiceInvoice GetLog(int id, Int64 nUserId)
        {
            ServiceInvoice oServiceInvoice = new ServiceInvoice();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();
                IDataReader reader = ServiceInvoiceDA.GetLog(tc, id);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oServiceInvoice = CreateObject(oReader);
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
                throw new ServiceException("Failed to Get ServiceInvoice", e);
                #endregion
            }
            return oServiceInvoice;
        }

		public List<ServiceInvoice> Gets (string sSQL, Int64 nUserID)
		{
			List<ServiceInvoice> oServiceInvoices = new List<ServiceInvoice>();
			TransactionContext tc = null;
			try
			{
				tc = TransactionContext.Begin();
				IDataReader reader = null;
				reader = ServiceInvoiceDA.Gets(tc, sSQL);
				oServiceInvoices = CreateObjects(reader);
				reader.Close();
				tc.End();
			}
			catch (Exception e)
			{
				#region Handle Exception
				if (tc != null);
				tc.HandleError();
				ExceptionLog.Write(e);
				throw new ServiceException("Failed to Get ServiceInvoice", e);
				#endregion
			}
			return oServiceInvoices;
		}

        public List<ServiceInvoice> Gets(Int64 nUserID)
        {
            List<ServiceInvoice> oServiceInvoices = new List<ServiceInvoice>();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();
                IDataReader reader = null;
                reader = ServiceInvoiceDA.Gets(tc);
                oServiceInvoices = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null) ;
                tc.HandleError();
                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get ServiceInvoice", e);
                #endregion
            }
            return oServiceInvoices;
        }
		#endregion
	}

}
