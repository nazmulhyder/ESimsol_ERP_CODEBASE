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
    public class EmployeeBatchDetailDA
    {
        #region Insert Update Delete Function

        public static IDataReader InsertUpdate(TransactionContext tc, EmployeeBatchDetail oEmployeeBatchDetail, EnumDBOperation eEnumDBOperation, Int64 nUserID, string sIDs)
        {
            return tc.ExecuteReader("EXEC [SP_IUD_EmployeeBatchDetail]"
                                   + "%n, %n, %n, %s,%s,%s,%s,%s,%s,%d,%n,%n,%n,%n,%s",
                                  oEmployeeBatchDetail.EmployeeBatchDetailID, oEmployeeBatchDetail.EmployeeBatchID, oEmployeeBatchDetail.EmployeeID, oEmployeeBatchDetail.Location, oEmployeeBatchDetail.Department, oEmployeeBatchDetail.Designation, oEmployeeBatchDetail.ShiftName, oEmployeeBatchDetail.AttendanceScheme, oEmployeeBatchDetail.SalaryScheme, oEmployeeBatchDetail.DateOfJoin, oEmployeeBatchDetail.GrossAmount, oEmployeeBatchDetail.ComplianceGross, (int)eEnumDBOperation, nUserID, sIDs);
        }

        public static void Delete(TransactionContext tc, EmployeeBatchDetail oEmployeeBatchDetail, EnumDBOperation eEnumDBOperation, Int64 nUserID, string sIDs)
        {
            tc.ExecuteNonQuery("EXEC [SP_IUD_EmployeeBatchDetail]"
                                    + "%n, %n, %n, %s,%s,%s,%s,%s,%s,%d,%n,%n,%n,%n,%s",
                                    oEmployeeBatchDetail.EmployeeBatchDetailID, oEmployeeBatchDetail.EmployeeBatchID, oEmployeeBatchDetail.EmployeeID, oEmployeeBatchDetail.Location, oEmployeeBatchDetail.Department, oEmployeeBatchDetail.Designation, oEmployeeBatchDetail.ShiftName, oEmployeeBatchDetail.AttendanceScheme, oEmployeeBatchDetail.SalaryScheme, oEmployeeBatchDetail.DateOfJoin, oEmployeeBatchDetail.GrossAmount, oEmployeeBatchDetail.ComplianceGross, (int)eEnumDBOperation, nUserID, sIDs);
        }

        #endregion

        public static IDataReader Gets(TransactionContext tc, int id)
        {
            return tc.ExecuteReader("SELECT * FROM View_EmployeeBatchDetail WHERE EmployeeBatchID = %n Order By EmployeeBatchDetailID", id);
        }

        public static IDataReader ArchiveSalaryChnage(TransactionContext tc, ArchiveSalaryStruc oArchiveSalaryStruc, Int64 nUserID)
        {
            return tc.ExecuteReader("EXEC [SP_ArchiveSalaryChnage]" + "%n, %n, %n", oArchiveSalaryStruc.ArchiveDataID, oArchiveSalaryStruc.EmployeeBatchID, nUserID);
        }
    }
}
