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
    public class GUQCDA
    {
        #region Insert Update Delete Function

        public static IDataReader InsertUpdate(TransactionContext tc, GUQC oGUQC, EnumDBOperation eEnumDBOperation, Int64 nUserID)
        {
            return tc.ExecuteReader("EXEC [SP_IUD_GUQC]"
                                   + "%n,%n,%s,%d,%n,%n,  %n, %s, %n,%n",
                                   oGUQC.GUQCID, oGUQC.BUID, oGUQC.QCNo, oGUQC.QCDate, oGUQC.QCBy, oGUQC.BuyerID, oGUQC.StoreID, oGUQC.Remarks, nUserID, (int)eEnumDBOperation);
        }

        public static void Delete(TransactionContext tc, GUQC oGUQC, EnumDBOperation eEnumDBOperation, Int64 nUserID)
        {
            tc.ExecuteNonQuery("EXEC [SP_IUD_GUQC]"
                                   + "%n,%n,%s,%d,%n,%n,  %n, %s, %n,%n",
                                   oGUQC.GUQCID, oGUQC.BUID, oGUQC.QCNo, oGUQC.QCDate, oGUQC.QCBy, oGUQC.BuyerID, oGUQC.StoreID, oGUQC.Remarks, nUserID, (int)eEnumDBOperation);
        }

        #endregion

        #region Get & Exist Function
        public static IDataReader Get(TransactionContext tc, long nID)
        {
            return tc.ExecuteReader("SELECT * FROM View_GUQC WHERE GUQCID=%n", nID);
        }
        public static IDataReader Gets(TransactionContext tc)
        {
            return tc.ExecuteReader("SELECT * FROM View_GUQC ");
        }
        public static IDataReader Gets(TransactionContext tc, string sSQL)
        {
            return tc.ExecuteReader(sSQL);
        }

        #endregion

        public static IDataReader Approve(TransactionContext tc, GUQC oGUQC, Int64 nUserID)
        {
            return tc.ExecuteReader("EXEC [SP_CommitGUQC]" + "%n,%n", oGUQC.GUQCID,nUserID);
        }
    }

}
