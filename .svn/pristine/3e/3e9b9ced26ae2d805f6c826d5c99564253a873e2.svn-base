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
    #region EmployeeRequestOnAttendance
    public class EmployeeRequestOnAttendance
    {
        public EmployeeRequestOnAttendance()
        {
            EROAID = 0;
            EmployeeID = 0;
            EmployeeCode = "";
            AttendanceDate = DateTime.Now;
            IsOSD = 0;
            Remark = "";
            ApproveBy = 0;
            ApproveDate = DateTime.Now;
            ApproveByName = "";
            ErrorMessage = "";
            EmployeeName = "";
            CancelBy = 0;
            CancelDate = DateTime.Now;
            RecommendPersonID = 0;
        }

        #region Properties
        public int EROAID { get; set; }
        public int RecommendPersonID { get; set; }
        public int CancelBy { get; set; }
        public int EmployeeID { get; set; }
        public DateTime AttendanceDate { get; set; }
        public DateTime CancelDate { get; set; }
        public int IsOSD { get; set; }
        public string Remark { get; set; }
        public string EmployeeName { get; set; }
        public string EmployeeCode { get; set; }
        public int ApproveBy { get; set; }
        public DateTime ApproveDate { get; set; }
        public string ApproveByName { get; set; }
        public string ErrorMessage { get; set; }

        public string status
        {
            get
            {
                return this.ApproveBy<=0?"Applied":"Approved";
            }
        }
        #endregion

        #region Functions

        public string IsOSDInString
        {
            get
            {
                return (IsOSD > 0 ? "\u221A" : "x");
            }
        }
        public string ApproveDateInString
        {
            get
            {
                if (ApproveDate == DateTime.MinValue)
                {
                    return "-";
                }
                else
                {
                    return ApproveDate.ToString("dd MMM yyyy");
                }
            }
        }
        public string CancelDateInString
        {
            get
            {
                if (CancelDate == DateTime.MinValue)
                {
                    return "-";
                }
                else
                {
                    return CancelDate.ToString("dd MMM yyyy");
                }
            }
        }
        public string AttendanceDateInString
        {
            get
            {
                if (AttendanceDate == DateTime.MinValue)
                {
                    return "-";
                }
                else
                {
                    return AttendanceDate.ToString("dd MMM yyyy");
                }
            }
        }

        public static List<EmployeeRequestOnAttendance> Gets(string sSQL, long nUserID)
        {
            return EmployeeRequestOnAttendance.Service.Gets(sSQL, nUserID);
        }
        public static EmployeeRequestOnAttendance Get(string sSQL, long nUserID)
        {
            return EmployeeRequestOnAttendance.Service.Get(sSQL, nUserID);
        }
        public EmployeeRequestOnAttendance IUD(int nDBOperation, long nUserID)
        {
            return EmployeeRequestOnAttendance.Service.IUD(this, nDBOperation, nUserID);
        }
        public static List<EmployeeRequestOnAttendance> GetHierarchy(string sEmployeeIDs, long nUserID)
        {
            return EmployeeRequestOnAttendance.Service.GetHierarchy(sEmployeeIDs, nUserID);
        }
        #endregion

        #region ServiceFactory

        internal static IEmployeeRequestOnAttendanceService Service
        {
            get { return (IEmployeeRequestOnAttendanceService)Services.Factory.CreateService(typeof(IEmployeeRequestOnAttendanceService)); }
        }
        #endregion
    }
    #endregion

    #region IEmployeeRequestOnAttendance interface

    public interface IEmployeeRequestOnAttendanceService
    {
        List<EmployeeRequestOnAttendance> Gets(string sSQL, Int64 nUserID);
        EmployeeRequestOnAttendance Get(string sSQL, Int64 nUserID);
        EmployeeRequestOnAttendance IUD(EmployeeRequestOnAttendance oEmployeeRequestOnAttendance, int nDBOperation, Int64 nUserID);
        List<EmployeeRequestOnAttendance> GetHierarchy(string sEmployeeIDs, Int64 nUserID);
        
    }
    #endregion
}


