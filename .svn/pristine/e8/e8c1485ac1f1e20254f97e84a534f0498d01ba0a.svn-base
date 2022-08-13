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
	#region FGCost  
	public class FGCost : BusinessObject
	{	
		public FGCost()
		{
			FGCostID = 0; 
			ITransactionID = 0; 
			QCID = 0; 
			RMID = 0; 
			MUnitID = 0; 
			RMQty = 0; 
			UnitPrice = 0; 
			Amount = 0; 
			MUName = ""; 
			ProductCode = ""; 
			ProductName = ""; 
			LotNo = "";
            RMRequisitionNo = "";
            CurrencySymbol = "";
			ErrorMessage = "";
		}

		#region Property
		public int FGCostID { get; set; }
		public int ITransactionID { get; set; }
		public int QCID { get; set; }
		public int RMID { get; set; }
		public int MUnitID { get; set; }
		public double RMQty { get; set; }
		public double UnitPrice { get; set; }
		public double Amount { get; set; }
		public string MUName { get; set; }
		public string ProductCode { get; set; }
		public string ProductName { get; set; }
		public string LotNo { get; set; }
        public string RMRequisitionNo { get; set; }
        public string CurrencySymbol { get; set; }
		public string ErrorMessage { get; set; }
		#endregion 

		#region Derived Property
		#endregion 

		#region Functions 
		public static List<FGCost> Gets(int nQCID, long nUserID)
		{
            return FGCost.Service.Gets(nQCID,nUserID);
		}
		public static List<FGCost> Gets(string sSQL, long nUserID)
		{
			return FGCost.Service.Gets(sSQL,nUserID);
		}
		
	
		#endregion

		#region ServiceFactory
		internal static IFGCostService Service
		{
			get { return (IFGCostService)Services.Factory.CreateService(typeof(IFGCostService)); }
		}
		#endregion
	}
	#endregion

	#region IFGCost interface
	public interface IFGCostService 
	{
		List<FGCost> Gets(int nQCID, Int64 nUserID);
		List<FGCost> Gets( string sSQL, Int64 nUserID);
     
	}
	#endregion
}
