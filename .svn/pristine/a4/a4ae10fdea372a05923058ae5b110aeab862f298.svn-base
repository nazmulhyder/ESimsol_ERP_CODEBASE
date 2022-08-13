using System;
using System.Data;
using System.Data.SqlClient;
using ICS.Core.DataAccess;
using ESimSol.BusinessObjects;
using System.Data.OleDb;
using ICS.Core.Utility;

namespace ESimSol.Services.DataAccess
{
    public class ArchiveSalaryStrucDA
    {
        public ArchiveSalaryStrucDA() { }

        #region Insert Update Delete Function
        public static IDataReader InsertUpdate(TransactionContext tc, ArchiveSalaryStruc oArchiveSalaryStruc, EnumDBOperation eEnumDBOperation, Int64 nUserID)
        {
            return tc.ExecuteReader("EXEC [SP_IUD_ArchiveSalaryStruc]" + "%n, %s, %s, %s, %s, %n, %n, %n");
        }

        public static void Delete(TransactionContext tc, ArchiveSalaryStruc oArchiveSalaryStruc, EnumDBOperation eEnumDBOperation, Int64 nUserID)
        {
            tc.ExecuteNonQuery("EXEC [SP_IUD_ArchiveSalaryStruc]" + "%n, %s, %s, %s, %s, %n, %n, %n");
        }
        public static void ArchiveSalaryChnage(TransactionContext tc, int nArchiveDataID, EmployeeBatchDetail oEmployeeBatchDetail, Int64 nUserID)
        {
            tc.ExecuteNonQuery("EXEC [SP_ArchiveSalaryChnage]" + "%n, %n, %n, %n, %n, %n", oEmployeeBatchDetail.EmployeeID, nArchiveDataID, oEmployeeBatchDetail.EmployeeBatchDetailID, oEmployeeBatchDetail.GrossAmount, oEmployeeBatchDetail.ComplianceGross, nUserID);
        }
        #endregion

        #region Get & Exist Function
        public static IDataReader Get(TransactionContext tc, long nID)
        {
            return tc.ExecuteReader("SELECT * FROM View_ArchiveSalaryStruc WHERE ArchiveSalaryStrucID=%n", nID);
        }
        public static IDataReader Gets(TransactionContext tc)
        {
            return tc.ExecuteReader("SELECT * FROM View_ArchiveSalaryStruc Order By [Name]");
        }
        public static IDataReader Gets(TransactionContext tc, string sSQL)
        {
            return tc.ExecuteReader(sSQL);
        }
        #endregion
    }  
}
