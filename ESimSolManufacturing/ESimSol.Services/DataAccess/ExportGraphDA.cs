using System;
using System.Data;
using System.Data.SqlClient;
using ESimSol.BusinessObjects;
using System.Data.OleDb;
using ICS.Core.DataAccess;
using ICS.Core.Utility;

namespace ESimSol.Services.DataAccess
{
    public class ExportGraphDA
    {
        public ExportGraphDA() { }

        #region Generation Function


        #endregion

        #region Get & Exist Function


        public static IDataReader Gets(TransactionContext tc, int nBUID)
        {
            return tc.ExecuteReader("SELECT * FROM View_ExportBillReport Where [State]=5 And BUID=%n ORDER BY BankBranchID_Negotiation, MaturityDate,CurrencyID", nBUID);
        }

        public static IDataReader Gets(TransactionContext tc, string sSQL)
        {
            return tc.ExecuteReader("%q", sSQL);
        }

        public static IDataReader GetsForGraph(TransactionContext tc, string sYear, int nBankBranchID, int nBUID, string sDateCriteria)
        {
            string sSql = "";
            sSql = "SELECT * FROM View_ExportBillReport WHERE YEAR(" + sDateCriteria + ")=" + sYear;
            if (nBankBranchID > 0)
            {
                sSql = sSql + " AND BankBranchID_Negotiation=" + nBankBranchID;
            }
            if (nBUID > 0)
            {
                sSql = sSql + " AND BUID=" + nBUID;
            }
            sSql = sSql + " ORDER BY MONTH(" + sDateCriteria + ")";

            return tc.ExecuteReader(sSql);
        }

        #endregion

    }
}