using System;
using System.Data;
using System.Data.SqlClient;
using ESimSol.BusinessObjects;
using System.Data.OleDb;
using ICS.Core.DataAccess;
using ICS.Core.Utility;

namespace ESimSol.Services.DataAccess
{
    public class BUPermissionDA
    {
        public BUPermissionDA() { }

        #region Insert Update Delete Function
        public static IDataReader InsertUpdate(TransactionContext tc, BUPermission oBUPermission, EnumDBOperation eEnumDBOperation, Int64 nUserID)
        {
            return tc.ExecuteReader("EXEC [SP_IUD_BUPermission]" + "%n, %n, %n, %s, %n, %n",
                                    oBUPermission.BUPermissionID, oBUPermission.UserID, oBUPermission.BUID, oBUPermission.Remarks, (int)eEnumDBOperation, nUserID);
        }

        public static void Delete(TransactionContext tc, BUPermission oBUPermission, EnumDBOperation eEnumDBOperation, Int64 nUserID)
        {
            tc.ExecuteNonQuery("EXEC [SP_IUD_BUPermission]" + "%n, %n, %n, %s, %n, %n",
                                    oBUPermission.BUPermissionID, oBUPermission.UserID, oBUPermission.BUID, oBUPermission.Remarks, (int)eEnumDBOperation, nUserID);
        }

        public static IDataReader InsertUpdateBUWiseShift(TransactionContext tc, BUPermission oBUPermission, EnumDBOperation eEnumDBOperation, Int64 nUserID)
        {
            return tc.ExecuteReader("EXEC [SP_IUD_BUWiseShift]" + "%n, %n, %n, %s, %n, %n",
                                    oBUPermission.BUWiseShiftID, oBUPermission.ShiftID, oBUPermission.BUID, oBUPermission.Remarks, (int)eEnumDBOperation, nUserID);
        }
        public static void DeleteBUWiseShift(TransactionContext tc, BUPermission oBUPermission, EnumDBOperation eEnumDBOperation, Int64 nUserID)
        {
            tc.ExecuteNonQuery("EXEC [SP_IUD_BUWiseShift]" + "%n, %n, %n, %s, %n, %n",
                                    oBUPermission.BUWiseShiftID, oBUPermission.ShiftID, oBUPermission.BUID, oBUPermission.Remarks, (int)eEnumDBOperation, nUserID);
        }

        #endregion

        #region Get & Exist Function
        public static IDataReader Get(TransactionContext tc, long nID)
        {
            return tc.ExecuteReader("SELECT * FROM View_BUPermission WHERE BUPermissionID=%n", nID);
        }

        public static IDataReader Gets(TransactionContext tc)
        {
            return tc.ExecuteReader("SELECT * FROM View_BUPermission");
        }
        public static IDataReader GetsByUser(TransactionContext tc, int nPermittedUserID)
        {
            return tc.ExecuteReader("SELECT * FROM View_BUPermission WHERE UserID=%n", nPermittedUserID);
        }
        public static IDataReader Gets(TransactionContext tc, string sSQL)
        {
            return tc.ExecuteReader(sSQL);
        }
        #endregion
    }
}
