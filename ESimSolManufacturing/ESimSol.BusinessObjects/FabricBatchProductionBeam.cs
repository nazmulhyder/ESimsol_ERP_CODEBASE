using System;
using System.IO;
using ESimSol.BusinessObjects;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Collections.Generic;
using ICS.Core;
using ICS.Core.Framework;
using ICS.Core.Utility;

namespace ESimSol.BusinessObjects
{
    public class FabricBatchProductionBeam
    {
        public FabricBatchProductionBeam()
        {
            FBPBeamID = 0;
            FBPID = 0;
            FBPDetailID = 0;
            BeamID = 0;
            Qty = 0;
            QtyM = 0;
            FBID = 0;
            WeavingProcessType = EnumWeavingProcess.Warping;
            BeamName = "";
            BeamCode = "";
            IsFinish = false;
            BuyerID = 0;
            ErrorMessage = "";

            FabricSalesContractDetailID = 0;
            FEONo = "";
            IsInHouse = true;
            OrderType = EnumOrderType.None;
            Construction = "";
            BeamNo = "";
            BuyerName = "";
            TotalEnds = 0;
            Weave = "";
            ReedCount = 0;
            MachineStatus = EnumMachineStatus.Free;
            MachineName = "";
            MachineCode = "";
            Status = 0;

            FBStatus = EnumFabricBatchState.Initialize;
            StartTime = DateTime.MinValue;
            EndTime = new DateTime(1900, 01, 01, 1, 1, 1);
            FbpIdForWeaving = 0;
            BatchNo = "";
            MachineWiseQty = 0;
            FEOQty = 0;
            NoOfColor = 0;
            IsYarnDyed = false;
            ReedCount = 0;
            Dent = "";
            FabricBatchProductionBeams = new List<FabricBatchProductionBeam>();
            ChildMachineTypeID = 0;
            ChildMachineTypeName = "";
            FBPMachineID = 0;
            FBPMachineName = "";
            IsDirect = false;
            ParentMachineTypeName = "";
            IsDrawing = (int)EnumFabricBatchState.Initialize;
        }

        #region Properties

        public int FBPBeamID { get; set; }
        public int FBPID { get; set; }
        public int FBPDetailID { get; set; }
        public int BeamID { get; set; }
        public double Qty { get; set; }
        public double QtyM { get; set; }
        public bool IsFinish { get; set; }

        public int IsDrawing { get; set; }
        public int BuyerID { get; set; }
        public int FBID { get; set; }
        public EnumWeavingProcess WeavingProcessType { get; set; }
        public string BeamName { get; set; }
        public string BeamCode { get; set; }
        public string ErrorMessage { get; set; }
        public string BatchNo { get; set; }
        public string Params { get; set; }
        #endregion

        #region Derive Properties
        public int FEOID { get; set; }
        public int FEOSID { get; set; }
        public EnumFabricSpeType FSpcType { get; set; }
        public EnumPlanType PlanType { get; set; }
        public bool IsDirect { get; set; }
        public int ChildMachineTypeID { get; set; }
        public string ChildMachineTypeName { get; set; }
        public string ParentMachineTypeName { get; set; }
        public int FBPMachineID { get; set; }
        public string FBPMachineName { get; set; }
        public double FEOQty { get; set; }
        public double FBPQty { get; set; }
        public int FabricSalesContractDetailID { get; set; }
        public string FEONo { get; set; }
        public bool IsInHouse { get; set; }
        public EnumOrderType OrderType { get; set; }
        public string Construction { get; set; }
        public string BeamNo { get; set; }
        public string BuyerName { get; set; }
        public int TotalEnds { get; set; }
        public string Weave { get; set; }
        public int ReedCount { get; set; }
        public int FbpIdForWeaving { get; set; }
        public EnumMachineStatus MachineStatus { get; set; }
        public string MachineName { get; set; }
        public string MachineCode { get; set; }
        public int Status { get; set; }
        public double MachineWiseQty { get; set; }
        public EnumFabricBatchState FBStatus { get; set; }
        public DateTime StartTime { get;set;}
        public DateTime EndTime { get; set; }
        public double BatchQty { get; set; }
        public int NoOfColor { get; set; }
        public bool IsYarnDyed { get; set; }
        public string WarpColor { get; set; }
        public string WeftColor { get; set; }
        public double LoomReedCount { get; set; }
        public string LoomDent { get; set; }
        public List<FabricBatchProductionBeam> FabricBatchProductionBeams { get; set; }
        public string LowerUpperType
        {
            get
            {
                if ((int)FSpcType == 1 && (int)PlanType == 2) return "Lower Beam";
                else if ((int)FSpcType == 1 && (int)PlanType != 2) return "Upper Beam";
                else return "";
            }
        }
        public string FBStatusSt
        {
            get
            {
                return FabricBatchStateObj.GetEnumFabricBatchStateObjs(this.FBStatus);
                //return "Please Cheack BO";
            }
        }
        public string MachineStatusSt
        {
            get
            {
                return this.MachineStatus.ToString();
            }
        }

