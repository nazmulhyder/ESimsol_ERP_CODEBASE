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
    #region FPMgtReport
    public class FPMgtReport : BusinessObject
    {
        public FPMgtReport()
        {
            BUID = 0;
            BUCode = "";
            BUName = "";
            BUShortName = "";
            DocInHand = 0;
            DocInCollection = 0;
            FCAmount = 0;
            FDRAmount = 0;
            TotalRecAndMargin = 0;
            BillsLiability = 0;
            BillsPayableBTB = 0;
            AccountsPayableBTB = 0;
            TotalBTBLiability = 0;
            STLPackingCredit = 0;
            STLCashIncentive = 0;
            STLLATREDF = 0;
            STLCashCredit = 0;
            STLFDBPLDBPCredit = 0;
            TotalSTLAmount = 0;
            LTLLocalAmount = 0;
            LTLOBUAmount = 0;
            TotalLTLAmount = 0;
            LCInHand = 0;
            ContactInHand = 0;
            TotalLCContact = 0;
            PreviousMonthLiability = 0;
            CurrencyName = "";
            Param = "";
            ErrorMessage = "";
            FPData = new BusinessObjects.FPData();
            FPMgtReports = new List<FPMgtReport>();
        }

        #region Properties
        public int BUID { get; set; }
        public string BUCode { get; set; }
        public string BUName { get; set; }
        public string BUShortName { get; set; }
        public double DocInHand { get; set; }
        public double DocInCollection { get; set; }
        public double FCAmount { get; set; }
        public double FDRAmount { get; set; }
        public double TotalRecAndMargin { get; set; }
        public double BillsLiability { get; set; }
        public double BillsPayableBTB { get; set; }
        public double AccountsPayableBTB { get; set; }
        public double TotalBTBLiability { get; set; }
        public double STLPackingCredit { get; set; }
        public double STLCashIncentive { get; set; }
        public double STLLATREDF { get; set; }
        public double STLCashCredit { get; set; }
        public double STLFDBPLDBPCredit { get; set; }
        public double TotalSTLAmount { get; set; }
        public double LTLLocalAmount { get; set; }
        public double LTLOBUAmount { get; set; }
        public double TotalLTLAmount { get; set; }
        public double LCInHand { get; set; }
        public double ContactInHand { get; set; }
        public double TotalLCContact { get; set; }
        public double PreviousMonthLiability { get; set; }
        public string CurrencyName { get; set; }
        public string Param { get; set; }
        public string ErrorMessage { get; set; }
        #endregion

        #region Derived Properties
        public Company Company { get; set; }
        public List<FPMgtReport> FPMgtReports { get; set; }
        public FPData FPData { get; set; }

        public string TotalRecAndMarginSt
        {
            get
            {
                if (this.TotalRecAndMargin >=0)
                {
                    return Global.MillionFormat_Round(this.TotalRecAndMargin);
                }
                else
                {
                    return "(" + Global.MillionFormat_Round(this.TotalRecAndMargin*-1) + ")";
                }
            }
        }
        public string TotalBTBLiabilitySt
        {
            get
            {
                
                if (this.TotalBTBLiability>=0)
                {
                    return Global.MillionFormat_Round(this.TotalBTBLiability);
                }
                else
                {
                    return "(" + Global.MillionFormat_Round(this.TotalBTBLiability * -1) + ")";
                }
            }
        }
        public string TotalLCContactSt
        {
            get
            {
                
                if (this.TotalLCContact >= 0)
                {
                    return Global.MillionFormat_Round(this.TotalLCContact);
                }
                else
                {
                    return "(" + Global.MillionFormat_Round(this.TotalLCContact * -1) + ")";
                }
            }
        }
        public string TotalSTLAmountSt
        {
            get
            {
             
                if (this.TotalSTLAmount >= 0)
                {
                    return Global.MillionFormat_Round(this.TotalSTLAmount);
                }
                else
                {
                    return "(" + Global.MillionFormat_Round(this.TotalSTLAmount * -1) + ")";
                }
            }
        }
        public string TotalLTLAmountSt
        {
            get
            {
                if (this.TotalLTLAmount >= 0)
                {
                    return Global.MillionFormat_Round(this.TotalLTLAmount);
                }
                else
                {
                    return "(" + Global.MillionFormat_Round(this.TotalLTLAmount * -1) + ")";
                }
            }
        }
        #endregion

        #region Functions
        public static List<FPMgtReport> GetsFPMgtReports(DateTime PositionDate, int CurrencyID, bool IsApproved, int nUserID)
        {
            return FPMgtReport.Service.GetsFPMgtReports(PositionDate, CurrencyID, IsApproved, nUserID);
        }

        #endregion

        #region ServiceFactory
        internal static IFPMgtReportService Service
        {
            get { return (IFPMgtReportService)Services.Factory.CreateService(typeof(IFPMgtReportService)); }
        }
        #endregion
    }
    #endregion

    #region IFPMgtReport interface
    public interface IFPMgtReportService
    {
        List<FPMgtReport> GetsFPMgtReports(DateTime PositionDate, int CurrencyID, bool IsApproved, int nUserID);
    }
    #endregion
}
