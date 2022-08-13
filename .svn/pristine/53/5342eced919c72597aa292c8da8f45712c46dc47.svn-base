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
    #region MgtDashBoardAccount
    public class MgtDashBoardAccount : BusinessObject
    {
        public MgtDashBoardAccount()
        {
            BUID = 0;
            ReportDate = DateTime.Now;
            CashBalance = 0;
            BankBalance = 0;
            ForeignBankBalance = 0;
            Receivable = 0;
            Payable = 0;
            BankLoan = 0;
            ErrorMessage = "";         
        }

        #region Property
        public int BUID { get; set; }
        public DateTime ReportDate { get; set; }
        public double CashBalance { get; set; }
        public double BankBalance { get; set; }
        public double ForeignBankBalance { get; set; }
        public double Receivable { get; set; }
        public double Payable { get; set; }
        public double BankLoan { get; set; }
        public int CashCurrencyID { get; set; }
        public string CashCSymbol { get; set; }
        public int BankCurrencyID { get; set; }
        public string BankCSymbol { get; set; }
        public int FBankCurrencyID { get; set; }
        public string FBankCSymbol { get; set; }
        public int RcvCurrencyID { get; set; }
        public string RcvCSymbol { get; set; }
        public int PayableCurrencyID { get; set; }
        public string PayableCSymbol { get; set; }
        public int BLoanCurrencyID { get; set; }
        public string BLoanCSymbol { get; set; } 

        public string ErrorMessage { get; set; }

        #endregion

        #region Derived Property         
        public string ReportDateInString
        {
            get
            {
                return ReportDate.ToString("dd MMM yyyy");
            }
        }

        public string CashBalanceSt
        {
            get
            {
                return CashCSymbol + " " + CashBalance.ToString("#,##0.00;(#,##0.00)");
            }
        }
        public string BankBalanceSt
        {
            get
            {
                return BankCSymbol + " " + BankBalance.ToString("#,##0.00;(#,##0.00)");
            }
        }
        public string ForeignBankBalanceSt
        {
            get
            {
                return FBankCSymbol + " " + ForeignBankBalance.ToString("#,##0.00;(#,##0.00)");
            }
        }
        public string ReceivableSt
        {
            get
            {
                return RcvCSymbol + " " + Receivable.ToString("#,##0.00;(#,##0.00)");
            }
        }
        public string PayableSt
        {
            get
            {
                return PayableCSymbol + " " + Payable.ToString("#,##0.00;(#,##0.00)");
            }
        }
        public string BankLoanSt
        {
            get
            {
                return BLoanCSymbol + " " + BankLoan.ToString("#,##0.00;(#,##0.00)");
            }
        }

        
        #endregion

        #region Functions       
        public static List<MgtDashBoardAccount> Gets(MgtDashBoardAccount oMgtDashBoardAccount, long nUserID)
        {
            return MgtDashBoardAccount.Service.Gets(oMgtDashBoardAccount, nUserID);
        }
        #endregion

        #region ServiceFactory
        internal static IMgtDashBoardAccountService Service
        {
            get { return (IMgtDashBoardAccountService)Services.Factory.CreateService(typeof(IMgtDashBoardAccountService)); }
        }
        #endregion
    }
    #endregion

    #region IMgtDashBoardAccount interface
    public interface IMgtDashBoardAccountService
    {       
        List<MgtDashBoardAccount> Gets(MgtDashBoardAccount oMgtDashBoardAccount, Int64 nUserID);

    }
    #endregion
}
