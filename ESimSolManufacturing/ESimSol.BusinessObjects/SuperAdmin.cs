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
    #region SuperAdmin
    public class SuperAdmin
    {
        public SuperAdmin()
        {

            StartDate = DateTime.Now;
            EndDate = DateTime.Now;
            IsComp = true;
            ErrorMessage = "";
        }

        #region Properties
        public string ErrorMessage { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public bool IsComp { get; set; }
        #endregion

        #region Functions


        public static SuperAdmin MakeDayoffHoliday(string sDate, string eDate, bool isComp, long nUserID)
        {
            return SuperAdmin.Service.MakeDayoffHoliday(sDate, eDate, isComp, nUserID);
        }
 
        #endregion

        #region ServiceFactory

        internal static ISuperAdminService Service
        {
            get { return (ISuperAdminService)Services.Factory.CreateService(typeof(ISuperAdminService)); }
        }
        #endregion
    }
    #endregion

    #region IAttendanceDaily interface

    public interface ISuperAdminService
    {
        SuperAdmin MakeDayoffHoliday(string sDate, string eDate, bool isComp, Int64 nUserID);
    }
    #endregion
}

