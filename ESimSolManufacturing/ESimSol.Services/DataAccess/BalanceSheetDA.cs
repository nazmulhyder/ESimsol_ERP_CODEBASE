using System;
using System.Data;
using System.Data.SqlClient;
using ESimSol.BusinessObjects;
using System.Data.OleDb;
using ICS.Core.DataAccess;
using ICS.Core.Utility;


namespace ESimSol.Services.DataAccess
{
    public class BalanceSheetDA
    {
        public BalanceSheetDA() { }

        #region Insert Update Delete Function

        #endregion

        #region Get & Exist Function
        public static IDataReader Gets(TransactionContext tc, int nBUID, int nAccountType, DateTime dBalanceSheetStartDate, DateTime dBalanceSheetUptoDate, int nParentAccountHeadID, bool bIsApproved)
        {
            return tc.ExecuteReader("EXEC [SP_BalanceSheet]" + "%n, %n, %d, %d, %n, %b", nBUID, nAccountType, dBalanceSheetStartDate, dBalanceSheetUptoDate, nParentAccountHeadID, bIsApproved);
        }
        public static IDataReader GetsForRationAnalysis(TransactionContext tc, int nBUID, DateTime dBalanceSheetDate, int nRatioAnalysisID, bool bIsDivisible, int nParentAccountHeadID)
        {
            return tc.ExecuteReader("EXEC [SP_NotesOfRationAnalysis]" + "%n, %d, %n, %b, %n", nBUID, dBalanceSheetDate, nRatioAnalysisID, bIsDivisible, nParentAccountHeadID);
        }
        public static IDataReader ProcessBalanceSheet(TransactionContext tc, DateTime dBalanceSheetDate, string sStartBusinessCode, string sEndBusinessCode)
        {
            return tc.ExecuteReader("EXEC [SP_Process_BalanceSheet]" + "%d, %s, %s", dBalanceSheetDate, sStartBusinessCode, sEndBusinessCode);
        }
        #endregion
    }  
}
