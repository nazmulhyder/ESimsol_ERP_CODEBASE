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
	#region POSpec  
	public class POSpec : BusinessObject
	{	
		public POSpec()
		{
			POSpecID = 0; 
			SpecHeadID = 0; 
			PODetailID = 0; 
			PODescription = ""; 
			ErrorMessage = "";
            SpecName = string.Empty;
            POSpecs = new List<POSpec>();
		}

		#region Property
		public int POSpecID { get; set; }
		public int SpecHeadID { get; set; }
		public int PODetailID { get; set; }
		public string PODescription { get; set; }
		public string ErrorMessage { get; set; }
		#endregion 

		#region Derived Property
        public string SpecName { get; set; }
        public int ProductID { get; set; }
        public List<POSpec> POSpecs { get; set; }
		#endregion 

		#region Functions 
		public static List<POSpec> Gets(long nUserID)
		{
			return POSpec.Service.Gets(nUserID);
		}
		public static List<POSpec> Gets(string sSQL, long nUserID)
		{
			return POSpec.Service.Gets(sSQL,nUserID);
		}
		public POSpec Get(int id, long nUserID)
		{
			return POSpec.Service.Get(id,nUserID);
		}
        public POSpec IUD(short nDBOperation, int nUserID)
        {
            return POSpec.Service.IUD(this, nDBOperation, nUserID);
        }
		#endregion

		#region ServiceFactory
		internal static IPOSpecService Service
		{
			get { return (IPOSpecService)Services.Factory.CreateService(typeof(IPOSpecService)); }
		}
		#endregion
	}
	#endregion

	#region IPOSpec interface
	public interface IPOSpecService 
	{
		POSpec Get(int id, Int64 nUserID); 
		List<POSpec> Gets(Int64 nUserID);
		List<POSpec> Gets( string sSQL, Int64 nUserID);
        POSpec IUD(POSpec oPOSpec, short nDBOperation, int nUserID);
	}
	#endregion
}
