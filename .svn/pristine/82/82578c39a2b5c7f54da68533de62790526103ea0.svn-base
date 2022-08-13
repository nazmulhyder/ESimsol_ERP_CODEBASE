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
	#region KnitDyeingBatchYarn  
	public class KnitDyeingBatchYarn : BusinessObject
	{	
		public KnitDyeingBatchYarn()
		{
			KnitDyeingBatchYarnID = 0; 
			KnitDyeingBatchID = 0; 
			CompositionID = 0; 
			CompositionName = ""; 
			LotID = 0; 
			LotNo = ""; 
			Remarks = ""; 
			ErrorMessage = "";
		}

		#region Property
		public int KnitDyeingBatchYarnID { get; set; }
		public int KnitDyeingBatchID { get; set; }
		public int CompositionID { get; set; }
		public string CompositionName { get; set; }
		public int LotID { get; set; }
		public string LotNo { get; set; }
		public string Remarks { get; set; }
		public string ErrorMessage { get; set; }
		#endregion 

		#region Derived Property
		#endregion 

		#region Functions 
		public static List<KnitDyeingBatchYarn> Gets(long nUserID)
		{
			return KnitDyeingBatchYarn.Service.Gets(nUserID);
		}
		public static List<KnitDyeingBatchYarn> Gets(string sSQL, long nUserID)
		{
			return KnitDyeingBatchYarn.Service.Gets(sSQL,nUserID);
		}
		public KnitDyeingBatchYarn Get(int id, long nUserID)
		{
			return KnitDyeingBatchYarn.Service.Get(id,nUserID);
		}
		public KnitDyeingBatchYarn Save(long nUserID)
		{
			return KnitDyeingBatchYarn.Service.Save(this,nUserID);
		}
		public  string  Delete(int id, long nUserID)
		{
			return KnitDyeingBatchYarn.Service.Delete(id,nUserID);
		}
		#endregion

		#region ServiceFactory
		internal static IKnitDyeingBatchYarnService Service
		{
			get { return (IKnitDyeingBatchYarnService)Services.Factory.CreateService(typeof(IKnitDyeingBatchYarnService)); }
		}
		#endregion
	}
	#endregion

	#region IKnitDyeingBatchYarn interface
	public interface IKnitDyeingBatchYarnService 
	{
		KnitDyeingBatchYarn Get(int id, Int64 nUserID); 
		List<KnitDyeingBatchYarn> Gets(Int64 nUserID);
		List<KnitDyeingBatchYarn> Gets( string sSQL, Int64 nUserID);
		string Delete(int id, Int64 nUserID);
 		KnitDyeingBatchYarn Save(KnitDyeingBatchYarn oKnitDyeingBatchYarn, Int64 nUserID);
	}
	#endregion
}
