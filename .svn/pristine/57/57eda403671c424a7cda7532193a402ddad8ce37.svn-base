using System;
using System.IO;
using ICS.Core;
using ICS.Core.Framework;
using ICS.Core.Utility;
using ESimSol.BusinessObjects;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Collections.Generic;

namespace ESimSol.BusinessObjects
{
    #region IncrementApprisal
    public class IncrementApprisal
    {
        public IncrementApprisal()
        {
            EmployeeID = 0;
            EmployeeCode = ""; 
            EmployeeName = "";
            BusinessUnitID = 0;
            BusinessUnitName = "";
            LocationID = 0;
			LocationName = "";
            DepartmentID = 0; 
            DepartmentName = ""; 
            DesignationName = "";
            BeforeIncrement = 0.0; 
			BeforeEffectDate = DateTime.Now;
            RecentIncrement = 0.0;
            RecentEffectDate = DateTime.Now;
            Education = "";
            TotalLate = 0; 
            TotalLeave = 0; 
            TotalAbsent = 0;
            Warning = 0;
            JoiningDate = DateTime.Now;
            AttendancePercent = 0.0;
            PresentSalary = 0.0;

            LastPromotionDate = DateTime.Now;
        }

        #region Properties
        public int EmployeeID { get; set; }
        public double AttendancePercent { get; set; }
        public double PresentSalary { get; set; }
        public DateTime JoiningDate { get; set; }
        public DateTime LastPromotionDate { get; set; }
        public string EmployeeCode { get; set; }
        public string EmployeeName { get; set; }
        public int BusinessUnitID { get; set; }
        public string BusinessUnitName { get; set; }
        public int LocationID { get; set; }
        public string LocationName { get; set; }
        public int DepartmentID { get; set; }
        public string DepartmentName { get; set; }
        public string DesignationName { get; set; }
        public double BeforeIncrement { get; set; }
        public DateTime BeforeEffectDate { get; set; }
        public double RecentIncrement { get; set; }
        public DateTime RecentEffectDate { get; set; }
        public string Education { get; set; }
        public int TotalLate { get; set; }
        public int TotalLeave { get; set; }
        public int TotalAbsent { get; set; }
        public int Warning { get; set; }
        public string JoiningDateInString
        {
            get
            {
                return JoiningDate.ToString("dd MMM yyyy");
            }
        }
        public string BeforeEffectDateInString
        {
            get
            {
                return (BeforeEffectDate == DateTime.MinValue) ? "-" : BeforeEffectDate.ToString("dd MMM yyyy");
            }
        }
        public string RecentEffectDateInString
        {
            get
            {
                return (RecentEffectDate == DateTime.MinValue) ? "-" : RecentEffectDate.ToString("dd MMM yyyy");
            }
        }
        public string LastPromotionDateInString
        {
            get
            {
                return (LastPromotionDate.ToString("dd MMM yyyy") == "01 Jan 1900") ? "-" : LastPromotionDate.ToString("dd MMM yyyy");
            }
        }
        
        #endregion

        public List<BusinessUnit> BusinessUnits { get; set; }
        public List<IncrementApprisal> IncrementApprisals { get; set; }
        public Company Company { get; set; }

        #region Functions


        public static List<IncrementApprisal> Gets(string sSQL, long nUserID)
        {
            return IncrementApprisal.Service.Gets(sSQL, nUserID);
        }
        public static IncrementApprisal Get(string sSQL, long nUserID)
        {
            return IncrementApprisal.Service.Get(sSQL, nUserID);
        }
        public static List<IncrementApprisal> Search(DateTime UpToDate, string EmpIDs, string BUIDs, string LocationIDs, string DeptIDs, string DesignationIDs, DateTime JoiningDate, bool IsMultipleMonth, string sMonths, string sYears, bool IsJoinDate, double minsalary, double maxsalary, string BlockIDs, string GroupIDs, long nUserID)
        {
            return IncrementApprisal.Service.Search(UpToDate, EmpIDs, BUIDs, LocationIDs, DeptIDs, DesignationIDs, JoiningDate, IsMultipleMonth, sMonths, sYears, IsJoinDate, minsalary, maxsalary, BlockIDs, GroupIDs, nUserID);
        }
        #endregion

        #region ServiceFactory

        internal static IIncrementApprisalService Service
        {
            get { return (IIncrementApprisalService)Services.Factory.CreateService(typeof(IIncrementApprisalService)); }
        }
        #endregion
    }
    #endregion

    #region IIncrementApprisal interface

    public interface IIncrementApprisalService
    {
        List<IncrementApprisal> Gets(string sSQL, Int64 nUserID);
        IncrementApprisal Get(string sSQL, Int64 nUserID);
        List<IncrementApprisal> Search(DateTime UpToDate, string EmpIDs, string BUIDs, string LocationIDs, string DeptIDs, string DesignationIDs, DateTime JoiningDate, bool IsMultipleMonth, string sMonths, string sYears, bool IsJoinDate, double minsalary, double maxsalary, string BlockIDs, string GroupIDs, Int64 nUserID);
    }
    #endregion
}


