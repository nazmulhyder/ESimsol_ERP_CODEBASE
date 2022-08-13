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
	#region FNMachineProcess  
	public class FNMachineProcess : BusinessObject
	{	
		public FNMachineProcess()
		{
			FNMProcessID = 0; 
			FNMachineID = 0; 
			FNMachineNameCode = ""; 
			FNTPID = 0; 
			FNTreatment = EnumFNTreatment.None; 
			FNProcess = ""; 
			InActiveBy = 0; 
			InactiveByName = "";
            Description = "";
            Name = "";
			InActiveDate = DateTime.Now; 
			ErrorMessage = "";
		}

		#region Property
		public int FNMProcessID { get; set; }
		public int FNMachineID { get; set; }
		public string FNMachineNameCode { get; set; }
        public string Description { get; set; }
		public int FNTPID { get; set; }
        public EnumFNTreatment FNTreatment { get; set; }
        public string FNProcess { get; set; }
        public int FNTreatmentInt { get; set; }
        public int FNProcessInt { get; set; }
		public int InActiveBy { get; set; }
		public string InactiveByName { get; set; }
		public DateTime InActiveDate { get; set; }

        public string Name { get; set; }
        public string Params { get; set; }
		public string ErrorMessage { get; set; }
		#endregion 

		#region Derived Property
        public string FNTreatmentSt
        {
            get
            {
                return EnumObject.jGet(this.FNTreatment);
            }
        }
		public string InActiveDateInString 
		{
			get
			{
				return InActiveDate.ToString("dd MMM yyyy") ; 
			}
		}
		#endregion 

		#region Functions 
        public static List<FNMachineProcess> Gets(int nFNMachineID, long nUserID)
		{
            return FNMachineProcess.Service.Gets(nFNMachineID, nUserID);
		}
		public static List<FNMachineProcess> Gets(string sSQL, long nUserID)
		{
			return FNMachineProcess.Service.Gets(sSQL,nUserID);
		}
		public FNMachineProcess Get(int id, long nUserID)
		{
			return FNMachineProcess.Service.Get(id,nUserID);
		}
        public FNMachine Save(FNMachine oFNMachine, long nUserID)
		{
            return FNMachineProcess.Service.Save(oFNMachine, nUserID);
		}
		public  string  Delete(int id, long nUserID)
		{
			return FNMachineProcess.Service.Delete(id,nUserID);
		}
        public FNMachineProcess ChangeActivety(long nUserID)
        {
            return FNMachineProcess.Service.ChangeActivety(this, nUserID);
        }
		#endregion

		#region ServiceFactory
		internal static IFNMachineProcessService Service
		{
			get { return (IFNMachineProcessService)Services.Factory.CreateService(typeof(IFNMachineProcessService)); }
		}
		#endregion

    }
	#endregion

	#region IFNMachineProcess interface
	public interface IFNMachineProcessService 
	{
		FNMachineProcess Get(int id, Int64 nUserID);
        List<FNMachineProcess> Gets(int nFNMachineID, Int64 nUserID);
		List<FNMachineProcess> Gets( string sSQL, Int64 nUserID);
		string Delete(int id, Int64 nUserID);
        FNMachine Save(FNMachine oFNMachine, Int64 nUserID);
        FNMachineProcess ChangeActivety(FNMachineProcess oFNMachine, Int64 nUserID);
	}
	#endregion
}
