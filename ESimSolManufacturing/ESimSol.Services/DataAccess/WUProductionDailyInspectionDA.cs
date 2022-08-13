using System;
using System.Data;
using System.Data.SqlClient;
using ICS.Core.DataAccess;
using ESimSol.BusinessObjects.ReportingObject;
using System.Data.OleDb;
using ICS.Core.Utility;

namespace ESimSol.Services.DataAccess.ReportingDA
{
    public class WUProductionDailyInspectionDA
    {
        public WUProductionDailyInspectionDA() { }

        #region Get & Exist Function

        public static IDataReader Gets(TransactionContext tc, DateTime dtFrom, string sFEOID, string sBuyerID, string sFMID, DateTime dtTO, int TSUID)
        {
            return tc.ExecuteReader("EXEC [SP_RPT_ProductionDailyInspection]" + " %d, %s, %s, %s ,%d,%n", dtFrom, sFEOID, sBuyerID, sFMID, dtTO, TSUID);
        }


        #endregion
    }
}

