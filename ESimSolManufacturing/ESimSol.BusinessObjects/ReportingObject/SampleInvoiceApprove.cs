using System;
using System.IO;
using ESimSol.BusinessObjects;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Collections.Generic;
using System.Data;
using ICS.Core;
using ICS.Core.Framework;
using ICS.Core.Utility;

namespace ESimSol.BusinessObjects
{
    #region SampleInvoiceApprove
    public class SampleInvoiceApprove : BusinessObject
    {
        #region  Constructor
        public SampleInvoiceApprove()
        {
            
            SampleInvoiceDate=DateTime.Today;
            ContractorName="";
            ProductName="";
            Color="";
            Qty=0.0;
            UnitPrice=0.0;
            Amount=0.0;
            MKTPName="";
            SampleInvoiceNo = "";
            PINo="";
            Outstanding = 0.0;
            ApproveByName = "";
            EndDate = DateTime.Today;
            ErrorMessage = "";
            

        }
        #endregion

        #region Properties  
        
        public DateTime SampleInvoiceDate { get; set; }
        public string SampleInvoiceNo { get; set; }
        public string ContractorName { get; set; }
        public string ProductName { get; set; }
        public string Color { get; set; }
        public double Qty { get; set; }
        public double UnitPrice { get; set; }
         public double Amount { get; set; }
        public string MKTPName { get; set; }
        public string PartyConcernPerson { get; set; }
        public string PINo { get; set; }
        public double Outstanding { get; set; }
        public double SampleInvoiceID { get; set; }
        public double ContractorID { get; set; }
        public string ApproveByName { get; set; }
         public string LCNo { get; set; }
         public string Currency { get; set; }
        public int CurrentStatus { get; set; }
        public int PaymentType { get; set; }
        public int SampleInvoiceType { get; set; }
        public int DateType { get; set; }
        public int ReportType { get; set; }
        public DateTime EndDate { get; set; }
        public string Param { get; set; }
        public string ErrorMessage { get; set; }
        #endregion

        #region Derive Property


        public string SampleInvoiceDateSt
        {
            get
            {
                return (this.SampleInvoiceDate.ToString("dd MMM yyyy"));
            }
        }


        public string QtySt
        {
            get
            {
                return Global.MillionFormat(this.Qty);
            }

        }
        public string UnitPriceSt
        {
            get
            {
                return Global.MillionFormat(this.UnitPrice);
            }

        }
        public string AmountSt
        {
            get
            {
                return this.Currency+""+Global.MillionFormat(this.Amount);
            }

        }

        public string OutstandingInSt
        {
            get
            {
                return this.SampleInvoiceID+"~"+ Global.MillionFormat(this.Outstanding);
            }

        }
        public string CurrentStatusSt
        {
            get
            {
                return ((EnumSampleInvoiceStatus)CurrentStatus).ToString();
            }
        }
        public string PaymentTypeSt
        {
            get
            {
                return ((EnumOrderPaymentType)this.PaymentType).ToString();
            }
        }
        //public string InvoiceTypeSt
        //{
        //    get
        //    {
        //        return ((EnumSampleInvoiceType)this.SampleInvoiceType).ToString();
        //    }
        //}
      
        #endregion

        #region Functions
    
        public static List<SampleInvoiceApprove> Gets(string sSQL, int nReportType, long nUserID)
        {
            return SampleInvoiceApprove.Service.Gets( sSQL, nReportType,  nUserID);
        }
      
        #endregion

        #region Non DB Function

        #endregion
        
        #region ServiceFactory
        internal static ISampleInvoiceApproveService Service
        {
            get { return (ISampleInvoiceApproveService)Services.Factory.CreateService(typeof(ISampleInvoiceApproveService)); }
        }
        #endregion
    }
    #endregion

    
    #region ISampleInvoiceApprove interface
    public interface ISampleInvoiceApproveService
    {
        //List<SampleInvoiceApprove> Gets(DateTime dStartDate, DateTime dEndDate, int nCurrentStatus, int nReportType, Int64 nUserID);
        List<SampleInvoiceApprove> Gets(string sSQL, int nReportType, Int64 nUserID);
    }
    
    #endregion

}