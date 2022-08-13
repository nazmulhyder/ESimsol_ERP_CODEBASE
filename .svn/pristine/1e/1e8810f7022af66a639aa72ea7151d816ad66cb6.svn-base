using System;
using System.IO;
using ESimSol.BusinessObjects;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Collections.Generic;
using ICS.Core.Framework;
using ICS.Core.Utility;

namespace ESimSol.BusinessObjects
{
    #region ExportPIDetail
    public class ExportPIDetail : BusinessObject
    {
        public ExportPIDetail()
        {
            ExportPIDetailID = 0;
            ExportPIID = 0;
            FabricID = 0;
            ProductID = 0;
            Qty = 0;
            MUnitID = 0;
            UnitPrice = 0;
            Amount = 0;
            ProductCode = "";
            ProductName = "";
            ProductCount = "";
            PINo = "";
            MUName = "";
            MUNameTwo = "";
            Currency = "";
            ErrorMessage = "";
            ExportPI = null;
            CRateTwo = 0;
            ColorInfo = "";
            ExportPIDetailLogID = 0;
            ExportPILogID = 0;
            StyleNo = "";
            HSCode = "";
            ColorID = 0;
            ColorName = ""; 
            ProductDescription = "";
            SizeName = "";
            ModelReferenceID =0; 
            ModelReferenceName = "";
            Measurement = "";
            OrderSheetDetailID = 0;
            YetToProductionOrderQty = 0;
            IsApplySizer = false;
            ColorQty = 0;
            ExportPIDetails = new List<ExportPIDetail>();
            ExportQualityID = 0;
            FabricNo = "";
            FabricWidth = "";
            Construction = "";
            ProcessType = 0;
            FabricWeave = 0;
            FinishType = 0;
            FabricDesignID = 0;
            ProcessTypeName = "";
            FabricWeaveName = "";
            FinishTypeName = "";
            ExportQuality = "";
            ExportQualityID = 0;
            IssueDate = DateTime.Now;
            FSCNo = "";
            RecipeID = 0;
            RecipeName = "";
            QtyCom = 0;
            ReferenceCaption = "";
            IsDeduct = false;
            Weight = "";
            Shrinkage = "";
            DyeingType = 0;
            PolyMUnitID = 0;
            ShadeType = EnumDepthOfShade.Dark;
            SaleType = EnumProductionType.Full_Solution;
        }

        #region Properties
        public int ExportPIDetailID { get; set; }
        public int ExportPIID { get; set; }
        public int FabricID { get; set; }
        public int ProductID { get; set; }
        public int PolyMUnitID { get; set; }
        public double Qty { get; set; }
        public int MUnitID { get; set; }
        public double ColorQty { get; set; }
        public double UnitPrice { get; set; }
        public double Amount { get; set; }
        public string ProductCode { get; set; }
        public string ProductName { get; set; }
        public string ProductCount { get; set; }
        public string PINo { get; set; }
        public string MUName { get; set; }
        public string Currency { get; set; }
        public string ErrorMessage { get; set; }
        public ExportPI ExportPI { get; set; }
        public int ExportPIDetailLogID { get; set; }
        public int ExportPILogID { get; set; }
        public string ReferenceCaption { get; set; }
        public double AdjQty { get; set; }
        public double AdjRate { get; set; }
        public double DocCharge { get; set; }
        public double AdjValue { get; set; }
        public double CRate { get; set; }
        public double CRateTwo { get; set; }
        public double QtyCom { get; set; }
        public int ColorID { get; set; }
        public string ColorName { get; set; }
        public string ProductDescription { get; set; }
        public string SizeName { get; set; }
        public int ModelReferenceID { get; set; }
        public string ModelReferenceName { get; set; }
        public string FSCNo { get; set; }
        public string Measurement { get; set; }
        public double YetToProductionOrderQty { get; set; }
        public double YetToDeliveryOrderQty { get; set; }
        public int OrderSheetDetailID { get; set; }
        public int ExportQualityID { get; set; }
        public string ExportQuality { get; set; }
        public int RecipeID { get; set; }
        public string RecipeName { get; set; }
        public bool IsApplySizer { get; set; }
        public bool IsDeduct { get; set; }
        public DateTime IssueDate { get; set; }
        public int DyeingType { get; set; }
        public int PackingType { get; set; }
        public EnumDepthOfShade ShadeType { get; set; }
        public EnumProductionType SaleType { get; set; }
        #endregion

        #region Derived Property
        public string BuyerName { get; set; }
        public List<ExportPIDetail> ExportPIDetails { get; set; }
      
        public string BuyerReference { get; set; }
        public string StyleNo { get; set; }
        public string ColorInfo { get; set; }
        public string HSCode { get; set; }
        public string FabricNo { get; set; }
        public string Construction { get; set; }
        public string FabricWidth { get; set; }
        public string Weight { get; set; }
        public string Shrinkage { get; set; }
        public int ProcessType { get; set; }
        public int FabricWeave { get; set; }
        public int FinishType { get; set; }
        public int FabricDesignID { get; set; }
        public string ProcessTypeName { get; set; }
        public string FabricWeaveName { get; set; }
        public string FinishTypeName { get; set; }

