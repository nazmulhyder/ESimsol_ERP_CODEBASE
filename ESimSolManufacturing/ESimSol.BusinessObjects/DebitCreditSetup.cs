using System;
using System.IO;
using ESimSol.BusinessObjects;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Collections.Generic;
using ICS.Core;
using ICS.Core.Framework;
using ICS.Core.Utility;

namespace ESimSol.BusinessObjects
{
    #region DebitCreditSetup
    [DataContract]
    public class DebitCreditSetup : BusinessObject
    {
        public DebitCreditSetup()
        {
            DebitCreditSetupID = 0;
            IntegrationSetupDetailID = 0;
            DataCollectionQuery = "";
            CompareColumn = "";
            AccountHeadType = EnumAccountHeadType.None;
            AccountHeadTypeInInt = 0;
            FixedAccountHeadID = 0;
            AccountHeadSetup = "";
            AccountNameSetup = "";
            ReferenceType = EnumReferenceType.None;
            ReferenceTypeInInt = 0;
            CurrencySetup = "";
            ConversionRateSetup = "";
            AmountSetup = "";
            NarrationSetup = "";
            IsChequeReferenceCreate = false;
            IsDebit = false;
            ChequeReferenceDataSQL = "";
            ChequeReferenceCompareColumns = "";
            ChequeType = EnumChequeType.None;
            ChequeTypeInt = 0;
            ChequeSetup = "";
            ChequeReferenceAmountSetup = "";
            ChequeReferenceDescriptionSetup = "";
            ChequeReferenceDateSetup = "";
            IsCostCenterCreate = false;
            HasBillReference = false;
            HasChequeReference = false;
            CostcenterDataSQL = "";
            CostCenterCompareColumns = "";
            CostcenterSetup = "";
            CostCenterAmountSetup = "";
            CostCenterDescriptionSetup = "";
            CostCenterDateSetup = "";
            IsVoucherBill = false;
            VoucherBillDataSQL = "";
            VoucherBillCompareColumns = "";
            VoucherBillTrType = EnumVoucherBillTrType.None;
            VoucherBillTrTypeInInt = 0;
            VoucherBillSetup = "";
            VoucherBillAmountSetup = "";
            VoucherBillDescriptionSetup = "";
            VoucherBillDateSetup = "";
            IsOrderReferenceApply = false;
            OrderReferenceDataSQL = "";
            OrderReferenceCompareColumns = "";
            OrderReferenceSetup = "";
            OrderAmountSetup = "";
            OrderRemarkSetup = "";
            OrderDateSetup = "";
            OrderRefType = EnumVOrderRefType.None;
            OrderRefTypeInt = 0;
            OrderNoSetup = "";
            OrderRefColumn = "";
            OrderNoColumn ="";
            OrderDateColumn = "";
            HasOrderReference = false;
            IsInventoryEffect = false;
            InventoryDataSQL = "";
            InventoryCompareColumns = "";
            InventoryWorkingUnitSetup = "";
            InventoryProductSetup = "";
            InventoryQtySetup = "";
            InventoryUnitSetup = "";
            InventoryUnitPriceSetup = "";
            InventoryDescriptionSetup = "";
            InventoryDateSetup = "";
            Note = "";
            CostCenterCategorySetup = "";
            CostCenterNoColumn = "";
            CostCenterRefObjType = EnumReferenceType.None;
            CostCenterRefObjTypeInInt = 0;
            CostCenterRefObjColumn = "";
            VoucherBillNoColumn = "";
            VoucherBillRefObjType = EnumVoucherBillReferenceType.None;
            VoucherBillRefObjTypeInInt = 0;
            VoucherBillRefObjColumn = "";
            BillDateSetup = "";
            BillDueDateSetup = "";
            AccountCode = "";
            AccountHeadName = "";
            ErrorMessage = "";
            DataCollectionSetups = new List<DataCollectionSetup>();
        }

