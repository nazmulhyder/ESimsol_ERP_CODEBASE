using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ESimSol.BusinessObjects;
using ICS.Core.Utility;
using ICS.Core.DataAccess;
using System.Data;

namespace ESimSol.Services.DataAccess
{
    public class AuthorizationUserOEDODA
    {
        public AuthorizationUserOEDODA() { }

        #region Insert Update Delete Function
        public static IDataReader InsertUpdate(TransactionContext tc, AuthorizationUserOEDO oAuthorizationUserOEDO, EnumDBOperation eEnumDBOperation, Int64 nUserId)
        {
            return tc.ExecuteReader("EXEC [SP_IUD_AuthorizationUserOEDO]"
                                    + "%s, %n, %n",
                                    oAuthorizationUserOEDO.sString, nUserId, (int)eEnumDBOperation);
        }

        public static void Delete(TransactionContext tc, AuthorizationUserOEDO oAuthorizationUserOEDO, EnumDBOperation eEnumDBOperation, Int64 nUserId)
        {
            tc.ExecuteNonQuery("EXEC [SP_IUD_AuthorizationUserOEDO]"
                                    + "%n, %n, %n, %b, %b, %n, %n",
                                    oAuthorizationUserOEDO.AUOEDOID, oAuthorizationUserOEDO.UserID, oAuthorizationUserOEDO.AWUOEDBID, oAuthorizationUserOEDO.IsMTRApply, oAuthorizationUserOEDO.IsActive, nUserId, (int)eEnumDBOperation);
        }
        #endregion

        #region Get & Exist Function
        public static IDataReader Get(TransactionContext tc, long nID)
        {
            return tc.ExecuteReader("SELECT * FROM View_AuthorizationUserOEDO WHERE AUOEDOID=%n", nID);
        }

        public static IDataReader Gets(TransactionContext tc)
        {
            return tc.ExecuteReader("SELECT * FROM View_AuthorizationUserOEDO Order By AUOEDOID DESC");
        }
        public static IDataReader Gets(TransactionContext tc, string sSQL)
        {
            return tc.ExecuteReader(sSQL);
        }
        public static IDataReader GetsByUser(TransactionContext tc, int ID)
        {//User Get all Authorization by this user
            return tc.ExecuteReader("SELECT * FROM View_AuthorizationUserOEDO WHERE UserID=%n", ID);
        }
        #endregion
    }

  
}
