using ESimSol.BusinessObjects;
using ICS.Core.DataAccess;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace ESimSol.Services.DataAccess
{
    class EmployeeActivityNoteDA
    {
        #region Insert Update Delete Function


        public static IDataReader InsertUpdate(TransactionContext tc, EmployeeActivityNote oEmployeeActivityNote, EnumDBOperation eEnumDBOperation, Int64 nUserId)
        {
            return tc.ExecuteReader("EXEC [SP_IUD_EmployeeActivityNote]" + "%n, %n, %n, %s, %d, %n, %n", oEmployeeActivityNote.EANID, oEmployeeActivityNote.EACID, oEmployeeActivityNote.EmployeeID, oEmployeeActivityNote.Note, oEmployeeActivityNote.ActivityDate
                                     ,nUserId, (int)eEnumDBOperation);
        }

        public static void Delete(TransactionContext tc, EmployeeActivityNote oEmployeeActivityNote, EnumDBOperation eEnumDBOperation, Int64 nUserId)
        {
            tc.ExecuteNonQuery("EXEC [SP_IUD_EmployeeActivityNote]" + "%n, %n, %n, %s, %d, %n, %n", oEmployeeActivityNote.EANID, oEmployeeActivityNote.EACID, oEmployeeActivityNote.EmployeeID, oEmployeeActivityNote.Note, oEmployeeActivityNote.ActivityDate
                                     , nUserId, (int)eEnumDBOperation);
        }
        #endregion


        public static IDataReader Gets(TransactionContext tc)
        {
            return tc.ExecuteReader("SELECT * FROM View_EmployeeActivityNote");
        }
        public static IDataReader Gets(string sSQL, TransactionContext tc)
        {
            return tc.ExecuteReader(sSQL);
        }

        public static IDataReader Get(TransactionContext tc, int id)
        {
            return tc.ExecuteReader("SELECT * FROM View_EmployeeActivityNote WHERE EANID=%n", id);
        }
    }
}
