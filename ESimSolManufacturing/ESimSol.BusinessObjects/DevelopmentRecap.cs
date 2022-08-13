using System;
using System.IO;
using System.ComponentModel.DataAnnotations;
using ICS.Core;
using ESimSol.BusinessObjects;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Collections.Generic;
using ICS.Core.Framework;
using ICS.Core.Utility;
using ESimSol.BusinessObjects.ReportingObject;

namespace ESimSol.BusinessObjects
{
    #region DevelopmentRecap
    
    public class DevelopmentRecap : BusinessObject
    {
        public DevelopmentRecap()
        {
            DevelopmentRecapID = 0;
            BUID = 0;
            BusinessSessionID = 0;
            SessionName = "";
            DevelopmentRecapNo = "";
            TechnicalSheetID = 0;
            DevelopmentStatus = EnumDevelopmentStatus.Initialize;
            InquiryReceivedDate = DateTime.MinValue;
            GG = "";
            SampleQty = 0;
            SampleSizeID = 0;
            SampleReceivedDate = DateTime.MinValue;
            SampleSendingDate = DateTime.MinValue;
            SendingDeadLine = DateTime.MinValue;
            AwbNo = "";
            Remarks = "";
            SpecialFinish ="";
	        MerchandiserID = 0;
	        TransportType = 0;
            YarnCategoryID = 0;
            UnitID = 0;
	        Weight ="";
            UnitName ="";
            MerchandiserName ="";
            YarnName ="";
            CollectionName = "";
            StyleNo = "";
            BuyerID = 0;
            BuyerContactPersonID = 0;
            SampleSize = "";
            BuyerName = "";
            BuerContactPersonName = "";
            ErrorMessage = "";            
            PrepareBy = "";            
            StyleCoverImage = null;
            ProductName = "";
            FabricOptionA = "";
            ProductID = 0;
            FabricOptionB = "";
            FabricOptionC = "";
            Count = "";
            ProductCategoryID = 0;
            OperationDate = DateTime.Today;
            Note = "";
            BrandName = "";
            OperationBy = 0;
            CurrentStatusInt = 0;
            DevelopmentType = 0;
            OrderRecapQty = 0;
            DevelopmentTypeName = "";
            IsActive = true;
            UnitPrice = 0;
            CurrencyID = 0;
            CurrencyName = "";
            CurrencySymbol = "";
            DevelopmentYarnOptions = new List<DevelopmentYarnOption>();
            Currencys = new List<Currency>();
        }

        #region Properties         
        public int DevelopmentRecapID { get; set; }
        public int BUID { get; set; }
        public int BusinessSessionID { get; set; }         
        public string SessionName { get; set; }         
        public string DevelopmentRecapNo { get; set; }         
        public int TechnicalSheetID { get; set; }         
        public EnumDevelopmentStatus DevelopmentStatus { get; set; }         
        public DateTime InquiryReceivedDate { get; set; }         
        public string GG { get; set; }
        public string BrandName { get; set; }
        public string PrepareBy { get; set; }         
        public double SampleQty { get; set; }
         public int SampleSizeID { get; set; }         
        public DateTime SampleReceivedDate { get; set; }         
        public DateTime SampleSendingDate { get; set; }         
        public DateTime SendingDeadLine { get; set; }         
        public string AwbNo { get; set; }         
        public string Remarks { get; set; }         
        public int MerchandiserID { get; set; }         
        public EnumTransportType TransportType { get; set; }         
        public int YarnCategoryID { get; set; }         
        public int UnitID { get; set; }         
        public string SpecialFinish { get; set; }         
        public string Weight { get; set; }         
        public string UnitName { get; set; }         
        public string MerchandiserName { get; set; }         
        public string YarnName { get; set; }         
        public string CollectionName { get; set; }         
        public string StyleNo { get; set; }         
        public int BuyerID { get; set; }         
        public int BuyerContactPersonID  { get; set; }         
        public string SampleSize { get; set; }         
        public string BuyerName { get; set; }         
        public string BuerContactPersonName { get; set; }
        public string DevelopmentTypeName { get; set; }
        public string ErrorMessage { get; set; }         
        public string DevelopmentRecapDetailInString { get; set; }         
        public int ProductID { get; set; }         
        public string ProductName { get; set; }         
        public string Count { get; set; }
         public bool IsActive { get; set; }
        public int DevelopmentRecapHistoryID { get; set; }         
        public int CurrentStatusInt { get; set; }         
        public EnumDevelopmentStatus CurrentStatus { get; set; }         
        public EnumDevelopmentStatus PreviousStatus { get; set; }         
        public int DevelopmentType { get; set; }
        public double UnitPrice { get; set; }
        public int CurrencyID { get; set; }
        public string CurrencyName {get;set;}
        public string CurrencySymbol { get; set; }	    
        public int OperationBy { get; set; }         
        public DateTime OperationDate { get; set; }         
        public string Note { get; set; }         
        public DevelopmentRecapHistory DevelopmentRecapHistory { get; set; }         
        public List<DevelopmentRecapHistory> DevelopmentRecapHistorys = new List<DevelopmentRecapHistory>();         
        public double OrderRecapQty { get; set; }

