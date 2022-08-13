using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using ESimSol.BusinessObjects;
using ESimSol.Services.DataAccess;
using ICS.Core.DataAccess;
using ICS.Core.Utility;
using ICS.Core.Framework;

namespace ESimSol.Services.Services
{
    public class ConsumptionReportService : MarshalByRefObject, IConsumptionReportService
    {
        #region Private functions and declaration
        private ConsumptionReport MapObject(NullHandler oReader)
        {
            ConsumptionReport oConsumptionReport = new ConsumptionReport();
            oConsumptionReport.ITransactionID = oReader.GetInt32("ITransactionID");
            oConsumptionReport.LotNo = oReader.GetString("LotNo");
            oConsumptionReport.ProductID = oReader.GetInt32("ProductID");
            oConsumptionReport.ConsumptionUnitID = oReader.GetInt32("ConsumptionUnitID");
            oConsumptionReport.ConsumptionDetailID = oReader.GetInt32("ConsumptionDetailID");
            oConsumptionReport.ColorID = oReader.GetInt32("ColorID");
            oConsumptionReport.SizeID = oReader.GetInt32("SizeID");
            oConsumptionReport.MUnitID = oReader.GetInt32("MUnitID");
            oConsumptionReport.IssueQty = oReader.GetDouble("IssueQty");
            oConsumptionReport.UnitPrice = oReader.GetDouble("UnitPrice");
            oConsumptionReport.ConsumptionValue = oReader.GetDouble("ConsumptionValue");
            oConsumptionReport.ProductCode = oReader.GetString("ProductCode");
            oConsumptionReport.ProductName = oReader.GetString("ProductName");
            oConsumptionReport.MUnitSymbol = oReader.GetString("MUnitSymbol");
            oConsumptionReport.MUnitName = oReader.GetString("MUnitName");
            oConsumptionReport.ColorName = oReader.GetString("ColorName");
            oConsumptionReport.SizeName = oReader.GetString("SizeName");
            oConsumptionReport.ParentConsumptionUnitID = oReader.GetInt32("ParentConsumptionUnitID");
            oConsumptionReport.ParentSequence = oReader.GetInt32("ParentSequence");
            oConsumptionReport.CUSequence = oReader.GetInt32("CUSequence");
            oConsumptionReport.ParentConsumptionUnitName = oReader.GetString("ParentConsumptionUnitName");
            oConsumptionReport.ProductCategoryID = oReader.GetInt32("ProductCategoryID");
            oConsumptionReport.ProductCategoryName = oReader.GetString("ProductCategoryName");
            oConsumptionReport.ConsumptionUnitName = oReader.GetString("ConsumptionUnitName");
            oConsumptionReport.TransactionTime = oReader.GetDateTime("TransactionTime");
            oConsumptionReport.ConsumptionBy = oReader.GetInt32("ConsumptionBy");
            oConsumptionReport.ConsumptionByName = oReader.GetString("ConsumptionByName");
            oConsumptionReport.BUID = oReader.GetInt32("BUID");
            oConsumptionReport.BUName = oReader.GetString("BUName");
            oConsumptionReport.StoreID = oReader.GetInt32("StoreID");
            oConsumptionReport.StoreName = oReader.GetString("StoreName");
            oConsumptionReport.FileNo = oReader.GetString("FileNo");
            oConsumptionReport.CUGroupID = oReader.GetInt32("CUGroupID");
            oConsumptionReport.CUGroupName = oReader.GetString("CUGroupName");
            
            return oConsumptionReport;
        }

        private ConsumptionReport CreateObject(NullHandler oReader)
        {
            ConsumptionReport oConsumptionReport = new ConsumptionReport();
            oConsumptionReport = MapObject(oReader);
            return oConsumptionReport;
        }

        private List<ConsumptionReport> CreateObjects(IDataReader oReader)
        {
            List<ConsumptionReport> oConsumptionReport = new List<ConsumptionReport>();
            NullHandler oHandler = new NullHandler(oReader);
            while (oReader.Read())
            {
                ConsumptionReport oItem = CreateObject(oHandler);
                oConsumptionReport.Add(oItem);
            }
            return oConsumptionReport;
        }

        #endregion

        #region Interface implementation
        public ConsumptionReportService() { }     
        public List<ConsumptionReport> Gets(string sSQL, Int64 nUserID)
        {
            List<ConsumptionReport> oConsumptionReports = null;
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = ConsumptionReportDA.Gets(tc, sSQL);
                oConsumptionReports = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get ConsumptionReport", e);
                #endregion
            }
            return oConsumptionReports;
        }

        public List<ConsumptionReport> GetsConsumptionSummary(string sSQL, Int64 nUserID)
        {
            List<ConsumptionReport> oConsumptionReports = null;
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = ConsumptionReportDA.GetsConsumptionSummary(tc, sSQL);
                oConsumptionReports = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get ConsumptionReport", e);
                #endregion
            }
            return oConsumptionReports;
        }
        #endregion
    }
}
