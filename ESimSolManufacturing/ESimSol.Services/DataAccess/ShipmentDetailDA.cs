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
    public class ShipmentDetailDA
    {
        #region Insert Update Delete Function

        public static IDataReader InsertUpdate(TransactionContext tc, ShipmentDetail oShipmentDetail, EnumDBOperation eEnumDBOperation, Int64 nUserID, string sShipmentDetailIDs)
        {
            return tc.ExecuteReader("EXEC [SP_IUD_ShipmentDetail]"
                                   + "%n,%n,%n,%n,     %n,%n,%n,       %s,%n,%n,%s",
                                   oShipmentDetail.ShipmentDetailID, oShipmentDetail.ShipmentID, oShipmentDetail.OrderRecapID, oShipmentDetail.LotID,
                                   oShipmentDetail.CountryID, oShipmentDetail.ShipmentQty, oShipmentDetail.CTNQty, 
                                   oShipmentDetail.Remarks, nUserID, (int)eEnumDBOperation, "");
        }

        public static void Delete(TransactionContext tc, ShipmentDetail oShipmentDetail, EnumDBOperation eEnumDBOperation, Int64 nUserID, string sShipmentDetailIDs)
        {
            tc.ExecuteNonQuery("EXEC [SP_IUD_ShipmentDetail]"
                                   + "%n,%n,%n,%n,     %n,%n,%n,       %s,%n,%n,%s",
                                   oShipmentDetail.ShipmentDetailID, oShipmentDetail.ShipmentID, oShipmentDetail.OrderRecapID, oShipmentDetail.LotID,
                                   oShipmentDetail.CountryID, oShipmentDetail.ShipmentQty, oShipmentDetail.CTNQty,
                                   oShipmentDetail.Remarks, nUserID, (int)eEnumDBOperation, sShipmentDetailIDs);
        }

        #endregion

        #region Get & Exist Function
        public static IDataReader Get(TransactionContext tc, long nID)
        {
            return tc.ExecuteReader("SELECT * FROM View_ShipmentDetail WHERE ShipmentDetailID=%n", nID);
        }
        public static IDataReader Gets(TransactionContext tc, int nShipmentID)
        {
            return tc.ExecuteReader("SELECT * FROM View_ShipmentDetail WHERE ShipmentID =%n", nShipmentID);
        }
        public static IDataReader Gets(TransactionContext tc, string sSQL)
        {
            return tc.ExecuteReader(sSQL);
        }

        #endregion
    }

}
