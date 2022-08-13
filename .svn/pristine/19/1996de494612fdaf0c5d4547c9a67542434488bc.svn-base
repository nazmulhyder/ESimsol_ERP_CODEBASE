using System;
using System.IO;
using ESimSol.BusinessObjects;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Collections.Generic;
using ICS.Core.Framework;
using ICS.Core.Utility;

namespace ESimSol.BusinessObjects
{
    #region AccountsBook
    public class GeneralWorkingDay : BusinessObject
    {
        public GeneralWorkingDay()
        {
            GWDID = 0;
            GWDTitle = "";
            AttendanceDate = DateTime.Now;
            GWDApplyOn = EnumGWDApplyOn.None;
            GWDApplyOnInt = 0;
            Remarks = "";
            DBUserID = 0;
            DBServerDateTime = DateTime.Now;
            UserName = "";
            GeneralWorkingDayDetails = new List<GeneralWorkingDayDetail>();
            GeneralWorkingDayShifts = new List<GeneralWorkingDayShift>();
            ErrorMessage = "";
        }
        #region Properties
        public int GWDID { get; set; }
        public string GWDTitle { get; set; }
        public DateTime AttendanceDate { get; set; }
        public EnumGWDApplyOn GWDApplyOn { get; set; }
        public int GWDApplyOnInt { get; set; }
        public string Remarks { get; set; }
        public int DBUserID { get; set; }
        public DateTime DBServerDateTime { get; set; }
        public string UserName { get; set; }
        public List<GeneralWorkingDayDetail> GeneralWorkingDayDetails { get; set; }
        public List<GeneralWorkingDayShift> GeneralWorkingDayShifts { get; set; }
        public string ErrorMessage { get; set; }
        #endregion

        #region Derive Properties
        public string AttendanceDateInString
        {
            get
            {
                return this.AttendanceDate.ToString("dd MMM yyyy");
            }
        }

        public string GWDApplyOnSt
        {
            get
            {
                return EnumObject.jGet(this.GWDApplyOn);
            }
        }
        #endregion

        #region Functions

        public GeneralWorkingDay Save(long nUserID)
        {
            return GeneralWorkingDay.Service.Save(this, nUserID);
        }
        public string Delete(int nId, long nUserID)
        {
            return GeneralWorkingDay.Service.Delete(nId, nUserID);
        }
        public GeneralWorkingDay Get(int id, long nUserID)
        {
            return GeneralWorkingDay.Service.Get(id, nUserID);
        }
        public static List<GeneralWorkingDay> Gets(long nUserID)
        {
            return GeneralWorkingDay.Service.Gets(nUserID);
        }
        public static List<GeneralWorkingDay> Gets(string sSQL, long nUserID)
        {
            return GeneralWorkingDay.Service.Gets(sSQL, nUserID);
        }
        #endregion

        #region ServiceFactory
        internal static IGeneralWorkingDayService Service
        {
            get { return (IGeneralWorkingDayService)Services.Factory.CreateService(typeof(IGeneralWorkingDayService)); }
        }
        #endregion
    }
    #endregion

    #region IGeneralWorkingDay interface
    public interface IGeneralWorkingDayService
    {
        string Delete(int id, long nUserID);
        GeneralWorkingDay Get(int id, Int64 nUserID);
        GeneralWorkingDay Save(GeneralWorkingDay oGeneralWorkingDay, long nUserID);
        List<GeneralWorkingDay> Gets(long nUserID);
        List<GeneralWorkingDay> Gets(string sSQL, Int64 nUserID);

    }
    #endregion
}
