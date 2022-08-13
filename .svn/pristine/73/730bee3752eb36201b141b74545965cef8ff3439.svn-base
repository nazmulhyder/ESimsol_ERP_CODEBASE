using System;
using System.IO;
using ESimSol.BusinessObjects;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Collections.Generic;
using ICS.Core;
using ICS.Core.Framework;
using ICS.Core.Utility;




namespace ESimSol.BusinessObjects
{
    #region DyeingOrder

    public class DyeingOrder : BusinessObject
    {
        public DyeingOrder()
        {
            DyeingOrderID = 0;
            OrderNo = "";
            RefNo = "";
            ContractorID = 0;
            ContactPersonnelID = 0;
            ContactPersonnelID_DelTo = 0;
            MKTEmpID = 0;
            RefNo = "";
            StyleNo = "";
            Priority = EnumPriorityLevel.None;
            ApproveBy = 0;
            OrderDate = DateTime.Now;
            Note = "";
            ApproveDate = DateTime.Now;
            Qty = 0.0;
            Status = 0;
            Params = "";
            DyeingOrders = new List<DyeingOrder>();
            DeliveryToID = 0;
            PriorityInt = 0;
            Amount = 0;
            ExportPIID = 0;
            ReviseNo = 0;
            SampleInvoiceID = 0;
            ErrorMessage = "";
            ContactPersonnelName = "";
            SampleInvocieNo = "";
            ExportLCNo = "";
            ExportPINo = "";
            StripeOrder = "";
            KnittingStyle = "";
            Gauge = "";
            OrderValue = 0;
            OrderQty = 0;
            TotalDOQty = 0;
            PaymentType = 0;
            PONo = "";
            IsInHouse = true;
            NoCode = "";
            OrderType = "";
            BUID = 0;
            LabdipNo = "";
            LightSourchID = 0;
            LightSourchIDTwo = 0;
            OrderTypeSt = "";
            FSCNo = "";
            FSCDetailID = 0;
            ReviseNote = "";
            ReviseDate = DateTime.MinValue;
            DyeingOrderDetails = new List<DyeingOrderDetail>();
            DeliveryNote = "";
            MBuyer = "";
            MBuyerID = 0;
        }

        #region Properties
        public int DyeingOrderID { get; set; }
        public string OrderNo { get; set; }        
        public string PONo { get; set; } // Without Revise No
        public DateTime OrderDate { get; set; }
        public DateTime ReviseDate { get; set; }
        public int ContractorID { get; set; }
        public int MBuyerID { get; set; }
        public int ContactPersonnelID { get; set; }
        public int DeliveryToID { get; set; }
        public int ContactPersonnelID_DelTo { get; set; }
        public int MKTEmpID { get; set; }
        public string RefNo { get; set; }
        public string ReviseNote { get; set; }
        public string StyleNo { get; set; }
        public EnumPriorityLevel Priority { get; set; }
        public int PriorityInt { get; set; }
        public int LightSourchID { get; set; }
        public int LightSourchIDTwo { get; set; }
        public string Note { get; set; }
        public EnumDyeingStepType DyeingStepType { get; set; }
        public int DyeingStepTypeInt { get; set; }
        public int DyeingOrderType { get; set; }/// enumOrderType
        public int PaymentType { get; set; }
        public int ApproveBy { get; set; }
        public DateTime ApproveDate { get; set; }
        public double Amount { get; set; }
        public double Qty { get; set; }
        public int ExportPIID { get; set; }
        public int ReviseNo { get; set; }
        public bool IsInHouse { get; set; }
        public bool IsClose { get; set; }
        public bool IsCreateReviseNo { get; set; }
        public int SampleInvoiceID { get; set; }
        public string ErrorMessage { get; set; }
        public string Params { get; set; }
        public string ContactPersonnelName { get; set; }
        public string MBuyer { get; set; }
        public string NoCode { get; set; }
        public string DeliveryNote { get; set; }
        public string LabdipNo { get; set; }
        public string OrderType { get; set; }
        public int Status { get; set; }
        public int BUID { get; set; }
        public int FSCDetailID { get; set; }
        public int CurrencyID { get; set; }
        #endregion

