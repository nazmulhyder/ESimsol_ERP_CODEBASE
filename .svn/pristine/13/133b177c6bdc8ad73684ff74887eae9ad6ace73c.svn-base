using System;
using System.Data;
using System.Data.SqlClient;
using ESimSol.BusinessObjects;
using System.Data.OleDb;
using ICS.Core.DataAccess;
using ICS.Core.Utility;

namespace ESimSol.Services.DataAccess
{
    public class PTUUnit2DistributionDA
    {


        #region Get & Exist Function

        public static IDataReader PTUTransfer(PTUUnit2Distribution oPTUUnit2Distribution, TransactionContext tc, int nUserID)
        {
            return tc.ExecuteReader("Exec[SP_PTU_Receive]"+"%n,%n,%n, %n",oPTUUnit2Distribution.PTUUnit2DistributionID,oPTUUnit2Distribution.PTUUnit2ID, oPTUUnit2Distribution.Qty, nUserID);
        }
        public static IDataReader ReceiveInReadyeStock(PTUUnit2Distribution oPTUUnit2Distribution, TransactionContext tc, int nUserID)
        {
            return tc.ExecuteReader("Exec[SP_PTU_ReceiveInReadyeStock]" + "%n,%n,%n, %n,%n", oPTUUnit2Distribution.PTUUnit2ID, oPTUUnit2Distribution.LotID, oPTUUnit2Distribution.WorkingUnitID, oPTUUnit2Distribution.Qty, nUserID);
        }

        public static IDataReader ReceiveInAvilableStock(PTUUnit2Distribution oPTUUnit2Distribution, TransactionContext tc, int nUserID)
        {
            return tc.ExecuteReader("Exec[SP_PTU_ReceiveInAvilableStock]" + "%n,%n,%n, %n,%n", oPTUUnit2Distribution.PTUUnit2ID, oPTUUnit2Distribution.LotID, oPTUUnit2Distribution.WorkingUnitID, oPTUUnit2Distribution.Qty, nUserID);
        }

        public static IDataReader PTUTransferSubContract(PTUUnit2Distribution oPTUUnit2Distribution, TransactionContext tc, int nUserID)
        {
            return tc.ExecuteReader("Exec[SP_PTU_ReceiveSubContract]" + "%n,%n,%n, %n,%n,%n", oPTUUnit2Distribution.PTUUnit2ID, oPTUUnit2Distribution.PTUUnit2DistributionID, oPTUUnit2Distribution.WorkingUnitID, oPTUUnit2Distribution.Qty,oPTUUnit2Distribution.BUID, nUserID);
        }
        public static IDataReader Gets(TransactionContext tc)
        {
            return tc.ExecuteReader("SELECT * FROM View_PTUUnit2Distribution");
        }
        public static IDataReader Gets(TransactionContext tc, int nShelfID)
        {
            return tc.ExecuteReader("SELECT * FROM View_PTUUnit2Distribution WHERE ShelfID = %n", nShelfID);
        }
    
        public static IDataReader Gets(TransactionContext tc, string sSQL)
        {
            return tc.ExecuteReader(sSQL);
        }
        public static IDataReader ConfirmPTUUnit2Distribution(TransactionContext tc, PTUUnit2Distribution oPTUUnit2Distribution,  Int64 nUserID)
        {
            tc.ExecuteNonQuery("UPDATE PTUUnit2Distribution SET Qty = " + oPTUUnit2Distribution.Qty + " WHERE PTUUnit2DistributionID =" + oPTUUnit2Distribution.PTUUnit2DistributionID, nUserID);
            return tc.ExecuteReader("SELECT * FROM PTUUnit2Distribution WHERE PTUUnit2DistributionID = " + oPTUUnit2Distribution.PTUUnit2DistributionID, nUserID);
        }
        #endregion
    }  
    
    
}
