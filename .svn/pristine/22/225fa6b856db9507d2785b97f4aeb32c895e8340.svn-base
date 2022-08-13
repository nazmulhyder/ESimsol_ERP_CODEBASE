using System;
using System.Data;
using System.Data.SqlClient;
using ESimSol.BusinessObjects;
using System.Data.OleDb;
using ICS.Core.DataAccess;
using ICS.Core.Utility;

namespace ESimSol.Services.DataAccess
{
    public class VoucherHistoryDA
    {
        public VoucherHistoryDA() { }

        #region Insert Update Delete Function
        public static IDataReader InsertUpdate(TransactionContext tc, VoucherHistory oVoucherHistory, EnumDBOperation eEnumDBOperation, int nUserID)
        {
            return tc.ExecuteReader("EXEC [SP_IUD_VoucherHistory]"
                                    + "%n, %n, %n, %d, %n, %s, %n, %n",
                                    oVoucherHistory.VoucherHistoryID, oVoucherHistory.VoucherID, oVoucherHistory.UserID, oVoucherHistory.TransactionDate, (int)oVoucherHistory.ActionType, oVoucherHistory.Remarks, nUserID, (int)eEnumDBOperation);
        }

        public static void Delete(TransactionContext tc, VoucherHistory oVoucherHistory, EnumDBOperation eEnumDBOperation, int nUserID)
        {
            tc.ExecuteNonQuery("EXEC [SP_IUD_VoucherHistory]"
                                    + "%n, %n, %n, %d, %n, %s, %n, %n",
                                    oVoucherHistory.VoucherHistoryID, oVoucherHistory.VoucherID, oVoucherHistory.UserID, oVoucherHistory.TransactionDate, (int)oVoucherHistory.ActionType, oVoucherHistory.Remarks, nUserID, (int)eEnumDBOperation);
        }
        #endregion

        #region Get & Exist Function
        public static IDataReader Get(TransactionContext tc, int nID)
        {
            return tc.ExecuteReader("SELECT * FROM View_VoucherHistory WHERE VoucherHistoryID=%n", nID);
        }


        public static IDataReader Gets(TransactionContext tc)
        {
            return tc.ExecuteReader("SELECT * FROM View_VoucherHistory");
        }
        
        public static IDataReader Gets(TransactionContext tc, string sSQL)
        {
            return tc.ExecuteReader(sSQL);//use View_VoucherHistory
        }

        public static IDataReader Gets(TransactionContext tc, VoucherHistory oVoucherHistory)
        {
            string sSQL = "";

            if (oVoucherHistory.UserID > 0)
            {
                if (oVoucherHistory.VoucherTypeID > 0)
                {
                    sSQL = "SELECT * FROM View_VoucherHistory AS VH WHERE VH.VoucherTypeID=" + oVoucherHistory.VoucherTypeID + " AND VH.UserID=" + oVoucherHistory.UserID + " AND CONVERT(DATE, CONVERT(VARCHAR(12),VH.TransactionDate,106)) BETWEEN  CONVERT(DATE, CONVERT(VARCHAR(12),'" + oVoucherHistory.StartDateSt + "',106)) AND CONVERT(DATE, CONVERT(VARCHAR(12),'" + oVoucherHistory.EndDateSt + "',106)) ORDER BY VH.ActionType";
                }
                else
                {
                    sSQL = "SELECT * FROM View_VoucherHistory AS VH WHERE VH.UserID=" + oVoucherHistory.UserID + " AND CONVERT(DATE, CONVERT(VARCHAR(12),VH.TransactionDate,106)) BETWEEN  CONVERT(DATE, CONVERT(VARCHAR(12),'" + oVoucherHistory.StartDateSt + "',106)) AND CONVERT(DATE, CONVERT(VARCHAR(12),'" + oVoucherHistory.EndDateSt + "',106)) ORDER BY VH.ActionType";
                }

            }
            else
            {
                if (oVoucherHistory.VoucherTypeID > 0)
                {
                    sSQL = "SELECT * FROM View_VoucherHistory AS VH WHERE VH.VoucherTypeID=" + oVoucherHistory.VoucherTypeID + " AND CONVERT(DATE, CONVERT(VARCHAR(12),VH.TransactionDate,106)) BETWEEN  CONVERT(DATE, CONVERT(VARCHAR(12),'" + oVoucherHistory.StartDateSt + "',106)) AND CONVERT(DATE, CONVERT(VARCHAR(12),'" + oVoucherHistory.EndDateSt + "',106)) ORDER BY VH.ActionType";
                }
                else
                {
                    sSQL = "SELECT * FROM View_VoucherHistory AS VH WHERE CONVERT(DATE, CONVERT(VARCHAR(12),VH.TransactionDate,106)) BETWEEN  CONVERT(DATE, CONVERT(VARCHAR(12),'" + oVoucherHistory.StartDateSt + "',106)) AND CONVERT(DATE, CONVERT(VARCHAR(12),'" + oVoucherHistory.EndDateSt + "',106)) ORDER BY VH.ActionType";
                }
            }

            return tc.ExecuteReader(sSQL);//use View_VoucherHistory
        }
      

       

       
        #endregion
    }  
}
