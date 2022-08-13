using System;
using System.IO;
using ESimSol.BusinessObjects;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Collections.Generic;
using ICS.Core.Framework;
using ICS.Core.Utility;

namespace ESimSol.BusinessObjects
{
    #region ImportPayment    
    public class ImportPayment : BusinessObject
    {
        public ImportPayment()
        {
            ImportPaymentID = 0;
            ImportInvoiceID = 0;
            LiabilityType = EnumLiabilityType.None;
            LiabilityTypeInt = 0;
            PaymentDate = DateTime.Today;
            MarginAccountID = 0;
            MarginCurrencyID = 0;
            LCMarginAmount = 0;
            MarginCCRate = 0;
            LCMarginAmountBC = 0;
            LiabilityNo = "";
            InterestRate = 0.00;
            DateOfOpening = DateTime.Today;
            DateOfMaturity = DateTime.Today;
            BankAccountID = 0;
            CurrencyID = 0;
            Amount = 0;
            CCRate = 0;
            AmountBC = 0;
            Remarks = "";
            MarginSettledRate = 0;
            LiabilitySettledRate = 0;
            ForExGainLoss = EnumForExGainLoss.None;
            ForExGainLossInt = 0;
            ForExCurrencyID = 0;
            ForExAmount = 0;
            ForExCCRate = 0;
            ForExAmountBC = 0;
            ChargeAmount = 0;
            ChargeAmountBC = 0;
            ApprovedBy = 0;
            ImportInvoiceNo = "";
            ImportLCNo = "";
            InvoiceCurrencyID = 0;
            Amount_Invoice = 0;
            CRate_Acceptance = 0;
            Amount_LC = 0;
            MarginAccountName = "";
            MarginBankName = "";
            MarginAccountNo = "";
            MarginCSymbol = "";
            BankAccountName = "";
            BankName = "";
            BankBranchID = 0;
            BranchName = "";
            AccountNo = "";
            AccountType = EnumBankAccountType.None;
            CSymbol = "";
            ForExCSymbol = "";
            ApprovedByName = "";
            BUID = 0;
            ErrorMessage = "";
            EHTransactions = new List<EHTransaction>();

        }

        #region Properties
        public int ImportPaymentID { get; set; }
        public int ImportInvoiceID { get; set; }
        public EnumLiabilityType LiabilityType { get; set; }
        public DateTime PaymentDate { get; set; }
        public int LiabilityTypeInt { get; set; }
        public int MarginAccountID { get; set; }
        public int MarginCurrencyID { get; set; }
        public double LCMarginAmount { get; set; }
        public double MarginCCRate { get; set; }
        public double LCMarginAmountBC { get; set; }
        public string LiabilityNo { get; set; }
        public double InterestRate { get; set; }
        public DateTime DateOfOpening { get; set; }
        public DateTime DateOfMaturity { get; set; }
        public int BankAccountID { get; set; }
        public int CurrencyID { get; set; }
        public double Amount { get; set; }
        public double CCRate { get; set; }
        public double AmountBC { get; set; }
        public string Remarks { get; set; }
        public double MarginSettledRate { get; set; }
        public double LiabilitySettledRate { get; set; }
        public EnumForExGainLoss ForExGainLoss { get; set; }
        public int ForExGainLossInt { get; set; }
        public int ForExCurrencyID { get; set; }
        public double ForExAmount { get; set; }
        public double ForExCCRate { get; set; }
        public double ForExAmountBC { get; set; }
        public double ChargeAmount { get; set; }
        public double ChargeAmountBC { get; set; }
        public int ApprovedBy { get; set; }
        public string ImportInvoiceNo { get; set; }
        public string ImportLCNo { get; set; }
        public int InvoiceCurrencyID { get; set; }
        public double Amount_Invoice { get; set; }
        public double CRate_Acceptance { get; set; }
        public double Amount_LC { get; set; }
        public string MarginAccountName { get; set; }
        public string MarginBankName { get; set; }
        public string MarginAccountNo { get; set; }
        public string MarginCSymbol { get; set; }
        public string BankAccountName { get; set; }
        public string BankName { get; set; }
        public int BankBranchID { get; set; }
        public string BranchName { get; set; }
        public string AccountNo { get; set; }
        public EnumBankAccountType AccountType { get; set; }
        public string CSymbol { get; set; }
        public string ForExCSymbol { get; set; }
        public string ApprovedByName { get; set; }
        public int BUID { get; set; }
        public string ErrorMessage { get; set; }
        public List<EHTransaction> EHTransactions { get; set; }
        public string DateOfOpeningST
        {
            get
            {
                return DateOfOpening.ToString("dd MMM yyyy");
            }
        }
        public string DateOfMaturityST
        {
            get
            {
                return this.DateOfMaturity.ToString("dd MMM yyyy");
            }
        }
        public string LiabilityTypeSt
        {
            get
            {
                return EnumObject.jGet(this.LiabilityType);
            }
        }
        public string PaymentDateSt
        {
            get 
            {
                return this.PaymentDate.ToString("dd MMM yyyy");
            }
        }
        public string ForExGainLossSt
        {
            get
            {
                return EnumObject.jGet(this.ForExGainLoss);
            }
        }
        #endregion


        #region Functions
        public ImportPayment Get(int nid, int nUserID)
        {
            return ImportPayment.Service.Get(nid, nUserID);
        }
        public static List<ImportPayment> Gets(int nUserID)
        {
            return ImportPayment.Service.Gets(nUserID);
        }
        public static List<ImportPayment> Gets(string sSQL, int nUserID)
        {
            return ImportPayment.Service.Gets(sSQL, nUserID);
        }

        public ImportPayment GetBy(int nImportInvoiceID, int nUserID)
        {
            return ImportPayment.Service.GetBy(nImportInvoiceID, nUserID);
        }

        //public ImportPayment Save( int nUserID)
        //{
        //    return ImportPayment.Service.Save(this, nUserID);
        //}
        //public string Delete(int nUserID)
        //{
        //    return ImportPayment.Service.Delete(this, nUserID);
        //}


        #endregion

        #region ServiceFactory

        internal static IImportPaymentService Service
        {
            get { return (IImportPaymentService)Services.Factory.CreateService(typeof(IImportPaymentService)); }
        }
        #endregion
    }
    #endregion

    #region IImportPayment interface
    public interface IImportPaymentService
    {
        ImportPayment Get(int id, Int64 nUserID);
        ImportPayment GetBy(int nImportInvoiceID, Int64 nUserID);
        List<ImportPayment> Gets(Int64 nUserID);
        List<ImportPayment> Gets(string sSQL, Int64 nUserID);        
    }
    #endregion
}
