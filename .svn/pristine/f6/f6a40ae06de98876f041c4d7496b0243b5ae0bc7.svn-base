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
	#region CommercialFDBP  
	public class CommercialFDBP : BusinessObject
	{	
		public CommercialFDBP()
		{
			CommercialFDBPID = 0; 
			CommercialBSID = 0; 
			PurchaseDate = DateTime.Now; 
			PurchaseAmount = 0; 
			BankCharge = 0; 
			CRate = 0; 
			CurrencyID = 0; 
			CurrencySymbol = ""; 
			Remarks = "";
            BSAmount = 0;
            BuyerName = "";
            MasterLCNo = "";
            BankID = 0;
            ApprovedBy = 0;
            FDBPNo = "";
            BankRefNo = "";
            ApprovedByName = "";
            CommercialFDBPDetails = new List<CommercialFDBPDetail>();
            CommercialBS = new BusinessObjects.CommercialBS(); 
			ErrorMessage = "";
		}

		#region Property
		public int CommercialFDBPID { get; set; }
		public int CommercialBSID { get; set; }
		public DateTime PurchaseDate { get; set; }
		public double PurchaseAmount { get; set; }
		public double BankCharge { get; set; }
		public double CRate { get; set; }
		public int CurrencyID { get; set; }
		public string CurrencySymbol { get; set; }
		public string Remarks { get; set; }
        public string BuyerName { get; set; }
        public string MasterLCNo { get; set; }
        public double BSAmount { get; set; }
        public int BankID { get; set; }
        public int ApprovedBy { get; set; }
        public string ApprovedByName { get; set; }
        public string FDBPNo { get; set; }
        public string BankRefNo { get; set; }
		public string ErrorMessage { get; set; }
		#endregion 

		#region Derived Property 
        public List<CommercialFDBPDetail> CommercialFDBPDetails { get; set; }
        public CommercialBS CommercialBS { get; set; }
		public string PurchaseDateInString 
		{
			get
			{
				return PurchaseDate.ToString("dd MMM yyyy") ; 
			}
		}
		#endregion 

		#region Functions 
		
		public static List<CommercialFDBP> Gets(string sSQL, long nUserID)
		{
			return CommercialFDBP.Service.Gets(sSQL,nUserID);
		}
		public CommercialFDBP Get(int id, long nUserID)
		{
			return CommercialFDBP.Service.Get(id,nUserID);
		}
		public CommercialFDBP Save(long nUserID)
		{
			return CommercialFDBP.Service.Save(this,nUserID);
		}
        public CommercialFDBP Approve(long nUserID)
        {
            return CommercialFDBP.Service.Approve(this, nUserID);
        }
		public  string  Delete(int id, long nUserID)
		{
			return CommercialFDBP.Service.Delete(id,nUserID);
		}
		#endregion

		#region ServiceFactory
		internal static ICommercialFDBPService Service
		{
			get { return (ICommercialFDBPService)Services.Factory.CreateService(typeof(ICommercialFDBPService)); }
		}
		#endregion
	}
	#endregion

	#region ICommercialFDBP interface
	public interface ICommercialFDBPService 
	{
		CommercialFDBP Get(int id, Int64 nUserID); 
		List<CommercialFDBP> Gets( string sSQL, Int64 nUserID);
		string Delete(int id, Int64 nUserID);
 		CommercialFDBP Save(CommercialFDBP oCommercialFDBP, Int64 nUserID);
        CommercialFDBP Approve(CommercialFDBP oCommercialFDBP, Int64 nUserID);
        
	}
	#endregion
}
