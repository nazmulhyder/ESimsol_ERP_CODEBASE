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
    #region EmployeeOTonAttendance

    public class EmployeeOTonAttendance : BusinessObject
    {

        #region  Constructor
        public EmployeeOTonAttendance()
        {
            EmployeeID = 0;
            OTMinute = 0;
            ErrorMessage = "";
        }
        #endregion

        #region Properties

        public int EmployeeID { get; set; }
        public int OTMinute { get; set; }
        public string ErrorMessage { get; set; }
        public double OTHour
        {
            get
            {
                return (this.OTMinute <= 0) ? 0 : this.OTMinute / 60;
            }
        }

        #endregion


        #region Functions

        public static List<EmployeeOTonAttendance> Gets(bool IsCompliance, string employeeIDs, DateTime dtFrom, DateTime dtTo, long nUserID)
        {
            return EmployeeOTonAttendance.Service.Gets(IsCompliance, employeeIDs, dtFrom, dtTo, nUserID);
        }

        #endregion

        #region ServiceFactory
        internal static IEmployeeOTonAttendanceService Service
        {
            get { return (IEmployeeOTonAttendanceService)Services.Factory.CreateService(typeof(IEmployeeOTonAttendanceService)); }
        }
        #endregion
    }
    #endregion



    #region IEmployeeOTonAttendance interface
    public interface IEmployeeOTonAttendanceService
    {
        List<EmployeeOTonAttendance> Gets(bool IsCompliance, string employeeIDs, DateTime dtFrom, DateTime dtTo, long nUserID);
    }
    #endregion
}
