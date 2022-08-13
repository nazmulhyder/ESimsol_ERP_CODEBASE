using System;
using System.Data;
using System.Data.SqlClient;
using ICS.Base.DataAccess;
using ESimSol.BusinessObjects;
using System.Data.OleDb;
using ICS.Base.Client.Utility;


namespace ESimSol.Services.DataAccess
{
    public class AccountHeadDA
    {
        public AccountHeadDA() { }

        #region Insert Update Delete Function
        public static IDataReader InsertUpdate(TransactionContext tc, AccountHead oAccountHead, EnumDBOperation eEnumDBOperation)
        {
            return tc.ExecuteReader("EXEC [SP_InsertUpdateDeleteAccountHead]" 
                                    + "%n, %n, %s, %s, %n, %s, %b, %b, %n, %n, %n",
                                    oAccountHead.ObjectID, oAccountHead.DAHCID, oAccountHead.AccountCode, oAccountHead.AccountHeadName, oAccountHead.ReferenceObjectID, oAccountHead.Description, oAccountHead.IsJVNode, oAccountHead.IsDynamic, oAccountHead.ParentHeadID, User.CurrentUser.ObjectID, (int)eEnumDBOperation);
        }

        public static void Delete(TransactionContext tc, AccountHead oAccountHead, EnumDBOperation eEnumDBOperation)
        {
            tc.ExecuteNonQuery("EXEC [SP_InsertUpdateDeleteAccountHead]"
                                    + "%n, %n, %s, %s, %n, %s, %b, %b, %n, %n, %n",
                                    oAccountHead.ObjectID, oAccountHead.DAHCID, oAccountHead.AccountCode, oAccountHead.AccountHeadName, oAccountHead.ReferenceObjectID, oAccountHead.Description, oAccountHead.IsJVNode, oAccountHead.IsDynamic, oAccountHead.ParentHeadID, User.CurrentUser.ObjectID, (int)eEnumDBOperation);
        }
        #endregion

        #region Get & Exist Function
        public static IDataReader Get(TransactionContext tc, long nID)
        {
            return tc.ExecuteReader("SELECT * FROM AccountHead WHERE AccountHeadID=%n", nID);
        }
        public static IDataReader Get(TransactionContext tc, string sAccountCode)
        {
            return tc.ExecuteReader("SELECT * FROM AccountHead WHERE AccountCode=%s", sAccountCode);
        }
        public static IDataReader Gets(TransactionContext tc)
        {
            return tc.ExecuteReader("SELECT * FROM AccountHead");
        }
        public static IDataReader Gets(TransactionContext tc, string sSQL)
        {
            return tc.ExecuteReader(sSQL);
        }
        #endregion
    }
}
