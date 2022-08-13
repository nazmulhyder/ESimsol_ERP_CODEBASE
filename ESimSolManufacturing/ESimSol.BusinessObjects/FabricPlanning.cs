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
	#region FabricPlanning  
	public class FabricPlanning : BusinessObject
	{
        public FabricPlanning()
        {
            FabricPlanningID = 0;
            FabricID = 0;
            ProductID = 0;
            ProductName = "";
            Color = "";
            //FabricPlanCounts = new List<FabricPlanCount>();
            FabricPlannings = new List<FabricPlanning>();
            CellRowSpans = new List<CellRowSpan>();
            IsWarp = true;
            FSCDID = 0;
            RGB = "";
            ComboNo = 0;
            Value = 0;
            RepeatNo = 0;
            Count1 = 0;
            Count2 = 0;
            Count3 = 0;
            Count4 = 0;
            Count5 = 0;
            Count6 = 0;
            Count7 = 0;
            Count8 = 0;
            Count9 = 0;
            Count10 = 0;
            Count11 = 0;
            Count12 = 0;
            Count13 = 0;
            Count14 = 0;
            Count15 = 0;
            PantonNo = "";
            ColorNo = "";
            ErrorMessage = "";
        }

		#region Property
		public int FabricPlanningID { get; set; }
		public int FabricID { get; set; }
        public int FSCDID { get; set; }
		public int ProductID { get; set; }
        public bool IsWarp { get; set; }
        public string IsWarpSt { get { if (this.IsWarp) { return "Warp"; } else { return "Weft"; } } }
		public string Color { get; set; }
        public string RGB { get; set; }
        public int ComboNo { get; set; }
        public int Value { get; set; }
		public int RepeatNo { get; set; }
        public int SLNo { get; set; }
        public int EndsCount { get; set; }
        public int Count1 { get; set; }
        public int Count2 { get; set; }
        public int Count3 { get; set; }
        public int Count4 { get; set; }
        public int Count5 { get; set; }
        public int Count6 { get; set; }
        public int Count7 { get; set; }
        public int Count8 { get; set; }
        public int Count9 { get; set; }
        public int Count10 { get; set; }
        public int Count11 { get; set; }
        public int Count12 { get; set; }
        public int Count13 { get; set; }
        public int Count14 { get; set; }
        public int Count15 { get; set; }
        public int CountT
        {
            get
            {

                if (this.RepeatNo > 0)
                {
                    return (this.RepeatNo*this.Count1) + (this.RepeatNo*this.Count2) + (this.RepeatNo*this.Count3) + (this.RepeatNo*this.Count4) + (this.RepeatNo*this.Count5) + (this.RepeatNo*this.Count6) +(this.RepeatNo*this.Count7) + (this.RepeatNo*this.Count8) +(this.RepeatNo*this.Count9) + (this.RepeatNo*this.Count10)+ (this.RepeatNo*this.Count11) + (this.RepeatNo*this.Count12) + (this.RepeatNo*this.Count13) + (this.RepeatNo*this.Count14) + (this.RepeatNo*this.Count15);
                }
                else
                {
                    return this.Count1 + this.Count2 + this.Count3 + this.Count4 + this.Count5 + this.Count6 + this.Count7 + this.Count8 + this.Count9 + this.Count10 + this.Count11 + this.Count12 + this.Count13 + this.Count14 + this.Count15;
                }
            }
        }
        public string ProductName { get; set; }
		public string ErrorMessage { get; set; }
		#endregion 

		#region Derived Property
        public string PantonNo { get; set; }
        public string ColorNo { get; set; }
        //List<FabricPlanCount> FabricPlanCounts { get; set; }
        public List<FabricPlanning> FabricPlannings { get; set; }
        public List<CellRowSpan> CellRowSpans { get; set; }
        public List<dynamic> obje { get; set; }
		#endregion 

		#region Functions 
		public static List<FabricPlanning> Gets(long nUserID)
		{
			return FabricPlanning.Service.Gets(nUserID);
		}
		public static List<FabricPlanning> Gets(string sSQL, long nUserID)
		{
			return FabricPlanning.Service.Gets(sSQL,nUserID);
		}
		public FabricPlanning Get(int id, long nUserID)
		{
			return FabricPlanning.Service.Get(id,nUserID);
		}
		public FabricPlanning Save(long nUserID)
		{
			return FabricPlanning.Service.Save(this,nUserID);
		}
		public  string  Delete(int id, long nUserID)
		{
			return FabricPlanning.Service.Delete(id,nUserID);
		}
        public static List<FabricPlanning> MakeCombo(string sDyeingOrderDetailID, int nFabricID, int nComboNo, int nDBOperation, int nUserID)
        {
            return FabricPlanning.Service.MakeCombo(sDyeingOrderDetailID, nFabricID, nComboNo, nDBOperation, nUserID);
        }
		#endregion

		#region ServiceFactory
		internal static IFabricPlanningService Service
		{
			get { return (IFabricPlanningService)Services.Factory.CreateService(typeof(IFabricPlanningService)); }
		}
		#endregion

        
    }
	#endregion

	#region IFabricPlanning interface
	public interface IFabricPlanningService 
	{
		FabricPlanning Get(int id, Int64 nUserID); 
		List<FabricPlanning> Gets(Int64 nUserID);
		List<FabricPlanning> Gets( string sSQL, Int64 nUserID);
        List<FabricPlanning> MakeCombo(string sDyeingOrderDetailID, int nFabricID, int nComboNo, int nDBOperation, int nUserID);
		string Delete(int id, Int64 nUserID);
 		FabricPlanning Save(FabricPlanning oFabricPlanning, Int64 nUserID);
	}
	#endregion
}
