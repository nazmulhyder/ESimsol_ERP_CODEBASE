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
    public class EmployeeSettlementClearanceSetupDA
    {
        public EmployeeSettlementClearanceSetupDA() { }

        #region Insert Update Delete Function

        public static IDataReader IUD(TransactionContext tc, EmployeeSettlementClearanceSetup oEmployeeSettlementClearanceSetup, Int64 nUserID, int nDBOperation)
        {
            return tc.ExecuteReader("EXEC [SP_IUD_EmployeeSettlementClearanceSetup] %n,%n,%n,%n,%n",
                   oEmployeeSettlementClearanceSetup.ESCSetupID, oEmployeeSettlementClearanceSetup.ESCSID,
                   oEmployeeSettlementClearanceSetup.EmployeeID, nUserID, nDBOperation);

        }

        #endregion

        #region Get & Exist Function

        public static IDataReader Get(int nESCSetupID, TransactionContext tc)
        {
            return tc.ExecuteReader("SELECT * FROM View_EmployeeSettlementClearanceSetup WHERE ESCSetupID=%n", nESCSetupID);
        }
        public static IDataReader Get(string sSQL, TransactionContext tc)
        {
            return tc.ExecuteReader(sSQL);
        }
        public static IDataReader Gets(TransactionContext tc)
        {
            return tc.ExecuteReader("SELECT * FROM View_EmployeeSettlementClearanceSetup");
        }
        public static IDataReader Gets(string sSQL, TransactionContext tc)
        {
            return tc.ExecuteReader(sSQL);
        }

        #endregion
    }
}
