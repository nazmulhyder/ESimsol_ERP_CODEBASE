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
    #region DevelopmentRecapSummary
    
    public class DevelopmentRecapSummary : BusinessObject
    {
        public DevelopmentRecapSummary()
        {
            DevelopmentRecapID = 0;
            DevelopmentRecapNo = "";
            SessionName = "";
            TechnicalSheetID = 0;
            DevelopmentStatus = EnumDevelopmentStatus.Initialize;
            InquiryReceivedDate = DateTime.Today;
            Fabrication = "";
            SampleQty = 0;
            UnitPrice=0;
            CurrencySymbol = "";
            SampleSizeID = 0;
            SampleReceivedDate = DateTime.Today;
            SampleSendingDate = DateTime.Today;
            SendingDeadLine = DateTime.Today;
            AwbNo = "";
            GG = "";
            Weight = "";
			SpecialFinish ="";
            Count = "";
            Remarks = "";
            StyleNo = "";
            BuyerID = 0;
            SampleSize = "";
            BuyerName = "";
            BrandName = "";
            PrepareBy = "";
            ApprovedByName = "";
            CollectionName = "";
            DevelopmentType = 0;
            DevelopmentTypeName = "";
            StyleDescription = "";
            KnittingPattern = 0;
            ColorRange = "";
            MerchandiserName = "";
            FactoryName = "";
            RowNumber = 0;
            MaxRowNumber = 0;
            DevelopmentRecapSummarys = new List<DevelopmentRecapSummary>();
            DevelopmentRecapDetails = new List<DevelopmentRecapDetail>();
            ContractorList = new List<Contractor>();
            ProductCategorys = new List<ProductCategory>();
            BusinessSessions = new List<BusinessSession>();
            Brands = new List<Brand>();
            FabricOptionA = "";
            FabricOptionB = "";
            FabricOptionC = "";
            FabricOptionD = "";
        }

        #region Properties
         
        public int DevelopmentRecapID { get; set; }
         
        public string DevelopmentRecapNo { get; set; }
         
        public string SessionName { get; set; }
        public string BrandName { get; set; }

        public int TechnicalSheetID { get; set; }
         
        public EnumDevelopmentStatus DevelopmentStatus { get; set; }
         
        public DateTime InquiryReceivedDate { get; set; }
         
        public string Fabrication { get; set; }
        
         
        public string GG { get; set; }
         
        public string SpecialFinish{ get; set; }
         
        public string Count { get; set; }
         
        public string Weight { get; set; }
         
        public double SampleQty { get; set; }
        public double UnitPrice { get; set; }
        public string CurrencySymbol { get; set; }         
        public int SampleSizeID { get; set; }
         
        public DateTime SampleReceivedDate { get; set; }
         
        public DateTime SampleSendingDate { get; set; }
         
        public DateTime SendingDeadLine { get; set; }
         
        public string AwbNo { get; set; }
        
         
        public string Remarks { get; set; }
         
        public string StyleNo { get; set; }
         
        public int BuyerID { get; set; }
         
        public string SampleSize { get; set; }
         
        public string BuyerName { get; set; }
         
        public string PrepareBy { get; set; }
         
        public string ApprovedByName { get; set; }
         
        public string CollectionName { get; set; }
        
        
         
        public string StyleDescription { get; set; }
         
        public int KnittingPattern { get; set; }
         
        public string ColorRange { get; set; }
         
        public string MerchandiserName { get; set; }
         
        public string FactoryName { get; set; }
         
        public int RowNumber { get; set; }
         
        public int MaxRowNumber { get; set; }
        public string KnittingPatternName { get; set; }
        public int DevelopmentType { get; set; }
        public string DevelopmentTypeName { get; set; }
        #endregion

        #region Derived Property
        public System.Drawing.Image StyleCoverImage { get; set; }
        public List<Brand> Brands { get; set; }
        public List<DevelopmentRecapSummary> DevelopmentRecapSummarys { get; set; }
        public List<DevelopmentRecapDetail> DevelopmentRecapDetails { get; set; }
        public List<DevelopmentYarnOption> DevelopmentYarnOptions { get; set; }
        public List<Contractor> ContractorList { get; set; }
        public List<ProductCategory> ProductCategorys { get; set; }
        public List<BusinessSession> BusinessSessions { get; set; }
        public string FabricOptionA { get; set; }
        public string FabricOptionB { get; set; }
        public string FabricOptionC { get; set; }
        public string FabricOptionD { get; set; }
        public string DevelopmentStatusInString
        {
            get
            {
                return this.DevelopmentStatus.ToString();
            }
        }
        public string UnitPriceSt
        {
            get
            {
                return this.CurrencySymbol + " "+Global.MillionFormat(this.UnitPrice);
            }
        }
        public string InquiryReceivedDateInString
        {
            get
            {
                if (this.InquiryReceivedDate == DateTime.MinValue)
                {
                    return "";
                }
                else
                {
                    return this.InquiryReceivedDate.ToString("dd MMM yyyy");
                }
            }
        }
        public string SampleReceivedDateInString
        {
            get
            {
                if (this.SampleReceivedDate == DateTime.MinValue)
                {
                    return "";
                }
                else
                {
                    return this.SampleReceivedDate.ToString("dd MMM yyyy");
                }
            }
        }
        public string SampleSendingDateInString
        {
            get
            {
                if (this.SampleSendingDate == DateTime.MinValue)
                {
                    return "";
                }
                else
                {
                    return this.SampleSendingDate.ToString("dd MMM yyyy");
                }
            }
        }
        public string SendingDeadLineInString
        {
            get
            {
                if (this.SendingDeadLine == DateTime.MinValue)
                {
                    return "";
                }
                else
                {
                    return this.SendingDeadLine.ToString("dd MMM yyyy");
                }
            }
        }
        #endregion

        #region Functions
        public static List<DevelopmentRecapSummary> GetsRecapWithDevelopmentRecapSummarys(int nStartRow, int nEndRow, string SQL, string sDevelopmentRecapSummaryIDs, bool bIsPrint, long nUserID)
        {
            return DevelopmentRecapSummary.Service.GetsRecapWithDevelopmentRecapSummarys(nStartRow,nEndRow,SQL,sDevelopmentRecapSummaryIDs,bIsPrint, nUserID);
        }
        #endregion

        #region NonDB Functions
        public static string IDInString(List<DevelopmentRecapSummary> oDevelopmentRecapSummarys)
        {
            string sReturn = "";
            foreach (DevelopmentRecapSummary oItem in oDevelopmentRecapSummarys)
            {
                sReturn = sReturn + oItem.DevelopmentRecapID.ToString() + ",";
            }
            if (sReturn == "") return "";
            sReturn = sReturn.Remove(sReturn.Length - 1, 1);
            return sReturn;
        }
        #endregion

        #region ServiceFactory
        internal static IDevelopmentRecapSummaryService Service
        {
            get { return (IDevelopmentRecapSummaryService)Services.Factory.CreateService(typeof(IDevelopmentRecapSummaryService)); }
        }

        #endregion
    }
    #endregion

    #region IDevelopmentRecapSummary interface
     
    public interface IDevelopmentRecapSummaryService
    {
         
        List<DevelopmentRecapSummary> GetsRecapWithDevelopmentRecapSummarys(int nStartRow, int nEndRow, string SQL, string sDevelopmentRecapSummaryIDs, bool bIsPrint, Int64 nUserID);
    }
    #endregion
}
