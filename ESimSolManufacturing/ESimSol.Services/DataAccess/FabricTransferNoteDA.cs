using System;
using System.Data;
using System.Data.SqlClient;
using ICS.Core.DataAccess;
using ESimSol.BusinessObjects;
using System.Data.OleDb;
using ICS.Core.Utility;

namespace ESimSol.Services.DataAccess
{
    public class FabricTransferNoteDA
    {
        #region Insert Update Delete Function
        public static IDataReader InsertUpdate(TransactionContext tc, FabricTransferNote oFabricTransferNote, EnumDBOperation eEnumDBOperation, Int64 nUserID)
        {
            return tc.ExecuteReader("EXEC [SP_IUD_FabricTransferNote]"
                                    + "%n, %s, %s, %D, %n, %d, %n, %n, %s",
                                    oFabricTransferNote.FTNID, oFabricTransferNote.FTNNo, oFabricTransferNote.Note, oFabricTransferNote.NoteDate, oFabricTransferNote.DisburseBy, oFabricTransferNote.DisburseByDate, (int)eEnumDBOperation, nUserID, oFabricTransferNote.FTPListIDs);
        }

        public static void Delete(TransactionContext tc, FabricTransferNote oFabricTransferNote, EnumDBOperation eEnumDBOperation, Int64 nUserID)
        {
            tc.ExecuteNonQuery("EXEC [SP_IUD_FabricTransferNote]"
                                    + "%n, %s, %s, %D, %n, %d, %n, %n, %s",
                                    oFabricTransferNote.FTNID, oFabricTransferNote.FTNNo, oFabricTransferNote.Note, oFabricTransferNote.NoteDate, oFabricTransferNote.DisburseBy, oFabricTransferNote.DisburseByDate, (int)eEnumDBOperation, nUserID, oFabricTransferNote.FTPListIDs);
        }
        public static IDataReader Disburse(TransactionContext tc, FabricTransferNote oFabricTransferNote, Int64 nUserID)
        {
            return tc.ExecuteReader("EXEC [SP_DisburseFabricTransferNote]"
                                    + "%n, %s, %s, %D, %n, %d, %n",
                                    oFabricTransferNote.FTNID, oFabricTransferNote.FTNNo, oFabricTransferNote.Note, oFabricTransferNote.NoteDate, oFabricTransferNote.DisburseBy, oFabricTransferNote.DisburseByDate, nUserID);
        }
        public static IDataReader Receive(TransactionContext tc, FabricTransferNote oFabricTransferNote, Int64 nUserID)
        {
            return tc.ExecuteReader("EXEC [SP_ReceiveFabricTransferNote]"
                                    + "%n, %s, %b, %n, %n",
                                    oFabricTransferNote.FTNID, oFabricTransferNote.FTPListIDs, oFabricTransferNote.IsFTPLD, oFabricTransferNote.WUID, nUserID);
        }
        #endregion

        #region Get & Exist Function
        public static IDataReader Gets(TransactionContext tc)
        {
            return tc.ExecuteReader("SELECT * FROM View_FabricTransferNote WHERE DisburseBy=0");
        }
        public static IDataReader Gets(TransactionContext tc, string sSQL)
        {
            return tc.ExecuteReader(sSQL);
        }
        public static IDataReader Get(TransactionContext tc, long nFTNID)
        {
            return tc.ExecuteReader("SELECT * FROM View_FabricTransferNote WHERE FTNID=%n", nFTNID);
        }
        //public static void Disburse(TransactionContext tc, FabricTransferNote oFabricTransferNote, long nUserId)
        //{
        //    tc.ExecuteNonQuery("UPDATE FabricTransferNote SET DisburseBy=%n, DisburseByDate=%d WHERE FTNID=%n", nUserId, DateTime.Now, oFabricTransferNote.FTNID);
        //}
        #endregion
    }
}
