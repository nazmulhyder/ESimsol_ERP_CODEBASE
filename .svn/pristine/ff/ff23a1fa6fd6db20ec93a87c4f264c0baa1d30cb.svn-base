using System;
using System.Data;
using System.Data.SqlClient;
using ESimSol.BusinessObjects;
using System.Data.OleDb;
using ICS.Core.DataAccess;
using ICS.Core.Utility;

namespace ESimSol.Services.DataAccess
{
    public class DeliverySetupDA
    {
        public DeliverySetupDA() { }

        #region Insert Update Delete Function
        public static IDataReader InsertUpdate(TransactionContext tc, DeliverySetup oDeliverySetup, EnumDBOperation eEnumDBOperation, Int64 nUserId)
        {
            return tc.ExecuteReader("EXEC [SP_IUD_DeliverySetup]"
                                    + "%n, %s, %n, %n, %n, %s, %s, %n, %q, %n, %n, %n, %n",
                                     oDeliverySetup.DeliverySetupID,
                                     oDeliverySetup.PrintHeader, 
                                     oDeliverySetup.OrderPrintNo, 
                                     oDeliverySetup.ChallanPrintNo,
                                     oDeliverySetup.BUID,
                                     oDeliverySetup.DCPrefix,
                                     oDeliverySetup.GPPrefix,
                                     (int)oDeliverySetup.PrintFormatType,
                                     oDeliverySetup.ImagePad,
                                     oDeliverySetup.OverDCQty,
                                     oDeliverySetup.OverDeliverPercentage,
                                     nUserId, 
                                    (int)eEnumDBOperation);
        }
        public static void Delete(TransactionContext tc, DeliverySetup oDeliverySetup, EnumDBOperation eEnumDBOperation, Int64 nUserId)
        {
            tc.ExecuteNonQuery("EXEC [SP_IUD_DeliverySetup]"
                                    + "%n, %s, %n, %n, %n, %s, %s, %n, %q, %n, %n, %n, %n",
                                     oDeliverySetup.DeliverySetupID,
                                     oDeliverySetup.PrintHeader,
                                     oDeliverySetup.OrderPrintNo,
                                     oDeliverySetup.ChallanPrintNo,
                                     oDeliverySetup.BUID,
                                     oDeliverySetup.DCPrefix,
                                     oDeliverySetup.GPPrefix,
                                     (int)oDeliverySetup.PrintFormatType,
                                     oDeliverySetup.ImagePad,
                                     oDeliverySetup.OverDCQty,
                                     oDeliverySetup.OverDeliverPercentage,
                                     nUserId,
                                    (int)eEnumDBOperation);
        }
        #endregion

        public static void UpdateImagePad(TransactionContext tc, DeliverySetup oDeliverySetup, Int64 nUserID)
        {
            SqlParameter Photo = new SqlParameter();
            Photo.SqlDbType = SqlDbType.Image;
            Photo.ParameterName = "Photopic";
            Photo.Value = oDeliverySetup.ImagePad;

            string sSQL = SQLParser.MakeSQL("UPDATE DeliverySetup SET ImagePad=%q"
                + " WHERE DeliverySetupID=%n", "@Photopic", oDeliverySetup.DeliverySetupID);

            tc.ExecuteNonQueryCommText(CommandType.Text, sSQL, Photo);
        }

        #region Get & Exist Function
        public static IDataReader Get(TransactionContext tc, long nID)
        {
            return tc.ExecuteReader("SELECT * FROM DeliverySetup WHERE DeliverySetupID=%n", nID);
        }
        public static IDataReader GetByBU(TransactionContext tc, int nBUID)
        {
            return tc.ExecuteReader("SELECT TOP 1 * FROM DeliverySetup WHERE BUID =%n", nBUID);
        }
     
        public static IDataReader Gets(TransactionContext tc)
        {
            return tc.ExecuteReader("SELECT * FROM DeliverySetup");
        }
        public static IDataReader Gets(TransactionContext tc, string sSQL)
        {
            return tc.ExecuteReader(sSQL);
        }
        #endregion
    }
}

