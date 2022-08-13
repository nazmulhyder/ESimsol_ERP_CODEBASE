using System;
using System.Data;
using System.Data.SqlClient;
using ICS.Core.DataAccess;
using ESimSol.BusinessObjects;
using System.Data.OleDb;
using ICS.Core.Utility;

namespace ESimSol.Services.DataAccess
{
    public class VehicleColorDA
    {
        public VehicleColorDA() { }

        #region Insert Update Delete Function
        public static IDataReader InsertUpdate(TransactionContext tc, VehicleColor oVehicleColor, EnumDBOperation eEnumDBOperation, Int64 nUserID)
        {
            return tc.ExecuteReader("EXEC [SP_IUD_VehicleColor]"
                                    + "%n , %s, %s, %n, %s, %n, %n",
                                    oVehicleColor.VehicleColorID, oVehicleColor.ColorCode, oVehicleColor.ColorName, oVehicleColor.ColorType, oVehicleColor.Remarks,  (int)eEnumDBOperation, nUserID);
        }

        public static void Delete(TransactionContext tc, VehicleColor oVehicleColor, EnumDBOperation eEnumDBOperation, Int64 nUserID)
        {
            tc.ExecuteNonQuery("EXEC [SP_IUD_VehicleColor]"
                                    + "%n , %s, %s, %n, %s, %n, %n",
                                    oVehicleColor.VehicleColorID, oVehicleColor.ColorCode, oVehicleColor.ColorName, oVehicleColor.ColorType, oVehicleColor.Remarks, (int)eEnumDBOperation, nUserID);
        }
        #endregion

        #region Get & Exist Function
        public static IDataReader Get(TransactionContext tc, long nID)
        {
            return tc.ExecuteReader("SELECT * FROM VehicleColor WHERE VehicleColorID=%n", nID);
        }

        public static IDataReader Gets(TransactionContext tc)
        {
            return tc.ExecuteReader("SELECT * FROM VehicleColor Order By [ColorName]");
        }
        public static IDataReader Gets(TransactionContext tc, string sSQL)
        {
            return tc.ExecuteReader(sSQL);//use View_VehicleColor
        }

        public static IDataReader GetsByColorCode(TransactionContext tc, string sColorCode)
        {
            string sSql = "SELECT * FROM VehicleColor WHERE ColorCode like '%" + sColorCode + "%' Order By [ColorName]";
            return tc.ExecuteReader( sSql);
        }

        public static IDataReader GetsByColorNameWithType(TransactionContext tc, string sColorName, int nColorType)
        {
            string sSql = "SELECT * FROM VehicleColor WHERE ColorName like '%" + sColorName + "%' AND ColorType = "+ nColorType + " Order By [ColorName]";
            return tc.ExecuteReader(sSql);
        }
        #endregion
    }
}
