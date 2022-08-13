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
    #region BankBranch
    
    public class BankBranch : BusinessObject
    {
        public BankBranch()
        {
            BankBranchID = 0;
            BankID = 0;
            BranchCode = "";
            BranchName = "";
            Address = "";
            SwiftCode = "";
            PhoneNo = "";
            FaxNo = "";
            Note = "";
            IsOwnBank = true;
            IsActive = true;
            BankName = "";
            ErrorMessage = "";
            BankPersonnels = new List<BankPersonnel>();
        }

        #region Properties
        
        public int BankBranchID { get; set; }
        public int BankID { get; set; }
        public string BranchCode { get; set; }
        public string BranchName { get; set; }
        public string Address { get; set; }
        public string SwiftCode { get; set; }
        public string PhoneNo { get; set; }
        public string FaxNo { get; set; }
        public string Note { get; set; }
        public bool IsOwnBank { get; set; }
        public bool IsActive { get; set; }
        public string BankName { get; set; }
        public string ErrorMessage { get; set; }

        #endregion

        #region Derived Property
        public string SelectedBank { get; set; }
        public int BUID { get; set; }
        public string DeptIDs { get; set; }
        public List<BankBranch> BankBranchForSelectedBank { get; set; }
        public List<Bank> Banks { get; set; }
        public List<BankAccount> BankAccounts { get; set; }
        public List<BankPersonnel> BankPersonnels { get; set; }
        public List<BusinessUnit> BusinessUnits { get; set; }
        public List<BankBranchBU> BankBranchBUs { get; set; }
        public List<BankBranchDept> BankBranchDepts { get; set; }
        
        public bool IsHeterogeneous { get; set; }
        public string BankBranchName 
        {
            get 
            {
                return this.BankName + "[" + this.BranchName + "]";
            } 
        }
        #endregion


        #region Functions
        public static List<BankBranch> Gets(Int64 nUserID)
        {
            return BankBranch.Service.Gets(nUserID);
        }
        public static List<BankBranch> Gets(string sSQL, Int64 nUserID)
        {
            return BankBranch.Service.Gets(sSQL,nUserID);
        }
        public static List<BankBranch> GetsOwnBranchs(Int64 nUserID)
        {
            return BankBranch.Service.GetsOwnBranchs(nUserID);
        }
        public static List<BankBranch> GetsByBank(int nBankID, Int64 nUserID)
        {
            return BankBranch.Service.GetsByBank(nBankID,nUserID);
        }
        public static List<BankBranch> GetsByDeptAndBU(string sDeptIDs, int nBUID, string sBankName, Int64 nUserID)
        {
            return BankBranch.Service.GetsByDeptAndBU(sDeptIDs, nBUID,sBankName,  nUserID);
        }
        public static BankBranch Get(int nId, Int64 nUserID)
        {
            return BankBranch.Service.Get(nId,nUserID);
        }
        public BankBranch Save(Int64 nUserID)
        {
            return BankBranch.Service.Save(this,nUserID);
        }
        public string Delete(int nId, Int64 nUserID)
        {
            return BankBranch.Service.Delete(nId,nUserID);
        }

        #endregion

        #region ServiceFactory
        internal static IBankBranchService Service
        {
            get { return (IBankBranchService)Services.Factory.CreateService(typeof(IBankBranchService)); }
        }
        #endregion
    }
    #endregion

    #region BankBranchs
    public class BankBranchs : IndexedBusinessObjects
    {
        #region Collection Class Methods
        public void Add(BankBranch item)
        {
            base.AddItem(item);
        }
        public void Remove(BankBranch item)
        {
            base.RemoveItem(item);
        }
        public BankBranch this[int index]
        {
            get { return (BankBranch)GetItem(index); }
        }
        public int GetIndex(int id)
        {
            return base.GetIndex(new ID(id));
        }
        #endregion
    }
    #endregion

    #region IBankBranch interface
    
    public interface IBankBranchService
    {
        BankBranch Get(int id, Int64 nUserID);        
        List<BankBranch> GetsByBank(int nBankID, Int64 nUserID);
        List<BankBranch> GetsByDeptAndBU(string sDeptIDs, int nBUID, string sBankName, Int64 nUserID);  
        List<BankBranch> Gets(Int64 nUserID);
        List<BankBranch> Gets(string sSQL,Int64 nUserID);
        List<BankBranch> GetsOwnBranchs(Int64 nUserID);        
        string Delete(int id, Int64 nUserID);        
        BankBranch Save(BankBranch oBankBranch, Int64 nUserID);
    }
    #endregion
}

