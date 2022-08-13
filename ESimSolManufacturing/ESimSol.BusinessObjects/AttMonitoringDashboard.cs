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
    #region AttMonitoringDashboardSheet
    [DataContract]
    public class AttMonitoringDashboard : BusinessObject
    {
        public AttMonitoringDashboard()
        {

            TotalEmployee = 0;
            PresentPercent = 0;
            AbsentPercent = 0;
            LeavePercent = 0;
            LatePercent = 0;
            EarlyPercent = 0;
            ErrorMessage = "";

        }

        #region Properties
        public int TotalEmployee { get; set; }
        public double PresentPercent { get; set; }
        public double AbsentPercent { get; set; }
        public double LeavePercent { get; set; }
        public double LatePercent { get; set; }
        public double EarlyPercent { get; set; }
        public string ErrorMessage { get; set; }

        #endregion

        #region Derived Property
        public Company Company { get; set; }
        
        #endregion

        #region Functions
        public static AttMonitoringDashboard Get(DateTime StartDate, DateTime EndDate, long nUserID)
        {
            return AttMonitoringDashboard.Service.Get(StartDate, EndDate, nUserID);
        }

        #endregion

        #region ServiceFactory

        internal static IAttMonitoringDashboardService Service
        {
            get { return (IAttMonitoringDashboardService)Services.Factory.CreateService(typeof(IAttMonitoringDashboardService)); }
        }
        #endregion
    }
    #endregion

    #region IAttMonitoringDashboard interface
    
    public interface IAttMonitoringDashboardService
    {
        AttMonitoringDashboard Get(DateTime StartDate, DateTime EndDate, Int64 nUserID);
    }
    #endregion
}
