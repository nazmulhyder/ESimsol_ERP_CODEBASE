using System;
using System.Data;
using System.Data.SqlClient;
using ESimSol.BusinessObjects;
using System.Data.OleDb;
using ICS.Core.DataAccess;
using ICS.Core.Utility;


namespace ESimSol.Services.DataAccess
{
    public class MeasurementUnitDA
    {
        public MeasurementUnitDA() { }

        #region Insert Update Delete Function
        public static IDataReader InsertUpdate(TransactionContext tc, MeasurementUnit oMeasurementUnit, EnumDBOperation eEnumDBOperation, Int64 nUserId)
        {
            return tc.ExecuteReader("EXEC [SP_IUD_MeasurementUnit]"
                                    + "%n, %s, %n, %s, %s, %b, %b, %n, %n",
                                    oMeasurementUnit.MeasurementUnitID, oMeasurementUnit.UnitName, (int)oMeasurementUnit.UnitType, oMeasurementUnit.Symbol, oMeasurementUnit.Note, oMeasurementUnit.IsRound, oMeasurementUnit.IsSmallUnit, nUserId, (int)eEnumDBOperation);
        }

        public static void Delete(TransactionContext tc, MeasurementUnit oMeasurementUnit, EnumDBOperation eEnumDBOperation, Int64 nUserId)
        {
            tc.ExecuteNonQuery("EXEC [SP_IUD_MeasurementUnit]"
                                    + "%n, %s, %n, %s, %s, %b, %b, %n, %n",
                                    oMeasurementUnit.MeasurementUnitID, oMeasurementUnit.UnitName, (int)oMeasurementUnit.UnitType, oMeasurementUnit.Symbol, oMeasurementUnit.Note, oMeasurementUnit.IsRound, oMeasurementUnit.IsSmallUnit, nUserId, (int)eEnumDBOperation);
        }
        #endregion

        #region Get & Exist Function
        public static IDataReader Get(TransactionContext tc, long nID)
        {
            return tc.ExecuteReader("SELECT * FROM MeasurementUnit WHERE MeasurementUnitID=%n", nID);
        }

        public static IDataReader Gets(TransactionContext tc)
        {
            return tc.ExecuteReader("SELECT * FROM MeasurementUnit ORDER BY UnitType ASC");            
        }

        public static IDataReader Gets(TransactionContext tc, int nUnitType)
        {
            return tc.ExecuteReader("SELECT * FROM MeasurementUnit WHERE UnitType=%n", nUnitType);            
        }
        public static IDataReader GetsbyProductID(TransactionContext tc, int productId)
        {
            return tc.ExecuteReader("SELECT * FROM MeasurementUnit WHERE UnitType IN (SELECT UnitType FROM View_Product WHERE ProductID=%n)", productId);            
        }
        public static IDataReader Gets(TransactionContext tc, string sSQL)
        {
            return tc.ExecuteReader(sSQL);
        }      
        #endregion
    }
}