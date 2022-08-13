using System;
using System.IO;
using ESimSol.BusinessObjects;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Collections.Generic;
using System.Data;
using ICS.Core;
using ICS.Core.Framework;
using ICS.Core.Utility;
using System.Collections;

namespace ESimSol.BusinessObjects
{
    #region DUStatement
    public class DUStatement
    {
        #region  Constructor
        public DUStatement()
        {
            BUID = 0;
            OrderType = 0;
            TitleDate = "";
            StatementType = EnumStatementType.ProductionOrder;
            DyeingOrderDetails = new List<DyeingOrderDetail>();
            ExportPIs = new List<ExportPI>();
            ExportPIDetails = new List<ExportPIDetail>();
            ExportBills = new List<ExportBill>();
            DUDeliveryOrders = new List<DUDeliveryOrder>();
            DUDeliveryOrderDetails = new List<DUDeliveryOrderDetail>();
            DUDeliveryChallans = new List<DUDeliveryChallan>();
            DUDeliveryChallanRegisters = new List<DUDeliveryChallanRegister>();
            DUDeliveryChallanDetails = new List<DUDeliveryChallanDetail>();
            DUClaimOrders = new List<DUClaimOrder>();
            DUClaimOrderDetails = new List<DUClaimOrderDetail>();
            DUDeliveryChallanDetails_Claim = new List<DUDeliveryChallanDetail>();
            DUDeliveryOrderDetails_Claim = new List<DUDeliveryOrderDetail>();
            DUReturnChallanDetails_Claim = new List<DUReturnChallanDetail>();
            DUReturnChallanDetails = new List<DUReturnChallanDetail>();
            SampleInvoices = new List<SampleInvoice>();
            FabricDeliveryChallanDetails = new List<FabricDeliveryChallanDetail>();
            FabricDeliveryChallans = new List<FabricDeliveryChallan>();
            LotParents = new List<LotParent>();
            DURequisitionDetails = new List<DURequisitionDetail>();
            DUProGuideLineDetails_Return = new List<DUProGuideLineDetail>();
            DUProGuideLineDetails_Receive = new List<DUProGuideLineDetail>();
            DUReturnChallans = new List<DUReturnChallan>();
        }
        #endregion

        #region Properties
        public int BUID { get; set; }
        public EnumBusinessUnitType BusinessUnitType { get; set; }
        public string Currency { get; set; }
        public string MUName { get; set; }
        public string PrepareBy { get; set; }
        public double LCValue { get; set; }
        public double PIValue { get; set; }
        public double Qty_YetToDC { get; set; }
        public double Qty_YetToDO { get; set; }
        public double Qty_DC { get; set; }
        public double Qty_RC { get; set; }
        public double Qty_DO { get; set; }
        public double Qty_PO { get; set; }// Production Order
        public double Qty_Total { get; set; }
        public double Qty_Claim { get; set; }
        public EnumStatementType StatementType { get; set; }
        public string Title { get; set; }
        public string TitleNo { get; set; }
        public string TitleDate { get; set; }
        public int OrderType { get; set; }
        public string BuyerName { get; set; }
        public string OrderTypeSt
        {
            get
            {
                return ((EnumOrderType)this.OrderType).ToString();
            }
        }
        public double AcceptanceIssue { get; set; }
        public double AcceptanceRcvd { get; set; }
        public string MaturityDate { get; set; }
        public string PaymentDate { get; set; }
        public double Amount_LC { get; set; }
        public List<ExportBill> ExportBills { get; set; }
        public ExportLC ExportLC { get; set; }
        public Company Company { get; set; }
        public BusinessUnit BusinessUnit { get; set; }
        public List<ExportPI> ExportPIs { get; set; }
        public List<ExportPIDetail> ExportPIDetails { get; set; }
        public List<DyeingOrderDetail> DyeingOrderDetails { get; set; }
        public List<DUDeliveryOrder> DUDeliveryOrders { get; set; }
        public List<DUDeliveryOrderDetail> DUDeliveryOrderDetails { get; set; }
        public List<DUDeliveryOrderDetail> DUDeliveryOrderDetails_Claim { get; set; }
        public List<DUDeliveryChallan> DUDeliveryChallans { get; set; }
        public List<DUDeliveryChallanRegister> DUDeliveryChallanRegisters { get; set; }
        public List<DUDeliveryChallanDetail> DUDeliveryChallanDetails { get; set; }
        public List<DUDeliveryChallanDetail> DUDeliveryChallanDetails_Claim { get; set; }
        public List<DUReturnChallan> DUReturnChallans { get; set; }
        public List<DUReturnChallanDetail> DUReturnChallanDetails { get; set; }
        public List<FabricDeliveryChallanDetail> FabricDeliveryChallanDetails { get; set; }
        public List<FabricDeliveryChallan> FabricDeliveryChallans { get; set; }
        public List<FabricDeliveryOrder> FabricDeliveryOrders { get; set; }
        public List<FabricDeliveryOrderDetail> FabricDeliveryOrderDetails { get; set; }
        public List<DyeingOrderReport> DyeingOrderReports { get; set; }
        public List<SampleInvoice> SampleInvoices { get; set; }
        public List<DUClaimOrderDetail> DUClaimOrderDetails { get; set; }
        public List<DUReturnChallanDetail> DUReturnChallanDetails_Claim { get; set; }
        public List<DUClaimOrder> DUClaimOrders { get; set; }
        public List<LotParent> LotParents { get; set; }
        public List<DURequisitionDetail> DURequisitionDetails { get; set; }
        public List<DUProGuideLineDetail> DUProGuideLineDetails_Receive { get; set; }
        public List<DUProGuideLineDetail> DUProGuideLineDetails_Return { get; set; }
      
        public Hashtable HeaderTable { get; set; }
        public string ErrorMessage { get; set; }
        #endregion

        public DateTime LCOpenDate { get; set; }

        public string LCOpenDateSt { get; set; }
    }
    #endregion

}