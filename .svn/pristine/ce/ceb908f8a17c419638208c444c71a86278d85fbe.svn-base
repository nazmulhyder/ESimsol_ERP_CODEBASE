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
    #region BalanceSheet
    public class BalanceSheet : BusinessObject
    {
        public BalanceSheet()
        {
            AccountHeadID = 0;
            AccountCode = "";
            AccountHeadName = "";
            ParentAccountHeadID = 0;
            ComponentType = EnumComponentType.None;
            AccountType = EnumAccountType.None;
            OpenningBalance = 0;
            DebitTransaction = 0;
            CreditTransaction = 0;
            ClosingBalance = 0;
            ErrorMessage = "";
            Sequence = 0;
            TLiablityAndOwnersEquitys = new List<TChartsOfAccount>();
            TempDate = DateTime.Today;
            IsForCurrentDate = false;
            BUID = 0;
            nSearcingCriteria = 0;//single Date
            StartDate = DateTime.Today;
            EndDate = DateTime.Today;
            ReportCaption = "";
            Param = "";
            IsApproved = false;
        }

        #region Properties
        public int AccountHeadID { get; set; }
        public string AccountCode { get; set; }
        public string AccountHeadName { get; set; }
        public int ParentAccountHeadID { get; set; }
        public EnumComponentType ComponentType { get; set; }
        public int ComponentTypeInt { get; set; }
        public EnumAccountType AccountType { get; set; }
        public int AccountTypeInt { get; set; }
        public double OpenningBalance { get; set; }
        public double DebitTransaction { get; set; }
        public double CreditTransaction { get; set; }
        public double ClosingBalance { get; set; }
        public string ErrorMessage { get; set; }
        public int Sequence { get; set; }
        public bool IsForCurrentDate { get; set; }
        public bool IsApproved { get; set; }
        public int BUID { get; set; }
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
        public List<BalanceSheet> BalanceSheets { get; set; }
        public Company Company { get; set; }
        public List<BalanceSheet> Assets { get; set; }
        public List<BalanceSheet> LiabilitysOwnerEquitys { get; set; }
        public double TotalAssets { get; set; }
        public double TotalLiabilitysOwnerEquitys { get; set; }

        public double TotalAssetsTemp { get; set; }
        public double TotalLiabilitysOwnerEquitysTemp { get; set; }
        public string SessionDate { get; set; }
        public DateTime TempDate { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public int nSearcingCriteria { get; set; }
        public string Param { get; set; }
        public string ReportCaption { get; set; }

        public List<EnumObject> AccountTypeObjs { get; set; }
        #region Tree Property of Balance sheet
        public TChartsOfAccount TAsset { get;set;}
        public List<TChartsOfAccount> TLiablityAndOwnersEquitys {get;set;}
        #endregion

        public string BalanceSheetDate
        {
            get
            {             
                return this.TempDate.ToString("dd MMM yyyy");
            }
        }
        public string BalanceSheetDateTemp
        {
            get
            {
                if (this.nSearcingCriteria == 1)//Monthly
                {
                    
                    return Convert.ToDateTime(this.Param.Split('~')[1]).ToString("dd MMM yyyy");
                }
                else
                {
                    return DateTime.Today.ToString("dd MMM yyyy");
                }
            }
        }

        public string StartDateInSt
        {
            get
            {
                return this.StartDate.ToString("dd MMM yyyy");
            }
        }
        public string EndDateSt
        {
            get
            {
                return this.EndDate.ToString("dd MMM yyyy");
            }
        }
        
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
        public double CGSGBalance //Component, Group, Sub Group, Ledger Balance
        {
            get
            {
                return this.ClosingBalance;
            }
        }
        public double CGSGBalanceTemp //Component, Group, Sub Group, Ledger Balance( for Second date selectoin)
        {
            get;
            set;

        }
        public string TotalAssetsInString
        {
            get
            {
                return Global.MillionFormat(TotalAssets);
            }
        }
        public string TotalLiabilitysOwnerEquitysInString
        {
            get
            {
                return Global.MillionFormat(TotalLiabilitysOwnerEquitys);
            }
        }

        public string TotalAssetsTempInString
        {
            get
            {
                return Global.MillionFormat(TotalAssetsTemp);
            }
        }
        public string TotalLiabilitysOwnerEquitysTempInString
        {
            get
            {
                return Global.MillionFormat(TotalLiabilitysOwnerEquitysTemp);
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
                        sLedgerBalance = Global.MillionFormat(LedgerBalance);
                    }
                }
                else
                {
                    sLedgerBalance = "";
                }
                return sLedgerBalance;
            }
        }
        public string CGSGBalanceInStringTest  //Component, Group, Sub Group Balance
        {
            get
            {
                if (this.AccountType != EnumAccountType.Ledger)
                {
                    
                    if (this.CGSGBalance < 0)
                    {

                        return "(" + Global.MillionFormat(this.CGSGBalance * (-1)) + ")";
                    }
                    else if (this.CGSGBalance == 0)
                    {
                      
                        return  "-";
                    }
                    else
                    {
                        return Global.MillionFormat(CGSGBalance);
                    }
                }

                else
                {
                    return "";
                }
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
                    else if (this.CGSGBalance == 0)
                    {
                        return "-";

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

        public string CGSGBalanceInStringTemp  //Component, Group, Sub Group, Ledger Balance   ( for Second date selectoin)
        {
            get
            {
                string sCGSGBalance = "";
                if (this.AccountType != EnumAccountType.Ledger)
                {
                    if (this.CGSGBalanceTemp < 0)
                    {
                        sCGSGBalance = "(" + Global.MillionFormat(this.CGSGBalanceTemp * (-1)) + ")";
                    }
                    else if (this.CGSGBalanceTemp == 0)
                    {
                        return "-";

                    }
                    else
                    {
                        sCGSGBalance = Global.MillionFormat(this.CGSGBalanceTemp);
                    }
                }
                else
                {
                    sCGSGBalance = "";
                }
                return sCGSGBalance;
            }
        }
        #endregion

        #region Functions

        public static List<BalanceSheet> Gets(int nBUID, int nAccountType, DateTime dBalanceSheetStartDate, DateTime dBalanceSheetUptoDate, int nParentAccountHeadID, bool bIsApproved, int nUserID)
        {
            return BalanceSheet.Service.Gets(nBUID, nAccountType, dBalanceSheetStartDate, dBalanceSheetUptoDate, nParentAccountHeadID, bIsApproved, nUserID);
        }
        public static List<BalanceSheet> GetsForRationAnalysis(int nBUID, DateTime dBalanceSheetDate, int nRatioAnalysisID, bool bIsDivisible, int nParentAccountHeadID, int nUserID)
        {
            return BalanceSheet.Service.GetsForRationAnalysis(nBUID, dBalanceSheetDate, nRatioAnalysisID, bIsDivisible, nParentAccountHeadID, nUserID);
        }
        public static List<BalanceSheet> ProcessBalanceSheet(DateTime dBalanceSheetDate, string sStartBusinessCode, string sEndBusinessCode, int nUserID)
        {
            return BalanceSheet.Service.ProcessBalanceSheet(dBalanceSheetDate, sStartBusinessCode, sEndBusinessCode, nUserID);
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
        public static List<BalanceSheet> GetStatements(EnumComponentType eEnumComponentType, List<BalanceSheet> oBalanceSheets)
        {
            List<BalanceSheet> oStatements = new List<BalanceSheet>();
            foreach (BalanceSheet oItem in oBalanceSheets)
            {
                if (oItem.ComponentType == eEnumComponentType)
                {
                    if (oItem.ComponentType == EnumComponentType.Asset)
                    {
                        if (oItem.ComponentType == EnumComponentType.Asset && oItem.AccountHeadID != 2)
                        {
                            oStatements.Add(oItem);
                        }
                    }
                    if (oItem.ComponentType == EnumComponentType.Liability)
                    {
                        if (oItem.ComponentType == EnumComponentType.Liability && oItem.AccountHeadID != 3)
                        {
                            oStatements.Add(oItem);
                        }
                    }
                    if (oItem.ComponentType == EnumComponentType.OwnersEquity)
                    {
                        if (oItem.ComponentType == EnumComponentType.OwnersEquity && oItem.AccountHeadID != 4)
                        {
                            oStatements.Add(oItem);
                        }
                    }
                }
            }
            return oStatements;
        }
        public static double ComponentBalance(EnumComponentType eEnumComponentType, List<BalanceSheet> oBalanceSheets)
        {
            double nClosingBalance = 0;

            foreach (BalanceSheet oItem in oBalanceSheets)
            {
                if (eEnumComponentType == EnumComponentType.Asset)
                {
                    if (oItem.AccountHeadID == 2)
                    {
                        
                        return  oItem.ClosingBalance;
                    }
                }
                else
                {
                    if (oItem.AccountHeadID == 3 || oItem.AccountHeadID==4)
                    {
                        nClosingBalance = nClosingBalance+  oItem.ClosingBalance;
                    }
                }
            }
            return nClosingBalance;
        }
        public static double ComponentBalanceTemp(EnumComponentType eEnumComponentType, List<BalanceSheet> oBalanceSheets)
        {
            double nClosingBalance = 0;

            foreach (BalanceSheet oItem in oBalanceSheets)
            {
                if (eEnumComponentType == EnumComponentType.Asset)
                {
                    if (oItem.AccountHeadID == 2)
                    {

                        return  oItem.CGSGBalanceTemp ;
                    }
                }
                else
                {
                    if (oItem.AccountHeadID == 3 || oItem.AccountHeadID == 4)
                    {
                        nClosingBalance = nClosingBalance + oItem.CGSGBalanceTemp;
                    }
                }
            }
            return nClosingBalance;
        }
        #endregion

        #region ServiceFactory
        internal static IBalanceSheetService Service
        {
            get { return (IBalanceSheetService)Services.Factory.CreateService(typeof(IBalanceSheetService)); }
        }
        #endregion
    }
    #endregion

    #region IBalanceSheet interface
    public interface IBalanceSheetService
    {
        List<BalanceSheet> Gets(int nBUID, int nAccountType, DateTime dBalanceSheetStartDate, DateTime dBalanceSheetUptoDate, int nParentAccountHeadID, bool bIsApproved, int nUserID);
        List<BalanceSheet> GetsForRationAnalysis(int nBUID, DateTime dBalanceSheetDate, int nRatioAnalysisID, bool bIsDivisible, int nParentAccountHeadID, int nUserID);
        List<BalanceSheet> ProcessBalanceSheet(DateTime dBalanceSheetDate, string sStartBusinessCode, string sEndBusinessCode, int nUserID);
    }
    #endregion
}
