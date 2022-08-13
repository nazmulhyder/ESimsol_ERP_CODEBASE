using System;
using System.Data;
using System.Data.SqlClient;
using ESimSol.BusinessObjects;
using System.Data.OleDb;
using ICS.Core.DataAccess;
using ICS.Core.Utility;

namespace ESimSol.Services.DataAccess
{
    public class SampleInvoiceDA
    {
        public SampleInvoiceDA() { }

       
        public static IDataReader InsertUpdate(TransactionContext tc, SampleInvoice oSampleInvoice, EnumDBOperation eEnumDBOperation, Int64 nUserID)
        {
            return tc.ExecuteReader("EXEC [SP_IUD_SampleInvoice]"
                                    + "%n,   %s,                                                     %D,                             %n,                          %n,                            %n,                                     %s,                            %n,                            %n,                           %n,                        %n,                           %n,                               %n,         %n,            %n,     %n,%s,%s,%D,%D ,%n ,%n,%n,%b,%n,%n,%n,%n ",
                                    oSampleInvoice.SampleInvoiceID, oSampleInvoice.SampleInvoiceNo, oSampleInvoice.SampleInvoiceDate, oSampleInvoice.ContractorID, oSampleInvoice.ContractorPersopnnalID, oSampleInvoice.PaymentType, oSampleInvoice.MRNo, oSampleInvoice.CurrentStatus,oSampleInvoice.Amount, oSampleInvoice.ConversionRate, oSampleInvoice.CurrencyID,oSampleInvoice.ProductionSettlementStatus,oSampleInvoice.PaymentSettlementStatus,  oSampleInvoice.InvoiceType,oSampleInvoice.OrderType,oSampleInvoice.MKTEmpID,oSampleInvoice.ApprovalRemark,oSampleInvoice.Remark, oSampleInvoice.PaymentDate, oSampleInvoice.RequirementDate, oSampleInvoice.BUID,  oSampleInvoice.ExchangeCurrencyID, oSampleInvoice.RateUnit, oSampleInvoice.IsAdvance,oSampleInvoice.Charge, oSampleInvoice.Discount,  nUserID,  (int)eEnumDBOperation);
        }
        public static IDataReader InsertUpdateLog(TransactionContext tc, SampleInvoice oSampleInvoice, EnumDBOperation eEnumDBOperation, Int64 nUserID)
        {
            return tc.ExecuteReader("EXEC [SP_IUD_SampleInvoiceLog]"
                                    + "%n,   %s,                                                     %D,                             %n,                          %n,                            %n,                                     %s,                            %n,                            %n,                           %n,                        %n,                           %n,                               %n,         %n,            %n,     %n,%s,%s,%D,%D ,%n ,%n,%n,%b,%b,%n,%n,%n,%n ",
                                    oSampleInvoice.SampleInvoiceID, oSampleInvoice.SampleInvoiceNo, oSampleInvoice.SampleInvoiceDate, oSampleInvoice.ContractorID, oSampleInvoice.ContractorPersopnnalID, oSampleInvoice.PaymentType, oSampleInvoice.MRNo, oSampleInvoice.CurrentStatus, oSampleInvoice.Amount, oSampleInvoice.ConversionRate, oSampleInvoice.CurrencyID, oSampleInvoice.ProductionSettlementStatus, oSampleInvoice.PaymentSettlementStatus, oSampleInvoice.InvoiceType, oSampleInvoice.OrderType, oSampleInvoice.MKTEmpID, oSampleInvoice.ApprovalRemark, oSampleInvoice.Remark, oSampleInvoice.PaymentDate, oSampleInvoice.RequirementDate, oSampleInvoice.BUID, oSampleInvoice.ExchangeCurrencyID, oSampleInvoice.RateUnit, oSampleInvoice.IsAdvance, oSampleInvoice.IsRevise, oSampleInvoice.Charge, oSampleInvoice.Discount,nUserID, (int)eEnumDBOperation);
        }
        public static void Delete(TransactionContext tc, SampleInvoice oSampleInvoice, EnumDBOperation eEnumDBOperation, Int64 nUserID)
        {
            tc.ExecuteNonQuery("EXEC [SP_IUD_SampleInvoice]"
                                    + "%n,   %s,                                                     %D,                             %n,                          %n,                            %n,                                     %s,                            %n,                            %n,                           %n,                        %n,                           %n,                               %n,         %n,            %n,     %n,%s,%s,%D,%D ,%n ,%n,%n,%b,%n,%n,%n,%n  ",
                                    oSampleInvoice.SampleInvoiceID, oSampleInvoice.SampleInvoiceNo, oSampleInvoice.SampleInvoiceDate, oSampleInvoice.ContractorID, oSampleInvoice.ContractorPersopnnalID, oSampleInvoice.PaymentType, oSampleInvoice.MRNo, oSampleInvoice.CurrentStatus, oSampleInvoice.Amount, oSampleInvoice.ConversionRate, oSampleInvoice.CurrencyID, oSampleInvoice.ProductionSettlementStatus, oSampleInvoice.PaymentSettlementStatus, oSampleInvoice.InvoiceType, oSampleInvoice.OrderType, oSampleInvoice.MKTEmpID, oSampleInvoice.ApprovalRemark, oSampleInvoice.Remark, oSampleInvoice.PaymentDate, oSampleInvoice.RequirementDate, oSampleInvoice.BUID, oSampleInvoice.ExchangeCurrencyID, oSampleInvoice.RateUnit, oSampleInvoice.IsAdvance,oSampleInvoice.Charge, oSampleInvoice.Discount, nUserID, (int)eEnumDBOperation);
        }
        public static IDataReader InsertUpdate_Adj(TransactionContext tc, SampleInvoice oSampleInvoice, EnumDBOperation eEnumDBOperation, Int64 nUserID)
        {
            return tc.ExecuteReader("EXEC [SP_IUD_SampleInvoice_Adj]"
                                    + "%n,   %s,                                                     %D,                             %n,                          %n,                            %n,                                     %s,                            %n,                            %n,                           %n,                        %n,                           %n,                             %s, %s, %n ,%n,%n ",
                                    oSampleInvoice.SampleInvoiceID, oSampleInvoice.SampleInvoiceNo, oSampleInvoice.SampleInvoiceDate, oSampleInvoice.ExportPIID, oSampleInvoice.ContractorID, oSampleInvoice.ContractorPersopnnalID, (int)oSampleInvoice.CurrentStatus, oSampleInvoice.Amount, oSampleInvoice.ConversionRate, oSampleInvoice.CurrencyID,  oSampleInvoice.InvoiceType,  oSampleInvoice.MKTEmpID, oSampleInvoice.ApprovalRemark, oSampleInvoice.Remark,oSampleInvoice.BUID, nUserID, (int)eEnumDBOperation);
        }
        public static void Delete_Adj(TransactionContext tc, SampleInvoice oSampleInvoice, EnumDBOperation eEnumDBOperation, Int64 nUserID)
        {
            tc.ExecuteNonQuery("EXEC [SP_IUD_SampleInvoice_Adj]"
                               + "%n,   %s,                                                     %D,                             %n,                          %n,                            %n,                                     %s,                            %n,                            %n,                           %n,                        %n,                           %n,                             %s, %s, %n ,%n,%n ",
                                    oSampleInvoice.SampleInvoiceID, oSampleInvoice.SampleInvoiceNo, oSampleInvoice.SampleInvoiceDate, oSampleInvoice.ExportPIID, oSampleInvoice.ContractorID, oSampleInvoice.ContractorPersopnnalID, (int)oSampleInvoice.CurrentStatus, oSampleInvoice.Amount, oSampleInvoice.ConversionRate, oSampleInvoice.CurrencyID, oSampleInvoice.InvoiceType, oSampleInvoice.MKTEmpID, oSampleInvoice.ApprovalRemark, oSampleInvoice.Remark,oSampleInvoice.BUID, nUserID, (int)eEnumDBOperation);

        }
      
