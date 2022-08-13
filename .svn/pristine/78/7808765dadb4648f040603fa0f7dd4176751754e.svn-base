using System;
using System.Data;
using System.Data.SqlClient;
using ICS.Core;
using ESimSol.BusinessObjects;
using System.Data.OleDb;
using ICS.Core.DataAccess;
using ICS.Core.Utility;


namespace ESimSol.Services.DataAccess
{
    public class CashFlowDA
    {
        public CashFlowDA() { }

        #region Insert Update Delete Function
        public static IDataReader InsertUpdate(TransactionContext tc, CashFlow oCashFlow, EnumDBOperation eEnumDBOperation, Int64 nUserId)
        {
            return tc.ExecuteReader("EXEC [SP_IUD_CashFlow]"
                                    + "%n, %n, %n,  %n,  %n",
                                    oCashFlow.CashFlowID, oCashFlow.CashFlowHeadID, oCashFlow.VoucherDetailID, nUserId, (int)eEnumDBOperation);
        }

        public static void Delete(TransactionContext tc, CashFlow oCashFlow, EnumDBOperation eEnumDBOperation, Int64 nUserId)
        {
            tc.ExecuteNonQuery("EXEC [SP_IUD_CashFlow]"
                                    + "%n, %n, %n,  %n,  %n",
                                    oCashFlow.CashFlowID, oCashFlow.CashFlowHeadID, oCashFlow.VoucherDetailID, nUserId, (int)eEnumDBOperation);
        }
        public static void UpdateCashFlow(TransactionContext tc, int nCashFlowID, int nCashFlowHeadID)
        {
            tc.ExecuteNonQuery("Update CashFlow SET CashFlowHeadID = "+nCashFlowHeadID+" WHERE CashFlowID = "+nCashFlowID);
                                  
        }
        public static void CashFlowManage(TransactionContext tc, CashFlow oCashFlow, Int64 nUserId)
        {
            tc.ExecuteNonQuery("EXEC [SP_Cash_Flow_Manage]" + "%n, %d, %d, %n", oCashFlow.BUID, oCashFlow.StartDate, oCashFlow.EndDate, nUserId);
        }
        #endregion

        #region Get & Exist Function
        public static IDataReader Get(TransactionContext tc, long nID)
        {
            return tc.ExecuteReader("SELECT * FROM View_CashFlow WHERE CashFlowID=%n", nID);
        }
        public static IDataReader Gets(TransactionContext tc)
        {
            return tc.ExecuteReader("SELECT * FROM View_CashFlow");
        }
        public static IDataReader GetsForCashManage(TransactionContext tc, CashFlow oCashFlow)
        {
            return tc.ExecuteReader("SELECT * FROM View_CashFlow AS HH WHERE HH.BUID = %n AND HH.CashFlowHeadID = %n AND CONVERT(DATE,CONVERT(VARCHAR(12),HH.VoucherDate,106)) BETWEEN CONVERT(DATE,CONVERT(VARCHAR(12),%d,106)) AND CONVERT(DATE,CONVERT(VARCHAR(12),%d,106)) ORDER BY HH.VoucherDate, HH.VoucherDetailID ASC", oCashFlow.BUID, oCashFlow.CashFlowHeadID, oCashFlow.StartDate, oCashFlow.EndDate);
        }
        public static IDataReader GetsByName(TransactionContext tc, string sName)
        {

            return tc.ExecuteReader("SELECT * FROM View_CashFlow WHERE CashFlowName LIKE ('%" + sName + "%')   Order by [CashFlowName]");
        }
        public static IDataReader Gets(TransactionContext tc, CashFlow oCashFlow)
        {
            return tc.ExecuteReader("SELECT * FROM View_CashFlow AS CF WHERE CF.BUID =%n AND CF.CashFlowHeadID = %n AND ISNULL(CF.AuthorizedBy,0)!=0  AND CONVERT(DATE,CONVERT(VARCHAR(12), CF.VoucherDate,106)) BETWEEN CONVERT(DATE,CONVERT(VARCHAR(12), %d,106)) AND CONVERT(DATE,CONVERT(VARCHAR(12), %d,106)) ORDER BY CF.VoucherDate, CF.VoucherDetailID ASC", oCashFlow.BUID, oCashFlow.CashFlowHeadID, oCashFlow.StartDate, oCashFlow.EndDate);
        }
        public static IDataReader Gets(TransactionContext tc, string sSQL)
        {
            return tc.ExecuteReader(sSQL);
        }
        public static IDataReader GetCashFlowBreakDowns(TransactionContext tc, CashFlowDmSetup oCashFlowDmSetup)
        {
            return tc.ExecuteReader("EXEC [SP_CashFlowDmStatementBreakDown]" + "%n, %d, %d, %n, %n",
                                    oCashFlowDmSetup.BUID, oCashFlowDmSetup.StartDate, oCashFlowDmSetup.EndDate, oCashFlowDmSetup.CashFlowHeadID, oCashFlowDmSetup.SubGroupID);
        }
        #endregion
    }
}
