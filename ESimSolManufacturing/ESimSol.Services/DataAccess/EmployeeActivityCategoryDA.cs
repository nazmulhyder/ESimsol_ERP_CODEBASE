using ESimSol.BusinessObjects;
using ICS.Core.DataAccess;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace ESimSol.Services.DataAccess
{
    class EmployeeActivityCategoryDA
    {
        #region Insert Update Delete Function
        public static IDataReader InsertUpdate(TransactionContext tc, EmployeeActivityCategory oEmployeeActivityCategory, EnumDBOperation eEnumDBOperation, Int64 nUserId)
        {
            return tc.ExecuteReader("EXEC [SP_IUD_EmployeeActivityCategory]" + "%n, %s, %n, %n", oEmployeeActivityCategory.EACID, oEmployeeActivityCategory.Description,
                                     nUserId, (int)eEnumDBOperation);
        }

        public static void Delete(TransactionContext tc, EmployeeActivityCategory oEmployeeActivityCategory, EnumDBOperation eEnumDBOperation, Int64 nUserId)
        {
            tc.ExecuteNonQuery("EXEC [SP_IUD_EmployeeActivityCategory]" + "%n, %s, %n, %n", oEmployeeActivityCategory.EACID, oEmployeeActivityCategory.Description,
                                     nUserId, (int)eEnumDBOperation);
        }
        #endregion


        public static IDataReader Gets(TransactionContext tc)
        {
            return tc.ExecuteReader("SELECT * FROM EmployeeActivityCategory");
        }

        public static IDataReader Gets(string sSQL, TransactionContext tc)
        {
            return tc.ExecuteReader(sSQL);
        }
    }
}
