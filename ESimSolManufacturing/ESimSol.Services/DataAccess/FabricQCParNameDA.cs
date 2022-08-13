using System;
using ICS.Core.Framework;
using ICS.Core.Utility;
using ICS.Core;
using ESimSol.BusinessObjects;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Data.OleDb;
using ICS.Core.DataAccess;


namespace ESimSol.Services.DataAccess
{
    public class FabricQCParNameDA
    {
        #region Insert Update Delete Function
        public static IDataReader InsertUpdate(TransactionContext tc, FabricQCParName oFabricQCParName, EnumDBOperation eEnumDBOperation, Int64 nUserID)
        {
            return tc.ExecuteReader("EXEC [SP_IUD_FabricQCParName]"
                                   + "%n,%s,%s,%n,%n",
                                   oFabricQCParName.FabricQCParNameID,
                                   oFabricQCParName.Name,
                                   oFabricQCParName.Note,                    
                                   nUserID,
                                   (int)eEnumDBOperation);
        }

        public static void Delete(TransactionContext tc, FabricQCParName oFabricQCParName, EnumDBOperation eEnumDBOperation, Int64 nUserID)
        {
            tc.ExecuteNonQuery("EXEC [SP_IUD_FabricQCParName]"
                                   + "%n,%s,%s,%n,%n",
                                   oFabricQCParName.FabricQCParNameID,
                                   oFabricQCParName.Name,
                                   oFabricQCParName.Note,      
                                   nUserID,
                                   (int)eEnumDBOperation);
        }

        #endregion
        #region Get & Exist Function
        public static IDataReader Get(TransactionContext tc, long nID)
        {
            return tc.ExecuteReader("SELECT * FROM View_FabricQCParName WHERE FabricQCParNameID=%n", nID);
        }
        public static IDataReader Gets(TransactionContext tc)
        {
            return tc.ExecuteReader("SELECT * FROM View_FabricQCParName");
        }
        public static IDataReader Gets(TransactionContext tc, string sSQL)
        {
            return tc.ExecuteReader(sSQL);
        }
        #endregion
    }
}
