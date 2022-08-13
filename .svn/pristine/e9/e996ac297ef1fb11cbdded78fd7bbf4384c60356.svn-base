using System;
using System.IO;
using ESimSol.BusinessObjects;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Collections.Generic;
using System.Data;
using ICS.Core.Framework;
using ICS.Core.Utility;

namespace ESimSol.BusinessObjects
{
    #region ExportPI
    public class ExportPI : BusinessObject
    {
        public ExportPI()
        {
            ExportPIID = 0;
            PaymentType = EnumPIPaymentType.LC;
            PaymentTypeInInt = (int)EnumPIPaymentType.LC;
            PINo = "";
            PIStatus = EnumPIStatus.Initialized;
            PIStatusInInt = 0;
            IssueDate = DateTime.Now;
            ValidityDate = IssueDate.AddDays(7);
            ContractorID = 0;
            BuyerID = 0;
            MKTEmpID = 0;
            BankBranchID = 0;
            BankAccountID = 0;
            CurrencyID = 2;
            DeliveryToID = 0;
            Qty = 0;
            Amount = 0;
            IsLIBORRate = true;
            IsBBankFDD = true;
            LCTermID = 8;
            OverdueRate = 16.5;
            LCOpenDate = DateTime.Now;
            DeliveryDate = DateTime.Now;
            Note = "";
            ApprovedBy = 0;
            ApprovedDate = DateTime.Now;
            LCID = 0;
            ExportPIPrintSetupID = 0;
            BUID = 0;
            PIType = EnumPIType.Open;
            ShipmentTerm = EnumShipmentTerms.None;
            ShipmentTermInInt = (int)EnumShipmentTerms.None;
            YetToProductionOrderQty = 0;

            ContractorContactPersonPhone = "";
            BankName = "";
            BranchName = "";
            AccountName = "";
            ContractorName = "";
            BuyerName = "";
            DeliveryToName = "";
            ExportLCNo = "";
            ContractorAddress = "";
            ContractorPhone = "";
            ContractorFax = "";
            ContractorEmail = "";
            MKTPName = "";
            MKTPNickName = "";
            Currency = "";
            LCTermsName = "";
            ErrorMessage = "";            
            ExportPITandCClauses = new List<ExportPITandCClause>();
            ExportPIDetails = new List<ExportPIDetail>();
            ExportPIActionInInt = 0;    
            BranchAddress = "";
            ExportPILogID = 0;
            ShipmentDate = DateTime.MinValue;
            Params = "";
            IsApproved = false;
            //DOType = 0;
            IsCreateReviseNo = false;
            ColorInfo = "";
            DepthOfShade = "";
            YarnCount = "";
            ContractorContactPerson = 0;
            BuyerContactPerson = 0;
            ContractorContactPersonName = "";
            BuyerContactPersonName = "";
            RateUnit = 1;
            AmendmentNo = 0;
            AmendmentRequired = false;
            BankAccountNo = "";
            ConversionRate = 0;
            SCRemarks  ="";
            BankChargeInfo ="";
            BankCharge = 0;
            MasterContactNo = "";
            OrderSheetID = 0;
            BUName = "";
            BUShortName = "";
            OrderSheetNo = "";
            ProductNatureInt = 0;
            PartyName = "";
            PartyAddress = "";
            DeliveryToAddress = "";
            ProductNature = EnumProductNature.Dyeing;
            SizeCategories = new List<SizeCategory>();
            ColorCategories = new List<ColorCategory>();
            ColorSizeRatios = new List<ColorSizeRatio>();
            PISizerBreakDowns = new List<PISizerBreakDown>();
            //PTUUnit2s = new List<PTUUnit2>();
            MasterPIMappings = new List<MasterPIMapping>();
            AttCount = "";
            MotherBuyerID = 0;
            MotherBuyerName = "";
            SampleInvoiceIDs = "";
            PrepareByName = "";
            IsPrintAC = true;
        }

