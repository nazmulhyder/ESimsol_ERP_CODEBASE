using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using ESimSol.BusinessObjects.ReportingObject;
using ESimSol.Services.DataAccess.ReportingDA;
using ICS.Core.DataAccess;
using ICS.Core.Utility;
using ICS.Core.Framework;
using ICS.Core.Utility;
namespace ESimSol.Services.Services.ReportingService
{
    public class RptStockReportSimplifiedService : MarshalByRefObject, IRptStockReportSimplifiedService
    {
        #region Private functions and declaration
        private RptStockReportSimplified MapObject(NullHandler oReader)
        {
            RptStockReportSimplified oRptStockReportSimplified = new RptStockReportSimplified();
            oRptStockReportSimplified.WorkingUnitID = oReader.GetInt32("WorkingUnitID");
            oRptStockReportSimplified.LotID = oReader.GetInt32("LotID");
            oRptStockReportSimplified.LotNo = oReader.GetString("LotNo");
            oRptStockReportSimplified.ProductID = oReader.GetInt32("ProductID");
            oRptStockReportSimplified.ProductName = oReader.GetString("ProductName");
            oRptStockReportSimplified.Brand = oReader.GetString("Brand");
            oRptStockReportSimplified.Code = oReader.GetString("Code");
            oRptStockReportSimplified.UseInProcesss = oReader.GetString("UseInProcesss");
            oRptStockReportSimplified.Category = oReader.GetInt32("Category");
            oRptStockReportSimplified.CategoryName = oReader.GetString("CategoryName");
            oRptStockReportSimplified.ProductReceivedDate = oReader.GetDateTime("");
            oRptStockReportSimplified.StoreName = oReader.GetString("StoreName");
            oRptStockReportSimplified.ImportedQty = oReader.GetDouble("ImportedQty");
            oRptStockReportSimplified.OpeningQty = oReader.GetDouble("OpeningQty");
            oRptStockReportSimplified.PTTransferedQty = oReader.GetDouble("PTTransferedQty");
            oRptStockReportSimplified.DyeingTransferdQty = oReader.GetDouble("DyeingTransferdQty");
            oRptStockReportSimplified.PrintingTransferedQty = oReader.GetDouble("PrintingTransferedQty");
            oRptStockReportSimplified.FinisingTransferedQty = oReader.GetDouble("FinisingTransferedQty");
            oRptStockReportSimplified.PretreatmentUsage = oReader.GetDouble("PretreatmentUsage");
            oRptStockReportSimplified.DyeingUsage = oReader.GetDouble("DyeingUsage");
            oRptStockReportSimplified.PrintingUsage = oReader.GetDouble("PrintingUsage");
            oRptStockReportSimplified.FinisingUsage = oReader.GetDouble("FinisingUsage");
            oRptStockReportSimplified.StockInMainStore = oReader.GetDouble("StockInMainStore");
            oRptStockReportSimplified.StockInProductionFloor = oReader.GetDouble("StockInProductionFloor");
            oRptStockReportSimplified.Unit = oReader.GetInt32("Unit");
            oRptStockReportSimplified.StockValue = oReader.GetDouble("StockValue");
            oRptStockReportSimplified.ManufacturingDate = oReader.GetDateTime("ManufacturingDate");
            oRptStockReportSimplified.ExpireyDate = oReader.GetDateTime("ExpireyDate");
            oRptStockReportSimplified.LotAge = oReader.GetInt32("LotAge");
            oRptStockReportSimplified.ConsumptionFrequency = oReader.GetDouble("ConsumptionFrequency");
            oRptStockReportSimplified.ApproxStockOutDeadline = oReader.GetDateTime("ApproxStockOutDeadline");
            oRptStockReportSimplified.ImportPI = oReader.GetString("ImportPI");
            oRptStockReportSimplified.UnitPrice = oReader.GetDouble("UnitPrice");
            oRptStockReportSimplified.LCNo = oReader.GetString("LCNo");
            return oRptStockReportSimplified;
        }
        private RptStockReportSimplified CreateObject(NullHandler oReader)
        {
            RptStockReportSimplified oRptStockReportSimplified = new RptStockReportSimplified();
            oRptStockReportSimplified = MapObject(oReader);
            return oRptStockReportSimplified;
        }

        private List<RptStockReportSimplified> CreateObjects(IDataReader oReader)
        {
            List<RptStockReportSimplified> oRptStockReportSimplifieds = new List<RptStockReportSimplified>();
            NullHandler oHandler = new NullHandler(oReader);
            while (oReader.Read())
            {
                RptStockReportSimplified oItem = CreateObject(oHandler);
                oRptStockReportSimplifieds.Add(oItem);
            }
            return oRptStockReportSimplifieds;
        }
        #endregion

        #region Interface implementation
        public RptStockReportSimplifiedService() { }

        public List<RptStockReportSimplified> Gets(string sSQL, DateTime StartTime, DateTime EndTime, int StoreID, Int64 nUserId)
        {
            List<RptStockReportSimplified> oRptStockReportSimplifieds = new List<RptStockReportSimplified>();
            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = RptStockReportSimplifiedDA.Gets(tc, sSQL, StartTime, EndTime, StoreID);
                oRptStockReportSimplifieds = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get Production Cost Analysis ", e);

                #endregion
            }
            return oRptStockReportSimplifieds;
        }
        #endregion
    }
}
