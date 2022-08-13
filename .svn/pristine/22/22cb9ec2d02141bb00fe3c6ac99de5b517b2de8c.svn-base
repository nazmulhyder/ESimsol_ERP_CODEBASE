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
    #region VOSummery
    public class VOSummery : BusinessObject
    {
        public VOSummery()
        {
            VOrderID = 0;
            OrderNo = "";
            BUID = 0;
            AccountHeadID = 0;
            SubledgerID = 0;
            CurrencyID = 0;
            CSymbol = "";
            OpenIsDebit = false;
            OpenDebit = 0;
            OpenCredit = 0;
            OpeningAmount = 0;
            DebitAmount = 0;
            CreditAmount = 0;
            CloseIsDebit = false;
            ClosingBalance = 0;
            StartDate = DateTime.Today;
            EndDate = DateTime.Today;
            IsApproved = false;
            BalanceStatusInt = 0;
            DueDays = 0;
            ErrorMessage = "";
            VOSummerys = new List<VOSummery>();
        }

        #region Properties
        public int VOrderID { get; set; }
        public string OrderNo { get; set; }
        public int BUID { get; set; }
        public int AccountHeadID { get; set; }
        public int SubledgerID { get; set; }
        public int CurrencyID { get; set; }
        public string CSymbol { get; set; }
        public bool OpenIsDebit { get; set; }
        public double OpenDebit { get; set; }
        public double OpenCredit { get; set; }
        public double OpeningAmount { get; set; }
        public double DebitAmount { get; set; }
        public double CreditAmount { get; set; }
        public bool CloseIsDebit { get; set; }
        public double ClosingBalance { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public bool IsApproved { get; set; }
        public int BalanceStatusInt { get; set; }
        public int DueDays { get; set; }
        public string ErrorMessage { get; set; }
        #endregion

        #region Derived Properties
        public Company Company { get; set; }
        public List<VOSummery> VOSummerys { get; set; }
        public string DueDaysSt
        {
            get
            {
                if(this.DueDays>0)
                {
                    return this.DueDays + " Day's";
                }
                else
                {
                    return "-";
                }
            }
        }
        public string OpenDrSt
        {
            get
            {
                return this.OpenIsDebit == true ? "Dr" : "Cr";
            }
        }
        public string CloseDrSt
        {
            get
            {
                return this.CloseIsDebit == true ? "Dr" : "Cr";
            }
        }
        public string OpeningAmountSt
        {
            get
            {
                return this.CSymbol + " " + Global.MillionFormat(this.OpeningAmount) + " " + this.OpenDrSt;
            }
        }
        public string ClosingBalanceSt
        {
            get
            {
                return this.CSymbol + " " + Global.MillionFormat(this.ClosingBalance) + " " + this.CloseDrSt;
            }
        }
        public string DebitAmountSt
        {
            get
            {
                return this.CSymbol + " " + Global.MillionFormat(this.DebitAmount);
            }
        }
        public string CreditAmountSt
        {
            get
            {
                return this.CSymbol + " " + Global.MillionFormat(this.CreditAmount);
            }
        }
        public string StartDateSt
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
        #endregion

        #region Functions
        public static List<VOSummery> GetsVOSummerys(VOSummery oVOSummery, int nUserID)
        {
            return VOSummery.Service.GetsVOSummerys(oVOSummery, nUserID);
        }

        #endregion

        #region ServiceFactory
        internal static IVOSummeryService Service
        {
            get { return (IVOSummeryService)Services.Factory.CreateService(typeof(IVOSummeryService)); }
        }
        #endregion
    }
    #endregion

    #region IVOSummery interface
    public interface IVOSummeryService
    {
        List<VOSummery> GetsVOSummerys(VOSummery oVOSummery, int nUserID);
    }
    #endregion
}
