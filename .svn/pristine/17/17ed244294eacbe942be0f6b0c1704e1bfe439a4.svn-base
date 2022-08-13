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
    public class PayrollProcessManagementDA
    {
        public PayrollProcessManagementDA() { }

        #region Insert Update Delete Function

        public static IDataReader IUD(TransactionContext tc, PayrollProcessManagement oPayrollProcessManagement, Int64 nUserID)
        {
            return tc.ExecuteReader("EXEC [SP_IUD_PayrollProcessManagement] %n,%n,%n,%n,%n,%n,%d,%d,%d,%n,%n,%n",
                   oPayrollProcessManagement.PPMID, oPayrollProcessManagement.CompanyID,oPayrollProcessManagement.BusinessUnitID,
                   oPayrollProcessManagement.LocationID,(int) oPayrollProcessManagement.Status,
                   (int)oPayrollProcessManagement.PaymentCycle, oPayrollProcessManagement.ProcessDate,
                   oPayrollProcessManagement.SalaryFrom, oPayrollProcessManagement.SalaryTo, nUserID
                   , oPayrollProcessManagement.MonthID, oPayrollProcessManagement.BankAccountID);

        }



        public static void ProcessPayroll(TransactionContext tc, int nSalarySchemeID, int nLocationID, DateTime dStartDate, DateTime dEndDate, string sAllowanceIDs, int nPayRollProcessID, Int64 nUserID)
        {            
            tc.ExecuteNonQuery("EXEC [SP_Process_Payroll] %n,%n,%d,%d,%s,%n,%n",
            nSalarySchemeID,nLocationID,dStartDate,dEndDate,sAllowanceIDs,
            nPayRollProcessID,nUserID);
        }

        //public static IDataReader ProcessPayrollByEmployee(TransactionContext tc, int nEmployeeID, int nLocationID, DateTime dStartDate, DateTime dEndDate, string sAllowanceIDs, int nPayRollProcessID, Int64 nUserID)
        //{
        //    return tc.ExecuteReader("EXEC [SP_Process_Payroll_V1] %n,%n,%d,%d,%s,%n,%n",
        //    nEmployeeID, nLocationID, dStartDate, dEndDate, sAllowanceIDs,
        //    nPayRollProcessID, nUserID);
        //}

        public static int ProcessPayrollByEmployee(TransactionContext tc, int nIndex, int nLocationID, DateTime dStartDate, DateTime dEndDate, string sAllowanceIDs, int nPayRollProcessID, Int64 nUserID)
        {
            int nNewIndex = 0;
            //object oIndex = tc.ExecuteScalar("EXEC [SP_Process_Payroll_V1] %n,%n,%d,%d,%s,%n,%n",
            object oIndex = tc.ExecuteScalar("EXEC [SP_Process_Payroll_Corporate] %n,%n,%d,%d,%s,%n,%n",
            nIndex, nLocationID, dStartDate, dEndDate, sAllowanceIDs,
            nPayRollProcessID, nUserID);
            if (oIndex != null)
            {
                nNewIndex = Convert.ToInt32(oIndex);
            }
            return nNewIndex;
        }

        public static IDataReader Delete(TransactionContext tc, int nPPMID)
        {
            return tc.ExecuteReader("EXEC [SP_Process_Payroll_Delete] %n",nPPMID);
        }

        public static int ProcessPayroll_Mamiya(TransactionContext tc, int nIndex, int nPayRollProcessID, string sEmployeeIDs, Int64 nUserID)
        {
            int nNewIndex = 0;
            object oIndex = tc.ExecuteScalar("EXEC [SP_Process_Payroll_Mamiya] %n  ,%n  ,%s  ,%n",
            nIndex, nPayRollProcessID, sEmployeeIDs, nUserID);
            if (oIndex != null)
            {
                nNewIndex = Convert.ToInt32(oIndex);
            }
            return nNewIndex;
        }

        public static int ProcessPayroll_Corporate(TransactionContext tc, int nIndex, int nPayRollProcessID, string sEmployeeIDs, Int64 nUserID)
        {
            int nNewIndex = 0;
            object oIndex = tc.ExecuteScalar("EXEC [SP_Process_Payroll_Corporate_V1] %n  ,%n  ,%s  ,%n",
            nIndex, nPayRollProcessID, sEmployeeIDs, nUserID);
            if (oIndex != null)
            {
                nNewIndex = Convert.ToInt32(oIndex);
            }
            return nNewIndex;
        }

        public static int ProcessPayroll_Corporate_Discontinue(TransactionContext tc, int nIndex, int nPayRollProcessID, string sEmployeeIDs, Int64 nUserID)
        {
            int nNewIndex = 0;
            object oIndex = tc.ExecuteScalar("EXEC [SP_Process_Payroll_Corporate_Discontinue] %n  ,%n  ,%s  ,%n",
            nIndex, nPayRollProcessID, sEmployeeIDs, nUserID);
            if (oIndex != null)
            {
                nNewIndex = Convert.ToInt32(oIndex);
            }
            return nNewIndex;
        }

        public static int ProcessPayroll_Production(TransactionContext tc, int nIndex, int nPayRollProcessID, string sEmployeeIDs, Int64 nUserID)
        {
            int nNewIndex = 0;
            object oIndex = tc.ExecuteScalar("EXEC [SP_Process_Payroll_Production] %n  ,%n  ,%s  ,%n",
            nIndex, nPayRollProcessID, sEmployeeIDs, nUserID);
            if (oIndex != null)
            {
                nNewIndex = Convert.ToInt32(oIndex);
            }
            return nNewIndex;
        }
        public static IDataReader SettlementSalaryProcess(int nEmployeeSettlementID, Int64 nUserID, TransactionContext tc)
        {
            return tc.ExecuteReader("EXEC [SP_Process_Payroll_Mamiya_Settlement] %n,%n",
            nEmployeeSettlementID,nUserID);
        }
        public static IDataReader SettlementSalaryProcess_Corporate(int nEmployeeSettlementID, Int64 nUserID, TransactionContext tc)
        {
            return tc.ExecuteReader("EXEC [SP_Process_Payroll_Settlement_Corporate] %n,%n",
            nEmployeeSettlementID, nUserID);
        }


        public static IDataReader PPM_Unfreeze(int nPayrollProcessManagementID, TransactionContext tc)
        {
            tc.ExecuteNonQuery("UPDATE PayrollProcessManagement SET [Status]=1 WHERE PPMID=%n", nPayrollProcessManagementID);
            return tc.ExecuteReader("SELECT * FROM View_PayrollProcessManagement WHERE PPMID=%n", nPayrollProcessManagementID);
        }
        #endregion


        public static IDataReader CheckPPM(int nEmployeeID, int nMonthID, int nYear, TransactionContext tc)
        {
            return tc.ExecuteReader("EXEC [SP_CheckPPM]" + "%n, %n, %n"
                , nEmployeeID
                , nMonthID
                , nYear
            );
        }
        public static int CheckPayrollProcessForIndex(string sSQL, TransactionContext tc)
        {
            int index = 0;
            object obj = tc.ExecuteScalar(sSQL);
            if (obj != null)
            {
                index = Convert.ToInt32(obj);
            }
            return index;
        }

        #region Get & Exist Function

        public static IDataReader Get(int nPayrollProcessManagementID, TransactionContext tc)
        {
            return tc.ExecuteReader("SELECT * FROM View_PayrollProcessManagement WHERE PPMID=%n", nPayrollProcessManagementID);
        }
        public static IDataReader Gets(TransactionContext tc)
        {
            return tc.ExecuteReader("SELECT * FROM View_PayrollProcessManagement");
        }
        public static IDataReader Gets(string sSQL, TransactionContext tc)
        {
            return tc.ExecuteReader(sSQL);
        }

        public static bool CheckPayrollProcess(string sSQL, TransactionContext tc)
        {
            object obj = tc.ExecuteScalar(sSQL);
            if (obj == null)
            {
                return false;
            }
            else
            {
                int n = Convert.ToInt32(obj);
                if (n > 0) return true;
                else
                    return false;
            }
        }

        #endregion









        public static IDataReader IUDComp(TransactionContext tc, PayrollProcessManagement oPayrollProcessManagement, Int64 nUserID)
        {
            return tc.ExecuteReader("EXEC [SP_IUD_CompliancePayrollProcessManagement] %n,%n,%n,%n,%n,%n,%d,%d,%d,%n,%n,%n,%n",
                   oPayrollProcessManagement.PPMID, oPayrollProcessManagement.CompanyID,oPayrollProcessManagement.BusinessUnitID,
                   oPayrollProcessManagement.LocationID,(int) oPayrollProcessManagement.Status,
                   (int)oPayrollProcessManagement.PaymentCycle, oPayrollProcessManagement.ProcessDate,
                   oPayrollProcessManagement.SalaryFrom, oPayrollProcessManagement.SalaryTo, nUserID
                   , oPayrollProcessManagement.MonthID, oPayrollProcessManagement.BankAccountID, oPayrollProcessManagement.MOCID);

        }

        public static void ProcessPayrollComp(TransactionContext tc, int nSalarySchemeID, int nLocationID, DateTime dStartDate, DateTime dEndDate, string sAllowanceIDs, int nPayRollProcessID, Int64 nUserID)
        {            
            tc.ExecuteNonQuery("EXEC [SP_Process_Payroll] %n,%n,%d,%d,%s,%n,%n",
            nSalarySchemeID,nLocationID,dStartDate,dEndDate,sAllowanceIDs,
            nPayRollProcessID,nUserID);
        }

        //public static IDataReader ProcessPayrollByEmployee(TransactionContext tc, int nEmployeeID, int nLocationID, DateTime dStartDate, DateTime dEndDate, string sAllowanceIDs, int nPayRollProcessID, Int64 nUserID)
        //{
        //    return tc.ExecuteReader("EXEC [SP_Process_Payroll_V1] %n,%n,%d,%d,%s,%n,%n",
        //    nEmployeeID, nLocationID, dStartDate, dEndDate, sAllowanceIDs,
        //    nPayRollProcessID, nUserID);
        //}

        public static int ProcessPayrollByEmployeeComp(TransactionContext tc, int nIndex, int nLocationID, DateTime dStartDate, DateTime dEndDate, string sAllowanceIDs, int nPayRollProcessID, Int64 nUserID)
        {
            int nNewIndex = 0;
            //object oIndex = tc.ExecuteScalar("EXEC [SP_Process_Payroll_V1] %n,%n,%d,%d,%s,%n,%n",
            object oIndex = tc.ExecuteScalar("EXEC [SP_Process_Payroll_Corporate] %n,%n,%d,%d,%s,%n,%n",
            nIndex, nLocationID, dStartDate, dEndDate, sAllowanceIDs,
            nPayRollProcessID, nUserID);
            if (oIndex != null)
            {
                nNewIndex = Convert.ToInt32(oIndex);
            }
            return nNewIndex;
        }

        public static IDataReader DeleteComp(TransactionContext tc, int nPPMID)
        {
            return tc.ExecuteReader("EXEC [SP_Process_Compliance_Payroll_Delete] %n",nPPMID);
        }

        public static int ProcessPayroll_MamiyaComp(TransactionContext tc, int nIndex, int nPayRollProcessID, string sEmployeeIDs, Int64 nUserID)
        {
            int nNewIndex = 0;
            object oIndex = tc.ExecuteScalar("EXEC [SP_Process_Payroll_Mamiya] %n  ,%n  ,%s  ,%n",
            nIndex, nPayRollProcessID, sEmployeeIDs, nUserID);
            if (oIndex != null)
            {
                nNewIndex = Convert.ToInt32(oIndex);
            }
            return nNewIndex;
        }

        public static int ProcessPayroll_CorporateComp(TransactionContext tc, int nIndex, int nPayRollProcessID, string sEmployeeIDs, Int64 nUserID)
        {
            int nNewIndex = 0;
            object oIndex = tc.ExecuteScalar("EXEC [SP_Process_Comp_Payroll_Corporate_V1] %n  ,%n  ,%s  ,%n",
            nIndex, nPayRollProcessID, sEmployeeIDs, nUserID);
            if (oIndex != null)
            {
                nNewIndex = Convert.ToInt32(oIndex);
            }
            return nNewIndex;
        }

        public static int ProcessPayroll_Corporate_DiscontinueComp(TransactionContext tc, int nIndex, int nPayRollProcessID, string sEmployeeIDs, Int64 nUserID)
        {
            int nNewIndex = 0;
            object oIndex = tc.ExecuteScalar("EXEC [SP_Process_Payroll_Corporate_Discontinue] %n  ,%n  ,%s  ,%n",
            nIndex, nPayRollProcessID, sEmployeeIDs, nUserID);
            if (oIndex != null)
            {
                nNewIndex = Convert.ToInt32(oIndex);
            }
            return nNewIndex;
        }

        public static int ProcessPayroll_ProductionComp(TransactionContext tc, int nIndex, int nPayRollProcessID, string sEmployeeIDs, Int64 nUserID)
        {
            int nNewIndex = 0;
            object oIndex = tc.ExecuteScalar("EXEC [SP_Process_Payroll_Production] %n  ,%n  ,%s  ,%n",
            nIndex, nPayRollProcessID, sEmployeeIDs, nUserID);
            if (oIndex != null)
            {
                nNewIndex = Convert.ToInt32(oIndex);
            }
            return nNewIndex;
        }
        public static IDataReader SettlementSalaryProcessComp(int nEmployeeSettlementID, Int64 nUserID, TransactionContext tc)
        {
            return tc.ExecuteReader("EXEC [SP_Process_Payroll_Mamiya_Settlement] %n,%n",
            nEmployeeSettlementID,nUserID);
        }
        public static IDataReader SettlementSalaryProcess_CorporateComp(int nEmployeeSettlementID, Int64 nUserID, TransactionContext tc)
        {
            return tc.ExecuteReader("EXEC [SP_Process_Payroll_Settlement_Corporate] %n,%n",
            nEmployeeSettlementID, nUserID);
        }


        public static IDataReader PPM_UnfreezeComp(int nPayrollProcessManagementID, TransactionContext tc)
        {
            tc.ExecuteNonQuery("UPDATE PayrollProcessManagement SET [Status]=1 WHERE PPMID=%n", nPayrollProcessManagementID);
            return tc.ExecuteReader("SELECT * FROM View_PayrollProcessManagement WHERE PPMID=%n", nPayrollProcessManagementID);
        }

        public static IDataReader CheckPPMComp(int nEmployeeID, int nMonthID, int nYear, TransactionContext tc)
        {
            return tc.ExecuteReader("EXEC [SP_CheckPPM]" + "%n, %n, %n"
                , nEmployeeID
                , nMonthID
                , nYear
            );
        }
        public static int CheckPayrollProcessForIndexComp(string sSQL, TransactionContext tc)
        {
            int index = 0;
            object obj = tc.ExecuteScalar(sSQL);
            if (obj != null)
            {
                index = Convert.ToInt32(obj);
            }
            return index;
        }

        public static IDataReader GetComp(int nPayrollProcessManagementID, TransactionContext tc)
        {
            return tc.ExecuteReader("SELECT * FROM View_CompliancePayrollProcessManagement WHERE PPMID=%n", nPayrollProcessManagementID);
        }
        public static IDataReader GetsComp(TransactionContext tc)
        {
            return tc.ExecuteReader("SELECT * FROM View_CompliancePayrollProcessManagement");
        }
        public static IDataReader GetsComp(string sSQL, TransactionContext tc)
        {
            return tc.ExecuteReader(sSQL);
        }

        public static bool CheckPayrollProcessComp(string sSQL, TransactionContext tc)
        {
            object obj = tc.ExecuteScalar(sSQL);
            if (obj == null)
            {
                return false;
            }
            else
            {
                int n = Convert.ToInt32(obj);
                if (n > 0) return true;
                else
                    return false;
            }
        }

    }
}
