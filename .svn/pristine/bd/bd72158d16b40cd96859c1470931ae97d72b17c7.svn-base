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
    #region ImportPaymentRequestDetail
    [DataContract]
    public class ImportPaymentRequestDetail : BusinessObject
    {
        public ImportPaymentRequestDetail()
        {
            PIPRDetailID = 0;
            PIPRID = 0;
            ImportInvoiceID = 0;
            ImportInvoiceNo = "";
            ImportLCNo = "";
            DateofInvoice = DateTime.Now;
            Amount = 0;
            BUID = 0;
            DateofMaturity = DateTime.Now;
            IssueBankName = "";
            ErrorMessage = "";
        }

        #region Properties
        public int PIPRDetailID { get; set; }
        public int PIPRID { get; set; }
        public int ImportInvoiceID { get; set; }
        public string ImportInvoiceNo { get; set; }
        public string ImportLCNo { get; set; }
        public DateTime DateofInvoice { get; set; }
        public double Amount { get; set; }        
        public DateTime DateofMaturity { get; set; }
        public string IssueBankName { get; set; }
        public string AccountNo { get; set; }
        public string ContractorName { get; set; }
        public double CCRate { get; set; }
        public int CurrencyID { get; set; }
        public string Currency { get; set; }
        public int Status { get; set; }
        public string ErrorMessage { get; set; }
        public int BankBranchID { get; set; }
        public int BUID { get; set; }
        #endregion

        #region Derived Property
         //public PurchaseInvoice PurchaseInvoice { get; set; }
         public string DateofMaturityST
        {
            get
            {
                return this.DateofMaturity.ToString("dd MMM yyyy");
            }
        }
         public string AmountSt
        {
            get
            {
                return this.Currency + Global.MillionFormat(this.Amount);
            }
        }
         public string AmountBCSt
        {
            get
            {
                return "BDT "+Global.MillionFormat(this.Amount*this.CCRate);
            }
        }
        public string InvoiceNoWithLCNo
         {
             get
             {
                 return this.ImportInvoiceNo + this.ImportLCNo;
             }
         }
        
        public string StatusSt
        {
            get
            {
                return this.Status.ToString();
            }
        }
        #endregion

        #region Functions
        public static List<ImportPaymentRequestDetail> Gets(int nImportPaymentRequestID, int nUserID)
        {
            return ImportPaymentRequestDetail.Service.Gets(nImportPaymentRequestID, nUserID);
        }
        public static List<ImportPaymentRequestDetail> Gets(string sSQL, int nUserID)
        {
            return ImportPaymentRequestDetail.Service.Gets(sSQL, nUserID);
        }
        public ImportPaymentRequestDetail Get(int nImportPaymentRequestDetailID, int nUserID)
        {
            return ImportPaymentRequestDetail.Service.Get(nImportPaymentRequestDetailID, nUserID);
        }
        public ImportPaymentRequestDetail GetBy(int nPurchaseInvoiceLClID, int nUserID)
        {
            return ImportPaymentRequestDetail.Service.GetBy(nPurchaseInvoiceLClID, nUserID);
        }
        public ImportPaymentRequestDetail Save(int nUserID)
        {
            return ImportPaymentRequestDetail.Service.Save(this, nUserID);
        }
      
      
        #endregion

        #region ServiceFactory

        internal static IImportPaymentRequestDetailService Service
        {
            get { return (IImportPaymentRequestDetailService)Services.Factory.CreateService(typeof(IImportPaymentRequestDetailService)); }
        }
        #endregion
    }
    #endregion

    #region IImportPaymentRequestDetail interface
    public interface IImportPaymentRequestDetailService
    {
        ImportPaymentRequestDetail Get(int nImportPaymentRequestDetailID, Int64 nUserID);
        ImportPaymentRequestDetail GetBy(int nPurchaseInvoiceLClID, Int64 nUserID);
        List<ImportPaymentRequestDetail> Gets(int nImportPaymentRequestID, Int64 nUserID);
        List<ImportPaymentRequestDetail> Gets(string sSQL, Int64 nUserID);
        ImportPaymentRequestDetail Save(ImportPaymentRequestDetail oImportPaymentRequestDetail, Int64 nUserID);
       
    }
    #endregion
}