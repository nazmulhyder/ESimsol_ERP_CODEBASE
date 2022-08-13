using System;
using System.Data;
using System.Data.SqlClient;
using ICS.Core.DataAccess;
using ESimSol.BusinessObjects;
using System.Data.OleDb;
using ICS.Core.Utility;

namespace ESimSol.Services.DataAccess
{
    public class FNLabDipDetailDA
    {
        public FNLabDipDetailDA() { }

        #region Insert Update Delete Function
        public static IDataReader IUD(TransactionContext tc, FNLabDipDetail oFNLabDipDetail, int nBDOperation , int nUserID)
        {
            return tc.ExecuteReader("EXEC [SP_IUD_FNLabDipDetail] %n, %n, %n, %n, %n,%n, %s, %s, %s, %s, %s,%s,%s,%n, %n, %n",
                                    oFNLabDipDetail.FNLabDipDetailID, oFNLabDipDetail.FabricID,  oFNLabDipDetail.ColorSet, oFNLabDipDetail.ShadeCount,
                                    oFNLabDipDetail.KnitPlyYarn,oFNLabDipDetail.Combo, oFNLabDipDetail.LDNo, oFNLabDipDetail.ColorName, oFNLabDipDetail.RefNo, oFNLabDipDetail.PantonNo, oFNLabDipDetail.RGB, oFNLabDipDetail.LotNo, oFNLabDipDetail.Note,oFNLabDipDetail.ReferenceLDID, nUserID, nBDOperation);

        }
        public static void UpdateLot(TransactionContext tc, FNLabDipDetail oFNLabDipDetail, Int64 nUserID)
        {
            tc.ExecuteNonQuery("UPDATE FNLabDipDetail SET  LotNo=%s WHERE FNLabDipDetailID=%n", oFNLabDipDetail.LotNo, oFNLabDipDetail.FNLabDipDetailID);
        }
        #endregion


        #region
        public static IDataReader Save_LDNo(TransactionContext tc, FNLabDipDetail oFNLabDipDetail, int nUserID)
        {
            return tc.ExecuteReader("EXEC [SP_IUD_FNLabDip_LDNo] %n, %n,%s,%n,%n",
                                    oFNLabDipDetail.FabricID, oFNLabDipDetail.FNLabDipDetailID, oFNLabDipDetail.LDNo,oFNLabDipDetail.ReceiveBY, nUserID);
        }
        public static void Save_LDNo_FromFabric(TransactionContext tc, Fabric oFabric, Int64 nUserID)
        {
            tc.ExecuteNonQuery("EXEC [SP_IUD_FNLabDip_LDNo_FromFabric] %n, %s,%s,%d, %n", oFabric.FabricID, oFabric.HandLoomNo, oFabric.Code, NullHandler.GetNullValue(oFabric.ReceiveDate), nUserID);
        }
        #endregion

        #region Get Functions
        public static IDataReader Get(TransactionContext tc, int nID)
        {
            return tc.ExecuteReader("SELECT * FROM  View_FNLabDipDetail Where FNLabDipDetailID=%n", nID);
        }
        public static IDataReader Gets(TransactionContext tc, int nLabDipID)
        {
            return tc.ExecuteReader("SELECT * FROM View_FNLabDipDetail Where LabDipID=%n", nLabDipID);
        }
        public static IDataReader GetsBy(TransactionContext tc, int nFabricID)
        {
            return tc.ExecuteReader("SELECT * FROM View_FNLabDipDetail Where FabricID=%n", nFabricID);
        }
        public static IDataReader Gets(TransactionContext tc, string sSQL)
        {
            return tc.ExecuteReader(sSQL);
        }

        public static int FNLabDipRecipeCount(TransactionContext tc, int nFNLabDipDetailID)
        {
            object obj = tc.ExecuteScalar("Select RecipeCount from (SELECT isnull((SELECT Count(*) from FNRecipeLab Where FNLDDID =TT.FNLabdipDetailID ),0) as RecipeCount from FNLabdipDetail as TT Where FNLabdipDetailID=%n) as DD where isnull(RecipeCount,0)>0 ", nFNLabDipDetailID);
            if (obj == null) return 0;
            return Convert.ToInt32(obj);
        }
        public static IDataReader Submited(TransactionContext tc, FNLabDipDetail oFNLabDipDetail, int nUserID)
        {
            oFNLabDipDetail.LDStatus = EnumLDStatus.Submit;
            string sSQL1 = SQLParser.MakeSQL("Update FNLabdipDetail Set SubmitBy=%n ,SubmitByDate =GetDate(),LDStatus=%n WHERE FNLabDipDetailID=%n", nUserID, (int)oFNLabDipDetail.LDStatus, oFNLabDipDetail.FNLabDipDetailID);
            tc.ExecuteNonQuery(sSQL1);
            return tc.ExecuteReader("SELECT * FROM View_FNLabDipDetail WHERE FNLabDipDetailID=%n", oFNLabDipDetail.FNLabDipDetailID);
        }
        public static IDataReader Issued(TransactionContext tc, FNLabDipDetail oFNLabDipDetail, int nUserID)
        {
            string sSQL1 = SQLParser.MakeSQL("Update FNLabdipDetail Set LDStatus=%n  WHERE FNLabDipDetailID=%n", (int)oFNLabDipDetail.LDStatus, oFNLabDipDetail.FNLabDipDetailID);
            tc.ExecuteNonQuery(sSQL1);
            return tc.ExecuteReader("SELECT * FROM View_FNLabDipDetail WHERE FNLabDipDetailID=%n", oFNLabDipDetail.FNLabDipDetailID);
        }
        public static IDataReader UpdateShade(TransactionContext tc, FNLabDipDetail oFNLabDipDetail, int nUserID)
        {
            oFNLabDipDetail.LDStatus = EnumLDStatus.Approved;
            string sSQL1 = SQLParser.MakeSQL("Update FNLabdipDetail Set ShadeApproveDate=%d, ShadeID_Ap=%n ,LDStatus=%n,ShadeApproveBy=%n , ApprovalNote=%s WHERE FNLabDipDetailID=%n", oFNLabDipDetail.ShadeApproveDate, (int)oFNLabDipDetail.ShadeID_Ap, (int)oFNLabDipDetail.LDStatus, nUserID, oFNLabDipDetail.ApprovalNote, oFNLabDipDetail.FNLabDipDetailID);
            tc.ExecuteNonQuery(sSQL1);
            return tc.ExecuteReader("SELECT * FROM View_FNLabDipDetail WHERE FNLabDipDetailID=%n", oFNLabDipDetail.FNLabDipDetailID);
        }

        public static IDataReader UpdateColorSet(TransactionContext tc, FNLabDipDetail oFNLabDipDetail)
        {
            string sSQL1 = SQLParser.MakeSQL("Update FNLabDipDetail Set ColorSet=%n WHERE FNLabDipDetailID=%n", oFNLabDipDetail.ColorSet, oFNLabDipDetail.FNLabDipDetailID);
            tc.ExecuteNonQuery(sSQL1);
            return tc.ExecuteReader("SELECT * FROM View_FNLabDipDetail WHERE FNLabDipDetailID=%n", oFNLabDipDetail.FNLabDipDetailID);

        }

        #endregion
    }
}