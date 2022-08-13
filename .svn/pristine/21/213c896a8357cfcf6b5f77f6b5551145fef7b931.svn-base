using System;
using System.Data;
using System.Data.SqlClient;
using ESimSol.BusinessObjects;
using System.Data.OleDb;
using ICS.Core.DataAccess;
using ICS.Core.Utility;

namespace ESimSol.Services.DataAccess
{
    public class ImportInvoiceIBPDA
    {
        public ImportInvoiceIBPDA() { }

        #region Generation Function
      

        #endregion

        #region Get & Exist Function
       

        public static IDataReader Gets(TransactionContext tc,int nBUID)
        {
            return tc.ExecuteReader("SELECT * FROM View_ImportInvoice where Bankstatus=3 and BUID=%n order by BankBranchID_Nego ,DateofMaturity,CurrencyID", nBUID);
        }

        public static IDataReader Gets(TransactionContext tc, string sSQL)
        {
            return tc.ExecuteReader("%q", sSQL);
        }

        public static IDataReader GetsForGraph(TransactionContext tc, string sYear, int nBankBranchID,int nBUID)
        {
            string sSql = "";
            sSql = "SELECT * FROM View_ImportInvoice WHERE BankStatus = 3 AND (ISNULL(YEAR(DateofMaturity),0) = " + sYear + " OR (ISNULL(YEAR(DateofMaturity),0) = 0 AND ISNULL(YEAR(DateofAcceptance),0) = " + sYear + "))";
            if (nBankBranchID > 0)
            {
                sSql = sSql + " AND BankBranchID_Nego=" + nBankBranchID;
            }
            if (nBUID > 0)
            {
                sSql = sSql + " AND BUID=" + nBUID;
            }
            sSql = sSql + " ORDER BY MONTH(DateofMaturity)";

            return tc.ExecuteReader(sSql);
        }

        #endregion

    }
}