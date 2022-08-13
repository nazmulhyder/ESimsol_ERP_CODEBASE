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
    public class EmployeeBonusProcessDA
    {
        public EmployeeBonusProcessDA() { }

        #region Insert Update Delete Function

        public static IDataReader IUD(TransactionContext tc, EmployeeBonusProcess oEmployeeBonusProcess, Int64 nUserID, int nDBOperation)
        {
            return tc.ExecuteReader("EXEC [SP_IUD_EmployeeBonus] %n,%n,%n,%n,%b,%d,%d,%u,%n,%n,%n,%n",
                oEmployeeBonusProcess.EBPID,
                oEmployeeBonusProcess.BUID,
                oEmployeeBonusProcess.LocationID,
                oEmployeeBonusProcess.SalaryheadID,
                oEmployeeBonusProcess.IsEmployeeWise,
                oEmployeeBonusProcess.ProcessDate,
                oEmployeeBonusProcess.BonusDeclarationDate,
                oEmployeeBonusProcess.Note,
                oEmployeeBonusProcess.Year,
                oEmployeeBonusProcess.MonthID,
                nUserID,
                nDBOperation
                );
        }

        public static int ProcessBonus(TransactionContext tc, int nIndex, int nEBPID, string sEmployeeIDs, Int64 nUserID)
        {

            int nNewIndex = 0;
            object oIndex = tc.ExecuteScalar("EXEC [SP_Process_EmployeeBonus] %s,%n,%n,%n",
                sEmployeeIDs,
                nEBPID,
                nUserID,
                nIndex);
            if (oIndex != null)
            {
                nNewIndex = Convert.ToInt32(oIndex);
            }
            return nNewIndex;

        }

        #endregion

        #region Get & Exist Function
        public static IDataReader Get(TransactionContext tc, string sSQL)
        {
            return tc.ExecuteReader(sSQL);
        }
        public static IDataReader Gets(string sSQL, TransactionContext tc)
        {
            return tc.ExecuteReader(sSQL);
        }
        public static IDataReader Gets(TransactionContext tc)
        {
            return tc.ExecuteReader("SELECT * FROM EmployeeBonusProcess");
        }

        public static IDataReader Get(int nEBPID, TransactionContext tc)
        {
            return tc.ExecuteReader("SELECT * FROM EmployeeBonusProcess WHERE EBPID=%n", nEBPID);
        }

        public static bool CheckBonusProcess(string sSQL, TransactionContext tc)
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
    }
}


