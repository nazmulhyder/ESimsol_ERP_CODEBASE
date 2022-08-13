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
	#region KnitDyeingBatchDetail  
	public class KnitDyeingBatchDetail : BusinessObject
	{	
		public KnitDyeingBatchDetail()
		{
			KnitDyeingBatchDetailID = 0; 
			KnitDyeingBatchID = 0; 
			KnitDyeingPTUID = 0; 
			FabricTypeID = 0; 
			GrayDiaID = 0; 
			RollQty = 0; 
			GrayQty = 0; 
			ProcessLoss = 0; 
			FinishQty = 0; 
			FinishDiaID = 0; 
			FinishGSMID = 0;
            GrayFabricID = 0;
            GrayFabricName = "";
			Remarks = ""; 
			ErrorMessage = "";
		}
		#region Property
		public int KnitDyeingBatchDetailID { get; set; }
		public int KnitDyeingBatchID { get; set; }
		public int KnitDyeingPTUID { get; set; }
		public int FabricTypeID { get; set; }
		public int GrayDiaID { get; set; }
		public double RollQty { get; set; }
		public double GrayQty { get; set; }
		public double ProcessLoss { get; set; }
		public double FinishQty { get; set; }
		public int FinishDiaID { get; set; }
		public int FinishGSMID { get; set; }
		public string Remarks { get; set; }
		public string ErrorMessage { get; set; }
		#endregion 
		#region Derived Property
        public string FabricTypeName { get; set; }
        public string GrayDiaName { get; set; }
        public string FinishDiaName { get; set; }
        public string FinishGSMName { get; set; }
        public int GrayFabricID { get; set; }
        public string GrayFabricName { get; set; }
        public string FullName
        {
            get
            {
                return this.GrayFabricName + " | " + this.FabricTypeName + " |" + this.FinishGSMName + " | " + this.GrayDiaName + " | " + this.FinishDiaName + " | " + this.GrayQty.ToString("#,##0.00"); 
            }
        }
		#endregion 

		#region Functions 
		public static List<KnitDyeingBatchDetail> Gets(long nUserID)
		{
			return KnitDyeingBatchDetail.Service.Gets(nUserID);
		}
		public static List<KnitDyeingBatchDetail> Gets(string sSQL, long nUserID)
		{
			return KnitDyeingBatchDetail.Service.Gets(sSQL,nUserID);
		}
		public KnitDyeingBatchDetail Get(int id, long nUserID)
		{
			return KnitDyeingBatchDetail.Service.Get(id,nUserID);
		}
        public static List<KnitDyeingBatchDetail> Gets(int id, long nUserID)
        {
            return KnitDyeingBatchDetail.Service.Gets(id, nUserID);
        }
		public KnitDyeingBatchDetail Save(long nUserID)
		{
			return KnitDyeingBatchDetail.Service.Save(this,nUserID);
		}
		public  string  Delete(int id, long nUserID)
		{
			return KnitDyeingBatchDetail.Service.Delete(id,nUserID);
		}
		#endregion

		#region ServiceFactory
		internal static IKnitDyeingBatchDetailService Service
		{
			get { return (IKnitDyeingBatchDetailService)Services.Factory.CreateService(typeof(IKnitDyeingBatchDetailService)); }
		}
		#endregion
	}
	#endregion

	#region IKnitDyeingBatchDetail interface
	public interface IKnitDyeingBatchDetailService 
	{
		KnitDyeingBatchDetail Get(int id, Int64 nUserID); 
		List<KnitDyeingBatchDetail> Gets(Int64 nUserID);
		List<KnitDyeingBatchDetail> Gets( string sSQL, Int64 nUserID);
		string Delete(int id, Int64 nUserID);
 		KnitDyeingBatchDetail Save(KnitDyeingBatchDetail oKnitDyeingBatchDetail, Int64 nUserID);
        List<KnitDyeingBatchDetail> Gets(int id, Int64 nUserID);
	}
	#endregion
}
