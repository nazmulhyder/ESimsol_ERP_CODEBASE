using System;
using System.Data;
using System.Data.SqlClient;
using ICS.Core.DataAccess;
using ESimSol.BusinessObjects;
using System.Data.OleDb;
using ICS.Core.Utility;

namespace ESimSol.Services.DataAccess
{
    public class PurchaseInvoiceDA
    {
        public PurchaseInvoiceDA() { }

        #region New Version 

        #region Insert, Update, Delete

        public static IDataReader InsertUpdate(TransactionContext tc, PurchaseInvoice oPurchaseInvoice, EnumDBOperation eEnumDBOperation, Int64 nUserId)
        {
            return tc.ExecuteReader("EXEC [SP_IUD_PurchaseInvoice]"
                                    + " %n,                                 %s,                                     %n,                     %s,                         %d,                             %d,                                 %n,                                 %n,                                         %n,                                 %n,                                                    %n,                             %n,                                     %n,                         %n,                             %n,                                 %d,                                 %s,                     %n,                              %d,                             %n,                                 %s,                         %s,                             %n,                            %n,                                         %n,                         %n,                                     %n,             %n,                       %n,                         %n,                                 %n,                             %n,                         %n,                                 %n,                            %n",
                                        oPurchaseInvoice.PurchaseInvoiceID, oPurchaseInvoice.PurchaseInvoiceNo,     oPurchaseInvoice.BUID,  oPurchaseInvoice.BillNo,    oPurchaseInvoice.DateofBill,    oPurchaseInvoice.DateofInvoice,     oPurchaseInvoice.InvoiceTypeInt,    oPurchaseInvoice.InvoicePaymentModeInt,     oPurchaseInvoice.InvoiceStatusInt,  oPurchaseInvoice.RefTypeInt,        oPurchaseInvoice.ContractorID,  oPurchaseInvoice.ContractorPersonalID,  oPurchaseInvoice.Amount,    oPurchaseInvoice.CurrencyID,    oPurchaseInvoice.ConvertionRate,    oPurchaseInvoice.DateofMaturity,    oPurchaseInvoice.Note,  oPurchaseInvoice.ApprovedBy,     oPurchaseInvoice.ApprovedDate,  oPurchaseInvoice.PaymentTermID,     oPurchaseInvoice.ShipBy,    oPurchaseInvoice.TradeTerm,     oPurchaseInvoice.DeliveryTo,   oPurchaseInvoice.DeliveryToContactPerson,   oPurchaseInvoice.BillTo,    oPurchaseInvoice.BIllToContactPerson,   nUserId,        (int)eEnumDBOperation,    oPurchaseInvoice.Discount,  oPurchaseInvoice.ServiceCharge,     oPurchaseInvoice.NetAmount,     oPurchaseInvoice.RateOn,    oPurchaseInvoice.PaymentMethodInt, oPurchaseInvoice.BankAccountID,  oPurchaseInvoice.ServiceChargeID);
        }
        public static void Delete(TransactionContext tc, PurchaseInvoice oPurchaseInvoice, EnumDBOperation eEnumDBOperation, Int64 nUserId)
        {
            tc.ExecuteNonQuery("EXEC [SP_IUD_PurchaseInvoice]"
                                    + " %n,                                 %s,                                     %n,                     %s,                         %d,                             %d,                                 %n,                                 %n,                                         %n,                                                             %n,                         %n,                             %n,                                     %n,                         %n,                             %n,                                 %d,                                 %s,                     %n,                              %d,                             %n,                                 %s,                         %s,                             %n,                            %n,                                         %n,                         %n,                                     %n,             %n,                       %n,                         %n,                                 %n,                             %n,                         %n,                                 %n, %n",
                                        oPurchaseInvoice.PurchaseInvoiceID, oPurchaseInvoice.PurchaseInvoiceNo, oPurchaseInvoice.BUID, oPurchaseInvoice.BillNo, oPurchaseInvoice.DateofBill, oPurchaseInvoice.DateofInvoice, oPurchaseInvoice.InvoiceTypeInt, oPurchaseInvoice.InvoicePaymentModeInt, oPurchaseInvoice.InvoiceStatusInt, oPurchaseInvoice.RefTypeInt,  oPurchaseInvoice.ContractorID, oPurchaseInvoice.ContractorPersonalID, oPurchaseInvoice.Amount, oPurchaseInvoice.CurrencyID, oPurchaseInvoice.ConvertionRate, oPurchaseInvoice.DateofMaturity, oPurchaseInvoice.Note, oPurchaseInvoice.ApprovedBy, oPurchaseInvoice.ApprovedDate, oPurchaseInvoice.PaymentTermID, oPurchaseInvoice.ShipBy, oPurchaseInvoice.TradeTerm, oPurchaseInvoice.DeliveryTo, oPurchaseInvoice.DeliveryToContactPerson, oPurchaseInvoice.BillTo, oPurchaseInvoice.BIllToContactPerson, nUserId, (int)eEnumDBOperation, oPurchaseInvoice.Discount, oPurchaseInvoice.ServiceCharge, oPurchaseInvoice.NetAmount, oPurchaseInvoice.RateOn, oPurchaseInvoice.PaymentMethodInt, oPurchaseInvoice.BankAccountID, oPurchaseInvoice.ServiceChargeID);
        }
        public static IDataReader Approve(TransactionContext tc, PurchaseInvoice oPurchaseInvoice, Int64 nUserId)
        {
            return tc.ExecuteReader("EXEC [SP_ApprovedPurchaseInvoice]" + "%n, %n", oPurchaseInvoice.PurchaseInvoiceID, nUserId);
        }
        public static IDataReader UndoApproved(TransactionContext tc, PurchaseInvoice oPurchaseInvoice, Int64 nUserId)
        {
            return tc.ExecuteReader("EXEC [SP_UndoApprovedPurchaseInvoice]" + "%n, %n", oPurchaseInvoice.PurchaseInvoiceID, nUserId);
        }
        
