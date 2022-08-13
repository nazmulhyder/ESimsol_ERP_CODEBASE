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
    public class FGQCDetailDA
    {
        #region Insert Update Delete Function

        public static IDataReader InsertUpdate(TransactionContext tc, FGQCDetail oFGQCDetail, EnumDBOperation eEnumDBOperation, Int64 nUserID, string sFGQCDetailIDs)
        {
            return tc.ExecuteReader("EXEC [SP_IUD_FGQCDetail]"
                                    + "%n, %n, %n, %n, %n, %n, %n, %n, %n, %n, %n, %s ",
                                    oFGQCDetail.FGQCDetailID, oFGQCDetail.FGQCID, oFGQCDetail.RefTypeInt, oFGQCDetail.RefID, oFGQCDetail.ProductID, oFGQCDetail.MUnitID, oFGQCDetail.Qty, oFGQCDetail.UnitPrice, oFGQCDetail.StoreID, nUserID, (int)eEnumDBOperation, sFGQCDetailIDs);
        }

        public static void Delete(TransactionContext tc, FGQCDetail oFGQCDetail, EnumDBOperation eEnumDBOperation, Int64 nUserID, string sFGQCDetailIDs)
        {
            tc.ExecuteNonQuery("EXEC [SP_IUD_FGQCDetail]"
                                    + "%n, %n, %n, %n, %n, %n, %n, %n, %n, %n, %n, %s ",
                                    oFGQCDetail.FGQCDetailID, oFGQCDetail.FGQCID, oFGQCDetail.RefTypeInt, oFGQCDetail.RefID, oFGQCDetail.ProductID, oFGQCDetail.MUnitID, oFGQCDetail.Qty, oFGQCDetail.UnitPrice, oFGQCDetail.StoreID, nUserID, (int)eEnumDBOperation, sFGQCDetailIDs);
        }
        #endregion

        #region Get & Exist Function
        public static IDataReader Get(TransactionContext tc, long nID)
        {
            return tc.ExecuteReader("SELECT * FROM View_FGQCDetail WHERE FGQCDetailID=%n", nID);
        }
        public static IDataReader Gets(int id, TransactionContext tc)
        {
            return tc.ExecuteReader("SELECT * FROM View_FGQCDetail WHERE FGQCID = %n", id);
        }
        public static IDataReader Gets(TransactionContext tc, string sSQL)
        {
            return tc.ExecuteReader(sSQL);
        }
        public static IDataReader FGQCProcess(TransactionContext tc, FGQC oFGQC)
        {
            return tc.ExecuteReader("EXEC [SP_FGQCProcess]" + "%n, %d", oFGQC.BUID, oFGQC.FGQCDate);
        }
        #endregion
    }
}
