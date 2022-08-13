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
    #region FabricBatchProduction
    public class FabricBatchProduction : BusinessObject
    {
        public FabricBatchProduction()
        {
            FBPID = 0;
            FBID = 0;
            WeavingProcess = EnumWeavingProcess.Warping;
            FMID = 0;
            StartTime = DateTime.MinValue;//set defult value like as min value
            EndTime = DateTime.MinValue;//set defult value like as min value
            Qty = 0;
            Texture = "";
            BatchNo = "";
            FabricSalesContractDetailID = 0;
            FabricBatchQty = 0;
            FEONo = "";
            IsInHouse = true;
            TotalEnds = 0;
            NoOfSection = 0;
            WarpCount = 0;
            OrderType = EnumOrderType.None;
            BuyerName = "";
            FEOID = 0;
            Construction = "";
            GainLossPer = 0;
            FabricBatchStatus = EnumFabricBatchState.Initialize;
            FabricMachineName = "";
            MachineStatus = EnumMachineStatus.Free;
            BeamStatus = EnumMachineStatus.Free;
            Params = "";
            ErrorMessage = "";
            IssueDate = new DateTime(1900, 01, 01);

            FabricWeave = "";// EnumFabricWeave.None;
            FabricBatchProductionColors = new List<FabricBatchProductionColor>();
            FabricBatchProductionBatchMans = new List<FabricBatchProductionBatchMan>();
            FabricBatchProductionBreakages = new List<FabricBatchProductionBreakage>();
            FabricMachines = new List<FabricMachine>();
            FabricBreakages = new List<FabricBreakage>();
            BatchMans = new List<Employee>();
            HRMShifts = new List<HRMShift>();
            FabricBatchRawMaterials = new List<FabricBatchRawMaterial>();
            FBPBs = new List<FabricBatchProductionBeam>();
            FBPBs1 = new List<FabricBatchProductionBeam>();
            IsFromRunOut = false;
            ProductID = 0;
            ProductName = "";
            BeamID = 0;
            IsRunOut = false;
            BatchStatus = EnumBatchStatus.None;
            ColorName = string.Empty;
            ShiftID = 0;
            NoOfColorWF = 0;
            ProductionStatus = EnumProductionStatus.Initialize;
            FBPBList = new List<FabricBatchProductionBeam>();
            IsDirect = false;
            SettingLength = 0;
            FinishDate = DateTime.Today;
            FabricBatch = new FabricBatch();
            FabricBatchProductionDetails = new List<FabricBatchProductionDetail>();
            FEOSID = 0;
            QtyPrev = 0;
        }

        #region Properties
        public int FBPID { get; set; }
        public int FBID { get; set; }
        public string BatchNo { get; set; }
        public int FabricSalesContractDetailID { get; set; }
        public EnumWeavingProcess WeavingProcess { get; set; }
        public string FabricWeave { get; set; }
        public string FabricMachineName { get; set; }
        public double Qty { get; set; }
        public EnumFabricBatchState FabricBatchStatus { get; set; }
        public int ShiftID { get; set; }
        public EnumFabricBatchState FBPriviousStatus { get; set; }
        public EnumProductionStatus ProductionStatus { get; set; }
        public int FabricBatchStatusInInt { get; set; }
        public int FBPriviousStatusInInt { get; set; }
        public string FEONo { get; set; }
        public string BuyerName { get; set; }
        public double FabricBatchQty { get; set; }
        public string Texture { get; set; }
        public int FMID { get; set; }
        public EnumMachineStatus MachineStatus { get; set; }
        public EnumMachineStatus BeamStatus { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public double GainLossPer { get; set; }
        public string Construction { get; set; }
        public double OrderQty { get; set; }
        public double QtyPrev { get; set; }// Previous Pro Qty such as Warping, If we In Sizing
        public string ErrorMessage { get; set; }
        public string Params { get; set; }
        public FabricSalesContractDetail FabricSalesContractDetail { get; set; }
        public List<FabricSalesContractDetail> FabricSalesContractDetails { get; set; }
        public DateTime IssueDate { get; set; }
        public bool IsInHouse { get; set; }
        public EnumOrderType OrderType { get; set; }
        public double TotalEnds { get; set; }
        public int NoOfSection { get; set; }
        public double WarpCount { get; set; }
        public bool IsFromRunOut { get; set; }
        public int ProductID { get; set; }
        public string ProductName { get; set; }
        public int NoOfColor { get; set; }/// Warp Color Count
        public int NoOfColorWF { get; set; }/// Weft Color Count
        public DateTime FinishDate { get; set; }
        #endregion

        #region Derive Property 
        public string ColorName { get; set; }
        public EnumPlanType PlanType { get; set; }
        public int BeamID { get; set; }
        public int FEOID { get; set; }
        public int FEOSID { get; set; }
        public EnumFabricSpeType FSpcType { get; set; }
        public bool IsRunOut { get; set; }
        public bool IsDirect { get; set; }
        public int WarpBeam { get; set; }
        public List<FabricMachine> FabricMachines { get; set; }
        public List<FabricBreakage> FabricBreakages { get; set; }
        public List<Employee> BatchMans { get; set; }
        public List<HRMShift> HRMShifts { get; set; }
        public List<FabricBatchProductionColor> FabricBatchProductionColors { get; set; }
        public List<FabricBatchProductionBatchMan> FabricBatchProductionBatchMans { get; set; }
        public List<FabricBatchProductionDetail> FabricBatchProductionDetails { get; set; }
        public List<FabricBatchProductionBreakage> FabricBatchProductionBreakages { get; set; }
        public List<FabricBatchRawMaterial> FabricBatchRawMaterials { get; set; }
        public List<FabricBatchProductionBeam> FBPBs { get; set; }
        public List<FabricBatchProductionBeam> FBPBList { get; set; }
        public List<FabricBatchProductionBeam> FBPBs1 { get; set; }
        public FabricBatch FabricBatch { get; set; }
        public EnumBatchStatus BatchStatus { get; set; }
        public FabricBatchSizingSolution oFBSS { get; set; }
        public double SettingLength { get; set; }
        public double SettingLengthinMeter { get { return Global.GetMeter(this.SettingLength, 2); } }
        public string FinishDateInString
        {
            get
            {
                return this.FinishDate.ToString("dd MMM yyyy");
            }
        }
        public string LowerUpperType
        {
            get
            {
                if ((int)FSpcType == 1 && (int)PlanType == 2) return "Lower Beam";
                else if ((int)FSpcType == 1 && (int)PlanType != 2) return "Upper Beam";
                else return "";
            }
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
        public string EndTimeSt
        {
            get
            {

                if (this.EndTime == DateTime.MinValue)
                {
                    return "";
                }
                else
                {
                    return EndTime.ToString("dd MMM yyyy HH:mm");
                }

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
        }
        public string FabricBatchStatusST
        {
            get
            {
                return this.FabricBatchStatus.ToString();
            }
        }
        public double QtySt
        {
            get
            {
                if (this.Qty == 0) { return 0.00; }
                else { return Convert.ToDouble(Global.MillionFormat(this.Qty)); }
            }
        }
        public double QtyInMeterSt
        {         
            get
            {
                var Result = Global.GetMeter(this.Qty, 2);
                if (this.Qty == 0) { return 0.00; }               
                else { return Result; }
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
        public string BatchDuration
        {
            get
            {
                if (this.FabricBatchStatus == EnumFabricBatchState.Warping || this.FabricBatchStatus == EnumFabricBatchState.Sizing || this.FabricBatchStatus == EnumFabricBatchState.DrawingIn || this.FabricBatchStatus == EnumFabricBatchState.Weaving)
                {
                    return this.StartTime.ToString("dd MMM yyyy HH:mm") + " to Running";
                }
                else if (this.FabricBatchStatus == EnumFabricBatchState.warping_Finish || this.FabricBatchStatus == EnumFabricBatchState.Sizing_Finish || this.FabricBatchStatus == EnumFabricBatchState.DrawingIn_Finish || this.FabricBatchStatus == EnumFabricBatchState.Weaving_Finish)
                {
                    return this.StartTime.ToString("dd MMM yyyy HH:mm") + " to " + this.EndTime.ToString("dd MMM yyyy HH:mm");
                }
                else
                {
                    return "";
                }
            }
        }
        public string WeavingProcessST
        {
            get
            {
                return  this.WeavingProcess.ToString();
                
            }
        }
        public string ProductionStatusSt
        {
            get
            {
                return EnumObject.jGet(this.ProductionStatus);

            }
        }

        #region Exccel Upload
        public string MachineCode { get; set; }
        public string BeamNo { get; set; }
        public int TempIndex { get; set; }
        #endregion

        #endregion

        #region Functions
        public static List<FabricBatchProduction> Gets(int nFBID, long nUserID)
        {
            return FabricBatchProduction.Service.Gets(nFBID, nUserID);
        }
        public static List<FabricBatchProduction> Gets(string sSQL, long nUserID)
        {
            return FabricBatchProduction.Service.Gets(sSQL, nUserID);
        }
        //public static List<FabricBatchProduction> GetsSummery(string sSQL, long nUserID)
        //{
        //    return FabricBatchProduction.Service.GetsSummery(sSQL, nUserID);
        //}
        public FabricBatchProduction Get(int nId, long nUserID)
        {
            return FabricBatchProduction.Service.Get(nId, nUserID);
        }

        public FabricBatchProduction GetByBatchAndWeavingType(int nFBID, EnumWeavingProcess eProcess, long nUserID)
        {
            return FabricBatchProduction.Service.GetByBatchAndWeavingType(nFBID, (int)eProcess, nUserID);
        }
        public FabricBatchProduction Save(long nUserID)
        {
            return FabricBatchProduction.Service.Save(this, nUserID);
        }
        public string Delete(long nUserID)
        {
            return FabricBatchProduction.Service.Delete(this, nUserID);
        }
     
        #endregion

        #region ServiceFactory
        internal static IFabricBatchProductionService Service
        {
            get { return (IFabricBatchProductionService)Services.Factory.CreateService(typeof(IFabricBatchProductionService)); }
        }
        #endregion
    }
    #endregion

    #region IFabricBatchProduction interface
    public interface IFabricBatchProductionService
    {
        List<FabricBatchProduction> Gets(int nFBID, long nUserID);
        List<FabricBatchProduction> Gets(string sSQL, long nUserID);
        //List<FabricBatchProduction> GetsSummery(string sSQL, long nUserID);
        FabricBatchProduction Get(int id, long nUserID);
        FabricBatchProduction GetByBatchAndWeavingType(int id, int eProcess, long nUserID); //
        FabricBatchProduction Save(FabricBatchProduction oFabricBatchProduction, long nUserID);
        string Delete(FabricBatchProduction oFabricBatchProduction, long nUserID);
    }
    #endregion
}
