using System;
using System.IO;
using ESimSol.BusinessObjects;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Collections.Generic;
using System.Drawing;
using ICS.Core;
using ICS.Core.Framework;
using ICS.Core.Utility;

namespace ESimSol.BusinessObjects
{
    #region FabricExecutionOrder

    public class FabricExecutionOrder : BusinessObject
    {
        #region  Constructor
        public FabricExecutionOrder()
        {
            FEOID = 0;
            FEONo = "";
            FabricID = 0;
            ProductID = 0;
            OrderType = EnumOrderType.None;
            BuyerID = 0;
            ContractorPersonalID = 0;
            StyleNo = "";
            MktPersonID = 0;
            OrderRef = "";
            BuyerRef = "";
            Process = "";
            Emirzing = "";
            LightSourceDes = "";
            ReqFinishedGSM = 0;
            GarmentWash = "";
            TestStandard = "";
            FinalInspection = "";
            FinishWidth = "";
            CW = "";
            EndUse = "";
            Qty = 0;
            PPSampleQty = 0;
            TestSampleQty = 0;
            Note = "";
            OrderDate = DateTime.Now;
            ExpectedDeliveryDate = DateTime.Now;
            ExpDelEndDate = DateTime.Now;
            IsInHouse = true;
            SaleOrderID = 0;
            ApproveDate = DateTime.MinValue;
            ErrorMessage = "";
            FEODetails = new List<FabricExecutionOrderDetail>();
            FEONotes = new List<FabricExecutionOrderNote>();
            Params = "";
            IsNewFEOSpecification = false;
            FabricDeliveryOrderQty = 0;
            FEOColor = "";
            PreShipmentSampleReq = false;
            IsFinish = false;
            LightSourceID = 0;
            PPSampleDate = DateTime.Today;
            PreSampleDate = DateTime.Today;
            GreyQty = 0;
            FactoryName = "";
            FDO = "";
            DEONo = "";
           
            FabricDeliveryChallans = new List<FabricDeliveryChallan>();
            FabricDeliveryChallanDetails = new List<FabricDeliveryChallanDetail>();

            FabricIssueDate = DateTime.Today;
            FabricApprovedDate = DateTime.Today;
            FabricRemark = "";
            ProcessType = 0;
            FabricWeave = 0;
            FinishType = 0;

            ProductionDoneQty = 0;
            InProductionQty = 0;
            WarpDoneQty = 0;
            IsTapModule = false;
            IsCheckMaxQty = false;
            MaxQty = 0;
            IssueDatePI = DateTime.MinValue;
            OpeningDateLC = DateTime.MinValue;
            FEOFs = new List<FabricExecutionOrderFabric>();
            NoOfProcessProgram = 0;
            PreparedByName = "";

            ExportPIs = new List<ExportPI>();
            DONo = "";
            MUName = "";
            IsYarnDyed = false;
            FEOYarnQty = 0;
            YarnTransferQty = 0;
            FEOYRs = new List<FabricExecutionOrderYarnReceive>();
            CountFBQCDetail = 0;
            FYDNo = string.Empty;
           
           
        }
        #endregion

        #region Properties
     
        public int FEOID { get; set; }
        public string FEONo { get; set; }
        public int FabricID { get; set; }
        public int ProductID { get; set; }
        public EnumOrderType OrderType { get; set; }
        public int BuyerID { get; set; }
        public int ContractorPersonalID { get; set; }
        public string StyleNo { get; set; }
        public int MktPersonID { get; set; }
        public string OrderRef { get; set; }
        public string BuyerRef { get; set; }
        public string Process { get; set; }
        public string Emirzing { get; set; }
        public string LightSourceDes { get; set; }
        public double ReqFinishedGSM { get; set; }
        public string GarmentWash { get; set; }
        public string TestStandard { get; set; }
        public string FinalInspection { get; set; }
        public string FinishWidth { get; set; }
        public string CW { get; set; }
        public string EndUse { get; set; }
        public double Qty { get; set; }
        public double PPSampleQty { get; set; }
        public double TestSampleQty { get; set; }
        public string Note { get; set; }
        public DateTime OrderDate { get; set; }
        public DateTime ReviseDate { get; set; }
        public DateTime ExpectedDeliveryDate { get; set; }
        public DateTime ExpDelEndDate { get; set; }
        public bool IsInHouse { get; set; }
        public int SaleOrderID { get; set; }
        public int ApproveBy { get; set; }
        public DateTime ApproveDate { get; set; }
        public string FEOColor { get; set; }
        public string FactoryName { get; set; }
        public string FDO { get; set; }
        public string DEONo { get; set; }
        public string ErrorMessage { get; set; }
        public bool PreShipmentSampleReq { get; set; }
        public bool IsFinish { get; set; }
        public int LightSourceID { get; set; }
        public int LogCount { get; set; }

