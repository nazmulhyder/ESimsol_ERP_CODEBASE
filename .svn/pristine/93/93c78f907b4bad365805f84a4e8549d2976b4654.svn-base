using System;
using System.Data;
using System.Data.SqlClient;
using ICS.Core;
using ESimSol.BusinessObjects;
using System.Data.OleDb;
using ICS.Core.Framework;
using ICS.Core.Utility;
using ICS.Core.DataAccess;


namespace ESimSol.Services.DataAccess
{
    public class ManagementDashboardDA
    {
        public static IDataReader Gets(TransactionContext tc, string sSQL, int BusinessSessionID)
        {
            return tc.ExecuteReader("EXEC[SP_ManagementDashboard]" + "%s,%n", sSQL, BusinessSessionID);
        }
    }
}
