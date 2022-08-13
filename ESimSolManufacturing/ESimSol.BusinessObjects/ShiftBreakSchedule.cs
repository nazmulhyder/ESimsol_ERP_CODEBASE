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
    #region ShiftBreakSchedule

    public class ShiftBreakSchedule : BusinessObject
    {
        public ShiftBreakSchedule()
        {
            ShiftBScID = 0;
            ShiftID = 0;
            ShiftBNID = 0;
            StartTime = DateTime.Now;
            EndTime = DateTime.Now;
            IsActive = true;
            ErrorMessage = "";
            this.ShiftBreakName = "";

        }

        #region Properties
        public int ShiftBScID { get; set; }
        public int ShiftID { get; set; }
        public int ShiftBNID { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public bool IsActive { get; set; }
        public string ErrorMessage { get; set; }
        public string ShiftBreakName { get; set; }

        #endregion

        #region Derived Property
        public string Activity { get { if (IsActive)return "Active"; else return "Inactive"; } }

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

        public string SBSchedule
        {
            get
            {
                return StartTime.ToString("H:mm")+"-"+EndTime.ToString("H:mm");
            }
        }
        public string SBNameSchedule
        {
            get
            {
                return this.ShiftBreakName + "(" + this.StartTime.ToString("H:mm") + "-" + this.EndTime.ToString("H:mm") + ")";
            }
        }

        #endregion

        #region Functions
        public static ShiftBreakSchedule Get(int Id, long nUserID)
        {
            return ShiftBreakSchedule.Service.Get(Id, nUserID);
        }
        public static ShiftBreakSchedule Get(string sSQL, long nUserID)
        {
            return ShiftBreakSchedule.Service.Get(sSQL, nUserID);
        }
        public static List<ShiftBreakSchedule> Gets(long nUserID)
        {
            return ShiftBreakSchedule.Service.Gets(nUserID);
        }
        public static List<ShiftBreakSchedule> Gets(string sSQL, long nUserID)
        {
            return ShiftBreakSchedule.Service.Gets(sSQL, nUserID);
        }

        public ShiftBreakSchedule IUD(int nDBOperation, long nUserID)
        {
            return ShiftBreakSchedule.Service.IUD(this, nDBOperation, nUserID);
        }

        public static ShiftBreakSchedule Activite(int nId, long nUserID)
        {
            return ShiftBreakSchedule.Service.Activite(nId, nUserID);
        }
        #endregion

        #region ServiceFactory
        internal static IShiftBreakScheduleService Service
        {
            get { return (IShiftBreakScheduleService)Services.Factory.CreateService(typeof(IShiftBreakScheduleService)); }
        }

        #endregion
    }
    #endregion

    #region IShiftBreakSchedule interface

    public interface IShiftBreakScheduleService
    {
        ShiftBreakSchedule Get(int id, Int64 nUserID);
        ShiftBreakSchedule Get(string sSQL, Int64 nUserID);
        List<ShiftBreakSchedule> Gets(Int64 nUserID);
        List<ShiftBreakSchedule> Gets(string sSQL, Int64 nUserID);
        ShiftBreakSchedule IUD(ShiftBreakSchedule oShiftBreakSchedule, int nDBOperation, Int64 nUserID);
        ShiftBreakSchedule Activite(int nId, Int64 nUserID);
    }
    #endregion
}
