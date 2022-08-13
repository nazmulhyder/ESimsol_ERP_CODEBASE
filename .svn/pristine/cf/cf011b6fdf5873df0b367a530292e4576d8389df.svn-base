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
	#region OrderStepGroupDetail  
	public class OrderStepGroupDetail : BusinessObject
	{	
		public OrderStepGroupDetail()
		{
			OrderStepGroupDetailID = 0; 
			OrderStepGroupID = 0; 
			OrderStepID = 0; 
			Sequence = 0; 
			StepName = "";
            bIsUp = true;
			ErrorMessage = "";
		}

		#region Property
		public int OrderStepGroupDetailID { get; set; }
		public int OrderStepGroupID { get; set; }
		public int OrderStepID { get; set; }
		public int Sequence { get; set; }
		public string StepName { get; set; }
        public bool bIsUp { get; set; }

		public string ErrorMessage { get; set; }
		#endregion 

		#region Derived Property
		#endregion 

		#region Functions 
		public static List<OrderStepGroupDetail> Gets(int id, long nUserID)
		{
			return OrderStepGroupDetail.Service.Gets(id, nUserID);
		}
		public static List<OrderStepGroupDetail> Gets(string sSQL, long nUserID)
		{
			return OrderStepGroupDetail.Service.Gets(sSQL,nUserID);
		}
		public OrderStepGroupDetail Get(int id, long nUserID)
		{
			return OrderStepGroupDetail.Service.Get(id,nUserID);
		}
	
		#endregion

		#region ServiceFactory
		internal static IOrderStepGroupDetailService Service
		{
			get { return (IOrderStepGroupDetailService)Services.Factory.CreateService(typeof(IOrderStepGroupDetailService)); }
		}
		#endregion
	}
	#endregion

	#region IOrderStepGroupDetail interface
	public interface IOrderStepGroupDetailService 
	{
		OrderStepGroupDetail Get(int id, Int64 nUserID); 
		List<OrderStepGroupDetail> Gets(int id, Int64 nUserID);
		List<OrderStepGroupDetail> Gets( string sSQL, Int64 nUserID);
	
	}
	#endregion
}