        public DateTime PPSampleDate { get; set; }
        public DateTime PreSampleDate { get; set; }
        public double GreyQty { get; set; }
        public double MaxQty { get; set; }
        public bool IsCheckMaxQty { get; set; }
        //write By Mahabub
        public double ProductionDoneQty { get; set; }
        public double InProductionQty { get; set; }
        public double WarpDoneQty { get; set; }
        public bool IsTapModule { get; set; }
        public List<ExportPI> ExportPIs { get; set; }
        public List<FabricExecutionOrderYarnReceive> FEOYRs { get; set; }
        public bool IsYarnDyed { get; set; }
        public double FEOYarnQty { get; set; }
        public double YarnTransferQty { get; set; }
        public int CountFBQCDetail { get; set; }

        #endregion

        #region Derive Properties
        //write By Mahabub
        public string FYDNo { get; set; }
        public string FYDNoWithDONo
        {
            get
            {

                if (!string.IsNullOrEmpty(DONo)){
                    if (!string.IsNullOrEmpty(FYDNo))
                        return this.DONo + ',' + this.FYDNo;
                    else return this.DONo;
                }
                    
                else
                    return this.FYDNo;

            }
        }
        public double YetToInProductionQty
        {
            get
            {
                return (this.Qty - (this.ProductionDoneQty + this.InProductionQty));

            }
        }
        public string LogCountSt
        {
            get
            {
                if (this.LogCount <= 0)
                {
                    return "";
                }
                else
                {
                    return "R-" + this.LogCount.ToString();
                }
            }
        }
        public double YetToWarpDoneQty
        {
            get
            {
                return (this.Qty - this.WarpDoneQty);

            }
        }
        public string FabricNo { get; set; }
        public string Composition { get; set; }
        public string Construction { get; set; }

        public string ColorName { get; set; }

        public int ProcessType { get; set; }
        public int FabricWeave { get; set; }
        public int FinishType { get; set; }

        public string ProcessTypeName { get; set; }
        public string FabricWeaveName { get; set; }
        public string FinishTypeName { get; set; }

        public string MUName { get; set; }
        public string Weave { get; set; }

        public string PINo { get; set; }

        public int PaymentType { get; set; }
        public string LCNo { get; set; }
        public DateTime ShipmentDate { get; set; }


        public string BuyerName { get; set; }
        public string BCPerson { get; set; }
        public string MKTPerson { get; set; }
        public string PreparedByName { get; set; }
        public string DONo { get; set; } 
        public string ApproveByName { get; set; }
        public double FabricDeliveryOrderQty { get; set; }
        public int NoOfProcessProgram { get; set; }

        
        public ExportPI ExportPI { get; set; }
        public Contractor Contractor { get; set; }
        public Company Company { get; set; }
        public List<FabricDeliveryChallan> FabricDeliveryChallans { get; set; }
        public List<FabricDeliveryChallanDetail> FabricDeliveryChallanDetails { get; set; }
        public List<FabricExecutionOrderFabric> FEOFs { get; set; }
        public DateTime FabricIssueDate { get; set; }
        public DateTime FabricApprovedDate { get; set; }
        public string FabricRemark { get; set; }

        public DateTime IssueDatePI { get; set; }

