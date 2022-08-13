using System;
using System.Data;
using System.Data.SqlClient;
using ICS.Core.DataAccess;
using ESimSol.BusinessObjects;
using System.Data.OleDb;
using ICS.Core.Utility;
namespace ESimSol.Services.DataAccess
{

    public class DUProGuideLineDetailDA
    {
        public DUProGuideLineDetailDA() { }

        #region Insert Update Delete Function
        public static IDataReader InsertUpdate(TransactionContext tc, DUProGuideLineDetail oDUProGuideLineDetail, EnumDBOperation eEnumDBOperation, Int64 nUserId)
        {
            return tc.ExecuteReader("EXEC[SP_IUD_DUProGuideLineDetail]"
                                    + "%n,%n,%n,%s,%n,  %n,%n,%n,%n,%n,%n,  %s,%s,%n,%n,%s",
                                    oDUProGuideLineDetail.DUProGuideLineDetailID, oDUProGuideLineDetail.DUProGuideLineID,
                                    oDUProGuideLineDetail.ProductID, oDUProGuideLineDetail.LotNo, oDUProGuideLineDetail.LotID,
                                    oDUProGuideLineDetail.LotParentID, oDUProGuideLineDetail.Qty, oDUProGuideLineDetail.UnitPrice,
                                    oDUProGuideLineDetail.CurrencyID, oDUProGuideLineDetail.MUnitID, oDUProGuideLineDetail.BagNo,
                                    oDUProGuideLineDetail.Brand, oDUProGuideLineDetail.Note, nUserId, (int)eEnumDBOperation, "");
        }
        public static void Delete(TransactionContext tc, DUProGuideLineDetail oDUProGuideLineDetail, EnumDBOperation eEnumDBOperation, Int64 nUserId, string sPIDetailIDs)
        {
            tc.ExecuteNonQuery("EXEC [SP_IUD_DUProGuideLineDetail]"
                                      + "%n,%n,%n,%s,%n,  %n,%n,%n,%n,%n,%n,  %s,%s,%n,%n,%s",
                                    oDUProGuideLineDetail.DUProGuideLineDetailID, oDUProGuideLineDetail.DUProGuideLineID,
                                    oDUProGuideLineDetail.ProductID, oDUProGuideLineDetail.LotNo, oDUProGuideLineDetail.LotID,
                                    oDUProGuideLineDetail.LotParentID, oDUProGuideLineDetail.Qty, oDUProGuideLineDetail.UnitPrice,
                                    oDUProGuideLineDetail.CurrencyID, oDUProGuideLineDetail.MUnitID, oDUProGuideLineDetail.BagNo,
                                    oDUProGuideLineDetail.Brand, oDUProGuideLineDetail.Note, nUserId, (int)eEnumDBOperation, sPIDetailIDs);
        }

        #endregion

        #region Get & Exist Function
        public static IDataReader Get(TransactionContext tc, long nID)
        {
            return tc.ExecuteReader("SELECT * FROM View_DUProGuideLineDetail WHERE DUProGuideLineDetailID=%n", nID);
        }
        public static IDataReader Gets(TransactionContext tc)
        {
            return tc.ExecuteReader("SELECT * FROM View_DUProGuideLineDetail");
        }
        public static IDataReader Gets(int DUProGuideLineID, TransactionContext tc)
        {
            return tc.ExecuteReader("SELECT * FROM View_DUProGuideLineDetail WHERE DUProGuideLineID =" + DUProGuideLineID);
        }
        public static IDataReader Gets(string sSQL, TransactionContext tc)
        {
            return tc.ExecuteReader(sSQL);
        }
   
        public static void UpdateReturnQty(TransactionContext tc, DUProGuideLineDetail oDUProGuideLineDetail, EnumDBOperation eEnumDBOperation, Int64 nUserId)
        {
            tc.ExecuteNonQuery("UPDATE DUProGuideLineDetail SET Qty_Return = %n WHERE DUProGuideLineDetailID= %n", oDUProGuideLineDetail.Qty, oDUProGuideLineDetail.DUProGuideLineDetailID);
        }
        #endregion
    }
}