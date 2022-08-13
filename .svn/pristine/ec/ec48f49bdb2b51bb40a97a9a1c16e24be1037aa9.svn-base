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
    public class EmployeeRequestOnAttendanceDA
    {
        public EmployeeRequestOnAttendanceDA() { }

        #region Insert Update Delete Function

        public static IDataReader IUD(TransactionContext tc, EmployeeRequestOnAttendance oEmployeeRequestOnAttendance, Int64 nUserID, int nDBOperation)
        {
            return tc.ExecuteReader("EXEC [SP_IUD_EmployeeRequestOnAttendance]" + "%n, %n, %d, %n, %s, %n, %n",
                oEmployeeRequestOnAttendance.EROAID
                ,oEmployeeRequestOnAttendance.EmployeeID
                ,oEmployeeRequestOnAttendance.AttendanceDate
                ,oEmployeeRequestOnAttendance.IsOSD
                ,oEmployeeRequestOnAttendance.Remark
                , nUserID
                , nDBOperation
            );
        }
        public static IDataReader GetHierarchy(TransactionContext tc, string sEmployeeIDs)
        {
            return tc.ExecuteReader("EXEC [SP_GetHierarchyList]" + "%s,%s,%s,%b,%b,%s,%b", "EmployeeReportingPerson", "RPID", "EmployeeID", 1, 1, sEmployeeIDs, 1);
        }

        #endregion

        #region Get & Exist Function
        public static IDataReader Get(TransactionContext tc, string sSQL)
        {
            return tc.ExecuteReader(sSQL);
        }
        public static IDataReader Gets(string sSQL, TransactionContext tc)
        {
            return tc.ExecuteReader(sSQL);
        }


        #endregion
    }
}


