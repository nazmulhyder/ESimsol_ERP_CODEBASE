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
    #region TradingSaleInvoice
    public class TradingSaleInvoice
    {
        #region  Constructor
        public TradingSaleInvoice()
        {
            TradingSaleInvoiceID = 0;
            BUID = 0;
            TradingSaleInvoiceStatus = EnumTradingSaleInvoiceStatus.Intialize;
            SalesType = EnumSalesType.None;
            SalesTypeInt = 0;
            InvoiceNo = "";
            InvoiceDate = DateTime.Now;
            BuyerID = 0;
            ContactPersonID = 0;
            CurrencyID = 0;
            Note = "";
            ApprovedBy = 0;
            GrossAmount = 0;
            DiscountAmount = 0;
            VatAmount = 0;
            ServiceCharge = 0;
            CommissionAmount = 0;
            NetAmount = 0;
            BUName = "";
            BuyerName = "";
            BuyerAddress = "";
            BuyerPhone = "";
            ContactPersonName = "";
            ContactPersonPhone = "";
            CurrencyName = "";
            CurrencySymbol = "";
            ApprovedByName = "";
            YetToChallanQty = 0;
            DueAmount = 0;
            CardNo = "";
            CardPaid = 0;
            CashPaid = 0;
            ATMCardID = 0;
            ErrorMessage = "";
            IsChallanExist = false;
            TradingSaleInvoiceDetails = new List<TradingSaleInvoiceDetail>();
            Currencys = new List<Currency>();
            SalesTypeObjs = new List<SalesTypeObj>();
            ReviseRequest = new BusinessObjects.ReviseRequest();
            TradingSaleInvoiceLogID = 0;
        }
        #endregion

        #region Properties
        public int TradingSaleInvoiceID { get; set; }
        public int BUID { get; set; }
        public EnumTradingSaleInvoiceStatus TradingSaleInvoiceStatus { get; set; }
        public EnumSalesType SalesType { get; set; }
        public int SalesTypeInt { get; set; }
        public string InvoiceNo { get; set; }
        public DateTime InvoiceDate { get; set; }
        public int BuyerID { get; set; }
        public Boolean IsChallanExist { get; set; }
        public int ContactPersonID { get; set; }
        public int CurrencyID { get; set; }
        public string Note { get; set; }
        public int ApprovedBy { get; set; }
        public double GrossAmount { get; set; }
        public double DiscountAmount { get; set; }
        public double VatAmount { get; set; }
        public double ServiceCharge { get; set; }
        public double CommissionAmount { get; set; }
        public double NetAmount { get; set; }
        public string BUName { get; set; }
        public string BuyerName { get; set; }
        public string BuyerAddress { get; set; }
        public string BuyerPhone { get; set; }
        public string ContactPersonName { get; set; }
        public string ContactPersonPhone { get; set; }
        public string CurrencyName { get; set; }
        public string CurrencySymbol { get; set; }
        public string ApprovedByName { get; set; }
        public double YetToChallanQty { get; set; }
        public double DueAmount { get; set; }

        public string CardNo { get; set; }
        public double CardPaid { get; set; }
        public double CashPaid { get; set; }
        public int ATMCardID { get; set; }
        public string ErrorMessage { get; set; }

        public string SalesmanName { get; set; }
        public ReviseRequest ReviseRequest { get; set; }
        public int TradingSaleInvoiceLogID { get; set; }
        #endregion

        #region Derived Property
        public List<TradingSaleInvoiceDetail> TradingSaleInvoiceDetails { get; set; }
        public List<Currency> Currencys { get; set; }
        public List<SalesTypeObj> SalesTypeObjs { get; set; }
        public string TradingSaleInvoiceStatusSt
        {
            get
            {
                return this.TradingSaleInvoiceStatus.ToString();
            }
        }
        public string SalesTypeSt
        {
            get
            {
                return SalesTypeObj.GetStringSalesType(this.SalesType);
            }
        }
        public string InvoiceDateInString
        {
            get
            {
                return this.InvoiceDate.ToString("dd MMM yyyy");
            }
        }
        public double DiscountAmountInPercent
        {
            get
            {
                if (this.DiscountAmount > 0)
                {
                    return Math.Round(((this.DiscountAmount * 100) / this.GrossAmount), 4);
                }
                else
                {
                    return 0.00;
                }
            }
        }
        public double VatAmountInPercent
        {
            get
            {
                if (this.VatAmount > 0)
                {
                    return Math.Round(((this.VatAmount * 100) / this.GrossAmount), 4);
                }
                else
                {
                    return 0.00;
                }
            }
        }
        public double ServiceChargeInPercent
        {
            get
            {
                if (this.ServiceCharge > 0)
                {
                    return Math.Round(((this.ServiceCharge * 100) / this.GrossAmount), 4);
                }
                else
                {
                    return 0.00;
                }
            }
        }
        #endregion

        #region Functions
        public TradingSaleInvoice Get(int nTradingSaleInvoiceID, int nUserID)
        {
            return TradingSaleInvoice.Service.Get(nTradingSaleInvoiceID, nUserID);
        }
        public TradingSaleInvoice GetLog(int nTradingSaleInvoiceLogID, int nUserID)
        {
            return TradingSaleInvoice.Service.GetLog(nTradingSaleInvoiceLogID, nUserID);
        }
        public TradingSaleInvoice Save(int nUserID)
        {
            return TradingSaleInvoice.Service.Save(this, nUserID);
        }

        public TradingSaleInvoice Approved(int nUserID)
        {
            return TradingSaleInvoice.Service.Approved(this, nUserID);
        }
        public TradingSaleInvoice UndoApproved(int nUserID)
        {
            return TradingSaleInvoice.Service.UndoApproved(this, nUserID);
        }
        public string Delete(int nUserID)
        {
            return TradingSaleInvoice.Service.Delete(this, nUserID);
        }
        public static List<TradingSaleInvoice> Gets(int nUserID)
        {
            return TradingSaleInvoice.Service.Gets(nUserID);
        }
        public static List<TradingSaleInvoice> Gets(string sSQl, int nUserID)
        {
            return TradingSaleInvoice.Service.Gets(sSQl, nUserID);
        }
        public static List<TradingSaleInvoice> GetsInitialInvoices(int nBUID, int nUserID)
        {
            return TradingSaleInvoice.Service.GetsInitialInvoices(nBUID, nUserID);
        }
        public TradingSaleInvoice RequestRevise(long nUserID)
        {
            return TradingSaleInvoice.Service.RequestRevise(this, nUserID);
        }
        public TradingSaleInvoice AcceptRevise(int nUserID)
        {
            return TradingSaleInvoice.Service.AcceptRevise(this, nUserID);
        }
        #endregion

        #region Non DB Function
        public string IDInString(List<TradingSaleInvoice> oTradingSaleInvoices)
        {
            string sReturn = "";
            foreach (TradingSaleInvoice oItem in oTradingSaleInvoices)
            {
                sReturn = sReturn + oItem.TradingSaleInvoiceID.ToString() + ",";
            }
            if (sReturn.Length > 0) sReturn = sReturn.Remove(sReturn.Length - 1, 1);
            return sReturn;
        }
        #endregion

        #region ServiceFactory
        internal static ITradingSaleInvoiceService Service
        {
            get { return (ITradingSaleInvoiceService)Services.Factory.CreateService(typeof(ITradingSaleInvoiceService)); }
        }
        #endregion
    }
    #endregion

    #region ITradingSaleInvoice interface
    public interface ITradingSaleInvoiceService
    {
        TradingSaleInvoice Get(int nTradingSaleInvoiceID, int nUserID);
        TradingSaleInvoice GetLog(int nTradingSaleInvoiceLogID, int nUserID);
        TradingSaleInvoice Save(TradingSaleInvoice oTradingSaleInvoice, int nUserID);
        TradingSaleInvoice Approved(TradingSaleInvoice oTradingSaleInvoice, int nUserID);
        TradingSaleInvoice UndoApproved(TradingSaleInvoice oTradingSaleInvoice, int nUserID);
        string Delete(TradingSaleInvoice oTradingSaleInvoice, int nUserID);
        List<TradingSaleInvoice> Gets(int nUserID);
        List<TradingSaleInvoice> Gets(string sSQl, int nUserID);
        List<TradingSaleInvoice> GetsInitialInvoices(int nBUID, int nUserID);
        TradingSaleInvoice RequestRevise(TradingSaleInvoice oTradingSaleInvoice, Int64 nUserID);
        TradingSaleInvoice AcceptRevise(TradingSaleInvoice oTradingSaleInvoice, int nUserID);
    }
    #endregion
}
