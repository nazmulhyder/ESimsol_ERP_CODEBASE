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
    #region LoanInterest
    public class LoanInterest : BusinessObject
    {
        public LoanInterest()
        {
            LoanInterestID = 0;
            LoanID = 0;
            LoanAmount = 0;
            InterestEffectDate = DateTime.Today;
            InterestStartDate = DateTime.Today;
            InterestEndDate = DateTime.Today;
            InterestDays = 0;
            InterestType = EnumInterestType.None;
            CurrencyID = 0;
            InterestRate = 0;
            InterestAmount = 0;
            CRate = 0;
            InterestAmountBC = 0;
            EntryUserID = 0;
            CurrencySymbol = "";
            EntryUserName = "";
            ErrorMessage = "";
        }

        #region Property
        public int LoanInterestID { get; set; }
        public int LoanID { get; set; }
        public double LoanAmount { get; set; }
        public DateTime InterestEffectDate { get; set; }
        public DateTime InterestStartDate { get; set; }
        public DateTime InterestEndDate { get; set; }
        public int InterestDays { get; set; }
        public EnumInterestType InterestType { get; set; }
        public int CurrencyID { get; set; }
        public double InterestRate { get; set; }
        public double InterestAmount { get; set; }
        public double CRate { get; set; }
        public double InterestAmountBC { get; set; }
        public int EntryUserID { get; set; }
        public string CurrencySymbol { get; set; }
        public string EntryUserName { get; set; }
        public string ErrorMessage { get; set; }
        #endregion

        #region Derived Property
        public string InterestEffectDateSt
        {
            get
            {
                return this.InterestEffectDate.ToString("dd MMM yyyy");
            }
        }
        public string InterestStartDateSt
        {
            get
            {
                return this.InterestStartDate.ToString("dd MMM yyyy");
            }
        }
        public string InterestEndDateSt
        {
            get
            {
                return this.InterestEndDate.ToString("dd MMM yyyy");
            }
        }
        public string InterestTypeSt
        {
            get
            {
                return EnumObject.jGet(this.InterestType);
            }
        }
        public string LoanAmountSt
        {
            get
            {
                return this.LoanAmount.ToString("#,##0.00;(#,##0.00)");
            }
        }
        public string InterestRateSt
        {
            get
            {
                return this.InterestRate.ToString("#,##0.00;(#,##0.00)");
            }
        }
        public string InterestAmountSt
        {
            get
            {
                return this.InterestAmount.ToString("#,##0.00;(#,##0.00)");
            }
        }
        public string CRateSt
        {
            get
            {
                return this.CRate.ToString("#,##0.00;(#,##0.00)");
            }
        }
        public string InterestAmountBCSt
        {
            get
            {
                return this.InterestAmountBC.ToString("#,##0.00;(#,##0.00)");
            }
        }
        #endregion

        #region Functions
        public static List<LoanInterest> Gets(long nUserID)
        {
            return LoanInterest.Service.Gets(nUserID);
        }
        public static List<LoanInterest> GetsByLoan(int nLoanID, long nUserID)
        {
            return LoanInterest.Service.GetsByLoan(nLoanID, nUserID);
        }
        public static List<LoanInterest> Gets(string sSQL, long nUserID)
        {
            return LoanInterest.Service.Gets(sSQL, nUserID);
        }
        public LoanInterest Get(int id, long nUserID)
        {
            return LoanInterest.Service.Get(id, nUserID);
        }

        public string Delete(int id, long nUserID)
        {
            return LoanInterest.Service.Delete(id, nUserID);
        }
        #endregion

        #region ServiceFactory
        internal static ILoanInterestService Service
        {
            get { return (ILoanInterestService)Services.Factory.CreateService(typeof(ILoanInterestService)); }
        }
        #endregion
    }
    #endregion

    #region ILoanInterest interface
    public interface ILoanInterestService
    {
        LoanInterest Get(int id, Int64 nUserID);
        List<LoanInterest> Gets(Int64 nUserID);
        List<LoanInterest> GetsByLoan(int nLoanID, Int64 nUserID);        
        List<LoanInterest> Gets(string sSQL, Int64 nUserID);
        string Delete(int id, Int64 nUserID);
    }
    #endregion
}
