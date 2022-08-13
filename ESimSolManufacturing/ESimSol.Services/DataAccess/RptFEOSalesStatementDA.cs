using System;
using System.Data;
using System.Data.SqlClient;
using ICS.Core.DataAccess;
using ESimSol.BusinessObjects.ReportingObject;
using System.Data.OleDb;
using ICS.Core.Utility;

namespace ESimSol.Services.DataAccess.ReportingDA
{
    public class RptFEOSalesStatementDA
    {
        public RptFEOSalesStatementDA() { }

        #region Get & Exist Function

        public static IDataReader Gets(TransactionContext tc,DateTime dtFrom, DateTime dtTo,int nExeType)
        {
            return tc.ExecuteReader("EXEC [SP_Rpt_FEOSalesStatement]" + " %d, %d, %n", dtFrom, dtTo, nExeType);
        }


        #endregion
    }
}

