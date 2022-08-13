using System;
using System.Data;
using System.Data.SqlClient;
using ICS.Core.DataAccess;
using ESimSol.BusinessObjects;
using System.Data.OleDb;
using ICS.Core.Utility;

namespace ESimSol.Services.DataAccess
{
    public class LHRuleDA
    {
        public LHRuleDA() { }

        #region Insert Update Delete Function
        public static IDataReader InsertUpdate(TransactionContext tc, LHRule oLHRule, EnumDBOperation eEnumDBOperation, Int64 nUserID)
        {
            return tc.ExecuteReader("EXEC [SP_IUD_LHRule]"
                                    + "%n, %n, %n, %s, %n, %n, %s",
                                    oLHRule.LHRuleID, oLHRule.LeaveHeadID, oLHRule.LHRuleTypeInt, oLHRule.Remarks, (int)eEnumDBOperation, nUserID, "");
        }
        public static void Delete(TransactionContext tc, LHRule oLHRule, EnumDBOperation eEnumDBOperation, Int64 nUserID, string sLHRuleIDs)
        {
            tc.ExecuteNonQuery("EXEC [SP_IUD_LHRule]"
                                     + "%n, %n, %n, %s, %n, %n, %s",
                                    oLHRule.LHRuleID, oLHRule.LeaveHeadID, oLHRule.LHRuleTypeInt, oLHRule.Remarks, (int)eEnumDBOperation, nUserID, sLHRuleIDs);
        }
        #endregion

        #region Get & Exist Function
        public static IDataReader Get(TransactionContext tc, long nID)
        {
            return tc.ExecuteReader("SELECT * FROM View_LHRule WHERE LHRuleID=%n", nID);
        }
        public static IDataReader Gets(TransactionContext tc)
        {
            return tc.ExecuteReader("SELECT * FROM View_LHRule Order By [Name]");
        }
        public static IDataReader Gets(TransactionContext tc, string sSQL)
        {
            return tc.ExecuteReader(sSQL);//use View_LHRule
        }
        #endregion
    }  
}

