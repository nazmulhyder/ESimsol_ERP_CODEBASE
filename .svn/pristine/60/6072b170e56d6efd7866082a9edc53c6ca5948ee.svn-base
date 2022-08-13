using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using ESimSol.BusinessObjects;
using ESimSol.Services.DataAccess;
using ICS.Core.DataAccess;
using ICS.Core.Utility;
using ICS.Core.Framework;

namespace ESimSol.Services.Services
{
    public class DebitCreditSetupService : MarshalByRefObject, IDebitCreditSetupService
    {
        #region Private functions and declaration
        private DebitCreditSetup MapObject(NullHandler oReader)
        {
            DebitCreditSetup oDebitCreditSetup = new DebitCreditSetup();
            oDebitCreditSetup.DebitCreditSetupID = oReader.GetInt32("DebitCreditSetupID");
            oDebitCreditSetup.IntegrationSetupDetailID = oReader.GetInt32("IntegrationSetupDetailID");
            oDebitCreditSetup.DataCollectionQuery = oReader.GetString("DataCollectionQuery");
            oDebitCreditSetup.CompareColumn = oReader.GetString("CompareColumn");
            oDebitCreditSetup.AccountHeadType = (EnumAccountHeadType)oReader.GetInt32("AccountHeadType");
            oDebitCreditSetup.AccountHeadTypeInInt = oReader.GetInt32("AccountHeadType");
            oDebitCreditSetup.FixedAccountHeadID = oReader.GetInt32("FixedAccountHeadID");            
            oDebitCreditSetup.AccountHeadSetup = oReader.GetString("AccountHeadSetup");
            oDebitCreditSetup.AccountNameSetup = oReader.GetString("AccountNameSetup");
            oDebitCreditSetup.ReferenceType = (EnumReferenceType)oReader.GetInt32("ReferenceType");
            oDebitCreditSetup.ReferenceTypeInInt = oReader.GetInt32("ReferenceType");
            oDebitCreditSetup.CurrencySetup = oReader.GetString("CurrencySetup");
            oDebitCreditSetup.ConversionRateSetup = oReader.GetString("ConversionRateSetup");
            oDebitCreditSetup.AmountSetup = oReader.GetString("AmountSetup");
            oDebitCreditSetup.NarrationSetup = oReader.GetString("NarrationSetup");
            oDebitCreditSetup.IsChequeReferenceCreate = oReader.GetBoolean("IsChequeReferenceCreate");
            oDebitCreditSetup.ChequeReferenceDataSQL = oReader.GetString("ChequeReferenceDataSQL");
            oDebitCreditSetup.ChequeReferenceCompareColumns = oReader.GetString("ChequeReferenceCompareColumns");
            oDebitCreditSetup.ChequeType = (EnumChequeType)oReader.GetInt32("ChequeType");
            oDebitCreditSetup.ChequeTypeInt = oReader.GetInt32("ChequeType");
            oDebitCreditSetup.ChequeSetup = oReader.GetString("ChequeSetup");
            oDebitCreditSetup.ChequeReferenceAmountSetup = oReader.GetString("ChequeReferenceAmountSetup");
            oDebitCreditSetup.ChequeReferenceDescriptionSetup = oReader.GetString("ChequeReferenceDescriptionSetup");
            oDebitCreditSetup.ChequeReferenceDateSetup = oReader.GetString("ChequeReferenceDateSetup");
            oDebitCreditSetup.IsCostCenterCreate = oReader.GetBoolean("IsCostCenterCreate");
            oDebitCreditSetup.HasBillReference = oReader.GetBoolean("HasBillReference");
            oDebitCreditSetup.HasChequeReference = oReader.GetBoolean("HasChequeReference");
            oDebitCreditSetup.CostcenterDataSQL = oReader.GetString("CostcenterDataSQL");
            oDebitCreditSetup.CostCenterCompareColumns = oReader.GetString("CostCenterCompareColumns");
            oDebitCreditSetup.CostcenterSetup = oReader.GetString("CostcenterSetup");
            oDebitCreditSetup.CostCenterAmountSetup = oReader.GetString("CostCenterAmountSetup");
            oDebitCreditSetup.CostCenterDescriptionSetup = oReader.GetString("CostCenterDescriptionSetup");
            oDebitCreditSetup.CostCenterDateSetup = oReader.GetString("CostCenterDateSetup");
            oDebitCreditSetup.IsVoucherBill = oReader.GetBoolean("IsVoucherBill");
            oDebitCreditSetup.VoucherBillDataSQL = oReader.GetString("VoucherBillDataSQL");
            oDebitCreditSetup.VoucherBillCompareColumns = oReader.GetString("VoucherBillCompareColumns");
            oDebitCreditSetup.VoucherBillTrType = (EnumVoucherBillTrType)oReader.GetInt32("VoucherBillTrType");
            oDebitCreditSetup.VoucherBillTrTypeInInt = oReader.GetInt32("VoucherBillTrType");
            oDebitCreditSetup.VoucherBillSetup = oReader.GetString("VoucherBillSetup");
            oDebitCreditSetup.VoucherBillAmountSetup = oReader.GetString("VoucherBillAmountSetup");
            oDebitCreditSetup.VoucherBillDescriptionSetup = oReader.GetString("VoucherBillDescriptionSetup");
            oDebitCreditSetup.VoucherBillDateSetup = oReader.GetString("VoucherBillDateSetup");
            oDebitCreditSetup.IsOrderReferenceApply = oReader.GetBoolean("IsOrderReferenceApply");
            oDebitCreditSetup.OrderReferenceDataSQL = oReader.GetString("OrderReferenceDataSQL");
            oDebitCreditSetup.OrderReferenceCompareColumns = oReader.GetString("OrderReferenceCompareColumns");
            oDebitCreditSetup.OrderReferenceSetup = oReader.GetString("OrderReferenceSetup");
            oDebitCreditSetup.OrderAmountSetup = oReader.GetString("OrderAmountSetup");
            oDebitCreditSetup.OrderRemarkSetup = oReader.GetString("OrderRemarkSetup");
            oDebitCreditSetup.OrderDateSetup = oReader.GetString("OrderDateSetup");
            oDebitCreditSetup.OrderRefType = (EnumVOrderRefType)oReader.GetInt32("OrderRefType");
            oDebitCreditSetup.OrderRefTypeInt = oReader.GetInt32("OrderRefType");
            oDebitCreditSetup.OrderNoSetup = oReader.GetString("OrderNoSetup");
            oDebitCreditSetup.OrderRefColumn = oReader.GetString("OrderRefColumn");
            oDebitCreditSetup.OrderNoColumn = oReader.GetString("OrderNoColumn");
            oDebitCreditSetup.OrderDateColumn = oReader.GetString("OrderDateColumn");
            oDebitCreditSetup.HasOrderReference = oReader.GetBoolean("HasOrderReference");
            oDebitCreditSetup.IsInventoryEffect = oReader.GetBoolean("IsInventoryEffect");
            oDebitCreditSetup.InventoryDataSQL = oReader.GetString("InventoryDataSQL");
            oDebitCreditSetup.InventoryCompareColumns = oReader.GetString("InventoryCompareColumns");
            oDebitCreditSetup.InventoryWorkingUnitSetup = oReader.GetString("InventoryWorkingUnitSetup");
            oDebitCreditSetup.InventoryProductSetup = oReader.GetString("InventoryProductSetup");
            oDebitCreditSetup.InventoryQtySetup = oReader.GetString("InventoryQtySetup");
            oDebitCreditSetup.InventoryUnitSetup = oReader.GetString("InventoryUnitSetup");
            oDebitCreditSetup.InventoryUnitPriceSetup = oReader.GetString("InventoryUnitPriceSetup");
            oDebitCreditSetup.InventoryDescriptionSetup = oReader.GetString("InventoryDescriptionSetup");
            oDebitCreditSetup.InventoryDateSetup = oReader.GetString("InventoryDateSetup");
            oDebitCreditSetup.IsDebit = oReader.GetBoolean("IsDebit");
            oDebitCreditSetup.Note = oReader.GetString("Note");
            oDebitCreditSetup.CostCenterCategorySetup = oReader.GetString("CostCenterCategorySetup");
            oDebitCreditSetup.CostCenterNoColumn = oReader.GetString("CostCenterNoColumn");
            oDebitCreditSetup.CostCenterRefObjType = (EnumReferenceType)oReader.GetInt32("CostCenterRefObjType");
            oDebitCreditSetup.CostCenterRefObjTypeInInt = oReader.GetInt32("CostCenterRefObjType");
            oDebitCreditSetup.CostCenterRefObjColumn = oReader.GetString("CostCenterRefObjColumn");
            oDebitCreditSetup.VoucherBillNoColumn = oReader.GetString("VoucherBillNoColumn");
            oDebitCreditSetup.VoucherBillRefObjType = (EnumVoucherBillReferenceType)oReader.GetInt32("VoucherBillRefObjType");
            oDebitCreditSetup.VoucherBillRefObjTypeInInt = oReader.GetInt32("VoucherBillRefObjType");
            oDebitCreditSetup.VoucherBillRefObjColumn = oReader.GetString("VoucherBillRefObjColumn");
            oDebitCreditSetup.BillDateSetup = oReader.GetString("BillDateSetup");
            oDebitCreditSetup.BillDueDateSetup = oReader.GetString("BillDueDateSetup");
            oDebitCreditSetup.AccountCode = oReader.GetString("AccountCode");
            oDebitCreditSetup.AccountHeadName = oReader.GetString("AccountHeadName");
            return oDebitCreditSetup;
        }

