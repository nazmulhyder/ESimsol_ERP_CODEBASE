using System;
using System.Data;
using System.Data.SqlClient;
using ICS.Core.DataAccess;
using ESimSol.BusinessObjects;
using System.Data.OleDb;
using ICS.Core.Utility;

namespace ESimSol.Services.DataAccess
{
    public class VehicleTypeDA
    {
        public VehicleTypeDA() { }

        #region Insert Update Delete Function
        public static IDataReader InsertUpdate(TransactionContext tc, VehicleType oVehicleType, EnumDBOperation eEnumDBOperation, Int64 nUserID)
        {
            return tc.ExecuteReader("EXEC [SP_IUD_VehicleType]"
                                    + "%n , %s, %s, %s, %n, %n",
                                    oVehicleType.VehicleTypeID, oVehicleType.VehicleTypeCode, oVehicleType.VehicleTypeName, oVehicleType.Remarks,  (int)eEnumDBOperation, nUserID);
        }

        public static void Delete(TransactionContext tc, VehicleType oVehicleType, EnumDBOperation eEnumDBOperation, Int64 nUserID)
        {
            tc.ExecuteNonQuery("EXEC [SP_IUD_VehicleType]"
                                    + "%n , %s, %s, %s, %n, %n",
                                    oVehicleType.VehicleTypeID, oVehicleType.VehicleTypeCode, oVehicleType.VehicleTypeName, oVehicleType.Remarks, (int)eEnumDBOperation, nUserID);
        }
        #endregion

        #region Get & Exist Function
        public static IDataReader Get(TransactionContext tc, long nID)
        {
            return tc.ExecuteReader("SELECT * FROM VehicleType WHERE VehicleTypeID=%n", nID);
        }
        public static IDataReader Gets(TransactionContext tc)
        {
            return tc.ExecuteReader("SELECT * FROM VehicleType Order By [VehicleTypeName]");
        }
        public static IDataReader Gets(TransactionContext tc, string sSQL)
        {
            return tc.ExecuteReader(sSQL);//use View_VehicleType
        }
        #endregion
    }
}
