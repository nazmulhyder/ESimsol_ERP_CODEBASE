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
    #region Bank
    
    public class Bank : BusinessObject
    {
        public Bank()
        {
            BankID = 0;
            Code = "";
            Name= "";
            ShortName = "";
            IsActive = true;
            FaxNo = "";
            ChequeSetupID = 0;
            ChequeSetupName = "";

            ErrorMessage = "";
            Banks = new List<Bank>();
        }

        #region Properties
        
        public int BankID { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public string ShortName { get; set; }
        public bool IsActive { get; set; }
        public string FaxNo { get; set; }
        public int ChequeSetupID { get; set; }
        public string ChequeSetupName { get; set; }
        public string ErrorMessage { get; set; }
        #endregion

        #region Derived Property
        public List<BankBranch> BankBranchs { get; set; }
        public List<Bank> Banks { get; set; }
        public Company Company { get; set; }
     
        #endregion

        #region Functions

        public static List<Bank> Gets(long nUserID)
        {
            return Bank.Service.Gets(nUserID);
        }
        public static List<Bank> Gets(string sSQL, Int64 nUserID)
        {
            return Bank.Service.Gets(sSQL, nUserID);
        }
        public Bank Get(int nId, long nUserID)
        {
            return Bank.Service.Get(nId,nUserID);
        }
        public Bank Save(long nUserID)
        {
            return Bank.Service.Save(this, nUserID);
        }
        public string Delete(int nId, long nUserID)
        {
            return Bank.Service.Delete(nId, nUserID);
        }
        public static List<Bank> GetByCategory(bool bCategory, long nUserID)
        {
            return Bank.Service.GetByCategory(bCategory, nUserID);
        }
        public static List<Bank> GetByNegoBank(int nBankID, long nUserID)
        {
            return Bank.Service.GetByNegoBank(nBankID, nUserID);
        }

        #endregion

        #region ServiceFactory
        internal static IBankService Service
        {
            get { return (IBankService)Services.Factory.CreateService(typeof(IBankService)); }
        }
        #endregion
    }
    #endregion

    #region IBank interface
    
    public interface IBankService
    {
        
        Bank Get(int id, long nUserID);
        List<Bank> Gets(long nUserID);
        List<Bank> Gets(string sSQL, Int64 nUserID);
        string Delete(int id, long nUserID);
        Bank Save(Bank oBank, long nUserID);
        List<Bank> GetByCategory(bool bCategory, long nUserID);
        List<Bank> GetByNegoBank(int nBankID, long nUserID);
    }
    #endregion
}