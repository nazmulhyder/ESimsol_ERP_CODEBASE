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
    #region ImportLCRegister
    public class ImportLCRegister : BusinessObject
    {
        public ImportLCRegister()
        {
            ImportLCDetailID = 0;
            ImportLCID = 0;
            ProductID = 0;
            MUnitID = 0;
            UnitPrice = 0;
            Qty = 0;
            Amount = 0;
            AmountWithRate = 0;
            CCRate = 0;
            ImportLCNo = "";
            BUID = 0;
            SLNo = "";
            ImportLCDate = DateTime.MinValue;
            LCCurrentStatus = EnumLCCurrentStatus.None;
            PaymentInstructionType = EnumPaymentInstruction.None;
            SupplierID = 0;
            ContactPersonID = 0;
            ConcernPersonID = 0;
            CurrencyID = 0;
            TotalValue = 0;
            AdviseBBID = 0;
            LCTermID = 0;
            ReceivedBy = 0;
            ShipmentDate = DateTime.MinValue;
            VersionNo = 0;
            ExpireDate = DateTime.MinValue;
            DeliveryClause = "";
            PaymentClause = "";
            ShipmentBy = EnumShipmentBy.None;
            ReceiveDate = DateTime.Now;
            ApprovedByName = "";
            ProductCode = "";
            ProductName = "";
            MUName = "";
            MUSymbol = "";
            SupplierName = "";
            SCPName = "";
            CPName = "";
            CurrencyName = "";
            CurrencySymbol = "";
            BankName = "";
            BranchName = "";
            LCTermName = "";
            ErrorMessage = "";
            SearchingData = "";
            InvoiceQty = 0;     
            ImportPIID = 0;
		    ImportPINo = "";
		    PIDate = DateTime.Now;
            LCAppType = EnumLCAppType.None;
            ImportPIType = EnumImportPIType.None;
            PIValue = 0;
            InvoiceValue = 0;
            ReportLayout = EnumReportLayout.None;
            ExportLCNo = "";
            BankShortName = "";
            FileNo = "";
            AmmendmentAmount = 0;
            AmendmentDate = DateTime.Now;
            LCType = 0;
            ImportProductName = "";
        }

        #region Properties
        public int ImportLCDetailID { get; set; }
        public int ImportLCID { get; set; }
        public int ProductID { get; set; }
        public int MUnitID { get; set; }
        public double UnitPrice { get; set; }
        public double Qty { get; set; }
        public double Amount { get; set; }
        public double AmountWithRate { get; set; }
        public double CCRate { get; set; }
        public int BUID { get; set; }
        public string SLNo { get; set; }
        public DateTime ImportLCDate { get; set; }
        public EnumLCCurrentStatus LCCurrentStatus { get; set; }
        public EnumPaymentInstruction PaymentInstructionType { get; set; }
        public EnumLCAppType LCAppType { get; set; }
        public EnumImportPIType ImportPIType { get; set; }
        public int SupplierID { get; set; }
        public int ContactPersonID { get; set; }
        public int ConcernPersonID { get; set; }
        public int CurrencyID { get; set; }
        public double TotalValue { get; set; }
        public int AdviseBBID { get; set; }
        public int LCTermID { get; set; }
        public int ReceivedBy { get; set; }
        public DateTime ShipmentDate { get; set; }
        public int VersionNo { get; set; }
        public DateTime ExpireDate { get; set; }
        public DateTime ReceiveDate { get; set; }
        public string DeliveryClause { get; set; }
        public string PaymentClause { get; set; }
        public EnumShipmentBy ShipmentBy { get; set; }
        public string ApprovedByName { get; set; }
        public string ProductCode { get; set; }
        public string ProductName { get; set; }
        public string MUName { get; set; }
        public string MUSymbol { get; set; }
        public string SupplierName { get; set; }
        public string SCPName { get; set; }
        public string CPName { get; set; }
        public string FileNo { get; set; }
        public string CurrencyName { get; set; }
        public string CurrencySymbol { get; set; }
        public string BankName { get; set; }
        public string BankShortName { get; set; }
        public string BranchName { get; set; }
        public string LCTermName { get; set; }
        public string ImportLCNo { get; set; }
        public string ErrorMessage { get; set; }
        public string SearchingData { get; set; }
        public double InvoiceQty { get; set; }
        public double InvoiceValue{ get; set; }
        public string ExportLCNo { get; set; }
        public int ImportPIID { get; set; }
        public string ImportPINo { get; set; }
        public DateTime PIDate { get; set; }
        public double PIValue { get; set; }
        public double AmmendmentAmount { get; set; }
        public DateTime AmendmentDate { get; set; }
        public EnumProductNature ProductType { get; set; }
        public EnumReportLayout ReportLayout { get; set; }
        public string ImportProductName { get; set; }
        public int LCType { get; set; }
        #endregion

        #region Derived Property
        public double YetToInvoiceQty { get { return (this.Qty - this.InvoiceQty); } }
        public double InvoiceAmount { get { return (this.InvoiceQty * this.UnitPrice); } }
        public double YetToInvoiceAmount { get { return (this.YetToInvoiceQty * this.UnitPrice); } }

        public string ProductTypeSt
        {
            get
            {
                return EnumObject.jGet(this.ProductType);
            }
        }
        public double LCAmount
        {
            get
            {
                return this.UnitPrice * this.Qty;
            }
            set { }
        }
        public string ImportLCDateSt
        {
            get 
            {
                if (this.ImportLCDate == DateTime.MinValue)
                {
                    return "--";
                }
                else
                {
                    return this.ImportLCDate.ToString("dd MMM yyyy");
                }
            }
        }
        public string ReceiveDateInString
        {
            get
            {
                return this.ReceiveDate.ToString("dd MMM yyyy");
            }
        }
        public string ShipmentDatet
        {
            get
            {
                if (this.ShipmentDate == DateTime.MinValue)
                {
                    return "--";
                }
                else
                {
                    return this.ShipmentDate.ToString("dd MMM yyyy");
                }
            }
        }
        public string ExpireDateSt
        {
            get
            {
                if (this.ExpireDate == DateTime.MinValue)
                {
                    return "--";
                }
                else
                {
                    return this.ExpireDate.ToString("dd MMM yyyy");
                }
            }
        }

        public string PIDateSt
        {
            get
            {
                if (this.PIDate == DateTime.MinValue)
                {
                    return "--";
                }
                else
                {
                    return this.PIDate.ToString("dd MMM yyyy");
                }
            }
        }

        public string ImportLCStatusSt
        {
            get
            {
                return EnumObject.jGet(this.LCCurrentStatus);
            }
        }
       
        public string PaymentInstructionTypeSt
        {
            get
            {
                return EnumObject.jGet(this.PaymentInstructionType);
            }
        }
        public string LCAppTypeST
        {
            get
            {
                return EnumObject.jGet(this.LCAppType);
            }
        }
        public string ImportPITypeST
        {
            get
            {
                return EnumObject.jGet(this.ImportPIType);
            }
        }
        #endregion

        #region Functions
        public static List<ImportLCRegister> Gets(string sSQL, long nUserID)
        {
            return ImportLCRegister.Service.Gets(sSQL, nUserID);
        }
        #endregion

        #region ServiceFactory
        internal static IImportLCRegisterService Service
        {
            get { return (IImportLCRegisterService)Services.Factory.CreateService(typeof(IImportLCRegisterService)); }
        }
        #endregion
    }
    #endregion

    #region IImportLCRegister interface

    public interface IImportLCRegisterService
    {
        List<ImportLCRegister> Gets(string sSQL, Int64 nUserID);
    }
    #endregion
}
