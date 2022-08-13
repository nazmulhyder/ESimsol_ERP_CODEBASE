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
    public class WorkingHourDA
    {
        public WorkingHourDA() { }

        #region Get & Exist Function
        public static IDataReader GetsWorkingHour(string sParam,  Int64 nUserId,TransactionContext tc)
        {
            string sEmployeeIDs = sParam.Split('~')[0];
            string  sLocationID = sParam.Split('~')[1];
            string  DepartmentIds = sParam.Split('~')[2];
            string  DesignationIds = sParam.Split('~')[3];
            string SalarySchemeIDs = sParam.Split('~')[4];
            DateTime dtDateFrom = Convert.ToDateTime(sParam.Split('~')[5]);
            DateTime dtDateTo = Convert.ToDateTime(sParam.Split('~')[6]);
            string BusinessUnitIds = sParam.Split('~')[7];

            return tc.ExecuteReader("EXEC [SP_rpt_WorkingHour] %s,%s,%s,%s,%s,%s,%d,%d,%n",
            sEmployeeIDs, BusinessUnitIds, sLocationID, DepartmentIds, DesignationIds, SalarySchemeIDs, dtDateFrom, dtDateTo, nUserId);
        }

        #endregion
    }
}
