using System;
using System.IO;
using ESimSol.BusinessObjects;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Collections.Generic;
using ICS.Core.Framework;

namespace ESimSol.BusinessObjects
{

    #region BUWiseSubLedger
    public class BUWiseSubLedger : BusinessObject
    {
        public BUWiseSubLedger()
        {
            BUWiseSubLedgerID = 0;
            BusinessUnitID = 0;
            SubLedgerID = 0;
            ErrorMessage = "";
        }

        #region Properties
        public int BUWiseSubLedgerID { get; set; }
        public int BusinessUnitID { get; set; }
        public int SubLedgerID { get; set; }
        public string ErrorMessage { get; set; }
        #endregion

        #region Derived Propertise
        public List<BUWiseSubLedger> BUWiseSubLedgers { get; set; }

        public Company Company { get; set; }
        public TChartsOfAccount TChartsOfAccount { get; set; }
        #endregion

        #region Functions
        public string CopyBasicChartOfAccount(int nCompanyID, int nUserID)
        {
            return BUWiseSubLedger.Service.CopyBasicChartOfAccount(nCompanyID, nUserID);
        }
        public BUWiseSubLedger Get(int id, int nUserID)
        {
            return BUWiseSubLedger.Service.Get(id, nUserID);
        }
        public BUWiseSubLedger Save(int nUserID)
        {
            return BUWiseSubLedger.Service.Save(this, nUserID);
        }
        public BUWiseSubLedger SaveFromCC(int nUserID)
        {
            return BUWiseSubLedger.Service.SaveFromCC(this, nUserID);
        }
        public string Delete(int id, int nUserID)
        {
            return BUWiseSubLedger.Service.Delete(id, nUserID);
        }
        public static List<BUWiseSubLedger> Gets(int nUserID)
        {
            return BUWiseSubLedger.Service.Gets(nUserID);
        }
        public static List<BUWiseSubLedger> Gets(int nBUID, int nUserID)
        {
            return BUWiseSubLedger.Service.Gets(nBUID, nUserID);
        }
        public static List<BUWiseSubLedger> GetsByCC(int nAHID, int nUserID)
        {
            return BUWiseSubLedger.Service.GetsByCC(nAHID, nUserID);
        }
        #endregion

        #region ServiceFactory
        internal static IBUWiseSubLedgerService Service
        {
            get { return (IBUWiseSubLedgerService)Services.Factory.CreateService(typeof(IBUWiseSubLedgerService)); }
        }
        #endregion
    }
    #endregion

    #region IBUWiseSubLedger interface
    public interface IBUWiseSubLedgerService
    {
        BUWiseSubLedger Get(int id, int nUserID);
        List<BUWiseSubLedger> Gets(int nUserID);
        List<BUWiseSubLedger> Gets(int nBUID, int nUserID);
        List<BUWiseSubLedger> GetsByCC(int nAHID, int nUserID);
        string CopyBasicChartOfAccount(int id, int nUserID);
        string Delete(int id, int nUserID);
        BUWiseSubLedger Save(BUWiseSubLedger oBUWiseSubLedger, int nUserID);
        BUWiseSubLedger SaveFromCC(BUWiseSubLedger oBUWiseSubLedger, int nUserID);
    }
    #endregion
    

}
