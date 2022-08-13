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
    #region MonthlyAttendance_Leave
    [DataContract]
    public class MonthlyAttendance_Leave : BusinessObject
    {
        public MonthlyAttendance_Leave()
        {
            EmployeeID = 0;
            LeaveID = 0;
            LeaveName = "";
            Enjoyed = 0;
            ErrorMessage = "";
        }


        #region Properties

        public int EmployeeID { get; set; }
        public int LeaveID { get; set; }
        public string LeaveName { get; set; }
        public int Enjoyed { get; set; }
        public string ErrorMessage { get; set; }

        #endregion

        #region Derived Property

        #endregion

        #region Functions
        public static List<MonthlyAttendance_Leave> Gets(string sEmployeeIDs, string sBusinessUnitIds, string sLocationID, string sDepartmentIds, string sDesignationIds, string sSalarySchemeIDs, DateTime DateFrom, DateTime DateTo, string sWorkingStatus, long nUserID)
        {
            return MonthlyAttendance_Leave.Service.Gets(sEmployeeIDs, sBusinessUnitIds, sLocationID, sDepartmentIds, sDesignationIds, sSalarySchemeIDs, DateFrom, DateTo, sWorkingStatus, nUserID);
        }
        public static List<MonthlyAttendance_Leave> Gets_Comp(string sEmployeeIDs, string sBusinessUnitIds, string sLocationID, string sDepartmentIds, string sDesignationIds, string sSalarySchemeIDs, DateTime DateFrom, DateTime DateTo, string sWorkingStatus, long nUserID)
        {
            return MonthlyAttendance_Leave.Service.Gets_Comp(sEmployeeIDs, sBusinessUnitIds, sLocationID, sDepartmentIds, sDesignationIds, sSalarySchemeIDs, DateFrom, DateTo, sWorkingStatus, nUserID);
        }
        #endregion

        #region ServiceFactory
        internal static IMonthlyAttendance_LeaveService Service
        {
            get { return (IMonthlyAttendance_LeaveService)Services.Factory.CreateService(typeof(IMonthlyAttendance_LeaveService)); }
        }

        #endregion
    }

    #endregion

    #region IMonthlyAttendance_Leave interface
    [ServiceContract]
    public interface IMonthlyAttendance_LeaveService
    {
        List<MonthlyAttendance_Leave> Gets(string sEmployeeIDs, string sBusinessUnitIds, string sLocationID, string sDepartmentIds, string sDesignationIds, string sSalarySchemeIDs, DateTime DateFrom, DateTime DateTo, string sWorkingStatus, long nUserID);
        List<MonthlyAttendance_Leave> Gets_Comp(string sEmployeeIDs, string sBusinessUnitIds, string sLocationID, string sDepartmentIds, string sDesignationIds, string sSalarySchemeIDs, DateTime DateFrom, DateTime DateTo, string sWorkingStatus, long nUserID);
    }
    #endregion
}
