using System;
using System.Data;
using System.Data.SqlClient;
using ESimSol.BusinessObjects;
using System.Data.OleDb;
using ICS.Core.DataAccess;
using ICS.Core.Utility;

namespace ESimSol.Services.DataAccess
{

    public class ITransactionGRNDA
    {
        public ITransactionGRNDA() { }

        #region Get & Exist Function
        public static IDataReader Gets(TransactionContext tc, DateTime dStartDate, DateTime dEndDate, int nBUID, int nGRNType, int nImportProductID)
        {
            return tc.ExecuteReader("EXEC [SP_RPT_ITransactionGRN]" + " %d,%d,%n,%n,%n", dStartDate, dEndDate.AddDays(1), nBUID, nGRNType, nImportProductID);
        }
        #endregion
    }


}
