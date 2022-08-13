using System;
using System.IO;
using ICS.Core;
using ICS.Core.Framework;
using ICS.Core.Utility;
using ESimSol.BusinessObjects;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Collections.Generic;
using System.Data;

namespace ESimSol.BusinessObjects
{
    #region AttendanceMonitoring

    public class AttendanceMonitoring : BusinessObject
    {
        public AttendanceMonitoring()
        {
            AMID = 0;
            DepartmentID = 0;
            DesignationID = 0;
            ShiftID = 0;
            Required = 0;
            Exists = 0;
            MaleExistPerson = 0;
            FemaleExistPerson = 0;
            Present = 0;
            MalePresent = 0;
            FemalePresent = 0;
            Absent = 0;
            MaleAbsent = 0;
            FemaleAbsent = 0;
            DayOff = 0;
            HoliDay = 0;
            MaleDayOff = 0;
            FemaleDayOff = 0;
            Leave = 0;
            MaleLeave = 0;
            FemaleLeave = 0;
            DepartmentName = "";
            DesignationName = "";
            ShiftName = "";
            Gender = "";
            ErrorMessage = "";

            //LINE or BLOCK
            BlockID = 0;
            BlockName = "";

            Count = 0;
            Status = "";

            BUID =0 ;
            BUName = "";

            LocationName = "";
            MaleLate = 0;
            FemaleLate = 0;
            MaleEarlyLeave = 0;
            FemaleEarlyLeave = 0;

            Percent = 0.0;
        }

        #region Properties
        public double Percent { get; set; }
        public int AMID { get; set; }
        public int MaleLate { get; set; }
        public int FemaleLate { get; set; }
        public int MaleEarlyLeave { get; set; }
        public int FemaleEarlyLeave { get; set; }
        public string LocationName { get; set; }
        public int DepartmentID { get; set; }
        public int DesignationID { get; set; }
        public int RosterPlanID { get; set; }
        public int ShiftID { get; set; }
        public int Required { get; set; }
        public int Exists { get; set; }
        public int MaleExistPerson { get; set; }
        public int FemaleExistPerson { get; set; }
        public int Present { get; set; }
        public int MalePresent { get; set; }
        public int FemalePresent { get; set; }
        public int Absent { get; set; }
        public int MaleAbsent { get; set; }
        public int FemaleAbsent { get; set; }
        public int DayOff { get; set; }
        public int HoliDay { get; set; }
        public int MaleDayOff { get; set; }
        public int FemaleDayOff { get; set; }
        public int Leave { get; set; }
        public int MaleLeave { get; set; }
        public int FemaleLeave { get; set; }
        public string ErrorMessage { get; set; }
        public string DepartmentName { get; set; }
        public string DesignationName { get; set; }
        public string ShiftName { get; set; }
        public string Gender { get; set; }

        // BLOCk or LINE
        public int BlockID { get; set; }
        public string BlockName { get; set; }
        public int Count { get; set; }
        public string Status { get; set; }

        // BU
        public int BUID { get; set; }
        public string BUName { get; set; }

        public int HRNeed
        {

            get { return this.Required - this.Present; }

        }

        public string RequiredSt
        {

            get { if ((this.Required) > 0)  return (this.Required).ToString(); else return "-"; }

        }
        public string ExistsSt
        {

            get { if ((this.Exists) > 0)  return (this.Exists).ToString(); else return "-"; }

        }
        public string MaleExistPersonSt
        {

            get { if ((this.MaleExistPerson) > 0)  return (this.MaleExistPerson).ToString(); else return "-"; }

        }
        public string FemaleExistPersonSt
        {

            get { if ((this.FemaleExistPerson) > 0)  return (this.FemaleExistPerson).ToString(); else return "-"; }

        }
        public string PresentSt
        {

            get { if ((this.Present) > 0)  return (this.Present).ToString(); else return "-"; }

        }
        public string MalePresentSt
        {

            get { if ((this.MalePresent) > 0)  return (this.MalePresent).ToString(); else return "-"; }

        }
        public string FemalePresentSt
        {

            get { if ((this.FemalePresent) > 0)  return (this.FemalePresent).ToString(); else return "-"; }

        }
        public string AbsentSt
        {

            get { if ((this.Absent) > 0)  return (this.Absent).ToString(); else return "-"; }

        }
        public string MaleAbsentSt
        {

            get { if ((this.MaleAbsent) > 0)  return (this.MaleAbsent).ToString(); else return "-"; }

        }
        public string FemaleAbsentSt
        {

            get { if ((this.FemaleAbsent) > 0)  return (this.FemaleAbsent).ToString(); else return "-"; }

        }
        public string DayOffSt
        {

            get { if ((this.DayOff) > 0)  return (this.DayOff).ToString(); else return "-"; }

        }
        public string HoliDaySt
        {

            get { if ((this.HoliDay) > 0)  return (this.HoliDay).ToString(); else return "-"; }

        }
        public string MaleLeaveSt
        {

            get { if ((this.MaleLeave) > 0)  return (this.MaleLeave).ToString(); else return "-"; }

        }
        public string FemaleLeaveSt
        {

            get { if ((this.FemaleLeave) > 0)  return (this.FemaleLeave).ToString(); else return "-"; }

        }
        public string LeaveSt
        {

            get { if ((this.Leave) > 0)  return (this.Leave).ToString(); else return "-"; }

        }