        // development Recap History
        #endregion

        #region Derived Property
        public System.Drawing.Image StyleCoverImage { get; set; }
        public List<ContactPersonnel> ContactPersonnels { get; set; }
        public BusinessUnit BusinessUnit { get; set; }
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
                return this.CurrencySymbol + " " + Global.MillionFormat(this.UnitPrice);
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
        public string InquiryReceivedDateAsString
        {
            get
            {
                return this.InquiryReceivedDate.ToString("M/dd/yyyy");
            }
        }
        public string SampleReceivedDateAsString
        {
            get
            {
                return SampleReceivedDate.ToString("M/dd/yyyy");
            }
        }
        public string SampleSendingDateAsString
        {
            get
            {
                return this.SampleSendingDate.ToString("M/dd/yyyy");
            }
        }
        public string IsActiveInString
        {
            get
            {
             if(this.IsActive)
             {
                 return "Active";
             }
             else
             {
                 return "In Active";
             }
            }
        }
        public List<SizeCategory> SampleSizes { get; set; }
        public List<DevelopmentRecapDetail> DevelopmentRecapDetails { get; set; }         
        public List<DevelopmentRecap> DevelopmentRecapList { get; set; }
        public List<Contractor> ContractorList { get; set; }
        public List<ProductCategory> ProductCategorys { get; set; }
        public Company Company { get; set; }
        public string ImageUrl { get; set; }
        public int DevelopmentStatusInInt { get; set; }
        public string FabricOptionA { get; set; }
        public string FabricOptionB { get; set; }
        public string FabricOptionC { get; set; }
        public int ProductCategoryID { get; set; }         
        public List<ProductionOrderDetail> ProductionOrderDetails { get; set; }         
        public VOrder SaleOrder { get; set; }
        public ApprovalRequest ApprovalRequest { get; set; }
        public List<ColorSizeRatio> ColorSizeRatios { get; set; }         
        public List<DevelopmentRecapSizeColorRatio> DevelopmentRecapSizeColorRatios { get; set; }         
        public List<DevelopmentYarnOption> DevelopmentYarnOptions{ get; set; }
        public List<ProductionOrder> ProductionOrders { get; set; }         
        public ProductionOrder ProductionOrder { get; set; }
        public TechnicalSheet TechnicalSheet { get; set; }
        public EnumDevelopmentRecapActionType ActionType { get; set; }
        public int StatusExtra { get; set; }
        public string ActionTypeExtra { get; set; }// for approve
        public List<BusinessSession> BusinessSessions { get; set; }
        public List<Currency> Currencys { get; set; }
        #endregion

        #region Functions
        public static List<DevelopmentRecap> Gets_Report(int id, long nUserID)
        {
            return DevelopmentRecap.Service.Gets_Report(id, nUserID);
        }
        public static List<DevelopmentRecap> Gets(long nUserID)
        {
            return DevelopmentRecap.Service.Gets( nUserID);
        }
        public static List<DevelopmentRecap> GetsRecapWithDevelopmentRecaps(int nStartRow, int nEndRow, string sDevelopmentRecapIDs, bool bIsPrint, long nUserID)
        {
            return DevelopmentRecap.Service.GetsRecapWithDevelopmentRecaps(nStartRow,nEndRow,sDevelopmentRecapIDs, bIsPrint, nUserID);
        }
        public static List<DevelopmentRecap> GetsRecapWithRowNumber(int nStartRow, int nEndRow, long nUserID)
        {
            return DevelopmentRecap.Service.GetsRecapWithRowNumber(nStartRow, nEndRow, nUserID);
        }
        public DevelopmentRecap ActiveInActive(int nID, bool bIsActive, long nUserID)
        {
            return DevelopmentRecap.Service.ActiveInActive(nID, bIsActive, nUserID);
        }

        public DevelopmentRecap Get(int id, long nUserID)
        {
            return DevelopmentRecap.Service.Get(id, nUserID);
        }

