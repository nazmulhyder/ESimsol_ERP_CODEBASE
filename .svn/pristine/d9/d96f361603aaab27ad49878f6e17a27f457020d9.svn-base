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
    public class EmployeeBonusDA
    {
        public EmployeeBonusDA() { }

        public static IDataReader Process(TransactionContext tc, int SalaryHeadID, int Month, int Year, string Purpose, DateTime UptoDate, Int64 nUserID)
        {
            return tc.ExecuteReader("EXEC [SP_Process_EmployeeBonus] %n,%n,%n,%s,%d,%n",
                                       SalaryHeadID, Month,Year, Purpose, UptoDate, nUserID);
        }       
        public static IDataReader Delete(TransactionContext tc, string IDs, long nID)
        {
            return tc.ExecuteReader("DELETE FROM EmployeeBonus WHERE EBID IN(SELECT items FROM dbo.SplitInToDataSet(%s,','))", IDs);
        }
        public static IDataReader Gets(string sSQL, TransactionContext tc)
        {
            return tc.ExecuteReader(sSQL);
        }
        public static IDataReader Approve(TransactionContext tc, string sParams, long nID)
        {
            string sEBIDs = sParams.Split('~')[0];
            int ProcessMonth = Convert.ToInt32(sParams.Split('~')[1]);
            int ProcessYear = Convert.ToInt32(sParams.Split('~')[2]);
            string sEmployeeIDs = sParams.Split('~')[3];
            int nLocationID = Convert.ToInt32(sParams.Split('~')[4]);
            string sDepartmentIds = sParams.Split('~')[5];
            string sDesignationIds = sParams.Split('~')[6];
            int nCategory = Convert.ToInt32(sParams.Split('~')[7]);

            return tc.ExecuteReader("EXEC [SP_Approve_EmployeeBonus] %s,%n,%n,%n,%s,%s,%n,%n",
                           sEBIDs, ProcessMonth, ProcessYear, nLocationID, sDepartmentIds,sDesignationIds,nCategory, nID);

            //return tc.ExecuteReader("UPDATE EmployeeBonus SET Approveby=%n , ApproveDate=%d WHERE EBID IN(%s);SELECT * FROM View_EmployeeBonus WHERE EBID IN(%s)", nID, DateTime.Now, IDs, IDs);
        }
        public static bool IsExists(string sSQL, TransactionContext tc)
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
