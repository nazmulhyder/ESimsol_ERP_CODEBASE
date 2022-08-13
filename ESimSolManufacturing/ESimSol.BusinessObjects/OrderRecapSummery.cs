using System;
using System.IO;
using ICS.Core;
using ESimSol.BusinessObjects;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Collections.Generic;
using ICS.Core.Framework;
using ICS.Core.Utility;

namespace ESimSol.BusinessObjects
{

    #region OrderRecapSummery
    
    public class OrderRecapSummery : BusinessObject
    {
        public OrderRecapSummery()
        {
            OrderRecapID = 0;
            OrderRecapNo = "";
            TechnicalSheetID = 0;
            PreparedBy = "";
            StyleNo = "";
            FactoryName = "";
            ProductID = 0;
            ProductName = "";
            Fabrication = "";
            Count = "";
            GG = "";
            Weight = "N/A";
            Button = "N/A";
            Zipper = "N/A";
            Print = "N/A";
            Embrodery = "N/A";
            Badge = "N/A";
            Studs = "N/A";
            FabricAttachment = "N/A";
            Quantity = 0;
            UnitName = "";
            OrderDate = DateTime.Now;
            ShipmentDate = DateTime.Now;
            ShipmentMode = EnumTransportType.None;
            LCReceivedDate = DateTime.Today;
            Price = 0;
            TotalAmount = 0;
            ColorName = "";
            Label = "";
            HangTag = "";
            SizeRatio = "";
            RowNumber = 0;
            MaxRowNumber = 0;
            BuyerID = 0;
            BuyerName = "";
            BrandName = "";
            ErrorMessage = "";
            ApprovedByName = "";
            Incoterms = EnumIncoterms.None;
            StyleCoverImage = null;
            SLNo = "";
            SessionName = "";
            FabricID = 0;
            YarnContent = "";
            FabricationID = 0;
            CurrencyID = 0;
            CurrencyName = "";
            CurrencySymbol = "";
            StyleDescription = "";
            CMValue = 0;
            KnittingPattern = 0;
            KnittingPatternName = "";
            MerchandiserName = "";
            BusinessSessions = new List<BusinessSession>();
            FabricOptionA = "";
            FabricOptionB = "";
            FabricOptionC = "";
            FabricOptionD = "";
            SpecialFinish = "";
            PaymentTerm = "";
            Note = "";
            LocalYarnSupplierID = 0;
            ImportYarnSupplierID = 0;
            LocalYarnSupplierName = "";
            ImportYarnSupplierName = "";
            CommercialRemarks = "";
            Brands = new List<Brand>();
            Factorys = new List<Contractor>();
        }

        #region Properties
         
        public int OrderRecapID { get; set; }
         
        public string OrderRecapNo { get; set; }
         
        public string SLNo { get; set; }
         
        public string SessionName { get; set; }
         
        public int TechnicalSheetID { get; set; }
         
        public DateTime OrderDate { get; set; }
         
        public EnumIncoterms Incoterms { get; set; }
         
        public string PreparedBy { get; set; }
         
        public int FabricationID { get; set; }
         
        public string Fabrication { get; set; }
         
        public string ApprovedByName { get; set; }
         
        public DateTime ShipmentDate { get; set; }
         
        public EnumTransportType ShipmentMode { get; set; }
         
        public DateTime LCReceivedDate { get; set; }
        public string BrandName { get; set; }
        public string GG { get; set; }
         
        public string PaymentTerm { get; set; }
         
        public string ColorName { get; set; }
         
        public string FabricAttachment { get; set; }
         
        public string Studs { get; set; }
         
        public string Badge { get; set; }
         
        public string Embrodery { get; set; }
         
        public string Label { get; set; }
         
        public string HangTag { get; set; }
         
        public string SizeRatio { get; set; }
         
        public string FactoryName { get; set; }
         
        public int FabricID { get; set; }

         
        public string YarnContent { get; set; }
         
        public double Price { get; set; }
         
        public double Quantity { get; set; }
         
        public double TotalAmount { get; set; }
         
        public string UnitName { get; set; }
         
        public int ProductID { get; set; }
         
        public string ProductName { get; set; }
         
        public string Count { get; set; }
         
        public string Weight { get; set; }
         
        public byte[] RecognizeImage { get; set; }
         
        public string StyleNo { get; set; }
         
        public int BuyerID { get; set; }
         
        public string BuyerName { get; set; }
         
        public string Button { get; set; }
         
        public string Zipper { get; set; }
         
        public string ErrorMessage { get; set; }
         
        public string Print { get; set; }
         
        public string StyleDescription { get; set; }
         
