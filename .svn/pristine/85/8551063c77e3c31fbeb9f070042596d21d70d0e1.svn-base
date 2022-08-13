using System;
using System.Data;
using System.Data.SqlClient;
using ICS.Core.DataAccess;
using ESimSol.BusinessObjects;
using System.Data.OleDb;
using ICS.Core.Utility;

namespace ESimSol.Services.DataAccess
{
    public class FabricSpecificationNoteDA
    {
        public FabricSpecificationNoteDA() { }

        #region Insert Update Delete Function
        public static IDataReader InsertUpdate(TransactionContext tc, FabricSpecificationNote oFabricSpecificationNote, EnumDBOperation eEnumDBOperation, Int64 nUserID)
        {
            return tc.ExecuteReader("EXEC [SP_IUD_FabricSpecificationNote]"
                                    + "%n, %n, %s, %n, %n",
                                    oFabricSpecificationNote.FabricSpecificationNoteID, oFabricSpecificationNote.FEOSID, oFabricSpecificationNote.Note, (int)eEnumDBOperation, nUserID);
        }

        public static void Delete(TransactionContext tc, FabricSpecificationNote oFabricSpecificationNote, EnumDBOperation eEnumDBOperation, Int64 nUserID)
        {
            tc.ExecuteNonQuery("EXEC [SP_IUD_FabricSpecificationNote]"
                                      + "%n, %n, %s, %n, %n",
                                    oFabricSpecificationNote.FabricSpecificationNoteID, oFabricSpecificationNote.FEOSID, oFabricSpecificationNote.Note, (int)eEnumDBOperation, nUserID);
        }
        public static void DeleteAll(TransactionContext tc, int nFEOSID, string sIDs)
        {
            tc.ExecuteNonQuery("Delete from FabricSpecificationNote where FEOSID>0 and FEOSID=%n and FabricSpecificationNoteID not in (%q)", nFEOSID, sIDs);
        }
        #endregion

        #region Get & Exist Function
        public static IDataReader Get(TransactionContext tc, long nID)
        {
            return tc.ExecuteReader("SELECT * FROM FabricSpecificationNote WHERE FabricSpecificationNoteID=%n", nID);
        }
        public static IDataReader Gets(TransactionContext tc)
        {
            return tc.ExecuteReader("SELECT * FROM FabricSpecificationNote");
        }
        public static IDataReader Gets(TransactionContext tc, int nFEOSID)
        {
            return tc.ExecuteReader("SELECT * FROM FabricSpecificationNote WHERE FEOSID=%n", nFEOSID);
        }
        #endregion
    }
}

