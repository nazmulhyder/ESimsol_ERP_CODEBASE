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
    #region ImportPaymentSettlement
    public class ImportPaymentSettlement : BusinessObject
    {
        public ImportPaymentSettlement()
        {
            PIPRDetailID = 0;
            ImportPaymentRequestID = 0;
            ImportInvoiceID = 0;
            ImportInvoiceNo = "";
            DateofInvoice = DateTime.Now;
            Amount = 0;
            DateofMaturity = DateTime.Now;
            BankName = "";
            BranchName = "";
            ErrorMessage = "";
            ImportPaymentID = 0;
            PaymentDate = DateTime.Today;
            PMTLiabilityNo = "";
            PMTLoanOpenDate = DateTime.Today;
            PMTRemarks = "";
            PMTAmount = 0;
            PMTCSymbol = "";
            PMTCCRate = 0;
            PMTAmountBC = 0;
            PMTApprovedBy = 0;
            PMTApprovedByName = "";
            ForExGainLoss = EnumForExGainLoss.None;
            BankShortName = "";
        }

        #region Properties
        public int PIPRDetailID { get; set; }
        public int ImportPaymentRequestID { get; set; }
        public int ImportInvoiceID { get; set; }
        public string ImportInvoiceNo { get; set; }
        public DateTime DateofInvoice { get; set; }
        public double Amount { get; set; }
        public string RefNo { get; set; }
        public string ImportLCNo { get; set; }
        public DateTime DateofMaturity { get; set; }
        public string BankName { get; set; }
        public string BranchName { get; set; }
        public string AccountNo { get; set; }
        public string SupplierName { get; set; }
        public double CCRate { get; set; }
        public int CurrencyID { get; set; }
        public string Currency_Inv { get; set; }
        public int Status { get; set; }
        public string ErrorMessage { get; set; }
        public int BankBranchID { get; set; }
        public EnumLiabilityType LiabilityType { get; set; }
        public int LiabilityTypeInt { get; set; }
        public int ImportPaymentID { get; set; }
        public DateTime PaymentDate { get; set; }
        public string PMTLiabilityNo { get; set; }
        public DateTime PMTLoanOpenDate { get; set; }
        public string PMTRemarks { get; set; }
        public double PMTAmount { get; set; }
        public string PMTCSymbol { get; set; }
        public double PMTCCRate { get; set; }
        public double PMTAmountBC { get; set; }
        public int PMTApprovedBy { get; set; }
        public string PMTApprovedByName { get; set; }
        public EnumForExGainLoss ForExGainLoss { get; set; }
        public string BankShortName { get; set; }
        #endregion

        #region Derived Property
        public int SelectedOption { get; set; }
        public int RequestBy { get; set; }
        public DateTime LetterIssueDate { get; set; }
        public string LiabilityTypeSt
        {
            get
            {
                return EnumObject.jGet(this.LiabilityType);
            }
        }
        public string ForExGainLossSt
        {
            get
            {
                return EnumObject.jGet(this.ForExGainLoss);
            }
        }
        public string LetterIssueDateSt
        {
            get
            {
                return this.LetterIssueDate.ToString("dd MMM yyyy");
            }
        }
        public string PaymentDateSt
        {
            get
            {
                if (this.PaymentDate == DateTime.MinValue)
                {
                    return "--";
                }
                else
                {
                    return this.PaymentDate.ToString("dd MMM yyyy");
                }
            }
        }
        public string DateofMaturityST
        {
            get
            {
                return this.DateofMaturity.ToString("dd MMM yyyy");
            }
        }
        public string PMTLoanOpenDateST
        {
            get
            {
                if (this.PMTLoanOpenDate == DateTime.MinValue)
                {
                    return "--";
                }
                else
                {
                    return this.PMTLoanOpenDate.ToString("dd MMM yyyy");
                }
            }
        }
        public string PMTAmountST
        {
            get
            {
                return this.PMTCSymbol + " " + Global.MillionFormat(this.PMTAmount);
            }
        }

        public string AmountSt
        {
            get
            {
                return this.Currency_Inv + Global.MillionFormat(this.Amount);
            }
        }
        public string StatusSt
        {
            get
            {
                return this.Status.ToString();
            }
        }
        #endregion

        #region Functions
        public static List<ImportPaymentSettlement> Gets(int buid, int nBankStatus, int nUserID)
        {
            return ImportPaymentSettlement.Service.Gets(buid, nBankStatus, nUserID);
        }
        public static List<ImportPaymentSettlement> Gets(string sSQL, int nUserID)
        {
            return ImportPaymentSettlement.Service.Gets(sSQL, nUserID);
        }
        public ImportPaymentSettlement Save(ImportPayment oImportPayment, int nUserID)
        {
            return ImportPaymentSettlement.Service.Save(oImportPayment, nUserID);
        }
        public ImportPaymentSettlement Approved(ImportPayment oImportPayment, int nUserID)
        {
            return ImportPaymentSettlement.Service.Approved(oImportPayment, nUserID);
        }

        #endregion

        #region ServiceFactory

        internal static IImportPaymentSettlementService Service
        {
            get { return (IImportPaymentSettlementService)Services.Factory.CreateService(typeof(IImportPaymentSettlementService)); }
        }
        #endregion
    }
    #endregion

    #region IImportPaymentSettlement interface
    public interface IImportPaymentSettlementService
    {
        List<ImportPaymentSettlement> Gets(int buid, int nBankStatus, Int64 nUserID);
        List<ImportPaymentSettlement> Gets(string sSQL, Int64 nUserID);
        ImportPaymentSettlement Save(ImportPayment oImportPayment, Int64 nUserID);
        ImportPaymentSettlement Approved(ImportPayment oImportPayment, Int64 nUserID);
    }
    #endregion
}