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
    #region TradingSaleReturnDetail
    public class TradingSaleReturnDetail
    {
        #region  Constructor
        public TradingSaleReturnDetail()
        {
            TradingSaleReturnDetailID = 0;
            TradingSaleReturnID = 0;
            ProductID = 0;
            ItemDescription = "";
            MeasurementUnitID = 0;
            ReturnQty = 0;
            UnitPrice = 0;
            Amount = 0;
            ProductCode = "";
            ProductName = "";
            UnitName = "";
            Symbol = "";
            ProductCategoryName = "";
            ErrorMessage = "";
            LotNo = "";
        }
        #endregion

        #region Properties
        public int TradingSaleReturnDetailID { get; set; }
        public int TradingSaleReturnID { get; set; }
        public int ProductID { get; set; }
        public string ItemDescription { get; set; }
        public int MeasurementUnitID { get; set; }
        public double ReturnQty { get; set; }
        public double UnitPrice { get; set; }
        public double Amount { get; set; }
        public string ProductCode { get; set; }
        public string ProductName { get; set; }
        public string UnitName { get; set; }
        public string Symbol { get; set; }
        public string ProductCategoryName { get; set; }
        public string ErrorMessage { get; set; }
        #endregion

        #region Derived Property
        public string LotNo { get; set; }

        #endregion

        #region Functions
        public TradingSaleReturnDetail Get(int nTradingSaleReturnDetailID, int nUserID)
        {
            return TradingSaleReturnDetail.Service.Get(nTradingSaleReturnDetailID, nUserID);
        }
        public TradingSaleReturnDetail Save(int nUserID)
        {
            return TradingSaleReturnDetail.Service.Save(this, nUserID);            
        }
        public string Delete(int nUserID)
        {
            return TradingSaleReturnDetail.Service.Delete(this, nUserID);            
        }
        public static List<TradingSaleReturnDetail> Gets(int nUserID)
        {
            return TradingSaleReturnDetail.Service.Gets(nUserID);            
        }
        public static List<TradingSaleReturnDetail> Gets(int nSaleReturnID, int nUserID)
        {
            return TradingSaleReturnDetail.Service.Gets(nSaleReturnID, nUserID);            
        }
        public static List<TradingSaleReturnDetail> Gets(string sSQl, int nUserID)
        {
            return TradingSaleReturnDetail.Service.Gets(sSQl, nUserID);            
        }
        #endregion

        #region Non DB Function
        public string IDInString(List<TradingSaleReturnDetail> oTradingSaleReturnDetails)
        {
            string sReturn = "";
            foreach (TradingSaleReturnDetail oItem in oTradingSaleReturnDetails)
            {
                sReturn = sReturn + oItem.TradingSaleReturnDetailID.ToString() + ",";
            }
            if (sReturn.Length > 0) sReturn = sReturn.Remove(sReturn.Length - 1, 1);
            return sReturn;
        }
        #endregion

        #region ServiceFactory
        internal static ITradingSaleReturnDetailService Service
        {
            get { return (ITradingSaleReturnDetailService)Services.Factory.CreateService(typeof(ITradingSaleReturnDetailService)); }
        }
        #endregion
    }
    #endregion

    #region ITradingSaleReturnDetail interface
    public interface ITradingSaleReturnDetailService
    {
        TradingSaleReturnDetail Get(int id, int nUserID);
        List<TradingSaleReturnDetail> Gets(int nUserID);
        List<TradingSaleReturnDetail> Gets(string sSQL, int nUserID);
        List<TradingSaleReturnDetail> Gets(int nSaleReturnID, int nUserID);
        TradingSaleReturnDetail Save(TradingSaleReturnDetail oTradingSaleReturnDetail, int nUserID);
        string Delete(TradingSaleReturnDetail oTradingSaleReturnDetail, int nUserID);
    }
    #endregion
}
