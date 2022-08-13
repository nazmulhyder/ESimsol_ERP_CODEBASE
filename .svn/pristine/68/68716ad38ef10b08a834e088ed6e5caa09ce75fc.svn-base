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
    #region EmployeeSettlementSalary
    public class EmployeeSettlementSalary
    {
        public EmployeeSettlementSalary()
        {
            EmployeeSalaryID = 0;
            EmployeeID = 0; 
            LocationID = 0;
            DepartmentID = 0;
            DesignationID = 0;
            GrossAmount = 0; 
            NetAmount = 0;
            TotalWorkingDay = 0; 
            TotalAbsent = 0; 
            TotalDayOff = 0;
            TotalHoliday = 0;
            TotalEarlyLeaving = 0; 
            TotalLate = 0;
            TotalPLeave = 0;
            TotalUpLeave = 0;
            RevenueStemp = 0;
            EmployeeName = "";
            EmployeeCode = "";
            JoiningDate = DateTime.Now;
            LocationName = "";
            DepartmentName = "";
            DesignationName = "";
            OTHour = 0;
            OTRatePerHour = 0;


            CompGrossAmount = 0.0;
            CompNetAmount = 0.0;
            CompOTHour= 0.0;
            CompOTRatePerHour= 0.0;
            CompTotalWorkingDay= 0;
            CompTotalAbsent= 0;
            CompTotalLate= 0;
            CompTotalEarlyLeaving= 0;
            CompTotalDayOff= 0;
            CompTotalLeave= 0;
            CompTotalHoliday = 0;

            StartDate = DateTime.Now;
            EndDate = DateTime.Now;

        }
    #endregion
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }

        public double CompGrossAmount { get; set; }
        public double CompNetAmount { get; set; }
        public double CompOTHour { get; set; }
        public double CompOTRatePerHour { get; set; }

        public int CompTotalWorkingDay { get; set; }
        public int CompTotalAbsent { get; set; }
        public int CompTotalLate { get; set; }
        public int CompTotalEarlyLeaving { get; set; }
        public int CompTotalDayOff { get; set; }
        public int CompTotalLeave { get; set; }
        public int CompTotalHoliday { get; set; }



        public int EmployeeSalaryID { get; set; }
        public int EmployeeID { get; set; }
        public int LocationID { get; set; }
        public int DepartmentID { get; set; }
        public int DesignationID { get; set; }
        public double GrossAmount { get; set; }
        public double OTHour { get; set; }
        public double OTRatePerHour { get; set; }
        public double NetAmount { get; set; }
        public int TotalWorkingDay { get; set; }
        public int TotalAbsent { get; set; }
        public int TotalDayOff { get; set; }
        public int TotalHoliday { get; set; }
        public int TotalEarlyLeaving { get; set; }
        public int TotalLate { get; set; }
        public int TotalPLeave { get; set; }
        public int TotalUpLeave { get; set; }
        public double RevenueStemp { get; set; }
        public string EmployeeName { get; set; }
        public string EmployeeCode { get; set; }
        public string LocationName { get; set; }
        public string DepartmentName { get; set; }
        public string DesignationName { get; set; }
        public DateTime JoiningDate { get; set; }

        #region derivedproperties

        public string JoiningDateInString
        {
            get
            {
                return JoiningDate.ToString("dd MMM yyyy");
            }
        }
        public int Present { 
            get 
            {
                return (this.TotalWorkingDay - this.TotalAbsent - this.TotalUpLeave - this.TotalPLeave);
            } 
        }

        #endregion

        #region Functions
        public static List<EmployeeSettlementSalary> Gets(string sSQL, long nUserID)
        {
            return EmployeeSettlementSalary.Service.Gets(sSQL, nUserID);
        }
        public static EmployeeSettlementSalary Get(string sSQL, long nUserID)
        {
            return EmployeeSettlementSalary.Service.Get(sSQL, nUserID);
        }
        #endregion

        #region ServiceFactory

        internal static IEmployeeSettlementSalaryService Service
        {
            get { return (IEmployeeSettlementSalaryService)Services.Factory.CreateService(typeof(IEmployeeSettlementSalaryService)); }
        }
        #endregion
    }

    #region IEmployeeSettlementSalary interface

    public interface IEmployeeSettlementSalaryService
    {
        List<EmployeeSettlementSalary> Gets(string sSQL, Int64 nUserID);
        EmployeeSettlementSalary Get(string sSQL, Int64 nUserID);
    }
    #endregion
}


