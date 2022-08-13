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
    public class RosterTransferDA
    {
        public RosterTransferDA() { }

        #region Insert Update Delete Function

        public static IDataReader IUD(TransactionContext tc, RosterTransfer oRosterTransfer, Int64 nUserID, int nDBOperation)
        {
            return tc.ExecuteReader("EXEC [SP_IUD_RosterTransfer] %n,%n,%n,%d,%b,%n,%n",
                   oRosterTransfer.RosterTransferID, oRosterTransfer.EmployeeID,
                   oRosterTransfer.ShiftID, oRosterTransfer.Date, oRosterTransfer.IsDayOff,
                   nUserID, nDBOperation);

        }

        #endregion

        #region Get & Exist Function

        public static IDataReader Get(int nShiftBNID, TransactionContext tc)
        {
            return tc.ExecuteReader("SELECT * FROM View_RosterTransfer WHERE ShiftBNID=%n", nShiftBNID);
        }
        public static IDataReader Get(string sSQL, TransactionContext tc)
        {
            return tc.ExecuteReader(sSQL);
        }
        public static IDataReader Gets(TransactionContext tc)
        {
            return tc.ExecuteReader("SELECT * FROM View_RosterTransfer ");
        }
        public static IDataReader Gets(string sSQL, TransactionContext tc)
        {
            return tc.ExecuteReader(sSQL);
        }
        public static IDataReader Gets(string EmployeeIDs, int ShiftID, DateTime StartDate, DateTime EndDate, TransactionContext tc)
        {
            return tc.ExecuteReader("EXEC [SP_Search_ShiftRostering] %s,%n,%d,%d",
                EmployeeIDs, ShiftID, StartDate, EndDate);
        }
        public static IDataReader Transfer(string EmployeeIDs, int ShiftID, DateTime StartDate, DateTime EndDate, Int64 nUserID, int nDBOperation,TransactionContext tc)
        {
            return tc.ExecuteReader("EXEC [SP_IUD_ShiftRostering_Transfer] %s,%n,%d,%d,%n,%n",
                EmployeeIDs, ShiftID, StartDate, EndDate, nUserID, nDBOperation);
        }
        public static IDataReader Swap(int RosterPlanID, DateTime StartDate, DateTime EndDate, Int64 nUserID, int nDBOperation, TransactionContext tc)
        {
            return tc.ExecuteReader("EXEC [SP_IUD_ShiftRostering_Swap] %n,%d,%d,%n,%n",
                 RosterPlanID, StartDate, EndDate, nUserID, nDBOperation);
        }
        #endregion
    }
}
