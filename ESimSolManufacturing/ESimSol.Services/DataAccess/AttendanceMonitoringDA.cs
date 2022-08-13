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
    public class AttendanceMonitoringDA
    {
        public AttendanceMonitoringDA() { }

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

        public static IDataReader Gets(string sBUnit, string sLocationID, string sDepartmentIDs, string sDesignationIDs, string sShiftIds, string sBlockIDs, DateTime dDate, string sGroupIDs, Int64 nUserID, TransactionContext tc)
        {
            return tc.ExecuteReader("EXEC [SP_Rpt_AttendanceMonitoring] %s,%s,%s,%s,%s,%s,%d,%n,%s",
                   sBUnit, sLocationID, sDepartmentIDs, sDesignationIDs,
                    sShiftIds, sBlockIDs, dDate, nUserID,sGroupIDs);
        }
        public static IDataReader GetsComp(string sBUnit, string sLocationID, string sDepartmentIDs, string sDesignationIDs, string sShiftIds, string sBMMIDs, DateTime dDate, string sGroupIDs, string sBlockIDs, Int64 nUserID, TransactionContext tc)
        {
            return tc.ExecuteReader("EXEC [SP_Rpt_Comp_AttendanceMonitoring] %s,%s,%s,%s,%s,%s,%d,%n,%s,%s",
                   sBUnit, sLocationID, sDepartmentIDs, sDesignationIDs,
                    sShiftIds, sBMMIDs, dDate, nUserID, sGroupIDs, sBlockIDs);
        }
        public static IDataReader Gets_LINE(string sBUnit, string sLocationID, string sDepartmentIDs, string sDesignationIDs, string sShiftIds, string sBMMIDs, DateTime dDate, Int64 nUserID, TransactionContext tc)
        {
            return tc.ExecuteReader("EXEC [SP_Rpt_AttendanceMonitoring_Block] %s,%s,%s,%s,%s,%s,%d,%n",
                   sBUnit, sLocationID,sDepartmentIDs, sDesignationIDs,
                   sShiftIds, sBMMIDs, dDate, nUserID);
        }

        public static IDataReader Gets_DeptSecWise(string sBUnit, string sLocationID, string sDepartmentIDs, string sDesignationIDs, string sShiftIds, string sBMMIDs, DateTime dDate, Int64 nUserID, TransactionContext tc)
        {
            return tc.ExecuteReader("EXEC [SP_Rpt_AttendanceMonitoring_DeptSec] %s,%s,%s,%s,%s,%s,%d,%n ",
                   sBUnit, sLocationID,sDepartmentIDs, sDesignationIDs,
                   sShiftIds, sBMMIDs, dDate, nUserID);
        }

         public static IDataReader GetsManPower(TransactionContext tc, string SqlQuery, DateTime AttendanceDate, int ReportLayout)
        {


            return tc.ExecuteReader("EXEC [SP_Rpt_ManPower]" + "%s, %d, %n", SqlQuery, AttendanceDate, ReportLayout);
            
        }
       
        #endregion
    }
}
