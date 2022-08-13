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

    public class OrderRecapSummeryDA
    {
        public OrderRecapSummeryDA() { }

        #region image Function
          public static void UpdateCoverPage(TransactionContext tc, TechnicalSheetImage oTechnicalSheetImage)
        {
            SqlParameter picparameter = new SqlParameter();
            picparameter.SqlDbType = SqlDbType.Image;
            picparameter.ParameterName = "pic";
            picparameter.Value = oTechnicalSheetImage.ThumbnailImage;
            string sSQL = SQLParser.MakeSQL("UPDATE OrderRecapSummery SET RecognizeImage=%q " + " WHERE TechnicalSheetID=%n", "@pic", oTechnicalSheetImage.TechnicalSheetID);
            tc.ExecuteNonQueryCommText(CommandType.Text, sSQL, picparameter);
        }
        #endregion

        #region Get & Exist Function
        public static IDataReader GetsRecapWithOrderRecapSummerys(TransactionContext tc,int nOT, int nStartRow, int nEndRow, string SQL, string sOrderRecapIDs, bool bIsPrint, int nSortBy, Int64 nUserID)
        {
            return tc.ExecuteReader("EXEC [SP_OrderRecapSummary]" + "%n, %n, %n, %s, %s, %b, %n, %n", nOT, nStartRow, nEndRow, SQL, sOrderRecapIDs, bIsPrint, nSortBy, nUserID);
        }
        #endregion
    }
  
}
