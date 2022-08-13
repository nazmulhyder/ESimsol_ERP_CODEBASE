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
    #region VoucherCheque
    public class VoucherCheque : BusinessObject
    {
        public VoucherCheque()
        {
            VoucherChequeID = 0;
            VoucherDetailID = 0;
            AccountHeadID = 0;
            VoucherID = 0;
            ChequeType = EnumChequeType.None;            
            ChequeID = 0;
            TransactionDate = DateTime.Now;
            Amount = 0;
            AccountHeadID = 0;
            CCTID = 0;
            AccountHeadName = "";
            AccountCode = "";
            ChequeDate = DateTime.Now;
            ChequeNo = "";
            BankName = "";
            BranchName = "";
            AccountNo = "";
            CCID = 0;
            CostCenterCode = "";
            CostCenterName = "";
        }

        #region Properties
        public int VoucherChequeID { get; set; }
        public int VoucherDetailID { get; set; }
        public int CCTID { get; set; }
        public EnumChequeType ChequeType { get; set; }        
        public int ChequeID { get; set; }
        public DateTime TransactionDate { get; set; }
        public double Amount { get; set; }
        public int AccountHeadID { get; set; }
        public int VoucherID { get; set; }
        public string AccountHeadName { get; set; }
        public string AccountCode { get; set; }
        public string ChequeNo { get; set; }
        public DateTime ChequeDate { get; set; }
        public string BankName { get; set; }
        public string BranchName { get; set; }
        public string AccountNo { get; set; }

        public int CCID { get; set; }
        public string CostCenterName { get; set; }
        public string CostCenterCode { get; set; }
        public string TransactionDateInString { get { return TransactionDate.ToString("dd MMM yyyy"); } }
        public string ChequeDateInString { get { return ChequeType == EnumChequeType.Cash ? "Date" : ChequeDate.ToString("dd MMM yyyy"); } }
        public string AmountInString
        {
            get
            {
                return Global.MillionFormat(this.Amount);
            }
        }       
        #endregion

        #region Functions

        public static List<VoucherCheque> GetsBy(int nVoucherID, int nUserID)
        {
            return VoucherCheque.Service.GetsBy(nVoucherID, nUserID);
        }
        public VoucherCheque Get(int id, int nUserID)
        {
            return VoucherCheque.Service.Get(id, nUserID);
        }
        public VoucherCheque Save(int nUserID)
        {
            return VoucherCheque.Service.Save(this, nUserID);
        }
        public bool Delete(int id, int nUserID)
        {
            return VoucherCheque.Service.Delete(id, nUserID);
        }
        public static List<VoucherCheque> Gets(int nUserID)
        {
            return VoucherCheque.Service.Gets(nUserID);
        }
        public static List<VoucherCheque> Gets(string sSQL, int nUserID)
        {
            return VoucherCheque.Service.Gets(sSQL, nUserID);
        }
        #endregion

        #region ServiceFactory
        internal static IVoucherChequeService Service
        {
            get { return (IVoucherChequeService)Services.Factory.CreateService(typeof(IVoucherChequeService)); }
        }
        #endregion
    }
    #endregion

    #region IVoucherCheque interface
    public interface IVoucherChequeService
    {
        VoucherCheque Get(int id, int nUserID);
        List<VoucherCheque> Gets(int nVoucherDetailID, int nUserID);
        List<VoucherCheque> GetsBy(int nVoucherID, int nUserID);
        List<VoucherCheque> Gets(int nUserID);
        bool Delete(int id, int nUserID);
        VoucherCheque Save(VoucherCheque oVoucherCheque, int nUserID);
        List<VoucherCheque> Gets(string sSQL, int nUserID);
    }
    #endregion
}
