using System;
using System.Data;
using System.Data.SqlClient;
using ESimSol.BusinessObjects;
using System.Data.OleDb;
using ICS.Core.DataAccess;
using ICS.Core.Utility;

namespace ESimSol.Services.DataAccess
{

    public class NOADA
    {
        public NOADA() { }

        #region Insert Update Delete Function
        public static IDataReader InsertUpdate(TransactionContext tc, NOA oNOA, EnumDBOperation eEnumDBOperation, Int64 nUserId)
        {
            return tc.ExecuteReader("EXEC[SP_IUD_NOA]"
                                    + "%n, %s, %d,  %n, %n,  %s, %n, %n, %n",
                                    oNOA.NOAID, oNOA.NOANo, oNOA.NOADate,oNOA.BUID,  oNOA.PrepareBy, oNOA.Note, oNOA.ApproveBy, nUserId, (int)eEnumDBOperation);
        }

        public static void Delete(TransactionContext tc, NOA oNOA, EnumDBOperation eEnumDBOperation, Int64 nUserId)
        {
            tc.ExecuteNonQuery("EXEC[SP_IUD_NOA]"
                                    + "%n, %s, %d,  %n, %n,  %s, %n, %n, %n",
                                    oNOA.NOAID, oNOA.NOANo, oNOA.NOADate, oNOA.BUID, oNOA.PrepareBy, oNOA.Note, oNOA.ApproveBy, nUserId, (int)eEnumDBOperation);
        }
        #endregion

        #region Undo Approve
        public static void UndoApprove(TransactionContext tc, int NOAID)
        {
            tc.ExecuteNonQuery("Update NOA SET ApproveBy = 0 , ApproveDate = null WHERE NOAID = %n", NOAID);
        }
        #endregion

        public static IDataReader RequestNOARevise(TransactionContext tc, NOA oNOA, Int64 nUserID)
        {
            return tc.ExecuteReader("EXEC[SP_NOARevise]" + "%n,%n",
                                    oNOA.NOAID
                                    , nUserID);
        }
        #region Get & Exist Function
        public static IDataReader Get(TransactionContext tc, long nId)
        {
            return tc.ExecuteReader("SELECT * FROM View_NOA WHERE NOAID=%n", nId);
        }
        public static IDataReader GetByLog(TransactionContext tc, long nId)
        {
            return tc.ExecuteReader("SELECT * FROM View_NOALog WHERE NOALogID=%n", nId);
        }

        public static IDataReader GetsWaitForApproval(TransactionContext tc)
        {
            return tc.ExecuteReader("SELECT * FROM View_NOA WHERE ISNULL(ApproveBy,0)=0 ");
        }
        public static IDataReader Gets(TransactionContext tc, string sSQL)
        {
            return tc.ExecuteReader(sSQL);
        }

        #endregion
    }  
    
    
}
