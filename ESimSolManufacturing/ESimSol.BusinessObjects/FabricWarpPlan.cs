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
    #region FabricWarpPlan
    public class FabricWarpPlan : BusinessObject
    {
        public FabricWarpPlan()
        {
            FabricWarpPlanID = 0;
            FSCDID = 0;
            FEOSID = 0;
            FMID = 0;
            NoOfColor = 0;
            FinishDate = DateTime.Now;
            Sequence = 0;
            Note = "";
            ErrorMessage = "";
            FabricWarpPlans = new List<FabricWarpPlan>();
            ExeNo = "";
            Weave = "";
            ContractorName = "";
            MachineName = "";
            ScheduleNo = "";
            BatchNo = "";
            Construction = "";
            Composition = "";
            Params = "";
            RequiredWarpLength = 0;
            DBUserID = 0;
            WeftColor = 0;
            WarpColor = 0;
            REEDWidth = 0;
            ReedNo = "";
            WarpLength = 0;
            Qty = 0;
            WarpBeam = "";
            SzingBeam="";
            GreigeDemand = 0;
            TFLength = 0;
            TotalEnds = 0;
            Qty_FWP = 0;
            MachineCode = "";
            QtySizing = 0;
            WMCode = "";
            BalanceQty = 0;
            FabricMachines = new List<FabricMachine>();
            Status = EnumFabricBatchState.Initialize;
            PlanStatus = EnumFabricPlanStatus.Initialize;
            Priority = 0;
            ExeQty = 0;
            FBID = 0;
            PlanType = EnumPlanType.General;
        }

        #region Property
        public int FabricWarpPlanID { get; set; }
        public int FSCDID { get; set; }
        public int FEOSID { get; set; }
        public int FMID { get; set; }
        public int NoOfColor { get; set; }
        public double Qty { get; set; }
        public double Qty_FWP { get; set; }
        public DateTime FinishDate { get; set; }
        public int Sequence { get; set; }
        public EnumFabricBatchState Status { get; set; }
        public EnumFabricPlanStatus PlanStatus { get; set; }
        public string Note { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public bool IsIncreaseTime { get; set; }
        public string WMCode { get; set; }
        public int Priority { get; set; }
        public EnumPlanType PlanType { get; set; }
        public EnumFabricSpeType FSpcType { get; set; }
        public string ErrorMessage { get; set; }
        
        #endregion

        #region Derived Property
        public string ExeNo { get; set; }
        public int FBID { get; set; }
        public string Weave { get; set; }
        public double ExeQty { get; set; }
        public string ContractorName { get; set; }
        public string MachineName { get; set; }
        public string MachineCode { get; set; }
        public string Construction { get; set; }
        public string Composition { get; set; }
        public string ScheduleNo { get; set; }
        public string ReedNo { get; set; }
        public double REEDWidth { get; set; }
        public string BatchNo { get; set; }
        public int WarpColor { get; set; }
        public int WeftColor { get; set; }
        public int DBUserID { get; set; }
        public double WarpLength { get; set; }
        public string WarpBeam { get; set; }
        public double BalanceQty { get; set; }
        public double QtySizing { get; set; }
        public string SzingBeam { get; set; }
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
        public string FinishDateInString
        {
            get
            {
                return FinishDate.ToString("dd MMM yyyy");
            }
        }
        string sStatus = "";
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
        public string BatchNoMCode
        {
            get
            {


                return this.WMCode + "" + this.BatchNo;
            }
        }
        public string Params { get; set; }
        
        public List<FabricWarpPlan> FabricWarpPlans { get; set; }
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
        public double RequiredWarpLength { get; set; }
        public double GreigeDemand { get; set; }
        public double TFLength { get; set; }
        public double OrderQty { get; set; }
        public double TotalEnds { get; set; }

        #endregion

        #region Functions
        public static List<FabricWarpPlan> Gets(long nUserID)
        {
            return FabricWarpPlan.Service.Gets(nUserID);
        }
        public static List<FabricWarpPlan> Gets(string sSQL, long nUserID)
        {
            return FabricWarpPlan.Service.Gets(sSQL, nUserID);
        }
        public FabricWarpPlan Get(int id, long nUserID)
        {
            return FabricWarpPlan.Service.Get(id, nUserID);
        }
        public FabricWarpPlan Save(long nUserID)
        {
            return FabricWarpPlan.Service.Save(this, nUserID);
        }
        public string Delete( long nUserID)
        {
            return FabricWarpPlan.Service.Delete(this, nUserID);
        }
        public FabricWarpPlan UpdatePlanStatus(long nUserID)
        {
            return FabricWarpPlan.Service.UpdatePlanStatus(this, nUserID);
        }
        public static List<FabricWarpPlan> Swap(List<FabricWarpPlan> oFabricWarpPlans, long nUserID)
        {
            return FabricWarpPlan.Service.Swap(oFabricWarpPlans, nUserID);
        }
        #endregion

        #region ServiceFactory
        internal static IFabricWarpPlanService Service
        {
            get { return (IFabricWarpPlanService)Services.Factory.CreateService(typeof(IFabricWarpPlanService)); }
        }
        #endregion
    }
    #endregion

    #region IFabricWarpPlan interface
    public interface IFabricWarpPlanService
    {
        FabricWarpPlan Get(int id, Int64 nUserID);
        List<FabricWarpPlan> Gets(Int64 nUserID);
        List<FabricWarpPlan> Gets(string sSQL, Int64 nUserID);
        string Delete(FabricWarpPlan oFabricWarpPlan, Int64 nUserID);
        FabricWarpPlan Save(FabricWarpPlan oFabricWarpPlan, Int64 nUserID);
        FabricWarpPlan UpdatePlanStatus(FabricWarpPlan oFabricWarpPlan, Int64 nUserID);
        List<FabricWarpPlan> Swap(List<FabricWarpPlan> oFabricWarpPlan, long nUserID);
    }
    #endregion
}
