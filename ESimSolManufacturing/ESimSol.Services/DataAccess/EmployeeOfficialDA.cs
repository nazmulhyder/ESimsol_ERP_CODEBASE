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
    public class EmployeeOfficialDA
    {
        public EmployeeOfficialDA() { }

        #region Insert Update Delete Function
        public static IDataReader IUD(TransactionContext tc, EmployeeOfficial oEO, int nEnumDBOperation, Int64 nUserID)
        {
            return tc.ExecuteReader("EXEC [SP_IUD_EmployeeOfficial]"
                                    + "%n,%n, %n, %n, %n, %n, %n, %d, %d, %b, %n ,%b,%n, %n",
                                    oEO.EmployeeOfficialID, oEO.EmployeeID, oEO.AttendanceSchemeID, oEO.DRPID, oEO.DesignationID, oEO.CurrentShiftID, oEO.WorkingStatus, oEO.DateOfJoin, oEO.DateOfConfirmation, oEO.IsActive, oEO.EmployeeTypeID,oEO.IsUser, nUserID, nEnumDBOperation);
        }


        #endregion

        #region Get & Exist Function
        public static IDataReader Get(TransactionContext tc, int nID)//EmployeeOfficialID
        {
            return tc.ExecuteReader("SELECT * FROM VIEW_EmployeeOfficial WHERE EmployeeOfficialID=%n", nID);
        }
        public static IDataReader GetByEmployee(TransactionContext tc, int nEmployeeID)
        {
            return tc.ExecuteReader("SELECT * FROM VIEW_EmployeeOfficialALL WHERE EmployeeID=%n", nEmployeeID);
        }

        public static IDataReader Gets(TransactionContext tc, int nEmployeeID)
        {
            return tc.ExecuteReader("SELECT * FROM EmployeeOfficial WHERE EmployeeID=%n", nEmployeeID);
        }
        public static IDataReader Gets(TransactionContext tc, string sSQL)
        {
            return tc.ExecuteReader(sSQL);
        }
        public static IDataReader Get(TransactionContext tc, string sSQL)
        {
            return tc.ExecuteReader(sSQL);
        }
        public static bool GetEmpConf(string sSQL, TransactionContext tc)
        {
            object obj = tc.ExecuteScalar(sSQL);
            if (obj == null)
            {
                return false;
            }
            else
            {
                int n = Convert.ToInt32(obj);
                if (n > 0) return true;
                else
                    return false;
            }
        }
        #endregion

    }
}
