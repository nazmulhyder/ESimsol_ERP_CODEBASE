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
	#region PackingListDetail  
	public class PackingListDetail : BusinessObject
	{	
		public PackingListDetail()
		{
			PackingListDetailID = 0; 
			PackingListID = 0; 
			ColorID = 0; 
			SizeID = 0; 
			Qty = 0; 
			ColorName = ""; 
			SizeName = "";
            ErrorMessage = "";
		}

		#region Property
		public int PackingListDetailID { get; set; }
		public int PackingListID { get; set; }
		public int ColorID { get; set; }
		public int SizeID { get; set; }
		public double Qty { get; set; }
		public string ColorName { get; set; }
		public string SizeName { get; set; }
        public string ErrorMessage { get; set; }
		#endregion 

		#region Derived Property
		#endregion 

		#region Functions 
		public static List<PackingListDetail> Gets(int nPackingID, long nUserID)
		{
            return PackingListDetail.Service.Gets(nPackingID,nUserID);
		}
		public static List<PackingListDetail> Gets(string sSQL, long nUserID)
		{
			return PackingListDetail.Service.Gets(sSQL,nUserID);
		}
		public PackingListDetail Get(int id, long nUserID)
		{
			return PackingListDetail.Service.Get(id,nUserID);
		}

		
		#endregion

		#region ServiceFactory
		internal static IPackingListDetailService Service
		{
			get { return (IPackingListDetailService)Services.Factory.CreateService(typeof(IPackingListDetailService)); }
		}
		#endregion
	}
	#endregion

	#region IPackingListDetail interface
	public interface IPackingListDetailService 
	{
		PackingListDetail Get(int id, Int64 nUserID); 
		List<PackingListDetail> Gets(int nPackingID, Int64 nUserID);
		List<PackingListDetail> Gets( string sSQL, Int64 nUserID);
		
	}
	#endregion
}
