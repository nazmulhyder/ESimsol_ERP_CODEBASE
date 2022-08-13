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
    public class ExportPISignatoryDA
    {
        #region Insert Update Delete Function

        public static IDataReader InsertUpdate(TransactionContext tc, ExportPISignatory oExportPISignatory, EnumDBOperation eEnumDBOperation, Int64 nUserID)
        {
            return tc.ExecuteReader("EXEC [SP_IUD_ExportPISignatory]"
                                   + "%n,%n,%n,%n,%n,%n,%b,%n,%n",
                                   oExportPISignatory.ExportPISignatoryID, oExportPISignatory.ExportPIID, oExportPISignatory.ApprovalHeadID, oExportPISignatory.SLNo, oExportPISignatory.ReviseNo, oExportPISignatory.RequestTo, oExportPISignatory.IsApprove, nUserID, (int)eEnumDBOperation);
        }
        public static void Delete(TransactionContext tc, ExportPISignatory oExportPISignatory, EnumDBOperation eEnumDBOperation, Int64 nUserID)
        {
            tc.ExecuteNonQuery("EXEC [SP_IUD_ExportPISignatory]"
                                    + "%n,%n,%n,%n,%n,%n,%b,%n,%n",
                                    oExportPISignatory.ExportPISignatoryID, oExportPISignatory.ExportPIID, oExportPISignatory.ApprovalHeadID, oExportPISignatory.SLNo, oExportPISignatory.ReviseNo, oExportPISignatory.RequestTo, oExportPISignatory.IsApprove, nUserID, (int)eEnumDBOperation);
        }
        public static void DeleteAll(TransactionContext tc, int nExportPIID, string sExportPISignatoryID)
        {
            tc.ExecuteNonQuery("DELETE FROM ExportPISignatory WHERE isnull(ApproveBy,0)=0 and  ExportPIID=%n and ExportPISignatoryID not in (%q)", nExportPIID, sExportPISignatoryID);
        }
        #endregion

        #region Get & Exist Function
        public static IDataReader Get(TransactionContext tc, long nID)
        {
            return tc.ExecuteReader("SELECT * FROM View_ExportPISignatory WHERE ExportPISignatoryID=%n", nID);
        }
        public static IDataReader Gets(TransactionContext tc, int nExportPIID)
        {
            return tc.ExecuteReader("SELECT * FROM View_ExportPISignatory where ExportPIID=%n", nExportPIID);
        }
        public static IDataReader Gets(TransactionContext tc, string sSQL)
        {
            return tc.ExecuteReader(sSQL);
        }

        #endregion
    }

}