        public static IDataReader UpdateSampleInvoice(TransactionContext tc, SampleInvoice oSampleInvoice, EnumDBOperation eEnumDBOperation, Int64 nUserID)
        {
            return tc.ExecuteReader("EXEC [SP_IUD_SampleInvoice_Update]"
                                    + "%n,   %n,  %n,   %n,   %n, %b,%n,%n,%n ",
                                    oSampleInvoice.SampleInvoiceID, oSampleInvoice.PaymentType, oSampleInvoice.ConversionRate, oSampleInvoice.CurrencyID, oSampleInvoice.ExchangeCurrencyID, oSampleInvoice.IsAdvance, oSampleInvoice.MKTEmpID, nUserID, (int)eEnumDBOperation);
        }
        public static void RemoveExportPIFromBill(TransactionContext tc, SampleInvoice oSampleInvoice, Int64 nUserID)
        {
            tc.ExecuteNonQuery("Update SampleInvoice Set ExportPIID =0 Where SampleInvoiceID =%n", oSampleInvoice.SampleInvoiceID);
            tc.ExecuteNonQuery("Update DyeingOrder Set ExportPIID =0 Where SampleInvoiceID =%n", oSampleInvoice.SampleInvoiceID);
        }
        public static IDataReader ExportPI_Attach(TransactionContext tc, SampleInvoice oSampleInvoice, int nDBOperation, Int64 nUserID)
        {
            return tc.ExecuteReader("EXEC [SP_IUD_SampleInvoice_AttachPI]" + "%n, %s, %n, %n", oSampleInvoice.ExportPIID, oSampleInvoice.SampleInvoiceNo, nUserID, nDBOperation);
        }
        public static IDataReader UpdateSInvoiceNo(TransactionContext tc, SampleInvoice oSampleInvoice, Int64 nUserID, EnumDBOperation eEnumDBOperation)
        {
            return tc.ExecuteReader("EXEC [SP_IUD_SampleInvoiceNoChange]" + "%n,  %s,%n,%n", oSampleInvoice.SampleInvoiceID, oSampleInvoice.SampleInvoiceNo, nUserID, eEnumDBOperation);
        }
        #region Change Payment Type
        public static IDataReader SaveChangePayType(TransactionContext tc, int SampleInvoiceID, int PaymentType, string sNote, int DBOperation, Int64 nUserID)
        {
            return tc.ExecuteReader(CommandType.StoredProcedure, "[SP_SampleInvoice_ChangePaymentType]", SampleInvoiceID, PaymentType, sNote, DBOperation, nUserID);
        }
        #endregion

