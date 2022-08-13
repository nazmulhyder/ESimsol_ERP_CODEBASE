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
    public class WeavingYarnStockDA
    {
        #region Get & Exist Function
        public static IDataReader Get(TransactionContext tc, long nID)
        {
            return tc.ExecuteReader("SELECT * FROM View_WeavingYarnStock WHERE WeavingYarnStockID=%n", nID);
        }
        public static IDataReader Gets(TransactionContext tc)
        {
            return tc.ExecuteReader("SELECT * FROM View_WeavingYarnStock ");
        }
        public static IDataReader Gets(TransactionContext tc, string sSQL, int nType)
        {
            return tc.ExecuteReader("EXEC [SP_WeavingYarnStock]" + "%s,%n", sSQL, nType);
        }
        #endregion
    }

}
