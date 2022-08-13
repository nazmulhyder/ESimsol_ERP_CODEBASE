using System;
using System.IO;
using ESimSol.BusinessObjects;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Collections.Generic;
using ICS.Core.Framework;

namespace ESimSol.BusinessObjects
{

    #region StatementSetup
    public class StatementSetup : BusinessObject
    {
        public StatementSetup()
        {
            StatementSetupID = 0;
            StatementSetupName = "";
            AccountsHeadDefineNature = EnumAccountsHeadDefineNature.None;
            Note = "";            
            ErrorMessage = "";

        }

        #region Properties
        public int StatementSetupID { get; set; }
        public string StatementSetupName { get; set; }
        public EnumAccountsHeadDefineNature AccountsHeadDefineNature { get; set; }
        public string Note { get; set; }        
        public string ErrorMessage { get; set; }
        #endregion

        #region Derived Property
        public List<OperationCategorySetup> OperationCategorySetups { get; set; }
        public List<LedgerBreakDown> LedgerBreakDowns { get; set; }
        #endregion

        #region Functions
        public StatementSetup Get(int id, int nUserID)
        {
            return StatementSetup.Service.Get(id, nUserID);
        }
        public StatementSetup Save(int nUserID)
        {
            return StatementSetup.Service.Save(this, nUserID);
        }
        public string Delete(int id, int nUserID)
        {
            return StatementSetup.Service.Delete(id, nUserID);
        }
        public static List<StatementSetup> Gets(int nUserID)
        {
            return StatementSetup.Service.Gets(nUserID);
        }
        public static List<StatementSetup> Gets(string sSQL, int nUserID)
        {
            return StatementSetup.Service.Gets(sSQL, nUserID);
        }
        #endregion

   
        #region ServiceFactory
        internal static IStatementSetupService Service
        {
            get { return (IStatementSetupService)Services.Factory.CreateService(typeof(IStatementSetupService)); }
        }
        #endregion
    }
    #endregion

    #region Report Study
    public class StatementSetupList : List<StatementSetup>
    {
        public string ImageUrl { get; set; }
    }
    #endregion

    #region IStatementSetup interface
    public interface IStatementSetupService
    {
        StatementSetup Get(int id, int nUserID);
        List<StatementSetup> Gets(int nUserID);
        string Delete(int id, int nUserID);
        StatementSetup Save(StatementSetup oStatementSetup, int nUserID);
        List<StatementSetup> Gets(string sSQL, int nUserID);
    }
    #endregion
    
  
}
