using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using ESimSol.BusinessObjects;
using ESimSol.Services.DataAccess;
using ICS.Core.DataAccess;
using ICS.Core.Utility;
using ICS.Core.Framework;

namespace ESimSol.Services.Services
{
    public class VehicleChassisService : MarshalByRefObject, IVehicleChassisService
    {
        #region Private functions and declaration
        private VehicleChassis MapObject(NullHandler oReader)
        {
            VehicleChassis oVehicleChassis = new VehicleChassis();
            oVehicleChassis.VehicleChassisID = oReader.GetInt32("VehicleChassisID");
            oVehicleChassis.FileNo = oReader.GetString("FileNo");
            oVehicleChassis.ChassisNo = oReader.GetString("ChassisNo");
            oVehicleChassis.ManufacturerID = oReader.GetInt32("ManufacturerID");
            oVehicleChassis.ManufacturerName = oReader.GetString("ManufacturerName");
            oVehicleChassis.EnginePosition = oReader.GetString("EnginePosition");
            oVehicleChassis.EngineLayout = oReader.GetString("EngineLayout");
            oVehicleChassis.DriveWheels = oReader.GetString("DriveWheels");
            oVehicleChassis.TorqueSplit = oReader.GetString("TorqueSplit");
            oVehicleChassis.Steering = oReader.GetString("Steering");
            oVehicleChassis.WheelSizeFront = oReader.GetString("WheelSizeFront");
            oVehicleChassis.WheelSizeRear = oReader.GetString("WheelSizeRear");
            oVehicleChassis.TyresFront = oReader.GetString("TyresFront");
            oVehicleChassis.TyresRear = oReader.GetString("TyresRear");
            oVehicleChassis.BrakesFR = oReader.GetString("BrakesFR");
            oVehicleChassis.FrontBrakeDiameter = oReader.GetString("FrontBrakeDiameter");
            oVehicleChassis.RearBrakeDiameter = oReader.GetString("RearBrakeDiameter");
            oVehicleChassis.Gearbox = oReader.GetString("Gearbox");
            oVehicleChassis.TopGearRatio = oReader.GetString("TopGearRatio");
            oVehicleChassis.FinalDriveRatio = oReader.GetString("FinalDriveRatio");
            oVehicleChassis.Remarks = oReader.GetString("Remarks");
            return oVehicleChassis;
        }

        private VehicleChassis CreateObject(NullHandler oReader)
        {
            VehicleChassis oVehicleChassis = new VehicleChassis();
            oVehicleChassis = MapObject(oReader);
            return oVehicleChassis;
        }

        private List<VehicleChassis> CreateObjects(IDataReader oReader)
        {
            List<VehicleChassis> oVehicleChassis = new List<VehicleChassis>();
            NullHandler oHandler = new NullHandler(oReader);
            while (oReader.Read())
            {
                VehicleChassis oItem = CreateObject(oHandler);
                oVehicleChassis.Add(oItem);
            }
            return oVehicleChassis;
        }

        #endregion

        #region Interface implementation
        public VehicleChassisService() { }

        public VehicleChassis Save(VehicleChassis oVehicleChassis, Int64 nUserID)
        {
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
                IDataReader reader;
                if (oVehicleChassis.VehicleChassisID <= 0)
                {
                    reader = VehicleChassisDA.InsertUpdate(tc, oVehicleChassis, EnumDBOperation.Insert, nUserID);
                }
                else
                {
                    reader = VehicleChassisDA.InsertUpdate(tc, oVehicleChassis, EnumDBOperation.Update, nUserID);
                }
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oVehicleChassis = new VehicleChassis();
                    oVehicleChassis = CreateObject(oReader);
                }
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Save VehicleChassis. Because of " + e.Message, e);
                #endregion
            }
            return oVehicleChassis;
        }
        public string Delete(int id, Int64 nUserId)
        {
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
                VehicleChassis oVehicleChassis = new VehicleChassis();
                oVehicleChassis.VehicleChassisID = id;
                VehicleChassisDA.Delete(tc, oVehicleChassis, EnumDBOperation.Delete, nUserId);
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                return e.Message.Split('~')[0];
                #endregion
            }
            return Global.DeleteMessage;
        }

        public VehicleChassis Get(int id, Int64 nUserId)
        {
            VehicleChassis oVehicleChassis = new VehicleChassis();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();
                IDataReader reader = VehicleChassisDA.Get(tc, id);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oVehicleChassis = CreateObject(oReader);
                }
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get VehicleChassis", e);
                #endregion
            }
            return oVehicleChassis;
        }

        public List<VehicleChassis> Gets(Int64 nUserID)
        {
            List<VehicleChassis> oVehicleChassiss = new List<VehicleChassis>();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = VehicleChassisDA.Gets(tc);
                oVehicleChassiss = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get VehicleChassis", e);
                #endregion
            }
            return oVehicleChassiss;
        }
        public List<VehicleChassis> Gets(string sSQL, Int64 nUserID)
        {
            List<VehicleChassis> oVehicleChassiss = null;
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = VehicleChassisDA.Gets(tc, sSQL);
                oVehicleChassiss = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get VehicleChassis", e);
                #endregion
            }
            return oVehicleChassiss;
        }
        public List<VehicleChassis> GetsByChassisNo(string sChassis, Int64 nUserID)
        {
            List<VehicleChassis> oVehicleChassiss = null;
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();
                IDataReader reader = null;
                reader = VehicleChassisDA.GetsByChassisNo(tc, sChassis);
                oVehicleChassiss = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get VehicleChassis", e);
                #endregion
            }
            return oVehicleChassiss;
        }


        #endregion
    }
}
