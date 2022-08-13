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
    public class SampleInvoiceChargeDA
    {
        #region Insert Update Delete Function
        public static IDataReader InsertUpdate(TransactionContext tc, SampleInvoiceCharge oSampleInvoiceCharge, EnumDBOperation eEnumDBOperation, Int64 nUserID)
        {
            return tc.ExecuteReader("EXEC [SP_IUD_SampleInvoiceCharge]"
                                   + "%n,%n,%n,%s,%n,%n,%n",
                                   oSampleInvoiceCharge.SampleInvoiceChargeID,
                                   oSampleInvoiceCharge.SampleInvoiceID,
                                   oSampleInvoiceCharge.InoutType,
                                   oSampleInvoiceCharge.Name,
                                   oSampleInvoiceCharge.Amount,
                                   nUserID,
                                   (int)eEnumDBOperation);
        }

        public static void Delete(TransactionContext tc, SampleInvoiceCharge oSampleInvoiceCharge, EnumDBOperation eEnumDBOperation, Int64 nUserID)
        {
            tc.ExecuteNonQuery("EXEC [SP_IUD_SampleInvoiceCharge]"
                                   + "%n,%n,%n,%s,%n,%n,%n",
                                   oSampleInvoiceCharge.SampleInvoiceChargeID,
                                   oSampleInvoiceCharge.SampleInvoiceID,
                                   oSampleInvoiceCharge.InoutType,
                                   oSampleInvoiceCharge.Name,
                                   oSampleInvoiceCharge.Amount,
                                   nUserID,
                                   (int)eEnumDBOperation);
        }

        #endregion

        #region Get & Exist Function
        public static IDataReader Get(TransactionContext tc, long nID)
        {
            return tc.ExecuteReader("SELECT * FROM View_SampleInvoiceCharge WHERE SampleInvoiceChargeID=%n", nID);
        }
        public static IDataReader Gets(TransactionContext tc, int nSampleInvoiceID)
        {
            return tc.ExecuteReader("SELECT * FROM View_SampleInvoiceCharge Where SampleInvoiceID =%n", nSampleInvoiceID);
        }
        public static IDataReader Gets(TransactionContext tc, string sSQL)
        {
            return tc.ExecuteReader(sSQL);
        }

        #endregion
    }
}
