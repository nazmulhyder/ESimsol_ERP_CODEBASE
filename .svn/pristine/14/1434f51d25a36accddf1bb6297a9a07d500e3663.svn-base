using System;
using System.IO;
using ESimSol.BusinessObjects;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Collections.Generic;
using System.Drawing;
using ICS.Core;
using ICS.Core.Framework;
using ICS.Core.Utility;

namespace ESimSol.BusinessObjects
{
    public class HolidayCalendarDetail : BusinessObject
    {
        public HolidayCalendarDetail()
        {
            HolidayCalendarDetailID = 0;
            HolidayCalendarID = 0;
            HolidayID = 0;
            HolidayName = "";
            CalendarApply = EnumCalendarApply.Full_Company;
            StartDate = DateTime.Now;
            EndDate = DateTime.Now;
            Remarks = "";
            ErrorMessage = "";
            HolidayCalendarDRPs = new List<HolidayCalendarDRP>();
        }
        #region Properties
        public int HolidayCalendarDetailID { get; set; }
        public int HolidayCalendarID { get; set; }
        public int HolidayID { get; set; }
        public string HolidayName { get; set; }
        public EnumCalendarApply CalendarApply { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string Remarks { get; set; }
        public string ErrorMessage { get; set; }
        public List<HolidayCalendarDRP> HolidayCalendarDRPs { get; set; }
        #endregion
        #region derived properties
        public string StartDateInString
        {
            get
            {
                return this.StartDate.ToString("dd MMM yyyy");
            }
        }
        public string EndDateInString
        {
            get
            {
                return this.EndDate.ToString("dd MMM yyyy");
            }
        }

        public string CalendarApplyInString
        {
            get
            {
                return this.CalendarApply.ToString();
            }
        }
        #endregion
        public HolidayCalendarDetail Save(long nUserID)
        {
            return HolidayCalendarDetail.Service.Save(this, nUserID);
        }
        public static List<HolidayCalendarDetail> Gets(string sSQL, long nUserID)
        {
            return HolidayCalendarDetail.Service.Gets(sSQL, nUserID);
        }
        public HolidayCalendarDetail Get(int id, long nUserID)
        {
            return HolidayCalendarDetail.Service.Get(id, nUserID);
        }

        public string Delete(int id, long nUserID)
        {
            return HolidayCalendarDetail.Service.Delete(id, nUserID);
        }


        #region ServiceFactory
        internal static IHolidayCalendarDetailService Service
        {
            get { return (IHolidayCalendarDetailService)Services.Factory.CreateService(typeof(IHolidayCalendarDetailService)); }
        }
        #endregion
    }
    #region IHolidayCalendarDetailService interface

    public interface IHolidayCalendarDetailService
    {
        HolidayCalendarDetail Save(HolidayCalendarDetail oHolidayCalendarDetail, Int64 nUserID);
        List<HolidayCalendarDetail> Gets(string sSQL, long nUserID);
        HolidayCalendarDetail Get(int id, long nUserID);
        string Delete(int id, long nUserID);

    }
    #endregion
}
