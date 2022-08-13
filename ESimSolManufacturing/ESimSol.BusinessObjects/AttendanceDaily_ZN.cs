using System;
using System.IO;
using ICS.Core;
using ICS.Core.Framework;
using ICS.Core.Utility;
using ESimSol.BusinessObjects;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Collections.Generic;


namespace ESimSol.BusinessObjects
{
    #region AttendanceDaily_ZN

    public class AttendanceDaily_ZN
    {
        public AttendanceDaily_ZN()
        {
            AttendanceID = 0;
            EmployeeID = 0;

            AttendanceSchemeID = 0;
            LocationID = 0;
            DepartmentID = 0;
            DesignationID = 0;
            RosterPlanID = 0;
            ShiftID = 0;
            AttendanceDate = DateTime.Now;
            InTime = DateTime.Now;
            OutTime = DateTime.MinValue;
            LateArrivalMinute = 0;
            EarlyDepartureMinute = 0;
            TotalWorkingHourInMinute = 0;
            OverTimeInMinute = 0;
            IsDayOff = false;
            IsLeave = false;
            IsUnPaid = false;
            WorkingStatus = EnumEmployeeWorkigStatus.None;
            Note = "";
            APMID = 0;
            IsLock = false;
            IsNoWork = false;
            IsAbsent = false;
            IsProductionBase = true;
            ErrorMessage = "";
            EmployeeName = "";
            EmployeeCode = "";
            AttendanceDateFrom = DateTime.Now;
            AttendanceDateTo = DateTime.Now;
            EmployeeIDs = "";
            DepartmentIDs = "";
            DesignationIDs = "";
            LeaveHeadID = 0;
            LastAttendanceDate = DateTime.Now;
            OT_NowWork_First=0;
            OT_NowWork_2nd=0;
            OT_NowWork_Rest = 0;
            AttendanceDaily_ZNs = new List<AttendanceDaily_ZN>();
            LeaveName = "";
            ShiftName = "";
            OT_NHR = 0;
            OT_HHR = 0;
            LeaveType = EnumLeaveType.None;
            LeaveDuration = 0;
            IsOSD = false;
            Remark = "";
            IsManualOT = false;
            IsPromoted = false;
            BanglaFont = "";
            CompanyName = "";
            GroupByID = 0;
            IsEnum = true;
            SLNo = "";

            PresentCounter = 0;
            AbsentCounter = 0;
            LateCounter = 0;
            EarlyCounter = 0;
            LeaveCounter = 0;
            DayoffCounter = 0;
            HolidayCounter = 0;
            OverTimeCounter = 0;

            UnitName = "";
            UnitAddress = "";
            EndDate = "";
            StartDate = "";
        }


        #region Properties

        public string EndDate { get; set; }
        public string StartDate { get; set; }
        public int PresentCounter { get; set; }
        public int AbsentCounter { get; set; }
        public int LateCounter { get; set; }
        public int EarlyCounter { get; set; }
        public int LeaveCounter { get; set; }
        public int DayoffCounter { get; set; }
        public int HolidayCounter { get; set; }
        public int OverTimeCounter { get; set; }
        public bool IsEnum { get; set; }
        public string BanglaFont { get; set; }
        public string UnitName { get; set; }
        public string UnitAddress { get; set; }
        public string CompanyName { get; set; }
        public string SLNo { get; set; }
        public int AttendanceID { get; set; }
        public int GroupByID { get; set; }

        public int EmployeeID { get; set; }

        public int AttendanceSchemeID { get; set; }

        public int LocationID { get; set; }

        public int DepartmentID { get; set; }

        public int DesignationID { get; set; }

        public int RosterPlanID { get; set; }

        public int ShiftID { get; set; }

        public DateTime AttendanceDate { get; set; }
        public DateTime ShiftStartTime { get; set; }
        public DateTime ShiftEndTime { get; set; }
        public DateTime AttendanceDateFrom { get; set; }
        public DateTime AttendanceDateTo { get; set; }

