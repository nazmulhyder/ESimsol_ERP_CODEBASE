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
    #region StatementNote
    public class StatementNote : BusinessObject
    {
        public StatementNote()
        {
            VoucherID = 0;
            VoucherNo = "";
            VoucherDate = DateTime.Today;
            ApprovedBy = 0;
            ApprovedByName = "";
            PrepareBy = 0;
            PrepareByName = "";
            VoucherNarration = "";
            CashVoucherDetailID = 0;
            VoucherDetailID = 0;
            CashAccountHeadID = 0;
            CashAccountCode = "";
            CashAccountName = "";
            CashSubLedgerID = 0;
            CashSubLedgerName = "";
            CashIsDebit = false;
            AccountHeadID = 0;
            ParticularAccountCode = "";
            ParticularAccountName = "";
            ParticularSubLedgerID = 0;
            ParticularSubLedgerName = "";
            IsDebit = false;
            Amount = 0;
            CurrencyID = 0;
            CurrencySymbol = "";
            ErrorMessage = "";
        }

        #region Properties
        public int VoucherID { get; set; }
        public string VoucherNo { get; set; }
        public DateTime VoucherDate { get; set; }
        public int ApprovedBy { get; set; }
        public string ApprovedByName { get; set; }
        public int PrepareBy { get; set; }
        public string PrepareByName { get; set; }
        public string VoucherNarration { get; set; }
        public int CashVoucherDetailID { get; set; }
        public int VoucherDetailID { get; set; }
        public int CashAccountHeadID { get; set; }
        public string CashAccountCode { get; set; }
        public string CashAccountName { get; set; }
        public int CashSubLedgerID { get; set; }
        public string CashSubLedgerName { get; set; }
        public bool CashIsDebit { get; set; }
        public int AccountHeadID { get; set; }
        public string ParticularAccountCode { get; set; }
        public string ParticularAccountName { get; set; }
        public int ParticularSubLedgerID { get; set; }
        public string ParticularSubLedgerName { get; set; }
        public bool IsDebit { get; set; }
        public double Amount { get; set; }
        public int CurrencyID { get; set; }
        public string CurrencySymbol { get; set; }
        public string ErrorMessage { get; set; }
        #endregion

        #region Derived Property
        public List<StatementNote> StatementNotes { get; set; }
        public Company Company { get; set; }
        public string VoucherDateString
        {
            get
            {
                return VoucherDate.ToString("dd MMM yyyy");
            }
        }
        public string AmountInString
        {
            get
            {
                return this.CurrencySymbol + " " + Global.MillionFormat(this.Amount);
            }
        }

        public string EffectedAccounts
        {
            get
            {
                if (this.CashSubLedgerID == 00)
                {
                    return this.CashAccountName;
                }
                else
                {
                    return this.CashSubLedgerName;
                }
            }
        }

        public string ParticularAccounts
        {
            get
            {
                if (this.ParticularSubLedgerID == 0)
                {
                    return this.ParticularAccountName;
                }
                else
                {
                    return this.ParticularSubLedgerName;
                }
            }
        }
        public string CashDebitCredit
        {
            get
            {
                if (this.CashIsDebit)
                {
                    return "Debit";
                }
                else
                {
                    return "Credit";
                }
            }
        }

        public string ParticularDebitCredit
        {
            get
            {
                if (this.IsDebit)
                {
                    return "Debit";
                }
                else
                {
                    return "Credit";
                }
            }
        }       
        #endregion

        #region Functions
        public static List<StatementNote> Gets(int nStatementSetupID, DateTime dstartDate, DateTime dendDate, int nBUID, int nAccountHeadID, bool bIsDebit,  int nUserID)
        {
            return StatementNote.Service.Gets(nStatementSetupID, dstartDate, dendDate, nBUID, nAccountHeadID, bIsDebit, nUserID);
        }
        #endregion


        #region ServiceFactory
        internal static IStatementNoteService Service
        {
            get { return (IStatementNoteService)Services.Factory.CreateService(typeof(IStatementNoteService)); }
        }
        #endregion
    }
    #endregion

    #region IStatementNote interface
    public interface IStatementNoteService
    {
        List<StatementNote> Gets(int nStatementSetupID, DateTime dstartDate, DateTime dendDate, int nBUID, int nAccountHeadID, bool bIsDebit, int nUserID);
    }
    #endregion
}
