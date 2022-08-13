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
    #region LeaveReport_XL

    public class LeaveReport_XL : BusinessObject
    {
        public LeaveReport_XL()
        {
            EmployeeCode = "";
            EmployeeName = "";
            DepartmentName = "";
            DesignationName = "";
            DateOfJoin = DateTime.Now;
            CL = 0;
            SL = 0;
            EL = 0;
            LWP = 0;
            shortLeave = 0;
            ErrorMessage = "";

        }

        #region Properties
        public string EmployeeCode { get; set; }
        public string EmployeeName { get; set; }
        public string DepartmentName { get; set; }
        public string DesignationName { get; set; }
        public DateTime DateOfJoin { get; set; }
        public double CL { get; set; }
        public double SL { get; set; }
        public double EL { get; set; }
        public double LWP { get; set; }
        public double shortLeave { get; set; }
        public string ErrorMessage { get; set; }
        public bool IsActive { get; set; }
        #endregion

        #region Derived Property
        public string DateOfJoinInString
        {
            get
            {
                return DateOfJoin.ToString("dd MMM yyyy");
            }
        }
        public string EmployeeWorkingStatus
        {
            get
            {
                return this.IsActive ? "Continued" : "Discontinued";
            }
        }
        #endregion

        #region Functions
        public static List<LeaveReport_XL> Gets(DateTime StartDate, DateTime EndDate, long nUserID)
        {
            return LeaveReport_XL.Service.Gets(StartDate, EndDate, nUserID);
        }

        #endregion

        #region ServiceFactory
        internal static ILeaveReport_XLService Service
        {
            get { return (ILeaveReport_XLService)Services.Factory.CreateService(typeof(ILeaveReport_XLService)); }
        }

        #endregion
    }
    #endregion

    #region ILeaveReport_XL interface

    public interface ILeaveReport_XLService
    {
        List<LeaveReport_XL> Gets(DateTime StartDate, DateTime EndDate, Int64 nUserID);
   

    }
    #endregion
}
