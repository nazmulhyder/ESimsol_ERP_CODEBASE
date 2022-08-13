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
    #region FabricSizingPlan
    public class FabricSizingPlan : BusinessObject
    {
        public FabricSizingPlan()
        {
            FabricSizingPlanID = 0;
            FSCDID = 0;
            FEOSID = 0;
            FMID = 0;
            NoOfColor = 0;
            FinishDate = DateTime.Now;
            Sequence = 0;
            Note = "";
            ErrorMessage = "";
            FabricSizingPlanDetails = new List<FabricSizingPlanDetail>();
            FabricSizingPlans = new List<FabricSizingPlan>();
            ExeNo = "";
            Weave = "";
            ContractorName = "";
            MachineName = "";
            ScheduleNo = "";
            BatchNo = "";
            Construction = "";
            Composition = "";
            Params = "";
            FabricWarpPlanID = 0;
            RequiredWarpLength = 0;
            DBUserID = 0;
            WeftColor = 0;
            WarpColor = 0;
            OrderInfo = "";
            REEDWidth = 0;
            ReedNo = "";
            WarpBeam = "";
            WarpLength = 0;
            BeamName = "";
            SzingBeam="";
            Qty = 0;
            FabricMachines = new List<FabricMachine>();
            PlanStatus = EnumFabricPlanStatus.Initialize;
            Status = EnumFabricBatchState.Initialize;
            Priority = 0;
            LFID = 0;
            FBID = 0;
            WaterQty = 0;
        }

        #region Property
        public int FabricSizingPlanID { get; set; }
        public int FSCDID { get; set; }
        public int FEOSID { get; set; }
        public int FMID { get; set; }
        public int FabricWarpPlanID { get; set; }
        public int NoOfColor { get; set; }
        public DateTime FinishDate { get; set; }
        public int Sequence { get; set; }
        public EnumFabricPlanStatus PlanStatus { get; set; }
        public EnumFabricBatchState Status { get; set; }
        public string Note { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public bool IsIncreaseTime { get; set; }
        public int LFID { get; set; }
        public int Priority { get; set; }
        public double WaterQty { get; set; }
        public string ErrorMessage { get; set; }
        #endregion

        #region Derived Property
        public int FBID { get; set; }
        public string ExeNo { get; set; }
        public string Weave { get; set; }
        public string ContractorName { get; set; }
        public string MachineName { get; set; }
        public string Construction { get; set; }
        public string Composition { get; set; }
        public double RequiredWarpLength { get; set; }
        public double Qty { get; set; }
        public double ExeQty { get; set; }
        public string ScheduleNo { get; set; }
        public string ReedNo { get; set; }
        public double REEDWidth { get; set; }
        public string BatchNo { get; set; }
        public int WarpColor { get; set; }
        public int WeftColor { get; set; }
        public int DBUserID { get; set; }
        public double WarpLength { get; set; }
        public string WarpBeam { get; set; }
        public string BeamName { get; set; }
        public string SzingBeam { get; set; }
        public double BalanceQty { get; set; }
        public string OrderInfo { get; set; }
        public EnumFabricSpeType FSpcType { get; set; }
        public EnumPlanType PlanType { get; set; }

        public string LowerUpperType
        {
            get
            {
                if ((int)FSpcType == 1 && (int)PlanType == 2) return "Lower Beam";
                else if ((int)FSpcType == 1 && (int)PlanType != 2) return "Upper Beam";
                else return "";
            }
        }
        public double ExeQtyInM
        {
            get
            {
                return Global.GetMeter(this.ExeQty, 2);
            }
        }
        public double QtyInM
        {
            get
            {
                return Global.GetMeter(this.Qty, 2);
            }
        }
        public string FinishDateInString
        {
            get
            {
                if (this.FinishDate == DateTime.MinValue) return " ";
                return FinishDate.ToString("dd MMM yyyy");
            }
        }
        public string LF
        {
            get
            {
                if (this.LFID == 1) return "Floor 1";
                else if (this.LFID == 2) return "Floor 2";
                else if (this.LFID == 3) return "Floor 3";
                else if (this.LFID == 4) return "Floor 4";
                else if (this.LFID == 5) return "Floor 5";
                else if (this.LFID == 6) return "Floor 6";
                else return "";
            }
        }
        public string PrioritySt
        {
            get
            {
                if (this.Priority == 1) return "Low";
                else if (this.Priority == 2) return "Medium";
                else if (this.Priority == 3) return "High";
                else if (this.Priority == 4) return "Urgent";
                else if (this.Priority == 5) return "Top Urgent";
                else if (this.Priority == 6) return "Loom Program";
                else return "";
            }
        }
        public string StatusSt
        {
            get
            {
                if ((int)this.Status == (int)EnumFabricBatchState.Initialize)
                {
                    return EnumObject.jGet(this.PlanStatus);
                }
                else
                {
                    return EnumObject.jGet(this.Status);
                }
            }
        }
        public string PlanStatusSt
        {
            get
            {
                return EnumObject.jGet(this.PlanStatus);
            }
        }
        public string Params { get; set; }
        
        public List<FabricSizingPlan> FabricSizingPlans { get; set; }
        public List<FabricSizingPlanDetail> FabricSizingPlanDetails { get; set; }
        public List<FabricMachine> FabricMachines { get; set; }
        
        public string StartTimeSt
        {
            get
            {
                return StartTime.ToString("dd MMM yyyy HH:mm");
            }
        }
        public string StartDateSt
        {
            get
            {
                return StartTime.ToString("dd MMM yyyy");
            }
        }
      
        public string EndDateSt
        {
            get
            {
                return EndTime.ToString("dd MMM yyyy");
            }
        }
        public string EndTimeSt
        {
            get
            {
                return EndTime.ToString("dd MMM yyyy HH:mm");
            }
        }
        public string StartTimeTS
        {
            get
            {
                return this.StartTime.ToString("HH:mm");

            }
        }
        public string EndTimeTS
        {
            get
            {
                return this.EndTime.ToString("HH:mm");

            }
        }
        #endregion

        #region Functions
        public static List<FabricSizingPlan> Gets(long nUserID)
        {
            return FabricSizingPlan.Service.Gets(nUserID);
        }
        public static List<FabricSizingPlan> Gets(string sSQL, long nUserID)
        {
            return FabricSizingPlan.Service.Gets(sSQL, nUserID);
        }
        public FabricSizingPlan Get(int id, long nUserID)
        {
            return FabricSizingPlan.Service.Get(id, nUserID);
        }
        public FabricSizingPlan Save(long nUserID)
        {
            return FabricSizingPlan.Service.Save(this, nUserID);
        }
        public string Delete( long nUserID)
        {
            return FabricSizingPlan.Service.Delete(this, nUserID);
        }
        public FabricSizingPlan UpdatePlanStatus(long nUserID)
        {
            return FabricSizingPlan.Service.UpdatePlanStatus(this, nUserID);
        }
        public FabricSizingPlan UpdateWaterQty(long nUserID)
        {
            return FabricSizingPlan.Service.UpdateWaterQty(this, nUserID);
        }
        public static List<FabricSizingPlan> Swap(List<FabricSizingPlan> oFabricSizingPlans, long nUserID)
        {
            return FabricSizingPlan.Service.Swap(oFabricSizingPlans, nUserID);
        }
        #endregion

        #region ServiceFactory
        internal static IFabricSizingPlanService Service
        {
            get { return (IFabricSizingPlanService)Services.Factory.CreateService(typeof(IFabricSizingPlanService)); }
        }
        #endregion
    }
    #endregion

    #region IFabricSizingPlan interface
    public interface IFabricSizingPlanService
    {
        FabricSizingPlan Get(int id, Int64 nUserID);
        List<FabricSizingPlan> Gets(Int64 nUserID);
        List<FabricSizingPlan> Gets(string sSQL, Int64 nUserID);
        string Delete(FabricSizingPlan oFabricSizingPlan, Int64 nUserID);
        FabricSizingPlan Save(FabricSizingPlan oFabricSizingPlan, Int64 nUserID);
        FabricSizingPlan UpdatePlanStatus(FabricSizingPlan oFabricSizingPlan, Int64 nUserID);
        FabricSizingPlan UpdateWaterQty(FabricSizingPlan oFabricSizingPlan, Int64 nUserID);
        List<FabricSizingPlan> Swap(List<FabricSizingPlan> oFabricSizingPlan, long nUserID);

    }
    #endregion
}
