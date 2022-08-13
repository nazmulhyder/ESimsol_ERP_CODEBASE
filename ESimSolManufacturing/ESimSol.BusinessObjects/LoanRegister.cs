using System;
using ICS.Core.Framework;
using ICS.Core.Utility;
using ICS.Core;
using ESimSol.BusinessObjects;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization;
using System.ServiceModel;
using ESimSol.BusinessObjects.ReportingObject;

namespace ESimSol.BusinessObjects
{
    #region LoanRegister
    public class LoanRegister : BusinessObject
    {
        public LoanRegister()
        {
            LoanInstallmentID = 0;
            LoanID = 0;
            LoanType = EnumFinanceLoanType.None;
            InstallmentNo = "";
            InstallmentStartDate = DateTime.Now;
            InstallmentDate = DateTime.Now;
            InstallmentPrincipal = 0;
            LoanTransferType = EnumLoanTransfer.None;
            TransferDate = DateTime.Now;
            TransferDays = 0;
            TransferInterestRate = 0;
            TransferInterestAmount = 0;
            SettlementDate = DateTime.Now;
            InterestDays = 0;
            InstallmentInterestRate = 0;
            InstallmentInterestAmount = 0;
            InstallmentLiborRate = 0;
            InstallmentLiborInterestAmount = 0;
            ChargeAmount = 0;
            TotalPayableAmount = 0;
            PaidAmount = 0;
            PrincipalDeduct = 0;
            PrincipalBalance = 0;
            SettleByName = "";
            Remarks = "";
            LoanNo = "";
            LoanRefType = EnumLoanRefType.None;
            LoanRefID = 0;
            LoanRefNo = "";
            IssueDate = DateTime.Now;
            BUID = 0;
            BUName = "";
            BUShortName = "";
            LoanStartDate = DateTime.Now;
            PrincipalAmount = 0;
            LoanCurencyID = 0;
            CurrencySymbol = "";
            CRate = 0;
            PrincipalAmountBC = 0;
            LoanAmount = 0;
            InterestRate = 0;
            LiborRate = 0;
            StlmtStartDate = DateTime.Now;
            RcvBankAccountID = 0;
            RcvBankAccountNo = "";
            BankShortName = "";
            RcvDate = DateTime.Now;
            ApprovedByName = "";
            ApprovedDate = DateTime.Now;
            ReceivedByName = "";
            ErrorMessage = "";
            SearchingData = "";
            ReportLayout = EnumReportLayout.None;
        }

        #region Property
        public int LoanInstallmentID { get; set; }
        public int LoanID { get; set; }
        public EnumFinanceLoanType LoanType { get; set; }
        public string InstallmentNo { get; set; }
        public DateTime InstallmentStartDate { get; set; }
        public DateTime InstallmentDate { get; set; }
        public double InstallmentPrincipal { get; set; }
        public EnumLoanTransfer LoanTransferType { get; set; }
        public DateTime TransferDate { get; set; }
        public int TransferDays { get; set; }
        public double TransferInterestRate { get; set; }
        public double TransferInterestAmount { get; set; }
        public string SearchingData { get; set; }
        public DateTime SettlementDate { get; set; }
        public EnumReportLayout ReportLayout { get; set; }
        public int InterestDays { get; set; }
        public double InstallmentInterestRate { get; set; }
        public double InstallmentInterestAmount { get; set; }
        public double InstallmentLiborRate { get; set; }
        public double InstallmentLiborInterestAmount { get; set; }
        public double ChargeAmount { get; set; }
        public double TotalPayableAmount { get; set; }
        public double PaidAmount { get; set; }
        public double PrincipalDeduct { get; set; }
        public double PrincipalBalance { get; set; }
        public string SettleByName { get; set; }
        public string Remarks { get; set; }
        public string LoanNo { get; set; }
        public EnumLoanRefType LoanRefType { get; set; }
        public int LoanRefID { get; set; }
        public string LoanRefNo { get; set; }
        public DateTime IssueDate { get; set; }
        public int BUID { get; set; }
        public string BUName { get; set; }
        public string BUShortName { get; set; }
        public DateTime LoanStartDate { get; set; }
        public double PrincipalAmount { get; set; }
        public int LoanCurencyID { get; set; }
        public string CurrencySymbol { get; set; }
        public double CRate { get; set; }
        public double PrincipalAmountBC { get; set; }
        public double LoanAmount { get; set; }
        public double InterestRate { get; set; }
        public double LiborRate { get; set; }
        public DateTime StlmtStartDate { get; set; }
        public int RcvBankAccountID { get; set; }
        public string RcvBankAccountNo { get; set; }
        public string BankShortName { get; set; }
        public DateTime RcvDate { get; set; }
        public string ApprovedByName { get; set; }
        public DateTime ApprovedDate { get; set; }
        public string ReceivedByName { get; set; }
        public string ErrorMessage { get; set; }
        #endregion

        #region Derived Property
        public string LoanRefTypeSt
        {
            get
            {
                return EnumObject.jGet(this.LoanRefType);
            }
        }
        public string LoanTypeSt
        {
            get
            {
                return EnumObject.jGet(this.LoanType);
            }
        }
        public string LoanTransferTypeSt
        {
            get
            {
                return EnumObject.jGet(this.LoanTransferType);
            }
        }
        public string InstallmentStartDateInString
        {
            get
            {
                return InstallmentStartDate.ToString("dd MMM yyyy");
            }
        }
        public string InstallmentDateInString
        {
            get
            {
                return InstallmentDate.ToString("dd MMM yyyy");
            }
        }
        public string TransferDateInString
        {
            get
            {
                return TransferDate.ToString("dd MMM yyyy");
            }
        }
        public string SettlementDateInString
        {
            get
            {
                return SettlementDate.ToString("dd MMM yyyy");
            }
        }
        public string IssueDateInString
        {
            get
            {
                return IssueDate.ToString("dd MMM yyyy");
            }
        }
        public string LoanStartDateInString
        {
            get
            {
                return LoanStartDate.ToString("dd MMM yyyy");
            }
        }
        public string StlmtStartDateInString
        {
            get
            {
                return StlmtStartDate.ToString("dd MMM yyyy");
            }
        }
        public string RcvDateInString
        {
            get
            {
                return RcvDate.ToString("dd MMM yyyy");
            }
        }
        public string ApprovedDateInString
        {
            get
            {
                return ApprovedDate.ToString("dd MMM yyyy");
            }
        }
        #endregion

        #region Functions
      
        public static List<LoanRegister> Gets(string sSQL, long nUserID)
        {
            return LoanRegister.Service.Gets(sSQL, nUserID);
        }
      
        #endregion

        #region ServiceFactory
        internal static ILoanRegisterService Service
        {
            get { return (ILoanRegisterService)Services.Factory.CreateService(typeof(ILoanRegisterService)); }
        }
        #endregion
    }
    #endregion

    #region ILoanRegister interface
    public interface ILoanRegisterService
    {
       
        List<LoanRegister> Gets(string sSQL, Int64 nUserID);
       
    }
    #endregion
}
