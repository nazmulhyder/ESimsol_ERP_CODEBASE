using System;
using System.Data;
using System.Data.SqlClient;
using ESimSol.BusinessObjects;
using System.Data.OleDb;
using ICS.Core.DataAccess;
using ICS.Core.Utility;

namespace ESimSol.Services.DataAccess
{
    public class DispoProductionDA
    {
        public DispoProductionDA() { }

        #region Generation Function

        #endregion

        #region Get & Exist Function

        public static IDataReader Gets(TransactionContext tc, string sSQL)
        {
            return tc.ExecuteReader("EXEC [SP_RPT_DispoProduction]" + "%s", sSQL);
        }
        public static IDataReader Get(TransactionContext tc, long nID)
        {
            return tc.ExecuteReader("SELECT * FROM View_FabricSalesContractReport WHERE FabricSalesContractDetailID=%n", nID);
        }
        public static IDataReader SetFabricExcNo(TransactionContext tc, DispoProduction oDispoProduction, Int64 nUserID)
        {
            return tc.ExecuteReader("EXEC [SP_FabricExcNo]" + "%n, %n, %s,%n,%D, %n", oDispoProduction.FabricSalesContractID, oDispoProduction.FabricSalesContractDetailID, oDispoProduction.ExeNo, oDispoProduction.ProcessType, NullHandler.GetNullValue(oDispoProduction.FabricReceiveDate), nUserID);
        }
        public static IDataReader OperationLab(TransactionContext tc, DispoProduction oDispoProduction, EnumDBOperation eEnumDBOperation, Int64 nUserID)
        {
            return tc.ExecuteReader("EXEC [SP_IUD_FSCDetailLab] " + "%n, %n, %n,%n, %n", oDispoProduction.FabricSalesContractID, oDispoProduction.FabricSalesContractDetailID, oDispoProduction.LabStatus, nUserID, (int)eEnumDBOperation);
        }
        public static IDataReader CreateLab(TransactionContext tc, DispoProduction oDispoProduction, EnumDBOperation eEnumDBOperation, long nUserID)
        {
            return tc.ExecuteReader("EXEC [SP_IUD_FSCLabMapping] " + "%s, %s, %n, %n", oDispoProduction.FabricSalesContractID, oDispoProduction.Params, nUserID, (int)eEnumDBOperation);
        }

        public static IDataReader UpdateMail(TransactionContext tc, DispoProduction oDispoProduction, Int64 nUserID)
        {
            tc.ExecuteNonQuery("UPDATE FabricRnD SET ForwardBy= %n, ForwardDate = %D WHERE FSCDID IN ( " + oDispoProduction.Params + " )", nUserID, DateTime.Now);
            return tc.ExecuteReader("SELECT * FROM View_FabricSalesContractReport WHERE FabricSalesContractDetailID IN ( " + oDispoProduction.Params + " )");
        }
        #endregion


    }
}