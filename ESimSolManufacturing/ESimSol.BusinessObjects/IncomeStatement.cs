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
    #region IncomeStatement
    public class IncomeStatement : BusinessObject
    {
        public IncomeStatement()
        {
            AccountHeadID = 0;
            BUID = 0;
            AccountCode = "";
            AccountHeadName = "";
            ParentAccountHeadID = 0;
            ComponentType = EnumComponentType.None;
            AccountType = EnumAccountType.None;
            OpenningBalance = 0;
            OpenningBalanceFor_PSession = 0;
            DebitTransaction = 0;
            CreditTransaction = 0;
            DebitTransactionFor_PSession = 0;
            CreditTransactionFor_PSession = 0;
            ClosingBalance = 0;
            AccountingSessions = new List<AccountingSession>();
            AccountTypeInInt = 1;
            AccountingSessionID = 0;
            IncomeStatements = new List<IncomeStatement>();
            CISSetup = EnumCISSetup.None;

            PurchaseCreditTransaction = 0;
            PurchaseCreditTransactionFor_PSession = 0;
            ClosingBalanceFor_PSession = 0;
            PriviosSessoinName = "";
            ErrorMessage = "";
        }

        #region Properties
        public int AccountHeadID { get; set; }
        public int BUID { get; set; }
        public string AccountCode { get; set; }
        public string AccountHeadName { get; set; }
        public int ParentAccountHeadID { get; set; }
        public EnumComponentType ComponentType { get; set; }
        public EnumAccountType AccountType { get; set; }
        public int AccountTypeInInt { get; set; }
        public double OpenningBalance { get; set; }
        public double OpenningBalanceFor_PSession { get; set; }
        public double DebitTransaction { get; set; }
        public double DebitTransactionFor_PSession { get; set; }
        public double CreditTransaction { get; set; }
        public double CreditTransactionFor_PSession { get; set; }
        public double ClosingBalance { get; set; }
        public double ClosingBalanceFor_PSession { get; set; }
        public double PurchaseCreditTransaction { get; set; }
        public double PurchaseCreditTransactionFor_PSession { get; set; }
        public bool IsForCurrentDate { get; set; }
        public EnumCISSetup CISSetup { get; set; }
     
        public string ErrorMessage { get; set; }
        #endregion

        #region Derived Property
        #region OpeningDrBalance
        public double OpeningDrBalance
        {
            get
            {
                if (this.IsDeditBalance(this.ComponentType, this.OpenningBalance))
                {
                    if (this.OpenningBalance < 0)
                    {
                        return this.OpenningBalance * (-1);
                    }
                    else
                    {
                        return this.OpenningBalance;
                    }
                }
                else
                {
                    return 0.00;
                }
            }
        }
        #endregion
        #region OpeningCrBalance
        public double OpeningCrBalance
        {
            get
            {
                if (this.IsDeditBalance(this.ComponentType, this.OpenningBalance))
                {
                    return 0.00;
                }
                else
                {
                    if (this.OpenningBalance < 0)
                    {
                        return this.OpenningBalance * (-1);
                    }
                    else
                    {
                        return this.OpenningBalance;
                    }
                }
            }
        }
        #endregion
        #region ClosingDrBalance
        public double ClosingDrBalance
        {
            get
            {
                if (this.IsDeditBalance(this.ComponentType, this.ClosingBalance))
                {
                    if (this.ClosingBalance < 0)
                    {
                        return this.ClosingBalance * (-1);
                    }
                    else
                    {
                        return this.ClosingBalance;
                    }
                }
                else
                {
                    return 0.00;
                }
            }
        }
        #endregion
        #region ClosingCrBalance
        public double ClosingCrBalance
        {
            get
            {
                if (this.IsDeditBalance(this.ComponentType, this.ClosingBalance))
                {
                    return 0.00;
                }
                else
                {
                    if (this.ClosingBalance < 0)
                    {
                        return this.ClosingBalance * (-1);
                    }
                    else
                    {
                        return this.ClosingBalance;
                    }
                }
            }
        }
        #endregion
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string StartDateSt { get { return this.StartDate.ToString("dd MMM yyyy"); } }
        public string EndDateSt { get { return this.EndDate.ToString("dd MMM yyyy"); } }
        public string StartDateFullSt { get { return this.StartDate.ToString("dd MMMM yyyy"); } }
        public string EndDateFullSt { get { return this.EndDate.ToString("dd MMMM yyyy"); } }
        public List<IncomeStatement> IncomeStatements { get; set; }
        public List<IncomeStatement> IncomeStatementsFor_PrivSession { get; set; }
        public List<IncomeStatement> Revenues { get; set; }
        public List<IncomeStatement> Expenses { get; set; }
        public List<IncomeStatement> Children { get; set; }
        public int AccountingSessionID { get; set; }
        public double TotalRevenues { get; set; }
        public double TotalExpenses { get; set; }
        public double TotalRevenuesFor_PSession { get; set; }
        public double TotalExpensesFor_PSession { get; set; }
        public string ProfiteLossAmount { get; set; }
        public string SessionDate { get; set; }
        public string PriviosSessoinName { get; set; }
        public Company Company { get; set; }
        public TChartsOfAccount TRevenue { get; set; }
        public TChartsOfAccount TExpenditure { get; set; }
        public List<EnumObject> AccountTypeObjs { get; set; }        
        public List<CIStatementSetup> CIStatementSetups { get; set; }
        public double LedgerBalance
        {
            get 
            {
                if (this.AccountType == EnumAccountType.Ledger)
                {
                    return this.ClosingBalance;
                }
                else
                {
                    return 0.00;
                }
            }
        }
        public double CGSGBalance //Component, Group, Sub Group,Ledger Balance
        {
            get
            {
                return this.ClosingBalance;
            }
        }
        public double CGSGBalance_ForSession
        {
            get
            {
                return this.ClosingBalanceFor_PSession;
            }
        }
        public string LedgerBalanceInString
        {
            get
            {
                string sLedgerBalance = "";
                if (this.AccountType == EnumAccountType.Ledger)
                {
                    if (this.LedgerBalance < 0)
                    {

                        sLedgerBalance = "(" + Global.MillionFormat(this.LedgerBalance * (-1)) + ")";
                    }
                    else if (this.LedgerBalance == 0)
                    {

                        sLedgerBalance = "-";
                    }
                    else
                    {
                        sLedgerBalance = Global.MillionFormat(this.LedgerBalance);
                    }
                }
                else
                {
                    sLedgerBalance = "";
                }
                return sLedgerBalance;
            }
        }
        public string CGSGBalanceInString  //Component, Group, Sub Group, Ledger Balance
        {
            get
            {
                string sCGSGBalance = "";
                if (this.AccountType != EnumAccountType.Ledger)
                {
                    if (this.CGSGBalance < 0)
                    {
                        sCGSGBalance = "(" + Global.MillionFormat(this.CGSGBalance * (-1)) + ")";
                    }                    
                    else if (this.CGSGBalance==0)
                    {
                        sCGSGBalance = "-";
                    }
                    else
                    {
                        sCGSGBalance = Global.MillionFormat(this.CGSGBalance);
                    }
                }
                else
                {
                    sCGSGBalance = "";
                }
                return sCGSGBalance;
            }
        }
        public string TotalRevenuesInString
        {
            get
            {
                return Global.MillionFormat(this.TotalRevenues);
            }
        }
        public string TotalExpensesInString
        {
            get
            {
                return Global.MillionFormat(this.TotalExpenses);
            }
        }
        public List<AccountingSession> AccountingSessions { get; set; } 
        #endregion

        #region Functions
        public static List<IncomeStatement> Gets(int nBUID, DateTime dStartDate, DateTime dEndDate, int ParentHeadID, int nAccountTypeInInt, int nUserID)
        {
            return IncomeStatement.Service.Gets(nBUID, dStartDate, dEndDate, ParentHeadID, nAccountTypeInInt, nUserID);
        }
        public static List<IncomeStatement> ProcessIncomeStatement(DateTime dStartDate, DateTime dEndDate, int nUserID)
        {
            return IncomeStatement.Service.ProcessIncomeStatement(dStartDate, dEndDate, nUserID);
        }
        #endregion

        #region Non DB Function
        public bool IsDeditBalance(EnumComponentType eEnumComponentType, double nAmount)
        {
            bool bIsDebitBalance = false;
            switch (eEnumComponentType)
            {
                case EnumComponentType.Asset:
                    #region Asset
                    if (nAmount >= 0)
                    {
                        bIsDebitBalance = true;
                    }
                    else
                    {
                        bIsDebitBalance = false;
                    }
                    break;
                    #endregion
                case EnumComponentType.Liability:
                    #region Laibility
                    if (nAmount >= 0)
                    {
                        bIsDebitBalance = false;
                    }
                    else
                    {
                        bIsDebitBalance = true;
                    }
                    break;
                    #endregion
                case EnumComponentType.OwnersEquity:
                    #region OwnerEquity
                    if (nAmount >= 0)
                    {
                        bIsDebitBalance = false;
                    }
                    else
                    {
                        bIsDebitBalance = true;
                    }
                    break;
                    #endregion
                case EnumComponentType.Income:
                    #region Income
                    if (nAmount >= 0)
                    {
                        bIsDebitBalance = false;
                    }
                    else
                    {
                        bIsDebitBalance = true;
                    }
                    break;
                    #endregion
                case EnumComponentType.Expenditure:
                    #region Expeness
                    if (nAmount >= 0)
                    {
                        bIsDebitBalance = true;
                    }
                    else
                    {
                        bIsDebitBalance = false;
                    }
                    break;
                    #endregion
            }
            return bIsDebitBalance;
        }
        public static List<IncomeStatement> GetStatements(EnumComponentType eEnumComponentType, List<IncomeStatement> oIncomeStatements)
        {
            List<IncomeStatement> oStatements = new List<IncomeStatement>();
            foreach (IncomeStatement oItem in oIncomeStatements)
            {
                if (oItem.ComponentType == eEnumComponentType)
                {
                    if (oItem.ComponentType == EnumComponentType.Income)
                    {
                        if (oItem.ComponentType == EnumComponentType.Income && oItem.AccountHeadID != 5)
                        {
                            oStatements.Add(oItem);
                        }
                    }
                    if (oItem.ComponentType == EnumComponentType.Expenditure)
                    {
                        if (oItem.ComponentType == EnumComponentType.Expenditure && oItem.AccountHeadID != 6)
                        {
                            oStatements.Add(oItem);
                        }
                    }
                }
            }
            return oStatements;
        }
        public static double ComponentBalance(EnumComponentType eEnumComponentType, List<IncomeStatement> oIncomeStatements)
        {
            double nClosingBalance = 0;
            foreach (IncomeStatement oItem in oIncomeStatements)
            {
                if (eEnumComponentType == EnumComponentType.Income)
                {
                    if (oItem.AccountHeadID == 5)
                    {
                        return oItem.ClosingBalance;
                    }
                }
                else
                {
                    if (oItem.AccountHeadID == 6)
                    {
                        return oItem.ClosingBalance;
                    }
                }                
            }
            return nClosingBalance;
        }
        public static double ComponentBalanceForPSession(EnumComponentType eEnumComponentType, List<IncomeStatement> oIncomeStatements)
        {
            double nClosingBalance = 0;
            foreach (IncomeStatement oItem in oIncomeStatements)
            {
                if (eEnumComponentType == EnumComponentType.Income)
                {
                    if (oItem.AccountHeadID == 5)
                    {
                        return oItem.CGSGBalance_ForSession;
                    }
                }
                else
                {
                    if (oItem.AccountHeadID == 6)
                    {
                        return oItem.CGSGBalance_ForSession;
                    }
                }
            }
            return nClosingBalance;
        }
        #endregion

        #region ServiceFactory
        internal static IIncomeStatementService Service
        {
            get { return (IIncomeStatementService)Services.Factory.CreateService(typeof(IIncomeStatementService)); }
        }
        #endregion
    }
    #endregion

    #region IIncomeStatement interface
    public interface IIncomeStatementService
    {
        List<IncomeStatement> Gets(int nBUID, DateTime dStartDate, DateTime dEndDate, int ParentHeadID, int nAccountTypeInInt, int nUserID);
        List<IncomeStatement> ProcessIncomeStatement(DateTime dStartDate, DateTime dEndDate, int nUserID);
    }
    #endregion
}
