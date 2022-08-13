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
    public class EmployeeConfirmationDA
    {
        public EmployeeConfirmationDA() { }

        #region Insert Update Delete Function

        public static IDataReader IUD(TransactionContext tc, EmployeeConfirmation oEmployeeConfirmation, Int64 nUserID, int nDBOperation)
        {
            return tc.ExecuteReader("EXEC [SP_IUD_EmployeeConfirmation] %n,%n,%n, %s, %d ,%d,%s,%n,%n,%n",
                   oEmployeeConfirmation.ECID, (int)oEmployeeConfirmation.EmployeeCategory,
                   oEmployeeConfirmation.EmployeeID, oEmployeeConfirmation.EmployeeCode, oEmployeeConfirmation.StartDate, oEmployeeConfirmation.EndDate,
                   oEmployeeConfirmation.Note, oEmployeeConfirmation.MotherEmployeeID, nUserID, nDBOperation);
        }

        #endregion

        #region Get & Exist Function

        public static IDataReader Get(int nECID, TransactionContext tc)
        {
            return tc.ExecuteReader("SELECT * FROM View_EmployeeConfirmation WHERE ECID=%n", nECID);
        }
        public static IDataReader Get(string sSQL, TransactionContext tc)
        {
            return tc.ExecuteReader(sSQL);
        }
        public static IDataReader Gets(TransactionContext tc)
        {
            return tc.ExecuteReader("SELECT * FROM View_EmployeeConfirmation");
        }
        public static IDataReader Gets(string sSQL, TransactionContext tc)
        {
            return tc.ExecuteReader(sSQL);
        }
        #endregion
    }
}