        public static List<DevelopmentRecap> GetsByTechnicalSheet(int nTechnicalSheetID, long nUserID)
        {
            return DevelopmentRecap.Service.GetsByTechnicalSheet(nTechnicalSheetID, nUserID);
        }
        

        public DevelopmentRecap UpdateInqRcvDate(int id, DateTime dInqRcvDate, long nUserID)
        {
            return DevelopmentRecap.Service.UpdateInqRcvDate(id, dInqRcvDate, nUserID);
        }

        public DevelopmentRecap UpdateSmplRcvDate(int id, DateTime dSmplRcvDate, long nUserID)
        {
            return DevelopmentRecap.Service.UpdateSmplRcvDate(id,dSmplRcvDate,  nUserID);
        }

        public DevelopmentRecap UpdateSmplSendingDate(int id, DateTime dSmplSendingDate, long nUserID)
        {
            return DevelopmentRecap.Service.UpdateSmplSendingDate(id, dSmplSendingDate, nUserID);
        }
        public DevelopmentRecap UpdateSendingDeadLine(int id, DateTime dSendingDeadLine, long nUserID)
        {
            return DevelopmentRecap.Service.UpdateSendingDeadLine(id, dSendingDeadLine, nUserID);
        }

        public static List<DevelopmentRecap> Gets(string sSql, long nUserID)
        {
            return DevelopmentRecap.Service.Gets(sSql, nUserID);
        }
        public DevelopmentRecap Save(long nUserID)
        {
            return DevelopmentRecap.Service.Save(this, nUserID);
        }
        public DevelopmentRecap SaveInProduction(long nUserID)
        {
            return DevelopmentRecap.Service.SaveInProduction(this, nUserID);
        }
        public string Delete(int id, long nUserID)
        {
            return DevelopmentRecap.Service.Delete(id, nUserID);
        }

        public DevelopmentRecap ChangeContactStatus(ApprovalRequest oApprovalRequest, long nUserID)
        {
            return DevelopmentRecap.Service.ChangeStatus(this,oApprovalRequest, nUserID);
        }
        #endregion

        #region NonDB Functions
        public static string IDInString(List<DevelopmentRecap> oDevelopmentRecaps)
        {
            string sReturn = "";
            foreach (DevelopmentRecap oItem in oDevelopmentRecaps)
            {
                sReturn = sReturn + oItem.DevelopmentRecapID.ToString() + ",";
            }
            if (sReturn == "") return "";
            sReturn = sReturn.Remove(sReturn.Length - 1, 1);
            return sReturn;
        }
        #endregion

        #region ServiceFactory
        internal static IDevelopmentRecapService Service
        {
            get { return (IDevelopmentRecapService)Services.Factory.CreateService(typeof(IDevelopmentRecapService)); }
        }

        #endregion
    }
    #endregion

    #region IDevelopmentRecap interface
     
    public interface IDevelopmentRecapService
    {
        DevelopmentRecap Get(int id, Int64 nUserID);
        List<DevelopmentRecap> GetsByTechnicalSheet(int nTechnicalSheetID, Int64 nUserID);
        DevelopmentRecap UpdateInqRcvDate(int id, DateTime dInqRcvDate, Int64 nUserID);
        DevelopmentRecap UpdateSmplRcvDate(int id, DateTime dSmplRcvDate, Int64 nUserID);
        DevelopmentRecap UpdateSmplSendingDate(int id, DateTime dSmplSendingDate, Int64 nUserID);
        DevelopmentRecap UpdateSendingDeadLine(int id, DateTime dSendingDeadLine, Int64 nUserID);
        List<DevelopmentRecap> Gets_Report(int id, Int64 nUserID);
        List<DevelopmentRecap> Gets(Int64 nUserID);
        List<DevelopmentRecap> Gets(string sSql, Int64 nUserID);
        List<DevelopmentRecap> GetsRecapWithRowNumber(int nStartRow, int nEndRow, Int64 nUserID);
        List<DevelopmentRecap> GetsRecapWithDevelopmentRecaps(int nStartRow, int nEndRow, string sDevelopmentRecapIDs, bool bIsPrint, Int64 nUserID);
        string Delete(int id, Int64 nUserID);        
        DevelopmentRecap Save(DevelopmentRecap oDevelopmentRecap, Int64 nUserID);
        DevelopmentRecap ActiveInActive(int nID, bool bIsActive, Int64 nUserID);
        
        DevelopmentRecap SaveInProduction(DevelopmentRecap oDevelopmentRecap, Int64 nUserID);
        DevelopmentRecap ChangeStatus(DevelopmentRecap oDevelopmentRecap, ApprovalRequest oApprovalRequest, Int64 nUserID);
    }
    #endregion
}
