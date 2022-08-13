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
    #region BudgetDetail
    public class BudgetDetail : BusinessObject
    {
        public BudgetDetail()
        {
            BudgetDetailID = 0;
            BudgetID = 0;
            AccountHeadID = 0;
            BudgetAmount = 0.0;
            Remarks = "";
            AccountHeadName = "";
            ParentHeadID = 0;
            AccountType = EnumAccountType.None;
            AccountCode = "";
            ComponentType = EnumComponentType.None;
            ErrorMessage = "";
        }

        #region Properties
        public int BudgetDetailID { get; set; }
        public int BudgetID { get; set; }
        public int AccountHeadID { get; set; }
        public double BudgetAmount { get; set; }
        public string Remarks { get; set; }
        public string AccountHeadName { get; set; }
        public int ParentHeadID { get; set; }
        public EnumAccountType AccountType { get; set; }
        public string AccountCode { get; set; }
        public bool IsJVNode { get; set; }
        public int AccountTypeInInt { get; set; }
        public string ErrorMessage { get; set; }
        public EnumComponentType ComponentType { get; set; }
        #endregion

        #region Derived Property
        public string AccountTypeInString
        {
            get
            {
                return EnumObject.jGet(this.AccountType);
            }
        }
        public string BudgetAmountSt
        {
            get
            {
                if(this.IsDebit)
                {
                    return "Dr " + Global.TakaFormat(BudgetAmount);
                }
                else
                {
                    return "Cr " + Global.TakaFormat(BudgetAmount);
                }
            }
        }
        public bool IsDebit
        {
            get
            {
                if(this.ComponentType==EnumComponentType.Asset || this.ComponentType==EnumComponentType.Expenditure)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }
        #endregion

        #region Functions
        public static List<BudgetDetail> Gets(string sSQL, long nUserID)
        {
            return BudgetDetail.Service.Gets(sSQL, nUserID);
        }
        public static List<BudgetDetail> GetsByBID(int BID, long nUserID)
        {
            return BudgetDetail.Service.GetsByBID(BID, nUserID);
        }

        public BudgetDetail Save(BudgetDetail oBudgetDetail, long nUserID)
        {
            return BudgetDetail.Service.Save(oBudgetDetail, nUserID);
        }
        #endregion

        #region ServiceFactory
        internal static IBudgetDetailService Service
        {
            get { return (IBudgetDetailService)Services.Factory.CreateService(typeof(IBudgetDetailService)); }
        }
        #endregion


    }

    public class TBudgetDetail
    {
        public TBudgetDetail()
        {
            BudgetID = 0;
            BudgetDetailID = 0;
            BudgetID = 0;
            AccountType = EnumAccountType.None;
        }
        #region Properties
        public int BudgetDetailID { get; set; }
        public int BudgetID { get; set; }
        public int AccountHeadID { get; set; }
        public double BudgetAmount { get; set; }
        public string Remarks { get; set; }
        public string AccountHeadName { get; set; }
        public EnumAccountType AccountType { get; set; }
        public string ErrorMessage { get; set; }
        public List<Budget> oTBudgetDetails { get; set; }
      
        public List<TBudgetDetail> children { get; set; }
        public List<TBudgetDetail> TBudgetDetails { get; set; }
        #endregion
        public string AccountTypeInString
        {
            get
            {
                return EnumObject.jGet(this.AccountType);
            }
        }
        
        public int parentid { get; set; }
        public int id { get; set; }
        public string text { get; set; }
        public string attributes { get; set; }
        public string code { get; set; }
        public bool IsjvNode { get; set; }
        public int AccountTypeInInt { get; set; }
        public string BudgetAmountSt
        {
            get
            {
                if (this.IsDebit)
                {
                    return "Dr " + Global.TakaFormat(BudgetAmount);
                }
                else
                {
                    return "Cr " + Global.TakaFormat(BudgetAmount);
                }
            }
        }
        public EnumComponentType ComponentType { get; set; }
        public bool IsDebit
        {
            get
            {
                if (this.ComponentType == EnumComponentType.Asset || this.ComponentType == EnumComponentType.Expenditure)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }
    }
    #endregion

    #region IBudgetDetail interface

    public interface IBudgetDetailService
    {
        List<BudgetDetail> Gets(string sSQL, Int64 nUserID);
        List<BudgetDetail> GetsByBID(int BID, Int64 nUserID);
        BudgetDetail Save(BudgetDetail oBudgetDetail, Int64 nUserID);
    }
    #endregion
}
