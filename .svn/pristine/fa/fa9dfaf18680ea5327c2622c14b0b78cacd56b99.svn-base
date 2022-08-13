using ESimSol.BusinessObjects;
using ICS.Core.DataAccess;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace ESimSol.Services.DataAccess
{
    class EmployeeAdvanceSalaryProcessDA
    {
        public static IDataReader InsertUpdate(TransactionContext tc, EmployeeAdvanceSalaryProcess oEmployeeAdvanceSalaryProcess, EnumDBOperation eEnumDBOperation, Int64 nUserID)
        {
            return tc.ExecuteReader("EXEC [SP_IUD_EmployeeAdvanceSalaryProcess]" 
                + "%n, %n, %n, %s, %d, %d, %d, %n, %n, %n ",
                 oEmployeeAdvanceSalaryProcess.EASPID,
                 oEmployeeAdvanceSalaryProcess.BUID,
                 oEmployeeAdvanceSalaryProcess.LocationID,
                 oEmployeeAdvanceSalaryProcess.Description,
                 oEmployeeAdvanceSalaryProcess.StartDate,
                 oEmployeeAdvanceSalaryProcess.EndDate,
                 oEmployeeAdvanceSalaryProcess.DeclarationDate,
                 oEmployeeAdvanceSalaryProcess.SalaryHeadID,
                 nUserID,
                 eEnumDBOperation);
        }

        public static void Delete(TransactionContext tc, EmployeeAdvanceSalaryProcess oEmployeeAdvanceSalaryProcess, EnumDBOperation eEnumDBOperation, Int64 nUserID)
        {
            tc.ExecuteNonQuery("EXEC [SP_IUD_EmployeeAdvanceSalaryProcess]"
                + "%n, %n, %n, %s, %d, %d, %d, %n, %n, %n ",
                 oEmployeeAdvanceSalaryProcess.EASPID,
                 oEmployeeAdvanceSalaryProcess.BUID,
                 oEmployeeAdvanceSalaryProcess.LocationID,
                 oEmployeeAdvanceSalaryProcess.Description,
                 oEmployeeAdvanceSalaryProcess.StartDate,
                 oEmployeeAdvanceSalaryProcess.EndDate,
                 oEmployeeAdvanceSalaryProcess.DeclarationDate,
                 oEmployeeAdvanceSalaryProcess.SalaryHeadID,
                 nUserID,
                 eEnumDBOperation);
        }


        public static IDataReader Gets(TransactionContext tc, string sSQL)
        {
            return tc.ExecuteReader(sSQL);
        }


        public static IDataReader Gets(TransactionContext tc)
        {
            return tc.ExecuteReader("SELECT * FROM View_EmployeeAdvanceSalaryProcess");
        }

        public static IDataReader Get(TransactionContext tc, string sSQL)
        {
            return tc.ExecuteReader(sSQL);
        }
        public static IDataReader Get(TransactionContext tc, int id)
        {
            return tc.ExecuteReader("SELECT * FROM View_EmployeeAdvanceSalaryProcess WHERE EASPID=%n", id);
        }
    }
}

