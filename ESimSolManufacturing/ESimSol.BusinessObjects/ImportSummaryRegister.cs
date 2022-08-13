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
    #region ImportSummaryRegister
    public class ImportSummaryRegister : BusinessObject
    {
        public ImportSummaryRegister()
        {
            BUID = 0;
            ImportPIDetailID = 0;
            ImportPIID = 0;
            ImportPINo = "";
            ImportPIDate = DateTime.Now;
            ImportPIType = EnumImportPIType.None;
            ProductName = "";
            UnitPrice = 0;
            PIQty = 0;
            CRate = 0;
            PIAmount = 0;
            PartyName = "";
            SalesContractNo = "";
            SalesContractDate = DateTime.Now;
            LCID = 0;
            LCNo = "";
            LCDate = DateTime.Now;
            LCAdviseBankName = "";
            LCAmount = 0;
            InvoiceDetailID = 0;
            InvoiceID = 0;
            InvoiceNo = "";
            InvoiceDate = DateTime.Now;
            InvoiceQty = 0;
            InvoiceValue = 0;
            InvoiceDue = 0;
            AcceptanceDate = DateTime.Now;
            MaturityDate = DateTime.Now;
            PaymentDate = DateTime.Now;
            PaymentType = EnumLiabilityType.None;
            MaturityValue = 0;
            GRNDetailID = 0;
            RefType = EnumGRNType.None;
            RefObjectID = 0;
            GRNReceivedQty = 0;
            GoodReceivedDate = DateTime.Now;
            ErrorMessage = "";
            SearchingData = "";
            ReportLayout = "";
        }

        #region Property
        public int BUID { get; set; }
        public int ImportPIDetailID { get; set; }
        public int ImportPIID { get; set; }
        public string ImportPINo { get; set; }
        public DateTime ImportPIDate { get; set; }
        public string ProductName { get; set; }
        public double UnitPrice { get; set; }
        public double PIQty { get; set; }
        public double CRate { get; set; }
        public double PIAmount { get; set; }
        public string PartyName { get; set; }
        public string SalesContractNo { get; set; }
        public DateTime SalesContractDate { get; set; }
        public int LCID { get; set; }
        public string LCNo { get; set; }
        public DateTime LCDate { get; set; }
        public string LCAdviseBankName { get; set; }
        public double LCAmount { get; set; }
        public int InvoiceDetailID { get; set; }
        public int InvoiceID { get; set; }
        public string InvoiceNo { get; set; }
        public DateTime InvoiceDate { get; set; }
        public double InvoiceQty { get; set; }
        public double InvoiceValue { get; set; }
        public double InvoiceDue { get; set; }
        public DateTime AcceptanceDate { get; set; }
        public DateTime MaturityDate { get; set; }
        public DateTime PaymentDate { get; set; }
        public EnumLiabilityType PaymentType { get; set; }
        public double MaturityValue { get; set; }
        public int GRNDetailID { get; set; }
        public EnumGRNType RefType { get; set; }
        public EnumImportPIType ImportPIType { get; set; }
        public int RefObjectID { get; set; }
        public double GRNReceivedQty { get; set; }
        public DateTime GoodReceivedDate { get; set; }
        public string ErrorMessage { get; set; }
        #endregion

        #region Derived Property
        public string SearchingData { get; set; }
        public string ReportLayout { get; set; }
        public string SupplierIDs { get; set; }
        public string ProductIDs { get; set; }
        public string ImportPITypeSt
        {
            get
            {
                return EnumObject.jGet(this.ImportPIType);
            }
        }
        public string RefTypeSt
        {
            get
            {
                return EnumObject.jGet(this.RefType);
            }
        }
        public string PaymentTypeSt
        {
            get
            {
                return EnumObject.jGet(this.PaymentType);
            }
        }
        public string ImportPIDateInString
        {
            get
            {
                return ImportPIDate.ToString("dd MMM yyyy");
            }
        }
        public string SalesContractDateInString
        {
            get
            {
                return SalesContractDate.ToString("dd MMM yyyy");
            }
        }
        public string LCDateInString
        {
            get
            {
                return LCDate.ToString("dd MMM yyyy");
            }
        }
        public string InvoiceDateInString
        {
            get
            {
                return InvoiceDate.ToString("dd MMM yyyy");
            }
        }
        public string AcceptanceDateInString
        {
            get
            {
                return AcceptanceDate.ToString("dd MMM yyyy");
            }
        }
        public string MaturityDateInString
        {
            get
            {
                return MaturityDate.ToString("dd MMM yyyy");
            }
        }
        public string PaymentDateInString
        {
            get
            {
                return PaymentDate.ToString("dd MMM yyyy");
            }
        }
        public string GoodReceivedDateInString
        {
            get
            {
                return GoodReceivedDate.ToString("dd MMM yyyy");
            }
        }
        #endregion

        #region Functions
        public static List<ImportSummaryRegister> Gets(string sGRNDSql, string sInvoiceDSql, string sPIDSql, long nUserID)
        {
            return ImportSummaryRegister.Service.Gets(sGRNDSql, sInvoiceDSql, sPIDSql, nUserID);
        }
        
        #endregion

        #region ServiceFactory
        internal static IImportSummaryRegisterService Service
        {
            get { return (IImportSummaryRegisterService)Services.Factory.CreateService(typeof(IImportSummaryRegisterService)); }
        }
        #endregion
    }
    #endregion

    #region IImportSummaryRegister interface
    public interface IImportSummaryRegisterService
    {
        List<ImportSummaryRegister> Gets(string sGRNDSql, string sInvoiceDSql, string sPIDSql, Int64 nUserID);
    }
    #endregion
}
