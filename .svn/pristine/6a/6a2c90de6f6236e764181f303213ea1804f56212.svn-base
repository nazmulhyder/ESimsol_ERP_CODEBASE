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
    public class LeaveLedgerDA
    {
        public LeaveLedgerDA() { }

        #region Get & Exist Function

        //public static int GetCurrentACSID(TransactionContext tc, int nEmployeeID)
        //{
        //    int nACSID = 0;
        //    string sSQL = "SELECT top(1) ACSID FROM AttendanceCalendarSession WHERE AttendanceCalendarID IN ("
        //                + " SELECT AttendanceCalendarID FROM AttendanceScheme WHERE AttendanceSchemeID IN ("
        //                + " SELECT AttendanceSchemeID FROM EmployeeOfficial WHERE EmployeeID=1)) "
        //                + " AND IsActive=1";
        //    object objAcsID = tc.ExecuteScalar(sSQL);
        //    if (objAcsID != null && objAcsID != "")
        //    {
        //        nACSID = Convert.ToInt32(objAcsID);
        //    }
        //    else {
        //        nACSID = 0;
        //    }
        //    return nACSID;
        //}
        //public static IDataReader Gets(TransactionContext tc, string sSQL)
        //{
        //    return tc.ExecuteReader(sSQL);
        //}

        public static IDataReader Gets(int nEmployeeID, int nACSID, TransactionContext tc)
        {
            return tc.ExecuteReader("EXEC [SP_Process_LeaveLedger] %n,%n", nEmployeeID, nACSID);
        }
        #endregion
    }
}
