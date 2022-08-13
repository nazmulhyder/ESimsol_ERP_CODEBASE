using System;
using System.IO;
using System.ComponentModel.DataAnnotations;
using ESimSol.BusinessObjects;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Collections.Generic;
using ICS.Core.Framework;
using ICS.Core.Utility;

namespace ESimSol.BusinessObjects
{
    #region DURequisition

    public class DURequisition : BusinessObject
    {
        public DURequisition()
        {
            DURequisitionID = 0;
            RequisitionNo="";
            ReqDate= DateTime.Now;
            BUID_issue = 0;
            BUID_Receive=0;
            RequisitionType=EnumInOutType.None;
            WorkingUnitID_Issue=0;
            WorkingUnitID_Receive=0;
            RequisitionbyID=0;
            ApprovebyID=0;
            ApproveDate = DateTime.MinValue;
            IssuebyID=0;
            IssueDate = DateTime.MinValue;
            ReceiveByID=0;
            ReceiveDate = DateTime.MinValue;
            OpeartionUnitType = EnumOperationUnitType.None;
            Note = "";
            SetupName = "";
            OrderNo = "";
            IsOpenOrder = false;
            this.DURequisitionDetails = new List<DURequisitionDetail>();
            ErrorMessage = "";
        }

        #region Properties
        public int DURequisitionID { get; set; }
        public string RequisitionNo { get; set; }
        public DateTime ReqDate { get; set; }
        public int BUID_issue { get; set; }
        public int BUID_Receive { get; set; }
        public string IssueStore { get; set; }
        public string ReceiveStore { get; set; }
        public EnumInOutType RequisitionType { get; set; }
        public int WorkingUnitID_Issue { get; set; }
        public int WorkingUnitID_Receive { get; set; }
        public int RequisitionbyID { get; set; }
        public int ApprovebyID { get; set; }
        public string ApprovedByName { get; set; }
        public string SetupName { get; set; }
        public DateTime ApproveDate { get; set; }
        public int IssuebyID { get; set; }
        public DateTime IssueDate { get; set; }
        public string IssuedByName { get; set; }
        public int ReceiveByID { get; set; }
        public DateTime ReceiveDate { get; set; }
        public string RequisitionByName { get; set; }
        public string ReceivedByName { get; set; }
        public string Note { get; set; }
        public EnumOperationUnitType OpeartionUnitType { get; set; }
        public int ApproveBy { get; set; }
        public int DyeingOrderType { get; set; }
        public string Params { get; set; }
        public bool IsOpenOrder { get; set; }
        public string ErrorMessage { get; set; }
        public List<DURequisitionDetail> DURequisitionDetails { get; set; }
        #endregion

        #region Derived Property
        public int BUID { get; set; }
        public string OrderNo { get; set; }
        public string ReqDateST
        {
            get
            {
                if (this.ReqDate == DateTime.MinValue)
                {
                    return "-";
                }
                else
                {
                    return ReqDate.ToString("dd MMM yyyy");
                }
            }
        }
        public string IssueDateST
        {
            get
            {
                if (this.IssueDate == DateTime.MinValue)
                {
                    return "-";
                }
                else
                {
                    return IssueDate.ToString("dd MMM yyyy");
                }
            }
        }
        public string ReceiveDateST
        {
            get
            {
                if (this.ReceiveDate == DateTime.MinValue)
                {
                    return "-";
                }
                else
                {
                    return ReceiveDate.ToString("dd MMM yyyy");
                }
            }
        }
        public string ApproveDateST
        {
            get
            {
                if (this.ApproveDate == DateTime.MinValue)
                {
                    return "-";
                }
                else
                {
                    return ApproveDate.ToString("dd MMM yyyy");
                }
            }
        }

        public string ApproveDateTimeST
        {
            get
            {
                if (this.ApproveDate == DateTime.MinValue)
                {
                    return " ";
                }
                else
                {
                    return ApproveDate.ToString("dd MMM yyyy hh:mm tt");
                }
            }
        }
        public string DURequisitionStatus
        {
            get
            {
                if (this.ReceiveByID != 0)
                {
                    return "Received";
                }
                else if (this.ApproveBy != 0 && this.IssuebyID == 0)
                {
                    return "Approved";
                }
                else if (this.IssuebyID != 0 && this.ReceiveByID == 0)
                {
                    return "Issued";
                }
                else
                {
                    return "Initialized";
                }
            }
        }
        public int RequisitionTypeInt { get { return (int)RequisitionType; } }
        public string RequisitionTypeST { get { return EnumObject.jGet(this.RequisitionType); } }
        public int OperationUnitTypeInt { get { return (int)OpeartionUnitType; } }
        public string OperationUnitTypeST { get { return EnumObject.jGet(this.OpeartionUnitType); } }
        #endregion

        #region Functions
        public static List<DURequisition> Gets(long nUserID)
        {
            return DURequisition.Service.Gets(nUserID);
        }
        public static List<DURequisition> Gets(string sSQL, long nUserID)
        {
            return DURequisition.Service.Gets(sSQL, nUserID);
        }
        public DURequisition Get(int id, long nUserID)
        {
            return DURequisition.Service.Get(id, nUserID);
        }

        public DURequisition Save(long nUserID)
        {
            return DURequisition.Service.Save(this, nUserID);
        }
        public DURequisition Issue(Int64 nUserID)
        {
            return DURequisition.Service.Issue(this, nUserID);
        }
        public DURequisition UndoIssue(Int64 nUserID)
        {
            return DURequisition.Service.UndoIssue(this, nUserID);
        }
        public DURequisition Receive(Int64 nUserID)
        {
            return DURequisition.Service.Receive(this, nUserID);
        }
        public DURequisition Approve(Int64 nUserID)
        {
            return DURequisition.Service.Approve(this, nUserID);
        }
        public DURequisition UndoApprove(Int64 nUserID)
        {
            return DURequisition.Service.UndoApprove(this, nUserID);
        }
        public string Delete(long nUserID)
        {
            return DURequisition.Service.Delete(this, nUserID);
        }

        #endregion

        #region ServiceFactory
        internal static IDURequisitionService Service
        {
            get { return (IDURequisitionService)Services.Factory.CreateService(typeof(IDURequisitionService)); }
        }
        #endregion


    }
    #endregion

    #region IDURequisition interface

    public interface IDURequisitionService
    {
        DURequisition Get(int id, Int64 nUserID);
        List<DURequisition> Gets(string sSQL, long nUserID);
        List<DURequisition> Gets(Int64 nUserID);
        string Delete(DURequisition oDURequisition, Int64 nUserID);
        DURequisition Save(DURequisition oDURequisition, Int64 nUserID);
        DURequisition Approve(DURequisition oDURequisition, Int64 nUserID);
        DURequisition UndoApprove(DURequisition oDURequisition, Int64 nUserID);
        DURequisition Issue(DURequisition oDURequisition, Int64 nUserID);
        DURequisition UndoIssue(DURequisition oDURequisition, Int64 nUserID);
        DURequisition Receive(DURequisition oDURequisition, Int64 nUserID);
    
    }
    #endregion
}
