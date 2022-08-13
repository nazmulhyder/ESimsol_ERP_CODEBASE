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
    #region ChangesEquitySetupDetail
    public class ChangesEquitySetupDetail : BusinessObject
    {
        public ChangesEquitySetupDetail()
        {
            ChangesEquitySetupDetailID = 0;
            ChangesEquitySetupID = 0;
            EffectedAccountID = 0;
            AccountCode = "";
            AccountHeadName = "";
            ErrorMessage = "";
        }
        #region Properties
        public int ChangesEquitySetupDetailID { get; set; }
        public int ChangesEquitySetupID { get; set; }
        public int EffectedAccountID { get; set; }
        public string AccountCode { get; set; }
        public string AccountHeadName { get; set; }
        public string ErrorMessage { get; set; }
        #endregion

        #region Derived Property

        #endregion

        #region Functions

        public ChangesEquitySetupDetail Get(int id, int nUserID)
        {
            return ChangesEquitySetupDetail.Service.Get(id, nUserID);
        }
        public ChangesEquitySetupDetail Save(int nUserID)
        {
            return ChangesEquitySetupDetail.Service.Save(this, nUserID);
        }
        public string Delete(int id, int nUserID)
        {
            return ChangesEquitySetupDetail.Service.Delete(id, nUserID);
        }
        public static List<ChangesEquitySetupDetail> Gets(int nUserID)
        {
            return ChangesEquitySetupDetail.Service.Gets(nUserID);
        }
        public static List<ChangesEquitySetupDetail> Gets(int nChangesEquitySetupID, int nUserID)
        {
            return ChangesEquitySetupDetail.Service.Gets(nChangesEquitySetupID, nUserID);
        }
        public static List<ChangesEquitySetupDetail> Gets(string sSQL, int nUserID)
        {
            return ChangesEquitySetupDetail.Service.Gets(sSQL, nUserID);
        }
        #endregion


        #region ServiceFactory
        internal static IChangesEquitySetupDetailService Service
        {
            get { return (IChangesEquitySetupDetailService)Services.Factory.CreateService(typeof(IChangesEquitySetupDetailService)); }
        }
        #endregion
    }
    #endregion

    #region IChangesEquitySetupDetail interface
    public interface IChangesEquitySetupDetailService
    {
        ChangesEquitySetupDetail Get(int id, int nUserID);
        List<ChangesEquitySetupDetail> Gets(int nUserID);
        List<ChangesEquitySetupDetail> Gets(int nChangesEquitySetupID, int nUserID);
        string Delete(int id, int nUserID);
        ChangesEquitySetupDetail Save(ChangesEquitySetupDetail oChangesEquitySetupDetail, int nUserID);
        List<ChangesEquitySetupDetail> Gets(string sSQL, int nUserID);
    }
    #endregion
}