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

    public class ORAssortmentDA
    {
        public ORAssortmentDA() { }

        #region Insert Update Delete Function
        public static IDataReader InsertUpdate(TransactionContext tc, ORAssortment oORAssortment, EnumDBOperation eEnumDBOperation, Int64 nUserID)
        {
            return tc.ExecuteReader("EXEC [SP_IUD_ORAssortment]" + "%n, %n, %n, %n, %n, %n, %n, %s",
                                    oORAssortment.ORAssortmentID, oORAssortment.OrderRecapID, oORAssortment.ColorID, oORAssortment.SizeID, oORAssortment.Qty, nUserID, (int)eEnumDBOperation, "");
        }

        public static void Delete(TransactionContext tc, ORAssortment oORAssortment, EnumDBOperation eEnumDBOperation, Int64 nUserID, String sORAssortmentIDs)
        {
            tc.ExecuteNonQuery("EXEC [SP_IUD_ORAssortment]" + "%n, %n, %n, %n, %n, %n, %n, %s",
                                    oORAssortment.ORAssortmentID, oORAssortment.OrderRecapID, oORAssortment.ColorID, oORAssortment.SizeID, oORAssortment.Qty, nUserID, (int)eEnumDBOperation, sORAssortmentIDs);
        }
        #endregion

        #region Get & Exist Function
        public static IDataReader Get(TransactionContext tc, long nID)
        {
            return tc.ExecuteReader("SELECT * FROM View_ORAssortment WHERE ORAssortmentID=%n", nID);
        }

        public static IDataReader Gets(int nOrderRecapID, TransactionContext tc)
        {
            return tc.ExecuteReader("SELECT * FROM View_ORAssortment WHERE OrderRecapID =%n ORDER BY ColorSequence", nOrderRecapID);
        }
        public static IDataReader Gets(TransactionContext tc, string sSQL)
        {
            return tc.ExecuteReader(sSQL);
        }

        public static IDataReader GetsByLog(TransactionContext tc, int id)
        {
            return tc.ExecuteReader("SELECT * FROM View_ORAssortmentLog WHERE OrderRecapLogID=%n ORDER BY ColorSequence", id);//don not  change  ColorSequence Column
        }
        #endregion
    }
    
   
}
