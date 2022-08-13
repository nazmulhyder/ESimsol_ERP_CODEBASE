using System;
using System.Data;
using System.Data.SqlClient;
using ESimSol.BusinessObjects;
using System.Data.OleDb;
using ICS.Core.DataAccess;
using ICS.Core.Utility;

namespace ESimSol.Services.DataAccess
{
    public class ImportInvoiceDA
    {
        public ImportInvoiceDA() { }

        #region New Version By Mohammad Shahjada Sagor on 18 March 2014

        #region Insert, Update, Delete

        public static IDataReader InsertUpdate(TransactionContext tc, ImportInvoice oImportInvoice, EnumDBOperation eENumDBPurchaseInvoice, Int64 nUserId)
         {
            return tc.ExecuteReader("EXEC [SP_IUD_ImportInvoice]"
                                    + "%n,%s,%s,%n,%d,%n,%n,%d,%n,%n,%n,%n,%n,%n, %s, %n,%n",
                                    oImportInvoice.ImportInvoiceID,
                                    oImportInvoice.FileNo,
                                    oImportInvoice.ImportInvoiceNo,
                                    oImportInvoice.InvoiceType,
                                    oImportInvoice.DateofInvoice,
                                    oImportInvoice.ImportLCID,
                                    oImportInvoice.ContractorID,
                                    oImportInvoice.DateofReceive,
                                    oImportInvoice.Amount,
                                    oImportInvoice.CurrencyID,
                                    oImportInvoice.BankStatus,
                                    oImportInvoice.InvoiceStatus,
                                    oImportInvoice.ProductionApprovalStatus,
                                    oImportInvoice.BUID,
                                    oImportInvoice.Remarks,
                                    nUserId,
                                    (int)eENumDBPurchaseInvoice);
        }
        public static void Delete(TransactionContext tc, ImportInvoice oImportInvoice, EnumDBOperation eENumDBPurchaseInvoice, Int64 nUserId)
        {
            tc.ExecuteNonQuery("EXEC [SP_IUD_ImportInvoice]"
                                    + "%n,%s,%s,%n,%d,%n,%n,%d,%n,%n,%n,%n,%n,%n, %s, %n,%n",
                                    oImportInvoice.ImportInvoiceID,
                                    oImportInvoice.FileNo,
                                    oImportInvoice.ImportInvoiceNo,
                                    oImportInvoice.InvoiceType,
                                    oImportInvoice.DateofInvoice,
                                    oImportInvoice.ImportLCID,
                                    oImportInvoice.ContractorID,
                                    oImportInvoice.DateofReceive,
                                    oImportInvoice.Amount,
                                    oImportInvoice.CurrencyID,
                                    oImportInvoice.BankStatus,
                                    oImportInvoice.InvoiceStatus,
                                    oImportInvoice.ProductionApprovalStatus,
                                    oImportInvoice.BUID,
                                    oImportInvoice.Remarks,
                                    nUserId,
                                    (int)eENumDBPurchaseInvoice);
        }
        public static void UpdateStatus(TransactionContext tc, ImportInvoice oImportInvoice, Int64 nUserId)
        {
            tc.ExecuteNonQuery("Update ImportInvoice set CurrentStatus=2 where ImportInvoiceID=%n",oImportInvoice.ImportInvoiceID  );
        }
        public static void UpdateCommission(TransactionContext tc, ImportInvoice oImportInvoice, Int64 nUserId)
        {
            tc.ExecuteNonQuery("Update ImportInvoice set CommissionInPercent=%n, Commission=%n, CommissionRemarks=%s where ImportInvoiceID=%n",oImportInvoice.CommissionInPercent, oImportInvoice.Commission, oImportInvoice.CommissionRemarks, oImportInvoice.ImportInvoiceID);
        }
        public static IDataReader InsertUpdatePIPayment(TransactionContext tc, ImportInvoice oPurchaseInvoice, EnumDBOperation eDBOperation, Int64 nUserId)
        {
            return tc.ExecuteReader("EXEC [SP_IUD_PurchaseInvoicePayment]"
                                    + "%n,%n,%s,%D,%D,%n,%n,%n,%s,%n,%n",
                                    oPurchaseInvoice.ImportInvoiceID,
                                    oPurchaseInvoice.BankStatus,
                                    oPurchaseInvoice.BankStatus,
                                    oPurchaseInvoice.BankStatus,
                                    oPurchaseInvoice.LiabilityNo,
                                    oPurchaseInvoice.DateofPayment,
                                    oPurchaseInvoice.DateofMaturity,
                                    oPurchaseInvoice.CurrencyID,
                                    oPurchaseInvoice.CCRate,
                                    oPurchaseInvoice.Amount,
                                    oPurchaseInvoice.Remark_Pament,
                                    nUserId,
                                    (int)eDBOperation);
        }     
        public static IDataReader InsertUpdatePILCHistory(TransactionContext tc, ImportInvoice oImportInvoice, EnumDBOperation eENumDBPurchaseInvoice, Int64 nUserId)
        {
            return tc.ExecuteReader("EXEC [SP_IUD_ImportInvoiceHistory]"
                                    + "%n,%n,%n,%n,%D,%D,%D,%D,%D,%n,%s,%s,%D,%s,%D,%n,%n",
                                    oImportInvoice.ImportInvoiceID,
                                    oImportInvoice.BankStatus,
                                    oImportInvoice.InvoiceStatus,
                                    oImportInvoice.ProductionApprovalStatus,
                                    oImportInvoice.DateofBankInfo,
                                    oImportInvoice.DateOfTakeOutDoc,
                                    oImportInvoice.DateofNegotiation,
                                    oImportInvoice.DateofAcceptance,
                                    NullHandler.GetNullValue(oImportInvoice.DateofMaturity),
                                    oImportInvoice.CRate_Acceptance,
                                    oImportInvoice.CommonRemarks,//All Remarsk Will 
                                    oImportInvoice.ABPNo,
                                    oImportInvoice.CommonDate,//Common Date
                                    oImportInvoice.BillofEntryNo,
                                    oImportInvoice.BillofEntryDate,
                                    nUserId,
                                    (int)eENumDBPurchaseInvoice);
        }
        public static IDataReader InsertUpdateInvoiceStatus(TransactionContext tc, ImportInvoice oImportInvoice, EnumDBOperation eENumDBPurchaseInvoice, Int64 nUserId)
        {
            return tc.ExecuteReader("EXEC [SP_IUD_ImportInvoiceHistory_Update]"
                                    + "%n,%n,%n,%s,%n,%n",
                                    oImportInvoice.ImportInvoiceID,
                                    (int)oImportInvoice.BankStatus,
                                    (int)oImportInvoice.InvoiceStatus,
                                    oImportInvoice.CommonRemarks,//All Remarsk Will 
                                    nUserId,
                                    (int)eENumDBPurchaseInvoice);
        }
        public static IDataReader UpdateAmount(TransactionContext tc, ImportInvoice oImportInvoice)
        {
            string sSQL1 = SQLParser.MakeSQL("Update ImportInvoice Set Amount=%n WHERE ImportInvoiceID=%n", oImportInvoice.Amount, oImportInvoice.ImportInvoiceID);
            tc.ExecuteNonQuery(sSQL1);
            return tc.ExecuteReader("SELECT * FROM View_ImportInvoice WHERE ImportInvoiceID=%n", oImportInvoice.ImportInvoiceID);
        }

