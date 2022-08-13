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


namespace ESimSol.Services.Services.ReportingService
{
    public class LotStockReportService : MarshalByRefObject, ILotStockReportService
    {
        #region MAP OBJ
        private LotStockReport MapObject(NullHandler oReader)
        {
            LotStockReport oLotStockReport = new LotStockReport();
            oLotStockReport.BUID = oReader.GetInt32("BUID");
            oLotStockReport.LotID = oReader.GetInt32("LotID");
            oLotStockReport.LCNo = oReader.GetString("LCNo");
            oLotStockReport.InvoiceNo = oReader.GetString("InvoiceNo");
            oLotStockReport.LotNo = oReader.GetString("LotNo");
            oLotStockReport.ProductID = oReader.GetInt32("ProductID");
            oLotStockReport.ProductCategoryID = oReader.GetInt32("ProductCategoryID");
            oLotStockReport.ProductBaseID = oReader.GetInt32("ProductBaseID");
            oLotStockReport.Qty_Total = oReader.GetDouble("Qty_Total");
            oLotStockReport.Balance = oReader.GetDouble("Balance");
            oLotStockReport.ProductCode = oReader.GetString("ProductCode");
            oLotStockReport.ProductName = oReader.GetString("ProductName");
            oLotStockReport.CategoryName = oReader.GetString("CategoryName");
            oLotStockReport.ProductName_Base = oReader.GetString("ProductName_Base");
            oLotStockReport.ContractorName = oReader.GetString("ContractorName");
            oLotStockReport.LocationName = oReader.GetString("LocationName");
            oLotStockReport.OperationUnitName = oReader.GetString("OperationUnitName");

            oLotStockReport.ReceiveDate = oReader.GetDateTime("ReceiveDate");
            oLotStockReport.IssueDate = oReader.GetDateTime("IssueDate");
            oLotStockReport.ContractorID = oReader.GetInt32("ContractorID");
            oLotStockReport.MUnitID = oReader.GetInt32("MUnitID");
            oLotStockReport.MUnit = oReader.GetString("MUnit");
            oLotStockReport.Qty_Opening = oReader.GetDouble("Qty_Opening");
            oLotStockReport.Qty_Received = oReader.GetDouble("Qty_Received");
            oLotStockReport.Qty_Issue = oReader.GetDouble("Qty_Issue");
            oLotStockReport.Qty_Balance = oReader.GetDouble("Qty_Balance");
            oLotStockReport.QtyTr = oReader.GetDouble("QtyTr");
            oLotStockReport.QtyAssign = oReader.GetDouble("QtyAssign");

            return oLotStockReport;
        }
        private LotStockReport CreateObject(NullHandler oReader)
        {
            LotStockReport oLotStockReport = new LotStockReport();
            oLotStockReport = MapObject(oReader);
            return oLotStockReport;
        }
        private List<LotStockReport> CreateObjects(IDataReader oReader)
        {
            List<LotStockReport> oLotStockReports = new List<LotStockReport>();
            NullHandler oHandler = new NullHandler(oReader);
            while (oReader.Read())
            {
                LotStockReport oItem = CreateObject(oHandler);
                oLotStockReports.Add(oItem);
            }
            return oLotStockReports;
        }

        #endregion

        #region Interface implementation
        public LotStockReportService() { }
        public List<LotStockReport> Gets(string sSQL, Int64 nUserId)
        {
            List<LotStockReport> oLotStockReports = new List<LotStockReport>(); ;
            LotStockReport oLotStockReport = new LotStockReport();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();
                IDataReader reader = null;
                reader = LotStockReportDA.Gets(tc, sSQL);
                oLotStockReports = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception ex)
            {
                #region Handle Exception
                oLotStockReport.ErrorMessage = ex.Message;
                oLotStockReports = new List<LotStockReport>();
                oLotStockReports.Add(oLotStockReport);
                #endregion
            }
            return oLotStockReports;
        }
        public List<LotStockReport> GetsRPTLot(LotStockReport oLotStockReport, Int64 nUserId)
        {
            List<LotStockReport> oLotStockReports = new List<LotStockReport>();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();
                IDataReader reader = null;
                reader = LotStockReportDA.GetsRPTLot(tc, oLotStockReport);
                oLotStockReports = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception ex)
            {
                #region Handle Exception
                oLotStockReport = new LotStockReport();
                oLotStockReport.ErrorMessage = ex.Message;
                oLotStockReports = new List<LotStockReport>();
                oLotStockReports.Add(oLotStockReport);
                #endregion
            }
            return oLotStockReports;
        }
        public List<LotStockReport> GetsAll_RPTLot(LotStockReport oLotStockReport, Int64 nUserId)
        {
            List<LotStockReport> oLotStockReports = new List<LotStockReport>();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();
                IDataReader reader = null;
                reader = LotStockReportDA.GetsAll_RPTLot(tc, oLotStockReport, nUserId);
                oLotStockReports = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception ex)
            {
                #region Handle Exception
                oLotStockReport = new LotStockReport();
                oLotStockReport.ErrorMessage = ex.Message;
                oLotStockReports = new List<LotStockReport>();
                oLotStockReports.Add(oLotStockReport);
                #endregion
            }
            return oLotStockReports;
        }

        public List<LotStockReport> Gets_StockWiseLotReport(DateTime startdate, DateTime endate, string WUIDs, int nUserID)
        {
            List<LotStockReport> oLotStockReports = new List<LotStockReport>();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();
                IDataReader reader = null;
                reader = LotStockReportDA.Gets_StockWiseLotReport(tc, startdate, endate, WUIDs, nUserID);
                oLotStockReports = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception ex)
            {
                #region Handle Exception
                LotStockReport oLotStockReport = new LotStockReport();
                oLotStockReport.ErrorMessage = ex.Message;
                oLotStockReports = new List<LotStockReport>();
                oLotStockReports.Add(oLotStockReport);
                #endregion
            }
            return oLotStockReports;
        }
        #endregion
    }
}
