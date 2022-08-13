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
    #region AttendanceSchemeDayOff

    public class AttendanceSchemeDayOff : BusinessObject
    {
        public AttendanceSchemeDayOff()
        {
            AttendanceSchemeDayOffID = 0;
            AttendanceSchemeID = 0;
            WeekDay = "";
            DayOffType = 0;
            IsAlternateFromFirstWeek = true;
            NoOfRandomDayOff = 0;
            InTime = DateTime.Now;
            OutTime = DateTime.Now;
            ErrorMessage = "";
        }

        #region Properties
        public int AttendanceSchemeDayOffID { get; set; }
        public int AttendanceSchemeID { get; set; }
        public string WeekDay { get; set; }
        //public bool IsAlternate { get; set; }
        public int DayOffType { get; set; }
        public DateTime InTime { get; set; }
        public DateTime OutTime { get; set; }
        public bool IsAlternateFromFirstWeek { get; set; }
        public int NoOfRandomDayOff { get; set; }
        public string ErrorMessage { get; set; }

        #endregion

        #region Derived Property

        public string DayOffInfo
        {
            
            get{
                 //string[] Days = { "Sat", "Sun", "Mon", "Tues", "Wed", "Thu", "Fri" };
                if (this.DayOffType == (int)EnumDayOffType.Continous)
                {
                    return this.WeekDay + " - Continous";
                }
                else if (this.DayOffType == (int)EnumDayOffType.Alternate)
                {
                    return this.WeekDay + " - Alt." + ((this.IsAlternateFromFirstWeek) ? " First Week" : " Second Week");
                }
                else if (this.DayOffType == (int)EnumDayOffType.Random)
                {
                    return "Any " + this.NoOfRandomDayOff + ((this.NoOfRandomDayOff > 1) ? " Days" : " Day") + "/ Month";
                }
                else if (this.DayOffType == (int)EnumDayOffType.HalfDay)
                {
                    //return "Day Off from " + this.InTimeSt + " To " + this.OutTimeSt;
                    return this.WeekDay + " - Half Day";
                }
                else 
                {
                    return "";
                }
                 
            }
            
        }
        public string InTimeSt
        {
            get
            {
                if (this.InTime.ToString("HH:mm") != "00:00")
                    return this.InTime.ToString("HH:mm");
                else
                    return "-";
            }
        }
        public string OutTimeSt
        {
            get
            {
                if (this.OutTime.ToString("HH:mm") != "00:00")
                    return this.OutTime.ToString("HH:mm");
                else
                    return "-";
            }
        }
        #endregion

        #region Functions
        public static List<AttendanceSchemeDayOff> Gets(int nAttendanceSchemeID, long nUserID)
        {
            return AttendanceSchemeDayOff.Service.Gets(nAttendanceSchemeID, nUserID);
        }
        public static List<AttendanceSchemeDayOff> Gets(string sSQL, long nUserID)
        {
            return AttendanceSchemeDayOff.Service.Gets(sSQL, nUserID);
        }
        public AttendanceSchemeDayOff Get(int id, long nUserID)
        {
            return AttendanceSchemeDayOff.Service.Get(id, nUserID);
        }
        public AttendanceSchemeDayOff IUD(int nDBOperation, long nUserID)
        {
            return AttendanceSchemeDayOff.Service.IUD(this, nDBOperation, nUserID);
        }

        #endregion

        #region ServiceFactory

        internal static IAttendanceSchemeDayOffService Service
        {
            get { return (IAttendanceSchemeDayOffService)Services.Factory.CreateService(typeof(IAttendanceSchemeDayOffService)); }
        }
        #endregion
    }
    #endregion

    #region IAttendanceSchemeDayOff interface

    public interface IAttendanceSchemeDayOffService
    {
        AttendanceSchemeDayOff Get(int id, Int64 nUserID);
        List<AttendanceSchemeDayOff> Gets(int attendanceSchemeID, Int64 nUserID);
        List<AttendanceSchemeDayOff> Gets(string sSQL, Int64 nUserID);
        AttendanceSchemeDayOff IUD(AttendanceSchemeDayOff oAttendanceSchemeDayOff, int nDBOperation, Int64 nUserID);
    }
    #endregion


}
