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
    public class RPTSalarySheetDA
    {
        public RPTSalarySheetDA() { }

        #region Insert Update Delete Function


        #endregion

        #region Get & Exist Function

        public static IDataReader Get(int nEmployeeSalaryID, TransactionContext tc)
        {
            return tc.ExecuteReader("SELECT * FROM View_EmployeeSalary WHERE EmployeeSalaryID=%n", nEmployeeSalaryID);
        }
        public static IDataReader Get(string sSQL, TransactionContext tc)
        {
            return tc.ExecuteReader(sSQL);
        }
        public static IDataReader Gets(TransactionContext tc)
        {
            return tc.ExecuteReader("SELECT * FROM View_EmployeeSalary");
        }
        public static IDataReader Gets(string sSQL, TransactionContext tc)
        {
            return tc.ExecuteReader(sSQL);

        }
        public static IDataReader GetEmployeesSalary(TransactionContext tc, string sBU, string sLocationID, string sDepartmentIDs, string sDesignationIDs, string sSalarySchemeIDs, string sBlockIDs, string sGroupIDs, string sEmpIDs, int nMonthID, int nYear, bool bNewJoin, int IsOutSheet, double nStartSalaryRange, double nEndSalaryRange, bool IsCompliance, int nPayType, bool IsMacthExact, int BankAccID)
        {
            return tc.ExecuteReader("EXEC [SP_RPT_SalarySheet] " + "%s,%s,%s,%s,%s,%s,%s,%s,%n,%n,%b,%n,%n,%n,%b,%n,%b,%n",
                   sBU
                   , sLocationID
                   , sDepartmentIDs
                   , sDesignationIDs
                   , sSalarySchemeIDs
                   , sBlockIDs
                   , sGroupIDs
                   , sEmpIDs
                   , nMonthID
                   , nYear
                   , bNewJoin
                   , IsOutSheet
                   , nStartSalaryRange
                   , nEndSalaryRange
                   , IsCompliance
                   , nPayType
                   , IsMacthExact
                   , BankAccID
                   );

        }
        #endregion
    }
}

