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


    public class CostSheetHistoryDA
    {
        public CostSheetHistoryDA() { }

        #region Insert Update Delete Function
        public static IDataReader InsertUpdate(TransactionContext tc, CostSheetHistory oCostSheetHistory, EnumDBOperation eEnumDBOperation, Int64 nUserID, String sCostSheetHistoryIDs)
        {
            return tc.ExecuteReader("EXEC [SP_IUD_CostSheetHistory]"
                                    + "%n,%n,%n,%n,%n,%n,%n,%n,%n,%n,%s", oCostSheetHistory.CostSheetHistoryID, nUserID, (int)eEnumDBOperation, "");
        }

        public static void Delete(TransactionContext tc, CostSheetHistory oCostSheetHistory, EnumDBOperation eEnumDBOperation, Int64 nUserID, String sCostSheetHistoryIDs)
        {
            tc.ExecuteNonQuery("EXEC [SP_IUD_CostSheetHistory]"
                                    + "%n,%n,%n,%n,%n,%n,%n,%n,%n,%n,%s", oCostSheetHistory.CostSheetHistoryID, nUserID, (int)eEnumDBOperation, sCostSheetHistoryIDs);
        }

        #endregion

        #region Get & Exist Function
        public static IDataReader Get(TransactionContext tc, long nID)
        {
            return tc.ExecuteReader("SELECT * FROM View_CostSheetHistory WHERE CostSheetHistoryID=%n", nID);
        }

        public static IDataReader Gets(int CostSheetID, TransactionContext tc)
        {
            return tc.ExecuteReader("SELECT * FROM View_CostSheetHistory where CostSheetID =%n", CostSheetID);
        }
        public static IDataReader Gets(TransactionContext tc, string sSQL)
        {
            return tc.ExecuteReader(sSQL);
        }
        #endregion
    }
    

}
