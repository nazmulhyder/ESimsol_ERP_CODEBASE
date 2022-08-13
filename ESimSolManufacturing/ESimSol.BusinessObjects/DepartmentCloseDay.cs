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

    #region DepartmentCloseDay

    public class DepartmentCloseDay : BusinessObject
    {
        public DepartmentCloseDay()
        {
            DepartmentCloseDayID = 0;
            DepartmentRequirementPolicyID = 0;
            WeekDay = "";
            ErrorMessage = "";
        }

        #region Properties
        public int DepartmentCloseDayID { get; set; }
        public int DepartmentRequirementPolicyID { get; set; }
        public string WeekDay { get; set; }
        public string ErrorMessage { get; set; }

        #endregion

        #region Derived Property
        public string DepartmentRequirementPolicyName { get; set; }
        public string DayName { get; set; }

        #endregion

        #region Functions
        public static List<DepartmentCloseDay> Gets(long nUserID)
        {
            return DepartmentCloseDay.Service.Gets(nUserID);
        }
        public static List<DepartmentCloseDay> Gets(int nDepartmentRequirementPolicyID, long nUserID)
        {
            return DepartmentCloseDay.Service.Gets(nDepartmentRequirementPolicyID, nUserID);
        }
        public static List<DepartmentCloseDay> Gets(string sSQL, long nUserID)
        {
            return DepartmentCloseDay.Service.Gets(sSQL, nUserID);
        }

        //public DepartmentCloseDay Get(int id, long nUserID)
        //{
        //    return (DepartmentCloseDay)ICSWCFServiceClient.CallMethod(ServiceType, wcfSessionid, "Get", id)[0];
        //}

        public DepartmentCloseDay IUD(int nDBOperation, long nUserID)
        {
            return DepartmentCloseDay.Service.IUD(this, nDBOperation, nUserID);
        }
        #endregion

        #region ServiceFactory
        internal static IDepartmentCloseDayService Service
        {
            get { return (IDepartmentCloseDayService)Services.Factory.CreateService(typeof(IDepartmentCloseDayService)); }
        }

        #endregion
    }
    #endregion

    #region IDepartmentCloseDay interface

    public interface IDepartmentCloseDayService
    {
        

        List<DepartmentCloseDay> Gets(Int64 nUserID);
        List<DepartmentCloseDay> Gets(int nDepartmentRequirementPolicyID, Int64 nUserID);
        List<DepartmentCloseDay> Gets(string sSQL, Int64 nUserID);
        DepartmentCloseDay IUD(DepartmentCloseDay oDepartmentCloseDay, int nDBOperation, Int64 nUserID);
    }
    #endregion

}
