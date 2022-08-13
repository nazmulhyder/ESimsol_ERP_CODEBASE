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
    public class AttendanceDaily_ZNDA
    {
        public AttendanceDaily_ZNDA() { }

        #region Insert Update Delete Function

        public static IDataReader IUD(TransactionContext tc, AttendanceDaily_ZN oAttendanceDaily_ZN, Int64 nUserID, int nDBOperation)
        {
            return tc.ExecuteReader("EXEC [SP_IUD_AttendanceDaily_ZN] %n,%n,%d,%D,%D,%D,%D,%n,%n,%n,%n,%n,%n,%n,%b,%b,%b, %b,%b,%b,%b,%n,%s,%b,%b,%n,%n,%n",
                   oAttendanceDaily_ZN.AttendanceID,
                   oAttendanceDaily_ZN.EmployeeID,
                   oAttendanceDaily_ZN.AttendanceDate,
                   oAttendanceDaily_ZN.InTime,
                   oAttendanceDaily_ZN.OutTime,

                   oAttendanceDaily_ZN.CompInTime,
                   oAttendanceDaily_ZN.CompOutTime,

                   oAttendanceDaily_ZN.LateArrivalMinute,
                   oAttendanceDaily_ZN.EarlyDepartureMinute,
                   oAttendanceDaily_ZN.OverTimeInMinute,

                   oAttendanceDaily_ZN.CompLateArrivalMinute,
                   oAttendanceDaily_ZN.CompEarlyDepartureMinute,
                   oAttendanceDaily_ZN.CompTotalWorkingHourInMinute,
                   oAttendanceDaily_ZN.CompOverTimeInMinute,

                   oAttendanceDaily_ZN.IsDayOff,
                   oAttendanceDaily_ZN.IsLeave,
                   oAttendanceDaily_ZN.IsUnPaid,

                   oAttendanceDaily_ZN.IsHoliday,
                   oAttendanceDaily_ZN.IsCompDayOff,
                   oAttendanceDaily_ZN.IsCompLeave,
                   oAttendanceDaily_ZN.IsCompHoliday,

                   oAttendanceDaily_ZN.WorkingStatus,
                   oAttendanceDaily_ZN.Note,
                   oAttendanceDaily_ZN.IsLock,

                   oAttendanceDaily_ZN.IsManual,

                   nUserID, nDBOperation,
                   oAttendanceDaily_ZN.LeaveHeadID
                   );
        }

        public static IDataReader ManualAttendance_Update(TransactionContext tc, AttendanceDaily_ZN oAttendanceDaily_ZN, Int64 nUserID, int nDBOperation)
        {
            return tc.ExecuteReader("EXEC [SP_IUD_AttendanceDaily_ZN_Manual] %n,%n,%d,%d,%D,%D,%D,%D,%n,%n,%n,%n,%n,%n,%b,%b,%b,%b,%n,%s,%b,%b,%n,%n,%n",
                   oAttendanceDaily_ZN.AttendanceID,
                   oAttendanceDaily_ZN.EmployeeID,
                   oAttendanceDaily_ZN.AttendanceDateFrom,
                   oAttendanceDaily_ZN.AttendanceDateTo,
                   oAttendanceDaily_ZN.InTime,
                   oAttendanceDaily_ZN.OutTime,

                   oAttendanceDaily_ZN.CompInTime,
                   oAttendanceDaily_ZN.CompOutTime,

                   oAttendanceDaily_ZN.LateArrivalMinute,
                   oAttendanceDaily_ZN.EarlyDepartureMinute,
                   oAttendanceDaily_ZN.OverTimeInMinute,

                   oAttendanceDaily_ZN.CompLateArrivalMinute,
                   oAttendanceDaily_ZN.CompEarlyDepartureMinute,
                //oAttendanceDaily_ZN.CompTotalWorkingHourInMinute,
                   oAttendanceDaily_ZN.CompOverTimeInMinute,

                   oAttendanceDaily_ZN.IsDayOff,
                   oAttendanceDaily_ZN.IsHoliday,
                   oAttendanceDaily_ZN.IsCompDayOff,
                   oAttendanceDaily_ZN.IsCompHoliday,

                   oAttendanceDaily_ZN.WorkingStatus,
                   oAttendanceDaily_ZN.Note,
                   oAttendanceDaily_ZN.IsLock,

                   oAttendanceDaily_ZN.IsManual,

                   nUserID, nDBOperation,

                   oAttendanceDaily_ZN.LeaveHeadID
                   );
        }

        public static IDataReader UploadAttendanceXL(TransactionContext tc, AttendanceDaily_ZN oAttendanceDaily_ZN, Int64 nUserID)
        {
            return tc.ExecuteReader("EXEC [SP_IUD_AttendanceDaily_ZN_UploadXL]%s,%s,%d,%n",
                   oAttendanceDaily_ZN.EmployeeCode,
                   oAttendanceDaily_ZN.ErrorMessage,//att. type
                   oAttendanceDaily_ZN.AttendanceDate,// att start date
                   nUserID
                   );
        }

        #endregion

        #region Get & Exist Function
        public static IDataReader Get(TransactionContext tc, int nEmployeeID, DateTime dAttendanceDate)
        {
            return tc.ExecuteReader("SELECT * FROM View_AttendanceDaily_ZN WHERE EmployeeID=%n AND AttendanceDate=%d", nEmployeeID, dAttendanceDate);
        }
        public static IDataReader Get(TransactionContext tc, string sSQL)
        {
            return tc.ExecuteReader(sSQL);
        }
        public static IDataReader Gets(string sSQL, TransactionContext tc)
        {
            return tc.ExecuteReader(sSQL);
        }

        public static IDataReader GetsDayWiseAbsent(int nDays, DateTime dDate, TransactionContext tc)
        {
            return tc.ExecuteReader("EXEC [SP_Rpt_DayWiseAbsent] %n,%d",
                   nDays,
                   dDate);
        }
        public static IDataReader GetsTimeCard(string sEmployeeIDs, DateTime Startdate, DateTime EndDate, string sLocationID, string sDepartmentIds, string sType, string sBUnitIDs, double nStartSalaryRange, double nEndSalaryRange, string sBlockIDs, string sGroupIDs, Int64 nUserID, TransactionContext tc)
        {
            return tc.ExecuteReader("EXEC [SP_rpt_TimeCard_ZN] %s,%d,%d,%s,%s,%s,%s,%n,%n,%s, %s,%n",
                   sEmployeeIDs,
                   Startdate, EndDate, sLocationID, sDepartmentIds, sType, sBUnitIDs,
                   nStartSalaryRange, nEndSalaryRange, sBlockIDs, sGroupIDs, nUserID);
        }
        public static IDataReader GetsTimeCardComp(string sEmployeeIDs, DateTime Startdate, DateTime EndDate, string sLocationID, string sDepartmentIds, string sType, string sBUnitIDs, double nStartSalaryRange, double nEndSalaryRange, string sBlockIDs, string sGroupIDs, Int64 nUserID, TransactionContext tc)
        {
            return tc.ExecuteReader("EXEC [SP_rpt_TimeCard_ZN_Comp] %s,%d,%d,%s,%s,%s,%s,%n,%n,%s, %s,%n",
                   sEmployeeIDs,
                   Startdate, EndDate, sLocationID, sDepartmentIds, sType, sBUnitIDs,
                   nStartSalaryRange, nEndSalaryRange, sBlockIDs, sGroupIDs, nUserID);
        }

        public static IDataReader GetsTimeCardMaxOTConf(string sEmployeeIDs, DateTime Startdate, DateTime EndDate, string sLocationID, string sDepartmentIds, string sBUnitIDs, double nStartSalaryRange, double nEndSalaryRange, string sBlockIDs, string sGroupIDs, int nMOCID, Int64 nUserID, TransactionContext tc)
        {
            return tc.ExecuteReader("EXEC [SP_RPT_TimeCard_MaxOTCon] %s,%d,%d,%s,%s,%s,%n,%n,%s,%s, %n,%n",
                   sEmployeeIDs,
                   Startdate, EndDate, sLocationID, sDepartmentIds, sBUnitIDs,
                   nStartSalaryRange, nEndSalaryRange, sBlockIDs, sGroupIDs, nMOCID, nUserID);//SP_RPT_TimeCardAsPerConf
        }
        public static IDataReader GetsTimeCardMaxOTConfSearch(string sEmployeeIDs, DateTime Startdate, DateTime EndDate, string sLocationID, string sDepartmentIds, string sBUnitIDs, double nStartSalaryRange, double nEndSalaryRange, string sBlockIDs, string sGroupIDs, int nMOCID, Int64 nUserID, TransactionContext tc)
        {
            return tc.ExecuteReader("EXEC [SP_RPT_TimeCard_MaxOTCon] %s,%d,%d,%s,%s,%s,%n,%n,%s,%s, %n,%n",
                   sEmployeeIDs,
                   Startdate, EndDate, sLocationID, sDepartmentIds, sBUnitIDs,
                   nStartSalaryRange, nEndSalaryRange, sBlockIDs, sGroupIDs, nMOCID, nUserID);
        }
        #endregion
    }
}
