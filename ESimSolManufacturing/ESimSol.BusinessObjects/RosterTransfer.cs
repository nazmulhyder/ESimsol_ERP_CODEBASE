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
    #region RosterTransfer

    public class RosterTransfer : BusinessObject
    {
        public RosterTransfer()
        {
            RosterTransferID = 0;
            EmployeeID = 0;
            ShiftID = 0;
            Date = DateTime.Now;
            IsDayOff = false;
            ErrorMessage = "";
            ShiftStartTime = DateTime.Now;
            ShiftEndTime = DateTime.Now;
            EmployeeCode = "";
            EmployeeName = "";
            ShiftName = "";
        }

        #region Properties
        public int RosterTransferID { get; set; }
        public int EmployeeID { get; set; }
        public int ShiftID { get; set; }
        public DateTime Date { get; set; }
        public bool IsDayOff { get; set; }
        public string ErrorMessage { get; set; }
        #endregion

        #region Derived Property
        public string EmployeeCode { get; set; }
        public string EmployeeName { get; set; }
        public string ShiftName { get; set; }
        public string DateInString
        {
            get
            {
                return Date.ToString("dd MMM yyyy");
            }
        }
        public DateTime ShiftStartTime { get; set; }
        public DateTime ShiftEndTime { get; set; }
        public string ShiftStartTimeInString
        {
            get
            {
                return ShiftStartTime.ToString("H:mm");
            }
        }
        public string ShiftEndTimeInString
        {
            get
            {
                return ShiftEndTime.ToString("H:mm");
            }
        }
        public string ShiftWithDuration
        {
            get
            {
                if (this.ShiftName != "")
                {
                    return this.ShiftName + "(" + this.ShiftStartTimeInString + "-" + this.ShiftEndTimeInString + ")";
                }
                else { return this.ShiftName; }
            }
        }
        #endregion

        #region Functions
        public static RosterTransfer Get(int Id, long nUserID)
        {
            return RosterTransfer.Service.Get(Id, nUserID);
        }
        public static RosterTransfer Get(string sSQL, long nUserID)
        {
            return RosterTransfer.Service.Get(sSQL, nUserID);
        }
        public static List<RosterTransfer> Gets(long nUserID)
        {
            return RosterTransfer.Service.Gets(nUserID);
        }
        public static List<RosterTransfer> Gets(string sSQL, long nUserID)
        {
            return RosterTransfer.Service.Gets(sSQL, nUserID);
        }
        public static List<RosterTransfer> Gets(string EmployeeIDs, int ShiftID, DateTime StartDate, DateTime EndDate, long nUserID)
        {
            return RosterTransfer.Service.Gets(EmployeeIDs, ShiftID, StartDate, EndDate, nUserID);
        }

        public RosterTransfer IUD(int nDBOperation, long nUserID)
        {
            return RosterTransfer.Service.IUD(this, nDBOperation, nUserID);
        }
        public List<RosterTransfer> Transfer(string EmployeeIDs, int ShiftID, DateTime StartDate, DateTime EndDate, int nDBOperation, long nUserID)
        {
            return RosterTransfer.Service.Transfer(EmployeeIDs, ShiftID, StartDate, EndDate, nDBOperation, nUserID);
        }
        public List<RosterTransfer> Swap(int RosterPlanID, DateTime StartDate, DateTime EndDate, int nDBOperation, long nUserID)
        {
            return RosterTransfer.Service.Swap(RosterPlanID, StartDate, EndDate, nDBOperation, nUserID);
        }
        #endregion

        #region ServiceFactory
        internal static IRosterTransferService Service
        {
            get { return (IRosterTransferService)Services.Factory.CreateService(typeof(IRosterTransferService)); }
        }

        #endregion
    }
    #endregion

    #region IRosterTransfer interface

    public interface IRosterTransferService
    {
        RosterTransfer Get(int id, Int64 nUserID);
        RosterTransfer Get(string sSQL, Int64 nUserID);
        List<RosterTransfer> Gets(Int64 nUserID);
        List<RosterTransfer> Gets(string sSQL, Int64 nUserID);
        List<RosterTransfer> Gets(string EmployeeIDs, int ShiftID, DateTime StartDate, DateTime EndDate, Int64 nUserID);
        RosterTransfer IUD(RosterTransfer oRosterTransfer, int nDBOperation, Int64 nUserID);
        List<RosterTransfer> Transfer(string EmployeeIDs, int ShiftID, DateTime StartDate, DateTime EndDate, int nDBOperation, Int64 nUserID);
        List<RosterTransfer> Swap(int RosterPlanID, DateTime StartDate, DateTime EndDate, int nDBOperation, Int64 nUserID);

    }
    #endregion
}
