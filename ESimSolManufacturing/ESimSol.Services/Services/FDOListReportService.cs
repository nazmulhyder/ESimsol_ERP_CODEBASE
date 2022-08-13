using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using ESimSol.BusinessObjects;
using ESimSol.Services.DataAccess;
using ICS.Core.DataAccess;
using ICS.Core.Utility;
using ICS.Core.Framework;
using ESimSol.BusinessObjects.ReportingObject;
using ESimSol.Services.DataAccess.ReportingDA;

namespace ESimSol.Services.Services.ReportingService
{
    public class FDOListReportService : MarshalByRefObject, IFDOListReportService
    {
        #region Private functions and declaration
        private FDOListReport MapObject(NullHandler oReader)
        {
            FDOListReport oFDOListReport = new FDOListReport();
            oFDOListReport.BuyerName = oReader.GetString("BuyerName");
            oFDOListReport.GarmentsName = oReader.GetString("GarmentsName");
            oFDOListReport.FEOID = oReader.GetInt32("FEOID");
            oFDOListReport.FEONo = oReader.GetString("FEONo");
            oFDOListReport.FEODate = oReader.GetDateTime("FEODate");
            oFDOListReport.FDOType = (EnumDOType)oReader.GetInt32("FDOType");
            oFDOListReport.FDODate = oReader.GetDateTime("FDODate");
            oFDOListReport.FDONo = oReader.GetString("FDONo");
            oFDOListReport.PINo = oReader.GetString("PINo");
            oFDOListReport.LCNo = oReader.GetString("LCNo");
            oFDOListReport.LCDate = oReader.GetDateTime("LCDate");
            oFDOListReport.Construction = oReader.GetString("Construction");
            oFDOListReport.OrderQty = oReader.GetDouble("OrderQty");
            oFDOListReport.Weave = oReader.GetString("Weave");
            oFDOListReport.Color = oReader.GetString("Color");
            oFDOListReport.BuyerRef = oReader.GetString("BuyerRef");
            oFDOListReport.PPSample = oReader.GetDouble("PPSample");
            oFDOListReport.BulkDelivered = oReader.GetDouble("BulkDelivered");
            oFDOListReport.TotalDelivered = oReader.GetDouble("TotalDelivered");
            oFDOListReport.Balance = oReader.GetDouble("Balance");
            oFDOListReport.OrderStock = oReader.GetDouble("OrderStock");
            oFDOListReport.PPSampleDate = oReader.GetDateTime("PPSampleDate");
            oFDOListReport.BulkStartDate = oReader.GetDateTime("BulkStartDate");
            oFDOListReport.BulkEndDate = oReader.GetDateTime("BulkEndDate");
            oFDOListReport.DelStartDate = oReader.GetDateTime("DelStartDate");
            oFDOListReport.DelEndDate = oReader.GetDateTime("DelEndDate");
            oFDOListReport.ChallanDate = oReader.GetDateTime("ChallanDate");
            oFDOListReport.CountChallan = oReader.GetInt32("CountChallan");
            oFDOListReport.FDODQty = oReader.GetDouble("FDODQty");
            oFDOListReport.FDOCQty = oReader.GetDouble("FDOCQty");
            oFDOListReport.DelCompleteDate = oReader.GetDateTime("DelCompleteDate");
            oFDOListReport.OrderRef = oReader.GetString("OrderRef");
            return oFDOListReport;
        }
        private FDOListReport CreateObject(NullHandler oReader)
        {
            FDOListReport oFDOListReport = new FDOListReport();
            oFDOListReport = MapObject(oReader);
            return oFDOListReport;
        }
        private List<FDOListReport> CreateObjects(IDataReader oReader)
        {
            List<FDOListReport> oFDOListReport = new List<FDOListReport>();
            NullHandler oHandler = new NullHandler(oReader);
            while (oReader.Read())
            {
                FDOListReport oItem = CreateObject(oHandler);
                oFDOListReport.Add(oItem);
            }
            return oFDOListReport;
        }

        #endregion

        #region Interface implementation
        public List<FDOListReport> Gets(string sSQL, Int64 nUserID)
        {
            List<FDOListReport> oFDOListReports = new List<FDOListReport>();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = FDOListReportDA.Gets(tc, sSQL);
                oFDOListReports = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                oFDOListReports = new List<FDOListReport>();
                FDOListReport oFDOListReport = new FDOListReport();
                oFDOListReport.ErrorMessage = e.Message.Split('~')[0];
                oFDOListReports.Add(oFDOListReport);
                #endregion
            }
            return oFDOListReports;
        }

        public List<FDOListReport> GetsBysp(string sParams, Int64 nUserID)
        {
            List<FDOListReport> oFDOListReports = new List<FDOListReport>();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = FDOListReportDA.GetsBysp(tc, sParams);
                oFDOListReports = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                oFDOListReports = new List<FDOListReport>();
                FDOListReport oFDOListReport = new FDOListReport();
                oFDOListReport.ErrorMessage = e.Message.Split('~')[0];
                oFDOListReports.Add(oFDOListReport);
                #endregion
            }
            return oFDOListReports;
        }
        public List<FDOListReport> Gets(Int64 nUserID)
        {
            List<FDOListReport> oFDOListReports = new List<FDOListReport>();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();
                IDataReader reader = null;
                reader = FDOListReportDA.Gets(tc);
                oFDOListReports = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                oFDOListReports = new List<FDOListReport>();
                FDOListReport oFDOListReport = new FDOListReport();
                oFDOListReport.ErrorMessage = e.Message.Split('~')[0];
                oFDOListReports.Add(oFDOListReport);
                #endregion
            }
            return oFDOListReports;
        }
        public FDOListReport Get(int id, Int64 nUserId)
        {
            FDOListReport oFDOListReport = new FDOListReport();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();
                IDataReader reader = FDOListReportDA.Get(tc, id);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oFDOListReport = CreateObject(oReader);
                }
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                oFDOListReport = new FDOListReport();
                oFDOListReport.ErrorMessage = e.Message.Split('~')[0];
                #endregion
            }
            return oFDOListReport;
        }

        #endregion
    }
}
