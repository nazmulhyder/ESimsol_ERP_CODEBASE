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
    #region Currency
    
    public class Currency : BusinessObject
    {
        public Currency()
        {
            CurrencyID = 0;
            CurrencyName = "";
            IssueFigure = "";
            Symbol = "";
            SmallerUnit = "";
            SmUnitValue = 0;
            Note = "";
            ErrorMessage = "";

        }

        #region Properties
        
        public int CurrencyID { get; set; }
        
        public string CurrencyName { get; set; }
        
        public string IssueFigure { get; set; }
        
        public string Symbol { get; set; }
        
        public string SmallerUnit { get; set; }
        
        public double SmUnitValue { get; set; }
        
        public string Note { get; set; }
        
        public string ErrorMessage { get; set; }
        #endregion

        #region Derived Property        
        
        public int ConvertedToCurrencyID { get; set; }
        
        public int BankID { get; set; }
        public List<CurrencyConversion> ConversionListForSelectedCurrncy { get; set; }
        public List<Currency> ToCurrencys { get; set; }
        public List<Bank> Banks { get; set; }
        public Company MyCompany { get; set; }
        public string NameSymbol
        {
            get
            {
                return this.CurrencyName + "[" + this.Symbol + "]";
            }
        }
        #endregion

        #region Functions

        public static List<Currency> Gets(long nUserID)
        {
            return Currency.Service.Gets(nUserID);
        }
        public static List<Currency> Gets(string sql, long nUserID)
        {
            return Currency.Service.Gets(sql, nUserID);
        }

        public static List<Currency> GetsLeftSelectedCurrency(int nCurrencyID, long nUserID)
        {
            return Currency.Service.GetsLeftSelectedCurrency(nCurrencyID, nUserID);
        }

        public Currency Get(int nId, long nUserID)
        {
            return Currency.Service.Get(nId,nUserID);
        }

        public Currency Save(long nUserID)
        {
            return Currency.Service.Save(this,nUserID);
        }

        public string Delete(int nId, long nUserID)
        {
            return Currency.Service.Delete(nId,nUserID);
        }

        #endregion

        #region ServiceFactory
        internal static ICurrencyService Service
        {
            get { return (ICurrencyService)Services.Factory.CreateService(typeof(ICurrencyService)); }
        }
        #endregion
    }
    #endregion
        
    #region ICurrency interface
    
    public interface ICurrencyService
    {
        
        Currency Get(int id, long nUserID);
        
        List<Currency> GetsLeftSelectedCurrency(int nCurrencyID, long nUserID);
        
        List<Currency> Gets(long nUserID);
        List<Currency> Gets(string sql, long nUserID);

        string Delete(int id, long nUserID);
        
        Currency Save(Currency oCurrency, long nUserID);
    }
    #endregion
}
