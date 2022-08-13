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
    public class EmployeeLeaveTransferDA
    {
        public EmployeeLeaveTransferDA() { }

        #region Get & Exist Function
        public static IDataReader Get(TransactionContext tc, int nLeaveTransferID)
        {
            return tc.ExecuteReader("Select * from View_LeaveTransfer Where LeaveTransferID=%n", nLeaveTransferID);
        }

        public static IDataReader Gets(TransactionContext tc, int nEmpLeaveLedgerID)
        {
            return tc.ExecuteReader("Select * from [View_LeaveTransfer] Where ELLIDFrom=%n OR ELLIDTo=%n", nEmpLeaveLedgerID, nEmpLeaveLedgerID);
        }
        public static IDataReader Gets(TransactionContext tc, string sSQL)
        {
            return tc.ExecuteReader(sSQL);
        }


        #endregion
    }
}
