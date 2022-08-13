using System;
using System.IO;
using ICS.Core;
using ICS.Core.Framework;
using ICS.Core.Utility;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Collections.Generic;


namespace ESimSol.BusinessObjects
{
    #region CompanyRuleDescription

    public class CompanyRuleDescription : BusinessObject
    {
        public CompanyRuleDescription()
        {
            CRDID = 0;
            CRNID = 0;
            Description = "";
            IsActive = true;
            ErrorMessage = "";

        }

        #region Properties
        public int CRDID { get; set; }
        public int CRNID { get; set; }
        public string Description { get; set; }
        public bool IsActive { get; set; }
        public string ErrorMessage { get; set; }

        #endregion

        #region Derived Property
        public string ActivityInString { get { if (IsActive)return "Active"; else return "Inactive"; } }
        public CompanyRuleName CompanyRuleName { get; set; }
        public string Header { get; set; }
        #endregion

        #region Functions
        public static CompanyRuleDescription Get(int Id, long nUserID)
        {
            return CompanyRuleDescription.Service.Get(Id, nUserID);
        }
        public static CompanyRuleDescription Get(string sSQL, long nUserID)
        {
            return CompanyRuleDescription.Service.Get(sSQL, nUserID);
        }
        public static List<CompanyRuleDescription> Gets(long nUserID)
        {
            return CompanyRuleDescription.Service.Gets(nUserID);
        }
        public static List<CompanyRuleDescription> Gets(string sSQL, long nUserID)
        {
            return CompanyRuleDescription.Service.Gets(sSQL, nUserID);
        }
        public CompanyRuleDescription IUD(int nDBOperation, long nUserID)
        {
            return CompanyRuleDescription.Service.IUD(this, nDBOperation, nUserID);
        }
        public static CompanyRuleDescription Activite(int nId, long nUserID)
        {
            return CompanyRuleDescription.Service.Activite(nId,nUserID);
        }
        #endregion

        #region ServiceFactory
        internal static ICompanyRuleDescriptionService Service
        {
            get { return (ICompanyRuleDescriptionService)Services.Factory.CreateService(typeof(ICompanyRuleDescriptionService)); }
        }
        #endregion
    }
    #endregion

    #region ICompany RuleDescription interface

    public interface ICompanyRuleDescriptionService
    {
        CompanyRuleDescription Get(int id, Int64 nUserID);
        CompanyRuleDescription Get(string sSQL, Int64 nUserID);
        List<CompanyRuleDescription> Gets(Int64 nUserID);
        List<CompanyRuleDescription> Gets(string sSQL, Int64 nUserID);
        CompanyRuleDescription IUD(CompanyRuleDescription oCompanyRuleDescription, int nDBOperation, Int64 nUserID);
        CompanyRuleDescription Activite(int nId, Int64 nUserID);
    }
    #endregion
}
