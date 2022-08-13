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
    public class AttendanceDailyDA
    {
        public AttendanceDailyDA() { }

        #region Insert Update Delete Function

        public static IDataReader IUD(TransactionContext tc, AttendanceDaily oAttendanceDaily, Int64 nUserID, int nDBOperation)
        {
            return tc.ExecuteReader("EXEC [SP_IUD_AttendanceDaily] %n,%n,%d,%D,%D,%D,%D,%n,%n,%n,%n,%n,%n,%n,%b,%b,%b, %b,%b,%b,%b,%n,%s,%b,%b,%n,%n,%n",
                   oAttendanceDaily.AttendanceID, 
                   oAttendanceDaily.EmployeeID, 
                   oAttendanceDaily.AttendanceDate,
                   oAttendanceDaily.InTime, 
                   oAttendanceDaily.OutTime,

                   oAttendanceDaily.CompInTime,
                   oAttendanceDaily.CompOutTime,

                   oAttendanceDaily.LateArrivalMinute,
                   oAttendanceDaily.EarlyDepartureMinute,
                   oAttendanceDaily.OverTimeInMinute,

                   oAttendanceDaily.CompLateArrivalMinute,
                   oAttendanceDaily.CompEarlyDepartureMinute,
                   oAttendanceDaily.CompTotalWorkingHourInMinute,
                   oAttendanceDaily.CompOverTimeInMinute,

                   oAttendanceDaily.IsDayOff,
                   oAttendanceDaily.IsLeave,
                   oAttendanceDaily.IsUnPaid,

                   oAttendanceDaily.IsHoliday,
                   oAttendanceDaily.IsCompDayOff,
                   oAttendanceDaily.IsCompLeave,
                   oAttendanceDaily.IsCompHoliday,

                   oAttendanceDaily.WorkingStatus,
                   oAttendanceDaily.Note,
                   oAttendanceDaily.IsLock,

                   oAttendanceDaily.IsManual,
                 
                   nUserID, nDBOperation,
                   oAttendanceDaily.LeaveHeadID
                   );
        }

        public static IDataReader Update_AttendanceDaily_Manual_Single(TransactionContext tc, DateTime dtStartDate, DateTime dtEndDate, int nEmployeeID, int nBufferTime, bool bIsOverTime, Int64 nUserID)
        {
            return tc.ExecuteReader("EXEC [SP_Process_UpdateComplianceAttendanceDaily] %d,%d,%n,%n,%b,%n",
                   dtStartDate,
                   dtEndDate,
                   nEmployeeID,
                   nBufferTime,
                   bIsOverTime,
                   nUserID
                   );
        }
        public static IDataReader Update_AttendanceDaily_Manual_All(TransactionContext tc, DateTime dtStartDate, DateTime dtEndDate, int nEmployeeID, int nBufferTime, bool bIsOverTime, Int64 nUserID)
        {
            return tc.ExecuteReader("EXEC [SP_Process_UpdateComplianceAttendanceDaily] %d,%d,%n,%n,%b,%n",
                   dtStartDate,
                   dtEndDate,
                   nEmployeeID,
                   nBufferTime,
                   bIsOverTime,
                   nUserID
                   );
        }
        public static IDataReader ManualAttendance_Update(TransactionContext tc, AttendanceDaily oAttendanceDaily, Int64 nUserID, int nDBOperation)
        {
            return tc.ExecuteReader("EXEC [SP_IUD_AttendanceDaily_Manual] %n,%n,%d,%d,%D,%D,%D,%D,%n,%n,%n,%n,%n,%n,%b,%b,%b,%b,%n,%s,%b,%b,%n,%n,%n",
                   oAttendanceDaily.AttendanceID,
                   oAttendanceDaily.EmployeeID,
                   oAttendanceDaily.AttendanceDateFrom,
                   oAttendanceDaily.AttendanceDateTo,
                   oAttendanceDaily.InTime,
                   oAttendanceDaily.OutTime,

                   oAttendanceDaily.CompInTime,
                   oAttendanceDaily.CompOutTime,

                   oAttendanceDaily.LateArrivalMinute,
                   oAttendanceDaily.EarlyDepartureMinute,
                   oAttendanceDaily.OverTimeInMinute,

                   oAttendanceDaily.CompLateArrivalMinute,
                   oAttendanceDaily.CompEarlyDepartureMinute,
                   //oAttendanceDaily.CompTotalWorkingHourInMinute,
                   oAttendanceDaily.CompOverTimeInMinute,

                   oAttendanceDaily.IsDayOff,
                   oAttendanceDaily.IsHoliday,
                   oAttendanceDaily.IsCompDayOff,
                   oAttendanceDaily.IsCompHoliday,

                   oAttendanceDaily.WorkingStatus,
                   oAttendanceDaily.Note,
                   oAttendanceDaily.IsLock,

                   oAttendanceDaily.IsManual,

                   nUserID, nDBOperation,

                   oAttendanceDaily.LeaveHeadID
                   );
        }

        public static IDataReader UploadAttendanceXL(TransactionContext tc, AttendanceDaily oAttendanceDaily, Int64 nUserID)
        {
            return tc.ExecuteReader("EXEC [SP_IUD_AttendanceDaily_UploadXL]%s,%s,%d,%n",
                   oAttendanceDaily.EmployeeCode,
                   oAttendanceDaily.ErrorMessage,//att. type
                   oAttendanceDaily.AttendanceDate,// att start date
                   nUserID
                   );
        }

        public static IDataReader AttendanceDaily_Manual_Single(TransactionContext tc, AttendanceDaily oAttendanceDaily, Int64 nUserID)
        {
            return tc.ExecuteReader("EXEC [SP_IUD_AttendanceDaily_Manual_Single] %n,%D,%D,%n,%b,%b,%b,%n,%n,%s,%b,%n,%n",
                   oAttendanceDaily.AttendanceID,
                   oAttendanceDaily.InTime,
                   oAttendanceDaily.OutTime,
                   oAttendanceDaily.ShiftID,
                   oAttendanceDaily.IsOSD,
                   oAttendanceDaily.IsDayOff,
                   oAttendanceDaily.IsAbsent,
                   oAttendanceDaily.LateArrivalMinute,
                   oAttendanceDaily.EarlyDepartureMinute,
                   oAttendanceDaily.Remark,
                   oAttendanceDaily.IsManualOT,
                   oAttendanceDaily.OverTimeInMinute,
                   nUserID
                   );
        }
        public static IDataReader AttendanceDaily_Manual_Single_Comp(TransactionContext tc, AttendanceDaily oAttendanceDaily, Int64 nUserID)
        {
            return tc.ExecuteReader("EXEC [SP_IUD_AttendanceDaily_Manual_Single_Comp] %n,%D,%D,%n,%n,%b,%b,%n,%n,%b,%n",
                   oAttendanceDaily.AttendanceID,
                   oAttendanceDaily.InTime,
                   oAttendanceDaily.OutTime,
                   oAttendanceDaily.LateArrivalMinute,
                   oAttendanceDaily.EarlyDepartureMinute,
                   oAttendanceDaily.IsAbsent,
                   oAttendanceDaily.IsManualOT,
                   oAttendanceDaily.OverTimeInMinute,
                   oAttendanceDaily.CompLeaveHeadID,
                   oAttendanceDaily.IsCompDayOff,
                   nUserID
                   );
        }
        public static IDataReader AttendanceDaily_Manual_Single_Comp_Conf(TransactionContext tc, AttendanceDaily oAttendanceDaily, Int64 nUserID)
        {
            return tc.ExecuteReader("EXEC [SP_IUD_TimeCardMaxOTCon ] %n,%d,%n,%D,%D,%b,%b,%n,%b,%n,%n",
                   oAttendanceDaily.EmployeeID,
                   oAttendanceDaily.AttendanceDateInString,
                   oAttendanceDaily.MOCID,
                   oAttendanceDaily.InTime,
                   oAttendanceDaily.OutTime,
                   oAttendanceDaily.IsAbsent,
                   oAttendanceDaily.IsCompDayOff,
                   oAttendanceDaily.CompLeaveHeadID,
                   oAttendanceDaily.IsManualOT,
                   oAttendanceDaily.OverTimeInMinute,
                   nUserID
                   );
        }
        public static IDataReader AttendanceDaily_Manual_Single_Comp_ForDailyAttendance(TransactionContext tc, AttendanceDaily oAttendanceDaily, Int64 nUserID)
        {
            return tc.ExecuteReader("EXEC [SP_IUD_AttendanceDaily_Manual_Single_Comp] %n,%D,%D,%n,%n,%b,%b,%n,%n,%b,%n",
                   oAttendanceDaily.AttendanceID,
                   oAttendanceDaily.InTime,
                   oAttendanceDaily.OutTime,
                   oAttendanceDaily.LateArrivalMinute,
                   oAttendanceDaily.EarlyDepartureMinute,
                   oAttendanceDaily.IsAbsent,
                   oAttendanceDaily.IsManualOT,
                   oAttendanceDaily.OverTimeInMinute,
                   oAttendanceDaily.CompLeaveHeadID,
                   oAttendanceDaily.IsCompDayOff,
                   nUserID
                   );
        }
        #endregion

        #region Get & Exist Function
        public static IDataReader Get(TransactionContext tc, int nEmployeeID, DateTime dAttendanceDate)
        {
            return tc.ExecuteReader("SELECT * FROM View_AttendanceDaily WHERE EmployeeID=%n AND AttendanceDate=%d", nEmployeeID, dAttendanceDate);
        }
        public static IDataReader Get(TransactionContext tc, string sSQL)
        {
            return tc.ExecuteReader(sSQL);
        }
        public static IDataReader Gets(string sSQL, TransactionContext tc)
        {
            return tc.ExecuteReader(sSQL);
        }
        public static IDataReader Gets(string sEmployeeIDs, string sBusinessUnitIds, string sLocationID, string sDepartmentIds, string sDesignationIds, string sSalarySchemeIDs, DateTime DateFrom, DateTime DateTo, string sWorkingStatus, string sGroupIDs, string sBlockIDs, double nStartSalaryRange, double nEndSalaryRange, string ShiftIds, string sRemarks, Int64 nUserID, TransactionContext tc)
        {
            return tc.ExecuteReader("EXEC [SP_Rpt_MaternityFollowUp] %s,%s,%s,%s,%s,%s,%d,%d,%s,%n, %s, %s,%n,%n,%s, %s",
                  sEmployeeIDs, sBusinessUnitIds, sLocationID, sDepartmentIds, sDesignationIds, sSalarySchemeIDs, DateFrom, DateTo, sWorkingStatus, nUserID, sGroupIDs, sBlockIDs, nStartSalaryRange, nEndSalaryRange, ShiftIds, sRemarks);
        }

        public static IDataReader GetsContinuousAbsent(DateTime DateFrom, DateTime DateTo, string BUIDs, string LocIDs, string DepartmentIDs, string DesignationIDs, int DayCount, Int64 nUserID, TransactionContext tc)
        {
            return tc.ExecuteReader("EXEC [SP_Rpt_ContinuousAbsent] %d,%d,%s,%s,%s,%s,%n,%n",
                  DateFrom, DateTo, BUIDs, LocIDs, DepartmentIDs, DesignationIDs, DayCount,nUserID);
        }
        public static IDataReader GetsRecord(string sEmployeeIDs, string sBusinessUnitIds, string sLocationID, string sDepartmentIds, string sDesignationIds, string sSalarySchemeIDs, DateTime DateFrom, DateTime DateTo, string sWorkingStatus, string sGroupIDs, string sBlockIDs, double nStartSalaryRange, double nEndSalaryRange, string ShiftIds, Int64 nUserID, TransactionContext tc)
        {
            return tc.ExecuteReader("EXEC [SP_Rpt_AttSummary] %s,%s,%s,%s,%s,%s,%d,%d,%s,%n, %s, %s,%n,%n,%s",
                  sEmployeeIDs, sBusinessUnitIds, sLocationID, sDepartmentIds, sDesignationIds, sSalarySchemeIDs, DateFrom, DateTo, sWorkingStatus, nUserID, sGroupIDs, sBlockIDs, nStartSalaryRange, nEndSalaryRange, ShiftIds);
        }

        public static IDataReader GetsDayWiseAbsent(int nDays, DateTime dDate, TransactionContext tc)
        {
            return tc.ExecuteReader("EXEC [SP_Rpt_DayWiseAbsent] %n,%d",
                   nDays,
                   dDate);
        }
        #endregion

        public static IDataReader UploadAttXL(TransactionContext tc, AttendanceDaily oAttendanceDaily, bool IsNUInTime, bool IsNUOutTime, bool IsNULate, bool IsNUEarly, bool IsNUInDate, bool IsNUOutDate, bool IsNUOT, bool IsNURemark, bool IsComp, Int64 nUserID)
        {
            return tc.ExecuteReader("EXEC [SP_Upload_ManualDailyAtt_UploadXL]%D, %s, %s, %D, %D, %n, %n, %n, %s, %n, %b, %b, %b, %b, %b, %b, %b, %b, %b",
                   oAttendanceDaily.AttendanceDate,
                   oAttendanceDaily.EmployeeCode,
                   oAttendanceDaily.ErrorMessage,
                   (IsComp == true) ? oAttendanceDaily.CompInTime : oAttendanceDaily.InTime,
                   (IsComp == true) ? oAttendanceDaily.CompOutTime : oAttendanceDaily.OutTime, 
                   (IsComp == true) ? oAttendanceDaily.CompLateArrivalMinute:oAttendanceDaily.LateArrivalMinute,
                   (IsComp == true) ? oAttendanceDaily.CompEarlyDepartureMinute:oAttendanceDaily.EarlyDepartureMinute,
                   (IsComp == true) ? oAttendanceDaily.CompOverTimeInMinute : oAttendanceDaily.OverTimeInMinute,
                   oAttendanceDaily.Remark,
                   nUserID,
                   IsNUInTime,
                   IsNUOutTime,
                   IsNULate,
                   IsNUEarly,
                   IsNUInDate,
                   IsNUOutDate,
                   IsNUOT,
                   IsNURemark,
                   IsComp
                   );
        }
        public static IDataReader MakeAbsent(TransactionContext tc, string sAttendanceDate, bool bOperation, bool bIsLeaveBefore, bool bIsLeaveAfter, bool bIsAbsentBefore, bool bIsAbsentAfter, bool bIsHolidayBefore, bool bIsHolidayAfter, bool bIsDayOffBefore, bool bIsDayOffAfter, string BUIDs, string LocIDs, string DepartmentIDs, string DesignationIDs, bool IsComp, Int64 nUserID)
        {
            return tc.ExecuteReader("EXEC [SP_Rpt_BridgeAbsent]%d, %b, %b, %b, %b, %b, %b, %b, %b, %b, %s, %s, %s, %s, %b, %n",
                   sAttendanceDate
                   ,bOperation
                   , bIsLeaveBefore
                   , bIsLeaveAfter
                   , bIsAbsentBefore
                   , bIsAbsentAfter
                   , bIsHolidayBefore
                   , bIsHolidayAfter
                   , bIsDayOffBefore
                   , bIsDayOffAfter
                   , BUIDs
                   , LocIDs
                   , DepartmentIDs
                   , DesignationIDs
                   , IsComp
                   ,nUserID
                   );
        }

        public static IDataReader DayoffListExcel(TransactionContext tc, string sAttendanceDate, bool bIsDayoffThisDay, string BUIDs, string LocIDs, string DepartmentIDs, string DesignationIDs, bool IsComp, int nType, Int64 nUserID)
        {
            return tc.ExecuteReader("EXEC [SP_Rpt_DayoffThisDay]%d, %b, %s, %s, %s, %s, %b, %n, %n",
                   sAttendanceDate
                   , bIsDayoffThisDay
                   , BUIDs
                   , LocIDs
                   , DepartmentIDs
                   , DesignationIDs
                   , IsComp
                   , nUserID
                   , nType
                   );
        }
        public static IDataReader MakeLeave(TransactionContext tc, string sAttendanceDate, bool bIsDayoffThisDay, string BUIDs, string LocIDs, string DepartmentIDs, string DesignationIDs, bool IsComp, int nType, int nLeaveHeadID, DateTime sStartDate, DateTime sEndDate, Int64 nUserID)
        {
            return tc.ExecuteReader("EXEC [SP_ProcessLeaveOnDayoff]%d, %b, %s, %s, %s, %s, %b, %n, %n, %n, %d, %d",
                   sAttendanceDate
                   , bIsDayoffThisDay
                   , BUIDs
                   , LocIDs
                   , DepartmentIDs
                   , DesignationIDs
                   , IsComp
                   , nUserID
                   , nType
                   , nLeaveHeadID
                   , sStartDate
                   , sEndDate
                   );
        }
        public static int GetTotalCount(TransactionContext tc, string sSql, Int64 nUserID)
        {
            int nTotalCount = 0;
            object oIndex = tc.ExecuteScalar(sSql);
            if (oIndex != null)
            {
                nTotalCount = Convert.ToInt32(oIndex);
            }
            return nTotalCount;
        }

    }
}
