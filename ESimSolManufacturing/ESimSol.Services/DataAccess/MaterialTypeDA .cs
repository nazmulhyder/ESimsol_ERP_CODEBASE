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

    public class MaterialTypeDA
    {
        public MaterialTypeDA() { }

        #region Insert Update Delete Function
        public static IDataReader InsertUpdate(TransactionContext tc, MaterialType oMaterialType, EnumDBOperation eEnumDBOperation, Int64 nUserID)
        {
            return tc.ExecuteReader("EXEC [SP_IUD_MaterialType]"
                                    + "%n,  %s, %s, %n,%n",
                                    oMaterialType.MaterialTypeID,  oMaterialType.Name, oMaterialType.Note, (int)eEnumDBOperation, nUserID);
        }

        public static void Delete(TransactionContext tc, MaterialType oMaterialType, EnumDBOperation eEnumDBOperation, Int64 nUserID)
        {
            tc.ExecuteNonQuery("EXEC [SP_IUD_MaterialType]"
                                    + "%n,  %s, %s, %n,%n",
                                    oMaterialType.MaterialTypeID, oMaterialType.Name, oMaterialType.Note, (int)eEnumDBOperation, nUserID);
        }
        #endregion

        #region Get & Exist Function
        public static IDataReader Get(TransactionContext tc, long nID)
        {
            return tc.ExecuteReader("SELECT * FROM MaterialType WHERE MaterialTypeID=%n", nID);
        }

        public static IDataReader Gets(TransactionContext tc)
        {
            return tc.ExecuteReader("SELECT * FROM MaterialType");
        }
        public static IDataReader Gets(TransactionContext tc, string sSQL)
        {
            return tc.ExecuteReader(sSQL);
        }


        #endregion
    }  
  
}
