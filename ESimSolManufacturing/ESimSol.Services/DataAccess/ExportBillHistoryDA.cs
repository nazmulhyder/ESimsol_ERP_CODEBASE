using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using ESimSol.BusinessObjects;
using ESimSol.Services.DataAccess;
using ICS.Core.DataAccess;
using ICS.Core.Utility;
using ICS.Core.Framework;


namespace ESimSol.Services.DataAccess
{
    public class ExportBillHistoryDA
    {
        public ExportBillHistoryDA() { }

        #region Insert Function
        public static IDataReader InsertUpdateHistory_SendToBuyer(TransactionContext tc, ExportBill oExportBill, EnumDBOperation eENumDBOperation, Int64 nUserId)
        {
            return tc.ExecuteReader("EXEC [SP_IUD_ExportBill_SendToBuyer]"
                                    + "%n,%n,%D,%s,%n,%n",
                                    oExportBill.ExportBillID,
                                    (int)oExportBill.State,
                                    oExportBill.SendToParty,
                                    oExportBill.NoteCarry,
                                    nUserId,
                                    (int)eENumDBOperation);
        }
        public static IDataReader InsertUpdateHistory(TransactionContext tc, ExportBill oExportBill, EnumDBOperation eENumDBOperation, Int64 nUserId)
        {
            return tc.ExecuteReader("EXEC [SP_IUD_ExportBillHistory]"
                                    + "%n, %n, %D, %D, %D, %D, %s, %D, %n, %D, %s, %n, %n",
                                    oExportBill.ExportBillID,
                                    (int)oExportBill.State,
                                    oExportBill.SendToParty,
                                    oExportBill.RecdFromParty,
                                    oExportBill.SendToBankDate,
                                    oExportBill.RecedFromBankDate,
                                    oExportBill.LDBCNo,
                                    oExportBill.LDBCDate,
                                    oExportBill.AcceptanceRate,
                                    oExportBill.BankFDDRecDate,
                                    oExportBill.NoteCarry,
                                    nUserId,
                                    (int)eENumDBOperation);
        }
        public static IDataReader InsertUpdateMaturityReceived(TransactionContext tc, ExportBill oExportBill, EnumDBOperation eENumDBOperation, Int64 nUserId)
        {
            return tc.ExecuteReader("EXEC [SP_IUD_ExportBillMaturityReceived]"
                                    + "%n, %n, %D, %n, %D, %D, %n, %s, %n, %n",
                                    oExportBill.ExportBillID,
                                    (int)oExportBill.State,
                                    oExportBill.AcceptanceDate,
                                    oExportBill.AcceptanceRate,
                                    oExportBill.MaturityReceivedDate,
                                    oExportBill.MaturityDate,
                                    oExportBill.LCTramsID,
                                    oExportBill.NoteCarry,
                                    nUserId,
                                    (int)eENumDBOperation);
        }
        public static IDataReader InsertUpdateSAN(TransactionContext tc, ExportBill oExportBill, EnumDBOperation eENumDBOperation, Int64 nUserId)
        {
            return tc.ExecuteReader("EXEC [SP_IUD_ExportBill_SendToBuyer]"
                                      + "%n,%n,%D,%s,%n,%n",
                                      oExportBill.ExportBillID,
                                      (int)oExportBill.State,
                                     NullHandler.GetNullValue(oExportBill.SendToParty),
                                      oExportBill.NoteCarry,
                                      nUserId,
                                      (int)eENumDBOperation);
        }
        public static IDataReader ExportBill_Encashment(TransactionContext tc, ExportBill oExportBill, Int64 nUserID)
        {
            return tc.ExecuteReader("EXEC [SP_ExportBill_Encashment]" + "%n, %n, %D, %n, %n, %n, %s, %n, %n, %n, %n, %n, %n,%n,%n,%n,%b,%n, %n",
                                   oExportBill.ExportBillID, (int)oExportBill.State, oExportBill.EncashmentDate, oExportBill.EncashCurrencyID, oExportBill.EncashCRate, oExportBill.EncashAmountBC, oExportBill.EncashRemarks, oExportBill.OverDueAmount, oExportBill.ExpBankAccountID, oExportBill.ExpCurrencyID, oExportBill.ExpAmount, oExportBill.ExpCRate, oExportBill.ExpAmountBC, oExportBill.DiscountAdjAmount, oExportBill.DiscountAdjCRate, oExportBill.DiscountAdjAmountBC, oExportBill.DiscountAdjIsGain, oExportBill.DiscountAdjGainLossBC, nUserID);
        }
        public static IDataReader InsertUpdate_Realized(TransactionContext tc, ExportBill oExportBill, EnumDBOperation eEnumDBOperation, Int64 nUserID)
        {
            return tc.ExecuteReader("EXEC [SP_IUD_ExportBillHistory_Realized]" + "%n,%n,%d,%s,%n,%n",
                                   oExportBill.ExportBillID, (int)oExportBill.State, oExportBill.RelizationDate, oExportBill.NoteCarry, nUserID, (int)eEnumDBOperation);
        }
      
        #endregion

       

        #region Delete Function
        public static void Delete(TransactionContext tc, ExportBillHistory oExportBillHistory)
        {
            tc.ExecuteNonQuery("DELETE FROM ExportBillHistory WHERE ExportBillHistoryID=%n", oExportBillHistory.ExportBillHistoryID);
        }
        public static void DeleteByBill(TransactionContext tc, int nID)
        {
            tc.ExecuteNonQuery("DELETE FROM ExportBillHistory WHERE LCBillID=%n", nID);
        }
        #endregion
        #region Generation Function
        public static int GetNewID(TransactionContext tc)
        {
            return tc.GenerateID("ExportBillHistory", "ExportBillHistoryID");
        }
        #endregion


        #region Get & Exist Function
        public static IDataReader Get(TransactionContext tc, int nID)
        {
            return tc.ExecuteReader("SELECT * FROM View_ExportBillHistory LH WHERE LH.ExportBillHistoryID=%n", nID);
        }
        public static IDataReader Getby(TransactionContext tc, int nLCBillID, int eEvent)
        {
            return tc.ExecuteReader("SELECT * FROM View_ExportBillHistory WHERE ExportBillID=%n AND [State]=%n", nLCBillID, eEvent);
        }

        public static IDataReader Gets(TransactionContext tc, int nExportBillID)
        {
            return tc.ExecuteReader("SELECT * FROM View_ExportBillHistory LH WHERE LH.ExportBillID=%n order by DBServerDateTime", nExportBillID);
        }
        public static int GetBillHisEven(TransactionContext tc, int nLBillID, int eBillEvent)
        {
            object obj = tc.ExecuteScalar("SELECT isnull(State,0) FROM View_ExportBillHistory WHERE ExportBillID=%n and [State]=%n", nLBillID, eBillEvent);
            if (obj == null) return 0;
            int aaaa = 0;
            aaaa = Convert.ToInt32(obj);
            return aaaa;
        }
        public static IDataReader InsertUpdateHistory_Manualy(TransactionContext tc, ExportBill oExportBill, EnumDBOperation eENumDBOperation, Int64 nUserId)
        {
            return tc.ExecuteReader("EXEC [SP_IUD_ExportBillHistory_Update]" + "%n,%n,%s,%n,%n", oExportBill.ExportBillID, (int)oExportBill.StateInt, oExportBill.NoteCarry, nUserId, (int)eENumDBOperation);
        }
     
        #endregion
    }
}
