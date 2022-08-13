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
    #region PurchasePaymentContract
    public class ImportLCDetail : BusinessObject
    {
        #region  Constructor
        public ImportLCDetail()
        {
            ImportLCDetailID = 0;
            ImportLCID = 0;
            ImportLCLogID = 0;
            ImportPIType = EnumImportPIType.Foreign;
            ImportPIID = 0;
            Amount = 0;
            LCAppTypeInt = 0;
            IsTransShipmentAllow = true;
            IsTransShipmentAllow = true;
            ProductType = 0;
            ProductTypeName = "";
            AmendmentDate = DateTime.Now;
        }
        #endregion

        #region Properties
        public int ImportLCDetailID { get; set; }
        public int ImportLCDetailLogID { get; set; }
        public int ImportLCID { get; set; }
        public int ImportLCLogID { get; set; }
        public int ImportPIID { get; set; }
        public double Amount { get; set; }
        public DateTime IssueDate { get; set; }
        public List<ImportLCDetailProduct> ImportLCDetailProducts { get; set; }
        public double ConversionRate { get; set; }
        public double Qty { get; set; }
        public int AdviseBankBranchID  { get; set; }
        public DateTime AskingDeliveryDate { get; set; }
        public DateTime DateOfApproved { get; set; }
        public bool IsTransShipmentAllow { get; set; }
        public bool IsPartShipmentAllow { get; set; }
        public int SupplierID { get; set; }
        public string SupplierName { get; set; }
        public string ProductTypeName { get; set; }
        public string SupplierNameCode { get; set; }
        public int ContractorPersonnelID { get; set; }
        public int CurrencyID { get; set; }
        public int LCTermID { get; set; }
        public int PaymentInstructionInt { get; set; }
        public string Currency { get; set; }
        public string LCTermsName { get; set; }
        public string Note { get; set; }
        public string NumberOfContainers { get; set; }
        public double OverDueRate { get; set; }
        public EnumPaymentInstruction PaymentInstruction  { get; set; }
        public string ImportPINo { get; set; }
        public string BankName { get; set; }
        public EnumImportPIType ImportPIType  { get; set; }
        public int PurchaseContactTypeInt { get; set; }
        public EnumShipmentBy ShipmentBy { get; set; }
        public int LCAppTypeInt { get; set; }
        public string IsPartShipmentAllowInString
        {
            get
            {
                if(this.IsPartShipmentAllow)
                {
                    return "Yes";
                }
                else
                {
                    return "No";
                }
            }
        }
        public string SLNo { get; set; }
        public string DateOfApprovedInString
        {
            get
            {
                return this.DateOfApproved.ToString("dd MMM yyyy");
            }
        }
        public string PurchaseContactTypeInString
        {
            get
            {
                return ImportPIType.ToString();
            }
        }

        public string PaymentInstructionTypeSt
        {
            get
            {
                return EnumObject.jGet(this.PaymentInstruction);
            }
        }

        public string ShipmentByInString
        {
            get
            {
                return ShipmentBy.ToString();
            }
        }
        public string AmendmentDateSt
        {
            get
            {
                if (this.AmendmentDate == DateTime.MinValue)
                {
                    return "";
                }
                else
                {
                    return this.AmendmentDate.ToString("dd MMM yyyy");
                }
            }
        }
        public string AmountSt
        {
            get
            {
                return this.Currency+""+Global.MillionFormat(this.Amount);
            }
        }
        public EnumProductNature ProductType { get; set; }
        public DateTime AmendmentDate { get; set; }
     //   
        #endregion


        #region Functions
        public ImportLCDetail Get(int nImportLCDetailDetailID, int nUserID)
        {
            return ImportLCDetail.Service.Get(nImportLCDetailDetailID, nUserID);
        }
     
        public static List<ImportLCDetail> Gets(int nImportLCID, int nUserID)
        {
            return ImportLCDetail.Service.Gets(nImportLCID, nUserID);
        }

        public static List<ImportLCDetail> GetsLog(int nImportLCLogID, int nUserID)
        {
            return ImportLCDetail.Service.GetsLog(nImportLCLogID, nUserID);
        }
        public static List<ImportLCDetail> GetsByInvoice(int nInvoiceID, int nUserID)
        {
            return ImportLCDetail.Service.GetsByInvoice(nInvoiceID, nUserID);
        }

        public string Delete( int nUserID)
        {
            return ImportLCDetail.Service.Delete(this, nUserID);
        }


        #endregion

        #region ServiceFactory

        internal static IImportLCDetailService Service
        {
            get { return (IImportLCDetailService)Services.Factory.CreateService(typeof(IImportLCDetailService)); }
        }

        #endregion

    }
    #endregion

    #region IPurchasePaymentContract interface

    public interface IImportLCDetailService
    {
        ImportLCDetail Get(int id, Int64 nUserID);
        List<ImportLCDetail> Gets(int nImportLCID, Int64 nUserID);
        List<ImportLCDetail> GetsLog(int nImportLCLogID, Int64 nUserID);
        List<ImportLCDetail> GetsByInvoice(int nInvoiceID, Int64 nUserID);
        string Delete(ImportLCDetail oPurchasePaymentContract, Int64 nUserID);
     

    }
    #endregion
    
}
