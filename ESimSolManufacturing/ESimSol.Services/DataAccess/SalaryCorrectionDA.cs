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
    public class SalaryCorrectionDA
    {
        public SalaryCorrectionDA() { }

        #region Insert Update Delete Function

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
        public static IDataReader GetsReason(string sBU, string sLocationID, int nMonthID, int nYear, int nRowLength, int nLoadRecords, string sEmployeeIDs, bool bIsCallFromExcel, TransactionContext tc)
        {
            return tc.ExecuteReader("EXEC [SP_FindErrorOnProcessedSalary]" + "%s, %s, %n, %n, %n, %n, %s, %b"
                , sBU
                , sLocationID
                , nMonthID
                , nYear
                , nRowLength
                , nLoadRecords
                , sEmployeeIDs
                , bIsCallFromExcel
            );
        }
        


        #endregion
    }
}

