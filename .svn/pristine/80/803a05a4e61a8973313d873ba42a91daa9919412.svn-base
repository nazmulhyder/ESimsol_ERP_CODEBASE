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
    #region TradingDeliveryChallanDetail
    public class TradingDeliveryChallanDetail
    {
        #region  Constructor
        public TradingDeliveryChallanDetail()
        {
            TradingDeliveryChallanDetailID = 0;
            TradingDeliveryChallanID = 0;
            TradingSaleInvoiceDetailID = 0;
            ProductID = 0;
            ItemDescription = "";
            UnitID = 0;
            LotID = 0;
            LotNo = "";
            LotBalance = 0;
            ChallanQty = 0;
            UnitPrice = 0;
            Amount = 0;
            ProductCode = "";
            ProductName = "";
            UnitName = "";
            Symbol = "";
            YetToChallanQty = 0;
            ErrorMessage = "";
        }
        #endregion

        #region Properties
        public int TradingDeliveryChallanDetailID { get; set; }
        public int TradingDeliveryChallanID { get; set; }
        public int TradingSaleInvoiceDetailID { get; set; }
        public int ProductID { get; set; }
        public string ItemDescription { get; set; }
        public int UnitID { get; set; }
        public int LotID { get; set; }
        public String LotNo { get; set; }
        public double LotBalance { get; set; }
        public double ChallanQty { get; set; }
        public double UnitPrice { get; set; }
        public double Amount { get; set; }
        public string ProductCode { get; set; }
        public string ProductName { get; set; }
        public string UnitName { get; set; }
        public string Symbol { get; set; }
        public double YetToChallanQty { get; set; }
        public string ErrorMessage { get; set; }
        #endregion

        #region Derived Property

        #endregion

        #region Functions
        public TradingDeliveryChallanDetail Get(int nTradingDeliveryChallanDetailID, int nUserID)
        {
            return TradingDeliveryChallanDetail.Service.Get(nTradingDeliveryChallanDetailID, nUserID);
        }
        public TradingDeliveryChallanDetail Save(int nUserID)
        {
            return TradingDeliveryChallanDetail.Service.Save(this, nUserID);
        }

        public string Delete(int nUserID)
        {
            return TradingDeliveryChallanDetail.Service.Delete(this, nUserID);
        }

        public static List<TradingDeliveryChallanDetail> Gets(int nUserID)
        {
            return TradingDeliveryChallanDetail.Service.Gets(nUserID);
        }
        public static List<TradingDeliveryChallanDetail> Gets(int nTradingDeliveryChallanID, int nUserID)
        {
            return TradingDeliveryChallanDetail.Service.Gets(nTradingDeliveryChallanID, nUserID);
        }
        public static List<TradingDeliveryChallanDetail> Gets(string sSQl, int nUserID)
        {
            return TradingDeliveryChallanDetail.Service.Gets(sSQl, nUserID);
        }
        #endregion

        #region Non DB Function
        public string IDInString(List<TradingDeliveryChallanDetail> oTradingDeliveryChallanDetails)
        {
            string sReturn = "";
            foreach (TradingDeliveryChallanDetail oItem in oTradingDeliveryChallanDetails)
            {
                sReturn = sReturn + oItem.TradingDeliveryChallanDetailID.ToString() + ",";
            }
            if (sReturn.Length > 0) sReturn = sReturn.Remove(sReturn.Length - 1, 1);
            return sReturn;
        }
        #endregion

        #region ServiceFactory
        internal static ITradingDeliveryChallanDetailService Service
        {
            get { return (ITradingDeliveryChallanDetailService)Services.Factory.CreateService(typeof(ITradingDeliveryChallanDetailService)); }
        }
        #endregion
    }
    #endregion

    #region ITradingDeliveryChallanDetail interface
    public interface ITradingDeliveryChallanDetailService
    {
        TradingDeliveryChallanDetail Get(int nTradingDeliveryChallanDetailID, int nUserID);
        TradingDeliveryChallanDetail Save(TradingDeliveryChallanDetail oTradingDeliveryChallanDetail, int nUserID);
        string Delete(TradingDeliveryChallanDetail oTradingDeliveryChallanDetail, int nUserID);
        List<TradingDeliveryChallanDetail> Gets(int nUserID);
        List<TradingDeliveryChallanDetail> Gets(int nBUID, int nUserID);
        List<TradingDeliveryChallanDetail> Gets(string sSQl, int nUserID);
    }
    #endregion
}
