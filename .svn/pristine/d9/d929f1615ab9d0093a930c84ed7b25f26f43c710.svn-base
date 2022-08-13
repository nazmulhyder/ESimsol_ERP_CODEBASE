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

    #region VoucherReference
    public class VoucherReference : BusinessObject
    {
        public VoucherReference()
        {
            VoucherReferenceID = 0;
            VoucherDetailID = 0;
            Amount = 1;
            CurrencyID = 1;
            CurrencyConversionRate = 1;
            Description = "";
            TransactionDate = DateTime.Today;
            CurrencySymbol = "";

        }

        #region Properties
        public int VoucherReferenceID { get; set; }
        public long VoucherDetailID { get; set; }
        public double Amount { get; set; }
        public string Description { get; set; }
        public DateTime TransactionDate { get; set; }
        public long AccountHeadID { get; set; }
        public int UserID { get; set; }
        public int CurrencyID { get; set; }
        public double CurrencyConversionRate { get; set; }
        public string CurrencySymbol { get; set; }
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
        #endregion

        #region Functions
        public static List<VoucherReference> Gets(int nUserID)
        {
            return VoucherReference.Service.Gets(nUserID);
        }
        public static List<VoucherReference> GetsBy(int nVoucherlID, int nUserID)
        {
            return VoucherReference.Service.GetsBy(nVoucherlID, nUserID);
        }
        public VoucherReference Get(int id, int nUserID)
        {
            return VoucherReference.Service.Get(id, nUserID);
        }
        public VoucherReference Save(int nUserID)
        {
            return VoucherReference.Service.Save(this, nUserID);
        }
        public bool Delete(int id, int nUserID)
        {
            return VoucherReference.Service.Delete(id, nUserID);
        }
        public static List<VoucherReference> Gets(int nVoucherDetailID, int nUserID)
        {
            return VoucherReference.Service.Gets(nVoucherDetailID, nUserID);
        }
        public static List<VoucherReference> Gets(string sSQL, int nUserID)
        {
            return VoucherReference.Service.Gets(sSQL, nUserID);
        }
        #endregion

        #region ServiceFactory
        internal static IVoucherReferenceService Service
        {
            get { return (IVoucherReferenceService)Services.Factory.CreateService(typeof(IVoucherReferenceService)); }
        }
        #endregion
    }
    #endregion

    #region IVoucherReference interface
    public interface IVoucherReferenceService
    {
        VoucherReference Get(int id, int nUserID);
        List<VoucherReference> Gets(int nVoucherDetailID, int nUserID);
        List<VoucherReference> GetsBy(int nVoucherlID, int nUserID);
        List<VoucherReference> Gets(int nUserID);
        bool Delete(int id, int nUserID);
        VoucherReference Save(VoucherReference oVoucherReference, int nUserID);
        List<VoucherReference> Gets(string sSQL, int nUserID);
    }
    #endregion
}
