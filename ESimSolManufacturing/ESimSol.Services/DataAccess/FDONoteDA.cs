using System;
using System.Data;
using System.Data.SqlClient;
using ICS.Core.DataAccess;
using ESimSol.BusinessObjects;
using System.Data.OleDb;
using ICS.Core.Utility;

namespace ESimSol.Services.DataAccess
{
    public class FDONoteDA
    {
        #region Insert Update Delete Function
        public static IDataReader InsertUpdate(TransactionContext tc, FDONote oFDONote, EnumDBOperation eEnumDBOperation, Int64 nUserID)
        {
            return tc.ExecuteReader("EXEC [SP_IUD_FDONote]"
                                    + "%n, %n, %s, %n,%n",
                                    oFDONote.FDONoteID, oFDONote.FDOID, oFDONote.Note, nUserID, (int)eEnumDBOperation);
        }

        public static void Delete(TransactionContext tc, FDONote oFDONote, EnumDBOperation eEnumDBOperation, Int64 nUserID)
        {
            tc.ExecuteNonQuery("EXEC [SP_IUD_FDONote]"
                                    + "%n, %n, %s, %n,%n",
                                    oFDONote.FDONoteID, oFDONote.FDOID, oFDONote.Note, nUserID, (int)eEnumDBOperation);
        }
        #endregion

        #region Get & Exist Function
        public static IDataReader GetByOrderID(TransactionContext tc, int nFDOID)
        {
            return tc.ExecuteReader("SELECT * FROM FDONote WHERE FDOID=%n", nFDOID);
        }
   
        public static IDataReader Gets(TransactionContext tc, string sSQL)
        {
            return tc.ExecuteReader(sSQL);
        }
        #endregion
    }
}