using System;
using System.IO;
using ESimSol.BusinessObjects;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Collections.Generic;
using System.Drawing;
using ICS.Core;
using ICS.Core.Framework;
using ICS.Core.Utility;


namespace ESimSol.BusinessObjects
{
    public class CompliancePayrollProcessManagement : BusinessObject
    {
        public CompliancePayrollProcessManagement()
        {
            PPMID = 0;
            CompanyID = 0;
            BusinessUnitID = 0;
            LocationID = 0;
            DepartmentID = 0;
            YearID = 0;
            MonthID = 0;            
            MOCID = 0;
            PaymentCycle = EnumPaymentCycle.None;
            ProcessDate = DateTime.Now;
            SalaryFrom = DateTime.Now;
            SalaryTo = DateTime.Now;
            ApprovedBy = 0;
            BUCode = "";
            BUName = "";
            LocCode = "";
            LocName = "";
            DeptCode = "";
            DeptName = "";
            TimeCardName = "";
            ApprovedByName = "";
            EmpCount = 0;
            BusinessUnits = new List<BusinessUnit>();
            Locations = new List<Location>();
            Departments = new List<Department>();
            TimeCards = new List<MaxOTConfiguration>();
            ErrorMessage = "";
            PPMIDs = "";
        }
        #region Properties
        public int PPMID { get; set; }
        public int CompanyID { get; set; }
        public int BusinessUnitID { get; set; }
        public int LocationID { get; set; }
        public int EmpCount { get; set; }
        public int DepartmentID { get; set; }
        public int YearID { get; set; }
        public string ErrorMessage { get; set; }
        public int MonthID { get; set; }        
        public int MOCID { get; set; }
        public EnumPaymentCycle PaymentCycle { get; set; }
        public DateTime ProcessDate { get; set; }
        public DateTime SalaryFrom { get; set; }
        public DateTime SalaryTo { get; set; }
        public int ApprovedBy { get; set; }
        public string BUCode { get; set; }
        public string BUName { get; set; }
        public string LocCode { get; set; }
        public string LocName { get; set; }
        public string DeptCode { get; set; }
        public string DeptName { get; set; }
        public string TimeCardName { get; set; }
        public string ApprovedByName { get; set; }
        public List<BusinessUnit> BusinessUnits { get; set; }
        public List<Location> Locations { get; set; }
        public List<Department> Departments { get; set; }
        public List<MaxOTConfiguration> TimeCards { get; set; }
        #endregion
        #region Derived Properties
        public string MonthInString
        {
            get
            {
                return EnumObject.jGet((EnumMonth)this.MonthID);
            }
        }
        public string SalaryFromInString
        {
            get
            {
                return this.SalaryFrom.ToString("dd MMM yyyy");
            }
        }
        public string SalaryToInString
        {
            get
            {
                return this.SalaryTo.ToString("dd MMM yyyy");
            }
        }
        public string ProcessDateInString
        {
            get
            {
                return this.ProcessDate.ToString("dd MMM yyyy");
            }
        }
        public string PPMIDs { get; set; } 
        #endregion

        #region Functions
        public static List<CompliancePayrollProcessManagement> Gets(string sSQL, long nUserID)
        {
            return CompliancePayrollProcessManagement.Service.Gets(sSQL, nUserID);
        }

        public static List<CompliancePayrollProcessManagement> GetsRunningEmployeeBatchs(CompliancePayrollProcessManagement oCPPM, long nUserID)
        {
            return CompliancePayrollProcessManagement.Service.GetsRunningEmployeeBatchs(oCPPM, nUserID);
        }
        public static List<CompliancePayrollProcessManagement> GetsArchiveEmployeeBatchs(CompliancePayrollProcessManagement oCPPM, long nUserID)
        {
            return CompliancePayrollProcessManagement.Service.GetsArchiveEmployeeBatchs(oCPPM, nUserID);
        }
        public static CompliancePayrollProcessManagement CompPayRollProcess(CompliancePayrollProcessManagement oCPPM, string sBUIDs, string sLocationIDs, string sDepartmentIDs, long nUserID)
        {
            return CompliancePayrollProcessManagement.Service.CompPayRollProcess(oCPPM, sBUIDs, sLocationIDs, sDepartmentIDs, nUserID);
        }
        public static string DeleteCompPayRollProcess(string sCPPMIDs, long nUserID)
        {
            return CompliancePayrollProcessManagement.Service.DeleteCompPayRollProcess(sCPPMIDs, nUserID);
        }
        #endregion

        #region ServiceFactory
        internal static ICompliancePayrollProcessManagementService Service
        {
            get { return (ICompliancePayrollProcessManagementService)Services.Factory.CreateService(typeof(ICompliancePayrollProcessManagementService)); }
        }
        #endregion
    }

    #region ICompliancePayrollProcessManagementService interface

    public interface ICompliancePayrollProcessManagementService
    {
        List<CompliancePayrollProcessManagement> Gets(string sSQL, long nUserID);
        List<CompliancePayrollProcessManagement> GetsRunningEmployeeBatchs(CompliancePayrollProcessManagement oCPPM, long nUserID);
        List<CompliancePayrollProcessManagement> GetsArchiveEmployeeBatchs(CompliancePayrollProcessManagement oCPPM, long nUserID);
        CompliancePayrollProcessManagement CompPayRollProcess(CompliancePayrollProcessManagement oCPPM, string sBUIDs, string sLocationIDs, string sDepartmentIDs, long nUserID);
        string DeleteCompPayRollProcess(string sCPPMIDs, long nUserID);
    }
    #endregion
}
