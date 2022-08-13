using System;
using System.IO;
using ICS.Core;
using ICS.Core.Framework;
using ICS.Core.Utility;
using ESimSol.BusinessObjects;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Collections.Generic;

namespace ESimSol.BusinessObjects
{
    #region WorkingHourReport
    public class WorkingHourReport
    {
        public WorkingHourReport()
        {

            LocationName = "";
            BlockName ="";
            Hour8 =0;
            Hour9 =0;
            Hour10 =0;
            Hour11 =0;
            Hour12 =0;
            Hour12P5 =0;
            Hour13 =0;
            BonusWorkHour = 0;
        }

        #region Properties
        public int Hour8 { get; set; }
        public int Hour9 { get; set; }
        public int Hour10 { get; set; }
        public int Hour11 { get; set; }
        public int Hour12 { get; set; }
        public int Hour12P5 { get; set; }
        public int Hour13 { get; set; }
        public int BonusWorkHour { get; set; }
        public string BlockName { get; set; }
        public string LocationName { get; set; }
        #endregion

        #region Functions


        public static List<WorkingHourReport> Gets(string sSQL, long nUserID)
        {
            return WorkingHourReport.Service.Gets(sSQL, nUserID);
        }
        public static WorkingHourReport Get(string sSQL, long nUserID)
        {
            return WorkingHourReport.Service.Get(sSQL, nUserID);
        }
        public static List<WorkingHourReport> Gets(DateTime dDate, string sBusinessUnitIds, string sLocationID, string sDepartmentIds, string sDesignationIds, string sEmployeeIDs, string sShiftIDs, long nUserID)
        {
            return WorkingHourReport.Service.Gets(dDate, sBusinessUnitIds, sLocationID, sDepartmentIds, sDesignationIds, sEmployeeIDs, sShiftIDs, nUserID);
        }
        #endregion

        #region ServiceFactory

        internal static IWorkingHourReportService Service
        {
            get { return (IWorkingHourReportService)Services.Factory.CreateService(typeof(IWorkingHourReportService)); }
        }
        #endregion
    }
    #endregion

    #region IAttendanceDaily interface

    public interface IWorkingHourReportService
    {
        List<WorkingHourReport> Gets(string sSQL, Int64 nUserID);
        WorkingHourReport Get(string sSQL, Int64 nUserID);
        List<WorkingHourReport> Gets(DateTime dDate, string sBusinessUnitIds, string sLocationID, string sDepartmentIds, string sDesignationIds, string sEmployeeIDs, string sShiftIDs, Int64 nUserID);
       
      
    }
    #endregion
}

