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
	#region SpecHead  
	public class SpecHead : BusinessObject
	{	
		public SpecHead()
		{
			SpecHeadID = 0; 
			SpecName = ""; 
			IsActive = true; 
			ErrorMessage = "";
            Params = string.Empty;
		}

		#region Property
		public int SpecHeadID { get; set; }
		public string SpecName { get; set; }
		public bool IsActive { get; set; }
		public string ErrorMessage { get; set; }
        public string Params { get; set; }
		#endregion 

		#region Derived Property
		#endregion 

		#region Functions 
		
		public static List<SpecHead> Gets(string sSQL, long nUserID)
		{
			return SpecHead.Service.Gets(sSQL,nUserID);
		}
		public SpecHead Get(int id, long nUserID)
		{
			return SpecHead.Service.Get(id,nUserID);
		}
        public SpecHead IUD(int nDBOperation, long nUserID)
        {
            return SpecHead.Service.IUD(this, nDBOperation, nUserID);
        }
		
		#endregion

		#region ServiceFactory
		internal static ISpecHeadService Service
		{
			get { return (ISpecHeadService)Services.Factory.CreateService(typeof(ISpecHeadService)); }
		}
		#endregion
	}
	#endregion

	#region ISpecHead interface
	public interface ISpecHeadService 
	{
		SpecHead Get(int id, Int64 nUserID); 
        List<SpecHead> Gets( string sSQL, Int64 nUserID);
        SpecHead IUD(SpecHead oSpecHead, int nDBOperation, Int64 nUserID);
	}
	#endregion
}
