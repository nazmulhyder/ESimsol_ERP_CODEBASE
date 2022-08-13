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
    #region AccountingRatioSetupDetail
    public class AccountingRatioSetupDetail : BusinessObject
    {
        public AccountingRatioSetupDetail()
        {
            AccountingRatioSetupDetailID = 0;
            AccountingRatioSetupID = 0;
            SubGroupID = 0;
            IsDivisible = false;
            RatioName = "";
            DivisibleName = "";
            DividerName = "";
            SubGroupCode = "";
            SubGroupName = "";
            AccountType = EnumAccountType.None;
            RatioComponent = EnumRatioComponent.None;
            RatioComponentInt = (int)EnumRatioComponent.None;
            RatioSetupTypeInt = 0;
            ComponentID = 0;
            ErrorMessage = "";
        }
        #region Properties
        public int AccountingRatioSetupDetailID { get; set; }
        public int AccountingRatioSetupID { get; set; }
        public int SubGroupID { get; set; }
        public bool IsDivisible { get; set; }
        public string RatioName { get; set; }
        public string DivisibleName { get; set; }
        public string DividerName { get; set; }
        public string SubGroupCode { get; set; }
        public string SubGroupName { get; set; }
        public EnumAccountType AccountType { get; set; }
        public int ComponentID { get; set; }
        public EnumRatioComponent RatioComponent { get; set; }
        public int RatioComponentInt { get; set; }
        public int RatioSetupTypeInt { get; set; }
             
        public string ErrorMessage { get; set; }
        #endregion

        #region Derived Property
      public string RatioComponentSt
        {
            get
            {
                return EnumObject.jGet(this.RatioComponent);
            }
        }
        #endregion

        #region Functions

        public AccountingRatioSetupDetail Get(int id, int nUserID)
        {
            return AccountingRatioSetupDetail.Service.Get(id, nUserID);
        }
        public AccountingRatioSetupDetail Save(int nUserID)
        {
            return AccountingRatioSetupDetail.Service.Save(this, nUserID);
        }
        public string Delete(int id, int nUserID)
        {
            return AccountingRatioSetupDetail.Service.Delete(id, nUserID);
        }
        public static List<AccountingRatioSetupDetail> Gets(int nUserID)
        {
            return AccountingRatioSetupDetail.Service.Gets(nUserID);
        }
        public static List<AccountingRatioSetupDetail> GetsForARS(int nARSID,bool bIsDivisible, int nUserID)
        {
            return AccountingRatioSetupDetail.Service.GetsForARS(nARSID, bIsDivisible, nUserID);
        }
        public static List<AccountingRatioSetupDetail> Gets(string sSQL, int nUserID)
        {
            return AccountingRatioSetupDetail.Service.Gets(sSQL, nUserID);
        }
        #endregion


        #region ServiceFactory
        internal static IAccountingRatioSetupDetailService Service
        {
            get { return (IAccountingRatioSetupDetailService)Services.Factory.CreateService(typeof(IAccountingRatioSetupDetailService)); }
        }
        #endregion
    }
    #endregion



    #region IAccountingRatioSetupDetail interface
    public interface IAccountingRatioSetupDetailService
    {
        AccountingRatioSetupDetail Get(int id, int nUserID);
        List<AccountingRatioSetupDetail> Gets(int nUserID);
        List<AccountingRatioSetupDetail> GetsForARS(int nARSID, bool bIsDivisible, int nUserID);
        string Delete(int id, int nUserID);
        AccountingRatioSetupDetail Save(AccountingRatioSetupDetail oAccountingRatioSetupDetail, int nUserID);

        List<AccountingRatioSetupDetail> Gets(string sSQL, int nUserID);


    }
    #endregion
}