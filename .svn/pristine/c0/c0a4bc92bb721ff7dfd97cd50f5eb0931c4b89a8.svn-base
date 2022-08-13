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
	#region BUWiseConsumptionUnit  
	public class BUWiseConsumptionUnit : BusinessObject
	{	
		public BUWiseConsumptionUnit()
		{
			BUWiseConsumptionUnitID = 0; 
			BUID = 0; 
			ConsumptionUnitID = 0; 
			BUName = ""; 
			ConsumptionUnitName = ""; 
			ErrorMessage = "";
		}

		#region Property
		public int BUWiseConsumptionUnitID { get; set; }
		public int BUID { get; set; }
		public int ConsumptionUnitID { get; set; }
		public string BUName { get; set; }
		public string ConsumptionUnitName { get; set; }
        public string ConsumptionUnitNote { get; set; }
		public string ErrorMessage { get; set; }
		#endregion 

		#region Derived Property
		#endregion 

		#region Functions 
        public static List<BUWiseConsumptionUnit> Gets(int nID, long nUserID) //nID is ConsumptionUnit ID
		{
            return BUWiseConsumptionUnit.Service.Gets(nID, nUserID);
		}
		public static List<BUWiseConsumptionUnit> Gets(string sSQL, long nUserID)
		{
			return BUWiseConsumptionUnit.Service.Gets(sSQL,nUserID);
		}
		public BUWiseConsumptionUnit Get(int id, long nUserID)
		{
			return BUWiseConsumptionUnit.Service.Get(id,nUserID);
		}
		public BUWiseConsumptionUnit Save(long nUserID)
		{
			return BUWiseConsumptionUnit.Service.Save(this,nUserID);
		}
		public  string  Delete(int id, long nUserID)
		{
			return BUWiseConsumptionUnit.Service.Delete(id,nUserID);
		}
		#endregion

		#region ServiceFactory
		internal static IBUWiseConsumptionUnitService Service
		{
			get { return (IBUWiseConsumptionUnitService)Services.Factory.CreateService(typeof(IBUWiseConsumptionUnitService)); }
		}
		#endregion
	}
	#endregion

	#region IBUWiseConsumptionUnit interface
	public interface IBUWiseConsumptionUnitService 
	{
		BUWiseConsumptionUnit Get(int id, Int64 nUserID); 
		List<BUWiseConsumptionUnit> Gets(int nID, Int64 nUserID);
		List<BUWiseConsumptionUnit> Gets( string sSQL, Int64 nUserID);
		string Delete(int id, Int64 nUserID);
 		BUWiseConsumptionUnit Save(BUWiseConsumptionUnit oBUWiseConsumptionUnit, Int64 nUserID);
	}
	#endregion
}
