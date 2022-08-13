using System;
using System.Data;
using System.Data.SqlClient;
using ICS.Core.DataAccess;
using ESimSol.BusinessObjects;
using System.Data.OleDb;
using ICS.Core.Utility;

namespace ESimSol.Services.DataAccess
{
    public class BatchProcessPlannedDateDA
    {
        public BatchProcessPlannedDateDA() { }

        #region Insert Update Delete Function
        public static IDataReader InsertUpdate(TransactionContext tc, BatchProcessPlannedDate oBPPD, EnumDBOperation eEnumDBOperation, Int64 nUserID)
        {
            return tc.ExecuteReader("EXEC [SP_IUD_BatchProcessPlannedDate]"
                                    + "%n, %n, %n, %n, %d, %n, %n",
                                    oBPPD.BatchProcessPlannedDateID, oBPPD.FNBatchCardID, oBPPD.FNTreatmentProcessID, oBPPD.FNBatchID, oBPPD.PlannedDate, (int)eEnumDBOperation, nUserID);
        }

        public static void Delete(TransactionContext tc, BatchProcessPlannedDate oBPPD, EnumDBOperation eEnumDBOperation, Int64 nUserID)
        {
            tc.ExecuteNonQuery("EXEC [SP_IUD_BatchProcessPlannedDate]"
                                     + "%n, %n, %n, %n, %d, %n, %n",
                                    oBPPD.BatchProcessPlannedDateID, oBPPD.FNBatchCardID, oBPPD.FNTreatmentProcessID, oBPPD.FNBatchID, oBPPD.PlannedDate, (int)eEnumDBOperation, nUserID);
        }
        #endregion

        #region Get & Exist Function
        public static IDataReader Get(TransactionContext tc, long nID)
        {
            return tc.ExecuteReader("SELECT * FROM View_BatchProcessPlannedDate WHERE BatchProcessPlannedDateID=%n", nID);
        }

        public static IDataReader Gets(TransactionContext tc)
        {
            return tc.ExecuteReader("SELECT * FROM View_BatchProcessPlannedDate");
        }
        public static IDataReader Gets(TransactionContext tc, string sSQL)
        {
            return tc.ExecuteReader(sSQL);//use View_BatchProcessPlannedDate
        }
        #endregion
    }  
}

