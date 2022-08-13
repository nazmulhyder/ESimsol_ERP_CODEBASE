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
    #region ImportLetterSetup
    [DataContract]
    public class ImportLetterSetup : BusinessObject
    {
        #region  Constructor
        public ImportLetterSetup()
        {
            ImportLetterSetupID = 0;

            LetterType = EnumImportLetterType.None;
            LetterTypeInt = (int)EnumImportLetterType.None;
            LetterName = "";
            IsPrintAddress = false;
            RefNo = "";
            To = "";
            ToName = "";
            Subject = "";
            DearSir = "";
            Body1 = "";
            Body2 = "";
            Body3 = "";
            ThankingOne = "";
            ThankingTwo = "";
            AdvicsBank = "";
            Authorize1 = "";
            Authorize2 = "";
            Authorize3 = "";
            SubjectTwo = "";
            Authorize1IsAuto = false;
            Authorize2IsAuto = false;
            Authorize3IsAuto = false;
            IsAutoRefNo = false;
            Activity = false;
            IsPrintSupplierAddress = false;
            IsPrintProductName = false;
            IsPrintPINo = false;
            IsPrinTnC = false;
            BUName = "";
            Remark = "";
            ErrorMessage = "";
            BUID = 0;
            Sequence = 0;
            ImportLetterSetups = new List<ImportLetterSetup>();
            LCAppTypeInt = 0;
            LCAppType = EnumLCAppType.None;
            LCPaymentTypeInt = 0;
            LCPaymentType = EnumLCPaymentType.None;
            ProductType = 0;
            SupplierName = "";
            LCNo = "";
            LCValue = "";
            Clause = "";
            PIBank = "";
            InvoiceNo = "Invoice No & Date";
            InvoiceValue = "Invoice Value";
            PIBankAddress = "BankAddress";
            HeaderType = 1;
            Origin = "";
            ContractorAddress = "";
            MasterLCNo = "Master LC No";
            MasterLCs = "";
            BLNo = "";
            IsCalMaturityDate = false;
        }
        #endregion

        #region Properties
        public int ImportLetterSetupID { get; set; }
        public int BUID { get; set; }
        public EnumImportLetterType LetterType { get; set; }
        public int LetterTypeInt { get; set; }
        public int IssueToType { get; set; }
        public string LetterName { get; set; }
        public bool IsPrintSupplierAddress { get; set; }
        public bool IsPrintProductName { get; set; }
        public bool IsPrintPINo { get; set; }
        public bool IsPrinTnC { get; set; }
        public bool IsPrintAddress { get; set; }
        public bool IsPrintDateCurrentDate { get; set; }
        public bool IsPrintDateObject { get; set; }
        public string LCPayType { get; set; }
        public bool IsAutoRefNo { get; set; }
        public string RefNo { get; set; }
        public string To { get; set; }
        public string ToName { get; set; }
        public string Subject { get; set; }
        public string SubjectTwo { get; set; }
        public string DearSir { get; set; }
        public string Body1 { get; set; }
        public string Body2 { get; set; }
        public string Body3 { get; set; }
        public string For { get; set; }
        public string ForName { get; set; }
        public string ThankingOne { get; set; }
        public string ThankingTwo { get; set; }
        public string AdvicsBank { get; set; }
        public string Authorize1 { get; set; }
        public string Authorize2 { get; set; }
        public string Authorize3 { get; set; }
        public bool Authorize1IsAuto { get; set; }
        public bool Authorize2IsAuto { get; set; }
        public bool Authorize3IsAuto { get; set; }
        public bool Activity { get; set; }
        public string BUName { get; set; }
        public string Remark { get; set; }
        public int ProductType { get; set; }
        public int Sequence { get; set; }
        public List<ImportLetterSetup> ImportLetterSetups { get; set; }
        public string ErrorMessage { get; set; }
        public string ActivityInSt
        {
            get
            {
                if (this.Activity == true) return "Active";
                else if (this.Activity == false) return "Inactive";
                else return "-";
            }
        }
        public string LetterTypeSt
        {
            get
            {
                return this.LetterType.ToString();
            }
        }
        public string IssueToTypeSt
        {
            get
            {
                return ((EnumImportLetterIssueTo)this.IssueToType).ToString();
            }
        }
        public string SupplierName { get; set; }
       
        public string PIBank { get; set; }
        public string PIBankAddress { get; set; }
        public string BLNo { get; set; }
        public string LCNo { get; set; }
        public string MasterLCNo { get; set; }
        public string LCValue { get; set; }
        public string InvoiceNo { get; set; }
        public string InvoiceValue { get; set; }
        public int HeaderType { get; set; }
        public string Clause { get; set; }
        public EnumLCAppType LCAppType { get; set; }
        public int LCAppTypeInt { get; set; }
        public EnumLCPaymentType LCPaymentType { get; set; }
        public int LCPaymentTypeInt { get; set; }
        public int BankBranchID { get; set; }
        public bool IsPrintPIBankAddress { get; set; }
        public bool IsPrintMaturityDate { get; set; }
        public bool IsCalMaturityDate { get; set; }
      
        #region Carry Property
        public string ContractorName { get; set; }
        public string ContractorAddress { get; set; }
        public string Address_Supplier { get; set; }
        public string Origin { get; set; }
        public string MasterLCs { get; set; }
        #endregion
        #endregion

        #region Functions
        public ImportLetterSetup Get(int nId, Int64 nUserID)
         {
             return ImportLetterSetup.Service.Get(nId, nUserID);
         }
         public ImportLetterSetup Get(int nLetterType, int nIssueToType, int nBUID, int nImportLCID, string sSQL, Int64 nUserID)
         {
             return ImportLetterSetup.Service.Get(nLetterType, nIssueToType, nBUID, nImportLCID, sSQL, nUserID);
         }
         public ImportLetterSetup GetForIPR(int nLetterType, int nIssueToType, int nBUID, int nIPRID, string sSQL, Int64 nUserID)
         {
             return ImportLetterSetup.Service.GetForIPR(nLetterType, nIssueToType, nBUID, nIPRID, sSQL, nUserID);
         }
         public ImportLetterSetup GetBy(int nLetterType, int nIssueToType, int nBUID, int nImportLCID, string sSQL, Int64 nUserID)
         {
             return ImportLetterSetup.Service.GetBy(nLetterType, nIssueToType, nBUID, nImportLCID, sSQL, nUserID);
         }
         public ImportLetterSetup Save(Int64 nUserID)
         {
             return ImportLetterSetup.Service.Save(this, nUserID);
         }
         public ImportLetterSetup Get(int nLetterType, int nIssueToType, int nImportLCID, string sSQL, Int64 nUserID)
         {
             return ImportLetterSetup.Service.Get(nLetterType, nIssueToType, nImportLCID, sSQL, nUserID);
         }
         public string Delete( Int64 nUserID)
         {
             return ImportLetterSetup.Service.Delete(this, nUserID);
         }         
         public static List<ImportLetterSetup> Gets(Int64 nUserID)
         {
             return ImportLetterSetup.Service.Gets(nUserID);
         }
         public static List<ImportLetterSetup> UpdateSequence(ImportLetterSetup oImportLetterSetup, Int64 nUserID)
         {
             return ImportLetterSetup.Service.UpdateSequence(oImportLetterSetup, nUserID);
         }
         public static List<ImportLetterSetup> Gets(string sSQL, Int64 nUserID)
         {
             return ImportLetterSetup.Service.Gets(sSQL,nUserID);
         }
         public static List<ImportLetterSetup> Gets(bool bActivity, int nBUID, Int64 nUserID)
         {
             return ImportLetterSetup.Service.Gets(bActivity, nBUID, nUserID);
         }

         public static List<ImportLetterSetup> BUWiseGets( int nBUID, Int64 nUserID)
         {
             return ImportLetterSetup.Service.BUWiseGets( nBUID, nUserID);
         }

        public ImportLetterSetup Activate(ImportLetterSetup oImportLetterSetup,Int64 nUserID)
        {
            return ImportLetterSetup.Service.Activate(this, nUserID);
        }
        #endregion
        #region ServiceFactory
        internal static IImportLetterSetupService Service
        {
            get { return (IImportLetterSetupService)Services.Factory.CreateService(typeof(IImportLetterSetupService)); }
        }
        #endregion
    }
    #endregion

    
    #region IImportLetterSetup interface
    [ServiceContract]
    public interface IImportLetterSetupService
    {
    
        ImportLetterSetup Get(int nID, Int64 nUserID);
        ImportLetterSetup Get(int nLetterType, int nIssueToType, int nBUID, int nImportLCID, string sSQL, Int64 nUserID);
        ImportLetterSetup Get(int nLetterType, int nIssueToType, int nImportLCID, string sSQL, Int64 nUserID);
        ImportLetterSetup GetBy(int nLetterType, int nIssueToType, int nBUID, int nImportLCID, string sSQL, Int64 nUserID);
        ImportLetterSetup GetForIPR(int nLetterType, int nIssueToType, int nBUID, int nIPRID, string sSQL, Int64 nUserID);
        List<ImportLetterSetup> Gets( Int64 nUserID);
        List<ImportLetterSetup> UpdateSequence(ImportLetterSetup oImportLetterSetup, Int64 nUserID);
        List<ImportLetterSetup> Gets(string sSQL, Int64 nUserID);        
        List<ImportLetterSetup> Gets(bool bActivity, int BUID, Int64 nUserID);
        List<ImportLetterSetup> BUWiseGets(int BUID, Int64 nUserID);
        ImportLetterSetup Save(ImportLetterSetup oImportLetterSetup, Int64 nUserID);
        string Delete(ImportLetterSetup oImportLetterSetup, Int64 nUserID);
        ImportLetterSetup Activate(ImportLetterSetup oImportLetterSetup, Int64 nUserID);
    }
    #endregion

}
