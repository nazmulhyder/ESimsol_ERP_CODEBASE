using System;
using System.Data;
using System.Data.SqlClient;
using ICS.Core.DataAccess;
using ESimSol.BusinessObjects;
using System.Data.OleDb;
using ICS.Core.Utility;

namespace ESimSol.Services.DataAccess
{
    public class BudgetDA
    {
        public BudgetDA() { }

        #region Insert Update Delete Function
        public static IDataReader InsertUpdate(TransactionContext tc, Budget oBudget, EnumDBOperation eEnumDBOperation, Int64 nUserID)
        {
            return tc.ExecuteReader("EXEC [SP_IUD_Budget]"
                                    + "%n, %s, %s, %d, %n, %n, %s, %n, %n, %n, %n, %n",
                                    oBudget.BudgetID, oBudget.BudgetNo, oBudget.ReviseNo, oBudget.IssueDate, oBudget.AccountingSessionID,
                                    (int)oBudget.BudgetType, oBudget.Remarks, oBudget.ApproveBy, oBudget.BUID, oBudget.BudgetStatus, (int)eEnumDBOperation, nUserID);
        }
        public static void Delete(TransactionContext tc, Budget oBudget, EnumDBOperation eEnumDBOperation, Int64 nUserID)
        {
            tc.ExecuteNonQuery("EXEC [SP_IUD_Budget]"
                                    + "%n, %s, %s, %d, %n, %n, %s, %n, %n, %n, %n, %n",
                                    oBudget.BudgetID, oBudget.BudgetNo, oBudget.ReviseNo, oBudget.IssueDate, oBudget.AccountingSessionID,
                                    (int)oBudget.BudgetType, oBudget.Remarks, oBudget.ApproveBy, oBudget.BUID, oBudget.BudgetStatus, (int)eEnumDBOperation, nUserID);
        }
        #endregion
        #region Revise
        public static IDataReader Revise(TransactionContext tc, Budget oBudget, EnumDBOperation eEnumDBOperation, Int64 nUserID)
        {
            return tc.ExecuteReader("EXEC [SP_AcceptBudgetRevise]"
                                    + "%n, %s, %s, %d, %n, %n, %s, %n, %n, %n, %n, %n",
                                    oBudget.BudgetID, oBudget.BudgetNo, oBudget.ReviseNo, oBudget.IssueDate, oBudget.AccountingSessionID,
                                    (int)oBudget.BudgetType, oBudget.Remarks, oBudget.ApproveBy, oBudget.BUID, oBudget.BudgetStatus, (int)eEnumDBOperation, nUserID);
        }
        #endregion

        #region Get & Exist Function
        public static IDataReader Gets(TransactionContext tc, string sSQL)
        {
            return tc.ExecuteReader(sSQL);//use View_Budget
        }
        public static IDataReader Get(TransactionContext tc, long nID)
        {
            return tc.ExecuteReader("SELECT * FROM View_Budget WHERE BudgetID=%n", nID);
        }
        #endregion
    }
}
