using System;
using ICS.Core.Framework;
using ICS.Core.Utility;
using ICS.Core;
using ESimSol.BusinessObjects;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Data.OleDb;
using ICS.Core.DataAccess;

namespace ESimSol.Services.DataAccess
{
    public class DUHardWindingReportDA
    {

        #region Get & Exist Function
        public static IDataReader Gets(TransactionContext tc, DUHardWindingReport oDUHardWindingReport)
        {
            return tc.ExecuteReader("EXEC [SP_RPT_DUHardWindingReport]"
                                   + "%n,%n,%D,%D",
                                   oDUHardWindingReport.BUID, oDUHardWindingReport.ReportLayout, oDUHardWindingReport.StartDate, oDUHardWindingReport.EndDate);
        }

        #endregion
    }

}
