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


    public class UserWiseContractorConfigureDA
    {
        public UserWiseContractorConfigureDA() { }

        #region Insert Update Delete Function
        public static IDataReader InsertUpdate(TransactionContext tc, UserWiseContractorConfigure oUserWiseContractorConfigure, EnumDBOperation eEnumDBOperation, Int64 nUserID)
        {
            return tc.ExecuteReader("EXEC [SP_IUD_UserWiseContractorConfigure]" + "%n, %n, %n, %n,  %n, %s",
                                    oUserWiseContractorConfigure.UserWiseContractorConfigureID, oUserWiseContractorConfigure.UserID,  oUserWiseContractorConfigure.ContractorID,  (int)eEnumDBOperation, nUserID,"");
        }
        public static void Delete(TransactionContext tc, UserWiseContractorConfigure oUserWiseContractorConfigure, EnumDBOperation eEnumDBOperation, Int64 nUserID,  string sContractorIDs)
        {
            tc.ExecuteNonQuery("EXEC [SP_IUD_UserWiseContractorConfigure]" + "%n, %n, %n, %n, %n, %s",
                                    oUserWiseContractorConfigure.UserWiseContractorConfigureID, oUserWiseContractorConfigure.UserID, oUserWiseContractorConfigure.ContractorID, (int)eEnumDBOperation, nUserID, sContractorIDs);
        }
        #endregion

        #region Get & Exist Function
        public static IDataReader Get(TransactionContext tc, long nID)
        {
            return tc.ExecuteReader("SELECT * FROM View_UserWiseContractorConfigure WHERE UserWiseContractorConfigureID=%n", nID);
        }

        public static IDataReader Gets(TransactionContext tc)
        {
            return tc.ExecuteReader("SELECT * FROM View_UserWiseContractorConfigure");
        }

        public static IDataReader GetsByUser(int id, TransactionContext tc)
        {
            return tc.ExecuteReader("SELECT * FROM View_UserWiseContractorConfigure WHERE UserID = %n",id);
        }
        public static IDataReader Gets(TransactionContext tc, string sSQL)
        {
            return tc.ExecuteReader(sSQL);
        }


        #endregion
    }
    
    
}
