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
    #region RPTSalarySheet
    [DataContract]
    public class RPTSalarySheet : BusinessObject
    {
        public RPTSalarySheet()
        {
            EmployeeSalaryID = 0;
            EmployeeID = 0;
            LocationID = 0;
            DepartmentID = 0;
            DesignationID = 0;
            GrossAmount = 0;
            BMMID = 0;
            
            StartDate = DateTime.Now;
            EndDate = DateTime.Now;
            
            OTRatePerHour = 0;
            TotalAbsent = 0;
            TotalLate = 0;
            TotalEarlyLeaving = 0;
            TotalDayOff = 0;
            RevenueStemp = 0;
            ErrorMessage = "";
            EmployeeTypeName = "";
            Gender= "";
            CashAmount = 0;
            Params = "";

            BusinessUnitID = 0;
            BUName = "";
            BlockName = "";

            LateInMin = 0;

            Present = 0;
            TotalLeave = 0;
            OTAmount = 0;
            AccountNo = "";
            BankName = "";
            EmpNameInBangla = "";
            DsgNameInBangla = "";
            BUAddress = "";
            TotalDays = 0;
            EarlyInMin = 0;
            EWD = 0;
            LastGross = 0;
            BankAmount = 0;
            IncrementAmount = 0;
            EffectedDate = DateTime.MinValue;
            TotalPLeave = 0;
            TotalUpLeave = 0;
            TotalHoliday = 0;
            TotalDOff = 0;
            EmployeeCodeSL = 0;
            ContactNo = "";
            EmpGroup = "";
            PaymentType = "";
        }

        #region Properties
        public string ContactNo { get; set; }
        public DateTime EffectedDate { get; set; }
        public int EWD { get; set; }
        public int TotalHoliday { get; set; }
        public int TotalDOff { get; set; }
        public int BMMID { get; set; }
        public int TotalPLeave { get; set; }
        public int TotalUpLeave { get; set; }
        public int EarlyInMin { get; set; }
        public int TotalDays { get; set; }
        public int Present { get; set; }
        public int TotalLeave { get; set; }
        public double OTAmount { get; set; }
        public double IncrementAmount { get; set; }
        public double BankAmount { get; set; }
        public double LastGross { get; set; }
        public string AccountNo { get; set; }
        public string BankName { get; set; }
        public string BUAddress { get; set; }
        public string EmpNameInBangla { get; set; }
        public string DsgNameInBangla { get; set; }
        public int EmployeeSalaryID { get; set; }
        public int EmployeeID { get; set; }
        public int LocationID { get; set; }
        public int DepartmentID { get; set; }
        public int DesignationID { get; set; }
        public double GrossAmount { get; set; }
        public double NetAmount { get; set; }
        public DateTime SalaryDate { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public double OTHour { get; set; }
        public double OTRatePerHour { get; set; }
        public int TotalAbsent { get; set; }
        public int TotalLate { get; set; }
        public int TotalEarlyLeaving { get; set; }
        public int TotalDayOff { get; set; }
        public double RevenueStemp { get; set; }
        public string ErrorMessage { get; set; }
        public string EmployeeTypeName { get; set; }
        public string Gender { get; set; }
        public string EmpGroup { get; set; }
        public string PaymentType { get; set; }
        public string Params { get; set; }

        public int LateInMin { get; set; }
       
        #endregion

        #region Derived Property
        public Company Company { get; set; }
        public List<RPTSalarySheet> EmployeeSalarys { get; set; }
        public List<RPTSalarySheetDetail> EmployeeSalaryDetails { get; set; }
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
        public int EmployeeCodeSL { get; set; }
        public string LocationName { get; set; }
        public string DepartmentName { get; set; }
        public string ParentDepartmentName { get; set; }
        public string DesignationName { get; set; }
        public DateTime JoiningDate { get; set; }
        public DateTime DateOfConfirmation { get; set; }
        public double CashAmount { get; set; }
        public int BusinessUnitID { get; set; }
        public string BUName { get; set; }
        public string BlockName { get; set; }

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


        //public double EWD { get { return this.TotalPresent + this.TotalHoliday + this.TotalDayOff + this.TotalPLeave + this.TotalUpLeave; } }//Employee Working Day(EWD)
        public double PD { get { return this.Present + this.TotalDayOff + this.TotalPLeave; } }//Payment Day(PD)
        public double PDComp { get { return this.Present + this.TotalDayOff + this.TotalLeave; } }//Payment Day(PD)

        #region Payroll Break Down


        public string EffectedDateInStr
        {
            get
            {
                return (this.EffectedDate == DateTime.MinValue) ? "-" : this.EffectedDate.ToString("dd MMM yyyy");
            }
        }
       
        #endregion

        #endregion

        #region Functions
        public static RPTSalarySheet Get(int id, long nUserID)
        {
            return RPTSalarySheet.Service.Get(id, nUserID);
        }

        public static RPTSalarySheet Get(string sSQL, long nUserID)
        {
            return RPTSalarySheet.Service.Get(sSQL, nUserID);
        }

        public static List<RPTSalarySheet> Gets(long nUserID)
        {
            return RPTSalarySheet.Service.Gets(nUserID);
        }

        public static List<RPTSalarySheet> Gets(string sSQL, long nUserID)
        {
            return RPTSalarySheet.Service.Gets(sSQL, nUserID);
        }
        public static List<RPTSalarySheet> GetEmployeesSalary(string sBU, string sLocationID, string sDepartmentIDs, string sDesignationIDs, string sSalarySchemeIDs, string sBlockIDs, string sGroupIDs, string sEmpIDs, int nMonthID, int nYear, bool bNewJoin, int IsOutSheet, double nStartSalaryRange, double nEndSalaryRange, bool IsCompliance, int nPayType, bool IsMacthExact, int BankAccID)
        {
            return RPTSalarySheet.Service.GetEmployeesSalary(sBU, sLocationID, sDepartmentIDs, sDesignationIDs, sSalarySchemeIDs, sBlockIDs, sGroupIDs, sEmpIDs, nMonthID, nYear, bNewJoin, IsOutSheet, nStartSalaryRange, nEndSalaryRange, IsCompliance, nPayType, IsMacthExact, BankAccID);
        }
        #endregion

        #region ServiceFactory
        internal static IRPTSalarySheetService Service
        {
            get { return (IRPTSalarySheetService)Services.Factory.CreateService(typeof(IRPTSalarySheetService)); }
        }
        #endregion
    }
    #endregion

    #region IEmployeeSalary interface

    public interface IRPTSalarySheetService
    {
        RPTSalarySheet Get(int id, Int64 nUserID);

        RPTSalarySheet Get(string sSQL, Int64 nUserID);

        List<RPTSalarySheet> Gets(Int64 nUserID);
        List<RPTSalarySheet> Gets(string sSQL, Int64 nUserID);
        List<RPTSalarySheet> GetEmployeesSalary(string sBU, string sLocationID, string sDepartmentIDs, string sDesignationIDs, string sSalarySchemeIDs, string sBlockIDs, string sGroupIDs, string sEmpIDs, int nMonthID, int nYear, bool bNewJoin, int IsOutSheet, double nStartSalaryRange, double nEndSalaryRange, bool IsCompliance, int nPayType, bool IsMacthExact, int BankAccID);

    }
    #endregion
}

