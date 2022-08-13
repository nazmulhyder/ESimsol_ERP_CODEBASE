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
    #region EarnLeave

    public class EarnLeave : BusinessObject
    {
        public EarnLeave()
        {
            EmpLeaveLedgerID = 0;
            EmployeeID = 0;
            Code = "";
            Name = "";
            DPTName = "";
            DSGName = "";
            DateOfJoin = DateTime.Now;
            PresentDayBalance = 0;
            LastProcessDate = DateTime.Now;
            PreviousLastDate = DateTime.Now;
            TotalDays = 0;
            TotalLeave = 0;
            ClassifiedEL = 0;
            PresencePerLeave = 0;
            Enjoyed = 0;
            Present=0;
            RunningEL = 0;
            ErrorMessage = "";
            PresentONAtt = 0;
            AbsentOnAtt = 0;
            DayoffOnAtt = 0;
            HolidayOnAtt = 0;
            LeaveOnAtt = 0;


            TotalHoliday = 0;
            TotalDayOff = 0;
            TotalAbsent = 0;
            TotalPresent = 0;
            TotalLeaveOnAttendance = 0;

            LocationID = 0;
            LocationName = "";
            BusinessUnitID = 0;
            DesignationID = 0;
            DepartmentID = 0;
        }

        #region Properties
        public int LocationID { get; set; }
        public int DepartmentID { get; set; }
        public int BusinessUnitID { get; set; }
        public int DesignationID { get; set; }

        public int TotalHoliday { get; set; }
        public int TotalDayOff { get; set; }
        public int TotalAbsent { get; set; }
        public int TotalPresent { get; set; }
        public int TotalLeaveOnAttendance { get; set; }

        public int TotalDays { get; set; }
        public int TotalLeave { get; set; }
        public int PresentONAtt { get; set; }
        public int AbsentOnAtt { get; set; }
        public int DayoffOnAtt { get; set; }
        public int HolidayOnAtt { get; set; }
        public int LeaveOnAtt { get; set; }
        public int EmpLeaveLedgerID { get; set; }
        public int EmployeeID { get; set; }
        public string Code { get; set; }
        public string LocationName { get; set; }
        
        public string Name { get; set; }
        public string DPTName { get; set; }
        public string DSGName { get; set; }
        public DateTime PreviousLastDate { get; set; }
        public DateTime DateOfJoin { get; set; }
        public int PresentDayBalance { get; set; }
        public DateTime LastProcessDate { get; set; }
        public int ClassifiedEL { get; set; }
        public int PresencePerLeave { get; set; }
        public int Enjoyed { get; set; }
        public int Present { get; set; }
        public int RunningEL { get; set; }
        public string ErrorMessage { get; set; }
        #endregion

        #region Derived Property
        public string DateOfJoinInString
        {
            get
            {
                return DateOfJoin.ToString("dd MMM yyyy");
            }
        }
        public string PreviousLastDateInString
        {
            get
            {
                return (this.PreviousLastDate == DateTime.MinValue) ? "-" : PreviousLastDate.ToString("dd MMM yyyy");
            }
        }
        public string LastProcessDateInString
        {
            get
            {
                return LastProcessDate.ToString("dd MMM yyyy");
            }
        }
        public int Balance
        {
            get
            {
                return this.ClassifiedEL-this.Enjoyed+this.RunningEL;
            }
        }
        public int BalanceWithoutRunning
        {
            get
            {
                return this.ClassifiedEL - this.Enjoyed;
            }
        }
        public int TotalEL
        {
            get
            {
                return this.ClassifiedEL + this.RunningEL;
            }
        }
        #endregion

        #region Functions
        public static List<EarnLeave> ELGets(string sEmployeeIDs, string sLocationID, string DepartmentIDs, string DesignationIDs, DateTime Date, string sBUnit, int nLoadRecordsS, int nRowLengthS, int isAll, long nUserID)
        {
            return EarnLeave.Service.ELGets(sEmployeeIDs, sLocationID, DepartmentIDs, DesignationIDs, Date, sBUnit, nLoadRecordsS, nRowLengthS, isAll, nUserID);
        }
        public static List<EarnLeave> ELGetsClassified(string sSQL, long nUserID)
        {
            return EarnLeave.Service.ELGetsClassified(sSQL, nUserID);
        }
        public EarnLeave ELProcess(int EmpLeaveLedgerID, DateTime LastProcessDate, long nUserID)
        {
            return EarnLeave.Service.ELProcess(EmpLeaveLedgerID, LastProcessDate, nUserID);
        }
        public static List<EarnLeave> ELGetsToEncash(string sBusinessUnitIds, string sEmployeeIDs, string sLocationID, string sDepartmentIds, string sDesignationIDs, int nACSID, double nStartSalaryRange, double nEndSalaryRange, int nLoadRecordsS, int nRowLengthS, long nUserID)
        {
            return EarnLeave.Service.ELGetsToEncash(sBusinessUnitIds, sEmployeeIDs, sLocationID, sDepartmentIds, sDesignationIDs, nACSID, nStartSalaryRange, nEndSalaryRange, nLoadRecordsS, nRowLengthS, nUserID);
        }
        public int EncashEL(int nIndex, string sEmpLeaveLedgerIDs, DateTime DeclarationDate, int nACSID, int nConsiderEL, bool IsApplyforallemployee, bool IsEncashPresentBalance, long nUserID)
        {
            return EarnLeave.Service.EncashEL(nIndex, sEmpLeaveLedgerIDs, DeclarationDate, nACSID, nConsiderEL, IsApplyforallemployee, IsEncashPresentBalance, nUserID);
        }
        #endregion

        #region ServiceFactory
        internal static IEarnLeaveService Service
        {
            get { return (IEarnLeaveService)Services.Factory.CreateService(typeof(IEarnLeaveService)); }
        }

        #endregion
    }
    #endregion

    #region IEarnLeave interface

    public interface IEarnLeaveService
    {
        List<EarnLeave> ELGetsClassified(string sSQL, Int64 nUserId);
        List<EarnLeave> ELGets(string sEmployeeIDs, string sLocationID, string DepartmentIDs, string DesignationIDs, DateTime Date, string sBUnit, int nLoadRecordsS, int nRowLengthS, int isAll, Int64 nUserID);
        EarnLeave ELProcess(int EmpLeaveLedgerID, DateTime LastProcessDate, Int64 nUserId);
        List<EarnLeave> ELGetsToEncash(string sBusinessUnitIds, string sEmployeeIDs, string sLocationID, string sDepartmentIds, string sDesignationIDs, int nACSID, double nStartSalaryRange, double nEndSalaryRange, int nLoadRecordsS, int nRowLengthS, Int64 nUserID);
        int EncashEL(int nIndex, string sEmpLeaveLedgerIDs, DateTime DeclarationDate, int nACSID, int nConsiderEL, bool IsApplyforallemployee, bool IsEncashPresentBalance, Int64 nUserID);
    }
    #endregion
}
