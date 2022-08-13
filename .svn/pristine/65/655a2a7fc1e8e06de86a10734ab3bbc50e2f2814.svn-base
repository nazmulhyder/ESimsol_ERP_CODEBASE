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
	#region GUPReportSetUp  
	public class GUPReportSetUp : BusinessObject
	{	
		public GUPReportSetUp()
		{
			GUPReportSetUpID = 0; 
			ProductionStepID = 0; 
			Sequence = 0;
            StepName = "";
            IsUp = true;
            GUPReportSetUps = new List<GUPReportSetUp>();
			ErrorMessage = "";
		}

		#region Property
		public int GUPReportSetUpID { get; set; }
		public int ProductionStepID { get; set; }
		public int Sequence { get; set; }
        public string StepName { get; set; }
        public EnumProductionStepType ProductionStepType { get; set; }
        public int ProductionStepTypeInt { get; set;}
		public string ErrorMessage { get; set; }
		#endregion 

		#region Derived Property
        public bool IsUp { get; set; }
        public List<GUPReportSetUp> GUPReportSetUps { get; set; }
		#endregion 

		#region Functions 
		public static List<GUPReportSetUp> Gets(long nUserID)
		{
			return GUPReportSetUp.Service.Gets(nUserID);
		}
		public static List<GUPReportSetUp> Gets(string sSQL, long nUserID)
		{
			return GUPReportSetUp.Service.Gets(sSQL,nUserID);
		}
		public GUPReportSetUp Get(int id, long nUserID)
		{
			return GUPReportSetUp.Service.Get(id,nUserID);
		}
		public GUPReportSetUp Save(long nUserID)
		{
			return GUPReportSetUp.Service.Save(this,nUserID);
		}
		public  string  Delete(int id, long nUserID)
		{
			return GUPReportSetUp.Service.Delete(id,nUserID);
		}
        public static List<GUPReportSetUp> UpDown(GUPReportSetUp oGUPReportSetUp, long nUserID)
        {
            return GUPReportSetUp.Service.UpDown(oGUPReportSetUp, nUserID);
        }
 
		#endregion

		#region ServiceFactory
		internal static IGUPReportSetUpService Service
		{
			get { return (IGUPReportSetUpService)Services.Factory.CreateService(typeof(IGUPReportSetUpService)); }
		}
		#endregion
	}
	#endregion

	#region IGUPReportSetUp interface
	public interface IGUPReportSetUpService 
	{
		GUPReportSetUp Get(int id, Int64 nUserID); 
		List<GUPReportSetUp> Gets(Int64 nUserID);
		List<GUPReportSetUp> Gets( string sSQL, Int64 nUserID);
		string Delete(int id, Int64 nUserID);
 		GUPReportSetUp Save(GUPReportSetUp oGUPReportSetUp, Int64 nUserID);
        List<GUPReportSetUp> UpDown(GUPReportSetUp oGUPReportSetUp, Int64 nUserID);
       
	}
	#endregion
}
