using System;
using System.Data;
using System.Data.SqlClient;
using ICS.Core.DataAccess;
using ESimSol.BusinessObjects;
using System.Data.OleDb;
using ICS.Core.Utility;

namespace ESimSol.Services.DataAccess
{
    public class LabDipDetailDA
    {
        public LabDipDetailDA() { }

        #region Insert Update Delete Function
        public static IDataReader IUD(TransactionContext tc, LabDipDetail oLabDipDetail, int nBDOperation , int nUserID)
        {
            return tc.ExecuteReader("EXEC [SP_IUD_LabDipDetail] %n, %n, %n, %n, %n, %n, %s, %s, %s, %s, %s, %n, %s, %n, %D, %D, %n,%n,%n,%n, %n",
                                    oLabDipDetail.LabDipDetailID, oLabDipDetail.LabDipID, oLabDipDetail.ProductID, oLabDipDetail.ColorSet, oLabDipDetail.ShadeCount,
                                    oLabDipDetail.KnitPlyYarn, oLabDipDetail.ColorName, oLabDipDetail.RefNo, oLabDipDetail.PantonNo, oLabDipDetail.RGB, oLabDipDetail.ColorNo,
                                    oLabDipDetail.Combo, oLabDipDetail.LotNo, oLabDipDetail.ColorCreateBy, oLabDipDetail.ColorCreateDate, oLabDipDetail.ColorCreateDate, oLabDipDetail.Gauge, oLabDipDetail.LabdipColorID, oLabDipDetail.WarpWeftTypeInt, nUserID, nBDOperation);

        }
        public static void UpdateLot(TransactionContext tc, LabDipDetail oLabDipDetail, Int64 nUserID)
        {
            tc.ExecuteNonQuery("UPDATE LabDipDetail SET  LotNo=%s WHERE LabDipDetailID=%n", oLabDipDetail.LotNo, oLabDipDetail.LabDipDetailID);
        }
        public static IDataReader IssueColor(TransactionContext tc, int nLabDipDetailID, int nUserID)
        {
            return tc.ExecuteReader("EXEC [SP_IUD_LabDip_IssueColor] %n, %n", nLabDipDetailID, nUserID);
        }
        
        #endregion


        #region
        public static IDataReader MakeTwistedGroup(TransactionContext tc, string sLabDipDetailID, int nLabDipID, int nTwistedGroup, int nParentID, int nDBOperation, int nUserID)
        {
            return tc.ExecuteReader("EXEC [SP_IUD_LabDipDetail_Twisted] %s, %n, %n, %n, %n, %n", sLabDipDetailID, nLabDipID, nTwistedGroup, nParentID, nUserID, nDBOperation);
        }
        public static IDataReader Save_ColorNo(TransactionContext tc, LabDipDetail oLabDipDetail, Int64 nUserID)
        {
            return tc.ExecuteReader("EXEC [SP_IUD_LabDip_LDNo] %n, %n, %n,%s, %n",
                                    oLabDipDetail.LabDipID, oLabDipDetail.LabDipDetailID, oLabDipDetail.LabdipColorID,oLabDipDetail.ColorNo, nUserID);

        }
        public static IDataReader Save_PantonNo(TransactionContext tc, LabDipDetail oLabDipDetail, Int64 nUserID)
        {
            string sSQL1 = SQLParser.MakeSQL("UPDATE LabdipDetail SET PantonNo =%s, PantonPageNo=%s WHERE LabDipDetailID=%n", oLabDipDetail.PantonNo, oLabDipDetail.PantonPageNo, oLabDipDetail.LabDipDetailID);
            tc.ExecuteNonQuery(sSQL1);
            return tc.ExecuteReader("SELECT * FROM View_LabdipDetail WHERE LabDipDetailID=%n", oLabDipDetail.LabDipDetailID);
        }
        public static IDataReader LabDip_Receive_Submit(TransactionContext tc, LabDipDetail oLabDipDetail, int nBDOperation, int nUserID)
        {
            return tc.ExecuteReader("EXEC [SP_IUD_LabDip_Receive_Submit] %n, %n, %n, %n",
                                    oLabDipDetail.LabDipID, oLabDipDetail.LabDipDetailID, nBDOperation, nUserID);

        }
        #endregion

        #region Get Functions
        public static IDataReader Get(TransactionContext tc, int nID)
        {
            return tc.ExecuteReader("SELECT * FROM  View_LabdipDetail Where LabDipDetailID=%n", nID);
        }
        public static IDataReader Gets(TransactionContext tc, int nLabDipID)
        {
            return tc.ExecuteReader("SELECT * FROM View_LabdipDetail Where LabDipID=%n", nLabDipID);
        }
        public static IDataReader Gets(TransactionContext tc, string sSQL)
        {
            return tc.ExecuteReader(sSQL);
        }

        public static int LabDipDetailCount(TransactionContext tc, int nLabDipID)
        {

            object obj = tc.ExecuteScalar("Select COUNT(*) TotalItem from LabdipDetail Where LabDipID=" + nLabDipID);
            return (int)obj;
        }
        #endregion
    }
}