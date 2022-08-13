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
    #region VehicleColor

    public class VehicleColor : BusinessObject
    {
        public VehicleColor()
        {
            VehicleColorID = 0;
            ColorCode = "";
            ColorName = "";
            ColorType=0;
            Remarks="";
            ErrorMessage = "";
            Param = "";
            VehicleColors = new List<VehicleColor>();
        }

        #region Properties

        public int VehicleColorID { get; set; }

        public string ColorCode { get; set; }

        public string ColorName { get; set; }

        public int ColorType { get; set; }

        public string Remarks { get; set; }
        public string ErrorMessage { get; set; }
        #endregion

        #region Derived Property

        public List<VehicleColor> VehicleColors { get; set; }
        public string Param { get; set; }

        public string ColorTypeInString { 
            get { 
                   return Enum.GetName(typeof(EnumColorType),this.ColorType).ToString();
                }
        }
        #endregion

        #region Functions

        public static List<VehicleColor> Gets(long nUserID)
        {
            return VehicleColor.Service.Gets(nUserID);
        }
        public static List<VehicleColor> Gets(string sSQL, Int64 nUserID)
        {
            return VehicleColor.Service.Gets(sSQL, nUserID);
        }

        public VehicleColor Get(int nId, long nUserID)
        {
            return VehicleColor.Service.Get(nId, nUserID);
        }

        public VehicleColor Save(long nUserID)
        {
            return VehicleColor.Service.Save(this, nUserID);
        }

        public string Delete(int nId, long nUserID)
        {
            return VehicleColor.Service.Delete(nId, nUserID);
        }
        public static List<VehicleColor> GetsByColorCode(string ColorCode, long nUserID)
        {
            return VehicleColor.Service.GetsByColorCode(ColorCode, nUserID);
        }
        public static List<VehicleColor> GetsByColorNameWithType(string ColorName,int ColorType, long nUserID)
        {
            return VehicleColor.Service.GetsByColorNameWithType(ColorName,ColorType, nUserID);
        }

        #endregion

        #region ServiceFactory
        internal static IVehicleColorService Service
        {
            get { return (IVehicleColorService)Services.Factory.CreateService(typeof(IVehicleColorService)); }
        }
        #endregion
    }
    #endregion

    #region IVehicleColor interface

    public interface IVehicleColorService
    {

        VehicleColor Get(int id, long nUserID);

        List<VehicleColor> Gets(long nUserID);
        List<VehicleColor> Gets(string sSQL, Int64 nUserID);

        string Delete(int id, long nUserID);

        VehicleColor Save(VehicleColor oVehicleColor, long nUserID);

        List<VehicleColor> GetsByColorCode(string ColorCode, long nUserID);

        List<VehicleColor> GetsByColorNameWithType(string ColorName,int ColorType, long nUserID);
    }
    #endregion
}