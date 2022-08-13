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
    #region LoanRequest

    public class LoanRequest : BusinessObject
    {

        #region  Constructor
        public LoanRequest()
        {
            LoanRequestID = 0;
            RequestNo = string.Empty;
            EmployeeID = 0;
            LoanType = EnumLoanType.None;
            RequestDate = DateTime.Today;
            ExpectDate = DateTime.Today;
            Purpose = string.Empty;
            RequestStatus = EnumRequestStatus.Request;
            LoanAmount = 0;
            NoOfInstallment = 0;
            InstallmentAmount = 0;
            InterestRate = 0;
            Remarks = string.Empty;
            ProceedBy = 0;
            EmployeeLoanID = 0;
            IsPFLoan = true;
            ErrorMessage = string.Empty;
            Params = string.Empty;
        }
        #endregion

        #region Properties

        public int LoanRequestID { get; set; }
        public string RequestNo { get; set; }
        public int EmployeeID { get; set; }
        public EnumLoanType LoanType { get; set; }
        public DateTime RequestDate { get; set; }
        public DateTime ExpectDate { get; set; }
        public string Purpose { get; set; }
        public EnumRequestStatus RequestStatus { get; set; }
        public double LoanAmount { get; set; }
        public short NoOfInstallment { get; set; }
        public double InstallmentAmount { get; set; }
        public double InterestRate { get; set; }
        public string Remarks { get; set; }
        public int ProceedBy { get; set; }
        public int EmployeeLoanID { get; set; }
        public bool IsPFLoan { get; set; }
        public string ErrorMessage { get; set; }
        public string Params { get; set; }

        #endregion

        #region Derived Properties

        public string EmployeeName { get; set; }
        public string EmployeeCode { get; set; }
        public string ProceedByName { get; set; }
        public string OfficialInfo { get; set; }
        public string SalaryInfo { get; set; }
        public string LastLoanInfo { get; set; }
        public string PFInfo { get; set; }
        public bool IsAdjustable { get { return (this.EmployeeLoanID > 0) ? true : false; } }

        public string LoanTypeStr
        {
            get
            {
                return this.LoanType.ToString();
            }
        }

        public string RequestStatusStr
        {
            get
            {
                return this.RequestStatus.ToString();
            }
        }
        
        public string RequestDateStr
        {
            get
            {
                return this.RequestDate.ToString("dd MMM yyyy");
            }
        }

        public string ExpectDateStr
        {
            get
            {
                return this.ExpectDate.ToString("dd MMM yyyy");
            }
        }


        public string EmployeeNameCode
        {
            get
            {
                return this.EmployeeName + " ["+ this.EmployeeCode+"]" ;
            }
        }

        public string IsAdjustableStr
        {
            get
            {
                return (this.IsAdjustable) ? "Adjustable Request" : "New Request";
            }
        }
        #endregion


        #region Functions

        public LoanRequest IUD(int nDBOperation, long nUserID)
        {
            return LoanRequest.Service.IUD(this, nDBOperation, nUserID);
        }
        public static LoanRequest Get(int nLoanRequestID, long nUserID)
        {
            return LoanRequest.Service.Get(nLoanRequestID, nUserID);
        }
        public static List<LoanRequest> Gets(string sSQL, long nUserID)
        {
            return LoanRequest.Service.Gets(sSQL, nUserID);
        }
        public LoanRequest Approval(List<EmployeeLoanInstallment> oELIs, long nUserID)
        {
            return LoanRequest.Service.Approval(this, oELIs, nUserID);
        }
        #endregion

        #region ServiceFactory
        internal static ILoanRequestService Service
        {
            get { return (ILoanRequestService)Services.Factory.CreateService(typeof(ILoanRequestService)); }
        }
        #endregion
    }
    #endregion



    #region ILoanRequest interface
    public interface ILoanRequestService
    {
        LoanRequest IUD(LoanRequest oLoanRequest, int nDBOperation, long nUserID);

        LoanRequest Approval(LoanRequest oLoanRequest, List<EmployeeLoanInstallment> oELIs, long nUserID);

        LoanRequest Get(int nLoanRequestID, long nUserID);

        List<LoanRequest> Gets(string sSQL, long nUserID);
    }
    #endregion
}