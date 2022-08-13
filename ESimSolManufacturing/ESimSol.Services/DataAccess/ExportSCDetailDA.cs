using System;
using System.Data;
using System.Data.SqlClient;
using ESimSol.BusinessObjects;
using System.Data.OleDb;
using ICS.Core.DataAccess;
using ICS.Core.Utility;

namespace ESimSol.Services.DataAccess
{
    public class ExportSCDetailDA
    {
        public ExportSCDetailDA() { }

        #region Insert Update Delete Function
        public static IDataReader InsertUpdate(TransactionContext tc, ExportSCDetail oExportSCDetail, EnumDBOperation eEnumDBOperation, Int64 nUserID, string sExportSCDetaillIDs)
        {
            return tc.ExecuteReader("EXEC[SP_IUD_ExportSCDetail]" + "%n, %n, %n,%n, %n,%n, %n, %n,%n, %s,%s,%s,  %n, %n, %n,%n,%s, %n,%n,%n,%b,%b,%b,%n, %n, %n,%s",
                                    oExportSCDetail.ExportSCDetailID, oExportSCDetail.ExportSCID, oExportSCDetail.ProductionType,oExportSCDetail.ProductID, oExportSCDetail.Qty, oExportSCDetail.OverQty,   oExportSCDetail.MUnitID, oExportSCDetail.UnitPrice, oExportSCDetail.Amount,   oExportSCDetail.StyleNo, oExportSCDetail.ColorInfo, oExportSCDetail.BuyerRef, oExportSCDetail.ColorID, oExportSCDetail.ModelReferenceID, oExportSCDetail.OrderSheetDetailID, oExportSCDetail.PolyMUnitID, oExportSCDetail.ProductDescription, oExportSCDetail.ColorQty, oExportSCDetail.DyeingType,	oExportSCDetail.BagCount,oExportSCDetail.IsBuyerYarn,oExportSCDetail.IsBuyerDyes,oExportSCDetail.IsBuyerChemical,oExportSCDetail.ExportPIDetailID, (int)eEnumDBOperation, nUserID, sExportSCDetaillIDs);
        }
        public static void Delete(TransactionContext tc, ExportSCDetail oExportSCDetail, EnumDBOperation eEnumDBOperation, Int64 nUserID, string sExportSCDetaillIDs)
        {
            tc.ExecuteNonQuery("EXEC[SP_IUD_ExportSCDetail]" + "%n, %n, %n,%n, %n,%n, %n, %n,%n, %s,%s,%s,  %n, %n, %n,%n,%s, %n,%n,%n,%b,%b,%b,%n, %n, %n,%s",
                                    oExportSCDetail.ExportSCDetailID, oExportSCDetail.ExportSCID, oExportSCDetail.ProductionType, oExportSCDetail.ProductID, oExportSCDetail.Qty, oExportSCDetail.OverQty, oExportSCDetail.MUnitID, oExportSCDetail.UnitPrice, oExportSCDetail.Amount, oExportSCDetail.StyleNo, oExportSCDetail.ColorInfo, oExportSCDetail.BuyerRef, oExportSCDetail.ColorID, oExportSCDetail.ModelReferenceID, oExportSCDetail.OrderSheetDetailID, oExportSCDetail.PolyMUnitID, oExportSCDetail.ProductDescription, oExportSCDetail.ColorQty, oExportSCDetail.DyeingType, oExportSCDetail.BagCount, oExportSCDetail.IsBuyerYarn, oExportSCDetail.IsBuyerDyes, oExportSCDetail.IsBuyerChemical, oExportSCDetail.ExportPIDetailID, (int)eEnumDBOperation, nUserID, sExportSCDetaillIDs);
        }
        //public static void DeleteRevise(TransactionContext tc, ExportSCDetail oExportSCDetail, EnumDBOperation eEnumDBOperation, Int64 nUserID, string sExportSCDetailIDs)
        //{
        //    tc.ExecuteNonQuery("EXEC [SP_IUD_ExportSCDetailRevise]" + "%n, %n, %n, %n, %n, %n, %n, %n, %s,%s,%s,%s,%s,%s, %n, %n",
        //                           oExportSCDetail.ExportSCDetailID, oExportSCDetail.ProductID, oExportSCDetail.Qty, oExportSCDetail.MUnitID, oExportSCDetail.UnitPrice, oExportSCDetail.Amount, oExportSCDetail.Description, oExportSCDetail.StyleNo, oExportSCDetail.StyleNo, oExportSCDetail.BuyerRef, oExportSCDetail.ColorInfo, sExportSCDetailIDs, (int)eEnumDBOperation, nUserID);
        //}
        #endregion

        #region Get & Exist Function
        public static IDataReader Get(TransactionContext tc, long nID)
        {
            return tc.ExecuteReader("SELECT * FROM View_ExportSCDetail WHERE ExportSCDetailID=%n", nID);
        }

        public static IDataReader Gets(TransactionContext tc,int nExportLCID)
        {
            return tc.ExecuteReader("SELECT * FROM View_ExportSCDetail WHERE ExportPIID in (Select ExportPIID from [ExportPI] where LCID =%n ) ", nExportLCID);
        }

        public static IDataReader GetsByESCID(TransactionContext tc, int nExportSCID)
        {
            return tc.ExecuteReader("SELECT * FROM View_ExportSCDetail WHERE ExportSCID=%n Order By ExportSCDetailID", nExportSCID);
        }
        public static IDataReader Gets(TransactionContext tc)
        {
            return tc.ExecuteReader("SELECT * FROM View_ExportSCDetail");
        }

        public static IDataReader GetsByPI(TransactionContext tc, int nPIID)
        {
            return tc.ExecuteReader("SELECT * FROM View_ExportSCDetail WHERE ExportSCID in (Select ExportSCID from ExportSC where ExportPIID=%n) order by ProductID", nPIID);
        }
        public static IDataReader Gets(TransactionContext tc, string sSQL)
        {
            return tc.ExecuteReader(sSQL);
        }
        public static IDataReader GetsLog(TransactionContext tc, int nPIID)
        {
            return tc.ExecuteReader("SELECT * FROM [View_ExportSCDetailsLog] WHERE ExportSCID=%n", nPIID);
        }
        public static IDataReader GetsByLC(TransactionContext tc, int nExportLCID)
        {
            return tc.ExecuteReader("SELECT * FROM View_ExportSCDetails_LC WHERE PIID in (Select ExportPIID from [ExportPI] where LCID =%n ) ", nExportLCID);
        }
        public static void Save_UP(TransactionContext tc, ExportSCDetail oExportSCDetail, Int64 nUserId)
        {
            tc.ExecuteNonQuery("Update ExportSCDetail SET UnitPrice =%n,Amount=%n  WHERE ExportSCDetailID = %n", oExportSCDetail.UnitPrice, oExportSCDetail.Amount, oExportSCDetail.ExportSCDetailID);
        }
        #endregion
    }
}
