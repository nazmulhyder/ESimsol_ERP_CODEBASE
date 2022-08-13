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
    #region ServiceBookLeave
    public class ServiceBookLeave : BusinessObject
    {
        public ServiceBookLeave()
        {
            StartDate = DateTime.Now;
            EndDate = DateTime.Now;
            Leave ="";
            LeaveTaken =0;
            ErrorMessage = "";
        }

        #region Properties
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string Leave { get; set; }
        public int LeaveTaken { get; set; }
      
        public string ErrorMessage { get; set; }
        #endregion

        #region Derived Property
        public string StartDateInString
        {
            get
            {
                return StartDate.ToString("dd MMM yyyy");
            }
        }
        public string EndDateInString
        {
            get
            {
                return EndDate.ToString("dd MMM yyyy");
            }
        }
        #endregion

        #region Functions
        public static List<ServiceBookLeave> Gets(int nEmployeeID, long nUserID)
        {
            return ServiceBookLeave.Service.Gets(nEmployeeID, nUserID);
        }
        #endregion

        #region ServiceFactory
        internal static IServiceBookLeaveService Service
        {
            get { return (IServiceBookLeaveService)Services.Factory.CreateService(typeof(IServiceBookLeaveService)); }
        }

        #endregion
    }
    #endregion

    #region IServiceBookLeave interface

    public interface IServiceBookLeaveService
    {
        List<ServiceBookLeave> Gets(int nEmployeeID, Int64 nUserID);
    }
    #endregion
}
