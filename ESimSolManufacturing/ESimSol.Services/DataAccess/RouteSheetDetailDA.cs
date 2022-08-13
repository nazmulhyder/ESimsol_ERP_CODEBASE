using System;
using System.Data;
using System.Data.SqlClient;
using ICS.Core.DataAccess;
using ESimSol.BusinessObjects;
using System.Data.OleDb;
using ICS.Core.Utility;

namespace ESimSol.Services.DataAccess
{
    public class RouteSheetDetailDA
    {
        public RouteSheetDetailDA() { }


        #region Insert Update Delete Function
        public static IDataReader IUD(TransactionContext tc, RouteSheetDetail oRSD, int nDBOperation, long nUserID)
        {
            return tc.ExecuteReader("EXEC [SP_IUD_RouteSheetDetail]"
                                   + "%n, %n, %n, %n, %b, %s, %n, %n, %n, %s, %n, %s, %n, %b, %b, %n, %n, %n, %n, %n, %n,%n,%n, %n, %n",
                                     oRSD.RouteSheetDetailID, oRSD.RouteSheetID, oRSD.ProcessID, oRSD.ParentID, oRSD.IsDyesChemical, oRSD.TempTime, oRSD.GL, oRSD.Percentage, oRSD.DAdjustment, oRSD.Note, oRSD.BatchManID
                                    ,oRSD.Equation, oRSD.Sequence, oRSD.ForCotton, oRSD.SupportMaterial, oRSD.TotalQty, oRSD.AddOneQty, oRSD.AddTwoQty, oRSD.AddThreeQty, oRSD.ReturnQty, oRSD.SuggestLotID,(int)oRSD.RecipeCalType,(int)oRSD.ProductType,  nUserID, nDBOperation);
        }

        #endregion

        #region Add RouteSheet Templet
        public static IDataReader IUDTemplate(TransactionContext tc, int nDSID, int nRSID, long nUserID)
        {
            return tc.ExecuteReader("EXEC [SP_IUD_RouteSheetDetailForTemplate] %n,%n,%n", nDSID, nRSID, nUserID);
        }
        public static IDataReader IUDTemplateCopyFromRS(TransactionContext tc, int nRSID_CopyFrom, int nRSID, long nUserID)
        {
            return tc.ExecuteReader("EXEC [SP_IUD_RouteSheetDetailForRSCopy] %n,%n,%n", nRSID_CopyFrom, nRSID, nUserID);
        }

        #endregion

        #region Dyes Chemical Out
        public static IDataReader DyeChemicalOut(TransactionContext tc, RouteSheetDetail oRSD, int nLotID ,short nAddFor, long nUserID)
        {
            return tc.ExecuteReader("EXEC [SP_IUD_RouteSheet_DyeChemicalOut] %n, %n, %n, %n, %n, %n", oRSD.RouteSheetDetailID, oRSD.RouteSheetID, oRSD.ProcessID, nLotID, nAddFor, nUserID);
        }
        public static IDataReader DyeChemicalOut_V2(TransactionContext tc, RSDetailAdditonal oRSDA,  long nUserID)
        {
            return tc.ExecuteReader("EXEC [SP_IUD_RouteSheet_DyeChemicalOut_V2] %n, %n, %n, %n, %n, %n", oRSDA.RouteSheetDetailID, oRSDA.RouteSheetID, oRSDA.RSDetailAdditonalID, oRSDA.LotID, oRSDA.SequenceNo, nUserID);
        }
        public static IDataReader DyeChemicalOut_Return(TransactionContext tc, RouteSheetDetail oRSD, int nLotID, short nAddFor, long nUserID)
        {
            return tc.ExecuteReader("EXEC [SP_IUD_RouteSheet_DyeChemicalOut_Return] %n, %n, %n, %n, %n, %n", oRSD.RouteSheetDetailID, oRSD.RouteSheetID, oRSD.ProcessID, nLotID, nAddFor, nUserID);
        }

        public static IDataReader Update_RSDetail(TransactionContext tc, RouteSheetDetail oRSD)
        {
            if (oRSD.RSDetailAdditonals.Count == 1 && oRSD.RSDetailAdditonals[0].RSDetailAdditonalID > 0)
            {
                string sSQL1 = SQLParser.MakeSQL("Update RSDetailAdditonal Set LotID=%n WHERE ISNULL(ApprovedByID,0)=0 AND RSDetailAdditonalID=%n", oRSD.SuggestLotID, oRSD.RSDetailAdditonals[0].RSDetailAdditonalID);
                tc.ExecuteNonQuery(sSQL1);
            }
            else if (oRSD.RSDetailAdditonals.Count == 0 && oRSD.TotalQtyLotID == 0)
            {
                string sSQL1 = SQLParser.MakeSQL("Update RouteSheetDetail Set SuggestLotID=%n, SuggestLotNo=%s WHERE ISNULL(TotalQtyLotID,0)=0 AND RouteSheetDetailID=%n", oRSD.SuggestLotID, oRSD.SuggestLotNo, oRSD.RouteSheetDetailID);
                tc.ExecuteNonQuery(sSQL1);
            }
            else throw new Exception("This lot is already used. Failed to update!");

            return tc.ExecuteReader("SELECT * FROM View_RouteSheetDetail WHERE RouteSheetDetailID=%n", oRSD.RouteSheetDetailID);
        }
        #endregion

        #region Get Functions
        public static IDataReader Get(TransactionContext tc, int nID)
        {
            return tc.ExecuteReader("SELECT * FROM View_RouteSheetDetail WHERE RouteSheetDetailID=%n", nID);
        }
        public static IDataReader Gets(TransactionContext tc, string sSQL)
        {
            return tc.ExecuteReader(sSQL);
        }
        public static IDataReader Gets(TransactionContext tc, int nRSID)
        {

            return tc.ExecuteReader("Select * from View_RouteSheetDetail Where RouteSheetID=%n Order By  Sequence,RouteSheetDetailID", nRSID);
        }
        public static Int64 GetDyeChemicalOut(TransactionContext tc, int nRouteSheetID)
        {
            var obj = tc.ExecuteScalar("Select  Count(*)DyeChemicalOut from RouteSheetDetail Where RouteSheetID=" + nRouteSheetID + " And TotalQtyLotID>0");
            return Convert.ToInt64(obj);
        }
        public static void UpdateSequence(TransactionContext tc, RouteSheetDetail oRouteSheetDetail)
        {
            tc.ExecuteNonQuery("Update RouteSheetDetail SET Sequence = %n WHERE RouteSheetDetailID = %n", oRouteSheetDetail.Sequence, oRouteSheetDetail.RouteSheetDetailID);
        }
        #endregion
    }
}
