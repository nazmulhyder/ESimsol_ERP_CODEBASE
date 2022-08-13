using System;
using System.Data;
using System.Data.SqlClient;
using ICS.Core.DataAccess;
using ESimSol.BusinessObjects;
using System.Data.OleDb;
using ICS.Core.Utility;

namespace ESimSol.Services.DataAccess
{
    public class EmployeeLoanRefundDA
    {
        public EmployeeLoanRefundDA() { }


        #region Insert Update Delete Function
        public static IDataReader IUD(TransactionContext tc, EmployeeLoanRefund oELS, int nDBOperation, long nUserID)
        {

            return tc.ExecuteReader("EXEC [SP_IUD_EmployeeLoanRefund]" + "%n, %n, %n, %n, %n, %s, %d, %s, %n, %n", oELS.ELRID, oELS.EmployeeLoanID, oELS.NoOfInstallmentRefund, oELS.Amount, oELS.ServiceCharge, oELS.Note, oELS.RefundDate, oELS.RefundNo, nUserID, nDBOperation);
        }

        #endregion

        #region Get Functions
        public static IDataReader Get(TransactionContext tc, int nID)
        {
            return tc.ExecuteReader("SELECT * FROM View_EmployeeLoanRefund WHERE ELRID=%n", nID);
        }

        public static IDataReader Gets(TransactionContext tc, string sSQL)
        {
            return tc.ExecuteReader(sSQL);
        }

        #endregion
    }
}
