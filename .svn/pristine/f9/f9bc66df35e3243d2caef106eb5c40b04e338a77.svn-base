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
	#region FabricPlanCount  
	public class FabricPlanCount : BusinessObject
	{	
		public FabricPlanCount()
		{
			FabricPlanCountID = 0; 
			FabricPlanningID = 0; 
			SLNo = 0;
            YarnCount = 0;
            TotalYarnCount = 0;
            Repeat = ""; 
			RepeatCount = 0; 
			ErrorMessage = "";
		}

		#region Property
		public int FabricPlanCountID { get; set; }
		public int FabricPlanningID { get; set; }
        public int SLNo { get; set; }
        public int YarnCount { get; set; }
        public int TotalYarnCount { get; set; }
		public string Repeat { get; set; }
		public int RepeatCount { get; set; }
		public string ErrorMessage { get; set; }
		#endregion 

		#region Derived Property
		#endregion 

		#region Functions 
		public static List<FabricPlanCount> Gets(long nUserID)
		{
			return FabricPlanCount.Service.Gets(nUserID);
		}
		public static List<FabricPlanCount> Gets(string sSQL, long nUserID)
		{
			return FabricPlanCount.Service.Gets(sSQL,nUserID);
		}
		public FabricPlanCount Get(int id, long nUserID)
		{
			return FabricPlanCount.Service.Get(id,nUserID);
		}
		public FabricPlanCount Save(long nUserID)
		{
			return FabricPlanCount.Service.Save(this,nUserID);
		}
		public  string  Delete(int id, long nUserID)
		{
			return FabricPlanCount.Service.Delete(id,nUserID);
		}
		#endregion

		#region ServiceFactory
		internal static IFabricPlanCountService Service
		{
			get { return (IFabricPlanCountService)Services.Factory.CreateService(typeof(IFabricPlanCountService)); }
		}
		#endregion

    }
	#endregion

	#region IFabricPlanCount interface
	public interface IFabricPlanCountService 
	{
		FabricPlanCount Get(int id, Int64 nUserID); 
		List<FabricPlanCount> Gets(Int64 nUserID);
		List<FabricPlanCount> Gets( string sSQL, Int64 nUserID);
		string Delete(int id, Int64 nUserID);
 		FabricPlanCount Save(FabricPlanCount oFabricPlanCount, Int64 nUserID);
	}
	#endregion
}
