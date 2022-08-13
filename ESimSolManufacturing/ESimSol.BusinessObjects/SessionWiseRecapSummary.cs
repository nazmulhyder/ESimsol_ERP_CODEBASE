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

    #region SessionWiseRecapSummary
    
    public class SessionWiseRecapSummary : BusinessObject
    {
        public SessionWiseRecapSummary()
        {
            TempID = 0;
            BuyerID = 0;
            BusinessSessionID = 0;
            BuyerName = "";
            StyleCount = 0;
            OrderQty = 0;
            OrderValue = 0;
            ShipmentQty = 0;
            ShipmentValue = 0;
            EndosmentCommission = 0;
            B2BCommisssion = 0;
            TotalCommission = 0;
            ErrorMessage = "";

        }

        #region Properties
         
        public int TempID { get; set; }
         
        public int BuyerID { get; set; }
         
        public int BusinessSessionID { get; set; }
         
        public int StyleCount { get; set; }
         
        public double OrderQty { get; set; }
         
        public string BuyerName { get; set; }
         
        public double OrderValue { get; set; }
         
        public double ShipmentQty { get; set; }
         
        public double ShipmentValue { get; set; }
         
        public double EndosmentCommission { get; set; }
         
        public double B2BCommisssion { get; set; }
         
        public double TotalCommission { get; set; }
        

         
        public string ErrorMessage { get; set; }
        #endregion

        #region Derived Property

        public List<SessionWiseRecapSummary> SessionWiseRecapSummarys { get; set; }
        public List<BusinessSession> BusinessSessions { get; set; }


        public string OrderQtyInString
        {
            get
            {
                return Global.MillionFormat(this.OrderQty) + " Pcs";
            }

        }

        public Company Company { get; set; }

   

        public string StyleCountInString
        {
            get
            {
                return this.BuyerID + "~" + this.BusinessSessionID+"~" + this.StyleCount;
            }
        }
        #endregion

        #region Functions


        public static List<SessionWiseRecapSummary> Gets(int nSessionID, string sBuyerIDs, long nUserID)
        {
            return SessionWiseRecapSummary.Service.Gets(nSessionID,sBuyerIDs, nUserID);
        }

        #endregion

        #region ServiceFactory
 
        internal static ISessionWiseRecapSummaryService Service
        {
            get { return (ISessionWiseRecapSummaryService)Services.Factory.CreateService(typeof(ISessionWiseRecapSummaryService)); }
        }
        #endregion
    }
    #endregion

    #region Report Study
    public class SessionWiseRecapSummaryList : List<SessionWiseRecapSummary>
    {
        public string ImageUrl { get; set; }
    }
    #endregion

    #region ISessionWiseRecapSummary interface
     
    public interface ISessionWiseRecapSummaryService
    {
         
        List<SessionWiseRecapSummary> Gets(int nSessionID, string sBuyerIDs, Int64 nUserID);

    }
    #endregion
    
    
    
  
}
