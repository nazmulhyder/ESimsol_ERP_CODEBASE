using System;
using System.IO;
using ICS.Core;
using ICS.Core.Framework;
using ICS.Core.Utility;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Collections.Generic;

namespace ESimSol.BusinessObjects
{
    #region AttendanceDailyManualHistory

    public class AttendanceDailyManualHistory : BusinessObject
    {
        public AttendanceDailyManualHistory()
        {

            ADMHID = 0;
            MOMUHID = 0;
            MOCAID = 0;
            AttendanceID = 0;
            Description = "";
            DBServerDateTime = DateTime.Now;
            ErrorMessage = "";
            EmployeeName = "";

        }

        #region Properties
        public int ADMHID { get; set; }
        public int MOMUHID { get; set; }
        public int MOCAID { get; set; }
        public int AttendanceID { get; set; }
        public string Description { get; set; }
        public DateTime DBServerDateTime { get; set; }
        public string ErrorMessage { get; set; }
        #endregion

        #region Derived Property
        public string EmployeeName { get; set; }

        public string DBServerDateTimeInString
        {
            get
            {
               return DBServerDateTime.ToString("dd MMM yyyy HH:mm");
            }
        }

        #endregion

        #region Functions

        public static List<AttendanceDailyManualHistory> Gets(string sSQL, long nUserID)
        {
            return AttendanceDailyManualHistory.Service.Gets(sSQL, nUserID);
        }

        #endregion

        #region ServiceFactory
        internal static IAttendanceDailyManualHistoryService Service
        {
            get { return (IAttendanceDailyManualHistoryService)Services.Factory.CreateService(typeof(IAttendanceDailyManualHistoryService)); }
        }

        #endregion
    }
    #endregion

    #region IAttendanceDailyManualHistory interface

    public interface IAttendanceDailyManualHistoryService
    {
        List<AttendanceDailyManualHistory> Gets(string sSQL, Int64 nUserID);
    }
    #endregion
}
