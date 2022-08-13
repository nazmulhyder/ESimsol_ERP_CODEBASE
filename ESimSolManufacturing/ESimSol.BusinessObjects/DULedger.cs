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
    
    public class DULedger : BusinessObject
    {
        public DULedger()
        {
            DyeingOrderID = 0;
            OrderNo = "";
            ContractorID = 0;
            MKTEmpID = 0;
            OrderDate=DateTime.Now;
            Qty = 0.0;
            Status = 0;
            Amount = 0;
            ExportPIID = 0;
            SampleInvoiceID = 0;
            ErrorMessage = "";
            SampleInvocieNo = "";
            ExportPINo = "";
            Qty_DC = 0;
            Amount_DC = 0;
            Qty_Paid = 0;
            Amount_Paid = 0;
            PaymentType = 0;
            OrderType = "";
            BUID = 0;
            OrderTypeSt = this.DyeingOrderTypeSt;
        }
       
        #region Properties
        public int DyeingOrderID { get; set; }
        public string OrderNo { get; set; }
        public DateTime OrderDate { get; set; }
        public int ContractorID { get; set; }
        public int MKTEmpID { get; set; }
        public int MUnitID { get; set; }
        public int DyeingOrderType { get; set; }/// enumOrderType
        public int PaymentType { get; set; }
        public double Amount { get; set; }
        public double Qty { get; set; }
        public double Qty_DC { get; set; }
        public double Amount_DC { get; set; }
        public double Qty_Paid { get; set; }
        public double Amount_Paid { get; set; }
        public int ExportPIID { get; set; }
        public int SampleInvoiceID { get; set; }
        public string ErrorMessage { get; set; }
        public string Params { get; set; }
        public string OrderType { get; set; }
        public int Status { get; set; }
        public int BUID { get; set; }
        #endregion

        #region Derived Property
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public int ViewType { get; set; }
        public int Layout { get; set; }
        public int CurrencyID { get; set; }
        public int MKT_CPID { get; set; }
        public int OrderCount { get; set; }
        public string LCNo { get; set; }
        public string MUName { get; set; }
        public string CurrencyName { get; set; }
        public string CurrencySymbol { get; set; }
        public string ContractorName { get; set; }
        public string MKTPName { get; set; }
        public string SampleInvocieNo { get; set; }
        public string ExportPINo { get; set; }
        public string StartDateSt
        {
            get
            {
                return StartDate.ToString("dd MMM yyyy");
            }
        }
        public string EndDateSt
        {
            get
            {
                return EndDate.ToString("dd MMM yyyy");
            }
        }
        public string OrderDateSt
        {
            get
            {
                return OrderDate.ToString("dd MMM yyyy");
            }
        }
        public string StateSt
        {
            get
            {
                return ((EnumDyeingOrderState)Status).ToString();
            }
        }
        public string PaymentTypeSt
        {
            get
            {
                return EnumObject.jGet((EnumOrderPaymentType)this.PaymentType);
            }
        }
        public string DyeingOrderTypeSt
        {
            get
            {
                return EnumObject.jGet((EnumOrderType)this.DyeingOrderType);
            }
        }
        public string OrderTypeSt { get; set; }
        public string PaymentTypeSet { get; set; }
        public string AmountSt { get; set; }
        public string QtySt { get; set; }
        public string Qty_YetToSt { get; set; }
        public string Amount_YetToSt { get; set; }
        public string Qty_DCSt { get; set; }
        public string Amount_DCSt { get; set; }
        public string Amount_PaidSt { get; set; }

        public double Qty_Yet { get { return (this.Qty - this.Qty_DC); } }
        public double Amount_Yet { get { return (this.Amount - this.Amount_Paid); } }
        

        #endregion

        #region Functions
        public static List<DULedger> Gets(DULedger oDULedger, long nUserID)
        {
            return DULedger.Service.Gets(oDULedger, nUserID);
        }
     
        #endregion

        #region ServiceFactory
        internal static IDULedgerService Service
        {
            get { return (IDULedgerService)Services.Factory.CreateService(typeof(IDULedgerService)); }
        }
        #endregion
    }

    #region IDyeingOrder interface

    public interface IDULedgerService
    {
        List<DULedger> Gets(DULedger oDULedger, long nUserID);
    }
    #endregion

    #endregion
}

