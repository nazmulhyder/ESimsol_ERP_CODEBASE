using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ESimSol.BusinessObjects;
using ICS.Core.Utility;
using ICS.Core.DataAccess;
using System.Data;

namespace ESimSol.Services.DataAccess
{
    public class EmployeeLoanDisbursementPolicyDA
    {
        public EmployeeLoanDisbursementPolicyDA() { }

        #region Insert Update Delete Function

        public static IDataReader IUD(TransactionContext tc, EmployeeLoanDisbursementPolicy oEmployeeLoanDisbursementPolicy, Int64 nUserID, int nDBOperation)
        {
            return tc.ExecuteReader("EXEC [SP_IUD_EmployeeLoanDisbursementPolicy] %n,%n,%n,%n,%d,%n,%n",
                   oEmployeeLoanDisbursementPolicy.ELDPID, oEmployeeLoanDisbursementPolicy.EmployeeLoanID,
                   oEmployeeLoanDisbursementPolicy.Amount, oEmployeeLoanDisbursementPolicy.ReceivableAmount,
                   oEmployeeLoanDisbursementPolicy.ExpectedDisburseDate, nUserID, nDBOperation);
                   

        }

        #endregion

        #region Get & Exist Function

        public static IDataReader Get(int nELDPID, TransactionContext tc)
        {
            return tc.ExecuteReader("SELECT * FROM EmployeeLoanDisbursementPolicy WHERE ELDPID=%n", nELDPID);
        }
        public static IDataReader Get(string sSQL, TransactionContext tc)
        {
            return tc.ExecuteReader(sSQL);
        }
        public static IDataReader Gets(TransactionContext tc)
        {
            return tc.ExecuteReader("SELECT * FROM EmployeeLoanDisbursementPolicy");
        }
        public static IDataReader Gets(string sSQL, TransactionContext tc)
        {
            return tc.ExecuteReader(sSQL);
        }


        #endregion
    }
}
