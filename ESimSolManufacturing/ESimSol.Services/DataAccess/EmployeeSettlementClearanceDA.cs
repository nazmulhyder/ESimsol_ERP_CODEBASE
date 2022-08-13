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
    public class EmployeeSettlementClearanceDA
    {
        public EmployeeSettlementClearanceDA() { }

        #region Insert Update Delete Function

        public static IDataReader IUD(TransactionContext tc, EmployeeSettlementClearance oEmployeeSettlementClearance, Int64 nUserID, int nDBOperation)
        {
            return tc.ExecuteReader("EXEC [SP_IUD_EmployeeSettlementClearance] %n,%n,%n,%n,%n,%n",
                   oEmployeeSettlementClearance.ESCID,
                   oEmployeeSettlementClearance.EmployeeSettlementID,
                   oEmployeeSettlementClearance.ESCSetupID,
                   oEmployeeSettlementClearance.CurrentStatus,
                   nUserID, nDBOperation);
        }

        #endregion

        #region Get & Exist Function

        public static IDataReader Get(int nESCID, TransactionContext tc)
        {
            return tc.ExecuteReader("SELECT * FROM View_EmployeeSettlementClearance WHERE ESCID=%n", nESCID);
        }
        public static IDataReader Get(string sSQL, TransactionContext tc)
        {
            return tc.ExecuteReader(sSQL);
        }
        public static IDataReader Gets(TransactionContext tc)
        {
            return tc.ExecuteReader("SELECT * FROM View_EmployeeSettlementClearance");
        }
        public static IDataReader Gets(string sSQL, TransactionContext tc)
        {
            return tc.ExecuteReader(sSQL);
        }

        #endregion
    }
}
