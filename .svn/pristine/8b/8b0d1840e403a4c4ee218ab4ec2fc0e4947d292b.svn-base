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
    #region EmployeeSalary
    [DataContract]
    public class EmployeeSalary : BusinessObject
    {
        public EmployeeSalary()
        {
            EmployeeSalaryID = 0;
            EmployeeID = 0;
            LocationID = 0;
            DepartmentID = 0;
            DesignationID = 0;
            GrossAmount = 0;
            NetAmount = 0;
            NetAmount_ZN = 0;
            CurrencyID = 0;
            SalaryDate = DateTime.Now;
            SalaryReceiveDate = DateTime.Now;
            PayrollProcessID = 0;
            IsManual = true;
            StartDate = DateTime.Now;
            EndDate = DateTime.Now;
            IsLock = true;

            ProductionAmount = 0;
            ProductionBonus = 0;
            OTHour = 0;
            OTHr_2ndPortion = 0;
            OTHr_Rest = 0;
            OTRatePerHour = 0;
            TotalWorkingDay = 0;
            TotalAbsent = 0;
            TotalLate = 0;
            TotalEarlyLeaving = 0;
            TotalDayOff = 0;
            TotalHoliday = 0;
            TotalUpLeave = 0;
            TotalPLeave = 0;
            RevenueStemp = 0;
            TotalNoWorkDay = 0;
            TotalNoWorkDayAllowance = 0;
            AddShortFall = 0;
            IsProductionBase = true;
            IsAllowOverTime = true;
            AllowanceNameWithDay = "";
            ErrorMessage = "";
            EmployeeTypeName = "";
            Gender= "";
            CashAmount = 0;
            BankAmount = 0;
            Params = "";
            Allowance = 0.0;

            CompOTHour = 0;
            CompOTRatePerHour = 0;
            CompNetAmount = 0;

            BusinessUnitID = 0;
            BUName = "";

            BMMID = 0;
            BlockName = "";

            CompGrossAmount = 0;
            LateInMin = 0;
            IsOutSheet = false;

            CompTotalAbsent = 0;
            CompTotalDayOff = 0;
            CompTotalHoliday = 0;
            CompTotalLeave = 0;
            CompTotalLate = 0;
            CompTotalEarlyLeave = 0;
            CompLateInMin = 0;
            CompTotalWorkingDay = 0;
            BusinessUnits = new List<BusinessUnit>();
            NoOfEmployee = 0;
            GrossOnEWD = 0;
            TotalPayable = 0.0;
            OTAmount = 0.0;
            MonthID = 0;
            Year = 0;
            IsBank = false;
            IsCash = false;
            MOCID = 0;
            ETIN = "";
        }

        #region Properties
        public string ETIN { get; set; }
        public int MOCID { get; set; }
        public int NoOfEmployee { get; set; }
        public int MonthID { get; set; }
        public int Year { get; set; }
        public int CompTotalAbsent { get; set; }
        public int CompTotalDayOff { get; set; }
        public int CompTotalHoliday { get; set; }
        public int CompTotalLeave { get; set; }
        public int CompTotalLate { get; set; }
        public int CompTotalEarlyLeave { get; set; }
        public int CompTotalWorkingDay { get; set; }
        public int CompLateInMin { get; set; }
        public int EmployeeSalaryID { get; set; }
        public int EmployeeID { get; set; }
        public int LocationID { get; set; }
        public int DepartmentID { get; set; }
        public int DesignationID { get; set; }
        public double TotalPayable { get; set; }
        public double GrossAmount { get; set; }
        public double OTAmount { get; set; }
        public double GrossOnEWD { get; set; }
        public double Allowance { get; set; }
        public double NetAmount { get; set; }
        public double NetAmount_ZN { get; set; }
        public int CurrencyID { get; set; }
        public DateTime SalaryDate { get; set; }
        public DateTime SalaryReceiveDate { get; set; }
        public int PayrollProcessID { get; set; }
        public bool IsManual { get; set; }
        public bool IsOutSheet { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public bool IsLock { get; set; }
        public double ProductionAmount { get; set; }
        public double ProductionBonus { get; set; }
        public double OTHour { get; set; }
        public double OTHr_2ndPortion { get; set; }
        public double OTHr_Rest { get; set; }
        public double OTRatePerHour { get; set; }
        public int TotalWorkingDay { get; set; }
        public int TotalAbsent { get; set; }
        public int TotalLate { get; set; }
        public int TotalEarlyLeaving { get; set; }
        public int EarlyLeavingMinute { get; set; }
        public int TotalDayOff { get; set; }
        public int TotalHoliday { get; set; }
        public int TotalUpLeave { get; set; }
        public int TotalPLeave { get; set; }
        public double RevenueStemp { get; set; }
        public int TotalNoWorkDay { get; set; }
        public double TotalNoWorkDayAllowance { get; set; }
        public double AddShortFall { get; set; }
        public string ErrorMessage { get; set; }
        public string EmployeeTypeName { get; set; }
        public string Gender { get; set; }
        public string Params { get; set; }

        public double CompOTHour { get; set; }
        public double CompOTRatePerHour { get; set; }
        public double CompNetAmount { get; set; }

        public double CompGrossAmount { get; set; }
        public int LateInMin { get; set; }
       
        #endregion

        #region Derived Property
        public Company Company { get; set; }
        public List<EmployeeSalary> EmployeeSalarys { get; set; }
        public List<EmployeeSalaryDetail> EmployeeSalaryDetails { get; set; }
        public List<RPTSalarySheet> EmployeeSalarySheets { get; set; }
        public List<RPTSalarySheetDetail> EmployeeSalarySheetDetails { get; set; }
        public EmployeeSalaryDetail EmployeeSalaryDetail { get; set; }
        public List<SalaryHead> SalaryHeads { get; set; }
        public List<EmployeeSalaryDetailDisciplinaryAction> EmployeeSalaryDetailDisciplinaryActions { get; set; }
        public EmployeeOfficial EmployeeOfficial { get; set; }
        public List<EmployeeOfficial> EmployeeOfficials { get; set; }
        public List<Employee> Employees { get; set; }
        public List<AttendanceDaily> AttendanceDailys { get; set; }
        public List<EmployeeBankAccount> EmployeeBankAccounts  { get; set; }
        public List<TransferPromotionIncrement> TransferPromotionIncrements { get; set; }
        public List<BusinessUnit> BusinessUnits { get; set; }
        public string EmployeeName { get; set; }
        public string EmployeeNameInBangla { get; set; }
        public string EmployeeCode { get; set; }
        public string LocationName { get; set; }
        public string DepartmentName { get; set; }
        public string ParentDepartmentName { get; set; }
        public string DesignationName { get; set; }
        public DateTime JoiningDate { get; set; }
        public DateTime DateOfConfirmation { get; set; }
        public bool IsProductionBase { get; set; }
        public bool IsAllowOverTime { get; set; }
        public string AllowanceNameWithDay { get; set; }
        public double CashAmount { get; set; }
        public double BankAmount { get; set; }
        public int BusinessUnitID { get; set; }
        public string BUName { get; set; }
        public int BMMID { get; set; }
        public string BlockName { get; set; }
        public bool IsBank { get; set; }
        public bool IsCash { get; set; }
        public string EmployeeNameCode
        {
            get
            {
                return EmployeeName + "[" + EmployeeCode + "]";
            }
        }

        public string JoiningDateInString
        {
            get
            {
                return JoiningDate.ToString("dd MMM yyyy");
            }
        }
        public string DateOfConfirmationInString
        {
            get
            {
                return DateOfConfirmation.ToString("dd MMM yyyy");
            }
        }

        public string StartDateInString
        {
            get
            {
                return StartDate.ToString("dd MMM yyyy");
            }
        }

        public string EndDateInString
        {
            get
            {
                return EndDate.ToString("dd MMM yyyy");
            }
        }

        public string SalaryDateInString
        {
            get
            {
                return SalaryDate.ToString("dd MMM yyyy");
            }
        }

        public string SalaryForInString
        {
            get
            {
                return StartDate.ToString("dd MMM yyyy") + " To " + EndDate.ToString("dd MMM yyyy");
            }
        }

        public double OverTimeAmount
        {
            get
            {
                return OTHour * OTRatePerHour;
            }
        }
        public double CompOverTimeAmount
        {
            get
            {
                return CompOTHour * CompOTRatePerHour;
            }
        }
        public string OverTimeAmountSt
        {
            get
            {
                if (OTHour * OTRatePerHour > 0) return (OTHour * OTRatePerHour).ToString(); else return "-";
            }
        }
        public double AddShort
        {
            get
            {
                //if ((GrossAmount - ProductionAmount)>0)
                //{
                //    return GrossAmount - ProductionAmount;
                //}

                //else
                //{
                //    return 0;
                //}
                return this.AddShortFall;
            }
        }
        public double TotalPresent
        {
            get
            {
                return TotalWorkingDay - TotalAbsent - TotalUpLeave - TotalPLeave;
            }
        }
        public double CompTotalPresent
        {
            get
            {
                return CompTotalWorkingDay - CompTotalAbsent - CompTotalLeave;
            }
        }
        public double DeriveNetAmount
        {
            get
            {
                if (this.NetAmount < 0) return 0; else return NetAmount;

            }
        }
        public double EWD { get { return this.TotalPresent + this.TotalHoliday + this.TotalDayOff + this.TotalPLeave + this.TotalUpLeave; } }//Employee Working Day(EWD)
        public double PD { get { return this.TotalPresent + this.TotalHoliday + this.TotalDayOff + this.TotalPLeave; } }//Payment Day(PD)

        #region Payroll Break Down
        public double Salary { get; set; }
        public double Overtime { get; set; }


        public string SalaryEndYear
        {
            get
            {
                return this.EndDate.ToString("yyyy");
            }
        }

        public string SalaryEndMonth
        {
            get
            {
                return this.EndDate.ToString("MMM");
            }
        }
       
        #endregion

        #endregion

        #region Functions
        public static EmployeeSalary Get(int id, long nUserID)
        {
            return EmployeeSalary.Service.Get(id, nUserID);
        }

        public static EmployeeSalary Get(string sSQL, long nUserID)
        {
            return EmployeeSalary.Service.Get(sSQL, nUserID);
        }

        public static List<EmployeeSalary> Gets(long nUserID)
        {
            return EmployeeSalary.Service.Gets(nUserID);
        }

        public static List<EmployeeSalary> Gets(string sSQL, long nUserID)
        {
            return EmployeeSalary.Service.Gets(sSQL, nUserID);
        }
        public EmployeeSalary ProcessSalaryComp(long nUserID)
        {
            return EmployeeSalary.Service.ProcessSalaryComp(this, nUserID);
        }
        public EmployeeSalary ProcessSalary(long nUserID)
        {
            return EmployeeSalary.Service.ProcessSalary(this, nUserID);
        }

        public EmployeeSalary IUD(int nDBOperation, long nUserID)
        {
            return EmployeeSalary.Service.IUD(this, nDBOperation, nUserID);
        }
        public static List<EmployeeSalary> Gets_ZN(string sEmpIDs, string sDate, string sDepartmentIDs, string sDesignationIDs, string sSalarySchemeIDs, long nUserID)
        {
            return EmployeeSalary.Service.Gets_ZN(sEmpIDs, sDate, sDepartmentIDs, sDesignationIDs, sSalarySchemeIDs, nUserID);
        }
        public static List<EmployeeSalary> GetsComparisonReport(string BUIDs, string LocIDs, string DeptIDs, string DesignationIDs, string SchemeIDs, string EmpIDs, bool isMonthWise, int MonthFrom, int YearFrom, int MonthTo, int YearTo, int ComparisonYearFrom, int ComparisonYearTo, double MinSalary, double MaxSalary, string GroupIDs, string BlockIDs, int GroupBy, long nUserID)
        {
            return EmployeeSalary.Service.GetsComparisonReport(BUIDs, LocIDs, DeptIDs, DesignationIDs, SchemeIDs, EmpIDs, isMonthWise, MonthFrom, YearFrom, MonthTo, YearTo, ComparisonYearFrom, ComparisonYearTo, MinSalary, MaxSalary, GroupIDs, BlockIDs, GroupBy, nUserID);
        }
        public static List<EmployeeSalary> GetsPayRollBreakDown(DateTime StartDate, DateTime EndDate, bool IsDateSearch, int nLocationID, long nUserID)
        {
            return EmployeeSalary.Service.GetsPayRollBreakDown(StartDate, EndDate, IsDateSearch, nLocationID, nUserID);
        }
        public static List<EmployeeSalary> GetsEmployeeSettleemtSalary(string sSQL, long nUserID)
        {
            return EmployeeSalary.Service.GetsEmployeeSettleemtSalary(sSQL, nUserID);
        }
        public static bool UpdateOutSheet(string sSQL, long nUserID)
        {
            return EmployeeSalary.Service.UpdateOutSheet(sSQL, nUserID);
        }
        #endregion

        #region ServiceFactory
        internal static IEmployeeSalaryService Service
        {
            get { return (IEmployeeSalaryService)Services.Factory.CreateService(typeof(IEmployeeSalaryService)); }
        }
        #endregion
    }
    #endregion

    #region IEmployeeSalary interface
    
    public interface IEmployeeSalaryService
    {
        EmployeeSalary Get(int id, Int64 nUserID);

        EmployeeSalary Get(string sSQL, Int64 nUserID);

        List<EmployeeSalary> Gets(Int64 nUserID);

        bool UpdateOutSheet(string sSQL, Int64 nUserID);
        EmployeeSalary ProcessSalaryComp(EmployeeSalary oEmployeeSalary, Int64 nUserID);
        EmployeeSalary ProcessSalary(EmployeeSalary oEmployeeSalary, Int64 nUserID);
        List<EmployeeSalary> Gets(string sSQL, Int64 nUserID);
        List<EmployeeSalary> GetsComparisonReport(string BUIDs, string LocIDs, string DeptIDs, string DesignationIDs, string SchemeIDs, string EmpIDs, bool isMonthWise, int MonthFrom, int YearFrom, int MonthTo, int YearTo, int ComparisonYearFrom, int ComparisonYearTo, double MinSalary, double MaxSalary, string GroupIDs, string BlockIDs, int GroupBy, long nUserID);
        EmployeeSalary IUD(EmployeeSalary oEmployeeSalarySheet, int nDBOperation, Int64 nUserID);
        List<EmployeeSalary> Gets_ZN(string sEmpIDs, string sDate, string sDepartmentIDs, string sDesignationIDs, string sSalarySchemeIDs, Int64 nUserID);
        List<EmployeeSalary> GetsPayRollBreakDown(DateTime StartDate, DateTime EndDate, bool IsDateSearch,int nLocationID, Int64 nUserID);
        List<EmployeeSalary> GetsEmployeeSettleemtSalary(string sSQL, Int64 nUserID);
    }
    #endregion
}
