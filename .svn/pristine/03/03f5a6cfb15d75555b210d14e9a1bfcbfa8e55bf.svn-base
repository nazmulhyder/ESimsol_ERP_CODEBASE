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
    public class RptDailyBeamStockReportService : MarshalByRefObject, IRptDailyBeamStockReportService
    {
        #region Private functions and declaration
        private RptDailyBeamStockReport MapObject(NullHandler oReader)
        {
            RptDailyBeamStockReport oRptDailyBeamStockReport = new RptDailyBeamStockReport();
            oRptDailyBeamStockReport.StartTime = oReader.GetDateTime("StartTime");
            oRptDailyBeamStockReport.WeavingProcess = (EnumWeavingProcess)oReader.GetInt32("WeavingProcess");
            oRptDailyBeamStockReport.Construction = oReader.GetString("Construction");
            oRptDailyBeamStockReport.ReedCount = oReader.GetInt32("ReedCount");
            oRptDailyBeamStockReport.Weave = oReader.GetString("Weave");
            oRptDailyBeamStockReport.TotalEnds = oReader.GetInt32("TotalEnds");
            oRptDailyBeamStockReport.Buyer = oReader.GetString("Buyer");
            oRptDailyBeamStockReport.FEONo = oReader.GetString("FEONo");
            oRptDailyBeamStockReport.Option = oReader.GetString("Option");
            oRptDailyBeamStockReport.WeftColor = oReader.GetString("WeftColor");
            oRptDailyBeamStockReport.BeamStock = oReader.GetString("BeamStock");
            oRptDailyBeamStockReport.LoomNo = oReader.GetString("LoomNo");
            oRptDailyBeamStockReport.Remarks = oReader.GetString("Remarks");
            oRptDailyBeamStockReport.BuyerID = oReader.GetInt32("BuyerID");
            oRptDailyBeamStockReport.FEOID = oReader.GetInt32("FEOID");
            oRptDailyBeamStockReport.IsInHouse = oReader.GetBoolean("IsInHouse");
            oRptDailyBeamStockReport.OrderType = (EnumFabricRequestType)oReader.GetInt32("OrderType");
            return oRptDailyBeamStockReport;
        }
        private RptDailyBeamStockReport CreateObject(NullHandler oReader)
        {
            RptDailyBeamStockReport oRptDailyBeamStockReport = new RptDailyBeamStockReport();
            oRptDailyBeamStockReport = MapObject(oReader);
            return oRptDailyBeamStockReport;
        }
        private List<RptDailyBeamStockReport> CreateObjects(IDataReader oReader)
        {
            List<RptDailyBeamStockReport> oRptDailyBeamStockReport = new List<RptDailyBeamStockReport>();
            NullHandler oHandler = new NullHandler(oReader);
            while (oReader.Read())
            {
                RptDailyBeamStockReport oItem = CreateObject(oHandler);
                oRptDailyBeamStockReport.Add(oItem);
            }
            return oRptDailyBeamStockReport;
        }

        #endregion

        #region Interface implementation
        public List<RptDailyBeamStockReport> Gets(string sSQL, Int64 nUserID)
        {
            List<RptDailyBeamStockReport> oRptDailyBeamStockReports = new List<RptDailyBeamStockReport>();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = RptDailyBeamStockReportDA.Gets(tc, sSQL);
                oRptDailyBeamStockReports = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                oRptDailyBeamStockReports = new List<RptDailyBeamStockReport>();
                RptDailyBeamStockReport oRptDailyBeamStockReport = new RptDailyBeamStockReport();
                oRptDailyBeamStockReport.ErrorMessage = e.Message.Split('~')[0];
                oRptDailyBeamStockReports.Add(oRptDailyBeamStockReport);
                #endregion
            }
            return oRptDailyBeamStockReports;
        }
        public List<RptDailyBeamStockReport> Gets(Int64 nUserID)
        {
            List<RptDailyBeamStockReport> oRptDailyBeamStockReports = new List<RptDailyBeamStockReport>();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = RptDailyBeamStockReportDA.Gets(tc);
                oRptDailyBeamStockReports = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                oRptDailyBeamStockReports = new List<RptDailyBeamStockReport>();
                RptDailyBeamStockReport oRptDailyBeamStockReport = new RptDailyBeamStockReport();
                oRptDailyBeamStockReport.ErrorMessage = e.Message.Split('~')[0];
                oRptDailyBeamStockReports.Add(oRptDailyBeamStockReport);
                #endregion
            }
            return oRptDailyBeamStockReports;
        }

        #endregion
    }
}
