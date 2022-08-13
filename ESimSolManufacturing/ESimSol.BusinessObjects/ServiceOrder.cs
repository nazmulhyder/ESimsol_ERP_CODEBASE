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
	#region ServiceOrder  
	public class ServiceOrder : BusinessObject
	{	
		public ServiceOrder()
		{
			ServiceOrderID = 0; 
			ServiceOrderNo = ""; 
			ServiceOrderType = EnumServiceOrderType.None; 
			VehicleRegistrationID = 0; 
			AdvisorID = 0; 
			CustomerID = 0; 
			ContactPersonID = 0; 
			KilometerReading = ""; 
			ServiceOrderDate = DateTime.Now; 
			IssueDate = DateTime.Now; 
			RcvDateTime = DateTime.Now; 
			DelDateTime = DateTime.Now; 
			Remarks = "";
            OrderStatus = EnumServiceOrderStatus.Initialize; 
			ApproveByID = 0; 
			ActualRcvDateTime = DateTime.Now; 
			ActualDelDateTime = DateTime.Now; 
			LastUpdateBy = 0; 
			LastUpdateDateTime = DateTime.Now; 
			MobilityService = ""; 
			IPNo = ""; 
			IPExpDate = ""; 
			SoldByDealer = ""; 
			NoShowStatus = ""; 
			ReasonOfVisit = ""; 
			ExtendedWarranty = ""; 
			ServicePlan = ""; 
			RSAPolicyNo = "";
            FuelStatus = EnumFuelStatus.None; 
			NoOfKeys = 0; 
			ENetAmount = 0; 
			ELCAmount = 0; 
			EPartsAmount = 0;
            ModeOfPayment = EnumPaymentMethod.None; 
			IsTaxesApplicable = true; 
			IsWindows = true; 
			IsWiperBlades = true; 
			IsLIghts = true; 
			IsExhaustSys = true; 
			IsUnderbody = true; 
			IsEngineComp = true; 
			IsWashing = true; 
			IsOilLevel = true; 
			IsCoolant = true; 
			IsWindWasher = true; 
			IsBreakes = true; 
			IsAxle = true; 
			IsMonograms = true; 
			IsPolishing = true; 
			IsOwnersManual = true; 
			IsScheManual = true; 
			IsNavManual = true; 
			IsWBook = true; 
			IsRefGuide = true; 
			IsSpareWheel = true; 
			IsToolKits = true; 
			IsFloorMats = true; 
			IsMudFlaps = true; 
			IsWarningT = true; 
			IsFirstAidKit = true; 
			NoOfCDs = 0; 
			IsOtherLoose = true; 
			ServiceOrderNoFull = ""; 
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
			AdvisorName = ""; 
			AttachDocumentID = 0; 
            ServiceOrderTypeInt  = 0; 
            OrderStatusInt = 0; 
            FuelStatusInt = 0;
            ModeOfPaymentInt = 0;
            CurrencyID = 0;
			ErrorMessage = "";
            RemainingFreeService = "";
		}

		#region Property
		public int ServiceOrderID { get; set; }
		public string ServiceOrderNo { get; set; }
		public EnumServiceOrderType ServiceOrderType { get; set; }
		public int VehicleRegistrationID { get; set; }
		public int AdvisorID { get; set; }
        public int CustomerID { get; set; }
        public int CurrencyID { get; set; }
		public int ContactPersonID { get; set; }
		public string KilometerReading { get; set; }
		public DateTime ServiceOrderDate { get; set; }
		public DateTime IssueDate { get; set; }
		public DateTime RcvDateTime { get; set; }
		public DateTime DelDateTime { get; set; }
        public string CurrencySymbol { get; set; }
        public string CurrencyName { get; set; }
        public string Remarks { get; set; }
        public EnumServiceOrderStatus OrderStatus { get; set; }
		public int ApproveByID { get; set; }
		public DateTime ActualRcvDateTime { get; set; }
		public DateTime ActualDelDateTime { get; set; }
		public int LastUpdateBy { get; set; }
		public DateTime LastUpdateDateTime { get; set; }
		public string MobilityService { get; set; }
		public string IPNo { get; set; }
		public string IPExpDate { get; set; }
		public string SoldByDealer { get; set; }
		public string NoShowStatus { get; set; }
		public string ReasonOfVisit { get; set; }
		public string ExtendedWarranty { get; set; }
		public string ServicePlan { get; set; }
		public string RSAPolicyNo { get; set; }
		public EnumFuelStatus FuelStatus { get; set; }
		public int NoOfKeys { get; set; }
		public double ENetAmount { get; set; }
		public double ELCAmount { get; set; }
		public double EPartsAmount { get; set; }
		public EnumPaymentMethod ModeOfPayment { get; set; }
		public bool IsTaxesApplicable { get; set; }
		public bool IsWindows { get; set; }
		public bool IsWiperBlades { get; set; }
		public bool IsLIghts { get; set; }
		public bool IsExhaustSys { get; set; }
		public bool IsUnderbody { get; set; }
		public bool IsEngineComp { get; set; }
		public bool IsWashing { get; set; }
		public bool IsOilLevel { get; set; }
		public bool IsCoolant { get; set; }
		public bool IsWindWasher { get; set; }
		public bool IsBreakes { get; set; }
		public bool IsAxle { get; set; }
		public bool IsMonograms { get; set; }
		public bool IsPolishing { get; set; }
		public bool IsOwnersManual { get; set; }
		public bool IsScheManual { get; set; }
		public bool IsNavManual { get; set; }
		public bool IsWBook { get; set; }
		public bool IsRefGuide { get; set; }
		public bool IsSpareWheel { get; set; }
		public bool IsToolKits { get; set; }
		public bool IsFloorMats { get; set; }
		public bool IsMudFlaps { get; set; }
		public bool IsWarningT { get; set; }
		public bool IsFirstAidKit { get; set; }
		public int NoOfCDs { get; set; }
		public bool IsOtherLoose { get; set; }
		public string ServiceOrderNoFull { get; set; }
		public string VehicleRegNo { get; set; }
		public string ChassisNo { get; set; }
		public string EngineNo { get; set; }
		public string VehicleModelNo { get; set; }
		public string VehicleTypeName { get; set; }
		public string ApproveByName { get; set; }
        public string CustomerName { get; set; }
        public string CustomerAddress { get; set; }
        public string CustomerPhone { get; set; }
        public string CustomerEmail { get; set; }
		public string ContactPerson { get; set; }
		public string ContactPersonPhone { get; set; }
		public string AdvisorName { get; set; }
		public int AttachDocumentID { get; set; }
        public int ServiceOrderTypeInt{ get; set; }
        public int OrderStatusInt{ get; set; }
        public int FuelStatusInt{ get; set; }
        public int ModeOfPaymentInt { get; set; }
        public string Params { get; set; }
		public string ErrorMessage { get; set; }
        public string RemainingFreeService { get; set; }
		#endregion 

		#region Derived Property
        public List<ServiceOrderDetail> ServiceOrderDetails { get; set; }
        public List<ServiceOrderDetail> RegularServiceOrderDetails { get; set; }
        public List<ServiceOrderDetail> ExtraServiceOrderDetails { get; set; }
        public System.Drawing.Image Img_Damages { get; set; }
        public string ServiceOrderDateSt
        {
            get
            {
                return this.ServiceOrderDate.ToString("dd MMM yyyy");
            }
        }
        public string DelDateTimeSt
        {
            get
            {
                if (DelDateTime == DateTime.MinValue)
                    return "--";
                return this.DelDateTime.ToString("dd MMM yyyy hh:mm tt");
            }
        }
        public string ActualDelDateTimeSt
        {
            get
            {

                if (ActualDelDateTime == DateTime.MinValue)
                    return "--";
                return this.ActualDelDateTime.ToString("dd MMM yyyy hh:mm tt");
            }
        }
        public string RcvDateTimeSt
        {
            get
            {
                if (RcvDateTime == DateTime.MinValue)
                    return "--";
                return this.RcvDateTime.ToString("dd MMM yyyy H:mm tt");
            }
        }
        public string ActualRcvDateTimeSt
        {
            get
            {
                if (ActualRcvDateTime == DateTime.MinValue)
                    return "--";
                return this.ActualRcvDateTime.ToString("dd MMM yyyy H:mm tt");
            }
        }
        public string IssueDateSt
        {
            get
            {
                return this.IssueDate.ToString("dd MMM yyyy");
            }
        }
        public string OrderStatusSt
        {
            get
            {
                return EnumObject.jGet(this.OrderStatus);
            }
        }
        public string ServiceOrderTypeSt
        {
            get
            {
                return EnumObject.jGet(this.ServiceOrderType);
            }
        }
        
		#endregion 

		#region Functions 
		public static List<ServiceOrder> Gets(long nUserID)
		{
			return ServiceOrder.Service.Gets(nUserID);
		}
		public static List<ServiceOrder> Gets(string sSQL, long nUserID)
		{
			return ServiceOrder.Service.Gets(sSQL,nUserID);
		}
		public ServiceOrder Get(int id, long nUserID)
		{
			return ServiceOrder.Service.Get(id,nUserID);
		}
		public ServiceOrder Save(long nUserID)
		{
			return ServiceOrder.Service.Save(this,nUserID);
		}
		public  string  Delete(int id, long nUserID)
		{
			return ServiceOrder.Service.Delete(id,nUserID);
		}
        public string UpdateStatus(ServiceOrder oServiceOrder, long nUserID)
        {
            return ServiceOrder.Service.UpdateStatus(oServiceOrder, nUserID);
        }
		#endregion

		#region ServiceFactory
		internal static IServiceOrderService Service
		{
			get { return (IServiceOrderService)Services.Factory.CreateService(typeof(IServiceOrderService)); }
		}
		#endregion

        public int nRequest { get; set; }

        public string CustomerVoice { get; set; }

        public string TechincalVoice { get; set; }
    }
	#endregion

	#region IServiceOrder interface
	public interface IServiceOrderService 
	{
		ServiceOrder Get(int id, Int64 nUserID); 
		List<ServiceOrder> Gets(Int64 nUserID);
		List<ServiceOrder> Gets( string sSQL, Int64 nUserID);
		string Delete(int id, Int64 nUserID);
 		ServiceOrder Save(ServiceOrder oServiceOrder, Int64 nUserID);
        string UpdateStatus(ServiceOrder oServiceOrder, Int64 nUserID);
	}
	#endregion
}
