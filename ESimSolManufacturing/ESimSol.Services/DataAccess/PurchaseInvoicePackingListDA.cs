using System;
using System.Data;
using System.Data.SqlClient;
using ICS.Core.DataAccess;
using ESimSol.BusinessObjects;
using System.Data.OleDb;
using ICS.Core.Utility;

namespace ESimSol.Services.DataAccess
{
    public class PurchaseInvoicePackingListDA
    {
        public PurchaseInvoicePackingListDA() { }

        #region Insert Function
        public static IDataReader InsertUpdate(TransactionContext tc, PurchaseInvoicePackingList oPPL, EnumDBOperation eEnumDBOperation, Int64 nUserID)
        {
            return tc.ExecuteReader("EXEC [SP_IUD_PurchaseInvoicePackingList]" + "%n, %n, %n, %n,  %s, %n, %n",
                                    oPPL.PurchaseInvoicePackingListID, oPPL.PurchaseInvoiceLCDetailID, oPPL.PurchaseInvoiceDetailID, oPPL.MUnitID, oPPL.LotNo, (int)eEnumDBOperation, nUserID);
        }
        public static void Delete(TransactionContext tc, PurchaseInvoicePackingList oPPL, EnumDBOperation eEnumDBOperation, Int64 nUserID)
        {
            tc.ExecuteNonQuery("EXEC [SP_IUD_PurchaseInvoicePackingList]" + "%n, %n, %n, %n,  %s, %n, %n",
                                     oPPL.PurchaseInvoicePackingListID, oPPL.PurchaseInvoiceLCDetailID, oPPL.PurchaseInvoiceDetailID, oPPL.MUnitID, oPPL.LotNo, (int)eEnumDBOperation, nUserID);
        }
      
        #endregion

    

        #region Delete Function
        public static void Delete(TransactionContext tc, PurchaseInvoicePackingList oPurchaseInvoicePackingList, Int64 nUserId)
        {
            tc.ExecuteNonQuery("EXEC [SP_IUD_PurchaseInvoicePackingList]"
                                    + "%n, %s, %b, %n, %n",
                                    oPurchaseInvoicePackingList.PurchaseInvoicePackingListID,  nUserId, 3);
        }
        #endregion

        #region Generation Function
        public static int GetNewID(TransactionContext tc)
        {
            return tc.GenerateID("PurchaseInvoicePackingList", "PurchaseInvoicePackingListID");
        }
        #endregion

        #region Get & Exist Function
        public static IDataReader Get(TransactionContext tc, int nID)
        {
            return tc.ExecuteReader("SELECT * FROM PurchaseInvoicePackingList WHERE PurchaseInvoicePackingListID=%n", nID);
        }
        public static IDataReader Gets(TransactionContext tc)
        {
            return tc.ExecuteReader("SELECT * FROM PurchaseInvoicePackingList Order By [PurchaseInvoicePackingListID]");
        }
        public static IDataReader GetsBy(TransactionContext tc, int nPurchaseInvoiceLCDetailID)
        {
            return tc.ExecuteReader("Select * from View_PurchaseInvoicePackingList where PurchaseInvoiceLCDetailID in (Select PurchaseInvoiceLC.PurchaseInvoiceLCID from PurchaseInvoiceLC where PurchaseInvoiceLCID=%n ) order by ProductID", nPurchaseInvoiceLCDetailID);
        }
        public static IDataReader Gets(TransactionContext tc, int nPurchaseInvoiceLCDetailID)
        {
            return tc.ExecuteReader("Select * from View_PurchaseInvoicePackingList where PurchaseInvoiceLCDetailID=%n order by ProductID", nPurchaseInvoiceLCDetailID);
        }

        #endregion
    }


}
