using System;
using System.IO;
using ESimSol.BusinessObjects;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Collections.Generic;
using ICS.Core;
using ICS.Core.Framework;
using ICS.Core.Utility;

namespace ESimSol.BusinessObjects
{
    #region EmployeeLoan

    public class EmployeeLoan : BusinessObject
    {

        #region  Constructor
        public EmployeeLoan()
        {
            EmployeeLoanID = 0;
            EmployeeID = 0;
            Code = string.Empty;
            LoanType = EnumLoanType.None;
            Purpose = string.Empty;
            InterestRate = 0;
            NoOfTotalInstallment = 0;
            RecommendBy = 0;
            RecommendNote = string.Empty;
            RecommendDate = DateTime.Today;
            ApproveBy = 0;
            ApproveNote = string.Empty;
            ApproveDate = DateTime.Today;
            ErrorMessage = string.Empty;
            Params = string.Empty;
        }
        #endregion

        #region Properties

        public int EmployeeLoanID { get; set; }
        public int EmployeeID { get; set; }
        public string Code { get; set; }
        public EnumLoanType LoanType { get; set; }
        public string Purpose { get; set; }
        public double InterestRate { get; set; }
        public double NoOfTotalInstallment { get; set; }
        public double InstallmentAmount { get; set; }
        public int RecommendBy { get; set; }
        public string RecommendNote { get; set; }
        public DateTime RecommendDate { get; set; }
        public int ApproveBy { get; set; }
        public string ApproveNote { get; set; }
        public DateTime ApproveDate { get; set; }
        public bool IsFinish { get; set; }
        public string FinishNote { get; set; }
        public bool IsPFLoan { get; set; }
        public string ErrorMessage { get; set; }
        public string Params { get; set; }

        #endregion

        #region Derived Properties
        public double LoanAmount { get; set; }
        public double RefundableAmount { get; set; }
        public double RemainingInstallment { get; set; }
        public string EmployeeName { get; set; }
        public string EmployeeCode { get; set; }
        public string RecommendByName { get; set; }
        public string ApproveByName { get; set; }

        public string OfficialInfo { get; set; }
        public string SalaryInfo { get; set; }
        public string LastLoanInfo { get; set; }
        public string PFInfo { get; set; }
        public bool IsActive { get; set; }
        public string EmployeeWorkingStatus
        {
            get
            {
                return this.IsActive ? "Continued" : "Discontinued";
            }
        }

        public string LoanTypeStr
        {
            get
            {
                return this.LoanType.ToString();
            }
        }

        public string RecommendDateStr
        {
            get
            {
                return this.RecommendDate.ToString("dd MMM yyyy");
            }
        }

        public string ApproveDateStr
        {
            get
            {
                return this.ApproveDate.ToString("dd MMM yyyy");
            }
        }

        public string EmployeeNameCode
        {
            get
            {
                return this.EmployeeName + " [" + this.EmployeeCode + "]";
            }
        }


        #endregion


        #region Functions

        public static EmployeeLoan Get(int nEmployeeLoanID, long nUserID)
        {
            return EmployeeLoan.Service.Get(nEmployeeLoanID, nUserID);
        }
        public static List<EmployeeLoan> Gets(string sSQL, long nUserID)
        {
            return EmployeeLoan.Service.Gets(sSQL, nUserID);
        }

        #endregion

        #region ServiceFactory
        internal static IEmployeeLoanService Service
        {
            get { return (IEmployeeLoanService)Services.Factory.CreateService(typeof(IEmployeeLoanService)); }
        }
        #endregion
    }
    #endregion


    #region IEmployeeLoan interface
    public interface IEmployeeLoanService
    {
        EmployeeLoan Get(int nEmployeeLoanID, long nUserID);

        List<EmployeeLoan> Gets(string sSQL, long nUserID);
    }
    #endregion
}