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

    #region LedgerBreakDown
    public class LedgerBreakDown : BusinessObject
    {
        #region  Constructor
        public LedgerBreakDown()
        {
            LedgerBreakDownID = 0;
            ReferenceID= 0;
            IsEffectedAccounts = true;
            AccountHeadID = 0;
            AccountCode = "";
            AccountHeadName = "";
            Note = "";
            ErrorMessage = "";            
        }
        #endregion

        #region Properties
        public int LedgerBreakDownID { get; set; }
        public int ReferenceID { get; set; }
        public bool IsEffectedAccounts { get; set; }
        public int AccountHeadID { get; set; }
        public string AccountCode { get; set; }
        public string AccountHeadName { get; set; }
        public string Note { get; set; }
        public string ErrorMessage { get; set; }
        #endregion

        #region Functions
        public LedgerBreakDown Get(int id, int nUserID)
        {
            return LedgerBreakDown.Service.Get(id, nUserID);
        }
        public static List<LedgerBreakDown> Gets(int id,  bool bIsEffectedAccounts, int nUserID)
        {
            return LedgerBreakDown.Service.Gets(id, bIsEffectedAccounts, nUserID);
        }
        public LedgerBreakDown Save(int nUserID)
        {
            return LedgerBreakDown.Service.Save(this, nUserID);
        }
        public string Delete(int id, int nUserID)
        {
            return LedgerBreakDown.Service.Delete(id, nUserID);
        }
        public static List<LedgerBreakDown> Gets(int id, int nUserID)
        {
            return LedgerBreakDown.Service.Gets(id, nUserID);
        }
        #endregion

        #region ServiceFactory
        internal static ILedgerBreakDownService Service
        {
            get { return (ILedgerBreakDownService)Services.Factory.CreateService(typeof(ILedgerBreakDownService)); }
        }
        #endregion
    }
    #endregion


    #region ILedgerBreakDown interface
    public interface ILedgerBreakDownService
    {
        LedgerBreakDown Get(int nID, int nUserID);
        List<LedgerBreakDown> Gets(int id, bool bIsEffectedAccounts, int nUserID);
        List<LedgerBreakDown> Gets(int id, int nUserID);
        string Delete(int id, int nUserID);
        LedgerBreakDown Save(LedgerBreakDown oLedgerBreakDown, int nUserID);
    }
    #endregion

}
