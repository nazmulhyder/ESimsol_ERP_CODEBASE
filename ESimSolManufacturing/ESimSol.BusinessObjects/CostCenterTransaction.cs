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
    #region CostCenterTransaction
    public class CostCenterTransaction : BusinessObject
    {
        public CostCenterTransaction()
        {
            CCTID = 0;
            CCID = 0;
            AccountHeadID = 0;
            VoucherDetailID = 0;
            VoucherID = 0;
            Amount = 1;
            CurrencyID = 1;
            CurrencyConversionRate = 1;
            Description = "";
            TransactionDate = DateTime.Today;
            LastUpdateBY = 0;
            LastUpdateDate = DateTime.Today;            
            CCCategoryID = 0;
            CurrencySymbol = "";
            CompanyID = 0;
            CostCenterCode = "";
            IsBillRefApply = false;
            IsOrderRefApply = false;
            IsChequeApply = false;
            VBTransactions = new List<VoucherBillTransaction>();
            VOReferences = new List<VOReference>();
            VoucherCheques = new List<VoucherCheque>();
            AmountBC = 0;
        }

        #region Properties
        public int CCTID { get; set; }
        public int CCID { get; set; }
        public int AccountHeadID { get; set; }
        public long VoucherDetailID { get; set; }
        public int VoucherID { get; set; }
        public double Amount { get; set; }
        public int CurrencyID { get; set; }
        public double CurrencyConversionRate { get; set; }
        public string Description { get; set; }
        public DateTime TransactionDate { get; set; }
        public int LastUpdateBY { get; set; }
        public bool IsBillRefApply { get; set; }
        public bool IsOrderRefApply { get; set; }
        public bool IsChequeApply { get; set; }
        public int CCCategoryID { get; set; }
        public string CurrencySymbol { get; set; }
        public DateTime LastUpdateDate { get; set; }        
        public List<Currency> LstCurrency { get; set; }
        public string CostCenterName { get; set; }
        public string CostCenterCode { get; set; }
        public string AccountHeadName { get; set; }
        public string VoucherNo { get; set; }
        public int CompanyID { get; set; }
        public double CurrentBalance { get; set; }
        public bool  IsDr { get; set; }
        public string CategoryName { get; set; }
        public double AmountBC { get; set; }
        public string TransactionDateInString
        {
            get
            {
                return TransactionDate.ToString("dd MMM yyyy");
            }
        }
        public string AmountInString
        {
            get
            {
                return Global.MillionFormat(this.Amount);
            }
        }

        private string sCurrentBalance = "";
        public string CurrentBalanceSt
        {
            get
            {
                string sIsDr = "";
                if (this.CurrentBalance<0)
                {
                    sIsDr = "Cr";
                    sCurrentBalance = Global.MillionFormat((-1)*this.CurrentBalance) + " " + sIsDr;
                }
                else
                {
                    sIsDr = "Dr";
                    sCurrentBalance = Global.MillionFormat(this.CurrentBalance) + " " + sIsDr;
                }
                
                return sCurrentBalance;
            }
        }
        private string sIsDrSt = "";
        public string IsDrSt
        {
            get
            {
                if (this.IsDr)
                {
                    sIsDrSt = "Dr";
                  
                }
                else
                {
                    sIsDrSt = "Cr";
                    
                }
                return sIsDrSt;
            }
        }
        public List<VoucherBillTransaction> VBTransactions { get; set; }
        public List<VOReference> VOReferences { get; set; }
        public List<VoucherCheque> VoucherCheques { get; set; }
        #endregion

        #region Functions
        public static List<CostCenterTransaction> Gets(int nVoucherDetailID, int nUserID)
        {
            return CostCenterTransaction.Service.Gets(nVoucherDetailID, nUserID);
        }
        public static List<CostCenterTransaction> GetsBy(int nVoucherID, int nUserID)
        {
            return CostCenterTransaction.Service.GetsBy(nVoucherID, nUserID);
        }
        public static List<CostCenterTransaction> GetsByAcccountHead(int nAccountHeadID, int nUserID)
        {
            return CostCenterTransaction.Service.GetsByAcccountHead(nAccountHeadID, nUserID);
        }
        public CostCenterTransaction Get(int id, int nUserID)
        {
            return CostCenterTransaction.Service.Get(id, nUserID);
        }
        public CostCenterTransaction Save(int nUserID)
        {
            return CostCenterTransaction.Service.Save(this, nUserID);
        }
        public bool Delete(int id, int nUserID)
        {
            return CostCenterTransaction.Service.Delete(id, nUserID);
        }
        public static List<CostCenterTransaction> Gets(int nUserID)
        {
            return CostCenterTransaction.Service.Gets(nUserID);
        }
        public static List<CostCenterTransaction> GetsbyCC(string sSQL, int nUserID)
        {
            return CostCenterTransaction.Service.GetsbyCC(sSQL, nUserID);
        }
        public static List<CostCenterTransaction> Gets(string sSQL, int nUserID)
        {
            return CostCenterTransaction.Service.Gets(sSQL, nUserID);
        }
        public static double GetCurrentBalance(int nCCID, int nCurrencyID, bool bIsApproved, int nUserId)
        {
            return CostCenterTransaction.Service.GetCurrentBalance(nCCID, nCurrencyID, bIsApproved, nUserId);
        }
        #endregion

        #region ServiceFactory
        internal static ICostCenterTransactionService Service
        {
            get { return (ICostCenterTransactionService)Services.Factory.CreateService(typeof(ICostCenterTransactionService)); }
        }
        #endregion
    }
    #endregion

    #region ICostCenterTransaction interface
    public interface ICostCenterTransactionService
    {
        CostCenterTransaction Get(int id, int nUserID);
        List<CostCenterTransaction> Gets(int nVoucherDetailID, int nUserID);
        List<CostCenterTransaction> Gets(string sSQL, int nUserID);
        List<CostCenterTransaction> GetsBy(int nVoucherID, int nUserID);
        List<CostCenterTransaction> Gets(int nUserID);
        bool Delete(int id, int nUserID);
        CostCenterTransaction Save(CostCenterTransaction oCostCenterTransaction, int nUserID);
        List<CostCenterTransaction> GetsbyCC(string sSQL, int nUserID);
        List<CostCenterTransaction> GetsByAcccountHead(int nAccountHeadID, int nUserID);
        double GetCurrentBalance(int nCCID, int nCurrencyID, bool bIsApproved, int nUserId);
    }
    #endregion
}
