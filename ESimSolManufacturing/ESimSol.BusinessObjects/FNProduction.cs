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
	#region FNProduction  
	public class FNProduction : BusinessObject
	{	
		public FNProduction()
		{
			FNProductionID = 0; 
			FNMachineID = 0; 
			FNTPID = 0;
			IssueDate = DateTime.Now;
            //StartDateTime = Convert.ToDateTime("01 Jan 1990"); 
            //EndDateTime =  Convert.ToDateTime("01 Jan 1990"); //DateTime.MinValue; 
            StartDateTime = DateTime.Now;
            EndDateTime = DateTime.MinValue;
			FNMachineName = "";
			FNMachineName = "";
            NoOfBatchInOrder = "";
            Params = "";
            IsDelatable = true;
            FNTreatment = EnumFNTreatment.None;
            //FNProcess = EnumFNProcess.None;
            FNProcess = string.Empty;
            FNProductionConsumptions = new List<FNProductionConsumption>();
            FNProductionBatchs = new List<FNProductionBatch>();
            FNBatchCards = new List<FNBatchCard>();
			ErrorMessage = "";
            FNDyeingType = EnumFNDyeingType.None;
		}

		#region Property
		public int FNProductionID { get; set; }
		public int FNMachineID { get; set; }
		public int FNTPID { get; set; }
		public DateTime IssueDate { get; set; }
		public DateTime StartDateTime { get; set; }
		public DateTime EndDateTime { get; set; }
		public string FNMachineName { get; set; }
        public string NoOfBatchInOrder { get; set; }
        public EnumFNTreatment FNTreatment { get; set; }
        //public EnumFNProcess FNProcess { get; set; }
        public string FNProcess { get; set; }
        public bool IsDelatable { get; set; }
        public string Params { get; set; }
		public string ErrorMessage { get; set; }
        public EnumFNDyeingType FNDyeingType { get; set; }
		#endregion 

        #region Derived Property
        public List<FNBatchCard> FNBatchCards { get; set; }
        public List<FNProductionConsumption> FNProductionConsumptions { get; set; }
        public List<FNProductionBatch> FNProductionBatchs { get; set; }
		public string IssueDateSt
		{
			get
			{
				return IssueDate.ToString("dd MMM yyyy") ; 
			}
		}
        public int FNTreatmentInt
        {
            get
            {
                return (int)this.FNTreatment;
            }
        }
		public string StartDateTimeSt
		{
			get
			{
                if (this.StartDateTime == DateTime.MinValue)
                {
                    return "";
                }
                else
                {
                    return StartDateTime.ToString("dd MMM yyyy hh:mm:ss tt");
                }
			}
		}
		public string EndDateTimeSt
		{
			get
            {
                if (this.EndDateTime.Year <= 1950)
                {
                    return "";
                }
                else
                {
                    return EndDateTime.ToString("dd MMM yyyy hh:mm:ss tt"); 
                }
			}
		}
        public string FNTreatmentSt
        {
            get
            {
                return EnumObject.jGet(this.FNTreatment);
            }
        }
     
		#endregion 

		#region Functions 
		public static List<FNProduction> Gets(long nUserID)
		{
			return FNProduction.Service.Gets(nUserID);
		}
		public static List<FNProduction> Gets(string sSQL, long nUserID)
		{
			return FNProduction.Service.Gets(sSQL,nUserID);
		}
		public FNProduction Get(int id, long nUserID)
		{
			return FNProduction.Service.Get(id,nUserID);
		}
		public FNProduction Save(long nUserID)
		{
			return FNProduction.Service.Save(this,nUserID);
		}
        public FNProduction Run(long nUserID)
        {
            return FNProduction.Service.Run(this, nUserID);
        }
        public FNProduction RunOut(long nUserID)
        {
            return FNProduction.Service.RunOut(this, nUserID);
        }
		public  string  Delete(int id, long nUserID)
		{
			return FNProduction.Service.Delete(id,nUserID);
		}
		#endregion

		#region ServiceFactory
		internal static IFNProductionService Service
		{
			get { return (IFNProductionService)Services.Factory.CreateService(typeof(IFNProductionService)); }
		}
		#endregion


    }
	#endregion

	#region IFNProduction interface
	public interface IFNProductionService 
	{
		FNProduction Get(int id, Int64 nUserID); 
		List<FNProduction> Gets(Int64 nUserID);
		List<FNProduction> Gets( string sSQL, Int64 nUserID);
		string Delete(int id, Int64 nUserID);
 		FNProduction Save(FNProduction oFNProduction, Int64 nUserID);
        FNProduction Run(FNProduction oFNProduction, Int64 nUserID);
        FNProduction RunOut(FNProduction oFNProduction, Int64 nUserID);
	}
	#endregion
}
