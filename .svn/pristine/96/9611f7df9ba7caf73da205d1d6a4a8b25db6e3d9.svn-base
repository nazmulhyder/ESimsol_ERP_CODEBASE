using System;
using System.Data;
using System.Data.SqlClient;
using ESimSol.BusinessObjects;
using System.Data.OleDb;
using ICS.Core.DataAccess;
using ICS.Core.Utility;

namespace ESimSol.Services.DataAccess
{
    public class SalaryFieldSetupDA
    {
        public SalaryFieldSetupDA() { }

        #region Insert Update Delete Function
        public static IDataReader InsertUpdate(TransactionContext tc, SalaryFieldSetup oSalaryFieldSetup, EnumDBOperation eEnumDBSalaryFieldSetup, Int64 nUserID)
        {
            return tc.ExecuteReader("EXEC [SP_IUD_SalaryFieldSetup]" + "%n, %s, %s, %n, %s, %n, %n",
                                    oSalaryFieldSetup.SalaryFieldSetupID, oSalaryFieldSetup.SetupNo, oSalaryFieldSetup.SalaryFieldSetupName, oSalaryFieldSetup.PageOrientationInt, oSalaryFieldSetup.Remarks, nUserID, (int)eEnumDBSalaryFieldSetup);
        }

        public static void Delete(TransactionContext tc, SalaryFieldSetup oSalaryFieldSetup, EnumDBOperation eEnumDBSalaryFieldSetup, Int64 nUserID)
        {
            tc.ExecuteNonQuery("EXEC [SP_IUD_SalaryFieldSetup]" + "%n, %s, %s, %n, %s, %n, %n",
                                    oSalaryFieldSetup.SalaryFieldSetupID, oSalaryFieldSetup.SetupNo, oSalaryFieldSetup.SalaryFieldSetupName, oSalaryFieldSetup.PageOrientationInt, oSalaryFieldSetup.Remarks, nUserID, (int)eEnumDBSalaryFieldSetup);
        }
        #endregion

        #region Get & Exist Function
        public static IDataReader Gets(TransactionContext tc)
        {
            return tc.ExecuteReader("SELECT * FROM View_SalaryFieldSetup ORDER BY SalaryFieldSetupID ASC");
        }

        public static IDataReader Get(TransactionContext tc, long nID)
        {
            return tc.ExecuteReader("SELECT * FROM View_SalaryFieldSetup WHERE SalaryFieldSetupID=%n", nID);
        }
        #endregion       
    }
}
