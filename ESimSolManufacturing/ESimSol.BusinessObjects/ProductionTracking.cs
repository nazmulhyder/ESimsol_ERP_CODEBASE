
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
    #region ProductionTracking
    
    public class ProductionTracking : BusinessObject
    {
        public ProductionTracking()
        {

            OrderRecapID = 0;
            TechnicalSheetID = 0;
            BuyerID = 0;
            StyleNo = "";
            OrderRecapNo = "";
            BuyerName = "";
            StartDate = DateTime.Now;
            OrderQty = 0;
            KnittingQty = 0;
            YetToKnitingQty = 0;
            LinkingQty = 0;
            YetToLinkingQty = 0;
            TrimmingQty = 0;
            YetToTrimmingQty = 0;
            EmbroideryQty = 0;
            YetToEmbroideryQty = 0;
            SewingQty = 0;
            YetToSewingQty = 0;
            IronQty = 0;
            YetToIronQty = 0;
            PolyQty = 0;
            YetToPolyQty = 0;
            CartonQty = 0;
            YetToCartonQty = 0;
            ErrorMessage = "";
            TodayCarton = 0;
            TodayEmbroidery = 0;
            TodayIron = 0;
            TodayKnitting = 0;
            TodayLinking = 0;
            TodayPoly = 0;
            TodaySewing = 0;
            TodayTrimming = 0;
        }

        #region Properties
         

        public int OrderRecapID { get; set; }
         
        public int TechnicalSheetID { get; set; }
         
        public int BuyerID { get; set; }
         
        public string StyleNo { get; set; }
         

        public string OrderRecapNo { get; set; }
         
        public string BuyerName { get; set; }
         
        public DateTime StartDate { get; set; }
         
        public double OrderQty { get; set; }
         
        public double KnittingQty { get; set; }
         
        public double YetToKnitingQty { get; set; }
         
        public double TodayKnitting { get; set; }
         
        public double LinkingQty { get; set; }
         
        public double YetToLinkingQty { get; set; }
         
        public double TodayLinking { get; set; }
         
        public double TrimmingQty { get; set; }
         
        public double YetToTrimmingQty { get; set; }
         
        public double TodayTrimming { get; set; }
         
        public double EmbroideryQty { get; set; }
         
        public double YetToEmbroideryQty { get; set; }
         
        public double TodayEmbroidery { get; set; }
         
        public double SewingQty { get; set; }
         
        public double YetToSewingQty { get; set; }
         
        public double TodaySewing { get; set; }
         
        public double IronQty { get; set; }
         
        public double YetToIronQty { get; set; }
         
        public double TodayIron { get; set; }
         
        public double PolyQty { get; set; }
         
        public double YetToPolyQty { get; set; }
         
        public double TodayPoly { get; set; }
         
        public double CartonQty { get; set; }
         
        public double YetToCartonQty { get; set; }
         
        public double TodayCarton { get; set; }
         
        public string ErrorMessage { get; set; }

        #endregion


        #region Derived Property
         
        public string OrderRecapIDs { get; set; }
         
        public List<ProductionTracking> ProductionTrackings { get; set; }
        #endregion

        #region Functions

        public static List<ProductionTracking> GetProductionTracking(string nOrderRecapIDs, long nUserID)
        {
             return ProductionTracking.Service.GetProductionTracking(nOrderRecapIDs, nUserID);
        }


        #endregion

        #region ServiceFactory
        internal static IProductionTrackingService Service
        {
            get { return (IProductionTrackingService)Services.Factory.CreateService(typeof(IProductionTrackingService)); }
        }

        #endregion
    }
    #endregion

    #region IProductionTracingUnit interface
     
    public interface IProductionTrackingService
    {
        //  
        //  ProductionTracingUnit Get(int id, Int64 nUserID);
         
        List<ProductionTracking> GetProductionTracking(string nOrderRecapIDs, Int64 nUserID);

    }
    #endregion

}

