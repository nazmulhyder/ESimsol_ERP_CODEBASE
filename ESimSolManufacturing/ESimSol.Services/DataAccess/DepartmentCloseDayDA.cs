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
    public class DepartmentCloseDayDA
    {
        #region Insert Update Delete Function

        public static IDataReader InsertUpdate(TransactionContext tc, DepartmentCloseDay oDepartmentCloseDay, EnumDBOperation eEnumDBOperation, string sIDs)
        {
            return tc.ExecuteReader("EXEC [SP_IUD_DepartmentCloseDay] %n, %n, %s, %n, %s", oDepartmentCloseDay.DepartmentCloseDayID, oDepartmentCloseDay.DepartmentRequirementPolicyID, oDepartmentCloseDay.WeekDay, (int)eEnumDBOperation, sIDs);
        }
        public static void Delete(TransactionContext tc, DepartmentCloseDay oDepartmentCloseDay, EnumDBOperation eEnumDBOperation, string sIDs)
        {
            tc.ExecuteNonQuery("EXEC [SP_IUD_DepartmentCloseDay] %n, %n, %s, %n, %s", oDepartmentCloseDay.DepartmentCloseDayID, oDepartmentCloseDay.DepartmentRequirementPolicyID, oDepartmentCloseDay.WeekDay, (int)eEnumDBOperation, sIDs);
        }

        #endregion

        #region Get & Exist Function
        public static IDataReader Get(TransactionContext tc, long nID)
        {
            return tc.ExecuteReader("SELECT * FROM DepartmentCloseDay WHERE DepartmentCloseDayID=%n", nID);
        }

        public static IDataReader Gets(TransactionContext tc)
        {
            return tc.ExecuteReader("SELECT * FROM DepartmentCloseDay");
        }
        public static IDataReader Gets(TransactionContext tc, int nDepartmentRequirementPolicyID)
        {
            return tc.ExecuteReader("SELECT * FROM DepartmentCloseDay WHERE DepartmentRequirementPolicyID=%n", nDepartmentRequirementPolicyID);
        }
        public static IDataReader Gets(TransactionContext tc, string sSQL)
        {
            return tc.ExecuteReader(sSQL);
        }
        #endregion
    }
}
