using System;
using System.Data;
using System.Data.SqlClient;
using ICS.Core;
using ESimSol.BusinessObjects;
using System.Data.OleDb;
using ICS.Core.Framework;
using ICS.Core.Utility;
using ICS.Core.DataAccess;

namespace ESimSol.Services.DataAccess
{
    public class OrderRecapDetailDA
    {
        public OrderRecapDetailDA() { }

        #region Insert Update Delete Function
        public static IDataReader InsertUpdate(TransactionContext tc, OrderRecapDetail oOrderRecapDetail, EnumDBOperation eEnumDBOrderRecapDetail, Int64 nUserId)
        {
            return tc.ExecuteReader("EXEC [SP_IUD_OrderRecapDetail]"
                                    + "%n, %n, %n, %n, %n, %n, %n, %n, %n, %n, %s",
                                    oOrderRecapDetail.OrderRecapDetailID, oOrderRecapDetail.OrderRecapID, oOrderRecapDetail.ColorID, oOrderRecapDetail.SizeID, oOrderRecapDetail.MeasurementUnitID, oOrderRecapDetail.UnitPrice, oOrderRecapDetail.Quantity, oOrderRecapDetail.Amount, nUserId, (int)eEnumDBOrderRecapDetail, "");
        }

        
        public static void Delete(TransactionContext tc, OrderRecapDetail oOrderRecapDetail, EnumDBOperation eEnumDBOrderRecapDetail, Int64 nUserId, string sOrderRecapDetailIDs)
        {
            tc.ExecuteNonQuery("EXEC [SP_IUD_OrderRecapDetail]"
                                    + "%n, %n, %n, %n, %n, %n, %n, %n, %n, %n, %s",
                                    oOrderRecapDetail.OrderRecapDetailID, oOrderRecapDetail.OrderRecapID, oOrderRecapDetail.ColorID, oOrderRecapDetail.SizeID, oOrderRecapDetail.MeasurementUnitID, oOrderRecapDetail.UnitPrice, oOrderRecapDetail.Quantity, oOrderRecapDetail.Amount, nUserId, (int)eEnumDBOrderRecapDetail, sOrderRecapDetailIDs);
        }

        #endregion

        #region Get & Exist Function
      
        public static IDataReader Gets(TransactionContext tc, int id)
        {
            return tc.ExecuteReader("SELECT * FROM View_OrderRecapDetail WHERE OrderRecapID=%n ORDER BY ColorSequence", id);//don not  change  ColorSequence Column
        }

        public static IDataReader GetsByLog(TransactionContext tc, int id)
        {
            return tc.ExecuteReader("SELECT * FROM View_OrderRecapDetailLog WHERE OrderRecapLogID=%n ORDER BY ColorSequence", id);//don not  change  ColorSequence Column
        }
        public static IDataReader GetsByStyle(TransactionContext tc, int tsid)
        {
            return tc.ExecuteReader("SELECT * FROM View_OrderRecapDetail WHERE OrderRecapID IN (SELECT OrderRecapID FROM OrderRecap WHERE TechnicalSheetID=%n)", tsid);
        }

        public static IDataReader Gets(TransactionContext tc, string sSql)
        {
            return tc.ExecuteReader(sSql);
        }
        public static void DeleteByOrderRecap(TransactionContext tc, int nOrderRecapID)
        {
            tc.ExecuteNonQuery("DELETE FROM OrderRecapDetail WHERE OrderRecapID=%n", nOrderRecapID);
        }
        #endregion
    }
}
