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
	#region FNTreatmentProcess  
	public class FNTreatmentProcess : BusinessObject
	{	
		public FNTreatmentProcess()
		{
			FNTPID = 0; 
			Description = " ";
            FNTreatment = EnumFNTreatment.None;
            FNProcess = string.Empty;
            FNTreatmentInt = 0;
            FNProcessInt = 0;
            Code = 0;
            IsProduction = false;
            Childrens = new List<FNTreatmentProcess>();
			ErrorMessage = "";
		}

		#region Property
		public int FNTPID { get; set; }
		public string Description { get; set; }
        public EnumFNTreatment FNTreatment { get; set; }
        public int FNTreatmentInt { get; set; }
        //public EnumFNProcess FNProcess { get; set; }
        public string FNProcess { get; set; }
        public int FNProcessInt { get; set; }
        public int Code { get; set; }
        public bool IsProduction { get; set; }
		public string ErrorMessage { get; set; }
		#endregion 

		#region Derived Property
        public List<FNTreatmentProcess> Childrens { get; set; }
        public string FNTreatmentSt
        {
            get
            {
                return EnumObject.jGet(this.FNTreatment);
            }
        }
        //public string FNProcessSt
        //{
        //    get
        //    {
        //        return EnumObject.jGet(this.FNProcess);
        //    }
        //}
        public int FNMachineID { get; set; }
		#endregion 

		#region Functions 
		public static List<FNTreatmentProcess> Gets(long nUserID)
		{
			return FNTreatmentProcess.Service.Gets(nUserID);
		}
		public static List<FNTreatmentProcess> Gets(string sSQL, long nUserID)
		{
			return FNTreatmentProcess.Service.Gets(sSQL,nUserID);
		}
		public FNTreatmentProcess Get(int id, long nUserID)
		{
			return FNTreatmentProcess.Service.Get(id,nUserID);
		}
		public FNTreatmentProcess Save(long nUserID)
		{
			return FNTreatmentProcess.Service.Save(this,nUserID);
		}
		public  string  Delete(int id, long nUserID)
		{
			return FNTreatmentProcess.Service.Delete(id,nUserID);
		}
		#endregion

		#region ServiceFactory
		internal static IFNTreatmentProcessService Service
		{
			get { return (IFNTreatmentProcessService)Services.Factory.CreateService(typeof(IFNTreatmentProcessService)); }
		}
		#endregion

    }
	#endregion

	#region IFNTreatmentProcess interface
	public interface IFNTreatmentProcessService 
	{
		FNTreatmentProcess Get(int id, Int64 nUserID); 
		List<FNTreatmentProcess> Gets(Int64 nUserID);
		List<FNTreatmentProcess> Gets( string sSQL, Int64 nUserID);
		string Delete(int id, Int64 nUserID);
 		FNTreatmentProcess Save(FNTreatmentProcess oFNTreatmentProcess, Int64 nUserID);
	}
	#endregion
}