        #region Properties        
        public int DebitCreditSetupID { get; set; }        
        public int IntegrationSetupDetailID { get; set; }
        public string DataCollectionQuery { get; set; }
        public string CompareColumn { get; set; }
        public EnumAccountHeadType AccountHeadType { get; set; }        
        public int AccountHeadTypeInInt { get; set; }        
        public int FixedAccountHeadID { get; set; }        
        public string AccountHeadSetup { get; set; }
        public string AccountNameSetup { get; set; }
        public EnumReferenceType ReferenceType { get; set; }        
        public int ReferenceTypeInInt { get; set; }        
        public string CurrencySetup { get; set; }        
        public string ConversionRateSetup { get; set; }        
        public string AmountSetup { get; set; }        
        public string NarrationSetup { get; set; }        
        public bool IsChequeReferenceCreate { get; set; }        
        public string ChequeReferenceDataSQL { get; set; }        
        public string ChequeReferenceCompareColumns { get; set; }
        public EnumChequeType ChequeType { get; set; }
        public int ChequeTypeInt { get; set; }
        public string ChequeSetup { get; set; }
        public string ChequeReferenceAmountSetup { get; set; }        
        public string ChequeReferenceDescriptionSetup { get; set; }        
        public string ChequeReferenceDateSetup { get; set; }        
        public bool IsCostCenterCreate { get; set; }
        public bool HasBillReference { get; set; }
        public bool HasChequeReference { get; set; }
        public string CostcenterDataSQL { get; set; }        
        public string CostCenterCompareColumns { get; set; }        
        public string CostcenterSetup { get; set; }        
        public string CostCenterAmountSetup { get; set; }        
        public string CostCenterDescriptionSetup { get; set; }        
        public string CostCenterDateSetup { get; set; }        
        public bool IsVoucherBill { get; set; }        
        public string VoucherBillDataSQL { get; set; }        
        public string VoucherBillCompareColumns { get; set; }        
        public EnumVoucherBillTrType VoucherBillTrType { get; set; }        
        public int VoucherBillTrTypeInInt { get; set; }        
        public string VoucherBillSetup { get; set; }        
        public string VoucherBillAmountSetup { get; set; }        
        public string VoucherBillDescriptionSetup { get; set; }        
        public string VoucherBillDateSetup { get; set; }        
        public bool IsOrderReferenceApply { get; set; }
        public string OrderReferenceDataSQL { get; set; }
        public string OrderReferenceCompareColumns { get; set; }
        public string OrderReferenceSetup { get; set; }
        public string OrderAmountSetup { get; set; }
        public string OrderRemarkSetup { get; set; }
        public string OrderDateSetup { get; set; }
        public EnumVOrderRefType OrderRefType { get; set; }
        public int OrderRefTypeInt { get; set; }
        public string OrderNoSetup { get; set; }
        public string OrderRefColumn { get; set; }
        public string OrderNoColumn { get; set; }
        public string OrderDateColumn { get; set; }
        public bool HasOrderReference { get; set; }
        public bool IsInventoryEffect { get; set; }        
        public string InventoryDataSQL { get; set; }        
        public string InventoryCompareColumns { get; set; }        
        public string InventoryWorkingUnitSetup { get; set; }        
        public string InventoryProductSetup { get; set; }        
        public string InventoryQtySetup { get; set; }        
        public string InventoryUnitSetup { get; set; }        
        public string InventoryUnitPriceSetup { get; set; }        
        public string InventoryDescriptionSetup { get; set; }        
        public string InventoryDateSetup { get; set; }        
        public bool IsDebit { get; set; }        
        public string Note { get; set; }        
        public string CostCenterCategorySetup { get; set; }        
        public string CostCenterNoColumn { get; set; }
        public EnumReferenceType CostCenterRefObjType { get; set; }        
        public int CostCenterRefObjTypeInInt { get; set; }        
        public string CostCenterRefObjColumn { get; set; }        
        public string VoucherBillNoColumn { get; set; }        
        public EnumVoucherBillReferenceType VoucherBillRefObjType { get; set; }        
        public int VoucherBillRefObjTypeInInt { get; set; }        
        public string VoucherBillRefObjColumn { get; set; }
        public string BillDateSetup { get; set; }        
        public string BillDueDateSetup { get; set; }        
        public string AccountCode { get; set; }        
        public string AccountHeadName { get; set; }        
        public string ErrorMessage { get; set; }
        #endregion

