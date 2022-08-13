using System;
using ICS.Core.Framework;
using ICS.Core.Utility;
using ICS.Core;
using ESimSol.BusinessObjects;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization;
using System.ServiceModel;
using ESimSol.BusinessObjects.ReportingObject;

namespace ESimSol.BusinessObjects
{
    #region CashFlowDmSetup
    public class CashFlowDmSetup : BusinessObject
    {
        public CashFlowDmSetup()
        {
            CashFlowDmSetupID = 0;
            CashFlowHeadID = 0;            
            SubGroupID = 0;
            IsDebit = false;
            Remarks = "";
            SubGroupCode = "";
            SubGroupName = "";
            SubGroupType = 0;
            DisplayCaption = "";
            CashFlowHeadType = EnumCashFlowHeadType.None;
            CashFlowHeadTypeInt = 0;
            Sequence = 0;
            Amount = 0;
            IsDetailsView = false;
            ErrorMessage = "";
            CashFlows = new List<CashFlow>();
            CashFlowHeadName = "";
        }

        #region Property
        public int CashFlowDmSetupID { get; set; }
        public int CashFlowHeadID { get; set; }
        public int SubGroupID { get; set; }
        public bool IsDebit { get; set; }
        public string Remarks { get; set; }
        public string SubGroupCode { get; set; }
        public string SubGroupName { get; set; }
        public int SubGroupType { get; set; }
        public string DisplayCaption { get; set; }
        public EnumCashFlowHeadType CashFlowHeadType { get; set; }
        public int CashFlowHeadTypeInt { get; set; }
        public int Sequence { get; set; }
        public double Amount { get; set; }
        public string ErrorMessage { get; set; }
        #endregion

        #region Derived Property   
        public string CashFlowHeadName { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public bool IsDetailsView { get; set; }
        public string StartDateSt { get { return this.StartDate.ToString("dd MMM yyyy"); } }
        public string EndDateSt { get { return this.EndDate.ToString("dd MMM yyyy"); } }
        public string SessionDate { get; set; }
        public int BUID { get; set; }
        public Company Company { get; set; }
        public List<CashFlowDmSetup> CashFlowDmSetups { get; set; }
        public List<CashFlow> CashFlows { get; set; }
        public string AmountSt
        {
            get
            {
                if (this.Amount == 0.00)
                {
                    return "-";
                }
                else if (this.Amount < 0.00)
                {
                    return "(" + Global.MillionFormat(this.Amount * (-1)) + ")";
                }
                else
                {
                    return Global.MillionFormat(this.Amount);
                }
            }
        }
        public string CashFlowHeadTypeSt
        {
            get
            {
                return  EnumObject.jGet(this.CashFlowHeadType).ToString();
            }
        }
        public string IsDebitSt
        {
            get
            {
                if(this.IsDebit)
                {
                    return "Debit";
                }else
                {
                    return "Credit";
                }
            }
        }
        #endregion

        #region Functions
        public static List<CashFlowDmSetup> Gets(int nBUID, DateTime dStartDate, DateTime dEndDate, bool bIsDetails, long nUserID)
        {
            return CashFlowDmSetup.Service.Gets(nBUID, dStartDate, dEndDate,bIsDetails, nUserID);
        }
        public static List<CashFlowDmSetup> Gets(long nUserID)
        {
            return CashFlowDmSetup.Service.Gets(nUserID);
        }
        public static List<CashFlowDmSetup> Gets(string sSQL, long nUserID)
        {
            return CashFlowDmSetup.Service.Gets(sSQL, nUserID);
        }
        public CashFlowDmSetup Get(int id, long nUserID)
        {
            return CashFlowDmSetup.Service.Get(id, nUserID);
        }
        public CashFlowDmSetup Save(long nUserID)
        {
            return CashFlowDmSetup.Service.Save(this, nUserID);
        }
        public string Delete(int id, long nUserID)
        {
            return CashFlowDmSetup.Service.Delete(id, nUserID);
        }
        #endregion

        #region ServiceFactory
        internal static ICashFlowDmSetupService Service
        {
            get { return (ICashFlowDmSetupService)Services.Factory.CreateService(typeof(ICashFlowDmSetupService)); }
        }
        #endregion
    }
    #endregion

    #region ICashFlowDmSetup interface
    public interface ICashFlowDmSetupService
    {
        CashFlowDmSetup Get(int id, Int64 nUserID);
        List<CashFlowDmSetup> Gets(int nBUID, DateTime dStartDate, DateTime dEndDate, bool bIsDetails, Int64 nUserID);
        List<CashFlowDmSetup> Gets(Int64 nUserID);
        List<CashFlowDmSetup> Gets(string sSQL, Int64 nUserID);
        string Delete(int id, Int64 nUserID);
        CashFlowDmSetup Save(CashFlowDmSetup oCashFlowDmSetup, Int64 nUserID);
    }
    #endregion
}