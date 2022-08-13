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
	#region KnitDyeingBatchGrayChallan  
	public class KnitDyeingBatchGrayChallan : BusinessObject
	{	
		public KnitDyeingBatchGrayChallan()
		{
			KnitDyeingBatchGrayChallanID = 0; 
			KnitDyeingBatchDetailID = 0; 
			GrayFabricID = 0; 
			StoreID = 0; 
			StoreName = ""; 
			LotID = 0; 
			LotNo = ""; 
			MUnitID = 0; 
			UnitName = "";
            FabricName = "";
			Qty = 0; 
			Remarks = ""; 
			DisburseBy = 0; 
			DisburseByName = ""; 
			ErrorMessage = "";
		}

		#region Property
		public int KnitDyeingBatchGrayChallanID { get; set; }
		public int KnitDyeingBatchDetailID { get; set; }
		public int GrayFabricID { get; set; }
		public int StoreID { get; set; }
		public string StoreName { get; set; }
		public int LotID { get; set; }
		public string LotNo { get; set; }
		public int MUnitID { get; set; }
		public string UnitName { get; set; }
		public double Qty { get; set; }
		public string Remarks { get; set; }
		public int DisburseBy { get; set; }
		public string DisburseByName { get; set; }
		public string ErrorMessage { get; set; }
		#endregion 

		#region Derived Property
        public string FabricName { get; set; }
		#endregion 

		#region Functions 
		public static List<KnitDyeingBatchGrayChallan> Gets(long nUserID)
		{
			return KnitDyeingBatchGrayChallan.Service.Gets(nUserID);
		}
		public static List<KnitDyeingBatchGrayChallan> Gets(string sSQL, long nUserID)
		{
			return KnitDyeingBatchGrayChallan.Service.Gets(sSQL,nUserID);
		}
		public KnitDyeingBatchGrayChallan Get(int id, long nUserID)
		{
			return KnitDyeingBatchGrayChallan.Service.Get(id,nUserID);
		}
		public KnitDyeingBatchGrayChallan Save(long nUserID)
		{
			return KnitDyeingBatchGrayChallan.Service.Save(this,nUserID);
		}
		public  string  Delete(int id, long nUserID)
		{
			return KnitDyeingBatchGrayChallan.Service.Delete(id,nUserID);
		}
		#endregion

		#region ServiceFactory
		internal static IKnitDyeingBatchGrayChallanService Service
		{
			get { return (IKnitDyeingBatchGrayChallanService)Services.Factory.CreateService(typeof(IKnitDyeingBatchGrayChallanService)); }
		}
		#endregion
	}
	#endregion

	#region IKnitDyeingBatchGrayChallan interface
	public interface IKnitDyeingBatchGrayChallanService 
	{
		KnitDyeingBatchGrayChallan Get(int id, Int64 nUserID); 
		List<KnitDyeingBatchGrayChallan> Gets(Int64 nUserID);
		List<KnitDyeingBatchGrayChallan> Gets( string sSQL, Int64 nUserID);
		string Delete(int id, Int64 nUserID);
 		KnitDyeingBatchGrayChallan Save(KnitDyeingBatchGrayChallan oKnitDyeingBatchGrayChallan, Int64 nUserID);
	}
	#endregion
}
