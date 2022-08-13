using System;
using System.Data;
using System.Data.SqlClient;
using ICS.Core;
using ESimSol.BusinessObjects;
using System.Data.OleDb;
using ICS.Core.DataAccess;
using ICS.Core.Utility;


namespace ESimSol.Services.DataAccess
{
    public class RMClosingStockDA
    {
        public RMClosingStockDA() { }

        #region Insert Update Delete Function
        public static IDataReader InsertUpdate(TransactionContext tc, RMClosingStock oRMClosingStock, EnumDBOperation eEnumDBOperation, Int64 nUserId)
        {
            return tc.ExecuteReader("EXEC [SP_IUD_RMClosingStock]"
                                    + "%n,%s,%n,%n,%n,%d,%n, %s,%n,%n",
                                    oRMClosingStock.RMClosingStockID, oRMClosingStock.SLNo, oRMClosingStock.BUID, oRMClosingStock.AccountingSessionID,  oRMClosingStock.ClosingStockValue, oRMClosingStock.StockDate, oRMClosingStock.ApprovedBy, oRMClosingStock.Remarks, nUserId, (int)eEnumDBOperation);
        }

        public static void Delete(TransactionContext tc, RMClosingStock oRMClosingStock, EnumDBOperation eEnumDBOperation, Int64 nUserId)
        {
            tc.ExecuteNonQuery("EXEC [SP_IUD_RMClosingStock]"
                                    + "%n,%s,%n,%n,%n,%d,%n, %s,%n,%n",
                                    oRMClosingStock.RMClosingStockID, oRMClosingStock.SLNo, oRMClosingStock.BUID, oRMClosingStock.AccountingSessionID, oRMClosingStock.ClosingStockValue, oRMClosingStock.StockDate, oRMClosingStock.ApprovedBy, oRMClosingStock.Remarks, nUserId, (int)eEnumDBOperation);
        }

        public static void Approve(TransactionContext tc, RMClosingStock oRMClosingStock, long nID)
        {
            tc.ExecuteNonQuery("UPDATE RMClosingStock SET ApprovedBy =" + nID + " WHERE RMClosingStockID=%n", oRMClosingStock.RMClosingStockID);
        }

        public static void UndoApprove(TransactionContext tc, RMClosingStock oRMClosingStock, long nID)
        {
            tc.ExecuteNonQuery("UPDATE RMClosingStock SET ApprovedBy = 0 WHERE RMClosingStockID=%n", oRMClosingStock.RMClosingStockID);
        }
        #endregion

        #region Get & Exist Function
        public static IDataReader Get(TransactionContext tc, long nID)
        {
            return tc.ExecuteReader("SELECT * FROM View_RMClosingStock WHERE RMClosingStockID=%n", nID);
        }
        public static IDataReader Gets(TransactionContext tc)
        {
            return tc.ExecuteReader("SELECT * FROM View_RMClosingStock");
        }

        public static IDataReader GetsByName(TransactionContext tc, string sName)
        {

            return tc.ExecuteReader("SELECT * FROM View_RMClosingStock WHERE (ISNULL(AccoutingSessionName,'')+ISNULL(BUName,'')+ISNULL(RMAccountHeadName,'')) LIKE ('%" + sName + "%')");
        }

        public static IDataReader Gets(TransactionContext tc, string sSQL)
        {
            return tc.ExecuteReader(sSQL);
        }
        #endregion
    }
}
