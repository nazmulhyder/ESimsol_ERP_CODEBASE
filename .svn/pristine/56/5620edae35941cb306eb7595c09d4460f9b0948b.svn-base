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
    public class CommercialInvoiceDA
    {
        public CommercialInvoiceDA() { }

        #region Insert Update Delete Function
        public static IDataReader InsertUpdate(TransactionContext tc, CommercialInvoice oCommercialInvoice, EnumDBOperation eEnumDBOperation, Int64 nUserID)
        {
            return tc.ExecuteReader("EXEC[SP_IUD_CommercialInvoice]" + "%n, %n, %n, %s, %b, %d, %n, %n, %n, %n, %n,  %n, %n, %s, %s, %s, %s, %s, %d, %n,%n, %n,%n, %n, %n",
                                    oCommercialInvoice.CommercialInvoiceID, oCommercialInvoice.LCTransferID, oCommercialInvoice.MasterLCID, oCommercialInvoice.InvoiceNo, oCommercialInvoice.IsSystemGeneratedInvoiceNo, oCommercialInvoice.InvoiceDate, (int)oCommercialInvoice.InvoiceStatus, oCommercialInvoice.BuyerID, oCommercialInvoice.ProductionFactoryID, oCommercialInvoice.InvoiceAmount, oCommercialInvoice.DiscountAmount, oCommercialInvoice.AdditionAmount,  oCommercialInvoice.NetInvoiceAmount, oCommercialInvoice.Note, oCommercialInvoice.ReceiptNo, oCommercialInvoice.TransportNo, oCommercialInvoice.DriverName, oCommercialInvoice.Carrier, oCommercialInvoice.DeliveryDate, oCommercialInvoice.ApprovedBy,  (int)oCommercialInvoice.ShipmentMode, (int)oCommercialInvoice.CIFormat, oCommercialInvoice.AnnualBonus,   nUserID, (int)eEnumDBOperation);
        }

        public static void Delete(TransactionContext tc, CommercialInvoice oCommercialInvoice, EnumDBOperation eEnumDBOperation, Int64 nUserID)
        {
            tc.ExecuteNonQuery("EXEC[SP_IUD_CommercialInvoice]" + "%n, %n, %n, %s, %b, %d, %n, %n, %n, %n, %n,  %n, %n, %s, %s, %s, %s, %s, %d, %n,%n, %n,%n, %n, %n",
                                    oCommercialInvoice.CommercialInvoiceID, oCommercialInvoice.LCTransferID, oCommercialInvoice.MasterLCID, oCommercialInvoice.InvoiceNo, oCommercialInvoice.IsSystemGeneratedInvoiceNo, oCommercialInvoice.InvoiceDate, (int)oCommercialInvoice.InvoiceStatus, oCommercialInvoice.BuyerID, oCommercialInvoice.ProductionFactoryID, oCommercialInvoice.InvoiceAmount, oCommercialInvoice.DiscountAmount, oCommercialInvoice.AdditionAmount, oCommercialInvoice.NetInvoiceAmount, oCommercialInvoice.Note, oCommercialInvoice.ReceiptNo, oCommercialInvoice.TransportNo, oCommercialInvoice.DriverName, oCommercialInvoice.Carrier, oCommercialInvoice.DeliveryDate, oCommercialInvoice.ApprovedBy, (int)oCommercialInvoice.ShipmentMode, (int)oCommercialInvoice.CIFormat, oCommercialInvoice.AnnualBonus, nUserID, (int)eEnumDBOperation);
        }

        public static void ChangeField(TransactionContext tc, CommercialInvoice oCommercialInvoice,Int64 nUserID)
        {
            string sSQL = "Update CommercialInvoice SET ";
            if (oCommercialInvoice.GSP) { sSQL += "GSP =" + (oCommercialInvoice.bIsChangeField == true ? "1" : "0"); }
            if (oCommercialInvoice.IC) { sSQL += "IC =" + (oCommercialInvoice.bIsChangeField == true ? "1" : "0"); }
            if (oCommercialInvoice.BL) { sSQL += "BL =" + (oCommercialInvoice.bIsChangeField == true ? "1" : "0"); }
            sSQL += " WHERE CommercialInvoiceID = "+oCommercialInvoice.CommercialInvoiceID;
            tc.ExecuteNonQuery(sSQL);
        }

    
        

        #endregion

        #region Get & Exist Function
        public static IDataReader Get(TransactionContext tc, long nID)
        {
            return tc.ExecuteReader("SELECT * FROM View_CommercialInvoice WHERE CommercialInvoiceID=%n", nID);
        }

        public static IDataReader Gets(TransactionContext tc)
        {
            return tc.ExecuteReader("SELECT * FROM View_CommercialInvoice");
        }

        public static IDataReader GetsByTransfer(TransactionContext tc, int id)
        {
            return tc.ExecuteReader("SELECT * FROM View_CommercialInvoice WHERE LCTransferID = %n",id);
        }
        public static IDataReader GetsByLC(TransactionContext tc, int id)
        {
            return tc.ExecuteReader("SELECT * FROM View_CommercialInvoice WHERE MasterLCID = %n", id);
        }
        

        public static IDataReader Gets(TransactionContext tc, string sSQL)
        {
            return tc.ExecuteReader(sSQL);
        }
        #endregion
    }
}
