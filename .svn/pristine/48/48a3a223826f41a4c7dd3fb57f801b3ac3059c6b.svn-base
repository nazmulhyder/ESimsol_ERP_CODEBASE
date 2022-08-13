using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using ESimSol.BusinessObjects;
using ESimSol.Services.DataAccess;
using ICS.Core.DataAccess;
using ICS.Core.Framework;
using ICS.Core.Utility;
using ESimSol.BusinessObjects.ReportingObject;
using ESimSol.Services.DataAccess.ReportingDA;
using ESimSol.BusinessObjects.ReportingObject;

namespace ESimSol.Services.Services
{
    public class ProductReconciliationReportService : MarshalByRefObject, IProductReconciliationReportService
    {
        #region Private functions and declaration
        int i = 0;

        private ProductReconciliationReport MapObject( NullHandler oReader)
        {
            i++;
           
           ProductReconciliationReport oProductReconciliationReport=new ProductReconciliationReport();
            oProductReconciliationReport.ProductID = oReader.GetInt32("ProductID");
            oProductReconciliationReport.ProductName = oReader.GetString("ProductName");
            oProductReconciliationReport.ProductCode = oReader.GetString("ProductCode");
            oProductReconciliationReport.ProductCategoryID = oReader.GetInt32("ProductCategoryID");
            oProductReconciliationReport.CategoryName = oReader.GetString("CategoryName");
            oProductReconciliationReport.StockInHand = oReader.GetDouble("StockInHand");
            oProductReconciliationReport.ProYetToWithLC = oReader.GetDouble("ProductionYetToWithLC");
            oProductReconciliationReport.ProYetToWithoutLC = oReader.GetDouble("ProductionYetToWithoutLC");
            oProductReconciliationReport.LCReceiveDONoReceive = oReader.GetDouble("LCReceiveDONoReceive");
            //oProductReconciliationReport.LCReceiveDONoReceive_Part = oReader.GetDouble("LCReceiveDONoReceive_Part");
            oProductReconciliationReport.PIIssueLCnDONotReceive = oReader.GetDouble("PIIssueLCnDONotReceive");
            oProductReconciliationReport.ProductionYetToSample = oReader.GetDouble("ProductionYetToSample");
            
            oProductReconciliationReport.ShipmentDone = oReader.GetDouble("ShipmentDone");
            oProductReconciliationReport.DocInCnF = oReader.GetDouble("DocInCnF");
            oProductReconciliationReport.DocReceive = oReader.GetDouble("DocReceive");
            oProductReconciliationReport.InvoiceWithoutBL = oReader.GetDouble("InvoiceWithoutBL");
            oProductReconciliationReport.LCOpen = oReader.GetDouble("LCOpen");
            oProductReconciliationReport.GoodsRelease = oReader.GetDouble("GoodsRelease");
            oProductReconciliationReport.GoodsinTrasit = oReader.GetDouble("GoodsinTrasit");
            oProductReconciliationReport.POReceive = oReader.GetDouble("POReceive");
            oProductReconciliationReport.MinimumStock = oReader.GetDouble("MinimumStock");
            oProductReconciliationReport.ProductionYetTo = oProductReconciliationReport.ProYetToWithLC + oProductReconciliationReport.ProYetToWithoutLC;

            return oProductReconciliationReport;
        }

        private ProductReconciliationReport CreateObject(NullHandler oReader)
        {
            ProductReconciliationReport oProductReconciliationReport = new ProductReconciliationReport();
          oProductReconciliationReport=  MapObject( oReader);
          return oProductReconciliationReport;
        }

        private List<ProductReconciliationReport> CreateObjects(IDataReader oReader)
        {
            List<ProductReconciliationReport> oProductReconciliationReports = new List<ProductReconciliationReport>();
            NullHandler oHandler = new NullHandler(oReader);
            while (oReader.Read())
            {
                ProductReconciliationReport oItem = CreateObject(oHandler);
                oProductReconciliationReports.Add(oItem);
            }
            return oProductReconciliationReports;
        }
        #endregion

        public List<ProductReconciliationReport> GetsPR(int nBUID, string sProductIDs, DateTime dStartDate, DateTime dEndDate, int nReportType, int nSortType, Int64 nUserID)
        {
            List<ProductReconciliationReport> oProductReconciliationReports = null;

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = ProductReconciliationReportDA.GetsPR(tc, nBUID, sProductIDs, dStartDate, dEndDate, nReportType, nSortType);
                oProductReconciliationReports = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get ProductReconciliationReports", e);
                #endregion
            }

            return oProductReconciliationReports;
        }

        public List<ProductReconciliationReport> GetsImport(int nBUID,string sProductIDs, int nReportType, Int64 nUserID)
        {
            List<ProductReconciliationReport> oProductReconciliationReports = null;

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = ProductReconciliationReportDA.GetsImport(tc,  nBUID,sProductIDs, nReportType);
                oProductReconciliationReports = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get ProductReconciliationReports", e);
                #endregion
            }

            return oProductReconciliationReports;
        }
       

    }
}
