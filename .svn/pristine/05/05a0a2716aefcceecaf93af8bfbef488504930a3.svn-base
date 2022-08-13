using System;
using System.Data;
using System.Data.SqlClient;
using ICS.Core.DataAccess;
using ESimSol.BusinessObjects;
using System.Data.OleDb;
using ICS.Core.Utility;

namespace ESimSol.Services.DataAccess
{
    public class DebitCreditSetupDA
    {
        public DebitCreditSetupDA() { }

        #region Insert Update Delete Function
        public static IDataReader InsertUpdate(TransactionContext tc, DebitCreditSetup oDebitCreditSetup, EnumDBOperation eEnumDBOperation, Int64 nUserID, String sDebitCreditSetupIDs)
        {
            return tc.ExecuteReader("EXEC [SP_IUD_DebitCreditSetup]" + "%n, %n, %s, %s, %n, %n, %s, %s, %n, %s, %s, %s, %s, %b, %s, %s, %n, %s, %s, %s, %s, %b, %b, %b, %s, %s, %s, %s, %s, %s, %b, %s, %s, %s, %s, %s, %s, %s, %b, %s, %s, %s, %s, %s, %s, %s, %s, %s, %b, %s, %s, %s, %n, %s, %s, %n, %s, %s, %s, %b, %s, %s, %s, %s, %s, %s, %n, %s, %s, %s, %s, %b, %n, %n, %s",
                                    oDebitCreditSetup.DebitCreditSetupID, oDebitCreditSetup.IntegrationSetupDetailID, oDebitCreditSetup.DataCollectionQuery, oDebitCreditSetup.CompareColumn, oDebitCreditSetup.AccountHeadTypeInInt, oDebitCreditSetup.FixedAccountHeadID, oDebitCreditSetup.AccountHeadSetup, oDebitCreditSetup.AccountNameSetup, oDebitCreditSetup.ReferenceTypeInInt, oDebitCreditSetup.CurrencySetup, oDebitCreditSetup.ConversionRateSetup, oDebitCreditSetup.AmountSetup, oDebitCreditSetup.NarrationSetup, oDebitCreditSetup.IsChequeReferenceCreate, oDebitCreditSetup.ChequeReferenceDataSQL, oDebitCreditSetup.ChequeReferenceCompareColumns, oDebitCreditSetup.ChequeTypeInt, oDebitCreditSetup.ChequeSetup, oDebitCreditSetup.ChequeReferenceAmountSetup, oDebitCreditSetup.ChequeReferenceDescriptionSetup, oDebitCreditSetup.ChequeReferenceDateSetup, oDebitCreditSetup.IsCostCenterCreate, oDebitCreditSetup.HasBillReference, oDebitCreditSetup.HasChequeReference, oDebitCreditSetup.CostcenterDataSQL, oDebitCreditSetup.CostCenterCompareColumns, oDebitCreditSetup.CostcenterSetup, oDebitCreditSetup.CostCenterAmountSetup, oDebitCreditSetup.CostCenterDescriptionSetup, oDebitCreditSetup.CostCenterDateSetup, oDebitCreditSetup.IsVoucherBill, oDebitCreditSetup.VoucherBillDataSQL, oDebitCreditSetup.VoucherBillCompareColumns, oDebitCreditSetup.VoucherBillTrTypeInInt, oDebitCreditSetup.VoucherBillSetup, oDebitCreditSetup.VoucherBillAmountSetup, oDebitCreditSetup.VoucherBillDescriptionSetup, oDebitCreditSetup.VoucherBillDateSetup, oDebitCreditSetup.IsInventoryEffect, oDebitCreditSetup.InventoryDataSQL, oDebitCreditSetup.InventoryCompareColumns, oDebitCreditSetup.InventoryWorkingUnitSetup, oDebitCreditSetup.InventoryProductSetup, oDebitCreditSetup.InventoryQtySetup, oDebitCreditSetup.InventoryUnitSetup, oDebitCreditSetup.InventoryUnitPriceSetup, oDebitCreditSetup.InventoryDescriptionSetup, oDebitCreditSetup.InventoryDateSetup, oDebitCreditSetup.IsDebit, oDebitCreditSetup.Note, oDebitCreditSetup.CostCenterCategorySetup, oDebitCreditSetup.CostCenterNoColumn, oDebitCreditSetup.CostCenterRefObjTypeInInt, oDebitCreditSetup.CostCenterRefObjColumn, oDebitCreditSetup.VoucherBillNoColumn, oDebitCreditSetup.VoucherBillRefObjTypeInInt, oDebitCreditSetup.VoucherBillRefObjColumn, oDebitCreditSetup.BillDateSetup, oDebitCreditSetup.BillDueDateSetup, oDebitCreditSetup.IsOrderReferenceApply, oDebitCreditSetup.OrderReferenceDataSQL, oDebitCreditSetup.OrderReferenceCompareColumns, oDebitCreditSetup.OrderReferenceSetup, oDebitCreditSetup.OrderAmountSetup, oDebitCreditSetup.OrderRemarkSetup, oDebitCreditSetup.OrderDateSetup, oDebitCreditSetup.OrderRefTypeInt, oDebitCreditSetup.OrderNoSetup, oDebitCreditSetup.OrderRefColumn, oDebitCreditSetup.OrderNoColumn, oDebitCreditSetup.OrderDateColumn, oDebitCreditSetup.HasOrderReference, nUserID, (int)eEnumDBOperation, sDebitCreditSetupIDs);
        }

