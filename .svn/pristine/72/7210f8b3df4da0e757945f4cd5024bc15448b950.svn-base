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
    #region ProductionReport
    public class ProductionReport : BusinessObject
    {
        public ProductionReport()
        {
            ProductionSheetID = 0;
            CycleTime = 0;
            OperationEmpID = 0;
            Cavity = 0;
            OperatorName = "";
            MachinID = 0;
            MachineNo = "";
            ModelReferenceName = "";
            ColorName = "";
            ShortCounter = 0;
            CapacityPerHour = 0;
            ProductionHour = 0;
            TotalProductionCapacity = 0;
            ActualMoldingProduction = 0;
            ProductionInPercent = 0;
            ActualFinishGoods = 0;
            Remarks = "";
            BUID = 0;
            ProductCode = "";
            ProductName = "";
            SizeName = "";
            RunnerWeight = 0;
            RejectedQty = 0;
            OperatorPerMachine = 0;
            SupervisorName = "";
            ProductCategoryID = 0;
            ProductCategoryName = "";
            UnitSymbol = "";
            SheetNo = "";
            ExportPINo = "";
            CustomerName = "";
            TransactionDate = DateTime.Today;
            ShiftID =0;
            DailyProdHour =0;
            DailyTergetedProdHour =0;
            DailyProdCapacity =0;
            DailyProdQty =0;
            DailyProdEfficiency =0;
            ShiftName = "";
            ProductionReports = new List<ProductionReport>();
            RMRequisitionMaterials = new List<RMRequisitionMaterial>();
            ErrorMessage = "";
        }
        #region Properties
        public int ProductionSheetID { get; set; }
        public int CycleTime { get; set; }
        public int OperationEmpID { get; set; }
        public int Cavity { get; set; }
        public int BUID { get; set; }
        public int MachinID { get; set; }
        public string MachineNo { get; set; }
        public string OperatorName { get; set; }
        public string ModelReferenceName { get; set; }
        public string ColorName { get; set; }
        public int ShortCounter { get; set; }
        public int CapacityPerHour { get; set; }
        public double ProductionHour { get; set; }
        public double TotalProductionCapacity { get; set; }
        public double ActualMoldingProduction { get; set; }
        public double ProductionInPercent { get; set; }
        public double ActualFinishGoods { get; set; }
        public string Remarks { get; set; }
        public string ProductCode { get; set; }
        public string ProductName { get; set; }
        public string SizeName { get; set; }
        public double RunnerWeight { get; set; }
        public double RejectedQty { get; set; }
        public double OperatorPerMachine { get; set; }
        public string SupervisorName { get; set; }
        public int ProductCategoryID { get; set; }
        public string ProductCategoryName { get; set; }
        public string UnitSymbol { get; set; }
        public string SheetNo { get; set; }
        public DateTime TransactionDate { get; set; }
        public int ShiftID { get; set; }
        public double DailyProdHour { get; set; }
        public double DailyTergetedProdHour { get; set; }
        public double DailyProdCapacity { get; set; }
        public double DailyProdQty { get; set; }
        public double DailyProdEfficiency { get; set; }
        public string ShiftName { get; set; }
        public string ErrorMessage { get; set; }
        #endregion

        #region Derived Properties        
        public string ExportPINo { get; set; }
        public string CustomerName { get; set; }
        public DateTime TransactionStartDate { get; set; }
        public DateTime TransactionEndDate { get; set; }
        public List<ProductionReport> ProductionReports { get; set; }
        public List<RMRequisitionMaterial> RMRequisitionMaterials { get; set; }
        public BusinessUnit BusinessUnit { get; set; }
        public Company Company { get; set; }
        #endregion

        #region Functions
        public static List<ProductionReport> Gets(string sSQL, int nUserID)
        {
            return ProductionReport.Service.Gets(sSQL, nUserID);
        }
        #endregion


        #region ServiceFactory
        internal static IProductionReportService Service
        {
            get { return (IProductionReportService)Services.Factory.CreateService(typeof(IProductionReportService)); }
        }
        #endregion
    }
    #endregion

    #region IProductionReport interface
    public interface IProductionReportService
    {
        List<ProductionReport> Gets(string sSQL, int nUserID);
    }
    #endregion
    
   
}
