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
	#region PQSpec  
	public class PQSpec : BusinessObject
	{	
		public PQSpec()
		{
            PQSpecLogID = 0;
            PQDetailLogID = 0;
			PQSpecID = 0; 
			SpecHeadID = 0; 
			PQDetailID = 0; 
			PQDescription = ""; 
			ErrorMessage = "";
            SpecName = string.Empty;
            PQSpecs = new List<PQSpec>();
            SupplierID = 0;
           
		}

        #region Property
        public int PQSpecID { get; set; }
        public int PQSpecLogID { get; set; }
        public int PQDetailLogID { get; set; }
		public int SpecHeadID { get; set; }
		public int PQDetailID { get; set; }
		public string PQDescription { get; set; }
		public string ErrorMessage { get; set; }
		#endregion 

		#region Derived Property
        public string SpecName { get; set; }
        public int ProductID { get; set; }
        public List<PQSpec> PQSpecs { get; set; }
        public int SupplierID { get; set; }
       
		#endregion 

		#region Functions 
		public static List<PQSpec> Gets(long nUserID)
		{
			return PQSpec.Service.Gets(nUserID);
		}
        public static List<PQSpec> Gets(string sSQL, long nUserID)
        {
            return PQSpec.Service.Gets(sSQL, nUserID);
        }
        public static List<PQSpec> GetsByLog(string sSQL, long nUserID)
        {
            return PQSpec.Service.GetsByLog(sSQL, nUserID);
        }
		public PQSpec Get(int id, long nUserID)
		{
			return PQSpec.Service.Get(id,nUserID);
		}
	
        public PQSpec IUD(short nDBOperation, int nUserID)
        {
            return PQSpec.Service.IUD(this, nDBOperation, nUserID);
        }
		#endregion

		#region ServiceFactory
		internal static IPQSpecService Service
		{
			get { return (IPQSpecService)Services.Factory.CreateService(typeof(IPQSpecService)); }
		}
		#endregion
	}
	#endregion

	#region IPQSpec interface
	public interface IPQSpecService 
	{
		PQSpec Get(int id, Int64 nUserID);
        List<PQSpec> Gets(Int64 nUserID);
        List<PQSpec> Gets(string sSQL, Int64 nUserID);
        List<PQSpec> GetsByLog(string sSQL, Int64 nUserID);

        PQSpec IUD(PQSpec oPQSpec, short nDBOperation, int nUserID);
	}
	#endregion
}
