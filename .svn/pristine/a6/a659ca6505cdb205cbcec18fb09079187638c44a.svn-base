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
    #region ChartsOfAccount
    public class ChartsOfAccount : BusinessObject
    {
        public ChartsOfAccount()
        {
            AccountHeadID = 0;
            DAHCID = 0;
            AccountCode = "";
            AccountHeadName = "";
            AccountType = EnumAccountType.None;
            CurrencyID = 0;
            ReferenceObjectID = 0;
            Description = "";
            IsJVNode = true;
            IsDynamic = false;
            ParentHeadID = 0;
            ParentHeadName = "";
            PathName = "";
            ComponentID = 0;
            ErrorMessage = "";
            CName = "";
            CSymbol = "";
            IsCostCenterApply = false;
            IsBillRefApply = false;
            IsChequeApply = false;
            IsInventoryApply = false;
            IsOrderReferenceApply = false;
            ACCostCenters = new List<ACCostCenter>();
            ReferenceType = EnumReferenceType.None;
            VoucherReferenceTypeObjs = new List<EnumObject>();
            AccountTypeObjs = new List<EnumObject>();
            COATemplateID = 0;
            IDs = "";
            AccountHeadCodeName = "";
            ComponentType = "";
            VoucherTypeID = 0;
            IsDebit = false;
            LedgerBalance = "";
            IsPaymentCheque = false;
            InventoryHeadID = 0;
            AccountOperationType = EnumAccountOperationType.General;
            BusinessUnitID = 0;
        }
        
        #region Properties  
        public int AccountHeadID { get; set; }
        public int DAHCID { get; set; }
        public string AccountCode { get; set; }
        public string AccountHeadName { get; set; }
        public EnumAccountType AccountType { get; set; }
        public int CurrencyID { get; set; }
        public int ReferenceObjectID { get; set; }
        public string Description { get; set; }
        public bool IsJVNode { get; set; }
        public bool IsDynamic { get; set; }
        public int ParentHeadID { get; set; }
        public string ParentHeadName { get; set; }
        public string PathName { get; set; }
        public int ComponentID { get; set; }
        public string ComponentType { get; set; }
        public string ErrorMessage { get; set; }
        public double OpeningBalance { get; set; }
        public string CName { get; set; }
        public string CSymbol { get; set; }
        public bool IsCostCenterApply { get; set; }
        public bool IsBillRefApply { get; set; }
        public bool IsChequeApply { get; set; }
        public bool IsInventoryApply { get; set; }
        public bool IsOrderReferenceApply { get; set; }
        public int ReferenceTypeInt { get; set; }
        public int InventoryHeadID { get; set; }
        public string IDs { get; set; }
        public double Amount { get; set; }
        public EnumReferenceType ReferenceType { get; set; }
        public List<EnumObject> AccountTypeObjs { get; set; }
        public AccountsBookSetup AccountsBookSetup { get; set; }
        public List<AccountsBookSetupDetail> AccountsBookSetupDetails { get; set; }
        public int COATemplateID { get; set; }
        public string AccountHeadCodeName { get; set; }
        public int VoucherTypeID { get; set; }
        public bool IsDebit { get; set; }
        public string LedgerBalance { get; set; }
        public bool IsPaymentCheque { get; set; }
        public EnumAccountOperationType AccountOperationType { get; set; }
        public EnumAccountOperationType ParentAccountOperationType { get; set; }

        #region Derived Property
        public IEnumerable<ChartsOfAccount> ChildNodes { get; set; }
        public List<EnumObject> VoucherReferenceTypeObjs { get; set; }        
        public ChartsOfAccount Parent { get; set; }        
        public List<ACCostCenter> ACCostCenters { get; set; }
        public List<AccountHeadConfigure> AccHeadCostCenters { get; set; }
        public List<AccountHeadConfigure> AccHeadProductCategorys { get; set; }
        public Company Company { get; set; }
        public string AccountHeadNameCode 
        {
            get 
            {
                if (this.AccountType == EnumAccountType.Component)
                {
                    return AccountHeadName + " @" + AccountType;
                }
                else
                {
                    return AccountHeadName + "[" + AccountCode + "]" + " @" + AccountType;
                }
            }
        }       
        public string AccountHeadNameType
        {
            get
            {
                return AccountHeadName + " @" + AccountType;
            }
        }
        public string DisplayMessage { get; set; }
        public int BusinessUnitID { get; set; }
        public string AccountTypeInString 
        {
            get
            {
                return EnumObject.jGet(this.AccountType);
            }
        }
        public int AccountTypeInInt { get; set; }
        public string NewAccountCode { get; set; }
        public List<ChartsOfAccount> ChartsOfAccounts { get; set; }
        public TChartsOfAccount TChartsOfAccount { get; set; }
        public List<TChartsOfAccount> TChartsOfAccounts { get; set; }
        public string AccountOperationTypeSt { get { return EnumObject.jGet(this.AccountOperationType); } }
        #endregion

        #endregion

        #region Functions
        public ChartsOfAccount SaveTemplateTree(int nUserID)
        {
            return ChartsOfAccount.Service.SaveTemplateTree(this, nUserID);
        }      
        public ChartsOfAccount Update_DynamicHead(int nUserID)
        {
            return ChartsOfAccount.Service.Update_DynamicHead(this,nUserID);
        }
        public ChartsOfAccount Update_InventoryHead(int nUserID)
        {
            return ChartsOfAccount.Service.Update_InventoryHead(this, nUserID);
        }

        
        public ChartsOfAccount MoveChartOfAccount(int nUserID)
        {
            return ChartsOfAccount.Service.MoveChartOfAccount(this,nUserID);
        }
        public string CopyCOA(string sAccountHeadIDs, int nCompanyID, int nUserID)
        {
            return ChartsOfAccount.Service.CopyCOA(sAccountHeadIDs, nCompanyID, nUserID);
        }
        public static List<ChartsOfAccount> GetsbyAccountsName(string sAccountsName,  int nUserID)
        {
            return ChartsOfAccount.Service.GetsbyAccountsName(sAccountsName,  nUserID);
        }
        public List<ChartsOfAccount> GetsForCOATemplate(int nUserID)
        {
            return ChartsOfAccount.Service.GetsForCOATemplate(nUserID);
        }
        public static List<ChartsOfAccount> AccountHeadGets(int nParentHeadID, bool bIsDebit, int nUserID)
        {
            return ChartsOfAccount.Service.AccountHeadGets(nParentHeadID,bIsDebit,nUserID);
        }
        public static List<ChartsOfAccount> GetRefresh(int nParentHeadID, int nUserID)
        {
            return ChartsOfAccount.Service.GetRefresh(nParentHeadID, nUserID);
        }
        public static List<ChartsOfAccount> SaveCopyAccountHeads(string IDs, int nUserID)
        {
            return ChartsOfAccount.Service.SaveCopyAccountHeads(IDs, nUserID);
        }
        public ChartsOfAccount Get(int id, int nUserID)
        {
            return ChartsOfAccount.Service.Get(id, nUserID);
        }
        public ChartsOfAccount GetByCode(string sAccountCode, int nUserID)
        {
            return ChartsOfAccount.Service.GetByCode(sAccountCode, nUserID);
        }
        public ChartsOfAccount Save(int nUserID)
        {
            return ChartsOfAccount.Service.Save(this, nUserID);
        }
        public string Delete(int id, int nUserID)
        {
            return ChartsOfAccount.Service.Delete(id, nUserID);
        }
        public static string GetAccountCode(int nParentID, int nUserID)
        {
            return ChartsOfAccount.Service.GetAccountCode(nParentID, nUserID);
        }
        public static List<ChartsOfAccount> Gets(int nUserID)
        {
            return ChartsOfAccount.Service.Gets(nUserID);
        }
        public static List<ChartsOfAccount> GetsByParent(int nParentID, int nUserID)
        {
            return ChartsOfAccount.Service.GetsByParent(nParentID, nUserID);
        }
        public static List<ChartsOfAccount> Gets(string sSQL, int nUserID)
        {
            return ChartsOfAccount.Service.Gets(sSQL, nUserID);
        }
        public static List<ChartsOfAccount> GetsByCodeOrName(ChartsOfAccount oChartsOfAccount, int nUserID)
        {
            return ChartsOfAccount.Service.GetsByCodeOrName(oChartsOfAccount, nUserID);
        }
        public static List<ChartsOfAccount> GetsByCodeOrNameWithBU(ChartsOfAccount oChartsOfAccount, int nUserID)
        {
            return ChartsOfAccount.Service.GetsByCodeOrNameWithBU(oChartsOfAccount, nUserID);
        }
        public static List<ChartsOfAccount> GetsByCodeOrNameWithBUForVoucher(ChartsOfAccount oChartsOfAccount, int nUserID)
        {
            return ChartsOfAccount.Service.GetsByCodeOrNameWithBUForVoucher(oChartsOfAccount, nUserID);
        } 

        /// <summary>
        /// multi purpose function
        /// parameters ComponentID,AccountType,AccountHeadCodeName. none compulsory
        /// WILL RETURN DATA ACCORDING TO PROVIDED PARAMETERS. see DA for paramter list
        /// </summary>
        /// <param name="oChartsOfAccount"></param>
        /// <param name="nUserID"></param>
        /// <returns></returns>
        public static List<ChartsOfAccount> GetsByComponentAndCodeName(ChartsOfAccount oChartsOfAccount, int nUserID)
        {
            return ChartsOfAccount.Service.GetsByComponentAndCodeName(oChartsOfAccount, nUserID);
        }
        public static List<ChartsOfAccount> GetsbyAccountTypeOrName(int nAccountType, string sAccountsName, int nUserID)
        {
            return ChartsOfAccount.Service.GetsbyAccountTypeOrName( nAccountType, sAccountsName, nUserID);
        }
        #endregion

        #region ServiceFactory
        internal static IChartsOfAccountService Service
        {
            get { return (IChartsOfAccountService)Services.Factory.CreateService(typeof(IChartsOfAccountService)); }
        }
        #endregion
    }
    #endregion

    #region IChartsOfAccount interface
    public interface IChartsOfAccountService
    {
        ChartsOfAccount Save(ChartsOfAccount oChartsOfAccount, int nUserID);
        ChartsOfAccount SaveTemplateTree(ChartsOfAccount oChartsOfAccount, int nUserID);     
        ChartsOfAccount Update_DynamicHead(ChartsOfAccount oChartsOfAccount, int nUserID);
        ChartsOfAccount Update_InventoryHead(ChartsOfAccount oChartsOfAccount, int nUserID);
        ChartsOfAccount MoveChartOfAccount(ChartsOfAccount oChartsOfAccount, int nUserID);
        string Delete(int id, int nUserID);
        string GetAccountCode(int nParentID, int nUserID);
        string CopyCOA(string sAccountHeadIDs,  int nCompanyID, int nUserID);
        ChartsOfAccount Get(int id, int nUserID);
        ChartsOfAccount GetByCode(string sAccountCode, int nUserID);
        List<ChartsOfAccount> GetsbyAccountsName(string sAccountsName,  int nUserID);
        List<ChartsOfAccount> Gets(int nUserID);
        List<ChartsOfAccount> GetsByParent(int nParentID, int nUserID);        
        List<ChartsOfAccount> GetsForCOATemplate(int nUserID);
        List<ChartsOfAccount> AccountHeadGets(int nVoucherTypeID, bool bIsDebit, int nUserID);
        List<ChartsOfAccount> Gets(string sSQL, int nUserID);
        List<ChartsOfAccount> GetRefresh(int nParentHeadID, int nUserID);
        List<ChartsOfAccount> SaveCopyAccountHeads(string IDs, int nUserID);
        List<ChartsOfAccount> GetsByCodeOrName(ChartsOfAccount oChartsOfAccount, int nUserID);
        List<ChartsOfAccount> GetsByCodeOrNameWithBU(ChartsOfAccount oChartsOfAccount, int nUserID);
        List<ChartsOfAccount> GetsByCodeOrNameWithBUForVoucher(ChartsOfAccount oChartsOfAccount, int nUserID);
        List<ChartsOfAccount> GetsByComponentAndCodeName(ChartsOfAccount oChartsOfAccount, int nUserID);
        List<ChartsOfAccount> GetsbyAccountTypeOrName(int nAccountType,string sAccountsName, int nUserID);
    }
    #endregion

    #region TChartsOfAccount
    //this is a derive class that use only for display user menu(j tree tree architecture reference : http://www.jeasyui.com/documentation/index.php# )
    public class TChartsOfAccount
    {
        public TChartsOfAccount()
        {
            id = 0;
            text = "";
            state = "";
            attributes = "";
            parentid = 0;
            code = "";
            CurrencyID = 0;
            CName = "";
            CSymbol = "";
            Description = "";
            PathName = "";
            ComponentID = 0;
            AccountTypeInString = "";
            AccountTypeInInt = 0;
            IsjvNode = false;
            AccountHeadId = 0;
            COATemplateID = 0;
            InventoryHeadID = 0;
            ErrorMessage = "";
            BUAHID = 0;
            CGSGBalanceInString = "";
            LadgerBalanceInString = "";
            TChartsOfAccounts = new List<TChartsOfAccount>();
        }
        public int id { get; set; }                 //: node id, which is important to load remote data
        public string text { get; set; }            //: node text to show
        public string state { get; set; }           //: node state, 'open' or 'closed', default is 'open'. When set to 'closed', the node have children nodes and will load them from remote site        
        public string attributes { get; set; }      //: custom attributes can be added to a node // attributes set a sting that hold action name & controller namr seperated by '~'
        public int parentid { get; set; }
        public string code { get; set; }
        public int CurrencyID { get; set; }
        public string CName { get; set; }
        public string CSymbol { get; set; }
        public string AccountTypeInString { get; set; }
        public int AccountTypeInInt { get; set; }
        public int InventoryHeadID { get; set; }
        public string PathName { get; set; }        
        public int ComponentID { get; set; }
        public string Description { get; set; }
        public bool IsjvNode { get; set; }
        public bool IsChecked { get; set; }
        public int AccountHeadId { get; set; }
        public int COATemplateID { get; set; }
        public int BUAHID { get; set; }
        public string AOTypeSt { get; set; }
        public int AOTypeInInt { get; set; }
        public int ParentAOTypeInInt { get; set; }
        public string CGSGBalanceInString { get; set; }
        public string LadgerBalanceInString { get; set; }
        public double Amount { get; set; }
        public string ErrorMessage { get; set; }
        public List<TChartsOfAccount> children { get; set; }//: an array nodes defines some children nodes
        public Company Company { get; set; }
        public List<TChartsOfAccount> TChartsOfAccounts { get; set; }
    }
    #endregion
}