        #region Derived Property        
        public List<DataCollectionSetup> DataCollectionSetups { get; set; }        
        public string TranjactionTypeInString
        {
            get
            {
                if (this.IsDebit == true)
                {
                    return "Debit";
                }
                else
                {
                    return "Credit";
                }
            }
        }
        public string AccountHeadTypeInString
        {
            get
            {
                return this.AccountHeadType.ToString();
            }
        }
        public string IsChequeReferenceCreateInString
        {
            get
            {
                if (this.IsChequeReferenceCreate == true)
                {
                    return "YES";
                }
                else
                {
                    return "No";
                }
            }
        }
        public string IsCostCenterCrateInString
        {
            get
            {
                if (this.IsCostCenterCreate == true)
                {
                    return "YES";
                }
                else
                {
                    return "No";
                }
            }
        }
        public string IsVoucherBillInString
        {
            get
            {
                if (this.IsVoucherBill == true)
                {
                    return "YES";
                }
                else
                {
                    return "No";
                }
            }
        }
        public string IsOrderReferenceApplyInString
        {
            get
            {
                if (this.IsOrderReferenceApply == true)
                {
                    return "YES";
                }
                else
                {
                    return "No";
                }
            }
        }
        public string IsInventoryEffectInString
        {
            get
            {
                if (this.IsInventoryEffect == true)
                {
                    return "YES";
                }
                else
                {
                    return "No";
                }
            }
        }
        #endregion

        #region Functions
        public static List<DebitCreditSetup> Gets(int nIntegrationSetupDetailID, long nUserID)
        {
            return DebitCreditSetup.Service.Gets(nIntegrationSetupDetailID, nUserID);            
        }
        public static List<DebitCreditSetup> GetsByIntegrationSetup(int nIntegrationSetupID, long nUserID)
        {
            return DebitCreditSetup.Service.GetsByIntegrationSetup(nIntegrationSetupID, nUserID);            
        }

        public static List<DebitCreditSetup> Gets(string sSQL, long nUserID)
        {
            return DebitCreditSetup.Service.Gets(sSQL, nUserID);            
        }

        public DebitCreditSetup Get(int nDebitCreditSetupID, long nUserID)
        {
            return DebitCreditSetup.Service.Get(nDebitCreditSetupID, nUserID);            
        }

        public DebitCreditSetup Save(long nUserID)
        {
            return DebitCreditSetup.Service.Save(this, nUserID);            
        }
        #endregion

        #region ServiceFactory
        internal static IDebitCreditSetupService Service
        {
            get { return (IDebitCreditSetupService)Services.Factory.CreateService(typeof(IDebitCreditSetupService)); }
        }
        #endregion        
    }
    #endregion

    #region IDebitCreditSetup interface
    
    public interface IDebitCreditSetupService
    {        
        DebitCreditSetup Get(int nDebitCreditSetupID, Int64 nUserID);        
        List<DebitCreditSetup> Gets(int nIntegrationSetupDetailID, Int64 nUserID);        
        List<DebitCreditSetup> GetsByIntegrationSetup(int nIntegrationSetupID, Int64 nUserID);        
        List<DebitCreditSetup> Gets(string sSQL, Int64 nUserID);        
        DebitCreditSetup Save(DebitCreditSetup oDebitCreditSetup, Int64 nUserID);
    }
    #endregion
}
