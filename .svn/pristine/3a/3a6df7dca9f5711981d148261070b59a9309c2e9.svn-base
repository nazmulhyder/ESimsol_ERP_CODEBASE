using System;
using System.IO;
using ICS.Core;
using ESimSol.BusinessObjects;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Collections.Generic;
using ICS.Core.Framework;
using ICS.Core.Utility;

namespace ESimSol.BusinessObjects
{

    #region RecapShipmentSummary
    
    public class RecapShipmentSummary : BusinessObject
    {
        public RecapShipmentSummary()
        {
            
            BuyerID =0;
		    BuyerName ="";
            StyleNo = "";
            OrderNo = "";
            CMValue = 0;
            FOBValue = 0;
            CurrencySymbol = "";
		    ShipmentDate  =DateTime.Now;
		    Qty =0;
		    DataViewType =0;// 1: Buyer & DAte;;  2: Buyer & Month;;  3 : Month;
		    ShipmentMonth  =0;
		    ShipmentMonthInString  ="";
            NumberOfShipment = 0;
            ErrorMessage = "";
            ShipmentCount = 0;
            TempID = 0;
            MonthWiseSummaries = new List<RecapShipmentSummary>();
            BuyerWithMonthWiseSummaries = new List<RecapShipmentSummary>();
        }

        #region Properties
         
        public int BuyerID { get; set; }
         
        public string BuyerName { get; set; }
         
        public string StyleNo { get; set; }
         
        public string OrderNo { get; set; }
         
        public double CMValue { get; set; }
         
        public double FOBValue { get; set; }
         
        public string CurrencySymbol { get; set; }
         
        public DateTime ShipmentDate { get; set; }
         
        public string ShipmentMonthInString { get; set; }        
         
        public double Qty { get; set; }
         
        public int DataViewType { get; set; }
         
        public int ShipmentMonth { get; set; }
         
        public int NumberOfShipment { get; set; }
       
         
        public string ErrorMessage { get; set; }
        #endregion

        #region Derived Property
        public int TempID { get; set; }
        public int ShipmentCount { get; set; }
        public List<RecapShipmentSummary> RecapShipmentSummarys { get; set; }
        public List<RecapShipmentSummary> MonthWiseSummaries { get; set; }
        public List<RecapShipmentSummary> BuyerWithMonthWiseSummaries { get; set; }
        public List<RecapShipmentSummary> BuyerWithDateWiseSummaries { get; set; }        
        public string QuantityInString
        {
            get
            {
                return Global.MillionFormat(this.Qty)+" Pcs";
            }
        }

        public string CMValueInString
        {
            get
            {
                return this.CurrencySymbol + " " + Global.MillionFormat(this.CMValue);
            }

        }

        public string FOBValueInString
        {
            get
            {
                return this.CurrencySymbol+" "+Global.MillionFormat(this.FOBValue);
            }

        }

        public Company Company { get; set; }

        public string ShipmentDateInString 
        {
            get
            {
                return this.ShipmentDate.ToString("dd MMM yyyy");
            }
        }
        public string ShipmentCountInString
        {
            get
            {
                return this.ShipmentCount.ToString()+"~"+this.TempID.ToString();
            }
        }   
        #endregion

        #region Functions


        public static List<RecapShipmentSummary> Gets(string sYear, int BUID, int nUserType,  long nUserID)
        {
            return RecapShipmentSummary.Service.Gets(sYear, BUID, nUserType, nUserID);
        }

        #endregion

        #region ServiceFactory

   
        internal static IRecapShipmentSummaryService Service
        {
            get { return (IRecapShipmentSummaryService)Services.Factory.CreateService(typeof(IRecapShipmentSummaryService)); }
        }

        #endregion
    }
    #endregion

    #region Report Study
    public class RecapShipmentSummaryList : List<RecapShipmentSummary>
    {
        public string ImageUrl { get; set; }
    }
    #endregion

    #region IRecapShipmentSummary interface
     
    public interface IRecapShipmentSummaryService
    {
         
        List<RecapShipmentSummary> Gets(string sYear,int BUID, int nUserType, Int64 nUserID);

    }
    #endregion
    
    
  
}