        public DateTime InTime { get; set; }

        public DateTime OutTime { get; set; }
        public DateTime CompInTime { get; set; }

        public DateTime CompOutTime { get; set; }

        public int LateArrivalMinute { get; set; }

        public int EarlyDepartureMinute { get; set; }

        public int TotalWorkingHourInMinute { get; set; }

        public int OverTimeInMinute { get; set; }

        public int CompLateArrivalMinute { get; set; }
        public int CompEarlyDepartureMinute { get; set; }
        public int CompTotalWorkingHourInMinute { get; set; }
        public int CompOverTimeInMinute { get; set; }
        public bool IsDayOff { get; set; }
        public bool IsLeave { get; set; }
        public bool IsUnPaid { get; set; }
        public bool IsHoliday { get; set; }
        public bool IsCompDayOff { get; set; }
        public bool IsAbsent { get; set; }
        public bool IsCompLeave { get; set; }
        public bool IsCompHoliday { get; set; }
        public bool IsPromoted { get; set; }
        public EnumEmployeeWorkigStatus WorkingStatus { get; set; }
        public string Note { get; set; }
        public int APMID { get; set; }
        public bool IsLock { get; set; }
        public bool IsNoWork { get; set; }
        public bool IsManual { get; set; }
        public int LastUpdatedBY { get; set; }
        public DateTime LastUpdatedDate { get; set; }
        public string ErrorMessage { get; set; }
        public string sRandom { get; set; }
        public string DepartmentIDs { get; set; }
        public string EmployeeIDs { get; set; }
        public string DesignationIDs { get; set; }
        public string LeaveName { get; set; }
        public string ShiftName { get; set; }
        public double OT_NHR { get; set; }
        public double OT_HHR { get; set; }
        public EnumLeaveType LeaveType { get; set; }
        public int LeaveDuration { get; set; }
        public bool IsOSD { get; set; }
        public bool IsManualOT { get; set; }
        
        #endregion

        #region Derived Property

        public string EmployeeName { get; set; }

        public string EmployeeCode { get; set; }

        public string AttendanceSchemeName { get; set; }

        public string DepartmentName { get; set; }

        public string DesignationName { get; set; }

        public string HRM_ShiftName { get; set; }

        public string EmployeeTypeName { get; set; }

        public string RosterPlanName { get; set; }

        public string LocationName { get; set; }

        public int OT_NowWork_First { get; set; }
        public int OT_NowWork_2nd { get; set; }
        public int OT_NowWork_Rest { get; set; }

        public List<AttendanceDaily_ZN> AttendanceDaily_ZNs { get; set; }

        public List<Employee> Employees { get; set; }
        public List<MaxOTConfiguration> MaxOTConfiguration { get; set; }

        public EmployeeSalary EmployeeSalary { get; set; }

        public Company Company { get; set; }
        public string BUName { get; set; }
        public string BUAddress { get; set; }
        public string Remark { get; set; }

        public bool IsProductionBase { get; set; }

        public DateTime JoiningDate { get; set; }


        public DateTime DateOfBirth { get; set; }

        public int LeaveHeadID { get; set; }

        public DateTime LastAttendanceDate { get; set; }

        //=============properties for report

        public int TotalWorkingDay { get; set; }

        public int TotalShift { get; set; }

        public double TotalWorkingHour { get; set; }

        public int PresentShift { get; set; }

        public int AbsentShift { get; set; }

        public int IsDayOFFs { get; set; }

        public int Leave { get; set; }

        public int Paid { get; set; }

        public int InWorkPlace { get; set; }

        public int OSD { get; set; }

        public int Suspended { get; set; }

        public int Holiday { get; set; }

        public int Late { get; set; }

        public int EarlyLeaving { get; set; }

        public double OvertimeInhour { get; set; }

        public List<EmployeeType> EmployeeTypes { get; set; }
        public List<LeaveHead> LeaveHeads { get; set; }
        public List<BusinessUnit> BusinessUnits { get; set; }

