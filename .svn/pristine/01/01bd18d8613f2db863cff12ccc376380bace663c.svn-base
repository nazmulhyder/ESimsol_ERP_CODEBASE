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
    public class BenefitOnAttendanceDA
    {
        public BenefitOnAttendanceDA() { }

        #region Insert Update Delete Function

        public static IDataReader IUD(TransactionContext tc, BenefitOnAttendance oBenefitOnAttendance, Int64 nUserID, int nDBOperation)
        {
            return tc.ExecuteReader("EXEC [SP_IUD_BenefitOnAttendance] %n,%s,%n,%D,%D,%n,%n,%n,%b,%n,%n,%b,%n,%n,%n,%n,%b,%d,%d,%n,%d,%d,%n,%n,%n",
                   oBenefitOnAttendance.BOAID, oBenefitOnAttendance.Name, oBenefitOnAttendance.BenefitOn,
                   oBenefitOnAttendance.StartTime, oBenefitOnAttendance.EndTime, oBenefitOnAttendance.TolarenceInMinute,
                   oBenefitOnAttendance.OTInMinute, oBenefitOnAttendance.OTDistributePerPresence, oBenefitOnAttendance.IsFullWorkingHourOT,oBenefitOnAttendance.SalaryHeadID,
                   oBenefitOnAttendance.AllowanceOn, oBenefitOnAttendance.IsPercent, oBenefitOnAttendance.Value,
                   oBenefitOnAttendance.LeaveHeadID, oBenefitOnAttendance.LeaveAmount, oBenefitOnAttendance.HolidayID, oBenefitOnAttendance.IsContinous,
                   oBenefitOnAttendance.BenefitStartDate, oBenefitOnAttendance.BenefitEndDate, oBenefitOnAttendance.ApproveBy,
                   oBenefitOnAttendance.ApproveDate, oBenefitOnAttendance.InactiveDate, oBenefitOnAttendance.InactiveBy,
                   nUserID, nDBOperation);
        }

        #endregion

        #region Get & Exist Function

        public static IDataReader Get(int nBOAID, TransactionContext tc)
        {
            return tc.ExecuteReader("SELECT * FROM VIEW_BenefitOnAttendance WHERE BOAID=%n", nBOAID);
        }
        public static IDataReader Get(string sSQL, TransactionContext tc)
        {
            return tc.ExecuteReader(sSQL);
        }
        public static IDataReader Gets(TransactionContext tc)
        {
            return tc.ExecuteReader("SELECT * FROM VIEW_BenefitOnAttendance");
        }
        public static IDataReader Gets(string sSQL, TransactionContext tc)
        {
            return tc.ExecuteReader(sSQL);
        }


        #endregion
    }
}
