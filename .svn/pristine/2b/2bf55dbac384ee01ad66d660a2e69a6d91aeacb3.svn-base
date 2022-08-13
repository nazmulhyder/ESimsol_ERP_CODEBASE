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

    public class ProformaInvoiceTermsAndConditionDA
    {
        public ProformaInvoiceTermsAndConditionDA() { }

        #region Insert Update Delete Function
        public static IDataReader InsertUpdate(TransactionContext tc, ProformaInvoiceTermsAndCondition oProformaInvoiceTermsAndCondition, EnumDBOperation eEnumDBOperation, string sProformaInvoiceTermsAndConditionIDs, Int64 nUserID)
        {
            return tc.ExecuteReader("EXEC [SP_IUD_ProformaInvoiceTermsAndCondition]"
                                    + "%n,%n,%s,%n,%n,%s", oProformaInvoiceTermsAndCondition.ProformaInvoiceTermsAndConditionID, oProformaInvoiceTermsAndCondition.ProformaInvoiceID, oProformaInvoiceTermsAndCondition.TermsAndCondition, nUserID, (int)eEnumDBOperation, "");
        }
        public static void Delete(TransactionContext tc, ProformaInvoiceTermsAndCondition oProformaInvoiceTermsAndCondition, EnumDBOperation eEnumDBOperation, string sProformaInvoiceTermsAndConditionIDs, Int64 nUserID)
        {
            tc.ExecuteNonQuery("EXEC [SP_IUD_ProformaInvoiceTermsAndCondition]"
                                    + "%n,%n,%s,%n,%n,%s", oProformaInvoiceTermsAndCondition.ProformaInvoiceTermsAndConditionID, oProformaInvoiceTermsAndCondition.ProformaInvoiceID, oProformaInvoiceTermsAndCondition.TermsAndCondition, nUserID, (int)eEnumDBOperation,sProformaInvoiceTermsAndConditionIDs);
        }

        #endregion

        #region Get & Exist Function
        public static IDataReader Get(TransactionContext tc, long nID)
        {
            return tc.ExecuteReader("SELECT * FROM View_ProformaInvoiceTermsAndCondition WHERE ProformaInvoiceTermsAndConditionID=%n", nID);
        }

        public static IDataReader Gets(TransactionContext tc, int id)
        {
            return tc.ExecuteReader("SELECT * FROM View_ProformaInvoiceTermsAndCondition WHERE ProformaInvoiceID = %n", id);
        }

        public static IDataReader GetsPILog(TransactionContext tc, int id)
        {
            return tc.ExecuteReader("SELECT * FROM View_ProformaInvoiceTermsAndConditionLog WHERE ProformaInvoiceLogID = %n", id);
        }
        //GetsPILog

        public static IDataReader Gets(TransactionContext tc, string sSQL)
        {
            return tc.ExecuteReader(sSQL);
        }
        #endregion
    }
    
    
}
