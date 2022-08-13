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
    #region CurrencyConversion
    
    public class CurrencyConversion : BusinessObject
    {
        public CurrencyConversion()
        {
            CurrencyConversionID = 0;
            CurrencyID = 0;
            ToCurrencyID = 0;
            BankID = 0;
            RateSale = 0;
            RatePurchase = 0;
            IsOpen = true;
            Date = DateTime.Now;
            Note = "";
            ErrorMessage = "";
        }

        #region Properties
        public int CurrencyConversionID { get; set; }
        public int CurrencyID { get; set; }
        public int ToCurrencyID { get; set; }
        public int BankID { get; set; }
        public double RateSale { get; set; }
        public double RatePurchase { get; set; }
        public DateTime Date { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string Note { get; set; }
        public string BankCode { get; set; }
        public string BankName { get; set; }
        public string CurrencyName { get; set; }
        public string Currency { get; set; }
        public string ToCurrencyName { get; set; }
        public string ToCurrency { get; set; }
        public bool IsOpen { get; set; }
        #endregion

        #region Derived Property
        public string IsOpenSt
        {
            get
            {
                if(IsOpen)
                    return "Open";
                else
                    return "Bank";
            }
        }
        public string ErrorMessage { get; set; }
        public string DateSt
        {
            get
            {
                return this.Date.ToString("dd MMM yyyy");
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
                if (EndDate == DateTime.MinValue)
                    return "-";
                else
                    return this.EndDate.ToString("dd MMM yyyy");
            }
        }
        public string ConversionRate
        {
            get
            {
                return this.Currency +  "1 -- " + this.ToCurrency + this.RatePurchase;
            } 
        }
        public string ConversionRateRateSale
        {
            get
            {
                return this.Currency + "1 -- " + this.ToCurrency + this.RateSale;
            }
        }
        #endregion

        #region Functions

        public static List<CurrencyConversion> Gets(long nUserID)
        {
            return CurrencyConversion.Service.Gets(nUserID);
        }
        public static List<CurrencyConversion> GetsByFromCurrency(int nID, long nUserID)
        {
            return CurrencyConversion.Service.GetsByFromCurrency(nID,nUserID);
        }
        public CurrencyConversion GetsLastConversionRate(int nFromCurrencyID, int nToCurrencyID, long nUserID)
        {
            return CurrencyConversion.Service.GetsLastConversionRate(nFromCurrencyID, nToCurrencyID,nUserID);
        }
        public CurrencyConversion Get(int nId, long nUserID)
        {
            return CurrencyConversion.Service.Get(nId,nUserID);
        }
        public CurrencyConversion Save(long nUserID)
        {
            return CurrencyConversion.Service.Save(this, nUserID);
        }
        public string Delete(int nId, long nUserID)
        {
            return CurrencyConversion.Service.Delete(nId, nUserID);
        }
        #endregion

        #region ServiceFactory
        internal static ICurrencyConversionService Service
        {
            get { return (ICurrencyConversionService)Services.Factory.CreateService(typeof(ICurrencyConversionService)); }
        }
        #endregion
    }
    #endregion
       
    #region ICurrencyConversion interface
    
    public interface ICurrencyConversionService
    {
        CurrencyConversion Get(int id, long nUserID);
        List<CurrencyConversion> Gets(long nUserID);
        List<CurrencyConversion> GetsByFromCurrency(int nID, long nUserID);
        CurrencyConversion GetsLastConversionRate(int nFromCurrencyID, int nToCurrencyID, long nUserID);
        string Delete(int id, long nUserID);
        CurrencyConversion Save(CurrencyConversion oCurrencyConversion, long nUserID);
    }
    #endregion
}
