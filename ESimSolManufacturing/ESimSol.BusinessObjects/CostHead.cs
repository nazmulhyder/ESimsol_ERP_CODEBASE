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
	#region CostHead  
	public class CostHead : BusinessObject
	{	
		public CostHead()
		{
			CostHeadID = 0; 
			Name = ""; 
			Note = "";
            CostHeadType = EnumCostHeadType.None;
            //CostHeadCategorey = EnumCostHeadCategorey.None; 
			ErrorMessage = "";
		}

		#region Property
		public int CostHeadID { get; set; }
		public string Name { get; set; }
		public string Note { get; set; } 
		public EnumCostHeadType CostHeadType { get; set; }
        //public EnumCostHeadCategorey CostHeadCategorey { get; set; }
		public string ErrorMessage { get; set; }
        public string Params { get; set; }
		#endregion 

        #region Derived Property 
        public string CostHeadTypeSt { get { return this.CostHeadType.ToString(); } }
        //public string CostHeadCategoreySt { get { return this.CostHeadCategorey.ToString(); } }
		#endregion 

		#region Functions 
		public static List<CostHead> Gets(long nUserID)
		{
			return CostHead.Service.Gets(nUserID);
		}
		public static List<CostHead> Gets(string sSQL, long nUserID)
		{
			return CostHead.Service.Gets(sSQL,nUserID);
		}
		public CostHead Get(int id, long nUserID)
		{
			return CostHead.Service.Get(id,nUserID);
		}
		public CostHead Save(long nUserID)
		{
			return CostHead.Service.Save(this,nUserID);
		}
		public  string  Delete(int id, long nUserID)
		{
			return CostHead.Service.Delete(id,nUserID);
		}
		#endregion

		#region ServiceFactory
		internal static ICostHeadService Service
		{
			get { return (ICostHeadService)Services.Factory.CreateService(typeof(ICostHeadService)); }
		}
		#endregion
	}
	#endregion

	#region ICostHead interface
	public interface ICostHeadService 
	{
		CostHead Get(int id, Int64 nUserID); 
		List<CostHead> Gets(Int64 nUserID);
		List<CostHead> Gets( string sSQL, Int64 nUserID);
		string Delete(int id, Int64 nUserID);
 		CostHead Save(CostHead oCostHead, Int64 nUserID);
	}
	#endregion
}
