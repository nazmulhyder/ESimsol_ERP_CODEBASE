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
    #region FabricBatchLoom
    public class FabricBatchLoom : BusinessObject
    {
        public FabricBatchLoom()
        {
            FabricBatchLoomID = 0;
            FBID = 0;
            WeavingProcess = EnumWeavingProcess.Warping;
            FMID = 0;
            StartTime = DateTime.MinValue;//set defult value like as min value
            EndTime = DateTime.MinValue;//set defult value like as min value
            Qty = 0;
            RPM = 0;
            Texture = "";
            BatchNo = "";
            //FabricBatchQty = 0;
            FEONo = "";
            IsInHouse = true;
            TotalEnds = 0;
            NoOfSection = 0;
            WarpCount = 0;
            OrderType = EnumOrderType.None;
            BuyerName = "";
            Construction = "";
            WarpDoneQty = 0;
            FabricBatchStatus = EnumFabricBatchState.Initialize;
            FabricMachineName = "";
            MachineStatus = EnumMachineStatus.Free;
            BeamStatus = EnumMachineStatus.Free;
            Params = "";
            ErrorMessage = "";
            Reed = "";
            IssueDate = new DateTime(1900, 01, 01);
            FLPID = 0;
            FabricWeave = "";// EnumFabricWeave.None;
            FabricBatchProductionColors = new List<FabricBatchProductionColor>();
            FabricBatchLoomDetails = new List<FabricBatchLoomDetail>();
            FabricBatchProductionBreakages = new List<FabricBatchProductionBreakage>();
            FabricMachines = new List<FabricMachine>();
            FabricBreakages = new List<FabricBreakage>();
            BatchMans = new List<Employee>();
            HRMShifts = new List<HRMShift>();
            FabricBatchRawMaterials = new List<FabricBatchRawMaterial>();
            IsFromRunOut = false;
            ProductID = 0;
            ProductName = "";
            BeamID = 0;
            IsRunOut = false;
            ReedCount = 0;
            Dent = "";
            BatchStatus = EnumBatchStatus.None;
            TSUID = 0;
            IsHold = false;
            Efficiency = 0;
            Pick = 0;
            ColorName = string.Empty;
            ShiftID = 0;
            Weave = "";
            FabricBatch = new FabricBatch();
            FabricBatchProductionBeam = new FabricBatchProductionBeam();
            FBPBeamID = 0;
            Status = EnumFabricBatchStatus.Initialize;
            FSCDID = 0;
            BuyerID = 0;
            QtyPro = 0;
            RSShifts = new List<RSShift>();
            BUID = 0;
            FEOSID = 0;
            BeamQty = 0;
            IsDrawing = 0;
            FEOS = new FabricExecutionOrderSpecification();
        }

        #region Properties
        public int FabricBatchLoomID { get; set; }
        public int FBID { get; set; }
        public int FLPID { get; set; }
        public string BatchNo { get; set; }
        public EnumWeavingProcess WeavingProcess { get; set; }
        public string FabricWeave { get; set; }
        public string FabricMachineName { get; set; }
        public double Qty { get; set; }
        public EnumFabricBatchState FabricBatchStatus { get; set; }
        public int ShiftID { get; set; }
        public EnumFabricBatchState FBPriviousStatus { get; set; }
        public int FabricBatchStatusInInt { get; set; }
        public string FEONo { get; set; }
        public string BuyerName { get; set; }
        public int BuyerID { get; set; }
        public int FSCDID { get; set; }
        public int FEOSID { get; set; }
        public string Reed { get; set; }
        public int BUID { get; set; }
        public string Texture { get; set; }
        public int FMID { get; set; }
        public EnumMachineStatus MachineStatus { get; set; }
        public EnumMachineStatus BeamStatus { get; set; }
        public EnumFabricBatchStatus Status { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public double WarpDoneQty { get; set; } 
        public double Efficiency { get; set; }
        public double Pick { get; set; }
        public string Construction { get; set; }
        public int RPM { get; set; }
        public double QtyInMtr
        {
            get
            {
                return Global.GetMeter(this.Qty, 2);
            }
        }
        public string QtyInMtrSt
        {
            get
            {
                return Global.MillionFormat(Global.GetMeter(this.Qty, 2));
            }
        }
        public string ErrorMessage { get; set; }
        public string Params { get; set; }
        public FabricSalesContractDetail FabricSalesContractDetail { get; set; }
        public List<FabricSalesContractDetail> FabricSalesContractDetails { get; set; }
        public DateTime IssueDate { get; set; }
        public bool IsInHouse { get; set; }
        public EnumOrderType OrderType { get; set; }
        public bool IsYarnOut { get; set; }
        public double TotalEnds { get; set; }
        public string Weave { get; set; }
        public int NoOfSection { get; set; }
        public double WarpCount { get; set; }
        public bool IsFromRunOut { get; set; }
        public int ProductID { get; set; }
        public string ProductName { get; set; }
        public double ReedCount { get; set; }
        public String Dent { get; set; }
        public int TSUID { get; set; }
        public bool IsHold { get; set; }
        #endregion

        #region Derive Property 
        public int FBPBeamID { get; set; }
        public string ColorName { get; set; }
        public int BeamID { get; set; }
        public bool IsRunOut { get; set; }
        public double QtyPro { get; set; }
        public double BeamQty { get; set; }
        public FabricBatch FabricBatch { get; set; }
        public FabricExecutionOrderSpecification FEOS { get; set; }
        public string ShiftName { get; set; }
        public DateTime ShiftStartTime { get; set; }
        public DateTime ShiftEndTime { get; set; }
        public FabricBatchProductionBeam FabricBatchProductionBeam { get; set; }
        public List<FabricMachine> FabricMachines { get; set; }
        public List<FabricBreakage> FabricBreakages { get; set; }
        public List<Employee> BatchMans { get; set; }
        public List<HRMShift> HRMShifts { get; set; }
        public List<RSShift> RSShifts { get; set; }
        public List<FabricBatchProductionColor> FabricBatchProductionColors { get; set; }
        public List<FabricBatchLoomDetail> FabricBatchLoomDetails { get; set; }
        public List<FabricBatchProductionBreakage> FabricBatchProductionBreakages { get; set; }
        public List<FabricBatchRawMaterial> FabricBatchRawMaterials { get; set; }
        //public List<FabricBatchLoomBeam> FBPBs { get; set; }
        //public List<FabricBatchLoomBeam> FBPBs1 { get; set; }
        public EnumBatchStatus BatchStatus { get; set; }
        public FabricBatchSizingSolution oFBSS { get; set; }
        public double BeamQtyInM { get { return Global.GetMeter(this.BeamQty, 2); } }
        public double QtyProInM { get { return Global.GetMeter(this.QtyPro, 2); } }
        public int IsDrawing { get; set; }
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
        public string FSpcTypeSt
        {
            get
            {
                return EnumObject.jGet(this.FSpcType);
            }
        }
        public double RemainingQty 
        {
            get { return BeamQty - QtyPro; }
        }
        public double RemainingQtyInM
        {
            get { return Global.GetMeter(this.BeamQty, 2) - Global.GetMeter(this.QtyPro, 2); }
        }
        public string IssueDateSt
        {
            get
            {
                DateTime MinValue = new DateTime(1900, 01, 01, 1, 1, 1);
                if (this.IssueDate == MinValue)
                {
                    return "";
                }
                return IssueDate.ToString("dd MMM yyyy");
            }
        }
        public string StatusSt
        {
            get
            {
                return EnumObject.jGet(this.Status);
            }
        }
        public string ShiftWithDuration
        {
            get
            {
                return this.ShiftName + "(" + this.ShiftStartTime.ToString("H:mm") + "-" + this.ShiftEndTime.ToString("H:mm") + ")";
            }
        }
        public string StartTimeSt
        {
            get
            {
               
                if (this.StartTime == DateTime.MinValue)
                {
                    return "";
                }
                else
                {
                    return StartTime.ToString("dd MMM yyyy HH:mm");
                }
                
            }
        }
        public string StartDateSt
        {
            get
            {
               // DateTime MinValue = new DateTime(1900, 01, 01, 1, 1, 1);
                if (this.StartTime == DateTime.MinValue)
                {
                    return "";
                }
                return StartTime.ToString("dd MMM yyyy");
            }
        }
        public string EndDateSt
        {
            get
            {
                //DateTime MinValue = new DateTime(1900, 01, 01, 1, 1, 1);
                if (this.EndTime == DateTime.MinValue)
                {
                    return "";
                }
                return EndTime.ToString("dd MMM yyyy");
            }
        }
        public string OrderNo
        {
            get
            {
                string sPrifix = "";
                //if (this.FabricSalesContractDetailID > 0)
                //{
                //    if (this.IsInHouse) { sPrifix = "EXE"; } else sPrifix = "SCW";

                //    if (this.OrderType == EnumOrderType.Bulk) { sPrifix = sPrifix + "-BLK-"; }
                //    else if (this.OrderType == EnumOrderType.AkijExtra) { sPrifix = sPrifix + "-EXT-"; }
                //    else if (this.OrderType == EnumOrderType.Development) { sPrifix = sPrifix + "-DEV-"; }
                //    else if (this.OrderType == EnumOrderType.SMS) { sPrifix = sPrifix + "-SMS-"; }
                    return sPrifix + this.FEONo;

                //}
                 return "";
            }
            set { }
        }
        public string FabricBatchStatusInString
        {
            get
            {
                return this.FabricBatchStatus.ToString();
            }
        }
        public string QtySt
        {
            get
            {
                if (this.Qty == 0) { return ""; }
                else { return Global.MillionFormat(this.Qty); }
            }
        }
        public string StartDateStForTimeSpan
        {
            get
            {
                return this.StartTime.ToString("HH:mm");

            }
        }
        public string EndDateStForTimeSpan
        {
            get
            {
                return this.EndTime.ToString("HH:mm");

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
        }
        //public string BatchDuration
        //{
        //    get
        //    {
        //        if (this.FabricBatchStatus == EnumFabricBatchState.Warping || this.FabricBatchStatus == EnumFabricBatchState.Sizing || this.FabricBatchStatus == EnumFabricBatchState.DrawingIn || this.FabricBatchStatus == EnumFabricBatchState.Weaving)
        //        {
        //            return this.StartTime.ToString("dd MMM yyyy HH:mm") + " to Running";
        //        }
        //        else if (this.FabricBatchStatus == EnumFabricBatchState.warping_Finish || this.FabricBatchStatus == EnumFabricBatchState.Sizing_Finish || this.FabricBatchStatus == EnumFabricBatchState.DrawingIn_Finish || this.FabricBatchStatus == EnumFabricBatchState.Weaving_Finish)
        //        {
        //            return this.StartTime.ToString("dd MMM yyyy HH:mm") + " to " + this.EndTime.ToString("dd MMM yyyy HH:mm");
        //        }
        //        else
        //        {
        //            return "";
        //        }
        //    }
        //}
        public string BatchDuration
        {
            get
            {
                if (this.Pick >0  && ((this.RPM * this.Efficiency)>0))
                {
                    double nDur = this.QtyInMtr / ((this.RPM * this.Efficiency * 24 * 60) / (this.Pick * 39.37 * 100));
                    string sDur = nDur.ToString("0.0");
                    Int64 nDay = Convert.ToInt32(sDur.Split('.')[0]);
                    Int64 nHour = Convert.ToInt32(sDur.Split('.')[1]);
                    DateTime dt = this.StartTime;
                    dt = dt.AddDays(nDay);
                    dt = dt.AddHours(nHour);
                    return sDur + "    (" + dt.ToString("dd MMM yyyy hh:mm tt") + ")";
                }                    
                return "";
            }
        }
        public string ReedCountWithDent
        {
            get
            {
                string sStr = "";
                if (this.ReedCount > 0)
                {
                    sStr += this.ReedCount.ToString().Split('.')[0];
                }
                if (!string.IsNullOrEmpty(this.Dent))
                {
                    sStr += "/" + this.Dent.ToString();
                }
                //return this.ReedCount.ToString().Split('.')[0] + "/" + this.Dent.ToString().Split('.')[0];
                return sStr;
            }
        }


        #region Exccel Upload
        public string MachineCode { get; set; }
        public string BeamNo { get; set; }
        public int TempIndex { get; set; }
        #endregion

        #endregion

        #region Functions
        public static List<FabricBatchLoom> Gets(int nFBID, long nUserID)
        {
            return FabricBatchLoom.Service.Gets(nFBID, nUserID);
        }
        public static List<FabricBatchLoom> Gets(string sSQL, long nUserID)
        {
            return FabricBatchLoom.Service.Gets(sSQL, nUserID);
        }
        public static List<FabricBatchLoom> GetsSummery(string sSQL, long nUserID)
        {
            return FabricBatchLoom.Service.GetsSummery(sSQL, nUserID);
        }
        public FabricBatchLoom Get(int nId, long nUserID)
        {
            return FabricBatchLoom.Service.Get(nId, nUserID);
        }

        public FabricBatchLoom GetByBatchAndWeavingType(int nFBID, EnumWeavingProcess eProcess, long nUserID)
        {
            return FabricBatchLoom.Service.GetByBatchAndWeavingType(nFBID, (int)eProcess, nUserID);
        }
        public FabricBatchLoom Save(long nUserID)
        {
            return FabricBatchLoom.Service.Save(this, nUserID);
        }
     
        public static List<FabricBatchLoom> ImportFabricBatchLoom(List<FabricBatchLoom> oFBPs, long nUserID)
        {
            return FabricBatchLoom.Service.ImportFabricBatchLoom(oFBPs, nUserID);
        }
        public FabricBatchLoom UpdateWeaving(long nUserID)
        {
            return FabricBatchLoom.Service.UpdateWeaving(this, nUserID);
        }
        public string Delete(int id, long nUserID)
        {
            return FabricBatchLoom.Service.Delete(id, nUserID);
        }
        #endregion

        #region ServiceFactory
        internal static IFabricBatchLoomService Service
        {
            get { return (IFabricBatchLoomService)Services.Factory.CreateService(typeof(IFabricBatchLoomService)); }
        }
        #endregion
    }
    #endregion

    #region IFabricBatchLoom interface
    public interface IFabricBatchLoomService
    {
        List<FabricBatchLoom> Gets(int nFBID, long nUserID);
        List<FabricBatchLoom> Gets(string sSQL, long nUserID);
        List<FabricBatchLoom> GetsSummery(string sSQL, long nUserID);
        FabricBatchLoom Get(int id, long nUserID);
        FabricBatchLoom GetByBatchAndWeavingType(int id, int eProcess, long nUserID); //
        FabricBatchLoom Save(FabricBatchLoom oFabricBatchLoom, long nUserID);
        List<FabricBatchLoom> ImportFabricBatchLoom(List<FabricBatchLoom> oFBPs, long nUserID);
        FabricBatchLoom UpdateWeaving(FabricBatchLoom oFBP, long nUserID);
        string Delete(int id, Int64 nUserID);
    }
    #endregion
}
