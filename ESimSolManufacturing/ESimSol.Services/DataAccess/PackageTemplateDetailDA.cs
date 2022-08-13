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

    public class PackageTemplateDetailDA
    {
        public PackageTemplateDetailDA() { }

        #region Insert Update Delete Function
        public static IDataReader InsertUpdate(TransactionContext tc, PackageTemplateDetail oPackageTemplateDetail, EnumDBOperation eEnumDBOperation, Int64 nUserID, string sPackageTemplateDetailIDs)
        {
            return tc.ExecuteReader("EXEC[SP_IUD_PackageTemplateDetail]"
                                    + "%n, %n,%n,%s,%n,%n,%s",
                                    oPackageTemplateDetail.PackageTemplateDetailID, oPackageTemplateDetail.PackageTemplateID, oPackageTemplateDetail.ProductID, oPackageTemplateDetail.Quantity, nUserID, (int)eEnumDBOperation, "");
        }

        public static void Delete(TransactionContext tc, PackageTemplateDetail oPackageTemplateDetail, EnumDBOperation eEnumDBOperation, Int64 nUserID, string sPackageTemplateDetailIDs)
        {
            tc.ExecuteNonQuery("EXEC[SP_IUD_PackageTemplateDetail]"
                                    + "%n, %n,%n,%s,%n,%n,%s",
                                    oPackageTemplateDetail.PackageTemplateDetailID, oPackageTemplateDetail.PackageTemplateID, oPackageTemplateDetail.ProductID, oPackageTemplateDetail.Quantity, nUserID, (int)eEnumDBOperation, sPackageTemplateDetailIDs);
        }
        #endregion

        #region Get & Exist Function
        public static IDataReader Get(TransactionContext tc, long nID)
        {
            return tc.ExecuteReader("SELECT * FROM View_PackageTemplateDetail WHERE PackageTemplateDetailID=%n", nID);
        }
        public static IDataReader Gets(int nPackageTemplateID, TransactionContext tc)
        {
            return tc.ExecuteReader("SELECT * FROM View_PackageTemplateDetail WHERE PackageTemplateID =%n", nPackageTemplateID);
        }

        public static IDataReader Gets(TransactionContext tc)
        {
            return tc.ExecuteReader("SELECT * FROM View_PackageTemplateDetail");
        }
        public static IDataReader Gets(TransactionContext tc, string sSQL)
        {
            return tc.ExecuteReader(sSQL);
        }
        #endregion
    }
    
    
   
}
