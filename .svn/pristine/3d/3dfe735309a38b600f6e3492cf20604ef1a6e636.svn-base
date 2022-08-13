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
    public class AttendanceSummery_List : BusinessObject
    {
        public AttendanceSummery_List()
        {
            EmployeeID = 0;
            EmployeeCode = "";
            EmployeeName = "";
            DepartmentName = "";
            DesignationName = "";
            JoiningDate = DateTime.Now;
            WorkingTill = "";
            TotalPresent = 0;
            TotalAbsent = 0;
            TotalLate = 0;
            TotalEarlyLeave = 0;
            TotalLeave = 0;
            LeaveBalance = "";
            Performance = "";
            ErrorMessage = "";
        }


        #region Properties

        public int EmployeeID { get; set; }
        public string EmployeeCode { get; set; }
        public string EmployeeName { get; set; }
        public string DepartmentName { get; set; }
        public string DesignationName { get; set; }
        public DateTime JoiningDate { get; set; }
        public string WorkingTill { get; set; }
        public int TotalPresent { get; set; }
        public int TotalAbsent { get; set; }
        public int TotalLate { get; set; }
        public int TotalEarlyLeave { get; set; }
        public int TotalLeave { get; set; }
        public string LeaveBalance { get; set; }
        public string Performance { get; set; }
        public string ErrorMessage { get; set; }

        #endregion

        #region Derived Property
        public string JoiningDateInString
        {
            get
            {
                return JoiningDate.ToString("dd MMM yyyy");
            }
        }
        public string TotalPresentInString
        {
            get
            {
                if (this.TotalPresent > 0)
                    return TotalPresent + " D";
                else
                    return "-";
            }
        }
        public string TotalAbsentInString
        {
            get
            {
                if (this.TotalAbsent > 0)
                    return TotalAbsent + " D";
                else return "-";
            }
        }
        public string TotalLateInString
        {
            get
            {
                if (this.TotalLate > 0)
                    return TotalLate + " D";
                else return "-";
            }
        }
        public string TotalEarlyLeaveInString
        {
            get
            {
                if (this.TotalEarlyLeave > 0)
                    return TotalEarlyLeave + " D";
                else return "-";
            }
        }
        public string TotalLeaveInString
        {
            get
            {
                if (this.TotalLeave > 0)
                    return TotalLeave + " D";
                else return "-";
            }
        }
        #endregion

        #region Functions

        public static List<AttendanceSummery_List> Gets(string sParams, long nUserID)
        {
            return AttendanceSummery_List.Service.Gets(sParams, nUserID);
        }
      
        #endregion

        #region ServiceFactory
        internal static IAttendanceSummery_ListService Service
        {
            get { return (IAttendanceSummery_ListService)Services.Factory.CreateService(typeof(IAttendanceSummery_ListService)); }
        }
      
        #endregion
    }
    #endregion

    #region IAttendanceSummery interface
    
    public interface IAttendanceSummery_ListService
    {
        List<AttendanceSummery_List> Gets(string Sparams, Int64 nUserID);

    }
    #endregion
}
