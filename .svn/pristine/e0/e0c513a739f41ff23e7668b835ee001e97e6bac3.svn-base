using System;
using System.Data;
using System.Data.SqlClient;
using ICS.Core.DataAccess;
using ESimSol.BusinessObjects;
using System.Data.OleDb;
using ICS.Core.Utility;

namespace ESimSol.Services.DataAccess
{
    public class FNQCParameterDA
    {
        public FNQCParameterDA() { }

        #region Insert Update Delete Function
        public static IDataReader InsertUpdate(TransactionContext tc, FNQCParameter oFNQCParameter, EnumDBOperation eEnumDBOperation, Int64 nUserID)
        {
            return tc.ExecuteReader("EXEC [SP_IUD_FNQCParameter]"
                                    + "%n, %s, %n, %n, %n, %n",
                                    oFNQCParameter.FNQCParameterID, oFNQCParameter.Name, oFNQCParameter.Code, oFNQCParameter.FnQCTestGroupID, (int)eEnumDBOperation, nUserID);
        }

        public static void Delete(TransactionContext tc, FNQCParameter oFNQCParameter, EnumDBOperation eEnumDBOperation, Int64 nUserID)
        {
            tc.ExecuteNonQuery("EXEC [SP_IUD_FNQCParameter]"
                                    + "%n, %s, %n, %n, %n, %n",
                                    oFNQCParameter.FNQCParameterID, oFNQCParameter.Name, oFNQCParameter.Code, oFNQCParameter.FnQCTestGroupID, (int)eEnumDBOperation, nUserID);
        }
        #endregion

        #region Get & Exist Function
        public static IDataReader Get(TransactionContext tc, long nID)
        {
            return tc.ExecuteReader("SELECT * FROM View_FNQCParameter WHERE FNQCParameterID=%n", nID);
        }
        public static IDataReader Gets(TransactionContext tc)
        {
            return tc.ExecuteReader("SELECT * FROM View_FNQCParameter Order By Name");
        }
        public static IDataReader Gets(TransactionContext tc, string sSQL)
        {
            return tc.ExecuteReader(sSQL);
        }
        #endregion
    }  
}

