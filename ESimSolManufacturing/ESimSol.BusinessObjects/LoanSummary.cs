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
    #region LoanSummary

    public class LoanSummary : BusinessObject
    {

        #region  Constructor
        public LoanSummary()
        {
            DepartmentID = 0;
            DepartmentName = "";
            LoanAmount = 0;
            InstallmentDeduction = 0;
            InterestDeduction = 0;
            RefundAmount = 0;
            RefundCharge = 0;
            ErrorMessage = string.Empty;
            Params = string.Empty;
        }
        #endregion

        #region Properties

        public int DepartmentID { get; set; }
        public string DepartmentName { get; set; }
        public double LoanAmount { get; set; }
        public double InstallmentDeduction { get; set; }
        public double InterestDeduction { get; set; }
        public double RefundAmount { get; set; }
        public double RefundCharge  { get; set; }
        public string ErrorMessage { get; set; }
        public string Params { get; set; }

        #endregion

        #region Derived Properties

        public double LoanBalance
        {
            get
            {
                return this.LoanAmount - (this.InstallmentDeduction - this.RefundAmount);
            }
        }

        public double TotalExtra
        {
            get
            {
                return this.InterestDeduction + this.RefundCharge;
            }
        }


        #endregion


        #region Functions

        public static List<LoanSummary> Gets(DateTime dtFrom, DateTime dtTo, string sDeptID, int nSalaryMonth, long nUserID)
        {
            return LoanSummary.Service.Gets(dtFrom, dtTo, sDeptID, nSalaryMonth, nUserID);
        }

        #endregion

        #region ServiceFactory
        internal static ILoanSummaryService Service
        {
            get { return (ILoanSummaryService)Services.Factory.CreateService(typeof(ILoanSummaryService)); }
        }
        #endregion
    }
    #endregion



    #region ILoanSummary interface
    public interface ILoanSummaryService
    {
        List<LoanSummary> Gets(DateTime dtFrom, DateTime dtTo, string sDeptID, int nSalaryMonth, long nUserID);
    }
    #endregion
}