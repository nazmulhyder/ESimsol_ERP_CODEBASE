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
    #region EmployeeSalary_MAMIYA
    [DataContract]
    public class EmployeeSalary_MAMIYA : BusinessObject
    {
        public EmployeeSalary_MAMIYA()
        {
            EmployeeSalaryID=0;
            EmployeeID =0;
            EmployeeCode ="";
            EmployeeName="";
            DepartmentName = "";
            DesignationName="";
	        DateOfJoin =DateTime.Now;
            Basic=0;
            HRent =0;
            Med=0;
            Conveyance = 0;
            GrossSalary =0;
	        AbsentHr_Sick =0;
            AbsentHr_WOPay =0;
            SLDeduction = 0;
            TotalAbsentAmount =0;
            LoanBalance = 0;
            EarnedPay =0;
	        ShiftAmount =0;
            OT_NHR =0;
            OT_HHR =0;
            FHOT = 0;
            OTAmount =0;
            AttendanceBonus =0;
            ADJCR =0;
            Gratuity = 0;
            OtherAll = 0;
            FB = 0;
	        GrossPay =0;
            PF =0;
            MobileBill = 0;
            PFProfit = 0;
            TRNS =0;
            DORM =0;
            ADV=0 ;
            ADJDR =0;
            RS =0;
	        PLoan =0;
            IncomeTax = 0;
            DeductionTotal =0;
            Fracretained =0;
            NetPay =0;
            HolidayAll = 0;
            ErrorMessage = "";

            // FOR PAY SLIP
            LocationName ="";
            TotalPresent =0;
            TotalAbsent =0;
            TotalDayOff =0;
            TotalLeave = 0;
            TotalPLeave =0;
            TotalUpLeave = 0;
            EarningsTotal =0;
            BankName ="";
            BankAccountNo = "";
            CasualLeave = 0;
            EarnLeave = 0;
            InterestAmt = 0;
            InstallmentAmt = 0;
            FriDay = 0;
            HoliDay = 0;
            ShiftAllDay = 0;
           
            //for salary summery
             DepartmentID = 0;
             NoOfEmp = 0;

            // for settlement of resignation
             DateOfResigEffect = DateTime.Now;
             DateOfConfirmation = DateTime.Now;
             DateOfBirth = DateTime.Now;
             TotalNoWork = 0;
             SettlementType = EnumSettleMentType.None;
             SalaryStartDate = DateTime.Now;
             SalaryEndDate = DateTime.Now;
             TotalNoWorkDayAllowance = 0;
             ELBalance = 0;
             ELAmount = 0;
             NoticePayAddition = 0;
             NoticePayDeduction = 0;
             EmployeeTypeID = 0;

            //for detail list
             StructureBasic = 0;
             StructureHR = 0;
             StructureMedical = 0;
             StructureGross = 0;
             RS = 0;
             Allowance = 0.0;
             EmpCategory = EnumEmployeeCategory.None; 
             LastMonthLoanBalance = 0.0;
             NightAllowDays = 0;
             TotalWorkingDay = 0;
             LastMonthPFSub = 0.0;
             NightAllow = 0.0;
             SickLeaveDeduction = 0.0;
             ConfirmationDate = DateTime.Now;
        }

        #region Properties
        public EnumEmployeeCategory EmpCategory { get; set; }
        public double LastMonthLoanBalance { get; set; }
        public int NightAllowDays { get; set; }
        public int TotalWorkingDay { get; set; }
        public double SickLeaveDeduction { get; set; }
        public double LastMonthPFSub { get; set; }
        public double NightAllow { get; set; }
        public DateTime ConfirmationDate { get; set; }
        public int EmployeeSalaryID { get; set; }
        public int EmployeeID { get; set; }
        public string EmployeeCode { get; set; }
        public string EmployeeName { get; set; }
        public string DepartmentName { get; set; }
        public string DesignationName { get; set; }
        public DateTime DateOfJoin { get; set; }
        public double Basic { get; set; }
        public double Allowance { get; set; }
        public double HRent { get; set; }
        public double Med { get; set; }
        public double Conveyance { get; set; }
        public double GrossSalary { get; set; }
        public double AbsentHr_Sick { get; set; }
        public double AbsentHr_WOPay { get; set; }
        public double SLDeduction { get; set; }
        public double TotalAbsentAmount { get; set; }
        public double LoanBalance { get; set; }
        public double EarnedPay { get; set; }
        public double ShiftAmount { get; set; }
        public double OT_NHR { get; set; }
        public double OT_HHR { get; set; }
        public double FHOT { get; set; }
        public double OT_NHR_AMT { get; set; }
        public double OT_HHR_AMT { get; set; }
        public double OTAmount { get; set; }
        public double AttendanceBonus { get; set; }
        public double ADJCR { get; set; }
        public double Gratuity { get; set; }
        public double OtherAll { get; set; }
        public double FB { get; set; }
        public double GrossPay { get; set; }
        public double PF { get; set; }
        public double MobileBill { get; set; }
        public double PFProfit { get; set; }
        public double TRNS { get; set; }
        public double DORM { get; set; }
        public double ADV { get; set; }
        public double ADJDR { get; set; }
        public double RS { get; set; }
        public double PLoan { get; set; }
        public double DeductionTotal { get; set; }
        public double Fracretained { get; set; }
        public double NetPay { get; set; }
        public double HolidayAll { get; set; }
        public int EmployeeTypeID { get; set; }
        public string ErrorMessage { get; set; }

        // FOR PAYSLIP
        public string LocationName { get; set; }
        public int TotalPresent { get; set; }
        public int TotalAbsent { get; set; }
        public int TotalDayOff { get; set; }
        public int TotalLeave { get; set; }
        public int TotalPLeave { get; set; }
        public int TotalUpLeave { get; set; }
        public double EarningsTotal { get; set; }
        public string BankName { get; set; }
        public string BankAccountNo { get; set; }
        public double CasualLeave { get; set; }
        public double EarnLeave { get; set; }
        public double ELBalance { get; set; }
        public double ELAmount { get; set; }
        public double InterestAmt { get; set; }
        public double InstallmentAmt { get; set; }
        public int FriDay { get; set; }
        public int HoliDay { get; set; }
        public int ShiftAllDay { get; set; }
        public double IncomeTax { get; set; }

        public double NoticePayAddition { get; set; }
        public double NoticePayDeduction { get; set; }

        public int DepartmentID { get; set; }
        public int NoOfEmp { get; set; }

        //for detail list
        public double StructureBasic { get; set; }
        public double StructureHR { get; set; }
        public double StructureMedical { get; set; }
        public double StructureGross { get; set; }

        public string EmployeeCategoryInString
        {
            get
            {
                return EnumObject.jGet(EmpCategory);
            }
        }
        public double OT_HHR_HR
        {
            get
            {
                return this.OT_HHR / 60;
            }
        }

        public double OT_NHR_HR
        {
            get
            {
                return this.OT_NHR / 60;
            }
        }

        public double FHOT_HR
        {
            get
            {
                return this.FHOT / 60;
            }
        }

        public string DateOfJoinInString
        {
            get
            {
                return DateOfJoin.ToString("dd MMM yyyy");
            }
        }
        public DateTime DateOfResigEffect { get; set; }
        public string DateOfResigInString
        {
            get
            {
                return DateOfResigEffect.ToString("dd MMM yyyy");
            }
        }
        public string ConfirmationDateInString
        {
            get
            {
                return ConfirmationDate.ToString("dd MMM yyyy");
            }
        }

        public DateTime DateOfConfirmation { get; set; }
        public string DateOfConfirmationInString
        {
            get
            {
                return DateOfConfirmation.ToString("dd MMM yyyy");
            }
        }

        public DateTime DateOfBirth { get; set; }
        public string DateOfBirthInString
        {
            get
            {
                return DateOfBirth.ToString("dd MMM yyyy");
            }
        }
        public EnumSettleMentType SettlementType { get; set; }
        public string SettlementTypeInString
        {
            get
            {
                return SettlementType.ToString();
            }
        }
        public DateTime SalaryStartDate { get; set; }
        public string SalaryStartDateInString
        {
            get
            {
                return SalaryStartDate.ToString("dd MMM yyyy");
            }
        }
        public DateTime SalaryEndDate { get; set; }
        public string SalaryEndDateInString
        {
            get
            {
                return SalaryEndDate.ToString("dd MMM yyyy");
            }
        }
        public bool IsActive { get; set; }
        public string EmployeeWorkingStatus
        {
            get
            {
                return this.IsActive ? "Continued" : "Discontinued";
            }
        }
        public int TotalNoWork { get; set; }
        public double TotalNoWorkDayAllowance { get; set; }
        public Company Company { get; set; }
        public List<EmployeeSalary_MAMIYA> EmployeeSalary_MAMIYAs { get; set; }
        public List<EmployeeSalaryStructureDetail> EmployeeSalaryStructureDetails { get; set; }
        public List<EmployeeSalaryStructure> EmployeeSalaryStructures { get; set; }
        public List<EmployeeSettlement> EmployeeSettlements { get; set; }
        #endregion

        #endregion

        #region Functions
        public static List<EmployeeSalary_MAMIYA> Gets_MAMIYA(DateTime dtDateFrom, DateTime dtDateTo,string sEmpIDs,int nLocationID, string sDepartmentIds, string sDesignationIDs, string sSalarySchemeIDs, int nPayType, long nUserID)
        {
            return EmployeeSalary_MAMIYA.Service.Gets_MAMIYA(dtDateFrom, dtDateTo, sEmpIDs, nLocationID, sDepartmentIds, sDesignationIDs, sSalarySchemeIDs,nPayType, nUserID);
        }
        public static List<EmployeeSalary_MAMIYA> Gets_PaySlip(DateTime dtDateFrom, DateTime dtDateTo, string sEmpIDs, int nLocationID, string sDepartmentIds, string sDesignationIDs, string sSalarySchemeIDs,int nPayType, long nUserID)
        {
            return EmployeeSalary_MAMIYA.Service.Gets_PaySlip(dtDateFrom, dtDateTo, sEmpIDs, nLocationID, sDepartmentIds, sDesignationIDs, sSalarySchemeIDs,nPayType, nUserID);
        }
        public static List<EmployeeSalary_MAMIYA> Gets_SalarySummery_MAMIYA(DateTime dtDateFrom, DateTime dtDateTo, string sEmpIDs, int nLocationID, string sDepartmentIds, string sDesignationIDs, string sSalarySchemeIDs,int nPayType, long nUserID)
        {
            return EmployeeSalary_MAMIYA.Service.Gets_SalarySummery_MAMIYA(dtDateFrom, dtDateTo, sEmpIDs, nLocationID, sDepartmentIds, sDesignationIDs, sSalarySchemeIDs, nPayType, nUserID);
        }
        public static List<EmployeeSalary_MAMIYA> Gets_FinalSettlementOfResig(int nEmployeeSettlementID, long nUserID)
        {
            return EmployeeSalary_MAMIYA.Service.Gets_FinalSettlementOfResig(nEmployeeSettlementID, nUserID);
        }
        public static List<EmployeeSalary_MAMIYA> Gets_OTSheet(DateTime dtStartDate, DateTime dtEndDate, long nUserID)
        {
            return EmployeeSalary_MAMIYA.Service.Gets_OTSheet(dtStartDate, dtEndDate, nUserID);
        }
        public static List<EmployeeSalary_MAMIYA> GetsSettlementDetailList(string sParam, long nUserID, int FinancialUserType)
        {
            return EmployeeSalary_MAMIYA.Service.GetsSettlementDetailList(sParam, nUserID, FinancialUserType);
        }
        #endregion

        #region ServiceFactory
        internal static IEmployeeSalary_MAMIYAService Service
        {
            get { return (IEmployeeSalary_MAMIYAService)Services.Factory.CreateService(typeof(IEmployeeSalary_MAMIYAService)); }
        }
        #endregion
    }

    #region IEmployeeSalary_MAMIYA interface

    public interface IEmployeeSalary_MAMIYAService
    {
        List<EmployeeSalary_MAMIYA> Gets_MAMIYA(DateTime dtDateFrom, DateTime dtDateTo, string sEmpIDs, int nLocationID, string sDepartmentIds, string sDesignationIDs, string sSalarySchemeIDs, int nPayType, long nUserID);
        List<EmployeeSalary_MAMIYA> Gets_PaySlip(DateTime dtDateFrom, DateTime dtDateTo, string sEmpIDs, int nLocationID, string sDepartmentIds, string sDesignationIDs, string sSalarySchemeIDs,int nPayType, long nUserID);
        List<EmployeeSalary_MAMIYA> Gets_SalarySummery_MAMIYA(DateTime dtDateFrom, DateTime dtDateTo, string sEmpIDs, int nLocationID, string sDepartmentIds, string sDesignationIDs, string sSalarySchemeIDs,int nPayType, long nUserID);
        List<EmployeeSalary_MAMIYA> Gets_FinalSettlementOfResig(int nEmployeeSettlementID, long nUserID);
        List<EmployeeSalary_MAMIYA> Gets_OTSheet(DateTime dtStartDate, DateTime dtEndDate, long nUserID);
        List<EmployeeSalary_MAMIYA> GetsSettlementDetailList(string sParam, long nUserID, int FinancialUserType);
    }
    #endregion
}
