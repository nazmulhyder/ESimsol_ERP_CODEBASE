using System;
using System.Data;
using System.Data.SqlClient;
using ESimSol.BusinessObjects;
using System.Data.OleDb;
using ICS.Core.DataAccess;
using ICS.Core.Utility;

namespace ESimSol.Services.DataAccess
{

    public class FabricSalesContractNoteDA
    {
        public FabricSalesContractNoteDA() { }

        #region Insert Update Delete Function
        public static IDataReader InsertUpdate(TransactionContext tc, FabricSalesContractNote oFabricSalesContractNote, EnumDBOperation eEnumDBOperation, Int64 nUserID, string sFabricSalesContractNoteID)
        {
            return tc.ExecuteReader("EXEC [SP_IUD_FabricSalesContractNote]" + "%n, %n, %s, %n, %s, %n, %n", 
                oFabricSalesContractNote.FabricSalesContractNoteID, oFabricSalesContractNote.FabricSalesContractID, oFabricSalesContractNote.Note, (int)oFabricSalesContractNote.Sequence, sFabricSalesContractNoteID, nUserID, (int)eEnumDBOperation);
        }
        public static void InsertUpdateTwo(TransactionContext tc, FabricSalesContractNote oFabricSalesContractNote, EnumDBOperation eEnumDBOperation, Int64 nUserID, string sFabricSalesContractNoteID)
        {
             tc.ExecuteNonQuery("EXEC [SP_IUD_FabricSalesContractNote]" + "%n, %n, %s, %n, %s, %n, %n",
                oFabricSalesContractNote.FabricSalesContractNoteID, oFabricSalesContractNote.FabricSalesContractID, oFabricSalesContractNote.Note, (int)oFabricSalesContractNote.Sequence, sFabricSalesContractNoteID, nUserID, (int)eEnumDBOperation);
        }
        public static void Delete(TransactionContext tc, FabricSalesContractNote oFabricSalesContractNote, EnumDBOperation eEnumDBOperation, Int64 nUserID, string sFabricSalesContractNoteID)
        {
            tc.ExecuteNonQuery("EXEC [SP_IUD_FabricSalesContractNote]" + "%n, %n, %s, %n, %s, %n, %n",
                  oFabricSalesContractNote.FabricSalesContractNoteID, oFabricSalesContractNote.FabricSalesContractID, oFabricSalesContractNote.Note, (int)oFabricSalesContractNote.Sequence, sFabricSalesContractNoteID, nUserID, (int)eEnumDBOperation);
        }
        public static void DeleteAll(TransactionContext tc, int nFabricSalesContractID,string sIDs)
        {
            tc.ExecuteNonQuery("Delete from FabricSalesContractNote where isnull(FabricSalesContractID,0)>0 and FabricSalesContractID=%n and FabricSalesContractNoteID in (%q)", nFabricSalesContractID, sIDs);
        }
        #endregion

        #region Get & Exist Function
        public static IDataReader Get(TransactionContext tc, long nID)
        {
            return tc.ExecuteReader("SELECT * FROM FabricSalesContractNote WHERE FabricSalesContractNoteID=%n", nID);
        }

        public static IDataReader Gets(TransactionContext tc, int nFSCID)
        {
            return tc.ExecuteReader("SELECT * FROM FabricSalesContractNote WHERE FabricSalesContractID = %n", nFSCID);
        }
        public static IDataReader GetsLog(TransactionContext tc, int nFSCID)
        {
            return tc.ExecuteReader("SELECT * FROM FabricSalesContractNoteLog WHERE FabricSalesContractLogID = %n", nFSCID);
        }
        public static IDataReader Gets(TransactionContext tc, string sSQL)
        {
            return tc.ExecuteReader(sSQL);
        }
        #endregion
    }


}
