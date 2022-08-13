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
    public class MaxOTConfigurationUserDA
    {
        public MaxOTConfigurationUserDA() { }

        #region Insert Update Delete Function

        public static IDataReader InsertUpdate(TransactionContext tc, MaxOTConfigurationUser oMaxOTConfigurationUser, EnumDBOperation eEnumDBOperation, Int64 nUserID)
        {
            return tc.ExecuteReader("EXEC [SP_IUD_MaxOTConfigurationUser]"
                                    + "%n,%n,%n,%n,%n,%b,%s",
                                    oMaxOTConfigurationUser.MOCUID
                                    , oMaxOTConfigurationUser.MOCID
                                    , oMaxOTConfigurationUser.UserID
                                    , nUserID
                                    , (int)eEnumDBOperation
                                    , false
                                    , "");
        }

        public static void Delete(TransactionContext tc, MaxOTConfigurationUser oMaxOTConfigurationUser, EnumDBOperation eEnumDBOperation, Int64 nUserID, bool IsUserBased, string ids)
        {
            tc.ExecuteNonQuery("EXEC [SP_IUD_MaxOTConfigurationUser]"
                                    + "%n,%n,%n,%n,%n,%b,%s",
                                    oMaxOTConfigurationUser.MOCUID
                                    , oMaxOTConfigurationUser.MOCID
                                    , oMaxOTConfigurationUser.UserID
                                    , nUserID
                                    , (int)eEnumDBOperation
                                    , IsUserBased
                                    , ids);
        }

        #endregion

        #region Get & Exist Function
        public static IDataReader GetsUser(TransactionContext tc, Int64 id)
        {
            return tc.ExecuteReader("select * from MaxOTConfigurationUser where UserID=" + id);
        }
        public static IDataReader Get(TransactionContext tc, string sSQL)
        {
            return tc.ExecuteReader(sSQL);
        }
        public static IDataReader Gets(string sSQL, TransactionContext tc)
        {
            return tc.ExecuteReader(sSQL);
        }

        #endregion
    }
}

