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
    #region ExportSCDetail
    public class ExportSCDetail : BusinessObject
    {
        public ExportSCDetail()
        {
            ExportSCDetailID = 0;
            ExportSCID = 0;
            ProductID = 0;
            Qty = 0;
            MUnitID = 0;
            UnitPrice = 0;
            Amount = 0;
            Description = "";
            ProductCode = "";
            ProductName = "";
            ProductCount = "";
            PINo = "";
            MUName = "";
            Currency = "";
            ErrorMessage = "";
            ColorInfo = "";
            DOQty = 0;
            StyleNo = "";
            ColorID =0;
            ModelReferenceID =0;
            OrderSheetDetailID = 0;
            Measurement  ="";
            ProductDescription = "";
            ColorQty = 0;
            ColorName = "";
            ModelReferenceName = "";
            OrderSheetID = 0;
            IsApplySizer = false;
            YetToProductionOrderQty = 0;
            YetToDeliveryOrderQty = 0;
            SizeName = "";
            OverQty = 0;
            TotalQty = 0;
            ExportPIDetailID = 0;
            ExportSCDetails = new List<ExportSCDetail>();
            DyeingType = 0;
            BagCount = 0;
            PolyMUnitID = 0;
            IsBuyerYarn = false;
            IsBuyerDyes = false;
            IsBuyerChemical = false;
        
        }

        #region Properties
        public int ExportSCDetailID { get; set; }
        public int ExportPIDetailID { get; set; }
        public int ExportSCID { get; set; }
        public int ProductID { get; set; }
        public int ProductionType { get; set; }
        public int PolyMUnitID { get; set; }
        public double Qty { get; set; }
        public double AdjQty { get; set; }
        public double AdjRate { get; set; }
        public int MUnitID { get; set; }
        public double UnitPrice { get; set; }
        public double Amount { get; set; }
        public string Description { get; set; }
        public string ProductCode { get; set; }
        public string ProductName { get; set; }
        public string ProductCount { get; set; }
        public string PINo { get; set; }
        public string MUName { get; set; }
        public string Currency { get; set; }
        public int ColorID { get; set; }
        public int ModelReferenceID { get; set; }
        public int OrderSheetDetailID { get; set; }
        public string Measurement { get; set; }
        public string ProductDescription { get; set; }
        public int ColorQty { get; set; }
        public string ColorName { get; set; }
        public string ModelReferenceName { get; set; }
        public int OrderSheetID { get; set; }
        public bool IsApplySizer { get; set; }
        public double YetToProductionOrderQty { get; set; }
        public string SizeName { get; set; }
        public double YetToDeliveryOrderQty { get; set; }
        public double OverQty { get; set; }
        public double TotalQty { get; set; }
        public int DyeingType { get; set; }
        public int BagCount { get; set; }
        public bool IsBuyerYarn { get; set; }
        public bool IsBuyerDyes { get; set; }
        public bool IsBuyerChemical { get; set; }
        public string ErrorMessage { get; set; }
      
        #endregion

        #region Derived Property
        public string BuyerName { get; set; }
        public List<ExportSCDetail> ExportSCDetails { get; set; }
        public string Construction { get; set; }
        public string BuyerRef { get; set; }
        public string StyleNo { get; set; }
        public string ColorInfo { get; set; }
        public double POQty { get; set; }
        public double YetToPO
        {
            get
            {
                return Math.Round((this.Qty - this.POQty), 2);
            }
        }
        public double DOQty { get; set; }
        public double YetToDO
        {
            get
            {
                return Math.Round((this.Qty - this.DOQty), 2);
            }
        }
        public string ProductNameCode
        {
            get
            {
                return this.ProductName + "[" + this.ProductCode + "]";
            }
        }

        public string AmountSt
        {
            get
            {
                return this.Currency + "" + Global.MillionFormat(this.Amount);
            }
        }
        public string QtySt
        {
            get
            {
                return  Global.MillionFormat(this.Qty);
            }
        }
        public string UnitPriceSt
        {
            get
            {
                return Global.MillionFormat(this.UnitPrice);
            }
        }
        public string ProductionTypeST
        {
            get
            {
                return ((EnumProductionType)(this.ProductionType)).ToString();
            }
        }
        private double _nQtyTwo;
        public double QtyTwo
        {
            get
            {
                if(this.MUnitID==2)
                {
                    // 2-Kg To Lbs
                    _nQtyTwo = Math.Round((this.Qty * 0.4535923700), 4); //0.45359237001003542909395360718511
                }
                if (this.MUnitID == 4)
                {
                    // 2-Yard To Meter
                    _nQtyTwo = Math.Round((Qty / 1.0936133), 8);
                }
                else  
                {
                    // 2-Yard To Meter
                    _nQtyTwo = Math.Round((Qty * 0.453592370),4);//0.45359237001003542909395360718511
                   
                } 
                return _nQtyTwo;
            }
        }
        
        private string _sMUNameTwo;
        public string MUNameTwo
        {
            get
            {
                if (this.MUnitID == 3)
                {
                    /// 2-Kg To Lbs

                    _sMUNameTwo = "KG";
                }
                if (this.MUnitID == 4)
                {
                    /// 2-Yard To Meter

                    _sMUNameTwo = "MTR";
                }
                else
                {
                    /// 2-Yard To Meter
                    _sMUNameTwo = "Kg";
                }
                return _sMUNameTwo;
            }
        }
        #endregion

        #region Functions
        public static List<ExportSCDetail> Gets(int nExportLCID, Int64 nUserID)
        {
            return ExportSCDetail.Service.Gets(nExportLCID, nUserID);
        }
        public static List<ExportSCDetail> GetsByESCID(int nExportSCID, Int64 nUserID)
        {
            return ExportSCDetail.Service.GetsByESCID(nExportSCID, nUserID);
        }
        public static List<ExportSCDetail> Gets(Int64 nUserID)
        {
            return ExportSCDetail.Service.Gets(nUserID);
        }
        public static List<ExportSCDetail> GetsByPI(int nExportPIID, Int64 nUserID)
        {
            return ExportSCDetail.Service.GetsByPI(nExportPIID, nUserID);
        }
        public static List<ExportSCDetail> GetsLog(int nExportPIID, Int64 nUserID)
        {
            return ExportSCDetail.Service.GetsLog(nExportPIID, nUserID);
        }
        public static List<ExportSCDetail> Gets(string sSQL, Int64 nUserID)
        {
            return ExportSCDetail.Service.Gets(sSQL, nUserID);
        }
        public ExportSCDetail Get(int id, Int64 nUserID)
        {
            return ExportSCDetail.Service.Get(id, nUserID);
        }        
        public string Delete(Int64 nUserID)
        {
            return ExportSCDetail.Service.Delete(this, nUserID);
        }
        public ExportSCDetail Save(Int64 nUserID)
        {
            return ExportSCDetail.Service.Save(this, nUserID);
        }
        #endregion

        #region conversion
    
        #endregion

        #region ServiceFactory
        internal static IExportSCDetailService Service
        {
            get { return (IExportSCDetailService)Services.Factory.CreateService(typeof(IExportSCDetailService)); }
        }
        #endregion
    }
    #endregion

    #region IExportSCDetail interface
    public interface IExportSCDetailService
    {
        ExportSCDetail Get(int id, Int64 nUserID);
        List<ExportSCDetail> Gets(int nExportLCID, Int64 nUserID);
        List<ExportSCDetail> GetsByESCID(int nExportSCID, Int64 nUserID);
        List<ExportSCDetail> Gets(Int64 nUserID);
        List<ExportSCDetail> GetsByPI(int nExportPIID, Int64 nUserID);
        List<ExportSCDetail> Gets(string sSQL, Int64 nUserID);
        List<ExportSCDetail> GetsLog(int nExportPIID, Int64 nUserID);        
        string Delete(ExportSCDetail oExportSCDetail, Int64 nUserID);
        ExportSCDetail Save(ExportSCDetail oExportSCDetail, Int64 nUserID);
    }
    #endregion
}
