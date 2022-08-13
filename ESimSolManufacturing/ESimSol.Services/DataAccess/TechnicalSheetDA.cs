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
    public class TechnicalSheetDA
    {
        public TechnicalSheetDA() { }

        #region Insert Update Delete Function
        public static IDataReader InsertUpdate(TransactionContext tc, TechnicalSheet oTechnicalSheet, EnumDBOperation eEnumDBOperation, Int64 nUserID)
        {
            return tc.ExecuteReader("EXEC [SP_IUD_TechnicalSheet]"
                                    + "%n, %s, %n, %n,%n, %n, %n, %n, %n,%s, %s, %s, %s,%s, %s,%s,%n, %n, %s, %s, %n, %s,%n,%s,%s,%s, %s, %n, %n,%n,%n,%n, %n, %n",
                                    oTechnicalSheet.TechnicalSheetID, oTechnicalSheet.StyleNo, oTechnicalSheet.BusinessSessionID, (int)oTechnicalSheet.DevelopmentStatus, oTechnicalSheet.BUID, oTechnicalSheet.BuyerID, oTechnicalSheet.ProductID, (int)oTechnicalSheet.Dept, oTechnicalSheet.BuyerConcernID, oTechnicalSheet.GG, oTechnicalSheet.Count, oTechnicalSheet.SpecialFinish,oTechnicalSheet.Weight, oTechnicalSheet.Line, oTechnicalSheet.Designer, oTechnicalSheet.Story, oTechnicalSheet.GarmentsClassID, oTechnicalSheet.GarmentsSubClassID, oTechnicalSheet.Intake, oTechnicalSheet.Note, (int)oTechnicalSheet.KnittingPattern, oTechnicalSheet.StyleDescription, oTechnicalSheet.TSTypeInInt,  oTechnicalSheet.FabConstruction, oTechnicalSheet.Wash, oTechnicalSheet.FabWidth, oTechnicalSheet.FabCode, oTechnicalSheet.BrandID, (int)oTechnicalSheet.SubGender, oTechnicalSheet.MerchandiserID, oTechnicalSheet.ApproxQty,  oTechnicalSheet.GSMID,  nUserID, (int)eEnumDBOperation);
        }

        public static void Delete(TransactionContext tc, TechnicalSheet oTechnicalSheet, EnumDBOperation eEnumDBOperation, Int64 nUserID)
        {
            tc.ExecuteNonQuery("EXEC [SP_IUD_TechnicalSheet]"
                                    + "%n, %s, %n, %n,%n, %n, %n, %n, %n,%s, %s, %s, %s,%s, %s,%s,%n, %n, %s, %s, %n, %s,%n,%s,%s,%s, %s, %n, %n,%n,%n,%n, %n, %n",
                                    oTechnicalSheet.TechnicalSheetID, oTechnicalSheet.StyleNo, oTechnicalSheet.BusinessSessionID, (int)oTechnicalSheet.DevelopmentStatus, oTechnicalSheet.BUID, oTechnicalSheet.BuyerID, oTechnicalSheet.ProductID, (int)oTechnicalSheet.Dept, oTechnicalSheet.BuyerConcernID, oTechnicalSheet.GG, oTechnicalSheet.Count, oTechnicalSheet.SpecialFinish, oTechnicalSheet.Weight, oTechnicalSheet.Line, oTechnicalSheet.Designer, oTechnicalSheet.Story, oTechnicalSheet.GarmentsClassID, oTechnicalSheet.GarmentsSubClassID, oTechnicalSheet.Intake, oTechnicalSheet.Note, (int)oTechnicalSheet.KnittingPattern, oTechnicalSheet.StyleDescription, oTechnicalSheet.TSTypeInInt, oTechnicalSheet.FabConstruction, oTechnicalSheet.Wash, oTechnicalSheet.FabWidth, oTechnicalSheet.FabCode, oTechnicalSheet.BrandID, (int)oTechnicalSheet.SubGender, oTechnicalSheet.MerchandiserID, oTechnicalSheet.ApproxQty, oTechnicalSheet.GSMID, nUserID, (int)eEnumDBOperation);
        }
        public static IDataReader UpdateStatus(TransactionContext tc, int nTechnicalSheetID, int nDevelopmentStatus, Int64 nUserID)
        {
            return tc.ExecuteReader("EXEC [SP_UpdateTechnicalSheetStatus]"+ "%n, %n, %n", nTechnicalSheetID, nDevelopmentStatus, nUserID);
        }
        #endregion

        #region Get & Exist Function
        public static IDataReader Get(TransactionContext tc, long nID)
        {
            return tc.ExecuteReader("SELECT * FROM View_TechnicalSheet WHERE TechnicalSheetID=%n", nID);
        }

        public static IDataReader GetByStyleNo(TransactionContext tc, string StyleNo)
        {
            return tc.ExecuteReader("SELECT * FROM View_TechnicalSheet WHERE StyleNo=%s", StyleNo);
        }

        public static IDataReader GetsDistinctSession(TransactionContext tc)
        {
            return tc.ExecuteReader("SELECT DISTINCT(YEAR(DBServerDateTime)) AS EntrySession FROM TechnicalSheet");
        }

        public static IDataReader BUWiseGets(int BUID, string DevelopmentStatus, TransactionContext tc)
        {
            if (DevelopmentStatus != null && DevelopmentStatus!="")
            {
                return tc.ExecuteReader("SELECT * FROM View_TechnicalSheet WHERE BUID = %n AND DevelopmentStatus IN ("+DevelopmentStatus+")", BUID);
            }else
            {
                return tc.ExecuteReader("SELECT * FROM View_TechnicalSheet WHERE BUID = %n ",BUID);
            }
            
        }

        public static IDataReader WaitForApproval(TransactionContext tc)
        {
            return tc.ExecuteReader("SELECT * FROM View_TechnicalSheet WHERE DevelopmentStatus=1");//Requestforapproved = 1,
        }

        public static IDataReader Gets(TransactionContext tc, string sSQL)
        {
            return tc.ExecuteReader(sSQL);
        }

        public static IDataReader Gets_Report(TransactionContext tc, int id)
        {
            return tc.ExecuteReader("SELECT * FROM View_TechnicalSheet WHERE TechnicalSheetID=%n", id);
        }
        #endregion
    }
}
