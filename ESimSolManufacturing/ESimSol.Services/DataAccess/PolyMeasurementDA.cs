using System;
using System.Data;
using System.Data.SqlClient;
using ICS.Core;
using ESimSol.BusinessObjects;
using System.Data.OleDb;
using ICS.Core.Framework;
using ICS.Core.Utility;
using ICS.Core.DataAccess;

namespace ESimSol.Services.DataAccess
{
    public class PolyMeasurementDA
    {
        public PolyMeasurementDA() { }

        #region Insert Update Delete Function
        public static IDataReader InsertUpdate(TransactionContext tc, PolyMeasurement oPolyMeasurement, EnumDBOperation eEnumDBOperation, Int64 nUserID)
        {
            return tc.ExecuteReader("EXEC [SP_IUD_PolyMeasurement]"
                                    + "%n, %s, %s, %n, %n,%s, %n,%s, %n,%s, %n,%s, %n,%s, %n,%s,  %n,%s,   %n, %n",
                                    oPolyMeasurement.PolyMeasurementID, oPolyMeasurement.Measurement, oPolyMeasurement.Note, oPolyMeasurement.PolyMeasurementTypeInt,
                                    oPolyMeasurement.Length,oPolyMeasurement.LengthUnit, oPolyMeasurement.Width,oPolyMeasurement.WidthUnit, oPolyMeasurement.Thickness,
                                    oPolyMeasurement.ThicknessUnit, oPolyMeasurement.Flap,oPolyMeasurement.FlapUnit, oPolyMeasurement.Lip,oPolyMeasurement.LipUnit,
                                    oPolyMeasurement.Gusset, oPolyMeasurement.GussetUnit,oPolyMeasurement.Gusset1,oPolyMeasurement.GussetUnit1, nUserID, (int)eEnumDBOperation);
        }

        public static void Delete(TransactionContext tc, PolyMeasurement oPolyMeasurement, EnumDBOperation eEnumDBOperation, Int64 nUserID)
        {
            tc.ExecuteNonQuery("EXEC [SP_IUD_PolyMeasurement]"
                                    + "%n, %s, %s, %n, %n,%s, %n,%s, %n,%s, %n,%s, %n,%s, %n,%s, %n,%s,   %n, %n",
                                    oPolyMeasurement.PolyMeasurementID, oPolyMeasurement.Measurement, oPolyMeasurement.Note, oPolyMeasurement.PolyMeasurementTypeInt,
                                    oPolyMeasurement.Length, oPolyMeasurement.LengthUnit, oPolyMeasurement.Width, oPolyMeasurement.WidthUnit, oPolyMeasurement.Thickness,
                                    oPolyMeasurement.ThicknessUnit, oPolyMeasurement.Flap, oPolyMeasurement.FlapUnit, oPolyMeasurement.Lip, oPolyMeasurement.LipUnit,
                                    oPolyMeasurement.Gusset, oPolyMeasurement.GussetUnit, oPolyMeasurement.Gusset1, oPolyMeasurement.GussetUnit1, nUserID, (int)eEnumDBOperation);
        }
        #endregion

        #region Get & Exist Function
        public static IDataReader Get(TransactionContext tc, long nID)
        {
            return tc.ExecuteReader("SELECT * FROM PolyMeasurement WHERE PolyMeasurementID=%n", nID);
        }

        public static IDataReader Gets(TransactionContext tc)
        {
            return tc.ExecuteReader("SELECT * FROM PolyMeasurement ORDER BY Measurement");
        }       
        public static IDataReader Gets(TransactionContext tc, string sSQL)
        {
            return tc.ExecuteReader(sSQL);
        }
        public static IDataReader GetsbyMeasurement(TransactionContext tc, string sMeasurement)
        {
            return tc.ExecuteReader("SELECT * FROM PolyMeasurement WHERE Measurement LIKE ('%" + sMeasurement + "%') ");
        }
      
        #endregion
    }
}