        public static void UpdatePaymentMode(TransactionContext tc, PurchaseInvoice oPurchaseInvoice)
        {
            tc.ExecuteNonQuery(" Update PurchaseInvoice SET InvoicePaymentMode = %n WHERE PurchaseInvoiceID  = %n", oPurchaseInvoice.InvoicePaymentModeInt, oPurchaseInvoice.PurchaseInvoiceID);
        }

        public static void UpdateVoucherEffect(TransactionContext tc, PurchaseInvoice oPurchaseInvoice)
        {
            tc.ExecuteNonQuery(" Update PurchaseInvoice SET IsWillVoucherEffect = %b WHERE PurchaseInvoiceID  = %n", oPurchaseInvoice.IsWillVoucherEffect, oPurchaseInvoice.PurchaseInvoiceID);
        }  

        
        
        #endregion

        #region Get & Exist Function


        public static IDataReader Get(int nPurchaseInvoiceID, TransactionContext tc)
        {
            return tc.ExecuteReader("select * from View_PurchaseInvoice where PurchaseInvoiceID=%n", nPurchaseInvoiceID);
        }
         public static IDataReader Get( int nRefID, int RefType, TransactionContext tc)/// Must Change It after Change Ref Type for LC
        {
            return tc.ExecuteReader("select * from View_PurchaseInvoice where PurchaseLCID=%n", nRefID);
        }
    
        public static IDataReader Gets(int nPurchaseLCID,TransactionContext tc)
        {
            return tc.ExecuteReader("select * from View_PurchaseInvoice where PurchaseLCID=%n", nPurchaseLCID);
        }
        public static IDataReader GetsbyPC(int nPCID, TransactionContext tc)
        {
            return tc.ExecuteReader("select * from View_PurchaseInvoice where PCID=%n", nPCID);
        }
        public static IDataReader Gets(TransactionContext tc)
        {
            return tc.ExecuteReader("SELECT * FROM View_PurchaseInvoice");
        }      
        public static IDataReader Gets(TransactionContext tc, string sSQL)
        {
            return tc.ExecuteReader(sSQL);
        }

        

        #endregion

        #endregion


        #region Old Version

        #region Insert Update Function
        //public static IDataReader InsertUpdate(TransactionContext tc, string sObjString, EnumDBOperation eEnumDBOperation, Int64 nUserId)
        //{
        //    return tc.ExecuteReader("EXEC [SP_IUD_PurchaseInvoice_Product] %s,%n,%n", sObjString, (int)eEnumDBOperation, nUserId);
        //}
        #endregion

        #region Delete Function
        //public static void Delete(TransactionContext tc, int nID)
        //{

        //    tc.ExecuteNonQuery("DELETE FROM PurchaseInvoice WHERE PurchaseInvoiceID=%n", nID);
        //}
        #endregion


        #region Status Update Function
        //public static void Update_Status(TransactionContext tc, int eInvoiceEvent, int nID)
        //{
        //    tc.ExecuteNonQuery("UPDATE PurchaseInvoice SET Status = %n WHERE PurchaseInvoiceID=%n",eInvoiceEvent, nID);
        //}
        //public static void Update_TakeOutDoc(TransactionContext tc, DateTime dDateOfTakeOutDoc, int eInvoiceEvent, int eInvoiceBankStatus, int nID)
        //{
        //    tc.ExecuteNonQuery("UPDATE PurchaseInvoice SET DateOfTakeOutDoc=%D, CurrentStatus = %n,BankStatus=%n WHERE PurchaseInvoiceID=%n", dDateOfTakeOutDoc,eInvoiceEvent, eInvoiceBankStatus, nID);
        //}
        //public static void Update_Acceptance(TransactionContext tc, PurchaseInvoice oPurchaseInvoice)
        //{
        //    tc.ExecuteNonQuery("UPDATE PurchaseInvoice SET  BankStatus = %n,NegotiationDate=%d, MaturityDate=%d, AcceptedOn=%d, AcceptedBy=%n,ExceedDays=%n WHERE PurchaseInvoiceID=%n", (int)oPurchaseInvoice.BankStatus, oPurchaseInvoice.NegotiationDate, oPurchaseInvoice.MaturityDate, oPurchaseInvoice.AcceptedOn, oPurchaseInvoice.AcceptedBy, oPurchaseInvoice.ExceedDays, oPurchaseInvoice.PurchaseInvoiceID);
        //}
        //public static void Update_PaymentRequest(TransactionContext tc, PurchaseInvoice oPurchaseInvoice)
        //{
        //    tc.ExecuteNonQuery("UPDATE PurchaseInvoice SET  BankStatus = %n,ApplicationDate=%d,Recommendation=%s WHERE PurchaseInvoiceID=%n", (int)oPurchaseInvoice.BankStatus, oPurchaseInvoice.ApplicationDate, oPurchaseInvoice.Recommendation, oPurchaseInvoice.PurchaseInvoiceID);
        //}
        //public static void PurchaseInvoicePaymentDone(TransactionContext tc, PurchaseInvoice oPurchaseInvoice, PurchaseInvoiceHistry oPurchaseInvoiceHistry, Int64 nUserId)
        //{
        //    tc.ExecuteScalar(CommandType.StoredProcedure, "[SP_PurchaseImportInvoicePaymentDone]", oPurchaseInvoice.PurchaseInvoiceID, oPurchaseInvoice.BillPaymentDate, oPurchaseInvoiceHistry.Note, nUserId);

