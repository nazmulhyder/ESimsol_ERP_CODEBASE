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
    #region BankAccount
    
    public class BankAccount : BusinessObject
    {
        #region  Constructor
        public BankAccount()
        {
            BankAccountID = 0;
            AccountName = "";
            AccountNo = "";
            BankID = 0;
            BankBranchID = 0;
            AccountType = EnumBankAccountType.None;
            AccountTypeInInt = (int)EnumBankAccountType.None;
            LimitAmount = 0;
            CurrentLimit = 0;
            BusinessUnitID = 0;
            BusinessUnitName = "";
            BusinessUnitCode = "";
            BusinessUnitNameCode = "";
            BankAccountName = "";
            BankName = "";
            BankShortName = "";
            BranchName = "";
            BranchAddress = "";
            BankNameAccountNo = "";
            OperationalDeptInInt = 0;
        }
        #endregion

        #region Properties
        
        public int BankAccountID{get ; set; }       
        
        public string AccountName{get ; set; }
        
        public string AccountNo{get ; set; }
        
        public int BankID{get ; set; }
        
        public int BankBranchID { get; set; }
        
        public EnumBankAccountType AccountType { get; set; }
        public int AccountTypeInInt { get; set; }
        
        public double LimitAmount{get ; set; }
        
        public double CurrentLimit{get ; set; }
        public int BusinessUnitID { get; set; }
        public string BusinessUnitName { get; set; }
        public string BusinessUnitCode { get; set; }
        public string BusinessUnitNameCode { get; set; }
        public string BankAccountName { get; set; }
        public string BankName { get; set; }
        public string BankShortName { get; set; }
        public string BranchName { get; set; }
        public string BranchAddress { get; set; }
        public int OperationalDeptInInt { get; set; }
        public string BankNameAccountNo { get; set; }
        public string ErrorMessage { get; set; }
        
        #endregion

        #region Derived Property
        public string SelectedBank { get; set; }
        
        public string BankNameBranch { get; set; }
        
        public List<BankAccount> BankAccountForSelectedBank { get; set; }
        public List<BankAccount> BankAccountForSelectedBankBranch { get; set; }
        public string AccountTypeInString
        {
            get
            {
                return AccountType.ToString();
            }
        }

        public string DisplayAccoountType
        {
            get
            {
                return AccountNo+"["+BankShortName+"]";
            }
        }

        public string AccountNameandNo
        {
            get
            {
                return AccountName +" ["+ AccountNo+"]";
            }
        }
        
           public string AccountWithBankName
        {
            get
            {
                return this.AccountNo + '[' + this.BankName + ']';
            }
        }
        #endregion

        #region Functions

        public static List<BankAccount> Gets(long nUserID)
        {
            return BankAccount.Service.Gets(nUserID);
        }
        public static List<BankAccount> Gets(string sSQL, long nUserId)
        {
            return BankAccount.Service.Gets(sSQL,nUserId);
        }

        public static List<BankAccount> GetsByBank(int nBankID, long nUserID)
        {
            return BankAccount.Service.GetsByBank(nBankID, nUserID);
        }
        public static List<BankAccount> GetsByBankBranch(int nBankBranchID, long nUserID)
        {
            return BankAccount.Service.GetsByBankBranch(nBankBranchID,nUserID);
        }
        public static List<BankAccount> GetsByDeptAndBU(string sDeptIDs, int nBUID, long nUserID)
        {
            return BankAccount.Service.GetsByDeptAndBU(sDeptIDs, nBUID, nUserID);
        }
        public BankAccount Get(int nId, long nUserID)
        {
            return BankAccount.Service.Get(nId,nUserID);
        }
        public BankAccount GetViaCostCenter(int nCCID, long nUserID)
        {
            return BankAccount.Service.GetViaCostCenter(nCCID, nUserID);
        }
        public BankAccount GetPartyWiseDefultAccount(int nPartyID, long nUserID)
        {
            return BankAccount.Service.GetPartyWiseDefultAccount(nPartyID, nUserID);
        }

        public BankAccount Save(long nUserID)
        {
            return BankAccount.Service.Save(this,nUserID);
        }

        public string Delete(int nId, long nUserID)
        {
            return BankAccount.Service.Delete(nId,nUserID);
        }

        #endregion

        #region ServiceFactory
        internal static IBankAccountService Service
        {
            get { return (IBankAccountService)Services.Factory.CreateService(typeof(IBankAccountService)); }
        }
        #endregion

       
    }
    #endregion

    

    #region IBankAccount interface
    
    public interface IBankAccountService
    {
        
        BankAccount Get(int id, long nUserID);
        BankAccount GetViaCostCenter(int nCCID, long nUserID);
        BankAccount GetPartyWiseDefultAccount(int nPartyID, long nUserID);        
        List<BankAccount> Gets(long nUserID);
        List<BankAccount> Gets(string sSQL, long nUserId);
        List<BankAccount> GetsByBank(int nBankID, long nUserID);
        List<BankAccount> GetsByBankBranch(int nBankBranchID, long nUserID);
        List<BankAccount> GetsByDeptAndBU(string sDeptIDs, int BUID, long nUserID);
        string Delete(int id, long nUserID);
        BankAccount Save(BankAccount oBankAccount, long nUserID);
    }
    #endregion
}