        public DateTime OpeningDateLC { get; set; }
        public string IsFinishSt
        {
            get
            {
                if (this.IsFinish) return "Finished";
                else return "-";
            }
        }


        public string IssueDatePISt
        {
            get
            {
                return (this.IssueDatePI == DateTime.MinValue) ? "" : this.IssueDatePI.ToString("dd MMM yyyy");
            }
        }

        public string OpeningDateLCSt
        {
            get
            {
                return (this.OpeningDateLC == DateTime.MinValue) ? "" : this.OpeningDateLC.ToString("dd MMM yyyy");
            }
        }

        public string FabricIssueDateSt
        {
            get
            {
                return this.FabricIssueDate.ToString("dd MMM yyyy");
            }
        }

        public string ApproveByNameSt
        {
            get
            {
                if (this.ApproveBy <= 0)
                {
                    return "-";
                }
                else
                {
                    return this.ApproveByName;
                }
            }
        }




        public double YetToFabricDeliveryOrderQty { get { return this.Qty - this.FabricDeliveryOrderQty; } }

        public int OrderTypeInt { get { return (int)this.OrderType; } }
        public string OrderTypeInStr { get { return this.OrderType.ToString(); } }

        //public string OrderNo
        //{
        //    get
        //    {
        //        string sPrifix = "";
        //        if (this.FEOID > 0)
        //        {
        //            if (this.IsInHouse) { sPrifix = "EXE"; } else sPrifix = "SCW";

        //            if (this.OrderType == EnumOrderType.Bulk) { sPrifix = sPrifix + "-BLK-"; }
        //            else if (this.OrderType == EnumOrderType.AkijExtra) { sPrifix = sPrifix + "-EXT-"; }
        //            else if (this.OrderType == EnumOrderType.Development) { sPrifix = sPrifix + "-DEV-"; }
        //            else if (this.OrderType == EnumOrderType.SMS) { sPrifix = sPrifix + "-SMS-"; }
        //            else { sPrifix = sPrifix + "-"; }

        //            return sPrifix + this.FEONo;

        //        }
        //        else return "";
        //    }
        //}
        public string OrderDateInStr { get { return this.OrderDate.ToString("dd MMM yyyy"); } }
        public string ExpectedDeliveryDateInStr { get { if (this.ExpectedDeliveryDate != DateTime.MinValue) return this.ExpectedDeliveryDate.ToString("dd MMM yyyy"); else return "-"; } }
        public string ExpDelEndDateInStr { get { if (this.ExpDelEndDate != DateTime.MinValue) return this.ExpDelEndDate.ToString("dd MMM yyyy"); else return "-"; } }
        public string ShipmentDateInStr { get { if (this.ShipmentDate != DateTime.MinValue) return this.ShipmentDate.ToString("dd MMM yyyy"); else return ""; } }
        public string ApproveDateInStr { get { if (this.ApproveBy > 0 && this.ApproveDate != DateTime.MinValue) return this.ApproveDate.ToString("dd MMM yyyy"); else  return ""; } }
        public string PPSampleDateSt
        {
            get
            {
                return (this.PPSampleDate == DateTime.MinValue) ? "" : this.PPSampleDate.ToString("dd MMM yyyy");
            }
        }
        public string PreSampleDateSt
        {
            get
            {
                return (this.PreSampleDate == DateTime.MinValue) ? "" : this.PreSampleDate.ToString("dd MMM yyyy");
             
            }
        }
        public double OrderQtyInYard { get { return this.Qty > 0 ? Math.Round(this.Qty, 2) : 0; } }
        public double OrderQtyInMeter { get { return this.Qty > 0 ? Global.GetMeter(this.Qty, 2) : 0; } }




        public double PPSampleQtyInYard { get { return this.PPSampleQty > 0 ? Math.Round(this.PPSampleQty, 2) : 0; } }
        public double PPSampleQtyInMeter { get { return this.PPSampleQty > 0 ? Global.GetMeter(this.PPSampleQty, 2) : 0; } }



