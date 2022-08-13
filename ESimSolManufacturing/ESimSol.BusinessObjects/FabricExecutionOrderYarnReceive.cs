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
    #region FabricExecutionOrderYarnReceive

    public class FabricExecutionOrderYarnReceive : BusinessObject
    {
        public FabricExecutionOrderYarnReceive()
        {
            FEOYID = 0;
            WYRequisitionID = 0;
            FSCDID = 0;
            ReceiveQty = 0;
            ReceiveBy = 0;
            ReceiveDate = DateTime.MinValue;
            WUID = 0;
            ErrorMessage = "";
            Params = "";
            CellRowSpans = new List<CellRowSpan>();
            LotBalance = 0;
            FEOYRs = new List<FabricExecutionOrderYarnReceive>();
            //IsAvailable = false; //Yarn is available in My own stock or not.
            RequiredQty = 0;
            IsFabricYarnOrderAllocation = false;
            Value = 0;
            FYDCDetailID = 0;
            ReqQty = 0;
            DyeingOrderID = 0;
            DestinationLotID = 0;
            Remarks = "";
            BuyerName = "";
            RequisitionNo = "";
            BeamFinish = 0;
            Dia = "";
            IssuedLength = 0.0;
            TFLength = 0.0;
            TFLengthLB = 0;
            BeamCount = 0;
            TFConeSet = 0;

            WarpWeftType = EnumWarpWeft.None;
            FEOSDID = 0;
            ExeNo = "";
            FEOSID = 0;
            ReceiveByName="";
            IssueDate = DateTime.Now;
            IssueBy=0;
            IssueByName = "";
        }
        public int FEOYID { get; set; }
        public int WYRequisitionID { get; set; }
        public int FSCDID { get; set; }
        public int FEOSDID { get; set; }
       
        public double ReceiveQty { get; set; }
        public int ReceiveBy { get; set; }
        public DateTime ReceiveDate { get; set; }
        public int DyeingOrderID { get; set; }/// For Report
        public int WUID { get; set; }
        public int SUDeliveryChallanDetailID { get; set; }
        public string ErrorMessage { get; set; }
        public string Params { get; set; }
        public EnumWYarnType WYarnType { get; set; }
        //public bool IsAvailable { get; set; }
        public List<CellRowSpan> CellRowSpans { get; set; }
        public List<FabricExecutionOrderYarnReceive> FEOYRs { get; set; }
        public double Value { get; set; }
        public string Dia { get; set; }
        public double IssuedLength { get; set; }
        public double TFLength { get; set; }
        public double TFLengthLB { get; set; }

        public double BeamFinish { get; set; }
        public int BeamCount { get; set; }
        public int TFConeSet { get; set; }
        public string BeamNo { get; set; }
        public double RequiredWarpLength { get; set; }
        public string SearchStringDate { get; set; }
        public EnumInOutType RequisitionType { get; set; }
        public EnumWarpWeft WarpWeftType { get; set; }
        public double  ReqQty { get; set; }
        public double OrderQty { get; set; }
        public string Remarks { get; set; }
        public int DyeingOrderDetailID  { get; set; }
        public int IssueLotID  { get; set; }
        public string DestinationLotNo { get; set; }
        public int DestinationLotID { get; set; }
        public string RequisitionNo { get; set; }
        public double ReqCones { get; set; }
        public int NumberOfCone { get; set; }

        #region Derive Value
        public int BUID { get; set; }
        public string ExeNo { get; set; }
        public int FEOSID { get; set; }
        public string ReceiveByName { get; set; }
        public DateTime IssueDate { get; set; }
        public int IssueBy { get; set; }
        public string IssueByName { get; set; }

        public double LotBalance { get; set; }
        public string UnitName { get; set; }
        public string LocationName { get; set; }
        public int ProductID { get; set; }
        public string ProductName { get; set; }
        public string ProductCode { get; set; }
        public string LotNo { get; set; }
        public double UnitPrice { get; set; }

        public int BuyerID { get; set; }
        public string DispoNo { get; set; }
        public EnumOrderType OrderType { get; set; }
        public bool IsInHouse { get; set; }
        public string BuyerName { get; set; }

        public string ColorName { get; set; }
        public string Warp { get; set; }

         public string Length { get; set; }
         public int HanksCone { get; set; }
         public int BagQty { get; set; }
         public double RequiredQty { get; set; }
         public double BalanceQty { get; set; }
         public string RequisitionTypeSt
         {
             get
             {
                 if (EnumInOutType.Disburse == this.RequisitionType) return "SRS";
                 else if (EnumInOutType.Receive == this.RequisitionType) return "SRM";
                 else return "-";
             }
         }
         public string LotBalanceSt
         {
             get
             {
                 return Global.MillionFormat(this.LotBalance);
             }
         }
         public string WarpWeftTypeSt
         {
             get
             {
                 return EnumObject.jGet(this.WarpWeftType);
             }
         }
         public string WYarnTypeStr { get { return EnumObject.jGet(this.WYarnType); } }
        public double QtyInLbs
        {
            get
            {
                return Global.GetLBS(this.ReceiveQty, 2);
            }
        }

        public string ReceiveDateInStr
        {
            get
            {
                return (this.ReceiveBy != 0) ? ReceiveDate.ToString("dd MMM yyyy hh:mm tt") : "";
            }
        }
        public string IssueDateInStr
        {
            get
            {
                return (this.IssueBy != 0) ? IssueDate.ToString("dd MMM yyyy hh:mm tt") : "";
            }
        }
        public string WUName
        {
            get
            {
                return this.UnitName + "["+ this.LocationName + "]";
            }
        }

        public string ProductNameCode
        {
            get
            {
                if (this.ProductName == "" && this.ProductCode == "") return "";
                else return this.ProductName + " [" + this.ProductCode + "]";
            }
        }

        public bool IsFabricYarnOrderAllocation { get; set; }
        public int FYDCDetailID { get; set; }
        #endregion

        #region Functions

        public static FabricExecutionOrderYarnReceive Get(int nFEOYID, long nUserID)
        {
            return FabricExecutionOrderYarnReceive.Service.Get(nFEOYID, nUserID);
        }
        public static List<FabricExecutionOrderYarnReceive> Gets(int nWYRID, long nUserID)
        {
            return FabricExecutionOrderYarnReceive.Service.Gets(nWYRID, nUserID);
        }
        public static List<FabricExecutionOrderYarnReceive> Gets(string sSQL, long nUserID)
        {
            return FabricExecutionOrderYarnReceive.Service.Gets(sSQL, nUserID);
        }
      
        public FabricExecutionOrderYarnReceive ReceiveFEOY(long nUserID)
        {
            return FabricExecutionOrderYarnReceive.Service.ReceiveFEOY(this, nUserID);
        }
        public static List<FabricExecutionOrderYarnReceive> SaveDetail(List<FabricExecutionOrderYarnReceive> oFabricExecutionOrderYarnReceives, long nUserID)
        {
            return FabricExecutionOrderYarnReceive.Service.SaveDetail(oFabricExecutionOrderYarnReceives, nUserID);
        }
    
        public string Delete(int nFEOYID, int nFEOID, long nUserID)
        {
            return FabricExecutionOrderYarnReceive.Service.Delete(nFEOYID, nFEOID, nUserID);
        }
        public FabricExecutionOrderYarnReceive UpdateObj(long nUserID)
        {
            return FabricExecutionOrderYarnReceive.Service.UpdateObj(this, nUserID);
        }
        //public FabricExecutionOrderYarnReceive IUD(int nDBOperation, long nUserID)
        //{
        //    return FabricExecutionOrderYarnReceive.Service.IUD(this, nDBOperation, nUserID);
        //}
        #endregion

        #region ServiceFactory

        internal static IFabricExecutionOrderYarnReceiveService Service
        {
            get { return (IFabricExecutionOrderYarnReceiveService)Services.Factory.CreateService(typeof(IFabricExecutionOrderYarnReceiveService)); }
        }
        #endregion
    }
    #endregion

    #region IFabricExecutionOrderYarnReceive interface

    public interface IFabricExecutionOrderYarnReceiveService
    {
        FabricExecutionOrderYarnReceive Get(int nFEOYID, Int64 nUserID);
        List<FabricExecutionOrderYarnReceive> Gets(int nWYRID, Int64 nUserID);
        List<FabricExecutionOrderYarnReceive> Gets(string sSQL, Int64 nUserID);
        List<FabricExecutionOrderYarnReceive> SaveDetail(List<FabricExecutionOrderYarnReceive> oFabricExecutionOrderYarnReceives, Int64 nUserID);
        //FabricExecutionOrderYarnReceive Receive(FabricExecutionOrderYarnReceive oFEOYR, Int64 nUserID);
        //List<FabricExecutionOrderYarnReceive> Receives(FabricExecutionOrderYarnReceive oFEOYR, Int64 nUserID);
        string Delete(int nFEOYID, int nFEOID, long nUserID);
        //FabricExecutionOrderYarnReceive IUD(FabricExecutionOrderYarnReceive oFEOYR, int nDBOperation, long nUserID);
        FabricExecutionOrderYarnReceive ReceiveFEOY(FabricExecutionOrderYarnReceive oFEOYR, Int64 nUserID);
        FabricExecutionOrderYarnReceive UpdateObj(FabricExecutionOrderYarnReceive oFabricExecutionOrderYarnReceive, Int64 nUserID);
    }
    #endregion
}
