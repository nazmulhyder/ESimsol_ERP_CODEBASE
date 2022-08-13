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
    #region ImportPIRegister
    public class ImportPIRegister : BusinessObject
    {
        public ImportPIRegister()
        {
            ImportPIDetailID = 0;
            ImportPIID = 0;
            ProductID = 0;
            MUnitID = 0;
            UnitPrice = 0;
            Qty = 0;
            Amount = 0;
            ImportPINo = "";
            BUID = 0;
            SLNo = "";
            IssueDate = DateTime.MinValue;
            ImportPIStatus = EnumImportPIState.Initialized;
            PaymentInstructionType = EnumPaymentInstruction.None;
            SupplierID = 0;
            ContactPersonID = 0;
            ConcernPersonID = 0;
            CurrencyID = 0;
            TotalValue = 0;
            AdviseBBID = 0;
            LCTermID = 0;
            ImportPIType = EnumImportPIType.None;
            ApprovedBy = 0;
            DateOfApproved = DateTime.MinValue;
            VersionNumber = 0;
            ValidityDate = DateTime.MinValue;
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
            SCPName = "";
            CPName = "";
            CurrencyName = "";
            CurrencySymbol = "";
            BankName = "";
            BranchName = "";
            LCTermName = "";
            ImportLCNo = "";
            ErrorMessage = "";
            SearchingData = "";
            RateUnit = 1;
            ReportLayout = EnumReportLayout.None;
            //Pi register two
            ImportPIDate = DateTime.Now;
            PIQty = 0;
            PIAmount=0;
            SalesContractNo="";
            SalesContractDate=DateTime.Now;
            LCID=0;
            LCNo="";
            LCDate=DateTime.Now;
            BankBranchID=0;
            LCAmount=0;
            InvoiceQty=0;
            InvoiceValue=0;
            ContractorName="";
            ContractorID=0;
            GRNQty = 0;
        }

        #region Properties
        public int ImportPIDetailID { get; set; }
        public int ImportPIID { get; set; }
        public int ProductID { get; set; }
        public int MUnitID { get; set; }
        public string MUnitName { get; set; }
        public double UnitPrice { get; set; }
        public double Qty { get; set; }
        public double Amount { get; set; }
        public string ImportPINo { get; set; }
        public int BUID { get; set; }
        public string SLNo { get; set; }
        public DateTime IssueDate { get; set; }
        public EnumImportPIState ImportPIStatus { get; set; }
        public EnumPaymentInstruction PaymentInstructionType { get; set; }
        public int SupplierID { get; set; }
        public int ContactPersonID { get; set; }
        public int ConcernPersonID { get; set; }
        public int CurrencyID { get; set; }
        public double TotalValue { get; set; }
        public int AdviseBBID { get; set; }
        public int LCTermID { get; set; }
        public EnumImportPIType ImportPIType { get; set; }
        public EnumProductNature ProductType { get; set; }
        public int ApprovedBy { get; set; }
        public DateTime DateOfApproved { get; set; }
        public int VersionNumber { get; set; }
        public DateTime ValidityDate { get; set; }
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
        public string SCPName { get; set; }
        public string CPName { get; set; }
        public string CurrencyName { get; set; }
        public string CurrencySymbol { get; set; }
        public string BankName { get; set; }
        public string BranchName { get; set; }
        public string LCTermName { get; set; }
        public string ImportLCNo { get; set; }
        public string ErrorMessage { get; set; }
        public string SearchingData { get; set; }
        public int RateUnit { get; set; }
        public EnumReportLayout ReportLayout { get; set; }
        #endregion

        #region Derived Property
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
        public string IssueDateSt
        {
            get 
            {
                if (this.IssueDate == DateTime.MinValue)
                {
                    return "--";
                }
                else
                {
                    return this.IssueDate.ToString("dd MMM yyyy");
                }
            }
        }
        public string DateOfApprovedSt
        {
            get
            {
                if (this.DateOfApproved == DateTime.MinValue)
                {
                    return "--";
                }
                else
                {
                    return this.DateOfApproved.ToString("dd MMM yyyy");
                }
            }
        }
        public string ValidityDateSt
        {
            get
            {
                if (this.ValidityDate == DateTime.MinValue)
                {
                    return "--";
                }
                else
                {
                    return this.ValidityDate.ToString("dd MMM yyyy");
                }
            }
        }
        public string ImportPIStatusSt
        {
            get
            {
                return EnumObject.jGet(this.ImportPIStatus);
            }
        }
        public string ImportPITypeSt
        {
            get
            {
                return EnumObject.jGet(this.ImportPIType);
            }
        }
        public string ProductTypeSt
        {
            get
            {
                return EnumObject.jGet(this.ProductType);
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

        #region Property Import PI register Two
        public DateTime ImportPIDate { get; set; }
        public double PIQty { get; set; }
        public double PIAmount { get; set; }
        public string SalesContractNo { get; set; }
        public DateTime SalesContractDate { get; set; }
        public int LCID { get; set; }
        public string LCNo { get; set; }
        public DateTime LCDate { get; set; }
        public int BankBranchID { get; set; }
        public double LCAmount { get; set; }
        public double InvoiceQty { get; set; }
        public double InvoiceValue { get; set; }
        public string ContractorName { get; set; }
        public int ContractorID { get; set; }
        public double GRNQty { get; set; }
        public string ImportPIDateSt
        {
            get
            {
                if (this.ImportPIDate == DateTime.MinValue)
                {
                    return "--";
                }
                else
                {
                    return this.ImportPIDate.ToString("dd MMM yyyy");
                }
            }
        }
        public string SalesContractDateSt
        {
            get
            {
                if (this.SalesContractDate == DateTime.MinValue)
                {
                    return "--";
                }
                else
                {
                    return this.SalesContractDate.ToString("dd MMM yyyy");
                }
            }
        }
        public string LCDateSt
        {
            get
            {
                if (this.LCDate == DateTime.MinValue)
                {
                    return "--";
                }
                else
                {
                    return this.LCDate.ToString("dd MMM yyyy");
                }
            }
        }
        #endregion

        #region Functions
        public static List<ImportPIRegister> Gets(string sSQL, long nUserID)
        {
            return ImportPIRegister.Service.Gets(sSQL, nUserID);
        }
        public static List<ImportPIRegister> GetsForPIRegTwo(string sSQL, long nUserID)
        {
            return ImportPIRegister.Service.GetsForPIRegTwo(sSQL, nUserID);
        }
        #endregion

        #region ServiceFactory
        internal static IImportPIRegisterService Service
        {
            get { return (IImportPIRegisterService)Services.Factory.CreateService(typeof(IImportPIRegisterService)); }
        }
        #endregion
    }
    #endregion

    #region IImportPIRegister interface

    public interface IImportPIRegisterService
    {
        List<ImportPIRegister> Gets(string sSQL, Int64 nUserID);
        List<ImportPIRegister> GetsForPIRegTwo(string sSQL, Int64 nUserID);
    }
    #endregion
}
