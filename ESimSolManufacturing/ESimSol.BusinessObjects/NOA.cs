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
    #region NOA Bill of Quantity
    public class NOA : BusinessObject
    {
        public NOA()
        {
            NOALogID = 0;
            NOAID = 0;
            NOANo = "";
            NOADate = DateTime.Now;
            PrepareBy = 0;
            ApprovedByName = "";
            ApproveBy = 0;
            PrepareByName = "";
            ApproveDate = DateTime.Now;
            Note = "";
            EmployeeName = "";
            BUID = 0;
            BUCode = "";
            BUName = "";
            NOADetailLst = new List<NOADetail>();
            NOASignatoryComments = new List<NOASignatoryComment>();
            NOARequisitionList = new List<NOARequisition>();
            NOAQuotationList = new List<NOAQuotation>();
            PurchaseQuotationDetailList = new List<PurchaseQuotationDetail>();
            Note_History = "";
            Remarks = "";
            PQSpecs = new List<PQSpec>();
            NOASpecs = new List<NOASpec>();
            DiscountInAmount = 0;
            DiscountInPercent = 0;
            NOADetailCount = 0;
            ApprovalHead = "";
        }

        #region Properties
        public int NOAID { get; set; }
        public int NOALogID { get; set; }
        public string NOANo { get; set; }
        public string Remarks { get; set; }
        public DateTime NOADate { get; set; }
        public string ErrorMessage { get; set; }
        public int PrepareBy { get; set; }
        public string ApprovedByName { get; set; }
        public int ApproveBy { get; set; }
        public string PrepareByName { get; set; }
        public DateTime ApproveDate { get; set; }
        public string Note { get; set; }
        public string Note_History { get; set; }
        public string EmployeeName { get; set; }
        public int BUID { get; set; }
       public string BUCode { get; set; }
        public string BUName { get; set; }
        public string ApprovalHead { get; set; }
        #endregion

        #region Derive Property

        public List<SupplierRateProcess> SupplierRateProcess { get; set; }
        public string ApproveDateInString
        {
            get { return this.ApproveDate.ToString("dd MMM yyyy"); }
        }
        public string NOADateInString
        {
            get { return this.NOADate.ToString("dd MMM yyyy"); }
        }
        public List<NOADetail> NOADetailLst { get; set; }
        public List<NOASignatoryComment> NOASignatoryComments { get; set; }
        public List<NOARequisition> NOARequisitionList { get; set; }
        public List<NOAQuotation> NOAQuotationList { get; set; }
        public List<PurchaseQuotationDetail> PurchaseQuotationDetailList { get; set; }
        public List<NOA> NOAList { get; set; }
        public Company Company { get; set; }
        public List<PQSpec> PQSpecs { get; set; }
        public List<NOASpec> NOASpecs { get; set; }
        public double DiscountInPercent { get; set; }
        public double DiscountInAmount { get; set; }
        public int NOADetailCount { get; set; }
        #endregion

        #region Functions
    
        public static List<NOA> GetsWaitForApproval(int nUserID)
        {
            return NOA.Service.GetsWaitForApproval(nUserID);
        }
        public static List<NOA> Gets(string sSQL, int nUserID)
        {
            return NOA.Service.Gets(sSQL, nUserID);
        }

        public NOA Get(long id, int nUserID)
        {
            return NOA.Service.Get(id, nUserID);
        }
        public NOA GetByLog(long id, int nUserID)
        {
            return NOA.Service.GetByLog(id, nUserID);
        }

        public NOA Save(int nUserID)
        {
            return NOA.Service.Save(this, nUserID);
        }

        public NOA UndoApprove(int nUserID)
        {
            return NOA.Service.UndoApprove(this, nUserID);
        }
        public NOA Approve(int nUserID)
        {
            return NOA.Service.Approve(this, nUserID);
        }

        public NOA RequestNOARevise(long nUserID)
        {
            return NOA.Service.RequestNOARevise(this, nUserID);
        }
        public NOA AcceptRevise(long nUserID)
        {
            return NOA.Service.AcceptRevise(this, nUserID);
        }
      
        public string Delete(int id, int nUserID)
        {
            return NOA.Service.Delete(id, nUserID);
        }
        #endregion

        #region ServiceFactory
        internal static INOAService Service
        {
            get { return (INOAService)Services.Factory.CreateService(typeof(INOAService)); }
        }
        #endregion
    }
    #endregion

    #region Report Study
    public class NOAList : List<NOA>
    {
        public string ImageUrl { get; set; }
    }
    #endregion

    #region INOA interface
    public interface INOAService
    {
        NOA RequestNOARevise(NOA oNOA, Int64 nUserID);
        NOA AcceptRevise(NOA oNOA, Int64 nUserID);
        NOA Get(long id, int nUserID);
        NOA GetByLog(long id, int nUserID);
        List<NOA> GetsWaitForApproval(int nUserID);
        List<NOA> Gets(string sSQL, int nUserID);
        string Delete(int id, int nUserID);
        NOA Save(NOA oNOA, int nUserID);
        NOA UndoApprove(NOA oNOA, int nUserID);
        NOA Approve(NOA oNOA, int nUserID);

    }
    #endregion
    
    

}
