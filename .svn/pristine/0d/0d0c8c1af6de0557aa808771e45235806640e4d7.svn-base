using System;
using System.Data;
using System.Data.SqlClient;
using ICS.Core.DataAccess;
using ESimSol.BusinessObjects.ReportingObject;
using System.Data.OleDb;
using ICS.Core.Utility;

namespace ESimSol.Services.DataAccess.ReportingDA
{
    public class RptProductionCostAnalysisDA
    {
        public RptProductionCostAnalysisDA() { }

        #region Get & Exist Function

        public static IDataReader MailContent(TransactionContext tc, int PSSID, DateTime StartTime, DateTime EndTime)
        {
            return tc.ExecuteReader("EXEC [SP_Rpt_ProductionCostAnalysis]" + "%n, %D, %D", PSSID, StartTime, EndTime);
        }
        public static IDataReader MailContentDUProductionRFT(TransactionContext tc, int PSSID, DateTime StartTime, DateTime EndTime, int nViewType)
        {
            return tc.ExecuteReader("EXEC [SP_Rpt_DUProductionRFT]" + "%n, %D, %D,%n", PSSID, StartTime, EndTime, nViewType);
        }


        #endregion
    }
}

