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

    public class CostSheetPackageDetailDA
    {
        public CostSheetPackageDetailDA() { }

        #region Insert Update Delete Function
        public static IDataReader InsertUpdate(TransactionContext tc, CostSheetPackageDetail oCostSheetPackageDetail, EnumDBOperation eEnumDBOperation, Int64 nUserID)
        {
            return tc.ExecuteReader("EXEC[SP_IUD_CostSheetPackageDetail]"
                                    + "%n, %n,%n,%s,%n,%n",
                                    oCostSheetPackageDetail.CostSheetPackageDetailID, oCostSheetPackageDetail.CostSheetPackageID, oCostSheetPackageDetail.ProductID, oCostSheetPackageDetail.Description, nUserID, (int)eEnumDBOperation );
        }

        public static void Delete(TransactionContext tc, CostSheetPackageDetail oCostSheetPackageDetail, EnumDBOperation eEnumDBOperation, Int64 nUserID )
        {
            tc.ExecuteNonQuery("EXEC[SP_IUD_CostSheetPackageDetail]"
                                    + "%n, %n,%n,%s,%n,%n",
                                    oCostSheetPackageDetail.CostSheetPackageDetailID, oCostSheetPackageDetail.CostSheetPackageID, oCostSheetPackageDetail.ProductID, oCostSheetPackageDetail.Description, nUserID, (int)eEnumDBOperation);
        }
        #endregion

        #region Get & Exist Function
        public static IDataReader Get(TransactionContext tc, long nID)
        {
            return tc.ExecuteReader("SELECT * FROM View_CostSheetPackageDetail WHERE CostSheetPackageDetailID=%n", nID);
        }
        public static IDataReader GetsByCostSheetID(int id, TransactionContext tc)
        {
            return tc.ExecuteReader("SELECT * FROM View_CostSheetPackageDetail WHERE CostSheetPackageID IN ( SELECT CostSheetPackageID FROM CostSheetPackage WHERE CostSheetID = %n)", id);
        }

        public static IDataReader Gets(TransactionContext tc)
        {
            return tc.ExecuteReader("SELECT * FROM View_CostSheetPackageDetail");
        }
        public static IDataReader Gets(TransactionContext tc, string sSQL)
        {
            return tc.ExecuteReader(sSQL);
        }
        #endregion
    }
  
}
