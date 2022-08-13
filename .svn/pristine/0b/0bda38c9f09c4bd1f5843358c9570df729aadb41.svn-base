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
    #region SubledgerRefConfig
    public class SubledgerRefConfig : BusinessObject
    {
        public SubledgerRefConfig()
        {
            SubledgerRefConfigID = 0;
            AccountHeadID = 0;
            SubledgerID = 0;
            IsBillRefApply = false;
            IsOrderRefApply = false;
            AccountHeadName = "";
            AccountCode = "";
            SubledgerName = "";
            SubledgerCode = "";
            ErrorMessage = "";
            IsChecked = false;
        }

        #region Properties
        public int SubledgerRefConfigID { get; set; }
        public int AccountHeadID { get; set; }
        public int SubledgerID { get; set; }
        public bool IsBillRefApply { get; set; }
        public bool IsOrderRefApply { get; set; }
        public string AccountHeadName { get; set; }
        public string AccountCode { get; set; }
        public string SubledgerName { get; set; }
        public string SubledgerCode { get; set; }
        public bool IsChecked { get; set; }
        public string ErrorMessage { get; set; }
        #endregion

        #region Functions
        public SubledgerRefConfig Get(int nAccountHeadID, int nUserID)
        {
            return SubledgerRefConfig.Service.Get(nAccountHeadID, nUserID);
        }
        public SubledgerRefConfig Save(int nUserID)
        {
            return SubledgerRefConfig.Service.Save(this, nUserID);
        }
        public string Delete(int id, int nUserID)
        {
            return SubledgerRefConfig.Service.Delete(id, nUserID);
        }
        public static List<SubledgerRefConfig> Gets(int nSubledgerID, int nUserID)
        {
            return SubledgerRefConfig.Service.Gets(nSubledgerID, nUserID);
        }
        public static List<SubledgerRefConfig> Gets(string sSQL, int nUserID)
        {
            return SubledgerRefConfig.Service.Gets(sSQL, nUserID);
        }
        #endregion

        #region ServiceFactory
        internal static ISubledgerRefConfigService Service
        {
            get { return (ISubledgerRefConfigService)Services.Factory.CreateService(typeof(ISubledgerRefConfigService)); }
        }
        #endregion
    }
    #endregion

    #region ISubledgerRefConfig interface
    public interface ISubledgerRefConfigService
    {
        SubledgerRefConfig Save(SubledgerRefConfig oSubledgerRefConfig, int nUserID);
        SubledgerRefConfig Get(int nAccountHeadID, int nUserID);
        List<SubledgerRefConfig> Gets(int nSubledgerID, int nUserID);
        List<SubledgerRefConfig> Gets(string sSQL, int nUserID);
        string Delete(int id, int nUserID);
    }
    #endregion
}