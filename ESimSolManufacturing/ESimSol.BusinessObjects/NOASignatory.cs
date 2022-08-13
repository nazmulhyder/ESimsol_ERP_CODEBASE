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
	#region NOASignatory  
	public class NOASignatory : BusinessObject
	{	
		public NOASignatory()
        {
            NOASignatoryLogID = 0;
            NOALogID = 0;
            NOASignatoryID = 0; 
			NOAID = 0; 
			SLNo  = 0; 
			ReviseNo = 0; 
			RequestTo = 0; 
			ApproveBy = 0; 
			ApproveDate = DateTime.Now;
            IsApprove = false; 
			ErrorMessage = "";
            ApprovalHeadID = 0;
            Note = "";
		}

        #region Property
        public int NOASignatoryLogID { get; set; }
        public int NOALogID { get; set; }
        public int NOASignatoryID { get; set; }
		public int NOAID { get; set; }
		public int SLNo  { get; set; }
		public int ReviseNo { get; set; }
		public int RequestTo { get; set; }
        public int ApprovalHeadID { get; set; }
		public int ApproveBy { get; set; }
        public string Note { get; set; }
		public DateTime ApproveDate { get; set; }
        public bool IsApprove { get; set; }
		public string ErrorMessage { get; set; }
        public string HeadName { get; set; }
		#endregion 

		#region Derived Property
        public string Name_Request { get; set; }
		public string ApproveDateSt
		{
			get
			{
                if (this.ApproveDate == DateTime.MinValue) return "";
                return ApproveDate.ToString("dd MMM yyyy") ; 
			}
		}
        public string Status
        {
            get
            {
                if (this.ApproveBy<=0) return " Wating For Approve";
                return "Approved";
            }
        }
		#endregion 

        #region Functions
        public static List<NOASignatory> Gets(int nNOAID, long nUserID)
        {
            return NOASignatory.Service.Gets(nNOAID, nUserID);
        }
        public static List<NOASignatory> GetsByLog(int nNOAID, long nUserID)
        {
            return NOASignatory.Service.GetsByLog(nNOAID, nUserID);
        }
		public static List<NOASignatory> Gets(string sSQL, long nUserID)
		{
			return NOASignatory.Service.Gets(sSQL,nUserID);
		}
		public NOASignatory Get(int id, long nUserID)
		{
			return NOASignatory.Service.Get(id,nUserID);
		}
	
        public static List<NOASignatory> SaveAll(List<NOASignatory> oNOASignatorys,string sNOASignatoryID, long nUserID)
        {
            return NOASignatory.Service.SaveAll(oNOASignatorys, sNOASignatoryID, nUserID);
        }
        public NOASignatory Save(Int64 nUserID)
        {
            return NOASignatory.Service.Save(this, nUserID);
        }
		public  string  Delete(long nUserID)
		{
			return NOASignatory.Service.Delete(this,nUserID);
		}
		#endregion

		#region ServiceFactory
		internal static INOASignatoryService Service
		{
			get { return (INOASignatoryService)Services.Factory.CreateService(typeof(INOASignatoryService)); }
		}
		#endregion
	}
	#endregion

	#region INOASignatory interface
	public interface INOASignatoryService 
	{
        NOASignatory Get(int id, Int64 nUserID);
        List<NOASignatory> Gets(int nNOAID, Int64 nUserID);
        List<NOASignatory> GetsByLog(int nNOAID, Int64 nUserID);
		List<NOASignatory> Gets( string sSQL, Int64 nUserID);
        string Delete(NOASignatory oNOASignatory, Int64 nUserID);
        NOASignatory Save(NOASignatory oNOASignatory, Int64 nUserID);
        List<NOASignatory> SaveAll(List<NOASignatory> oNOASignatorys, string sNOASignatoryID,Int64 nUserID);
	}
	#endregion
}
