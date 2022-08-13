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
    #region SP_GeneralJournal
    public class SP_GeneralJournal : BusinessObject
    {
        public SP_GeneralJournal()
        {
            VoucherDetailID = 0;
            VoucherID = 0;
            AccountHeadID = 0;
            IsDebit = false;
            Narration = "";
            DebitAmount = 0.0;
            CreditAmount = 0.0;
            VoucherNo = "";
            VoucherTypeID = 0;
            VoucherDate = DateTime.Today;
            VoucherName = "";
            AccountCode = "";
            AccountHeadName = "";
            EndDate = DateTime.Now;
            StartDate = DateTime.Now;
            CurrencyID  = 0;
			CurrencySymbol = "";
            ErrorMessage = "";
            IsNullOrNot = false;
            DisplayVouchers = new List<SP_GeneralJournal>();
            DisplayMode = EnumDisplayMode.None;
            DisplayModeInInt = 0;
            ConfigTitle = "";
            ConfigType = EnumConfigureType.None;
            VoucherNarration = "";
            WeekStartDate = DateTime.Now;
            WeekEndDate = DateTime.Now;
            DateSearch = 0;
            BusinessUnitID = 0;
            IsForCurrentDate = false;
            DisplayModes = new List<EnumObject>();
            ACConfigs = new List<ACConfig>();
            IsApproved = false;
        }

        #region Properties For SP_GeneralJournal
        public int PUEGID { get; set; }
        public string Name { get; set; }
        public string Note { get; set; }
        public int VoucherDetailID { get; set; }
        public int VoucherID { get; set; }
        public int AccountHeadID { get; set; }
        public bool IsDebit { get; set; }
        public string Narration { get; set; }
        public double DebitAmount { get; set; }
        public double CreditAmount { get; set; }
        public string VoucherNo { get; set; }
        public int VoucherTypeID { get; set; }
        public DateTime VoucherDate { get; set; }
        public string VoucherName { get; set; }
        public string AccountCode { get; set; }
        public string AccountHeadName { get; set; }
        public int CurrencyID{ get; set; }
        public string CurrencySymbol { get; set; }
        public int DisplayModeInInt { get; set; }
        public EnumDisplayMode DisplayMode { get; set; }
        public string ErrorMessage { get; set; }
        public int DateSearch { get; set; }
        public int BusinessUnitID { get; set; }
        public bool IsForCurrentDate { get; set; }
        #endregion

        #region Derive Properties
        public bool IsApproved { get; set; }
        public List<ACConfig> ACConfigs { get; set; }
        public List<SP_GeneralJournal> GeneralJournalList { get; set; }
        public List<SP_GeneralJournal> SP_GeneralJournalList { get; set; }
        public List<SP_GeneralJournal> DisplayVouchers { get; set; }
        public List<EnumObject> DisplayModes { get; set; }
        public EnumConfigureType ConfigType { get; set; }
        public Company Company { get; set; }
        public bool IsNullOrNot { get; set; }
        public string VoucherNarration { get; set; }
        public DateTime WeekStartDate { get; set; }
        public DateTime WeekEndDate { get; set; }
        public string VoucherDateInString
        {
            get
            {
                if (this.VoucherID == 0)
                {
                    return this.ConfigTitle;
                }
                else
                {
                    return this.VoucherDate.ToString("dd MMM yyyy");
                }
            }
        }
        public string StringVoucherDate
        {
            get
            {
                return this.VoucherDate.ToString("dd MMM yyyy");
            }
        }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        
        public string ConfigTitle { get; set; }
        public string VoucherNoInString
        {
            get
            {
                return this.VoucherNo + '~' + this.VoucherID;
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
        public string DebitAmountInString
        {
            get
            {
                return Global.MillionFormat(this.DebitAmount);
            }
        }
        public string CreditAmountInString
        {
            get
            {
                return Global.MillionFormat(this.CreditAmount);
            }
        }
        public string CurrentBalanceSt = "";
        public string sIsDebitSt = "";
        public string IsDebitSt
        {
            get
            {
                if (IsDebit)
                {
                    sIsDebitSt = "Debit";
                }
                else
                {
                    sIsDebitSt = "Credit";
                }
                return sIsDebitSt;
            }
        }        
        public string TotalDebitAmountInString { get; set; }
        public string TotalCreditAmountInString { get; set; }
        #endregion
        

        #region Functions
        public static List<SP_GeneralJournal> GetsGeneralJournal(string sSQL, int nUserID)
        {
            return SP_GeneralJournal.Service.GetsGeneralJournal(sSQL,  nUserID);
        }
        public static List<SP_GeneralJournal> Gets(string sSQL,int nCompanyID, int nUserID)
        {
            return SP_GeneralJournal.Service.Gets(sSQL, nCompanyID,  nUserID);
        }
        public static List<SP_GeneralJournal> Gets_SuspendALog(string sSQL,int nCompanyID, int nUserID)
        {
            return SP_GeneralJournal.Service.Gets_SuspendALog(sSQL, nCompanyID,  nUserID);
        }
        #endregion

        #region Non DB Function
        public static string GetTotalBalance(bool bIsDebit, List<SP_GeneralJournal> oSP_GeneralJournals)
        {
           double nTotalAmount = 0;
           foreach (SP_GeneralJournal oItem in oSP_GeneralJournals)
           {
               if (oItem.VoucherDetailID > 0)
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
           }
           return Global.MillionFormat(nTotalAmount);
        }
        #endregion

    
        #region ServiceFactory
        internal static ISP_GeneralJournalService Service
        {
            get { return (ISP_GeneralJournalService)Services.Factory.CreateService(typeof(ISP_GeneralJournalService)); }
        }
        #endregion

    }
    #endregion
   
    #region ISP_GeneralJournal
    public interface ISP_GeneralJournalService
    {
        List<SP_GeneralJournal> GetsGeneralJournal(string sSQL, int nUserID);
        List<SP_GeneralJournal> Gets(string sSQL, int nCompanyID, int nUserID);
        List<SP_GeneralJournal> Gets_SuspendALog(string sSQL, int nCompanyID, int nUserID);
    }
    #endregion
}