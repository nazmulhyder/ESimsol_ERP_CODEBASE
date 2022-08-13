using System;
using System.Data;
using System.Data.SqlClient;
using ICS.Core.DataAccess;
using ESimSol.BusinessObjects;
using System.Data.OleDb;
using ICS.Core.Utility;
namespace ESimSol.Services.DataAccess
{
    public class ArchiveDataDA
    {
        #region Insert Update Delete Function

        public static IDataReader InsertUpdate(TransactionContext tc, ArchiveData oArchiveData, EnumDBOperation eEnumDBOperation, Int64 nUserID)
        {
            return tc.ExecuteReader("EXEC [SP_IUD_ArchiveData]"
                                   + "%n,%s,%d,%n,%n,%n,%s,%n,%n",
                                   oArchiveData.ArchiveDataID,oArchiveData.ArchiveNo,oArchiveData.ArchiveDate, oArchiveData.ArchiveMonthID, oArchiveData.ArchiveYearID, oArchiveData.ArchiveStatus, oArchiveData.Remarks, nUserID, (int)eEnumDBOperation);
        }

        public static void Delete(TransactionContext tc, ArchiveData oArchiveData, EnumDBOperation eEnumDBOperation, Int64 nUserID)
        {
            tc.ExecuteNonQuery("EXEC [SP_IUD_ArchiveData]"
                                    + "%n,%s,%d,%n,%n,%n,%s,%n,%n",
                                    oArchiveData.ArchiveDataID,oArchiveData.ArchiveNo,oArchiveData.ArchiveDate, oArchiveData.ArchiveMonthID, oArchiveData.ArchiveYearID, oArchiveData.ArchiveStatus, oArchiveData.Remarks, nUserID, (int)eEnumDBOperation);
        }


        #endregion

        #region Get & Exist Function
        public static IDataReader Get(TransactionContext tc, long nID)
        {
            return tc.ExecuteReader("SELECT * FROM View_ArchiveData WHERE ArchiveDataID=%n", nID);
        }
        public static IDataReader Gets(TransactionContext tc, string sSQL)
        {
            return tc.ExecuteReader(sSQL);
        }

        #endregion
    }
}
