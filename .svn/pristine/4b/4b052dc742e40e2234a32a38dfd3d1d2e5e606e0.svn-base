using System;
using System.IO;
using ICS.Core;
using ESimSol.BusinessObjects;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Collections.Generic;
using ICS.Core.Framework;
using ICS.Core.Utility;
using ESimSol.BusinessObjects.ReportingObject;

namespace ESimSol.BusinessObjects
{

    #region TradingSaleOrder

    public class TradingSaleOrder : BusinessObject
    {
        public TradingSaleOrder()
        {
            TradingSaleOrderID = 0;
            BUID = 0;
            DemandRequsitionID = 0;
            TradingSaleOrderNo = "";
            OrderType = EnumTradingSaleOrderType.None;
            ContractorID = 0;
            ContractorPersonalID = 0;
            OrderCreateDate = DateTime.Now;
            RequestedDeliveryDate = DateTime.Now;
            IsActive = false;
            Note = "";
            SaleValidatyDate = DateTime.Now;
            ContractorName = "";
            ContractPersonName = "";
            Amount = 0;
        }

        #region Properties

        public int TradingSaleOrderID { get; set; }

        public int BUID { get; set; }

        public int DemandRequsitionID { get; set; }

        public string TradingSaleOrderNo { get; set; }

        public string ContractorName { get; set; }

        public EnumTradingSaleOrderType OrderType { get; set; }

        public int ContractorID { get; set; }

        public double Amount { get; set; }


        public int ContractorPersonalID { get; set; }

        public string ContractPersonName { get; set; }

        public DateTime OrderCreateDate { get; set; }

        public DateTime RequestedDeliveryDate { get; set; }

        public bool IsActive { get; set; }

        public string Note { get; set; }

        public DateTime SaleValidatyDate { get; set; }

        public string TradingSaleOrderDetailInString { get; set; }

        public string ErrorMessage { get; set; }
        #endregion

        #region Derive Property
        public List<Location> Locations { get; set; }
        public List<TradingSaleOrder> TradingSaleOrders { get; set; }
        public List<TradingSaleOrderDetail> TradingSaleOrderDetails { get; set; }
        public Company Company { get; set; }
        public Contractor Contractor { get; set; }
        public WorkingUnit WorkingUnit { get; set; }
        public List<ContactPersonnel> ContactPersonnels { get; set; }
        public List<MeasurementUnit> MeasurementUnits { get; set; }
        //public DemandRequisition DemandRequisition { get; set; }
        //public CommissionSetup CommissionSetup { get; set; }
        public string OrderTypeInString
        {
            get
            {
                return OrderType.ToString();
            }
        }
        public string OrderCreateDateInString
        {
            get
            {
                return OrderCreateDate.ToString("dd MMM yyyy");
            }
        }

        public string RequestedDeliveryDateInString
        {
            get
            {
                return RequestedDeliveryDate.ToString("dd MMM yyyy");
            }
        }
        public string SaleValidatyDateInString
        {
            get
            {
                return SaleValidatyDate.ToString("dd MMM yyyy");
            }
        }
        public int OrderTypeInt { get; set; }

        #endregion

        #region Functions

        public static List<TradingSaleOrder> Gets(long nUserID)
        {
            return TradingSaleOrder.Service.Gets(nUserID);
        }

        public static List<TradingSaleOrder> Gets(string sSQL, long nUserID)
        {
            return TradingSaleOrder.Service.Gets(sSQL, nUserID);
        }

        public static string GetTradingSaleOrderNo(long nUserID)
        {
            return TradingSaleOrder.Service.GetTradingSaleOrderNo(nUserID);
        }
        public TradingSaleOrder Get(int id, long nUserID)
        {
            return TradingSaleOrder.Service.Get(id, nUserID);
        }
        public TradingSaleOrder GetByTradingSaleOrderNo(string sTradingSaleOrderNo, long nUserID)
        {
            return TradingSaleOrder.Service.GetByTradingSaleOrderNo(sTradingSaleOrderNo, nUserID);
        }

        public TradingSaleOrder Save(long nUserID)
        {
            return TradingSaleOrder.Service.Save(this, nUserID);
        }

        public TradingSaleOrder SaveInvoice(SaleInvoice oSaleInvoice, long nUserID)
        {
            return TradingSaleOrder.Service.SaveInvoice(oSaleInvoice, nUserID);
        }

        public string Delete(int id, long nUserID)
        {
            return TradingSaleOrder.Service.Delete(id, nUserID);
        }

        #endregion

        #region ServiceFactory
        internal static ITradingSaleOrderService Service
        {
            get { return (ITradingSaleOrderService)Services.Factory.CreateService(typeof(ITradingSaleOrderService)); }
        }
        #endregion
    }
    #endregion

    #region ITradingSaleOrder interface

    public interface ITradingSaleOrderService
    {

        TradingSaleOrder Get(int id, Int64 nUserID);

        TradingSaleOrder GetByTradingSaleOrderNo(string sTradingSaleOrderNo, Int64 nUserID);


        List<TradingSaleOrder> Gets(Int64 nUserID);

        List<TradingSaleOrder> Gets(string sSQL, Int64 nUserID);

        string Delete(int id, Int64 nUserID);

        TradingSaleOrder Save(TradingSaleOrder oTradingSaleOrder, Int64 nUserID);

        TradingSaleOrder SaveInvoice(SaleInvoice oSaleInvoice, Int64 nUserID);

        string GetTradingSaleOrderNo(Int64 nUserID);
    }
    #endregion
   
}

