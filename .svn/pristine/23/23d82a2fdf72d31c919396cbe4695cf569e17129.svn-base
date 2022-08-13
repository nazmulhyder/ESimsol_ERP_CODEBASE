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
	#region FNExecutionOrderProcess  
	public class FNExecutionOrderProcess : BusinessObject
	{	
		public FNExecutionOrderProcess()
		{
			FNExOProcessID = 0; 
			FNExOID = 0; 
			Remark = ""; 
			Sequence = 0; 
			FNTPID = 0; 
			FNTreatment = EnumFNTreatment.None; 
			FNProcess = "";
            Code = "";
			ErrorMessage = "";
		}

		#region Property
		public int FNExOProcessID { get; set; }
		public int FNExOID { get; set; }
		public string Remark { get; set; }
		public int Sequence { get; set; }
		public int FNTPID { get; set; }
        public string Code { get; set; }
        public EnumFNTreatment FNTreatment { get; set; }
		public string FNProcess { get; set; }
		public string ErrorMessage { get; set; }
		#endregion 

		#region Derived Property
        public bool IsUp { get; set; }
        public string FNTreatmentSt
        {
            get
            {
                return EnumObject.jGet(this.FNTreatment);
            }
        }
      
		#endregion 

		#region Functions 
		public static List<FNExecutionOrderProcess> Gets( int nID, long nUserID)
		{
            return FNExecutionOrderProcess.Service.Gets(nID,nUserID);
		}
		public static List<FNExecutionOrderProcess> Gets(string sSQL, long nUserID)
		{
			return FNExecutionOrderProcess.Service.Gets(sSQL,nUserID);
		}
		public FNExecutionOrderProcess Get(int id, long nUserID)
		{
			return FNExecutionOrderProcess.Service.Get(id,nUserID);
		}
        //public FNExecutionOrder Save(FNExecutionOrder oFNExecutionOrder, long nUserID)
        //{
        //    return FNExecutionOrderProcess.Service.Save(oFNExecutionOrder, nUserID);
        //}
		public  string  Delete(int id, long nUserID)
		{
			return FNExecutionOrderProcess.Service.Delete(id,nUserID);
		}
        public FNExecutionOrderProcess UpDown(FNExecutionOrderProcess oFNExecutionOrderProcess, long nUserID)
        {
            return FNExecutionOrderProcess.Service.UpDown(oFNExecutionOrderProcess, nUserID);
        }
        public static List<FNExecutionOrderProcess> RefreshSequence(List<FNExecutionOrderProcess> oFNExecutionOrderProcess, long nUserID)
        {
            return FNExecutionOrderProcess.Service.RefreshSequence(oFNExecutionOrderProcess, nUserID);
        }
		#endregion

		#region ServiceFactory
		internal static IFNExecutionOrderProcessService Service
		{
			get { return (IFNExecutionOrderProcessService)Services.Factory.CreateService(typeof(IFNExecutionOrderProcessService)); }
		}
		#endregion
	}
	#endregion

	#region IFNExecutionOrderProcess interface
	public interface IFNExecutionOrderProcessService 
	{
		FNExecutionOrderProcess Get(int id, Int64 nUserID); 
		List<FNExecutionOrderProcess> Gets(int nID, Int64 nUserID);

		List<FNExecutionOrderProcess> Gets( string sSQL, Int64 nUserID);
		string Delete(int id, Int64 nUserID);
        //FNExecutionOrder Save(FNExecutionOrder oFNExecutionOrder, Int64 nUserID);
        FNExecutionOrderProcess UpDown(FNExecutionOrderProcess oFNExecutionOrderProcess, Int64 nUserID);
        List<FNExecutionOrderProcess> RefreshSequence(List<FNExecutionOrderProcess> oFNExecutionOrderProcess, long nUserID);
	}
	#endregion
}
