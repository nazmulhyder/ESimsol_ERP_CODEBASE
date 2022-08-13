using System;
using System.IO;
using ESimSol.BusinessObjects;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Collections.Generic;
using System.Data;
using ICS.Core;
using ICS.Core.Framework;
using ICS.Core.Utility;


namespace ESimSol.BusinessObjects
{
    #region RouteSheetHistory

    public class RouteSheetHistory : BusinessObject
    {
        public RouteSheetHistory()
        {
            RouteSheetHistoryID = 0;
            RouteSheetID = 0;
            EventTime = DateTime.Now;
            EventEmpID = 0;
            CurrentStatus = EnumRSState.None;
            PreviousState = EnumRSState.None;
            Note = string.Empty;
            ShadePercentage = 0;
            MachineID_Hydro = 0;
            MachineID_Dryer = 0;
            Value_Dyes = 0;
            Value_Chemcial = 0;
            Value_Yarn = 0;
            MachineSpeed = 0;
            RBSpeed = 0;
            Value_Yarn = 0; 
            ErrorMessage = string.Empty;
            Params = string.Empty;
            UserName = "";
        }

        #region Properties
        public int RouteSheetHistoryID { get; set; }
        public int RouteSheetID { get; set; }
        public DateTime EventTime { get; set; }
        public int EventEmpID { get; set; }
        public EnumRSState CurrentStatus { get; set; }
        public EnumRSState PreviousState { get; set; }
        public string Note { get; set; }

        public double ShadePercentage { get; set; }
        public int MachineID_Hydro { get; set; }
        public int MachineID_Dryer { get; set; }
        public double Value_Dyes { get; set; }
        public double Value_Chemcial { get; set; }
        public double Value_Yarn { get; set; }

        public string MachineName_Hydro { get; set; }
        public string MachineName_Dryer { get; set; }
        public double MachineSpeed { get; set; }
        public double RBSpeed { get; set; }
        #endregion

        #region Derive
        public string ErrorMessage { get; set; }
        public string Params { get; set; }

        public string RouteSheetNo { get; set; }
        public string EventEmpName { get; set; }
        public string UserName { get; set; }
        public string CurrentStatusStr
        {
            get
            {
                return this.CurrentStatus.ToString();
            }
        }
        public string PreviousStateStr
        {
            get
            {
                return this.PreviousState.ToString();
            }
        }

        public string EventTimeStr
        {
            get
            {
                return this.EventTime.ToString("dd MMM yyyy hh:mm tt");
            }
        }
        public string EventDT
        {
            get
            {
                return this.EventTime.ToString("dd MMM yyyy");
            }
        }
        public string TimeSt
        {
            get
            {
                return this.EventTime.ToString("HH:mm");

            }
        }
        #endregion


        #region Functions

        public static RouteSheetHistory Get(int nRouteSheetHistoryID, long nUserID)
        {
            return RouteSheetHistory.Service.Get(nRouteSheetHistoryID, nUserID);
        }
        public  RouteSheetHistory GetBy(int nRSID,int nRSStatus, long nUserID)
        {
            return RouteSheetHistory.Service.GetBy(nRSID, nRSStatus, nUserID);
        }
        public static List<RouteSheetHistory> Gets(string sSQL, long nUserID)
        {
            return RouteSheetHistory.Service.Gets(sSQL, nUserID);
        }
        public RouteSheetHistory ChangeRSStatus(long nUserID)
        {
            return RouteSheetHistory.Service.ChangeRSStatus(this, nUserID);
        }
        public static List<RouteSheetHistory> Gets(int nRSID, long nUserID)
        {
            return RouteSheetHistory.Service.Gets(nRSID, nUserID);
        }
        public RouteSheetHistory ChangeRSStatus_Process(long nUserID)
        {
            return RouteSheetHistory.Service.ChangeRSStatus_Process(this, nUserID);
        }

        public RouteSheetHistory GetRSDyeingProgress(long nUserID)
        {
            return RouteSheetHistory.Service.GetRSDyeingProgress(this, nUserID);
        }
        public RouteSheetHistory UpdateEventTime(long nUserID)
        {
            return RouteSheetHistory.Service.UpdateEventTime(this, nUserID);
        }
        #endregion

        #region ServiceFactory
        internal static IRouteSheetHistoryService Service
        {
            get { return (IRouteSheetHistoryService)Services.Factory.CreateService(typeof(IRouteSheetHistoryService)); }
        }
        #endregion


    }

    #endregion

    #region IRouteSheetHistory interface
    public interface IRouteSheetHistoryService
    {
        RouteSheetHistory GetBy(int nRSID, int nRSStatus, long nUserID);
        RouteSheetHistory Get(int nRouteSheetHistoryID, long nUserID);
        RouteSheetHistory GetRSDyeingProgress(RouteSheetHistory oRouteSheetHistory, long nUserID);
        List<RouteSheetHistory> Gets(string sSQL, long nUserID);
        List<RouteSheetHistory> Gets(int nRSID, long nUserID);
        RouteSheetHistory ChangeRSStatus(RouteSheetHistory oRSH, long nUserID);
        RouteSheetHistory UpdateEventTime(RouteSheetHistory oRSH, long nUserID);
        RouteSheetHistory ChangeRSStatus_Process(RouteSheetHistory oRSH, long nUserID);
    }
    #endregion

}