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
    #region BudgetVariance
    
    public class BudgetVariance : BusinessObject
    {
        public BudgetVariance()
        {
            BudgetID = 0;
            BudgetDetailID = 0;
            ComponentID = 0;
            AccountHeadID = 0;
            AccountCode = "";
            AccountHeadName = "";
            SubGroupID = 0;
            SubGroupName = "";
            SubGroupCode = "";
            MonthNo = 0;
            NameofMonth = "";
            StartDate = DateTime.Now;
            EndDate = DateTime.Now;
            BudgetAmount = 0.0;
            DebitAmount = 0.0;
            CreditAmount = 0.0;
            ActualAmount = 0.0;
            VarianceAmount = 0.0;
            AchievePercent = 0.0;
            TotalBudgetAmount = 0.0;
            TotalActualAmount = 0.0;
            TotalVarianceAmount = 0.0;
            TotalAchievePercent = 0.0;
            ErrorMessage = "";
        }

        #region Properties
        public int BudgetID {get;set;}
        public int BudgetDetailID {get;set;}
        public int ComponentID {get;set;}
        public int AccountHeadID {get;set;}
        public string AccountCode {get;set;}
        public string AccountHeadName {get;set;}
        public int SubGroupID {get;set;}
        public string SubGroupName {get;set;}
        public string SubGroupCode { get; set; }
        public int MonthNo {get;set;}
        public string NameofMonth {get;set;}
        public DateTime StartDate {get;set;}
        public DateTime EndDate {get;set;}
        public double BudgetAmount {get;set;}
        public double DebitAmount { get; set; }
        public double CreditAmount { get; set; }
        public double ActualAmount { get; set; }
        public double VarianceAmount { get; set; }
        public double AchievePercent { get; set; }
        public double TotalBudgetAmount { get; set; }
        public double TotalActualAmount { get; set; }
        public double TotalVarianceAmount { get; set; }
        public double TotalAchievePercent { get; set; }
        public int ReportType { get; set; }
        public bool IsApproved { get; set; }
        public string ErrorMessage { get; set; }
        #endregion

        #region Derived Property
        public string DebitAmountSt
        {
            get
            {
                return Global.TakaFormat(DebitAmount);
            }
        }
        public string CreditAmountSt
        {
            get
            {
                return Global.TakaFormat(CreditAmount);
            }
        }
        public string BudgetAmountSt
        {
            get
            {
                return Global.TakaFormat(BudgetAmount);
            }
        }
        public string ActualAmountSt
        {
            get
            {
                return Global.TakaFormat(ActualAmount);
            }
        }
        public string VarianceAmountSt
        {
            get
            {
                if (VarianceAmount < 0)
                {
                    return "(" + Global.TakaFormat(VarianceAmount) + ")";
                }
                else
                {
                    return Global.TakaFormat(VarianceAmount);
                }

            }
        }
        public string AchievePercentSt
        {
            get
            {
                return Global.TakaFormat(AchievePercent) + " %";
            }
        }
        public string TotalBudgetAmountSt
        {
            get
            {
                return Global.TakaFormat(TotalBudgetAmount);
            }
        }
        public string TotalActualAmountSt
        {
            get
            {
                return Global.TakaFormat(TotalActualAmount);
            }
        }
        public string TotalVarianceAmountSt
        {
            get
            {
                if (TotalVarianceAmount<0)
                {
                    return "(" + Global.TakaFormat(TotalVarianceAmount) + ")";
                }
                else
                {
                    return Global.TakaFormat(TotalVarianceAmount);
                }
                
            }
        }
        public string TotalAchievePercentSt
        {
            get
            {
                return Global.TakaFormat(TotalAchievePercent)+"%";
            }
        }
        public string StartDateSt { get { return this.StartDate.ToString("dd MMM yyyy"); } }
        public string EndDateSt { get { return this.EndDate.ToString("dd MMM yyyy"); } }
     
        #endregion

        #region Functions

        public static List<BudgetVariance> Gets(long nUserID)
        {
            return BudgetVariance.Service.Gets(nUserID);
        }
        public static List<BudgetVariance> Gets(string sSQL, Int64 nUserID)
        {
            return BudgetVariance.Service.Gets(sSQL, nUserID);
        }
        public static List<BudgetVariance> GetsReport(int nBudgetID, int nReportType, bool IsApproved, string sStartDateSt, string sEndDateSt, Int64 nUserID)
        {
            return BudgetVariance.Service.GetsReport(nBudgetID, nReportType, IsApproved, sStartDateSt, sEndDateSt, nUserID);
        }

        #endregion

        #region ServiceFactory
        internal static IBudgetVarianceService Service
        {
            get { return (IBudgetVarianceService)Services.Factory.CreateService(typeof(IBudgetVarianceService)); }
        }
        #endregion
    }
    #endregion

    #region IBudgetVariance interface
    
    public interface IBudgetVarianceService
    {
        List<BudgetVariance> Gets(long nUserID);
        List<BudgetVariance> Gets(string sSQL, Int64 nUserID);
        List<BudgetVariance> GetsReport(int nBudgetID, int nReportType, bool IsApproved, string sStartDateSt, string sEndDateSt, Int64 nUserID);
    }
    #endregion
}
