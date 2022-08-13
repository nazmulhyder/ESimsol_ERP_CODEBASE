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
    #region TrialBalance_Categorized
    public class TrialBalance_Categorized : BusinessObject
    {
        public TrialBalance_Categorized()
        {
            AccountHeadID = 0;
            AccountCode = "";
            AccountHeadName = "";
            ParentAccountHeadID = 0;
            ComponentType = EnumComponentType.None;
            AccountType = EnumAccountType.None;
            OpenningBalance = 0;
            DebitAmount = 0;
            CreditAmount = 0;
            ClosingBalance = 0;
            AccountingYears = new List<AccountingSession>();
            BusinessUnitID = 0;
            ErrorMessage = "";
            AccountTypeInInt = (int)EnumAccountType.Ledger;
            Level = 0;
            ACConfigs = new List<ACConfig>();
            OptionID = 0;
            CCOptionID = 0;
            CCID = 0;
            VoucherBillID = 0;
            IsApproved = false;
            BUName = "Group Accounts";
            BusinessUnitIDs = "0";
            CurrencyID = 0;
        }

        #region Properties
        public int AccountHeadID { get; set; }
        public string AccountCode { get; set; }
        public string AccountHeadName { get; set; }
        public int ParentAccountHeadID { get; set; }
        public EnumComponentType ComponentType { get; set; }
        public EnumAccountType AccountType { get; set; }
        public Double OpenningBalance { get; set; }
        public Double DebitAmount { get; set; }
        public Double CreditAmount { get; set; }
        public Double ClosingBalance { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public int BusinessUnitID { get; set; }
        public string ErrorMessage { get; set; }
        public int AccountTypeInInt { get; set; }
        public int Level { get; set; }
        public int OptionID { get; set; }
        public int CCOptionID { get; set; }
        public List<ACConfig> ACConfigs { get; set; }
        public int CCID { get; set; }
        public int VoucherBillID { get; set; }
        public int CurrencyID { get; set; }
        public bool IsApproved { get; set; }
        public bool IsForCurrentDate { get; set; }
        public string BusinessUnitIDs { get; set; }
        public string BUName { get; set; }
        #endregion

        #region Derived Property
        public List<GLMonthlySummary> GLMonthlySummarys { get; set; }
        public string AccountHeadCodeName { get { return this.AccountHeadName + " [" + this.AccountCode + "]"; } }
        public string TotalDebitAmount { get; set; }
        public string TotalCreditAmount { get; set; }
        public string DateType { get; set; }

        public string DebitAmountInString { get { return this.DebitAmount == 0 ? "-" : Global.MillionFormat(this.DebitAmount); } }
        public string CreditAmountInString { get { return this.CreditAmount == 0 ? "-" : Global.MillionFormat(this.CreditAmount); } }

        #region OpeningBalanceSt
        public string OpeningBalanceSt { get { return this.OpenningBalance == 0 ? "-" : this.OpenningBalance < 0 ? "(" + Global.MillionFormat(this.OpenningBalance * (-1)) + ")" : Global.MillionFormat(Math.Abs(this.OpenningBalance)); } }
        #endregion
        #region ClosingBalanceSt
        public string ClosingBalanceSt { get { return this.ClosingBalance == 0 ? "-" : this.ClosingBalance < 0 ? "(" + Global.MillionFormat(this.ClosingBalance * (-1)) + ")" : Global.MillionFormat(Math.Abs(this.ClosingBalance)); } }
        public Double DebitClosingBalance { get { return this.ComponentType == EnumComponentType.Asset || this.ComponentType == EnumComponentType.Expenditure ? this.ClosingBalance : 0; } }
        public Double CreditClosingBalance { get { return this.ComponentType == EnumComponentType.Asset || this.ComponentType == EnumComponentType.Expenditure ? 0 : this.ClosingBalance; } }
        public string DebitClosingBalanceSt { get { return this.ComponentType == EnumComponentType.Asset || this.ComponentType == EnumComponentType.Expenditure ? this.ClosingBalanceSt : "-"; } }
        public string CreditClosingBalanceSt { get { return this.ComponentType == EnumComponentType.Asset || this.ComponentType == EnumComponentType.Expenditure ? "-" : this.ClosingBalanceSt; } }
        #endregion
        public List<TrialBalance_Categorized> TrialBalance_Categorizeds { get; set; }
        public List<AccountingSession> AccountingYears { get; set; }
        public Company Company { get; set; }

        public string StartDateSt { get { return this.StartDate.ToString("dd MMM yyyy"); } }
        public string EndDateSt { get { return this.EndDate.ToString("dd MMM yyyy"); } }
        private string _sAccountTypeSt = "";
        public string AccountTypeSt { get { _sAccountTypeSt = this.AccountType.ToString(); return _sAccountTypeSt; } set { _sAccountTypeSt = value == null ? "" : value; } }
        #endregion

        #region Functions
        public static List<TrialBalance_Categorized> Gets(int nAccountHead, DateTime dStartDate, DateTime dEndDate, bool bIsApproved, int nCurrencyID, int nBusinessUnitID, int nUserID)
        {
            return TrialBalance_Categorized.Service.Gets(nAccountHead, dStartDate, dEndDate, bIsApproved, nCurrencyID, nBusinessUnitID, nUserID);
        }
        public static List<TrialBalance_Categorized> ProcessTrialBalance(DateTime dStartDate, DateTime dEndDate, string sStartLedgerCode, string sEndLedgerCode, string sSQL, int nUserID)
        {
            return TrialBalance_Categorized.Service.ProcessTrialBalance(dStartDate, dEndDate, sStartLedgerCode, sEndLedgerCode, sSQL, nUserID);
        }
        #endregion

        #region ServiceFactory
        internal static ITrialBalance_CategorizedService Service
        {
            get { return (ITrialBalance_CategorizedService)Services.Factory.CreateService(typeof(ITrialBalance_CategorizedService)); }
        }
        #endregion
    }
    #endregion

    #region ITrialBalance_Categorized interface
    public interface ITrialBalance_CategorizedService
    {
        List<TrialBalance_Categorized> Gets(int nAccountHead, DateTime dStartDate, DateTime dEndDate, bool bIsApproved, int nCurrencyID, int nBusinessUnitID, int nUserID);
        List<TrialBalance_Categorized> ProcessTrialBalance(DateTime dStartDate, DateTime dEndDate, string sStartLedgerCode, string sEndLedgerCode, string sSQL, int nUserID);
    }
    #endregion

    #region TChartsOfAccount
    //this is a derive class that use only for display user menu(j tree tree architecture reference : http://www.jeasyui.com/documentation/index.php# )
    public class TTrialBalance_Categorized
    {
        public TTrialBalance_Categorized()
        {
            AccountHeadID = 0;
            AccountCode = "";
            AccountHeadName = "";
            ParentHeadID = 0;
            ComponentID = 0;
            AccountTypeInString = "";
            OpenningBalance = "";
            ClosingBalance = "";
            DebitAmount = "";
            CreditAmount = "";
            state = "open";
            attributes = "";
            ClosingBalanceDbl = 0;
            CreditAmountDbl = 0;
            DebitAmountDbl = 0;
            OpenningBalanceDbl = 0;
            AccountType = EnumAccountType.None;
            ComponentType = EnumComponentType.None;
            ErrorMessage = "";
        }
        public int AccountHeadID { get; set; }                 //: node id, which is important to load remote data
        public string AccountCode { get; set; }            //: node text to show
        public string AccountHeadName { get; set; }
        public string state { get; set; }           //: node state, 'open' or 'closed', default is 'open'. When set to 'closed', the node have children nodes and will load them from remote site        
        public string attributes { get; set; }      //: custom attributes can be added to a node // attributes set a sting that hold action name & controller namr seperated by '~'
        public int ParentHeadID { get; set; }
        public string AccountTypeInString { get; set; }
        public string DebitAmount { get; set; }
        public string CreditAmount { get; set; }
        public string ErrorMessage { get; set; }
        public int ComponentID { get; set; }
        public Company Company { get; set; }
        public string OpenningBalance { get; set; }
        public string ClosingBalance { get; set; }
        public List<TTrialBalance_Categorized> children { get; set; }//: an array nodes defines some children nodes
        public List<TTrialBalance_Categorized> TTrialBalance_Categorizeds { get; set; }
        public List<EnumObject> AccountTypeObjs { get; set; }
        public double OpenningBalanceDbl { get; set; }
        public double DebitAmountDbl { get; set; }
        public double CreditAmountDbl { get; set; }
        public double ClosingBalanceDbl { get; set; }
        public EnumAccountType AccountType { get; set; }
        public EnumComponentType ComponentType { get; set; }
    }
    #endregion
}
