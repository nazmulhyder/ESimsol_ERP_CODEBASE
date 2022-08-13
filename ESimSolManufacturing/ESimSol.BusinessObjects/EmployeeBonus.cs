using System;
using System.IO;
using ICS.Core;
using ICS.Core.Framework;
using ICS.Core.Utility;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Collections.Generic;
using System.Globalization;

namespace ESimSol.BusinessObjects
{
    #region EmployeeBonus
    public class EmployeeBonus : BusinessObject
    {
        public EmployeeBonus()
        {
            EBID = 0;
            EmployeeID = 0;
            //LocationID=0;
            DepartmentID = 0;
            DesignationID = 0;
            //SalaryHeadID=0;
            BonusAmount = 0;
            ETIN = "";
            //Note="";
            //Year=0;
            //Month= EnumMonth.None;
            //ApproveBy = 0;
            //ApproveDate = DateTime.Now;
            ErrorMessage = "";
            EmployeeCode = "";
            EmployeeName = "";
            LocationName = "";
            DesignationName = "";
            DepartmentName = "";
            SalaryHeadName = "";
            JoiningDate = DateTime.Now;
            ConfirmationDate = DateTime.Now;
            //BonusDeclarationDate = DateTime.Now;
            EmployeeCategory = EnumEmployeeCategory.None;
            sIDs = "";
            ApproveByName = "";
            EmployeeTypeName = "";
            ESSID = 0;
            Stamp = 0;
            GrossAmount = 0;
            EBGrossAmount = 0;
            OthersAmount = 0;
            BankAccountNo = "";
            BankBranchName = "";
            BasicAmount = 0;
            BusinessUnitID = 0;
            BusinessUnitName = "";
            CompGrossAmount = 0.0;
            CompBasicAmount = 0.0;
            CompBonusAmount = 0.0;
            ManPower = 0;
            BlockName = "";
            BlockID = 0;
            CompEBGrossAmount = 0;
            EmployeeBankAccountID = 0;
            InPercent = 0;
            CalculateON = EnumPayrollApplyOn.None;
            CompInPercent = 0;
            CompCalculateON = EnumPayrollApplyOn.None;
            BonusCashAmount = 0;
            BonusBankAmount = 0;
        }

        #region Properties
        public double GrossAmount { get; set; }
        public int BlockID { get; set; }
        public EnumPayrollApplyOn CalculateON { get; set; }
        public EnumPayrollApplyOn CompCalculateON { get; set; }
        public int InPercent { get; set; }
        public int CompInPercent { get; set; }
        public int EmployeeBankAccountID { get; set; }
        public int EBID { get; set; }
        public int ManPower { get; set; }
        public double CompEBGrossAmount { get; set; }
        public double CompGrossAmount { get; set; }
        public double CompBasicAmount { get; set; }
        public double CompBonusAmount { get; set; }
        public int EmployeeID { get; set; }
        public int LocationID { get; set; }
        public int DepartmentID { get; set; }
        public int DesignationID { get; set; }
        public int SalaryHeadID { get; set; }
        public double BonusAmount { get; set; }
        public double BasicAmount { get; set; }
        public string Note { get; set; }
        public string BlockName { get; set; }
        public string BusinessUnitName { get; set; }
        public int Year { get; set; }
        public EnumMonth Month { get; set; }
        //public int ApproveBy  { get; set; }
        public DateTime ApproveDate { get; set; }
        public DateTime BonusDeclarationDate { get; set; }
        public string ErrorMessage { get; set; }
        public string sIDs { get; set; }
       public double BonusCashAmount{ get; set; }
       public double BonusBankAmount { get; set; }
       public string ETIN { get; set; }
        
        #endregion

        #region Derived Property
       public Company Company { get; set; }
        public string CalculateONSt
        {
            get
            {
                return this.CalculateON.ToString();
            }
        }
        public string CompCalculateONSt
        {
            get
            {
                return this.CompCalculateON.ToString();
            }
        }
        public string EBPID { get; set; }
        public string EmployeeName { get; set; }
        public string EmployeeCode { get; set; }
		public string LocationName { get; set; }
		public string DesignationName { get; set; }
		public string DepartmentName { get; set; }
		public string SalaryHeadName { get; set; }
        public string ApproveByName { get; set; }
		public DateTime  JoiningDate  { get; set; }
        public DateTime ConfirmationDate  { get; set; }
        public int BusinessUnitID  { get; set; }
        public EnumEmployeeCategory EmployeeCategory { get; set; }
        public List<EmployeeBonus> EmployeeBonuss { get; set; }
        public List<BusinessUnit> BusinessUnits { get; set; }

        public string EmployeeCategoryInString
        {
            get { return this.EmployeeCategory.ToString(); }
        }
        public string ConfirmationDateInString
        {
            get { return this.ConfirmationDate.ToString("dd MMM yyyy"); }
        }
        public string JoiningDateInString
        {
            get { return this.JoiningDate.ToString("dd MMM yyyy"); }
        }
        public string BonusDeclarationDateInString
        {
            get { return this.BonusDeclarationDate.ToString("dd MMM yyyy"); }
        }
        public int MonthInt { get; set; }
        public string MonthInString
        {
            get { return this.Month.ToString(); }
        }
        public string MonthInStringFull
        {
            get { return CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName((this.MonthInt > 0 ?this.MonthInt: 1)); }
        }
        public string ApproveDateInString
        {
            get { if (ApproveDate > Convert.ToDateTime("01 JAN 1900")) return this.ApproveDate.ToString("dd MMM yyyy"); else return ""; }
        }
        public string EmployeeTypeName { get; set; }

        public int ESSID { get; set; }
        public double Stamp { get; set; }
        public double EBGrossAmount { get; set; }
        public double OthersAmount { get; set; }
        public string BankAccountNo { get; set; }
        public string BankBranchName { get; set; }

        #endregion

        #region Functions
        public static List<EmployeeBonus> Process(int SalaryHeadID, int Month, int Year, string Purpose, DateTime UptoDate, long nUserID)
        {
            return EmployeeBonus.Service.Process(SalaryHeadID, Month,Year, Purpose, UptoDate, nUserID);
        }
        public static List<EmployeeBonus> Gets(string sSQL, long nUserID)
        {
            return EmployeeBonus.Service.Gets(sSQL, nUserID);
        }
        public static string Delete(string IDs, long nUserID)
        {
            return EmployeeBonus.Service.Delete(IDs, nUserID);
        }
        public static List<EmployeeBonus> Approve(string sParams, long nUserID)
        {
            return EmployeeBonus.Service.Approve(sParams, nUserID);
        }
        #endregion

        #region ServiceFactory
        internal static IEmployeeBonusService Service
        {
            get { return (IEmployeeBonusService)Services.Factory.CreateService(typeof(IEmployeeBonusService)); }
        }

        #endregion
    }
    #endregion

    #region IEmployeeBonus interface

    public interface IEmployeeBonusService
    {
        List<EmployeeBonus> Process(int SalaryHeadID, int Month, int Year, string Purpose, DateTime UptoDate, Int64 nUserID);
        string Delete(string IDs, Int64 nUserID);
        List<EmployeeBonus> Gets(string sSQL, Int64 nUserID);
        List<EmployeeBonus> Approve(string sParams, Int64 nUserID);
    }
    #endregion
}
