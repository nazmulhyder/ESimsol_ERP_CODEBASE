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
    #region TradingSaleReturn
    public class TradingSaleReturn
    {
        #region  Constructor
        public TradingSaleReturn()
        {
            TradingSaleReturnID = 0;
            BUID = 0;
            ReturnNo = "";
            ReturnDate = DateTime.Now;
            BuyerID = 0;
            ContactPersonID = 0;
            CurrencyID = 0;            
            Note = "";
            ApprovedBy = 0;
            GrossAmount = 0;
            PaymentAmount = 0;
            StoreID = 0;
            BUName = "";
            BuyerName = "";
            BuyerAddress = "";
            BuyerPhone = "";
            ContactPersonName = "";
            ContactPersonPhone = "";
            CurrencyName = "";
            CurrencySymbol = "";            
            ApprovedByName = "";
            StoreName = "";
            DueAmount = 0;
            ErrorMessage = "";
            TradingSaleReturnDetails = new List<TradingSaleReturnDetail>();
            Currencys = new List<Currency>();
        }
        #endregion

        #region Properties
        public int TradingSaleReturnID { get; set; }
        public int BUID { get; set; }
        public string ReturnNo { get; set; }
        public DateTime ReturnDate { get; set; }
        public int BuyerID { get; set; }
        public int ContactPersonID { get; set; }
        public int CurrencyID { get; set; }        
        public string Note { get; set; }
        public int ApprovedBy { get; set; }
        public double GrossAmount { get; set; }
        public double PaymentAmount { get; set; }
        public int StoreID { get; set; }
        public string BUName { get; set; }
        public string BuyerName { get; set; }
        public string BuyerAddress { get; set; }
        public string BuyerPhone { get; set; }
        public string ContactPersonName { get; set; }
        public string ContactPersonPhone { get; set; }
        public string CurrencyName { get; set; }
        public string CurrencySymbol { get; set; }
        public string ApprovedByName { get; set; }
        public string StoreName { get; set; }
        public double DueAmount { get; set; }
        public string ErrorMessage { get; set; }

        public string SalesmanName { get; set; }
        #endregion

        #region Derived Property
        public List<TradingSaleReturnDetail> TradingSaleReturnDetails { get; set; }        
        public List<Currency> Currencys { get; set; }
  
        public string ReturnDateInString
        {
            get
            {
                return this.ReturnDate.ToString("dd MMM yyyy");
            }
        }      
        #endregion

        #region Functions
        public TradingSaleReturn Get(int nTradingSaleReturnID, int nUserID)
        {
            return TradingSaleReturn.Service.Get(nTradingSaleReturnID, nUserID);
        }
        public TradingSaleReturn Save(int nUserID)
        {
            return TradingSaleReturn.Service.Save(this, nUserID);
        }

        public TradingSaleReturn Approved(int nUserID)
        {
            return TradingSaleReturn.Service.Approved(this, nUserID);
        }

        public string Delete(int nUserID)
        {
            return TradingSaleReturn.Service.Delete(this, nUserID);
        }
        public static List<TradingSaleReturn> Gets(int nUserID)
        {
            return TradingSaleReturn.Service.Gets(nUserID);
        }
        public static List<TradingSaleReturn> Gets(string sSQl, int nUserID)
        {
            return TradingSaleReturn.Service.Gets(sSQl, nUserID);
        }
        public static List<TradingSaleReturn> GetsInitialReturns(int nBUID, int nUserID)
        {
            return TradingSaleReturn.Service.GetsInitialReturns(nBUID, nUserID);
        }
        #endregion

        #region Non DB Function
        public string IDInString(List<TradingSaleReturn> oTradingSaleReturns)
        {
            string sReturn = "";
            foreach (TradingSaleReturn oItem in oTradingSaleReturns)
            {
                sReturn = sReturn + oItem.TradingSaleReturnID.ToString() + ",";
            }
            if (sReturn.Length > 0) sReturn = sReturn.Remove(sReturn.Length - 1, 1);
            return sReturn;
        }
        #endregion

        #region ServiceFactory
        internal static ITradingSaleReturnService Service
        {
            get { return (ITradingSaleReturnService)Services.Factory.CreateService(typeof(ITradingSaleReturnService)); }
        }
        #endregion
    }
    #endregion

    #region ITradingSaleReturn interface
    public interface ITradingSaleReturnService
    {
        TradingSaleReturn Get(int nTradingSaleReturnID, int nUserID);
        TradingSaleReturn Save(TradingSaleReturn oTradingSaleReturn, int nUserID);
        TradingSaleReturn Approved(TradingSaleReturn oTradingSaleReturn, int nUserID);
        string Delete(TradingSaleReturn oTradingSaleReturn, int nUserID);        
        List<TradingSaleReturn> Gets(int nUserID);       
        List<TradingSaleReturn> Gets(string sSQl, int nUserID);
        List<TradingSaleReturn> GetsInitialReturns(int nBUID, int nUserID);
    }
    #endregion
}