        //=============


        public AttendanceSummery_Chart AttendanceSummery_Chart { get; set; }
        public AttendancePerformanceChart AttendancePerformanceChart { get; set; }

        public List<AttendanceSummery_List> AttendanceSummery_Lists { get; set; }

        public List<AttendanceCalendarSession> AttendanceCalendarSessions { get; set; }
        public int WorkingStatusInt { get; set; }
        public string AttStatusInString
        {
            get
            {
                string sAttStatus = "";
                if (this.InTimeInString_12hr == "-" && this.IsLeave == false && this.IsHoliday == false && this.IsDayOff == false)
                {
                    sAttStatus += "A,";
                }
                else
                {                    
                    if (this.IsDayOff == true)
                    {
                        sAttStatus += "Off,";
                        if (this.InTimeInString_12hr != "-" || this.OutTimeInString_12hr != "-")
                        {
                           sAttStatus += "P,";
                            if (this.LateArrivalMinute > 0)
                            {
                                sAttStatus += "Late,";
                            }
                            if (this.EarlyDepartureMinute > 0)
                            {
                                sAttStatus += "Early,";
                            }
                        }
                    }
                    else if (this.IsHoliday == true)
                    {
                        sAttStatus += "HD,";
                        if (this.InTimeInString_12hr != "-" || this.OutTimeInString_12hr != "-")
                        {
                           sAttStatus += "P,";
                            if (this.LateArrivalMinute > 0)
                            {
                                sAttStatus += "Late,";
                            }
                            if (this.EarlyDepartureMinute > 0)
                            {
                                sAttStatus += "Early,";
                            }
                        }
                    }
                    else if (this.IsLeave == true)
                    {
                        sAttStatus += this.LeaveName + ",";
                        if (this.InTimeInString_12hr != "-" || this.OutTimeInString_12hr != "-")
                        {
                            sAttStatus += "P,";
                            if (this.LateArrivalMinute > 0)
                            {
                                sAttStatus += "Late,";
                            }
                            if (this.EarlyDepartureMinute > 0)
                            {
                                sAttStatus += "Early,";
                            }
                        }
                    }
                    else
                    {
                        if (this.InTimeInString_12hr != "-" || this.OutTimeInString_12hr != "-")
                        {
                            sAttStatus += "P,";
                            if (this.LateArrivalMinute > 0)
                            {
                                sAttStatus += "Late,";
                            }
                            if (this.EarlyDepartureMinute > 0)
                            {
                                sAttStatus += "Early,";
                            }
                        }
                    }
                }
                if(sAttStatus != "")
                    sAttStatus = sAttStatus.Remove(sAttStatus.Length - 1, 1);
                return sAttStatus;
            }
        }
        public string WorkingStatusInString
        {
            get
            {
                return WorkingStatus.ToString();
            }
        }
        public string TotalWorkingHourInMinuteSt
        {
            get
            {
                if (TotalWorkingHourInMinute > 0)
                    return TotalWorkingHourInMinute.ToString();
                else
                    return "-";
            }
        }
        public string TotalWorkingDaySt
        {
            get
            {
                if (TotalWorkingDay > 0)
                    return TotalWorkingDay.ToString();
                else
                    return "-";
            }
        }
        public string AttendanceDateInString
        {
            get
            {
                return AttendanceDate.ToString("dd MMM yyyy");
            }
        }
        public string AttendanceDateInStringForBangla
        {
            get
            {
                return AttendanceDate.ToString("dd-MM-yyyy");
            }
        }
        public string JoiningDateInString
        {
            get
            {
                return JoiningDate.ToString("dd MMM yyyy");
            }
        }
        public string JoiningDateInStringBijoy
        {
            get
            {
                return JoiningDate.ToString("dd/MM/yyyy");
            }
        }
        public string LastAttendanceDateInString
        {
            get
            {
                return LastAttendanceDate.ToString("dd MMM yyyy");
            }
        }
        public string DateOfBirthInString
        {
            get
            {
                return DateOfBirth.ToString("dd MMM yyyy");
            }
        }



