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
	#region CommercialFDBPDetail  
	public class CommercialFDBPDetail : BusinessObject
	{	
		public CommercialFDBPDetail()
		{
			CommercialFDBPDetailID = 0; 
			CommercialFDBPID = 0; 
			BankAccountID = 0; 
			AmountInCurrency = 0; 
			CRate = 0; 
			AmountBC = 0; 
			Remarks = ""; 
			BankName = ""; 
			BankAccountNo = ""; 
			ErrorMessage = "";
		}

		#region Property
		public int CommercialFDBPDetailID { get; set; }
		public int CommercialFDBPID { get; set; }
		public int BankAccountID { get; set; }
		public double AmountInCurrency { get; set; }
		public double CRate { get; set; }
		public double AmountBC { get; set; }
		public string Remarks { get; set; }
		public string BankName { get; set; }
		public string BankAccountNo { get; set; }
		public string ErrorMessage { get; set; }
		#endregion 

		#region Derived Property
		#endregion 

		#region Functions 
		public static List<CommercialFDBPDetail> Gets(int CommercialFDBPID, long nUserID)
		{
			return CommercialFDBPDetail.Service.Gets(CommercialFDBPID, nUserID);
		}
		public static List<CommercialFDBPDetail> Gets(string sSQL, long nUserID)
		{
			return CommercialFDBPDetail.Service.Gets(sSQL,nUserID);
		}
		
		#endregion

		#region ServiceFactory
		internal static ICommercialFDBPDetailService Service
		{
			get { return (ICommercialFDBPDetailService)Services.Factory.CreateService(typeof(ICommercialFDBPDetailService)); }
		}
		#endregion
	}
	#endregion

	#region ICommercialFDBPDetail interface
	public interface ICommercialFDBPDetailService 
	{
		List<CommercialFDBPDetail> Gets(int CommercialFDBPID, Int64 nUserID);
		List<CommercialFDBPDetail> Gets( string sSQL, Int64 nUserID);
	
	}
	#endregion
}
