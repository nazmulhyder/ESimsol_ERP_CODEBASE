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
    #region ImportPaymentRequest
    public class ImportPaymentRequest : BusinessObject
    {
        public ImportPaymentRequest()
        {
            ImportPaymentRequestID = 0;
            RefNo = "";
            BankAccountID = 0;
            LetterIssueDate = DateTime.Now;
            RequestBy = 0;
            ApprovedBy = 0;
            Note = "";
            AccountNo = "";
            BankName = "";
            BranchName = "";
            BranchAddress = "";
            RequestByName = "";
            ApprovedByName = "";
            ErrorMessage = "";
            ImportPaymentRequestDetails = new List<ImportPaymentRequestDetail>();
            BankAccounts = new List<BankAccount>();
            BUID = 0;
            LiabilityType = EnumLiabilityType.None;
        }

        #region Properties
        public int ImportPaymentRequestID { get; set; }
        public string RefNo { get; set; }
        public int BankAccountID { get; set; }
        public int BUID { get; set; }
        public DateTime LetterIssueDate { get; set; }
        public int RequestBy { get; set; }
        public int ApprovedBy { get; set; }
        public string Note { get; set; }
        public string AccountNo { get; set; }
        public int BankID { get; set; }
        public int BankBranchID { get; set; }
        public string BankName { get; set; }
        public string BranchName { get; set; }
        public string BranchAddress { get; set; }
        public string RequestByName { get; set; }
        public string ApprovedByName { get; set; }
        public string ErrorMessage { get; set; }
        public EnumLiabilityType LiabilityType { get; set; }
        public int LiabilityTypeInt { get; set; }
        public int CurrencyType { get; set; }
        public int Paymentthrough { get; set; }
        public double CRate { get; set; }
        public int Status { get; set; }
        public int StatusInt { get; set; }
        #endregion

        #region Derived Property
        private string _sStatusSt = "";
        public string StatusSt
        {
            get
            {
                if (this.StatusInt <= 0)
                {
                    _sStatusSt = "Initialize";
                }
                else
                {
                    _sStatusSt = this.Status.ToString();
                }
                return _sStatusSt;
            }
        }
        public int SelectedOption { get; set; }
        public DateTime LetterIssueDate_end { get; set; }
        [DataMember]
        public List<ImportPaymentRequestDetail> ImportPaymentRequestDetails { get; set; }
        public List<ImportInvoice> PurchaseInvoiceLCs { get; set; }
        [DataMember]
        public List<BankAccount> BankAccounts { get; set; }
        public string LetterIssueDateInString
        {
            get
            {
                return this.LetterIssueDate.ToString("dd MMM yyyy");
            }
        }
        public string BankBranchName
        {
            get
            {
                return this.BankName + "[" + this.BranchName + "]";
            }
        }
        public string BankBranchAccount
        {
            get
            {
                return this.BankName + "[" + this.BranchName + "] " + this.AccountNo;
            }
        }
        public string LiabilityTypeST
        {
            get
            {
                return EnumObject.jGet(this.LiabilityType);
            }
        }
        #endregion

        #region Functions
        public static List<ImportPaymentRequest> Gets(int nUserID)
        {
            return ImportPaymentRequest.Service.Gets(nUserID);
        }
        public static List<ImportPaymentRequest> Gets(string sSQL, int nUserID)
        {
            return ImportPaymentRequest.Service.Gets(sSQL, nUserID);
        }
        public static List<ImportPaymentRequest> WaitForApproval(int buid, int nUserID)
        {
            return ImportPaymentRequest.Service.WaitForApproval(buid, nUserID);
        }

        public ImportPaymentRequest Get(int id, int nUserID)
        {
            return ImportPaymentRequest.Service.Get(id, nUserID);
        }
        public ImportPaymentRequest GetByPInvoice(int id, int nUserID)
        {
            return ImportPaymentRequest.Service.GetByPInvoice(id, nUserID);
        }
        public ImportPaymentRequest Save(int nUserID)
        {
            return ImportPaymentRequest.Service.Save(this, nUserID);
        }
        public ImportPaymentRequest Cancel_Request(int nUserID)
        {
            return ImportPaymentRequest.Service.Cancel_Request(this, nUserID);
        }
        public ImportPaymentRequest Approved(int nUserID)
        {
            return ImportPaymentRequest.Service.Approved(this, nUserID);
        }
        public string Delete(int nUserID)
        {
            return ImportPaymentRequest.Service.Delete(this, nUserID);
        }



        #endregion

        #region ServiceFactory


        internal static IImportPaymentRequestService Service
        {
            get { return (IImportPaymentRequestService)Services.Factory.CreateService(typeof(IImportPaymentRequestService)); }
        }
        #endregion
    }
    #endregion

    #region IImportPaymentRequest interface
    public interface IImportPaymentRequestService
    {
        ImportPaymentRequest Get(int id, Int64 nUserID);
        ImportPaymentRequest GetByPInvoice(int id, Int64 nUserID);
        List<ImportPaymentRequest> WaitForApproval(int nBUID, Int64 nUserID);
        List<ImportPaymentRequest> Gets(Int64 nUserID);
        List<ImportPaymentRequest> Gets(string sSQL, Int64 nUserID);
        string Delete(ImportPaymentRequest oImportPaymentRequest, Int64 nUserID);
        ImportPaymentRequest Save(ImportPaymentRequest oImportPaymentRequest, Int64 nUserID);
        ImportPaymentRequest Cancel_Request(ImportPaymentRequest oImportPaymentRequest, Int64 nUserID);
        ImportPaymentRequest Approved(ImportPaymentRequest oImportPaymentRequest, Int64 nUserID);
    }
    
    #endregion
}
