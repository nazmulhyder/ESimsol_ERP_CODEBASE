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
    public class GUProductionOrderDetailDA
    {
        public GUProductionOrderDetailDA() { }

        #region Insert Update Delete Function
        public static IDataReader InsertUpdate(TransactionContext tc, GUProductionOrderDetail oGUProductionOrderDetail, EnumDBOperation eEnumDBOperation, Int64 nUserID, String sGUProductionOrderDetailIDs)
        {
            return tc.ExecuteReader("EXEC [SP_IUD_GUProductionOrderDetail]"
                                    + "%n, %n, %n,%n, %n, %n, %n, %n,%s",
                                    oGUProductionOrderDetail.GUProductionOrderDetailID, oGUProductionOrderDetail.GUProductionOrderID, oGUProductionOrderDetail.ColorID, oGUProductionOrderDetail.SizeID, oGUProductionOrderDetail.UnitID, oGUProductionOrderDetail.Qty, (int)eEnumDBOperation, nUserID, sGUProductionOrderDetailIDs);
        }

        public static void Delete(TransactionContext tc, GUProductionOrderDetail oGUProductionOrderDetail, EnumDBOperation eEnumDBOperation, Int64 nUserID, String sGUProductionOrderDetailIDs)
        {
            tc.ExecuteNonQuery("EXEC [SP_IUD_GUProductionOrderDetail]"
                                    + "%n, %n, %n,%n, %n, %n, %n, %n,%s",
                                    oGUProductionOrderDetail.GUProductionOrderDetailID, oGUProductionOrderDetail.GUProductionOrderID, oGUProductionOrderDetail.ColorID, oGUProductionOrderDetail.SizeID, oGUProductionOrderDetail.UnitID, oGUProductionOrderDetail.Qty, (int)eEnumDBOperation, nUserID, sGUProductionOrderDetailIDs);
        }
        #endregion

        #region Get & Exist Function
        public static IDataReader Get(TransactionContext tc, long nID)
        {
            return tc.ExecuteReader("SELECT * FROM View_GUProductionOrderDetail WHERE GUProductionOrderDetailID=%n", nID);
        }

        public static IDataReader Gets(TransactionContext tc)
        {
            return tc.ExecuteReader("SELECT * FROM View_GUProductionOrderDetail");
        }

        public static IDataReader GetsByGUProductionOrder(int nid, TransactionContext tc)
        {
            return tc.ExecuteReader("SELECT * FROM View_GUProductionOrderDetail WHERE GUProductionOrderID=%n", nid);
        }

        public static IDataReader Gets(int nid, TransactionContext tc)
        {
            return tc.ExecuteReader("SELECT * FROM View_GUProductionOrderDetail WhERE GUProductionOrderID = %n", nid);
        }

        public static IDataReader Gets(TransactionContext tc, string sSQL)
        {
            return tc.ExecuteReader(sSQL);
        }
        #endregion
    }
}
