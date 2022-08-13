using System;
using System.Data;
using System.Data.SqlClient;
using ESimSol.BusinessObjects;
using System.Data.OleDb;
using ICS.Core.DataAccess;
using ICS.Core.Utility;

namespace ESimSol.Services.DataAccess
{
    public class MasterPIMappingDA
    {
        public MasterPIMappingDA() { }

        #region Insert Update Delete Function
        public static IDataReader InsertUpdate(TransactionContext tc, MasterPIMapping oMasterPIMapping, EnumDBOperation eEnumDBOperation, Int64 nUserID,string sIDs )
        {
            return tc.ExecuteReader("EXEC [SP_IUD_MasterPIMapping]" + "%n, %n, %n,  %n, %n, %n,%n,%s",
                                    oMasterPIMapping.MasterPIMappingID, oMasterPIMapping.MasterPIID, oMasterPIMapping.ExportPIID, oMasterPIMapping.CurrencyID,  oMasterPIMapping.Amount,  (int)eEnumDBOperation, nUserID, sIDs);
        }
        public static void Delete(TransactionContext tc, MasterPIMapping oMasterPIMapping, EnumDBOperation eEnumDBOperation, Int64 nUserID, string sIDs)
        {
            tc.ExecuteNonQuery("EXEC [SP_IUD_MasterPIMapping]" + "%n, %n, %n,  %n, %n, %n,%n,%s",
                                    oMasterPIMapping.MasterPIMappingID, oMasterPIMapping.MasterPIID, oMasterPIMapping.ExportPIID, oMasterPIMapping.CurrencyID, oMasterPIMapping.Amount, (int)eEnumDBOperation, nUserID, sIDs);
        }

        #endregion

        #region Get & Exist Function
        public static IDataReader Get(TransactionContext tc, long nID)
        {
            return tc.ExecuteReader("SELECT * FROM View_MasterPIMapping WHERE MasterPIMappingID=%n", nID);
        }

        public static IDataReader Gets(TransactionContext tc,int nExportPIID)
        {
            return tc.ExecuteReader("SELECT * FROM View_MasterPIMapping WHERE ExportPIID =%n order by MasterPIMappingID ", nExportPIID);
        }

      
        public static IDataReader Gets(TransactionContext tc)
        {
            return tc.ExecuteReader("SELECT * FROM View_MasterPIMapping");
        }

        public static IDataReader GetsByMasterPI(TransactionContext tc, int nMasterPIID)
        {
            return tc.ExecuteReader("SELECT * FROM View_MasterPIMapping WHERE MasterPIID=%n order by ExportPIID", nMasterPIID);
        }
        public static IDataReader GetsByPIAndSortByOrderSheet(TransactionContext tc, int nPIID)
        {
            return tc.ExecuteReader("SELECT * FROM View_MasterPIMapping WHERE ExportPIID=%n ORDER BY OrderSheetDetailID  ASC", nPIID);
        }
        public static IDataReader Gets(TransactionContext tc, string sSQL)
        {
            return tc.ExecuteReader(sSQL);
        }
        public static IDataReader GetsLog(TransactionContext tc, int nPIID)
        {
            return tc.ExecuteReader("SELECT * FROM [View_MasterPIMappingLog] WHERE ExportPIID=%n", nPIID);
        }
        public static IDataReader GetsLogDetail(TransactionContext tc, int nPILogID)
        {
            return tc.ExecuteReader("SELECT * FROM [View_MasterPIMappingLog] WHERE ExportPILogID=%n  Order BY MasterPIMappingLogID", nPILogID);
        }

      
        #endregion
    }
}
