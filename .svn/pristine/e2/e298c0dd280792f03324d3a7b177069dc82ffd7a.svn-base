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
    public class VOrderDA
    {
        public VOrderDA() { }

        #region Insert Update Delete Function
        public static IDataReader InsertUpdate(TransactionContext tc, VOrder oVOrder, EnumDBOperation eEnumDBOperation, Int64 nUserID)
        {
            return tc.ExecuteReader("EXEC [SP_IUD_VOrder]" + "%n, %n, %s, %n, %n, %s, %d, %n, %s, %n, %n",
                                    oVOrder.VOrderID, oVOrder.BUID, oVOrder.RefNo, oVOrder.VOrderRefTypeInt, oVOrder.VOrderRefID, oVOrder.OrderNo, oVOrder.OrderDate, oVOrder.SubledgerID, oVOrder.Remarks, (int)eEnumDBOperation, nUserID);
        }
        public static void Delete(TransactionContext tc, VOrder oVOrder, EnumDBOperation eEnumDBOperation, Int64 nUserID)
        {
            tc.ExecuteNonQuery("EXEC [SP_IUD_VOrder]" + "%n, %n, %s, %n, %n, %s, %d, %n, %s, %n, %n",
                                    oVOrder.VOrderID, oVOrder.BUID, oVOrder.RefNo, oVOrder.VOrderRefTypeInt, oVOrder.VOrderRefID, oVOrder.OrderNo, oVOrder.OrderDate, oVOrder.SubledgerID, oVOrder.Remarks, (int)eEnumDBOperation, nUserID);
        }
        public static void UpdateSubledger(TransactionContext tc, int nVOrderID, int nSubledgerID)
        {
            tc.ExecuteNonQuery("UPDATE VOrder SET SubledgerID = %n WHERE VOrderID = %n", nSubledgerID, nVOrderID);
        }
        #endregion

        #region Get & Exist Function
        public static IDataReader Get(TransactionContext tc, long nID)
        {
            return tc.ExecuteReader("SELECT * FROM View_VOrder WHERE VOrderID=%n", nID);
        }
        public static IDataReader Gets(TransactionContext tc)
        {
            return tc.ExecuteReader("SELECT * FROM View_VOrder");
        }
        public static IDataReader Gets(TransactionContext tc, string sSQL)
        {
            return tc.ExecuteReader(sSQL);
        }
        #endregion
    }  
}
