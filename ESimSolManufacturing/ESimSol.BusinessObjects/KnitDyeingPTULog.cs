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
	#region KnitDyeingPTULog  
	public class KnitDyeingPTULog : BusinessObject
	{
        public KnitDyeingPTULog()
        {
            
            KnitDyeingPTULogID = 0;
            KnitDyeingPTUID = 0;
            KnitDyeingPTURefType = EnumKnitDyeingPTURefType.None;
            KnitDyeingPTURefObjID = 0;
            Qty = 0;
            Remarks = "";
            DBUserID = 0;
            DBServerDateTime = DateTime.Now;
            UserName = "";
            RefNo = "";
            ErrorMessage = "";
        }

		#region Property
        
        public int KnitDyeingPTULogID { get; set; }
        public int KnitDyeingPTUID { get; set; }
        public EnumKnitDyeingPTURefType KnitDyeingPTURefType { get; set; }
        public int KnitDyeingPTURefTypeInt { get; set; }
        public int KnitDyeingPTURefObjID { get; set; }
        public double Qty { get; set; }
        public string Remarks { get; set; }
        public int DBUserID { get; set; }
        public DateTime DBServerDateTime { get; set; }
        public string UserName { get; set; }
        public string RefNo { get; set; }
		public string ErrorMessage { get; set; }
		#endregion 

		#region Derived Property
        public string DBServerDateTimeSt
        {
            get
            {
                return this.DBServerDateTime.ToString("dd MMM yyyy hh:mm tt");
            }
        }
		#endregion 

		#region Functions 
		public static List<KnitDyeingPTULog> Gets(long nUserID)
		{
			return KnitDyeingPTULog.Service.Gets(nUserID);
		}
		public static List<KnitDyeingPTULog> Gets(string sSQL, long nUserID)
		{
			return KnitDyeingPTULog.Service.Gets(sSQL,nUserID);
		}
        public static List<KnitDyeingPTULog> Gets(int nKnitDyeingPTUID, int eType, long nUserID)
        {
            return KnitDyeingPTULog.Service.Gets(nKnitDyeingPTUID, eType, nUserID);
        }
		public KnitDyeingPTULog Get(int id, long nUserID)
		{
			return KnitDyeingPTULog.Service.Get(id,nUserID);
		}
		public KnitDyeingPTULog Save(long nUserID)
		{
			return KnitDyeingPTULog.Service.Save(this,nUserID);
		}
		public  string  Delete(int id, long nUserID)
		{
			return KnitDyeingPTULog.Service.Delete(id,nUserID);
		}
		#endregion

		#region ServiceFactory
		internal static IKnitDyeingPTULogService Service
		{
			get { return (IKnitDyeingPTULogService)Services.Factory.CreateService(typeof(IKnitDyeingPTULogService)); }
		}
		#endregion
	}
	#endregion

	#region IKnitDyeingPTULog interface
	public interface IKnitDyeingPTULogService 
	{
		KnitDyeingPTULog Get(int id, Int64 nUserID); 
		List<KnitDyeingPTULog> Gets(Int64 nUserID);
        List<KnitDyeingPTULog> Gets(int nID, int nEType, Int64 nUserID);
		List<KnitDyeingPTULog> Gets( string sSQL, Int64 nUserID);
		string Delete(int id, Int64 nUserID);
 		KnitDyeingPTULog Save(KnitDyeingPTULog oKnitDyeingPTULog, Int64 nUserID);
	}
	#endregion
}
