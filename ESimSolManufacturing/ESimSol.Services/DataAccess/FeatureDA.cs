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
    public class FeatureDA
    {
        public FeatureDA() { }

        #region Insert Update Delete Function
        public static IDataReader InsertUpdate(TransactionContext tc, Feature oFeature, EnumDBOperation eEnumDBOperation, Int64 nUserID)
        {
            return tc.ExecuteReader("EXEC [SP_IUD_Feature]"
                                    + "%n, %s, %s, %n, %n, %n,%n,%n, %s, %n, %n, %n",
                                    oFeature.FeatureID, oFeature.FeatureCode, oFeature.FeatureName, oFeature.FeatureTypeInInt, oFeature.CurrencyID, oFeature.UnitPrice,oFeature.VatAmount, oFeature.Price, oFeature.Remarks, oFeature.VehicleModelID, nUserID, (int)eEnumDBOperation);
        }

        public static void Delete(TransactionContext tc, Feature oFeature, EnumDBOperation eEnumDBOperation, Int64 nUserID)
        {
            tc.ExecuteNonQuery("EXEC [SP_IUD_Feature]"
                                    + "%n, %s, %s, %n, %n, %n,%n,%n, %s, %n, %n, %n",
                                    oFeature.FeatureID, oFeature.FeatureCode, oFeature.FeatureName, oFeature.FeatureTypeInInt, oFeature.CurrencyID, oFeature.UnitPrice, oFeature.VatAmount, oFeature.Price, oFeature.Remarks, oFeature.VehicleModelID, nUserID, (int)eEnumDBOperation);
        }

        #endregion

        #region Get & Exist Function
        public static IDataReader Get(TransactionContext tc, long nID)
        {
            return tc.ExecuteReader("SELECT * FROM View_Feature WHERE FeatureID=%n", nID);
        }

        public static IDataReader Gets(TransactionContext tc)
        {
            return tc.ExecuteReader("SELECT * FROM View_Feature ORDER BY FeatureCode");
        }


        public static IDataReader Gets(TransactionContext tc, string sSQL)
        {
            return tc.ExecuteReader(sSQL);
        }
        public static IDataReader GetsbyFeatureNameWithType(TransactionContext tc, string sFeatureName, string sFTypes)
        {

            string sSQL = "SELECT * FROM View_Feature", sTempSql = "";
            if(!string.IsNullOrWhiteSpace(sFeatureName))
            {
                Global.TagSQL(ref sTempSql);
                sTempSql += " FeatureName LIKE ('%" + sFeatureName + "%')";
            }
            if (!string.IsNullOrWhiteSpace(sFTypes))
            {
                Global.TagSQL(ref sTempSql);
                sTempSql += " FeatureType In (" + sFTypes + ")";
            }
            sSQL = sSQL + sTempSql + "  ORDER BY FeatureType";
            return tc.ExecuteReader(sSQL);
        }
      
        #endregion
    }
}
