using System;
using System.IO;
using ICS.Core;
using ESimSol.BusinessObjects;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Collections.Generic;
using ICS.Core.Framework;
using ICS.Core.Utility;
namespace ESimSol.BusinessObjects
{

    #region PurchaseRequisition

    public class PurchaseRequisition : BusinessObject
    {
        public PurchaseRequisition()
        {
            PRID = 0;
            PRNo = ""; 
            LastSupplyDate = DateTime.Now;
            PRDate = DateTime.Now;
            RequirementDate = DateTime.Now;
            Note = "";
            RequisitionBy = 0;
            RequisitionByName = "";
            RequisitionByCode = "";
            EmployeeDesignationType = EnumEmployeeDesignationType.None;
            ApproveBy = 0;
            ProductCode = "";
            RequiredFor = "";
            ApproveDate = DateTime.Now;
            BUID = 0;
            RequiremenStatus = 0;
            DepartmentID = 0;
            DepartmentName = "";
            FinishByID = 0;
            CancelByID = 0;
            PriortyLevel = EnumPriortyLevel.Normal;
            PriortyLevelInt = (int)EnumPriortyLevel.Normal;
            ErrorMessage = "";
            ApprovalSequence = 0;
            ApprovalStatus = "";
            bIsReviseWithNo = true;            
            IDNo = "";
            oPRSpecs = new List<PRSpec>();
        }

        #region Properties
        public int PRID { get; set; }
        public int ApprovalSequence { get; set; }
        public int FinishByID { get; set; }
        public int CancelByID { get; set; }
        public int DepartmentID { get; set; }
        public string PRNo { get; set; }
        public string ApprovalStatus { get; set; }
        public string RequisitionByName { get; set; }
        public string FinishByName { get; set; }
        public string CancelByName { get; set; }
        public string RequisitionByCode { get; set; }
        public EnumEmployeeDesignationType EmployeeDesignationType { get; set; }
        public EnumPriortyLevel PriortyLevel { get; set; }
        public int PriortyLevelInt { get; set; }
        public string RequiredFor { get; set; }
        public string ProductCode { get; set; }
        public string DepartmentName { get; set; }
        public DateTime PRDate { get; set; }
        public DateTime LastSupplyDate { get; set; }
        public int RequisitionBy { get; set; }
        public int ApproveBy { get; set; }
        public DateTime RequirementDate { get; set; }
        public DateTime ApproveDate { get; set; }
        public string Note { get; set; }
        public int BUID { get; set; }
        public bool bIsReviseWithNo { get; set; }
        public string IDNo { get; set; }
        public string ErrorMessage { get; set; }
        #endregion

        #region Derived Property
        public string PriortyLevelInString
        {
            get
            {
                return this.PriortyLevel.ToString();
            }
        }
        public string EmployeeDesignationTypeInString
        {
            get
            {
                return this.EmployeeDesignationType.ToString();
            }
        }
        public string ApprovedByName { get; set; }

        public string BUName { get; set; }

        public string BUCode { get; set; }
        
        public string PrepareByName { get; set; }
        public string DeptCode { get; set; }
        public string DeptName { get; set; }
        public List<ContactPersonnel> SCPersons { get; set; }
        public int Status { get; set; }// Add with  NOA or PO
        public int RequiremenStatus { get; set; }// Add with  NOA or PO
        public int TotalDetail { get; set; }// TotalItemDetail
        public int TotalConfirm { get; set; }// TotalItemDetail
        private int _nTotalConfirmPer = 0;
        public string TotalConfirmPer// TotalConfirm Prcentagel
        {
            get
            {
                if (this.TotalConfirm <= 0)
                {
                    _nTotalConfirmPer = 0;
                }
                else
                {
                    _nTotalConfirmPer = (this.TotalConfirm * 100) / this.TotalDetail;
                }
                return _nTotalConfirmPer.ToString() + " %";
            }
        }
        public string PRDateST
        {
            get
            {
                return PRDate.ToString("dd MMM yyyy");
            }
        }
        public string PRStatus
        {
            get
            {
                return (this.Status==4)?"Requested For Revise":this.ApprovalStatus;
            }
        }
        public string RequirementDateSt
        {
            get
            {
                return this.RequirementDate.ToString("dd MMM yyyy");
            }
        }

