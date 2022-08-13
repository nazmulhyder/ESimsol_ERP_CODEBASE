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
    public class WorkingHourReportDA
    {
        public WorkingHourReportDA() { }

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
        public static IDataReader Gets(DateTime dDate, string sBusinessUnitIds, string sLocationID, string sDepartmentIds, string sDesignationIds, string sEmployeeIDs, string sShiftIDs, Int64 nUserID, TransactionContext tc)
        {
            return tc.ExecuteReader("EXEC [SP_Rpt_WorkingHourReport] %d,%s,%s,%s,%s,%s,%s,%n",
                  dDate, sBusinessUnitIds, sLocationID, sDepartmentIds, sDesignationIds, sEmployeeIDs, sShiftIDs, nUserID);
        }


        #endregion
    }
}

