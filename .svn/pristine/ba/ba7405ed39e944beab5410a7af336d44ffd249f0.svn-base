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
	#region Commercial Encashment Detail  
	public class CommercialEncashmentDetail : BusinessObject
	{	
		public CommercialEncashmentDetail()
		{
			CommercialEncashmentDetailID = 0; 
			CommercialEncashmentID = 0; 
			BankAccountID = 0; 
			BankAccountNo = ""; 
			ExpenditureHeadID = 0; 
			ExpenditureHeadName = ""; 
			CurrencyID = 0; 
			AmountInCurrency = 0; 
			CRate = 0; 
			AmountBC = 0; 
			CurrencySymbol = ""; 
			ErrorMessage = "";
		}

		#region Property
		public int CommercialEncashmentDetailID { get; set; }
		public int CommercialEncashmentID { get; set; }
		public int BankAccountID { get; set; }
		public string BankAccountNo { get; set; }
		public int ExpenditureHeadID { get; set; }
		public string ExpenditureHeadName { get; set; }
		public int CurrencyID { get; set; }
		public double AmountInCurrency { get; set; }
		public double CRate { get; set; }
		public double AmountBC { get; set; }
		public string CurrencySymbol { get; set; }
		public string ErrorMessage { get; set; }
		#endregion 

		#region Derived Property
		#endregion 

		#region Functions 
		public static List<CommercialEncashmentDetail> Gets(int CommercialEncashmentID,  long nUserID)
		{
			return CommercialEncashmentDetail.Service.Gets(CommercialEncashmentID, nUserID);
		}
		public static List<CommercialEncashmentDetail> Gets(string sSQL, long nUserID)
		{
			return CommercialEncashmentDetail.Service.Gets(sSQL,nUserID);
		}
		
		#endregion

		#region ServiceFactory
		internal static ICommercialEncashmentDetailService Service
		{
			get { return (ICommercialEncashmentDetailService)Services.Factory.CreateService(typeof(ICommercialEncashmentDetailService)); }
		}
		#endregion
	}
	#endregion

	#region ICommercialEncashmentDetail interface
	public interface ICommercialEncashmentDetailService 
	{
		List<CommercialEncashmentDetail> Gets(int CommercialEncashmentID, Int64 nUserID);
		List<CommercialEncashmentDetail> Gets( string sSQL, Int64 nUserID);
	
	}
	#endregion
}
