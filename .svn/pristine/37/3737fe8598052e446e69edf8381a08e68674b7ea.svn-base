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
	#region BUWiseParty  
	public class BUWiseParty : BusinessObject
	{	
		public BUWiseParty()
		{
			BUWisePartyID = 0; 
			BUID = 0; 
			ContractorID = 0; 
			BUName = ""; 
			ContractorName = ""; 
			ErrorMessage = "";
		}

		#region Property
		public int BUWisePartyID { get; set; }
		public int BUID { get; set; }
		public int ContractorID { get; set; }
		public string BUName { get; set; }
		public string ContractorName { get; set; }
		public string ErrorMessage { get; set; }
		#endregion 

		#region Derived Property
		#endregion 

		#region Functions 
        public static List<BUWiseParty> Gets(int nID, long nUserID) //nID is contractor ID
		{
            return BUWiseParty.Service.Gets(nID, nUserID);
		}
		public static List<BUWiseParty> Gets(string sSQL, long nUserID)
		{
			return BUWiseParty.Service.Gets(sSQL,nUserID);
		}
		public BUWiseParty Get(int id, long nUserID)
		{
			return BUWiseParty.Service.Get(id,nUserID);
		}
		public BUWiseParty Save(long nUserID)
		{
			return BUWiseParty.Service.Save(this,nUserID);
		}
		public  string  Delete(int id, long nUserID)
		{
			return BUWiseParty.Service.Delete(id,nUserID);
		}
		#endregion

		#region ServiceFactory
		internal static IBUWisePartyService Service
		{
			get { return (IBUWisePartyService)Services.Factory.CreateService(typeof(IBUWisePartyService)); }
		}
		#endregion
	}
	#endregion

	#region IBUWiseParty interface
	public interface IBUWisePartyService 
	{
		BUWiseParty Get(int id, Int64 nUserID); 
		List<BUWiseParty> Gets(int nID, Int64 nUserID);
		List<BUWiseParty> Gets( string sSQL, Int64 nUserID);
		string Delete(int id, Int64 nUserID);
 		BUWiseParty Save(BUWiseParty oBUWiseParty, Int64 nUserID);
	}
	#endregion
}
