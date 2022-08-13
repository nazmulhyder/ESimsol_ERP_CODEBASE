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
	#region KnitDyeingBatch  
	public class KnitDyeingBatch : BusinessObject
	{	
		public KnitDyeingBatch()
		{
			KnitDyeingBatchID = 0; 
			BUID = 0; 
			KnitDyingProgramID = 0; 
			BatchIssueDate = DateTime.Now; 
			BatchNo = "";
			RefObjectID = 0; 
			ColorID = 0; 
			FabricID = 0; 
			WashTypeID = 0; 
			FinishedGSMID = 0; 
			MachineID = 0; 
			LoadTime = DateTime.Now; 
			UnloadTime = DateTime.Now; 
			MUnitID = 0; 
			MUName = ""; 
			TotalGrayQty = 0; 
			TotalFinishQty = 0; 
			ProcessLoss = 0;
            KnitDyeingBatchDetails = new List<KnitDyeingBatchDetail>();
            KnitDyeingBatchYarns = new List<KnitDyeingBatchYarn>();
            KnitDyeingBatchGrayChallans = new List<KnitDyeingBatchGrayChallan>();
			ApprovedBy = 0; 
			ApprovedByName = ""; 
			Remarks = "";
            BuyerName = "";
            RefObjectNo = "";
			ErrorMessage = "";
		}

		#region Property
		public int KnitDyeingBatchID { get; set; }
		public int BUID { get; set; }
		public int KnitDyingProgramID { get; set; }
		public DateTime BatchIssueDate { get; set; }
		public string BatchNo { get; set; }
        public int RefObjectID { get; set; }
		public int ColorID { get; set; }
		public int FabricID { get; set; }
		public int WashTypeID { get; set; }
		public int FinishedGSMID { get; set; }
		public int MachineID { get; set; }
		public DateTime LoadTime { get; set; }
		public DateTime UnloadTime { get; set; }
		public int MUnitID { get; set; }
        public int BuyerID { get; set; }
        public string BuyerName { get; set; }
        public double OrderQty { get; set; }
		public string MUName { get; set; }
		public double TotalGrayQty { get; set; }
        public string OrderRecapNo { get; set; }
		public double TotalFinishQty { get; set; }
		public double ProcessLoss { get; set; }
		public int ApprovedBy { get; set; }
		public string ApprovedByName { get; set; }
		public string Remarks { get; set; }
        public string RefObjectNo { get; set; }
        public List<KnitDyeingBatchDetail> KnitDyeingBatchDetails { get; set; }
        public List<KnitDyeingBatchYarn> KnitDyeingBatchYarns { get; set; }
        public List<KnitDyeingBatchGrayChallan> KnitDyeingBatchGrayChallans { get; set; }
		public string ErrorMessage { get; set; }
		#endregion 

		#region Derived Property
        public int TechnicalSheetID { get; set; }
        public string StyleNo { get; set; }
        public string ColorName { get; set; }
        public string FinishedGSMName { get;set; }
        public string WashName { get; set; }
        public string MachineName { get; set; }
        public string FabricName { get; set; }
		public string BatchIssueDateSt 
		{
			get
			{
                return this.BatchIssueDate.ToString("dd MMM yyyy");
			}
		}
		public string LoadTimeSt 
		{
			get
			{
				return LoadTime.ToString("dd MMM yyyy") ; 
			}
		}
		public string UnloadTimeSt 
		{
			get
			{
				return UnloadTime.ToString("dd MMM yyyy") ; 
			}
		}
        public string PanelHeader
        {
            get
            {
                return "Buyer Name:" + this.BuyerName + " | Batch No:" + this.BatchNo + " | Style:" + this.StyleNo + " | Order No:" + this.OrderRecapNo + " | Color:" + this.ColorName;
            }
        }
		#endregion 

		#region Functions 
		public static List<KnitDyeingBatch> Gets(long nUserID)
		{
			return KnitDyeingBatch.Service.Gets(nUserID);
		}
		public static List<KnitDyeingBatch> Gets(string sSQL, long nUserID)
		{
			return KnitDyeingBatch.Service.Gets(sSQL,nUserID);
		}
		public KnitDyeingBatch Get(int id, long nUserID)
		{
			return KnitDyeingBatch.Service.Get(id,nUserID);
		}
		public KnitDyeingBatch Save(long nUserID)
		{
			return KnitDyeingBatch.Service.Save(this,nUserID);
		}
        public KnitDyeingBatch Approved(long nUserID)
        {
            return KnitDyeingBatch.Service.Approved(this, nUserID);
        }
        public KnitDyeingBatch SaveAll(long nUserID)
        {
            return KnitDyeingBatch.Service.SaveAll(this, nUserID);
        }
        public KnitDyeingBatch SaveKnitDyeingBatchGrayChallans(long nUserID)
        {
            return KnitDyeingBatch.Service.SaveKnitDyeingBatchGrayChallans(this, nUserID);
        }
		public  string  Delete(int id, long nUserID)
		{
			return KnitDyeingBatch.Service.Delete(id,nUserID);
		}
		#endregion

		#region ServiceFactory
		internal static IKnitDyeingBatchService Service
		{
			get { return (IKnitDyeingBatchService)Services.Factory.CreateService(typeof(IKnitDyeingBatchService)); }
		}
		#endregion
	}
	#endregion

	#region IKnitDyeingBatch interface
	public interface IKnitDyeingBatchService 
	{
		KnitDyeingBatch Get(int id, Int64 nUserID); 
		List<KnitDyeingBatch> Gets(Int64 nUserID);
		List<KnitDyeingBatch> Gets( string sSQL, Int64 nUserID);
		string Delete(int id, Int64 nUserID);
 		KnitDyeingBatch Save(KnitDyeingBatch oKnitDyeingBatch, Int64 nUserID);
        KnitDyeingBatch Approved(KnitDyeingBatch oKnitDyeingBatch, Int64 nUserID);
 		KnitDyeingBatch SaveKnitDyeingBatchGrayChallans(KnitDyeingBatch oKnitDyeingBatch, Int64 nUserID);
        KnitDyeingBatch SaveAll(KnitDyeingBatch oKnitDyeingBatch, Int64 nUserID);
	}
	#endregion
}
