using System;
using System.Data;
using System.Data.SqlClient;
using ESimSol.BusinessObjects;
using System.Data.OleDb;
using ICS.Core.DataAccess;
using ICS.Core.Utility;

namespace ESimSol.Services.DataAccess
{

    public class LedgerBreakDownDA
    {
        public LedgerBreakDownDA() { }

        #region Insert Function
        public static IDataReader InsertUpdate(TransactionContext tc, LedgerBreakDown oLedgerBreakDown, EnumDBOperation eEnumDBOperation, Int64 nUserID, string sRefernceIDs, bool bIsEffectedAccounts)
        {
            return tc.ExecuteReader("EXEC [SP_IUD_LedgerBreakDown]" + "%n, %n, %b, %n, %n, %n, %s, %b", 
                                oLedgerBreakDown.LedgerBreakDownID, oLedgerBreakDown.ReferenceID, oLedgerBreakDown.IsEffectedAccounts, oLedgerBreakDown.AccountHeadID, nUserID, (int)eEnumDBOperation, sRefernceIDs, bIsEffectedAccounts);
        }
        #endregion
        
            
        #region Update Function
        public static void Delete(TransactionContext tc, LedgerBreakDown oLedgerBreakDown, EnumDBOperation eEnumDBOperation, Int64 nUserID, string sRefernceIDs, bool bIsEffectedAccounts)
        {
            tc.ExecuteNonQuery("EXEC [SP_IUD_LedgerBreakDown]" + "%n, %n, %b, %n, %n, %n, %s, %b",
                                oLedgerBreakDown.LedgerBreakDownID, oLedgerBreakDown.ReferenceID, oLedgerBreakDown.IsEffectedAccounts, oLedgerBreakDown.AccountHeadID, nUserID, (int)eEnumDBOperation, sRefernceIDs, bIsEffectedAccounts);
        }

        public static void DeleteLedgerBreakdown(TransactionContext tc, int nReferenceID)
        {
            tc.ExecuteNonQuery("DELETE FROM LedgerBreakDown WHERE IsEffectedAccounts=0 AND ReferenceID=%n", nReferenceID);
        }
        #endregion


        #region Get & Exist Function
        public static IDataReader Get(TransactionContext tc, Int64 nID)
        {
            return tc.ExecuteReader("SELECT * FROM [VIEW_LedgerBreakDown] WHERE LedgerBreakDown=%n", nID);
        }
        public static IDataReader Gets(TransactionContext tc)
        {
            return tc.ExecuteReader("SELECT * FROM [VIEW_LedgerBreakDown]");
        }
        public static IDataReader Gets(TransactionContext tc, int nReferenceID, bool bIsEffectedAccounts)
        {
            return tc.ExecuteReader("SELECT * FROM [VIEW_LedgerBreakDown] WHERE ReferenceID = %n AND IsEffectedAccounts = %b", nReferenceID, bIsEffectedAccounts);
        }
        public static IDataReader Gets(TransactionContext tc, int nStatementSetupID)
        {
            return tc.ExecuteReader("SELECT * FROM [VIEW_LedgerBreakDown] WHERE ReferenceID IN  (SELECT LedgerGroupSetupID FROM LedgerGroupSetup WHERE OCSID IN (SELECT OperationCategorySetupID FROM OperationCategorySetup WHERE StatementSetupID = %n)) AND IsEffectedAccounts = %b",nStatementSetupID, false);
        }
        #endregion
    } 

}
