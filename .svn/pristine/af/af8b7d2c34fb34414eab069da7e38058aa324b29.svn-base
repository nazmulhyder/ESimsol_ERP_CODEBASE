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
    public class DevelopmentRecapHistoryDA
    {
        public DevelopmentRecapHistoryDA() { }

        #region Insert Update Delete Function
        public static IDataReader InsertUpdate(TransactionContext tc, DevelopmentRecapHistory oDevelopmentRecapHistory, EnumDBOperation eEnumDBOperation, Int64 nUserID)
        {
            return tc.ExecuteReader("EXEC [SP_IUD_DevelopmentRecapHistory]"
                                    + "%n, %n, %n, %n, %d, %s, %n, %n",
                                    oDevelopmentRecapHistory.DevelopmentRecapHistoryID, oDevelopmentRecapHistory.DevelopmentRecapID, (int)oDevelopmentRecapHistory.CurrentStatus, oDevelopmentRecapHistory.PreviousStatus, oDevelopmentRecapHistory.OperationDate, oDevelopmentRecapHistory.Note, nUserID, (int)eEnumDBOperation);
        }

        public static void Delete(TransactionContext tc, DevelopmentRecapHistory oDevelopmentRecapHistory, EnumDBOperation eEnumDBOperation, Int64 nUserID)
        {
            tc.ExecuteNonQuery("EXEC [SP_IUD_DevelopmentRecapHistory]"
                                    + "%n, %n, %n, %n, %d, %s, %n, %n",
                                    oDevelopmentRecapHistory.DevelopmentRecapHistoryID, oDevelopmentRecapHistory.DevelopmentRecapID, (int)oDevelopmentRecapHistory.CurrentStatus, oDevelopmentRecapHistory.PreviousStatus, oDevelopmentRecapHistory.OperationDate, oDevelopmentRecapHistory.Note, nUserID, (int)eEnumDBOperation);
        }
        #endregion

        #region Get & Exist Function
        public static IDataReader Get(TransactionContext tc, long nID)
        {
            return tc.ExecuteReader("SELECT * FROM DevelopmentRecapHistory WHERE DevelopmentRecapHistoryID=%n", nID);
        }

        public static IDataReader Gets(TransactionContext tc)
        {
            return tc.ExecuteReader("SELECT * FROM DevelopmentRecapHistory");
        }
        public static IDataReader Gets(TransactionContext tc, string sSQL)
        {
            return tc.ExecuteReader(sSQL);
        }

        public static IDataReader GetsDevelopmentRecapHistotry(TransactionContext tc, long nDevelopmentRecapID, int nCurrentStatus)
        {
            return tc.ExecuteReader("select * from View_DevelopmentRecapHistory where DevelopmentRecapID=%n and CurrentStatus=%n",nDevelopmentRecapID, nCurrentStatus);
        }
        #endregion
    }
}
