using System;
using System.IO;
using ESimSol.BusinessObjects;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Collections.Generic;
using ICS.Core;
using ICS.Core.Framework;
using ICS.Core.Utility;

namespace ESimSol.BusinessObjects
{
    #region EmployeeLeaveOnAttendance

    public class EmployeeLeaveOnAttendance : BusinessObject
    {

        #region  Constructor
        public EmployeeLeaveOnAttendance()
        {
            EmployeeID = 0;
            LeaveDays = 0;
            LeaveHeadID = 0;
            CompLeaveHeadID = 0;
            LeaveHeadName = string.Empty;
            Params = string.Empty;
        }
        #endregion

        #region Properties

        public int EmployeeID { get; set; }
        public int LeaveDays { get; set; }
        public int LeaveHeadID { get; set; }
        public int CompLeaveHeadID { get; set; }
        public string LeaveHeadName { get; set; }
        public string ErrorMessage { get; set; }
        public string Params { get; set; }

        #endregion


        #region Functions

        public static List<EmployeeLeaveOnAttendance> Gets(string employeeIDs, DateTime dtFrom, DateTime dtTo, long nUserID)
        {
            return EmployeeLeaveOnAttendance.Service.Gets(employeeIDs, dtFrom, dtTo, nUserID);
        }
        public static List<EmployeeLeaveOnAttendance> GetsComp(string employeeIDs, DateTime dtFrom, DateTime dtTo, long nUserID)
        {
            return EmployeeLeaveOnAttendance.Service.GetsComp(employeeIDs, dtFrom, dtTo, nUserID);
        }
        public static List<EmployeeLeaveOnAttendance> GetsActulaForComp(string employeeIDs, DateTime dtFrom, DateTime dtTo, long nUserID)
        {
            return EmployeeLeaveOnAttendance.Service.GetsActulaForComp(employeeIDs, dtFrom, dtTo, nUserID);
        }
        #endregion

        #region ServiceFactory
        internal static IEmployeeLeaveOnAttendanceService Service
        {
            get { return (IEmployeeLeaveOnAttendanceService)Services.Factory.CreateService(typeof(IEmployeeLeaveOnAttendanceService)); }
        }
        #endregion
    }
    #endregion



    #region IEmployeeLeaveOnAttendance interface
    public interface IEmployeeLeaveOnAttendanceService
    {
        List<EmployeeLeaveOnAttendance> Gets(string employeeIDs, DateTime dtFrom, DateTime dtTo, long nUserID);
        List<EmployeeLeaveOnAttendance> GetsComp(string employeeIDs, DateTime dtFrom, DateTime dtTo, long nUserID);
        List<EmployeeLeaveOnAttendance> GetsActulaForComp(string employeeIDs, DateTime dtFrom, DateTime dtTo, long nUserID);
    }
    #endregion
}