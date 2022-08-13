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
    public class EarnLeaveDA
    {
        public EarnLeaveDA() { }

        #region Insert Update Delete Function
        public static IDataReader ELProcess(TransactionContext tc, int EmpLeaveLedgerID, DateTime LastProcessDate, Int64 nUserID)
        {
            return tc.ExecuteReader("EXEC [SP_Process_EL] %n,%d", EmpLeaveLedgerID, LastProcessDate);
        }
        #endregion

        #region Get & Exist Function
        public static IDataReader ELGets(TransactionContext tc, string EmployeeIDs, string sLocationID, string DepartmentIDs, string DesignationIDs, DateTime Date, string sBUnit, int nLoadRecordsS, int nRowLengthS, int isAll, Int64 nUserID)
        {
            return tc.ExecuteReader("EXEC [SP_RPT_ELLedger] %s,%s,%s,%s,%d,%s,%n, %n, %n,%n", EmployeeIDs, sLocationID, DepartmentIDs, DesignationIDs, Date, sBUnit, nUserID, nLoadRecordsS, nRowLengthS, isAll);
        }
        public static IDataReader ELGetsToEncash(TransactionContext tc, string sBusinessUnitIds, string sEmployeeIDs, string sLocationID, string sDepartmentIds, string sDesignationIDs, int nACSID, double nStartSalaryRange, double nEndSalaryRange, int nLoadRecordsS, int nRowLengthS, Int64 nUserID)
        {
            return tc.ExecuteReader("EXEC [SP_RPT_ELGetsToEncash] %s,%s,%s,%s,%s,%n,%n,%n,%n,%n,%n", sBusinessUnitIds, sLocationID, sDepartmentIds, sDesignationIDs,sEmployeeIDs, nACSID, nStartSalaryRange, nEndSalaryRange, nUserID, nLoadRecordsS, nRowLengthS);
        }
        //public static IDataReader EncashEL(TransactionContext tc, int nIndex, string sEmpLeaveLedgerIDs, DateTime DeclarationDate, int nACSID, int nConsiderEL, bool IsApplyforallemployee, Int64 nUserID)
        //{
        //    return tc.ExecuteReader("EXEC [SP_Process_EncashEL] %s,%d,%n,%n,%b,%n", sEmpLeaveLedgerIDs, DeclarationDate, nACSID, nConsiderEL, IsApplyforallemployee, nUserID);
        //}

        public static int EncashEL(TransactionContext tc, int nIndex, string sEmpLeaveLedgerIDs, DateTime DeclarationDate, int nACSID, int nConsiderEL, bool IsApplyforallemployee, bool IsEncashPresentBalance, Int64 nUserID)
        {
            int nNewIndex = 0;
            //object oIndex = tc.ExecuteScalar("EXEC [SP_Process_Payroll_V1] %n,%n,%d,%d,%s,%n,%n",
            object oIndex = tc.ExecuteScalar("EXEC [SP_Process_EncashEL] %n,%s,%d,%n,%n,%b, %b,%n",
            nIndex, sEmpLeaveLedgerIDs, DeclarationDate, nACSID, nConsiderEL, IsApplyforallemployee, IsEncashPresentBalance, nUserID);
            if (oIndex != null)
            {
                nNewIndex = Convert.ToInt32(oIndex);
            }
            return nNewIndex;
        }


        public static IDataReader ELGetsClassified(TransactionContext tc, string sSQL)
        {
            return tc.ExecuteReader(sSQL);
        }

        #endregion
    }
}
