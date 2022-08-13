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
	#region FNProductionBatch  
	public class FNProductionBatch : BusinessObject
	{	
		public FNProductionBatch()
		{
			FNPBatchID = 0; 
			FNProductionID = 0; 
			FNBatchID = 0; 
			StartQty = 0; 
			EndQty = 0; 
			StartDateTime = DateTime.Now; 
			EndDateTime = Convert.ToDateTime("01 Jan 1990"); 
			StartBatcherID = 0; 
			EndBatcherID = 0; 
			MachineSpeed = 0; 
			StartWidth = 0;
            EndWidth = 0; 
			FlameIntensity = 0; 
			FlamePosition = 0; 
			Pressure_Bar = 0; 
			Temp_C = 0; 
			Remark = ""; 
			StartBatcherName = ""; 
			EndBatcherName = ""; 
			BatchNo = "";
            FNExONo = "";
            Ref_FNPBatchID = 0;
			ErrorMessage = "";
            FNProcess = "";
            ShiftID = 0;
            ShadeID = 0;
            FNTPID = 0;
            PH = 0;
            DeriveFNBatchID = 0;
            FNBatchCardID = 0;
            Params = string.Empty;
            FNPBQualityID = 0;
            QCStatus = EnumQCStatus.Yet_To_QC;
            FNProductionConsumptions = new List<FNProductionConsumption>();
            IsProduction = false;
            FNTreatmentSubProcessID = 0;
		}

		#region Property
        public int FNPBatchID { get; set; }
        public int FNBatchCardID { get; set; }
		public int FNProductionID { get; set; }
        public int FNBatchID { get; set; }
        public int FNExOID { get; set; }
        public int FNMachineID { get; set; }
		public double StartQty { get; set; }
		public double EndQty { get; set; }
		public DateTime StartDateTime { get; set; }
		public DateTime EndDateTime { get; set; }
		public int StartBatcherID { get; set; }
		public int EndBatcherID { get; set; }
		public double MachineSpeed { get; set; }
        public double StartWidth { get; set; }
        public double EndWidth { get; set; }
        public double FlameIntensity { get; set; }
        public double FlamePosition { get; set; }
		public double Pressure_Bar { get; set; }
		public double Temp_C { get; set; }
		public string Remark { get; set; }
		public string StartBatcherName { get; set; }
		public string EndBatcherName { get; set; }
        public string FNExONo { get; set; }
		public string BatchNo { get; set; }
        public int Ref_FNPBatchID { get; set; }
        public string ErrorMessage { get; set; }
        public string FNProcess { get; set; }
        public int FNTPID { get; set; }
        public EnumShade ShadeID {get; set;}
        public int ShiftID  {get; set;}
        public double PH  {get; set;}
        public int DeriveFNBatchID  {get; set;}
        public double FNExOQty { get; set; }
        public string Params { get; set; }
        public int FNPBQualityID { get; set; }
        public double CausticStrength { get; set; }
        public EnumQCStatus QCStatus { get; set; }
        public bool IsProduction { get; set; }
        public int FNTreatmentSubProcessID { get; set; }
		#endregion 

		#region Derived Property
        public List<FNMachine> FNMachines { get; set; }
        public List<FNProductionConsumption> FNProductionConsumptions { get; set; }
        public string ShiftName { get; set; }
        public string QCStatusStr { get { return EnumObject.jGet(this.QCStatus); } }
        public string UserName { get; set; }
        public FNProduction FNProduction { get; set; }
        public string StartDateTimeSt
		{
			get
			{
                return this.StartDateTime.ToString("dd MMM yyyy hh:mm tt"); 
			}
		}
        public string IsProductionInString
        {
            get
            {
                if (IsProduction) return "Reproduction";
                return "Fresh";
            }
        }
        public string EndDateTimeSt
		{
			get
			{
                if (this.EndDateTime == Convert.ToDateTime("01 Jan 0001 12:00:00 AM") || this.EndDateTime == Convert.ToDateTime("01 Jan 1990"))
                {
                    return this.StartDateTime.ToString("dd MMM yyyy hh:mm tt");
                }
                else
                {
                    return this.EndDateTime.ToString("dd MMM yyyy hh:mm tt");
                }
			}
		}
        public string ShadeStr { get { return this.ShadeID.ToString(); } }

        public double Pressure_Bar_Actual { get; set; }
        public double PH_Actual { get; set; }
        public double Temp_C_Actual { get; set; }
        public double MachineSpeed_Actual { get; set; }
        public double CausticStrength_Actual { get; set; }
        public double FlameIntensity_Actual { get; set; }
		#endregion 

		#region Functions 
		public static List<FNProductionBatch> Gets(long nUserID)
		{
			return FNProductionBatch.Service.Gets(nUserID);
		}
        public static List<FNProductionBatch> Gets(int id, long nUserID)
        {
            return FNProductionBatch.Service.Gets(id,nUserID);
        }
		public static List<FNProductionBatch> Gets(string sSQL, long nUserID)
		{
			return FNProductionBatch.Service.Gets(sSQL,nUserID);
		}
		public FNProductionBatch Get(int id, long nUserID)
		{
			return FNProductionBatch.Service.Get(id,nUserID);
		}
        public FNProductionBatch Save(long nUserID)
        {
            return FNProductionBatch.Service.Save(this, nUserID);
        }
        public FNProductionBatch Save_Process(long nUserID)
        {
            return FNProductionBatch.Service.Save_Process(this, nUserID);
        }
		public  string  Delete(int id, long nUserID)
		{
			return FNProductionBatch.Service.Delete(id,nUserID);
		}
        public FNProductionBatch QCRequest(EnumDBOperation eDBOperation, long nUserID)
        {
            return FNProductionBatch.Service.QCRequest(this, eDBOperation, nUserID);
        }
		#endregion

		#region ServiceFactory
		internal static IFNProductionBatchService Service
		{
			get { return (IFNProductionBatchService)Services.Factory.CreateService(typeof(IFNProductionBatchService)); }
		}
		#endregion

    }
	#endregion

	#region IFNProductionBatch interface
	public interface IFNProductionBatchService 
	{
		FNProductionBatch Get(int id, Int64 nUserID); 
		List<FNProductionBatch> Gets(Int64 nUserID);
        List<FNProductionBatch> Gets(int id, Int64 nUserID);
		List<FNProductionBatch> Gets( string sSQL, Int64 nUserID);
		string Delete(int id, Int64 nUserID);
        FNProductionBatch Save(FNProductionBatch oFNProductionBatch, Int64 nUserID);
        FNProductionBatch Save_Process(FNProductionBatch oFNProductionBatch, Int64 nUserID);
        FNProductionBatch QCRequest(FNProductionBatch oFNProduction, EnumDBOperation eDBOperation, Int64 nUserID);
	}
	#endregion
}
