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
    #region Budget
    
    public class Budget : BusinessObject
    {
        public Budget()
        {
            BudgetID = 0;
            BudgetNo = "";
            ReviseNo = "";
            IssueDate = DateTime.Today;
            AccountingSessionID = 0;
            BudgetType = EnumBudgetType.None;
            BudgetStatus = EnumBudgetStatus.Initialized;
            ApproveBy = 0;
            ErrorMessage = "";
            BudgetDetails = new List<BudgetDetail>();
        }

        #region Properties
        public int BudgetID { get; set; }
        public string BudgetNo { get; set; }
        public string ReviseNo { get; set; }
        public DateTime IssueDate { get; set; }
        public int AccountingSessionID { get; set; }
        public EnumBudgetType BudgetType { get; set; }
        public EnumBudgetStatus BudgetStatus { get; set; }
        public string Remarks { get; set; }
        public int ApproveBy { get; set; }
        public List<BudgetDetail> BudgetDetails { get; set; }
        public string SessionName { get; set; }
        public int BUID { get; set; }
        public string BUName { get; set; }
        public string ApproveByName { get; set; }
        public string ErrorMessage { get; set; }
        #endregion

        #region Derived Property
        public string IssueDateSt
        {
            get
            {
                return IssueDate.ToString("dd MMM yyyy");
            }
        }
        public string BudgetTypeSt
        {
            get
            {
                return BudgetType.ToString();
            }
        }
        public string BudgetStatusSt
        {
            get
            {
                return BudgetStatus.ToString();
            }
        }
        #endregion

        #region Functions

        public static List<Budget> Gets(string sSQL, long nUserID)
        {
            return Budget.Service.Gets(sSQL, nUserID);
        }
        public Budget Get(int nBudgetID, long nUserID)
        {
            return Budget.Service.Get(nBudgetID, nUserID);
        }
        public Budget Save(Budget oBudget, long nUserID)
        {
            return Budget.Service.Save(oBudget, nUserID);
        }
        public Budget Revise(Budget oBudget, long nUserID)
        {
            return Budget.Service.Revise(oBudget, nUserID);
        }
        public string Delete(int nId, long nUserID)
        {
            return Budget.Service.Delete(nId, nUserID);
        }
        public Budget BudgetStatusChange(Budget oBudget, EnumDBOperation oDBOperation, long nUserID)
        {
            return Budget.Service.BudgetStatusChange(oBudget, oDBOperation, nUserID);
        }
        #endregion

        #region ServiceFactory
        internal static IBudgetService Service
        {
            get { return (IBudgetService)Services.Factory.CreateService(typeof(IBudgetService)); }
        }
        #endregion

    }
    #endregion

    #region IBudget interface
    
    public interface IBudgetService
    {
        List<Budget> Gets(string sSQL, Int64 nUserID);
        Budget Save(Budget oBudget, Int64 nUserID);
        Budget Revise(Budget oBudget, Int64 nUserID);
        Budget Get(int nBudgetID, Int64 nUserID);
        string Delete(int id, long nUserID);
        Budget BudgetStatusChange(Budget oBudget, EnumDBOperation oDBOperation, long nUserID);
    }
    #endregion


    public class TBudget
    {
        public TBudget()
        {
            BudgetID = 0;
            BudgetNo = "";
            ReviseNo = "";
            IssueDate = DateTime.MinValue;
            AccountingSessionID = 0;
            BudgetType = EnumBudgetType.None;
            BudgetStatus = EnumBudgetStatus.Initialized;
            ApproveBy = 0;
            ErrorMessage = "";
        }

        #region Properties
        public int BudgetID { get; set; }
        public string BudgetNo { get; set; }
        public string ReviseNo { get; set; }
        public DateTime IssueDate { get; set; }
        public int AccountingSessionID { get; set; }
        public EnumBudgetType BudgetType { get; set; }
        public EnumBudgetStatus BudgetStatus { get; set; }
        public string Remarks { get; set; }
        public int ApproveBy { get; set; }
        public List<Budget> oTBudgets { get; set; }
        public string ErrorMessage { get; set; }
        public int ParentID { get; set; }
        public List<TBudget> children { get; set; }
        public List<TBudget> TBudgets { get; set; }
        #endregion

        #region Derived Property
        public string IssueDateSt
        {
            get
            {
                return IssueDate.ToString("dd MMM yyyy");
            }
        }
        public string BudgetTypeSt
        {
            get
            {
                return BudgetType.ToString();
            }
        }
        public string BudgetStatusSt
        {
            get
            {
                return BudgetStatus.ToString();
            }
        }
        #endregion
    }
}