        private DebitCreditSetup CreateObject(NullHandler oReader)
        {
            DebitCreditSetup oDebitCreditSetup = new DebitCreditSetup();
            oDebitCreditSetup = MapObject(oReader);
            return oDebitCreditSetup;
        }

        private List<DebitCreditSetup> CreateObjects(IDataReader oReader)
        {
            List<DebitCreditSetup> oDebitCreditSetup = new List<DebitCreditSetup>();
            NullHandler oHandler = new NullHandler(oReader);
            while (oReader.Read())
            {
                DebitCreditSetup oItem = CreateObject(oHandler);
                oDebitCreditSetup.Add(oItem);
            }
            return oDebitCreditSetup;
        }

        #endregion

        #region Interface implementation
        public DebitCreditSetupService() { }

        public DebitCreditSetup Save(DebitCreditSetup oDebitCreditSetup, Int64 nUserID)
        {
            TransactionContext tc = null;

            List<DebitCreditSetup> _oDebitCreditSetups = new List<DebitCreditSetup>();
            oDebitCreditSetup.ErrorMessage = "";
            try
            {
                tc = TransactionContext.Begin(true);
                IDataReader reader;
                reader = DebitCreditSetupDA.InsertUpdate(tc, oDebitCreditSetup, EnumDBOperation.Update, nUserID, "");
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oDebitCreditSetup = new DebitCreditSetup();
                    oDebitCreditSetup = CreateObject(oReader);
                }
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                oDebitCreditSetup.ErrorMessage = e.Message;
                #endregion
            }
            return oDebitCreditSetup;
        }

