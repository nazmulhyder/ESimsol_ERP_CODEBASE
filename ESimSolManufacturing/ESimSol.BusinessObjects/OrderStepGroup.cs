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
	#region OrderStepGroup  
	public class OrderStepGroup : BusinessObject
	{	
		public OrderStepGroup()
		{
			OrderStepGroupID = 0; 
			GroupName = ""; 
			Note = "";
            bIsInitialSave = false;
            BUID = 0;
            OrderStepGroupDetails = new List<OrderStepGroupDetail>();
			ErrorMessage = "";
		}

		#region Property
		public int OrderStepGroupID { get; set; }
		public string GroupName { get; set; }
		public string Note { get; set; }
        public bool bIsInitialSave { get; set; }
        public int BUID { get; set; }
		public string ErrorMessage { get; set; }
		#endregion 

		#region Derived Property
        public List<OrderStepGroupDetail> OrderStepGroupDetails { get; set; }
		#endregion 

		#region Functions 
		public static List<OrderStepGroup> Gets(long nUserID)
		{
			return OrderStepGroup.Service.Gets(nUserID);
		}
        public static List<OrderStepGroup> Gets(int BUID, long nUserID)
		{
			return OrderStepGroup.Service.Gets(BUID, nUserID);
		}
        
		public static List<OrderStepGroup> Gets(string sSQL, long nUserID)
		{
			return OrderStepGroup.Service.Gets(sSQL,nUserID);
		}
		public OrderStepGroup Get(int id, long nUserID)
		{
			return OrderStepGroup.Service.Get(id,nUserID);
		}
		public OrderStepGroup Save(long nUserID)
		{
			return OrderStepGroup.Service.Save(this,nUserID);
		}
		public  string  Delete(int id, long nUserID)
		{
			return OrderStepGroup.Service.Delete(id,nUserID);
		}
        public OrderStepGroup UpDown(OrderStepGroupDetail oOrderStepGroupDetail, long nUserID)
        {
            return OrderStepGroup.Service.UpDown(oOrderStepGroupDetail, nUserID);
        }
		#endregion

		#region ServiceFactory
		internal static IOrderStepGroupService Service
		{
			get { return (IOrderStepGroupService)Services.Factory.CreateService(typeof(IOrderStepGroupService)); }
		}
		#endregion
	}
	#endregion

	#region IOrderStepGroup interface
	public interface IOrderStepGroupService 
	{
		OrderStepGroup Get(int id, Int64 nUserID); 
		List<OrderStepGroup> Gets(Int64 nUserID);
        List<OrderStepGroup> Gets(int BUID, Int64 nUserID);
        
		List<OrderStepGroup> Gets( string sSQL, Int64 nUserID);
		string Delete(int id, Int64 nUserID);
 		OrderStepGroup Save(OrderStepGroup oOrderStepGroup, Int64 nUserID);
        OrderStepGroup UpDown(OrderStepGroupDetail oOrderStepGroupDetail, Int64 nUserID);
	}
	#endregion
}
