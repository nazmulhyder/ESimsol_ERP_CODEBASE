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
    public class CommercialInvoiceDocDA
    {
        public CommercialInvoiceDocDA() { }

        #region Insert Function
        public static void Insert(TransactionContext tc, CommercialInvoiceDoc oCommercialInvoiceDoc, Int64 nUserID)
        {
            SqlParameter fileparameter = new SqlParameter();
            fileparameter.SqlDbType = SqlDbType.VarBinary;
            fileparameter.ParameterName = "file";
            fileparameter.Value = oCommercialInvoiceDoc.DocFile;

            string sSQL = SQLParser.MakeSQL("INSERT INTO CommercialInvoiceDoc(CommercialInvoiceDocID,CommercialInvoiceID,DocType,DocName,Note,DocFile,FileType,DBUserID,DBServerDateTime)"
                + " VALUES(%n, %n, %n, %s, %s, %q, %s, %n, %D)",
                oCommercialInvoiceDoc.CommercialInvoiceDocID, oCommercialInvoiceDoc.CommercialInvoiceID, (int)oCommercialInvoiceDoc.DocType, oCommercialInvoiceDoc.DocName, oCommercialInvoiceDoc.Note, "@file", oCommercialInvoiceDoc.FileType, nUserID, DateTime.Now);

            tc.ExecuteNonQueryCommText(CommandType.Text, sSQL, fileparameter);
        }

        #endregion

        #region Update Function
        public static void Update(TransactionContext tc, CommercialInvoiceDoc oCommercialInvoiceDoc, Int64 nUserID)
        {
            SqlParameter fileparameter = new SqlParameter();
            fileparameter.SqlDbType = SqlDbType.VarBinary;
            fileparameter.ParameterName = "file";
            fileparameter.Value = oCommercialInvoiceDoc.DocFile;

            string sSQL = SQLParser.MakeSQL("UPDATE CommercialInvoiceDoc SET CommercialInvoiceID=%n, DocType=%n, DocName=%s, Note=%s, DocFile=%q, FileType=%s, DBUserID=%n, DBServerDateTime=%D"
                + " WHERE CommercialInvoiceDocID=%n", oCommercialInvoiceDoc.CommercialInvoiceID, (int)oCommercialInvoiceDoc.DocType, oCommercialInvoiceDoc.DocName, oCommercialInvoiceDoc.Note, "@file", oCommercialInvoiceDoc.FileType, nUserID, DateTime.Now, oCommercialInvoiceDoc.CommercialInvoiceDocID);

            tc.ExecuteNonQueryCommText(CommandType.Text, sSQL, fileparameter);
        }

        #endregion

        #region Delete Function
        public static void Delete(TransactionContext tc, int nID)
        {
            tc.ExecuteNonQuery("DELETE FROM CommercialInvoiceDoc WHERE CommercialInvoiceDocID=%n", nID);
        }
        #endregion

        #region Generation Function
        public static int GetNewID(TransactionContext tc)
        {
            return tc.GenerateID("CommercialInvoiceDoc", "CommercialInvoiceDocID");
        }
        #endregion

        #region Get & Exist Function

        public static IDataReader Gets(TransactionContext tc, int id)
        {
            return tc.ExecuteReader("SELECT * FROM View_CommercialInvoiceDoc WHERE CommercialInvoiceID=%n", id);
        }

        public static IDataReader GetWithDocFile(TransactionContext tc, int id)
        {
            return tc.ExecuteReader("SELECT * FROM View_CommercialInvoiceDocWithFile WHERE CommercialInvoiceDocID=%n", id);
        }

        public static IDataReader Get(TransactionContext tc, int id)
        {
            return tc.ExecuteReader("SELECT * FROM View_CommercialInvoiceDoc WHERE CommercialInvoiceDocID=%n", id);
        }
        #endregion
    }
}
