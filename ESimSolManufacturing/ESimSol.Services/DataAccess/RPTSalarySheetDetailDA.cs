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
    public class RPTSalarySheetDetailDA
    {
        public RPTSalarySheetDetailDA() { }

        #region Insert Update Delete Function


        #endregion

        #region Get & Exist Function

        public static IDataReader Get(int nEmployeeSalaryDetailID, TransactionContext tc)
        {
            return tc.ExecuteReader("SELECT * FROM View_EmployeeSalaryDetail WHERE EmployeeSalaryDetailID=%n", nEmployeeSalaryDetailID);
        }
        public static IDataReader Get(string sSQL, TransactionContext tc)
        {
            return tc.ExecuteReader(sSQL);
        }
        public static IDataReader Gets(TransactionContext tc)
        {
            return tc.ExecuteReader("SELECT * FROM View_EmployeeSalaryDetail");
        }
        public static IDataReader Gets(string sSQL, TransactionContext tc)
        {
            return tc.ExecuteReader(sSQL);
        }
        public static IDataReader GetEmployeesSalaryDetail(TransactionContext tc, string sBU, string sLocationID, string sDepartmentIDs, string sDesignationIDs
            , string sSalarySchemeIDs, string sBlockIDs, string sGroupIDs, string sEmpIDs, int nMonthID, int nYear, bool bNewJoin, int IsOutSheet
            , double nStartSalaryRange, double nEndSalaryRange, bool IsCompliance)
        {
            return tc.ExecuteReader("EXEC [SP_RPT_SalarySheetDetail] " + "%s,%s,%s,%s,%s,%s,%s,%s,%n,%n,%b,%n,%n,%n,%b",
                   sBU
                   ,sLocationID
                   ,sDepartmentIDs
                   ,sDesignationIDs
                   ,sSalarySchemeIDs
                   ,sBlockIDs
                   ,sGroupIDs
                   ,sEmpIDs
                   ,nMonthID
                   ,nYear
                   ,bNewJoin
                   ,IsOutSheet
                   ,nStartSalaryRange
                   ,nEndSalaryRange
                   ,IsCompliance
                   );

        }

        #endregion
    }
}
