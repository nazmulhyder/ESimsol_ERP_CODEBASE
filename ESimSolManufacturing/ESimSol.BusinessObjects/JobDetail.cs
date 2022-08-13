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
	#region JobDetail  
	public class JobDetail : BusinessObject
	{	
		public JobDetail()
		{
			JobDetailID = 0; 
			JobID = 0; 
			OrderRecapID = 0; 
			OrderRecapNo = ""; 
			ShipmentDate = DateTime.Now; 
			TotalQuantity = 0;
            ProductName = "";
            DeptName = "";
            TSTypeInt = (int)EnumTSType.Sweater;
			ErrorMessage = "";
		}

		#region Property
		public int JobDetailID { get; set; }
		public int JobID { get; set; }
		public int OrderRecapID { get; set; }
        public int TSTypeInt { get; set; }
		public string ErrorMessage { get; set; }
		#endregion 

		#region Derived Property
        public string OrderRecapNo { get; set; }
        public DateTime ShipmentDate { get; set; }
        public double TotalQuantity { get; set; }
        public string ProductName { get; set; }
        public string DeptName { get; set; }
		public string ShipmentDateInString 
		{
			get
			{
				return ShipmentDate.ToString("dd MMM yyyy") ; 
			}
		}
		#endregion 

		#region Functions 
		public static List<JobDetail> Gets(int nJobID, long nUserID)
		{
            return JobDetail.Service.Gets(nJobID,nUserID);
		}
		public static List<JobDetail> Gets(string sSQL, long nUserID)
		{
			return JobDetail.Service.Gets(sSQL,nUserID);
		}
		public JobDetail Get(int id, long nUserID)
		{
			return JobDetail.Service.Get(id,nUserID);
		}
	
		#endregion

		#region ServiceFactory
		internal static IJobDetailService Service
		{
			get { return (IJobDetailService)Services.Factory.CreateService(typeof(IJobDetailService)); }
		}
		#endregion
	}
	#endregion

	#region IJobDetail interface
	public interface IJobDetailService 
	{
		JobDetail Get(int id, Int64 nUserID); 
		List<JobDetail> Gets(int nJobID, Int64 nUserID);
		List<JobDetail> Gets( string sSQL, Int64 nUserID);
	
	}
	#endregion
}
