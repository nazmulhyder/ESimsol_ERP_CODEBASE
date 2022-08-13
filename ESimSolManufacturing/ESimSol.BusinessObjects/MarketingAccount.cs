using System;
using System.IO;
using ESimSol.BusinessObjects;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Collections.Generic;
using ICS.Core.Framework;
using ICS.Core.Utility;
using System.Data;


namespace ESimSol.BusinessObjects
{
    #region MarketingAccount
    public class MarketingAccount : BusinessObject
    {
        public MarketingAccount()
        {
            MarketingAccountID = 0;
          
            Name = "";
            EmployeeID= 0;
            Phone= "";
            Email= "";
            ShortName = "";
            Phone2 = "";
            Note = "";
            MultipleItemReturn = false;
            IsGroup = false;
            Activity = true;
            EmployeeCode = "";
            GroupID = 0;
            UserID = 0;
            GroupName = "";
            UserName = "";
            Name_Group = "";
           
        }

        #region Properties
        public int MarketingAccountID { get; set; }
        public string Name { get; set; }
        public int EmployeeID { get; set; }
        public int GroupID { get; set; }
        public int UserID { get; set; }
        public string EmployeeCode { get; set; }
        public string Phone { get; set; }
        public string Phone2{get;set;}
        public string Email { get; set; }
        public bool IsGroup { get; set; }
        public bool IsGroupHead { get; set; }
        public bool Activity { get; set; }
        public string ShortName { get; set; }
        public string Note { get; set; }
        public string ErrorMessage { get; set; }
        public string GroupName { get; set; }
        public string UserName { get; set; }
        public string NameCode
        {
            get
            {
                 string sNameCode = "";
                if (this.MarketingAccountID > 0)
                {
                    sNameCode = this.Name + "[" + this.MarketingAccountID.ToString() + "]";
                }
                else
                {
                    sNameCode = this.Name;
                }
                return sNameCode;
            }           
        }
        public string ActivitySt
        {
            get
            {
                 if(this.Activity== true)
                 {
                     return "Active";
                 }
                 else{
                     return "In Active";
                 }
            }
        }
        public string GroupHeadSt
        {
            get
            {
                if (this.IsGroupHead == true)
                {
                    return "Group Head";
                }
                else
                {
                    return "--";
                }
            }
        }
        public string Name_Group { get; set; }
        #endregion

        #region Derived Property
        public List<BusinessUnit> BusinessUnits { get; set; }
        public List<MarketingAccount_BU> MarketingAccount_BUs { get; set; }
        public bool MultipleItemReturn { get; set; }
        public List<MarketingAccount> MarketingAccounts { get; set;}
      
        #endregion

        #region Functions
        public static List<MarketingAccount> GetsByName(string sName, int nBUID, int nUserID)
        {
            return MarketingAccount.Service.GetsByName(sName, nBUID, nUserID);
        }
        public static List<MarketingAccount> GetsByNameGroup(string sName, int nBUID, int nUserID)
        {
            return MarketingAccount.Service.GetsByNameGroup(sName, nBUID, nUserID);
        }
        public MarketingAccount CommitActivity(int id, bool ActiveInActive, int nUserID)
        {
            return MarketingAccount.Service.CommitActivity(id, ActiveInActive, nUserID);
        }
        public MarketingAccount GroupActivity(int id,string sMarketingIDs, bool ActiveInActive, int nUserID)
        {
            return MarketingAccount.Service.GroupActivity(id,sMarketingIDs, ActiveInActive, nUserID);
        }
        public MarketingAccount GetByUser( int nUserID)
        {
            return MarketingAccount.Service.GetByUser( nUserID);
        }
        public MarketingAccount Get(int id, int nUserID)
        {
            return MarketingAccount.Service.Get(id, nUserID);
        }
        public MarketingAccount Save(int nUserID)
        {
            return MarketingAccount.Service.Save(this, nUserID);
        }
        public string Delete(int id, int nUserID)
        {
            return MarketingAccount.Service.Delete(id, nUserID);
        }
        public static List<MarketingAccount> Gets(int nUserID)
        {
            return MarketingAccount.Service.Gets(nUserID);
        }
        public static List<MarketingAccount> GetsByBU(int nBUID, int nUserID)
        {
            return MarketingAccount.Service.GetsByBU(nBUID, nUserID);
        }
        public static List<MarketingAccount> GetsByBUAndGroup(int nBUID, int nUserID)
        {
            return MarketingAccount.Service.GetsByBUAndGroup(nBUID, nUserID);
        }
        public static List<MarketingAccount> Gets(string sSQL, int nUserID)
        {
            return MarketingAccount.Service.Gets(sSQL, nUserID);
        }
        public static List<MarketingAccount> GetsGroup(string sName, int nBUID, int nUserID)
        {
            return MarketingAccount.Service.GetsGroup(sName,nBUID, nUserID);
        }
        public static List<MarketingAccount> GetsByUser(int nBUID, int nUserID)
        {
            return MarketingAccount.Service.GetsByUser(nBUID, nUserID);
        }
        public static List<MarketingAccount> GetsGroupHead(string sName, int nBUID, int nUserID)
        {
            return MarketingAccount.Service.GetsGroupHead( sName,  nBUID,  nUserID);
        }
        #endregion


        #region ServiceFactory
        internal static IMarketingAccountService Service
        {
            get { return (IMarketingAccountService)Services.Factory.CreateService(typeof(IMarketingAccountService)); }
        }
        #endregion
    }
    #endregion

    #region IMarketingAccount interface
    public interface IMarketingAccountService
    {
        MarketingAccount Get(int id, int nUserID);
        MarketingAccount GetByUser(int nUserID);
        MarketingAccount CommitActivity(int id, bool ActiveInActive, int nUserID);
        MarketingAccount GroupActivity(int id,string sMarketingIDs, bool ActiveInActive, int nUserID); 
        List<MarketingAccount> Gets(int nUserID);
        List<MarketingAccount> GetsByName(string sName, int nBUID, int nUserID);
        List<MarketingAccount> GetsByNameGroup(string sName, int nBUID, int nUserID);
        List<MarketingAccount> GetsGroup(string sName, int nBUID, int nUserID);
        List<MarketingAccount> GetsByBU(int nBUID, int nUserID);
        List<MarketingAccount> GetsByBUAndGroup(int nBUID, int nUserID);
        List<MarketingAccount> GetsByUser(int nBUID, int nUserID);
        List<MarketingAccount> GetsGroupHead(string sName, int nBUID, int nUserID);
        List<MarketingAccount> Gets(string sSQL, int nUserID);
        string Delete(int id, int nUserID);
        MarketingAccount Save(MarketingAccount oMarketingAccount, int nUserID);
        
       
    }
    #endregion
}