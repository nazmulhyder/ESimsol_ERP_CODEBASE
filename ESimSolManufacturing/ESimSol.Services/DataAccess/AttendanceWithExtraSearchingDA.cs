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
    public class AttendanceWithExtraSearchingDA
    {
        public AttendanceWithExtraSearchingDA() { }

        #region Get

        public static IDataReader Gets(string sParam, Int64 nUserID, TransactionContext tc)
        {
            DateTime StartDate=Convert.ToDateTime(sParam.Split('~')[0]);
            DateTime EndDate=Convert.ToDateTime(sParam.Split('~')[1]);
            string sBusinessUnitIds=sParam.Split('~')[2];
            string sLocationID=sParam.Split('~')[3];
            string sDepartmentIds=sParam.Split('~')[4];
            string sDesignationIds=sParam.Split('~')[5];
            string sSalarySchemeIDs=sParam.Split('~')[6];
            string sEmployeeIDs=sParam.Split('~')[7];
            int CriteriaType = Convert.ToInt32(sParam.Split('~')[8]);
            DateTime StartInTime=Convert.ToDateTime(sParam.Split('~')[9]);
            DateTime EndInTime=Convert.ToDateTime(sParam.Split('~')[10]);
            DateTime StartOutTime=Convert.ToDateTime(sParam.Split('~')[11]);
            DateTime EndOutTime=Convert.ToDateTime(sParam.Split('~')[12]);
            int StartLate=Convert.ToInt32(sParam.Split('~')[13]);
            int EndLate=Convert.ToInt32(sParam.Split('~')[14]);
            int StartEarly=Convert.ToInt32(sParam.Split('~')[15]);
            int EndEarly=Convert.ToInt32(sParam.Split('~')[16]);
            int StartDaysCount=Convert.ToInt32(sParam.Split('~')[17]);
            int EndDaysCount = Convert.ToInt32(sParam.Split('~')[18]);

            return tc.ExecuteReader("EXEC [SP_Rpt_AttendanceWithExtraSearching] %s,%s,%s,%s,%s,%s,%s,%s,%n,%D,%D,%D,%D,%n,%n,%n,%n,%n,%n,%n",
            StartDate.ToString("dd MMM yyyy"),
            EndDate.ToString("dd MMM yyyy"),
            sBusinessUnitIds,
            sLocationID,
            sDepartmentIds,
            sDesignationIds,
            sSalarySchemeIDs,
            sEmployeeIDs,
            CriteriaType ,
            StartInTime,
            EndInTime,
            StartOutTime,
            EndOutTime,
            StartLate,
            EndLate,
            StartEarly,
            EndEarly,
            StartDaysCount,
            EndDaysCount,
            nUserID
          );
        }
        public static IDataReader GetsComp(string sParam, Int64 nUserID, TransactionContext tc)
        {
            DateTime StartDate = Convert.ToDateTime(sParam.Split('~')[0]);
            DateTime EndDate = Convert.ToDateTime(sParam.Split('~')[1]);
            string sBusinessUnitIds = sParam.Split('~')[2];
            string sLocationID = sParam.Split('~')[3];
            string sDepartmentIds = sParam.Split('~')[4];
            string sDesignationIds = sParam.Split('~')[5];
            string sSalarySchemeIDs = sParam.Split('~')[6];
            string sEmployeeIDs = sParam.Split('~')[7];
            int CriteriaType = Convert.ToInt32(sParam.Split('~')[8]);
            DateTime StartInTime = Convert.ToDateTime(sParam.Split('~')[9]);
            DateTime EndInTime = Convert.ToDateTime(sParam.Split('~')[10]);
            DateTime StartOutTime = Convert.ToDateTime(sParam.Split('~')[11]);
            DateTime EndOutTime = Convert.ToDateTime(sParam.Split('~')[12]);
            int StartLate = Convert.ToInt32(sParam.Split('~')[13]);
            int EndLate = Convert.ToInt32(sParam.Split('~')[14]);
            int StartEarly = Convert.ToInt32(sParam.Split('~')[15]);
            int EndEarly = Convert.ToInt32(sParam.Split('~')[16]);
            int StartDaysCount = Convert.ToInt32(sParam.Split('~')[17]);
            int EndDaysCount = Convert.ToInt32(sParam.Split('~')[18]);

            return tc.ExecuteReader("EXEC [SP_Rpt_Comp_AttendanceWithExtraSearching] %d,%d,%s,%s,%s,%s,%s,%s,%n,%D,%D,%D,%D,%n,%n,%n,%n,%n,%n,%n",
            StartDate,
            EndDate,
            sBusinessUnitIds,
            sLocationID,
            sDepartmentIds,
            sDesignationIds,
            sSalarySchemeIDs,
            sEmployeeIDs,
            CriteriaType,
            StartInTime,
            EndInTime,
            StartOutTime,
            EndOutTime,
            StartLate,
            EndLate,
            StartEarly,
            EndEarly,
            StartDaysCount,
            EndDaysCount,
            nUserID
          );
        }


        #endregion

    }
}
