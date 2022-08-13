using System;
using System.IO;
using ESimSol.BusinessObjects;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Collections.Generic;
using ICS.Core.Framework;

namespace ESimSol.BusinessObjects
{  
    #region AccountsBookSetupDetail
    public class AccountsBookSetupDetail : BusinessObject
    {
        public AccountsBookSetupDetail()
        {
            AccountsBookSetupDetailID = 0;
            AccountsBookSetupID = 0;
            AccountHeadID = 0;
            ComponentType = EnumComponentType.None;
            ComponentTypeInt = 0;
            AccountHeadName = "";
            AccountHeadCode = "";
            CategoryName = "";
            ErrorMessage = "";
        }
        
        #region Properties
        public int AccountsBookSetupDetailID { get; set; }
        public int AccountsBookSetupID { get; set; }
        public int AccountHeadID { get; set; }
        public EnumComponentType ComponentType { get; set; }
        public int ComponentTypeInt { get; set; }
        public string AccountHeadName { get; set; }
        public string AccountHeadCode { get; set; }
        public string CategoryName { get; set; }
        public string ErrorMessage { get; set; }

        public string ComponentTypeSt
        {
            get
            {
                return EnumObject.jGet(this.ComponentType);
            }
        }
        #endregion

        #region Functions
        public AccountsBookSetupDetail Get(int id, int nUserID)
        {
            return AccountsBookSetupDetail.Service.Get(id, nUserID);
        }
        public AccountsBookSetupDetail Save(int nUserID)
        {
            return AccountsBookSetupDetail.Service.Save(this, nUserID);
        }
        public string Delete(int id, int nUserID)
        {
            return AccountsBookSetupDetail.Service.Delete(id, nUserID);
        }
        public static List<AccountsBookSetupDetail> Gets(int id, int nUserID)
        {
            return AccountsBookSetupDetail.Service.Gets(id, nUserID);
        }
        public static List<AccountsBookSetupDetail> GetsSQL(string sSQL, int nUserID)
        {
            return AccountsBookSetupDetail.Service.GetsSQL(sSQL, nUserID);
        }
        #endregion

        #region ServiceFactory
        internal static IAccountsBookSetupDetailService Service
        {
            get { return (IAccountsBookSetupDetailService)Services.Factory.CreateService(typeof(IAccountsBookSetupDetailService)); }
        }
        #endregion
    }
    #endregion

    #region IAccountsBookSetupDetail interface
    public interface IAccountsBookSetupDetailService
    {
        AccountsBookSetupDetail Save(AccountsBookSetupDetail oAccountsBookSetupDetail, int nUserID);
        AccountsBookSetupDetail Get(int id, int nUserID);
        List<AccountsBookSetupDetail> Gets(int id, int nUserID);
        List<AccountsBookSetupDetail> GetsSQL(string sSQL, int nUserID);
        string Delete(int id, int nUserID);
    }
    #endregion
}
