using System;
using System.Data;
using System.Data.SqlClient;
using ICS.Core.DataAccess;
using ESimSol.BusinessObjects.ReportingObject;
using System.Data.OleDb;
using ICS.Core.Utility;

namespace ESimSol.Services.DataAccess.ReportingDA
{
    public class FabricFSStatusDA
    {
        public FabricFSStatusDA() { }

        #region Get & Exist Function
        public static IDataReader GetsFabricFSStatus(TransactionContext tc, int nRpeortType,string SQL)
        {
            return tc.ExecuteReader("EXEC [SP_RPT_FSCStatus]" + "%s,%n", SQL, nRpeortType);
        }


        #endregion
    }
}

