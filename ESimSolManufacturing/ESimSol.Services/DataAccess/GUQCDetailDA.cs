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
    public class GUQCDetailDA
    {
        #region Insert Update Delete Function

        public static IDataReader InsertUpdate(TransactionContext tc, GUQCDetail oGUQCDetail, EnumDBOperation eEnumDBOperation, Int64 nUserID, string sGUQCDetailIDs)
        {
            return tc.ExecuteReader("EXEC [SP_IUD_GUQCDetail]"
                                   + "%n,%n,%n, %n,%n,    %s,   %n,%n,%s",
                                   oGUQCDetail.GUQCDetailID, oGUQCDetail.GUQCID, oGUQCDetail.OrderRecapID, oGUQCDetail.QCPassQty, oGUQCDetail.RejectQty, oGUQCDetail.Remarks, nUserID, (int)eEnumDBOperation, "");
        }

        public static void Delete(TransactionContext tc, GUQCDetail oGUQCDetail, EnumDBOperation eEnumDBOperation, Int64 nUserID, string sGUQCDetailIDs)
        {
            tc.ExecuteNonQuery("EXEC [SP_IUD_GUQCDetail]"
                                   + "%n,%n,%n, %n,%n,    %s,   %n,%n,%s",
                                   oGUQCDetail.GUQCDetailID, oGUQCDetail.GUQCID, oGUQCDetail.OrderRecapID, oGUQCDetail.QCPassQty, oGUQCDetail.RejectQty, oGUQCDetail.Remarks, nUserID, (int)eEnumDBOperation, sGUQCDetailIDs);
        }

        #endregion

        #region Get & Exist Function
        public static IDataReader Get(TransactionContext tc, long nID)
        {
            return tc.ExecuteReader("SELECT * FROM View_GUQCDetail WHERE GUQCDetailID=%n", nID);
        }
        public static IDataReader Gets(TransactionContext tc, int nGUQCID)
        {
            return tc.ExecuteReader("SELECT * FROM View_GUQCDetail WHERE GUQCID =%n", nGUQCID);
        }
        public static IDataReader Gets(TransactionContext tc, string sSQL)
        {
            return tc.ExecuteReader(sSQL);
        }

        #endregion
    }

}
