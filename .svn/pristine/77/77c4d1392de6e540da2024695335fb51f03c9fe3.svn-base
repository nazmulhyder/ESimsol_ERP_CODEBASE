using System;
using ICS.Core.Framework;
using ICS.Core.Utility;
using ICS.Core;
using ESimSol.BusinessObjects;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization;
using System.ServiceModel;
using ESimSol.BusinessObjects.ReportingObject;

namespace ESimSol.BusinessObjects
{
	#region ServiceInvoice  
	public class ServiceInvoice : BusinessObject
	{	
		public ServiceInvoice()
		{
			ServiceInvoiceID=0;
            ServiceInvoiceNo= "";
            InvoiceType = EnumInvoiceType.ServiceInvoice;
            InvoiceTypeInt = (int)EnumInvoiceType.ServiceInvoice;
            ServiceInvoiceType = EnumServiceInvoiceType.None;
            VehicleRegistrationID=0;
            WorkOrderByID=0;
            CustomerID=0;
            ContactPersonID=0;
            ServiceOrderID = 0;
            KilometerReading= "";
            ServiceInvoiceDate=DateTime.Now;
            Remarks = "";
            CustomerRemarks = "";
            OfficeRemarks = "";
            InvoiceStatus = EnumServiceInvoiceStatus.None;
            ApproveByID=0;
            VehicleRegNo = "";
            ChassisNo = "";
            EngineNo = "";
            VehicleModelNo = "";
            VehicleTypeName = "";
            ApproveByName = "";
            CustomerName = "";
            CustomerPhone = "";
            ContactPerson = "";
            ContactPersonPhone = "";
            ServiceInvoiceDetails = new List<ServiceInvoiceDetail>();
            ServiceILaborChargeDetails = new List<ServiceILaborChargeDetail>();
            ServiceInvoiceTermsList = new List<ServiceInvoiceTerms>();
            PartsCharge_Vat = 0;
            PartsCharge_Net =0;
            PartsCharge_VatAmount = 0;
            LaborCharge_VatAmount = 0;
            PrepaireBy = 0;
            ServiceScheduleID = 0;
			ScheduleDate = DateTime.MinValue;
            ServiceDoneDate = DateTime.MinValue;     
            PrepaireByName = "";
			ErrorMessage = "";
		}

		#region Property
        public int ServiceInvoiceID { get; set; }
        public EnumServiceInvoiceType ServiceInvoiceType { get; set; }
        public EnumInvoiceType InvoiceType { get; set; }
        public int InvoiceTypeInt { get; set; }
        public int VehicleRegistrationID { get; set; }
        public int WorkOrderByID { get; set; }
        public int CustomerID { get; set; }
        public int ContactPersonID { get; set; }
        public int ServiceOrderID { get; set; }
        public int CurrencyID { get; set; }
        public string CurrencyName { get; set; }
        public EnumServiceInvoiceStatus InvoiceStatus { get; set; }
        public DateTime ServiceInvoiceDate { get; set; }
        public int ApproveByID { get; set; }
        public string KilometerReading { get; set; }
        public string ServiceInvoiceNo { get; set; }
        public string ServiceOrderNo { get; set; }
        public string ContactPerson { get; set; }
        public string WorkOrderByName { get; set; }
        public string CustomerName { get; set; }
        public string ApproveByName { get; set; }
        public string VehicleTypeName { get; set; }
        public string EngineNo { get; set; }
        public string ChassisNo { get; set; }
        public string VehicleModelNo { get; set; }
        public string VehicleRegNo { get; set; }
        public string Remarks { get; set; }
        public string CustomerRemarks { get; set; }
        public string OfficeRemarks { get; set; }
        public string CustomerPhone { get; set; }
        public string ContactPersonPhone { get; set; }
        public double DiscountAmount_Parts { get; set; }
        public double PartsCharge_Vat { get; set; }
        public double PartsCharge_Net { get; set; }
        public int ServiceScheduleID { get; set; }
        public DateTime ScheduleDate { get; set; }
        public DateTime ServiceDoneDate { get; set; }
        public double NetAmount_Parts { get; set; }
        public double NetAmount_Payble { get; set; }
        public double LaborCharge_Total { get; set; }
        public double LaborCharge_Discount { get; set; }
        public double LaborCharge_Vat { get; set; }
        public double LaborCharge_Net { get; set; }
        public List<ServiceInvoiceDetail> ServiceInvoiceDetails { get; set; }
        public List<ServiceInvoiceTerms> ServiceInvoiceTermsList { get; set; }
        public List<ServiceILaborChargeDetail> ServiceILaborChargeDetails { get; set; }
        public int nRequest { get; set; }
        public string Params { get; set; }
        public System.Drawing.Image Img_Damages { get; set; }
        public int PrepaireBy { get; set; }
        public string PrepaireByName { get; set; }
        public string ErrorMessage { get; set; }
		#endregion 

