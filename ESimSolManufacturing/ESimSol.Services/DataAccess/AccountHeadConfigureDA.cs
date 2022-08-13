using System;
using System.Data;
using System.Data.SqlClient;
using ESimSol.BusinessObjects;
using System.Data.OleDb;
using ICS.Core.DataAccess;
using ICS.Core.Utility;

namespace ESimSol.Services.DataAccess
{
    public class AccountHeadConfigureDA
    {
        #region Insert Update Delete Function
        public static IDataReader InsertUpdate(TransactionContext tc, AccountHeadConfigure oAccountHeadConfigure, EnumDBOperation eEnumDBOperation, Int64 nUserID)
        {
            return tc.ExecuteReader("EXEC [SP_IUD_AccountHeadConfigure]"
                                    + "%n, %n, %n, %n, %n, %n",
                                    oAccountHeadConfigure.AccountHeadConfigureID,
                                    oAccountHeadConfigure.AccountHeadID,
                                    oAccountHeadConfigure.ReferenceObjectID,
                                    oAccountHeadConfigure.ReferenceObjectType,                                     
                                    nUserID,
                                    (int)eEnumDBOperation);
        }

        public static void Delete(TransactionContext tc, AccountHeadConfigure oAccountHeadConfigure, EnumDBOperation eEnumDBOperation, Int64 nUserID)
        {
            tc.ExecuteNonQuery("EXEC [SP_IUD_AccountHeadConfigure]"
                                    + "%n, %n, %n, %n, %n, %n",
                                    oAccountHeadConfigure.AccountHeadConfigureID,
                                    oAccountHeadConfigure.AccountHeadID,
                                    oAccountHeadConfigure.ReferenceObjectID,
                                    oAccountHeadConfigure.ReferenceObjectType,
                                    nUserID,
                                    (int)eEnumDBOperation);
        }
        #endregion

        #region Get & Exist Function
        public static IDataReader Gets(TransactionContext tc)
        {
            return tc.ExecuteReader("SELECT * FROM View_AccountHeadConfigure");
        }
        public static IDataReader Gets(TransactionContext tc, int nExplationType, int nAccountHeadID)
        {
            return tc.ExecuteReader("SELECT * FROM View_AccountHeadConfigure WHERE ReferenceObjectType=%n AND AccountHeadID=%n", nExplationType, nAccountHeadID);
        }
        public static IDataReader Gets(TransactionContext tc, string sSQL)
        {
            return tc.ExecuteReader(sSQL);
        }
        public static IDataReader Get(TransactionContext tc, long nID)
        {
            return tc.ExecuteReader("SELECT * FROM View_AccountHeadConfigure WHERE AccountHeadConfigureID=%n", nID);
        }

        #endregion

    }
}
