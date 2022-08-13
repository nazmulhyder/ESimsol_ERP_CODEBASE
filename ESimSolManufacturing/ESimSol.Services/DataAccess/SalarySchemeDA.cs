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
    public class SalarySchemeDA
    {
        public SalarySchemeDA() { }

        #region Insert Update Delete Function

        public static IDataReader IUD(TransactionContext tc, SalaryScheme oSalaryScheme, Int64 nUserID, int nDBOperation)
        {
            return tc.ExecuteReader("EXEC [SP_IUD_SalaryScheme] %n,%s,%n,%n,%s,%b,%b,%s,%n,%n,%n,%b,%n,%n,%n,%n,%b,%n,%b,%n,%n,%n,%n,%n,%n,%n,%n,%n",
                   oSalaryScheme.SalarySchemeID, oSalaryScheme.Name,
                   (int)oSalaryScheme.NatureOfEmployee,
                   (int)oSalaryScheme.PaymentCycleInt, oSalaryScheme.Description,
                   oSalaryScheme.IsAllowBankAccount, oSalaryScheme.IsAllowOverTime,
                   (int)oSalaryScheme.OverTimeON, oSalaryScheme.DividedBy,
                   oSalaryScheme.MultiplicationBy, oSalaryScheme.FixedOTRatePerHour, oSalaryScheme.IsAttendanceDependant,
                   oSalaryScheme.LateCount,oSalaryScheme.EarlyLeavingCount,oSalaryScheme.FixedLatePenalty, oSalaryScheme.FixedEarlyLeavePenalty,
                   oSalaryScheme.IsProductionBase, oSalaryScheme.StartDay,oSalaryScheme.IsGratuity, oSalaryScheme.GraturityMaturedInYear,
                   oSalaryScheme.NoOfMonthCountOneYear, oSalaryScheme.GratuityApplyOn,
                   nUserID, nDBOperation,

                   (int)oSalaryScheme.CompOverTimeON,
                   oSalaryScheme.CompDividedBy,
                   oSalaryScheme.CompMultiplicationBy,
                   oSalaryScheme.CompFixedOTRatePerHour
                       
                   );
           
        }

        public static IDataReader Activity(int nSalarySchemeID, bool IsActive, TransactionContext tc)
        {

            return tc.ExecuteReader("UPDATE SalaryScheme SET IsActive=%b WHERE SalarySchemeID=%n;SELECT * FROM SalaryScheme WHERE SalarySchemeID=%n", IsActive, nSalarySchemeID, nSalarySchemeID);

        }
        #endregion

        #region Get & Exist Function

        public static IDataReader Get(int nSalarySchemeID, TransactionContext tc)
        {
            return tc.ExecuteReader("SELECT * FROM SalaryScheme WHERE SalarySchemeID=%n", nSalarySchemeID);
        }
        public static IDataReader Gets(TransactionContext tc)
        {
            return tc.ExecuteReader("SELECT * FROM SalaryScheme");
        }
        public static IDataReader Gets(string sSQL, TransactionContext tc)
        {
            return tc.ExecuteReader(sSQL);
        }

        public static bool IsAssigned(string sSQL, TransactionContext tc)
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
