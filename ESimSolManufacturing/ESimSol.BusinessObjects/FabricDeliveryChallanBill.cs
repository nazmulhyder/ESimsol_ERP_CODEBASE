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
    #region FabricDeliveryChallanBill
    
    public class FabricDeliveryChallanBill : BusinessObject
    {
        public FabricDeliveryChallanBill()
        {
            SCNo = "";
            SCDate = DateTime.Now;
            OrderType = 0;
            BuyerID = 0;
            BuyerName = "";
            Construction = "";
            ProductID = 0;
            ProductName = "";
            ContractorID = 0;
            ContractorName = "";
            MKTPersonID = 0;
            MKTPersonName = "";
            MUnitID = 0;
            MUnitName = "";
            ExeNo = "";
            CurrencyID = 0;
            CurrencyName = "";
            QTY = 0;
            UnitPrice = 0;
            IsDeduct = false;
            Qty_DC = 0;
            MUnit_DC = 0;
            MUnitName_DC = "";
            OrderName = "";
            UnitPrice_DC = 0;
            Currency_DC = 0;
            CurrencyName_DC = "";
            DiscountAmount = 0;
            AdditionalAmount = 0;
            PaymentAmount = 0;
            Params = "";
            ErrorMessage = "";
        }

        #region Properties

        public string SCNo { get; set; }
        public DateTime SCDate { get; set; }
        public int OrderType { get; set; }
        public int BuyerID { get; set; }
        public string BuyerName { get; set; }
        public string Construction { get; set; }
        public int ProductID { get; set; }
        public int FSCID { get; set; }
        public int FSCDID { get; set; }
        public string ProductName { get; set; }
        public int ContractorID { get; set; }
        public string ContractorName { get; set; }
        public int MKTPersonID { get; set; }
        public string MKTPersonName { get; set; }
        public int MUnitID { get; set; }
        public string MUnitName { get; set; }
        public int CurrencyID { get; set; }
        public string CurrencyName { get; set; }
        public double QTY { get; set; }
        public double UnitPrice { get; set; }
        public string ExeNo { get; set; }
        public bool IsDeduct { get; set; }
        public double Qty_DC { get; set; }
        public int MUnit_DC { get; set; }
        public string MUnitName_DC { get; set; }
        public double UnitPrice_DC { get; set; }
        public int Currency_DC { get; set; }
        public string CurrencyName_DC { get; set; }
        public double DiscountAmount { get; set; }
        public double AdditionalAmount { get; set; }
        public double PaymentAmount { get; set; }
        public string OrderTypeSt { get; set; }
        public string OrderName { get; set; }
        public int FabricOrderType { get; set; }
        public string ErrorMessage { get; set; }
        public string Params { get; set; }
        #endregion

        #region Derived Property
        public string SCDateSt
        {
            get
            {
                return this.SCDate.ToString("dd MMM yyyy");
            }
        }
        public double Amount
        {
            get
            {
                return Math.Round(this.QTY * this.UnitPrice);
            }
        }
        public double Total_DC
        {
            get
            {
                return Math.Round(this.Qty_DC * this.UnitPrice);
            }
        }
        public double YetToDelivery
        {
            get
            {
                return this.QTY - this.Qty_DC;
            }
        }
        public double YetToReceived
        {
            get
            {
                return this.Amount - this.Total_DC;
            }
        }
        #endregion

        #region Functions
        public static List<FabricDeliveryChallanBill> Gets(string sSQL, Int64 nUserID)
        {
            return FabricDeliveryChallanBill.Service.Gets(sSQL, nUserID);
        }

        #endregion

        #region ServiceFactory
        internal static IFabricDeliveryChallanBillService Service
        {
            get { return (IFabricDeliveryChallanBillService)Services.Factory.CreateService(typeof(IFabricDeliveryChallanBillService)); }
        }
        #endregion
    }
    #endregion

    #region IFabricDeliveryChallanBill interface
    
    public interface IFabricDeliveryChallanBillService
    {
        List<FabricDeliveryChallanBill> Gets(string sSQL, Int64 nUserID);
    }
    #endregion
}

