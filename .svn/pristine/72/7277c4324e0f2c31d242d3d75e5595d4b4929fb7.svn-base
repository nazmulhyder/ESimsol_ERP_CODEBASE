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
    #region LandingCostRegister
    public class LandingCostRegister : BusinessObject
    {
        public LandingCostRegister()
        {
            PurchaseInvoiceID = 0;
            BUID = 0;
            BillNo = "";
            DateofBill = DateTime.Today;
            DateofInvoice = DateTime.Today;
            InvoiceType = EnumPInvoiceType.None;
            RefType = EnumInvoiceReferenceType.None;
            PaymentMethod = EnumPaymentMethod.None;
            InvoicePaymentMode = EnumInvoicePaymentMode.None;
            InvoiceStatus = EnumPInvoiceStatus.Initialize;
            ContractorID = 0;
            CurrencyID = 0;
            ConvertionRate = 0;
            ApprovedBy = 0;
            ApprovedDate = DateTime.Today;
            BankAccountID = 0;
            PurchaseInvoiceDetailID = 0;
            ProductID = 0;
            LCID = 0;
            InvoiceID = 0;
            CostHeadID = 0;
            Remarks = "";
            LandingCostType = EnumLandingCostType.Ledger;
            Amount = 0;
            BUName = "";
            BUShortName = "";
            SupplierName = "";
            CurrencySymbol = "";
            ApprovedByName = "";
            BankAccountNo = "";
            ProductCode = "";
            ProductName = "";
            ImportLCNo = "";
            ImportLCDate = DateTime.Today;
            LCCurrencyID = 0;
            LCCurrencySymbol = "";
            ImportLCAmount = 0;
            ImportInvoiceNo = "";
            ImportInvoiceDate = DateTime.Today;
            ImportInvoiceAmount = 0;
            CostHeadCode = "";
            CostHeadName = "";
            ProductType = EnumProductNature.Yarn;
            SearchingData = "";
            ErrorMessage = "";
        }

        #region Properties
        public int PurchaseInvoiceID { get; set; }
        public int BUID { get; set; }
        public string BillNo { get; set; }
        public DateTime DateofBill { get; set; }
        public DateTime DateofInvoice { get; set; }
        public EnumPInvoiceType InvoiceType { get; set; }
        public EnumInvoiceReferenceType RefType { get; set; }
        public EnumPaymentMethod PaymentMethod { get; set; }
        public EnumInvoicePaymentMode InvoicePaymentMode { get; set; }
        public EnumPInvoiceStatus InvoiceStatus { get; set; }
        public int ContractorID { get; set; }
        public int CurrencyID { get; set; }
        public double ConvertionRate { get; set; }
        public int ApprovedBy { get; set; }
        public DateTime ApprovedDate { get; set; }
        public int BankAccountID { get; set; }
        public int PurchaseInvoiceDetailID { get; set; }
        public int ProductID { get; set; }
        public int LCID { get; set; }
        public int InvoiceID { get; set; }
        public int CostHeadID { get; set; }
        public string Remarks { get; set; }
        public EnumLandingCostType LandingCostType { get; set; }
        public double Amount { get; set; }
        public string BUName { get; set; }
        public string BUShortName { get; set; }
        public string SupplierName { get; set; }
        public string CurrencySymbol { get; set; }
        public string ApprovedByName { get; set; }
        public string BankAccountNo { get; set; }
        public string ProductCode { get; set; }
        public string ProductName { get; set; }
        public string ImportLCNo { get; set; }
        public DateTime ImportLCDate { get; set; }
        public int LCCurrencyID { get; set; }
        public string LCCurrencySymbol { get; set; }
        public double ImportLCAmount { get; set; }
        public string ImportInvoiceNo { get; set; }
        public DateTime ImportInvoiceDate { get; set; }
        public double ImportInvoiceAmount { get; set; }
        public string CostHeadCode { get; set; }
        public string CostHeadName { get; set; }
        public string ErrorMessage { get; set; }
        public EnumProductNature ProductType { get; set; }
        public string SearchingData { get; set; }
        #endregion

        #region Derived Property

        #endregion

        #region Functions
        public static List<LandingCostRegister> Gets(string sSQL, long nUserID)
        {
            return LandingCostRegister.Service.Gets(sSQL, nUserID);
        }
        #endregion

        #region ServiceFactory
        internal static ILandingCostRegisterService Service
        {
            get { return (ILandingCostRegisterService)Services.Factory.CreateService(typeof(ILandingCostRegisterService)); }
        }
        #endregion
    }
    #endregion

    #region ILandingCostRegister interface

    public interface ILandingCostRegisterService
    {
        List<LandingCostRegister> Gets(string sSQL, Int64 nUserID);
    }
    #endregion
}