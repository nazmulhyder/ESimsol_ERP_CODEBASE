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

    public class PurchaseRequisitionDetailDA
    {
        public PurchaseRequisitionDetailDA() { }

        #region Insert Update Delete Function
        public static IDataReader InsertUpdate(TransactionContext tc, PurchaseRequisitionDetail oPRDetail, EnumDBOperation eEnumDBOperation, Int64 nUserID, String sPurchaseRequisitionDetailIDs)
        {
            return tc.ExecuteReader("EXEC[SP_IUD_PurchaseRequisitionDetail] "
                                    + "%n, %n,%n, %n, %n, %s, %n, %n, %n, %s, %s , %n",
                                    oPRDetail.PRDetailID, oPRDetail.PRID, oPRDetail.OrderRecapID, oPRDetail.ProductID, oPRDetail.Qty, oPRDetail.Note, oPRDetail.MUnitID, nUserID, (int)eEnumDBOperation, sPurchaseRequisitionDetailIDs, oPRDetail.RequiredFor, oPRDetail.VehicleModelID);
        }

        public static void Delete(TransactionContext tc, PurchaseRequisitionDetail oPRDetail, EnumDBOperation eEnumDBOperation, Int64 nUserID, String sPurchaseRequisitionDetailIDs)
        {
            tc.ExecuteNonQuery("EXEC[SP_IUD_PurchaseRequisitionDetail] "
                                    + "%n, %n,%n, %n, %n, %s, %n, %n, %n, %s, %s , %n",
                                    oPRDetail.PRDetailID, oPRDetail.PRID, oPRDetail.OrderRecapID, oPRDetail.ProductID, oPRDetail.Qty, oPRDetail.Note, oPRDetail.MUnitID, nUserID, (int)eEnumDBOperation, sPurchaseRequisitionDetailIDs, oPRDetail.RequiredFor, oPRDetail.VehicleModelID);
        }
       
     
        #endregion

        #region Get & Exist Function
        public static IDataReader Get(TransactionContext tc, long nID)
        {
            return tc.ExecuteReader("SELECT * FROM View_PurchaseRequisitionDetail WHERE PurchaseRequisitionDetailID=%n", nID);
        }

        public static IDataReader Gets(int nPurchaseRequisitionID, TransactionContext tc)
        {
            return tc.ExecuteReader("SELECT * FROM View_PurchaseRequisitionDetail where PRID =%n ", nPurchaseRequisitionID);
        }
        public static IDataReader GetsBy(int nPRID,int nContractorID, TransactionContext tc)
        {
            //return tc.ExecuteReader("SELECT * FROM View_PurchaseRequisitionDetail where PRDetailID not in (Select PurchaseOrderDetail.PRDetailID from PurchaseOrderDetail) and ContractorID =%n ", nContractorID);
            return tc.ExecuteReader("SELECT * FROM View_PurchaseRequisitionDetail  where PRID=%n and PRDetailID not in (Select PRDetailID from PurchaseOrderDetail)", nPRID);
        }

        public static IDataReader Gets(TransactionContext tc, string sSQL)
        {
            return tc.ExecuteReader(sSQL);
        }
        #endregion
    }
    

}