        public string InTimeInStringWithDate
        {
            get
            {
                if (InTime.ToString("HH:mm") != "00:00")
                    return InTime.ToString("MM/dd/yyyy HH:mm");
                else
                    return "-";
            }
        }

        public string InTimeInString
        {
            get
            {
                if (InTime.ToString("HH:mm") != "00:00")
                    return InTime.ToString("HH:mm");
                else
                    return "-";
            }
        }
        public string InTimeInString_12hr
        {
            get
            {
                if (InTime.ToString("HH:mm") != "00:00")
                    return InTime.ToString("hh:mm tt");
                else
                    return "-";
            }
        }
        public string CompInTimeInString_12hr
        {
            get
            {
                if (CompInTime.ToString("HH:mm") != "00:00")
                    return CompInTime.ToString("hh:mm tt");
                else
                    return "-";
            }
        }
        public string OutTimeInStringWithDate
        {
            get
            {
                if (OutTime.ToString("HH:mm") != "00:00")
                    return OutTime.ToString("MM/dd/yyyy HH:mm");
                else
                    return "-";
            }
        }
        public string OutTimeInString
        {
            get
            {
                if (OutTime.ToString("HH:mm") != "00:00")
                    return OutTime.ToString("HH:mm");
                else
                    return "-";
            }
        }

        public string OutTimeInString_12hr
        {
            get
            {
                if (OutTime.ToString("HH:mm") != "00:00")
                    return OutTime.ToString("hh:mm tt");
                else
                    return "-";
            }
        }
        public string CompOutTimeInString_12hr
        {
            get
            {
                if (CompOutTime.ToString("HH:mm") != "00:00")
                    return CompOutTime.ToString("hh:mm tt");
                else
                    return "-";
            }
        }
        public string OutTimeInDateFormat
        {
            get
            {
                return (this.OutTime == DateTime.MinValue) ? "-" : this.OutTime.ToString("dd/MM/yyyy");
            }
        }

        public string CompInTimeInString
        {
            get
            {
                if (CompInTime.ToString("HH:mm") != "00:00")
                    return CompInTime.ToString("HH:mm");
                else
                    return "-";
            }
        }
        public string CompOverTimeInMinuteHourSt
        {
            get
            {
                string S = "";
                if (CompOverTimeInMinute > 0)
                {
                    if (CompOverTimeInMinute / 60 >= 1) { S += ((CompOverTimeInMinute - CompOverTimeInMinute % 60) / 60).ToString() + "h "; }
                    if (CompOverTimeInMinute % 60 != 0) { S += (CompOverTimeInMinute % 60).ToString() + "m"; }
                    return S;
                }
                else return "-";
            }
        }
        public string CompTotalWorkingHourSt
        {
            get
            {
                string S = "";
                if (this.CompTotalWorkingHourInMinute > 0)
                {
                    if (CompTotalWorkingHourInMinute / 60 >= 1) { S += ((CompTotalWorkingHourInMinute - CompTotalWorkingHourInMinute % 60) / 60).ToString() + "h "; }
                    if (CompTotalWorkingHourInMinute % 60 != 0) { S += (CompTotalWorkingHourInMinute % 60).ToString() + "m"; }
                    return S;
                }
                else return "-";
            }
        }
        public string CompOutTimeInString
        {
            get
            {
                if (CompOutTime.ToString("HH:mm") != "00:00")
                    return CompOutTime.ToString("HH:mm");
                else
                    return "-";
            }
        }

