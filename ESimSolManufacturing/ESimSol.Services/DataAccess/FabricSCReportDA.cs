using System;
using System.Data;
using System.Data.SqlClient;
using ESimSol.BusinessObjects;
using System.Data.OleDb;
using ICS.Core.DataAccess;
using ICS.Core.Utility;

namespace ESimSol.Services.DataAccess
{
    public class FabricSCReportDA
    {
        public FabricSCReportDA() { }

        #region Generation Function

        #endregion

        #region Get & Exist Function
     
        public static IDataReader Gets(TransactionContext tc, string sSQL)
        {
            return tc.ExecuteReader(sSQL);
        }
        public static IDataReader Get(TransactionContext tc, long nID)
        {
            return tc.ExecuteReader("SELECT * FROM View_FabricSalesContractReport WHERE FabricSalesContractDetailID=%n", nID);
        }
        public static IDataReader SetFabricExcNo(TransactionContext tc, FabricSCReport oFabricSCReport, Int64 nUserID)
        {
            return tc.ExecuteReader("EXEC [SP_FabricExcNo]" + "%n, %n, %s,%n,%D, %n", oFabricSCReport.FabricSalesContractID, oFabricSCReport.FabricSalesContractDetailID, oFabricSCReport.ExeNo, oFabricSCReport.ProcessType, NullHandler.GetNullValue(oFabricSCReport.FabricReceiveDate), nUserID);
        }
        public static IDataReader OperationLab(TransactionContext tc, FabricSCReport oFabricSCReport, EnumDBOperation eEnumDBOperation, Int64 nUserID)
        {
            return tc.ExecuteReader("EXEC [SP_IUD_FSCDetailLab] " + "%n, %n, %n,%n, %n", oFabricSCReport.FabricSalesContractID, oFabricSCReport.FabricSalesContractDetailID, oFabricSCReport.LabStatus, nUserID, (int)eEnumDBOperation);
        }
        public static IDataReader CreateLab(TransactionContext tc, FabricSCReport oFabricSCReport, EnumDBOperation eEnumDBOperation, long nUserID)
        {
            return tc.ExecuteReader("EXEC [SP_IUD_FSCLabMapping] " + "%s, %s, %n, %n", oFabricSCReport.FabricSalesContractID, oFabricSCReport.Params, nUserID, (int)eEnumDBOperation);
        }

        public static IDataReader UpdateMail(TransactionContext tc, FabricSCReport oFabricSCReport, Int64 nUserID)
        {
            tc.ExecuteNonQuery("UPDATE FabricRnD SET ForwardBy= %n, ForwardDate = %D WHERE FSCDID IN ( " + oFabricSCReport.Params + " )", nUserID, DateTime.Now);
            return tc.ExecuteReader("SELECT * FROM View_FabricSalesContractReport WHERE FabricSalesContractDetailID IN ( " + oFabricSCReport.Params + " )");
        }
        #endregion


    }
}