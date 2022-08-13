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
    public class HIAUserAssignDA
    {
        public HIAUserAssignDA() { }

        #region Insert Update Delete Function
        public static IDataReader InsertUpdate(TransactionContext tc, HIAUserAssign oHIAUserAssign, EnumDBOperation eEnumDBOperation, Int64 nUserID, String sHIAUserAssignIDs)
        {
            return tc.ExecuteReader("EXEC [SP_IUD_HIAUserAssign]" + "%n, %n, %n, %n, %n, %s",
                                    oHIAUserAssign.HIAUserAssignID, oHIAUserAssign.HIASetupID, oHIAUserAssign.UserID, nUserID, (int)eEnumDBOperation, "");
        }

        public static void Delete(TransactionContext tc, HIAUserAssign oHIAUserAssign, EnumDBOperation eEnumDBOperation, Int64 nUserID, String sHIAUserAssignIDs)
        {
            tc.ExecuteNonQuery("EXEC [SP_IUD_HIAUserAssign]" + "%n, %n, %n, %n, %n, %s",
                                    oHIAUserAssign.HIAUserAssignID, oHIAUserAssign.HIASetupID, oHIAUserAssign.UserID, nUserID, (int)eEnumDBOperation, sHIAUserAssignIDs);
        }
     
        #endregion

        #region Get & Exist Function
        public static IDataReader Get(TransactionContext tc, long nID)
        {
            return tc.ExecuteReader("SELECT * FROM View_HIAUserAssign WHERE HIAUserAssignID=%n", nID);
        }

        public static IDataReader Gets(int nHIASetupID, TransactionContext tc)
        {
            return tc.ExecuteReader("SELECT * FROM View_HIAUserAssign where HIASetupID =%n", nHIASetupID);
        }

        public static IDataReader Gets(TransactionContext tc, string sSQL)
        {
            return tc.ExecuteReader(sSQL);
        }
        #endregion
    }
}
