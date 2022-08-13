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
    #region MeetingSummary
    
    public class MeetingSummary : BusinessObject
    {
        public MeetingSummary()
        {
            MeetingSummaryID = 0;

            MarketingScheduleID = 0;
            MeetingSummarizeBy = 0;
            MeetingSummaryText = "";
            MarketingScheduleNo = "";
            MeetingSummarizeByName = "";
            DBServerDateTime = DateTime.Now;
            RefType = EnumMKTRef.None;
            RefID = 0;
            MeetingLocationAndTime = "";
            ErrorMessage = "";
        }

        #region Properties
        
        public int MeetingSummaryID { get; set; }

        public int MarketingScheduleID { get; set; }
        public int MeetingSummarizeBy { get; set; }
        public string MeetingSummaryText { get; set; }
        public string MarketingScheduleNo { get; set; }
        public string MeetingSummarizeByName { get; set; }
        public int BuyerID { get; set; }
        public string BuyerName { get; set; }
        public string BuyerShortName { get; set; }
        public DateTime DBServerDateTime { get; set; }
        public EnumMKTRef RefType { get; set; }
        public int RefID { get; set; }
        public int CurrencyID { get; set; }
        public double Price { get; set; }
        public string MeetingLocationAndTime { get; set; }        
        public string ErrorMessage { get; set; }
        #endregion

        #region Derived Property
        public string DBServerDateTimeInString { get { return this.DBServerDateTime.ToString("dd MMM yyyy hh:mm tt"); } }
        public string DBServerDateInString { get { return this.DBServerDateTime.ToString("dd MMM yyyy"); } }
        public string RefTypeSt { get { return this.RefType.ToString(); } }
        
        #endregion

        #region Functions
        public static List<MeetingSummary> Gets(int nMarketingScheduleID, long nUserID)
        {
            return MeetingSummary.Service.Gets(nMarketingScheduleID,nUserID);
        }
        public static List<MeetingSummary> GetsByRef(int nFabricID, int nRefType, long nUserID)
        {
            return MeetingSummary.Service.GetsByRef(nFabricID, nRefType, nUserID);
        }
        public static List<MeetingSummary> Gets(long nUserID)
        {
            return MeetingSummary.Service.Gets(nUserID);
        }
        public static List<MeetingSummary> Gets(string sSQL, long nUserID)
        {
            return MeetingSummary.Service.Gets(sSQL, nUserID);
        }
     
        public MeetingSummary Get(int nId, long nUserID)
        {
            return MeetingSummary.Service.Get(nId,nUserID);
        }
        public MeetingSummary Save(long nUserID)
        {
            return MeetingSummary.Service.Save(this, nUserID);
        }

        public string Delete(int nId, long nUserID)
        {
            return MeetingSummary.Service.Delete(nId, nUserID);
        }
        public MeetingSummary SaveFromFabric(MeetingSummary oMeetingSummary, int nMKTPID, int nBuyerID, long nUserID)
        {
            return MeetingSummary.Service.SaveFromFabric(oMeetingSummary, nMKTPID, nBuyerID, nUserID);
        }
         
       

        #endregion

        #region ServiceFactory
        internal static IMeetingSummaryService Service
        {
            get { return (IMeetingSummaryService)Services.Factory.CreateService(typeof(IMeetingSummaryService)); }
        }
        #endregion

    }
    #endregion

    #region Report Study
    public class MeetingSummaryList : List<MeetingSummary>
    {
        public string ImageUrl { get; set; }
    }
    #endregion

    #region IMeetingSummary interface
    
    public interface IMeetingSummaryService
    {
        
        MeetingSummary Get(int id, long nUserID);
        List<MeetingSummary> Gets(int nMarketingScheduleID, long nUserID);
        List<MeetingSummary> GetsByRef(int nFabricID, int nRefType, long nUserID);
        List<MeetingSummary> Gets(long nUserID);
        List<MeetingSummary> Gets(string sSQL, long nUserID);
        
        string Delete(int id, long nUserID);
        MeetingSummary SaveFromFabric(MeetingSummary oMeetingSUmmary, int nMKTPID, int nBuyerID, long nUserID);
        MeetingSummary Save(MeetingSummary oMeetingSummary, long nUserID);
        
        
    }
    #endregion
}