using System;
using System.Data;
using System.Data.SqlClient;
using ICS.Core.DataAccess;
using ESimSol.BusinessObjects;
using System.Data.OleDb;
using ICS.Core.Utility;

namespace ESimSol.Services.DataAccess
{
    public class VehicleEngineDA
    {
        public VehicleEngineDA() { }

        #region Insert Update Delete Function
        public static IDataReader InsertUpdate(TransactionContext tc, VehicleEngine oVehicleEngine, EnumDBOperation eEnumDBOperation, Int64 nUserID)
        {
            return tc.ExecuteReader("EXEC [SP_IUD_VehicleEngine]"
                                    + "%n , %s,%s,%s,%n,%n,%s, %s,%s,%s,%s,%s, %s,%s,%s,%s,%s, %s,%s,%s,%s,%s, %s, %n,%s,%s, %s,%n, %n, %n",
                                    oVehicleEngine.VehicleEngineID, oVehicleEngine.FileNo, oVehicleEngine.EngineNo,
                                    oVehicleEngine.EngineType, oVehicleEngine.FuelType, oVehicleEngine.ManufacturerID, oVehicleEngine.Cylinders, oVehicleEngine.Capacity,
                                    oVehicleEngine.BoreStroke, oVehicleEngine.BoreStrokeRation, oVehicleEngine.MaxPowerOutput, oVehicleEngine.SpecificOutput, oVehicleEngine.MaximumTorque,
                                    oVehicleEngine.SpecificTorque, oVehicleEngine.EngineConstruction, 
                                    oVehicleEngine.Sump, oVehicleEngine.CompressionRatio,
                                    oVehicleEngine.FuelSystem,  oVehicleEngine.BMEP,  oVehicleEngine.EngineCoolant,  oVehicleEngine.UnitaryCapacity,  oVehicleEngine.Aspiration,   oVehicleEngine.CatalyticConverter,  
                                    oVehicleEngine.YearOfManufactureID, oVehicleEngine.CountryOfOrigin, oVehicleEngine.Transmission,
                                    oVehicleEngine.Remarks, oVehicleEngine.YearOfModelID, (int)eEnumDBOperation, nUserID);
        }

        public static void Delete(TransactionContext tc, VehicleEngine oVehicleEngine, EnumDBOperation eEnumDBOperation, Int64 nUserID)
        {
            tc.ExecuteNonQuery("EXEC [SP_IUD_VehicleEngine]"
                                    + "%n , %s,%s,%s,%n,%n,%s, %s,%s,%s,%s,%s, %s,%s,%s,%s,%s, %s,%s,%s,%s,%s, %s, %n,%s,%s, %s,%n, %n, %n",
                                    oVehicleEngine.VehicleEngineID, oVehicleEngine.FileNo, oVehicleEngine.EngineNo,
                                    oVehicleEngine.EngineType, oVehicleEngine.FuelType, oVehicleEngine.ManufacturerID, oVehicleEngine.Cylinders, oVehicleEngine.Capacity,
                                    oVehicleEngine.BoreStroke, oVehicleEngine.BoreStrokeRation, oVehicleEngine.MaxPowerOutput, oVehicleEngine.SpecificOutput, oVehicleEngine.MaximumTorque,
                                    oVehicleEngine.SpecificTorque, oVehicleEngine.EngineConstruction,
                                    oVehicleEngine.Sump, oVehicleEngine.CompressionRatio,
                                    oVehicleEngine.FuelSystem, oVehicleEngine.BMEP, oVehicleEngine.EngineCoolant, oVehicleEngine.UnitaryCapacity, oVehicleEngine.Aspiration, oVehicleEngine.CatalyticConverter,
                                    oVehicleEngine.YearOfManufactureID, oVehicleEngine.CountryOfOrigin, oVehicleEngine.Transmission,
                                    oVehicleEngine.Remarks, oVehicleEngine.YearOfModelID, (int)eEnumDBOperation, nUserID);
        }
        #endregion

        #region Get & Exist Function
        public static IDataReader Get(TransactionContext tc, long nID)
        {
            return tc.ExecuteReader("SELECT * FROM View_VehicleEngine WHERE VehicleEngineID=%n", nID);
        }

        public static IDataReader Gets(TransactionContext tc)
        {
            return tc.ExecuteReader("SELECT * FROM View_VehicleEngine Order By [VehicleEngineID]");
        }
        public static IDataReader GetsByEngineNo(string sEngineNo,TransactionContext tc)
        {
            return tc.ExecuteReader("SELECT * FROM View_VehicleEngine WHERE EngineNo Like '%" + sEngineNo + "%' Order By [EngineNo]");
        }
        public static IDataReader Gets(TransactionContext tc, string sSQL)
        {
            return tc.ExecuteReader(sSQL);//use View_VehicleEngine
        }
        #endregion
    }
}
