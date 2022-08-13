using System;
using System.Data;
using System.Data.SqlClient;
using ICS.Core.DataAccess;
using ESimSol.BusinessObjects;
using System.Data.OleDb;
using ICS.Core.Utility;

namespace ESimSol.Services.DataAccess
{
    public class VehicleRegistrationDA
    {
        public VehicleRegistrationDA() { }

        #region Insert Update Delete Function
        public static IDataReader InsertUpdate(TransactionContext tc, VehicleRegistration oVehicleRegistration, EnumDBOperation eEnumDBOperation, Int64 nUserID)
        {
            return tc.ExecuteReader("EXEC [SP_IUD_VehicleRegistration]"
                                    + "%n, %s ,%d ,%n,%n,%n ,%n,%n, %s, %n,%s,%s,   %n ,%n,%n ",
                                    oVehicleRegistration.VehicleRegistrationID, oVehicleRegistration.VehicleRegNo, oVehicleRegistration.VehicleRegDateSt,
                                    oVehicleRegistration.VehicleTypeID, oVehicleRegistration.CustomerID, oVehicleRegistration.ContactPersonID, oVehicleRegistration.VehicleChassisID,
                                    oVehicleRegistration.VehicleEngineID, oVehicleRegistration.VehicleModelNo, oVehicleRegistration.VehicleColorID, oVehicleRegistration.DeliveryDate,  oVehicleRegistration.Remarks,
                                    oVehicleRegistration.VehicleRegistrationTypeInt, nUserID, (int)eEnumDBOperation);
        }

        public static void Delete(TransactionContext tc, VehicleRegistration oVehicleRegistration, EnumDBOperation eEnumDBOperation, Int64 nUserID)
        {
            tc.ExecuteNonQuery("EXEC [SP_IUD_VehicleRegistration]"
                                  + "%n, %s ,%d ,%n,%n,%n ,%n,%n, %s, %n,%s,%s,  %n ,%n,%n ",
                                    oVehicleRegistration.VehicleRegistrationID, oVehicleRegistration.VehicleRegNo, oVehicleRegistration.VehicleRegDateSt,
                                    oVehicleRegistration.VehicleTypeID, oVehicleRegistration.CustomerID, oVehicleRegistration.ContactPersonID, oVehicleRegistration.VehicleChassisID,
                                    oVehicleRegistration.VehicleEngineID, oVehicleRegistration.VehicleModelNo, oVehicleRegistration.VehicleColorID, oVehicleRegistration.DeliveryDate, oVehicleRegistration.Remarks,
                                    oVehicleRegistration.VehicleRegistrationTypeInt, nUserID, (int)eEnumDBOperation);
        }
        #endregion

        #region Get & Exist Function
        public static IDataReader Get(TransactionContext tc, long nID)
        {
            return tc.ExecuteReader("SELECT * FROM View_VehicleRegistration WHERE VehicleRegistrationID=%n", nID);
        }

        public static IDataReader Gets(TransactionContext tc)
        {
            return tc.ExecuteReader("SELECT * FROM View_VehicleRegistration Order By [VehicleRegistrationID]");
        }
        public static IDataReader Gets(TransactionContext tc, string sSQL)
        {
            return tc.ExecuteReader(sSQL);//use View_VehicleRegistration
        }
        public static IDataReader GetsByChassisNo(TransactionContext tc, string sChassisNo)
        {
            return tc.ExecuteReader("SELECT * FROM View_VehicleRegistration WHERE ChassisNo like '%" + sChassisNo + "%' Order By [ChassisNo]");//use View_VehicleRegistration
        }
        #endregion
    }
}
