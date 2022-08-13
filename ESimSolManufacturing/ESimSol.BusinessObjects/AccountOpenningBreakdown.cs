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
    #region AccountOpenningBreakdown
    public class AccountOpenningBreakdown : BusinessObject
    {
        public AccountOpenningBreakdown()
        {
            AccountOpenningBreakdownID = 0;
            AccountingSessionID = 0;
            BusinessUnitID = 0;
            AccountHeadID = 0;
            BreakdownObjID = 0;
            IsDr = false;
            BreakdownType = EnumBreakdownType.VoucherDetail;
            MUnitID = 0;
            WUnitID = 0;
            UnitPrice = 0;
            Qty = 0;
            CurrencyID = 0;
            ConversionRate = 0;
            AmountInCurrency = 0;
            DrAmount = 0;
            CrAmount = 0;
            OpenningBalance = 0;
            BCDrAmount = 0;
            BCCrAmount = 0;
            AccountCode = "";
            AccountHeadName = "";
            SessionName = "";
            UnitName = "";
            Symbol = "";
            WUName = "";
            BUCode = "00";
            BUName = "";
            CurrencySymbol = "";
            CurrencyName = "";
            BreakdownCode = "0000";
            BreakdownName = "";
            CCID = 0;
            CCName = "";
            CCCode = "";
            IsBTAply = false;
            ErrorMessage = "";
            AHOBs = new List<AHOB>();
        }

        #region Properties
        public int AccountOpenningBreakdownID { get; set; }
        public int AccountingSessionID { get; set; }
        public int BusinessUnitID { get; set; }
        public int AccountHeadID { get; set; }
        public int BreakdownObjID { get; set; }
        public bool IsDr { get; set; }
        public EnumBreakdownType BreakdownType { get; set; }
        public int MUnitID { get; set; }
        public int WUnitID { get; set; }
        public double UnitPrice { get; set; }
        public double Qty { get; set; }
        public int CurrencyID { get; set; }
        public double ConversionRate { get; set; }
        public double AmountInCurrency { get; set; }
        public double DrAmount { get; set; }
        public double CrAmount { get; set; }
        public double OpenningBalance { get; set; }
        public double BCDrAmount { get; set; }
        public double BCCrAmount { get; set; }
        public string AccountCode { get; set; }
        public string AccountHeadName { get; set; }
        public string SessionName { get; set; }
        public string UnitName { get; set; }
        public string Symbol { get; set; }
        public string WUName { get; set; }
        public string BUCode { get; set; }
        public string BUName { get; set; }
        public string CurrencySymbol { get; set; } 
        public string CurrencyName { get; set; }
        public string BreakdownCode { get; set; }
        public string BreakdownName { get; set; }
        public int CCID { get; set; }
        public string CCName { get; set; }
        public string CCCode { get; set; }
        public bool IsBTAply { get; set; }
        public string ErrorMessage { get; set; }
        public List<AHOB> AHOBs { get; set; }
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
        #endregion
        
        #region Functions
        public static List<AccountOpenningBreakdown> GetsByAccountAndSession(int nAccountHeadID, int nAccountingSessionID, int nBusinessUnitID, int nUserID)
        {
            return AccountOpenningBreakdown.Service.GetsByAccountAndSession(nAccountHeadID, nAccountingSessionID, nBusinessUnitID, nUserID);
        }
        public static List<AccountOpenningBreakdown> GetsSubLedgerWiseBills(int nAccountHeadID, int nAccountingSessionID, int nBusinessUnitID, int nSubLedgerID, int nUserID)
        {
            return AccountOpenningBreakdown.Service.GetsSubLedgerWiseBills(nAccountHeadID, nAccountingSessionID, nBusinessUnitID, nSubLedgerID, nUserID);
        }
        public AccountOpenningBreakdown GetsByAccountAndSubledgerAndSession(int nAccountHeadID, int nAccountingSessionID, int nBusinessUnitID, int nSubledgerID, int nUserID)
        {
            return AccountOpenningBreakdown.Service.GetsByAccountAndSubledgerAndSession(nAccountHeadID, nAccountingSessionID, nBusinessUnitID, nSubledgerID, nUserID);
        }
        public AccountOpenningBreakdown Get(int id, int nCompanyID, int nUserID)
        {
            return AccountOpenningBreakdown.Service.Get(id, nCompanyID, nUserID);
        }
        public AccountOpenningBreakdown Save(int nUserID)
        {
            return AccountOpenningBreakdown.Service.Save(this, nUserID);
        }
        public string Delete(int id, int nUserID)
        {
            return AccountOpenningBreakdown.Service.Delete(id, nUserID);
        }
        public static List<AccountOpenningBreakdown> Gets(int nCompanyID, int nUserID)
        {
            return AccountOpenningBreakdown.Service.Gets(nCompanyID, nUserID);
        }
        public static List<AccountOpenningBreakdown> Gets(string sSQL, int nCompanyID, int nUserID)
        {
            return AccountOpenningBreakdown.Service.Gets(sSQL,nCompanyID, nUserID);
        }
        #endregion

        #region ServiceFactory
        internal static IAccountOpenningBreakdownService Service
        {
            get { return (IAccountOpenningBreakdownService)Services.Factory.CreateService(typeof(IAccountOpenningBreakdownService)); }
        }
        #endregion
    }
    #endregion

    #region IAccountOpenningBreakdown interface
    public interface IAccountOpenningBreakdownService
    {
        AccountOpenningBreakdown Get(int id,int nCompanyID, int nUserID);
        List<AccountOpenningBreakdown> Gets(int nCompanyID, int nUserID);
        List<AccountOpenningBreakdown> GetsByAccountAndSession(int nAccountHeadID, int nAccountingSessionID, int nBusinessUnitID, int nUserID);
        AccountOpenningBreakdown GetsByAccountAndSubledgerAndSession(int nAccountHeadID, int nAccountingSessionID, int nBusinessUnitID, int nSubLedgerID, int nUserID);
        List<AccountOpenningBreakdown> GetsSubLedgerWiseBills(int nAccountHeadID, int nAccountingSessionID, int nBusinessUnitID, int nSubLedgerID, int nUserID);
        string Delete(int id, int nUserID);
        AccountOpenningBreakdown Save(AccountOpenningBreakdown oAccountOpenningBreakdown, int nUserID);
        List<AccountOpenningBreakdown> Gets(string sSQL, int nCompanyID, int nUserID);
    }
    #endregion
}
