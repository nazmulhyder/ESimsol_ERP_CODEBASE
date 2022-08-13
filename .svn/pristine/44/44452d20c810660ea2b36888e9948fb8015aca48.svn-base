using System;
using System.Data;
using System.Data.SqlClient;
using ESimSol.BusinessObjects;
using System.Data.OleDb;
using ICS.Core.DataAccess;
using ICS.Core.Utility;

namespace ESimSol.Services.DataAccess
{
    public class DUDeliveryChallanImageDA
    {
        public DUDeliveryChallanImageDA() { }

        #region Insert Update Delete Function
        public static IDataReader InsertUpdate(TransactionContext tc, DUDeliveryChallanImage oDUDeliveryChallanImage, EnumDBOperation eEnumDBOperation, Int64 nUserId)
        {
            return tc.ExecuteReader("EXEC [SP_IUD_DUDeliveryChallanImage]"
                                    + "%n, %n, %s, %s, %s, %q, %n, %n",
                                     oDUDeliveryChallanImage.DUDeliveryChallanImageID,
                                     oDUDeliveryChallanImage.DUDeliveryChallanID,
                                     oDUDeliveryChallanImage.Name,
                                     oDUDeliveryChallanImage.ContractNo,
                                     oDUDeliveryChallanImage.Note,
                                     oDUDeliveryChallanImage.Picture,
                                     nUserId,
                                    (int)eEnumDBOperation);
        }
        public static void Delete(TransactionContext tc, DUDeliveryChallanImage oDUDeliveryChallanImage, EnumDBOperation eEnumDBOperation, Int64 nUserId)
        {
            tc.ExecuteNonQuery("EXEC [SP_IUD_DUDeliveryChallanImage]"
                                    + "%n, %n, %s, %s, %s, %q, %n, %n",
                                     oDUDeliveryChallanImage.DUDeliveryChallanImageID,
                                     oDUDeliveryChallanImage.DUDeliveryChallanID,
                                     oDUDeliveryChallanImage.Name,
                                     oDUDeliveryChallanImage.ContractNo,
                                     oDUDeliveryChallanImage.Note,
                                     oDUDeliveryChallanImage.Picture,
                                     nUserId,
                                    (int)eEnumDBOperation);
        }
        #endregion

        public static void UpdatePicture(TransactionContext tc, DUDeliveryChallanImage oDUDeliveryChallanImage, Int64 nUserID)
        {
            SqlParameter Photo = new SqlParameter();
            Photo.SqlDbType = SqlDbType.Image;
            Photo.ParameterName = "Photopic";
            Photo.Value = oDUDeliveryChallanImage.Picture;

            string sSQL = SQLParser.MakeSQL("UPDATE DUDeliveryChallanImage SET Picture=%q"
                + " WHERE DUDeliveryChallanImageID=%n", "@Photopic", oDUDeliveryChallanImage.DUDeliveryChallanImageID);

            tc.ExecuteNonQueryCommText(CommandType.Text, sSQL, Photo);
        }

        #region Get & Exist Function
        public static IDataReader Get(TransactionContext tc, long nID)
        {
            return tc.ExecuteReader("SELECT * FROM DUDeliveryChallanImage WHERE DUDeliveryChallanImageID=%n", nID);
        }
        public static IDataReader GetByDeliveryChallan(TransactionContext tc, long nID)
        {
            return tc.ExecuteReader("SELECT TOP 1 * FROM DUDeliveryChallanImage WHERE DUDeliveryChallanID=%n ORDER BY DUDeliveryChallanImageID DESC", nID);
        }
        public static IDataReader GetByBU(TransactionContext tc, int nBUID)
        {
            return tc.ExecuteReader("SELECT TOP 1 * FROM DUDeliveryChallanImage WHERE BUID =%n", nBUID);
        }

        public static IDataReader Gets(TransactionContext tc)
        {
            return tc.ExecuteReader("SELECT * FROM DUDeliveryChallanImage");
        }
        public static IDataReader Gets(TransactionContext tc, string sSQL)
        {
            return tc.ExecuteReader(sSQL);
        }
        #endregion
    }
}

