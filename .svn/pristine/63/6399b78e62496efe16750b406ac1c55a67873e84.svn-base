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
    #region AttendanceWithExtraSearching
    [DataContract]
    public class AttendanceWithExtraSearching : BusinessObject
    {
        public AttendanceWithExtraSearching()
        {
            EmployeeID =0;
            EmployeeName="";
            EmployeeCode="";
            BusinessUnitName ="";
            LocationName ="";
	        DepartmentName=""; 
            DesignationName =""; 
            JoiningDate = DateTime.Now;
            PresentGross =0;
            AttDates ="";
            TotalDays=0;
            ErrorMessage = "";
        }

        #region Properties
        public int EmployeeID { get; set; }
        public string EmployeeName{ get; set; }
        public string EmployeeCode{ get; set; }
        public string  BusinessUnitName { get; set; }
        public string LocationName { get; set; }
	    public string DepartmentName{ get; set; } 
        public string  DesignationName { get; set; } 
        public DateTime JoiningDate { get; set; }
        public double PresentGross { get; set; }
        public string AttDates { get; set; }
        public int TotalDays{ get; set; }
        public string ErrorMessage { get; set; }

        #endregion

        #region Derived Property
        public string JoiningDateInString { get{return this.JoiningDate.ToString("dd MMM yyyy");}}
        #endregion

        #region Functions
        public static List<AttendanceWithExtraSearching> Gets(string sParams, long nUserID)
        {
            return AttendanceWithExtraSearching.Service.Gets(sParams, nUserID);
        }
        public static List<AttendanceWithExtraSearching> GetsComp(string sParams, long nUserID)
        {
            return AttendanceWithExtraSearching.Service.GetsComp(sParams, nUserID);
        }
        #endregion

        #region ServiceFactory
        internal static IAttendanceWithExtraSearchingService Service
        {
            get { return (IAttendanceWithExtraSearchingService)Services.Factory.CreateService(typeof(IAttendanceWithExtraSearchingService)); }
        }

        #endregion
    }

    #endregion

    #region IAttendanceWithExtraSearching interface
    [ServiceContract]
    public interface IAttendanceWithExtraSearchingService
    {
        List<AttendanceWithExtraSearching> Gets(string sParams, long nUserID);
        List<AttendanceWithExtraSearching> GetsComp(string sParams, long nUserID);
    }
    #endregion
}