        //}
        #endregion

        #region Generation Function
        //public static int GetNewID(TransactionContext tc)
        //{
        //    return tc.GenerateID("PurchaseInvoice", "PurchaseInvoiceID");
        //}
        #endregion



        //public static int GetPInvoiceCurrentStatus(TransactionContext tc, int nPInvoiceID)
        //{
        //    object obj = tc.ExecuteScalar("SELECT isnull(PurchaseInvoice.[CurrentStatus],0) FROM PurchaseInvoice WHERE PurchaseInvoiceID=%n ", nPInvoiceID);
        //    if (obj == null) return 0;
        //    int aaaa = 0;
        //    aaaa = Convert.ToInt32(obj);
        //    return aaaa;
        //}
        //public static int GetPInvoiceBankStatus(TransactionContext tc, int nPInvoiceID)
        //{
        //    object obj = tc.ExecuteScalar("SELECT isnull(BankStatus,0) FROM PurchaseInvoice WHERE PurchaseInvoiceID=%n ", nPInvoiceID);
        //    if (obj == null) return 0;
        //    int aaaa = 0;
        //    aaaa = Convert.ToInt32(obj);
        //    return aaaa;
        //}
        //public static int GetPInvoicePAprovalStatusStatus(TransactionContext tc, int nPInvoiceID)
        //{
        //    object obj = tc.ExecuteScalar("SELECT isnull(PurchaseInvoice.[ProductionApprovalStatus],0) FROM PurchaseInvoice WHERE PurchaseInvoiceID=%n ", nPInvoiceID);
        //    if (obj == null) return 0;
        //    int aaaa = 0;
        //    aaaa = Convert.ToInt32(obj);
        //    return aaaa;
        //}

        #region Get & Exist Function
        //public static IDataReader Get(TransactionContext tc, int nID)
        //{
        //    return tc.ExecuteReader("SELECT * FROM View_PurchaseInvoice WHERE PurchaseInvoiceID=%n", nID);
        //}
        //public static IDataReader Gets(TransactionContext tc)
        //{
        //    return tc.ExecuteReader("SELECT * FROM View_PurchaseInvoice Order By [PurchaseInvoiceNo]");
        //}
        //public static IDataReader GetsByInvoiceType(TransactionContext tc, int eInvoiceType)
        //{
        //    return tc.ExecuteReader("SELECT * FROM View_PurchaseInvoice WHERE InvoiceType=%n Order by [PurchaseInvoiceNo]", eInvoiceType);
        //}
        //public static IDataReader GetsByInvoiceEvent(TransactionContext tc, int eEnumInvoiceEvent)
        //{
        //    return tc.ExecuteReader("SELECT * FROM View_PurchaseInvoice WHERE Status=%n Order by [PurchaseInvoiceNo]", eEnumInvoiceEvent);
        //}
        //public static IDataReader GetsByInvoiceBankStatue(TransactionContext tc, int eEnumInvoiceBankStatus)
        //{
        //    return tc.ExecuteReader("SELECT * FROM View_PurchaseInvoice WHERE BankStatus=%n Order by [PurchaseInvoiceNo]", eEnumInvoiceBankStatus);
        //}
        //public static IDataReader GetsByType(TransactionContext tc, int eImportPIType)
        //{
        //    return tc.ExecuteReader("SELECT * FROM View_PurchaseInvoice WHERE InvoiceType=%n Order by [PurchaseInvoiceNo]", eImportPIType);
        //}
        //public static IDataReader Gets(TransactionContext tc, int nLCID)
        //{
        //    return tc.ExecuteReader("SELECT * FROM View_PurchaseInvoice where PPC_LC_Id=%n and InvoiceType=1 Order by ReceiveDate", nLCID);
        //}

        #endregion

        #endregion
    }

}
