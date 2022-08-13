using System;
using System.Collections.Generic;
using System.Data;
using ESimSol.BusinessObjects;
using ESimSol.Services.DataAccess;
using ICS.Core.DataAccess;
using ICS.Core.Framework;
using ICS.Core.Utility;
namespace ESimSol.Services.Services
{
    public class ProductionReportService : MarshalByRefObject, IProductionReportService
    {
        #region Private functions and declaration
        private ProductionReport MapObject(NullHandler oReader)
        {
            ProductionReport oProductionReport = new ProductionReport();
            oProductionReport.ProductionSheetID = oReader.GetInt32("ProductionSheetID");
            oProductionReport.CycleTime = oReader.GetInt32("CycleTime");
            oProductionReport.OperationEmpID = oReader.GetInt32("OperationEmpID");
            oProductionReport.Cavity = oReader.GetInt32("Cavity");
            oProductionReport.OperatorName = oReader.GetString("OperatorName");
            oProductionReport.MachinID = oReader.GetInt32("MachinID");
            oProductionReport.MachineNo = oReader.GetString("MachineNo");
            oProductionReport.ModelReferenceName = oReader.GetString("ModelReferenceName");
            oProductionReport.ColorName = oReader.GetString("ColorName");
            oProductionReport.ShortCounter = oReader.GetInt32("ShortCounter");
            oProductionReport.CapacityPerHour = oReader.GetInt32("CapacityPerHour");
            oProductionReport.ProductionHour = oReader.GetInt32("ProductionHour");
            oProductionReport.TotalProductionCapacity = oReader.GetInt32("TotalProductionCapacity");
            oProductionReport.ActualMoldingProduction = oReader.GetInt32("ActualMoldingProduction");
            oProductionReport.ProductionInPercent = oReader.GetInt32("ProductionInPercent");
            oProductionReport.ActualFinishGoods = oReader.GetInt32("ActualFinishGoods");
            oProductionReport.Remarks = oReader.GetString("Remarks");
            oProductionReport.ProductCode = oReader.GetString("ProductCode");
            oProductionReport.ProductName = oReader.GetString("ProductName");
            oProductionReport.SizeName = oReader.GetString("SizeName");
            oProductionReport.RunnerWeight = oReader.GetDouble("RunnerWeight");
            oProductionReport.RejectedQty = oReader.GetDouble("RejectedQty");
            oProductionReport.OperatorPerMachine = oReader.GetDouble("OperatorPerMachine");
            oProductionReport.SupervisorName = oReader.GetString("SupervisorName");
            oProductionReport.ProductCategoryID = oReader.GetInt32("ProductCategoryID");
            oProductionReport.ProductCategoryName = oReader.GetString("ProductCategoryName");
            oProductionReport.UnitSymbol = oReader.GetString("UnitSymbol");
            oProductionReport.SheetNo = oReader.GetString("SheetNo");
            oProductionReport.TransactionDate = oReader.GetDateTime("TransactionDate");
            oProductionReport.ShiftID = oReader.GetInt32("ShiftID");
            oProductionReport.DailyProdHour = oReader.GetDouble("DailyProdHour");
            oProductionReport.DailyTergetedProdHour = oReader.GetDouble("DailyTergetedProdHour");
            oProductionReport.DailyProdCapacity = oReader.GetDouble("DailyProdCapacity");
            oProductionReport.DailyProdQty = oReader.GetDouble("DailyProdQty");
            oProductionReport.DailyProdEfficiency = oReader.GetDouble("DailyProdEfficiency");
            oProductionReport.ShiftName = oReader.GetString("ShiftName");
            return oProductionReport;
        }

        private ProductionReport CreateObject(NullHandler oReader)
        {
            ProductionReport oProductionReport = new ProductionReport();
            oProductionReport = MapObject(oReader);
            return oProductionReport;
        }

        private List<ProductionReport> CreateObjects(IDataReader oReader)
        {
            List<ProductionReport> oProductionReport = new List<ProductionReport>();
            NullHandler oHandler = new NullHandler(oReader);
            while (oReader.Read())
            {
                ProductionReport oItem = CreateObject(oHandler);
                oProductionReport.Add(oItem);
            }
            return oProductionReport;
        }

        #endregion

        #region Interface implementation
        public ProductionReportService() { }
        public List<ProductionReport> Gets(string sSQL, int nUserID)
        {
            List<ProductionReport> oProductionReports = new List<ProductionReport>();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();
                IDataReader reader = null;
                reader = ProductionReportDA.Gets(tc, sSQL);
                oProductionReports = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get ProductionReport", e);
                #endregion
            }

            return oProductionReports;
        }
        #endregion
    }   
    
   
}
