using System;
using System.IO;
using ESimSol.BusinessObjects;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Collections.Generic;
using ICS.Core.Framework;
using ICS.Core.Utility;

namespace ESimSol.BusinessObjects
{
    #region SP_GeneralLedger
    [DataContract]
    public class SP_GeneralLedger : BusinessObject
    {
        public SP_GeneralLedger()
        {
            SP_GeneralLedgerID =0;
            VoucherID =0;
            AccountHeadID =0;
            Amount =0;
            IsDebit =false;
            CurrentBalance =0;
            Narration ="";
            VoucherNo ="";
            VoucherDate =DateTime.Now;
            AccountCode ="";
            AccountHeadName ="";
            OpenningBalance = 0;
            VoucherNarration="";
            Particulars = "";
            EndDate = DateTime.Now;
            StartDate = DateTime.Now;
            IsOpenningBalance = false;
            DebitAmount = 0;
            CreditAmount = 0;
            CurrencyID =0;
			ConversionRate =0;
            IsApproved = true;
            ErrorMessage = "";
            IsNullOrNot = false;
            ConfigTitle = "";
            ConfigType = EnumConfigureType.None;
            DisplayMode = EnumDisplayMode.None;
            DisplayModeInInt = 0;
            DisplayVouchers = new List<SP_GeneralLedger>();
            WeekStartDate = DateTime.Now;
            WeekEndDate = DateTime.Now;
            CompareOperatorObjs = new List<EnumObject>();
            CompanyID = 0;
            BusinessUnitID = 0;
            BusinessUnitIDs = "0";//default Group Accounts, Don't Reset it
            BUName = "Group Accounts";
            ACConfigs = new List<ACConfig>();
            SP_GeneralLedgerList = new List<SP_GeneralLedger>();
        }

        #region Properties For SP_GeneralLedger
        public int SP_GeneralLedgerID { get; set; }
        public int VoucherID { get; set; }
        public int AccountHeadID { get; set; }
        public double Amount { get; set; }
        public double DebitAmount { get; set; }
        public double CreditAmount { get; set; }
        public bool IsDebit { get; set; }
        public double CurrentBalance { get; set; }
        public string Narration { get; set; }
        public string VoucherNo { get; set; }
        public DateTime VoucherDate { get; set; }
        public double OpenningBalance { get; set; }
        public string VoucherNarration { get; set; }
        public string Particulars { get; set; }
        public string DateType { get; set; }
        public bool IsApproved { get; set; }
        public string ErrorMessage { get; set; }
        public int VoucherDetailID { get; set; }
        public bool IsNullOrNot { get; set; } 
        public string ConfigTitle { get; set; }
        public int BusinessUnitID { get; set; }
        public string BusinessUnitIDs { get; set; }
        public string BUName { get; set; }

        public EnumConfigureType ConfigType { get; set; }
        public EnumDisplayMode DisplayMode { get; set; }
        public int DisplayModeInInt { get; set; }
        public ACConfig ACConfig { get; set; }
        public List<ACConfig> ACConfigs { get; set; } 
        public List<CostCenterTransaction> CostCenterTransactions { get; set; }   
        public List<VoucherBillTransaction> VoucherBillTransactions { get; set; }
        public List<VPTransaction> VPTransactions { get; set; }  
        public List<VoucherReference> VoucherReferences { get; set; }
        public List<EnumObject> DisplayModes { get; set; }
        public List<EnumObject> CompareOperatorObjs { get; set; }
        #endregion

