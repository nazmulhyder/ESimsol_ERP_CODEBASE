using System;
using System.Data;
using System.Data.SqlClient;
using ICS.Core.DataAccess;
using ESimSol.BusinessObjects;
using System.Data.OleDb;
using ICS.Core.Utility;
namespace ESimSol.Services.DataAccess
{

    public class MeasurementUnitConDA
    {
        public MeasurementUnitConDA() { }

        #region Insert Update Delete Function
        public static IDataReader InsertUpdate(TransactionContext tc, MeasurementUnitCon oMeasurementUnitCon, EnumDBOperation eEnumDBOperation, Int64 nUserId)
        {
            return tc.ExecuteReader("EXEC[SP_IUD_MeasurementUnitCon]"
                                    + "%n,%n, %n, %n, %n, %n",
                                    oMeasurementUnitCon.MeasurementUnitConID, oMeasurementUnitCon.FromMUnitID,  oMeasurementUnitCon.ToMUnitID, oMeasurementUnitCon.Value, nUserId, (int)eEnumDBOperation);
        }
        public static void Delete(TransactionContext tc, MeasurementUnitCon oMeasurementUnitCon, EnumDBOperation eEnumDBOperation, Int64 nUserId)
        {
            tc.ExecuteNonQuery("EXEC[SP_IUD_MeasurementUnitCon]"
                                    + "%n,%n, %n, %n, %n, %n",
                                    oMeasurementUnitCon.MeasurementUnitConID, oMeasurementUnitCon.FromMUnitID, oMeasurementUnitCon.ToMUnitID, oMeasurementUnitCon.Value, nUserId, (int)eEnumDBOperation);
        }
        #endregion

        #region Get & Exist Function
        public static IDataReader Get(TransactionContext tc, long nID)
        {
            return tc.ExecuteReader("SELECT * FROM View_MeasurementUnitCon WHERE MeasurementUnitConID=%n", nID);
        }

        public static IDataReader GetBy(TransactionContext tc, int nFromMUnitID, int ToMUnitID)
        {
            return tc.ExecuteReader("SELECT * FROM View_MeasurementUnitCon WHERE FromMUnitID=%n and ToMUnitID=%n", nFromMUnitID, ToMUnitID);
        }
        public static IDataReader GetByBU(TransactionContext tc, int nBUID)
        {
            return tc.ExecuteReader("SELECT Top(1)* FROM View_MeasurementUnitCon WHERE MeasurementUnitConID in (select MeasurementUnitConID from MeasurementUnitBU where BUID=%n) order by MeasurementUnitConID DESC", nBUID);
        }

        public static IDataReader Gets(TransactionContext tc)
        {
            return tc.ExecuteReader("SELECT * FROM View_MeasurementUnitCon");
        }
        public static IDataReader Gets(TransactionContext tc,int nBUID )
        {
            return tc.ExecuteReader("SELECT * FROM View_MeasurementUnitCon WHERE BUID=%n", nBUID);
        }
        public static IDataReader Gets(TransactionContext tc, string sSQL)
        {
            return tc.ExecuteReader(sSQL);
        }
        public static IDataReader Activate(TransactionContext tc, MeasurementUnitCon oMeasurementUnitCon)
        {
            string sSQL1 = SQLParser.MakeSQL("Update MeasurementUnitCon Set Activity=~Activity WHERE MeasurementUnitConID=%n", oMeasurementUnitCon.MeasurementUnitConID);
            tc.ExecuteNonQuery(sSQL1);
            return tc.ExecuteReader("SELECT * FROM View_MeasurementUnitCon WHERE MeasurementUnitConID=%n", oMeasurementUnitCon.MeasurementUnitConID);
        }
        public static IDataReader GetByMUnit(TransactionContext tc, int nFromMUnitID)
        {
            return tc.ExecuteReader("SELECT top(1)* FROM View_MeasurementUnitCon WHERE FromMUnitID=%n ", nFromMUnitID);
        }
     
    
        #endregion
    }
}