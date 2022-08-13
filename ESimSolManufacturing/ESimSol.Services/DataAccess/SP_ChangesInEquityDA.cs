using System;
using System.Data;
using System.Data.SqlClient;
using ESimSol.BusinessObjects;
using System.Data.OleDb;
using ICS.Core.DataAccess;
using ICS.Core.Utility;

namespace ESimSol.Services.DataAccess
{
    public class SP_ChangesInEquityDA
    {
        public SP_ChangesInEquityDA() { }

        

        #region Get & Exist Function

        public static IDataReader Gets(TransactionContext tc, int nAccountingSessionID, int nBusinessUnitID)
        {
            return tc.ExecuteReader("EXEC [dbo].[SP_ChangesInEquity] %n,%n", nAccountingSessionID, nBusinessUnitID);
        }
       
        #endregion
    }  
}
