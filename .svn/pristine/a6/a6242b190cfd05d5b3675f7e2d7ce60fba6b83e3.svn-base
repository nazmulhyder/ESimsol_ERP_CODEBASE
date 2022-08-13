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
    public class HolidayCalendarDRP : BusinessObject
    {
        public HolidayCalendarDRP()
        {
            HolidayCalendarDRPID = 0;
            HolidayCalendarDetailID = 0;
            DRPID = 0;
            BusinessUnitID = 0;
            LocationID = 0;
            DepartmentID = 0;
            BUName = "";
            Location = "";
            Department = "";
            ErrorMessage = "";
            //EmployeeBatchDetails = new List<EmployeeBatchDetail>();
        }
        #region Properties
        public int HolidayCalendarDRPID { get; set; }
        public int HolidayCalendarDetailID { get; set; }
        public int DRPID { get; set; }
        public int BusinessUnitID { get; set; }
        public int LocationID { get; set; }
        public int DepartmentID { get; set; }
        public string BUName { get; set; }
        public string Location { get; set; }
        public string Department { get; set; }
        public string ErrorMessage { get; set; }
        //public List<EmployeeBatchDetail> EmployeeBatchDetails { get; set; }
        #endregion

        public static List<HolidayCalendarDRP> Gets(int id, long nUserID)
        {
            return HolidayCalendarDRP.Service.Gets(id, nUserID);
        }
     
        #region ServiceFactory
        internal static IHolidayCalendarDRPService Service
        {
            get { return (IHolidayCalendarDRPService)Services.Factory.CreateService(typeof(IHolidayCalendarDRPService)); }
        }
        #endregion
    }
    #region IHolidayCalendarDRPService interface

    public interface IHolidayCalendarDRPService
    {
        List<HolidayCalendarDRP> Gets(int id, long nUserID);

    }
    #endregion
}