        public double TestSampleQtyInYard { get { return this.TestSampleQty > 0 ? Math.Round(this.TestSampleQty, 2) : 0; } }
        public double TestSampleQtyInMeter { get { return this.TestSampleQty > 0 ? Global.GetMeter(this.TestSampleQty, 2) : 0; } }




        public List<FabricExecutionOrderDetail> FEODetails { get; set; }
        public List<FabricExecutionOrderNote> FEONotes { get; set; }

        public bool IsNewFEOSpecification { get; set; }
        public string Params { get; set; }
        #endregion

        #region Functions
        public static FabricExecutionOrder Get(int nFEOID, long nUserID)
        {
            return FabricExecutionOrder.Service.Get(nFEOID, nUserID);
        }
        public static List<FabricExecutionOrder> GetByFEONo(int nIsInHouse, string sFEONo, int nYear, long nUserID)
        {
            return FabricExecutionOrder.Service.GetByFEONo(nIsInHouse, sFEONo, nYear, nUserID);
        }
        public static List<FabricExecutionOrder> Gets(string sSQL, long nUserID)
        {
            return FabricExecutionOrder.Service.Gets(sSQL, nUserID);
        }
        /// <summary>
        /// added by fahim0abir on date: 19 Aug 2015
        /// to get single FEO using FEONO and/or PIID
        /// </summary>
        /// <param name="sSQL"></param>
        /// <param name="nUserID"></param>
        /// <returns></returns>
        public FabricExecutionOrder Get(string sSQL, long nUserID)
        {
            return FabricExecutionOrder.Service.Get(sSQL, nUserID);
        }
        public FabricExecutionOrder IUD(int nDBOperation, long nUserID)
        {
            return FabricExecutionOrder.Service.IUD(this, nDBOperation, nUserID);
        }
        public FabricExecutionOrder UnapproveFEO(long nUserID)
        {
            return FabricExecutionOrder.Service.UnapproveFEO(this, nUserID);
        }
        public FabricExecutionOrder Copy(Int64 nUserID)
        {
            return FabricExecutionOrder.Service.Copy(this, nUserID);
        }
        public FabricExecutionOrder SaveLog(Int64 nUserID)
        {
            return FabricExecutionOrder.Service.SaveLog(this, nUserID);
        }
        public static FabricExecutionOrder ProcessYarnDyed(int nFEOID, long nUserID)
        {
            return FabricExecutionOrder.Service.ProcessYarnDyed(nFEOID, nUserID);
        }
        public FabricExecutionOrder UpdateFinish(int nFEOID, bool bIsFinish, long nUserID)
        {
            return FabricExecutionOrder.Service.UpdateFinish(nFEOID, bIsFinish, nUserID);
        }
        #endregion

        #region ServiceFactory
        internal static IFabricExecutionOrderService Service
        {
            get { return (IFabricExecutionOrderService)Services.Factory.CreateService(typeof(IFabricExecutionOrderService)); }
        }
        #endregion

    }
    #endregion

    #region IFabricExecutionOrder interface
    public interface IFabricExecutionOrderService
    {
        FabricExecutionOrder Get(int nFEOID, long nUserID);
        List<FabricExecutionOrder> GetByFEONo(int nIsInHouse, string sFEONo, int nYear, long nUserID);
        List<FabricExecutionOrder> Gets(string sSQL, long nUserID);
        FabricExecutionOrder Get(string sSQL, long nUserID);
        FabricExecutionOrder IUD(FabricExecutionOrder oFabricExecutionOrder, int nDBOperation, long nUserID);
        FabricExecutionOrder UnapproveFEO(FabricExecutionOrder oFabricExecutionOrder, long nUserID);
        FabricExecutionOrder Copy(FabricExecutionOrder oFEO, Int64 nUserID);
        FabricExecutionOrder SaveLog(FabricExecutionOrder oFEO, Int64 nUserID);
        FabricExecutionOrder ProcessYarnDyed(int nFEOID, Int64 nUserID);
        FabricExecutionOrder UpdateFinish(int nFEOID, bool bIsFinish, Int64 nUserID);
    }
    #endregion
}