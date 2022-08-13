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
    public class AttendancePerformanceChart : BusinessObject
    {
        public AttendancePerformanceChart()
        {
            Month = DateTime.Now;
            PresentPercent = 0;
            ErrorMessage = "";
            
        }

        #region Properties
        public DateTime Month { get; set; }
        public double PresentPercent { get; set; }
        public string ErrorMessage { get; set; }

        #endregion

        #region Derived Property

        public List<AttendancePerformanceChart> List1 { get; set; } 
        public List<AttendancePerformanceChart> List2 { get; set; } 
        public List<AttendancePerformanceChart> List3 { get; set; } 

        public string MonthInString
        {
            get { return this.Month.ToString("MMM"); }
        }
        public string YearInString
        {
            get { return this.Month.ToString("yyyy"); }
        }
        #endregion

        #region Functions

        public static AttendancePerformanceChart Get(long nUserID)
        {
            return AttendancePerformanceChart.Service.Get(nUserID);
        }

        #endregion

        #region ServiceFactory
        internal static IAttendancePerformanceChartService Service
        {
            get { return (IAttendancePerformanceChartService)Services.Factory.CreateService(typeof(IAttendancePerformanceChartService)); }
        }
        #endregion
    }
    #endregion

    #region IAttendancePerformanceChart interface
    public interface IAttendancePerformanceChartService
    {
        AttendancePerformanceChart Get(Int64 nUserID);

    }
    #endregion
}
