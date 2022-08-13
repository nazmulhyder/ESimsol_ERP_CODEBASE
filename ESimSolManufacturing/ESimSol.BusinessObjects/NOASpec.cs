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
	#region NOASpecID  
	public class NOASpec : BusinessObject
	{	
		public NOASpec()
		{
            NOASpecLogID = 0;
            NOADetailLogID = 0;
			NOASpecID = 0; 
			SpecHeadID = 0; 
			NOADetailID = 0; 
			NOADescription = ""; 
			ErrorMessage = "";
            SpecName = string.Empty;
            NOASpecs = new List<NOASpec>();
		}

        #region Property
        public int NOASpecLogID { get; set; }
        public int NOADetailLogID { get; set; }
        public int NOASpecID { get; set; }
		public int SpecHeadID { get; set; }
		public int NOADetailID { get; set; }
		public string NOADescription { get; set; }
		public string ErrorMessage { get; set; }
		#endregion 

		#region Derived Property
        public int ProductID { get; set; }
        public string SpecName { get; set; }
        public List<NOASpec> NOASpecs { get; set; }
		#endregion 

		#region Functions 
		public static List<NOASpec> Gets(long nUserID)
		{
			return NOASpec.Service.Gets(nUserID);
		}
        public static List<NOASpec> Gets(string sSQL, long nUserID)
        {
            return NOASpec.Service.Gets(sSQL, nUserID);
        }
        public static List<NOASpec> GetsByLog(string sSQL, long nUserID)
        {
            return NOASpec.Service.GetsByLog(sSQL, nUserID);
        }
		public NOASpec Get(int id, long nUserID)
		{
			return NOASpec.Service.Get(id,nUserID);
		}
        public NOASpec IUD(short nDBOperation, int nUserID)
        {
            return NOASpec.Service.IUD(this, nDBOperation, nUserID);
        }
		#endregion

		#region ServiceFactory
		internal static INOASpecService Service
		{
            get { return (INOASpecService)Services.Factory.CreateService(typeof(INOASpecService)); }
		}
		#endregion
	}
	#endregion

	#region INOASpec interface
	public interface INOASpecService 
	{
		NOASpec Get(int id, Int64 nUserID);
        List<NOASpec> Gets(Int64 nUserID);
        List<NOASpec> Gets(string sSQL, Int64 nUserID);
        List<NOASpec> GetsByLog(string sSQL, Int64 nUserID);
        NOASpec IUD(NOASpec oNOASpec, short nDBOperation, int nUserID);
	}
	#endregion
}
