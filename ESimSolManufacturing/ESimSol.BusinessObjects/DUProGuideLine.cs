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
    #region DUProGuideLine

    public class DUProGuideLine : BusinessObject
    {
        public DUProGuideLine()
        {
            DUProGuideLineID = 0;
            SLNo = "";
            ChallanNo = "";
            BUID = 0;
            DyeingOrderID = 0;
            ContractorID = 0;
            ContractorType= 2;
            ContractorName = "";
            IssueDate = DateTime.Now;
            DyeingOrderNo = "";
            OrderType = EnumOrderType.None;
            WorkingUnitID = 0;
            ApproveByID = 0;
            ApproveDate = DateTime.MinValue;
            ReceiveByID = 0;
            ReceiveDate = DateTime.MinValue;
            ReturnByID = 0;
            ReturnDate = DateTime.MinValue;
            ReturnByName = "";
            InOutType = EnumInOutType.Receive;
            ProductType = EnumProductNature.Yarn;
            Note = "";
            Qty = 0;
            VehicleNo = "";
            GateInNo = "";
            OrderDate = DateTime.Today;
            this.DUProGuideLineDetails = new List<DUProGuideLineDetail>();
            ErrorMessage = "";
        }

        #region Properties
        public int DUProGuideLineID { get; set; }
        public int DyeingOrderID { get; set; }
        public string DyeingOrderNo { get; set; }
        public string SLNo { get; set; }
        public string StyleNo { get; set; }
        public string RefNo { get; set; }
        public string ChallanNo { get; set; }
        public int BUID { get; set; }
        public string ReceiveStore { get; set; }
        public EnumOrderType OrderType { get; set; }
        public int WorkingUnitID { get; set; }
        public int ApproveByID { get; set; }
        public string ApprovedByName { get; set; }
        public DateTime ApproveDate { get; set; } 
        public int ContractorID { get; set; }
        public string ContractorName { get; set; }
        public DateTime IssueDate { get; set; }
        public string VehicleNo { get; set; }
        public string GateInNo { get; set; }
        public int ReceiveByID { get; set; }
        public double Qty { get; set; }
        public DateTime ReceiveDate { get; set; }
        public string ReceivedByName { get; set; }
        public string Note { get; set; }
        public EnumProductNature ProductType { get; set; }
        public int ApproveBy { get; set; }
        public int ContractorType { get; set; }
        public int ReturnByID { get; set; }
        public int ProductTypeInt { get; set; }
        public DateTime ReturnDate { get; set; }
        public DateTime OrderDate { get; set; }
        public string ReturnByName { get; set; }
        public EnumInOutType InOutType { get; set; }
        public int InOutTypeInt { get; set; }
        public string ErrorMessage { get; set; }
        public List<DUProGuideLineDetail> DUProGuideLineDetails { get; set; }
        #endregion

        #region Derived Property
        //public int ProductTypeInt { get { return (int)ProductType; } }

        public string ProductTypeST
        {
            get { return EnumObject.jGet(this.ProductType); }
        }
        public string InOutTypeST
        {
            get 
            {
                if (this.InOutType == EnumInOutType.Disburse)
                {
                    return "Return";
                }
                else
                {
                    return "Receive";
                }
            }
        }
      public string OrderDateSt
        {
            get
            {
                if (this.OrderDate == DateTime.MinValue)
                {
                    return "-";
                }
                else
                {
                    return OrderDate.ToString("dd MMM yyyy");
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
        public string DUProGuideLineStatus
        {
            get
            {
                if (this.ReceiveByID != 0)
                {
                    if (this.InOutType == EnumInOutType.Disburse)
                    {
                        return "Returned";
                    }
                    else return "Received";
                }
                else if (this.ApproveByID != 0 && this.ReceiveByID == 0)
                {
                    return "Approved";
                }
                else
                {
                    return "Initialized";
                }
            }
        }
        public int OrderTypeInt { get { return (int)OrderType; } }
        public string OrderTypeST { get; set;}
        //public string OrderTypeST { get { return EnumObject.jGet(this.OrderType); } }
        #endregion

        #region Functions
        public static List<DUProGuideLine> Gets(long nUserID)
        {
            return DUProGuideLine.Service.Gets(nUserID);
        }
        public static List<DUProGuideLine> Gets(string sSQL, long nUserID)
        {
            return DUProGuideLine.Service.Gets(sSQL, nUserID);
        }
        public DUProGuideLine Get(int id, long nUserID)
        {
            return DUProGuideLine.Service.Get(id, nUserID);
        }
        public DUProGuideLine Save(long nUserID)
        {
            return DUProGuideLine.Service.Save(this, nUserID);
        }
        public DUProGuideLine Update_ReturnQty(long nUserID)
        {
            return DUProGuideLine.Service.Update_ReturnQty(this, nUserID);
        }
        public DUProGuideLine Approve(Int64 nUserID)
        {
            return DUProGuideLine.Service.Approve(this, nUserID);
        }
        public DUProGuideLine Receive(Int64 nUserID)
        {
            return DUProGuideLine.Service.Receive(this, nUserID);
        }
        public DUProGuideLine Return(Int64 nUserID)
        {
            return DUProGuideLine.Service.Return(this, nUserID);
        }
        public string Delete(long nUserID)
        {
            return DUProGuideLine.Service.Delete(this, nUserID);
        }
        public DUProGuideLine UndoApprove(Int64 nUserID)
        {
            return DUProGuideLine.Service.UndoApprove(this, nUserID);
        }

        #endregion

        #region ServiceFactory
        internal static IDUProGuideLineService Service
        {
            get { return (IDUProGuideLineService)Services.Factory.CreateService(typeof(IDUProGuideLineService)); }
        }
        #endregion

    }
    #endregion

    #region IDUProGuideLine interface

    public interface IDUProGuideLineService
    {
        DUProGuideLine Get(int id, Int64 nUserID);
        List<DUProGuideLine> Gets(string sSQL, long nUserID);
        List<DUProGuideLine> Gets(Int64 nUserID);
        string Delete(DUProGuideLine oDUProGuideLine, Int64 nUserID);
        DUProGuideLine Save(DUProGuideLine oDUProGuideLine, Int64 nUserID);
        DUProGuideLine Update_ReturnQty(DUProGuideLine oDUProGuideLine, Int64 nUserID);
        DUProGuideLine Approve(DUProGuideLine oDUProGuideLine, Int64 nUserID);
        DUProGuideLine UndoApprove(DUProGuideLine oDUProGuideLine, Int64 nUserID);
        DUProGuideLine Receive(DUProGuideLine oDUProGuideLine, Int64 nUserID);
        DUProGuideLine Return(DUProGuideLine oDUProGuideLine, Int64 nUserID);
    }
    #endregion
}
