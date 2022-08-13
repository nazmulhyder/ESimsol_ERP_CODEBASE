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
    #region ExportSCDetailDO
    public class ExportSCDetailDO : BusinessObject
    {
        public ExportSCDetailDO()
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
            StyleNo = "";
            ExportSCDetailDOs = new List<ExportSCDetailDO>();
        }

        #region Properties
        public int ExportSCDetailID { get; set; }
        public int ExportSCID { get; set; }
        public int ProductID { get; set; }
        public double Qty { get; set; }
        public int ProductionType { get; set; }
        public double POQty { get; set; }/// Dyeing Order/Production  Order Qty
        public double DOQty { get; set; } /// Delivery Order Qty
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
        public string ErrorMessage { get; set; }
      
        #endregion

        #region Derived Property
        public string BuyerName { get; set; }
        public List<ExportSCDetailDO> ExportSCDetailDOs { get; set; }
        public string Construction { get; set; }
        public string BuyerRef { get; set; }
        public string StyleNo { get; set; }
        public string ColorInfo { get; set; }
        public string ProductNameCode
        {
            get
            {
                return this.ProductName + "[" + this.ProductCode + "]";
            }
        }
        public double YetToPO
        {
            get
            {
                return Math.Round((this.Qty - this.POQty),2);
            }
        }
        public double YetToDO
        {
            get
            {
                return Math.Round((this.Qty - this.DOQty), 2);
            }
        }
        public string AmountSt
        {
            get
            {
                return this.Currency+""+Global.MillionFormat(this.Qty*this.UnitPrice);
            }
        }
      
      
      
        #endregion

        #region Functions
        public static List<ExportSCDetailDO> Gets(int nExportLCID, Int64 nUserID)
        {
            return ExportSCDetailDO.Service.Gets(nExportLCID, nUserID);
        }
        public static List<ExportSCDetailDO> GetsByESCID(int nExportSCID, Int64 nUserID)
        {
            return ExportSCDetailDO.Service.GetsByESCID(nExportSCID, nUserID);
        }
        public static List<ExportSCDetailDO> Gets(Int64 nUserID)
        {
            return ExportSCDetailDO.Service.Gets(nUserID);
        }
        public static List<ExportSCDetailDO> GetsByPI(int nExportPIID, Int64 nUserID)
        {
            return ExportSCDetailDO.Service.GetsByPI(nExportPIID, nUserID);
        }
     
        public static List<ExportSCDetailDO> Gets(string sSQL, Int64 nUserID)
        {
            return ExportSCDetailDO.Service.Gets(sSQL, nUserID);
        }
        public ExportSCDetailDO Get(int id, Int64 nUserID)
        {
            return ExportSCDetailDO.Service.Get(id, nUserID);
        }        
       
        #endregion

        #region conversion
    
        #endregion

        #region ServiceFactory
        internal static IExportSCDetailDOService Service
        {
            get { return (IExportSCDetailDOService)Services.Factory.CreateService(typeof(IExportSCDetailDOService)); }
        }
        #endregion
    }
    #endregion

    #region IExportSCDetailDO interface
    public interface IExportSCDetailDOService
    {
        ExportSCDetailDO Get(int id, Int64 nUserID);
        List<ExportSCDetailDO> Gets(int nExportLCID, Int64 nUserID);
        List<ExportSCDetailDO> GetsByESCID(int nExportSCID, Int64 nUserID);
        List<ExportSCDetailDO> Gets(Int64 nUserID);
        List<ExportSCDetailDO> GetsByPI(int nExportPIID, Int64 nUserID);
        List<ExportSCDetailDO> Gets(string sSQL, Int64 nUserID);
       
      
    }
    #endregion
}