        #region Sample Invoice Payment Settlement
        public static IDataReader SavePaymentSettlement(TransactionContext tc, int SampleInvoiceID, int PaymentSettlement, string sNote, Int64 nUserID)
        {
            return tc.ExecuteReader(CommandType.StoredProcedure, "[SP_SampleInvoice_PaymentSettlement]", SampleInvoiceID, PaymentSettlement, sNote, nUserID);
        }
        #endregion

        #region Sample Invoice Payment Settlement
        public static IDataReader SaveProductionSettlement(TransactionContext tc, int SampleInvoiceID, int ProductionSettlement, string sNote,double nAmount, Int64 nUserID)
        {
            return tc.ExecuteReader(CommandType.StoredProcedure, "[SP_SampleInvoice_ProductionSettlement]", SampleInvoiceID, ProductionSettlement, sNote,nAmount, nUserID);
        }
        #endregion


        #region Update Function
   
        public static IDataReader SampleInvoice_Approved(TransactionContext tc, string sPCID, int eAppAtatus, bool bIsDeliveryLock, Int64 nUserID)
        {
            return tc.ExecuteReader(CommandType.StoredProcedure, "[SP_Approve_SampleInvoice]",  sPCID,  eAppAtatus,bIsDeliveryLock, nUserID);
        }
        public static IDataReader SampleInvoice_PaymentSettlment(TransactionContext tc, int nPCID, string sRemark,  Int64 nUserID)
        {
            return tc.ExecuteReader(CommandType.StoredProcedure, "[SP_SampleInvoice_PaymentSettlement]", nPCID, sRemark, nUserID);
        }
        public static void UpdateVoucherEffect(TransactionContext tc, SampleInvoice oSampleInvoice)
        {
            tc.ExecuteNonQuery(" Update SampleInvoice SET IsWillVoucherEffect = %b WHERE SampleInvoiceID  = %n", oSampleInvoice.IsWillVoucherEffect, oSampleInvoice.SampleInvoiceID);
        }

        
        #endregion

       

