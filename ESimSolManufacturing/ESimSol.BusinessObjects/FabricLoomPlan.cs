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
    #region FabricLoomPlan
    public class FabricLoomPlan : BusinessObject
    {
        public FabricLoomPlan()
        {
            FLPID = 0;
            FSCDID = 0;
            FBID = 0;
            FMID = 0;
            NoOfColorWF = 0;
            FinishDate = DateTime.Now;
            Sequence = 0;
            Note = "";
            ErrorMessage = "";
            FabricLoomPlans = new List<FabricLoomPlan>();
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
            GreigeDemand = 0;
            TFLength = 0;
            TotalEnds = 0;
            Qty_FWP = 0;
            MachineCode = "";
            WMCode = "";
            FabricMachines = new List<FabricMachine>();
            FabricPrograme = EnumFabricPrograme.None;
            PlanStatus = EnumFabricPlanStatus.Initialize;
            Priority = 0;
            ShiftID = 0;
            FabricLoomPlanDetails = new List<FabricLoomPlanDetail>();
        }

        #region Property
        public int FLPID { get; set; }
        public int FSCDID { get; set; }
        public int FBID { get; set; }
        public int FMID { get; set; }
        public int NoOfColorWF { get; set; }
        public double Qty { get; set; }
        public double Qty_FWP { get; set; }
        public DateTime FinishDate { get; set; }
        public int ShiftID { get; set; }
        public int Sequence { get; set; }
        public EnumFabricPrograme FabricPrograme { get; set; }
        public EnumFabricBatchState Status { get; set; }
        public EnumFabricPlanStatus PlanStatus { get; set; }
        public string Note { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public bool IsIncreaseTime { get; set; }
        public string WMCode { get; set; }
        public int Priority { get; set; }
        public string ErrorMessage { get; set; }
        
        #endregion

        #region Derived Property
        public int BUID { get; set; }
        public int BuyerID { get; set; }
        public string BuyerName { get; set; }
        public int FabricWeave { get; set; }
        public string FinishType { get; set; }
        public string HLReference { get; set; }
        public double Dent { get; set; }
        public double REED { get; set; }
        public string StyleNo { get; set; }
        public string BeamNo { get; set; }
        public double BeamQty { get; set; }
        public string ExeNo { get; set; }
        public string Weave { get; set; }
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
        public string Const_CO { get; set; }
        public string Shift_CO { get; set; }
        public double ReedCount_CO { get; set; }
        public string Dent_CO { get; set; }
        public double ExeQty { get; set; }
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
        public double QtyInM
        {
            get
            {
                return Global.GetMeter(this.Qty, 2);
            }
        }
        public double ExeQtyInM
        {
            get
            {
                return Global.GetMeter(this.ExeQty, 2);
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
                   sStatus= EnumObject.jGet(this.PlanStatus);
                }
                else
                {
                    sStatus= EnumObject.jGet(this.Status);
                }
                //if (this.ExeQty > 0 && this.PlanStatus == EnumFabricPlanStatus.Planned)
                //{
                //    return sStatus+"(Running)";
                //}
                return sStatus;
            }
        }
        public string PlanStatusSt
        {
            get
            {
               
                    return EnumObject.jGet(this.PlanStatus);
               
            }
        }
        public string FabricProgrameSt
        {
            get
            {
                return EnumObject.jGet(this.FabricPrograme);
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
        
        public List<FabricLoomPlan> FabricLoomPlans { get; set; }
        public List<FabricLoomPlanDetail> FabricLoomPlanDetails { get; set; }
        public List<FabricMachine> FabricMachines { get; set; }
        
        public string StartTimeSt
        {
            get
            {
                if (StartTime == DateTime.MinValue) return "";
                return StartTime.ToString("dd MMM yyyy HH:mm");
            }
        }
        public string StartDateSt
        {
            get
            {
                if (StartTime == DateTime.MinValue) return "";
                return StartTime.ToString("dd MMM yyyy");
            }
        }
      
        public string EndDateSt
        {
            get
            {
                if (EndTime == DateTime.MinValue) return "";
                return EndTime.ToString("dd MMM yyyy");
            }
        }
        public string EndTimeSt
        {
            get
            {
                if (EndTime == DateTime.MinValue) return "";
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
        public static List<FabricLoomPlan> Gets(long nUserID)
        {
            return FabricLoomPlan.Service.Gets(nUserID);
        }
        public static List<FabricLoomPlan> Gets(string sSQL, long nUserID)
        {
            return FabricLoomPlan.Service.Gets(sSQL, nUserID);
        }
        public FabricLoomPlan Get(int id, long nUserID)
        {
            return FabricLoomPlan.Service.Get(id, nUserID);
        }
        public FabricLoomPlan Save(long nUserID)
        {
            return FabricLoomPlan.Service.Save(this, nUserID);
        }
        public string Delete( long nUserID)
        {
            return FabricLoomPlan.Service.Delete(this, nUserID);
        }
        public FabricLoomPlan UpdatePlanStatus(long nUserID)
        {
            return FabricLoomPlan.Service.UpdatePlanStatus(this, nUserID);
        }
        public static List<FabricLoomPlan> Swap(List<FabricLoomPlan> oFabricLoomPlans, long nUserID)
        {
            return FabricLoomPlan.Service.Swap(oFabricLoomPlans, nUserID);
        }
        public static List<FabricLoomPlan> SaveMultiplePlan(List<FabricLoomPlan> oFabricLoomPlans, long nUserID)
        {
            return FabricLoomPlan.Service.SaveMultiplePlan(oFabricLoomPlans, nUserID);
        }
        #endregion

        #region ServiceFactory
        internal static IFabricLoomPlanService Service
        {
            get { return (IFabricLoomPlanService)Services.Factory.CreateService(typeof(IFabricLoomPlanService)); }
        }
        #endregion
    }
    #endregion

    #region IFabricLoomPlan interface
    public interface IFabricLoomPlanService
    {
        FabricLoomPlan Get(int id, Int64 nUserID);
        List<FabricLoomPlan> Gets(Int64 nUserID);
        List<FabricLoomPlan> Gets(string sSQL, Int64 nUserID);
        string Delete(FabricLoomPlan oFabricLoomPlan, Int64 nUserID);
        FabricLoomPlan Save(FabricLoomPlan oFabricLoomPlan, Int64 nUserID);
        FabricLoomPlan UpdatePlanStatus(FabricLoomPlan oFabricLoomPlan, Int64 nUserID);
        List<FabricLoomPlan> Swap(List<FabricLoomPlan> oFabricLoomPlan, long nUserID);
        List<FabricLoomPlan> SaveMultiplePlan(List<FabricLoomPlan> oFabricLoomPlan, long nUserID);
    }
    #endregion
}
