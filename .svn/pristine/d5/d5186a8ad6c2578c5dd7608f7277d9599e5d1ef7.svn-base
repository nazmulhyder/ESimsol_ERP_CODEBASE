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

    public class UserWiseContractorConfigureDetailDA
    {
        public UserWiseContractorConfigureDetailDA() { }

        #region Insert Update Delete Function
        public static IDataReader InsertUpdate(TransactionContext tc, UserWiseContractorConfigureDetail oUserWiseContractorConfigureDetail, EnumDBOperation eEnumDBOperation, Int64 nUserID, String sUserWiseContractorConfigureDetailIDs)
        {
            return tc.ExecuteReader("EXEC[SP_IUD_UserWiseContractorConfigureDetail]"
                                    + "%n,%n,%n,%n,%n,%s", oUserWiseContractorConfigureDetail.UserWiseContractorConfigureDetailID, oUserWiseContractorConfigureDetail.UserWiseContractorConfigureID, (int)oUserWiseContractorConfigureDetail.StyleType, nUserID, (int)eEnumDBOperation, "");
        }

        public static void Delete(TransactionContext tc, UserWiseContractorConfigureDetail oUserWiseContractorConfigureDetail, EnumDBOperation eEnumDBOperation, Int64 nUserID, String sUserWiseContractorConfigureDetailIDs)
        {
            tc.ExecuteNonQuery("EXEC[SP_IUD_UserWiseContractorConfigureDetail]"
                                    + "%n,%n,%n,%n,%n,%s", oUserWiseContractorConfigureDetail.UserWiseContractorConfigureDetailID, oUserWiseContractorConfigureDetail.UserWiseContractorConfigureID, (int)oUserWiseContractorConfigureDetail.StyleType, nUserID, (int)eEnumDBOperation, sUserWiseContractorConfigureDetailIDs);
        }


        #endregion

        #region Get & Exist Function
        public static IDataReader Get(TransactionContext tc, long nID)
        {
            return tc.ExecuteReader("SELECT * FROM UserWiseContractorConfigureDetail WHERE UserWiseContractorConfigureDetailID=%n", nID);
        }

        public static IDataReader Gets(int nUserWiseContractorConfigureID, TransactionContext tc)
        {
            return tc.ExecuteReader("SELECT * FROM UserWiseContractorConfigureDetail where UserWiseContractorConfigureID =%n ", nUserWiseContractorConfigureID);
        }

        public static IDataReader Gets(TransactionContext tc, string sSQL)
        {
            return tc.ExecuteReader(sSQL);
        }
        #endregion
    }
    
    
    
}
