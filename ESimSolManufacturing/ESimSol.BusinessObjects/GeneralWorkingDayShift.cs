using System;
using System.IO;
using ICS.Core;
using ICS.Core.Framework;
using ICS.Core.Utility;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Collections.Generic;


namespace ESimSol.BusinessObjects
{
    #region GeneralWorkingDayShifts

    public class GeneralWorkingDayShift : BusinessObject
    {
        public GeneralWorkingDayShift()
        {
            GWDSID = 0;
            GWDID = 0;
            ShiftID = 0;
            Name = "";
            StartTime = DateTime.Now;
            EndTime = DateTime.Now;
            DayStartTime = DateTime.Now;
            DayEndTime = DateTime.Now;
            DBUserID = 0;
            DBServerDateTime = DateTime.Now;
            ErrorMessage = "";
            ShiftIDs = "";
        }

        #region Properties
        public int GWDSID { get; set; }
        public int GWDID { get; set; }
        public int ShiftID { get; set; }
        public string Name { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public DateTime DayStartTime { get; set; }
        public DateTime DayEndTime { get; set; }
        public int DBUserID { get; set; }
        public DateTime DBServerDateTime { get; set; }
        public string ErrorMessage { get; set; }

        #endregion

        #region Derived Property
        public string ShiftIDs { get; set; }

        public string StartTimeInString
        {
            get
            {
                return StartTime.ToString("H:mm");
            }
        }
        public string EndTimeInString
        {
            get
            {
                return EndTime.ToString("H:mm");
            }
        }
        public string DayStartTimeInString
        {
            get
            {
                return DayStartTime.ToString("H:mm");
            }
        }
        public string DayEndTimeInString
        {
            get
            {
                return DayEndTime.ToString("H:mm");
            }
        }
        #endregion

        #region Functions
        public static List<GeneralWorkingDayShift> Gets(int id, long nUserID)
        {
            return GeneralWorkingDayShift.Service.Gets(id, nUserID);
        }
        //public List<GeneralWorkingDayShift> Save(int nDBOperation, long nUserID)
        //{
        //    return GeneralWorkingDayShift.Service.Save(this, nDBOperation, nUserID);
        //}

        #endregion

        #region ServiceFactory
        internal static IGeneralWorkingDayShiftService Service
        {
            get { return (IGeneralWorkingDayShiftService)Services.Factory.CreateService(typeof(IGeneralWorkingDayShiftService)); }
        }

        #endregion
    }
    #endregion

    #region IGeneralWorkingDayShift interface
    public interface IGeneralWorkingDayShiftService
    {
        List<GeneralWorkingDayShift> Gets(int id, Int64 nUserID);
        //List<GeneralWorkingDayShift> Save(GeneralWorkingDayShift oGeneralWorkingDayShift, int nDBOperation, Int64 nUserID);
    }
    #endregion
}
