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
	#region KnitDyeingYarnRequisition  
	public class KnitDyeingYarnRequisition : BusinessObject
	{	
		public KnitDyeingYarnRequisition()
		{
			KnitDyeingYarnRequisitionID = 0;
            KnitDyeingProgramID = 0;
            KnitDyeingYarnRequisitionLogID = 0; 
            KnitDyeingProgramLogID = 0;
			YarnCountID = 0; 
			UsagesParcent = 0; 
			FabricTypeID = 0;
            FinishRequiredQty = 0;
            GracePercent = 0;
			RequiredQty = 0; 
			MUnitID = 0;
            UnitSymbol = "";
			Remarks = "";
            YarnName = "";
            FabricTypeName = ""; 
			ErrorMessage = "";
		}

		#region Property
		public int KnitDyeingYarnRequisitionID { get; set; }
        public int KnitDyeingYarnRequisitionLogID { get; set; }
		public int KnitDyeingProgramID { get; set; }
        public int KnitDyeingProgramLogID { get; set; }
		public int YarnCountID { get; set; }
		public double UsagesParcent { get; set; }
        public int FabricTypeID { get; set; }
        public double FinishRequiredQty { get; set; }
        public double GracePercent { get; set; }
        public double RequiredQty { get; set; }
		public int MUnitID { get; set; }
        public string UnitSymbol { get; set; }
		public string Remarks { get; set; }
        public string YarnName { get; set; }
        public string FabricTypeName { get; set; }
		public string ErrorMessage { get; set; }
		#endregion 

		#region Derived Property
		#endregion 

		#region Functions 
		public static List<KnitDyeingYarnRequisition> Gets(long nUserID)
		{
			return KnitDyeingYarnRequisition.Service.Gets(nUserID);
		}
        public static List<KnitDyeingYarnRequisition> Gets(int id, long nUserID)
        {
            return KnitDyeingYarnRequisition.Service.Gets(id, nUserID);
        }
        public static List<KnitDyeingYarnRequisition> GetsLog(int Logid, long nUserID)
        {
            return KnitDyeingYarnRequisition.Service.GetsLog(Logid, nUserID);
        }
		public static List<KnitDyeingYarnRequisition> Gets(string sSQL, long nUserID)
		{
			return KnitDyeingYarnRequisition.Service.Gets(sSQL,nUserID);
		}
		public KnitDyeingYarnRequisition Get(int id, long nUserID)
		{
			return KnitDyeingYarnRequisition.Service.Get(id,nUserID);
		}
		public KnitDyeingYarnRequisition Save(long nUserID)
		{
			return KnitDyeingYarnRequisition.Service.Save(this,nUserID);
		}
		public  string  Delete(int id, long nUserID)
		{
			return KnitDyeingYarnRequisition.Service.Delete(id,nUserID);
		}
		#endregion

		#region ServiceFactory
		internal static IKnitDyeingYarnRequisitionService Service
		{
			get { return (IKnitDyeingYarnRequisitionService)Services.Factory.CreateService(typeof(IKnitDyeingYarnRequisitionService)); }
		}
		#endregion
	}
	#endregion

	#region IKnitDyeingYarnRequisition interface
	public interface IKnitDyeingYarnRequisitionService 
	{
		KnitDyeingYarnRequisition Get(int id, Int64 nUserID); 
		List<KnitDyeingYarnRequisition> Gets(Int64 nUserID);
		List<KnitDyeingYarnRequisition> Gets( string sSQL, Int64 nUserID);
		string Delete(int id, Int64 nUserID);
 		KnitDyeingYarnRequisition Save(KnitDyeingYarnRequisition oKnitDyeingYarnRequisition, Int64 nUserID);
        List<KnitDyeingYarnRequisition> Gets(int id, Int64 nUserID);
        List<KnitDyeingYarnRequisition> GetsLog(int LogId, Int64 nUserID);
        
	}
	#endregion
}
