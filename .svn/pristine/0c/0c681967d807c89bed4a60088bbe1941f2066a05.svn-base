using System;
using System.IO;
using ESimSol.BusinessObjects;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Collections.Generic;
using ICS.Core;
using ICS.Core.Framework;
using ICS.Core.Utility;

namespace ESimSol.BusinessObjects
{
    #region ServiceWork

    public class ServiceWork : BusinessObject
    {
        public ServiceWork()
        {
            ServiceWorkID = 0;
            ServiceCode = "";
            ServiceName = "";
            ServiceType = EnumServiceType.None;
            Remarks="";
            ErrorMessage = "";
        }

        #region Properties
        public int ServiceWorkID { get; set; }
        public string ServiceCode { get; set; }
        public string ServiceName { get; set; }
        public EnumServiceType ServiceType { get; set; }
        public string Remarks { get; set; }
        public string ErrorMessage { get; set; }
        #endregion

        #region Derived Property
        public string Param { get; set; }
        public int ServiceTypeInt 
        { 
            get { return (int)ServiceType; } 
        }
        public string ServiceTypeSt {
            get
            {
                return EnumObject.jGet(this.ServiceType);
            }
        }
        #endregion

        #region Functions

        public static List<ServiceWork> Gets(long nUserID)
        {
            return ServiceWork.Service.Gets(nUserID);
        }
        public static List<ServiceWork> Gets(string sSQL, Int64 nUserID)
        {
            return ServiceWork.Service.Gets(sSQL, nUserID);
        }

        public ServiceWork Get(int nId, long nUserID)
        {
            return ServiceWork.Service.Get(nId, nUserID);
        }

        public ServiceWork Save(long nUserID)
        {
            return ServiceWork.Service.Save(this, nUserID);
        }

        public string Delete(int nId, long nUserID)
        {
            return ServiceWork.Service.Delete(nId, nUserID);
        }
        public static List<ServiceWork> GetsByServiceCode(string ServiceCode, long nUserID)
        {
            return ServiceWork.Service.GetsByServiceCode(ServiceCode, nUserID);
        }
        public static List<ServiceWork> GetsByServiceNameWithType(string ServiceName,int ServiceType, long nUserID)
        {
            return ServiceWork.Service.GetsByServiceNameWithType(ServiceName,ServiceType, nUserID);
        }

        #endregion

        #region ServiceFactory
        internal static IServiceWorkService Service
        {
            get { return (IServiceWorkService)Services.Factory.CreateService(typeof(IServiceWorkService)); }
        }
        #endregion
    }
    #endregion

    #region IServiceWork interface

    public interface IServiceWorkService
    {

        ServiceWork Get(int id, long nUserID);

        List<ServiceWork> Gets(long nUserID);
        List<ServiceWork> Gets(string sSQL, Int64 nUserID);

        string Delete(int id, long nUserID);

        ServiceWork Save(ServiceWork oServiceWork, long nUserID);

        List<ServiceWork> GetsByServiceCode(string ServiceCode, long nUserID);

        List<ServiceWork> GetsByServiceNameWithType(string ServiceName,int ServiceType, long nUserID);
    }
    #endregion
}