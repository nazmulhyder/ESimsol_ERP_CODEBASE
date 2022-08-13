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
    #region COA_AccountHeadConfig
    public class COA_AccountHeadConfig : BusinessObject
    {
        public COA_AccountHeadConfig()
        {
            AccountHeadConfigID = 0;
            AccountHeadID = 0;

            IsCostCenterApply = false;
            IsBillRefApply = false;
            IsInventoryApply = false;
            IsOrderReferenceApply = false;
            IsVoucherReferenceApply = false;
            CounterHeadID = 0;
            AccountType = EnumAccountType.None;
            ComponentID = 0;
            AccountHeadName = "";
            AccountCode = "";
            CounterHeadName = "";
            CounterHeadCode = "";
            ErrorMessage = "";
            IDs = "";
            BOAHs = new List<BusinessUnitWiseAccountHead>();
        }

        #region Properties
        public int AccountHeadConfigID { get; set; }
        public int AccountHeadID { get; set; }
        public bool IsCostCenterApply { get; set; }
        public bool IsBillRefApply { get; set; }
        public bool IsInventoryApply { get; set; }
        public bool IsOrderReferenceApply { get; set; }
        public bool IsVoucherReferenceApply { get; set; }
        public int CounterHeadID { get; set; }
        public EnumAccountType AccountType { get; set; }
        public int ComponentID { get; set; }
        public string AccountHeadName { get; set; }
        public string AccountCode { get; set; }
        public string CounterHeadName { get; set; }
        public string CounterHeadCode { get; set; }
        public string ErrorMessage { get; set; }
        public string IDs { get; set; }
        public List<AccountHeadConfigure> AccHeadCostCenters { get; set; }
        public List<AccountHeadConfigure> AccHeadProductCategorys { get; set; }
        public List<BusinessUnitWiseAccountHead> BOAHs { get; set; }        
        public List<ACCostCenter> ACCostCenters { get; set; }
        public List<ProductCategory> ProductCategorys { get; set; }
        #endregion

        #region Functions
        public COA_AccountHeadConfig Get(int nAccountHeadID, int nUserID)
        {
            return COA_AccountHeadConfig.Service.Get(nAccountHeadID, nUserID);
        }
        public COA_AccountHeadConfig Save(int nUserID)
        {
            return COA_AccountHeadConfig.Service.Save(this, nUserID);
        }
        public string Delete(int id, int nUserID)
        {
            return COA_AccountHeadConfig.Service.Delete(id, nUserID);
        }
        public static List<COA_AccountHeadConfig> Gets(int nCompanyID, int nUserID)
        {
            return COA_AccountHeadConfig.Service.Gets(nCompanyID, nUserID);
        }
        public static List<COA_AccountHeadConfig> Gets(string sSQL, int nUserID)
        {
            return COA_AccountHeadConfig.Service.Gets(sSQL, nUserID);
        }
        #endregion

        #region ServiceFactory
        internal static ICOA_AccountHeadConfigService Service
        {
            get { return (ICOA_AccountHeadConfigService)Services.Factory.CreateService(typeof(ICOA_AccountHeadConfigService)); }
        }
        #endregion
    }
    #endregion

    #region ICOA_AccountHeadConfig interface
    public interface ICOA_AccountHeadConfigService
    {
        COA_AccountHeadConfig Save(COA_AccountHeadConfig oCOA_AccountHeadConfig, int nUserID);
        COA_AccountHeadConfig Get(int nAccountHeadID, int nUserID);
        List<COA_AccountHeadConfig> Gets(int nCompanyID, int nUserID);
        List<COA_AccountHeadConfig> Gets(string sSQL, int nUserID);
        string Delete(int id, int nUserID);
    }
    #endregion
}