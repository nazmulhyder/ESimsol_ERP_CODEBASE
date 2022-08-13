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
    #region TempObject
    public class TempObject : BusinessObject
    {
        public TempObject()
        {
            AccountHeadID = 0;
            AccountHeadCode = "";
            AccountHeadName = "";
            SubGroupName = "";
            IsDebit = false;
            OCSID = 0;
            LedgerGroupSetUpID = 0;
            LedgerGroupName = "";
            ErrorMessage = "";
        }

        #region Properties
        public int AccountHeadID { get; set; }
        public string AccountHeadCode { get; set; }
        public string AccountHeadName { get; set; }
        public string SubGroupName { get; set; }
        public bool IsDebit { get; set; }
        public int OCSID { get; set; }
        public int LedgerGroupSetUpID { get; set; }
        public string LedgerGroupName { get; set; }
        public string ErrorMessage { get; set; }

        #endregion

        #region Derived Property
        public string IsDebitSt
        {
            get 
            {
                if (this.IsDebit)
                {
                    return "Debit";
                }
                else
                {
                    return "Credit";
                }
            }
        }
        #endregion

        #region Functions
        public static List<TempObject> Gets(int nStatementSetupID, DateTime dstartDate, DateTime dendDate, int nBUID, int nUserID)
        {
            return TempObject.Service.Gets(nStatementSetupID, dstartDate, dendDate, nBUID, nUserID);
        }
        #endregion


        #region ServiceFactory
        internal static ITempObjectService Service
        {
            get { return (ITempObjectService)Services.Factory.CreateService(typeof(ITempObjectService)); }
        }
        #endregion
    }
    #endregion

    #region ITempObject interface
    public interface ITempObjectService
    {
        List<TempObject> Gets(int nStatementSetupID, DateTime dstartDate, DateTime dendDate, int nBUID, int nUserID);
    }
    #endregion
}
