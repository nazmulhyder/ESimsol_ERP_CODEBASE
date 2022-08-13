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

    #region TradingSaleOrderDetail

    public class TradingSaleOrderDetail : BusinessObject
    {
        public TradingSaleOrderDetail()
        {
            TradingSaleOrderDetailID = 0;
            TradingSaleOrderID = 0;
            ProductID = 0;
            OrderQty = 0;
            UnitPrice = 0;
            MeasurementUnitID = 0;
            VatInPercent = 0;
            InvoiceQty = 0;
            ContractorName = "";
        }

        #region Properties

        public int TradingSaleOrderDetailID { get; set; }

        public int TradingSaleOrderID { get; set; }

        public int ProductID { get; set; }

        public EnumProductGrade ProductGrade { get; set; }

        public double OrderQty { get; set; }

        public double UnitPrice { get; set; }

        public int MeasurementUnitID { get; set; }

        public double VatInPercent { get; set; }

        public double InvoiceQty { get; set; }

        public string SaleOrderNo { get; set; }

        public string ContractorName { get; set; }

        public DateTime OrderCreateDate { get; set; }

        public EnumTradingSaleOrderType OrderType { get; set; }

        public string ProductName { get; set; }

        public string ProductCode { get; set; }

        public double YetToInvoice { get; set; }

        public string MeasurementUnitName { get; set; }

        public string ErrorMessage { get; set; }

        public int ContractorID { get; set; }

        public double GradeAPrice { get; set; }

        public double GradeBPrice { get; set; }

        public double GradeCPrice { get; set; }
        #endregion

        #region Derive Property

        public List<Location> Locations { get; set; }
        public double TotalValue
        {
            get
            {
                return (UnitPrice * OrderQty);
            }
        }
        public double NetVal
        {
            get
            {
                return ((TotalValue * VatInPercent / 100) + TotalValue);
            }
        }
        public string ProductNameCode
        {
            get
            {
                return ProductName + " [" + ProductCode + "]";
            }
        }


        public string ProductGradeInString
        {
            get
            {
                return ProductGrade.ToString();
            }
        }
        #endregion

        #region Functions

        public static List<TradingSaleOrderDetail> Gets(long nUserID)
        {
            return TradingSaleOrderDetail.Service.Gets(nUserID);
        }
        public static List<TradingSaleOrderDetail> Gets(int id, long nUserID)
        {
            return TradingSaleOrderDetail.Service.Gets(id, nUserID);
        }
        public static List<TradingSaleOrderDetail> GetsForInvoice(int id, long nUserID)
        {
            return TradingSaleOrderDetail.Service.GetsForInvoice(id, nUserID);
        }

        public TradingSaleOrderDetail Get(int id, long nUserID)
        {
            return TradingSaleOrderDetail.Service.Get(id, nUserID);
        }


        public TradingSaleOrderDetail Save(long nUserID)
        {
            return TradingSaleOrderDetail.Service.Save(this, nUserID);
        }

        public string Delete(int id, long nUserID)
        {
            return TradingSaleOrderDetail.Service.Delete(id, nUserID);
        }

        #endregion

        #region ServiceFactory
        internal static ITradingSaleOrderDetailService Service
        {
            get { return (ITradingSaleOrderDetailService)Services.Factory.CreateService(typeof(ITradingSaleOrderDetailService)); }
        }
        #endregion
    }
    #endregion

    #region ITradingSaleOrderDetail interface

    public interface ITradingSaleOrderDetailService
    {

        TradingSaleOrderDetail Get(int id, Int64 nUserID);

        List<TradingSaleOrderDetail> Gets(Int64 nUserID);

        List<TradingSaleOrderDetail> Gets(int id, Int64 nUserID);

        List<TradingSaleOrderDetail> GetsForInvoice(int id, Int64 nUserID);

        string Delete(int id, Int64 nUserID);

        TradingSaleOrderDetail Save(TradingSaleOrderDetail oTradingSaleOrderDetail, Int64 nUserID);
    }
    #endregion
  
}

