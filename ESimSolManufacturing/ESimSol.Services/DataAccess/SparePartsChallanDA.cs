using System;
using System.Data;
using System.Data.SqlClient;
using ICS.Core.DataAccess;
using ESimSol.BusinessObjects;
using System.Data.OleDb;
using ICS.Core.Utility;

namespace ESimSol.Services.DataAccess
{
    public class SparePartsChallanDA
    {
        public SparePartsChallanDA() { }

        #region Insert Update Delete Function
        public static IDataReader InsertUpdate(TransactionContext tc, SparePartsChallan oSparePartsChallan, EnumDBOperation eEnumDBOperation, Int64 nUserID)
        {
            return tc.ExecuteReader("EXEC [SP_IUD_SparePartsChallan]" +
                "%n, %s, %d, %n, %n, %n, %n, %s, %n, %n",
                oSparePartsChallan.SparePartsChallanID, oSparePartsChallan.ChallanNo, oSparePartsChallan.ChallanDate, oSparePartsChallan.SparePartsRequisitionID,
                oSparePartsChallan.StoreID, oSparePartsChallan.DisburseBy, oSparePartsChallan.BUID, oSparePartsChallan.Remarks, (int)eEnumDBOperation, nUserID);
        }

        public static void Delete(TransactionContext tc, SparePartsChallan oSparePartsChallan, EnumDBOperation eEnumDBOperation, Int64 nUserID)
        {
            tc.ExecuteNonQuery("EXEC [SP_IUD_SparePartsChallan]" +
                "%n, %s, %d, %n, %n, %n, %n, %s, %n, %n",
                oSparePartsChallan.SparePartsChallanID, oSparePartsChallan.ChallanNo, oSparePartsChallan.ChallanDate, oSparePartsChallan.SparePartsRequisitionID,
                oSparePartsChallan.StoreID, oSparePartsChallan.DisburseBy, oSparePartsChallan.BUID, oSparePartsChallan.Remarks, (int)eEnumDBOperation, nUserID);
        }
        public static IDataReader Disburse(TransactionContext tc, SparePartsChallan oSparePartsChallan, EnumDBOperation eEnumDBOperation, Int64 nUserID)
        {
            return tc.ExecuteReader("EXEC [SP_SparePartsChallanDisburse]" +
                "%n, %n, %n",  oSparePartsChallan.SparePartsChallanID, (int)eEnumDBOperation, nUserID);
        }
        #endregion

        #region Get & Exist Function
        public static IDataReader Get(TransactionContext tc, long nID)
        {
            return tc.ExecuteReader("SELECT * FROM View_SparePartsChallan WHERE SparePartsChallanID=%n", nID);
        }

        public static IDataReader Gets(TransactionContext tc)
        {
            return tc.ExecuteReader("SELECT * FROM View_SparePartsChallan Order By [Name]");
        }
        public static IDataReader Gets(TransactionContext tc, string sSQL)
        {
            return tc.ExecuteReader(sSQL);//use View_SparePartsChallan
        }
        #endregion
    }
}
