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
    #region VehicleType

    public class VehicleType : BusinessObject
    {
        public VehicleType()
        {
            VehicleTypeID = 0;
            VehicleTypeCode = "";
            VehicleTypeName = "";
            Remarks="";
            ErrorMessage = "";
        }

        #region Properties
        public int VehicleTypeID { get; set; }
        public string VehicleTypeCode { get; set; }
        public string VehicleTypeName { get; set; }
        public string Remarks { get; set; }
        public string ErrorMessage { get; set; }
        #endregion

        #region Derived Property
        public string Param { get; set; }
      
        #endregion

        #region Functions

        public static List<VehicleType> Gets(long nUserID)
        {
            return VehicleType.Service.Gets(nUserID);
        }
        public static List<VehicleType> Gets(string sSQL, Int64 nUserID)
        {
            return VehicleType.Service.Gets(sSQL, nUserID);
        }

        public VehicleType Get(int nId, long nUserID)
        {
            return VehicleType.Service.Get(nId, nUserID);
        }

        public VehicleType Save(long nUserID)
        {
            return VehicleType.Service.Save(this, nUserID);
        }

        public string Delete(int nId, long nUserID)
        {
            return VehicleType.Service.Delete(nId, nUserID);
        }
        #endregion

        #region ServiceFactory
        internal static IVehicleTypeService Service
        {
            get { return (IVehicleTypeService)Services.Factory.CreateService(typeof(IVehicleTypeService)); }
        }
        #endregion
    }
    #endregion

    #region IVehicleType interface

    public interface IVehicleTypeService
    {
        VehicleType Get(int id, long nUserID);
        List<VehicleType> Gets(long nUserID);
        List<VehicleType> Gets(string sSQL, Int64 nUserID);
        string Delete(int id, long nUserID);
        VehicleType Save(VehicleType oVehicleType, long nUserID);
    }
    #endregion
}