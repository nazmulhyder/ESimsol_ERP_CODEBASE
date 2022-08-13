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
    #region ImportInvoiceIBP
    public class ImportInvoiceIBP : BusinessObject
    {
        public ImportInvoiceIBP()
        {
            ImportLCNo = "";
            ImportInvoiceNo = "";
            ContractorName = "";
            BankName = "";
            DateofMaturity = DateTime.Today;
            Amount = 0.0;
            ImportInvoiceID = 0;
            BankBranchID = 0;
            CurrencyID = 0;
            BankStatus = EnumInvoiceBankStatus.None;
            CurrentStatus = EnumInvoiceEvent.None;
            Amount_LC = 0.0;
            LCCurrentStatus = EnumLCCurrentStatus.None;
            LCPaymentType = EnumLCPaymentType.None;
            CCRate = 0.0;
            ErrorMessage = "";
            BankBranchs = new List<BankBranch>();
            BUID = 0;
        }

        #region Properties
        public string ImportLCNo { get; set; }
        public string ImportInvoiceNo { get; set; }
        public string ContractorName { get; set; }
        public string BankName { get; set; }
        public string BankNickName { get; set; }
        public DateTime DateofMaturity { get; set; }
        public DateTime ImportLCDate { get; set; }
        public DateTime DateofAcceptance { get; set; }
        public double Amount_LC { get; set; }
        public double Amount { get; set; }
        public int ImportInvoiceID { get; set; }
        public int BankBranchID { get; set; }
        public int CurrencyID { get; set; }
        public string ABPNo { get; set; }
        public string BUName { get; set; }
        public string CurrencyName { get; set; }
        public int Tenor { get; set; }
        public int BUID { get; set; }
        public string Currency { get; set; }
        public EnumInvoiceBankStatus BankStatus { get; set; }
        public EnumInvoiceEvent CurrentStatus { get; set; }
        public EnumLCCurrentStatus LCCurrentStatus { get; set; }
        public EnumLCPaymentType LCPaymentType { get; set; }
        public double CCRate { get; set; }

        public string ErrorMessage { get; set; }
        public string SelectedOption { get; set; }

        public DateTime DateofMaturityEnd { get; set; }

        #region AmountBCSt
        public double AmountBC
        {
            get
            {
                return (this.Amount * this.CCRate);
            }

        }
        public string AmountBCSt
        {
            get
            {
                return Global.MillionFormat((this.Amount*this.CCRate));
            }

        }
        public string AmountST
        {
            get
            {
                return this.Currency + " " + Global.MillionFormat(this.Amount);
            }

        }
        public string DateofMaturityST
        {
            get
            {
                if (this.DateofMaturity == DateTime.MinValue)
                {
                    return "";
                }
                else
                {
                    return this.DateofMaturity.ToString("dd MMM yyyy");
                }
            }

        }
        public string ImportLCDateSt
        {
            get
            {
                return this.ImportLCDate.ToString("dd MMM yyyy");
            }

        }
        public string DateofMaturityForGraph
        {
            get
            {
                if (this.DateofMaturity == DateTime.MinValue) return "Pending";
                return this.DateofMaturity.ToString("MMM yyyy");
            }
        }
        public string DateofAcceptanceSt
        {
            get
            {
                return this.DateofAcceptance.ToString("MMM yyyy");
            }
        }
        public string BankStatusSt
        {
            get
            {
                return EnumObject.jGet((EnumInvoiceBankStatus)this.BankStatus);
            }
        }
        #endregion
        #endregion



        #region Derived Property
        public  List<ImportInvoiceIBP> ImportInvoiceIBPs {get; set; }
        public List<ImportInvoiceIBP> PIIBPs_ChartList { get; set; }

        public List<BankBranch> BankBranchs { get; set; }
        
        #endregion

        #region Functions

    
        public static List<ImportInvoiceIBP> Gets(int buid, int nUserID)
        {
            return ImportInvoiceIBP.Service.Gets( buid, nUserID);
        }
        public static List<ImportInvoiceIBP> Gets(string sSQL, int nUserID)
        {
            return ImportInvoiceIBP.Service.Gets(sSQL, nUserID);
        }
     
        public static List<ImportInvoiceIBP> GetsForGraph(string sYear,int nBankBranchID,int BUID, int nUserID)
        {
            return ImportInvoiceIBP.Service.GetsForGraph(sYear, nBankBranchID, BUID,nUserID);
        }



        #endregion
        #region Non BD Functions

        public static double TotalMonth_Currency(int nBankBranchID, int nYear, int nMonth,int nCurrencyID, List<ImportInvoiceIBP> oImportInvoiceIBPs)
        {

            double nAmount = 0;

            foreach (ImportInvoiceIBP oitem in oImportInvoiceIBPs)
            {
                if (oitem.BankBranchID == nBankBranchID && oitem.DateofMaturity.Year == nYear && oitem.DateofMaturity.Month == nMonth && oitem.CurrencyID == nCurrencyID)
                {
                    nAmount = nAmount + oitem.Amount;
                }
            }

            return nAmount;
        }
        public static double TotalBank_Currency(int nBankBranchID, int nCurrencyID, List<ImportInvoiceIBP> oImportInvoiceIBPs)
        {

            double nAmount = 0;

            foreach (ImportInvoiceIBP oitem in oImportInvoiceIBPs)
            {
                if (oitem.BankBranchID == nBankBranchID & oitem.CurrencyID == nCurrencyID)
                {
                    nAmount = nAmount + oitem.Amount;
                }
            }

            return nAmount;
        }
        #endregion 
        #region ServiceFactory

      
           internal static IImportInvoiceIBPService Service
        {
            get { return (IImportInvoiceIBPService)Services.Factory.CreateService(typeof(IImportInvoiceIBPService)); }
        }
        #endregion
    }
    #endregion

    #region Report Study
    public class ImportInvoiceIBPList : List<ImportInvoiceIBP>
    {
        public string ImageUrl { get; set; }
    }
    #endregion

    #region IImportInvoiceIBP interface
    public interface IImportInvoiceIBPService
    {
       
        List<ImportInvoiceIBP> Gets(int BUID,Int64 nUserID);
        List<ImportInvoiceIBP> Gets(string sSQL, Int64 nUserID);
        List<ImportInvoiceIBP> GetsForGraph(string sYear,int nBankBranchID,int BUID, Int64 nUserID);

       
    }
    #endregion
}