using System;
using System.IO;
using System.Data;
using System.Linq;
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
    #region FabricDeliveryChallanDetail
    public class FabricDeliveryChallanDetail : BusinessObject
    {
        #region  Constructor
        public FabricDeliveryChallanDetail()
        {

            FDCDID = 0;
            FDCID = 0;
            FabricID = 0;
            LotID = 0;
            MUID = 0;
            Qty = 0;
            IsInHouse = true;
            ProductID = 0;
            ErrorMessage = "";
            FabricDeliveryChallanDetails = new List<FabricDeliveryChallanDetail>();
            ChallanDate = DateTime.Now;
            ChallanNo = "";
            UnitPrice = 0;
            Qty_DO = 0;
            CellRowSpans = new List<CellRowSpan>();
            WorkingUnitID = 0;
            ExeNo = "";
            FabricDesignName = "";
            BuyerRef = "";
            BuyerName = "";
            NoRoll = 0;
            ParentFDCID = 0;
            FSCDID = 0;
            BuyerCPName = "";
            MKTPerson = "";
            Amount = 0;
            Qty_Meter = 0;
            DOPriceType = EnumDOPriceType.None;
            FNBatchQCDetailID = 0;
            SCDetailType = EnumSCDetailType.None;
            RollNo = "";
        }
        #endregion

        #region Properties
        public int FDCDID { get; set; }
        public int FDCID { get; set; }
        public int FDODID { get; set; }
        public int LotID { get; set; }
        public int FabricID { get; set; }
        public int MUID { get; set; }
        public double LotQty { get; set; }
        public double Qty { get; set; }
        public double UnitPrice { get; set; }
        public string ErrorMessage { get; set; }
        public DateTime ChallanDate { get; set; }
        public string ChallanNo { get; set; }
        public int WorkingUnitID { get; set; }
        #endregion

        #region Derive Properties
        public int FNBatchQCDetailID { get; set; }
        public FabricDeliveryChallan FDC { get; set; }
        public string FabricNo { get; set; }
        public int ContractorID { get; set; }
        public string Construction { get; set; }
        public int ProductID { get; set; }
        public string ProductCode { get; set; }
        public string ProductName { get; set; }
        public string MUName { get; set; }
        public string LotNo { get; set; }
        public double StockInHand { get; set; }
        public double QtyInMeter { get { return this.Qty > 0 ? Global.GetMeter(this.Qty, 2) : 0; } }
        public string FEONo { get; set; }
        public string ExeNo { get; set; }
        public string BuyerName { get; set; }
        public string BuyerCPName { get; set; }
        public string MKTPerson { get; set; }
        public bool IsInHouse { get; set; }
        public string ColorInfo { get; set; }
        public string BuyerRef { get; set; }
        public string FabricDesignName { get; set; }
        public string FabricWeave { get; set; }
        public string FinishTypeName { get; set; }
        public string FinishWidth { get; set; }
        public string FabricWidth { get; set; }
        public string StyleNo { get; set; }
        public double Qty_DO { get; set; }
        public double Amount { get; set; }
        public double NoRoll { get; set; }
        public string ProcessTypeName { get; set; }
        public string Weight { get; set; }
        public int FSCDID { get; set; }
        public int ParentFDCID { get; set; }
        public EnumDOPriceType DOPriceType { get; set; }
        public EnumSCDetailType SCDetailType { get; set; }
        public string Shrinkage { get; set; }
        public string LCNo { get; set; }
        public string RollNo { get; set; }
        
        public string PINo { get; set; }
        public DateTime PIDate { get; set; }
        public DateTime LCDate { get; set; }
        public EnumFNShade ShadeID { get; set; }
        public string ShadeStr { get { return (EnumFNShade.NA == ShadeID) ? "" : this.ShadeID.ToString(); ; } }
        public string Params { get; set; }

        public List<FabricDeliveryChallanDetail> FabricDeliveryChallanDetails { get; set; }
        public string PIDateSt
        {
            get
            {
                return this.PIDate.ToString("dd MMM yyyy");
            }
        }
        public string LCDateSt
        {
            get
            {
                return this.LCDate.ToString("dd MMM yyyy");
            }
        }
        public string ChallanDateSt
        {
            get
            {
                return this.ChallanDate.ToString("dd MMM yyyy");
            }
        }
        public string ProductNameCode
        {
            get
            {
                return this.ProductName + "[" + this.ProductCode + "]";
            }
        }
        public string QtyLbsSt
        {
            get
            {
                return Global.MillionFormat(Global.GetLBS(this.Qty, 2));
            }
        }
        public string OrderNo { get { if (this.IsInHouse) return this.FEONo; else return this.FEONo; } }
        public double Qty_Meter { get; set; } //ForInterFace
        public List<CellRowSpan> CellRowSpans { get; set; }
        #endregion

        #region Functions
        public static FabricDeliveryChallanDetail Get(int nFDCDID, long nUserID)
        {
            return FabricDeliveryChallanDetail.Service.Get(nFDCDID, nUserID);
        }
        public static List<FabricDeliveryChallanDetail> Gets(int nFDCID, bool bIsSample, long nUserID)
        {
            return FabricDeliveryChallanDetail.Service.Gets(nFDCID, bIsSample, nUserID);
        }
        public static List<FabricDeliveryChallanDetail> GetsForAdj(int nContractorID, int nParentFDCID, long nUserID)
        {
            return FabricDeliveryChallanDetail.Service.GetsForAdj(nContractorID, nParentFDCID, nUserID);
        }
        public static List<FabricDeliveryChallanDetail> Gets(string sSQL, long nUserID)
        {
            return FabricDeliveryChallanDetail.Service.Gets(sSQL, nUserID);
        }
        public FabricDeliveryChallanDetail IUD(int nDBOperation, long nUserID)
        {
            return FabricDeliveryChallanDetail.Service.IUD(this, nDBOperation, nUserID);
        }
        public FabricDeliveryChallanDetail Update_Adj(long nUserID)
        {
            return FabricDeliveryChallanDetail.Service.Update_Adj(this, nUserID);
        }
        public FabricDeliveryChallanDetail SaveMultipleFDCD(long nUserID)
        {
            return FabricDeliveryChallanDetail.Service.SaveMultipleFDCD(this, nUserID);
        }
        #endregion

        #region ServiceFactory
        internal static IFabricDeliveryChallanDetailService Service
        {
            get { return (IFabricDeliveryChallanDetailService)Services.Factory.CreateService(typeof(IFabricDeliveryChallanDetailService)); }
        }
        #endregion


    }
    #endregion

    #region IFabricDeliveryChallanDetail interface
    public interface IFabricDeliveryChallanDetailService
    {
        FabricDeliveryChallanDetail Get(int nFDCDID, long nUserID);
        List<FabricDeliveryChallanDetail> Gets(int nFDCID, bool bIsSample, long nUserID);
        List<FabricDeliveryChallanDetail> Gets(string sSQL, long nUserID);
        List<FabricDeliveryChallanDetail> GetsForAdj(int nContractorID, int nParentFDCID, long nUserID);
        FabricDeliveryChallanDetail IUD(FabricDeliveryChallanDetail oFabricDeliveryChallanDetail, int nDBOperation, long nUserID);
        FabricDeliveryChallanDetail Update_Adj(FabricDeliveryChallanDetail oFabricDeliveryChallanDetail, long nUserID);
        FabricDeliveryChallanDetail SaveMultipleFDCD(FabricDeliveryChallanDetail oFabricDeliveryChallanDetail, long nUserID);
    }
    #endregion

    #region Row Span Generation
    public class RowSpanFDCD
    {
        public static List<CellRowSpan> RowMerge(List<FabricDeliveryChallanDetail> oFDCDs)
        {
            var oTFDCDs = new List<FabricDeliveryChallanDetail>();
            List<CellRowSpan> oSaleRowSpans = new List<CellRowSpan>();
            int[,] mergerCell2D = new int[1, 2];
            int[] rowIndex = new int[15];
            int[] rowSpan = new int[15];

            while (oFDCDs.Count() > 0)
            {
                oTFDCDs = oFDCDs.Where(x => x.FEONo == oFDCDs.FirstOrDefault().FEONo && x.FabricNo == oFDCDs.FirstOrDefault().FabricNo && x.Construction == oFDCDs.FirstOrDefault().Construction).ToList();
                oFDCDs.RemoveAll(x => x.FEONo == oTFDCDs.FirstOrDefault().FEONo && x.FabricNo == oTFDCDs.FirstOrDefault().FabricNo && x.Construction == oTFDCDs.FirstOrDefault().Construction);

                rowIndex[0] = rowIndex[0] + rowSpan[0]; //
                rowSpan[0] = oTFDCDs.Count(); //
                oSaleRowSpans.Add(MakeSpan.GenerateRowSpan("Span", rowIndex[0], rowSpan[0]));
            }
            return oSaleRowSpans;
        }

    }
    #endregion
}