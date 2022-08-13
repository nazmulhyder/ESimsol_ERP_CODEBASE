using System;
using System.IO;
using ICS.Core;
using ICS.Core.Framework;
using ICS.Core.Utility;
using ESimSol.BusinessObjects;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Collections.Generic;
using System.Data;

namespace ESimSol.BusinessObjects
{
    #region AttendanceDaily
    public class AttendanceDaily
    {
        public AttendanceDaily()
        {
            AttendanceID = 0;
            BusinessUnitID = 0;
            EmployeeID = 0;
            AttendanceSchemeID = 0;
            LocationID = 0;
            DepartmentID = 0;
            DesignationID = 0;
            RosterPlanID = 0;
            ShiftID = 0;
            AttendanceDate = DateTime.Now;
            InTime = DateTime.Now;
            OutTime = DateTime.Now;
            LateArrivalMinute = 0;
            CompLateArrivalMinute = 0;
            EarlyDepartureMinute = 0;
            CompEarlyDepartureMinute = 0;
            TotalWorkingHourInMinute = 0;
            OverTimeInMinute = 0;
            IsDayOff = false;
            IsCompDayOff = false;
            IsLeave = false;
            IsUnPaid = false;
            WorkingStatus = EnumEmployeeWorkigStatus.None;
            Note = "";
            APMID = 0;
            IsLock = false;
            IsNoWork = false;
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
            CompLeaveHeadID = 0;
            LastAttendanceDate=  DateTime.Now;
            LeaveStatus = "";
            IsHoliday = false;
            IsOSD = false;
            LeaveType = EnumLeaveType.None;
            BUName = "";
            Remark = "";
            IsAbsent = false;
            IsManualOT = false;
            IsRemarked = false;
            LastPunchDate = DateTime.Now;
            AbsentFrom = DateTime.Now;
            AbsentDayCount = 0;
            TotalAttendanceCount = 0;
            Gender = "";
            AttendanceDailys = new List<AttendanceDaily>();
            StartDate = DateTime.Now;
            EndDate = DateTime.Now;
            MOCID = 0;
            MOCAID = 0;
        }

        #region Properties

