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
	#region FabricPlan  
	public class FabricPlan : BusinessObject
	{
        public FabricPlan()
        {
            FabricPlanID = 0;
            FabricID = 0;
            ProductID = 0;
            ProductName = "";
            Color = "";
            FabricPlans = new List<FabricPlan>();
            CellRowSpans = new List<CellRowSpan>();
            WarpWeftType = EnumWarpWeft.Warp;
            RGB = "";
            TwistedGroup = 0;
            Value = 0;
            PantonNo = "";
            ColorNo = "";
            FabricPlanDetails = new List<FabricPlanDetail>();
            FabricPlanOrderID = 0;
            Qty = 0;
            ErrorMessage = "";
        }

		#region Property
		public int FabricPlanID { get; set; }
        public int SLNo { get; set; }
        public int ProductID { get; set; }
        public int LabdipDetailID { get; set; }
        public EnumWarpWeft WarpWeftType { get; set; }
        public string WarpWeftTypeSt { get { return this.WarpWeftType.ToString(); } }
		public string Color { get; set; }
        public string RGB { get; set; }
        public int TwistedGroup { get; set; }
        public int Value { get; set; }
        public int EndsCount { get; set; }
        public string ProductName { get; set; }
        public int FabricPlanOrderID { get; set; }
        public int RefID { get; set; }
        public EnumFabricPlanRefType RefType { get; set; }
        public int FabricID { get; set; }
        public double Qty { get; set; }/// for calculation
        public string ErrorMessage { get; set; }
		#endregion 

		#region Derived Property
        public int FabricPlanOrderIDTo { get; set; }
        public string PantonNo { get; set; }
        public string ColorNo { get; set; }
        public List<FabricPlan> FabricPlans { get; set; }
        public List<FabricPlanDetail> FabricPlanDetails { get; set; }
        
        public List<CellRowSpan> CellRowSpans { get; set; }
       
		#endregion 

		#region Functions 
		public static List<FabricPlan> Gets(long nUserID)
		{
			return FabricPlan.Service.Gets(nUserID);
		}
		public static List<FabricPlan> Gets(string sSQL, long nUserID)
		{
			return FabricPlan.Service.Gets(sSQL,nUserID);
		}
		public FabricPlan Get(int id, long nUserID)
		{
			return FabricPlan.Service.Get(id,nUserID);
		}
		public FabricPlan Save(long nUserID)
		{
			return FabricPlan.Service.Save(this,nUserID);
		}
        public string Delete(List<FabricPlan> oFabricPlans, long nUserID)
		{
            return FabricPlan.Service.Delete(oFabricPlans, nUserID);
		}
        public static List<FabricPlan> MakeCombo(string sFabricPlanID, int nFabricPlanOrderID, int nComboNo, int nDBOperation, int nUserID)
        {
            return FabricPlan.Service.MakeCombo(sFabricPlanID, nFabricPlanOrderID, nComboNo, nDBOperation, nUserID);
        }
        public static List<FabricPlan> SaveSequence(List<FabricPlan> oFabricPlans, long nUserID)
        {
            return FabricPlan.Service.SaveSequence(oFabricPlans, nUserID);
        }
        public FabricPlan UpdateLDDetailID(long nUserID)
        {
            return FabricPlan.Service.UpdateLDDetailID(this, nUserID);
        }
        public string UpdateYarn(List<FabricPlan> oFabricPlans, long nUserID)
        {
            return FabricPlan.Service.UpdateYarn(oFabricPlans, nUserID);
        }
        //public List<FabricPlan> CopyFabricPlans(long nUserID)
        //{
        //    return FabricPlan.Service.CopyFabricPlans(this, nUserID);
        //}
		#endregion

		#region ServiceFactory
		internal static IFabricPlanService Service
		{
			get { return (IFabricPlanService)Services.Factory.CreateService(typeof(IFabricPlanService)); }
		}
		#endregion

        
    }
	#endregion

	#region IFabricPlan interface
	public interface IFabricPlanService 
	{
		FabricPlan Get(int id, Int64 nUserID); 
		List<FabricPlan> Gets(Int64 nUserID);
		List<FabricPlan> Gets( string sSQL, Int64 nUserID);
        List<FabricPlan> SaveSequence(List<FabricPlan> oFabricPlans, Int64 nUserID);
        List<FabricPlan> MakeCombo(string sFabricPlanID, int nFabricPlanOrderID, int nComboNo, int nDBOperation, int nUserID);
        string Delete(List<FabricPlan> oFabricPlans, Int64 nUserID);
 		FabricPlan Save(FabricPlan oFabricPlan, Int64 nUserID);
        FabricPlan UpdateLDDetailID(FabricPlan oFabricPlan, Int64 nUserID);
        string UpdateYarn(List<FabricPlan> oFabricPlans, Int64 nUserID);
        //List<FabricPlan> CopyFabricPlans(FabricPlan oFabricPlan, Int64 nUserID);
	}
	#endregion
}
