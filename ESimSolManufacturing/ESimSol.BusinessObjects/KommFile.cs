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
	#region KommFile  
	public class KommFile : BusinessObject
	{	
		public KommFile()
		{
			KommFileID = 0; 
            ModelNo = "";
            VehicleOrderID = 0;
            KommFileImageID = 0;
            CurrencyID = 0;
            CurrencySymbol = "";
            CurrencyName = "";
            Remarks = "";
            VehicleModelID = 0;
            InteriorColorCode = "";
            InteriorColorID = 0;
            InteriorColorName = "";
            ExteriorColorCode = "";
            ExteriorColorID = 0;
            ExteriorColorName = "";
            Upholstery = "";
            UpholsteryCode = "";
            UpholsteryID = 0;
            Trim = "";
            TrimCode = "";
            TrimID = 0;
            WheelsID = 0;
            Wheels = "";
            WheelsCode = "";
            IssueDate = DateTime.Now;
            FeatureSetupName = "";
            ETAValue = 0;
            UnitPrice = 0;
            VatInPercent = 0;
            RegistrationFeePercent = 0;
            ETAType = EnumDisplayPart.None;
            FileNo = "";
            KommNo = "";
            ChassisNo = "";
            ChassisID = 0;
            EngineNo = "";
            EngineID = 0;
            SalesStatus = 0;
            VATPercentage = 0;
            ExShowroomPriceBC = 0;
            SalesQuotationCount = 0;
            KommFileStatus = EnumKommFileStatus.None;
            OrderStatus = EnumVOStatus.None;
            KommFileDetails = new List<KommFileDetail>();
            KommFileList = new List<KommFile>();
			ErrorMessage = "";
		}

		#region Property
        public int KommFileID { get; set; }
        public int VehicleOrderID { get; set; }
        public int KommFileImageID { get; set; }
        public int CurrencyID { get; set; }
        public string CurrencySymbol { get; set; }
        public string CurrencyName { get; set; }
        public string ModelNo { get; set; }
        public int VehicleModelID { get; set; }
        public string YearOfManufacture { get; set; }
        public string MaximumTorque { get; set; }
        public string MaxPowerOutput { get; set; }
        public string Transmission { get; set; }
        public string EngineType { get; set; }
        public string CountryOfOrigin { get; set; }
        public string KommNo { get; set; }
        public string VehicleOrderNo { get; set; }
        public string RefNo { get; set; }
        public string Remarks { get; set; }
        public string InteriorColorCode { get; set; }
        public int InteriorColorID { get; set; }
        public string InteriorColorName { get; set; }
        public string FileNo { get; set; }
        public int BUID { get; set; }
        public string ExteriorColorCode { get; set; }
        public int ExteriorColorID { get; set; }
        public string ExteriorColorName { get; set; }
        public int ETAValue { get; set; }
        public EnumDisplayPart ETAType { get; set; }
        public int ETATypeInInt { get; set; }
        public DateTime IssueDate { get; set; }
        public string FeatureSetupName { get; set; }
        public EnumVOStatus OrderStatus { get; set; }
        public int OrderStatusInInt { get; set; }
        public string ChassisNo { get; set; }
        public int ChassisID { get; set; }
        public string EngineNo { get; set; }
        public int EngineID { get; set; }
        public Double UnitPrice { get; set; }
        public Double VatInPercent { get; set; }
        public string ErrorMessage { get; set; }
        public Double RegistrationFeePercent { get; set; }
        public double VATPercentage { get; set; }
        public double ExShowroomPriceBC { get; set; }
        public string Upholstery { get; set; }
        public string  UpholsteryCode { get; set; }
        public int UpholsteryID { get; set; }
        public string Trim { get; set; }
        public string TrimCode{ get; set; }
        public int TrimID { get; set; }
        public string Wheels{ get; set; }
        public string WheelsCode { get; set; }
        public int WheelsID { get; set; }
        public int SalesStatus { get; set; }
        public EnumKommFileStatus KommFileStatus { get; set; }
        public int SalesQuotationCount { get; set; }
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
        public string SalesStatusInString
        {
            get
            {
                return Enum.GetName(typeof(EnumSalesStatus), this.SalesStatus);
            }
        }
        public string MKTStatus
        {
            get
            {
                return this.SalesQuotationCount+" Quotation";
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
        public int KommFileStatusInInt
        {
            get
            {
                return (int)(this.KommFileStatus);
            }
        }
        public string KommFileStatusInString
        {
            get
            {
                return EnumObject.jGet(this.KommFileStatus);
            }
        }
        public string OrderStatusInString
        {
            get
            {
                return EnumObject.jGet(this.OrderStatus);
            }
        }
        public List<KommFileDetail> KommFileDetails { get; set; }
        public List<KommFile> KommFileList { get; set; }
		#endregion 

		#region Functions 
		public static List<KommFile> BUWiseGets(int buid, long nUserID)
		{
            return KommFile.Service.BUWiseGets(buid, nUserID);
		}
		public static List<KommFile> Gets(string sSQL, long nUserID)
		{
			return KommFile.Service.Gets(sSQL,nUserID);
		}
		public KommFile Get(int id, long nUserID)
		{
			return KommFile.Service.Get(id,nUserID);
		}
		public KommFile Save(long nUserID)
		{
			return KommFile.Service.Save(this,nUserID);
		}
        public KommFile UpdateStatus(long nUserID)
        {
            return KommFile.Service.UpdateStatus(this, nUserID);
        }
        public KommFile Approve(long nUserID)
        {
            return KommFile.Service.Approve(this, nUserID);
        }
		public  string  Delete(int id, long nUserID)
		{
			return KommFile.Service.Delete(id,nUserID);
		}
		#endregion

		#region ServiceFactory
		internal static IKommFileService Service
		{
			get { return (IKommFileService)Services.Factory.CreateService(typeof(IKommFileService)); }
		}
		#endregion

        public string Params { get; set; }
    }
	#endregion

	#region IKommFile interface
	public interface IKommFileService 
	{
		KommFile Get(int id, Int64 nUserID);
        List<KommFile> BUWiseGets(int buid, Int64 nUserID);
		List<KommFile> Gets( string sSQL, Int64 nUserID);
		string Delete(int id, Int64 nUserID);
 		KommFile Save(KommFile oKommFile, Int64 nUserID);
        KommFile Approve(KommFile oKommFile, Int64 nUserID);
        KommFile UpdateStatus(KommFile oKommFile, Int64 nUserID);
	}
	#endregion
}
