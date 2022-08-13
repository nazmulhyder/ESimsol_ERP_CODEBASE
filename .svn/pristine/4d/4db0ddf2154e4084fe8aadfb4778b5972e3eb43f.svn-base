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
    #region ChequeRequisitionDetail
    public class ChequeRequisitionDetail
    {
        #region  Constructor
        public ChequeRequisitionDetail()
        {
            ChequeRequisitionDetailID = 0;
            ChequeRequisitionID = 0;
            VoucherBillID = 0;
            Amount = 0;
            Remarks = "";
            BillNo = "";
            BillDate = DateTime.Now;
            AccountHeadName = "";
            BillAmount = 0;
            RemainningBalance = 0;
            YetToChequeRequisition = 0;
            ErrorMessage = "";

        }
        #endregion

        #region Properties
        public int ChequeRequisitionDetailID { get; set; }
        public int ChequeRequisitionID { get; set; }
        public int VoucherBillID { get; set; }
        public double Amount { get; set; }
        public string Remarks { get; set; }
        public string BillNo { get; set; }
        public DateTime BillDate { get; set; }
        public string AccountHeadName { get; set; }
        public double BillAmount { get; set; }
        public double RemainningBalance { get; set; }
        public double YetToChequeRequisition { get; set; }
        public string ErrorMessage { get; set; }
        #endregion

        #region Derived Property
        public string BillDateSt
        {
            get
            {
                return this.BillDate.ToString("dd MMM yyyy");
            }
        }
        public string BillAmountSt
        {
            get
            {
                return Global.TakaFormat(this.BillAmount);
            }
        }
        public string YetToChequeRequisitionSt
        {
            get
            {
                return Global.TakaFormat(this.YetToChequeRequisition);
            }
        }

        #endregion

        #region Functions
        public ChequeRequisitionDetail Get(int nChequeRequisitionDetailID, int nUserID)
        {
            return ChequeRequisitionDetail.Service.Get(nChequeRequisitionDetailID, nUserID);
        }
        public ChequeRequisitionDetail Save(int nUserID)
        {
            return ChequeRequisitionDetail.Service.Save(this, nUserID);
        }
        public string Delete(int nUserID)
        {
            return ChequeRequisitionDetail.Service.Delete(this, nUserID);
        }
        public static List<ChequeRequisitionDetail> Gets(int nUserID)
        {
            return ChequeRequisitionDetail.Service.Gets(nUserID);
        }
        public static List<ChequeRequisitionDetail> Gets(int nChequeRequisitionID, int nUserID)
        {
            return ChequeRequisitionDetail.Service.Gets(nChequeRequisitionID, nUserID);
        }
        public static List<ChequeRequisitionDetail> Gets(string sSQl, int nUserID)
        {
            return ChequeRequisitionDetail.Service.Gets(sSQl, nUserID);
        }
        #endregion

        #region Non DB Function
        public string IDInString(List<ChequeRequisitionDetail> oChequeRequisitionDetails)
        {
            string sReturn = "";
            foreach (ChequeRequisitionDetail oItem in oChequeRequisitionDetails)
            {
                sReturn = sReturn + oItem.ChequeRequisitionDetailID.ToString() + ",";
            }
            if (sReturn.Length > 0) sReturn = sReturn.Remove(sReturn.Length - 1, 1);
            return sReturn;
        }
        #endregion

        #region ServiceFactory
        internal static IChequeRequisitionDetailService Service
        {
            get { return (IChequeRequisitionDetailService)Services.Factory.CreateService(typeof(IChequeRequisitionDetailService)); }
        }
        #endregion
    }
    #endregion

    #region IChequeRequisitionDetail interface
    public interface IChequeRequisitionDetailService
    {
        ChequeRequisitionDetail Get(int id, int nUserID);
        List<ChequeRequisitionDetail> Gets(int nUserID);
        List<ChequeRequisitionDetail> Gets(string sSQL, int nUserID);
        List<ChequeRequisitionDetail> Gets(int nChequeRequisitionID, int nUserID);
        ChequeRequisitionDetail Save(ChequeRequisitionDetail oChequeRequisitionDetail, int nUserID);
        string Delete(ChequeRequisitionDetail oChequeRequisitionDetail, int nUserID);
    }
    #endregion
}