        #region Derived Property
        public List<DyeingOrderDetail> DyeingOrderDetails { get; set; }
        public List<DyeingOrder> DyeingOrders { get; set; }
        //public List<DUDyeingOrderStep> DUDyeingOrderSteps { get; set; }
        public string ContractorName { get; set; }
        public string DeliveryToName { get; set; }
        public string MKTPName { get; set; }
        public string CPName { get; set; }
        public string ApproveByName { get; set; }
        public string PreaperByName { get; set; }
        public string SampleInvocieNo { get; set; }
        public string ExportLCNo { get; set; }
        public string ExportPINo { get; set; }
        public string StripeOrder { get; set; }
        public string KnittingStyle { get; set; }
        public string Gauge { get; set; }
        public string FSCNo { get; set; }
        //public int DeliveryZoneID { get; set; }
        public string OrderDateSt
        {
            get
            {
                return OrderDate.ToString("dd MMM yyyy");
            }
        }
        public string ReviseDateSt
        {
            get
            {
                if (this.ReviseDate == DateTime.MinValue) return "";
                else return this.ReviseDate.ToString("dd MMM yyyy");
            }
        }
        public string StateSt
        {
            get
            {
                return ((EnumDyeingOrderState)Status).ToString();
            }
        }        
        public string OrderNoFull
        {
            get
            {
                string sOrderNoFull = "";
                sOrderNoFull = this.NoCode + this.OrderNo;
                return sOrderNoFull;
            }
        }
        public string PaymentTypeSt
        {
            get
            {
                return EnumObject.jGet((EnumOrderPaymentType)this.PaymentType);
            }
        }
        public string OrderTypeSt { get; set; }
      
        public string PrioritySt
        {
            get
            {
                return this.Priority.ToString();
            }
        }
        public string AmountSt
        {
            get
            {
                { return Global.MillionFormat(this.Amount); }

            }
        }
        public string QtySt
        {
            get
            {
                { return Global.MillionFormat(this.Qty); }

            }
        }
        public string IsInHouseSt
        {
            get
            {
                if (this.IsInHouse == true) return "In House";
                else if (this.IsInHouse == false) return "Out Side";
                else return "-";
            }
        }
        public string IsCloseSt
        {
            get
            {
                if (this.IsClose == true) return "Close";
                else if (this.IsClose == false) return "Running ";
                else return "-";
            }
        }

        public double OrderValue { get; set; }
        public double OrderQty { get; set; }
        public double TotalDOQty { get; set; }
        #endregion

        #region Functions
        public static DyeingOrder Get(int nId, long nUserID)
        {
            return DyeingOrder.Service.Get(nId, nUserID);
        }
        public static DyeingOrder GetFSCD(int nFSEDetailID, long nUserID)
        {
            return DyeingOrder.Service.GetFSCD(nFSEDetailID, nUserID);
        }
        public static List<DyeingOrder> GetsByPaymentType(string sPaymentTypes, long nUserID)
        {
            return DyeingOrder.Service.GetsByPaymentType(sPaymentTypes, nUserID);
        }

