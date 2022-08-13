using System;
using System.IO;
using ESimSol.BusinessObjects;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Collections.Generic;
using ICS.Core;
using ICS.Core.Framework;
using ICS.Core.Utility;

namespace ESimSol.BusinessObjects
{
    #region SalesComPayment
    public class SalesComPayment :BusinessObject
    {
        public SalesComPayment(){
        SalesComPaymentID =0;
        MRNo ="";
        MRDate =DateTime.Today;
        PaymentMode = EnumPaymentMethod.None;
        PaymentType = EnumPayment_CommissionType.None;
        ContactPersonnelID =0;
        Amount =0;
        Note="";
        ApproveBy=0;
        ApproveDate =DateTime.Today;
        CRate=1;
        CurrencyID=0;
        DocNo="";
        DocDate=DateTime.Today;
        BankAccountID=0;
        BUID=0;
        SalesComPaymentDetails = new List<SalesComPaymentDetail>();
        SampleInvoiceID = 0;
        }

        #region Properties
        public int SalesComPaymentID {get;set;}
        public string MRNo {get;set;}
        public DateTime MRDate {get;set;}
        public EnumPaymentMethod PaymentMode { get; set; }
        public EnumPayment_CommissionType PaymentType { get; set; }
        public int ContactPersonnelID {get;set;}
        public double Amount {get;set;}
        public string Note {get;set;}
        public int ApproveBy {get;set;}
        public DateTime ApproveDate {get;set;}
        public double CRate {get;set;}
        public int CurrencyID {get;set;}
        public string DocNo {get;set;}
        public DateTime DocDate {get;set;}
        public int BankAccountID {get;set;}
        public int BUID {get;set;}
        public int SampleInvoiceID { get; set; }
        public string ErrorMessage { get; set; }
        public string Params { get; set; }

        #endregion
        #region derived Properties
        public string SampleInvoiceNo {get; set;}
        public DateTime SampleInvoiceDate { get; set; }
        public string CPName { get; set; }
        public string ContractorName { get; set; }
        public string Currency { get; set; }
        public string AccountName { get; set; }
        public string AccountNo { get; set; }
        public string ApproveByName { get; set; }
        public string PreparedByName { get; set; }
        public int BankBranchID { get; set; }
        public int BankID { get; set; }
        public string BankName { get; set; }
        public string BranchName { get; set; }
        public string MRDateStr { get { return (this.MRDate == DateTime.MinValue) ? "--" : this.MRDate.ToString("dd MMM yyyy"); } }
        public string DocDateStr { get { return (this.DocDate == DateTime.MinValue) ? "--" : this.DocDate.ToString("dd MMM yyyy"); } }
        public string SampleInvoiceDateStr { get { return (this.SampleInvoiceDate == DateTime.MinValue) ? "--" : this.SampleInvoiceDate.ToString("dd MMM yyyy"); } }

        public string PaymentTypeStr { get { return this.PaymentType.ToString(); } }
        public string PaymentModeStr { get { return this.PaymentMode.ToString(); } }
        public List<SalesComPaymentDetail> SalesComPaymentDetails { get; set; }
        public string BankNameWithBranch { get { return (this.BankName + " [ " + this.BranchName + " ] "); } }
       public double SampleInvoice_Amount { get; set; }


        #endregion

        #region Functions
        public static List<SalesComPayment> Gets(string sSQL, long nUserID)
        {
            return SalesComPayment.Service.Gets(sSQL, nUserID);
        }

        public SalesComPayment IUD(int nDBOperation, long nUserID)
        {
            return SalesComPayment.Service.IUD(this, nDBOperation, nUserID);
        }

        public static SalesComPayment Get(int nSalesComPaymentID, long nUserID)
        {
            return SalesComPayment.Service.Get(nSalesComPaymentID, nUserID);
        }


        #endregion

        #region ServiceFactory
        internal static ISalesComPaymentService Service
        {
            get { return (ISalesComPaymentService)Services.Factory.CreateService(typeof(ISalesComPaymentService)); }
        }
        #endregion

    }
    #endregion

    #region ISUProductionProcess interface

    public interface ISalesComPaymentService
    {
        List<SalesComPayment> Gets(string sSQL, Int64 nUserID);
        SalesComPayment IUD(SalesComPayment oSalesComPayment, int nDBOperation, Int64 nUserID);
        SalesComPayment Get(int nSalesComPaymentID, Int64 nUserID);
    }
    #endregion


}
