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
    #region BankReconciliationOpenning
    public class BankReconciliationOpenning : BusinessObject
    {
        public BankReconciliationOpenning()
        {
            BankReconciliationOpenningID = 0;
            AccountingSessionID = 0;
            BusinessUnitID = 0;
            AccountHeadID = 0;
            SubledgerID = 0;
            IsDr = false;
            CurrencyID = 0;
            ConversionRate = 0;
            AmountInCurrency = 0;
            DrAmount = 0;
            CrAmount = 0;
            OpenningBalance = 0;
            BCDrAmount = 0;
            BCCrAmount = 0;
            SubledgerCode = "";
            SubledgerName = "";
            AccountCode = "";
            AccountHeadName = "";
            SessionName = "";
            BUCode = "00";
            BUName = "";
            CurrencySymbol = "";
            CurrencyName = "";
            ErrorMessage = "";
            OpenningDate = DateTime.Today;
            BaseCurrencyId = 0;
            BaseCurrencySymbol = "";
            LstCurrency = new List<Currency>();
            BusinessUnits = new List<BusinessUnit>();
        }

        #region Properties
        public int BankReconciliationOpenningID { get; set; }
        public int AccountingSessionID { get; set; }
        public int BusinessUnitID { get; set; }
        public int AccountHeadID { get; set; }
        public int SubledgerID { get; set; }
        public bool IsDr { get; set; }        
        public int CurrencyID { get; set; }
        public double ConversionRate { get; set; }
        public double AmountInCurrency { get; set; }
        public double DrAmount { get; set; }
        public double CrAmount { get; set; }
        public double OpenningBalance { get; set; }
        public string SubledgerCode { get; set; }
        public string SubledgerName { get; set; }		
        public double BCDrAmount { get; set; }
        public double BCCrAmount { get; set; }
        public string AccountCode { get; set; }
        public string AccountHeadName { get; set; }
        public string SessionName { get; set; }                
        public string BUCode { get; set; }
        public string BUName { get; set; }
        public string CurrencySymbol { get; set; } 
        public string CurrencyName { get; set; }                
        public string ErrorMessage { get; set; }        
        public string IsDebitInString
        {
            get
            {
                if (this.IsDr)
                {
                    return "Debit";
                }
                else
                {
                    return "Credit";
                }
            }
        }
        public string OpenningDateSt
        {
            get
            {
                return this.OpenningDate.ToString("dd MMM yyyy");
            }
        }        
        public DateTime OpenningDate { get; set; }
        public int BaseCurrencyId { get; set; }
        public string BaseCurrencySymbol { get; set; }
        public List<Currency> LstCurrency { get; set; }
        public List<BusinessUnit> BusinessUnits { get; set; }
        #endregion
        
        #region Functions
        public static List<BankReconciliationOpenning> GetsByAccountAndSession(int nAccountHeadID, int nAccountingSessionID, int nBusinessUnitID, int nUserID)
        {
            return BankReconciliationOpenning.Service.GetsByAccountAndSession(nAccountHeadID, nAccountingSessionID, nBusinessUnitID, nUserID);
        }
        public static List<BankReconciliationOpenning> GetsSubLedgerWiseBills(int nAccountHeadID, int nAccountingSessionID, int nBusinessUnitID, int nSubLedgerID, int nUserID)
        {
            return BankReconciliationOpenning.Service.GetsSubLedgerWiseBills(nAccountHeadID, nAccountingSessionID, nBusinessUnitID, nSubLedgerID, nUserID);
        }
        public BankReconciliationOpenning GetsByAccountAndSubledgerAndSession(int nAccountHeadID, int nAccountingSessionID, int nBusinessUnitID, int nSubledgerID, int nUserID)
        {
            return BankReconciliationOpenning.Service.GetsByAccountAndSubledgerAndSession(nAccountHeadID, nAccountingSessionID, nBusinessUnitID, nSubledgerID, nUserID);
        }
        public BankReconciliationOpenning Get(int id, int nCompanyID, int nUserID)
        {
            return BankReconciliationOpenning.Service.Get(id, nCompanyID, nUserID);
        }
        public BankReconciliationOpenning Save(int nUserID)
        {
            return BankReconciliationOpenning.Service.Save(this, nUserID);
        }

        public string  BRBalanceTranfer(BankReconciliationOpenning oBankReconciliationOpenning,int nUserID)
        {
            return BankReconciliationOpenning.Service.BRBalanceTranfer(oBankReconciliationOpenning, nUserID);
        }
        public string Delete(int id, int nUserID)
        {
            return BankReconciliationOpenning.Service.Delete(id, nUserID);
        }
        public static List<BankReconciliationOpenning> Gets(int nCompanyID, int nUserID)
        {
            return BankReconciliationOpenning.Service.Gets(nCompanyID, nUserID);
        }
        public static List<BankReconciliationOpenning> Gets(string sSQL, int nCompanyID, int nUserID)
        {
            return BankReconciliationOpenning.Service.Gets(sSQL,nCompanyID, nUserID);
        }
        #endregion

        #region ServiceFactory
        internal static IBankReconciliationOpenningService Service
        {
            get { return (IBankReconciliationOpenningService)Services.Factory.CreateService(typeof(IBankReconciliationOpenningService)); }
        }
        #endregion
    }
    #endregion

    #region IBankReconciliationOpenning interface
    public interface IBankReconciliationOpenningService
    {
        BankReconciliationOpenning Get(int id,int nCompanyID, int nUserID);
        List<BankReconciliationOpenning> Gets(int nCompanyID, int nUserID);
        List<BankReconciliationOpenning> GetsByAccountAndSession(int nAccountHeadID, int nAccountingSessionID, int nBusinessUnitID, int nUserID);
        BankReconciliationOpenning GetsByAccountAndSubledgerAndSession(int nAccountHeadID, int nAccountingSessionID, int nBusinessUnitID, int nSubLedgerID, int nUserID);
        List<BankReconciliationOpenning> GetsSubLedgerWiseBills(int nAccountHeadID, int nAccountingSessionID, int nBusinessUnitID, int nSubLedgerID, int nUserID);
        string Delete(int id, int nUserID);
        BankReconciliationOpenning Save(BankReconciliationOpenning oBankReconciliationOpenning, int nUserID);
        string BRBalanceTranfer(BankReconciliationOpenning oBankReconciliationOpenning, int nUserID);
        
        List<BankReconciliationOpenning> Gets(string sSQL, int nCompanyID, int nUserID);
    }
    #endregion
}