        public static List<DyeingOrder> Gets(string sSQL, long nUserID)
        {
            return DyeingOrder.Service.Gets(sSQL, nUserID);
        }
        public static List<DyeingOrder> GetsByNo(string sContractorIDs, long nUserID)
        {
            return DyeingOrder.Service.GetsByNo(sContractorIDs, nUserID);
        }
        public static List<DyeingOrder> GetsBy(string sContractorID, long nUserID)
        {
            return DyeingOrder.Service.GetsBy(sContractorID, nUserID);
        }
        public static List<DyeingOrder> GetsByPI(int nExportPIID, long nUserID)
        {
            return DyeingOrder.Service.GetsByPI(nExportPIID, nUserID);
        }
        public static List<DyeingOrder> GetsByInvoice(int nSampleInvoiceID, long nUserID)
        {
            return DyeingOrder.Service.GetsByInvoice(nSampleInvoiceID, nUserID);
        }
        public DyeingOrder Save(long nUserID)
        {
            return DyeingOrder.Service.Save(this, nUserID);
        }
        public DyeingOrder Save_Log(long nUserID)
        {
            return DyeingOrder.Service.Save_Log(this, nUserID);
        }
        public DyeingOrder Approve(long nUserID)
        {
            return DyeingOrder.Service.Approve(this, nUserID);
        }
        public DyeingOrder DOSave_Auto(long nUserID)
        {
            return DyeingOrder.Service.DOSave_Auto(this, nUserID);
        }
        public DyeingOrder DyeingOrderSendToProduction(long nUserID)
        {
            return DyeingOrder.Service.DyeingOrderSendToProduction(this, nUserID);
        }
        public DyeingOrder DOCancel(long nUserID)
        {
            return DyeingOrder.Service.DOCancel(this, nUserID);
        }
        public string Delete(long nUserID)
        {
            return DyeingOrder.Service.Delete(this, nUserID);
        }
        public DyeingOrder CreateServisePI(long nUserID)
        {
            return DyeingOrder.Service.CreateServisePI(this, nUserID);
        }
        public static List<DyeingOrder> DyeingOrderAdjustmentForExportPI(string sDyeingOrderIDs, int nExportPIID, int nDBOperation, long nUserID)
        {
            return DyeingOrder.Service.DyeingOrderAdjustmentForExportPI(sDyeingOrderIDs, nExportPIID, nDBOperation, nUserID);
        }
        public DyeingOrder OrderClose(Int64 nUserID)
        {
            return DyeingOrder.Service.OrderClose(this, nUserID);
        }
        public DyeingOrder UpdateMasterBuyer(Int64 nUserID)
        {
            return DyeingOrder.Service.UpdateMasterBuyer(this, nUserID);
        }

        public DyeingOrder DyeingOrderHistory(long nUserID)
        {
            return DyeingOrder.Service.DyeingOrderHistory(this, nUserID);
        }
        #endregion

        #region ServiceFactory
        internal static IDyeingOrderService Service
        {
            get { return (IDyeingOrderService)Services.Factory.CreateService(typeof(IDyeingOrderService)); }
        }
        #endregion
    }


    #region IDyeingOrder interface

    public interface IDyeingOrderService
    {
        DyeingOrder Get(int id, long nUserID);
        DyeingOrder GetFSCD(int nFSEDetailID, long nUserID);
        List<DyeingOrder> GetsByPaymentType(string sPaymentTypes, long nUserID);
        List<DyeingOrder> Gets(string sSQL, long nUserID);
        List<DyeingOrder> GetsByNo(string sOrderNo, long nUserID);
        List<DyeingOrder> GetsBy(string sContractorIDs, long nUserID);
        List<DyeingOrder> GetsByInvoice(int nSampleInvoiceID, long nUserID);
        List<DyeingOrder> GetsByPI(int nExportPIID, long nUserID);
        DyeingOrder Save(DyeingOrder oDyeingOrder, long nUserID);
        DyeingOrder Save_Log(DyeingOrder oDyeingOrder, long nUserID);
        DyeingOrder Approve(DyeingOrder oDyeingOrder, long nUserID);
        DyeingOrder DOSave_Auto(DyeingOrder oDyeingOrder, long nUserID);
        DyeingOrder DOCancel(DyeingOrder oDyeingOrder, long nUserID);
        string Delete(DyeingOrder oDyeingOrder, long nUserID);
        DyeingOrder DyeingOrderSendToProduction(DyeingOrder oDyeingOrder, long nUserID);
        List<DyeingOrder> DyeingOrderAdjustmentForExportPI(string sDyeingOrderIDs, int nExportPIID, int nDBOperation, long nUserID);
        DyeingOrder CreateServisePI(DyeingOrder oDyeingOrder, long nUserID);
        DyeingOrder OrderClose(DyeingOrder oDyeingOrder, Int64 nUserID);
        DyeingOrder UpdateMasterBuyer(DyeingOrder oDyeingOrder, Int64 nUserID);
        DyeingOrder DyeingOrderHistory(DyeingOrder oDyeingOrder, Int64 nUserID);

    }
    #endregion

    #endregion
}