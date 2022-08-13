using System;
using System.Data;
using System.Data.SqlClient;
using ICS.Core.DataAccess;
using ESimSol.BusinessObjects;
using System.Data.OleDb;
using ICS.Core.Utility;

namespace ESimSol.Services.DataAccess
{
    public class ImportLC_ClauseDA
    {
        public ImportLC_ClauseDA() { }

        #region Insert Function

        public static IDataReader InsertUpdate(TransactionContext tc, ImportLC_Clause oImportLC_Clause, EnumDBOperation eEnumDBOperation, Int64 nUserID, string sImportLC_ClauseID)
        {
            return tc.ExecuteReader("EXEC [SP_IUD_ImportLC_Clause]" + "%n, %n, %n, %s, %s, %n, %n,%n",
                oImportLC_Clause.ImportLC_ClauseID, oImportLC_Clause.ImportLCID, oImportLC_Clause.ImportLCLogID, oImportLC_Clause.Clause, oImportLC_Clause.Caption,oImportLC_Clause.LCCurrentStatusInt, nUserID, (int)eEnumDBOperation);
        }
    
        public static void Delete(TransactionContext tc, ImportLC_Clause oImportLC_Clause, EnumDBOperation eEnumDBOperation, Int64 nUserID, string sImportLC_ClauseID)
        {
            tc.ExecuteNonQuery("EXEC [SP_IUD_ImportLC_Clause]" + "%n, %n, %n, %s, %s, %n, %n,%n",
                  oImportLC_Clause.ImportLC_ClauseID, oImportLC_Clause.ImportLCID, oImportLC_Clause.ImportLCLogID, oImportLC_Clause.Clause, oImportLC_Clause.Caption, oImportLC_Clause.LCCurrentStatusInt, nUserID, (int)eEnumDBOperation);
        }

        public static void DeleteClause(TransactionContext tc, int id, int nImportLCLogID, int nLCCurrentStatusInt)
        {
            tc.ExecuteNonQuery("Delete from ImportLC_Clause where ImportLCID=%n and ImportLCLogID=%n and LCCurrentStatus=%n",id, nImportLCLogID, nLCCurrentStatusInt);
        }

        public static void Insert(TransactionContext tc, ImportLC_Clause ImportLC_Clause)
        {
            tc.ExecuteNonQuery("INSERT INTO ImportLC_Clause(ImportLC_ClauseID, ImportLCID, ClauseID)"
                + " VALUES(%n,  %n,  %n)",
                ImportLC_Clause.ImportLC_ClauseID, ImportLC_Clause.ImportLCID, ImportLC_Clause.ClauseID);
        }
        #endregion

        #region Update Function
        public static void Update(TransactionContext tc, ImportLC_Clause ImportLC_Clause)
        {
            tc.ExecuteNonQuery("UPDATE ImportLC_Clause SET ImportLCID=%n, ClauseID=%n"
                            + " WHERE ImportLC_ClauseID=%n", ImportLC_Clause.ImportLCID, ImportLC_Clause.ClauseID, ImportLC_Clause.ImportLC_ClauseID);
        }
        #endregion

        #region Delete Function
        public static void DeleteByImportLC_RLC(TransactionContext tc, int nImportLCID, string sCaluseIDs)
        {
            tc.ExecuteNonQuery("Delete from ImportLC_Clause where ImportLCID=%n and ClauseID not in (%q)", nImportLCID, sCaluseIDs);
        }
        #endregion

        #region Generation Function
        public static int GetNewID(TransactionContext tc)
        {
            return tc.GenerateID("ImportLC_Clause", "ImportLC_ClauseID");
        }
        #endregion

        #region Get & Exist Function

        public static IDataReader Gets(TransactionContext tc, int nImportLCID, int nImportLCLogID, int nLCCurrentStatus)
        {
            return tc.ExecuteReader("SELECT * FROM ImportLC_Clause WHERE ImportLCID = %n and ImportLCLogID=%n and LCCurrentStatus=%n", nImportLCID, nImportLCLogID, nLCCurrentStatus);
        }

        public static IDataReader GetsByImportLCID(TransactionContext tc, int nImportLCID)
        {
            return tc.ExecuteReader("SELECT * FROM ImportLC_Clause WHERE ImportLCID = %n", nImportLCID);
        }

        public static IDataReader GetsBySQL(TransactionContext tc, string sSQL)
        {
            return tc.ExecuteReader(sSQL);
        }

       
        #endregion
    }
   
   
}