        public Company Company { get; set; }

        public List<AttendanceMonitoring> AttendanceMonitorings { get; set; }
        public List<AttendanceMonitoring> AttendanceMonitorings_DepSec { get; set; }

        #endregion

        #region Functions
        public static List<AttendanceMonitoring> Gets(string sSQL, long nUserID)
        {
            return AttendanceMonitoring.Service.Gets(sSQL, nUserID);
        }
        public static AttendanceMonitoring Get(string sSQL, long nUserID)
        {
            return AttendanceMonitoring.Service.Get(sSQL, nUserID);
        }
        public static List<AttendanceMonitoring> Gets(string sBUnit, string sLocationID, string sDepartmentIDs, string sDesignationIDs, string sShiftIds, string sBlockIDs, DateTime dDate, string sGroupIDs, long nUserID)
        {
            return AttendanceMonitoring.Service.Gets(sBUnit, sLocationID, sDepartmentIDs, sDesignationIDs, sShiftIds, sBlockIDs, dDate, sGroupIDs, nUserID);
        }
        public static List<AttendanceMonitoring> GetsComp(string sBUnit, string sLocationID, string sDepartmentIDs, string sDesignationIDs, string sShiftIds, string sBMMIDs, DateTime dDate, string sGroupIDs, string sBlockIDs, long nUserID)
        {
            return AttendanceMonitoring.Service.GetsComp(sBUnit, sLocationID, sDepartmentIDs, sDesignationIDs, sShiftIds, sBMMIDs, dDate, sGroupIDs, sBlockIDs, nUserID);
        }
        public static List<AttendanceMonitoring> Gets_LINE(string sBUnit, string sLocationID, string sDepartmentIDs, string sDesignationIDs, string sShiftIds, string sBMMIDs, DateTime dDate, long nUserID)
        {
            return AttendanceMonitoring.Service.Gets_LINE(sBUnit, sLocationID,sDepartmentIDs, sDesignationIDs, sShiftIds, sBMMIDs, dDate, nUserID);
        }
        public static List<AttendanceMonitoring> Gets_DeptSecWise(string sBUnit, string sLocationID,string sDepartmentIDs, string sDesignationIDs, string sShiftIds, string sBMMIDs, DateTime dDate, long nUserID)
        {
            return AttendanceMonitoring.Service.Gets_DeptSecWise(sBUnit, sLocationID,sDepartmentIDs, sDesignationIDs, sShiftIds, sBMMIDs, dDate, nUserID);
        }

        public static DataSet GetsManPower(string SqlQuery, DateTime AttendanceDate, int ReportLayout, Int64 nUserID)
        {
            return AttendanceMonitoring.Service.GetsManPower(SqlQuery, AttendanceDate, ReportLayout, nUserID);
        }       
        #endregion

        #region ServiceFactory
        internal static IAttendanceMonitoringService Service
        {
            get { return (IAttendanceMonitoringService)Services.Factory.CreateService(typeof(IAttendanceMonitoringService)); }
        }
        #endregion
    }
    #endregion

    #region IAttendanceMonitoring interface

    public interface IAttendanceMonitoringService
    {
        List<AttendanceMonitoring> Gets(string sSQL, Int64 nUserID);
        AttendanceMonitoring Get(string sSQL, Int64 nUserID);
        List<AttendanceMonitoring> Gets(string sBUnit, string sLocationID, string sDepartmentIDs, string sDesignationIDs, string sShiftIds, string sBlockIDs, DateTime dDate, string sGroupIDs, Int64 nUserID);
        List<AttendanceMonitoring> GetsComp(string sBUnit, string sLocationID, string sDepartmentIDs, string sDesignationIDs, string sShiftIds, string sBMMIDs, DateTime dDate, string sGroupIDs, string sBlockIDs, Int64 nUserID);
        List<AttendanceMonitoring> Gets_LINE(string sBUnit, string sLocationID, string sDepartmentIDs, string sDesignationIDs, string sShiftIds, string sBMMIDs, DateTime dDate, Int64 nUserID);
        List<AttendanceMonitoring> Gets_DeptSecWise(string sBUnit, string sLocationID, string sDepartmentIDs, string sDesignationIDs, string sShiftIds, string sBMMIDs, DateTime dDate, Int64 nUserID);
        DataSet GetsManPower(string SqlQuery, DateTime AttendanceDate, int ReportLayout, Int64 nUserID);
    
    }
    #endregion
}
