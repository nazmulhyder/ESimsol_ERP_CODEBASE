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
	#region FNProductionConsumption  
	public class FNProductionConsumption : BusinessObject
	{	
		public FNProductionConsumption()
		{
			FNPConsumptionID = 0; 
			FNProductionID = 0;
            FNPBatchID = 0;
			ProductID = 0; 
			LotID = 0; 
			Qty = 0; 
			MUID = 0; 
			ProductCode = ""; 
			ProductName = ""; 
			LotNo = "";
            MUName = "";
            Symbol = "";
            CurrencySymbol = "";
            LotBalance = 0;
            UnitPrice = 0;
            FNBatchCardID = 0;
            FNRDetailID = 0;
            QCStatus = EnumQCStatus.Yet_To_QC;
			ErrorMessage = "";
            IsQCDone = false;
		}

		#region Property
		public int FNPConsumptionID { get; set; }
		public int FNProductionID { get; set; }
		public int ProductID { get; set; }
		public int LotID { get; set; }
        public double Qty { get; set; }
        public double UnitPrice { get; set; }
		public int MUID { get; set; }
		public string ProductCode { get; set; }
		public string ProductName { get; set; }
        public string LotNo { get; set; }
        public string FNRNo { get; set; }
		public string MUName { get; set; }
        public double LotBalance { get; set; }
        public string Symbol { get; set; }
        public int FNPBatchID { get; set; }
        public int FNRDetailID { get; set; }
        public string CurrencySymbol { get; set; }
        public bool IsQCDone { get; set; }
        
        public string ErrorMessage { get; set; }
		#endregion 

		#region Derived Property
        public int FNBatchCardID { get; set; }
        public EnumQCStatus QCStatus { get; set; }
        public FNProduction FNProduction { get; set; }
        public double YetToConsumption
        {
            get
            {
                return this.LotBalance + this.Qty;
            }
        }
        public string IsQCDoneSt
        {
            get
            {
                if (this.IsQCDone) return "Comsume";
                else return "Wating For QC";
            }
        }
		#endregion 

		#region Functions 
		public static List<FNProductionConsumption> Gets(int id, long nUserID)
		{
			return FNProductionConsumption.Service.Gets(id, nUserID);
		}
		public static List<FNProductionConsumption> Gets(string sSQL, long nUserID)
		{
			return FNProductionConsumption.Service.Gets(sSQL,nUserID);
		}
		public FNProductionConsumption Get(int id, long nUserID)
		{
			return FNProductionConsumption.Service.Get(id,nUserID);
		}
		public FNProductionConsumption Save(long nUserID)
		{
			return FNProductionConsumption.Service.Save(this,nUserID);
		}
		public  string  Delete(int id, long nUserID)
		{
			return FNProductionConsumption.Service.Delete(id,nUserID);
		}
		#endregion

		#region ServiceFactory
		internal static IFNProductionConsumptionService Service
		{
			get { return (IFNProductionConsumptionService)Services.Factory.CreateService(typeof(IFNProductionConsumptionService)); }
		}
		#endregion


    }
	#endregion

	#region IFNProductionConsumption interface
	public interface IFNProductionConsumptionService 
	{
		FNProductionConsumption Get(int id, Int64 nUserID); 
		List<FNProductionConsumption> Gets(int id, Int64 nUserID);
		List<FNProductionConsumption> Gets( string sSQL, Int64 nUserID);
		string Delete(int id, Int64 nUserID);
 		FNProductionConsumption Save(FNProductionConsumption oFNProductionConsumption, Int64 nUserID);
	}
	#endregion
}
