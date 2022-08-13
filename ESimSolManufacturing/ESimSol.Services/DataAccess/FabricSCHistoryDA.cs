using System;
using System;
using System.Data;
using System.Data.SqlClient;
using ICS.Core.DataAccess;
using ESimSol.BusinessObjects;
using System.Data.OleDb;
using ICS.Core.Utility;

namespace ESimSol.Services.DataAccess
{
    public class FabricSCHistoryDA
    {
        #region Insert Update Delete Function
        public static IDataReader InsertUpdate(TransactionContext tc, FabricSCHistory oFabricSCHistory, EnumDBOperation eEnumDBOperation, int nUserID)
        {
            return tc.ExecuteReader("EXEC [SP_IUD_FabricSCHistory]"
                                    + "%n, %n,%n, %n,%n, %n,%n, %s, %n, %n",
                                     oFabricSCHistory.FabricSCHistoryID, oFabricSCHistory.FabricSCID, oFabricSCHistory.FabricSCDetailID, (int)oFabricSCHistory.FSCStatus, (int)oFabricSCHistory.FSCDStatus, (int)oFabricSCHistory.FSCStatus_Prv, (int)oFabricSCHistory.FSCDStatus_Prv, oFabricSCHistory.Note, nUserID, (int)eEnumDBOperation);
        }

        public static void Delete(TransactionContext tc, FabricSCHistory oFabricSCHistory, EnumDBOperation eEnumDBOperation, int nUserID)
        {
            tc.ExecuteNonQuery("EXEC [SP_IUD_FabricSCHistory]"
                                    + "%n, %n,%n, %n,%n, %n,%n, %s, %n, %n",
                                     oFabricSCHistory.FabricSCHistoryID, oFabricSCHistory.FabricSCID, oFabricSCHistory.FabricSCDetailID, oFabricSCHistory.FSCStatusInt, oFabricSCHistory.FSCDStatusInt, oFabricSCHistory.FSCStatus_PrvInt, oFabricSCHistory.FSCDStatus_PrvInt, oFabricSCHistory.Note, nUserID, (int)eEnumDBOperation);
        }
        #endregion

        #region Get & Exist Function
        public static IDataReader Gets(TransactionContext tc, int nFSCID)
        {
            return tc.ExecuteReader("SELECT * FROM View_FabricSCHistory where FabricSCID=%n", nFSCID);
        }
        public static IDataReader Get(TransactionContext tc, int nFabricSCHistoryID)
        {
            return tc.ExecuteReader("SELECT * FROM View_FabricSCHistory WHERE FabricSCHistoryID=%n", nFabricSCHistoryID);
        }

        public static IDataReader Gets(TransactionContext tc, string sSQL)
        {
            return tc.ExecuteReader(sSQL);
        }
        #endregion
    }
}