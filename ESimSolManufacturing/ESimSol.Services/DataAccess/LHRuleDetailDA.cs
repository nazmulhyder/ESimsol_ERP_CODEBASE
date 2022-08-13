using System;
using System.Data;
using System.Data.SqlClient;
using ICS.Core.DataAccess;
using ESimSol.BusinessObjects;
using System.Data.OleDb;
using ICS.Core.Utility;

namespace ESimSol.Services.DataAccess
{
    public class LHRuleDetailDA
    {
        public LHRuleDetailDA() { }

        #region Insert Update Delete Function
        public static IDataReader InsertUpdate(TransactionContext tc, LHRuleDetail oLHRuleDetail, EnumDBOperation eEnumDBOperation, Int64 nUserID)
        {
            return tc.ExecuteReader("EXEC [SP_IUD_LHRuleDetail]"
                                    + "%n, %n, %n, %s, %n, %n, %n, %s",
                                    oLHRuleDetail.LHRuleDetailID, oLHRuleDetail.LHRuleID, oLHRuleDetail.LHRuleValueTypeInt, oLHRuleDetail.LHRuleValue, oLHRuleDetail.Sequence, (int)eEnumDBOperation, nUserID, "");
        }
        public static void Delete(TransactionContext tc, LHRuleDetail oLHRuleDetail, EnumDBOperation eEnumDBOperation, Int64 nUserID, string sLHRuleDetailIDs)
        {
            tc.ExecuteNonQuery("EXEC [SP_IUD_LHRuleDetail]"
                                    + "%n, %n, %n, %s, %n, %n, %n, %s",
                                    oLHRuleDetail.LHRuleDetailID, oLHRuleDetail.LHRuleID, oLHRuleDetail.LHRuleValueTypeInt, oLHRuleDetail.LHRuleValue, oLHRuleDetail.Sequence, (int)eEnumDBOperation, nUserID, sLHRuleDetailIDs);
        }
        #endregion

        #region Get & Exist Function
        public static IDataReader Get(TransactionContext tc, long nID)
        {
            return tc.ExecuteReader("SELECT * FROM View_LHRuleDetail WHERE LHRuleDetailID=%n ORDER BY Sequence", nID);
        }
        public static IDataReader Gets(TransactionContext tc)
        {
            return tc.ExecuteReader("SELECT * FROM View_LHRuleDetail Order By Sequence");
        }
        public static IDataReader Gets(TransactionContext tc, string sSQL)
        {
            return tc.ExecuteReader(sSQL);//use View_LHRuleDetail
        }
        #endregion
    }  
}


