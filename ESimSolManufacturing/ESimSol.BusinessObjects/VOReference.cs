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

    #region VOReference
    public class VOReference : BusinessObject
    {
        public VOReference()
        {
            VOReferenceID = 0;
            VoucherDetailID = 0;
            AccountHeadID = 0;
            VoucherID = 0;
            OrderID = 0;
            TransactionDate = DateTime.Today;
            Remarks = "";
            IsDebit = false;
            CurrencyID = 0;
            ConversionRate = 0;
            AmountInCurrency = 0;
            Amount = 0;
            CCTID = 0;
            RefNo = "";
            OrderNo = "";
            OrderDate = DateTime.Now;
            SubledgerID = 0;
            SubledgerName = "";
            ErrorMessage = "";
            VoucherNo = "";
            CurrencyName = "";
            Symbol = "";
            AccountHeadCode = "";
            AccountHeadName = "";
            ComponentID = 0;
        }

        #region Properties
        public int VOReferenceID { get; set; }
        public int VoucherDetailID { get; set; }
        public long AccountHeadID { get; set; }
        public long VoucherID { get; set; }
        public int OrderID { get; set; }
        public DateTime TransactionDate { get; set; }
        public string Remarks { get; set; }
        public bool IsDebit { get; set; }
        public int CurrencyID { get; set; }
        public double ConversionRate { get; set; }
        public double AmountInCurrency { get; set; }
        public double Amount { get; set; }
        public int CCTID { get; set; }
        public string RefNo { get; set; }
        public string OrderNo { get; set; }
        public DateTime OrderDate { get; set; }
        public int SubledgerID { get; set; }
        public string SubledgerName { get; set; }
        public string VoucherNo { get; set; }
        public string CurrencyName { get; set; }
        public string Symbol { get; set; }
        public string AccountHeadCode{ get; set; }
        public string AccountHeadName { get; set; }
        public int ComponentID { get; set; }
        public string ErrorMessage { get; set; }
        #endregion

        #region Derived Property
        public string TransactionDateInString
        {
            get
            {
                return this.TransactionDate.ToString("dd MMM yyyy");
            }
        }
        public string AmountInString
        {
            get
            {
                return Global.MillionFormat(this.Amount);
            }
        }
        public string AmountInCurrencySt
        {
            get
            {
                return Global.MillionFormat(this.AmountInCurrency);
            }
        }
        public string OrderDateSt { get { return this.OrderDate.ToString("dd MMM yyyy"); } }        
        #endregion

        #region Functions
        public static List<VOReference> Gets(int nUserID)
        {
            return VOReference.Service.Gets(nUserID);
        }
        public static List<VOReference> GetsBy(int nVoucherlID, int nUserID)
        {
            return VOReference.Service.GetsBy(nVoucherlID, nUserID);
        }
        public static List<VOReference> GetsByOrder(int nVOrderID, int nUserID)
        {
            return VOReference.Service.GetsByOrder(nVOrderID, nUserID);
        }
        public VOReference Get(int id, int nUserID)
        {
            return VOReference.Service.Get(id, nUserID);
        }
        public VOReference Save(int nUserID)
        {
            return VOReference.Service.Save(this, nUserID);
        }
        public bool Delete(int id, int nUserID)
        {
            return VOReference.Service.Delete(id, nUserID);
        }
        public static List<VOReference> Gets(int nVoucherDetailID, int nUserID)
        {
            return VOReference.Service.Gets(nVoucherDetailID, nUserID);
        }
        public static List<VOReference> Gets(string sSQL, int nUserID)
        {
            return VOReference.Service.Gets(sSQL, nUserID);
        }
        #endregion

        #region ServiceFactory
        internal static IVOReferenceService Service
        {
            get { return (IVOReferenceService)Services.Factory.CreateService(typeof(IVOReferenceService)); }
        }
        #endregion
    }
    #endregion

    #region IVOReference interface
    public interface IVOReferenceService
    {
        VOReference Get(int id, int nUserID);
        List<VOReference> Gets(int nVoucherDetailID, int nUserID);
        List<VOReference> GetsBy(int nVoucherlID, int nUserID);
        List<VOReference> GetsByOrder(int nVOrderID, int nUserID);
        List<VOReference> Gets(int nUserID);
        bool Delete(int id, int nUserID);
        VOReference Save(VOReference oVOReference, int nUserID);
        List<VOReference> Gets(string sSQL, int nUserID);
    }
    #endregion
}