        public string OvertimeTimeInString
        {
            get
            {
                if (OverTimeInMinute > 0)
                    return ((OverTimeInMinute / 60).ToString()).Split('.')[0] + ":" + (OverTimeInMinute % 60).ToString();
                else
                    return "-";
            }
        }
        public string OverTimeInMinuteHourSt
        {
            get
            {
                string S = "";
                if (OverTimeInMinute > 0)
                {
                    if (OverTimeInMinute / 60 >= 1) { S += ((OverTimeInMinute - OverTimeInMinute % 60) / 60).ToString() + "h "; }
                    if (OverTimeInMinute % 60 != 0) { S += (OverTimeInMinute % 60).ToString() + "m"; }
                    return S;
                }
                else return "-";
            }
        }
        public string LateArrivalMinuteSt
        {
            get
            {
                if (LateArrivalMinute > 0) return LateArrivalMinute.ToString(); else return "-";
            }
        }
        public string LateArrivalHourSt
        {
            get
            {
                string S = "";
                if (LateArrivalMinute > 0)
                {
                    if (LateArrivalMinute / 60 >= 1) { S += ((LateArrivalMinute - LateArrivalMinute % 60) / 60).ToString() + "h "; }
                    if (LateArrivalMinute % 60 != 0) { S += (LateArrivalMinute % 60).ToString() + "m"; }
                    return S;
                }
                else return "-";
            }
        }
        public string EarlyDepartureMinuteSt
        {
            get
            {
                if (EarlyDepartureMinute > 0) return EarlyDepartureMinute.ToString(); else return "-";
            }
        }
        public string EarlyDepartureHrSt
        {
            get
            {
                string S = "";
                if (EarlyDepartureMinute > 0)
                {
                    if (EarlyDepartureMinute / 60 >= 1) { S += ((EarlyDepartureMinute - EarlyDepartureMinute % 60) / 60).ToString() + "h "; }
                    if (EarlyDepartureMinute % 60 != 0) { S += (EarlyDepartureMinute % 60).ToString() + "m"; }
                    return S;
                }
                else return "-";
            }
        }
        public string LateArrivalMinuteInString
        {
            get
            {
                if (LateArrivalMinute > 0) return "Late"; else return "";
            }
        }

        public string EarlyDepartureMinuteInString
        {
            get
            {
                if (EarlyDepartureMinute > 0 && LateArrivalMinute > 0) return ",Early Leave"; else if (EarlyDepartureMinute > 0 && LateArrivalMinute <= 0) return "Early Leave"; else return "";
            }
        }
        public string IsLeaveInString
        {
            get
            {
                if (IsLeave == true && (LateArrivalMinute > 0 || EarlyDepartureMinute > 0)) 
                    return ",Leave"; 
                else if (IsLeave == true && EarlyDepartureMinute <= 0 && LateArrivalMinute <= 0) 
                    return "Leave"; 
                else return "";
            }
        }
        public string IsDayOffInString
        {
            get
            {
                if (IsDayOff == true && (IsLeave == true || LateArrivalMinute > 0 || EarlyDepartureMinute > 0)) return ",DayOff"; else if (IsDayOff == true && IsLeave == false && LateArrivalMinute <= 0 && EarlyDepartureMinute <= 0) return "DayOff"; else return "";
            }
        }
        public string IsOSDString
        {
            get
            {
                if (this.IsOSD == true) return ",OSD"; else return "";
            }
        }
        public string IsHoliDayInString
        {
            get
            {
                if (IsHoliday == true && (IsDayOff == true || IsLeave == true || LateArrivalMinute > 0 || EarlyDepartureMinute > 0)) return ",Holiday"; else if (IsHoliday == true && IsDayOff == false && IsLeave == false && LateArrivalMinute <= 0 && EarlyDepartureMinute <= 0) return "Holiday"; else return "";
            }
        }
        public string IsPaidInString
        {
            get
            {
                if (IsUnPaid == true && (IsHoliday == true || IsDayOff == true || IsLeave == true ))//|| LateArrivalMinute > 0 || EarlyDepartureMinute > 0)) 
                    return ",UnPaid"; 
                else if (IsUnPaid == true && IsHoliday == false && IsDayOff == false && IsLeave == false)// && LateArrivalMinute <= 0 && EarlyDepartureMinute <= 0) 
                    return "UnPaid"; 
                else return "";
            }
        }
        public string IsPaidInStringComp
        {
            get
            {
                if (IsUnPaid == true && (IsHoliday == true || IsDayOff == true || IsLeave == true))//|| LateArrivalMinute > 0 || EarlyDepartureMinute > 0)) 
                    return ",LWP";
                else if (IsUnPaid == true && IsHoliday == false && IsDayOff == false && IsLeave == false)// && LateArrivalMinute <= 0 && EarlyDepartureMinute <= 0) 
                    return "LWP";
                else return "";
            }
        }
        public string AttDescriptionInString
        {
            get
            {
                return LateArrivalMinuteInString + EarlyDepartureMinuteInString + IsLeaveInString + IsDayOffInString + IsHoliDayInString + IsPaidInString + IsOSDString;
            }
        }
        public string AttDescriptionInStringComp
        {
            get
            {
                return LateArrivalMinuteInString + EarlyDepartureMinuteInString + IsLeaveInString + IsDayOffInString + IsHoliDayInString + IsPaidInStringComp + IsOSDString;
            }
        }

