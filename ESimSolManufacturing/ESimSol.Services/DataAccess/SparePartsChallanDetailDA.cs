using System;
using System.Data;
using System.Data.SqlClient;
using ICS.Core.DataAccess;
using ESimSol.BusinessObjects;
using System.Data.OleDb;
using ICS.Core.Utility;

namespace ESimSol.Services.DataAccess
{
    public class SparePartsChallanDetailDA
    {
        public SparePartsChallanDetailDA() { }

        #region Insert Update Delete Function
        public static IDataReader InsertUpdate(TransactionContext tc, SparePartsChallanDetail oSparePartsChallanDetail, EnumDBOperation eEnumDBOperation, Int64 nUserID, string sSparePartsChallanDetailIDs)
        {
            return tc.ExecuteReader("EXEC [SP_IUD_SparePartsChallanDetail]" +
                "%n, %n, %n, %n, %n, %n, %n, %s, %n, %n, %s",
                oSparePartsChallanDetail.SparePartsChallanDetailID, oSparePartsChallanDetail.SparePartsChallanID, oSparePartsChallanDetail.SparePartsRequisitionDetailID,
                oSparePartsChallanDetail.ProductID, oSparePartsChallanDetail.LotID, oSparePartsChallanDetail.MUnitID, oSparePartsChallanDetail.ChallanQty,
                oSparePartsChallanDetail.Remarks, (int)eEnumDBOperation, nUserID, sSparePartsChallanDetailIDs);
        }
        public static void Delete(TransactionContext tc, SparePartsChallanDetail oSparePartsChallanDetail, EnumDBOperation eEnumDBOperation, Int64 nUserID, string sSparePartsChallanDetailIDs)
        {
            tc.ExecuteNonQuery("EXEC [SP_IUD_SparePartsChallanDetail]" +
                 "%n, %n, %n, %n, %n, %n, %n, %s, %n, %n, %s",
                oSparePartsChallanDetail.SparePartsChallanDetailID, oSparePartsChallanDetail.SparePartsChallanID, oSparePartsChallanDetail.SparePartsRequisitionDetailID,
                oSparePartsChallanDetail.ProductID, oSparePartsChallanDetail.LotID, oSparePartsChallanDetail.MUnitID, oSparePartsChallanDetail.ChallanQty,
                oSparePartsChallanDetail.Remarks, (int)eEnumDBOperation, nUserID, sSparePartsChallanDetailIDs);
        }
        #endregion

        #region Get & Exist Function
        public static IDataReader Get(TransactionContext tc, long nID)
        {
            return tc.ExecuteReader("SELECT * FROM View_SparePartsChallanDetail WHERE SparePartsChallanDetailID=%n", nID);
        }

        public static IDataReader Gets(TransactionContext tc)
        {
            return tc.ExecuteReader("SELECT * FROM View_SparePartsChallanDetail Order By [Name]");
        }
        public static IDataReader GetsBySparePartsChallanID(TransactionContext tc, int nSparePartsChallanID)
        {
            return tc.ExecuteReader("SELECT * FROM View_SparePartsChallanDetail WHERE SparePartsChallanID=" + nSparePartsChallanID);
        }
        public static IDataReader Gets(TransactionContext tc, string sSQL)
        {
            return tc.ExecuteReader(sSQL);//use View_SparePartsChallanDetail
        }
        #endregion
    }
}
