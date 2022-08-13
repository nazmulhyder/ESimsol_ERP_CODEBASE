using System;
using System.Data;
using System.Data.SqlClient;
using ICS.Core.DataAccess;
using ESimSol.BusinessObjects;
using System.Data.OleDb;
using ICS.Core.Utility;

namespace ESimSol.Services.DataAccess
{
    public class FNBatchRawMaterialDA
    {
        public FNBatchRawMaterialDA() { }

        #region Insert Update Delete Function
        public static IDataReader FabricOut(TransactionContext tc, FNBatchRawMaterial oFNBRM, Int64 nUserID)
        {

            return tc.ExecuteReader("EXEC [SP_Process_FNBatchRawMaterialOut] %n, %n, %n, %n, %n, %n", oFNBRM.FBRMID, oFNBRM.FNBatchID, oFNBRM.LotID, oFNBRM.Qty,oFNBRM.FNOrderFabricReceiveID, nUserID);

        }
        #endregion

        #region Get & Exist Function
        public static IDataReader Get(TransactionContext tc, int nFBRMID)
        {
            return tc.ExecuteReader("SELECT * FROM View_FNBatchRawMaterial WHERE FBRMID=%n", nFBRMID);
        }

        public static IDataReader Gets(TransactionContext tc, string sSQL)
        {
            return tc.ExecuteReader(sSQL);
        }


        #endregion
    }
}
