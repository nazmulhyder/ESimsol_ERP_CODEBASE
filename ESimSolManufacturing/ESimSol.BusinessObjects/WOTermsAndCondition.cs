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
	#region WOTermsAndCondition  
	public class WOTermsAndCondition : BusinessObject
	{	
		public WOTermsAndCondition()
		{
			WOTermsAndConditionID = 0; 
			WOID = 0; 
			TermsAndCondition = ""; 
			ErrorMessage = "";
		}

		#region Property
		public int WOTermsAndConditionID { get; set; }
		public int WOID { get; set; }
		public string TermsAndCondition { get; set; }
		public string ErrorMessage { get; set; }
		#endregion 

		#region Derived Property
		#endregion 

		#region Functions 
		public static List<WOTermsAndCondition> Gets(int nWOID, long nUserID)
		{
			return WOTermsAndCondition.Service.Gets(nWOID, nUserID);
		}
		public static List<WOTermsAndCondition> Gets(string sSQL, long nUserID)
		{
			return WOTermsAndCondition.Service.Gets(sSQL,nUserID);
		}
		public WOTermsAndCondition Get(int id, long nUserID)
		{
			return WOTermsAndCondition.Service.Get(id,nUserID);
		}
		
		public  string  Delete(int id, long nUserID)
		{
			return WOTermsAndCondition.Service.Delete(id,nUserID);
		}
		#endregion

		#region ServiceFactory
		internal static IWOTermsAndConditionService Service
		{
			get { return (IWOTermsAndConditionService)Services.Factory.CreateService(typeof(IWOTermsAndConditionService)); }
		}
		#endregion
	}
	#endregion

	#region IWOTermsAndCondition interface
	public interface IWOTermsAndConditionService 
	{
		WOTermsAndCondition Get(int id, Int64 nUserID); 
		List<WOTermsAndCondition> Gets(int nWOID, Int64 nUserID);
		List<WOTermsAndCondition> Gets( string sSQL, Int64 nUserID);
		string Delete(int id, Int64 nUserID);
 	
	}
	#endregion
}
