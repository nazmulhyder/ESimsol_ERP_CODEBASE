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
    public class EmployeeLoanInstallmentPolicyDA
    {
        public EmployeeLoanInstallmentPolicyDA() { }

        #region Insert Update Delete Function

        public static IDataReader IUD(TransactionContext tc, EmployeeLoanInstallmentPolicy oEmployeeLoanInstallmentPolicy, Int64 nUserID, int nDBOperation)
        {
            return tc.ExecuteReader("EXEC [SP_IUD_EmployeeLoanInstallmentPolicy] %n,%n,%n,%d,%u,%n,%n",
                   oEmployeeLoanInstallmentPolicy.ELIPID, oEmployeeLoanInstallmentPolicy.EmployeeLoanID,
                   oEmployeeLoanInstallmentPolicy.Amount,
                   oEmployeeLoanInstallmentPolicy.ExpectedRealizeDate, 
                   oEmployeeLoanInstallmentPolicy.RealizeNote, nUserID, nDBOperation);

        }

        #endregion

        #region Get & Exist Function

        public static IDataReader Get(int nELIPID, TransactionContext tc)
        {
            return tc.ExecuteReader("SELECT * FROM EmployeeLoanInstallmentPolicy WHERE ELIPID=%n", nELIPID);
        }
        public static IDataReader Get(string sSQL, TransactionContext tc)
        {
            return tc.ExecuteReader(sSQL);
        }
        public static IDataReader Gets(TransactionContext tc)
        {
            return tc.ExecuteReader("SELECT * FROM EmployeeLoanInstallmentPolicy");
        }
        public static IDataReader Gets(string sSQL, TransactionContext tc)
        {
            return tc.ExecuteReader(sSQL);
        }


        #endregion
    }
}
