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
    #region ImportLC
    public class ImportLC : BusinessObject
    {
        #region  Constructor
        public ImportLC()
        {
            ImportLCID = 0;
            AmendmentNo = 0;
            ContractorID = 0;
            Amount = 0;
            ReceiveAllInvoices = false;
            CCRate = 0;
            CurrencyID =0;
            LCCurrentStatus = EnumLCCurrentStatus.None;
            LCRequestDate = DateTime.Today;
            CoverNoteDate = DateTime.Today;
            ExpireDate = LCRequestDate.AddDays(90);
            ShipmentDate = LCRequestDate.AddDays(75);
            ErrorMessage = "";
            ImportPIType = EnumImportPIType.None;
            ImportLCDetails = new List<ImportLCDetail>();
            ImportLC_Clauses = new List<ImportLC_Clause>();
            Currency = "";
            BUID = 0;
            BUName = "";
            BUShortName = "";
            FileNo = "";
            ImportInvoices = new List<ImportInvoice>();
            LCTermsName = "";
            LCAppType = EnumLCAppType.None;
            LCMargin = 0;
            Tolerance = 0;
            BankAccountID = 0;
            IsConfirmation = false;
            AckmentRecDate = DateTime.Now;
            ForwardDate = DateTime.Now;
            LCAppTypeInt = 0;
            ImportLCLogID = 0;
            LCCurrentStatusInt = 0;
            LCTermID_Bene = 0;
            LCTermID = 0;
            LCTermsName_Bene = "";
            BBankRefNo = "";
        }
        #endregion

        #region Properties
        public int ImportLCID { get; set; }
        public int ImportLCLogID { get; set; }
        public int AmendmentNo { get; set; }
        public int BUID { get; set; }
        public string ImportLCNo { get; set; }
        public string BBankRefNo { get; set; }
        public DateTime ImportLCDate { get; set; }
        public int ContractorID { get; set; }
        public int BankBranchID_Nego { get; set; }
        public double Amount { get; set; }
        public DateTime ReceiveDate { get; set; }
        public int ReceivedBy { get; set; }
        public bool ReceiveAllInvoices { get; set; }
        public double CCRate { get; set; }
        public int CurrencyID { get; set; }
        public EnumLCPaymentType LCPaymentType { get; set; }
        public int LCPaymentTypeInt { get; set; }
        public EnumLCCurrentStatus LCCurrentStatus { get; set; }
        public int LCCurrentStatusInt { get; set; }
        public DateTime ExpireDate { get; set; }
        public DateTime ShipmentDate { get; set; }
        public string LCCoverNoteNo { get; set; }
        public DateTime CoverNoteDate { get; set; }
        public DateTime LCRequestDate { get; set; }
        public string LCANo { get; set; }
        public int InsuranceCompanyID { get; set; }
        public EnumImportPIType ImportPIType { get; set; }
        public string FileNo { get; set; }
        public double OverDuerate { get; set; }
        public bool IsLIBORrate { get; set; }
        public double Amount_Invoice { get; set; }
        public string BankName_Nego { get; set; }
        public string BBranchName_Nego { get; set; }
        public string BankAddress_Nego { get; set; }
        public string ContractorName { get; set; }
        public string InsuranceName { get; set; }
        public string ErrorMessage { get; set; }
        public bool IsTransShipmentAllow { get; set; }
        public bool IsPartShipmentAllow { get; set; }
        public int LCTermID { get; set; }
        public int LCTermID_Bene{ get; set; }
        public string LCTermsName { get; set; }
        public string LCTermsName_Bene { get; set; }
        public int PaymentInstructionInt { get; set; }
        public EnumShipmentBy ShipmentBy { get; set; }
        public string  BUName { get; set; }
        public string BUShortName { get; set; }
        public string ReceivedByUserName { get; set; }
        public string CurrencyName { get; set; }
        public string Currency { get; set; }
        public string InsuranceCompanyName { get; set; }
        public string InsuranceCompanyAddress { get; set; }
        public EnumLCAppType LCAppType { get; set; }
        public int LCAppTypeInt { get; set; }
        public EnumProductNature ProductType { get; set; }
        public double LCMargin { get; set; }
        public double Tolerance { get; set; }
        public int BankAccountID { get; set; }
        public bool IsConfirmation { get; set; }
        public EnumLCChargeType LCChargeType { get; set; }
        public int LCChargeTypeInt { get; set; }
        public DateTime AckmentRecDate { get; set; }
        public DateTime ForwardDate { get; set; }
        public List<ImportLCDetail> ImportLCDetails { get; set; }
        public List<ImportLCDetailProduct> ImportLCDetailProducts { get; set; }
        public List<BankBranch> BankBranchs { get; set; }
        public List<ImportLCClauseSetup> ImportLCClauseSetups { get; set; }
        public List<ImportLC_Clause> ImportLC_Clauses { get; set; }
        public List<ImportInvoice> ImportInvoices { get; set; }
        #endregion
        #region Derived
        public BusinessUnit BusinessUnit { get; set; }
        public string LCCurrentStatusInString
        {
            get
            {
                return LCCurrentStatus.ToString();
            }
          
        }
        public string LCChargeTypeSt
        {
            get
            {
                return EnumObject.jGet((EnumLCChargeType)this.LCChargeType).ToString();
            }

        }
        public string LCPaymentTypeSt
        {
            get
            {

                return EnumObject.jGet((EnumLCPaymentType)this.LCPaymentType).ToString();
            }
         
        }
        public string LCAppTypeSt
        {
            get
            {
                return EnumObject.jGet((EnumLCAppType)this.LCAppType).ToString() + "[" + EnumObject.jGet((EnumImportPIType)this.ImportPIType).ToString()+"]";
            }
        }
        public string PaymentInstructionInString
        {
            get
            {
                return EnumObject.jGet((EnumPaymentInstruction)this.PaymentInstructionInt).ToString();
            }

        }
       public string IsPartShipmentAllowInstring
        {
            get
            {
                if(this.IsPartShipmentAllow)
                {
                    return "Allowed";
                }else{
                    return "Not Allowed";
                }
            }
        }
        public string IsTransShipmentAllowInstring
       {
           get
           {
               if (this.IsTransShipmentAllow)
               {
                   return "Allowed";
               }
               else
               {
                   return "Not Allowed";
               }
           }
       }
        public string ShipmentByInString
       {
           get
           {
               return EnumObject.jGet((EnumShipmentBy)this.ShipmentBy).ToString();
           }
       }
        public string AmountSt
        {
            get
            {
                return this.Currency+""+Global.MillionFormat(this.Amount);
            }

        }
        public Double Balance
        {
            get
            {
                  return (this.Amount- this.Amount_Invoice);
            }

        }
      
        public string LCNo
        {
            get
            {
                if (this.ImportLCNo == "")
                {
                    return "Waiting For LC Open";
                }
                else
                {
                    return this.ImportLCNo;
                }
            }

        }
        public string ImportLCDateInString
        {

            get
            {
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
        public string ReceiveDateInString
        {

            get
            {
                if (this.ReceiveDate == DateTime.MinValue)
                {
                    return "";
                }
                else
                {
                    return this.ReceiveDate.ToString("dd MMM yyyy");
                }
            }

        }
        public string ExpireDateInString
        {

            get
            {
                return this.ExpireDate.ToString("dd MMM yyyy");
            }

        }
        public string ShipmentDateInString
        {

            get
            {
                return this.ShipmentDate.ToString("dd MMM yyyy");
            }

        }
       public string LCRequestDateInString
        {

            get
            {
                return this.LCRequestDate.ToString("dd MMM yyyy");
            }

        }
        public string CoverNoteDateInString
        {

            get
            {
                return this.CoverNoteDate.ToString("dd MMM yyyy");
            }

        }
        public string AckmentRecDateSt
        {

            get
            {
                if (this.AckmentRecDate == DateTime.MinValue)
                {
                    return "";
                }
                else
                {
                    return this.AckmentRecDate.ToString("dd MMM yyyy");
                }
            }

        }
        public string ForwardDateSt
        {

            get
            {
                if (this.ForwardDate == DateTime.MinValue)
                {
                    return "";
                }
                else
                {
                    return this.ForwardDate.ToString("dd MMM yyyy");
                }
            }

        }
        #endregion

        #region Functions
        public ImportLC Get(int nImportLCID, int nUserID)
        {
            return ImportLC.Service.Get(nImportLCID, nUserID);
        }
        public ImportLC GetLog(int nImportLCID, int nUserID)
        {
            return ImportLC.Service.GetLog(nImportLCID, nUserID);
        }
        public static List<ImportLC> Gets(string sSQL, int nUserID)
        {
            return ImportLC.Service.Gets(sSQL, nUserID);
        }
        public static List<ImportLC> GetsByStatus(string sLCCurrentStatus, int nBUID, int nUserID)
        {
            return ImportLC.Service.GetsByStatus(sLCCurrentStatus, nBUID, nUserID);
        }
        public ImportLC Save(int nUserID)
        {
            return ImportLC.Service.Save(this, nUserID);
        }
         public ImportLC SaveLog(int nUserID)
        {
            return ImportLC.Service.SaveLog(this, nUserID);
        }
       
      public string Delete( int nUserID)
        {
            return ImportLC.Service.Delete(this, nUserID);
        }
        public ImportLC UpdateForLCOpen(int nUserID)
        {
            return ImportLC.Service.UpdateForLCOpen(this, nUserID);
        }
        public ImportLC UpdateImportLC_FileNo(int nUserID)
        {
            return ImportLC.Service.UpdateImportLC_FileNo(this, nUserID);
        }
        public ImportLC RequestConfirm(int nUserID)
        {
            return ImportLC.Service.RequestConfirm(this, nUserID);
        }
        public ImportLC Save_UpdateStatus(int nUserID)
        {
            return ImportLC.Service.Save_UpdateStatus(this, nUserID);
        }
        #endregion

        #region ServiceFactory
        internal static IImportLCService Service
        {
            get { return (IImportLCService)Services.Factory.CreateService(typeof(IImportLCService)); }
        }
        #endregion

    }
    #endregion

    #region IImportLC interface
    public interface IImportLCService
    {
        #region 
        ImportLC Get(int id, Int64 nUserID);
        ImportLC GetLog(int nImportLCID, Int64 nUserID);
        List<ImportLC> GetsByStatus( string sLCCurrentStatus, int nBUID, Int64 nUserID);
        List<ImportLC> Gets(string sSQL, Int64 nUserID);
        string Delete(ImportLC oImportLC, Int64 nUserID);
        ImportLC Save(ImportLC oImportLC, Int64 nUserID);
        ImportLC SaveLog(ImportLC oImportLC, Int64 nUserID);
        ImportLC UpdateForLCOpen(ImportLC oImportLC, Int64 nUserID);
        ImportLC UpdateImportLC_FileNo(ImportLC oImportLC, Int64 nUserID);
        ImportLC RequestConfirm(ImportLC oImportLC, Int64 nUserID);
        ImportLC Save_UpdateStatus(ImportLC oImportLC, Int64 nUserID);
        #endregion
    }
    #endregion

}
