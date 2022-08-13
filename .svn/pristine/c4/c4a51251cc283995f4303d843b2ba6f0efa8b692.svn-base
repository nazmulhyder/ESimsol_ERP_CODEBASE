using System;
using System.Data;
using System.Data.SqlClient;
using ESimSol.BusinessObjects;
using System.Data.OleDb;
using ICS.Core.DataAccess;
using ICS.Core.Utility;

namespace ESimSol.Services.DataAccess
{
    public class SP_RatioSetupDA
    {
        public SP_RatioSetupDA() { }

        

        #region Get & Exist Function
      
        public static IDataReader Gets(TransactionContext tc, int nAccountingRatioSetupID, DateTime dStartDate, DateTime dEndDate, int nBusinessUnitID)
        {
            return tc.ExecuteReader("EXEC [dbo].[SP_RatioSetup] %n,%d,%d,%n", nAccountingRatioSetupID, dStartDate, dEndDate, nBusinessUnitID);
        }
       
        #endregion
    }  
}
