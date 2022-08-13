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
    public class BenefitOnAttendanceEmployeeAssignDA
    {
        public BenefitOnAttendanceEmployeeAssignDA() { }

        #region Insert Update Delete Function
        public static IDataReader InsertUpdate(TransactionContext tc, BenefitOnAttendanceEmployeeAssign oBOAEA, Int64 nUserId,  short eEnumDBOperation)
        {
            return tc.ExecuteReader("EXEC [SP_IUD_BenefitOnAttendanceEmployeeAssign]"
                                    + " %n, %n, %d, %d, %n, %n",
                                     oBOAEA.BOAEAID, oBOAEA.BOAEmployeeID, oBOAEA.StartDate, oBOAEA.EndDate, nUserId, eEnumDBOperation);
        }

        public static void Delete(TransactionContext tc, BenefitOnAttendanceEmployeeAssign oBOAEA,  Int64 nUserId,  short eEnumDBOperation)
        {
            tc.ExecuteNonQuery("EXEC [SP_IUD_BenefitOnAttendanceEmployeeAssign]"
                                    + " %n, %n, %d, %d, %n, %n",
                                     oBOAEA.BOAEAID, oBOAEA.BOAEmployeeID, oBOAEA.StartDate, oBOAEA.EndDate, nUserId, eEnumDBOperation);
        }
        #endregion

        #region Get & Exist Function
        public static IDataReader Get(TransactionContext tc, long nID)
        {
            return tc.ExecuteReader("SELECT * FROM BenefitOnAttendanceEmployeeAssign WHERE BOAEAID=%n", nID);
        }
        public static IDataReader Gets(TransactionContext tc, string sSQL)
        {
            return tc.ExecuteReader(sSQL);
        }
        #endregion
    }
}