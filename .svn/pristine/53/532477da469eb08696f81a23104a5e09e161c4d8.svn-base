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
    #region MarketingSchedule
    
    public class MarketingSchedule : BusinessObject
    {
        public MarketingSchedule()
        {
            MarketingScheduleID = 0;

            ScheduleNo = "";
            MKTPersonID = 0;
            BuyerID = 0;
            ScheduleDateTime = DateTime.Now;
            MeetingLocation = "";
            MeetingDuration = "";
            Remarks = "";
            ScheduleAssignBy = 0;
            MKTPersonName = "";
            BuyerName = "";
            BuyerShortName = "";
            MeetingSummaryText = "";
            ScheduleAssignByName = "";
            IsForBaseCollection = true;
            IsForBaseCalendar = false;
            IsForMKTPerson = false;
            
            ErrorMessage = "";

        }
        #region Properties
        
        public int MarketingScheduleID { get; set; }

        public string ScheduleNo { get; set; }

        public int MKTPersonID { get; set; }

        public int BuyerID { get; set; }

        public DateTime ScheduleDateTime{ get; set; }

        public string MeetingLocation { get; set; }
        public string MeetingDuration { get; set; }
        public string Remarks { get; set; }
        public int ScheduleAssignBy { get; set; }
        public string MKTPersonName { get; set; }
        public string BuyerName { get; set; }
        public string BuyerShortName { get; set; }
        public string MeetingSummaryText { get; set; }
        public string ScheduleAssignByName { get; set; }

        public bool IsForBaseCollection { get; set; }
        public bool IsForBaseCalendar { get; set; }
        public bool IsForMKTPerson { get; set; }
        public string ErrorMessage { get; set; }
        #endregion

        #region Derived Property
        public List<MeetingSummary> MeetingSummarys { get; set; }
        public List<MarketingSchedule> MarketingSchedules { get; set; }
        public string ScheduleDateInString { get { return this.ScheduleDateTime.ToString("dd MMM yyyy"); } }
        public string ScheduleDateTimeInString { get { return this.ScheduleDateTime.ToString("dd MMM yyyy hh:mm tt"); } }

        public string MeetingDurationHour { get { return this.MeetingDuration + " Hours"; } }
        #endregion

        #region Functions
        public static List<MarketingSchedule> GetsByCurrentMonth(DateTime dMonth, int nMKTPersonID, long nUserID)
        {
            return MarketingSchedule.Service.GetsByCurrentMonth(dMonth, nMKTPersonID, nUserID);
        }
        public static List<MarketingSchedule> GetsByCurrentMonth(DateTime dMonth, long nUserID)
        {
            return MarketingSchedule.Service.GetsByCurrentMonth(dMonth,nUserID);
        }
        //public MarketingSchedule GetByFabricID(int nBuyerID, int nFabricID, long nUserID)
        //{
        //    return MarketingSchedule.Service.GetByFabricID(nBuyerID, nFabricID, nUserID);
        //}
        public static List<MarketingSchedule> Gets(DateTime dScheduleDateTime, long nUserID)
        {
            return MarketingSchedule.Service.Gets(dScheduleDateTime, nUserID);
        }
        public static List<MarketingSchedule> Gets(int nMKTPersonID,DateTime dScheduleDateTime, long nUserID)
        {
            return MarketingSchedule.Service.Gets(nMKTPersonID, dScheduleDateTime, nUserID);
        }
        public static List<MarketingSchedule> Gets(long nUserID)
        {
            return MarketingSchedule.Service.Gets(nUserID);
        }
        public static List<MarketingSchedule> Gets(string sSQL, long nUserID)
        {
            return MarketingSchedule.Service.Gets(sSQL, nUserID);
        }
     
        public MarketingSchedule Get(int nId, long nUserID)
        {
            return MarketingSchedule.Service.Get(nId,nUserID);
        }
               
        public MarketingSchedule Save(long nUserID)
        {
            return MarketingSchedule.Service.Save(this, nUserID);
        }

        public string Delete(int nId, long nUserID)
        {
            return MarketingSchedule.Service.Delete(nId, nUserID);
        }
       

        #endregion

        #region ServiceFactory
        internal static IMarketingScheduleService Service
        {
            get { return (IMarketingScheduleService)Services.Factory.CreateService(typeof(IMarketingScheduleService)); }
        }
        #endregion
    }
    #endregion

    #region Report Study
    public class MarketingScheduleList : List<MarketingSchedule>
    {
        public string ImageUrl { get; set; }
    }
    #endregion

    #region IMarketingSchedule interface
    
    public interface IMarketingScheduleService
    {
        MarketingSchedule Get(int id, long nUserID);
        List<MarketingSchedule> GetsByCurrentMonth(DateTime dMonth, int nMKTPersonID, long nUserID);
        List<MarketingSchedule> GetsByCurrentMonth(DateTime dMonth, long nUserID);
        List<MarketingSchedule> Gets(DateTime dScheduleDateTime, long nUserID);
        List<MarketingSchedule> Gets(int nMKTPersonID, DateTime dScheduleDateTime, long nUserID);
        List<MarketingSchedule> Gets(long nUserID);
        List<MarketingSchedule> Gets(string sSQL, long nUserID);
        //MarketingSchedule GetByFabricID(int nBuyerID, int nFabricID, long nUserID);
        string Delete(int id, long nUserID);
        MarketingSchedule Save(MarketingSchedule oMarketingSchedule, long nUserID);
        
        
    }
    #endregion
}