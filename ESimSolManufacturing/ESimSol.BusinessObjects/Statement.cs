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
    #region Statement
    public class Statement : BusinessObject
    {
        public Statement()
        {
            LedgerGroupSetupID = 0;
            OCSID = 0;
            GroupType = 0;   // Operation Group or Ledger Group
            GroupName = "";
            BalanceAmount = 0;
            StatementSetupID = 0;
            StartDate = DateTime.Today;
            EndDate = DateTime.Today;
            IsDr = true;
            BalanceAmountInString = "";
            ErrorMessage = "";
            DateSearch = 0;
            NetIncreaseDecreaseText = "";
            StatementName = "";
            LedgerGroupName = "";
            StatementNotes = new List<StatementNote>();
            StatementSetups = new List<StatementSetup>();
            Statements = new List<Statement>();
            Title = "";
            IsDebit = false;
        }

        #region Properties
        public int LedgerGroupSetupID { get; set; }
        public int OCSID { get; set; }
        public int GroupType { get; set; }
        public string GroupName { get; set; }
        public double BalanceAmount { get; set; }
        public string BalanceAmountInString { get; set; }
        public int StatementSetupID { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public bool IsDr { get; set; }
        public double OpeningBalance { get; set; }
        public double ClosingBalance { get; set; }
        public double PeriodClosingBalance { get; set; }
        public string NetIncreaseDecreaseText { get; set; }
        public string ErrorMessage { get; set; }
        public string StatementName { get; set; }
        public string LedgerGroupName { get; set; }
        public int DCO { get; set; } // Date Compare Operator
        public int BUID { get; set; }
        public bool IsDebit { get; set; }
        #endregion

        #region Derived Property
        public string Title { get; set; }
        public List<StatementNote> StatementNotes { get; set; }
        public List<StatementSetup> StatementSetups { get; set; }
        public List<Statement> Statements { get; set; }
        public Company Company { get; set; }
        public int DateSearch { get; set; }
        public string StartDateInString
        {
            get
            {
                return StartDate.ToString("dd MMM yyyy");
            }
        }
        public string EndDateInString
        {
            get
            {
                return EndDate.ToString("dd MMM yyyy");
            }
        }

        public string OpeningBalnceInstring
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
        public string ClosingBalanceInstring
        {
            get
            {
                if (this.ClosingBalance < 0)
                {
                    return "(" + Global.MillionFormat(this.ClosingBalance * (-1)) + ")";
                }
                else
                {
                    return Global.MillionFormat(this.ClosingBalance);
                }
            }
        }

        public string PeriodClosingBalanceInstring
        {
            get
            {
                if (this.PeriodClosingBalance < 0)
                {
                    return "(" + Global.MillionFormat(this.PeriodClosingBalance * (-1)) + ")";
                }
                else
                {
                    return Global.MillionFormat(this.PeriodClosingBalance);
                }
            }
        }

        
        #endregion

        #region Functions
        public static List<Statement> Gets(int nStatementSetupID, DateTime dstartDate, DateTime dendDate, int nBUID, int nUserID)
        {
            return Statement.Service.Gets(nStatementSetupID, dstartDate, dendDate, nBUID, nUserID);
        }
        #endregion


        #region ServiceFactory
        internal static IStatementService Service
        {
            get { return (IStatementService)Services.Factory.CreateService(typeof(IStatementService)); }
        }
        #endregion
    }
    #endregion

    #region IStatement interface
    public interface IStatementService
    {
        List<Statement> Gets(int nStatementSetupID, DateTime dstartDate, DateTime dendDate, int nBUID, int nUserID);
    }
    #endregion
}
