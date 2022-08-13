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
    public class ORBarCodeDA
    {
        public ORBarCodeDA() { }

        #region Insert Update Delete Function
        public static IDataReader InsertUpdate(TransactionContext tc, ORBarCode oORBarCode, EnumDBOperation eEnumDBOperation, Int64 nUserID)
        {
            return tc.ExecuteReader("EXEC [SP_IUD_ORBarCode]" + "%n, %n, %n, %n, %s, %n, %n, %s",
                                    oORBarCode.ORBarCodeID, oORBarCode.OrderRecapID, oORBarCode.ColorID, oORBarCode.SizeID, oORBarCode.BarCode, nUserID, (int)eEnumDBOperation, "");
        }

        public static void Delete(TransactionContext tc, ORBarCode oORBarCode, EnumDBOperation eEnumDBOperation, Int64 nUserID, String sORBarCodeIDs)
        {
            tc.ExecuteNonQuery("EXEC [SP_IUD_ORBarCode]" + "%n, %n, %n, %n, %s, %n, %n, %s",
                                    oORBarCode.ORBarCodeID, oORBarCode.OrderRecapID, oORBarCode.ColorID, oORBarCode.SizeID, oORBarCode.BarCode, nUserID, (int)eEnumDBOperation, sORBarCodeIDs);
        }
        #endregion

        #region Get & Exist Function
        public static IDataReader Get(TransactionContext tc, long nID)
        {
            return tc.ExecuteReader("SELECT * FROM View_ORBarCode WHERE ORBarCodeID=%n", nID);
        }

        public static IDataReader Gets(int nOrderRecapID, TransactionContext tc)
        {
            return tc.ExecuteReader("SELECT * FROM View_ORBarCode WHERE OrderRecapID =%n ORDER BY ColorSequence", nOrderRecapID);
        }
        public static IDataReader Gets(TransactionContext tc, string sSQL)
        {
            return tc.ExecuteReader(sSQL);
        }

        public static IDataReader GetsByLog(TransactionContext tc, int id)
        {
            return tc.ExecuteReader("SELECT * FROM View_ORBarCodeLog WHERE OrderRecapLogID=%n ORDER BY ColorSequence", id);//don not  change  ColorSequence Column
        }
        #endregion
    }
}
