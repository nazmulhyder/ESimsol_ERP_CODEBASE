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
    #region VehicleChassis

    public class VehicleChassis : BusinessObject
    {
        public VehicleChassis()
        {
            VehicleChassisID = 0;
            FileNo = "";
            ChassisNo = "";
            ManufacturerID = 0;
            ManufacturerName = "";
            EnginePosition="";
            EngineLayout="";
            DriveWheels="";
            TorqueSplit="";
            Steering="";
            WheelSizeFront="";
            WheelSizeRear="";
            TyresFront="";
            TyresRear="";
            BrakesFR="";
            FrontBrakeDiameter="";
            RearBrakeDiameter="";
            Gearbox="";
            TopGearRatio="";
            FinalDriveRatio="";
            Remarks = "";
            ErrorMessage = "";

            VehicleChassiss = new List<VehicleChassis>();
        }

        #region Properties

        public int VehicleChassisID { get; set; }
        public string FileNo { get; set; }
        public string ChassisNo { get; set; }
        public string ManufacturerName { get; set; }
        public int ManufacturerID { get; set; }
        public string EnginePosition { get; set; }
        public string EngineLayout { get; set; }
        public string DriveWheels { get; set; }
        public string TorqueSplit { get; set; }
        public string Steering { get; set; }
        public string WheelSizeFront { get; set; }
        public string WheelSizeRear { get; set; }
        public string TyresFront { get; set; }
        public string TyresRear { get; set; }
        public string BrakesFR { get; set; }
        public string FrontBrakeDiameter { get; set; }
        public string RearBrakeDiameter { get; set; }
        public string Gearbox { get; set; }
        public string TopGearRatio { get; set; }
        public string FinalDriveRatio { get; set; }
        public string Remarks { get; set; }
        public string ErrorMessage { get; set; }


        #endregion

        #region Derived Property
        public List<VehicleChassis> VehicleChassiss { get; set; }
        public string Param { get; set; }

        #endregion

        #region Functions

        public static List<VehicleChassis> Gets(long nUserID)
        {
            return VehicleChassis.Service.Gets(nUserID);
        }

        public static List<VehicleChassis> GetsByChassisNo(string sChassisNo,long nUserID)
        {
            return VehicleChassis.Service.GetsByChassisNo(sChassisNo,nUserID);
        }
        public static List<VehicleChassis> Gets(string sSQL, Int64 nUserID)
        {
            return VehicleChassis.Service.Gets(sSQL, nUserID);
        }

        public static VehicleChassis Get(int nId, long nUserID)
        {
            return VehicleChassis.Service.Get(nId, nUserID);
        }

        public VehicleChassis Save(long nUserID)
        {
            return VehicleChassis.Service.Save(this, nUserID);
        }

        public string Delete(int nId, long nUserID)
        {
            return VehicleChassis.Service.Delete(nId, nUserID);
        }


        #endregion

        #region ServiceFactory
        internal static IVehicleChassisService Service
        {
            get { return (IVehicleChassisService)Services.Factory.CreateService(typeof(IVehicleChassisService)); }
        }
        #endregion
    }
    #endregion

    #region IVehicleChassis interface

    public interface IVehicleChassisService
    {

        VehicleChassis Get(int id, long nUserID);

        List<VehicleChassis> Gets(long nUserID);
        List<VehicleChassis> Gets(string sSQL, Int64 nUserID);

        List<VehicleChassis> GetsByChassisNo(string sChassisNo, long nUserID);
        string Delete(int id, long nUserID);

        VehicleChassis Save(VehicleChassis oVehicleChassis, long nUserID);
    }
    #endregion
}