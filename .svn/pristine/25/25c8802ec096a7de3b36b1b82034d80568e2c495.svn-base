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
    #region WorkingHour

    public class WorkingHour : BusinessObject
    {
        public WorkingHour()
        {
            ShiftID = 0;
            ShiftName = "";
            TotalEmployee=0;
            TotalPresent=0;
            TotalAbsent = 0;
            TotalLeave=0;
            NormalPresent=0;
            OTPresent=0;
            NormalWorkingHourInMinute=0;
            OTWorkingHourInMinute = 0;
            TotalWorkingHourInMinute = 0;
            ErrorMessage = "";

        }

        #region Properties
        public int ShiftID { get; set; }
        public string ShiftName { get; set; }
        public int  TotalEmployee{ get; set; }
        public double TotalPresent{ get; set; }
        public double TotalAbsent { get; set; }
        public double TotalLeave{ get; set; }
        public double NormalPresent { get; set; }
        public double OTPresent { get; set; }
        public int NormalWorkingHourInMinute{ get; set; }
        public int OTWorkingHourInMinute { get; set; }
        public int TotalWorkingHourInMinute { get; set; }
        public string ErrorMessage { get; set; }

        #endregion

        #region Derived Property

        public string NormalWorkingHourST
        {
            get
            {
                return Global.MinInHourMin(this.NormalWorkingHourInMinute);
            }
        }
        public string OTWorkingHourST
        {
            get
            {
                return Global.MinInHourMin(this.OTWorkingHourInMinute);
            }
        }
        public string TotalWorkingHourST
        {
            get
            {
                return Global.MinInHourMin(this.TotalWorkingHourInMinute);
            }
        }
        #endregion

        #region Functions
        public static List<WorkingHour> GetsWorkingHour(string sParam, long nUserID)
        {
            return WorkingHour.Service.GetsWorkingHour(sParam, nUserID);
        }
        #endregion

        #region ServiceFactory
        internal static IWorkingHourService Service
        {
            get { return (IWorkingHourService)Services.Factory.CreateService(typeof(IWorkingHourService)); }
        }

        #endregion
    }
    #endregion

    #region IWorkingHour interface

    public interface IWorkingHourService
    {
        List<WorkingHour> GetsWorkingHour(string sParam, Int64 nUserID);
    }
    #endregion
}
