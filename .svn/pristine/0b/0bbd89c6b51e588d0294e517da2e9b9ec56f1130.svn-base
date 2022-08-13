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
    public class FNOrderFabricTransferDA
    {
        #region Insert Update Delete Function
        public static IDataReader InsertUpdate(TransactionContext tc, FNOrderFabricTransfer oFNOrderFabricTransfer, EnumDBOperation eEnumDBOperation, Int64 nUserID)
        {
             return tc.ExecuteReader("EXEC [SP_IUD_FNOrderFabricTransfer]"
                                   + "%n,%n,%n,%n,%n,%n,%n,%n",
                                   oFNOrderFabricTransfer.FNOrderFabricTransferID,
                                   oFNOrderFabricTransfer.FSCDID_From,
                                   oFNOrderFabricTransfer.FSCDID_To,
                                   oFNOrderFabricTransfer.FNOrderFabricReceiveID_From,
                                   oFNOrderFabricTransfer.FNOrderFabricReceiveID_To,
                                   oFNOrderFabricTransfer.Qty,
                                   nUserID,
                                   (int)eEnumDBOperation);
        }

        public static void Delete(TransactionContext tc, FNOrderFabricTransfer oFNOrderFabricTransfer, EnumDBOperation eEnumDBOperation, Int64 nUserID)
        {
            tc.ExecuteNonQuery("EXEC [SP_IUD_FNOrderFabricTransfer]"
                                   + "%n,%n,%n,%n,%n,%n,%n,%n",
                                   oFNOrderFabricTransfer.FNOrderFabricTransferID,
                                   oFNOrderFabricTransfer.FSCDID_From,
                                   oFNOrderFabricTransfer.FSCDID_To,
                                   oFNOrderFabricTransfer.FNOrderFabricReceiveID_From,
                                   oFNOrderFabricTransfer.FNOrderFabricReceiveID_To,
                                   oFNOrderFabricTransfer.Qty,
                                   nUserID,
                                   (int)eEnumDBOperation);
        }

        #endregion

        #region Get & Exist Function
        public static IDataReader Get(TransactionContext tc, long nID)
        {
            return tc.ExecuteReader("SELECT * FROM View_FNOrderFabricTransfer WHERE FNOrderFabricTransferID=%n", nID);
        }
        public static IDataReader Gets(TransactionContext tc)
        {
            return tc.ExecuteReader("SELECT * View_FROM FNOrderFabricTransfer");
        }
        public static IDataReader Gets(TransactionContext tc, string sSQL)
        {
            return tc.ExecuteReader(sSQL);
        }

        #endregion
    }

}
