using System;
using System.Data;
using System;
using System.Data;
using System.Data.SqlClient;
using ICS.Core.DataAccess;
using ESimSol.BusinessObjects;
using System.Data.OleDb;
using ICS.Core.Utility;

namespace ESimSol.Services.DataAccess
{
    public class ImportLCHistryDA
    {
        public ImportLCHistryDA() { }

        #region Insert Function
        public static void Insert(TransactionContext tc, ImportLCHistry oImportLCHistry, Int64 nUserId)
        {
            tc.ExecuteNonQuery("INSERT INTO ImportLCHistry(ImportLCHistryID,ImportLCID,OperationDate,CurrentStatus,PrevioustStatus,Note,DBUserID)"
                + " VALUES(%n,%n,%q,%n,%n,%s,%n)",
                oImportLCHistry.ImportLCHistryID, oImportLCHistry.ImportLCID, Global.DBDateTime, (int)oImportLCHistry.CurrentStatus, (int)oImportLCHistry.PrevioustStatus, oImportLCHistry.Note, nUserId);
        }
        #endregion

        #region Update Function
        public static void Update(TransactionContext tc, ImportLCHistry oImportLCHistry,Int64 nUserId)
        {
            tc.ExecuteNonQuery("UPDATE ImportLCHistry SET ImportLCID=%n,OperationDate=%q,CurrentStatus=%n,PrevioustStatus=%n,Note=%s,DBUserID=%n WHERE ImportLCHistryID=%n",
               oImportLCHistry.ImportLCID, Global.DBDateTime, (int)oImportLCHistry.CurrentStatus, (int)oImportLCHistry.PrevioustStatus, oImportLCHistry.Note, nUserId, oImportLCHistry.ImportLCHistryID);
        }
        public static void UpdateByImportLCID(TransactionContext tc, ImportLCHistry oImportLCHistry, Int64 nUserId)
        {
            tc.ExecuteNonQuery("UPDATE ImportLCHistry SET OperationDate=%q,CurrentStatus=%n,Note=%s,DBUserID=%n  where ImportLCID=%n and CurrentStatus=%n and ImportLCHistryID=(select MAX(ImportLCHistryID) from ImportLCHistry where ImportLCID=%n and CurrentStatus=%n)",
                Global.DBDateTime, oImportLCHistry.CurrentStatus, oImportLCHistry.Note, nUserId, oImportLCHistry.ImportLCID, (int)oImportLCHistry.PrevioustStatus, oImportLCHistry.ImportLCID, (int)oImportLCHistry.PrevioustStatus);
        }
        #endregion

        #region Delete Function
        public static void Delete(TransactionContext tc, int nID)
        {
            tc.ExecuteNonQuery("DELETE FROM ImportLCHistry WHERE ImportLCHistryID=%n", nID);
        }
        public static void DeleteWithInvoice(TransactionContext tc, int nID)
        {
            tc.ExecuteNonQuery("DELETE FROM ImportLCHistry WHERE ImportLCID=%n", nID);
        }
        #endregion
        #region Generation Function
        public static int GetNewID(TransactionContext tc)
        {
            return tc.GenerateID("ImportLCHistry", "ImportLCHistryID");
        }
        #endregion


        #region Get & Exist Function
        public static IDataReader Get(TransactionContext tc, int nID)
        {
            return tc.ExecuteReader("SELECT * FROM ImportLCHistry WHERE ImportLCHistryID=%n", nID);
        }
        public static IDataReader Get(TransactionContext tc, int nImportLCID, EnumLCCurrentStatus eEvent)
        {
            return tc.ExecuteReader("SELECT * FROM ImportLCHistry WHERE ImportLCID=%n AND CurrentStatus=%n", nImportLCID, (int)eEvent);
        }
    
        public static IDataReader Gets(TransactionContext tc, int nInvoiceID)
        {
            return tc.ExecuteReader("SELECT * FROM ImportLCHistry WHERE ImportLCID=%n", nInvoiceID);
        }
    
        public static IDataReader Gets(TransactionContext tc, int nImportLCID, string eStatus)
        {
            return tc.ExecuteReader("SELECT * FROM ImportLCHistry WHERE ImportLCID=%n AND CurrentStatus in (%q)", nImportLCID, eStatus);
        }
        public static int GetImportLCStatusStatus(TransactionContext tc, int nImportLCID, EnumLCCurrentStatus eEvent)
        {
            object obj = tc.ExecuteScalar("SELECT CurrentStatus FROM ImportLCHistry WHERE ImportLCID=%n AND CurrentStatus=%n", nImportLCID, (int)eEvent);
            if (obj == null) return 0;
            int aaaa = 0;
            aaaa = Convert.ToInt32(obj);
            return aaaa;
        }
        #endregion

    }
}
