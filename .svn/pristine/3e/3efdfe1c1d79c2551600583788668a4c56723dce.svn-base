using System;
using System.IO;
using System.ComponentModel.DataAnnotations;
using ESimSol.BusinessObjects;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Collections.Generic;
using ICS.Core.Framework;
using ICS.Core.Utility;

namespace ESimSol.BusinessObjects
{
    #region ImportClaim

    public class ImportClaim : BusinessObject
    {
        public ImportClaim()
        {
            ImportClaimID = 0;
            ImportInvoiceID = 0;
            ImportInvoiceNo = "";
            ImportInvoiceQty = 0;
            ImportInvoiceAmount = 0;
            IssueDate = DateTime.Today;
            ImportLCID = 0;
            ImportLCNo = "";
            ImportLCDate = DateTime.Now;
            ImportInvoiceDate = DateTime.MinValue;
            ContractorName = "";
            ClaimNo = "";
            ClaimReasonID=0;
            Note = "";
            SettleBy=0;
            RequestBy=0;
            RequestDate = DateTime.Today;
            ApproveBy=0;
            Amount = 0;
            ApproveDate = DateTime.Today;
            this.ImportClaimDetails = new List<ImportClaimDetail>();
            ErrorMessage = "";
            Currency = "";
        }

        #region Properties
        public int ImportClaimID { get; set; }
        public int ImportLCID { get; set; }
        public int ImportInvoiceID { get; set; }
        public string ClaimNo { get; set; }
        public int ClaimReasonID { get; set; }
        public string Note { get; set; }
        public int SettleBy { get; set; }
        public int RequestBy { get; set; }
        public DateTime RequestDate { get; set; }
        public int ApproveBy { get; set; }
        public DateTime ApproveDate { get; set; }
        public DateTime ImportInvoiceDate { get; set; }
        public string ImportInvoiceNo { get; set; }
        public double ImportInvoiceQty { get; set; }
        public double ImportInvoiceAmount { get; set; }
        public double Amount { get; set; }
        public DateTime IssueDate { get; set; }
        public string ImportLCNo { get; set; }
        public DateTime ImportLCDate { get; set; }
        public string ContractorName { get; set; }
        public string ErrorMessage { get; set; }
        public string Currency { get; set; }
        public List<ImportClaimDetail> ImportClaimDetails { get; set; }
        #endregion

        #region Derived Property
        public int BUID { get; set; }
        public string ImportInvoiceAmountST
        {
            get
            {
                return Global.MillionFormat(this.ImportInvoiceAmount);
            }
        }
        public string IssueDateST
        {
            get
            {
                if (this.IssueDate == DateTime.MinValue)
                {
                    return "-";
                }
                else
                {
                    return IssueDate.ToString("dd MMM yyyy");
                }
            }
        }
        public string ImportLCDateST
        {
            get
            {
                if (this.ImportLCDate == DateTime.MinValue)
                {
                    return "-";
                }
                else
                {
                    return ImportLCDate.ToString("dd MMM yyyy");
                }
            }
        }
        public string RequestDateST
        {
            get
            {
                if (this.RequestDate == DateTime.MinValue)
                {
                    return "-";
                }
                else
                {
                    return RequestDate.ToString("dd MMM yyyy");
                }
            }
        }
        public string ApproveDateST
        {
            get
            {
                if (this.ApproveDate == DateTime.MinValue)
                {
                    return "-";
                }
                else
                {
                    return ApproveDate.ToString("dd MMM yyyy");
                }
            }
        }
        public string ImportInvoiceDateST
        {
            get
            {
                if (this.ImportInvoiceDate == DateTime.MinValue)
                {
                    return "-";
                }
                else
                {
                    return ImportInvoiceDate.ToString("dd MMM yyyy");
                }
            }
        }

        public string ImportClaimStatus
        {
            get
            {
                if (this.RequestBy != 0 && this.ApproveBy == 0)
                {
                    return "Req. For Approval";
                }
                else if (this.ApproveBy != 0)
                {
                    return "Approved";
                }
                else 
                {
                    return "Initialized";
                }
            }
        }
        #endregion

  

        #region Functions
        public static List<ImportClaim> Gets(long nUserID)
        {
            return ImportClaim.Service.Gets(nUserID);
        }
        public static List<ImportClaim> Gets(string sSQL, long nUserID)
        {
            return ImportClaim.Service.Gets(sSQL, nUserID);
        }
        public ImportClaim Get(int id, long nUserID)
        {
            return ImportClaim.Service.Get(id, nUserID);
        }
      
        public ImportClaim Save(long nUserID)
        {
            return ImportClaim.Service.Save(this, nUserID);
        }
        public ImportClaim Request(Int64 nUserID)
        {
            return ImportClaim.Service.Request(this, nUserID);
        }
        public ImportClaim Approve(Int64 nUserID)
        {
            return ImportClaim.Service.Approve(this, nUserID);
        }
        public string Delete(long nUserID)
        {
            return ImportClaim.Service.Delete(this, nUserID);
        }

        #endregion

        #region ServiceFactory
        internal static IImportClaimService Service
        {
            get { return (IImportClaimService)Services.Factory.CreateService(typeof(IImportClaimService)); }
        }
        #endregion
    }
    #endregion

    #region IImportClaim interface

    public interface IImportClaimService
    {
        ImportClaim Get(int id, Int64 nUserID);
        List<ImportClaim> Gets(string sSQL, long nUserID);
        List<ImportClaim> Gets(Int64 nUserID);
        string Delete(ImportClaim oImportClaim, Int64 nUserID);
        ImportClaim Save(ImportClaim oImportClaim, Int64 nUserID);
        ImportClaim Request(ImportClaim oImportClaim, Int64 nUserID);
        ImportClaim Approve(ImportClaim oImportClaim, Int64 nUserID);
    }
    #endregion
}
