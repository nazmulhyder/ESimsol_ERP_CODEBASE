using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using ESimSol.BusinessObjects;
using System.Data.OleDb;
using ICS.Core.DataAccess;
using ICS.Core.Utility;

namespace ESimSol.Services.DataAccess
{
    class SP_GeneralLedgerDA
    {
        public SP_GeneralLedgerDA() { }

        #region Inseret Update Delete
        public static IDataReader InsertUpdate(TransactionContext tc, SP_GeneralLedger oSP_GeneralLedger, Int64 nUserID, EnumDBOperation eEnumDBOperation)
        {
            return tc.ExecuteReader("Not Required");
        }

        public static void Delete(TransactionContext tc, SP_GeneralLedger oSP_GeneralLedger, EnumDBOperation eEnumDBOperation, Int64 nUserID)
        {
            tc.ExecuteNonQuery("Not Required"); ;
        }

        #endregion

        #region Get & Exist Function
        public static IDataReader GetsGeneralLedger(TransactionContext tc, int nAccountHeadId, DateTime dstartDate, DateTime dendDate, int nCurrencyID, bool bIsApproved, string BUIDs)
        {
            //return tc.ExecuteReader("EXECUTE [SP_GeneralLedger] %n, %d, %d, %n, %b, %n", nAccountHeadId, dstartDate, dendDate, nCurrencyID, bIsApproved, nBUID);
            return tc.ExecuteReader("EXECUTE [SP_GeneralLedger2] %n, %d, %d, %n, %b, %s", nAccountHeadId, dstartDate, dendDate, nCurrencyID, bIsApproved, BUIDs); //For Test
            
        }
        public static IDataReader ProcessGeneralLedger(TransactionContext tc, int nAccountHeadId, DateTime dstartDate, DateTime dendDate, int nCurrencyID, bool bIsApproved, string sSQL)
        {
            return tc.ExecuteReader("EXECUTE [SP_GeneralLedger] %n, %d, %d, %n, %b, %s", nAccountHeadId, dstartDate, dendDate, nCurrencyID, bIsApproved, sSQL);
        }
        public static IDataReader GetsWithConfig(TransactionContext tc, int nAccountHeadId, DateTime dstartDate, DateTime dendDate, int nCurrencyID, bool bIsApproved)
        {
            return tc.ExecuteReader("EXECUTE [SP_GeneralLedgerWithConfig] %n, %d, %d, %n, %b", nAccountHeadId, dstartDate, dendDate, nCurrencyID, bIsApproved);
        }
        public static IDataReader Gets(TransactionContext tc, string sSQL)
        {
            return tc.ExecuteReader(sSQL);
        }
        private static SP_GeneralLedger MapObject_Report(NullHandler oReader)
        {
            SP_GeneralLedger oSP_GeneralLedger = new SP_GeneralLedger();
           // oSP_GeneralLedger.CCTID = oReader.GetInt32("CCTID");
            oSP_GeneralLedger.AccountHeadName = oReader.GetString("CostCenterName");
            oSP_GeneralLedger.DebitAmount_BC = oReader.GetDouble("Dr_Amount");
            oSP_GeneralLedger.CreditAmount_BC = oReader.GetDouble("CR_Amount");
            oSP_GeneralLedger.OpenningBalance = oReader.GetDouble("OpeningBalance");
            oSP_GeneralLedger.CurrentBalance = oReader.GetDouble("ClosingBalance");
            oSP_GeneralLedger.IsDebit_OpeningBalance = oReader.GetBoolean("IsDebit_Opening");
            oSP_GeneralLedger.IsDebit = oReader.GetBoolean("IsDebit_Closing");

           return oSP_GeneralLedger;
        }
        private static SP_GeneralLedger CreateObject_report(NullHandler oReader)
        {
            SP_GeneralLedger oSP_GeneralLedger = new SP_GeneralLedger();
            oSP_GeneralLedger = MapObject_Report(oReader);
            return oSP_GeneralLedger;
        }
        private static List<SP_GeneralLedger> CreateObjects_report(IDataReader oReader)
        {
            List<SP_GeneralLedger> oSP_GeneralLedger = new List<SP_GeneralLedger>();
            NullHandler oHandler = new NullHandler(oReader);
            while (oReader.Read())
            {
                SP_GeneralLedger oItem = CreateObject_report(oHandler);
                oSP_GeneralLedger.Add(oItem);
            }
            return oSP_GeneralLedger;
        }

        public static List<SP_GeneralLedger> GetsForReport(TransactionContext tc, SP_GeneralLedger oGL)
        {
            List<SP_GeneralLedger> oGLs = new List<SP_GeneralLedger>();
            IDataReader reader = null;
            reader = tc.ExecuteReader("EXEC [SP_GeneralLedger_CostBreakUp]" + " %n, %d, %d, %n ", oGL.AccountHeadID, oGL.StartDate, oGL.EndDate, oGL.CompanyID);
            oGLs = CreateObjects_report(reader);
            reader.Close();
            return oGLs;
        }
       
        #endregion
    }
}
