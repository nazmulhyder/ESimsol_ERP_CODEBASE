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
    #region ChequeRequisition
    public class ChequeRequisition
    {
        #region  Constructor
        public ChequeRequisition()
        {
            ChequeRequisitionID = 0;
            RequisitionNo = "";
            BUID = 0;
            RequisitionStatus = EnumChequeRequisitionStatus.Initialized;
            RequisitionStatusInt = 0;
            RequisitionDate = DateTime.Now;            
            SubledgerID = 0;
            PayTo = 0;
            ChequeDate = DateTime.Now;
            ChequeType = EnumPaymentType.None;
            ChequeTypeInt = 0;
            BankAccountID = 0;
            BankBookID = 0;
            ChequeID = 0;
            ChequeAmount = 0;
            ApprovedBy = 0;
            Remarks = "";
            BUName = "";
            BUCode = "";
            SubledgerName = "";
            SubledgerCode = "";
            ChequeIssueTo = "";
            SecondLineIssueTo = "";
            AccountNo = "";
            BankName = "";
            BranchName = "";
            BookCode = "";
            ChequeNo = "";
            ChequeStatus = EnumChequeStatus.Initiate;
            ApprovedByName = "";
            ErrorMessage = "";
            ChequeRequisitionDetails = new List<ChequeRequisitionDetail>();
            IsWillVoucherEffect = true;
        }
        #endregion

        #region Properties
        public int ChequeRequisitionID { get; set; }
        public string RequisitionNo { get; set; }
        public int BUID { get; set; }
        public EnumChequeRequisitionStatus RequisitionStatus { get; set; }
        public int RequisitionStatusInt { get; set; }
        public DateTime RequisitionDate { get; set; }        
        public int SubledgerID { get; set; }
        public int PayTo { get; set; }
        public DateTime ChequeDate { get; set; }
        public EnumPaymentType ChequeType { get; set; }
        public int ChequeTypeInt { get; set; }
        public int BankAccountID { get; set; }
        public int BankBookID { get; set; }
        public int ChequeID { get; set; }
        public double ChequeAmount { get; set; }
        public int ApprovedBy { get; set; }
        public string Remarks { get; set; }
        public string BUName { get; set; }
        public string BUCode { get; set; }        
        public string SubledgerName { get; set; }
        public string SubledgerCode { get; set; }
        public string ChequeIssueTo { get; set; }
        public string SecondLineIssueTo { get; set; }
        public string AccountNo { get; set; }
        public string BankName { get; set; }
        public string BranchName { get; set; }
        public string BookCode { get; set; }
        public string ChequeNo { get; set; }
        public EnumChequeStatus ChequeStatus { get; set; }
        public string ApprovedByName { get; set; }
        public bool IsWillVoucherEffect { get; set; }
        public string ErrorMessage { get; set; }
        #endregion

        #region Derived Property        
        public List<ChequeRequisitionDetail> ChequeRequisitionDetails { get; set; }
        public string RequisitionDateSt
        {
            get
            {
                return this.RequisitionDate.ToString("dd MMM yyyy");
            }
        }
        public string ChequeDateSt
        {
            get
            {
                return this.ChequeDate.ToString("dd MMM yyyy");
            }
        }
        public string ChequeStatusSt
        {
            get
            {
                return EnumObject.jGet(this.ChequeStatus);
            }
        }
        public string IsWillVoucherEffectSt
        {
            get
            {
                if (this.IsWillVoucherEffect)
                {
                    return "Yes";
                }
                else
                {
                    return "No";
                }
            }
        }
        #endregion

        #region Functions
        public ChequeRequisition Get(int nChequeRequisitionID, int nUserID)
        {
            return ChequeRequisition.Service.Get(nChequeRequisitionID, nUserID);
        }
        public ChequeRequisition Save(int nUserID)
        {
            return ChequeRequisition.Service.Save(this, nUserID);
        }

        public ChequeRequisition Approved(int nUserID)
        {
            return ChequeRequisition.Service.Approved(this, nUserID);
        }

        public string Delete(int nUserID)
        {
            return ChequeRequisition.Service.Delete(this, nUserID);
        }
        public static List<ChequeRequisition> Gets(int nUserID)
        {
            return ChequeRequisition.Service.Gets(nUserID);
        }
        public static List<ChequeRequisition> Gets(string sSQl, int nUserID)
        {
            return ChequeRequisition.Service.Gets(sSQl, nUserID);
        }
        public static List<ChequeRequisition> GetsInitialChequeRequisitions(int nUserID)
        {
            return ChequeRequisition.Service.GetsInitialChequeRequisitions(nUserID);
        }
        public ChequeRequisition UpdateVoucherEffect(long nUserID)
        {
            return ChequeRequisition.Service.UpdateVoucherEffect(this, nUserID);
        }
        #endregion

        #region Non DB Function
        public string IDInString(List<ChequeRequisition> oChequeRequisitions)
        {
            string sReturn = "";
            foreach (ChequeRequisition oItem in oChequeRequisitions)
            {
                sReturn = sReturn + oItem.ChequeRequisitionID.ToString() + ",";
            }
            if (sReturn.Length > 0) sReturn = sReturn.Remove(sReturn.Length - 1, 1);
            return sReturn;
        }
        #endregion

        #region ServiceFactory
        internal static IChequeRequisitionService Service
        {
            get { return (IChequeRequisitionService)Services.Factory.CreateService(typeof(IChequeRequisitionService)); }
        }
        #endregion
    }
    #endregion

    #region IChequeRequisition interface
    public interface IChequeRequisitionService
    {
        ChequeRequisition Get(int nChequeRequisitionID, int nUserID);
        ChequeRequisition Save(ChequeRequisition oChequeRequisition, int nUserID);
        ChequeRequisition Approved(ChequeRequisition oChequeRequisition, int nUserID);
        string Delete(ChequeRequisition oChequeRequisition, int nUserID);
        List<ChequeRequisition> Gets(int nUserID);
        List<ChequeRequisition> Gets(string sSQl, int nUserID);
        List<ChequeRequisition> GetsInitialChequeRequisitions(int nUserID);
        ChequeRequisition UpdateVoucherEffect(ChequeRequisition oChequeRequisition, Int64 nUserID);   
    }
    #endregion
}