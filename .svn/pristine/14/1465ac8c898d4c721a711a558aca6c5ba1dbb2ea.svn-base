using System;
using System.Data;
using System.Data.SqlClient;
using ESimSol.BusinessObjects;
using System.Data.OleDb;
using ICS.Core.DataAccess;
using ICS.Core.Utility;

namespace ESimSol.Services.DataAccess
{
    public class ExportPIPrintSetupDA
    {
        public ExportPIPrintSetupDA() { }

        #region Insert Update Delete Function
        public static IDataReader InsertUpdate(TransactionContext tc, ExportPIPrintSetup oExportPIPrintSetup, EnumDBOperation eEnumDBOperation, Int64 nUserId)
        {
            return tc.ExecuteReader("EXEC [SP_IUD_ExportPIPrintSetup]"
                                    + "%n, %s, %d, %s, %s, %s, %s, %s, %s, %s, %s, %s, %s, %s, %s, %n,%b,%n,%n,%n,%s,%n, %n, %n",
                                     oExportPIPrintSetup.ExportPIPrintSetupID, oExportPIPrintSetup.SetupNo, oExportPIPrintSetup.Date, oExportPIPrintSetup.Note, oExportPIPrintSetup.Preface, oExportPIPrintSetup.TermsOfPayment, oExportPIPrintSetup.PartShipment, oExportPIPrintSetup.ShipmentBy, oExportPIPrintSetup.PlaceOfShipment, oExportPIPrintSetup.PlaceOfDelivery, oExportPIPrintSetup.Delivery, oExportPIPrintSetup.RequiredPaper, oExportPIPrintSetup.OtherTerms, oExportPIPrintSetup.AcceptanceBy, oExportPIPrintSetup.For, oExportPIPrintSetup.HeaderType, oExportPIPrintSetup.Activity, oExportPIPrintSetup.BUID, oExportPIPrintSetup.BaseCurrencyID, oExportPIPrintSetup.ValidityDays, oExportPIPrintSetup.BINNo, oExportPIPrintSetup.PrintNo, nUserId, (int)eEnumDBOperation);
        }

        public static void Delete(TransactionContext tc, ExportPIPrintSetup oExportPIPrintSetup, EnumDBOperation eEnumDBOperation, Int64 nUserId)
        {
            tc.ExecuteNonQuery("EXEC [SP_IUD_ExportPIPrintSetup]"
                                    + "%n, %s, %d, %s, %s, %s, %s, %s, %s, %s, %s, %s, %s, %s, %s, %n,%b,%n,%n,%n,%s,%n, %n, %n",
                                     oExportPIPrintSetup.ExportPIPrintSetupID, oExportPIPrintSetup.SetupNo, oExportPIPrintSetup.Date, oExportPIPrintSetup.Note, oExportPIPrintSetup.Preface, oExportPIPrintSetup.TermsOfPayment, oExportPIPrintSetup.PartShipment, oExportPIPrintSetup.ShipmentBy, oExportPIPrintSetup.PlaceOfShipment, oExportPIPrintSetup.PlaceOfDelivery, oExportPIPrintSetup.Delivery, oExportPIPrintSetup.RequiredPaper, oExportPIPrintSetup.OtherTerms, oExportPIPrintSetup.AcceptanceBy, oExportPIPrintSetup.For, oExportPIPrintSetup.HeaderType, oExportPIPrintSetup.Activity, oExportPIPrintSetup.BUID, oExportPIPrintSetup.BaseCurrencyID, oExportPIPrintSetup.ValidityDays, oExportPIPrintSetup.BINNo, oExportPIPrintSetup.PrintNo, nUserId, (int)eEnumDBOperation);
        }

        public static void ActivatePIPrintSetup(TransactionContext tc, ExportPIPrintSetup oExportPIPrintSetup)
        {
            tc.ExecuteNonQuery("Update ExportPIPrintSetup Set Activity=0 WHERE Activity=1 and BUID = %n", oExportPIPrintSetup.BUID);//all are in active for specific Business Unit
            tc.ExecuteNonQuery("Update ExportPIPrintSetup Set Activity=~Activity  Where  BUID = %n and ExportPIPrintSetupID=%n", oExportPIPrintSetup.BUID, oExportPIPrintSetup.ExportPIPrintSetupID);//Active just specific item
        }
        #endregion

        #region Get & Exist Function
        public static IDataReader Get(TransactionContext tc, long nID)
        {
            return tc.ExecuteReader("SELECT * FROM ExportPIPrintSetup WHERE ExportPIPrintSetupID=%n", nID);
        }

        public static IDataReader Get(TransactionContext tc, bool bActivity , int BUID)
        {
            return tc.ExecuteReader("SELECT * FROM ExportPIPrintSetup WHERE Activity=%b AND BUID = %n", bActivity, BUID);
        }

        public static IDataReader Get(TransactionContext tc, string sSetupNo)
        {
            return tc.ExecuteReader("SELECT * FROM ExportPIPrintSetup WHERE SetupNo=%n", sSetupNo);
        }
        
        public static IDataReader Gets(TransactionContext tc)
        {
            return tc.ExecuteReader("SELECT * FROM ExportPIPrintSetup");
        }

        public static IDataReader BUWiseGets(TransactionContext tc, int BUID)
        {
            return tc.ExecuteReader("SELECT * FROM ExportPIPrintSetup WHERE BUID = %n", BUID);
        }
        
        public static IDataReader Gets(TransactionContext tc, string sSQL)
        {
            return tc.ExecuteReader(sSQL);
        }
        #endregion
    }
}
