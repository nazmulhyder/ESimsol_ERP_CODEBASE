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
    public class DevelopmentYarnOptionDA
    {
        public DevelopmentYarnOptionDA() { }

        #region Insert Update Delete Function
        public static IDataReader InsertUpdate(TransactionContext tc, DevelopmentYarnOption oDevelopmentYarnOption, EnumDBOperation eEnumDBOperation, Int64 nUserID, String sDevelopmentYarnOptionIDs)
        {
            return tc.ExecuteReader("EXEC [SP_IUD_DevelopmentYarnOption]" + "%n, %n, %n, %s, %s, %n, %n, %s", 
                                    oDevelopmentYarnOption.DevelopmentYarnOptionID, oDevelopmentYarnOption.DevelopmentRecapID,oDevelopmentYarnOption.YarnCategoryID,  oDevelopmentYarnOption.Note, oDevelopmentYarnOption.YarnPly, nUserID, (int)eEnumDBOperation, "");
        }

        public static void Delete(TransactionContext tc, DevelopmentYarnOption oDevelopmentYarnOption, EnumDBOperation eEnumDBOperation, Int64 nUserID, String sDevelopmentYarnOptionIDs)
        {
            tc.ExecuteNonQuery("EXEC [SP_IUD_DevelopmentYarnOption]" + "%n, %n, %n, %s, %s, %n, %n, %s",
                                    oDevelopmentYarnOption.DevelopmentYarnOptionID, oDevelopmentYarnOption.DevelopmentRecapID, oDevelopmentYarnOption.YarnCategoryID, oDevelopmentYarnOption.Note, oDevelopmentYarnOption.YarnPly, nUserID, (int)eEnumDBOperation, sDevelopmentYarnOptionIDs);
        }
        #endregion

        #region Get & Exist Function
        public static IDataReader Get(TransactionContext tc, long nID)
        {
            return tc.ExecuteReader("SELECT * FROM View_DevelopmentYarnOption WHERE DevelopmentYarnOptionID=%n", nID);
        }

        public static IDataReader Gets(int nDevelopmentRecapID, TransactionContext tc)
        {
            return tc.ExecuteReader("SELECT * FROM View_DevelopmentYarnOption where DevelopmentRecapID =%n", nDevelopmentRecapID);
        }

        public static IDataReader Gets_Report(TransactionContext tc, int id)
        {
            return tc.ExecuteReader("SELECT * FROM View_DevelopmentYarnOption WHERE DevelopmentRecapID=%n", id);
        }

        public static IDataReader Gets(TransactionContext tc, string sSQL)
        {
            return tc.ExecuteReader(sSQL);
        }
        #endregion
    }
    
 
}
