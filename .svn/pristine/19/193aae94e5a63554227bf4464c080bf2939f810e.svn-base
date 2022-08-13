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
    #region SaleInvoice
    
    public class SaleInvoice : BusinessObject
    {
        public SaleInvoice()
        {
            SaleInvoiceID=0;
            InvoiceNo="";
            InvoiceDate=DateTime.Now;
            ContractorID=0;
            ContactPersonID=0;
            SalesQuotationID=0;
            IsNewOrder=false;
            VehicleLocation=0;
            PRNo="";
            KommNo = "";
            BankName = "";
            SpecialInstruction="";
            ETAAgreement="";
            ETAWeeks="";
            CurrencyID=0;
            OTRAmount=0;
            VatAmount = 0;
            DiscountAmount=0;
            NetAmount=0;
            AdvanceAmount=0;
            AdvanceDate=DateTime.Now;
            PaymentMode=0;
            ChequeNo="";
            MarketingAccountName = "";
            ChequeDate=DateTime.Now;
            BankID=0;
            ReceivedBy = 0;
            ReceivedByName = "";
            Remarks="";
            AttachmentDoc=0;
            ApprovedBy = 0;
            InteriorColorCode="";  
            InteriorColorName="";
            ExteriorColorCode="";
            ExteriorColorName="";
            EngineNo="";
            ChassisNo="";
            ModelNo="";
            SQNo = "";
            SalesQuotationImageID=0;
            CustomerName="";
            CustomerAddress="";
            CustomerCity="";
            CustomerLandline="";
            CustomerCellPhone="";
            RegistrationFee = 0;
            CurrencyName="";
            TDSAmount = 0;
            CurrencySymbol="";
            ReceivedOn="";
            ApprovedByName = "";
            PIID = 0;
        }

        #region Properties
        public int SaleInvoiceID {get; set;}
        public int PIID { get; set; }
        public string PINo { get; set; }
        public string InvoiceNo { get; set; }
        public string SQNo { get; set; }
        public string KommNo { get; set; }
        public int MarketingAccountID { get; set; }
        public string MarketingAccountName { get; set; }
        public DateTime InvoiceDate {get; set;}
        public int ContractorID { get; set; }
        public int ContactPersonID {get; set;}
        public int SalesQuotationID {get; set;}
        public bool IsNewOrder {get; set;}
        public int VehicleLocation {get; set;}
        public string PRNo { get; set; }
        public string BankName { get; set; }
        public string SpecialInstruction {get; set;}
        public string ETAAgreement {get; set;}
        public string ETAWeeks {get; set;}
        public int CurrencyID {get; set;}
        public double OTRAmount {get; set;}
        public double DiscountAmount {get; set;}
        public double NetAmount {get; set;}
        public double AdvanceAmount {get; set;}
        public DateTime AdvanceDate {get; set;}
        public int PaymentMode {get; set;}
        public string ChequeNo {get; set;}
        public DateTime ChequeDate { get; set; }
        public int BankID {get; set;}
        public int ReceivedBy {get; set;}
        public string ReceivedByName {get; set;}
        public string ReceivedOn {get; set;}
        public string Remarks { get; set; }
        public int AttachmentDoc {get; set;}
        public int ApprovedBy {get; set;}
        public string InteriorColorCode {get; set;}
        public string InteriorColorName {get; set;}
        public string ExteriorColorCode {get; set;}
        public string ExteriorColorName {get; set;}
        public string EngineNo {get; set;}
        public string ChassisNo {get; set;}
        public string ModelNo {get; set;}
        public int VehicleModelID { get; set; }
        public int SalesQuotationImageID { get; set; }
        public string CustomerName {get; set;}
        public string CustomerAddress {get; set;}
        public string CustomerCity {get; set;}
        public string CustomerLandline {get; set;}
        public string CustomerCellPhone {get; set;}
        public string CurrencyName {get; set;}
        public string CurrencySymbol {get; set;}
        public string ApprovedByName {get; set;}
        public EnumSaleInvoiceStatus InvoiceStatus { get; set; }
        public int BUID { get; set; }
        public double CRate { get; set; }
        public int ProductID { get; set; }
        public string ProductName { get; set; }
        public double VatAmount { get; set; }
        public double PDIChargeAmount { get; set; }
        public double FreeServiceChargeAmount { get; set; }
        public double TDSAmount { get; set; }
        public string Params { get; set; }
        public string ErrorMessage { get; set; }        
      
        #endregion

         #region Derived Property
         public string AdvanceDateST
         {
             get { return this.AdvanceDate.ToString("dd MMM yyyy"); }
         }
         public string ChequeDateST
         {
             get { return this.ChequeDate.ToString("dd MMM yyyy"); }
         }
         public string InvoiceDateST
         {
             get { return this.InvoiceDate.ToString("dd MMM yyyy"); }
         }
         public int InvoiceStatusInt
         {
             get { return (int)(this.InvoiceStatus); }
         }
         public string InvoiceStatusST
         {
             get { return EnumObject.jGet(this.InvoiceStatus); }
         }

         public double RegistrationFee { get; set; }
        #endregion

        #region Functions
        public static List<SaleInvoice> Gets(long nUserID)
        {
            return SaleInvoice.Service.Gets( nUserID);
        }
     
        public SaleInvoice Get(int id, long nUserID)
        {
            return SaleInvoice.Service.Get(id, nUserID);
        }
    
        public SaleInvoice Save(long nUserID)
        {
            return SaleInvoice.Service.Save(this, nUserID);
        }
        public static List<SaleInvoice> Gets(string sSQL, long nUserID)
        {
            return SaleInvoice.Service.Gets(sSQL, nUserID);
        }
        public string Delete(int id, long nUserID)
        {
            return SaleInvoice.Service.Delete(id, nUserID);
        }

        public string UpdateStatus(SaleInvoice oServiceOrder, long nUserID)
        {
            return SaleInvoice.Service.UpdateStatus(oServiceOrder, nUserID);
        }
        #endregion

        #region ServiceFactory
        internal static ISaleInvoiceService Service
        {
            get { return (ISaleInvoiceService)Services.Factory.CreateService(typeof(ISaleInvoiceService)); }
        }

        #endregion


        public int nRequest { get; set; }
    }
    #endregion

    #region ISaleInvoice interface
     
    public interface ISaleInvoiceService
    {
         
        SaleInvoice Get(int id, Int64 nUserID);
         
        List<SaleInvoice> Gets(Int64 nUserID);
     
        List<SaleInvoice> Gets(string sSQL, Int64 nUserID);
         
        string Delete(int id, Int64 nUserID);
        SaleInvoice Save(SaleInvoice oSaleInvoice, Int64 nUserID);
        string UpdateStatus(SaleInvoice oSaleInvoice, Int64 nUserID);
    }
    #endregion
}

