using System;
using System.IO;
using ICS.Core;
using ICS.Core.Framework;
using ICS.Core.Utility;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Collections.Generic;
using System.Linq;

namespace ESimSol.BusinessObjects
{
    public class LeaveStatus : BusinessObject
    {
        public LeaveStatus()
        {
            EmployeeID = 0;
            LeaveHeadID = 0;
            LeaveDays = 0;
            LeaveHeadName = "";
            LeaveHeadShortName = "";
        }

        #region Properties
        public int EmployeeID { get; set; }
        public int LeaveHeadID { get; set; }
        public int LeaveDays { get; set; }
        public string LeaveHeadName { get; set; }
        public string LeaveHeadShortName { get; set; }
        #endregion

        #region Functions
        public static List<LeaveStatus> Gets(string sSQL,DateTime SalaryStartDate,DateTime SalaryEndDate, long nUserID)
        {
            return LeaveStatus.Service.Gets(sSQL,SalaryStartDate,SalaryEndDate, nUserID);
        }
        public static List<LeaveStatus> CompGets(string sSQL, int nMOCID, DateTime SalaryStartDate, DateTime SalaryEndDate, long nUserID)
        {
            return LeaveStatus.Service.CompGets(sSQL, nMOCID, SalaryStartDate, SalaryEndDate, nUserID);
        }
        #endregion
        #region ServiceFactory

        internal static ILeaveStatusService Service
        {
            get { return (ILeaveStatusService)Services.Factory.CreateService(typeof(ILeaveStatusService)); }
        }
        #endregion
    }

    #region ILeaveStatus interface


    public interface ILeaveStatusService
    {
        List<LeaveStatus> Gets(string sSQL, DateTime SalaryStartDate, DateTime SalaryEndDate, long nUserID);
        List<LeaveStatus> CompGets(string sSQL, int nMOCID, DateTime SalaryStartDate, DateTime SalaryEndDate, long nUserID);
    }
    #endregion
}