        #region Properties
        public int ExportPIID { get; set; }
        public int ExportPILogID { get; set; }
        public EnumPIPaymentType PaymentType { get; set; }
        public int PaymentTypeInInt { get; set; }
        public string PINo { get; set; }
        public EnumPIStatus PIStatus { get; set; }
        public int PIStatusInInt { get; set; }
        public DateTime IssueDate { get; set; }
        public DateTime ValidityDate { get; set; }
        public int ContractorID { get; set; }
        public int BuyerID { get; set; }
        public int MKTEmpID { get; set; }
        public int BankBranchID { get; set; }
        public int BankAccountID { get; set; }
        public int CurrencyID { get; set; }        
        public int DeliveryToID { get; set; }
        public double Qty { get; set; }
        public double Amount { get; set; }
        public bool IsLIBORRate { get; set; }
        public bool IsBBankFDD { get; set; }
        public int LCTermID { get; set; }
        public double OverdueRate { get; set; }
        public DateTime LCOpenDate { get; set; }
        public DateTime DeliveryDate { get; set; }
        public string Note { get; set; }
        public string NoteTwo { get; set; }
        public int ApprovedBy { get; set; }
        public DateTime ApprovedDate { get; set; }
        public int LCID { get; set; }
        public int ExportPIPrintSetupID { get; set; }
        public string BankName { get; set; }
        public string BranchName { get; set; }
        public string AccountName { get; set; }
        public string BankAccountNo { get; set; }
        public string ContractorName { get; set; }
        public string BuyerName { get; set; }
        public string DeliveryToName { get; set; }
        public string ContractorAddress { get; set; }
        public string ContractorPhone { get; set; }
        public string ContractorFax { get; set; }
        public string ContractorEmail { get; set; }
        public string MKTPName { get; set; }
        public string MKTPNickName { get; set; }
        public string Currency { get; set; }
        public string LCTermsName { get; set; }
        public string Params { get; set; }
        public bool IsApproved { get; set; }
        public EnumShipmentTerms ShipmentTerm { get; set; }
        public int ShipmentTermInInt { get; set; }
        public int ContractorType { get; set; }
        public string ContractorContactPersonPhone { get; set; }
        public int RateUnit { get; set; }
        public string PrepareByName { get; set; }
        public bool IsCreateReviseNo { get; set; }
        public string ColorInfo { get; set; }
        public string DepthOfShade { get; set; }
        public string YarnCount { get; set; }
        public int BUID { get; set; }
        public int ReviseNo { get; set; }
        public int OrderSheetID { get; set; }
        public string BUName { get; set; }
        public string BUShortName { get; set; }
        public string OrderSheetNo { get; set; }
        public double YetToProductionOrderQty { get; set; }
        public EnumPIType PIType { get; set; }
        public int ContractorContactPerson { get; set; }
        public int BuyerContactPerson { get; set; }
        public EnumProductNature ProductNature { get; set; }
        public int ProductNatureInt { get; set; }
        public string ContractorContactPersonName { get; set; }
        public string BuyerContactPersonName { get; set; }
        public double ConversionRate { get; set; }
        public string SCRemarks { get; set; }
        public string BankChargeInfo { get; set; }
        public double BankCharge { get; set; }
        public string MasterContactNo { get; set; }
        public string PartyName { get; set; }
        public string PartyAddress { get; set; }
 
        public string DeliveryToAddress { get; set; }
        //public List<PTUUnit2> PTUUnit2s { get; set; }
        public string AttCount { get; set; }
        public DateTime OpeningDate { get; set; }
        #endregion

        #region Derive Property
        public string SampleInvoiceIDs { get; set; }
        public int ExportPIActionInInt { get; set; }
        public List<LCTerm> LCTerms { get; set; }
        public ExportPIPrintSetup ExportPIPrintSetup { get; set; }
        public List<ExportTermsAndCondition> ExportPIPrintSetupClauses { get; set; }
        public List<ExportPITandCClause> ExportPITandCClauses { get; set; }
        public List<ExportPIDetail> ExportPIDetails { get; set; }
        public List<MasterPIMapping> MasterPIMappings { get; set; }
        //public List<ExportPI> ExportPIs { get; set; }
        public List<SizeCategory> SizeCategories { get; set; }
        public List<ColorCategory> ColorCategories { get; set; }
        public List<ColorSizeRatio> ColorSizeRatios { get; set; }
        public List<PISizerBreakDown> PISizerBreakDowns { get; set; }
        public ExportLC ExportLC { get; set; }
        public List<OrderSheet> OrderSheets { get; set; }

