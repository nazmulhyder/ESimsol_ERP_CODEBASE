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
	#region VehicleOrder  
	public class VehicleOrder : BusinessObject
	{	
		public VehicleOrder()
		{
			VehicleOrderID = 0; 
            ModelNo = "";
            RefNo = "";
            VehicleModelID = 0;
            InteriorColorID = 0;
            ExteriorColorID = 0;
            VehicleOrderImageID = 0;
            Remarks = "";
            InteriorColorCode = "";
            InteriorColorName = "";
            ExteriorColorCode = "";
            ExteriorColorName = "";

            Upholstery = "";
            UpholsteryCode = "";
            UpholsteryID = 0;
            Trim = "";
            TrimCode = "";
            TrimID = 0;
            Wheels = "";
            WheelsCode = "";
            WheelsID = 0;

            IssueDate = DateTime.Now;
            FeatureSetupName = "";
            ChassisID = 0;
            EngineID = 0;
            ETAValue = 0;
            ETAType = EnumDisplayPart.None;
            FileNo = "";
            ChassisNo = "";
            EngineNo = "";
            VATPercentage = 0;
            OfferPrice = 0;
            ExShowroomPriceBC = 0;
            CurrencyID = 0;
            OrderStatus = EnumVOStatus.None;
            VehicleOrderDetails = new List<VehicleOrderDetail>();
            VehicleOrderList = new List<VehicleOrder>();
			ErrorMessage = "";
		}

		#region Property
		public int VehicleOrderID { get; set; }
        public int VehicleModelID { get; set; }
		public string ModelNo { get; set; }
        public string RefNo { get; set; }
		public string Remarks { get; set; }
        public int ExteriorColorID { get; set; }
        public string InteriorColorCode { get; set; }
        public string InteriorColorName { get; set; }
        public string FileNo { get; set; }
        public int InteriorColorID { get; set; }
        public int BUID { get; set; }
        public string ExteriorColorCode { get; set; }
        public string ExteriorColorName { get; set; }

        public string Upholstery { get; set; }
        public string UpholsteryCode { get; set; }
        public int UpholsteryID { get; set; }
        public string Trim { get; set; }
        public string TrimCode { get; set; }
        public int TrimID { get; set; }
        public string Wheels { get; set; }
        public string WheelsCode { get; set; }
        public int WheelsID { get; set; }

        public int ChassisID { get; set; }
        public int EngineID { get; set; }
        public int ETAValue { get; set; }
        public EnumDisplayPart ETAType { get; set; }
        public int ETATypeInInt { get; set; }
        public DateTime IssueDate { get; set; }
        public string FeatureSetupName { get; set; }
        public EnumVOStatus OrderStatus { get; set; }
        public int OrderStatusInInt { get; set; }
        public string ChassisNo { get; set; }
        public string EngineNo { get; set; }
        public double VATPercentage { get; set; }
        public double OfferPrice { get; set; }
        public double ExShowroomPriceBC { get; set; } 
        public int CurrencyID { get; set; }
        public int VehicleOrderImageID { get; set; }
		public string ErrorMessage { get; set; }
		#endregion 

		#region Derived Property
        public int ProductNatureInInt { get; set; }
        public byte[] LargeImage { get; set; }
        public VehicleOrderImage VehicleOrderImage { get; set; }
        public string IssueDateInString
        {
            get
            {
                return this.IssueDate.ToString("dd MMM yyyy");
            }
        }
        public string ETATypeInString
        {
            get
            {
                 return EnumObject.jGet(this.ETAType);
            }
        }
        public string ETAValueWithTypeInString
        {
            get
            {
                return this.ETAValue+" "+EnumObject.jGet(this.ETAType);
            }
        }
        public string PossibleDateInString
        {
            get
            {
                if(this.ETAType==EnumDisplayPart.Day)
                {
                    return this.IssueDate.AddDays(this.ETAValue).ToString("dd MMM yyyy");
                }
                else if (this.ETAType == EnumDisplayPart.Week)
                {
                    return this.IssueDate.AddDays(this.ETAValue*7).ToString("dd MMM yyyy");
                }
                else if (this.ETAType == EnumDisplayPart.Month)
                {
                    return this.IssueDate.AddMonths(this.ETAValue).ToString("dd MMM yyyy");
                }
                else if (this.ETAType == EnumDisplayPart.Year)
                {
                    return this.IssueDate.AddYears(this.ETAValue).ToString("dd MMM yyyy");
                }
                else
                {
                    return "";
                }
                
            }
        }
        public string OrderStatusInString
        {
            get
            {
                return EnumObject.jGet(this.OrderStatus);
            }
        }
        public List<VehicleOrderDetail> VehicleOrderDetails { get; set; }
        public List<VehicleOrder> VehicleOrderList { get; set; }
		#endregion 

		#region Functions 
		public static List<VehicleOrder> BUWiseGets(int buid, long nUserID)
		{
            return VehicleOrder.Service.BUWiseGets(buid, nUserID);
		}
		public static List<VehicleOrder> Gets(string sSQL, long nUserID)
		{
			return VehicleOrder.Service.Gets(sSQL,nUserID);
		}
		public VehicleOrder Get(int id, long nUserID)
		{
			return VehicleOrder.Service.Get(id,nUserID);
		}
		public VehicleOrder Save(long nUserID)
		{
			return VehicleOrder.Service.Save(this,nUserID);
		}

        public VehicleOrder Approve(long nUserID)
        {
            return VehicleOrder.Service.Approve(this, nUserID);
        }
		public  string  Delete(int id, long nUserID)
		{
			return VehicleOrder.Service.Delete(id,nUserID);
		}
		#endregion

		#region ServiceFactory
		internal static IVehicleOrderService Service
		{
			get { return (IVehicleOrderService)Services.Factory.CreateService(typeof(IVehicleOrderService)); }
		}
		#endregion
	}
	#endregion

	#region IVehicleOrder interface
	public interface IVehicleOrderService 
	{
		VehicleOrder Get(int id, Int64 nUserID);
        List<VehicleOrder> BUWiseGets(int buid, Int64 nUserID);
		List<VehicleOrder> Gets( string sSQL, Int64 nUserID);
		string Delete(int id, Int64 nUserID);
 		VehicleOrder Save(VehicleOrder oVehicleOrder, Int64 nUserID);
        VehicleOrder Approve(VehicleOrder oVehicleOrder, Int64 nUserID);
        
	}
	#endregion
}