        #endregion



        #region Get & Exist Function


        public static IDataReader Get(int nImportInvoiceID, TransactionContext tc)
        {
            return tc.ExecuteReader("select * from View_ImportInvoice where ImportInvoiceID=%n", nImportInvoiceID);
        }
        public static IDataReader Get(int nInvoiceType, int nImportPIID, TransactionContext tc)
        {
            return tc.ExecuteReader("SELECT * FROM View_ImportInvoice where InvoiceType=%n and ImportPIID=%n",  nInvoiceType,  nImportPIID);
        }
        public static IDataReader Gets(int nImportLCID, TransactionContext tc)
        {
            return tc.ExecuteReader("select * from View_ImportInvoice where ImportLCID=%n", nImportLCID);
        }

        public static IDataReader Gets(TransactionContext tc)
        {
            return tc.ExecuteReader("SELECT * FROM View_ImportInvoice");
        }

        public static IDataReader Gets(TransactionContext tc, string sSQL)
        {
            return tc.ExecuteReader(sSQL);
        }
        public static bool SaveDeliveryNotice(TransactionContext tc, ImportInvoice oImportInvoice, Int64 nUserID)
        {
            try
            {
                tc.ExecuteNonQuery("Update  ImportInvoice SET DeliveryNoticeDate =%D WHERE ImportInvoiceID =%n",NullHandler.GetNullValue(oImportInvoice.DeliveryNoticeDate), oImportInvoice.ImportInvoiceID); 
            }
            catch
            {
                return false;
            }
            return true;
        }
        #endregion
        #endregion
    }

}
