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
    #region CompanyRuleName

    public class CompanyRuleName : BusinessObject
    {
        public CompanyRuleName()
        {

            CRNID = 0;
            Description = "";
            IsActive = true;
            ErrorMessage = "";

        }

        #region Properties

        public int CRNID { get; set; }

        public string Description { get; set; }

        public bool IsActive { get; set; }

        public string ErrorMessage { get; set; }

        #endregion

        #region Derived Property
        public string EncryptedID { get; set; }
        public string ActivityInString { get { if (IsActive)return "Active"; else return "Inactive"; } }
        public List<CompanyRuleDescription> CompanyRuleDescriptions  { get; set; }
        public List<CompanyRuleName> CompanyRuleNames { get; set; }
        public Employee Employee { get; set; }
        public Company Company { get; set; }
        #endregion

        #region Functions
        public static CompanyRuleName Get(int Id, long nUserID)
        {
            return CompanyRuleName.Service.Get(Id, nUserID);
        }
        public static CompanyRuleName Get(string sSQL, long nUserID)
        {
            return CompanyRuleName.Service.Get(sSQL, nUserID);
        }
        public static List<CompanyRuleName> Gets(long nUserID)
        {
            return CompanyRuleName.Service.Gets(nUserID);
        }

        public static List<CompanyRuleName> Gets(string sSQL, long nUserID)
        {
            return CompanyRuleName.Service.Gets(sSQL, nUserID);
        }

        public CompanyRuleName IUD(int nDBOperation, long nUserID)
        {
            return CompanyRuleName.Service.IUD(this, nDBOperation, nUserID);
        }

        public static CompanyRuleName Activite(int nId, long nUserID)
        {
            return CompanyRuleName.Service.Activite(nId,  nUserID);
        }
        #endregion

        #region ServiceFactory
        internal static ICompanyRuleNameService Service
        {
            get { return (ICompanyRuleNameService)Services.Factory.CreateService(typeof(ICompanyRuleNameService)); }
        }

        #endregion
    }
    #endregion

    #region ICompanyRuleName interface

    public interface ICompanyRuleNameService
    {
        CompanyRuleName Get(int id, Int64 nUserID);
        CompanyRuleName Get(string sSQL, Int64 nUserID);
        List<CompanyRuleName> Gets(Int64 nUserID);
        List<CompanyRuleName> Gets(string sSQL, Int64 nUserID);
        CompanyRuleName IUD(CompanyRuleName oCompanyRuleName, int nDBOperation, Int64 nUserID);
        CompanyRuleName Activite(int nId,  Int64 nUserID);
    }
    #endregion
}
