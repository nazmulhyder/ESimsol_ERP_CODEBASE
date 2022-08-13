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
    public class OrderRecapYarnDA
    {
        public OrderRecapYarnDA() { }

        #region Insert Update Delete Function
        public static IDataReader InsertUpdate(TransactionContext tc, OrderRecapYarn oOrderRecapYarn, EnumDBOperation eEnumDBOperation, Int64 nUserID, string sOrderRecapYarnIDs)
        {
            return tc.ExecuteReader("EXEC [SP_IUD_OrderRecapYarn]" + "%n, %n, %n,  %s,%n,%n,%s, %s, %n, %n",
                                    oOrderRecapYarn.OrderRecapYarnID, oOrderRecapYarn.RefObjectID, oOrderRecapYarn.YarnID, oOrderRecapYarn.YarnPly, (int)oOrderRecapYarn.RefType, (int)oOrderRecapYarn.YarnType, oOrderRecapYarn.Note,  sOrderRecapYarnIDs, nUserID, (int)eEnumDBOperation);
        }


        public static void Delete(TransactionContext tc, OrderRecapYarn oOrderRecapYarn, EnumDBOperation eEnumDBOperation, Int64 nUserID, string sOrderRecapYarnIDs)
        {
            tc.ExecuteNonQuery("EXEC [SP_IUD_OrderRecapYarn]" + "%n, %n, %n,  %s,%n,%n,%s, %s, %n, %n",
                                    oOrderRecapYarn.OrderRecapYarnID, oOrderRecapYarn.RefObjectID, oOrderRecapYarn.YarnID, oOrderRecapYarn.YarnPly, (int)oOrderRecapYarn.RefType, (int)oOrderRecapYarn.YarnType, oOrderRecapYarn.Note, sOrderRecapYarnIDs, nUserID, (int)eEnumDBOperation);
        }
        #endregion

        #region Get & Exist Function
        public static IDataReader Gets_Report(TransactionContext tc, int id)
        {
            return tc.ExecuteReader("SELECT * FROM View_OrderRecapYarn WHERE DevelopmentRecapID=%n", id);
        }
        public static IDataReader Get(TransactionContext tc, long nID)
        {
            return tc.ExecuteReader("SELECT * FROM View_OrderRecapYarn WHERE OrderRecapYarnID=%n", nID);
        }

        public static IDataReader Gets(TransactionContext tc)
        {
            return tc.ExecuteReader("SELECT * FROM View_OrderRecapYarn");
        }
        public static IDataReader Gets(TransactionContext tc, int nrefID, int nRefType)
        {
            return tc.ExecuteReader("SELECT * FROM View_OrderRecapYarn WHERE RefObjectID=%n AND RefType =%n", nrefID, nRefType);
        }

        public static IDataReader GetsByLog(TransactionContext tc, int id)
        {
            return tc.ExecuteReader("SELECT * FROM View_OrderRecapYarnLog WHERE OrderRecapLogID=%n", id);
        }
        public static IDataReader Gets(TransactionContext tc, string sSQL)
        {
            return tc.ExecuteReader(sSQL);
        }
        #endregion
    }
}
