using ESimSol.BusinessObjects;
using ICS.Core.DataAccess;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace ESimSol.Services.DataAccess
{
    class EmployeeAdvanceSalaryDA
    {
        public static IDataReader InsertUpdate(TransactionContext tc, EmployeeAdvanceSalaryProcess oEmployeeAdvanceSalaryProcess, EnumDBOperation eEnumDBOperation, Int64 nUserID)
        {
            return tc.ExecuteReader("EXEC [SP_IUD_EmployeeAdvanceSalaryProcess] %n, %n, %n, %s, %d, %d, %d, %n, %n, %n",
                   oEmployeeAdvanceSalaryProcess.EASPID,
                   oEmployeeAdvanceSalaryProcess.BUID,
                   oEmployeeAdvanceSalaryProcess.LocationID,
                   oEmployeeAdvanceSalaryProcess.StartDate,
                   oEmployeeAdvanceSalaryProcess.EndDate,
                   oEmployeeAdvanceSalaryProcess.DeclarationDate,
                   oEmployeeAdvanceSalaryProcess.SalaryHeadID,
                   nUserID,
                   (int)eEnumDBOperation
                );
        }

        public static void Delete(TransactionContext tc, EmployeeAdvanceSalary oEmployeeAdvanceSalary, EnumDBOperation eEnumDBOperation, Int64 nUserID)
        {
            tc.ExecuteNonQuery("EXEC []");
        }


        public static IDataReader Gets(TransactionContext tc, string sSQL)
        {
            return tc.ExecuteReader(sSQL);
        }

        public static int EmployeeAdvanceSalarySave(TransactionContext tc, int nIndex, int EASPID,  Int64 nUserID)
        {
            int nNewIndex = 0;
            object oIndex = tc.ExecuteScalar("EXEC [SP_IUD_EmployeeAdvanceSalary] %n  ,%n  ,%n",
            nIndex, EASPID, nUserID);
            if (oIndex != null)
            {
                nNewIndex = Convert.ToInt32(oIndex);
            }
            return nNewIndex;
        }

        public static IDataReader Gets(TransactionContext tc)
        {
            return tc.ExecuteReader("SELECT * FROM View_EmployeeAdvanceSalary");
        }

        public static IDataReader Get(TransactionContext tc, string sSQL)
        {
            return tc.ExecuteReader(sSQL);
        }
        //public static IDataReader Get(TransactionContext tc, string sSQL)
        //{
        //    return tc.ExecuteReader(sSQL);
        //}
    }
}
