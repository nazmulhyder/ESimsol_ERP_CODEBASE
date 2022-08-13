using System;
using System.Data;
using System.Data.SqlClient;
using ESimSol.BusinessObjects;
using System.Data.OleDb;
using ICS.Core.DataAccess;
using ICS.Core.Utility;

namespace ESimSol.Services.DataAccess
{
    public class NOAQuotationDA
    {
        public NOAQuotationDA() { }

        #region Insert Update Delete Function
        public static IDataReader InsertUpdate(TransactionContext tc, NOAQuotation oNOAQuotation, EnumDBOperation eEnumDBOperation, Int64 nUserID, string @DetailIDs)
        {
            return tc.ExecuteReader("EXEC [SP_IUD_NOAQuotation]"
                                    + "%n, %n, %n, %n, %n,%s",
                                    oNOAQuotation.NOAQuotationID, oNOAQuotation.NOADetailID, oNOAQuotation.PQDetailID, (int)eEnumDBOperation, nUserID, @DetailIDs);
        }
        public static void Delete(TransactionContext tc, NOAQuotation oNOAQuotation, EnumDBOperation eEnumDBOperation, Int64 nUserID, string @DetailIDs)
        {
            tc.ExecuteNonQuery("EXEC [SP_IUD_NOAQuotation]"
                                    + "%n, %n, %n, %n, %n,%s",
                                    oNOAQuotation.NOAQuotationID, oNOAQuotation.NOADetailID, oNOAQuotation.PQDetailID, (int)eEnumDBOperation, nUserID, @DetailIDs);
        }
        #endregion

        #region Get & Exist Function
        public static IDataReader Get(TransactionContext tc, long nId)
        {
            return tc.ExecuteReader("SELECT * FROM View_NOAQuotation WHERE NOAQuotationID=%n", nId);
        }

        public static IDataReader Gets(TransactionContext tc)
        {
            return tc.ExecuteReader("SELECT * FROM View_NOAQuotation");
        }

        public static IDataReader Gets(string sSQL, TransactionContext tc)
        {
            return tc.ExecuteReader(sSQL);
        }
        public static IDataReader GetsByLog(string sSQL, TransactionContext tc)
        {
            return tc.ExecuteReader(sSQL);
        }
        public static IDataReader Gets(TransactionContext tc, long nNOADetailId)
        {
            return tc.ExecuteReader("SELECT * FROM View_NOAQuotation WHERE NOADetailID=%n ", nNOADetailId);
        }
        public static IDataReader Gets(TransactionContext tc, string sSQL)
        {
            return tc.ExecuteReader(sSQL);
        }
        #endregion
    }  
    

}
