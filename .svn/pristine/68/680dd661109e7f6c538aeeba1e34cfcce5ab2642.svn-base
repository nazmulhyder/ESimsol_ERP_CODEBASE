using System;
using ICS.Core.Framework;
using ICS.Core.Utility;
using ICS.Core;
using ESimSol.BusinessObjects;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Data.OleDb;
using ICS.Core.DataAccess;

namespace ESimSol.Services.DataAccess
{
    public class BankReconcilationStatementDA
    {
        #region Get & Exist Function
        public static IDataReader GetBankReconcilationStatements(TransactionContext tc, BankReconcilationStatement oBankReconcilationStatement)
        {
            return tc.ExecuteReader("EXEC [SP_BankReconcilationStatement]" + "%n, %n,%n,%d",
                                    oBankReconcilationStatement.SubLedgerID, oBankReconcilationStatement.AccountHeadID, oBankReconcilationStatement.BusinessUnitID, oBankReconcilationStatement.BalanceDate);
        }

        public static IDataReader Get(TransactionContext tc, long nID)
        {
            return tc.ExecuteReader("SELECT * FROM View_BankReconcilationStatement WHERE BankReconcilationStatementID=%n", nID);
        }
        public static IDataReader Gets(TransactionContext tc)
        {
            return tc.ExecuteReader("SELECT * FROM View_BankReconcilationStatement ");
        }
        public static IDataReader Gets(TransactionContext tc, string sSQL)
        {
            return tc.ExecuteReader(sSQL);
        }

        #endregion
    }

}
