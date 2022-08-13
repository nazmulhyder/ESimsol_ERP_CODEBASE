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
    public class LCTransferDetailDA
    {
        public LCTransferDetailDA() { }

        #region Insert Update Delete Function
        public static IDataReader InsertUpdate(TransactionContext tc, LCTransferDetail oLCTransferDetail, EnumDBOperation eEnumDBOperation, Int64 nUserID, String sLCTransferDetailIDs)
        {
            return tc.ExecuteReader("EXEC [SP_IUD_LCTransferDetail]" + "%n, %n, %n, %n, %n, %n, %n, %n, %n, %n,  %n, %n, %n, %n, %s", 
                                    oLCTransferDetail.LCTransferDetailID, oLCTransferDetail.LCTransferID, oLCTransferDetail.ProformaInvoiceDetailID, oLCTransferDetail.TechnicalSheetID, oLCTransferDetail.OrderRecapID, oLCTransferDetail.TransferQty, oLCTransferDetail.FOB, oLCTransferDetail.Amount, oLCTransferDetail.CommissionInPercent, oLCTransferDetail.FactoryFOB,   oLCTransferDetail.CommissionPerPcs, oLCTransferDetail.CommissionAmount, nUserID, (int)eEnumDBOperation, "");
        }
        public static void Delete(TransactionContext tc, LCTransferDetail oLCTransferDetail, EnumDBOperation eEnumDBOperation, Int64 nUserID, String sLCTransferDetailIDs)
        {
            tc.ExecuteNonQuery("EXEC [SP_IUD_LCTransferDetail]" + "%n, %n, %n, %n, %n, %n, %n, %n, %n, %n,  %n, %n, %n, %n, %s",
                                    oLCTransferDetail.LCTransferDetailID, oLCTransferDetail.LCTransferID, oLCTransferDetail.ProformaInvoiceDetailID, oLCTransferDetail.TechnicalSheetID, oLCTransferDetail.OrderRecapID, oLCTransferDetail.TransferQty, oLCTransferDetail.FOB, oLCTransferDetail.Amount, oLCTransferDetail.CommissionInPercent, oLCTransferDetail.FactoryFOB, oLCTransferDetail.CommissionPerPcs, oLCTransferDetail.CommissionAmount, nUserID, (int)eEnumDBOperation, sLCTransferDetailIDs);
        }
        #endregion

        #region Get & Exist Function
        public static IDataReader Get(TransactionContext tc, long nID)
        {
            return tc.ExecuteReader("SELECT * FROM View_LCTransferDetail WHERE LCTransferDetailID=%n", nID);
        }
        public static IDataReader Gets(int id, TransactionContext tc)
        {
            return tc.ExecuteReader("SELECT * FROM View_LCTransferDetail WHERE LCTransferID =%n", id);
        }
        public static IDataReader GetsLog(int id, TransactionContext tc)
        {
            return tc.ExecuteReader("SELECT * FROM View_LCTransferDetailLog WHERE  LCTransferLogID = %n", id);
        }    
        
        public static IDataReader Gets(TransactionContext tc, string sSQL)
        {
            return tc.ExecuteReader(sSQL);
        }
        #endregion
    }
}
