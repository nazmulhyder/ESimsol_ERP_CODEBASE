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
    #region DUDeliveryOrder
    
    public class DUDeliveryOrder : BusinessObject
    {
        public DUDeliveryOrder()
        {
            DUDeliveryOrderID = 0;
            DONo = "";
            ContractorID = 0;
            ContactPersonnelID = 0;
            DODate = DateTime.Now;
            Note = "";
            ApproveDate = DateTime.Now;
            Qty = 0.0;
            DeliveryDate = DateTime.Now;
            OrderType = 0;
            OrderID = 0;
            DOStatus = 0;
            ApproveDate = DateTime.Now;
            ExportPIID = 0;
            IsRaw = false;
            DeliveryDate = DateTime.Now;
            ErrorMessage = "";
            DeliveryPoint = "";
            ContactPersonnelName = "";
            ContractorName = "";
            DeliveryToName = "";
            MKTPName = "";
            ApproveByName = "";
            PreaperByName = "";
            ExportLCNo = "";
            ExportPINo = "";
            OrderNo = "";
            IsRevise = false;
            DONoCode = "";
            BUID = 0;
            DUDeliveryOrderDetails = new List<DUDeliveryOrderDetail>();
        }
       
        #region Properties

        public int DUDeliveryOrderID { get; set; }
        public string DONo { get; set; }
        public DateTime DODate { get; set; }
        public int ContractorID { get; set; }
        public int ContactPersonnelID { get; set; }
        public string Note { get; set; }
        public int OrderType { get; set; }
        public int OrderID { get; set; }
        public int DOStatus { get; set; }
        public int ApproveBy { get; set; }
        public DateTime ApproveDate { get; set; }
        public double Qty { get; set; }
        public int ExportPIID { get; set; }
        public bool IsRaw { get; set; }
        public bool IsRevise { get; set; }
        public DateTime DeliveryDate { get; set; }
        public string ErrorMessage { get; set; }
        public string DeliveryPoint { get; set; }
        public string ContactPersonnelName { get; set; }
      
        #endregion

        #region Derived Property

        public List<DUDeliveryOrderDetail> DUDeliveryOrderDetails { get; set; }
        public string ContractorName { get; set; }
        public string DeliveryToName { get; set; }
        public string MKTPName { get; set; }
        public string ApproveByName { get; set; }
        public string PreaperByName { get; set; }
        public string ExportLCNo { get; set; }
        public string ExportPINo { get; set; }
        
        public string OrderNo { get; set; }
        public string DOStatusSt
        {
            get
            {
                return ((EnumDOStatus)this.DOStatus ).ToString();
            }
        }
        public string DODateSt
        {
            get
            {
                return DODate.ToString("dd MMM yyyy");
            }
        }
        public string DeliveryDateSt
        {
            get
            {
                return this.DeliveryDate.ToString("dd MMM yyyy");
            }
        }

        public int PaymentType { get; set; }

        //public string OrderTypeSt
        //{
        //    get
        //    {
        //        return EnumObject.jGet((EnumOrderType)this.OrderType);
        //    }
        //}
        public string OrderTypeSt { get; set; }
        public string DONoCode { get; set; }
        public string DONoFull
        {
            get
            {
                return this.DONoCode + "" + this.DONo;
                //if(this.OrderType==(int)EnumOrderType.BulkOrder)
                //{
                //    return "BDO"+""+this.DONo;
                //}
                //else if (this.OrderType == (int)EnumOrderType.ClaimOrder)
                //{
                //    return "CODO" + "" + this.DONo;
                //}
                //else if (this.OrderType == (int)EnumOrderType.DyeingOnly)
                //{
                //    return "RODO" + "" + this.DONo;
                //}
                //else 
                //{
                //    return "SDO" + "" + this.DONo;
                //}
            }
        }
  
        public string QtySt
        {
            get
            {
                { return Global.MillionFormat(this.Qty); }

            }
        }
        public double OrderValue { get; set; }
        public double OrderQty { get; set; }
        public double TotalDOQty { get; set; }
        public int BUID { get; set; }
        #endregion

        #region Functions
        public static DUDeliveryOrder Get(int nId, long nUserID)
        {
            return DUDeliveryOrder.Service.Get(nId, nUserID);
        }

        public static List<DUDeliveryOrder> GetsByPaymentType(string sPaymentTypes,long nUserID)
        {
            return DUDeliveryOrder.Service.GetsByPaymentType(sPaymentTypes, nUserID);
        }

        public static List<DUDeliveryOrder> Gets(string sSQL, long nUserID)
        {
            return DUDeliveryOrder.Service.Gets(sSQL, nUserID);
        }
        public static List<DUDeliveryOrder> GetsByNo(string sContractorIDs, long nUserID)
        {
            return DUDeliveryOrder.Service.GetsByNo(sContractorIDs, nUserID);
        }
        public static List<DUDeliveryOrder> GetsBy(string sContractorID, long nUserID)
        {
            return DUDeliveryOrder.Service.GetsBy(sContractorID, nUserID);
        }
        public static List<DUDeliveryOrder> GetsByPI(int nExportPIID, long nUserID)
        {
            return DUDeliveryOrder.Service.GetsByPI(nExportPIID, nUserID);
        }
        public DUDeliveryOrder Save( long nUserID)
        {
            return DUDeliveryOrder.Service.Save(this,  nUserID);
        }
        public DUDeliveryOrder Save_Log(long nUserID)
        {
            return DUDeliveryOrder.Service.Save_Log(this, nUserID);
        }
        public DUDeliveryOrder Approve(long nUserID)
        {
            return DUDeliveryOrder.Service.Approve(this, nUserID);
        }
        public  DUDeliveryOrder DUDeliveryOrderSendToProduction(long nUserID)
        {
            return DUDeliveryOrder.Service.DUDeliveryOrderSendToProduction(this, nUserID);
        }
        public DUDeliveryOrder DOCancel(long nUserID)
        {
            return DUDeliveryOrder.Service.DOCancel(this, nUserID);
        }
        public DUDeliveryOrder UpdateDONo(Int64 nUserID)
        {
            return DUDeliveryOrder.Service.UpdateDONo(this, nUserID);
        }
        public string Delete(long nUserID)
        {
            return DUDeliveryOrder.Service.Delete(this,nUserID);
        }

        public static List<DUDeliveryOrder> DUDeliveryOrderAdjustmentForExportPI(string sDUDeliveryOrderIDs, int nExportPIID, int nDBOperation, long nUserID)
        {
            return DUDeliveryOrder.Service.DUDeliveryOrderAdjustmentForExportPI(sDUDeliveryOrderIDs, nExportPIID, nDBOperation, nUserID);
        }
        #endregion

        #region ServiceFactory
        internal static IDUDeliveryOrderService Service
        {
            get { return (IDUDeliveryOrderService)Services.Factory.CreateService(typeof(IDUDeliveryOrderService)); }
        }
        #endregion
    }


    #region IDUDeliveryOrder interface
    
    public interface IDUDeliveryOrderService
    {
        DUDeliveryOrder Get(int id, long nUserID);
        List<DUDeliveryOrder> GetsByPaymentType(string sPaymentTypes, long nUserID);
        List<DUDeliveryOrder> Gets(string sSQL, long nUserID);
        List<DUDeliveryOrder> GetsByNo(string sOrderNo, long nUserID);
        List<DUDeliveryOrder> GetsBy(string sContractorIDs, long nUserID);
        List<DUDeliveryOrder> GetsByPI(int nExportPIID, long nUserID);
        DUDeliveryOrder Save(DUDeliveryOrder oDUDeliveryOrder,  long nUserID);
        DUDeliveryOrder Save_Log(DUDeliveryOrder oDUDeliveryOrder, long nUserID);
        DUDeliveryOrder Approve(DUDeliveryOrder oDUDeliveryOrder, long nUserID);
        DUDeliveryOrder DOCancel(DUDeliveryOrder oDUDeliveryOrder, long nUserID);
        DUDeliveryOrder UpdateDONo(DUDeliveryOrder oDUDeliveryOrder, Int64 nUserID);
        string Delete(DUDeliveryOrder oDUDeliveryOrder, long nUserID);
        DUDeliveryOrder DUDeliveryOrderSendToProduction(DUDeliveryOrder oDUDeliveryOrder, long nUserID);
        List<DUDeliveryOrder> DUDeliveryOrderAdjustmentForExportPI(string sDUDeliveryOrderIDs, int nExportPIID, int nDBOperation, long nUserID);
        
       
    }
    #endregion

    #endregion
}
