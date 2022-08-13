using System;
using System.Data;
using System.Data.SqlClient;
using ESimSol.BusinessObjects;
using System.Data.OleDb;
using ICS.Core.DataAccess;
using ICS.Core.Utility;

namespace ESimSol.Services.DataAccess
{

    public class NOADetailDA
    {
        public NOADetailDA() { }

        #region Insert Update Delete Function
        public static IDataReader InsertUpdate(TransactionContext tc, NOADetail oNOADetail, EnumDBOperation eEnumDBOperation, Int64 nUserID, string sDetailIDs)
        {
            return tc.ExecuteReader("EXEC [SP_IUD_NOADetail]"
                                    + "%n, %n, %n, %n, %n,%n,%s,%n,%n,%n,%n,%n,%s",
                                    oNOADetail.NOADetailID, oNOADetail.NOAID, oNOADetail.ProductID, oNOADetail.MUnitID, oNOADetail.RequiredQty, oNOADetail.PurchaseQty, oNOADetail.Note, oNOADetail.ApprovedRate, oNOADetail.PQDetailID, (int)eEnumDBOperation, nUserID, oNOADetail.PRDetailID, sDetailIDs);
        }
        public static void Delete(TransactionContext tc, NOADetail oNOADetail, EnumDBOperation eEnumDBOperation, Int64 nUserID, string sDetailIDs)
        {
            tc.ExecuteNonQuery("EXEC [SP_IUD_NOADetail]"
                                    + "%n, %n, %n, %n, %n,%n,%s,%n,%n,%n,%n,%n,%s",
                                    oNOADetail.NOADetailID, oNOADetail.NOAID, oNOADetail.ProductID, oNOADetail.MUnitID, oNOADetail.RequiredQty, oNOADetail.PurchaseQty, oNOADetail.Note, oNOADetail.ApprovedRate, oNOADetail.PQDetailID, (int)eEnumDBOperation, nUserID, oNOADetail.PRDetailID, sDetailIDs);
        }
        //UpdatePaymentTerms
        #endregion

        #region Get & Exist Function
        public static IDataReader Get(TransactionContext tc, long nId)
        {
            return tc.ExecuteReader("SELECT * FROM View_NOADetail WHERE NOADetailID=%n", nId);
        }

        public static IDataReader Gets(TransactionContext tc)
        {
            return tc.ExecuteReader("SELECT * FROM View_NOADetail");
        }
        public static IDataReader Gets(string sSQL, TransactionContext tc)
        {
            return tc.ExecuteReader(sSQL);
        }
        
        public static IDataReader Gets(TransactionContext tc, long nNOAId)
        {
            return tc.ExecuteReader("SELECT * FROM View_NOADetail WHERE NOAID=%n ORDER BY  ProductID,  NOADetailID", nNOAId);
        }
        public static IDataReader GetsByLog(TransactionContext tc, long nNOAId)
        {
            return tc.ExecuteReader("SELECT * FROM View_NOADetailLog WHERE NOALogID=%n ORDER BY  ProductID,  NOADetailLogID", nNOAId);
        }
        public static IDataReader GetsBy(TransactionContext tc, int nNOAId, int nContractorID)
        {
            return tc.ExecuteReader("SELECT * FROM View_NOADetail WHERE NOAID=%n and SupplierID=%n ORDER BY  ProductID", nNOAId, nContractorID);
        }
        public static IDataReader Gets(TransactionContext tc, string sSQL)
        {
            return tc.ExecuteReader(sSQL);
        }
        #endregion
    }  
    
   
    
  
}
