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
	#region ServiceOrderDetail  
	public class ServiceOrderDetail : BusinessObject
	{	
		public ServiceOrderDetail()
		{
			ServiceOrderDetailID = 0; 
            ServiceOrderID = 0; 
            WorkDescription = "";
            ServiceWorkType = EnumServiceType.None;
            ServiceWorkTypeInt =(int)EnumServiceType.None;
			ErrorMessage = "";
		}

		#region Property
		public int ServiceOrderDetailID { get; set; }
		public int ServiceOrderID { get; set; }
        public string WorkDescription { get; set; }
        public EnumServiceType ServiceWorkType { get; set; }
		public string ErrorMessage { get; set; }
        public int ServiceWorkTypeInt { get; set; }
  
		#endregion 

		#region Functions 
		public static List<ServiceOrderDetail> Gets(int id, long nUserID)
		{
			return ServiceOrderDetail.Service.Gets(id, nUserID);
		}
		public static List<ServiceOrderDetail> Gets(string sSQL, long nUserID)
		{
			return ServiceOrderDetail.Service.Gets(sSQL,nUserID);
		}
		public ServiceOrderDetail Get(int id, long nUserID)
		{
			return ServiceOrderDetail.Service.Get(id,nUserID);
		}
		
		#endregion

		#region ServiceFactory
		internal static IServiceOrderDetailService Service
		{
			get { return (IServiceOrderDetailService)Services.Factory.CreateService(typeof(IServiceOrderDetailService)); }
		}
		#endregion
	}
	#endregion

	#region IServiceOrderDetail interface
	public interface IServiceOrderDetailService 
	{
		ServiceOrderDetail Get(int id, Int64 nUserID); 
		List<ServiceOrderDetail> Gets(int id, Int64 nUserID);
		List<ServiceOrderDetail> Gets( string sSQL, Int64 nUserID);
	}
	#endregion
}
