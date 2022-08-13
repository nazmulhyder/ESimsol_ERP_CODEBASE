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
    #region FabricBatch
    public class FabricBatch : BusinessObject
    {
        public FabricBatch()
        {
            FBID = 0;
            BatchNo = "";
            FabricSalesContractDetailID = 0;
            StyleNo = "";
            Qty = 0;
            Status = EnumFabricBatchState.Initialize;
            StatusInInt = (int)EnumFabricBatchState.Initialize;
            IssueDate = DateTime.Now;
            TotalEnds = 0;
            NoOfSection = 0;
            WarpCount = 1;
            FEONo = "";
            BuyerName = "";
            Construction = "";
            OrderQty = 0;
            NoOfWarp = 0;
            NoOfWeft = 0;
            IsInHouse = true;
            OrderType = EnumOrderType.None;
            IsRawMaterialOut = false;
            Params = "";
            IsYarn = true;
            FinishType = "";// EnumFinishType.None;
            ErrorMessage = "";
            FWPDID = 0;
            FMID = 0;
            FabricSalesContractDetails = new List<FabricSalesContractDetail>();
            FabricBatchRawMaterials = new List<FabricBatchRawMaterial>();
            FabricBatchHistorys = new List<FabricBatchHistory>();
            Beams = new List<FabricMachine>();
            IsNotWarpDone = false;
            WeavingProcessType = 0;
            WeavingProcessTypeTemp = 0;
            BuyerRef = "";
            Color = "";
            ProcessTypeName = "";
            Weave="";
            Width = "";
            ProductName = "";
            IsYarnDyed = false;
            FWPD_Length = 0;
            IsFromQcMenu = false;
            Texture = "";
            ReedCountWithDent = "";
            ContractionPercentage = 0;
            WarpingMachineCode = string.Empty;
            WarpingMachineStatus = EnumMachineStatus.Free;
            PONo = "";
            FEOSID = 0;
            WMCode = "";
            WarpBeam = "";
            ReedCount = 0;
            PlanType = EnumPlanType.General;
        }

        #region Properties
        public int FBID { get; set; }
        public string BatchNo { get; set; }
        public int FabricSalesContractDetailID { get; set; }
        public int FEOSID { get; set; }
        public string StyleNo { get; set; }
        public double Qty { get; set; }
        public EnumFabricBatchState Status { get; set; }
        public int StatusInInt { get; set; }
        public string FEONo { get; set; }
        public string PONo { get; set; }
        public string BuyerName { get; set; }
        public int NoOfWarp { get; set; }
        public int NoOfWeft { get; set; }
        public double QtyPro { get; set; }
        
        public string Construction { get; set; }
        public double OrderQty { get; set; }
        public EnumPlanType PlanType { get; set; }
        public string ErrorMessage { get; set; }
        public string Params { get; set; }
        public FabricSalesContractDetail FabricSalesContractDetail { get; set; }
        public List<FabricSalesContractDetail> FabricSalesContractDetails { get; set; }
        public List<FabricBatchHistory> FabricBatchHistorys { get; set; }
        public EnumWeavingProcess WeavingProcess { get; set; }
        public DateTime IssueDate { get; set; }
        public double TotalEnds { get; set; }
        public int NoOfSection { get; set; }
        public string WMCode { get; set; }
        public double WarpCount { get; set; }
        public bool IsInHouse { get; set; }
        public EnumOrderType OrderType { get; set; }
        public bool IsRawMaterialOut { get; set; }
        public string  FinishType {get;set;}
        public string WarpBeam { get; set; }
        public int FWPDID { get; set; }
        public int FMID { get; set; }
        public List<FabricMachine> Beams { get; set; }
        public bool IsNotWarpDone { get; set; }
        public int WeavingProcessType { get; set; }
        public int WeavingProcessTypeTemp { get; set; }
        public string BuyerRef { get; set; }
        public string Color { get; set; }
        public string ProcessTypeName { get; set; }
        public string Weave { get; set; }
        public string Width { get; set; }
        public string ProductName { get; set; }
        public bool IsYarnDyed { get; set; }
        public double FWPD_Length { get; set; }
        public bool IsFromQcMenu { get; set; }
        public string Texture { get; set; }
        public DateTime WarpingStartTime { get; set; }
        public DateTime SizingStartTime { get; set; }
        public DateTime DrawingStartTime { get; set; }
        public DateTime LoomStartTime { get; set; }
        public double ContractionPercentage { get; set; }
        public EnumMachineStatus WarpingMachineStatus { get; set; }
        #endregion

        #region Derive Property
        public double ReedCount { get; set; }
        public EnumFabricSpeType FSpcType { get; set; }
        public string BatchNoMCode
        {
            get
            {
                return this.WMCode + "" + this.BatchNo;
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
        public int FEOID
        {
            get
            {
                return this.FabricSalesContractDetailID;
            }
        }
        public string StatusInSt
        {
            get
            {
                return EnumObject.jGet(this.Status);
            }
        }
        public string WarpingMachineStatusStr
        {
            get
            {
                return this.WarpingMachineStatus.ToString();
            }
        }
        public string WarpingMachineCode { get; set; }
        public bool IsYarn { get; set; }
        public List<FabricBatchRawMaterial> FabricBatchRawMaterials { get; set; }

        public string ReedCountWithDent { get; set; }
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

                //    return sPrifix + this.FEONo;

                //}
                //else return "";
                return this.FEONo;
            }
        }
      
        public string FabricBatchWithStatus
        {
            get
            {
                if (this.Status >= EnumFabricBatchState.Initialize)
                {
                    return this.BatchNo + " [ " + FabricBatchStateObj.GetEnumFabricBatchStateObjs(this.Status) + " ]";
                }
                else return "";
              
            }
        }
        public double YetToInProductionQty
        {
            get
            {
                return (this.Qty - this.QtyPro);
            }
        }
      
        public string StatusSt
        {
            get
            {
                if (this.Status >= EnumFabricBatchState.Initialize)
                {
                    return FabricBatchStateObj.GetEnumFabricBatchStateObjs(this.Status);
                }
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
        public string QtySt
        {
            get
            {
                return Global.MillionFormat(this.Qty);
            }
        }
        public string QtyInMeterSt
        {
            get
            {
                return Global.MillionFormat(Global.GetMeter(this.Qty,2));
            }
        }
        public string IssueDateSt
        {
            get
            {
                return this.IssueDate.ToString("dd MMM yyyy");
            }
        }

        public string WarpingStartTimeStr
        {
            get
            {
                return (this.WarpingStartTime == DateTime.MinValue) ? "" : this.WarpingStartTime.ToString("dd MMM yyyy hh:mm tt");
            }
        }

        public string SizingStartTimeStr
        {
            get
            {
                return (this.SizingStartTime == DateTime.MinValue) ? "" : this.SizingStartTime.ToString("dd MMM yyyy hh:mm tt");
            }
        }

        public string DrawingStartTimeStr
        {
            get
            {
                return (this.DrawingStartTime == DateTime.MinValue) ? "" : this.DrawingStartTime.ToString("dd MMM yyyy hh:mm tt");
            }
        }

        public string LoomStartTimeStr
        {
            get
            {
                return (this.LoomStartTime == DateTime.MinValue) ? "" : this.LoomStartTime.ToString("dd MMM yyyy hh:mm tt");
            }
        }
        #endregion

        #region Functions
        public static List<FabricBatch> Gets(long nUserID)
        {
            return FabricBatch.Service.Gets(nUserID);
        }
        public static List<FabricBatch> GetsByFabricSalesContractDetailID(int nFabricSalesContractDetailID, long nUserID)
        {
            return FabricBatch.Service.GetsByFabricSalesContractDetailID(nFabricSalesContractDetailID, nUserID);
        }
        public static List<FabricBatch> Gets(string sSQL, long nUserID)
        {
            return FabricBatch.Service.Gets(sSQL, nUserID);
        }
        public FabricBatch Get(int nId, long nUserID)
        {
            return FabricBatch.Service.Get(nId, nUserID);
        }
        public FabricBatch GetByBatchNo(string sBatchNo, long nUserID)
        {
            return FabricBatch.Service.GetByBatchNo(sBatchNo, nUserID);
        }
        public FabricBatch Save(long nUserID)
        {
            return FabricBatch.Service.Save(this, nUserID);
        }
        public FabricBatch Finish(long nUserID)
        {
            return FabricBatch.Service.Finish(this, nUserID);
        }
        public FabricBatch BatchFinish(long nUserID)
        {
            return FabricBatch.Service.BatchFinish(this, nUserID);
        }
        
        public FabricBatch RowMatarialOut(long nUserID)
        {
            return FabricBatch.Service.RowMatarialOut(this, nUserID);
        }
        public string Delete(int nId, long nUserID)
        {
            return FabricBatch.Service.Delete(nId, nUserID);
        }

        public FabricBatch FabricProductionQCDone(long nUserID)
        {
            return FabricBatch.Service.FabricProductionQCDone(this, nUserID);
        }
        public FabricBatch UpdateBatchNo(Int64 nUserID)
        {
            return FabricBatch.Service.UpdateBatchNo(this, nUserID);
        }
        #endregion

        #region ServiceFactory
        internal static IFabricBatchService Service
        {
            get { return (IFabricBatchService)Services.Factory.CreateService(typeof(IFabricBatchService)); }
        }
        #endregion
    }
    #endregion

    #region IFabricBatch interface
    public interface IFabricBatchService
    {
        List<FabricBatch> Gets(long nUserID);
        List<FabricBatch> Gets(string sSQL, long nUserID);
        List<FabricBatch> GetsByFabricSalesContractDetailID(int nFabricSalesContractDetailID, long nUserID);
        FabricBatch Get(int id, long nUserID);
        FabricBatch GetByBatchNo(string sBatchNo, long nUserID);
        FabricBatch Save(FabricBatch oFabricBatch, long nUserID);
        FabricBatch Finish(FabricBatch oFabricBatch, long nUserID);
        FabricBatch BatchFinish(FabricBatch oFabricBatch, long nUserID);
        FabricBatch RowMatarialOut(FabricBatch oFabricBatch, long nUserID);
        FabricBatch FabricProductionQCDone(FabricBatch oFabricBatch, long nUserID);
        FabricBatch UpdateBatchNo(FabricBatch oFabricBatch, Int64 nUserID);
        string Delete(int id, long nUserID);
    }
    #endregion
}
