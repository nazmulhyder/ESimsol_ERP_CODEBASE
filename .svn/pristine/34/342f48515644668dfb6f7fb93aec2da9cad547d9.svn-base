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
	#region MarketingAccount_BU  
	public class MarketingAccount_BU : BusinessObject
	{	
		public MarketingAccount_BU()
		{
			MarketingAccount_BUID = 0; 
			BUID = 0;
            MarketingAccountID = 0; 
			BUName = "";
            Name = ""; 
			ErrorMessage = "";
		}

		#region Property
		public int MarketingAccount_BUID { get; set; }
		public int BUID { get; set; }
        public int MarketingAccountID { get; set; }
		public string BUName { get; set; }
        public string Name { get; set; }
        public string Params { get; set; }
		public string ErrorMessage { get; set; }
		#endregion 

		#region Derived Property
		#endregion 

		#region Functions 
        public static List<MarketingAccount_BU> Gets(int nID, long nUserID) //nID is ProductCategory ID
		{
            return MarketingAccount_BU.Service.Gets(nID, nUserID);
		}
		public static List<MarketingAccount_BU> Gets(string sSQL, long nUserID)
		{
			return MarketingAccount_BU.Service.Gets(sSQL,nUserID);
		}
		public MarketingAccount_BU Get(int id, long nUserID)
		{
			return MarketingAccount_BU.Service.Get(id,nUserID);
		}
		public MarketingAccount_BU Save(long nUserID)
		{
			return MarketingAccount_BU.Service.Save(this,nUserID);
		}
		public  string  Delete(int id, long nUserID)
		{
			return MarketingAccount_BU.Service.Delete(id,nUserID);
		}
		#endregion

		#region ServiceFactory
		internal static IMarketingAccount_BUService Service
		{
			get { return (IMarketingAccount_BUService)Services.Factory.CreateService(typeof(IMarketingAccount_BUService)); }
		}
		#endregion
	}
	#endregion

	#region IMarketingAccount_BU interface
	public interface IMarketingAccount_BUService 
	{
		MarketingAccount_BU Get(int id, Int64 nUserID); 
		List<MarketingAccount_BU> Gets(int nID, Int64 nUserID);
		List<MarketingAccount_BU> Gets( string sSQL, Int64 nUserID);
		string Delete(int id, Int64 nUserID);
 		MarketingAccount_BU Save(MarketingAccount_BU oMarketingAccount_BU, Int64 nUserID);
	}
	#endregion
}
