using System;
using System.Data;
using System.Data.SqlClient;
using ESimSol.BusinessObjects;
using System.Data.OleDb;
using ICS.Core.DataAccess;
using ICS.Core.Utility;

namespace ESimSol.Services.DataAccess
{
    public class SalesProfitDA
    {
        public SalesProfitDA() { }

        #region Get & Exist Function
        public static IDataReader Gets(TransactionContext tc, int nOrderID, DateTime StartDate, DateTime EndDate)
        {
            return tc.ExecuteReader("EXEC [SP_SalesProfit]" + "%n,%d,%d", nOrderID, StartDate, EndDate);
        }

       
        #endregion
    }
    

}
