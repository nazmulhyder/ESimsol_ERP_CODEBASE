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

    #region HRMShift

    public class HRMShift : BusinessObject
    {
        public HRMShift()
        {
            ShiftID = 0;
            Code = "";
            Name = "";
            ReportTime = new DateTime(1950, 01, 01);
            StartTime = new DateTime(1950, 01, 01);
            EndTime = new DateTime(1950, 01, 01);
            ToleranceTime = new DateTime(1950, 01, 01);
            TotalWorkingTime = 0;
            DayStartTime = new DateTime(1950, 01, 01);
            DayEndTime = new DateTime(1950, 01, 01);
            ToleranceForEarlyInMin = 0;
            IsActive = true;
            Sequence = 0;
            ErrorMessage = "";
            IsOT = false;
            OTStartTime = new DateTime(1950, 01, 01);
            OTEndTime = new DateTime(1950, 01, 01);
            IsOTOnActual = false;
            OTCalculateAfterInMinute = 0;
            NameBangla = "";
            MaxOTComplianceInMin=0;
            IsLeaveOnOFFHoliday = true;
            CompMaxEndTime = DateTime.Now;
            ShiftOTSlabs = new List<ShiftOTSlab>();
            ShiftBreakSchedules = new List<ShiftBreakSchedule>();
            IsOutOrOT = true;
            IsWithComp = false;
            IsHalfDayOff = false;
            PStart = new DateTime(1950, 01, 01);
            PEnd = new DateTime(1950, 01, 01);
        }

        #region Properties
        public bool IsOutOrOT { get; set; }
        public bool IsWithComp { get; set; }
        public bool IsHalfDayOff { get; set; }
        public int ToleranceForEarlyInMin { get; set; }
        public int ShiftID { get; set; }
        public int MaxOTComplianceInMin { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public string NameBangla { get; set; }
        public DateTime PStart { get; set; }
        public DateTime PEnd { get; set; }
        public DateTime ReportTime { get; set; }
        public DateTime CompMaxEndTime { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public int TotalWorkingTime { get; set; }
        public DateTime ToleranceTime { get; set; }
        public DateTime DayStartTime { get; set; }
        public DateTime DayEndTime { get; set; }
        public bool IsActive { get; set; }
        public int Sequence { get; set; }
        public string ErrorMessage { get; set; }

        public bool IsOT { get; set; }
        public DateTime OTStartTime { get; set; }
        public DateTime OTEndTime { get; set; }

        public string ReportTimeInString
        {
            get
            {
                return ReportTime.ToString("H:mm");
            }
        }
        public string CompMaxEndTimeInString
        {
            get
            {
                return CompMaxEndTime.ToString("H:mm");
            }
        }

        public string MaxOTComplianceInMinInString
        {
            get
            {
                return (MaxOTComplianceInMin / 60) + ":" + (MaxOTComplianceInMin % 60);
            }
        }
        public string StartTimeInString
        {
            get
            {
                return StartTime.ToString("H:mm");
            }
        }

        public string EndTimeInString
        {
            get
            {
                return EndTime.ToString("H:mm");
            }
        }
        public string DayStartTimeInString
        {
            get
            {
                return DayStartTime.ToString("H:mm");
            }
        }
        public string PStartInString
        {
            get
            {
                return PStart.ToString("H:mm");
            }
        }
        public string PEndInString
        {
            get
            {
                return PEnd.ToString("H:mm");
            }
        }
        public string DayEndTimeInString
        {
            get
            {
                return DayEndTime.ToString("H:mm");
            }
        }

        public string OTStartTimeInString
        {
            get
            {
                return OTStartTime.ToString("H:mm");
            }
        }
        public string OTEndTimeInString
        {
            get
            {
                return OTEndTime.ToString("H:mm");
            }
        }
        public string ToleranceTimeInString
        {
            get
            {
                return ToleranceTime.ToString("H:mm");
            }
        }

        public string ConvertTotalWorkingTime(int totalWorkingTime)
        {
            //int h = 0, m = 0;
            //while (true)
            //{
            //    if (totalWorkingTime % 60 == 0)
            //    {
            //        h++;
            //    }
            //    totalWorkingTime -= 60;
            //    if (totalWorkingTime < 60) break;
            //}
            //if (totalWorkingTime > 0)
            //{
            //    m = totalWorkingTime;
            //}
            //return h + " : " + m;
            string S = "";
            if (totalWorkingTime > 0)
            {
                if (totalWorkingTime / 60 >= 1) { S += ((totalWorkingTime - totalWorkingTime % 60) / 60).ToString() + " h. "; }
                if (totalWorkingTime % 60 != 0) { S += (totalWorkingTime % 60).ToString() + " m."; }
                return S;
            }
            else return "-";
        }

        public string ConvertIsActive(bool IsActive)
        {
            if (IsActive) return "Active";
            return "Inactive";
        }

        public string ShiftWithDuration
        {
            get
            {
                if (this.Name != "--Select Shift--")
                {
                    return this.Name + "(" + this.StartTimeInString + "-" + this.EndTimeInString + ")";
                }
                else { return this.Name; }
            }
        }
        public bool IsOTOnActual { get; set; }
        public int OTCalculateAfterInMinute { get; set; }
        public bool IsLeaveOnOFFHoliday { get; set; }
        #endregion

        #region derived property


        public List<HRMShift> HRMShifts { get; set; }
        public List<ShiftOTSlab> ShiftOTSlabs { get; set; }
        public List<ShiftBreakSchedule> ShiftBreakSchedules { get; set; }
        public Company Company { get; set; }

        public string TotalWorkingTimeInString
        {
            get
            {
                return ConvertTotalWorkingTime(TotalWorkingTime);
            }
        }

        public string IsActiveInString
        {
            get
            {
                return ConvertIsActive(IsActive);
            }
        }



        #endregion

        #region Functions

        public HRMShift Get(int id, long nUserID)
        {
            return HRMShift.Service.Get(id, nUserID);
        }
        public static List<HRMShift> Gets(long nUserID)
        {
            return HRMShift.Service.Gets(nUserID);
        }
        public static List<HRMShift> BUWiseGets(int BUID, long nUserID)
        {
            return HRMShift.Service.BUWiseGets(BUID, nUserID);
        }
        public HRMShift Save(long nUserID)
        {
            return HRMShift.Service.Save(this, nUserID);
        }
        public HRMShift Copy(long nUserID)
        {
            return HRMShift.Service.Copy(this, nUserID);
        }
        public string Delete(int id, long nUserID)
        {
            return HRMShift.Service.Delete(id, nUserID);
        }
        public static List<HRMShift> Gets(string sSQL, long nUserID)
        {
            return HRMShift.Service.Gets(sSQL, nUserID);
        }
        public HRMShift ShiftInActive(int nShiftID, int ntRShiftID,long nUserID)
        {
            return HRMShift.Service.ShiftInActive( nShiftID,  ntRShiftID, nUserID);
        }
        #endregion

        #region ServiceFactory
        internal static IHRMShiftService Service
        {
            get { return (IHRMShiftService)Services.Factory.CreateService(typeof(IHRMShiftService)); }
        }

        #endregion
    }
    #endregion


    #region IHRMShiftService interface

    public interface IHRMShiftService
    {
        HRMShift Get(int id, Int64 nUserID);
        List<HRMShift> Gets(Int64 nUserID);
        List<HRMShift> BUWiseGets(int BUID, Int64 nUserID);
        
        List<HRMShift> Gets(string sSQL, Int64 nUserID);
        string Delete(int id, Int64 nUserID);
        HRMShift Save(HRMShift oHRMShift, Int64 nUserID);
        HRMShift Copy(HRMShift oHRMShift, Int64 nUserID);
        HRMShift ShiftInActive(int nShiftID, int ntRShiftID, Int64 nUserID);
    }
    #endregion
}
