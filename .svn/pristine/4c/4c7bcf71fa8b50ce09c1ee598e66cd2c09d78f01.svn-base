using System;
using System.Data;
using System.Data.SqlClient;
using ICS.Core.DataAccess;
using ESimSol.BusinessObjects;
using System.Data.OleDb;
using ICS.Core.Utility;

namespace ESimSol.Services.DataAccess
{
    public class VehicleChassisDA
    {
        public VehicleChassisDA() { }

        #region Insert Update Delete Function
        public static IDataReader InsertUpdate(TransactionContext tc, VehicleChassis oVehicleChassis, EnumDBOperation eEnumDBOperation, Int64 nUserID)
        {
            return tc.ExecuteReader("EXEC [SP_IUD_VehicleChassis]"
                                    + "%n , %s,%s,%n,%s,%s, %s,%s,%s,%s,%s, %s,%s,%s,%s,%s, %s,%s,%s,%s, %n, %n",
                                    oVehicleChassis.VehicleChassisID, oVehicleChassis.FileNo, oVehicleChassis.ChassisNo,
                                    oVehicleChassis.ManufacturerID, oVehicleChassis.EnginePosition, oVehicleChassis.EngineLayout, oVehicleChassis.DriveWheels,
                                    oVehicleChassis.TorqueSplit, oVehicleChassis.Steering, oVehicleChassis.WheelSizeFront, oVehicleChassis.WheelSizeRear,
                                    oVehicleChassis.TyresFront, oVehicleChassis.TyresRear, oVehicleChassis.BrakesFR, oVehicleChassis.FrontBrakeDiameter,
                                    oVehicleChassis.RearBrakeDiameter, oVehicleChassis.Gearbox, oVehicleChassis.TopGearRatio, oVehicleChassis.FinalDriveRatio,
                                    oVehicleChassis.Remarks, (int)eEnumDBOperation, nUserID);
        }

        public static void Delete(TransactionContext tc, VehicleChassis oVehicleChassis, EnumDBOperation eEnumDBOperation, Int64 nUserID)
        {
            tc.ExecuteNonQuery("EXEC [SP_IUD_VehicleChassis]"
                                    + "%n , %s,%s,%n,%s,%s, %s,%s,%s,%s,%s, %s,%s,%s,%s,%s, %s,%s,%s,%s, %n, %n",
                                    oVehicleChassis.VehicleChassisID, oVehicleChassis.FileNo, oVehicleChassis.ChassisNo,
                                    oVehicleChassis.ManufacturerID, oVehicleChassis.EnginePosition, oVehicleChassis.EngineLayout, oVehicleChassis.DriveWheels,
                                    oVehicleChassis.TorqueSplit, oVehicleChassis.Steering, oVehicleChassis.WheelSizeFront, oVehicleChassis.WheelSizeRear,
                                    oVehicleChassis.TyresFront, oVehicleChassis.TyresRear, oVehicleChassis.BrakesFR, oVehicleChassis.FrontBrakeDiameter,
                                    oVehicleChassis.RearBrakeDiameter, oVehicleChassis.Gearbox, oVehicleChassis.TopGearRatio, oVehicleChassis.FinalDriveRatio,
                                    oVehicleChassis.Remarks, (int)eEnumDBOperation, nUserID);
        }
        #endregion

        #region Get & Exist Function
        public static IDataReader Get(TransactionContext tc, long nID)
        {
            return tc.ExecuteReader("SELECT * FROM View_VehicleChassis WHERE VehicleChassisID=%n", nID);
        }

        public static IDataReader Gets(TransactionContext tc)
        {
            return tc.ExecuteReader("SELECT * FROM View_VehicleChassis Order By [VehicleChassisID]");
        }
        public static IDataReader Gets(TransactionContext tc, string sSQL)
        {
            return tc.ExecuteReader(sSQL);//use View_VehicleChassis
        }
        public static IDataReader GetsByChassisNo(TransactionContext tc, string sChassisNo)
        {
            return tc.ExecuteReader("SELECT * FROM View_VehicleChassis WHERE ChassisNo like '%" + sChassisNo + "%' Order By [ChassisNo]");//use View_VehicleChassis
        }
        #endregion
    }
}
