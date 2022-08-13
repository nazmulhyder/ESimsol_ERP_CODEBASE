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
	#region RecycleProcess  
	public class RecycleProcess : BusinessObject
	{	
		public RecycleProcess()
		{
			RecycleProcessID = 0; 
			ProcessDate = DateTime.Now; 
			RefNo = ""; 
			Note = ""; 
			BUID = 0;
            RecycleProcessType = EnumRecycleProcessType.RunnerRecycle;
            ProductNature = EnumProductNature.Hanger;
            ApprovedBy = 0;
            Qty = 0;
            ApprovedByName = "";
            RecycleProcessDetails = new List<RecycleProcessDetail>();
            DamageDetails = new List<RecycleProcessDetail>();
            RecycleProcessList = new List<RecycleProcess>();
			ErrorMessage = "";
		}

		#region Property
		public int RecycleProcessID { get; set; }
		public DateTime ProcessDate { get; set; }
		public string RefNo { get; set; }
		public string Note { get; set; }
		public int BUID { get; set; }
        public EnumRecycleProcessType RecycleProcessType { get; set; }
		public EnumProductNature ProductNature { get; set; }
        public int ApprovedBy { get; set; }
        public string ApprovedByName { get; set; }
        public double Qty { get; set; }
		public string ErrorMessage { get; set; }
		#endregion 

		#region Derived Property
        public int ProductNatureInInt { get; set; }
        public List<RecycleProcessDetail> RecycleProcessDetails { get; set; }
        public List<RecycleProcessDetail> DamageDetails { get; set; }
        public List<RecycleProcess> RecycleProcessList { get; set; }

        public string RecycleProcessTypeINST
        {
            get
            {
                return EnumObject.jGet(this.RecycleProcessType);              
            }
           
        }

		public string ProcessDateInString 
		{
			get
			{
				return ProcessDate.ToString("dd MMM yyyy") ; 
			}
		}
		#endregion 

		#region Functions 
		public static List<RecycleProcess> Gets(long nUserID)
		{
			return RecycleProcess.Service.Gets(nUserID);
		}
		public static List<RecycleProcess> Gets(string sSQL, long nUserID)
		{
			return RecycleProcess.Service.Gets(sSQL,nUserID);
		}
		public RecycleProcess Get(int id, long nUserID)
		{
			return RecycleProcess.Service.Get(id,nUserID);
		}
		public RecycleProcess Save(long nUserID)
		{
			return RecycleProcess.Service.Save(this,nUserID);
		}

        public RecycleProcess Approve(long nUserID)
        {
            return RecycleProcess.Service.Approve(this, nUserID);
        }
		public  string  Delete(int id, long nUserID)
		{
			return RecycleProcess.Service.Delete(id,nUserID);
		}
		#endregion

		#region ServiceFactory
		internal static IRecycleProcessService Service
		{
			get { return (IRecycleProcessService)Services.Factory.CreateService(typeof(IRecycleProcessService)); }
		}
		#endregion
	}
	#endregion

	#region IRecycleProcess interface
	public interface IRecycleProcessService 
	{
		RecycleProcess Get(int id, Int64 nUserID); 
		List<RecycleProcess> Gets(Int64 nUserID);
		List<RecycleProcess> Gets( string sSQL, Int64 nUserID);
		string Delete(int id, Int64 nUserID);
 		RecycleProcess Save(RecycleProcess oRecycleProcess, Int64 nUserID);
        RecycleProcess Approve(RecycleProcess oRecycleProcess, Int64 nUserID);
        
	}
	#endregion
}
