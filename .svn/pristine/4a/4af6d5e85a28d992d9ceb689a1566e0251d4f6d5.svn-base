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
    #region MaxOTConfiguration
    public class MaxOTConfiguration : BusinessObject
    {
        public MaxOTConfiguration()
        {
            MOCID = 0;
            MaxOTInMin = 0;
            MaxBeforeInMin = 0;
            ErrorMessage = "";
            Sequence = 0;
            TimeCardName = "";
            IsEnum = false;
            UserID = 0;
            MOCUID = 0;
            IsPresentOnDayOff = false;
            IsPresentOnHoliday = false;
            IsActive = false;
            LastUpdateBy = 0;
            UserName = "";
            LastUpdateByName = "";
            SourceTimeCardID = 0;
            SourceTimeCardName = "";
            MaxOTCEmployeeTypes = new List<MaxOTCEmployeeType>();
        }
        #region Properties
        public bool IsEnum { get; set; }
        public bool IsPresentOnDayOff { get; set; }
        public bool IsPresentOnHoliday { get; set; }
        public int MOCID { get; set; }
        public int MOCUID { get; set; }
        public int UserID { get; set; }
        public int Sequence { get; set; }
        public int MaxOTInMin { get; set; }
        public int MaxBeforeInMin { get; set; }
        public string TimeCardName { get; set; }
        public string ErrorMessage { get; set; }
        public bool IsActive { get; set; }
        public int LastUpdateBy { get; set; }
        public string UserName { get; set; }
        public string LastUpdateByName { get; set; }
        public int SourceTimeCardID { get; set; }
        public string SourceTimeCardName { get; set; }
        public List<MaxOTCEmployeeType> MaxOTCEmployeeTypes { get; set; }

        #endregion

        #region Derive Properties
        public string IsPresentOnDayOffSt
        {
            get
            {
                if (this.IsPresentOnDayOff)
                {
                    return "Yes";
                }
                else
                {
                    return "No";
                }
            }
        }
        public string IsPresentOnHolidaySt
        {
            get
            {
                if (this.IsPresentOnHoliday)
                {
                    return "Yes";
                }
                else
                {
                    return "No";
                }
            }
        }

        public string ActivityStatus
        {
            get
            {
                if (IsActive == true)
                {
                    return "Active";
                }
                else
                {
                    return "InActive";
                }
            }
        }

        public string MaxOTInMinHourSt
        {
            get
            {
                string S = "";
                if (MaxOTInMin > 0)
                {
                    if (MaxOTInMin / 60 >= 1) { S += ((MaxOTInMin - MaxOTInMin % 60) / 60).ToString() + "h "; }
                    if (MaxOTInMin % 60 != 0) { S += (MaxOTInMin % 60).ToString() + "m"; }
                    return S;
                }
                else if (MaxOTInMin < 0)
                {

                    if ((MaxOTInMin * (-1)) / 60 >= 1) { S += (((MaxOTInMin * (-1)) - (MaxOTInMin * (-1)) % 60) / 60).ToString() + "h "; }
                    if ((MaxOTInMin * (-1)) % 60 != 0) { S += ((MaxOTInMin * (-1)) % 60).ToString() + "m"; }
                    return "(" + S + ")";
                }
                else return "-";
            }
        }

        public string MaxBeforeInMinHourSt
        {
            get
            {
                string S = "";
                if (MaxBeforeInMin > 0)
                {
                    if (MaxBeforeInMin / 60 >= 1) { S += ((MaxBeforeInMin - MaxBeforeInMin % 60) / 60).ToString() + "h "; }
                    if (MaxBeforeInMin % 60 != 0) { S += (MaxBeforeInMin % 60).ToString() + "m"; }
                    return S;
                }
                else if (MaxBeforeInMin < 0)
                {

                    if ((MaxBeforeInMin * (-1)) / 60 >= 1) { S += (((MaxBeforeInMin * (-1)) - (MaxBeforeInMin * (-1)) % 60) / 60).ToString() + "h "; }
                    if ((MaxBeforeInMin * (-1)) % 60 != 0) { S += ((MaxBeforeInMin * (-1)) % 60).ToString() + "m"; }
                    return "(" + S + ")";
                }
                else return "-";
            }
        }

        #endregion

        #region Functions

        public MaxOTConfiguration Save(long nUserID)
        {
            return MaxOTConfiguration.Service.Save(this, nUserID);
        }
        public string Delete(long nUserID)
        {
            return MaxOTConfiguration.Service.Delete(this, nUserID);
        }
        public static List<MaxOTConfiguration> Gets(long nUserID)
        {
            return MaxOTConfiguration.Service.Gets(nUserID);
        }
        public static List<MaxOTConfiguration> Gets(bool bIsActive, long nUserID)
        {
            return MaxOTConfiguration.Service.Gets(bIsActive, nUserID);
        }
        public static List<MaxOTConfiguration> GetsDayoff(long nUserID)
        {
            return MaxOTConfiguration.Service.GetsDayoff(nUserID);
        }
        public static List<MaxOTConfiguration> GetsByUser(long nUserID)
        {
            return MaxOTConfiguration.Service.GetsByUser(nUserID);
        }
        public MaxOTConfiguration Activity(long nUserID)
        {
            return MaxOTConfiguration.Service.Activity(this, nUserID);
        }

        public static List<MaxOTConfiguration> Gets(string sSQL, long nUserID)
        {
            return MaxOTConfiguration.Service.Gets(sSQL, nUserID);
        }

        public MaxOTConfiguration Get(int id, long nUserID)
        {
            return MaxOTConfiguration.Service.Get(id, nUserID);
        }

        #endregion

        #region ServiceFactory
        internal static IMaxOTConfigurationService Service
        {
            get { return (IMaxOTConfigurationService)Services.Factory.CreateService(typeof(IMaxOTConfigurationService)); }
        }
        #endregion
    }
    #endregion

    #region IMaxOTConfiguration interface
    public interface IMaxOTConfigurationService
    {
        string Delete(MaxOTConfiguration oMaxOTConfiguration, long nUserID);

        MaxOTConfiguration Save(MaxOTConfiguration oMaxOTConfiguration, long nUserID);
        MaxOTConfiguration Activity(MaxOTConfiguration oMaxOTConfiguration, Int64 nUserID);
        List<MaxOTConfiguration> Gets(long nUserID);
        List<MaxOTConfiguration> Gets(bool bIsActive, long nUserID);
        List<MaxOTConfiguration> GetsDayoff(long nUserID);
        List<MaxOTConfiguration> GetsByUser(long nUserID);
        List<MaxOTConfiguration> Gets(string sSQL, long nUserID);
        MaxOTConfiguration Get(int id, long nUserID);

    }
    #endregion
}

