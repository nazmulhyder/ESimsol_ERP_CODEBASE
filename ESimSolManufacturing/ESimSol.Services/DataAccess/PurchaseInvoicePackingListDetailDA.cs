using System;
using System.Data;
using System.Data.SqlClient;
using ICS.Core.DataAccess;
using ESimSol.BusinessObjects;
using System.Data.OleDb;
using ICS.Core.Utility;

namespace ESimSol.Services.DataAccess
{
    public class PurchaseInvoicePackingListDetailDA
    {
        public PurchaseInvoicePackingListDetailDA() { }

        #region Insert Function
        public static IDataReader InsertUpdate(TransactionContext tc, PurchaseInvoicePackingListDetail oPPLD, EnumDBOperation eEnumDBOperation, Int64 nUserID,String sPPLDetails)
        {
            return tc.ExecuteReader("EXEC [SP_IUD_PurchaseInvoicePackingListDetail]" + "%n, %n, %n, %n, %s,%n, %n",
                                    oPPLD.PurchaseInvoicePackingListDetailID, oPPLD.PurchaseInvoicePackingListID, oPPLD.NoOfBag, oPPLD.WeightPerBag,oPPLD.BagDes, (int)eEnumDBOperation, nUserID);
        }
        public static void Delete(TransactionContext tc, PurchaseInvoicePackingListDetail oPPLD, EnumDBOperation eEnumDBOperation, Int64 nUserID, String sPPLDetails)
        {
            tc.ExecuteNonQuery("EXEC [SP_IUD_PurchaseInvoicePackingListDetail]" + "%n, %n, %n, %n, %s,%n, %n",
                                           oPPLD.PurchaseInvoicePackingListDetailID, oPPLD.PurchaseInvoicePackingListID, oPPLD.NoOfBag, oPPLD.WeightPerBag, oPPLD.BagDes, (int)eEnumDBOperation, nUserID);
        }
        #endregion
       
        #region Generation Function
        public static int GetNewID(TransactionContext tc)
        {
            return tc.GenerateID("PurchaseInvoicePackingListDetail", "PurchaseInvoicePackingListDetailID");
        }
        #endregion

        #region Get & Exist Function
        public static IDataReader Get(TransactionContext tc, int nID)
        {
            return tc.ExecuteReader("SELECT * FROM PurchaseInvoicePackingListDetail WHERE PurchaseInvoicePackingListDetailID=%n", nID);
        }
        public static IDataReader Gets(TransactionContext tc)
        {
            return tc.ExecuteReader("SELECT * FROM PurchaseInvoicePackingListDetail Order By [PurchaseInvoicePackingListDetailID]");
        }


        public static IDataReader Gets(TransactionContext tc, int nPPListID)
        {
            return tc.ExecuteReader("SELECT * FROM PurchaseInvoicePackingListDetail WHERE PurchaseInvoicePackingListID=%n", nPPListID);
        }

        #endregion
    }


}
