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
    #region PRSpec
    public class PRSpec :BusinessObject
    {
        public PRSpec()
		{
            PRSpecID = 0; 
			SpecHeadID = 0; 
			PRDetailID = 0; 
			PRDescription = ""; 
			ErrorMessage = "";
            SL = 0;
            SpecName = string.Empty;
            PRSpecs = new List<PRSpec>();
		}

		#region Property
		public int PRSpecID { get; set; }
		public int SpecHeadID { get; set; }
		public int PRDetailID { get; set; }
		public string PRDescription { get; set; }
		public string ErrorMessage { get; set; }
        public int SL { get; set; }
		#endregion 

		#region Derived Property
        public string SpecName { get; set; }
        public int ProductID { get; set; }
        public List<PRSpec> PRSpecs { get; set; }
		#endregion 

		#region Functions 
        public static List<PRSpec> Gets(long nUserID)
		{
            return PRSpec.Service.Gets(nUserID);
		}
        public static List<PRSpec> Gets(string sSQL, long nUserID)
		{
            return PRSpec.Service.Gets(sSQL, nUserID);
		}
        public PRSpec Get(int id, long nUserID)
		{
            return PRSpec.Service.Get(id, nUserID);
		}

        public PRSpec IUD(short nDBOperation, int nUserID)
        {
            return PRSpec.Service.IUD(this, nDBOperation, nUserID);
        }
        public string Delete(int id, long nUserID)
        {
            return PRSpec.Service.Delete(id, nUserID);
        }
        public List<PRSpec> RefreshSequence(List<PRSpec> oPRSpecs, long nUserID)
        {
            return PRSpec.Service.RefreshSequence(oPRSpecs, nUserID);
        }
		#endregion

		#region ServiceFactory
		internal static IPRSpecService Service
		{
            get { return (IPRSpecService)Services.Factory.CreateService(typeof(IPRSpecService)); }
		}
		#endregion
    }
    #endregion

    #region IPQSpec interface
    public interface IPRSpecService
    {
        PRSpec Get(int id, Int64 nUserID);
        List<PRSpec> Gets(Int64 nUserID);
        List<PRSpec> Gets(string sSQL, Int64 nUserID);
        PRSpec IUD(PRSpec oPRSpec, short nDBOperation, int nUserID);
        string Delete(int id, Int64 nUserID);
        List<PRSpec> RefreshSequence(List<PRSpec> oPRSpecs, long nUserID);
    }
    #endregion
}
