using System;
using System.Data;
using System.Data.SqlClient;
using ESimSol.BusinessObjects;
using System.Data.OleDb;
using ICS.Core.DataAccess;
using ICS.Core.Utility;

namespace ESimSol.Services.DataAccess
{
    public class DUOrderRSDA
    {
        public DUOrderRSDA() { }

        #region Generation Function

        #endregion

        #region Get & Exist Function

        public static IDataReader Gets(TransactionContext tc, string sDyeingOrderDetailID, int nRSID, int nOrderType, int nReportType)
        {
            return tc.ExecuteReader("EXEC [sp_RPT_DUOrderRS] '" + sDyeingOrderDetailID + "'," + nRSID + "," + nOrderType + "," + nReportType);
        }
        public static IDataReader Gets(TransactionContext tc, string sSql, int nReportType)
        {
            return tc.ExecuteReader("EXEC [sp_RPT_DUOrderRS_Report] '" + sSql + "'," + nReportType);
        }
        public static IDataReader GetsQC(TransactionContext tc, string sSql, int nReportType)
        {
            return tc.ExecuteReader("EXEC [SP_RPT_RSReportQC] '" + sSql + "'," + nReportType);
        }

        public static IDataReader GetsQCByRaqLot(TransactionContext tc, int nRawLotID, int nReportType)
        {
            return tc.ExecuteReader("EXEC [SP_RPT_RSReportQCBYRawLot] " + nRawLotID + "," + nReportType);
        }

        #endregion
    }
}