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
    public class FabricExpDeliveryDA
    {
        #region Insert Update Delete Function
        public static IDataReader InsertUpdate(TransactionContext tc, FabricExpDelivery oFabricExpDelivery, EnumDBOperation eEnumDBOperation, Int64 nUserID)
        {
            return tc.ExecuteReader("EXEC [SP_IUD_FabricExpDelivery]"
                                   + "%n,%n,%n,%d,%n,%n",
                                   oFabricExpDelivery.FabricExpDeliveryID,
                                   oFabricExpDelivery.FSCDID,
                                   oFabricExpDelivery.Qty,
                                   oFabricExpDelivery.DeliveryDate,
                                   nUserID,
                                   (int)eEnumDBOperation);
        }

        public static void Delete(TransactionContext tc, FabricExpDelivery oFabricExpDelivery, EnumDBOperation eEnumDBOperation, Int64 nUserID)
        {
            tc.ExecuteNonQuery("EXEC [SP_IUD_FabricExpDelivery]"
                                   + "%n,%n,%n,%d,%n,%n",
                                   oFabricExpDelivery.FabricExpDeliveryID,
                                   oFabricExpDelivery.FSCDID,
                                   oFabricExpDelivery.Qty,
                                   oFabricExpDelivery.DeliveryDate,
                                   nUserID,
                                   (int)eEnumDBOperation);
        }

        #endregion

        #region Get & Exist Function
        public static IDataReader Get(TransactionContext tc, long nID)
        {
            return tc.ExecuteReader("SELECT * FROM View_FabricExpDelivery WHERE FabricExpDeliveryID=%n", nID);
        }
        public static IDataReader Gets(TransactionContext tc)
        {
            return tc.ExecuteReader("SELECT * FROM View_FabricExpDelivery");
        }
        public static IDataReader Gets(TransactionContext tc, string sSQL)
        {
            return tc.ExecuteReader(sSQL);
        }

        #endregion
    }

}
