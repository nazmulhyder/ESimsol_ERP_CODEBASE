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

    public class PackageTemplateDA
    {
        public PackageTemplateDA() { }

        #region Insert Update Delete Function
        public static IDataReader InsertUpdate(TransactionContext tc, PackageTemplate oPackageTemplate, EnumDBOperation eEnumDBOperation, Int64 nUserID)
        {
            return tc.ExecuteReader("EXEC [SP_IUD_PackageTemplate]"
                                    + "%n,%s,%s,%s,%n,%n,%n",
                                    oPackageTemplate.PackageTemplateID, oPackageTemplate.PackageNo, oPackageTemplate.PackageName, oPackageTemplate.Note, oPackageTemplate.BUID,  nUserID, (int)eEnumDBOperation);
        }

        public static void Delete(TransactionContext tc, PackageTemplate oPackageTemplate, EnumDBOperation eEnumDBOperation, Int64 nUserID)
        {
            tc.ExecuteNonQuery("EXEC [SP_IUD_PackageTemplate]"
                                    + "%n,%s,%s,%s,%n,%n,%n",
                                    oPackageTemplate.PackageTemplateID, oPackageTemplate.PackageNo, oPackageTemplate.PackageName, oPackageTemplate.Note, oPackageTemplate.BUID, nUserID, (int)eEnumDBOperation);
        }



        #endregion

        #region Get & Exist Function
        public static IDataReader Get(TransactionContext tc, long nID)
        {
            return tc.ExecuteReader("SELECT * FROM PackageTemplate WHERE PackageTemplateID=%n", nID);
        }

        public static IDataReader Gets(TransactionContext tc)
        {
            return tc.ExecuteReader("SELECT * FROM PackageTemplate");
        }
        public static IDataReader Gets(TransactionContext tc, string sSQL)
        {
            return tc.ExecuteReader(sSQL);
        }
        #endregion
    }
    
    

}
