using System;
using System.Data;
using System.Data.SqlClient;
using ICS.Core.DataAccess;
using ESimSol.BusinessObjects;
using System.Data.OleDb;
using ICS.Core.Utility;
namespace ESimSol.Services.DataAccess
{

    public class ClaimReasonDA
    {
        public ClaimReasonDA() { }

        #region Insert Update Delete Function
        public static IDataReader InsertUpdate(TransactionContext tc, ClaimReason oClaimReason, EnumDBOperation eEnumDBOperation, Int64 nUserId)
        {
            return tc.ExecuteReader("EXEC[SP_IUD_ClaimReason]"
                                    + "%n,%n,%s ,%n,%s,%n, %n",
                                    oClaimReason.ClaimReasonID, oClaimReason.BUID, oClaimReason.Name, oClaimReason.OperationType, oClaimReason.Note, nUserId, (int)eEnumDBOperation);
        }
        public static void Delete(TransactionContext tc, ClaimReason oClaimReason, EnumDBOperation eEnumDBOperation, Int64 nUserId)
        {
            tc.ExecuteNonQuery("EXEC[SP_IUD_ClaimReason]"
                                   + "%n,%n,%s ,%n,%s,%n, %n",
                                    oClaimReason.ClaimReasonID, oClaimReason.BUID, oClaimReason.Name, oClaimReason.OperationType, oClaimReason.Note, nUserId, (int)eEnumDBOperation);
        }
        #endregion

        #region Get & Exist Function
        public static IDataReader Get(TransactionContext tc, long nID)
        {
            return tc.ExecuteReader("SELECT * FROM ClaimReason WHERE ClaimReasonID=%n", nID);
        }

        public static IDataReader GetByType(TransactionContext tc, int nOperationType)
        {
            return tc.ExecuteReader("SELECT * FROM ClaimReason WHERE OperationType=%n and Activity=1", nOperationType);
        }

        public static IDataReader Gets(TransactionContext tc)
        {
            return tc.ExecuteReader("SELECT * FROM ClaimReason");
        }
        public static IDataReader Gets(TransactionContext tc, int nBUID)
        {
            return tc.ExecuteReader("SELECT * FROM ClaimReason WHERE BUID=%n", nBUID);
        }
        public static IDataReader Gets(TransactionContext tc, string sSQL)
        {
            return tc.ExecuteReader(sSQL);
        }

        public static IDataReader Gets(string sSQL, TransactionContext tc)
        {
            return tc.ExecuteReader(sSQL);
        }
        public static IDataReader Activate(TransactionContext tc, ClaimReason oClaimReason)
        {
            string sSQL1 = SQLParser.MakeSQL("Update ClaimReason Set Activity=~Activity WHERE ClaimReasonID=%n", oClaimReason.ClaimReasonID);
            tc.ExecuteNonQuery(sSQL1);
            return tc.ExecuteReader("SELECT * FROM ClaimReason WHERE ClaimReasonID=%n", oClaimReason.ClaimReasonID);
        }
        #endregion
    }
}