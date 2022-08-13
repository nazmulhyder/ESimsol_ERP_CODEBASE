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
    public class EmployeeLeaveLedgerDA
    {
        public EmployeeLeaveLedgerDA() { }

        #region Insert Update Delete Function
        public static IDataReader IUD(TransactionContext tc, EmployeeLeaveLedger oELL, int nDBOperation, Int64 nUserID)
        {
            return tc.ExecuteReader("EXEC [SP_IUD_EmployeeLeaveLedger] %n, %n, %n, %n, %n, %n, %n, %b, %n, %b, %n, %n, %n, %b",
                                   oELL.EmpLeaveLedgerID, oELL.EmployeeID, oELL.ASLID, oELL.LeaveID, oELL.DeferredDay, oELL.ActivationAfter, oELL.TotalDay, oELL.IsLeaveOnPresence, oELL.PresencePerLeave, oELL.IsCarryForward, oELL.MaxCarryDays, nUserID, nDBOperation,oELL.IsComp);
        }
        public static IDataReader TransferLeave(TransactionContext tc, int nELIDFrom,int nELIDTo,int nDays,string sNote, Int64 nUserID)
        {
            return tc.ExecuteReader("EXEC [SP_TransferLeave] %n, %n, %n, %s, %n", nELIDFrom, nELIDTo, nDays, sNote, nUserID);
        }
        
        #endregion

        #region Get & Exist Function
        public static IDataReader Get(TransactionContext tc, int nEmployeeLeaveLedgerID)
        {
            return tc.ExecuteReader("SELECT * FROM View_EmployeeLeaveLedger WHERE EmpLeaveLedgerID=%n", nEmployeeLeaveLedgerID);
        }
        public static IDataReader GetsActiveLeaveLedger(TransactionContext tc, int nEmpID)
        {
            return tc.ExecuteReader("SELECT * FROM View_EmployeeLeaveLedger AS HH WHERE HH.EmployeeID= %n AND HH.ACSID IN (SELECT ACS.ACSID FROM AttendanceCalendarSession AS ACS WHERE ACS.IsActive = 1 AND ACS.ACSID IN (SELECT MM.ACSID FROM EmployeeLeaveLedger AS MM WHERE MM.EmployeeID= %n))", nEmpID, nEmpID);
        }
        public static IDataReader Gets(TransactionContext tc, int nEmpID)
        {
            return tc.ExecuteReader("Select * from View_EmployeeLeaveLedger Where EmployeeID= %n And ACSID=(Select Max(ACSID) from EmployeeLeaveLedger Where EmployeeID=%n)", nEmpID, nEmpID);
        }
        public static IDataReader Gets(TransactionContext tc, string sSQL)
        {
            return tc.ExecuteReader(sSQL);
        }


        #endregion
        
    }
}
