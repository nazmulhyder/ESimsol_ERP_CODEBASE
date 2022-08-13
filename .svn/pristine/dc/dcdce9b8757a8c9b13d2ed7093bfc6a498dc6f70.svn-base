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
    #region AttendanceProcessManagement
    public class AttendanceProcessManagement : BusinessObject
    {
        public AttendanceProcessManagement()
        {
            APMID = 0;
            CompanyID = 1;
            LocationID = 0;
            DepartmentID = 0;
            ProcessType = EnumProcessType.None;
            ShiftID = 0;
            Status = EnumProcessStatus.Initialize;//Enum
            StatusInt = 0;
            AttendanceDate = DateTime.Now;
            ErrorMessage = "";
            ShiftName = "";
            BusinessUnitID = 0;
            BUName = "";
            BUShortName = "";
            Shifts = new List<HRMShift>();
            BusinessUnits = new List<BusinessUnit>();
            Locations = new List<Location>();
            Departments = new List<Department>();
            TimeCards = new List<MaxOTConfiguration>();
            StartDate = DateTime.Today;
            EndDate = DateTime.Today;
            EmpCount = 0;
            EmployeeID = 0;
        }

        #region Properties

        public int APMID { get; set; }
        public int CompanyID { get; set; }
        public int LocationID { get; set; }
        public int DepartmentID { get; set; }
        public EnumProcessType ProcessType { get; set; }
        public int ShiftID { get; set; }
        public EnumProcessStatus Status { get; set; }
        public int StatusInt { get; set; }
        public DateTime AttendanceDate { get; set; }
        public string ErrorMessage { get; set; }
        public int BusinessUnitID { get; set; }
        public string BUName { get; set; }
        public string BUShortName { get; set; }
        public int EmpCount { get; set; }
        #endregion

        #region Derived Property
        public string LocationName { get; set; }
        public string DepartmenName { get; set; }
        public List<HRMShift> Shifts { get; set; }
        public List<BusinessUnit> BusinessUnits { get; set; }
        public List<Location> Locations { get; set; }
        public List<Department> Departments { get; set; }
        public List<MaxOTConfiguration> TimeCards { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public int EmployeeID { get; set; }
        public string AttendanceDateInString
        {
            get
            {
                return AttendanceDate.ToString("dd MMM yyyy");
            }
        }

        public int ProcessTypeInt { get; set; }
        public string ProcessTypeInString
        {
            get
            {
                return ProcessType.ToString();
            }
        }
        public string StatusInString
        {
            get
            {
                return Status.ToString();
            }
        }

        public string ShiftName { get; set; }

        public List<AttendanceProcessManagement> AttendanceProcessManagements { get; set; }

        #endregion

        #region Functions
        public static List<AttendanceProcessManagement> Gets(long nUserID)
        {
            return AttendanceProcessManagement.Service.Gets(nUserID);
        }
        public static List<AttendanceProcessManagement> Gets(string sSQL, long nUserID)
        {
            return AttendanceProcessManagement.Service.Gets(sSQL, nUserID);
        }
        public static AttendanceProcessManagement Get(int id, long nUserID)
        {
            return AttendanceProcessManagement.Service.Get(id, nUserID);
        }

        public AttendanceProcessManagement IUD(int nDBOperation, long nUserID)
        {
            return AttendanceProcessManagement.Service.IUD(this, nDBOperation, nUserID);
        }
        public AttendanceProcessManagement IUD_V2(EnumDBOperation eDBOperation, long nUserID)
        {
            return AttendanceProcessManagement.Service.IUD_V2(this, eDBOperation, nUserID);
        }
        public RTPunchLog ProcessDataCollectionRT(List<RTPunchLog> oRTs, long nUserID)
        {
            return AttendanceProcessManagement.Service.ProcessDataCollectionRT(oRTs, nUserID);
        }

        public int ProcessAttendanceDaily_V1(int nIndex, long nUserID)
        {
            return AttendanceProcessManagement.Service.ProcessAttendanceDaily_V1(this, nIndex, nUserID);
        }

        public AttendanceProcessManagement ProcessBreezeAbsent(DateTime StartDate, DateTime EndDate, long nUserID)
        {
            return AttendanceProcessManagement.Service.ProcessBreezeAbsent(StartDate, EndDate, nUserID);
        }
        public AttendanceProcessManagement ManualAttendanceProcess(string sBUIDs, string sLocationIDs, string sDepartmentIDs, int nEmployeeID, DateTime dAttendanceDate, Int64 nUserID)
        {
            return AttendanceProcessManagement.Service.ManualAttendanceProcess(sBUIDs, sLocationIDs, sDepartmentIDs, nEmployeeID, dAttendanceDate, nUserID);
        }
        public AttendanceProcessManagement ManualCompAttendanceProcess(string sBUIDs, string sLocationIDs, string sDepartmentIDs, DateTime dAttendanceDate, int nMOCID, Int64 nUserID)
        {
            return AttendanceProcessManagement.Service.ManualCompAttendanceProcess(sBUIDs, sLocationIDs, sDepartmentIDs, dAttendanceDate, nMOCID, nUserID);
        }
        public static string APMProcess(string sBUIDs, string sLocationIDs, string sDepartmentIDs, DateTime dStartDate, DateTime dEndDate, Int64 nUserID)
        {
            return AttendanceProcessManagement.Service.APMProcess(sBUIDs, sLocationIDs, sDepartmentIDs, dStartDate, dEndDate, nUserID);
        }
        #endregion

        #region ServiceFactory
        internal static IAttendanceProcessManagementService Service
        {
            get { return (IAttendanceProcessManagementService)Services.Factory.CreateService(typeof(IAttendanceProcessManagementService)); }
        }
        #endregion
    }
    #endregion

    #region IAttendanceProcessManagement interface

    public interface IAttendanceProcessManagementService
    {
        List<AttendanceProcessManagement> Gets(Int64 nUserID);
        List<AttendanceProcessManagement> Gets(string sSQL, Int64 nUserID);
        AttendanceProcessManagement Get(int id, Int64 nUserID);
        AttendanceProcessManagement IUD(AttendanceProcessManagement oAttendanceProcessManagement, int nDBOperation, Int64 nUserID);
        AttendanceProcessManagement IUD_V2(AttendanceProcessManagement oAttendanceProcessManagement, EnumDBOperation eDBOperation, Int64 nUserID);
        int ProcessAttendanceDaily_V1(AttendanceProcessManagement oAttendanceProcessManagement, int nIndex, Int64 nUserID);
        RTPunchLog ProcessDataCollectionRT(List<RTPunchLog> oRTs, Int64 nUserID);
        AttendanceProcessManagement ProcessBreezeAbsent(DateTime StartDate, DateTime EndDate, Int64 nUserID);
        AttendanceProcessManagement ManualAttendanceProcess(string sBUIDs, string sLocationIDs, string sDepartmentIDs, int nEmployeeID, DateTime dAttendanceDate, Int64 nUserID);
        AttendanceProcessManagement ManualCompAttendanceProcess(string sBUIDs, string sLocationIDs, string sDepartmentIDs, DateTime dAttendanceDate, int nMOCID, Int64 nUserID);
        string APMProcess(string sBUIDs, string sLocationIDs, string sDepartmentIDs, DateTime dStartDate, DateTime dEndDate, Int64 nUserID);        
    }
    #endregion
}
