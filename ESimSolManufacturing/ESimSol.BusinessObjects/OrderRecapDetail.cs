using System;
using System.IO;
using System.ComponentModel.DataAnnotations;
using ICS.Core;
using ESimSol.BusinessObjects;
using ESimSol.BusinessObjects.ReportingObject;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Collections.Generic;
using ICS.Core.Framework;
using ICS.Core.Utility;



namespace ESimSol.BusinessObjects
{
    #region OrderRecapDetail
    
    public class OrderRecapDetail : BusinessObject
    {
        public OrderRecapDetail()
        {
            OrderRecapDetailID = 0;
            OrderRecapID = 0;
            OrderRecapDetailLogID = 0;
			OrderRecapLogID = 0;
            ColorID = 0;
            SizeID = 0;
            MeasurementUnitID = 0;
            UnitPrice = 0;
            Quantity = 0;
            Amount = 0;
            ColorSequence = 0;
            SizeSequence = 0;
            PoductionQty = 0;
            CurrencySymbol = "";
            UnitSymbol = "";
            YetToPoductionQty = 0;
            YetToScheduleQty = 0;
            OrderRecapNo = "";
            ShipmentDate = DateTime.Now;
        }

        #region Properties
         
        public int OrderRecapDetailID { get; set; }
        public string UnitSymbol { get; set; }
        public int OrderRecapID { get; set; }
         
        public int OrderRecapDetailLogID { get; set; }
         
        public int OrderRecapLogID { get; set; }
         
        public int ColorID { get; set; }
         
        public int SizeID { get; set; }
         
        public string ErrorMessage { get; set; }
         
        public int MeasurementUnitID { get; set; }
         
        public double UnitPrice { get; set; }
         
        public double Quantity { get; set; }
         
        public double Amount { get; set; }
         
        public double PoductionQty { get; set; }
         
        public double YetToPoductionQty { get; set; }
        public double YetToScheduleQty { get; set; }
         
        public string CurrencySymbol { get; set; }
         
        public int ColorSequence { get; set; }
         
        public int SizeSequence { get; set; }
        #endregion

        #region derived property
        public int TechnicalSheetID { get; set; }
        public List<TechnicalSheetColor> Colors { get; set; }
        public List<TechnicalSheetSize> Sizes { get; set; }
        public List<MeasurementUnit> Units { get; set; }
        public List<ColorSizeRatio> ColorSizeRatios { get; set; }
        public string OrderRecapNo { get; set; }
        public DateTime ShipmentDate { get; set; } 
         
        public string ColorName { get; set; }
         
        public string SizeName { get; set; }
         
        public string UnitName { get; set; }
        public string ColorNameInString
        {
            get
            {
                return ColorID.ToString();
            }
        }

        public string SizeNameInString
        {
            get
            {
                return SizeID.ToString();
            }
        }

        public string UnitNameInString
        {
            get
            {
                return MeasurementUnitID.ToString();
            }
        }

        public string ShipmentDateInString
        {
            get
            {
                return ShipmentDate.ToString("dd MMM yyyy");
            }
        }
        public string UnitPriceInString
        {
            get
            {
                return Global.MillionFormat(UnitPrice);
            }
        }
        public string QuantityInString
        {
            get
            {
                return Global.MillionFormat(Quantity);
            }
        }
        public string AmountInString
        {
            get
            {
                return Global.MillionFormat(Amount);
            }
        }

        public string AmountWithCurrency
        {
            get
            {
                return this.CurrencySymbol + " " + Global.MillionFormat(Amount);
            }
        }


        #endregion

        #region Functions
        public OrderRecapDetail Save(long nUserID)
        {
            return OrderRecapDetail.Service.Save(this, nUserID);
        }

        public string Delete(int id, long nUserID,string sOrderRecapDetailIDs)
        {
            return OrderRecapDetail.Service.Delete(id, nUserID,sOrderRecapDetailIDs);
        }

        public static List<OrderRecapDetail> Gets(int id, long nUserID) //OrderRecapId
        {
            return OrderRecapDetail.Service.Gets(id, nUserID);
        }
        public static List<OrderRecapDetail> GetsByLog(int id, long nUserID) //OrderRecapLogId
        {
            return OrderRecapDetail.Service.GetsByLog(id, nUserID);
        }
        public static List<OrderRecapDetail> Gets(string sSql, long nUserID) //OrderRecapId
        {
            return OrderRecapDetail.Service.Gets(sSql, nUserID);
        }
        #endregion

        #region ServiceFactory
        internal static IOrderRecapDetailService Service
        {
            get { return (IOrderRecapDetailService)Services.Factory.CreateService(typeof(IOrderRecapDetailService)); }
        }

        #endregion
    }
    #endregion


    #region IOrderRecapDetailService interface
     
    public interface IOrderRecapDetailService
    {        
         
        List<OrderRecapDetail> Gets(int id, Int64 nUserID);   //OrderRecapID
        
         
        List<OrderRecapDetail> GetsByLog(int id, Int64 nUserID);   //OrderRecapLogID
         
        string Delete(int id, Int64 nUserID, string sOrderRecapDetailID);
         
        OrderRecapDetail Save(OrderRecapDetail oOrderRecapDetail, Int64 nUserID);
         
        List<OrderRecapDetail> Gets(string sSql, Int64 nUserID);         
    }
    #endregion

    
}
