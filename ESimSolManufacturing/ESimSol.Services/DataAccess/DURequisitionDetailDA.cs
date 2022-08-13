using System;
using System.Data;
using System.Data.SqlClient;
using ICS.Core.DataAccess;
using ESimSol.BusinessObjects;
using System.Data.OleDb;
using ICS.Core.Utility;
namespace ESimSol.Services.DataAccess
{

    public class DURequisitionDetailDA
    {
        public DURequisitionDetailDA() { }

        #region Insert Update Delete Function
        public static IDataReader InsertUpdate(TransactionContext tc, DURequisitionDetail oDURequisitionDetail, EnumDBOperation eEnumDBOperation, Int64 nUserId)
        {
            return tc.ExecuteReader("EXEC[SP_IUD_DURequisitionDetail]"
                                    + "%n,%n,%n,%n,%n, %n,%n,%n,%n,%n, %n, %s,%n,%n,%s",
                                    oDURequisitionDetail.DURequisitionDetailID, oDURequisitionDetail.DURequisitionID, 
                                    oDURequisitionDetail.DyeingOrderID, oDURequisitionDetail.ProductID, oDURequisitionDetail.LotID,
                                    oDURequisitionDetail.DestinationLotID,oDURequisitionDetail.Qty, oDURequisitionDetail.UnitPrice, 
                                    oDURequisitionDetail.CurrencyID, oDURequisitionDetail.MUnitID,oDURequisitionDetail.BagNo,
                                    oDURequisitionDetail.Note, nUserId, (int)eEnumDBOperation, "");
        }
        public static void Delete(TransactionContext tc, DURequisitionDetail oDURequisitionDetail, EnumDBOperation eEnumDBOperation, Int64 nUserId, string sPIDetailIDs)
        {
            tc.ExecuteNonQuery("EXEC [SP_IUD_DURequisitionDetail]"
                                     + "%n,%n,%n,%n,%n, %n,%n,%n,%n,%n, %n, %s,%n,%n,%s",
                                    oDURequisitionDetail.DURequisitionDetailID, oDURequisitionDetail.DURequisitionID, 
                                    oDURequisitionDetail.DyeingOrderID, oDURequisitionDetail.ProductID, oDURequisitionDetail.LotID,
                                    oDURequisitionDetail.DestinationLotID,oDURequisitionDetail.Qty, oDURequisitionDetail.UnitPrice, 
                                    oDURequisitionDetail.CurrencyID, oDURequisitionDetail.MUnitID,oDURequisitionDetail.BagNo,
                                    oDURequisitionDetail.Note, nUserId, (int)eEnumDBOperation, sPIDetailIDs);
        }

        #endregion

        #region Get & Exist Function
        public static IDataReader Get(TransactionContext tc, long nID)
        {
            return tc.ExecuteReader("SELECT * FROM View_DURequisitionDetail WHERE DURequisitionDetailID=%n", nID);
        }
        public static IDataReader Gets(TransactionContext tc)
        {
            return tc.ExecuteReader("SELECT * FROM View_DURequisitionDetail");
        }
        public static IDataReader Gets(int DURequisitionID, TransactionContext tc)
        {
            return tc.ExecuteReader("SELECT * FROM View_DURequisitionDetail WHERE DURequisitionID =" + DURequisitionID);
        }
        public static IDataReader Gets(string sSQL, TransactionContext tc)
        {
            return tc.ExecuteReader(sSQL);
        }
        #endregion
    }
}