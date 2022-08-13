using System;
using System.IO;
using ESimSol.BusinessObjects;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Collections.Generic;
using ICS.Core.Framework;
using ICS.Core.Utility;

namespace ESimSol.BusinessObjects
{
    #region PurchaseInvoiceLCReport
    public class PurchaseInvoiceLCReport : BusinessObject
    {
        #region  Constructor
        public PurchaseInvoiceLCReport()
        {
            PurchaseInvoiceID = 0;
            PurchaseInvoiceNo = "";
            SupplierID = 0;
            SupplierName = "";
            InvoiceType = EnumImportPIType.None;
            CurrencyName = "";
            CurrencySymbol = "";
            MUnit = "";
            DateofInvoice = DateTime.Now;
            PurchaseInvoiceDetailID = 0;
            ProductID = 0;
            ProductName = "";
            ProductCode = "";
            UnitPrice = 0;
            Quantity = 0;
            Company = new Company();
            CurrentStatus = EnumPurchaseInvoiceEvent.Initialized;
            ReceiveQty = 0;
            LCNO = "";
            DateOfLC = DateTime.Now;
            PaymentType = EnumLCPaymentType.None;
            NegotiateBankBranchID = 0;
            BankName = "";
            BLNo = "";
            BLDate = DateTime.Now;
            ETADate = DateTime.Now;
            LiabilityType = EnumLiabilityType.None;
            LiabilityNo = "";
            DateOfStockIn = DateTime.Now;
            BankStatus = EnumInvoiceBankStatus.None;
            DateofPayment = DateTime.Now;
            ErrorMessage = "";
            //Adv Searching 
            sPCTypes = "";
            sPaymentTypes = "";
            sBankStatus = "";
            sApprovalStatus = "";
            Currencys = new List<Currency>();
            SelectedOption = 0;
            DateofInvoiceEnd = DateTime.Now;
            SelectedOptionMDate = 0;
            DateofMaturity = DateTime.Now;
            DateofMaturityEnd = DateTime.Now;
            sLCStatus = "";

        }
        #endregion
        #region Properties
        [DataMember]
        public int PurchaseInvoiceID { get; set; }
        [DataMember]
        public string PurchaseInvoiceNo { get; set; }
        [DataMember]
        public int SupplierID { get; set; }
        [DataMember]
        public string SupplierName { get; set; }
        [DataMember]
        public EnumImportPIType InvoiceType { get; set; }
        [DataMember]
        public int PurchaseInvoiceLCReportTypeInInt { get; set; }
        [DataMember]
        public string CurrencyName { get; set; }
         [DataMember]
        public string CurrencySymbol { get; set; }
        
        [DataMember]
        public DateTime DateofInvoice { get; set; }
        [DataMember]
        public int PurchaseInvoiceDetailID { get; set; }
        [DataMember]
        public int ProductID { get; set; }
        [DataMember]
        public string ProductName { get; set; }
        [DataMember]
        public string ProductCode { get; set; }
        [DataMember]
        public double UnitPrice { get; set; }
        [DataMember]
        public double Quantity { get; set; }
        [DataMember]
        public string MUnit { get; set; }
        [DataMember]
        public string LCNO { get; set; }
        [DataMember]
        public DateTime DateOfLC { get; set; }
        [DataMember]
        public int NegotiateBankBranchID { get; set; }
        [DataMember]
        public string BankName { get; set; }
        [DataMember]
        public string BLNo { get; set; }
        [DataMember]
        public DateTime BLDate { get; set; }
        [DataMember]
        public DateTime ETADate { get; set; }
        [DataMember]
        public EnumLiabilityType LiabilityType { get; set; }
        [DataMember]
        public int LiabilityTypeInt { get; set; }
        [DataMember]
        public string LiabilityNo { get; set; }
        [DataMember]
        public DateTime DateOfStockIn { get; set; }
        [DataMember]
        public EnumInvoiceBankStatus BankStatus { get; set; }
        [DataMember]
        public DateTime DateofPayment { get; set; }
        [DataMember]
        public string ErrorMessage { get; set; }
        [DataMember]
        public EnumPurchaseInvoiceEvent CurrentStatus { get; set; }

