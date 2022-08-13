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
	#region LotMixing  
	public class LotMixing : BusinessObject
	{	
		public LotMixing()
		{
			LotMixingID = 0;
            SLNo = "";
            SLNoFull = ""; 
			BUID = 0; 
			CreateDate = DateTime.Now; 
			ApproveByID = 0;
            CompleteByID = 0;
			ApproveDate = DateTime.Now; 
			Remarks = ""; 
			Percentage  = 0;
            WorkingUnitID = 0;
            ApproveByName = "";
            PrepareByName = "";
            CompleteByName = "";
            BUName = "";
			ErrorMessage = "";
            LotMixingDetails = new List<LotMixingDetail>();
            LotMixingDetails_In = new List<LotMixingDetail>();
            LotMixingDetails_Out = new List<LotMixingDetail>();
		}

		#region Property
        public int LotMixingID { get; set; }
        public int WorkingUnitID { get; set; }
		public string SLNo { get; set; }
		public int BUID { get; set; }
		public DateTime CreateDate { get; set; }
		public int ApproveByID { get; set; }
        public int CompleteByID { get; set; }
		public DateTime ApproveDate { get; set; }
		public string Remarks { get; set; }
        public double Percentage { get; set; }
        public string BUName { get; set; }
        public string ApproveByName { get; set; }
        public string PrepareByName { get; set; }
        public string CompleteByName { get; set; }
        public string SLNoFull { get; set; }
        public string Params { get; set; }
		public string ErrorMessage { get; set; }
		#endregion 

		#region Derived Property
		public string CreateDateInString 
		{
			get
			{
				return CreateDate.ToString("dd MMM yyyy") ; 
			}
		}
		public string ApproveDateInString 
		{
			get
			{
				return ApproveDate.ToString("dd MMM yyyy") ; 
			}
		}
        public List<LotMixingDetail> LotMixingDetails { get; set; }
        public List<LotMixingDetail> LotMixingDetails_In { get; set; }
        public List<LotMixingDetail> LotMixingDetails_Out { get; set; }
		#endregion 

		#region Functions 
		public static List<LotMixing> Gets(long nUserID)
		{
			return LotMixing.Service.Gets(nUserID);
		}
		public static List<LotMixing> Gets(string sSQL, long nUserID)
		{
			return LotMixing.Service.Gets(sSQL,nUserID);
		}
		public LotMixing Get(int id, long nUserID)
		{
			return LotMixing.Service.Get(id,nUserID);
		}
        public LotMixing Save(long nUserID)
        {
            return LotMixing.Service.Save(this, nUserID);
        }
        public LotMixing Approve(long nUserID)
        {
            return LotMixing.Service.Approve(this, nUserID);
        }
        public LotMixing UndoApprove(long nUserID)
        {
            return LotMixing.Service.UndoApprove(this, nUserID);
        }
        public LotMixing Complete(long nUserID)
        {
            return LotMixing.Service.Complete(this, nUserID);
        }
		public  string  Delete(int id, long nUserID)
		{
			return LotMixing.Service.Delete(id,nUserID);
		}
		#endregion

		#region ServiceFactory
		internal static ILotMixingService Service
		{
			get { return (ILotMixingService)Services.Factory.CreateService(typeof(ILotMixingService)); }
		}
		#endregion

        public EnumTwistingStatus Status { get; set; }
    }
	#endregion

	#region ILotMixing interface
	public interface ILotMixingService 
	{
		LotMixing Get(int id, Int64 nUserID); 
		List<LotMixing> Gets(Int64 nUserID);
		List<LotMixing> Gets( string sSQL, Int64 nUserID);
		string Delete(int id, Int64 nUserID);
        LotMixing Save(LotMixing oLotMixing, Int64 nUserID);
        LotMixing Approve(LotMixing oLotMixing, Int64 nUserID);
        LotMixing UndoApprove(LotMixing oLotMixing, Int64 nUserID);
        LotMixing Complete(LotMixing oLotMixing, Int64 nUserID);
	}
	#endregion
}
