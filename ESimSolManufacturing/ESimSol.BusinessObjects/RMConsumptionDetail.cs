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
	#region RMConsumptionDetail  
	public class RMConsumptionDetail : BusinessObject
	{	
		public RMConsumptionDetail()
		{
			RMConsumptionDetailID = 0; 
			RMConsumptionID = 0; 
			ITransactionID = 0; 
			ProductID = 0; 
			LotID = 0; 
			WUID = 0; 
			MUnitID = 0; 
			Qty = 0; 
			UnitPrice = 0; 
			Amount = 0; 
			ProductCode = ""; 
			ProductName = ""; 
			WUName = ""; 
			MUName = ""; 
			LotNo = "";
            RefNo = "";
			ErrorMessage = "";
		}

		#region Property
		public int RMConsumptionDetailID { get; set; }
		public int RMConsumptionID { get; set; }
		public int ITransactionID { get; set; }
		public int ProductID { get; set; }
		public int LotID { get; set; }
		public int WUID { get; set; }
		public int MUnitID { get; set; }
		public double Qty { get; set; }
		public double UnitPrice { get; set; }
		public double Amount { get; set; }
		public string ProductCode { get; set; }
		public string ProductName { get; set; }
		public string WUName { get; set; }
		public string MUName { get; set; }
		public string LotNo { get; set; }
        public string RefNo { get; set; }
		public string ErrorMessage { get; set; }
		#endregion 

		#region Derived Property
		#endregion 

		#region Functions 
		public static List<RMConsumptionDetail> Gets(int id, long nUserID)
		{
			return RMConsumptionDetail.Service.Gets(id, nUserID);
		}
		public static List<RMConsumptionDetail> Gets(string sSQL, long nUserID)
		{
			return RMConsumptionDetail.Service.Gets(sSQL,nUserID);
		}
		
		#endregion

		#region ServiceFactory
		internal static IRMConsumptionDetailService Service
		{
			get { return (IRMConsumptionDetailService)Services.Factory.CreateService(typeof(IRMConsumptionDetailService)); }
		}
		#endregion
	}
	#endregion

	#region IRMConsumptionDetail interface
	public interface IRMConsumptionDetailService 
	{
	
		List<RMConsumptionDetail> Gets(int id, Int64 nUserID);
		List<RMConsumptionDetail> Gets( string sSQL, Int64 nUserID);
	
	}
	#endregion
}
