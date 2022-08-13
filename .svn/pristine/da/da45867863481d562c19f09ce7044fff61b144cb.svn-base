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
	#region FPMReportSetup  
	public class FPMReportSetup : BusinessObject
	{	
		public FPMReportSetup()
		{
			FPMReportSetupID = 0; 
			SetUpType = 0; 
			AccountHeadID = 0; 
			SubSetup = 0;
            SetUpTypeInt = 0;
			AccountCode = ""; 
			AccountHeadName = ""; 
			ErrorMessage = "";
		}

		#region Property
		public int FPMReportSetupID { get; set; }
        public EnumFPReportSetUpType SetUpType { get; set; }
		public int AccountHeadID { get; set; }
        public EnumFPReportSubSetup SubSetup { get; set; }
		public string AccountCode { get; set; }
		public string AccountHeadName { get; set; }
        public int SetUpTypeInt { get; set; }
		public string ErrorMessage { get; set; }
		#endregion 

		#region Derived Property
        public string SetUpTypeInString
        {
            get
            {
                return EnumObject.jGet(this.SetUpType);
            }
        }
        public string SubSetupInString
        {
            get
            {
                return EnumObject.jGet(this.SubSetup);
            }
        }
		#endregion 

		#region Functions 
		public static List<FPMReportSetup> Gets(long nUserID)
		{
			return FPMReportSetup.Service.Gets(nUserID);
		}
		public static List<FPMReportSetup> Gets(string sSQL, long nUserID)
		{
			return FPMReportSetup.Service.Gets(sSQL,nUserID);
		}
		public FPMReportSetup Get(int id, long nUserID)
		{
			return FPMReportSetup.Service.Get(id,nUserID);
		}
		public FPMReportSetup Save(long nUserID)
		{
			return FPMReportSetup.Service.Save(this,nUserID);
		}
		public  string  Delete(int id, long nUserID)
		{
			return FPMReportSetup.Service.Delete(id,nUserID);
		}
		#endregion

		#region ServiceFactory
		internal static IFPMReportSetupService Service
		{
			get { return (IFPMReportSetupService)Services.Factory.CreateService(typeof(IFPMReportSetupService)); }
		}
		#endregion
	}
	#endregion

	#region IFPMReportSetup interface
	public interface IFPMReportSetupService 
	{
		FPMReportSetup Get(int id, Int64 nUserID); 
		List<FPMReportSetup> Gets(Int64 nUserID);
		List<FPMReportSetup> Gets( string sSQL, Int64 nUserID);
		string Delete(int id, Int64 nUserID);
 		FPMReportSetup Save(FPMReportSetup oFPMReportSetup, Int64 nUserID);
	}
	#endregion
}
