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
    public class AttendanceDailyV2
    {
        public AttendanceDailyV2()
        {
            MOCAID = 0;
            AttendanceID = 0;
            EmployeeID = 0;
            MOCID = 0;
            AttendanceDate = DateTime.Now;
            InTime = DateTime.Now;
            OutTime = DateTime.Now;
            OverTimeInMin = 0;
            IsDayOff = false;
            IsHoliday = false;
            IsNoLate = false;
            IsNoEarly = false;
            IsLeave = false;
            LeaveHeadID = 0;
            TotalWorkingHourInMinute = 0;
            BusinessUnitID = 0;
            LocationID = 0;
            DepartmentID = 0;
            DesignationID = 0;
            ShiftID = 0;
            IsManual = false;
            IsOSD = false;
            BUName = "";
            BUAddress = "";
            LocationName = "";
            DepartmentName = "";
            DesignationName = "";
            ShiftName = "";
            TimeCardName = "";
            LeaveName = "";
            EmployeeCode = "";
            EmployeeName = "";
            JoiningDate = DateTime.Now;
            LateArrivalMinute = 0;
            EarlyDepartureMinute = 0;
            IsUnPaid = false;
            ShiftStartTime = DateTime.Now;
            ShiftEndTime = DateTime.Now;
            Remark = "";
            IsAbsent = false;
            IsManualOT = false;
            ErrorMessage = "";
            ManualUpdateHistory = "";
            MOCAIDs = "";
            LeaveAssignDate = DateTime.Now;
            CompensatoryLeaveType = 0;
            TotalAttendanceCount = 0;
        }
        #region Properties
        public int MOCAID { get; set; }
        public int AttendanceID { get; set; }
        public int EmployeeID { get; set; }
        public int MOCID { get; set; }
        public DateTime AttendanceDate { get; set; }
        public DateTime InTime { get; set; }
        public DateTime OutTime { get; set; }
        public int OverTimeInMin { get; set; }
        public bool IsDayOff { get; set; }
        public bool IsManualOT { get; set; }
        public bool IsHoliday { get; set; }
        public bool IsLeave { get; set; }
        public bool IsNoLate { get; set; }
        public bool IsNoEarly { get; set; }
        public int LeaveHeadID { get; set; }
        public int TotalWorkingHourInMinute { get; set; }
        public int BusinessUnitID { get; set; }
        public int LocationID { get; set; }
        public int DepartmentID { get; set; }
        public int DesignationID { get; set; }
        public int ShiftID { get; set; }
        public bool IsManual { get; set; }
        public bool IsOSD { get; set; }
        public string BUName { get; set; }
        public string BUAddress { get; set; }
        public string ManualUpdateHistory { get; set; }
        public string LocationName { get; set; }
        public string DepartmentName { get; set; }
        public string TimeCardName { get; set; }
        public string DesignationName { get; set; }
        public string ShiftName { get; set; }
        public string LeaveName { get; set; }
        public string EmployeeCode { get; set; }
        public string EmployeeName { get; set; }
        public DateTime JoiningDate { get; set; }
        public int LateArrivalMinute { get; set; }
        public int EarlyDepartureMinute { get; set; }
        public bool IsUnPaid { get; set; }
        public DateTime ShiftStartTime { get; set; }
        public DateTime ShiftEndTime { get; set; }
        public string ErrorMessage { get; set; }
        public bool IsAbsent { get; set; }
        public string Remark { get; set; }
        public string MOCAIDs { get; set; }
        public DateTime LeaveAssignDate { get; set; }
        public int CompensatoryLeaveType { get; set; }
        public int TotalAttendanceCount { get; set; }
        #endregion

        #region Derived Properties

        public string ManualRecord
        {
            get
            {
                return this.IsManual == true ? "Manual" : "";
            }
        }
        public int ShiftDuration
        {
            get
            {
               if(this.IsDayOff==true||this.IsLeave==true || this.IsHoliday==true)
               {
                   return 0;
               }
                 return (int)this.ShiftEndTime.Subtract(this.ShiftStartTime).TotalMinutes;
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
        public string ShiftStartTimeInString
        {
            get
            {
                if (ShiftStartTime.ToString("HH:mm") != "00:00")
                    return ShiftStartTime.ToString("HH:mm");
                else
                    return "-";
            }
        }
        public string ShiftEndTimeInString
        {
            get
            {
                if (ShiftEndTime.ToString("HH:mm") != "00:00")
                    return ShiftEndTime.ToString("HH:mm");
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
       
        public string TotalWorkingHourSt
        {
            get
            {
                return Global.MinInHourMin(this.TotalWorkingHourInMinute);
            }
        }

        public string OverTimeInMinuteHourSt
        {
            get
            {
                string S = "";
                if (OverTimeInMin > 0)
                {
                    if (OverTimeInMin / 60 >= 1) { S += (OverTimeInMin / 60).ToString() + "h "; }
                    if (OverTimeInMin % 60 != 0) { S += (OverTimeInMin % 60).ToString() + "m"; }
                    return S;
                }
                else return "-";
            }
        }


        public string AttStatusInString
        {
            get
            {
                string sAttStatus = "";
                if (this.InTimeInString == "-" && this.IsLeave == false && this.IsHoliday == false && this.IsDayOff == false)
                {
                    sAttStatus += "A,";
                }
                else
                {
                    if (this.IsDayOff == true)
                    {
                        sAttStatus += "Off,";
                        if (this.InTimeInString != "-" || this.OutTimeInString != "-")
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
                        if (this.InTimeInString != "-" || this.OutTimeInString != "-")
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
                        if (this.InTimeInString != "-" || this.OutTimeInString != "-")
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
                        if (this.InTimeInString != "-" || this.OutTimeInString != "-")
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
                if (sAttStatus != "")
                    sAttStatus = sAttStatus.Remove(sAttStatus.Length - 1, 1);
                return sAttStatus;
            }
        }
        #endregion





        #region Function
        public static List<AttendanceDailyV2> CompGets(string sSQL, long nUserID)
        {
            return AttendanceDailyV2.Service.CompGets(sSQL, nUserID);
        }

         public static List<AttendanceDailyV2> Gets(string sSQL, long nUserID)
        {
            return AttendanceDailyV2.Service.Gets(sSQL, nUserID);
        }

         public AttendanceDailyV2 AttendanceDaily_Manual_Single(long nUserID)
         {
             return AttendanceDailyV2.Service.AttendanceDaily_Manual_Single(this, nUserID);
         }

         public AttendanceDailyV2 AttendanceDaily_Manual_Single_Comp(long nUserID)
         {
             return AttendanceDailyV2.Service.AttendanceDaily_Manual_Single_Comp(this, nUserID);
         }

         public string AssignLeave(long nUserID)
         {
             return AttendanceDailyV2.Service.AssignLeave(this, nUserID);
         }

         public static AttendanceDailyV2 GetTotalCount(string ssql, long nUserID)
         {
             return AttendanceDailyV2.Service.GetTotalCount(ssql, nUserID);
         }
        #endregion

        #region Non DB Function
        public static string GetAttendanceSummary(List<AttendanceDailyV2> oAttendanceDailyV2s, string sDaySign, bool bIsWithDays = true)
        {
            string sAttendanceSummary = "";
            if (sDaySign == "OT")
            {
                int nTotalOTInMin = 0;
                foreach (AttendanceDailyV2 oItem in oAttendanceDailyV2s)
                {
                    nTotalOTInMin = nTotalOTInMin + oItem.OverTimeInMin;
                }
                sAttendanceSummary = Global.MinInHourMin(nTotalOTInMin);
            }
            else if (sDaySign == "Late Hour")
            {
                int nTotalLateInMin = 0;
                foreach (AttendanceDailyV2 oItem in oAttendanceDailyV2s)
                {
                    nTotalLateInMin = nTotalLateInMin + oItem.LateArrivalMinute;
                }
                sAttendanceSummary = Global.MinInHourMin(nTotalLateInMin);
            }
            else if (sDaySign == "Early Out Days")
            {
                int nTotalEarlyOutDays = 0;
                foreach (AttendanceDailyV2 oItem in oAttendanceDailyV2s)
                {
                    if (oItem.EarlyDepartureMinute > 0)
                    {
                        nTotalEarlyOutDays++;
                    }
                }
                sAttendanceSummary = nTotalEarlyOutDays.ToString() + (bIsWithDays == true ? " DAYS" : "");
            }
            else if (sDaySign == "Early Out Mins")
            {
                int nTotalEarlyOutMins = 0;
                foreach (AttendanceDailyV2 oItem in oAttendanceDailyV2s)
                {
                    nTotalEarlyOutMins = nTotalEarlyOutMins + oItem.EarlyDepartureMinute;
                }
                sAttendanceSummary = Global.MinInHourMin(nTotalEarlyOutMins);
            }
            else if (sDaySign == "SD")
            {
                int nTotalShiftDurations = 0;
                foreach (AttendanceDailyV2 oItem in oAttendanceDailyV2s)
                {
                    nTotalShiftDurations = nTotalShiftDurations + oItem.ShiftDuration;
                }
                sAttendanceSummary = (bIsWithDays == true ? Global.MinInHourMin(nTotalShiftDurations) : nTotalShiftDurations.ToString());
            }

            else if (sDaySign == "Total Working Hour")
            {
                int nTotalWHour = 0;
                foreach (AttendanceDailyV2 oItem in oAttendanceDailyV2s)
                {
                    nTotalWHour = nTotalWHour + oItem.TotalWorkingHourInMinute;
                }
                sAttendanceSummary = (bIsWithDays == true ? Global.MinInHourMin(nTotalWHour) : nTotalWHour.ToString());
            }
            else
            {
                int nCountedDays = 0;
                foreach (AttendanceDailyV2 oItem in oAttendanceDailyV2s)
                {
                    if (sDaySign == "Leave")
                    {
                        if (oItem.IsLeave)
                        {
                            nCountedDays = nCountedDays + 1;
                        }
                    }
                    else
                    {
                        string[] aDaysSigns = oItem.AttStatusInString.Split(',');
                        if (aDaysSigns != null && aDaysSigns.Length > 0)
                        {
                            for (int i = 0; i < aDaysSigns.Length; i++)
                            {
                                if (aDaysSigns[i] == sDaySign)
                                {
                                    nCountedDays = nCountedDays + 1;
                                }
                            }
                        }
                    }
                }
                sAttendanceSummary = nCountedDays.ToString() + (bIsWithDays == true ? " DAYS" : "");
            }
            return sAttendanceSummary;
        }
        #endregion


        #region ServiceFactory

        internal static IAttendanceDailyV2Service Service
        {
            get { return (IAttendanceDailyV2Service)Services.Factory.CreateService(typeof(IAttendanceDailyV2Service)); }
        }
        #endregion
    }

    #region IAttendanceDailyV2interface


    public interface IAttendanceDailyV2Service
    {
        List<AttendanceDailyV2> CompGets(string sSQL, Int64 nUserID);
        List<AttendanceDailyV2> Gets(string sSQL, Int64 nUserID);
        AttendanceDailyV2 AttendanceDaily_Manual_Single(AttendanceDailyV2 oAttendanceDailyV2, Int64 nUserID);
        AttendanceDailyV2 AttendanceDaily_Manual_Single_Comp(AttendanceDailyV2 oAttendanceDailyV2, Int64 nUserID);
        string AssignLeave(AttendanceDailyV2 oAttendanceDailyV2, Int64 nUserID);
        AttendanceDailyV2 GetTotalCount(string ssql, Int64 nUserID);
    }
    #endregion




    #region HCM Search Obj
    public class HCMSearchObj
    {
        public HCMSearchObj()
        {
            BUIDs = "";
            EmployeeIDs = "";
            LocationIDs = "";
            DepartmentIDs = "";
            StartDate = DateTime.Now;
            EndDate = DateTime.Now;
            BlockIDs = "";
            GroupIDs = "";
            MOCID = 0;
            PrintFormatInt = 0;
            StartSalaryRange = 0;
            EndSalaryRange = 0;
            SalarySchemeIDs = "";
            AttendanceSchemeIDs = "";
            ShiftIDs = "";
            EmployeeTypeIDs = "";
            DesignationIDs = "";
            IsJoiningDate = false;
            JoiningStartDate = DateTime.Now;
            JoiningEndDate = DateTime.Now;
            AuthenticationNo = "";
            CategoryID = EnumEmployeeCategory.None;
            PaymentTypeID = 0;
            IsNewJoin = false;
            IsRound = false;
            IsOT = false;
            SalaryStartDate = DateTime.Now;
            SalaryEndDate = DateTime.Now;
            GroupBySerialID = 0;
            SalaryFieldSetupID = 0;
            LeaveHeadID = 0;
            CompensatoryLeaveType = 1;
            LoadRecords = 0;
            RowLength = 0;
            IsManual = false;
            PIMSRoaster = 0;
            SelectedColNames = "";
            IsSearchByBank = false;
            PrintHeaderType = "";
            HeaderHeightInch = 0;
            FooterHeightInch = 0;
            BankAccountID = 0;
            LetterInformationDate = DateTime.Now;
        }

        #region Properties
        public string BUIDs { get; set; }
        public string EmployeeIDs { get; set; }
        public string LocationIDs { get; set; }
        public string DepartmentIDs { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public DateTime SalaryStartDate { get; set; }
        public DateTime SalaryEndDate { get; set; }
        public string BlockIDs { get; set; }
        public string GroupIDs { get; set; }
        public int MOCID { get; set; }
        public int PrintFormatInt { get; set; }
        public EnumTimeCardFormat PrintFormat { get; set; }
        public double StartSalaryRange { get; set; }
        public double EndSalaryRange { get; set; }
        public string SalarySchemeIDs { get; set; }
        public string AttendanceSchemeIDs { get; set; }
        public string ShiftIDs { get; set; }
        public string EmployeeTypeIDs { get; set; }
        public string DesignationIDs { get; set; }
        public bool IsJoiningDate { get; set; }
        public DateTime JoiningStartDate { get; set; }
        public DateTime JoiningEndDate { get; set; }
        public string AuthenticationNo { get; set; }
        public EnumEmployeeCategory CategoryID { get; set; }
        public int PaymentTypeID { get; set; }
        public bool IsNewJoin { get; set; }
        public bool IsRound { get; set; }
        public bool IsOT { get; set; }
        public int GroupBySerialID { get; set; }
        public int SalaryFieldSetupID { get; set; }
        public int LeaveHeadID { get; set; }
        public int CompensatoryLeaveType { get; set; }
        public int LoadRecords { get; set; }
        public int RowLength { get; set; }
        public bool IsManual { get; set; }
        public int PIMSRoaster { get; set; }
        public string SelectedColNames { get; set; }
        public bool IsSearchByBank { get; set; }
        public string PrintHeaderType { get; set; }
        public float HeaderHeightInch { get; set; }
        public float FooterHeightInch { get; set; }
        public int BankAccountID { get; set; }
        public DateTime LetterInformationDate { get; set; }
        #endregion

    }
    #endregion
}