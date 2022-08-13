using System;
using System.Collections.Generic;
using System.Data;
using ESimSol.BusinessObjects;
using ESimSol.Services.DataAccess;
using ICS.Core.DataAccess;
using ICS.Core.Framework;
using ICS.Core.Utility;
namespace ESimSol.Services.Services
{
    public class VoucherService : MarshalByRefObject, IVoucherService
    {
        #region Private functions and declaration
        private Voucher MapObject(NullHandler oReader)
        {
            Voucher oVoucher = new Voucher();
            oVoucher.VoucherID = oReader.GetInt32("VoucherID");
            oVoucher.BUID = oReader.GetInt32("BUID");
            oVoucher.VoucherTypeID = oReader.GetInt32("VoucherTypeID");
            oVoucher.VoucherNo = oReader.GetString("VoucherNo");
            oVoucher.Narration = oReader.GetString("Narration");
            oVoucher.ReferenceNote = oReader.GetString("ReferenceNote");
            oVoucher.VoucherDate = oReader.GetDateTime("VoucherDate");            
            oVoucher.AuthorizedBy = oReader.GetInt32("AuthorizedBy");
            oVoucher.VoucherAmount = oReader.GetDouble("VoucherAmount");
            oVoucher.BUCode = oReader.GetString("BUCode");
            oVoucher.BUName = oReader.GetString("BUName");
            oVoucher.BUShortName = oReader.GetString("BUShortName");
            oVoucher.OperationType = (EnumVoucherOperationType)oReader.GetInt32("OperationType");
            oVoucher.PrintCount = oReader.GetInt32("PrintCount");
            oVoucher.BatchID = oReader.GetInt32("BatchID");
            oVoucher.TaxEffect = (EnumTaxEffect)oReader.GetInt32("TaxEffect");
            oVoucher.TaxEffectInt = oReader.GetInt32("TaxEffect");
            oVoucher.CRate = oReader.GetDouble("CRate");
            oVoucher.TotalAmount = oReader.GetDouble("TotalAmount");
            oVoucher.VoucherName = oReader.GetString("VoucherName");
            oVoucher.NumberMethod = (EnumNumberMethod)oReader.GetInt32("NumberMethod");
            oVoucher.NumberMethodInInt = oReader.GetInt32("NumberMethod");
            oVoucher.CurrencyID = oReader.GetInt32("CurrencyID");
            oVoucher.CUSymbol = oReader.GetString("CUSymbol");
            oVoucher.AuthorizedByName = oReader.GetString("AuthorizedByName");
            oVoucher.PreparedByName = oReader.GetString("PreparedByName");
            oVoucher.DBServerDate = oReader.GetDateTime("DBServerDate");
            oVoucher.BUIDCodeNames = oReader.GetString("BUIDCodeNames");
            oVoucher.BaseCurrencyID = oReader.GetInt32("BaseCurrencyID");
            oVoucher.BaseCUSymbol = oReader.GetString("BaseCUSymbol");
            oVoucher.VoucherBatchNO = oReader.GetString("VoucherBatchNO");
            oVoucher.CounterVoucherID = oReader.GetInt32("CounterVoucherID");
            oVoucher.VoucherCategory = (EnumVoucherCategory)oReader.GetInt32("VoucherCategory");
            return oVoucher;
        }
        private Voucher CreateObject(NullHandler oReader)
        {
            Voucher oVoucher = new Voucher();
            oVoucher = MapObject(oReader);
            return oVoucher;
        }
        private List<Voucher> CreateObjects(IDataReader oReader)
        {
            List<Voucher> oVoucher = new List<Voucher>();
            NullHandler oHandler = new NullHandler(oReader);
            while (oReader.Read())
            {
                Voucher oItem = CreateObject(oHandler);
                oVoucher.Add(oItem);
            }
            return oVoucher;
        }
       
        #endregion

        #region Functions
        private ChartsOfAccount GetAccountHead(TransactionContext tc, int nAccountHeadID)
        {
            ChartsOfAccount oChartsOfAccount = new ChartsOfAccount();
            IDataReader reader = ChartsOfAccountDA.GetDependencies(tc, nAccountHeadID);
            NullHandler oReader = new NullHandler(reader);
            if (reader.Read())
            {
                oChartsOfAccount.IsCostCenterApply = oReader.GetBoolean("IsCostCenterApply");
                oChartsOfAccount.IsBillRefApply = oReader.GetBoolean("IsBillRefApply");
                oChartsOfAccount.IsInventoryApply = oReader.GetBoolean("IsInventoryApply");
                oChartsOfAccount.IsOrderReferenceApply = oReader.GetBoolean("IsOrderReferenceApply");               
            }
            reader.Close();

            return oChartsOfAccount;
        }
        private string AccountHeadWiseInputValidation(TransactionContext tc, VoucherDetail oVoucherDetail, string sReferenceNo, double nDifferAcceptValue)
        {
            string sErrorMEssage = "";
            bool IsCostCenterApply = false;
            bool IsBillRefApply = false;
            bool IsInventoryApply = false;
            bool IsOrderReferenceApply = false;
            bool IsChequeApply = false;
            IDataReader reader = ChartsOfAccountDA.GetDependencies(tc, oVoucherDetail.AccountHeadID);
            NullHandler oReader = new NullHandler(reader);
            if (reader.Read())
            {
                IsCostCenterApply = oReader.GetBoolean("IsCostCenterApply");
                IsBillRefApply = oReader.GetBoolean("IsBillRefApply");
                IsInventoryApply = oReader.GetBoolean("IsInventoryApply");
                IsOrderReferenceApply = oReader.GetBoolean("IsOrderReferenceApply");
                IsChequeApply = (EnumAccountOperationType)oReader.GetInt16("AccountOperationType") == EnumAccountOperationType.BankAccount;
                if (IsChequeApply)
                {
                    IsCostCenterApply = true;
                }
            }
            reader.Close();

            double nCCAmount = GetCCAmount(oVoucherDetail.CCTs);
            double nVBAmount = GetVBAmount(oVoucherDetail.VoucherBillTrs);
            double nIRAmount = GetIRAmount(oVoucherDetail.VPTransactions);
            double nOrderRefAmount = GetOrderRefAmount(oVoucherDetail.VOReferences);
            double nVCAmount = GetVCAmount(oVoucherDetail.VoucherCheques);
            double nDifferAmount = 0;

            #region Cost Center
            if (IsCostCenterApply)
            {
                nDifferAmount = 0;
                nDifferAmount = oVoucherDetail.AmountInCurrency - nCCAmount;
                if (nDifferAmount < 0)
                {
                    nDifferAmount = nDifferAmount * (-1);
                }
                if (nDifferAmount > nDifferAcceptValue)
                {
                    return "Please Check Cost Center Amount for " + oVoucherDetail.AccountHeadName + "!\nLedger Amount :" + Global.MillionFormat(oVoucherDetail.AmountInCurrency) + " but Cost Center Amount : " + Global.MillionFormat(nCCAmount) + "!\nOn " + sReferenceNo;
                }

                #region Check Subledger Reference
                string sSubledgerName ="";
                double nSLBillAmount = 0;
                double nSLOrderRefAmount = 0;
                bool bIsSLBillRefApply = false;
                bool bIsSLOrderRefApply = false;
                foreach (CostCenterTransaction oItem in oVoucherDetail.CCTs)
                {       
                    sSubledgerName ="";
                    bIsSLBillRefApply = false;
                    bIsSLOrderRefApply = false;
                    nSLBillAmount = GetVBAmount(oItem.VBTransactions);                    
                    nSLOrderRefAmount = GetOrderRefAmount(oItem.VOReferences);

                    IDataReader slreader = SubledgerRefConfigDA.GetDependencies(tc, oVoucherDetail.AccountHeadID, oItem.CCID);
                    NullHandler oSLReader = new NullHandler(slreader);
                    if (slreader.Read())
                    {
                        sSubledgerName = oSLReader.GetString("SubledgerName");
                        bIsSLBillRefApply = oSLReader.GetBoolean("IsBillRefApply");
                        bIsSLOrderRefApply = oSLReader.GetBoolean("IsOrderRefApply");                        
                    }
                    slreader.Close();

                    #region Subledger Bill Transaction
                    if (bIsSLBillRefApply)
                    {
                        nDifferAmount = 0;
                        nDifferAmount = oItem.Amount - nSLBillAmount;
                        if (nDifferAmount < 0)
                        {
                            nDifferAmount = nDifferAmount * (-1);
                        }
                        if (nDifferAmount > nDifferAcceptValue)
                        {
                            return "Please Check Subledger Bill Amount for " + oVoucherDetail.AccountHeadName + " & Subledger : " + sSubledgerName + "!\nSubledger Amount :" + Global.MillionFormat(nCCAmount) + " but Bill Amount : " + Global.MillionFormat(nSLBillAmount) + "!\nOn " + sReferenceNo;
                        }
                    }
                    else
                    {
                        if (nSLBillAmount > 0)
                        {
                            return "Subledger Bill Not Applicable for " + oVoucherDetail.AccountHeadName + " & Subledger : " + sSubledgerName + "!\nOn " + sReferenceNo;
                        }
                    }
                    #endregion

                    #region Subledger Order Transaction
                    if (bIsSLOrderRefApply)
                    {
                        nDifferAmount = 0;
                        nDifferAmount = oItem.Amount - nSLOrderRefAmount;
                        if (nDifferAmount < 0)
                        {
                            nDifferAmount = nDifferAmount * (-1);
                        }
                        if (nDifferAmount > nDifferAcceptValue)
                        {
                            return "Please Check Subledger Order Amount for " + oVoucherDetail.AccountHeadName + " & Subledger : " + sSubledgerName + "!\nSubledger Amount :" + Global.MillionFormat(nCCAmount) + " but Order Amount : " + Global.MillionFormat(nSLOrderRefAmount) + "!\nOn " + sReferenceNo;
                        }
                    }
                    else
                    {
                        if (nSLOrderRefAmount > 0)
                        {
                            return "Subledger Order Not Applicable for " + oVoucherDetail.AccountHeadName + " & Subledger : " + sSubledgerName + "!\nOn " + sReferenceNo;
                        }
                    }
                    #endregion

                }
                #endregion

            }
            else
            {
                if (nCCAmount > 0)
                {
                    return "Cost Center Not Applicable for " + oVoucherDetail.AccountHeadName + "!\nOn " + sReferenceNo;
                }
            }
            #endregion

            #region Voucher Bill Transaction
            if (IsBillRefApply)
            {
                nDifferAmount = 0;
                nDifferAmount = oVoucherDetail.AmountInCurrency - nVBAmount;
                if (nDifferAmount < 0)
                {
                    nDifferAmount = nDifferAmount * (-1);
                }
                if (nDifferAmount > nDifferAcceptValue)
                {
                    return "Please Check Voucher Bill Amount for " + oVoucherDetail.AccountHeadName + "!\nLedger Amount :" + Global.MillionFormat(oVoucherDetail.AmountInCurrency) + " but Bill Amount : " + Global.MillionFormat(nVBAmount) + "!\nOn " + sReferenceNo;
                }
            }
            else
            {
                if (nVBAmount > 0)
                {
                    return "Voucher Bill Not Applicable for " + oVoucherDetail.AccountHeadName + "!\nOn " + sReferenceNo;
                }
            }
            #endregion

            #region Inventory Reference
            if (IsInventoryApply)
            {
                nDifferAmount = 0;
                nDifferAmount = oVoucherDetail.AmountInCurrency - nIRAmount;
                if (nDifferAmount < 0)
                {
                    nDifferAmount = nDifferAmount * (-1);
                }
                if (nDifferAmount > nDifferAcceptValue)
                {
                    return "Please Check Inventory Amount for " + oVoucherDetail.AccountHeadName + "!\nLedger Amount :" + Global.MillionFormat(oVoucherDetail.AmountInCurrency) + " but Inventory Amount : " + Global.MillionFormat(nIRAmount) + "!\nOn " + sReferenceNo;
                }
            }
            else
            {
                if (nIRAmount > 0)
                {
                    return "Inventory Reference Not Applicable for " + oVoucherDetail.AccountHeadName + "!\nOn " + sReferenceNo;
                }
            }
            #endregion

            #region Order Reference
            if (IsOrderReferenceApply)
            {
                nDifferAmount = 0;
                nDifferAmount = oVoucherDetail.AmountInCurrency - nOrderRefAmount;
                if (nDifferAmount < 0)
                {
                    nDifferAmount = nDifferAmount * (-1);
                }
                if (nDifferAmount > nDifferAcceptValue)
                {
                    return "Please Check Order Reference Amount for " + oVoucherDetail.AccountHeadName + "!\nLedger Amount :" + Global.MillionFormat(oVoucherDetail.AmountInCurrency) + " but Order Reference Amount : " + Global.MillionFormat(nOrderRefAmount) + "!\nOn " + sReferenceNo;
                }
            }
            else
            {
                if (nOrderRefAmount > 0)
                {
                    return "Order Reference Not Applicable for " + oVoucherDetail.AccountHeadName + "!\nOn " + sReferenceNo;
                }
            }

            if (IsOrderReferenceApply)
            {
                if (oVoucherDetail.VOReferences != null)
                {
                    foreach (VOReference oItem in oVoucherDetail.VOReferences)
                    {
                        if (oItem.OrderID <= 0 && (oItem.Remarks == "" || oItem.Remarks == null))
                        {
                            return "Please enter sales reference or remarks for " + oVoucherDetail.AccountHeadName + "!\nOn " + sReferenceNo;
                        }
                    }
                }
            }
            #endregion

            #region Voucher Cheque
            //if (IsChequeApply)
            //{
            //    nDifferAmount = 0;
            //    nDifferAmount = oVoucherDetail.AmountInCurrency - nVCAmount;
            //    if (nDifferAmount < 0)
            //    {
            //        nDifferAmount = nDifferAmount * (-1);
            //    }
            //    if (nDifferAmount > nDifferAcceptValue)
            //    {
            //        return "Please Check Voucher Cheque Reference Amount for " + oVoucherDetail.AccountHeadName + "!\nLedger Amount :" + Global.MillionFormat(oVoucherDetail.AmountInCurrency) + " but Voucher Reference Amount : " + Global.MillionFormat(nVCAmount) + "!\nOn " + sReferenceNo;
            //    }
            //}
            //else
            //{
            //    if (nVCAmount > 0)
            //    {
            //        return "Voucher Cheque Reference Not Applicable for " + oVoucherDetail.AccountHeadName + "!\nOn " + sReferenceNo;
            //    }
            //}
            #endregion

            return sErrorMEssage;
        }

        private string FeedBackForDebitCreditEqual(List<VoucherDetail> oVoucherDetails, double nAllowDifferAmount)
        {
            string sFeedBackMessage = "";
            double nSumDebitAmount = 0;
            double nSumCreditAmount = 0;
            if (oVoucherDetails != null)
            {
                if (oVoucherDetails.Count > 0)
                {
                    foreach (VoucherDetail oItem in oVoucherDetails)
                    {
                        if (oItem.IsDebit == true)
                        {
                            nSumDebitAmount = nSumDebitAmount + oItem.Amount;
                        }
                        else
                        {
                            nSumCreditAmount = nSumCreditAmount + oItem.Amount;
                        }
                    }
                    double nDifferAmount = nSumDebitAmount - nSumCreditAmount;
                    if (nDifferAmount < 0)
                    {
                        nDifferAmount = nDifferAmount * (-1);
                    }
                    if (nDifferAmount > nAllowDifferAmount)
                    {
                        sFeedBackMessage = "Please Confirm Debit & Credit Amount must be equal!\nYour Debit Amount:" + Global.MillionFormat(nSumDebitAmount) + " but Credit Amount:" + Global.MillionFormat(nSumCreditAmount);
                    }
                }
                else
                {
                    sFeedBackMessage = "Please Enter At Least One Debit Item";
                }
            }
            else
            {
                sFeedBackMessage = "Please Enter At Least One Debit Item";
            }
            return sFeedBackMessage;
        }
        private string FeedBackForDataValidation(List<VoucherDetail> oVoucherDetails)
        {
            string sFeedBackMessage = "";
            if (oVoucherDetails != null)
            {
                if (oVoucherDetails.Count > 0)
                {
                    foreach (VoucherDetail oItem in oVoucherDetails)
                    {
                        #region Voucher Detail
                        if (oItem.AccountHeadID <= 0)
                        {
                            return "Invalid Account Head!";
                        }                        
                        if (oItem.CurrencyID <= 0)
                        {
                            return "Invalid Currency for " + oItem.AccountHeadName + "[" + oItem.AccountHeadCode + "]!";
                        }
                        if (oItem.AmountInCurrency <= 0)
                        {
                            return "Invalid Amount In Currency for " + oItem.AccountHeadName + "[" + oItem.AccountHeadCode + "]!";
                        }
                        if (oItem.ConversionRate <= 0)
                        {
                            return "Invalid Conversion Rate for " + oItem.AccountHeadName + "[" + oItem.AccountHeadCode + "]!";
                        }
                        if (oItem.Amount <= 0)
                        {
                            return "Invalid Amount for " + oItem.AccountHeadName + "[" + oItem.AccountHeadCode + "]!";
                        }
                        #endregion
                    }
                }
                else
                {
                    sFeedBackMessage = "Please Enter At Least One Debit Item";
                }
            }
            else
            {
                sFeedBackMessage = "Please Enter At Least One Debit Item";
            }
            return sFeedBackMessage;
        }        
        private double GetCCAmount(List<CostCenterTransaction> oCostCenterTransactions)
        {
            double nCCAmount = 0;
            if (oCostCenterTransactions != null)
            {
                foreach (CostCenterTransaction oItem in oCostCenterTransactions)
                {
                    nCCAmount = nCCAmount + oItem.Amount;
                }
            }
            return nCCAmount;
        }

        private double GetVBAmount(List<VoucherBillTransaction> oVoucherBillTransactions)
        {
            double nVBAmount = 0;
            if (oVoucherBillTransactions != null)
            {
                foreach (VoucherBillTransaction oItem in oVoucherBillTransactions)
                {
                    nVBAmount = nVBAmount + oItem.Amount;
                }
            }
            return nVBAmount;
        }

        private double GetIRAmount(List<VPTransaction> oVPTransactions)
        {
            double nIRAmount = 0;
            if (oVPTransactions != null)
            {
                foreach (VPTransaction oItem in oVPTransactions)
                {
                    nIRAmount = nIRAmount + oItem.Amount;
                }
            }
            return nIRAmount;
        }

        private double GetOrderRefAmount(List<VOReference> oVOReferences)
        {
            double nOrderRefAmount = 0;
            if (oVOReferences != null)
            {
                foreach (VOReference oItem in oVOReferences)
                {
                    nOrderRefAmount = nOrderRefAmount + oItem.AmountInCurrency;
                }
            }
            return nOrderRefAmount;
        }

        private double GetVCAmount(List<VoucherCheque> oVoucherCheques)
        {
            double nVRAmount = 0;
            if (oVoucherCheques != null)
            {
                foreach (VoucherCheque oItem in oVoucherCheques)
                {
                    nVRAmount = nVRAmount + oItem.Amount;
                }
            }
            return nVRAmount;
        }
        #endregion

        public VoucherService() { }

