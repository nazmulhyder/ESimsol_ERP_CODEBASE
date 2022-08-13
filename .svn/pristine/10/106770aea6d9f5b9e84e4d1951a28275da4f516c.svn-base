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
    #region AccountingRatioSetup
    public class AccountingRatioSetup : BusinessObject
    {
        public AccountingRatioSetup()
        {
            AccountingRatioSetupID = 0;
            Name = "";
            RatioFormat = EnumRatioFormat.None;
            DivisibleName = "";
            DividerName = "";
            Remarks = "";
            RatioSetupType = EnumRatioSetupType.GenrealSetup;
            RatioSetupTypeInt = (int)EnumRatioSetupType.GenrealSetup;
            AccountingRatioSetupDetails = new List<AccountingRatioSetupDetail>();
            Divisibles = new List<AccountingRatioSetupDetail>();
            Dividers = new List<AccountingRatioSetupDetail>();
            ErrorMessage = "";
        }
        #region Properties
        public int AccountingRatioSetupID { get; set; }
        public string Name { get; set; }
        public EnumRatioFormat RatioFormat { get; set; }
        public string DivisibleName { get; set; }
        public string DividerName { get; set; }
        public string Remarks { get; set; }
        public EnumRatioSetupType RatioSetupType { get; set; }
        public int RatioSetupTypeInt { get; set; }
        public string ErrorMessage { get; set; }
        #endregion

        #region Derived Property
        public List<AccountingRatioSetupDetail> AccountingRatioSetupDetails { get; set; }
        public List<AccountingRatioSetupDetail> Divisibles { get; set; }
        public List<AccountingRatioSetupDetail> Dividers { get; set; }

        public string RatioFormatSt { get { return EnumObject.jGet(this.RatioFormat); } }
        #endregion

        #region Functions

        public AccountingRatioSetup Get(int id, int nUserID)
        {
            return AccountingRatioSetup.Service.Get(id, nUserID);
        }
        public AccountingRatioSetup Save(int nUserID)
        {
            return AccountingRatioSetup.Service.Save(this, nUserID);
        }
        public string Delete(int id, int nUserID)
        {
            return AccountingRatioSetup.Service.Delete(id, nUserID);
        }
        public static List<AccountingRatioSetup> Gets(int nUserID)
        {
            return AccountingRatioSetup.Service.Gets(nUserID);
        }
       
        public static List<AccountingRatioSetup> Gets(string sSQL, int nUserID)
        {
            return AccountingRatioSetup.Service.Gets(sSQL, nUserID);
        }
        #endregion


        #region ServiceFactory
        internal static IAccountingRatioSetupService Service
        {
            get { return (IAccountingRatioSetupService)Services.Factory.CreateService(typeof(IAccountingRatioSetupService)); }
        }
        #endregion
    }
    #endregion

    #region IAccountingRatioSetup interface
    public interface IAccountingRatioSetupService
    {
        AccountingRatioSetup Get(int id, int nUserID);
        List<AccountingRatioSetup> Gets(int nUserID);
       
        string Delete(int id, int nUserID);
        AccountingRatioSetup Save(AccountingRatioSetup oAccountingRatioSetup, int nUserID);

        List<AccountingRatioSetup> Gets(string sSQL, int nUserID);


    }
    #endregion


}