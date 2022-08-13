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
    public class MeasurementSpecDetailDA
    {
        public MeasurementSpecDetailDA() { }

        #region Insert Update Delete Function
        public static IDataReader InsertUpdate(TransactionContext tc, MeasurementSpecDetail oMeasurementSpecDetail, EnumDBOperation eEnumDBOperation, Int64 nUserID)
        {
            return tc.ExecuteReader("EXEC [SP_IUD_MeasurementSpecDetail]"
                                    + "%n, %n, %s, %s, %n, %n, %n, %n, %n,%n, %n",
                                    oMeasurementSpecDetail.MeasurementSpecDetailID, oMeasurementSpecDetail.MeasurementSpecID, oMeasurementSpecDetail.POM, oMeasurementSpecDetail.DescriptionNote, oMeasurementSpecDetail.Addition, oMeasurementSpecDetail.Deduction, oMeasurementSpecDetail.SizeID, oMeasurementSpecDetail.SizeValue, oMeasurementSpecDetail.Sequence, nUserID, (int)eEnumDBOperation);
        }

        public static void Delete(TransactionContext tc, MeasurementSpecDetail oMeasurementSpecDetail, EnumDBOperation eEnumDBOperation, Int64 nUserID)
        {
            tc.ExecuteNonQuery("EXEC [SP_IUD_MeasurementSpecDetail]"
                                    + "%n, %n, %s, %s, %n, %n, %n, %n, %n,%n, %n",
                                    oMeasurementSpecDetail.MeasurementSpecDetailID, oMeasurementSpecDetail.MeasurementSpecID, oMeasurementSpecDetail.POM, oMeasurementSpecDetail.DescriptionNote, oMeasurementSpecDetail.Addition, oMeasurementSpecDetail.Deduction, oMeasurementSpecDetail.SizeID, oMeasurementSpecDetail.SizeValue, oMeasurementSpecDetail.Sequence, nUserID, (int)eEnumDBOperation);
        }
        #endregion

        #region Get & Exist Function
        public static IDataReader Gets_Report(TransactionContext tc, int id)
        {
            return tc.ExecuteReader("SELECT * FROM View_MeasurementSpecDetail WHERE MeasurementSpecID =(SELECT MeasurementSpecID FROM MeasurementSpec WHERE TechnicalSheetID=%n) ", id);
        }
        public static IDataReader Get(TransactionContext tc, long nID)
        {
            return tc.ExecuteReader("SELECT * FROM View_MeasurementSpecDetail WHERE MeasurementSpecDetailID=%n", nID);
        }

        public static IDataReader Gets(TransactionContext tc)
        {
            return tc.ExecuteReader("SELECT * FROM View_MeasurementSpecDetail");
        }
        public static IDataReader Gets(TransactionContext tc, int nMSID)
        {
            return tc.ExecuteReader("SELECT * FROM View_MeasurementSpecDetail WHERE MeasurementSpecID=%n ORDER BY POM", nMSID);
        }

        public static IDataReader GetsByTechnicalSheet(TransactionContext tc, int nTSID)
        {
            return tc.ExecuteReader("SELECT * FROM View_MeasurementSpecDetail WHERE MeasurementSpecID =(SELECT MeasurementSpecID FROM MeasurementSpec WHERE TechnicalSheetID=%n) ORDER BY Sequence", nTSID);
        }

        public static IDataReader Gets(TransactionContext tc, string sSQL)
        {
            return tc.ExecuteReader(sSQL);
        }
        #endregion
    }
}
