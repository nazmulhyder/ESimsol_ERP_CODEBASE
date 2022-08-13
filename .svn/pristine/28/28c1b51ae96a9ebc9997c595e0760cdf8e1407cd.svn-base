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
    #region TradingPaymentDetail
    public class TradingPaymentDetail
    {
        #region  Constructor
        public TradingPaymentDetail()
        {
            TradingPaymentDetailID = 0;
            TradingPaymentID = 0;
            ReferenceID = 0;
            Amount = 0;
            Remarks = "";
            InvoiceNo = "";
            InvoiceDate = DateTime.Today;
            InvoiceAmount = 0;
            ReferenceType = EnumPaymentRefType.None;
            ReferenceTypeInt = 0;
            YetToPayment = 0;
            ErrorMessage = "";

        }
        #endregion

        #region Properties
        public int TradingPaymentDetailID { get; set; }
        public int TradingPaymentID { get; set; }
        public int ReferenceID { get; set; }
        public double Amount { get; set; }
        public string Remarks { get; set; }
        public string InvoiceNo { get; set; }
        public DateTime InvoiceDate { get; set; }
        public double InvoiceAmount { get; set; }
        public EnumPaymentRefType ReferenceType { get; set; }
        public int ReferenceTypeInt { get; set; }
        public double YetToPayment { get; set; }
        public string ErrorMessage { get; set; }
        #endregion

        #region Derived Property
        public string InvoiceDateSt
        {
            get
            {
                return this.InvoiceDate.ToString("dd MMM yyyy");
            }
        }
        public string InvoiceAmountSt
        {
            get
            {
                return Global.TakaFormat(this.InvoiceAmount);
            }
        }
        public string YetToPaymentSt
        {
            get
            {
                return Global.TakaFormat(this.YetToPayment);
            }
        }

        #endregion

        #region Functions
        public TradingPaymentDetail Get(int nTradingPaymentDetailID, int nUserID)
        {
            return TradingPaymentDetail.Service.Get(nTradingPaymentDetailID, nUserID);
        }
        public TradingPaymentDetail Save(int nUserID)
        {
            return TradingPaymentDetail.Service.Save(this, nUserID);
        }
        public string Delete(int nUserID)
        {
            return TradingPaymentDetail.Service.Delete(this, nUserID);
        }
        public static List<TradingPaymentDetail> Gets(int nUserID)
        {
            return TradingPaymentDetail.Service.Gets(nUserID);
        }
        public static List<TradingPaymentDetail> Gets(int nTradingPaymentID, int nUserID)
        {
            return TradingPaymentDetail.Service.Gets(nTradingPaymentID, nUserID);
        }
        public static List<TradingPaymentDetail> Gets(string sSQl, int nUserID)
        {
            return TradingPaymentDetail.Service.Gets(sSQl, nUserID);
        }
        #endregion

        #region Non DB Function
        public string IDInString(List<TradingPaymentDetail> oTradingPaymentDetails)
        {
            string sReturn = "";
            foreach (TradingPaymentDetail oItem in oTradingPaymentDetails)
            {
                sReturn = sReturn + oItem.TradingPaymentDetailID.ToString() + ",";
            }
            if (sReturn.Length > 0) sReturn = sReturn.Remove(sReturn.Length - 1, 1);
            return sReturn;
        }
        #endregion

        #region ServiceFactory
        internal static ITradingPaymentDetailService Service
        {
            get { return (ITradingPaymentDetailService)Services.Factory.CreateService(typeof(ITradingPaymentDetailService)); }
        }
        #endregion
    }
    #endregion

    #region ITradingPaymentDetail interface
    public interface ITradingPaymentDetailService
    {
        TradingPaymentDetail Get(int id, int nUserID);
        List<TradingPaymentDetail> Gets(int nUserID);
        List<TradingPaymentDetail> Gets(string sSQL, int nUserID);
        List<TradingPaymentDetail> Gets(int nTradingPaymentID, int nUserID);
        TradingPaymentDetail Save(TradingPaymentDetail oTradingPaymentDetail, int nUserID);
        string Delete(TradingPaymentDetail oTradingPaymentDetail, int nUserID);
    }
    #endregion
}
