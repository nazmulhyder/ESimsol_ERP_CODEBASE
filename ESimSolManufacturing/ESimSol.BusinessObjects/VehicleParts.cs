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
    #region VehicleParts

    public class VehicleParts : BusinessObject
    {
        public VehicleParts()
        {
            VehiclePartsID = 0;
            PartsCode = "";
            PartsName = "";
            PartsType = 0;
            Remarks = "";
            ErrorMessage = "";
            Param = "";
            VehiclePartss = new List<VehicleParts>();
        }

        #region Properties

        public int VehiclePartsID { get; set; }

        public string PartsCode { get; set; }

        public string PartsName { get; set; }

        public int PartsType { get; set; }

        public string Remarks { get; set; }
        public string ErrorMessage { get; set; }
        #endregion

        #region Derived Property

        public List<VehicleParts> VehiclePartss { get; set; }
        public string Param { get; set; }

        public string PartsTypeInString
        {
            get
            {
                return Enum.GetName(typeof(EnumPartsType), this.PartsType).ToString();
            }
        }
        #endregion

        #region Functions

        public static List<VehicleParts> Gets(long nUserID)
        {
            return VehicleParts.Service.Gets(nUserID);
        }
        public static List<VehicleParts> Gets(string sSQL, Int64 nUserID)
        {
            return VehicleParts.Service.Gets(sSQL, nUserID);
        }

        public VehicleParts Get(int nId, long nUserID)
        {
            return VehicleParts.Service.Get(nId, nUserID);
        }

        public VehicleParts Save(long nUserID)
        {
            return VehicleParts.Service.Save(this, nUserID);
        }

        public string Delete(int nId, long nUserID)
        {
            return VehicleParts.Service.Delete(nId, nUserID);
        }
        public static List<VehicleParts> GetsByPartsCode(string PartsCode, long nUserID)
        {
            return VehicleParts.Service.GetsByPartsCode(PartsCode, nUserID);
        }
        public static List<VehicleParts> GetsByPartsNameWithType(string PartsName, int PartsType, long nUserID)
        {
            return VehicleParts.Service.GetsByPartsNameWithType(PartsName, PartsType, nUserID);
        }

        #endregion

        #region ServiceFactory
        internal static IVehiclePartsService Service
        {
            get { return (IVehiclePartsService)Services.Factory.CreateService(typeof(IVehiclePartsService)); }
        }
        #endregion
    }
    #endregion

    #region IVehicleParts interface

    public interface IVehiclePartsService
    {

        VehicleParts Get(int id, long nUserID);

        List<VehicleParts> Gets(long nUserID);
        List<VehicleParts> Gets(string sSQL, Int64 nUserID);

        string Delete(int id, long nUserID);

        VehicleParts Save(VehicleParts oVehicleParts, long nUserID);

        List<VehicleParts> GetsByPartsCode(string PartsCode, long nUserID);

        List<VehicleParts> GetsByPartsNameWithType(string PartsName, int PartsType, long nUserID);
    }
    #endregion
}