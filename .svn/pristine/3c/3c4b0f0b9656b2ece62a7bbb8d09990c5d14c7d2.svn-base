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
    public class ProformaInvoiceRequiredDocDA
    {
        public ProformaInvoiceRequiredDocDA() { }

        #region Insert Update Delete Function
        public static IDataReader InsertUpdate(TransactionContext tc, ProformaInvoiceRequiredDoc oProformaInvoiceRequiredDoc, EnumDBOperation eEnumDBOperation, string sProformaInvoiceRequireDocIDs, Int64 nUserID)
        {
            return tc.ExecuteReader("EXEC [SP_IUD_ProformaInvoiceRequiredDoc]"
                                    + "%n,%n,%n,%n,%n,%s", oProformaInvoiceRequiredDoc.ProformaInvoiceRequiredDocID, oProformaInvoiceRequiredDoc.ProformaInvoiceID, (int)oProformaInvoiceRequiredDoc.DocType, nUserID, (int)eEnumDBOperation,"");
        }
        public static void Delete(TransactionContext tc, ProformaInvoiceRequiredDoc oProformaInvoiceRequiredDoc, EnumDBOperation eEnumDBOperation, string sProformaInvoiceRequireDocIDs, Int64 nUserID)
        {
            tc.ExecuteNonQuery("EXEC [SP_IUD_ProformaInvoiceRequiredDoc]"
                                    + "%n,%n,%n,%n,%n,%s", oProformaInvoiceRequiredDoc.ProformaInvoiceRequiredDocID, oProformaInvoiceRequiredDoc.ProformaInvoiceID, (int)oProformaInvoiceRequiredDoc.DocType, nUserID, (int)eEnumDBOperation, sProformaInvoiceRequireDocIDs);
        }

        #endregion

        #region Get & Exist Function
        public static IDataReader Get(TransactionContext tc, long nID)
        {
            return tc.ExecuteReader("SELECT * FROM View_ProformaInvoiceRequiredDoc WHERE ProformaInvoiceRequiredDocID=%n", nID);
        }

        public static IDataReader Gets(TransactionContext tc, int id)
        {
            return tc.ExecuteReader("SELECT * FROM View_ProformaInvoiceRequiredDoc WHERE ProformaInvoiceID = %n", id);
        }

        public static IDataReader GetsPILog(TransactionContext tc, int id)
        {
            return tc.ExecuteReader("SELECT * FROM View_ProformaInvoiceRequiredDocLog WHERE ProformaInvoiceLogID = %n", id);
        }

        

        public static IDataReader Gets(TransactionContext tc, string sSQL)
        {
            return tc.ExecuteReader(sSQL);
        }
        #endregion
    }
}
