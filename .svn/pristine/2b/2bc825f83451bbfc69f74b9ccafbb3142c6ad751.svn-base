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
    #region ExportGraph
    public class ExportGraph : BusinessObject
    {
        public ExportGraph()
        {
            ExportLCNo = "";
            ExportBillNo = "";
            ContractorName = "";
            BankName = "";
            MaturityDate = DateTime.MinValue;
            MaturityReceivedDate = DateTime.MinValue;
            RelizationDate = DateTime.MinValue;
            Amount = 0.0;
            ImportInvoiceID = 0;
            BankBranchID = 0;
            CurrencyID = 0;
            State = EnumLCBillEvent.BOEinHand;
            Amount_LC = 0.0;
            AcceptanceRate = 0.0;
            ErrorMessage = "";
            BankBranchs = new List<BankBranch>();
            BUID = 0;
            ApplicantName = "";
            SelectedOption = "";
            PINo = "";
        }

        #region Properties
        public string ExportLCNo { get; set; }
        public string ExportBillNo { get; set; }
        public string ContractorName { get; set; }
        public string BankName { get; set; }
        public string BankNickName { get; set; }
        public string MKTPName { get; set; }
        public string BBranchName_Issue { get; set; }
        public string BranchName_Issue { get; set; }
        public DateTime MaturityDate { get; set; }
        public DateTime ExportLCDate { get; set; }
        public DateTime RelizationDate { get; set; }
        public DateTime DateofAcceptance { get; set; }
        public DateTime ExportStartDate { get; set; }
        public DateTime ExportEndDate { get; set; }
        public double Amount_LC { get; set; }
        public double Amount { get; set; }
        public int ImportInvoiceID { get; set; }
        public int BankBranchID { get; set; }
        public int CurrencyID { get; set; }
        public string LDBCNo { get; set; }
        public string LDBPNo { get; set; }
        public string PINo { get; set; }
        public string BUName { get; set; }
        public string CurrencyName { get; set; }
        public string Tenor { get; set; }
        public int BUID { get; set; }
        public string Currency { get; set; }
        public EnumLCBillEvent State { get; set; }
        public double AcceptanceRate { get; set; }
        public string ErrorMessage { get; set; }
        public string SelectedOption { get; set; }
        public string ApplicantName { get; set; }
        public DateTime MaturityReceivedDate { get; set; }
        public DateTime MaturityDateEnd { get; set; }

        #region AmountBCSt
        public double AmountBC
        {
            get
            {
                return (this.Amount * this.AcceptanceRate);
            }

        }
        public string AmountBCSt
        {
            get
            {
                return Global.MillionFormat((this.Amount * this.AcceptanceRate));
            }

        }
        public string AmountST
        {
            get
            {
                return this.Currency + " " + Global.MillionFormat(this.Amount);
            }

        }
        public string MaturityReceivedDateSt
        {
            get
            {
                if (MaturityReceivedDate== DateTime.MinValue)
                {
                    return "--";
                    
                }
                else
                {
                    return this.MaturityReceivedDate.ToString("dd MMM yyyy");
                }
            }
            
        }
        public string RelizationDateSt
        {
            get
            {
                if (this.RelizationDate == DateTime.MinValue)
                {
                    return "--";

                }
                else
                {
                    return this.RelizationDate.ToString("dd MMM yyyy");
                }
               
            }
        }
        public string IssueBankNameAndBrunch
        {
            get
            {
                return (this.BranchName_Issue + " [" + this.BBranchName_Issue + "]");

            }

        }

        
        public string MaturityDateST
        {
            get
            {
                if (this.MaturityDate == DateTime.MinValue)
                {
                    return "--";
                }
                else
                {
                    return this.MaturityDate.ToString("dd MMM yyyy");
                }
            }

        }
        public string ExportLCDateSt
        {
            get
            {
                if (DateTime.Compare(ExportLCDate, DateTime.MinValue) > 0)
                {
                    return this.ExportLCDate.ToString("dd MMM yyyy");
                }
                else
                {
                    return "--";
                }
            }

        }
        public string MaturityDateForGraph
        {
            get
            {
                return this.MaturityDate.ToString("MMM yyyy");
            }
        }
        public string DateofAcceptanceST
        {
            get
            {
                if (DateTime.Compare(DateofAcceptance, DateTime.MinValue) > 0)
                {
                    return this.DateofAcceptance.ToString("dd MMM yyyy");
                }
                else
                {
                    return "--";
                }
            }
        }
        public string StateSt
        {
            get
            {
                return EnumObject.jGet((EnumLCBillEvent)this.State);
            }
        }
        #endregion
        #endregion



        #region Derived Property
        public List<ExportGraph> ExportGraphs { get; set; }
        public List<ExportGraph> PIIBPs_ChartList { get; set; }

        public List<BankBranch> BankBranchs { get; set; }

        #endregion

        #region Functions


        public static List<ExportGraph> Gets(int buid, int nUserID)
        {
            return ExportGraph.Service.Gets(buid, nUserID);
        }
        public static List<ExportGraph> Gets(string sSQL, int nUserID)
        {
            return ExportGraph.Service.Gets(sSQL, nUserID);
        }

        public static List<ExportGraph> GetsForGraph(string sYear, int nBankBranchID, int BUID,string sDateCriteria, int nUserID)
        {
            return ExportGraph.Service.GetsForGraph(sYear, nBankBranchID, BUID,sDateCriteria, nUserID);
        }



        #endregion
        #region Non BD Functions

        public static double TotalMonth_Currency(int nBankBranchID, int nYear, int nMonth, int nCurrencyID, List<ExportGraph> oExportGraphs)
        {

            double nAmount = 0;

            foreach (ExportGraph oitem in oExportGraphs)
            {
                if (oitem.BankBranchID == nBankBranchID && oitem.MaturityDate.Year == nYear && oitem.MaturityDate.Month == nMonth && oitem.CurrencyID == nCurrencyID)
                {
                    nAmount = nAmount + oitem.Amount;
                }
            }

            return nAmount;
        }
        public static double TotalBank_Currency(int nBankBranchID, int nCurrencyID, List<ExportGraph> oExportGraphs)
        {

            double nAmount = 0;

            foreach (ExportGraph oitem in oExportGraphs)
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


        internal static IExportGraphService Service
        {
            get { return (IExportGraphService)Services.Factory.CreateService(typeof(IExportGraphService)); }
        }
        #endregion
    }
    #endregion

    #region Report Study
    public class ExportGraphList : List<ExportGraph>
    {
        public string ImageUrl { get; set; }
    }
    #endregion

    #region IExportGraph interface
    public interface IExportGraphService
    {

        List<ExportGraph> Gets(int BUID, Int64 nUserID);
        List<ExportGraph> Gets(string sSQL, Int64 nUserID);
        List<ExportGraph> GetsForGraph(string sYear, int nBankBranchID, int BUID,string sDateCriteria, Int64 nUserID);


    }
    #endregion
}