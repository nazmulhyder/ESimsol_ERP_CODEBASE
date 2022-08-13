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
    #region FDCRegister
    public class FDCRegister : BusinessObject
    {
        #region  Constructor
        public FDCRegister()
        {

            FDCDID=0;
            FDCID=0;
            FabricID = 0;
            LotID=0;
            MUID = 0;
            Qty = 0;
            IsInHouse = true;
            ProductID = 0;
            ErrorMessage = "";
            FDCRegisters = new List<FDCRegister>();
            ChallanDate = DateTime.Now;
            FDODate = DateTime.Now;
            DONo = "";
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
            MKTGroup = "";
            StoreName = "";
            StyleNo = "";
            DriverName = "";
            Construction = "";
            LCNo = "";
            SCNoFull = "";
            LotNo = "";
            DeliveryPoient = "";
            ChallanNo = "";
            DONo = "";
            StoreName = "";
            StyleNo = "";
            DriverName = "";
            PINo = "";
            ExeNo = "";
            SCNoFull = "";
            LotNo = "";
            VehicleNo = "";
            DODateSearchString = "";
            DCDateSearchString = "";
            ExportPIID = 0;
            QtyOrder = 0;
            QtyDelivery = 0;
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
        public bool IsDelivered { get; set; }
        public DateTime FDODate { get; set; }
        public string SCNoFull { get; set; }
        public int FEOID { get; set; }
        public string ContractorName { get; set; }
        public bool IsSample { get; set; }
        public int BuyerID { get; set; }
        public EnumDOType DOType { get; set; }
        public int ApproveBy { get; set; }
        public int DisburseBy { get; set; }
        public string DONo { get; set; }
        public string FDONo { get; set; }
        public string StoreName { get; set; }
        public string DriverName { get; set; }
        public string Params { get; set; }
        public string DODateSearchString { get; set; }
        public string DCDateSearchString { get; set; }
        public int BUID { get; set; }
        public int Printlayout { get; set; }

        #endregion

        #region Derive Properties
        public FabricDeliveryChallan FDC { get; set; }
        public string FabricNo { get; set; }
        public int ContractorID { get; set; }
        public string Construction { get; set; }
        public int ProductID { get; set; }
        public string ProductCode { get; set; }
        public string ProductName { get; set; }
        public string DisburseByName { get; set; }
        public string MUName { get; set; }
        public string LotNo { get; set; }
        public double StockInHand { get; set; }
        public double QtyInMeter { get { return this.Qty > 0 ? Global.GetMeter(this.Qty, 2) : 0; } }
        public string FEONo { get; set; }
        public string ExeNo { get; set; }
        public string BuyerName { get; set; }
        public string BuyerCPName { get; set; }
        public string MKTPerson { get; set; }
        public string MKTGroup { get; set; }
        public double YetToDelivery
        {
            get
            {
                if (this.QtyOrder < this.QtyDelivery) return 0;
                else return Math.Round((this.QtyOrder - this.QtyDelivery), 2);
            }
        }
        public double Amount
        {
            get
            {
                return this.UnitPrice * this.Qty;
            }
        }
        public int MKTPersonID { get; set; }
        public bool IsInHouse { get; set; }
        public string ColorInfo { get; set; }
        public string BuyerRef { get; set; }
        public string DeliveryPoient { get; set; }
        public string FabricDesignName { get; set; }
        public string FabricWeave { get; set; }
        public string FinishTypeName { get; set; }
        public string FinishWidth { get; set; }
        public string FabricWidth { get; set; }
        public string StyleNo { get; set; }
        public double Qty_DO { get; set; }
        public double QtyOrder { get; set; }
        public double QtyDelivery { get; set; }
        public double NoRoll { get; set; }
        public string ProcessTypeName { get; set; }
        public string Weight { get; set; }
        public int FSCDID { get; set; }
        public int ExportPIID { get; set; }
        
        public int ParentFDCID { get; set; }
        public string Shrinkage { get; set; }
        public string LCNo { get; set; }
        public string PINo { get; set; }
        public DateTime PIDate { get; set; }
        public DateTime LCDate { get; set; }
        public EnumFNShade ShadeID { get; set; }
        public string VehicleNo { get; set; }
        public string ChallanRemarks { get; set; }
        public double Qty_DC { get; set; }
        public string ShadeStr { get { return (EnumFNShade.NA == ShadeID) ? "" : this.ShadeID.ToString(); ; } }
        public string DOTypeStr 
        { 
            get 
            { 
                return (EnumDOType.None == DOType) ? "" : EnumObject.jGet(this.DOType);
            } 
        }
        
        public List<FDCRegister> FDCRegisters { get; set; }
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
        public string FDODateSt
        {
            get
            {
                return this.FDODate.ToString("dd MMM yyyy");
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
        public double YetToDC
        {
            get
            {
                return this.Qty-this.Qty_DC;
            }
        }
        public string OrderNo { get { if (this.IsInHouse) return this.FEONo; else return this.FEONo; } }

        public List<CellRowSpan> CellRowSpans { get; set; }
        #endregion

        #region Property for FDC2
        public int FDOID { get; set; }
        public int FDOType { get; set; }
        public string DCNo { get; set; }
        public DateTime DCDate { get; set; }
        public DateTime DODate { get; set; }
        public double SampleQty { get; set; }
        public int BCPID { get; set; }
        public int ExportPIDetailID { get; set; }
        public string BuyerReference { get; set; }
        public int FinishType { get; set; }
        public string FabricWeaveName { get; set; }
        public int ProcessType { get; set; }
        public int FSCID { get; set; }
        public string MUnit { get; set; }
        public bool IsPrint { get; set; }
        public bool IsYD { get; set; }
        public string DCDateSt 
        {
            get
            {
                if (DCDate == DateTime.MinValue) return "";
                return this.DCDate.ToString("dd MMM yyyy");
            }
        }
        public string DODateSt
        {
            get
            {
                if (DODate == DateTime.MinValue) return "";
                return this.DODate.ToString("dd MMM yyyy");
            }
        }
        #endregion

        #region Functions
        public static List<FDCRegister> Gets(string sSQL, long nUserID)
        {
            return FDCRegister.Service.Gets(sSQL, nUserID);
        }
        public static List<FDCRegister> Gets_FDO(string sSQL, long nUserID)
        {
            return FDCRegister.Service.Gets_FDO(sSQL, nUserID);
        }
        public static List<FDCRegister> Gets_FDC(string sSQL, long nUserID)
        {
            return FDCRegister.Service.Gets_FDC(sSQL, nUserID);
        }
        public static List<FDCRegister> Gets_For_FDC2(string sSQL, long nUserID)
        {
            return FDCRegister.Service.Gets_For_FDC2(sSQL, nUserID);
        }
        #endregion 

        #region ServiceFactory
        internal static IFDCRegisterService Service
        {
            get { return (IFDCRegisterService)Services.Factory.CreateService(typeof(IFDCRegisterService)); }
        }
        #endregion

    }
    #endregion

    #region IFDCRegister interface
    public interface IFDCRegisterService
    {
        List<FDCRegister> Gets(string sSQL, long nUserID);
        List<FDCRegister> Gets_FDO(string sSQL, long nUserID);
        List<FDCRegister> Gets_FDC(string sSQL, long nUserID);
        List<FDCRegister> Gets_For_FDC2(string sSQL, long nUserID);
    }
    #endregion 

    //#region Row Span Generation
    //public class RowSpanFDCD
    //{
    //    public static List<CellRowSpan> RowMerge(List<FDCRegister> oFDCDs)
    //    {
    //        var oTFDCDs = new List<FDCRegister>();
    //        List<CellRowSpan> oSaleRowSpans = new List<CellRowSpan>();
    //        int[,] mergerCell2D = new int[1, 2];
    //        int[] rowIndex = new int[15];
    //        int[] rowSpan = new int[15];

    //        while (oFDCDs.Count() > 0)
    //        {
    //            oTFDCDs = oFDCDs.Where(x => x.FEONo == oFDCDs.FirstOrDefault().FEONo && x.FabricNo == oFDCDs.FirstOrDefault().FabricNo && x.Construction == oFDCDs.FirstOrDefault().Construction).ToList();
    //            oFDCDs.RemoveAll(x => x.FEONo == oTFDCDs.FirstOrDefault().FEONo && x.FabricNo == oTFDCDs.FirstOrDefault().FabricNo && x.Construction == oTFDCDs.FirstOrDefault().Construction);

    //            rowIndex[0] = rowIndex[0] + rowSpan[0]; //
    //            rowSpan[0] = oTFDCDs.Count(); //
    //            oSaleRowSpans.Add(MakeSpan.GenerateRowSpan("Span", rowIndex[0], rowSpan[0]));
    //        }
    //        return oSaleRowSpans;
    //    }

    //}
    //#endregion
}
