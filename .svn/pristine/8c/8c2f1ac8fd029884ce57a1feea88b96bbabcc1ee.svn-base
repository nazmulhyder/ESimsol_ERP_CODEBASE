using System;
using System.IO;
using ESimSol.BusinessObjects;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Collections.Generic;
using ICS.Core;
using ICS.Core.Framework;
using ICS.Core.Utility;


namespace ESimSol.BusinessObjects
{
    #region ContractorPersonal
    
    public class PartyWiseBank : BusinessObject
    {
        public PartyWiseBank()
        {
            PartyWiseBankID = 0;
            BankID = 0;
            BranchName = "";
            ContractorID=0;
            AccountName = "";
            AccountNo = "";
            ContractorName = "";
            BankName = "";
            ErrorMessage = "";
        }

        #region Properties
        public int PartyWiseBankID { get; set; }
        public int ContractorID { get; set; }
        public int BankID { get; set; }
        public string BranchName { get; set; }
        public string AccountName { get; set; }
        public string AccountNo { get; set; }
        public string BankName { get; set; }
        public string ContractorName { get; set; }
        public string ErrorMessage { get; set; }
        #endregion

        #region Functions
        public  PartyWiseBank Get(int nId, long nUserID)
        {
            return PartyWiseBank.Service.Get(nId, nUserID);
        }
        public PartyWiseBank Save(long nUserID)
        {
            return PartyWiseBank.Service.Save(this, nUserID);
        }

        public string Delete(int nId, long nUserID)
        {
            return PartyWiseBank.Service.Delete(nId, nUserID);
        }
        public static List<PartyWiseBank> Gets(long nUserID)
        {
            return PartyWiseBank.Service.Gets(nUserID);
        }

        public static List<PartyWiseBank> Gets(string sSQL, long nUserID)
        {
            return PartyWiseBank.Service.Gets(sSQL,nUserID);
        }
        public static List<PartyWiseBank> GetsByContractor(int nContractorID, long nUserID)
        {
            return PartyWiseBank.Service.GetsByContractor(nContractorID,nUserID);
        }

        #endregion

        #region ServiceFactory
        internal static IPartyWiseBankService Service
        {
            get { return (IPartyWiseBankService)Services.Factory.CreateService(typeof(IPartyWiseBankService)); }
        }
        #endregion
    }
    #endregion

    #region IContractorPersonal interface
    public interface IPartyWiseBankService
    {
        PartyWiseBank Get(int id, long nUserID);
        string Delete(int id, long nUserID);
        PartyWiseBank Save(PartyWiseBank oContractorPersonal, long nUserID);
        List<PartyWiseBank> Gets(long nUserID);
        List<PartyWiseBank> Gets(string sSQL, long nUserID);
        List<PartyWiseBank> GetsByContractor(int nContractorID, long nUserID);
    }
    #endregion
}