        public Contractor Buyer { get; set; }
        public Contractor DeliveryTo { get; set; }
        public string ExportLCNo { get; set; }
        public string BranchAddress { get; set; }
        public int CurrentUserId { get; set; }
        public int CurrentStatus_LC { get; set; }
        public int AmendmentNo { get; set; }
        public bool AmendmentRequired { get; set; }
        public DateTime AmendmentDate { get; set; }
        public int MotherBuyerID { get; set; }
        public string MotherBuyerName { get; set; }
        public string PITypeInString
        {
            get { return this.PIType.ToString(); }
        }

        public DateTime ShipmentDate { get; set; }
        public string ErrorMessage { get; set; }
        public string QtyWithPIID
        {
            get
            {
                return this.ExportPIID + "~" + this.Qty;
            }
        }
       
        #region PINo_Full
        private string _sPINo_Full="";
        public string PINo_Full
        {
            get
            {
                if (this.ReviseNo>0)
                {

                    _sPINo_Full = this.PINo + "R-" + this.ReviseNo;
                }
                else
                {
                    _sPINo_Full = this.PINo;
                }
                return _sPINo_Full;
            }
        }
        #endregion
        #region ELCNo
        private string _sELCNo;
        public string ELCNo
        {
            get
            {
                if (String.IsNullOrEmpty(this.ExportLCNo))
                {
                    if (this.PaymentType == EnumPIPaymentType.NonLC)
                    {

                        _sELCNo = "Non LC";
                    }
                    else
                    {
                        _sELCNo = "Waiting For L/C ";
                    }
                }
                else if (this.AmendmentNo > 0)
                {
                    _sELCNo = this.ExportLCNo + " A-" + this.AmendmentNo.ToString();
                }
                else
                {
                    _sELCNo = this.ExportLCNo;
                }
                return _sELCNo;
            }
        }
        #endregion

        public string UPriceSt
        {
            get { return this.Currency + Global.MillionFormat(this.Amount / this.Qty); }
        }
     
        public int IsBBankFDDInInt
        {
            get 
            {
                if (this.IsBBankFDD)
                {
                    return 1;
                }
                else
                {
                    return 0;
                }
            }
        }

        public int IsLIBORRateInInt
        {
            get
            {
                if (this.IsLIBORRate)
                {
                    return 1;
                }
                else
                {
                    return 0;
                }
            }
        }

        public string ContractorNameCode
        {
            get
            {
                return this.ContractorName + "[" + this.ContractorID.ToString() + "]";
            }
        }
        //public double TotalQtyKG
        //{
        //    get { return Global.GetKG(this.Qty, 2); }
        //}
        //public double QtyLBS { get { return Global.GetLBS(this.Qty, 2); } }
        ///// <summary>
        ///// added by fahim0abir on date: 16 Aug 2015
        ///// for Spinning Unit Production Plan Informations Window.
        ///// </summary>
        //public string TotalQtyKGSt { get { return Global.MillionFormat(this.TotalQtyKG); } }
        //public string QtyLBSSt { get { return Global.MillionFormat(this.QtyLBS); } }

        public string QtySt { get { return Global.MillionFormat(this.Qty); } }
        public string OverdueRateSt { get { return Global.MillionFormat(this.OverdueRate); } }
        
        public string IssueDateInString
        {
            get
            {
                return this.IssueDate.ToString("dd MMM yyyy");
            }
        }
        public string ApprovedDateInString
        {
            get
            {
                return this.ApprovedDate.ToString("dd MMM yyyy");
            }
        }

