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
    #region PayrollProcessManagement

    public class PayrollProcessManagement : BusinessObject
    {
        public PayrollProcessManagement()
        {
            PPMID = 0;
            CompanyID = 1;
            LocationID = 0;
            Status = EnumProcessStatus.Initialize;
            PaymentCycle = EnumPaymentCycle.None;
            ProcessDate = DateTime.Now;
            SalaryFrom = DateTime.Now;
            SalaryTo = DateTime.Now;
            ErrorMessage = "";
            AllowanceIDs = "";
            SalaryHeads = new List<SalaryHead>();
            SalarySchemes = new List<SalaryScheme>();
            EmployeeGroups = new List<EmployeeGroup>();
            DepartmentRequirementPolicys = new List<DepartmentRequirementPolicy>();
            SalaryCorrections = new List<SalaryCorrection>();
            MonthID = EnumMonth.None;
            BusinessUnitID = 0;
            BankAccountID = 0;
            DeptCount = 0;
            SchemeCount = 0;
            EncryptID = "";
            EmployeeCount = 0;

            //Compliance Payroll Process
            MOCID = 0;
        }

        #region Properties

        public int MOCID { get; set; }
        public int EmployeeCount { get; set; }
        public int PPMID { get; set; }
        public int DeptCount { get; set; }
        public int SchemeCount { get; set; }
        public int CompanyID { get; set; }
        public int LocationID { get; set; }
        public EnumProcessStatus Status { get; set; }
        public EnumPaymentCycle PaymentCycle { get; set; }
        public DateTime ProcessDate { get; set; }
        public DateTime SalaryFrom { get; set; }
        public DateTime SalaryTo { get; set; }
        public string ErrorMessage { get; set; }
        public string EncryptID { get; set; }
        public string AllowanceIDs { get; set; }
        public EnumMonth MonthID { get; set; }
        public int BusinessUnitID { get; set; }
        public int BankAccountID { get; set; }
        #endregion

        #region Derived Property
        public string CompanyNmae { get; set; }
        public string LocationName { get; set; }
        public string BUName { get; set; }
        public string CountString { get { return this.EmployeeCount.ToString(); ; } }
        public string Details
        {
            get
            {
                return "Department: " + this.DeptCount + ", Scheme: " + this.SchemeCount + ", Employee: " + this.EmployeeCount;
            }
        }
        public string ProcessDateInString
        {
            get
            {
                return this.ProcessDate.ToString("dd MMM yyyy");
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

        public string SalaryForInString
        {
            get
            {
                return this.SalaryFrom.ToString("dd MMM yyyy") + " To " + this.SalaryTo.ToString("dd MMM yyyy");
            }
        }

        public string SalaryMonthInString
        {
            get
            {
                return this.SalaryFrom.ToString(" MMM ") + " - " + this.SalaryTo.ToString(" MMM ") + "  " + this.SalaryTo.ToString("yyyy");
            }
        }

        public int PaymentCycleInt { get; set; }
        public string PaymentCycleInString
        {
            get
            {
                return PaymentCycle.ToString();
            }
        }

        public int StatusInt { get; set; }
        public string StatusInString
        {
            get
            {
                return Status.ToString();
            }
        }

        public int MonthIDInt { get; set; }
        public string MonthIDInString
        {
            get
            {
                return MonthID.ToString();
            }
        }

        public List<Location> Locations { get; set; }
        public List<DepartmentRequirementPolicy> DepartmentRequirementPolicys { get; set; }
        public List<SalaryScheme> SalarySchemes { get; set; }
        public List<SalaryHead> SalaryHeads { get; set; }
        public List<EmployeeGroup> EmployeeGroups { get; set; }
        public List<SalaryCorrection> SalaryCorrections { get; set; }
        #endregion

        #region Functions
        public static PayrollProcessManagement Get(int id, long nUserID)
        {
            return PayrollProcessManagement.Service.Get(id, nUserID);
        }

        public static int CheckPayrollProcess(string sSQL, long nUserID)
        {
            return PayrollProcessManagement.Service.CheckPayrollProcess(sSQL, nUserID);
        }
        public static List<PayrollProcessManagement> Gets(long nUserID)
        {
            return PayrollProcessManagement.Service.Gets(nUserID);
        }

        public static PayrollProcessManagement CheckPPM(int nEmployeeID, int nMonthID, int nYear, long nUserID)
        {
            return PayrollProcessManagement.Service.CheckPPM(nEmployeeID, nMonthID, nYear, nUserID);
        }
        public static List<PayrollProcessManagement> Gets(string sSQL, long nUserID)
        {
            return PayrollProcessManagement.Service.Gets(sSQL, nUserID);
        }

        public PayrollProcessManagement IUD(long nUserID)
        {
            return PayrollProcessManagement.Service.IUD(this, nUserID);
        }
        public PayrollProcessManagement IUD_V1(long nUserID)
        {
            return PayrollProcessManagement.Service.IUD_V1(this, nUserID);
        }
        public string ProcessPayroll(int nSalarySchemeID, int nLocationID, DateTime dStartDate, DateTime dEndDate, string sAllowanceIDs, int nPayRollProcessID, long nUserID)
        {
            return PayrollProcessManagement.Service.ProcessPayroll(nSalarySchemeID, nLocationID, dStartDate, dEndDate, sAllowanceIDs, nPayRollProcessID, nUserID);
        }

        //public string ProcessPayrollByEmployee(int nEmployeeID, int nLocationID, DateTime dStartDate, DateTime dEndDate, string sAllowanceIDs, int nPayRollProcessID, long nUserID)
        //{
        //    return (string)ICSWCFServiceClient.CallMethod(ServiceType, wcfSessionid, "ProcessPayrollByEmployee", nEmployeeID, nLocationID, dStartDate, dEndDate, sAllowanceIDs, nPayRollProcessID)[0];
        //}

        public int ProcessPayrollByEmployee(int nIndex, int nLocationID, DateTime dStartDate, DateTime dEndDate, string sAllowanceIDs, int nPayRollProcessID, long nUserID)
        {
            return PayrollProcessManagement.Service.ProcessPayrollByEmployee(nIndex, nLocationID, dStartDate, dEndDate, sAllowanceIDs, nPayRollProcessID, nUserID);
        }

        public PayrollProcessManagement Delete(int nPPMID, long nUserID)
        {
            return PayrollProcessManagement.Service.Delete(nPPMID, nUserID);

        }
        public int ProcessPayroll_Mamiya(int nIndex, int nPayRollProcessID, string sEmployeeIDs, long nUserID)
        {
            return PayrollProcessManagement.Service.ProcessPayroll_Mamiya(nIndex, nPayRollProcessID, sEmployeeIDs, nUserID);
        }
        public int ProcessPayroll_Corporate(int nIndex, int nPayRollProcessID, string sEmployeeIDs, long nUserID)
        {
            return PayrollProcessManagement.Service.ProcessPayroll_Corporate(nIndex, nPayRollProcessID, sEmployeeIDs, nUserID);
        }
        public int ProcessPayroll_Corporate_Discontinue(int nIndex, int nPayRollProcessID, string sEmployeeIDs, long nUserID)
        {
            return PayrollProcessManagement.Service.ProcessPayroll_Corporate_Discontinue(nIndex, nPayRollProcessID, sEmployeeIDs, nUserID);
        }
        public static EmployeeSalary SettlementSalaryProcess(int nEmployeeSettlementID, long nUserID)
        {
            return PayrollProcessManagement.Service.SettlementSalaryProcess(nEmployeeSettlementID, nUserID);
        }
        public static EmployeeSalary SettlementSalaryProcess_Corporate(int nEmployeeSettlementID, long nUserID)
        {
            return PayrollProcessManagement.Service.SettlementSalaryProcess_Corporate(nEmployeeSettlementID, nUserID);
        }

        public static List<EmployeeSalary> SettlementSalaryProcess_Corporate_Multiple(EmployeeSettlement oEmployeeSettlement, long nUserID)
        {
            return PayrollProcessManagement.Service.SettlementSalaryProcess_Corporate_Multiple(oEmployeeSettlement, nUserID);
        }
        public int ProcessPayroll_Production(int nIndex, int nPayRollProcessID, string sEmployeeIDs, long nUserID)
        {
            return PayrollProcessManagement.Service.ProcessPayroll_Production(nIndex, nPayRollProcessID, sEmployeeIDs, nUserID);
        }
        public static PayrollProcessManagement PPM_Unfreeze(int id, long nUserID)
        {
            return PayrollProcessManagement.Service.PPM_Unfreeze(id, nUserID);
        }


        #region Compliance Salary
        public static PayrollProcessManagement GetComp(int id, long nUserID)
        {
            return PayrollProcessManagement.Service.GetComp(id, nUserID);
        }

        public static int CheckPayrollProcessComp(string sSQL, long nUserID)
        {
            return PayrollProcessManagement.Service.CheckPayrollProcessComp(sSQL, nUserID);
        }
        public static List<PayrollProcessManagement> GetsComp(long nUserID)
        {
            return PayrollProcessManagement.Service.GetsComp(nUserID);
        }

        public static PayrollProcessManagement CheckPPMComp(int nEmployeeID, int nMonthID, int nYear, long nUserID)
        {
            return PayrollProcessManagement.Service.CheckPPMComp(nEmployeeID, nMonthID, nYear, nUserID);
        }
        public static List<PayrollProcessManagement> GetsComp(string sSQL, long nUserID)
        {
            return PayrollProcessManagement.Service.GetsComp(sSQL, nUserID);
        }

        public PayrollProcessManagement IUDComp(long nUserID)
        {
            return PayrollProcessManagement.Service.IUDComp(this, nUserID);
        }
        public PayrollProcessManagement IUD_V1Comp(long nUserID)
        {
            return PayrollProcessManagement.Service.IUD_V1Comp(this, nUserID);
        }

        public string ProcessPayrollComp(int nSalarySchemeID, int nLocationID, DateTime dStartDate, DateTime dEndDate, string sAllowanceIDs, int nPayRollProcessID, long nUserID)
        {
            return PayrollProcessManagement.Service.ProcessPayrollComp(nSalarySchemeID, nLocationID, dStartDate, dEndDate, sAllowanceIDs, nPayRollProcessID, nUserID);
        }

        //public string ProcessPayrollByEmployee(int nEmployeeID, int nLocationID, DateTime dStartDate, DateTime dEndDate, string sAllowanceIDs, int nPayRollProcessID, long nUserID)
        //{
        //    return (string)ICSWCFServiceClient.CallMethod(ServiceType, wcfSessionid, "ProcessPayrollByEmployee", nEmployeeID, nLocationID, dStartDate, dEndDate, sAllowanceIDs, nPayRollProcessID)[0];
        //}

        public int ProcessPayrollByEmployeeComp(int nIndex, int nLocationID, DateTime dStartDate, DateTime dEndDate, string sAllowanceIDs, int nPayRollProcessID, long nUserID)
        {
            return PayrollProcessManagement.Service.ProcessPayrollByEmployeeComp(nIndex, nLocationID, dStartDate, dEndDate, sAllowanceIDs, nPayRollProcessID, nUserID);
        }

        public PayrollProcessManagement DeleteComp(int nPPMID, long nUserID)
        {
            return PayrollProcessManagement.Service.DeleteComp(nPPMID, nUserID);

        }
        public int ProcessPayroll_MamiyaComp(int nIndex, int nPayRollProcessID, string sEmployeeIDs, long nUserID)
        {
            return PayrollProcessManagement.Service.ProcessPayroll_MamiyaComp(nIndex, nPayRollProcessID, sEmployeeIDs, nUserID);
        }
        public int ProcessPayroll_CorporateComp(int nIndex, int nPayRollProcessID, string sEmployeeIDs, long nUserID)
        {
            return PayrollProcessManagement.Service.ProcessPayroll_CorporateComp(nIndex, nPayRollProcessID, sEmployeeIDs, nUserID);
        }
        public int ProcessPayroll_Corporate_DiscontinueComp(int nIndex, int nPayRollProcessID, string sEmployeeIDs, long nUserID)
        {
            return PayrollProcessManagement.Service.ProcessPayroll_Corporate_DiscontinueComp(nIndex, nPayRollProcessID, sEmployeeIDs, nUserID);
        }
        public static EmployeeSalary SettlementSalaryProcessComp(int nEmployeeSettlementID, long nUserID)
        {
            return PayrollProcessManagement.Service.SettlementSalaryProcessComp(nEmployeeSettlementID, nUserID);
        }
        public static EmployeeSalary SettlementSalaryProcess_CorporateComp(int nEmployeeSettlementID, long nUserID)
        {
            return PayrollProcessManagement.Service.SettlementSalaryProcess_CorporateComp(nEmployeeSettlementID, nUserID);
        }

        public static List<EmployeeSalary> SettlementSalaryProcess_Corporate_MultipleComp(EmployeeSettlement oEmployeeSettlement, long nUserID)
        {
            return PayrollProcessManagement.Service.SettlementSalaryProcess_Corporate_MultipleComp(oEmployeeSettlement, nUserID);
        }
        public int ProcessPayroll_ProductionComp(int nIndex, int nPayRollProcessID, string sEmployeeIDs, long nUserID)
        {
            return PayrollProcessManagement.Service.ProcessPayroll_ProductionComp(nIndex, nPayRollProcessID, sEmployeeIDs, nUserID);
        }
        public static PayrollProcessManagement PPM_UnfreezeComp(int id, long nUserID)
        {
            return PayrollProcessManagement.Service.PPM_UnfreezeComp(id, nUserID);
        }
        #endregion

        #endregion

        #region ServiceFactory
        internal static IPayrollProcessManagementService Service
        {
            get { return (IPayrollProcessManagementService)Services.Factory.CreateService(typeof(IPayrollProcessManagementService)); }
        }

        #endregion
    }
    #endregion

    #region IPayrollProcessManagement interface

    public interface IPayrollProcessManagementService
    {
        PayrollProcessManagement CheckPPM(int nEmployeeID, int nMonthID, int nYear, Int64 nUserID);
       
        int CheckPayrollProcess(string sSQL, Int64 nUserID);
        PayrollProcessManagement Get(int id, Int64 nUserID);
        PayrollProcessManagement PPM_Unfreeze(int id, Int64 nUserID);
        List<PayrollProcessManagement> Gets(Int64 nUserID);
        List<PayrollProcessManagement> Gets(string sSQL, Int64 nUserID);
        PayrollProcessManagement IUD(PayrollProcessManagement oPayrollProcessManagement, Int64 nUserID);
        PayrollProcessManagement IUD_V1(PayrollProcessManagement oPayrollProcessManagement, Int64 nUserID);
        string ProcessPayroll(int nSalarySchemeID, int nLocationID, DateTime dStartDate, DateTime dEndDate, string sAllowanceIDs, int nPayRollProcessID, Int64 nUserID);
        int ProcessPayrollByEmployee(int nIndex, int nLocationID, DateTime dStartDate, DateTime dEndDate, string sAllowanceIDs, int nPayRollProcessID, Int64 nUserID);
        PayrollProcessManagement Delete(int nPPMID, Int64 nUserID);

        int ProcessPayroll_Mamiya(int nIndex, int nPayRollProcessID, string sEmployeeIDs, long nUserID);
        int ProcessPayroll_Corporate(int nIndex, int nPayRollProcessID, string sEmployeeIDs, long nUserID);
        int ProcessPayroll_Corporate_Discontinue(int nIndex, int nPayRollProcessID, string sEmployeeIDs, long nUserID);
        int ProcessPayroll_Production(int nIndex, int nPayRollProcessID, string sEmployeeIDs, long nUserID);
        EmployeeSalary SettlementSalaryProcess(int nEmployeeSettlementID, Int64 nUserID);
        EmployeeSalary SettlementSalaryProcess_Corporate(int nEmployeeSettlementID, Int64 nUserID);
        List<EmployeeSalary> SettlementSalaryProcess_Corporate_Multiple(EmployeeSettlement oEmployeeSettlement, Int64 nUserID);

        #region Compliance Salary
        PayrollProcessManagement CheckPPMComp(int nEmployeeID, int nMonthID, int nYear, Int64 nUserID);

        int CheckPayrollProcessComp(string sSQL, Int64 nUserID);
        PayrollProcessManagement GetComp(int id, Int64 nUserID);
        PayrollProcessManagement PPM_UnfreezeComp(int id, Int64 nUserID);
        List<PayrollProcessManagement> GetsComp(Int64 nUserID);
        List<PayrollProcessManagement> GetsComp(string sSQL, Int64 nUserID);
        PayrollProcessManagement IUDComp(PayrollProcessManagement oPayrollProcessManagement, Int64 nUserID);
        PayrollProcessManagement IUD_V1Comp(PayrollProcessManagement oPayrollProcessManagement, Int64 nUserID);
        string ProcessPayrollComp(int nSalarySchemeID, int nLocationID, DateTime dStartDate, DateTime dEndDate, string sAllowanceIDs, int nPayRollProcessID, Int64 nUserID);
        int ProcessPayrollByEmployeeComp(int nIndex, int nLocationID, DateTime dStartDate, DateTime dEndDate, string sAllowanceIDs, int nPayRollProcessID, Int64 nUserID);
        PayrollProcessManagement DeleteComp(int nPPMID, Int64 nUserID);

        int ProcessPayroll_MamiyaComp(int nIndex, int nPayRollProcessID, string sEmployeeIDs, long nUserID);
        int ProcessPayroll_CorporateComp(int nIndex, int nPayRollProcessID, string sEmployeeIDs, long nUserID);
        int ProcessPayroll_Corporate_DiscontinueComp(int nIndex, int nPayRollProcessID, string sEmployeeIDs, long nUserID);
        int ProcessPayroll_ProductionComp(int nIndex, int nPayRollProcessID, string sEmployeeIDs, long nUserID);
        EmployeeSalary SettlementSalaryProcessComp(int nEmployeeSettlementID, Int64 nUserID);
        EmployeeSalary SettlementSalaryProcess_CorporateComp(int nEmployeeSettlementID, Int64 nUserID);
        List<EmployeeSalary> SettlementSalaryProcess_Corporate_MultipleComp(EmployeeSettlement oEmployeeSettlement, Int64 nUserID);
        #endregion
    }
    #endregion
}
