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
    #region SP_RatioSetup
    public class SP_RatioSetup : BusinessObject
    {
        public SP_RatioSetup()
        {
            AccountingRatioSetupID = 0;
            Name = "";
            RatioFormat = EnumRatioFormat.None;
            DivisibleName = "";
            DividerName = "";
            DivisibleAmount = 0;
            DividerAmount = 0;
            RatioBalance = 0;
            BusinessUnitID = 0;
            RatioSetupType = EnumRatioSetupType.GenrealSetup;
            StartDate = DateTime.Now;
            EndDate = DateTime.Now;
            IsForCurrentDate = false;
            DivisibleComponent = 0;
            DividerComponent = 0;
            BUID = 0;
            SP_RatioSetups = new List<SP_RatioSetup>();

            ErrorMessage = "";
        }
        #region Properties
        public int AccountingRatioSetupID { get; set; }
        public string Name { get; set; }
        public EnumRatioFormat RatioFormat { get; set; }
        public string DivisibleName { get; set; }
        public string DividerName { get; set; }
        public Double DivisibleAmount { get; set; }
        public Double DividerAmount { get; set; }
        public Double RatioBalance { get; set; }
        public int BusinessUnitID { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public bool IsForCurrentDate { get; set; }
        public EnumRatioSetupType RatioSetupType { get; set; }
        public int DivisibleComponent { get; set; }
        public int DividerComponent { get; set; }
        public int BUID { get; set; }
        public string ErrorMessage { get; set; }
        #endregion

        #region Derived Property
        public List<SP_RatioSetup> SP_RatioSetups { get; set; }
        public string StartDateSt { get { return this.StartDate.ToString("dd MMM yyyy"); } }
        public string EndDateSt { get { return this.EndDate.ToString("dd MMM yyyy"); } }
        public string PercentageSt { get { return this.RatioFormat == EnumRatioFormat.Percentage && this.RatioBalance > 0 ? "X 100 = " + this.RatioBalanceSt + " X 100 = " + Global.MillionFormat(this.RatioBalance * 100) + "% " : " 0% "; } }
        public string DivisibleAmountSt { get { return Global.MillionFormat(this.DivisibleAmount); } }
        public string DividerAmountSt { get { return Global.MillionFormat(this.DividerAmount); } }
        public string RatioBalanceSt { get { return Global.MillionFormat(this.RatioBalance); } }
        public string RatioSt { get { return this.RatioFormat == EnumRatioFormat.Ratio && this.RatioBalance > 0 ? " = " + this.RatioBalanceSt + " : 1" : " = 0:0 "; } }
        public string RatioFormatSt { get { return EnumObject.jGet(this.RatioFormat); } }
        public string Separator { get { return this.RatioFormat == EnumRatioFormat.Percentage ? "X 100 = " : " = "; } }
        #endregion

        #region Functions


        public static List<SP_RatioSetup> Gets(int nAccountingRatioSetupID, DateTime dStartDate, DateTime dEndDate, int nBusinessUnitID, int nUserID)
        {
            return SP_RatioSetup.Service.Gets(nAccountingRatioSetupID, dStartDate, dEndDate, nBusinessUnitID, nUserID);
        }

        #endregion


        #region ServiceFactory
        internal static ISP_RatioSetupService Service
        {
            get { return (ISP_RatioSetupService)Services.Factory.CreateService(typeof(ISP_RatioSetupService)); }
        }
        #endregion
    }
    #endregion



    #region ISP_RatioSetup interface
    public interface ISP_RatioSetupService
    {

        List<SP_RatioSetup> Gets(int nAccountingRatioSetupID, DateTime dStartDate, DateTime dEndDate, int nBusinessUnitID, int nUserID);


    }
    #endregion


}