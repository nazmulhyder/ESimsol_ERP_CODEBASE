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



    #region PurchaseQuotation

    public class PurchaseQuotation : BusinessObject
    {
        public PurchaseQuotation()
        {
            PurchaseQuotationLogID = 0;
            PurchaseQuotationID = 0;
            PurchaseQuotationNo = "";
            QuotationStatus = EnumQuotationStatus.Initialize;
            QuotationStatusInInt = 0;
            SupplierReference = "";
            RateCollectDate =DateTime.Now;
            ExpiredDate = DateTime.Now;
            SCPerson = 0;
            SCPersonName ="";
            CollectBy = 0;
            CurrencyID = 1;
            CurrencyName = "";
            CurrencySymbol = "";
            ApprovedBy = 0;
            ApprovedDate = DateTime.Now;
            Remarks = "";
            SupplierID = 0; 
            SupplierName= "";
            CollectByName ="";
            ApprovedByName = "";
            Parameter = "";
            BuyerID = 0;
            BuyerName = "";
            Source = EnumSource.Manual;
            SourceInInt = 0;
            Activity = true;
            SupplierAddress = "";
            BUID = 0;
            BUName = "";
            ErrorMessage = "";
            DiscountInPercent=0;
            DiscountInAmount = 0;
            oPQSpec = new List<PQSpec>();
            Contractors = new Contractor();
        

        }

        #region Properties

        public int PurchaseQuotationLogID { get; set; }
        public int PurchaseQuotationID { get; set; }
        public string PurchaseQuotationNo { get; set; }
        public EnumQuotationStatus QuotationStatus { get; set; }
        public int QuotationStatusInInt { get; set; }
        public string SupplierReference { get; set; }
        public DateTime ExpiredDate { get; set; }
        public int SCPerson { get; set; }
        public string SCPersonName { get; set; }
        public int CollectBy { get; set; }
        public int CurrencyID { get; set; }
        public DateTime RateCollectDate { get; set; }
        public string CurrencyName { get; set; }
        public string CurrencySymbol { get; set; }
        public int ApprovedBy { get; set; }
        public string Parameter { get; set; }
        public DateTime ApprovedDate { get; set; }
        public EnumSource   Source {get;set;}
        public bool Activity { get; set; }
        public string Remarks { get; set; }
        public int BuyerID { get; set; }
        public string BuyerName { get; set; }
        public string SupplierAddress { get; set; }
        public string Note { get; set; }
        public int SupplierID { get; set; }
        public string PaymentTerm { get; set; }
       
        public int BUID { get; set; }
        public string BUName { get; set; }
        public string SupplierName { get; set; }
        public int SourceInInt { get; set; }
        public string CollectByName { get; set; }
        public string ApprovedByName { get; set; }

        public string ErrorMessage { get; set; }
        public double DiscountInPercent { get; set; }
        public double DiscountInAmount { get; set; }
        public double VatInPercent { get; set; }
        public double VatInAmount { get; set; }
        public double TransportCostInPercent { get; set; }
        public double TransportCostInAmount { get; set; }
        #endregion

        #region Derived Property
        public List<ContactPersonnel> SCPersons { get; set; }
        public List<PurchaseQuotation> PurchaseQuotations { get; set; }
        public Contractor Contractors { get; set; }
        public List<PQSpec> oPQSpec { get; set; }
     
        public Company Company { get; set; }

        public string RateCollectDateInString
        {
            get
            {
                return RateCollectDate.ToString("dd MMM yyyy");
            }
        }

        public string ExpiredDateInString
        {
            get
            {
                return ExpiredDate.ToString("dd MMM yyyy");
            }
        }
        public string ApprovedDateInString
        {
            get
            {
                return ApprovedDate.ToString("dd MMM yyyy");
            }
        }

        public string StatusInString
        {
            get
            {
                return QuotationStatus.ToString();
            }
        }

        public List<PurchaseQuotationDetail> PurchaseQuotationDetails { get; set; }
        public List<PQTermsAndCondition> PQTermsAndConditions { get; set; }
        #endregion

        #region Functions

        public static List<PurchaseQuotation> Gets(long nUserID)
        {
            return PurchaseQuotation.Service.Gets(nUserID);
        }
        public static List<PurchaseQuotation> Gets(string sSQL, long nUserID)
        {
            return PurchaseQuotation.Service.Gets(sSQL, nUserID);
        }

        public static List<PurchaseQuotation> GetsByLog(string sSQL, long nUserID)
        {
            return PurchaseQuotation.Service.GetsByLog(sSQL, nUserID);
        }

        public static List<PurchaseQuotation> GetsByBU(int nBUID, long nUserID)
        {
            return PurchaseQuotation.Service.GetsByBU(nBUID, nUserID);
        }
        public PurchaseQuotation Get(int id, long nUserID)
        {
            return PurchaseQuotation.Service.Get(id, nUserID);
        }
        public PurchaseQuotation GetByLog(int id, long nUserID)
        {
            return PurchaseQuotation.Service.GetByLog(id, nUserID);
        }
        public PurchaseQuotation SendToMgt(int id, long nUserID)
        {
            return PurchaseQuotation.Service.SendToMgt(id, nUserID);
        }
        public PurchaseQuotation Save(long nUserID)
        {
            return PurchaseQuotation.Service.Save(this, nUserID);
        }
        public PurchaseQuotation Approve(long nUserID)
        {
            return PurchaseQuotation.Service.Approve(this, nUserID);
        }
        public string Delete(int id, long nUserID)
        {
            return PurchaseQuotation.Service.Delete(id, nUserID);
        }
        public PurchaseQuotation RequestQuotationRevise(long nUserID)
        {
            return PurchaseQuotation.Service.RequestQuotationRevise(this, nUserID);
        }
        public PurchaseQuotation AcceptRevise(long nUserID)
        {
            return PurchaseQuotation.Service.AcceptRevise(this, nUserID);
        }
        #endregion

        #region ServiceFactory

        internal static IPurchaseQuotationService Service
        {
            get { return (IPurchaseQuotationService)Services.Factory.CreateService(typeof(IPurchaseQuotationService)); }
        }

        #endregion
    }
    #endregion

    #region IPurchaseQuotation interface

    public interface IPurchaseQuotationService
    {
        PurchaseQuotation RequestQuotationRevise(PurchaseQuotation oPurchaseQuotation, Int64 nUserID);
        PurchaseQuotation AcceptRevise(PurchaseQuotation oPurchaseQuotation, Int64 nUserID);
        PurchaseQuotation Get(int id, Int64 nUserID);
        PurchaseQuotation GetByLog(int id, Int64 nUserID);
        PurchaseQuotation SendToMgt(int id, Int64 nUserID);
        List<PurchaseQuotation> Gets(Int64 nUserID);
        List<PurchaseQuotation> Gets(string sSQL, Int64 nUserID);
        List<PurchaseQuotation> GetsByLog(string sSQL, Int64 nUserID);
        List<PurchaseQuotation> GetsByBU(int nBUID, Int64 nUserID);
        string Delete(int id, Int64 nUserID);
        PurchaseQuotation Approve(PurchaseQuotation oPurchaseQuotation, Int64 nUserID);
        PurchaseQuotation Save(PurchaseQuotation oPurchaseQuotation, Int64 nUserID);

    }
    #endregion
    
    
}
