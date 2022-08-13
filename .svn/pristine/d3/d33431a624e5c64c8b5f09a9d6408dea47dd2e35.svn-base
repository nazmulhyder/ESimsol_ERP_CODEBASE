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
	#region CommercialBS  
	public class CommercialBS : BusinessObject
	{	
		public CommercialBS()
		{
			CommercialBSID = 0; 
			MasterLCID = 0; 
			RefNo = ""; 
			BUID = 0; 
			BuyerID = 0;
            BSStatus = EnumCommercialBSStatus.Intialized; 
			BankID = 0; 
			BSAmount = 0; 
			Remarks = ""; 
			SubmissionDate = DateTime.Now; 
			SubmissionRemarks = ""; 
			FDBPNo = ""; 
			BankRefNo = ""; 
			FDBPReceiveDate = DateTime.Now; 
			FDBPRemarks = ""; 
			MaturityRcvDate = DateTime.Now; 
			MaturityDate = DateTime.Now; 
			MaturityRemarks = ""; 
			RealizationDate = DateTime.Now;
            IssueDate = DateTime.Now;
			RealizationRemarks = ""; 
			BankName = ""; 
			BuyerName = ""; 
			BUName = ""; 
			MasterLCNo = ""; 
			BankBranchID = 0;
            CommercialFDBPID = 0;
            ApprovedBy = 0;
            ApprovedByName = "";
			BankBranchName = "";
            CurrencySymbol = "";
            BCurrencyID = 0;
            BSAmountBC = 0;
            CRate = 0;
            LCValue = 0;
            BankCharge = 0;
            PurchaseAmount = 0;
            BCurrencySymbol = "";
            CommercialEncashmentID = 0;
            BankChargeBC = 0;
            PurchaseAmountBC = 0;
            PurchseApproveBy = 0;
            EncashApproveBy = 0;
            PurchseApproveByName = "";
            EncashApproveByName = "";
            DynamicDate = DateTime.Today;
            ActionType = EnumCommercialBSActionType.Intialized;
            CommercialBSDetails = new List<CommercialBSDetail>();
            SearchingData = "";
			ErrorMessage = "";
		}

		#region Property
		public int CommercialBSID { get; set; }
		public int MasterLCID { get; set; }
		public string RefNo { get; set; }
		public int BUID { get; set; }
		public int BuyerID { get; set; }
        public EnumCommercialBSStatus BSStatus { get; set; }
        public DateTime IssueDate { get; set; }
		public int BankID { get; set; }
		public double BSAmount { get; set; }
		public string Remarks { get; set; }
		public DateTime SubmissionDate { get; set; }
		public string SubmissionRemarks { get; set; }
		public string FDBPNo { get; set; }
		public string BankRefNo { get; set; }
        public int ApprovedBy { get; set; }
        public string ApprovedByName { get; set; }
        public double PurchaseAmountBC { get; set; }
        public double BankChargeBC { get; set; }
		public DateTime FDBPReceiveDate { get; set; }
		public string FDBPRemarks { get; set; }
		public DateTime MaturityRcvDate { get; set; }
		public DateTime MaturityDate { get; set; }
		public string MaturityRemarks { get; set; }
		public DateTime RealizationDate { get; set; }
		public string RealizationRemarks { get; set; }
		public string BankName { get; set; }
		public string BuyerName { get; set; }
		public string BUName { get; set; }
		public string MasterLCNo { get; set; }
		public int BankBranchID { get; set; }
        public int CommercialFDBPID { get; set; }
        public int AdviceBankID { get; set; }
        public int CurrencyID { get; set; }
		public string BankBranchName { get; set; }
        public string SearchingData { get; set; }
        public int BCurrencyID { get; set; }
        public double BSAmountBC { get; set; }
        public double CRate { get; set; }
        public string BCurrencySymbol { get; set; }
        public double LCValue { get; set; }
        public double BankCharge { get; set; }
        public double PurchaseAmount { get; set; }
        public int  PurchseApproveBy  { get; set; }
        public int      EncashApproveBy  { get; set; }
         public string    PurchseApproveByName  { get; set; }
         public string EncashApproveByName { get; set; }
		public string ErrorMessage { get; set; }
		#endregion 

		#region Derived Property
        public DateTime DynamicDate { get; set; }
        public EnumCommercialBSActionType ActionType { get; set; }
        public List<CommercialBSDetail> CommercialBSDetails { get; set; }
        public string ActionTypeInString { get; set; }
        public int BSStatusInInt { get; set; }
        public string CurrencySymbol { get; set; }
        public int CommercialEncashmentID { get; set; }
        public string BSAmountST
        {
            get
            {
                return  this.CurrencySymbol+Global.MillionFormat(this.BSAmount);
            }
        }
        public string BSStatusST
        {
            get
            {
                return EnumObject.jGet(this.BSStatus);
            }
        }
        public string IssueDateInString
        {
            get
            {
                if (this.IssueDate == DateTime.MinValue)
                {
                    return "";
                }
                else
                {
                    return IssueDate.ToString("dd MMM yyyy");
                }
            }
        }
		public string SubmissionDateInString 
		{
			get
			{
                if (this.SubmissionDate == DateTime.MinValue)
                {
                    return "";
                }
                else
                {
                    return SubmissionDate.ToString("dd MMM yyyy");
                }
			}
		}
		public string FDBPReceiveDateInString 
		{
			get
			{
                if (this.FDBPReceiveDate == DateTime.MinValue)
                {
                    return "";
                }
                else
                {
                    return FDBPReceiveDate.ToString("dd MMM yyyy");
                }
			}
		}
		public string MaturityRcvDateInString 
		{
			get
			{
                if (this.MaturityRcvDate == DateTime.MinValue)
                {
                    return "";
                }
                else
                {
                    return MaturityRcvDate.ToString("dd MMM yyyy");
                }
			}
		}
		public string MaturityDateInString 
		{
			get
			{
                if (this.MaturityDate == DateTime.MinValue)
                {
                    return "";
                }
                else
                {
                    return MaturityDate.ToString("dd MMM yyyy");
                }
			}
		}
		public string RealizationDateInString 
		{
			get
			{
                if (this.RealizationDate == DateTime.MinValue)
                {
                    return "";
                }
                else
                {
                    return RealizationDate.ToString("dd MMM yyyy");
                }
			}
		}
      
		#endregion 

		#region Functions 

		public static List<CommercialBS> Gets(string sSQL, long nUserID)
		{
			return CommercialBS.Service.Gets(sSQL,nUserID);
		}
		public CommercialBS Get(int id, long nUserID)
		{
			return CommercialBS.Service.Get(id,nUserID);
		}
		public CommercialBS Save(long nUserID)
		{
			return CommercialBS.Service.Save(this,nUserID);
		}
        public CommercialBS ChangeStatus(long nUserID)
        {
            return CommercialBS.Service.ChangeStatus(this, nUserID);
        }
		public  string  Delete(int id, long nUserID)
		{
			return CommercialBS.Service.Delete(id,nUserID);
		}
		#endregion

		#region ServiceFactory
		internal static ICommercialBSService Service
		{
			get { return (ICommercialBSService)Services.Factory.CreateService(typeof(ICommercialBSService)); }
		}
		#endregion
	}
	#endregion

	#region ICommercialBS interface
	public interface ICommercialBSService 
	{
		CommercialBS Get(int id, Int64 nUserID); 
		List<CommercialBS> Gets( string sSQL, Int64 nUserID);
		string Delete(int id, Int64 nUserID);
 		CommercialBS Save(CommercialBS oCommercialBS, Int64 nUserID);
        CommercialBS ChangeStatus(CommercialBS oCommercialBS, Int64 nUserID);
        
	}
	#endregion
}
