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
    #region TradingDeliveryChallan
    public class TradingDeliveryChallan
    {
        #region  Constructor
        public TradingDeliveryChallan()
        {
            TradingDeliveryChallanID = 0;
            BUID = 0;
            TradingSaleInvoiceID = 0;
            ChallanNo = "";
            ChallanDate = DateTime.Now;
            BuyerID = 0;
            StoreID = 0;
            CurrencyID = 0;
            NetAmount = 0;
            Note = "";
            DeliveryBy = 0;
            DeliveryByName = "";
            BUName = "";
            BuyerName = "";
            StoreName = "";
            CurrencyName = "";
            CurrencySymbol = "";
            InvoiceNo = "";
            TotalQty = 0;
            ErrorMessage = "";
            WorkingUnits = new List<WorkingUnit>();
            TradingSaleInvoice = new TradingSaleInvoice();
        }
        #endregion

        #region Properties
        public int TradingDeliveryChallanID { get; set; }
        public int BUID { get; set; }
        public int TradingSaleInvoiceID { get; set; }
        public string ChallanNo { get; set; }
        public DateTime ChallanDate { get; set; }
        public int BuyerID { get; set; }
        public int StoreID { get; set; }
        public int CurrencyID { get; set; }
        public double NetAmount { get; set; }
        public double TotalQty { get; set; }
        public string Note { get; set; }
        public int DeliveryBy { get; set; }
        public string DeliveryByName { get; set; }
        public string BUName { get; set; }
        public string BuyerName { get; set; }
        public string StoreName { get; set; }
        public string CurrencyName { get; set; }
        public string CurrencySymbol { get; set; }

        public string InvoiceNo { get; set; }
        public string ErrorMessage { get; set; }
        #endregion

        #region Derived Property
        public List<TradingDeliveryChallanDetail> TradingDeliveryChallanDetails { get; set; }
        public List<WorkingUnit> WorkingUnits { get; set; }
        public List<Company> Companys { get; set; }
        public TradingSaleInvoice TradingSaleInvoice { get; set; }
        public string ChallanDateST
        {
            get
            {
                return this.ChallanDate.ToString("dd MMM yyyy");
            }
        }
        #endregion

        #region Functions
        public TradingDeliveryChallan Get(int nTradingDeliveryChallanID, int nUserID)
        {
            return TradingDeliveryChallan.Service.Get(nTradingDeliveryChallanID, nUserID);
        }
        public TradingDeliveryChallan Save(int nUserID)
        {
            return TradingDeliveryChallan.Service.Save(this, nUserID);
        }
        public TradingDeliveryChallan Disburse(int nUserID)
        {
            return TradingDeliveryChallan.Service.Disburse(this, nUserID);
        }
        public string Delete(int nUserID)
        {
            return TradingDeliveryChallan.Service.Delete(this, nUserID);
        }
        public static List<TradingDeliveryChallan> Gets(int nUserID)
        {
            return TradingDeliveryChallan.Service.Gets(nUserID);
        }
        public static List<TradingDeliveryChallan> GetsInitialTradingDeliveryChallans(int nBUID, int nUserID)
        {
            return TradingDeliveryChallan.Service.GetsInitialTradingDeliveryChallans(nBUID, nUserID);
        }
        public static List<TradingDeliveryChallan> Gets(string sSQl, int nUserID)
        {
            return TradingDeliveryChallan.Service.Gets(sSQl, nUserID);
        }
        #endregion

        #region Non DB Function
        public string IDInString(List<TradingDeliveryChallan> oTradingDeliveryChallans)
        {
            string sReturn = "";
            foreach (TradingDeliveryChallan oItem in oTradingDeliveryChallans)
            {
                sReturn = sReturn + oItem.TradingDeliveryChallanID.ToString() + ",";
            }
            if (sReturn.Length > 0) sReturn = sReturn.Remove(sReturn.Length - 1, 1);
            return sReturn;
        }
        #endregion

        #region ServiceFactory
        internal static ITradingDeliveryChallanService Service
        {
            get { return (ITradingDeliveryChallanService)Services.Factory.CreateService(typeof(ITradingDeliveryChallanService)); }
        }
        #endregion
    }
    #endregion

    #region ITradingDeliveryChallan interface
    public interface ITradingDeliveryChallanService
    {
        TradingDeliveryChallan Get(int nTradingDeliveryChallanID, int nUserID);
        TradingDeliveryChallan Save(TradingDeliveryChallan oTradingDeliveryChallan, int nUserID);
        TradingDeliveryChallan Disburse(TradingDeliveryChallan oTradingDeliveryChallan, int nUserID);
        string Delete(TradingDeliveryChallan oTradingDeliveryChallan, int nUserID);
        List<TradingDeliveryChallan> Gets(int nUserID);
        List<TradingDeliveryChallan> Gets(string sSQl, int nUserID);
        List<TradingDeliveryChallan> GetsInitialTradingDeliveryChallans(int nBUID, int nUserID);
    }
    #endregion
}
