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
    #region EmployeeSalaryStructure
    [DataContract]
    public class EmployeeSalaryStructure : BusinessObject
    {
        public EmployeeSalaryStructure()
        {
            ESSID = 0;
            EmployeeID = 0;
            SSGradeID = 0;
            SalarySchemeID = 0;
            Description = "";
            GrossAmount = 0;
            IsIncludeFixedItem = true;
            ActualGrossAmount = 0;
            CurrencyID = 0;
            IsActive = true;
            FatherName = "";
            IsAllowBankAccount = false;
            IsFullBonus = false;
            IsAllowOverTime = false;
            IsAttendanceDependent = true;
            StartDay = 0;
            ErrorMessage = "";
            CurrencySymbol = "";
            EmployeeSalaryStructureDetails = new List<EmployeeSalaryStructureDetail>();
            NatureOfEmployee = EnumEmployeeNature.None;
            IDs = "";
            CashAmount = 0;
            DateOfBirth = DateTime.Now;
            DateOfJoin = DateTime.Now;
            Gender = "";
            Religion = "";
            LocationName = "";
            ESSHistoryID = 0;
            CompGrossAmount = 0;
            IsCashFixed = false;
            BonusCashAmount = 0;
        }

        #region Properties
        public int ESSID { get; set; }
        public int EmployeeID { get; set; }
        public int SSGradeID { get; set; }
        public int SalarySchemeID { get; set; }
        public string Description { get; set; }
        public double GrossAmount { get; set; }
        public bool IsIncludeFixedItem { get; set; }
        public double ActualGrossAmount { get; set; }
        public string FatherName { get; set; }
        public int CurrencyID { get; set; }
        public bool IsActive { get; set; }
        public bool IsCashFixed { get; set; }
        public bool IsAllowBankAccount { get; set; }
        public bool IsFullBonus { get; set; }
        public bool IsAllowOverTime { get; set; }
        public bool IsAttendanceDependent { get; set; }
        public int StartDay { get; set; }
        public string ErrorMessage { get; set; }
        public double CashAmount { get; set; }
        public double BonusCashAmount { get; set; }
        public double CompGrossAmount { get; set; }
        #endregion

        #region Derived Property
        public string Activity { get { if (IsActive)return "Active"; else return "Inactive"; } }
        public string SSGradeName { get; set; }
        public string SalarySchemeName { get; set; }
        public string EmployeeName { get; set; }
        public string EmployeeCode { get; set; }
        public string LocationName { get; set; }
        public string DepartmentName { get; set; }
        public string DesignationName { get; set; }
        public string EmployeeTypeName { get; set; }
        public EnumPaymentCycle PaymentCycle { get; set; }
        public List<EmployeeSalaryStructureDetail> EmployeeSalaryStructureDetails { get; set; }
        public EnumEmployeeNature NatureOfEmployee { get; set; }
        public int NatureOfEmployeeInt { get; set; }
        public string NatureOfEmployeeInString
        {
            get
            {
                return NatureOfEmployee.ToString();
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
        public DateTime DateOfJoin { get; set; }
        public string DateOfJoinInString { get { return this.DateOfJoin.ToString("dd MMM yyyy"); } }
        public string CurrencySymbol { get; set; }

        public int PaymentCycleInt { get; set; }
        public string PaymentCycleInString
        {
            get
            {
                return PaymentCycle.ToString();
            }
        }

        public EnumOverTimeON OverTimeON { get; set; }
        public string OverTimeONInString
        {
            get
            {
                return OverTimeON.ToString();
            }
        }

        public double DividedBy { get; set; }

        public double MultiplicationBy { get; set; }

        public bool IsProductionBase { get; set; }
        
        public int LateCount { get; set; }
        
        public int EarlyLeavingCount { get; set; }

        public string IDs { get; set; }

        public string Gender { get; set; }

        public string Religion { get; set; }

        public int ESSHistoryID { get; set; }
        #endregion

        #region Functions
        public static EmployeeSalaryStructure Get(int id, long nUserID)
        {
            return EmployeeSalaryStructure.Service.Get(id, nUserID);
        }
        public static EmployeeSalaryStructure GetByEmp(int nEmployeeID, long nUserID)
        {
            return EmployeeSalaryStructure.Service.GetByEmp(nEmployeeID, nUserID);
        }
        public static EmployeeSalaryStructure Get(string sSQL, long nUserID)
        {
            return EmployeeSalaryStructure.Service.Get(sSQL, nUserID);
        }
        public static List<EmployeeSalaryStructure> HistoryGets(string sSQL, long nUserID)
        {
            return EmployeeSalaryStructure.Service.HistoryGets(sSQL, nUserID);
        }
        public static List<EmployeeSalaryStructure> Gets(long nUserID)
        {
            return EmployeeSalaryStructure.Service.Gets(nUserID);
        }

        public static List<EmployeeSalaryStructure> Gets(string sSQL, long nUserID)
        {
            return EmployeeSalaryStructure.Service.Gets(sSQL, nUserID);
        }

        public EmployeeSalaryStructure IUD(int nDBOperation, long nUserID)
        {
            return EmployeeSalaryStructure.Service.IUD(this, nDBOperation, nUserID);
        }
        public static EmployeeSalaryStructure Activite(int EmpID, int nId, bool Active, long nUserID)
        {
            return EmployeeSalaryStructure.Service.Activite(EmpID, nId, Active, nUserID);
        }

        public static List<EmployeeSalaryStructure> CopyEmployeeSalaryStructure(int nCopyFromESSID, List<Employee> oEmployees, long nUserID)
        {
            return EmployeeSalaryStructure.Service.CopyEmployeeSalaryStructure(nCopyFromESSID, oEmployees, nUserID);
        }

        public List<EmployeeSalaryStructure> MultipleIncrement(string sParams, long nUserID)
        {
            return EmployeeSalaryStructure.Service.MultipleIncrement(sParams, nUserID);
        }
        public EmployeeSalaryStructure EditBankCash(long nUserID)
        {
            return EmployeeSalaryStructure.Service.EditBankCash(this, nUserID);
        }
        #endregion

        #region ServiceFactory
        internal static IEmployeeSalaryStructureService Service
        {
            get { return (IEmployeeSalaryStructureService)Services.Factory.CreateService(typeof(IEmployeeSalaryStructureService)); }
        }
        #endregion
    }
    #endregion

    #region IEmployeeSalaryStructure interface
    
    public interface IEmployeeSalaryStructureService
    {
        EmployeeSalaryStructure EditBankCash(EmployeeSalaryStructure oEmployeeSalaryStructure, Int64 nUserID);
        EmployeeSalaryStructure Get(int id, Int64 nUserID);
        EmployeeSalaryStructure GetByEmp(int nEmployeeID, Int64 nUserID);        
        EmployeeSalaryStructure Get(string sSQL, Int64 nUserID);        
        List<EmployeeSalaryStructure> Gets(Int64 nUserID);        
        List<EmployeeSalaryStructure> Gets(string sSQL, Int64 nUserID);
        List<EmployeeSalaryStructure> HistoryGets(string sSQL, Int64 nUserID);
        EmployeeSalaryStructure IUD(EmployeeSalaryStructure oEmployeeSalaryStructure, int nDBOperation, Int64 nUserID);        
        EmployeeSalaryStructure Activite(int EmpID, int nId, bool Active, Int64 nUserID);        
        List<EmployeeSalaryStructure> CopyEmployeeSalaryStructure(int nCopyFromESSID, List<Employee> oEmployees, Int64 nUserID);
        List<EmployeeSalaryStructure> MultipleIncrement(string sParams, Int64 nUserID);

    }
    #endregion
}
