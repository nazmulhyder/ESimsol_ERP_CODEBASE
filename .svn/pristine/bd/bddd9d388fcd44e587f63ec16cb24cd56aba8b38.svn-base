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
    public class EmployeeSalaryV2DA
    {
        public static IDataReader CompGets(string sSQL, DateTime SalaryStartDate, DateTime SalaryEndDate, TransactionContext tc)
        {
            return tc.ExecuteReader("EXEC [SP_CompSalarySheet]" + "%s,%d,%d",
                                    sSQL, SalaryStartDate, SalaryEndDate);
        }

        public static IDataReader Gets(string sSQL,DateTime SalaryStartDate, DateTime SalaryEndDate, TransactionContext tc)
        {
            return tc.ExecuteReader("EXEC [SP_SalarySheet]" + "%s,%d,%d",
                                   sSQL, SalaryStartDate, SalaryEndDate);
        }
    }
}