        [DataMember]
        public List<BankBranch> BankBranchs { get; set; }
        [DataMember]
        public string sPCTypes { get; set; }
        [DataMember]
        public string sPaymentTypes { get; set; }
        [DataMember]
        public string sBankStatus { get; set; }
        [DataMember]
        public string sApprovalStatus { get; set; }
        [DataMember]
        public List<Currency> Currencys { get; set; }

        [DataMember]
        public int SelectedOption { get; set; }
        [DataMember]
        public DateTime DateofInvoiceEnd { get; set; }
        [DataMember]
        public int SelectedOptionMDate { get; set; }
        [DataMember]
        public DateTime DateofMaturity { get; set; }
        [DataMember]
        public DateTime DateofMaturityEnd { get; set; }
        [DataMember]
        public string sLCStatus { get; set; }
        [DataMember]
        public double ReceiveQty { get; set; }
        [DataMember]
        public EnumLCPaymentType PaymentType { get; set; }

        #region Derive
        public string InvoiceTypeInString
        {
            get
            {
                return InvoiceType.ToString();
            }
        }

        public string DateofInvoiceInString
        {
            get
            {
                return DateofInvoice.ToString("dd MMM yyyy");
            }
        }
        public string DateOfLCInString
        {
            get
            {
                return DateOfLC.ToString("dd MMM yyyy");
            }
        }
        public string BLDateInString
        {
            get
            {
                return BLDate.ToString("dd MMM yyyy");
            }
        }
        public string ETADateInString
        {
            get
            {
                return ETADate.ToString("dd MMM yyyy");
            }
        }
        public string DateOfStockInInString
        {
            get
            {
                return DateOfStockIn.ToString("dd MMM yyyy");
            }
        }
        public string DateofPaymentInString
        {
            get
            {
                return DateofPayment.ToString("dd MMM yyyy");
            }
        }
        
        public string AmountSt
        {
            get
            {
                return this.CurrencySymbol + Global.MillionFormat((this.Quantity * this.UnitPrice));
            }
        }

        public double Amount
        {
            get
            {
                return (this.Quantity * this.UnitPrice);
            }
        }

        public string UPriceSt
        {
            get
            {
                return this.CurrencySymbol + this.UnitPrice.ToString();
            }
        }

        public string QtySt
        {
            get
            {
                return this.Quantity.ToString() + this.MUnit;
            }
        }

        public string CurrentStatusInString
        {
            get
            {
                return CurrentStatus.ToString();
            }
        }
        public string PaymentTypeInString
        {
            get
            {
                return PaymentType.ToString();
            }
        }
        public string LiabilityTypeInString
        {
            get
            {
                return LiabilityType.ToString();
            }
        }
        public string BankStatusInString
        {
            get
            {
                return BankStatus.ToString();
            }
        }
        public string ShortQtySt
        {
            get
            {
                return (this.Quantity - this.ReceiveQty).ToString() + this.MUnit;
            }
        }
        public double ShortQty
        {
            get
            {
                return (this.Quantity - this.ReceiveQty) ;
            }
        }
        public string ShortValueSt
        {
            get
            {
                return this.CurrencySymbol + ((this.Quantity - this.ReceiveQty)*this.UnitPrice).ToString() ;
            }
        }
        public double ShortValue
        {
            get
            {
                return (this.Quantity - this.ReceiveQty) * this.UnitPrice;
            }
        }
        public double Delay
        {
            get
            {
                return Math.Round(this.ShortValue/360);
            }
        }
        public List<PurchaseInvoiceLCReport> PurchaseInvoiceLCReports { get; set; }
        #endregion Derive

        [DataMember]
        public Company Company { get; set; }



        #endregion

        #region Functions
        public static List<PurchaseInvoiceLCReport> Gets(string sSQL, int nUserID)
        {
            return PurchaseInvoiceLCReport.Service.Gets(sSQL, nUserID);
        }


        #endregion

        #region ServiceFactory

        internal static IPurchaseInvoiceLCReportService Service
        {
            get { return (IPurchaseInvoiceLCReportService)Services.Factory.CreateService(typeof(IPurchaseInvoiceLCReportService)); }
        }
        #endregion
    }
    #endregion


    #region IPurchaseInvoiceLCReport interface
    [ServiceContract]
    public interface IPurchaseInvoiceLCReportService
    {

        [OperationContract]
        List<PurchaseInvoiceLCReport> Gets(string sSQL, Int64 nUserID);

    }
    #endregion
}