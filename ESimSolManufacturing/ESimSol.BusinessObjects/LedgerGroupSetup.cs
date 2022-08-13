using System;
using System.IO;
using ESimSol.BusinessObjects;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Collections.Generic;
using System.Data;
using ICS.Core.Framework;


namespace ESimSol.BusinessObjects
{
    #region LedgerGroupSetup
    public class LedgerGroupSetup : BusinessObject
    {
        #region  Constructor
        public LedgerGroupSetup()
        {
            LedgerGroupSetupID = 0;
            OCSID = 0;
            Note = "";
            LedgerGroupSetupName = "";            
            ErrorMessage = "";
            IsDr = false;
        }
        #endregion

        #region Properties
        public int LedgerGroupSetupID { get; set; }
        public int OCSID { get; set; }
        public string LedgerGroupSetupName { get; set; }
        public string Note { get; set; }
        public string ErrorMessage { get; set; }        
        public bool IsDr { get; set; }

        #endregion
        
        #region Derived Groups
        public List<LedgerBreakDown> LedgerBreakDowns { get; set; }
        #endregion

        #region Functions
        public static List<LedgerGroupSetup> Gets(int nOSCID, int nUserID)
        {
            return LedgerGroupSetup.Service.Gets(nOSCID, nUserID);
        }
        public LedgerGroupSetup Get(int id, int nUserID)
        {
            return LedgerGroupSetup.Service.Get(id, nUserID);
        }
        public LedgerGroupSetup Save(int nUserID)
        {
            return LedgerGroupSetup.Service.Save(this, nUserID);
        }
        public string Delete(int id, int nUserID)
        {
            return LedgerGroupSetup.Service.Delete(id, nUserID);
        }
        public static List<LedgerGroupSetup> Gets(string sSQL, int nUserID)
        {
            return LedgerGroupSetup.Service.Gets(sSQL, nUserID);
        }
        #endregion

        #region ServiceFactory
        internal static ILedgerGroupSetupService Service
        {
            get { return (ILedgerGroupSetupService)Services.Factory.CreateService(typeof(ILedgerGroupSetupService)); }
        }
        #endregion
    }
    #endregion

    #region ILedgerGroupSetup interface
    public interface ILedgerGroupSetupService
    {
        LedgerGroupSetup Get(int nID, int nUserID);
        List<LedgerGroupSetup> Gets(int nOCSID, int nUserID);
        List<LedgerGroupSetup> Gets(string sSQL, int nUserID);
        string Delete(int id, int nUserID);
        LedgerGroupSetup Save(LedgerGroupSetup oLedgerGroupSetup, int nUserID);
    }
    #endregion
 
}
