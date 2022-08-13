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
	#region RMConsumption  
	public class RMConsumption : BusinessObject
	{	
		public RMConsumption()
		{
			RMConsumptionID = 0; 
			ConsumptionNo = ""; 
			ConsumptionDate = DateTime.Now; 
			BUID = 0;
            Remarks = "Raw Material Consumption"; 
			ApprovedBy = 0; 
			ApprovedByName = "";
            BUName = "";
            BUShortName = "";
            TriggeTypeInt = 0;
            ConsumptionAmount = 0;
            RMConsumptionDetails = new List<RMConsumptionDetail>();
			ErrorMessage = "";
		}

		#region Property
		public int RMConsumptionID { get; set; }
		public string ConsumptionNo { get; set; }
		public DateTime ConsumptionDate { get; set; }
		public int BUID { get; set; }
		public string Remarks { get; set; }
		public int ApprovedBy { get; set; }
		public string ApprovedByName { get; set; }
        public string BUName { get; set; }
        public string BUShortName { get; set; }
        public int TriggeTypeInt { get; set; }
        public double ConsumptionAmount { get; set; }
		public string ErrorMessage { get; set; }
		#endregion 

		#region Derived Property
        public List<RMConsumptionDetail> RMConsumptionDetails { get; set; }
		public string ConsumptionDateST
		{
			get
			{
				return ConsumptionDate.ToString("dd MMM yyyy") ; 
			}
		}
		#endregion 

		#region Functions 
		public static List<RMConsumption> Gets(long nUserID)
		{
			return RMConsumption.Service.Gets(nUserID);
		}
		public static List<RMConsumption> Gets(string sSQL, long nUserID)
		{
			return RMConsumption.Service.Gets(sSQL,nUserID);
		}
		public RMConsumption Get(int id, long nUserID)
		{
			return RMConsumption.Service.Get(id,nUserID);
		}
		public RMConsumption Save(long nUserID)
		{
			return RMConsumption.Service.Save(this,nUserID);
		}
        public RMConsumption GetSuggestMaterialConsumptionDate(string sSQl, long nUserID)
        {
            return RMConsumption.Service.GetSuggestMaterialConsumptionDate(sSQl, nUserID);
        }
        public string YetToMaterialConsumptionDate(string sSQl, long nUserID)
        {
            return RMConsumption.Service.YetToMaterialConsumptionDate(sSQl, nUserID);
        }
        public RMConsumption Approved(long nUserID)
        {
            return RMConsumption.Service.Approved(this, nUserID);
        }
		public  string  Delete(int id, long nUserID)
		{
			return RMConsumption.Service.Delete(id,nUserID);
		}
		#endregion

		#region ServiceFactory
		internal static IRMConsumptionService Service
		{
			get { return (IRMConsumptionService)Services.Factory.CreateService(typeof(IRMConsumptionService)); }
		}
		#endregion
	}
	#endregion

	#region IRMConsumption interface
	public interface IRMConsumptionService 
	{
		RMConsumption Get(int id, Int64 nUserID); 
		List<RMConsumption> Gets(Int64 nUserID);
		List<RMConsumption> Gets( string sSQL, Int64 nUserID);
		string Delete(int id, Int64 nUserID);
 		RMConsumption Save(RMConsumption oRMConsumption, Int64 nUserID);
        RMConsumption GetSuggestMaterialConsumptionDate(string sSQl, Int64 nUserID);
        string YetToMaterialConsumptionDate(string sSQl, Int64 nUserID);        
        RMConsumption Approved(RMConsumption oRMConsumption, Int64 nUserID);        
	}
	#endregion
}
