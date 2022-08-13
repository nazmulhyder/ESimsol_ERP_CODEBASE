using System;
using System.IO;
using ESimSol.BusinessObjects;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Collections.Generic;
using System.Data;
using ICS.Core.Framework;
using ICS.Core.Utility;

namespace ESimSol.BusinessObjects
{
    public class ImportOutStandingDetail
    {
        public ImportOutStandingDetail() 
        {
        LCID =0;
        InvoiceID = 0; 
        InvoiceType =0;
        ProductID =0;
        LCNo = string.Empty; 
        InvoiceNo =string.Empty;
        ProductName = string.Empty;
        MUnit = string.Empty;
        ProductCode =string.Empty;
        ContractorID =0;
        ContractorName =string.Empty;
        Qty = 0;
        UnitPrice =0;
        Qty_Invoice = 0;
        Qty_PI =0;
        ImportPIID =0;
        ImportPIDetailID =0;
        PINo =string.Empty;
        LotNo =string.Empty;
        WUName =string.Empty;
        ParentType =0;
        ParentID =0;
        WorkingUnitID =0;
        Qty_TR = 0; 
        GRNID =0;
        GRNDetailID =0;
        GRNNo =string.Empty;
        AgentName =string.Empty;
        Qty_StockIn=0;
        Qty_Short =0;
        ImportInvoiceDetailID  =0;
        BLNo =string.Empty;
        BLDate = DateTime.Today;
        DocNo = "";
        FileNo = "";
        ErrorMessage = string.Empty;
        Params = string.Empty;
        valueSt = Global.MillionFormat(value);
        }

        #region Properties
        public int  LCID {get; set;}
        public int  InvoiceID  {get; set;}
        public int  InvoiceType  {get; set;}
        public int  ProductID  {get; set;}
        public string  LCNo {get; set;}
        public string  InvoiceNo {get; set;}
        public string ProductName { get; set; }
        public string MUnit { get; set; }
        public string  ProductCode {get; set;}
        public int     ContractorID {get; set;}
        public string  ContractorName {get; set;}
        public double  Qty {get; set;}
        public double  UnitPrice {get; set;}
        public double  Qty_Invoice {get; set;}
        public double  Qty_PI {get; set;}
        public int     ImportPIID {get; set;}
        public int     ImportPIDetailID {get; set;}
        public string  PINo {get; set;}
        public string  LotNo {get; set;}
        public string  WUName {get; set;}
        public int     ParentType {get; set;}
        public int     ParentID {get; set;}
        public int     WorkingUnitID {get; set;}
        public double  Qty_TR {get; set;}
        public int     GRNID {get; set;}
        public int     GRNDetailID {get; set;}
        public string  GRNNo {get; set;}
        public string  AgentName {get; set;}
        public double  Qty_StockIn {get; set;}
        public double  Qty_Short {get; set;}
        public int  ImportInvoiceDetailID {get; set;}
        public string  BLNo {get; set;}
        public DateTime BLDate {get; set;}
        public EnumInvoiceEvent InvoiceStatus { get; set; }
        public DateTime ImportLCDate { get; set; }
        public DateTime DateofInvoice { get; set; }
        public DateTime ExpireDate { get; set; }
        public DateTime ShipmentDate { get; set; }
        public DateTime CnFSendDate { get; set; }
        public string FileNo { get; set; }
        public string DocNo { get; set; }
        public string ErrorMessage { get; set; }
        public string Params { get; set; }
        #endregion
        #region derivedProperties
        public string BLDateStr { get { return (this.BLDate == DateTime.MinValue)? "-": this.BLDate.ToString("dd MMM yyyy"); } }
        public string ImportLCDateStr { get { return (this.ImportLCDate == DateTime.MinValue) ? "-" : this.ImportLCDate.ToString("dd MMM yyyy"); } }
        public string ShipmentDateStr { get { return (this.ShipmentDate == DateTime.MinValue) ? "-" : this.ShipmentDate.ToString("dd MMM yyyy"); } }
        public string ExpireDateStr { get { return (this.ExpireDate == DateTime.MinValue) ? "-" : this.ExpireDate.ToString("dd MMM yyyy"); } }
        public string CnFSendDateStr { get { return (this.CnFSendDate == DateTime.MinValue) ? "-" : this.CnFSendDate.ToString("dd MMM yyyy"); } }
        public string DateofInvoiceStr { get { return (this.DateofInvoice == DateTime.MinValue) ? "-" : this.DateofInvoice.ToString("dd MMM yyyy"); } }

        public double value { get { return this.Qty * this.UnitPrice; } }
        public string valueSt { get ; set; }
        public double Amount_PI { get { return this.Qty_PI * this.UnitPrice; } }
        public string InvoiceStatusSt { get { return EnumObject.jGet(this.InvoiceStatus); } }

        #endregion
        #region Functions
        public static List<ImportOutStandingDetail> Gets(int nBUID, int nLCPaymentType, int nBankBranchID, int nCurrencyID, DateTime stratDate, DateTime endDate, int nSPRptType,int nDate, long nUserID)
        {
            return ImportOutStandingDetail.Service.Gets(nBUID, nLCPaymentType, nBankBranchID, nCurrencyID, stratDate, endDate, nSPRptType,nDate, nUserID);
        }

        #endregion

        #region ServiceFactory
        internal static IImportOutStandingDetailService Service
        {
            get { return (IImportOutStandingDetailService)Services.Factory.CreateService(typeof(IImportOutStandingDetailService)); }
        }
        #endregion
    }

  
    #region IImportOutStandingDetail interface

    public interface IImportOutStandingDetailService
    {
        List<ImportOutStandingDetail> Gets(int nBUID, int nLCPaymentType, int nBankBranchID, int nCurrencyID, DateTime stratDate, DateTime endDate, int nSPRptType,int nDate, long nUserID);
     
    }
    #endregion

    
}
