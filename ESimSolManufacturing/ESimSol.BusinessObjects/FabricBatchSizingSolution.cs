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
	#region FabricBatchSizingSolution  
	public class FabricBatchSizingSolution : BusinessObject
	{	
		public FabricBatchSizingSolution()
		{
			FBID = 0; 
			WaterQty = 0; 
			Dry = 0; 
			Wet = 0; 
			RF = 0; 
			Viscosity = 0; 
			FinalVolume = 0; 
			RestQty = 0; 
			PreviousRestQty = 0; 
			ErrorMessage = "";
            SugPrevRestQty = 0;
		}

		#region Property
		public int FBID { get; set; }
		public double WaterQty { get; set; }
		public double Dry { get; set; }
		public double Wet { get; set; }
		public double RF { get; set; }
		public double Viscosity { get; set; }
		public double FinalVolume { get; set; }
		public double RestQty { get; set; }
		public double PreviousRestQty { get; set; }
		public string ErrorMessage { get; set; }
		#endregion 

		#region Derived Property
        public double SugPrevRestQty { get; set; }
		#endregion 

		#region Functions 
		public static List<FabricBatchSizingSolution> Gets(long nUserID)
		{
			return FabricBatchSizingSolution.Service.Gets(nUserID);
		}
		public static List<FabricBatchSizingSolution> Gets(string sSQL, long nUserID)
		{
			return FabricBatchSizingSolution.Service.Gets(sSQL,nUserID);
		}
		public static FabricBatchSizingSolution Get(int id, long nUserID)
		{
			return FabricBatchSizingSolution.Service.Get(id,nUserID);
		}
		public FabricBatchSizingSolution Save(long nUserID)
		{
			return FabricBatchSizingSolution.Service.Save(this,nUserID);
		}
        public static double GetPrevQtyForSizing(long nUserID)
        {
            return FabricBatchSizingSolution.Service.GetPrevQtyForSizing(nUserID);
        }
		public  string  Delete(int id, long nUserID)
		{
			return FabricBatchSizingSolution.Service.Delete(id,nUserID);
		}
		#endregion

		#region ServiceFactory
		internal static IFabricBatchSizingSolutionService Service
		{
			get { return (IFabricBatchSizingSolutionService)Services.Factory.CreateService(typeof(IFabricBatchSizingSolutionService)); }
		}
		#endregion
	}
	#endregion

	#region IFabricBatchSizingSolution interface
	public interface IFabricBatchSizingSolutionService 
	{
		FabricBatchSizingSolution Get(int id, Int64 nUserID); 
		List<FabricBatchSizingSolution> Gets(Int64 nUserID);
		List<FabricBatchSizingSolution> Gets( string sSQL, Int64 nUserID);
		string Delete(int id, Int64 nUserID);
 		FabricBatchSizingSolution Save(FabricBatchSizingSolution oFabricBatchSizingSolution, Int64 nUserID);
        double GetPrevQtyForSizing(Int64 nUserID);
	}
	#endregion
}
