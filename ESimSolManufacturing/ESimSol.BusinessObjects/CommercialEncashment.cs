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
	#region CommercialEncashment  
	public class CommercialEncashment : BusinessObject
	{	
		public CommercialEncashment()
		{
			CommercialEncashmentID = 0; 
			CommercialBSID = 0; 
			EncashmentDate = DateTime.Now; 
			ApprovedBy = 0; 
			ApprovedByName = ""; 
			AmountInCurrency = 0; 
			AmountBC = 0; 
			EncashRate = 0; 
			OverDueInCurrency = 0; 
			OverDueBC = 0; 
			GainLossCurrencyID = 0; 
			GainLossAmount = 0; 
			IsGain = true; 
			PDeductionInCurrency = 0; 
			PDeductionBC = 0; 
			Remarks = ""; 
			MasterLCNo = ""; 
			BankRefNo = ""; 
			FDBPNo = ""; 
			BuyerName = ""; 
			BankName = ""; 
			BSAmount = 0; 
			BSAmountBC = 0; 
			CRate = 0; 
			LCValue = 0; 
			PurchaseValue = 0; 
			BankCharge = 0;
            BankID = 0;
            GainLossCurrencySymbol = "";
            CurrencySymbol = "";
            BCurrencySymbol = "";
            BankChargeBC = 0;
            PurchaseAmountBC = 0;
			BSIssueDate = DateTime.Now; 
			SubmissionDate = DateTime.Now; 
			FDBPReceiveDate = DateTime.Now; 
			MaturityDate = DateTime.Now; 
			RealizationDate = DateTime.Now;
            Balance = 0;
            RemainingBalance = 0;
            RemainingBalanceBC = 0;
            LCCurrencyID = 0;
            CommercialEncashmentDetails = new List<CommercialEncashmentDetail>();
			ErrorMessage = "";
		}

		#region Property
		public int CommercialEncashmentID { get; set; }
		public int CommercialBSID { get; set; }
		public DateTime EncashmentDate { get; set; }
		public int ApprovedBy { get; set; }
		public string ApprovedByName { get; set; }
		public double AmountInCurrency { get; set; }
		public double AmountBC { get; set; }
		public double EncashRate { get; set; }
		public double OverDueInCurrency { get; set; }
		public double OverDueBC { get; set; }
		public int GainLossCurrencyID { get; set; }
		public double GainLossAmount { get; set; }
		public bool IsGain { get; set; }
		public double PDeductionInCurrency { get; set; }
		public double PDeductionBC { get; set; }
		public string Remarks { get; set; }
		public string MasterLCNo { get; set; }
		public string BankRefNo { get; set; }
		public string FDBPNo { get; set; }
		public string BuyerName { get; set; }
		public string BankName { get; set; }
		public double BSAmount { get; set; }
		public double BSAmountBC { get; set; }
		public double CRate { get; set; }
        public string CurrencySymbol { get; set; }
		public double LCValue { get; set; }
		public double PurchaseValue { get; set; }
		public double BankCharge { get; set; }
        public string GainLossCurrencySymbol { get; set; }
        public double RemainingBalance { get; set; }
        public double RemainingBalanceBC { get; set; }
        public int BankID { get; set; }
        public int LCCurrencyID { get; set; }
		public DateTime BSIssueDate { get; set; }
		public DateTime SubmissionDate { get; set; }
		public DateTime FDBPReceiveDate { get; set; }
		public DateTime MaturityDate { get; set; }
		public DateTime RealizationDate { get; set; }
        public double PurchaseAmountBC { get; set; }
        public double BankChargeBC { get; set; }
        public string BCurrencySymbol { get; set; }
        public double Balance { get; set; }
		public string ErrorMessage { get; set; }
		#endregion 

		#region Derived Property
        public List<CommercialEncashmentDetail> CommercialEncashmentDetails { get; set; }
        public CommercialBS CommercialBS { get; set; }
		public string EncashmentDateInString 
		{
			get
			{
				return EncashmentDate.ToString("dd MMM yyyy") ; 
			}
		}
		public string BSIssueDateInString 
		{
			get
			{
				return BSIssueDate.ToString("dd MMM yyyy") ; 
			}
		}
		public string SubmissionDateInString 
		{
			get
			{
				return SubmissionDate.ToString("dd MMM yyyy") ; 
			}
		}
		public string FDBPReceiveDateInString 
		{
			get
			{
				return FDBPReceiveDate.ToString("dd MMM yyyy") ; 
			}
		}
		public string MaturityDateInString 
		{
			get
			{
				return MaturityDate.ToString("dd MMM yyyy") ; 
			}
		}
		public string RealizationDateInString 
		{
			get
			{
				return RealizationDate.ToString("dd MMM yyyy") ; 
			}
		}
		#endregion 

		#region Functions 
		
		public static List<CommercialEncashment> Gets(string sSQL, long nUserID)
		{
			return CommercialEncashment.Service.Gets(sSQL,nUserID);
		}
		public CommercialEncashment Get(int id, long nUserID)
		{
			return CommercialEncashment.Service.Get(id,nUserID);
		}
		public CommercialEncashment Save(long nUserID)
		{
			return CommercialEncashment.Service.Save(this,nUserID);
		}
        public CommercialEncashment Approve(long nUserID)
        {
            return CommercialEncashment.Service.Approve(this, nUserID);
        }
		public  string  Delete(int id, long nUserID)
		{
			return CommercialEncashment.Service.Delete(id,nUserID);
		}
		#endregion

		#region ServiceFactory
		internal static ICommercialEncashmentService Service
		{
			get { return (ICommercialEncashmentService)Services.Factory.CreateService(typeof(ICommercialEncashmentService)); }
		}
		#endregion
	}
	#endregion

	#region ICommercialEncashment interface
	public interface ICommercialEncashmentService 
	{
		CommercialEncashment Get(int id, Int64 nUserID); 
	
		List<CommercialEncashment> Gets( string sSQL, Int64 nUserID);
		string Delete(int id, Int64 nUserID);
 		CommercialEncashment Save(CommercialEncashment oCommercialEncashment, Int64 nUserID);
        CommercialEncashment Approve(CommercialEncashment oCommercialEncashment, Int64 nUserID);
        
	}
	#endregion
}