		#region Derived Property
        public double PartsCharge_VatAmount { get; set; }
        public double LaborCharge_VatAmount { get; set; }
        public int ServiceInvoiceLogID { get; set; }
        public int InvoiceStatusInt { get { return (int)InvoiceStatus; } }
        public int ServiceInvoiceTypeInt { get { return (int)ServiceInvoiceType; } }
        public string RequisitionNo { get; set; }
        public string ServiceInvoiceDateSt
        {
            get
            {
                return this.ServiceInvoiceDate.ToString("dd MMM yyyy");
            }
        }
        public string ScheduleDateSt
        {
            get
            {
                return this.ScheduleDate==DateTime.MinValue?"": this.ScheduleDate.ToString("dd MMM yyyy");
            }
        }
        public string ServiceDoneDateSt
        {
            get
            {
                return this.ServiceDoneDate == DateTime.MinValue ? "" : this.ServiceDoneDate.ToString("dd MMM yyyy");
            }
        }
        public string InvoiceStatusSt
        {
            get
            {
                return EnumObject.jGet(this.InvoiceStatus);
            }
        }
        public string ServiceInvoiceTypeSt
        {
            get
            {
                return EnumObject.jGet(this.ServiceInvoiceType);
            }
        }
         
            
		#endregion 

		#region Functions 
		public static List<ServiceInvoice> Gets(long nUserID)
		{
			return ServiceInvoice.Service.Gets(nUserID);
		}
        public static List<ServiceInvoice> Gets(string sSQL, long nUserID)
        {
            return ServiceInvoice.Service.Gets(sSQL, nUserID);
        }
		public ServiceInvoice Get(int id, long nUserID)
		{
			return ServiceInvoice.Service.Get(id,nUserID);
		}
        public ServiceInvoice GetLog(int id, long nUserID)
        {
            return ServiceInvoice.Service.GetLog(id, nUserID);
        }
		public ServiceInvoice Save(long nUserID)
		{
			return ServiceInvoice.Service.Save(this,nUserID);
		}
        public ServiceInvoice Revise(long nUserID)
        {
            return ServiceInvoice.Service.Revise(this, nUserID);
        }
        public static List<ServiceInvoice> GetsServiceInvoiceLog(int id, long nUserID) // id is PI ID
        {
            return ServiceInvoice.Service.GetsServiceInvoiceLog(id, nUserID);
        }
		public  string  Delete(int id, long nUserID)
		{
			return ServiceInvoice.Service.Delete(id,nUserID);
		}
        public string Approve(ServiceInvoice oServiceInvoice, long nUserID)
        {
            return ServiceInvoice.Service.Approve(oServiceInvoice, nUserID);
        }
		#endregion

		#region ServiceFactory
		internal static IServiceInvoiceService Service
		{
			get { return (IServiceInvoiceService)Services.Factory.CreateService(typeof(IServiceInvoiceService)); }
		}
		#endregion

    }
	#endregion

	#region IServiceInvoice interface
	public interface IServiceInvoiceService 
	{
		ServiceInvoice Get(int id, Int64 nUserID);
        ServiceInvoice GetLog(int id, Int64 nUserID);
		List<ServiceInvoice> Gets( string sSQL, Int64 nUserID);
        List<ServiceInvoice> Gets(Int64 nUserID);
		string Delete(int id, Int64 nUserID);
 		ServiceInvoice Save(ServiceInvoice oServiceInvoice, Int64 nUserID);
        ServiceInvoice Revise(ServiceInvoice oServiceInvoice, Int64 nUserID);
        List<ServiceInvoice> GetsServiceInvoiceLog(int id, Int64 nUserID);
        string Approve(ServiceInvoice oServiceInvoice, Int64 nUserID);
        
	}
	#endregion
}