        public double QtyInMtr
        {
            get
            {
                return Global.GetMeter(this.Qty, 2);
            }
        }

        public double FEOQtyInMtr
        {
            get
            {
                return Global.GetMeter(this.FEOQty, 2);
            }
        }

        public double FBPQtyInMtr
        {
            get
            {
                return Global.GetMeter(this.FBPQty, 2);
            }
        }
        
        public double MachineWiseQtyInMtr
        {
            get
            {
                return Global.GetMeter(this.MachineWiseQty, 2);
            }
        }

        public double BatchQtyInMtr
        {
            get
            {
                return Global.GetMeter(this.BatchQty, 2);
            }
        }
        public string IsDrawingSt
        {
            get
            {
                if (IsDrawing == (int)EnumFabricBatchState.DrawingIn) return "Drawing";
                else if (IsDrawing == (int)EnumFabricBatchState.LeasingIn) return "Leasing";
                return "";
            }
            set{}
        }

        public double YetWeaveQty
        {
            get
            {
                return this.Qty - this.BatchQty;
            }
        }

        public double YetWeaveQtyInMtr
        {
            get
            {
                return Global.GetMeter(this.YetWeaveQty, 2);
            }
        }

        public int RPM { get; set; }
        public double RCount { get; set; }
        public string Dent { get; set; }

        public string WeavingProcessTypeStr
        {
            get { return this.WeavingProcessType.ToString(); }
        }

        /// <summary>
        /// StatusRBFW: Ready For loom, Loom Running
        /// 01. Ready For Loom : If the Beam finish from Sizing but not engaged or insert for Loom, is Ready for Loom.
        ///                      In that case Weaving Process must be 1 (Sizing) and Beam is Finish true and This beam 
        ///                      is not inserted at loom. [Database return maintaining above logic].
        /// 02. Running Loom : If the Finish Beam for Sizing insert to a loom machine, is running loom. 
        ///                     In that case weaving process must be 3 (loom).
        /// </summary>
        public string StatusRBFW// Ready Beam For Weaving
        {
            get
            {
                //if (this.FbpIdForWeaving > 0 && this.EndTime.ToString("dd MMM yyyy") == new DateTime(0001, 01, 01).ToString("dd MMM yyyy"))
                //{
                //    return "Loom Running";
                //}
                //else if (this.FbpIdForWeaving > 0 && this.EndTime != new DateTime(0001, 01, 01, 1, 1, 1))
                //{
                //    return "-";
                //}
                //return "Ready For Loom";
                if (this.WeavingProcessType == EnumWeavingProcess.Sizing) { return "Ready For Loom"; }
                else if (this.WeavingProcessType == EnumWeavingProcess.Loom && this.EndTime == DateTime.MinValue) { return "Loom Running"; }
                else if (this.WeavingProcessType == EnumWeavingProcess.Loom && this.EndTime != DateTime.MinValue) { return "Beam Hold"; }
                else return "-";
            }
        }
        public string MachineNameSt
        {
            get
            {
                if (this.WeavingProcessType == EnumWeavingProcess.Loom && this.MachineStatus == EnumMachineStatus.Running)
                {
                    return this.MachineName;
                }
                return "-";
            }
        }
        public string IsFinishSt
        {
            get
            {
                return (this.IsFinish ? "Finished" : "-");
            }
        }
        public string OrderNo
        {
            get
            {
                //string sPrifix = "";
                //if (this.FabricSalesContractDetailID > 0)
                //{
                //    if (this.IsInHouse) { sPrifix = "EXE"; } else sPrifix = "SCW";

                //    if (this.OrderType == EnumOrderType.Bulk) { sPrifix = sPrifix + "-BLK-"; }
                //    else if (this.OrderType == EnumOrderType.AkijExtra) { sPrifix = sPrifix + "-EXT-"; }
                //    else if (this.OrderType == EnumOrderType.Development) { sPrifix = sPrifix + "-DEV-"; }
                //    else if (this.OrderType == EnumOrderType.SMS) { sPrifix = sPrifix + "-SMS-"; }
                //    else { sPrifix = sPrifix + "-"; }

                   return this.FEONo;

                //}
                //else return "";
                //return "Please Check BO";

            }
        }
        public string MachineCodeWithName
        {
            get
            {
                if (!string.IsNullOrEmpty(this.MachineCode))
                {
                    return this.MachineCode + " [" + this.MachineName + "] ";
                }
                else
                {
                    return this.MachineName;
                }
            }
        }

