using System;
using System.Data;
using System.Data.SqlClient;
using ICS.Core.DataAccess;
using ESimSol.BusinessObjects;
using System.Data.OleDb;
using ICS.Core.Utility;
namespace ESimSol.Services.DataAccess
{

    public class SampleInvoiceSetupDA
    {
        public SampleInvoiceSetupDA() { }

        #region Insert Update Delete Function
        public static IDataReader InsertUpdate(TransactionContext tc, SampleInvoiceSetup oSampleInvoiceSetup, EnumDBOperation eEnumDBOperation, Int64 nUserId)
        {
            return tc.ExecuteReader("EXEC[SP_IUD_SampleInvoiceSetup]"
                                    + "%n,%n, %s,%s,%s,%s, %n,%b,%n,%b, %n,%n ",
                                    oSampleInvoiceSetup.SampleInvoiceSetupID, oSampleInvoiceSetup.InvoiceType, oSampleInvoiceSetup.Code, 
                                    oSampleInvoiceSetup.Name,                 oSampleInvoiceSetup.ShortName,    oSampleInvoiceSetup.PrintName, 
                                    oSampleInvoiceSetup.PrintNo,              oSampleInvoiceSetup.Activity,     oSampleInvoiceSetup.BUID, 
                                    oSampleInvoiceSetup.IsRateChange,           nUserId,            (int)eEnumDBOperation);
        }
        public static void Delete(TransactionContext tc, SampleInvoiceSetup oSampleInvoiceSetup, EnumDBOperation eEnumDBOperation, Int64 nUserId)
        {
            tc.ExecuteNonQuery("EXEC[SP_IUD_SampleInvoiceSetup]"
                                   + "%n,%n, %s,%s,%s,%s, %n,%b,%n,%b, %n,%n ",
                                    oSampleInvoiceSetup.SampleInvoiceSetupID, oSampleInvoiceSetup.InvoiceType, oSampleInvoiceSetup.Code,
                                    oSampleInvoiceSetup.Name, oSampleInvoiceSetup.ShortName, oSampleInvoiceSetup.PrintName,
                                    oSampleInvoiceSetup.PrintNo, oSampleInvoiceSetup.Activity, oSampleInvoiceSetup.BUID,
                                    oSampleInvoiceSetup.IsRateChange, nUserId, (int)eEnumDBOperation);
        }
        #endregion

        #region Get & Exist Function
        public static IDataReader Get(TransactionContext tc, long nID)
        {
            return tc.ExecuteReader("SELECT * FROM View_SampleInvoiceSetup WHERE SampleInvoiceSetupID=%n", nID);
        }
        public static IDataReader GetByType(TransactionContext tc, int nInvoiceType, int nBUID)
        {
            return tc.ExecuteReader("SELECT * FROM View_SampleInvoiceSetup WHERE  Activity=1 and InvoiceType=%n and BUID=%n", nInvoiceType, nBUID);
        }
        public static IDataReader GetByBU(TransactionContext tc, int nBUID)
        {
            return tc.ExecuteReader("SELECT * FROM View_SampleInvoiceSetup WHERE  Activity=1 and BUID=%n",  nBUID);
        }
        public static IDataReader Gets(TransactionContext tc)
        {
            return tc.ExecuteReader("SELECT * FROM View_SampleInvoiceSetup");
        }
        public static IDataReader Gets(TransactionContext tc, int nBUID)
        {
            return tc.ExecuteReader("SELECT * FROM View_SampleInvoiceSetup WHERE BUID=%n", nBUID);
        }
        public static IDataReader Gets(TransactionContext tc, string sSQL)
        {
            return tc.ExecuteReader(sSQL);
        }
        public static IDataReader Gets(string sSQL, TransactionContext tc)
        {
            return tc.ExecuteReader(sSQL);
        }
        public static IDataReader Activate(TransactionContext tc, SampleInvoiceSetup oSampleInvoiceSetup)
        {
            string sSQL1 = SQLParser.MakeSQL("Update SampleInvoiceSetup Set Activity=~Activity WHERE SampleInvoiceSetupID=%n", oSampleInvoiceSetup.SampleInvoiceSetupID);
            tc.ExecuteNonQuery(sSQL1);
            return tc.ExecuteReader("SELECT * FROM View_SampleInvoiceSetup WHERE SampleInvoiceSetupID=%n", oSampleInvoiceSetup.SampleInvoiceSetupID);

        }
        #endregion
    }
}