        public string LCOpenDateInString
        {
            get
            {
                return this.LCOpenDate.ToString("dd MMM yyyy");
            }
        }
        public string DeliveryDateInString
        {
            get
            {
                return this.DeliveryDate.ToString("dd MMM yyyy");
            }
        }
        public string LCOpenDateInST
        {
            get
            {
                if (this.LCOpenDate == DateTime.MinValue) return DateTime.Now.ToString("dd MMM yyyy");

                else return this.LCOpenDate.ToString("dd MMM yyyy");
            }
        }
        public string DeliveryDateInST
        {
            get
            {
                if (this.DeliveryDate == DateTime.MinValue) return DateTime.Now.ToString("dd MMM yyyy");

                else return this.DeliveryDate.ToString("dd MMM yyyy");
            }
        }
        public string ValidityDateInString
        {
            get
            {
                if(this.ValidityDate ==  DateTime.Today)
                {
                    return this.ValidityDate.AddDays(7).ToString("dd MMM yyyy");
                }else
                {
                    return this.ValidityDate.ToString("dd MMM yyyy");
                }
            }
        }
        private string sPIStatusSt = "";
        public string PIStatusSt
        {
            get
            {
                sPIStatusSt = EnumObject.jGet(this.PIStatus) ;
                return sPIStatusSt;
            }
        }
        private string sExportLCStatusS = "";
        public string ExportLCStatusSt
        {
            get
            {
                if (String.IsNullOrEmpty(this.ExportLCNo))
                {
                    sExportLCStatusS = "Wating For L/C";
                }
                else if (this.AmendmentRequired)
                {
                    sExportLCStatusS = "Amendment Required";
                }
                else
                {
                    sExportLCStatusS = EnumObject.jGet((EnumExportLCStatus)this.CurrentStatus_LC);
                }
                return sExportLCStatusS;
            }
        }
        public string PaymentTypeSt
        {
            get
            {
                return EnumObject.jGet(this.PaymentType);
            }
        }
        public string ShipmentTermSt
        {
            get
            {
                return this.ShipmentTerm.ToString();
            }
        }
        public string ShipmentDateInStr
        {
            get
            {
                if (this.ShipmentDate != DateTime.MinValue)
                    return ShipmentDate.ToString("dd MMM yyyy");
                else
                    return "";
            }
        }
        public System.Drawing.Image Signature { get; set; }
        public System.Drawing.Image BU_Footer { get; set; }
        public bool IsPrintAC { get; set; } //Print Bank Account In PI Preview

        #region AmountSt
        public string AmountSt
        {
            get
            {
                return this.Currency + " " + Global.MillionFormat(this.Amount);
                //return this.Currency + "" + this.Amount.ToString("#,##0.0000");
            }
        }
        #endregion

        
        #endregion
      
        #region Functions

        public static List<ExportPI> Gets(Int64 nUserID)
        {
            return ExportPI.Service.Gets(nUserID);
        }
        public static List<ExportPI> Gets(string sSQL, Int64 nUserID)
        {
            return ExportPI.Service.Gets(sSQL, nUserID);
        }
      
    
        public static List<ExportPI> GetsLog(int nExportPIID, Int64 nUserID)
        {
            return ExportPI.Service.GetsLog(nExportPIID, nUserID);
        }
        public ExportPI Get(int id, Int64 nUserID)
        {
            return ExportPI.Service.Get(id, nUserID);
        }
        public ExportPI GetByLog(int nLogid, Int64 nUserID)
        {
            return ExportPI.Service.GetByLog(nLogid, nUserID);
        }
        public ExportPI Get(string sPINo, int nTextTileUnit, Int64 nUserID)
        {
            return ExportPI.Service.Get(sPINo, nTextTileUnit, nUserID);
        }
        public ExportPI Save(Int64 nUserID)
        {
            return ExportPI.Service.Save(this, nUserID);
        }
        public ExportPI SavePIMapping(Int64 nUserID)
        {
            return ExportPI.Service.SavePIMapping(this, nUserID);
        }
        public ExportPI PISWAP(Int64 nUserID)
        {
            return ExportPI.Service.PISWAP(this, nUserID);
        }
        public ExportPI UpdatePIInfo(Int64 nUserID)
        {
            return ExportPI.Service.UpdatePIInfo(this, nUserID);
        }
        public ExportPI Copy(Int64 nUserID)
        {
            return ExportPI.Service.Copy(this, nUserID);
        }
   
