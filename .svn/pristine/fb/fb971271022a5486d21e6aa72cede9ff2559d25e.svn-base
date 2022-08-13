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
    #region TradingPayment
    public class TradingPayment
    {
        #region  Constructor
        public TradingPayment()
        {
            TradingPaymentID = 0;
            BUID = 0;
            ContractorID = 0;
            ContactPersonnelID = 0;
            AccountHeadID = 0;
            RefNo = "";
            PaymentDate = DateTime.Today;
            EncashmentDate = DateTime.Today;
            PaymentMethod = EnumPaymentMethod.None;
            PaymentMethodInt = 0;
            CurrencyID = 0;
            Amount = 0;
            ReferenceType = EnumPaymentRefType.None;
            ReferenceTypeInt = 0;
            TradingPaymentStatus = EnumPaymentStatus.Initialize;
            TradingPaymentStatusInt = 0;
            ApprovedBy = 0;
            ApprovedDate = DateTime.Now;
            Note = "";
            ChequeNo = "";
            AccountID = 0;
            BankName = "";
            AccountNo = "";
            BranchName = "";
            BUName = "";
            BUCode = "";
            ContractorName = "";
            ContactPersonName = "";
            CurrencyName = "";
            CurrencySymbol = "";
            AccountCode = "";
            AccountHeadName = "";
            ApprovedByName = "";
            SalesManID = 0;
            SalesManName = string.Empty;
            ErrorMessage = "";
            TradingPaymentDetails = new List<TradingPaymentDetail>();
            Currencys = new List<Currency>();
            ChartsOfAccounts = new List<ChartsOfAccount>();
            BankAccounts = new List<BankAccount>();
        }
        #endregion

        #region Properties
        public int TradingPaymentID { get; set; }
        public int BUID { get; set; }
        public int ContractorID { get; set; }
        public int ContactPersonnelID { get; set; }
        public int AccountHeadID { get; set; }
        public string RefNo { get; set; }
        public DateTime PaymentDate { get; set; }
        public DateTime EncashmentDate { get; set; }
        public EnumPaymentMethod PaymentMethod { get; set; }
        public int PaymentMethodInt { get; set; }
        public int CurrencyID { get; set; }
        public double Amount { get; set; }
        public EnumPaymentRefType ReferenceType { get; set; }
        public int ReferenceTypeInt { get; set; }
        public EnumPaymentStatus TradingPaymentStatus { get; set; }
        public int TradingPaymentStatusInt { get; set; }
        public int ApprovedBy { get; set; }
        public DateTime ApprovedDate { get; set; }
        public string Note { get; set; }
        public string ChequeNo { get; set; }
        public int AccountID { get; set; }
        public string BankName { get; set; }
        public string AccountNo { get; set; }
        public string BranchName { get; set; }
        public string BUName { get; set; }
        public string BUCode { get; set; }
        public string ContractorName { get; set; }
        public string ContactPersonName { get; set; }
        public string CurrencyName { get; set; }
        public string CurrencySymbol { get; set; }
        public string AccountCode { get; set; }
        public string AccountHeadName { get; set; }
        public string ApprovedByName { get; set; }
        public int SalesManID { get; set; }
        public string SalesManName { get; set; }
        public string ErrorMessage { get; set; }
        #endregion

        #region Derived Property
        public List<ChartsOfAccount> ChartsOfAccounts { get; set; }
        public List<BankAccount> BankAccounts { get; set; }
        public List<TradingPaymentDetail> TradingPaymentDetails { get; set; }
        public List<Currency> Currencys { get; set; }
        public string PaymentDateSt
        {
            get
            {
                return this.PaymentDate.ToString("dd MMM yyyy");
            }
        }
        public string EncashmentDateSt
        {
            get
            {
                return this.EncashmentDate.ToString("dd MMM yyyy");
            }
        }
        public string ApprovedDateSt
        {
            get
            {
                return this.ApprovedDate.ToString("dd MMM yyyy");
            }
        }


        #endregion

        #region Functions
        public TradingPayment Get(int nTradingPaymentID, int nUserID)
        {
            return TradingPayment.Service.Get(nTradingPaymentID, nUserID);
        }
        public TradingPayment Save(int nUserID)
        {
            return TradingPayment.Service.Save(this, nUserID);
        }

        public TradingPayment Approved(int nUserID)
        {
            return TradingPayment.Service.Approved(this, nUserID);
        }

        public string Delete(int nUserID)
        {
            return TradingPayment.Service.Delete(this, nUserID);
        }
        public static List<TradingPayment> Gets(int nUserID)
        {
            return TradingPayment.Service.Gets(nUserID);
        }
        public static List<TradingPayment> Gets(string sSQl, int nUserID)
        {
            return TradingPayment.Service.Gets(sSQl, nUserID);
        }
        public static List<TradingPayment> GetsInitialTradingPayments(int nBUID, EnumPaymentRefType eTradingPaymentRefType, int TradingPaymentBy, int nUserID)
        {
            return TradingPayment.Service.GetsInitialTradingPayments(nBUID, eTradingPaymentRefType, TradingPaymentBy, nUserID);
        }
        #endregion

        #region Non DB Function
        public string IDInString(List<TradingPayment> oTradingPayments)
        {
            string sReturn = "";
            foreach (TradingPayment oItem in oTradingPayments)
            {
                sReturn = sReturn + oItem.TradingPaymentID.ToString() + ",";
            }
            if (sReturn.Length > 0) sReturn = sReturn.Remove(sReturn.Length - 1, 1);
            return sReturn;
        }
        #endregion

        #region ServiceFactory
        internal static ITradingPaymentService Service
        {
            get { return (ITradingPaymentService)Services.Factory.CreateService(typeof(ITradingPaymentService)); }
        }
        #endregion
    }
    #endregion

    #region ITradingPayment interface
    public interface ITradingPaymentService
    {
        TradingPayment Get(int nTradingPaymentID, int nUserID);
        TradingPayment Save(TradingPayment oTradingPayment, int nUserID);
        TradingPayment Approved(TradingPayment oTradingPayment, int nUserID);
        string Delete(TradingPayment oTradingPayment, int nUserID);
        List<TradingPayment> Gets(int nUserID);
        List<TradingPayment> Gets(string sSQl, int nUserID);
        List<TradingPayment> GetsInitialTradingPayments(int nBUID, EnumPaymentRefType eTradingPaymentRefType, int TradingPaymentBy, int nUserID);
    }
    #endregion
}

