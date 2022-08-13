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
    #region BankReconciliation
    public class BankReconciliation : BusinessObject
    {
        public BankReconciliation()
        {
            BankReconciliationID = 0;
            SubledgerID = 0;
            VoucherDetailID = 0;
            CCTID = 0;
            AccountHeadID = 0;
            ReconcilHeadID = 0;
            ReconcilDate = DateTime.MinValue;
            ReconcilRemarks = "";
            IsDebit = false;
            Amount = 0;
            ReconcilStatus = EnumReconcilStatus.Initialize;
            ReconcilStatusInt = 0;
            ReconcilBy = 0;
            SubledgerCode = "";
            SubledgerName = "";
            AccountCode = "";
            AccountHeadName = "";
            RCAHCode = "";
            RCAHName = "";
            ReverseHead = "";
            ReconcilByName = "";
            VoucherID = 0;
            VoucherDate = DateTime.Today;
            VoucherNo = "";
            DebitAmount = 0;
            CreditAmount = 0;
            CurrentBalance = 0;
            ErrorMessage = "";
            BUID = 0;
            StartDate = DateTime.Today;
            EndDate = DateTime.Today;
            OpeningBalance = 0;
            BRObjs = new List<BRObj>();
            BankReconciliations = new List<BankReconciliation>();
            ReconcileDataType = EnumReconcileDataType.None;
            Currency = new Currency();
            ClosingBalanceSt = "";
            CurrencyID = 0;
            ReverseHeadID = 0;
            ReverseHeadIsLedger = false;
            ReverseHeadName = "";
            DrCAmount = 0;
            CrCAmount = 0;
            CUSymbol = "";
        }

        #region Properties
        public int BankReconciliationID { get; set; }
        public int SubledgerID { get; set; }
        public int VoucherDetailID { get; set; }
        public int CCTID { get; set; }
        public int AccountHeadID { get; set; }
        public int ReconcilHeadID { get; set; }
        public DateTime ReconcilDate { get; set; }
        public string ReconcilRemarks { get; set; }
        public bool IsDebit { get; set; }
        public double Amount { get; set; }
        public EnumReconcilStatus ReconcilStatus { get; set; }
        public int ReconcilStatusInt { get; set; }
        public int ReconcilBy { get; set; }
        public string SubledgerCode { get; set; }
        public string SubledgerName { get; set; }
        public string AccountCode { get; set; }
        public string AccountHeadName { get; set; }
        public string RCAHCode { get; set; }
        public string RCAHName { get; set; }
        public string ReverseHead { get; set; }
        public string ReconcilByName { get; set; }
        public int VoucherID { get; set; }
        public DateTime VoucherDate { get; set; }
        public string VoucherNo { get; set; }
        public double DebitAmount { get; set; }
        public double CreditAmount { get; set; }
        public double CurrentBalance { get; set; }
        public string ErrorMessage { get; set; }
        public string ClosingBalanceSt { get; set; }
        public int ReverseHeadID { get; set; }
        public bool ReverseHeadIsLedger { get; set; }
        public string ReverseHeadName { get; set; }
        public double DrCAmount { get; set; }
        public double CrCAmount { get; set; }
        public string CUSymbol { get; set; }
        
        #endregion

        #region Derived Property
        public int BUID { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }        
        public double OpeningBalance { get; set; }
        public string ReconcilDateSt
        {
            get
            {
                if (this.ReconcilDate == DateTime.MinValue)
                {
                    return "";
                }
                else
                {
                    return this.ReconcilDate.ToString("dd MMM yyyy");
                }
            }
        }
        public string VoucherDateSt
        {
            get
            {
                return this.VoucherDate.ToString("dd MMM yyyy");
            }
        }
        public string AmountSt
        {
            get
            {
                return Global.MillionFormat(this.Amount);
            }
        }
        public string DebitAmountSt
        {
            get
            {
                return Global.MillionFormat(this.DebitAmount);
            }
        }
        public string CreditAmountSt
        {
            get
            {
                return Global.MillionFormat(this.CreditAmount);
            }
        }
        public string CurrentBalanceSt
        {
            get
            {
                return Global.MillionFormat(this.CurrentBalance);
            }
        }
        public string OpeningBalanceSt
        {
            get
            {
                if (this.OpeningBalance < 0)
                {
                    return "(" + Global.MillionFormat(this.OpeningBalance * (-1)) + ")";
                }
                else
                {
                    return Global.MillionFormat(this.OpeningBalance);
                }
            }
        }
        public List<BankReconciliation> BankReconciliations { get; set; }
        public List<BankReconcilationStatement> BankReconciliationStatements { get; set; }
        public List<BRObj> BRObjs { get; set; }
        public Company Company { get; set; }
        public Currency Currency { get; set; }
        public int CurrencyID { get; set; }
        public EnumReconcileDataType ReconcileDataType { get; set; }
        #endregion

        #region Functions
        public static List<BankReconciliation> PrepareBankReconciliation(BankReconciliation oBankReconciliation, Int64 nUserID)
        {
            return BankReconciliation.Service.PrepareBankReconciliation(oBankReconciliation, nUserID);
        }
        public static List<BankReconciliation> MultiSave(BankReconciliation oBankReconciliation, Int64 nUserID)
        {
            return BankReconciliation.Service.MultiSave(oBankReconciliation, nUserID);
        }
        public static List<BankReconciliation> Gets(long nUserID)
        {
            return BankReconciliation.Service.Gets(nUserID);
        }
        public static List<BankReconciliation> Gets(string sSQL, Int64 nUserID)
        {
            return BankReconciliation.Service.Gets(sSQL, nUserID);
        }
        public BankReconciliation GetLastTransaction(int nSubledgerID, long nUserID)
        {
            return BankReconciliation.Service.GetLastTransaction(nSubledgerID, nUserID);
        }
        public BankReconciliation Get(int nId, long nUserID)
        {
            return BankReconciliation.Service.Get(nId, nUserID);
        }
        public BankReconciliation Save(long nUserID)
        {
            return BankReconciliation.Service.Save(this, nUserID);
        }
        public BankReconciliation UpdateRemarks(long nUserID)
        {
            return BankReconciliation.Service.UpdateRemarks(this, nUserID);
        }
        public string Delete(int nId, long nUserID)
        {
            return BankReconciliation.Service.Delete(nId, nUserID);
        }
        public static string AuthorizeBankReconciliation(List<BankReconciliation> oBankReconciliations, long nUserID)
        {
            return BankReconciliation.Service.AuthorizeBankReconciliation(oBankReconciliations, nUserID);
        }
        #endregion

        #region ServiceFactory
        internal static IBankReconciliationService Service
        {
            get { return (IBankReconciliationService)Services.Factory.CreateService(typeof(IBankReconciliationService)); }
        }
        #endregion
    }
    #endregion

    #region IBankReconciliation interface

    public interface IBankReconciliationService
    {
        BankReconciliation Get(int id, long nUserID);
        BankReconciliation GetLastTransaction(int nSubledgerID, long nUserID);
        List<BankReconciliation> Gets(long nUserID);
        List<BankReconciliation> Gets(string sSQL, Int64 nUserID);
        string Delete(int id, long nUserID);
        BankReconciliation Save(BankReconciliation oBankReconciliation, long nUserID);
        BankReconciliation UpdateRemarks(BankReconciliation oBankReconciliation, long nUserID);
        List<BankReconciliation> PrepareBankReconciliation(BankReconciliation oBankReconciliation, Int64 nUserID);
        List<BankReconciliation> MultiSave(BankReconciliation oBankReconciliation, Int64 nUserID);
        string AuthorizeBankReconciliation(List<BankReconciliation> oBankReconciliations, long nUserID);
    }

    #endregion

    #region Bank Reconciliation Temp Object
    public class BRObj
    {
        public BRObj()
        {
            SL = 0;
            BRID = 0;
            SLID = 0;
            VDID = 0;
            CCTID = 0;
            AHID = 0;
            RHID = 0;
            RDate = DateTime.MinValue;
            RRmrk = "";
            IsDr = false;
            Amount = 0;
            RStatus = EnumReconcilStatus.Initialize;
            RStatusInt = 0;
            ReconcilBy = 0;
            AHCode = "";
            AHName = "";
            RCAHCode = "";
            RCAHName = "";
            RVAH = "";
            VID = 0;
            VDate = DateTime.Today;
            VNo = "";
            DrAmount = 0;
            CrAmount = 0;
            CBalance = 0;
            EMessage = "";
        }

        #region Properties
        public int SL { get; set; }
        public int BRID { get; set; }
        public int SLID { get; set; }
        public int VDID { get; set; }
        public int CCTID { get; set; }
        public int AHID { get; set; }
        public int RHID { get; set; }
        public DateTime RDate { get; set; }
        public string RRmrk { get; set; }
        public bool IsDr { get; set; }
        public double Amount { get; set; }
        public EnumReconcilStatus RStatus { get; set; }
        public int RStatusInt { get; set; }
        public int ReconcilBy { get; set; }
        public string AHCode { get; set; }
        public string AHName { get; set; }
        public string RCAHCode { get; set; }
        public string RCAHName { get; set; }
        public string RVAH { get; set; }
        public int VID { get; set; }
        public DateTime VDate { get; set; }
        public string VNo { get; set; }
        public double DrAmount { get; set; }
        public double CrAmount { get; set; }
        public double CBalance { get; set; }
        public string EMessage { get; set; }
        #endregion

        #region Derived Property
        public string RDateSt
        {
            get
            {
                if (this.RDate == DateTime.MinValue)
                {
                    return "";
                }
                else
                {
                    return this.RDate.ToString("dd/MM/yyyy");
                }
            }
        }
        public string RDateStRpt
        {
            get
            {
                if (this.RDate == DateTime.MinValue)
                {
                    return "";
                }
                else
                {
                    return this.RDate.ToString("dd MMM yyyy");
                }
            }
        }
        public string VDateSt
        {
            get
            {
                return this.VDate.ToString("dd MMM yyyy");
            }
        }
        public string AmountSt
        {
            get
            {
                return Global.MillionFormat(this.Amount);
            }
        }
        public string DrAmountSt
        {
            get
            {
                if (this.IsDr)
                {
                    return Global.MillionFormat(this.DrAmount);
                }
                else
                {
                    return "--";
                }

            }
        }
        public string CrAmountSt
        {
            get
            {
                if (this.IsDr)
                {
                    return "--";
                }
                else
                {
                    return Global.MillionFormat(this.CrAmount);
                }


            }
        }
        public string CBalanceSt
        {
            get
            {
                if (this.CBalance < 0)
                {
                    return "(" + Global.MillionFormat(this.CBalance * (-1)) + ")";
                }
                else
                {
                    return Global.MillionFormat(this.CBalance);
                }
            }
        }
        #endregion
    }
    #endregion
}