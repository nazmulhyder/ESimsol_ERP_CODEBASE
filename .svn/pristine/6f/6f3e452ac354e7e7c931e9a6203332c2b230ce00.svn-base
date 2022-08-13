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
	#region PaymentDetail  
	public class PaymentDetail : BusinessObject
	{	
		public PaymentDetail()
		{
            PaymentDetailID = 0;
            PaymentID = 0;
            ReferenceID = 0;
            PaymentAmount = 0;
            CurrencyID = 0; 
			Note = ""; 
            CRate = 0;
            InvoiceType = 0;
            DisCount = 0;
            AlreadyPaid = 0;
            PaymentAmount = 0;
            ContractorID = 0;
            SampleInvoiceNo = "";
            AmountInCurrency  = 0;
            ExchangeCurrencyID =   0;
            AmountWithExchangeRate = 0;
            SampleInvoiceDate = DateTime.Now;
            CurrencySymbol = "";
            ExchangeCurrencySymbol = "";
            AlreadyDiscount = 0;
            RateUnit = 1;
            ReferenceType = EnumSampleInvoiceType.None;
			ErrorMessage = "";
            ContractorName = "";
            AdditionalAmount = 0;
            AlreadyAdditionalAmount = 0;
            MRNo = "";
		}

		#region Property
		public int PaymentDetailID { get; set; }
        public int PaymentID { get; set; }
        public EnumSampleInvoiceType ReferenceType { get; set; }
        public int ReferenceTypeInInt { get; set; }
        public int ReferenceID { get; set; }
        public string SampleInvoiceNo { get; set; }
        public double PaymentAmount { get; set; }
        public double AdditionalAmount { get; set; }
		public string Note { get; set; }
        public double CRate { get; set; }
        public int CurrencyID { get; set; }
        public string CurrencySymbol { get; set; }
        public string ExchangeCurrencySymbol { get; set; }
        public int InvoiceType { get; set; }
        public double DisCount { get; set; }
        public double AlreadyPaid { get; set; }
        public double AmountWithExchangeRate { get; set; }
        public double AlreadyDiscount { get; set; }
        public double AlreadyAdditionalAmount { get; set; }
        public int ContractorID { get; set; }
        public int ExchangeCurrencyID { get; set; }
        public double AmountInCurrency { get; set; }
        public DateTime SampleInvoiceDate { get; set; }
        public int RateUnit { get; set; }
		public string ErrorMessage { get; set; }
		#endregion 

		#region Derived Property
        public string ContractorName { get; set; }
        public string MRNo { get; set; }
        public DateTime MRDate { get; set; }
        public double YetToPayment{get;set;}
        public int MKTEmpID { get; set; }
        public string MKTEmpName { get; set; }
        public double NetPayble
        {
            get
            {
                return (this.AmountWithExchangeRate ) - (this.DisCount);
            }
        }
        public string InvoiceTypeSt
        {
            get
            {
                return EnumObject.jGet((EnumSampleInvoiceType)this.InvoiceType);
            }
        }
        public string CRateInString
        {
            get
            {
                return Global.MillionFormat(this.CRate);
            }
        }
        public string AmountInCurrencyInString
        {
            get
            {
                return this.CurrencySymbol+" "+Global.MillionFormat(this.AmountInCurrency);
            }
        }

        public string AmountWithExchangeRateInString
        {
            get
            {
                return this.ExchangeCurrencySymbol + " " + Global.MillionFormat(this.AmountWithExchangeRate);
            }
        }
        public string SampleInvoiceDateInString
        {
            get
            {
                return this.SampleInvoiceDate.ToString("dd MMM yyyy");
            }
        }
		#endregion 

		#region Functions 
	
        
		public static List<PaymentDetail> Gets(string sSQL, long nUserID)
		{
			return PaymentDetail.Service.Gets(sSQL,nUserID);
		}
		public PaymentDetail Get(int id, long nUserID)
		{
			return PaymentDetail.Service.Get(id,nUserID);
		}
        public static List<PaymentDetail> Gets(int nPaymentID, long nUserID)
        {
            return PaymentDetail.Service.Gets(nPaymentID, nUserID);
        }
        public string Delete(long nUserID)
        {
            return PaymentDetail.Service.Delete(this, nUserID);
        }
	
		#endregion

		#region ServiceFactory
		internal static IPaymentDetailService Service
		{
			get { return (IPaymentDetailService)Services.Factory.CreateService(typeof(IPaymentDetailService)); }
		}
		#endregion
	}
	#endregion

	#region IPaymentDetail interface
	public interface IPaymentDetailService 
	{
		PaymentDetail Get(int id, Int64 nUserID);
        List<PaymentDetail> Gets(int nPaymentID, Int64 nUserID);
		List<PaymentDetail> Gets( string sSQL, Int64 nUserID);
        string Delete(PaymentDetail oPaymentDetail, long nUserID);
	
	}
	#endregion
}
