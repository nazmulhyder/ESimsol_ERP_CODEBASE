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
    #region BankPersonnel
    
    public class BankPersonnel : BusinessObject
    {
        public BankPersonnel()
        {
            BankPersonnelID = 0;
            BankID = 0;
            BankBranchID = 0;
            Name = "";
            Address = "";
            Phone = "";
            Email = "";
            Note= "";
            ErrorMessage = "";
        }

        #region Properties
        
        public int BankPersonnelID { get; set; }
        
        public int BankID { get; set; }
        
        public int BankBranchID { get; set; }
        
        public string Name { get; set; }
        
        public string Address { get; set; }
        
        public string Phone { get; set; }
        
        public string Email { get; set; }
        
        public string Note { get; set; }
        
        public string ErrorMessage { get; set; }
        #endregion

        #region Derived Property
        public string SelectedBank { get; set; }
        public List<BankPersonnel> BankPersonnelForSelectedBank { get; set; }
        public List<BankPersonnel> BankPersonnelForSelectedBankBranch { get; set; }
        #endregion

        #region Functions

        public static List<BankPersonnel> Gets(long nUserID)
        {
            return BankPersonnel.Service.Gets(nUserID);
        }

        public static List<BankPersonnel> GetsByBank(int nBankID, long nUserID)
        {
            return BankPersonnel.Service.GetsByBank(nBankID,nUserID);
        }
        public static List<BankPersonnel> GetsByBankBranch(int nBankBranchID, long nUserID)
        {
            return BankPersonnel.Service.GetsByBankBranch(nBankBranchID,nUserID);
        }
        public BankPersonnel Get(int nId, long nUserID)
        {
            return BankPersonnel.Service.Get(nId,nUserID);
        }

        public BankPersonnel Save(long nUserID)
        {
            return BankPersonnel.Service.Save(this,nUserID);
        }

        public string Delete(int nId, long nUserID)
        {
            return BankPersonnel.Service.Delete(nId,nUserID);
        }

        #endregion

        #region ServiceFactory
        internal static IBankPersonnelService Service
        {
            get { return (IBankPersonnelService)Services.Factory.CreateService(typeof(IBankPersonnelService)); }
        }
        #endregion
    }
    #endregion

    #region BankPersonnels
    public class BankPersonnels : IndexedBusinessObjects
    {
        #region Collection Class Methods
        public void Add(BankPersonnel item)
        {
            base.AddItem(item);
        }
        public void Remove(BankPersonnel item)
        {
            base.RemoveItem(item);
        }
        public BankPersonnel this[int index]
        {
            get { return (BankPersonnel)GetItem(index); }
        }
        public int GetIndex(int id)
        {
            return base.GetIndex(new ID(id));
        }
        #endregion
    }
    #endregion

    #region IBankPersonnel interface
    
    public interface IBankPersonnelService
    {
        
        BankPersonnel Get(int id, long nUserID);
        
        List<BankPersonnel> Gets(long nUserID);
        
        List<BankPersonnel> GetsByBank(int nBankID, long nUserID);
        
        List<BankPersonnel> GetsByBankBranch(int nBankBranchID, long nUserID);
        
        string Delete(int id, long nUserID);
        
        BankPersonnel Save(BankPersonnel oBankPersonnel, long nUserID);
    }
    #endregion
}
