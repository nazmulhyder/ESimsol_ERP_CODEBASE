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
	#region KnitDyeingGrayChallanDetail  
	public class KnitDyeingGrayChallanDetail : BusinessObject
	{	
		public KnitDyeingGrayChallanDetail()
		{
			KnitDyeingGrayChallanDetailID = 0; 
			KnitDyeingGrayChallanID = 0; 
			KnitDyeingBatchDetailID = 0; 
			GrayFabricID = 0; 
			StoreID = 0; 
			LotID = 0; 
			MUnitID = 0; 
			Qty = 0; 
			Remarks = ""; 
			StoreName = ""; 
			LotNo = "";
            UnitName = ""; 
			ErrorMessage = "";
		}

		#region Property
		public int KnitDyeingGrayChallanDetailID { get; set; }
		public int KnitDyeingGrayChallanID { get; set; }
		public int KnitDyeingBatchDetailID { get; set; }
		public int GrayFabricID { get; set; }
		public int StoreID { get; set; }
		public int LotID { get; set; }
		public int MUnitID { get; set; }
		public double Qty { get; set; }
		public string Remarks { get; set; }
		public string StoreName { get; set; }
		public string LotNo { get; set; }
        public string UnitName { get; set; }
        public string FabricName { get; set; }
		public string ErrorMessage { get; set; }
		#endregion 

		#region Derived Property
		#endregion 

		#region Functions 
		public static List<KnitDyeingGrayChallanDetail> Gets(long nUserID)
		{
			return KnitDyeingGrayChallanDetail.Service.Gets(nUserID);
		}
		public static List<KnitDyeingGrayChallanDetail> Gets(string sSQL, long nUserID)
		{
			return KnitDyeingGrayChallanDetail.Service.Gets(sSQL,nUserID);
		}
		public KnitDyeingGrayChallanDetail Get(int id, long nUserID)
		{
			return KnitDyeingGrayChallanDetail.Service.Get(id,nUserID);
		}
		public KnitDyeingGrayChallanDetail Save(long nUserID)
		{
			return KnitDyeingGrayChallanDetail.Service.Save(this,nUserID);
		}
		public  string  Delete(int id, long nUserID)
		{
			return KnitDyeingGrayChallanDetail.Service.Delete(id,nUserID);
		}
		#endregion

		#region ServiceFactory
		internal static IKnitDyeingGrayChallanDetailService Service
		{
			get { return (IKnitDyeingGrayChallanDetailService)Services.Factory.CreateService(typeof(IKnitDyeingGrayChallanDetailService)); }
		}
		#endregion
	}
	#endregion

	#region IKnitDyeingGrayChallanDetail interface
	public interface IKnitDyeingGrayChallanDetailService 
	{
		KnitDyeingGrayChallanDetail Get(int id, Int64 nUserID); 
		List<KnitDyeingGrayChallanDetail> Gets(Int64 nUserID);
		List<KnitDyeingGrayChallanDetail> Gets( string sSQL, Int64 nUserID);
		string Delete(int id, Int64 nUserID);
 		KnitDyeingGrayChallanDetail Save(KnitDyeingGrayChallanDetail oKnitDyeingGrayChallanDetail, Int64 nUserID);
	}
	#endregion
}
