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
    public class GUProductionOrderHistoryDA
    {
        public GUProductionOrderHistoryDA() { }

        #region Insert Update Delete Function
        public static IDataReader InsertUpdate(TransactionContext tc, GUProductionOrderHistory oGUProductionOrderHistory, EnumDBOperation eEnumDBOperation, Int64 nUserID)
        {
            return tc.ExecuteReader("EXEC [SP_IUD_GUProductionOrderHistory]"
                                    + "%n, %n, %n, %n, %s, %n, %n",
                                    oGUProductionOrderHistory.GUProductionOrderHistoryID, oGUProductionOrderHistory.GUProductionOrderID, (int)oGUProductionOrderHistory.PreviousStatus, (int)oGUProductionOrderHistory.CurrentStatus, oGUProductionOrderHistory.Note, (int)eEnumDBOperation, nUserID);
        }

        public static void Delete(TransactionContext tc, GUProductionOrderHistory oGUProductionOrderHistory, EnumDBOperation eEnumDBOperation, Int64 nUserID)
        {
            tc.ExecuteNonQuery("EXEC [SP_IUD_GUProductionOrderHistory]"
                                    + "%n, %n, %n, %n, %s, %n, %n",
                                    oGUProductionOrderHistory.GUProductionOrderHistoryID, oGUProductionOrderHistory.GUProductionOrderID, (int)oGUProductionOrderHistory.PreviousStatus, (int)oGUProductionOrderHistory.CurrentStatus, oGUProductionOrderHistory.Note, (int)eEnumDBOperation, nUserID);
        }
        #endregion

        #region Get & Exist Function
        public static IDataReader Get(TransactionContext tc, long nID)
        {
            return tc.ExecuteReader("SELECT * FROM GUProductionOrderHistory WHERE GUProductionOrderHistoryID=%n", nID);
        }

        public static IDataReader Gets(TransactionContext tc)
        {
            return tc.ExecuteReader("SELECT * FROM GUProductionOrderHistory");
        }
        public static IDataReader Gets(TransactionContext tc, string sSQL)
        {
            return tc.ExecuteReader(sSQL);
        }
        #endregion
    }
}
