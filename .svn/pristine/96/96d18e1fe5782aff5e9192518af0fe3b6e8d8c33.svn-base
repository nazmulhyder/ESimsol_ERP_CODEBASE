using System;
using ICS.Core.Framework;
using ICS.Core.Utility;
using ICS.Core;
using ESimSol.BusinessObjects;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization;
using System.ServiceModel;
using ESimSol.BusinessObjects.ReportingObject;

namespace ESimSol.BusinessObjects
{
    #region BankBranchBU
    public class BankBranchBU : BusinessObject
    {
        public BankBranchBU()
        {
            BankBranchBUID = 0;
            BUID = 0;
            BankBranchID = 0;
            BUName = "";
            BUShortName = "";
            BranchName = "";
            BranchCode = "";
            ErrorMessage = "";
        }

        #region Property
        public int BankBranchBUID { get; set; }
        public int BUID { get; set; }
        public int BankBranchID { get; set; }
        public string BUName { get; set; }
        public string BUShortName { get; set; }
        public string BranchName { get; set; }
        public string BranchCode { get; set; }
        public string ErrorMessage { get; set; }
        #endregion

        #region Derived Property
        #endregion

        #region Functions
        public static List<BankBranchBU> Gets(int nID, long nUserID) //nID is contractor ID
        {
            return BankBranchBU.Service.Gets(nID, nUserID);
        }
        public static List<BankBranchBU> Gets(string sSQL, long nUserID)
        {
            return BankBranchBU.Service.Gets(sSQL, nUserID);
        }
        public BankBranchBU Get(int id, long nUserID)
        {
            return BankBranchBU.Service.Get(id, nUserID);
        }
        public BankBranchBU Save(long nUserID)
        {
            return BankBranchBU.Service.Save(this, nUserID);
        }
        public string Delete(int id, long nUserID)
        {
            return BankBranchBU.Service.Delete(id, nUserID);
        }
        #endregion

        #region ServiceFactory
        internal static IBankBranchBUService Service
        {
            get { return (IBankBranchBUService)Services.Factory.CreateService(typeof(IBankBranchBUService)); }
        }
        #endregion
    }
    #endregion

    #region IBankBranchBU interface
    public interface IBankBranchBUService
    {
        BankBranchBU Get(int id, Int64 nUserID);
        List<BankBranchBU> Gets(int nID, Int64 nUserID);
        List<BankBranchBU> Gets(string sSQL, Int64 nUserID);
        string Delete(int id, Int64 nUserID);
        BankBranchBU Save(BankBranchBU oBankBranchBU, Int64 nUserID);
    }
    #endregion
  
}
