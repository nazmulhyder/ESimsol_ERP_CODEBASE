using System;
using System.IO;
using ICS.Core;
using ESimSol.BusinessObjects;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Collections.Generic;
using ICS.Core.Framework;
using ICS.Core.Utility;


namespace ESimSol.BusinessObjects
{
    #region CashFlow

    public class CashFlow : BusinessObject
    {
        public CashFlow()
        {
            CashFlowID = 0;
            CEVoucherDetailID = 0;
            CEIsDebit = false;
            CEAmount = 0;
            CashFlowHeadID = 0;
            VoucherDetailID = 0;
            IsDebit = false;
            Amount = 0;
            BUID = 0;
            AccountHeadCode = "";
            AccountHeadName = "";
            VoucherID = 0;
            VoucherNo = "";
            Narration = "";
            VoucherDate = DateTime.Today;
            CurrencySymbol = "";
            SubGroupID = 0;
            SubGroupName = "";
            SubGroupCode = "";
            AccountHeadID = 0;
        }

        #region Properties
        public int CashFlowID { get; set; }
        public int CEVoucherDetailID { get; set; }
        public bool CEIsDebit { get; set; }
        public double CEAmount { get; set; }
        public int CashFlowHeadID { get; set; }
        public int VoucherDetailID { get; set; }
        public bool IsDebit { get; set; }
        public double Amount { get; set; }
        public int VoucherID { get; set; }
        public string VoucherNo { get; set; }
        public string AccountHeadCode { get; set; }
        public string AccountHeadName { get; set; }
        public string Narration { get; set; }
        public DateTime VoucherDate { get; set; }
        public string CurrencySymbol { get; set; }
        public int BUID { get; set; }
        public string DisplayCaption { get; set; }
        public int SubGroupID { get; set; }
        public string SubGroupCode { get; set; }
        public string SubGroupName { get; set; }
        public int AccountHeadID { get; set; }
        public string ErrorMessage { get; set; }
        #endregion

        #region Derived Property

        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string VoucherDateST
        {
            get
            {
                return this.VoucherDate.ToString("dd MMM yyyy");
            }
        }
        public string AmountST
        {
            get
            {
                if (this.IsDebit)
                {
                    return "Dr " + Global.MillionFormat(this.Amount) + " " + this.CurrencySymbol;
                }
                else
                {
                    return "Cr " + Global.MillionFormat(this.Amount) + " " + this.CurrencySymbol;
                }
            }
        }
        public string CEAmountST
        {
            get
            {
                if (this.CEIsDebit)
                {
                    return "Dr " + Global.MillionFormat(this.CEAmount) + " " + this.CurrencySymbol;
                }
                else
                {
                    return "Cr " + Global.MillionFormat(this.CEAmount) + " " + this.CurrencySymbol;
                }
            }
        }

        public double DebitAmount
        {
            get
            {
                if (this.IsDebit)
                {
                    return this.Amount;
                }
                else
                {
                    return 0;
                }
            }
        }
        public double CreditAmount
        {
            get
            {
                if (this.IsDebit)
                {
                    return 0;
                }
                else
                {
                    return this.Amount;
                }
            }
        }
        #endregion

        #region Functions

        public static List<CashFlow> Gets(long nUserID)
        {
            return CashFlow.Service.Gets(nUserID);
        }

        public static List<CashFlow> GetsByName(string sName, long nUserID)
        {
            return CashFlow.Service.GetsByName(sName, nUserID);
        }


        public CashFlow Get(int id, long nUserID)
        {
            return CashFlow.Service.Get(id, nUserID);
        }

        public CashFlow Save(long nUserID)
        {
            return CashFlow.Service.Save(this, nUserID);
        }
        public static List<CashFlow> Gets(string sSQL, long nUserID)
        {
            return CashFlow.Service.Gets(sSQL, nUserID);
        }
        public static List<CashFlow> Gets(CashFlow oCashFlow, long nUserID)
        {
            return CashFlow.Service.Gets(oCashFlow, nUserID);
        }
        public static List<CashFlow> GetsForCashManage(CashFlow oCashFlow, long nUserID)
        {
            return CashFlow.Service.GetsForCashManage(oCashFlow, nUserID);
        }
        public static List<CashFlow> UpdateCashFlows(string sCashFlowIDs, int CashFlowHeadID, long nUserID)
        {
            return CashFlow.Service.UpdateCashFlows(sCashFlowIDs, CashFlowHeadID, nUserID);
        }
        public string Delete(int id, long nUserID)
        {
            return CashFlow.Service.Delete(id, nUserID);
        }
        public static List<CashFlow> GetCashFlowBreakDowns(CashFlowDmSetup oCashFlowDmSetup, long nUserID)
        {
            return CashFlow.Service.GetCashFlowBreakDowns(oCashFlowDmSetup, nUserID);
        }
        #endregion

        #region ServiceFactory
        internal static ICashFlowService Service
        {
            get { return (ICashFlowService)Services.Factory.CreateService(typeof(ICashFlowService)); }
        }

        #endregion
    }
    #endregion

    #region ICashFlow interface
     
    public interface ICashFlowService
    {
         
        CashFlow Get(int id, Int64 nUserID);
         
        List<CashFlow> Gets(Int64 nUserID);
        List<CashFlow> GetsForCashManage(CashFlow oCashFlow, Int64 nUserID);
        List<CashFlow> Gets(string sSQL, Int64 nUserID);
        List<CashFlow> Gets(CashFlow oCashFlow, Int64 nUserID);
        List<CashFlow> UpdateCashFlows(string sCashFlowIDs, int CashFlowHeadID, Int64 nUserID);       
        string Delete(int id, Int64 nUserID);         
        CashFlow Save(CashFlow oCashFlow, Int64 nUserID);
        List<CashFlow> GetsByName(string sName,  Int64 nUserID);
        List<CashFlow> GetCashFlowBreakDowns(CashFlowDmSetup oCashFlowDmSetup, Int64 nUserID);
    }
    #endregion
}