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
    #region ExportLC
    [DataContract]
    public class ExportLC : BusinessObject
    {
        public ExportLC()
        {
            ExportLCID = 0;
            ExportLCNo = "";
            OpeningDate = new DateTime(1900, 01, 01);
            BBranchID_Advice = 0;
            BankBranchID_Forwarding = 0;
            ApplicantID = 0;
            ContactPersonalID = 0;
            DeliveryToID = 0;
            Amount = 0;
            CurrencyID = 2;
            ShipmentDate = new DateTime(1900, 01, 01);
            ShipmentClause = "";
            ExpiryDate = new DateTime(1900, 01, 01);
            ExpiryClause = "";
            AtSightDiffered = false;
            ShipmentFrom = "";
            PartialShipmentAllowed = false;
            TransShipmentAllowed = false;
            RefTypeInInt = 0;
            //BankAuthorisedOneID = 0;
            //BankAuthorisedTwoID = 0;
            CurrentStatus = EnumExportLCStatus.FreshLC;
            CurrentStatusInInt = (int)EnumExportLCStatus.None;
            FileNo = "";
            Remark = "";
            BBranchID_Issue = 0;
            LiborRate = true;
            BBankFDD = true;
            PartialShipmentAllowed=true;
            TransShipmentAllowed = true;
            OverDueRate = 0;
            OverDuePeriod = "";
            VersionNo = 0;
            LCRecivedDate = new DateTime(1900, 01, 01);
            IsForeignBank = false;
            NoteUD = "";
            NoteQuery = "";
            FrightPrepaid = "";
            DarkMedium = "";
            ExportLCLogID = 0;
            Year = DateTime.Today.ToString("yy");
            GetOriginalCopy = false;
            DCharge = 0;
            BBranchID_Nego = 0;
            Stability = false;
            AuthorizeBy = DateTime.Now;
            DBServerUserID = 0;
            DBServerDate = DateTime.Now;
            ApplicantName = "";
            DeliveryToName = "";
            ApplicantAddress = "";
            SLNo = 0;
            BankName_Nego = "";
            AuthorizedDate = DateTime.Now;
            MKTPerson = "";
            ErrorMessage = "";
            BUID = 0;
            GarmentsQty = "";
            NegoDays = 0;
            HSCode = "";
            AreaCode = "";
            AmendmentDate = DateTime.Now;
            BankName_Advice = "";
            IsRecDateSearch = false;
            FactoryAddress = "";
            PaymentInstruction = EnumPaymentInstruction.None;
            MasterLCNos = "";
            MasterLCDates = "";
            HaveQuery = true;
            AmountBill = 0;
            ExportLCType = EnumExportLCType.LC;
            ExportLCTypeInt = (int)EnumExportLCType.LC;
        }

        #region Properties
        public int ExportLCID { get; set; }
        public string ExportLCNo { get; set; }
        public DateTime OpeningDate { get; set; }
        public int BBranchID_Advice { get; set; }
        public int BankBranchID_Forwarding { get; set; }
        public int BBranchID_Issue { get; set; }
        public int BBranchID_Nego { get; set; }
        public int ApplicantID { get; set; }
        public int ContactPersonalID { get; set; }
        public int DeliveryToID { get; set; }
        public double Amount { get; set; }
        public int CurrencyID { get; set; }
        public int RefTypeInInt { get; set; }
        public DateTime ShipmentDate { get; set; }
        public string ShipmentClause { get; set; }
        public DateTime ExpiryDate { get; set; }
        public string ExpiryClause { get; set; }
        public EnumExportLCStatus CurrentStatus { get; set; }
        public EnumExportLCType ExportLCType { get; set; }
        public int ExportLCTypeInt { get; set; }
        public int CurrentStatusInInt { get; set; }
        public string FileNo { get; set; }
        public string Remark { get; set; }
        public int VersionNo { get; set; }
        public DateTime LCRecivedDate { get; set; }
        public bool LiborRate { get; set; }
        public bool BBankFDD { get; set; }
        public double OverDueRate { get; set; }
        public string OverDuePeriod { get; set; }
        public int LCTramsID { get; set; }
        public bool IsForeignBank { get; set; }
        public string NoteUD { get; set; }
        public string NoteQuery   { get; set; }
        public string FrightPrepaid { get; set; }
        public string DarkMedium { get; set; }
        public bool AtSightDiffered { get; set; }
        public bool HaveQuery { get; set; }
        public string ShipmentFrom { get; set; }
        public bool PartialShipmentAllowed { get; set; }
        public bool TransShipmentAllowed { get; set; }
        public int ExportLCLogID { get; set; }
        public string Year { get; set; }
        public bool GetOriginalCopy { get; set; }
        public double DCharge { get; set; }
        public bool Stability { get; set; }
        public int BUID { get; set; }
        public EnumPaymentInstruction PaymentInstruction { get; set; }
        public DateTime AuthorizeBy { get; set; }
        public DateTime AuthorizedDate { get; set; }
        public int DBServerUserID { get; set; }
        public DateTime DBServerDate { get; set; }
        public int NegoDays { get; set; }
        public string HSCode { get; set; }
        public string AreaCode { get; set; }
        public string ApplicantName { get; set;}
        public string DeliveryToName { get; set; }
        public string ApplicantAddress { get; set; }
        public string GarmentsQty { get; set; }
        public string ContactPersonnelName { get; set; }
        public string CurrencyName { get; set; }
        public string LCTermsName { get; set; }
        public string Currency { get; set; }
        public string MKTPerson { get; set; }
        public string ErrorMessage { get; set; }
        public DateTime AmendmentDate { get; set; }
        public string BBranchName_Issue { get; set; }
        public string BankName_Issue { get; set; }
        public string BankName_Advice { get; set; }
        public string BBranchName_Advice { get; set; }
        public string BankName_Nego { get; set; }
        public string BankBranchName_Nego { get; set; }
        public bool IsRecDateSearch { get; set; }
        public bool IsAmendment { get; set; } // Carry Field For execute Amendment Log
        public string FactoryAddress { get; set; }
        public int MasterLCID { get; set; }
        //public string MasterLCDates { get; set; }
        public string Params { get; set; }
        #endregion

        #region Derived Property
        public int SLNo { get; set; }
        public string MasterLCNos { get; set; }
        public string MasterLCDates { get; set; }
        public double AmountBill { get; set; }
        public List<ExportPILCMapping> ExportPILCMappings { get; set; }
        public List<MasterLCMapping> MasterLCMappings { get; set; }
            
        //public List<LCTerm> LCTerms { get; set; }
        public List<BankBranch> BankBranchs { get; set; }
        
        public string FileNoYear
        {
            get { return this.FileNo + "/" + this.Year; }

        }
        public string AmendmentDateSt
        {
            get
            {
                
                if (this.AmendmentDate == DateTime.MinValue)
                {
                    return "-";
                }
                else
                {
                    return AmendmentDate.ToString("dd MMM yyyy");
                }
            }
        }
       
      
        public string LCRecivedDateST
        {
            get
            {
                DateTime MinValue = new DateTime(1900, 01, 01);
                if (this.LCRecivedDate == MinValue)
                {
                    return "-";
                }
                else
                {
                    return LCRecivedDate.ToString("dd MMM yyyy");
                }
            }
        }
        public string CurrentStatusInST
        {
            get
            {
                return EnumObject.jGet(this.CurrentStatus);
            }
        }
        public string OpeningDateST
        {
            get
            {
                DateTime MinValue = new DateTime(1900, 01, 01);
                if (this.OpeningDate == MinValue)
                {
                    return "-";
                }
                else
                {
                    return OpeningDate.ToString("dd MMM yyyy");
                }
            }
        }
        public string ExpiryDateST
        {
            get
            {
                DateTime MinValue = new DateTime(1900, 01, 01);
                if (this.ExpiryDate == MinValue)
                {
                    return "-";
                }
                else
                {
                    return ExpiryDate.ToString("dd MMM yyyy");
                }
            }
        }
        public string ShipmentDateST
        {
            get
            {
                DateTime MinValue = new DateTime(1900, 01, 01);
                if (this.ShipmentDate == MinValue)
                {
                    return "-";
                }
                else
                {
                    return ShipmentDate.ToString("dd MMM yyyy");
                }
            }
        }
        public string AmountSt
        {
            get
            {
                if (this.Amount < 0)
                {
                    return this.Currency + " (" + Global.MillionFormat(this.Amount * (-1)) + ")";
                }
                else
                {
                    return this.Currency + " " + Global.MillionFormat(this.Amount);
                }
            }
        }
        private string sGetOrginalCopy = "";
        public string GetOrginalCopySt
        {
            get
            {
                if (this.GetOriginalCopy)
                {
                    sGetOrginalCopy = "Yes";
                }
                else
                {
                    sGetOrginalCopy = "No";
                }

                return sGetOrginalCopy;
            }
        }
        public string BalanceSt
        {
            get
            {
                if (this.Amount < 0)
                {
                    return this.Currency + " (" + Global.MillionFormat((this.Amount - this.AmountBill) * (-1)) + ")";
                }
                else
                {
                    return this.Currency + " " + Global.MillionFormat(this.Amount - this.AmountBill);
                }
            }
        }
       
        #region AmendmentFullNo
        private string _sAmendmentFullNo;
        public string AmendmentFullNo
        {
            get
            {
                if (this.VersionNo == 0) return "";

                _sAmendmentFullNo = " A-" + this.VersionNo;
                return _sAmendmentFullNo;
            }
        }
        #endregion        
        public string ExportLCTypeSt
        {
            get
            {
                return this.ExportLCType.ToString();

            }
        }
       // for report
      
        #endregion

        #region Non DB function
      
        #endregion

        #region Functions
        public static List<ExportLC> Gets(int nBUID,DateTime dLCDate,Int64 nUserID)
        {
            return ExportLC.Service.Gets( nBUID,dLCDate,nUserID);
        }
        public static List<ExportLC> Gets(int nBUID, Int64 nUserID)
        {
            return ExportLC.Service.Gets(nBUID,nUserID);
        }
        public static List<ExportLC> GetsLog(int nExportLCID, Int64 nUserID)
        {
            return ExportLC.Service.GetsLog(nExportLCID,nUserID);
        }

        public static List<ExportLC> Gets(string sSQL, Int64 nUserID)
        {
            return ExportLC.Service.Gets(sSQL,nUserID);
        }

        public static List<ExportLC> GetsSQL(string sSQL, Int64 nUserID)
        {
            return ExportLC.Service.GetsSQL(sSQL, nUserID);
        }
        public ExportLC Get(int id, Int64 nUserID)
        {
            return ExportLC.Service.Get(id, nUserID);
        }
        public ExportLC GetLog(int id, Int64 nUserID)
        {
            return ExportLC.Service.GetLog(id, nUserID);
        }
        public ExportLC GetByNo(int nBUID,string sExportLCNo, Int64 nUserID)
        {
            return ExportLC.Service.GetByNo( nBUID,sExportLCNo, nUserID);
        }

        public ExportLC Save(Int64 nUserID)
        {
            return ExportLC.Service.Save(this, nUserID);
        }
        public ExportLC SaveMLC(Int64 nUserID)
        {
            return ExportLC.Service.SaveMLC(this, nUserID);
        }
        public ExportLC SaveLog(Int64 nUserID)
        {
            return ExportLC.Service.SaveLog(this, nUserID);
        }
    
        public ExportLC Approved(Int64 nUserID)
        {
            return ExportLC.Service.Approved(this, nUserID);
        }
        public ExportLC UpdateExportLCStatus(Int64 nUserID)
        {
            return ExportLC.Service.UpdateExportLCStatus(this, nUserID);
        }
        public ExportLC UpdateForGetOrginalCopy(int nExportLCID,Int64 nUserID)
        {
            return ExportLC.Service.UpdateForGetOrginalCopy(nExportLCID, nUserID);
        }
        public ExportLC UpdateUDInfo( int nOperation,Int64 nUserID)
        {
            return ExportLC.Service.UpdateUDInfo(this,nOperation, nUserID);
        }
        public string Delete(Int64 nUserID)
        {
            return ExportLC.Service.Delete(this, nUserID);
        }

        public ExportLC GetLogForVersionNo(int nExportLCID, int nVersionNo, Int64 nUserID)
        {
            return ExportLC.Service.GetLogForVersionNo(nExportLCID, nVersionNo, nUserID);
        }

        public static List<ExportLC> Gets_DistinctItem(string sSQL, Int64 nUserID)
        {
            return ExportLC.Service.Gets_DistinctItem(sSQL, nUserID);
        }
       
        #endregion

        #region NonDB Functions
      

        #endregion

        #region ServiceFactory

     
        internal static IExportLCService Service
        {
            get { return (IExportLCService)Services.Factory.CreateService(typeof(IExportLCService)); }
        }
        #endregion
    }
    #endregion

    #region IExportLC interface
    
    public interface IExportLCService
    {
        ExportLC Get(int id, Int64 nUserID);
        ExportLC GetLog(int id, Int64 nUserID);
        ExportLC GetByNo(int nBUID,string sExportLCNo, Int64 nUserID);        
        List<ExportLC> Gets(int nBUID,DateTime dLCDate,Int64 nUserID);
        List<ExportLC> Gets(string sSQL, Int64 nUserID);
        List<ExportLC> GetsSQL(string sSQL, Int64 nUserID);
        List<ExportLC> Gets( int nBUID,Int64 nUserID);
        List<ExportLC> GetsLog(int nExportLCID, Int64 nUserID);
        ExportLC GetLogForVersionNo(int nExportLCID, int nVersionNo, Int64 nUserID);
        string Delete(ExportLC oExportLC, Int64 nUserID);
        ExportLC Save(ExportLC oExportLC, Int64 nUserID);
        ExportLC SaveMLC(ExportLC oExportLC, Int64 nUserID);
        ExportLC SaveLog(ExportLC oExportLC, Int64 nUserID);
        ExportLC Approved(ExportLC oExportLC, Int64 nUserID);
        ExportLC UpdateExportLCStatus(ExportLC oExportLC, Int64 nUserID);
        ExportLC UpdateForGetOrginalCopy(int nExportLCID, Int64 nUserID);
        ExportLC UpdateUDInfo(ExportLC oExportLC,int nOperation, Int64 nUserID);
        List<ExportLC> Gets_DistinctItem(string sSQL, Int64 nUserID);
       
    }

    #endregion
}