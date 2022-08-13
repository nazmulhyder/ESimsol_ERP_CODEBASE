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
    #region ExportLCUPUD
    public class ExportLCUPUD : BusinessObject
    {
        public ExportLCUPUD()
        {
            ExportPIID = 0;
            ExportLCID = 0;
            FileNo = "";
            PINo = "";
            PIDate = DateTime.Now;
            LCNo = "";
            IssueDate = DateTime.Now;
            ContractorID = 0;
            MKTEmpID = 0;
            VersionNo = 0;
            LCOpenDate = DateTime.Now;
            AmendmentDate = DateTime.Now;
            LCReceiveDate = DateTime.Now;
            ApproveDate = DateTime.Now;
            LCStatus = 0;
            BankBranchID_Advice = 0;
            BankBranchID_Issue = 0;
            BankBranchID_Negotiation = 0;
            Amount = 0;
            Qty = 0;
            Qty_DC = 0;
            Amount_DC = 0;
            Amount_Bill = 0;
            Qty_Bill = 0;
            Amount_DO = 0;
            Qty_DO = 0;
            BUID = 0;
            ContractorName = "";
            LastDeliveryDate = DateTime.Now;
            DOValue = 0;
            DOQty = 0;
            MKTPersonName = "";
            Currency = "";
            CurrencyID = 0;
            BuyerID = 0;
            BuyerName = "";
            BankName_Nego = "";
            BankName_Issue = "";
            InvoiceNo = "";
            UDID = 0;
            UDNo = "";
            UDRecdDate = DateTime.Now;
            UPNo = "";
            ExportUPDate = DateTime.Now;
            MUnitID = 0;
            MUSymbol = "";
            BUName = "";
            ShipmentDate = DateTime.Now;
            ExpiryDate = DateTime.Now;
            ExportUPDetails = new List<ExportUPDetail>();
            BBranchName_Issue = "";
            ADate = DateTime.MinValue;
            AUDNo = "";
            ErrorMessage = "";
        }

        #region Property
        public int ExportPIID { get; set; }
        public int ExportLCID { get; set; }
        public string FileNo { get; set; }
        public string PINo { get; set; }
        public DateTime PIDate { get; set; }
        public string LCNo { get; set; }
        public DateTime IssueDate { get; set; }
        public int ContractorID { get; set; }
        public int MKTEmpID { get; set; }
        public int VersionNo { get; set; }
        public DateTime LCOpenDate { get; set; }
        public DateTime AmendmentDate { get; set; }
        public DateTime LCReceiveDate { get; set; }
        public DateTime ApproveDate { get; set; }
        public int LCStatus { get; set; }
        public int BankBranchID_Advice { get; set; }
        public int BankBranchID_Issue { get; set; }
        public int BankBranchID_Negotiation { get; set; }
        public double Amount { get; set; }
        public double Qty { get; set; }
        public double Qty_DC { get; set; }
        public double Amount_DC { get; set; }
        public double Amount_Bill { get; set; }
        public double Qty_Bill { get; set; }
        public double Amount_DO { get; set; }
        public double Qty_DO { get; set; }
        public int BUID { get; set; }
        public string ContractorName { get; set; }
        public DateTime LastDeliveryDate { get; set; }
        public double DOValue { get; set; }
        public double DOQty { get; set; }
        public string MKTPersonName { get; set; }
        public string Currency { get; set; }
        public int CurrencyID { get; set; }
        public int BuyerID { get; set; }
        public string BuyerName { get; set; }
        public string BankName_Nego { get; set; }
        public string BankName_Issue { get; set; }
        public string InvoiceNo { get; set; }
        public int UDID { get; set; }
        public string UDNo { get; set; }
        public DateTime UDRecdDate { get; set; }
        public string UPNo { get; set; }
        public DateTime ExportUPDate { get; set; }
        public string BillNo { get; set; }
        public int MUnitID { get; set; }
        public string MUSymbol { get; set; }
        public string BUName { get; set; }
        public DateTime ShipmentDate { get; set; }
        public DateTime ExpiryDate { get; set; }
        public string BBranchName_Issue { get; set; }

        public string ErrorMessage { get; set; }
        public int ReportLayout { get; set; }
        public int Status { get; set; }
        public string AUDNo { get; set; }
        public DateTime ADate { get; set; }
        #endregion

        #region Derived Property

        public List<ExportUPDetail> ExportUPDetails { get; set; }
        public string PIDateInString
        {
            get
            {
                if (PIDate == DateTime.MinValue) return "";
                return PIDate.ToString("dd MMM yyyy");
            }
        }
        public string ADateInString
        {
            get
            {
                if (ADate == DateTime.MinValue) return "";
                return ADate.ToString("dd MMM yyyy");
            }
        }
        public string IssueDateInString
        {
            get
            {
                if (IssueDate == DateTime.MinValue) return "";
                return IssueDate.ToString("dd MMM yyyy");
            }
        }
        public string LCOpenDateSt
        {
            get
            {
                if (LCOpenDate == DateTime.MinValue) return "";
                return LCOpenDate.ToString("dd MMM yyyy");
            }
        }
        public string AmendmentDateSt
        {
            get
            {
                if (AmendmentDate == DateTime.MinValue) return "";
                return AmendmentDate.ToString("dd MMM yyyy");
            }
        }
        public string LCReceiveDateInString
        {
            get
            {
                if (LCReceiveDate == DateTime.MinValue) return "";
                return LCReceiveDate.ToString("dd MMM yyyy");
            }
        }
        public string ApproveDateInString
        {
            get
            {
                if (ApproveDate == DateTime.MinValue) return "";
                return ApproveDate.ToString("dd MMM yyyy");
            }
        }
        public string LastDeliveryDateInString
        {
            get
            {
                if (LastDeliveryDate == DateTime.MinValue) return "";
                return LastDeliveryDate.ToString("dd MMM yyyy");
            }
        }
        public string UDRecdDateInString
        {
            get
            {
                if (UDRecdDate == DateTime.MinValue) return "";
                return UDRecdDate.ToString("dd MMM yyyy");
            }
        }
        public string ExportUPDateInString
        {
            get
            {
                if (ExportUPDate == DateTime.MinValue) return "";
                return ExportUPDate.ToString("dd MMM yyyy");
            }
        }
        public string ShipmentDateInString
        {
            get
            {
                if (ShipmentDate == DateTime.MinValue) return "";
                return ShipmentDate.ToString("dd MMM yyyy");
            }
        }
        public string ExpiryDateInString
        {
            get
            {
                if (ExpiryDate == DateTime.MinValue) return "";
                return ExpiryDate.ToString("dd MMM yyyy");
            }
        }

        public int UDDelayDaysFromShipmjent
        {
            get
            {
                if (UDRecdDate == DateTime.MinValue) return (int)(DateTime.Now - ShipmentDate).TotalDays;
                return (int)(UDRecdDate - ShipmentDate).TotalDays;
            }
        }
        public int UDDelayDaysFromExpiry
        {
            get
            {
                if (UDRecdDate == DateTime.MinValue) return (int)(DateTime.Now - ExpiryDate).TotalDays;
                return (int)(UDRecdDate - ExpiryDate).TotalDays;
            }
        }

        public int UPDelayDaysFromShipmjent
        {
            get
            {
                if (ExportUPDate == DateTime.MinValue) return (int)(DateTime.Now - ShipmentDate).TotalDays;
                return (int)(ExportUPDate - ShipmentDate).TotalDays;
            }
        }
        public int UPDelayDaysFromExpiry
        {
            get
            {
                if (ExportUPDate == DateTime.MinValue) return (int)(DateTime.Now - ExpiryDate).TotalDays;
                return (int)(ExportUPDate - ExpiryDate).TotalDays;
            }
        }
        
        #endregion

        #region Functions
        
        public static List<ExportLCUPUD> Gets(string sSQL, long nUserID)
        {
            return ExportLCUPUD.Service.Gets(sSQL, nUserID);
        }
        #endregion

        #region ServiceFactory
        internal static IExportLCUPUDService Service
        {
            get { return (IExportLCUPUDService)Services.Factory.CreateService(typeof(IExportLCUPUDService)); }
        }
        #endregion

        public List<ExportLCUPUD> ExportLCUPUDs { get; set; }
    }
    #endregion

    #region IExportLCUPUD interface
    public interface IExportLCUPUDService
    {
        List<ExportLCUPUD> Gets(string sSQL, Int64 nUserID);
    }
    #endregion
}
