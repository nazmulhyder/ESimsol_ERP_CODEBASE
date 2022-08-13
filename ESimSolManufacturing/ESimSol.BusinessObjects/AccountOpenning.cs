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
    #region AccountOpenning
    public class AccountOpenning : BusinessObject
    {
        public AccountOpenning()
        {
            AccountOpenningID = 0;
            AccountingSessionID = 0;
            AccountHeadID = 0;
            BusinessUnitID = 0;
            IsDebit = false;
            CurrencyID = 0;
            AmountInCurrency = 0;
            DrAmount = 0;
            CrAmount = 0;
            ConversionRate = 0;
            OpenningBalance = 0;
            BCDrAmount = 0;
            BCCrAmount = 0;
            BUCode = "00";
            BUName = "";
            AccountCode = "";
            AccountHeadName = "";
            SessionName = "";
            CName = "";
            CSymbol = "";
            ComponentID = 0;
            ComponentType = "";
            OpenningDate = DateTime.Now;
            BaseCurrencyId = 0;
            BaseCurrencySymbol = "";
            CostCenterID = 0;
            CCCode = "0000";
            CCName = "";
            ErrorMessage = "";
            IsCostCenterApply = false;
            AccountOpennings = new List<AccountOpenning>();
            AccountOpenningBreakdowns = new List<AccountOpenningBreakdown>();
            LstCurrency = new List<Currency>();
            BusinessUnits = new List<BusinessUnit>();
            AHOBs = new List<AHOB>();
        }

        #region Properties
        public int AccountOpenningID { get; set; }
        public int AccountingSessionID { get; set; }
        public int AccountHeadID { get; set; }
        public int BusinessUnitID { get; set; }
        public bool IsDebit { get; set; }
        public int CurrencyID { get; set; }
        public double AmountInCurrency { get; set; }
        public double DrAmount { get; set; }
        public double CrAmount { get; set; }
        public double ConversionRate { get; set; }
        public double OpenningBalance { get; set; }
        public double BCDrAmount { get; set; }
        public double BCCrAmount { get; set; }
        public string BUCode { get; set; }
        public string BUName { get; set; }
        public int CostCenterID { get; set; }
        public string CCCode { get; set; }
        public string CCName { get; set; }
        public string AccountCode { get; set; }
        public string AccountHeadName { get; set; }
        public string SessionName { get; set; }
        public string CName { get; set; }
        public string CSymbol { get; set; }
        public int ComponentID { get; set; }
        public string ComponentType { get; set; }
        public DateTime OpenningDate { get; set; }
        public int BaseCurrencyId { get; set; }
        public string BaseCurrencySymbol { get; set; }
        public string ErrorMessage { get; set; }
        #endregion

        #region Derive Property
        public List<AHOB> AHOBs { get; set; }
        public bool IsCostCenterApply { get; set; }
        public List<AccountOpenning> AccountOpennings { get; set; }
        public List<AccountOpenningBreakdown> AccountOpenningBreakdowns { get; set; }
        public List<Currency> LstCurrency { get; set; }
        public List<BusinessUnit> BusinessUnits { get; set; }
        public string DR_CR
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
        public string OpenningDateInString
        {
            get
            {
                return this.OpenningDate.ToString("dd MMM yyyy");
            }
        }
        #endregion

        #region Functions
        public static List<AccountOpenning> Gets(int nSessionID, int nCompanyID, int nUserID)
        {
            return AccountOpenning.Service.Gets(nSessionID, nCompanyID, nUserID);
        }
        public AccountOpenning SetOpenningBalance(int nUserID)
        {
            return AccountOpenning.Service.SetOpenningBalance(this, nUserID);
        }
        public AccountOpenning SetOpenningBalanceSubledger(int nUserID)
        {
            return AccountOpenning.Service.SetOpenningBalanceSubledger(this, nUserID);
        }
        public AccountOpenning Get(int id, int nCompanyID, int nUserID)
        {
            return AccountOpenning.Service.Get(id, nCompanyID, nUserID);
        }
        public AccountOpenning Get(int nAccountHeadID, int nSessionID, int nBusinessUnitID, int nUserID)
        {
            return AccountOpenning.Service.Get(nAccountHeadID, nSessionID, nBusinessUnitID, nUserID);
        }
        public static List<AccountOpenning> GetsByAccountAndSession(int nAccountHeadID, int nSessionID, int nUserID)
        {
            return AccountOpenning.Service.GetsByAccountAndSession(nAccountHeadID, nSessionID, nUserID);
        }

        public AccountOpenning Save(int nUserID)
        {
            return AccountOpenning.Service.Save(this, nUserID);
        }

        #endregion

        #region ServiceFactory
        internal static IAccountOpenningService Service
        {
            get { return (IAccountOpenningService)Services.Factory.CreateService(typeof(IAccountOpenningService)); }
        }
        #endregion
    }
    #endregion

    #region IAccountOpenning interface
    public interface IAccountOpenningService
    {
        AccountOpenning SetOpenningBalance(AccountOpenning oAccountOpenning, int nUserID);
        AccountOpenning SetOpenningBalanceSubledger(AccountOpenning oAccountOpenning, int nUserID);
        AccountOpenning Get(int id, int nCompanyID, int nUserID);
        AccountOpenning Get(int nAccountHeadID, int nSessionID, int nBusinessUnitID, int nUserID);
        List<AccountOpenning> Gets(int nSessionID, int nCompanyID, int nUserID);
        List<AccountOpenning> GetsByAccountAndSession(int nAccountHeadID, int nSessionID, int nUserID);
        AccountOpenning Save(AccountOpenning oAccountOpenning, int nUserID);
    }
    #endregion
    
    #region AHOB (Account Head Opening Breakdown)
    public class AHOB : BusinessObject
    {
        public AHOB()
        {
            AHOBT = EnumBreakdownType.CostCenter;
            AHOBTInt = 1;
            AHOBTStr = "Subledger";
            AHOBID = 0;
            Name = "";
            Code = "";
            UnitID = 0;
            UName = "";
            WUID = 0;
            WUName = "";
            Qty = 0;
            UPrice = 0;
            Amount = 0;
            DrAmount = 0;
            CrAmount = 0;
            CFormat = "";
            CID = 0;
            CRate = 0;
            CAmount = 0;
            CSymbol = "";
            DR_CR = "";

            BillAmount = 0;
            BillDate = DateTime.Now;
            BillID = 0;
            CCID = 0;
            CCName = "";
            CCCode = "";
            IsBTAply = false;
        }

        #region Properties
        public int AHOBID { get; set; }
        public EnumBreakdownType AHOBT { get; set; }
        public int AHOBTInt { get; set; }
        public string AHOBTStr { get; set; }
        public int BObjID { get; set; }  //BreakdownObjID
        public string Name { get; set; }  //FOR BT Name = Bill No, For CC Name = CCName, For IR Name = Product Name 
        public string Code { get; set; }
        public int UnitID { get; set; }
        public string UName { get; set; }
        public int WUID { get; set; }
        public string WUName { get; set; }
        public double Qty { get; set; }
        public double UPrice { get; set; }
        public double Amount { get; set; }
        public double DrAmount { get; set; }
        public double CrAmount { get; set; }
        public string CFormat { get; set; }
        public string DR_CR { get; set; }
        public int CID { get; set; }
        public double CRate { get; set; }
        public double CAmount { get; set; }
        public string CSymbol { get; set; }

        public double BillAmount { get; set; }
        public DateTime BillDate { get; set; }
        public int BillID { get; set; }
        public int CCID { get; set; }
        public string CCName { get; set; }
        public string CCCode { get; set; }
        public bool IsBTAply { get; set; }

        public string BillDateInString
        {
            get { return this.BillDate.ToString("dd MMM yyyy"); }
        }

        #endregion


    }
    #endregion
}