        public string ReedCountWithDent
        {
            get
            {

                if (this.RCount > 0 || !string.IsNullOrEmpty(this.Dent))
                {
                    return this.RCount.ToString().Split('.')[0] + "/" + this.Dent.ToString().Split('.')[0];
                }
                else
                {
                    return "";
                }

            }
        }
       
        public string StartTimeStr
        {
            get
            {
                return  (this.StartTime != DateTime.MinValue)?this.StartTime.ToString("dd MMM yyyy hh:mm"):"";
            }
        }
     

        public string EndTimeStr
        {
            get
            {
                return (this.EndTime != new DateTime(1900, 01, 01, 1, 1, 1) && this.EndTime != DateTime.MinValue) ? this.EndTime.ToString("dd MMM yyyy hh:mm") : "";
            }
        }
        public string SearchByBatchNoWithReadCount { get { return this.BatchNo + " " + this.ReedCountWithDent + " " + this.MachineCode; } }

   
        #endregion

        #region Functions
        public static List<FabricBatchProductionBeam> Gets(long nUserID)
        {
            return FabricBatchProductionBeam.Service.Gets(nUserID);
        }
        public static List<FabricBatchProductionBeam> GetsFinishedBeams(long nUserID)
        {
            return FabricBatchProductionBeam.Service.GetsFinishedBeams(nUserID);
        }
        public static List<FabricBatchProductionBeam> GetsByFabricBatchProduction(int nFBPID, long nUserID)
        {
            return FabricBatchProductionBeam.Service.GetsByFabricBatchProduction(nFBPID, nUserID);
        }
        public static List<FabricBatchProductionBeam> Gets(string sSQL, long nUserID)
        {
            return FabricBatchProductionBeam.Service.Gets(sSQL, nUserID);
        }
        public FabricBatchProductionBeam Save(long nUserID)
        {
            return FabricBatchProductionBeam.Service.Save(this, nUserID);
        }
        public FabricBatchProductionBeam Finish(long nUserID)
        {
            return FabricBatchProductionBeam.Service.Finish(this, nUserID);
        }
        public FabricBatchProductionBeam TransferFinishBeam(long nUserID)
        {
            return FabricBatchProductionBeam.Service.TransferFinishBeam(this, nUserID);
        }
        public FabricBatchProductionBeam Get(int nEPIDID, long nUserID)
        {
            return FabricBatchProductionBeam.Service.Get(nEPIDID, nUserID);
        }
        public string Delete(int nId, long nUserID)
        {
            return FabricBatchProductionBeam.Service.Delete(nId, nUserID);
        }
        public List<FabricBatchProductionBeam> DrawingLeasingOperation(List<FabricBatchProductionBeam> oFabricBatchProductionBeams, long nUserID)
        {
            return FabricBatchProductionBeam.Service.DrawingLeasingOperation(oFabricBatchProductionBeams, nUserID);
        }
        #endregion

        #region ServiceFactory
        internal static IFabricBatchProductionBeamService Service
        {
            get { return (IFabricBatchProductionBeamService)Services.Factory.CreateService(typeof(IFabricBatchProductionBeamService)); }
        }
        #endregion
    }

    #region IFabricBatchProductionBeam interface
    public interface IFabricBatchProductionBeamService
    {
        List<FabricBatchProductionBeam> Gets(long nUserID);
        List<FabricBatchProductionBeam> GetsFinishedBeams(long nUserID);
        List<FabricBatchProductionBeam> GetsByFabricBatchProduction(int nFBPID, long nUserID);
        List<FabricBatchProductionBeam> Gets(string sSQL, long nUserID);
        FabricBatchProductionBeam Save(FabricBatchProductionBeam oFabricBatchProductionBeam, long nUserID);
        FabricBatchProductionBeam Finish(FabricBatchProductionBeam oFabricBatchProductionBeam, long nUserID);
        FabricBatchProductionBeam TransferFinishBeam(FabricBatchProductionBeam oFabricBatchProductionBeam, long nUserID);
        FabricBatchProductionBeam Get(int nEPIDID, long nUserID);
        string Delete(int id, long nUserID);
        List<FabricBatchProductionBeam> DrawingLeasingOperation(List<FabricBatchProductionBeam> oFabricBatchProductionBeams, long nUserID);
    }
    #endregion
}
