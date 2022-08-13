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
	#region FADepreciation  
	public class FADepreciation : BusinessObject
	{	
		public FADepreciation()
		{
			FADepreciationID = 0; 
			FADepreciationNo = ""; 
			BUID = 0; 
			BUName = ""; 
			DepreciationDate = DateTime.Now; 
			ApprovedBy = 0; 
			ApprovedByName = ""; 
			Remarks = ""; 
			ErrorMessage = "";
            BUName = "";
            Params = "";
            bIsApproved = true;
            FADepreciationDetails = new List<FADepreciationDetail>();
            FADepreciations = new List<FADepreciation>();
		}

		#region Property
		public int FADepreciationID { get; set; }
		public string FADepreciationNo { get; set; }
		public int BUID { get; set; }
		public string BUName { get; set; }
		public DateTime DepreciationDate { get; set; }
		public int ApprovedBy { get; set; }
		public string ApprovedByName { get; set; }
		public string Remarks { get; set; }

		public string ErrorMessage { get; set; }
		#endregion 

		#region Derived Property
        public string Params { get; set; }
        public bool bIsApproved { get; set; }
        public List<FADepreciationDetail> FADepreciationDetails;
        public List<FADepreciation> FADepreciations;
		public string DepreciationDateInString 
		{
			get
			{
				return DepreciationDate.ToString("dd MMM yyyy") ; 
			}
		}
		#endregion 

		#region Functions 
	
		public static List<FADepreciation> Gets(string sSQL, long nUserID)
		{
			return FADepreciation.Service.Gets(sSQL,nUserID);
		}
		public FADepreciation Get(int id, long nUserID)
		{
			return FADepreciation.Service.Get(id,nUserID);
		}
		public FADepreciation Save(long nUserID)
		{
			return FADepreciation.Service.Save(this,nUserID);
		}
        public FADepreciation Approval(long nUserID)
		{
            return FADepreciation.Service.Approval(this, nUserID);
		}
        
		public  string  Delete(int id, long nUserID)
		{
			return FADepreciation.Service.Delete(id,nUserID);
		}
		#endregion

		#region ServiceFactory
		internal static IFADepreciationService Service
		{
			get { return (IFADepreciationService)Services.Factory.CreateService(typeof(IFADepreciationService)); }
		}
		#endregion
	}
	#endregion

	#region IFADepreciation interface
	public interface IFADepreciationService 
	{
		FADepreciation Get(int id, Int64 nUserID); 
	   List<FADepreciation> Gets( string sSQL, Int64 nUserID);
		string Delete(int id, Int64 nUserID);
 		FADepreciation Save(FADepreciation oFADepreciation, Int64 nUserID);
        FADepreciation Approval(FADepreciation oFADepreciation, Int64 nUserID);
        
	}
	#endregion
}
