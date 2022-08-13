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
    #region ImportInvoice
    public class ImportInvoice : BusinessObject
    {
        #region  Constructor
        public ImportInvoice()
        {
            ImportInvoiceID = 0;
            FileNo = "";
            ImportInvoiceNo = "";
            DateofInvoice = DateTime.Now;
            InvoiceType = EnumImportPIType.None;
            ImportLCID = 0;
            ContractorID = 0;
            DateofReceive = DateTime.Now;
            Amount = 0;
            CurrencyID = 0;
            DateofBankInfo = DateTime.MinValue;
            DateOfTakeOutDoc = DateTime.MinValue;
            DateofApplication = DateTime.MinValue;
            DateofNegotiation = DateTime.MinValue;
            DateofAcceptance = DateTime.Now;
            DateofMaturity = DateTime.MinValue;
            Remark_Pament = "";
            AcceptedBy = 0;
            DateofPayment = DateTime.Now;
            ExceedDays = 0;
            AdjustmentValue = 0;
            BankStatus = EnumInvoiceBankStatus.None;
            InvoiceStatus = EnumInvoiceEvent.None;
            InvoiceStatusInt = 0;
            ProductionApprovalStatus = EnumApprovalStatus.None;
            LCCurrentStatus = EnumLCCurrentStatus.None;
            ContractorName = "";
            ImportLCNo = "";
            ImportLCDate = DateTime.Now;
            BankName_Nego = "";
            BankBranchID_Nego = 0;
            Qty = 0;
            BLNo = "";
            BLDate = DateTime.Now;
            DeliveryNoticeDate = DateTime.MinValue;
            ETADate = DateTime.Now;
            BUName = "";
            BUCode = "";
            BaseCurrencyID = 0;
            Currency = "";
            ABPNo = "";
            MUnit = "";
            ShipmentDate = DateTime.Now;
            AcceptedByName = "";
            //CnFID = 0;
            CRate_Acceptance = 0;
            //CnFRemarks = "";
            PositionOuterDate = DateTime.Now;
            PositionJTDate = DateTime.Now;
            AssesmentDate = DateTime.Now;
            AssesmentRemarks = "";
            NotingDate = DateTime.Now;
            NotingRemarks = "";
            ExamineDate = DateTime.Now;
            ExamineRemarks = "";
            DOReceiveFromDate = DateTime.Now;
            DOReceiveFromRemarks = "";
            BillofEntryDate = DateTime.MinValue;
            BillofEntryNo = "";
            CommonDate = DateTime.Now;
            CommonRemarks = "";
            Origin = "";
            ImportInvoiceDetails = new List<ImportInvoiceDetail>();
            ImportCnf=new ImportCnf();
            CurrencyID_LC = 0;
            LCPaymentTypeInt = 0;
            LCTermsName_Bene = "";
            CommissionInPercent = 0;
            Commission =0;
            CommissionRemarks = "";
            PaymentExist = false;
        }

        #endregion

        #region Properties
        public int ImportInvoiceID { get; set; }
        public string ImportInvoiceNo { get; set; }
        public string FileNo { get; set; }
        public DateTime DateofInvoice { get; set; }
        public EnumImportPIType InvoiceType { get; set; }
        public int ImportLCID { get; set; }
        public int ContractorID { get; set; }
        public DateTime DateofReceive { get; set; }
        public double Amount { get; set; }
        public double Amount_LC { get; set; }
        public int CurrencyID { get; set; }
        public DateTime DateofBankInfo { get; set; }
        public DateTime DateOfTakeOutDoc { get; set; }
        public DateTime DateofApplication { get; set; }
        public DateTime DateofNegotiation { get; set; }
        public DateTime DateofAcceptance { get; set; }
        public DateTime DateofMaturity { get; set; }
        public string CommonRemarks { get; set; }
        public DateTime CommonDate { get; set; }
        public string ABPNo { get; set; }
        public string Remark_Pament { get; set; }
        public int AcceptedBy { get; set; }
        public DateTime DateofPayment { get; set; }
        public DateTime ShipmentDate { get; set; }
        public int ExceedDays { get; set; }
        public string AcceptedByName { get; set; }
        public int AdjustmentValue { get; set; }
        public int InvoiceTypeInt { get; set; }
        public EnumInvoiceBankStatus BankStatus { get; set; }
        public string MUnit { get; set; }
        public int BankStatusInt { get; set; }
        public double CommissionInPercent {get;set;}
        public double Commission  {get;set;}
        public string CommissionRemarks { get; set; }
        public bool PaymentExist { get; set; }
        public EnumInvoiceEvent InvoiceStatus { get; set; }
        public int InvoiceStatusInt { get; set; }
        public EnumApprovalStatus ProductionApprovalStatus { get; set; }//Enum ???

        public string ImportPITypeInString { get; set; }
        public int ImportPIID { get; set; }
        public int BaseCurrencyID { get; set; }
        //public string CnFRemarks { get; set; }
        public int CurrencyID_LC { get; set; }
        //public string CnFName { get; set; }
        public DateTime PositionOuterDate { get; set; }
        public DateTime PositionJTDate { get; set; }
        public DateTime AssesmentDate { get; set; }
        public string AssesmentRemarks { get; set; }
        public DateTime NotingDate { get; set; }
        public string NotingRemarks { get; set; }
        public DateTime ExamineDate { get; set; }
        public string ExamineRemarks { get; set; }
        public DateTime DOReceiveFromDate { get; set; }
        public string DOReceiveFromRemarks { get; set; }
        public string BillofEntryNo { get; set; }
        public DateTime BillofEntryDate { get; set; }
       
        //public string GoodsInTransitRemarks { get; set; }
        //public string VehicleInfo { get; set; }
        //public string DriverCotractNumber { get; set; }

        public string Remarks { get; set; }
        public string ErrorMessage { get; set; }

        public double TotalInvoiceValue { get; set; }
        #region Properties for Import Invoice management
        public string ImportLCNo { get; set; }        
        public string LCPaymentType { get; set; } //EnumLCPaymentType
        public int LCPaymentTypeInt { get; set; }
        public DateTime ImportLCDate { get; set; }
        public string BankName_Nego { get; set; }
        public string BBranchName_Nego { get; set; }
        public string BankAddress_Nego { get; set; }
        public string ContractorName { get; set; }
        public string Address_Con { get; set; }
        public int BankBranchID_Nego { get; set; }
        public string BLNo { get; set; }
        public string Origin { get; set; }
        public DateTime BLDate { get; set; }
        public DateTime DeliveryNoticeDate { get; set; }

        public DateTime ETADate { get; set; }
        public string BUName { get; set; }
        public string BUCode { get; set; }
        public int BUID { get; set; }
        public string LCTermsName { get; set; }
        public string LCTermsName_Bene { get; set; }
      
        #endregion
        


        #region Derive property     
        public ImportBL ImportBL { get; set; }
        public ImportCnf ImportCnf { get; set; }
        public double Qty { get; set; }
        public double CCRate { get; set; }
        public double CRate_Acceptance { get; set; }
        public int Tenor { get; set; }
        public int PaymentInstructionType { get; set; }
        public string Currency { get; set; }
        public string LiabilityNo { get; set; }
        public EnumLiabilityType LiabilityType { get; set; }
        public int LiabilityTypeInt { get; set; }
        public List<ImportInvoiceDetail> ImportInvoiceDetails { get; set; }
        public ImportInvoiceHistory ImportInvoiceHistory { get; set; }
        //public PurchaseInvoicePayment PurchaseInvoicePayment { get; set; }
        public EnumLCCurrentStatus LCCurrentStatus { get; set; }

        public string sPCTypes { get; set; }
        public string sPaymentTypes { get; set; }
        public List<Currency> Currencys { get; set; }    
        public Company Company { get; set; }
        public string sLCStatus { get; set; }
        public string sCurrentStatus { get; set; }
        public string sBankStatus { get; set; }
      

        #endregion

        public string AmountSt
        {
            get
            {
                return this.Currency + " " + Global.MillionFormat(this.Amount);
            }
        }
        
        public string AmountLCInSt
        {
            get
            {
                return this.Currency + " " + Global.MillionFormat(this.Amount_LC);
            }
        }
        public string DateofPaymentSt
        {
            get
            {
                if (this.DateofPayment==DateTime.MinValue)
                {
                    return "-";
                }
                else
                {
                    return DateofPayment.ToString("dd MMM yyyy");
                }
            }
        }
        public string DateofMaturityST
        {
            get
            {
              if(this.DateofMaturity==DateTime.MinValue)
              {
                  return " ";
              }
              else
              {
                  return DateofMaturity.ToString("dd MMM yyyy");
              }
                
            }
        }
        public string DateofNegotiationInST
        {
            get
            {
             if(this.DateofNegotiation==DateTime.MinValue)
             {
                 return "-";
             }
             else
             {
                 return DateofNegotiation.ToString("dd MMM yyyy");
             }
            }
        }
        public string DateofReceiveInString
        {
            get
            {
                return DateofReceive.ToString("dd MMM yyyy");
            }
        }
        public string DateofBankInfoSt
        {
            get
            {
                if (this.DateofBankInfo == DateTime.MinValue)
                {
                    return "";
                }
                else
                {
                    return DateofBankInfo.ToString("dd MMM yyyy");
                }

            }
        }
        public string DateOfTakeOutDocSt
        {
            get
            {
                if (this.DateOfTakeOutDoc == DateTime.MinValue)
                {
                    return "";
                }
                else
                {
                    return DateOfTakeOutDoc.ToString("dd MMM yyyy");
                }

            }
        }
        public string ShipmentDateInString
        {
            get{
                return this.ShipmentDate.ToString("dd MMM yyyy");
            }
        }

        public string PositionOuterDateSt
        {
            get
            {
                 if(this.PositionOuterDate==DateTime.MinValue)
                 {
                     return "";
                 }
                 else
                 {
                     return this.PositionOuterDate.ToString("dd MMM yyyy");
                 }
            }
        }
        public string PositionJTDateSt
        {
            get
            {
                if (this.PositionJTDate == DateTime.MinValue)
                {
                    return "";
                }
                else
                {
                    return this.PositionJTDate.ToString("dd MMM yyyy");
                }
            }
        }
        public string AssesmentDateInString
        {
            get
            {
                if (this.AssesmentDate == DateTime.MinValue)
                {
                    return "";
                }
                else
                {
                    return this.AssesmentDate.ToString("dd MMM yyyy");
                }
            }
        }
        public string NotingDateInString
        {
            get
            {
                if (this.NotingDate == DateTime.MinValue)
                {
                    return "";
                }
                else
                {
                    return this.NotingDate.ToString("dd MMM yyyy");
                }
            }
        }

        public string ExamineDateInString
        {
            get
            {
                if (this.ExamineDate == DateTime.MinValue)
                {
                    return "";
                }
                else
                {
                    return this.ExamineDate.ToString("dd MMM yyyy");
                }
            }
        }

        public string DOReceiveFromDateInString
        {
            get
            {
                if (this.DOReceiveFromDate == DateTime.MinValue)
                {
                    return "";
                }
                else
                {
                    return this.DOReceiveFromDate.ToString("dd MMM yyyy");
                }
            }
        }
        public string BillofEntryDateSt
        {
            get
            {
                if (this.BillofEntryDate == DateTime.MinValue)
                {
                    return "";
                }
                else
                {
                    return this.BillofEntryDate.ToString("dd MMM yyyy");
                }
            }
        }
        public string DateofAcceptanceSt
        {
            get
            {
                if (this.DateofAcceptance == DateTime.MinValue)
                {
                    return "-";
                }
                else
                {
                    return DateofAcceptance.ToString("dd MMM yyyy");
                }

            }
        }

      
        public string ImportLCDateInString
        {
            get
            {
                //return ImportLCDate.ToString("dd MMM yyyy");
                if (this.ImportLCDate == DateTime.MinValue)
                {
                    return "";
                }
                else
                {
                    return this.ImportLCDate.ToString("dd MMM yyyy");
                }

            }
        }
        public string LiabilityTypeSt
        {
            get
            {
                if (this.LiabilityType == EnumLiabilityType.None)
                {
                    return "-";
                }
                else
                {
                    return EnumObject.jGet(this.LiabilityType);
                }
            }
        }
        public string BLDateSt
        {
            get
            {
                if (this.BLDate == DateTime.MinValue || this.BLDate.ToString("dd MMM yyyy") == "01 Jan 1900")
                {
                    return "";
                }
                else
                {
                    return this.BLDate.ToString("dd MMM yyyy");
                }
               
            }
        }
        public string DeliveryNoticeDateSt
        {
            get
            {
                if (this.DeliveryNoticeDate==DateTime.MinValue)
                {
                    return "";
                }
                else
                {
                    return this.DeliveryNoticeDate.ToString("dd MMM yyyy");
                }

            }
        }
        public string ETADateSt
        {
            get
            {
                if (this.ETADate == DateTime.MinValue)
                {
                    return "";
                }
                else
                {
                    return this.ETADate.ToString("dd MMM yyyy");
                }

            }
           
        }

        public string CurrentStatusInSt
        {
            get
            {
                return EnumObject.jGet(this.InvoiceStatus);
            }
        }

        public string InvoiceTypeSt
        {
            get
            {
                return EnumObject.jGet(this.InvoiceType);
            }
        }

        public string BankStatusInSt
        {
            get
            {
                return EnumObject.jGet(this.BankStatus);
            }
        }

        public string InvoiceDateInString
        {

            get
            {
                if (this.DateofInvoice == DateTime.MinValue)
                {
                    return "";
                }
                else
                {
                    return this.DateofInvoice.ToString("dd MMM yyyy");
                }
            }

        }

        public string ReceiveDateInString
        {

            get
            {
                if (this.DateofReceive == DateTime.MinValue)
                {
                    return "";
                }
                else
                {
                    return this.DateofReceive.ToString("dd MMM yyyy");
                }
            }

        }
        public string LCCurrentStatusSt
        {
            get
            {
                return LCCurrentStatus.ToString();
            }

        }
        #endregion
        #region Derived Properties
        public string LCDescription
        {
            get
            {
                return "At "+this.Tenor+" Days "+this.LCTermsName+" From "+ ((EnumPaymentInstruction)this.PaymentInstructionType).ToString();
            }
        }
    
        #endregion

        #region Function New Version
        public ImportInvoice Save(int nUserID)
        {
            return ImportInvoice.Service.Save(this, nUserID);
        }
        public ImportInvoice SavePIHistory(int nUserID)
        {
            return ImportInvoice.Service.SavePIHistory(this, nUserID);
        }
        public ImportInvoice Save_UpdateStatus(int nUserID)
        {
            return ImportInvoice.Service.Save_UpdateStatus(this, nUserID);
        }
        public ImportInvoice SaveAccptanceVoucher(int nUserID)
        {
            return ImportInvoice.Service.SaveAccptanceVoucher(this, nUserID);
        }
        public ImportInvoice UpdateCommission(int nUserID)
        {
            return ImportInvoice.Service.UpdateCommission(this, nUserID);
        }
        public bool SaveDeliveryNotice(ImportInvoice oImportInvoice, int nUserID)
        {
            return ImportInvoice.Service.SaveDeliveryNotice(oImportInvoice, nUserID);
        }
        public string Delete(int nUserID)
        {
            return ImportInvoice.Service.Delete(this, nUserID);
        }

        public ImportInvoice Get(int nImportInvoiceID, int nUserID)
        {
            return ImportInvoice.Service.Get(nImportInvoiceID, nUserID);
        }
        public ImportInvoice Get(int nInvoiceType, int nImportPIID, int nUserID)
        {
            return ImportInvoice.Service.Get( nInvoiceType,  nImportPIID, nUserID);
        }
        public static List<ImportInvoice> Gets(int nImportLCID, int nUserID)
        {
            return ImportInvoice.Service.Gets(nImportLCID, nUserID);
        }
        public static List<ImportInvoice> Gets(int nUserID)
        {
            return ImportInvoice.Service.Gets(nUserID);
        }
        public static List<ImportInvoice> Gets(string sSQL, int nUserID)
        {
            return ImportInvoice.Service.Gets(sSQL, nUserID);
        }
        public ImportInvoice UpdateAmount(long nUserID)
        {
            return ImportInvoice.Service.UpdateAmount(this, nUserID);
        }

        #endregion


        #region ServiceFactory
        internal static IImportInvoiceService Service
        {
            get { return (IImportInvoiceService)Services.Factory.CreateService(typeof(IImportInvoiceService)); }
        }
        #endregion


    }
    #endregion

    #region IImportInvoice interface
    public interface IImportInvoiceService
    {
        List<ImportInvoice> Gets(Int64 nUserID);
        string Delete(ImportInvoice oImportLC, Int64 nUserID);
        ImportInvoice Save(ImportInvoice oImportInvoice, Int64 nUserID);
        ImportInvoice SavePIHistory(ImportInvoice oImportInvoice, Int64 nUserID);
        ImportInvoice Save_UpdateStatus(ImportInvoice oImportInvoice, Int64 nUserID);
        ImportInvoice SaveAccptanceVoucher(ImportInvoice oImportInvoice, Int64 nUserID);
        ImportInvoice UpdateCommission(ImportInvoice oImportInvoice, Int64 nUserID);
        ImportInvoice UpdateAmount(ImportInvoice oImportInvoice, Int64 nUserID);
        ImportInvoice Get(int nImportInvoiceID, Int64 nUserID);
        ImportInvoice Get(int nInvoiceType, int nImportPIID, Int64 nUserID);
        List<ImportInvoice> Gets(int nImportLCID, Int64 nUserID);
        List<ImportInvoice> Gets(string sSQL, Int64 nUserID);
        bool SaveDeliveryNotice(ImportInvoice oImportInvoice, Int64 nUserID);
    

    }
    #endregion
}
