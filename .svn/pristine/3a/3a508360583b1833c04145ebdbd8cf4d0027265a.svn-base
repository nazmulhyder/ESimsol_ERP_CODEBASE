using System;
using System;
using System.Data;
using System.Data.SqlClient;
using ICS.Core.DataAccess;
using ESimSol.BusinessObjects;
using System.Data.OleDb;
using ICS.Core.Utility;

namespace ESimSol.Services.DataAccess
{
    public class DeliveryZoneDA
    {
        #region Insert Update Delete Function
        public static IDataReader InsertUpdate(TransactionContext tc, DeliveryZone oDeliveryZone, EnumDBOperation eEnumDBOperation, int nUserID)
        {
            return tc.ExecuteReader("EXEC [SP_IUD_DeliveryZone]"
                                    + "%n, %n, %s, %n",
                                    (int)eEnumDBOperation, oDeliveryZone.DeliveryZoneID, oDeliveryZone.DeliveryZoneName, nUserID);
        }

        public static void Delete(TransactionContext tc, DeliveryZone oDeliveryZone, EnumDBOperation eEnumDBOperation, int nUserID)
        {
            tc.ExecuteNonQuery("EXEC [SP_IUD_DeliveryZone]"
                                    + "%n, %n, %s, %n",
                                    (int)eEnumDBOperation, oDeliveryZone.DeliveryZoneID, oDeliveryZone.DeliveryZoneName, nUserID);
        }
        #endregion

        #region Get & Exist Function
        public static IDataReader Gets(TransactionContext tc)
        {
            return tc.ExecuteReader("SELECT * FROM DeliveryZone");
        }
        public static IDataReader Get(TransactionContext tc, int nDeliveryZoneID)
        {
            return tc.ExecuteReader("SELECT * FROM DeliveryZone WHERE DeliveryZoneID=%n", nDeliveryZoneID);
        }

        public static IDataReader Gets(TransactionContext tc, string sSQL)
        {
            return tc.ExecuteReader(sSQL);
        }
        #endregion
    }
}