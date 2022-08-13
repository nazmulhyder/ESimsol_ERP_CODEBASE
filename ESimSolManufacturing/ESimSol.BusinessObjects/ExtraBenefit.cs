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
    #region ExtraBenefit

    public class ExtraBenefit : BusinessObject
    {
        public ExtraBenefit()
        {
            AttendanceDate = DateTime.Now;
            BOAName = "";
            EmployeeID = 0;
            EmployeeCode = "";
            EmployeeName = "";
            BusinessUnitName = "";
            BusinessUnitAddress = "";
            LocationName = "";
            DepartmentName = "";
            DesignationName = "";
            JoiningDate = DateTime.Now;
            Salary = 0;
            InTime = DateTime.Now;
            OutTime = DateTime.Now;
            Percent = 0;
            PerDayAmount = 0;
            PayableAmount = 0;
            ErrorMessage = "";
            IsActive = false;
            List = new List<ExtraBenefit>();
            Days = 0;
            DaysCount = 0;
        }

        #region Properties
        public DateTime AttendanceDate { get; set; }
        public string BOAName { get; set; }
        public int Days { get; set; }
        public int EmployeeID { get; set; }
        public string EmployeeCode { get; set; }
        public string EmployeeName { get; set; }
        public string BusinessUnitName { get; set; }
        public string BusinessUnitAddress { get; set; }
        public string LocationName { get; set; }
        public string DepartmentName { get; set; }
        public string DesignationName { get; set; }
        public DateTime JoiningDate { get; set; }
        public double Salary { get; set; }
        public DateTime InTime { get; set; }
        public DateTime OutTime { get; set; }
        public double Percent { get; set; }
        public double PerDayAmount { get; set; }
        public double PayableAmount { get; set; }
        public string ErrorMessage { get; set; }
        public bool IsActive { get; set; }
        public int DaysCount { get; set; }
        public string AttendanceDateInString
        {
            get
            {
                return AttendanceDate.ToString("dd MMM yyyy");
            }
        }
        public string InTimeInString
        {
            get
            {
                if (InTime.ToString("HH:mm") != "00:00")
                    return InTime.ToString("HH:mm");
                else
                    return "-";
            }
        }
        public string OutTimeInString
        {
            get
            {
                if (OutTime.ToString("HH:mm") != "00:00")
                    return OutTime.ToString("HH:mm");
                else
                    return "-";
            }
        }
        public string JoiningDateInString
        {
            get
            {
                return JoiningDate.ToString("dd MMM yyyy");
            }
        }
        public string WorkingStatusInST { get { return (this.IsActive) ? "Continued" : "Discontinued"; } }
        List<ExtraBenefit> List { get; set; }
        #endregion

        #region Functions
        public static List<ExtraBenefit> Gets(DateTime dDateFrom, DateTime dDateTo, string BOAIDs, string sEmployeeIDs, string sLocationID, string sDepartmentIDs, string sBusinessUnitIDs, double nStartSalary, double nEndSalary, long nUserID)
        {
            return ExtraBenefit.Service.Gets(dDateFrom, dDateTo, BOAIDs, sEmployeeIDs, sLocationID, sDepartmentIDs, sBusinessUnitIDs, nStartSalary, nEndSalary, nUserID);
        }
        public static List<ExtraBenefit> Gets(string sSQL, bool bIsComp, long nUserID)
        {
            return ExtraBenefit.Service.Gets(sSQL, bIsComp, nUserID);
        }

        #endregion

        #region ServiceFactory
        internal static IExtraBenefitService Service
        {
            get { return (IExtraBenefitService)Services.Factory.CreateService(typeof(IExtraBenefitService)); }
        }

        #endregion
    }
    #endregion

    #region IExtraBenefit interface

    public interface IExtraBenefitService
    {
        List<ExtraBenefit> Gets(DateTime dDateFrom, DateTime dDateTo, string BOAIDs, string sEmployeeIDs, string sLocationID, string sDepartmentIDs, string sBusinessUnitIDs, double nStartSalary, double nEndSalary, Int64 nUserID);
        List<ExtraBenefit> Gets(string sSQL, bool bIsComp, Int64 nUserID);
    }

    #endregion
}
