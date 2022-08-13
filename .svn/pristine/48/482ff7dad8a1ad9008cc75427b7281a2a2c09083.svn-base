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
	#region BUWiseProductCategory  
	public class BUWiseProductCategory : BusinessObject
	{	
		public BUWiseProductCategory()
		{
			BUWiseProductCategoryID = 0; 
			BUID = 0; 
			ProductCategoryID = 0; 
			BUName = ""; 
			ProductCategoryName = ""; 
			ErrorMessage = "";
		}

		#region Property
		public int BUWiseProductCategoryID { get; set; }
		public int BUID { get; set; }
		public int ProductCategoryID { get; set; }
		public string BUName { get; set; }
		public string ProductCategoryName { get; set; }
		public string ErrorMessage { get; set; }
		#endregion 

		#region Derived Property
		#endregion 

		#region Functions 
        public static List<BUWiseProductCategory> Gets(int nID, long nUserID) //nID is ProductCategory ID
		{
            return BUWiseProductCategory.Service.Gets(nID, nUserID);
		}
		public static List<BUWiseProductCategory> Gets(string sSQL, long nUserID)
		{
			return BUWiseProductCategory.Service.Gets(sSQL,nUserID);
		}
		public BUWiseProductCategory Get(int id, long nUserID)
		{
			return BUWiseProductCategory.Service.Get(id,nUserID);
		}
		public BUWiseProductCategory Save(long nUserID)
		{
			return BUWiseProductCategory.Service.Save(this,nUserID);
		}
		public  string  Delete(int id, long nUserID)
		{
			return BUWiseProductCategory.Service.Delete(id,nUserID);
		}
		#endregion

		#region ServiceFactory
		internal static IBUWiseProductCategoryService Service
		{
			get { return (IBUWiseProductCategoryService)Services.Factory.CreateService(typeof(IBUWiseProductCategoryService)); }
		}
		#endregion
	}
	#endregion

	#region IBUWiseProductCategory interface
	public interface IBUWiseProductCategoryService 
	{
		BUWiseProductCategory Get(int id, Int64 nUserID); 
		List<BUWiseProductCategory> Gets(int nID, Int64 nUserID);
		List<BUWiseProductCategory> Gets( string sSQL, Int64 nUserID);
		string Delete(int id, Int64 nUserID);
 		BUWiseProductCategory Save(BUWiseProductCategory oBUWiseProductCategory, Int64 nUserID);
	}
	#endregion
}
