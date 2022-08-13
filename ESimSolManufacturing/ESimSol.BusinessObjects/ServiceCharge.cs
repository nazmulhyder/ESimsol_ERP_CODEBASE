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
    #region ServiceCharge
    public class ServiceCharge
    {
        public ServiceCharge()
        {

            ServiceChargeID = 0;
            Name = "";
            Note = "";
            ErrorMessage = "";
        }

        #region Properties
        public int ServiceChargeID { get; set; }
        public string Name { get; set; }
        public string Note { get; set; }
        public string ErrorMessage { get; set; }
        #endregion

        #region Functions


        public static List<ServiceCharge> Gets(string sSQL, long nUserID)
        {
            return ServiceCharge.Service.Gets(sSQL, nUserID);
        }
        public static ServiceCharge Get(string sSQL, long nUserID)
        {
            return ServiceCharge.Service.Get(sSQL, nUserID);
        }
        public static ServiceCharge Get(int id, long nUserID)
        {
            return ServiceCharge.Service.Get(id, nUserID);
        }
        public ServiceCharge IUD(int nDBOperation, long nUserID)
        {
            return ServiceCharge.Service.IUD(this, nDBOperation, nUserID);
        }
 
        #endregion

        #region ServiceFactory

        internal static IServiceChargeService Service
        {
            get { return (IServiceChargeService)Services.Factory.CreateService(typeof(IServiceChargeService)); }
        }
        #endregion
    }
    #endregion

    #region IAttendanceDaily interface

    public interface IServiceChargeService
    {
        List<ServiceCharge> Gets(string sSQL, Int64 nUserID);
        ServiceCharge Get(string sSQL, Int64 nUserID);
        ServiceCharge Get(int id, Int64 nUserID);
        ServiceCharge IUD(ServiceCharge oServiceCharge, int nDBOperation, Int64 nUserID);
       
      
    }
    #endregion
}

