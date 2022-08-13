using System;
using System.Data;
using System.Data.SqlClient;
using ICS.Core.DataAccess;
using ESimSol.BusinessObjects;
using System.Data.OleDb;
using ICS.Core.Utility;

namespace ESimSol.Services.DataAccess
{
    public class BudgetVarianceDA
    {
        public BudgetVarianceDA() { }

        #region Insert Update Delete Function
        public static IDataReader InsertUpdate(TransactionContext tc, BudgetVariance oBudgetVariance, EnumDBOperation eEnumDBOperation, Int64 nUserID)
        {
            return tc.ExecuteReader("EXEC [SP_IUD_BudgetVariance]"
                                    + "%n, %n, %b, %d, %d",
                                    oBudgetVariance.BudgetID, oBudgetVariance.ReportType, oBudgetVariance.IsApproved, oBudgetVariance.StartDate, oBudgetVariance.EndDate);
        }

        public static void Delete(TransactionContext tc, BudgetVariance oBudgetVariance, EnumDBOperation eEnumDBOperation, Int64 nUserID)
        {
            tc.ExecuteReader("EXEC [SP_IUD_BudgetVariance]"
                                    + "%n, %n, %b, %d, %d",
                                    oBudgetVariance.BudgetID, oBudgetVariance.ReportType, oBudgetVariance.IsApproved, oBudgetVariance.StartDate, oBudgetVariance.EndDate);
        }
        #endregion

        #region Get & Exist Function
        public static IDataReader Get(TransactionContext tc, long nID)
        {
            return tc.ExecuteReader("SELECT * FROM View_BudgetVariance WHERE BudgetVarianceID=%n", nID);
        }

        public static IDataReader Gets(TransactionContext tc)
        {
            return tc.ExecuteReader("SELECT * FROM View_BudgetVariance Order By [Name]");
        }
        public static IDataReader Gets(TransactionContext tc, string sSQL)
        {
            return tc.ExecuteReader(sSQL);//use View_BudgetVariance
        }
        public static IDataReader GetsReport(TransactionContext tc, int nBudgetID, int nReportType, bool IsApproved, string sStartDateSt, string sEndDateSt)
        {
            return tc.ExecuteReader("EXEC [SP_Budget_Variance]"+nBudgetID+","+nReportType+","+IsApproved+",'"+sStartDateSt+"','"+sEndDateSt+"'");
        }

        public static IDataReader GetByCategory(TransactionContext tc, bool bCategory)
        {
            return tc.ExecuteReader("SELECT * FROM View_BudgetVariance WHERE Category=%b Order By [Name] ", bCategory);
        }

        public static IDataReader GetByNegoBudgetVariance(TransactionContext tc, int nBudgetVarianceID)
        {
            return tc.ExecuteReader("select * from View_BudgetVariance where BudgetVarianceID in (select DISTINCT(NegotiationBudgetVarianceID) from EXPORTLC where NegotiationBudgetVarianceID>0 ) ", nBudgetVarianceID);
        }
        #endregion
    }  
}

