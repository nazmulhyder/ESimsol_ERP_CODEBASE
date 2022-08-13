using System;
using System.IO;
using ESimSol.BusinessObjects;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Collections.Generic;
using ICS.Core;
using ICS.Core.Framework;
using ICS.Core.Utility;

namespace ESimSol.BusinessObjects.ReportingObject
{
    #region AMGSalarySheet
    public class AMGSalarySheet
    {
        #region  Constructor
        public AMGSalarySheet()
        {
            EmployeeSalaryID = 0;
            EmployeeID = 0;
            BUID = 0; 
            LocationID = 0;
            DepartmentID = 0; 
            DesignationID = 0; 
            TotalDays = 0; 
            DayOffHoliday  = 0;
            EWD = 0; 
            LWP = 0;
            CL = 0;
            EL = 0; 
            SL = 0;
            ML = 0; 
            Absent = 0;
            Present = 0;
            Gross = 0;
            Basics = 0;
            HR = 0;
            Med = 0; 
            Food = 0;
            Conv = 0;
            Earning = 0; 
            AttBonus = 0;
            OT_HR = 0;
            OT_Rate = 0; 
            OT_Amount = 0;
            GrossEarning = 0;
            AbsentAmount = 0;
            Advance = 0;
            Stemp = 0;
            Welfare = 0;
            TotalDeduction = 0;
            NetAmount = 0;
            Code = string.Empty;
            Name  = string.Empty;
            DOJ = DateTime.Now;
            Grade = string.Empty;
            BUName = string.Empty;
            LocName = string.Empty;
            DptName = string.Empty;
            DsgName = string.Empty;
            AccountNo = string.Empty;
            BankName = string.Empty;
            ExtraOTHR = 0;
            ExtraOTAmount = 0;
            BUAddress = string.Empty;
            EmpNameInBangla = string.Empty;
            DsgNameInBangla = string.Empty;
            GroupByID = 0;
            SalaryHeads = new List<SalaryHead>();
            AMGSalarySheets = new List<AMGSalarySheet>();
            EmployeeSalaryDetails = new List<EmployeeSalaryDetail>();
            EmployeeSalaryDetailDisciplinaryActions = new List<EmployeeSalaryDetailDisciplinaryAction>();
            Employees = new List<Employee>();
            ErrorMessage = "";
            Company = new Company();
            StartDate = DateTime.Today;
            EndDate = DateTime.Today;
            BlockName = "";
        }
        #endregion
        #region Properties
        public int EmployeeSalaryID { get; set; }
        public int EmployeeID { get; set; }
        public int BUID { get; set; }
        public int LocationID { get; set; }
        public int DepartmentID { get; set; }
        public int DesignationID { get; set; }
        public int TotalDays { get; set; }
        public int DayOffHoliday { get; set; }
        public int EWD { get; set; } 
        public int LWP { get; set; }
        public int CL { get; set; }
        public int EL { get; set; }
        public int SL { get; set; }
        public int ML { get; set; }
        public int Absent { get; set; }
        public int Present { get; set; }
        public double Gross { get; set; }
        public double ExtraOTHR { get; set; }
        public double ExtraOTAmount { get; set; }
        public double Basics { get; set; }
        public double HR { get; set; }
        public double Med { get; set; }
        public double Food { get; set; }
        public double Conv { get; set; }
        public double Earning { get; set; }
        public double AttBonus { get; set; }
        public double OT_HR { get; set; }
        public double OT_Rate { get; set; }
        public double OT_Amount { get; set; }
        public double GrossEarning { get; set; }
        public double AbsentAmount { get; set; }
        public double Advance { get; set; }
        public double Stemp { get; set; }
        public double Welfare { get; set; }
        public double TotalDeduction { get; set; }
        public double NetAmount { get; set; }
        public string Code { get; set; }
        public string Name  { get; set; }
        public DateTime DOJ { get; set; }
        public string Grade { get; set; }
        public string BUName { get; set; }
        public string LocName { get; set; }
        public string DptName { get; set; }
        public string DsgName { get; set; }
        public string AccountNo { get; set; }
        public string BankName { get; set; }
        public string BUAddress { get; set; }
        public string EmpNameInBangla { get; set; }
        public string DsgNameInBangla { get; set; }
        public int GroupByID { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string BlockName { get; set; }
        public string ErrorMessage { get; set; }       
        public Company Company { get; set; }
        #endregion

        #region Derived Property
        public int LeaveDays
        {
            get
            {
                return this.LWP + this.CL + this.EL + this.SL + this.ML;
            }
        }

        public string DOJInStr
        {
            get
            {
                return this.DOJ.ToString("dd MMM yyyy");
            }
        }
        public string DOJInStrWithSlash
        {
            get
            {
                return this.DOJ.ToString("dd/MM/yyyy");
            }
        }
        public string SalaryDate
        {
            get
            {
                return DateTime.Now.ToString("dd/MM/yyyy");
            }
        }

        public string DOJShort
        {
            get
            {
                return this.DOJ.ToString("dd-MMM-yy");
            }
        }
       
        public List<SalaryHead> SalaryHeads { get; set; }
        public List<AMGSalarySheet> AMGSalarySheets { get; set; }
        public List<Employee> Employees { get; set; }
        public List<EmployeeSalaryDetail> EmployeeSalaryDetails { get; set; }
        public List<EmployeeSalaryDetailDisciplinaryAction> EmployeeSalaryDetailDisciplinaryActions { get; set; }
        #endregion

        #region Functions
        public static List<AMGSalarySheet> Gets(string BUIDs, string LocationIDs, string DepartmentIDs, string DesignationIDs, string SalarySchemeIDs, string EmployeeIDs, int nMonthID, int nYear, bool IsNewJoin, int IsOutSheet, double nMinSalary, double nMaxSalary, string sGroupIDs, string sBlockIDs, bool IsComp, long nUserID)
        {
            return AMGSalarySheet.Service.Gets(BUIDs, LocationIDs, DepartmentIDs, DesignationIDs, SalarySchemeIDs, EmployeeIDs, nMonthID, nYear, IsNewJoin, IsOutSheet, nMinSalary, nMaxSalary, sGroupIDs, sBlockIDs, IsComp, nUserID);
        }
        public static List<AMGSalarySheet> GetsComp(string BUIDs, string LocationIDs, string DepartmentIDs, string DesignationIDs, string SalarySchemeIDs, string EmployeeIDs, int nMonthID, int nYear, bool IsNewJoin, int IsOutSheet, double nMinSalary, double nMaxSalary, string sGroupIDs, string sBlockIDs, int nTimeCardID, long nUserID)
        {
            return AMGSalarySheet.Service.GetsComp(BUIDs, LocationIDs, DepartmentIDs, DesignationIDs, SalarySchemeIDs, EmployeeIDs, nMonthID, nYear, IsNewJoin, IsOutSheet, nMinSalary, nMaxSalary, sGroupIDs, sBlockIDs, nTimeCardID, nUserID);
        }
        public static List<AMGSalarySheet> GetsPaySlip(string BUIDs, string LocationIDs, string DepartmentIDs, string DesignationIDs, string SalarySchemeIDs, string EmployeeIDs, int nMonthID, int nYear, bool IsNewJoin, int IsOutSheet, double nMinSalary, double nMaxSalary, string sGroupIDs, string sBlockIDs, int nMOCID, long nUserID)
        {
            return AMGSalarySheet.Service.GetsPaySlip(BUIDs, LocationIDs, DepartmentIDs, DesignationIDs, SalarySchemeIDs, EmployeeIDs, nMonthID, nYear, IsNewJoin, IsOutSheet, nMinSalary, nMaxSalary, sGroupIDs, sBlockIDs, nMOCID, nUserID);
        }
        #endregion

        #region Non DB Function

        #endregion
        #region ServiceFactory
        internal static IAMGSalarySheetService Service
        {
            get { return (IAMGSalarySheetService)Services.Factory.CreateService(typeof(IAMGSalarySheetService)); }
        }

        #endregion
    }
    #endregion


