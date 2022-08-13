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
    #region ITaxLedger

    public class ITaxLedger : BusinessObject
    {
        public ITaxLedger()
        {

            ITaxLedgerID = 0;
            ITaxRateSchemeID = 0;
            EmployeeID = 0;
            TaxBySalaryAmount = 0;
            RebateAmount = 0;
            AdvancePaidAmount = 0;
            PaidByPreviousLedger = 0;
            InstallmentAmount = 0;
            AlreadyPaidBySalary = 0;
            InActiveDate = DateTime.Now;
            ErrorMessage = "";
            TaxableAmount = 0;
            AssessmentYear = "";
            EmployeeNameCode = "";
            TotalTax = 0;
            IsActive = 0;
            DesignationName = "";
        }

        #region Properties

        public int ITaxLedgerID { get; set; }
        public int ITaxRateSchemeID { get; set; }
        public int EmployeeID { get; set; }
        public double TaxBySalaryAmount { get; set; }
        public double RebateAmount { get; set; }
        public double AdvancePaidAmount { get; set; }
        public double PaidByPreviousLedger { get; set; }
        public double InstallmentAmount { get; set; }
        public double AlreadyPaidBySalary { get; set; }
        public DateTime InActiveDate { get; set; }
        public string ErrorMessage { get; set; }
        public int IsActive { get; set; }
        #endregion

        #region Derived Property
        public double TaxableAmount { get; set; }
        public double TotalTax { get; set; }
        public string AssessmentYear { get; set; }
        public string EmployeeNameCode { get; set; }
        public string DepartmentName { get; set; }
        public string DesignationName { get; set; }
        public List<ITaxAssessmentYear> ITaxAssessmentYears { get; set; }
        public ITaxRateScheme ITaxRateScheme { get; set; }
        public EmployeeTINInformation EmployeeTINInformation { get; set; }
        public ITaxAssessmentYear ITaxAssessmentYear { get; set; }
        public List<ITaxLedgerSalaryHead> ITaxLedgerSalaryHeads { get; set; }
        public List<EmployeeSalaryDetail> EmployeeSalaryDetails { get; set; }
        public List<EmployeeSalary> EmployeeSalarys { get; set; }
        public List<SalaryHead> SalaryHeads { get; set; }
        public List<ITaxRateSlab> ITaxRateSlabs { get; set; }
        public List<ITaxRebatePayment> ITaxRebatePayments { get; set; }
        public List<ITaxRebateItem> ITaxRebateItems { get; set; }
        public Employee Employee { get; set; }
        public List<EmployeeBankAccount> EmployeeBankAccounts { get; set; }
        public List<ITaxHeadConfiguration> ITaxHeadConfigurations { get; set; }
        public List<ITaxRebateScheme> ITaxRebateSchemes { get; set; }
        public List<EmployeeSalaryStructureDetail> EmployeeSalaryStructureDetails { get; set; }
        
        public Company Company { get; set; }
        public double TaxAmount
        {
            get
            {
                if (this.TotalTax==0)
                {
                    return this.TaxBySalaryAmount;
                }
                else
                {
                    return TotalTax + PaidByPreviousLedger + AdvancePaidAmount + RebateAmount;
                }                
            }
        }
        public string StatusInStr
        {
            get
            {
                return (IsActive == 1) ? "Active" : "InActive";
            }
        }
        

        #endregion

        #region Functions
        public static ITaxLedger Get(int Id, long nUserID)
        {
            return ITaxLedger.Service.Get(Id, nUserID);
        }
        public static ITaxLedger Get(string sSQL, long nUserID)
        {
            return ITaxLedger.Service.Get(sSQL, nUserID);
        }
        public static List<ITaxLedger> Gets(long nUserID)
        {
            return ITaxLedger.Service.Gets(nUserID);
        }
        public static List<ITaxLedger> Gets(string sSQL, long nUserID)
        {
            return ITaxLedger.Service.Gets(sSQL, nUserID);
        }
        public List<ITaxLedger> ITaxProcess(int nEmployeeID, int nITaxRateSchemeID, bool IsAllEmployee, bool IsConsiderMaxRebate, double MinGross, long nUserID)
        {
            return ITaxLedger.Service.ITaxProcess(nEmployeeID, nITaxRateSchemeID, IsAllEmployee, IsConsiderMaxRebate, MinGross, nUserID);
        }
        public List<ITaxLedger> ITaxReprocess(int nEmployeeID, int nITaxRateSchemeID, bool IsAllEmployee, bool IsConsiderMaxRebate, double MinGross, DateTime dtDate, long nUserID)
        {
            return ITaxLedger.Service.ITaxReprocess(nEmployeeID, nITaxRateSchemeID, IsAllEmployee, IsConsiderMaxRebate, MinGross, dtDate, nUserID);
        }
        public static ITaxLedger ITaxLedger_Delete(string sITaxLedgerIDs, long nUserID)
        {
            return ITaxLedger.Service.ITaxLedger_Delete(sITaxLedgerIDs, nUserID);
        }
        #endregion

        #region ServiceFactory
        internal static IITaxLedgerService Service
        {
            get { return (IITaxLedgerService)Services.Factory.CreateService(typeof(IITaxLedgerService)); }
        }

        #endregion
    }
    #endregion

    #region IITaxLedger interface

    public interface IITaxLedgerService
    {
        ITaxLedger Get(int id, Int64 nUserID);
        ITaxLedger Get(string sSQL, Int64 nUserID);
        List<ITaxLedger> Gets(Int64 nUserID);
        List<ITaxLedger> Gets(string sSQL, Int64 nUserID);
        List<ITaxLedger> ITaxProcess(int nEmployeeID, int nITaxRateSchemeID, bool IsAllEmployee, bool IsConsiderMaxRebate, double MinGross, Int64 nUserID);
        List<ITaxLedger> ITaxReprocess(int nEmployeeID, int nITaxRateSchemeID, bool IsAllEmployee, bool IsConsiderMaxRebate, double MinGross, DateTime dtDate, Int64 nUserID);
        ITaxLedger ITaxLedger_Delete(string sITaxLedgerIDs, Int64 nUserID);
    }
    #endregion
}
