using System;
using System.Data;
using System.Data.SqlClient;
using ESimSol.BusinessObjects;
using System.Data.OleDb;
using ICS.Core.DataAccess;
using ICS.Core.Utility;


namespace ESimSol.Services.DataAccess
{
    public class ImportLCDetailProductDA
    {
        public ImportLCDetailProductDA() { }


        #region New Version By Mohammad Shahjada Sagor on 02 March 2014

        #region Insert, Update, Delete

        //public static IDataReader InsertUpdate(TransactionContext tc, PurchasePaymentContractDetail oPurchasePaymentContractDetail, EnumDBOperation eEnumDBPurchaseLC, Int64 nUserId)
        //{
        //    return tc.ExecuteReader("EXEC [SP_IUD_PurchasePaymentContractDetail]"
        //                            + "%n,%n,%n,%n,%n,%n,%n,%n,%s,%n,%s",
        //                           oPurchasePaymentContractDetail.PPCDetailID, oPurchasePaymentContractDetail.PPCID, oPurchasePaymentContractDetail.ProductID, oPurchasePaymentContractDetail.Quantity, oPurchasePaymentContractDetail.MeasurementUnitID, oPurchasePaymentContractDetail.GrossOrNetWeight, oPurchasePaymentContractDetail.UnitPrice,oPurchasePaymentContractDetail.PackingQty,oPurchasePaymentContractDetail.Note,(int)eEnumDBPurchaseLC, "");
        //}

       
        //public static void Delete(TransactionContext tc, PurchasePaymentContractDetail oPurchasePaymentContractDetail, EnumDBOperation eEnumDBPurchaseLC, Int64 nUserId, string sPPCDetailIDS)
        //{
        //           tc.ExecuteReader("EXEC [SP_IUD_PurchasePaymentContractDetail]"
        //                            + "%n,%n,%n,%n,%n,%n,%n,%n,%s,%n,%s",
        //                           oPurchasePaymentContractDetail.PPCDetailID, oPurchasePaymentContractDetail.PPCID, oPurchasePaymentContractDetail.ProductID, oPurchasePaymentContractDetail.Quantity, oPurchasePaymentContractDetail.MeasurementUnitID, oPurchasePaymentContractDetail.GrossOrNetWeight, oPurchasePaymentContractDetail.UnitPrice, oPurchasePaymentContractDetail.PackingQty, oPurchasePaymentContractDetail.Note, (int)eEnumDBPurchaseLC, sPPCDetailIDS);
        //}



        #endregion

        #endregion

        #region Old Version

        #region Insert Function
        //public static void Insert(TransactionContext tc, PurchasePaymentContractDetail oPPD)
        //{
        //    tc.ExecuteNonQuery("INSERT INTO PurchasePaymentContractDetail(PPCDetailID, PPCID, ProductID, Quantity, MeasurementUnitID, GrossOrNetWeight, UnitPrice, PackingQty, Note)"
        //        + " VALUES(%n, %n, %n, %n, %n, %n, %n, %n, %s)",
        //        oPPD.PPCDetailID, oPPD.PPCID, oPPD.ProductID, oPPD.Quantity, oPPD.MeasurementUnitID, oPPD.GrossOrNetWeight, oPPD.UnitPrice, oPPD.PackingQty, oPPD.Note);
        //}
        #endregion

        #region Update Function
        //public static void Update(TransactionContext tc, PurchasePaymentContractDetail oPPCD)
        //{
        //    tc.ExecuteNonQuery("UPDATE PurchasePaymentContractDetail SET PPCID=%n, ProductID=%n, Quantity=%n, MeasurementUnitID=%n, GrossOrNetWeight=%n, UnitPrice=%n, PackingQty=%n, Note=%s"
        //                    + " WHERE PPCDetailID=%n", oPPCD.PPCID, oPPCD.ProductID, oPPCD.Quantity, oPPCD.MeasurementUnitID, oPPCD.GrossOrNetWeight, oPPCD.UnitPrice, oPPCD.PackingQty, oPPCD.Note, oPPCD.PPCDetailID);
        //}
        #endregion

        #region Delete Function
        //public static void Delete(TransactionContext tc, int nID)
        //{
        //    tc.ExecuteNonQuery("DELETE FROM PurchasePaymentContractDetail WHERE PPCDetailID=%n", nID);
        //}
        #endregion

        #region Generation Function
        //public static int GetNewID(TransactionContext tc)
        //{
        //    return tc.GenerateID("PurchasePaymentContractDetail", "PPCDetailID");
        //}
        #endregion

        #endregion


        #region Get & Exist Function
        public static IDataReader Get(TransactionContext tc, int nID)
        {
            return tc.ExecuteReader("SELECT * FROM View_PurchasePaymentContractDetail WHERE PPCDetailID=%n", nID);
        }
        public static IDataReader Gets(TransactionContext tc)
        {
            return tc.ExecuteReader("SELECT * FROM View_PurchasePaymentContractDetail Order By [PPCDetailID]");
        }
        public static IDataReader Gets(TransactionContext tc, int nPPCID)
        {
            return tc.ExecuteReader("SELECT * FROM View_PurchasePaymentContractDetail where PPCID=%n Order By [PPCDetailID]", nPPCID);
        }
        public static IDataReader GetsByLCID(TransactionContext tc, int nLCID)
        {
            string sSQL = "Select * from View_PurchasePaymentContractDetail where PPCID in (Select PurchasePaymentContract.PPCID from PurchasePaymentContract where Activity=1 and PurchaseLCID= " + nLCID + " and VersionNumber=(Select MAX(VersionNumber)  from View_PurchasePaymentContract where PurchaseLCID= " + nLCID + ") )";
            return tc.ExecuteReader(sSQL);
        }
        public static double GetValue(TransactionContext tc, int nPPCDetailID)
        {
            object obj = tc.ExecuteScalar("SELECT isnull(Quantity*UnitPrice,0) FROM View_PurchasePaymentContractDetail Where PPCDetailID=%n", nPPCDetailID);
            if (obj == null) return -1;
            return Convert.ToDouble(obj);
        }

        public static IDataReader GetsBySQL(TransactionContext tc, string sSQL)
        {
            return tc.ExecuteReader(sSQL);
        }

        #endregion
    }
   
    
}
