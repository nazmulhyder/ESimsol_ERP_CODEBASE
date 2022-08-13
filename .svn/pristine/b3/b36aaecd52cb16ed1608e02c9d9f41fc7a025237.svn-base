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
    public class FPMgtReportService : MarshalByRefObject, IFPMgtReportService
    {
        #region Private functions and declaration
        private FPMgtReport MapObject(NullHandler oReader)
        {
            FPMgtReport oFPMgtReport = new FPMgtReport();
            oFPMgtReport.BUID = oReader.GetInt32("BUID");
            oFPMgtReport.BUCode = oReader.GetString("BUCode");
            oFPMgtReport.BUName = oReader.GetString("BUName");
            oFPMgtReport.BUShortName = oReader.GetString("BUShortName");
            oFPMgtReport.CurrencyName = oReader.GetString("CurrencyName");

            oFPMgtReport.DocInHand = oReader.GetDouble("DocInHand");
            oFPMgtReport.DocInCollection = oReader.GetDouble("DocInCollection");
            oFPMgtReport.FCAmount = oReader.GetDouble("FCAmount");
            oFPMgtReport.FDRAmount = oReader.GetDouble("FDRAmount");
            oFPMgtReport.TotalRecAndMargin = oReader.GetDouble("TotalRecAndMargin");
            oFPMgtReport.BillsLiability = oReader.GetDouble("BillsLiability");
            oFPMgtReport.BillsPayableBTB = oReader.GetDouble("BillsPayableBTB");
            oFPMgtReport.AccountsPayableBTB = oReader.GetDouble("AccountsPayableBTB");
            oFPMgtReport.TotalBTBLiability = oReader.GetDouble("TotalBTBLiability");
            oFPMgtReport.STLPackingCredit = oReader.GetDouble("STLPackingCredit");
            oFPMgtReport.STLCashIncentive = oReader.GetDouble("STLCashIncentive");
            oFPMgtReport.STLLATREDF = oReader.GetDouble("STLLATREDF");
            oFPMgtReport.STLCashCredit = oReader.GetDouble("STLCashCredit");
            oFPMgtReport.STLFDBPLDBPCredit = oReader.GetDouble("STLFDBPLDBPCredit");
            oFPMgtReport.TotalSTLAmount = oReader.GetDouble("TotalSTLAmount");
            oFPMgtReport.LTLLocalAmount = oReader.GetDouble("LTLLocalAmount");
            oFPMgtReport.LTLOBUAmount = oReader.GetDouble("LTLOBUAmount");
            oFPMgtReport.TotalLTLAmount = oReader.GetDouble("TotalLTLAmount");
            oFPMgtReport.LCInHand = oReader.GetDouble("LCInHand");
            oFPMgtReport.ContactInHand = oReader.GetDouble("ContactInHand");
            oFPMgtReport.TotalLCContact = oReader.GetDouble("TotalLCContact");
            oFPMgtReport.PreviousMonthLiability = oReader.GetDouble("PreviousMonthLiability");
           
            return oFPMgtReport;
        }

        private FPMgtReport CreateObject(NullHandler oReader)
        {
            FPMgtReport oFPMgtReport = new FPMgtReport();
            oFPMgtReport = MapObject(oReader);
            return oFPMgtReport;
        }

        private List<FPMgtReport> CreateObjects(IDataReader oReader)
        {
            List<FPMgtReport> oFPMgtReport = new List<FPMgtReport>();
            NullHandler oHandler = new NullHandler(oReader);
            while (oReader.Read())
            {
                FPMgtReport oItem = CreateObject(oHandler);
                oFPMgtReport.Add(oItem);
            }
            return oFPMgtReport;
        }

        #endregion

        #region Interface implementation
        public FPMgtReportService() { }

        public List<FPMgtReport> GetsFPMgtReports(DateTime PositionDate, int CurrencyID, bool IsApproved, int nUserID)
        {
            List<FPMgtReport> oFPMgtReports = new List<FPMgtReport>();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();
                IDataReader reader = null;
                //reader = FPMgtReportDA.Gets(tc);
                reader = FPMgtReportDA.GetsFPMgtReports(tc, PositionDate, CurrencyID, IsApproved, nUserID);
                oFPMgtReports = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get FPMgtReport", e);
                #endregion
            }
            return oFPMgtReports;
        }

        #endregion
    }
}
