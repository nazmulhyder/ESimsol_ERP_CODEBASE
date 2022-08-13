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
    #region ImportInvoiceRegister
    public class ImportInvoiceRegister : BusinessObject
    {
        public ImportInvoiceRegister()
        {
            ImportInvoiceDetailID = 0;
            ImportInvoiceID = 0;
            ProductID = 0;
            MUnitID = 0;
            UnitPrice = 0;
            Qty = 0;
            Amount = 0;
            ImportInvoiceNo = "";
            BUID = 0;
            ABPNo = "";
            DateofInvoice = DateTime.MinValue;
            InvoiceStatus = EnumInvoiceEvent.None;
            PaymentInstructionType = EnumPaymentInstruction.None;
            ContractorID = 0;
            ContactPersonID = 0;
            ConcernPersonID = 0;
            CurrencyID = 0;
            InvoiceAmount = 0;
            AdviseBBID = 0;
            LCTermID = 0;
            InvoiceType = EnumImportPIType.None;
            AcceptedBy = 0;
            DateofAcceptance = DateTime.MinValue;
            VersionNumber = 0;
            DateofApplication = DateTime.MinValue;
            DeliveryClause = "";
            PaymentClause = "";
            ShipmentBy = EnumShipmentBy.None;
            Remarks = "";
            ApprovedByName = "";
            ProductCode = "";
            ProductName = "";
            MUName = "";
            MUSymbol = "";
            SupplierName = "";
            FileNo = "";
            BLNo = "";
            BLDate = DateTime.MinValue;
            CurrencyName = "";
            CurrencySymbol = "";
            CRate_Acceptance = 0;
            LCTermName = "";
            ImportLCNo = "";
            ImportLCDate = DateTime.MinValue;
            ErrorMessage = "";
            SearchingData = "";
            RateUnit = 1;
            ReportLayout = EnumReportLayout.None;
            BankName_Nego="";
            BBRanchName_Nego = "";
            MasterLCNos = "";
            BillofEntryDate = DateTime.Now;
            BillofEntryNo = "";
        }

        #region Properties
        public int ImportInvoiceDetailID { get; set; }
        public int ImportInvoiceID { get; set; }
        public int ProductID { get; set; }
        public int MUnitID { get; set; }
        public double UnitPrice { get; set; }
        public double Qty { get; set; }
        public double Amount { get; set; }
        public string ImportInvoiceNo { get; set; }
        public int BUID { get; set; }
        public string ABPNo { get; set; }
        public DateTime DateofInvoice { get; set; }
        public EnumInvoiceEvent InvoiceStatus { get; set; }
        public EnumPaymentInstruction PaymentInstructionType { get; set; }
        public int ContractorID { get; set; }
        public int ContactPersonID { get; set; }
        public int ConcernPersonID { get; set; }
        public int CurrencyID { get; set; }
        public double InvoiceAmount { get; set; }
        public int AdviseBBID { get; set; }
        public int LCTermID { get; set; }
        public EnumImportPIType InvoiceType { get; set; }
        public int AcceptedBy { get; set; }
        public DateTime DateofAcceptance { get; set; }
        public int VersionNumber { get; set; }
        public DateTime DateofApplication { get; set; }
        public DateTime DateofMaturity { get; set; }
        public DateTime DateofNegotiation { get; set; }
        public DateTime DateofPayment { get; set; }
        public DateTime DateofReceive { get; set; }
        public DateTime DateofBankInfo { get; set; }
        public DateTime DateOfTakeOutDoc { get; set; } 
        public string DeliveryClause { get; set; }
        public string PaymentClause { get; set; }
        public EnumShipmentBy ShipmentBy { get; set; }
        public string Remarks { get; set; }
        public string ApprovedByName { get; set; }
        public string ProductCode { get; set; }
        public string ProductName { get; set; }
        public string MUName { get; set; }
        public string MUSymbol { get; set; }
        public string SupplierName { get; set; }
        public string FileNo { get; set; }
        public string BLNo { get; set; }
        public DateTime BLDate { get; set; }
        public string CurrencyName { get; set; }
        public string CurrencySymbol { get; set; }
        public double CRate_Acceptance { get; set; }
        public string LCTermName { get; set; }
        public string ImportLCNo { get; set; }
        public DateTime ImportLCDate { get; set; }
        public string ErrorMessage { get; set; }
        public string SearchingData { get; set; }
        public int RateUnit { get; set; }
        public string BankName_Nego { get; set; }
        public string BBRanchName_Nego { get; set; }
        public DateTime BillofEntryDate { get; set; }
        public string BillofEntryNo { get; set; }
        public EnumReportLayout ReportLayout { get; set; }
        public EnumProductNature ProductType { get; set; }
        #endregion

        #region Derived Property
        public string MasterLCNos { get; set; }
        public string ProductTypeSt
        {
            get
            {
                return EnumObject.jGet(this.ProductType);
            }
        }


        public string BankWithBranch 
        {
            get { return this.BankName_Nego + " [" + this.BBRanchName_Nego + "]"; }
        }
        public string UnitPriceSt
        {
            get
            {
                if (this.RateUnit <= 1)
                {
                    return Global.MillionFormat(this.UnitPrice);
                }
                else
                {
                    return Global.MillionFormat(this.UnitPrice) + "/" + this.RateUnit.ToString();
                }
            }
        }
        public string BLDateSt
        {
            get
            {
                if (this.BLDate == DateTime.MinValue)
                {
                    return "--";
                }
                else
                {
                    return this.BLDate.ToString("dd MMM yyyy");
                }
            }
        }
        public string DateofInvoiceSt
        {
            get 
            {
                if (this.DateofInvoice == DateTime.MinValue)
                {
                    return "--";
                }
                else
                {
                    return this.DateofInvoice.ToString("dd MMM yyyy");
                }
            }
        }
        public string DateofAcceptanceSt
        {
            get
            {
                if (this.DateofAcceptance == DateTime.MinValue)
                {
                    return "--";
                }
                else
                {
                    return this.DateofAcceptance.ToString("dd MMM yyyy");
                }
            }
        }
        public string DateofApplicationSt
        {
            get
            {
                if (this.DateofApplication == DateTime.MinValue)
                {
                    return "--";
                }
                else
                {
                    return this.DateofApplication.ToString("dd MMM yyyy");
                }
            }
        }
        public string DateofReceiveInString 
        {
            get
            {
                if (this.DateofReceive == DateTime.MinValue)
                {
                    return "--";
                }
                else
                {
                    return this.DateofReceive.ToString("dd MMM yyyy");
                }
            }
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
        public string DateofBankInfoInString
        {
            get
            {
                if (this.DateofBankInfo == DateTime.MinValue)
                {
                    return "--";
                }
                else
                {
                    return this.DateofBankInfo.ToString("dd MMM yyyy");
                }
            }
        }
        public string DateOfTakeOutDocInString
        {
            get
            {
                if (this.DateOfTakeOutDoc == DateTime.MinValue)
                {
                    return "--";
                }
                else
                {
                    return this.DateOfTakeOutDoc.ToString("dd MMM yyyy");
                }
            }
        }
        public string DateofMaturityInSt
        {
            get
            {
                if (this.DateofMaturity == DateTime.MinValue)
                {
                    return "--";
                }
                else
                {
                    return this.DateofMaturity.ToString("dd MMM yyyy");
                }
            }
        }
        public string BillOfEntryDateSt
        {
            get
            {
                if (this.BillofEntryDate == DateTime.MinValue)
                {
                    return "--";
                }
                else
                {
                    return this.BillofEntryDate.ToString("dd MMM yyyy");
                }
            }
        }
        
        public string ImportInvoiceStatusSt
        {
            get
            {
                return EnumObject.jGet(this.InvoiceStatus);
            }
        }
        public string ImportInvoiceTypeSt
        {
            get
            {
                return EnumObject.jGet(this.InvoiceType);
            }
        }
        public string PaymentInstructionTypeSt
        {
            get
            {
                return EnumObject.jGet(this.PaymentInstructionType);
            }
        }
        #endregion

        #region Functions
        public static List<ImportInvoiceRegister> Gets(string sSQL, long nUserID)
        {
            return ImportInvoiceRegister.Service.Gets(sSQL, nUserID);
        }
        #endregion

        #region ServiceFactory
        internal static IImportInvoiceRegisterService Service
        {
            get { return (IImportInvoiceRegisterService)Services.Factory.CreateService(typeof(IImportInvoiceRegisterService)); }
        }
        #endregion
    }
    #endregion

    #region IImportInvoiceRegister interface

    public interface IImportInvoiceRegisterService
    {
        List<ImportInvoiceRegister> Gets(string sSQL, Int64 nUserID);
    }
    #endregion
}
