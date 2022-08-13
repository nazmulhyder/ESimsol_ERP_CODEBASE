using System;
using System.Data;
using System.Data.SqlClient;
using ICS.Core.DataAccess;
using ESimSol.BusinessObjects;
using System.Data.OleDb;
using ICS.Core.Utility;

namespace ESimSol.Services.DataAccess
{
    public class FNQCResultDA
    {
        public FNQCResultDA() { }

        #region Insert Update Delete Function
        public static IDataReader InsertUpdate(TransactionContext tc, FNQCResult oFNQCResult, EnumDBOperation eEnumDBOperation, Int64 nUserID)
        {
            return tc.ExecuteReader("EXEC [SP_IUD_FNQCResult]"
                                    + "%n, %n, %n, %n, %s, %s, %s, %s, %s, %n, %n",
                                    oFNQCResult.FNQCResultID,     oFNQCResult.FNQCParameterID,   oFNQCResult.FNTPID,    oFNQCResult.FNPBatchID,   oFNQCResult.SubName,
                                    oFNQCResult.Value,            oFNQCResult.ValueResult,     oFNQCResult.Note,        oFNQCResult.TestMethod,      (int)eEnumDBOperation,      nUserID);
        }
        public static void Delete(TransactionContext tc, FNQCResult oFNQCResult, EnumDBOperation eEnumDBOperation, Int64 nUserID)
        {
            tc.ExecuteNonQuery("EXEC [SP_IUD_FNQCResult]"
                                   + "%n, %n, %n, %n, %s, %s, %s, %s, %s, %n, %n",
                                    oFNQCResult.FNQCResultID, oFNQCResult.FNQCParameterID, oFNQCResult.FNTPID, oFNQCResult.FNPBatchID,           oFNQCResult.SubName,
                                    oFNQCResult.Value, oFNQCResult.ValueResult, oFNQCResult.Note, oFNQCResult.TestMethod, (int)eEnumDBOperation, nUserID);
        }
        #endregion

        #region Get & Exist Function
        public static IDataReader Get(TransactionContext tc, long nID)
        {
            return tc.ExecuteReader("SELECT * FROM View_FNQCResult WHERE FNQCResultID=%n", nID);
        }
        public static IDataReader Gets(TransactionContext tc)
        {
            return tc.ExecuteReader("SELECT * FROM View_FNQCResult Order By [Name]");
        }
        public static IDataReader Gets(TransactionContext tc, string sSQL)
        {
            return tc.ExecuteReader(sSQL);
        }
        #endregion
    }  
}