        public DebitCreditSetup Get(int nDebitCreditSetupID, Int64 nUserId)
        {
            DebitCreditSetup oAccountHead = new DebitCreditSetup();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = DebitCreditSetupDA.Get(tc, nDebitCreditSetupID);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oAccountHead = CreateObject(oReader);
                }
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get DebitCreditSetup", e);
                #endregion
            }

            return oAccountHead;
        }

        public List<DebitCreditSetup> Gets(int nIntegrationSetupDetailID, Int64 nUserID)
        {
            List<DebitCreditSetup> oDebitCreditSetup = null;

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();
                IDataReader reader = null;
                reader = DebitCreditSetupDA.Gets(tc, nIntegrationSetupDetailID);
                oDebitCreditSetup = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get DebitCreditSetup", e);
                #endregion
            }

            return oDebitCreditSetup;
        }

        public List<DebitCreditSetup> GetsByIntegrationSetup(int nIntegrationSetupID, Int64 nUserID)
        {
            List<DebitCreditSetup> oDebitCreditSetup = null;

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();
                IDataReader reader = null;
                reader = DebitCreditSetupDA.GetsByIntegrationSetup(tc, nIntegrationSetupID);
                oDebitCreditSetup = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get DebitCreditSetup", e);
                #endregion
            }

            return oDebitCreditSetup;
        }

        public List<DebitCreditSetup> Gets(string sSQL, Int64 nUserID)
        {
            List<DebitCreditSetup> oDebitCreditSetup = null;

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = DebitCreditSetupDA.Gets(tc, sSQL);
                oDebitCreditSetup = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get DebitCreditSetup", e);
                #endregion
            }

            return oDebitCreditSetup;
        }
        #endregion
    }
}
