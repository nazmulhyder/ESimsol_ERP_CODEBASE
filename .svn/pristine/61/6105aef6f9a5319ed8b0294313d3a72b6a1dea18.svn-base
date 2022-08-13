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

    #region VoucherBillTransaction
    public class VoucherBillTransaction : BusinessObject
    {
        public VoucherBillTransaction()
        {
            VoucherBillTransactionID = 0;
            VoucherDetailID = 0;
            Amount = 1;
            CurrencyID = 1;
            ConversionRate = 1;
            BillNo = "";
            BillAmount = 0;
            VoucherBillID = 0;
            TransactionDate = DateTime.Today;
            TrType = EnumVoucherBillTrType.None;
            EnumId = 0;
            EnumValue = "";
            BillDate = DateTime.Now;
            CurrencySymbol = "";
            BaseCurrencyID = 0;
            BaseCurrencySymbol = "";
            VoucherNo = "";
            VoucherID = 0;
            AccountHeadID = 0;
            ApprovedBy = 0;

            CCTID = 0;
            CCID = 0;
            CostCenterCode = "";
            CostCenterName = "";
        }

        #region Properties
        public int VoucherBillTransactionID { get; set; }
        public int VoucherBillID { get; set; }
        public long VoucherDetailID { get; set; }
        public double Amount { get; set; }
        public int CurrencyID { get; set; }
        public double ConversionRate { get; set; }
        public DateTime TransactionDate { get; set; }
        public bool IsDr { get; set; }
        public string AccountHeadName { get; set; }
        public string VoucherNo { get; set; }
        public string BillNo { get; set; }
        public string CurrencySymbol { get; set; }
        public double BillAmount { get; set; }
        public string DR_CR { get; set; }
        public int EnumId { get; set; }
        public string EnumValue { get; set; }
        public EnumVoucherBillTrType TrType { get; set; }
        public int BaseCurrencyID { get; set; }
        public string BaseCurrencySymbol { get; set; }
        public DateTime BillDate { get; set; }
        public int VoucherID { get; set; }
        public int AccountHeadID { get; set; }
        public int ApprovedBy { get; set; }
        public int CCTID { get; set; }
        public int CCID { get; set; }
        public string CostCenterCode { get; set; }
        public string CostCenterName { get; set; }

        public string TransactionDateInString{get{return TransactionDate.ToString("dd MMM yyyy");}}
        public string AmountInString{get{return Global.MillionFormat(this.Amount);}}
        public string ExplanationTransactionTypeInString{get{return this.TrType.ToString();}}
        public string DebitAmountInString { get { return this.IsDr ? this.CurrencySymbol + " " + Global.MillionFormat(this.Amount) : "0.00"; } }
        public string CreditAmountInString { get { return !this.IsDr ? this.CurrencySymbol + " " + Global.MillionFormat(this.Amount) : "0.00"; } }
        public string VoucherNoAndID{get{return this.VoucherNo + "~" + this.VoucherID;}}
        public string ConversionRateInString { get { return this.ConversionRate == 1 ? " " : Global.MillionFormat(ConversionRate); } }
        public string TrTypeST { get { return EnumObject.jGet(this.TrType); } }
        #endregion

        #region Functions

        public static List<VoucherBillTransaction> GetsBy(int nVoucherID, int nUserID)
        {
            return VoucherBillTransaction.Service.GetsBy(nVoucherID, nUserID);
        }
        public VoucherBillTransaction Get(int id, int nUserID)
        {
            return VoucherBillTransaction.Service.Get(id, nUserID);
        }
        public VoucherBillTransaction Save(int nUserID)
        {
            return VoucherBillTransaction.Service.Save(this, nUserID);
        }
        public bool Delete(int id, int nUserID)
        {
            return VoucherBillTransaction.Service.Delete(id, nUserID);
        }
        public static List<VoucherBillTransaction> Gets(int nUserID)
        {
            return VoucherBillTransaction.Service.Gets(nUserID);
        }
        public static List<VoucherBillTransaction> Gets(string sSQL, int nUserID)
        {
            return VoucherBillTransaction.Service.Gets(sSQL, nUserID);
        }




        #endregion

        #region ServiceFactory
        internal static IVoucherBillTransactionService Service
        {
            get { return (IVoucherBillTransactionService)Services.Factory.CreateService(typeof(IVoucherBillTransactionService)); }
        }
        #endregion
    }
    #endregion

    #region IVoucherBillTransaction interface
    public interface IVoucherBillTransactionService
    {
        VoucherBillTransaction Get(int id, int nUserID);
        List<VoucherBillTransaction> Gets(int nVoucherDetailID, int nUserID);
        List<VoucherBillTransaction> GetsBy(int nVoucherID, int nUserID);
        List<VoucherBillTransaction> Gets(int nUserID);
        bool Delete(int id, int nUserID);
        VoucherBillTransaction Save(VoucherBillTransaction oVoucherBillTransaction, int nUserID);
        List<VoucherBillTransaction> Gets(string sSQL, int nUserID);
    }
    #endregion
}