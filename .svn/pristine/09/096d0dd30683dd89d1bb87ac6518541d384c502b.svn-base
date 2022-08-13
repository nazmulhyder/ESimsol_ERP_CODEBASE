using System;
using System.IO;
using ICS.Core;
using ICS.Core.Framework;
using ICS.Core.Utility;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Collections.Generic;


namespace ESimSol.BusinessObjects
{
    #region EmployeeBankAccount

    public class EmployeeBankAccount : BusinessObject
    {
        public EmployeeBankAccount()
        {
            EmployeeBankACID = 0;
            EmployeeID = 0;
            BankBranchID = 0;
            AccountName = "";
            AccountNo = "";
            AccountType = EnumBankAccountType.None;
            SwiftCode = "";
            BankName = "";
            Description = "";
            IsActive = true;
            BankBranchs = new List<BankBranch>();
            ErrorMessage = "";
            BranchName = "";
            BankAddress = "";
        }

        #region Properties
        public int EmployeeBankACID { get; set; }
        public int EmployeeID { get; set; }
        public int BankBranchID { get; set; }
        public string AccountName { get; set; }
        public string AccountNo { get; set; }
        public EnumBankAccountType AccountType { get; set; }
        public string SwiftCode { get; set; }
        public string BankAddress { get; set; }
        public string Description { get; set; }
        public bool IsActive { get; set; }
        public string BankName { get; set; }
        public string BranchName { get; set; }
        public string ErrorMessage { get; set; }
        #endregion

        #region Derive
        public string BankBranchName { get; set; }
        public int AccountTypeInt { get; set; }
        public string Activity { get { if (this.IsActive)return "Active"; else return "Inactive"; } }
        public List<BankBranch> BankBranchs { get; set; }
        #endregion

        #region Functions

        public static List<EmployeeBankAccount> Gets(int nEmployeeID, long nUserID)
        {
            return EmployeeBankAccount.Service.Gets(nEmployeeID, nUserID);
        }

        public static List<EmployeeBankAccount> Gets(string sSql, long nUserID)
        {
            return EmployeeBankAccount.Service.Gets(sSql, nUserID);
        }

        public EmployeeBankAccount Get(int id, long nUserID) //EmployeeBankACID 
        {
            return EmployeeBankAccount.Service.Get(id, nUserID);
        }

        public EmployeeBankAccount IUD(int nDBOperation, long nUserID)
        {
            return EmployeeBankAccount.Service.IUD(this, nDBOperation, nUserID);
        }

        #endregion

        #region ServiceFactory
        internal static IEmployeeBankAccountService Service
        {
            get { return (IEmployeeBankAccountService)Services.Factory.CreateService(typeof(IEmployeeBankAccountService)); }
        }

        #endregion
    }
    #endregion

    #region Report Study
    public class EmployeeBankAccountList : List<EmployeeBankAccount>
    {
        public string ImageUrl { get; set; }
    }
    #endregion

    #region IEmployeeBankAccount interface

    public interface IEmployeeBankAccountService
    {
        EmployeeBankAccount Get(int id, Int64 nUserID);//EmployeeBankACID 
        List<EmployeeBankAccount> Gets(int nEmployeeID, Int64 nUserID);
        EmployeeBankAccount IUD(EmployeeBankAccount oEmployeeBankAccount, int nDBOperation, Int64 nUserID);
        List<EmployeeBankAccount> Gets(string sSql, Int64 nUserID);
    }
    #endregion
}