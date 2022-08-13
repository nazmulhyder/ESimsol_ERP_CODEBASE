using ICS.Core.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ESimSol.BusinessObjects
{
    #region WUSubContractTermsCondition
    public class WUSubContractTermsCondition : BusinessObject
    {
        public WUSubContractTermsCondition()
        {
            WUSubContractTermsConditionID = 0;
            WUSubContractID = 0;
            TermsAndCondition = "";
            DBUserID = 0;
            DBServerDateTime = DateTime.Today;
        }

        #region Properties
        public int WUSubContractTermsConditionID { get; set; }
        public int WUSubContractID { get; set; }
        public string TermsAndCondition { get; set; }
        public int DBUserID { get; set; }
        public DateTime DBServerDateTime { get; set; }
        public string ErrorMessage { get; set; }
        #endregion

        #region Functions
        public static List<WUSubContractTermsCondition> Gets(int id, long nUserID)
        {
            return WUSubContractTermsCondition.Service.Gets(id, nUserID);
        }
        public static List<WUSubContractTermsCondition> Get(string sSQL, int nCurrentUserID)
        {
            return WUSubContractTermsCondition.Service.Get(sSQL, nCurrentUserID);
        }

        #endregion

        #region ServiceFactory
        internal static IWUSubContractTermsConditionService Service
        {
            get { return (IWUSubContractTermsConditionService)Services.Factory.CreateService(typeof(IWUSubContractTermsConditionService)); }
        }
        #endregion
    }
    #endregion

    #region IWUSubContractTermsCondition interface
    public interface IWUSubContractTermsConditionService
    {
        List<WUSubContractTermsCondition> Gets(int id, Int64 nUserID);
        List<WUSubContractTermsCondition> Get(string sSQL, int nCurrentUserID);
    }
    #endregion
}