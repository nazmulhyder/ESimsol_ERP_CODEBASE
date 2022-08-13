using System;
using System.Data;
using System.Data.SqlClient;
using ICS.Core.DataAccess;
using ESimSol.BusinessObjects;
using System.Data.OleDb;
using ICS.Core.Utility;

namespace ESimSol.Services.DataAccess
{
    public class EmployeeProcessMailHistoryDA
    {
        public EmployeeProcessMailHistoryDA() { }

        #region Insert Update Delete Function
        public static IDataReader InsertUpdate(TransactionContext tc, EmployeeProcessMailHistory oEmployeeProcessMailHistory, EnumDBOperation eEnumDBOperation, Int64 nUserID)
        {
            return tc.ExecuteReader("EXEC [SP_IUD_EmployeeProcessMailHistory]"
                                    + "%n, %n, %n, %b, %s, %n, %n",
                                    oEmployeeProcessMailHistory.EPMHID, oEmployeeProcessMailHistory.PPMID, oEmployeeProcessMailHistory.EmployeeID, 
                                    oEmployeeProcessMailHistory.IsStatus, oEmployeeProcessMailHistory.FeedBackMessage, (int)eEnumDBOperation, nUserID);
        }

        public static void Delete(TransactionContext tc, EmployeeProcessMailHistory oEmployeeProcessMailHistory, EnumDBOperation eEnumDBOperation, Int64 nUserID)
        {
            tc.ExecuteNonQuery("EXEC [SP_IUD_EmployeeProcessMailHistory]"
                                    + "%n, %n, %n, %b, %s, %n, %n",
                                    oEmployeeProcessMailHistory.EPMHID, oEmployeeProcessMailHistory.PPMID, oEmployeeProcessMailHistory.EmployeeID,
                                    oEmployeeProcessMailHistory.IsStatus, oEmployeeProcessMailHistory.FeedBackMessage, (int)eEnumDBOperation, nUserID);
        }
        #endregion

        #region Get & Exist Function
        public static IDataReader Get(TransactionContext tc, long nID)
        {
            return tc.ExecuteReader("SELECT * FROM View_EmployeeProcessMailHistory WHERE EPMHID=%n", nID);
        }
        public static IDataReader Gets(TransactionContext tc)
        {
            return tc.ExecuteReader("SELECT * FROM View_EmployeeProcessMailHistory");
        }
        public static IDataReader Gets(TransactionContext tc, string sSQL)
        {
            return tc.ExecuteReader(sSQL);
        }
        #endregion
    }  
}


