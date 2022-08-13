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
    public class IncrementApprisalDA
    {
        public IncrementApprisalDA() { }

        #region Insert Update Delete Function

        public static IDataReader Search(TransactionContext tc, DateTime UpToDate, string EmpIDs, string BUIDs, string LocationIDs, string DeptIDs, string DesignationIDs, DateTime JoiningDate, bool IsMultipleMonth, string sMonths, string sYears, bool IsJoinDate, double minsalary, double maxsalary, string BlockIDs, string GroupIDs, Int64 nUserID)
        {
            return tc.ExecuteReader("EXEC [SP_Rpt_Incrementapprisal]" + "%s, %s, %s, %s, %s, %s,%d,%b,%s,%s,%b, %n,%n,%n, %s,%s"
                , UpToDate.ToString("dd MMM yyyy")
                , EmpIDs
                , BUIDs
                , LocationIDs
                , DeptIDs
                , DesignationIDs
                , JoiningDate
                , IsMultipleMonth
                , sMonths
                , sYears
                , IsJoinDate
                , minsalary
                , maxsalary
                , nUserID
                , BlockIDs
                , GroupIDs
            );
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

        #endregion
    }
}


