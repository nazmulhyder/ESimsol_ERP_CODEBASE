using System;
using System.Data;
using System.Data.SqlClient;
using ICS.Core.DataAccess;
using ESimSol.BusinessObjects;
using System.Data.OleDb;
using ICS.Core.Utility;

namespace ESimSol.Services.DataAccess
{
    public class VehiclePartsDA
    {
        public VehiclePartsDA() { }

        #region Insert Update Delete Function
        public static IDataReader InsertUpdate(TransactionContext tc, VehicleParts oVehicleParts, EnumDBOperation eEnumDBOperation, Int64 nUserID)
        {
            return tc.ExecuteReader("EXEC [SP_IUD_VehicleParts]"
                                    + "%n , %s, %s, %n, %s, %n, %n",
                                    oVehicleParts.VehiclePartsID, oVehicleParts.PartsCode, oVehicleParts.PartsName, oVehicleParts.PartsType, oVehicleParts.Remarks, (int)eEnumDBOperation, nUserID);
        }

        public static void Delete(TransactionContext tc, VehicleParts oVehicleParts, EnumDBOperation eEnumDBOperation, Int64 nUserID)
        {
            tc.ExecuteNonQuery("EXEC [SP_IUD_VehicleParts]"
                                    + "%n , %s, %s, %n, %s, %n, %n",
                                    oVehicleParts.VehiclePartsID, oVehicleParts.PartsCode, oVehicleParts.PartsName, oVehicleParts.PartsType, oVehicleParts.Remarks, (int)eEnumDBOperation, nUserID);
        }
        #endregion

        #region Get & Exist Function
        public static IDataReader Get(TransactionContext tc, long nID)
        {
            return tc.ExecuteReader("SELECT * FROM VehicleParts WHERE VehiclePartsID=%n", nID);
        }

        public static IDataReader Gets(TransactionContext tc)
        {
            return tc.ExecuteReader("SELECT * FROM VehicleParts Order By [PartsName]");
        }
        public static IDataReader Gets(TransactionContext tc, string sSQL)
        {
            return tc.ExecuteReader(sSQL);//use View_VehicleParts
        }

        public static IDataReader GetsByPartsCode(TransactionContext tc, string sPartsCode)
        {
            string sSql = "SELECT * FROM VehicleParts WHERE PartsCode like '%" + sPartsCode + "%' Order By [PartsName]";
            return tc.ExecuteReader(sSql);
        }

        public static IDataReader GetsByPartsNameWithType(TransactionContext tc, string sPartsName, int nPartsType)
        {
            string sSql = "SELECT * FROM VehicleParts WHERE (ISNULL(PartsName,'')+ISNULL(PartsCode,'')) like '%" + sPartsName + "%' AND PartsType = " + nPartsType + " Order By [PartsName]";
            return tc.ExecuteReader(sSql);
        }
        #endregion
    }
}
