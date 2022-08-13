using System;
using System.Data;
using System.Data.SqlClient;
using ESimSol.BusinessObjects;
using System.Data.OleDb;
using ICS.Core.DataAccess;
using ICS.Core.Utility;

namespace ESimSol.Services.DataAccess
{
    public class OperationUnitDA
    {
        public OperationUnitDA() { }

        #region Insert Update Delete Function
        public static IDataReader InsertUpdate(TransactionContext tc, OperationUnit oOperationUnit, EnumDBOperation eEnumDBOperation, Int64 nUserId)
        {
            return tc.ExecuteReader("EXEC [SP_IUD_OperationUnit]"
                                    + "%n, %s, %s, %b, %s, %n, %n",
                                    oOperationUnit.OperationUnitID, oOperationUnit.OperationUnitName, oOperationUnit.Description, oOperationUnit.IsStore, oOperationUnit.ShortName, nUserId, (int)eEnumDBOperation);
        }

        public static void Delete(TransactionContext tc, OperationUnit oOperationUnit, EnumDBOperation eEnumDBOperation, Int64 nUserId)
        {
            tc.ExecuteNonQuery("EXEC [SP_IUD_OperationUnit]"
                                    +"%n, %s, %s, %b, %s, %n, %n",
                                    oOperationUnit.OperationUnitID, oOperationUnit.OperationUnitName, oOperationUnit.Description, oOperationUnit.IsStore, oOperationUnit.ShortName, nUserId, (int)eEnumDBOperation);
        }
        #endregion

        #region Get & Exist Function
        public static IDataReader Get(TransactionContext tc, long nID)
        {
            return tc.ExecuteReader("SELECT * FROM OperationUnit WHERE OperationUnitID=%n", nID);
        }
        public static IDataReader Gets(TransactionContext tc)
        {
            return tc.ExecuteReader("SELECT * FROM OperationUnit");
        }
        public static IDataReader Gets(TransactionContext tc, string sSQL)
        {
            return tc.ExecuteReader(sSQL);
        }
        #endregion
    }
}
