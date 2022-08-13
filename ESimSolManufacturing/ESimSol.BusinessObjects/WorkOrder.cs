using System;
using ICS.Core.Framework;
using ICS.Core.Utility;
using ICS.Core;
using ESimSol.BusinessObjects;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization;
using System.ServiceModel;
using ESimSol.BusinessObjects.ReportingObject;

namespace ESimSol.BusinessObjects
{
    #region WorkOrder
    public class WorkOrder : BusinessObject
    {
        public WorkOrder()
        {
            WorkOrderID = 0;
            BUID = 0;
            FileNo = "";
            WorkOrderNo = "";
            WorkOrderDate = DateTime.Today;
            ExpectedDeliveryDate = DateTime.Today;
            WorkOrderStatus = EnumWorkOrderStatus.Intialize;
            WorkOrderStatusInt = 0;
            SupplierID = 0;
            ContactPersonID = 0;
            Note = "";
            MerchandiserID = 0;
            ApproveDate = DateTime.Today;
            ApproveBy = 0;
            CurrencyID = 0;
            CRate = 0;
            RateUnit = 1;
            ReviseNo = 0;
            FullFileNo = "";
            SupplierName = "";
            SupplierAddress = "";
            ContactPersonName = "";
            MerchandiserName = "";
            ApproveByName = "";
            CurrencyName = "";
            CurrencySymbol = "";
            Qty = 0;
            Amount = 0;
            YetToGRNQty = 0;
            WorkOrderLogID = 0;
            PreparedByName = "";
            PreparedBy = 0;
            ExpectedDeliveryDate = DateTime.Now;
            WorkOrderDetails = new List<WorkOrderDetail>();
            ReviseRequest = new BusinessObjects.ReviseRequest();
            ErrorMessage = "";
        }

        #region Property
        public int WorkOrderID { get; set; }
        public int BUID { get; set; }
        public string FileNo { get; set; }
        public string WorkOrderNo { get; set; }
        public DateTime WorkOrderDate { get; set; }
        public DateTime ExpectedDeliveryDate { get; set; }
        public EnumWorkOrderStatus WorkOrderStatus { get; set; }
        public int WorkOrderStatusInt { get; set; }
        public int SupplierID { get; set; }
        public int ContactPersonID { get; set; }
        public string Note { get; set; }
        public int MerchandiserID { get; set; }
        public DateTime ApproveDate { get; set; }
        public int ApproveBy { get; set; }
        public int CurrencyID { get; set; }
        public double CRate { get; set; }
        public int RateUnit { get; set; }
        public int ReviseNo { get; set; }
        public string FullFileNo { get; set; }
        public string SupplierName { get; set; }
        public string SupplierAddress { get; set; }
        public string ContactPersonName { get; set; }
        public string MerchandiserName { get; set; }
        public string ApproveByName { get; set; }
        public string CurrencyName { get; set; }
        public string CurrencySymbol { get; set; }
        public string PreparedByName { get; set; }
        public int PreparedBy { get; set; }
        public double Qty { get; set; }
        public double Amount { get; set; }
        public double YetToGRNQty { get; set; }
        public int WorkOrderLogID { get; set; }
        public bool IsNewVersion { get; set; }
        public string ErrorMessage { get; set; }
        #endregion

        #region Derived Property
        public List<WorkOrderDetail> WorkOrderDetails { get; set; }
        public List<WorkOrder> WorkOrderList { get; set; }
        public List<WOTermsAndCondition> WOTermsAndConditions { get; set; }
        public List<SignatureSetup> SignatureSetups { get; set; }
        public EnumWorkOrderActionType WorkOrderActionType { get; set; }
        public ApprovalRequest ApprovalRequest { get; set; }
        public ReviseRequest ReviseRequest { get; set; }
        public Company Company { get; set; }
        public BusinessUnit BusinessUnit { get; set; }
        public string ActionTypeExtra { get; set; }
        public string AmountSt
        {
            get
            {
                return Global.MillionFormat(this.Amount);
            }
        }
        public string ExpectedDeliveryDateSt
        {
            get
            {
                return ExpectedDeliveryDate.ToString("dd MMM yyyy");
            }
        }
        public string WorkOrderDateSt
        {
            get
            {
                return WorkOrderDate.ToString("dd MMM yyyy");
            }
        }
        public string ApproveDateSt
        {
            get
            {
                return ApproveDate.ToString("dd MMM yyyy");
            }
        }
        public string WorkOrderStatusSt
        {
            get
            {
                return this.WorkOrderStatus.ToString();
            }
        }
        public string CRateSt
        {
            get
            {
                return Global.MillionFormat(this.CRate);
            }
        }
        #endregion

        #region Functions
        public static List<WorkOrder> Gets(long nUserID)
        {
            return WorkOrder.Service.Gets(nUserID);
        }
        public static List<WorkOrder> Gets(string sSQL, long nUserID)
        {
            return WorkOrder.Service.Gets(sSQL, nUserID);
        }
        public WorkOrder Get(int id, long nUserID)
        {
            return WorkOrder.Service.Get(id, nUserID);
        }
        public WorkOrder GetByLog(int Logid, long nUserID)
        {
            return WorkOrder.Service.GetByLog(Logid, nUserID);
        }
        public WorkOrder Save(long nUserID)
        {
            return WorkOrder.Service.Save(this, nUserID);
        }
        public WorkOrder BillDone(long nUserID)
        {
            return WorkOrder.Service.BillDone(this, nUserID);
        }
        public WorkOrder AcceptRevise(long nUserID)
        {
            return WorkOrder.Service.AcceptRevise(this, nUserID);
        }
        public WorkOrder ChangeStatus(long nUserID)
        {
            return WorkOrder.Service.ChangeStatus(this, nUserID);
        }
        public string Delete(int id, long nUserID)
        {
            return WorkOrder.Service.Delete(id, nUserID);
        }
        #endregion

        #region ServiceFactory
        internal static IWorkOrderService Service
        {
            get { return (IWorkOrderService)Services.Factory.CreateService(typeof(IWorkOrderService)); }
        }
        #endregion
    }
    #endregion

    #region IWorkOrder interface
    public interface IWorkOrderService
    {
        WorkOrder Get(int id, Int64 nUserID);
        WorkOrder GetByLog(int Logid, Int64 nUserID);
        List<WorkOrder> Gets(Int64 nUserID);      
        List<WorkOrder> Gets(string sSQL, Int64 nUserID);
        string Delete(int id, Int64 nUserID);
        WorkOrder Save(WorkOrder oWorkOrder, Int64 nUserID);
        WorkOrder BillDone(WorkOrder oWorkOrder, Int64 nUserID);
        WorkOrder AcceptRevise(WorkOrder oWorkOrder, Int64 nUserID);

        WorkOrder ChangeStatus(WorkOrder oWorkOrder, Int64 nUserID);

    }
    #endregion
}
