using System;
using System.IO;
using ESimSol.BusinessObjects;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Collections.Generic;
using System.Collections;
using ICS.Core.Framework;
using ICS.Core.Utility;


namespace ESimSol.BusinessObjects
{
    #region AccountEffect
    public class AccountEffect : BusinessObject
    {
        public AccountEffect()
        {
            AccountEffectID = 0;
            ModuleName = EnumModuleName.None;
            ModuleNameInt = 0;
            ModuleObjID = 0;
            AccountEffectType = EnumAccountEffectType.None;
            AccountEffectTypeInt = 0;
            DrAccountHeadID = 0;
            CrAccountHeadID = 0;
            Remarks = "";
            DrAccountCode = "";
            DrAccountHeadName = "";
            CrAccountCode = "";
            CrAccountHeadName = "";
            DebitSubLedgerID = 0;
            CreditSubLedgerID = 0;
            DebitSubLedgerCode= "";
		    DebitSubLedgerName= "";
		    CreditSubLedgerCode = "";
            CreditSubLedgerName = "";
            ErrorMessage = "";
            AccountEffects = new List<AccountEffect>();
        }
        #region Properties
        public int AccountEffectID { get; set; }
        public EnumModuleName ModuleName { get; set; }
        public int ModuleNameInt { get; set; }
        public int ModuleObjID { get; set; }
        public EnumAccountEffectType AccountEffectType { get; set; }
        public int AccountEffectTypeInt { get; set; }
        public int DrAccountHeadID { get; set; }
        public int CrAccountHeadID { get; set; }
        public string Remarks { get; set; }
        public string DrAccountCode { get; set; }
        public string DrAccountHeadName { get; set; }
        public string CrAccountCode { get; set; }
        public string CrAccountHeadName { get; set; }
        public int DebitSubLedgerID { get; set; }
        public int CreditSubLedgerID { get; set; }
         public string    DebitSubLedgerCode{ get; set; }
		public string DebitSubLedgerName{ get; set; }
		public string CreditSubLedgerCode { get; set; }
        public string CreditSubLedgerName { get; set; }
        public string ErrorMessage { get; set; }
        public List<AccountEffect> AccountEffects { get; set; }
        public List<EnumObject> AccountEffectTypes { get; set; }
        public string AccountEffectTypeSt
        {
            get 
            {
                return EnumObject.jGet(this.AccountEffectType);
            }
        }
        #endregion

        #region Functions
        public AccountEffect Get(int id, int nCompanyID, int nUserID)
        {
            return AccountEffect.Service.Get(id, nUserID);
        }
        public AccountEffect Save(int nUserID)
        {
            return AccountEffect.Service.Save(this, nUserID);
        }
        public string Delete(int id, int nUserID)
        {
            return AccountEffect.Service.Delete(id, nUserID);
        }
        public static List<AccountEffect> Gets(int nUserID)
        {
            return AccountEffect.Service.Gets(nUserID);
        }
        public static List<AccountEffect> Gets(int nModuleObjID, EnumModuleName eModuleName, int nUserID)
        {
            return AccountEffect.Service.Gets(nModuleObjID, eModuleName, nUserID);
        }
        public static List<AccountEffect> Gets(string sSQL, int nUserID)
        {
            return AccountEffect.Service.Gets(sSQL, nUserID);
        }
        #endregion

        #region ServiceFactory
        internal static IAccountEffectService Service
        {
            get { return (IAccountEffectService)Services.Factory.CreateService(typeof(IAccountEffectService)); }
        }
        #endregion
    }
    #endregion

    #region IAccountEffect interface
    public interface IAccountEffectService
    {
        AccountEffect Save(AccountEffect oAccountEffect, int nUserID);
        AccountEffect Get(int id, int nUserID);
        List<AccountEffect> Gets(int nUserID);
        List<AccountEffect> Gets(int nModuleObjID, EnumModuleName eModuleName, int nUserID);
        List<AccountEffect> Gets(string sSQL, int nUserID);
        string Delete(int id, int nUserID);
    }
    #endregion
}