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
	#region KnitDyeingPTU  
	public class KnitDyeingPTU : BusinessObject
	{	
		public KnitDyeingPTU()
		{
			KnitDyeingPTUID = 0; 
			KnitDyeingProgramDetailID = 0;
            KnitDyeingProgramID = 0;
			ColorID = 0; 
			GarmentsQty = 0; 
			GarmentsMUnitID = 0; 
			FabricTypeID = 0;
            FinishDiaID = 0;
			GSMID = 0; 
			CompositionID = 0; 
			PantoneNo = ""; 
			ShadeRecipe = ""; 
			ReqFabricQty = 0; 
			MUnitID = 0; 
			KnitYarnBookQty = 0; 
			KnitYarnIssueQty = 0; 
			KnitPipeLineQty = 0; 
			KnitProcessLossQty = 0; 
			KnitRejectQty = 0; 
			GrayFabricRcvQty = 0; 
			DyeingIssueQty = 0; 
			DyeingPipeLineQty = 0; 
			ReDyeingQty = 0; 
			DyeingGainLossQty = 0; 
			DyeingFinishQty = 0; 
			ReFinishingQty = 0; 
			FinishingGainLossQty = 0; 
			FinishingQty = 0; 
			ChallanQty = 0;
            CompositionName = "";
            RefObjectNo = "";
            PendingDyeingBatchQty = 0;
			ErrorMessage = "";
		}

		#region Property
		public int KnitDyeingPTUID { get; set; }
        public int KnitDyeingProgramDetailID { get; set; }
        public int KnitDyeingProgramID { get; set; }
		public int ColorID { get; set; } 
		public double GarmentsQty { get; set; }
		public int GarmentsMUnitID { get; set; }
        public int FinishDiaID { get; set; }
		public int FabricTypeID { get; set; }
		public int GSMID { get; set; }
		public int CompositionID { get; set; }
		public string PantoneNo { get; set; }
		public string ShadeRecipe { get; set; }
		public double ReqFabricQty { get; set; }
		public int MUnitID { get; set; }
		public double KnitYarnBookQty { get; set; }
		public double KnitYarnIssueQty { get; set; }
		public double KnitPipeLineQty { get; set; }
		public double KnitProcessLossQty { get; set; }
		public double KnitRejectQty { get; set; }
		public double GrayFabricRcvQty { get; set; }
		public double DyeingIssueQty { get; set; }
		public double DyeingPipeLineQty { get; set; }
		public double ReDyeingQty { get; set; }
		public double DyeingGainLossQty { get; set; }
		public double DyeingFinishQty { get; set; }
		public double ReFinishingQty { get; set; }
		public double FinishingGainLossQty { get; set; }
		public double FinishingQty { get; set; }
		public double ChallanQty { get; set; }
        public string ColorName { get; set; }
        public string CompositionName { get; set; }
        public double PendingDyeingBatchQty { get; set; }
        public string RefObjectNo { get; set; }
		public string ErrorMessage { get; set; }
		#endregion 

		#region Derived Property
        public EnumKnitDyeingPTURefType KnitDyeingPTUPTURefType { get; set; } 
        public string FabricTypeName { get; set; }
        public string GSMName { get; set; }
        public string FinishDiaName { get; set; }
        public string UnitName { get; set; }

        #region Reflink of ptu
        public string ReqFabricQtyInString
        {
            get
            {
                return this.KnitDyeingPTUID + "~" + (int)EnumKnitDyeingPTURefType.KnitDyeing_Program_Issue + "~" + Global.MillionFormatActualDigit(this.ReqFabricQty);
            }
        }
        public string KnitYarnBookQtySt
        {
            get
            {

                return this.KnitDyeingPTUID + "~" + (int)EnumKnitDyeingPTURefType.Yarn_Booking + "~" + Global.MillionFormatActualDigit(this.KnitYarnBookQty);
            }
        }
        public string KnitYarnIssueQtySt
        {
            get
            {

                return this.KnitDyeingPTUID + "~" + (int)EnumKnitDyeingPTURefType.Knitting_Issue + "~" + Global.MillionFormatActualDigit(this.KnitYarnIssueQty);
            }
        }
        public string GrayFabricRcvQtySt
        {
            get
            {

                return this.KnitDyeingPTUID + "~" + (int)EnumKnitDyeingPTURefType.Gray_Fabric_Received + "~" + Global.MillionFormatActualDigit(this.GrayFabricRcvQty);
            }
        }
        public string KnitProcessLossQtySt
        {
            get
            {

                return this.KnitDyeingPTUID + "~" + (int)EnumKnitDyeingPTURefType.Knitting_Process_Loss + "~" + Global.MillionFormatActualDigit(this.KnitProcessLossQty);
            }
        }
        public string DyeingIssueQtySt
        {
            get
            {
                return this.KnitDyeingPTUID + "~" + (int)EnumKnitDyeingPTURefType.Gray_Fabric_Issue_For_Dyeing + "~" + Global.MillionFormatActualDigit(this.DyeingIssueQty);
            }
        }
        #endregion

        public double ProcessLoss { get; set; }
        public string ProcessLossSt
        {
            get
            {
                return this.ProcessLoss.ToString("##0.00") + "%";
            }
        }
        public double DeliveryBalance { get; set; }
        public string DeliveryBalanceSt
        {
            get
            {
                return this.DeliveryBalance.ToString("##0.00");
            }
        }
        public double DyeingBalance { get; set; }
        public string DyeingBalanceSt
        {
            get
            {
                return this.DyeingBalance.ToString("##0.00");
            }
        }
       
     
        public string KnitPipeLineQtySt
        {
            get
            {
                return this.KnitPipeLineQty.ToString("##0.00");
            }
        }
     
        public string KnitRejectQtySt
        {
            get
            {
                return this.KnitRejectQty.ToString("##0.00");
            }
        }
      
       
        public string DyeingPipeLineQtySt
        {
            get
            {
                return this.DyeingPipeLineQty.ToString("##0.00");
            }
        }
        public string ReDyeingQtySt
        {
            get
            {
                return this.ReDyeingQty.ToString("##0.00");
            }
        }
        public string DyeingGainLossQtySt
        {
            get
            {
                return this.DyeingGainLossQty.ToString("##0.00");
            }
        }
        public string DyeingFinishQtySt
        {
            get
            {
                return this.DyeingFinishQty.ToString("##0.00");
            }
        }
        public string ReFinishingQtySt
        {
            get
            {
                return this.ReFinishingQty.ToString("##0.00");
            }
        }
        public string FinishingGainLossQtySt
        {
            get
            {
                return this.FinishingGainLossQty.ToString("##0.00");
            }
        }
        public string FinishingQtySt
        {
            get
            {
                return this.FinishingQty.ToString("##0.00");
            }
        }
        public string ChallanQtySt
        {
            get
            {
                return this.ChallanQty.ToString("##0.00");
            }
        }
        public string PendingDyeingBatchQtySt
        {
            get
            {
                return this.PendingDyeingBatchQty.ToString("##0.00");
            }
        }
        public string FullName
        {
            get
            {
                return this.CompositionName + " | " + this.FabricTypeName + " | " + this.FinishDiaName + " | " + this.GSMName + " | " + this.ReqFabricQty.ToString("#,##0.00")+ " | "+  this.PendingDyeingBatchQtySt + " | " + this.UnitName;
            }
        }
		#endregion 

		#region Functions 
		public static List<KnitDyeingPTU> Gets(long nUserID)
		{
			return KnitDyeingPTU.Service.Gets(nUserID);
		}
		public static List<KnitDyeingPTU> Gets(string sSQL, long nUserID)
		{
			return KnitDyeingPTU.Service.Gets(sSQL,nUserID);
		}
		public KnitDyeingPTU Get(int id, long nUserID)
		{
			return KnitDyeingPTU.Service.Get(id,nUserID);
		}
		public KnitDyeingPTU Save(long nUserID)
		{
			return KnitDyeingPTU.Service.Save(this,nUserID);
		}
		public  string  Delete(int id, long nUserID)
		{
			return KnitDyeingPTU.Service.Delete(id,nUserID);
		}
		#endregion

		#region ServiceFactory
		internal static IKnitDyeingPTUService Service
		{
			get { return (IKnitDyeingPTUService)Services.Factory.CreateService(typeof(IKnitDyeingPTUService)); }
		}
		#endregion
	}
	#endregion

	#region IKnitDyeingPTU interface
	public interface IKnitDyeingPTUService 
	{
		KnitDyeingPTU Get(int id, Int64 nUserID); 
		List<KnitDyeingPTU> Gets(Int64 nUserID);
		List<KnitDyeingPTU> Gets( string sSQL, Int64 nUserID);
		string Delete(int id, Int64 nUserID);
 		KnitDyeingPTU Save(KnitDyeingPTU oKnitDyeingPTU, Int64 nUserID);
	}
	#endregion
}
