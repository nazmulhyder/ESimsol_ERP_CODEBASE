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
    #region AttendanceAccessPoint

    public class AttendanceAccessPoint : BusinessObject
    {
        public AttendanceAccessPoint()
        {
            AAPID = 0;
            Name = "";
            MachineSLNo = "";
            Note = "";
            DataProvider = EnumDataProvider.None;
            DataSource = "";
            DBLoginID = "";
            DBPassword = "";
            DBName = "";
            IsThisPC = false;
            IsActive = false;
            ErrorMessage = "";
            Params = "";
            Query = "";
            AAPEs = new List<AttendanceAccessPointEmployee>();
        }

        #region Properties

        public int AAPID { get; set; }
        public string Name { get; set; }
        public string MachineSLNo { get; set; }
        public string Note { get; set; }
        public EnumDataProvider DataProvider { get; set; }
        public string DataSource { get; set; }
        public string DBLoginID { get; set; }
        public string DBPassword { get; set; }
        public string DBName { get; set; }
        public bool IsThisPC { get; set; }
        public bool IsActive { get; set; }
        public string ErrorMessage { get; set; }
        public string Params { get; set; }
        public string Query { get; set; }

        #endregion

        #region Derived Property

        public int DataProviderInInt { get; set; }
        public List<AttendanceAccessPointEmployee> AAPEs { get; set; }
        public string PCInfo { get { if (this.IsThisPC) return "Current PC"; else return "Other PC"; } }
        public string ActivityInStr { get { if (this.IsActive) return "Active"; else return "Inactive"; } }

        public string DataProviderInStr
        {
            get
            {
                return this.DataProvider.ToString();
            }
        }
        #endregion

        #region Functions

        public static AttendanceAccessPoint Get(int nAAPID, long nUserID)
        {
            return AttendanceAccessPoint.Service.Get(nAAPID, nUserID);
        }
        public static List<AttendanceAccessPoint> Gets(string sSQL, long nUserID)
        {
            return AttendanceAccessPoint.Service.Gets(sSQL, nUserID);
        }
        public AttendanceAccessPoint IUD(int nDBOperation, long nUserID)
        {
            return AttendanceAccessPoint.Service.IUD(this, nDBOperation, nUserID);
        }


        #endregion

        #region ServiceFactory
        internal static IAttendanceAccessPointService Service
        {
            get { return (IAttendanceAccessPointService)Services.Factory.CreateService(typeof(IAttendanceAccessPointService)); }
        }

        #endregion
    }
    #endregion

    #region IAttendanceAccessPoint interface

    public interface IAttendanceAccessPointService
    {

        AttendanceAccessPoint Get(int nAAPID, Int64 nUserID);
        List<AttendanceAccessPoint> Gets(string sSQL, Int64 nUserID);
        AttendanceAccessPoint IUD(AttendanceAccessPoint oAttendanceAccessPoint, int nDBOperation, Int64 nUserID);
    }
    #endregion
}
