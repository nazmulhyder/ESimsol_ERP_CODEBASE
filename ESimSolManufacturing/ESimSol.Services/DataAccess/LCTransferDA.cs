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
    public class LCTransferDA
    {
        public LCTransferDA() { }

        #region Insert Update Delete Function
        public static IDataReader InsertUpdate(TransactionContext tc, LCTransfer oLCTransfer, EnumDBOperation eEnumDBOperation, Int64 nUserID)
        {
            return tc.ExecuteReader("EXEC[SP_IUD_LCTransfer]" + "%n, %n, %s, %n, %d, %n, %n, %n, %n, %s, %d, %n, %n, %n, %s, %b,%n,%n,%n,%n",
                                    oLCTransfer.LCTransferID, oLCTransfer.MasterLCID, oLCTransfer.RefNo, (int)oLCTransfer.LCTransferStatus, oLCTransfer.TransferIssueDate, oLCTransfer.ProductionFactoryID, oLCTransfer.BuyerID, oLCTransfer.CommissionFavorOf, oLCTransfer.CommissionAccountID, oLCTransfer.TransferNo, oLCTransfer.TransferDate, oLCTransfer.TransferAmount, oLCTransfer.CommissionAmount, oLCTransfer.ApprovedBy, oLCTransfer.Note, oLCTransfer.IsCommissionCollect,oLCTransfer.FactoryBranchID,oLCTransfer.BUID,  nUserID, (int)eEnumDBOperation);
        }

        public static void Delete(TransactionContext tc, LCTransfer oLCTransfer, EnumDBOperation eEnumDBOperation, Int64 nUserID)
        {
            tc.ExecuteNonQuery("EXEC[SP_IUD_LCTransfer]" + "%n, %n, %s, %n, %d, %n, %n, %n, %n, %s, %d, %n, %n, %n, %s, %b,%n,%n,%n,%n",
                                    oLCTransfer.LCTransferID, oLCTransfer.MasterLCID, oLCTransfer.RefNo, (int)oLCTransfer.LCTransferStatus, oLCTransfer.TransferIssueDate, oLCTransfer.ProductionFactoryID, oLCTransfer.BuyerID, oLCTransfer.CommissionFavorOf, oLCTransfer.CommissionAccountID, oLCTransfer.TransferNo, oLCTransfer.TransferDate, oLCTransfer.TransferAmount, oLCTransfer.CommissionAmount, oLCTransfer.ApprovedBy, oLCTransfer.Note, oLCTransfer.IsCommissionCollect, oLCTransfer.FactoryBranchID, oLCTransfer.BUID, nUserID, (int)eEnumDBOperation);
        }
        public static IDataReader ChangeStatus(TransactionContext tc, LCTransfer oLCTransfer, EnumDBOperation eEnumDBOperation, Int64 nUserID)
        {
            return tc.ExecuteReader("EXEC[SP_LCTransferOperation]" + "%n,%n,%s,%n,%n,%n",
                                    oLCTransfer.LCTransferID, (int)oLCTransfer.LCTransferStatus, oLCTransfer.Note, (int)oLCTransfer.LCTranferActionType, nUserID, (int)eEnumDBOperation);
        }
        #region Accept LCTransfer Revise
        public static IDataReader AcceptRevise(TransactionContext tc, LCTransfer oLCTransfer, double nTotalDetailQty, Int64 nUserID)
        {
            return tc.ExecuteReader("EXEC[SP_AcceptLCTransferRevise]" + "%n, %n, %s, %n, %d, %n, %n, %n, %n, %s, %d, %n, %n, %n, %s, %b,%n,%n,%n,%n",
                                    oLCTransfer.LCTransferID, oLCTransfer.MasterLCID, oLCTransfer.RefNo, (int)oLCTransfer.LCTransferStatus, oLCTransfer.TransferIssueDate, oLCTransfer.ProductionFactoryID, oLCTransfer.BuyerID, oLCTransfer.CommissionFavorOf, oLCTransfer.CommissionAccountID, oLCTransfer.TransferNo, oLCTransfer.TransferDate, oLCTransfer.TransferAmount, oLCTransfer.CommissionAmount, oLCTransfer.ApprovedBy, oLCTransfer.Note, oLCTransfer.IsCommissionCollect, oLCTransfer.FactoryBranchID,oLCTransfer.BUID,  nUserID, nTotalDetailQty);
        }
        #endregion
        #endregion

        #region Get & Exist Function
        public static void UpdateTransferNoDate(TransactionContext tc, LCTransfer oLCTransfer, Int64 nUserID)
        {
            tc.ExecuteNonQuery("Update LCTransfer SET TransferNo = %s ,  TransferDate = %d WHERE LCTransferID = %n",oLCTransfer.TransferNo,oLCTransfer.TransferDate,oLCTransfer.LCTransferID);
        }
        public static IDataReader Get(TransactionContext tc, long nID)
        {
            return tc.ExecuteReader("SELECT * FROM View_LCTransfer WHERE LCTransferID=%n", nID);
        }

        public static IDataReader GetLog(TransactionContext tc, long nID)
        {
            return tc.ExecuteReader("SELECT * FROM View_LCTransferLog WHERE LCTransferLogID = %n", nID);
        }

        public static IDataReader Gets(TransactionContext tc)
        {
            return tc.ExecuteReader("SELECT * FROM View_LCTransfer");
        }
                
        public static IDataReader Gets(TransactionContext tc, string sSQL)
        {
            return tc.ExecuteReader(sSQL);
        }

        public static IDataReader Gets(TransactionContext tc, int id)
        {
            return tc.ExecuteReader("SELECT * FROM View_LCTransfer WHERE MasterLCID =%n",id);
        }
        #endregion
    }
}
