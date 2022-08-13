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
    #region ConsumptionForecast
    public class ConsumptionForecast : BusinessObject
    {
        public ConsumptionForecast()
        {
            BUID = 0;
            ProductID = 0;
		    ProductCode ="";
		    ProductName ="";
		    IsBooking =false;									
		    BookingMUnitID =0;
		    BookingMUSambol ="";
		    BookingQty =0;
            RRMUnitID =0;
		    RRMUSambol ="";
		    RRMUnitQty =0;
		    StockUnitID =0;									
		    StockMUSambol ="";
		    StockQty =0;
            ConsumtionQty = 0;
            YetToConsumtionQty = 0;
            ProductNature = (int)EnumProductNature.Hanger;
            StartDate = DateTime.Today;
            EndDate = DateTime.Today;
            bIsWithPI = false;
            ErrorMessage = "";
        }

        #region Properties
        public int BUID { get; set; }
        public int ProductID { get; set; }
		public string ProductCode {get; set;}
		public string ProductName {get; set;}
		public bool	IsBooking {get; set;}		
        public int BookingMUnitID {get; set;}
		public string BookingMUSambol {get; set;}
		public double BookingQty {get; set;}							
		public int RRMUnitID {get; set;}
		public string RRMUSambol {get; set;}
		public double RRMUnitQty {get; set;}
		public int StockUnitID {get; set;}							
		public string StockMUSambol{get; set;}
		public double StockQty {get; set;}
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public bool bIsWithPI { get; set; }
        public int ProductNature { get; set; }
        public double ConsumtionQty { get; set; }
        public double YetToConsumtionQty { get; set; }
        public string ErrorMessage { get; set; }
        #endregion

        #region Derived Property
        public double RequiredProductQty 
        {
            get 
            {
                if (RRMUnitQty > StockQty)
                    return (this.RRMUnitQty - this.StockQty);
                else
                    return 0;
            } 
        }
        public List<ConsumptionForecast> ConsumptionProduct { get; set; }
        public List<ConsumptionForecast> ConsumptionRM { get; set; }

        public string BookingQtySt
        {
            get{return Global.MillionFormat(this.BookingQty);}
        }
        public string StockQtySt
        {
            get{return Global.MillionFormat(this.StockQty);}
        }
        public string RRMUnitQtySt
        {
            get{return Global.MillionFormat(this.RRMUnitQty);}
        }
        public string RequiredProductQtySt
        {
            get { return Global.MillionFormat(this.RequiredProductQty); }
        }

        public string ConsumtionQtySt
        {
            get { return Global.MillionFormat(this.ConsumtionQty); }
        }
        public string YetToConsumtionQtySt
        {
            get { return Global.MillionFormat(this.YetToConsumtionQty); }
        }
        #endregion


        #region Functions
        public static List<ConsumptionForecast> PrepareConsumptionForecast(ConsumptionForecast oConsumptionForecast, Int64 nUserID)
        {
            return ConsumptionForecast.Service.PrepareConsumptionForecast(oConsumptionForecast, nUserID);
        }
      
        #endregion

        #region ServiceFactory
        internal static IConsumptionForecastService Service
        {
            get { return (IConsumptionForecastService)Services.Factory.CreateService(typeof(IConsumptionForecastService)); }
        }
        #endregion
    }
    #endregion

    #region IConsumptionForecast interface

    public interface IConsumptionForecastService
    {
        List<ConsumptionForecast> PrepareConsumptionForecast(ConsumptionForecast oConsumptionForecast, Int64 nUserID);
    }
    #endregion
}