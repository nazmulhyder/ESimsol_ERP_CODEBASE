using System;
using System.IO;
using ESimSol.BusinessObjects;
using System.Drawing;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Collections.Generic;
using System.Data;
using ICS.Core;
using ICS.Core.Framework;
using ICS.Core.Utility;

namespace ESimSol.BusinessObjects
{
    #region DUPSchedule
    public class DUPSchedule : BusinessObject
    {
        #region  Constructor
        public DUPSchedule()
        {
            DUPScheduleID = 0;
            BatchGroup = 0;
            Status = EnumProductionScheduleStatus.Hold;
            ScheduleNo = "";
            MachineID = 0;
            OrderNo = "";
            ContractorName = "";
            DBUserID = 0;
            PSBatchNo = EnumNumericOrder.First;
            LocationID = 0;
            Qty = 0;
            ScheduleType = true;
            StartTime = DateTime.Now;
            EndTime = DateTime.Now;
            DBUserID = 0;
            ErrorMessage = "";
            MachineNo = "";
            MachineName = "";
            Capacity = 0;
            ColorName = "";
            LocationName = "";
            DeliveryDate = DateTime.Now;
            ProductName = "";
            IsIncreaseTime = false;
            DUPSchedules = new List<DUPSchedule>();
            DUPScheduleDetails = new List<DUPScheduleDetail>();
            OrderCount = 0;
            RouteSheetID = 0;
            OrderInfo = "";
            RouteSheetNo = "";
            BuyerRef = "";
            Note = "";
            RSShiftID = 0;
            Shift = "";
            RSStatus = 0;
            Capacity2 = "";
            DUPSLots = new List<DUPSLot>();
        }
        #endregion

