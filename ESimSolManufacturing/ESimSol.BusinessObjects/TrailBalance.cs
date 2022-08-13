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
    #region TrailBalance
    public class TrailBalance : BusinessObject
    {
        public TrailBalance()
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
        }

        #region Properties
        public int AccountHeadID { get; set; }
        public string AccountCode { get; set; }
        public string AccountHeadName { get; set; }
        public int ParentAccountHeadID { get; set; }
        public EnumComponentType ComponentType { get; set; }
        public EnumAccountType AccountType { get; set; }
        public double OpenningBalance { get; set; }
        public double DebitAmount { get; set; }
        public double CreditAmount { get; set; }
        public double ClosingBalance { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public int BusinessUnitID { get; set; }
        public string ErrorMessage { get; set; }
        public int AccountTypeInInt { get; set; }
        #endregion

        #region Derived Property
        public string TotalDebitAmount { get; set; }
        public string TotalCreditAmount { get; set; }
        public string DateType { get; set; }

        public string DebitAmountInString
        {
            get
            {
                if (this.DebitAmount == 0)
                {
                    return "-";
                }
                else
                {
                    return Global.MillionFormat(this.DebitAmount);
                }
            }
        }
        public string CreditAmountInString
        {
            get
            {
                if (this.CreditAmount == 0)
                {
                    return "-";
                }
                else
                {
                    return Global.MillionFormat(this.CreditAmount);
                }
            }
        }

        #region OpeningBalanceSt
        public string OpeningBalanceSt
        {
            get
            {
                string sOpeningBalance = "";
                if (this.OpenningBalance == 0)
                {
                    sOpeningBalance = "-";
                }
                else if (this.OpenningBalance < 0)
                {
                    sOpeningBalance = "(" + Global.MillionFormat(this.OpenningBalance * (-1)) + ")";
                }
                else
                {
                    sOpeningBalance = Global.MillionFormat(Math.Abs(this.OpenningBalance));
                }
                return sOpeningBalance;
            }
        }
        #endregion
        #region ClosingBalanceSt
        public string ClosingBalanceSt
        {
            get
            {
                string sClosingCrBalance = "";
                if (this.ClosingBalance == 0)
                {
                    sClosingCrBalance = "-";
                }
                else if (this.ClosingBalance < 0)
                {
                    sClosingCrBalance = "(" + Global.MillionFormat(this.ClosingBalance * (-1)) + ")";
                }
                else
                {
                    sClosingCrBalance = Global.MillionFormat(Math.Abs(this.ClosingBalance));
                }
                return sClosingCrBalance;
            }
        }
        #endregion
        public List<TrailBalance> TrailBalances { get; set; }
        public List<AccountingSession> AccountingYears { get; set; }
        public Company Company { get; set; }

        public string StartDateSt { get { return this.StartDate.ToString("dd MMM yyyy"); } }
        public string EndDateSt { get { return this.EndDate.ToString("dd MMM yyyy"); } }
        private string _sAccountTypeSt = "";
        public string AccountTypeSt { get { _sAccountTypeSt = this.AccountType.ToString(); return _sAccountTypeSt; } set { _sAccountTypeSt = value == null ? "" : value; } }
        #endregion

        #region Functions
        public static List<TrailBalance> Gets(int nAccountHead, int AccountTypeInInt, DateTime dStartDate, DateTime dEndDate, int nBusinessUnitID, int nUserID)
        {
            return TrailBalance.Service.Gets(nAccountHead, AccountTypeInInt, dStartDate, dEndDate, nBusinessUnitID, nUserID);
        }
        public static List<TrailBalance> ProcessTrialBalance(DateTime dStartDate, DateTime dEndDate, string sStartLedgerCode, string sEndLedgerCode, string sSQL, int nUserID)
        {
            return TrailBalance.Service.ProcessTrialBalance(dStartDate, dEndDate, sStartLedgerCode, sEndLedgerCode, sSQL, nUserID);
        }
        #endregion

        #region ServiceFactory
        internal static ITrailBalanceService Service
        {
            get { return (ITrailBalanceService)Services.Factory.CreateService(typeof(ITrailBalanceService)); }
        }
        #endregion
    }
    #endregion

    #region ITrailBalance interface
    public interface ITrailBalanceService
    {
        List<TrailBalance> Gets(int nAccountHead, int AccountTypeInInt, DateTime dStartDate, DateTime dEndDate, int nBusinessUnitID, int nUserID);
        List<TrailBalance> ProcessTrialBalance(DateTime dStartDate, DateTime dEndDate, string sStartLedgerCode, string sEndLedgerCode, string sSQL, int nUserID);
    }
    #endregion

    #region TChartsOfAccount
    //this is a derive class that use only for display user menu(j tree tree architecture reference : http://www.jeasyui.com/documentation/index.php# )
    public class TTrailBalance
    {
        public TTrailBalance()
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
        public List<TTrailBalance> children { get; set; }//: an array nodes defines some children nodes
        public List<TTrailBalance> TTrailBalances { get; set; }
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
