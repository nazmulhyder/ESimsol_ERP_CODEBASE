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
#region FNProductionPlan  
	public class FNProductionPlan : BusinessObject
	{	
		public FNProductionPlan()
		{
			FNPPID = 0; 
			PlanNo = ""; 
			FSCDID = 0; 
			FNMachineID = 0; 
			Qty = 0; 
			StartTime = DateTime.Now; 
			EndTime = DateTime.Now;
            FabricNo = ""; 
			ColorName = "";
            FNExONo = "";
			FNMachineNameCode = "";
            ReviseCount = 0;
            FNExOID = 0;
            FinishWidth = "";
            BuyerName = "";
            BuyerID = 0;
            CountName  = "";
            Construction  = "";
            FinishTypeName = "";
            GSM = 0;
            ReqFinishedGSM = 0;
            YetToBatchQty = 0;
            FNEODQty = 0;
            GreyQty = 0;
            FabricWeaveName = "";
			ErrorMessage = "";
            ReceiveDate = DateTime.Now;
            DateWiseSequence = 0;
            FNTreatment = 0;
            BatchNo = "";
            BUID = 0;
            Params = "";
            Note = "";
            PlanStatus = EnumFabricPlanStatus.Initialize;
            ExeNo = "";
            ContractorID = 0;
            ContractorName = "";
            SCDate = DateTime.Now;
            FabricDesignName = "";
            ReportType = 0;
		}

		#region Property
        public DateTime ReceiveDate { get; set; }
        public int DateWiseSequence { get; set; }
        public int FNTreatment { get; set; }
		public int FNPPID { get; set; }
		public string PlanNo { get; set; }
        public EnumFabricPlanStatus PlanStatus { get; set; }
		public int FSCDID { get; set; }
		public int FNMachineID { get; set; }
		public double Qty { get; set; }
		public DateTime StartTime { get; set; }
		public DateTime EndTime { get; set; }
        public string FabricNo { get; set; }
		public string ColorName { get; set; }
		public string FNMachineNameCode { get; set; }
        public string FNExONo { get; set; }
        public int FEOID { get; set; }
        public int FNExOID { get;set;}
        public double FNEODQty { get; set; }
        public string FinishWidth { get; set; }
        public bool IsInHouse { get; set; }
        public int ReviseCount { get; set; }
        public double YetToBatchQty { get; set; }
        public EnumOrderType OrderType { get; set; }
        public string     BuyerName { get; set; }
        public int BuyerID { get; set; }
        public string CountName  { get; set; }
        public string Construction  { get; set; }
        public double  GSM { get; set; }
        public double ReqFinishedGSM { get; set; }
        public double GreyQty { get; set; }
        public string FinishTypeName { get; set; }
		public string ErrorMessage { get; set; }
        public string BatchNo { get; set; }
        public int BUID { get; set; }
        public string Note { get; set; }
        public string ExeNo { get; set; }
        public int ContractorID { get; set; }
        public string ContractorName { get; set; }
        public DateTime SCDate { get; set; }
        public string FabricWeaveName { get; set; }
        public string FabricDesignName { get; set; }
        //print
        public int ReportType { get; set; }

		#endregion

		#region Derived Property
        public bool IsIncreaseTime { get; set; }
        public string Params { get; set; }
        public string OrderNo
        {
            get
            {
                return this.FNExONo;
            }
        }
        public string PlanStatusSt
        {
            get
            {
                return EnumObject.jGet(this.PlanStatus);
            }
        }
        public EnumFNTreatment EnumFNTreatment { get; set; }
        public string TreatmentProcess { get { return this.EnumFNTreatment.ToString(); } }
        public string SCDateSt
        {
            get
            {
                return this.SCDate.ToString("dd MMM yyyy");
            }
        }
		public string StartTimeInString 
		{
			get
			{
				return StartTime.ToString("dd MMM yyyy hh:mm:ss tt") ; 
			}
		}
        public string ReceiveDateStr
        {
            get
            {
                return this.ReceiveDate.ToString("dd MMM yyyy");
            }
        }
		public string EndTimeInString 
		{
			get
			{
                return EndTime.ToString("dd MMM yyyy hh:mm:ss tt"); 
			}
		}
        public string StartTimeSt
        {
            get
            {
                return StartTime.ToString("dd MMM yyyy");
            }
        }

        public string EndTimeSt
        {
            get
            {
                return EndTime.ToString("dd MMM yyyy");
            }
        }
        public string StartTimeTS
        {
            get
            {
                return this.StartTime.ToString("HH:mm");

            }
        }
        public string EndTimeTS
        {
            get
            {
                return this.EndTime.ToString("HH:mm");

            }
        }
		#endregion 

		#region Functions 
		public static List<FNProductionPlan> Gets(long nUserID)
		{
			return FNProductionPlan.Service.Gets(nUserID);
		}
		public static List<FNProductionPlan> Gets(string sSQL, long nUserID)
		{
			return FNProductionPlan.Service.Gets(sSQL,nUserID);
		}
		public FNProductionPlan Get(int id, long nUserID)
		{
			return FNProductionPlan.Service.Get(id,nUserID);
		}
		public FNProductionPlan Save(long nUserID)
		{
			return FNProductionPlan.Service.Save(this,nUserID);
		}
		public  string  Delete(int id, long nUserID)
		{
			return FNProductionPlan.Service.Delete(id,nUserID);
		}
		#endregion

		#region ServiceFactory
		internal static IFNProductionPlanService Service
		{
			get { return (IFNProductionPlanService)Services.Factory.CreateService(typeof(IFNProductionPlanService)); }
		}
		#endregion
	}
	#endregion

	#region IFNProductionPlan interface
	public interface IFNProductionPlanService 
	{
		FNProductionPlan Get(int id, Int64 nUserID); 
		List<FNProductionPlan> Gets(Int64 nUserID);
		List<FNProductionPlan> Gets( string sSQL, Int64 nUserID);
		string Delete(int id, Int64 nUserID);
 		FNProductionPlan Save(FNProductionPlan oFNProductionPlan, Int64 nUserID);
	}
	#endregion
}
