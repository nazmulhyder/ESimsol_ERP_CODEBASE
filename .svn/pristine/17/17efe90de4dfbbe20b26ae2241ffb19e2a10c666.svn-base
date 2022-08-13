using System;
using ICS.Core.Framework;
using ICS.Core.Utility;
using ICS.Core;
using ESimSol.BusinessObjects;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization;
using System.ServiceModel;
using ESimSol.BusinessObjects.ReportingObject;

namespace ESimSol.BusinessObjects
{
    #region AttendenceRegister
    public class AttendenceRegister : BusinessObject
    {
        public AttendenceRegister()
        {
            EmployeeID = 0;
            EmployeeName = "";
            Code = "";
            BusinessUnitID = 0;
            LocationID = 0;
            DepartmentID = 0;

            BUName = "";
            DesignationID = 0;
            LocationName = "";
            Department = "";
            Designation = "";
            Gender = "";
            ShiftID = 0;
            Fullfiled = 0;

            LessThan11Hrs = 0;
            LessThan9Hrs = 0;
            LessThan6Hrs = 0;
            TotalAbsent = 0;
            TotalLeave = 0;
            DutyHour = 0;
            EmployeeTypeName = "";

            ErrorMessage = "";

            MonthID = 0;
            YearID = 0;
            LateAttendanceCount = 0;
            EarlyLeaveCount = 0;
            TotalWorkingDays = 0;
        }

        #region Property
        public int TotalWorkingDays { get; set; }
        public int MonthID { get; set; }
        public int YearID { get; set; }
        public int LateAttendanceCount { get; set; }
        public int EarlyLeaveCount { get; set; }
        public int EmployeeID { get; set; }
        public string EmployeeName { get; set; }
        public string Code { get; set; }
        public int BusinessUnitID { get; set; }
        public int LocationID { get; set; }
        public int DepartmentID { get; set; }
        public string EmployeeTypeName { get; set; }
        public string BUName { get; set; }
        public int DesignationID { get; set; }
        public string LocationName { get; set; }
        public string Department { get; set; }
        public string Designation { get; set; }
        public string Gender { get; set; }
        public int ShiftID { get; set; }
        public int Fullfiled { get; set; }
        public int LessThan11Hrs { get; set; }

        public int LessThan9Hrs { get; set; }
        public int LessThan6Hrs { get; set; }
        public int TotalAbsent { get; set; }

        public int TotalLeave { get; set; }
        public int TotalDayOff { get; set; }
        public double DutyHour { get; set; }

        public string ErrorMessage { get; set; }
        public EnumReportLayout ReportLayout { get; set; }
        #endregion

        #region Derived Property
        
        //public string ApproveDateInString
        //{
        //    get
        //    {
        //        if (ApproveDate == DateTime.MinValue) return "";
        //        return ApproveDate.ToString("dd MMM yyyy");
        //    }
        //}
        //public string EmployeeTypeInString
        //{
        //    get
        //    {
        //        return EnumObject.jGet(this.EmployeeType);
        //    }
        //}
        #endregion

        #region Functions
        public static List<AttendenceRegister> Gets(int nAttendenceID, long nUserID)
        {
            return AttendenceRegister.Service.Gets(nAttendenceID, nUserID);
        }
        public static List<AttendenceRegister> Gets(string sSQL, long nUserID)
        {
            return AttendenceRegister.Service.Gets(sSQL, nUserID);
        }
        public AttendenceRegister Get(int id, long nUserID)
        {
            return AttendenceRegister.Service.Get(id, nUserID);
        }
        public static List<AttendenceRegister> Gets(string sEmployeeIDs, string sBusinessUnitIds, string sLocationID, string sDepartmentIds, string sDesignationIds, string sSalarySchemeIDs, DateTime DateFrom, DateTime DateTo, string sWorkingStatus, string sGroupIDs, string sBlockIDs, double nStartSalaryRange, double nEndSalaryRange, string ShiftIds, string sRemarks, long nUserID)
        {
            return AttendenceRegister.Service.Gets(sEmployeeIDs, sBusinessUnitIds, sLocationID, sDepartmentIds, sDesignationIds, sSalarySchemeIDs, DateFrom, DateTo, sWorkingStatus, sGroupIDs, sBlockIDs, nStartSalaryRange, nEndSalaryRange, ShiftIds, sRemarks, nUserID);
        }
        public static List<AttendenceRegister> GetsLateEarly(string sEmployeeIDs, string sBusinessUnitIds, string sLocationID, string sDepartmentIds, string sDesignationIds, string sSalarySchemeIDs, DateTime DateFrom, DateTime DateTo, string sWorkingStatus, string sGroupIDs, string sBlockIDs, double nStartSalaryRange, double nEndSalaryRange, string ShiftIds, string sRemarks, bool bIsMultipleMonth, string sMonthIDs, string sYearIDs, long nUserID)
        {
            return AttendenceRegister.Service.GetsLateEarly(sEmployeeIDs, sBusinessUnitIds, sLocationID, sDepartmentIds, sDesignationIds, sSalarySchemeIDs, DateFrom, DateTo, sWorkingStatus, sGroupIDs, sBlockIDs, nStartSalaryRange, nEndSalaryRange, ShiftIds, sRemarks, bIsMultipleMonth, sMonthIDs, sYearIDs, nUserID);
        }
        #endregion

        #region ServiceFactory
        internal static IAttendenceRegisterService Service
        {
            get { return (IAttendenceRegisterService)Services.Factory.CreateService(typeof(IAttendenceRegisterService)); }
        }
        #endregion

        public List<AttendenceRegister> AttendenceRegisters { get; set; }
    }
    #endregion

    #region IAttendenceRegister interface
    public interface IAttendenceRegisterService
    {
        AttendenceRegister Get(int id, Int64 nUserID);
        List<AttendenceRegister> Gets(int nAttendenceID, Int64 nUserID);
        List<AttendenceRegister> Gets(string sSQL, Int64 nUserID);
        List<AttendenceRegister> Gets(string sEmployeeIDs, string sBusinessUnitIds, string sLocationID, string sDepartmentIds, string sDesignationIds, string sSalarySchemeIDs, DateTime DateFrom, DateTime DateTo, string sWorkingStatus, string sGroupIDs, string sBlockIDs, double nStartSalaryRange, double nEndSalaryRange, string ShiftIds, string sRemarks, Int64 nUserID);
        List<AttendenceRegister> GetsLateEarly(string sEmployeeIDs, string sBusinessUnitIds, string sLocationID, string sDepartmentIds, string sDesignationIds, string sSalarySchemeIDs, DateTime DateFrom, DateTime DateTo, string sWorkingStatus, string sGroupIDs, string sBlockIDs, double nStartSalaryRange, double nEndSalaryRange, string ShiftIds, string sRemarks, bool bIsMultipleMonth, string sMonthIDs, string sYearIDs, Int64 nUserID);
    }
    #endregion
}
