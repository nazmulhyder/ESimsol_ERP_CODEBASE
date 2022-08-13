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
    public class DUDeliverySummaryRPTDA
    {

        #region Get & Exist Function
        public static IDataReader GetsData(TransactionContext tc, DUDeliverySummaryRPT oDUDeliverySummaryRPT)
        {
            return tc.ExecuteReader("EXEC [SP_RPT_DUDeliverySummary]"
                                    + "%n, %n, %D, %D, %s",
                                    oDUDeliverySummaryRPT.BUID, oDUDeliverySummaryRPT.ReportLayout, oDUDeliverySummaryRPT.StartDate, oDUDeliverySummaryRPT.EndDate, oDUDeliverySummaryRPT.OrderTypeSt);
        }

        #endregion
    }

}
