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
    #region ImportRegister

    public class ImportRegister : BusinessObject
    {
        public ImportRegister()
        {
            ImportPIDetailID = 0;
            ImportPIID = 0;
            ProductID = 0;
            ProductCode = "";
            ProductName = "";
            MUnitID = 0;
            UnitPrice = 0.0;
            PIQty = 0.0;
            PIAmount = 0.0;
            ImportPINo = "";
            BUID = 0;
            ImportPIDate = DateTime.MinValue;
            InvoiceType = EnumImportPIType.None;
            ImportPIStatus = EnumImportPIState.Initialized;
            PartyName = "";
            LCAmount = 0.0;
            IssueBankName = "";
            IssueBankSName = "";
            NegoBankName = "";
            NegoBankSName = "";
            ImportInvoiceNo = "";
            ImportInvoiceDate = DateTime.MinValue;
            InvoiceValue = 0.0;
            InvoiceDueValue = 0.0;
            BillofLoadingNo = "";
            BillofLoadingDate = DateTime.MinValue;
            BillOfEntrtNo = "";
            BillofEntrtDate = DateTime.MinValue;
            GoodsRcvQty = 0.0;
            GoodsRcvDate = DateTime.MinValue;
            AcceptanceDate = DateTime.MinValue;
            MaturityDate = DateTime.MinValue;
            PaymentDate = DateTime.MinValue;
            Params = "";
            ErrorMessage = "";
        }

        #region Properties
        public int ImportPIDetailID { get; set; }
        public int ImportPIID { get; set; }
        public int ProductID { get; set; }
        public string ProductCode { get; set; }
        public string ProductName { get; set; }
        public int MUnitID { get; set; }
        public double UnitPrice { get; set; }
        public double PIQty { get; set; }
        public double PIAmount { get; set; }
        public string ImportPINo { get; set; }
        public int BUID { get; set; }
        public DateTime ImportPIDate { get; set; }
        public EnumImportPIState ImportPIStatus { get; set; }
        public int ImportPIStatusInt { get; set; }
        public int SupplierID { get; set; }
        public string SupplierName { get; set; }
        public EnumProductNature ProductType { get; set; }
        public string PartyName { get; set; }
        public int ImportLCID { get; set; }
        public string ImportLCNo { get; set; }
        public DateTime ImportLCDate { get; set; }
        public string SalesContractNo { get; set; }
        public DateTime SalesContractDate { get; set; }
        public int BankAccountID { get; set; }
        public int BankBranchID_Nego { get; set; }
        public double LCAmount { get; set; }
        public string IssueBankName { get; set; }
        public string IssueBankSName { get; set; }
        public string NegoBankName { get; set; }
        public string NegoBankSName { get; set; }
        public string ImportInvoiceNo { get; set; }
        public DateTime ImportInvoiceDate { get; set; }
        public double InvoiceValue { get; set; }
        public double InvoiceDueValue { get; set; }
        public string BillofLoadingNo { get; set; }
        public DateTime BillofLoadingDate { get; set; }
        public string BillOfEntrtNo { get; set; }
        public DateTime BillofEntrtDate { get; set; }
        public double GoodsRcvQty { get; set; }
        public DateTime GoodsRcvDate { get; set; }
        public DateTime AcceptanceDate { get; set; }
        public double MaturityValue { get; set; }
        public DateTime MaturityDate { get; set; }
        public DateTime PaymentDate { get; set; }
        public EnumImportPIType InvoiceType { get; set; }
        public string Params { get; set; }
        public string ErrorMessage { get; set; }
        #endregion

        #region Derived Property
        public string ImportPIStatusSt
        {
            get
            {
                return ImportPIStatus.ToString();
            }
        }
        public string ImportPIDateSt
        {
            get
            {
                return ImportPIDate.ToString("dd MMM yyyy");
            }
        }
        public string ImportLCDateSt
        {
            get
            {
                return ImportLCDate.ToString("dd MMM yyyy");
            }
        }
        public string ImportInvoiceDateSt
        {
            get
            {
                return ImportInvoiceDate.ToString("dd MMM yyyy");
            }
        }
        public string BillofLoadingDateSt
        {
            get
            {
                return BillofLoadingDate.ToString("dd MMM yyyy");
            }
        }
        public string BillofEntrtDateSt
        {
            get
            {
                return BillofEntrtDate.ToString("dd MMM yyyy");
            }
        }
        public string GoodsRcvDateSt
        {
            get
            {
                return GoodsRcvDate.ToString("dd MMM yyyy");
            }
        }
        public string PaymentDateSt
        {
            get
            {
                return PaymentDate.ToString("dd MMM yyyy");
            }
        }
        public string MaturityDateSt
        {
            get
            {
                return MaturityDate.ToString("dd MMM yyyy");
            }
        }
        public string AcceptanceDateSt
        {
            get
            {
                return AcceptanceDate.ToString("dd MMM yyyy");
            }
        }
        public string InvoiceTypeSt
        {
            get
            {
                return this.InvoiceType.ToString();
            }
        }
        public string ProductTypeSt
        {
            get
            {
                return this.ProductType.ToString();
            }
        }
        public string InvoiceDueValueSt
        {
            get
            {
                return Global.MillionFormat(this.InvoiceDueValue);
            }
        }
        public string PIQtySt
        {
            get
            {
                return Global.MillionFormat(this.PIQty);
            }
        }
        public string PIAmountSt
        {
            get
            {
                return Global.MillionFormat(this.PIAmount);
            }
        }
        public string LCAmountSt
        {
            get
            {
                return Global.MillionFormat(this.LCAmount);
            }
        }
        public string InvoiceValueSt
        {
            get
            {
                return Global.MillionFormat(this.InvoiceValue);
            }
        }
        public string GoodsRcvQtySt
        {
            get
            {
                return Global.MillionFormat(this.GoodsRcvQty);
            }
        }
        public string MaturityValueSt
        {
            get
            {
                return Global.MillionFormat(this.MaturityValue);
            }
        }
        #endregion

        #region Functions

        public List<ImportRegister> Gets(string sSQL, long nUserID)
        {
            return ImportRegister.Service.Gets(sSQL, nUserID);
        }

        #endregion

        #region ServiceFactory
        internal static IImportRegisterService Service
        {
            get { return (IImportRegisterService)Services.Factory.CreateService(typeof(IImportRegisterService)); }
        }
        #endregion
    }
    #endregion

    #region IImportRegister interface

    public interface IImportRegisterService
    {
        List<ImportRegister> Gets(string sSQL, long nUserID);
    }
    #endregion
}
