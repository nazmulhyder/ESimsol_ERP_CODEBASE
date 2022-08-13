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
    #region AttendanceAccessPointEmployee
    public class AttendanceAccessPointEmployee : BusinessObject
    {
        public AttendanceAccessPointEmployee()
        {
            AAPEmployeeID = 0;
            AAPID = 0;
            EmployeeID = 0;
            IsActive = false;
            InactiveDate = DateTime.MinValue;
            InactiveBy = 0;
            ErrorMessage = "";
        }

        #region Properties
        public int AAPEmployeeID { get; set; }
        public int AAPID { get; set; }
        public int EmployeeID { get; set; }
        public bool IsActive { get; set; }
        public DateTime InactiveDate { get; set; }
        public int InactiveBy { get; set; }
        public string ErrorMessage { get; set; }
        #endregion

        #region Derived Properties
        public AttendanceAccessPoint AAP { get; set; }

        public string EmployeeName { get; set; }
        public string EmployeeCode { get; set; }
        public string InactiveByName { get; set; }

        public string ActivityInStr { get { if (this.IsActive) return "Active"; else return "Inactive"; } }
        public string EmployeeNameCode
        {
            get
            {
               return this.EmployeeName + "[" + this.EmployeeCode +"]";
            }
        }
        public string InactiveDateInStr
        {
            get
            {
                return (this.InactiveBy!=0)?this.InactiveDate.ToString("dd MMM yyyy"): "";
            }
        }
        
        #endregion


        #region Functions
        public static AttendanceAccessPointEmployee Get(int nAAPEmployeeID, long nUserID)
        {
            return AttendanceAccessPointEmployee.Service.Get(nAAPEmployeeID, nUserID);
        }
        public static List<AttendanceAccessPointEmployee> Gets(string sSQL, long nUserID)
        {
            return AttendanceAccessPointEmployee.Service.Gets(sSQL, nUserID);
        }
        public AttendanceAccessPointEmployee IUD(int nDBOperation, long nUserID)
        {
            return AttendanceAccessPointEmployee.Service.IUD(this, nDBOperation, nUserID);
        }
        #endregion

        #region ServiceFactory
        internal static IAttendanceAccessPointEmployeeService Service
        {
            get { return (IAttendanceAccessPointEmployeeService)Services.Factory.CreateService(typeof(IAttendanceAccessPointEmployeeService)); }
        }
        #endregion
    }
    #endregion

    #region IAttendanceAccessPointEmployee interface

    public interface IAttendanceAccessPointEmployeeService
    {
        AttendanceAccessPointEmployee Get(int nAAPEmployeeID, Int64 nUserID);

        List<AttendanceAccessPointEmployee> Gets(string sSQL, Int64 nUserID);

        AttendanceAccessPointEmployee IUD(AttendanceAccessPointEmployee oAttendanceAccessPointEmployee, int nDBOperation, Int64 nUserID);
    }
    #endregion


}

