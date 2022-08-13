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
    #region AttendanceSummery
    [DataContract]
    public class AttendanceSummery_Chart : BusinessObject
    {
        public AttendanceSummery_Chart()
        {
            TotalPresent = 0;
            PresentPercent = 0;
            TotalAbsent = 0;
            AbsentPercent = 0;
            TotalLeave = 0;
            LeavePercent = 0;
            TotalEarlyLeave = 0;
            EarlyLeavePercent = 0;
            TotalLate = 0;
            LatePercent = 0;
            ErrorMessage = "";
        }


        #region Properties
        public int TotalPresent { get; set; }
        public double PresentPercent { get; set; }
        public int TotalAbsent { get; set; }
        public double AbsentPercent { get; set; }
        public int TotalLeave { get; set; }
        public double LeavePercent { get; set; }
        public double TotalEarlyLeave { get; set; }
        public double EarlyLeavePercent { get; set; }
        public double TotalLate { get; set; }
        public double LatePercent { get; set; }
        public string ErrorMessage { get; set; }

        #endregion

        #region Derived Property

        #endregion

        #region Functions

        public static AttendanceSummery_Chart Get(DateTime DateFrom, DateTime DateTo, long nUserID)
        {
            return AttendanceSummery_Chart.Service.Get(DateFrom, DateTo, nUserID);
        }

        #endregion

        #region ServiceFactory
        internal static IAttendanceSummery_ChartService Service
        {
            get { return (IAttendanceSummery_ChartService)Services.Factory.CreateService(typeof(IAttendanceSummery_ChartService)); }
        }
        #endregion
    }
    #endregion

    #region IAttendanceSummery interface
    public interface IAttendanceSummery_ChartService
    {
        AttendanceSummery_Chart Get(DateTime DateFrom, DateTime DateTo, Int64 nUserID);

    }
    #endregion
}
