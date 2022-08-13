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
    #region ManualAttendance
    [DataContract]
    public class PunchLog : BusinessObject
    {
        public PunchLog()
        {
            PunchLogID = 0;
            MachineSLNo = "";
            EmployeeID = 0;
            LocationID = 0;
            DepartmentID = 0;
            DesignationID = 0;
            CardNo = "";
            PunchDateTime = DateTime.Now;
            ErrorMessage = "";

            //Derive
            
            EmployeeName = "";
            EmployeeCode = "";
            DepartmentName = "";
            DesignationName = "";
            LocationName = "";
            BUName = "";
            PunchDateTime_ST = "";
            sParam = "";
            
        }

        #region Properties
        public int PunchLogID { get; set; }
        public string sParam { get; set; }
        public string MachineSLNo { get; set; }
        public int EmployeeID { get; set; }
        public int LocationID { get; set; }
        public int DepartmentID { get; set; }
        public int DesignationID { get; set; }
        public string CardNo { get; set; }
        public DateTime PunchDateTime { get; set; }
        public string ErrorMessage { get; set; }
        public string PunchDateTime_ST { get; set; }
        #endregion

        #region Derived Property
        public string EmployeeName { get; set; }
        public string EmployeeCode { get; set; }
        public string DepartmentName { get; set; }
        public string DesignationName { get; set; }
        public string LocationName { get; set; }
        public string BUName { get; set; }
        public string PunchTimeInString
        {
            get
            {
                return PunchDateTime.ToString("HH:mm");
            }
        }

        public string PunchTimeInStringAMPM
        {
            get
            {
                if (PunchDateTime.ToString("HH:mm") != "00:00")
                    return PunchDateTime.ToString("hh:mm tt");
                else
                    return "-";
            }
        }
        public string PunchDateInString
        {
            get
            {
                return PunchDateTime.ToString("dd MMM yyy");
            }
        }

        #endregion

        #region Functions
        public static PunchLog Get(int id, long nUserID)
        {
            return PunchLog.Service.Get(id, nUserID);
        }

        public static PunchLog Get(string sSQL, long nUserID)
        {
            return PunchLog.Service.Get(sSQL, nUserID);
        }

        public static List<PunchLog> Gets(long nUserID)
        {
            return PunchLog.Service.Gets(nUserID);
        }

        public static List<PunchLog> Gets(string sSQL, long nUserID)
        {
            return PunchLog.Service.Gets(sSQL, nUserID);
        }

        public PunchLog IUD(string  sEmployeeIds,DateTime dtPunchTime, int nDBOperation, long nUserID)
        {
            return PunchLog.Service.IUD(sEmployeeIds,dtPunchTime, nDBOperation, nUserID);
        }
        //public string Delete(int id,DateTime dtPunchTime, long nUserID)
        //{
        //    return PunchLog.Service.Delete(id, dtPunchTime, nUserID);
        //}
        public bool Delete(string ids, string dtPunchTime, long nUserID)
        {
            return PunchLog.Service.Delete(ids, dtPunchTime, nUserID);
        }

        public static List<PunchLog> UploadXL(List<PunchLog> oPunchLogXLs, long nUserID)
        {
            return PunchLog.Service.UploadXL(oPunchLogXLs, nUserID);
        }

        #endregion

        #region ServiceFactory
        internal static IPunchLogService Service
        {
            get { return (IPunchLogService)Services.Factory.CreateService(typeof(IPunchLogService)); }
        }
        #endregion
    }
    #endregion

    #region IPunchLog interface

    public interface IPunchLogService
    {
        PunchLog Get(int id, Int64 nUserID);

        PunchLog Get(string sSQL, Int64 nUserID);

        List<PunchLog> Gets(Int64 nUserID);

        List<PunchLog> Gets(string sSQL, Int64 nUserID);

        PunchLog IUD(string sEmployeeIds,DateTime dtPunchTime, int nDBOperation, Int64 nUserID);

        //string Delete(int id, DateTime dtPunchTime, Int64 nUserID);
        bool Delete(string ids, string dtPunchTime, Int64 nUserID);

        List<PunchLog> UploadXL(List<PunchLog> oPunchLogXLs, Int64 nUserID);
    }
    #endregion
}
