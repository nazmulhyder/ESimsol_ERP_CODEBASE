using System;
using System.IO;
using ICS.Core;
using ICS.Core.Framework;
using ICS.Core.Utility;

using ESimSol.BusinessObjects;
using System.Runtime.Serialization;
using System.Collections.Generic;

namespace ESimSol.BusinessObjects
{
    #region EmployeeLeaveInfo

    public class EmployeeLeaveInfo : BusinessObject
    {
        public EmployeeLeaveInfo()
        {
            EmployeeID = 0;
            LeaveHeadID = 0;
            LeaveCount = 0;
            EmployeeName = string.Empty;
            EmployeeCode = string.Empty;
            DepartmentName = string.Empty;
            DesignationName = string.Empty;
            ErrorMessage = string.Empty;
        }

        #region Properties

        public int EmployeeID { get; set; }
        public int LeaveHeadID { get; set; }
        public int LeaveCount { get; set; }
        public string EmployeeName { get; set; }
        public string EmployeeCode { get; set; }
        public string DepartmentName { get; set; }
        public string DesignationName { get; set; }

        public string ErrorMessage { get; set; }
        #endregion

        #region Functions

        public static List<EmployeeLeaveInfo> Gets(DateTime dtFrom, DateTime dtTo, int ACSID, int LeaveHeadId, string EmpIds, string DeptIds, string DesignationIds, bool bReportingPerson,  long nUserID)
        {
            return EmployeeLeaveInfo.Service.Gets(dtFrom, dtTo, ACSID, LeaveHeadId, EmpIds, DeptIds, DesignationIds,bReportingPerson, nUserID);
        }


        #endregion

        #region ServiceFactory
        internal static IEmployeeLeaveInfoService Service
        {
            get { return (IEmployeeLeaveInfoService)Services.Factory.CreateService(typeof(IEmployeeLeaveInfoService)); }
        }
        #endregion
    }
    #endregion

    #region IEmployeeLeaveInfo interface

    public interface IEmployeeLeaveInfoService
    {
        List<EmployeeLeaveInfo> Gets(DateTime dtFrom, DateTime dtTo, int ACSID, int LeaveHeadId, string EmpIds, string DeptIds, string DesignationIds, bool bReportingPerson, Int64 nUserID);
    }
    #endregion
}
