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

    #region VPTransaction
    public class VPTransaction : BusinessObject
    {
        public VPTransaction()
        {
            VPTransactionID = 0;
            VoucherDetailID = 0;
            AccountHeadID = 0;
            VoucherID = 0;
            ProductID = 0;
            ProductName = "";
            ProductCode = "";
            WorkingUnitID = 0;
            WorkingUnitName = "";
            MUnitID = 0;
            MUnitName = "";
            Qty = 0;
            UnitPrice = 0;
            CurrencyID = 1;
            ConversionRate = 1;
            TransactionDate = DateTime.Today;
            Description = "";
            CurrencySymbol = "";
        }

        #region Properties
        public int VPTransactionID { get; set; }
        public long VoucherDetailID { get; set; }
        public long AccountHeadID { get; set; }
        public long VoucherID { get; set; }
        public double Qty { get; set; }
        public double UnitPrice { get; set; }
        public int ProductID { get; set; }
        public int WorkingUnitID { get; set; }
        public int CurrencyID { get; set; }
        public double ConversionRate { get; set; }
        public DateTime TransactionDate { get; set; }
        public bool IsDr { get; set; }
        public string AccountHeadName { get; set; }
        public string CurrencySymbol { get; set; }
        public string VoucherNo { get; set; }
        public string ProductName { get; set; }
        public string ProductCode { get; set; }
        public string MUnitName { get; set; }
        public string WorkingUnitName { get; set; }
        public int MUnitID { get; set; }
        public string Description { get; set; }
        public string DR_CR { get; set; }
        public double Amount { get; set; }
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
        public string UnitPriceInString
        {
            get
            {
                return Global.MillionFormat(this.UnitPrice);
            }
        }
        public string QtyInString
        {
            get
            {
                return this.Qty.ToString("#,##0.00####");
            }
        }
        #endregion

        #region Functions
        public static List<VPTransaction> Gets(int nUserID)
        {
            return VPTransaction.Service.Gets(nUserID);
        }
        public static List<VPTransaction> GetsBy(int nVoucherID, int nUserID)
        {
            return VPTransaction.Service.GetsBy(nVoucherID, nUserID);
        }
        public VPTransaction Get(int id, int nUserID)
        {
            return VPTransaction.Service.Get(id, nUserID);
        }
        public VPTransaction Save(int nUserID)
        {
            return VPTransaction.Service.Save(this, nUserID);
        }
        public bool Delete(int id, int nUserID)
        {
            return VPTransaction.Service.Delete(id, nUserID);
        }
        public static List<VPTransaction> Gets(int nVoucherDetailID, int nUserID)
        {
            return VPTransaction.Service.Gets(nVoucherDetailID, nUserID);
        }
        public static List<VPTransaction> Gets(string sSQL, int nUserID)
        {
            return VPTransaction.Service.Gets(sSQL, nUserID);
        }
        #endregion

        #region ServiceFactory
        internal static IVPTransactionService Service
        {
            get { return (IVPTransactionService)Services.Factory.CreateService(typeof(IVPTransactionService)); }
        }
        #endregion
    }
    #endregion

    #region IVPTransaction interface
    public interface IVPTransactionService
    {
        VPTransaction Get(int id, int nUserID);
        List<VPTransaction> Gets(int nVoucherDetailID, int nUserID);
        List<VPTransaction> GetsBy(int nVoucherID, int nUserID);
        List<VPTransaction> Gets(int nUserID);
        bool Delete(int id, int nUserID);
        VPTransaction Save(VPTransaction oVPTransaction, int nUserID);
        List<VPTransaction> Gets(string sSQL, int nUserID);
    }
    #endregion
}
