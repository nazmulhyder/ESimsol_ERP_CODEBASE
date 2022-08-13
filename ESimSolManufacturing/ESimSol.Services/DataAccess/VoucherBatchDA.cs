using System;
using System.Data;
using System.Data.SqlClient;
using ESimSol.BusinessObjects;
using System.Data.OleDb;
using ICS.Core.DataAccess;
using ICS.Core.Utility;

namespace ESimSol.Services.DataAccess
{
    public class VoucherBatchDA
    {
        public VoucherBatchDA() { }

        #region Insert Update Delete Function
        public static IDataReader InsertUpdate(TransactionContext tc, VoucherBatch oVoucherBatch, EnumDBOperation eEnumDBOperation, Int64 nUserID)
        {
            return tc.ExecuteReader("EXEC [SP_IUD_VoucherBatch]"
                                    + "%n, %s, %n, %d, %n, %n, %d, %n, %n",
                                    oVoucherBatch.VoucherBatchID, oVoucherBatch.BatchNO, oVoucherBatch.CreateBy,oVoucherBatch.CreateDate, oVoucherBatch.BatchStatus,oVoucherBatch.RequestTo,oVoucherBatch.RequestDate, (int)eEnumDBOperation, nUserID);
        }

        public static void Delete(TransactionContext tc, VoucherBatch oVoucherBatch, EnumDBOperation eEnumDBOperation, Int64 nUserID)
        {
            tc.ExecuteNonQuery("EXEC [SP_IUD_VoucherBatch]"
                                   + "%n, %s, %n, %d, %n, %n, %d, %n, %n",
                                   oVoucherBatch.VoucherBatchID, oVoucherBatch.BatchNO, oVoucherBatch.CreateBy, oVoucherBatch.CreateDate, oVoucherBatch.BatchStatus, oVoucherBatch.RequestTo, oVoucherBatch.RequestDate, (int)eEnumDBOperation, nUserID);
        }

        public static IDataReader UpdateStatus(TransactionContext tc, VoucherBatch oVoucherBatch, Int64 nUserID)
        {
            return tc.ExecuteReader("EXEC [SP_VoucherBatchStatusUpdate]"
                                    + "%n, %n, %n, %n",
                                    oVoucherBatch.VoucherBatchID, (int)oVoucherBatch.BatchStatus, oVoucherBatch.RequestTo, nUserID);
        }

        public static void VoucherBatchTransfer(TransactionContext tc, int nVoucherBatchID, string sVoucherIDs)
        {
            tc.ExecuteNonQuery("UPDATE Voucher SET BatchID=" + nVoucherBatchID + " WHERE VoucherID IN (" + sVoucherIDs + ")");
        }
        #endregion

        #region Get & Exist Function
        public static IDataReader Get(TransactionContext tc, long nID)
        {
            return tc.ExecuteReader("SELECT * FROM View_VoucherBatch WHERE VoucherBatchID=%n", nID);
        }

        public static IDataReader Gets(TransactionContext tc)
        {
            return tc.ExecuteReader("SELECT * FROM View_VoucherBatch ORDER BY CreateDate ASC");
        }
        public static IDataReader GetsByCreateBy(TransactionContext tc, int nUserID)
        {
            return tc.ExecuteReader("SELECT * FROM View_VoucherBatch WHERE BatchStatus<5 AND CreateBy=" + nUserID.ToString() + " ORDER BY CreateDate ASC");
        }
        public static IDataReader GetsTransferTo(TransactionContext tc, int nVoucherBatchID, int nUserID)
        {
            return tc.ExecuteReader("SELECT * FROM View_VoucherBatch WHERE CreateBy=" + nUserID + " AND VoucherBatchID!=" + nVoucherBatchID + " AND BatchStatus=1 ORDER BY CreateDate ASC");
        }
        public static IDataReader Gets(TransactionContext tc, string sSQL)
        {
            return tc.ExecuteReader(sSQL);//use View_VoucherBatch
        }
      

       

       
        #endregion
    }  
}
