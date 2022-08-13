using System;
using System.Data;
using System.Data.SqlClient;
using ICS.Core.DataAccess;
using ESimSol.BusinessObjects;
using System.Data.OleDb;
using ICS.Core.Utility;
namespace ESimSol.Services.DataAccess
{

    public class SupplierRateProcessDA
    {
        public SupplierRateProcessDA() { }



        #region Get & Exist Function

        public static IDataReader Gets(int nNOAID, TransactionContext tc)
        {
            return tc.ExecuteReader("EXEC [SP_SupplierRateProcess]" + "%n", nNOAID);
        }
        public static IDataReader GetsByLog(int nNOAID, TransactionContext tc)
        {
            return tc.ExecuteReader("EXEC [SP_SupplierRateProcessLog]" + "%n", nNOAID);
        }
        public static IDataReader GetsBy(int nNOAID, int nNOASignatoryID, TransactionContext tc)
        {
            return tc.ExecuteReader("EXEC [SP_SupplierRateProcess_Signatory]" + "%n,%n", nNOAID, nNOASignatoryID);
        }
        #endregion
    }
     
    

}