        public int KnittingPattern { get; set; }
        public string KnittingPatternName { get; set; }
         
        public string MerchandiserName { get; set; }
         
        public string Note { get; set; }
         
        public int RowNumber { get; set; }// it use for paginition
         
        public int MaxRowNumber { get; set; }// it use for paginition
         
        public int CurrencyID { get; set; }
         
        public string CurrencyName { get; set; }
         
        public string CurrencySymbol { get; set; }

         
        public double CMValue { get; set; }

         
        public string SpecialFinish { get; set; }
         
        public int LocalYarnSupplierID { get; set; }
         
        public int ImportYarnSupplierID { get; set; }
         
        public string LocalYarnSupplierName { get; set; }
         
        public string ImportYarnSupplierName { get; set; }
         
        public string CommercialRemarks { get; set; }

        #endregion

        #region Derived Property
        public System.Drawing.Image StyleCoverImage { get; set; }        
        public bool IsRateView { get; set; }
        public bool IsCMValue { get; set; }
        public string OrderDateInString
        {
            get
            {
                return this.OrderDate.ToString("dd MMM yyyy");
            }
        }
        public string ShipmentDateInString
        {
            get
            {
                return this.ShipmentDate.ToString("dd MMM yyyy");
            }
        }

        public string LCReceivedDateInString
        {
            get
            {
                if (LCReceivedDate == DateTime.MinValue)
                {
                    return "";
                }
                else
                {
                    return this.LCReceivedDate.ToString("dd MMM yyyy");
                }
            }
        }

        public string TotalAmountInString
        {
            get
            {
                return Global.MillionFormat(this.TotalAmount);
            }
        }

        public string CMValueInStringwithSlash
        {
            get
            {
                return " /$" + this.CMValue.ToString() + "(CM)";
            }
        }


        public string CMValueInStringwithOutSlash
        {
            get
            {
                return " $" + this.CMValue.ToString() + "(CM)";
            }
        }

        public string IncotermsInString
        {
            get
            {
                return this.Incoterms.ToString();
            }
        }

        public string QuantityInString
        {
            get
            {
                return Global.MillionFormat(this.Quantity, 0);
            }
        }

        public string PriceInString
        {
            get
            {
                return Global.MillionFormat(this.Price);
            }
        }
        public List<SizeCategory> SampleSizes { get; set; }
        public List<OrderRecapSummery> OrderRecapSummeryList { get; set; }
        public List<Contractor> ContractorList { get; set; }
        public List<ProductCategory> ProductCategorys { get; set; }
        public List<BusinessSession> BusinessSessions { get; set; }
        public List<OrderRecapDetail> OrderRecapDetails { get; set; }
        public List<Brand> Brands { get; set; }
        public List<Contractor> Factorys { get; set; }
        public Company Company { get; set; }
        public string ImageUrl { get; set; }
        public int DevelopmentStatusInInt { get; set; }
        public string FabricOptionA { get; set; }
        public string FabricOptionB { get; set; }
        public string FabricOptionC { get; set; }
        public string FabricOptionD { get; set; }



        #endregion

        #region Functions

        public static List<OrderRecapSummery> GetsRecapWithOrderRecapSummerys(int nOT, int nStartRow, int nEndRow, string SQL, string sOrderRecapSummeryIDs, bool bIsPrint, int nSortBy, long nUserID)
        {
            return OrderRecapSummery.Service.GetsRecapWithOrderRecapSummerys(nOT, nStartRow, nEndRow, SQL, sOrderRecapSummeryIDs, bIsPrint, nSortBy, nUserID);
        }
        #endregion

        #region NonDB Functions
        public static string IDInString(List<OrderRecapSummery> oOrderRecapSummerys)
        {
            string sReturn = "";
            foreach (OrderRecapSummery oItem in oOrderRecapSummerys)
            {
                sReturn = sReturn + oItem.OrderRecapID.ToString() + ",";
            }
            if (sReturn == "") return "";
            sReturn = sReturn.Remove(sReturn.Length - 1, 1);
            return sReturn;
        }
        #endregion

        #region ServiceFactory
        internal static IOrderRecapSummeryService Service
        {
            get { return (IOrderRecapSummeryService)Services.Factory.CreateService(typeof(IOrderRecapSummeryService)); }
        }

        #endregion
    }
    #endregion

    #region IOrderRecapSummery interface
     
    public interface IOrderRecapSummeryService
    {

        List<OrderRecapSummery> GetsRecapWithOrderRecapSummerys(int nOT, int nStartRow, int nEndRow, string SQL, string sOrderRecapSummeryIDs, bool bIsPrint, int nSortBy, Int64 nUserID);
    }
    #endregion

}
