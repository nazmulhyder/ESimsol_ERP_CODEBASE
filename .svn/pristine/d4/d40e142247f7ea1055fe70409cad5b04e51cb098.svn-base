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
    public class EmployeeCardHistoryDA
    {
        public EmployeeCardHistoryDA() { }

        #region Insert Update Delete Function

        public static IDataReader IUD(TransactionContext tc, EmployeeCardHistory oEmployeeCardHistory, Int64 nUserID)
        {
            return tc.ExecuteReader("",
                   oEmployeeCardHistory.ECHID, oEmployeeCardHistory.EmployeeCardID,
                   oEmployeeCardHistory.PreviousStatus, oEmployeeCardHistory.CurrentStatus,
                   
                   nUserID);

        }

        #endregion

        #region Get & Exist Function

        public static IDataReader Get(int nEmployeeCardHistoryID, TransactionContext tc)
        {
            return tc.ExecuteReader("SELECT * FROM View_EmployeeCardHistory WHERE EmployeeCardHistoryID=%n", nEmployeeCardHistoryID);
        }
        public static IDataReader Get(string sSQL, TransactionContext tc)
        {
            return tc.ExecuteReader(sSQL);
        }
        public static IDataReader Gets(TransactionContext tc)
        {
            return tc.ExecuteReader("SELECT * FROM View_EmployeeCardHistory");
        }
        public static IDataReader Gets(string sSQL, TransactionContext tc)
        {
            return tc.ExecuteReader(sSQL);
        }

        #endregion
    }
}
