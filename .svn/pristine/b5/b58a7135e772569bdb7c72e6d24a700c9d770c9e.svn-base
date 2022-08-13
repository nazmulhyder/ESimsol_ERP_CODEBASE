using System;
using System.Data;
using System.Data.SqlClient;
using ICS.Core;
using ESimSol.BusinessObjects;
using System.Data.OleDb;
using ICS.Core.Framework;
using ICS.Core.Utility;
using ICS.Core.DataAccess;
 
namespace ESimSol.Services.DataAccess
{

    public class MasterLCHistoryDA
    {
        public MasterLCHistoryDA() { }

        #region Insert Update Delete Function
        public static IDataReader InsertUpdate(TransactionContext tc, MasterLCHistory oMasterLCHistory, EnumDBOperation eEnumDBOperation, Int64 nUserID, String sMasterLCHistoryIDs)
        {
            return tc.ExecuteReader("EXEC [SP_IUD_MasterLCHistory]"
                                    + "%n,%n,%n,%n,%n,%n,%n,%n,%n,%n,%s", oMasterLCHistory.MasterLCHistoryID, nUserID, (int)eEnumDBOperation, "");
        }

        public static void Delete(TransactionContext tc, MasterLCHistory oMasterLCHistory, EnumDBOperation eEnumDBOperation, Int64 nUserID, String sMasterLCHistoryIDs)
        {
            tc.ExecuteNonQuery("EXEC [SP_IUD_MasterLCHistory]"
                                    + "%n,%n,%n,%n,%n,%n,%n,%n,%n,%n,%s", oMasterLCHistory.MasterLCHistoryID, nUserID, (int)eEnumDBOperation, sMasterLCHistoryIDs);
        }

        #endregion

        #region Get & Exist Function
        public static IDataReader Get(TransactionContext tc, long nID)
        {
            return tc.ExecuteReader("SELECT * FROM View_MasterLCHistory WHERE MasterLCHistoryID=%n", nID);
        }

        public static IDataReader Gets(int MasterLCID, TransactionContext tc)
        {
            return tc.ExecuteReader("SELECT * FROM View_MasterLCHistory where MasterLCID =%n", MasterLCID);
        }
        public static IDataReader Gets(TransactionContext tc, string sSQL)
        {
            return tc.ExecuteReader(sSQL);
        }
        #endregion
    }
    
  
}