    #region IAMGSalarySheet interface
    public interface IAMGSalarySheetService
    {
        List<AMGSalarySheet> Gets(string BUIDs, string LocationIDs, string DepartmentIDs, string DesignationIDs, string SalarySchemeIDs, string EmployeeIDs, int nMonthID, int nYear, bool IsNewJoin, int IsOutSheet, double nMinSalary, double nMaxSalary, string sGroupIDs, string sBlockIDs, bool IsComp, Int64 nUserID);
        List<AMGSalarySheet> GetsComp(string BUIDs, string LocationIDs, string DepartmentIDs, string DesignationIDs, string SalarySchemeIDs, string EmployeeIDs, int nMonthID, int nYear, bool IsNewJoin, int IsOutSheet, double nMinSalary, double nMaxSalary, string sGroupIDs, string sBlockIDs, int nTimeCardID, Int64 nUserID);
        List<AMGSalarySheet> GetsPaySlip(string BUIDs, string LocationIDs, string DepartmentIDs, string DesignationIDs, string SalarySchemeIDs, string EmployeeIDs, int nMonthID, int nYear, bool IsNewJoin, int IsOutSheet, double nMinSalary, double nMaxSalary, string sGroupIDs, string sBlockIDs, int nMOCID, long nUserID);

    }

    #endregion

}
