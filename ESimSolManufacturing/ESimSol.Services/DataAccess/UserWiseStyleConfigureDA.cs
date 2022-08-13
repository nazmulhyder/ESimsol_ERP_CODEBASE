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


    public class UserWiseStyleConfigureDA
    {
        public UserWiseStyleConfigureDA() { }

        #region Insert Update Delete Function
        public static IDataReader InsertUpdate(TransactionContext tc, UserWiseStyleConfigure oUserWiseStyleConfigure, EnumDBOperation eEnumDBOperation, Int64 nUserID)
        {
            return tc.ExecuteReader("EXEC [SP_IUD_UserWiseStyleConfigure]" + "%n, %n, %n, %n, %n",
                                    oUserWiseStyleConfigure.UserWiseStyleConfigureID, oUserWiseStyleConfigure.UserID, oUserWiseStyleConfigure.TechnicalSheetID, (int)eEnumDBOperation, nUserID);
        }

        public static void Delete(TransactionContext tc, UserWiseStyleConfigure oUserWiseStyleConfigure, EnumDBOperation eEnumDBOperation, Int64 nUserID)
        {
            tc.ExecuteNonQuery("EXEC [SP_IUD_UserWiseStyleConfigure]" + "%n, %n, %n, %n, %n",
                                    oUserWiseStyleConfigure.UserWiseStyleConfigureID, oUserWiseStyleConfigure.UserID, oUserWiseStyleConfigure.TechnicalSheetID, (int)eEnumDBOperation, nUserID);
        }

        #endregion

        #region Get & Exist Function
        public static IDataReader Get(TransactionContext tc, long nID)
        {
            return tc.ExecuteReader("SELECT * FROM View_UserWiseStyleConfigure WHERE UserWiseStyleConfigureID=%n", nID);
        }

        public static IDataReader Gets(TransactionContext tc)
        {
            return tc.ExecuteReader("SELECT * FROM View_UserWiseStyleConfigure");
        }

        public static IDataReader GetsByUser(int id, TransactionContext tc)
        {
            return tc.ExecuteReader("SELECT * FROM View_UserWiseStyleConfigure WHERE UserID = %n", id);
        }
        public static IDataReader Gets(TransactionContext tc, string sSQL)
        {
            return tc.ExecuteReader(sSQL);
        }


        #endregion
    }
    
    
  
}
