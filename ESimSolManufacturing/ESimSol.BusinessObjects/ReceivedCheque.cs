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
    #region ReceivedCheque
    public class ReceivedCheque : BusinessObject
    {
        public ReceivedCheque()
        {
            ReceivedChequeID = 0;
            ContractorID = 0;
            IssueDate = DateTime.Now;
            ChequeStatus = EnumReceivedChequeStatus.Initiate;
            ChequeNo = "";
            TransactionType = EnumTransactionType.None;
            ChequeDate = DateTime.Now;
            Amount = 0;
            BankName = "";
            BranchName = "";
            AccountNo = "";
            Remarks = "";
            ReceivedAccontID = 0;
            AuthorizedBy = 0;
            MoneyReceiptNo = "";
            MoneyReceiptDate = DateTime.Today;
            BillNumber = "";
            ProductDetails = "";
            ReceivedAccontNo = "";
            AuthorizedByName = "";
            ContractorName = "";
            SubLedgerID = 0;
            SubLedgerName = "";
            SubLedgerCode = "";
            SubLedgerNameCode = "";
            Setup = EnumVoucherSetup.None;
            ErrorMessage = "";
        }

        #region Properties
        public int ReceivedChequeID { get; set; }
        public int ContractorID { get; set; }//ACCostCenterID
        public DateTime IssueDate { get; set; }
        public EnumReceivedChequeStatus ChequeStatus { get; set; }
        public string ChequeNo { get; set; }
        public EnumTransactionType TransactionType { get; set; }
        public DateTime ChequeDate { get; set; }
        public double Amount { get; set; }
        public string BankName { get; set; }
        public string BranchName { get; set; }
        public string AccountNo { get; set; }
        public string Remarks { get; set; }
        public int ReceivedAccontID { get; set; }
        public int SubLedgerID { get; set; }
        public int AuthorizedBy { get; set; }
        public string MoneyReceiptNo { get; set; }
        public DateTime MoneyReceiptDate { get; set; }
        public string BillNumber { get; set; }
        public string ProductDetails { get; set; }
        public string ReceivedAccontNo { get; set; }
        public string AuthorizedByName { get; set; }
        public string ContractorName { get; set; }
        public string SubLedgerName { get; set; }
        public string SubLedgerCode { get; set; }
        public string SubLedgerNameCode { get; set; }
        public EnumVoucherSetup Setup { get; set; }
        public string ErrorMessage { get; set; }
        #endregion

        #region Derive Property
        public List<Voucher> Vouchers { get; set; }
        public List<EnumObject> TransactionTypes { get; set; }
        public string ChequeStatusSt { get { return EnumObject.jGet(this.ChequeStatus); } }
        public string TransactionTypeSt { get { return EnumObject.jGet(this.TransactionType); } }
        public string AmountSt { get { return Global.MillionFormat(this.Amount); } }
        public string AmountTaka { get { return Global.TakaWords(this.Amount); } }
        public string IssueDateSt { get { return this.IssueDate.ToString("dd MMM yyyy"); } }
        public string ChequeDateSt { get { return this.ChequeDate.ToString("dd MMM yyyy"); } }
        public string MoneyReceiptDateSt { get { return this.MoneyReceiptDate.ToString("dd MMM yyyy"); } }
        #endregion

        #region Functions
        public static List<ReceivedCheque> Gets(int nUserID)
        {
            return ReceivedCheque.Service.Gets(nUserID);
        }
        public static List<ReceivedCheque> Gets(string sSQL, int nUserID)
        {
            return ReceivedCheque.Service.Gets(sSQL, nUserID);
        }
        public List<ReceivedCheque> GetsForSuggestion(int nUserID)
        {
            return ReceivedCheque.Service.GetsForSuggestion(this, nUserID);
        }
        public ReceivedCheque Get(int id, int nUserID)
        {
            return ReceivedCheque.Service.Get(id, nUserID);
        }
        public ReceivedCheque Save(int nUserID)
        {
            return ReceivedCheque.Service.Save(this, nUserID);
        }
        public string Delete(int id, int nUserID)
        {
            return ReceivedCheque.Service.Delete(id, nUserID);
        }

        public ReceivedCheque UpdateReceivedChequeStatus(ReceivedChequeHistory oReceivedChequeHistory, int nCurrentUserID)
        {
            return ReceivedCheque.Service.UpdateReceivedChequeStatus(this, oReceivedChequeHistory, nCurrentUserID);
        }

        #endregion

        #region ServiceFactory
        internal static IReceivedChequeService Service
        {
            get { return (IReceivedChequeService)Services.Factory.CreateService(typeof(IReceivedChequeService)); }
        }
        #endregion
    }
    #endregion

    #region IReceivedCheque interface
    public interface IReceivedChequeService
    {
        ReceivedCheque Get(int id, int nUserID);
        List<ReceivedCheque> Gets(int nUserID);
        List<ReceivedCheque> Gets(string sSQL, int nUserID);
        List<ReceivedCheque> GetsForSuggestion(ReceivedCheque oReceivedCheque, int nUserID);
        string Delete(int id, int nUserID);
        ReceivedCheque Save(ReceivedCheque oOrganizationalInformation, int nUserID);
        ReceivedCheque UpdateReceivedChequeStatus(ReceivedCheque oReceivedCheque, ReceivedChequeHistory oReceivedChequeHistory, int nCurrentUserID);
    }
    #endregion
}