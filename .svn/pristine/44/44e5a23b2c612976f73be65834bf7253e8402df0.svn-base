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
	#region PISpec  
	public class PISpec : BusinessObject
	{	
		public PISpec()
		{
			PISpecID = 0; 
			SpecHeadID = 0; 
			PIDetailID = 0; 
			PIDescription = ""; 
			ErrorMessage = "";
            SpecName = string.Empty;
            PISpecs = new List<PISpec>();
		}

		#region Property
		public int PISpecID { get; set; }
		public int SpecHeadID { get; set; }
		public int PIDetailID { get; set; }
		public string PIDescription { get; set; }
		public string ErrorMessage { get; set; }
		#endregion 

		#region Derived Property
        public string SpecName { get; set; }
        public int ProductID { get; set; }
        public List<PISpec> PISpecs { get; set; }
		#endregion 

		#region Functions 
		public static List<PISpec> Gets(long nUserID)
		{
			return PISpec.Service.Gets(nUserID);
		}
		public static List<PISpec> Gets(string sSQL, long nUserID)
		{
			return PISpec.Service.Gets(sSQL,nUserID);
		}
		public PISpec Get(int id, long nUserID)
		{
			return PISpec.Service.Get(id,nUserID);
		}
        public PISpec IUD(short nDBOperation, int nUserID)
        {
            return PISpec.Service.IUD(this, nDBOperation, nUserID);
        }
		#endregion

		#region ServiceFactory
		internal static IPISpecService Service
		{
			get { return (IPISpecService)Services.Factory.CreateService(typeof(IPISpecService)); }
		}
		#endregion
	}
	#endregion

	#region IPISpec interface
	public interface IPISpecService 
	{
		PISpec Get(int id, Int64 nUserID); 
		List<PISpec> Gets(Int64 nUserID);
		List<PISpec> Gets( string sSQL, Int64 nUserID);
        PISpec IUD(PISpec oPISpec, short nDBOperation, int nUserID);

	}
	#endregion
}
