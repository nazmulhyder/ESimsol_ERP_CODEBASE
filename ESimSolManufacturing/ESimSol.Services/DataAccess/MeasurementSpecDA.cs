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
    public class MeasurementSpecDA
    {
        public MeasurementSpecDA() { }

        #region Insert Update Delete Function
        public static IDataReader InsertUpdate(TransactionContext tc, MeasurementSpec oMeasurementSpec, EnumDBOperation eEnumDBOperation, Int64 nUserID)
        {
            return tc.ExecuteReader("EXEC [SP_IUD_MeasurementSpec]"
                                    + "%n, %n, %n, %n, %n, %n, %s, %s, %n, %n",
                                    oMeasurementSpec.MeasurementSpecID, oMeasurementSpec.TechnicalSheetID, oMeasurementSpec.SampleSizeID, oMeasurementSpec.SizeClassID, oMeasurementSpec.GarmentsTypeID, oMeasurementSpec.MeasurementUnitID, oMeasurementSpec.ShownAs, oMeasurementSpec.Note,  nUserID, (int)eEnumDBOperation);
        }

        public static void Delete(TransactionContext tc, MeasurementSpec oMeasurementSpec, EnumDBOperation eEnumDBOperation, Int64 nUserID)
        {
            tc.ExecuteNonQuery("EXEC [SP_IUD_MeasurementSpec]"
                                    + "%n, %n, %n, %n, %n, %n, %s, %s, %n, %n",
                                    oMeasurementSpec.MeasurementSpecID, oMeasurementSpec.TechnicalSheetID, oMeasurementSpec.SampleSizeID, oMeasurementSpec.SizeClassID, oMeasurementSpec.GarmentsTypeID, oMeasurementSpec.MeasurementUnitID, oMeasurementSpec.ShownAs, oMeasurementSpec.Note,  nUserID, (int)eEnumDBOperation);
        }
        #endregion

        #region Get & Exist Function
        public static IDataReader Gets_Report(TransactionContext tc, int id)
        {
            return tc.ExecuteReader("SELECT * FROM View_MeasurementSpec WHERE TechnicalSheetID=%n", id);
        }
        public static IDataReader Get(TransactionContext tc, long nID)
        {
            return tc.ExecuteReader("SELECT * FROM View_MeasurementSpec WHERE MeasurementSpecID=%n", nID);
        }

        public static IDataReader Gets(TransactionContext tc)
        {
            return tc.ExecuteReader("SELECT * FROM View_MeasurementSpec");
        }
        public static IDataReader Gets(TransactionContext tc, string sSQL)
        {
            return tc.ExecuteReader(sSQL);
        }
        #endregion
    }
}
