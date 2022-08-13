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
	#region SalesQuotation  
	public class SalesQuotation : BusinessObject
	{	
		public SalesQuotation()
		{
			SalesQuotationID = 0;
            RefNo = "";
            FileNo = "";
            BUID = 0;
            QuotationDate = DateTime.Now;
            MarketingPerson  = 0;
            QuotationType = EnumQuotationType.Stock_Item;
            BuyerID  = 0;
            KommFileID  = 0;
            VehicleModelID  = 0;
            ExteriorColorID  = 0;
            InteriorColorID  = 0;
            VehicleChassisID  = 0;
            VehicleEngineID  = 0;
            Upholstery = "";
            UpholsteryCode = "";
            UpholsteryID = 0;
            Trim = "";
            TrimCode = "";
            TrimID = 0;
            WheelsID = 0;
            Wheels = "";
            WheelsCode = "";
            Remarks = "";
            FeatureSetupName = "";
            CurrencyID  = 0;
            DiscountAmount = 0; 
            DiscountPercent = 0;
            OptionTotal  = 0;
            UnitPrice  = 0;
            VatAmount  = 0;
            RegistrationFee  = 0;
            OTRAmount  = 0;
            Warranty = "";
            DeliveryDate = "";
            AdvancePayment  = 0;
            PaymentTerm = "";
            ValidityOfOffer = "";
            AfterSalesService = "";
            OfferValidity = "";
            OrderSpecifications = "";
            VehicleInspection = "";
            CancelOrChangeOrder = "";
            PaymentMode = "";
            DeliveryDescription = "";
            PriceFluctuationClause = "";
            CustomsClearance = "";
            Insurance = "";
            ForceMajeure = "";
            FuelQuality = "";
            SpecialInstruction = "";
            WarrantyTerms = "";
            PrintOTRAmount = 0;
            BuyerName = "";
            BuyerShortName = "";
            BuyerAddress = "";
            CurrencyName = "";
            Symbol = "";
            MarketingAccountName = "";
            UserID  = 0;
            KommNo = "";
            ModelNo = "";
            ModelCode = "";
            SeatingCapacity = "";
            ModelSessionName = "";
            ExteriorColorCode = "";
            ExteriorColorName = "";
            InteriorColorCode = "";
            InteriorColorName = "";
            EngineNo = "";
            ManufacturerName = "";
            MaxPowerOutput = "";
            MaximumTorque = "";
            EngineType = "";
            CountryOfOrigin = "";
            Transmission = "";
            YearOfManufacture = "";
            YearOfModel = "";
            ChassisNo = "";
            VehicleOrderID = 0;
            SalesQuotationImageID = 0;
            Acceptance = "";
            VehicleSpecification = "";
            SalesStatus = 0;
            SalesStatusRemarks="";
            SalesQuotationDetails = new List<SalesQuotationDetail>();
            SalesQuotationList = new List<SalesQuotation>();
			ErrorMessage = "";
            Capacity = "";
            Complementary = "";
            PartyWiseBankID = 0;
            OfferPrice = 0.0;
            NewOfferPrice = 0.0;
            DiscountPrice = 0.0;
            VATPercentage = 0;
            ModelShortName="";
			DisplacementCC="";
			Acceleration="";
			TopSpeed="";
            ExShowroomPriceBC = 0;
            SalesQuotationLogID = 0;
            ETAValue = 0;
            ETAType = EnumDisplayPart.None;
            IssueDate = DateTime.Now;
		}

		#region Property
        public int SalesQuotationID { get; set; }
        public int SalesQuotationLogID { get; set; }
        public int BuyerID { get; set; }
        public int CurrencyID { get; set; }
        public string ModelNo { get; set; }
        public string KommNo { get; set; }
        public int MarketingPerson { get; set; }
        public string RefNo { get; set; }
		public string Remarks { get; set; }
        public string InteriorColorCode { get; set; }
        public string InteriorColorName { get; set; }
        public string FileNo { get; set; }
        public int BUID { get; set; }
        public string ExteriorColorCode { get; set; }
        public string ExteriorColorName { get; set; }
        public int VehicleModelID { get; set; }
        public EnumQuotationType QuotationType { get; set; }
        public int QuotationTypeInInt { get; set; }
        public DateTime QuotationDate { get; set; }
        public string FeatureSetupName { get; set; }
        public int KommFileID { get; set; }
        public string ChassisNo { get; set; }
        public int SalesQuotationImageID { get; set; }
        public int VehicleOrderImageID { get; set; }

        public string EngineNo { get; set; }
        public Double UnitPrice { get; set; }
        public int ExteriorColorID { get; set; }
        public int InteriorColorID { get; set; }
        public int VehicleChassisID { get; set; }
        public int VehicleEngineID { get; set; }
        public string Upholstery { get; set; }
        public string UpholsteryCode { get; set; }
        public int UpholsteryID { get; set; }
        public string Trim { get; set; }
        public string TrimCode { get; set; }
        public int TrimID { get; set; }
        public string Wheels { get; set; }
        public string WheelsCode { get; set; }
        public int WheelsID { get; set; }
        public int VehicleOrderID { get; set; }
        public double OfferPrice { get; set; }
        public double NewOfferPrice { get; set; }
        public double DiscountPrice { get; set; }
        public Double OptionTotal { get; set; }
        public Double VatAmount { get; set; }
        public Double TDSAmount { get; set; }
        public Double RegistrationFee { get; set; }
        public Double OTRAmount { get; set; }
        public string Warranty { get; set; }
        public string DeliveryDate { get; set; }
        public Double AdvancePayment { get; set; }
        public Double DiscountAmount { get; set; }
        public Double DiscountPercent { get; set; }
        public string  PaymentTerm { get; set; }
        public string Acceptance { get; set; }
        public string VehicleSpecification { get; set; }
        public string ValidityOfOffer { get; set; }
        public string AfterSalesService { get; set; }
        public string OfferValidity { get; set; }
        public string OrderSpecifications { get; set; }
        public string VehicleInspection { get; set; }
        public string CancelOrChangeOrder { get; set; }
        public string PaymentMode { get; set; }
        public string DeliveryDescription { get; set; }
        public string PriceFluctuationClause { get; set; }
        public string CustomsClearance { get; set; }
        public string Insurance { get; set; }
        public string ForceMajeure { get; set; }
        public string FuelQuality { get; set; }
        public string SpecialInstruction { get; set; }
        public string WarrantyTerms { get; set; }
        public string BuyerName { get; set; }
        public string BuyerShortName { get; set; }
        public string BuyerAddress { get; set; }
        public string CurrencyName { get; set; }
        public string Symbol { get; set; }
        public string MarketingAccountName { get; set; }
        public int UserID { get; set; }
        public string  ManufacturerName { get; set; }
        public string MaxPowerOutput { get; set; }
        public string   MaximumTorque { get; set; }
        public string EngineType  { get; set; }
        public string CountryOfOrigin { get; set; }
        public string Transmission { get; set; }
        public EnumDriveType DriveType { get; set; }
        public string YearOfManufacture { get; set; }
        public string SeatingCapacity { get; set; }
        public string ModelSessionName { get; set; }
        public int SalesStatus { get; set; }
        public string SalesStatusRemarks { get; set; }
        public string Capacity { get; set; }
        public int PartyWiseBankID { get; set; }
        public string Params { get; set; }
        public string Complementary { get; set; }
        public double VATPercentage { get; set; }
        public double ExShowroomPriceBC { get; set; }
        public string ModelCode { get; set; }
        public string ModelShortName { get; set; }
        public string DisplacementCC { get; set; }
        public string Acceleration { get; set; }
        public string TopSpeed { get; set; }
        public double PrintOTRAmount { get; set; }
        public int ApproveBy { get; set; }
        public string ApproveByName { get; set; }
        public DateTime ApproveByDate { get; set; }
        public int VersionNo { get; set; }
        public string YearOfModel { get; set; }

        public string ErrorMessage { get; set; }
		#endregion 

		#region Derived Property
        public int ETAValue { get; set; }
        public EnumDisplayPart ETAType { get; set; }
        public DateTime IssueDate { get; set; }
        public int ProductNatureInInt { get; set; }
        public byte[] LargeImage { get; set; }
        public VehicleOrderImage VehicleOrderImage { get; set; }
        public string PossibleDateInString
        {
            get
            {
                if (this.ETAType == EnumDisplayPart.Day)
                {
                    return this.IssueDate.AddDays(this.ETAValue).ToString("dd MMM yyyy");
                }
                else if (this.ETAType == EnumDisplayPart.Week)
                {
                    return this.IssueDate.AddDays(this.ETAValue * 7).ToString("dd MMM yyyy");
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
        public string QuotationDateInString
        {
            get
            {
                if (QuotationDate == DateTime.MinValue) return DateTime.Now.ToString("dd MMM yyyy");
                else if (QuotationDate == DateTime.Parse("01 Jan 1900")) return DateTime.Now.ToString("dd MMM yyyy");
                return this.QuotationDate.ToString("dd MMM yyyy");
            }
        }
        public string QuotationTypeInString
        {
            get
            {
                 return EnumObject.jGet(this.QuotationType);
            }
        }
        public string DriveTypeInString
        {
            get
            {
                return EnumObject.jGet(this.DriveType);
            }
        }
        public Contractor Buyer { get; set; }
        public double AmountWithDiscount
        {
            get
            {
                return this.OptionTotal - ((this.OptionTotal * this.DiscountPercent) / 100);
            }
        }
        //public double TotalAmount
        //{
        //    get
        //    {
        //        return this.OptionTotal + this.UnitPrice;
        //    }
        //}
        public double TotalAmount
        {
            get
            {
                return this.NewOfferPrice + this.OptionTotal + this.VatAmount + this.RegistrationFee + this.TDSAmount;
            }
        }
        public string AdvancePaymentInWords
        {
            get
            {
                return Global.TakaWords(this.AdvancePayment);
            }
        }
        public string SalesStatusInString
        {
            get
            {
                return Enum.GetName(typeof(EnumSalesStatus), this.SalesStatus);
            }
        }
        public System.Drawing.Image Signature { get; set; }
        public List<SalesQuotationDetail> SalesQuotationDetails { get; set; }
        public List<SalesQuotation> SalesQuotationList { get; set; }
		#endregion 

		#region Functions 
		public static List<SalesQuotation> BUWiseGets(int buid, long nUserID)
		{
            return SalesQuotation.Service.BUWiseGets(buid, nUserID);
		}
		public static List<SalesQuotation> Gets(string sSQL, long nUserID)
		{
			return SalesQuotation.Service.Gets(sSQL,nUserID);
		}
        public SalesQuotation Get(int id, long nUserID)
        {
            return SalesQuotation.Service.Get(id, nUserID);
        }
        public SalesQuotation GetLog(int id, long nUserID)
        {
            return SalesQuotation.Service.Get(id, nUserID);
        }
		public SalesQuotation Save(long nUserID)
		{
			return SalesQuotation.Service.Save(this,nUserID);
		}

        public SalesQuotation Approve(long nUserID)
        {
            return SalesQuotation.Service.Approve(this, nUserID);
        }
        public SalesQuotation UndoApprove(long nUserID)
        {
            return SalesQuotation.Service.UndoApprove(this, nUserID);
        }
        public SalesQuotation Revise(long nUserID)
        {
            return SalesQuotation.Service.Revise(this, nUserID);
        }
        public SalesQuotation UpdateStatus(long nUserID)
        {
            return SalesQuotation.Service.UpdateStatus(this, nUserID);
        }
        public string UpdateBQ(long nUserID)
        {
            return SalesQuotation.Service.UpdateBQ(this, nUserID);
        }
		public  string  Delete(int id, long nUserID)
		{
			return SalesQuotation.Service.Delete(id,nUserID);
		}
		#endregion

		#region ServiceFactory
		internal static ISalesQuotationService Service
		{
			get { return (ISalesQuotationService)Services.Factory.CreateService(typeof(ISalesQuotationService)); }
		}
		#endregion

    }
	#endregion

	#region ISalesQuotation interface
	public interface ISalesQuotationService 
	{
        SalesQuotation Get(int id, Int64 nUserID);
        SalesQuotation GetLog(int id, Int64 nUserID);
        List<SalesQuotation> BUWiseGets(int buid, Int64 nUserID);
		List<SalesQuotation> Gets( string sSQL, Int64 nUserID);
		string Delete(int id, Int64 nUserID);
 		SalesQuotation Save(SalesQuotation oSalesQuotation, Int64 nUserID);
        SalesQuotation UpdateStatus(SalesQuotation oSalesQuotation, Int64 nUserID);
        SalesQuotation Approve(SalesQuotation oSalesQuotation, Int64 nUserID);
        SalesQuotation UndoApprove(SalesQuotation oSalesQuotation, Int64 nUserID);
        SalesQuotation Revise(SalesQuotation oSalesQuotation, Int64 nUserID);
        string UpdateBQ(SalesQuotation oSalesQuotation, Int64 nUserID);
	}
	#endregion
}
