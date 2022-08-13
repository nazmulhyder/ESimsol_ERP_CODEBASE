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
    #region EHTransaction
    public class EHTransaction : BusinessObject
    {
        public EHTransaction()
        {
            EHTransactionID = 0;
            AccountHeadID = 0;
            ExpenditureType = EnumExpenditureType.None;
            ExpenditureTypeInt = 0;
            RefObjectID = 0;
            CurrencyID = 0;
            Amount = 0;
            CCRate = 0;
            AmountBC = 0;
            Remarks = "";
            AccountHeadName = "";
            CSymbol = "";
            ErrorMessage = "";
        }

        #region Properties
        public int EHTransactionID { get; set; }
        public int AccountHeadID { get; set; }
        public EnumExpenditureType ExpenditureType { get; set; }
        public int ExpenditureTypeInt { get; set; }
        public int RefObjectID { get; set; }
        public int CurrencyID { get; set; }
        public double Amount { get; set; }
        public double CCRate { get; set; }
        public double AmountBC { get; set; }
        public string Remarks { get; set; }
        public string AccountHeadName { get; set; }
        public string CSymbol { get; set; }
        public string ErrorMessage { get; set; }
        #endregion

        #region Functions
        public EHTransaction Get(int nId, Int64 nUserID)
        {
            return EHTransaction.Service.Get(nId, nUserID);
        }
        public static List<EHTransaction> Gets(Int64 nUserID)
        {
            return EHTransaction.Service.Gets(nUserID);
        }
        public static List<EHTransaction> Gets(string sSQL, Int64 nUserID)
        {
            return EHTransaction.Service.Gets(sSQL, nUserID);
        }
        public static List<EHTransaction> Gets(int nRefObjectID, EnumExpenditureType eExpenditureType, Int64 nUserID)
        {
            return EHTransaction.Service.Gets(nRefObjectID, eExpenditureType, nUserID);
        }        
        public EHTransaction Save(Int64 nUserID)
        {
            return EHTransaction.Service.Save(this, nUserID);
        }
        public string Delete(Int64 nUserID)
        {
            return EHTransaction.Service.Delete(this, nUserID);
        }
        #endregion

        #region ServiceFactory
        internal static IEHTransactionService Service
        {
            get { return (IEHTransactionService)Services.Factory.CreateService(typeof(IEHTransactionService)); }
        }
        #endregion
    }
    #endregion

    #region IEHTransaction interface    
    public interface IEHTransactionService
    {
        EHTransaction Get(int id, Int64 nUserID);        
        List<EHTransaction> Gets(Int64 nUserID);
        List<EHTransaction> Gets(int nRefObjectID, EnumExpenditureType eExpenditureType, Int64 nUserID);        
        List<EHTransaction> Gets(string sSQL, Int64 nUserID);
        EHTransaction Save(EHTransaction oEHTransaction, Int64 nUserID);
        string Delete(EHTransaction oEHTransaction, Int64 nUserID);
    }
    #endregion
}
