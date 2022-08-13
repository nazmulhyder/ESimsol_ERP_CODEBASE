using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using ESimSol.BusinessObjects;
using ESimSol.BusinessObjects.ReportingObject;
using ESimSol.Services.DataAccess.ReportingDA;
using ICS.Core.DataAccess;
using ICS.Core.Utility;
using ICS.Core.Framework;

namespace ESimSol.Services.Services.ReportingService
{
    public class RptDailyLogReportService : MarshalByRefObject, IRptDailyLogReportService
    {
        #region Private functions and declaration
        private RptDailyLogReport MapObject(NullHandler oReader)
        {
            RptDailyLogReport oRptDailyLogReport = new RptDailyLogReport();
            oRptDailyLogReport.StartTime = oReader.GetDateTime("StartTime");
            oRptDailyLogReport.EndTime = oReader.GetDateTime("EndTime");
            oRptDailyLogReport.FEOID = oReader.GetInt32("FEOID");
            oRptDailyLogReport.FEONo = oReader.GetString("FEONo");
            oRptDailyLogReport.OrderType = (EnumFabricRequestType)oReader.GetInt32("OrderType");
            oRptDailyLogReport.IsInHouse = oReader.GetBoolean("IsInHouse");
            oRptDailyLogReport.Construction = oReader.GetString("Construction");
            oRptDailyLogReport.BuyerName = oReader.GetString("BuyerName");
            oRptDailyLogReport.TotalEnds = oReader.GetInt32("TotalEnds");
            oRptDailyLogReport.FabricWeaveName = oReader.GetString("FabricWeaveName");
            oRptDailyLogReport.ReedCount = oReader.GetDouble("ReedCount");
            oRptDailyLogReport.Dent = oReader.GetString("Dent");
            oRptDailyLogReport.MachineCode = oReader.GetString("MachineCode");
            oRptDailyLogReport.RefNo = oReader.GetString("RefNo");
            oRptDailyLogReport.RunLoom = oReader.GetInt32("RunLoom");
            oRptDailyLogReport.StopLoom = oReader.GetInt32("StopLoom");
            oRptDailyLogReport.Remark = oReader.GetString("Remark");
            oRptDailyLogReport.IsYarnDyed = oReader.GetBoolean("IsYarnDyed");
            oRptDailyLogReport.BuyerID = oReader.GetInt32("BuyerID");
            oRptDailyLogReport.FabricWeave = oReader.GetInt32("FabricWeave");
            oRptDailyLogReport.ProcessType = oReader.GetInt32("ProcessType");
            oRptDailyLogReport.StopLoomNo = oReader.GetString("StopLoomNo");
            oRptDailyLogReport.TSUID = oReader.GetInt32("TSUID");
            oRptDailyLogReport.TSUName = oReader.GetString("TSUName");
            oRptDailyLogReport.WarpColor = oReader.GetString("WarpColor");
            oRptDailyLogReport.WeftColor = oReader.GetString("WeftColor");
            oRptDailyLogReport.FBPID = oReader.GetInt32("FBPID");
            oRptDailyLogReport.StopLoomFMIDs = oReader.GetString("StopLoomFMIDs");
            oRptDailyLogReport.RunLoomFMIDs = oReader.GetString("RunLoomFMIDs");
            oRptDailyLogReport.WarpLot = oReader.GetString("WarpLot");
            oRptDailyLogReport.WeftLot = oReader.GetString("WeftLot");
            return oRptDailyLogReport;
        }

        private RptDailyLogReport CreateObject(NullHandler oReader)
        {
            RptDailyLogReport oRptDailyLogReport = new RptDailyLogReport();
            oRptDailyLogReport = MapObject(oReader);
            return oRptDailyLogReport;
        }

        private List<RptDailyLogReport> CreateObjects(IDataReader oReader)
        {
            List<RptDailyLogReport> oRptDailyLogReports = new List<RptDailyLogReport>();
            NullHandler oHandler = new NullHandler(oReader);
            while (oReader.Read())
            {
                RptDailyLogReport oItem = CreateObject(oHandler);
                oRptDailyLogReports.Add(oItem);
            }
            return oRptDailyLogReports;
        }
        #endregion

        #region Interface implementation
        public RptDailyLogReportService() { }

        public List<RptDailyLogReport> Gets(Int64 nUserId)
        {
            List<RptDailyLogReport> oRptDailyLogReports = new List<RptDailyLogReport>(); ;
            RptDailyLogReport oRptDailyLogReport= new RptDailyLogReport();
            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();
                IDataReader reader = null;
                reader = RptDailyLogReportDA.Gets(tc);
                oRptDailyLogReports = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception ex)
            {
                #region Handle Exception
                oRptDailyLogReport.ErrorMessage = ex.Message;
                oRptDailyLogReports = new List<RptDailyLogReport>();
                oRptDailyLogReports.Add(oRptDailyLogReport);
                #endregion
            }
            return oRptDailyLogReports;
        }
        public List<RptDailyLogReport> Gets(string sSQL, Int64 nUserId)
        {
            List<RptDailyLogReport> oRptDailyLogReports = new List<RptDailyLogReport>(); ;
            RptDailyLogReport oRptDailyLogReport = new RptDailyLogReport();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();
                IDataReader reader = null;
                reader = RptDailyLogReportDA.Gets(tc, sSQL);
                oRptDailyLogReports = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception ex)
            {
                #region Handle Exception
                oRptDailyLogReport.ErrorMessage = ex.Message;
                oRptDailyLogReports = new List<RptDailyLogReport>();
                oRptDailyLogReports.Add(oRptDailyLogReport);
                #endregion
            }
            return oRptDailyLogReports;
        }
       
        public List<RptDailyLogReport> Gets(DateTime dtLoomStart, string sFEOIDs, string sBuyerIDs, int nFabricWeave, int nProcessType, string sConstruction, int nTsuid,double ReedCount, long nUserID)
        {
            List<RptDailyLogReport> oRptDailyLogReports = new List<RptDailyLogReport>(); ;
            RptDailyLogReport oRptDailyLogReport = new RptDailyLogReport();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();
                IDataReader reader = null;
                reader = RptDailyLogReportDA.Gets(tc, dtLoomStart, sFEOIDs, sBuyerIDs, nFabricWeave, nProcessType, sConstruction, nTsuid,ReedCount);
                oRptDailyLogReports = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception ex)
            {
                #region Handle Exception
                oRptDailyLogReport.ErrorMessage = ex.Message;
                oRptDailyLogReports = new List<RptDailyLogReport>();
                oRptDailyLogReports.Add(oRptDailyLogReport);
                #endregion
            }
            return oRptDailyLogReports;
        }
        #endregion
    }
}