        public string IsNoWorkInString
        {
            get
            {
                if (IsNoWork == true) return "No Work"; else return "-";
            }
        }

        //public string Name//EmployeeName
        //{
        //    get
        //    {
        //        if (EmployeeNameCode != "") return EmployeeNameCode.Split('[')[0]; else return "";
        //    }
        //}
        public string EmployeeNameCode
        {
            get
            {
                return EmployeeName + "[" + EmployeeCode + "]";
            }
        }

        public string OTHrSt
        {
            get
            {
                return Global.MinInHourMin(this.OverTimeInMinute);
            }
        }
        public string TotalWorkingHourSt
        {
            get
            {
                return Global.MinInHourMin(this.TotalWorkingHourInMinute);
            }
        }
        public string OT_NowWork_FirstST
        {
            get
            {
                if(this.IsNoWork)
                    return "NW";
                else
                return Global.MinInHourMin(this.OT_NowWork_First);
            }
        }
        public string OT_NowWork_2ndST
        {
            get
            {
                if (this.IsNoWork)
                    return "NW";
                else
                return Global.MinInHourMin(this.OT_NowWork_2nd);
            }
        }

        public List<HRMShift> HRMShifts { get; set; }
        #endregion

        #region Functions


        public static List<AttendanceDaily_ZN> Gets(string sSQL, long nUserID)
        {
            return AttendanceDaily_ZN.Service.Gets(sSQL, nUserID);
        }
        public static AttendanceDaily_ZN Get(string sSQL, long nUserID)
        {
            return AttendanceDaily_ZN.Service.Get(sSQL, nUserID);
        }
        public AttendanceDaily_ZN Get(int nEmployeeID, DateTime dAttendanceDate, long nUserID)
        {
            return AttendanceDaily_ZN.Service.Get(nEmployeeID, dAttendanceDate, nUserID);
        }
        public static List<AttendanceDaily_ZN> GetsForReport(string sSQL, long nUserID)
        {
            return AttendanceDaily_ZN.Service.GetsForReport(sSQL, nUserID);
        }
        public AttendanceDaily_ZN IUD(int nDBOperation, long nUserID)
        {
            return AttendanceDaily_ZN.Service.IUD(this, nDBOperation, nUserID);
        }
        public static List<AttendanceDaily_ZN> NoWorkExecution(List<AttendanceDaily_ZN> oAttendanceDaily_ZNs, long nUserID)
        {
            return AttendanceDaily_ZN.Service.NoWorkExecution(oAttendanceDaily_ZNs, nUserID);
        }
        public static List<AttendanceDaily_ZN> CancelNoWorkExecution(List<AttendanceDaily_ZN> oAttendanceDaily_ZNs, long nUserID)
        {
            return AttendanceDaily_ZN.Service.CancelNoWorkExecution(oAttendanceDaily_ZNs, nUserID);
        }
        public AttendanceDaily_ZN ManualAttendance_Update(int nDBOperation, long nUserID)
        {
            return AttendanceDaily_ZN.Service.ManualAttendance_Update(this, nDBOperation, nUserID);
        }
        public static List<AttendanceDaily_ZN> UploadAttendanceXL(List<AttendanceDaily_ZN> oAttendanceDaily_ZNs, long nUserID)
        {
            return AttendanceDaily_ZN.Service.UploadAttendanceXL(oAttendanceDaily_ZNs, nUserID);
        }
        public static List<AttendanceDaily_ZN> GetsDayWiseAbsent(int nDays, DateTime dDate, long nUserID)
        {
            return AttendanceDaily_ZN.Service.GetsDayWiseAbsent(nDays, dDate, nUserID);
        }
        public static List<AttendanceDaily_ZN> EmployeeWiseReprocessComp(int EmployeeID, DateTime Startdate, DateTime EndDate, long nUserID)
        {
            return AttendanceDaily_ZN.Service.EmployeeWiseReprocessComp(EmployeeID, Startdate, EndDate, nUserID);
        }
        public int ProcessCompAsPerEdit(string EmployeeID, DateTime Startdate, DateTime EndDate, int MOCID, int nIndex, string sLocationIDs, string sBUIDs, long nUserID)
        {
            return AttendanceDaily_ZN.Service.ProcessCompAsPerEdit(EmployeeID, Startdate, EndDate, MOCID, nIndex, sLocationIDs, sBUIDs, nUserID);
        }
        public static List<AttendanceDaily_ZN> GetsTimeCard(string sEmployeeIDs, DateTime Startdate, DateTime EndDate, string sLocationID, string sDepartmentIds, string sType, string sBUnitIDs, double nStartSalaryRange, double nEndSalaryRange, string sBlockIDs, string sGroupIDs, long nUserID)
        {
            return AttendanceDaily_ZN.Service.GetsTimeCard(sEmployeeIDs, Startdate, EndDate, sLocationID, sDepartmentIds, sType, sBUnitIDs, nStartSalaryRange, nEndSalaryRange, sBlockIDs, sGroupIDs, nUserID);
        }
        public static List<AttendanceDaily_ZN> GetsTimeCardComp(string sEmployeeIDs, DateTime Startdate, DateTime EndDate, string sLocationID, string sDepartmentIds, string sType, string sBUnitIDs, double nStartSalaryRange, double nEndSalaryRange, string sBlockIDs, string sGroupIDs, long nUserID)
        {
            return AttendanceDaily_ZN.Service.GetsTimeCardComp(sEmployeeIDs, Startdate, EndDate, sLocationID, sDepartmentIds, sType, sBUnitIDs, nStartSalaryRange, nEndSalaryRange, sBlockIDs, sGroupIDs, nUserID);
        }
        public static List<AttendanceDaily_ZN> GetsTimeCardMaxOTConf(string sEmployeeIDs, DateTime Startdate, DateTime EndDate, string sLocationID, string sDepartmentIds, string sBUnitIDs, double nStartSalaryRange, double nEndSalaryRange, string sBlockIDs, string sGroupIDs, int nMOCID, long nUserID)
        {
            return AttendanceDaily_ZN.Service.GetsTimeCardMaxOTConf(sEmployeeIDs, Startdate, EndDate, sLocationID, sDepartmentIds, sBUnitIDs, nStartSalaryRange, nEndSalaryRange, sBlockIDs, sGroupIDs, nMOCID, nUserID);
        }
        public static List<AttendanceDaily_ZN> GetsTimeCardMaxOTConfSearch(string sEmployeeIDs, DateTime Startdate, DateTime EndDate, string sLocationID, string sDepartmentIds, string sBUnitIDs, double nStartSalaryRange, double nEndSalaryRange, string sBlockIDs, string sGroupIDs, int nMOCID, long nUserID)
        {
            return AttendanceDaily_ZN.Service.GetsTimeCardMaxOTConfSearch(sEmployeeIDs, Startdate, EndDate, sLocationID, sDepartmentIds, sBUnitIDs, nStartSalaryRange, nEndSalaryRange, sBlockIDs, sGroupIDs, nMOCID, nUserID);
        }
        

