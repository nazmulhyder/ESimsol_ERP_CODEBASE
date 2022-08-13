using System;
using System.Data;
using System.Data.SqlClient;
using ICS.Core.DataAccess;
using ESimSol.BusinessObjects;
using System.Data.OleDb;
using ICS.Core.Utility;


namespace ESimSol.Services.DataAccess
{
    public class FDCRegisterDA
    {
        public FDCRegisterDA() { }

        #region Get & Exist Function
        public static IDataReader Gets(TransactionContext tc, string sSQL, Int64 nUserId)
        {
            return tc.ExecuteReader(sSQL);
        }
        public static IDataReader Gets_FDO(TransactionContext tc, string sSQL, Int64 nUserId)
        {
            return tc.ExecuteReader("EXEC [SP_RPT_FDO] %s, %n", sSQL, 0);
        }
        public static IDataReader Gets_FDC(TransactionContext tc, string sSQL, Int64 nUserId)
        {
            return tc.ExecuteReader("EXEC [SP_RPT_FDC] %s, %n", sSQL, 0);
        }
        public static IDataReader Gets_For_FDC2(TransactionContext tc, string sSQL, Int64 nUserId)
        {
            return tc.ExecuteReader("EXEC [SP_RPT_FDCTwo] %s, %n", sSQL, 0);
        }
        #endregion

    }
}

