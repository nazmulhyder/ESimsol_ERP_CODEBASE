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
	#region Job  
	public class Job : BusinessObject
	{	
		public Job()
		{
			JobID = 0;
            JobNo = "";
			IssueDate = DateTime.Now; 
			TechnicalSheetID = 0;
            BUID = 0;
			Remarks = ""; 
			StyleNo = "";
            MerchandiserID = 0;
			MerchandiserName = "";
            BusinessSessionID = 0;
			SessionName = "";
            BuyerID = 0;
			BuyerName = "";
            IsTAPExist = false;
            IsTAPApproved = false;
            ApprovedBy = 0;
            ApprovedByName = "";
			ErrorMessage = "";
            JobDetails = new List<JobDetail>();
		}

		#region Property
		public int JobID { get; set; }
		public DateTime IssueDate { get; set; }
		public int TechnicalSheetID { get; set; }
        public int BUID { get; set; }
		public string Remarks { get; set; }
        public string JobNo { get; set; }
        public int ApprovedBy { get; set; }
        public string ApprovedByName { get; set; }
        public bool IsTAPExist { get; set; }
        public bool IsTAPApproved { get; set; }
		public string ErrorMessage { get; set; }
		#endregion 

		#region Derived Property

        public string StyleNo { get; set; }
        public int MerchandiserID { get; set; }
        public string MerchandiserName { get; set; }
        public int BusinessSessionID { get; set; }
        public string SessionName { get; set; }
        public int BuyerID { get; set; }
        public string BuyerName { get; set; }
        public string TampleteName { get; set; }
		public string IssueDateInString 
		{
			get
			{
				return IssueDate.ToString("dd MMM yyyy") ; 
			}
		}

        public string IsTAPExistInString 
		{
			get
			{
                if (IsTAPExist)
                    return "Yes";
                else
                    return "No";
                
			}
		}
        public string IsTAPApprovedInString
        {
            get
            {
                if (IsTAPApproved)
                    return "Yes";
                else
                    return "No";

            }
        }
        public List<JobDetail> JobDetails { get; set; }
        public List<TAPDetail> TAPDetails { get; set; }
		#endregion 

		#region Functions 
		public static List<Job> Gets(int buid, long nUserID)
		{
            return Job.Service.Gets(buid, nUserID);
		}
		public static List<Job> Gets(string sSQL, long nUserID)
		{
			return Job.Service.Gets(sSQL,nUserID);
		}
		public Job Get(int id, long nUserID)
		{
			return Job.Service.Get(id,nUserID);
		}
		public Job Save(long nUserID)
		{
			return Job.Service.Save(this,nUserID);
		}
        public Job Approve(long nUserID)
        {
            return Job.Service.Approve(this, nUserID);
        }
        public Job UndoApprove(long nUserID)
        {
            return Job.Service.UndoApprove(this, nUserID);
        }
		public  string  Delete(int id, long nUserID)
		{
			return Job.Service.Delete(id,nUserID);
		}
		#endregion

		#region ServiceFactory
		internal static IJobService Service
		{
			get { return (IJobService)Services.Factory.CreateService(typeof(IJobService)); }
		}
		#endregion
	}
	#endregion

	#region IJob interface
	public interface IJobService 
	{
		Job Get(int id, Int64 nUserID); 
		List<Job> Gets(int buid, Int64 nUserID);
		List<Job> Gets( string sSQL, Int64 nUserID);
		string Delete(int id, Int64 nUserID);
 		Job Save(Job oJob, Int64 nUserID);
        Job Approve(Job oJob, Int64 nUserID);
        Job UndoApprove(Job oJob, Int64 nUserID);
        
        
	}
	#endregion
}
