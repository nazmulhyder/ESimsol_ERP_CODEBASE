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
    #region ImportLCReport
    public class ImportLCReport : BusinessObject
    {
        #region  Constructor
        public ImportLCReport()
        {
            PPCID = 0;
            ImportLCNO = "";
            ImportLCDate = DateTime.Now;
            SupplierName = "";
            ProductName = "";
            ProductCode = "";
            UnitPrice = 0;
            Quantity = 0;
            NegotiateBankID = 0;
            NegotiateBankName = "";
            LCPaymentType = EnumLCPaymentType.None;
            LCAmount = 0;
            LCCoverNoteNo = "";
            ExpireDate = DateTime.Now;
            ShipmentDate = DateTime.Now;
            LCCurrentStatus = EnumLCCurrentStatus.None;
            LCANo = "";
            CurrencySymbol = "";
            MUnit = "";
            Company = new Company();
            ErrorMessage = "";

            //Adv Searching 

            sPaymentTypes = "";
            sApprovalStatus = "";
            Currencys = new List<Currency>();
            SelectedOption = 0;
            DateofLCEnd = DateTime.Now;
            sLCStatus = "";

        }
        #endregion
        #region Properties
        
        
        public int PPCID { get; set; }
        
        public string ImportLCNO { get; set; }
        
        public DateTime ImportLCDate { get; set; }

        
        public string SupplierName { get; set; }
        
        public string ProductName { get; set; }
        
        public string ProductCode { get; set; }
        
        public double UnitPrice { get; set; }
        
        public double Quantity { get; set; }
        
        public string NegotiateBankName { get; set; }
        
        public int NegotiateBankID { get; set; }
        
        public EnumLCPaymentType LCPaymentType { get; set; }
        
        public double LCAmount { get; set; }
        
        public string LCCoverNoteNo { get; set; }
        
        public DateTime ExpireDate { get; set; }
        
        public DateTime ShipmentDate { get; set; }
        
        public EnumLCCurrentStatus LCCurrentStatus { get; set; }
        
        public string LCANo { get; set; }
        
        public string CurrencySymbol { get; set; }
        
        public string MUnit { get; set; }

        
        public string ErrorMessage { get; set; }


        //Adv Searching
       
        
        public string sPaymentTypes { get; set; }
        
        public string sApprovalStatus { get; set; }
        
        public List<Currency> Currencys { get; set; }

        
        public int SelectedOption { get; set; }
        
        public DateTime DateofLCEnd { get; set; }
      
        
        public DateTime DateofMaturity { get; set; }
        
        public DateTime DateofMaturityEnd { get; set; }
        
        public string sLCStatus { get; set; }
        
        public Company Company { get; set; }
        
        public List<BankBranch> BankBranchs { get; set; }

        #region Derive
        public string LCPaymentTypeInString
        {
            get
            {
                return LCPaymentType.ToString();
            }
        }

        public string ImportLCDateInString
        {
            get
            {
                return ImportLCDate.ToString("dd MMM yyyy");
            }
        }

        public string ExpireDateInString
        {
            get
            {
                return ExpireDate.ToString("dd MMM yyyy");
            }
        }

        public string ShipmentDateInString
        {
            get
            {
                return ShipmentDate.ToString("dd MMM yyyy");
            }
        }

        public string LCCurrentStatusInString
        {
            get
            {
                return LCCurrentStatus.ToString();
            }
        }

        public string ValueSt
        {
            get
            {
                return this.CurrencySymbol + Global.MillionFormat((this.Quantity * this.UnitPrice));
            }
        }
        public string LCAmountSt
        {
            get
            {
                return this.CurrencySymbol + Global.MillionFormat( this.LCAmount);
            }
        }
        public string UPriceSt
        {
            get
            {
                return this.CurrencySymbol + this.UnitPrice.ToString();
            }
        }

        public string QtySt
        {
            get
            {
                return this.Quantity.ToString() + this.MUnit;
            }
        }

        public List<ImportLCReport> ImportLCReports { get; set; }
        #endregion Derive


        #endregion

        #region Functions
        public static List<ImportLCReport> Gets(string sSQL, int nUserID)
        {
            return ImportLCReport.Service.Gets(sSQL, nUserID);
        }


        #endregion

        
        #region ServiceFactory
        internal static IImportLCReportService Service
        {
            get { return (IImportLCReportService)Services.Factory.CreateService(typeof(IImportLCReportService)); }
        }
        #endregion
    }
    #endregion


    #region IImportLCReport interface
    
    public interface IImportLCReportService
    {

        [OperationContract]
        List<ImportLCReport> Gets(string sSQL, Int64 nUserID);

    }
    #endregion
}