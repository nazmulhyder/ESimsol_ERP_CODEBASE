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
    #region CostSetup

    public class CostSetup : BusinessObject
    {
        public CostSetup()
        {
            CostSetupID=0;
            CustomsDuty=0;
            RegulatoryDuty=0;
            SupplementaryDuty=0;
            ValueAddedTxt=0;
            AdvanceIncomeTax=0;
            AdvanceTradeVat=0;
            ATVDeductedProfit=0;
            CustomClearingAndInsuranceFee=0;
            MarginRate = 0;
            CurrencyRate = 0;
      }

        #region Properties
        public int CostSetupID { get; set; }
        public double CustomsDuty { get; set; }
        public double RegulatoryDuty { get; set; }
        public double SupplementaryDuty { get; set; }
        public double ValueAddedTxt { get; set; }
        public double AdvanceIncomeTax { get; set; }
        public double AdvanceTradeVat { get; set; }
        public double ATVDeductedProfit { get; set; }
        public double CustomClearingAndInsuranceFee { get; set; }
        public double MarginRate { get; set; }
        public double CurrencyRate { get; set; }
        public string ErrorMessage { get; set; }
        #endregion

        #region Derived Property
        public string Param { get; set; }
      
        #endregion

        #region Functions

        public static List<CostSetup> Gets(long nUserID)
        {
            return CostSetup.Service.Gets(nUserID);
        }
        public static List<CostSetup> Gets(string sSQL, Int64 nUserID)
        {
            return CostSetup.Service.Gets(sSQL, nUserID);
        }

        public CostSetup Get(int nId, long nUserID)
        {
            return CostSetup.Service.Get(nId, nUserID);
        }

        public CostSetup Save(long nUserID)
        {
            return CostSetup.Service.Save(this, nUserID);
        }

        public string Delete(int nId, long nUserID)
        {
            return CostSetup.Service.Delete(nId, nUserID);
        }
     
        #endregion

        #region ServiceFactory
        internal static ICostSetupService Service
        {
            get { return (ICostSetupService)Services.Factory.CreateService(typeof(ICostSetupService)); }
        }
        #endregion
    }
    #endregion

    #region ICostSetup interface

    public interface ICostSetupService
    {
        CostSetup Get(int id, long nUserID);
        List<CostSetup> Gets(long nUserID);
        List<CostSetup> Gets(string sSQL, Int64 nUserID);
        string Delete(int id, long nUserID);
        CostSetup Save(CostSetup oCostSetup, long nUserID);
    }
    #endregion
}