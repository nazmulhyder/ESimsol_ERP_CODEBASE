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
    public class EmployeeCardDA
    {
        public EmployeeCardDA() { }

        #region Insert Update Delete Function

        public static IDataReader IUD(TransactionContext tc, EmployeeCard oEmployeeCard, Int64 nUserID)
        {
            return tc.ExecuteReader("EXEC [SP_IUD_EmployeeCard] %n,%n,%n,%d,%d,%b,%n",
                   oEmployeeCard.EmployeeCardID, oEmployeeCard.EmployeeID,
                   oEmployeeCard.EmployeeCardStatus, oEmployeeCard.IssueDate,
                   oEmployeeCard.ExpireDate, oEmployeeCard.IsActive,
                   nUserID);

        }

        #endregion

        #region Get & Exist Function

        public static IDataReader Get(int nEmployeeCardID, TransactionContext tc)
        {
            return tc.ExecuteReader("SELECT * FROM View_EmployeeCard WHERE EmployeeCardID=%n", nEmployeeCardID);
        }
        public static IDataReader Get(string sSQL, TransactionContext tc)
        {
            return tc.ExecuteReader(sSQL);
        }
        public static IDataReader Gets(TransactionContext tc)
        {
            return tc.ExecuteReader("SELECT * FROM View_EmployeeCard");
        }
        public static IDataReader Gets(string sSQL, TransactionContext tc)
        {
            return tc.ExecuteReader(sSQL);
        }

        #endregion
    }
}
