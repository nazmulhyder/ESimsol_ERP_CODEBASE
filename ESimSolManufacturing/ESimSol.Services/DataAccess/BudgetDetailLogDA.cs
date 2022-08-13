using System;
using System.Data;
using System.Data.SqlClient;
using ICS.Core.DataAccess;
using ESimSol.BusinessObjects;
using System.Data.OleDb;
using ICS.Core.Utility;

namespace ESimSol.Services.DataAccess
{
    public class BudgetDetailLogDA
    {
        public BudgetDetailLogDA() { }

        #region Insert Update Delete Function
        public static IDataReader InsertUpdate(TransactionContext tc, BudgetDetailLog oBudgetDetailLog, EnumDBOperation eEnumDBOperation, Int64 nUserID)
        {
            return tc.ExecuteReader("EXEC [SP_IUD_BudgetDetailLog]"
                                    + "%n, %n, %n, %n, %s, %n, %n",
                                    oBudgetDetailLog.BudgetDetailLogID, oBudgetDetailLog.BudgetLogID, oBudgetDetailLog.AccountHeadID, 
                                    oBudgetDetailLog.BudgetAmount, oBudgetDetailLog.Remarks, (int)eEnumDBOperation, nUserID);
        }
        public static void Delete(TransactionContext tc, BudgetDetailLog oBudgetDetailLog, EnumDBOperation eEnumDBOperation, Int64 nUserID)
        {
            tc.ExecuteReader("EXEC [SP_IUD_BudgetDetailLog]"
                                    + "%n, %n, %n, %n, %s, %n, %n",
                                    oBudgetDetailLog.BudgetDetailLogID, oBudgetDetailLog.BudgetLogID, oBudgetDetailLog.AccountHeadID,
                                    oBudgetDetailLog.BudgetAmount, oBudgetDetailLog.Remarks, (int)eEnumDBOperation, nUserID);
        }
        #endregion

        #region Get & Exist Function
        public static IDataReader Gets(TransactionContext tc, string sSQL)
        {
            return tc.ExecuteReader(sSQL);//use View_BudgetDetailLog
        }
        #endregion
    }  
}


