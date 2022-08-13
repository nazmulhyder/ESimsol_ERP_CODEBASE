using System;
using System.IO;
using ESimSol.BusinessObjects;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Collections.Generic;
using ICS.Core.Framework;

namespace ESimSol.BusinessObjects
{
    #region AccountsBookSetup
    public class AccountsBookSetup : BusinessObject
    {
        public AccountsBookSetup()
        {
            AccountsBookSetupID = 0;
            AccountsBookSetupName = "";
            MappingType = EnumACMappingType.None;
            MappingTypeInt = 0;
            Note = "";
            ErrorMessage = "";
            AccountsBookSetupDetails = new List<AccountsBookSetupDetail>();
        }
        #region Properties
        public int AccountsBookSetupID { get; set; }
        public string AccountsBookSetupName { get; set; }
        public EnumACMappingType MappingType { get; set; }
        public int MappingTypeInt { get; set; }
        public string Note { get; set; }
        public string ErrorMessage { get; set; }
        public List<AccountsBookSetupDetail> AccountsBookSetupDetails { get; set; }
        #endregion

        #region Derived Property
        public string MappingTypeSt
        {
            get
            {
                return EnumObject.jGet(this.MappingType);
            }
        }
        #endregion

        #region Functions
        public AccountsBookSetup Get(int id,int nUserID)
        {
            return AccountsBookSetup.Service.Get(id, nUserID);
        }
        public AccountsBookSetup Save(int nUserID)
        {
            return AccountsBookSetup.Service.Save(this, nUserID);
        }
        public string Delete(int id,int nUserID)
        {
            return AccountsBookSetup.Service.Delete(id,nUserID);
        }
        public static List<AccountsBookSetup> Gets(int nUserID)
        {
            return AccountsBookSetup.Service.Gets(nUserID);
        }
        public static List<AccountsBookSetup> Gets(string sSQL, int nUserID)
        {
            return AccountsBookSetup.Service.Gets(sSQL, nUserID);
        }

        #endregion

        #region ServiceFactory
        internal static IAccountsBookSetupService Service
        {
            get { return (IAccountsBookSetupService)Services.Factory.CreateService(typeof(IAccountsBookSetupService)); }
        }
        #endregion
    }
    #endregion

    #region IAccountsBookSetup interface
    public interface IAccountsBookSetupService
    {
        AccountsBookSetup Save(AccountsBookSetup oAccountsBookSetup, int nUserID);
        AccountsBookSetup Get(int id, int nUserID);
        List<AccountsBookSetup> Gets(int nUserID);
        List<AccountsBookSetup> Gets(string sSQL, int nUserID);
        string Delete(int id,int nUserID);
    }
    #endregion
}
