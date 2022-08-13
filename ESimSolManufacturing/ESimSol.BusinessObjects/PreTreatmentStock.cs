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
	#region PreTreatmentStock  
	public class PreTreatmentStock : BusinessObject
	{	
		public PreTreatmentStock()
		{
			ProductID = 0; 
			ProductName = ""; 
			ProductCode = ""; 
			LotID = 0; 
			DisburseQty = 0; 
			ConsumptionQty = 0;
            YetToConsumptionQty = 0;
            TreatmentID = 0;
            LotNo = "";
			ErrorMessage = "";
		}

		#region Property
		public int ProductID { get; set; }
		public string ProductName { get; set; }
		public string ProductCode { get; set; }
		public int LotID { get; set; }
		public double DisburseQty { get; set; }
		public double ConsumptionQty { get; set; }
        public double YetToConsumptionQty { get; set; }
        public string LotNo { get; set; }
        public int TreatmentID { get; set; }
		public string ErrorMessage { get; set; }
		#endregion 

		#region Derived Property
		#endregion 

		#region Functions 
		
		public static List<PreTreatmentStock> Gets(string sSQL, long nUserID)
		{
			return PreTreatmentStock.Service.Gets(sSQL,nUserID);
		}
        public static List<PreTreatmentStock> GetsStock(int ProductID,int TreatmentProcess, long nUserID)
        {
            return PreTreatmentStock.Service.GetsStock(ProductID,TreatmentProcess, nUserID);
        }
		
		#endregion

		#region ServiceFactory
		internal static IPreTreatmentStockService Service
		{
			get { return (IPreTreatmentStockService)Services.Factory.CreateService(typeof(IPreTreatmentStockService)); }
		}
		#endregion
	}
	#endregion

	#region IPreTreatmentStock interface
	public interface IPreTreatmentStockService 
	{
		List<PreTreatmentStock> Gets( string sSQL, Int64 nUserID);
        List<PreTreatmentStock> GetsStock(int ProductID, int TreatmentProcess, Int64 nUserID);
	}
	#endregion
}
