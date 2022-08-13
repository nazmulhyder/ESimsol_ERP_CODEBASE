using System;
using System.Data;
using System.Data.SqlClient;
using ICS.Core.DataAccess;
using ESimSol.BusinessObjects;
using System.Data.OleDb;
using ICS.Core.Utility;
namespace ESimSol.Services.DataAccess
{
    public class DUProductionStatusDA
    {
        public DUProductionStatusDA() { }
        public static IDataReader GetsDUProductionStatus(TransactionContext tc, int BUID, int nLayout, DateTime StartDate, DateTime EndDate, string sSQL, EnumRSState nRSState)
        {
            return tc.ExecuteReader("EXEC [SP_RPT_DUProductionStatus] " + " %n, %n, %d, %d,%s,%n", BUID, nLayout, StartDate, EndDate, sSQL, (int)nRSState);
        }
    }
}