        public static void Delete(TransactionContext tc, DebitCreditSetup oDebitCreditSetup, EnumDBOperation eEnumDBOperation, Int64 nUserID, String sDebitCreditSetupIDs)
        {
            tc.ExecuteNonQuery("EXEC [SP_IUD_DebitCreditSetup]" + "%n, %n, %s, %s, %n, %n, %s, %s, %n, %s, %s, %s, %s, %b, %s, %s, %n, %s, %s, %s, %s, %b, %b, %b, %s, %s, %s, %s, %s, %s, %b, %s, %s, %s, %s, %s, %s, %s, %b, %s, %s, %s, %s, %s, %s, %s, %s, %s, %b, %s, %s, %s, %n, %s, %s, %n, %s, %s, %s, %b, %s, %s, %s, %s, %s, %s, %n, %s, %s, %s, %s, %b, %n, %n, %s",
                                    oDebitCreditSetup.DebitCreditSetupID, oDebitCreditSetup.IntegrationSetupDetailID, oDebitCreditSetup.DataCollectionQuery, oDebitCreditSetup.CompareColumn, oDebitCreditSetup.AccountHeadTypeInInt, oDebitCreditSetup.FixedAccountHeadID, oDebitCreditSetup.AccountHeadSetup, oDebitCreditSetup.AccountNameSetup, oDebitCreditSetup.ReferenceTypeInInt, oDebitCreditSetup.CurrencySetup, oDebitCreditSetup.ConversionRateSetup, oDebitCreditSetup.AmountSetup, oDebitCreditSetup.NarrationSetup, oDebitCreditSetup.IsChequeReferenceCreate, oDebitCreditSetup.ChequeReferenceDataSQL, oDebitCreditSetup.ChequeReferenceCompareColumns, oDebitCreditSetup.ChequeTypeInt, oDebitCreditSetup.ChequeSetup, oDebitCreditSetup.ChequeReferenceAmountSetup, oDebitCreditSetup.ChequeReferenceDescriptionSetup, oDebitCreditSetup.ChequeReferenceDateSetup, oDebitCreditSetup.IsCostCenterCreate, oDebitCreditSetup.HasBillReference, oDebitCreditSetup.HasChequeReference, oDebitCreditSetup.CostcenterDataSQL, oDebitCreditSetup.CostCenterCompareColumns, oDebitCreditSetup.CostcenterSetup, oDebitCreditSetup.CostCenterAmountSetup, oDebitCreditSetup.CostCenterDescriptionSetup, oDebitCreditSetup.CostCenterDateSetup, oDebitCreditSetup.IsVoucherBill, oDebitCreditSetup.VoucherBillDataSQL, oDebitCreditSetup.VoucherBillCompareColumns, oDebitCreditSetup.VoucherBillTrTypeInInt, oDebitCreditSetup.VoucherBillSetup, oDebitCreditSetup.VoucherBillAmountSetup, oDebitCreditSetup.VoucherBillDescriptionSetup, oDebitCreditSetup.VoucherBillDateSetup, oDebitCreditSetup.IsInventoryEffect, oDebitCreditSetup.InventoryDataSQL, oDebitCreditSetup.InventoryCompareColumns, oDebitCreditSetup.InventoryWorkingUnitSetup, oDebitCreditSetup.InventoryProductSetup, oDebitCreditSetup.InventoryQtySetup, oDebitCreditSetup.InventoryUnitSetup, oDebitCreditSetup.InventoryUnitPriceSetup, oDebitCreditSetup.InventoryDescriptionSetup, oDebitCreditSetup.InventoryDateSetup, oDebitCreditSetup.IsDebit, oDebitCreditSetup.Note, oDebitCreditSetup.CostCenterCategorySetup, oDebitCreditSetup.CostCenterNoColumn, oDebitCreditSetup.CostCenterRefObjTypeInInt, oDebitCreditSetup.CostCenterRefObjColumn, oDebitCreditSetup.VoucherBillNoColumn, oDebitCreditSetup.VoucherBillRefObjTypeInInt, oDebitCreditSetup.VoucherBillRefObjColumn, oDebitCreditSetup.BillDateSetup, oDebitCreditSetup.BillDueDateSetup, oDebitCreditSetup.IsOrderReferenceApply, oDebitCreditSetup.OrderReferenceDataSQL, oDebitCreditSetup.OrderReferenceCompareColumns, oDebitCreditSetup.OrderReferenceSetup, oDebitCreditSetup.OrderAmountSetup, oDebitCreditSetup.OrderRemarkSetup, oDebitCreditSetup.OrderDateSetup, oDebitCreditSetup.OrderRefTypeInt, oDebitCreditSetup.OrderNoSetup, oDebitCreditSetup.OrderRefColumn, oDebitCreditSetup.OrderNoColumn, oDebitCreditSetup.OrderDateColumn, oDebitCreditSetup.HasOrderReference, nUserID, (int)eEnumDBOperation, sDebitCreditSetupIDs);
        }


        #endregion

        #region Get & Exist Function
        public static IDataReader Get(TransactionContext tc, long nID)
        {
            return tc.ExecuteReader("SELECT * FROM View_DebitCreditSetup WHERE DebitCreditSetupID=%n", nID);
        }

        public static IDataReader Gets(TransactionContext tc, int nIntegrationSetupDetailID)
        {
            return tc.ExecuteReader("SELECT * FROM View_DebitCreditSetup where IntegrationSetupDetailID =%n", nIntegrationSetupDetailID);
        }

        public static IDataReader GetsByIntegrationSetup(TransactionContext tc, int nIntegrationSetupID)
        {
            return tc.ExecuteReader("SELECT * FROM View_DebitCreditSetup WHERE IntegrationSetupDetailID IN (SELECT IntegrationSetupDetailID FROM IntegrationSetupDetail WHERE IntegrationSetupID = %n)", nIntegrationSetupID);
        }

        public static IDataReader Gets(TransactionContext tc, string sSQL)
        {
            return tc.ExecuteReader(sSQL);
        }
        #endregion
    }
}
