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

    public class CostSheetPackageDA
    {
        public CostSheetPackageDA() { }

        #region Insert Update Delete Function
        public static IDataReader InsertUpdate(TransactionContext tc, CostSheetPackage oCostSheetPackage, EnumDBOperation eEnumDBOperation, Int64 nUserID)
        {
            return tc.ExecuteReader("EXEC [SP_IUD_CostSheetPackage]"
                                    + "%n,%s,%n,%n,%s,%n,%n,%s",
                                    oCostSheetPackage.CostSheetPackageID, oCostSheetPackage.PackageName,  oCostSheetPackage.CostSheetID, oCostSheetPackage.Price, oCostSheetPackage.Remark, nUserID, (int)eEnumDBOperation,"");
        }

        public static void Delete(TransactionContext tc, CostSheetPackage oCostSheetPackage, EnumDBOperation eEnumDBOperation, Int64 nUserID, string sCostSheetPackageIDs)
        {
            tc.ExecuteNonQuery("EXEC [SP_IUD_CostSheetPackage]"
                                    + "%n,%s,%n,%n,%s,%n,%n,%s",
                                    oCostSheetPackage.CostSheetPackageID, oCostSheetPackage.PackageName, oCostSheetPackage.CostSheetID, oCostSheetPackage.Price, oCostSheetPackage.Remark, nUserID, (int)eEnumDBOperation, sCostSheetPackageIDs);
        }



        #endregion

        #region Get & Exist Function
        public static IDataReader Get(TransactionContext tc, long nID)
        {
            return tc.ExecuteReader("SELECT * FROM CostSheetPackage WHERE CostSheetPackageID=%n", nID);
        }

        public static IDataReader Gets(TransactionContext tc)
        {
            return tc.ExecuteReader("SELECT * FROM CostSheetPackage");
        }

        public static IDataReader Gets(int id, TransactionContext tc)
        {
            return tc.ExecuteReader("SELECT * FROM CostSheetPackage WHERE CostSheetID = %n",id);
        }
        public static IDataReader Gets(TransactionContext tc, string sSQL)
        {
            return tc.ExecuteReader(sSQL);
        }
        #endregion
    }
  
}