        #region Derive Properties
        public bool IsForCurrentDate { get; set; }
        public int CurrencyID { get; set; }
        public List<SP_GeneralLedger> SP_GeneralLedgerList { get; set; }
        public List<SP_GeneralLedger> DisplayVouchers { get; set; }  
        public Currency Currency { get; set; }
        public List<Currency> Currencies { get; set; }
        public Company Company { get; set; }
        public bool IsOpenningBalance { get; set; }
        public DateTime WeekStartDate { get; set; }
        public DateTime WeekEndDate { get; set; }
        public string WeekStartDateSt { get { return this.WeekStartDate.ToString("dd MMM yyyy"); } }
        public string WeekEndDateSt { get { return this.WeekEndDate.ToString("dd MMM yyyy"); } }
        public int CompanyID { get; set; }
        public string VoucherIDWithNo
        {
            get
            {
                return this.VoucherNo + "~" + this.VoucherID;
            }
        }
        public string OpenningBalanceInString
        {
            get
            {
                if (this.OpenningBalance < 0)
                {
                    return "(" + Global.MillionFormat(this.OpenningBalance * (-1)) + ")";
                }
                else
                {
                    return Global.MillionFormat(this.OpenningBalance);
                }
            }
        }
        public string VoucherDateInString
        {
            get 
            {
                if (this.IsOpenningBalance)
                {
                    return "";
                }
                else if (this.VoucherID == 0)
                {
                    return this.ConfigTitle;
                }                
                else
                {
                    return this.VoucherDate.ToString("dd MMM yyyy");
                }
            }
        }
        public string VoucherDateXLSt { get { return this.IsOpenningBalance ? "" : this.VoucherID == 0 ? this.ConfigTitle : this.VoucherDate.ToString("MM/dd/yyyy"); } }
        public string IsDrst
        {
            get
            {
                if (this.IsDebit)
                {
                    return "Dr";
                }
                else
                {
                    return "Cr";
                }

            }
        }
        public string IsDr_Opreningst
        {
            get
            {
                if (this.IsDebit_OpeningBalance)
                {
                    return "Dr";
                }
                else
                {
                    return "Cr";
                }

            }
        }
        public string Accountnameandcode
        {
            get
            {
                if (AccountCode == null)
                {
                    return "";// AccountCode.ToString() + " " + AccountHeadName;  // Comment by Faruk diagonosis further 
                }
                else
                {
                    return AccountCode.ToString() + " " + AccountHeadName;
                }
            }
        }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
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
        public string CurrentBalanceInString { get { return this.CurrentBalance < 0 ? "(" + Global.MillionFormat(this.CurrentBalance * (-1)) + ")" : Global.MillionFormat(this.CurrentBalance); } }
        public string DebitAmountInString { get { return this.DebitAmount <= 0 ? "-" : Global.MillionFormat(this.DebitAmount); } }
        public string CreditAmountInString { get { return this.CreditAmount <= 0 ? "-" : Global.MillionFormat(this.CreditAmount); } }
        public string TotalDebitAmountInString { get; set; }
        public string TotalCreditAmountInString { get; set; }
        public double DebitAmount_BC { get; set; }
        public double CreditAmount_BC { get; set; }
        #endregion

        #region Temporary declared
        public bool IsDebit_OpeningBalance { get; set; }
        public string AccountCode { get; set; }
        public string AccountHeadName { get; set; }
        public double ConversionRate { get; set; }
        #endregion

        #region Non DB Function
        public static string CalculateTotalDebitCreditAmount(List<SP_GeneralLedger> oSP_GeneralLedgers, bool bIsDebit)
        {
            double nTotalAmount = 0;
            foreach (SP_GeneralLedger oItem in oSP_GeneralLedgers)
            {
                if (bIsDebit)
                {
                    nTotalAmount = nTotalAmount + oItem.DebitAmount;
                }
                else
                {
                    nTotalAmount = nTotalAmount + oItem.CreditAmount;
                }
            }
            return Global.MillionFormat(nTotalAmount);
        }
        #endregion

        #region Functions
        public static List<SP_GeneralLedger> GetsGeneralLedger(int nAccountHeadId, DateTime dstartDate, DateTime dendDate, int nCurrencyID, bool bIsApproved, string BusinessUnitIDs, int nUserID)
        {
            return SP_GeneralLedger.Service.GetsGeneralLedger(nAccountHeadId, dstartDate, dendDate, nCurrencyID, bIsApproved, BusinessUnitIDs, nUserID);
        }
        public static List<SP_GeneralLedger> ProcessGeneralLedger(int nAccountHeadId, DateTime dstartDate, DateTime dendDate, int nCurrencyID, bool bIsApproved, string sSQL, int nUserID)
        {
            return SP_GeneralLedger.Service.ProcessGeneralLedger(nAccountHeadId, dstartDate, dendDate, nCurrencyID, bIsApproved, sSQL, nUserID);
        }
        public static List<SP_GeneralLedger> Gets(string sSQL, int nUserID)
        {
            return SP_GeneralLedger.Service.Gets(sSQL, nUserID);
        }
        public static List<SP_GeneralLedger> GetsForReport(SP_GeneralLedger oCCT, int nUserID)
        {
            return SP_GeneralLedger.Service.GetsForReport(oCCT, nUserID);
        }
        #endregion


        #region ServiceFactory
        internal static ISP_GeneralLedgerService Service
        {
            get { return (ISP_GeneralLedgerService)Services.Factory.CreateService(typeof(ISP_GeneralLedgerService)); }
        }
        #endregion

    
    }
    #endregion

    #region ISP_GeneralLedger
    public interface ISP_GeneralLedgerService
    {
        List<SP_GeneralLedger> GetsGeneralLedger(int nAccountHeadId, DateTime dstartDate, DateTime dendDate, int nCurrencyID, bool bIsApproved, string BusinessUnitIDs, int nUserID);
        List<SP_GeneralLedger> ProcessGeneralLedger(int nAccountHeadId, DateTime dstartDate, DateTime dendDate, int nCurrencyID, bool bIsApproved, string sSQL, int nUserID);
        List<SP_GeneralLedger> Gets(string sSQL, int nUserID);
        List<SP_GeneralLedger> GetsForReport(SP_GeneralLedger oCCT, int nUserID);
    }
    #endregion
}