        #region Properties
        public string OrderNo { get; set; }
        public int DUPScheduleID { get; set; }
        public int BatchGroup { get; set; }
        public string ProductName { get; set; }
        public string ColorName { get; set; }
        public string BuyerRef { get; set; }
        public string ContractorName { get; set; }
        public string ScheduleNo { get; set; }
        public int MachineID { get; set; }
        public EnumNumericOrder PSBatchNo { get; set; }
        public int LocationID { get; set; }
        public int BUID { get; set; }
        public double Qty { get; set; }
        public bool ScheduleType { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public DateTime DeliveryDate { get; set; }
        public int DBUserID { get; set; }
        public string ErrorMessage { get; set; }
        public EnumProductionScheduleStatus Status { get; set; }
        public int StatusInt { get; set; }
        public int RSStatus { get; set; }
        public string OrderInfo { get; set; }
        public int OrderCount { get; set; }
        public int RouteSheetID { get; set; }
        public string RouteSheetNo { get; set; }
        public int WorkingUnitID { get; set; }
        public string LotNo { get; set; }
        public string Note { get; set; }
        public string Params { get; set; }
        public int LotID { get; set; }
        public int RSShiftID { get; set; }
        public string Shift { get; set; }
        #region Production 'Schedule Status' and 'Batch No' In String

        public string StatusSt
        {
            get
            {
                return Status.ToString();
            }
        }
        #endregion

        public string StartTimeSt
        {
            get
            {
                return StartTime.ToString("dd MMM yyyy HH:mm");
            }
        }
        public string StartDateSt
        {
            get
            {
                return StartTime.ToString("dd MMM yyyy");
            }
        }
        public string DeliveryDateSt
        {
            get
            {
                return DeliveryDate.ToString("dd MMM yyyy");
            }
        }
        public string EndDateSt
        {
            get
            {
                return EndTime.ToString("dd MMM yyyy");
            }
        }
        public string EndTimeSt
        {
            get
            {
                return EndTime.ToString("dd MMM yyyy HH:mm");
            }
        }
        public string StartTimeTS
        {
            get
            {
                return this.StartTime.ToString("HH:mm");

            }
        }
        public string EndTimeTS
        {
            get
            {
                return this.EndTime.ToString("HH:mm");

            }
        }

        public string ScheduleTypeName
        {

            get
            {
                if (this.ScheduleType)
                {
                    return "Regular";
                }
                else
                {
                    return "Booking";
                }
            }


        }
        public int TimeDiffInHours
        {
            get {
                TimeSpan oTS = new TimeSpan();
                oTS = this.EndTime - this.StartTime;
                int nDay = oTS.Days;
                return oTS.Hours + (nDay > 0 ? (nDay*24) : 0);
            }
        }
     

        #endregion
        #region derived Properties
        public string MachineNo { get; set; }
        public string MachineName { get; set; }
        public double Capacity { get; set; }
        public string Capacity2 { get; set; }
        public string PSBatchNoSt
        {
            get
            {
                return PSBatchNo.ToString();
            }
        }
        public string LocationName { get; set; }
        public List<Location> Locations { get; set; }
        public List<Machine> DyeMachines { get; set; }
        public List<DUPScheduleDetail> DUPScheduleDetails { get; set; }
        public List<DUPSchedule> DUPSchedules { get; set; }
        public List<DUPSLot> DUPSLots { get; set; }

        public string MachineNoWithCapacity { get { return this.MachineName + "(" + ((this.Capacity <= 0) ? "" : this.Capacity.ToString("00"))+((string.IsNullOrEmpty(this.Capacity2) ? "" : this.Capacity2)) + ")"; } }

        public bool IsIncreaseTime { get; set; }

        #endregion

        #region Functions

        public DUPSchedule Get(int nId, long nUserID)
        {
            return DUPSchedule.Service.Get(nId,nUserID);
        }
        public DUPSchedule Save(long nUserID)
        {
            return DUPSchedule.Service.Save(this, nUserID);
        }
        public DUPSchedule SaveRS(long nUserID)
        {
            return DUPSchedule.Service.SaveRS(this, nUserID);
        }
        public DUPSchedule Update_Status(long nUserID)
        {
            return DUPSchedule.Service.Update_Status(this, nUserID);
        }
        public static int GetsMax(string sSql, long nUserID)
        {
            return DUPSchedule.Service.GetsMax(sSql, nUserID);
        }

        public string Delete(long nUserID)
        {
            return DUPSchedule.Service.Delete(this, nUserID);
        }
        public static List<DUPSchedule> Gets(string sSql, long nUserID)
        {
            return DUPSchedule.Service.Gets(sSql, nUserID);
        }

        public static List<DUPSchedule> Gets(long nUserID)
        {
            return DUPSchedule.Service.Gets(nUserID);
        }

        public static List<DUPSchedule> Gets(DateTime dStartDate, DateTime dEndDate, string sLocationIDs,long nUserID)
        {
            return DUPSchedule.Service.Gets(dStartDate, dEndDate, sLocationIDs, nUserID);
        }
        #endregion

        #region NonDB Functions

        #endregion

        #region ServiceFactory
        internal static IDUPScheduleService Service
        {
            get { return (IDUPScheduleService)Services.Factory.CreateService(typeof(IDUPScheduleService)); }
        }
        #endregion

    }


    #region IDUPSchedule interface
    public interface IDUPScheduleService
    {
        DUPSchedule Get(int nID, long nUserID);
        List<DUPSchedule> Gets(long nUserID);
        List<DUPSchedule> Gets(DateTime dStartDate, DateTime dEndDate, string sLocationIDs,long nUserID);
        int GetsMax(string sSql, long nUserID);
        List<DUPSchedule> Gets(string sSql, long nUserID);
        string Delete(DUPSchedule oDUPSchedule, long nUserID);
        DUPSchedule Save(DUPSchedule oDUPSchedule, long nUserID);
        DUPSchedule Update_Status(DUPSchedule oDUPSchedule, long nUserID);
        DUPSchedule SaveRS(DUPSchedule oDUPSchedule, long nUserID);
    }        
    #endregion

    #endregion
}
