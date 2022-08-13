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
    public class CommercialInvoiceDetailDA
    {
        public CommercialInvoiceDetailDA() { }

        #region Insert Update Delete Function
        public static IDataReader InsertUpdate(TransactionContext tc, CommercialInvoiceDetail oCommercialInvoiceDetail, EnumDBOperation eEnumDBOperation, Int64 nUserID, String sCommercialInvoiceDetailIDs)
        {
            return tc.ExecuteReader("EXEC [SP_IUD_CommercialInvoiceDetail]" + "%n, %n, %n, %n, %n, %d, %n, %n,%n, %n, %n, %s,%s,%n, %n, %n, %s",
                                    oCommercialInvoiceDetail.CommercialInvoiceDetailID, oCommercialInvoiceDetail.CommercialInvoiceID, oCommercialInvoiceDetail.ReferenceDetailID, oCommercialInvoiceDetail.TechnicalSheetID, oCommercialInvoiceDetail.OrderRecapID, oCommercialInvoiceDetail.ShipmentDate, oCommercialInvoiceDetail.InvoiceQty, oCommercialInvoiceDetail.UnitPrice, oCommercialInvoiceDetail.Discount, oCommercialInvoiceDetail.FOB, oCommercialInvoiceDetail.Amount, oCommercialInvoiceDetail.HSCode, oCommercialInvoiceDetail.CAT, oCommercialInvoiceDetail.CartonQty, nUserID, (int)eEnumDBOperation, sCommercialInvoiceDetailIDs);
        }
        public static void Delete(TransactionContext tc, CommercialInvoiceDetail oCommercialInvoiceDetail, EnumDBOperation eEnumDBOperation, Int64 nUserID, String sCommercialInvoiceDetailIDs)
        {
            tc.ExecuteNonQuery("EXEC [SP_IUD_CommercialInvoiceDetail]" + "%n, %n, %n, %n, %n, %d, %n, %n,%n, %n, %n, %s,%s,%n, %n, %n, %s",
                                    oCommercialInvoiceDetail.CommercialInvoiceDetailID, oCommercialInvoiceDetail.CommercialInvoiceID, oCommercialInvoiceDetail.ReferenceDetailID, oCommercialInvoiceDetail.TechnicalSheetID, oCommercialInvoiceDetail.OrderRecapID, oCommercialInvoiceDetail.ShipmentDate, oCommercialInvoiceDetail.InvoiceQty, oCommercialInvoiceDetail.UnitPrice, oCommercialInvoiceDetail.Discount, oCommercialInvoiceDetail.FOB, oCommercialInvoiceDetail.Amount, oCommercialInvoiceDetail.HSCode, oCommercialInvoiceDetail.CAT, oCommercialInvoiceDetail.CartonQty,  nUserID, (int)eEnumDBOperation, sCommercialInvoiceDetailIDs);
        }
        #endregion

        #region Get & Exist Function
        public static IDataReader Get(TransactionContext tc, long nID)
        {
            return tc.ExecuteReader("SELECT * FROM View_CommercialInvoiceDetail WHERE CommercialInvoiceDetailID=%n", nID);
        }
        public static IDataReader Gets(int nCommercialInvoiceID, TransactionContext tc)
        {
            return tc.ExecuteReader("SELECT * FROM View_CommercialInvoiceDetail where CommercialInvoiceID =%n", nCommercialInvoiceID);
        }
        public static IDataReader Gets(TransactionContext tc, string sSQL)
        {
            return tc.ExecuteReader(sSQL);
        }
        #endregion
    }
}
