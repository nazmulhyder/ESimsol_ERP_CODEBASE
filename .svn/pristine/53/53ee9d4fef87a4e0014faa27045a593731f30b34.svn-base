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
    public class BenefitOnAttendanceEmployeeStoppedDA
    {
        public BenefitOnAttendanceEmployeeStoppedDA() { }

        #region Insert Update Delete Function

        public static IDataReader InsertUpdate(TransactionContext tc, BenefitOnAttendanceEmployeeStopped oBOAEmployeeStopped, Int64 nUserID, int nDBOperation)
        {
            return tc.ExecuteReader("EXEC [SP_IUD_BenefitOnAttendanceEmployeeStopped]  %n,%n, %n,%n,%D,%D,%D,%b,%n,%n",
                   oBOAEmployeeStopped.BOAESID, oBOAEmployeeStopped.BOAEmployeeID, oBOAEmployeeStopped.BOAID,oBOAEmployeeStopped.EmployeeID,
                   oBOAEmployeeStopped.StartDate, oBOAEmployeeStopped.EndDate, oBOAEmployeeStopped.InactiveDate,oBOAEmployeeStopped.IsPermanent,
                   nUserID, nDBOperation);
        }

        public static void Delete(TransactionContext tc, BenefitOnAttendanceEmployeeStopped oBOAEmployeeStopped, Int64 nUserID, int nDBOperation)
        {
             tc.ExecuteNonQuery("EXEC [SP_IUD_BenefitOnAttendanceEmployeeStopped]  %n,%n, %n,%n,%D,%D,%D,%b,%n,%n",
                   oBOAEmployeeStopped.BOAESID, oBOAEmployeeStopped.BOAEmployeeID, oBOAEmployeeStopped.BOAID, oBOAEmployeeStopped.EmployeeID,
                   oBOAEmployeeStopped.StartDate, oBOAEmployeeStopped.EndDate, oBOAEmployeeStopped.InactiveDate, oBOAEmployeeStopped.IsPermanent,
                   nUserID, nDBOperation);
        }
        #endregion

        #region Get & Exist Function

        public static IDataReader Get(int nBOAEmployeeID, TransactionContext tc)
        {
            return tc.ExecuteReader("SELECT * FROM VIEW_BenefitOnAttendanceEmployeeStopped WHERE BOAEmployeeID=%n", nBOAEmployeeID);
        }
        public static IDataReader GetBy(int nEmployeeID, int nBOAID,  TransactionContext tc)
        {
            return tc.ExecuteReader("SELECT * FROM BenefitOnAttendanceEmployeeStopped where BOAEmployeeID in (SELECT BenefitOnAttendanceEmployee.BOAEmployeeID FROM BenefitOnAttendanceEmployee WHERE EmployeeID=%n and BOAID=%n)", nEmployeeID,  nBOAID);
        }
        public static IDataReader Gets(TransactionContext tc)
        {
            return tc.ExecuteReader("SELECT * FROM VIEW_BenefitOnAttendanceEmployeeStopped");
        }
        public static IDataReader Gets(string sSQL, TransactionContext tc)
        {
            return tc.ExecuteReader(sSQL);
        }


        #endregion
    }
}
