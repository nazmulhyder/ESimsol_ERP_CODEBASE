using System;
using System.Data;
using System.Data.SqlClient;
using ESimSol.BusinessObjects;
using System.Data.OleDb;
using ICS.Core.DataAccess;
using ICS.Core.Utility;

namespace ESimSol.Services.DataAccess
{
    public class AccountOpenningDA
    {
        public AccountOpenningDA() { }

        #region Insert Update Delete Function
        public static IDataReader SetOpenningBalance(TransactionContext tc, AccountOpenning oAccountOpenning, Int64 nUserID)
        {
            return tc.ExecuteReader("EXEC [SP_SetOpeingBalance]"
                                    + "%n, %n, %n, %d, %b, %n, %n, %n, %n, %n",
                                        oAccountOpenning.AccountingSessionID,
                                        oAccountOpenning.BusinessUnitID,
                                        oAccountOpenning.AccountHeadID,
                                        oAccountOpenning.OpenningDate,
                                        oAccountOpenning.IsDebit,
                                        oAccountOpenning.CurrencyID,
                                        oAccountOpenning.AmountInCurrency,
                                        oAccountOpenning.ConversionRate,
                                        oAccountOpenning.OpenningBalance,
                                        nUserID);
        }
        public static IDataReader SetOpenningBalanceSubledger(TransactionContext tc, AccountOpenning oAccountOpenning, Int64 nUserID)
        {
            return tc.ExecuteReader("EXEC [SP_SetOpeingBalanceSubledger]"
                                    + "%n, %n, %n, %n, %d, %b, %n, %n, %n, %n, %n",
                                        oAccountOpenning.AccountingSessionID,
                                        oAccountOpenning.BusinessUnitID,
                                        oAccountOpenning.AccountHeadID,
                                        oAccountOpenning.CostCenterID,
                                        oAccountOpenning.OpenningDate,
                                        oAccountOpenning.IsDebit,
                                        oAccountOpenning.CurrencyID,
                                        oAccountOpenning.AmountInCurrency,
                                        oAccountOpenning.ConversionRate,
                                        oAccountOpenning.OpenningBalance,
                                        nUserID);
        }
        public static void Delete(TransactionContext tc, int nAccountHeadID, int nAccountingSessionID, string sAccountOpenningIDs)
        {
            tc.ExecuteReader("DELETE FROM AccountOpenning WHERE AccountHeadID = " + nAccountHeadID.ToString() + " AND AccountingSessionID=" + nAccountingSessionID.ToString() + " AND AccountOpenningID NOT IN (SELECT items FROM dbo.SplitInToDataSet('" + sAccountOpenningIDs + "',','))");
        }        
        #endregion


        #region Get & Exist Function
        public static IDataReader Get(TransactionContext tc, long nID, int nCompanyID)
        {
            return tc.ExecuteReader("SELECT * FROM AccountOpenning WHERE AccountOpenningID=%n AND CompanyID=%n", nID, nCompanyID);
        }

        public static IDataReader Gets(TransactionContext tc, int nCompanyID)
        {
            return tc.ExecuteReader("SELECT * FROM View_AccountOpenning AND CompanyID=%n",nCompanyID);
        }
        public static IDataReader Gets(TransactionContext tc, int nSessionID, int nCompanyID)
        {
            return tc.ExecuteReader("SELECT * FROM View_AccountOpenning WHERE AccountingSessionID=%n AND CompanyID=%n", nSessionID, nCompanyID);
        }
        public static IDataReader Get(TransactionContext tc, int nAccountHeadID, int nSessionID, int nBusinessUnitID)
        {
            return tc.ExecuteReader("SELECT * FROM View_AccountOpenning WHERE AccountHeadID=%n AND AccountingSessionID=%n and BusinessUnitID=%n", nAccountHeadID, nSessionID, nBusinessUnitID);
        }
        public static IDataReader GetsByAccountAndSession(TransactionContext tc, int nAccountHeadID, int nSessionID, int nUserID)
        {
            return tc.ExecuteReader("SELECT * FROM View_AccountOpenning AS TT WHERE TT.AccountHeadID=%n AND TT.AccountingSessionID=%n", nAccountHeadID, nSessionID);
        }   
            public static IDataReader Gets(TransactionContext tc, string sSQL)
        {
            return tc.ExecuteReader(sSQL);
        }

        #endregion
    }  
   
}
