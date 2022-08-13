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
	#region KnitDyeingGrayChallan  
	public class KnitDyeingGrayChallan : BusinessObject
	{	
		public KnitDyeingGrayChallan()
		{
			KnitDyeingGrayChallanID = 0; 
			KnitDyeingBatchID = 0; 
			BUID = 0; 
			ChallanNo = ""; 
			ChallanDate = DateTime.Now; 
			DisburseBy = 0; 
			Remarks = ""; 
			TruckNo = ""; 
			DriverName = ""; 
			DisburseByName = ""; 
			ErrorMessage = "";
            KnitDyeingGrayChallanDetails = new List<KnitDyeingGrayChallanDetail>();
		}

		#region Property
		public int KnitDyeingGrayChallanID { get; set; }
		public int KnitDyeingBatchID { get; set; }
		public int BUID { get; set; }
		public string ChallanNo { get; set; }
		public DateTime ChallanDate { get; set; }
		public int DisburseBy { get; set; }
		public string Remarks { get; set; }
		public string TruckNo { get; set; }
		public string DriverName { get; set; }
		public string DisburseByName { get; set; }
		public string ErrorMessage { get; set; }
        public List<KnitDyeingGrayChallanDetail> KnitDyeingGrayChallanDetails { get; set; }
		#endregion 

		#region Derived Property
        public string BatchNo { get; set; }
        public string BUShortName { get; set; }
        public string BUName { get; set; }
        public string BUAddress { get; set; }
		public string ChallanDateSt
		{
			get
			{
				return ChallanDate.ToString("dd MMM yyyy") ; 
			}
		}
		#endregion 

		#region Functions 
		public static List<KnitDyeingGrayChallan> Gets(long nUserID)
		{
			return KnitDyeingGrayChallan.Service.Gets(nUserID);
		}
		public static List<KnitDyeingGrayChallan> Gets(string sSQL, long nUserID)
		{
			return KnitDyeingGrayChallan.Service.Gets(sSQL,nUserID);
		}
        public KnitDyeingGrayChallan Get(int id, long nUserID)
		{
			return KnitDyeingGrayChallan.Service.Get(id,nUserID);
		}
		public KnitDyeingGrayChallan Save(long nUserID)
		{
			return KnitDyeingGrayChallan.Service.Save(this,nUserID);
		}
        public KnitDyeingGrayChallan Disburse(long nUserID)
        {
            return KnitDyeingGrayChallan.Service.Disburse(this, nUserID);
        }
		public  string  Delete(int id, long nUserID)
		{
			return KnitDyeingGrayChallan.Service.Delete(id,nUserID);
		}
		#endregion

		#region ServiceFactory
		internal static IKnitDyeingGrayChallanService Service
		{
			get { return (IKnitDyeingGrayChallanService)Services.Factory.CreateService(typeof(IKnitDyeingGrayChallanService)); }
		}
		#endregion
	}
	#endregion

	#region IKnitDyeingGrayChallan interface
	public interface IKnitDyeingGrayChallanService 
	{
		KnitDyeingGrayChallan Get(int id, Int64 nUserID); 
		List<KnitDyeingGrayChallan> Gets(Int64 nUserID);
		List<KnitDyeingGrayChallan> Gets( string sSQL, Int64 nUserID);
		string Delete(int id, Int64 nUserID);
 		KnitDyeingGrayChallan Save(KnitDyeingGrayChallan oKnitDyeingGrayChallan, Int64 nUserID);
 		KnitDyeingGrayChallan Disburse(KnitDyeingGrayChallan oKnitDyeingGrayChallan, Int64 nUserID);
	}
	#endregion
}