        private string _sStatus = "";
        public string StatusSt
        {
            get
            {
                _sStatus = ((EnumPurchaseRequisitionStatus)this.Status).ToString();

                return ((EnumPurchaseRequisitionStatus)this.Status).ToString();
            }
        }

       
         

        public List<PurchaseRequisitionDetail> PurchaseRequisitionDetails { get; set; }
        public List<PRSpec> oPRSpecs { get; set; }
        #endregion

        #region Functions

        public static List<PurchaseRequisition> GetsBy(string nStatus, long nUserID)
        {
            return PurchaseRequisition.Service.GetsBy(nStatus, nUserID);
        }
        public static List<PurchaseRequisition> GetsByBU(int nBUID, long nUserID)
        {
            return PurchaseRequisition.Service.GetsByBU(nBUID, nUserID);
        }
        public static List<PurchaseRequisition> Gets(string sSQL, long nUserID)
        {
            return PurchaseRequisition.Service.Gets(sSQL, nUserID);
        }

        public PurchaseRequisition Get(int id, long nUserID)
        {
            return PurchaseRequisition.Service.Get(id, nUserID);
        }

        public PurchaseRequisition Save(long nUserID)
        {
            return PurchaseRequisition.Service.Save(this, nUserID);
        }
        public PurchaseRequisition AcceptRevise(long nUserID)
        {
            return PurchaseRequisition.Service.AcceptRevise(this, nUserID);
        }
        public PurchaseRequisition Approved(long nUserID)
        {
            return PurchaseRequisition.Service.Approved(this, nUserID);
        }
        public PurchaseRequisition UndoRequestRevise(long nUserID)
        {
            return PurchaseRequisition.Service.UndoRequestRevise(this, nUserID);
        }
        public PurchaseRequisition RequestRequisitionRevise(long nUserID)
        {
            return PurchaseRequisition.Service.RequestRequisitionRevise(this, nUserID);
        }
        public PurchaseRequisition Finish(long nUserID)
        {
            return PurchaseRequisition.Service.Finish(this, nUserID);
        }
        public PurchaseRequisition Cancel(long nUserID)
        {
            return PurchaseRequisition.Service.Cancel(this, nUserID);
        }
        public string Delete(int id, long nUserID)
        {
            return PurchaseRequisition.Service.Delete(id, nUserID);
        }
        #endregion

        #region ServiceFactory

        internal static IPurchaseRequisitionService Service
        {
            get { return (IPurchaseRequisitionService)Services.Factory.CreateService(typeof(IPurchaseRequisitionService)); }
        }

        #endregion
    }
    #endregion

    #region IPurchaseRequisition interface

    public interface IPurchaseRequisitionService
    {

        PurchaseRequisition RequestRequisitionRevise(PurchaseRequisition oPurchaseRequisition, Int64 nUserID);
        PurchaseRequisition Get(int id, Int64 nUserID);
        List<PurchaseRequisition> GetsBy(string nStatus, Int64 nUserID);
        List<PurchaseRequisition> GetsByBU(int nBUID, Int64 nUserID);
        List<PurchaseRequisition> Gets(string sSQL, Int64 nUserID);
        string Delete(int id, Int64 nUserID);
        PurchaseRequisition AcceptRevise(PurchaseRequisition oPurchaseRequisition, Int64 nUserID);
        PurchaseRequisition Save(PurchaseRequisition oPurchaseRequisition, Int64 nUserID);        
        PurchaseRequisition UndoRequestRevise(PurchaseRequisition oPurchaseRequisition, Int64 nUserID);
        PurchaseRequisition Approved(PurchaseRequisition oPurchaseRequisition, Int64 nUserID);
        PurchaseRequisition Finish(PurchaseRequisition oPurchaseRequisition, Int64 nUserID);
        PurchaseRequisition Cancel(PurchaseRequisition oPurchaseRequisition, Int64 nUserID);

    }
    #endregion


}
