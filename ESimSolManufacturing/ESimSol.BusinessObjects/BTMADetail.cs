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
	#region BTMADetail  
	public class BTMADetail : BusinessObject
	{	
		public BTMADetail()
		{
			BTMADetailID = 0; 
			BTMAID = 0; 
			ProductID = 0; 
			ProductName = ""; 
			Qty = 0; 
			MUnitID = 0; 
			UnitPrice = 0;
            MUName = "";
            ErrorMessage = "";
            Remarks = "";
            PINo = "";
            MUNameTwo = "";
            QtyTwo = 0;
		}

		#region Property
		public int BTMADetailID { get; set; }
		public int BTMAID { get; set; }
		public int ProductID { get; set; }
		public string ProductName { get; set; }
		public double Qty { get; set; }
        public double QtyTwo { get; set; }
		public int MUnitID { get; set; }
		public double UnitPrice { get; set; }
        public string MUName { get; set; }
        public string MUNameTwo { get; set; }
        public string Remarks { get; set; }
		public string ErrorMessage { get; set; }
        public string PINo { get; set; }
        public double Amount { get { return this.Qty * this.UnitPrice; } }
        public double QtyInKg
        {
            get
            {
                return Global.GetKG(this.Qty, 2);
            }
        }
		#endregion 

		#region Functions 
		public static List<BTMADetail> Gets(long nUserID)
		{
			return BTMADetail.Service.Gets(nUserID);
		}
		public static List<BTMADetail> Gets(string sSQL, long nUserID)
		{
			return BTMADetail.Service.Gets(sSQL,nUserID);
		}
        public static List<BTMADetail> Gets(int nBTMAID, long nUserID)
        {
            return BTMADetail.Service.Gets(nBTMAID, nUserID);
        }
		public BTMADetail Get(int id, long nUserID)
		{
			return BTMADetail.Service.Get(id,nUserID);
		}
		public BTMADetail Save(long nUserID)
		{
			return BTMADetail.Service.Save(this,nUserID);
		}
		public  string  Delete(int id, long nUserID)
		{
			return BTMADetail.Service.Delete(id,nUserID);
		}
		#endregion

		#region ServiceFactory
		internal static IBTMADetailService Service
		{
			get { return (IBTMADetailService)Services.Factory.CreateService(typeof(IBTMADetailService)); }
		}
		#endregion
    }
	#endregion

	#region IBTMADetail interface
	public interface IBTMADetailService 
	{
		BTMADetail Get(int id, Int64 nUserID); 
		List<BTMADetail> Gets(Int64 nUserID);
        List<BTMADetail> Gets(string sSQL, Int64 nUserID);
        List<BTMADetail> Gets(int id, Int64 nUserID);
		string Delete(int id, Int64 nUserID);
 		BTMADetail Save(BTMADetail oBTMADetail, Int64 nUserID);
	}
	#endregion
}
