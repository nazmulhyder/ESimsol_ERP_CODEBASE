using System;
using System.Data;
using System.Data.SqlClient;
using ICS.Core.DataAccess;
using ESimSol.BusinessObjects;
using System.Data.OleDb;
using ICS.Core.Utility;

namespace ESimSol.Services.DataAccess
{
    public class ImportLC_RequestDA
    {
        public ImportLC_RequestDA() { }

        #region Insert Function
        public static void Insert(TransactionContext tc, ImportLC_Request ImportLC_Request)
        {
            tc.ExecuteNonQuery("INSERT INTO ImportLC_Request(ImportLC_RequestID, ImportLCID, ClauseID)"
                + " VALUES(%n,  %n,  %n)",
                ImportLC_Request.ImportLC_RequestID, ImportLC_Request.ImportLCID, ImportLC_Request.ClauseID);
        }
        #endregion

        #region Update Function
        public static void Update(TransactionContext tc, ImportLC_Request ImportLC_Request)
        {
            tc.ExecuteNonQuery("UPDATE ImportLC_Request SET ImportLCID=%n, ClauseID=%n"
                            + " WHERE ImportLC_RequestID=%n", ImportLC_Request.ImportLCID, ImportLC_Request.ClauseID, ImportLC_Request.ImportLC_RequestID);
        }
        #endregion

        #region Delete Function
        public static void DeleteByImportLC_RLC(TransactionContext tc, int nImportLCID, string sCaluseIDs)
        {
            tc.ExecuteNonQuery("Delete from ImportLC_Request where ImportLCID=%n and ClauseID not in (%q)", nImportLCID, sCaluseIDs);
        }
        #endregion

        #region Generation Function
        public static int GetNewID(TransactionContext tc)
        {
            return tc.GenerateID("ImportLC_Request", "ImportLC_RequestID");
        }
        #endregion

        #region Get & Exist Function
      
        public static IDataReader Gets(TransactionContext tc, int nImportLCID)
        {
            return tc.ExecuteReader("SELECT ImportLC_Request.*,Clause.[Text] FROM ImportLC_Request,Clause where Clause.ClauseID=ImportLC_Request.ClauseID and ImportLC_Request.ImportLCID=%n Order By [ImportLC_RequestID]", nImportLCID);
        }

        public static IDataReader GetsByImportLCID(TransactionContext tc, int nImportLCID)
        {
            return tc.ExecuteReader("SELECT * FROM ImportLC_Request WHERE ImportLCID = %n", nImportLCID);
        }

        public static IDataReader GetsBySQL(TransactionContext tc, string sSQL)
        {
            return tc.ExecuteReader(sSQL);
        }

       
        #endregion
    }
   
   
}
