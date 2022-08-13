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
	#region LotSpec  
	public class LotSpec : BusinessObject
	{	
		public LotSpec()
		{
			LotSpecID = 0; 
			SpecHeadID = 0; 
			LotID = 0; 
			SpecDescription = ""; 
			ErrorMessage = "";
            SpecName = string.Empty;
            oLotSpecs = new List<LotSpec>();
		}

		#region Property
		public int LotSpecID { get; set; }
		public int SpecHeadID { get; set; }
		public int LotID { get; set; }
		public string SpecDescription { get; set; }
		public string ErrorMessage { get; set; }
		#endregion 

		#region Derived Property
        public string SpecName { get; set; }
        public int ProductID { get; set; }
        public List<LotSpec> oLotSpecs { get; set; }
		#endregion 

		#region Functions 
		public static List<LotSpec> Gets(long nUserID)
		{
			return LotSpec.Service.Gets(nUserID);
		}
		public static List<LotSpec> Gets(string sSQL, long nUserID)
		{
			return LotSpec.Service.Gets(sSQL,nUserID);
		}
		public LotSpec Get(int id, long nUserID)
		{
			return LotSpec.Service.Get(id,nUserID);
		}
        public LotSpec IUD(short nDBOperation, int nUserID)
        {
            return LotSpec.Service.IUD(this, nDBOperation, nUserID);
        }
		#endregion

		#region ServiceFactory
		internal static ILotSpecService Service
		{
			get { return (ILotSpecService)Services.Factory.CreateService(typeof(ILotSpecService)); }
		}
		#endregion
	}
	#endregion

	#region ILotSpec interface
	public interface ILotSpecService 
	{
		LotSpec Get(int id, Int64 nUserID); 
		List<LotSpec> Gets(Int64 nUserID);
		List<LotSpec> Gets( string sSQL, Int64 nUserID);
        LotSpec IUD(LotSpec oLotSpec, short nDBOperation, int nUserID);
		
	}
	#endregion
}