        public string Delete(Int64 nUserID)
        {
            return ExportPI.Service.Delete(this, nUserID);
        }
        public static List<ExportPI> GetsByLCID(int nLCID, Int64 nUserID)
        {
            return ExportPI.Service.GetsByLCID(nLCID, nUserID);
        }
        public ExportPI UpdatePIStatus(Int64 nUserID)
        {
            return ExportPI.Service.UpdatePIStatus(this, nUserID);
        }
        public ExportPI UpdatePINo(Int64 nUserID)
        {
            return ExportPI.Service.UpdatePINo(this, nUserID);
        }
        public ExportPI Approve(ExportPI oExportPI, Int64 nUserID)
        {
            return ExportPI.Service.Approve(oExportPI, nUserID);
        }
        public ExportPI UpdatePaymentType(Int64 nUserID)
        {
            return ExportPI.Service.UpdatePaymentType(this, nUserID);
        }
        public ExportPI AcceptExportPIRevise(Int64 nUserID)
        {
            return ExportPI.Service.AcceptExportPIRevise(this, nUserID);
        }
        #endregion

        #region Non DB Functions
     

        public static string IDInString(List<ExportPI> oExportPIs)
        {
            string sReturn = "";
            foreach (ExportPI oItem in oExportPIs)
            {
                sReturn = sReturn + oItem.ExportPIID.ToString() + ",";
            }
            if (sReturn == "") return "";
            sReturn = sReturn.Remove(sReturn.Length - 1, 1);
            return sReturn;
        }

        #endregion

        #region ServiceFactory
        internal static IExportPIService Service
        {
            get { return (IExportPIService)Services.Factory.CreateService(typeof(IExportPIService)); }
        }
        #endregion
    }
    #endregion

    #region IExportPI interface
    public interface IExportPIService
    {
        ExportPI Get(int id, Int64 nUserID);
        ExportPI GetByLog(int id, Int64 nUserID);
        ExportPI Get(string sPINo, int nTextTileUnit, Int64 nUserID);
        List<ExportPI> Gets(Int64 nUserID);
        List<ExportPI> Gets(string sSQL, Int64 nUserID);
        List<ExportPI> GetsLog(int nExportPIID, Int64 nUserID);
        List<ExportPI> Gets(int nContractorID, string sLCIDs, Int64 nUserID);
        List<ExportPI> Gets(int nContractorID, string sLCIDs, bool bPaymentType, Int64 nUserID);
        List<ExportPI> GetsByLCID(int nLCID, Int64 nUserID);
        ExportPI Save(ExportPI oExportPI, Int64 nUserID);
        ExportPI Approve(ExportPI oExportPI, Int64 nUserID);

        ExportPI SavePIMapping(ExportPI oExportPI, Int64 nUserID);
        ExportPI UpdatePIInfo(ExportPI oExportPI, Int64 nUserID);
        ExportPI UpdatePINo(ExportPI oExportPI, Int64 nUserID);
        ExportPI Copy(ExportPI oExportPI, Int64 nUserID);
        ExportPI PISWAP(ExportPI oExportPI, Int64 nUserID);
        string Delete(ExportPI oExportPI, Int64 nUserID);
        List<ExportPI> GetsByPIIDs(string sIDs, Int64 nUserID);
        ExportPI UpdatePIStatus(ExportPI oExportPI, Int64 nUserID);
        ExportPI UpdatePaymentType(ExportPI oExportPI, Int64 nUserID);
        ExportPI AcceptExportPIRevise(ExportPI oExportPI, Int64 nUserID);
    }
    #endregion
}
