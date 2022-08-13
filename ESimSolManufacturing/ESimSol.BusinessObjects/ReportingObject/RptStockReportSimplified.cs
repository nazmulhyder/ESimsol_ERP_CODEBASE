using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ESimSol.BusinessObjects.ReportingObject
{
    public class RptStockReportSimplified
    {
        public RptStockReportSimplified()
        {
            WorkingUnitID = 0;
            LotID = 0;
            LotNo = "";
            ProductID = 0;
            ProductName = "";
            Brand = "";
            Code = "";
            UseInProcesss = "";
            Category = 0;
            CategoryName = "";
            ProductReceivedDate = DateTime.Now;
            StoreName = "";
            ImportedQty = 0;
            OpeningQty = 0;
            PTTransferedQty = 0;
            DyeingTransferdQty = 0;
            PrintingTransferedQty = 0;
            FinisingTransferedQty = 0;
            PretreatmentUsage = 0;
            DyeingUsage = 0;
            PrintingUsage = 0;
            FinisingUsage = 0;
            StockInMainStore = 0;
            StockInProductionFloor =0;
            Unit = 0;
            StockValue = 0;
            ManufacturingDate = DateTime.Now;
            ExpireyDate = DateTime.Now;
            LotAge = 0;
            ConsumptionFrequency = 0;
            ApproxStockOutDeadline = DateTime.Now;
            ImportPI = "";
            LCNo = "";
            UnitPrice = 0;
            ErrorMessage = "";
        }

        #region Properties
        public int WorkingUnitID { get; set; }
        public int LotID { get; set; }
        public string LotNo { get; set; }
        public int ProductID { get; set; }
        public string ProductName { get; set; }
        public string Brand { get; set; }
        public string Code { get; set; }
        public string UseInProcesss { get; set; }
        public int Category { get; set; }
        public string CategoryName { get; set; }
        public DateTime ProductReceivedDate { get; set; }
        public string StoreName { get; set; }
        public double ImportedQty { get; set; }
        public double OpeningQty { get; set; }
        public double PTTransferedQty { get; set; }
        public double DyeingTransferdQty { get; set; }
        public double PrintingTransferedQty { get; set; }
        public double FinisingTransferedQty { get; set;}
        public double PretreatmentUsage { get; set; }
        public double DyeingUsage { get; set; }
        public double PrintingUsage { get; set; }
        public double FinisingUsage { get; set; }
        public double StockInMainStore { get; set; }
        public double StockInProductionFloor { get; set; }
        public int Unit { get; set; }
        public double StockValue { get; set; }
        public DateTime ManufacturingDate { get; set; }
        public DateTime ExpireyDate { get; set; }
        public int LotAge { get; set; }
        public double ConsumptionFrequency { get; set; }
        public DateTime ApproxStockOutDeadline { get; set; }
        public string ImportPI { get; set; }
        public string LCNo { get; set; }
        public double UnitPrice { get; set; }
        public string ErrorMessage { get; set; }
        #endregion
        #region Derived Property
        public string ProductReceivedDateST
        {
            get
            {
                return this.ProductReceivedDate.ToString("dd MMM yyyy");
            }
        }
        public string ManufacturingDateST
        {
            get
            {
                return this.ManufacturingDate.ToString("dd MMM yyyy");
            }
        }
        public string ExpireyDateST
        {
            get
            {
                return this.ExpireyDate.ToString("dd MMM yyyy");
            }
        }
        public string ApproxStockOutDeadlineST
        {
            get
            {
                return this.ApproxStockOutDeadline.ToString("dd MMM yyyy");
            }
        }


        #endregion
        #region Functions
        public static List<RptStockReportSimplified> Gets(string sSQL, DateTime StartTime, DateTime EndTime, int StoreID, long nUserID)
        {
            return RptStockReportSimplified.Service.Gets(sSQL, StartTime, EndTime, StoreID, nUserID);
        }
        #endregion
        #region ServiceFactory
        internal static IRptStockReportSimplifiedService Service
        {
            get { return (IRptStockReportSimplifiedService)Services.Factory.CreateService(typeof(IRptStockReportSimplifiedService)); }
        }

        #endregion
    }
    #region IRptStockReportSimplified interface
    public interface IRptStockReportSimplifiedService
    {
        List<RptStockReportSimplified> Gets(string sSQL, DateTime StartTime, DateTime EndTime, int StoreID, long nUserID);
    }
    #endregion
}
