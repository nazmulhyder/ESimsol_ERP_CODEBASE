using System;
using System.Data;
using System.Data.SqlClient;
using ICS.Core.DataAccess;
using ESimSol.BusinessObjects;
using System.Data.OleDb;
using ICS.Core.Utility;

namespace ESimSol.Services.DataAccess
{
    public class StyleWiseStockDA
    {
        public StyleWiseStockDA() { }

        #region Get & Exist Function
        public static IDataReader Gets(TransactionContext tc, string sSQL, int nSelectedQty)
        {
            return tc.ExecuteReader("EXEC [SP_StyleWiseStock] %s,%n", sSQL, nSelectedQty);
        }
        #endregion
    }
}
