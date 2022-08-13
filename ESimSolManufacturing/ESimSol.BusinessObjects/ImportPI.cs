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
    #region ImportPI
    public class ImportPI : BusinessObject
    {
        #region  Constructor
        public ImportPI()
        {
            ImportPIID = 0;
            ImportPINo = "";
            BUID = 0;
            SLNo = "";
            IssueDate = DateTime.Today;
            ReceiveDate = DateTime.Today;
            ValidityDate = IssueDate.AddDays(7);
            ImportPIStatus = EnumImportPIState.Initialized;
            ImportPIStatusInt = 0;
            SupplierID = 0;
            ContactPersonID = 0;
            ConcernPersonID = 0;
            CurrencyID = 0;
            TotalValue = 0;
            BankBranchID_Advise = 0;
            LCTermID = 0;
            ContainerNo = "";
            ImportPIType = EnumImportPIType.None;
            ImportPITypeInt = 0;
            AskingDeliveryDate = DateTime.MinValue;
            AskingLCDate = DateTime.MinValue;
            IsTransShipmentAllow = true;
            IsPartShipmentAllow = true;
            OverDueRate = 0;
            IsLIBORrate = true;
            ApprovedBy = 0;
            DateOfApproved = DateTime.Today;
            VersionNumber = 0;
           
            DeliveryClause = "";
            PaymentClause = "";
            ShipmentBy = EnumShipmentBy.None;
            ShipmentByInt = 0;
            IsReviseRequest = false;
            PaymentInstructionType = EnumPaymentInstruction.None;
            PaymentInstructionTypeInt = 0;
            Note = "";
            IsCreateReviseNo = true;
            SupplierName = "";
            SupplierNameCode = "";
            CurrencySymbol = "";
            BankName = "";
            BranchName = "";
            CPersonName = "";
            BUCode = "";
            BUName = "";
            LCTermsName = "";
            ConcernPersonName = "";
            ApproveByName = "";
            ImportPILogID = 0;
            ImportLCID = 0;
            AgentID = 0;
            AgentContactPersonID = 0;
            RateUnit = 1;
            AgentName = "";
            AgentContactPersonName = "";
            ErrorMessage = "";
            ImportPIDetails = new List<ImportPIDetail>();
            ImportPIGRNDetails = new List<ImportPIGRNDetail>();
            ProductType = 0;
            ShipmentTermInt = 0;
            ShipmentTerm = EnumShipmentTerms.None;
            RefType = EnumImportPIRefType.None;
            RefTypeInInt = 0;
            PIEntryType = EnumImportPIEntryType.Open_PI;
            PIEntryTypeInInt = 0;
            SwiftCode = "";
            ImportLCNo = "";
            ConvertionRate = 0;
            DiscountAmount = 0;
            Count = 0;
            ImportPIReferenceList = new List<ImportPIReference>();

            ProductTypeName = "";
        }
        #endregion
        #region Properties
        public int ImportPIID { get; set; }
        public int ImportPILogID { get; set; }
        public string ImportPINo { get; set; }
        public string ProductTypeName { get; set; }
        public int BUID { get; set; }
        public string SLNo { get; set; }
        public double ConvertionRate { get; set; }
        public double DiscountAmount { get; set; }
        public DateTime IssueDate { get; set; }
        public DateTime ReceiveDate { get; set; }
        public EnumImportPIState ImportPIStatus { get; set; }
        public int ImportPIStatusInt { get; set; }
        public int SupplierID { get; set; }
        public int ContactPersonID { get; set; }
        public int ConcernPersonID { get; set; }
        public int CurrencyID { get; set; }
        public bool IsCreateReviseNo { get; set; }
        public double TotalValue { get; set; }
        public int BankBranchID_Advise { get; set; }
        public int LCTermID { get; set; }
        public string ContainerNo { get; set; }
        public EnumImportPIType ImportPIType { get; set; }
        public int ImportPITypeInt { get; set; }
        public DateTime AskingDeliveryDate { get; set; }
        public bool IsTransShipmentAllow { get; set; }
        public bool IsPartShipmentAllow { get; set; }
        public double OverDueRate { get; set; }
        public bool IsLIBORrate { get; set; }
        public int ApprovedBy { get; set; }
        public DateTime DateOfApproved { get; set; }
        public int VersionNumber { get; set; }
        public DateTime ValidityDate { get; set; }
        public DateTime AskingLCDate { get; set; }
        public string DeliveryClause { get; set; }
        public string PaymentClause { get; set; }
        public EnumShipmentBy ShipmentBy { get; set; }
        public int ShipmentByInt { get; set; }
        public bool IsReviseRequest { get; set; }
        public EnumPaymentInstruction PaymentInstructionType { get; set; }
        public int PaymentInstructionTypeInt { get; set; }
        public string Note { get; set; }
        public string SupplierName { get; set; }
        public string SupplierNameCode { get; set; }
        public string CurrencySymbol { get; set; }
        public string BankName { get; set; }
        public string BranchName { get; set; }
        public string SwiftCode { get; set; }
        public string CPersonName { get; set; }
        public string BUCode { get; set; }
        public string BUName { get; set; }
        public string SupplierAddress { get; set; }
        public string LCTermsName { get; set; }
        public string ConcernPersonName { get; set; }
        public string ApproveByName { get; set; }
        public int CurrentUserId { get; set; }
        public Company Company { get; set; }
        public int ImportLCID { get; set; }
        public int RateUnit { get; set; }
        public EnumProductNature ProductType { get; set; }
        public int AgentID { get; set; }
        public int AgentContactPersonID { get; set; }
        public string AgentName { get; set; }
        public string AgentContactPersonName { get; set; }
        public EnumShipmentTerms ShipmentTerm { get; set; }
        public int ShipmentTermInt { get; set; }
        public string ImportLCNo { get; set; }
        public int RefTypeInInt { get; set; }
        public int PIEntryTypeInInt { get; set; }
        public EnumImportPIRefType RefType { get; set; }
        public EnumImportPIEntryType PIEntryType { get; set; }
        public string ErrorMessage { get; set; }
        public int Count { get; set; }

        #region Derive Property
        public List<ImportPI> ImportPIs { get; set; }
        public List<ImportPIDetail> ImportPIDetails { get; set; }
        public string AmountSt
        {
            get
            {
                return this.CurrencySymbol + Global.MillionFormat(this.TotalValue);
            }
        }
        public string ImportPIStatusSt
        {
            get
            {
                return this.ImportPIStatus.ToString();
            }
        }
        public string PaymentInstructionTypeSt
        {
            get
            {
                return PaymentInstructionType.ToString();
            }
        }
        public string ImportPITypeSt
        {
            get
            {
                return this.ImportPIType.ToString();
            }
        }
        public string IssueDateSt
        {
            get
            {
                return IssueDate.ToString("dd MMM yyyy");
            }
        }
        public string ReceiveDateSt
        {
            get
            {
                return this.ReceiveDate.ToString("dd MMM yyyy");
            }
        }
        public string validityDateSt
        {
            get
            {
                return ValidityDate.ToString("dd MMM yyyy");
            }
        }
        public string AskingLCDateInString
        {
             get
            {
                if (this.AskingLCDate == DateTime.MinValue)
                {
                    return "";
                }
                else
                {
                    return this.AskingLCDate.ToString("dd MMM yyyy");
                }
            }
            
        }
        public string AskingDeliveryDateInString
        {
             get
            {
                if (this.AskingDeliveryDate == DateTime.MinValue)
                {
                    return "";
                }
                else
                {
                    return this.AskingDeliveryDate.ToString("dd MMM yyyy");
                }
            }
            
        }
        
        #endregion
        #endregion

        #region Derived Properties
        public List<ImportPIReference> ImportPIReferenceList { get; set; }
        public string PINoWithVersion
        {
            get
            {
                if(this.VersionNumber>0)
                {
                    return this.SLNo + "-" + this.VersionNumber;
                }
                else
                {
                    return this.SLNo;
                }
            }
        }
        public List<ImportPIGRNDetail> ImportPIGRNDetails { get; set; }
        #endregion

        #region Functions [DB]
        public ImportPI Get(int nImportPIID, int nUserID)
        {
            return ImportPI.Service.Get(nImportPIID, nUserID);
        }
        public ImportPI Get(string sPurchaseContactNo, int nUserID)
        {
            return ImportPI.Service.Get(sPurchaseContactNo, nUserID);
        }
        public static List<ImportPI> Gets(string sSQL, int nUserID)
        {
            return ImportPI.Service.Gets(sSQL, nUserID);
        }
        public static List<ImportPI> GetsByImportPIType(string sPCTypesIDs, int nUserID)
        {
            return ImportPI.Service.GetsByImportPIType(sPCTypesIDs, nUserID);
        }
        public static List<ImportPI> Gets(int nSupplierID, int nUserID)
        {
            return ImportPI.Service.Gets(nSupplierID, nUserID);
        }
        public static List<ImportPI> GetsImportPI(int nSupplierID, string sStatus, string sPCType, int nUserID)
        {
            return ImportPI.Service.GetsImportPI(nSupplierID, sStatus, sPCType, nUserID);
        }
        public ImportPI Save(long nUserID)
        {
            return ImportPI.Service.Save(this, nUserID);
        }
        public string Delete(long nUserID)
        {
            return ImportPI.Service.Delete(this, nUserID);
        }
        public ImportPI ApproveImportPI(long nUserID)
        {
            return ImportPI.Service.ApproveImportPI(this, nUserID);
        }
        public ImportPI RequestForReviseImportPI(long nUserID)
        {
            return ImportPI.Service.RequestForReviseImportPI(this, nUserID);
        }
        public ImportPI ReviseImportPI(long nUserID)
        {
            return ImportPI.Service.ReviseImportPI(this, nUserID);
        }
        public static List<ImportPI> GetsByLCID(int nLCID, Int64 nUserID)
        {
            return ImportPI.Service.GetsByLCID(nLCID, nUserID);
        }
        public ImportPI UpdateAmount(long nUserID)
        {
            return ImportPI.Service.UpdateAmount(this, nUserID);
        }
   
        #endregion

        #region ServiceFactory
        internal static IImportPIService Service
        {
            get { return (IImportPIService)Services.Factory.CreateService(typeof(IImportPIService)); }
        }
        #endregion
    }
    #endregion

    #region IImportPI interface
    public interface IImportPIService
    {
        ImportPI Get(int nImportPIID, Int64 nUserID);
        ImportPI Get(string sPurchaseContactNo, Int64 nUserID);
        List<ImportPI> GetsByImportPIType(string PCTypesIDs, Int64 nUserID);
        List<ImportPI> Gets(string sSQL, Int64 nUserID);
        List<ImportPI> Gets(int nSupplierID, Int64 nUserID);
        List<ImportPI> GetsImportPI(int nSupplierID, string sStatus, string sPCType, Int64 nUserID);
        string Delete(ImportPI oImportPI, Int64 nUserID);
        ImportPI Save(ImportPI oImportPI, Int64 nUserID);
        ImportPI ApproveImportPI(ImportPI oImportPI, Int64 nUserID);
        ImportPI RequestForReviseImportPI(ImportPI oImportPI, Int64 nUserID);
        ImportPI ReviseImportPI(ImportPI oImportPI, Int64 nUserID);
        List<ImportPI> GetsByLCID(int nLCID, Int64 nUserID);
        ImportPI UpdateAmount(ImportPI oImportInvoice, Int64 nUserID);
    }
    #endregion
}