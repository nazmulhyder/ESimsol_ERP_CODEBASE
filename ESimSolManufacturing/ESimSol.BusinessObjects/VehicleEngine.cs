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
    #region VehicleEngine

    public class VehicleEngine : BusinessObject
    {
        public VehicleEngine()
        {
           
            VehicleEngineID=0;
            FileNo="";
            EngineNo="";
            FuelType = 0;
            ManufacturerName = "";
            ManufacturerID=0;
            Cylinders="";
            Capacity="";
            BoreStroke="";
            BoreStrokeRation="";
            MaxPowerOutput="";
            SpecificOutput="";
            MaximumTorque="";
            SpecificTorque="";
            EngineConstruction="";
            Sump="";
            CompressionRatio="";
            FuelSystem="";
            BMEP="";
            EngineCoolant="";
            UnitaryCapacity="";
            Aspiration="";
            CatalyticConverter="";
            YearOfManufactureID=0;
            CountryOfOrigin="";
            Transmission = "";
            Remarks="";
            ErrorMessage = "";
            YearOfManufacture = "";
            YearOfModel = "";
            YearOfModelID = 0;
            VehicleEngines = new List<VehicleEngine>();
        }
          
        #region Properties
        public int VehicleEngineID {get; set;}
        public string FileNo {get; set;}
        public string EngineNo {get; set;}
        public string EngineType {get; set;}
        public int FuelType { get; set; }
        public string ManufacturerName { get; set; }
        public int ManufacturerID {get; set;}
        public string Cylinders {get; set;}
        public string Capacity {get; set;}
        public string BoreStroke {get; set;}
        public string BoreStrokeRation {get; set;}
        public string MaxPowerOutput {get; set;}
        public string SpecificOutput {get; set;}
        public string MaximumTorque {get; set;}
        public string SpecificTorque {get; set;}
        public string EngineConstruction {get; set;}
        public string Sump {get; set;}
        public string CompressionRatio {get; set;}
        public string FuelSystem {get; set;}
        public string BMEP {get; set;}
        public string EngineCoolant {get; set;}
        public string UnitaryCapacity {get; set;}
        public string Aspiration {get; set;}
        public string CatalyticConverter {get; set;}
        public int YearOfManufactureID {get; set;}
        public string CountryOfOrigin {get; set;}
        public string Transmission {get; set;}
        public string YearOfManufacture { get; set; }
        public string Remarks {get; set;}
        public string YearOfModel { get; set; }
        public int YearOfModelID { get; set; }
        public string ErrorMessage { get; set; }
        #endregion

        #region Derived Property
        public List<VehicleEngine> VehicleEngines { get; set; }
        public string FuelTypeInString
        {
            get
            {
                return Enum.GetName(typeof(EnumFuelType), this.FuelType).ToString();
            }
        }
        public string Param { get; set; }

        #endregion

        #region Functions

        public static List<VehicleEngine> Gets(long nUserID)
        {
            return VehicleEngine.Service.Gets(nUserID);
        }
        public static List<VehicleEngine> Gets(string sSQL, Int64 nUserID)
        {
            return VehicleEngine.Service.Gets(sSQL, nUserID);
        }

        public static VehicleEngine Get(int nId, long nUserID)
        {
            return VehicleEngine.Service.Get(nId, nUserID);
        }
        public static List<VehicleEngine> GetsByEngineNo(string sEngineNo, Int64 nUserID)
        {
            return VehicleEngine.Service.GetsByEngineNo(sEngineNo, nUserID);
        }
        public VehicleEngine Save(long nUserID)
        {
            return VehicleEngine.Service.Save(this, nUserID);
        }

        public string Delete(int nId, long nUserID)
        {
            return VehicleEngine.Service.Delete(nId, nUserID);
        }
       

        #endregion

        #region ServiceFactory
        internal static IVehicleEngineService Service
        {
            get { return (IVehicleEngineService)Services.Factory.CreateService(typeof(IVehicleEngineService)); }
        }
        #endregion
    }
    #endregion

    #region IVehicleEngine interface

    public interface IVehicleEngineService
    {

        VehicleEngine Get(int id, long nUserID);

        List<VehicleEngine> Gets(long nUserID);
        List<VehicleEngine> Gets(string sSQL, Int64 nUserID);
        List<VehicleEngine> GetsByEngineNo(string sEngineNo, long nUserID);
        
        string Delete(int id, long nUserID);
        VehicleEngine Save(VehicleEngine oVehicleEngine, long nUserID);
    }
    #endregion
}