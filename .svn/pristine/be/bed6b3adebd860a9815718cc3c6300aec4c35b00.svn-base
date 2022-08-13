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
    #region NegativeLedger
    public class NegativeLedger : BusinessObject
    {
        public NegativeLedger()
        {
            AccountHeadID = 0;
            AccountCode = "";
            AccountHeadName = "";
            ComponentType = EnumComponentType.None;
            ComponentTypeInInt = 0;
            AccountType = EnumAccountType.None;
            AccountTypeInInt = 0;
            OpenningBalance = 0;
            DebitAmount  = 0;
            CreditAmount = 0;
            ClosingBalance = 0;
            AccountingSessionID = 0;
            AccountingSessionName = "";
        }

        #region Properties
        public int AccountHeadID { get; set; }
        public string AccountCode { get; set; }
        public string AccountHeadName { get; set; }
        public EnumComponentType ComponentType { get; set; }
        public EnumAccountType AccountType { get; set; }
        public double OpenningBalance { get; set; }
        public double DebitAmount { get; set; }
        public double CreditAmount { get; set; }
        public double ClosingBalance { get; set; }
        public int AccountingSessionID { get; set; }
        public string AccountingSessionName { get; set; }
        public int ComponentTypeInInt { get; set; }
        public int AccountTypeInInt { get; set; }
        public List<NegativeLedger> NegativeLedgers { get; set; }
        public Company Company { get; set; }
        #endregion

        #region Derived Property
        public string TotalDebitAmount { get; set; }
        public string TotalCreditAmount { get; set; }
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
        #endregion

        #region Functions
        public static List<NegativeLedger> Gets(int nCompanyID, int nUserID)
        {
            return NegativeLedger.Service.Gets(nCompanyID, nUserID);
        }
        #endregion

        #region ServiceFactory
        internal static INegativeLedgerService Service
        {
            get { return (INegativeLedgerService)Services.Factory.CreateService(typeof(INegativeLedgerService)); }
        }
        #endregion

    }
    #endregion

    #region INegativeLedger interface
    public interface INegativeLedgerService
    {
        List<NegativeLedger> Gets(int nCompanyID, int nUserID);
    }
    #endregion
}
