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
    public class BodyMeasureDA
    {
        public BodyMeasureDA() { }

        #region Insert Update Delete Function
        public static IDataReader InsertUpdate(TransactionContext tc, BodyMeasure oBodyMeasure, EnumDBOperation eEnumDBOperation, Int64 nUserID, string sBodyMeasureIDs)
        {
            return tc.ExecuteReader("EXEC [SP_IUD_BodyMeasure]" + "%n, %n, %n, %n, %n, %s, %n, %n, %s",
                                    oBodyMeasure.BodyMeasureID, oBodyMeasure.CostSheetID, oBodyMeasure.BodyPartID, oBodyMeasure.MeasureInCM, oBodyMeasure.GSM, oBodyMeasure.Remarks, nUserID, (int)eEnumDBOperation, sBodyMeasureIDs);
        }
        public static void Delete(TransactionContext tc, BodyMeasure oBodyMeasure, EnumDBOperation eEnumDBOperation, Int64 nUserID, string sBodyMeasureIDs)
        {
            tc.ExecuteNonQuery("EXEC [SP_IUD_BodyMeasure]" + "%n, %n, %n, %n, %n, %s, %n, %n, %s",
                                    oBodyMeasure.BodyMeasureID, oBodyMeasure.CostSheetID, oBodyMeasure.BodyPartID, oBodyMeasure.MeasureInCM, oBodyMeasure.GSM, oBodyMeasure.Remarks, nUserID, (int)eEnumDBOperation, sBodyMeasureIDs);
        }
        #endregion

        #region Get & Exist Function
        public static IDataReader Get(TransactionContext tc, long nID)
        {
            return tc.ExecuteReader("SELECT * FROM View_BodyMeasure WHERE BodyMeasureID=%n", nID);
        }

        public static IDataReader Gets(TransactionContext tc)
        {
            return tc.ExecuteReader("SELECT * FROM View_BodyMeasure");
        }
        public static IDataReader Gets(TransactionContext tc, int nCostSheetID)
        {
            return tc.ExecuteReader("SELECT * FROM View_BodyMeasure WHERE CostSheetID=%n ORDER BY BodyMeasureID", nCostSheetID);
        }
        public static IDataReader Gets(TransactionContext tc, string sSQL)
        {
            return tc.ExecuteReader(sSQL);
        }
        #endregion
    }
}
