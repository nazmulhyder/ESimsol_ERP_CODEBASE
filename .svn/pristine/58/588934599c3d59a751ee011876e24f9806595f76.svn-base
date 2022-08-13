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
    #region AttendanceRatioReport
    [DataContract]
    public class AttendanceRatioReport 
    {
        public AttendanceRatioReport()
        {
            DepartmentID = 0;
            DepartmentName = "";
            ShiftID = 0;
            ShiftName = "";
            EmpTotal = 0;
            TotalPresent = 0;
            OTPresent = 0;
            AbsentLeave = 0;
            New = 0;
            Lefty = 0;
            Permanent = 0;
            Probationary = 0;
            Contractual = 0;
            ErrorMessage = "";
            LocationName = "";
            Designation = "";
            Gender = "";
        }

        #region Properties
        public int DepartmentID { get; set; }
        public int Permanent { get; set; }
        public int Probationary { get; set; }
        public int Contractual { get; set; }
        public string LocationName { get; set; }
        public string Designation { get; set; }
        public string DepartmentName { get; set; }
        public string Gender { get; set; }
        public int ShiftID { get; set; }
        public string ShiftName { get; set; }
        public int EmpTotal { get; set; }
        public int TotalPresent { get; set; }
        public int OTPresent { get; set; }
        public int AbsentLeave { get; set; }
        public int New { get; set; }
        public int Lefty { get; set; }
        public string ErrorMessage { get; set; }

        #endregion

        #region Functions
        public static List<AttendanceRatioReport> Gets(DateTime dDate, string sBusinessUnitIds, string sLocationID, string sDepartmentIds, string sDesignationIds, string sEmployeeIDs, string sShiftIDs, long nUserID)
        {
            return AttendanceRatioReport.Service.Gets(dDate, sBusinessUnitIds, sLocationID, sDepartmentIds, sDesignationIds, sEmployeeIDs, sShiftIDs, nUserID);
        }
        public static List<AttendanceRatioReport> GetsComp(DateTime dDate, string sBusinessUnitIds, string sLocationID, string sDepartmentIds, string sDesignationIds, string sEmployeeIDs, string sShiftIDs, long nUserID)
        {
            return AttendanceRatioReport.Service.GetsComp(dDate, sBusinessUnitIds, sLocationID, sDepartmentIds, sDesignationIds, sEmployeeIDs, sShiftIDs, nUserID);
        }

        #endregion

        #region ServiceFactory
        internal static IAttendanceRatioReportService Service
        {
            get { return (IAttendanceRatioReportService)Services.Factory.CreateService(typeof(IAttendanceRatioReportService)); }
        }
        #endregion
    }
    #endregion

    #region IAttendanceRatioReport interface

    public interface IAttendanceRatioReportService
    {
        List<AttendanceRatioReport> Gets(DateTime dDate, string sBusinessUnitIds, string sLocationID, string sDepartmentIds, string sDesignationIds, string sEmployeeIDs, string sShiftIDs, Int64 nUserID);
        List<AttendanceRatioReport> GetsComp(DateTime dDate, string sBusinessUnitIds, string sLocationID, string sDepartmentIds, string sDesignationIds, string sEmployeeIDs, string sShiftIDs, Int64 nUserID);
    }
    #endregion
}
