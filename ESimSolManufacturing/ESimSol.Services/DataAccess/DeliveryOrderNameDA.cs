using System;
using System.Data;
using System.Data.SqlClient;
using ICS.Core.DataAccess;
using ESimSol.BusinessObjects;
using System.Data.OleDb;
using ICS.Core.Utility;

namespace ESimSol.Services.DataAccess
{
    public class DeliveryOrderNameDA
    {
        #region Insert Update Delete Function
        public static IDataReader InsertUpdate(TransactionContext tc, DeliveryOrderName oDeliveryOrderName, EnumDBOperation eEnumDBOperation, Int64 nUserID)
        {
            return tc.ExecuteReader("EXEC [SP_IUD_DeliveryOrderName]"
                                    + "%n, %s,%b, %n,%n,%b,%b,%n, %n",
                                    oDeliveryOrderName.DeliveryOrderNameID, oDeliveryOrderName.Name, oDeliveryOrderName.Activity, oDeliveryOrderName.Sequence, oDeliveryOrderName.OrderType, oDeliveryOrderName.IsFoc,oDeliveryOrderName.IsGrey, (int)eEnumDBOperation, nUserID);
        }

        public static void Delete(TransactionContext tc, DeliveryOrderName oDeliveryOrderName, EnumDBOperation eEnumDBOperation, Int64 nUserID)
        {
            tc.ExecuteNonQuery("EXEC [SP_IUD_DeliveryOrderName]"
                                    + "%n, %s,%b, %n,%n,%b,%b,%n, %n",
                                      oDeliveryOrderName.DeliveryOrderNameID, oDeliveryOrderName.Name, oDeliveryOrderName.Activity, oDeliveryOrderName.Sequence,oDeliveryOrderName.OrderType,oDeliveryOrderName.IsFoc,oDeliveryOrderName.IsGrey, (int)eEnumDBOperation, nUserID);
        }

        #endregion

        #region Get & Exist Function
        public static IDataReader Gets(TransactionContext tc)
        {
            return tc.ExecuteReader("SELECT * FROM DeliveryOrderName");
        }
        public static IDataReader GetsActivity(TransactionContext tc, bool bActivity)
        {
            return tc.ExecuteReader("SELECT * FROM DeliveryOrderName where Activity=%b", bActivity);
        }
        public static IDataReader Get(TransactionContext tc, long nDeliveryOrderNameID)
        {
            return tc.ExecuteReader("SELECT * FROM DeliveryOrderName WHERE DeliveryOrderNameID=%n", nDeliveryOrderNameID);
        }
        #endregion
    }
}
