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


    public class ProformaInvoiceDetailDA
    {
        public ProformaInvoiceDetailDA() { }

        #region Insert Update Delete Function
        public static IDataReader InsertUpdate(TransactionContext tc, ProformaInvoiceDetail oProformaInvoiceDetail, EnumDBOperation eEnumDBOperation, Int64 nUserID, String sProformaInvoiceDetailIDs)
        {
            return tc.ExecuteReader("EXEC [SP_IUD_ProformaInvoiceDetail]"
                                    + "%n, %n, %n, %n, %d, %n, %n, %n, %n, %n, %n, %n, %n, %n, %n, %s", oProformaInvoiceDetail.ProformaInvoiceDetailID, oProformaInvoiceDetail.ProformaInvoiceID, oProformaInvoiceDetail.OrderRecapID, oProformaInvoiceDetail.TechnicalSheetID, oProformaInvoiceDetail.ShipmentDate, oProformaInvoiceDetail.Quantity, oProformaInvoiceDetail.FOB, oProformaInvoiceDetail.BuyerCommissionInPercent, oProformaInvoiceDetail.BuyerCommission, oProformaInvoiceDetail.AdjustAdditon, oProformaInvoiceDetail.AdjustDeduction, oProformaInvoiceDetail.UnitPrice, oProformaInvoiceDetail.Amount, nUserID, (int)eEnumDBOperation, "");
        }

        public static void Delete(TransactionContext tc, ProformaInvoiceDetail oProformaInvoiceDetail, EnumDBOperation eEnumDBOperation, Int64 nUserID, String sProformaInvoiceDetailIDs)
        {
            tc.ExecuteNonQuery("EXEC [SP_IUD_ProformaInvoiceDetail]"
                                    + "%n, %n, %n, %n, %d, %n, %n, %n, %n, %n, %n, %n, %n, %n, %n, %s", oProformaInvoiceDetail.ProformaInvoiceDetailID, oProformaInvoiceDetail.ProformaInvoiceID, oProformaInvoiceDetail.OrderRecapID, oProformaInvoiceDetail.TechnicalSheetID, oProformaInvoiceDetail.ShipmentDate, oProformaInvoiceDetail.Quantity, oProformaInvoiceDetail.FOB, oProformaInvoiceDetail.BuyerCommissionInPercent, oProformaInvoiceDetail.BuyerCommission, oProformaInvoiceDetail.AdjustAdditon, oProformaInvoiceDetail.AdjustDeduction, oProformaInvoiceDetail.UnitPrice, oProformaInvoiceDetail.Amount, nUserID, (int)eEnumDBOperation, sProformaInvoiceDetailIDs);
        }


        #endregion

        #region Get & Exist Function
        public static IDataReader Get(TransactionContext tc, long nID)
        {
            return tc.ExecuteReader("SELECT * FROM View_ProformaInvoiceDetail WHERE ProformaInvoiceDetailID=%n", nID);
        }

        public static IDataReader Gets(int nProformaInvoiceID, TransactionContext tc)
        {
            return tc.ExecuteReader("SELECT * FROM View_ProformaInvoiceDetail where ProformaInvoiceID =%n", nProformaInvoiceID);
        }

        public static IDataReader GetsPILog(int nProformaInvoiceLogID, TransactionContext tc)
        {
            return tc.ExecuteReader("SELECT * FROM View_ProformaInvoiceDetailLog where ProformaInvoiceLogID =%n", nProformaInvoiceLogID);
        }

        public static IDataReader Gets(TransactionContext tc, string sSQL)
        {
            return tc.ExecuteReader(sSQL);
        }
        #endregion
    }
    
    
    
 
}
