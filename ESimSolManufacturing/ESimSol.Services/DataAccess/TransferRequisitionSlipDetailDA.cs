using System;
using System.Data;
using System.Data.SqlClient;
using ICS.Core.DataAccess;
using ESimSol.BusinessObjects;
using System.Data.OleDb;
using ICS.Core.Utility;
namespace ESimSol.Services.DataAccess
{
    public class TransferRequisitionSlipDetailDA
    {
        public TransferRequisitionSlipDetailDA() { }

        #region Insert Function
        public static IDataReader InsertUpdate(TransactionContext tc, TransferRequisitionSlipDetail oTRSDetail, EnumDBOperation eEnumDBOperation, Int64 nUserID, string sDetailIDs)
        {
            return tc.ExecuteReader("EXEC [SP_IUD_TransferRequisitionSlipDetail]" + "%n, %n, %n, %n, %n, %n, %n, %n, %n, %n, %s, %s, %n, %n, %n, %s",
                                     oTRSDetail.TRSDetailID, oTRSDetail.TRSID, oTRSDetail.StyleID, oTRSDetail.ProductID, oTRSDetail.LotID, oTRSDetail.QTY, oTRSDetail.MUnitID, oTRSDetail.BagBales, oTRSDetail.UnitPrice, oTRSDetail.CurrencyID, oTRSDetail.SuggestLotNo, oTRSDetail.Remark, oTRSDetail.DestinationLotID,  nUserID, (int)eEnumDBOperation, sDetailIDs);
        }
        public static void Delete(TransactionContext tc, TransferRequisitionSlipDetail oTRSDetail, EnumDBOperation eEnumDBOperation, Int64 nUserID, string sDetailIDs)
        {
            tc.ExecuteNonQuery("EXEC [SP_IUD_TransferRequisitionSlipDetail]" + "%n, %n, %n, %n, %n, %n, %n, %n, %n, %n, %s, %s, %n, %n, %n, %s",
                                     oTRSDetail.TRSDetailID, oTRSDetail.TRSID, oTRSDetail.StyleID, oTRSDetail.ProductID, oTRSDetail.LotID, oTRSDetail.QTY, oTRSDetail.MUnitID, oTRSDetail.BagBales, oTRSDetail.UnitPrice, oTRSDetail.CurrencyID, oTRSDetail.SuggestLotNo, oTRSDetail.Remark, oTRSDetail.DestinationLotID, nUserID, (int)eEnumDBOperation, sDetailIDs);
        }
        #endregion

        #region Get & Exist Function
        public static IDataReader Get(TransactionContext tc, int nID)
        {
            return tc.ExecuteReader("SELECT * FROM View_TransferRequisitionSlipDetail WHERE RequisitionSlipDetailID=%n", nID);
        }
        public static IDataReader Gets(TransactionContext tc, int nTRSID)
        {
            return tc.ExecuteReader("SELECT * FROM View_TransferRequisitionSlipDetail WHERE [TRSID]=%n", nTRSID);
        }
        public static IDataReader Gets(TransactionContext tc, string sSQL)
        {
            return tc.ExecuteReader(sSQL);
        }
        #endregion

    }
}
