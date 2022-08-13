using System;
using System.IO;
using ESimSol.BusinessObjects;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Collections.Generic;
using ICS.Core.Framework;

namespace ESimSol.BusinessObjects
{
    #region EmployeeAdvanceSalary
    public class EmployeeAdvanceSalary : BusinessObject
    {
        public EmployeeAdvanceSalary()
        {
            EASID = 0;
            EASPID = 0;
            EmployeeID = 0;
            LocationID = 0;
            LocationName = "";
            Code = "";
            BusinessUnitID = 0;
            BUName = "";
            Name = "";
            DesignationID = 0;
            DepartmentName = "";
            DepartmentID = 0;
            DepartmentIDs = "";
            LocationIDs = "";
            DesignationName = "";
            DRPID = 0;
            TotalPresent = 0;
            TotalAbsent = 0;
            TotalPaidLeave = 0;
            TotalUPLeave = 0;
            TotalLateInDays = 0;
            TotalEarlyInDays = 0;
            TotalLateInMin = 0;
            TotalEarlyInMin = 0;
            TotalHoliday = 0;
            TotalDayOff = 0;
            GrossSalary = 0.0;
            GrossEarnings = 0.0;
            TotalDeductions = 0.0;
            NetAmount = 0.0;
            ErrorMessage = "";
            Params = "";
            JoiningDate = DateTime.Now;
            BlockName = "";
        }
        #region Properties
        public int EASID { get; set; }
        public int EASPID { get; set; }
        public int BusinessUnitID { get; set; }
        public int LocationID { get; set; }
        public int EmployeeID { get; set; }
        public int DepartmentID { get; set; }
        public string LocationName { get; set; }
        public string BlockName { get; set; }
        public string BUName { get; set; }
        public string Name { get; set; }
        public string DepartmentIDs { get; set; }
        public string LocationIDs { get; set; }
        public string DepartmentName { get; set; }
        public string Code { get; set; }
        public int DesignationID { get; set; }
        public string DesignationName { get; set; }
        public int DRPID { get; set; }
        public int TotalPresent { get; set; }
        public int TotalAbsent { get; set; }
        public int TotalPaidLeave { get; set; }
        public int TotalUPLeave { get; set; }
        public int TotalLateInDays { get; set; }
        public int TotalEarlyInDays { get; set; }
        public int TotalLateInMin { get; set; }
        public int TotalEarlyInMin { get; set; }
        public int TotalHoliday { get; set; }
        public int TotalDayOff { get; set; }
        public double GrossSalary { get; set; }
        public double GrossEarnings { get; set; }
        public double TotalDeductions { get; set; }
        public double NetAmount { get; set; }
        public string ErrorMessage { get; set; }
        public string Params { get; set; }
        public DateTime JoiningDate { get; set; }
        #endregion

        #region Derived Property
        public List<EmployeeAdvanceSalary> EmployeeAdvanceSalarys { get; set; }

        public double PaymentDays
        {
            get
            {
                double nTotal = 0.0;
                nTotal = this.TotalPresent + this.TotalPaidLeave + this.TotalHoliday + this.TotalDayOff;
                if (nTotal > 0)
                    return nTotal;
                else return 0;
            }
        }
        public string TotalPresentSt
        {
            get
            {
                if (TotalPresent > 0)
                    return TotalPresent.ToString();
                else
                    return "-";
            }
        }
        public string TotalAbsentSt
        {
            get
            {
                if (TotalAbsent > 0)
                    return TotalAbsent.ToString();
                else
                    return "-";
            }
        }
        public string TotalPaidLeaveSt
        {
            get
            {
                if (TotalPaidLeave > 0)
                    return TotalPaidLeave.ToString();
                else
                    return "-";
            }
        }
        public string TotalUPLeaveSt
        {
            get
            {
                if (TotalUPLeave > 0)
                    return TotalUPLeave.ToString();
                else
                    return "-";
            }
        }
        public string TotalLateInDaysSt
        {
            get
            {
                if (TotalLateInDays > 0)
                    return TotalLateInDays.ToString();
                else
                    return "-";
            }
        }
        public string TotalEarlyInDaysSt
        {
            get
            {
                if (TotalEarlyInDays > 0)
                    return TotalEarlyInDays.ToString();
                else
                    return "-";
            }
        }
        public string TotalLateInMinSt
        {
            get
            {
                if (TotalLateInMin > 0)
                    return TotalLateInMin.ToString();
                else
                    return "-";
            }
        }
        public string TotalEarlyInMinsSt
        {
            get
            {
                if (TotalEarlyInMin > 0)
                    return TotalEarlyInMin.ToString();
                else
                    return "-";
            }
        }
        public string TotalHolidaySt
        {
            get
            {
                if (TotalHoliday > 0)
                    return TotalHoliday.ToString();
                else
                    return "-";
            }
        }
        public string TotalDayOffSt
        {
            get
            {
                if (TotalDayOff > 0)
                    return TotalDayOff.ToString();
                else
                    return "-";
            }
        }
        public string JoiningDateInString
        {
            get
            {
                return this.JoiningDate.ToString("dd MMM yyyy");
            }
        }





        public Company Company { get; set; }

        #endregion

        #region Functions
        public EmployeeAdvanceSalary Save(int nUserID)
        {
            return EmployeeAdvanceSalary.Service.Save(this, nUserID);
        }
        public string Delete(int id, int nUserID)
        {
            return EmployeeAdvanceSalary.Service.Delete(id, nUserID);
        }
        public static List<EmployeeAdvanceSalary> Gets(int nUserID)
        {
            return EmployeeAdvanceSalary.Service.Gets(nUserID);
        }
        public static List<EmployeeAdvanceSalary> Gets(string sSQL, int nUserID)
        {
            return EmployeeAdvanceSalary.Service.Gets(sSQL, nUserID);
        }
        public static EmployeeAdvanceSalary Get(string sSQL, long nUserID)
        {
            return EmployeeAdvanceSalary.Service.Get(sSQL, nUserID);
        }
        public int EmployeeAdvanceSalarySave(int nIndex, int EASPID, long nUserID)
        {
            return EmployeeAdvanceSalary.Service.EmployeeAdvanceSalarySave(nIndex, EASPID, nUserID);
        }

        #endregion

        #region ServiceFactory
        internal static IEmployeeAdvanceSalaryService Service
        {
            get { return (IEmployeeAdvanceSalaryService)Services.Factory.CreateService(typeof(IEmployeeAdvanceSalaryService)); }
        }
        #endregion
    }
    #endregion

    #region IEmployeeAdvanceSalary interface
    public interface IEmployeeAdvanceSalaryService
    {
        EmployeeAdvanceSalary Save(EmployeeAdvanceSalary oEmployeeAdvanceSalary, int nUserID);
        List<EmployeeAdvanceSalary> Gets(int nUserID);
        List<EmployeeAdvanceSalary> Gets(string sSQL, int nUserID);
        string Delete(int id, int nUserID);
        EmployeeAdvanceSalary Get(string sSQL, Int64 nUserID);
        int EmployeeAdvanceSalarySave(int nIndex, int EASPID, long nUserID);
    }
    #endregion
}

