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
    public class VehicleEngineService : MarshalByRefObject, IVehicleEngineService
    {
        #region Private functions and declaration
        private VehicleEngine MapObject(NullHandler oReader)
        {
            VehicleEngine oVehicleEngine = new VehicleEngine();
            oVehicleEngine.VehicleEngineID = oReader.GetInt32("VehicleEngineID");
            oVehicleEngine.FileNo = oReader.GetString("FileNo");
            oVehicleEngine.EngineNo = oReader.GetString("EngineNo");
            oVehicleEngine.EngineType = oReader.GetString("EngineType");
            oVehicleEngine.FuelType = oReader.GetInt16("FuelType");
            oVehicleEngine.ManufacturerID = oReader.GetInt32("ManufacturerID");
            oVehicleEngine.ManufacturerName = oReader.GetString("ManufacturerName");
            oVehicleEngine.Cylinders=oReader.GetString("Cylinders");
            oVehicleEngine.Capacity=oReader.GetString("Capacity");
            oVehicleEngine.BoreStroke = oReader.GetString("BoreStroke");
            oVehicleEngine.BoreStrokeRation = oReader.GetString("BoreStrokeRation");
            oVehicleEngine.MaxPowerOutput = oReader.GetString("MaxPowerOutput");
            oVehicleEngine.SpecificOutput = oReader.GetString("SpecificOutput");
            oVehicleEngine.EngineConstruction = oReader.GetString("EngineConstruction");
            oVehicleEngine.MaximumTorque = oReader.GetString("MaximumTorque");
            oVehicleEngine.SpecificTorque = oReader.GetString("SpecificTorque");
            oVehicleEngine.Sump = oReader.GetString("Sump");
            oVehicleEngine.CompressionRatio = oReader.GetString("CompressionRatio");
            oVehicleEngine.FuelSystem = oReader.GetString("FuelSystem");
            oVehicleEngine.BMEP = oReader.GetString("BMEP");
            oVehicleEngine.EngineCoolant = oReader.GetString("EngineCoolant");
            oVehicleEngine.UnitaryCapacity = oReader.GetString("UnitaryCapacity");
            oVehicleEngine.Aspiration = oReader.GetString("Aspiration");
            oVehicleEngine.CatalyticConverter = oReader.GetString("CatalyticConverter");
            oVehicleEngine.YearOfManufacture = oReader.GetString("YearOfManufacture");
            oVehicleEngine.YearOfManufactureID = oReader.GetInt32("YearOfManufactureID");
            oVehicleEngine.CountryOfOrigin = oReader.GetString("CountryOfOrigin");
            oVehicleEngine.Transmission = oReader.GetString("Transmission");
            oVehicleEngine.Remarks = oReader.GetString("Remarks");
            oVehicleEngine.YearOfModel = oReader.GetString("YearOfModel");
            oVehicleEngine.YearOfModelID = oReader.GetInt32("YearOfModelID");            
            return oVehicleEngine;
        }

        private VehicleEngine CreateObject(NullHandler oReader)
        {
            VehicleEngine oVehicleEngine = new VehicleEngine();
            oVehicleEngine = MapObject(oReader);
            return oVehicleEngine;
        }

        private List<VehicleEngine> CreateObjects(IDataReader oReader)
        {
            List<VehicleEngine> oVehicleEngine = new List<VehicleEngine>();
            NullHandler oHandler = new NullHandler(oReader);
            while (oReader.Read())
            {
                VehicleEngine oItem = CreateObject(oHandler);
                oVehicleEngine.Add(oItem);
            }
            return oVehicleEngine;
        }

        #endregion

        #region Interface implementation
        public VehicleEngineService() { }

        public VehicleEngine Save(VehicleEngine oVehicleEngine, Int64 nUserID)
        {
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
                IDataReader reader;
                if (oVehicleEngine.VehicleEngineID <= 0)
                {
                    reader = VehicleEngineDA.InsertUpdate(tc, oVehicleEngine, EnumDBOperation.Insert, nUserID);
                }
                else
                {
                    reader = VehicleEngineDA.InsertUpdate(tc, oVehicleEngine, EnumDBOperation.Update, nUserID);
                }
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oVehicleEngine = new VehicleEngine();
                    oVehicleEngine = CreateObject(oReader);
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
                throw new ServiceException("Failed to Save VehicleEngine. Because of " + e.Message, e);
                #endregion
            }
            return oVehicleEngine;
        }
        public string Delete(int id, Int64 nUserId)
        {
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
                VehicleEngine oVehicleEngine = new VehicleEngine();
                oVehicleEngine.VehicleEngineID = id;
                VehicleEngineDA.Delete(tc, oVehicleEngine, EnumDBOperation.Delete, nUserId);
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

        public VehicleEngine Get(int id, Int64 nUserId)
        {
            VehicleEngine oVehicleEngine = new VehicleEngine();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();
                IDataReader reader = VehicleEngineDA.Get(tc, id);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oVehicleEngine = CreateObject(oReader);
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
                throw new ServiceException("Failed to Get VehicleEngine", e);
                #endregion
            }
            return oVehicleEngine;
        }

        public List<VehicleEngine> Gets(Int64 nUserID)
        {
            List<VehicleEngine> oVehicleEngines = new List<VehicleEngine>();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = VehicleEngineDA.Gets(tc);
                oVehicleEngines = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get VehicleEngine", e);
                #endregion
            }
            return oVehicleEngines;
        }
        public List<VehicleEngine> Gets(string sSQL, Int64 nUserID)
        {
            List<VehicleEngine> oVehicleEngines = null;
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = VehicleEngineDA.Gets(tc, sSQL);
                oVehicleEngines = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get VehicleEngine", e);
                #endregion
            }
            return oVehicleEngines;
        }

        public List<VehicleEngine> GetsByEngineNo(string sEngineNo, Int64 nUserID)
        {
            List<VehicleEngine> oVehicleEngines = null;
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = VehicleEngineDA.GetsByEngineNo(sEngineNo, tc);
                oVehicleEngines = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get VehicleEngine", e);
                #endregion
            }
            return oVehicleEngines;
        }

      
        #endregion
    }
}
