using System;
using System.IO;
using ICS.Core;
using ICS.Core.Framework;
using ICS.Core.Utility;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Collections.Generic;
using System.Data;


namespace ESimSol.BusinessObjects
{
    #region BenefitOnAttendanceEmployeeLedger

    public class BenefitOnAttendanceEmployeeLedger : BusinessObject
    {
        public BenefitOnAttendanceEmployeeLedger()
        {
            BOAELID = 0;
            BOAEmployeeID = 0;
            AttendanceDate = DateTime.Now;
            BOAName = "";
            BenefitOn = EnumBenefitOnAttendance.None;
            StartTime = DateTime.Now;
            EndTime = DateTime.Now;
            EmployeeID = 0;
            EmployeeCode = "";
            EmployeeName = "";
            DepartmentName = "";
            DesignationName = "";
            ErrorMessage = "";
            TotalDay = 0;
            Value = 0;
            JoiningDate = DateTime.Now;
         
        }

        #region Properties
        public int BOAELID { get; set; }
        public double Value { get; set; }
        public int BOAEmployeeID { get; set; }
        public DateTime AttendanceDate { get; set; }
        public DateTime JoiningDate { get; set; }
        public string BOAName { get; set; }
        public EnumBenefitOnAttendance BenefitOn { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public int EmployeeID { get; set; }
        public string EmployeeCode { get; set; }
        public string EmployeeName { get; set; }
        public string DepartmentName { get; set; }
        public string DesignationName { get; set; }
        public string ErrorMessage { get; set; }
        public int TotalDay { get; set; }
        #endregion

        #region Derived Property
        public string TimeSlotInString
        {
            get
            {
                if (this.BenefitOn == EnumBenefitOnAttendance.Time_Slot)
                {
                    if (StartTime.ToString("HH:mm") != "00:00" && EndTime.ToString("HH:mm") != "00:00")
                        return StartTime.ToString("HH:mm") + "-" + EndTime.ToString("HH:mm");
                    else
                        return "-";
                }
                else
                    return "-";
            }
        }

        public int BenefitOnInt { get; set; }
        public string BenefitOnInString
        {
            get
            {
                return this.BenefitOn.ToString();
            }
        }
        public string AttendanceDateInString
        {
            get
            {
                return this.AttendanceDate.ToString("dd MMM yyyy");
            }
        }
        public List<BenefitOnAttendanceEmployeeLedger> BOAELs { get; set; }
        public Company Company { get; set; }

        #endregion

        #region User Define Functions
        #region Object Mapping
        private static BenefitOnAttendanceEmployeeLedger MappingObject(DataRow oDataRow)
        {
            BenefitOnAttendanceEmployeeLedger oBenefitOnAttendanceEmployeeLedger = new BenefitOnAttendanceEmployeeLedger();
            oBenefitOnAttendanceEmployeeLedger.BOAEmployeeID = (oDataRow["BOAEmployeeID"] == DBNull.Value) ? 0 : Convert.ToInt32(oDataRow["BOAEmployeeID"]);
            oBenefitOnAttendanceEmployeeLedger.TotalDay = (oDataRow["TotalDay"] == DBNull.Value) ? 0 : Convert.ToInt32(oDataRow["TotalDay"]);
            oBenefitOnAttendanceEmployeeLedger.Value = (oDataRow["Value"] == DBNull.Value) ? 0 : Convert.ToDouble(oDataRow["Value"]);
            oBenefitOnAttendanceEmployeeLedger.BOAName = (oDataRow["BOAName"] == DBNull.Value) ? "" : Convert.ToString(oDataRow["BOAName"]);
            return oBenefitOnAttendanceEmployeeLedger;
        }
        #endregion

        #region CreateObject
        public static BenefitOnAttendanceEmployeeLedger CreateObject(DataRow oDataRow)
        {
            BenefitOnAttendanceEmployeeLedger oBenefitOnAttendanceEmployeeLedger = new BenefitOnAttendanceEmployeeLedger();
            oBenefitOnAttendanceEmployeeLedger = MappingObject(oDataRow);
            return oBenefitOnAttendanceEmployeeLedger;
        }
        #endregion

        #region CreateObjects
        public static List<BenefitOnAttendanceEmployeeLedger> CreateObjects(DataTable oDataTable)
        {
            List<BenefitOnAttendanceEmployeeLedger> oBenefitOnAttendanceEmployeeLedgers = new List<BenefitOnAttendanceEmployeeLedger>();
            foreach (DataRow oDataRow in oDataTable.Rows)
            {
                BenefitOnAttendanceEmployeeLedger oItem = CreateObject(oDataRow);
                oBenefitOnAttendanceEmployeeLedgers.Add(oItem);
            }
            return oBenefitOnAttendanceEmployeeLedgers;
        }
        public static List<BenefitOnAttendanceEmployeeLedger> CreateObjects(DataRow[] oDataRows)
        {
            List<BenefitOnAttendanceEmployeeLedger> oBenefitOnAttendanceEmployeeLedgers = new List<BenefitOnAttendanceEmployeeLedger>();
            foreach (DataRow oDataRow in oDataRows)
            {
                BenefitOnAttendanceEmployeeLedger oItem = CreateObject(oDataRow);
                oBenefitOnAttendanceEmployeeLedgers.Add(oItem);
            }
            return oBenefitOnAttendanceEmployeeLedgers;
        }
        #endregion
        #endregion
        #region Functions
        public static List<BenefitOnAttendanceEmployeeLedger> Gets(string sSQL,long nUserID)
        {
            return BenefitOnAttendanceEmployeeLedger.Service.Gets(sSQL,nUserID);
        }

        #endregion

        #region ServiceFactory
        internal static IBenefitOnAttendanceEmployeeLedgerService Service
        {
            get { return (IBenefitOnAttendanceEmployeeLedgerService)Services.Factory.CreateService(typeof(IBenefitOnAttendanceEmployeeLedgerService)); }
        }

        #endregion
    }
    #endregion

    #region IBenefitOnAttendanceEmployeeLedger interface

    public interface IBenefitOnAttendanceEmployeeLedgerService
    {
        List<BenefitOnAttendanceEmployeeLedger> Gets(string sSQL, Int64 nUserID);
        List<BenefitOnAttendanceEmployeeLedger> BOA_ReportGets(string sSQL, Int64 nUserID);
    }

    #endregion
}