        #region Interface implementation     
        #region Manual Voucher
        public Voucher Save(Voucher oVoucher, int nUserId)
        {           
            TransactionContext tc = null;
            try
            {   
                tc = TransactionContext.Begin(true);
                List<VoucherDetail> oVoucherDetails = new List<VoucherDetail>();
                List<CostCenterTransaction> oCostCenterTransactions = new List<CostCenterTransaction>();
                List<VoucherCheque> oVoucherCheques = new List<VoucherCheque>();
                List<VoucherBillTransaction> VoucherBillTrs = new List<VoucherBillTransaction>();
                List<VPTransaction> oVPTransactions = new List<VPTransaction>();
                List<VOReference> oVOReferences = new List<VOReference>();

                //oVoucherDetails = this.RoundingDecimalValue(oVoucher.VoucherDetailLst);
                oVoucherDetails = oVoucher.VoucherDetailLst;
                string sVoucherDetailIDs = "";
                int nVoucherDetailID = 0;
                int nCurrencyID = 0;
                double nConversionRate = 0;
                string sErrorMessage = "";

                #region Input Validation
                #region Voucher Data Validation
                sErrorMessage = "";
                sErrorMessage = this.FeedBackForDataValidation(oVoucherDetails);
                if (sErrorMessage != "")
                {
                    throw new Exception(sErrorMessage);
                }
                #endregion

                #region Debit Credit Equal
                sErrorMessage = "";
                sErrorMessage = this.FeedBackForDebitCreditEqual(oVoucherDetails, 0.01);
                if (sErrorMessage != "")
                {
                    throw new Exception(sErrorMessage);
                }
                #endregion

                #region Reference Value Check
                foreach (VoucherDetail oTempVoucherDetail in oVoucherDetails)
                {
                    sErrorMessage = "";
                    sErrorMessage = this.AccountHeadWiseInputValidation(tc, oTempVoucherDetail, "", 1.00);
                    if (sErrorMessage != "")
                    {
                        throw new Exception(sErrorMessage);
                    }
                }
                #endregion
                #endregion

                #region Voucher
                IDataReader reader;
                EnumRoleOperationType eRole = EnumRoleOperationType.None;
                if (oVoucher.VoucherID <= 0)
                {
                    eRole = EnumRoleOperationType.Add;
                    AuthorizationRoleDA.CheckUserPermission(tc, nUserId, EnumModuleName.Voucher, EnumRoleOperationType.Add);
                    reader = VoucherDA.InsertUpdate(tc, oVoucher, EnumDBOperation.Insert, nUserId);
                }
                else
                {
                    eRole = EnumRoleOperationType.Edit;
                    AuthorizationRoleDA.CheckUserPermission(tc, nUserId, EnumModuleName.Voucher, EnumRoleOperationType.Edit);
                    DBOperationArchiveDA.Insert(tc, EnumDBOperation.Update, EnumModuleName.Voucher, (int)oVoucher.VoucherID, "View_Voucher", "VoucherID", "BUID", "VoucherNo", nUserId);
                    reader = VoucherDA.InsertUpdate(tc, oVoucher, EnumDBOperation.Update, nUserId);
                }
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oVoucher = new Voucher();
                    oVoucher = CreateObject(oReader);
                }
                reader.Close();
                #endregion

                #region Voucher History
                IDataReader historyreader;
                VoucherHistory oVoucherHistory=new VoucherHistory();
                oVoucherHistory.UserID=nUserId;
                oVoucherHistory.VoucherID=(int)oVoucher.VoucherID;
                oVoucherHistory.TransactionDate = DateTime.Now;
                oVoucherHistory.ActionType = eRole;
                oVoucherHistory.Remarks = oVoucher.Narration;
                historyreader = VoucherHistoryDA.InsertUpdate(tc, oVoucherHistory, EnumDBOperation.Insert, nUserId);
                historyreader.Close();
                #endregion

                #region Voucher Details
                if (oVoucherDetails != null)
                {
                    bool bIsUpdate = false;
                    foreach (VoucherDetail oItem in oVoucherDetails)
                    {
                        oCostCenterTransactions = oItem.CCTs;
                        oVoucherCheques = oItem.VoucherCheques;
                        VoucherBillTrs = oItem.VoucherBillTrs;
                        oVPTransactions = oItem.VPTransactions;
                        oVOReferences = oItem.VOReferences;

                        oItem.VoucherID = oVoucher.VoucherID;
                        nCurrencyID = oItem.CurrencyID;
                        nConversionRate = oItem.ConversionRate;

                        IDataReader readerdetail;
                        if (oItem.VoucherDetailID <= 0)
                        {
                            bIsUpdate = false;
                            readerdetail = VoucherDetailDA.InsertUpdate(tc, oItem, EnumDBOperation.Insert, "");
                        }
                        else
                        {
                            bIsUpdate = true;
                            readerdetail = VoucherDetailDA.InsertUpdate(tc, oItem, EnumDBOperation.Update, "");
                        }
                        NullHandler oReaderDetail = new NullHandler(readerdetail);

                        if (readerdetail.Read())
                        {
                            sVoucherDetailIDs = sVoucherDetailIDs + oReaderDetail.GetString("VoucherDetailID") + ",";
                            nVoucherDetailID = oReaderDetail.GetInt32("VoucherDetailID");
                        }
                        readerdetail.Close();

                        #region Delete Voucher Referrences
                        if (bIsUpdate)
                        {
                            #region Delete Subledger
                            CostCenterTransaction oCostCenterTransaction = new CostCenterTransaction();
                            oCostCenterTransaction.VoucherDetailID = nVoucherDetailID;
                            CostCenterTransactionDA.Delete(tc, oCostCenterTransaction, EnumDBOperation.Delete, nUserId);
                            #endregion
                            #region Delete VoucherBillTransactions
                            VoucherBillTransaction oVoucherBillTr = new VoucherBillTransaction();
                            oVoucherBillTr.VoucherDetailID = nVoucherDetailID;
                            VoucherBillTransactionDA.Delete(tc, oVoucherBillTr, EnumDBOperation.Delete, nUserId);
                            #endregion
                            #region Delete VoucherCheques Transaction
                            VoucherCheque oVoucherCheque = new VoucherCheque();
                            oVoucherCheque.VoucherDetailID = nVoucherDetailID;
                            VoucherChequeDA.Delete(tc, oVoucherCheque, EnumDBOperation.Delete, nUserId);
                            #endregion
                            #region Delete VPTransaction Transaction
                            VPTransaction oVPTransaction = new VPTransaction();
                            oVPTransaction.VoucherDetailID = nVoucherDetailID;
                            VPTransactionDA.Delete(tc, oVPTransaction, EnumDBOperation.Delete, nUserId);
                            #endregion
                            #region Delete Order Reference
                            VOReference oVOReference = new VOReference();
                            oVOReference.VoucherDetailID = nVoucherDetailID;
                            VOReferenceDA.Delete(tc, oVOReference, EnumDBOperation.Delete, nUserId);
                            #endregion
                        }
                        #endregion

                        #region CostCenterTransactions
                        if (oCostCenterTransactions.Count > 0)
                        {
                            int nCCTID = 0;
                            List<VoucherBillTransaction> oVBTransactions = new List<VoucherBillTransaction>();
                            List<VOReference> oSLVOReferences = new List<VOReference>();
                            List<VoucherCheque> oSLCheques = new List<VoucherCheque>();
                            foreach (CostCenterTransaction oCCT in oCostCenterTransactions)
                            {
                                nCCTID = 0;
                                oVBTransactions = oCCT.VBTransactions;
                                oSLVOReferences = oCCT.VOReferences;
                                oSLCheques = oCCT.VoucherCheques;
                                IDataReader readerCCT;
                                oCCT.VoucherDetailID = nVoucherDetailID;
                                oCCT.CurrencyID = nCurrencyID;
                                oCCT.CurrencyConversionRate = nConversionRate;
                                readerCCT = CostCenterTransactionDA.InsertUpdate(tc, oCCT, EnumDBOperation.Insert, nUserId);
                                NullHandler oReaderCCT = new NullHandler(readerCCT);
                                if (readerCCT.Read())
                                {
                                    nCCTID = oReaderCCT.GetInt32("CCTID");
                                }
                                readerCCT.Close();

                                #region Subledger Bills
                                if (oVBTransactions.Count > 0)
                                {
                                    foreach (VoucherBillTransaction oVBT in oVBTransactions)
                                    {
                                        IDataReader readerSLBT;
                                        oVBT.VoucherDetailID = nVoucherDetailID;
                                        oVBT.CCTID = nCCTID;
                                        oVBT.CurrencyID = nCurrencyID;
                                        oVBT.ConversionRate = nConversionRate;
                                        readerSLBT = VoucherBillTransactionDA.InsertUpdate(tc, oVBT, EnumDBOperation.Insert, nUserId);
                                        NullHandler oReaderVBT = new NullHandler(readerSLBT);
                                        readerSLBT.Close();
                                    }
                                }
                                #endregion

                                #region Subledger Order Ref
                                if (oSLVOReferences.Count > 0)
                                {
                                    foreach (VOReference oVOR in oSLVOReferences)
                                    {
                                        IDataReader readerSLOR;
                                        oVOR.VoucherDetailID = nVoucherDetailID;
                                        oVOR.CCTID = nCCTID;
                                        oVOR.CurrencyID = nCurrencyID;
                                        oVOR.ConversionRate = nConversionRate;
                                        readerSLOR = VOReferenceDA.InsertUpdate(tc, oVOR, EnumDBOperation.Insert, nUserId);
                                        NullHandler oReaderSLOR = new NullHandler(readerSLOR);
                                        readerSLOR.Close();
                                    }
                                }
                                #endregion

                                #region Subledger Cheques
                                if (oSLCheques.Count > 0)
                                {
                                    foreach (VoucherCheque oVC in oSLCheques)
                                    {
                                        IDataReader readerSLVC;
                                        oVC.VoucherDetailID = nVoucherDetailID;
                                        oVC.CCTID = nCCTID;
                                        readerSLVC = VoucherChequeDA.InsertUpdate(tc, oVC, EnumDBOperation.Insert, nUserId);
                                        NullHandler oReaderVBT = new NullHandler(readerSLVC);
                                        readerSLVC.Close();
                                    }
                                }
                                #endregion
                            }

                            //Update Voucher Amount as CostCenter Transaction Amount if one Voucher Detail has only one Subledger
                            if (oCostCenterTransactions.Count == 1 && nCCTID > 0 && nVoucherDetailID > 0)
                            {
                                CostCenterTransaction oCostCenterTransaction = new CostCenterTransaction();
                                oCostCenterTransaction.CCTID = nCCTID;
                                oCostCenterTransaction.VoucherDetailID = nVoucherDetailID;
                                CostCenterTransactionDA.Update(tc, oCostCenterTransaction, EnumDBOperation.Update, nUserId);
                            }
                        }
                        #endregion
                        
                        #region VoucherBillTransactions
                        if (VoucherBillTrs.Count > 0)
                        {
                            foreach (VoucherBillTransaction oVBT in VoucherBillTrs)
                            {
                                IDataReader readerVBT;
                                oVBT.VoucherDetailID = nVoucherDetailID;
                                oVBT.CCTID = 0;
                                oVBT.CurrencyID = nCurrencyID;
                                oVBT.ConversionRate = nConversionRate;
                                readerVBT = VoucherBillTransactionDA.InsertUpdate(tc, oVBT, EnumDBOperation.Insert, nUserId);
                                NullHandler oReaderVBT = new NullHandler(readerVBT);
                                readerVBT.Close();
                            }
                        }
                        #endregion
                        
                        #region VoucherCheques
                        if (oVoucherCheques.Count > 0)
                        {
                            foreach (VoucherCheque oVC in oVoucherCheques)
                            {
                                IDataReader readerVC;
                                oVC.VoucherDetailID = nVoucherDetailID;
                                readerVC = VoucherChequeDA.InsertUpdate(tc, oVC, EnumDBOperation.Insert, nUserId);
                                NullHandler oReaderCCT = new NullHandler(readerVC);
                                readerVC.Close();
                            }
                        }
                        #endregion

                        #region VPTransactions
                        if (oVPTransactions.Count > 0)
                        {
                            foreach (VPTransaction oVPT in oVPTransactions)
                            {
                                IDataReader readerVPTT;
                                oVPT.VoucherDetailID = nVoucherDetailID;
                                oVPT.CurrencyID = nCurrencyID;
                                oVPT.ConversionRate = nConversionRate;
                                readerVPTT = VPTransactionDA.InsertUpdate(tc, oVPT, EnumDBOperation.Insert, nUserId);
                                NullHandler oReaderVPT = new NullHandler(readerVPTT);
                                readerVPTT.Close();
                            }
                        }
                        #endregion

                        #region VOReferences
                        if (oVOReferences.Count > 0)
                        {
                            foreach (VOReference oVOReference in oVOReferences)
                            {
                                IDataReader readerOReference;
                                oVOReference.VoucherDetailID = nVoucherDetailID;
                                oVOReference.CurrencyID = nCurrencyID;
                                oVOReference.ConversionRate = nConversionRate;
                                readerOReference = VOReferenceDA.InsertUpdate(tc, oVOReference, EnumDBOperation.Insert, nUserId);
                                NullHandler oReaderOrderReference = new NullHandler(readerOReference);
                                readerOReference.Close();
                            }
                        }
                        #endregion
                    }
                    if (sVoucherDetailIDs.Length > 0)
                    {
                        sVoucherDetailIDs = sVoucherDetailIDs.Remove(sVoucherDetailIDs.Length - 1, 1);
                    }

                    VoucherDetail oVoucherDetail = new VoucherDetail();
                    oVoucherDetail.VoucherID = oVoucher.VoucherID;
                    VoucherDetailDA.Delete(tc, oVoucherDetail, EnumDBOperation.Delete, sVoucherDetailIDs);
                }
                #endregion

                #region Counter Voucher 
                //if (oVoucher.VoucherCategory == EnumVoucherCategory.Purchase)
                //{
                //    VoucherDA.CounterVoucher(tc, oVoucher.VoucherID, nUserId);
                //}
                #endregion

                #region Get Voucher
                IDataReader readerVoucher;
                readerVoucher = VoucherDA.Get(tc, oVoucher.VoucherID);
                NullHandler oReaderVoucher = new NullHandler(readerVoucher);
                if (readerVoucher.Read())
                {
                    oVoucher = new Voucher();
                    oVoucher = CreateObject(oReaderVoucher);
                }
                readerVoucher.Close();
                #endregion

                #region Delete Counter Voucher
                //if (oVoucher.CounterVoucherID > 0 && oVoucher.VoucherCategory == EnumVoucherCategory.Sales)
                //{
                //    VoucherDA.CounterVoucherDelete(tc, oVoucher.VoucherID, oVoucher.CounterVoucherID);
                //}
                #endregion

                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                oVoucher = new Voucher();
                oVoucher.ErrorMessage = e.Message.Split('~')[0];                
                #endregion
            }
            return oVoucher;
        }
        public Voucher UpdatePrintCount(Voucher oVoucher, int nUserId)
        {
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);                
                VoucherDA.UpdatePrintCount(tc, oVoucher, nUserId);                
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                oVoucher = new Voucher();
                oVoucher.ErrorMessage = e.Message.Split('~')[0];                
                #endregion
            }
            return oVoucher;
        }                
        public Voucher ApprovedVoucher(Voucher oVoucher, int nUserId)
        {
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
                IDataReader reader;
                oVoucher.AuthorizedBy =Convert.ToInt32(nUserId);
                AuthorizationRoleDA.CheckUserPermission(tc, nUserId, EnumModuleName.Voucher, EnumRoleOperationType.Approved);
                reader = VoucherDA.ApprovedVoucher(tc, oVoucher, false);                
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oVoucher = new Voucher();
                    oVoucher = CreateObject(oReader);
                }
                reader.Close();

                
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                oVoucher = new Voucher();
                oVoucher.ErrorMessage = e.Message.Split('~')[0];
                //ExceptionLog.Write(e);

                //throw new ServiceException("Failed to Save Voucher. Because of " + e.Message, e);
                #endregion
            }
            return oVoucher;
        }
        public List<Voucher> ApprovedVouchers(Voucher oVoucher, int nUserId)
        {
            List<Voucher> oVouchers = new List<Voucher>();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();
                IDataReader reader = null;
                oVoucher.AuthorizedBy = Convert.ToInt32(nUserId);
                reader = VoucherDA.ApprovedVoucher(tc, oVoucher, false);
                oVouchers = CreateObjects(reader);
                reader.Close();


                #region Voucher History
                foreach (Voucher oItem in oVouchers)
                {
                    IDataReader historyreader;
                    VoucherHistory oVoucherHistory = new VoucherHistory();
                    oVoucherHistory.UserID = nUserId;
                    oVoucherHistory.VoucherID = (int)oItem.VoucherID;
                    oVoucherHistory.TransactionDate = DateTime.Now;
                    oVoucherHistory.ActionType = EnumRoleOperationType.Approved;
                    oVoucherHistory.Remarks = oItem.Narration;
                    historyreader = VoucherHistoryDA.InsertUpdate(tc, oVoucherHistory, EnumDBOperation.Insert, nUserId);
                    historyreader.Close();
                }
                #endregion
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                oVouchers = new List<Voucher>();
                oVoucher = new Voucher();
                oVoucher.ErrorMessage = e.Message.Split('~')[0];
                oVouchers.Add(oVoucher);
                #endregion
            }

            return oVouchers;
        }
        public List<Voucher> CommitInventoryEffect(List<Voucher> oVouchers, int nUserID)
        {
            Voucher oVoucher = new Voucher();
            List<Voucher> oTempVouchers = new List<Voucher>();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();
                foreach (Voucher oItem in oVouchers)
                {
                    if (oItem.VoucherCategory == EnumVoucherCategory.Sales)
                    {
                        //Commit Counter Voucher
                        VoucherDA.CounterVoucher(tc, oItem.VoucherID, nUserID);

                        #region Get Sales Voucher
                        IDataReader readerVoucher;
                        readerVoucher = VoucherDA.Get(tc, oItem.VoucherID);
                        NullHandler oReaderVoucher = new NullHandler(readerVoucher);
                        if (readerVoucher.Read())
                        {
                            oVoucher = new Voucher();
                            oVoucher = CreateObject(oReaderVoucher);
                            oTempVouchers.Add(oVoucher);
                        }
                        readerVoucher.Close();
                        #endregion

                        #region Approved Counter Voucher
                        if (oVoucher.AuthorizedBy != 0 && oVoucher.CounterVoucherID > 0)
                        {
                            VoucherDA.CounterVoucherApproved(tc, nUserID, oVoucher.CounterVoucherID);
                        }
                        #endregion
                    }
                    else
                    {
                        #region Get Sales Voucher
                        IDataReader readerVoucher;
                        readerVoucher = VoucherDA.Get(tc, oItem.VoucherID);
                        NullHandler oReaderVoucher = new NullHandler(readerVoucher);
                        if (readerVoucher.Read())
                        {
                            oVoucher = new Voucher();
                            oVoucher = CreateObject(oReaderVoucher);
                            oTempVouchers.Add(oVoucher);
                        }
                        readerVoucher.Close();
                        #endregion
                    }
                }
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                oTempVouchers = new List<Voucher>();
                oVoucher = new Voucher();
                oVoucher.ErrorMessage = e.Message.Split('~')[0];
                oTempVouchers.Add(oVoucher);
                #endregion
            }
            return oTempVouchers;
        }
        public Voucher CommitVoucherNo(int nBUID, int nVoucherTypeID, DateTime dVoucherDate, int nUserId)
        {
            TransactionContext tc = null;
            Voucher oVoucher = new Voucher();
            try
            {
                tc = TransactionContext.Begin(true);
                IDataReader reader;
                reader = VoucherDA.CommitVoucherNo(tc,  nBUID, nVoucherTypeID, dVoucherDate);               
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oVoucher = new Voucher();
                    oVoucher.BUID = oReader.GetInt32("BUID");
                    oVoucher.VoucherNo = oReader.GetString("VoucherNo");
                    oVoucher.VoucherName = oReader.GetString("VoucherName");
                    oVoucher.NumberMethod = (EnumNumberMethod)oReader.GetInt32("NumberMethod");
                    oVoucher.NumberMethodInInt = oReader.GetInt32("NumberMethod");
                    oVoucher.CurrencyID = oReader.GetInt32("CurrencyID");
                    oVoucher.CUSymbol = oReader.GetString("CUSymbol");
                    oVoucher.VoucherDate = dVoucherDate;                  
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
                throw new ServiceException("Failed to Save Voucher. Because of " + e.Message, e);
                #endregion
            }
            return oVoucher;
        }
        public Voucher GetMaxDate(int nVType, int nBUID, int nUserId)
        {
            Voucher oAccountHead = new Voucher();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = VoucherDA.GetMaxDate(tc, nVType, nBUID);
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
                throw new ServiceException(e.Message);
                #endregion
            }

            return oAccountHead;
        }        
        public string Delete(int id, int nUserId)
        {
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
                Voucher oVoucher = new Voucher();
                oVoucher.VoucherID = id;
                AuthorizationRoleDA.CheckUserPermission(tc, nUserId, EnumModuleName.Voucher, EnumRoleOperationType.Delete);
                DBOperationArchiveDA.Insert(tc, EnumDBOperation.Delete, EnumModuleName.Voucher, (int)oVoucher.VoucherID, "View_Voucher", "VoucherID", "BUID", "VoucherNo", nUserId);
                VoucherDA.Delete(tc, oVoucher, EnumDBOperation.Delete, nUserId);
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                return e.Message.Split('~')[0];
                //ExceptionLog.Write(e);
                //throw new ServiceException("Failed to Delete Voucher. Because of " + e.Message, e);
                #endregion
            }
            return "Data Delete Successfully";
        }
        public Voucher UnApprovedVoucher(Int64 id, int nUserId)
        {
            Voucher oVoucher = new Voucher();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);

                #region Un Approved Voucher
                AuthorizationRoleDA.CheckUserPermission(tc, nUserId, EnumModuleName.Voucher, EnumRoleOperationType.UnApproved);
                VoucherDA.UnApprovedVoucher(tc, id);
                #endregion

                #region Voucher History
                IDataReader historyreader;
                VoucherHistory oVoucherHistory = new VoucherHistory();
                oVoucherHistory.UserID = nUserId;
                oVoucherHistory.VoucherID = (int)id;
                oVoucherHistory.TransactionDate = DateTime.Now;
                oVoucherHistory.ActionType =  EnumRoleOperationType.UnApproved;
                oVoucherHistory.Remarks = "Un approved Voucher";
                historyreader = VoucherHistoryDA.InsertUpdate(tc, oVoucherHistory, EnumDBOperation.Insert, nUserId);
                historyreader.Close();
                #endregion

                #region Get Voucher
                IDataReader readerVoucher;
                readerVoucher = VoucherDA.Get(tc, id);
                NullHandler oReaderVoucher = new NullHandler(readerVoucher);
                if (readerVoucher.Read())
                {
                    oVoucher = new Voucher();
                    oVoucher = CreateObject(oReaderVoucher);
                }
                readerVoucher.Close();
                #endregion

                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                oVoucher = new Voucher();
                oVoucher.ErrorMessage = e.Message.Split('~')[0];
                #endregion
            }
            return oVoucher;
        }
        public Voucher Get(long id, int nUserId)
        {
            Voucher oAccountHead = new Voucher();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = VoucherDA.Get(tc, id);
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
                throw new ServiceException(e.Message);
                #endregion
            }

            return oAccountHead;
        }

        public Voucher LastNarration(int nUserId)
        {
            Voucher oAccountHead = new Voucher();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = VoucherDA.LastNarration(tc, nUserId);
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
                throw new ServiceException(e.Message);
                #endregion
            }

            return oAccountHead;
        }
        public Voucher GetProfitLossAppropriationAccountVoucher(int nBUID, DateTime dStartDate, DateTime dEndDate, int nUserID)
        {
            Voucher oAccountHead = new Voucher();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = VoucherDA.GetProfitLossAppropriationAccountVoucher(tc, nBUID, dStartDate, dEndDate);
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
                throw new ServiceException(e.Message);
                #endregion
            }

            return oAccountHead;
        }        
        public List<Voucher> GetsWaitForApproval(int nUserId)
        {
            List<Voucher> oVoucher = new List<Voucher>();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();
                IDataReader reader = null;
                reader = VoucherDA.GetsWaitForApproval(tc, nUserId);
                oVoucher = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                throw new ServiceException(e.Message);
                #endregion
            }

            return oVoucher;
        }            
        public List<Voucher> Gets(int nUserId)
        {
            List<Voucher> oVoucher = null;

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = VoucherDA.Gets(tc);
                oVoucher = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                throw new ServiceException(e.Message);
                #endregion
            }

            return oVoucher;
        }

        public List<Voucher> GetsByBatch(int nVoucherBatchID, int nUserID)
        {
            List<Voucher> oVoucher = new List<Voucher>();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();
                IDataReader reader = null;
                reader = VoucherDA.GetsByBatch(tc, nVoucherBatchID);
                oVoucher = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                throw new ServiceException(e.Message);
                #endregion
            }

            return oVoucher;
        }
        public List<Voucher> GetsByBatchForApprove(int nVoucherBatchID, int nUserID)
        {
            List<Voucher> oVoucher = new List<Voucher>();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();
                IDataReader reader = null;
                reader = VoucherDA.GetsByBatchForApprove(tc, nVoucherBatchID);
                oVoucher = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                throw new ServiceException(e.Message);
                #endregion
            }

            return oVoucher;
        }
        public List<Voucher> Gets(string sSQL, int nUserId)
        {
            List<Voucher> oVoucher = null;

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = VoucherDA.Gets(tc, sSQL);
                oVoucher = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                throw new ServiceException(e.Message);
                #endregion
            }

            return oVoucher;
        }
        public List<Voucher> GetsByVoucherType(int nVoucherTypeID, int nUserId)
        {
            List<Voucher> oVoucher = null;

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = VoucherDA.GetsByVoucherType(tc, nVoucherTypeID);
                oVoucher = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                throw new ServiceException(e.Message);
                #endregion
            }

            return oVoucher;
        }
        public Voucher CommitProfitLossAccounts(int nBUID, int nSessionID, int nUserId)
        {
            TransactionContext tc = null;
            Voucher oVoucher = new Voucher();
            try
            {
                tc = TransactionContext.Begin(true);                
                IDataReader reader;
                reader = VoucherDA.CommitProfitLossAccounts(tc, nBUID, nSessionID, nUserId);                
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oVoucher = new Voucher();
                    oVoucher = CreateObject(oReader);
                }
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                oVoucher = new Voucher();
                oVoucher.ErrorMessage = e.Message.Split('~')[0];                
                #endregion
            }
            return oVoucher;
        }
        public Voucher CommitProfitLossAppropriationAccounts(Voucher oVoucher, int nUserId)
        {
            TransactionContext tc = null;            
            try
            {
                tc = TransactionContext.Begin(true);
                IDataReader reader;
                reader = VoucherDA.CommitProfitLossAppropriationAccounts(tc, oVoucher, nUserId);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oVoucher = new Voucher();
                    oVoucher = CreateObject(oReader);
                }
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                oVoucher = new Voucher();
                oVoucher.ErrorMessage = e.Message.Split('~')[0];
                #endregion
            }
            return oVoucher;
        }
        #endregion

        #region Auto Voucher
        private string GetVoucherNo(TransactionContext tc, int nBUID, int nVoucherTypeID, DateTime dVoucherDate, Int64 nUserID)
        {
            string sVoucherNo = "";
            IDataReader reader;
            reader = VoucherDA.CommitVoucherNo(tc, nBUID, nVoucherTypeID, dVoucherDate);
            NullHandler oReader = new NullHandler(reader);
            if (reader.Read())
            {
                sVoucherNo = oReader.GetString("VoucherNo");
            }
            reader.Close();
            return sVoucherNo;
        }
        private object[] GetPerameterValue(string sParam, DataRow oDataRow)
        {
            string[] aParams = sParam.Split(',');
            object[] args = new object[aParams.Length];

            int i = 0;
            foreach (string oItem in aParams)
            {
                args[i] = oDataRow[oItem];
                i++;
            }
            return args;
        }
        private List<DataCollectionSetup> GetDataCollectionSetups(EnumDataSetupType eEnumDataSetupType, List<DataCollectionSetup> oDataCollectionSetups)
        {
            List<DataCollectionSetup> oTempDataCollectionSetups = new List<DataCollectionSetup>();
            foreach (DataCollectionSetup oItem in oDataCollectionSetups)
            {
                if (oItem.DataSetupType == eEnumDataSetupType)
                {
                    oTempDataCollectionSetups.Add(oItem);
                }
            }
            return oTempDataCollectionSetups;
        }
        private string GetDataCollectSQL(List<DataCollectionSetup> oDataCollectionSetups, DataRow oDataRow)
        {
            string sSQL = "";
            foreach (DataCollectionSetup oItem in oDataCollectionSetups)
            {
                if (oItem.DataGenerateType == EnumDataGenerateType.AutomatedData)
                {
                    sSQL = sSQL + "(" + SQLParser.MakeSQL(oItem.QueryForValue, this.GetPerameterValue(oItem.ReferenceValueFields, oDataRow)) + ") +";
                }
                else if (oItem.DataGenerateType == EnumDataGenerateType.FixedData)
                {
                    sSQL = sSQL + " ' " + oItem.FixedText + " '+";
                }
                else if (oItem.DataGenerateType == EnumDataGenerateType.ManualData)
                {
                    sSQL = sSQL + " 1+";
                }
            }
            if (sSQL.Length > 0)
            {
                sSQL = sSQL.Remove(sSQL.Length - 1, 1);
            }
            return sSQL;
        }
        private Voucher GetVocuherData(TransactionContext tc, DataRow oDataRow, IntegrationSetupDetail oIntegrationSetupDetail, Int64 nUserID)
        {
            string sSQL = ""; string sTempSQL = ""; string sAdvSQL = "";
            Voucher oVoucher = new Voucher();
            List<DataCollectionSetup> oDataCollectionSetups = new List<DataCollectionSetup>();

            #region BUID, Code & Name
            #region BUID
            oDataCollectionSetups = new List<DataCollectionSetup>();
            oDataCollectionSetups = this.GetDataCollectionSetups(EnumDataSetupType.BusinessUnitSetup, oIntegrationSetupDetail.DataCollectionSetups);
            sTempSQL = this.GetDataCollectSQL(oDataCollectionSetups, oDataRow);
            sSQL = sSQL + " (" + sTempSQL + ") AS BUID,";
            sAdvSQL = sTempSQL;
            #endregion

            #region BUCode
            sTempSQL = "SELECT HH.Code FROM BusinessUnit AS HH WHERE BusinessUnitID=(" + sAdvSQL + ")";
            sSQL = sSQL + " (" + sTempSQL + ") AS BUCode,";
            #endregion

            #region BUCode
            sTempSQL = "SELECT HH.Name FROM BusinessUnit AS HH WHERE BusinessUnitID=(" + sAdvSQL + ")";
            sSQL = sSQL + " (" + sTempSQL + ") AS BUName,";
            #endregion
            #endregion

            #region Voucher Date
            oDataCollectionSetups = new List<DataCollectionSetup>();
            oDataCollectionSetups = this.GetDataCollectionSetups(EnumDataSetupType.VoucherDateSetup, oIntegrationSetupDetail.DataCollectionSetups);
            sTempSQL = this.GetDataCollectSQL(oDataCollectionSetups, oDataRow);
            sSQL = sSQL + " (" + sTempSQL + ") AS VoucherDate,";
            #endregion

            #region Voucher Narration
            oDataCollectionSetups = new List<DataCollectionSetup>();
            oDataCollectionSetups = this.GetDataCollectionSetups(EnumDataSetupType.NarrationSetup, oIntegrationSetupDetail.DataCollectionSetups);
            sTempSQL = this.GetDataCollectSQL(oDataCollectionSetups, oDataRow);
            sSQL = sSQL + " (" + sTempSQL + ") AS Narration,";
            #endregion

            #region Voucher ReferenceNote
            oDataCollectionSetups = new List<DataCollectionSetup>();
            oDataCollectionSetups = this.GetDataCollectionSetups(EnumDataSetupType.ReferenceNoteSetup, oIntegrationSetupDetail.DataCollectionSetups);
            sTempSQL = this.GetDataCollectSQL(oDataCollectionSetups, oDataRow);
            sSQL = sSQL + " (" + sTempSQL + ") AS ReferenceNote,";
            #endregion

            #region BaseCurrencyID
            sTempSQL = "SELECT BaseCurrencyID FROM View_Company WHERE CompanyID=1";
            sSQL = sSQL + " (" + sTempSQL + ") AS BaseCurrencyID,";
            #endregion

            #region BaseCUSymbol
            sTempSQL = "SELECT CurrencySymbol FROM View_Company WHERE CompanyID=1";
            sSQL = sSQL + " (" + sTempSQL + ") AS BaseCUSymbol,";
            #endregion

            #region BatchID
            sTempSQL = "SELECT MAX(VoucherBatchID) FROM VoucherBatch WHERE CreateBy=" + nUserID.ToString() + " AND BatchStatus=1";
            sSQL = sSQL + " (" + sTempSQL + ") AS BatchID";
            #endregion

            #region SQL Execute
            IDataReader reader;
            sSQL = "SELECT " + sSQL;
            reader = VoucherDA.Gets(tc, sSQL);
            NullHandler oDataReader = new NullHandler(reader);
            if (reader.Read())
            {
                oVoucher = new Voucher();
                oVoucher.BUID = oDataReader.GetInt32("BUID");
                oVoucher.BUCode = oDataReader.GetString("BUCode");
                oVoucher.BUName = oDataReader.GetString("BUName");
                oVoucher.VoucherDate = oDataReader.GetDateTime("VoucherDate");
                oVoucher.Narration = oDataReader.GetString("Narration");
                oVoucher.ReferenceNote = oDataReader.GetString("ReferenceNote");
                oVoucher.BaseCurrencyID = oDataReader.GetInt32("BaseCurrencyID");
                oVoucher.BaseCUSymbol = oDataReader.GetString("BaseCUSymbol");
                oVoucher.BatchID = oDataReader.GetInt32("BatchID");
            }
            reader.Close();
            #endregion

            #region Check BatchID
            if (oVoucher.BatchID <= 0)
            {
                VoucherBatch oVoucherBatch = new VoucherBatch();
                oVoucherBatch.BatchStatus = EnumVoucherBatchStatus.BatchOpen;
                reader = VoucherBatchDA.InsertUpdate(tc, oVoucherBatch, EnumDBOperation.Insert, nUserID);
                oDataReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oVoucher.BatchID = oDataReader.GetInt32("VoucherBatchID");
                }
                reader.Close();
            }
            #endregion
            return oVoucher;
        }
        private VoucherDetail GetVocuherDetailData(TransactionContext tc, Voucher oVoucher, DataRow oRow, DataRow oDataRowDebitCredit, DebitCreditSetup oDebitCreditSetup)
        {
            string sSQL = ""; string sTempSQL = ""; string sAdvSQL = "";
            VoucherDetail oVoucherDetail = new VoucherDetail();
            List<DataCollectionSetup> oDataCollectionSetups = new List<DataCollectionSetup>();

            #region Account Hade
            if (oDebitCreditSetup.AccountHeadType == EnumAccountHeadType.FixedAccountType)
            {
                #region AccountHeadID
                sTempSQL = oDebitCreditSetup.FixedAccountHeadID.ToString();
                sSQL = sSQL + " (" + sTempSQL + ") AS AccountHeadID,";
                #endregion

                #region AccountHead Code
                sTempSQL = "SELECT AccountCode FROM ChartsOfAccount WHERE AccountHeadID=" + oDebitCreditSetup.FixedAccountHeadID.ToString();
                sSQL = sSQL + " (" + sTempSQL + ") AS AccountCode,";
                #endregion

                #region AccountHead Name
                sTempSQL = "SELECT AccountHeadName FROM ChartsOfAccount WHERE AccountHeadID=" + oDebitCreditSetup.FixedAccountHeadID.ToString();
                sSQL = sSQL + " (" + sTempSQL + ") AS AccountHeadName,";
                #endregion
            }
            else if (oDebitCreditSetup.AccountHeadType == EnumAccountHeadType.DecidedAccountHead)
            {
                #region AccountHeadID
                oDataCollectionSetups = new List<DataCollectionSetup>();
                oDataCollectionSetups = this.GetDataCollectionSetups(EnumDataSetupType.AccountHeadSetup, oDebitCreditSetup.DataCollectionSetups);
                sTempSQL = this.GetDataCollectSQL(oDataCollectionSetups, oDataRowDebitCredit);
                sSQL = sSQL + " (" + sTempSQL + ") AS AccountHeadID,";
                sAdvSQL = sTempSQL;
                #endregion

                #region AccountHead Code
                sTempSQL = "SELECT AccountCode FROM ChartsOfAccount WHERE AccountHeadID=(" + sAdvSQL + ")";
                sSQL = sSQL + " (" + sTempSQL + ") AS AccountCode,";
                #endregion

                #region AccountHead Name
                sTempSQL = "SELECT AccountHeadName FROM ChartsOfAccount WHERE AccountHeadID=(" + sAdvSQL + ")";
                sSQL = sSQL + " (" + sTempSQL + ") AS AccountHeadName,";
                #endregion
            }
            else
            {
                #region AccountHeadID
                oDataCollectionSetups = new List<DataCollectionSetup>();
                oDataCollectionSetups = this.GetDataCollectionSetups(EnumDataSetupType.AccountHeadSetup, oDebitCreditSetup.DataCollectionSetups);
                sTempSQL = this.GetDataCollectSQL(oDataCollectionSetups, oDataRowDebitCredit);
                sTempSQL = "SELECT AccountHeadID FROM ChartsOfAccount WHERE ReferenceObjectID=(" + sTempSQL + ") AND ReferenceType=" + oDebitCreditSetup.ReferenceTypeInInt.ToString();
                sSQL = sSQL + " (" + sTempSQL + ") AS AccountHeadID,";
                sAdvSQL = sTempSQL;
                #endregion

                #region AccountHead Code
                sTempSQL = "SELECT AccountCode FROM ChartsOfAccount WHERE AccountHeadID=(" + sAdvSQL + ")";
                sSQL = sSQL + " (" + sTempSQL + ") AS AccountCode,";
                #endregion

                #region AccountHead Name
                sTempSQL = "SELECT AccountHeadName FROM ChartsOfAccount WHERE AccountHeadID=(" + sAdvSQL + ")";
                sSQL = sSQL + " (" + sTempSQL + ") AS AccountHeadName,";
                #endregion
            }

            #endregion

            #region VoucherDetail CurrencyId
            oDataCollectionSetups = new List<DataCollectionSetup>();
            oDataCollectionSetups = this.GetDataCollectionSetups(EnumDataSetupType.CurrencySetup, oDebitCreditSetup.DataCollectionSetups);
            sTempSQL = this.GetDataCollectSQL(oDataCollectionSetups, oDataRowDebitCredit);
            sSQL = sSQL + " (" + sTempSQL + ") AS CurrencyId,";
            sAdvSQL = sTempSQL;
            #endregion

            #region Currency Name
            sTempSQL = "SELECT CurrencyName FROM Currency WHERE CurrencyID=(" + sAdvSQL + ")";
            sSQL = sSQL + " (" + sTempSQL + ") AS CurrencyName,";
            #endregion

            #region Currency Symbol
            sTempSQL = "SELECT Symbol FROM Currency WHERE CurrencyID=(" + sAdvSQL + ")";
            sSQL = sSQL + " (" + sTempSQL + ") AS CurrencySymbol,";
            #endregion

            #region VoucherDetail ConversionRate
            oDataCollectionSetups = new List<DataCollectionSetup>();
            oDataCollectionSetups = this.GetDataCollectionSetups(EnumDataSetupType.ConversionRateSetup, oDebitCreditSetup.DataCollectionSetups);
            sTempSQL = this.GetDataCollectSQL(oDataCollectionSetups, oDataRowDebitCredit);
            sSQL = sSQL + " (" + sTempSQL + ") AS ConversionRate,";
            #endregion

            #region Voucher Detail AmountInCurrency
            oDataCollectionSetups = new List<DataCollectionSetup>();
            oDataCollectionSetups = this.GetDataCollectionSetups(EnumDataSetupType.VoucherDetailAmountSetup, oDebitCreditSetup.DataCollectionSetups);
            sTempSQL = this.GetDataCollectSQL(oDataCollectionSetups, oDataRowDebitCredit);
            sSQL = sSQL + " (" + sTempSQL + ") AS AmountInCurrency,";
            #endregion

            #region Voucher Detail Narration
            oDataCollectionSetups = new List<DataCollectionSetup>();
            oDataCollectionSetups = this.GetDataCollectionSetups(EnumDataSetupType.VoucherDetailNarrationSetup, oDebitCreditSetup.DataCollectionSetups);
            sTempSQL = this.GetDataCollectSQL(oDataCollectionSetups, oDataRowDebitCredit);
            sSQL = sSQL + " (" + sTempSQL + ") AS Narration";
            #endregion

            #region Account head Name (If COA not found then it Suggest user For Configure Dynamic COA Head )
            oDataCollectionSetups = new List<DataCollectionSetup>();
            oDataCollectionSetups = this.GetDataCollectionSetups(EnumDataSetupType.AccountNameSetup, oDebitCreditSetup.DataCollectionSetups);
            sTempSQL = this.GetDataCollectSQL(oDataCollectionSetups, oDataRowDebitCredit);
            if (sTempSQL.Length > 0)
            {
                sSQL = sSQL + ",(" + sTempSQL + ") AS NarrationHeadName";
            }
            #endregion

            #region SQL Execute
            IDataReader reader;
            sSQL = "SELECT " + sSQL;
            reader = VoucherDA.Gets(tc, sSQL);
            NullHandler oDataReader = new NullHandler(reader);
            if (reader.Read())
            {
                oVoucherDetail = new VoucherDetail();
                oVoucherDetail.AccountHeadID = oDataReader.GetInt32("AccountHeadID");
                oVoucherDetail.AccountHeadCode = oDataReader.GetString("AccountCode");
                oVoucherDetail.AccountHeadName = oDataReader.GetString("AccountHeadName");
                oVoucherDetail.CurrencyID = oDataReader.GetInt32("CurrencyId");
                oVoucherDetail.CUName = oDataReader.GetString("CurrencyName");
                oVoucherDetail.CUSymbol = oDataReader.GetString("CurrencySymbol");
                oVoucherDetail.ConversionRate = oDataReader.GetDouble("ConversionRate");
                oVoucherDetail.AmountInCurrency = oDataReader.GetDoubleRound("AmountInCurrency");
                oVoucherDetail.Narration = oDataReader.GetString("Narration");
                oVoucherDetail.Narration = oVoucherDetail.Narration.Trim();
                if (oVoucherDetail.Narration == "." || oVoucherDetail.Narration == "N/A") { oVoucherDetail.Narration = ""; }
                if (oVoucherDetail.AccountHeadID <= 0)
                {
                    oVoucherDetail.Narration = oDataReader.GetString("NarrationHeadName");
                }
            }
            reader.Close();
            #endregion

            if (oVoucherDetail.AccountHeadID <= 0)
            {
                throw new Exception("Account Head Not Found! For " + oVoucherDetail.Narration + " with "+oVoucher.ReferenceNote);
            }

            #region Check BU, Area, Zone, Site, Dept, Product Existance
            //AreaID, AreaCode, AreaName, ZoneID, ZoneCode, ZoneName, SiteID, SiteCode, SiteName, DeptID, DeptCode, DeptName, ProductID, ProductCode, ProductName           
            if (oRow.Table.Columns.Contains("AreaID"))
            {
                if (!oRow.Table.Columns.Contains("AreaCode"))
                {
                    throw new Exception("Area Code Not Contain in your selected Operation for " + oVoucher.ReferenceNote + "!");
                }
                if (!oRow.Table.Columns.Contains("AreaName"))
                {
                    throw new Exception("Area Name Not Contain in your selected Operation for " + oVoucher.ReferenceNote + "!");
                }
            }

            if (oRow.Table.Columns.Contains("ZoneID"))
            {
                if (!oRow.Table.Columns.Contains("ZoneCode"))
                {
                    throw new Exception("Zone Code Not Contain in your selected Operation for " + oVoucher.ReferenceNote + "!");
                }
                if (!oRow.Table.Columns.Contains("ZoneName"))
                {
                    throw new Exception("Zone Name Not Contain in your selected Operation for " + oVoucher.ReferenceNote + "!");
                }
            }

            if (oRow.Table.Columns.Contains("SiteID"))
            {
                if (!oRow.Table.Columns.Contains("SiteCode"))
                {
                    throw new Exception("Site Code Not Contain in your selected Operation for " + oVoucher.ReferenceNote + "!");
                }
                if (!oRow.Table.Columns.Contains("SiteName"))
                {
                    throw new Exception("Site Name Not Contain in your selected Operation for " + oVoucher.ReferenceNote + "!");
                }
            }

            if (oRow.Table.Columns.Contains("DeptID"))
            {
                if (!oRow.Table.Columns.Contains("DeptCode"))
                {
                    throw new Exception("Dept Code Not Contain in your selected Operation for " + oVoucher.ReferenceNote + "!");
                }
                if (!oRow.Table.Columns.Contains("DeptName"))
                {
                    throw new Exception("Dept Name Not Contain in your selected Operation for " + oVoucher.ReferenceNote + "!");
                }
            }

            if (oRow.Table.Columns.Contains("ProductID"))
            {
                if (!oRow.Table.Columns.Contains("ProductCode"))
                {
                    throw new Exception("Product Code Not Contain in your selected Operation for " + oVoucher.ReferenceNote + "!");
                }
                if (!oRow.Table.Columns.Contains("ProductName"))
                {
                    throw new Exception("Product Name Not Contain in your selected Operation for " + oVoucher.ReferenceNote + "!");
                }
            }

            #endregion

            #region Map BU, Area, Zone, Site, Dept, Product Existance            
            if (oRow.Table.Columns.Contains("AreaID"))
            {
                oVoucherDetail.AreaID = (oRow["AreaID"] != DBNull.Value) ? Convert.ToInt32(oRow["AreaID"]) : 0;
                oVoucherDetail.AreaCode = (oRow["AreaCode"] != DBNull.Value) ? oRow["AreaCode"].ToString() : "00";
                oVoucherDetail.AreaName = (oRow["AreaName"] != DBNull.Value) ? oRow["AreaName"].ToString() : "";
            }

            if (oRow.Table.Columns.Contains("ZoneID"))
            {
                oVoucherDetail.ZoneID = (oRow["ZoneID"] != DBNull.Value) ? Convert.ToInt32(oRow["ZoneID"]) : 0;
                oVoucherDetail.ZoneCode = (oRow["ZoneCode"] != DBNull.Value) ? oRow["ZoneCode"].ToString() : "00";
                oVoucherDetail.ZoneName = (oRow["ZoneName"] != DBNull.Value) ? oRow["ZoneName"].ToString() : "";
            }

            if (oRow.Table.Columns.Contains("SiteID"))
            {
                oVoucherDetail.SiteID = (oRow["SiteID"] != DBNull.Value) ? Convert.ToInt32(oRow["SiteID"]) : 0;
                oVoucherDetail.SiteCode = (oRow["SiteCode"] != DBNull.Value) ? oRow["SiteCode"].ToString() : "0000";
                oVoucherDetail.SiteName = (oRow["SiteName"] != DBNull.Value) ? oRow["SiteName"].ToString() : "";
            }

            if (oRow.Table.Columns.Contains("DeptID"))
            {
                oVoucherDetail.DeptID = (oRow["DeptID"] != DBNull.Value) ? Convert.ToInt32(oRow["DeptID"]) : 0;
                oVoucherDetail.DeptCode = (oRow["DeptCode"] != DBNull.Value) ? oRow["DeptCode"].ToString() : "00";
                oVoucherDetail.DeptName = (oRow["DeptName"] != DBNull.Value) ? oRow["DeptName"].ToString() : "";
            }

            if (oRow.Table.Columns.Contains("ProductID"))
            {
                oVoucherDetail.ProductID = (oRow["ProductID"] != DBNull.Value) ? Convert.ToInt32(oRow["ProductID"]) : 0;
                oVoucherDetail.PCode = (oRow["ProductCode"] != DBNull.Value) ? oRow["ProductCode"].ToString() : "00000";
                oVoucherDetail.PName = (oRow["ProductName"] != DBNull.Value) ? oRow["ProductName"].ToString() : "";
            }
            #endregion

            return oVoucherDetail;
        }
        private CostCenterTransaction GetCostCenterTransactionData(TransactionContext tc, DataRow oRow, DebitCreditSetup oDebitCreditSetup, long nUserId, DataRow oRowDebitCredit, Voucher oVoucher, VoucherDetail oVoucherDetail)
        {
            string sSQL = ""; string sTempSQL = ""; string sAdvSQL = ""; int nCCID = 0; int nCostCenterCategoryID = 0; string sCCSQL = "";
            CostCenterTransaction oCostCenterTransaction = new CostCenterTransaction();
            List<DataCollectionSetup> oDataCollectionSetups = new List<DataCollectionSetup>();

            #region Cost Center CCID
            oDataCollectionSetups = new List<DataCollectionSetup>();
            oDataCollectionSetups = this.GetDataCollectionSetups(EnumDataSetupType.CostCenterSetup, oDebitCreditSetup.DataCollectionSetups);
            sTempSQL = this.GetDataCollectSQL(oDataCollectionSetups, oRow);
            sCCSQL = sCCSQL + "(" + sTempSQL + ") AS CCID,";

            #region Cost Center Category Setup
            oDataCollectionSetups = new List<DataCollectionSetup>();
            oDataCollectionSetups = this.GetDataCollectionSetups(EnumDataSetupType.CostCenterCategorySetup, oDebitCreditSetup.DataCollectionSetups);
            string sCCCategorySQL = this.GetDataCollectSQL(oDataCollectionSetups, oRow);
            sCCSQL = sCCSQL + " (" + sCCCategorySQL + ") AS CategoryID";
            #endregion


            IDataReader ccreader;
            sCCSQL = "Select " + sCCSQL;
            ccreader = VoucherDA.Gets(tc, sCCSQL);
            NullHandler oCCDataReader = new NullHandler(ccreader);
            if (ccreader.Read())
            {
                nCCID = oCCDataReader.GetInt32("CCID");
                nCostCenterCategoryID = oCCDataReader.GetInt32("CategoryID");
            }
            ccreader.Close();
            if (nCCID <= 0)
            {
                #region Insert Cost Center
                if (nCostCenterCategoryID <= 0)
                {
                    throw new Exception("Invalid Cost Center Category for " + oVoucher.ReferenceNote + "!");
                }
                ACCostCenter oACCostCenter = new ACCostCenter();
                oACCostCenter.ACCostCenterID = 0;
                oACCostCenter.Code = "";
                oACCostCenter.Name = Convert.ToString(oRow[oDebitCreditSetup.CostCenterNoColumn]);
                oACCostCenter.Description = "Auto Creat from Voucher";
                oACCostCenter.ParentID = nCostCenterCategoryID;
                oACCostCenter.ReferenceType = oDebitCreditSetup.CostCenterRefObjType;
                oACCostCenter.ReferenceTypeInt = (int)oDebitCreditSetup.CostCenterRefObjType;
                oACCostCenter.ReferenceObjectID = Convert.ToInt32(oRow[oDebitCreditSetup.CostCenterRefObjColumn]);
                oACCostCenter.ActivationDate = DateTime.Today;
                oACCostCenter.ExpireDate = DateTime.Today;
                oACCostCenter.IsActive = true;

                if (oACCostCenter.Name == null || oACCostCenter.Name == "")
                {
                    throw new Exception("Invalid Cost Center Name for " + oVoucher.ReferenceNote + "!");
                }
                if (oACCostCenter.ParentID <= 0)
                {
                    throw new Exception("Invalid Cost Center Category for " + oVoucher.ReferenceNote + "!");
                }
                if (oACCostCenter.ReferenceType == EnumReferenceType.None)
                {
                    throw new Exception("Invalid Cost Center Reference Type for " + oVoucher.ReferenceNote + "!");
                }
                if (oACCostCenter.ReferenceObjectID <= 0)
                {
                    throw new Exception("Invalid Cost Center Reference Object for " + oVoucher.ReferenceNote + "!");
                }

                ccreader = null;
                ccreader = ACCostCenterDA.InsertUpdate(tc, oACCostCenter, EnumDBOperation.Insert, nUserId);
                oCCDataReader = new NullHandler(ccreader);
                if (ccreader.Read())
                {
                    nCCID = oCCDataReader.GetInt32("ACCostCenterID");
                }
                ccreader.Close();
                if (nCCID <= 0)
                {
                    throw new Exception("Invalid Cost Center for " + oVoucher.ReferenceNote + "!");
                }

                #region Subledger Wise Business Unit
                string sBusinessUnitIDs = oVoucherDetail.BUID.ToString();
                BUWiseSubLedger oBUWiseSubLedger = new BUWiseSubLedger();
                oBUWiseSubLedger.SubLedgerID = nCCID;
                BUWiseSubLedgerDA.IUDFromCC(tc, oBUWiseSubLedger, sBusinessUnitIDs, nUserId);
                #endregion

                #endregion
            }
            sSQL = sSQL + " (" + nCCID.ToString() + ") AS CCID,";
            sAdvSQL = sTempSQL;
            #endregion

            #region CCName
            sTempSQL = "SELECT Name FROM ACCostCenter WHERE ACCostCenterID=(" + sAdvSQL + ")";
            sSQL = sSQL + " (" + sTempSQL + ") AS CCName,";
            #endregion

            #region IsBillRefApply
            sTempSQL = "SELECT HH.IsBillRefApply FROM SubledgerRefConfig AS HH WHERE HH.SubledgerID=(" + sAdvSQL + ") AND HH.AccountHeadID=" + oVoucherDetail.AccountHeadID.ToString();
            sSQL = sSQL + " (" + sTempSQL + ") AS IsBillRefApply,";
            #endregion

            #region IsOrderRefApply
            sTempSQL = "SELECT HH.IsOrderRefApply FROM SubledgerRefConfig AS HH WHERE HH.SubledgerID=(" + sAdvSQL + ") AND HH.AccountHeadID=" + oVoucherDetail.AccountHeadID.ToString();
            sSQL = sSQL + " (" + sTempSQL + ") AS IsOrderRefApply,";
            #endregion

            #region IsChequeApply
            sTempSQL = "SELECT 0 AS IsChequeApply FROM SubledgerRefConfig AS HH WHERE HH.SubledgerID=(" + sAdvSQL + ") AND HH.AccountHeadID=" + oVoucherDetail.AccountHeadID.ToString();
            sSQL = sSQL + " (" + sTempSQL + ") AS IsChequeApply,";
            #endregion

            #region Cost Center Description
            oDataCollectionSetups = new List<DataCollectionSetup>();
            oDataCollectionSetups = this.GetDataCollectionSetups(EnumDataSetupType.CostCenterDescriptionSetup, oDebitCreditSetup.DataCollectionSetups);
            sTempSQL = this.GetDataCollectSQL(oDataCollectionSetups, oRow);
            sSQL = sSQL + " (" + sTempSQL + ") AS Description,";
            #endregion

            #region Cost Center TransactionDate
            oDataCollectionSetups = new List<DataCollectionSetup>();
            oDataCollectionSetups = this.GetDataCollectionSetups(EnumDataSetupType.CostCenterDateSetup, oDebitCreditSetup.DataCollectionSetups);
            sTempSQL = this.GetDataCollectSQL(oDataCollectionSetups, oRow);
            sSQL = sSQL + " (" + sTempSQL + ") AS TransactionDate,";
            #endregion

            #region Cost Center Amount
            oDataCollectionSetups = new List<DataCollectionSetup>();
            oDataCollectionSetups = this.GetDataCollectionSetups(EnumDataSetupType.CostCenterAmountSetup, oDebitCreditSetup.DataCollectionSetups);
            sTempSQL = this.GetDataCollectSQL(oDataCollectionSetups, oRow);
            sSQL = sSQL + " (" + sTempSQL + ") AS Amount";
            #endregion

            #region SQL Execute
            IDataReader reader;
            sSQL = "SELECT " + sSQL;
            reader = VoucherDA.Gets(tc, sSQL);
            NullHandler oDataReader = new NullHandler(reader);
            if (reader.Read())
            {
                oCostCenterTransaction = new CostCenterTransaction();
                oCostCenterTransaction.CCID = oDataReader.GetInt32("CCID");
                oCostCenterTransaction.CostCenterName = oDataReader.GetString("CCName");
                oCostCenterTransaction.IsBillRefApply = oDataReader.GetBoolean("IsBillRefApply");
                oCostCenterTransaction.IsOrderRefApply = oDataReader.GetBoolean("IsOrderRefApply");
                oCostCenterTransaction.IsChequeApply = oDataReader.GetBoolean("IsChequeApply");
                oCostCenterTransaction.Amount = oDataReader.GetDoubleRound("Amount");
                oCostCenterTransaction.Description = oDataReader.GetString("Description");
                oCostCenterTransaction.TransactionDate = oDataReader.GetDateTime("TransactionDate");
                oCostCenterTransaction.Description = oCostCenterTransaction.Description.Trim();
                if (oCostCenterTransaction.Description == "." || oCostCenterTransaction.Description == "N/A") { oCostCenterTransaction.Description = ""; }
            }
            reader.Close();
            #endregion

            #region Sub Ledger Bill
            if (oDebitCreditSetup.HasBillReference)
            {
                oCostCenterTransaction.VBTransactions = GetVoucherBillTransactions(tc, oRow, oDebitCreditSetup, oVoucher, oVoucherDetail, nUserId, oCostCenterTransaction.CCID);
            }
            #endregion

            #region Sub Ledger Order Ref
            if (oDebitCreditSetup.HasOrderReference)
            {
                oCostCenterTransaction.VOReferences = GetVoucherOrders(tc, oVoucher, oRow, oDebitCreditSetup, oVoucherDetail, nUserId, oCostCenterTransaction.CCID, oVoucher.BUID);
            }
            #endregion

            #region Sub Ledger Cheque
            if (oDebitCreditSetup.HasChequeReference)
            {
                oCostCenterTransaction.VoucherCheques = GetVoucherCheques(tc, oRow, oDebitCreditSetup, oVoucherDetail);
            }
            #endregion

            return oCostCenterTransaction;
        }
        private List<CostCenterTransaction> GetCostCenterTransactions(TransactionContext tc, DataRow oDataRow, DebitCreditSetup oDebitCreditSetup, Voucher oVoucher, VoucherDetail oVoucherDetail, long nUserId)
        {
            #region Declaration
            CostCenterTransaction oCostCenterTransaction = new CostCenterTransaction();
            CostCenterTransaction oTempCostCenterTransaction = new CostCenterTransaction();
            List<CostCenterTransaction> oCostCenterTransactions = new List<CostCenterTransaction>();
            List<DataCollectionSetup> oDataCollectionSetups = new List<DataCollectionSetup>();
            DataCollectionSetup oDataCollectionSetup = new DataCollectionSetup();
            string sSQL = "";
            #endregion

            #region Voucher Reference Data SQL
            oDataCollectionSetup.DataGenerateType = EnumDataGenerateType.AutomatedData;
            oDataCollectionSetup.QueryForValue = oDebitCreditSetup.CostcenterDataSQL;
            oDataCollectionSetup.ReferenceValueFields = oDebitCreditSetup.CostCenterCompareColumns;
            oDataCollectionSetups.Add(oDataCollectionSetup);
            sSQL = this.GetDataCollectSQL(oDataCollectionSetups, oDataRow);
            #endregion

            #region SQL Execute
            DataSet oDataSet = new DataSet();
            DataTable oDataTable = new DataTable();
            IDataReader reader = VoucherDA.Gets(tc, sSQL);
            oDataSet.Load(reader, LoadOption.OverwriteChanges, new string[1]);
            oDataTable = oDataSet.Tables[0];
            reader.Close();

            foreach (DataRow oRow in oDataTable.Rows)
            {
                oCostCenterTransaction = new CostCenterTransaction();
                oTempCostCenterTransaction = new CostCenterTransaction();
                oTempCostCenterTransaction = this.GetCostCenterTransactionData(tc, oRow, oDebitCreditSetup, nUserId, oDataRow, oVoucher, oVoucherDetail);
                oCostCenterTransaction.AccountHeadID = oVoucherDetail.AccountHeadID;
                oCostCenterTransaction.AccountHeadName = oVoucherDetail.AccountHeadName;
                oCostCenterTransaction.CurrencyID = oVoucherDetail.CurrencyID;
                oCostCenterTransaction.CurrencySymbol = oVoucherDetail.CUSymbol;
                oCostCenterTransaction.CurrencyConversionRate = oVoucherDetail.ConversionRate;
                oCostCenterTransaction.CCID = oTempCostCenterTransaction.CCID;
                oCostCenterTransaction.CostCenterName = oTempCostCenterTransaction.CostCenterName;
                oCostCenterTransaction.Amount = oTempCostCenterTransaction.Amount;
                oCostCenterTransaction.Description = oTempCostCenterTransaction.Description;
                oCostCenterTransaction.TransactionDate = oTempCostCenterTransaction.TransactionDate;
                oCostCenterTransaction.IsBillRefApply = oTempCostCenterTransaction.IsBillRefApply;
                oCostCenterTransaction.VBTransactions = oTempCostCenterTransaction.VBTransactions;
                oCostCenterTransaction.IsOrderRefApply = oTempCostCenterTransaction.IsOrderRefApply;
                oCostCenterTransaction.VOReferences = oTempCostCenterTransaction.VOReferences;
                oCostCenterTransaction.IsChequeApply = oTempCostCenterTransaction.IsChequeApply;
                oCostCenterTransaction.VoucherCheques = oTempCostCenterTransaction.VoucherCheques;
                oCostCenterTransactions.Add(oCostCenterTransaction);
            }
            #endregion

            return oCostCenterTransactions;
        }
        private VoucherBillTransaction GetVoucherBillTransactionData(TransactionContext tc, DataRow oRow, DebitCreditSetup oDebitCreditSetup, VoucherDetail oVoucherDetail, long nUserId, int nSubLedgerID)
        {
            string sSQL = ""; string sTempSQL = ""; string sAdvSQL = ""; int nVoucherBillID = 0; double nBillAmount = 0;
            VoucherBillTransaction oVoucherBillTransaction = new VoucherBillTransaction();
            List<DataCollectionSetup> oDataCollectionSetups = new List<DataCollectionSetup>();

            #region VoucherBillID
            oDataCollectionSetups = new List<DataCollectionSetup>();
            oDataCollectionSetups = this.GetDataCollectionSetups(EnumDataSetupType.VoucherBillSetup, oDebitCreditSetup.DataCollectionSetups);
            sTempSQL = this.GetDataCollectSQL(oDataCollectionSetups, oRow);

            string sCCSQL = "SELECT (" + sTempSQL + ") AS VoucherBillID";
            IDataReader vbreader;
            vbreader = VoucherDA.Gets(tc, sCCSQL);
            NullHandler oVBDataReader = new NullHandler(vbreader);
            if (vbreader.Read())
            {
                nVoucherBillID = oVBDataReader.GetInt32("VoucherBillID");
            }
            vbreader.Close();
            if (nVoucherBillID <= 0)
            {
                #region Voucher Bill Amount, Bill Date & Bill Due Date
                string sBillSQL = ""; DateTime dBillDate = DateTime.Now; DateTime dBillDueDate = DateTime.Now;
                #region Amount
                oDataCollectionSetups = new List<DataCollectionSetup>();
                oDataCollectionSetups = this.GetDataCollectionSetups(EnumDataSetupType.VoucherBillAmountSetup, oDebitCreditSetup.DataCollectionSetups);
                sTempSQL = this.GetDataCollectSQL(oDataCollectionSetups, oRow);
                sBillSQL = "(" + sTempSQL + ") AS Amount,";
                #endregion

                #region Bill Date
                oDataCollectionSetups = new List<DataCollectionSetup>();
                oDataCollectionSetups = this.GetDataCollectionSetups(EnumDataSetupType.BillDateSetup, oDebitCreditSetup.DataCollectionSetups);
                sTempSQL = this.GetDataCollectSQL(oDataCollectionSetups, oRow);
                sBillSQL = sBillSQL + "(" + sTempSQL + ") AS BillDate,";
                #endregion

                #region Bill Due Date
                oDataCollectionSetups = new List<DataCollectionSetup>();
                oDataCollectionSetups = this.GetDataCollectionSetups(EnumDataSetupType.BillDueDateSetup, oDebitCreditSetup.DataCollectionSetups);
                sTempSQL = this.GetDataCollectSQL(oDataCollectionSetups, oRow);
                sBillSQL = sBillSQL + "(" + sTempSQL + ") AS BillDueDate";
                #endregion


                vbreader = null;
                sBillSQL = "Select " + sBillSQL;
                vbreader = VoucherDA.Gets(tc, sBillSQL);
                oVBDataReader = new NullHandler(vbreader);
                if (vbreader.Read())
                {
                    nBillAmount = oVBDataReader.GetDoubleRound("Amount");
                    dBillDate = oVBDataReader.GetDateTime("BillDate");
                    dBillDueDate = oVBDataReader.GetDateTime("BillDueDate");
                }
                vbreader.Close();
                #endregion

                #region Insert VoucherBill
                VoucherBill oVoucherBill = new VoucherBill();
                oVoucherBill.VoucherBillID = 0;
                oVoucherBill.BillNo = Convert.ToString(oRow[oDebitCreditSetup.VoucherBillNoColumn]);
                oVoucherBill.AccountHeadID = oVoucherDetail.AccountHeadID;
                oVoucherBill.SubLedgerID = nSubLedgerID;
                oVoucherBill.IsDebit = oVoucherBill.IsDebit;
                oVoucherBill.CreditDays = 0;
                oVoucherBill.BillDate = dBillDate;
                oVoucherBill.DueDate = dBillDueDate;
                oVoucherBill.TrType = oDebitCreditSetup.VoucherBillTrType;
                oVoucherBill.TrTypeInInt = oDebitCreditSetup.VoucherBillTrTypeInInt;
                oVoucherBill.CurrencyID = oVoucherDetail.CurrencyID;
                oVoucherBill.CurrencyRate = oVoucherDetail.ConversionRate;
                oVoucherBill.CurrencyAmount = nBillAmount;
                oVoucherBill.Amount = oVoucherBill.CurrencyAmount * oVoucherDetail.ConversionRate;
                oVoucherBill.ReferenceType = oDebitCreditSetup.VoucherBillRefObjType;
                oVoucherBill.ReferenceTypeInInt = oDebitCreditSetup.VoucherBillRefObjTypeInInt;
                oVoucherBill.ReferenceObjID = Convert.ToInt32(oRow[oDebitCreditSetup.VoucherBillRefObjColumn]);
                oVoucherBill.BUID = oVoucherDetail.BUID;

                if (oVoucherBill.BillNo == null || oVoucherBill.BillNo == "")
                {
                    throw new Exception("Invalid Voucher Bill No!");
                }
                if (oVoucherBill.CurrencyAmount <= 0)
                {
                    throw new Exception("Invalid Voucher Bill Amount!");
                }
                if (oVoucherBill.ReferenceType == EnumVoucherBillReferenceType.None)
                {
                    throw new Exception("Invalid Voucher Bill Reference Type!");
                }
                if (oVoucherBill.ReferenceObjID <= 0)
                {
                    throw new Exception("Invalid Voucher Bill Reference Object!");
                }
                if (oVoucherBill.CurrencyID <= 0)
                {
                    throw new Exception("Invalid Voucher Bill Currency!");
                }

                vbreader = null;
                vbreader = VoucherBillDA.InsertUpdate(tc, oVoucherBill, EnumDBOperation.Insert, nUserId);
                oVBDataReader = new NullHandler(vbreader);
                if (vbreader.Read())
                {
                    nVoucherBillID = oVBDataReader.GetInt32("VoucherBillID");
                }
                vbreader.Close();
                if (nVoucherBillID <= 0)
                {
                    throw new Exception("Invalid Cost VoucherBill!");
                }
                #endregion
            }

            sSQL = sSQL + " (" + nVoucherBillID.ToString() + ") AS VoucherBillID,";
            sAdvSQL = sTempSQL;
            #endregion

            #region Name
            sTempSQL = "SELECT BillNo FROM VoucherBill WHERE VoucherBillID=(" + nVoucherBillID.ToString() + ")";
            sSQL = sSQL + " (" + sTempSQL + ") AS BillNo,";
            #endregion

            #region VoucherBillDescription
            //oDataCollectionSetups = new List<DataCollectionSetup>();
            //oDataCollectionSetups = this.GetDataCollectionSetups(EnumDataSetupType.VoucherBillDescriptionSetup, oDebitCreditSetup.DataCollectionSetups);
            //sTempSQL = this.GetDataCollectSQL(oDataCollectionSetups, oRow);
            //sSQL = sSQL + " (" + sTempSQL + ") AS Description,";
            #endregion

            #region Voucher Bill TransactionDate
            oDataCollectionSetups = new List<DataCollectionSetup>();
            oDataCollectionSetups = this.GetDataCollectionSetups(EnumDataSetupType.VoucherBillDateSetup, oDebitCreditSetup.DataCollectionSetups);
            sTempSQL = this.GetDataCollectSQL(oDataCollectionSetups, oRow);
            sSQL = sSQL + " (" + sTempSQL + ") AS TransactionDate,";
            #endregion

            #region Voucher Bill Amount
            oDataCollectionSetups = new List<DataCollectionSetup>();
            oDataCollectionSetups = this.GetDataCollectionSetups(EnumDataSetupType.VoucherBillAmountSetup, oDebitCreditSetup.DataCollectionSetups);
            sTempSQL = this.GetDataCollectSQL(oDataCollectionSetups, oRow);
            sSQL = sSQL + " (" + sTempSQL + ") AS Amount";
            #endregion

            #region SQL Execute
            IDataReader reader;
            sSQL = "SELECT " + sSQL;
            reader = VoucherDA.Gets(tc, sSQL);
            NullHandler oDataReader = new NullHandler(reader);
            if (reader.Read())
            {
                oVoucherBillTransaction = new VoucherBillTransaction();
                oVoucherBillTransaction.VoucherBillID = oDataReader.GetInt32("VoucherBillID");
                oVoucherBillTransaction.BillNo = oDataReader.GetString("BillNo");
                oVoucherBillTransaction.Amount = oDataReader.GetDoubleRound("Amount");
                oVoucherBillTransaction.TransactionDate = oDataReader.GetDateTime("TransactionDate");
                oVoucherBillTransaction.CCID = nSubLedgerID;
            }
            reader.Close();
            #endregion

            return oVoucherBillTransaction;
        }
        private List<VoucherBillTransaction> GetVoucherBillTransactions(TransactionContext tc, DataRow oDataRow, DebitCreditSetup oDebitCreditSetup, Voucher oVoucher, VoucherDetail oVoucherDetail, long nUserId, int nSubLedgerID)
        {
            #region Declaration
            VoucherBillTransaction oVoucherBillTransaction = new VoucherBillTransaction();
            VoucherBillTransaction oTempVoucherBillTransaction = new VoucherBillTransaction();
            List<VoucherBillTransaction> oVoucherBillTransactions = new List<VoucherBillTransaction>();
            List<DataCollectionSetup> oDataCollectionSetups = new List<DataCollectionSetup>();
            DataCollectionSetup oDataCollectionSetup = new DataCollectionSetup();
            string sSQL = "";
            #endregion

            #region Voucher Reference Data SQL
            oDataCollectionSetup.DataGenerateType = EnumDataGenerateType.AutomatedData;
            oDataCollectionSetup.QueryForValue = oDebitCreditSetup.VoucherBillDataSQL;
            oDataCollectionSetup.ReferenceValueFields = oDebitCreditSetup.VoucherBillCompareColumns;
            oDataCollectionSetups.Add(oDataCollectionSetup);
            sSQL = this.GetDataCollectSQL(oDataCollectionSetups, oDataRow);
            #endregion

            #region SQL Execute
            DataSet oDataSet = new DataSet();
            DataTable oDataTable = new DataTable();
            IDataReader reader = VoucherDA.Gets(tc, sSQL);
            oDataSet.Load(reader, LoadOption.OverwriteChanges, new string[1]);
            oDataTable = oDataSet.Tables[0];
            reader.Close();

            foreach (DataRow oRow in oDataTable.Rows)
            {
                oVoucherBillTransaction = new VoucherBillTransaction();
                oTempVoucherBillTransaction = new VoucherBillTransaction();
                oTempVoucherBillTransaction = this.GetVoucherBillTransactionData(tc, oRow, oDebitCreditSetup, oVoucherDetail, nUserId, nSubLedgerID);
                oVoucherBillTransaction.CurrencyID = oVoucherDetail.CurrencyID;
                oVoucherBillTransaction.CurrencySymbol = oVoucherDetail.CUSymbol;
                oVoucherBillTransaction.ConversionRate = oVoucherDetail.ConversionRate;
                oVoucherBillTransaction.VoucherBillID = oTempVoucherBillTransaction.VoucherBillID;
                oVoucherBillTransaction.BillNo = oTempVoucherBillTransaction.BillNo;
                oVoucherBillTransaction.TrType = oDebitCreditSetup.VoucherBillTrType;
                oVoucherBillTransaction.Amount = oTempVoucherBillTransaction.Amount;
                oVoucherBillTransaction.TransactionDate = oTempVoucherBillTransaction.TransactionDate;
                oVoucherBillTransaction.CCID = oTempVoucherBillTransaction.CCID;
                oVoucherBillTransactions.Add(oVoucherBillTransaction);
            }
            #endregion

            return oVoucherBillTransactions;
        }
        private VoucherCheque GetVocuherChequeData(TransactionContext tc, DataRow oDataRow, DebitCreditSetup oDebitCreditSetup)
        {
            string sSQL = ""; string sTempSQL = ""; string sAdvSQL = "";
            VoucherCheque oVoucherCheque = new VoucherCheque();
            List<DataCollectionSetup> oDataCollectionSetups = new List<DataCollectionSetup>();

            #region ChequeID
            oDataCollectionSetups = new List<DataCollectionSetup>();
            oDataCollectionSetups = this.GetDataCollectionSetups(EnumDataSetupType.ChequeSetup, oDebitCreditSetup.DataCollectionSetups);
            sTempSQL = this.GetDataCollectSQL(oDataCollectionSetups, oDataRow);
            sSQL = sSQL + " (" + sTempSQL + ") AS ChequeID,";
            sAdvSQL = sTempSQL;
            #endregion

            #region ChequeNo
            if (oDebitCreditSetup.ChequeType == EnumChequeType.Payment)
            {
                sTempSQL = "SELECT ChequeNo FROM Cheque WHERE ChequeID=(" + sAdvSQL + ")";
                sSQL = sSQL + " (" + sTempSQL + ") AS ChequeNo,";
            }
            else if (oDebitCreditSetup.ChequeType == EnumChequeType.Received)
            {
                sTempSQL = "SELECT ChequeNo FROM ReceivedCheque WHERE ReceivedChequeID=(" + sAdvSQL + ")";
                sSQL = sSQL + " (" + sTempSQL + ") AS ChequeNo,";
            }
            else
            {
                sTempSQL = "SELECT '' AS ChequeNo";
                sSQL = sSQL + " (" + sTempSQL + ") AS ChequeNo,";
            }
            #endregion

            #region ChequeDate
            if (oDebitCreditSetup.ChequeType == EnumChequeType.Payment)
            {
                sTempSQL = "SELECT ChequeDate FROM Cheque WHERE ChequeID=(" + sAdvSQL + ")";
                sSQL = sSQL + " (" + sTempSQL + ") AS ChequeDate,";
            }
            else if (oDebitCreditSetup.ChequeType == EnumChequeType.Received)
            {
                sTempSQL = "SELECT ChequeDate FROM ReceivedCheque WHERE ReceivedChequeID=(" + sAdvSQL + ")";
                sSQL = sSQL + " (" + sTempSQL + ") AS ChequeDate,";
            }
            else
            {
                sTempSQL = "SELECT GETDATE() AS ChequeDate";
                sSQL = sSQL + " (" + sTempSQL + ") AS ChequeDate,";
            }
            #endregion

            #region BankName
            if (oDebitCreditSetup.ChequeType == EnumChequeType.Payment)
            {
                sTempSQL = "SELECT BankName FROM View_Cheque WHERE ChequeID=(" + sAdvSQL + ")";
                sSQL = sSQL + " (" + sTempSQL + ") AS BankName,";
            }
            else if (oDebitCreditSetup.ChequeType == EnumChequeType.Received)
            {
                sTempSQL = "SELECT BankName FROM ReceivedCheque WHERE ReceivedChequeID=(" + sAdvSQL + ")";
                sSQL = sSQL + " (" + sTempSQL + ") AS BankName,";
            }
            else
            {
                sTempSQL = "SELECT '' AS BankName";
                sSQL = sSQL + " (" + sTempSQL + ") AS BankName,";
            }
            #endregion

            #region Voucher Reference Amount
            oDataCollectionSetups = new List<DataCollectionSetup>();
            oDataCollectionSetups = this.GetDataCollectionSetups(EnumDataSetupType.ChequeReferenceAmountSetup, oDebitCreditSetup.DataCollectionSetups);
            sTempSQL = this.GetDataCollectSQL(oDataCollectionSetups, oDataRow);
            sSQL = sSQL + " (" + sTempSQL + ") AS Amount,";
            #endregion

            #region Voucher Reference Description
            oDataCollectionSetups = new List<DataCollectionSetup>();
            oDataCollectionSetups = this.GetDataCollectionSetups(EnumDataSetupType.ChequeReferenceDescriptinSetup, oDebitCreditSetup.DataCollectionSetups);
            sTempSQL = this.GetDataCollectSQL(oDataCollectionSetups, oDataRow);
            sSQL = sSQL + " (" + sTempSQL + ") AS Description,";
            #endregion

            #region Voucher Reference TransactionDate
            oDataCollectionSetups = new List<DataCollectionSetup>();
            oDataCollectionSetups = this.GetDataCollectionSetups(EnumDataSetupType.ChequeReferenceDateSetup, oDebitCreditSetup.DataCollectionSetups);
            sTempSQL = this.GetDataCollectSQL(oDataCollectionSetups, oDataRow);
            sSQL = sSQL + " (" + sTempSQL + ") AS TransactionDate";
            #endregion

            #region SQL Execute
            IDataReader reader;
            sSQL = "SELECT " + sSQL;
            reader = VoucherDA.Gets(tc, sSQL);
            NullHandler oDataReader = new NullHandler(reader);
            if (reader.Read())
            {
                oVoucherCheque = new VoucherCheque();
                oVoucherCheque.ChequeType = oDebitCreditSetup.ChequeType;
                oVoucherCheque.ChequeID = oDataReader.GetInt32("ChequeID");
                oVoucherCheque.ChequeNo = oDataReader.GetString("ChequeNo");
                oVoucherCheque.ChequeDate = oDataReader.GetDateTime("ChequeDate");
                oVoucherCheque.BankName = oDataReader.GetString("BankName");
                oVoucherCheque.Amount = oDataReader.GetDoubleRound("Amount");
                oVoucherCheque.TransactionDate = oDataReader.GetDateTime("TransactionDate");
            }
            reader.Close();
            #endregion

            return oVoucherCheque;
        }
        private List<VoucherCheque> GetVoucherCheques(TransactionContext tc, DataRow oDataRow, DebitCreditSetup oDebitCreditSetup, VoucherDetail oVoucherDetail)
        {
            #region Declaration
            VoucherCheque oVoucherCheque = new VoucherCheque();
            VoucherCheque oTempVoucherCheque = new VoucherCheque();
            List<VoucherCheque> oVoucherCheques = new List<VoucherCheque>();
            List<DataCollectionSetup> oDataCollectionSetups = new List<DataCollectionSetup>();
            DataCollectionSetup oDataCollectionSetup = new DataCollectionSetup();
            string sSQL = "";
            #endregion

            #region Voucher Reference Data SQL
            oDataCollectionSetup.DataGenerateType = EnumDataGenerateType.AutomatedData;
            oDataCollectionSetup.QueryForValue = oDebitCreditSetup.ChequeReferenceDataSQL;
            oDataCollectionSetup.ReferenceValueFields = oDebitCreditSetup.ChequeReferenceCompareColumns;
            oDataCollectionSetups.Add(oDataCollectionSetup);
            sSQL = this.GetDataCollectSQL(oDataCollectionSetups, oDataRow);
            #endregion

            #region SQL Execute
            DataSet oDataSet = new DataSet();
            DataTable oDataTable = new DataTable();
            IDataReader reader = VoucherDA.Gets(tc, sSQL);
            oDataSet.Load(reader, LoadOption.OverwriteChanges, new string[1]);
            oDataTable = oDataSet.Tables[0];
            reader.Close();

            foreach (DataRow oRow in oDataTable.Rows)
            {
                oVoucherCheque = new VoucherCheque();
                oTempVoucherCheque = new VoucherCheque();
                oTempVoucherCheque = this.GetVocuherChequeData(tc, oRow, oDebitCreditSetup);
                oVoucherCheque.ChequeType = oTempVoucherCheque.ChequeType;
                oVoucherCheque.ChequeID = oTempVoucherCheque.ChequeID;
                oVoucherCheque.ChequeNo = oTempVoucherCheque.ChequeNo;
                oVoucherCheque.ChequeDate = oTempVoucherCheque.ChequeDate;
                oVoucherCheque.BankName = oTempVoucherCheque.BankName;
                oVoucherCheque.Amount = oTempVoucherCheque.Amount;
                oVoucherCheque.TransactionDate = oTempVoucherCheque.TransactionDate;
                oVoucherCheques.Add(oVoucherCheque);
            }
            #endregion

            return oVoucherCheques;
        }
        private VOReference GetVocuherOrderData(TransactionContext tc, Voucher oVoucher, DataRow oDataRow, DebitCreditSetup oDebitCreditSetup, long nUserId, int nSubLedgerID, int nBUID)
        {
            string sVOrderSql = ""; int nVOredrID = 0;
            string sSQL = ""; string sTempSQL = ""; string sAdvSQL = "";
            VOReference oVOReference = new VOReference();
            List<DataCollectionSetup> oDataCollectionSetups = new List<DataCollectionSetup>();

            #region OrderID
            oDataCollectionSetups = new List<DataCollectionSetup>();
            oDataCollectionSetups = this.GetDataCollectionSetups(EnumDataSetupType.OrderSetup, oDebitCreditSetup.DataCollectionSetups);
            sTempSQL = this.GetDataCollectSQL(oDataCollectionSetups, oDataRow);
            sSQL = sSQL + " (" + sTempSQL + ") AS OrderID,";
            sVOrderSql = " (" + sTempSQL + ") AS OrderID";
            sAdvSQL = sTempSQL;
            #endregion

            #region Insert VOrder
            IDataReader voreader;
            sVOrderSql = "Select " + sVOrderSql;
            voreader = VoucherDA.Gets(tc, sVOrderSql);
            NullHandler oVODataReader = new NullHandler(voreader);
            if (voreader.Read())
            {
                nVOredrID = oVODataReader.GetInt32("OrderID");                
            }
            voreader.Close();
            if (nVOredrID <= 0)
            {
                VOrder oVOrder = new VOrder();
                oVOrder.VOrderID = 0;
                oVOrder.BUID = nBUID;
                oVOrder.RefNo = "";
                oVOrder.VOrderRefType = oDebitCreditSetup.OrderRefType;
                oVOrder.VOrderRefTypeInt = (int)oDebitCreditSetup.OrderRefType;
                oVOrder.VOrderRefID = Convert.ToInt32(oDataRow[oDebitCreditSetup.OrderRefColumn]);
                oVOrder.OrderNo = Convert.ToString(oDataRow[oDebitCreditSetup.OrderNoColumn]);
                oVOrder.OrderDate = Convert.ToDateTime(oDataRow[oDebitCreditSetup.OrderDateColumn]);
                oVOrder.SubledgerID = nSubLedgerID;
                oVOrder.Remarks = "Auto generat for " + oDebitCreditSetup.OrderRefType.ToString() + " no " + Convert.ToString(oDataRow[oDebitCreditSetup.OrderNoColumn]);
                oVOrder.SubledgerName = "";
                oVOrder.BUName = "";
                oVOrder.BUCode = "";

                if (oVOrder.OrderNo == null || oVOrder.OrderNo == "")
                {
                    throw new Exception("Invalid Order No for " + oVoucher.ReferenceNote + "!");
                }
                if (oVOrder.VOrderRefID <= 0)
                {
                    throw new Exception("Invalid Cost Center Category for " + oVoucher.ReferenceNote + "!");
                }
                if ((int)oVOrder.VOrderRefType <= (int)EnumVOrderRefType.Manual)
                {
                    throw new Exception("Invalid Order Reference Type for " + oVoucher.ReferenceNote + "!");
                }
                if (oVOrder.VOrderRefID <= 0)
                {
                    throw new Exception("Invalid Order Reference for " + oVoucher.ReferenceNote + "!");
                }

                voreader = null;
                voreader = VOrderDA.InsertUpdate(tc, oVOrder, EnumDBOperation.Insert, nUserId);
                oVODataReader = new NullHandler(voreader);
                if (voreader.Read())
                {
                    nVOredrID = oVODataReader.GetInt32("VOrderID");
                }
                voreader.Close();
                if (nVOredrID <= 0)
                {
                    throw new Exception("Invalid Order Ref for " + oVoucher.ReferenceNote + "!");
                }
            }
            else
            {
                if (nSubLedgerID > 0)
                {
                    VOrderDA.UpdateSubledger(tc, nVOredrID, nSubLedgerID);
                }
            }
            #endregion

            #region RefNo
            sTempSQL = "SELECT RefNo FROM VOrder WHERE VOrderID=(" + sAdvSQL + ")";
            sSQL = sSQL + " (" + sTempSQL + ") AS RefNo,";
            #endregion

            #region OrderNo
            sTempSQL = "SELECT OrderNo FROM VOrder WHERE VOrderID=(" + sAdvSQL + ")";
            sSQL = sSQL + " (" + sTempSQL + ") AS OrderNo,";
            #endregion

            #region SubledgerID
            sTempSQL = "SELECT SubledgerID FROM VOrder WHERE VOrderID=(" + sAdvSQL + ")";
            sSQL = sSQL + " (" + sTempSQL + ") AS SubledgerID,";
            #endregion

            #region Order Amount
            oDataCollectionSetups = new List<DataCollectionSetup>();
            oDataCollectionSetups = this.GetDataCollectionSetups(EnumDataSetupType.OrderAmountSetup, oDebitCreditSetup.DataCollectionSetups);
            sTempSQL = this.GetDataCollectSQL(oDataCollectionSetups, oDataRow);
            sSQL = sSQL + " (" + sTempSQL + ") AS Amount,";
            #endregion

            #region Order Remarks
            oDataCollectionSetups = new List<DataCollectionSetup>();
            oDataCollectionSetups = this.GetDataCollectionSetups(EnumDataSetupType.OrderRemarkSetup, oDebitCreditSetup.DataCollectionSetups);
            sTempSQL = this.GetDataCollectSQL(oDataCollectionSetups, oDataRow);
            sSQL = sSQL + " (" + sTempSQL + ") AS Remarks,";
            #endregion

            #region Order TransactionDate
            oDataCollectionSetups = new List<DataCollectionSetup>();
            oDataCollectionSetups = this.GetDataCollectionSetups(EnumDataSetupType.OrderDateSetup, oDebitCreditSetup.DataCollectionSetups);
            sTempSQL = this.GetDataCollectSQL(oDataCollectionSetups, oDataRow);
            sSQL = sSQL + " (" + sTempSQL + ") AS TransactionDate";
            #endregion

            #region SQL Execute
            IDataReader reader;
            sSQL = "SELECT " + sSQL;
            reader = VoucherDA.Gets(tc, sSQL);
            NullHandler oDataReader = new NullHandler(reader);
            if (reader.Read())
            {
                oVOReference = new VOReference();
                oVOReference.OrderID = oDataReader.GetInt32("OrderID");
                oVOReference.RefNo = oDataReader.GetString("RefNo");
                oVOReference.OrderNo = oDataReader.GetString("OrderNo");                
                oVOReference.AmountInCurrency = oDataReader.GetDoubleRound("Amount");
                oVOReference.Remarks = oDataReader.GetString("Remarks");
                oVOReference.TransactionDate = oDataReader.GetDateTime("TransactionDate");
                oVOReference.SubledgerID = oDataReader.GetInt32("SubledgerID");
            }
            reader.Close();
            #endregion

            return oVOReference;
        }
        private List<VOReference> GetVoucherOrders(TransactionContext tc, Voucher oVoucher, DataRow oDataRow, DebitCreditSetup oDebitCreditSetup, VoucherDetail oVoucherDetail, long nUserId, int nSubLedgerID, int nBUID)
        {
            #region Declaration
            VOReference oVOReference = new VOReference();
            VOReference oTempVOReference = new VOReference();
            List<VOReference> oVOReferences = new List<VOReference>();
            List<DataCollectionSetup> oDataCollectionSetups = new List<DataCollectionSetup>();
            DataCollectionSetup oDataCollectionSetup = new DataCollectionSetup();
            string sSQL = "";
            #endregion

            #region Voucher Reference Data SQL
            oDataCollectionSetup.DataGenerateType = EnumDataGenerateType.AutomatedData;
            oDataCollectionSetup.QueryForValue = oDebitCreditSetup.OrderReferenceDataSQL;
            oDataCollectionSetup.ReferenceValueFields = oDebitCreditSetup.OrderReferenceCompareColumns;
            oDataCollectionSetups.Add(oDataCollectionSetup);
            sSQL = this.GetDataCollectSQL(oDataCollectionSetups, oDataRow);
            #endregion

            #region SQL Execute
            DataSet oDataSet = new DataSet();
            DataTable oDataTable = new DataTable();
            IDataReader reader = VoucherDA.Gets(tc, sSQL);
            oDataSet.Load(reader, LoadOption.OverwriteChanges, new string[1]);
            oDataTable = oDataSet.Tables[0];
            reader.Close();

            foreach (DataRow oRow in oDataTable.Rows)
            {
                oVOReference = new VOReference();
                oTempVOReference = new VOReference();
                oTempVOReference = this.GetVocuherOrderData(tc, oVoucher, oRow, oDebitCreditSetup, nUserId, nSubLedgerID,  nBUID);
                oVOReference.OrderID = oTempVOReference.OrderID;
                oVOReference.RefNo = oTempVOReference.RefNo;
                oVOReference.OrderNo = oTempVOReference.OrderNo;
                oVOReference.AmountInCurrency = oTempVOReference.AmountInCurrency;
                oVOReference.TransactionDate = oTempVOReference.TransactionDate;
                oVOReference.Remarks = oTempVOReference.Remarks;
                oVOReference.SubledgerID = oTempVOReference.SubledgerID;
                oVOReferences.Add(oVOReference);
            }
            #endregion

            return oVOReferences;
        }
        private VPTransaction GetVPTransactionData(TransactionContext tc, DataRow oRow, DebitCreditSetup oDebitCreditSetup)
        {
            string sSQL = ""; string sTempSQL = ""; string sAdvSQL = "";
            VPTransaction oVPTransaction = new VPTransaction();
            List<DataCollectionSetup> oDataCollectionSetups = new List<DataCollectionSetup>();

            #region WorkingUnitID
            oDataCollectionSetups = new List<DataCollectionSetup>();
            oDataCollectionSetups = this.GetDataCollectionSetups(EnumDataSetupType.InventoryWorkingUnitSetup, oDebitCreditSetup.DataCollectionSetups);
            sTempSQL = this.GetDataCollectSQL(oDataCollectionSetups, oRow);
            sSQL = sSQL + " (" + sTempSQL + ") AS WorkingUnitID,";
            sAdvSQL = sTempSQL;
            #endregion

            #region WorkingUnitName
            sTempSQL = "SELECT LocationName+'['+OperationUnitName+']' FROM View_WorkingUnit WHERE WorkingUnitID=(" + sAdvSQL + ")";
            sSQL = sSQL + " (" + sTempSQL + ") AS WorkingUnitName,";
            #endregion

            #region ProductID
            oDataCollectionSetups = new List<DataCollectionSetup>();
            oDataCollectionSetups = this.GetDataCollectionSetups(EnumDataSetupType.InventoryProductSetup, oDebitCreditSetup.DataCollectionSetups);
            sTempSQL = this.GetDataCollectSQL(oDataCollectionSetups, oRow);
            sSQL = sSQL + " (" + sTempSQL + ") AS ProductID,";
            sAdvSQL = sTempSQL;
            #endregion

            #region ProductName
            sTempSQL = "SELECT ProductName FROM Product WHERE ProductID=(" + sAdvSQL + ")";
            sSQL = sSQL + " (" + sTempSQL + ") AS ProductName,";
            #endregion

            #region MUnitID
            oDataCollectionSetups = new List<DataCollectionSetup>();
            oDataCollectionSetups = this.GetDataCollectionSetups(EnumDataSetupType.InventoryUnitSetup, oDebitCreditSetup.DataCollectionSetups);
            sTempSQL = this.GetDataCollectSQL(oDataCollectionSetups, oRow);
            sSQL = sSQL + " (" + sTempSQL + ") AS MUnitID,";
            sAdvSQL = sTempSQL;
            #endregion

            #region MUnitName
            sTempSQL = "SELECT Symbol FROM MeasurementUnit WHERE MeasurementUnitID=(" + sAdvSQL + ")";
            sSQL = sSQL + " (" + sTempSQL + ") AS MUnitName,";
            #endregion

            #region InventoryDescription
            oDataCollectionSetups = new List<DataCollectionSetup>();
            oDataCollectionSetups = this.GetDataCollectionSetups(EnumDataSetupType.InventoryDescriptionSetup, oDebitCreditSetup.DataCollectionSetups);
            sTempSQL = this.GetDataCollectSQL(oDataCollectionSetups, oRow);
            sSQL = sSQL + " (" + sTempSQL + ") AS Description,";
            #endregion

            #region Inventory Date
            oDataCollectionSetups = new List<DataCollectionSetup>();
            oDataCollectionSetups = this.GetDataCollectionSetups(EnumDataSetupType.InventoryDateSetup, oDebitCreditSetup.DataCollectionSetups);
            sTempSQL = this.GetDataCollectSQL(oDataCollectionSetups, oRow);
            sSQL = sSQL + " (" + sTempSQL + ") AS TransactionDate,";
            #endregion

            #region Inventory Qty
            oDataCollectionSetups = new List<DataCollectionSetup>();
            oDataCollectionSetups = this.GetDataCollectionSetups(EnumDataSetupType.InventoryQtySetup, oDebitCreditSetup.DataCollectionSetups);
            sTempSQL = this.GetDataCollectSQL(oDataCollectionSetups, oRow);
            sSQL = sSQL + " (" + sTempSQL + ") AS Qty,";
            #endregion

            #region UnitPrice
            oDataCollectionSetups = new List<DataCollectionSetup>();
            oDataCollectionSetups = this.GetDataCollectionSetups(EnumDataSetupType.InventoryUnitPriceSetup, oDebitCreditSetup.DataCollectionSetups);
            sTempSQL = this.GetDataCollectSQL(oDataCollectionSetups, oRow);
            sSQL = sSQL + " (" + sTempSQL + ") AS UnitPrice";
            #endregion

            #region SQL Execute
            IDataReader reader;
            sSQL = "SELECT " + sSQL;
            reader = VoucherDA.Gets(tc, sSQL);
            NullHandler oDataReader = new NullHandler(reader);
            if (reader.Read())
            {
                oVPTransaction = new VPTransaction();
                oVPTransaction.WorkingUnitID = oDataReader.GetInt32("WorkingUnitID");
                oVPTransaction.WorkingUnitName = oDataReader.GetString("WorkingUnitName");
                oVPTransaction.ProductID = oDataReader.GetInt32("ProductID");
                oVPTransaction.ProductName = oDataReader.GetString("ProductName");
                oVPTransaction.MUnitID = oDataReader.GetInt32("MUnitID");
                oVPTransaction.MUnitName = oDataReader.GetString("MUnitName");
                oVPTransaction.TransactionDate = oDataReader.GetDateTime("TransactionDate");
                oVPTransaction.Qty = oDataReader.GetDoubleRound("Qty");
                oVPTransaction.UnitPrice = oDataReader.GetDouble("UnitPrice");
                oVPTransaction.Description = oDataReader.GetString("Description");
                oVPTransaction.Amount = (oVPTransaction.Qty * oVPTransaction.UnitPrice);
            }
            reader.Close();
            #endregion

            return oVPTransaction;
        }
        private List<VPTransaction> GetVPTransactions(TransactionContext tc, DataRow oDataRow, DebitCreditSetup oDebitCreditSetup, Voucher oVoucher, VoucherDetail oVoucherDetail)
        {
            #region Declaration
            VPTransaction oVPTransaction = new VPTransaction();
            VPTransaction oTempVPTransaction = new VPTransaction();
            List<VPTransaction> oVPTransactions = new List<VPTransaction>();
            List<DataCollectionSetup> oDataCollectionSetups = new List<DataCollectionSetup>();
            DataCollectionSetup oDataCollectionSetup = new DataCollectionSetup();
            string sSQL = "";
            #endregion

            #region Voucher Reference Data SQL
            oDataCollectionSetup.DataGenerateType = EnumDataGenerateType.AutomatedData;
            oDataCollectionSetup.QueryForValue = oDebitCreditSetup.InventoryDataSQL;
            oDataCollectionSetup.ReferenceValueFields = oDebitCreditSetup.InventoryCompareColumns;
            oDataCollectionSetups.Add(oDataCollectionSetup);
            sSQL = this.GetDataCollectSQL(oDataCollectionSetups, oDataRow);
            #endregion

            #region SQL Execute
            DataSet oDataSet = new DataSet();
            DataTable oDataTable = new DataTable();
            IDataReader reader = VoucherDA.Gets(tc, sSQL);            
            oDataSet.Load(reader, LoadOption.OverwriteChanges, new string[1]);
            oDataTable = oDataSet.Tables[0];
            reader.Close();

            foreach (DataRow oRow in oDataTable.Rows)
            {
                oVPTransaction = new VPTransaction();
                oTempVPTransaction = new VPTransaction();
                oTempVPTransaction = this.GetVPTransactionData(tc, oRow, oDebitCreditSetup);
                oVPTransaction.CurrencyID = oVoucherDetail.CurrencyID;
                oVPTransaction.CurrencySymbol = oVoucherDetail.CUSymbol;
                oVPTransaction.ConversionRate = oVoucherDetail.ConversionRate;
                oVPTransaction.WorkingUnitID = oTempVPTransaction.WorkingUnitID;
                oVPTransaction.WorkingUnitName = oTempVPTransaction.WorkingUnitName;
                oVPTransaction.ProductID = oTempVPTransaction.ProductID;
                oVPTransaction.ProductName = oTempVPTransaction.ProductName;
                oVPTransaction.MUnitID = oTempVPTransaction.MUnitID;
                oVPTransaction.MUnitName = oTempVPTransaction.MUnitName;
                oVPTransaction.Qty = oTempVPTransaction.Qty;
                oVPTransaction.UnitPrice = oTempVPTransaction.UnitPrice;
                oVPTransaction.Amount = oTempVPTransaction.Amount;
                oVPTransaction.TransactionDate = oTempVPTransaction.TransactionDate;
                oVPTransaction.Description = oTempVPTransaction.Description;
                oVPTransactions.Add(oVPTransaction);
            }
            #endregion

            return oVPTransactions;
        }
        private double GetVoucherAmount(List<VoucherDetail> oVoucherDetails)
        {
            double nVoucherAmount = 0;
            foreach (VoucherDetail oItem in oVoucherDetails)
            {
                if (oItem.IsDebit)
                {
                    nVoucherAmount = nVoucherAmount + oItem.Amount;
                }
            }
            return nVoucherAmount;
        }
        private object[] GetPerameterValueFromObject(string sParam, object oPerameterObject)
        {
            string[] aParams = sParam.Split(',');
            object[] args = new object[aParams.Length];

            int i = 0;
            foreach (string oItem in aParams)
            {
                args[i] = oPerameterObject.GetType().GetProperty(oItem).GetValue(oPerameterObject, null);
                i++;
            }
            return args;
        }
        public List<Voucher> GetsAutoVoucher(IntegrationSetup oIntegrationSetup, object oPerameterObject, bool bUserPerameterObject, Int64 nUserId)
         {
            #region Declaration
            Voucher oVoucher = new Voucher();
            Voucher oTempVoucher = new Voucher();
            List<Voucher> oVouchers = new List<Voucher>();
            VoucherDetail oVoucherDetail = new VoucherDetail();
            VoucherDetail oTempVoucherDetail = new VoucherDetail();
            List<VoucherDetail> oVoucherDetails = new List<VoucherDetail>();
            TransactionContext tc = null;
            DataSet oDataSet = new DataSet();
            DataTable oDataTable = new DataTable();
            int nVoucherID = 0;
            #endregion

            try
            {
                tc = TransactionContext.Begin();
                IDataReader reader;
                string sSQL = "";
                
                if (bUserPerameterObject)
                {
                    sSQL = SQLParser.MakeSQL(oIntegrationSetup.DataCollectionSQL, this.GetPerameterValueFromObject(oIntegrationSetup.KeyColumn, oPerameterObject));
                }
                else
                {   
                    string sSQL1StPart = "";
                    string sSQL2ndPart = "";
                    sSQL1StPart = oIntegrationSetup.DataCollectionSQL.Split('#')[0];
                    if (oIntegrationSetup.DataCollectionSQL.Split('#').Length > 1)
                    {
                        sSQL2ndPart = oIntegrationSetup.DataCollectionSQL.Split('#')[1];
                    }

                    if ((EnumCompareOperator)oIntegrationSetup.DateType == EnumCompareOperator.EqualTo)
                    {
                        //Hare note column use as date compare column
                        sSQL = sSQL1StPart + " AND CONVERT(DATE,CONVERT(VARCHAR(12)," + oIntegrationSetup.Note + ",106)) = CONVERT(DATE,CONVERT(VARCHAR(12),'" + oIntegrationSetup.StartDate.ToString("dd MMM yyyy") + "',106)) " + sSQL2ndPart;
                        sSQL = SQLParser.MakeSQL(sSQL, oIntegrationSetup.BUID);
                    }
                    else if ((EnumCompareOperator)oIntegrationSetup.DateType == EnumCompareOperator.Between)
                    {
                        //Hare note column use as date compare column
                        sSQL = sSQL1StPart + " AND CONVERT(DATE,CONVERT(VARCHAR(12)," + oIntegrationSetup.Note + ",106)) BETWEEN CONVERT(DATE,CONVERT(VARCHAR(12),'" + oIntegrationSetup.StartDate.ToString("dd MMM yyyy") + "',106))  AND CONVERT(DATE,CONVERT(VARCHAR(12),'" + oIntegrationSetup.EndDate.ToString("dd MMM yyyy") + "',106)) " + sSQL2ndPart;
                        sSQL = SQLParser.MakeSQL(sSQL, oIntegrationSetup.BUID);
                    }
                    else
                    {
                        //sSQL = oIntegrationSetup.DataCollectionSQL;          
                        sSQL = sSQL1StPart + " " + sSQL2ndPart;
                        sSQL = SQLParser.MakeSQL(sSQL, oIntegrationSetup.BUID);
                    }
                }
                reader = VoucherDA.Gets(tc, sSQL);
                oDataSet.Load(reader, LoadOption.OverwriteChanges, new string[1]);
                oDataTable = oDataSet.Tables[0];
                reader.Close();
                int nCount = 0; bool bFlag = false;
                foreach (DataRow oRow in oDataTable.Rows)
                {
                    if (oIntegrationSetup.IntegrationSetupDetails != null)
                    {
                        foreach (IntegrationSetupDetail oIntegrationSetupDetail in oIntegrationSetup.IntegrationSetupDetails)
                        {
                            #region Map Voucher Data
                            bFlag = true;
                            nVoucherID--;
                            oVoucher = new Voucher();
                            oTempVoucher = new Voucher();
                            oTempVoucher = this.GetVocuherData(tc, oRow, oIntegrationSetupDetail, nUserId);
                            oVoucher.VoucherID = nVoucherID;
                            oVoucher.VoucherAmount = 1;
                            oVoucher.AuthorizedBy = Convert.ToInt32(nUserId);
                            oVoucher.VoucherTypeID = oIntegrationSetupDetail.VoucherTypeID;
                            oVoucher.VoucherName = oIntegrationSetupDetail.VoucherName;
                            oVoucher.BUID = oTempVoucher.BUID;
                            oVoucher.BUCode = oTempVoucher.BUCode;
                            oVoucher.BUName = oTempVoucher.BUName;
                            oVoucher.VoucherDate = oTempVoucher.VoucherDate;
                            oVoucher.VoucherNo = this.GetVoucherNo(tc, oVoucher.BUID, oVoucher.VoucherTypeID, oVoucher.VoucherDate, nUserId);
                            oVoucher.Narration = oTempVoucher.Narration;
                            oVoucher.ReferenceNote = oTempVoucher.ReferenceNote;
                            oVoucher.BaseCurrencyID = oTempVoucher.BaseCurrencyID;
                            oVoucher.BaseCUSymbol = oTempVoucher.BaseCUSymbol;
                            oVoucher.BatchID = oTempVoucher.BatchID;

                            if (oIntegrationSetupDetail.VoucherDateSetup == "Manual Data")
                            {
                                oVoucher.ManualDataEntry = "VoucherDate,";
                            }

                            #region Map Voucher Detail Data
                            oVoucherDetails = new List<VoucherDetail>();
                            if (oIntegrationSetupDetail.DebitCreditSetups != null)
                            {
                                foreach (DebitCreditSetup oDebitCreditSetup in oIntegrationSetupDetail.DebitCreditSetups)
                                {
                                    DataSet oDataSetDebitCredit = new DataSet();
                                    DataTable oDataTableDebitCredit = new DataTable();
                                    IDataReader readerdebitcredit;
                                    string sSQLDebitCredit = SQLParser.MakeSQL(oDebitCreditSetup.DataCollectionQuery, this.GetPerameterValue(oDebitCreditSetup.CompareColumn, oRow));
                                    readerdebitcredit = VoucherDA.Gets(tc, sSQLDebitCredit);
                                    oDataSetDebitCredit.Load(readerdebitcredit, LoadOption.OverwriteChanges, new string[1]);
                                    oDataTableDebitCredit = oDataSetDebitCredit.Tables[0];
                                    readerdebitcredit.Close();

                                    #region Debit or Credit Process
                                    foreach (DataRow oRowDebitCredit in oDataTableDebitCredit.Rows)
                                    {
                                        if (oDebitCreditSetup.AccountHeadSetup == "Manual Data")
                                        {
                                            oVoucher.ManualDataEntry = "AccountHead,";
                                        }

                                        #region Declaration
                                        oVoucherDetail = new VoucherDetail();
                                        oTempVoucherDetail = new VoucherDetail();
                                        #endregion

                                        #region Voucher Detail
                                        oTempVoucherDetail = this.GetVocuherDetailData(tc, oVoucher, oRow, oRowDebitCredit, oDebitCreditSetup);
                                        oVoucherDetail.BUID = oVoucher.BUID;                                      
                                        oVoucherDetail.AreaID = oTempVoucherDetail.AreaID;
                                        oVoucherDetail.AreaCode = oTempVoucherDetail.AreaCode;
                                        oVoucherDetail.AreaName = oTempVoucherDetail.AreaName;
                                        oVoucherDetail.ZoneID = oTempVoucherDetail.ZoneID;
                                        oVoucherDetail.ZoneCode = oTempVoucherDetail.ZoneCode;
                                        oVoucherDetail.ZoneName = oTempVoucherDetail.ZoneName;
                                        oVoucherDetail.SiteID = oTempVoucherDetail.SiteID;
                                        oVoucherDetail.SiteCode = oTempVoucherDetail.SiteCode;
                                        oVoucherDetail.SiteName = oTempVoucherDetail.SiteName;
                                        oVoucherDetail.DeptID = oTempVoucherDetail.DeptID;
                                        oVoucherDetail.DeptCode = oTempVoucherDetail.DeptCode;
                                        oVoucherDetail.DeptName = oTempVoucherDetail.DeptName;
                                        oVoucherDetail.ProductID = oTempVoucherDetail.ProductID;
                                        oVoucherDetail.PCode = oTempVoucherDetail.PCode;
                                        oVoucherDetail.PName = oTempVoucherDetail.PName;
                                        oVoucherDetail.AccountHeadID = oTempVoucherDetail.AccountHeadID;
                                        oVoucherDetail.AccountHeadCode = oTempVoucherDetail.AccountCode;
                                        oVoucherDetail.AccountHeadName = oTempVoucherDetail.AccountHeadName;
                                        oVoucherDetail.CurrencyID = oTempVoucherDetail.CurrencyID;
                                        oVoucherDetail.CUName = oTempVoucherDetail.CUName;
                                        oVoucherDetail.CUSymbol = oTempVoucherDetail.CUSymbol;
                                        oVoucherDetail.ConversionRate = oTempVoucherDetail.ConversionRate;
                                        oVoucherDetail.AmountInCurrency = oTempVoucherDetail.AmountInCurrency;
                                        oVoucherDetail.Narration = oTempVoucherDetail.Narration;
                                        oVoucherDetail.IsDebit = oDebitCreditSetup.IsDebit;
                                        if (oTempVoucherDetail.AmountInCurrency < 0)
                                        {
                                            oVoucherDetail.AmountInCurrency = (-1) * oVoucherDetail.AmountInCurrency;
                                            oVoucherDetail.IsDebit = !oDebitCreditSetup.IsDebit;
                                        }
                                        oVoucherDetail.Amount = (oVoucherDetail.AmountInCurrency * oVoucherDetail.ConversionRate);
                                        if (oVoucherDetail.IsDebit)
                                        {
                                            oVoucherDetail.DebitAmount = oVoucherDetail.Amount;
                                            oVoucherDetail.CreditAmount = 0.00;
                                        }
                                        else
                                        {
                                            oVoucherDetail.DebitAmount = 0.00;
                                            oVoucherDetail.CreditAmount = oVoucherDetail.Amount;
                                        }

                                        #endregion
                                        if (Math.Round(oVoucherDetail.Amount, 2) != 0)
                                        {
                                            ChartsOfAccount oChartsOfAccount = this.GetAccountHead(tc, oVoucherDetail.AccountHeadID); 
                                            #region Voucher CostCenter
                                            if (oDebitCreditSetup.IsCostCenterCreate && oChartsOfAccount.IsCostCenterApply)
                                            {
                                                oVoucherDetail.CCTs = GetCostCenterTransactions(tc, oRowDebitCredit, oDebitCreditSetup, oVoucher, oVoucherDetail, nUserId);
                                            }
                                            #endregion

                                            #region Voucher Bill Transactions
                                            if (oDebitCreditSetup.IsVoucherBill && oChartsOfAccount.IsBillRefApply)
                                            {
                                                if (!oDebitCreditSetup.HasBillReference)
                                                {
                                                    oVoucherDetail.VoucherBillTrs = this.GetVoucherBillTransactions(tc, oRowDebitCredit, oDebitCreditSetup, oVoucher, oVoucherDetail, nUserId, 0);
                                                }
                                            }
                                            #endregion

                                            #region Voucher Cheques
                                            if (oDebitCreditSetup.IsChequeReferenceCreate)
                                            {
                                                if (!oDebitCreditSetup.HasChequeReference)
                                                {
                                                    oVoucherDetail.VoucherCheques = GetVoucherCheques(tc, oRowDebitCredit, oDebitCreditSetup, oVoucherDetail);
                                                }
                                            }
                                            #endregion

                                            #region Voucher Order Reference
                                            if (oDebitCreditSetup.IsOrderReferenceApply && oChartsOfAccount.IsOrderReferenceApply)
                                            {
                                                if (!oDebitCreditSetup.HasOrderReference)
                                                {
                                                    oVoucherDetail.VOReferences = GetVoucherOrders(tc, oVoucher, oRowDebitCredit, oDebitCreditSetup, oVoucherDetail, nUserId, 0, oVoucher.BUID);
                                                }
                                            }
                                            #endregion

                                            #region VPTransactions
                                            if (oDebitCreditSetup.IsInventoryEffect && oChartsOfAccount.IsInventoryApply)
                                            {
                                                oVoucherDetail.VPTransactions = GetVPTransactions(tc, oRowDebitCredit, oDebitCreditSetup, oVoucher, oVoucherDetail);
                                            }
                                            #endregion

                                            string sErrorMessage = "";
                                            sErrorMessage = this.AccountHeadWiseInputValidation(tc, oVoucherDetail, oVoucher.ReferenceNote, 5.00);
                                            if (sErrorMessage != "")
                                            {
                                                throw new Exception(sErrorMessage);
                                            }

                                            oVoucherDetails.Add(oVoucherDetail);
                                        }
                                        //else
                                        //{
                                        //    bFlag = false;
                                        //}
                                    }
                                    #endregion
                                }
                            }
                            #endregion

                            #region Auto Head Define for Inventory Voucher Debit/Credit Miss Match
                            if (oIntegrationSetup.VoucherSetup == EnumVoucherSetup.Import_Payment_Settlement_By_Loan || oIntegrationSetup.VoucherSetup == EnumVoucherSetup.Import_Invoice_Inventory_Voucher_GRN_InventoryItem || oIntegrationSetup.VoucherSetup == EnumVoucherSetup.Import_Invoice_Inventory_Voucher_GRN_InventoryItem || oIntegrationSetup.VoucherSetup == EnumVoucherSetup.Local_Invoice_Inventory_Voucher_GRN || oIntegrationSetup.VoucherSetup == EnumVoucherSetup.Local_Invoice_Inventory_Voucher_GRN_FixedAsset)
                            {
                                double nTotalDebitAmount = this.GetDebitCreditAmount(oVoucherDetails, true);
                                double nTotalCreditAmount = this.GetDebitCreditAmount(oVoucherDetails, false);
                                double nCurrencyConversionDiffAmount = (Math.Round(nTotalDebitAmount, 2) - Math.Round(nTotalCreditAmount, 2));
                                if (nCurrencyConversionDiffAmount != 0.00)
                                {
                                    ChartsOfAccount oChartsOfAccount = new ChartsOfAccount();
                                    #region Get Fixed Account Head
                                    IDataReader coareader;
                                    coareader = ChartsOfAccountDA.Get(tc, 20); //Fixed Account Head Currency Conversion Gain/Loss for GRN
                                    NullHandler oCOADataReader = new NullHandler(coareader);
                                    if (coareader.Read())
                                    {
                                        oChartsOfAccount.AccountHeadID = oCOADataReader.GetInt32("AccountHeadID");
                                        oChartsOfAccount.AccountCode = oCOADataReader.GetString("AccountCode");
                                        oChartsOfAccount.AccountHeadName = oCOADataReader.GetString("AccountHeadName");
                                        oChartsOfAccount.CurrencyID = oCOADataReader.GetInt32("CurrencyID");
                                        oChartsOfAccount.CSymbol = oCOADataReader.GetString("CSymbol");
                                    }
                                    coareader.Close();
                                    #endregion

                                    double nGAPAmount = nCurrencyConversionDiffAmount;
                                    if (nCurrencyConversionDiffAmount < 0)
                                    {
                                        nGAPAmount = (nCurrencyConversionDiffAmount * (-1));
                                    }
                                    if (oChartsOfAccount.AccountHeadID > 0 && nGAPAmount > 0 && nGAPAmount < 5)
                                    {
                                        oVoucherDetail = new VoucherDetail();
                                        oVoucherDetail.BUID = oVoucher.BUID;
                                        oVoucherDetail.AccountHeadID = oChartsOfAccount.AccountHeadID;
                                        oVoucherDetail.AccountHeadCode = oChartsOfAccount.AccountCode;
                                        oVoucherDetail.AccountHeadName = oChartsOfAccount.AccountHeadName;
                                        oVoucherDetail.CurrencyID = oChartsOfAccount.CurrencyID;
                                        oVoucherDetail.CUName = oChartsOfAccount.CSymbol;
                                        oVoucherDetail.CUSymbol = oChartsOfAccount.CSymbol;
                                        oVoucherDetail.AmountInCurrency = Math.Round(nCurrencyConversionDiffAmount, 2);
                                        oVoucherDetail.ConversionRate = 1.00;
                                        oVoucherDetail.Narration = "Currency Conversion Gain Loss for " + oVoucher.Narration;
                                        oVoucherDetail.IsDebit = false;
                                        if (oVoucherDetail.AmountInCurrency < 0)
                                        {
                                            oVoucherDetail.AmountInCurrency = (-1) * oVoucherDetail.AmountInCurrency;
                                            oVoucherDetail.IsDebit = !oVoucherDetail.IsDebit;
                                        }
                                        oVoucherDetail.Amount = Math.Round((oVoucherDetail.AmountInCurrency * oVoucherDetail.ConversionRate), 2);
                                        if (oVoucherDetail.IsDebit)
                                        {
                                            oVoucherDetail.DebitAmount = oVoucherDetail.Amount;
                                            oVoucherDetail.CreditAmount = 0.00;
                                        }
                                        else
                                        {
                                            oVoucherDetail.DebitAmount = 0.00;
                                            oVoucherDetail.CreditAmount = oVoucherDetail.Amount;
                                        }
                                        oVoucherDetails.Add(oVoucherDetail);
                                    }
                                }

                            }
                            #endregion


                            #region Voucher Mapping
                            VoucherMapping oVoucherMapping = new VoucherMapping();
                            oVoucherMapping.TableName = oIntegrationSetupDetail.UpdateTable;
                            oVoucherMapping.PKColumnName = oIntegrationSetupDetail.KeyColumn;
                            oVoucherMapping.VoucherSetup = oIntegrationSetup.VoucherSetup;
                            oVoucherMapping.VoucherSetupInt = (int)oIntegrationSetup.VoucherSetup;
                            oVoucherMapping.PKValue = (oRow[oIntegrationSetupDetail.KeyColumn] == DBNull.Value) ? 0 : Convert.ToInt32(oRow[oIntegrationSetupDetail.KeyColumn]);
                            oVoucher.VoucherMapping = oVoucherMapping;
                            oVoucher.TableName = oVoucherMapping.TableName;
                            oVoucher.PKValue = oVoucherMapping.PKValue;
                            #endregion

                            oVoucher.VoucherDetailLst = oVoucherDetails;
                            oVoucher.VoucherAmount = this.GetVoucherAmount(oVoucherDetails);
                            nCount++;
                            oVoucher.PrintCount = nCount;                            
                            if (oVoucher.VoucherAmount>0)
                            {
                                oVouchers.Add(oVoucher);
                            }
                            #endregion
                        }
                    }
                }
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                oVouchers = new List<Voucher>();
                oVoucher = new Voucher();
                oVoucher.ErrorMessage = e.Message.Split('~')[0];
                oVouchers.Add(oVoucher);

                //ExceptionLog.Write(e);
                //throw new ServiceException(e.Message);
                #endregion
            }
            return oVouchers;
        }
        public List<Voucher> CommitAutoVoucher(TransactionContext tc, bool bUsetc, List<Voucher> oParamVouchers, Int64 nUserId)
        {
            #region Declaration
            Voucher oVoucher = new Voucher();
            List<VoucherDetail> oVoucherDetails = new List<VoucherDetail>();
            List<CostCenterTransaction> oCostCenterTransactions = new List<CostCenterTransaction>();
            List<VoucherCheque> oVoucherCheques = new List<VoucherCheque>();
            List<VOReference> oVOReferences = new List<VOReference>();
            List<VoucherBillTransaction> VoucherBillTrs = new List<VoucherBillTransaction>();
            List<VPTransaction> oVPTransactions = new List<VPTransaction>();
            List<Voucher> oVouchers = new List<Voucher>();
            VoucherMapping oVoucherMapping = new VoucherMapping();
            if (!bUsetc) { tc = null; }
            #endregion

            try
            {
                tc = TransactionContext.Begin(true);
                string sVoucherDetailIDs = "";
                int nVoucherDetailID = 0;
                int nCurrencyID = 0;
                double nConversionRate = 0;
                string sErrorMessage = "";

                foreach (Voucher oTempVoucher in oParamVouchers)
                {
                    #region Check Debit Credit Amount Are Equal
                    oVoucherDetails = new List<VoucherDetail>();
                    oVoucherDetails = oTempVoucher.VoucherDetailLst;

                    oVoucherMapping = new VoucherMapping();
                    oVoucherMapping = oTempVoucher.VoucherMapping;

                    sVoucherDetailIDs = "";
                    nVoucherDetailID = 0;

                    #region Voucher Data Validation
                    sErrorMessage = "";
                    sErrorMessage = this.FeedBackForDataValidation(oVoucherDetails);
                    if (sErrorMessage != "")
                    {
                        throw new Exception(sErrorMessage);
                    }
                    #endregion

                    #region Debit Credit Equal
                    sErrorMessage = "";
                    sErrorMessage = this.FeedBackForDebitCreditEqual(oVoucherDetails, 0.5);
                    if (sErrorMessage != "")
                    {
                        throw new Exception(sErrorMessage);
                    }
                    #endregion

                    #region Reference Value Check
                    foreach (VoucherDetail oTempVoucherDetail in oVoucherDetails)
                    {
                        sErrorMessage = "";
                        sErrorMessage = this.AccountHeadWiseInputValidation(tc, oTempVoucherDetail, "", 5.00);
                        if (sErrorMessage != "")
                        {
                            throw new Exception(sErrorMessage);
                        }
                    }
                    #endregion

                    #endregion

                    #region Voucher
                    IDataReader reader;
                    oTempVoucher.VoucherID = 0;
                    EnumRoleOperationType eRole = EnumRoleOperationType.None;
                    if (oTempVoucher.VoucherID <= 0)
                    {
                        eRole = EnumRoleOperationType.Add;
                        reader = VoucherDA.InsertUpdate(tc, oTempVoucher, EnumDBOperation.Insert, nUserId);
                    }
                    else
                    {
                        eRole = EnumRoleOperationType.Edit;
                        reader = VoucherDA.InsertUpdate(tc, oTempVoucher, EnumDBOperation.Update, nUserId);
                    }
                    NullHandler oReader = new NullHandler(reader);
                    if (reader.Read())
                    {
                        oVoucher = new Voucher();
                        oVoucher = CreateObject(oReader);
                    }

                    reader.Close();
                    #endregion

                    #region Voucher History
                    IDataReader historyreader;
                    VoucherHistory oVoucherHistory = new VoucherHistory();
                    oVoucherHistory.UserID = (int)nUserId;
                    oVoucherHistory.VoucherID = (int)oVoucher.VoucherID;
                    oVoucherHistory.TransactionDate = DateTime.Now;
                    oVoucherHistory.ActionType = eRole;
                    oVoucherHistory.Remarks = oVoucher.Narration;
                    historyreader = VoucherHistoryDA.InsertUpdate(tc, oVoucherHistory, EnumDBOperation.Insert, (int)nUserId);
                    historyreader.Close();
                    #endregion

                    #region Voucher Details
                    if (oVoucherDetails != null)
                    {
                        bool bIsDebit = false;
                        bool bIsUpdate = false;
                        foreach (VoucherDetail oItem in oVoucherDetails)
                        {
                            oCostCenterTransactions = oItem.CCTs;
                            oVoucherCheques = oItem.VoucherCheques;
                            oVOReferences = oItem.VOReferences;
                            VoucherBillTrs = oItem.VoucherBillTrs;
                            oVPTransactions = oItem.VPTransactions;

                            oItem.VoucherID = oVoucher.VoucherID;
                            nCurrencyID = oItem.CurrencyID;
                            nConversionRate = oItem.ConversionRate;

                            IDataReader readerdetail;
                            if (oItem.VoucherDetailID <= 0)
                            {
                                bIsUpdate = false;
                                readerdetail = VoucherDetailDA.InsertUpdate(tc, oItem, EnumDBOperation.Insert, "");
                            }
                            else
                            {
                                bIsUpdate = true;
                                readerdetail = VoucherDetailDA.InsertUpdate(tc, oItem, EnumDBOperation.Update, "");
                            }
                            NullHandler oReaderDetail = new NullHandler(readerdetail);

                            if (readerdetail.Read())
                            {
                                sVoucherDetailIDs = sVoucherDetailIDs + oReaderDetail.GetString("VoucherDetailID") + ",";
                                nVoucherDetailID = oReaderDetail.GetInt32("VoucherDetailID");
                                bIsDebit = oReaderDetail.GetBoolean("IsDebit");
                            }
                            readerdetail.Close();

                            #region Voucher With Cost Center
                            #region Delete CostCenterTransactions
                            if (bIsUpdate)
                            {
                                CostCenterTransaction oCostCenterTransaction = new CostCenterTransaction();
                                oCostCenterTransaction.VoucherDetailID = nVoucherDetailID;
                                CostCenterTransactionDA.Delete(tc, oCostCenterTransaction, EnumDBOperation.Delete, nUserId);
                            }
                            #endregion

                            #region CostCenterTransactions
                            if (oCostCenterTransactions.Count > 0)
                            {
                                int nCCTID = 0;
                                List<VoucherBillTransaction> oVBTransactions = new List<VoucherBillTransaction>();
                                List<VOReference> oSLVOReferences = new List<VOReference>();
                                List<VoucherCheque> oSLCheques = new List<VoucherCheque>();
                                foreach (CostCenterTransaction oCCT in oCostCenterTransactions)
                                {
                                    nCCTID = 0;
                                    oVBTransactions = oCCT.VBTransactions;
                                    oSLVOReferences = oCCT.VOReferences;
                                    oSLCheques = oCCT.VoucherCheques;
                                    
                                    IDataReader readerCCT;
                                    oCCT.VoucherDetailID = nVoucherDetailID;
                                    oCCT.CurrencyID = nCurrencyID;
                                    oCCT.CurrencyConversionRate = nConversionRate;
                                    readerCCT = CostCenterTransactionDA.InsertUpdate(tc, oCCT, EnumDBOperation.Insert, nUserId);
                                    NullHandler oReaderCCT = new NullHandler(readerCCT);
                                    if (readerCCT.Read())
                                    {
                                        nCCTID = oReaderCCT.GetInt32("CCTID");
                                    }
                                    readerCCT.Close();

                                    #region Subledger Bills
                                    if (oVBTransactions.Count > 0)
                                    {
                                        foreach (VoucherBillTransaction oVBT in oVBTransactions)
                                        {
                                            IDataReader readerSLBT;
                                            oVBT.VoucherDetailID = nVoucherDetailID;
                                            oVBT.CCTID = nCCTID;
                                            oVBT.CurrencyID = nCurrencyID;
                                            oVBT.ConversionRate = nConversionRate;
                                            readerSLBT = VoucherBillTransactionDA.InsertUpdate(tc, oVBT, EnumDBOperation.Insert, nUserId);
                                            NullHandler oReaderVBT = new NullHandler(readerSLBT);
                                            readerSLBT.Close();
                                        }
                                    }
                                    #endregion

                                    #region Subledger Order Ref
                                    if (oSLVOReferences.Count > 0)
                                    {
                                        foreach (VOReference oVOR in oSLVOReferences)
                                        {
                                            IDataReader readerSLOR;
                                            oVOR.VoucherDetailID = nVoucherDetailID;
                                            oVOR.IsDebit = bIsDebit;
                                            oVOR.CCTID = nCCTID;
                                            oVOR.CurrencyID = nCurrencyID;
                                            oVOR.ConversionRate = nConversionRate;
                                            oVOR.Amount = (oVOR.AmountInCurrency * nConversionRate);
                                            readerSLOR = VOReferenceDA.InsertUpdate(tc, oVOR, EnumDBOperation.Insert, nUserId);
                                            NullHandler oReaderSLOR = new NullHandler(readerSLOR);
                                            readerSLOR.Close();
                                        }
                                    }
                                    #endregion

                                    #region Subledger Cheques
                                    if (oSLCheques.Count > 0)
                                    {
                                        foreach (VoucherCheque oVC in oSLCheques)
                                        {
                                            IDataReader readerSLVC;
                                            oVC.VoucherDetailID = nVoucherDetailID;
                                            oVC.CCTID = nCCTID;
                                            readerSLVC = VoucherChequeDA.InsertUpdate(tc, oVC, EnumDBOperation.Insert, nUserId);
                                            NullHandler oReaderVBT = new NullHandler(readerSLVC);
                                            readerSLVC.Close();
                                        }
                                    }
                                    #endregion
                                }
                                //Update Voucher Amount as CostCenter Transaction Amount if one Voucher Detail has only one Subledger
                                if (oCostCenterTransactions.Count == 1 && nCCTID > 0 && nVoucherDetailID > 0)
                                {
                                    CostCenterTransaction oCostCenterTransaction = new CostCenterTransaction();
                                    oCostCenterTransaction.CCTID = nCCTID;
                                    oCostCenterTransaction.VoucherDetailID = nVoucherDetailID;
                                    CostCenterTransactionDA.Update(tc, oCostCenterTransaction, EnumDBOperation.Update, nUserId);
                                }
                            }
                            #endregion
                            #endregion

                            #region Voucher With VoucherBillTransactions
                            #region Delete VoucherBillTransactions
                            if (bIsUpdate)
                            {
                                VoucherBillTransaction oVoucherBillTr = new VoucherBillTransaction();
                                oVoucherBillTr.VoucherDetailID = nVoucherDetailID;
                                VoucherBillTransactionDA.Delete(tc, oVoucherBillTr, EnumDBOperation.Delete, nUserId);
                            }
                            #endregion

                            #region VoucherBillTransactions
                            if (VoucherBillTrs.Count > 0)
                            {
                                foreach (VoucherBillTransaction oVBT in VoucherBillTrs)
                                {
                                    IDataReader readerVBT;
                                    oVBT.VoucherDetailID = nVoucherDetailID;
                                    oVBT.CCTID = 0;
                                    oVBT.CurrencyID = nCurrencyID;
                                    oVBT.ConversionRate = nConversionRate;
                                    readerVBT = VoucherBillTransactionDA.InsertUpdate(tc, oVBT, EnumDBOperation.Insert, nUserId);
                                    NullHandler oReaderVBT = new NullHandler(readerVBT);
                                    readerVBT.Close();
                                }
                            }
                            #endregion
                            #endregion

                            #region VoucherChequeReference
                            #region Delete VoucherChequeReferences Transaction
                            if (bIsUpdate)
                            {
                                VoucherCheque oVoucherCheque = new VoucherCheque();
                                oVoucherCheque.VoucherDetailID = nVoucherDetailID;
                                VoucherChequeDA.Delete(tc, oVoucherCheque, EnumDBOperation.Delete, nUserId);
                            }
                            #endregion

                            #region VoucherChequeReferences
                            if (oVoucherCheques.Count > 0)
                            {
                                foreach (VoucherCheque oVC in oVoucherCheques)
                                {
                                    IDataReader readerVC;
                                    oVC.VoucherDetailID = nVoucherDetailID;
                                    readerVC = VoucherChequeDA.InsertUpdate(tc, oVC, EnumDBOperation.Insert, nUserId);
                                    NullHandler oReaderCCT = new NullHandler(readerVC);
                                    readerVC.Close();
                                }
                            }
                            #endregion
                            #endregion

                            #region VProduct Transaction
                            #region Delete VPTransaction Transaction
                            if (bIsUpdate)
                            {
                                VPTransaction oVPTransaction = new VPTransaction();
                                oVPTransaction.VoucherDetailID = nVoucherDetailID;
                                VPTransactionDA.Delete(tc, oVPTransaction, EnumDBOperation.Delete, nUserId);
                            }
                            #endregion

                            #region VPTransactions
                            if (oVPTransactions.Count > 0)
                            {
                                foreach (VPTransaction oVPT in oVPTransactions)
                                {
                                    IDataReader readerVPTT;
                                    oVPT.VoucherDetailID = nVoucherDetailID;
                                    oVPT.CurrencyID = nCurrencyID;
                                    oVPT.ConversionRate = nConversionRate;
                                    readerVPTT = VPTransactionDA.InsertUpdate(tc, oVPT, EnumDBOperation.Insert, nUserId);
                                    NullHandler oReaderVPT = new NullHandler(readerVPTT);
                                    readerVPTT.Close();
                                }
                            }
                            #endregion
                            #endregion

                            #region VOReferences
                            #region Delete VOReferences Transaction
                            if (bIsUpdate)
                            {
                                VOReference oVOReference = new VOReference();
                                oVOReference.VoucherDetailID = nVoucherDetailID;
                                VOReferenceDA.Delete(tc, oVOReference, EnumDBOperation.Delete, nUserId);
                            }
                            #endregion

                            #region VOReferences
                            if (oVOReferences.Count > 0)
                            {
                                foreach (VOReference oVOReference in oVOReferences)
                                {
                                    IDataReader readerOReference;
                                    oVOReference.VoucherDetailID = nVoucherDetailID;
                                    oVOReference.CurrencyID = nCurrencyID;
                                    oVOReference.ConversionRate = nConversionRate;
                                    oVOReference.Amount = (oVOReference.AmountInCurrency * nConversionRate);
                                    readerOReference = VOReferenceDA.InsertUpdate(tc, oVOReference, EnumDBOperation.Insert, nUserId);
                                    NullHandler oReaderOrderReference = new NullHandler(readerOReference);
                                    readerOReference.Close();
                                }
                            }
                            #endregion
                            #endregion
                        }
                        if (sVoucherDetailIDs.Length > 0)
                        {
                            sVoucherDetailIDs = sVoucherDetailIDs.Remove(sVoucherDetailIDs.Length - 1, 1);
                        }

                        VoucherDetail oVoucherDetail = new VoucherDetail();
                        oVoucherDetail.VoucherID = oVoucher.VoucherID;
                        VoucherDetailDA.Delete(tc, oVoucherDetail, EnumDBOperation.Delete, sVoucherDetailIDs);
                    }
                    #endregion

                    #region VoucherMapping
                    IDataReader mappingreader;
                    oVoucherMapping.VoucherID = oVoucher.VoucherID;

                    mappingreader = VoucherMappingDA.InsertUpdate(tc, oVoucherMapping, EnumDBOperation.Insert, nUserId);
                    mappingreader.Close();
                    #endregion

                    #region Approved & Get Voucher
                    IDataReader readerVoucher;
                    oVoucher.AuthorizedBy = (int)nUserId;
                    oVoucher.Narration = oVoucher.VoucherID.ToString();
                    readerVoucher = VoucherDA.ApprovedVoucher(tc, oVoucher, true);
                    NullHandler oReaderVoucher = new NullHandler(readerVoucher);
                    if (readerVoucher.Read())
                    {
                        oVoucher = new Voucher();
                        oVoucher = CreateObject(oReaderVoucher);
                    }
                    readerVoucher.Close();
                    #endregion

                    oVouchers.Add(oVoucher);
                }
                if (!bUsetc) { tc.End(); }
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                oVoucher = new Voucher();
                oVoucher.ErrorMessage = e.Message.Split('~')[0];
                oVouchers = new List<Voucher>();
                oVouchers.Add(oVoucher);
                #endregion
            }
            return oVouchers;
        }
        public double GetDebitCreditAmount(List<VoucherDetail> oVoucherDetails, bool bIsDebit)
        {
            double nAmount = 0.00;
            foreach (VoucherDetail oVoucherDetail in oVoucherDetails)
            {
                if (oVoucherDetail.IsDebit == bIsDebit)
                {
                    nAmount = nAmount + oVoucherDetail.Amount;
                }
            }
            return nAmount;
        }
        #endregion
        #endregion
    }   
}