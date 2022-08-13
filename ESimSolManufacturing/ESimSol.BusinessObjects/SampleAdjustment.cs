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
	#region SampleAdjustment  
	public class SampleAdjustment : BusinessObject
	{	
		public SampleAdjustment()
		{
			SampleAdjustmentID = 0; 
			AdjustmentNo = ""; 
			BUID = 0; 
			SANID = 0; 
			ContractorID = 0; 
			CurrencyID = 0; 
			IssueDate = DateTime.Now; 
			AdjustAmount = 0; 
			Remarks = ""; 
			ApprovedBy = 0; 
			ContractorName = ""; 
			CurrencyName = "";
            ApprovedByName = "";
            SampleAdjustmentDetails = new List<SampleAdjustmentDetail>();
			ErrorMessage = "";
		}

		#region Property
		public int SampleAdjustmentID { get; set; }
		public string AdjustmentNo { get; set; }
        public string SANNo { get; set; }
		public int BUID { get; set; }
		public int SANID { get; set; }
		public int ContractorID { get; set; }
		public int CurrencyID { get; set; }
		public DateTime IssueDate { get; set; }
		public double AdjustAmount { get; set; }
		public string Remarks { get; set; }
		public int ApprovedBy { get; set; }
		public string ContractorName { get; set; }
		public string CurrencyName { get; set; }
        public double SANAdjust { get; set; }
        public double AlreadyAdjust { get; set; }
        public double RemainingAdjust { get; set; }
        public double WaitingAdjust { get; set; }
        public string ApprovedByName { get; set; }
        public string Params { get; set; }
		public string ErrorMessage { get; set; }

		#endregion 

		#region Derived Property
        public List<SampleAdjustmentDetail> SampleAdjustmentDetails { get; set; }
		public string IssueDateInString 
		{
			get
			{
				return IssueDate.ToString("dd MMM yyyy") ; 
			}
		}
        public string SANAdjustInSt
        {
            get
            {
                return this.CurrencyName + " " + Global.MillionFormat(this.SANAdjust);
            }
        }
        public string AlreadyAdjustInSt
        {
            get
            {
                return this.CurrencyName + " " + Global.MillionFormat(this.AlreadyAdjust);
            }
        }
        public string RemainingAdjustInSt
        {
            get
            {
                return this.CurrencyName + " " + Global.MillionFormat(this.RemainingAdjust);
            }
        }
        public string WaitingAdjustInSt
        {
            get
            {
                return this.CurrencyName + " " + Global.MillionFormat(this.WaitingAdjust);
            }
        }
        public string AdjustAmountInSt
        {
            get
            {
                return this.CurrencyName+ " " + Global.MillionFormat(this.AdjustAmount);
            }
        }
		#endregion 

		#region Functions 
		public static List<SampleAdjustment> Process(int BUID, long nUserID)
		{
            return SampleAdjustment.Service.Process(BUID, nUserID);
		}
		public static List<SampleAdjustment> Gets(string sSQL, long nUserID)
		{
			return SampleAdjustment.Service.Gets(sSQL,nUserID);
		}
        public static List<SampleAdjustment> Gets(long nUserID)
        {
            return SampleAdjustment.Service.Gets(nUserID);
        }
		public SampleAdjustment Get(int id, long nUserID)
		{
			return SampleAdjustment.Service.Get(id,nUserID);
		}
		public SampleAdjustment Save(long nUserID)
		{
			return SampleAdjustment.Service.Save(this,nUserID);
		}
        public SampleAdjustment Approve(long nUserID)
        {
            return SampleAdjustment.Service.Approve(this, nUserID);
        }
        public SampleAdjustment UnApprove(long nUserID)
        {
            return SampleAdjustment.Service.UnApprove(this, nUserID);
        }        
		public  string  Delete(int id, long nUserID)
		{
			return SampleAdjustment.Service.Delete(id,nUserID);
		}
		#endregion

		#region ServiceFactory
		internal static ISampleAdjustmentService Service
		{
			get { return (ISampleAdjustmentService)Services.Factory.CreateService(typeof(ISampleAdjustmentService)); }
		}
		#endregion
	}
	#endregion

	#region ISampleAdjustment interface
	public interface ISampleAdjustmentService 
	{
		SampleAdjustment Get(int id, Int64 nUserID);
        List<SampleAdjustment> Process(int BUID, Int64 nUserID);
		List<SampleAdjustment> Gets( string sSQL, Int64 nUserID);
        List<SampleAdjustment> Gets(Int64 nUserID);
		string Delete(int id, Int64 nUserID);
 		SampleAdjustment Save(SampleAdjustment oSampleAdjustment, Int64 nUserID);
        SampleAdjustment Approve(SampleAdjustment oSampleAdjustment, Int64 nUserID);
        SampleAdjustment UnApprove(SampleAdjustment oSampleAdjustment, Int64 nUserID);
	}
	#endregion
}
