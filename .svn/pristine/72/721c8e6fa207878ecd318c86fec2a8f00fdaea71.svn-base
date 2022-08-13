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
    public class EmployeeReportingPersonDA
    {
        public EmployeeReportingPersonDA() { }

        #region Insert Update Delete Function
        public static IDataReader IUD(TransactionContext tc, EmployeeReportingPerson oEmployeeReportingPerson, int nDBOperation, Int64 nUserID)
        {
            return tc.ExecuteReader("EXEC [SP_IUD_EmployeeReportingPerson] %n,%n,%n,%d,%d,%b,%n,%n",
                                   oEmployeeReportingPerson.ERPID, oEmployeeReportingPerson.RPID, oEmployeeReportingPerson.EmployeeID, oEmployeeReportingPerson.StartDate, oEmployeeReportingPerson.EndDate,oEmployeeReportingPerson.IsActive, nUserID, nDBOperation);
        }
        #endregion

        public static IDataReader GetHierarchy(TransactionContext tc, string sEmployeeIDs)
        {
            return tc.ExecuteReader("EXEC [SP_GetHierarchyList]" + "%s,%s,%s,%b,%b,%s,%b", "EmployeeReportingPerson", "RPID", "EmployeeID", 1, 1, sEmployeeIDs, 1);
        }
        #region Get & Exist Function
        public static IDataReader Get(TransactionContext tc, int nERPID)
        {
            return tc.ExecuteReader("SELECT * FROM View_EmployeeReportingPerson WHERE ERPID=%n", nERPID);
        }
        public static IDataReader Gets(TransactionContext tc, int nEmpID)
        {
            return tc.ExecuteReader("SELECT * FROM View_EmployeeReportingPerson WHERE EmployeeID=%n", nEmpID);
        }
        public static IDataReader Gets(TransactionContext tc, string sSQL)
        {
            return tc.ExecuteReader(sSQL);
        }


        #endregion
    }
}
