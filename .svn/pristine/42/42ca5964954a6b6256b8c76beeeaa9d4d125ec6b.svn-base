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
    public class BenefitOnAttendanceEmployeeDA
    {
        public BenefitOnAttendanceEmployeeDA() { }

        #region Insert Update Delete Function

        public static IDataReader IUD(TransactionContext tc, BenefitOnAttendanceEmployee oBenefitOnAttendanceEmployee, Int64 nUserID, int nDBOperation)
        {
            return tc.ExecuteReader("EXEC [SP_IUD_BenefitOnAttendanceEmployee] %n,%n,%n,%d,%b,%n,%n",
                   oBenefitOnAttendanceEmployee.BOAEmployeeID, oBenefitOnAttendanceEmployee.BOAID,
                   oBenefitOnAttendanceEmployee.EmployeeID, oBenefitOnAttendanceEmployee.InactiveDate,
                   oBenefitOnAttendanceEmployee.IsTemporaryAssign,
                   nUserID, nDBOperation);
        }

        #endregion

        #region Get & Exist Function

        public static IDataReader Get(int nBOAEmployeeID, TransactionContext tc)
        {
            return tc.ExecuteReader("SELECT * FROM View_BenefitOnAttendanceEmployee WHERE BOAEmployeeID=%n", nBOAEmployeeID);
        }
        public static IDataReader Get(string sSQL, TransactionContext tc)
        {
            return tc.ExecuteReader(sSQL);
        }
        public static IDataReader Gets(TransactionContext tc,int nEmployeeID)
        {
            return tc.ExecuteReader("SELECT * FROM View_BenefitOnAttendanceEmployee where EmployeeID=%n", nEmployeeID);
        }
        public static IDataReader Gets(string sSQL, TransactionContext tc)
        {
            return tc.ExecuteReader(sSQL);
        }


        #endregion
    }
}
