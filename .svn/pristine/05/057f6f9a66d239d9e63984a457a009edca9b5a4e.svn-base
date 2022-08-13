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
    #region AccountsBook
    public class AccountsBook : BusinessObject
    {
        public AccountsBook()
        {
            AccountHeadID = 0;
            AccountCode="";
            AccountHeadName = "";
            AccountType = EnumAccountType.None;
            AccountTypeInInt = 0;
            ComponentType = EnumComponentType.None;
            ComponentTypeInInt = 0;
            MappingType = EnumACMappingType.None;
            MappingTypeInt = 0;
            OpenningBalance = 0;
            DebitAmount = 0;
            CreditAmount = 0;
            ClosingBalance = 0;
            ErrorMessage = "";
            AccountsBookSetupID = 0;
            StartDate = DateTime.Now;
            EndDate = DateTime.Now;
            AccountsBookSetups = new List<AccountsBookSetup>();
            AccountsBookSetupName = "";
            BUID = 0;
            ParentHeadID = 0;
            ParentHeadName = "";
            BUIDs = "0";
            BUName = "Group Accounts";
            IsApproved = false;
        }
        #region Properties
        public int AccountHeadID { get; set; }
        public string AccountCode { get; set; }
        public string AccountHeadName { get; set; }
        public EnumAccountType AccountType { get; set; }
        public int AccountTypeInInt { get; set; }
        public double OpenningBalance { get; set; }
        public double DebitAmount { get; set; }
        public double CreditAmount { get; set; }
        public double ClosingBalance { get; set; }
        public string ErrorMessage { get; set; }
        public EnumComponentType ComponentType { get; set; }
        public int ComponentTypeInInt { get; set; }
        public EnumACMappingType MappingType { get; set; }
        public int ParentHeadID { get; set; }
        public string ParentHeadName { get; set; }
        public int MappingTypeInt { get; set; }
        public int BUID { get; set; }
        public string BUIDs { get; set; }
        public string BUName { get; set; }
        public bool IsApproved { get; set; }
        #endregion

        #region Derive Properties
        public int AccountsBookSetupID { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public List<AccountsBookSetup> AccountsBookSetups { get; set; }
        public List<AccountsBook> AccountsBooks { get; set; }
        public Company Company { get; set; }
        public string AccountsBookSetupName { get; set; }
        public string AccountTypeSt
        {
            get
            {
                return EnumObject.jGet(this.AccountType);
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
                else if (this.OpenningBalance == 0)
                {
                    return "-";
                }
                else
                {
                    return Global.MillionFormat(this.OpenningBalance);
                }
            }
        }
        public string DebitAmountInString
        {
            get
            {
                if (this.DebitAmount < 0)
                {
                    return "(" + Global.MillionFormat(this.DebitAmount * (-1)) + ")";
                }
                else if (this.DebitAmount == 0)
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
                if (this.CreditAmount < 0)
                {
                    return "(" + Global.MillionFormat(this.CreditAmount * (-1)) + ")";
                }
                else if (this.CreditAmount == 0)
                {
                    return "-";
                }
                else
                {
                    return Global.MillionFormat(this.CreditAmount);
                }
            }
        }
        public string ClosingBalanceInString
        {
            get
            {
                if (this.ClosingBalance < 0)
                {
                    return "(" + Global.MillionFormat(this.ClosingBalance * (-1)) + ")";
                }
                else if (this.ClosingBalance == 0)
                {
                    return "-";
                }
                else
                {
                    return Global.MillionFormat(this.ClosingBalance);
                }
            }
        }
        #endregion

        #region Functions
        public static List<AccountsBook> Gets(int nAccountsBookSetupID, DateTime dStartDate, DateTime dEndDate, string BUIDs, bool bApprovedOnly, int nUserID)
        {
            return AccountsBook.Service.Gets(nAccountsBookSetupID, dStartDate, dEndDate, BUIDs, bApprovedOnly, nUserID);
        }
        #endregion

        #region ServiceFactory
        internal static IAccountsBookService Service
        {
            get { return (IAccountsBookService)Services.Factory.CreateService(typeof(IAccountsBookService)); }
        }
        #endregion
    }
    #endregion

    #region IAccountsBook interface
    public interface IAccountsBookService
    {
        List<AccountsBook> Gets(int nAccountsBookSetupID, DateTime dStartDate, DateTime dEndDate, string BUIDs, bool bApprovedOnly, int nUserID);
    }
    #endregion
}
