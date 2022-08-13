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
	#region LotLocation  
	public class LotLocation : BusinessObject
	{	
		public LotLocation()
		{
			LotLocationID = 0; 
			LotID = 0; 
			LotLocationName = ""; 
			Remarks = ""; 
			ErrorMessage = "";
		}

		#region Property
		public int LotLocationID { get; set; }
		public int LotID { get; set; }
		public string LotLocationName { get; set; }
		public string Remarks { get; set; }
		public string ErrorMessage { get; set; }
		#endregion 

		#region Derived Property
		#endregion 

		#region Functions 
		public static List<LotLocation> Gets(long nUserID)
		{
			return LotLocation.Service.Gets(nUserID);
		}
		public static List<LotLocation> Gets(string sSQL, long nUserID)
		{
			return LotLocation.Service.Gets(sSQL,nUserID);
		}
		public LotLocation Get(int id, long nUserID)
		{
			return LotLocation.Service.Get(id,nUserID);
		}
		public LotLocation Save(long nUserID)
		{
			return LotLocation.Service.Save(this,nUserID);
		}
		public  string  Delete(int id, long nUserID)
		{
			return LotLocation.Service.Delete(id,nUserID);
		}
		#endregion

		#region ServiceFactory
		internal static ILotLocationService Service
		{
			get { return (ILotLocationService)Services.Factory.CreateService(typeof(ILotLocationService)); }
		}
		#endregion
	}
	#endregion

	#region ILotLocation interface
	public interface ILotLocationService 
	{
		LotLocation Get(int id, Int64 nUserID); 
		List<LotLocation> Gets(Int64 nUserID);
		List<LotLocation> Gets( string sSQL, Int64 nUserID);
		string Delete(int id, Int64 nUserID);
 		LotLocation Save(LotLocation oLotLocation, Int64 nUserID);
	}
	#endregion
}