        public string ProductNameCode
        {
            get
            {
                return this.ProductName + "[" + this.ProductCode + "]";
            }
        }
        public string IssueDateInString
        {
            get
            {
                return this.IssueDate.ToString("dd MMM yyyy");
            }
        }
        public string AmountSt
        {
            get
            {
                if (this.IsDeduct) { return "("+Global.MillionFormat(this.Amount)+")";}
                else { return Global.MillionFormat(this.Amount); }
            }
        }
        public double AmountCom
        {
            get
            {
                return this.CRate*(this.QtyCom);
            }
        }
        public double AmountComTwo
        {
            get
            {
                return this.CRateTwo * (this.QtyCom);
            }
        }
        public double QtyTwo { get; set; }
        public double UnitPriceTwo { get; set; }
     
        public string MUNameTwo { get; set; }
     
        public double ActualQty
        {
            get
            {
                return this.Qty-this.AdjQty;
            }
        }
        public double ActualAmount
        {
            get
            {
                return (this.Qty - this.AdjQty) * (this.UnitPrice - this.AdjRate);
            }
        }
        public string QtySt
        {
            get
            {
                return Global.MillionFormatActualDigit(this.Qty);
            }
        }
        public string UnitPriceSt
        {
            get
            {
                return Global.MillionFormat(this.UnitPrice);
            }
        }
        public string QtyWithSymbol
        {
            get
            {
                return Global.MillionFormatActualDigit(this.Qty)+" "+this.MUName;
            }
        }

        //public double value
        //{
        //    get { return (this.Qty-this.AdjQty) * this.UnitPrice ;}
        //}
        public double Comvalue
        {
            get { return (this.QtyCom) * this.CRate; }

        }
        public double ActualRate
        {
            get { return this.UnitPrice - this.CRate- this.CRateTwo; }

        }
        

        #endregion

        #region Functions
        public static List<ExportPIDetail> Gets(int nExportPIID, Int64 nUserID)
        {
            return ExportPIDetail.Service.Gets(nExportPIID, nUserID);
        }
        public static List<ExportPIDetail> Gets(Int64 nUserID)
        {
            return ExportPIDetail.Service.Gets(nUserID);
        }
        public static List<ExportPIDetail> GetsByPI(int nExportPIID, Int64 nUserID)
        {
            return ExportPIDetail.Service.GetsByPI(nExportPIID, nUserID);
        }
        public static List<ExportPIDetail> GetsByPIAndSortByOrderSheet(int nExportPIID, Int64 nUserID)
        {
            return ExportPIDetail.Service.GetsByPIAndSortByOrderSheet(nExportPIID, nUserID);
        }        
        public static List<ExportPIDetail> GetsLog(int nExportPIID, Int64 nUserID)
        {
            return ExportPIDetail.Service.GetsLog(nExportPIID, nUserID);
        }
        public static List<ExportPIDetail> GetsLogDetail(int nExportPILogID, Int64 nUserID)
        {
            return ExportPIDetail.Service.GetsLogDetail(nExportPILogID, nUserID);
        }
        //
        public static List<ExportPIDetail> Gets(string sSQL, Int64 nUserID)
        {
            return ExportPIDetail.Service.Gets(sSQL, nUserID);
        }
        public ExportPIDetail Get(int id, Int64 nUserID)
        {
            return ExportPIDetail.Service.Get(id, nUserID);
        }
        public ExportPIDetail UpdateQuality(Int64 nUserID)
        {
            return ExportPIDetail.Service.UpdateQuality(this, nUserID);
        }
        public ExportPIDetail UpdateCRate(Int64 nUserID)
        {
            return ExportPIDetail.Service.UpdateCRate(this, nUserID);
        }
        #endregion

        #region conversion
      
        #endregion

        #region ServiceFactory
        internal static IExportPIDetailService Service
        {
            get { return (IExportPIDetailService)Services.Factory.CreateService(typeof(IExportPIDetailService)); }
        }
        #endregion
    }
    #endregion

    #region IExportPIDetail interface
    public interface IExportPIDetailService
    {
        ExportPIDetail Get(int id, Int64 nUserID);
        List<ExportPIDetail> Gets(int nExportPIID, Int64 nUserID);
        List<ExportPIDetail> Gets(Int64 nUserID);
        List<ExportPIDetail> GetsByPI(int nExportPIID, Int64 nUserID);
        List<ExportPIDetail> GetsByPIAndSortByOrderSheet(int nExportPIID, Int64 nUserID);        
        List<ExportPIDetail> Gets(string sSQL, Int64 nUserID);
        List<ExportPIDetail> GetsLog(int nExportPIID, Int64 nUserID);
        List<ExportPIDetail> GetsLogDetail(int nExportPILogID, Int64 nUserID);
        ExportPIDetail UpdateQuality(ExportPIDetail oExportPIDetail, Int64 nUserID);
        ExportPIDetail UpdateCRate(ExportPIDetail oExportPIDetail, Int64 nUserID);
    }
    #endregion
}