        #region Generation Function
        public static int GetNewID(TransactionContext tc)
        {
            return tc.GenerateID("SampleInvoice", "SampleInvoiceID");
        }
        #endregion

        #region Get & Exist Function
        public static IDataReader Get(TransactionContext tc, int nID)
        {
            return tc.ExecuteReader("SELECT * FROM View_SampleInvoice WHERE SampleInvoiceID=%n", nID);
        }
    
        public static IDataReader GetbySampleOrderID(TransactionContext tc, int nID)
        {
            return tc.ExecuteReader("SELECT * FROM View_SampleInvoice WHERE SampleInvoiceID in (select SampleOrder.ContractID from SampleOrder where SampleOrder.SampleOrderID=%n)", nID);
        }
        public static IDataReader Get(TransactionContext tc, string sPCNo)
        {
            return tc.ExecuteReader("SELECT * FROM View_SampleInvoice WHERE SampleInvoiceNo=%s", sPCNo);
        }
        public static IDataReader Gets(TransactionContext tc)
        {
            return tc.ExecuteReader("SELECT * FROM View_SampleInvoice");
        }
        public static IDataReader Gets(TransactionContext tc,string sSQL)
        {
            return tc.ExecuteReader( sSQL);
        }
        public static IDataReader Gets_CashReturn(TransactionContext tc, string sSQL)
        {
            return tc.ExecuteReader(sSQL);
        }
        public static IDataReader Gets(TransactionContext tc, string sContractorIDs, int ePaymentType)
        {

            // Initialized = 0,  WaitingForApprove = 1,    Approved = 2,  Not_Approved = 3,// InTransit = 3,   RequestToPaymentTypeChange = 4,// InTransit = 3,       Settled = 5,
            return tc.ExecuteReader("select * from View_SampleInvoice as SampleInvoice where PaymentSettlementStatus<3 and SampleInvoiceType in (1,2,6) and CurrentStatus in (1,2) and ContractorID in(%q) and PaymentType=%n and SampleInvoiceID not in (select SalesContractAdjustment.SampleInvoiceID from SalesContractAdjustment)  order by SampleInvoiceID ", sContractorIDs, ePaymentType);
        }
        public static IDataReader GetsByNo(TransactionContext tc, string sSampleInvoiceNo, int ePaymentType)
        {
            //return tc.ExecuteReader("select * from View_SampleInvoice as SampleInvoice where SampleInvoiceType in (1,2,4,6) and CurrentStatus in (1,2,4) and SampleInvoiceNo in('%q') and PaymentType=%n and SampleInvoiceID not in (select SalesContractAdjustment.SampleInvoiceID from SalesContractAdjustment)  order by SampleInvoiceID ", sSampleInvoiceNo, ePaymentType);
            return tc.ExecuteReader("select * from View_SampleInvoice as SampleInvoice where SampleInvoiceType in (1,2,6) and CurrentStatus in (1,2) and SampleInvoiceNo in('%q') and PaymentType=%n and SampleInvoiceID not in (select SalesContractAdjustment.SampleInvoiceID from SalesContractAdjustment)  order by SampleInvoiceID ", sSampleInvoiceNo, ePaymentType);
        }
        public static IDataReader ExportPISNA(TransactionContext tc, int nExportPIID, Int64 nUserID)
        {
            return tc.ExecuteReader("EXEC [SP_IUD_ExportPI_SAN]" + "%n,  %n", nExportPIID, nUserID);
        }

        //public static int GetApprovedEMPID(TransactionContext tc, int nSampleInvoiceID)
        //{
        //    object obj = tc.ExecuteScalar("SELECT ApprovedEMPID from SampleInvoice WHERE SampleInvoiceID=%n", nSampleInvoiceID);
        //    if (obj == null) return -1;
        //    return Convert.ToInt32(obj);
        //}
    
        #endregion
    }
}