        public int MOCID { get; set; }
        public int MOCAID { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string Gender { get; set; }
        public int AttendanceID { get; set; }
        public int TotalAttendanceCount { get; set; }
        public int BusinessUnitID { get; set; }
        public bool IsRemarked { get; set; }
        public int EmployeeID { get; set; }
        public int AttendanceSchemeID { get; set; }
        public int LocationID { get; set; }
        public int DepartmentID { get; set; }
        public int DesignationID { get; set; }
        public int RosterPlanID { get; set; }
        public int ShiftID { get; set; }
        public DateTime AttendanceDate { get; set; }
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
        public int CompLateArrivalMinute  { get; set; }
        public int CompEarlyDepartureMinute  { get; set; }
        public int CompTotalWorkingHourInMinute { get; set; }
        public int CompOverTimeInMinute { get; set; }
        public bool IsDayOff { get; set; }
        public bool IsLeave { get; set; }
        public bool IsUnPaid { get; set; }
        public bool IsHoliday { get; set; }
        public bool IsAbsent { get; set; }
        public bool IsOSD { get; set; }
        public bool IsCompDayOff { get; set; }
        public bool IsCompLeave { get; set; }
        public bool IsCompHoliday { get; set; }
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
        public string LeaveStatus { get; set; }
        public EnumLeaveType LeaveType { get; set; }
        #endregion

        #region Derived Property

        public DateTime LastPunchDate { get; set; }
        public DateTime AbsentFrom { get; set; }
        public int AbsentDayCount { get; set; }
        public string EmployeeName { get; set; }
        public string EmployeeCode { get; set; }
        public string AttendanceSchemeName { get; set; }
        public string DepartmentName { get; set; }
        public string DesignationName { get; set; }
        public string HRM_ShiftName { get; set; }
        public string EmployeeTypeName { get; set; }
        public string RosterPlanName { get; set; }
        public string LocationName { get; set; }
        public List<AttendanceDaily> AttendanceDailys { get; set; }
        public List<Employee> Employees { get; set; }
        public EmployeeSalary EmployeeSalary { get; set; }
        public Company Company { get; set; }
        public bool IsProductionBase { get; set; }
        public DateTime JoiningDate { get; set; }
        public DateTime DateOfBirth { get; set; }
        public int LeaveHeadID { get; set; }
        public int CompLeaveHeadID { get; set; }
        public DateTime LastAttendanceDate { get; set; }
        public string BUName { get; set; }
        public string Remark { get; set; }
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
        public int Holiday { get; set;} 
        public int Late { get; set; }
        public int EarlyLeaving { get; set; }
        public double OvertimeInhour { get; set; }
        public List<EmployeeType> EmployeeTypes { get; set; }

        //=============

        public AttendanceSummery_Chart AttendanceSummery_Chart { get; set; }
        public AttendancePerformanceChart AttendancePerformanceChart { get; set; }
        
        public List<AttendanceSummery_List> AttendanceSummery_Lists { get; set; }
        
        public List<AttendanceCalendarSession> AttendanceCalendarSessions { get; set; }
        public int WorkingStatusInt { get; set; }


        public string LastPunchDateInString
        {
            get
            {
                return LastPunchDate.ToString("dd MMM yyyy");
            }
        }
        public string AbsentFromInString
        {
            get
            {
                return AbsentFrom.ToString("dd MMM yyyy");
            }
        }



        public string WorkingStatusInString
        {
            get
            {
                return WorkingStatus.ToString();
            }
        }
        public string EmployeeWorkingStatusInST
        {
            get
            {
                return this.WorkingStatus == EnumEmployeeWorkigStatus.InWorkPlace ? "Continued" : "Discontinued";
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
        public string JoiningDateInString
        {
            get
            {
                return JoiningDate.ToString("dd MMM yyyy");
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
        public string CompInTimeInStringWithDate
        {
            get
            {
                if (CompInTime.ToString("HH:mm") != "00:00")
                    return CompInTime.ToString("MM/dd/yyyy HH:mm");
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
        public string CompOutTimeInStringWithDate
        {
            get
            {
                if (CompOutTime.ToString("HH:mm") != "00:00")
                    return CompOutTime.ToString("MM/dd/yyyy HH:mm");
                else
                    return "-";
            }
        }
        public string InTimeInStringAMPM
        {
            get
            {
                if (InTime.ToString("HH:mm") != "00:00")
                    return InTime.ToString("hh:mm tt");
                else
                    return "-";
            }
        }
        public string CompInTimeInStringAMPM
        {
            get
            {
                if (CompInTime.ToString("HH:mm") != "00:00")
                    return CompInTime.ToString("hh:mm tt");
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
        public string OutTimeInStringAMPM
        {
            get
            {
                if (OutTime.ToString("HH:mm") != "00:00")
                    return OutTime.ToString("hh:mm tt");
                else
                    return "-";
            }
        }
        public string CompOutTimeInStringAMPM
        {
            get
            {
                if (CompOutTime.ToString("HH:mm") != "00:00")
                    return CompOutTime.ToString("hh:mm tt");
                else
                    return "-";
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
        public string LateArrivalMinuteSt
        {
            get
            {
                if (LateArrivalMinute > 0) return LateArrivalMinute.ToString(); else return "-";
            }
        }
        public string CompLateArrivalMinuteSt
        {
            get
            {
                if (CompLateArrivalMinute > 0) return CompLateArrivalMinute.ToString(); else return "-";
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
        public string CompLateArrivalHourSt
        {
            get
            {
                string S = "";
                if (CompLateArrivalMinute > 0)
                {
                    if (CompLateArrivalMinute / 60 >= 1) { S += ((CompLateArrivalMinute - CompLateArrivalMinute % 60) / 60).ToString() + "h "; }
                    if (CompLateArrivalMinute % 60 != 0) { S += (CompLateArrivalMinute % 60).ToString() + "m"; }
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
        public string CompEarlyDepartureMinuteSt
        {
            get
            {
                if (CompEarlyDepartureMinute > 0) return CompEarlyDepartureMinute.ToString(); else return "-";
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
        public string CompEarlyDepartureHrSt
        {
            get
            {
                string S = "";
                if (CompEarlyDepartureMinute > 0)
                {
                    if (CompEarlyDepartureMinute / 60 >= 1) { S += ((CompEarlyDepartureMinute - CompEarlyDepartureMinute % 60) / 60).ToString() + "h "; }
                    if (CompEarlyDepartureMinute % 60 != 0) { S += (CompEarlyDepartureMinute % 60).ToString() + "m"; }
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

        public string IsOSDString
        {
            get
            {
                if (this.IsOSD==true) return ",OSD"; else return "";
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
                if (IsLeave == true && (LateArrivalMinute > 0 || EarlyDepartureMinute > 0)) return ",Leave"; else if (IsLeave == true && EarlyDepartureMinute <= 0 && LateArrivalMinute <= 0) return "Leave"; else return "";
            }
        }
        public string IsDayOffInString
        {
            get
            {
                if (IsDayOff == true && (IsLeave == true || LateArrivalMinute > 0 || EarlyDepartureMinute > 0)) return ",DayOff"; else if (IsDayOff == true && IsLeave == false && LateArrivalMinute <= 0 && EarlyDepartureMinute <= 0) return "DayOff"; else return "";
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
                if (IsUnPaid == true && (IsHoliday == true || IsDayOff == true || IsLeave == true || LateArrivalMinute > 0 || EarlyDepartureMinute > 0)) return ",UnPaid"; else if (IsUnPaid == true && IsHoliday == false && IsDayOff == false && IsLeave == false && LateArrivalMinute <= 0 && EarlyDepartureMinute <= 0) return "UnPaid"; else return "";
            }
        }
        public string AttDescriptionInString
        {
            get
            {
                return LateArrivalMinuteInString + EarlyDepartureMinuteInString + IsLeaveInString + IsDayOffInString + IsHoliDayInString + IsPaidInString + IsOSDString;
            }
        }

        public string AttStatusInString
        {
            get
            {
                //string sAttStatus = "";
                //if (this.InTimeInString == "-" && this.OutTimeInString == "-" && this.IsLeave == false && this.IsHoliday == false && this.IsDayOff == false)
                //{
                //    sAttStatus += "A,";
                //}
                //else
                //{

                //    if (this.IsLeave == true)
                //    {
                //        //sAttStatus += "Leave,";
                //        sAttStatus += (this.LeaveHeadID > 0 ? this.LeaveStatus : "Leave")+",";
                //    }
                //    if (this.LateArrivalMinute > 0)
                //    {
                //        sAttStatus += "Late,";
                //    }
                //    if (this.IsHoliday == true)
                //    {
                //        sAttStatus += "HD,";
                //    }
                //    if (this.EarlyDepartureMinute > 0)
                //    {
                //        sAttStatus += "Early,";
                //    }
                //    if (this.IsDayOff == true)
                //    {
                //        sAttStatus = "Off,";
                //    }
                //    sAttStatus += "P,";
                //}

                //sAttStatus = sAttStatus.Remove(sAttStatus.Length-1, 1);
                //return sAttStatus;

                string sAttStatus = "";
                if (this.InTimeInString == "-" && this.OutTimeInString == "-" && this.IsOSD == false && this.IsLeave == false && this.IsHoliday == false && this.IsDayOff == false)
                {
                    sAttStatus += "A,";
                }
                else
                {
                    if ((this.InTimeInString != "-" || this.OutTimeInString != "-" || this.IsOSD == true))// && (this.IsDayOff == false))
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
                    if (this.IsDayOff == true)// && (this.OutTimeInString == "-" && this.OutTimeInString == "-"))
                    {
                        sAttStatus += "Off,";
                    }
                    if (this.IsHoliday == true)
                    {
                        sAttStatus += "HD,";
                    }
                    if (this.IsLeave == true)
                    {
                        //sAttStatus += this.LeaveName + ",";
                        sAttStatus += (this.LeaveHeadID > 0 ? this.LeaveStatus : "Leave") + ",";
                    }

                }
                if (sAttStatus != "")
                    sAttStatus = sAttStatus.Remove(sAttStatus.Length - 1, 1);
                return sAttStatus;
            }
        }

        public string AttStatusInString_usedInLeave
        {
            get
            {
                string sAttStatus = "";
                if (this.InTimeInString == "-" && this.OutTimeInString == "-" && this.IsOSD == false && this.IsLeave == false && this.IsHoliday == false && this.IsDayOff == false)
                {
                    sAttStatus += "A,";
                }
                else
                {
                    if ((this.InTimeInString != "-" || this.OutTimeInString != "-" || this.IsOSD == true) && (this.IsDayOff == false))
                    {
                        sAttStatus += "("+this.InTimeInString+"-"+this.OutTimeInString+"),";
                        if (this.LateArrivalMinute > 0)
                        {
                            sAttStatus += "Late,";
                        }
                        if (this.EarlyDepartureMinute > 0)
                        {
                            sAttStatus += "Early,";
                        }
                    }
                    if (this.IsDayOff == true)// && (this.OutTimeInString == "-" && this.OutTimeInString == "-"))
                    {
                        sAttStatus += "Off,";
                    }
                    if (this.IsHoliday == true)
                    {
                        sAttStatus += "HD,";
                    }
                    if (this.IsLeave == true)
                    {
                        //sAttStatus += this.LeaveName + ",";
                        sAttStatus += (this.LeaveHeadID > 0 ? this.LeaveStatus : "Leave") + ",";
                    }

                }
                if (sAttStatus != "")
                    sAttStatus = sAttStatus.Remove(sAttStatus.Length - 1, 1);
                return sAttStatus;
            }
        }

        public string AttStatusInStringInShort
        {
            get
            {
                string sAttStatus = "";
                if (this.InTimeInString == "-" && this.OutTimeInString == "-" && this.IsOSD == false && this.IsLeave == false && this.IsHoliday == false && this.IsDayOff == false)
                {
                    sAttStatus += "A,";
                }
                else
                {
                    if ((this.InTimeInString != "-" || this.OutTimeInString != "-" || this.IsOSD == true))
                    {
                        sAttStatus += "P,";
                        if (this.LateArrivalMinute > 0)
                        {
                            sAttStatus += "L,";
                        }
                        if (this.EarlyDepartureMinute > 0)
                        {
                            sAttStatus += "E,";
                        }
                    }
                    if (this.IsDayOff == true)// && (this.InTimeInString_12hr == "-" && this.InTimeInString_12hr == "-"))
                    {
                        sAttStatus += "Off,";
                    }
                    if (this.IsHoliday == true)
                    {
                        sAttStatus += "HD,";
                    }
                    if (this.IsLeave == true)
                    {
                        //sAttStatus += this.LeaveName + ",";
                        sAttStatus += (this.LeaveHeadID > 0 ? this.LeaveStatus : "Leave") + ",";
                    }

                }
                if (sAttStatus == "")
                {
                    sAttStatus += ",";
                }
                if (sAttStatus != "")
                    sAttStatus = sAttStatus.Remove(sAttStatus.Length - 1, 1);
                return sAttStatus;
            }
        }

        public string AttStatusInStringInShortComp
        {
            get
            {
                string sAttStatus = "";
                if (this.CompInTimeInString == "-" && this.CompOutTimeInString == "-" && this.IsOSD == false && this.IsCompLeave == false && this.IsCompHoliday == false && this.IsCompDayOff == false)
                {
                    sAttStatus += "A,";
                }
                else
                {
                    if ((this.CompInTimeInString != "-" || this.CompOutTimeInString != "-" || this.IsOSD == true))// && (this.IsCompDayOff == false))
                    {
                        sAttStatus += "P,";
                        if (this.CompLateArrivalMinute > 0)
                        {
                            sAttStatus += "L,";
                        }
                        if (this.CompEarlyDepartureMinute > 0)
                        {
                            sAttStatus += "E,";
                        }
                    }
                    if (this.IsCompDayOff == true)
                    {
                        sAttStatus += "Off,";
                    }
                    if (this.IsCompHoliday == true)
                    {
                        sAttStatus += "HD,";
                    }
                    if (this.IsCompLeave == true)
                    {
                        //sAttStatus += this.LeaveName + ",";
                        sAttStatus += (this.CompLeaveHeadID > 0 ? this.LeaveStatus : "Leave") + ",";
                    }

                }
                if (sAttStatus == "")
                {
                    sAttStatus += ",";
                }
                if (sAttStatus != "")
                    sAttStatus = sAttStatus.Remove(sAttStatus.Length - 1, 1);
                return sAttStatus;
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
        public string CompOTHrSt
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
        public string TotalWorkingHourSt
        {
            get
            {
                string S = "";
                if (this.TotalWorkingHourInMinute > 0)
                {
                    if (TotalWorkingHourInMinute / 60 >= 1) { S += ((TotalWorkingHourInMinute - TotalWorkingHourInMinute % 60) / 60).ToString() + "h "; }
                    if (TotalWorkingHourInMinute % 60 != 0) { S += (TotalWorkingHourInMinute % 60).ToString() + "m"; }
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

        public string ManualInString
        {
            get {
                if (this.IsManual == true) { return "Manual"; }
                else { return "-"; }
            }
        }

        public string AttStatusInST_24
        {
            get
            {
                string ST = "";
                if ((this.InTimeInString != "-" || this.OutTimeInString != "-" || this.IsOSD == true)  && !this.IsDayOff)
                {
                    ST += this.InTimeInString + " - " + this.OutTimeInString;
                    if (this.IsLeave) ST += "(" + this.LeaveStatus + ")";
                }
                else if (this.InTimeInString == "-" && this.OutTimeInString == "-" && this.IsOSD == false && !this.IsDayOff)
                { 
                    ST = "Absent"; 
                    if (this.IsLeave)
                    {
                        ST = "";
                        ST += this.LeaveStatus; 
                    } 
                }
                else if (this.IsDayOff) { ST = "DayOff"; }
                else { ST = "-"; }
                return ST;

            }
        }
        public string AttStatusInST
        {
            get
            {
                string ST = "";
                if ((this.InTimeInString != "-" || this.OutTimeInString != "-" || this.IsOSD == true) && !this.IsDayOff && !this.IsLeave)
                {
                    ST+=this.InTimeInString != "-" ? this.InTime.ToString("hh:mm tt") : this.InTimeInString;
                    ST+= " - ";
                    ST+=this.OutTimeInString != "-" ? this.OutTime.ToString("hh:mm tt") : this.OutTimeInString;
                }
                else if (this.InTimeInString == "-" && this.OutTimeInString == "-" && this.IsOSD == false && !this.IsDayOff && !this.IsLeave) { ST = "Absent"; }
                else if (this.IsDayOff) { ST= "DayOff"; }
                else if (this.IsLeave) { ST= "Leave"; }
                else { ST= "-"; }
                return ST;
                
            }
        }
        public bool IsManualOT { get; set; }
        public List<HRMShift> HRMShifts { get; set; }
        #endregion

        #region Functions


        public static List<AttendanceDaily> Gets(string sSQL, long nUserID)
        {
            return AttendanceDaily.Service.Gets(sSQL, nUserID);
        }
        public static DataSet GetsDataSet(string sSQL, long nUserID)
        {
            return AttendanceDaily.Service.GetsDataSet(sSQL, nUserID);
        }
        public static List<AttendanceDaily> GetsRecord(string sEmployeeIDs, string sBusinessUnitIds, string sLocationID, string sDepartmentIds, string sDesignationIds, string sSalarySchemeIDs, DateTime DateFrom, DateTime DateTo, string sWorkingStatus, string sGroupIDs, string sBlockIDs, double nStartSalaryRange, double nEndSalaryRange, string ShiftIds, long nUserID)
        {
            return AttendanceDaily.Service.GetsRecord(sEmployeeIDs, sBusinessUnitIds, sLocationID, sDepartmentIds, sDesignationIds, sSalarySchemeIDs, DateFrom, DateTo, sWorkingStatus, sGroupIDs, sBlockIDs, nStartSalaryRange, nEndSalaryRange, ShiftIds, nUserID);
        }
        public static List<AttendanceDaily> GetsContinuousAbsent(DateTime DateFrom, DateTime DateTo, string BUIDs, string LocIDs, string DepartmentIDs, string DesignationIDs,int DayCount, long nUserID)
        {
            return AttendanceDaily.Service.GetsContinuousAbsent(DateFrom, DateTo, BUIDs, LocIDs, DepartmentIDs, DesignationIDs, DayCount, nUserID);
        }
        public static AttendanceDaily Get(string sSQL, long nUserID)
        {
            return AttendanceDaily.Service.Get(sSQL, nUserID);
        }
        public AttendanceDaily Get(int nEmployeeID, DateTime dAttendanceDate, long nUserID)
        {
            return AttendanceDaily.Service.Get(nEmployeeID, dAttendanceDate, nUserID);
        }
        public static List<AttendanceDaily> GetsForReport(string sSQL, long nUserID)
        {
            return AttendanceDaily.Service.GetsForReport(sSQL, nUserID);
        }
        public AttendanceDaily IUD(int nDBOperation, long nUserID)
        {
            return AttendanceDaily.Service.IUD(this, nDBOperation, nUserID);
        }
        public static List<AttendanceDaily> NoWorkExecution(List<AttendanceDaily> oAttendanceDailys, long nUserID)
        {
            return AttendanceDaily.Service.NoWorkExecution(oAttendanceDailys, nUserID);
        }
        public static List<AttendanceDaily> CancelNoWorkExecution(List<AttendanceDaily> oAttendanceDailys, long nUserID)
        {
            return AttendanceDaily.Service.CancelNoWorkExecution(oAttendanceDailys, nUserID);
        }
        public AttendanceDaily ManualAttendance_Update(int nDBOperation, long nUserID)
        {
            return AttendanceDaily.Service.ManualAttendance_Update(this, nDBOperation, nUserID);
        }
        public static List<AttendanceDaily> UploadAttendanceXL(List<AttendanceDaily> oAttendanceDailys, long nUserID)
        {
            return AttendanceDaily.Service.UploadAttendanceXL(oAttendanceDailys, nUserID);
        }
        public static List<AttendanceDaily> GetsDayWiseAbsent(int nDays, DateTime dDate, long nUserID)
        {
            return AttendanceDaily.Service.GetsDayWiseAbsent(nDays, dDate, nUserID);
        }

        public static List<AttendanceDaily> EmployeeWiseReprocess(int EmployeeID, DateTime Startdate, DateTime EndDate, long nUserID)
        {
            return AttendanceDaily.Service.EmployeeWiseReprocess(EmployeeID, Startdate, EndDate, nUserID);
        }
        public AttendanceDaily AttendanceDaily_Manual_Single(long nUserID)
        {
            return AttendanceDaily.Service.AttendanceDaily_Manual_Single(this, nUserID);
        }
        public AttendanceDaily Update_AttendanceDaily_Manual_Single(DateTime dtStartDate, DateTime dtEndDate, int nEmployeeID, int nBufferTime, bool bIsOverTime, long nUserID)
        {
            return AttendanceDaily.Service.Update_AttendanceDaily_Manual_Single(dtStartDate, dtEndDate, nEmployeeID, nBufferTime, bIsOverTime, nUserID);
        }
        public List<AttendanceDaily> Update_AttendanceDaily_Manual_All(long nUserID)
        {
            return AttendanceDaily.Service.Update_AttendanceDaily_Manual_All(this, nUserID);
        }
        public AttendanceDaily AttendanceDaily_Manual_Single_Comp(long nUserID)
        {
            return AttendanceDaily.Service.AttendanceDaily_Manual_Single_Comp(this, nUserID);
        }
        public AttendanceDaily AttendanceDaily_Manual_Single_Comp_Conf(long nUserID)
        {
            return AttendanceDaily.Service.AttendanceDaily_Manual_Single_Comp_Conf(this, nUserID);
        }
        public AttendanceDaily AttendanceDaily_Manual_Single_Comp_ForDailyAttendance(long nUserID)
        {
            return AttendanceDaily.Service.AttendanceDaily_Manual_Single_Comp_ForDailyAttendance(this, nUserID);
        }
        public static List<AttendanceDaily> LeaveReportGets(string sSQL, long nUserID)
        {
            return AttendanceDaily.Service.LeaveReportGets(sSQL, nUserID);
        }
        public static List<AttendanceDaily> MakeAbsent(string sAttendanceDate, bool bOperation, bool bIsLeaveBefore, bool bIsLeaveAfter, bool bIsAbsentBefore, bool bIsAbsentAfter, bool bIsHolidayBefore, bool bIsHolidayAfter, bool bIsDayOffBefore, bool bIsDayOffAfter, string BUIDs, string LocIDs, string DepartmentIDs, string DesignationIDs, bool IsComp, long nUserID)
        {
            return AttendanceDaily.Service.MakeAbsent(sAttendanceDate, bOperation, bIsLeaveBefore, bIsLeaveAfter, bIsAbsentBefore, bIsAbsentAfter, bIsHolidayBefore, bIsHolidayAfter, bIsDayOffBefore, bIsDayOffAfter, BUIDs, LocIDs, DepartmentIDs, DesignationIDs, IsComp, nUserID);
        }
        public static List<AttendanceDaily> DayoffListExcel(string sAttendanceDate, bool bIsDayoffThisDay, string BUIDs, string LocIDs, string DepartmentIDs, string DesignationIDs, bool IsComp, int nType, long nUserID)
        {
            return AttendanceDaily.Service.DayoffListExcel(sAttendanceDate, bIsDayoffThisDay, BUIDs, LocIDs, DepartmentIDs, DesignationIDs, IsComp, nType, nUserID);
        }
        public static List<AttendanceDaily> MakeLeave(string sAttendanceDate, bool bIsDayoffThisDay, string BUIDs, string LocIDs, string DepartmentIDs, string DesignationIDs, bool IsComp, int nType, int nLeaveHeadID, DateTime sStartDate, DateTime sEndDate, long nUserID)
        {
            return AttendanceDaily.Service.MakeLeave(sAttendanceDate, bIsDayoffThisDay, BUIDs, LocIDs, DepartmentIDs, DesignationIDs, IsComp, nType, nLeaveHeadID, sStartDate, sEndDate, nUserID);
        }
        public static List<AttendanceDaily> UploadAttXL(List<AttendanceDaily> oAttendanceDailys, bool IsNUInTime, bool IsNUOutTime, bool IsNULate, bool IsNUEarly, bool IsNUInDate, bool IsNUOutDate, bool IsNUOT, bool IsNURemark, bool IsComp, long nUserID)
        {
            return AttendanceDaily.Service.UploadAttXL(oAttendanceDailys, IsNUInTime, IsNUOutTime, IsNULate, IsNUEarly, IsNUInDate, IsNUOutDate, IsNUOT, IsNURemark, IsComp, nUserID);
        }
        public static AttendanceDaily GetTotalCount(string ssql, long nUserID)
        {
            return AttendanceDaily.Service.GetTotalCount(ssql, nUserID);
        }

        public static List<AttendanceDaily> Gets(string sEmployeeIDs, string sBusinessUnitIds, string sLocationID, string sDepartmentIds, string sDesignationIds, string sSalarySchemeIDs, DateTime DateFrom, DateTime DateTo, string sWorkingStatus, string sGroupIDs, string sBlockIDs, double nStartSalaryRange, double nEndSalaryRange, string ShiftIds, string sRemarks, long nUserID)
        {
            return AttendanceDaily.Service.Gets(sEmployeeIDs, sBusinessUnitIds, sLocationID, sDepartmentIds, sDesignationIds, sSalarySchemeIDs, DateFrom, DateTo, sWorkingStatus, sGroupIDs, sBlockIDs, nStartSalaryRange, nEndSalaryRange, ShiftIds, sRemarks, nUserID);
        }
        
 
        #endregion

        #region ServiceFactory

        internal static IAttendanceDailyService Service
        {
            get { return (IAttendanceDailyService)Services.Factory.CreateService(typeof(IAttendanceDailyService)); }
        }
        #endregion
    }
    #endregion

    #region IAttendanceDaily interface

    public interface IAttendanceDailyService
    {

        List<AttendanceDaily> Gets(string sEmployeeIDs, string sBusinessUnitIds, string sLocationID, string sDepartmentIds, string sDesignationIds, string sSalarySchemeIDs, DateTime DateFrom, DateTime DateTo, string sWorkingStatus, string sGroupIDs, string sBlockIDs, double nStartSalaryRange, double nEndSalaryRange, string ShiftIds, string sRemarks, Int64 nUserID);
        AttendanceDaily GetTotalCount(string ssql, Int64 nUserID);
        List<AttendanceDaily> GetsContinuousAbsent(DateTime DateFrom, DateTime DateTo, string BUIDs, string LocIDs, string DepartmentIDs, string DesignationIDs, int DayCount, long nUserID);
        List<AttendanceDaily> GetsRecord(string sEmployeeIDs, string sBusinessUnitIds, string sLocationID, string sDepartmentIds, string sDesignationIds, string sSalarySchemeIDs, DateTime DateFrom, DateTime DateTo, string sWorkingStatus, string sGroupIDs, string sBlockIDs, double nStartSalaryRange, double nEndSalaryRange, string ShiftIds, long nUserID);
        AttendanceDaily Get(int nEmployeeID, DateTime dAttendanceDate, Int64 nUserID);
        List<AttendanceDaily> Gets(string sSQL, Int64 nUserID);
        DataSet GetsDataSet(string sSQL, Int64 nUserID);

        List<AttendanceDaily> MakeAbsent(string sAttendanceDate, bool bOperation, bool bIsLeaveBefore, bool bIsLeaveAfter, bool bIsAbsentBefore, bool bIsAbsentAfter, bool bIsHolidayBefore, bool bIsHolidayAfter, bool bIsDayOffBefore, bool bIsDayOffAfter, string BUIDs, string LocIDs, string DepartmentIDs, string DesignationIDs, bool IsComp, long nUserID);
        List<AttendanceDaily> DayoffListExcel(string sAttendanceDate, bool bIsDayoffThisDay, string BUIDs, string LocIDs, string DepartmentIDs, string DesignationIDs, bool IsComp, int nType, long nUserID);
        List<AttendanceDaily> MakeLeave(string sAttendanceDate, bool bIsDayoffThisDay, string BUIDs, string LocIDs, string DepartmentIDs, string DesignationIDs, bool IsComp, int nType, int nLeaveHeadID, DateTime sStartDate, DateTime sEndDate, long nUserID);
        
        AttendanceDaily Get(string sSQL, Int64 nUserID);
        List<AttendanceDaily> GetsForReport(string sSQL, Int64 nUserID);
        AttendanceDaily IUD(AttendanceDaily oAttendanceDaily, int nDBOperation, Int64 nUserID);
        List<AttendanceDaily> NoWorkExecution(List<AttendanceDaily> oAttendanceDailys, Int64 nUserID);
        List<AttendanceDaily> CancelNoWorkExecution(List<AttendanceDaily> oAttendanceDailys, Int64 nUserID);
        AttendanceDaily ManualAttendance_Update(AttendanceDaily oAttendanceDaily, int nDBOperation, Int64 nUserID);
        List<AttendanceDaily> UploadAttendanceXL(List<AttendanceDaily> oAttendanceDailys, Int64 nUserID);
        List<AttendanceDaily> GetsDayWiseAbsent(int nDays, DateTime dDate, Int64 nUserID);
        List<AttendanceDaily> EmployeeWiseReprocess(int EmployeeID, DateTime Startdate, DateTime EndDate, Int64 nUserID);
        AttendanceDaily AttendanceDaily_Manual_Single(AttendanceDaily oAttendanceDaily, Int64 nUserID);
        AttendanceDaily Update_AttendanceDaily_Manual_Single(DateTime dtStartDate, DateTime dtEndDate, int nEmployeeID, int nBufferTime, bool bIsOverTime, Int64 nUserID);
        List<AttendanceDaily> Update_AttendanceDaily_Manual_All(AttendanceDaily oAttendanceDaily, Int64 nUserID);
        AttendanceDaily AttendanceDaily_Manual_Single_Comp(AttendanceDaily oAttendanceDaily, Int64 nUserID);
        AttendanceDaily AttendanceDaily_Manual_Single_Comp_Conf(AttendanceDaily oAttendanceDaily, Int64 nUserID);
        AttendanceDaily AttendanceDaily_Manual_Single_Comp_ForDailyAttendance(AttendanceDaily oAttendanceDaily, Int64 nUserID);
        List<AttendanceDaily> LeaveReportGets(string sSQL, Int64 nUserID);
        List<AttendanceDaily> UploadAttXL(List<AttendanceDaily> oAttendanceDailys, bool IsNUInTime, bool IsNUOutTime, bool IsNULate, bool IsNUEarly, bool IsNUInDate, bool IsNUOutDate, bool IsNUOT, bool IsNURemark, bool IsComp, long nUserID);
      
    }
    #endregion
}