        #endregion

        #region ServiceFactory

        internal static IAttendanceDaily_ZNService Service
        {
            get { return (IAttendanceDaily_ZNService)Services.Factory.CreateService(typeof(IAttendanceDaily_ZNService)); }
        }
        #endregion
    }
    #endregion

    #region IAttendanceDaily_ZN interface

    public interface IAttendanceDaily_ZNService
    {
        AttendanceDaily_ZN Get(int nEmployeeID, DateTime dAttendanceDate, Int64 nUserID);
        List<AttendanceDaily_ZN> Gets(string sSQL, Int64 nUserID);
        AttendanceDaily_ZN Get(string sSQL, Int64 nUserID);
        int ProcessCompAsPerEdit(string EmployeeID, DateTime Startdate, DateTime EndDate, int MOCID, int nIndex, string sLocationIDs, string sBUIDs, Int64 nUserID);
        List<AttendanceDaily_ZN> EmployeeWiseReprocessComp(int EmployeeID, DateTime Startdate, DateTime EndDate, Int64 nUserID);
        List<AttendanceDaily_ZN> GetsForReport(string sSQL, Int64 nUserID);
        AttendanceDaily_ZN IUD(AttendanceDaily_ZN oAttendanceDaily_ZN, int nDBOperation, Int64 nUserID);
        List<AttendanceDaily_ZN> NoWorkExecution(List<AttendanceDaily_ZN> oAttendanceDaily_ZNs, Int64 nUserID);
        List<AttendanceDaily_ZN> CancelNoWorkExecution(List<AttendanceDaily_ZN> oAttendanceDaily_ZNs, Int64 nUserID);
        AttendanceDaily_ZN ManualAttendance_Update(AttendanceDaily_ZN oAttendanceDaily_ZN, int nDBOperation, Int64 nUserID);
        List<AttendanceDaily_ZN> UploadAttendanceXL(List<AttendanceDaily_ZN> oAttendanceDaily_ZNs, Int64 nUserID);
        List<AttendanceDaily_ZN> GetsDayWiseAbsent(int nDays, DateTime dDate, Int64 nUserID);
        List<AttendanceDaily_ZN> GetsTimeCard(string sEmployeeIDs, DateTime Startdate, DateTime EndDate, string sLocationID, string sDepartmentIds, string sType, string sBUnitIDs, double nStartSalaryRange, double nEndSalaryRange, string sBlockIDs, string sGroupIDs, Int64 nUserID);
        List<AttendanceDaily_ZN> GetsTimeCardComp(string sEmployeeIDs, DateTime Startdate, DateTime EndDate, string sLocationID, string sDepartmentIds, string sType, string sBUnitIDs, double nStartSalaryRange, double nEndSalaryRange, string sBlockIDs, string sGroupIDs, Int64 nUserID);

        List<AttendanceDaily_ZN> GetsTimeCardMaxOTConf(string sEmployeeIDs, DateTime Startdate, DateTime EndDate, string sLocationID, string sDepartmentIds, string sBUnitIDs, double nStartSalaryRange, double nEndSalaryRange, string sBlockIDs, string sGroupIDs, int nMOCID, long nUserID);
        List<AttendanceDaily_ZN> GetsTimeCardMaxOTConfSearch(string sEmployeeIDs, DateTime Startdate, DateTime EndDate, string sLocationID, string sDepartmentIds, string sBUnitIDs, double nStartSalaryRange, double nEndSalaryRange, string sBlockIDs, string sGroupIDs, int nMOCID, long nUserID);
    }
    #endregion
}
