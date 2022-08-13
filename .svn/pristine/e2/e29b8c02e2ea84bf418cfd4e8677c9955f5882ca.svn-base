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
    #region AccountHeadConfigure
    public class AccountHeadConfigure : BusinessObject
    {
        public AccountHeadConfigure()
        {
            AccountHeadConfigureID = 0;
            AccountHeadID = 0;
            ReferenceObjectID = 0;
            ReferenceObjectType = EnumVoucherExplanationType.VoucherDetail;
            ReferenceObjectTypeInInt = 0;
            ErrorMessage = "";
            Name = "";
            CostCenterDescription = "";
            ProductCategoryName = "";
            AccountHeadName = "";
            AccountPathName = "";
            IsBillRefApply = false;
            IsOrderRefApply = false;            
        }

        #region Properties
        public int AccountHeadConfigureID { get; set; }
        public int AccountHeadID { get; set; }
        public int ReferenceObjectID { get; set; }
        public EnumVoucherExplanationType ReferenceObjectType { get; set; }
        public int ReferenceObjectTypeInInt { get; set; }
        public string ErrorMessage { get; set; }
        public string Name { get; set; }
        public string CostCenterDescription { get; set; }
        public string AccountHeadName { get; set; }
        public string AccountPathName { get; set; }
        public string ProductCategoryName { get; set; }
        public bool IsBillRefApply { get; set; }
        public bool IsOrderRefApply { get; set; }
        public string IsBillRefApplySt
        {
            get
            {
                if (this.IsBillRefApply)
                {
                    return "Yes";
                }
                else
                {
                    return "No";
                }
            }
        }
        public string IsOrderRefApplySt
        {
            get
            {
                if (this.IsOrderRefApply)
                {
                    return "Yes";
                }
                else
                {
                    return "No";
                }
            }
        }        
        #endregion

        #region Functions
        public static List<AccountHeadConfigure> Gets(int nUserID)
        {
            return AccountHeadConfigure.Service.Gets(nUserID);
        }
        public static List<AccountHeadConfigure> Gets(string sSQL, int nUserID)
        {
            return AccountHeadConfigure.Service.Gets(sSQL, nUserID);
        }
        public AccountHeadConfigure Get(int id, int nUserID)
        {
            return AccountHeadConfigure.Service.Get(id, nUserID);
        }
        public AccountHeadConfigure Save(int nUserID)
        {
            return AccountHeadConfigure.Service.Save(this, nUserID);
        }
        public string Delete(int id, int nUserID)
        {
            return AccountHeadConfigure.Service.Delete(id, nUserID);
        }
        public static List<AccountHeadConfigure> Gets(int eEnumVoucherExplanationType, int nAccountHeadID, int nUserID)
        {
            return AccountHeadConfigure.Service.Gets(eEnumVoucherExplanationType, nAccountHeadID, nUserID);
        }
        #endregion

        #region ServiceFactory
        internal static IAccountHeadConfigureService Service
        {
            get { return (IAccountHeadConfigureService)Services.Factory.CreateService(typeof(IAccountHeadConfigureService)); }
        }
        #endregion
    }
    #endregion

    #region IBank interface
    public interface IAccountHeadConfigureService
    {
        List<AccountHeadConfigure> Gets(int nUserID);
        List<AccountHeadConfigure> Gets(int eEnumVoucherExplanationType, int nAccountHeadID, int nUserID);
        List<AccountHeadConfigure> Gets(string sSQL, int nUserID);
        AccountHeadConfigure Get(int id, int nUserID);
        AccountHeadConfigure Save(AccountHeadConfigure oAccountHeadConfigure, int nUserID);
        string Delete(int id, int nUserID);
    }
    #endregion
}
